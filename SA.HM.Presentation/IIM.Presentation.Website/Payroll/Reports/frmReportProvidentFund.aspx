<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HMReport.Master" AutoEventWireup="true"
    CodeBehind="frmReportProvidentFund.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportProvidentFund" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLoanSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Provident Fund</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Provident Fund Report</li>";
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

    </script>
    <div id="SearchPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Provident Fund Info
        </a>
        <div class="HMBodyContainer">
            <UserControl:EmployeeForLoanSearch ID="employeeForLoanSearch" runat="server" />
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblLeaveMode" runat="server" Text="Department"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="customMediumDropDownSize"
                        TabIndex="4">
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblProcessDate" runat="server" Text="Salary Month"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:DropDownList ID="ddlProcessMonth" runat="server" CssClass="customSmallDropDownSize"
                        TabIndex="2">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="Label1" runat="server" Text="Report Type"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlReportType" runat="server" CssClass="customMediumDropDownSize"
                        TabIndex="4">
                        <asp:ListItem Value="0" Text="---Select---"></asp:ListItem>
                        <asp:ListItem Value="PF" Text="PF">PF</asp:ListItem>
                        <asp:ListItem Value="Tax" Text="Tax">Tax</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                </div>
                <div class="divBox divSectionRightRight">
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary"
                    OnClick="btnGenerate_Click" />
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000;
            top: 2000;" clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="block" style="display: none;">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Provident Fund Info</a>
        <div class="block-body collapse in">
            <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
                Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="100%" Height="820px">
            </rsweb:ReportViewer>
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

        var x = '<%=dispalyReport%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>
