using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using log4net;
[assembly:log4net.Config.XmlConfigurator(Watch=true)]
namespace OracleDBandSQLiteDemo
{
    class Program
    {
        static SQLiteHelper sh = null;
        static string IP_Address = "";
        static string MAC_Address = "";
        static void Main(string[] args)
        {
            string currentDic = System.IO.Directory.GetCurrentDirectory();
            string sqliteDBPath = currentDic + "\\localSQLite.db";
            if (!File.Exists(sqliteDBPath))
            {
                SQLiteConnection.CreateFile(sqliteDBPath);
            }
            config.SQLiteDatabaseFile = sqliteDBPath;
            IniReadAllInfo();
            //表一:可刷卡的人員權限表
            SQLiteTable tbe = new SQLiteTable("EmpList");
            tbe.Columns.Add(new SQLiteColumn("MACH_ID"));
            tbe.Columns.Add(new SQLiteColumn("EMP_NO",ColType.Text,false,false,false,true));
            tbe.Columns.Add(new SQLiteColumn("CHECK_TYPE"));  //檢查類型
            tbe.Columns.Add(new SQLiteColumn("EFFECTFLAG", ColType.Text, false, false, false,false ,"Y"));   //有效否, 預設"Y"
            tbe.Columns.Add(new SQLiteColumn("DELETEFLAG", ColType.Text, false, false, false,false, "N"));  //刪除否,預設"N"
            tbe.Columns.Add(new SQLiteColumn("DELETESERIAL", ColType.Integer));
            //表二:刷卡測試記錄
            SQLiteTable tbr = new SQLiteTable("TestRecoards");
            tbr.Columns.Add(new SQLiteColumn("UID", true));
            //devID
            tbr.Columns.Add(new SQLiteColumn("DEVID"));
            //cardID
            tbr.Columns.Add(new SQLiteColumn("CARDID"));
            //testType
            tbr.Columns.Add(new SQLiteColumn("TEST_TYPE"));
            //result:OK,請重新測試,無權限測試
            tbr.Columns.Add(new SQLiteColumn("RESULT"));
            //handValue
            tbr.Columns.Add(new SQLiteColumn("HANDVALUE"));
            //LF_Value
            tbr.Columns.Add(new SQLiteColumn("LF_VALUE"));
            //RF_Value
            tbr.Columns.Add(new SQLiteColumn("RF_VALUE"));
            //Test_Date
            tbr.Columns.Add(new SQLiteColumn("TEST_DATE"));

            using (SQLiteConnection conn = new SQLiteConnection(config.SQLiteSource))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    sh = new SQLiteHelper(cmd);
                    sh.CreateTable(tbe); //建立、開啟刷卡權限表
                    sh.CreateTable(tbr); //建立、開啟刷卡記錄表
                    //InsertFakeData();

                    GetLocalMacandIP();
                    string IP_Addr_Info = "IP Address = " + IP_Address;
                    Console.WriteLine(IP_Addr_Info);
                    string MAC_Info = "MAC = " + MAC_Address;
                    Console.WriteLine(MAC_Info);

                    //string currentDic = System.IO.Directory.GetCurrentDirectory();
                    string IniPath = currentDic + "\\SYS.ini";
                    CIniFile iniHelper = new CIniFile(IniPath);
                    //序列號讀出後+1
                    string lastUpdateSN = iniHelper.IniReadValue("SYNCINFO", "LAST_UPDATE_SN");
                    Console.WriteLine("LastUpdateSN:"+lastUpdateSN);
                    int serialNo =int.Parse(lastUpdateSN);
                    serialNo += 1;
                    iniHelper.IniWriteValue("SYNCINFO", "LAST_UPDATE_SN", serialNo.ToString());//序列號寫回
                    //連接Oracle數據庫-------------------------------------------------------------------------------------
                    COraDb g_condb = null;
                    string hostaddress = "10.195.211.101";
                    string dbname = "ZZDRI";
                    string uid = "ESD_YT";
                    //密碼要用DES寫入ini文件
                    string pwd = "ESD_YT";
                    //LogHelper.WriteLog("寫log測試~~");
                    //LogHelper.WriteDebug("寫debug測試!");
                    ILog _logger = LogManager.GetLogger("AppLogger.cs");
                    _logger.Info("Error Info Test!");

                    try
                    {
                        //LOG TEST
                        string currentLine=new StackFrame(true).GetFileLineNumber().ToString();
                        string currentClass = new StackFrame(true).GetFileName().ToString();
                        sLog.WriteLog("Write SLOG Test!  "+currentLine+"  "+currentClass);

                        //LOG TEST
                        bool conflag = true;
                        g_condb = new COraDb(hostaddress,dbname, uid, pwd);
                        conflag = g_condb.Exists();
                        if (conflag == false)
                        {
                            //連接數據庫失敗
                            Console.WriteLine("Cannot connect to DB,Network error");
                            //return;
                        }
                    }
                    catch (Exception econdb)
                    {
                        //連接數據庫失敗
                        Console.WriteLine("Cannot connect to DB,Network error"+econdb.Message);
                        
                        return;
                    }
                    string orasql = "select * from ESD_VIEW";
                    DataTable dt = g_condb.get_data(orasql);
                    sh.BeginTransaction();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic["MACH_ID"] = dt.Rows[i]["MACH_ID"];
                        dic["EMP_NO"] = dt.Rows[i]["EMP_NO"];
                        dic["CHECK_TYPE"] = dt.Rows[i]["CHECK_TYPE"];
                        dic["EFFECTFLAG"] = dt.Rows[i]["EFFECTFLAG"];
                        dic["DELETEFLAG"] = dt.Rows[i]["DELETEFLAG"];
                        dic["DELETESERIAL"] = 1;
                        //sh.Insert("EmpList", dic);
                        string sql = string.Format("INSERT OR REPLACE INTO EmpList (MACH_ID,EMP_NO,CHECK_TYPE,EFFECTFLAG,DELETEFLAG,DELETESERIAL) values(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",{5}); ",
                                            dt.Rows[i]["MACH_ID"], dt.Rows[i]["EMP_NO"], dt.Rows[i]["CHECK_TYPE"], dt.Rows[i]["EFFECTFLAG"], dt.Rows[i]["DELETEFLAG"], serialNo);

