using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace OracleDBandSQLiteDemo
{
    /// <summary>
    /// 儲存全域的數據庫路徑完整名稱 以及 密碼
    /// 並組成連接字串以供調用
    /// </summary>
    class config
    {
        public static string SQLiteDatabaseFile = "";
        private static string SQLitePW = "1234";
        private static SQLiteConnectionStringBuilder connStr = new SQLiteConnectionStringBuilder();

        public static string SQLiteSource
        {
            get
            {
                connStr.DataSource = SQLiteDatabaseFile;
                connStr.Password = SQLitePW;
                return connStr.ToString();   //;Password={1}
            }
        }
    }
}
