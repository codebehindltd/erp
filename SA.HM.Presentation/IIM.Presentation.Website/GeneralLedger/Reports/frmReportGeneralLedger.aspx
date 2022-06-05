﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportGeneralLedger.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.Reports.frmReportGeneralLedger" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>General Ledger</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#ContentPlaceHolder1_companyProjectUserControl_hfCompanyAll").val() != "") {
                var company = _.findWhere(CompanyList, { CompanyId: parseInt($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val()) });

                if (company != null) {
                    if (company.IsProfitableOrganization) {
                        $("#DonorContainer").hide();
                    }
                    else { $("#DonorContainer").show(); }
                }
            }

            if ($("#ContentPlaceHolder1_hfAccountsHead").val() != "") {
                $("#txtSearch").val($("#ContentPlaceHolder1_hfAccountsHead").val());
            }

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

            $('#ContentPlaceHolder1_txtEndDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtStartDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $('#' + txtStartDate).keypress(function (e) {
                if (e.keyCode == '13') {
                    e.preventDefault();
                    $('#' + txtEndDate).focus();
                }
            });

            var btnGenarate = '<%=btnGenarate.ClientID%>'
            $('#' + txtEndDate).keypress(function (e) {
                if (e.keyCode == '13') {
                    e.preventDefault();
                    $('#' + btnGenarate).focus();
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
                    $('#DateRangePanel').hide();
                    PopulateFiscalYear();
                }
                else if (searchType == 2) {
                    $('#DateRangePanel').show();
                    $('#FiscalYearPanel').hide();
                }
            });

            $("#txtSearch").autocomplete({
                source: function (request, response) {
                    var groupRIndividualSearch = $("#ContentPlaceHolder1_ddlCoaType").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: 'frmReportGeneralLedger.aspx/GetAutoCompleteData',
                        data: "{'searchText':'" + request.term + "','groupRIndividualSearch':'" + groupRIndividualSearch + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.NodeHead,
                                    value: m.NodeId,
                                    Lvl: m.Lvl,
                                    IsTransactionalHead: m.IsTransactionalHead
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfAccountsHead").val(ui.item.label);
                    $("#ContentPlaceHolder1_hfAccountsHeadId").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfAccountsHeadLevel").val(ui.item.Lvl);
                    $("#ContentPlaceHolder1_hfIsTransactionalHead").val((ui.item.IsTransactionalHead == null ? "0" : "1"));
                }
            });

        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

        function PopulateFiscalYear() {
            //$('#SearchTypePanel').show();
            $('#<%=ddlFiscalYear.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            if ($('#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject').val() == "0") {
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
            var ddlFiscalYear = '<%=ddlFiscalYear.ClientID%>';
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

            var nodeId = $("#ContentPlaceHolder1_hfAccountsHeadId").val();
            if (nodeId == "") {
                toastr.warning('Please Select Account Head.');
                return false;
            }

            $("#ContentPlaceHolder1_hfCompanyId").val(company);
            $("#ContentPlaceHolder1_hfProjectId").val(project);
            $("#ContentPlaceHolder1_hfFiscalYear").val(fiscalYear);
            $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
            $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());
        }
    </script>
    <asp:HiddenField ID="hfAccountsHead" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfAccountsHeadId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfAccountsHeadLevel" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="SingleprojectId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsTransactionalHead" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyIsProfitable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyAll" runat="server" />
    <asp:HiddenField ID="hfWithOrWithoutOpening" runat="server" Value="" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfFiscalYear" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfProjectName" runat="server" Value=""></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Details Ledger" Value="DetailsReport"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="CommonDropDownHiddenFieldForPleaseSelect" runat="server"></asp:HiddenField>
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
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Account Head"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <input type="text" id="txtSearch" class="form-control" tabindex="6" />
                    </div>
                </div>
                <div class="form-group">
                    <div>
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
                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" TabIndex="7"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" TabIndex="8"></asp:TextBox><input
                            type="hidden" id="hidToDate" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label5" runat="server" class="control-label" Text="With/Without Opening"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="dllWithOrWithoutOpening" CssClass="form-control" runat="server">
                            <asp:ListItem Value="WithOpening">With Opening Balance</asp:ListItem>
                            <asp:ListItem Value="WithoutOpening">Without Opening Balance</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Report Currency"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:HiddenField ID="hflocalCurrencyId" runat="server" Value=""></asp:HiddenField>
                        <asp:DropDownList ID="ddlCurrencyId" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" id="divGenerate">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            TabIndex="10" OnClick="btnGenarate_Click" OnClientClick="javascript:return ValidationProcess();" />
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
            Report:: General Ledger
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

        var x = '<%=_GeneralLedgerInfo%>';
        if (x > -1)
            EntryPanelVisibleFalse();

        var single = '<%=isSingle%>';
        if (single == "True") {
            $('#CompanyProjectPanel').hide();
        }
        else {
            $('#CompanyProjectPanel').show();
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
    </script>
</asp:Content>
