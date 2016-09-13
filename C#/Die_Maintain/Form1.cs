using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace Die_Maintain
{
    public partial class Form1 : Form
    {
        private OleDbConnection con =
            new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0;Data Source = " +
                getpath() + ";Jet OleDb:DataBase Password = 25d55ad283aa400af464");
        public OleDbDataAdapter da;
        public DataSet ds;

        public string str_combox3 = "and 1 = 1 ";
        public string str_combox7 = "and 1 = 1 ";
        public string str_date1 = "and 报修日期 Between #2000-01-01# and ";
        public string str_date2 = null;

        public string excute = "select ID,报修日期,完成日期,模具编号,报修人,工时,使用工具及材料,故障现象,故障原因,解决措施及完成情况,维修人员 from 维修记录表 where 1 = 1 ";

        public static string getpath()
        {
            string str;
            IniFile ini = new IniFile(Application.StartupPath + @"\模具维修记录配置文件.ini");
            str = ini.ReadString("config", "Path");
            return str;
        }
        public static string getpassword()
        {
            string str;
            IniFile ini = new IniFile(Application.StartupPath + @"\模具维修记录配置文件.ini");
            str = ini.ReadString("config", "Password");
            return str;
        }
        public Form1()
        {
            InitializeComponent();

            str_date2 = "#" + System.DateTime.Today.ToString("yyyy-MM-dd") + "#";

            label9.Width = (int)(label9.Height / 2 * 10.0);
            label9.Height = 30;
            label10.Width = (int)(label10.Height / 2 * 10.0);
            label10.Height = 30;

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RowHeadersVisible = false;

            string sql;

            sql = "select distinct 报修人 from 维修记录表";
            da = new OleDbDataAdapter(sql, con);
            ds = new DataSet();
            try
            {
                con.Open();
                da.Fill(ds, "combo");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error");
                return;
            }
            finally
            {
                con.Close();
            }

            DataRow dr = ds.Tables["combo"].NewRow();
            dr = ds.Tables["combo"].NewRow();
            dr["报修人"] = "请选择";
            ds.Tables["combo"].Rows.InsertAt(dr, 0);
            comboBox3.DataSource = ds.Tables["combo"];
            comboBox3.ValueMember = "报修人";
            comboBox3.DisplayMember = "报修人";

            sql = "select distinct 车型 from 模具表";
            da = new OleDbDataAdapter(sql, con);

            try
            {
                con.Open();
                da.Fill(ds, "模具");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!");
            }
            finally
            {
                con.Close();
            }
            dr = ds.Tables["模具"].NewRow();
            dr["车型"] = "请选择";
            ds.Tables["模具"].Rows.InsertAt(dr, 0);

            comboBox1.DataSource = ds.Tables["模具"];
            comboBox1.ValueMember = "车型";
            comboBox1.DisplayMember = "车型";

            ds = new DataSet();
            try
            {
                con.Open();
                da.Fill(ds, "模具");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!");
            }
            finally
            {
                con.Close();
            }

            dr = ds.Tables["模具"].NewRow();
            dr["车型"] = "请选择";
            ds.Tables["模具"].Rows.InsertAt(dr, 0);

            comboBox9.DataSource = ds.Tables["模具"];
            comboBox9.ValueMember = "车型";
            comboBox9.DisplayMember = "车型";

            /*sql = "select distinct 零件编号 from 模具表 order by 零件编号 ASC";
            da = new OleDbDataAdapter(sql, con);

            try
            {
                con.Open();
                da.Fill(ds, "零件");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!");
            }
            finally
            {
                con.Close();
            }
            dr = ds.Tables["零件"].NewRow();
            dr["零件编号"] = "请选择";
            ds.Tables["零件"].Rows.InsertAt(dr, 0);

            comboBox4.DataSource = ds.Tables["零件"];
            comboBox4.ValueMember = "零件编号";
            comboBox4.DisplayMember = "零件编号";*/

            comboBox2.Items.Add("请选择");
            comboBox4.Items.Add("请选择");
            comboBox5.Items.Add("请选择");
            comboBox6.Items.Add("请选择");
            comboBox7.Items.Add("请选择");
            comboBox8.Items.Add("请选择");

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
            comboBox8.SelectedIndex = 0;

            dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView1_CellDoubleClick);
            dateTimePicker1.ValueChanged += new EventHandler(dateTimePicker1_ValueChanged);
            dateTimePicker2.ValueChanged += new EventHandler(dateTimePicker2_ValueChanged);
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            comboBox3.SelectedIndexChanged += new EventHandler(comboBox3_SelectedIndexChanged);
            comboBox4.SelectedIndexChanged += new EventHandler(comboBox4_SelectedIndexChanged);
            comboBox6.SelectedIndexChanged += new EventHandler(comboBox6_SelectedIndexChanged);
            comboBox7.SelectedIndexChanged += new EventHandler(comboBox7_SelectedIndexChanged);
            comboBox9.SelectedIndexChanged += new EventHandler(comboBox9_SelectedIndexChanged);

            sql = excute + str_combox3 + str_combox7 + str_date1 + str_date2;
            excute_sql(sql);

        }

        void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox9.SelectedIndex == 0)
                return;
            string sql = "select distinct 零件类型,零件编号 from 模具表 where 车型 = '" + comboBox9.SelectedValue + "' order by 零件编号 ASC";
            da = new OleDbDataAdapter(sql, con);
            DataSet dss = new DataSet();
            try
            {
                con.Open();
                da.Fill(dss, "Die");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!");
            }
            finally
            {
                con.Close();
            }
            DataRow dr = dss.Tables["Die"].NewRow();
            dr["零件类型"] = "请选择";
            dr["零件编号"] = "请选择";
            dss.Tables["Die"].Rows.InsertAt(dr, 0);

            comboBox8.DataSource = dss.Tables["Die"];
            comboBox8.ValueMember = "零件类型";
            comboBox8.DisplayMember = "零件类型";

            comboBox4.DataSource = dss.Tables["Die"];
            comboBox4.ValueMember = "零件编号";
            comboBox4.DisplayMember = "零件编号";
        }

        void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == 0)
            {
                comboBox5.DataSource = null;
                comboBox5.Items.Clear();
                comboBox5.Items.Add("请选择");
                comboBox5.SelectedIndex = 0;
                return;
            }
            string sql = "select 模具编号 from 模具表 where 零件编号 = '" + comboBox4.SelectedValue + "' order by 模具编号 ASC";
            da = new OleDbDataAdapter(sql, con);
            ds = new DataSet();
            try
            {
                con.Open();
                da.Fill(ds, "模具");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!");
            }
            finally
            {
                con.Close();
            }
            DataRow dr = ds.Tables["模具"].NewRow();
            dr["模具编号"] = "请选择";
            ds.Tables["模具"].Rows.InsertAt(dr, 0);

            comboBox5.DataSource = ds.Tables["模具"];
            comboBox5.ValueMember = "模具编号";
            comboBox5.DisplayMember = "模具编号";
        }

        void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.SelectedIndex == 0)
            {
                str_combox7 = "and 1 = 1 ";
            }
            else
            {
                str_combox7 = "and 模具编号 = '"+ comboBox7.SelectedValue +"'";
            }
            string sql = excute + str_combox3 + str_combox7 + str_date1 + str_date2;
            excute_sql(sql);
        }

        void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex == 0)
                return;
            string sql = "select 模具编号 from 模具表 where 零件编号 = '" + comboBox6.SelectedValue + "' order by 模具编号 ASC";
            da = new OleDbDataAdapter(sql, con);
            ds = new DataSet();
            try
            {
                con.Open();
                da.Fill(ds, "模具");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!");
            }
            finally
            {
                con.Close();
            }
            DataRow dr = ds.Tables["模具"].NewRow();
            dr["模具编号"] = "请选择";
            ds.Tables["模具"].Rows.InsertAt(dr, 0);

            comboBox7.DataSource = ds.Tables["模具"];
            comboBox7.ValueMember = "模具编号";
            comboBox7.DisplayMember = "模具编号";

            sql = "select ID,报修日期,完成日期,维修记录表.模具编号,报修人,工时,使用工具及材料,故障现象,故障原因,解决措施及完成情况,维修人员" +
                " from 维修记录表 inner join 模具表 on 维修记录表.模具编号 = 模具表.模具编号 where 1 = 1 "
                + str_combox3 + str_date1 + str_date2 + " and 零件编号 = '" + comboBox6.SelectedValue + "'";
            excute_sql(sql);
        }

        void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                str_combox3 = "and 1 = 1 ";
            }
            else
            {
                str_combox3 = "and 报修人 = '" + comboBox3.SelectedValue + "'";
            }
            string sql = excute + str_combox3 + str_combox7 + str_date1 + str_date2;
            excute_sql(sql);
        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                return;
            string sql = "select distinct 零件类型,零件编号 from 模具表 where 车型 = '"+ comboBox1.SelectedValue +"' order by 零件编号 ASC";
            da = new OleDbDataAdapter(sql, con);
            DataSet dss = new DataSet();
            try
            {
                con.Open();
                da.Fill(dss, "Die");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!");
            }
            finally
            {
                con.Close();
            }
            DataRow dr = dss.Tables["Die"].NewRow();
            dr["零件类型"] = "请选择";
            dr["零件编号"] = "请选择";
            dss.Tables["Die"].Rows.InsertAt(dr, 0);

            comboBox2.DataSource = dss.Tables["Die"];
            comboBox2.ValueMember = "零件类型";
            comboBox2.DisplayMember = "零件类型";

            comboBox6.DataSource = dss.Tables["Die"];
            comboBox6.ValueMember = "零件编号";
            comboBox6.DisplayMember = "零件编号";

            sql = "select ID,报修日期,完成日期,维修记录表.模具编号,报修人,工时,使用工具及材料,故障现象,故障原因,解决措施及完成情况,维修人员" +
                " from 维修记录表 inner join 模具表 on 维修记录表.模具编号 = 模具表.模具编号 where 1 = 1 " 
                + str_combox3 + str_date1 + str_date2 + " and 车型 = '"+ comboBox1.SelectedValue +"'";
            excute_sql(sql);
        }

        void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("日期范围错误", "Error");
                return;
            }
            str_date2 = "#" + dateTimePicker2.Value + "#";
            string sql = excute + str_combox3 + str_combox7 + str_date1 + str_date2;
            excute_sql(sql);
        }

        void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            str_date1 = "and 报修日期 Between #" + dateTimePicker1.Value + "# and ";
            string sql = excute + str_combox3 + str_combox7 + str_date1 + str_date2;
            excute_sql(sql);
        }

        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex >= dataGridView1.RowCount)
                return;

            Form2 form = new Form2();

            form.index = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
            form.Form2_Fill();
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;

            dateTimePicker2.Value = System.DateTime.Now;
            dateTimePicker1.Value = System.DateTime.Now;

            str_combox3 = "and 1 = 1 ";
            str_combox7 = "and 1 = 1 ";
            str_date1 = "and 报修日期 Between #2000-01-01# and ";
            str_date2 = "#" + System.DateTime.Today.ToString("yyyy-MM-dd") + "#";

            excute_sql(excute);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == 0 || comboBox5.SelectedIndex == 0 || textBox1.Text == "" || textBox2.Text == ""
                || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == ""  ||
                textBox7.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("请填写完整信息！");
                return;
            }
            try
            {
                Convert.ToDouble(textBox8.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("工时请填写数字！","Error!");
                textBox8.Text = "";
                return;
            }

            #region 处理图片

            byte[][] by = new byte[5][];
            int l = openFileDialog1.SafeFileNames.Length < 5 ? openFileDialog1.SafeFileNames.Length : 5;
            if (l == 1 && openFileDialog1.SafeFileNames[0] == "openFileDialog1")
                l = 0;
            for (int i = 0; i < l; i++)
            {
                FileStream fs = new FileStream(openFileDialog1.FileNames[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                by[i] = new byte[fs.Length];
                fs.Read(by[i], 0, by[i].Length);
                fs.Close();
            }

            #endregion




            OleDbParameter[] sp = new OleDbParameter[]
            {
                new OleDbParameter("@StartTime",dateTimePicker4.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@FinishTime",dateTimePicker3.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@Device",comboBox5.SelectedValue),
                new OleDbParameter("@Told",textBox1.Text),
                new OleDbParameter("@UsedTime",textBox8.Text),
                new OleDbParameter("@UsedMethod",textBox7.Text),
                new OleDbParameter("@Appearance",textBox2.Text),
                new OleDbParameter("@Reason",textBox3.Text),
                new OleDbParameter("@Solution",textBox4.Text),
                new OleDbParameter("@Serviceman",textBox5.Text),
                new OleDbParameter("@Pic1",by[0])
            };

            string sql = null;
            OleDbParameter[] op = null;
            switch (l)
            {
                case 0: op = null;
                    sql = "insert into 维修记录表(报修日期,完成日期,模具编号,报修人,工时,使用工具及材料,故障现象,故障原因," +
                        "解决措施及完成情况,维修人员) values(@StartTime,@FinishTime,@Device,@Told," +
                        "@UsedTime,@UsedMethod,@Appearance,@Reason,@Solution,@Serviceman)";
                    break;
                case 1: op = null;
                    sql = "insert into 维修记录表(报修日期,完成日期,模具编号,报修人,工时,使用工具及材料,故障现象,故障原因," +
                        "解决措施及完成情况,维修人员,图片1) values(@StartTime,@FinishTime,@Device,@Told," +
                        "@UsedTime,@UsedMethod,@Appearance,@Reason,@Solution,@Serviceman,@Pic1)";
                    break;
                case 2: op = new OleDbParameter[]
                        {
                            new OleDbParameter("@Pic2",by[1])
                        };
                    sql = "insert into 维修记录表(报修日期,完成日期,模具编号,报修人,工时,使用工具及材料,故障现象,故障原因," +
                        "解决措施及完成情况,维修人员,图片1,图片2) values(@StartTime,@FinishTime,@Device,@Told," +
                        "@UsedTime,@UsedMethod,@Appearance,@Reason,@Solution,@Serviceman,@Pic1,@Pic2)";
                    break;
                case 3: op = new OleDbParameter[]
                        {
                            new OleDbParameter("@Pic2",by[1]),
                            new OleDbParameter("@Pic3",by[2])
                        };
                    sql = "insert into 维修记录表(报修日期,完成日期,模具编号,报修人,工时,使用工具及材料,故障现象,故障原因," +
                        "解决措施及完成情况,维修人员,图片1,图片2,图片3) values(@StartTime,@FinishTime,@Device,@Told," +
                        "@UsedTime,@UsedMethod,@Appearance,@Reason,@Solution,@Serviceman,@Pic1,@Pic2,@Pic3)";
                    break;
                case 4: op = new OleDbParameter[]
                        {
                            new OleDbParameter("@Pic2",by[1]),
                            new OleDbParameter("@Pic3",by[2]),
                            new OleDbParameter("@Pic4",by[3])
                        };
                    sql = "insert into 维修记录表(报修日期,完成日期,模具编号,报修人,工时,使用工具及材料,故障现象,故障原因," +
                        "解决措施及完成情况,维修人员,图片1,图片2,图片3,图片4) values(@StartTime,@FinishTime,@Device,@Told," +
                        "@UsedTime,@UsedMethod,@Appearance,@Reason,@Solution,@Serviceman,@Pic1,@Pic2,@Pic3,@Pic4)";
                    break;
                case 5: op = new OleDbParameter[]
                        {
                            new OleDbParameter("@Pic2",by[1]),
                            new OleDbParameter("@Pic3",by[2]),
                            new OleDbParameter("@Pic4",by[3]),
                            new OleDbParameter("@Pic5",by[4])
                        };
                    sql = "insert into 维修记录表(报修日期,完成日期,模具编号,报修人,工时,使用工具及材料,故障现象,故障原因," +
                        "解决措施及完成情况,维修人员,图片1,图片2,图片3,图片4,图片5) values(@StartTime,@FinishTime,@Device,@Told," +
                        "@UsedTime,@UsedMethod,@Appearance,@Reason,@Solution,@Serviceman,@Pic1,@Pic2,@Pic3,@Pic4,@Pic5)";
                    break;

            }
            int val;
            OleDbCommand cmd = new OleDbCommand(sql, con);
            try
            {
                cmd.Parameters.AddRange(sp);
                if (op != null)
                    cmd.Parameters.AddRange(op);
                con.Open();
                val = cmd.ExecuteNonQuery();
                if (val != 0)
                {
                    MessageBox.Show("添加成功！", "提示");
                    
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return;
            }
            finally
            {
                con.Close();
            }
            //清理界面
            if (val != 0)
            {
                dateTimePicker3.Value = System.DateTime.Now;
                dateTimePicker4.Value = System.DateTime.Now;
                comboBox4.SelectedIndex = 0;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                comboBox9.SelectedIndex = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "图片文件(*.jpg,*.gif,*.bmp)|*.jpg;*.gif;*.bmp";
            openFileDialog1.Multiselect = true;
            textBox6.Text = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int l = openFileDialog1.SafeFileNames.Length < 5 ? openFileDialog1.SafeFileNames.Length : 5;
                if (textBox6.Text == "")
                    textBox6.Text += openFileDialog1.SafeFileNames[0];
                else textBox6.Text += "|" + openFileDialog1.SafeFileNames[0];
                for (int i = 1; i < l; i++)
                    textBox6.Text += "|" + openFileDialog1.SafeFileNames[i];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public void excute_sql(string sql)
        {
            da = new OleDbDataAdapter(sql, con);
            try
            {
                ds = new DataSet();
                con.Open();
                da.Fill(ds, "维修记录表");
                dataGridView1.DataSource = ds.Tables["维修记录表"];
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error");
            }
            finally
            {
                con.Close();
                toolStripStatusLabel1.Text = "共有" + dataGridView1.Rows.Count.ToString() + "条记录符合查询条件";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
