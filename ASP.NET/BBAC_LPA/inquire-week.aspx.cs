﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Globalization;

public partial class inquire_week : System.Web.UI.Page
{
    
    static SqlConnection con = new SqlConnection("server=localhost;database=BBAC_LPA;uid=sa;pwd=123;");
    SqlDataAdapter da = null;
    static string sql = null;
    public static int weekofyear(DateTime dtime)
    {
        /*int weeknum = 1;
        DateTime tmpdate = DateTime.Parse(dtime.Year.ToString() + "-1" + "-1");
        DayOfWeek firstweek = tmpdate.DayOfWeek;
        for (int i = (int)firstweek + 1; i < dtime.DayOfYear; i = i + 7)
        {
            weeknum = weeknum + 1;
        }
        
        return weeknum;
        DateTime mDatetime = new DateTime(System.DateTime.Now.Year, 1, 1);//year为要求的那一年   
        int firstweekfirstday = Convert.ToInt32(mDatetime.DayOfWeek);//一年中第一天是周几   
        int days = 7 - firstweekfirstday;
        DateTime secondweekfisrtday = mDatetime.AddDays(days); //第二周一
        DateTime fisrtday = mDatetime.AddDays(week * 7);//第N周第一天   
        DateTime lastday = mDatetime.AddDays(week * 7 + 6);//第N周最后一天
         * */
    
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("Login.aspx");
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        TextBox1.Text =weekofyear(System.DateTime.Now).ToString();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Label7.Visible = false;
        if (TextBox1.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "alert('请填写周次！')", true);
            return;
        }
        
