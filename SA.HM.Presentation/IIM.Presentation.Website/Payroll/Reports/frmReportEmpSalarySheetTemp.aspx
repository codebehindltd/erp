<%@ Page Title="" Language="C#" MasterPageFile="~/Common/ReportViewer.Master" AutoEventWireup="true"
    CodeBehind="frmReportEmpSalarySheetTemp.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportEmpSalarySheetTemp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagName="EmpSearch" TagPrefix="UserControl" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
    </script>
    <asp:HiddenField ID="hfIsSingle" runat="server" Value="0" />
    <asp:Panel ID="pnlReporContainer" runat="server" ScrollBars="Both" Height="700px">
        <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
            PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
            Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt" Width="830px" Height="820px">
        </rsweb:ReportViewer>
    </asp:Panel>
</asp:Content>
