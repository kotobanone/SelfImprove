using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CAA_EncryptTool
{
    class cshop_Aes
    {
        private static byte[] i_IV = { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 };

        public static string AESEncrypt(string Data, byte[] Key)
        {
            Byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(Data);
            Byte[] pwdBytes = Key;// Encoding.ASCII.GetBytes(Key);
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Key = pwdBytes;
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.IV = i_IV;
            ICryptoTransform cTransform = rijndaelCipher.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray,0,resultArray.Length);
        }

        public static string AESDecrypt(string Data, byte[] Key)
        {
            string retSTR = "Input Text Error!";
            byte[] plainText;
            try
            {
                byte[] pwdBytes = Key;
                byte[] encryptedData = Convert.FromBase64String(Data); 

                Rijndael rijndaelCipher = Rijndael.Create();
                rijndaelCipher.Key = pwdBytes;
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                rijndaelCipher.IV = i_IV;

                ICryptoTransform transfrom = rijndaelCipher.CreateDecryptor();
                plainText = transfrom.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            }
            catch (Exception ex)
            {                
                return retSTR;
            }
            return Encoding.UTF8.GetString(plainText);
            
        }
    }
}
