<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportGuestPaymentInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.Reports.frmReportGuestPaymentInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Guest Payment Info</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'
            var ddlGuestType = '<%=ddlGuestType.ClientID%>'

            var guestType = $('#' + ddlGuestType).val();
            if (guestType == "InHouseGuest") {
                $('#InnhouseGuest').show();
            }
            else {
                $('#InnhouseGuest').hide();
            }

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $('#' + ddlGuestType).change(function () {
                var guestType = $('#' + ddlGuestType).val();
                if (guestType == "InHouseGuest") {
                    $('#InnhouseGuest').show();
                }
                else {
                    $('#InnhouseGuest').hide();
                }
            });


        });

        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

        function CheckValidation() {
        }

    </script>
    <div id="SearchPanel" class="panel panel-default">        
        <div class="panel-heading">Search Information</div>
        <div class="panel-body">
            <div class="form-horizontal">                
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblGuestType" runat="server" class="control-label" Text="Guest Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlGuestType" CssClass="form-control" runat="server">
                                <asp:ListItem Text="In-House Guest" Value="InHouseGuest" />
                                <asp:ListItem Text="Walk-In Guest" Value="OutSideGuest" />
                            </asp:DropDownList>
                        </div>
                        <div id="InnhouseGuest">
                            <div class="col-md-2">
                                <asp:Label ID="lblPaymentType" runat="server" class="control-label" Text="Payment Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPaymentType" CssClass="form-control" runat="server">
                                    <asp:ListItem Text="--- All ---" Value="All" />
                                    <asp:ListItem Text="Advance" Value="Advance" />
                                    <asp:ListItem Text="Paid Out" Value="PaidOut" />
                                    <asp:ListItem Text="Refund" Value="Refund" />
                                    <asp:ListItem Text="Company" Value="Company" />
                                    <asp:ListItem Text="Employee" Value="Employee" />
                                    <asp:ListItem Text="Other Room " Value="OtherRoom " />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>                    
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                            <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                            <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
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
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblPaymentMode" runat="server" class="control-label" Text="Payment Mode"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPaymentMode" CssClass="form-control" runat="server">
                            <asp:ListItem Text="--- All ---" Value="All" />
                            <asp:ListItem Text="Cash" Value="Cash" />
                            <asp:ListItem Text="Card" Value="Card" />
                            <asp:ListItem Text="Refund" Value="Refund" />
                            <asp:ListItem Text="Company" Value="Company" />
                            <asp:ListItem Text="Employee" Value="Employee" />
                            <asp:ListItem Text="Other Room " Value="OtherRoom " />
                        </asp:DropDownList>
                    </div>
                </div>               
                <div class="row">
                    <div class="col-md-12">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary"
                        OnClick="btnSearch_Click" OnClientClick="javascript:return CheckValidation()" />
                        </div>
                </div>
            </div>
        </div>
    </div>    
    <div class="row">
        <div class="columnRight">
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
    <div id="ReportPanel" class="panel panel-default" style="display: none">        
            <div class="panel-heading">Report:: Guest Payment
            Information</div>
        <div class="panel-body">
            <div class="ReporContainerDiv">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    <%--<LocalReport ReportPath="HotelManagement\Reports\Rdlc\rptGuestPaymentInfo.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="TransactionDataSource" Name="DSGuestPayment" />
                        </DataSources>
                    </LocalReport>--%>
                </rsweb:ReportViewer>
                <%--<asp:ObjectDataSource ID="TransactionDataSource" runat="server" SelectMethod="GetData"
                    TypeName="HotelManagement.Presentation.Website.HotelManagementDBDataSetTableAdapters.GetGuestPaymentInformationForReport_SPTableAdapter"
                    OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:FormParameter FormField="ddlFilterBy" Name="GuestType" Type="String" />
                        <asp:FormParameter FormField="ddlServiceId" Name="PaymentType" Type="String" />
                        <asp:FormParameter FormField="txtFromDate" Name="FromDate" Type="DateTime" />
                        <asp:FormParameter FormField="txtToDate" Name="ToDate" Type="DateTime" />
                        <asp:FormParameter FormField="ddlPaymentMode" Name="PaymentMode" Type="String" />
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
