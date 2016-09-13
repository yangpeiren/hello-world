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
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Data.SqlClient;

public partial class cpassword : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection("server=(local);database=BBAC_LPA;uid=sa;pwd=123;");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("Login.aspx");
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        string oldpas = password0.Text.Trim();
        string newpas = password.Text.Trim();
        if (newpas == ""|| oldpas == "")
            ScriptManager.RegisterClientScriptBlock(Button1, this.GetType(), "info",
              "alert('请注意页面信息！');", true);
        else
        {
            oldpas = GetMD5HashHex(oldpas);
            newpas = GetMD5HashHex(newpas);
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                string sql = "select * from Management where  ID = @ID and Password = @Password";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ID", Session["userid"]);
                cmd.Parameters.AddWithValue("@Password", oldpas);
                SqlDataReader sdr = cmd.ExecuteReader();
                
                if (!sdr.Read())
                {
                    ScriptManager.RegisterClientScriptBlock(Button1, this.GetType(), "info",
                        "alert('原密码错误！');", true);
                    return;
                }
                con.Close();
                sql = "update Management set Password = @Password,SetTime = @SetTime where ID = @ID";
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Password", newpas);
                cmd.Parameters.AddWithValue("@SetTime",System.DateTime.Now);
                cmd.Parameters.AddWithValue("@ID", Session["userid"]);
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                if (cmd.ExecuteNonQuery() != 1)
                {
                    ScriptManager.RegisterClientScriptBlock(Button1, this.GetType(), "info",
                        "alert('修改失败，请联系管理员！');", true);
                    return;
                }
                else 
                    ScriptManager.RegisterClientScriptBlock(Button1, this.GetType(), "info",
                        "alert('修改成功！');", true);
            }
            catch (System.Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert(\"" + ex.Message.ToString() + "\")</script>");
            }
            finally
            {
                con.Close();        //关闭连接            
            }
        }
    }
}
