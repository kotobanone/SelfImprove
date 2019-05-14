using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace ImageUrlDemo1
{
    class config
    {
        public static string SQLiteDatabaseFile = "";
        private static string SQLitePW = "pdss";
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
