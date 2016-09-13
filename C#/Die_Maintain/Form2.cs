using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Drawing.Imaging;

namespace Die_Maintain
{
    public partial class Form2 : Form
    {
        private static Bitmap mBitmap = null;
        private static System.Drawing.Printing.PrintDocument printDoc = new System.Drawing.Printing.PrintDocument();
        public string index;

        public int max_width = 300;
        public int max_height = 600;
        public int current_height = 0;
        private OleDbConnection con =
            new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0;Data Source = " +
                getpath() + ";Jet OleDb:DataBase Password = 25d55ad283aa400af464");

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
        public Form2()
        {
            InitializeComponent();

            label9.Width = (int)(label9.Height / 2 * 10.0);
            label9.Height = 30;
            label10.Width = (int)(label10.Height / 2 * 10.0);
            label10.Height = 30;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintPanel(this.panel1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void PrintPanel(Panel p)
        {
            Graphics mygraphics = p.CreateGraphics();
            Size s = p.Size;
            mBitmap = new Bitmap(s.Width, s.Height, mygraphics);
            Graphics memoryGraphics = Graphics.FromImage(mBitmap);
            IntPtr dc1 = mygraphics.GetHdc();
            IntPtr dc2 = memoryGraphics.GetHdc();
            BitBlt(dc2, 0, 0, p.ClientRectangle.Width, p.ClientRectangle.Height, dc1, 0, 0, 13369376);
            mygraphics.ReleaseHdc(dc1);
            memoryGraphics.ReleaseHdc(dc2);

            saveFileDialog1.Filter = "图片文件(*.jpg)|*.jpg;";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    mBitmap.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error");
                    return;
                }
                MessageBox.Show("保存成功！", "提示");
            }

        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr HDest, int nXDest, int nYDest, int nWidth, int hHeight, IntPtr HSrc, int nXSrc, int nYSrc, int DwRop);

        private static void PrintDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(mBitmap, 0, 0);
        }
        public void Form2_Fill()
        {

            string sql = "select * from 维修记录表 inner join 模具表 on 维修记录表.模具编号 = 模具表.模具编号 where ID = " + index;
            OleDbCommand cmd = new OleDbCommand(sql, con);
            OleDbDataReader reader;
            int picnum;//the number of pic
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                if (!reader.Read())
                    throw new Exception("查询集为空");
                #region 给弹出的详细信息界面赋值

                textBox8.Text = Convert.ToDateTime(reader["报修日期"]).ToString("yyyy-MM-dd");
                textBox9.Text = Convert.ToDateTime(reader["完成日期"]).ToString("yyyy-MM-dd");
                textBox1.Text = Convert.ToString(reader["报修人"]);
                textBox6.Text = Convert.ToString(reader["零件编号"]);
                textBox7.Text = Convert.ToString(reader["维修记录表.模具编号"]);
                textBox5.Text = Convert.ToString(reader["维修人员"]);
                textBox2.Text = Convert.ToString(reader["故障现象"]);
                textBox3.Text = Convert.ToString(reader["解决措施及完成情况"]);
                textBox4.Text = Convert.ToString(reader["故障原因"]);
                textBox10.Text = Convert.ToString(reader["工时"]) + "小时";
                textBox11.Text = Convert.ToString(reader["使用工具及材料"]);
                //form

                #endregion

                #region 处理图片

                string str = "图片";
                for (picnum = 0; picnum < 5; picnum++)
                {
                    string p = str + (picnum + 1).ToString();
                    if (reader[p].ToString() == "")
                        break;
                }

                switch (picnum)
                {
                    case 0: break;
                    case 1:
                        try
                        {
                            current_height = 20;
                            max_width = 300;
                            Add_PictureBox(new MemoryStream((byte[])reader["图片1"]), new PictureBox());
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message, "Error");
                        }
                        break;
                    case 2:
                        try
                        {
                            current_height = 20;
                            max_width = 300;
                            Add_PictureBox(new MemoryStream((byte[])reader["图片1"]), new PictureBox());
                            Add_PictureBox(new MemoryStream((byte[])reader["图片2"]), new PictureBox());
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message, "Error");
                        }
                        break;
                    case 3:
                        try
                        {
                            current_height = 20;
                            max_width = 240;
                            Add_PictureBox(new MemoryStream((byte[])reader["图片1"]), new PictureBox());
                            Add_PictureBox(new MemoryStream((byte[])reader["图片2"]), new PictureBox());
                            Add_PictureBox(new MemoryStream((byte[])reader["图片3"]), new PictureBox());
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message, "Error");
                        }
                        break;
                    case 4:
                        try
                        {
                            current_height = 20;
                            max_width = 180;
                            Add_PictureBox(new MemoryStream((byte[])reader["图片1"]), new PictureBox());
                            Add_PictureBox(new MemoryStream((byte[])reader["图片2"]), new PictureBox());
                            Add_PictureBox(new MemoryStream((byte[])reader["图片3"]), new PictureBox());
                            Add_PictureBox(new MemoryStream((byte[])reader["图片4"]), new PictureBox());
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message, "Error");
                        }
                        break;
                    case 5:
                        try
                        {
                            current_height = 20;
                            max_width = 133;
                            Add_PictureBox(new MemoryStream((byte[])reader["图片1"]), new PictureBox());
                            Add_PictureBox(new MemoryStream((byte[])reader["图片2"]), new PictureBox());
                            Add_PictureBox(new MemoryStream((byte[])reader["图片3"]), new PictureBox());
                            Add_PictureBox(new MemoryStream((byte[])reader["图片4"]), new PictureBox());
                            Add_PictureBox(new MemoryStream((byte[])reader["图片5"]), new PictureBox());
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message, "Error");
                        }
                        break;





                }

                #endregion
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!");
            }
            finally
            {
                con.Close();
            }
        }

        public void Add_PictureBox(MemoryStream ms, PictureBox pb)
        {
            if (current_height >= max_height)
                throw new Exception("长度过大！");
            pb.Image = Image.FromStream(ms, true);
            pb.Location = new Point(10, current_height);

            int x = max_width;
            int y = (int)pb.Image.Height * max_width / pb.Image.Width;
            y = y > max_height - current_height ? max_height - current_height : y;
            pb.Size = new Size(x, y);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            current_height += y + 15;

            pb.DoubleClick += new EventHandler(pb_DoubleClick);
            this.groupBox1.Controls.Add(pb);
        }

        void pb_DoubleClick(object sender, EventArgs e)
        {

            PictureBox name = (PictureBox)sender;
            Bitmap bp = (Bitmap)name.Image.Clone();
            Form3 form = new Form3();
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
        public Bitmap SmallPic(Bitmap strOldPic, int intWidth, int intHeight)
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
