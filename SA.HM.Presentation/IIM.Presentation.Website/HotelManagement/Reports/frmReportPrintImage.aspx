<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HMReport.Master" AutoEventWireup="true" CodeBehind="frmReportPrintImage.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportPrintImage" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Guest Documents</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            var title = " ";
            setTimeout(function () {
                $("img").each(function () {
                    title = title + $(this).attr('title');
                    $(this).attr("src", $(this).attr('title'));
                    $(this).attr("width", '500px');
                    $(this).attr("padding-bottom",'10px');
                });
              
            }, 3000);  
        });

        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

    </script>
    <div id="SearchPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
        </a>
        <div class="HMBodyContainer">
            <div class="block-body collapse in" style=" display:none">
                <div class="HMContainerRow">
                    <div class="divSection">
                            <asp:TextBox ID="txtGuestId" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtGuestType" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>

    </div>
    <div class="divClear">
    </div>
    <div class="row">
        <div class="columnRight">
        </div>
        <div class="clear">
        </div>
    </div>
    <div id="ReportPanel" class="block" style="display: none">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Report:: Guest Documents </a>
        <div class="block-body collapse in">
            <div class="ReporContainerDiv">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    <LocalReport ReportPath="HotelManagement\Reports\Rdlc\rptPrintGuestDocuments.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="TransactionDataSource" Name="DSPrintGuestDocuments" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="TransactionDataSource" runat="server" SelectMethod="GetData"
                    TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetGuestDocumentByGuestIdAndType_SPTableAdapter"
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:FormParameter FormField="txtGuestId" Name="GuestId" Type="String" />
                        <asp:FormParameter FormField="txtGuestType" Name="GuestType" Type="String" />
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
