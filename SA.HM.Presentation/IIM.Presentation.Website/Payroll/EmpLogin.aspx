<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpLogin.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.EmpLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en">
<head>
    <meta charset="utf-8">
    <title runat="server" id="SiteTitle"></title>
    <meta content="IE=edge,chrome=1" http-equiv="X-UA-Compatible">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/lib/bootstrap/css/bootstrap.css">
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/css/theme.css">
    <link rel="stylesheet" href="/StyleSheetOld/lib/font-awesome/css/font-awesome.css">
    <link rel="stylesheet" href="/StyleSheetOld/css/flexslider.css" type="text/css" media="screen">
    <script src="/StyleSheetOld/lib/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/css/HMStyleSheet.css">
    <script type="text/javascript">
        $(document).ready(function () {
            $('input[type!=hidden]:enabled:visible,textarea:enabled:visible').live('keypress', function (e) {
                code = e.keyCode ? e.keyCode : e.which;

                var currentClassName = "";
                var clicked = $(this);

                //if (clicked.attr('class')) {
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
                //.replace("a" , "b")  works only on first occurrence of "a"
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
<body class="">
    <div class="navbar">
        <div class="navbar-inner">
            <ul class="nav pull-right">
            </ul>
            <a class="brand" href="#">
                <img src="../StyleSheet/images/Innboard-Logo_White.png" class=" InnBoardIcon" alt="logo" /></a>
        </div>
    </div>
    <div class="row-fluid">
        <div class="dialog">
            <div class="block">
                <p id="pLogIn" class="block-heading" runat="server">
                    Log In</p>
                <div class="block-body">
                    <form id="form2" runat="server">
                    <asp:Panel ID="pnlLogin" runat="server">
                        <label>
                            Employee Code</label>
                        <asp:TextBox ID="txtEmpCode" runat="server" CssClass="span12" TabIndex="1"></asp:TextBox>
                        <label>
                            Password</label>
                        <asp:TextBox ID="txtEmpPassword" runat="server" TextMode="Password" CssClass="span12"
                            onkeypress="return EnterEvent(event)" TabIndex="2"></asp:TextBox>
                        <asp:Button ID="btnLogin" Text="Log In" CssClass="TransactionalButton btn btn-primary pull-right"
                            runat="server" OnClick="btnLogin_Click" TabIndex="3" />
                        &nbsp; &nbsp; &nbsp;
                    </asp:Panel>
                    <asp:Panel ID="pnlEmpAttendance" runat="server">
                        <div style="width: 243px;">
                            <div class="divSectionLeftLeft" style="width: 160px;">
                                <asp:Label ID="lblAttendanceDate" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="lblEntryTime" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="lblExitTime" runat="server"></asp:Label>
                                <br />
                            </div>
                            <div class="divSectionLeftRight">
                                <div>
                                    <asp:Button ID="btnStart" Text="Start" CssClass="TransactionalButton btn btn-primary pull-left"
                                        runat="server" TabIndex="3" OnClick="btnStart_Click" Width="70px" />
                                </div>
                                &nbsp;
                                <div>
                                    <asp:Button ID="btnEnd" Text="End" CssClass="TransactionalButton btn btn-primary pull-left"
                                        runat="server" TabIndex="3" OnClick="btnEnd_Click" Width="70px" />
                                </div>
                                &nbsp;
                                <div>
                                    <asp:Button ID="btnOK" Text="Ok" CssClass="TransactionalButton btn btn-primary pull-left"
                                        runat="server" TabIndex="3" OnClick="btnOK_Click" Width="70px" />
                                </div>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                    </asp:Panel>
                    <div class="clearfix">
                    </div>
                    </form>
                    <div id="MessageBox" style="display: none;">
                        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <%--  <p><a href="#">Forgot your password?</a></p>--%>
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
