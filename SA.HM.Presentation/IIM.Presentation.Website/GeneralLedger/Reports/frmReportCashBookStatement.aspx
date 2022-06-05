<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HMReport.Master" AutoEventWireup="true" EnableEventValidation ="false"
    CodeBehind="frmReportCashBookStatement.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.Reports.frmReportCashBookStatement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Cash Book Statement</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);


            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            var ddlGLCompany = '<%=ddlGLCompany.ClientID%>'
            var selectedIndex = parseFloat($('#' + ddlGLCompany).prop("selectedIndex"));
            if (selectedIndex > 0) {
                $("#" + ddlGLProject).attr("disabled", false);
            }
            else {
                $("#" + ddlGLProject).attr("disabled", true);
            }

            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'
            $('#' + txtStartDate).datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });

            // Default Control Enter and Focus----------
            $("#txtSearch").keypress(function (e) {
                if (e.keyCode == '13') {
                    //e.stopPropagation()
                    e.preventDefault();
                    $('#' + txtStartDate).focus();
                }
            });

            $('#' + txtStartDate).keypress(function (e) {
                if (e.keyCode == '13') {
                    //e.stopPropagation()
                    e.preventDefault();
                    $('#' + txtEndDate).focus();
                }
            });

            var btnGenarate = '<%=btnGenarate.ClientID%>'
            $('#' + txtEndDate).keypress(function (e) {
                if (e.keyCode == '13') {
                    //e.stopPropagation()
                    e.preventDefault();
                    $('#' + btnGenarate).focus();
                }
            });


            //--AutoComplete Mode----------  

            //CommonHelper.AutoSearchClientDataSource("txtSearch", "ContentPlaceHolder1_ddlAccountHead", "ContentPlaceHolder1_hfSearch");
            SearchText();
            var ddlNodeId = '<%=ddlNodeId.ClientID%>'

            $("#txtSearch").blur(function () {
                SearchTextForId(document.getElementById('txtSearch').value);
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
                }
                else if (searchType == 2) {
                    $('#DateRangePanel').show();
                    $('#FiscalYearPanel').hide();
                }
            });
        });


        //---------------------------
        function SearchTextForId() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/Reports/frmReportCashBookStatement.aspx/FillForm",
                data: "{'searchText':'" + document.getElementById('txtSearch').value + "'}",
                dataType: "json",
                success: function (data) {
                    var ddlNodeId = '<%=ddlNodeId.ClientID%>'
                    //alert(data.d);
                    if (data.d != 0) {
                        $('#divGenarate').show("slow");
                    }
                    $('#' + ddlNodeId).val(data.d);
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }
        //---------------------------
        function SearchText() {
            $('.ThreeColumnTextBox').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/GeneralLedger/Reports/frmReportCashBookStatement.aspx/GetAutoCompleteData",
                        data: "{searchText:'" + document.getElementById('txtSearch').value + "',searchProjectId:'" + $("#<%=ddlGLProject.ClientID %>").val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                }
            });
        }

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
            $('#SearchTypePanel').show();
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
            var searchType = $("#<%=ddlSearchType.ClientID %>").val();
            var fiscalYear = $("#<%=ddlFiscalYear.ClientID %>").val();

            if (company == "0") {
                toastr.warning('Please Select Company.');
                return false;
            }
            if (project == "0") {
                toastr.warning('Please Select Project.');
                return false;
            }
            if (searchType == "1") {
                if (fiscalYear == "0") {
                    toastr.warning('Please Select Fiscal Year.');
                    return false;
                }
            }
            else if (searchType == "0") {
                toastr.warning('Please Select Search Type.');
                return false;
            }
        }
    </script>
    <asp:HiddenField ID="hfSearch" runat="server" />
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="SingleprojectId" runat="server"></asp:HiddenField>
    <div id="SearchPanel" class="block">
        <div class="row" style="padding-left: 30px">
            <div class="HMBodyContainer">
                <div class="divSection" id="CompanyProjectPanel" style="display: none;">
                    <div class="divBox divSectionLeftLeft">
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblGLCompany" runat="server" Text="Company"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlGLCompany" runat="server" onchange="PopulateProjects();">
                        </asp:DropDownList>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblGLProject" runat="server" Text="Project"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:DropDownList ID="ddlGLProject" runat="server" onchange="PopulateFiscalYear();">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRow" style="display: none;">
                    <div class="divBox divSectionLeftLeft">
                        <asp:HiddenField ID="txtEditNodeId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtCurrentUser" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblNodeId" runat="server" Text="Account Head"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlNodeId" CssClass="ThreeColumnDropDownList" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblSrcNodeHead" runat="server" Text="Account Head"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <input type="text" id="txtSearch" class="ThreeColumnTextBox" tabindex="1" />
                        <%--<div style="display: none;">
                            <asp:DropDownList ID="ddlAccountHead" runat="server" TabIndex="40">
                            </asp:DropDownList>
                        </div>--%>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div id="SearchTypePanel">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSearchType" runat="server" Text="Search Type"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="customSmallDropDownSize"
                                TabIndex="2">
                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem Value="1">Fiscal Year</asp:ListItem>
                                <asp:ListItem Value="2">Date Range</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="FiscalYearPanel" style="display: none;">
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblFiscalYear" runat="server" Text="Fiscal Year"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="customSmallDropDownSize"
                                TabIndex="2">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection" id="DateRangePanel" style="display: none;">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtStartDate" CssClass="datepicker" runat="server" TabIndex="2"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtEndDate" CssClass="datepicker" runat="server" TabIndex="3"></asp:TextBox><input
                            type="hidden" id="hidToDate" />
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton" id="divGenarate">
                    <asp:Button ID="btnGenarate" runat="server" TabIndex="4" Text="Generate" CssClass="btn btn-primary btn-sm"
                        OnClick="btnGenarate_Click" OnClientClick="javascript:return ValidationProcess();" />
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000;
            top: 2000;" clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="block" style="display: none;">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Report:: Cash Book
            Statement Information </a>
        <div class="block-body collapse in">
            <asp:Panel ID="pnlReporContainer" runat="server" ScrollBars="Both" Height="700px">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                </rsweb:ReportViewer>
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

        var xIsCompanyProjectPanelEnable = '<%=isCompanyProjectPanelEnable%>';
        if (xIsCompanyProjectPanelEnable > -1) {
            CompanyProjectPanelShow();
        }
        else {
            CompanyProjectPanelHide();
        }

        var x = '<%=_GeneralLedgerInfo%>';
        if (x > -1)
            EntryPanelVisibleFalse();


        var single = '<%=isSingle%>';
        if (single == "True") {
            $('#CompanyProjectPanel').hide();
            $('#SearchTypePanel').show();
        }
        else {
            $('#CompanyProjectPanel').show();
            $('#SearchTypePanel').hide();
        }

        var reportSearchTypeVal = '<%=reportSearchType%>';
        if (reportSearchTypeVal == "0") {
            $('#FiscalYearPanel').hide();
            $('#DateRangePanel').hide();
        }
        else if (reportSearchTypeVal == "1") {
            $('#SearchTypePanel').show();
            $('#FiscalYearPanel').show();
            $('#DateRangePanel').hide();
        }
        else if (reportSearchTypeVal == "2") {
            $('#SearchTypePanel').show();
            $('#FiscalYearPanel').hide();
            $('#DateRangePanel').show();
        }
    </script>
</asp:Content>
