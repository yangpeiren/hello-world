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

namespace MaintenanceDB
{
    public partial class Form7 : Form
    {
        public int num;
        public int Max;
        public static int max_Width = 230;
        public static int max_Height = 170;
        public Form7(int initialnum)
        {
            InitializeComponent();
            num = initialnum;
            BindData(initialnum);
            this.textBox2.Focus();
            pictureBox1.DoubleClick += new EventHandler(pictureBox1_DoubleClick);
        }

        void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Bitmap bp = (Bitmap)pictureBox1.Image.Clone();
            Form8 form = new Form8();
            if (bp.Width >= 1200)
                form.pictureBox1.Image = SmallPic(bp, 1200, 1200 * bp.Height / bp.Width);
            else form.pictureBox1.Image = bp;
            form.pictureBox1.Width = form.pictureBox1.Image.Width;
            form.pictureBox1.Height = form.pictureBox1.Image.Height;
            form.Width = form.pictureBox1.Width + 10;
            form.Height = form.pictureBox1.Height + 10;
            form.pictureBox1.Location = new Point(0, 0);
            form.Controls.Add(form.pictureBox1);
            form.ShowDialog();
        }
        public bool BindData(int number)
        {
            bool ret;
            string sql = "select 编号,format(维修记录.停机开始时间,\"yyyy-mm-dd hh:mm\") as 停机开始时间," +
                "format(维修记录.停机结束时间,\"yyyy-mm-dd hh:mm\") as 停机结束时间,format(维修记录.停机开始时间,\"yyyy-mm-dd\") as 日期,班次,所属区域," +
                "所属工位,CStr(datediff(\"n\",维修记录.停机开始时间,维修记录.停机结束时间)) + '分钟' as 时长,设备大类,设备类型,设备型号," +
                "设备名称,设备编号,故障类型,故障频次,故障描述,解决办法,设备工况,是否更换备件,维修记录.备用零件号,备件.备件名称 as 备件名称,重要性标志位,图片 " +
                "from 维修记录,设备,备件 where 维修记录.故障设备编号 = 设备.设备编号 and 维修记录.备用零件号 = 备件.备用零件号 and 编号 = @编号";
            OleDbCommand cmd = new OleDbCommand(sql, Form1.con);
            OleDbDataReader dr = null;
            try
            {
                Form1.con.Open();
                cmd.Parameters.Add(new OleDbParameter("@编号", number));
                dr = cmd.ExecuteReader();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally
            {
                if (dr.Read())
                {
                    label2.Text = dr["编号"].ToString();
                    label8.Text = dr["停机开始时间"].ToString();
                    label10.Text = dr["停机结束时间"].ToString();
                    label4.Text = dr["日期"].ToString();
                    label6.Text = dr["班次"].ToString();
                    if (dr["所属工位"].ToString() == "")
                        label21.Text = dr["所属区域"].ToString();
                    else
                        label21.Text = dr["所属区域"].ToString() + dr["所属工位"].ToString();
                    label12.Text = dr["时长"].ToString();
                    label13.Text = dr["设备型号"].ToString();
                    label15.Text = dr["设备名称"].ToString();
                    label23.Text = dr["设备编号"].ToString();
                    label19.Text = dr["设备大类"].ToString();
                    label17.Text = dr["设备类型"].ToString();
                    label25.Text = dr["设备工况"].ToString();
                    label27.Text = dr["故障类型"].ToString();
                    textBox1.Text = dr["故障描述"].ToString();
                    textBox1.SelectionStart = textBox1.Text.Length;
                    textBox2.Text = dr["解决办法"].ToString();
                    checkBox1.Checked = Convert.ToBoolean(dr["重要性标志位"]);
                    checkBox2.Checked = Convert.ToBoolean(dr["是否更换备件"]);
                    label34.Text = dr["故障频次"].ToString();
                    label35.Text = dr["备件名称"].ToString();
                    label36.Text = dr["备用零件号"].ToString();
                    if (dr["图片"].ToString() != "")
                    {
                        Image img = Image.FromStream(new MemoryStream((byte[])dr["图片"]), true);
                        if (img.Width <= pictureBox1.Width && img.Height <= pictureBox1.Height)
                            pictureBox1.Image = img;
                        else if (img.Width <= pictureBox1.Width && img.Height > pictureBox1.Height)
                        {
                            pictureBox1.Width = (int)img.Width * max_Height / img.Height;
                            pictureBox1.Image = img;
                        }
                        else if (img.Width > pictureBox1.Width && img.Height <= pictureBox1.Height)
                        {
                            pictureBox1.Height = (int)img.Height * max_Width / img.Width;
                            pictureBox1.Image = img;
                        }
                        else
                        {
                            if (img.Height / max_Height > img.Width / max_Width)
                                pictureBox1.Width = (int)img.Width * max_Height / img.Height;
                            else if (img.Height / max_Height < img.Width / max_Width)
                                pictureBox1.Height = (int)img.Height * max_Width / img.Width;
                            pictureBox1.Image = img;
                        }

                    }
                    else pictureBox1.Image = null;
                    ret = true;
                }
                else ret = false;
                Form1.con.Close();
            }
            return ret;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while (!BindData(++num))
            {
                if (num < Max)
                    num++;
                else
                {
                    num--;
                    MessageBox.Show("已是最后一项！");
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while (!BindData(--num))
            {
                if (num > 0)
                    num--;
                else
                {
                    num++;
                    MessageBox.Show("已是第一项！");
                    return;
                }
            }
        }
        private Bitmap SmallPic(Bitmap strOldPic, int intWidth, int intHeight)
        {
            Bitmap bp;
            try
            {
                bp = new System.Drawing.Bitmap(strOldPic, intWidth, intHeight);
                strOldPic.Dispose();

            }
            catch (Exception exp)
            { throw exp; }
            return bp;
        }
    }
}
