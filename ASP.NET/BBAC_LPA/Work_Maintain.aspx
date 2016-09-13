<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Work_Maintain.aspx.cs" Inherits="Work_Maintain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
    
        <table style="width:100%;">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="欢迎:"></asp:Label>
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Overline="False" 
                        Font-Underline="True" ForeColor="Red" Text="Label" Visible="False"></asp:Label>
                </td>
                <td align="right">
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Login.aspx">退出</asp:HyperLink>
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
            CellPadding="3" ForeColor="Black" GridLines="Vertical"
                        OnRowDeleting = "GridView_OnRowDeleting" OnRowCommand = "GridView1_RowCommand"
                        OnRowEditing = "GridView_OnRowEditing" OnRowUpdating = "GridView_OnRowUpdating"
                        OnRowCancelingEdit = "GridView_OnRowCancelingEdit">
            <FooterStyle BackColor="#CCCCCC" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
            <asp:BoundField HeaderText = "审查代码" DataField = "WorkSymbol" ReadOnly = "true"/>
            <asp:BoundField HeaderText = "车间" DataField = "ShopName" 
                    ControlStyle-Width = "100%">
<ControlStyle Width="100%"></ControlStyle>
                </asp:BoundField>
            <asp:BoundField HeaderText = "工段" DataField = "GroupName" 
                    ControlStyle-Width = "100%">
<ControlStyle Width="100%"></ControlStyle>
                </asp:BoundField>
            <asp:BoundField HeaderText = "班组" DataField = "TeamName" 
                    ControlStyle-Width = "100%">
<ControlStyle Width="100%"></ControlStyle>
                </asp:BoundField>
            <asp:BoundField HeaderText = "工作名" DataField = "WorkName" 
                    ControlStyle-Width = "100%">
<ControlStyle Width="100%"></ControlStyle>
                </asp:BoundField>
                <asp:ButtonField ButtonType="Button" CommandName="clicked" HeaderText="工作内容" 
                    ShowHeader="True" Text="查看" ItemStyle-HorizontalAlign = 'Center'/>
                    <asp:CommandField HeaderText = "编辑" ShowEditButton = "true" />
                    <asp:CommandField HeaderText = "删除" ShowDeleteButton = "true" />
                
            </Columns>
        </asp:GridView>
    
        <hr />
    
    </div>
    <div align = "center">
        <asp:GridView ID="GridView2" runat="server" BackColor="White" 
            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
            CellPadding="4" ForeColor="Black" GridLines= "Both">
            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
            <RowStyle HorizontalAlign=  "Left" />
        </asp:GridView>
        </div>
        <div align = "left">
        <table style="width:100%;">
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="修改工作内容:"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="审查代码:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" MaxLength="5"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label12" runat="server" Text="工作内容:"></asp:Label>
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="确认修改" onclick="Button1_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="新建工作:"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="审查代码:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server" MaxLength="5"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="车间:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="工段:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label9" runat="server" Text="班组:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="Label10" runat="server" Text="工作名:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label11" runat="server" Text="工作内容:"></asp:Label>
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload2" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
                        Text="后退(Back)" />
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="Button2" runat="server" Text="确认新建" onclick="Button2_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
