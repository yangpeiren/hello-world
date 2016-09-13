<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>请使用真实信息</title>
    <style type="text/css">
        #Select1
        {
            width: 94px;
        }
        .style7
        {
            width: 244px;
            height: 31px;
        }
        .style10
        {
            width: 244px;
            height: 29px;
        }
        .style15
        {
            width: 201px;
            height: 29px;
        }
        .style16
        {
            width: 201px;
            height: 31px;
        }
        .style18
        {
            width: 201px;
            height: 27px;
        }
        .style19
        {
            width: 244px;
            height: 27px;
        }
    </style>
</head>
<body scroll="auto" bgcolor="#ffffff">
    <form id="form1" runat="server">
    <div align = "center">
    
        <asp:Image ID="Image1" runat="server" ImageUrl="~/image/top.jpg" />
        <hr />
        <div align = "center">
        <table style="width: 60%; font: menu;" bgcolor="White" bordercolor="#CCFFCC">
        <tr>
            <td class="style18" align="right">
                <asp:Label ID="emailadd" runat="server" Text="电子邮件(EmailAddress):"></asp:Label>
                </td>
            <td class="style19" align="left">
                <asp:TextBox ID="email" runat="server" Width="238px"></asp:TextBox>
            </td>
            <td class="style19" align="left">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    EnableViewState="False" ErrorMessage="请使用公司内部邮箱注册" 
                    ValidationExpression="\w+([-+.']\w+)*@bbac.com.cn" 
                    ControlToValidate="email"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="style15" align="right">
                <asp:Label ID="realname" runat="server" Text="真实姓名(Name):"></asp:Label>
                </td>
            <td class="style10" align="left">
                <asp:TextBox ID="name" runat="server" Width="238px"></asp:TextBox>
            </td>
            <td class="style10" align="left">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style15" align="right">
                <asp:Label ID="Label1" runat="server" Text="密码(Password):"></asp:Label>
                </td>
            <td class="style10" align="left">
                <asp:TextBox ID="password" runat="server" Width="238px" TextMode="Password"></asp:TextBox>
            </td>
            <td class="style10" align="left">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                    ErrorMessage="请使用8到15位数字或字母" ValidationExpression="^[a-zA-Z0-9]{8,15}$" 
                    ControlToValidate="password"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="style16" align="right">
                <asp:Label ID="Label3" runat="server" Text="确认密码(Confirm Password):"></asp:Label>
                </td>
                
            <td class="style7" align="left">
                <asp:TextBox ID="cpassword" runat="server" Width="238px" 
                    TextMode="Password"></asp:TextBox>
            </td>
                
            <td class="style7" align="left">
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="password" ControlToValidate="cpassword" 
                    ErrorMessage="两次密码输入不一致"></asp:CompareValidator>
            </td>
                
        </tr>
        <tr>
            <td class="style16" align="right">
                <asp:Label ID="Label4" runat="server" Text="车间(Shop):"></asp:Label>
            </td>
                
            <td class="style7" align = "left">
                <asp:DropDownList ID="DropDownList1" runat="server">
                    <asp:ListItem>请选择</asp:ListItem>
                    <asp:ListItem>冲压</asp:ListItem>
                    <asp:ListItem>装焊</asp:ListItem>
                    <asp:ListItem>喷漆</asp:ListItem>
                    <asp:ListItem>总装</asp:ListItem>
                </asp:DropDownList>
            </td>
                
            <td class="style7">
                &nbsp;</td>
                
        </tr>
        <tr>
            <td class="style16" align="right">
                <asp:Button ID="signin" runat="server" Text="注册(Regist)" 
                    onclick="signin_Click1" />
                </td>
                
            <td class="style7">
                <asp:Button ID="back" runat="server" Text="返回(Back)" onclick="back_Click" />
            </td>
                
            <td class="style7">
                &nbsp;</td>
                
        </tr>
        </table>
    </div>
    </div>
    
    </form>
</body>
</html>
