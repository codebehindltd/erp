<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmAccountManager.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmAccountManager" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithBasicInfo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Salary Formula</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlDepartmentId").change(function () {

                var departmentId = $(this).val();
                if (departmentId == "0") { toastr.warning("Please Select Department"); return; }

                PageMethods.GetEmployeeByDepartmentWise(departmentId, OnEmployeeBasicSalarySucceeded, OnEmployeeBasicSalaryFailed);
            });

        });

        function OnEmployeeBasicSalarySucceeded(result) {
            $("#EmployeeSalaryBasicContainer").html(result);
        }
        function OnEmployeeBasicSalaryFailed() {

        }

        function SaveEmployeeNGradeWiseBasicSalary() {

            var empid = '0', acountManagerId = "0", gridLength = 0, tr = "", row = 0, isSeelct = false, departmentId = "0";
            var addedManager = new Array(), deletedManager = new Array();

            departmentId = $("#ContentPlaceHolder1_ddlDepartmentId").val();

            if (departmentId == "0") {
                toastr.info("Please Select Department");
                return false;
            }
            //else if ($("#EmployeeWiseList tbody tr").find("td:eq(2)").find("input").is(":checked") == false) {
            //    toastr.info("Please Select Employee");
            //    return false;
            //}

            gridLength = $("#EmployeeWiseList tbody tr").length;

            for (row = 0; row < gridLength; row++) {

                tr = $("#EmployeeWiseList tbody tr:eq(" + row + ")");
                empid = $.trim($(tr).find("td:eq(0)").text());
                acountManagerId = $.trim($(tr).find("td:eq(1)").text());
                isSelect = $(tr).find("td:eq(2)").find("input").is(":checked");

                if (isSelect && acountManagerId == "0") {
                    addedManager.push({
                        AccountManagerId: "0",
                        DepartmentId: departmentId,
                        EmpId: empid,
                        SortName: ''
                    });
                }
                else if (!isSelect && acountManagerId != "0") {
                    deletedManager.push({
                        AccountManagerId: acountManagerId,
                        DepartmentId: departmentId,
                        EmpId: empid,
                        SortName: ''
                    });
                }
            }

            PageMethods.SaveUpdateAccountManager(addedManager, deletedManager, OnUpdateEmployeeBasicSalarySucceeded, OnUpdateEmployeeBasicSalaryFailed);
            return false;
        }

        function OnUpdateEmployeeBasicSalarySucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ResetForm();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnUpdateEmployeeBasicSalaryFailed() {

        }


        function ResetForm() {           
            $("#EmployeeWiseList  tbody").html("");
            $("#ContentPlaceHolder1_ddlDepartmentId").val("0");
            $("#form1")[0].reset();
        }

    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>

    <div class="panel panel-default">
        <div class="panel-heading">Emploee Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div id="EmplyeeWiseSalaryBasic">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label2" runat="server" class="control-label" Text="Department"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" style="margin: 10px 5px 0 5px;">
                        <div id="EmployeeSalaryBasicContainer">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                            TabIndex="10" OnClientClick="javascript:return SaveEmployeeNGradeWiseBasicSalary();" />
                    </div>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
