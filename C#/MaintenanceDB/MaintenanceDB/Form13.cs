using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Office.Interop.Excel;
namespace MaintenanceDB
{
    public partial class Form13 : Form
    {
        private static int MAXROW = 30;
        public static string getexcelpath()
        {
            string str;
            IniFile ini = new IniFile(System.Windows.Forms.Application.StartupPath + @"\Config.ini");
            str = ini.ReadString("config", "Excelpath");
            return str;
        }

        public Form13()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Today.AddHours(8);
            dateTimePicker2.Value = DateTime.Today.AddHours(20);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "SELECT 所属区域,SUM(CInt(datediff(\"n\",维修记录.停机开始时间,维修记录.停机结束时间))) as 时间 " +
                "from 维修记录 inner join 设备 on 维修记录.故障设备编号 = 设备.设备编号 where 停机开始时间 between #" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "# and #" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "# group by 所属区域";
            Form12 form = new Form12(sql, SeriesChartType.Pie, "所属区域", "时间");
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "SELECT 故障类型,SUM(CInt(datediff(\"n\",维修记录.停机开始时间,维修记录.停机结束时间))) as 时间 " +
                "from 维修记录 where 停机开始时间 between #" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "# and #" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "# group by 故障类型";
            Form12 form = new Form12(sql, SeriesChartType.Pie, "故障类型", "时间");
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string type = null;
            string xval = null;
            switch (comboBox1.SelectedItem.ToString())
            {
                case "请选择":
                    MessageBox.Show("请选择时间粒度！", "错误！");
                    return;
                case "天":
                    type = "day(停机开始时间)";
                    xval = "CStr(day(停机开始时间)) + \"号\" as val,";
                    break;
                case "周":
                    type = "datepart(\"ww\",停机开始时间)";
                    xval = "\"第\" + CStr(datepart(\"ww\", 停机开始时间)) + \"周\" as val,";
                    break;
                case "月":
                    type = "month(停机开始时间)";
                    xval = "CStr(month(停机开始时间)) + \"月\" as val,";
                    break;
                case "年":
                    type = "year(停机开始时间)";
                    xval = "CStr(year(停机开始时间)) + \"年\" as val,";
                    break;
                default:
                    type = null;
                    break;
            }
            string sql = "SELECT " + xval + "SUM(CInt(datediff(\"n\",维修记录.停机开始时间,维修记录.停机结束时间))) as 时间 " +
                "from 维修记录 where 停机开始时间 between #" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "# and #" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "# and year(停机开始时间) >= '2010' group by " + type;
            Form12 form = new Form12(sql, SeriesChartType.Column, "val", "时间");
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sql = "SELECT 故障类型, SUM(CInt(datediff(\"n\",维修记录.停机开始时间,维修记录.停机结束时间))) AS 时间 " +
                "from 维修记录 where 停机开始时间 between #" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "# and #" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "# group by 维修记录.故障类型";
            Form12 form = new Form12(sql, SeriesChartType.Pie, "故障类型", "时间");
            form.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataSet dr = new DataSet();
            binddata(dr);
            OutputExcel(dr);

        }
        public void OutputExcel(DataSet ds)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            GC.Collect();
            //int rowIndex = 4;
            //t colIndex = 2;

            _Workbook xBk;
            _Worksheet xSt;
            #region generate source and delete blank columns
            excel = new ApplicationClass();
            //excel.Visible = true;
            xBk = excel.Workbooks.Open(getexcelpath(), 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, ";", true, false, 0, true, null, null);
            try
            {
                xSt = (_Worksheet)xBk.Sheets.get_Item("Z3");
                for(int r=0;r<ds.Tables[0].Rows.Count;r++)
                {
                    for(int i=0;i<ds.Tables[0].Columns.Count;i++)
                    {
                        xSt.Cells[r+4,i+3]=ds.Tables[0].Rows[r][i].ToString();     
                    }
                }
                xSt.Cells[34, 11] = System.DateTime.Today.ToString("yyyy-MM-dd");
                if (ds.Tables[0].Rows.Count > MAXROW)
                    throw new Exception("本次导出数据量过大，请注意导出的Excel没有屁股!");
                else
                {
                    for (int i = ds.Tables[0].Rows.Count; i < MAXROW; i++)
                        DeleteRows(xSt, ds.Tables[0].Rows.Count + 4);
                }                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "警告");
            }
            #endregion
            #region save file and quit excel
            saveFileDialog1.Filter = "Excel文件(*.xls)|*.xls";
            saveFileDialog1.FileName = System.Windows.Forms.Application.StartupPath + @"\data\205-Daily_report" 
                + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                xBk.SaveAs(saveFileDialog1.FileName, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, XlSaveAsAccessMode.xlNoChange, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                xBk.Close(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                excel.Quit();
                
            }
            #endregion
        }

        private void DeleteRows(_Worksheet sheet, int rowIndex)
        {
            Microsoft.Office.Interop.Excel.Range range = (Range)sheet.Rows[rowIndex, System.Reflection.Missing.Value];
            //range.Select();
            range.EntireRow.Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftUp);
        }

        void binddata(DataSet ds)
        {
            string sql = "select 设备.所属区域,设备.所属工位,设备.设备名称,维修记录.故障描述,维修记录.解决办法," +
                "(iif(是否更换备件,备件.备件名称,'否')) as 更换备件,format(维修记录.停机开始时间,\"HH:mm\") as 开始时间,format(维修记录.停机结束时间,\"HH:mm\") as 结束时间," +
                "datediff(\"n\",维修记录.停机开始时间,维修记录.停机结束时间) as 时长,故障频次,设备工况,维修人员姓名,\"KUKA\" as 责任方 " +
                "from 维修记录,设备,备件 where 维修记录.故障设备编号 = 设备.设备编号 and 维修记录.备用零件号 = 备件.备用零件号 and 停机开始时间 between @开始 and @结束 order by 维修记录.停机开始时间 asc";
            OleDbDataAdapter da = new OleDbDataAdapter(sql, Form1.con);
            //ds = new DataSet();

            #region Time control for which shift is working

            /*DateTime start = new DateTime();
            DateTime end = new DateTime();

            if (DateTime.Now >Form6.thisNight)
            {


            }
            else if (DateTime.Now > Form6.thisMorning && DateTime.Now < Form6.thisNight)
            {

            }
            else
            {

            }*/

            #endregion
            OleDbParameter[] para = new OleDbParameter[]
            {
                new OleDbParameter("@开始",dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")),
                new OleDbParameter("@结束",dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss"))
            };
            try
            {
                da.SelectCommand.Parameters.AddRange(para);
                da.Fill(ds);
                if (ds.Tables[0].Columns.Count == 0)
                    throw new Exception("没有数据！");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[1].ToString() == "")
                        dr.Delete();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "错误");
            }
            //BindingSource dataSource = new BindingSource(ds.Tables[0], null);
           
        }
    }
}
