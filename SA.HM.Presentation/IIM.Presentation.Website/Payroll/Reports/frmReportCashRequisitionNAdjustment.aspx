<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/Innboard.Master" EnableEventValidation="false" CodeBehind="frmReportCashRequisitionNAdjustment.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportCashRequisitionNAdjustment" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlAssignEmployee").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                // defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
        })

       

        function ValidationProcess() {
            var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            $("#ContentPlaceHolder1_hfCompanyId").val(company);
            $("#ContentPlaceHolder1_hfProjectId").val(project);
            $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
            $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());
        }
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
    </script>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfProjectName" runat="server" Value=""></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Cash Requisition And Adjustment</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-horizontal">
                    <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblAssignEmployee" runat="server" class="control-label" Text="Employee"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAssignEmployee" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblTransactionType" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlTransactionType" CssClass="form-control" runat="server">
                                <asp:ListItem Value="">--- All ---</asp:ListItem>
                                <asp:ListItem Value="Cash Requisition">Cash Requisition</asp:ListItem>
                                <asp:ListItem Value="Bill Voucher">Bill Voucher</asp:ListItem>
                                <asp:ListItem Value="Cash Requisition Adjustment">Cash Requisition Adjustment</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="dvSearchDateTime" class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Date</label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtSearchFromDate" placeholder="From Date" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtSearchToDate" placeholder="To Date" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Amount</label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtSearchFromAmount" placeholder="From Amount" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtSearchToAmount" placeholder="To Amount" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div id="dvSearchStatus" class="form-group">
                        <div id="TransactionNoDiv">
                            <div class="col-md-2">
                                <label class="control-label">Transaction No</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcTransactionNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div id="AdjustmentNoDiv">
                            <div class="col-md-2">
                                <label class="control-label">Adjustment No</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcAdjustmentNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Status</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSearchStatus" CssClass="form-control" runat="server">
                                <asp:ListItem Value="">-- All --</asp:ListItem>
                                <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                <asp:ListItem Value="Partially Checked">Partially Checked</asp:ListItem>
                                <asp:ListItem Value="Checked">Checked</asp:ListItem>
                                <asp:ListItem Value="Partially Approved">Partially Approved</asp:ListItem>
                                <asp:ListItem Value="Approved">Approved</asp:ListItem>
                                <%--<asp:ListItem Value="Adjustment">Adjustment</asp:ListItem>--%>
                                <asp:ListItem Value="Closed">Closed</asp:ListItem>
                                <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="dvRemarks" class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Remarks</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary"
                            OnClick="btnGenerate_Click" OnClientClick="javascript:return ValidationProcess();" />
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
            Cash Requisition And Adjustment</div>
        <div class="panel-body">
            <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="830px" Height="820px">
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
         var x = '<%=_CashRequisitionShow%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>