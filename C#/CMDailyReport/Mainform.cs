using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace CMDailyReport
{
    public partial class Mainform : Form
    {
        private DateTime dt;
        private string filename;
        private string filename_partly;

        public Mainform()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            Log.ExePossess();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GC.Collect();
            Log.ExePossessEnd();
            System.Windows.Forms.Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            (new Login()).ShowDialog(this);
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //DateTime dt;
            Date_Select ds = new Date_Select();
            try
            {
                if (ds.ShowDialog() == DialogResult.OK)
                {
                    this.dt = ds.ret.Date;
                }
                else
                {
                    return;
                }
                this.filename = Excel_operation.ReportPath + "CM_DailyReport_" + this.dt.ToString("yyyy-MM-dd") + ".xls";
                this.filename_partly = Excel_operation.ReportPath + "CM_DailyReport_" + this.dt.ToString("yyyy-MM-dd") + "_Partly.xls";
                if (Existcheck(filename))
                {   
                    OpenExcel(filename);
                }
                else
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                    //MessageBox.Show("Start Exporting report...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    worker.RunWorkerAsync();
                    WaitForm frm = new WaitForm(worker);
                    frm.ShowDialog(this);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("To be continued...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //return;
            (new inquire()).ShowDialog();
        }

        private void OpenExcel(string filename)
        {
            if(!Existcheck(filename))
                return;
            Log.FilePossess(filename);
            System.Diagnostics.Process.Start(filename);
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out   int ID);     
        public bool CheckOpen(string fileName)
        {
            bool inUse = true;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                fs.Close();
                inUse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return inUse;
        }

        private bool Existcheck(string filename)
        {
            if(File.Exists(filename))
                return true;
            return false;
        }

        private bool CheckDB(DateTime dt)
        {
            bool ret = false;
            try
            {
                string sql = "select distinct iif((SELECT count(ShopName) from (select distinct ShopName from Subline)) = (SELECT count(Line) from (select distinct Line from DownTime where TDate = @TDate)),'true','false') as result from Subline";
                OleDbCommand command = new OleDbCommand(sql, Db_operation.con);
                command.Parameters.Add(new OleDbParameter("@TDate", dt));
                Db_operation.con.Open();
                OleDbDataReader ole;
                ole = command.ExecuteReader();
                if (ole.HasRows)
                {
                    ole.Read();
                    ret = Convert.ToBoolean(ole.GetValue(0));
                }
                else
                {
                    throw new Exception("System error!");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Db_operation.con.Close();
            }
            return ret;
        }

        private void Export(string filename,DateTime dt)
        {
            
            try
            {
                string sql = "Select PosInExcel,Sequence,Target,Actual,(Actual - Target) as Loss,(0 - Downloss) as DLoss,Downtime from Downtime inner join Subline on Downtime.Line = Subline.ShopName and Downtime.SubLine = Subline.LineName where TDate = @TDate";
                OleDbDataAdapter da = new OleDbDataAdapter(sql, Db_operation.con);
                //OleDbParameter op = new OleDbParameter("@TDate", dt);
                da.SelectCommand.Parameters.Add(new OleDbParameter("@TDate", dt));
                //DataSet ds = new DataSet();
                System.Data.DataTable table1 = new System.Data.DataTable();
                System.Data.DataTable table2 = new System.Data.DataTable();
                System.Data.DataTable table3 = new System.Data.DataTable();
                da.Fill(table1);

                sql = "Select PosInExcel,TNo,Station,Downtime,Description,Loss from Top5 inner join Subline on Top5.Line = Subline.ShopName where TDate = @TDate and Subline.Sequence = 1 order by PosInExcel,TNo";
                da = new OleDbDataAdapter(sql, Db_operation.con);
                da.SelectCommand.Parameters.Add(new OleDbParameter("@TDate", dt));
                da.Fill(table2);

                sql = "select TNo,ShopName,Content,Memory from Escalation where TDate = @TDate order by TNo ASC";
                da = new OleDbDataAdapter(sql, Db_operation.con);
                da.SelectCommand.Parameters.Add(new OleDbParameter("@TDate", dt));
                da.Fill(table3);

                File.Copy(Excel_operation.TemplatePath, filename, true);

                _Application excel;
                GC.Collect();
                _Workbook xBk;
                _Worksheet xSt;

                excel = new ApplicationClass();
                //excel.Visible = true;
                xBk = excel.Workbooks.Open(filename, 0, false, 5, "", "123", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, ";", true, false, 0, true, null, null);
                xSt = (_Worksheet)xBk.Sheets.get_Item("Daily Production Status");

                InsertDownTime(xSt, table1);
                InsertTop5(xSt, table2);
                InsertEscalation(xSt, table3);
                xSt.Protect("123", Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, true, Type.Missing, Type.Missing);
                xBk.Save();
                xBk.Close(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                excel.Quit(); 
                IntPtr t = new IntPtr(excel.Hwnd);
                int k = 0;
                GetWindowThreadProcessId(t, out k);
                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
                p.Kill();
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Db_operation.con.Close();
            }
        }

        private int ColumnToIndex(string column)
        {
            if (!Regex.IsMatch(column.ToUpper(), @"[A-Z]+"))
            {
                throw new Exception("Invalid parameter");
            }
            int index = 0;
            char[] chars = column.ToUpper().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                index += ((int)chars[i] - (int)'A' + 1) * (int)Math.Pow(26, chars.Length - i - 1);
            }
            return index;
        }

        private void InsertDownTime(_Worksheet ws, System.Data.DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No data of DownTime", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                int col;
                int row;
                foreach (DataRow dr in dt.Rows)
                {
                    col = ColumnToIndex(Regex.Replace(dr["PosInExcel"].ToString(), @"\d", ""));
                    row = Convert.ToInt32(Regex.Replace(dr["PosInExcel"].ToString(), @"[^\d]*", "")) + Convert.ToInt32(dr["Sequence"].ToString()) - 1;
                    ws.Cells[row, col] = dr["Target"].ToString();
                    ws.Cells[row, col + 1] = dr["Actual"].ToString();
                    ws.Cells[row, col + 2] = dr["Loss"].ToString();
                    ws.Cells[row, col + 3] = dr["DLoss"].ToString();
                    ws.Cells[row, col + 4] = dr["Downtime"].ToString();
                }
            }
        }

        private void InsertTop5(_Worksheet ws, System.Data.DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No data of Top5", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                int col;
                int row;
                foreach (DataRow dr in dt.Rows)
                {
                    col = ColumnToIndex(Regex.Replace(dr["PosInExcel"].ToString(), @"\d", "")) + 5;
                    row = Convert.ToInt32(Regex.Replace(dr["PosInExcel"].ToString(), @"[^\d]*", "")) + Convert.ToInt32(dr["TNo"].ToString()) - 1;
                    ws.Cells[row, col] = dr["TNo"].ToString();
                    ws.Cells[row, col + 1] = dr["Station"].ToString();
                    ws.Cells[row, col + 2] = dr["Downtime"].ToString();
                    ((Range)ws.Cells[row, col + 3]).WrapText = true;
                    if (dr["Downtime"].ToString().Length > 60 && dr["Downtime"].ToString().Length <= 72)
                        ((Range)ws.Cells[row, col + 3]).Font.Size = 14;
                    else if (dr["Downtime"].ToString().Length > 72)
                        ((Range)ws.Cells[row, col + 3]).Font.Size = 12;
                    ws.Cells[row, col + 3] = dr["Description"].ToString();
                    ws.Cells[row, col + 4] = dr["Loss"].ToString();
                }
            }
        }

        private void InsertEscalation(_Worksheet ws, System.Data.DataTable dt)
        {
            if (dt.Rows.Count == 0)
            {
                //MessageBox.Show("No data of Escalation", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dt.Rows.Count > Escalation.MaxEscalation)
            {
                MessageBox.Show("Not all escalations will be export", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                int col;
                int row;
                foreach (DataRow dr in dt.Rows)
                {
                    col = ColumnToIndex(Regex.Replace(Excel_operation.EscalationPath, @"\d", ""));
                    row = Convert.ToInt32(Regex.Replace(Excel_operation.EscalationPath, @"[^\d]*", "")) + Convert.ToInt32(dr["TNo"].ToString()) - 1;
                    ws.Cells[row, col] = dr["TNo"].ToString();
                    ws.Cells[row, col + 1] = dr["ShopName"].ToString();
                    ws.Cells[row, col + 2] = dr["Content"].ToString();
                    ((Range)ws.Cells[row, col + 2]).WrapText = true;
                    if (dr["Content"].ToString().Length > 60 && dr["Content"].ToString().Length <= 72)
                        ((Range)ws.Cells[row, col + 2]).Font.Size = 14;
                    else if (dr["Content"].ToString().Length > 72)
                        ((Range)ws.Cells[row, col + 2]).Font.Size = 12;
                    ws.Cells[row, col + 3] = dr["Memory"].ToString();
                    ((Range)ws.Cells[row, col + 3]).WrapText = true;
                    if (dr["Memory"].ToString().Length > 60 && dr["Memory"].ToString().Length <= 72)
                        ((Range)ws.Cells[row, col + 3]).Font.Size = 14;
                    else if (dr["Memory"].ToString().Length > 72)
                        ((Range)ws.Cells[row, col + 3]).Font.Size = 12;
                }
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (CheckDB(dt))
                {
                    Export(filename, dt);
                    OpenExcel(filename);
                }
                else
                {
                    if (Existcheck(filename_partly))
                    {
                        if (!CheckOpen(filename_partly))
                        {
                            Export(filename_partly, dt);
                            OpenExcel(filename_partly);
                        }
                        else
                        {
                            OpenExcel(filename_partly);
                        }
                    }
                    else
                    {
                        Export(filename_partly, dt);
                        OpenExcel(filename_partly);
                    }
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}
