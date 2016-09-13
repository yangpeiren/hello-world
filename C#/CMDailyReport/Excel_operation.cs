using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMDailyReport
{
    class Excel_operation
    {
        public static int[] columnumber = getpath();
        public static string TemplatePath = getTemplate();
        public static string ReportPath = getReport();
        public static string EscalationPath = getEscalation();
        private static int[] getpath()
        {
            int[] str = new int[4];
            IniFile ini = new IniFile(System.Windows.Forms.Application.StartupPath + @"\Config.ini");
            str[0] = Convert.ToChar(ini.ReadString("config", "ExcelFL")) - 'A';
            str[1] = Convert.ToChar(ini.ReadString("config", "ExcelDT")) - 'A';
            str[2] = Convert.ToChar(ini.ReadString("config", "ExcelLS")) - 'A';
            str[3] = Convert.ToChar(ini.ReadString("config", "ExcelDP")) - 'A';
            return str;
        }
        private static string getTemplate()
        {
            IniFile ini = new IniFile(System.Windows.Forms.Application.StartupPath + @"\Config.ini");
            return ini.ReadString("config", "TemplatePath");
        }

        private static string getReport()
        {
            IniFile ini = new IniFile(System.Windows.Forms.Application.StartupPath + @"\Config.ini");
            return ini.ReadString("config", "ReportPath");
        }

        private static string getEscalation()
        {
            IniFile ini = new IniFile(System.Windows.Forms.Application.StartupPath + @"\Config.ini");
            return ini.ReadString("config", "EscalationExcelPos");
        }
    }
}
