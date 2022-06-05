<%@ Page Title="" Language="C#" MasterPageFile="~/Common/ReportViewer.Master" AutoEventWireup="true"
    CodeBehind="frmReporReservationBillPayment.aspx.cs" Inherits="HotelManagement.Presentation.Website.Banquet.Reports.frmReporReservationBillPayment" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Banquet</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Guest Transaction Info</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });
    </script>
    <div>
        <div class="row">
            <div class="columnRight">
                <asp:TextBox ID="txtPaymentIdList" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtGuestBillFromDate" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtGuestBillToDate" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtPrintedBy" runat="server" Visible="False"></asp:TextBox>
            </div>
            <div class="clear">
            </div>
        </div>
        <div>
            <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="720px">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    <%--<LocalReport ReportPath="HotelManagement\Reports\Rdlc\rptGuestTransactionInfo.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="TransactionDataSource" Name="GuestTransactionInfo" />
                        </DataSources>
                    </LocalReport>--%>
                </rsweb:ReportViewer>
                <%--<asp:ObjectDataSource ID="TransactionDataSource" runat="server" SelectMethod="GetData"
                    TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetGuestPaymentInvoiceInfo_SPTableAdapter"
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:FormParameter FormField="txtPaymentIdList" Name="SearchCriteria" Type="String" />
                        <asp:FormParameter FormField="txtCompanyName" Name="CompanyName" Type="String" />
                        <asp:FormParameter FormField="txtCompanyAddress" Name="CompanyAddress" Type="String" />
                        <asp:FormParameter FormField="txtCompanyWeb" Name="CompanyWeb" Type="String" />
                        <asp:FormParameter FormField="txtPrintedBy" Name="PrintedBy" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>--%>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
