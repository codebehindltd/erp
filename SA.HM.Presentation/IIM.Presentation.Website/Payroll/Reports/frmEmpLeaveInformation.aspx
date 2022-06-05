<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpLeaveInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmEmpLeaveInformation" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagName="EmployeeSearch" TagPrefix="UserControl" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Leave Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Leave</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
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

        function WorkAfterSearchEmployee()
        { }

    </script>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">Employee Leave Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div id="employeeSearchSection">
                    <UserControl:EmployeeSearch runat="server" ID="employeeeSearch" />
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Report Year"></asp:Label>
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
                            <asp:ListItem Value="0">Details</asp:ListItem>
                            <asp:ListItem Value="1">Monthly Summary</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary"
                            OnClick="btnGenerate_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">Employee Leave Information</div>
        <div class="panel-body">
            <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
            </rsweb:ReportViewer>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=dispalyReport%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
