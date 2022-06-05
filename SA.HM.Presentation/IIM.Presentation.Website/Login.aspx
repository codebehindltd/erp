<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HotelManagement.Presentation.Website.UserInformation.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en">
<head>
    <meta charset="utf-8">
    <title runat="server" id="SiteTitle"></title>
    <meta content="IE=edge,chrome=1" http-equiv="X-UA-Compatible">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/lib/bootstrap/css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/css/theme.css" />
    <link rel="stylesheet" href="/StyleSheetOld/lib/font-awesome/css/font-awesome.css" />
    <link rel="stylesheet" href="/StyleSheetOld/css/flexslider.css" type="text/css" media="screen" />
    <script src="/StyleSheetOld/lib/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/css/HMStyleSheet.css" />
    <script src="ScriptsOld/js.cookie.js" type="text/javascript"></script>
    <script type="text/javascript">
        var vc = "";
        $(document).ready(function () {
            if (Cookies.getJSON('menuoption') != undefined) {
                Cookies.remove('menuoption');
            }
            if (Cookies.getJSON('hpanel') != undefined) {
                Cookies.remove('hpanel');
            }
            if (Cookies.get('activeMenuIndex') != undefined) {
                Cookies.remove('activeMenuIndex');
            }

            $('input[type!=hidden]:enabled:visible,textarea:enabled:visible').live('keypress', function (e) {
                code = e.keyCode ? e.keyCode : e.which;

                var currentClassName = "";
                var clicked = $(this);

                if (typeof clicked.attr('class') != 'undefined') {
                    currentClassName = clicked.attr('class').split(' ')[0];
                }
                if (code.toString() == '13') {
                    if (currentClassName != "TransactionalButton") {
                        e.stopPropagation();
                        var x = $('input[type!=hidden]:enabled:visible,select:enabled:visible,textarea:enabled:visible');

                        for (var i = 0; i < x.length; i++) {
                            if ($(x[i]).attr("id") == $(this).attr("id")) {
                                $(x[i + 1]).focus();
                                $(x[i + 1]).select();
                            }
                        }
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            });
        });

        function validateTxt() {
            $("textarea, input[type='text']").change(function () {
                html = $(this).val(); //get the value
                html = html.replace(/< /g, "<"); //before: if there's space after < remove
                html = html.replace(/</g, "< "); // add space after <
                $(this).val(html); //set new value
            });
        }

        $(document).ready(function () {
            validateTxt();
        });

        function EnterEvent(e) {
            if (e.keyCode == 13) {
                __doPostBack('<%=btnLogin.ClientID%>', "");
            }
        }

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
    </script>
</head>
<body class="LogInPageDataGrid" id="LoginBackgroudDiv" runat="server">
    <div class="navbar">
        <div class="navbar-inner">
            <ul class="nav pull-right">
                <a class="brand  pull-right" href="#">
                    <%--<img src="Images/brought-to-you.png" style="" alt="logo" />--%>
                </a>
            </ul>
            <a class="brand" href="#">
                <%--<img src="../StyleSheet/images/Innboard-Logo_White.png" class=" InnBoardIcon" alt="logo" />--%>
            </a>
        </div>
    </div>
    <div class="row-fluid">
        <div class="dialog">
            <div class="block">
                <p class="block-heading">
                    Sign In
                </p>
                <div class="block-body">
                    <form id="form2" runat="server">
                        <label>
                            User Name</label>
                        <asp:TextBox ID="txtUserId" runat="server" CssClass="span12" TabIndex="1"></asp:TextBox>
                        <label>
                            Password</label>
                        <asp:TextBox ID="txtUserPassword" runat="server" TextMode="Password" CssClass="span12"
                            onkeypress="return EnterEvent(event)" TabIndex="2"></asp:TextBox>
                        <label class="remember-me">
                            <input type="checkbox" />
                            Remember me</label>
                        <asp:Button ID="btnLogin" Text="Sign In" CssClass="TransactionalButton btn btn-primary pull-right"
                            runat="server" OnClick="btnLogin_Click" TabIndex="3" />
                        <div class="clearfix">
                        </div>
                    </form>
                    <div>
                        <p>
                            <span style="color: #808B96; font-weight: bold;">Version:</span>&nbsp;10.1.1<span style="color: #1B4F72; font-weight: bold;"></span>
                        </p>
                    </div>
                    <div id="MessageBox" style="display: none;">
                        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="StyleSheetOld/lib/bootstrap/js/bootstrap.js"></script>
    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }
    </script>
</body>
</html>
