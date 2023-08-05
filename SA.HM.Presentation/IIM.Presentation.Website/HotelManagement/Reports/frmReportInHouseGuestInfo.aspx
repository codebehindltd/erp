<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportInHouseGuestInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportInHouseGuestInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var minCheckInDate = "";
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Guest House Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_ddlGuestCompany").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            ControlShowHideInfo();
            $("#<%=ddlReportType.ClientID %>").change(function () {
                ControlShowHideInfo();
            });

            $("#<%=ddlGuestCompany.ClientID %>").change(function () {
                var reportName = $("#ContentPlaceHolder1_ddlReportType").val();
                var companyName = $("#ContentPlaceHolder1_ddlGuestCompany").val();
                if ((reportName == "InHouseGustList") && (companyName == "0")) {
                    $("#DivOrderBy").show();
                }
                else {
                    $("#DivOrderBy").hide();
                }
            });

            minCheckInDate = $("#<%=hfMinCheckInDate.ClientID %>").val();           

            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDate").datepicker("option", "minDate", selectedDate);
                },

            });

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtFromDate").datepicker("option", "maxDate", selectedDate);
                }
            });
        });
        function ClearDate() {
        }
        function ShowHide(type) {

        }
        function ControlShowHideInfo()
        {
            debugger;
            if ($("#<%=ddlReportType.ClientID %>").val() == "InHouseGustList") {
                $('#DateRangeContainerDiv').show("slow");
                $('#CompanyInformationDiv').show("slow");
                $('#GroupInformationDiv').hide("slow");
                $('#ReferenceNumberDiv').show("slow");
                ClearDate();
            }
            else if ($("#<%=ddlReportType.ClientID %>").val() == "InHouseGustLedger") {
                $('#CompanyInformationDiv').hide("slow");
                $('#DateRangeContainerDiv').hide("slow");
                $('#ReferenceNumberDiv').hide("slow");
                ClearDate();
            }
            var reportName = $("#ContentPlaceHolder1_ddlReportType").val();
            var companyName = $("#ContentPlaceHolder1_ddlGuestCompany").val();
            if ((companyName == "0") && (reportName == "InHouseGustList")) {
                $("#DivOrderBy").show("slow");
            }
            else {
                $("#DivOrderBy").hide("slow");
            }
        }
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show("slow");
        }

        function SearchValidation() {
            var reportName = $("#ContentPlaceHolder1_ddlReportType").val();
            var companyName = $("#ContentPlaceHolder1_ddlGuestCompany").val();
            if ((reportName == "InHouseGustList") && (companyName == "0")) {
                $("#DivOrderBy").show("slow");
            }
            else {
                $("#DivOrderBy").hide("slow");
            }
        }

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
        <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfMinCheckInDate" runat="server" />
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
                        <asp:Label ID="lblReportType" runat="server" class="control-label required-field" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" TabIndex="4">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="InHouseGustList">In House Guest Report</asp:ListItem>
                            <asp:ListItem Value="InHouseGustLedger">In House Guest Ledger</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="CompanyInformationPanel" runat="server">
                        <div id="CompanyInformationDiv" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblGuestCompany" runat="server" class="control-label" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGuestCompany" runat="server" CssClass="form-control" TabIndex="4">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="GroupInformationDiv" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblGroupName" runat="server" class="control-label" Text="Group Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGroupName" runat="server" CssClass="form-control" TabIndex="4">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group" id="DateRangeContainerDiv" style="display:none;">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblShortName" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="DivOrderBy" style="display: none">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Order By"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlOrderBy" runat="server" CssClass="form-control" TabIndex="4">
                            <asp:ListItem Value="Room">Room</asp:ListItem>
                            <asp:ListItem Value="Company">Company</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="ReferenceNumberDiv" style="display: none">
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" class="control-label" Text="Reservation No."></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtReferenceNumber" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="columnRight">
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none">
        <div class="panel-heading">
            Report:: In House Guest Information
        </div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="700px">
                    <div id="OthersWithoutArrivalInfoDiv" runat="server">
                        <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                            PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                            WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                        </rsweb:ReportViewer>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransaction.ClientID %>"));

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1) {
            EntryPanelVisibleFalse();
        }

        var y = '<%=_IsDateInformationShow%>';
        if (y == 1) {
            //$('#DateInformationDiv').show();
            $('#CompanyInformationDiv').hide();
            $('#GroupInformationDiv').hide();
            //$("#CheckInOutDateContainer").hide();
            //$("#ExpectedDateContainer").hide();
        }
        else if (y == 2) {
            //$('#DateInformationDiv').hide();
            $('#CompanyInformationDiv').show();
            $('#GroupInformationDiv').hide();
            //$("#CheckInOutDateContainer").show();
            //$("#ExpectedDateContainer").hide();
        }
        else if (y == 3) {
            //$('#DateInformationDiv').hide();
            $('#CompanyInformationDiv').hide();
            $('#GroupInformationDiv').hide();
            //$("#CheckInOutDateContainer").show();
            //$("#ExpectedDateContainer").hide();
        }
        else if (y == 4) {
            //$('#DateInformationDiv').hide();
            $('#CompanyInformationDiv').hide();
            $('#GroupInformationDiv').show();
            //$("#CheckInOutDateContainer").hide();
            //$("#ExpectedDateContainer").hide();
        }
        else if (y == 5) {
            //$('#DateInformationDiv').hide();
            $('#CompanyInformationDiv').hide();
            $('#GroupInformationDiv').hide();
            //$("#CheckInOutDateContainer").hide();
            //$("#ExpectedDateContainer").show();
        }
        //else {
        //    //$('#DateInformationDiv').hide();
        //    $('#CompanyInformationDiv').show();
        //    $('#DivOrderBy').show();
        //    $('#GroupInformationDiv').hide();
        //    //$("#CheckInOutDateContainer").hide();
        //    //$("#ExpectedDateContainer").hide();
        //}

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
</asp:Content>
