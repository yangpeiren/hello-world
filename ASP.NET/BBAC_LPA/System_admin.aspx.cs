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
using System.Security.Authentication;
using System.Security.Cryptography;

public partial class System_admin : System.Web.UI.Page
{
    SqlCommand command;
    static SqlConnection con = new SqlConnection("server=(local);database=BBAC_LPA;uid=sa;pwd=123;");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["permission"].ToString() != "1111")
            {
                Response.Redirect("Login.aspx");
            }
            this.Label2.Text = Session["user"].ToString();
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
        SqlDataAdapter da = new SqlDataAdapter("select * from Management where ShopName = '" + Session["ShopName"] + "'", con);
        try
        {
            DataSet ds = new DataSet("Work");
            da.Fill(ds);
            this.GridView1.DataSource = ds;
            this.GridView1.DataKeyNames = new string[] { "ID" };
            this.GridView1.DataBind();
        }
        catch (IndexOutOfRangeException ee)
        {
            Response.Write("<script language='JavaScript'>alert('" + ee.Message + "');</script>");
            return;
        }
    }
    public static string GetMD5HashHex(String input)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        DES des = new DESCryptoServiceProvider();
        byte[] res = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(input), 0, input.Length);

        String returnThis = "";

        for (int i = 0; i < res.Length; i++)
        {
            returnThis += System.Uri.HexEscape((char)res[i]);
        }
        returnThis = returnThis.Replace("%", "");
        returnThis = returnThis.ToLower();
        return returnThis;
    }
    protected void GridView_OnRowDeleting(object sender,GridViewDeleteEventArgs e)
    {
        string sql = "delete from Management where ID = @ID";
        command = new SqlCommand(sql, con);
        command.Parameters.AddWithValue("@ID", GridView1.DataKeys[e.RowIndex].Value.ToString());
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
            Response.Write("<script language='JavaScript'>alert('" + ee.Message.ToString() + "')</script>");
            return;
        }
        finally
        {
            con.Close();
        }
        bind();
    }
    protected void GridView_OnRowEditing(object sender,GridViewEditEventArgs e)
    {
        this.GridView1.EditIndex = e.NewEditIndex;
        bind();
    }
    protected void GridView_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string sql = "update Management set SuperID = @SuperID,Position = @Position,ShopName = @ShopName,GroupName = "
            + "@GroupName ,TeamName = @TeamName,WorkGroup = @WorkGroup,Limit = @Limit where ID = @ID";
        command = new SqlCommand(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@SuperID", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[1].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@Position", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[3].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@ShopName", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[4].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@GroupName", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[5].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@TeamName",  ((TextBox)(GridView1.Rows[e.RowIndex].Cells[6].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@WorkGroup", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[7].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@Limit", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[10].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@ID", GridView1.DataKeys[e.RowIndex].Value.ToString())
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
            this.GridView1.EditIndex = -1;
        }
        catch (Exception ee)
        {
            Response.Write("<script language='JavaScript'>alert(\"" + ee.Message + "\")</script>");
            return;
        }
        bind();

    }
    protected void GridView_OnRowCancelingEdit(object sender,GridViewCancelEditEventArgs e)
    {
        this.GridView1.EditIndex = -1;
        bind();
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "click")
        {
            string id = e.CommandArgument.ToString();
            string petID = this.GridView1.DataKeys[Convert.ToInt32(id)].Value.ToString();
            Random ran = new Random();
            string newpwd = (ran.Next(99999999).ToString().PadLeft(8, '8'));
            string pwd = GetMD5HashHex(newpwd);
            string sql = "update Management set Password = '" + pwd + "',SetTime = '"+System.DateTime.Now+"' where ID = '" + petID + "'";
            command = new SqlCommand(sql, con);
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
                    Response.Write("<script language='JavaScript'>alert('操作已成功！\\n新密码为"+newpwd+"')</script>");
                }
                con.Close();
            }
            catch (Exception ee)
            {
                Response.Write("<script language='JavaScript'>alert(\"" + ee.Message.ToString() + "\")</script>");
                return;
            }
            finally
            {
                con.Close();
                bind();
            }
        }
        else if(e.CommandName == "change")
        {
            string id = e.CommandArgument.ToString();     
            string petID = this.GridView1.DataKeys[Convert.ToInt32(id)].Value.ToString();
            string sql = "select Active from Management where ID = '" + petID + "'";
            command = new SqlCommand(sql, con);
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                SqlDataReader sdr = command.ExecuteReader();
                if (!sdr.Read())
                {
                    Response.Write("<script language='JavaScript'>alert('操作失败！')</script>");
                    return;
                }
                else
                {
                    string result = sdr.GetSqlValue(0).ToString();
                    con.Close();
                    bool res = Convert.ToBoolean(result);
                    res = !res;
                    sql = "update Management set Active = '" + res + "' where ID = '" + petID + "'";
                    command = new SqlCommand(sql, con);
                    try
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                        con.Open();
                        if (command.ExecuteNonQuery() != 1)
                        {
                            Response.Write("<script language='JavaScript'>alert('操作失败！')</script>");
                            return;
                        }
                        Response.Write("<script language='JavaScript'>alert('操作已成功！')</script>");
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script language='JavaScript'>alert(\"" + ex.Message + "\")</script>");
                        return;
                    }

                }
            }
            catch (Exception ee)
            {
                Response.Write("<script language='JavaScript'>alert(\"" + ee.Message + "\")</script>");
                return;
            }
            finally
            {
                con.Close();
                bind();
            }        
            
        }        
                
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("maintain_route.aspx");
    }
}
