using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace DesAesEncryptExample
{
    class NetCryptoHelper
    {
        #region Des imprement
        /// <summary>
        /// DES預設密鑰向量
        /// </summary>
        public static byte[] DesIv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        
        /// <summary>
        /// DES加解密鑰需要8位
        /// </summary>
        public const string DesKey = "deskey8w";

        static byte[] GetDesKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "The key of DES cannot be empty!");
            }
            if (key.Length > 8)
            {
                key = key.Substring(0, 8);
            }
            if (key.Length < 8)
            {
                //不足8位用0補齊
                key = key.PadRight(8, '0');
            }
            return Encoding.UTF8.GetBytes(key);
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="source">欲加密的字串</param>
        /// <returns>加密後的字串</returns>
        public static string EncryptDes(string source, string key, byte[] iv)
        {
            using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
            {
                byte[] rgbKeys = GetDesKey(key);
                byte[] rgbIvs = iv;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(source);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desProvider.CreateEncryptor(rgbKeys, rgbIvs), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cryptoStream.FlushFinalBlock();
                        //第一種
                        return Convert.ToBase64String(memoryStream.ToArray());
                        //第二種
                        //StringBuilder result = new StringBuilder();
                        //foreach (byte b in memoryStream.ToArray())
                        //{
                        //    result.AppendFormat("{0:X2}",b);
                        //}
                        //return result.ToString();
                    }
                }
            }
        }

        public static string DecryptDes(string source, string key, byte[] iv)
        {
            using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
            {
                byte[] rgbKeys = GetDesKey(key);
                byte[] rgbIvs = iv;
                byte[] inputByteArray = Convert.FromBase64String(source);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desProvider.CreateDecryptor(rgbKeys, rgbIvs), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cryptoStream.FlushFinalBlock();
                        return Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
            }
        }

        #endregion

        #region aes實現
        public static string AesKey = "aeskey32w";

        static byte[] GetAesKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "The key of DES cannot be empty!");
            }
            if (key.Length > 32)
            {
                key = key.Substring(0, 32);
            }
            if (key.Length < 32)
            {
                key = key.PadRight(32, '0');
            }
            return Encoding.UTF8.GetBytes(key);
        }
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="soruce">欲加密的字串</param>
        /// <param name="key">aes密鑰</param>
        /// <returns></returns>
        public static string EncryptAes(string soruce, string key)
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = GetAesKey(key);
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform cryptoTransfrom = aesProvider.CreateEncryptor())
                {
                    byte[] inputBuffers = Encoding.UTF8.GetBytes(soruce);
                    byte[] result = cryptoTransfrom.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    aesProvider.Dispose();
                    return Convert.ToBase64String(result, 0, result.Length);
                }
            }
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="source">欲解密的字串</param>
        /// <param name="key">aes密鑰，長度必需32位</param>
        /// <returns>解密後的字串</returns>
        public static string DecryptAes(string source, string key)
        {
            using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = GetAesKey(key);
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;
                using(ICryptoTransform cryptoTransform=aesProvider.CreateDecryptor())
                {
                    byte[] inputBuffers = Convert.FromBase64String(source);
                    byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    aesProvider.Dispose();
                    return Encoding.UTF8.GetString(results);
                }
            }

        }

        #endregion
    }
}
