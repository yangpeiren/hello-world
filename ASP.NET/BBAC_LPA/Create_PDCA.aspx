<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Create_PDCA.aspx.cs" Inherits="Create_PDCA" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td width = "45%">
                <asp:Label ID="Label7" runat="server" Text="PDCA名称:"></asp:Label>
            </td>
            <td width = "30%" align = "left">
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
            <td width = "25%">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="TextBox1" ErrorMessage="RequiredFieldValidator" 
                    ValidationGroup="Group1">必须填写PDCA名称</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="审查序列号:"></asp:Label>
            </td>
            <td align = "left" >
                <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="责任人:"></asp:Label>
            </td>
            <td align = "left">
                <asp:Label ID="Label8" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label9" runat="server" Text="所属部门/位置:"></asp:Label>
            </td>
            <td align = "left">
                <asp:Label ID="Label10" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label11" runat="server" Text="生成日期:"></asp:Label>
            </td>
            <td align = "left">
                <asp:Label ID="Label12" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label16" runat="server" Text="审查项:"></asp:Label>
            </td>
            <td align = "left">
                <asp:Label ID="Label17" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align = "left">
                <asp:Button ID="Button3" runat="server" Text="查看审查内容" onclick="Button3_Click" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="Black" 
            GridLines="Both" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" 
                    BorderWidth="1px">
                    <RowStyle HorizontalAlign="Left" Wrap="true" />
                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
                <asp:Label ID="Label18" runat="server" Font-Bold="True" ForeColor="Red" 
                    Text="没有数据,请与管理员联系!" Visible="False"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ContentPlaceHolder2$Button3" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <table style="width:100%;">
        <tr>
            <td width = "45%">
                <asp:Label ID="Label13" runat="server" Text="问题描述:"></asp:Label>
            </td>
            <td colspan="2" align = "left">
                <textarea id="TextArea1" cols="30" name="S1" rows="4"></textarea></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label14" runat="server" Text="问题原因:"></asp:Label>
            </td>
            <td colspan="2" align = "left">
                <textarea id="TextArea2" cols="30" name="S2" rows="4"></textarea></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label15" runat="server" Text="应对措施:"></asp:Label>
            </td>
            <td colspan="2" align = "left">
                <textarea id="TextArea3" cols="30" name="S3" rows="4"></textarea></td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align = "left">
                <asp:Button ID="Button1" runat="server" Text="提交(Submit)" 
                    onclick="Button1_Click" ValidationGroup="Group1" />
            </td>
            <td align = "left">
                <asp:Button ID="Button2" runat="server" Text="退出创建(Exit)" 
                    onclick="Button2_Click" style="height: 26px" />
            </td>
        </tr>
        </table>
</asp:Content>

