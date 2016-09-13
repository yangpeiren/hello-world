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

public partial class PDCA_Detailview : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection("server=(local);database=BBAC_LPA;uid=sa;pwd=123;");
    SqlDataAdapter da;
    SqlCommand command;
    protected void Page_Load(object sender, EventArgs e)
    {
        DetailsView1.Attributes.Add("style","word-break:break-all;word-wrap:break-word");
        if (Session["petID"].ToString() == null)
        {
            Response.Write("<script language='JavaScript'>alert('Session错误，请重新登录\\n若错误仍然存在，请联系管理员！')</script>");
        }
        else
        {
            bind();
        }
    }
    protected void DetailsView1_ModeChaning(object sender, DetailsViewModeEventArgs e)
    {
        if (Label5.Text == "已关闭")
            return;
        this.DetailsView1.ChangeMode(e.NewMode);
        bind();
    }
    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        string PD = Request.Form["S1"];
        string RC = Request.Form["S2"];
        string CM = Request.Form["S3"];
        string sql = "update PDCA set PD = @PD,RC = @RC,CM = @CM where ProblemNO = @ProblemNO";
        command = new SqlCommand(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@PD", PD),
            new SqlParameter("@RC", RC),
            new SqlParameter("@CM", CM),
            new SqlParameter("@ProblemNO", Session["petID"])
        };
        try
        {
            da.SelectCommand.Parameters.AddRange(para);
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            int num = command.ExecuteNonQuery();
            if (num != 1)
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "info",
              "alert('内部错误，更新失败！');", true);
            else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "info",
              "alert('修改成功！');", true);
        }
        catch (Exception)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "info",
              "alert('内部错误，更新失败，请联系管理员！');", true);
            return;
        }
        finally
        {
            con.Close();
            this.DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            bind();
        }
    }
    public void bind()
    {
        string sql = "select ProblemNO,CheckNO,Pname,Management.Name as CreatedBy,convert(varchar(100),CreatedDate,23)as CreatedDate,Depart," +
                "PD,RC,CM,(case when Pstate = '1' then '未关闭' else '已关闭' end) as Pstate,"
                + "ClosedBy,convert(varchar(100),CloseDate,23)as CloseDate from PDCA join Management"+
                " on PDCA.CreatedBy = Management.ID" +
                " where ProblemNO = @ProblemNO ";
        da = new SqlDataAdapter(sql, con);
        da.SelectCommand.Parameters.AddWithValue("@ProblemNO", Session["petID"]);
        try
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            DataSet ds = new DataSet("Work");
            da.Fill(ds);
            Label5.Text = ds.Tables[0].Rows[0][9].ToString();
            if (ds.Tables[0].Rows[0][9].ToString() == "已关闭")
            {
                Button1.Enabled = false;
                sql = "select ProblemNO,CheckNO,Pname,FIRST.Name as CreatedBy,convert(varchar(100),CreatedDate,23)as CreatedDate,Depart," +
                "PD,RC,CM,(case when Pstate = '1' then '未关闭' else '已关闭' end) as Pstate,"
                + "SECOND.Name as ClosedBy,convert(varchar(100),CloseDate,23)as CloseDate from PDCA join Management FIRST" +
                " on PDCA.CreatedBy = FIRST.ID join Management SECOND on PDCA.ClosedBy = SECOND.ID" +
                " where ProblemNO = @ProblemNO ";
                da = new SqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("@ProblemNO", Session["petID"]);
                da.Fill(ds,"second");
                this.DetailsView1.DataSource = ds.Tables["second"];
            }
            else this.DetailsView1.DataSource = ds;
            this.DetailsView1.DataBind();
            
        }
        catch (Exception ee)
        {
            Response.Write("<script language='JavaScript'>alert(\""+ee.Message+"\")</script>");
        }
        finally
        {
            con.Close();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string sql = "update PDCA set Pstate = '0',ClosedBy = '" + Session["userid"]
            +"',CloseDate = '"+ System.DateTime.Now +"'  where ProblemNO = '" + Session["petID"] + "'";
        command = new SqlCommand(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@ClosedBy", Session["userid"]),
            new SqlParameter("@CloseDate", System.DateTime.Now),
            new SqlParameter("@ProblemNO", Session["petID"])
        };
        try
        {
            da.SelectCommand.Parameters.AddRange(para);
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            int num = command.ExecuteNonQuery();
            if (num != 1)
                Response.Write("<script language='JavaScript'>alert('发生内部错误，PDCA关闭失败！')</script>");
            else
                Response.Write("<script language='JavaScript'>alert('PDCA关闭成功！')</script>");
        }
        catch(Exception ee)
        {
            Response.Write("<script language='JavaScript'>alert(\"" + ee.Message + "\")</script>");
        }
        finally
        {
            con.Close();
            bind();
        }

    }
}
