<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false" 
    CodeBehind="frmReportProductReceiveInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.Reports.frmReportProductReceiveInfo" %>
<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product Received</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

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

            $("#ContentPlaceHolder1_ddlSupplier").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var category = $("#ContentPlaceHolder1_ddlCategory").val();
            var ddlItem = '<%=ddlProductId.ClientID%>'

            var txtHiddenItemId = '<%=txtHiddenItemId.ClientID%>'
            $('#' + ddlItem).change(function () {

                $('#' + txtHiddenItemId).val($('#' + ddlItem).val());
            });

            if ($('#' + txtHiddenItemId).val() > 0) {
                $('#' + ddlItem).val($('#' + txtHiddenItemId).val());
            }
        });

        function SetValForItem() {
            var itemId = $('#ContentPlaceHolder1_txtHiddenItemId').val();
            if (itemId != "") {
                $("#ContentPlaceHolder1_ddlProductId").val(itemId).trigger('change');
            }
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

        function LoadProductItem(itemType) {
            PageMethods.GetServiceByCriteria(itemType, OnFillServiceSucceeded, OnFillServiceFailed);
            return false;
        }

        function OnFillServiceSucceeded(result) {
            var list = result;
            var ddlItem = '<%=ddlProductId.ClientID%>';
            var control = $('#' + ddlItem);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].ItemId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            else {
                control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }
            return false;
        }

        function OnFillServiceFailed(error) {
            alert(error.get_message());
        }
        function ValidationProcess() {
            var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            $("#ContentPlaceHolder1_hfCompanyId").val(company);
            $("#ContentPlaceHolder1_hfProjectId").val(project);
            $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
            $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());
        }
    </script>
    <asp:HiddenField ID="txtHiddenItemId" runat="server" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfProjectName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label6" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" TabIndex="6">
                            <asp:ListItem Text="Date Wise" Value="DateRange"></asp:ListItem>
                            <asp:ListItem Text="Supplier Wise" Value="Supplier"></asp:ListItem>
                            <asp:ListItem Text="Category Wise" Value="Category"></asp:ListItem>
                            <asp:ListItem Text="Item Wise" Value="Item"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" class="control-label" Text="Display Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDisplayType" runat="server" CssClass="form-control" TabIndex="6">
                            <asp:ListItem Text="Details" Value="Details"></asp:ListItem>
                            <asp:ListItem Text="Summary" Value="Summary"></asp:ListItem>
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
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Category"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"
                            TabIndex="6" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblBundleName" runat="server" class="control-label" Text="Item"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlProductId" runat="server" CssClass="form-control"
                            TabIndex="6">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Supplier"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control"
                            TabIndex="6">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Receive Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtReceiveNumber" runat="server" CssClass="form-control"
                            TabIndex="6">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="PO Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPONumber" runat="server" CssClass="form-control"
                            TabIndex="6">
                        </asp:TextBox>
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
    <div id="ReportPanel" class="panel panel-default">
        <div class="panel-heading">
            Report:: Item Received Information
        </div>
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

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
