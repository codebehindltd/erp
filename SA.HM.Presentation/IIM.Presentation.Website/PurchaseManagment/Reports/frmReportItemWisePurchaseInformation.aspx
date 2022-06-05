<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportItemWisePurchaseInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.Reports.frmReportItemWisePurchaseInformation" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            var reportType = $("#ContentPlaceHolder1_ddlReportType").val();
            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Item Wise Purchase</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

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
            
            $("#ContentPlaceHolder1_ddlGLCompanyId").select2({
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

            $("#ContentPlaceHolder1_ddlPOrder").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCategoryId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlFromCostCenter").select2({
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
            $("#ContentPlaceHolder1_ddlReportType").change(function () {

                var reportType;
                reportType = $("#ContentPlaceHolder1_ddlReportType").val();
                if (reportType == "Date Wise") {
                    $("#fromCostCenter").show();
                    $("#item").hide();
                    $("#category").hide();
                }
                else if (reportType == "Item Wise") {
                    $("#fromCostCenter").show();
                    $("#item").show();
                    $("#category").show();
                }
                else if (reportType == "Costcenter Wise") {
                    $("#fromCostCenter").show();
                    $("#item").hide();
                    $("#category").hide();
                }
                else if (reportType == "Category Wise") {
                    $("#fromCostCenter").show();
                    $("#item").hide();
                    $("#category").show();
                }
                else if (reportType == "PO Number Wise") {
                    $("#fromCostCenter").show();
                    $("#item").hide();
                    $("#category").hide();
                }
                else if (reportType == "Supplier Wise") {
                    $("#fromCostCenter").hide();
                    $("#item").hide();
                    $("#category").hide();
                }
            });
            $("#ContentPlaceHolder1_ddlReportType").val(reportType).trigger('change');
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

    </script>
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
                        <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" TabIndex="6">
                            <asp:ListItem Value="Date Wise">Date Wise</asp:ListItem>
                            <asp:ListItem Value="Item Wise">Item Wise</asp:ListItem>
                            <asp:ListItem Value="Costcenter Wise">Cost Center Wise</asp:ListItem>
                            <asp:ListItem Value="Category Wise">Category Wise</asp:ListItem>
                            <asp:ListItem Value="PO Number Wise">PO Number Wise</asp:ListItem>
                            <asp:ListItem Value="Supplier Wise">Supplier Wise</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="CompanyDiv" style="display: none;">
                    <div class="col-md-2">
                        <asp:HiddenField ID="hfIsSingleGLCompany" runat="server"></asp:HiddenField>
                        <asp:Label ID="Label17" runat="server" class="control-label" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlGLCompanyId" runat="server" CssClass="form-control"
                            TabIndex="7">
                        </asp:DropDownList>
                    </div>
                </div>
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
                    <div id="fromCostCenter">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromCostCenter" runat="server" class="control-label" Text="From Cost Center"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFromCostCenter" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblSupplier" runat="server" class="control-label" Text="Supplier"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblPOrder" runat="server" class="control-label" Text="PO Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPOrder" CssClass="form-control" runat="server"></asp:TextBox>

                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblPOApprovalStatus" runat="server" class="control-label" Text="Purchase Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPOApprovalStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">--- All ---</asp:ListItem>
                            <asp:ListItem Value="1">Submitted</asp:ListItem>
                            <asp:ListItem Value="2">Approved</asp:ListItem>
                            <asp:ListItem Value="3">Checked</asp:ListItem>
                            <asp:ListItem Value="4">Canceled</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div id="category" style="display: none">
                        <div class="col-md-2">
                            <asp:Label ID="lblCategoryId" runat="server" class="control-label" Text="Category"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCategoryId" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="item" style="display: none">
                        <div class="col-md-2">
                            <asp:Label ID="lblProductId" runat="server" class="control-label" Text="Item Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlProductId" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblPurchaseType" runat="server" class="control-label" Text="Purchase Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPurchaseType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">--- All ---</asp:ListItem>
                            <asp:ListItem Value="AdHoc">Ad Hoc Purchase</asp:ListItem>
                            <asp:ListItem Value="Requisition">Requisition Wise Purchase</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClick="btnGenarate_Click" />
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
            Report:: Purchase Information
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

            if ($("#<%=hfIsSingleGLCompany.ClientID %>").val() == "1") {
                $('#CompanyDiv').hide();
            }
            else {
                $('#CompanyDiv').show();
            }

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
