<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation ="false"
    CodeBehind="frmEmpLeaveReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.Reports.frmEmpLeaveReport" %>

<%@ Register TagPrefix="UserControl" TagName="companyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagName="EmployeeSearch" TagPrefix="UserControl" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Leave Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Leave Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#ContentPlaceHolder1_ddlReportType").val() == "AllIndividual") {
                $(".AllIndividual").show();
                $(".AllYearly").hide();
            }
            else if ($("#ContentPlaceHolder1_ddlReportType").val() == "YearlyLeave") {
                $(".AllIndividual").hide();
                $(".AllYearly").show();
            }

            if ($("#<%=ddlEmployee.ClientID %>").val() == "0")
                $("#employeeSearchSection").hide();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#<%= ddlEmployee.ClientID %>").change(function () {
                if ($(this).val() == "0") {
                    $("#employeeSearchSection").hide();
                    $("#DepartmentInfoDiv").show();
                }
                else if ($(this).val() == "1") {
                    $("#employeeSearchSection").show();
                    $("#DepartmentInfoDiv").hide();
                }
            });
            var empType = $("#<%= ddlEmployee.ClientID %>").val();
            if (empType == "0") {
                $("#DepartmentInfoDiv").show();
            }
            else if (empType == "1") {
                $("#DepartmentInfoDiv").hide();
            }
            $("#<%= ddlDepartmentId.ClientID %>").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#<%= ddlReportType.ClientID %>").change(function () {
                if ($(this).val() == "AllIndividual") {
                    $(".AllIndividual").show();
                    $(".AllYearly").hide();
                }
                else if ($(this).val() == "YearlyLeave") {
                    $(".AllIndividual").hide();
                    $(".AllYearly").show();
                }
            });

        });
        function CheckValidity() {
            var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            $("#ContentPlaceHolder1_hfCompanyId").val(company);
            $("#ContentPlaceHolder1_hfProjectId").val(project);
            $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
            $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());

            var empType = $("#<%= ddlEmployee.ClientID %>").val();
            var employeeId = $("#ContentPlaceHolder1_employeeeSearch_txtSearchEmployee").val();
            var employeeName = $("#ContentPlaceHolder1_employeeeSearch_txtEmployeeName").val();
            $("#ContentPlaceHolder1_hfEmpId").val(employeeId);
            $("#ContentPlaceHolder1_hfEmployeeName").val(employeeName);
            if (empType == "1") {
                var empId = $("#ContentPlaceHolder1_employeeeSearch_hfEmployeeId").val();
                if (empId == "0") {
                    toastr.warning("Please Select an Employee");
                    return false;
                }

            }
            return true;
        }
        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
            DptShowHide();
        }

        function WorkAfterSearchEmployee() { }

    </script>
    <asp:HiddenField ID="hfEmpId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfEmployeeName" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyName" runat="server" Value="0" />
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0" />
    <asp:HiddenField ID="hfProjectName" runat="server" Value="0" />
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">Leave Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label id="lbl0001" class="control-label">
                            Report Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="All / Individual" Value="AllIndividual"></asp:ListItem>
                            <asp:ListItem Text="Yearly Leave" Value="YearlyLeave"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="AllYearly">
                        <div class="col-md-2">
                            <asp:Label ID="lblYear" runat="server" class="control-label" Text="Year"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group AllIndividual">
                    <div>
                        <div class="col-md-12">
                            <UserControl:companyProjectUserControl ID="companyProjectUserControl" runat="server" />
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label id="lblEmployee" class="control-label">
                            Employee Type</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0" Text="--- All Employee ----"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Individual Employee"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="employeeSearchSection">
                    <UserControl:EmployeeSearch runat="server" ID="employeeeSearch" />
                </div>
                <div class="form-group" id="DepartmentInfoDiv">
                    <div class="col-md-2">
                        <asp:Label ID="lblDepartmentId" runat="server" class="control-label" Text="Department"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group AllIndividual">
                    <div class="col-md-2">
                        <asp:Label ID="lblLeaveMode" runat="server" class="control-label" Text="Leave Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLeaveTypeId" runat="server" CssClass="form-control"
                            TabIndex="4">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary"
                            OnClick="btnGenerate_Click" OnClientClick="javascript:return CheckValidity()" />
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
        <div class="panel-heading">Leave Information</div>
        <div class="panel-body">
            <div style="width: 1005px; overflow-x: scroll;">
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
