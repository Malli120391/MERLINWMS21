using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventrax_CommonBackGroundService
{
    class LogWriter
    {
        public static void WriteLog(string logText)
        {

            StreamWriter writer = null;
            try
            {
                string EXEFOLDER = AppDomain.CurrentDomain.BaseDirectory;
                //writer = new StreamWriter(@"C:\Users\swamyp\Desktop\log.txt", true);
                writer = new StreamWriter(EXEFOLDER + DateTime.Now.Date.ToLongDateString() + "log.txt", true);
                writer.WriteLine(logText);

                writer.Flush();
                writer.Close();
            }
            catch
            {

            }
        }


        public static void WriteLog(string logText, string filename)
        {

            StreamWriter writer = null;
            try
            {
                string directory = AppDomain.CurrentDomain.BaseDirectory + @"logs";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string logfile = directory + @"\" + filename + ".txt";
                writer = new StreamWriter(logfile, true);

                writer.WriteLine(logText);

                writer.Flush();
                writer.Close();
            }
            catch
            {

            }
        }
    }
}
