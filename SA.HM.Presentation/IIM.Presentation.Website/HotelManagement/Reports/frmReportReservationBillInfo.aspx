﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Common/ReportViewer.Master" AutoEventWireup="true"
    CodeBehind="frmReportReservationBillInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportReservationBillInfo" %>

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
    <div style="display: none;">
        <asp:Button ID="btnEmailSend" runat="server" Text="Send Email"
            Style="margin-left: 630px; background-color: #286090; color: White; padding: 5px;"
            TabIndex="10" OnClick="btnEmailSend_Click" ClientIDMode="Static" />
    </div>
    <iframe id="frmPrint" name="IframeName" width="0" height="0" runat="server" style="left: -1000;
        top: 2000;" clientidmode="static"></iframe>
    <div>
        <div class="row">
            <div class="columnRight">
                <asp:TextBox ID="txtReservationNumber" runat="server" Visible="False"></asp:TextBox>
            </div>
            <div class="clear">
            </div>
        </div>
        <div>
            <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
            </rsweb:ReportViewer>
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
        function SendEmail(ss) {
            $('#btnEmailSend').trigger('click');
            return true;
        }        
    </script>
</asp:Content>
