<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Check_Maintain.aspx.cs" Inherits="Check_Maintain" %>

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
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
            CellPadding="3" ForeColor="Black" GridLines="Vertical" OnRowDeleting = "Gridview1_RowDeleteing">
            <Columns>
                <asp:BoundField DataField="CheckNO" HeaderText="序列号" ReadOnly="True" />
                <asp:BoundField DataField="WorkSymbol" HeaderText="LPA审查代码" ReadOnly="True" 
                    SortExpression="WorkSymbol" />
                <asp:BoundField DataField="Checker" HeaderText="检查人身份" 
                    SortExpression="Checker" />
                <asp:BoundField DataField="CheckerName" HeaderText="检查人姓名" 
                    SortExpression="CheckerName" />
                <asp:BoundField DataField="CheckerID" HeaderText="检查人ID" 
                    SortExpression="CheckerID" />
                <asp:BoundField DataField="ID" HeaderText="身份" ReadOnly="True" 
                    SortExpression="ID" />
                <asp:BoundField DataField="Checktime" HeaderText="生成日期" ReadOnly="True" 
                    SortExpression="Checktime" />
                <asp:CommandField ButtonType="Button" HeaderText="删除" ShowDeleteButton="True" 
                    ShowHeader="True" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="#CCCCCC" />
        </asp:GridView>
        <table style="width:100%;">
            <tr>
                <td>
                    &nbsp;</td>
                <td align="left">
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
