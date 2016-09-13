<%@ Page Language="C#" AutoEventWireup="true" CodeFile="System_admin.aspx.cs" Inherits="System_admin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table style="width:100%;">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="欢迎:"></asp:Label>
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td align="right">
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Login.aspx">退出</asp:HyperLink>
                </td>
            </tr>
        </table>
        <hr />
    </div>
        
                <div>
        
                <asp:GridView ID="GridView1" runat="server" BackColor="White" 
            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
            ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" OnRowCommand = "GridView1_RowCommand"
                        OnRowDeleting = "GridView_OnRowDeleting"
                        OnRowEditing = "GridView_OnRowEditing" OnRowUpdating = "GridView_OnRowUpdating"
                        OnRowCancelingEdit = "GridView_OnRowCancelingEdit">
            <FooterStyle BackColor="#CCCCCC" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
            <asp:BoundField HeaderText = "ID" DataField = "ID" ReadOnly = "true"/>
            <asp:BoundField HeaderText = "上级ID" DataField = "SuperID" 
                    ControlStyle-Width = "100%">
<ControlStyle Width="100%"></ControlStyle>
                </asp:BoundField>
            <asp:BoundField HeaderText = "姓名" DataField = "Name" ReadOnly = "true"/>
            <asp:BoundField HeaderText = "职位" DataField = "Position" 
                    ControlStyle-Width = "100%">
<ControlStyle Width="100%"></ControlStyle>
                </asp:BoundField>
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
            <asp:BoundField HeaderText = "组别" DataField = "WorkGroup" 
                    ControlStyle-Width = "100%">
<ControlStyle Width="100%"></ControlStyle>
                </asp:BoundField>
            <asp:BoundField HeaderText = "用户名" DataField = "UserName" ReadOnly = "true"/>
            <asp:ButtonField  HeaderText = "密码" ButtonType = "Button" CommandName = "click" Text = "重置密码" />
            <asp:BoundField HeaderText = "权限" DataField = "Limit" ControlStyle-Width = "100%">
<ControlStyle Width="100%"></ControlStyle>
                </asp:BoundField>
                <asp:ButtonField HeaderText="账号状态" ButtonType = "Button" CommandName = "change" DataTextField = "Active"/>
            <asp:CommandField HeaderText = "编辑"  ShowEditButton = "true" />
            <asp:CommandField HeaderText = "删除" ShowDeleteButton = "true" />
            </Columns>
        </asp:GridView>
        
                    <hr />
                    <table style="width:100%;">
                        <tr>
                            <td>
                                &nbsp;</td>
                    <td align="center">
                        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                            Text="后退(Back)" />
                    </td>
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
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </div>
        
    </form>
</body>
</html>
