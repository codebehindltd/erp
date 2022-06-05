<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ReportLCOverHeadExpense.aspx.cs" Inherits="HotelManagement.Presentation.Website.LCManagement.Reports.ReportLCOverHeadExpense" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
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
            $("#ContentPlaceHolder1_ddlOverHeadId").select2();
            $("#ContentPlaceHolder1_ddlLCId").select2();
        });
    </script>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" autocomplete="off" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" autocomplete="off" CssClass="form-control" runat="server"></asp:TextBox>
                        <input type="hidden" id="hidToDate" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 ">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Expense Head"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlOverHeadId" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 ">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="LC Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLCId" runat="server" CssClass="form-control" AutoPostBack="false">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 ">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="Payment">Payment</asp:ListItem>
                            <asp:ListItem Value="Receive">Receive</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 ">
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="Date Wise">Date Wise</asp:ListItem>
                            <asp:ListItem Value="OverHead Wise">Overhead Wise</asp:ListItem>
                            <asp:ListItem Value="LC Wise">LC Wise</asp:ListItem>
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
    <div id="ReportPanel" style="overflow-x: auto; display: none;" class=" panel panel-default">
        <div class="panel-heading">
            Report:: LC Overhead Expense Information
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
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
    </script>
</asp:Content>
