<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HMReport.Master" AutoEventWireup="true"
    CodeBehind="frmReportBanquetRevenueInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.Banquet.frmReportBanquetRevenueInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var ddlReportType = '<%=ddlReportType.ClientID%>';
            if ($('#' + ddlReportType).val() == "Monthly") {
                $('#MonthDiv').show();
            }
            else {
                $('#MonthDiv').hide();
            }


            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Banquet Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Banquet Revenue</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);


            $("#ContentPlaceHolder1_txtFromDate").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtToDate").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtFromDate").datepicker("option", "maxDate", selectedDate);
                }

            });

            var ddlReportType = '<%=ddlReportType.ClientID%>';
            $('#' + ddlReportType).change(function () {
                if ($('#' + ddlReportType).val() == "Monthly") {
                    $('#MonthDiv').show();
                }
                else {
                    $('#MonthDiv').hide();
                }
            });
        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
    </script>
    <div id="SearchPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
        </a>
        <div class="HMBodyContainer">
            <div class="block-body collapse in">
                <div class="HMContainerRow">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlYear" runat="server">
                                <asp:ListItem Value="2005">2005</asp:ListItem>
                                <asp:ListItem Value="2006">2006</asp:ListItem>
                                <asp:ListItem Value="2007">2007</asp:ListItem>
                                <asp:ListItem Value="2008">2008</asp:ListItem>
                                <asp:ListItem Value="2009">2009</asp:ListItem>
                                <asp:ListItem Value="2010">2010</asp:ListItem>
                                <asp:ListItem Value="2011">2011</asp:ListItem>
                                <asp:ListItem Value="2012">2012</asp:ListItem>
                                <asp:ListItem Value="2013">2013</asp:ListItem>
                                <asp:ListItem Value="2014">2014</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblReportType" runat="server" Text="Report Type"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlReportType" runat="server">
                                <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection" id="MonthDiv">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblMonth" runat="server" Text="Month"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlMonth" runat="server">
                                <asp:ListItem Value="January">January</asp:ListItem>
                                <asp:ListItem Value="February">February</asp:ListItem>
                                <asp:ListItem Value="March">March</asp:ListItem>
                                <asp:ListItem Value="April">April</asp:ListItem>
                                <asp:ListItem Value="May">May</asp:ListItem>
                                <asp:ListItem Value="June">June</asp:ListItem>
                                <asp:ListItem Value="July">July</asp:ListItem>
                                <asp:ListItem Value="August">August</asp:ListItem>
                                <asp:ListItem Value="September">September</asp:ListItem>
                                <asp:ListItem Value="October">October</asp:ListItem>
                                <asp:ListItem Value="November">November</asp:ListItem>
                                <asp:ListItem Value="December">December</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                        OnClick="btnSearch_Click" />
                </div>
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
    <div id="ReportPanel" class="block" style="display: none;">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Report:: Banquet
            Revenue Information </a>
        <div class="block-body collapse in">
            <div class="ReporContainerDiv">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                    DocumentMapWidth="100%" Font-Size="8pt" InteractiveDeviceInfos="(Collection)"
                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    <LocalReport ReportPath="Banquet\Reports\Rdlc\rptBanquetInformation.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="RoomReservationDataSource" Name="DSBanquetRevenueInfo" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="RoomReservationDataSource" runat="server" SelectMethod="GetData"
                    TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetBanquetRevenueInfoForBarNLineChart_SPTableAdapter"
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:FormParameter FormField="txtReportYear" Name="ReportYear" Type="String" />
                        <asp:FormParameter FormField="txtReportDurationName" Name="ReportDurationName" Type="String" />
                        <asp:FormParameter FormField="txtReportFor" Name="ReportFor" Type="String" />
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
