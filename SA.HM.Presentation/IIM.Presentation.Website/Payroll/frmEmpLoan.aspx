<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpLoan.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpLoan" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLoanSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        var vv = [];
        $(document).ready(function () {
            CommonHelper.ApplyIntigerValidation();
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            if (IsCanSave) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Loan Sanction</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlLoanPaymentFromAccountHeadId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val() == "1") {
                PerformFillFormAction($("#ContentPlaceHolder1_hfLoanId").val(), 1);
                $("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val("0");
            }

            $("#ContentPlaceHolder1_txtLoanDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_ddlCheckedBy").select2();
            $("#ContentPlaceHolder1_ddlApprovedBy").select2();
            $("#ContentPlaceHolder1_txtLoanAmount").blur(function () {

                //if (CommonHelper.IsDecimal($("#ContentPlaceHolder1_txtLoanAmount").val()) == false) {
                //    toastr.info("Loan amount must be number");
                //    //$("#ContentPlaceHolder1_txtLoanAmount").focus();
                //    $("#ContentPlaceHolder1_txtLoanAmount").val('');

                //    return;
                //}

                if ($("#ContentPlaceHolder1_txtLoanAmount").val() != "") {
                    InterestRateCalculation();
                }
            });

            $("#ContentPlaceHolder1_txtLoanTakenForPeriod").blur(function () {
                if ($("#ContentPlaceHolder1_txtLoanAmount").val() != "") {
                    if (CommonHelper.IsDecimal($("#ContentPlaceHolder1_txtLoanAmount").val()) == true) {
                        InterestRateCalculation();
                    }
                }
            });

            $("#ContentPlaceHolder1_ddlLoanTakenForMonthOrYear").change(function () {
                if (CommonHelper.IsDecimal($("#ContentPlaceHolder1_txtLoanAmount").val()) == false) {
                    toastr.info("Loan amount must be number");
                    return;
                }
                else if ($("#ContentPlaceHolder1_txtLoanAmount").val() != "") {
                    InterestRateCalculation();
                }
            });

            $("#ContentPlaceHolder1_btnSearch").click(function () {
                GridPaging(1, 1);
                return false;
            });

            $("#ContentPlaceHolder1_ddlLoanType").change(function (data) {
                PageMethods.GetEmpLoanInformation($(this).val(), OnSucceedLoanInfo, OnFailedLoanInfo);
            });

            $("#btnLoanApplicationForm").click(function () {
                GenerateLoanApplicationForm();
            });

            $("#btnLoanApplicationForm").hide();

            $("#ContentPlaceHolder1_btnCancel").click(function () {
                clear();
            });
            $("#ContentPlaceHolder1_employeeSearch_btnSrcEmployees").click(function () {
                var empId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
                if (empId != "0") {
                    $("#btnLoanApplicationForm").show();
                }

            });
            CommonHelper.ApplyDecimalValidation();

            $("#<%=txtLoanDate.ClientID %>").blur(function () {

                var date = $("#<%=txtLoanDate.ClientID %>").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#<%=txtLoanDate.ClientID %>").focus();
                        $("#<%=txtLoanDate.ClientID %>").val("");
                        return false;
                    }
                }
            });

        });
        function clear() {
            $("#ContentPlaceHolder1_employeeSearch_txtSearchEmployee").val('');
            $("#ContentPlaceHolder1_employeeSearch_txtEmployeeName").val('');
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val('0');
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeName").val('');

            $("#ContentPlaceHolder1_ddlLoanType").val('CompanyLoan');
            //$("#ContentPlaceHolder1_ddlLoanTakenForPeriod").val('1');
            $("#ContentPlaceHolder1_txtLoanTakenForPeriod").val('');
            $("#ContentPlaceHolder1_ddlLoanTakenForMonthOrYear").val('year');
            $("#ContentPlaceHolder1_txtLoanAmount").val('');

            $("#ContentPlaceHolder1_txtInterestAmount").val('');
            $("#ContentPlaceHolder1_txtPerInstallLoanAmount").val('');
            $("#ContentPlaceHolder1_txtPerInstallInterestAmount").val('');
            $("#ContentPlaceHolder1_txtTotalInstallAmount").val('');

            $("#ContentPlaceHolder1_hfInterestAmount").val('0');
            $("#ContentPlaceHolder1_hfPerInstallLoanAmount").val('0');
            $("#ContentPlaceHolder1_hfPerInstallInterestAmount").val('0');

            $("#ContentPlaceHolder1_txtLoanDate").val('');

            $("#ContentPlaceHolder1_ddlCheckedBy").val('0');
            $("#ContentPlaceHolder1_ddlApprovedBy").val('0');
        }
        function GenerateLoanApplicationForm() {

            var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            var loanAmount = $('#ContentPlaceHolder1_txtLoanAmount').val();
            var modeofReturn = $('#ContentPlaceHolder1_ddlLoanTakenForMonthOrYear').val();
            var purpose = $('#ContentPlaceHolder1_txtRemarks').val();
            var loanType = $('#ContentPlaceHolder1_ddlLoanType').val();

            var url = "/Payroll/Reports/frmLoanApplication.aspx?EmployeeId=" + employeeId + "," + loanAmount + "," + modeofReturn + "," + purpose + "," + loanType;

            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=760,height=780,left=300,top=50,resizable=yes");
        }

        function InterestRateCalculation() {
            if ($("#ContentPlaceHolder1_txtLoanTakenForPeriod").val() == "")
            {
                $("#ContentPlaceHolder1_txtLoanTakenForPeriod").val("1");
            }
            var loanPeriod = parseInt($("#ContentPlaceHolder1_txtLoanTakenForPeriod").val());
            var loanMonthRYear = $("#ContentPlaceHolder1_ddlLoanTakenForMonthOrYear").val();
            var loanAmount = $('#ContentPlaceHolder1_txtLoanAmount').val();
            var loanInterest = $("#ContentPlaceHolder1_lblInterestRate").text();
            var length = loanInterest.length;
            loanInterest = loanInterest.substring(2, length - 2);
            
            var interestAmount = 0;
            var loanInstallAmount = 0;
            var interestInstallAmount = 0;
            interestAmount = ((loanAmount) * (loanInterest / 100)).toFixed(2);

            if (loanMonthRYear == "year") {
                interestAmount *= loanPeriod;
            }
            else {
                //interestAmount /= 12;
                interestAmount /= loanPeriod;
                interestAmount *= loanPeriod;
            }

            var totalInstallmentNumber = 0;
            if (loanMonthRYear == "year") {
                totalInstallmentNumber = loanPeriod * 12;
            }
            else {
                totalInstallmentNumber = loanPeriod;
            }
                        
            loanInstallAmount = +((loanAmount) / (totalInstallmentNumber)).toFixed(2);
            interestInstallAmount = +((interestAmount) / (totalInstallmentNumber)).toFixed(2);

            var totalInstAmount = 0;
            totalInstAmount = +(loanInstallAmount + interestInstallAmount).toFixed(2);
            $("#ContentPlaceHolder1_txtTotalInstallAmount").val(totalInstAmount);

            $("#ContentPlaceHolder1_txtInterestAmount").val(interestAmount);
            $("#ContentPlaceHolder1_txtPerInstallLoanAmount").val(loanInstallAmount);
            $("#ContentPlaceHolder1_txtPerInstallInterestAmount").val(interestInstallAmount);

            $("#ContentPlaceHolder1_hfInterestAmount").val(interestAmount);
            $("#ContentPlaceHolder1_hfPerInstallLoanAmount").val(loanInstallAmount);
            $("#ContentPlaceHolder1_hfPerInstallInterestAmount").val(interestInstallAmount);
        }

        function OnSucceedLoanInfo(result) {
            $("#ContentPlaceHolder1_lblInterestRate").text(" (" + result + "%)");
            InterestRateCalculation();
        }

        function OnFailedLoanInfo(error) { alert(error); }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var employeeId = "", loanType = "", loanStatus = "";

            employeeId = $("#ContentPlaceHolder1_employeeForLoanSearch_hfEmployeeId").val();
            loanType = $("#ContentPlaceHolder1_ddlSLoanType").val();
            loanStatus = $("#ContentPlaceHolder1_ddlSLoanStatus").val();

            if (employeeId == "0")
                employeeId = "";

            var gridRecordsCount = $("#tblLoanSearch tbody tr").length;

            PageMethods.LoadEmployeeLoanInfo(employeeId, loanType, loanStatus, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoanLoadSucceed, OnLoanLoadFailed);
            return false;
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function OnLoanLoadSucceed(result) {

            $("#tblLoanSearch tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
                $("#tblLoanSearch tbody").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", cancelLink = "", holdupLink = "", collectLink = "", takenPeriodDisplay = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#tblLoanSearch tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                if (gridObject.LoanTakenForPeriod > 1) {
                    takenPeriodDisplay = gridObject.LoanTakenForPeriod + " " + gridObject.LoanTakenForMonthOrYear + "s";
                }
                else {
                    takenPeriodDisplay = gridObject.LoanTakenForPeriod + " " + gridObject.LoanTakenForMonthOrYear
                }

                tr += "<td align='left' style=\"width:15%;\">" + gridObject.EmployeeName + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.LoanType + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + takenPeriodDisplay + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.LoanAmount + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.InterestRate + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.InterestAmount + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.DueAmount + "</td>";
                tr += "<td align='center' style=\"width:20%;\">"
                if (gridObject.IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return PerformFillFormAction(" + gridObject.LoanId + "," + result.GridPageLinks.CurrentPageNumber + ")\" alt='Edit'  title='Edit' border='0' />";
                }
                if (gridObject.IsCanDelete) {
                    cancelLink = "&nbsp;<a href=\"javascript:void();\"  title=\"Cancel\" onclick=\"javascript:return CancelLoan(" + gridObject.LoanId + "," + gridObject.EmpId + "," + result.GridPageLinks.CurrentPageNumber + ");\"> <img alt=\"Cancel\" src=\"../Images/cancel.png\" /> </a>";
                }
                if (gridObject.IsCanChecked) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return LoanApprovalWithConfirmation('" + 'Checked' + "'," + gridObject.LoanId + ")\" alt='Checked'  title='Checked' border='0' />";

                }
                if (gridObject.IsCanApproved) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return LoanApprovalWithConfirmation('" + 'Approved' + "', " + gridObject.LoanId + ")\" alt='Approved'  title='Approved' border='0' />";
                }
                if (gridObject.ApprovedStatus == "Cancel") {
                    tr = "&nbsp;<a href=\"javascript:void();\"  title=\"Update Cancel\" onclick=\"javascript:return UpdateCancelLoan(" + gridObject.LoanId + "," + gridObject.EmpId + "," + result.GridPageLinks.CurrentPageNumber + ");\"> <img alt=\"Make Pending\" src=\"../Images/approved.png\" /> </a>";
                }
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"ShowDocument('" + gridObject.LoanId + "');\"> <img alt='Document' src='/Images/document.png' title='document' /></a>";
                tr += "</tr>"

                $("#tblLoanSearch tbody").append(tr);
                tr = ""; //editLink = ""; cancelLink = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
        }
        function OnLoanLoadFailed(error) {

        }
        function LoanApprova(ApprovedStatus, LoanId) {

            PageMethods.LoanApprova(LoanId, ApprovedStatus, OnApprovalSucceed, OnApprovalFailed);
        }
        function LoanApprovalWithConfirmation(ApprovedStatus, LoanId) {
            if (!confirm('Do you want to ' + (ApprovedStatus == 'Checked' ? 'check' : 'approve') + " ?")) {
                return false;
            }
            LoanApprova(ApprovedStatus, LoanId);
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging($("#GridPagingContainer").find("li.active").index(), 1);                
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }
        function PerformFillFormAction(loanId, currentPageNumber) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val(currentPageNumber);
            PageMethods.FillForm(loanId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {

            vv = result;
            if (IsCanEdit) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }

            $("#<%=btnSave.ClientID %>").val("Update");

            $("#<%=hfLoanId.ClientID %>").val(result.LoanId);
            $("#ContentPlaceHolder1_employeeSearch_txtSearchEmployee").val(result.EmpCode);
            $("#ContentPlaceHolder1_employeeSearch_txtEmployeeName").val(result.EmployeeName);
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val(result.EmpId);
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeName").val(result.EmployeeName);

            $("#ContentPlaceHolder1_ddlLoanType").val(result.LoanType);
            //$("#ContentPlaceHolder1_ddlLoanTakenForPeriod").val(result.LoanTakenForPeriod);
            $("#ContentPlaceHolder1_txtLoanTakenForPeriod").val(result.LoanTakenForPeriod);
            $("#ContentPlaceHolder1_ddlLoanTakenForMonthOrYear").val(result.LoanTakenForMonthOrYear);
            $("#ContentPlaceHolder1_txtLoanAmount").val(result.LoanAmount);
            $("#ContentPlaceHolder1_txtRemarks").val(result.Remarks);

            $("#ContentPlaceHolder1_txtInterestAmount").val(result.InterestAmount);
            $("#ContentPlaceHolder1_txtPerInstallLoanAmount").val(result.PerInstallLoanAmount);
            $("#ContentPlaceHolder1_txtPerInstallInterestAmount").val(result.PerInstallInterestAmount);
            $("#ContentPlaceHolder1_txtTotalInstallAmount").val(result.PerInstallLoanAmount + result.PerInstallInterestAmount);

            $("#ContentPlaceHolder1_hfInterestAmount").val(result.InterestAmount);
            $("#ContentPlaceHolder1_hfPerInstallLoanAmount").val(result.PerInstallLoanAmount);
            $("#ContentPlaceHolder1_hfPerInstallInterestAmount").val(result.PerInstallInterestAmount);

            $("#ContentPlaceHolder1_ddlLoanPaymentFromAccountHeadId").val(result.LoanPaymentFromAccountHeadId);

            $("#ContentPlaceHolder1_txtLoanDate").val(GetStringFromDateTime(result.LoanDate));

            $("#ContentPlaceHolder1_ddlCheckedBy").val(result.CheckedBy).trigger('change');
            $("#ContentPlaceHolder1_ddlApprovedBy").val(result.ApprovedBy).trigger('change');
            ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());
            $('#EntryPanel').show();
            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });

            return false;
        }
        function OnFillFormObjectFailed(error) {
            alert(error.get_message());
        }

        function SaveUpdate() {
            if (ValidityCheck() == true) {
                if ($("#<%=hfLoanId.ClientID %>").val() != "") {
                    UpdateLoan();
                }
                else {
                    return true;
                }
            }
            return false;
        }
        var vvcd = [];
        function UpdateLoan() {
            var LoanId = $("#<%=hfLoanId.ClientID %>").val();
            var EmpCode = $("#ContentPlaceHolder1_employeeSearch_txtSearchEmployee").val();
            var EmployeeName = $("#ContentPlaceHolder1_employeeSearch_txtEmployeeName").val();
            var EmpId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            var EmployeeName = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeName").val();

            var LoanType = $("#ContentPlaceHolder1_ddlLoanType").val();
            //var LoanTakenForPeriod = $("#ContentPlaceHolder1_ddlLoanTakenForPeriod").val();
            var LoanTakenForPeriod = $("#ContentPlaceHolder1_txtLoanTakenForPeriod").val();
            var LoanTakenForMonthOrYear = $("#ContentPlaceHolder1_ddlLoanTakenForMonthOrYear").val();
            var LoanAmount = $("#ContentPlaceHolder1_txtLoanAmount").val();

            var interestRate = $("#ContentPlaceHolder1_lblInterestRate").text().substr(2, 1);
            var InterestAmount = $("#ContentPlaceHolder1_txtInterestAmount").val();
            var PerInstallLoanAmount = $("#ContentPlaceHolder1_hfPerInstallLoanAmount").val();
            var PerInstallInterestAmount = $("#ContentPlaceHolder1_hfPerInstallInterestAmount").val();
            //var LoanDate = $("#ContentPlaceHolder1_txtLoanDate").val();
            var LoanDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtLoanDate").val(), innBoarDateFormat);

            var checkedBy = $("#ContentPlaceHolder1_ddlCheckedBy").val();
            var approvedBy = $("#ContentPlaceHolder1_ddlApprovedBy").val();

            var loan = {
                LoanId: LoanId,
                EmpId: EmpId,
                LoanNumber: "",
                LoanType: LoanType,
                LoanAmount: LoanAmount,
                InterestRate: interestRate,
                InterestAmount: InterestAmount,
                DueAmount: LoanAmount,
                DueInterestAmount: InterestAmount,
                LoanTakenForPeriod: LoanTakenForPeriod,
                LoanTakenForMonthOrYear: LoanTakenForMonthOrYear,
                PerInstallLoanAmount: PerInstallLoanAmount,
                PerInstallInterestAmount: PerInstallInterestAmount,
                LoanDate: LoanDate,
                CheckedBy: checkedBy,
                ApprovedBy: approvedBy
            };
            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();

            PageMethods.UpdateEmpLoan(loan, checkedBy, approvedBy, parseInt(randomDocId), deletedDoc, OnUpdateLoanSucceeded, OnUpdateLoanFailed);
        }

        var ms = [];

        function OnUpdateLoanSucceeded(result) {
            ms = result;
            CommonHelper.AlertMessage(result.AlertMessage);
            clear();
            ReloadGrid($("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val());
            PerformClearAction();
            $("#ContentPlaceHolder1_RandomDocId").val(result.Data);           
        }

        function OnUpdateLoanFailed(error) {
            toastr.error(error);
        }

        function ValidityCheck() {

            if ($("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val() == "0") {
                toastr.warning("Please select employee");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtLoanAmount").val() == "") {
                toastr.warning("Please give loan amount");
                return false;
            }
            else if (CommonHelper.IsDecimal($("#ContentPlaceHolder1_txtLoanAmount").val()) == false) {
                toastr.warning("Loan amount must be number");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtLoanDate").val() == "") {
                toastr.warning("Please give loan date");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlLoanPaymentFromAccountHeadId").val() == "0") {
                toastr.warning("Please select Loan From");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtRemarks").val() == "") {
                toastr.warning("Please give loan Remarks");
                return false;
            }
            else if ($("#ContentPlaceHolder1_approvedByEmployee_hfEmployeeId").val() == "0") {
                toastr.warning("Please give loan approved by");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtLoanTakenForPeriod").val() == "" || $("#ContentPlaceHolder1_txtLoanTakenForPeriod").val() == "0") {
                toastr.warning("Please give Loan Taken Period");
                return false;
            }            
            else {
                return true;
            }
            return false;
        }

        function CancelLoan(loanId, empId) {
            if (confirm("Do you want to cancel?")) {
                var updattype = "Cancel";
                PageMethods.ChangeLoanStatus(loanId, empId, updattype, OnCancelLoanSucceed, OnCancelLoanFailed);
            }
        }
        function UpdateCancelLoan(loanId, empId) {
            if (confirm("Do you want to update it?")) {
                var updattype = "Pending";
                PageMethods.ChangeLoanStatus(loanId, empId, updattype, OnCancelLoanSucceed, OnCancelLoanFailed);
            }
        }

        function OnCancelLoanSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            GridPaging(1, 1);
            return false;
        }
        function OnCancelLoanFailed() { }

        function PerformClearAction() {

            $("#frmHotelManagement")[0].reset();

            $("#<%=btnSave.ClientID %>").val("Save");

            $("#<%=hfLoanId.ClientID %>").val("");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeName").val("");
            $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val("0");

            return false;
        }

        function WorkAfterSearchEmployee() { }
        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/Payroll/Images/Loan/";
            var category = "LoanDocuments";
            var iframeid = 'frmPrint';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
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

        }

        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocument(randomId);
        }

        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfLoanId").val();
            if (id == "") {
                id = "0";
            }
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.GetUploadedDocByWebMethod(randomId, id, deletedDoc, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
        }

        function OnGetUploadedDocByWebMethodSucceeded(result) {
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
                DocTable += "<td align='left' style='width: 50%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                DocTable += "<td align='left' style='width: 20%'>";
                DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                DocTable += "</td>";
                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#DocumentInfo").html(DocTable);

            return false;
        }

        function DeleteDoc(docId, rowIndex) {
            var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();

            if (deletedDoc != "")
                deletedDoc += "," + docId;
            else
                deletedDoc = docId;

            $("#<%=hfDeletedDoc.ClientID %>").val(deletedDoc);

            $("#trdoc" + rowIndex).remove();
        }
        function OnGetUploadedDocByWebMethodFailed(error) {
            alert(error.get_message());
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
        function ShowDocument(id) {
            PageMethods.LoadLoanDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#LoanDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: "70%",
                height: 300,
                closeOnEscape: true,
                resizable: false,
                title: "Loan Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfLoanId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsCurrentOrPreviousPage" runat="server" Value="" />
    <asp:HiddenField ID="hfIsEditedFromApprovedForm" runat="server" Value="0" />

    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <div id="LoanDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="loanEntry" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-1">Loan Information</a></li>
            <li id="loanSearch" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                <a href="#tab-2">Search Loan</a></li>
        </ul>
        <div id="tab-1">
            <div id="EmpLoan" class="panel panel-default">
                <div class="panel-heading">
                    Loan Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeSearch ID="employeeSearch" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLoanType" runat="server" class="control-label" Text="Loan Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLoanType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Company Loan" Value="CompanyLoan"></asp:ListItem>
                                    <asp:ListItem Text="PF Loan" Value="PFLoan"> </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLoanAmount" runat="server" class="control-label required-field"
                                    Text="Loan Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLoanAmount" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                            </div>                            
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLoanTakenForPeriod" runat="server" class="control-label required-field" Text="Loan For Period"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLoanTakenForPeriod" CssClass="form-control quantity" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label Style="text-align: left" ID="lblLoanTakenForMonthOrYear" runat="server" class="control-label required-field"
                                    Text="Loan For Month/Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLoanTakenForMonthOrYear" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Month" Value="month"></asp:ListItem>
                                    <asp:ListItem Text="Year" Value="year"> </asp:ListItem>                                    
                                </asp:DropDownList>
                            </div>                            
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblInterestAmount" runat="server" class="control-label" Text="Interest Amount"></asp:Label><span
                                    id="lblInterestRate" runat="server"></span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtInterestAmount" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hfInterestAmount" Value="0" runat="server" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label Style="text-align: left" ID="lblPerInstallAmount" runat="server" class="control-label" Text="Installment Loan Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPerInstallLoanAmount" ReadOnly="true" CssClass="form-control"
                                    runat="server"></asp:TextBox>
                                <asp:HiddenField ID="hfPerInstallLoanAmount" Value="0" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label Style="text-align: left" ID="Label1" runat="server" class="control-label" Text="Installment Interest Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPerInstallInterestAmount" runat="server" CssClass="form-control"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hfPerInstallInterestAmount" Value="0" runat="server" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" Style="text-align: left" runat="server" class="control-label" Text="Total Installment Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTotalInstallAmount" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLoanDate" runat="server" class="control-label required-field" Text="Loan Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLoanDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Loan From"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlLoanPaymentFromAccountHeadId" runat="server" CssClass="form-control" TabIndex="27">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label required-field" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="display: none">
                            <div class="col-md-2">
                                <asp:Label ID="lblCheckedBy" runat="server" class="control-label required-field"
                                    Text="Checked By"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCheckedBy" runat="server" CssClass="form-control" TabIndex="27">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblApprovedBy" runat="server" class="control-label required-field"
                                    Text="Approved By"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlApprovedBy" runat="server" CssClass="form-control" TabIndex="28">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Attachment</label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Loan Document..." />
                            </div>
                        </div>
                        <div id="DocumentInfo">
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSave_Click" OnClientClick="return SaveUpdate();" />
                                <asp:Button ID="btnCancel" OnClientClick="return confirm('Do you want to clear?');" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary" />
                                <button type="button" id="btnLoanApplicationForm" class="TransactionalButton btn btn-primary">
                                    Generate Loan Application Form</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Loan Search
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:EmployeeForLoanSearch ID="employeeForLoanSearch" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSLoanType" runat="server" class="control-label" Text="Loan Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSLoanType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="---Please Select---" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Company Loan" Value="CompanyLoan"></asp:ListItem>
                                    <asp:ListItem Text="PF Loan" Value="PFLoan"> </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSLoanStatus" runat="server" class="control-label" Text="Loan Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSLoanStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Regular" Value="REGULAR"></asp:ListItem>
                                    <asp:ListItem Text="Holdup Loan" Value="HOLDUP"> </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary" />
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
                    <table id='tblLoanSearch' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 15%;" />
                            <col style="width: 11%;" />
                            <col style="width: 10%;" />
                            <col style="width: 11%;" />
                            <col style="width: 11%;" />
                            <col style="width: 11%;" />
                            <col style="width: 11%;" />
                            <col style="width: 20%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Employee
                                </td>
                                <td>Loan Type
                                </td>
                                <td>Loan Period
                                </td>
                                <td>Loan Amount
                                </td>
                                <td>Interest Rate
                                </td>
                                <td>Interest Amount
                                </td>
                                <td>Due Amount
                                </td>
                                <td></td>
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
                </div>
            </div>
        </div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
</asp:Content>
