<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCostCenterSelection.aspx.cs"
    Inherits="HotelManagement.Presentation.Website.POS.frmCostCenterSelection" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>Hotel Management Admin</title>
    <meta content="IE=edge,chrome=1" http-equiv="X-UA-Compatible">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/lib/bootstrap/css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/lib/bootstrap/css/bootstrap-responsive.css" />
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/css/theme.css" />
    <link href="/StyleSheetOld/css/custom.css" rel="stylesheet" type="text/css" />
    <link href="/StyleSheetOld/css/jquery-ui-1.11.2.css" rel="stylesheet" type="text/css" />
    <link href="../StyleSheetOld/css/ResponsiveTableStyle.css" rel="stylesheet" type="text/css" />
    <link href="../StyleSheetOld/css/toastr.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/css/HMStyleSheet.css" />
    <script src="../ScriptsOld/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="../ScriptsOld/jquery-ui-1.11.2.js" type="text/javascript"></script>
    <link href="/StyleSheetOld/css/toastr.css" rel="stylesheet" type="text/css" />
    <script src="/ScriptsOld/toastr.js" type="text/javascript"></script>
    <style type="text/css">
        .ui-dialog .ui-dialog-content
        {
            padding: 0.5em;
        }
    </style>
    <script type="text/javascript">

        var vc = '';

        $(document).ready(function () {

            $("#btnTokenList").click(function () {

                $("#TokenListDialog").dialog({
                    autoOpen: true,
                    modal: true,
                    minWidth: 536,
                    minHeight: 550,
                    closeOnEscape: false,
                    resizable: false,
                    fluid: true,
                    title: "Token List",
                    show: 'slide'
                });

            });

            $('.RoomOccupaiedDiv').on('click', function (e) {
                var kotId = $(this).next(".RoomNumberDiv").text();
                vc = $(this);

                $("#TokenListDialog").dialog("close");

                PageMethods.LoadHoldupBill(kotId, OnLoadHoldupBillSucceeded, OnHoldupBillFailed);
                return false;

            });

            $("#btnBillReprint").click(function () {
                $("#TouchKeypad").dialog({
                    width: '440',
                    maxWidth: 440,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    height: 'auto',
                    title: "Bill Number",
                    show: 'slide'
                });
            });

        });

        function OnLoadHoldupBillSucceeded(result) {
            if (result.IsSuccess == true) {
                window.location = result.RedirectUrl;
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }

        function OnHoldupBillFailed() { }

        function GoToLogOutPanel() {
            window.location = "/Logout.aspx?LoginType=RestaurentLogin";
            return false;
        }

        function myFunctionTouchKeypad(val) {
            var existingValue = $("#<%=txtBillId.ClientID %>").val();
            if (val != 99) {
                $("#<%=txtBillId.ClientID %>").val(existingValue + val);
            }
            else {
                if (existingValue.length > 0) {
                    var m = existingValue.substring(0, existingValue.length - 1);
                    $("#<%=txtBillId.ClientID %>").val(m);
                }
            }
        }

        function BillReprint() {

            if ($("#txtBillId").val() == "") {
                toastr.warning("Please Provide Bill Id.");
                return;
            }

            $("#hfBillId").val($("#txtBillId").val());

            PrintDocumentFunc("1");
            return false;
        }

        function PrintDocumentFunc(printTemplate) {
            if (printTemplate == "1") {
                $('#btnPrintPreview').trigger('click');
            }
            else if (printTemplate == "2") {
                //$('#btnPrintReportTemplate2').trigger('click');
            }
            else if (printTemplate == "3") {
                //$("#btnPrintReportTemplate3").trigger('click');
            }
            return true;
        }

        function ClosePrintDialog() { }

    </script>
</head>
<body class="">
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfBillId" runat="server"></asp:HiddenField>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <iframe id="IframeReportPrint" name="IframeReportPrint" width="0" height="0" runat="server"
        style="left: -1000; top: 2000;" clientidmode="static" scrolling="yes"></iframe>
    <div id="Div5" style="display: none;">
        <asp:Button ID="btnPrintPreview" runat="server" Text="Print Preview" ClientIDMode="Static"
            OnClick="btnPrintPreview_Click" />
    </div>
    <div id="LoadReport" style="display: none;">
        <div style="height: 555px; overflow-y: scroll;">
            <rsweb:ReportViewer ID="rvTransactionShow" ShowFindControls="false" ShowWaitControlCancelLink="false"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" ShowPageNavigationControls="true" ZoomMode="FullPage"
                Height="2000" ClientIDMode="Static" ShowRefreshButton="false">
            </rsweb:ReportViewer>
        </div>
    </div>
    <div class="container-fluid" style="padding-left: 5px; padding-right: 5px;">
        <div class="row-fluid restaurantPageHeader" style="margin: 0px; padding: 0px;">
            <div class="span2" style="padding: 10px 20px;">
                <img src="../StyleSheet/images/Innboard-Logo_White.png" class=" InnBoardIcon" alt="logo" />
            </div>
            <div class="span6">
            </div>
            <div class="span4">
                <div class="navbar navbar-inner" style="border: 0px;">
                    <ul class="nav pull-right">
                        <li style="color: #fff; vertical-align: middle; padding-top: 9px;">
                            <asp:Label ID="lblLoggedInUser" runat="server" Text=""></asp:Label>
                        </li>
                        <li><a href="/Logout.aspx?LoginType=RestaurentLogin" class="hidden-phone visible-tablet visible-desktop">
                            <i class="icon-book"></i>Logout</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="dialogForBearerLogin">
            <div class="row-fluid block" style="overflow: hidden; margin: 0px; padding: 0px;
                margin-top: 2px; margin-bottom: 5px; border: 0px;">
                <div class="span12" style="margin: 0px; padding: 0px;">
                    <asp:Literal ID="literalCostCenterInformation" runat="server"> </asp:Literal>
                </div>
                <div class="divClear">
                </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="dialogForBearerLogin">
            <div class="row-fluid block" style="overflow: hidden; margin: 0px; padding: 0px;
                margin-top: 2px; margin-bottom: 5px; border: 0px;">
                <a class="block-heading" href="javascript:void()">Token List</a>
                <div class="span12" style="padding: 0px; margin: 0;">
                    <div style="padding: 0px; border: 1px solid #ccc; margin: 5px; min-height: 50px;">
                        <input type="button" id="btnTokenList" value="Token List" style="margin-top: 5px;"
                            class="btn btn-primary btn-large" />
                        <input type="button" id="btnBillReprint" value="Bill Re-Print" style="margin-top: 5px;"
                            class="btn btn-primary btn-large" />
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
        </div>
    </div>
    <div id="TokenListDialog" style="display: none;">
        <div style="height: 550px;">
            <div style="height: 550px; overflow-y: scroll;">
                <asp:Literal ID="litTokenList" runat="server"> </asp:Literal>
            </div>
        </div>
    </div>
    <div id="TouchKeypad" style="display: none;">
        <div id="TouchKeypadResultDiv">
            <asp:TextBox ID="txtBillId" runat="server" CssClass="TouchKeypadResult" Height="40px"
                Font-Size="50px"></asp:TextBox>
        </div>
        <div class='divClear'>
        </div>
        <div class='block-body collapse in'>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img15' onclick='myFunctionTouchKeypad(7)' class="TouchNumericItemImageDiv"
                        src='/Images/7.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img16' onclick='myFunctionTouchKeypad(8)' class="TouchNumericItemImageDiv"
                        src='/Images/8.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img17' onclick='myFunctionTouchKeypad(9)' class="TouchNumericItemImageDiv"
                        src='/Images/9.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img12' onclick='myFunctionTouchKeypad(4)' class="TouchNumericItemImageDiv"
                        src='/Images/4.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img13' onclick='myFunctionTouchKeypad(5)' class="TouchNumericItemImageDiv"
                        src='/Images/5.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img14' onclick='myFunctionTouchKeypad(6)' class="TouchNumericItemImageDiv"
                        src='/Images/6.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img7' onclick='myFunctionTouchKeypad(1)' class="TouchNumericItemImageDiv"
                        src='/Images/1.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img10' onclick='myFunctionTouchKeypad(2)' class="TouchNumericItemImageDiv"
                        src='/Images/2.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img11' onclick='myFunctionTouchKeypad(3)' class="TouchNumericItemImageDiv"
                        src='/Images/3.jpg' />
                </div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img6' onclick='myFunctionTouchKeypad(0)' class="TouchNumericItemImageDiv"
                        src="/Images/0.jpg" />
                </div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img18' onclick='myFunctionTouchKeypad(99)' class="TouchNumericItemImageDiv"
                        src='/Images/Backspace.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img19' onclick='javascript:return BillReprint()' class="TouchNumericItemImageDiv"
                        src='/Images/Ok.jpg' /></div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
