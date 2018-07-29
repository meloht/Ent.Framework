using Ent.Framework.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Ent.Framework.Utils
{
    public class FtpUtil
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly string _ftpServerIp;
        private readonly string _ftpUserId;
        private readonly string _ftpPassword;
        private readonly int _bufferSize = 4096;

        public FtpUtil(string ftpServerIp, string ftpUserId, string ftpPassword)
        {
            _ftpServerIp = ftpServerIp;
            _ftpPassword = ftpPassword;
            _ftpUserId = ftpUserId;
        }

        private Uri GetFtpUri(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
                return new Uri($"ftp://{_ftpServerIp}");
            if (filePath.StartsWith("/"))
            {
                return new Uri($"ftp://{_ftpServerIp}{filePath}");
            }
            else
            {
                return new Uri($"ftp://{_ftpServerIp}/{filePath}");
            }

        }

        private FtpWebRequest ConnectFtp(string remoteFile)
        {
            Uri uri = GetFtpUri(remoteFile);
            // Create FtpWebRequest object from the Uri provided
            var reqFtp = (FtpWebRequest)WebRequest.Create(uri);

            // Provide the WebPermission Credintials
            reqFtp.Credentials = new NetworkCredential(_ftpUserId, _ftpPassword);

            // By default KeepAlive is true, where the control connection is not closed
            // after a command is executed.
            reqFtp.KeepAlive = true;

            return reqFtp;
        }



        private List<string> GetDirList(string remoteFile)
        {
            List<string> list = new List<string>();
            string directoryName = Path.GetDirectoryName(remoteFile);
            if (directoryName != null)
            {
                string remoteFolder = directoryName.Replace("\\", "/");
                string[] directory = remoteFolder.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                return directory.ToList();
            }
            return list;
        }

        private void CheckPathDir(string remoteFile)
        {
            List<string> list = GetDirList(remoteFile);
            if (list.Count > 0)
            {
                string lastDirectory = "";
                foreach (string dir in list)
                {
                    if (lastDirectory.Trim() != String.Empty)
                    {
                        lastDirectory += "/" + dir;
                    }
                    else
                    {
                        lastDirectory = dir;
                    }

                    if (!IsDirExists(lastDirectory))
                    {
                        CreateDir(lastDirectory);
                    }
                }
            }
        }


        private void ExecuteFtpCmd(FtpWebRequest reqFtp)
        {
            using (FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse())
            using (Stream ftpStream = response.GetResponseStream())
            {
                if (ftpStream != null) ftpStream.Close();
                response.Close();
            }
        }

        public bool Upload(string remoteFile, string localFile)
        {
            CheckPathDir(remoteFile);
            FileInfo fileInf = new FileInfo(localFile);

            FtpWebRequest reqFtp = ConnectFtp(remoteFile);

            // Specify the command to be executed.
            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;

            // Specify the data transfer type.
            reqFtp.UseBinary = true;

            // Notify the server about the size of the uploaded file
            reqFtp.ContentLength = fileInf.Length;

            // The buffer size is set to 2kb
            int buffLength = _bufferSize;
            byte[] buff = new byte[buffLength];
            int contentLen;

            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
            try
            {
                using (FileStream fs = fileInf.OpenRead())
                {
                    // Stream to which the file to be upload is written
                    using (Stream strm = reqFtp.GetRequestStream())
                    {
                        // Read from the file stream 2kb at a time
                        contentLen = fs.Read(buff, 0, buffLength);

                        // Till Stream content ends
                        while (contentLen != 0)
                        {
                            // Write Content from the file stream to the FTP Upload Stream
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }

                        // Close the file stream and the Request Stream
                        strm.Close();
                        fs.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message,ex);
            }
            return false;
        }
        public bool Upload(string remoteFile, Stream inputStream)
        {
            CheckPathDir(remoteFile);
            FtpWebRequest reqFtp = ConnectFtp(remoteFile);

            // Specify the command to be executed.
            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;

            // Specify the data transfer type.
            reqFtp.UseBinary = true;
            reqFtp.KeepAlive = false;
            // Notify the server about the size of the uploaded file
            reqFtp.ContentLength = inputStream.Length;

            // The buffer size is set to 2kb
            int buffLength = _bufferSize;
            byte[] buff = new byte[buffLength];
            int contentLen;

            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
            try
            {
                using (inputStream)
                // Stream to which the file to be upload is written
                using (Stream strm = reqFtp.GetRequestStream())
                {
                    // Read from the file stream 2kb at a time
                    contentLen = inputStream.Read(buff, 0, buffLength);

                    // Till Stream content ends
                    while (contentLen != 0)
                    {
                        // Write Content from the file stream to the FTP Upload Stream
                        strm.Write(buff, 0, contentLen);
                        contentLen = inputStream.Read(buff, 0, buffLength);
                    }

                    // Close the file stream and the Request Stream
                    strm.Close();
                    inputStream.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
            }
            return false;
        }

        public bool DeleteFile(string remoteFile)
        {
            try
            {
                FtpWebRequest reqFtp = ConnectFtp(remoteFile);

                reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;

                ExecuteFtpCmd(reqFtp);

                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
            }
            return false;
        }

        public List<string> GetFilesDetailList(string directory)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp = ConnectFtp(directory);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                reader.Close();
                response.Close();

                if (string.IsNullOrEmpty(result.ToString()))
                    return new List<string>();
                result.Remove(result.ToString().LastIndexOf("\n"), 1);

                return result.ToString().Split('\n').ToList();
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                return null;
            }
        }


        public bool Download(string filePath, Stream outputStream)
        {
            try
            {
                FtpWebRequest ftp = ConnectFtp(filePath);
                ftp.Method = WebRequestMethods.Ftp.DownloadFile;
                ftp.UseBinary = true;

                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                using (response)
                using (ftpStream)
                {
                    int bufferSize = _bufferSize;
                    int readCount;
                    byte[] buffer = new byte[bufferSize];

                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        outputStream.Write(buffer, 0, readCount);
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                    }

                    ftpStream.Close();

                    response.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
            }
            return false;
        }

        public Stream Download(string filePath)
        {
            try
            {
                FtpWebRequest ftp = ConnectFtp(filePath);
                ftp.Method = WebRequestMethods.Ftp.DownloadFile;
                ftp.UseBinary = true;

                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                return ftpStream;

            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                return Stream.Null;
            }

        }


        public bool Download(string remoteFile, string localFile)
        {
            try
            {
                FtpWebRequest reqFtp = ConnectFtp(remoteFile);

                reqFtp.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFtp.UseBinary = true;

                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = _bufferSize;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                FileStream outputStream = new FileStream(localFile, FileMode.Create);
                using (response)
                using (outputStream)
                using (ftpStream)
                {
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        outputStream.Write(buffer, 0, readCount);
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                    }

                    ftpStream.Close();
                    outputStream.Close();
                    response.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                return false;

            }
        }

        public long GetFileSize(string filename)
        {
            long fileSize = 0;
            try
            {
                FtpWebRequest reqFtp = ConnectFtp(filename);
                reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFtp.UseBinary = true;

                using (FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse())
                using (Stream ftpStream = response.GetResponseStream())
                {
                    fileSize = response.ContentLength;

                    if (ftpStream != null) ftpStream.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
            }
            return fileSize;
        }

        public bool ReName(string currentFilename, string newFilename)
        {
            try
            {
                FtpWebRequest reqFtp = ConnectFtp(currentFilename);
                reqFtp.Method = WebRequestMethods.Ftp.Rename;
                reqFtp.RenameTo = newFilename;
                reqFtp.UseBinary = true;

                ExecuteFtpCmd(reqFtp);
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
            }
            return false;
        }

        public bool CreateDir(string dirName)
        {
            try
            {
                // dirName = name of the directory to create.
                FtpWebRequest reqFtp = ConnectFtp(dirName);
                reqFtp.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFtp.UseBinary = true;

                ExecuteFtpCmd(reqFtp);
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
            }
            return false;
        }

        public bool RemoveDir(string dirName)
        {
            try
            {
                FtpWebRequest reqFtp = ConnectFtp(dirName);
                reqFtp.Method = WebRequestMethods.Ftp.RemoveDirectory;
                reqFtp.UseBinary = true;

                ExecuteFtpCmd(reqFtp);
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
            }
            return false;
        }

        public bool IsFileExists(string filename)
        {
            string directoryName = Path.GetDirectoryName(filename);
            if (string.IsNullOrEmpty(directoryName))
            {
                return false;
            }
            directoryName = directoryName.Replace("\\", "/");

            string file = Path.GetFileName(filename);
            List<string> list = GetFileList(directoryName);

            if (list != null && list.Contains(file))
                return true;
            return false;
        }

        public List<string> GetFileList(string dir)
        {
            StreamReader reader = null;
            List<string> list = new List<string>();
            try
            {
                FtpWebRequest reqFtp = ConnectFtp(dir);

                reqFtp.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse listResponse = (FtpWebResponse)reqFtp.GetResponse())
                {
                    var stream = listResponse.GetResponseStream();
                    if (stream != null)
                    {
                        reader = new StreamReader(stream);

                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            list.Add(line);
                            line = reader.ReadLine();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return list;
        }

        public bool IsDirExists(string dir)
        {
            var dirList = GetFileList(dir);
            if (dirList != null && dirList.Count > 0)
                return true;
            return false;
        }
    }
}
