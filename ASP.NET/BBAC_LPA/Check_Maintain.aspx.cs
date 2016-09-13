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

public partial class Check_Maintain : System.Web.UI.Page
{
    static SqlConnection con = new System.Data.SqlClient.SqlConnection("server=(local);database=BBAC_LPA;uid=sa;pwd=123;");
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
            bind();
        }
    }
    public void bind()
    {
        if (Session["ShopName"].ToString() == null)
        {
            Response.Write("<script language='JavaScript'>alert('您不能维护此表！');</script>");
            return;
        }
        DateTime st = System.DateTime.Now.AddDays(-14);
        DateTime fn = System.DateTime.Now;
        string sql = "select CheckNO,Checkup.WorkSymbol,Checker,CheckerName,CheckerID,ID,convert(varchar(100),Checktime,23)as Checktime from Checkup join Work on Checkup.WorkSymbol = Work.WorkSymbol" +
                " where Checktime Between @st and @fn and ShopName = '" + Session["ShopName"] + "'";
        da = new SqlDataAdapter(sql, con);
        da.SelectCommand.Parameters.AddWithValue("@st", st);
        da.SelectCommand.Parameters.AddWithValue("@fn", fn);
        try
        {
            DataSet ds = new DataSet("Work");
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            da.Fill(ds);
            this.GridView1.DataSource = ds;
            this.GridView1.DataKeyNames = new string[] { "CheckNO" };
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
    protected void Gridview1_RowDeleteing(object sender, GridViewDeleteEventArgs e)
    {
        string sql = "delete from Checkup where CheckNO = @CheckNO";
        command = new SqlCommand(sql, con);
        command.Parameters.AddWithValue("@CheckNO", GridView1.DataKeys[e.RowIndex].Value.ToString());
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
        bind();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("maintain_route.aspx");
    }
}
