using System;  
using System.Collections.Generic;  
using System.Threading.Tasks;  
using System.Threading;  

namespace MultiTaskAndWait
{
    class Program
    {
        // Main方法不能标记为异步  
        static void Main(string[] args)
        {
            Task<string> t = batchCallTestThread();
            Console.WriteLine(t.Result);
            Console.ReadLine();
        }

        static async Task<string> batchCallTestThread()
        {
            Console.WriteLine("主線程ID: {0}",
                Thread.CurrentThread.ManagedThreadId);
            List<Task<int>> task = new List<Task<int>>();
            for (int index = 2; index >= 0; index--)
            {
                Task<int> t = testThread(index);
                task.Add(t);
            }
            Thread.Sleep(120);
            Console.WriteLine("主線程運算了一些東西");
            for (int index = 0; index < 3; index++)
            {
                await task[index];
                Console.WriteLine("第{0}個線程運算結果為：{1}",
                    index,
                    task[index].Result);
            }
            return "** 主線程結束 ****";
        }

        static async Task<int> testThread(int index)
        {
            string label = "第" + index.ToString("000") + "線程";
            Console.WriteLine("線程{0}加載{1}",
                Thread.CurrentThread.ManagedThreadId,
                label);
            return await Task.Run<int>(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Thread.Sleep(100);
                    Console.WriteLine(label + "運算第{0}步", i);
                }
                return Thread.CurrentThread.ManagedThreadId;
            });
        }
    }  
}


  
