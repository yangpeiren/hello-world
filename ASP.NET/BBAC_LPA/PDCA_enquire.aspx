<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PDCA_enquire.aspx.cs" Inherits="PDCA_enquire" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td>
                &nbsp;</td>
            <td align="right">
                <asp:Label ID="Label5" runat="server" Text="PDCA状态:"></asp:Label>
            </td>
            <td align = "center">
                <asp:DropDownList ID="DropDownList2" runat="server">
                    <asp:ListItem Value="-1">全部(All)</asp:ListItem>
                    <asp:ListItem Value="1">打开(Open)</asp:ListItem>
                    <asp:ListItem Value="0">关闭(Closed)</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="right">
                <asp:Label ID="Label4" runat="server" Text="查询时间段:"></asp:Label>
            </td>
            <td align = "center">
                <asp:DropDownList ID="DropDownList1" runat="server">
                </asp:DropDownList>
            </td>
            <td align="left">
                <asp:Button ID="Button1" runat="server" Text="查询(GO)" onclick="Button1_Click" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td align = "left">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                CellPadding="3" ForeColor="Black" GridLines="Vertical" OnRowCommand = "GridView1_RowCommand"
                 RowStyle-Width = "100" DataKeyNames = "ProblemNO">
                <FooterStyle BackColor="#CCCCCC" />
                <Columns>
                <asp:TemplateField HeaderText = "编号" ItemStyle-HorizontalAlign = 'Center'>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# Eval("ProblemNO") %>'
                         CommandName = 'numclick' >LinkButton</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:BoundField HeaderText="LPA序列号" ReadOnly="True" DataField = "CheckNO" ItemStyle-Width = "100"/>
                    <asp:BoundField HeaderText="PDCA名称" ReadOnly="True" DataField = "Pname" ItemStyle-Width = "100"/>
                    <asp:BoundField HeaderText="创建人" ReadOnly="True" DataField = "CreatedBy" ItemStyle-Width = "100"/>
                    <asp:BoundField HeaderText="创建日期" ReadOnly="True" DataField = "CreatedDate" ItemStyle-Width = "100"/>
                    <asp:BoundField HeaderText="创建部门" ReadOnly="True" DataField = "Depart" ItemStyle-Width = "100"/>
                    <asp:BoundField HeaderText="问题描述" DataField = "PD" ItemStyle-Width = "100"/>
                    <asp:BoundField HeaderText="问题原因" DataField = "RC" ItemStyle-Width = "100"/>
                    <asp:BoundField HeaderText="应对措施" DataField = "CM" ItemStyle-Width = "100"/>
                </Columns>
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#CCCCCC" />
            </asp:GridView>
            <asp:Label ID="Label1" runat="server" Text="本月没有数据！" Font-Bold="True" 
                Font-Size="Medium" ForeColor="Red" Visible="False"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ContentPlaceHolder2$Button1" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

