<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="cpassword.aspx.cs" Inherits="cpassword" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style4
        {
            height: "40px";
        }
        .mm
        {
        	height:"40px";
        }
        .nn
        {
        	height:"40px";
        }
        .ss
        {
        	height:"40px";
        }
        .style5
        {
            height: "40px";
        }
        .style6
        {
            height: "40px";
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div align = "center">
    <table style="width:80%;">
        <tr>
            <td width="38%" align="right" class="style4"></td>
            <td width="34%" class = "ss">&nbsp;
                </td>
            <td width="5%" class = "style5">&nbsp;
                </td>
            <td width="23%" class = "nn">&nbsp;
                </td>
        </tr>
        <tr>
            <td align="right" class="style4">
                <asp:Label ID="Label4" runat="server" Text="旧密码&lt;br&gt;(Old Password):"></asp:Label>
            </td>
            <td class = "ss" align = "right">
                <asp:TextBox ID="password0" runat="server" Width="238px" TextMode="Password"></asp:TextBox>
                </td>
            <td align = "left" class = "style5">
                <img alt="" src="image/system-lock-screen.png"  /></td>
            <td align="left" class = "nn">&nbsp;
                </td>
        </tr>
        <tr>
            <td align="right" class="style4">
                <asp:Label ID="Label1" runat="server" Text="密码(Password):"></asp:Label>
            </td>
            <td class="ss" align = "right">
                <asp:TextBox ID="password" runat="server" Width="238px" TextMode="Password"></asp:TextBox>
                </td>
            <td align = "left" class = "style5">
                <img alt="" src="image/system-lock-screen.png"  /></td>
            <td align="left" class = "nn">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                    ErrorMessage="请使用8到15位数字或字母" ValidationExpression="^[a-zA-Z0-9]{8,15}$" 
                    ControlToValidate="password"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td align="right" class="style4">
                <asp:Label ID="Label3" runat="server" Text="确认密码&lt;br&gt;(Confirm Password):"></asp:Label>
                </td>
            <td class="ss" align = "right">
                <asp:TextBox ID="cppassword" runat="server" Width="238px" 
                    TextMode="Password"></asp:TextBox>
                </td>
            <td align = "left" class = "style5">
                <img alt="" src="image/system-lock-screen.png"  /></td>
            <td align="left" class = "nn">
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="password" ControlToValidate="cppassword" 
                    ErrorMessage="两次密码输入不一致"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td class="style4">&nbsp;
                </td>
            <td align="right" class = "ss">&nbsp;
                </td>
            <td class = "style5">&nbsp;
                </td>
            <td class = "nn">&nbsp;
                </td>
        </tr>
        <tr>
            <td class="style4">&nbsp;
                </td>
            <td align="center" class = "ss">
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="确定(Submit)" />
            </td>
            <td class = "style5">&nbsp;
                </td>
            <td class = "nn">&nbsp;
                </td>
        </tr>
    </table>
    </div>
</asp:Content>


