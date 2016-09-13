using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;//添加的命名空间，对文件进行操作

namespace picture_handle
{
    public partial class Frm_Main : Form
    {
        #region 全局变量
        Point M_pot_p = new Point();//原始位置

        int M_int_mx = 0, M_int_my = 0;//下次能继续
        int M_int_maxX, M_int_maxY;//加快读取用
        #endregion

        public Frm_Main()
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
                int width = (int)(bm.Width * (zoom + e.Delta * 0.005/12));
                int height = (int)(bm.Height * (zoom + e.Delta * 0.005/12));
                if (pictureBox1.Width >= width || pictureBox1.Height >= height)
                {
                    return;
                }
                SmallPic(bm,width,height);

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
            Rectangle rect = new Rectangle(new Point(30,30),new Size(this.pictureBox1.Width - 60,this.pictureBox1.Height - 60));
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
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请确保已选择保存路径以及填写正确工号！", "错误！");
                return;
            }
            Bitmap bm = (Bitmap)pictureBox1.Image.Clone();
            Rectangle rect = new Rectangle(-M_int_mx, -M_int_my, pictureBox1.Width, pictureBox1.Height);
            if (pictureBox1.Image.Width <= pictureBox1.Width || pictureBox1.Image.Height <= pictureBox1.Height)
            {
                MessageBox.Show("图片没有填满图片框", "错误！");
                return;
            }
            Bitmap saver = bm.Clone(rect, bm.PixelFormat);
            saver.Save(textBox1.Text + "\\" + textBox2.Text + ".jpg");
            bm.Dispose(); 
            saver.Dispose();
            MessageBox.Show("保存成功！", "操作成功");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }
   
    }
}
