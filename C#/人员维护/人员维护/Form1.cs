using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using DataGridViewAutoFilter;

namespace 人员维护
{
    public partial class MainForm : Form
    {
        public static string consent =
        "Provider = Microsoft.Jet.OLEDB.4.0;Data Source = " + getpath();
        public static OleDbConnection con = new OleDbConnection(consent);
        public static string getpath()
        {
            string str;
            IniFile ini = new IniFile(Application.StartupPath + @"\人员维护.ini");
            str = ini.ReadString("config", "Path");
            return str;
        }
        public MainForm()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            binddata();
        }
        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex >= dataGridView1.RowCount)
                return;

            Form2 form = new Form2(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());
            //form.num = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
            //form.index = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
            //form.Form2_Fill();
            form.ShowDialog();
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
         private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
            {
                DataGridViewAutoFilterColumnHeaderCell filterCell = dataGridView1.CurrentCell.OwningColumn.HeaderCell as DataGridViewAutoFilterColumnHeaderCell;
                if (filterCell != null)
                {
                    filterCell.ShowDropDownList();
                    e.Handled = true;
                }
            }
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
        private void dataGridView1_DataBindingComplete(object sender,
            DataGridViewBindingCompleteEventArgs e)
        {
            String filterStatus = DataGridViewAutoFilterColumnHeaderCell
                .GetFilterStatus(dataGridView1);
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

        private void 人员添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            if (form.ShowDialog() == DialogResult.OK)
            {
                binddata();
            }
        }
        void binddata()
        {
            try
            {
                string sql = "select 编码,姓名,性别," +
                    "DateDiff(\"yyyy\", 出生日期, Now())+ Int( Format(now(), \"mmdd\") <= Format(出生日期, \"mmdd\") ) as 年龄,"
                    + "工段名称,户口所在地,户口类别,民族,政治面貌,婚否 from 人员管理 order by 编码 ASC";
                OleDbDataAdapter da = new OleDbDataAdapter(sql, con);
                con.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                BindingSource dataSource = new BindingSource(ds.Tables[0], null);
                dataGridView1.DataSource = dataSource;
                this.dataGridView1.AutoResizeColumns();
                dataGridView1.KeyDown += new KeyEventHandler(dataGridView1_KeyDown);
                dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
                dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView1_CellDoubleClick);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "错误！");
                Application.Exit();
            }
            finally
            {
                con.Close();
            }
        }
    }
}
