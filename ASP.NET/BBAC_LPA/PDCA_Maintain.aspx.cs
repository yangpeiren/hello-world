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

public partial class PDCA_Maintain : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection("server=(local);database=BBAC_LPA;uid=sa;pwd=123;");
    SqlDataAdapter da;
    SqlCommand command;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["permission"].ToString() != "1111")
            {
                Response.Redirect("Login.aspx");
            }
            if (Session["user"].ToString() != null)
                this.Label2.Text = Session["user"].ToString();
            else Response.Redirect("Login.aspx");
            GridView1.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            GridView2.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            bind();
            bind2();
        }
    }
    public void bind()
    {
        DateTime st = DateTime.Parse(System.DateTime.Now.Year.ToString() + "-1" + "-1").AddMonths(
            System.DateTime.Now.Month-2);
        DateTime fn = st.AddMonths(2);
        string sql = "select ProblemNO,CheckNO,Pname,FIRST.Name as CreatedBy,convert(varchar(100),CreatedDate,23)as CreatedDate,Depart," +
                "PD,RC,CM,(case when Pstate = '1' then '未关闭' else '已关闭' end) as Pstate,"+ 
                "SECOND.Name as ClosedBy,convert(varchar(100),CloseDate,23)as CloseDate from PDCA join Management FIRST" +
                " on PDCA.CreatedBy = FIRST.ID join Management SECOND on PDCA.ClosedBy = SECOND.ID" +
                " where CreatedDate Between @st and @fn";
        da = new SqlDataAdapter(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@st", st),
            new SqlParameter("@fn", fn)
        };
        try
        {
            da.SelectCommand.Parameters.AddRange(para);
            DataSet ds = new DataSet("Work");
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            da.Fill(ds);
            this.GridView1.DataSource = ds;
            this.GridView1.DataKeyNames = new string[] { "ProblemNO" };
            this.GridView1.DataBind();
        }
        catch (Exception ee)
        {
            Response.Write("<script language='JavaScript'>alert(\"" + ee.Message.ToString() + "\");</script>");
            return;
        }
        finally
        {
            con.Close();
        }
    }
    public void bind2()
    {
        DateTime st = DateTime.Parse(System.DateTime.Now.Year.ToString() + "-1" + "-1").AddMonths(
            System.DateTime.Now.Month - 2);
        DateTime fn = st.AddMonths(2);
        string sql = "select ProblemNO,CheckNO,Pname,Management.Name as CreatedBy,convert(varchar(100),CreatedDate,23)as CreatedDate,Depart," +
                "PD,RC,CM from PDCA join Management on PDCA.CreatedBy = Management.ID" +
                " where Pstate = '1' and CreatedDate Between @st and @fn";
        da = new SqlDataAdapter(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@st", st),
            new SqlParameter("@fn", fn)
        };
        try
        {
            da.SelectCommand.Parameters.AddRange(para);
            DataSet ds = new DataSet("Work");
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            da.Fill(ds);
            this.GridView2.DataSource = ds;
            this.GridView2.DataKeyNames = new string[] { "ProblemNO" };
            this.GridView2.DataBind();
        }
        catch (Exception ee)
        {
            Response.Write("<script language='JavaScript'>alert(\"" + ee.Message.ToString() + "\");</script>");
            return;
        }
        finally
        {
            con.Close();
        }
    }
    protected void GridView_OnRowDeleting(object sender,GridViewDeleteEventArgs e)
    {
        string sql = "delete from PDCA where ProblemNO = @ProblemNO";
        command = new SqlCommand(sql, con);
        command.Parameters.AddWithValue("@ProblemNO", GridView1.DataKeys[e.RowIndex].Value.ToString());
        try
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            if(command.ExecuteNonQuery() != 1)
                Response.Write("<script language='JavaScript'>alert('删除失败！')</script>");
            else Response.Write("<script language='JavaScript'>alert('删除成功！')</script>");
        }
        catch (Exception ee)
        {
            Response.Write("<script language='JavaScript'>alert(\"" + ee.Message.ToString() + "\")</script>");
            return;
        }
        finally
        {
            con.Close();
        }
        bind();
    }
    protected void GridView2_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string sql = "delete from PDCA where ProblemNO = @ProblemNO";
        command = new SqlCommand(sql, con);
        command.Parameters.AddWithValue("@ProblemNO", GridView2.DataKeys[e.RowIndex].Value.ToString());
        try
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            if (command.ExecuteNonQuery() != 1)
                Response.Write("<script language='JavaScript'>alert('删除失败！')</script>");
            else Response.Write("<script language='JavaScript'>alert('删除成功！')</script>");
        }
        catch (Exception ee)
        {
            Response.Write("<script language='JavaScript'>alert(\"" + ee.Message.ToString() + "\")</script>");
            return;
        }
        finally
        {
            con.Close();
        }
        bind2();
    }
    protected void GridView_OnRowEditing(object sender,GridViewEditEventArgs e)
    {
        this.GridView1.EditIndex = e.NewEditIndex;
        bind();
    }
    protected void GridView2_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        this.GridView2.EditIndex = e.NewEditIndex;
        bind2();
    }
    protected void GridView_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string sql = "update PDCA set Pname = @Pname,CreatedDate = @CreatedDate,Depart = @Depart,PD = "
            + "@PD,RC = @RC,CM = @CM where ProblemNO = @ProblemNO";
        command = new SqlCommand(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@Pname", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[2].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@CreatedDate", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[4].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@Depart", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[5].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@PD", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[6].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@RC", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[7].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@CM", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[8].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@ProblemNO", GridView1.DataKeys[e.RowIndex].Value.ToString())
        };
        try
        {
            da.SelectCommand.Parameters.AddRange(para);
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            if (command.ExecuteNonQuery() != 1)
            {
                Response.Write("<script language='JavaScript'>alert('更新失败！')</script>");
                return;
            }
            else
            {
                Response.Write("<script language='JavaScript'>alert('更新成功！')</script>");
            }
            con.Close();
            this.GridView1.EditIndex = -1;
        }
        catch (Exception ee)
        {
            Response.Write("<script language='JavaScript'>alert(\"" + ee.Message.ToString() + "\")</script>");
            return;
        }
        bind();
    }
    
    protected void GridView2_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string sql = "update PDCA set Pname = @Pname,CreatedDate = @CreatedDate,Depart = @Depart,PD = "
            + "@PD,RC = @RC,CM = @CM where ProblemNO = @ProblemNO";
        command = new SqlCommand(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@Pname", ((TextBox)(GridView2.Rows[e.RowIndex].Cells[2].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@CreatedDate", ((TextBox)(GridView2.Rows[e.RowIndex].Cells[4].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@Depart", ((TextBox)(GridView2.Rows[e.RowIndex].Cells[5].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@PD", ((TextBox)(GridView2.Rows[e.RowIndex].Cells[6].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@RC", ((TextBox)(GridView2.Rows[e.RowIndex].Cells[7].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@CM", ((TextBox)(GridView2.Rows[e.RowIndex].Cells[8].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@ProblemNO", GridView2.DataKeys[e.RowIndex].Value.ToString())
        };
        try
        {
            command.Parameters.AddRange(para);
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            if (command.ExecuteNonQuery() != 1)
            {
                Response.Write("<script language='JavaScript'>alert('更新失败！')</script>");
                return;
            }
            else
            {
                Response.Write("<script language='JavaScript'>alert('更新成功！')</script>");
            }
            con.Close();
            this.GridView2.EditIndex = -1;
        }
        catch (Exception ee)
        {
            Response.Write("<script language='JavaScript'>alert(\"" + ee.Message.ToString() + "\")</script>");
            return;
        }
        bind2();
    }
    protected void GridView_OnRowCancelingEdit(object sender,GridViewCancelEditEventArgs e)
    {
        this.GridView1.EditIndex = -1;
        bind();
    }
    protected void GridView2_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.GridView2.EditIndex = -1;
        bind2();
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "change")
        {
            string id = e.CommandArgument.ToString();
            string petID = this.GridView1.DataKeys[Convert.ToInt32(id)].Value.ToString();
            string sql = "update PDCA set Pstate = '1',ClosedBy = null,CloseDate = null where ProblemNO = @ID";
            command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@ID",petID);
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                if (command.ExecuteNonQuery() != 1)
                {
                    Response.Write("<script language='JavaScript'>alert('重置失败！')</script>");
                    return;
                }
                else
                {
                    Response.Write("<script language='JavaScript'>alert('操作已成功！')</script>");
                }
                con.Close();    
            }
            catch (Exception ee)
            {
                Response.Write("<script language='JavaScript'>alert(\"" + ee.Message.ToString() + "\")</script>");
                return;
            }
        }        
        bind();        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("maintain_route.aspx");
    }
}
