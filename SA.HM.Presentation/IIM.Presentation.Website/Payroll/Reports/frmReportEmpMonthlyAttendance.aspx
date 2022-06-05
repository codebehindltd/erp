<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportEmpMonthlyAttendance.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmReportEmpMonthlyAttendance" %>

<%@ Register TagPrefix="UserControl" TagName="companyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            if ($("#<%=ddlEmployee.ClientID %>").val() == "0")
                $("#employeeSearchSection").hide();

            if ($("#<%=ddlEmployee.ClientID %>").val() == "0") {
                $("#DepartmentInfoDiv").show();
            }
            else {
                $("#DepartmentInfoDiv").hide();
            }

            $("#<%= ddlEmployee.ClientID %>").change(function () {
                if ($(this).val() == "0") {
                    $("#DepartmentInfoDiv").show();
                }
                else {
                    $("#DepartmentInfoDiv").hide();
                }
            });

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Attendance & Time Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Attendance Report</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_ddlEmployee").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }

        function ValidationProcess() {
            var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            $("#ContentPlaceHolder1_hfCompanyId").val(company);
            $("#ContentPlaceHolder1_hfProjectId").val(project);
            $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
            $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());
        }
    </script>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyName" runat="server" Value="0" />
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0" />
    <asp:HiddenField ID="hfProjectName" runat="server" Value="0" />
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:companyProjectUserControl ID="companyProjectUserControl" runat="server" />
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblEmployee" runat="server" class="control-label" Text="Employee"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlEmployee" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="DepartmentInfoDiv" class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDepartment" runat="server" class="control-label" Text="Department"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlDepartment" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblYear" runat="server" class="control-label" Text="Year"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblMonth" runat="server" class="control-label" Text="Month"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlMonth" CssClass="form-control" runat="server">
                            <asp:ListItem Value="January">January</asp:ListItem>
                            <asp:ListItem Value="February">February</asp:ListItem>
                            <asp:ListItem Value="March">March</asp:ListItem>
                            <asp:ListItem Value="April">April</asp:ListItem>
                            <asp:ListItem Value="May">May</asp:ListItem>
                            <asp:ListItem Value="June">June</asp:ListItem>
                            <asp:ListItem Value="July">July</asp:ListItem>
                            <asp:ListItem Value="August">August</asp:ListItem>
                            <asp:ListItem Value="September">September</asp:ListItem>
                            <asp:ListItem Value="October">October</asp:ListItem>
                            <asp:ListItem Value="November">November</asp:ListItem>
                            <asp:ListItem Value="December">December</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server">
                            <asp:ListItem Value="1" Text="Report Type 1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Report Type 2"></asp:ListItem>
                            <%--<asp:ListItem Value="3" Text="Monthly Summary"></asp:ListItem> --%>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary"
                            OnClick="btnGenarate_Click" OnClientClick="javascript:return ValidationProcess();" />
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
            Report:: Employee
            Monthly Attendance
        </div>
        <div class="panel-body">
            <div style="height: 700px; overflow-x: scroll; overflow-y: scroll; text-align: left;">
                <rsweb:ReportViewer ID="rvTransaction" runat="server" ShowFindControls="false" ShowWaitControlCancelLink="false"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" Font-Names="Verdana"
                    Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                </rsweb:ReportViewer>
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

            if ($("#ContentPlaceHolder1_companyProjectUserControl_hfIsSingle").val() != "1") {
                var c = $("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val();
                var p = $("#ContentPlaceHolder1_hfProjectId").val();

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val($("#ContentPlaceHolder1_hfProjectId").val()).trigger("change");

            }

            var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            $("#ContentPlaceHolder1_hfCompanyId").val(company);
            $("#ContentPlaceHolder1_hfProjectId").val(project);
            $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
            $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());
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
