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
    public partial class Input : Form
    {
        private string disp = null;
        private DateTime time = DateTime.Now;
        private bool withoutTop5 = false;
        enum presult { Ini, OK, NOK };

        public Input(string plant,DateTime heute)
        {
            disp = plant;
            time = heute.Date;
            try
            {
                string sql = "select LineName from Subline where ShopName = '" + plant + "' order by Sequence ASC";
                Db_operation.con.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(sql, Db_operation.con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                string[] str = new string[ds.Tables[0].Rows.Count];
                for (int temp = 0; temp < ds.Tables[0].Rows.Count; temp++)
                {
                    str[temp] = ds.Tables[0].Rows[temp][0].ToString();
                }
                InitializeComponent(str);
                this.StartPosition = FormStartPosition.CenterScreen;

                if (Insertcheck())
                {

                    DialogResult result = MessageBox.Show("On Day " + time.ToString("yyyy-MM-dd") + " ,record of Line " + disp + " already exists in DB, do you want to change it?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        LoadfromDB(heute, plant);
                        //MessageBox.Show("To be continue...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Db_operation.con.Close();
            }
            
            //InitializeComponent();
        }

        private void Input_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Your ID is:" + disp;
            this.Text = "Input-" + disp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (TabPage tp in tabControl1.TabPages)
            {
                if (!checktab(tp))
                {
                    MessageBox.Show("Tabpage " + tp.Text + " is not finished, please finish it first!", "error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            try
            {
                Db_operation.con.Open();
                if (Insertcheck())
                {
                    DeletefromDB(this.time, this.disp);
                }
                switch (validcheck())
                {
                    case 0:
                        {
                            if (!withoutTop5)
                                return;
                            DbInsertDownTime();
                            Log.DBInsert();
                            break;
                        }
                    case 1:
                        {
                            DbInsertDownTime();
                            DbInsertTop5(panel1);
                            Log.DBInsert();
                            break;
                        }
                    case 2:
                        {
                            DbInsertDownTime();
                            DbInsertTop5(panel1);
                            DbInsertTop5(panel2);
                            Log.DBInsert();
                            break;
                        }
                    case 3:
                        {
                            DbInsertDownTime();
                            DbInsertTop5(panel1);
                            DbInsertTop5(panel2);
                            DbInsertTop5(panel3);
                            Log.DBInsert();
                            break;
                        }
                    case 4:
                        {
                            DbInsertDownTime();
                            DbInsertTop5(panel1);
                            DbInsertTop5(panel2);
                            DbInsertTop5(panel3);
                            DbInsertTop5(panel4);
                            Log.DBInsert();
                            break;
                        }
                    case 5:
                        {
                            DbInsertDownTime();
                            DbInsertTop5(panel1);
                            DbInsertTop5(panel2);
                            DbInsertTop5(panel3);
                            DbInsertTop5(panel4);
                            DbInsertTop5(panel5);
                            Log.DBInsert();
                            break;
                        }
                    default: return;
                }
                MessageBox.Show("Add in DB successful!", "Info!",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
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

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (Control ct in this.panel1.Controls)
            {
                if (ct.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    ((TextBox)ct).Text = null;
                }
                if (ct.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
                {
                    ((NumericUpDown)ct).Value = 0;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (Control ct in this.panel2.Controls)
            {
                if (ct.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    ((TextBox)ct).Text = null;
                }
                if (ct.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
                {
                    ((NumericUpDown)ct).Value = 0;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (Control ct in this.panel3.Controls)
            {
                if (ct.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    ((TextBox)ct).Text = null;
                }
                if (ct.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
                {
                    ((NumericUpDown)ct).Value = 0;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (Control ct in this.panel4.Controls)
            {
                if (ct.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    ((TextBox)ct).Text = null;
                }
                if (ct.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
                {
                    ((NumericUpDown)ct).Value = 0;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach (Control ct in this.panel5.Controls)
            {
                if (ct.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    ((TextBox)ct).Text = null;
                }
                if (ct.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
                {
                    ((NumericUpDown)ct).Value = 0;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button3.PerformClick();
            button4.PerformClick();
            button5.PerformClick();
            button6.PerformClick();
            button7.PerformClick();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Filter = "Excel(*.xls,*.xlsx)|*.xls;*.xlsx";
            if(this.openFileDialog1.ShowDialog() == DialogResult.OK)
                loadexcel(openFileDialog1.FileNames);
        }

        private presult checkpanel(Panel pnl)
        {
            presult ret = presult.Ini;
            bool ini = true;
            bool finish = true;
            foreach (Control ct in pnl.Controls)
            {
                if (ct.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    if (((TextBox)ct).Text == "" )
                    {
                        ini = ini & true;
                        finish = finish & false;
                    }
                    else
                    {
                        ini = ini & false;
                        finish = finish & true;
                    }
                }
                if ((ct.GetType().ToString() == "System.Windows.Forms.NumericUpDown") && !((ct.Name.Split(new char[] { '_' })[1] == "Loss") || (ct.Name.Split(new char[] { '_' })[1] == "Downtime")))
                {
                    if (((NumericUpDown)ct).Value == 0)
                    {
                        ini = ini & true;
                        finish = finish & false;
                    }
                    else
                    {
                        ini = ini & false;
                        finish = finish & true;
                    }
                }
            }
            if ((ini == true) & (finish == false))
            {
                ret = presult.Ini;
            }
            else if ((ini == false) & (finish == true))
            {
                ret = presult.OK;
            }
            else
                ret = presult.NOK;
            return ret;
        }

        private bool checktab(TabPage pnl)
        {
            foreach (Control ct in pnl.Controls)
            {
                if (ct.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
                {
                    if (((NumericUpDown)ct).Value == 0)
                    {
                        if (ct.Name.Split(new char[] { '_' })[1] == "n1" || ct.Name.Split(new char[] { '_' })[1] == "n2")
                        {
                            DialogResult dr =  MessageBox.Show("Dose the line " + pnl.Text + " have no production?", "warning!",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                            if (dr == DialogResult.Yes)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool DbInsertDownTime()
        {
            bool ret = false;
            foreach (TabPage tp in tabControl1.TabPages)
            {
                string sql = "insert into DownTime (TDate,Line,SubLine,Target,Actual," +
                "Downloss,Downtime) values(@TDate,@Line,@SubLine,@Target,@Actual," +
                "@Downloss,@Downtime)";
                int Target = 0, Actual = 0, Downloss = 0, Downtime = 0;
                foreach (Control ct in tp.Controls)
                {
                    if (ct.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
                    {
                        if(ct.Name.Split(new char[] { '_' })[1] == "n1")
                            Target = Decimal.ToInt32(((NumericUpDown)ct).Value);
                        if (ct.Name.Split(new char[] { '_' })[1] == "n2")
                            Actual = Decimal.ToInt32(((NumericUpDown)ct).Value);
                        if (ct.Name.Split(new char[] { '_' })[1] == "n3")
                            Downloss = Decimal.ToInt32(((NumericUpDown)ct).Value);
                        if (ct.Name.Split(new char[] { '_' })[1] == "n4")
                            Downtime = Decimal.ToInt32(((NumericUpDown)ct).Value);
                    }
                }
                OleDbParameter[] para = new OleDbParameter[]
                {
                    new OleDbParameter("@TDate",time),
                    new OleDbParameter("@Line",disp),
                    new OleDbParameter("@SubLine",tp.Text),
                    new OleDbParameter("@Target",Target),
                    new OleDbParameter("@Actual",Actual),
                    new OleDbParameter("@Downloss",Downloss),
                    new OleDbParameter("@Downtime",Downtime),
                };
                OleDbCommand cmd = new OleDbCommand(sql, Db_operation.con);
                int num = 0;
                cmd.Parameters.AddRange(para);
                num = cmd.ExecuteNonQuery();
                if (num == 0)
                {
                    ret = false;
                    throw new Exception("Fail to insert items into DB!");
                }
            }
            return ret;
        }

        private bool DbInsertTop5(Panel pl)
        {
            bool ret = false;
            string sql = "insert into Top5 (TDate,Line,SubLine,TNo,Station," +
            "Downtime,Loss,Description) values(@TDate,@Line,@SubLine,@TNo,@Station," +
            "@Downtime,@Loss,@Description)";
            int Loss = 0, Downtime = 0;
            string Station = null,Description = null;
            foreach (Control ct in pl.Controls)
            {
                if (ct.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    if (ct.Name.Split(new char[] { '_' })[1] == "Station")
                        Station = ((TextBox)ct).Text;
                    if (ct.Name.Split(new char[] { '_' })[1] == "Description")
                        Description = ((TextBox)ct).Text;

                }
                if (ct.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
                {
                    if (ct.Name.Split(new char[] { '_' })[1] == "Downtime")
                        Downtime = Decimal.ToInt32(((NumericUpDown)ct).Value);
                    if (ct.Name.Split(new char[] { '_' })[1] == "Loss")
                        Loss = Decimal.ToInt32(((NumericUpDown)ct).Value);
                }
            }
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@TDate",time),
                new OleDbParameter("@Line",disp),
                new OleDbParameter("@SubLine",Station),
                new OleDbParameter("@TNo",Convert.ToInt32(pl.Name.Remove(0,5))),
                new OleDbParameter("@Station",Station),
                new OleDbParameter("@Downtime",Downtime),
                new OleDbParameter("@Loss",Loss),
                new OleDbParameter("@Description",Description)
            };
            OleDbCommand cmd = new OleDbCommand(sql, Db_operation.con);
            int num = 0;
            cmd.Parameters.AddRange(para);
            num = cmd.ExecuteNonQuery();
            if (num == 0)
            {
                ret = false;
                throw new Exception("Fail to insert items into DB!");
            }
            return ret;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CleanTab(tabControl1.SelectedTab);            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            foreach (TabPage tp in tabControl1.TabPages)
            {
                CleanTab(tp);
            }
        }

        private int validcheck()
        {
            int i = 0;
            presult[] checkresult = new presult[5];
            checkresult[0] = checkpanel(panel1);
            checkresult[1] = checkpanel(panel2);
            checkresult[2] = checkpanel(panel3);
            checkresult[3] = checkpanel(panel4);
            checkresult[4] = checkpanel(panel5);
            if (checkresult[0] == presult.NOK)
            {
                MessageBox.Show("Please finish Top5 No1!", "error!");
                return i;
            }
            else if (checkresult[0] == presult.Ini)
            {
                if ((checkresult[1] == presult.Ini) && (checkresult[2] == presult.Ini) && (checkresult[3] == presult.Ini) && (checkresult[4] == presult.Ini))
                {
                    if (MessageBox.Show("Do you want finish without Top5?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        withoutTop5 = true;
                    return i;
                }
                else
                {
                    MessageBox.Show("Please finish Top5 No1 First!", "error!");
                    return i;
                }
            }
            else
            {
                i++;
                if (checkresult[1] == presult.NOK)
                {
                    MessageBox.Show("Please finish Top5 No2!", "error!");
                    return i;
                }
                else if (checkresult[1] == presult.Ini)
                {
                    if ((checkresult[2] == presult.Ini) && (checkresult[3] == presult.Ini) && (checkresult[4] == presult.Ini))
                    {
                        if (MessageBox.Show("Do you want finish only with Top5 No1?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                            i = 0;
                        return i;
                    }
                    else
                    {
                        MessageBox.Show("Please finish Top5 No2 First!", "error!");
                        i = 0;
                        return i;
                    }
                }
                else
                {
                    i++;
                    if (checkresult[2] == presult.NOK)
                    {
                        MessageBox.Show("Please finish Top5 No3!", "error!");
                        return i;
                    }
                    else if (checkresult[2] == presult.Ini)
                    {
                        if ((checkresult[3] == presult.Ini) && (checkresult[4] == presult.Ini))
                        {
                            if (MessageBox.Show("Do you want finish only with Top5 No1 and No2?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                                i = 0;
                            return i;
                        }
                        else
                        {
                            MessageBox.Show("Please finish Top5 No3 First!", "error!");
                            i = 0;
                            return i;
                        }
                    }
                    else
                    {
                        i++;
                        if (checkresult[3] == presult.NOK)
                        {
                            MessageBox.Show("Please finish Top5 No3!", "error!");
                            return i;
                        }
                        else if (checkresult[3] == presult.Ini)
                        {
                            if (checkresult[4] == presult.Ini)
                                return i;
                            else
                            {
                                MessageBox.Show("Please finish Top5 No4 First!", "error!");
                                i = 0;
                                return i;
                            }
                        }
                        else
                        {
                            i++;
                            if (checkresult[4] == presult.NOK)
                            {
                                MessageBox.Show("Please finish Top5 No5!", "error!");
                                return i;
                            }
                            else if (checkresult[4] == presult.Ini)
                                return i;
                            else
                            {
                                i++;
                                return i;
                            }
                        }

                    }
                }
            }
        }

        private bool Insertcheck()
        {
            bool ret = true;
            string sql = "select * from DownTime where TDate = @TDate and Line = @Line";
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@TDate",time),
                new OleDbParameter("@Line",disp)
            };
            OleDbCommand cmd = new OleDbCommand(sql, Db_operation.con);
            cmd.Parameters.AddRange(para);
            if (cmd.ExecuteScalar() == null)
                return false;
            return ret;
        }

        private void CleanTab(TabPage tp)
        {
            foreach (Control ct in tp.Controls)
            {
                if (ct.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
                {
                    ((NumericUpDown)ct).Value = 0;
                }
            }
        }

        private void loadexcel(string[] path)
        {
            string excelstring = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source = '" + path[0] + "';Extended Properties='Excel 12.0;HDR = YES;IMEX=1'";
            OleDbConnection excelcon = new OleDbConnection(excelstring);
            GC.Collect();
            try
            {
                string sql = "select TOP 5 * from [SAP$] order by " + (Excel_operation.columnumber[1] + 1).ToString() + " desc";// order by column" + Excel_operation.columnumber[1].ToString();///////???????
                //OleDbCommand command = new OleDbCommand(sql, excelcon);
                excelcon.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(sql, excelcon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (path.Length != 1)
                {
                    DataSet ds1 = new DataSet();
                    for (int count = 1; count < path.Length; count++)
                    {
                        excelstring = "Provider = Microsoft.ACE.OLEDB.12.0;Data Source = '" + path[count] + "';Extended Properties='Excel 12.0;HDR = YES;IMEX=1'";
                        excelcon = new OleDbConnection(excelstring);
                        da = new OleDbDataAdapter(sql, excelcon);
                        da.Fill(ds1,"table" + count.ToString());
                    }
                    foreach(DataTable dt in ds1.Tables)
                    {
                        foreach(DataRow dr in dt.Rows)
                        {
                            ds.Tables[0].ImportRow(dr);
                        }
                    }
                    ds1.Dispose();
                }
                
                int i = 0;
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = ds.Tables[0].Columns[Excel_operation.columnumber[1]].ColumnName + " Desc";
                DataTable dt2 = dv.ToTable();
                foreach(DataRow dr in dt2.Rows)
                {
                    if (i >= 5)
                        break;
                    if (dr[0].ToString() != "")
                    {
                        //Convert.ToInt32((dr[Excel_operation.columnumber[1]].ToString().Split(new char[]{'.'}))[0]);
                        //Convert.ToDouble(dr[Excel_operation.columnumber[2]]);
                        insertpanel((Panel)findcontrol(this.groupBox2, "panel" + (i + 1).ToString()), dr[Excel_operation.columnumber[0]].ToString(), Convert.ToInt32((Convert.ToDouble(dr[Excel_operation.columnumber[1]])) * 60), Convert.ToInt32(Convert.ToDouble(dr[Excel_operation.columnumber[2]])), dr[Excel_operation.columnumber[3]].ToString());
                        i++;
                    }
                }
                ds.Dispose();
                /*OleDbDataReader ole;
                //ole = command.ExecuteReader();
                int i = 0;
                while (ole.Read() && i < 5)
                {
                    // insertpanel((Panel)findcontrol(this.groupBox2, "panel" + (i + 1).ToString()), ole.GetValue(Excel_operation.columnumber[0]).ToString(), Convert.ToInt32(ole.GetValue(Excel_operation.columnumber[1])), Convert.ToInt32(ole.GetValue(Excel_operation.columnumber[2])), ole.GetValue(Excel_operation.columnumber[3]).ToString());
                    i++;
                }*/

                
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                excelcon.Close();
            }
        }

        private void insertpanel(Panel p, string station, int downtime, int loss, string description)
        {
            foreach (Control ct in p.Controls)
            {
                if (ct.GetType().ToString() != "System.Windows.Forms.Label")
                {
                    switch (ct.Name.Split(new char[] { '_' })[1])
                    {
                        case "Station":
                            ((TextBox)ct).Text = station;
                            break;
                        case "Downtime":
                            ((NumericUpDown)ct).Value = downtime;
                            break;
                        case "Loss":
                            //((NumericUpDown)ct).Value = loss;
                            break;
                        case "Description":
                            ((TextBox)ct).Text = description;
                            break;
                        default: continue;
                    }
                }
            }
        }

        private Control findcontrol(Control control, string controlName)
        {
            foreach (Control ct in control.Controls)
            {
                if (ct.Name == controlName)
                    return ct;
            }
            return null;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Escalation es = new Escalation(time,disp);
            this.Hide();
            if (es.ShowDialog(this) == DialogResult.Cancel)
            {
                this.Show();
            }
        }

        private void LoadfromDB(DateTime dt, string Line)
        {
            LoadPage(dt, Line);
            LoadPanel(dt, Line);
 
        }

        private void LoadPage(DateTime dt,string Line)
        {
            string sql = "select * from DownTime where TDate = @TDate and Line = @Line";
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@TDate",time),
                new OleDbParameter("@Line",disp)
            };
            OleDbCommand cmd = new OleDbCommand(sql, Db_operation.con);
            cmd.Parameters.AddRange(para);
            OleDbDataReader ole;
            ole = cmd.ExecuteReader();
            while (ole.Read())
            {
                foreach (TabPage tp in tabControl1.TabPages)
                {
                    if (tp.Text == ole.GetValue(3).ToString())
                    {
                        foreach (Control ct in tp.Controls)
                        {
                            if (ct.GetType().ToString() == "System.Windows.Forms.NumericUpDown")
                            {
                                if (ct.Name.Split(new char[] { '_' })[1] == "n1")
                                    ((NumericUpDown)ct).Value = Convert.ToInt32(ole.GetValue(4));
                                if (ct.Name.Split(new char[] { '_' })[1] == "n2")
                                    ((NumericUpDown)ct).Value = Convert.ToInt32(ole.GetValue(5));
                                if (ct.Name.Split(new char[] { '_' })[1] == "n3")
                                    ((NumericUpDown)ct).Value = Convert.ToInt32(ole.GetValue(6));
                                if (ct.Name.Split(new char[] { '_' })[1] == "n4")
                                    ((NumericUpDown)ct).Value = Convert.ToInt32(ole.GetValue(7));
                            }
                        }
                        break;
                    }
                    
                }
            }
        }

        private void LoadPanel(DateTime dt, string Line)
        {
            string sql = "select * from Top5 where TDate = @TDate and Line = @Line order by TNo desc";// order by column" + Excel_operation.columnumber[1].ToString();///////???????
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@TDate",time),
                new OleDbParameter("@Line",disp)
            };
            OleDbCommand command = new OleDbCommand(sql, Db_operation.con);
            command.Parameters.AddRange(para);
            OleDbDataReader ole = command.ExecuteReader();

            int i = 0;
            while(ole.Read())
            {
                if (i >= 5)
                    break;
                insertpanel((Panel)findcontrol(this.groupBox2, "panel" + (i + 1).ToString()), ole.GetValue(5).ToString(), Convert.ToInt32(ole.GetValue(6)), Convert.ToInt32(ole.GetValue(7)), ole.GetValue(8).ToString());
                i++;
            }
        }

        private void DeletefromDB(DateTime dt, string Line)
        {
            DeletePage(dt, Line);
            DeletePanel(dt, Line);

        }

        private void DeletePage(DateTime dt, string Line)
        {
            string sql = "delete * from DownTime where TDate = @TDate and Line = @Line";
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@TDate",time),
                new OleDbParameter("@Line",disp)
            };
            OleDbCommand cmd = new OleDbCommand(sql, Db_operation.con);
            cmd.Parameters.AddRange(para);
            if(cmd.ExecuteNonQuery() == 0)
                throw new Exception("System error!");
        }

        private void DeletePanel(DateTime dt, string Line)
        {
            string sql = "delete * from Top5 where TDate = @TDate and Line = @Line";
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@TDate",time),
                new OleDbParameter("@Line",disp)
            };
            OleDbCommand cmd = new OleDbCommand(sql, Db_operation.con);
            cmd.Parameters.AddRange(para);
            if (cmd.ExecuteNonQuery() == 0)
                throw new Exception("System error!");
        }
    }
}
