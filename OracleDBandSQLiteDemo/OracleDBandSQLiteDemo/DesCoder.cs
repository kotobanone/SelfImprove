using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace OracleDBandSQLiteDemo
{
    class DesCoder
    {
        //IV:ECARD_LH=[69,67,65,82,68,45,76,72]={0x45,0x43,0x41,0x52,0x44,0x2D,0x4C,0x48};
        private static byte[] IVs = { 0x45, 0x43, 0x41, 0x52, 0x44, 0x2D, 0x4C, 0x48 };
        //KEY:SIDC2018=[83,73,68,67,50,48,49,56]={0x53,0x49,0x44,0x43,0x32,0x30,0x31,0x38};
        private static byte[] Keys = { 0x53, 0x49, 0x44, 0x43, 0x32, 0x30, 0x31, 0x38 };
        /// <summary>
        /// DES加密字串
        /// </summary>
        /// <param name="encryptString">待加密的字串</param>
        /// <param name="encryptKey">8位加密密鑰</param>
        /// <returns>加密成功返回加密後密文字串，失敗返回源字串</returns>
        public static string EncryptDES(string encryptString)
        {
            try
            {
                byte[] rgbKey = Keys; //Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = IVs;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字串</param>
        /// <param name="decryptKey">8位的解密密鑰,和加密密鑰相同</param>
        /// <returns>解密成功返回解密後的字串，失敗返回源字串</returns>
        public static string DecryptDES(string decryptString)
        {
            try
            {
                byte[] rgbKey = Keys; // Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = IVs;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
    }
}
