using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MaintenanceDB
{
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
            this.label1.BackColor = Color.Transparent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new Form6()).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((new Form1()).ShowDialog() == DialogResult.OK)
                return;
            else return;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            (new Form9()).ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (new Form13()).ShowDialog();
        }
    }
}
