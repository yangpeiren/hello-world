using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace CMDailyReport
{
    public class Log
    {
        public static string LogPath = System.Windows.Forms.Application.StartupPath + @"\CM_DailyRoport.Log";
        public static DateTime time = System.DateTime.Now;
        public static string PCName = Dns.GetHostName();
        public static string username = System.Environment.UserName;
        public static string Ipaddr = GetIp();

        public static void DBInsert()
        {
            WriteLog("DataBase Insert Operation proceed.");
        }
        public static void ExePossess()
        {
            WriteLog("Process CM_DailyReport Running.");
        }
        public static void ExePossessEnd()
        {
            WriteLog("Process CM_DailyReport End.");
        }
        public static void FilePossess(string filename)
        {
            WriteLog(filename + " Last Opened by " + PCName + ".");
        }

        private static void WriteLog(string str)
        {
            
            StreamWriter sw = null;
            FileStream myFs = null;
            try
            {
                sw = new StreamWriter(LogPath, true);
                if (sw == null)
                {
                    myFs = new FileStream(LogPath, FileMode.Create);
                    sw = new StreamWriter(myFs);
                }
                sw.WriteLine(time.ToString() + " ; " + PCName + " ; " + username + " ; " + Ipaddr + " ; " + str);
            }
            catch(Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.Message, "error!");
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        public static string GetIp()
        {
            IPAddress[] IP = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ipa in IP)
            {
                if(ipa.ToString().Contains("172"))
                    return ipa.ToString();
            }
            return "0.0.0.0";

        }

    }
}
