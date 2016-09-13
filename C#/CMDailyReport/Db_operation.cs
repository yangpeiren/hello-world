using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace CMDailyReport
{

    class Db_operation
    {
        private static string constring = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source = " + getpath();
        public static OleDbConnection con = new OleDbConnection(constring);
        public static string getpath()
        {
            string str;
            IniFile ini = new IniFile(System.Windows.Forms.Application.StartupPath + @"\Config.ini");
            str = ini.ReadString("config", "DBPath");
            return str;
        }
    }
}
