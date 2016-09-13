using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;
using DataGridViewAutoFilter;

namespace MaintenanceDB
{
    public partial class Form3 : Form
    {
        public string ret;
        public Form3()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            binddataGridview1();
        }

        private void dataGridView1_BindingContextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                return;
            }

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
            }
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            String filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dataGridView1);
            if (String.IsNullOrEmpty(filterStatus))
            {
                showAllLabel.Visible = false;
                filterStatusLabel.Visible = false;
            }
            else
            {
                showAllLabel.Visible = true;
                filterStatusLabel.Visible = true;
                filterStatusLabel.Text = filterStatus;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 form = new Form5();
            if (form.ShowDialog() == DialogResult.OK)
                binddataGridview1();
        }

        private void showAllLabel_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(dataGridView1);
        }
        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex >= dataGridView1.RowCount)
                return;

            ret = dataGridView1[4, dataGridView1.CurrentCell.RowIndex].Value.ToString();
            this.DialogResult = DialogResult.OK;
        }
        void binddataGridview1()
        {
            try
            {
                string sql = "select * from 备件 order by 备件大类";
                OleDbDataAdapter da = new OleDbDataAdapter(sql, Form1.con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataRow[] drs = ds.Tables[0].Select("备用零件号 = '无备件'");
                foreach (DataRow dr in drs)
                    dr.Delete();
                BindingSource dataSource = new BindingSource(ds.Tables[0], null);
                dataGridView1.DataSource = dataSource;
                this.dataGridView1.AutoResizeColumns();
                dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
                dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView1_CellDoubleClick);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "错误！");
                Application.Exit();
            }
        }
    }
}
