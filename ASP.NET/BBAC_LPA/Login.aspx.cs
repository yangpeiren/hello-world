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
using Microsoft.VisualBasic;

public partial class Login : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection("server=(local);database=BBAC_LPA;uid=sa;pwd=123;");
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();
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
    public static SqlDataReader GetReader(string sql, params SqlParameter[] values)
    {
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddRange(values);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }
    protected void signin_Click1(object sender, EventArgs e)
    {
        string useremail = email.Text.Trim();
        string userpwd = password.Text.Trim();
        if (useremail.Length == 0 || userpwd.Length == 0)
        {
            Response.Write("<script language='JavaScript'>alert('请填写完整信息！')</script>");
            return;
        }
        try
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            userpwd = GetMD5HashHex(userpwd);
            string cmd = "select * from Management where  Username= @Username and Password = @Password";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@Username", useremail), 
                new SqlParameter("@Password", userpwd)
            };
            SqlDataReader sdr = GetReader(cmd,para);
            if (!sdr.HasRows)
            {
                Response.Write("<script language='JavaScript'>alert('用户名/密码错误！')</script>");
                email.Text = "";
                password.Text = "";
                return;
            }
            else
            {
                sdr.Read();
                if(sdr.GetSqlValue(11).ToString() == "False")
                {
                    Response.Write("<script language='JavaScript'>alert('用户未激活，请联系管理员！')</script>");
                    return;
                }
                Session.Add("useremail",sdr.GetSqlValue(8));
                Session.Add("user",sdr.GetSqlValue(2));
                Session.Add("userid", sdr.GetSqlValue(0));
                Session.Add("permission", sdr.GetSqlValue(10));
                Session.Add("WorkGroup", sdr.GetSqlValue(7));
                Session.Add("Position", sdr.GetSqlValue(3));
                Session.Add("ShopName", sdr.GetSqlValue(4));
                DateTime SetTime = Convert.ToDateTime(sdr.GetSqlValue(12));
                long time = DateAndTime.DateDiff(DateInterval.Day,SetTime,System.DateTime.Now,
                    Microsoft.VisualBasic.FirstDayOfWeek.Sunday,FirstWeekOfYear.Jan1);
                int val = 90;
                if (Session["permission"].ToString() == "1111" && RadioButton2.Checked)
                {
                    if (time < val - 3)
                        Response.Redirect("maintain_route.aspx");
                    else if(time >= val -3 && time < val)
                        Response.Write("<script language='JavaScript'>alert('密码还有"+Convert.ToString(val - time)+"天到期，请您尽快修改密码！'); window.location.href = 'maintain_route.aspx';</script>");
                    else
                        Response.Write("<script language='JavaScript'>alert('密码已经到期，请您尽快修改密码！'); window.location.href = 'maintain_route.aspx';</script>");
                }
                else
                {
                    if (time < val - 3)
                        Response.Redirect("frame.htm");
                    else if (time >= val - 3 && time < val)
                        Response.Write("<script language='JavaScript'>alert('密码还有" + Convert.ToString(val - time) + "天到期，请您尽快修改密码！'); window.location.href = 'frame.htm';</script>");
                    else 
                        Response.Write("<script language='JavaScript'>alert('密码时间已到期，请您尽快修改密码！'); window.location.href = 'frame.htm';</script>");

                }
            }
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
    protected void back_Click(object sender, EventArgs e)
    {
        email.Text = "";
        password.Text = "";
    }
}
