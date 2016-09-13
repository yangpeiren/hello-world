<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="enquire.aspx.cs" Inherits="enquire" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">

        .hidden { display:none;}
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <table style="width:100%;">
        <tr style = "width:35%;">
            <td align="right">
                <asp:Label ID="Label4" runat="server" Text="组别(Group):"></asp:Label>
            </td>
            <td align = "center">
                <asp:DropDownList ID="DropDownList1" runat="server">
                    <asp:ListItem Selected="True">请选择</asp:ListItem>
                    <asp:ListItem>A</asp:ListItem>
                    <asp:ListItem>B</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align = "left">
                <asp:Button ID="Button1" runat="server" Text="查询(GO)" onclick="Button1_Click" 
                    style="height: 26px" />
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
                AutoGenerateColumns="False"  DataKeyNames = "ShopName,GroupName,TeamName"
                 OnRowCommand = "GridView1_RowCommand" PageSize="15"
                 OnPageIndexChanging = "GridView1_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText = "CW">
                        <HeaderTemplate>
                            <asp:DropDownList ID = "Dropdownlist5" runat = "server" AutoPostBack = "true"
                OnSelectedIndexChanged = "Dropdownlist5_SelectedIndexChanged">
                            </asp:DropDownList>
                        </HeaderTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "Shop">
                        <HeaderTemplate>
                            <asp:DropDownList ID = "Dropdownlist2" runat = "server" AutoPostBack = "true" 
                OnSelectedIndexChanged = "Dropdownlist2_SelectedIndexChanged">
                                <asp:ListItem>车间</asp:ListItem>
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("ShopName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "Group">
                        <HeaderTemplate>
                            <asp:DropDownList ID = "Dropdownlist3" runat = "server" AutoPostBack = "true"
                OnSelectedIndexChanged = "Dropdownlist3_SelectedIndexChanged">
                                <asp:ListItem>工段</asp:ListItem>
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("GroupName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "Team">
                        <HeaderTemplate>
                            <asp:DropDownList ID = "Dropdownlist4" runat = "server" AutoPostBack = "true"
                OnSelectedIndexChanged = "Dropdownlist4_SelectedIndexChanged">
                                <asp:ListItem>班组</asp:ListItem>
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("TeamName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "TL Check" ItemStyle-HorizontalAlign = 'Center' >
                        <ItemTemplate>
                            <asp:linkbutton ID = "link1" runat = "server" Text='<%# Eval("TLCheck") %>' 
                    CommandName = "TLClick" ></asp:linkbutton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "GL Check" ItemStyle-HorizontalAlign = 'Center'>
                        <ItemTemplate>
                            <asp:linkbutton ID = "link2" runat = "server" Text='<%# Eval("GLCheck") %>'
                    CommandName = "GLClick"></asp:linkbutton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "SM Check" ItemStyle-HorizontalAlign = 'Center'>
                        <ItemTemplate>
                            <asp:linkbutton ID = "link3" runat = "server" Text='<%# Eval("SMCheck") %>'
                    CommandName = "SMClick"></asp:linkbutton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "GM Check" ItemStyle-HorizontalAlign = 'Center'>
                        <ItemTemplate>
                            <asp:linkbutton ID = "link4" runat = "server" Text='<%# Eval("GMCheck") %>'
                    CommandName = "GMClick"></asp:linkbutton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#CCCCCC" />
            </asp:GridView>
            <hr />
            <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" 
            Text="没有数据！" Visible="False"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID='ContentPlaceHolder2$Button1' EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    </div>
    
</asp:Content>

