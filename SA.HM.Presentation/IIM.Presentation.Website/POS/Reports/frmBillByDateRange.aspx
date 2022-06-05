<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBillByDateRange.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.Reports.frmBillByDateRange" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Restaurant Bill By Date</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'
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
        });
    </script>
    <div id="SearchPanel" class="panel panel-default">
        <div class="row" style="padding-left: 30px">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server"></asp:TextBox><input
                                type="hidden" id="hidFromDate" />
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server"></asp:TextBox><input
                                type="hidden" id="hidToDate" />
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
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Report:: Date Wise Sales Information</div>
        <div class="panel-body">
            <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                <LocalReport ReportPath="POS\Reports\Rdlc\rptRestaurantBillByDateRange.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="TransactionDataSource" Name="RestaurantBillByDateRange" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:ObjectDataSource ID="TransactionDataSource" runat="server" SelectMethod="GetData"
                TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetTotalServiceAndRoomBillByDate_SPTableAdapter"
                OldValuesParameterFormatString="original_{0}">
                <SelectParameters>
                    <asp:FormParameter FormField="txtStartDate" Name="StartDate" Type="DateTime" />
                    <asp:FormParameter FormField="txtEndDate" Name="EndDate" Type="DateTime" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
    <script type="text/javascript">
        var _IsReportPanelEnable = '<%=_IsReportPanelEnable%>';
        if (_IsReportPanelEnable > -1) {
            $('#ReportPanel').show();
        }
        else {
            $('#ReportPanel').hide();
        } 
    </script>
</asp:Content>
