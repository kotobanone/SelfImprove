using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesAesEncryptExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string plainText = "這是一串很長很長很長很長很長很長很長很長但其實不是很敏感的字串:&#$*@)*)^)!\\121\\412&*(%#)@";
            string encryptString = NetCryptoHelper.EncryptDes(plainText, NetCryptoHelper.DesKey, NetCryptoHelper.DesIv);
            Console.WriteLine("DES加密前的字串:{0}", plainText);
            Console.WriteLine("DES加密後的密文:{0}", encryptString);
            Console.WriteLine("DES解密後的明文:{0}", NetCryptoHelper.DecryptDes(encryptString, NetCryptoHelper.DesKey, NetCryptoHelper.DesIv));

            Console.WriteLine("------------分隔線-------------");
            Console.WriteLine("AES加密前的字串:{0}", plainText);
            encryptString = NetCryptoHelper.EncryptAes(plainText, NetCryptoHelper.AesKey);
            Console.WriteLine("AES加密後的密文:{0}", encryptString);
            Console.WriteLine("AES解密後的明文:{0}", NetCryptoHelper.DecryptAes(encryptString, NetCryptoHelper.AesKey));

            Console.ReadKey();
        }
    }
}
