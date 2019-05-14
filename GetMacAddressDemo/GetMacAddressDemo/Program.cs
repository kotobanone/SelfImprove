using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace GetMacAddressDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> macs = GetMacByIPconfig();
            foreach(string mac in macs)
            {
                Console.WriteLine(mac);
            }
            macs = GetMacByWMI();

           Console.WriteLine(macs[0]);
           macs = GetMacByNetworkInterface();
           Console.WriteLine(macs[0]);
           Console.WriteLine(GetMacBySendApp(localip.ToString()));

           Console.WriteLine(localip.ToString());

            Console.ReadKey();
        }
        /// <summary>
        /// 方法一
        /// 透過ipconfig /all命令讀取mac地址
        /// 會取到兩個字段，且需自行將mac地址字串截出
        /// </summary>
        public static List<string> GetMacByIPconfig()
        {
            List<string> macs = new List<string>();
            ProcessStartInfo startInfo = new ProcessStartInfo("ipconfig", "/all");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            Process p = Process.Start(startInfo);
            StreamReader reader = p.StandardOutput;
            string line = reader.ReadLine();
            while(!reader.EndOfStream)
            {
                if(!string.IsNullOrEmpty(line))
                {
                    line = line.Trim();
                    if(line.StartsWith("Physical Address")|| line.StartsWith("實體位址"))
                    {
                        macs.Add(line);
                    }
                }
                line = reader.ReadLine();
            }
            p.WaitForExit();
            p.Close();
            reader.Close();
            return macs;
        }
        /// <summary>
        /// 方法二
        /// 透過wmi讀取mac地址
        /// 此方法依賴wmi的系統服務，該服務一般不會被關閉; 
        /// 但如果系統服務缺失或出現問題，此方法無法取得mac地址
        /// </summary>
        public static List<string> GetMacByWMI()
        {
            List<string> macs = new List<string>();
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach(ManagementObject mo in moc)
                {
                    if((bool)mo["IPEnabled"])
                    {
                        mac = mo["MacAddress"].ToString();
                        macs.Add(mac);
                    }
                }
                moc = null;
                mc = null;
            }
            catch { }
            return macs;
        }
        /// <summary>
        /// 方法三
        /// 透過networkinterface讀取mac地址
        /// 如果當前網卡是禁用狀態，取不到該網卡的mac地址
        /// 如果當前啟用了多個網卡，最先返回的地址是最近啟用的網路連接信息
        /// </summary>
        public static List<string> GetMacByNetworkInterface()
        {
            List<string> macs = new List<string>();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach(NetworkInterface ni in interfaces)
            {
                macs.Add(ni.GetPhysicalAddress().ToString());
            }
            return macs;
        }
        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string p);
        /// <summary>
        /// 方法四
        /// 透過SendARP讀取mac地址
        /// 網路被禁用或未接入網路時此方法失效
        /// </summary>
        public static string GetMacBySendApp(string remoteIP)
        {
            StringBuilder macAddress = new StringBuilder();
            try
            {
                Int32 remote = inet_addr(remoteIP);
                Int64 macInfo = new Int64();
                Int32 length = 6;
                SendARP(remote, 0, ref macInfo, ref length);
                string temp = Convert.ToString(macInfo, 16).PadLeft(12, '0').ToUpper();
                int x = 12;
                for(int i=0;i<6;i++)
                {
                    if(i==5)
                    {
                        macAddress.Append(temp.Substring(x - 2, 2));
                    }
                    else
                    {
                        macAddress.Append(temp.Substring(x - 2, 2) + ":");
                    }
                    x -= 2;
                }
                return macAddress.ToString();
            }
            catch
            {
                return macAddress.ToString();
            }
        }

        //獲取本機 ipv4 ，要取得ipv6時使用 System.Net.Sockets.AddressFamily.InterNetworkV6
        public static IPAddress localip = Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
    }
}
