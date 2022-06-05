using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnBoardSDC.SDCTool
{
    class LogHelper
    {
        public static void Save(string sTag, string message)
        {
            string logPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\log\\";
            if (!Directory.Exists(logPath))//Create if not
            {
                Directory.CreateDirectory(logPath);
            }
            using (FileStream stream = new FileStream(logPath + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.OpenOrCreate | FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine(DateTime.Now + " [" + sTag + "] " + message);
            }
        }
    }
}
