using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WaitHandleTest
{
    class Program
    {
        static WaitHandle[] waitHandles = null;
        static object _lock = new object();
        static bool _isTimeOut = false;
        static void Main(string[] args)
        {
            DateTime dt = DateTime.Now;
            Console.WriteLine("進入主執行緒");
            //建立集合
            List<Calculator> calculator = new List<Calculator>() 
            {
                new Calculator{Result=0,WaitHandle=new AutoResetEvent(false),Name="NO.1"},
                new Calculator{Result=0,WaitHandle=new AutoResetEvent(false),Name="NO.2"}
            };
            //建立WaitHandle陣列，因為WaitHandle.WaitAll只收陣列
            waitHandles = new WaitHandle[calculator.Count];
            for (int i = 0; i < calculator.Count; i++)
            {
                waitHandles[i] = calculator[i].WaitHandle;
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask), calculator[0]);
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask), calculator[1]);
            //等待這兩隻執行緒完成工作
            WaitHandle.WaitAll(waitHandles,2000);
            _isTimeOut = true;
            Console.WriteLine("子執行緒完成，花費時間 ={0})", (DateTime.Now - dt).TotalMilliseconds);

            Console.ReadKey();
        }

        static void DoTask(Object state)
        {
            lock (_lock)
            {
                if (_isTimeOut)
                    return;
                Calculator calculator = (Calculator)state;
                Console.WriteLine("{0} 進入子執行緒", calculator.Name);

                AutoResetEvent reset = calculator.WaitHandle;

                //for (long i = 0; i < 1000000000; i++)
                //{
                //    calculator.Result++;
                //}

                for (long i = 0; i < 1000000000; i++)
                {
                    //離開子執行緒旗標 
                    if (_isTimeOut)
                    {
                        Console.WriteLine("{0} 計算結果 :{1}", calculator.Name, calculator.Result.ToString());
                        Console.WriteLine("{0} 離開子執行緒", calculator.Name);
                        return;
                    }
                     calculator.Result++;
                }


                Console.WriteLine("{0} 計算結果 :{1}", calculator.Name, calculator.Result.ToString());
                Console.WriteLine("{0} 離開子執行緒", calculator.Name);
                reset.Set();
            }
        } 
    }

    public class Calculator
    {
        public string Name { get; set; }
        public long Result { get; set; }

        private AutoResetEvent _WaitHandle;
        public AutoResetEvent WaitHandle
        {
            get { return _WaitHandle; }
            set { _WaitHandle = value; }
        }
    }
}
