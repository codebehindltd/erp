<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportRoomStatus.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportRoomStatus" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Status</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_txtSearchDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat               
            });
        });

        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtSearchDate').blur(function () {
                var dat = $('#ContentPlaceHolder1_txtSearchDate').val();

                if (dat == "") {

                    $('#ContentPlaceHolder1_lblMessage').text("Search Date must not be empty");
                }
                else {
                    $('#ContentPlaceHolder1_lblMessage').text("");
                }
            });

        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

    </script>
    <div class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">                        
                        <asp:Label ID="lblSearchDate" runat="server" class="control-label" Text="Search Date"></asp:Label>
                    </div>
                    <div class="col-md-4">                        
                        <asp:TextBox ID="txtSearchDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Report:: Room Status Information</div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvRoomStatusInfoByDate"
                    PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    <LocalReport ReportPath="HotelManagement\Reports\Rdlc\rptRoomStatusByDate.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="RoomStatusByDate" Name="RoomStatusByDate" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="RoomStatusByDate" runat="server" SelectMethod="GetData"
                    TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetRoomStatusInfoByDate_SPTableAdapter"
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:FormParameter DefaultValue="01/01/2000" FormField="txtFromDate" Name="SearchDate"
                            Type="DateTime" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
