using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data.OleDb;


public partial class Check : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection("server=localhost;database=BBAC_LPA;uid=sa;pwd=123;");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("Login.aspx");
        if (!this.IsPostBack)
        {
            SqlDataAdapter da = new SqlDataAdapter("select ShopName from Work group by ShopName", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "op");
            this.DropDownList1.DataSource = ds.Tables[0];
            this.DropDownList1.DataTextField = "ShopName";
            this.DropDownList1.DataValueField = "ShopName";
            this.DropDownList1.DataBind();
            this.DropDownList1.Items.Insert(0, new ListItem("请选择", "请选择"));
            /*if(Session["WorkGroup"]!=null)
                da = new SqlDataAdapter("select distinct SECOND.Name as name,SECOND.ID as ID from Management FIRST,Management SECOND" +
                " where @userid = SECOND.SuperID and FIRST.WorkGroup = SECOND.WorkGroup", con);
            else da = new SqlDataAdapter("select distinct SECOND.Name as name,SECOND.ID as ID from Management FIRST,Management SECOND" +
                " where  @userid = SECOND.SuperID", con);
            da.SelectCommand.Parameters.AddWithValue("@userid", Session["userid"]);
            ds = new DataSet();
            da.Fill(ds, "op");
            this.DropDownList5.DataSource = ds.Tables[0];
            this.DropDownList5.DataTextField = "name";
            this.DropDownList5.DataValueField = "ID";
            this.DropDownList5.DataBind();*/
            this.DropDownList5.Items.Insert(0, new ListItem("请选择", "请选择"));
            this.DropDownList5.Items.Insert(1, new ListItem(Session["user"].ToString(), Session["userid"].ToString()));
        }
        GridView1.Attributes.Add("style", "word-break:keep-all;word-wrap:normal");
    }
    public static SqlDataReader GetReader(string sql, params SqlParameter[] values)
    {
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddRange(values);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlDataAdapter da = new SqlDataAdapter("select  distinct GroupName from Work where ShopName=@id", con);
        string id = this.DropDownList1.SelectedValue;
        da.SelectCommand.Parameters.AddWithValue("@id", id);
        DataSet ds = new DataSet("Work");
        da.Fill(ds);
        this.DropDownList2.DataSource = ds.Tables[0];
        this.DropDownList2.DataTextField = "GroupName";
        this.DropDownList2.DataValueField = "GroupName";
        this.DropDownList2.DataBind();
        this.DropDownList2.Items.Insert(0, new ListItem("请选择", "请选择"));
    }
    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlDataAdapter da = new SqlDataAdapter("select distinct TeamName from Work where GroupName=@id", con);
        string id = this.DropDownList2.SelectedValue;
        da.SelectCommand.Parameters.AddWithValue("@id", id);
        DataSet ds = new DataSet("Work");
        da.Fill(ds);
        this.DropDownList3.DataSource = ds.Tables[0];
        this.DropDownList3.DataTextField = "TeamName";
        this.DropDownList3.DataValueField = "TeamName";
        this.DropDownList3.DataBind();
        this.DropDownList3.Items.Insert(0, new ListItem("请选择", "请选择"));
    }
    protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlDataAdapter da = new SqlDataAdapter("select WorkName from Work where TeamName=@TN and GroupName = @GN and ShopName = @SN", con);
        da.SelectCommand.Parameters.AddWithValue("@TN", this.DropDownList3.SelectedValue);
        da.SelectCommand.Parameters.AddWithValue("@GN", this.DropDownList2.SelectedValue);
        da.SelectCommand.Parameters.AddWithValue("@SN", this.DropDownList1.SelectedValue);
        DataSet ds = new DataSet("Work");
        da.Fill(ds);
        this.DropDownList4.DataSource = ds.Tables[0];
        this.DropDownList4.DataTextField = "WorkName";
        this.DropDownList4.DataValueField = "WorkName";
        this.DropDownList4.DataBind();
        this.DropDownList4.Items.Insert(0, new ListItem("请选择", "请选择"));
    }
    protected void DropDownList4_SelectedIndexChanged(object sender, EventArgs e)
    {
        string command = "select WorkSymbol from Work where Workname= @WorkName and TeamName = @TeamName";
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@WorkName", this.DropDownList4.SelectedValue), 
            new SqlParameter("@TeamName", this.DropDownList3.SelectedValue)
        };
        try
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            SqlDataReader sdr = GetReader(command,para);
            if (sdr.Read())
                Number.Text = sdr.GetString(0);
        }
        catch (System.Exception ex)
        {
            Response.Write("<script language='JavaScript'>alert('" + ex.Message.ToString() + "')</script>");
        }
        finally
        {
            con.Close();        //关闭连接        
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Label10.Visible = false;
        if (this.Number.Text.ToString().Trim() == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel4, this.GetType(), "info",
             "alert('请填写审查代码！');", true);
            return;
        }
        if (!CheckLPA(Number.Text))
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel4, this.GetType(), "info",
             "alert('工作不存在，请联系管理员！');", true); 
            return;
        }
        this.ExcelToDS(this.Number.Text.ToString().Trim());
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
            /*foreach (DataRow dr in dt.Rows)
            {
                bool flag = true;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!dr.IsNull(i))
                        flag = false;
                }
                if (flag)
                    dr.Delete();
            }*/
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
            Label10.Visible = true;
        }
        finally
        {
            conn.Close();        //关闭连接        
        }

    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        string s = null;
        int ID;
        string Checker = null;
        if (Number.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(Button4, this.GetType(), "info","alert('请填写审查代码！');", true);
            return;
        }
        if (!CheckLPA(Number.Text))
        {
            ScriptManager.RegisterClientScriptBlock(Button4, this.GetType(), "info",
             "alert('工作不存在，请联系管理员！');", true);
            return;
        }
        if (Session["permission"] != null)
        {
            if (Session["Position"].ToString() == "TL")
            {
                StringBuilder build = new StringBuilder(Number.Text.ToUpper());
                if (Session["WorkGroup"].ToString() == "A")
                {
                    build[3] = 'A';
                    ID = 1;
                }
                else
                {
                    build[3] = 'B';
                    ID = 2;
                }
                build.Remove(4, 1);
                Checker = build.ToString();
                s = Convert.ToString(ID);
            }
            else if (Session["Position"].ToString() == "GL")
            {
                StringBuilder build = new StringBuilder(Number.Text.ToUpper());
                if (Session["WorkGroup"].ToString() == "A")
                {
                    build[2] = 'A';
                    ID = 3;
                }
                else
                {
                    build[2] = 'B';
                    ID = 4;
                }
                build.Remove(3, 2);
                Checker = build.ToString();
                s = Convert.ToString(ID);
            }
            else if (Session["Position"].ToString() == "SM")
            {
                /*StringBuilder build = new StringBuilder(Number.Text.ToUpper());
                if (Session["WorkGroup"].ToString() == "A")
                {
                    build[1] = 'A';
                    ID = 5;
                }
                else
                {
                    build[1] = 'B';
                    ID = 6;
                }
                build.Remove(2, 3);*/
                Checker = "SM";
                ID = 5;
                s = Convert.ToString(ID);
            }
            else if (Session["Position"].ToString() == "GM")
            {
                Checker = "GM";
                ID = 7;
                s = Convert.ToString(ID);
            }
        }
        if (Checker == null)
        {
            ScriptManager.RegisterClientScriptBlock(Button4, this.GetType(), "info",
                "alert('Session错误，请重新登录，若问题依旧，请联系管理员！');", true);
            return;
        }
        else if (DropDownList5.SelectedValue == "请选择")
        {
            ScriptManager.RegisterClientScriptBlock(Button4, this.GetType(), "info","alert('请选择责任人！');", true);
            return;
        }
        else if (TextBox2.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(Button4, this.GetType(), "info", "alert('请选择日期！');", true);
            return;
        }
        else
        {
            DateTime date;
            try
            {
                date = Convert.ToDateTime(TextBox2.Text);
                SqlCommand insertCmd;             
                insertCmd = new SqlCommand(
                    "insert into Checkup(WorkSymbol,Checker,Checkername,CheckerID,ID,Checktime) values("
                    +"@WorkSymbol,@Checker,@Checkername,@CheckerID,@ID,@Checktime); select @@identity", con);
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();      //打开连接
                insertCmd.Parameters.AddWithValue("@WorkSymbol", Number.Text.Trim().ToUpper());
                insertCmd.Parameters.AddWithValue("@Checker", Checker.Trim());
                insertCmd.Parameters.AddWithValue("@Checkername", DropDownList5.SelectedItem.ToString());
                insertCmd.Parameters.AddWithValue("@CheckerID", DropDownList5.SelectedValue);
                insertCmd.Parameters.AddWithValue("@ID", s);
                insertCmd.Parameters.AddWithValue("@Checktime", date);
                string CheckNo = insertCmd.ExecuteScalar().ToString();//执行插入
                int flag = Convert.ToInt32(CheckNo);
                CheckNo =  CheckNo.PadLeft(8, '0');  
                if (flag <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(Button4, this.GetType(), "info","alert('操作失败！');", true);
                    return;
                }
                else
                {
                    Session["CheckNo"] = CheckNo;
                    Session["CheckerName"] = DropDownList5.SelectedItem;
                    Session["ChckerID"] = DropDownList5.SelectedValue;
                    Session["WorkSymbol"] = Number.Text.Trim().ToUpper();
                    Session["Checktime"] = date;
                    if (CheckBox1.Checked)
                    {
                        ScriptManager.RegisterClientScriptBlock(Button4, this.GetType(), "info",
                            "alert('添加成功！\\n审查对象: " + Number.Text.Trim().ToUpper() + "\\n责任人: " +
                            DropDownList5.SelectedItem + "\\n审查时间: " + TextBox2.Text.Trim() + "\\n即将跳转到PDCA生成页面！');"
                            +" window.location.href = 'Create_PDCA.aspx';", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(Button4, this.GetType(), "info",
                            "alert('添加成功！\\n审查对象: " + Number.Text.Trim().ToUpper() + "\\n责任人: " +
                            DropDownList5.SelectedItem + "\\n审查时间: " + TextBox2.Text.Trim() + "')", true);
                }
            }
            catch (System.Exception)
            {
                ScriptManager.RegisterClientScriptBlock(Button4, this.GetType(), "info",
                    "alert('添加时发生异常，请联系管理员！');", true);
                return;
            }
            finally
            {
                con.Close();        //关闭连接                  
            }
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        Response.Redirect("Check.aspx");
    }
    public bool CheckLPA(string name)
    {
        bool ret = false;
        string sql = "select * from Work where WorkSymbol = @WorkSymbol";
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@WorkSymbol", name),
        };
        try
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            SqlDataReader sdr = GetReader(sql,para);
            if (!sdr.Read())
            {
                ret = false;
            }
            else ret = true;
        }
        catch (Exception)
        {
            ret = false;
        }
        finally
        {
            con.Close();
        }
        return ret;
    }
}
