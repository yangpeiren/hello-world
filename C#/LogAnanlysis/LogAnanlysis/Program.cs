using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Ionic.Zip;
using System.Runtime.InteropServices;
using System.Windows.Forms;



namespace LogAnalysis
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName,
        string lpWindowName);
        // Activate an application window. 
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        static void Main(string[] args)
        {


            Process.Start(@"C:\Users\Peiren.Yang\Desktop\Automatisation_of_KUKALOGVIEWER\KukaLogViewer\KUKALOGVIEWER.exe");
            System.Threading.Thread.Sleep(5000);

            IntPtr calculatorHandle = FindWindow(null, "KUKA Logviewer");
            if (calculatorHandle == IntPtr.Zero)
            {
                MessageBox.Show("Calculator is not running.");
                return;
            }
            SetForegroundWindow(calculatorHandle);
            IntPtr hWnd = GetForegroundWindow();
            try
            {
                SendKeys.SendWait("%F");
                SendKeys.SendWait("O");
                SendKeys.SendWait(@"C:\Users\Peiren.Yang\Desktop\Automatisation_of_KUKALOGVIEWER\Log Files");
                SendKeys.SendWait("{ENTER}");
                //SendKeys.SendWait("{ENTER}");
                //SendKeys.Send("\"KrcLog.evt\" \"KrcLogB.evt\" \"KrcLogC.evt\" \"KrcLogI.evt\" \"KrcLogP.evt\" \"KrcLogS.evt\" \"KrcLogU.evt\" ");
                /*SendKeys.SendWait("{TAB}");
                System.Threading.Thread.Sleep(1000);
                SendKeys.SendWait("{TAB}");
                System.Threading.Thread.Sleep(1000);
                SendKeys.SendWait("{TAB}");
                System.Threading.Thread.Sleep(1000);
                SendKeys.SendWait("{TAB}");
                System.Threading.Thread.Sleep(1000);
                SendKeys.SendWait("{TAB}");
                System.Threading.Thread.Sleep(1000);
                SendKeys.SendWait("{TAB}");
                System.Threading.Thread.Sleep(1000);
                SendKeys.SendWait("{TAB}");
                System.Threading.Thread.Sleep(1000);
                SendKeys.SendWait("{TAB}");
                System.Threading.Thread.Sleep(1000);
                SendKeys.SendWait("^A");
                System.Threading.Thread.Sleep(1000);*/
                SendKeys.SendWait("\"KrcLog.evt\" \"KrcLogB.evt\" \"KrcLogC.evt\" \"KrcLogI.evt\" \"KrcLogP.evt\" \"KrcLogS.evt\" \"KrcLogU.evt\"");
                SendKeys.SendWait("{ENTER}");
                SendKeys.SendWait("ALT%E");
                SendKeys.SendWait("{ENTER}");
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
            System.Threading.Thread.Sleep(5000);
        }

    }
}


 
          