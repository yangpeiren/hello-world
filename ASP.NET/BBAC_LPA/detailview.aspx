<%@ Page Language="C#" AutoEventWireup="true" CodeFile="detailview.aspx.cs" Inherits="detailview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    </head>
    <body>
            <form id="form1" runat="server">
            <div align = "center">
            <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                ForeColor="Black" GridLines="Vertical" AllowPaging="False" 
                AutoGenerateColumns="False">
                <Columns>
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
            </div>
            </form>
</body>
</html>

