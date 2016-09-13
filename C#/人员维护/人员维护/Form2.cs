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

namespace 人员维护
{
    public partial class Form2 : Form
    {
        //public string num;
        public Form2(string num)
        {
            InitializeComponent();
            try
            {
                string sql = "select *,"
                    + "DateDiff(\"yyyy\", 出生日期, Now())+ Int( Format(now(), \"mmdd\") <= Format(出生日期, \"mmdd\") ) as 年龄,"
                    + "DateDiff(\"yyyy\", 工作年月, Now())+ Int( Format(now(), \"mmdd\") <= Format(工作年月, \"mmdd\") ) as 工龄,"
                    + "iif(isnull(何时结束),DateDiff(\"yyyy\", 何时开始, Now())+ Int( Format(now(), \"mmdd\") <= Format(何时开始, \"mmdd\")),DateDiff(\"yyyy\", 何时开始, 何时结束)+ Int( Format(何时结束, \"mmdd\") <= Format(何时开始, \"mmdd\"))) as 年限"
                    +" from 人员管理 where 编码 = '" + num + "'";
                OleDbCommand cmd = new OleDbCommand(sql, MainForm.con);
                OleDbDataReader dr;
                MainForm.con.Open();
                dr = cmd.ExecuteReader();
                if (!dr.Read())
                    throw new Exception("查无此人！");
                valueassign(dr);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,"错误！");
            }
            finally
            {
                MainForm.con.Close();
            }
        }
        public void valueassign(OleDbDataReader dr)
        {
            label2.Text = (dr["编码"].ToString() == "" ? "" : dr["编码"].ToString());
            label4.Text = (dr["姓名"].ToString() == "" ? "" : dr["姓名"].ToString());
            label6.Text = (dr["性别"].ToString() == "" ? "" : dr["性别"].ToString());
            label8.Text = (dr["民族"].ToString() == "" ? "" : dr["民族"].ToString());
            label10.Text = (dr["年龄"].ToString() == "" ? "" : dr["年龄"].ToString());
            label12.Text = (dr["出生日期"].ToString() == "" ? "" : Convert.ToDateTime(dr["出生日期"]).ToString("yyyy-MM-dd"));
            label14.Text = (dr["工段名称"].ToString() == "" ? "" : dr["工段名称"].ToString());
            label16.Text = (dr["身份证号"].ToString() == "" ? "" : dr["身份证号"].ToString());
            label18.Text = (dr["户口类别"].ToString() == "" ? "" : dr["户口类别"].ToString());
            label20.Text = (dr["户口所在家庭住址"].ToString() == "" ? "" : dr["户口所在家庭住址"].ToString());
            label22.Text = (dr["所在区街道办事处"].ToString() == "" ? "" : dr["所在区街道办事处"].ToString());
            label24.Text = (dr["手机"].ToString() == "" ? "" : dr["手机"].ToString());
            label26.Text = (dr["宅电"].ToString() == "" ? "" : dr["宅电"].ToString());
            label28.Text = (dr["邮编"].ToString() == "" ? "" : dr["邮编"].ToString());
            label30.Text = (dr["住址"].ToString() == "" ? "" : dr["住址"].ToString());
            label32.Text = (dr["政治面貌"].ToString() == "" ? "" : dr["政治面貌"].ToString());
            label34.Text = (dr["入团时间"].ToString() == "" ? "" : Convert.ToDateTime(dr["入团时间"]).ToString("yyyy-MM-dd"));
            label36.Text = (dr["入党时间"].ToString() == "" ? "" : Convert.ToDateTime(dr["入党时间"]).ToString("yyyy-MM-dd"));
            label38.Text = (dr["担任党群职务"].ToString() == "" ? "" : dr["担任党群职务"].ToString());
            label40.Text = (dr["进入本公司时间"].ToString() == "" ? "" : Convert.ToDateTime(dr["进入本公司时间"]).ToString("yyyy-MM-dd"));
            label42.Text = (dr["工作年月"].ToString() == "" ? "" : Convert.ToDateTime(dr["工作年月"]).ToString("yyyy-MM-dd"));
            label44.Text = (dr["工龄"].ToString() == "" ? "" : dr["工龄"].ToString());
            label46.Text = (dr["PC"].ToString() == "" ? "" : dr["PC"].ToString());
            label48.Text = (dr["PC值"].ToString() == "" ? "" : dr["PC值"].ToString());
            label50.Text = (dr["职务或工种"].ToString() == "" ? "" : dr["职务或工种"].ToString());
            label52.Text = (dr["签订时间"].ToString() == "" ? "" : Convert.ToDateTime(dr["签订时间"]).ToString("yyyy-MM-dd"));
            label54.Text = (dr["终止时间"].ToString() == "" ? "" : Convert.ToDateTime(dr["终止时间"]).ToString("yyyy-MM-dd"));
            label56.Text = (dr["合同期限"].ToString() == "" ? "" : dr["合同期限"].ToString());
            label58.Text = (dr["何时开始"].ToString() == "" ? "" : Convert.ToDateTime(dr["何时开始"]).ToString("yyyy-MM-dd"));
            label60.Text = (dr["何时结束"].ToString() == "" ? "" : Convert.ToDateTime(dr["何时结束"]).ToString("yyyy-MM-dd"));
            label62.Text = (dr["年限"].ToString() == "" ? "" : dr["年限"].ToString());
            label64.Text = (dr["退休时间"].ToString() == "" ? "" : Convert.ToDateTime(dr["退休时间"]).ToString("yyyy-MM-dd"));
            label66.Text = (dr["辞离职时间"].ToString() == "" ? "" : Convert.ToDateTime(dr["辞离职时间"]).ToString("yyyy-MM-dd"));
            label68.Text = (dr["原有"].ToString() == "" ? "" : dr["原有"].ToString());
            label70.Text = (dr["原专业"].ToString() == "" ? "" : dr["原专业"].ToString());
            label72.Text = (dr["现有"].ToString() == "" ? "" : dr["现有"].ToString());
            label74.Text = (dr["现专业"].ToString() == "" ? "" : dr["现专业"].ToString());
            label76.Text = (dr["毕业学校"].ToString() == "" ? "" : dr["毕业学校"].ToString());
            label78.Text = (dr["技术职称"].ToString() == "" ? "" : dr["技术职称"].ToString());
            label80.Text = (dr["技术等级"].ToString() == "" ? "" : dr["技术等级"].ToString());
            label82.Text = (dr["社会培训"].ToString() == "" ? "" : dr["社会培训"].ToString());
            label84.Text = (dr["公司级培训"].ToString() == "" ? "" : dr["公司级培训"].ToString());
            label86.Text = (dr["取得职业资格"].ToString() == "" ? "" : dr["取得职业资格"].ToString());
            label88.Text = (dr["特种作业证书"].ToString() == "" ? "" : dr["特种作业证书"].ToString());
            label90.Text = (dr["配偶姓名"].ToString() == "" ? "" : dr["配偶姓名"].ToString());
            label92.Text = (dr["配偶出生日期"].ToString() == "" ? "" : Convert.ToDateTime(dr["配偶出生日期"]).ToString("yyyy-MM-dd"));
            label94.Text = (dr["配偶民族"].ToString() == "" ? "" : dr["配偶民族"].ToString());
            label96.Text = (dr["配偶职务"].ToString() == "" ? "" : dr["配偶职务"].ToString());
            label98.Text = (dr["配偶工作单位"].ToString() == "" ? "" : dr["配偶工作单位"].ToString());
            label100.Text = (dr["配偶联系电话"].ToString() == "" ? "" : dr["配偶联系电话"].ToString());
            label102.Text = (dr["子女姓名"].ToString() == "" ? "" : dr["子女姓名"].ToString());
            label104.Text = (dr["子女性别"].ToString() == "" ? "" : dr["子女性别"].ToString());
            label106.Text = (dr["子女出生日期"].ToString() == "" ? "" : Convert.ToDateTime(dr["子女出生日期"]).ToString("yyyy-MM-dd"));
            label108.Text = (dr["是否领取独生子女证"].ToString() == "" ? "" : dr["是否领取独生子女证"].ToString());
            label110.Text = (dr["单位"].ToString() == "" ? "" : dr["单位"].ToString());
            label112.Text = (dr["奖惩时间"].ToString() == "" ? "" : dr["奖惩时间"].ToString());
            label114.Text = (dr["奖励名称"].ToString() == "" ? "" : dr["奖励名称"].ToString());
            label116.Text = (dr["惩罚原因"].ToString() == "" ? "" : dr["惩罚原因"].ToString());
            label118.Text = (dr["工伤证编号"].ToString() == "" ? "" : dr["工伤证编号"].ToString());
            label120.Text = (dr["工伤级别"].ToString() == "" ? "" : dr["工伤级别"].ToString());
            label122.Text = (dr["发生工伤时间"].ToString() == "" ? "" : Convert.ToDateTime(dr["发生工伤时间"]).ToString("yyyy-MM-dd"));
            label124.Text = (dr["是否领取供暖费"].ToString() == "" ? "" : dr["是否领取供暖费"].ToString());
            label126.Text = (dr["自何时开始"].ToString() == "" ? "" : Convert.ToDateTime(dr["自何时开始"]).ToString("yyyy-MM-dd"));
            label128.Text = (dr["自何时结束"].ToString() == "" ? "" : Convert.ToDateTime(dr["自何时结束"]).ToString("yyyy-MM-dd"));
            label130.Text = (dr["是否领取煤火费"].ToString() == "" ? "" : dr["是否领取煤火费"].ToString());
            label132.Text = (dr["工作服号"].ToString() == "" ? "" : dr["工作服号"].ToString());
            label134.Text = (dr["夏装"].ToString() == "" ? "" : dr["夏装"].ToString());
            label136.Text = (dr["鞋号"].ToString() == "" ? "" : dr["鞋号"].ToString());
            label138.Text = (dr["是否复转军人"].ToString() == "" ? "" : dr["是否复转军人"].ToString());
            label140.Text = (dr["军种"].ToString() == "" ? "" : dr["军种"].ToString());
            label142.Text = (dr["个人特长"].ToString() == "" ? "" : dr["个人特长"].ToString());
            label144.Text = (dr["献血时间"].ToString() == "" ? "" : Convert.ToDateTime(dr["献血时间"]).ToString("yyyy-MM-dd"));
            label146.Text = (dr["班车路线"].ToString() == "" ? "" : dr["班车路线"].ToString());
            label148.Text = (dr["班次"].ToString() == "" ? "" : dr["班次"].ToString());
            label150.Text = (dr["婚否"].ToString() == "" ? "" : dr["婚否"].ToString());
            if ((byte[])dr["相片"] != null)
            {
                MemoryStream ms = new MemoryStream((byte[])dr["相片"]);
                pictureBox1.Image = Image.FromStream(ms);
            }
        }
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Form4 form = new Form4();
            
