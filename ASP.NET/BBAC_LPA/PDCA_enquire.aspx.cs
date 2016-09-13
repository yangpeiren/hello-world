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

public partial class PDCA_enquire : System.Web.UI.Page
{
    static SqlConnection con = new System.Data.SqlClient.SqlConnection("server=(local);database=BBAC_LPA;uid=sa;pwd=123;");
    SqlDataAdapter da;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("Login.aspx");
        GridView1.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
        string s = null;
        for (int month = 1; month <= System.DateTime.Now.Month; month++)
        {
            s = Convert.ToString(month);
            DropDownList1.Items.Add(new ListItem(s + "月份", s));
        }
        DropDownList1.Items.Insert(0, new ListItem("请选择", "请选择"));
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedItem.ToString() == "请选择")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "info",
              "alert('请选择查询时段！');", true);
            return;
        }
        else
        {            
            bind(Convert.ToInt32(DropDownList2.SelectedValue));
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "numclick")
        {
            GridViewRow grv = ((GridViewRow)(((LinkButton)(e.CommandSource)).Parent.Parent));
            int a = grv.RowIndex;
            Session["petID"] = this.GridView1.DataKeys[grv.RowIndex].Value.ToString();
            Response.Redirect("PDCA_Detailview.aspx");
        }
    }
    public void bind(int value)
    {
        Label1.Visible = false;
        DateTime st = DateTime.Parse(System.DateTime.Now.Year.ToString() +
                "-1" + "-1").AddMonths(Convert.ToInt32(DropDownList1.SelectedValue) - 1);
        DateTime fn = st.AddMonths(1);
        string sql;
        if (value == -1)
        {
            sql = "select ProblemNO,CheckNO,Pname,Management.Name as CreatedBy,convert(varchar(100),CreatedDate,23)as CreatedDate,Depart," +
                "PD,RC,CM,Pstate,ClosedBy,CloseDate from PDCA join Management on PDCA.CreatedBy = Management.ID" +
                " where CreatedDate Between @st and @fn";
        }
        else
        {
            sql = "select ProblemNO,CheckNO,Pname,Management.Name as CreatedBy,convert(varchar(100),CreatedDate,23)as CreatedDate,Depart," +
                "PD,RC,CM,Pstate,ClosedBy,CloseDate from PDCA join Management on PDCA.CreatedBy = Management.ID" +
                " where Pstate = @Pstate and CreatedDate Between @st and @fn";
            
        }
        da = new SqlDataAdapter(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@st", st),
            new SqlParameter("@fn", fn),
            new SqlParameter("@Pstate", value)
        };
        try
        {
            da.SelectCommand.Parameters.AddRange(para);
            DataSet ds = new DataSet("Work");
            con.Open();
            da.Fill(ds);
            if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0)
            {
                Label1.Visible = true;
            }
            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();
        }
        catch (Exception ee)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "info",
                "alert('" + ee.Message + "');", true);
            return;
        }
        finally
        {
            con.Close();
        }
    }
}
