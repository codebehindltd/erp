<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.Login"
    ValidateRequest="false" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en">
<head>
    <title runat="server" id="SiteTitle"></title>

    <link href="/Content/bootstrap.css" type="text/css" rel="stylesheet" />
    <link href="/StyleSheet/lib/font-awesome/css/font-awesome.css" type="text/css" rel="stylesheet" />
    <link href="/StyleSheet/css/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/uploadify.css" rel="stylesheet" type="text/css" />
    <link href="/StyleSheet/css/toastr.css" rel="stylesheet" type="text/css" />
     <link href="/JSLibrary/JqueryTouchKeyBoard/css/keyboard.css" rel="Stylesheet" type="text/css" />
    <link href="/StyleSheet/menucss/menuStyle.css" rel="stylesheet" type="text/css" />
    <link href="/StyleSheet/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="/Content/SiteStyle.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-3.1.0.js" type="text/javascript"></script>
    <script src="/Scripts/bootstrap.min.js"></script>
    <script src="/Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.ui.timepicker.js" type="text/javascript"></script>
    <script src="/JSLibrary/JqueryTouchKeyBoard/js/jquery.keyboard.js" type="text/javascript"></script>
    <script src="/Scripts/underscore.js" type="text/javascript"></script>
    <script src="/Scripts/toastr.js" type="text/javascript"></script>
    <script src="/Scripts/js.cookie.js" type="text/javascript"></script>
    <link href="/Scripts/JqueryAlert/jquery-confirm.min.css" rel="stylesheet" />
    <script src="/Scripts/JqueryAlert/jquery-confirm.min.js"></script>
    <script src="/Scripts/HMCommonScript.js" type="text/javascript"></script>

    <script type="text/javascript">
        var innBoarDateFormat = "";
        $(document).ready(function () {
            $("#txtTouchKeypadResult").val('');

            if ($("#hfGroupList").val() != "") {
                var groupList = new Array();
                groupList = JSON.parse($("#hfGroupList").val());

                var row = 0; totalGroup = groupList.length, count = 1, query = "";

                for (row = 0; row < totalGroup; row++) {

                    if (count == 1) {
                        query += "<div class='btn-group btn-group-justified' role='group'>";
                    }

                    query += "<div class='btn-group text-center' role='group' onclick=\"LoadUserGroupWise(" + groupList[row].UserGroupId + ");\" style='cursor: pointer;'>" +
                                "<div class='thumbnail'>" +
                                "   <div class='caption'>" +
                                groupList[row].GroupName +
                                "   </div>" +
                                "</div>" +
                           "</div>";

                    if (count == 4) {
                        query += "</div>";
                        count = 0;
                    }

                    count++;
                }

                if (query != "") {
                    $("#groupListContainer").html(query);
                }
            }

            //CommonHelper.TouchScreenNumberKeyboardWithoutDotNContainer("numkb");
            //var keyboard = $('.numkb').getkeyboard();
            //keyboard.reveal();

            CommonHelper.TouchScreenNumberKeyboardWithoutDot("numkbnotdecimalpax", "KeyBoardContainerPassword");
            var keyboard = $('.numkbnotdecimalpax').getkeyboard();
            keyboard.reveal();

        });

        function LoadUserGroupWise(groupId) {
            PageMethods.GetUserInformationByUserGroup(groupId, OnSuccessGroupUserLoad, OnFail);
            return false;
        }
        function OnSuccessGroupUserLoad(result) {

            var row = 0; totalGroup = result.length, count = 1, query = "";

            for (row = 0; row < totalGroup; row++) {

                if (count == 1) {
                    query += "<div class='btn-group btn-group-justified' role='group'>";
                }

                query += "<div class='btn-group text-center' role='group' onclick=\"UserInformation('" + result[row].UserId + "');\" style='cursor: pointer;'>" +
                            "<div class='thumbnail'>" +
                            "   <div class='caption'>" +
                            result[row].UserName +
                            "   </div>" +
                            "</div>" +
                       "</div>";

                if (count == 4) {
                    query += "</div>";
                    count = 0;
                }

                count++;
            }

            if (query != "") {
                $("#userListContainer").html(query);
            }
        }

        function UserInformation(userId) {
            $("#lblUserId").text(userId);
        }

        function PerformAuthentication() {
            var txtUserId = $.trim($("#lblUserId").text());
            var txtUserPassword = $.trim($("#txtUserPassword").val());

            if (txtUserId == "" || txtUserPassword == "") {
                toastr.warning("Please Give User Id Or Password.");
                return false;
            }

            PageMethods.Authenticate(txtUserId, txtUserPassword, OnSuccess, OnFail);

            return false;
        }
        function OnSuccess(result) {

            if (result.IsSuccess) {
                //CommonHelper.AlertMessage(result.AlertMessage);
                window.location = result.RedirectUrl;
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnFail(xhr, err) {
            toastr.error(xhr.responseText);
        }

    </script>
</head>
<body>

    <form id="form1" runat="server" class="BusinessERP">
        <asp:HiddenField ID="hfGroupList" runat="server" Value="" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>

        <div class="container-fluid restaurant-wrapper">
            <header>
                <div class="row page-header">
                    <div class="col-xs-12 col-sm-2 col-md-2 site-logo">
                        <a class="brand" href="javascript:void()">
                            <img src="/StyleSheet/images/Innboard-Logo_White.png" class=" InnBoardIcon" alt="logo" /></a>
                    </div>

                    <div class="col-xs-12 col-sm-6 col-md-6">
                    </div>

                    <div class="col-xs-12 col-sm-4 col-md-4">
                    </div>
                </div>
            </header>
        </div>

        <div class="container-fluid" style="height: 83.3vh;">
            <div id="spinner" class="spinner" style="display: none;">
                <img id="img-spinner" src="../Images/spinner.gif" alt="Loading ..." />
            </div>

            <div class="row">
                <div class="col-md-7">
                    <div class="panel panel-default">
                        <div class="panel-heading">User Group</div>
                        <div class="panel-body">
                            <div id="groupListContainer">
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-default">
                        <div class="panel-heading">User Info</div>
                        <div class="panel-body">
                            <div id="userListContainer">
                            </div>
                        </div>
                    </div>

                </div>
                <div class="col-md-5">
                    <div class="form-horizontal">

                        <div class="panel panel-default">
                            <div class="panel-heading text-center">Login</div>
                            <div class="panel-body">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <label id="lblUserId" class="form-control" style="height: 35px; font-size: 20px;"></label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:TextBox ID="txtUserPassword" runat="server" CssClass="numkbnotdecimalpax form-control TransactionalButton"
                                            Height="35px" Font-Size="40px" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row no-gutters">
                                    <div class="col-md-12">
                                        <div id="KeyBoardContainerPassword" style="margin-top: 5px; height: 233px;">
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <input type="button" style="height: 38px; width: 99.6%; font-size: 1.5em; font-weight: bold;"
                                            class="btn btn-success" value="Log In"
                                            onclick="PerformAuthentication()" />
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>

        <div class="container-fluid">
            <footer>
                <div class="row footer">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <hr />
                        <div class="footer-padding contents-padding">
                            <p class="pull-right">
                                User:&nbsp; <span style="color: Blue; font-weight: bold;">
                                    <asp:Label ID="lblLoggedInUser" runat="server" Text=""></asp:Label></span>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                    Developed by <a href="http://datagridbd.com" target="_blank"><span style="color: Blue; font-weight: bold;">data grid limited</span></a>
                            </p>
                            <p>
                                &copy; 2013 <a href="http://www.innboard.com" target="_blank"><span style="color: Blue; font-weight: bold;">innboard.com</span></a>
                            </p>
                        </div>
                    </div>
                </div>
            </footer>
        </div>
    </form>
</body>
</html>
