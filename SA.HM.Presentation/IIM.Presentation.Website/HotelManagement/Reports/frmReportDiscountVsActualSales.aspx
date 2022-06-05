<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportDiscountVsActualSales.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportDiscountVsActualSales" %>

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


            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Discount VS Actual Sales</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

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
    <div id="SearchPanel" class="panel panel-default">        
        <div class="panel-heading">Search Information</div>
        <div class="panel-body">
            <div class="form-horizontal">                
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblYear" runat="server" class="control-label" Text="Year"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server">
                                <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>                   
                    <div id="MonthDiv" class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblMonth" runat="server" class="control-label" Text="Month"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlMonth" CssClass="form-control" runat="server">
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
                <div class="row" style="display: none">
                    <div class="columnRight">
                        <asp:TextBox ID="txtReportYear" runat="server"></asp:TextBox>
                        <asp:TextBox ID="txtReportDurationName" runat="server"></asp:TextBox>
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
            <div class="panel-heading">Report:: Discount
            VS Actual Sales Report</div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    <%--<LocalReport ReportPath="HotelManagement\Reports\Rdlc\rptDiscountVsActualSales.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="RoomReservationDataSource" Name="DSDiscountVSActualSales" />
                        </DataSources>
                    </LocalReport>--%>
                </rsweb:ReportViewer>
                <%--<asp:ObjectDataSource ID="RoomReservationDataSource" runat="server" SelectMethod="GetData"
                    TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetInnboardDiscountVSActualSalesInfoWithinYear_SPTableAdapter"
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:FormParameter FormField="txtReportYear" Name="ReportYear" Type="String" />
                        <asp:FormParameter FormField="txtReportDurationName" Name="ReportDurationName" Type="String" />
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
