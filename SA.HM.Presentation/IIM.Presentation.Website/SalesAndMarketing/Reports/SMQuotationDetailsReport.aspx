<%@ Page Title="" Language="C#" MasterPageFile="~/Common/ReportViewer.Master" AutoEventWireup="true" CodeBehind="SMQuotationDetailsReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.Reports.SMQuotationDetailsReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Reservation Bill</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });
    </script>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    
    <iframe id="frmPrint" name="IframeName" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
        clientidmode="static"></iframe>
    <div>
        <div class="row">
            <div class="clear">
            </div>
        </div>
        <div>
            <rsweb:reportviewer showfindcontrols="false" showwaitcontrolcancellink="false" id="rvTransaction"
                pagecountmode="Actual" sizetoreportcontent="true" showprintbutton="true" runat="server"
                font-names="Verdana" font-size="8pt" interactivedeviceinfos="(Collection)" waitmessagefont-names="Verdana"
                waitmessagefont-size="14pt" width="950px" height="820px">
            </rsweb:reportviewer>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransaction.ClientID %>"));

                var innerTbodyPrint = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTablePrint = '<table title="Print" onclick="PrintDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbodyPrint + '</table>'
                var innerTbodyEmail = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Images/emailsend.png" title="Send Mail"></td></tr></tbody>';
                var innerTableEmail = '<table title="Send Mail" onclick="SendEmail(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbodyEmail + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTablePrint + '</td><td height="28px">' + innerTableEmail + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }
        
    </script>
</asp:Content>
