<%@ Page Title="" Language="C#" MasterPageFile="~/Common/ReportViewer.Master" AutoEventWireup="true" 
CodeBehind="frmReportMemberPayment.aspx.cs" Inherits="HotelManagement.Presentation.Website.Membership.Reports.frmReportMemberPayment" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Member Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Member Payment</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });
    </script>
    <div>
        <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="720px">
            <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
            </rsweb:ReportViewer>
        </asp:Panel>
    </div>
</asp:Content>