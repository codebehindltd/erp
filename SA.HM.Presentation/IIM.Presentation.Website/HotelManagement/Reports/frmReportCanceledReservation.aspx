<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmReportCanceledReservation.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportCanceledReservation" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Reservation</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtFromDate").datepicker("option", "maxDate", selectedDate);
                }

            });

        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
    </script>
    <div id="SearchPanel" class="panel panel-default">        
        <div class="panel-heading">Search Information</div>
        <div class="panel-body">
            <div class="form-horizontal">                
                    <div class="left-float">
                        <div class="l-left">
                            <%-- Left Left--%>
                            <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                            <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                            <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
                             <asp:TextBox ID="txtReservationMode" runat="server" Visible="False"></asp:TextBox>
                        </div>
                        <div class="r-right">
                            <%--Right Right--%>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox><input type="hidden" id="hidFromDate" />
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox><input type="hidden" id="hidToDate" />
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
            <div class="panel-heading">Report:: Canceled Reservation
            Information</div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="600px">
                    <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                        PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                        <LocalReport ReportPath="HotelManagement\Reports\Rdlc\rptCanceledReservation.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="RoomReservationDataSource" Name="RoomReservationInfo" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="RoomReservationDataSource" runat="server" SelectMethod="GetData"
                        TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetCanceledReservationByDateRange_SPTableAdapter"
                        OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:FormParameter DefaultValue="01/01/2000" FormField="txtFromDate" Name="DateFrom"
                                Type="DateTime" />
                            <asp:FormParameter DefaultValue="01/01/2050" FormField="txtToDate" Name="DateTo"
                                Type="DateTime" />
                            <asp:FormParameter DefaultValue="Cancel" FormField="txtReservationMode" Name="ReservationMode" Type="String" />

                        </SelectParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
