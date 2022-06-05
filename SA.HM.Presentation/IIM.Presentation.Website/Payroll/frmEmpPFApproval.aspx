<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpPFApproval.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpPFApproval" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Provident Fund</a>";
            var formName = "<span class='divider'>/</span><li class='active'>PF Approval</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#btnGeneratePFForm").hide();
            $("#btnApprovePF").hide();
            $("#btnTerminate").hide();
            $("#btnGeneratePFWithdrawalForm").hide();

            $("#ContentPlaceHolder1_employeeSearch_btnSrcEmployees").click(function () {
                $("#basicinfo").hide();
                $("#btnGeneratePFForm").hide();
                $("#btnTerminate").hide();
                $("#btnApprovePF").hide();
                return false;
            });

            $("#btnSearch").click(function () {
                EmpForPFApprovalProcess();                
            });

            $("#btnApprovePF").click(function () {
                ApprovePF();
            });

            $("#btnTerminate").click(function () {
                //ApprovePF();
                TerminatePF();
            });

            $("#btnGeneratePFWithdrawalForm").click(function () {
                GeneratePFApplicationForm("GeneratePFWithdrawal");
            });

            $("#btnGeneratePFForm").click(function () {
                GeneratePFApplicationForm("GeneratePFForm");
            });

            $("#ContentPlaceHolder1_txtPFAppliDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });

        function EmpForPFApprovalProcess()
        {
            employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            PageMethods.GetEmpForPFApproval(employeeId, OnSearchSucceeded, OnSearchFailed);
            return false;
        }

        function OnSearchSucceeded(result) {
            $("#basicinfo").show();
            $("#<%=txtDepartment.ClientID %>").val(result.Department);
            $("#<%=txtDesignation.ClientID %>").val(result.Designation);
            $("#<%=txtGrade.ClientID %>").val(result.GradeName);
            $("#<%=txtPFAppliDate.ClientID %>").val(GetStringFromDateTime(result.ProbablePFEligibilityDate));

            if (result.PFEligibilityDate == null) {
                //$("#btnGeneratePFForm").show();
                $("#btnGeneratePFForm").hide();
                $("#btnApprovePF").show();
                $("#btnTerminate").hide();
                $("#btnGeneratePFWithdrawalForm").hide();
            }
            else {
                $("#btnGeneratePFForm").hide();
                $("#btnApprovePF").hide();
                $("#btnTerminate").show();
                //$("#btnGeneratePFWithdrawalForm").show();
                $("#btnGeneratePFWithdrawalForm").hide();
            }

            //EmpForPFApprovalProcess();
        }

        function OnSearchFailed(error) {

        }

        function ApprovePF() {
            employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            var pfEligibleDate = $("#<%=txtPFAppliDate.ClientID %>").val();
            PageMethods.ApprovePF(employeeId, pfEligibleDate, OnSaveSucceeded, OnSaveFailed);
            return false;
        }

        function OnSaveSucceeded(data) {
            CommonHelper.AlertMessage(data.AlertMessage);
            //window.location = "frmEmpPFApproval.aspx";
            //toastr.success("Gratuity approved successfully for the selected employee.");
        }

        function OnSaveFailed(data) {
            CommonHelper.AlertMessage(data.AlertMessage);
            //toastr.success("Please provide correct Information.");
        }

        function WorkAfterSearchEmployee() {

        }

        function TerminatePF() {
            employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            PageMethods.TerminatePF(employeeId, OnTerminateSucceeded, OnTerminateFailed);
            return false;
        }

        function OnTerminateSucceeded(data) {
            CommonHelper.AlertMessage(data.AlertMessage);
            //window.location = "frmEmpPFApproval.aspx";
        }
        function OnTerminateFailed(data) {
            CommonHelper.AlertMessage(data.AlertMessage);
        }

        function GeneratePFApplicationForm(actionType) {

            var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            var pfDate = $("#<%=txtPFAppliDate.ClientID %>").val();

            if (actionType == "GeneratePFForm") {
                var type = "application";
                var url = "/Payroll/Reports/frmReportPFApplication.aspx?EmployeeId=" + employeeId + "," + pfDate + "," + type;
            }
            if (actionType == "GeneratePFWithdrawal") {
                var type = "withdrawal";
                var url = "/Payroll/Reports/frmReportPFApplication.aspx?EmployeeId=" + employeeId + "," + pfDate + "," + type;
            }

            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=760,height=780,left=300,top=50,resizable=yes");
        }
    </script>
    <div id="" class="panel panel-default">
        <div class="panel-heading">
            Search Employee for
            PF Approval
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />
                <div id="basicinfo" style="display: none">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblDepartment" runat="server" class="control-label" Text="Department"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblDesignation" runat="server" class="control-label" Text="Employee Designation"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblGrade" runat="server" class="control-label" Text="Grade"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtGrade" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblPFAppliDate" runat="server" class="control-label" Text="PF Applicable Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtPFAppliDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary">
                            Search</button>
                        <button type="button" id="btnGeneratePFForm" style="display:none;" class="TransactionalButton btn btn-primary">
                            Generate PF Application Form</button>
                        <button type="button" id="btnGeneratePFWithdrawalForm" style="display:none;" class="TransactionalButton btn btn-primary">
                            Generate PF Withdrawal Form</button>
                        <button type="button" id="btnApprovePF" class="TransactionalButton btn btn-primary">
                            Approve PF</button>
                        <button type="button" id="btnTerminate" class="TransactionalButton btn btn-primary">
                            Terminate</button>
                        <%--<asp:Button ID="btnGeneratePFForm" runat="server" TabIndex="3" Text="Generate PF Application Form"
                    CssClass="TransactionalButton btn btn-primary" OnClick="btnGeneratePFForm_Click" />
                <asp:Button ID="btnApproveGratuity" runat="server" Text="Approve PF" CssClass="TransactionalButton btn btn-primary"
                    OnClientClick="return ApprovePF();" />
                <asp:Button ID="btnTerminate" runat="server" TabIndex="3" Text="Terminate" CssClass="TransactionalButton btn btn-primary"
                    OnClick="btnTerminate_Click" />--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
