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
using System.Data.OleDb;
using Microsoft.Office.Interop.Owc11;


public partial class Work_Maintain : System.Web.UI.Page
{
    static SqlConnection con = new SqlConnection("server=(local);database=BBAC_LPA;uid=sa;pwd=123;");
    SqlCommand command;
    protected void Page_Load(object sender, EventArgs e)
    {
        Label13.Visible = false;
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
        SqlDataAdapter da = new SqlDataAdapter("select * from Work where ShopName = '"+ Session["ShopName"] +"'", con);
        try
        {
            DataSet ds = new DataSet("Work");
            da.Fill(ds);
            this.GridView1.DataSource = ds;
            this.GridView1.DataKeyNames = new string[] { "WorkSymbol" };
            this.GridView1.DataBind();
        }
        catch (IndexOutOfRangeException ee)
        {
            Response.Write("<script language='JavaScript'>alert('" + ee.Message + "');</script>");
            return;
        }
    }
    protected void GridView_OnRowDeleting(object sender,GridViewDeleteEventArgs e)
    {
        string sql = "delete from Work where WorkSymbol = '" + GridView1.DataKeys[e.RowIndex].Value.ToString()
            + "'";
        command = new SqlCommand(sql, con);
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
        string sql = "update Work set ShopName = @ShopName,GroupName = @GroupName ,TeamName = @TeamName"
            + ",WorkName = @WorkName where WorkSymbol = @WorkSymbol";
        command = new SqlCommand(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@ShopName", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[1].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@GroupName", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[2].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@TeamName", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[3].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@WorkName", ((TextBox)(GridView1.Rows[e.RowIndex].Cells[4].Controls[0])).Text.ToString().Trim()),
            new SqlParameter("@WorkSymbol", GridView1.DataKeys[e.RowIndex].Value.ToString())
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
            Response.Write("<script language='JavaScript'>alert('" + ee.Message.ToString() + "')</script>");
            return;
        }
        bind();

    }
    protected void GridView_OnRowCancelingEdit(object sender,GridViewCancelEditEventArgs e)
    {
        this.GridView1.EditIndex = -1;
        bind();
    }
    public void ExcelToDS(string Path)
    {
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Server.MapPath("~/Work/" + Path + ".xls;")
            + "Extended Properties='Excel 8.0;HDR = No;IMEX=1'";
        OleDbConnection conn = new OleDbConnection(strConn);
        /*System.IO.FileInfo fleInfo = new System.IO.FileInfo(Server.MapPath("~/Work/" + Path + ".xls"));
        Response.Clear();
        Response.AddHeader("Content-Disposition", "inline; filename=" + "test.xls" );
        Response.AddHeader("Content-Length", fleInfo.Length.ToString());
        Response.ContentType = "application/vnd.ms-excel";
        Response.WriteFile(fleInfo.FullName);
        Response.End();*/
        
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
            this.GridView2.DataSource = dt;
            this.GridView2.DataBind();
            int count = 0;
            int num = 0;
            foreach (DataRow dr in dt.Rows)
            {
;
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
                    GridView2.Rows[count].Cells[0].Text = "<B><Center>" + GridView2.Rows[count].Cells[0].Text + "</Center></B>";
                    GridView2.Rows[count].Cells[2].Text = "<B><Center>" + GridView2.Rows[count].Cells[2].Text + "</Center></B>";
                    GridView2.Rows[count].Cells[5].Text = "<B><Center>" + GridView2.Rows[count].Cells[5].Text + "</Center></B>";
                }
                if (num >= 2)
                {
                    GridView2.Rows[count + 1].Cells[0].Text = "<B><Center>" + GridView2.Rows[count + 1].Cells[0].Text + "</Center></B>";
                    GridView2.Rows[count + 1].Cells[1].Text = "<B><Center>" + GridView2.Rows[count + 1].Cells[1].Text + "</Center></B>";
                    GridView2.Rows[count + 1].Cells[2].Text = "<B><Center>" + GridView2.Rows[count + 1].Cells[2].Text + "</Center></B>";
                    GridView2.Rows[count + 1].Cells[3].Text = "<B><Center>" + GridView2.Rows[count + 1].Cells[3].Text + "</Center></B>";
                    GridView2.Rows[count + 1].Cells[4].Text = "<B><Center>" + GridView2.Rows[count + 1].Cells[4].Text + "</Center></B>";
                    GridView2.Rows[count + 1].Cells[5].Text = "<B><Center>" + GridView2.Rows[count + 1].Cells[5].Text + "</Center></B>";
                    GridView2.Rows[count + 1].Cells[6].Text = "<B><Center>" + GridView2.Rows[count + 1].Cells[6].Text + "</Center></B>";
                    break;
                }
                count++;
            }
        }
        catch (Exception ex)
        {
            Label13.Text = ex.Message;
            Label13.Visible = true;
            return;
        }
        finally
        {
            conn.Close();        //关闭连接        
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "clicked")
        {
            string id = e.CommandArgument.ToString();
            string petID = this.GridView1.DataKeys[Convert.ToInt32(id)].Value.ToString().Trim();
            this.ExcelToDS(petID);
        }  
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (TextBox1.Text == "")
            Response.Write("<script language='JavaScript'>alert('请填写审查代码！')</script>");
        if (!FileUpload1.HasFile)
            Response.Write("<script language='JavaScript'>alert('请选择要上传的文件！')</script>");
        else 
        {
            if (System.IO.Path.GetExtension(FileUpload1.FileName) != ".xls")
            {
                Response.Write("<script language='JavaScript'>alert('文件类型不匹配！')</script>");
                return;
            }
            try
            {
                FileUpload1.SaveAs(Server.MapPath("~/Work/" + TextBox1.Text + ".xls"));
                /*Label13.Text = "客户端路径：" + FileUpload1.PostedFile.FileName + "<br>" +
                              "文件名：" + System.IO.Path.GetFileName(FileUpload1.FileName) + "<br>" +
                              "文件扩展名：" + System.IO.Path.GetExtension(FileUpload1.FileName) + "<br>" +
                              "文件大小：" + FileUpload1.PostedFile.ContentLength + " KB<br>" +
                              "文件MIME类型：" + FileUpload1.PostedFile.ContentType + "<br>" +
                              "保存路径：" + Server.MapPath("~/Work/" + TextBox1.Text + ".xls;");
                Label13.Visible = true;*/
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('"+ ex.Message.ToString() +"')</script>");
                return;
            }
            Response.Write("<script language='JavaScript'>alert('修改成功！')</script>");
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string WorkSymbol = TextBox2.Text;
        string ShopName = TextBox3.Text;
        string GroupName = TextBox4.Text;
        string TeamName = TextBox5.Text;
        string WorkName = TextBox6.Text;
        if(WorkSymbol =="" || ShopName == ""
            || GroupName == "" || TeamName == "" || WorkName == "")
            Response.Write("<script language='JavaScript'>alert('请将信息填写完整！')</script>");
        if (FileUpload2.HasFile)
        {
            if (System.IO.Path.GetExtension(FileUpload2.FileName) != ".xls")
            {
                Response.Write("<script language='JavaScript'>alert('文件类型不匹配！')</script>");
                return;
            }
            try
            {
                FileUpload2.SaveAs(Server.MapPath("~/Work/" + TextBox2.Text + ".xls"));
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message.ToString() + "')</script>");
                return;
            }
        }
        else Response.Write("<script language='JavaScript'>alert('请选择要上传的文件！')</script>");

        string sql = "insert into Work values(@WorkSymbol,@WorkName,@TeamName,@GroupName,@ShopName)";
        command = new SqlCommand(sql, con);
        SqlParameter[] para = new SqlParameter[]
        {
            new SqlParameter("@WorkSymbol", WorkSymbol),
            new SqlParameter("@WorkName", WorkName),
            new SqlParameter("@TeamName", TeamName),
            new SqlParameter("@GroupName", GroupName),
            new SqlParameter("@ShopName", ShopName)
        };
        try
        {
            command.Parameters.AddRange(para);
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            if (command.ExecuteNonQuery() != 1)
            {
                Response.Write("<script language='JavaScript'>alert('添加失败！')</script>");
                return;
            }
            else
            {
                Response.Write("<script language='JavaScript'>alert('添加成功！')</script>");
            }
            con.Close();
        }
        catch (Exception ee)
        {
            Response.Write("<script language='JavaScript'>alert('" + ee.Message.ToString() + "')</script>");
            return;
        }
        bind();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("maintain_route.aspx");
    }
}
