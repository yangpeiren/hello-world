using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data.OleDb;

public partial class Create_PDCA : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection("server=(local);database=BBAC_LPA;uid=sa;pwd=123;");
    SqlCommand cmd;
    SqlDataReader sdr;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("Login.aspx");
        Label5.Text = Session["CheckNo"].ToString();
        Label17.Text = Session["WorkSymbol"].ToString();
        Label8.Text = Session["CheckerName"].ToString();
        Label12.Text = Convert.ToDateTime(Session["Checktime"].ToString()).ToString("yyyy-MM-dd");
        string sql = "select ShopName,GroupName,TeamName from Management where ID = @ID";
        cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@ID", Session["ChckerID"]);
        try
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            sdr = cmd.ExecuteReader();
            string show = null;
            if (sdr.Read())
            {
                show = sdr.GetSqlValue(0).ToString();
                show += sdr.GetSqlValue(1).ToString();
                show += sdr.GetSqlValue(2).ToString();
                if (show == "")
                    show = "GM";
            }
            Label10.Text = show;
        }
        catch (Exception ee)
        {
            Response.Write("<script language='JavaScript'>alert('" + ee.Message + "！')</script>");
            return;
        }
        finally
        {
            con.Close();
        }
        GridView1.Attributes.Add("style", "word-break:keep-all;word-wrap:normal");
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Label18.Visible = false;
        this.ExcelToDS(Label17.Text);
    }
    public void ExcelToDS(string Path)
    {
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Server.MapPath("~/Work/" + Path + ".xls;")
                            + "Extended Properties='Excel 8.0;HDR = No;IMEX=1'"; 
        OleDbConnection conn = new OleDbConnection(strConn);
        try
        {
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [sheet1$]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            DataTable dt = ds.Tables["table1"];
            this.GridView1.DataSource = dt;
            this.GridView1.DataBind();
            int count = 0;
            int num = 0;
            foreach (DataRow dr in dt.Rows)
            {
                bool flag = true;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!dr.IsNull(i))
                        flag = false;
                }
                if (flag)
                    num++;
                else
                {
                    GridView1.Rows[count].Cells[0].Text = "<B><Center>" + GridView1.Rows[count].Cells[0].Text + "</Center></B>";
                    GridView1.Rows[count].Cells[2].Text = "<B><Center>" + GridView1.Rows[count].Cells[2].Text + "</Center></B>";
                    GridView1.Rows[count].Cells[5].Text = "<B><Center>" + GridView1.Rows[count].Cells[5].Text + "</Center></B>";
                }
                if (num >= 2)
                {
                    GridView1.Rows[count + 1].Cells[0].Text = "<B><Center>" + GridView1.Rows[count + 1].Cells[0].Text + "</Center></B>";
                    GridView1.Rows[count + 1].Cells[1].Text = "<B><Center>" + GridView1.Rows[count + 1].Cells[1].Text + "</Center></B>";
                    GridView1.Rows[count + 1].Cells[2].Text = "<B><Center>" + GridView1.Rows[count + 1].Cells[2].Text + "</Center></B>";
                    GridView1.Rows[count + 1].Cells[3].Text = "<B><Center>" + GridView1.Rows[count + 1].Cells[3].Text + "</Center></B>";
                    GridView1.Rows[count + 1].Cells[4].Text = "<B><Center>" + GridView1.Rows[count + 1].Cells[4].Text + "</Center></B>";
                    GridView1.Rows[count + 1].Cells[5].Text = "<B><Center>" + GridView1.Rows[count + 1].Cells[5].Text + "</Center></B>";
                    GridView1.Rows[count + 1].Cells[6].Text = "<B><Center>" + GridView1.Rows[count + 1].Cells[6].Text + "</Center></B>";
                    break;
                }
                count++;
            }
        }
        catch (System.Data.OleDb.OleDbException)
        {
            Label18.Visible = true;
        }
        finally
        {
            conn.Close();        //关闭连接        
        }

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("Check.aspx");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string PD = Request.Form["S1"];
        string RC = Request.Form["S2"];
        string CM = Request.Form["S3"];
        if (PD.Length == 0 || RC.Length == 0 || CM.Length == 0)
            Response.Write("<script language='JavaScript'>alert('问题描述/问题原因/应对措施 必须填写完整！')</script>");
        else
        {
            string sql = "insert into PDCA (CheckNO,Pname,CreatedBy,CreatedDate,Depart,PD,RC,CM)" +
            "values(@CheckNO,@Pname,@CreatedBy,@CreatedDate,@Depart,@PD,@RC,@CM); select @@identity";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@CheckNO", Label5.Text), 
                new SqlParameter("@Pname", TextBox1.Text),
                new SqlParameter("@CreatedBy", Session["ChckerID"]),
                new SqlParameter("@CreatedDate", Session["Checktime"]),
                new SqlParameter("@Depart", Label10.Text),
                new SqlParameter("@PD", PD),
                new SqlParameter("@RC",RC),
                new SqlParameter("@CM", CM)
            };
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddRange(para);
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                int num = Convert.ToInt32(cmd.ExecuteScalar());
                if (num <= 0)
                    Response.Write("<script language='JavaScript'>alert('PDCA创建失败！')</script>");
                else
                {
                    Response.Write("<script language='JavaScript'>alert('PDCA创建成功！\\n\\nPDCA编号为: " + num +
                        "');window.location.href = 'Check.aspx';</script>");
                }

            }
            catch (Exception ee)
            {
                Response.Write("<script language='JavaScript'>alert(\"" + ee.Message + "\")</script>");
            }
            finally
            {
                con.Close();
            }

        }


    }
}
