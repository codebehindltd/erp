<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportRoomOccupencyAnalysis.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportRoomOccupencyAnalysis" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var ddlReportType = '<%=ddlReportType.ClientID%>'; yearDiv
            if ($('#' + ddlReportType).val() == "DateRange") {
                $('#MonthDiv').show();
                $('#yearDiv').hide();
            }
            else {
                $('#MonthDiv').hide();
                $('#yearDiv').show();
            }


            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Occupancy Analysis</li>";
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

            var ddlReportType = '<%=ddlReportType.ClientID%>';
            $('#' + ddlReportType).change(function () {
                if ($('#' + ddlReportType).val() == "DateRange") {
                    $('#MonthDiv').show();
                    $('#yearDiv').hide();
                }
                else {
                    $('#MonthDiv').hide();
                    $('#yearDiv').show();
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
            <div class="form-horizontal" >                
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server">
                                <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                <asp:ListItem Value="DateRange">Date Range</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>                    
                    <div class="form-group" id="yearDiv">
                        <div class="col-md-2">
                            <asp:Label ID="lblYear" runat="server" class="control-label" Text="Year"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>                    
                    <div class="form-group" id="MonthDiv">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>                
                <div class="row" style="display: none">
                    <div class="columnRight">
                        <asp:TextBox ID="txtReportYear" runat="server"></asp:TextBox>
                        <asp:TextBox ID="txtReportFor" runat="server"></asp:TextBox>
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
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000;
            top: 2000;" clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">        
            <div class="panel-heading">Report:: Room Occupancy
            Analysis</div> 
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    <%--<LocalReport ReportPath="HotelManagement\Reports\Rdlc\rptRoomOccupencyAnalysis.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="RoomReservationDataSource" Name="DSRoomOccupencyAnalysis" />
                        </DataSources>
                    </LocalReport>--%>
                </rsweb:ReportViewer>
                <%--<asp:ObjectDataSource ID="RoomReservationDataSource" runat="server" SelectMethod="GetData"
                    TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetInnboardRoomTypeInfoBySearchCriteria_SPTableAdapter"
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:FormParameter FormField="txtReportFor" Name="ReportFor" Type="String" />
                        <asp:FormParameter FormField="txtReportYear" Name="ReportYear" Type="String" />
                        <asp:FormParameter DefaultValue="01/01/2000" FormField="txtFromDate" Name="FromDate"
                            Type="DateTime" />
                        <asp:FormParameter DefaultValue="01/01/2050" FormField="txtToDate" Name="ToDate"
                            Type="DateTime" />
                    </SelectParameters>
                </asp:ObjectDataSource>--%>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransaction.ClientID %>"));

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
