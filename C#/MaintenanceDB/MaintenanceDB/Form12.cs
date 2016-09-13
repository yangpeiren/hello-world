using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.OleDb;
using System.Xml;

namespace MaintenanceDB
{
    public partial class Form12 : Form
    {

        public Form12(string sql, SeriesChartType st, string xval, string yval)
        {
            InitializeComponent();

            OleDbCommand cmd = new OleDbCommand(sql, Form1.con);
            OleDbDataReader dr;
            try
            {
                if (Form1.con.State == ConnectionState.Open)
                    Form1.con.Close();
                Form1.con.Open();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return;
            }

            chart1.Series["Series1"].Points.DataBindXY(dr, xval, dr, yval);
            // Set Doughnut chart type
            if (st == SeriesChartType.Pie)
            {
                chart1.Series["Series1"].ChartType = SeriesChartType.Pie;

                // Set labels style
                chart1.Series["Series1"]["PieLabelStyle"] = "Outside";

                // Set Doughnut radius percentage
                chart1.Series["Series1"]["DoughnutRadius"] = "30";

                // Explode data point with label "Italy"
                //chart1.Series["Series1"].Points[]["Exploded"] = "true";

                // Enable 3D
                chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

                // Set drawing style
                chart1.Series["Series1"]["PieDrawingStyle"] = "SoftEdge";
                chart1.ChartAreas["ChartArea1"].BackColor = Color.Transparent;
            }
            else
            {
                chart1.Series["Series1"].ChartType = SeriesChartType.Column;
                chart1.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;

                // Enable 3D charts
                chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

                // Set Rotation angles
                chart1.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 30;
                chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 10;

                // Disable/enable right angle axis
                chart1.ChartAreas["ChartArea1"].Area3DStyle.IsRightAngleAxes = false;

                chart1.Series["Series1"]["DrawingStyle"] = "Cylinder";
                chart1.Series["Series1"].LegendText = "单位:分钟";
                chart1.Series["Series1"].Label = "#VAL";
                chart1.ChartAreas["ChartArea1"].BackColor = Color.White;

            }
            chart1.Series["Series1"].ToolTip = "#PERCENT";
            chart1.Series["Series1"].LabelToolTip = "停机总时间为: #VAL 分钟";
            chart1.Series["Series1"].LegendToolTip = "停机总时间为: #VAL 分钟";

            chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart1.BorderDashStyle = ChartDashStyle.Solid;
            chart1.BorderWidth = 2;
            chart1.BorderColor = Color.MidnightBlue;
            chart1.BackSecondaryColor = Color.Gainsboro;
            chart1.BackGradientStyle = GradientStyle.LeftRight;

            chart1.Legends["Legend1"].BackColor = Color.Transparent;
        }
    }
}
