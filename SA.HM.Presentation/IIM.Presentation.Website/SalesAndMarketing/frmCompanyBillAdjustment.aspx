<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCompanyBillAdjustment.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmCompanyBillAdjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#btnAdjustment").hide();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            PaymentRefundShowHide();

            $("#myTabs").tabs();

            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtCompanyBank", "ContentPlaceHolder1_ddlCompanyBank", "ContentPlaceHolder1_ddlCompanyBank");
            CommonHelper.AutoSearchClientDataSource("txtSupplierName", "ContentPlaceHolder1_ddlSupplierName", "ContentPlaceHolder1_ddlSupplierName");
            CommonHelper.AutoSearchClientDataSource("txtCashPayment", "ContentPlaceHolder1_ddlCashPayment", "ContentPlaceHolder1_ddlCashPayment");

            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmount = '<%=txtCalculatedLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmountHiddenField = '<%=txtCalculatedLedgerAmountHiddenField.ClientID%>'

            $("#ContentPlaceHolder1_ddlCompany").select2({
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

            $('#ContentPlaceHolder1_txtPaymentDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlCompanyBill").change(function () {
                $("#chkNonGeneratedBill").prop("checked", false);
                SearchCompanyBill();
            });

            $('#ContentPlaceHolder1_txtAdjustmentDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlAdvanceLedger").change(function () {

                var selectedAdvanceBill = _.findWhere(AdvanceBill, { CompanyPaymentId: parseInt($(this).val()) });

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

            $("#ContentPlaceHolder1_ddlAdjustmentType").change(function () {

                if ($(this).val() == "Adjustment") {
                    $("#AdvanceAdjustmentDiv").show();
                    $("#AdvanceRefundDiv").hide();
                    $("#ContentPlaceHolder1_txtLedgerAmount").val("");
                }
                else if ($(this).val() == "Refund") {
                    $("#AdvanceAdjustmentDiv").hide();
                    $("#AdvanceRefundDiv").show();

                    if ($("#ContentPlaceHolder1_txtBalanceAmount").val() != "") {
                        $("#ContentPlaceHolder1_txtLedgerAmount").val($("#ContentPlaceHolder1_txtBalanceAmount").val());
                        PaymentConvertion();
                    }
                }
            });

            $("#ContentPlaceHolder1_ddlCurrency").change(function () {
                PaymentConvertion();
            });

            var ddlCurrencyId = $("#<%=ddlCurrency.ClientID %>").val();
            PageMethods.LoadCurrencyType(ddlCurrencyId, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);

            $('#' + ddlCurrency).change(function () {
                var v = $("#<%=ddlCurrency.ClientID %>").val();
                PageMethods.LoadCurrencyType(v, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);
            });

            function OnLoadCurrencyTypeSucceeded(result) {
                $("#<%=hfCurrencyType.ClientID %>").val(result.CurrencyType);
                PageMethods.LoadCurrencyConversionRate(result.CurrencyId, OnLoadConversionRateSucceeded, OnLoadConversionRateFailed);
            }

            function OnLoadCurrencyTypeFailed() {

            }

            function OnLoadConversionRateSucceeded(result) {

                if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                    $("#<%=txtConversionRate.ClientID %>").val('');
                    $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').val("");
                    $('#ConversionPanel').hide();
                    $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').attr("disabled", false);
                }
                else {
                    $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                    $("#<%=hfConversionRate.ClientID %>").val(result.ConversionRate);

                    $('#ConversionPanel').show();
                    $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').val("");

                }
                CurrencyRateInfoEnable();
                PaymentConvertion();

            }

            function OnLoadConversionRateFailed() {
            }

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'

            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'
            $('#' + ddlPayMode).change(function () {
                PaymentModeShowHideInformation();
            });

            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }


        });

        function CompanyGeneratedBillBySearch() {
            CommonHelper.SpinnerOpen();
            var companyId = $("#ContentPlaceHolder1_ddlCompany").val();
            $("#BillInfo tbody").html("");
            PageMethods.CompanyGeneratedBillBySearch(companyId, OnLoadGeneratedBillSucceeded, OnGeneratedBillFailed);
            CompanyBillAdvanceBySearch();
        }

        function OnLoadGeneratedBillSucceeded(result) {

            var guestCompany = {};
            var companyBill = new Array();

            guestCompany = result[0];
            companyBill = result[1];

            var list = result;
            var control = $('#ContentPlaceHolder1_ddlCompanyBill');

            control.empty();
            if (companyBill != null) {
                if (companyBill.length > 0) {
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < companyBill.length; i++) {
                        control.append('<option title="' + companyBill[i].CompanyBillNumber + '" value="' + companyBill[i].CompanyBillId + '">' + companyBill[i].CompanyBillNumber + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            control.val($("#ContentPlaceHolder1_hfCompanyBillId").val());

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnGeneratedBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function GenerateCompanyBill() {

            CommonHelper.SpinnerOpen();

            var totalPayment = 0.00, advanceAmount = 0.00, detailsId = 0, paymentId = 0, accountingPostingHeadId = 0;
            var companyBillId = 0, companyId = 0, adjustmentType = '', companyPaymentAdvanceId = 0, adjustmentAmount = 0;
            var paymentType = '';

            var CompanyPaymentDetails = new Array();
            var CompanyPaymentDetailsEdited = new Array();
            var CompanyPaymentDetailsDeleted = new Array();

            if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cash") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCashPayment").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Card") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCompanyBank").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cheque") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCompanyBank").val();
            }

            companyId = $("#ContentPlaceHolder1_ddlCompany").val();
            paymentId = $("#ContentPlaceHolder1_hfPaymentId").val();
            companyBillId = $("#ContentPlaceHolder1_ddlCompanyBill").val();

            if (companyBillId == null) { companyBillId = "0"; }

            adjustmentType = $("#ContentPlaceHolder1_ddlAdjustmentType").val();
            companyPaymentAdvanceId = $("#ContentPlaceHolder1_ddlAdvanceLedger").val();
            adjustmentAmount = $("#ContentPlaceHolder1_txtLedgerAmount").val();
            paymentType = $("#ContentPlaceHolder1_ddlPayMode").val();

            var CompanyPayment = {};

            if ($("#ContentPlaceHolder1_ddlAdjustmentType").val() == "Adjustment") {
                CompanyPayment = {
                    PaymentId: paymentId,
                    CompanyBillId: companyBillId,
                    PaymentFor: 'Adjustment',
                    AdjustmentType: adjustmentType,
                    CompanyPaymentAdvanceId: companyPaymentAdvanceId,
                    CompanyId: companyId,
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

                CompanyPayment = {
                    PaymentId: paymentId,
                    CompanyBillId: companyBillId,
                    PaymentFor: 'Adjustment',
                    AdjustmentType: adjustmentType,
                    CompanyPaymentAdvanceId: companyPaymentAdvanceId,
                    CompanyId: companyId,
                    PaymentDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtPaymentDate2").val(), '/'),
                    AdvanceAmount: advanceAmount,
                    AdjustmentAmount: adjustmentAmount,
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
                    CompanyPaymentDetails.push({
                        PaymentDetailsId: 0,
                        CompanyBillDetailsId: $(this).find("td:eq(9)").text(),
                        CompanyPaymentId: $(this).find("td:eq(10)").text(),
                        BillId: $(this).find("td:eq(11)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId > 0) {
                    CompanyPaymentDetailsEdited.push({
                        PaymentDetailsId: detailsId,
                        CompanyBillDetailsId: $(this).find("td:eq(9)").text(),
                        CompanyPaymentId: $(this).find("td:eq(10)").text(),
                        BillId: $(this).find("td:eq(11)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == false && detailsId > 0) {
                    CompanyPaymentDetailsDeleted.push({
                        PaymentDetailsId: detailsId,
                        CompanyBillDetailsId: $(this).find("td:eq(9)").text(),
                        CompanyPaymentId: $(this).find("td:eq(10)").text(),
                        BillId: $(this).find("td:eq(11)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").text()
                    });
                }
            });

            PageMethods.AdjustedCompanyBillPayment(CompanyPayment, CompanyPaymentDetails, CompanyPaymentDetailsEdited, CompanyPaymentDetailsDeleted, OnGenerateCompanyBillSucceeded, OnGenerateCompanyBillFailed);

            return false;
        }

        function OnGenerateCompanyBillSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnGenerateCompanyBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function PerformClearAction() {
            $("#BillInfo tbody").html("");

            $("#ContentPlaceHolder1_hfPaymentId").val("0");
            $("#ContentPlaceHolder1_hfCompanyPaymentId").val("0");
            $("#ContentPlaceHolder1_hfCompanyBill").val("0");
            $("#ContentPlaceHolder1_hfCompanyBillId").val("0");
            $("#ContentPlaceHolder1_hfCompanyAdvancePaymentId").val("0");

            $('#ContentPlaceHolder1_ddlCompany').val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlCompanyBill").val("0");
            $("#ContentPlaceHolder1_txtBalanceAmount").val("");
            $("#ContentPlaceHolder1_txtRemainingAmount").val("");
            $("#ContentPlaceHolder1_txtAdjustedAmount").val("");
            $("#ContentPlaceHolder1_txtAdjustmentDate").datepicker("setDate", DayOpenDate);
            $("#chkNonGeneratedBill").prop("checked", false);
            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "1")
                $("#btnAdjustment").val("Bill Adjustment");
            else
                $("#btnAdjustment").val("Bill Adjustment").hide();
            $("#ContentPlaceHolder1_txtRemarks").val("");

            $("#ContentPlaceHolder1_hfCompanyAdvancePaymentId").val("0");
            $("#ContentPlaceHolder1_ddlAdvanceLedger").val("0");
            $("#chkNonGeneratedBill").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlCompanyBill").attr("disabled", false);

            $("#ContentPlaceHolder1_ddlCashPayment").val();
            $("#ContentPlaceHolder1_ddlCompanyBank").val();
            $("#txtCashPayment").val("");
            $("#txtCompanyBank").val("")
            $("#ContentPlaceHolder1_txtChecqueNumber").val("");

            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');
            $("#<%=txtExpireDate.ClientID %>").val('');

            $("#<%=ddlCurrency.ClientID %>").val('');
            $("#<%=txtCalculatedLedgerAmount.ClientID %>").val('');
            $("#<%=txtConversionRate.ClientID %>").val('');

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
            var companyId = $("#ContentPlaceHolder1_ddlCompany").val();
            var companyBillId = $("#ContentPlaceHolder1_ddlCompanyBill").val();

            $("#BillInfo tbody").html("");
            PageMethods.CompanyBillBySearch(companyBillId, companyId, OnLoadCompanyBillSucceeded, OnCompanyBillFailed);
        }

        function OnLoadCompanyBillSucceeded(result) {

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
                    tr += "<input type='checkbox' id='pay" + result[row].CompanyPaymentId + "'" + chk + " onclick='CalculatePayment(this)' />";
                    totalPaymentAmount += result[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result[row].CompanyPaymentId + "' onclick='CalculatePayment(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 15%'>" + result[row].ModuleName + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 20%'>" + result[row].BillNumber + "</td>";
                tr += "<td style='width: 15%'>" + result[row].DueAmount + "</td>";
                tr += "<td style='width: 18%'>0</td>";
                tr += "<td style='width: 15%'>" + result[row].DueAmount + "</td>";

                tr += "<td style='display:none;'>" + result[row].PaymentDetailsId + "</td>";
                tr += "<td style='display:none;'>" + result[row].CompanyBillId + "</td>";
                tr += "<td style='display:none;'>" + result[row].CompanyBillDetailsId + "</td>";
                tr += "<td style='display:none;'>" + result[row].CompanyPaymentId + "</td>";
                tr += "<td style='display:none;'>" + result[row].BillId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnCompanyBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function LoadNonGeneratedBill() {

            var companyId = $("#ContentPlaceHolder1_ddlCompany").val();
            $("#ContentPlaceHolder1_ddlCompanyBill").val("0");
            $("#BillInfo tbody").html("");

            if (companyId == "0") {
                toastr.warning("Please Select Company.");
                return false;
            }

            CommonHelper.SpinnerOpen();
            PageMethods.CompanyNonGeneratedBillBySearch(companyId, OnLoadNonGeneratedCompanyBillSucceeded, OnNonGeneratedCompanyBillFailed);

        }
        function OnLoadNonGeneratedCompanyBillSucceeded(result) {

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
                    tr += "<input type='checkbox' id='pay" + result[row].CompanyPaymentId + "'" + chk + " onclick='CalculatePayment(this)' />";
                    totalPaymentAmount += result[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result[row].CompanyPaymentId + "' onclick='CalculatePayment(this)' />";
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
                tr += "<td style=display:none;'>" + result[row].CompanyPaymentId + "</td>";
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
        function OnNonGeneratedCompanyBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function SearchPayment() {

            var dateFrom = null, dateTo = null, companyId = 0;

            companyId = $("#ContentPlaceHolder1_ddlCompanyForSearch").val();

            if ($("#ContentPlaceHolder1_txtFromDate").val() != "") {
                dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/')
            }
            if ($("#ContentPlaceHolder1_txtToDate").val() != "") {
                dateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/')
            }

            $("#BillInfoSearch tbody").html("");
            PageMethods.GetCompanyPaymentBySearch(companyId, dateFrom, dateTo, OnSearchPaymentSucceeded, OnSearchPaymentFailed);

            return false;
        }

        function OnSearchPaymentSucceeded(result) {
            $("#BillInfoSearch tbody").html("");
            var row = 0, tr = "";
            var isUpdatepermission = false, isDeletePermission = false;

            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "1")
                isUpdatepermission = true;
            if ($("#ContentPlaceHolder1_hfIsDeletePermission").val() == "1")
                isDeletePermission = true;
            debugger;
            for (row = 0; row < result.length; row++) {

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 15%'>" + result[row].LedgerNumber + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 20%'>" + result[row].CompanyName + "</td>";
                tr += "<td style='width: 15%'>" + result[row].AdjustmentType + "</td>";

                if (result[row].Remarks != "" && result[row].Remarks != null)
                    tr += "<td style='width: 30%'>" + result[row].Remarks + "</td>";
                else
                    tr += "<td style='width: 30%'></td>";

                //if (result[row].ApprovedStatus == null) {
                //    tr += "<td style='width:10%;'>";
                //    tr += "<a href='javascript:void();' onclick= \"javascript:return ApprovedPayment(" + result[row].PaymentId + ", '" + result[row].AdjustmentType + "')\" ><img alt='approved' src='../Images/approved.png' /></a>";
                //    tr += "&nbsp;&nbsp;";
                //    if (isUpdatepermission) {
                //        tr += "<a onclick=\"javascript:return FIllForEdit(" + result[row].PaymentId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                //        tr += "&nbsp;&nbsp;";
                //    }
                //    if (isDeletePermission) {
                //        tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteCompanyPayment(" + result[row].PaymentId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                //        tr += "</td>";
                //    }

                //}
                //else {
                //    tr += "<td style='width:10%;'>";
                //    tr += "</td>";
                //}
                tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";


                if (result[row].IsCanEdit && IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return FIllForEdit(" + result[row].PaymentId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (result[row].IsCanDelete && IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return DeleteCompanyPayment(" + result[row].PaymentId + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (result[row].IsCanChecked && IsCanSave) {

                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return CheckedPayment(" + result[row].PaymentId + ")\" alt='Check'  title='Check' border='0' />";
                }

                if (result[row].IsCanApproved && IsCanSave) {

                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return ApprovedPayment('" + result[row].PaymentId + "','" + result[row].AdjustmentType + "')\" alt='Approve'  title='Approve' border='0' />";
                }


                //tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.ReceiveType + "'," + gridObject.ReceivedId + ",'" + gridObject.Status + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Invoice' title='Receive Order Info' border='0' />";
                //tr += "&nbsp;&nbsp;<img src='../Images/note.png'  onClick= \"javascript:return ShowDealDocuments('" + gridObject.ReceivedId + "')\" alt='Invoice' title='Receive Order Info' border='0' />";
                tr += "</td>";

                tr += "<td style=display:none;'>" + result[row].PaymentId + "</td>";
                tr += "</tr>";

                $("#BillInfoSearch tbody").append(tr);
                tr = "";
            }
        }
        function OnSearchPaymentFailed() { }

        function FIllForEdit(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {

            $("#ContentPlaceHolder1_ddlAdjustmentType").val(result.CompanyPayment.AdjustmentType);
            $("#ContentPlaceHolder1_hfPaymentId").val(result.CompanyPayment.PaymentId);

            if (result.CompanyPayment.CompanyBillId != null)
                $("#ContentPlaceHolder1_hfCompanyBillId").val(result.CompanyPayment.CompanyBillId + "");
            else $("#ContentPlaceHolder1_hfCompanyBillId").val("0");

            $("#ContentPlaceHolder1_ddlCompany").val(result.CompanyPayment.CompanyId).trigger('change');
            $("#ContentPlaceHolder1_hfCompanyPaymentId").val(result.CompanyPayment.CompanyPaymentId);

            $("#ContentPlaceHolder1_txtRemarks").val(result.CompanyPayment.Remarks);
            $("#ContentPlaceHolder1_hfCompanyAdvancePaymentId").val(result.CompanyPayment.CompanyPaymentAdvanceId);

            $("#btnAdjustment").val("Update Bill Adjustment");

            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "1")
                $("#btnAdjustment").show();
            else
                $("#btnAdjustment").hide();
            $("#BillInfo tbody").html("");

            PaymentRefundShowHide();

            //------------ Refund

            if (result.CompanyPayment.AdjustmentType == "Refund") {

                $("#<%=ddlPayMode.ClientID %>").val(result.CompanyPayment.PaymentType);
                $("#<%=hfCurrencyType.ClientID %>").val(result.CompanyPayment.CurrencyType);

                if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                    $('#ConversionPanel').show();
                    $("#<%=txtCalculatedLedgerAmount.ClientID %>").val((result.CompanyPayment.AdjustmentAmount * result.CompanyPayment.ConvertionRate).toFixed(2));
                    $("#<%=txtCalculatedLedgerAmount.ClientID %>").attr("disabled", true);
                    $("#<%=txtConversionRate.ClientID %>").val(result.CompanyPayment.ConvertionRate);
                }
                else {
                    $('#ConversionPanel').hide();
                    $("#<%=txtCalculatedLedgerAmount.ClientID %>").attr("disabled", false);
                    $("#<%=txtConversionRate.ClientID %>").val('');
                }

                $("#<%=ddlCurrency.ClientID %>").val(result.CompanyPayment.CurrencyId);
                $("#<%=txtLedgerAmount.ClientID %>").val(result.CompanyPayment.AdjustmentAmount);
                $("#<%=txtPaymentDate2.ClientID %>").val(GetStringFromDateTime(result.CompanyPayment.PaymentDate));

                $("#<%=txtChecqueNumber.ClientID %>").val(result.CompanyPayment.ChequeNumber);

                $("#<%=ddlCompanyBank.ClientID %>").val(result.CompanyPayment.AccountingPostingHeadId + '');
                $('#txtCompanyBank').val($("#<%=ddlCompanyBank.ClientID %> option:selected").text());

                $("#<%=ddlCashPayment.ClientID %>").val(result.CompanyPayment.AccountingPostingHeadId + '');
                $('#txtCashPayment').val($("#<%=ddlCashPayment.ClientID %> option:selected").text());

                //------------------

                if (result.CompanyPayment.PaymentType == "Cash") {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').show();
                    $('#BankPaymentAccountHeadDiv').hide();
                }
                else if (result.CompanyPayment.PaymentType == "Card") {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').hide();
                    $('#BankPaymentAccountHeadDiv').hide();
                }
                else if (result.CompanyPayment.PaymentType == "Cheque") {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').show();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').hide();
                    $('#BankPaymentAccountHeadDiv').hide();
                }
                else if (result.CompanyPayment.PaymentType == "Adjustment") {
                    $('#CashReceiveAccountsInfo').show();
                    $('#CardReceiveAccountsInfo').hide();
                    $('#ChequeReceiveAccountsInfo').hide();
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                }
            }

            CompanyBillAdvanceBySearch();

            if (result.CompanyPayment.AdjustmentType != "Refund") {
                if (result.CompanyPayment.CompanyBillId != 0) {
                    LoadForEditGeneratedBill(result);
                }
                else {
                    LoadForEditNonGeneratedBill(result);
                }
            }
            $("#myTabs").tabs({ active: 0 });
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadForEditGeneratedBill(result) {

            $("#chkNonGeneratedBill").prop("checked", false);
            $("#chkNonGeneratedBill").attr("disabled", true);

            CompanyGeneratedBillBySearch();

            if (result.CompanyGeneratedBill.length > 0) {

                var row = 0, tr = "", chk = "checked='checked'", isChecked = "0";
                var totalPaymentAmount = 0.00;

                for (row = 0; row < result.CompanyGeneratedBill.length; row++) {

                    var pd = _.findWhere(result.CompanyPaymentDetails, { BillId: result.CompanyGeneratedBill[row].BillId });

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
                        tr += "<input type='checkbox' id='pay" + result.CompanyGeneratedBill[row].CompanyPaymentId + "'" + chk + " onclick='CalculatePayment(this)' />";
                        totalPaymentAmount += result.CompanyGeneratedBill[row].DueAmount;
                    }
                    else {
                        tr += "<input type='checkbox' id='pay" + result.CompanyGeneratedBill[row].CompanyPaymentId + "' onclick='CalculatePayment(this)' />";
                    }

                    tr += "</td>";

                    tr += "<td style='width: 15%'>" + result.CompanyGeneratedBill[row].ModuleName + "</td>";
                    tr += "<td style='width: 10%'>" + GetStringFromDateTime(result.CompanyGeneratedBill[row].PaymentDate) + "</td>";
                    tr += "<td style='width: 20%'>" + result.CompanyGeneratedBill[row].BillNumber + "</td>";
                    tr += "<td style='width: 15%'>" + result.CompanyGeneratedBill[row].DueAmount + "</td>";

                    if (pd != null) {
                        tr += "<td style='width: 18%'>" + pd.PaymentAmount + "</td>";
                    }
                    else {
                        tr += "<td style='width: 18%'></td>";
                    }

                    if (pd != null) {
                        tr += "<td style='width: 15%'>" + (result.CompanyGeneratedBill[row].DueAmount - pd.PaymentAmount) + "</td>";
                    }
                    else {
                        tr += "<td style='width: 15%'>" + result.CompanyGeneratedBill[row].DueAmount + "</td>";
                    }

                    if (pd != null) {
                        tr += "<td style='display:none;'>" + pd.PaymentDetailsId + "</td>";
                    }
                    else {
                        tr += "<td style='display:none;'>0</td>";
                    }

                    if (pd != null) {
                        tr += "<td style='display:none;'>" + pd.CompanyBillId + "</td>";
                    }
                    else {
                        tr += "<td style='display:none;'>0</td>";
                    }

                    if (pd != null) {
                        tr += "<td style='display:none;'>" + pd.CompanyBillDetailsId + "</td>";
                    }
                    else {
                        tr += "<td style='display:none;'>0</td>";
                    }

                    tr += "<td style='display:none;'>" + result.CompanyGeneratedBill[row].CompanyPaymentId + "</td>";
                    tr += "<td style='display:none;'>" + result.CompanyGeneratedBill[row].BillId + "</td>";

                    tr += "</tr>";

                    $("#BillInfo tbody").append(tr);
                    tr = "";
                    isChecked = "0";
                }

                setTimeout(function () { CalculatePaymentWhenEdit() }, 500);

                return false;
            }
        }

        function LoadForEditNonGeneratedBill(result) {

            $("#ContentPlaceHolder1_ddlCompanyBill").val("0");
            $("#chkNonGeneratedBill").prop("checked", true);
            $("#ContentPlaceHolder1_ddlCompanyBill").attr("disabled", true);

            CompanyGeneratedBillBySearch();

            if (result.CompanyBill.length > 0) {

                var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
                var totalPaymentAmount = 0.00;

                for (row = 0; row < result.CompanyBill.length; row++) {

                    var pd = _.findWhere(result.CompanyPaymentDetails, { BillId: result.CompanyBill[row].BillId });

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
                        tr += "<input type='checkbox' id='pay" + result.CompanyBill[row].CompanyPaymentId + "'" + chk + " onclick='CalculatePayment(this)' />";
                        totalPaymentAmount += result.CompanyBill[row].DueAmount;
                    }
                    else {
                        tr += "<input type='checkbox' id='pay" + result.CompanyBill[row].CompanyPaymentId + "' onclick='CalculatePayment(this)' />";
                    }

                    tr += "</td>";

                    tr += "<td style='width: 15%'>" + result.CompanyBill[row].ModuleName + "</td>";
                    tr += "<td style='width: 10%'>" + GetStringFromDateTime(result.CompanyBill[row].PaymentDate) + "</td>";
                    tr += "<td style='width: 20%'>" + result.CompanyBill[row].BillNumber + "</td>";
                    tr += "<td style='width: 15%'>" + result.CompanyBill[row].DueAmount + "</td>";

                    if (pd != null) {
                        tr += "<td style='width: 18%'>" + pd.PaymentAmount + "</td>";
                    }
                    else {
                        tr += "<td style='width: 18%'></td>";
                    }

                    if (pd != null) {
                        tr += "<td style='width: 15%'>" + (result.CompanyBill[row].DueAmount - pd.PaymentAmount) + "</td>";
                    }
                    else {
                        tr += "<td style='width: 15%'>" + result.CompanyBill[row].DueAmount + "</td>";
                    }

                    if (pd != null) {
                        tr += "<td style='display:none;'>" + pd.PaymentDetailsId + "</td>";
                    }
                    else {
                        tr += "<td style='display:none;'>" + 0 + "</td>";
                    }


                    tr += "<td style='display:none;'>" + 0 + "</td>";
                    tr += "<td style='display:none;'>" + 0 + "</td>";

                    tr += "<td style='display:none;'>" + result.CompanyBill[row].CompanyPaymentId + "</td>";
                    tr += "<td style='display:none;'>" + result.CompanyBill[row].BillId + "</td>";

                    tr += "</tr>";

                    $("#BillInfo tbody").append(tr);
                    tr = "";
                    isChecked = "0";
                }

                setTimeout(function () { CalculatePaymentWhenEdit() }, 500);

                return false;
            }
        }

        function DeleteCompanyPayment(paymentId) {
            PageMethods.DeleteCompanyPayment(paymentId, OnReceiveDeleteSucceed, OnReceiveDeleteFailed);
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

        function GoToAdvanceAdjustmentPage() {
            window.location = "/SalesAndMarketing/frmCompanyBillAdjustment.aspx";
            return false;
        }

        function CheckedPayment(PaymentId) {
            PageMethods.CheckedPaymentAdjustment(PaymentId, OnCheckedPaymentAdjustmentSucceded, OnCheckedPaymentAdjustmentFailed);
            return false;
        }
        function OnCheckedPaymentAdjustmentSucceded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GoToAdvanceAdjustmentPage();
            }
        }
        function OnCheckedPaymentAdjustmentFailed(result) {

        }

        function ApprovedPayment(paymentId, adjustmentType) {
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

        function CompanyBillAdvanceBySearch() {
            CommonHelper.SpinnerOpen();
            var companyId = $("#ContentPlaceHolder1_ddlCompany").val();
            $("#BillInfo tbody").html("");
            PageMethods.CompanyBillAdvanceBySearch(companyId, OnLoadAdvanceBillSucceeded, OnAdvanceBillFailed);
        }

        function OnLoadAdvanceBillSucceeded(result) {

            var guestCompany = {};
            var companyBill = new Array();

            guestCompany = result[0];
            companyBill = result[1];
            AdvanceBill = result[1];

            var list = result;
            var control = $('#ContentPlaceHolder1_ddlAdvanceLedger');

            control.empty();
            if (companyBill != null) {
                if (companyBill.length > 0) {
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < companyBill.length; i++) {
                        control.append('<option title="' + companyBill[i].LedgerNumber + '" value="' + companyBill[i].CompanyPaymentId + '">' + companyBill[i].LedgerNumber + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            control.val($("#ContentPlaceHolder1_hfCompanyAdvancePaymentId").val());
            if ($("#ContentPlaceHolder1_hfCompanyAdvancePaymentId").val() != "0") {
                var amount = _.findWhere(AdvanceBill, { CompanyPaymentId: parseInt($("#ContentPlaceHolder1_hfCompanyAdvancePaymentId").val(), 10) });
                $("#ContentPlaceHolder1_txtBalanceAmount").val(amount.AdvanceAmountRemaining);
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnAdvanceBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function PaymentConvertion() {

            var LedgerAmount = 0;
            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currencyType == "Local") {
                LedgerAmount = parseFloat($('#ContentPlaceHolder1_txtLedgerAmount').val());
                $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').val(LedgerAmount.toFixed(2));
            }
            else {
                LedgerAmount = parseFloat($('#ContentPlaceHolder1_txtConversionRate').val()) * parseFloat($('#ContentPlaceHolder1_txtLedgerAmount').val());

                $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').val(LedgerAmount.toFixed(2));
                $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').attr("disabled", true);
            }
        }

        function PaymentRefundShowHide() {
            if ($("#ContentPlaceHolder1_ddlAdjustmentType").val() == "Adjustment") {
                $("#AdvanceAdjustmentDiv").show();
                $("#AdvanceRefundDiv").hide();
                $("#ContentPlaceHolder1_txtLedgerAmount").val("");
            }
            else if ($("#ContentPlaceHolder1_ddlAdjustmentType").val() == "Refund") {
                $("#AdvanceAdjustmentDiv").hide();
                $("#AdvanceRefundDiv").show();

                if ($("#ContentPlaceHolder1_txtBalanceAmount").val() != "") {
                    $("#ContentPlaceHolder1_txtLedgerAmount").val($("#ContentPlaceHolder1_txtBalanceAmount").val());
                    PaymentConvertion();
                }
            }
        }

        function PaymentModeShowHideInformation() {
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'

            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
        }

        function CurrencyRateInfoEnable() {
            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            if (ddlCurrency == 0) {
                $('#ConversionPanel').hide()
            }
        }

    </script>    
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfPaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyPaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyBill" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyBillId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyAdvancePaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfConversionRate" runat="server" />
    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <asp:HiddenField ID="hfIsUpdatePermission" runat="server" />
    <asp:HiddenField ID="hfIsDeletePermission" runat="server" />
    <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
        <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Advance Adjustment</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Adjustment</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Adjustment Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPaymentDate" runat="server" class="control-label" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <input type="button" class="btn btn-primary" value="Search" onclick="CompanyGeneratedBillBySearch()" />
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
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Company Balance"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtBalanceAmount" CssClass="form-control" disabled runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div id="AdvanceAdjustmentDiv">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label6" runat="server" class="control-label" Text="Company Bill"></asp:Label>
                                </div>
                                <div class="col-md-7">
                                    <asp:DropDownList ID="ddlCompanyBill" runat="server" CssClass="form-control">
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
                        </div>
                        <div id="AdvanceRefundDiv" class="panel panel-default">
                            <div class="panel-heading">Payment Information</div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblPayMode" runat="server" class="control-label" Text="Payment Mode"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="Label8" runat="server" class="control-label required-field"
                                                Text="Payment Date"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtPaymentDate2" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="IntegratedGeneralLedgerDiv" style="display: none;">
                                        <div id="AdvanceDivPanel" style="display: none;">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblPaymentAccountHead" runat="server" class="control-label required-field"
                                                        Text="Payment Receive In"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <div id="CashReceiveAccountsInfo">
                                                        <asp:DropDownList ID="ddlCashReceiveAccountsInfo" runat="server" CssClass="form-control"
                                                            TabIndex="6">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div id="CardReceiveAccountsInfo" style="display: none;">
                                                        <asp:DropDownList ID="ddlCardReceiveAccountsInfo" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div id="ChequeReceiveAccountsInfo" style="display: none;">
                                                        <asp:DropDownList ID="ddlChequeReceiveAccountsInfo" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label9" runat="server" class="control-label required-field" Text="Income Purpose"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlIncomeSourceAccountsInfo" CssClass="form-control" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="CashOutDivPanel" style="display: none;">
                                            <div class="form-group" style="display: none;">
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblPaymentFrom" runat="server" class="control-label required-field"
                                                        Text="Payment From"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlPaymentFromAccountsInfo" CssClass="form-control" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblPaymentTo" runat="server" class="control-label required-field"
                                                        Text="Payment To"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlPaymentToAccountsInfo" CssClass="form-control" runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group" id="CompanyProjectPanel" style="display: none;">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblGLCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlGLCompany" CssClass="form-control" runat="server" onchange="PopulateProjects();">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblGLProject" runat="server" class="control-label" Text="Project"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlGLProject" CssClass="form-control" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblCurrency" runat="server" class="control-label required-field" Text="Currency Type"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlCurrency" TabIndex="6" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblReceiveAmount" runat="server" class="control-label required-field"
                                                Text="Payment Amount"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtLedgerAmount" disabled runat="server" CssClass="form-control" TabIndex="7"> </asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="ConversionPanel" class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblConversionRate" runat="server" class="control-label required-field"
                                                Text="Conversion Rate"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtConversionRate" CssClass="form-control" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="txtConversionRateHiddenField" runat="server"></asp:HiddenField>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblCurrencyAmount" runat="server" class="control-label" Text="Calculated Amount"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:HiddenField ID="txtCalculatedLedgerAmountHiddenField" runat="server"></asp:HiddenField>
                                            <asp:TextBox ID="txtCalculatedLedgerAmount" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="CashPaymentAccountHeadDiv" style="display: none;">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label10" runat="server" class="control-label required-field"
                                                    Text="Cash Head"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <input id="txtCashPayment" type="text" class="form-control" />
                                                <div style="display: none;">
                                                    <asp:DropDownList ID="ddlCashPayment" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblChecqueNumber" runat="server" class="control-label required-field"
                                                    Text="Cheque Number"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblCompanyBank" runat="server" class="control-label required-field"
                                                    Text="Bank Name"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <input id="txtCompanyBank" type="text" class="form-control" />
                                                <div style="display: none;">
                                                    <asp:DropDownList ID="ddlCompanyBank" CssClass="form-control" runat="server" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="CardPaymentAccountHeadDiv" style="display: none;">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label11" runat="server" class="control-label required-field" Text="Card Type"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlCardType" runat="server" TabIndex="8" CssClass="form-control">
                                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                                    <asp:ListItem Value="a">American Express</asp:ListItem>
                                                    <asp:ListItem Value="m">Master Card</asp:ListItem>
                                                    <asp:ListItem Value="v">Visa Card</asp:ListItem>
                                                    <asp:ListItem Value="d">Discover Card</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblCardNumber" runat="server" class="control-label" Text="Card Number"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblBankId" runat="server" class="control-label required-field" Text="Bank Name"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <input id="txtBankId" type="text" class="form-control" />
                                                <div style="display: none;">
                                                    <asp:DropDownList ID="ddlBankId" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="display: none;">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label12" runat="server" class="control-label" Text="Expiry Date"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label13" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtCardHolderName" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <input type="button" id="btnAdjustment" class="btn btn-primary" value="Adjustment" onclick="GenerateCompanyBill()" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Adjustment Search
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCompanyForSearch" runat="server" CssClass="form-control">
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
                                    <th style="width: 15%;">Ledger Number</th>
                                    <th style="width: 10%;">Payment Date</th>
                                    <th style="width: 20%;">Company Name</th>
                                    <th style="width: 15%;">Adjustment type</th>
                                    <th style="width: 30%;">Remarks</th>
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
