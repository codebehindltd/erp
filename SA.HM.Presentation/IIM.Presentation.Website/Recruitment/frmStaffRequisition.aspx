<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmStaffRequisition.aspx.cs" Inherits="HotelManagement.Presentation.Website.Recruitment.frmStaffRequisition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var editedItem = "";
        var DeletedStaffRequisitionDetails = new Array();
        var _fromDate = "", _toDate = "";
        var dt = new Date();
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Staff Requisition</li>";
            var breadCrumbs = moduleName + formName;
            
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#myTabs").tabs();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#ContentPlaceHolder1_ddlFiscalYearSetup").change(function () {
                var _id = $("#ContentPlaceHolder1_ddlFiscalYearSetup").val();
                PageMethods.GetFiscalYearInfo(_id, OnFiscalYearLoadSucceed, OnFiscalYearLoadFailed);
                return false;
            });
             $("#ContentPlaceHolder1_txtDemandedTime").datepicker({
                changeMonth: true,
                 changeYear: true,
                 minDate: dt,
                //maxDate: _toDate,
                dateFormat: innBoarDateFormat,
            });

            $("#btnAddRequsition").click(function () {

                var staffRequisitionDetailsId = $("#ContentPlaceHolder1_hfStaffRequisitionDetailsId").val();

                var departmentId = $("#ContentPlaceHolder1_ddlDepartments").val();
                var jobTypeId = $("#ContentPlaceHolder1_ddlJobType").val();
                var jobType = $("#ContentPlaceHolder1_ddlJobType option:selected").text();
                var jobLevel = $("#ContentPlaceHolder1_ddlJobLevel").val();
                var requisitionQuantity = $("#ContentPlaceHolder1_txtRequisitionQuantity").val();
                var salaryAmount = $("#ContentPlaceHolder1_txtSalaryAmount").val();
                var demandDate = $("#ContentPlaceHolder1_txtDemandedTime").val();
                var fiscalYear = $("#ContentPlaceHolder1_ddlFiscalYearSetup option:selected").text();
                var fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYearSetup").val();

                if (departmentId == 0) {
                    toastr.warning("Please Select Department.");
                    return;
                }
                else if (jobTypeId == 0) {
                    toastr.warning("Please Select Job Type.");
                    return;
                }
                else if (jobLevel == 0) {
                    toastr.warning("Please Select Job Level.");
                    return;
                }
                else if (requisitionQuantity == "") {
                    toastr.warning("Please Provide Requisition Quantity.");
                    return;
                }
                else if (salaryAmount == "") {
                    toastr.warning("Please Provide Salary Amount.");
                    return;
                }
                else if (demandDate == "") {
                    toastr.warning("Please Provide Demand Date.");
                    return;
                }
                else if (fiscalYearId == 0) {
                    toastr.warning("Please Select Fiscal Year.");
                    return;
                }

                if (editedItem != "") {
                    EditItem(staffRequisitionDetailsId, jobTypeId, jobType, jobLevel, requisitionQuantity, salaryAmount);
                    return;
                }

                AddStaffingBudget(staffRequisitionDetailsId, jobTypeId, jobType, jobLevel, requisitionQuantity, salaryAmount, departmentId,fiscalYear, fiscalYearId,);

                $("#ContentPlaceHolder1_ddlJobType").val("0");
                $("#ContentPlaceHolder1_ddlJobLevel").val("Entry");
                $("#ContentPlaceHolder1_txtRequisitionQuantity").val("");
                $("#ContentPlaceHolder1_txtSalaryAmount").val("");
                //$("#ContentPlaceHolder1_txtDemandedTime").val("");
                $("#ContentPlaceHolder1_ddlDepartments").prop('disabled', true);

            });

            $("#btnClearRequisition").click(function () {
                $("#ContentPlaceHolder1_ddlDepartments").val("0");
                $("#ContentPlaceHolder1_ddlJobType").val("0");
                $("#ContentPlaceHolder1_ddlJobLevel").val("Entry");
                $("#ContentPlaceHolder1_txtRequisitionQuantity").val("");
                $("#ContentPlaceHolder1_txtSalaryAmount").val("");
                $("#ContentPlaceHolder1_txtNoOfStaff").val("");
                $("#ContentPlaceHolder1_txtBudgetAmount").val("");
                $("#ContentPlaceHolder1_ddlFiscalYearSetup").val("0");
            });

            $("#ContentPlaceHolder1_ddlDepartments").change(function () {
                LoadBudgetInformation();
            });

            $("#ContentPlaceHolder1_ddlJobType").change(function () {
                LoadBudgetInformation();
            });

            $("#ContentPlaceHolder1_ddlJobLevel").change(function () {
                LoadBudgetInformation();
            });

        });

        function LoadBudgetInformation() {
            var departmentId = $("#ContentPlaceHolder1_ddlDepartments").val();
            var jobTypeId = $("#ContentPlaceHolder1_ddlJobType").val();
            var jobLevel = $("#ContentPlaceHolder1_ddlJobLevel").val();
            PageMethods.LoadStaffingBudget(departmentId, jobTypeId, jobLevel, OnStaffingBudgetLoadSucceed, OnStaffingBudgetLoadFailed);
        }

        function OnStaffingBudgetLoadSucceed(result) {
            if (result != null) {
                $("#ContentPlaceHolder1_txtNoOfStaff").val(result.NoOfStaff);
                $("#ContentPlaceHolder1_txtBudgetAmount").val(result.BudgetAmount);
            }
            else {
                $("#ContentPlaceHolder1_txtNoOfStaff").val("");
                $("#ContentPlaceHolder1_txtBudgetAmount").val("");
            }
        }
        function OnStaffingBudgetLoadFailed(error) {

        }

        function AddStaffingBudget(staffRequisitionDetailsId, jobTypeId, jobType, jobLevel, requisitionQuantity, salaryAmount, departmentId,fiscalYear, fiscalYearId,) {

            var isEdited = "0";
            var rowLength = $("#RequsitionGrid tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr = "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr = "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:18%;'>" + jobType + "</td>";
            tr += "<td style='width:18%;'>" + jobLevel + "</td>";
            tr += "<td style='width:18%;'>" + fiscalYear + "</td>";
            tr += "<td style='width:18%;'>" + requisitionQuantity + "</td>";
            tr += "<td style='width:18%;'>" + salaryAmount + "</td>";
            tr += "<td style='width:10%;'> <a onclick=\"javascript:return FIllForEdit(this);\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
            tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteRequsition(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            tr += "<td style='display:none'>" + staffRequisitionDetailsId + "</td>";
            tr += "<td style='display:none'>" + jobTypeId + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "<td style='display:none'>" + departmentId + "</td>";
            tr += "<td style='display:none'>" + fiscalYearId + "</td>";

            tr += "</tr>";

            $("#RequsitionGrid tbody").append(tr);

            tr = "";
        }

        function FIllForEdit(editItem) {

            editedItem = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAddRequsition").val("Update Requsition");

            var jobTypeId = $(tr).find("td:eq(7)").text();
            var jobLevel = $(tr).find("td:eq(1)").text();
            var noOfStaff = $(tr).find("td:eq(3)").text();
            var budgetAmount = $(tr).find("td:eq(4)").text();
            var fiscalYearId = $(tr).find("td:eq(10)").text();
            var departmentId = $(tr).find("td:eq(9)").text();

            $("#ContentPlaceHolder1_ddlJobType").val(jobTypeId);
            $("#ContentPlaceHolder1_ddlJobLevel").val(jobLevel);
            $("#ContentPlaceHolder1_txtRequisitionQuantity").val(noOfStaff);
            $("#ContentPlaceHolder1_txtSalaryAmount").val(budgetAmount);
            $("#ContentPlaceHolder1_ddlDepartments").prop('disabled', false);
            $("#ContentPlaceHolder1_ddlFiscalYearSetup").val(fiscalYearId);
            $("#ContentPlaceHolder1_ddlDepartments").val(departmentId);
        }
        function DeleteRequsition(deleteItem) {
            $(deleteItem).parent().parent().remove();
        }

        function EditItem(staffingBudgetDetailstId, jobTypeId, jobType, jobLevel, noOfStaff, budgetAmount) {

            $(editedItem).find("td:eq(5)").text(staffingBudgetDetailstId);
            $(editedItem).find("td:eq(6)").text(jobTypeId);

            $(editedItem).find("td:eq(0)").text(jobType);
            $(editedItem).find("td:eq(1)").text(jobLevel);

            $(editedItem).find("td:eq(2)").text(noOfStaff);
            $(editedItem).find("td:eq(3)").text(budgetAmount);


            if (staffingBudgetDetailstId != "0")
                $(editedItem).find("td:eq(7)").text("1");

            $("#ContentPlaceHolder1_ddlJobType").val("0");
            $("#ContentPlaceHolder1_ddlJobLevel").val("Entry");
            $("#ContentPlaceHolder1_txtRequisitionQuantity").val("");
            $("#ContentPlaceHolder1_txtSalaryAmount").val("");

            $("#btnAddRequsition").val("Add Requsition");

            editedItem = "";
        }

        function SaveUpdate() {
            
            if ($("#ContentPlaceHolder1_ddlDepartments").val() == "0") {
                toastr.info("Please Select Department.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtDemandedTime").val() == "") {
                toastr.info("Please Provide Demand Date.");
                return false;
            }
            else if ($("#RequsitionGrid tbody tr").length == 0) {
                toastr.info("Please Add Requisition.");
                return false;
            }

            var departmentId = $("#ContentPlaceHolder1_ddlDepartments").val();
            var demandedDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtDemandedTime").val(), innBoarDateFormat);
            var staffRequisitionId = $("#ContentPlaceHolder1_hfStaffRequisitionId").val();

            if (staffRequisitionId == "")
                staffRequisitionId = "0";

            var StaffRequisition = {
                StaffRequisitionId: staffRequisitionId,
                DepartmentId: departmentId
            };

            var staffRequisitionDetailsId = '', jobType = '', jobLevel = '', requisitionQuantity = '', salaryAmount = '', isEdited = "0";
            var NewlyAddedStaffRequisitionDetails = new Array(), EditedStaffRequisitionDetails = new Array();

            $("#RequsitionGrid tbody tr").each(function () {

                staffRequisitionDetailsId = $(this).find("td:eq(6)").text();
                jobType = $(this).find("td:eq(7)").text();
                jobLevel = $(this).find("td:eq(1)").text();
                requisitionQuantity = $(this).find("td:eq(3)").text();
                salaryAmount = $(this).find("td:eq(4)").text();
                isEdited = $(this).find("td:eq(8)").text();
                fiscalYearId = $(this).find("td:eq(10)").text();
                if (staffRequisitionDetailsId == "")
                    staffRequisitionDetailsId = "0";

                if ((staffRequisitionDetailsId == "0")) {

                    NewlyAddedStaffRequisitionDetails.push({
                        StaffRequisitionDetailsId: staffRequisitionDetailsId,
                        StaffRequisitionId: staffRequisitionId,
                        JobType: jobType,
                        JobLevel: jobLevel,
                        RequisitionQuantity: requisitionQuantity,
                        SalaryAmount: salaryAmount,
                        DemandDate: demandedDate,
                        FiscalYear: fiscalYearId
                    });
                }
                else if (staffRequisitionDetailsId != "0") {
                    EditedStaffRequisitionDetails.push({
                        StaffRequisitionDetailsId: staffRequisitionDetailsId,
                        StaffRequisitionId: staffRequisitionId,
                        JobType: jobType,
                        JobLevel: jobLevel,
                        RequisitionQuantity: requisitionQuantity,
                        SalaryAmount: salaryAmount,
                        DemandDate: demandedDate,
                        FiscalYear: fiscalYearId
                    });
                }

            });
            PageMethods.SaveStaffRequisition(staffRequisitionId, StaffRequisition, NewlyAddedStaffRequisitionDetails, EditedStaffRequisitionDetails, DeletedStaffRequisitionDetails, OnSaveStaffBudgetSucceed, OnSaveStaffBudgetFailed);

            return false;
        }
        function OnSaveStaffBudgetSucceed(result) {
            
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSaveStaffBudgetFailed(error) {

            toastr.info("Please Contact With Admin.");
        }

        function JobCircularDetails(jobCircularId) {

            PageMethods.GetJobCircularDetails(jobCircularId, OnJobCircularLoadSucceeded, OnJobCircularLoadFailed);

            return false;
        }

        function OnJobCircularLoadSucceeded(result) {
            $("#jobTitle").text(result.JobTitle);

            $("#ContentPlaceHolder1_ddlDepartments").val(result.DepartmentId + "");
            var department = $("#ContentPlaceHolder1_ddlDepartments option:selected").text();

            $("#department").text(department);
            $("#jobType").text(result.JobType);
            $("#jobLevel").text(result.JobLevel);
            $("#noofVacancies").text(result.NoOfVancancie);
            $("#demandDate").text(GetStringFromDateTime(result.DemandedTime));
            $("#ageRangeFrom").text(result.AgeRangeFrom);
            $("#ageRangeTo").text(result.AgeRangeTo);
            $("#gender").text(result.Gender);
            $("#yearofExperiance").text(result.YearOfExperiance);
            $("#jobDescription").text(result.JobDescription);
            $("#educationalQualification").text(result.EducationalQualification);
            $("#addditionalJobRequirement").text(result.AdditionalJobRequirement);

            $("#ContentPlaceHolder1_ddlDepartments").val("0");

            $("#CircularDetailsContainer").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 800,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Job Circular Details",
                show: 'slide'
            });
        }

        function OnJobCircularLoadFailed(error) { }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_ddlJobType").val("0");
            $("#ContentPlaceHolder1_ddlJobLevel").val("Entry");
            $("#ContentPlaceHolder1_txtRequisitionQuantity").val("");
            $("#ContentPlaceHolder1_txtSalaryAmount").val("");
            $("#ContentPlaceHolder1_ddlDepartments").val("0");
            $("#ContentPlaceHolder1_hfStaffRequisitionId").val("");
            $("#ContentPlaceHolder1_hfStaffRequisitionDetailsId").val("");
            $("#ContentPlaceHolder1_txtDemandedTime").val("");
            $("#ContentPlaceHolder1_btnSave").val("Save");
            $("#ContentPlaceHolder1_ddlDepartments").prop('disabled', false);
            $("#RequsitionGrid tbody").html("");

        }
        function OnFiscalYearLoadSucceed(result) {

            _fromDate = result.ReportFromDate;
            _toDate = result.ReportToDate;
            $("#ContentPlaceHolder1_txtDemandedTime").datepicker("option", "maxDate", _toDate);
            $("#ContentPlaceHolder1_txtDemandedTime").datepicker("option", "minDate", _fromDate);
           
            //return false;
        }
        function OnFiscalYearLoadFailed() {
        }
    </script>
    <asp:HiddenField ID="hfStaffRequisitionId" runat="server" Value="" />
    <asp:HiddenField ID="hfStaffRequisitionDetailsId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsCurrentOrPreviousPage" runat="server" Value="" />
    <div id="CircularDetailsContainer" style="display: none;">
        <table class="table table-bordered table-condensed table-responsive" id="DetailsJobCircularGrid"
            style="width: 100%;">
            <tbody>
                <tr style='background-color: #E3EAEB;'>
                    <td style="font-weight: bold; width: 70px;">Department:
                    </td>
                    <td colspan="3" id="department" style="width: 100px;"></td>
                </tr>
                <tr style='background-color: #F7F7F7;'>
                    <td style="font-weight: bold; width: 70px;">Job Type:
                    </td>
                    <td id="jobType" style="width: 70px;"></td>
                    <td style="font-weight: bold; width: 70px;">Job Level:
                    </td>
                    <td id="jobLevel" style="width: 70px;"></td>
                </tr>
                <tr style='background-color: #E3EAEB;'>
                    <td style="font-weight: bold; width: 70px;">No Of Vacancies:
                    </td>
                    <td id="noofVacancies" style="width: 70px;"></td>
                    <td style="font-weight: bold; width: 70px;">Budget Amount:
                    </td>
                    <td id="demandDate" style="width: 70px;"></td>
                </tr>
                <tr style='background-color: #F7F7F7;'>
                    <td style="font-weight: bold; width: 70px;">Age Range From:
                    </td>
                    <td id="ageRangeFrom" style="width: 70px;"></td>
                    <td style="font-weight: bold; width: 70px;">Age Range To:
                    </td>
                    <td id="ageRangeTo" style="width: 70px;"></td>
                </tr>
                <tr style='background-color: #E3EAEB;'>
                    <td style="font-weight: bold; width: 70px;">Gender:
                    </td>
                    <td id="gender" style="width: 70px;"></td>
                    <td style="font-weight: bold; width: 70px;">Year Of Experiance:
                    </td>
                    <td id="yearofExperiance" style="width: 70px;"></td>
                </tr>
                <tr style='background-color: #F7F7F7;'>
                    <td style="font-weight: bold; width: 70px;">Job Description:
                    </td>
                    <td id="jobDescription" colspan="3"></td>
                </tr>
                <tr style='background-color: #E3EAEB;'>
                    <td style="font-weight: bold; width: 100px;">Educational Qualification:
                    </td>
                    <td id="educationalQualification" colspan="3" style="width: 100px;"></td>
                </tr>
                <tr style='background-color: #F7F7F7;'>
                    <td style="font-weight: bold; width: 100px;">Additional Job Requirement:
                    </td>
                    <td id="addditionalJobRequirement" colspan="3" style="width: 100px;"></td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="circularEntry" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-1">Staff Requisition</a></li>
            <li id="circularSearch" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-2">Search Staff Requisition</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDepartments" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Requisition Details
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblLoanTakenForMonthOrYear" runat="server" class="control-label required-field"
                                        Text="Job Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblJobLevel" runat="server" class="control-label required-field" Text="Job Level"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlJobLevel" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="Entry" Text="Entry"></asp:ListItem>
                                        <asp:ListItem Value="Mid" Text="Mid"></asp:ListItem>
                                        <asp:ListItem Value="Top" Text="Top"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNoOfVancancie" runat="server" class="control-label" Text="Staff Requised"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNoOfStaff" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lbl1001" runat="server" class="control-label" Text="Budget Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtBudgetAmount" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Requisition Quantity"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtRequisitionQuantity" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Salary Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSalaryAmount" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblFiscalYearSetup" runat="server" class="control-label required-field" Text="Fiscal Year:"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlFiscalYearSetup" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblDemandedTime" runat="server" class="control-label required-field"
                                        Text="Demand Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDemandedTime" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="HMContainerRowButton">
                                <input id="btnAddRequsition" type="button" class="TransactionalButton btn btn-primary"
                                    value="Add Requisition" />
                                <input id="btnClearRequisition" type="button" class="TransactionalButton btn btn-primary"
                                    value="Clear" />
                            </div>
                            <div class="form-group" id="RequsitionTableContainer" style="overflow: scroll;">
                                <table class="table table-bordered table-condensed table-responsive" id="RequsitionGrid"
                                    style="width: 100%;">
                                    <thead>
                                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                            <th style="width: 18%;">Job Type
                                            </th>
                                            <th style="width: 18%;">Job Level
                                            </th>
                                            <th style="width: 18%;">Fiscal Year
                                            </th>
                                            <th style="width: 18%;">Requisition Quantity
                                            </th>
                                            <th style="width: 18%;">Salary Amount
                                            </th>
                                            <th style="width: 18%;">Action
                                            </th>
                                            <th style="display: none">StaffRequisitionDetailsId
                                            </th>
                                            <th style="display: none">JobLevelId
                                            </th>
                                            <th style="display: none">Is Edited
                                            </th>
                                            <th style="display: none">DepartmentId
                                            </th>
                                            <th style="display: none">FiscalYearID
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                        OnClientClick="return SaveUpdate();" />
                                    <%-- <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary" />--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Staff Requisition
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lbl101901" runat="server" class="control-label" Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchDepartment" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblFiscalYear" runat="server" class="control-label" Text="Fiscal Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvStaffRequisition" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" ForeColor="#333333"
                        OnRowDataBound="gvStaffRequisition_RowDataBound" OnRowCommand="gvStaffRequisition_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("StaffRequisitionId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Department" HeaderText="Department" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="JobTypeName" HeaderText="Job Type" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="JobLevel" HeaderText="Job Level" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RequisitionQuantity" HeaderText="Req. Qty." ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SalaryAmount" HeaderText="Salary" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                        CommandArgument='<%# bind("StaffRequisitionId") %>' ImageUrl="~/Images/approved.png"
                                        Text="" AlternateText="Approve Requisition" ToolTip="Approve Requisition" OnClientClick="return confirm('Do you want to Approve?');" />
                                    &nbsp;&nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False"
                                        CommandName="CmdDelete" CommandArgument='<%# bind("StaffRequisitionId") %>' ImageUrl="~/Images/cancel.png"
                                        Text="" AlternateText="Cancel Requisition" ToolTip="Cancel Requisition" OnClientClick="return confirm('Do you want to Cancel Item?');" />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("StaffRequisitionId") %>' ImageUrl="~/Images/edit.png"
                                        Text="" AlternateText="Edit Requisition" ToolTip="Edit Requisition" OnClientClick="return confirm('Do you want to Edit?');" />
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApprovedJobCircular"
                                        CommandArgument='<%# bind("JobCircularId") %>' OnClientClick='<%#String.Format("return JobCircular({0})", Eval("JobCircularId")) %>'
                                        ImageUrl="~/Images/approved.png" Text="" AlternateText="Details" ToolTip="Approve Received Item" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdJobCircularCancel"
                                        CommandArgument='<%# bind("JobCircularId") %>' OnClientClick='<%#String.Format("return CancelJobCircualr({0})", Eval("JobCircularId")) %>'
                                        ImageUrl="~/Images/cancel.png" Text="" AlternateText="Details" ToolTip="Cancel Received Item" />
                                    &nbsp;<asp:ImageButton ID="ImgDetailsRequisition" runat="server" CausesValidation="False"
                                        CommandName="CmdJobCircularDetails" CommandArgument='<%# bind("JobCircularId") %>'
                                        OnClientClick='<%#String.Format("return JobCircularDetails({0})", Eval("JobCircularId")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Receive Details" />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("JobCircularId") %>' OnClientClick='<%#String.Format("return FillForm({0})", Eval("JobCircularId")) %>'
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>--%>
                        </Columns>
                        <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#7C6F57" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
