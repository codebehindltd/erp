<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportRevenueStatement.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.Reports.frmReportRevenueStatement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var NotesNNodeHead = "";

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Balance Sheet</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#ContentPlaceHolder1_hfNotesNodes").val() != "") {

                var startDate = '', endDate = '', fiscalYearId = '';
                var companyId = 0, projectId = 0, donorId = 0, notesNodes = '';
                var searchType = "", withOrWithoutOpening = "";

                searchType = $("#ContentPlaceHolder1_ddlSearchType").val();
                withOrWithoutOpening = $("#ContentPlaceHolder1_dllWithOrWithoutOpening").val();

                if ($("#ContentPlaceHolder1_ddlSearchType").val() == "FiscalYear") {
                    fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYear").val();
                }
                else if ($("#ContentPlaceHolder1_ddlSearchType").val() == "DateRange") {

                    if ($("#ContentPlaceHolder1_txtStartDate").val() != "") {
                        startDate = $("#ContentPlaceHolder1_txtStartDate").val();
                    }

                    if ($("#ContentPlaceHolder1_txtEndDate").val() != "") {
                        endDate = $("#ContentPlaceHolder1_txtEndDate").val();
                    }
                }

                if ($("#ContentPlaceHolder1_ddlGLCompany").val() != "") {
                    companyId = $("#ContentPlaceHolder1_ddlGLCompany").val();
                }

                if ($("#ContentPlaceHolder1_ddlGLProject").val() != "") {
                    projectId = $("#ContentPlaceHolder1_ddlGLProject").val();
                }

                if ($("#ContentPlaceHolder1_ddlDonor").val() != "") {
                    donorId = $("#ContentPlaceHolder1_ddlDonor").val();
                }

                notesNodes = $("#ContentPlaceHolder1_hfNotesNodes").val();

                var iframeid = 'printDocNotes';
                var url = "/GeneralLedger/Reports/frmReportNotesBreakDownShow.aspx?&sd=" + startDate + "&ed=" + endDate + "&st=" + searchType + "&fy=" + fiscalYearId
                                                                                  + "&cp=" + companyId + "&pj=" + projectId + "&dr=" + donorId + "&wop=" + withOrWithoutOpening
                                                                                  + "&nod=" + notesNodes;

                parent.document.getElementById(iframeid).src = url;
            }

            if ($("#ContentPlaceHolder1_hfCompanyAll").val() != "") {

                ComanyList = JSON.parse($("#ContentPlaceHolder1_hfCompanyAll").val());
                //$("#ContentPlaceHolder1_hfCompanyAll").val("");

                var company = _.findWhere(ComanyList, { CompanyId: parseInt($("#ContentPlaceHolder1_ddlGLCompany").val()) });

                if (company != null) {
                    if (company.IsProfitableOrganization) {
                        $("#DonorContainer").hide();
                    }
                    else { $("#DonorContainer").show(); }
                }
            }

            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            var ddlGLCompany = '<%=ddlGLCompany.ClientID%>'
            var selectedIndex = parseFloat($('#' + ddlGLCompany).prop("selectedIndex"));
            if (selectedIndex > 0) {
                $("#" + ddlGLProject).attr("disabled", false);
            }
            else {
                $("#" + ddlGLProject).attr("disabled", true);
            }

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

        //CompanyProjectPanel Div Visible True/False-------------------
        function CompanyProjectPanelShow() {
            $('#CompanyProjectPanel').show("slow");
        }
        function CompanyProjectPanelHide() {
            $('#CompanyProjectPanel').hide("slow");
        }

        function PopulateProjects() {

            var company = _.findWhere(ComanyList, { CompanyId: parseInt($("#ContentPlaceHolder1_ddlGLCompany").val()) });

            if (company != null) {
                if (company.IsProfitableOrganization) {
                    $("#DonorContainer").hide();
                }
                else { $("#DonorContainer").show(); }
            }

            $("#<%=ddlGLProject.ClientID%>").attr("disabled", "disabled");
            if ($('#<%=ddlGLCompany.ClientID%>').val() == "0") {
                $('#<%=ddlGLProject.ClientID %>').empty().append('<option selected="selected" value="0">Please select</option>');
            }
            else {
                $('#<%=ddlGLProject.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                $.ajax({
                    type: "POST",
                    url: "/GeneralLedger/frmGLProject.aspx/PopulateProjects",
                    data: '{companyId: ' + $('#<%=ddlGLCompany.ClientID%>').val() + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnProjectsPopulated,
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            }
        }

        function OnProjectsPopulated(response) {

            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            $("#" + ddlGLProject).attr("disabled", false);
            PopulateControl(response.d, $("#<%=ddlGLProject.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }

        function PopulateFiscalYear() {
            //$('#SearchTypePanel').show();
            $("#<%=ddlFiscalYear.ClientID%>").attr("disabled", "disabled");
            if ($('#<%=ddlGLProject.ClientID%>').val() == "0") {
                $('#<%=ddlFiscalYear.ClientID %>').empty().append('<option selected="selected" value="0">Please select</option>');
            }
            else {
                $('#<%=ddlFiscalYear.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                $.ajax({
                    type: "POST",
                    url: "/GeneralLedger/frmGLProject.aspx/PopulateFiscalYear",
                    data: '{projectId: ' + $('#<%=ddlGLProject.ClientID%>').val() + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnFiscalYearPopulated,
                    failure: function (response) {
                        toastr.error(response.d);
                    }
                });
            }
        }

        function OnFiscalYearPopulated(response) {

            var ddlFiscalYear = '<%=ddlFiscalYear.ClientID%>'
            $("#" + ddlFiscalYear).attr("disabled", false);

            PopulateControl(response.d, $("#<%=ddlFiscalYear.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }

        function ValidationProcess() {
            var company = $("#<%=ddlGLCompany.ClientID %>").val();
            var project = $("#<%=ddlGLProject.ClientID %>").val();
            var fiscalYear = $("#<%=ddlFiscalYear.ClientID %>").val();

            if (company == "0") {
                toastr.warning('Please Select Company.');
                return false;
            }
            else if (fiscalYear == "0") {
                toastr.warning('Please Select Fiscal Year.');
                return false;
            }
        }

        function OpenNotesDetailReport(nodeId, particulars, notes) {

            var startDate = '', endDate = '', fiscalYearId = '';
            var companyId = 0, projectId = 0, donorId = 0, withOrWithoutOpening = '';
            //ContentPlaceHolder1_hfWithOrWithoutOpening
            withOrWithoutOpening = $("#ContentPlaceHolder1_dllWithOrWithoutOpening").val();

            NotesNNodeHead = "Notes(" + notes + ") details Of Account - " + particulars;

            if ($("#ContentPlaceHolder1_ddlSearchType").val() == "FiscalYear") {
                fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYear").val();
            }
            else if ($("#ContentPlaceHolder1_ddlSearchType").val() == "DateRange") {

                if ($("#ContentPlaceHolder1_txtStartDate").val() != "") {
                    startDate = $("#ContentPlaceHolder1_txtStartDate").val();
                }

                if ($("#ContentPlaceHolder1_txtEndDate").val() != "") {
                    endDate = $("#ContentPlaceHolder1_txtEndDate").val();
                }
            }

            if ($("#ContentPlaceHolder1_ddlGLCompany").val() != "") {
                companyId = $("#ContentPlaceHolder1_ddlGLCompany").val();
            }

            if ($("#ContentPlaceHolder1_ddlGLProject").val() != "") {
                projectId = $("#ContentPlaceHolder1_ddlGLProject").val();
            }

            if ($("#ContentPlaceHolder1_ddlDonor").val() != "") {
                donorId = $("#ContentPlaceHolder1_ddlDonor").val();
            }

            var iframeid = 'printDoc';
            var url = "/GeneralLedger/Reports/frmReportNotesDetails.aspx?nd=" + nodeId + "&sd=" + startDate + "&ed=" + endDate + "&fy=" + fiscalYearId
                                                                              + "&cp=" + companyId + "&pj=" + projectId + "&dr=" + donorId + "&wop=" + withOrWithoutOpening;
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
    <asp:HiddenField ID="hfWithOrWithoutOpening" runat="server" Value="" />

    <div id="DisplayNotesDetails" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="880" height="650" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>

    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div id="CompanyProjectPanel" style="display: none;">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="LiabilitiesAmountHiddenField" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="AssetsAmountHiddenField" runat="server"></asp:HiddenField>
                            <asp:Label ID="lblGLCompany" runat="server" class="control-label required-field"
                                Text="Company"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control" onchange="PopulateProjects();">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblGLProject" runat="server" class="control-label required-field"
                                Text="Project"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlGLProject" CssClass="form-control" runat="server" onchange="PopulateFiscalYear();">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" id="DonorContainer">
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
                        <asp:Label ID="lblFiscalYear" runat="server" class="control-label required-field"
                            Text="Fiscal Year"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFiscalYear" CssClass="form-control" runat="server" TabIndex="2">
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">
                        <asp:Label ID="lblProcessDate" runat="server" class="control-label required-field" Text="Process Month"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportMonth" CssClass="form-control" runat="server">
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
            Report:: Revenue Statement
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

    <div class="panel panel-default">
        <div class="panel-heading">
            Report:: Revenue Statement Details
        </div>
        <div class="panel-body">
            <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransactionDetails"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Height="820px">
            </rsweb:ReportViewer>
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

        var xIsCompanyProjectPanelEnable = '<%=isCompanyProjectPanelEnable%>';
        if (xIsCompanyProjectPanelEnable > -1) {
            CompanyProjectPanelShow();
        }
        else {
            CompanyProjectPanelHide();
        }

        var single = '<%=isSingle%>';
        if (single == "True") {
            $('#CompanyProjectPanel').hide();
            //$('#SearchTypePanel').show();
        }
        else {
            $('#CompanyProjectPanel').show();
            //$('#SearchTypePanel').hide();
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
