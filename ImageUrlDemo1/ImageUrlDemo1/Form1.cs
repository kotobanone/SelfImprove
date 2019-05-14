using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

using System.Drawing;

namespace ImageUrlDemo1
{
    public partial class Form1 : Form
    {
        static SQLiteHelper sh = null;
        static string IP_Address = "";
        static string MAC_Address = "";
        public Form1()
        {
            InitializeComponent();
            string iUrl = "http://112.efoxconn.com/Images/liantong.gif";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(iUrl);
            WebResponse response  = request.GetResponse();
            Image image = Image.FromStream(response.GetResponseStream());

            pictureBox1.Image = image;

            //SQLite DataBase
            string currentDic = System.IO.Directory.GetCurrentDirectory();
            string sqliteDBPath = currentDic + "\\localSQLite.db";
            if (!File.Exists(sqliteDBPath))
            {
                SQLiteConnection.CreateFile(sqliteDBPath);
            }
            config.SQLiteDatabaseFile = sqliteDBPath;
            //表一:可刷卡的人員權限表
            SQLiteTable tbe = new SQLiteTable("EmpList");
            tbe.Columns.Add(new SQLiteColumn("MACH_ID"));
            tbe.Columns.Add(new SQLiteColumn("EMP_NO", ColType.Text, false, false, false, true));
            tbe.Columns.Add(new SQLiteColumn("CHECK_TYPE"));  //檢查類型
            tbe.Columns.Add(new SQLiteColumn("EFFECTFLAG", ColType.Text, false, false, false, false, "Y"));   //有效否, 預設"Y"
            tbe.Columns.Add(new SQLiteColumn("DELETEFLAG", ColType.Text, false, false, false, false, "N"));  //刪除否,預設"N"
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
                    //sh.CreateTable(tbe); //建立、開啟刷卡權限表
                    //sh.CreateTable(tbr); //建立、開啟刷卡記錄表

                    string sql = "select * from EmpList";

                    DataTable dt = sh.Select(sql);
                    dataGridView1.DataSource = dt;
                }
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 20;
            this.Invoke(new MethodInvoker(() => { 
                for(int i=0;i<20;i++)
                {
                    progressBar1.Increment(1);
                    Thread.Sleep(200);
                }
                if(progressBar1.Value==progressBar1.Maximum)
                {
                    string updateMsg = string.Format("Update Complete! Total Update Data Num is {0}",progressBar1.Value);
                    label1.Text = updateMsg;
                }

            }));
        }
    }
}