            //tabpage1
            form.textBox1.Text = label2.Text;
            form.textBox2.Text = label4.Text;
            form.comboBox1.SelectedItem = label6.Text;
            form.textBox3.Text = label8.Text;
            form.comboBox2.SelectedValue = label14.Text;
            if (form.comboBox3.Items.IndexOf(label148.Text) == -1)
                form.comboBox3.Items.Add(label148.Text);
            form.comboBox3.SelectedItem = label148.Text;
            form.textBox4.Text = label16.Text;
            if (form.comboBox18.Items.IndexOf(label150.Text) == -1)
                form.comboBox18.Items.Add(label150.Text);
            form.comboBox18.SelectedItem = label150.Text;
            if (pictureBox1.Image != null)
                form.pictureBox1.Image = (Image)pictureBox1.Image.Clone();

            //tabpage2
            if (form.comboBox5.Items.IndexOf(label18.Text) == -1)
                form.comboBox5.Items.Add(label18.Text);
            form.comboBox5.SelectedItem = label18.Text;
            form.textBox6.Text = label20.Text;
            form.textBox7.Text = label22.Text;
            form.textBox8.Text = label24.Text;
            form.textBox9.Text = label26.Text;
            form.textBox10.Text = label28.Text;
            form.textBox11.Text = label30.Text;
            if (form.comboBox4.Items.IndexOf(label32.Text) == -1)
                form.comboBox4.Items.Add(label32.Text);
            form.comboBox4.SelectedItem = label32.Text;
            if (label34.Text != "")
            {
                form.picker1 = true;
                form.dateTimePicker1.Value = Convert.ToDateTime(label34.Text);
            }
            if (label36.Text != "")
            {
                form.picker2 = true;
                form.dateTimePicker2.Value = Convert.ToDateTime(label36.Text);
            }
            form.textBox14.Text = label38.Text;

