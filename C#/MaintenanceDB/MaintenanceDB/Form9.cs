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
    public partial class Form9 : Form
    {
        private BindingSource dataSource = null;
        private BindingSource dataSource2 = null;
        public Form9()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView2.AutoGenerateColumns = false;
            try
            {
                string sql = "select * from 设备 order by 所属区域";
                OleDbDataAdapter da = new OleDbDataAdapter(sql, Form1.con);
                DataSet ds = new DataSet();
                da.Fill(ds, "设备");
                dataSource = new BindingSource(ds.Tables["设备"], null);
                dataGridView1.DataSource = dataSource;
                this.dataGridView1.AutoResizeColumns();
                dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
                dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView1_CellDoubleClick);
                sql = "select * from 备件 order by 备件大类";
                da = new OleDbDataAdapter(sql, Form1.con);
                ds = new DataSet();
                da.Fill(ds, "备件");
                DataRow[] drs = ds.Tables[0].Select("备用零件号 = '无备件'");
                foreach (DataRow dr in drs)
                    dr.Delete();
                dataSource2 = new BindingSource(ds.Tables["备件"], null);
                dataGridView2.DataSource = dataSource2;
                this.dataGridView2.AutoResizeColumns();
                dataGridView2.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView2_DataBindingComplete);
                dataGridView2.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView2_CellDoubleClick);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "错误！");
                Application.Exit();
            }
        }

        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex >= dataGridView1.RowCount)
                return;
            string sql = "select 备用零件号 from 设备备件表 where 设备编号 = @设备编号";
            OleDbCommand cmd = new OleDbCommand(sql, Form1.con);
            OleDbDataReader dr = null;
            try
            {
                cmd.Parameters.Add(new OleDbParameter("@设备编号", dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value));
                Form1.con.Open();
                dr = cmd.ExecuteReader();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "错误");
                return;
            }
            finally
            {
                if (dr.Read())
                {
                    dataSource2.Filter = "备用零件号 = '" + dr[0].ToString() + "'";
                    while (dr.Read())
                    {
                        dataSource2.Filter += " Or 备用零件号 = '" + dr[0].ToString() + "'";
                    }
                }
                else
                {
                    MessageBox.Show("本设备目前没有备用零件！");
                }
                Form1.con.Close();
            }
        }

        void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.RowIndex >= dataGridView2.RowCount)
                return;
            string sql = "select 设备编号 from 设备备件表 where 备用零件号 = @备用零件号";
            OleDbCommand cmd = new OleDbCommand(sql, Form1.con);
            OleDbDataReader dr = null;
            try
            {
                cmd.Parameters.Add(new OleDbParameter("@备用零件号", dataGridView2[0, dataGridView2.CurrentCell.RowIndex].Value));
                Form1.con.Open();
                dr = cmd.ExecuteReader();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "错误");
                return;
            }
            finally
            {
                if (dr.Read())
                {
                    dataSource.Filter = "设备编号 = '" + dr[0].ToString() + "'";
                    while (dr.Read())
                    {
                        dataSource.Filter += " Or 设备编号 = '" + dr[0].ToString() + "'";
                    }
                }
                else
                {
                    MessageBox.Show("本零件目前没有对应设备！");
                }
                Form1.con.Close();
            }
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
                showAllLabel1.Visible = false;
                filterStatusLabel1.Visible = false;
            }
            else
            {
                showAllLabel1.Visible = true;
                filterStatusLabel1.Visible = true;
                filterStatusLabel1.Text = filterStatus;
            }
        }
        private void dataGridView2_BindingContextChanged(object sender, EventArgs e)
        {
            if (dataGridView2.DataSource == null)
            {
                return;
            }

            foreach (DataGridViewColumn col in dataGridView2.Columns)
            {
                col.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
            }
        }
        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            String filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dataGridView2);
            if (String.IsNullOrEmpty(filterStatus))
            {
                showAllLabel2.Visible = false;
                filterStatusLabel2.Visible = false;
            }
            else
            {
                showAllLabel2.Visible = true;
                filterStatusLabel2.Visible = true;
                filterStatusLabel2.Text = filterStatus;
            }
        }

        private void showAllLabel1_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(dataGridView1);
        }

        private void showAllLabel2_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(dataGridView2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new Form11()).ShowDialog();
        }

    }
}
