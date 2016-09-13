<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="enquire_individual.aspx.cs" Inherits="enquire_individual" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td align="right">
                <asp:Label ID="Label4" runat="server" Text="姓名(Name):"></asp:Label>
            </td>
            <td align = "center">
                <asp:DropDownList ID="DropDownList1" runat="server">
                </asp:DropDownList>
            </td>
            <td align = "left">
                <asp:Button ID="Button1" runat="server" Text="查询(GO)" onclick="Button1_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div align = "center">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#999999" 
                BorderStyle="Solid" BorderWidth="1px" CaptionAlign="Top" CellPadding="3" 
                ForeColor="Black" GridLines="Vertical" 
                AutoGenerateColumns="False" PageSize="20" AllowPaging="True"
                 OnPageIndexChanging = "GridView1_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText = "CW">
                        <HeaderTemplate>
                            <asp:DropDownList ID = "Dropdownlist5" runat = "server" AutoPostBack = "true"
                OnSelectedIndexChanged = "Dropdownlist5_SelectedIndexChanged">
                            </asp:DropDownList>
                        </HeaderTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "识别码">
                <ItemTemplate>
                    <asp:label ID = "label1" runat = "server" Text='<%# Eval("code") %>'></asp:label>                  
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText = "审查代码">
                <ItemTemplate>
                    <asp:label ID = "label2" runat = "server" Text='<%# Eval("worksymbol") %>'></asp:label>                   
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText = "审查人身份">
                <ItemTemplate>
                    <asp:label ID = "label3" runat = "server" Text='<%# Eval("checker") %>'></asp:label>                   
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText = "责任人">
                <ItemTemplate>
                    <asp:label ID = "label4" runat = "server" Text='<%# Eval("checkername") %>'></asp:label>                   
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText = "检查时间">
                <ItemTemplate>
                    <asp:label ID = "label5" runat = "server" Text='<%# Eval("checktime") %>'></asp:label>                  
                </ItemTemplate>
                </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#CCCCCC" />
            </asp:GridView>
            <asp:Label ID="Label6" runat="server" Text="没有任何数据！" ForeColor="Red" 
                Visible="False" Font-Bold="True"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID='ContentPlaceHolder2$Button1' EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    </div>
    <hr />
</asp:Content>

