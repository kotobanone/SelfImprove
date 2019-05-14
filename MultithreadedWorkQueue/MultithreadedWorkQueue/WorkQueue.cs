using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultithreadedWorkQueue
{
    public delegate void UserWorkEventHandler<T>(object sender, WorkQueue<T>.EnqueueEventArgs e);
    public class WorkQueue<T>
    {
        private bool IsWorking;  //表明處理線程是否正在工作
        private object lockIsWorking = new object();  //對IsWorking的同步對象
        private Queue<T> queue; //實際的列隊
        private object lockObj = new object();
        /// <summary>
        /// 綁定用戶需要對列隊中的item對象進行的操作事件
        /// </summary>
        public event UserWorkEventHandler<T> UserWork;
        public WorkQueue(int n)
        {
            queue = new Queue<T>(n);
        }
        public WorkQueue()
        {
            queue = new Queue<T>();
        }
        /// <summary>
        /// 謹慎使用此函數，僅保證此瞬間列隊值為空
        /// </summary>
        public bool IsEmpty()
        {
            lock(lockObj)
            { return queue.Count==0; }
        }

        private bool isOneThread;
        /// <summary>
        /// 列隊是否需要單線程順序執行
        /// </summary>
        public bool WorkSequential
        {
            get { return isOneThread; }
            set { isOneThread = value; }
        }
        /// <summary>
        /// 向工作列隊添加對象
        /// 對象添加後，如果已經綁定工作的事件
        /// 會觸發事件處理程序，對item對象進行處理
        /// </summary>
        public void EnqueueItem(T item)
        {
            lock(lockObj)
            {
                try
                {
                    queue.Enqueue(item);
                }
                catch(Exception)
                {
                    throw new OutOfMemoryException("Queue Memory is Overflow!");
                }
            }
            lock(lockIsWorking)
            {
                if(!IsWorking)
                {
                    IsWorking = true;
                    ThreadPool.QueueUserWorkItem(doUserWork);
                }
            }
        }
        /// <summary>
        /// 處理列隊中對象的方法
        /// </summary>
        /// <param name="o"></param>
        private void doUserWork(object o)
        {
            try
            {
                T item;
                while(true)
                {
                    lock(lockObj)
                    {
                        if(queue.Count>0)
                        {
                            item = queue.Dequeue();
                        }
                        else
                        {
                            return;
                        }
                    }

                    if(!item.Equals(default(T)))
                    {
                        if(isOneThread) //單線程執行
                        {
                            if(UserWork!=null)
                            {
                                UserWork(this, new EnqueueEventArgs(item));
                            }
                        }
                        else
                        {
                            ThreadPool.QueueUserWorkItem(obj =>
                                {
                                    if(UserWork!=null)
                                    {
                                        UserWork(this, new EnqueueEventArgs(obj));
                                    }
                                },item);
                        }
                    }

                }
            }
            finally
            {
                lock(lockIsWorking)
                {
                    IsWorking = false;
                }
            }
        }
        /// <summary>
        /// UserWork事件的參數，包含item對象
        /// </summary>
        public class EnqueueEventArgs:EventArgs
        {
            public T Item { get; private set; }
            public EnqueueEventArgs(object item)
            {
                try
                {
                    Item = (T)item;
                }
                catch(Exception)
                {
                    throw new InvalidCastException("Object to T translate failed!");
                }
            }
        }

    }
}
