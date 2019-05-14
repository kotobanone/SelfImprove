using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CAA_EncryptTool
{
    class Encrypt_CAA
    {
        [DllImport("Encrypt_CAA.dll")]
        public extern static void encrypt(char[] inputStr,StringBuilder outStr);
        [DllImport("Encrypt_CAA.dll")]
        public extern static void decrypt(char[] inputStr, StringBuilder outStr);
    }
}
