<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpGratuityApproval.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpGratuityApproval" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var minCheckInDate = "";
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Gratuity</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Gratuity Approval</li>";
            var breadCrumbs = moduleName + formName;
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            if (IsCanSave) {
                $('#btnApproveGratuity').show();
            } else {
                $('#btnApproveGratuity').hide();
            }

            minCheckInDate = $("#<%=hfMinCheckInDate.ClientID %>").val();

            //$("#btnApproveGratuity").hide();

            $("#btnSearch").click(function () {
                employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
                PageMethods.GetEmpForGratuityApproval(employeeId, OnSearchSucceeded, OnSearchFailed);
                return false;
            });
            $("#btnApproveGratuity").click(function () {
                ApproveGratuity();
            });

            $("#ContentPlaceHolder1_txtGratuityAppliDate").datepicker({
                changeMonth: true,
                changeYear: true,
                minDate: minCheckInDate,
                dateFormat: innBoarDateFormat
            });
        });

        function OnSearchSucceeded(result) {
            $("#basicinfo").show();
            if (result.GratuityEligibilityDate != null) {
                $("#btnApproveGratuity").hide();
                toastr.info("Gratuity already approved for this employee.");
            }
            else if (result.ProbableGratuityEligibilityDate > new Date()) {
                $("#btnApproveGratuity").hide();
                toastr.info("This employee yet not eligibile for gratuity.");
            }
            else {
                $("#btnApproveGratuity").show();
            }
            $("#<%=txtDepartment.ClientID %>").val(result.Department);
            $("#<%=txtDesignation.ClientID %>").val(result.Designation);
            $("#<%=txtGrade.ClientID %>").val(result.GradeName);
            $("#<%=txtGratuityAppliDate.ClientID %>").val(GetStringFromDateTime(result.ProbableGratuityEligibilityDate));
        }
        function OnSearchFailed(error) {

        }

        function ApproveGratuity() {
            employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            var graEligibleDate = $("#<%=txtGratuityAppliDate.ClientID %>").val();
            PageMethods.ApproveGratuity(employeeId, graEligibleDate, OnSaveSucceeded, OnSaveFailed);
            return false;
        }

        function OnSaveSucceeded(data) {
            CommonHelper.AlertMessage(data.AlertMessage);
            //toastr.success("Gratuity approved successfully for the selected employee.");
        }

        function OnSaveFailed(data) {
            CommonHelper.AlertMessage(data.AlertMessage);
            //toastr.success("Please provide correct Information.");
        }

        function WorkAfterSearchEmployee() {

        }


    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfMinCheckInDate" runat="server" />
    <div id="" class="panel panel-default">
        <div class="panel-heading">
            Search Employee for
            Gratuity Approval
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
                            <asp:Label ID="lblGratuityAppliDate" runat="server" class="control-label" Text="Gratuity Applicable Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtGratuityAppliDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary">
                            Search</button>
                        <button type="button" id="btnApproveGratuity" class="TransactionalButton btn btn-primary">
                            Approve Gratuity</button>
                        <%-- <asp:Button ID="btnApproveGratuity" runat="server" Text="Approve Gratuity" CssClass="TransactionalButton btn btn-primary"
                    OnClientClick="return ApproveGratuity();" />--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
