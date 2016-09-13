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
    public partial class Login : Form
    {
        
        public Login()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Your ID is:" + (radioButton6.Checked ? radioButton6.Text : null);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Your ID is:" + ( radioButton2.Checked ? radioButton2.Text : null);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Your ID is:" + ( radioButton3.Checked ? radioButton3.Text : null);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Your ID is:" + ( radioButton4.Checked ? radioButton4.Text : null);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Your ID is:" + ( radioButton5.Checked ? radioButton5.Text : null);
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Your ID is:" + ( radioButton9.Checked ? radioButton9.Text : null);
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Your ID is:" + ( radioButton7.Checked ? radioButton7.Text : null);
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Your ID is:" + ( radioButton8.Checked ? radioButton8.Text : null);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Your ID is:" + ( radioButton1.Checked ? radioButton1.Text : null);
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Your ID is:" + ( radioButton10.Checked ? radioButton10.Text : null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Input ip = new Input((toolStripStatusLabel1.Text.Split(new char[] { ':' }))[1],monthCalendar1.SelectionStart);
            this.DialogResult = DialogResult.OK;
            this.Dispose();
            ip.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }
    }
}
