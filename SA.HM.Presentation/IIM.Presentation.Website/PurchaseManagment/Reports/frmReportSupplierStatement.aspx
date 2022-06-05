<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportSupplierStatement.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.Reports.frmReportSupplierStatement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Supplier Statement</li>";
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
            $('#' + txtStartDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker({
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
            var supplier = $("#<%=ddlSupplier.ClientID %>").val();

            if (company == "0") {
                toastr.warning('Please Select Company.');
                return false;
            }
            if (project == "0") {
                toastr.warning('Please Select Project.');
                return false;
            }
            if (supplier == "0") {
                toastr.warning('Please Select Supplier Name.');
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
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="SingleprojectId" runat="server"></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group" id="CompanyProjectPanel" style="display: none;">
                    <div class="col-md-2">
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblGLCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control" onchange="PopulateProjects();">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblGLProject" runat="server" class="control-label" Text="Project"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGLProject" runat="server" CssClass="form-control" onchange="PopulateFiscalYear();">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:HiddenField ID="txtEditNodeId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtCurrentUser" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblNodeId" runat="server" class="control-label" Text="Account Head"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlNodeId" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSrcNodeHead" runat="server" class="control-label required-field" Text="Supplier Name"></asp:Label>                        
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSupplier" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div id="SearchTypePanel">
                        <div class="col-md-2">
                            <asp:Label ID="lblSearchType" runat="server" class="control-label required-field" Text="Search Type"></asp:Label>                           
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control"
                                TabIndex="2">
                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem Value="1">Fiscal Year</asp:ListItem>
                                <asp:ListItem Value="2">Date Range</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="FiscalYearPanel" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="lblFiscalYear" runat="server" class="control-label required-field" Text="Fiscal Year"></asp:Label>                            
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control"
                                TabIndex="2">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group" id="DateRangePanel" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" TabIndex="2"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox><input
                            type="hidden" id="hidToDate" />
                    </div>
                </div>
                <div class="row" id="divGenarate">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" TabIndex="4" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClick="btnGenarate_Click" OnClientClick="javascript:return ValidationProcess();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Report:: Cash Book Statement Information</div>
        <div class="panel-body">
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
