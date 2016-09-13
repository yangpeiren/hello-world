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

namespace 人员维护
{
    public partial class Form3 : Form
    {
        #region 全局变量
        Point M_pot_p = new Point();//原始位置

        int M_int_mx = 0, M_int_my = 0;//下次能继续
        int M_int_maxX, M_int_maxY;//加快读取用
        #endregion

        public bool tab = false;

        #region datetimepicker值变更检测

        private bool picker1 = false;
        private bool picker2 = false;

        private bool picker3 = false;
        private bool picker4 = false;
        private bool picker5 = false;

        private bool picker7 = false;
        private bool picker8 = false;
        private bool picker9 = false;
        private bool picker10 = false;

        private bool picker11 = false;
        private bool picker12 = false;
        private bool picker13 = false;
        private bool picker14 = false;

        private bool picker15 = false;
        private bool picker16 = false;
        private bool picker17 = false;

        #endregion

        public Form3()
        {
            InitializeComponent();

            this.AllowDrop = true;

            pictureBox1.AllowDrop = true;
            pictureBox1.DragEnter += new DragEventHandler(PictureBox1_DragEnter);
            pictureBox1.DragDrop += new DragEventHandler(PictureBox1_DragDrop);
            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
            pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            pictureBox1.MouseClick += new MouseEventHandler(pictureBox1_MouseClick);
            pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            tabPage1.MouseClick += new MouseEventHandler(tabPage1_MouseClick);
            tabPage2.MouseClick += new MouseEventHandler(tabPage2_MouseClick);

            tabControl1.Selecting +=new TabControlCancelEventHandler(tabControl1_Selecting);

            comboBox1.SelectedIndex = 0;
            bindcombobox2();
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
            comboBox8.SelectedIndex = 0;
            comboBox9.SelectedIndex = 0;
            comboBox10.SelectedIndex = 0;
            comboBox11.SelectedIndex = 0;
            comboBox12.SelectedIndex = 0;
            comboBox13.SelectedIndex = 0;
            comboBox14.SelectedIndex = 0;
            comboBox15.SelectedIndex = 0;
            comboBox16.SelectedIndex = 0;
            comboBox17.SelectedIndex = 0;
            comboBox18.SelectedIndex = 0;
            comboBox19.SelectedIndex = 0;


        }

        void tabPage2_MouseClick(object sender, MouseEventArgs e)
        {
            this.Focus();
        }

