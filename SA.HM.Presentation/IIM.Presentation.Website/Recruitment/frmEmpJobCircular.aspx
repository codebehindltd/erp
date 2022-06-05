<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpJobCircular.aspx.cs" Inherits="HotelManagement.Presentation.Website.Recruitment.frmEmpJobCircular" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Job Circular</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlAgeRangeFrom").change(function () {
                var AgeFrom = $("#ContentPlaceHolder1_ddlAgeRangeFrom").val();
                
                $('select#ContentPlaceHolder1_ddlAgeRangeTo').find('option').each(function () {
                    
                    var eachValue = $(this).val();
                    
                    if (eachValue <= AgeFrom && eachValue != 0) {
                        $(this).hide();
                    }
                    else {
                        $(this).show();
                    }
                });
                $('#ContentPlaceHolder1_ddlAgeRangeTo').val(0);

            })


            $("#ContentPlaceHolder1_ddlStaffRequisition").change(function () {
                if ($("#ContentPlaceHolder1_ddlStaffRequisition").val() != "0") {
                    LoadStaffRequisitionInformation();
                }
                else {
                    $("#ContentPlaceHolder1_ddlDepartments").val(result.DepartmentId);
                    $("#ContentPlaceHolder1_ddlJobType").val(result.JobType);
                    $("#ContentPlaceHolder1_ddlJobLevel").val(result.JobLevel);
                    $("#ContentPlaceHolder1_txtNoOfVancancie").val(result.RequisitionQuantity);
                    $("#ContentPlaceHolder1_txtDemandedTime").val(result.DemandDateString);
                }
            });

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtDemandedTime").datepicker({
                changeMonth: true,
                changeYear: true,
                minDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtFromDate").datepicker("option", "maxDate", selectedDate);
                }
            });

            function SaveUpdate() {

                if ($("#ContentPlaceHolder1_txtJobTitle").val() == "") {
                    toastr.info("Please Provide Job Title");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlDepartments").val() != "") {
                    toastr.info("Please Select Department");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlJobType").val() != "") {
                    toastr.info("Please Provide Job Type");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlJobLevel").val() != "") {
                    toastr.info("Please Provide Job Level");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtNoOfVancancie").val() != "") {
                    toastr.info("Please Provide No Of Vancancie");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtDemandedTime").val() != "") {
                    toastr.info("Please Provide Demanded Time");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlAgeRangeFrom").val() != "") {
                    toastr.info("Please Provide Age Range From");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlAgeRangeTo").val() != "") {
                    toastr.info("Please Provide Age Range To");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlAgeRangeTo").val() != "") {
                    toastr.info("Please Provide Age Range To");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlGender").val() != "") {
                    toastr.info("Please Provide Gender");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtJobDescription").val() != "") {
                    toastr.info("Please Provide Job Description");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtEducationalQualification").val() != "") {
                    toastr.info("Please Provide Education Qualification");
                    return false;
                }
                return true;
            }

        });

        function LoadStaffRequisitionInformation() {
            var staffRequisitionId = $("#ContentPlaceHolder1_ddlStaffRequisition").val();
            PageMethods.PageMethodsLoadStaffRequisitionInformation(staffRequisitionId, OnLoadStaffRequisitionInformationSucceed, OnLoadStaffRequisitionInformationFailed);
        }

        function OnLoadStaffRequisitionInformationSucceed(result) {
            if (result != null) {
                $("#ContentPlaceHolder1_ddlDepartments").val(result.DepartmentId);
                $("#ContentPlaceHolder1_ddlJobType").val(result.JobType);
                $("#ContentPlaceHolder1_ddlJobLevel").val(result.JobLevel);
                $("#ContentPlaceHolder1_txtNoOfVancancie").val(result.RequisitionQuantity);
                $("#ContentPlaceHolder1_txtDemandedTime").val(result.DemandDateString);
            }
        }
        function OnLoadStaffRequisitionInformationFailed(error) {

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

        function PrintJobCircularDetails(jobCircularId) {

            var url = "/Recruitment/Reports/frmReportJobCircularDetails.aspx?JobCircularId=" + jobCircularId;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=760,height=780,left=300,top=50,resizable=yes");

            //            window.location.href = "/Recruitment/Reports/frmReportJobCircularDetails.aspx?JobCircularId=" + jobCircularId;
            //            return false;                                           
        }
    </script>
    <asp:HiddenField ID="hfJobCircularId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsCurrentOrPreviousPage" runat="server" Value="" />
    <div id="CircularDetailsContainer" style="display: none;">
        <table id="DetailsJobCircularGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
            <tbody>
                <tr style='background-color: #F7F7F7;'>
                    <td style="font-weight: bold; width: 70px;">Job Title:
                    </td>
                    <td colspan="3" id="jobTitle" style="width: 100px;"></td>
                </tr>
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
                    <td style="font-weight: bold; width: 70px;">Demand Date:
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
                <a href="#tab-1">Job Circular Details</a></li>
            <li id="circularSearch" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-2">Search Job Circular</a></li>
        </ul>
        <div id="tab-1">
            <div id="" class="panel panel-default">
                <div class="panel-heading">Job Circular Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblStaffRequisition" runat="server" class="control-label required-field" Text="Staff Requisition"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlStaffRequisition" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLoanType" runat="server" class="control-label required-field" Text="Job Title"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtJobTitle" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlDepartments" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLoanTakenForMonthOrYear" runat="server" class="control-label required-field" Text="Job Type"></asp:Label>
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
                                    <asp:ListItem Value="0" Text="--- Please Select ---"></asp:ListItem>
                                    <asp:ListItem Value="Entry" Text="Entry"></asp:ListItem>
                                    <asp:ListItem Value="Mid" Text="Mid"></asp:ListItem>
                                    <asp:ListItem Value="Top" Text="Top"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblNoOfVancancie" runat="server" class="control-label required-field" Text="No Of Vacancies"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNoOfVancancie" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDemandedTime" runat="server" class="control-label required-field" Text="Demand Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDemandedTime" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Age Range From"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAgeRangeFrom" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                    <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                    <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                    <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                    <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                    <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                    <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                    <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                    <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                    <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                    <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                    <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                    <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                    <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                    <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                    <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                    <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                    <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                    <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                    <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                    <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                    <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                    <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                    <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                    <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                    <asp:ListItem Text="47" Value="46"></asp:ListItem>
                                    <asp:ListItem Text="48" Value="47"></asp:ListItem>
                                    <asp:ListItem Text="49" Value="48"></asp:ListItem>
                                    <asp:ListItem Text="50" Value="49"></asp:ListItem>
                                    <asp:ListItem Text="51" Value="50"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Age Range To"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAgeRangeTo" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                    <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                    <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                    <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                    <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                    <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                    <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                    <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                    <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                    <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                    <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                    <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                    <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                    <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                    <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                    <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                    <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                    <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                    <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                    <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                    <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                    <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                    <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                    <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                    <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                    <asp:ListItem Text="47" Value="46"></asp:ListItem>
                                    <asp:ListItem Text="48" Value="47"></asp:ListItem>
                                    <asp:ListItem Text="49" Value="48"></asp:ListItem>
                                    <asp:ListItem Text="50" Value="49"></asp:ListItem>
                                    <asp:ListItem Text="51" Value="50"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblGender" runat="server" class="control-label required-field" Text="Gender"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                    <asp:ListItem Text="Any" Value="Any"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Year Of Experience"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlYearOfExperiance" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                    <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                    <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                    <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                    <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                    <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblJobDescription" runat="server" class="control-label required-field" Text="Job Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtJobDescription" runat="server" CssClass="form-control"
                                    TextMode="MultiLine" Rows="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEducationalQualification" runat="server" class="control-label required-field" Text="Educational Qualification"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtEducationalQualification" runat="server" CssClass="form-control"
                                    TextMode="MultiLine" Rows="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Additional Job Requirement"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAdditionalJobRequirement" runat="server" CssClass="form-control"
                                    TextMode="MultiLine" Rows="5"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSave_Click" OnClientClick="return SaveUpdate();" />
                                <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">Search Job Circular</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSLoanType" runat="server" class="control-label" Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchDepartment" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblJobTitle" runat="server" class="control-label" Text="Job Title"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchJobTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
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
                <div class="panel-heading">Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvJobCircular" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" ForeColor="#333333" OnRowCommand="gvJobCircular_RowCommand"
                        OnRowDataBound="gvJobCircular_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("JobCircularId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="JobTitle" HeaderText="Job Title" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DepartmentName" HeaderText="Department Name" ItemStyle-Width="12%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="JobTypeName" HeaderText="Job Type" ItemStyle-Width="12%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="JobLevel" HeaderText="Job Level" ItemStyle-Width="12%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NoOfVancancie" HeaderText="No Of Vacancy" ItemStyle-Width="12%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ApprovedStatus" HeaderText="Status" ItemStyle-Width="12%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApprovedJobCircular"
                                        CommandArgument='<%# bind("JobCircularId") %>' OnClientClick="return confirm('Do you want to approve?');" OnClick='<%#String.Format("return JobCircular({0})", Eval("JobCircularId")) %>'
                                        ImageUrl="~/Images/approved.png" Text="" AlternateText="Details" ToolTip="Approve" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdJobCircularCancel"
                                        CommandArgument='<%# bind("JobCircularId") %>' OnClientClick="return confirm('Do you want to cancel?');"  OnClick='<%#String.Format("return CancelJobCircualr({0})", Eval("JobCircularId")) %>'
                                        ImageUrl="~/Images/cancel.png" Text="" AlternateText="Details" ToolTip="Cancel" />
                                    &nbsp;<asp:ImageButton ID="ImgDetailsRequisition" runat="server" CausesValidation="False"
                                        CommandName="CmdJobCircularDetails" CommandArgument='<%# bind("JobCircularId") %>'
                                        OnClientClick='<%#String.Format("return JobCircularDetails({0})", Eval("JobCircularId")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Receive Details" />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("JobCircularId") %>' OnClientClick="return confirm('Do you want to edit?');" OnClick='<%#String.Format("return FillForm({0})", Eval("JobCircularId")) %>'
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImagePrint" runat="server" CausesValidation="False" CommandName="CmdPrintJobCircularDetails"
                                        CommandArgument='<%# bind("JobCircularId") %>' OnClientClick='<%#String.Format("return PrintJobCircularDetails({0})", Eval("JobCircularId")) %>'
                                        ImageUrl="~/Images/ReportDocument.png" Text="" AlternateText="Details" ToolTip="Print Out Job Circular" />
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
