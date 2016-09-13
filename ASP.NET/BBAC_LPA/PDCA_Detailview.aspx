<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PDCA_Detailview.aspx.cs" Inherits="PDCA_Detailview" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td align="center">
                <asp:Label ID="Label4" runat="server" Text="PDCA状态:"></asp:Label>
            </td>
            <td align="left">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td align="left">
                <asp:Button ID="Button1" runat="server" Text="关闭PDCA" onclick="Button1_Click" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div align="left">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:DetailsView ID="DetailsView1" runat="server" 
                Height="50px" Width="80%" AutoGenerateRows="False" BackColor="White" 
                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    GridLines="Vertical" OnModeChanging = "DetailsView1_ModeChaning"
                    OnItemUpdating = "DetailsView1_ItemUpdating">
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
    <Fields>
        <asp:BoundField HeaderText="PDCA编号:" ReadOnly="True" DataField = "ProblemNO"
        ItemStyle-Width = "80%" ItemStyle-HorizontalAlign = "Center">
            <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:BoundField>
    <asp:BoundField HeaderText="LPA序列号:" ReadOnly="True" DataField = "CheckNO" 
            ItemStyle-Width = "80%" ItemStyle-HorizontalAlign = "Center">
        <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:BoundField>
                    <asp:BoundField HeaderText="PDCA名称:" ReadOnly="True" 
            DataField = "Pname" ItemStyle-Width = "80%" ItemStyle-HorizontalAlign = "Center">
                        <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:BoundField>
                    <asp:BoundField HeaderText="创建人:" ReadOnly="True" 
            DataField = "CreatedBy" ItemStyle-Width = "80%" ItemStyle-HorizontalAlign = "Center">
                        <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:BoundField>
                    <asp:BoundField HeaderText="创建日期:" ReadOnly="True" 
            DataField = "CreatedDate" ItemStyle-Width = "80%" ItemStyle-HorizontalAlign = "Center">
                        <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:BoundField>
                    <asp:BoundField HeaderText="创建部门:" ReadOnly="True" 
            DataField = "Depart" ItemStyle-Width = "80%" ItemStyle-HorizontalAlign = "Center">
                        <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="问题描述:">
        <EditItemTemplate>
                <textarea ID="TextArea1" cols="40" name="S1" rows="5"><%# Eval("CM")%></textarea>
            </EditItemTemplate>
            <InsertItemTemplate>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("PD") %>'></asp:TextBox>
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("PD") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="问题原因:">
        <EditItemTemplate>
                <textarea ID="TextArea2" cols="40" name="S2" rows="5"><%# Eval("CM")%></textarea>
            </EditItemTemplate>
            <InsertItemTemplate>
                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("RC") %>'></asp:TextBox>
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label2" runat="server" Text='<%# Bind("RC") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="应对措施:">    
            <EditItemTemplate>
                <textarea ID="TextArea3" cols="40" name="S3" rows="5"><%# Eval("CM")%></textarea>
            </EditItemTemplate>
            <InsertItemTemplate>
                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("CM") %>'></asp:TextBox>
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label3" runat="server" Text='<%# Bind("CM") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:TemplateField>
                    <asp:BoundField HeaderText="PDCA状态:" DataField = "Pstate" 
            ItemStyle-Width = "80%" ItemStyle-HorizontalAlign = "Center" 
            ReadOnly="True">
                        <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:BoundField>
                    <asp:BoundField HeaderText="关闭责任人:" DataField = "ClosedBy" 
            ItemStyle-Width = "80%" ItemStyle-HorizontalAlign = "Center" 
            ReadOnly="True">
                        <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:BoundField>
                    <asp:BoundField HeaderText="关闭日期:" DataField = "CloseDate" 
            ItemStyle-Width = "80%" ItemStyle-HorizontalAlign = "Center" 
            ReadOnly="True">
                        <ItemStyle HorizontalAlign="Center" Width="80%" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="编辑" ShowHeader="False">
            <EditItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" 
                    CommandName="Update" Text="更新" ></asp:LinkButton>
                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                    CommandName="Cancel" Text="取消"></asp:LinkButton>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                    CommandName="Edit" Text="编辑"></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
    </Fields>
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#DCDCDC" />
                </asp:DetailsView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

