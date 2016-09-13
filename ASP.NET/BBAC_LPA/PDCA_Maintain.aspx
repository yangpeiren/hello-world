<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PDCA_Maintain.aspx.cs" Inherits="PDCA_Maintain" %>

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
        <div>
            <asp:Label ID="Label3" runat="server" Text="已关闭的PDCA" ForeColor="Red"></asp:Label>
        </div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                CellPadding="3" ForeColor="Black" GridLines="Vertical" OnRowCommand = "GridView1_RowCommand"
                 RowStyle-Width = "100" DataKeyNames = "ProblemNO" OnRowDeleting = "GridView_OnRowDeleting"
                OnRowEditing = "GridView_OnRowEditing" OnRowUpdating = "GridView_OnRowUpdating"
                OnRowCancelingEdit = "GridView_OnRowCancelingEdit">
                <FooterStyle BackColor="#CCCCCC" />
<RowStyle Width="100px"></RowStyle>
                <Columns>
                <asp:BoundField HeaderText="编号" ReadOnly="True" DataField = "ProblemNO" ItemStyle-HorizontalAlign = 'Center'
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="LPA序列号" ReadOnly="True" DataField = "CheckNO"  
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="PDCA名称"  DataField = "Pname" 
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="创建人" ReadOnly = "true" DataField = "CreatedBy" 
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="创建日期"  DataField = "CreatedDate" 
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="创建部门"  DataField = "Depart" 
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="问题描述" DataField = "PD" ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="问题原因" DataField = "RC" ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="应对措施" DataField = "CM" ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:ButtonField ButtonType="Button" CommandName="change" HeaderText="状态" 
                        ShowHeader="True" DataTextField = "Pstate"/>
                    <asp:BoundField HeaderText="关闭责任人" ReadOnly = "true" ItemStyle-Width = "100" 
                        DataField = "ClosedBy">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="关闭日期" ItemStyle-Width = "100" 
                        DataField = "CloseDate">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:CommandField HeaderText="编辑" ShowEditButton="True" ShowHeader="True" />
                    <asp:CommandField HeaderText="删除" ShowDeleteButton="True" ShowHeader="True" />
                </Columns>
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#CCCCCC" />
            </asp:GridView>
        <hr />
        <div>
            <asp:Label ID="Label4" runat="server" Text="未关闭的PDCA" ForeColor="Red"></asp:Label>
        </div>
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                CellPadding="3" ForeColor="Black" GridLines="Vertical"
                 RowStyle-Width = "100" DataKeyNames = "ProblemNO" OnRowDeleting = "GridView2_OnRowDeleting"
                OnRowEditing = "GridView2_OnRowEditing" OnRowUpdating = "GridView2_OnRowUpdating"
                OnRowCancelingEdit = "GridView2_OnRowCancelingEdit">
                <FooterStyle BackColor="#CCCCCC" />
<RowStyle Width="100px"></RowStyle>
                <Columns>
                <asp:BoundField HeaderText="编号" ReadOnly="True" DataField = "ProblemNO"  ItemStyle-HorizontalAlign = 'Center'
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="LPA序列号" ReadOnly="True" DataField = "CheckNO" 
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="PDCA名称"  DataField = "Pname" 
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="创建人"  DataField = "CreatedBy" 
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="创建日期"  DataField = "CreatedDate" 
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="创建部门"  DataField = "Depart" 
                        ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="问题描述" DataField = "PD" ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="问题原因" DataField = "RC" ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="应对措施" DataField = "CM" ItemStyle-Width = "100">
<ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:CommandField HeaderText="编辑" ShowEditButton="True" ShowHeader="True" />
                    <asp:CommandField HeaderText="删除" ShowDeleteButton="True" ShowHeader="True" />
                </Columns>
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#CCCCCC" />
            </asp:GridView>
    
    </div>
    <div>
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
