using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace MultithreadedWorkQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkQueue<passRecoard> workQueue = new WorkQueue<passRecoard>();
            //註冊workqueue中的事件
            workQueue.UserWork += new UserWorkEventHandler<passRecoard>(workQueue_UserWork);
            //設定為單線程執行
            workQueue.WorkSequential = true;
            //設定程執行監控
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //ThreadPool.QueueUserWorkItem(o=>{

            List<passRecoard> prList = new List<passRecoard>();
            passRecoard p_R1 = new passRecoard();
            p_R1.cardID = 11;
            p_R1.empName = "張三";
            p_R1.empNo = "F201684";
            p_R1.Time = DateTime.Now.ToString();

            passRecoard p_R2 = new passRecoard();
            p_R2.cardID = 22;
            p_R2.empName = "李四";
            p_R2.empNo = "F201685";
            p_R2.Time = DateTime.Now.ToString();

            passRecoard p_R3 = new passRecoard();
            p_R3.cardID = 33;
            p_R3.empName = "王五";
            p_R3.empNo = "F201686";
            p_R3.Time = DateTime.Now.ToString();

            passRecoard p_R4 = new passRecoard();
            p_R4.cardID = 44;
            p_R4.empName = "趙六";
            p_R4.empNo = "F201687";
            p_R4.Time = DateTime.Now.ToString();
            prList.Add(p_R1);
            prList.Add(p_R2);
            prList.Add(p_R3);
            prList.Add(p_R4);

                foreach(var p in  prList)
                {
                    //在enqueue時，就會執行註冊的工作事件
                    workQueue.EnqueueItem(p);
                }
          //  });
            Console.ReadKey();
            watch.Stop();
            Console.WriteLine("Execute:{0}ms", watch.ElapsedMilliseconds);

            Console.WriteLine(workQueue.IsEmpty());
            Console.ReadKey();

        }
        /// <summary>
        /// 自定義的工作事件
        /// 使用EnqueueEentArgs監聽一有enqueue事件時執行
        /// </summary>
        static void workQueue_UserWork(object sender, WorkQueue<passRecoard>.EnqueueEventArgs e)
        {
            passRecoard nR = e.Item;
           // int n = e.Item;
            // if(n%10000==0)
             {
                 Console.WriteLine(e.Item.cardID.ToString());
                 Console.WriteLine(e.Item.empNo);
                 Console.WriteLine(e.Item.empName);
                 Console.WriteLine(e.Item.Time);
                 Thread.Sleep(1000);
             }
        }
    }    

    class passRecoard
    {
        public int cardID;
        public string empNo;
        public string Time;
        public string empName;
    }
}
