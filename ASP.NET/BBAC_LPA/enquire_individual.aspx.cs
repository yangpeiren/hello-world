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
using System.Globalization;

public partial class enquire_individual : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection("server=localhost;database=BBAC_LPA;uid=sa;pwd=123;");
    public static int weekofyear(DateTime dtime)
    {
        CultureInfo myCI = new CultureInfo("zh-CN");
        System.Globalization.Calendar myCal = myCI.Calendar;

        //日期格式
        CalendarWeekRule myCWR = (CalendarWeekRule)2;
        DayOfWeek myFirstDOW = (DayOfWeek)1;

        //本周为第myCal.GetWeekOfYear( DateTime.Now, myCWR, myFirstDOW )周
        int thisWeek = myCal.GetWeekOfYear(dtime, myCWR, myFirstDOW);

        return thisWeek;
    }
    public DateTime weektoday(int week)
    {
        DateTime tmpdate = DateTime.Parse(System.DateTime.Now.Year.ToString() + "-1" + "-1");
        DayOfWeek tmpday = tmpdate.DayOfWeek;
        DateTime dt;
        if (tmpdate.DayOfWeek == (DayOfWeek)0)
            tmpday = (DayOfWeek)7;
        if (tmpday < (DayOfWeek)4)
            dt = tmpdate.AddDays(7 - (int)tmpday + (week - 2) * 7 + 2);
        else dt = tmpdate.AddDays(7 - (int)tmpday + (week - 1) * 7 + 2);

        return GetWeekStart(dt);
    }
    private DateTime GetWeekStart(DateTime datePoint)//函数作用是接受某一天，然后返回该日期所处星期的第一天
    {
        DateTime result = new DateTime();
        DayOfWeek day = datePoint.DayOfWeek;
        string dayString = day.ToString();
        switch (dayString)
        {
            case "Monday":
                result = datePoint.Date;
                break;
            case "Tuesday":
                result = datePoint.Date.AddDays(-1);
                break;
            case "Wednesday":
                result = datePoint.Date.AddDays(-2);
                break;
            case "Thursday":
                result = datePoint.Date.AddDays(-3);
                break;
            case "Friday":
                result = datePoint.Date.AddDays(-4);
                break;
            case "Saturday":
                result = datePoint.Date.AddDays(-5);
                break;
            case "Sunday":
                result = datePoint.Date.AddDays(-6);
                break;
        }
        return result;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("Login.aspx");
        Label6.Visible = false;
        if (!IsPostBack)
        {
            string sql = null;
            SqlDataAdapter da = null;
            if (Session["Position"].ToString() == "GL")
                sql = "select ID,Name from Management where ID = @ID union " +
                    "select ID,Name from Management where SuperID = @SuperID";
            else if (Session["Position"].ToString() == "SM")
                sql = "select ID,Name from Management where ID = @ID union " +
                        "select ID,Name from Management where SuperID = @SuperID union " +
                        "select ID,Name from Management where SuperID in (select ID from Management where SuperID = @SuperID)";
            else if (Session["Position"].ToString() == "GM")
            {
                sql = "select ID,Name from Management where ID = @ID union " +
                    "select ID,Name from Management where SuperID = @SuperID union " +
                    "select ID,Name from Management where SuperID in (select ID from Management where SuperID = @SuperID) union " +
                    "select ID,Name from Management where SuperID in(select ID from Management where SuperID in (select ID from Management where SuperID = @SuperID))";
            }
            da = new SqlDataAdapter(sql, con);
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@ID", Session["userid"]),
                new SqlParameter("@SuperID", Session["userid"])
            };
            da.SelectCommand.Parameters.AddRange(para);
            DataSet ds = new DataSet();
            if (sql != null)
            {
                da.Fill(ds, "op");
                this.DropDownList1.DataSource = ds.Tables[0];
                this.DropDownList1.DataTextField = "Name";
                this.DropDownList1.DataValueField = "ID";
                this.DropDownList1.DataBind();
            }

            else
            {
                this.DropDownList1.Items.Insert(0, new ListItem(Session["user"].ToString(), Session["userid"].ToString()));
            }
            this.DropDownList1.Items.Insert(0, new ListItem("请选择", "请选择"));
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedValue == "请选择")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
              "alert('请选择查询对象！');", true);
            return;
        }
        else
        {
            bind(DropDownList1.SelectedValue.ToString(),DateTime.Parse(System.DateTime.Now.Year.ToString() + "-1" + "-1"), System.DateTime.Now);
            if(GridView1.Rows.Count != 0)
            {
            DropDownList ddl = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist5");
            ddl.SelectedIndex = 0;
            }
            Label6.Visible = false;
        }
    }
    protected void Dropdownlist5_SelectedIndexChanged(object sender,EventArgs e)
    {
        DropDownList ddl = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist5");
        DateTime st = weektoday(Convert.ToInt32(ddl.SelectedValue));
        DateTime fn = weektoday(Convert.ToInt32(ddl.SelectedValue) + 1);
        bind(DropDownList1.SelectedValue, st, fn);

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DateTime st = DateTime.Parse(System.DateTime.Now.Year.ToString() + "-1" + "-1"); 
        DateTime fn = System.DateTime.Now;
        GridView1.PageIndex = e.NewPageIndex;
        DropDownList ddl = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist5");
        if (ddl.SelectedValue.ToString() != "请选择")
        {
            st = weektoday(Convert.ToInt32(ddl.SelectedValue) - 1);
            fn = weektoday(Convert.ToInt32(ddl.SelectedValue));
        }
        bind(DropDownList1.SelectedValue,st,fn);
    }
    public void bind(string name,DateTime st,DateTime fn)
    {
        string sql = "select code,checkup.worksymbol,checker,checkername,convert(varchar(100),checktime,23)as checktime from checkup " +
         "join work on checkup.worksymbol = work.worksymbol " +
         "where CheckerID = @CheckerID and Checktime between @st and @fn";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@st", st),
            new SqlParameter("@fn", fn),
            new SqlParameter("@CheckerID", name)
        };
        try
        {
            da.SelectCommand.Parameters.AddRange(para);
            DataSet ds = new DataSet();
            string s = null;
            da.Fill(ds,"Work");
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "alert('没有数据！')", true);
                this.GridView1.DataSource = ds;
                this.GridView1.DataBind();
                Label6.Visible = true;
                return;
            }
            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();
            DropDownList ddl = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist5");
            int week = weekofyear(System.DateTime.Now);
            for (int i = 1; i <= week; i++)
            {
                s = Convert.ToString(i);
                ddl.Items.Add(new ListItem(s, s));
            }
            ddl.Items.Insert(0, new ListItem("请选择", "请选择"));
            ddl.SelectedValue = Convert.ToString(weekofyear(st));

        }
        catch (IndexOutOfRangeException ee)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
              "alert('" + ee.Message + "');", true);
            return;
        }
    }
}