        try
        {
            DateTime st = weektoday(Convert.ToInt32(TextBox1.Text));
            Session["begin"] = st;
            DateTime fn = weektoday(Convert.ToInt32(TextBox1.Text) + 1);
            Session["end"] = fn;
            sql = "select ShopName,GroupName,TeamName,"+
                "(case when(SUM(case when ID = '1' then 1 else 0 end)+SUM(case when ID = '2' then 1 else 0 end)) = '0'"+
                "then null else (SUM(case when ID = '1' then 1 else 0 end)+SUM(case when ID = '2' then 1 else 0 end)) end)TLCheck,"+
                "(case when(SUM(case when ID = '3' then 1 else 0 end)+SUM(case when ID = '4' then 1 else 0 end)) = '0'"+
                "then null else (SUM(case when ID = '3' then 1 else 0 end)+SUM(case when ID = '4' then 1 else 0 end)) end)GLCheck,"+
                "(case when(SUM(case when ID = '5' then 1 else 0 end)+SUM(case when ID = '6' then 1 else 0 end)) = '0'"+
                " then null else (SUM(case when ID = '5' then 1 else 0 end)+SUM(case when ID = '6' then 1 else 0 end)) end)SMCheck,"+
                "(case when(SUM(case when ID = '7' then 1 else 0 end)) = '0'"+
                "then null else (SUM(case when ID = '7' then 1 else 0 end)) end)GMCheck"+
                " from  Work left outer join Checkup on Checkup.WorkSymbol = Work.WorkSymbol " +
                "and Checktime Between @st and @fn group by ShopName,GroupName,TeamName";
            da = new SqlDataAdapter(sql, con);
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@st", st),
                new SqlParameter("@fn", fn)
            };
            da.SelectCommand.Parameters.AddRange(para);
            DataSet ds = new DataSet("Work");
            string s = null;
            s = TextBox1.Text.ToString().Trim();
            da.Fill(ds);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "alert('本周没有数据！')", true);
                Label7.Visible = true;
                return;
            }
            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();
            DropDownList ddl = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist1");            
            int week = weekofyear(System.DateTime.Now);
            for (int i = 1; i <= week; i++)
            {
                s = Convert.ToString(i);
                ddl.Items.Add(new ListItem(s,s));
            }
            s = TextBox1.Text.ToString().Trim();
            ddl.SelectedValue = s;
            
        }
        catch(Exception ee)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
              "alert(\"" + ee.Message + "\");", true);
            return;
        }
        da = new SqlDataAdapter("select ShopName from Work group by ShopName", con);
        DataSet dss = new DataSet();
        da.Fill(dss, "op");
        DropDownList ddl2 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist2");
        ddl2.DataSource = dss.Tables[0];
        ddl2.DataTextField = "ShopName";
        ddl2.DataValueField = "ShopName";
        ddl2.DataBind();
        ddl2.Items.Insert(0, new ListItem("车间", "车间"));
    }
    protected void Dropdownlist1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label7.Visible = false;
        DropDownList ddl = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist1");
        DateTime st = weektoday(Convert.ToInt32(ddl.SelectedValue));
        DateTime fn = weektoday(Convert.ToInt32(ddl.SelectedValue) + 1);  
        sql = "select ShopName,GroupName,TeamName," +
            "(case when(SUM(case when ID = '1' then 1 else 0 end)+SUM(case when ID = '2' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '1' then 1 else 0 end)+SUM(case when ID = '2' then 1 else 0 end)) end)TLCheck," +
            "(case when(SUM(case when ID = '3' then 1 else 0 end)+SUM(case when ID = '4' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '3' then 1 else 0 end)+SUM(case when ID = '4' then 1 else 0 end)) end)GLCheck," +
            "(case when(SUM(case when ID = '5' then 1 else 0 end)+SUM(case when ID = '6' then 1 else 0 end)) = '0'" +
            " then null else (SUM(case when ID = '5' then 1 else 0 end)+SUM(case when ID = '6' then 1 else 0 end)) end)SMCheck," +
            "(case when(SUM(case when ID = '7' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '7' then 1 else 0 end)) end)GMCheck" +
            " from  Work left outer join Checkup on Checkup.WorkSymbol = Work.WorkSymbol " +
            "and Checktime Between @st and @fn group by ShopName,GroupName,TeamName";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
                new SqlParameter("@st", st),
                new SqlParameter("@fn", fn)
        };
        try
        {
            da.SelectCommand.Parameters.AddRange(para);
            DataSet ds = new DataSet("Work");
            da.Fill(ds);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "alert('没有数据！')", true);
                return;
            }
            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();
            DropDownList ddl1 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist1");
            string s = null;
            
            int week = weekofyear(System.DateTime.Now);
            for (int i = 1; i <= week; i++)
            {
                s = Convert.ToString(i);
                ddl1.Items.Add(new ListItem(s, s));
            }
            s = ddl.SelectedValue.ToString().Trim();
            ddl1.SelectedValue = s;
            da = new SqlDataAdapter("select ShopName from Work group by ShopName", con);
            DataSet dss = new DataSet();
            da.Fill(dss, "op");
            DropDownList ddl2 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist2");
            ddl2.DataSource = dss.Tables[0];
            ddl2.DataTextField = "ShopName";
            ddl2.DataValueField = "ShopName";
            ddl2.DataBind();
            ddl2.Items.Insert(0, new ListItem("车间", "车间"));
        }
        catch (IndexOutOfRangeException ee)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
               "alert('" + ee.Message + "');", true);
            return;
        }
    }
    protected void Dropdownlist2_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label7.Visible = false;
        DropDownList ddl = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist1");
        DropDownList ddl2 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist2");
        DropDownList ddl3 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist3");
        DateTime st = weektoday(Convert.ToInt32(ddl.SelectedValue));
        DateTime fn = weektoday(Convert.ToInt32(ddl.SelectedValue) + 1);
        sql = "select ShopName,GroupName,TeamName," +
            "(case when(SUM(case when ID = '1' then 1 else 0 end)+SUM(case when ID = '2' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '1' then 1 else 0 end)+SUM(case when ID = '2' then 1 else 0 end)) end)TLCheck," +
            "(case when(SUM(case when ID = '3' then 1 else 0 end)+SUM(case when ID = '4' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '3' then 1 else 0 end)+SUM(case when ID = '4' then 1 else 0 end)) end)GLCheck," +
            "(case when(SUM(case when ID = '5' then 1 else 0 end)+SUM(case when ID = '6' then 1 else 0 end)) = '0'" +
            " then null else (SUM(case when ID = '5' then 1 else 0 end)+SUM(case when ID = '6' then 1 else 0 end)) end)SMCheck," +
            "(case when(SUM(case when ID = '7' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '7' then 1 else 0 end)) end)GMCheck" +
            " from  Work left outer join Checkup on Checkup.WorkSymbol = Work.WorkSymbol " +
            "and Checktime Between @st and @fn where ShopName = @ShopName group by ShopName,GroupName,TeamName";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
                new SqlParameter("@st", st),
                new SqlParameter("@fn", fn),
                new SqlParameter("@ShopName", ddl2.SelectedValue)
        };
        try
        {
            da.SelectCommand.Parameters.AddRange(para);
            DataSet ds = new DataSet("Work");
            da.Fill(ds);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "alert('没有数据！')", true);
                Label7.Visible = true;
                return;
            }
            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();
            DropDownList ddl1 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist1");
            int week = weekofyear(System.DateTime.Now);
            string s = null;
            for (int i = 1; i <= week; i++)
            {
                s = Convert.ToString(i);
                ddl1.Items.Add(new ListItem(s, s));
            }
            s = ddl.SelectedValue.ToString().Trim();
            ddl1.SelectedValue = s;

            DropDownList ddl4 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist2");           
            da = new SqlDataAdapter("select ShopName from Work group by ShopName", con);
            DataSet dss = new DataSet();
            da.Fill(dss, "op");
            ddl4.DataSource = dss.Tables[0];
            ddl4.DataTextField = "ShopName";
            ddl4.DataValueField = "ShopName";
            ddl4.DataBind();
            ddl4.SelectedValue = ddl2.SelectedValue;

            DropDownList ddl5 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist3");
            da = new SqlDataAdapter("select  distinct GroupName from Work where ShopName=@id", con);      
            string id = ddl2.SelectedValue;
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            ds = new DataSet("Work");
            da.Fill(ds);
            ddl5.DataSource = ds.Tables[0];
            ddl5.DataTextField = "GroupName";
            ddl5.DataValueField = "GroupName";
            ddl5.DataBind();
            ddl5.Items.Insert(0, new ListItem("工段", "工段"));

        }
        catch (IndexOutOfRangeException ee)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
               "alert('"+ee.Message+"');", true);
            return;
        }
    }
    protected void Dropdownlist3_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label7.Visible = false;
        DropDownList ddl = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist1");
        DropDownList ddl2 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist2");
        DropDownList ddl3 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist3");
        DateTime st = weektoday(Convert.ToInt32(ddl.SelectedValue));
        DateTime fn = weektoday(Convert.ToInt32(ddl.SelectedValue) + 1);
        sql = "select ShopName,GroupName,TeamName," +
            "(case when(SUM(case when ID = '1' then 1 else 0 end)+SUM(case when ID = '2' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '1' then 1 else 0 end)+SUM(case when ID = '2' then 1 else 0 end)) end)TLCheck," +
            "(case when(SUM(case when ID = '3' then 1 else 0 end)+SUM(case when ID = '4' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '3' then 1 else 0 end)+SUM(case when ID = '4' then 1 else 0 end)) end)GLCheck," +
            "(case when(SUM(case when ID = '5' then 1 else 0 end)+SUM(case when ID = '6' then 1 else 0 end)) = '0'" +
            " then null else (SUM(case when ID = '5' then 1 else 0 end)+SUM(case when ID = '6' then 1 else 0 end)) end)SMCheck," +
            "(case when(SUM(case when ID = '7' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '7' then 1 else 0 end)) end)GMCheck" +
            " from  Work left outer join Checkup on Checkup.WorkSymbol = Work.WorkSymbol " +
            "and Checktime Between @st and @fn where ShopName = @ShopName and GroupName = @GroupName group by ShopName,GroupName,TeamName";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
                new SqlParameter("@st", st),
                new SqlParameter("@fn", fn),
                new SqlParameter("@ShopName", ddl2.SelectedValue),
                new SqlParameter("@GroupName", ddl3.SelectedValue)
        };
        try
        {
            da.SelectCommand.Parameters.AddRange(para); 
            DataSet ds = new DataSet("Work");
            da.Fill(ds);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "alert('没有数据！')", true);
                Label7.Visible = true;
                return;
            }
            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();
            DropDownList ddl1 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist1");
            int week = weekofyear(System.DateTime.Now);
            string s = null;
            for (int i = 1; i <= week; i++)
            {
                s = Convert.ToString(i);
                ddl1.Items.Add(new ListItem(s, s));
            }
            s = ddl.SelectedValue.ToString().Trim();
            ddl1.SelectedValue = s;

            DropDownList ddl4 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist2");           
            da = new SqlDataAdapter("select ShopName from Work group by ShopName", con);
            DataSet dss = new DataSet();
            da.Fill(dss, "op");
            ddl4.DataSource = dss.Tables[0];
            ddl4.DataTextField = "ShopName";
            ddl4.DataValueField = "ShopName";
            ddl4.DataBind();
            ddl4.Items.Insert(0, new ListItem("车间", "车间"));
            ddl4.SelectedValue = ddl2.SelectedValue;

            DropDownList ddl5 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist3");
            da = new SqlDataAdapter("select  distinct GroupName from Work where ShopName=@id", con);      
            string id = ddl2.SelectedValue;
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            ds = new DataSet("Work");
            da.Fill(ds);
            ddl5.DataSource = ds.Tables[0];
            ddl5.DataTextField = "GroupName";
            ddl5.DataValueField = "GroupName";
            ddl5.DataBind();
            ddl5.Items.Insert(0, new ListItem("工段", "工段"));
            ddl5.SelectedValue = ddl3.SelectedValue;

            da = new SqlDataAdapter("select  distinct TeamName from Work where GroupName=@id", con);
            DropDownList ddl6 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist4");
            id = ddl3.SelectedValue;
            da.SelectCommand.Parameters.AddWithValue("@id", id);
             ds = new DataSet("Work");
            da.Fill(ds);
            ddl6.DataSource = ds.Tables[0];
            ddl6.DataTextField = "TeamName";
            ddl6.DataValueField = "TeamName";
            ddl6.DataBind();
            ddl6.Items.Insert(0, new ListItem("班组", "班组"));
        }
        catch (IndexOutOfRangeException ee)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
               "alert('" + ee.Message + "');", true);
            return;
        }
    }
    protected void Dropdownlist4_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label7.Visible = false;
        DropDownList ddl = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist1");
        DropDownList ddl2 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist2");
        DropDownList ddl3 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist3");
        DropDownList ddl4 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist4");
        DateTime st = weektoday(Convert.ToInt32(ddl.SelectedValue));
        DateTime fn = weektoday(Convert.ToInt32(ddl.SelectedValue)+ 1);
        sql = "select ShopName,GroupName,TeamName," +
            "(case when(SUM(case when ID = '1' then 1 else 0 end)+SUM(case when ID = '2' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '1' then 1 else 0 end)+SUM(case when ID = '2' then 1 else 0 end)) end)TLCheck," +
            "(case when(SUM(case when ID = '3' then 1 else 0 end)+SUM(case when ID = '4' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '3' then 1 else 0 end)+SUM(case when ID = '4' then 1 else 0 end)) end)GLCheck," +
            "(case when(SUM(case when ID = '5' then 1 else 0 end)+SUM(case when ID = '6' then 1 else 0 end)) = '0'" +
            " then null else (SUM(case when ID = '5' then 1 else 0 end)+SUM(case when ID = '6' then 1 else 0 end)) end)SMCheck," +
            "(case when(SUM(case when ID = '7' then 1 else 0 end)) = '0'" +
            "then null else (SUM(case when ID = '7' then 1 else 0 end)) end)GMCheck" +
            " from  Work left outer join Checkup" + " on Checkup.WorkSymbol = Work.WorkSymbol " +
            "and Checktime Between @st and @fn " +
            "where ShopName = @ShopName and GroupName = @GroupName and TeamName = @TeamName group by ShopName,GroupName,TeamName";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
                new SqlParameter("@st", st),
                new SqlParameter("@fn", fn),
                new SqlParameter("@ShopName", ddl2.SelectedValue),
                new SqlParameter("@GroupName", ddl3.SelectedValue),
                new SqlParameter("@TeamName", ddl4.SelectedValue)
        };
        try
        {
            da.SelectCommand.Parameters.AddRange(para); DataSet ds = new DataSet("Work");
            da.Fill(ds);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "alert('没有数据！')", true);
                Label7.Visible = true;
                return;
            }
            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();
            DropDownList ddl1 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist1");
            int week = weekofyear(System.DateTime.Now);
            string s = null;
            for (int i = 1; i <= week; i++)
            {
                s = Convert.ToString(i);
                ddl1.Items.Add(new ListItem(s, s));
            }
            s = ddl.SelectedValue.ToString().Trim();
            ddl1.SelectedValue = s;

            DropDownList ddl5 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist2");
            da = new SqlDataAdapter("select ShopName from Work group by ShopName", con);
            DataSet dss = new DataSet();
            da.Fill(dss, "op");
            ddl5.DataSource = dss.Tables[0];
            ddl5.DataTextField = "ShopName";
            ddl5.DataValueField = "ShopName";
            ddl5.DataBind();
            ddl5.Items.Insert(0, new ListItem("车间", "车间"));
            ddl5.SelectedValue = ddl2.SelectedValue;

            DropDownList ddl6 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist3");
            da = new SqlDataAdapter("select  distinct GroupName from Work where ShopName=@id", con);
            string id = ddl2.SelectedValue;
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            ds = new DataSet("Work");
            da.Fill(ds);
            ddl6.DataSource = ds.Tables[0];
            ddl6.DataTextField = "GroupName";
            ddl6.DataValueField = "GroupName";
            ddl6.DataBind();
            ddl6.Items.Insert(0, new ListItem("工段", "工段"));
            ddl6.SelectedValue = ddl3.SelectedValue;

            da = new SqlDataAdapter("select  distinct TeamName from Work where GroupName=@id", con);
            DropDownList ddl7 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist4");
            id = ddl3.SelectedValue;
            da.SelectCommand.Parameters.AddWithValue("@id", id);
            ds = new DataSet("Work");
            da.Fill(ds);
            ddl7.DataSource = ds.Tables[0];
            ddl7.DataTextField = "TeamName";
            ddl7.DataValueField = "TeamName";
            ddl7.DataBind();
            ddl7.Items.Insert(0, new ListItem("班组", "班组"));
            ddl7.SelectedValue = ddl4.SelectedValue;
        }
        catch (IndexOutOfRangeException ee)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
               "alert('" + ee.Message + "');", true);
            return;
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow grv = ((GridViewRow)(((LinkButton)(e.CommandSource)).Parent.Parent));
        string petID = this.GridView1.DataKeys[grv.RowIndex].Values["ShopName"].ToString();
        petID = petID + "," + this.GridView1.DataKeys[grv.RowIndex].Values["GroupName"].ToString();
        petID = petID + "," + this.GridView1.DataKeys[grv.RowIndex].Values["TeamName"].ToString();
        if (e.CommandName == "TLClick")
        {
            Session["TLClick"] = petID;
            Session["GLClick"] = null;
            Session["SMClick"] = null;
            Session["GMClick"] = null;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
               "window.open('detailview.aspx');", true);
        }
        else if (e.CommandName == "GLClick")
        {
            Session["TLClick"] = null;
            Session["GLClick"] = petID;
            Session["SMClick"] = null;
            Session["GMClick"] = null;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
               "window.open('detailview.aspx');", true);
        }
        else if (e.CommandName == "SMClick")
        {
            Session["TLClick"] = null;
            Session["GLClick"] = null;
            Session["SMClick"] = petID;
            Session["GMClick"] = null;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
               "window.open('detailview.aspx');", true);
        }
        else if (e.CommandName == "GMClick")
        {
            Session["TLClick"] = null;
            Session["GLClick"] = null;
            Session["SMClick"] = null;
            Session["GMClick"] = petID;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
               "window.open('detailview.aspx');", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "info",
               "alert('表格出错，请联系管理员！');", true);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {      
        if (this.GridView1.Rows.Count > 0)
        {
            DropDownList ddl = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist1");
            string name = "LPA_CW"+ ddl.SelectedValue;
            export("application/ms-excel", name + ".xls");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "alert('请先查询！')", true);
            //return;
        }

    }
    public void export(string fileType, string fileName)
    {
        if (sql == null)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, this.GetType(), "click", "alert('请先查询！')", true);
            return;
        }
        da = new SqlDataAdapter(sql, con);
        DropDownList ddl = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist1");
        DropDownList ddl2 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist2");
        DropDownList ddl3 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist3");
        DropDownList ddl4 = (DropDownList)this.GridView1.HeaderRow.FindControl("Dropdownlist4");
        DateTime st = weektoday(Convert.ToInt32(ddl.SelectedValue));
        DateTime fn = weektoday(Convert.ToInt32(ddl.SelectedValue) + 1);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@st", st),
            new SqlParameter("@fn", fn),
            new SqlParameter("@ShopName", ddl2.SelectedValue),
            new SqlParameter("@GroupName", ddl3.SelectedValue),
            new SqlParameter("@TeamName", ddl4.SelectedValue)
        };
        da.SelectCommand.Parameters.AddRange(para);
        DataSet ds = new DataSet("Work");
        da.Fill(ds, "table1");
        DataTable dt = ds.Tables["table1"];
        string filepath = Server.MapPath("~/Temp/" + fileName);
        FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("GB2312"));
        sw.WriteLine("CW"+ddl.SelectedValue + "\t" + ddl2.SelectedValue + "\t" + ddl3.SelectedValue + "\t" +
            ddl4.SelectedValue + "\tTLCheck\tGLCheck\tSMCheck\tGMCheck");
        foreach (DataRow dr in dt.Rows)
        {
            sw.WriteLine("" + "\t" + dr["ShopName"] + "\t" + dr["GroupName"] + "\t" + dr["TeamName"] + "\t"
                + dr["TLCheck"] + "\t" + dr["GLCheck"] + "\t" + dr["SMCheck"] + "\t" + dr["GMCheck"]);
        }
        sw.Close();
        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
        Response.ContentType = "application/ms-excel";// 指定返回的是一个不能被客户端读取的流，必须被下载
        Response.WriteFile(filepath); // 把文件流发送到客户端
        Response.End();
    }
}
