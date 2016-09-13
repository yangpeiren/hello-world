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
    public partial class Date_Select : Form
    {
        public DateTime ret;
        public Date_Select()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ret = this.monthCalendar1.SelectionStart;
            this.DialogResult = DialogResult.OK;
        }
    }
}
