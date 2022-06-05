<%@ Page Title="" Language="C#" MasterPageFile="~/Common/ReportViewer.Master" AutoEventWireup="true"
    CodeBehind="frmSTBillInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.Reports.frmSTBillInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfBillTemplate" runat="server" />
    <div style="display: none;">
        <asp:Button ID="btnPrintReportTemplate1" runat="server" Text="template1" OnClick="btnPrintReportTemplate1_Click"
            ClientIDMode="Static" />
    </div>
    <iframe id="frmPrint" name="IframeName" width="0" height="0" runat="server" style="left: -1000;
        top: 2000;" clientidmode="static" scrolling="yes"></iframe>
    <div>
        <div id="ReportPanel" class="block" style="width: 750px; height: 650px;">
            <div style="overflow-x: scroll; overflow-y: scroll; height:100%;">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" ShowPageNavigationControls="true" ZoomMode="FullPage" Height="2000"
                    ClientIDMode="Static">
                </rsweb:ReportViewer>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransaction.ClientID %>"));
                var printTemplate = $("#<%=hfBillTemplate.ClientID %>").val();

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(' + printTemplate + '); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }
        });

        function PrintDocumentFunc(printTemplate) {
            if (printTemplate == "1") {
                $('#btnPrintReportTemplate1').trigger('click');
            }
            else if (printTemplate == "2") {
                $('#btnPrintReportTemplate2').trigger('click');
            }
            else if (printTemplate == "3") {
                $("#btnPrintReportTemplate3").trigger('click');
            }
            return true;
        }   
    </script>
</asp:Content>
