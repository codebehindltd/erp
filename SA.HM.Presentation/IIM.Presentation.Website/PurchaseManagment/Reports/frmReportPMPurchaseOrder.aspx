<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportPMPurchaseOrder.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.Reports.frmReportPMPurchaseOrder" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Purchase Order Information</li>";
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
            <asp:TextBox ID="txtPOrderId" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
        </div>
    </div>
    <div id="ReportPanel" class="panel panel-default">
        <div class="panel-heading">
            Report:: Purchase Order Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="txtReportId" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblChangeStatus" runat="server" class="control-label" Text="Change Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlChangeStatus" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem>Submitted</asp:ListItem>
                            <asp:ListItem>Approved</asp:ListItem>
                            <asp:ListItem>Cancel</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnChangeStatus" runat="server" Text="Change" CssClass="TransactionalButton btn btn-primary btn-sm"
                            TabIndex="2" OnClick="btnChangeStatus_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-body">
            <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
                Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
            </rsweb:ReportViewer>
        </div>
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
    </script>
</asp:Content>
