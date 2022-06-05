<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportReconcilation.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportReconcilation" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employees Reconcilation</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        function CheckValdity() {

            if ($("#ContentPlaceHolder1_ddlEffectedMonth").val() == '0') {
                toastr.warning("Please Select Process Month");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlYear").val() == '0') {
                toastr.warning("Please Select Process Year");
                return false;
            }

            return true;
        }

        function ReportPanelShow() {
            $('#ReportPanel').show();
        }
        function ReportPanelHide() {
            $('#ReportPanel').hide();
        }

    </script>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblProcessDate" runat="server" class="control-label required-field" Text="Process Month"></asp:Label>                        
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlEffectedMonth" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblYear" runat="server" class="control-label" Text="Year"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <%--<div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="Label1" runat="server" Text="Department"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="ThreeColumnDropDownList">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>--%>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Is Management"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Bank"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlBankId" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
 <div class="col-md-12">
                    <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary"
                        OnClick="btnGenarate_Click" OnClientClick="javascript:return CheckValdity()" />
                </div>
                </div>
            </div>
        </div>
        </div>
        <div id="ReportPanel" class="panel panel-default" style="display: none">
            <div class="panel-heading">
                Report:: Employees Reconcilation</div>
            <div class="panel-body">
                <asp:Panel ID="pnlReporContainer" runat="server" ScrollBars="Both" Height="700px">
                    <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                        PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    </rsweb:ReportViewer>
                </asp:Panel>
            </div>
        </div>
        <script type="text/javascript">
            var xMessage = '<%=isDisplayReport%>';
            if (xMessage > -1) {
                ReportPanelShow();
            }
            else {
                ReportPanelHide();
            }        
        </script>
</asp:Content>
