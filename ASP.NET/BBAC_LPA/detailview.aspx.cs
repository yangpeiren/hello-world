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


public partial class detailview : System.Web.UI.Page
{
    char[] separator = { ',' };
    public string[] myword;
    protected void operate(int ID1, int ID2,string S1,string S2,string S3)
    {
        
        SqlConnection con = new SqlConnection("server=localhost;database=BBAC_LPA;uid=sa;pwd=123;");
        string sql = "select code,checkup.worksymbol,checker,checkername,convert(varchar(100),checktime,23)as checktime from checkup " +
         "join work on checkup.worksymbol = work.worksymbol " +
         "where shopname = @shopname and groupname = @groupname and teamname = @teamname " +
         "and (ID = @ID1 or ID = @ID2 and Checktime between @st and @fn)";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@shopname", S1),
            new SqlParameter("@groupname", S2),
            new SqlParameter("@teamname", S3),
            new SqlParameter("@ID1", ID1),
            new SqlParameter("@ID2", ID2),
            new SqlParameter("@st", Session["begin"]),
            new SqlParameter("@fn", Session["end"])
        };
        da.SelectCommand.Parameters.AddRange(para);
        DataSet ds = new DataSet("Work");
        da.Fill(ds);
        GridView1.DataSource = ds;
        GridView1.DataBind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("Login.aspx");
        bind();
    }
    /*protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        bind();
    }*/
    public void bind()
    {
        if (Session["TLClick"] != null)
        {
            string s = Session["TLClick"].ToString();
            myword = s.Split(separator);
            if (Session["Group"] != null)
            {
                if (Session["Group"].ToString() == "A")
                    operate(1, 1, myword[0], myword[1], myword[2]);
                else operate(2, 2, myword[0], myword[1], myword[2]);
            }
            else operate(1, 2, myword[0], myword[1], myword[2]);
        }
        else if (Session["GLClick"] != null)
        {
            string s = Session["GLClick"].ToString();
            myword = s.Split(separator);
            if (Session["Group"] != null)
            {
                if (Session["Group"].ToString() == "A")
                    operate(3, 3, myword[0], myword[1], myword[2]);
                else operate(4, 4, myword[0], myword[1], myword[2]);
            }
            else operate(3, 4, myword[0], myword[1], myword[2]);
        }
        else if (Session["SMClick"] != null)
        {
            string s = Session["SMClick"].ToString();
            myword = s.Split(separator);
            if (Session["Group"] != null)
            {
                if (Session["Group"].ToString() == "A")
                    operate(5, 5, myword[0], myword[1], myword[2]);
                else operate(6, 6, myword[0], myword[1], myword[2]);
            }
            else operate(5, 6, myword[0], myword[1], myword[2]);
        }
        else if (Session["GMClick"] != null)
        {
            string s = Session["GMClick"].ToString();
            myword = s.Split(separator);
            operate(7, 7, myword[0], myword[1], myword[2]);
        }
    }
}
