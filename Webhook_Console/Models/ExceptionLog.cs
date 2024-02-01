using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webhook_Console.Models
{
    public static class Log
    {

        public static void DataLog(string data, bool isFirst = false)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory; // You get main rott
                string filePath = Path.Combine(path, "Logs", $"{DateTime.Now:yyyy-MM-dd}.txt"); //"LogsFile.txt"

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                }

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    if (isFirst)
                        writer.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
                    else
                        writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine("Date : " + DateTime.Now.ToString());
                    writer.WriteLine();
                    writer.WriteLine(data);

                }
            }
            catch (Exception exx)
            {
                throw new Exception("File log exception: " + exx.Message, exx.InnerException); ;
            }
        }
        public static void ExceptionLog(Exception ex)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory; // You get main rott
                string filePath = Path.Combine(path, "Logs", $"{DateTime.Now:yyyy-MM-dd}.txt"); // $"{DateTime.Now:yyyy-MM-dd}.txt");

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                }

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine("Date : " + DateTime.Now.ToString());
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Message : " + ex.Message);
                        writer.WriteLine("Inner Exception : " + ex.InnerException);
                        writer.WriteLine("StackTrace : " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }
            }
            catch (Exception exx)
            {
                throw new Exception("File log exception: " + exx.Message, exx.InnerException); ;
            }

        }
    }
}