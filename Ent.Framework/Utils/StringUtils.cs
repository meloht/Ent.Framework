using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ent.Framework.Utils
{
    public class StringUtils
    {
        private static readonly Random RdRandom = new Random();
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        /// <summary>
        /// 精确到毫秒
        /// </summary>
        /// <returns></returns>
        public static string GenerateTimeString()
        {
            string id = DateTime.Now.ToString("yyyyMMdd-HHmmssfff");
            return id;
        }

        public static string GetGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static int GetRandomIndex(int maxValue)
        {
            return RdRandom.Next(maxValue);
        }

        public static int GetRandomIndex(int minValue, int maxValue)
        {
            return RdRandom.Next(minValue, maxValue);
        }

        /// <summary>
        /// 生成指定长度数字串，不足位数用0补齐
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string NextIntNumberRandom(int length)
        {
            int max = 1;
            for (int i = 0; i < length; i++)
            {
                max = max * 10;
            }
            int num = RdRandom.Next(max);
            if (num.ToString().Length < length)
            {
                return num.ToString().PadLeft(length, '0');
            }
            return num.ToString();
        }

        public static string GetRandomString(int stringlength)
        {
            return GetRandomString(null, stringlength);
        }

        //获得长度为stringlength 的随机字符串,以Keyset为字母表
        public static string GetRandomString(string keySet, int stringlength)
        {
            if (keySet == null || keySet.Length < 8) keySet = "abcdefghijklmnopqrstuvwxyz1234567890";
            int keySetLength = keySet.Length;
            StringBuilder str = new StringBuilder(keySetLength);
            for (int i = 0; i < stringlength; ++i)
            {
                str.Append(keySet[Random(keySetLength)]);
            }
            return str.ToString();
        }

        private static int Random(int maxValue)
        {
            decimal _base = (decimal)long.MaxValue;
            byte[] rndSeries = new byte[8];
            rng.GetBytes(rndSeries);
            return (int)(Math.Abs(BitConverter.ToInt64(rndSeries, 0)) / _base * maxValue);
        }
        /// <summary>
        /// 唯一订单号生成 20位 后三位是数字与字母随机组合
        /// </summary>
        /// <returns></returns>
        public static string GenerateOrderNumber()
        {
            string strDateTimeNumber = GenerateTimeString();
            if (strDateTimeNumber.Length < 17)
            {
                strDateTimeNumber = strDateTimeNumber.PadLeft(17, '0');
            }
            string strRandomResult = GetRandomString(3);
            return String.Format("{0}{1}", strDateTimeNumber, strRandomResult);
        }


        public static string GetFileNameFromUrl(string url)
        {
            string fileName = Path.GetFileName(url);
            return fileName;

        }

        public static string GetFileNameWithoutExt(string fileNameExt)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileNameExt);
            return fileName;
        }

        /// <summary> 
        /// 字符串转16进制字节数组 
        /// </summary> 
        /// <param name="hexString"></param> 
        /// <returns></returns> 
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToHexStr(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("X2"));
                }
            }
            return sb.ToString();
        }

        public static string EnBase64Code(string str)
        {
            System.Text.Encoding encode = System.Text.Encoding.UTF8;
            byte[] bytedata = encode.GetBytes(str);
            string strCode = Convert.ToBase64String(bytedata, 0, bytedata.Length);

            return strCode;
        }

        public static string DeBase64Code(string str)
        {
            byte[] bpath = Convert.FromBase64String(str);
            string strCode = System.Text.Encoding.UTF8.GetString(bpath);
            return strCode;
        }


        public static string[] SplitString(string s)
        {
            List<string> ls = new List<string>();
            if (string.IsNullOrEmpty(s))
                return ls.ToArray();

            return s.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
