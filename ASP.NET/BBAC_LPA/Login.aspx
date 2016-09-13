<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>登录</title>
    <style type="text/css">
        .style3
        {
        	width : 35%;
            height: 30px;
        }
        .style4
        {
        	width : 35%;
            height: 30px;
        }
        .style7
        {
            width: 30%;
            height: 30px;
        }
        .style12
        {
            height: 30px;
        }
        .style13
        {
            height: 30px;
        }
        .mm
        {
        	width :"25%";
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align = "center">
    
        <asp:Image ID="Image1" runat="server" ImageUrl="~/image/top.jpg"/>
    
    </div>
    <div align = "center">
        <hr />
    <table width = "50%">
        <tr>
            <td class="style7" align="left">
                <asp:Label ID="emailadd" runat="server" Text="电子邮件(EmailAddress):"></asp:Label>
                </td>
            <td class="style3" bordercolor="#CCCCFF" align="left">
                <asp:TextBox ID="email" runat="server" Width="238px"></asp:TextBox>
                </td>
            <td class="mm" bordercolor="#CCCCFF" align="left">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    EnableViewState="False" ErrorMessage="请用邮箱登录" 
                    ValidationExpression="\w+([-+.']\w+)*@bbac.com.cn" 
                    ControlToValidate="email"></asp:RegularExpressionValidator>
                </td>
            <td width = "10%" bordercolor="#CCCCFF">
                <asp:HyperLink 
                    ID="HyperLink2" runat="server" NavigateUrl="~/Default.aspx" TabIndex="6">注册</asp:HyperLink></td>
        </tr>
        <tr>
            <td class="style7" align="left">
                <asp:Label ID="Label1" runat="server" Text="密码(Password):"></asp:Label></td>
            <td bordercolor="#CCCCFF" class="style3" align="left">
                <asp:TextBox ID="password" runat="server" Width="238px" TextMode="Password"></asp:TextBox>
                </td>
            
            <td bordercolor="#CCCCFF" class="mm" align="left">
                <img alt="" src="image/system-lock-screen.png"  /></td>
            <td bordercolor="#CCCCFF">
                &nbsp;</td>
        </tr>
        <tr>
            <td >
                </td>
            <td class="style12" align="left">
                <asp:RadioButton ID="RadioButton1" runat="server" Checked="True" GroupName="G1" 
                    Text="用户(User)" />
&nbsp;&nbsp;&nbsp;
                <asp:RadioButton ID="RadioButton2" runat="server" GroupName="G1" 
                    Text="管理员(Admin)" />
                </td>
            <td class="style12" align="left">
                &nbsp;</td>
            <td >
                </td>
        </tr>
        <tr>
            <td class="style13" >
                </td>
            <td bordercolor="#CCCCFF" align="left" class="style13">
                </td>
            <td bordercolor="#CCCCFF" align="left" class="style13">
                &nbsp;</td>
            <td bordercolor="#CCCCFF" class="style13" >
                </td>
        </tr>
        <tr>
            <td >
                &nbsp;</td>
            <td bordercolor="#CCCCFF" class="style4" align="left">
                <asp:Button ID="Log" runat="server" Text="登录(Login)" 
                    onclick="signin_Click1" />
                </td>
            <td bordercolor="#CCCCFF" class="mm" align="left">
                <asp:Button ID="rst" runat="server" Text="重置(Reset)" onclick="back_Click" />
                </td>
            <td bordercolor="#CCCCFF">
                &nbsp;</td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
