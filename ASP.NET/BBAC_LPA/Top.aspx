<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Top.aspx.cs" Inherits="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat = "server">
<title>管理页面</title>
<script language=JavaScript1.2>
function showsubmenu(sid) {
	var whichEl = eval("submenu" + sid);
	var menuTitle = eval("menuTitle" + sid);
	if (whichEl.style.display == "none"){
		eval("submenu" + sid + ".style.display=\"\";");
	}else{
		eval("submenu" + sid + ".style.display=\"none\";");
	}
}
</script>
<meta http-equiv=Content-Type content=text/html;charset=gb2312>
<script language=JavaScript>
    function logout() {
        if (confirm("您确定要退出吗？"))
            top.location = "Login.aspx";
        return false;
    }
</script>
<script language=JavaScript1.2>
function showsubmenu(sid) {
	var whichEl = eval("submenu" + sid);
	var menuTitle = eval("menuTitle" + sid);
	if (whichEl.style.display == "none"){
		eval("submenu" + sid + ".style.display=\"\";");
	}else{
		eval("submenu" + sid + ".style.display=\"none\";");
	}
}
</script>
<meta http-equiv=Content-Type content=text/html;charset=gb2312>
<script language=JavaScript1.2>
function showsubmenu(sid) {
	var whichEl = eval("submenu" + sid);
	var menuTitle = eval("menuTitle" + sid);
	if (whichEl.style.display == "none"){
		eval("submenu" + sid + ".style.display=\"\";");
	}else{
		eval("submenu" + sid + ".style.display=\"none\";");
	}
}
</script>
<base target="main"/>
<link href="~/image/skin.css" rel="stylesheet" type="text/css">
<style type = "text/css">
.admin_txt {
	font-family: Arial, Helvetica, sans-serif;
	font-size: 12px;
	color: #FFFFFF;
	text-decoration: none;
	height: 38px;
	width: 100%;
	position: fixed;
	line-height: 38px;
}
.admin_topbg {
	background-image: url(image/top-right.gif);
	background-repeat: repeat-x;
}
    .style1
    {
        font-family: Arial, Helvetica, sans-serif;
        font-size: 12px;
        color: #FFFFFF;
        text-decoration: none;
        height: 38px;
        width: 100%;
        position: fixed;
        line-height: 38px;
    }
    .style2
    {
        height: 38px;
    }
</style>
</head>
<body leftmargin="0" topmargin="0">
<table id="Table1" runat = "server" width="100%" height="64" border="0" cellpadding="0" cellspacing="0" 
background ="image/top-right.gif"  class="admin_topbg">
  <tr>
    <td width="61%" height="64" background="image/top-right.gif">
    <img src="image/logo.jpg" width="262" height="64"></td>
    <td width="39%" valign="top">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" background ="image/top-right.gif">
      <tr>
        <td width="74%" class="style1"><b>欢迎:<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></b></td>
        <td width="22%" class="style2"><a href="#" target="_self" onClick="logout();">
            <img src="image/out.gif" alt="安全退出" width="46" height="20" border="0"></a></td>
        <td width="4%" class="style2"></td>
      </tr>
      <tr>
        <td height="19" colspan="3">&nbsp;</td>
        </tr>
    </table></td>
  </tr>
</table>
</body>
</html>


