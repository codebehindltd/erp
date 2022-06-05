<%@ Page Title="" Language="C#" MasterPageFile="~/Common/ReportViewer.Master" AutoEventWireup="true"
    CodeBehind="frmReportPMSalesInvoice.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.Reports.frmReportPMSalesInvoice" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Sales Invoice</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

        });

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div class="row">
        <div class="columnRight">
            <asp:TextBox ID="txtSalesId" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtSalesType" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtInvoiceId" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtEndDate" runat="server" Visible="False"></asp:TextBox>
        </div>
        <div class="clear">
        </div>
    </div>
    <div id="Div" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Report:: Sales Invoice
            Information </a>
        <div class="block-body collapse in">
            <div class="ReporContainerDiv">
                <div id="SalesInvoiceTemplateOne" runat="server">
                    <rsweb:ReportViewer ShowFindControls="False" ID="rvTransaction" PageCountMode="Actual"
                        SizeToReportContent="true" ShowPrintButton="true" runat="server" Font-Names="Verdana"
                        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="950px" Height="820px" DocumentMapWidth="100%"
                        ViewStateMode="Enabled" ZoomMode="FullPage">
                        <LocalReport ReportPath="SalesManagment\Reports\Rdlc\PMSalesInvoice.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="TransactionDataSource" Name="SalesInvoiceDS" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="TransactionDataSource" runat="server" SelectMethod="GetData"
                        TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetPMSalesInvoiceInfoForReport_SPTableAdapter"
                        OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:FormParameter FormField="txtSalesId" Name="SalesId" Type="Int32" />
                            <asp:FormParameter FormField="txtSalesType" Name="SalesType" Type="String" />
                            <asp:FormParameter FormField="txtInvoiceId" Name="InvoiceId" Type="String" />
                            <asp:FormParameter FormField="txtEndDate" Name="ToBillExpireDate" Type="DateTime" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div id="SalesInvoiceTemplateTwo" runat="server">
                    <rsweb:ReportViewer ShowFindControls="False" ID="rvTransactionTwo" PageCountMode="Actual"
                        SizeToReportContent="true" ShowPrintButton="true" runat="server" Font-Names="Verdana"
                        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="950px" Height="820px" DocumentMapWidth="100%"
                        ViewStateMode="Enabled" ZoomMode="FullPage">
                        <LocalReport ReportPath="SalesManagment\Reports\Rdlc\FNSalesInvoice.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="TransactionDataSourceTwo" Name="SalesInvoiceDS" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="TransactionDataSourceTwo" runat="server" SelectMethod="GetData"
                        TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetPMSalesInvoiceInfoForReport_SPTableAdapter"
                        OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:FormParameter FormField="txtSalesId" Name="SalesId" Type="Int32" />
                            <asp:FormParameter FormField="txtSalesType" Name="SalesType" Type="String" />
                            <asp:FormParameter FormField="txtInvoiceId" Name="InvoiceId" Type="String" />
                            <asp:FormParameter FormField="txtEndDate" Name="ToBillExpireDate" Type="DateTime" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
