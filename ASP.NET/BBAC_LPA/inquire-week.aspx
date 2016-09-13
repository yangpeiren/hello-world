<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="inquire-week.aspx.cs" Inherits="inquire_week" Title="无标题页"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style7
        {
            width: 20%px;
        }
        .hidden { display:none;}
        .style8
        {
            width: 40%;
        }
        .style9
        {
            width: 85px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    
    <table style="width:100%;">
        <tr>
            <td class="style8" align="right">
                <asp:Label ID="Label3" runat="server" Text="周次(Week):"></asp:Label>
            </td>
            <td class="style7">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Columns="3" MaxLength="2"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td align = "left" width = "40%">
                <asp:Button ID="Button1" runat="server" style="margin-left: 0px" 
                    Text="本周(This Week)" onclick="Button1_Click" />
            </td>
        </tr>
        <tr>
            <td class="style8">
                &nbsp;</td>
            <td class="style7">
                &nbsp;</td>
            <td align="left">
                <asp:Button ID="Button3" runat="server" Text="查询(GO)" onclick="Button3_Click" />
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
                 OnRowCommand = "GridView1_RowCommand" PageSize="15" >
                <Columns>
                <asp:TemplateField HeaderText = "CW">
                <HeaderTemplate>
                <asp:DropDownList ID = "Dropdownlist1" runat = "server"
                OnSelectedIndexChanged = "Dropdownlist1_SelectedIndexChanged" Enabled="False">
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
                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("ShopName") %>'></asp:Label>
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
            
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID='ContentPlaceHolder2$Button3' EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
            <hr />
        </div>
            <asp:Label ID="Label7" runat="server" Text="没有任何数据！" ForeColor="Red" 
                Visible="False" Font-Bold="True"></asp:Label>
    <table style="width:100%;">
        <tr>
            <td class="style9">
                &nbsp;</td>
            <td align="center">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style9">
                &nbsp;</td>
            <td>
                <asp:Button ID="Button2" runat="server" Text="导出Excel" 
                    onclick="Button2_Click" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style9">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
</table>
    
    </asp:Content>