            //tabpage3
            if (label40.Text != "")
            {
                form.picker3 = true;
                form.dateTimePicker3.Value = Convert.ToDateTime(label40.Text);
            }
            if (label42.Text != "")
            {
                form.picker4 = true;
                form.dateTimePicker4.Value = Convert.ToDateTime(label42.Text);
            }
            form.textBox5.Text = label46.Text;
            if (form.comboBox19.Items.IndexOf(label48.Text) == -1)
                form.comboBox19.Items.Add(label48.Text);
            form.comboBox19.SelectedItem = label48.Text;
            form.textBox13.Text = label50.Text;
            if (label52.Text != "")
            {
                form.picker5 = true;
                form.dateTimePicker5.Value = Convert.ToDateTime(label52.Text);
            }
            if (form.comboBox6.Items.IndexOf(label56.Text) == -1)
                form.comboBox6.Items.Add(label56.Text);
            form.comboBox6.SelectedItem = label56.Text;
            if (label58.Text != "")
            {
                form.picker7 = true;
                form.dateTimePicker7.Value = Convert.ToDateTime(label58.Text);
            }
            if (label60.Text != "")
            {
                form.picker8 = true;
                form.dateTimePicker8.Value = Convert.ToDateTime(label60.Text);
            }
            if (label64.Text != "")
            {
                form.picker9 = true;
                form.dateTimePicker9.Value = Convert.ToDateTime(label64.Text);
            }
            if (label66.Text != "")
            {
                form.picker10 = true;
                form.dateTimePicker10.Value = Convert.ToDateTime(label66.Text);
            }

