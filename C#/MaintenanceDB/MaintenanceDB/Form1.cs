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
using System.IO;
using DataGridViewAutoFilter;

namespace MaintenanceDB
{
    public partial class Form1 : Form
    {
        public static string getpath()
        {
            string str;
            IniFile ini = new IniFile(Application.StartupPath + @"\Config.ini");
            str = ini.ReadString("config", "Path");
            return str;
        }
        private static string constring = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source = " + getpath();
        public static OleDbConnection con = new OleDbConnection(constring);
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            button2.Enabled = false;
            textBox6.Text = "1";
            bindtextbox("select * from 故障原因");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            if (form.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = form.ret;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Form2 form = new Form2();
            if (form.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text =  form.ret;
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button2.Enabled = true;
                textBox2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                textBox2.Enabled = false;
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string year = dateTimePicker1.Value.Year.ToString();
            string month = dateTimePicker1.Value.Month.ToString();
            string day = dateTimePicker1.Value.Day.ToString();
            string hour = dateTimePicker2.Value.Hour.ToString();
            string minute = dateTimePicker2.Value.Minute.ToString();
            string date = year + "-" + month + "-" + day + " " + hour + ":" + minute;
            DateTime dtStart = Convert.ToDateTime(date);

            year = dateTimePicker3.Value.Year.ToString();
            month = dateTimePicker3.Value.Month.ToString();
            day = dateTimePicker3.Value.Day.ToString();
            hour = dateTimePicker4.Value.Hour.ToString();
            minute = dateTimePicker4.Value.Minute.ToString();
            date = year + "-" + month + "-" + day + " " + hour + ":" + minute;
            DateTime dtEnd = Convert.ToDateTime(date);

            if (textBox1.Text == "" || comboBox1.SelectedIndex == 0 || dtStart >= dtEnd
                ||textBox5.Text == "" || checkBox1.Checked == true && textBox2.Text == ""
                || comboBox2.SelectedIndex == 0 || textBox3.Text == "" || textBox4.Text == "" ||  textBox5.Text == "" || comboBox3.SelectedIndex == 0)
            {
                MessageBox.Show("请填写完整信息，保证起始结束时间正确！", "错误");
                return;
            }
            checkLinked();
            string sql = "insert into 维修记录 (停机开始时间,停机结束时间,班次,维修人员姓名,故障设备编号," +
            "故障类型,故障频次,故障描述,解决办法,设备工况,是否更换备件,备用零件号,重要性标志位,图片) values(@停机开始时间,@停机结束时间,@班次,@维修人员姓名,@故障设备编号," +
            "@故障类型,@故障频次,@故障描述,@解决办法,@设备工况,@是否更换备件,@备用零件号,@重要性标志位,@图片)";
            string text = "无备件";
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@停机开始时间",dtStart),
                new OleDbParameter("@停机结束时间",dtEnd),
                new OleDbParameter("@班次",comboBox1.SelectedItem),
                new OleDbParameter("@维修人员姓名",textBox1.Text),
                new OleDbParameter("@故障设备编号",Convert.ToInt32(textBox5.Text)),
                new OleDbParameter("@故障类型",comboBox2.SelectedItem),
                new OleDbParameter("@故障频次",Convert.ToInt32(textBox6.Text)),
                new OleDbParameter("@故障描述",textBox3.Text),                
                new OleDbParameter("@解决办法",textBox4.Text),
                new OleDbParameter("@设备工况",comboBox3.SelectedItem),
                new OleDbParameter("@是否更换备件",checkBox1.Checked),
                new OleDbParameter("@备用零件号", (textBox2.Text == "")?text:textBox2.Text),
                new OleDbParameter("@重要性标志位",checkBox2.Checked)
            };
            OleDbCommand cmd = new OleDbCommand(sql, Form1.con);
            int num = 0;
            try
            {
                Form1.con.Open();
                cmd.Parameters.AddRange(para);
                if (textBox8.Text != "")
                {
                    FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    byte[] by = new byte[fs.Length];
                    fs.Read(by, 0, by.Length);
                    fs.Close();
                    cmd.Parameters.Add(new OleDbParameter("@图片", by));
                }
                else cmd.Parameters.Add(new OleDbParameter("@图片", DBNull.Value));
                num = cmd.ExecuteNonQuery();
                if (num == 0)
                {
                    throw new Exception("记录加入数据库失败！");
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
                MessageBox.Show("记录添加成功！","消息");
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox8.Text = "";
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
                dateTimePicker3.Value = DateTime.Now;
                dateTimePicker4.Value = DateTime.Now;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == 0 || comboBox5.SelectedIndex == 0 || textBox7.Text == "" )
            {
                MessageBox.Show("请填写完整信息！", "错误");
                return;
            }
            string sql = "insert into 交接班记录 values(@日期,@班次,@白夜班,@内容)";
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@日期",Convert.ToDateTime(dateTimePicker5.Value.ToString())),
                new OleDbParameter("@班次",comboBox4.SelectedItem),
                new OleDbParameter("@白夜班",comboBox5.SelectedItem),
                new OleDbParameter("@内容",textBox7.Text)
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
                    throw new Exception("交接班记录加入数据库失败！");
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
                MessageBox.Show("交接班记录新建成功！");
                comboBox4.SelectedIndex = 0;
                comboBox5.SelectedIndex = 0;
                textBox7.Text = "";
                dateTimePicker5.Value = DateTime.Now;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "图片文件(*.jpg,*.gif,*.bmp)|*.jpg;*.gif;*.bmp";
            textBox8.Text = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox8.Text = openFileDialog1.SafeFileName;
        }
        public void bindtextbox(string sql)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand(sql,Form1.con);
                OleDbDataReader dr = null;
                Form1.con.Open();
                dr = cmd.ExecuteReader();
                
                if (!dr.HasRows)
                {
                    throw new Exception("查询数据库失败！");
                }
                comboBox2.Items.Clear();
                comboBox2.Items.Add("请选择");
                while(dr.Read())
                    comboBox2.Items.Add(dr["故障类型"].ToString());
                comboBox2.SelectedIndex = 0;
                
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                Form1.con.Close();
            }
            

        }
        void checkLinked()
        {
            if (!checkBox1.Checked)
                return;
            string sql = "select * from 设备备件表 where 设备编号 = @设备编号 and 备用零件号 = @备用零件号";
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@设备编号",textBox5.Text),
                new OleDbParameter("@备用零件号",textBox2.Text)
            };
            OleDbCommand cmd = new OleDbCommand(sql, Form1.con);
            try
            {
                Form1.con.Open();
                cmd.Parameters.AddRange(para);
                if (cmd.ExecuteScalar() == null)
                {
                    if (MessageBox.Show("数据库中没有此设备与此备件的关联，是否创建？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                    {
                        sql = "insert into 设备备件表 values(@设备编号,@备用零件号)";
                        OleDbParameter[] para1 = new OleDbParameter[]
                        {
                            new OleDbParameter("@设备编号",textBox5.Text),
                            new OleDbParameter("@备用零件号",textBox2.Text)
                        };
                        cmd = new OleDbCommand(sql, Form1.con);
                        try
                        {
                            cmd.Parameters.AddRange(para1);
                            int num = cmd.ExecuteNonQuery();
                            if (num == 0)
                            {
                                throw new Exception("关系新建失败！");
                            }
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message);
                        }
                    }
                    else
                    {
                        return;
                    }
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

        }
    }

}
