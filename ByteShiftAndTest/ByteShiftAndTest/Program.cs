using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteShiftAndTest
{
    class Program
    {
        static void Main(string[] args)
        {
            byte byte_A = 0x0A; //1010
            byte byte_C = 0x0C; //1100
            string testStr1 = ((byte_A >> 1 & 0x01) == 1) ? "右腳測試通過" : "右腳測試失敗";
            string testStr2 = ((byte_A >> 2 & 0x01) == 1) ? "左腳測試通過" : "左腳測試失敗";
            Console.WriteLine(testStr1);
            Console.WriteLine(testStr2);

            string testStr3 = ((byte_C >> 1 & 0x01) == 1) ? "右腳測試通過" : "右腳測試失敗";
            string testStr4 = ((byte_C >> 2 & 0x01) == 1) ? "左腳測試通過" : "左腳測試失敗";
            Console.WriteLine(testStr3);
            Console.WriteLine(testStr4);

            Console.ReadKey();


        }
    }
}