                        sh.Execute(sql);
                        //Console.WriteLine(t);
                    }
                    sh.Commit();
                    //END-連接Oracle數據庫-------------------------------------------------------------------------------------
                   conn.Close();
                }
            }
            CheckEmpList();
            //IniWriteDBInfo();
            //IniWriteBaseInfo();
            Console.WriteLine(sqliteDBPath);
            Console.ReadKey();
        }
       
        bool TestConnection()
        {
            try
            {
                using(SQLiteConnection conn = new SQLiteConnection(config.SQLiteDatabaseFile))
                {
                    conn.Open();
                    conn.Close();
                }
                return true;
            }
            catch(Exception ex)
            {
               Console.WriteLine(ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 在SQLiteDB中填入假資料
        /// </summary>
        static void InsertFakeData()
        {
            sh.BeginTransaction();

            for (int i = 0; i < 5; i++)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                /*
                 * MATCH_ID
                 * EMP_NO
                 * CKECK_TYPE
                 * EFECTFLAG
                 * DELETEFLAG
                 */
                dic["MACH_ID"] = "A123_B456_CDE";
                dic["EMP_NO"] = 15022440+i;
                dic["CHECK_TYPE"] = "1";
                dic["EFFECTFLAG"] = "Y";
                dic["DELETEFLAG"] = "N";
                //sh.Insert("EmpList", dic);
            }

            for (int i = 0; i < 4; i++)
            {
                Dictionary<string, object> dic_r = new Dictionary<string, object>();
                dic_r["DEVID"] = "A123_B456_CDE";
                dic_r["CARDID"] = 15022440 + i;
                dic_r["TEST_TYPE"] = "1";
                dic_r["RESULT"] = "Y";
                dic_r["HANDVALUE"] = 16.31;
                dic_r["LF_VALUE"] = 55.20;
                dic_r["RF_VALUE"] = 0.97;
                dic_r["TEST_DATE"] = "2018-01-26 10:38:12";
                //sh.Insert("TestRecoards", dic_r);
            }

                sh.Commit(); 
        }

        /// <summary>
        /// 取得本地IP與MAC地址
        /// </summary>
        static void GetLocalMacandIP()
        {
            IP_Address = System.Net.Dns.Resolve(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();  //已過時

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach(NetworkInterface adapter in adapters)
            {
                int i = adapter.GetPhysicalAddress().ToString().Length;
                if(adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet && i>0)
                {
                    MAC_Address = adapter.GetPhysicalAddress().ToString();
                    while(i>2)
                    {
                        i = i - 2;
                        MAC_Address = MAC_Address.Insert(i, ":");
                    }
                }
            }
        }

        static void IniWriteDBInfo()
        {
            string currentDic = System.IO.Directory.GetCurrentDirectory();
            string sqliteDBPath = currentDic + "\\SYS.ini";
            CIniFile iniHelper = new CIniFile(sqliteDBPath);
            iniHelper.IniWriteValue("ORACLEDB", "ORA_DB_IP", "10.195.211.101");
            iniHelper.IniWriteValue("ORACLEDB", "ORA_DB_NAME", "ZZDRI");
            iniHelper.IniWriteValue("ORACLEDB", "ORA_DB_UID", "ESD_YT");
            string orapwd = DesCoder.EncryptDES("ESD_YT");
            iniHelper.IniWriteValue("ORACLEDB", "ORA_DB_PWD", orapwd);
        }

        static void IniWriteBaseInfo()
        {
            string currentDic = System.IO.Directory.GetCurrentDirectory();
            string sqliteDBPath = currentDic + "\\SYS.ini";
            CIniFile iniHelper = new CIniFile(sqliteDBPath);
            iniHelper.IniWriteValue("MACHINFO", "MACHID", "ACC01EDD35B74B1D8D24773432514952");
            DateTime currentTime = System.DateTime.Now;
            string curTimeStr = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
            iniHelper.IniWriteValue("SYNCINFO", "LAST_SYNC_DATE", curTimeStr);
            iniHelper.IniWriteValue("SYNCINFO", "LAST_UPDATE_SN", "1");
        }

        static void IniReadAllInfo()
        {
            string currentDic = System.IO.Directory.GetCurrentDirectory();
            string sqliteDBPath = currentDic + "\\SYS.ini";
            CIniFile iniHelper = new CIniFile(sqliteDBPath);
            string s_oraDbIp = iniHelper.IniReadValue("ORACLEDB", "ORA_DB_IP");
            
            string s_oraDbName = iniHelper.IniReadValue("ORACLEDB", "ORA_DB_NAME");
            string s_oraUid = iniHelper.IniReadValue("ORACLEDB", "ORA_DB_UID");
            string s_oraPwd = iniHelper.IniReadValue("ORACLEDB", "ORA_DB_PWD");
            string s_dec_oraPwd = DesCoder.DecryptDES(s_oraPwd);
            string s_mach_id = iniHelper.IniReadValue("MACHINFO", "MACHID");
            string s_last_sync_date = iniHelper.IniReadValue("SYNCINFO", "LAST_SYNC_DATE");
            DateTime lastUpdateTime = Convert.ToDateTime(s_last_sync_date);
            string s_last_update_sn = iniHelper.IniReadValue("SYNCINFO", "LAST_UPDATE_SN");

            Console.WriteLine(s_oraDbIp);
            Console.WriteLine(s_oraDbName);
            Console.WriteLine(s_oraUid);
            Console.WriteLine(s_dec_oraPwd);
            Console.WriteLine(s_mach_id);
            Console.WriteLine(lastUpdateTime.ToString());
            Console.WriteLine(s_last_update_sn);
        }
        public static void CheckEmpList()
        {
            string currentDic = System.IO.Directory.GetCurrentDirectory();
            string sqliteDBPath = currentDic + "\\SYS.ini";
            CIniFile iniHelper = new CIniFile(sqliteDBPath);
            int serialNum = Int32.Parse(iniHelper.IniReadValue("SYNCINFO", "LAST_UPDATE_SN"));
            using (SQLiteConnection conn = new SQLiteConnection(config.SQLiteSource))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    sh = new SQLiteHelper(cmd);
                    int serNo = serialNum - 2;
                    if (serNo >= 0)
                    {
                        string delsql = string.Format("delete from EmpList where DELETESERIAL<{0}", serNo);
                        sh.Execute(delsql);
                        conn.Close();
                    }
                }
            }
        }

    }
}
