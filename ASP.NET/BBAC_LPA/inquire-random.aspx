<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="inquire-random.aspx.cs" Inherits="inquire_random" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style6
        {
            width: 20%;
        }
        .style9
        {
            width: 20%;
        }
        .hidden { display:none;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <table style="width:100%;" align = "left">
    <tr>
        <td class="style6" align="right">
            &nbsp;</td>
        <td width = "15%" align="right">
            <asp:Label ID="Label4" runat="server" Text="开始日期(Start):"></asp:Label>
        </td>
        <td width = "15%" align="right">
                <asp:Textbox runat = "server" CssClass ="Wdate"  type="text" id = "TextBox2"
                onClick="WdatePicker({isShowClear:false,readOnly:true,firstDayOfWeek:1})" />
        </td>
        <td width = "15%" align="right">
            <asp:Label ID="Label5" runat="server" Text="截止日期(End):"></asp:Label>
        </td>
        <td width = "15%" align="left">
                <asp:Textbox runat = "server" CssClass ="Wdate"  type="text" id = "TextBox3"
                
                onClick="WdatePicker({isShowClear:false,readOnly:true,firstDayOfWeek:1})" />
        </td>
        <td class="style9" align="left">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style6" align="right">
            &nbsp;</td>
        <td width = "15%" align="right">
            &nbsp;</td>
        <td width = "15%" align="right">
            &nbsp;</td>
        <td width = "15%" align="right">
            &nbsp;</td>
        <td width = "15%" align="left">
                &nbsp;</td>
        <td class="style9" align="left">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style6">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td width = "15%">
            <asp:Button ID="Button1" runat="server" Text="查询(GO)" onclick="Button1_Click" />
        </td>
        <td class="style9">
            &nbsp;</td>
    </tr>
</table>
    <hr />
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
                <label>Custom</label>
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
                        <asp:Label ID="Label12" runat="server" Text='<%# Eval("ShopName") %>'></asp:Label>
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
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("GroupName") %>'></asp:Label>
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
                        <asp:Label ID="Label14" runat="server" Text='<%# Eval("TeamName") %>'></asp:Label>
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
            <asp:AsyncPostBackTrigger ControlID='ContentPlaceHolder2$Button1' EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    </div>
    <table style="width:100%;">
        <tr>
            <td>
                &nbsp;</td>
            <td align="center">
                
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="Button2" runat="server" Text="导出Excel" 
                    onclick="Button2_Click" />
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
    </table>
</asp:Content>

