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
    public partial class Form11 : Form
    {
        public Form11()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form2 form = new Form2();
            if (form.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = form.ret;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            if (form.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = form.ret;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请填写完整信息！", "错误");
                return;
            }
            string sql = "insert into 设备备件表 values(@设备编号,@备用零件号)";
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@设备编号",Convert.ToInt32(textBox1.Text)),
                new OleDbParameter("@备用零件号",textBox2.Text)
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
                    throw new Exception("关系加入数据库失败！");
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
                MessageBox.Show("关系新建成功！");
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
