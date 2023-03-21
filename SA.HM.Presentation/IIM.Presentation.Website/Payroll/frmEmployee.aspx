<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmEmployee.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmployee" %>

<%@ Register TagPrefix="UserControl" TagName="companyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="srcCompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<%@ Register Assembly="flashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        var EmployeeType = new Array();
        var single = "";

        function OnEmpCodeCheckSucceeded(result) {
            if (!result) {
                toastr.warning("This Employee ID is Duplicate. Please Give Another Unique One.");
                $("#ContentPlaceHolder1_txtEmpCode").val("");
            }
        }
        function OnEmpCodeCheckFailed() { }

        $(document).ready(function () {

            $('#ContentPlaceHolder1_ddlMaritalStatus').each(function () {
                if ($(this).val() === 'Married') {
                    console.log("working");
                    $("#marriage").show();
                } else {
                    $("#marriage").hide();
                    /*$('input[name=marriage]').hide();*/
                }
            });
            
            $("#ContentPlaceHolder1_ddlMaritalStatus").change(function () {
                
                if ($(this).val() === 'Married') {
                    console.log("working");
                    $("#marriage").show();
                } else {
                    $("#marriage").hide();
                    /*$('input[name=marriage]').hide();*/
                }
            });


            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            CommonHelper.ApplyIntigerValidation();
            CommonHelper.ApplyDecimalValidation();
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

            $("#ContentPlaceHolder1_ddlEmployeeCompany").select2({
                tags: false,
                allowClear: true,
                placeholder: "",
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlWorkStation").select2({
                tags: false,
                allowClear: true,
                placeholder: "",
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlReportingTo").select2({
                tags: false,
                allowClear: true,
                placeholder: "",
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlReportingTo2").select2({
                tags: false,
                allowClear: true,
                placeholder: "",
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlBank").select2({
                tags: false,
                allowClear: true,
                placeholder: "",
                width: "99.75%"
            });

            single = $("#ContentPlaceHolder1_hfIsSingle").val();
            if (single == "1") {
                $('#glCompanyDiv').hide();

            }
            else {
                $('#glCompanyDiv').show();
            }

            var ctrl = '#<%=chkIsProvisionPeriod.ClientID%>'
            if ($(ctrl).is(':checked')) {
                $("#<%=txtProvisionPeriod.ClientID %>").attr('disabled', false);
            }
            else {
                $("#<%=txtProvisionPeriod.ClientID %>").attr('disabled', true);
            }

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfEmployeeType").val() != "") {
                EmployeeType = JSON.parse($("#ContentPlaceHolder1_hfEmployeeType").val());

                var empTypeId = $("#ContentPlaceHolder1_ddlEmpCategoryId").val();
                var empType = _.findWhere(EmployeeType, { TypeId: parseInt(empTypeId) });

                $("#ProvisionPeriodDiv").hide();
                $("#ContractEndDateDiv").hide();

                if (empType != null) {
                    if (empType.TypeCategory == "Contractual") {
                        $("#ContractEndDateDiv").show();
                    }
                    else if (empType.TypeCategory == "Probational") {
                        $("#ProvisionPeriodDiv").show();
                    }
                }
            }
            $("#<%=txtOfficialEmail.ClientID %>").blur(function () {
                if ($("#<%=txtOfficialEmail.ClientID %>").val() != "") {
                    var isValidEmail = CommonHelper.IsValidEmail($("#<%=txtOfficialEmail.ClientID %>").val());
                        if (!isValidEmail) {
                            toastr.warning("Invalid Email");
                            $("#<%=txtOfficialEmail.ClientID %>").focus();
                        return false;
                    }
                }
            });
            $("#<%=txtPersonalEmail.ClientID %>").blur(function () {
                if ($("#<%=txtPersonalEmail.ClientID %>").val() != "") {
                    var isValidEmail = CommonHelper.IsValidEmail($("#<%=txtPersonalEmail.ClientID %>").val());
                    if (!isValidEmail) {
                        toastr.warning("Invalid Email");
                        $("#<%=txtPersonalEmail.ClientID %>").focus();
                            return false;
                        }
                    }
            });
            $("#<%=txtAlternativeEmail.ClientID %>").blur(function () {
                if ($("#<%=txtPersonalEmail.ClientID %>").val() != "") {
                    var isValidEmail = CommonHelper.IsValidEmail($("#<%=txtAlternativeEmail.ClientID %>").val());
                    if (!isValidEmail) {
                        toastr.warning("Invalid Email");
                        $("#<%=txtAlternativeEmail.ClientID %>").focus();
                            return false;
                        }
                    }
            });
            $("#<%=txtEmergencyContactEmail.ClientID %>").blur(function () {
                if ($("#<%=txtEmergencyContactEmail.ClientID %>").val() != "") {
                    var isValidEmail = CommonHelper.IsValidEmail($("#<%=txtEmergencyContactEmail.ClientID %>").val());
                    if (!isValidEmail) {
                        toastr.warning("Invalid Email");
                        $("#<%=txtEmergencyContactEmail.ClientID %>").focus();
                            return false;
                        }
                    }
            });

            $('#ContentPlaceHolder1_A').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_B').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_C').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_D').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_E').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_F').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_G').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_H').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_I').click(function () {
                $('#SubmitButtonDiv').hide();
            });

            $("#SearchOutput").hide();
            $("#btnSearch").click(function () {
                $("#SearchOutput").show('slow');
                GridPaging(1, 1);
            });

            var txtEmpJoinDate = '<%=txtEmpJoinDate.ClientID%>'
            var txtEmpDateOfBirth = '<%=txtEmpDateOfBirth.ClientID%>'
            UploadComplete();

            $("#ContentPlaceHolder1_txtEmpCode").change(function () {
                if ($("#ContentPlaceHolder1_txtEmpCode").val() != "") {
                    if ($("#ContentPlaceHolder1_hfEmpCode").val() == $("#ContentPlaceHolder1_txtEmpCode").val()) {
                        return;
                    }

                    PageMethods.CheckDuplicateEmployeeCode($("#ContentPlaceHolder1_txtEmpCode").val(), OnEmpCodeCheckSucceeded, OnEmpCodeCheckFailed);
                }
                return false;
            });

            $("#<%=ddlEmpCategoryId.ClientID %>").change(function () {
                var ddlEmpCategoryId = $("#<%=ddlEmpCategoryId.ClientID %>").val();
                PageMethods.GetEmpTypeInfoById(ddlEmpCategoryId, OnPerformForGetEmpTypeInfoByIdSucceeded, OnPerformForGetEmpTypeInfoByIdFailed)
                return false;
            });


            $("#<%=ddlGradeId.ClientID %>").change(function () {
                LoadProvisionPeriodInfo();
            });

            $('#ContentPlaceHolder1_txtDateOfBirth').datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
                yearRange: "-100:+0",
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    AgeCalc(selectedDate, 'ContentPlaceHolder1_txtAge', 'ContentPlaceHolder1_hfAge');
                }
            });


            $('#ContentPlaceHolder1_txtDateOfMarriage').datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
                yearRange: "-100:+0",
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    AgeCalc(selectedDate, 'ContentPlaceHolder1_txtAge', 'ContentPlaceHolder1_hfAge');
                }
            });

            $('#ContentPlaceHolder1_txtNomineeDateOfBirth').datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
                yearRange: "-100:+0",
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    AgeCalc(selectedDate, 'ContentPlaceHolder1_txtNomineeAge', 'ContentPlaceHolder1_hfNomineeAge');
                }
            });

            $("#ContentPlaceHolder1_txtJoinDate").datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                yearRange: "-100:+0",
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtLeaveDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtLeaveDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                yearRange: "-100:+0",
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtJoinDate").datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtPExpireDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtPIssueDate").datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtPIssueDate").datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtPExpireDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtEmpJoinDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //maxDate: 0,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtContractEndDate').datepicker("option", "minDate", selectedDate);
                    $('#ContentPlaceHolder1_txtProvisionPeriod').datepicker("option", "minDate", selectedDate);
                    $('#ContentPlaceHolder1_txtDateOfBirth').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtContractEndDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: $('#ContentPlaceHolder1_txtEmpJoinDate').val(),
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtEmpJoinDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtProvisionPeriod').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $('#ContentPlaceHolder1_txtEmpDateOfBirth').datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: DayOpenDate,
                yearRange: "-100:+0",
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    AgeCalc(selectedDate, 'ContentPlaceHolder1_txtAge', 'ContentPlaceHolder1_hfAge');
                    $('#ContentPlaceHolder1_txtEmpJoinDate').datepicker("option", "minDate", selectedDate);
                    $('#ContentPlaceHolder1_txtPIssueDate').datepicker("option", "minDate", selectedDate);
                }
            });

            var benefitGrid = document.getElementById("<%=gvBenefit.ClientID %>");
            var rows = benefitGrid.getElementsByTagName("tr")
            for (var i = 0; i < rows.length - 1; i++) {
                $("#ContentPlaceHolder1_gvBenefit_txtBenefitEffectiveDate_" + i).datepicker({
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: innBoarDateFormat
                });
            }

            $('#ContentPlaceHolder1_ddlDivision').change(function () {
                LoadDistrict();
            });

            $('#ContentPlaceHolder1_ddlDistrict').change(function () {
                LoadThana();
            });

            $('#ContentPlaceHolder1_ddlDistrict').change(function () {
                var districtId = $("#<%=ddlDistrict.ClientID %>").val();
                    $("#<%=hfddlDistrictId.ClientID %>").val(districtId);
                });

            $('#ContentPlaceHolder1_ddlThana').change(function () {
                var thanaId = $("#<%=ddlThana.ClientID %>").val();
                $("#<%=hfddlThanaId.ClientID %>").val(thanaId);
            });

            $("#ContentPlaceHolder1_ddlTitle").change(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                if (lastName != "") {
                    $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName + " " + lastName);
                    }
                    else {
                        $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName);
                    }
            });

            $("#ContentPlaceHolder1_txtLastName").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                if (lastName != "") {
                    $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName + " " + lastName);
                    }
                    else {
                        $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName);
                    }
            });

            $("#ContentPlaceHolder1_txtFirstName").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                if (lastName != "") {
                    $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName + " " + lastName);
                    }
                    else {
                        $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName);
                    }
            });

            $("#ContentPlaceHolder1_ddlTitle").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var firstName = $("#<%=txtFirstName.ClientID %>").val();
                var lastName = $("#<%=txtLastName.ClientID %>").val();
                if (lastName != "") {
                    $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName + " " + lastName);
                    }
                    else {
                        $("#<%=txtDisplayName.ClientID %>").val(title + " " + firstName);
                    }
            });


            var hfIsPayrollCompanyAndEmployeeCompanyDifferent = $("#<%=hfIsPayrollCompanyAndEmployeeCompanyDifferent.ClientID %>").val();
            if (hfIsPayrollCompanyAndEmployeeCompanyDifferent == 1) {
                $("#EmployeeCompanyLabelDiv").show();
                $("#EmployeeCompanyControlDiv").show();
            }
            else {
                $("#EmployeeCompanyLabelDiv").hide();
                $("#EmployeeCompanyControlDiv").hide();
            }



            var isEmployeeBasicSetUp = $("#<%=hfIsEmployeeBasicSetUp.ClientID %>").val();
            if (isEmployeeBasicSetUp == 1) {
                //$($("#myTabs").find("li")[2]).hide();
                //$($("#myTabs").find("li")[3]).hide();
                //$($("#myTabs").find("li")[4]).hide();
                //$($("#myTabs").find("li")[5]).hide();
                //$($("#myTabs").find("li")[6]).hide();
                //$($("#myTabs").find("li")[7]).hide();
                //$($("#myTabs").find("li")[8]).hide();
            }

            var hfIsPayrollDependentHide = $("#<%=hfIsPayrollDependentHide.ClientID %>").val();
            if (hfIsPayrollDependentHide == 1) {
                $($("#myTabs").find("li")[4]).hide();
            }

            var hfIsPayrollBeneficiaryHide = $("#<%=hfIsPayrollBeneficiaryHide.ClientID %>").val();
            if (hfIsPayrollBeneficiaryHide == 1) {
                $($("#myTabs").find("li")[5]).hide();
            }

            var hfIsPayrollReferenceHide = $("#<%=hfIsPayrollReferenceHide.ClientID %>").val();
            if (hfIsPayrollReferenceHide == 1) {
                $($("#myTabs").find("li")[6]).hide();
            }

            var hfIsPayrollBenefitsHide = $("#<%=hfIsPayrollBenefitsHide.ClientID %>").val();
            if (hfIsPayrollBenefitsHide == 1) {
                $($("#myTabs").find("li")[7]).hide();
            }

            var hfIsPayrollLetterPanelHide = $("#<%=hfIsPayrollLetterPanelHide.ClientID %>").val();
            if (hfIsPayrollLetterPanelHide == 1) {
                $($("#myTabs").find("li")[9]).hide();
            }

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(<%=_CompanyId%>).trigger('change');
            PageMethods.LoadProjectByCompanyId(<%=_CompanyId%>, OnProjectLoadSucceeded, OnProjectLoadFailed);

            $("#ContentPlaceHolder1_txtEmpJoinDate").blur(function () {
                var date = $("#<%=txtEmpJoinDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtEmpJoinDate.ClientID %>").focus();
                        $("#<%=txtEmpJoinDate.ClientID %>").val("");
                        return false;
                    }
                }
            });
            $("#<%=txtEmpDateOfBirth.ClientID %>").blur(function () {

                var date = $("#<%=txtEmpDateOfBirth.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtEmpDateOfBirth.ClientID %>").focus();
                            $("#<%=txtEmpDateOfBirth.ClientID %>").val("");
                            return false;
                        }
                    }
            });
            $("#<%=txtJoinDate.ClientID %>").blur(function () {
                var date = $("#<%=txtJoinDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtJoinDate.ClientID %>").focus();
                            $("#<%=txtJoinDate.ClientID %>").val("");
                            return false;
                        }
                    }
            });
            $("#<%=txtLeaveDate.ClientID %>").blur(function () {
                var date = $("#<%=txtLeaveDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtLeaveDate.ClientID %>").focus();
                            $("#<%=txtLeaveDate.ClientID %>").val("");
                            return false;
                        }
                    }
            });
            $("#<%=txtDateOfBirth.ClientID %>").blur(function () {
                var date = $("#<%=txtDateOfBirth.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtDateOfBirth.ClientID %>").focus();
                            $("#<%=txtDateOfBirth.ClientID %>").val("");
                            return false;
                        }
                    }
            });
            $("#<%=txtContractEndDate.ClientID %>").blur(function () {
                var date = $("#<%=txtContractEndDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtContractEndDate.ClientID %>").focus();
                            $("#<%=txtContractEndDate.ClientID %>").val("");
                            return false;
                        }
                    }
            });
            $("#<%=txtPIssueDate.ClientID %>").blur(function () {
                var date = $("#<%=txtPIssueDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtPIssueDate.ClientID %>").focus();
                            $("#<%=txtPIssueDate.ClientID %>").val("");
                            return false;
                        }
                    }
            });
            $("#<%=txtPExpireDate.ClientID %>").blur(function () {
                var date = $("#<%=txtPExpireDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtPExpireDate.ClientID %>").focus();
                            $("#<%=txtPExpireDate.ClientID %>").val("");
                            return false;
                        }
                    }
            });
        });

            function OnProjectLoadSucceeded(result) {
                $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject').children().remove();
                $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject').append(`<option value="0">--- All ---</option>`);
                $.each(result, function(index, item) {
                    // access the properties of each user
                
                    if(item.ProjectId == <%=_ProjectId%>)
                {
                    $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject').append(`<option value="${item.ProjectId}" selected>${item.Name}</option>`);
            }
            else
            {
                    $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject').append(`<option value="${item.ProjectId}">${item.Name}</option>`);
            }
            });

            }

            function OnProjectLoadFailed() {

            }
            function LoadDistrict() {
                var divisionId = $("#<%=ddlDivision.ClientID %>").val();
            PageMethods.LoadDistrict(divisionId, OnLoadDivisionSucceeded, OnLoadDivisionFailed);
            return false;
        }

        function DeleteDoc(docId, rowIndex) {
            if (confirm("Do you want to delete?")) {
                PageMethods.DeleteDoc(docId, OnDeleteDocSucceeded, OnDeleteDocFailed);
                $("#trdoc" + rowIndex).remove();
                return false;
            }
        }

        function OnDeleteDocSucceeded(result) {
            if (result == true) {
                toastr.success("Delete operation successfully done.");
            }
            else {
                toastr.error("Delete operation unsuccfull.");
            }
        }

        function OnDeleteDocFailed(error) {
        }

        function LoadThana() {
            var districtId = $("#<%=ddlDistrict.ClientID %>").val();
            PageMethods.LoadThana(districtId, OnLoadDistrictSucceeded, OnLoadDistrictFailed);
            return false;
        }

        function OnLoadDivisionSucceeded(result) {
            var list = result;
            var controlId = '<%=ddlDistrict.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].DistrictName + '" value="' + list[i].DistrictId + '">' + list[i].DistrictName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
        }
        function OnLoadDivisionFailed(error) {
        }

        function OnLoadDistrictSucceeded(result) {
            var list = result;
            var controlId = '<%=ddlThana.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].ThanaName + '" value="' + list[i].ThanaId + '">' + list[i].ThanaName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
        }
        function OnLoadDistrictFailed(error) {
        }

        function PerformLetterPreviewAction(empId, letterType) {
            var url = "";
            var popup_window = "Preview";
            url = "/Payroll/Reports/frmPayrollReport.aspx?TId=" + empId + "," + letterType;
            window.open(url, popup_window, "width=750,height=680,left=300,top=50,resizable=yes");
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var companyId = $("#ContentPlaceHolder1_srcCompanyProjectUserControl_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_srcCompanyProjectUserControl_ddlGLProject").val();
            debugger;
            if (companyId == null) {
                //companyId = "1";
                companyId = "0";
            }

            if (projectId == null) {
                //projectId = "1";
                projectId = "0";
            }

            //if (projectId == "0") {
            //    projectId = "1";
            //}

            var gridRecordsCount = $("#gvGustIngormation tbody tr").length;
            var empName = $("#<%=txtEmployeeName.ClientID %>").val();
            var code = $("#<%=txtEmployeeCode.ClientID %>").val();
            var department = $("#<%=txtEmployeeDepartment.ClientID %>").val();
            var designation = $("#<%=txtEmployeeDesignation.ClientID %>").val();

            PageMethods.SearchNLoadEmpInformation(companyId, projectId, empName, code, department, designation, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }

        function OnLoadObjectSucceeded(result) {
            $("#gvEmployee tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#gvEmployee tbody ").append(emptyTr);
                $('#ContentPlaceHolder1_lblEmployeeNumberCount').text('Total Number Employee: 0');
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#gvEmployee tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:5%; cursor:pointer;\">" + gridObject.SerialNumber + "</td>";
                tr += "<td align='left' style=\"width:30%; cursor:pointer;\">" + gridObject.DisplayName + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\">" + gridObject.EmpCode + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.Department + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.Designation + "</td>";
                if (IsCanEdit) {
                    tr += "<td align='right' style=\"width:20%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.EmpId + "')\" alt='Edit Information' border='0' />";
                    var hfIsPayrollLetterPanelHide = $("#<%=hfIsPayrollLetterPanelHide.ClientID %>").val();
                    if (hfIsPayrollLetterPanelHide == 0) {
                        if (gridObject.AppoinmentLetter != "") {
                            tr += "&nbsp;&nbsp;<img id='preview' src='../Images/ReportDocument.png' onClick= \"javascript:return PerformLetterPreviewAction(" + gridObject.EmpId + ", 'AppoinmentLetter')\" alt='Appoinment Letter' Title='Appoinment Letter' border='0'/>";
                        }
                        if (gridObject.JoiningAgreement != "") {
                            tr += "&nbsp;&nbsp;<img id='preview' src='../Images/ReportDocument.png' onClick= \"javascript:return PerformLetterPreviewAction(" + gridObject.EmpId + ", 'JoiningAgreement')\" alt='Joining Agreement' Title='Joining Agreement' border='0'/>";
                        }
                        if (gridObject.ServiceBond != "") {
                            tr += "&nbsp;&nbsp;<img id='preview' src='../Images/ReportDocument.png' onClick= \"javascript:return PerformLetterPreviewAction(" + gridObject.EmpId + ", 'ServiceBond')\" alt='Service Bond' Title='Service Bond' border='0'/>";
                        }
                        if (gridObject.DSOAC != "") {
                            tr += "&nbsp;&nbsp;<img id='preview' src='../Images/ReportDocument.png' onClick= \"javascript:return PerformLetterPreviewAction(" + gridObject.EmpId + ", 'DSOAC')\" alt='DSOAC' Title='DSOAC' border='0'/>";
                        }
                        if (gridObject.ConfirmationLetter != "") {
                            tr += "&nbsp;&nbsp;<img id='preview' src='../Images/ReportDocument.png' onClick= \"javascript:return PerformLetterPreviewAction(" + gridObject.EmpId + ", 'ConfirmationLetter')\" alt='Confirmation Letter' Title='Confirmation Letter' border='0'/>";
                        }
                    }
                    tr += "</td>";
                }
                else {
                    tr += "<td align='right' style=\"width:20%; cursor:pointer;\"></td>";
                }

                tr += "</tr>"

                $("#gvEmployee tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            if (result.GridData.length > 0) {
                $('#ContentPlaceHolder1_lblEmployeeNumberCount').text('Total Employee Number : ' + result.GridData[0].TotalEmployeeNumber);
            }
            else {
                $('#ContentPlaceHolder1_lblEmployeeNumberCount').text('Total Employee Number : 0');
            }
            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformEditAction(empId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            var possiblePath = "frmEmployee.aspx?editId=" + empId;
            window.location = possiblePath;
        }

        function OnPerformForGetEmpTypeInfoByIdSucceeded(result) {
            var txtContractEndDate = '<%=txtContractEndDate.ClientID%>'

            $("#ProvisionPeriodDiv").hide();
            $("#ContractEndDateDiv").hide();

            if (result.TypeCategory == "Intern") {
                $("#ContractEndDateDiv").show();
            }
            if (result.TypeCategory == "Contractual") {
                $("#ContractEndDateDiv").show();
            }
            if (result.TypeCategory == "Probational") {
                $("#ProvisionPeriodDiv").show();
            }
        }

        function OnPerformForGetEmpTypeInfoByIdFailed(error) {
            toastr.error(error.get_message());
        }

        function ToggleFieldVisible() {
            $("#<%=txtProvisionPeriod.ClientID %>").val("");
            var ctrl = '#<%=chkIsProvisionPeriod.ClientID%>'
            if ($(ctrl).is(':checked')) {
                $("#<%=txtProvisionPeriod.ClientID %>").attr('disabled', false);
                LoadProvisionPeriodInfo();
            }
            else {
                $("#<%=txtProvisionPeriod.ClientID %>").attr('disabled', true);
            }
        }

        function LoadProvisionPeriodInfo() {
            var txtEmpJoinDateVal = $("#<%=txtEmpJoinDate.ClientID %>").val();
            var ddlGradeIdVal = $("#<%=ddlGradeId.ClientID %>").val();
            var IsProvisionPeriodVal = 0;
            var ctrl = '#<%=chkIsProvisionPeriod.ClientID%>'
            if ($(ctrl).is(':checked')) {
                IsProvisionPeriodVal = 1;
                PageMethods.LoadProvisionPeriodInformation(txtEmpJoinDateVal, ddlGradeIdVal, IsProvisionPeriodVal, OnLoadProvisionPeriodInformationSucceeded, OnLoadProvisionPeriodInformationFailed);
            }

            return false;
        }

        function OnLoadProvisionPeriodInformationSucceeded(result) {
            $("#<%=txtProvisionPeriod.ClientID %>").val(result);
            return false;
        }
        function OnLoadProvisionPeriodInformationFailed(error) {
            toastr.error(error.get_message());
        }

        function AgeCalc(dateOfBirth, display, hf) {
            if ($.trim(dateOfBirth) == "") {
                return;
            }

            var mydate = CommonHelper.DateFormatToMMDDYYYY(dateOfBirth, "/");
            var params = JSON.stringify({ dateOfBirth: $.trim(mydate) });

            mydate = CommonHelper.DateFormatToYYYYMMDD(dateOfBirth, '/');
            var today = moment();

            var a = moment(today);
            var b = moment(mydate);
            var extra = b.month();
            var isLeapYear = moment(b).isLeapYear();

            var years = a.diff(b, 'year');
            b.add(years, 'years');

            var months = a.diff(b, 'months');
            b.add(months, 'months');

            var days = a.diff(b, 'days');
            var displaySeq = years + ' years, ' + months + ' months, ' + days + ' days';
            $("#" + display).val(displaySeq);
            $("#" + hf).val(displaySeq);
        }

        $(function () {
            $("#myTabs").tabs();
        });

        $(function () {
            $("#tab-10").tabs();
        });

        function LoadSignatureUploader() {
            var randomId = +$("#ContentPlaceHolder1_RandomEmpId").val();
            var path = "/Payroll/Images/Signature/";
            var category = "Employee Signature";
            var iframeid = 'Iframe1';
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#popUpSignature").dialog({
                width: 600,
                height: 300,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "",
                show: 'slide'
            });
            return false;
        }

        function LoadDocumentUploader() {
            var randomId = +$("#ContentPlaceHolder1_RandomEmpId").val();
            var path = "/Payroll/Images/Documents/";
            var category = "Employee Document";
            var iframeid = 'Iframe2';
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;
            $("#popUpDocument").dialog({
                width: 600,
                height: 300,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "",
                show: 'slide'
            });
            return false;
        }

        function LoadOthersDocumentUploader() {
            var randomId = +$("#ContentPlaceHolder1_RandomEmpId").val();
            var path = "/Payroll/Images/Documents/";
            var category = "Employee Other Documents";
            var iframeid = 'frmPrint';
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#DocumentDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 300,
                closeOnEscape: false,
                resizable: false,
                title: "Documents Upload",
                show: 'slide'
            });
            return false;
        }

        function UploadComplete() {
            var id = $("#ContentPlaceHolder1_RandomEmpId").val();
            if (id != 0) {
                ShowUploadedDocument(id);
                ShowUploadedSignature(id);
                ShowUploadedOthersDocument(id);
            }
        }

        function ShowUploadedDocument(id) {
            PageMethods.GetUploadedImageByWebMethod(id, "Employee Document", OnGetUploadedImageByWebMethodSucceeded, OnGetUploadedImageByWebMethodFailed);
            return false;
        }

        function ShowUploadedSignature(id) {
            PageMethods.GetUploadedImageByWebMethod(id, "Employee Signature", OnGetUploadedSignatureByWebMethodSucceeded, OnGetUploadedSignatureByWebMethodFailed);
            return false;
        }

        function ShowUploadedOthersDocument(id) {
            PageMethods.GetUploadedDocumentsByWebMethod(id, "Employee Other Documents", OnGetUploadedOthersDocumentByWebMethodSucceeded, OnGetUploadedOthersDocumentByWebMethodFailed);
            return false;
        }

        function OnGetUploadedSignatureByWebMethodSucceeded(result) {
            $('#ContentPlaceHolder1_DocDiv').html(result);
            return false;
        }
        function OnGetUploadedSignatureByWebMethodFailed(error) {
            toastr.error(error.get_message());
        }

        function OnGetUploadedImageByWebMethodSucceeded(result) {
            $('#ContentPlaceHolder1_SigDiv').html(result);
            return false;
        }
        function OnGetUploadedImageByWebMethodFailed(error) {
            toastr.error(error.get_message());
        }

        function ShowDocument(path, name) {
            var iframeid = 'fileIframe';
            document.getElementById(iframeid).src = path;
            $("#ShowDocumentDiv").dialog({
                autoOpen: true,
                modal: true,
                width: "82%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Document - " + name,
                show: 'slide'
            });
            return false;
        }

        function OnGetUploadedOthersDocumentByWebMethodSucceeded(result) {
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            DocTable = "";

            DocTable += "<table id='DocTableList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }
                DocTable += "<td align='left' style='width: 50%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    if (result[row].Extention == ".jpg" || result[row].Extention == ".png") {
                        imagePath = "<img src='" + result[row].Path + result[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    }
                    else {
                        imagePath = "<img src='" + result[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    }
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                DocTable += "<td align='left' style='width: 20%'>";
                DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                DocTable += "</td>";
                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#ContentPlaceHolder1_DocumentInfo").html(DocTable);
            $('#OthersDocDiv').html(result);
            return false;
        }
        function OnGetUploadedOthersDocumentByWebMethodFailed(error) {
            toastr.error(error.get_message());
        }

        function CheckAgeEligibility() {
            var dob = $.trim($("#ContentPlaceHolder1_txtDateOfBirth").val());
            if (dob == "") {
                toastr.warning("Please Give Date of Birth");
                return false;
            }

            var vc = $("#ContentPlaceHolder1_hfAge").val();
            var year = 0, month = 0, days = 0;
            return true;
        }

        function ValidateNomineePerentage() {
            var givenPercentage = $("#ContentPlaceHolder1_txtPercentage").val();
            if (CommonHelper.IsDecimal(givenPercentage) == false) {
                toastr.warning("Percentage Amount Should Correct Format");
                return false;
            }

            var nomineeGridLength = $("#ContentPlaceHolder1_gvNominee tbody tr").length;
            var row = 0, percentage = 0.0;

            if (givenPercentage == "") {
                toastr.warning("Please Give Percentage");
                return false;
            }
            else if (parseFloat(givenPercentage) > 100) {
                toastr.warning("Total Beneficiary percentage shouldn't be more than 100");
                return false;
            }
            percentage += parseFloat(givenPercentage);

            if (nomineeGridLength > 1) {

                for (row = 1; row < nomineeGridLength; row++) {
                    var tr = $("#ContentPlaceHolder1_gvNominee tbody tr:eq(" + row + ")");

                    var editedNomneeId = parseFloat($("#ContentPlaceHolder1_gvNominee tbody tr:eq(" + row + ")").find("td:eq(3)").text());
                    var nomineeId = parseFloat($("#ContentPlaceHolder1_hfNomineeId").val());

                    if (editedNomneeId == nomineeId) {
                        continue;
                    }
                    else {
                        percentage += parseFloat(tr.find("td:eq(2)").text());
                    }

                    $("#ContentPlaceHolder1_hfNomineeId").val(0);
                    $("#ContentPlaceHolder1_lblNomineeId").text(0);
                }

                if (percentage > 100) {
                    toastr.warning("Total Beneficiary percentage should not greater than 100");
                    return false;
                }
            }
        }

        function ValidateReferencePerson() {
            var ReferenceName = $("#ContentPlaceHolder1_txtReferenceName").val();
            if (ReferenceName == "") {
                toastr.warning("Enter a reference name");
                $("#ContentPlaceHolder1_txtReferenceName").focus();
                return false;
            }
            var Relationship = $("#ContentPlaceHolder1_txtReferenceRelationship").val();

            if (Relationship == "") {
                toastr.warning("Enter Relationship");
                $("#ContentPlaceHolder1_txtReferenceRelationship").focus();
                return false;
            }

            var email = $("#<%=txtEmail.ClientID %>").val();
            if (email != "") {
                if (!CommonHelper.IsValidEmail(email)) {
                    toastr.warning("Please Insert Valid Email");
                    $("#<%=txtEmail.ClientID %>").focus();
                    return false;
                }
            }
        }
        function CheckAll(Checkbox) {
            var GridView1 = document.getElementById("<%=gvBenefit.ClientID %>");
            if ($("#ContentPlaceHolder1_gvBenefit_ChkCreate").is(':checked')) {
                for (i = 1; i < GridView1.rows.length; i++) {
                    GridView1.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = true;
                }
            }
            else {
                for (i = 1; i < GridView1.rows.length; i++) {
                    GridView1.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = false;
                }
            }
        }
        function CheckValidity() {
            debugger;
            var benefitGrid = document.getElementById("<%=gvBenefit.ClientID %>");
            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            if (companyId == null) {
                companyId = "1";
            }

            if (projectId == null) {
                projectId = "1";
            }

            if (projectId == "0") {
                projectId = "1";
            }

            if (companyId == "0") {
                if ($("#ContentPlaceHolder1_hfIsPayrollCompanyAndEmployeeCompanyDifferent").val() == "1") {
                    toastr.warning("Please Select Payroll Company.");
                }
                else {
                    toastr.warning("Please Select Company.");
                }

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                return false;
            }

            if (companyId != "0") {
                if (projectId == "0") {
                    if ($("#ContentPlaceHolder1_hfIsPayrollCompanyAndEmployeeCompanyDifferent").val() == "1") {
                        toastr.warning("Please Select Payroll Project.");
                    }
                    else {
                        toastr.warning("Please Select Project.");
                    }
                    $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                    return false;
                }
            }

            if ($("#ContentPlaceHolder1_hfIsPayrollCompanyAndEmployeeCompanyDifferent").val() == "1") {
                if ($("#ContentPlaceHolder1_ddlEmpCompanyId").val() == "0") {
                    toastr.warning("Please Select Employee Company.");
                    $("#ContentPlaceHolder1_ddlEmpCompanyId").focus();
                    return false;
                }
            }

            $("#ContentPlaceHolder1_hfGLCompanyId").val(companyId);
            $("#ContentPlaceHolder1_hfGLProjectId").val(projectId);

            for (var i = 0; i < benefitGrid.rows.length - 1; i++) {
                if ($("#ContentPlaceHolder1_gvBenefit_chkIsSavePermission_" + i).is(':checked')) {
                    var date = $("#ContentPlaceHolder1_gvBenefit_txtBenefitEffectiveDate_" + i).val();
                    if (date != "") {
                        date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                        var isValid = CommonHelper.IsVaildDate(date);
                        if (!isValid) {
                            $("#myTabs").tabs({ active: 7 });
                            toastr.warning("Invalid Date");
                            $("#ContentPlaceHolder1_gvBenefit_txtBenefitEffectiveDate_" + i).focus();
                            $("#ContentPlaceHolder1_gvBenefit_txtBenefitEffectiveDate_" + i).val("");
                            return false;
                        }
                    }

                }
            }
            return true;
        }
    </script>
    <asp:HiddenField ID="hfIsSingle" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfGLCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfGLProjectId" runat="server" Value="0" />
    <asp:HiddenField ID="hfNomineeId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hflocalCurrencyId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfddlDistrictId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfddlThanaId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsEmployeeBasicSetUp" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfEmployeeType" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfIsPayrollDependentHide" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsPayrollBeneficiaryHide" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsPayrollReferenceHide" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsPayrollBenefitsHide" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsPayrollLetterPanelHide" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsEmployeeCodeAutoGenerate" runat="server" />
    <asp:HiddenField ID="hfIsPayrollCompanyAndEmployeeCompanyDifferent" runat="server" Value="0" />
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Official</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Details</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Education</a></li>
            <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-4">Experience</a></li>
            <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-5">Dependent</a></li>
            <li id="F" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-6">Beneficiary</a></li>
            <li id="G" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-7">Reference</a></li>
            <li id="H" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-8">Benefits</a></li>
            <li id="I" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-9">Bank Info</a></li>
            <li id="J" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-10">Letter</a></li>
            <li id="K" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-11">Documents</a></li>
            <%--<li id="L" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-12">Others...</a></li>--%>
            <li id="M" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-13">Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="EmpMaster" class="panel panel-default">
                <div class="panel-heading">
                    Employee Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblGoToScrolling" runat="server" class="control-label" Text="Go To Scrolling"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGoToScrolling" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div id="code" runat="server">
                                <div class="col-md-2">
                                    <asp:Label ID="lblEmpCode" runat="server" class="control-label required-field" Text="Employee ID"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:HiddenField ID="hfEmpCode" runat="server" />
                                    <asp:TextBox ID="txtEmpCode" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTitle" runat="server" class="control-label" Text="Title"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTitle" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="">----Please Select----</asp:ListItem>
                                    <asp:ListItem Value="Mr.">Mr.</asp:ListItem>
                                    <asp:ListItem Value="Md.">Md.</asp:ListItem>
                                    <asp:ListItem Value="Ms.">Ms.</asp:ListItem>
                                    <asp:ListItem Value="Mrs.">Mrs.</asp:ListItem>
                                    <asp:ListItem Value="Miss">Miss</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFirstName" runat="server" class="control-label required-field"
                                    Text="First Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFirstName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLastName" runat="server" class="control-label" Text="Last Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLastName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDisplayName" runat="server" class="control-label" Text="Full Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" disabled></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmpJoinDate" runat="server" class="control-label required-field"
                                    Text="Join Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmpJoinDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDepartmentId" runat="server" class="control-label required-field"
                                    Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmpCategoryId" runat="server" class="control-label required-field"
                                    Text="Employee Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEmpCategoryId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div id="ContractEndDateDiv" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblContractEndDate" runat="server" class="control-label required-field"
                                        Text="Contract End Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContractEndDate" CssClass="form-control" runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="hftxtContractEndDate" runat="server" />
                                </div>
                            </div>
                            <div id="ProvisionPeriodDiv" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblProvisionPeriod" runat="server" class="control-label" Text="Probation Period"></asp:Label>
                                </div>
                                <div class="col-md-1">
                                    <asp:CheckBox ID="chkIsProvisionPeriod" runat="server" Text="" onclick="javascript: return ToggleFieldVisible();"
                                        TabIndex="9" />
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtProvisionPeriod" CssClass="form-control" Width="190px" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblGradeId" runat="server" class="control-label" Text="Employee Grade"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGradeId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Employee Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEmployeeStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDesignationId" runat="server" class="control-label required-field"
                                    Text="Designation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDesignationId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2" id="EmployeeCompanyLabelDiv">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field"
                                    Text="Employee Company"></asp:Label>
                            </div>
                            <div class="col-md-4" id="EmployeeCompanyControlDiv">
                                <asp:DropDownList ID="ddlEmpCompanyId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="PayrollWorkStationDiv" runat="server">
                            <div class="col-md-2">
                                <asp:Label ID="lblWorkStation" runat="server" class="control-label" Text="Work Station"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlWorkStation" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="CostCenterDiv" runat="server">
                            <div class="col-md-2">
                                <asp:Label ID="lblCostCenter" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="PayrollDonorNameAndActivityCodeHideDiv" runat="server">
                            <div class="col-md-2">
                                <asp:Label ID="lblDonor" runat="server" class="control-label" Text="Donor Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDonor" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblActivityCode" runat="server" class="control-label" Text="Activity Code"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtActivityCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReportingTo" runat="server" class="control-label" Text="Reporting To (1)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlReportingTo" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Reporting To (2)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlReportingTo2" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblOfficialEmail" runat="server" class="control-label"
                                    Text="Official Email"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtOfficialEmail" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label23" runat="server" class="control-label"
                                    Text="PABX Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPABXNumber" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Job Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <UserControl:companyProjectUserControl ID="companyProjectUserControl" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Payroll Currency"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPayrollCurrencyId" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label20" runat="server" class="control-label" Text="Not Effect On"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlNotEffectOnHead" runat="server" CssClass="form-control" TabIndex="3">
                                    <asp:ListItem Value="0">--- Not Applicable ---</asp:ListItem>
                                    <asp:ListItem Value="1">Attendance</asp:ListItem>
                                    <asp:ListItem Value="2">Salary</asp:ListItem>
                                    <asp:ListItem Value="3">Attendance & Salary</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="IsProvidentFundDeductHideDiv" runat="server">
                            <div class="col-md-2">
                                <asp:Label ID="Label24" runat="server" class="control-label" Text="Is PF Deduct?"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsProvidentFundDeduct" runat="server" CssClass="form-control" TabIndex="3">
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="EmployeeDetailsInformation" class="panel panel-default">
                <div class="panel-heading">
                    Employee Details Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFathersName" runat="server" class="control-label" Text="Father's Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtFathersName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMothersName" runat="server" class="control-label" Text="Mother's Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtMothersName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmpDateOfBirth" runat="server" class="control-label" Text="Date Of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmpDateOfBirth" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblGender" runat="server" class="control-label required-field" Text="Gender"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblBloodGroup" runat="server" class="control-label" Text="Blood Group"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblReligion" runat="server" class="control-label required-field" Text="Religion"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblHeight" runat="server" class="control-label" Text="Employee Height (Foot) "></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHeight" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblMaritalStatus" runat="server" class="control-label required-field"
                                    Text="Marital Status"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlMaritalStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div id="marriage" name="marriage" style="display: none">
                                <%--<div class="col-md-1">
                                <asp:Label ID="Label25" runat="server"  class="control-label required-field" Text="Date Of Marriage"></asp:Label>
                            </div>--%>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtDateOfMarriage" CssClass="form-control" runat="server" placeholder="Date Of Marriage"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <%--<div class="form-group" id="marriage" name="marriage" style="display:none">
                             <div class="col-md-2">
                                <asp:Label ID="Label25" runat="server"  class="control-label required-field" Text="Date Of Marriage"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDateOfMarriage"  CssClass="form-control" runat="server" ></asp:TextBox>
                            </div>
                        </div>--%>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblNatinality" runat="server" class="control-label required-field"
                                    Text="Nationality"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCountryId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblNationalId" runat="server" class="control-label" Text="National Id/SSN"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNationalId" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <asp:Panel ID="pnlDivisionDistrictThana" runat="server">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblDivision" runat="server" class="control-label" Text="Division"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblDistrict" runat="server" class="control-label" Text="District"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblThana" runat="server" class="control-label" Text="Thana/Upazilla"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlThana" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPassportNumber" runat="server" class="control-label" Text="Passport Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassportNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPIssueDate" runat="server" class="control-label" Text="Pass Issue Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPIssueDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPIssuePlace" runat="server" class="control-label" Text="Pass Issue Place"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPIssuePlace" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPExpireDate" runat="server" class="control-label" Text="Pass. Expiry Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentAddress" runat="server" class="control-label" Text="Present Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPresentAddress" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentCity" runat="server" class="control-label" Text="Present City"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPresentCity" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentZipCode" runat="server" class="control-label" Text="Present Zip/ P.O. Box"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPresentZipCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentCountry" runat="server" class="control-label" Text="Present Country"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPresentCountry" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentPhone" runat="server" class="control-label" Text="Official Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPresentPhone" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentAddress" runat="server" class="control-label" Text="Permanent Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPermanentAddress" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentCity" runat="server" class="control-label" Text="Permanent City"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPermanentCity" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentZipCode" runat="server" class="control-label" Text="Per. Zip/ P.O. Box"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPermanentZipCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentCountry" runat="server" class="control-label" Text="Permanent Country"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPermanentCountry" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentPhone" runat="server" class="control-label" Text="Personal Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPermanentPhone" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPersonalEmail" runat="server" class="control-label" Text="Personal Email"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPersonalEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAlternativeEmail" runat="server" class="control-label" Text="Alternative Email"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAlternativeEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label11" runat="server" class="control-label " Text="Emerg. Contact Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmergencyContactName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label13" runat="server" class="control-label" Text="Relationship"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmergencyContactRelationship" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label14" runat="server" class="control-label" Text="Emerg. Contact (Home)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmergencyContactNumberHome" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label runat="server" class="control-label" Text="Emerg. Contact (Work)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmergencyContactNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label15" runat="server" class="control-label" Text="Emerg. Contact Email"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmergencyContactEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                            <div class="col-md-2">
                                <asp:Label ID="lblTinNumber" runat="server" class="control-label" Text="TIN Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTinNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
            <div id="EducationInformation" class="panel panel-default">
                <div class="panel-heading">
                    Education Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblExamLevel" runat="server" class="control-label required-field"
                                    Text="Level"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlExamLevel" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblExamName" runat="server" class="control-label required-field" Text="Degree"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtExamName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblInstituteName" runat="server" class="control-label required-field"
                                    Text="Institute Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtInstituteName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSubjectName" runat="server" class="control-label" Text="Subject Name (Major)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSubjectName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPassYear" runat="server" class="control-label required-field" Text="Passing Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassYear" CssClass="form-control quantity" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPassClass" runat="server" class="control-label required-field"
                                    Text="Result (Class/GPA)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassClass" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpEducation" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnEmpEducation_Click" />
                                <asp:Label ID="hfEducationId" runat="server" class="control-label" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvEmpEducation" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="5" OnRowCommand="gvEmpEducation_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("EducationId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ExamName" HeaderText="Exam Name" ItemStyle-Width="51%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PassYear" HeaderText="PassYear" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PassClass" HeaderText="PassClass" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# Bind("EducationId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# Bind("EducationId") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to save?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
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
        </div>
        <div id="tab-4">
            <div id="ExperienceInformation" class="panel panel-default">
                <div class="panel-heading">
                    Experience Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCompanyName" runat="server" class="control-label required-field"
                                    Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCompanyName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCompanyUrl" runat="server" class="control-label" Text="Company Website"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCompanyUrl" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblJoinDate" runat="server" class="control-label required-field" Text="Join Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtJoinDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblJoinDesignation" runat="server" class="control-label" Text="Join Designation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtJoinDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveDate" runat="server" class="control-label required-field"
                                    Text="Leave Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLeaveDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveDesignation" runat="server" class="control-label" Text="Leave Designation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLeaveDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAchievements" runat="server" class="control-label" Text="Achievements"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAchievements" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpExperience" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnEmpExperience_Click" />
                                <asp:Label ID="hfExperienceId" runat="server" class="control-label" Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvExperience" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                OnRowCommand="gvExperience_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("ExperienceId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CompanyName" HeaderText="Company" ItemStyle-Width="51%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShowJoinDate" HeaderText="Join Date" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShowLeaveDate" HeaderText="Leave Date" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# Bind("ExperienceId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# Bind("ExperienceId") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to save?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
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
        </div>
        <div id="tab-5">
            <div id="DependentInformation" class="panel panel-default">
                <div class="panel-heading">
                    Dependent Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDependentName" runat="server" class="control-label required-field"
                                    Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDependentName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDateOfBirth" runat="server" class="control-label required-field" Text="Date Of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDateOfBirth" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Age"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfAge" runat="server" />
                                <asp:TextBox ID="txtAge" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRelationship" runat="server" class="control-label required-field"
                                    Text="Relationship"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRelationship" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label16" runat="server" class="control-label"
                                    Text="Blood Group"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBloodGroupDepen" runat="server" CssClass="form-control">
                                </asp:DropDownList>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpDependent" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnEmpDependent_Click" OnClientClick="javascript:return CheckAgeEligibility()" />
                                <asp:Label ID="lblHiddenDependentId" runat="server" class="control-label" Text=''
                                    Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvDependent" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                OnRowCommand="gvDependent_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("DependentId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DependentName" HeaderText="Name" ItemStyle-Width="50%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Relationship" HeaderText="Relationship" ItemStyle-Width="35%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# Bind("DependentId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# Bind("DependentId") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to delete?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
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
        </div>
        <div id="tab-6">
            <div id="Div3" class="panel panel-default">
                <div class="panel-heading">
                    Beneficiary Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtNomineeName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="Date Of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNomineeDateOfBirth" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label9" runat="server" class="control-label" Text="Age"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfNomineeAge" runat="server" />
                                <asp:TextBox ID="txtNomineeAge" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label10" runat="server" class="control-label required-field" Text="Relationship"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNomineeRelationship" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label12" runat="server" class="control-label required-field" Text="Percentage"></asp:Label>
                                <span>(%)</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPercentage" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAddNominee" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnAddNominee_Click" OnClientClick="javascript:return ValidateNomineePerentage()" />
                                <asp:Label ID="lblNomineeId" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvNominee" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                OnRowCommand="gvNominee_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("NomineeId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NomineeName" HeaderText="Name" ItemStyle-Width="45%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Relationship" HeaderText="Relationship" ItemStyle-Width="25%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Percentage" HeaderText="Percentage" ItemStyle-Width="15%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NomineeId" HeaderText="NomineeId" ItemStyle-Width="15%" ItemStyle-CssClass="HideTag" HeaderStyle-CssClass="HideTag">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# Bind("NomineeId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# Bind("NomineeId") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to delete?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
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
        </div>
        <div id="tab-7">
            <div id="Div7" class="panel panel-default">
                <div class="panel-heading">
                    Reference Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReferenceName" runat="server" class="control-label required-field" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtReferenceName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblOrganization" runat="server" class="control-label" Text="Organization"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtOrganization" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDesignation" runat="server" class="control-label" Text="Designation"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAddress" runat="server" class="control-label" Text="Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAddress" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMobile" runat="server" class="control-label" Text="Mobile"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMobile" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblEmail" runat="server" class="control-label" Text="Email"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReferenceRelationship" runat="server" class="control-label required-field" Text="Relationship"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtReferenceRelationship" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox TextMode="MultiLine" ID="txtDescription" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAddReference" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnAddReference_Click" OnClientClick="javascript:return ValidateReferencePerson()" />
                                <asp:Label ID="lblReferenceId" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvReference" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                OnRowCommand="gvReference_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("ReferenceId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="25%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Relationship" HeaderText="Relationship" ItemStyle-Width="25%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="30%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# Bind("ReferenceId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" OnClientClick="return confirm('Do you want to edit?');"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# Bind("ReferenceId") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to delete?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
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
        </div>
        <div id="tab-8">
            <asp:GridView ID="gvBenefit" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="1000"
                CssClass="table table-bordered table-condensed table-responsive">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblBenefitHeadId" runat="server" Text='<%#Eval("BenefitHeadId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="02%">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="BenefitName" HeaderText="Benefit" ItemStyle-Width="65%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Effective From" ShowHeader="False" ItemStyle-Width="30%">
                        <ItemTemplate>
                            <asp:TextBox ID="txtBenefitEffectiveDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </ItemTemplate>
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
        <div id="tab-9">
            <div id="Div1" class="panel panel-default">
                <div class="panel-heading">
                    Bank Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblBankId" runat="server" class="control-label" Text="Bank Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control" TabIndex="13">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblBranchName" runat="server" class="control-label" Text="Branch Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBranchName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblAccountType" runat="server" class="control-label" Text="Account Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAccountType" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAccountName" runat="server" class="control-label" Text="Account Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAccountName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblAccountNumber" runat="server" class="control-label" Text="Account Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAccountNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRouteNumber" runat="server" class="control-label" Text="Route Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRouteNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblCardNumber" runat="server" class="control-label" Text="Card Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCardNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarksForBankInfo" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarksForBankInfo" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-10">
            <div id="DivLetter" class="panel panel-default">
                <div class="panel-heading">
                    Letter Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label22" runat="server" class="control-label" Text="Appoinment Letter"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAppoinmentLetter" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="Label21" runat="server" class="control-label" Text="Confirmation Letter"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtConfirmationLetter" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label17" runat="server" class="control-label" Text="Joining Agreement"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtJoiningAgreement" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label18" runat="server" class="control-label" Text="Service Bond"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtServiceBond" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label19" runat="server" class="control-label" Text="DSOAC"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDSOAC" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-11">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Employee Documents
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:HiddenField ID="RandomEmpId" runat="server" />
                            <asp:HiddenField ID="tempEmpId" runat="server" />
                            <div class="col-md-2">
                                <asp:Label ID="lblSignatureImage" runat="server" class="control-label" Text="Employee Signature"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnSignatureUp" type="button" onclick="javascript: return LoadSignatureUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Upload Signature" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDocumentImage" runat="server" class="control-label" Text="Employee Picture"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnDocumentUp" type="button" onclick="javascript: return LoadDocumentUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Upload Picture" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <div id="DocDiv" style="width: 150px; height: 150px" runat="server">
                                </div>
                            </div>
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <div id="SigDiv" style="width: 150px; height: 150px" runat="server">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Attachment</label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnImageUp" type="button" onclick="javascript: return LoadOthersDocumentUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Others Document..." />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <div id="OthersDocDiv" style="width: 150px; height: 150px">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="block-body collapse in">
                        <div class="form-group">
                            <div id="DocumentInfo" runat="server" class="col-md-12">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-13">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:srcCompanyProjectUserControl ID="srcCompanyProjectUserControl" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmployeeCode" runat="server" class="control-label" Text="Employee ID"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmployeeCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDepartment" runat="server" class="control-label" Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmployeeDepartment" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmployeeName" runat="server" class="control-label" Text="Employee Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmployeeName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblEmployeeDesignation" runat="server" class="control-label" Text="Designation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmployeeDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm">
                                    Search</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="SearchOutput">
                    <div class="panel-body">
                        <table id='gvEmployee' class="table table-bordered table-condensed table-responsive"
                            width="100%">
                            <colgroup>
                                <col style="width: 5%;" />
                                <col style="width: 30%;" />
                                <col style="width: 10%;" />
                                <col style="width: 15%;" />
                                <col style="width: 15%;" />
                                <col style="width: 20%;" />
                            </colgroup>
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                    <td>#
                                    </td>
                                    <td>Name
                                    </td>
                                    <td>Code
                                    </td>
                                    <td>Department
                                    </td>
                                    <td>Designation
                                    </td>
                                    <td style="text-align: right;">Action
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                        <div class="childDivSection">
                            <div class="text-center" id="GridPagingContainer">
                                <ul class="pagination">
                                </ul>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-10">
                                <asp:Label ID="lblEmployeeNumberCount" runat="server" class="control-label" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
        </div>
    </div>
    <div class="row" id="SubmitButtonDiv" style="padding-top: 10px;">
        <div class="col-md-12">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                OnClick="btnSave_Click" OnClientClick="javascript:return CheckValidity()" />
            <asp:Button ID="btnCancel" OnClientClick="return confirm('Do you want to clear?');" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                OnClick="btnCancel_Click" />
        </div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="popUpSignature" style="display: none;">
        <iframe id="Iframe1" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="popUpDocument" style="display: none;">
        <iframe id="Iframe2" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
</asp:Content>
