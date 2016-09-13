﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices; //dllimport

namespace CMDailyReport
{
    class IniFile
    {
        public string path;
        public IniFile(string inipath)
        {
            FileInfo fileinfo = new FileInfo(inipath);
            if ((!fileinfo.Exists))
            {
                throw (new ApplicationException("ini file not found!"));
            }
            //复制路径
            path = inipath;
        }

        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        //读取配置文件  
        public string ReadString(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "1", temp, 255, this.path);
            return temp.ToString();
        }
        //写配置文件
        public bool WriteString(string Section, string Key, String str)
        {
            if (!WritePrivateProfileString(Section, Key, str, path))
            {
                return false;
            }
            return true;
        }
    }
}
