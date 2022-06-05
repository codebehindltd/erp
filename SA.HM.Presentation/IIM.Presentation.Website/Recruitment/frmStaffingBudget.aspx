<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmStaffingBudget.aspx.cs" Inherits="HotelManagement.Presentation.Website.Recruitment.frmStaffingBudget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var editedItem = "";
        var deleteDbItem = new Array();

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Staffing Budget</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#myTabs").tabs();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtDemandedTime").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#btnAddRequsition").click(function () {

                var staffingBudgetDetailstId = "0";

                var departments = $("#ContentPlaceHolder1_ddlDepartments").val();
                var jobTypeId = $("#ContentPlaceHolder1_ddlJobType").val();
                var jobType = $("#ContentPlaceHolder1_ddlJobType option:selected").text();
                var jobLevel = $("#ContentPlaceHolder1_ddlJobLevel").val();
                var noOfStaff = $("#ContentPlaceHolder1_txtNoOfStaff").val();
                var budgetAmount = $("#ContentPlaceHolder1_txtBudgetAmount").val();
                var fiscalYear = $("#ContentPlaceHolder1_ddlFiscalYearSetup option:selected").text();
                var fiscalYearId = $("#ContentPlaceHolder1_ddlFiscalYearSetup").val();
                staffingBudgetDetailstId = $("#ContentPlaceHolder1_hfStaffingBudgetDetailsId").val();
                if (departments == 0) {
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
                else if (noOfStaff == "") {
                    toastr.warning("Please Provide Staff Number.");
                    return;
                }
                else if (budgetAmount == "") {
                    toastr.warning("Please Provide Budget Amount.");
                    return;
                }
                else if (fiscalYear == 0) {
                    toastr.warning("Please Select Fiscal Year.");
                    return;
                }
                
                if (editedItem != "") {
                    EditItem(staffingBudgetDetailstId, jobTypeId, jobType, jobLevel, noOfStaff, budgetAmount, fiscalYear, fiscalYearId, departments);
                    return;
                }

                AddStaffingBudget(staffingBudgetDetailstId, jobTypeId, jobType, jobLevel, noOfStaff, budgetAmount, fiscalYear, fiscalYearId, departments);

                $("#ContentPlaceHolder1_ddlJobType").val("0");
                $("#ContentPlaceHolder1_ddlJobLevel").val("Entry");
                $("#ContentPlaceHolder1_txtNoOfStaff").val("");
                $("#ContentPlaceHolder1_txtBudgetAmount").val("");
                $("#ContentPlaceHolder1_ddlDepartments").val(departments);

            });

            $("#btnClearRequisition").click(function () {
                $("#ContentPlaceHolder1_ddlDepartments").val("0");
                $("#ContentPlaceHolder1_ddlJobType").val("0");
                $("#ContentPlaceHolder1_ddlJobLevel").val("Entry");
                $("#ContentPlaceHolder1_txtNoOfStaff").val("");
                $("#ContentPlaceHolder1_txtBudgetAmount").val("");
                $("#ContentPlaceHolder1_ddlFiscalYearSetup").val("0");
            });

        });

        function AddStaffingBudget(staffingBudgetDetailstId, jobTypeId, jobType, jobLevel, noOfStaff, budgetAmount, fiscalYear, fiscalYearId, department) {
            var isEdited = "0";
            var rowLength = $("#BudgetGrid tbody tr").length;

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
            tr += "<td style='width:18%;'>" + noOfStaff + "</td>";
            tr += "<td style='width:18%;'>" + budgetAmount + "</td>";
            tr += "<td style='width:10%;'> <a onclick=\"javascript:return FIllForEdit(this);\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
            tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteRequsition(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            tr += "<td style='display:none'>" + staffingBudgetDetailstId + "</td>";
            tr += "<td style='display:none'>" + jobTypeId + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "<td style='display:none'>" + fiscalYearId + "</td>";
            tr += "<td style='display:none'>" + department + "</td>";


            tr += "</tr>";

            $("#BudgetGrid tbody").append(tr);

            tr = "";
        }

        function FIllForEdit(editItem) {
            //$(editItem).parent().parent().remove();
            editedItem = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAddRequsition").val("Update Requsition");

            var jobTypeId = $(tr).find("td:eq(7)").text();
            var jobLevel = $(tr).find("td:eq(1)").text();
            var noOfStaff = $(tr).find("td:eq(3)").text();
            var budgetAmount = $(tr).find("td:eq(4)").text();
            var fiscalYearId = $(tr).find("td:eq(9)").text();
            var department = $(tr).find("td:eq(10)").text();
            var staffingBudgetDetailsId = $(tr).find("td:eq(6)").text();

            $("#ContentPlaceHolder1_ddlJobType").val(jobTypeId);
            $("#ContentPlaceHolder1_ddlJobLevel").val(jobLevel);
            $("#ContentPlaceHolder1_txtNoOfStaff").val(noOfStaff);
            $("#ContentPlaceHolder1_txtBudgetAmount").val(budgetAmount);
            $("#ContentPlaceHolder1_ddlFiscalYearSetup").val(fiscalYearId);
            $("#ContentPlaceHolder1_ddlDepartments").val(department);
            $("#ContentPlaceHolder1_hfStaffingBudgetDetailsId").val(staffingBudgetDetailsId);
            $("#ContentPlaceHolder1_ddlDepartments").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlDepartments").val(department + '').trigger('change');

        }
        function DeleteRequsition(deleteItem) {
            $(deleteItem).parent().parent().remove();
            var staffingBudgetId = "0", staffingBudgetDetailsId = "0";
            var tr = $(deleteItem).parent().parent();
            staffingBudgetId =$("#ContentPlaceHolder1_hfStaffingBudgetId").val();
            staffingBudgetDetailsId = $(tr).find("td:eq(6)").text();
            if ((staffingBudgetDetailsId != "0")) {
                deleteDbItem.push({
                    StaffingBudgetDetailsId: staffingBudgetDetailsId,
                    StaffingBudgetId: staffingBudgetId
                });
            }

        }

        function EditItem(staffingBudgetDetailstId, jobTypeId, jobType, jobLevel, noOfStaff, budgetAmount, fiscalYear, fiscalYearId, departments)
        {

            $(editedItem).find("td:eq(0)").text(jobType);
            $(editedItem).find("td:eq(1)").text(jobLevel);

            $(editedItem).find("td:eq(2)").text(fiscalYear);
            $(editedItem).find("td:eq(3)").text(noOfStaff);

            $(editedItem).find("td:eq(4)").text(budgetAmount);
            $(editedItem).find("td:eq(6)").text(staffingBudgetDetailstId);
            $(editedItem).find("td:eq(7)").text(jobTypeId);

            if (staffingBudgetDetailstId != "0")
                $(editedItem).find("td:eq(8)").text("1");

            $(editedItem).find("td:eq(9)").text(fiscalYearId);
            $(editedItem).find("td:eq(10)").text(departments);

            

            $("#ContentPlaceHolder1_ddlJobType").val("0");
            $("#ContentPlaceHolder1_ddlJobLevel").val("Entry");
            $("#ContentPlaceHolder1_txtNoOfStaff").val("");
            $("#ContentPlaceHolder1_txtBudgetAmount").val("");

            $("#btnAddRequsition").val("Add Requsition");

            editedItem = "";
        }

        function SaveUpdate() {

            if ($("#ContentPlaceHolder1_ddlDepartments").val() == "0") {
                toastr.info("Please Select Department.");
                return false;
            }
            else if ($("#BudgetGrid tbody tr").length == 0) {
                toastr.info("Please Add Budget.");
                return false;
            }

            var departmentId = $("#ContentPlaceHolder1_ddlDepartments").val();
            var staffingBudgetId = $("#ContentPlaceHolder1_hfStaffingBudgetId").val();

            if (staffingBudgetId == "")
                staffingBudgetId = "0";
            
            var StaffingBudget = {
                StaffingBudgetId: staffingBudgetId,
                DepartmentId: departmentId
            };

            var staffingBudgetDetailstId = '', jobType = '', jobLevel = '', noOfStaff = '', budgetAmount = '', isEdited = "0", fiscalYear = "0";
            var NewlyAddedStaffingBudgetDetails = new Array(), EditedStaffingBudgetDetails = new Array();

            $("#BudgetGrid tbody tr").each(function () {

                staffingBudgetDetailstId = $(this).find("td:eq(6)").text();
                //var staffingBudgetId = $("#")
                jobType = $(this).find("td:eq(7)").text();
                jobLevel = $(this).find("td:eq(1)").text();
                noOfStaff = $(this).find("td:eq(3)").text();
                budgetAmount = $(this).find("td:eq(4)").text();
                isEdited = $(this).find("td:eq(8)").text();
                fiscalYear = $(this).find("td:eq(9)").text();

                if (staffingBudgetDetailstId == "0") {

                    NewlyAddedStaffingBudgetDetails.push({
                        StaffingBudgetDetailsId: staffingBudgetDetailstId,
                        StaffingBudgetId: staffingBudgetId,
                        JobType: jobType,
                        JobLevel: jobLevel,
                        NoOfStaff: noOfStaff,
                        BudgetAmount: budgetAmount,
                        FiscalYear: fiscalYear
                    });
                }
                else if (staffingBudgetDetailstId != "0" && isEdited != "0") {
                    EditedStaffingBudgetDetails.push({
                        StaffingBudgetDetailsId: staffingBudgetDetailstId,
                        StaffingBudgetId: staffingBudgetId,
                        JobType: jobType,
                        JobLevel: jobLevel,
                        NoOfStaff: noOfStaff,
                        BudgetAmount: budgetAmount,
                        FiscalYear: fiscalYear
                    });
                }
            });

            PageMethods.SaveStaffingBudget(staffingBudgetId, StaffingBudget, NewlyAddedStaffingBudgetDetails, EditedStaffingBudgetDetails, deleteDbItem, OnSaveStaffBudgetSucceed, OnSaveStaffBudgetFailed);

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
            $("#ContentPlaceHolder1_txtNoOfStaff").val("");
            $("#ContentPlaceHolder1_txtBudgetAmount").val("");
            $("#ContentPlaceHolder1_ddlDepartments").val("0");
            $("#ContentPlaceHolder1_hfStaffingBudgetId").val("");

            $("#BudgetGrid tbody").html("");
        }
        function FillFormEdit(StaffingBudgetId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }

            FillForm(StaffingBudgetId);
            return false;
        }
        function FillForm(StaffingBudgetId) {

            CommonHelper.SpinnerOpen();

            PageMethods.FillForm(StaffingBudgetId, OnFillFormSucceed, OnFillFormFailed);

            return false;
        }
        function OnFillFormSucceed(result) {
            if (result != null) {
                $("#<%=btnSave.ClientID %>").val("Update");
                $("#BudgetTableContainer tbody").html("");
                $("#ContentPlaceHolder1_hfStaffingBudgetId").val(result.StaffingBudget.StaffingBudgetId);
                $("#ContentPlaceHolder1_ddlDepartments").val(result.StaffingBudget.DepartmentId);

                //BudgetTableContainer
                var rowLength = result.StaffingBudgetDetails.length;
                var row = 0;
                for (row = 0; row < rowLength; row++) {

                    AddStaffingBudget(result.StaffingBudgetDetails[row].StaffingBudgetDetailsId,
                        result.StaffingBudgetDetails[row].JobType, result.StaffingBudgetDetails[row].JobTypeName,
                        result.StaffingBudgetDetails[row].JobLevel, result.StaffingBudgetDetails[row].NoOfStaff,
                        result.StaffingBudgetDetails[row].BudgetAmount, result.StaffingBudgetDetails[row].FiscalYearName,
                        result.StaffingBudgetDetails[row].FiscalYear, result.StaffingBudgetDetails[row].DepartmentId);

                }
                $("#myTabs").tabs({ active: 0 });
                CommonHelper.SpinnerClose();
            }
            //AddStaffingBudget
            //AddStaffingBudget(staffingBudgetDetailstId, jobTypeId, jobType, jobLevel, noOfStaff, budgetAmount,fiscalYear,fiscalYearId,department) {

        }
        function OnFillFormFailed() {

        }
        function CancelStaffingBudget(StaffingBudgetId) {
            if (!confirm("Do you want to delete?")) {
                return;
            }
            $(StaffingBudgetId).parent().parent().remove();
            PageMethods.CancelStaffingBudget(StaffingBudgetId, CancelStaffingBudgetSucceed, CancelStaffingBudgetFailed);
            return false;
        }
        function CancelStaffingBudgetSucceed(result) {

            $("#ContentPlaceHolder1_btnSearch").trigger("click");
            CommonHelper.AlertMessage(result.AlertMessage);
            return false;
        }
        function CancelStaffingBudgetFailed(error) {
            return false;
        }

    </script>
    <asp:HiddenField ID="hfStaffingBudgetId" runat="server" Value="0" />
    <asp:HiddenField ID="hfStaffingBudgetDetailsId" runat="server" Value="0" />
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
                <a href="#tab-1">Staffing Budget</a></li>
            <li id="circularSearch" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-2">Search Staffing Budget</a></li>
        </ul>
        <div id="tab-1">
            <div id="" class="panel panel-default">
                <div class="panel-heading">
                    Staffing Budget Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDepartments" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblFiscalYearSetup" runat="server" class="control-label required-field" Text="Fiscal Year:"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlFiscalYearSetup" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
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
                                <asp:Label ID="lblNoOfVancancie" runat="server" class="control-label required-field"
                                    Text="No Of Staff"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNoOfStaff" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lbl1001" runat="server" class="control-label required-field" Text="Budget Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBudgetAmount" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <input id="btnAddRequsition" type="button" class="TransactionalButton btn btn-primary"
                                value="Add Budget" />
                            <input id="btnClearRequisition" type="button" class="TransactionalButton btn btn-primary"
                                value="Clear" />
                        </div>
                        <div class="form-group" id="BudgetTableContainer" style="overflow: scroll;">
                            <table class="table table-bordered table-condensed table-responsive" id="BudgetGrid"
                                style="width: 100%;">
                                <thead>
                                    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                        <th style="width: 18%;">Job Type
                                        </th>
                                        <th style="width: 18%;">Job Level
                                        </th>
                                        <th style="width: 18%;">Fiscal Year
                                        </th>
                                        <th style="width: 18%;">No Of Staff
                                        </th>
                                        <th style="width: 18%;">Budget Amount
                                        </th>
                                        <th style="width: 10%;">Action
                                        </th>
                                        <th style="display: none">StaffingBudgeDetailstId
                                        </th>
                                        <th style="display: none">JobTypelId
                                        </th>
                                        <th style="display: none">Is Edited
                                        </th>
                                        <th style="display: none">FiscalYearId
                                        </th>
                                        <th style="display: none">Department
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
                                <%--<asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary" />--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Staff Budget Search
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
                    <asp:GridView ID="gvStaffBudget" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" ForeColor="#333333" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("StaffingBudgetId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblDep" runat="server" Text='<%#Eval("DepartmentId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DepartmentName" HeaderText="Department Name" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NumberOfStaff" HeaderText="Number Of Staff" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalBudget" HeaderText="Total Budget" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                            <%--<asp:BoundField DataField="NoOfVancancie" HeaderText="No Of Vancancie" ItemStyle-Width="12%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <%--&nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApprovedJobCircular"
                                        CommandArgument='<%# bind("StaffingBudgetId") %>' OnClientClick='<%#String.Format("return JobCircular({0})", Eval("StaffingBudgetId")) %>'
                                        ImageUrl="~/Images/approved.png" Text="" AlternateText="Details" ToolTip="Approve Received Item" />--%>
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdJobCircularCancel"
                                        CommandArgument='<%# bind("StaffingBudgetId") %>' OnClientClick='<%#String.Format("return CancelStaffingBudget({0})", Eval("StaffingBudgetId")) %>'
                                        ImageUrl="~/Images/cancel.png" Text="" AlternateText="Details" ToolTip="Cancel Budget" />
                                    <%--&nbsp;<asp:ImageButton ID="ImgDetailsRequisition" runat="server" CausesValidation="False"
                                        CommandName="CmdJobCircularDetails" CommandArgument='<%# bind("StaffingBudgetId") %>'
                                        OnClientClick='<%#String.Format("return JobCircularDetails({0})", Eval("StaffingBudgetId")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Receive Details" />--%>
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("StaffingBudgetId") %>' OnClientClick='<%#String.Format("return FillFormEdit({0})", Eval("StaffingBudgetId")) %>'
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
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
