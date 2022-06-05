<%@ Page Title="" Language="C#" MasterPageFile="~/Common/ReportViewer.Master" AutoEventWireup="true"
    CodeBehind="frmReportKotBillInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.Reports.frmReportKotBillInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
        <asp:Button ID="btn2PrintReportFromClient" runat="server" Text="Button" OnClick="btn2PrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <iframe id="frmPrint" name="IframeName" width="0" height="0" runat="server" style="left: -1000;
        top: 2000;" clientidmode="static"></iframe>
    <iframe id="frmPrint2" name="IframeName" width="0" height="0" runat="server" style="left: -1000;
        top: 2000;" clientidmode="static"></iframe>
    <div>
        <div class="row">
            <div class="columnRight">
                <asp:TextBox ID="txtReservationNumber" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtContactNumber" runat="server" Visible="False"></asp:TextBox>
            </div>
            <div class="clear">
            </div>
        </div>
        <div id="KotBillTemplate1" runat="server">
            <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvKotBill"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                Width="600px" Height="500px" ShowPageNavigationControls="false" ClientIDMode="Static">
            </rsweb:ReportViewer>
        </div>
        <div id="KotBillTemplate2" runat="server">
            <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvKotBill2"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                Width="600px" Height="500px" ShowPageNavigationControls="false" ClientIDMode="Static">
            </rsweb:ReportViewer>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {
                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvKotBill.ClientID %>"));
                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);

            }

            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {
                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvKotBill2.ClientID %>"));
                var innerTbody2 = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable2 = '<table title="Print" onclick="PrintDocumentFunc2(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv2 = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable2 + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv2);

            }
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }
        function PrintDocumentFunc2(ss) {
            $('#btn2PrintReportFromClient').trigger('click');
            return true;
        }

    </script>
</asp:Content>
