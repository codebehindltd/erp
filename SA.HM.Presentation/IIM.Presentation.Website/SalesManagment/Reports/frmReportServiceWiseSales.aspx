<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HMReport.Master" AutoEventWireup="true"
    CodeBehind="frmReportServiceWiseSales.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.Reports.frmReportServiceWiseSales" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Service Sales Info</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $('#' + txtStartDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });
        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

        //MessageDiv Visible True/False-------------------
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
    <div id="SearchPanel" class="block">
        <div class="row" style="padding-left: 30px">
            <div class="HMBodyContainer">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <label>
                            Report Type</label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlReportType" runat="server" TabIndex="6" CssClass="ThreeColumnDropDownList">
                            <asp:ListItem Text="Customer Details" Value="CustomerDetails"></asp:ListItem>
                            <asp:ListItem Text="Service Details" Value="ServiceDetails"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>

                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <label id="lblFromDate">
                            From Date</label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox><input type="hidden"
                            id="hidFromDate" />
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <label id="lblToDate">
                            To Date</label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                        <input type="hidden" id="hidToDate" />
                    </div>
                </div>

                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <label id="lblServiceName">
                            Service Name</label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlServiceId" runat="server" TabIndex="6" CssClass="ThreeColumnDropDownList">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <label>
                            Customer</label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlCustomer" runat="server" TabIndex="6" CssClass="ThreeColumnDropDownList">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <label>
                            Customer Status</label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlCustomerStatus" runat="server" TabIndex="6" CssClass="ThreeColumnDropDownList">
                            <asp:ListItem Text="---All---" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Active" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary"
                        OnClick="btnGenarate_Click" />
                </div>
            </div>
        </div>
    </div>
    <div id="ReportPanel" class="block" style="display: none;">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Report:: Service
            Sales Information </a>
        <div class="block-body collapse in">
            <div style="width: 858px; height: 700px; overflow-x: scroll; overflow-y: scroll;
                text-align: left;">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                </rsweb:ReportViewer>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var xMessage = '<%=isMessageBoxEnable%>';
        if (xMessage > -1) {
            MessagePanelShow();
        }
        else {
            MessagePanelHide();
        }

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
