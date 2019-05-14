using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace OracleDBandSQLiteDemo
{
    class LogHelper
    {
        private LogHelper()
        { }

        private static readonly ILog _logger = LogManager.GetLogger("LogTrace");

        public static void SetConfig()
        {
            log4net.Config.DOMConfigurator.Configure();
        }
        public static void SetConfig(FileInfo configFile)
        {
            log4net.Config.DOMConfigurator.Configure(configFile);
        }

        public static void WriteLog(string info)
        {
            _logger.Info(info);
        }

        public static void WriteDebug(string info)
        {
            _logger.Debug(info);
        }
    }
}
