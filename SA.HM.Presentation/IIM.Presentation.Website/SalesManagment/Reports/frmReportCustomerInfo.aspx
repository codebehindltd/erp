<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HMReport.Master" AutoEventWireup="true" CodeBehind="frmReportCustomerInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.Reports.frmReportCustomerInfo" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Customer Info</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="block">
        <div class="row" style="padding-left: 30px">
            <div class="HMBodyContainer">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblCustomer" runat="server" Text="Customer"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlCustomer" CssClass="ThreeColumnDropDownList" runat="server" TabIndex="7">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary"
                        OnClick="btnGenarate_Click" />
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <%--<div class="row">
            <div class="columnRight">
                <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
            </div>
            <div class="clear">
            </div>
        </div>--%>
    </div>
    <div id="ReportPanel" class="block" style="display: none;">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Report:: Customer Information </a>
        <div class="block-body collapse in">
            <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                <%--<LocalReport ReportPath="SalesManagment\Reports\Rdlc\RptCustomerInformation.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="TransactionDataSource" Name="SalesCustomerDS" />
                    </DataSources>
                </LocalReport>--%>
            </rsweb:ReportViewer>
            <%--<asp:ObjectDataSource ID="TransactionDataSource" runat="server" SelectMethod="GetData"
                TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetSalesCustomerInfoForReport_SPTableAdapter"
                OldValuesParameterFormatString="original_{0}">
                <SelectParameters>
                    <asp:FormParameter FormField="ddlCustomer" Name="CustomerId" Type="String" />
                    <asp:FormParameter FormField="txtCompanyName" Name="CompanyName" Type="String" />
                    <asp:FormParameter FormField="txtCompanyAddress" Name="CompanyAddress" Type="String" />
                    <asp:FormParameter FormField="txtCompanyWeb" Name="CompanyWeb" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>--%>
        </div>
    </div>
    <script type="text/javascript">
        var xMessage = '<%=isMessageBoxEnable%>';
        if (xMessage > -1) {
            MessagePanelShow();
        }
        else {
            MessagePanelHide();
        }

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
