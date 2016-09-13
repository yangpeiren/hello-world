<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Check.aspx.cs" Inherits="Check" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">

        .style43
        {
            height: 23px;
            width: 199px;
        }
        .style45
        {
            height: 23px;
            width: 114px;
        }
        .style8
        {
            height: 23px;
            width: 124px;
        }
        .stylefree
        {
        	width : "1%";
        }
        .style18
        {
            height: 23px;
            width: 127px;
        }
        .style44
        {
            width: 199px;
        }
        .style46
        {
            width: 114px;
        }
        .style9
        {
            width: 124px;
        }
        .style15
        {
            width: 127px;
        }
        .style63
        {
            width:33%;
            height : 34px;
        }
        .style64
        {
            width: 33%;
            height : 34px;
        }
        .style59
        {
            width: 112px;
        }
        #TextArea1
        {
            width: 169px;
            height: 71px;
        }
        #TextArea2
        {
            width: 169px;
            height: 71px;
        }
        .hidden { display:none;}
        </style>
        <script language = "javascript">
            Date.prototype.Format = function(fmt) {
                //author: meizz 
                var o =
    {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
                if (/(y+)/.test(fmt))
                    fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
                for (var k in o)
                    if (new RegExp("(" + k + ")").test(fmt))
                    fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
                return fmt;
            }


            Date.prototype.addDays = function(d) {
                this.setDate(this.getDate() + d);
            };


            Date.prototype.addWeeks = function(w) {
                this.addDays(w * 7);
            };


            Date.prototype.addMonths = function(m) {
                var d = this.getDate();
                this.setMonth(this.getMonth() + m);

                if (this.getDate() < d)
                    this.setDate(0);
            };


            Date.prototype.addYears = function(y) {
                var m = this.getMonth();
                this.setFullYear(this.getFullYear() + y);

                if (m < this.getMonth()) {
                    this.setDate(0);
                }
            };
            var today = new Date();
            function getdate() {
                var num = today.getDay() - 1;
                var nextday = today;
                if (num < 0)
                    num = 0;
                nextday.addDays(-num);
                return nextday.Format("yyyy-MM-dd");
                
            }
            
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                BorderColor="#CCCCCC" BorderStyle= "None" BorderWidth="1px" CellPadding="4" 
                ForeColor="Black" GridLines= "Both">
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <RowStyle HorizontalAlign=  "Left" Wrap = "true"/>
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
            <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" 
                Text="没有数据,请与管理员联系!" Visible="False"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ContentPlaceHolder2$Button1" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
     
<hr />
    <br />
    <table style="width:100%;">
        <tr>
            <td class="style63" align="right">
                &nbsp;</td>
            <td align="left">
                <asp:Label ID="Label7" runat="server" Text="日期(Date):"></asp:Label>
                </td>
            <td class = "stylefree" align="right">
                &nbsp;</td>
            <td class="style59" align="left">
                <asp:Textbox runat = "server" CssClass ="Wdate" type="text" id = "TextBox2"
                onClick="var date = getdate(); WdatePicker({isShowClear:false,readOnly:true,firstDayOfWeek:1,minDate:date,maxDate:'%y-%M-%d'})" /></td>
            <td class = "style64">
                </td>
        </tr>
        <tr>
            <td class="style63" align="right">
                &nbsp;</td>
            <td  align="left">
                <asp:Label ID="Label6" runat="server" Text="责任人(Checked by):"></asp:Label>
            </td>
            <td class = "stylefree" align="right">
                &nbsp;</td>
            <td class="style59" align="left">
                        <asp:DropDownList ID="DropDownList5" runat="server">
                        </asp:DropDownList>
            </td>
            <td class = "style64">
            </td>
        </tr>
        <tr>
            <td class="style63" align="right">
                &nbsp;</td>
            <td align="left">
                <asp:Label ID="Label9" runat="server" Text="有问题:"></asp:Label>
            </td>
            <td class = "stylefree">
                &nbsp;</td>
            <td class="style59" align="left">
                <asp:CheckBox ID="CheckBox1" runat="server" Text="生成PDCA" />
            </td>
            <td class = "style64">
                </td>
        </tr>
        <tr>
            <td class="style63">
                &nbsp;</td>
            <td >
                        &nbsp;</td>
            <td class = "stylefree" >
                        &nbsp;</td>
            <td class="style59" align="left">
                &nbsp;</td>
            <td class = "style64">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style63">
                &nbsp;</td>
            <td >
                        <asp:Button ID="Button4" runat="server" onclick="Button4_Click" 
                    Text="完成添加(Submit)" />
            </td>
            <td class = "stylefree">
                </td>
            <td class="style59">
                <asp:Button ID="Button5" runat="server" Text="返回(Back)" onclick="Button5_Click" 
                    style="margin-left: 0px" />
            </td>
            <td class = "style64">
                &nbsp;</td>
        </tr>
        </table>
    </asp:Content>

<asp:Content ID="Content3" runat="server" 
    contentplaceholderid="ContentPlaceHolder2">
    
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" >
        <ContentTemplate>
            <table style="width: 100%;">
                <tr>
                    <td width = "10%">
                        &nbsp;</td>
                    <td class="style43">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="Number" 
                            Text="审查代码&lt;br&gt;(LPA Number):"></asp:Label>
                    </td>
                    <td class="style45" align="left">
                        &nbsp;<asp:TextBox ID="Number" runat="server" Width="80px" 
                            style="margin-left: 0px" MaxLength="5"></asp:TextBox></td>
                    <td class="style8">
                        <asp:Label ID="Label2" runat="server" Text="车间(Shop):" 
                    AssociatedControlID="DropDownList1"></asp:Label>
                    </td>
                    <td class="style18" align="left">
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                            <asp:ListItem Value=" ">请选择</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style45">
                        <asp:Label ID="Label8" runat="server" Text="工段(Group):"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" 
                    
    onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                            <asp:ListItem Value=" ">请选择</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td width = "10%" align="center">
                        &nbsp;</td>
                    <td align="center" class="style44">
                        &nbsp;</td>
                    <td class="style46" align="left">
                        <asp:Button 
                    ID="Button1" runat="server" onclick="Button1_Click" Text="获取审查内容" />
                    </td>
                    <td class="style9">
                        <asp:Label ID="Label4" runat="server" Text="班组(Team):"></asp:Label>
                    </td>
                    <td class="style15" align="left">
                        <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DropDownList3_SelectedIndexChanged">
                            <asp:ListItem Value=" ">请选择</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style45">
                        <asp:Label ID="Label5" runat="server" Text="审查项&lt;br&gt;(LPA Item):" 
                    AssociatedControlID="DropDownList4"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="DropDownList4" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DropDownList4_SelectedIndexChanged" 
                    style="height: 22px">
                            <asp:ListItem Value=" ">请选择</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </asp:Content>


