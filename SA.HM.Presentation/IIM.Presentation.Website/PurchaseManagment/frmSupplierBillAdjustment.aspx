<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmSupplierBillAdjustment.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmSupplierBillAdjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var AdvanceBill = new Array();
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            PaymentRefundShowHide();

            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtCompanyBank", "ContentPlaceHolder1_ddlCompanyBank", "ContentPlaceHolder1_ddlCompanyBank");
            CommonHelper.AutoSearchClientDataSource("txtSupplierName", "ContentPlaceHolder1_ddlSupplierName", "ContentPlaceHolder1_ddlSupplierName");
            CommonHelper.AutoSearchClientDataSource("txtCashPayment", "ContentPlaceHolder1_ddlCashPayment", "ContentPlaceHolder1_ddlCashPayment");

            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmount = '<%=txtCalculatedLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmountHiddenField = '<%=txtCalculatedLedgerAmountHiddenField.ClientID%>'

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlSupplier").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSupplierForSearch").select2({
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

            $('#ContentPlaceHolder1_txtAdjustmentDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtPaymentDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            if (IsCanSave) {
                $('#btnAdjustment').show();
            } else {
                $('#btnAdjustment').hide();
            }

            $("#ContentPlaceHolder1_ddlAdvanceLedger").change(function () {

                var selectedAdvanceBill = _.findWhere(AdvanceBill, { SupplierPaymentId: parseInt($(this).val()) });

                if (selectedAdvanceBill != null) {
                    $("#ContentPlaceHolder1_txtBalanceAmount").val(selectedAdvanceBill.AdvanceAmountRemaining);

                    if ($("#ContentPlaceHolder1_ddlAdjustmentType").val() == "Refund") {
                        $("#ContentPlaceHolder1_txtLedgerAmount").val(selectedAdvanceBill.AdvanceAmountRemaining);
                        PaymentConvertion();
                        $("#ContentPlaceHolder1_txtLedgerAmount").attr("disabled", false);
                    }
                    else {
                        $("#ContentPlaceHolder1_txtLedgerAmount").val("");
                        $("#ContentPlaceHolder1_txtLedgerAmount").attr("disabled", true);
                    }
                }
                else {
                    $("#ContentPlaceHolder1_txtLedgerAmount").val("");
                    $("#ContentPlaceHolder1_txtBalanceAmount").val("");
                }
            });

            $("#ContentPlaceHolder1_ddlAdjustmentType").change(function () {
                if ($(this).val() == "Adjustment") {
                    $("#AdvanceAdjustmentDiv").show();
                    $("#AdvanceRefundDiv").hide();
                    $("#ContentPlaceHolder1_txtLedgerAmount").val("");
                    $("#ContentPlaceHolder1_txtLedgerAmount").attr("disabled", true);
                }
                else if ($(this).val() == "Refund") {
                    $("#AdvanceAdjustmentDiv").hide();
                    $("#AdvanceRefundDiv").show();
                    $("#ContentPlaceHolder1_txtLedgerAmount").attr("disabled", false);
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

            $('#' + txtLedgerAmount).blur(function () {
                //            var selectedIndex = parseFloat($('#' + ddlCurrency).prop("selectedIndex"));
                //                if (selectedIndex < 1) {

                var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
                if (currencyType == "Local") {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val());

                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val()) * parseFloat($('#' + txtConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", true);
                }
            });

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

        function CurrencyRateInfoEnable() {
            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            if (ddlCurrency == 0) {
                $('#ConversionPanel').hide()
            }
        }

        function SupplierBillAdvanceBySearch() {
            CommonHelper.SpinnerOpen();
            var supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            $("#BillInfo tbody").html("");
            PageMethods.SupplierBillAdvanceBySearch(supplierId, OnLoadAdvanceBillSucceeded, OnAdvanceBillFailed);
        }

        function OnLoadAdvanceBillSucceeded(result) {

            var guestCompany = {};
            var supplierBill = new Array();

            guestCompany = result[0];
            supplierBill = result[1];
            AdvanceBill = result[1];

            var list = result;
            var control = $('#ContentPlaceHolder1_ddlAdvanceLedger');

            control.empty();
            if (supplierBill != null) {
                if (supplierBill.length > 0) {
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < supplierBill.length; i++) {
                        control.append('<option title="' + supplierBill[i].LedgerNumber + '" value="' + supplierBill[i].SupplierPaymentId + '">' + supplierBill[i].LedgerNumber + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            control.val($("#ContentPlaceHolder1_hfSupplierPaymentId").val());
            if ($("#ContentPlaceHolder1_hfSupplierPaymentId").val() != "0") {
                var amount = _.findWhere(AdvanceBill, { SupplierPaymentId: parseInt($("#ContentPlaceHolder1_hfSupplierPaymentId").val(), 10) });
                $("#ContentPlaceHolder1_txtBalanceAmount").val(amount.AdvanceAmountRemaining);
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnAdvanceBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function SupplierGeneratedBillBySearch() {
            CommonHelper.SpinnerOpen();
            var supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            $("#ContentPlaceHolder1_txtLedgerAmount").val("");
            $("#ContentPlaceHolder1_txtBalanceAmount").val("");
            $("#BillInfo tbody").html("");
            PageMethods.SupplierGeneratedBillBySearch(supplierId, OnLoadGeneratedBillSucceeded, OnGeneratedBillFailed);
            SupplierBillAdvanceBySearch();
        }

        function OnLoadGeneratedBillSucceeded(result) {

            var supplier = {};
            var supplierBill = new Array();

            supplier = result[0];
            supplierBill = result[1];

            var row = 0, tr = "", chk = "checked='checked'", isChecked = "0";
            var totalPaymentAmount = 0.00;

            for (row = 0; row < supplierBill.length; row++) {

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 8%'> ";

                if (isChecked == "1") {
                    tr += "<input type='checkbox' id='pay" + supplierBill[row].SupplierPaymentId + "'" + chk + " onclick='CalculatePayment(this)' />";
                    totalPaymentAmount += supplierBill[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + supplierBill[row].SupplierPaymentId + "' onclick='CalculatePayment(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 15%'>" + GetStringFromDateTime(supplierBill[row].PaymentDate) + "</td>";
                tr += "<td style='width: 25%'>" + supplierBill[row].BillNumber + "</td>";
                tr += "<td style='width: 16%'>" + supplierBill[row].DueAmount + "</td>";
                tr += "<td style='width: 20%'>0</td>";
                tr += "<td style='width: 16%'>" + supplierBill[row].DueAmount + "</td>";

                tr += "<td style=display:none;'>" + supplierBill[row].PaymentDetailsId + "</td>";
                tr += "<td style=display:none;'>" + supplierBill[row].SupplierPaymentId + "</td>";
                tr += "<td style=display:none;'>" + supplierBill[row].BillId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnGeneratedBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function GenerateSupplierBill() {

            if ($("#ContentPlaceHolder1_ddlSupplier").val() == "0") {
                toastr.info("Please Select Supplier");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlAdvanceLedger").val() == "0") {
                toastr.info("Please Select Advannce Bill To Adjustment");
                return false;
            }

            if ($("#ContentPlaceHolder1_ddlAdjustmentType").val() == "Adjustment") {
                if ($("#ContentPlaceHolder1_txtAdjustedAmount").val() == "") {
                    toastr.info("Please Select Bill To Adjustment");
                    return false;
                }
                else if (parseFloat($("#ContentPlaceHolder1_txtAdjustedAmount").val()) == 0) {
                    toastr.info("Please Select Bill To Adjustment");
                    return false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlAdjustmentType").val() == "Refund") {

                if ($("#ContentPlaceHolder1_ddlPayMode").val() == "0") {
                    toastr.info("Please Select Payment Mode");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtPaymentDate2").val() == "") {
                    toastr.info("Please Give Payment Date.");
                    return false;

                }
                else if ($("#ContentPlaceHolder1_txtLedgerAmount").val() == "") {
                    toastr.info("Please Give Refund Amount");
                    return false;
                }
                else if (parseFloat($("#ContentPlaceHolder1_txtLedgerAmount").val()) == 0) {
                    toastr.info("Please Give Refund Amount");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cash") {
                    if ($("#ContentPlaceHolder1_ddlCashPayment").val() == "0" || $("#txtCashPayment").val() == "") {
                        toastr.info("Please Select Cash Payment Account.");
                        return false;
                    }
                }
                else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Card") {
                    if ($("#ContentPlaceHolder1_ddlCompanyBank").val() == "0" || $("#txtCompanyBank").val() == "") {
                        toastr.info("Please Select Bank Payment Account.");
                        return false;
                    }
                }
                else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cheque") {
                    if ($("#ContentPlaceHolder1_ddlCompanyBank").val() == "0" || $("#txtCompanyBank").val() == "") {
                        toastr.info("Please Select Bank Payment Account.");
                        return false;
                    }
                }
            }

            CommonHelper.SpinnerOpen();

            var totalPayment = 0.00, advanceAmount = 0.00, detailsId = 0, paymentId = 0, accountingPostingHeadId = 0;
            var supplierId = 0, adjustmentType = '', supplierPaymentAdvanceId = 0, adjustmentAmount = 0;
            var paymentType = '';

            var SupplierPaymentDetails = new Array();
            var SupplierPaymentDetailsEdited = new Array();
            var SupplierPaymentDetailsDeleted = new Array();

            if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cash") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCashPayment").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Card") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCompanyBank").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cheque") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCompanyBank").val();
            }

            supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            paymentId = $("#ContentPlaceHolder1_hfPaymentId").val();
            adjustmentType = $("#ContentPlaceHolder1_ddlAdjustmentType").val();
            supplierPaymentAdvanceId = $("#ContentPlaceHolder1_ddlAdvanceLedger").val();
            adjustmentAmount = $("#ContentPlaceHolder1_txtLedgerAmount").val();
            paymentType = $("#ContentPlaceHolder1_ddlPayMode").val();

            var SupplierPayment = {};

            if ($("#ContentPlaceHolder1_ddlAdjustmentType").val() == "Adjustment") {

                SupplierPayment = {
                    PaymentId: paymentId,
                    PaymentFor: 'Adjustment',
                    AdjustmentType: adjustmentType,
                    SupplierPaymentAdvanceId: supplierPaymentAdvanceId,
                    SupplierId: supplierId,
                    PaymentDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtAdjustmentDate").val(), '/'),
                    AdvanceAmount: advanceAmount,
                    AdjustmentAmount: 0,
                    PaymentType: 'Adjustment',
                    AccountingPostingHeadId: accountingPostingHeadId,
                    Remarks: $("#ContentPlaceHolder1_txtRemarks").val(),
                    ChequeNumber: null,
                    ChequeDate: null,
                    CurrencyId: null,
                    ConvertionRate: null
                };
            }
            else {
                SupplierPayment = {
                    PaymentId: paymentId,
                    PaymentFor: 'Adjustment',
                    AdjustmentType: adjustmentType,
                    SupplierPaymentAdvanceId: supplierPaymentAdvanceId,
                    SupplierId: supplierId,
                    PaymentDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtAdjustmentDate").val(), '/'),
                    AdvanceAmount: advanceAmount,
                    AdjustmentAmount: adjustmentAmount,
                    PaymentType: paymentType,
                    AccountingPostingHeadId: accountingPostingHeadId,
                    Remarks: $("#ContentPlaceHolder1_txtRemarks").val(),
                    ChequeNumber: $("#ContentPlaceHolder1_txtChecqueNumber").val(),
                    ChequeDate: null,
                    CurrencyId: $("#ContentPlaceHolder1_ddlCurrency").val(),
                    ConvertionRate: $("#ContentPlaceHolder1_txtConversionRate").val() == "" ? "0" : $("#ContentPlaceHolder1_txtConversionRate").val()
                };
            }

            $("#BillInfo tbody tr").each(function () {

                detailsId = parseInt($(this).find("td:eq(6)").text(), 10);

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId == 0) {
                    SupplierPaymentDetails.push({
                        PaymentDetailsId: 0,
                        SupplierPaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        PaymentAmount: $(this).find("td:eq(4)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId > 0) {
                    SupplierPaymentDetailsEdited.push({
                        PaymentDetailsId: detailsId,
                        SupplierPaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        PaymentAmount: $(this).find("td:eq(4)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == false && detailsId > 0) {
                    SupplierPaymentDetailsDeleted.push({
                        PaymentDetailsId: detailsId,
                        SupplierPaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        PaymentAmount: $(this).find("td:eq(4)").text()
                    });
                }
            });

            PageMethods.AdjustedSupplierBillPayment(SupplierPayment, SupplierPaymentDetails, SupplierPaymentDetailsEdited, SupplierPaymentDetailsDeleted, OnGenerateSupplierBillSucceeded, OnGenerateSupplierBillFailed);

            return false;
        }

        function OnGenerateSupplierBillSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnGenerateSupplierBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function PerformClearAction() {

            $("#ContentPlaceHolder1_hfPaymentId").val("0");
            $("#ContentPlaceHolder1_hfSupplierPaymentId").val("0");
            $("#ContentPlaceHolder1_hfSupplierBill").val("0");
            $("#ContentPlaceHolder1_hfSupplierBillId").val("0");

            $("#BillInfo tbody").html("");
            $('#ContentPlaceHolder1_ddlSupplier').val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlSupplierBill").val("0");
            $("#ContentPlaceHolder1_txtBalanceAmount").val("");
            $("#ContentPlaceHolder1_txtRemainingAmount").val("");
            $("#ContentPlaceHolder1_txtAdjustedAmount").val("");
            $("#ContentPlaceHolder1_txtAdjustmentDate").datepicker("setDate", DayOpenDate);
            $("#chkNonGeneratedBill").prop("checked", false);
	
            $("#btnAdjustment").val("Bill Adjustment");
			if (IsCanSave) {
                $('#btnAdjustment').show();
            } else {
                $('#btnAdjustment').hide();
            }
            $("#ContentPlaceHolder1_txtRemarks").val("");
            //$("#ContentPlaceHolder1_ddlAdvanceLedger").val("0");

            $("#chkNonGeneratedBill").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlSupplierBill").attr("disabled", false);

            //$("#ContentPlaceHolder1_ddlAdvanceLedger").val("0");
            $("#ContentPlaceHolder1_ddlCashPayment").val();
            $("#ContentPlaceHolder1_ddlCompanyBank").val();
            $("#txtCashPayment").val("");
            $("#txtCompanyBank").val("")
            $("#ContentPlaceHolder1_txtChecqueNumber").val("");
            $("#ContentPlaceHolder1_ddlCurrency").val("1");

            $("#ContentPlaceHolder1_ddlAdvanceLedger").empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');

            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');
            $("#<%=txtExpireDate.ClientID %>").val('');

            //$("#<%=ddlCurrency.ClientID %>").val('');
            $("#<%=txtCalculatedLedgerAmount.ClientID %>").val('');
            $("#<%=txtConversionRate.ClientID %>").val('');
        }

        function CalculatePayment(control) {

            var tr = $(control).parent().parent();
            var billAmount = parseFloat($(tr).find("td:eq(3)").text());
            var billId = $(tr).find("td:eq(7)").text();
            var isChecked = $(tr).find("td:eq(0)").find("input").is(":checked");
            var balanceAmount = parseFloat($("#ContentPlaceHolder1_txtBalanceAmount").val());

            var totalPayment = 0.00;

            $("#BillInfo tbody tr").each(function () {

                var currentBillId = $(this).find("td:eq(7)").text();

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true && currentBillId != billId) {
                    totalPayment += parseFloat($(this).find("td:eq(3)").text());
                }
            });

            var adjustedAmount = 0.00, remainingAmount = 0.00, canAdjustedAmount = 0.00;

            if (isChecked == false) {
                $(tr).find("td:eq(4)").text("0");
                $(tr).find("td:eq(5)").text(toFixed(billAmount, 2));
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
                $(tr).find("td:eq(4)").text(canAdjustedAmount);
                $(tr).find("td:eq(5)").text(toFixed(billAmount - canAdjustedAmount, 2));
            }
            else {
                $(tr).find("td:eq(4)").text(billAmount);
                $(tr).find("td:eq(5)").text("0");
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

        function SearchPayment() {

            var dateFrom = null, dateTo = null, supplierId = 0;

            supplierId = $("#ContentPlaceHolder1_ddlSupplierForSearch").val();

            if ($("#ContentPlaceHolder1_txtFromDate").val() != "") {
                dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/')
            }
            if ($("#ContentPlaceHolder1_txtToDate").val() != "") {
                dateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/')
            }

            $("#BillInfoSearch tbody").html("");
            PageMethods.GetSupplierPaymentBySearch(supplierId, dateFrom, dateTo, OnSearchPaymentSucceeded, OnSearchPaymentFailed);

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

                tr += "<td style='width: 15%'>" + result[row].LedgerNumber + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 20%'>" + result[row].SupplierName + "</td>";
                tr += "<td style='width: 15%'>" + result[row].AdjustmentType + "</td>";

                if (result[row].Remarks != "" && result[row].Remarks != null)
                    tr += "<td style='width: 30%'>" + result[row].Remarks + "</td>";
                else
                    tr += "<td style='width: 30%'></td>";

                if (result[row].ApprovedStatus == null) {
                    tr += "<td style='width:10%;'>";
                    if (IsCanSave) {
                        tr += "<a href='javascript:void();' onclick= 'javascript:return ApprovedPayment(" + result[row].PaymentId + ")' ><img alt='approved' src='../Images/approved.png' /></a>";
                    }

                    tr += "&nbsp;&nbsp;";
                    if (IsCanEdit) {
                    tr += "<a onclick=\"javascript:return FIllForEdit(" + result[row].PaymentId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                    }

                    tr += "&nbsp;&nbsp;";
                    if (IsCanDelete) {
                    tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteSupplierPayment(" + result[row].PaymentId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
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
            CommonHelper.SpinnerOpen();
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#ContentPlaceHolder1_hfPaymentId").val(result.SupplierPayment.PaymentId);
            $("#ContentPlaceHolder1_ddlSupplier").val(result.SupplierPayment.SupplierId).trigger('change');
            $("#ContentPlaceHolder1_ddlAdjustmentType").val(result.SupplierPayment.AdjustmentType);
            $("#ContentPlaceHolder1_txtRemarks").val(result.SupplierPayment.Remarks);
            $("#ContentPlaceHolder1_hfSupplierPaymentId").val(result.SupplierPayment.SupplierPaymentAdvanceId);

            //if (IsCanUpdate) {
            //    $('#btnAdjustment').show();
            //} else {
            //    $('#btnAdjustment').hide();
            //}

            $("#btnAdjustment").val("Update Bill Adjustment");
            $("#BillInfo tbody").html("");

            PaymentRefundShowHide();

            //------------ Refund

            if (result.SupplierPayment.AdjustmentType == "Refund") {

                $("#<%=ddlPayMode.ClientID %>").val(result.SupplierPayment.PaymentType);

                $("#<%=hfCurrencyType.ClientID %>").val(result.SupplierPayment.CurrencyType);

                if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                    $('#ConversionPanel').show();
                    $("#<%=txtCalculatedLedgerAmount.ClientID %>").val((result.SupplierPayment.AdjustmentAmount * result.SupplierPayment.ConvertionRate).toFixed(2));
                    $("#<%=txtCalculatedLedgerAmount.ClientID %>").attr("disabled", true);
                    $("#<%=txtConversionRate.ClientID %>").val(result.SupplierPayment.ConvertionRate);
                }
                else {
                    $('#ConversionPanel').hide();
                    $("#<%=txtCalculatedLedgerAmount.ClientID %>").attr("disabled", false);
                    $("#<%=txtConversionRate.ClientID %>").val('');
                }

                $("#<%=ddlCurrency.ClientID %>").val(result.SupplierPayment.CurrencyId);
                $("#<%=txtLedgerAmount.ClientID %>").val(result.SupplierPayment.AdjustmentAmount);
                $("#<%=txtPaymentDate2.ClientID %>").val(GetStringFromDateTime(result.SupplierPayment.PaymentDate));

                $("#<%=txtChecqueNumber.ClientID %>").val(result.SupplierPayment.ChequeNumber);

                $("#<%=ddlCompanyBank.ClientID %>").val(result.SupplierPayment.AccountingPostingHeadId + '');
                $('#txtCompanyBank').val($("#<%=ddlCompanyBank.ClientID %> option:selected").text());

                $("#<%=ddlCashPayment.ClientID %>").val(result.SupplierPayment.AccountingPostingHeadId + '');
                $('#txtCashPayment').val($("#<%=ddlCashPayment.ClientID %> option:selected").text());

                //------------------

                if (result.SupplierPayment.PaymentType == "Cash") {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').show();
                    $('#BankPaymentAccountHeadDiv').hide();
                }
                else if (result.SupplierPayment.PaymentType == "Card") {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').hide();
                    $('#BankPaymentAccountHeadDiv').hide();
                }
                else if (result.SupplierPayment.PaymentType == "Cheque") {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').show();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').hide();
                    $('#BankPaymentAccountHeadDiv').hide();
                }
                else if (result.SupplierPayment.PaymentType == "Adjustment") {
                    $('#CashReceiveAccountsInfo').show();
                    $('#CardReceiveAccountsInfo').hide();
                    $('#ChequeReceiveAccountsInfo').hide();
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                }
            }

            SupplierBillAdvanceBySearch();

            //------------ Details

            var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
            var totalPaymentAmount = 0.00;

            for (row = 0; row < result.SupplierBill.length; row++) {

                var pd = _.findWhere(result.PaymentDetailsInfo, { SupplierPaymentId: result.SupplierBill[row].SupplierPaymentId });

                if (pd != null) {
                    isChecked = "1";
                }

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 8%'> ";

                if (isChecked == "1") {
                    tr += "<input type='checkbox' id='pay" + result.SupplierBill[row].CompanyPaymentId + "'" + chk + " onclick='CalculatePayment(this)' />";
                    totalPaymentAmount += result.SupplierBill[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result.SupplierBill[row].CompanyPaymentId + "' onclick='CalculatePayment(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 15%'>" + GetStringFromDateTime(result.SupplierBill[row].PaymentDate) + "</td>";
                tr += "<td style='width: 25%'>" + result.SupplierBill[row].BillNumber + "</td>";
                tr += "<td style='width: 16%'>" + result.SupplierBill[row].DueAmount + "</td>";

                if (pd != null) {
                    tr += "<td style='width: 20%'>" + pd.PaymentAmount + "</td>";
                }
                else {
                    tr += "<td style='width: 20%'></td>";
                }

                if (pd != null) {
                    tr += "<td style='width: 16%'>" + (result.SupplierBill[row].DueAmount - pd.PaymentAmount) + "</td>";
                }
                else {
                    tr += "<td style='width: 16%'>" + result.SupplierBill[row].DueAmount + "</td>";
                }

                if (pd != null) {
                    tr += "<td style=display:none;'>" + pd.PaymentDetailsId + "</td>";
                }
                else {
                    tr += "<td style=display:none;'>" + 0 + "</td>";
                }

                tr += "<td style=display:none;'>" + result.SupplierBill[row].SupplierPaymentId + "</td>";
                tr += "<td style=display:none;'>" + result.SupplierBill[row].BillId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
                isChecked = "0";
            }

            setTimeout(function () { CalculatePaymentWhenEdit() }, 500)

            $("#ContentPlaceHolder1_btnSave").val("Update");
            $("#myTabs").tabs({ active: 0 });

            CommonHelper.SpinnerClose();
        }

        function OnFillFormObjectFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }

        function CallCalculatePayment() { CalculatePayment(); }

        function DeleteSupplierPayment(paymentId) {
            if (!confirm("Do you want To delete?")) {
                return false;
            }
            PageMethods.DeleteSupplierPayment(paymentId, OnReceiveDeleteSucceed, OnReceiveDeleteFailed);
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

        function ApprovedPayment(paymentId) {
            if (!confirm("Do you want to approve?")) {
                return false;
            }
            PageMethods.ApprovedPaymentAdjustment(paymentId, OnApporavalSucceed, OnApporavalFailed);
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


    </script>

    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfPaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSupplierPaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSupplierBill" runat="server" Value="0" />
    <asp:HiddenField ID="hfSupplierBillId" runat="server" Value="0" />

    <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfConversionRate" runat="server" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />

    <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
        <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" CssClass="form-control">
        </asp:DropDownList>
    </div>

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Supplier Bill Adjustment</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Supplier Bill Adjustment</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Supplier Bill Search
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPaymentDate" runat="server" class="control-label" Text="Supplier"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <input type="button" class="btn btn-primary" value="Search" onclick="SupplierGeneratedBillBySearch()" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Adjustment Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAdjustmentType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Advance Adjustment" Value="Adjustment"></asp:ListItem>
                                    <asp:ListItem Text="Advance Refund" Value="Refund"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Advance Ledger"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAdvanceLedger" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Supplier Advance"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtBalanceAmount" CssClass="form-control" disabled runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div id="AdvanceAdjustmentDiv" class="panel panel-default">
                            <div class="panel-heading">Supplier Bill Information</div>
                            <div class="panel-body">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <table id="BillInfo" class="table table-bordered table-condensed table-hover table-responsive">
                                            <thead>
                                                <tr>
                                                    <th style="width: 8%;">Select</th>
                                                    <th style="width: 15%;">Bill Date</th>
                                                    <th style="width: 25%;">Bill Number</th>
                                                    <th style="width: 16%;">Due Amount</th>
                                                    <th style="width: 20%;">Payment Amount</th>
                                                    <th style="width: 16%;">Remaining Amount</th>
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
                                                <%--<asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                    <asp:ListItem Value="Card">Card</asp:ListItem>
                                    <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                    <asp:ListItem Value="Adjustment">Adjustment</asp:ListItem>
                                    <asp:ListItem Value="Loan">Supplier Loan</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="Label7" runat="server" class="control-label required-field"
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
                                                    <asp:Label ID="Label8" runat="server" class="control-label required-field" Text="Income Purpose"></asp:Label>
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
                                            <%--<asp:Label ID="lblDisplayConvertionRate" runat="server" class="control-label" Text=""></asp:Label>--%>
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
                                                <asp:Label ID="Label9" runat="server" class="control-label required-field"
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
                                                <asp:Label ID="Label10" runat="server" class="control-label required-field" Text="Card Type"></asp:Label>
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
                                                    <asp:Label ID="Label11" runat="server" class="control-label" Text="Expiry Date"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label12" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
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
                                <input type="button" id="btnAdjustment" class="btn btn-primary" value="Bill Adjustment" onclick="GenerateSupplierBill()" />
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
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Supplier"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSupplierForSearch" runat="server" CssClass="form-control">
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
                                    <th style="width: 20%;">Supplier Name</th>
                                    <th style="width: 15%;">Adjustment Type</th>
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
