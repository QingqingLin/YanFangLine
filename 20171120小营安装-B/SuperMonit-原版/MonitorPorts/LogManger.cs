using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MonitorPorts
{
    class LogManager
    {
        /// <summary>
        /// 重要信息写入日志
        /// </summary>
        /// <param name="logs">日志列表，每条日志占一行</param>
        public static void WriteProgramLog(string logs)
        {
            string LogAddress = Environment.CurrentDirectory + "\\Log.log";
            StreamWriter sw = new StreamWriter(LogAddress, true);
            sw.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString(), logs));
            sw.Close();
        }
    }
}
