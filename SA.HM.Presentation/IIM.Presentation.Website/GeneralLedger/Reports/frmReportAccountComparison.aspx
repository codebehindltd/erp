<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportAccountComparison.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.Reports.frmReportAccountComparison" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var NotesNNodeHead = "";

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Profit and Loss Statement</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#ContentPlaceHolder1_hfNotesNodes").val() != "") {

                var startDate = '', endDate = '', fiscalYearId = '', startDate2 = '', endDate2 = '';
                var companyId = 0, projectId = 0, donorId = 0, notesNodes = '';
                var searchType = "", withOrWithoutOpening = "";

                searchType = $("#ContentPlaceHolder1_ddlSearchType").val();
                withOrWithoutOpening = $("#ContentPlaceHolder1_dllWithOrWithoutOpening").val();

                if ($("#ContentPlaceHolder1_ddlSearchType").val() == "1") {
                    fiscalYearId = $("#ContentPlaceHolder1_hfFiscalYear").val();
                }
                else if ($("#ContentPlaceHolder1_ddlSearchType").val() == "2") {

                    if ($("#ContentPlaceHolder1_txtStartDate").val() != "") {
                        startDate = $("#ContentPlaceHolder1_txtStartDate").val();
                    }

                    if ($("#ContentPlaceHolder1_txtEndDate").val() != "") {
                        endDate = $("#ContentPlaceHolder1_txtEndDate").val();
                    }

                    if ($("#ContentPlaceHolder1_txtStartDate2").val() != "") {
                        startDate2 = $("#ContentPlaceHolder1_txtStartDate2").val();
                    }

                    if ($("#ContentPlaceHolder1_txtEndDate2").val() != "") {
                        endDate2 = $("#ContentPlaceHolder1_txtEndDate2").val();
                    }
                }

                if ($("#ContentPlaceHolder1_hfCompanyId").val() != "") {
                    companyId = $("#ContentPlaceHolder1_hfCompanyId").val();
                }

                if ($("#ContentPlaceHolder1_hfProjectId").val() != "") {
                    projectId = $("#ContentPlaceHolder1_hfProjectId").val();
                }

                if ($("#ContentPlaceHolder1_ddlDonor").val() != "") {
                    donorId = $("#ContentPlaceHolder1_ddlDonor").val();
                }

                notesNodes = $("#ContentPlaceHolder1_hfNotesNodes").val();

                var iframeid = 'printDocNotes';
                var url = "/GeneralLedger/Reports/frmReportNotesBreakDownShowComparison.aspx?&sd=" + startDate + "&ed=" + endDate + "&sd2=" + startDate2 + "&ed2=" + endDate2 + "&st=" + searchType + "&fy=" + fiscalYearId
                    + "&cp=" + companyId + "&pj=" + projectId + "&dr=" + donorId + "&nod=" + notesNodes + "&wop=" + withOrWithoutOpening;
                parent.document.getElementById(iframeid).src = url;
            }


            if ($("#ContentPlaceHolder1_companyProjectUserControl_hfCompanyAll").val() != "") {
                var company = _.findWhere(CompanyList, { CompanyId: parseInt($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val()) });

                if (company != null) {
                    if (company.IsProfitableOrganization) {
                        $("#DonorContainer").hide();
                        $("#ContentPlaceHolder1_hfCompanyIsProfitable").val("1");
                    }
                    else {
                        $("#DonorContainer").show();
                        $("#ContentPlaceHolder1_hfCompanyIsProfitable").val("0");
                    }
                }
            }

            $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany').change(function () {

                debugger;
                var company = _.findWhere(CompanyList, { CompanyId: parseInt($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val()) });

                if (company != null) {
                    if (company.IsProfitableOrganization) {
                        $("#DonorContainer").hide();
                        $("#ContentPlaceHolder1_hfCompanyIsProfitable").val("1");
                    }
                    else {
                        $("#DonorContainer").show();
                        $("#ContentPlaceHolder1_hfCompanyIsProfitable").val("0");
                    }
                }

            });

            $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject').change(function () {
                PopulateFiscalYear();
            });

            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'

            $('#ContentPlaceHolder1_txtStartDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtEndDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtEndDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtStartDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtStartDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtEndDate2').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtEndDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtStartDate2').datepicker("option", "maxDate", selectedDate);
                }
            });

            var ddlSearchType = '<%=ddlSearchType.ClientID%>'
            $('#' + ddlSearchType).change(function () {
                var searchType = $('#' + ddlSearchType).val();
                if (searchType == 0) {
                    toastr.warning('Please Select Search Type');
                    $('#FiscalYearPanel').hide();
                    $('#DateRangePanel').hide();
                }
                else if (searchType == 1) {
                    $('#FiscalYearPanel').show();
                    PopulateFiscalYear();
                    $('#DateRangePanel').hide();
                }
                else if (searchType == 2) {
                    $('#DateRangePanel').show();
                    $('#FiscalYearPanel').hide();
                }
            });
        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        function PopulateFiscalYear() {
            $('#<%=ddlFiscalYear.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');

            if ($('ContentPlaceHolder1_companyProjectUserControl_ddlGLProject').val() == "0") {
                $.ajax({
                    type: "POST",
                    url: "/GeneralLedger/frmGLProject.aspx/PopulateAllFiscalYear",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: OnFiscalYearPopulated,
                    failure: function (response) {
                        toastr.error(response.d);
                    }
                });
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/GeneralLedger/frmGLProject.aspx/PopulateFiscalYear",
                    data: '{projectId: ' + $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject').val() + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: OnFiscalYearPopulated,
                    failure: function (response) {
                        toastr.error(response.d);
                    }
                });
            }
        }

        function OnFiscalYearPopulated(response) {
            PopulateControl(response.d, $("#<%=ddlFiscalYear.ClientID %>"), $("#<%=CommonDropDownHiddenFieldForPleaseSelect.ClientID %>").val());
            PopulateControl(response.d, $("#<%=ddlFiscalYear2.ClientID %>"), $("#<%=CommonDropDownHiddenFieldForPleaseSelect.ClientID %>").val());
        }

        function ValidationProcess() {
            var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            var reportType = $("#<%=ddlReportType.ClientID %>").val();
            var searchType = $("#<%=ddlSearchType.ClientID %>").val();
            var fiscalYear = $("#<%=ddlFiscalYear.ClientID %>").val();
            var fiscalYear2 = $("#<%=ddlFiscalYear2.ClientID %>").val();

            if (reportType == "0") {
                toastr.warning('Please Select ReportType.');
                $("#<%=ddlReportType.ClientID %>").focus();
                return false;
            }
            if (searchType == "1") {
                if (fiscalYear == "0") {
                    toastr.warning('Please Select Fiscal Year.');
                    $("#<%=ddlFiscalYear.ClientID %>").focus();
                    return false;
                }
                if (fiscalYear2 == "0") {
                    toastr.warning('Please Select Fiscal Year.');
                    $("#<%=ddlFiscalYear2.ClientID %>").focus();
                    return false;
                }
            }
            else if (searchType == "0") {
                toastr.warning('Please Select Search Type.');
                $("#<%=ddlSearchType.ClientID %>").focus();
                return false;
            }

        $("#ContentPlaceHolder1_hfCompanyId").val(company);
        $("#ContentPlaceHolder1_hfProjectId").val(project);
        $("#ContentPlaceHolder1_hfFiscalYear").val(fiscalYear);
        $("#ContentPlaceHolder1_hfFiscalYear2").val(fiscalYear2);
        $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
        $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());
    }

    function OpenNotesDetailReport(nodeId, particulars, notes) {
        var startDate = '', endDate = '', fiscalYearId = '';
        var companyId = 0, projectId = 0, donorId = 0;

        NotesNNodeHead = "Notes(" + notes + ") details Of Account - " + particulars;

        if ($("#ContentPlaceHolder1_ddlSearchType").val() == "1") {
            fiscalYearId = $("#ContentPlaceHolder1_hfFiscalYear").val();
        }
        else if ($("#ContentPlaceHolder1_ddlSearchType").val() == "2") {

            if ($("#ContentPlaceHolder1_txtStartDate").val() != "") {
                startDate = $("#ContentPlaceHolder1_txtStartDate").val();
            }

            if ($("#ContentPlaceHolder1_txtEndDate").val() != "") {
                startDate = $("#ContentPlaceHolder1_txtEndDate").val();
            }
        }

        if ($("#ContentPlaceHolder1_hfCompanyId").val() != "") {
            companyId = $("#ContentPlaceHolder1_hfCompanyId").val();
        }

        if ($("#ContentPlaceHolder1_hfProjectId").val() != "") {
            projectId = $("#ContentPlaceHolder1_hfProjectId").val();
        }

        if ($("#ContentPlaceHolder1_ddlDonor").val() != "") {
            donorId = $("#ContentPlaceHolder1_ddlDonor").val();
        }

        var iframeid = 'printDoc';
        var url = "/GeneralLedger/Reports/frmReportNotesDetails.aspx?nd=" + nodeId + "&sd=" + startDate + "&ed=" + endDate + "&fy=" + fiscalYearId
            + "&cp=" + companyId + "&pj=" + projectId + "&dr=" + donorId;
        parent.document.getElementById(iframeid).src = url;

        $("#DisplayNotesDetails").dialog({
            autoOpen: true,
            modal: true,
            minWidth: 880,
            minHeight: 555,
            width: 'auto',
            closeOnEscape: false,
            resizable: false,
            height: 'auto',
            fluid: true,
            title: NotesNNodeHead,
            show: 'slide',
            close: ClosePrintDialog
        });

        return false;
    }

    function ClosePrintDialog() {
        //toastr.info("Ok ok ok");
    }

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="SingleprojectId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyIsProfitable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyAll" runat="server" />
    <asp:HiddenField ID="hfNotesNodes" runat="server" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfFiscalYear" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfFiscalYear2" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfProjectName" runat="server" Value=""></asp:HiddenField>
    <div id="DisplayNotesDetails" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="880" height="650" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="CommonDropDownHiddenFieldForPleaseSelect" runat="server"></asp:HiddenField>
                <div>
                    <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                    <div class="form-group" id="DonorContainer" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="Label2" runat="server" class="control-label" Text="Donor"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlDonor" runat="server" CssClass="form-control" TabIndex="3">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label required-field"
                            Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="0">--- Report Type ---</asp:ListItem>
                            <asp:ListItem Value="CashFlow">Cash Flow</asp:ListItem>
                            <asp:ListItem Value="ProfitLoss">Profit Loss Statement</asp:ListItem>
                            <asp:ListItem Value="BalanceSheet">Balance Sheet</asp:ListItem>
                            <asp:ListItem Value="ReceiptNPaymentStatement">Receipt & Payment Statement</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblSearchType" runat="server" class="control-label required-field"
                            Text="Search Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="1">Fiscal Year</asp:ListItem>
                            <asp:ListItem Value="2">Date Range</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="FiscalYearPanel" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblFiscalYear" runat="server" class="control-label required-field"
                            Text="Fiscal Year 1"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label required-field"
                            Text="Fiscal Year 2"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFiscalYear2" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="DateRangePanel" style="display: none;">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="Date 1"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server"></asp:TextBox><input
                                type="hidden" id="hidFromDate" />
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server"></asp:TextBox><input
                                type="hidden" id="hidToDate" />
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="Label5" runat="server" class="control-label" Text="Date 2"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtStartDate2" CssClass="form-control" runat="server"></asp:TextBox><input
                                type="hidden" id="hidFromDate2" />
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtEndDate2" CssClass="form-control" runat="server"></asp:TextBox><input
                                type="hidden" id="hidToDate2" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" class="control-label" Text="With/ Without Opening"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="dllWithOrWithoutOpening" CssClass="form-control" runat="server">
                            <asp:ListItem Value="WithOpening">With Opening Balance</asp:ListItem>
                            <asp:ListItem Value="WithoutOpening">Without Opening Balance</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Income Tax Percentage"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtIncomeTaxPercentage" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label8" runat="server" class="control-label" Text="Report Currency"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:HiddenField ID="hflocalCurrencyId" runat="server" Value=""></asp:HiddenField>
                        <asp:DropDownList ID="ddlCurrencyId" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClick="btnGenarate_Click" OnClientClick="javascript:return ValidationProcess();" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="columnRight">
                <asp:TextBox ID="txtUrl" runat="server" Visible="False"></asp:TextBox>
            </div>
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
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Report:: Profit and Loss Statement
        </div>
        <div class="panel-body">
            <div class="ReporContainerDiv" style="overflow: scroll;">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px">
                </rsweb:ReportViewer>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Report::Notes Break Down Information
            </div>
            <div class="panel-body">
                <iframe id="printDocNotes" name="printDoc" width="100%" height="650" frameborder="0"
                    style="overflow: hidden; background-color: White;"></iframe>
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

            if ($("#ContentPlaceHolder1_companyProjectUserControl_hfIsSingle").val() != "1") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val($("#ContentPlaceHolder1_hfProjectId").val()).trigger("change");

                var searchType = $("#<%=ddlSearchType.ClientID %>").val();

                if (searchType == "1") {
                    $("#<%=ddlFiscalYear.ClientID %>").val($("#ContentPlaceHolder1_hfFiscalYear").val()).trigger("change");
                    $("#<%=ddlFiscalYear2.ClientID %>").val($("#ContentPlaceHolder1_hfFiscalYear2").val()).trigger("change");
                }
            }
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }

        var xMessage = '<%=isMessageBoxEnable%>';
        if (xMessage > -1) {
            MessagePanelShow();
        }
        else {
            MessagePanelHide();
        }

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1) {
            EntryPanelVisibleFalse();
        }

        var reportSearchTypeVal = '<%=reportSearchType%>';
        if (reportSearchTypeVal == "0") {
            $('#FiscalYearPanel').hide();
            $('#DateRangePanel').hide();
        }
        else if (reportSearchTypeVal == "1") {
            $('#FiscalYearPanel').show();
            var fiscalIdVal1 = '<%=hfFiscalId1%>';
            var fiscalIdVal2 = '<%=hfFiscalId2%>';
            $.when(PopulateFiscalYear()).then(() => {
                $("#ContentPlaceHolder1_ddlFiscalYear").val(fiscalIdVal1);
                $("#ContentPlaceHolder1_ddlFiscalYear2").val(fiscalIdVal2);
            });
            $('#DateRangePanel').hide();
        }
        else if (reportSearchTypeVal == "2") {
            $('#FiscalYearPanel').hide();
            $('#DateRangePanel').show();
        }
    </script>
</asp:Content>
