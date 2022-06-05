<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmServiceAndRoomBillByDate.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmServiceAndRoomBillByDate" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Night Audit Report</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            var txtApprovedDate = '<%=txtApprovedDate.ClientID%>'

            $("#ContentPlaceHolder1_txtApprovedDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat               
            });
        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
    </script>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <%-- Left Left--%>
                        <asp:Label ID="lblApprovedDate" runat="server" class="control-label" Text="Night Audit Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <%--Right Right--%>
                        <asp:TextBox ID="txtApprovedDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidToDate" />
                        <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
                    </div>
                    <div class="col-md-2" style="display:none;">
                        <asp:Label ID="lblServiceName" runat="server" class="control-label" Text="Service Name"></asp:Label>
                    </div>
                    <div class="col-md-4" style="display:none;">
                        <asp:DropDownList ID="ddlServiceName" runat="server" CssClass="form-control">
                        </asp:DropDownList>
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
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000;
            top: 2000;" clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Report:: Night Audit Information</div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="700px">
                    <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                        PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                        <%--<LocalReport ReportPath="HotelManagement\Reports\Rdlc\rptServiceAndRoomBillByDate.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="TransactionDataSource" Name="DSServiceAndRoomBillByDate" />
                            </DataSources>
                        </LocalReport>--%>
                    </rsweb:ReportViewer>
                    <%--<asp:ObjectDataSource ID="TransactionDataSource" runat="server" SelectMethod="GetData"
                        TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetTotalServiceAndRoomBillByDate_SPTableAdapter"
                        OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:FormParameter FormField="txtApprovedDate" Name="ApprovedDate" Type="DateTime" />
                            <asp:FormParameter FormField="txtCompanyName" Name="CompanyName" Type="String" />
                            <asp:FormParameter FormField="txtCompanyAddress" Name="CompanyAddress" Type="String" />
                            <asp:FormParameter FormField="txtCompanyWeb" Name="CompanyWeb" Type="String" />
                            <asp:FormParameter FormField="ddlServiceName" Name="ServiceName" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>--%>
                </asp:Panel>
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
