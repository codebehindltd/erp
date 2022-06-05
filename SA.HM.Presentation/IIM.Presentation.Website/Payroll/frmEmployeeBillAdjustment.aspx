<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmployeeBillAdjustment.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmployeeBillAdjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var AdvanceBill = new Array();
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
                IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
                IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
                IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
                if (IsCanSave) {
                    $('#btnAdjustment').show();
                } else {
                    $('#btnAdjustment').hide();
                }
            }

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlEmployee").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlEmployeeForSearch").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlEmployeeBill").change(function () {
                $("#chkNonGeneratedBill").prop("checked", false);
                SearchCompanyBill();
            });

            $('#ContentPlaceHolder1_txtAdjustmentDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlAdvanceLedger").change(function () {

                var selectedAdvanceBill = _.findWhere(AdvanceBill, { EmployeePaymentId: parseInt($(this).val()) });

                if (selectedAdvanceBill != null) {
                    $("#ContentPlaceHolder1_txtBalanceAmount").val(selectedAdvanceBill.AdvanceAmountRemaining);

                    if ($("#ContentPlaceHolder1_ddlAdjustmentType").val() == "Refund") {
                        $("#ContentPlaceHolder1_txtLedgerAmount").val(selectedAdvanceBill.AdvanceAmountRemaining);
                        PaymentConvertion();
                    }
                    else {
                        $("#ContentPlaceHolder1_txtLedgerAmount").val("");
                    }
                }
            });

        });

        function EmployeeGeneratedBillBySearch() {
            CommonHelper.SpinnerOpen();
            var employeeId = $("#ContentPlaceHolder1_ddlEmployee").val();
            $("#BillInfo tbody").html("");
            PageMethods.EmployeeGeneratedBillBySearch(employeeId, OnLoadGeneratedBillSucceeded, OnGeneratedBillFailed);
            EmployeeBillAdvanceBySearch();
        }

        function OnLoadGeneratedBillSucceeded(result) {

            var employeeInfo = {};
            var employeeBill = new Array();

            employeeInfo = result[0];
            employeeBill = result[1];

            var list = result;
            var control = $('#ContentPlaceHolder1_ddlEmployeeBill');

            control.empty();
            if (employeeBill != null) {
                if (employeeBill.length > 0) {
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < employeeBill.length; i++) {
                        control.append('<option title="' + employeeBill[i].EmployeeBillNumber + '" value="' + employeeBill[i].EmployeeBillId + '">' + employeeBill[i].EmployeeBillNumber + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            control.val($("#ContentPlaceHolder1_hfEmployeeBillId").val());

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnGeneratedBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function GenerateEmployeeBill() {

            CommonHelper.SpinnerOpen();

            var totalPayment = 0.00, advanceAmount = 0.00, detailsId = 0, paymentId = 0, accountingPostingHeadId = 0;
            var employeeBillId = 0, employeeId = 0, adjustmentType = '', employeePaymentAdvanceId = 0, adjustmentAmount = 0;
            var paymentType = '';

            var EmployeePaymentDetails = new Array();
            var EmployeePaymentDetailsEdited = new Array();
            var EmployeePaymentDetailsDeleted = new Array();

            employeeId = $("#ContentPlaceHolder1_ddlEmployee").val();
            paymentId = $("#ContentPlaceHolder1_hfPaymentId").val();
            employeeBillId = $("#ContentPlaceHolder1_ddlEmployeeBill").val();

            if (employeeBillId == null) { employeeBillId = "0"; }

            adjustmentType = $("#ContentPlaceHolder1_ddlAdjustmentType").val();
            employeePaymentAdvanceId = $("#ContentPlaceHolder1_ddlAdvanceLedger").val();
            adjustmentAmount = $("#ContentPlaceHolder1_txtLedgerAmount").val();
            paymentType = $("#ContentPlaceHolder1_ddlPayMode").val();

            var EmployeePayment = {};

            if ($("#ContentPlaceHolder1_ddlAdjustmentType").val() == "Adjustment") {
                EmployeePayment = {
                    PaymentId: paymentId,
                    EmployeeBillId: employeeBillId,
                    PaymentFor: 'Adjustment',
                    AdjustmentType: adjustmentType,
                    EmployeePaymentAdvanceId: employeePaymentAdvanceId,
                    EmployeeId: employeeId,
                    PaymentDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtAdjustmentDate").val(), '/'),
                    AdvanceAmount: advanceAmount,
                    AdjustmentAmount: 0,
                    PaymentType: 'Adjustment',
                    AccountingPostingHeadId: accountingPostingHeadId,
                    Remarks: $("#ContentPlaceHolder1_txtRemarks").val(),
                    ChequeNumber: null,
                    CurrencyId: null,
                    ConvertionRate: null
                };
            }
            else {

                EmployeePayment = {
                    PaymentId: paymentId,
                    EmployeeBillId: employeeBillId,
                    PaymentFor: 'Adjustment',
                    AdjustmentType: adjustmentType,
                    EmployeePaymentAdvanceId: employeePaymentAdvanceId,
                    EmployeeId: employeeId,
                    PaymentDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtAdjustmentDate").val(), '/'),
                    AdvanceAmount: advanceAmount,
                    AdjustmentAmount: 0,
                    PaymentType: paymentType,
                    AccountingPostingHeadId: accountingPostingHeadId,
                    Remarks: $("#ContentPlaceHolder1_txtRemarks").val(),
                    ChequeNumber: $("#ContentPlaceHolder1_txtChecqueNumber").val(),
                    CurrencyId: $("#ContentPlaceHolder1_ddlCurrency").val(),
                    ConvertionRate: $("#ContentPlaceHolder1_txtConversionRate").val() == "" ? "0" : $("#ContentPlaceHolder1_txtConversionRate").val()
                };
            }

            $("#BillInfo tbody tr").each(function () {

                detailsId = parseInt($(this).find("td:eq(7)").text(), 10);

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId == 0) {
                    EmployeePaymentDetails.push({
                        PaymentDetailsId: 0,
                        EmployeeBillDetailsId: $(this).find("td:eq(9)").text(),
                        EmployeePaymentId: $(this).find("td:eq(10)").text(),
                        BillId: $(this).find("td:eq(11)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId > 0) {
                    EmployeePaymentDetailsEdited.push({
                        PaymentDetailsId: detailsId,
                        EmployeeBillDetailsId: $(this).find("td:eq(9)").text(),
                        EmployeePaymentId: $(this).find("td:eq(10)").text(),
                        BillId: $(this).find("td:eq(11)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == false && detailsId > 0) {
                    EmployeePaymentDetailsDeleted.push({
                        PaymentDetailsId: detailsId,
                        EmployeeBillDetailsId: $(this).find("td:eq(9)").text(),
                        EmployeePaymentId: $(this).find("td:eq(10)").text(),
                        BillId: $(this).find("td:eq(11)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").text()
                    });
                }
            });

            PageMethods.AdjustedEmployeeBillPayment(EmployeePayment, EmployeePaymentDetails, EmployeePaymentDetailsEdited, EmployeePaymentDetailsDeleted, OnGenerateEmployeeBillSucceeded, OnGenerateEmployeeBillFailed);

            return false;
        }

        function OnGenerateEmployeeBillSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnGenerateEmployeeBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function PerformClearAction() {
            $("#BillInfo tbody").html("");
            $('#ContentPlaceHolder1_ddlEmployee').val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlEmployeeBill").val("0");
            $("#ContentPlaceHolder1_txtBalanceAmount").val("");
            $("#ContentPlaceHolder1_txtRemainingAmount").val("");
            $("#ContentPlaceHolder1_txtAdjustedAmount").val("");
            $("#ContentPlaceHolder1_txtAdjustmentDate").datepicker("setDate", DayOpenDate);
            $("#chkNonGeneratedBill").prop("checked", false);
            $("#btnAdjustment").val("Bill Adjustment");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_ddlAdvanceLedger").val("0");

            $("#chkNonGeneratedBill").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlEmployeeBill").attr("disabled", false);
        }

        function CalculatePayment(control) {

            var tr = $(control).parent().parent();
            var billAmount = parseFloat($(tr).find("td:eq(4)").text());
            var billId = $(tr).find("td:eq(11)").text();
            var isChecked = $(tr).find("td:eq(0)").find("input").is(":checked");
            var balanceAmount = parseFloat($("#ContentPlaceHolder1_txtBalanceAmount").val());

            var totalPayment = 0.00;

            $("#BillInfo tbody tr").each(function () {

                var currentBillId = $(this).find("td:eq(11)").text();

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true && currentBillId != billId) {
                    totalPayment += parseFloat($(this).find("td:eq(4)").text());
                }
            });

            var adjustedAmount = 0.00, remainingAmount = 0.00, canAdjustedAmount = 0.00;

            if (isChecked == false) {
                $(tr).find("td:eq(5)").text("0");
                $(tr).find("td:eq(6)").text(toFixed(billAmount, 2));
                $("#ContentPlaceHolder1_txtAdjustedAmount").val(toFixed(totalPayment, 2));
                $("#ContentPlaceHolder1_txtRemainingAmount").val(toFixed(balanceAmount - totalPayment, 2));
                return;
            }

            adjustedAmount = totalPayment;
            remainingAmount = balanceAmount - adjustedAmount;

            if ((billAmount + adjustedAmount) <= balanceAmount && balanceAmount >= 0.00) {
                adjustedAmount = adjustedAmount + billAmount;
            }
            else if ((billAmount + adjustedAmount) > balanceAmount && balanceAmount > 0.00) {
                canAdjustedAmount = balanceAmount - adjustedAmount;
                adjustedAmount = adjustedAmount + canAdjustedAmount;
            }
            else {
                $(tr).find("td:eq(0)").find("input").prop("checked", false);
                toastr.info("No Balance Is Remaining.");
                return;
            }

            if (canAdjustedAmount > 0) {
                $(tr).find("td:eq(5)").text(canAdjustedAmount);
                $(tr).find("td:eq(6)").text(toFixed(billAmount - canAdjustedAmount, 2));
            }
            else {
                $(tr).find("td:eq(5)").text(billAmount);
                $(tr).find("td:eq(6)").text("0");
            }

            $("#ContentPlaceHolder1_txtAdjustedAmount").val(toFixed(adjustedAmount, 2));
            $("#ContentPlaceHolder1_txtRemainingAmount").val(toFixed(balanceAmount - adjustedAmount, 2));
        }

        function CalculatePaymentWhenEdit() {

            var totalPayment = 0.00, adjustedAmount = 0.00, remainingAmount = 0.00, balanceAmount = 0.00;
            var balanceAmount = parseFloat($("#ContentPlaceHolder1_txtBalanceAmount").val());

            $("#BillInfo tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                    totalPayment += parseFloat($(this).find("td:eq(4)").text());
                }
            });

            remainingAmount = balanceAmount - totalPayment;

            $("#ContentPlaceHolder1_txtAdjustedAmount").val(toFixed(totalPayment, 2));
            $("#ContentPlaceHolder1_txtRemainingAmount").val(toFixed(remainingAmount, 2));
        }

        function SearchCompanyBill() {
            CommonHelper.SpinnerOpen();
            var employeeId = $("#ContentPlaceHolder1_ddlEmployee").val();
            var employeeBillId = $("#ContentPlaceHolder1_ddlEmployeeBill").val();

            $("#BillInfo tbody").html("");
            PageMethods.EmployeeBillBySearch(employeeBillId, employeeId, OnLoadEmployeeBillSucceeded, OnEmployeeBillFailed);
        }

        function OnLoadEmployeeBillSucceeded(result) {

            var row = 0, tr = "", chk = "checked='checked'", isChecked = "0";
            var totalPaymentAmount = 0.00;

            for (row = 0; row < result.length; row++) {

                //isChecked = result[row].CompanyBillId > 0 ? "1" : "0";

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 7%'> ";

                if (isChecked == "1") {
                    tr += "<input type='checkbox' id='pay" + result[row].EmployeePaymentId + "'" + chk + " onclick='CalculatePayment(this)' />";
                    totalPaymentAmount += result[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result[row].EmployeePaymentId + "' onclick='CalculatePayment(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 15%'>" + result[row].ModuleName + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 20%'>" + result[row].BillNumber + "</td>";
                tr += "<td style='width: 15%'>" + result[row].DueAmount + "</td>";
                tr += "<td style='width: 18%'>0</td>";
                tr += "<td style='width: 15%'>" + result[row].DueAmount + "</td>";

                tr += "<td style=display:none;'>" + result[row].PaymentDetailsId + "</td>";
                tr += "<td style=display:none;'>" + result[row].EmployeeBillId + "</td>";
                tr += "<td style=display:none;'>" + result[row].EmployeeBillDetailsId + "</td>";
                tr += "<td style=display:none;'>" + result[row].EmployeePaymentId + "</td>";
                tr += "<td style=display:none;'>" + result[row].BillId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnEmployeeBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function LoadNonGeneratedBill() {

            var employeeId = $("#ContentPlaceHolder1_ddlEmployee").val();
            $("#ContentPlaceHolder1_ddlEmployeeBill").val("0");
            $("#BillInfo tbody").html("");

            if (employeeId == "0") {
                toastr.warning("Please Select Employee.");
                return false;
            }

            CommonHelper.SpinnerOpen();
            PageMethods.EmployeeNonGeneratedBillBySearch(employeeId, OnLoadNonGeneratedEmployeeBillSucceeded, OnNonGeneratedEmployeeBillFailed);
        }

        function OnLoadNonGeneratedEmployeeBillSucceeded(result) {

            var row = 0, tr = "", chk = "checked='checked'", isChecked = "0";
            var totalPaymentAmount = 0.00;

            for (row = 0; row < result.length; row++) {

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 7%'> ";

                if (isChecked == "1") {
                    tr += "<input type='checkbox' id='pay" + result[row].EmployeePaymentId + "'" + chk + " onclick='CalculatePayment(this)' />";
                    totalPaymentAmount += result[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result[row].EmployeePaymentId + "' onclick='CalculatePayment(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 15%'>" + result[row].ModuleName + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 20%'>" + result[row].BillNumber + "</td>";
                tr += "<td style='width: 15%'>" + result[row].DueAmount + "</td>";
                tr += "<td style='width: 18%'>0</td>";
                tr += "<td style='width: 15%'>" + result[row].DueAmount + "</td>";

                tr += "<td style=display:none;'>" + 0 + "</td>";
                tr += "<td style=display:none;'>" + 0 + "</td>";
                tr += "<td style=display:none;'>" + 0 + "</td>";
                tr += "<td style=display:none;'>" + result[row].EmployeePaymentId + "</td>";
                tr += "<td style=display:none;'>" + result[row].BillId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            $("#ContentPlaceHolder1_txtLedgerAmount").val(totalPaymentAmount);
            CommonHelper.ApplyDecimalValidation();

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnNonGeneratedEmployeeBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function SearchPayment() {

            var dateFrom = null, dateTo = null, employeeId = 0;

            employeeId = $("#ContentPlaceHolder1_ddlEmployeeForSearch").val();

            if ($("#ContentPlaceHolder1_txtFromDate").val() != "") {
                dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/')
            }
            if ($("#ContentPlaceHolder1_txtToDate").val() != "") {
                dateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/')
            }

            $("#BillInfoSearch tbody").html("");
            PageMethods.GetEmployeePaymentBySearch(employeeId, dateFrom, dateTo, OnSearchPaymentSucceeded, OnSearchPaymentFailed);

            return false;
        }

        function OnSearchPaymentSucceeded(result) {
            $("#BillInfoSearch tbody").html("");
            var row = 0, tr = "";

            for (row = 0; row < result.length; row++) {

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 20%'>" + result[row].LedgerNumber + "</td>";
                tr += "<td style='width: 15%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 20%'>" + result[row].EmployeeName + "</td>";

                if (result[row].Remarks != "" && result[row].Remarks != null)
                    tr += "<td style='width: 35%'>" + result[row].Remarks + "</td>";
                else
                    tr += "<td style='width: 35%'></td>";

                if (result[row].ApprovedStatus == null) {
                    tr += "<td style='width:10%;'>";
                    if (IsCanSave) {
                        tr += "<a href='javascript:void();' onclick= \"javascript:return ApprovedPayment(" + result[row].PaymentId + ", '" + result[row].AdjustmentType + "')\" ><img alt='approved' src='../Images/approved.png' /></a>";

                        tr += "&nbsp;&nbsp;";
                    }
                    if (IsCanEdit) {
                        tr += "<a onclick=\"javascript:return FIllForEdit(" + result[row].PaymentId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"

                        tr += "&nbsp;&nbsp;";
                    }
                    if (IsCanDelete) {
                        tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteEmployeePayment(" + result[row].PaymentId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    }
                    tr += "</td>";
                }
                else {
                    tr += "<td style='width:10%;'>";
                    tr += "</td>";
                }

                tr += "<td style=display:none;'>" + result[row].PaymentId + "</td>";
                tr += "</tr>";

                $("#BillInfoSearch tbody").append(tr);
                tr = "";
            }
        }
        function OnSearchPaymentFailed() { }

        function FIllForEdit(actionId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#ContentPlaceHolder1_ddlAdjustmentType").val(result.EmployeePayment.AdjustmentType);
            $("#ContentPlaceHolder1_hfPaymentId").val(result.EmployeePayment.PaymentId);

            if (result.EmployeePayment.EmployeeBillId != null)
                $("#ContentPlaceHolder1_hfEmployeeBillId").val(result.EmployeePayment.EmployeeBillId + "");
            else $("#ContentPlaceHolder1_hfEmployeeBillId").val("0");

            $("#ContentPlaceHolder1_ddlEmployee").val(result.EmployeePayment.EmployeeId).trigger('change');
            $("#ContentPlaceHolder1_hfEmployeePaymentId").val(result.EmployeePayment.EmployeePaymentId);
            $("#ContentPlaceHolder1_hfEmployeeAdvancePaymentId").val(result.EmployeePayment.EmployeePaymentAdvanceId);

            $("#ContentPlaceHolder1_txtRemarks").val(result.EmployeePayment.Remarks);
            if (IsCanEdit) {
                $('#btnAdjustment').show();
            } else {
                $('#btnAdjustment').hide();
            }
            $("#btnAdjustment").val("Update Bill Adjustment");
            $("#BillInfo tbody").html("");

            EmployeeBillAdvanceBySearch();

            if (result.EmployeePayment.EmployeeBillId != 0) {
                LoadForEditGeneratedBill(result);
            }
            else {
                LoadForEditNonGeneratedBill(result);
            }
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadForEditGeneratedBill(result) {

            $("#chkNonGeneratedBill").prop("checked", false);
            $("#chkNonGeneratedBill").attr("disabled", true);

            EmployeeGeneratedBillBySearch();

            if (result.EmployeeGeneratedBill.length > 0) {

                var row = 0, tr = "", chk = "checked='checked'", isChecked = "0";
                var totalPaymentAmount = 0.00;

                for (row = 0; row < result.EmployeeGeneratedBill.length; row++) {

                    var pd = _.findWhere(result.EmployeePaymentDetails, { BillId: result.EmployeeGeneratedBill[row].BillId });

                    if (pd != null) {
                        isChecked = "1";
                    }

                    if ((row + 1) % 2 == 0) {
                        tr += "<tr style='background-color:#FFFFFF;'>";
                    }
                    else {
                        tr += "<tr style='background-color:#E3EAEB;'>";
                    }

                    tr += "<td style='width: 7%'> ";

                    if (isChecked == "1") {
                        tr += "<input type='checkbox' id='pay" + result.EmployeeGeneratedBill[row].EmployeePaymentId + "'" + chk + " onclick='CalculatePayment(this)' />";
                        totalPaymentAmount += result.EmployeeGeneratedBill[row].DueAmount;
                    }
                    else {
                        tr += "<input type='checkbox' id='pay" + result.EmployeeGeneratedBill[row].EmployeePaymentId + "' onclick='CalculatePayment(this)' />";
                    }

                    tr += "</td>";

                    tr += "<td style='width: 15%'>" + result.EmployeeGeneratedBill[row].ModuleName + "</td>";
                    tr += "<td style='width: 10%'>" + GetStringFromDateTime(result.EmployeeGeneratedBill[row].PaymentDate) + "</td>";
                    tr += "<td style='width: 20%'>" + result.EmployeeGeneratedBill[row].BillNumber + "</td>";
                    tr += "<td style='width: 15%'>" + result.EmployeeGeneratedBill[row].DueAmount + "</td>";

                    if (pd != null) {
                        tr += "<td style='width: 18%'>" + pd.PaymentAmount + "</td>";
                    }
                    else {
                        tr += "<td style='width: 18%'></td>";
                    }

                    if (pd != null) {
                        tr += "<td style='width: 15%'>" + (result.EmployeeGeneratedBill[row].DueAmount - pd.PaymentAmount) + "</td>";
                    }
                    else {
                        tr += "<td style='width: 15%'>" + result.EmployeeGeneratedBill[row].DueAmount + "</td>";
                    }

                    if (pd != null) {
                        tr += "<td style='display:none;'>" + pd.PaymentDetailsId + "</td>";
                    }
                    else {
                        tr += "<td style='display:none;'>0</td>";
                    }

                    if (pd != null) {
                        tr += "<td style='display:none;'>" + pd.EmployeeBillId + "</td>";
                    }
                    else {
                        tr += "<td style='display:none;'>0</td>";
                    }

                    if (pd != null) {
                        tr += "<td style='display:none;'>" + pd.EmployeeBillDetailsId + "</td>";
                    }
                    else {
                        tr += "<td style='display:none;'>0</td>";
                    }

                    tr += "<td style=display:none;'>" + result.EmployeeGeneratedBill[row].EmployeePaymentId + "</td>";
                    tr += "<td style=display:none;'>" + result.EmployeeGeneratedBill[row].BillId + "</td>";

                    tr += "</tr>";

                    $("#BillInfo tbody").append(tr);
                    tr = "";
                }

                setTimeout(function () { CalculatePaymentWhenEdit() }, 500);
                $("#myTabs").tabs({ active: 0 });

                return false;
            }
        }

        function LoadForEditNonGeneratedBill(result) {

            $("#ContentPlaceHolder1_ddlEmployeeBill").val("0");
            $("#chkNonGeneratedBill").prop("checked", true);
            $("#ContentPlaceHolder1_ddlEmployeeBill").attr("disabled", true);

            EmployeeGeneratedBillBySearch();

            if (result.EmployeeBill.length > 0) {

                var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
                var totalPaymentAmount = 0.00;

                for (row = 0; row < result.EmployeeBill.length; row++) {

                    var pd = _.findWhere(result.EmployeePaymentDetails, { BillId: result.EmployeeBill[row].BillId });

                    if (pd != null) {
                        isChecked = "1";
                    }

                    if ((row + 1) % 2 == 0) {
                        tr += "<tr style='background-color:#FFFFFF;'>";
                    }
                    else {
                        tr += "<tr style='background-color:#E3EAEB;'>";
                    }

                    tr += "<td style='width: 7%'> ";

                    if (isChecked == "1") {
                        tr += "<input type='checkbox' id='pay" + result.EmployeeBill[row].EmployeePaymentId + "'" + chk + " onclick='CalculatePayment(this)' />";
                        totalPaymentAmount += result.EmployeeBill[row].DueAmount;
                    }
                    else {
                        tr += "<input type='checkbox' id='pay" + result.EmployeeBill[row].EmployeePaymentId + "' onclick='CalculatePayment(this)' />";
                    }

                    tr += "</td>";

                    tr += "<td style='width: 15%'>" + result.EmployeeBill[row].ModuleName + "</td>";
                    tr += "<td style='width: 10%'>" + GetStringFromDateTime(result.EmployeeBill[row].PaymentDate) + "</td>";
                    tr += "<td style='width: 20%'>" + result.EmployeeBill[row].BillNumber + "</td>";
                    tr += "<td style='width: 15%'>" + result.EmployeeBill[row].DueAmount + "</td>";

                    if (pd != null) {
                        tr += "<td style='width: 18%'>" + pd.PaymentAmount + "</td>";
                    }
                    else {
                        tr += "<td style='width: 18%'></td>";
                    }

                    if (pd != null) {
                        tr += "<td style='width: 15%'>" + (result.EmployeeBill[row].DueAmount - pd.PaymentAmount) + "</td>";
                    }
                    else {
                        tr += "<td style='width: 15%'>" + result.EmployeeBill[row].DueAmount + "</td>";
                    }

                    if (pd != null) {
                        tr += "<td style=display:none;'>" + pd.PaymentDetailsId + "</td>";
                    }
                    else {
                        tr += "<td style=display:none;'>" + 0 + "</td>";
                    }


                    tr += "<td style=display:none;'>" + 0 + "</td>";
                    tr += "<td style=display:none;'>" + 0 + "</td>";

                    tr += "<td style=display:none;'>" + result.EmployeeBill[row].EmployeePaymentId + "</td>";
                    tr += "<td style=display:none;'>" + result.EmployeeBill[row].BillId + "</td>";

                    tr += "</tr>";

                    $("#BillInfo tbody").append(tr);
                    tr = "";
                    isChecked = "0";
                }

                setTimeout(function () { CalculatePaymentWhenEdit() }, 500);
                $("#myTabs").tabs({ active: 0 });

                return false;
            }
        }

        function DeleteEmployeePayment(paymentId) {
            if (!confirm("Do you want to delete?")) {
                return false;
            }
            PageMethods.DeleteEmployeePayment(paymentId, OnReceiveDeleteSucceed, OnReceiveDeleteFailed);
            return false;
        }
        function OnReceiveDeleteSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchPayment();
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnReceiveDeleteFailed(error) {
            toastr.error(error.get_message());
        }

        function ApprovedPayment(paymentId, adjustmentType) {
            if (!confirm("Do you want to approve?")) {
                return false;
            }
            PageMethods.ApprovedPaymentAdjustment(paymentId, adjustmentType, OnApporavalSucceed, OnApporavalFailed);
            return false;
        }
        function OnApporavalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchPayment();
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApporavalFailed(error) {
            toastr.error(error.get_message());
        }

        function EmployeeBillAdvanceBySearch() {
            CommonHelper.SpinnerOpen();
            var employeeId = $("#ContentPlaceHolder1_ddlEmployee").val();
            $("#BillInfo tbody").html("");
            PageMethods.EmployeeBillAdvanceBySearch(employeeId, OnLoadAdvanceBillSucceeded, OnAdvanceBillFailed);
        }

        function OnLoadAdvanceBillSucceeded(result) {

            var employeeBill = new Array();

            employeeBill = result;
            AdvanceBill = result;

            var list = result;
            var control = $('#ContentPlaceHolder1_ddlAdvanceLedger');

            control.empty();
            if (employeeBill != null) {
                if (employeeBill.length > 0) {
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < employeeBill.length; i++) {
                        control.append('<option title="' + employeeBill[i].LedgerNumber + '" value="' + employeeBill[i].EmployeePaymentId + '">' + employeeBill[i].LedgerNumber + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            control.val($("#ContentPlaceHolder1_hfEmployeeAdvancePaymentId").val());
            if ($("#ContentPlaceHolder1_hfEmployeeAdvancePaymentId").val() != "0") {
                var amount = _.findWhere(AdvanceBill, { EmployeePaymentId: parseInt($("#ContentPlaceHolder1_hfEmployeeAdvancePaymentId").val(), 10) });
                $("#ContentPlaceHolder1_txtBalanceAmount").val(amount.AdvanceAmountRemaining);
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnAdvanceBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }


    </script>

    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfPaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfEmployeePaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyBill" runat="server" />
    <asp:HiddenField ID="hfEmployeeBillId" runat="server" Value="0" />
    <asp:HiddenField ID="hfEmployeeAdvancePaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Employee Bill Adjustment</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Employee Bill Adjustment</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Bill Search
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPaymentDate" runat="server" class="control-label" Text="Employee"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <input type="button" class="btn btn-primary" value="Search" onclick="EmployeeGeneratedBillBySearch()" />
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Adjustment Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAdjustmentType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Advance Adjustment" Value="Adjustment"></asp:ListItem>
                                    <asp:ListItem Text="Advance Refund" Value="Refund"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label7" runat="server" class="control-label" Text="Advance Ledger"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAdvanceLedger" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Employee Balance"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtBalanceAmount" CssClass="form-control" disabled runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Employee Bill"></asp:Label>
                            </div>
                            <div class="col-md-7">
                                <asp:DropDownList ID="ddlEmployeeBill" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        <input type="checkbox" id="chkNonGeneratedBill" onclick="LoadNonGeneratedBill()" />
                                    </span>
                                    <label class="form-control">Non Generated Bill</label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <table id="BillInfo" class="table table-bordered table-condensed table-hover table-responsive">
                                    <thead>
                                        <tr>
                                            <th style="width: 7%;">Select</th>
                                            <th style="width: 15%;">Payment Mode</th>
                                            <th style="width: 10%;">Bill Date</th>
                                            <th style="width: 20%;">Bill Number</th>
                                            <th style="width: 15%;">Due Amount</th>
                                            <th style="width: 18%;">Payment Amount</th>
                                            <th style="width: 15%;">Remaining Amount</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Remaining Balance"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRemainingAmount" CssClass="form-control" disabled runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Adjusted Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAdjustedAmount" CssClass="form-control" disabled runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPaymentDate2" runat="server" class="control-label required-field"
                                    Text="Adjustment Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAdjustmentDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                                    TabIndex="11"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-12">
                                <input type="button" id="btnAdjustment" class="btn btn-primary" value="Bill Adjustment" onclick="GenerateEmployeeBill()" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Bill Adjustment Search
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Employee"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlEmployeeForSearch" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" id="btnSearch" value="Search" class="TransactionalButton btn btn-primary btn-sm"
                                    onclick="javascript: return SearchPayment()" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="SearchResult" class="panel panel-default">
                    <div class="panel-heading">
                        Search Information
                    </div>
                    <div class="panel-body">
                        <table id="BillInfoSearch" class="table table-bordered table-condensed table-hover table-responsive">
                            <thead>
                                <tr>
                                    <th style="width: 20%;">Ledger Number</th>
                                    <th style="width: 15%;">Payment Date</th>
                                    <th style="width: 20%;">Company Name</th>
                                    <th style="width: 35%;">Remarks</th>
                                    <th style="width: 10%;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>




</asp:Content>
