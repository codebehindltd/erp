<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ItemConsumptionReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.Reports.ItemConsumptionReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>

        $(document).ready(function () {

            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'
            var itemId = $("#ContentPlaceHolder1_hfItemId").val();
            $("#ContentPlaceHolder1_ddlCostCenterfrom").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlLocation").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlEmployee").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCostCenter").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlProductId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

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

            $("#ContentPlaceHolder1_ddlCostCenterfrom").change(function () {
                LoadLocation($(this).val());
            });
            $("#ContentPlaceHolder1_ddlConsumptionType").change(function () {
                if ($(this).val() == "Employee")
                {
                    $("#EmployeeDiv").show();
                    $("#CostCenterDiv").hide();
                }
                else if ($(this).val() == "CostCenter") {
                    $("#CostCenterDiv").show();
                    $("#EmployeeDiv").hide();
                }
                else if($(this).val() == "0")
                {
                    $("#CostCenterDiv").hide();
                    $("#EmployeeDiv").hide();
                }
            });
            $("#ContentPlaceHolder1_ddlCategory").change(function () {
                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenterfrom").val();
                var categoryId = $(this).val();
                LoadProductByCategoryNCostcenterId(costCenterId, categoryId);
            });
            $("#ContentPlaceHolder1_ddlCategory").trigger("change");
            $("#ContentPlaceHolder1_ddlCostCenterfrom").trigger("change");
            $("#ContentPlaceHolder1_ddlConsumptionType").trigger("change");

            $("#ContentPlaceHolder1_ddlProductId").val(itemId);
            $('#ContentPlaceHolder1_ddlLocation').val($("#ContentPlaceHolder1_hfLocationId").val());
        });
        function LoadLocation(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationSucceeded, OnLoadLocationFailed);
        }
        function OnLoadLocationSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlLocation');

            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    if (list.length > 1)
                        control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].LocationId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            return false;
        }
        function OnLoadLocationFailed() { }

        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

        function LoadProductByCategoryNCostcenterId(costCenterId, categoryId) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './ItemConsumptionReport.aspx/LoadProductByCategoryNCostcenterId',
                data: "{'costCenterId':'" + costCenterId + "','categoryId':'" + categoryId + "'}",
                dataType: "json",
                async: false,
                success: function (data) {

                    SelectedItem = data.d;
                    var list = data.d;
                    var control = $('#ContentPlaceHolder1_ddlProductId');

                    control.empty();
                    if (list != null) {
                        if (list.length > 0) {

                            control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                            for (i = 0; i < list.length; i++) {
                                control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].ItemName + '</option>');
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                        }
                    }

                    //$("#ContentPlaceHolder1_hfItemId").val(control.val());
                },
                error: function (result) {
                }
            });
        }
        function GenarateClick() {
            var item = $("#ContentPlaceHolder1_ddlProductId").val();
            var location = $("#ContentPlaceHolder1_ddlLocation").val();
            $("#ContentPlaceHolder1_hfItemId").val(item);
            $("#ContentPlaceHolder1_hfLocationId").val(location);

            var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            $("#ContentPlaceHolder1_hfCompanyId").val(company);
            $("#ContentPlaceHolder1_hfProjectId").val(project);
            //$("#ContentPlaceHolder1_hfFiscalYear").val(fiscalYear);
            $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
            $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());
        }
    </script>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0"></asp:HiddenField>
        <asp:HiddenField ID="hfCompanyName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfProjectName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfLocationId" runat="server" Value="0"></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" TabIndex="6">
                            <asp:ListItem Value="Date Wise">Date Wise</asp:ListItem>
                            <asp:ListItem Value="Item Wise">Item Wise</asp:ListItem>
                            <asp:ListItem Value="Costcenter Wise">Cost Center Wise</asp:ListItem>
                            <asp:ListItem Value="Category Wise">Category Wise</asp:ListItem>
                            <asp:ListItem Value="Consumption Number Wise">Consumption Number Wise</asp:ListItem>
                            <asp:ListItem Value="Consumption Type Wise">Consumption Type Wise</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server"></asp:TextBox>
                        <input type="hidden" id="hidToDate" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCostCenterfrom" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCostCenterfrom" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblLocation" runat="server" class="control-label" Text="Store Location"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" TabIndex="6"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblConsumptionType" runat="server" class="control-label" Text="Consumption Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlConsumptionType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">--- ALL ---</asp:ListItem>
                            <asp:ListItem Value="Employee">Employee</asp:ListItem>
                            <asp:ListItem Value="CostCenter">Cost Center</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="EmployeeDiv">
                        <div class="col-md-2">
                            <asp:Label ID="lblEmployee" runat="server" class="control-label" Text="Employee"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="CostCenterDiv">
                        <div class="col-md-2">
                            <asp:Label ID="lblCostCenter" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblProductId" runat="server" class="control-label" Text="Item Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlProductId" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                          OnClientClick="javascript: return GenarateClick();"  OnClick="btnGenarate_Click" />
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
    <div id="ReportPanel" style="overflow-x: auto; display: none;" class=" panel panel-default">
        <div class="panel-heading">
            Report:: Item Consumption Information
        </div>
        <div class="panel-body">
            <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
            </rsweb:ReportViewer>
        </div>
    </div>
    <%--<div id="ReportPanel" class="panel panel-default" style="display: none;">
        
    </div>--%>
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
                var c = $("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val();
                var p = $("#ContentPlaceHolder1_hfProjectId").val();

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val($("#ContentPlaceHolder1_hfProjectId").val()).trigger("change");

            }

        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>

</asp:Content>
