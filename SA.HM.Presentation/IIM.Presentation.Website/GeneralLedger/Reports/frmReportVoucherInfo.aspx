<%@ Page Title="" Language="C#" MasterPageFile="~/Common/ReportViewer.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportVoucherInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.Reports.frmReportVoucherInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Voucher Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });

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
    <div class="row">
        <div class="columnRight">
            <asp:TextBox ID="txtDealId" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
        </div>
        <div class="clear">
        </div>
    </div>
    <iframe id="frmPrint" name="IframeName" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
        clientidmode="static"></iframe>
    <div id="ReportPanel" class="block">
        <div class="ReporContainerDiv">
            <div id="JournalNContraVoucherDiv" runat="server">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    <LocalReport ReportPath="GeneralLedger\Reports\Rdlc\RptVoucherInfo.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="TransactionDataSource" Name="VoucherInfo" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="TransactionDataSource" runat="server" SelectMethod="GetData"
                    TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetVoucherInfoByDealIdForReport_SPTableAdapter"
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:FormParameter FormField="txtDealId" Name="DealId" Type="Int32" />
                        <asp:FormParameter FormField="txtCompanyName" Name="CompanyName" Type="String" />
                        <asp:FormParameter FormField="txtCompanyAddress" Name="CompanyAddress" Type="String" />
                        <asp:FormParameter FormField="txtCompanyWeb" Name="CompanyWeb" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
            <div id="ReceiveVoucher" runat="server">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransactionReceive"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    <LocalReport ReportPath="GeneralLedger\Reports\Rdlc\RptReceiveVoucherInfo.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="TransactionDataSourceReceive" Name="VoucherInfo" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="TransactionDataSourceReceive" runat="server" SelectMethod="GetData"
                    TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetVoucherInfoByDealIdForReport_SPTableAdapter"
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:FormParameter FormField="txtDealId" Name="DealId" Type="Int32" />
                        <asp:FormParameter FormField="txtCompanyName" Name="CompanyName" Type="String" />
                        <asp:FormParameter FormField="txtCompanyAddress" Name="CompanyAddress" Type="String" />
                        <asp:FormParameter FormField="txtCompanyWeb" Name="CompanyWeb" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
            <div id="PaymentVoucher" runat="server">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransactionPayment"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    <LocalReport ReportPath="GeneralLedger\Reports\Rdlc\RptPaymentVoucherInfo.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="TransactionDataSourcePayment" Name="VoucherInfo" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="TransactionDataSourcePayment" runat="server" SelectMethod="GetData"
                    TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetVoucherInfoByDealIdForReport_SPTableAdapter"
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:FormParameter FormField="txtDealId" Name="DealId" Type="Int32" />
                        <asp:FormParameter FormField="txtCompanyName" Name="CompanyName" Type="String" />
                        <asp:FormParameter FormField="txtCompanyAddress" Name="CompanyAddress" Type="String" />
                        <asp:FormParameter FormField="txtCompanyWeb" Name="CompanyWeb" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div id="EntryPanel" class="block" style="display: none;">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Change Voucher Status</a>
        <div class="HMBodyContainer">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:HiddenField ID="txtReportId" runat="server"></asp:HiddenField>
                    <asp:Label ID="lblChangeStatus" runat="server" Text="Change Status"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlChangeStatus" runat="server" TabIndex="1">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                        <asp:ListItem>Cancel</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <asp:Button ID="btnChangeStatus" runat="server" Text="Change" CssClass="TransactionalButton btn btn-primary"
                    TabIndex="2" OnClick="btnChangeStatus_Click" />
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
        <asp:Button ID="btnReceivePrintReportFromClient" runat="server" Text="Button" OnClick="btnReceivePrintReportFromClient_Click"
            ClientIDMode="Static" />
        <asp:Button ID="btnPaymentPrintReportFromClient" runat="server" Text="Button" OnClick="btnPaymentPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }


        $(document).ready(function () {

            var printButtonFlag = parseInt('<%=printButtonFlag%>');

            if (printButtonFlag == 0 && (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome)) {
                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransaction.ClientID %>"));
                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);

            }

            if (printButtonFlag == 2 && (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome)) {
                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransactionReceive.ClientID %>"));
                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintReceiveDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);

            }

            if (printButtonFlag == 1 && (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome)) {
                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransactionPayment.ClientID %>"));
                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintPaymentDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);

            }
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }
        function PrintReceiveDocumentFunc(ss) {
            $('#btnReceivePrintReportFromClient').trigger('click');
            return true;
        }
        function PrintPaymentDocumentFunc(ss) {
            $('#btnPaymentPrintReportFromClient').trigger('click');
            return true;
        }

    </script>
</asp:Content>
