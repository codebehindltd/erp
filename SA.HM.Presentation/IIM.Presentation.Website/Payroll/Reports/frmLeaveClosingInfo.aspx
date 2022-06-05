<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HMReport.Master" AutoEventWireup="true"
    CodeBehind="frmLeaveClosingInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmLeaveClosingInfo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagName="EmpSearch" TagPrefix="UserControl" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Payslip</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            // $("#lblEmployee")._hide();
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

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="block">
        <div class="HMBodyContainer">           
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="Label1" runat="server" Text="Department"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="ThreeColumnDropDownList">
                    </asp:DropDownList>
                </div>
            </div>            
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary"
                    OnClick="btnGenarate_Click" OnClientClick="javascript:return CheckValdity()" />
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
    <%--<div class="row">
        <div class="columnRight">
            <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
        </div>
        <div class="clear">
        </div>
    </div>--%>
    <%--</div>--%>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000;
            top: 2000;" clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Report:: Employee
            Payslip</a>
        <div class="block-body collapse in">
            <div class="ReporContainerDiv">
                <asp:Panel ID="pnlReporContainer" runat="server" ScrollBars="Both" Height="700px">
                    <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                        PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                    </rsweb:ReportViewer>
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

        var xMessage = '<%=isMessageBoxEnable%>';
        if (xMessage > -1) {
            //MessagePanelShow();
        }
        else {
            //MessagePanelHide();
        }

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
