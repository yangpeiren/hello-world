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

public partial class maintain_route : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Label2.Text = Session["user"].ToString();
        if (Session["permission"].ToString() != "1111")
        {
            Response.Redirect("Login.aspx");
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Work_Maintain.aspx");
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("System_admin.aspx");
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("PDCA_Maintain.aspx");
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        Response.Redirect("Check_Maintain.aspx");
    }
}
