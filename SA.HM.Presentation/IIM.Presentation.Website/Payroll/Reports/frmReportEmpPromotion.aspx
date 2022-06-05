<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportEmpPromotion.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportEmpPromotion" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeSearchAll" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Promotion Report</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            var type = $("#ContentPlaceHolder1_ddlSrcType").val();
            ShowHideSrcDiv(type);

            $("#ContentPlaceHolder1_ddlSrcType").change(function () {
                var type = $("#ContentPlaceHolder1_ddlSrcType").val();
                ShowHideSrcDiv(type);
            });
        });
        function ShowHideSrcDiv(type) {
            if (type == "All") {
                $("#DepartmentDiv").show();
                $("#empSrcDiv").hide();

            }
            else if (type == "Individual") {
                $("#DepartmentDiv").hide();
                $("#empSrcDiv").show();
            }
            else {
                $("#DepartmentDiv").hide();
                $("#empSrcDiv").hide();
            }
        }
        function LoadSearch() {
            var type = $("#ContentPlaceHolder1_ddlSrcType").val();
            if (type == "0") {
                toastr.warning("Please Select Search Type");
                return false;
            }
            else if (type == "Individual") {
                var empId = $("#ContentPlaceHolder1_employeeSearchall_hfEmployeeId").val();
                if (empId == "0") {
                    toastr.warning("Please Select an Employee");
                    return false;
                }
            }
            else if (type == "All") {
                var year = $("#ContentPlaceHolder1_ddlFiscalYear").val();
                if (year == "0") {
                    toastr.warning("Please Select a Fiscal Year.");
                    $("#ContentPlaceHolder1_ddlFiscalYear").focus();
                    return false;
                }
            }

            return true;
        }
        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

        function WorkAfterSearchEmployee()
        { }

    </script>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Promotion Info</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Search Type"></asp:Label>                               
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSrcType" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0" Text="---Please Select---"></asp:ListItem>
                                    <asp:ListItem Value="All" Text="All Employee"></asp:ListItem>
                                    <asp:ListItem Value="Individual" Text="Individual"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            
                        </div>
                <div class="form-group" id="DepartmentDiv" style="display:none">
                    <div class="col-md-2">
                        <asp:Label ID="lblDepartment" runat="server" class="control-label" Text="Department"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDepartment" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    
                </div>
                <div id="empSrcDiv" style="display:none">
                            <UserControl:EmployeeSearchAll ID="employeeSearchall" runat="server" />
                        </div>
                
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFiscalYear" runat="server" class="control-label required-field"
                            Text="Fiscal Year"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary"
                            OnClick="btnGenerate_Click" OnClientClick="javascript: return LoadSearch()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Promotion Info</div>
        <div class="panel-body">
            <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="830px" Height="820px">
                </rsweb:ReportViewer>
            <%--<rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                Font-Names="Verdana" Font-Size="8pt" ShowPrintButton="true" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" Width="950px" Height="820px">
            </rsweb:ReportViewer>--%>
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
