using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using DataGridViewAutoFilter;

namespace MaintenanceDB
{
    public partial class Form2 : Form
    {
        public string ret;
        public Form2()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            bindGridview1();
        }

        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex >= dataGridView1.RowCount)
                return;

            ret = dataGridView1[6, dataGridView1.CurrentCell.RowIndex].Value.ToString();
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form = new Form4();
            if (form.ShowDialog() == DialogResult.OK)
                bindGridview1();
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

        private void showAllLabel_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(dataGridView1);
        }
        void bindGridview1()
        {
            try
            {
                string sql = "select * from 设备 order by 所属区域";
                OleDbDataAdapter da = new OleDbDataAdapter(sql, Form1.con);
                DataSet ds = new DataSet();
                da.Fill(ds);
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
