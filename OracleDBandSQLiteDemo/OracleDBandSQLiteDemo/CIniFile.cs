using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
namespace OracleDBandSQLiteDemo
{
    #region Ini文件操作
    public class CIniFile
    {
        //文件ini名稱  
        public string Path;

        //聲明讀寫ini文件的api函數  
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //class的構造函數，傳遞ini文件名  
        public CIniFile(string inipath)
        {
            Path = inipath;
        }

        //寫ini文件 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.Path);
        }

        //讀ini文件
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.Path);
            return temp.ToString();
        }
        /**/
        /// <summary>
        /// 驗證文件是否存在
        /// </summary>
        public bool ExistINIFile()
        {
            return File.Exists(this.Path);
        }
    }
    #endregion
}
