<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmLoanSearch.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmLoanSearch" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="EmployeeForLoanSearch" Src="~/HMCommon/UserControl/EmployeeSearch.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var vc = [];
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Loan Search</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_btnSearch").click(function () {
                GridPaging(1, 1);
                return false;
            });

            $("#ContentPlaceHolder1_txtCollectDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtLoanHoldupDateFrom").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtLoanHoldupDateTo").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtLoanHoldupDateTo").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtLoanHoldupDateFrom").datepicker("option", "maxDate", selectedDate);
                }
            });
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var employeeId = "", loanType = "", loanStatus = "";

            employeeId = $("#ContentPlaceHolder1_employeeForLoanSearch_hfEmployeeId").val();
            loanType = $("#ContentPlaceHolder1_ddlSLoanType").val();
            loanStatus = $("#ContentPlaceHolder1_ddlSLoanStatus").val();

            if (employeeId == "0")
                employeeId = "";

            var gridRecordsCount = $("#tblLoanSearch tbody tr").length;
            //string empId, string loanType, string loanStatus, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage
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
            vc = result;
            $("#tblLoanSearch tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            $("#tblLoanSearch tbody").append(result.GridBody);
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return;

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
                $("#tblLoanSearch tbody").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "", holdupLink = "", collectLink = "";
            var approvedLink = "", takenPeriodDisplay = "", cancelLink = "";

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
                    takenPeriodDisplay = gridObject.LoanTakenForPeriod + " " + gridObject.LoanTakenForMonthOrYear;
                }

                tr += "<td align='left' style=\"width:15%;\">" + gridObject.EmployeeName + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.LoanType + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + takenPeriodDisplay + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.LoanAmount + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.InterestRate + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.InterestAmount + "</td>";
                //tr += "<td align='left' style=\"width:11%;\">" + gridObject.DueAmount + "</td>";
                tr += "<td align='left' style=\"width:11%;\">" + gridObject.IsAutoLoanCollectionProcessEnable + "</td>";
                
                if (gridObject.LoanStatus == "Regular" && gridObject.ApprovedStatus == "Approved") {
                    holdupLink = "&nbsp;<a href=\"javascript:void(0);\"  title=\"Loan Holdup\" onclick=\"javascript:return LoanHoldup(" + gridObject.LoanId + "," + gridObject.EmpId + "," + result.GridPageLinks.CurrentPageNumber + ");\"><img alt=\"Loan Holdup\" src=\"../Images/holdup.png\" /></a>";
                    if (gridObject.IsAutoLoanCollectionProcessEnable == 0) {
                        collectLink = "&nbsp;<a href=\"javascript:void(0);\" title=\"Loan Collection\" onclick=\"javascript:return LoanCollection(" + gridObject.LoanId + "," + gridObject.EmpId + "," + result.GridPageLinks.CurrentPageNumber + ");\"><img alt=\"Loan Collection\" src=\"../Images/LoanCollection.png\" /></a>";
                    }
                }
                else if (gridObject.LoanStatus != "Holdup" && gridObject.ApprovedStatus != "Approved") {
                    approvedLink = "&nbsp;<a href=\"javascript:void(0);\"  title=\"Approved\" onclick=\"javascript:return LoanApproved(" + gridObject.LoanId + "," + gridObject.EmpId + ",'" + gridObject.ApprovedStatus + "'," + gridObject.CreatedBy + "," + result.GridPageLinks.CurrentPageNumber + ");\"> <img alt=\"Approval\" src=\"../Images/approved.png\" /> </a>";
                    cancelLink = "&nbsp;<a href=\"javascript:void(0);\"  title=\"Cancel\" onclick=\"javascript:return LoanCancel(" + gridObject.LoanId + "," + gridObject.EmpId + "," + result.GridPageLinks.CurrentPageNumber + ");\"> <img alt=\"Cancel\" src=\"../Images/cancel.png\" /> </a>";
                }

                tr += "<td align='center' style=\"width:20%;\">" + approvedLink + cancelLink + collectLink + holdupLink + "</td>";
                tr += "</tr>";

                $("#tblLoanSearch tbody").append(tr);
                tr = "";
                holdupLink = ""; collectLink = "";
                approvedLink = ""; cancelLink = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
        }
        function OnLoanLoadFailed(error) {

        }

        function LoanCollection(loanId, empId, currentPageNumber) {

            $("#<%=hfLoanId.ClientID %>").val(loanId);
            $("#<%=hfEmployeeId.ClientID %>").val(empId);

            PageMethods.GetLoanCollectionInfo(loanId, OnLoadLoanCollectionSucceed, OnLoadLoanCollectionFailed);

            $("#collectionDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Loan Collection",
                show: 'slide'
            });

            return false;
        }

        function OnLoadLoanCollectionSucceed(result) {

            $("#ContentPlaceHolder1_txtInstallmentNumber").val(result.InstallmentNumber);
            $("#ContentPlaceHolder1_txtPerInstallLoanAmount").val(result.PerInstallLoanAmount);
            $("#ContentPlaceHolder1_txtPerInstallInterestAmount").val(result.PerInstallInterestAmount);
            $("#ContentPlaceHolder1_txtCollectAmount").val(result.PerInstallLoanAmount + result.PerInstallInterestAmount);
            $("#ContentPlaceHolder1_txtLoanAmount").val(result.LoanAmount);
            $("#ContentPlaceHolder1_txtDueAmount").val(result.DueAmount);
            $("#ContentPlaceHolder1_txtOverDueAmount").val(result.OverDueAmount);
        }
        function OnLoadLoanCollectionFailed(error)
        { }

        function SaveLoanCollection() {

            var loanId = $("#<%=hfLoanId.ClientID %>").val();
            var empId = $("#<%=hfEmployeeId.ClientID %>").val();
            var strCollectDate = $("#ContentPlaceHolder1_txtCollectDate").val();

            var installmentNumber = $("#<%=txtInstallmentNumber.ClientID %>").val();
            //var collectionDate = $("#<%=txtCollectDate.ClientID %>").val();

            var collectionDate = strCollectDate == "" ? null : CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(strCollectDate, innBoarDateFormat);


            var collectedLoanAmount = $("#<%=txtPerInstallLoanAmount.ClientID %>").val();
            var collectedinterestAmount = $("#<%=txtPerInstallInterestAmount.ClientID %>").val();

            if (collectionDate == "") {
                toastr.info("Please Give Collection Date.");
                return false;
            }

            var loanCollection = {
                LoanId: loanId,
                EmpId: empId,
                InstallmentNumber: installmentNumber,
                CollectionDate: collectionDate,
                CollectedLoanAmount: collectedLoanAmount,
                CollectedInterestAmount: collectedinterestAmount
            };

            PageMethods.SaveLoanCollection(loanCollection, OnUpdateLoanCollectionSucceed, OnUpdateLoanCollectionFailed);

            return false;
        }

        function OnUpdateLoanCollectionSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#collectionDialog").dialog("close");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnUpdateLoanCollectionFailed(error) { }

        function LoanHoldup(loanId, empId, currentPageNumber) {

            $("#<%=hfLoanId.ClientID %>").val(loanId);
            $("#<%=hfEmployeeId.ClientID %>").val(empId);

            PageMethods.GetLoanInfo(loanId, OnLoadLoanSucceed, OnLoadLoanFailed);

            $("#holdupDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Loan Holdup",
                show: 'slide'
            });

            return false;
        }

        function OnLoadLoanSucceed(result) {

            $("#ContentPlaceHolder1_txtInstallmentNumberWhenLoanHoldup").val(result.Loan.InstallmentNumber);
            $("#ContentPlaceHolder1_txtPerInstallAmount").val(result.Loan.PerInstallLoanAmount + result.Loan.PerInstallInterestAmount);
            $("#ContentPlaceHolder1_txtLoanAmountWhenHoldup").val(result.Loan.LoanAmount);
            $("#ContentPlaceHolder1_txtDueAmountWhenHoldup").val(result.Loan.DueAmount);
            $("#ContentPlaceHolder1_txtOverdueAmountWhenHoldup").val(result.Loan.OverDueAmount);

            if (result.LoanHolpUp != null) {
                $("#ContentPlaceHolder1_txtLoanHoldupDateFrom").val(GetStringFromDateTime(result.LoanHolpUp.LoanHoldupDateFrom));
                $("#ContentPlaceHolder1_txtLoanHoldupDateTo").val(GetStringFromDateTime(result.LoanHolpUp.LoanHoldupDateTo));
                $("#ContentPlaceHolder1_ddlLoanStatus").val(result.LoanHolpUp.HoldupStatus);
                $("#ContentPlaceHolder1_txtRemarks").val(result.LoanHolpUp.Remarks);

                $("#ContentPlaceHolder1_hfLoanHoldupId").val(result.LoanHolpUp.LoanHoldupId);

                if (result.LoanHolpUp.HoldupStatus == 'Active') {
                    $("#ContentPlaceHolder1_btnResumeLoan").attr('disabled', false);
                }
                else {
                    $("#ContentPlaceHolder1_btnResumeLoan").attr('disabled', true);
                }

                if (result.LoanHolpUp.LoanHoldupId != 0) {
                    $("#ContentPlaceHolder1_btnLoanHoldupSave").val("Update");
                }
                else {
                    $("#ContentPlaceHolder1_btnLoanHoldupSave").val("Save");
                }
            }
            else {
                $("#ContentPlaceHolder1_txtLoanHoldupDateFrom").val("");
                $("#ContentPlaceHolder1_txtLoanHoldupDateTo").val("");
                $("#ContentPlaceHolder1_ddlLoanStatus").val("Active");
                $("#ContentPlaceHolder1_txtRemarks").val("");
                $("#ContentPlaceHolder1_hfLoanHoldupId").val("0");
                $("#ContentPlaceHolder1_btnLoanHoldupSave").val("Save");
            }

        }
        function OnLoadLoanFailed(error)
        { }

        function SaveLoanHoldup() {

            if (!confirm("Do You Want To Holdup This Loan?")) {
                return false;
            }

            var loanId = $("#<%=hfLoanId.ClientID %>").val();
            var empId = $("#<%=hfEmployeeId.ClientID %>").val();
            var loanHolpupId = $.trim($("#<%=hfLoanHoldupId.ClientID %>").val());

            var holdupDateFrom = $("#<%=txtLoanHoldupDateFrom.ClientID %>").val();
            if (holdupDateFrom == "") {
                isClose = false;
                toastr.warning("Enter Holdup Date From");
                $("#ContentPlaceHolder1_txtLoanHoldupDateFrom").focus();
                return false;
            }
            holdupDateFrom = CommonHelper.DateFormatToMMDDYYYY(holdupDateFrom, '/');
            var holdupDateTo = $("#<%=txtLoanHoldupDateTo.ClientID %>").val();
            if (holdupDateTo == "") {
                isClose = false;
                toastr.warning("Enter Holdup Date To");
                $("#ContentPlaceHolder1_txtLoanHoldupDateTo").focus();
                return false;
            }
            holdupDateTo = CommonHelper.DateFormatToMMDDYYYY(holdupDateTo, '/');
            var loanStatus = ($("#<%=ddlLoanStatus.ClientID %>").val());
            var remarks = $("#<%=txtRemarks.ClientID %>").val();
            var installmentNumberWhenLoanHoldup = $("#ContentPlaceHolder1_txtInstallmentNumberWhenLoanHoldup").val();
            var dueAmountWhenHoldup = $("#ContentPlaceHolder1_txtDueAmountWhenHoldup").val();
            var overdueAmountWhenHoldup = $("#ContentPlaceHolder1_txtOverdueAmountWhenHoldup").val();

            var loanHoldup = {
                LoanHoldupId: (loanHolpupId == "" ? "0" : loanHolpupId),
                LoanId: loanId,
                EmpId: empId,
                LoanHoldupDateFrom: holdupDateFrom,
                LoanHoldupDateTo: holdupDateTo,
                InstallmentNumberWhenLoanHoldup: installmentNumberWhenLoanHoldup,
                DueAmount: dueAmountWhenHoldup,
                OverDueAmount: overdueAmountWhenHoldup,
                HoldupStatus: loanStatus,
                Remarks: remarks
            };

            PageMethods.SaveLoanHoldUp(loanHoldup, OnSaveLoanHoldUpSucceed, OnSaveLoanHoldUpFailed);

            return false;
        }
        function OnSaveLoanHoldUpSucceed(result) {

            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#holdupDialog").dialog("close");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSaveLoanHoldUpFailed(error) { }

        function CloseCollectionDialog() {
            $("#collectionDialog").dialog("close");
            return false;
        }

        function CloseCollectionDialogWithConfirmaion() {
            if (!confirm("Do you want to Cancle"))
                return false;
            CloseCollectionDialog();
        }

        function CloseHoldUpDialog() {
            $("#holdupDialog").dialog("close");
            return false;
        }

        function CloseHoldUpDialogWithConfirmation() {
            if (!confirm("Do You Want to Cancle!"))
                return false;

            CloseHoldUpDialog();
        }

        function LoanApproved(loanId, empId, approvedStatus, createdBy, currentPageNumber) {
            $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val(currentPageNumber);
            PageMethods.LoanApproval(loanId, empId, approvedStatus, createdBy, OnApprovedLoanSucceed, OnApprovedLoanFailed);
        }
        function OnApprovedLoanSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ReloadGrid($("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val());
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            return false;
        }
        function OnApprovedLoanFailed() { }


        function LoanCancel(loanId, empId, currentPageNumber) {
            if (confirm("Do you want to cancel?")) {
                $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val(currentPageNumber);
                PageMethods.LoanCancel(loanId, empId, OnCancelLoanSucceed, OnCancelLoanFailed);
            }
        }
        function OnCancelLoanSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ReloadGrid($("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val());
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            return false;
        }
        function OnCancelLoanFailed() { }

        function PerformFillFormAction(loanId, currentPageIndex) {
            PageMethods.EditLoan(loanId, OnEditLoanSucceed, OnEditLoanFailed);
        }
        function OnEditLoanSucceed(result) {

            if (result.IsSuccess) {
                window.location = result.RedirectUrl;
            }
        }
        function OnEditLoanFailed(error) { }


        function PerformClearAction() {

            $("#frmHotelManagement")[0].reset();

            $("#<%=hfLoanId.ClientID %>").val("");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeName").val("");
            $("#ContentPlaceHolder1_approvedByEmployee_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_approvedByEmployee_hfEmployeeName").val("");
            $("#ContentPlaceHolder1_hfIsCurrentOrPreviousPage").val("0");

            return false;
        }
        function PerformClearActionButton() {
            $("#ContentPlaceHolder1_employeeForLoanSearch_ddlEmployee").val("0").trigger('change');           
            $("#ContentPlaceHolder1_ddlSLoanType").val("");
            $("#ContentPlaceHolder1_ddlSLoanStatus").val("REGULAR");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_employeeSearch_hfEmployeeName").val("");
            $("#ContentPlaceHolder1_approvedByEmployee_hfEmployeeId").val("0");
            $("#ContentPlaceHolder1_approvedByEmployee_hfEmployeeName").val("");
            return false;
        }

    </script>
    <asp:HiddenField ID="hfLoanHoldupId" runat="server" />
    <asp:HiddenField ID="hfLoanId" runat="server" Value="" />
    <asp:HiddenField ID="hfEmployeeId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsCurrentOrPreviousPage" runat="server" Value="" />
    <div id="collectionDialog" style="display: none;">
        <div id="collectionContent">
            <div class="panel-body">
                <div class="form-horizontal">
                    <fieldset>
                        <legend>Loan Collection Info</legend>
                        <div class="form-group">
                            <div class="col-md-2" style="width: 160px;">
                                <asp:Label ID="lblInstallmentNumber" runat="server" class="control-label" Text="Installment Number"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtInstallmentNumber" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Loan Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtLoanAmount" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2" style="width: 160px;">
                                <asp:Label ID="lblPerInstallAmount" runat="server" class="control-label" Text="Installment Loan Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtPerInstallLoanAmount" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDueAmount" runat="server" class="control-label" Text="Due Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtDueAmount" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2" style="width: 160px;">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Installment Interest Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtPerInstallInterestAmount" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblOverDueAmount" runat="server" class="control-label" Text="Over Due Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtOverDueAmount" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2" style="width: 160px;">
                                <asp:Label ID="lblCollectAmount" runat="server" class="control-label required-field" Text="Total Collection Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtCollectAmount" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblCollectDate" runat="server" class="control-label required-field" Text="Collection Date"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtCollectDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-10">
                                <asp:Button ID="btnSave" runat="server" OnClientClick="javascript:return SaveLoanCollection()"
                                    Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm" />
                                <asp:Button ID="btnCancel" runat="server" Text="Close" OnClientClick="javascript:return CloseCollectionDialogWithConfirmaion()"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <div id="holdupDialog" style="display: none;">
        <div id="holdupContent">
            <div class="panel-body">
                <div class="form-horizontal">
                    <fieldset>
                        <legend>Loan Info</legend>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Loan Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtLoanAmountWhenHoldup" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2" style="width: 145px;">
                                <asp:Label ID="lblInstallmentNumberWhenLoanHoldup" runat="server" class="control-label" Text="Installment Number"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtInstallmentNumberWhenLoanHoldup" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Due Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtDueAmountWhenHoldup" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-2" style="width: 145px;">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Installment Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtPerInstallAmount" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Over Due Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtOverdueAmountWhenHoldup" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </fieldset>
                    <fieldset>
                        <legend>Holdup Info</legend>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDurationForLoanHoldup" runat="server" class="control-label required-field" Text="Holdup Date From"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtLoanHoldupDateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2" style="width: 145px;">
                                <asp:Label ID="lblLoanTakenForMonthOrYear" runat="server" class="control-label required-field" Text="Holdup Date To"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtLoanHoldupDateTo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2" style="width: 145px;">
                                <asp:Label ID="Label7" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlLoanStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                    <asp:ListItem Text="Closed" Value="Closed"> </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-10">
                                <asp:Button ID="btnLoanHoldupSave" runat="server" OnClientClick="javascript:return SaveLoanHoldup()"
                                    Text="Save" CssClass="TransactionalButton btn btn-primary" />
                                <asp:Button ID="btnCloseHoldUpDialog" runat="server" OnClientClick="javascript:return CloseHoldUpDialogWithConfirmation()()"
                                    Text="Clear" CssClass="TransactionalButton btn btn-primary" />
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
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
                        <input type="button" class="TransactionalButton btn btn-primary" value="Clear" onclick="PerformClearActionButton()"/>
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
                    <col style="width: 10%;" />
                    <col style="width: 10%;" />
                    <col style="width: 10%;" />
                    <col style="width: 10%;" />
                    <col style="width: 10%;" />
                    <col style="width: 10%;" />
                    <col style="width: 10%;" />
                    <col style="width: 15%;" />
                </colgroup>
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                        <th>Employee
                        </th>
                        <th>Loan Type
                        </th>
                        <th>Loan Period
                        </th>
                        <th>Loan Amount
                        </th>
                        <th>Interest Rate
                        </th>
                        <th>Interest Amount
                        </th>
                        <th>Due Amount
                        </th>
                        <th>Status
                        </th>
                        <th style="text-align: center;">Action
                        </th>
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
</asp:Content>