        void tabPage1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Focus();
        }
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(!tab)
                e.Cancel = true;
        }
        void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox1.Focus();
        }
        void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int zoom = 1;
                Bitmap bm = (Bitmap)pictureBox1.Image;
                int width = (int)(bm.Width * (zoom + e.Delta * 0.005 / 12));
                int height = (int)(bm.Height * (zoom + e.Delta * 0.005 / 12));
                if (pictureBox1.Width >= width || pictureBox1.Height >= height)
                {
                    return;
                }
                SmallPic(bm, width, height);

            }
        }
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="strOldPic">原图片</param>
        /// <param name="intWidth">缩放后图片宽度</param>
        /// <param name="intHeight">缩放后图片长度</param>
        public void SmallPic(Bitmap strOldPic, int intWidth, int intHeight)
        {
            try
            {
                pictureBox1.Image = new System.Drawing.Bitmap(strOldPic, intWidth, intHeight);
                strOldPic.Dispose();

            }
            catch (Exception exp)
            { throw exp; }
        }
        void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap tmpbmp = (Bitmap)this.pictureBox1.Image;
                M_pot_p = e.Location;
                M_int_maxX = pictureBox1.Width - tmpbmp.Width;
                M_int_maxY = pictureBox1.Height - tmpbmp.Height;
                Cursor = Cursors.SizeAll;
            }
        }

        void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle face = new Rectangle(new Point(100, 80), new Size(this.pictureBox1.Width - 200, this.pictureBox1.Height - 300));
            Rectangle rect = new Rectangle(new Point(30, 30), new Size(this.pictureBox1.Width - 60, this.pictureBox1.Height - 60));
            e.Graphics.DrawRectangle(new Pen(Color.Red, 1), rect);
            e.Graphics.DrawRectangle(new Pen(Color.Red, 1), face);

        }

        void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && pictureBox1.Image != null)
            {
                Bitmap tmpbmp = (Bitmap)this.pictureBox1.Image;
                M_int_mx = M_int_mx - M_pot_p.X + e.X;
                M_int_my = M_int_my - M_pot_p.Y + e.Y;
                //锁定范围
                M_int_mx = Math.Min(0, Math.Max(M_int_maxX, M_int_mx));
                M_int_my = Math.Min(0, Math.Max(M_int_maxY, M_int_my));
                Graphics g = pictureBox1.CreateGraphics();
                g.DrawImage(tmpbmp, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height), new Rectangle(-M_int_mx, -M_int_my, pictureBox1.Width, pictureBox1.Height), GraphicsUnit.Pixel);

                //画出轮廓红线
                Rectangle rect = new Rectangle(new Point(30, 30), new Size(this.pictureBox1.Width - 60, this.pictureBox1.Height - 60));
                Rectangle face = new Rectangle(new Point(100, 80), new Size(this.pictureBox1.Width - 200, this.pictureBox1.Height - 300));
                g.DrawRectangle(new Pen(Color.Red, 1), rect);
                g.DrawRectangle(new Pen(Color.Red, 1), face);
                g.Dispose();
                M_pot_p = e.Location;
            }
        }

        void PictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FileDrop"))
            {
                string[] files = e.Data.GetData("FileDrop") as string[];
                if (files != null && files.Length > 0)
                {
                    pictureBox1.Image = Image.FromFile(files[0]);
                }
            }
        }
        void PictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FileDrop"))
            {
                e.Effect = e.AllowedEffect;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == ""
                || comboBox1.SelectedIndex == 0 || comboBox2.SelectedIndex == 0 || comboBox3.SelectedIndex == 0
                || comboBox18.SelectedIndex == 0)
            {
                MessageBox.Show("请填写完整信息！", "错误");
                return;
            }
            if(textBox1.Text.Length != 5 || textBox4.Text.Length != 18 )
            {
                MessageBox.Show("工号/身份证号 长度错误！", "错误");
                return;
            }

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("请载入照片！", "错误");
                return;
            }

            string birth = Convert.ToString(textBox4.Text);
            StringBuilder sb = new StringBuilder(birth);
            StringBuilder sb1 = new StringBuilder(birth);
            StringBuilder sb2 = new StringBuilder(birth);
            sb.Remove(0, 6);
            sb.Remove(4, 8);
            sb.Append("-");
            sb1.Remove(0, 10);
            sb1.Remove(2, 6);
            sb1.Append("-");
            sb2.Remove(0, 12);
            sb2.Remove(2, 4);
            sb.Append(sb1);
            sb.Append(sb2);
            birth = Convert.ToString(sb);
            birth = Convert.ToDateTime(birth).ToString();

            string huji = textBox4.Text;
            string contain = "110";
            string Location = null;
            if (huji.IndexOf(contain) > -1)
            {
                Location = "北京";
            }
            else Location = "外地";

            Bitmap bm = (Bitmap)pictureBox1.Image.Clone();
            Rectangle rect = new Rectangle(-M_int_mx, -M_int_my, pictureBox1.Width, pictureBox1.Height);
            if (pictureBox1.Image.Width <= pictureBox1.Width || pictureBox1.Image.Height <= pictureBox1.Height)
            {
                MessageBox.Show("图片没有填满图片框", "错误！");
                return;
            }
            MemoryStream ms = new MemoryStream();
            bm.Clone(rect, bm.PixelFormat).Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] by = ms.ToArray();
            //int i = ms.Read(by, 0, by.Length);
            

            string sql = "insert into 人员管理 (编码,姓名,相片,拼音姓,拼音名,性别,出生日期,工段名称,身份证号,户口所在地,民族,班次,婚否)"+
            " values (@编码,@姓名,@相片,@拼音姓,@拼音名,@性别,@出生日期,@工段名称,@身份证号,@户口所在地,@民族,@班次,@婚否)";

            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@编码",textBox1.Text),
                new OleDbParameter("@姓名",textBox2.Text),
                new OleDbParameter("@相片",by),
                new OleDbParameter("@拼音姓",textBox37.Text),
                new OleDbParameter("@拼音名",textBox38.Text),
                new OleDbParameter("@性别",comboBox1.SelectedItem),
                new OleDbParameter("@出生日期",birth),
                new OleDbParameter("@工段名称",comboBox2.SelectedValue),
                new OleDbParameter("@身份证号",textBox4.Text),
                new OleDbParameter("@户口所在地",Location),
                new OleDbParameter("@民族",textBox3.Text),
                new OleDbParameter("@班次",comboBox3.SelectedItem),
                new OleDbParameter("@婚否",comboBox18.SelectedItem)
            };
            int num = 0;
            try
            {
                MainForm.con.Open();
                OleDbCommand command = new OleDbCommand(sql, MainForm.con);
                command.Parameters.AddRange(para);
                num = command.ExecuteNonQuery();
                if (num == 0)
                {
                    throw new Exception("此人已存在！");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                ms.Close();
                MainForm.con.Close();
            }
            if (num != 0)
            {
                tab = true;
                tabControl1.SelectedIndex += 1;
                tab = false;
                this.AllowDrop = false;
                this.pictureBox1.AllowDrop = false;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void bindcombobox2()
        {
            string sql = "select distinct 工段名称 from 人员管理";
            OleDbDataAdapter da = new OleDbDataAdapter(sql, MainForm.con);
            DataSet ds = new DataSet();

            try
            {                
                MainForm.con.Open();
                da.Fill(ds, "combo");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误");
            }
            finally
            {
                MainForm.con.Close();
            }

            DataRow dr = ds.Tables["combo"].NewRow();
            dr = ds.Tables["combo"].NewRow();
            dr["工段名称"] = "请选择";
            ds.Tables["combo"].Rows.InsertAt(dr, 0);
            comboBox2.DataSource = ds.Tables["combo"];
            comboBox2.ValueMember = "工段名称";
            comboBox2.DisplayMember = "工段名称";
                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tab = true;
            tabControl1.SelectedIndex -= 1;
            tab = false;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string sql = "delete from 人员管理 where 编码 = '" + textBox1.Text + "'";
            OleDbCommand cmd = new OleDbCommand(sql, MainForm.con);
            try
            {
                MainForm.con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                MainForm.con.Close();
            }
            tab = true;
            tabControl1.SelectedIndex -= 1;
            tab = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox8.Text == "" || textBox10.Text == ""|| comboBox4.SelectedIndex == 0 || comboBox5.SelectedIndex == 0 )
            {
                MessageBox.Show("请填写完整信息！", "错误");
                return;
            }
            if (textBox8.Text.Length != 11 || textBox10.Text.Length != 6)
            {
                MessageBox.Show("手机号/邮编 长度错误！", "错误");
                return;
            }
            string sql = "update 人员管理 set 户口类别 = @户口类别,户口所在家庭住址 = @户口所在家庭住址,所在区街道办事处 = @所在区街道办事处," +
                        "手机 = @手机,宅电 = @宅电,邮编 = @邮编,住址 = @住址," +
                        "政治面貌 = @政治面貌,入团时间 = @入团时间,入党时间 = @入党时间,担任党群职务 = @担任党群职务"
                        + " where 编码 = '" + textBox1.Text + "'";


            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@户口类别",comboBox5.SelectedItem),
                new OleDbParameter("@户口所在家庭住址",textBox6.Text),
                new OleDbParameter("@所在区街道办事处",textBox7.Text),
                new OleDbParameter("@手机",textBox8.Text),
                new OleDbParameter("@宅电",textBox9.Text),
                new OleDbParameter("@邮编",textBox10.Text),
                new OleDbParameter("@住址",textBox11.Text),
                new OleDbParameter("@政治面貌",comboBox4.SelectedItem),
                new OleDbParameter("@入团时间",dateTimePicker1.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@入党时间",dateTimePicker2.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@担任党群职务",textBox14.Text)
            };
            

            int num = 0;
            try
            {
                OleDbCommand cmd = new OleDbCommand(sql, MainForm.con);
                cmd.Parameters.AddRange(para);
                if (!picker1)
                    cmd.Parameters["@入团时间"].Value = DBNull.Value;
                if (!picker2)
                    cmd.Parameters["@入党时间"].Value = DBNull.Value;
                MainForm.con.Open();
                num = cmd.ExecuteNonQuery();
                if (num == 0)
                {
                    throw new Exception("添加失败！");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                MainForm.con.Close();
            }
            if (num != 0)
            {
                tab = true;
                tabControl1.SelectedIndex += 1;
                tab = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!picker3 || !picker4 || !picker5 || comboBox6.SelectedIndex == 0 || textBox5.Text != "" && comboBox19.SelectedIndex == 0)
            {
                MessageBox.Show("请填写完整信息！", "错误");
                return;
            }

            string sql = "update 人员管理 set 进入本公司时间 = @进入本公司时间,工作年月 = @工作年月,PC = @PC," +
            "PC值 = @PC值,职务或工种 = @职务或工种,签订时间 = @签订时间,合同期限 = @终止时间,"+
            "何时开始 = @何时开始,何时结束 = @何时结束,退休时间 = @退休时间,辞离职时间 = @辞离职时间"
            + " where 编码 = '" + textBox1.Text + "'";

            string timelimit;
            switch (comboBox6.SelectedItem.ToString())
            {
                case "两年":
                    timelimit = "2";
                    break;
                case "三年":
                    timelimit = "3";
                    break;
                case "五年":
                    timelimit = "5";
                    break;
                case "无固定期":
                    timelimit = "无固定期";
                    break;
                default:
                    timelimit = "";
                    break;
            }
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@进入本公司时间",dateTimePicker3.Value),
                new OleDbParameter("@工作年月",dateTimePicker4.Value),
                new OleDbParameter("@PC",textBox5.Text),
                new OleDbParameter("@PC值",comboBox19.SelectedItem),
                new OleDbParameter("@职务或工种",textBox13.Text),
                new OleDbParameter("@签订时间",dateTimePicker5.Value),
                new OleDbParameter("@合同期限",timelimit),
                new OleDbParameter("@何时开始",dateTimePicker7.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@何时结束",dateTimePicker8.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@退休时间",dateTimePicker9.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@辞离职时间",dateTimePicker10.Value.ToString("yyyy-MM-dd"))
            };

            int num = 0;
            try
            {
                OleDbCommand cmd = new OleDbCommand(sql, MainForm.con);
                cmd.Parameters.AddRange(para);
                if (!picker7)
                    cmd.Parameters["@何时开始"].Value = DBNull.Value;
                if (!picker8)
                    cmd.Parameters["@何时结束"].Value = DBNull.Value;
                if (!picker9)
                    cmd.Parameters["@退休时间"].Value = DBNull.Value;
                if (!picker10)
                    cmd.Parameters["@辞离职时间"].Value = DBNull.Value;
                if (textBox5.Text == "")
                    cmd.Parameters["@PC值"].Value = DBNull.Value;
                MainForm.con.Open();
                num = cmd.ExecuteNonQuery();
                if (num == 0)
                {
                    throw new Exception("添加失败！");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                MainForm.con.Close();
            }
            if (num != 0)
            {
                tab = true;
                tabControl1.SelectedIndex += 1;
                tab = false;
            }

        }

       

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox16.Text == "" || textBox18.Text == "" || textBox19.Text == "" || comboBox7.SelectedIndex == 0 || comboBox8.SelectedIndex == 0)
            {
                MessageBox.Show("请填写完整信息！", "错误");
                return;
            }
            string sql = "update 人员管理 set 原有 = @原有,原专业 = @原专业,现有 = @现有," +
            "现专业 = @现专业,毕业学校 = @毕业学校,技术职称 = @技术职称,技术等级 = @技术等级," +
            "社会培训 = @社会培训,公司级培训 = @公司级培训,取得职业资格 = @取得职业资格,特种作业证书 = @特种作业证书"
            + " where 编码 = '" + textBox1.Text + "'";


            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@原有",comboBox7.SelectedItem),
                new OleDbParameter("@原专业",textBox16.Text),
                new OleDbParameter("@现有",comboBox8.SelectedItem),
                new OleDbParameter("@现专业",textBox18.Text),
                new OleDbParameter("@毕业学校",textBox19.Text),
                new OleDbParameter("@技术职称",textBox15.Text),
                new OleDbParameter("@技术等级",textBox17.Text),
                new OleDbParameter("@社会培训",textBox20.Text),
                new OleDbParameter("@公司级培训",textBox21.Text),
                new OleDbParameter("@取得职业资格",textBox22.Text),
                new OleDbParameter("@特种作业证书",textBox23.Text)
            };

            int num = 0;
            try
            {
                OleDbCommand cmd = new OleDbCommand(sql, MainForm.con);
                cmd.Parameters.AddRange(para);
                MainForm.con.Open();
                num = cmd.ExecuteNonQuery();
                if (num == 0)
                {
                    throw new Exception("添加失败！");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                MainForm.con.Close();
            }
            if (num != 0)
            {
                tab = true;
                tabControl1.SelectedIndex += 1;
                tab = false;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (textBox26.Text!="" && comboBox9.SelectedIndex == 0|| comboBox10.SelectedIndex == 0
                || comboBox10.SelectedIndex == 1 || textBox27.Text == ""
                || comboBox12.SelectedIndex == 0 || comboBox13.SelectedIndex == 0 || comboBox14.SelectedIndex == 0
                || comboBox14.SelectedIndex == 1 && comboBox15.SelectedIndex == 0 )
            {
                MessageBox.Show("请填写完整信息！", "错误");
                return;
            }
            string sql = "update 人员管理 set 奖惩时间 = @奖惩时间,奖励名称 = @奖励名称,惩罚原因 = @惩罚原因," +
            "工伤证编号 = @工伤证编号,工伤级别 = @工伤级别,发生工伤时间 = @发生工伤时间,是否领取供暖费 = @是否领取供暖费," +
            "自何时开始 = @自何时开始,自何时结束 = @自何时结束,是否领取煤火费 = @是否领取煤火费,工作服号 = @工作服号," +
            "夏装 = @夏装,鞋号 = @鞋号,是否复转军人 = @是否复转军人,军种 = @军种,个人特长 = @个人特长,献血时间 = @献血时间,班车路线 = @班车路线"
            + " where 编码 = '" + textBox1.Text + "'";

            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@奖惩时间",dateTimePicker11.Value),
                new OleDbParameter("@奖励名称",textBox24.Text),
                new OleDbParameter("@惩罚原因",textBox25.Text),
                new OleDbParameter("@工伤证编号",textBox26.Text),
                new OleDbParameter("@工伤级别",(textBox26.Text != "")?Convert.ToInt32(comboBox9.SelectedItem):-1),
                new OleDbParameter("@发生工伤时间",dateTimePicker12.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@是否领取供暖费",comboBox10.SelectedItem),
                new OleDbParameter("@自何时开始",dateTimePicker13.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@自何时结束",dateTimePicker14.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@是否领取煤火费",comboBox11.SelectedItem),
                new OleDbParameter("@工作服号",textBox27.Text),
                new OleDbParameter("@夏装",comboBox12.SelectedItem),
                new OleDbParameter("@鞋号",comboBox13.SelectedItem),
                new OleDbParameter("@是否复转军人",comboBox14.SelectedItem),
                new OleDbParameter("@军种",(comboBox14.SelectedIndex == 1)?comboBox15.SelectedItem:""),
                new OleDbParameter("@个人特长",textBox28.Text),
                new OleDbParameter("@献血时间",dateTimePicker15.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@班车路线",textBox29.Text)
            };

            int num = 0;
            try
            {
                OleDbCommand cmd = new OleDbCommand(sql, MainForm.con);
                cmd.Parameters.AddRange(para);
                if (!picker11)
                    cmd.Parameters["@奖惩时间"].Value = DBNull.Value;
                if (Convert.ToInt32(cmd.Parameters["@工伤级别"].Value) == -1)
                    cmd.Parameters["@工伤级别"].Value = DBNull.Value;
                if (!picker12)
                    cmd.Parameters["@发生工伤时间"].Value = DBNull.Value;
                if (!picker13)
                    cmd.Parameters["@自何时开始"].Value = DBNull.Value;
                if (!picker14)
                    cmd.Parameters["@自何时结束"].Value = DBNull.Value;
                if (!picker15)
                    cmd.Parameters["@献血时间"].Value = DBNull.Value;
                MainForm.con.Open();
                num = cmd.ExecuteNonQuery();
                if (num == 0)
                {
                    throw new Exception("添加失败！");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                MainForm.con.Close();
            }
            if (num != 0)
            {
                tab = true;
                tabControl1.SelectedIndex += 1;
                tab = false;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string sql = "update 人员管理 set 配偶姓名 = @配偶姓名,配偶出生日期 = @配偶出生日期,配偶民族 = @配偶民族," +
            "配偶职务 = @配偶职务,配偶工作单位 = @配偶工作单位,配偶联系电话 = @配偶联系电话,子女姓名 = @子女姓名," +
            "子女性别 = @子女性别,子女出生日期 = @子女出生日期,是否领取独生子女证 = @是否领取独生子女证,单位 = @单位"
            + " where 编码 = '" + textBox1.Text + "'";

            string timelimit;
            switch (comboBox17.SelectedItem.ToString())
            {
                case "有":
                    timelimit = "是";
                    break;
                case "无":
                    timelimit = "否";
                    break;
                default:
                    timelimit = "";
                    break;
            }
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@配偶姓名",textBox30.Text),
                new OleDbParameter("@配偶出生日期",dateTimePicker16.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@配偶民族",textBox35.Text),
                new OleDbParameter("@配偶职务",textBox31.Text),
                new OleDbParameter("@配偶工作单位",textBox32.Text),
                new OleDbParameter("@配偶联系电话",textBox33.Text),
                new OleDbParameter("@子女姓名",textBox34.Text),
                new OleDbParameter("@子女性别",comboBox16.SelectedItem),
                new OleDbParameter("@子女出生日期",dateTimePicker17.Value.ToString("yyyy-MM-dd")),
                new OleDbParameter("@是否领取独生子女证",timelimit),
                new OleDbParameter("@单位",textBox36.Text)
            };

            int num = 0;
            try
            {
                OleDbCommand cmd = new OleDbCommand(sql, MainForm.con);
                cmd.Parameters.AddRange(para);
                if (!picker16)
                    cmd.Parameters["@配偶出生日期"].Value = DBNull.Value;
                if (!picker17)
                    cmd.Parameters["@子女出生日期"].Value = DBNull.Value;
                if (textBox34.Text == "")
                    cmd.Parameters["@子女性别"].Value = DBNull.Value;
                if (timelimit == "")
                    cmd.Parameters["@是否领取独生子女证"].Value = DBNull.Value;
                MainForm.con.Open();
                num = cmd.ExecuteNonQuery();
                if (num == 0)
                {
                    throw new Exception("添加失败！");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                MainForm.con.Close();
            }
            if (num != 0)
            {
                MessageBox.Show("添加成功！");
                this.DialogResult = DialogResult.OK;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            picker1 = true;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            picker2 = true;
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            picker3 = true;
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            picker4 = true;
        }

        private void dateTimePicker5_ValueChanged(object sender, EventArgs e)
        {
            picker5 = true;
        }

        private void dateTimePicker7_ValueChanged(object sender, EventArgs e)
        {
            picker7 = true;
        }

        private void dateTimePicker8_ValueChanged(object sender, EventArgs e)
        {
            picker8 = true;
        }

        private void dateTimePicker9_ValueChanged(object sender, EventArgs e)
        {
            picker9 = true;
        }

        private void dateTimePicker10_ValueChanged(object sender, EventArgs e)
        {
            picker10 = true;
        }

        private void dateTimePicker11_ValueChanged(object sender, EventArgs e)
        {
            picker11 = true;
        }

        private void dateTimePicker12_ValueChanged(object sender, EventArgs e)
        {
            picker12 = true;
        }

        private void dateTimePicker13_ValueChanged(object sender, EventArgs e)
        {
            picker13 = true;
        }

        private void dateTimePicker14_ValueChanged(object sender, EventArgs e)
        {
            picker14 = true;
        }

        private void dateTimePicker15_ValueChanged(object sender, EventArgs e)
        {
            picker15 = true;
        }

        private void dateTimePicker16_ValueChanged(object sender, EventArgs e)
        {
            picker16 = true;
        }

        private void dateTimePicker17_ValueChanged(object sender, EventArgs e)
        {
            picker17 = true;
        }


    }
}
