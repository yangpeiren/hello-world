using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace MaintenanceDB
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            bindcomboBox1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0 || comboBox2.SelectedIndex == 0 || textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请填写设备完整信息！", "错误");
                return;
            }
            string sql = "insert into 备件 values(@备用零件号,@备件名称,@备件类型,@备件大类,@所在库位)";
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@备用零件号",textBox2.Text.Trim()),
                new OleDbParameter("@备件名称",textBox1.Text.Trim()),
                new OleDbParameter("@备件类型",comboBox2.SelectedValue),
                new OleDbParameter("@备件大类",comboBox1.SelectedValue),
                new OleDbParameter("@所在库位",textBox3.Text)
            };
            OleDbCommand cmd = new OleDbCommand(sql, Form1.con);
            int num = 0;
            try
            {
                Form1.con.Open();
                cmd.Parameters.AddRange(para);
                num = cmd.ExecuteNonQuery();
                if (num == 0)
                {
                    throw new Exception("设备加入数据库失败！");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                Form1.con.Close();
            }
            if (num != 0)
            {
                MessageBox.Show("设备新建成功！");
                this.DialogResult = DialogResult.OK;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindcomboBox2(comboBox1.SelectedValue.ToString());
        }

        void bindcomboBox1()
        {
            string sql = "select distinct 备件大类 from 备件";
            OleDbDataAdapter da = new OleDbDataAdapter(sql, Form1.con);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds, "备件");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            DataRow dr = ds.Tables["备件"].NewRow();
            dr = ds.Tables["备件"].NewRow();
            dr["备件大类"] = "请选择";
            ds.Tables["备件"].Rows.InsertAt(dr, 0);
            comboBox1.DataSource = ds.Tables["备件"];
            comboBox1.ValueMember = "备件大类";
        }
        void bindcomboBox2(string value)
        {
            string sql = "select distinct 备件类型 from 备件 where 备件大类 = @备件大类";
            OleDbDataAdapter da = new OleDbDataAdapter(sql, Form1.con);
            DataSet ds = new DataSet();
            try
            {
                da.SelectCommand.Parameters.Add(new OleDbParameter("@备件大类", value));
                da.Fill(ds, "备件");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            DataRow dr = ds.Tables["备件"].NewRow();
            dr = ds.Tables["备件"].NewRow();
            dr["备件类型"] = "请选择";
            ds.Tables["备件"].Rows.InsertAt(dr, 0);
            comboBox2.DataSource = ds.Tables["备件"];
            comboBox2.ValueMember = "备件类型";
        }
    }
}
