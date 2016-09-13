using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CMDailyReport
{
    public partial class Escalation : Form
    {
        public const int MaxEscalation = 7;
        private DateTime dt;
        private string SName;
        private int Number;
        public Escalation(DateTime time, string Name)
        {
            this.dt = time;
            this.SName = Name;
            try
            {
                string sql = "select TNo from Escalation where TDate = @TDate order by TNo ASC";
                Db_operation.con.Open();
                OleDbCommand cmd = new OleDbCommand(sql, Db_operation.con);
                cmd.Parameters.Add(new OleDbParameter("@TDate", time));
                OleDbDataReader ole;
                ole = cmd.ExecuteReader();
                int i = 1;
                while (ole.Read())
                {
                    i++;
                }
                this.Number = i;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Db_operation.con.Close();
            }
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.label5.Text = this.Number.ToString();
            this.label6.Text = this.SName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            this.textBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "" && this.textBox2.Text == "")
            {
                MessageBox.Show("Please finish Content and Memo!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Db_operation.con.Open();
                DbInsertEscalation(dt,SName);
                this.Number++;
                this.label5.Text = this.Number.ToString();
                MessageBox.Show("Add in DB successful!", "Info!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.textBox1.Text = "";
                this.textBox2.Text = "";
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

        private void DbInsertEscalation(DateTime dt,string ShopName)
        {

            string sql = "insert into Escalation (TDate,TNo,ShopName,Content,Memory" +
            ") values(@TDate,@TNo,@ShopName,@Content,@Memory)";
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@TDate",this.dt),
                new OleDbParameter("@TNo",this.Number),
                new OleDbParameter("@ShopName",this.SName),
                new OleDbParameter("@Content",this.textBox1.Text),
                new OleDbParameter("@Memory",this.textBox2.Text)
            };
            OleDbCommand cmd = new OleDbCommand(sql, Db_operation.con);
            int num = 0;
            cmd.Parameters.AddRange(para);
            num = cmd.ExecuteNonQuery();
            if (num == 0)
            {
                throw new Exception("Fail to insert items into DB!");
            }
        }

    }
}
