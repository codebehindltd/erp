<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReportEmpSalarySheet.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportEmpSalarySheet" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagName="EmpSearch" TagPrefix="UserControl" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Salary Sheet</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_ddlGLCompany").select2({
                tags: false,
                allowClear: true,
                placeholder: "",
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlDepartmentId").select2({
                tags: false,
                allowClear: true,
                placeholder: "",
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlDesignationId").select2({
                tags: false,
                allowClear: true,
                placeholder: "",
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlGradeId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            var single = $("#ContentPlaceHolder1_hfIsSingle").val();
            if (single == "1") {
                $('#glCompanyDiv').hide();

            }
            else {
                $('#glCompanyDiv').show();
            }
        });
        function CheckValidity() {
            var type = $("#ContentPlaceHolder1_EmployeeSearchControl_ddlEmployee").val();

            if (type == "1") {
                var name = $("#ContentPlaceHolder1_EmployeeSearchControl_txtSearchEmployee").val();
                var id = $("#ContentPlaceHolder1_EmployeeSearchControl_hfEmployeeId").val();
                if (name == "") {
                    toastr.warning("Please select an Employee");
                    return false;
                }
                else if (id == "0") {
                    toastr.warning("Please select a Valid Employee");
                    return false;
                }
            }

            return true;
        }
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
    <asp:HiddenField ID="hfIsPayrollCompanyAndEmployeeCompanyDifferent" runat="server" Value="0" />
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Process Month"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="--- All ----" Value="All"></asp:ListItem>
                            <asp:ListItem Text="--- Location Wise ----" Value="Location"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4">
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblProcessDate" runat="server" class="control-label required-field" Text="Report Month"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlEffectedMonth" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div id="glCompanyDiv" style="display: none">
                        <div class="col-md-2">
                            <asp:HiddenField ID="hfIsSingle" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hfGLCompanyId" runat="server" Value="0" />
                            <asp:HiddenField ID="hfGLProjectId" runat="server" Value="0" />
                            <asp:Label ID="lblGLCompany" runat="server" class="control-label"
                                Text="Company"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <UserControl:EmpSearch ID="EmployeeSearchControl" runat="server" />
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Department"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblDesignationId" runat="server" class="control-label"
                            Text="Designation"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDesignationId" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblWorkStation" runat="server" class="control-label" Text="Work Station"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlWorkStation" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Grade"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGrade" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReportFormat" runat="server" class="control-label" Text="Report Format"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportFormat" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Regular" Value="Regular"></asp:ListItem>
                            <asp:ListItem Text="Department Wise" Value="DepartmentWise"></asp:ListItem>
                            <asp:ListItem Text="Designation Wise" Value="DesignationWise"></asp:ListItem>
                            <asp:ListItem Text="Work Station Wise" Value="WorkStationWise"></asp:ListItem>
                            <asp:ListItem Text="Gender Wise" Value="GenderWise"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Currency Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCurrencyType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Local Currency" Value="LocalCurrency"></asp:ListItem>
                            <asp:ListItem Text="Individual Currency" Value="IndividualCurrency"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary"
                            OnClick="btnGenarate_Click" OnClientClick="javascript:return CheckValidity()" />
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
            Report:: Employee Salary Sheet Information
        </div>
        <div class="panel-body">
            <asp:Panel ID="pnlReporContainer" runat="server" ScrollBars="Both" Height="700px">

                <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="830px" Height="820px">
                </rsweb:ReportViewer>
            </asp:Panel>
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
