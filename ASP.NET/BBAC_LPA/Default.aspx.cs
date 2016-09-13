using System;
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

public partial class _Default : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection("server=(local);database=BBAC_LPA;uid=sa;pwd=123;");
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    //注册
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
    protected void signin_Click1(object sender, EventArgs e)
    {
        string useremail = email.Text.Trim();
        string username = name.Text.Trim();
        string userpwd = GetMD5HashHex(password.Text.Trim());
        if (useremail.Length == 0 || username.Length == 0 || userpwd.Length == 0)
        {
            Response.Write("<script language='JavaScript'>alert('请将信息填写完整！')</script>");
            return;
        }
        //验证用户名是否存在
        try
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from Management where  Username = @Username", con);
            cmd.Parameters.AddWithValue("@Username", useremail);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                Response.Write("<script language='JavaScript'>alert('用户名已经存在');window.location.href = 'Default.aspx';</script>");
                return;
            }
        }
        catch (System.Exception ex)
        {
            Response.Write("<script language='JavaScript'>alert(\"" + ex.Message + "\")</script>");
            return;
        }
        finally
        {
            con.Close();        //关闭连接        
        }
        //--------------------------------------------------------验证结束
        string sql = "insert into Management(Name,ShopName,Username,Password,Limit,SetTime) values(@Name,@ShopName,@Username,@Password,'0000',@SetTime)";
        SqlCommand insertCmd = new SqlCommand(sql, con);
        try
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();      //打开连接
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@Name", username.Trim()), 
                new SqlParameter("@Username", useremail.Trim()),
                new SqlParameter("@Password", userpwd.Trim()),
                new SqlParameter("@ShopName",DropDownList1.SelectedValue),
                new SqlParameter("@SetTime",System.DateTime.Now)
            };
            insertCmd.Parameters.AddRange(para);
            int flag = insertCmd.ExecuteNonQuery();  //执行插入
            if (flag > 0)
            {
                Response.Write("<script language='JavaScript'>alert('注册成功！');window.location.href = 'Login.aspx';</script>");
            }
            else
            {
                Response.Write("<script language='JavaScript'>alert('注册失败！');window.location.href = 'Default.aspx';</script>");
            }

        }
        catch (System.Exception ee)
        {
            Response.Write("<script language=javascript>alert(\"" + ee.Message + "\")</script>");
        }
        finally
        {
            con.Close();        //关闭连接  
        }

    }
    protected void back_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }

}
    
