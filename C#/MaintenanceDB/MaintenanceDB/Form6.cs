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
    public partial class Form6 : Form
    {
        public static DateTime lastMorning = DateTime.Today.AddHours(-16);
        public static DateTime lastNight = DateTime.Today.AddHours(-4);
        public static DateTime thisMorning = DateTime.Today.AddHours(8);
        public static DateTime thisNight = DateTime.Today.AddHours(20);
        public string banci;//白夜班
        public Form6()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            binddataGridView1();
        }
        void binddataGridView1()
        {
            string sql = "select 编号,format(维修记录.停机开始时间,\"yyyy-mm-dd\") as 日期,班次,所属区域," +
                "所属工位,CStr(datediff(\"n\",维修记录.停机开始时间,维修记录.停机结束时间)) + '分钟' as 时长,设备名称,停机开始时间,停机结束时间,故障频次,设备工况 " +
                "from 维修记录 inner join 设备 on 维修记录.故障设备编号 = 设备.设备编号 order by 维修记录.停机开始时间 asc";
            OleDbDataAdapter da = new OleDbDataAdapter(sql, Form1.con);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
                if (ds.Tables.Count == 0)
                    throw new Exception("没有数据！");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "错误");
            }
            BindingSource dataSource = new BindingSource(ds.Tables[0], null);
            dataGridView1.DataSource = dataSource;
            this.dataGridView1.AutoResizeColumns();
            dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
            //dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
            dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView1_CellDoubleClick);
            dataGridView1.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView1_RowHeaderMouseClick);

            if (DateTime.Now > thisNight)
            {
                dataSource.Filter = "停机开始时间 >= '" + thisMorning + "' And 停机开始时间 <= '" + thisNight + "'";
                banci = "夜班";

            }
            else if (DateTime.Now > thisMorning && DateTime.Now < thisNight)
            {
                dataSource.Filter = "停机开始时间 >= '" + lastNight + "' And 停机开始时间 <= '" + thisMorning + "'";
                banci = "白班";
            }
            else
            {
                dataSource.Filter = "停机开始时间 >= '" + lastMorning + "' And 停机开始时间 <= '" + lastNight + "'";
                banci = "夜班";
            }
            bindtextbox6(DateTime.Now.AddHours(-12));
        }

        void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex >= dataGridView1.RowCount)
                return;
            if (!bindtextbox6(Convert.ToDateTime(dataGridView1[6, dataGridView1.CurrentCell.RowIndex].Value)))
            {
                MessageBox.Show("本班没有添加交班记录");
                textBox6.Text = null;
            }
        }

        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex >= dataGridView1.RowCount)
                return;
            Form7 form = new Form7(Convert.ToInt32(dataGridView1[10, dataGridView1.CurrentCell.RowIndex].Value));
            form.Max = dataGridView1.Rows.Count;
            if (form.ShowDialog() == DialogResult.OK)
                return;
        }

        void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView1_BindingContextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                return;
            }

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderCell = new DataGridViewAutoFilterColumnHeaderCell(col.HeaderCell);
            }
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            String filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dataGridView1);
            if (String.IsNullOrEmpty(filterStatus))
            {
                showAllLabel.Visible = false;
                filterStatusLabel.Visible = false;
            }
            else
            {
                showAllLabel.Visible = true;
                filterStatusLabel.Visible = true;
                filterStatusLabel.Text = filterStatus;
            }
        }

        private void showAllLabel_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(dataGridView1);
        }
        public bool bindtextbox6(DateTime time)
        {
            bool ret;
            DateTime timeStart;
            DateTime timeEnd;
            string sql = "select * from 交接班记录 where 日期 between @开始 and @结束";
            OleDbCommand cmd = new OleDbCommand(sql, Form1.con);
            OleDbDataReader dr = null;
            if (time > time.Date.AddHours(20))
            {
                timeStart = time.Date.AddHours(20);
                timeEnd = time.Date.AddHours(32);
            }
            else if (time > time.Date.AddHours(8) && time < time.Date.AddHours(20))
            {
                timeStart = time.Date.AddHours(8);
                timeEnd = time.Date.AddHours(20);
            }
            else
            {
                timeStart = time.Date.AddHours(-4);
                timeEnd = time.Date.AddHours(8);
            }
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@开始",timeStart),
                new OleDbParameter("@结束",timeEnd)
            };
            try
            {
                Form1.con.Open();
                cmd.Parameters.AddRange(para);
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
                    textBox6.Text = Convert.ToDateTime(dr.GetValue(0)).ToString("yyyy-MM-dd") + " " + dr.GetValue(1).ToString() + "班 " +
                    dr.GetValue(2).ToString() + "\r\n" + dr.GetValue(3).ToString();
                    ret = true;
                }
                else
                {
                    ret = false;

                }
                Form1.con.Close();

            }
            return ret;
        }
    }
}
