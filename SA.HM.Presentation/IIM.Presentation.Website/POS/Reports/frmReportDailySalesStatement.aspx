<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportDailySalesStatement.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.Reports.frmReportDailySalesStatement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Daily Sales Statement</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var hfReportTypeSelectedVal = $("#<%=hfReportTypeSelectedVal.ClientID %>").val();
            if (hfReportTypeSelectedVal == "SingleDate") {
                $('#DateRangeDivPanel').hide();
                $('#SingleDateDivPanel').show();
                $('#TransactionTypeDiv').hide();
            }
            else if (hfReportTypeSelectedVal == "DateRange") {
                $('#SingleDateDivPanel').hide();
                $('#DateRangeDivPanel').show();
                $('#TransactionTypeDiv').show();
            }

            var ddlReportType = '<%=ddlReportType.ClientID%>'

            $('#' + ddlReportType).change(function () {
                if ($('#' + ddlReportType).val() == "DateRange") {
                    $('#SingleDateDivPanel').hide();
                    $('#DateRangeDivPanel').show();
                    $('#TransactionTypeDiv').show();                    
                }
                else if ($('#' + ddlReportType).val() == "SingleDate") {
                    $('#DateRangeDivPanel').hide();
                    $('#SingleDateDivPanel').show();
                    $('#TransactionTypeDiv').hide();
                }

                $("#<%=hfReportTypeSelectedVal.ClientID %>").val($(this).val());
            });

            if ($("#ContentPlaceHolder1_hfCostcenterId").val() != "") {

                var costcenter = "", ccId = [], ids = '', idHave = -1;

                ccId = $("#ContentPlaceHolder1_hfCostcenterId").val().split(',');

                $('#TableWiseItemInformation input').each(function () {

                    ids = $(this).attr('value');
                    idHave = _.indexOf(ccId, ids);

                    if (idHave != -1) {
                        $(this).attr('checked', true);

                        if (costcenter != "") {
                            costcenter += ', ' + $(this).attr('name');
                        }
                        else {
                            costcenter += $(this).attr('name');
                        }
                    }
                });

                $("#ContentPlaceHolder1_lblSelectedCostcenter").text(costcenter);
            }

            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'
            var txtFromToDate = '<%=txtFromToDate.ClientID%>'

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtFromToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#btnClear").click(function () {
                $("#ContentPlaceHolder1_hfCostcenterId").val("");
                $("#ContentPlaceHolder1_lblSelectedCostcenter").text("");
                $("#CostCenterInformation").hide();
            });

        });

        function GetCheckedCostcenter() {
            var costCenterId = "", costcenter = "";
            $('#TableWiseItemInformation input:checked').each(function () {
                if (costCenterId != "") {
                    costCenterId += ',' + $(this).attr('value');
                    costcenter += ', ' + $(this).attr('name');
                }
                else {
                    costCenterId += $(this).attr('value');
                    costcenter += $(this).attr('name');
                }
            });
            $("#ContentPlaceHolder1_hfCostcenterId").val(costCenterId);
            $("#ContentPlaceHolder1_lblSelectedCostcenter").text(costcenter);
            $("#CostcenterSelectContainer").dialog("close");
            $("#CostCenterInformation").show();
        }

        function CloseCostcenterDialog() {
            $("#CostcenterSelectContainer").dialog("close");
        }

        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

        function MultiCostCenterInfo() {
            $("#CostcenterSelectContainer").dialog({
                autoOpen: true,
                modal: true,
                width: 450,
                height: 280,
                closeOnEscape: true,
                resizable: false,
                title: "Cost Center Information",
                show: 'slide'
            });
            return;
        }
    </script>
    <div id="CostcenterSelectContainer" style="display: none;">
        <asp:Literal ID="ltCostcenter" runat="server"></asp:Literal>
    </div>
    <asp:HiddenField ID="hfCostcenterId" runat="server" />
    <asp:HiddenField ID="hfReportTypeSelectedVal" runat="server" />
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReportType" runat="server" class="control-label required-field"
                            Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server">
                            <asp:ListItem Value="DateRange">Date Range</asp:ListItem>
                            <asp:ListItem Value="SingleDate">Single Date (MTD)</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="SingleDateDivPanel" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="lbl" runat="server" class="control-label required-field" Text="Report Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-10" style="padding-right: 0;">
                                    <asp:TextBox ID="txtFromToDate" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-padding-left-none">
                                    <span style="margin-left: 3px;">
                                        <img src="../../Images/ListIcon.png" title='Multi Cost Center Info' style="cursor: pointer;"
                                            onclick='javascript:return MultiCostCenterInfo()' alt='Multi Cost Center Info'
                                            border='0' /></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="DateRangeDivPanel">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-10" style="padding-right: 0;">
                                    <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-padding-left-none">
                                    <span style="margin-left: 3px;">
                                        <img src="../../Images/ListIcon.png" title='Multi Cost Center Info' style="cursor: pointer;"
                                            onclick='javascript:return MultiCostCenterInfo()' alt='Multi Cost Center Info'
                                            border='0' /></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Filter By"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFilterBy" CssClass="form-control" runat="server">
                            <asp:ListItem Value="0">--- All ---</asp:ListItem>
                            <asp:ListItem Value="1">Complimentary</asp:ListItem>
                            <asp:ListItem Value="2">Without Complimentary</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Transaction By"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPaymentType" CssClass="form-control" runat="server">
                            <asp:ListItem Value="">--- All ---</asp:ListItem>
                            <asp:ListItem Value="Cash">Cash</asp:ListItem>
                            <asp:ListItem Value="Card">Card</asp:ListItem>
                            <asp:ListItem Value="Company">Company</asp:ListItem>
                            <asp:ListItem Value="Member">Member</asp:ListItem>
                            <asp:ListItem Value="Employee">Employee</asp:ListItem>
                            <asp:ListItem Value="Guest Room">Guest Room</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="TransactionTypeDiv">
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTransactionType" CssClass="form-control" runat="server">
                            <asp:ListItem Value="1">Outlet Wise F&B Gross Sales</asp:ListItem>
                            <asp:ListItem Value="2">Outlet Wise F&B Net Sales</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="CostCenterInformation" class="form-group" style="display: none;">
                    <div class="col-md-2" style="padding-left: 0;">
                        <asp:Label ID="lblCostCenter" runat="server" class="control-label" Text="Cost Center "></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:Label ID="lblSelectedCostcenter" runat="server" class="control-label" Text=""></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" />
                        <input type="button" id="btnClear" class="btn btn-primary btn-sm" value="Clear" />
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
    <div id="ReportPanel" class="panel panel-default">
        <div class="panel-heading">
            Report:: Daily Sales Statement
        </div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="800px">
                    <rsweb:ReportViewer ShowFindControls="true" ShowWaitControlCancelLink="false" ID="rvTransaction"
                        PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    </rsweb:ReportViewer>
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

        var _IsReportPanelEnable = '<%=_IsReportPanelEnable%>';
        if (_IsReportPanelEnable > -1) {
            $('#ReportPanel').show();
        }
        else {
            $('#ReportPanel').hide();
        }
    </script>
</asp:Content>