            //tabpage4
            if (form.comboBox7.Items.IndexOf(label68.Text) == -1)
                form.comboBox7.Items.Add(label68.Text);
            form.comboBox7.SelectedItem = label68.Text;
            form.textBox16.Text = label70.Text;
            if (form.comboBox8.Items.IndexOf(label72.Text) == -1)
                form.comboBox8.Items.Add(label72.Text);
            form.comboBox8.SelectedItem = label72.Text;
            form.textBox18.Text = label74.Text;
            form.textBox19.Text = label76.Text;
            form.textBox15.Text = label78.Text;
            form.textBox17.Text = label80.Text;
            form.textBox20.Text = label82.Text;
            form.textBox21.Text = label84.Text;
            form.textBox22.Text = label86.Text;
            form.textBox23.Text = label88.Text;

            //tabpage5
            if (label112.Text != "")
            {
                form.picker11 = true;
                form.dateTimePicker11.Value = Convert.ToDateTime(label112.Text);
            }
            form.textBox24.Text = label114.Text;
            form.textBox25.Text = label116.Text;
            form.textBox26.Text = label118.Text;
            if (form.comboBox9.Items.IndexOf(label120.Text) == -1)
                form.comboBox9.Items.Add(label120.Text);
            form.comboBox9.SelectedItem = label120.Text;
            if (label122.Text != "")
            {
                form.picker12 = true;
                form.dateTimePicker12.Value = Convert.ToDateTime(label122.Text);
            }
            if (form.comboBox10.Items.IndexOf(label124.Text) == -1)
                form.comboBox10.Items.Add(label124.Text);
            form.comboBox10.SelectedItem = label124.Text;
            if (label126.Text != "")
            {
                form.picker13 = true;
                form.dateTimePicker13.Value = Convert.ToDateTime(label126.Text);
            }
            if (label128.Text != "")
            {
                form.picker14 = true;
                form.dateTimePicker14.Value = Convert.ToDateTime(label128.Text);
            }
            if (form.comboBox11.Items.IndexOf(label130.Text) == -1)
                form.comboBox11.Items.Add(label130.Text);
            form.comboBox11.SelectedItem = label130.Text;
            form.textBox27.Text = label132.Text;
            if (form.comboBox12.Items.IndexOf(label134.Text) == -1)
                form.comboBox12.Items.Add(label134.Text);
            form.comboBox12.SelectedItem = label134.Text;
            if (form.comboBox13.Items.IndexOf(label136.Text) == -1)
                form.comboBox13.Items.Add(label136.Text);
            form.comboBox13.SelectedItem = label136.Text;
            if (form.comboBox14.Items.IndexOf(label138.Text) == -1)
                form.comboBox14.Items.Add(label138.Text);
            form.comboBox14.SelectedItem = label138.Text;
            if (form.comboBox15.Items.IndexOf(label140.Text) == -1)
                form.comboBox15.Items.Add(label140.Text);
            form.comboBox15.SelectedItem = label140.Text;
            if (label144.Text != "")
            {
                form.picker15 = true;
                form.dateTimePicker15.Value = Convert.ToDateTime(label144.Text);
            }
            form.textBox28.Text = label142.Text;
            form.textBox29.Text = label146.Text;

            //tabpage6
            form.textBox30.Text = label90.Text;
            if (label92.Text != "")
            {
                form.picker16 = true;
                form.dateTimePicker16.Value = Convert.ToDateTime(label92.Text);
            }
            form.textBox31.Text = label96.Text;
            form.textBox32.Text = label98.Text;
            form.textBox33.Text = label100.Text;
            form.textBox35.Text = label94.Text;
            form.textBox34.Text = label102.Text;
            if (form.comboBox16.Items.IndexOf(label104.Text) == -1)
                form.comboBox16.Items.Add(label104.Text);
            form.comboBox16.SelectedItem = label104.Text;
            if (label106.Text != "")
            {
                form.picker17 = true;
                form.dateTimePicker17.Value = Convert.ToDateTime(label106.Text);
            }
            if (form.comboBox17.Items.IndexOf(label108.Text) == -1)
                form.comboBox17.Items.Add(label108.Text);
            form.comboBox17.SelectedItem = label108.Text;
            form.textBox36.Text = label110.Text;

            form.Show();
        }
        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
