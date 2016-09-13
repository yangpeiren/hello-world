using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CMDailyReport
{
    public partial class WaitForm : Form
    {
        public WaitForm(BackgroundWorker worker)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);  
        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

    }
}
