using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OracleDBandSQLiteDemo
{
    class sLog
    {
        private static string _logFileName = "Log_" + DateTime.Now.Year.ToString()+DateTime.Now.Month.ToString()+DateTime.Now.Day.ToString();
        private static string _fileDirectory = System.Environment.CurrentDirectory + "//ESDLogs";
        private StreamWriter writer;
        private FileStream fileStream = null;
        private static void checkPath()
        {
            if(!System.IO.Directory.Exists(_fileDirectory))
            {
                System.IO.Directory.CreateDirectory(_fileDirectory);
            }
        }
        private static void checkFile(string fileName)
        {
            System.IO.StreamWriter sw;
            fileName = _fileDirectory + "//" + fileName + ".log";
            if(!System.IO.File.Exists(fileName))
            {
                sw = System.IO.File.CreateText(fileName);
                sw.Close();
            }
        }

        public static void WriteLog(string loginfo)
        {
            checkPath();
            checkFile(_logFileName);
            string fileName = _fileDirectory + "//" + _logFileName + ".log";
            System.IO.FileStream filestream = new System.IO.FileStream(fileName, System.IO.FileMode.Open | System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            System.IO.StreamWriter writer = new System.IO.StreamWriter(filestream, System.Text.Encoding.Default);

            writer.BaseStream.Seek(0, System.IO.SeekOrigin.End);
            writer.WriteLine("{0}--{1}", DateTime.Now.ToString(), loginfo);
            writer.Flush();
            writer.Close();
            filestream.Close();
        }

        public static string LogFileName
        {
            get { return _logFileName; }
            set { _logFileName = value; }
        }

        public static string FileDirectory
        {
            get { return _fileDirectory; }
            set { _fileDirectory = value; }
        }
    }
}
