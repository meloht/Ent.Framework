using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ent.Framework.Utils
{
    public class CryptUtil
    {
        public static string EncryptDes(string data, string key)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                //rgbIV与rgbKey可以不一样，这里只是为了简便，可以自行修改
                byte[] rgbIV = Encoding.UTF8.GetBytes(key.Substring(0, 8));

                byte[] inputByteArray = Encoding.UTF8.GetBytes(data);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();

                dCSP.Mode = CipherMode.ECB;

                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cStream.FlushFinalBlock();

                    }
                    return Convert.ToBase64String(mStream.ToArray());
                }
            }
            catch
            {
                return String.Empty;
            }
        }
        public static string DecryptDes(string encryptedString, string key)
        {
            byte[] btKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));

            byte[] btIV = Encoding.UTF8.GetBytes(key.Substring(0, 8));

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            des.Mode = CipherMode.ECB;
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Convert.FromBase64String(encryptedString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);

                        cs.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }
        }

        public static string MD5(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            foreach (var item in result)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }



        #region aes实现

        /// <summary>
        /// Aes加解密钥必须32位
        /// </summary>
        //public static string AesKey = "asekey32w";
        /// <summary>
        /// 获取Aes32位密钥
        /// </summary>
        /// <param name="key">Aes密钥字符串</param>
        /// <returns>Aes32位密钥</returns>
        static byte[] GetAesKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "Aes密钥不能为空");
            }
            if (key.Length < 32)
            {
                // 不足32补全
                key = key.PadRight(32, '0');
            }
            if (key.Length > 32)
            {
                key = key.Substring(0, 32);
            }
            return Encoding.UTF8.GetBytes(key);
        }

        public static string GetAesKeyFromString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "Aes密钥不能为空");
            }
            if (key.Length < 32)
            {
                // 不足32补全
                key = key.PadRight(32, '0');
            }
            if (key.Length > 32)
            {
                key = key.Substring(0, 32);
            }
            return key;
        }

        /// <summary>
        /// Aes加密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">aes密钥，长度必须32位</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptAes(string source, string key)
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = GetAesKey(key);
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor())
                {
                    byte[] inputBuffers = Encoding.UTF8.GetBytes(source);
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    aesProvider.Dispose();
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }
        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">aes密钥，长度必须32位</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptAes(string source, string key)
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = GetAesKey(key);
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                {
                    byte[] inputBuffers = Convert.FromBase64String(source);
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return Encoding.UTF8.GetString(results);
                }
            }


        }




        public static string AesEncrypt(string plainText, string key)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            byte[] encrypted;
            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = GetAesKey(key);
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor();

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return StringUtils.byteToHexStr(encrypted);

        }

        public static string AesDecrypt(string cipherText, string key)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");


            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = GetAesKey(key);
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor();

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(StringUtils.strToToHexByte(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }


        #endregion
    }
}
