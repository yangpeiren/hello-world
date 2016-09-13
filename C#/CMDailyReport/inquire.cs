using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
//using System.Windows.Forms.DataVisualization.Charting;

namespace CMDailyReport
{
    public partial class inquire : Form
    {
        private DataSet ds = null;
        public inquire()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
            Bindcheckedlistbox();
            this.checkedListBox1.SelectedIndexChanged += new EventHandler(checkedListBox1_SelectedIndexChanged);
            
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox2.DataSource = DataRowToTable(this.ds.Tables[0].Select("ShopName = '" + this.checkedListBox1.SelectedValue.ToString() + "'"));
            checkedListBox2.ValueMember = "LineName";
            checkedListBox2.DisplayMember = "LineName";
        }
        private void select_all_CheckedChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.GetItemChecked(0))
            {
                for (int j = 0; j < checkedListBox1.Items.Count; j++)
                    checkedListBox1.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < checkedListBox1.Items.Count; j++)
                    checkedListBox1.SetItemChecked(j, false);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for(int i = 0; i < checkedListBox1.Items.Count;i++)
                {
                    this.checkedListBox1.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    this.checkedListBox1.SetItemChecked(i, false);
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    this.checkedListBox2.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    this.checkedListBox2.SetItemChecked(i, false);
                }
            }
        }

        private void Bindcheckedlistbox()
        {
            try
            {
                string sql = "select ShopName,LineName from Subline order by ShopName,Sequence ASC";
                Db_operation.con.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(sql, Db_operation.con);
                this.ds = new DataSet();
                da.Fill(ds);
                checkedListBox1.DataSource = SelectDistinct("Table1",ds.Tables[0],"ShopName");
                checkedListBox1.ValueMember = "ShopName";
                checkedListBox1.DisplayMember = "ShopName";
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Db_operation.con.Close();
            }
        }

        private DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
        {
            DataTable dt = new DataTable(TableName);
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

            object LastValue = null;
            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }
            return dt;
        }

        private bool ColumnEqual(object A, object B)
        {

            // Compares two values to see if they are equal. Also compares DBNULL.Value.
            // Note: If your DataTable contains object fields, then you must extend this
            // function to handle them in a meaningful way if you intend to group on them.

            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }

        private void inquire_Load(object sender, EventArgs e)
        {
            this.checkedListBox1.SelectedItem = 0;
        }

        private DataTable DataRowToTable(DataRow[] rows)
        {
            DataTable dt = new DataTable();
            dt = this.ds.Tables[0].Clone();
            foreach (DataRow row in rows) 
            {
                dt.Rows.Add(row.ItemArray);
            }
            return dt;
        }
        
    }
}
