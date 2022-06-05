<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportJournalRegister.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.Reports.frmReportJournalRegister" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Transaction List</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

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
            $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany').change(function () {
                debugger;
                var company = _.findWhere(CompanyList, { CompanyId: parseInt($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val()) });

                if (company != null) {
                    if (company.IsProfitableOrganization) {
                        $("#DonorContainer").hide();
                    }
                    else { $("#DonorContainer").show(); }
                }

            });

            $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject').change(function () {
                PopulateFiscalYear();
            });
            $("#ContentPlaceHolder1_txtStartDate").blur(function () {
                var date = $("#ContentPlaceHolder1_txtStartDate").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#ContentPlaceHolder1_txtStartDate").focus();
                        $("#ContentPlaceHolder1_txtStartDate").val(DayOpenDate);
                        return false;
                    }
                }

            });
            $("#ContentPlaceHolder1_txtEndDate").blur(function () {
                var date = $("#ContentPlaceHolder1_txtEndDate").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#ContentPlaceHolder1_txtEndDateTo").focus();
                        $("#ContentPlaceHolder1_txtEndDateTo").val(DayOpenDate);
                        return false;
                    }
                }

            });
        });

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        function PopulateFiscalYear() {
            $('#<%=ddlFiscalYear.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            if ($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val() == "0") {
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
        }

        function ValidationProcess() {
            var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            var searchType = $("#<%=ddlSearchType.ClientID %>").val();
            var fiscalYear = $("#<%=ddlFiscalYear.ClientID %>").val();

            if (searchType == "1") {
                if (fiscalYear == "0") {
                    toastr.warning('Please Select Fiscal Year.');
                    $("#<%=ddlFiscalYear.ClientID %>").focus();
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
        $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
        $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());
    }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="SingleprojectId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfFiscalYear" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfProjectName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenFieldForPleaseSelect" runat="server"></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div>
                    <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                    <div class="form-group" id="DonorContainer" style="display:none;">
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
                    <div id="SearchTypePanel">
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
                    <div id="FiscalYearPanel" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="lblFiscalYear" runat="server" class="control-label required-field"
                                Text="Fiscal Year"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control" TabIndex="2">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group" id="DateRangePanel" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server"></asp:TextBox>
                        <input type="hidden" id="hidToDate" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblGLStatus" runat="server" class="control-label required-field" Text="Voucher Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGLStatus" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="1">Approved</asp:ListItem>
                            <asp:ListItem Value="0">Pending</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Report Currency"></asp:Label>
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
            Report:: Transaction List
        </div>
        <div class="panel-body">
            <asp:Panel ID="pnlReporContainer" runat="server" ScrollBars="Both" Height="700px">
                <div id="NoConfigurableBalanceSheet">
                    <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                        PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Height="820px">
                    </rsweb:ReportViewer>
                </div>
            </asp:Panel>
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

        var reportSearchTypeVal = '<%=reportSearchType%>';
        var fiscalIdVal = '<%=hfFiscalId%>';
        if (reportSearchTypeVal == "0") {
            $('#FiscalYearPanel').hide();
            $('#DateRangePanel').hide();
        }
        else if (reportSearchTypeVal == "1") {
            $('#FiscalYearPanel').show();
            $.when(PopulateFiscalYear()).then(() => {
                $("#ContentPlaceHolder1_ddlFiscalYear").val(fiscalIdVal);
            });
            $('#DateRangePanel').hide();
        }
        else if (reportSearchTypeVal == "2") {
            $('#FiscalYearPanel').hide();
            $('#DateRangePanel').show();
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
