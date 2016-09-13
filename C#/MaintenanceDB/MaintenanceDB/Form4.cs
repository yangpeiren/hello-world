using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.Sql;

namespace MaintenanceDB
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            bindcomboBox1();
            comboBox2.SelectedIndex = 0;
            bindcomboBox3();
            comboBox4.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0 || comboBox2.SelectedIndex == 0 || comboBox3.SelectedIndex == 0
                || comboBox4.SelectedIndex == 0 || textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请填写设备完整信息！", "错误");
                return;
            }
            string sql = "insert into 设备(设备名称,设备型号,设备类型,设备大类,所属工位,所属区域) values(@设备名称,@设备型号,@设备类型,@设备大类,@所属工位,@所属区域)";
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@设备名称",textBox2.Text.Trim()),
                new OleDbParameter("@设备型号",textBox1.Text.Trim()),
                new OleDbParameter("@设备类型",comboBox4.SelectedValue),
                new OleDbParameter("@设备大类",comboBox3.SelectedValue),
                new OleDbParameter("@所属工位",comboBox2.SelectedValue),
                new OleDbParameter("@所属区域",comboBox1.SelectedValue)
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindcomboBox4(comboBox3.SelectedValue.ToString());
        }
        void bindcomboBox1()
        {
            string sql = "select distinct 所属区域 from 设备";
            OleDbDataAdapter da = new OleDbDataAdapter(sql, Form1.con);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds, "设备");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            DataRow dr = ds.Tables["设备"].NewRow();
            dr = ds.Tables["设备"].NewRow();
            dr["所属区域"] = "请选择";
            ds.Tables["设备"].Rows.InsertAt(dr, 0);
            comboBox1.DataSource = ds.Tables["设备"];
            comboBox1.ValueMember = "所属区域";
        }
        void bindcomboBox2(string value)
        {
            string sql = "select distinct 所属工位 from 设备 where 所属区域 = @所属区域";
            OleDbDataAdapter da = new OleDbDataAdapter(sql, Form1.con);
            DataSet ds = new DataSet();
            try
            {
                da.SelectCommand.Parameters.Add(new OleDbParameter("@所属区域", value));
                da.Fill(ds, "设备");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            DataRow dr = ds.Tables["设备"].NewRow();
            dr = ds.Tables["设备"].NewRow();
            dr["所属工位"] = "请选择";
            ds.Tables["设备"].Rows.InsertAt(dr, 0);
            comboBox2.DataSource = ds.Tables["设备"];
            comboBox2.ValueMember = "所属工位";
        }
        void bindcomboBox3()
        {
            string sql = "select distinct 设备大类 from 设备";
            OleDbDataAdapter da = new OleDbDataAdapter(sql, Form1.con);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds, "设备");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            DataRow dr = ds.Tables["设备"].NewRow();
            dr = ds.Tables["设备"].NewRow();
            dr["设备大类"] = "请选择";
            ds.Tables["设备"].Rows.InsertAt(dr, 0);
            comboBox3.DataSource = ds.Tables["设备"];
            comboBox3.ValueMember = "设备大类";
        }
        void bindcomboBox4(string value)
        {
            string sql = "select distinct 设备类型 from 设备 where 设备大类 = @设备大类";
            OleDbDataAdapter da = new OleDbDataAdapter(sql, Form1.con);
            DataSet ds = new DataSet();
            try
            {
                da.SelectCommand.Parameters.Add(new OleDbParameter("@设备大类", value));
                da.Fill(ds, "设备");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            DataRow dr = ds.Tables["设备"].NewRow();
            dr = ds.Tables["设备"].NewRow();
            dr["设备类型"] = "请选择";
            ds.Tables["设备"].Rows.InsertAt(dr, 0);
            comboBox4.DataSource = ds.Tables["设备"];
            comboBox4.ValueMember = "设备类型";
        }
    }
}
