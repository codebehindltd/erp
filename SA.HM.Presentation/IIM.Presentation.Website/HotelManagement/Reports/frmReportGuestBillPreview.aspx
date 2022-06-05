<%@ Page Title="" Language="C#" MasterPageFile="~/Common/ReportViewer.Master" AutoEventWireup="true"
    CodeBehind="frmReportGuestBillPreview.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportGuestBillPreview" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Guest Bill</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });
        function InvoiceTemplate1VisibleTrue() {
            $('#InvoiceTemplate1').show();
        }

        function InvoiceTemplate1VisibleFalse() {
            $('#InvoiceTemplate1').hide();
        }

        function InvoiceTemplate2VisibleTrue() {
            $('#InvoiceTemplate2').show();
        }

        function InvoiceTemplate2VisibleFalse() {
            $('#InvoiceTemplate2').hide();
        }

    </script>
    <div style="display: none;">
        <asp:GridView ID="gvRoomDetail" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="200">
            <RowStyle BackColor="#E3EAEB" />
            <Columns>
                <asp:TemplateField HeaderText="RegistrationId" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IncomeNodeId" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblIncomeNodeId" runat="server" Text='<%#Eval("IncomeNodeId") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IDNO" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IDNO" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceDate" runat="server" Text='<%#Eval("ServiceDate") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Date" ItemStyle-Width="15%">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ServiceDate"))) %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="ServiceType" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServiceType") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ServiceId" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RoomNumber" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblRoomNumber" runat="server" Text='<%#Eval("RoomNumber") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label></ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Room Tariff">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceRate" runat="server" Text='<%# bind("ServiceRate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="15%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quantity" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceQuantity" runat="server" Text='<%# bind("ServiceQuantity") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="6%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="S. Charge">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceCharge" runat="server" Text='<%# bind("ServiceCharge") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="10%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vat">
                    <ItemTemplate>
                        <asp:Label ID="lblVatAmount" runat="server" Text='<%# bind("VatAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="8%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sales Commission" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblgvReferenceSalesCommission" runat="server" Text='<%# Bind("ReferenceSalesCommission") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Discount">
                    <ItemTemplate>
                        <asp:Label ID="lblDiscountAmount" runat="server" Text='<%# bind("DiscountAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="8%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalAmount" runat="server" Text='<%# bind("TotalAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="20%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="NightAudit" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblNightAuditApproved" runat="server" Text='<%# bind("NightAuditApproved") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="20%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:GridView ID="gvServiceDetail" Width="100%" runat="server" AllowPaging="True"
            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
            ForeColor="#333333" PageSize="200">
            <RowStyle BackColor="#E3EAEB" />
            <Columns>
                <asp:TemplateField HeaderText="RegistrationId" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IncomeNodeId" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblIncomeNodeId" runat="server" Text='<%#Eval("IncomeNodeId") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IDNO" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IDNO" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceDate" runat="server" Text='<%#Eval("ServiceDate") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ServiceType" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServiceType") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ServiceId" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label></ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceRate" runat="server" Text='<%# bind("ServiceRate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="10%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceQuantity" runat="server" Text='<%# bind("ServiceQuantity") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="6%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="S. Charge">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceCharge" runat="server" Text='<%# bind("ServiceCharge") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="10%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vat">
                    <ItemTemplate>
                        <asp:Label ID="lblVatAmount" runat="server" Text='<%# bind("VatAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="8%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Discount">
                    <ItemTemplate>
                        <asp:Label ID="lblDiscountAmount" runat="server" Text='<%# bind("DiscountAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="8%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalAmount" runat="server" Text='<%# bind("TotalAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="20%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="NightAudit" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblNightAuditApproved" runat="server" Text='<%# bind("NightAuditApproved") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="20%"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IsPaidService" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblgvIsPaidService" runat="server" Text='<%# Bind("IsPaidService") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <iframe id="frmPrint" name="IframeName" width="0" height="0" runat="server" style="left: -1000;
        top: 2000;" clientidmode="static"></iframe>
    <div>
        <div class="row">
            <div class="columnRight">
                <asp:TextBox ID="txtRegistrationNumber" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtGroupCompanyName" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtVatRegistrationNo" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtIsBillSplited" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtGuestBillFromDate" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtGuestBillToDate" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtPrintedBy" runat="server" Visible="False"></asp:TextBox>
            </div>
            <div class="clear">
            </div>
        </div>
        <div>
            <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="720px">
                <div id="InvoiceTemplate1" runat="server">
                    <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                        PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                        <LocalReport ReportPath="HotelManagement\Reports\Rdlc\rptGenerateGuestBill.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="TransactionDataSource" Name="GuestBillInfo" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="TransactionDataSource" runat="server" SelectMethod="GetData"
                        TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GenerateGuestBillTableAdapter"
                        OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:FormParameter FormField="txtRegistrationNumber" Name="RegistrationId" Type="String" />
                            <asp:FormParameter FormField="txtCompanyName" Name="CompanyName" Type="String" />
                            <asp:FormParameter FormField="txtCompanyAddress" Name="CompanyAddress" Type="String" />
                            <asp:FormParameter FormField="txtCompanyWeb" Name="CompanyWeb" Type="String" />
                            <asp:FormParameter FormField="txtIsBillSplited" Name="IsBillSplited" Type="String" />
                            <asp:FormParameter FormField="txtGuestBillFromDate" Name="GuestBillFromDate" Type="String" />
                            <asp:FormParameter FormField="txtGuestBillToDate" Name="GuestBillToDate" Type="String" />
                            <asp:FormParameter FormField="txtPrintedBy" Name="PrintedBy" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <div id="InvoiceTemplate2" runat="server">
                    <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvInvoiceTransaction"
                        PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    </rsweb:ReportViewer>
                </div>
            </asp:Panel>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=IsInvoiceTemplate1Visible%>';

        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvInvoiceTransaction.ClientID %>"));

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
    </script>
</asp:Content>
