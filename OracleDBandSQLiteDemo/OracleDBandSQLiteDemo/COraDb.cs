using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace OracleDBandSQLiteDemo
{
    public class COraDb
    {
        private string DB_name = "";
        private string DB_UID = "";
        private string DB_PWD = "";
        private string connectionString = "";

        OracleConnection con = new OracleConnection();
        OracleCommand com = new OracleCommand();
        OracleDataAdapter da = new OracleDataAdapter();

        //產生連接的字符串
        /// <summary>
        /// 龍華開發環境連接測試DB需時約4秒才會有回應
        /// </summary>
        public COraDb(string hostaddress,string data_source, string user_id, string password)
        {
            DB_name = data_source;
            DB_UID = user_id;
            DB_PWD = password;
            //connectionString = "Persist Security Info=True;Unicode=True;Data Source=" + DB_name + ";User ID=" + DB_UID + ";Password=" + DB_PWD + "";
            connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + hostaddress + ")(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=" + data_source + ")));Persist Security Info=True;User ID=" + user_id + ";Password=" + password + ";";
        }

        //判斷數據庫能否連通
        public bool Exists()
        {
            con.Close();
            con.ConnectionString = connectionString;
            try
            {
                con.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;

            }
            con.Close();
            return true;
        }

        //檢測數據庫是否連通
        public void CheckConnection()
        {
            con.Close();
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
        }

        //執行非查詢sql語句
        public int exec_data(string sql)
        {
            int i = -1;
            try
            {
                con.Close();
                com = new OracleCommand();
                con.ConnectionString = connectionString;
                con.Open();
                com.Connection = con;
                com.CommandText = sql;
                com.ExecuteNonQuery();//執行sql
                con.Close();
                com.Dispose();
                con.Dispose();
            }
            catch (Exception e)
            {
                con.Close();
                com.Dispose();
                con.Dispose();
            }
            return i;
        }

        //執行查詢語句
        public DataTable get_data(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Close();
                com = new OracleCommand();
                con.ConnectionString = connectionString;
                con.Open();
                com.Connection = con;
                com.CommandText = sql;
                da.SelectCommand = com;
                da.Fill(dt);
                con.Close();
                com.Dispose();
                con.Dispose();
            }
            catch (Exception e)
            {
                con.Close();
                com.Dispose();
                con.Dispose();
            }
            return dt;
        }

        //執行存儲過程
        public void RunProcedure(string storedProcName, OracleParameter[] parameters)
        {
            con.Close();
            con.ConnectionString = connectionString;
            con.Open();
            com.Connection = con;
            com.CommandText = storedProcName;//聲明存儲過程名
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Clear();
            foreach (OracleParameter parameter in parameters)
            {
                com.Parameters.Add(parameter);
            }
            com.ExecuteNonQuery();//執行存儲過程
            con.Close();
            com.Dispose();
            con.Dispose();
        }


    }
}
