<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="true"
    CodeBehind="frmCompanyBillReceive.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmCompanyBillReceive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var CompanyGeneratedBill = new Array();

        var ReceiveInformation = new Array();

        var ReceiveInformationDeleted = new Array();

        var ItemEdited = ""; var indexEdited = -1;

        $(function () {
            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmount = '<%=txtCalculatedLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmouvalidntHiddenField = '<%=txtCalculatedLedgerAmountHiddenField.ClientID%>'

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlCompanyForSearch").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCompanyBill").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_gvPaymentInfo_ChkAllSelect').click(function () {
                if ($('#ContentPlaceHolder1_gvPaymentInfo_ChkAllSelect').is(':checked')) {
                    CheckAllCheckBoxCreate()
                }
                else {
                    UnCheckAllCheckBoxCreate();
                }
            });

            CommonHelper.ApplyDecimalValidation();

            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtCompanyBank", "ContentPlaceHolder1_ddlCompanyBank", "ContentPlaceHolder1_ddlCompanyBank");
            CommonHelper.AutoSearchClientDataSource("txtCompanySearch", "ContentPlaceHolder1_ddlCompany", "ContentPlaceHolder1_hfCmpSearch");
            CommonHelper.AutoSearchClientDataSource("txtBankPayment", "ContentPlaceHolder1_ddlBankPayment", "ContentPlaceHolder1_ddlBankPayment");
            CommonHelper.AutoSearchClientDataSource("txtCashPayment", "ContentPlaceHolder1_ddlCashPayment", "ContentPlaceHolder1_ddlCashPayment");
            CommonHelper.AutoSearchClientDataSource("txtCashAndCashEquivalantPayment", "ContentPlaceHolder1_ddlCashAndCashEquivalantPayment", "ContentPlaceHolder1_ddlCashAndCashEquivalantPayment");

            $('#txtCompanySearch').blur(function () {
                if ($(this).val() != "") {
                    var cmpId = $("#ContentPlaceHolder1_hfCmpSearch").val();
                    if (cmpId != "") {
                        LoadCompanyInfo(cmpId);
                        LoadCompanyBill();
                        $("#CompanyInfo").show();
                    }
                    else {
                        $("#CompanyInfo").hide();
                    }
                }
            });

            $('#txtBankPayment').blur(function () {
                if ($(this).val() == "") {
                    $("#<%=ddlBankPayment.ClientID %>").val("0");
                }
            });

            $('#txtCashPayment').blur(function () {
                if ($(this).val() == "") {
                    $("#<%=ddlCashPayment.ClientID %>").val("0");
                }
            });

            $('#txtCashAndCashEquivalantPayment').blur(function () {
                if ($(this).val() == "") {
                    $("#<%=ddlCashAndCashEquivalantPayment.ClientID %>").val("0");
                }
            });

            $('#ContentPlaceHolder1_txtAdjustmentAmount').blur(function () {
                var paymentTotal = $("#ContentPlaceHolder1_txtTotalAmount").val();
                var adjustmentAmount = $("#ContentPlaceHolder1_txtAdjustmentAmount").val();
                var paymentAmount = parseFloat(paymentTotal) - parseFloat(adjustmentAmount);

                $("#ContentPlaceHolder1_txtLedgerAmount").val(paymentAmount);
                $("#ContentPlaceHolder1_txtCalculatedLedgerAmount").val(paymentAmount);
                CurrencyConvertion();

            });

            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currencyType == "Local") {
                $('#' + txtConversionRate).val("1")
                $('#' + txtCalculatedLedgerAmount).val("0");
                $('#ConversionPanel').hide();
                $('#' + txtCalculatedLedgerAmount).attr("disabled", false);
            }
            else {
                $('#ConversionPanel').show();
                $('#' + txtCalculatedLedgerAmount).val("0");
            }

            var transactionType = '<%=ddlTransactionType.ClientID%>'
            $('#' + transactionType).change(function () {
                if ($('#' + transactionType).val() == "Receive") {
                    $('#AdjustmentHeadDiv').hide();
                    $("#<%=lblAdvanceAmount.ClientID %>").text("Advance Amount");
                }
                else {
                    $('#AdjustmentHeadDiv').show();
                    $("#<%=lblAdvanceAmount.ClientID %>").text("Payment Amount");
                }
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
                    $("#<%=txtConversionRate.ClientID %>").val('1');
                    $('#' + txtCalculatedLedgerAmount).val("");
                    $('#ConversionPanel').hide();
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", false);
                }
                else {
                    $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                    $("#<%=hfConversionRate.ClientID %>").val(result.ConversionRate);

                    $('#ConversionPanel').show();
                }
                CurrencyRateInfoEnable();
                CurrencyConvertion();
            }

            function OnLoadConversionRateFailed() {
            }

            $('#' + txtLedgerAmount).blur(function () {
                debugger;
                var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
                if (currencyType == "Local") {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val());

                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val()) * parseFloat($('#' + txtConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", true);
                }
            });

            $('#' + txtConversionRate).blur(function () {
                var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
                if (currencyType == "Local") {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val());
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val()) * parseFloat($('#' + txtConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", true);
                }
            });
            debugger;
            var ddlPayMode = '<%=ddlPayMode.ClientID%>';

            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'
            $('#' + ddlPayMode).change(function () {
                PaymentModeShowHideInformation();
            });

            var ddlPaymentMode = '<%=ddlPaymentMode.ClientID%>';

            $('#' + ddlPaymentMode).change(function () {
                PaymentModeNewShowHideInformation();
            });


            $(function () {
                $("#myTabs").tabs();
            });

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'

            $('#' + txtFromDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtToDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtToDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtFromDate).datepicker("option", "maxDate", selectedDate);
                }
            });

            //$('#ContentPlaceHolder1_txtFromDate').datepicker({
            //    changeMonth: true,
            //    changeYear: true,
            //    dateFormat: innBoarDateFormat,
            //    onClose: function (selectedDate) {
            //        $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
            //    }
            //}).datepicker("setDate", DayOpenDate);

            //$('#ContentPlaceHolder1_txtToDate').datepicker({
            //    changeMonth: true,
            //    changeYear: true,
            //    dateFormat: innBoarDateFormat,
            //    onClose: function (selectedDate) {
            //        $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
            //    }
            //}).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtPaymentDate2').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtChequeDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });//.datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlCompanyBill").change(function () {

                var companyBillId = $(this).val();

                var bill = _.findWhere(CompanyGeneratedBill, { CompanyBillId: parseInt(companyBillId, 10) });
                if (bill != null) {
                    $("#ContentPlaceHolder1_ddlCurrency").val(bill.BillCurrencyId);
                    $("#ContentPlaceHolder1_ddlCurrency").trigger("change");
                    CurrencyConvertion();
                }

                SearchCompanyBill();
            });

            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#ContentPlaceHolder1_btnSave").hide();

        });

        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Membership</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Member Bill Payment</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            //EnableDisable For DropDown Change event--------------

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            //if (IsCanSave) {
            //    $('#ContentPlaceHolder1_btnSave').show();
            //} else {
            //    $('#ContentPlaceHolder1_btnSave').hide();
            //}

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() == "0") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#ChequeNChequeNumber').hide();
                $('#CurrencyAndPaymentAmountDiv').hide();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cash") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Bank") {
                $('#BankPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Loan") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').show();
                $('#CurrencyAndPaymentAmountDiv').show();
            }

            ddlGLProject = '<%=ddlGLProject.ClientID%>'
            var ddlGLCompany = '<%=ddlGLCompany.ClientID%>'
            var selectedIndex = parseFloat($('#' + ddlGLCompany).prop("selectedIndex"));
            if (selectedIndex > 0) {
                $("#" + ddlGLProject).attr("disabled", false);
            }
            else {
                $("#" + ddlGLProject).attr("disabled", true);
            }

            var txtCardNumber = '<%=txtCardNumber.ClientID%>'
            $('#' + txtCardNumber).blur(function () {
                //validateCard();
            });
            var ddlCardType = '<%=ddlCardType.ClientID%>'
            $('#' + ddlCardType).change(function () {
                //validateCard();
            });

            $("#ContentPlaceHolder1_ddlCurrency").change(function () {
                CurrencyConvertion();
            });

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'

            $('#' + txtFromDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtToDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtToDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtFromDate).datepicker("option", "maxDate", selectedDate);
                }
            });

            //$("#ContentPlaceHolder1_txtToDate").blur(function () {
            //    var date = $("#ContentPlaceHolder1_txtToDate").val();
            //    if (date != "") {
            //        date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
            //        var isValid = CommonHelper.IsVaildDate(date);
            //        if (!isValid) {
            //            toastr.warning("Invalid Date");
            //            $("#ContentPlaceHolder1_txtToDate").focus();
            //            $("#ContentPlaceHolder1_txtToDate").val(DayOpenDate);
            //            return false;
            //        }
            //    }

            //});
            //$("#ContentPlaceHolder1_txtFromDate").blur(function () {
            //    var date = $("#ContentPlaceHolder1_txtFromDate").val();
            //    if (date != "") {
            //        date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
            //        var isValid = CommonHelper.IsVaildDate(date);
            //        if (!isValid) {
            //            toastr.warning("Invalid Date");
            //            $("#ContentPlaceHolder1_txtFromDate").focus();
            //            $("#ContentPlaceHolder1_txtFromDate").val(DayOpenDate);
            //            return false;
            //        }
            //    }
            //});
        });

        function CurrencyRateInfoEnable() {
            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            if (ddlCurrency == 0) {
                $('#ConversionPanel').hide()
            }
        }

        function CheckAllCheckBoxCreate() {
            $('.Chk_Select input[type=checkbox]').each(function () {
                $(this).prop("checked", true);
            });
        }

        function UnCheckAllCheckBoxCreate() {
            $('.Chk_Select input[type=checkbox]').each(function () {
                $(this).prop("checked", false);
            });
        }

        function PaymentModeShowHideInformation() {
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'

            if ($('#' + ddlPayMode).val() == "Cash") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#ChequeNChequeNumber').hide();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#ChequeNChequeNumber').hide();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Bank") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#BankPaymentAccountHeadDiv').show();
                $('#ChequeNChequeNumber').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#ChequeNChequeNumber').hide();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $("#<%=txtAdjustmentAmount.ClientID %>").val("");
                $("#<%=ddlAdjustmentNodeHead.ClientID %>").val("0");
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#AdjustmentDiv').show();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#ChequeNChequeNumber').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Loan") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').show();
                $('#ChequeNChequeNumber').hide();
                $('#AdjustmentDiv').hide();
            }
}

function PaymentModeNewShowHideInformation() {
    var ddlPayMode = '<%=ddlPaymentMode.ClientID%>'
            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'
            if ($('#' + ddlPayMode).val() == "0") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#ChequeNChequeNumber').hide();
                $('#CurrencyAndPaymentAmountDiv').hide();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cash") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#ChequeNChequeNumber').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#ChequeNChequeNumber').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Bank") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#BankPaymentAccountHeadDiv').show();
                $('#ChequeNChequeNumber').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#ChequeNChequeNumber').hide();
                $('#AdjustmentDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Adjustment") {
                $("#<%=txtAdjustmentAmount.ClientID %>").val("");
                $("#<%=ddlAdjustmentNodeHead.ClientID %>").val("0");
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#AdjustmentDiv').show();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').hide();
                $('#ChequeNChequeNumber').hide();
                $('#CurrencyAndPaymentAmountDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Loan") {
                $("#<%=txtChequeNumber.ClientID %>").val("");
                $("#<%=txtChequeDate.ClientID %>").val("");
                $('#CashPaymentAccountHeadDiv').hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CashAndCashEquivalantPaymentAccountHeadDiv').show();
                $('#ChequeNChequeNumber').hide();
                $('#AdjustmentDiv').hide();
            }
}
function PopulateProjects() {
    $("#<%=ddlGLProject.ClientID%>").attr("disabled", "disabled");
            if ($('#<%=ddlGLCompany.ClientID%>').val() == "0") {
                $('#<%=ddlGLProject.ClientID %>').empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }
            else {
                $('#<%=ddlGLProject.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                $.ajax({
                    type: "POST",
                    url: "/GeneralLedger/frmGLProject.aspx/PopulateProjects",
                    data: '{companyId: ' + $('#<%=ddlGLCompany.ClientID%>').val() + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnProjectsPopulated,
            failure: function (response) {
                alert(response.d);
            }
            });
    }
}

function OnProjectsPopulated(response) {
    var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            $("#" + ddlGLProject).attr("disabled", false);

            PopulateControl(response.d, $("#<%=ddlGLProject.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }
        //For FillForm-------------------------   
        function FIllForEdit(actionId) {
            $("#ContentPlaceHolder1_hfPaymentId").val("0");
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function FIllForEditWithConfirmation(paymentId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            FIllForEdit(paymentId);
        }

        function OnFillFormObjectSucceeded(result) {
            debugger;
            ReceiveInformation = new Array();

            ReceiveInformationDeleted = new Array();
            $("#ContentPlaceHolder1_ddlTransactionType").val(result.CompanyPayment.PaymentFor).trigger('change');
            $("#ContentPlaceHolder1_hfPaymentId").val(result.CompanyPayment.PaymentId);
            $("#ContentPlaceHolder1_hfCompanyBillId").val(result.CompanyPayment.CompanyBillId + "");
            $("#ContentPlaceHolder1_hfCmpSearch").val(result.CompanyPayment.CompanyId);

            LoadCompanyBill();

            $("#<%=hfCurrencyType.ClientID %>").val(result.CompanyPayment.CurrencyType);
            $("#<%=hfCompanyPaymentId.ClientID %>").val(result.CompanyPayment.CompanyPaymentId);
            $("#CompanyInfo").show();

            $("#txtCompanySearch").val(result.Company.CompanyName);
            $("#<%=txtEmailAddress.ClientID %>").val(result.Company.EmailAddress);
            $("#<%=txtWebAddress.ClientID %>").val(result.Company.WebAddress);
            $("#<%=txtContactPerson.ClientID %>").val(result.Company.ContactPerson);
            $("#<%=txtContactNumber.ClientID %>").val(result.Company.ContactNumber);
            $("#<%=txtTelephoneNumber.ClientID %>").val(result.Company.TelephoneNumber);
            $("#<%=txtAddress.ClientID %>").val(result.Company.CompanyAddress);

            $("#<%=ddlCurrency.ClientID %>").val(result.CompanyPayment.CurrencyId);
            $("#<%=txtLedgerAmount.ClientID %>").val(result.CompanyPayment.CurrencyAmount);
            $("#<%=txtPaymentDate2.ClientID %>").val(GetStringFromDateTime(result.CompanyPayment.PaymentDate));
            $("#<%=txtChequeNumber.ClientID %>").val(result.CompanyPayment.ChequeNumber);

            var dChequeDate = new Date(result.CompanyPayment.ChequeDate);
            var shortChequeDate = "";
            if (!result.CompanyPayment.ChequeDate) {
                shortChequeDate = "";
            }
            else {
                shortChequeDate = GetStringFromDateTime(dChequeDate);
            }

            $("#<%=txtChequeDate.ClientID %>").val(shortChequeDate);

            $("#<%=ddlBankPayment.ClientID %>").val(result.CompanyPayment.AccountsPostingHeadId);
            $('#txtBankPayment').val($("#<%=ddlBankPayment.ClientID %> option:selected").text());

            $("#<%=ddlCashPayment.ClientID %>").val(result.CompanyPayment.AccountsPostingHeadId);
            $('#txtCashPayment').val($("#<%=ddlCashPayment.ClientID %> option:selected").text());

            $("#<%=ddlCashAndCashEquivalantPayment.ClientID %>").val(result.AccountsPostingHeadId);
            $('#txtCashAndCashEquivalantPayment').val($("#<%=ddlCashAndCashEquivalantPayment.ClientID %> option:selected").text());

            $("#<%=txtRemarks.ClientID %>").val(result.CompanyPayment.Remarks);

            $("#ContentPlaceHolder1_txtAdjustmentAmount").val(result.CompanyPayment.PaymentAdjustmentAmount);
            $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val(result.CompanyPayment.AdjustmentAccountHeadId + '').trigger('change');

            if ($("#ContentPlaceHolder1_hfEditPermission").val() == "1")
                $("#ContentPlaceHolder1_btnSave").val("Update").show();
            else
                $("#ContentPlaceHolder1_btnSave").hide();
            $('#btnNewBIll').hide();
            $('#EntryPanel').show();

            if (result.CompanyPayment.PaymentType == "Cash") {
                $("#ContentPlaceHolder1_ddlCashPayment").val(result.CompanyPayment.AccountingPostingHeadId + "");
                $("#txtCashPayment").val($("#ContentPlaceHolder1_ddlCashPayment option:selected").text());
            }
            else if (result.CompanyPayment.PaymentType == "Bank") {
                $("#ContentPlaceHolder1_ddlBankPayment").val(result.CompanyPayment.AccountingPostingHeadId + "");
                $("#txtBankPayment").val($("#ContentPlaceHolder1_ddlBankPayment option:selected").text());
            }
            else if (result.CompanyPayment.PaymentType == "Cheque") {
                $("#ContentPlaceHolder1_ddlCompanyBank").val(result.CompanyPayment.AccountingPostingHeadId + "");
                $("#txtCompanyBank").val($("#ContentPlaceHolder1_ddlCompanyBank option:selected").text());
            }
            else if ($('#ContentPlaceHolder1_ddlPayMode').val() == "Adjustment") {

            }
            else if (result.CompanyPayment.PaymentType == "Loan") {
                $("#ContentPlaceHolder1_ddlCashAndCashEquivalantPayment").val(result.CompanyPayment.AccountingPostingHeadId + "");
                $("#txtCashAndCashEquivalantPayment").val($("#ContentPlaceHolder1_ddlCashAndCashEquivalantPayment option:selected").text());
            }

            $("#<%=ddlPayMode.ClientID %>").val(result.CompanyPayment.PaymentType)

            $("#ReceiveInformationTbl tbody").html("");
            //ClearBillContainer();

            var tr = "";
            for (var i = 0; i < result.CompanyPaymentTransactionDetails.length; i++) {
                tr += "<tr>";
                tr += "<td style='width:10%;'>" + result.CompanyPaymentTransactionDetails[i].PaymentMode + "</td>";
                tr += "<td style='width:15%;'>" + result.CompanyPaymentTransactionDetails[i].PaymentHeadName + "</td>";
                tr += "<td style='width:15%;'>" + result.CompanyPaymentTransactionDetails[i].PaymentAmount + "</td>";
                tr += "<td style='width:10%;'>" + CommonHelper.DateFromDateTimeToDisplay(result.CompanyPaymentTransactionDetails[i].PaymentDate, innBoarDateFormat) + "</td>";
                tr += "<td style='width:5%;'>" + result.CompanyPaymentTransactionDetails[i].CurrencyTypeName + "</td>";
                tr += "<td style='width:5%;'>" + result.CompanyPaymentTransactionDetails[i].ConversionRate + "</td>";
                tr += "<td style='width:10%;'>" + ((result.CompanyPaymentTransactionDetails[i].ChequeDate == null || result.CompanyPaymentTransactionDetails[i].ChequeDate == "") ? "" : CommonHelper.DateFromDateTimeToDisplay(result.CompanyPaymentTransactionDetails[i].ChequeDate, innBoarDateFormat)) + "</td>";
                tr += "<td style='width:10%;'>" + result.CompanyPaymentTransactionDetails[i].ChequeNumber + "</td>";
                tr += "<td style='width:15%;'>" + result.CompanyPaymentTransactionDetails[i].Totalamount + "</td>";

                tr += "<td style='width:5%;'>";
                tr += "<a href='javascript:void()' onclick= 'EditAdhoqItem(this)' ><img alt='Delete' src='../Images/edit.png' /></a>";
                tr += "&nbsp;<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";
                tr += "<td style='display:none;'>" + result.CompanyPaymentTransactionDetails[i].PaymentHeadId + "</td>";
                tr += "<td style='display:none;'>" + result.CompanyPaymentTransactionDetails[i].CurrencyTypeId + "</td>";
                tr += "<td style='display:none;'>" + result.CompanyPaymentTransactionDetails[i].PaymentDetailsId + "</td>";
                tr += "</tr>";

                CommonHelper.ApplyDecimalValidation();
                ReceiveInformation.push({
                    PaymentHeadId: parseInt(result.CompanyPaymentTransactionDetails[i].PaymentHeadId, 10),
                    CurrencyTypeId: parseInt(result.CompanyPaymentTransactionDetails[i].CurrencyTypeId, 10),
                    PaymentMode: result.CompanyPaymentTransactionDetails[i].PaymentMode,
                    PaymentHeadName: result.CompanyPaymentTransactionDetails[i].PaymentHeadName,
                    PaymentAmount: result.CompanyPaymentTransactionDetails[i].PaymentAmount,
                    PaymentDate: moment(result.CompanyPaymentTransactionDetails[i].PaymentDate).format("MM/DD/YYYY"),
                    CurrencyTypeName: result.CompanyPaymentTransactionDetails[i].CurrencyTypeName,
                    ConversionRate: result.CompanyPaymentTransactionDetails[i].ConversionRate,
                    ChequeDate: (result.CompanyPaymentTransactionDetails[i].ChequeDate == "" || result.CompanyPaymentTransactionDetails[i].ChequeDate == null) ? moment(new Date()).format("MM/DD/YYYY") : moment(result.CompanyPaymentTransactionDetails[i].ChequeDate).format("MM/DD/YYYY"),
                    ChequeNumber: result.CompanyPaymentTransactionDetails[i].ChequeNumber,
                    Totalamount: result.CompanyPaymentTransactionDetails[i].Totalamount,
                    DetailId: parseInt(result.CompanyPaymentTransactionDetails[i].PaymentDetailsId, 10),
                    Id: 0
                });
            }

            $("#ReceiveInformationTbl tbody").prepend(tr);
            CalculatePaymentReceivedTotal();

            $("#BillInfo tbody").html("");
            $("#ContentPlaceHolder1_txtAdvanceAmount").val(result.CompanyPayment.AdvanceAmount);

            if (result.CompanyPaymentDetails.length > 0) {

                var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
                var totalPaymentAmount = 0.00;

                for (row = 0; row < result.CompanyPaymentDetails.length; row++) {

                    isChecked = result.CompanyPaymentDetails[row].CompanyBillDetailsId > 0 ? "1" : "0";

                    if ((row + 1) % 2 == 0) {
                        tr += "<tr style='background-color:#FFFFFF;'>";
                    }
                    else {
                        tr += "<tr style='background-color:#E3EAEB;'>";
                    }

                    tr += "<td style='width: 7%'> ";

                    if (isChecked == "1") {
                        tr += "<input type='checkbox' id='pay" + result.CompanyPaymentDetails[row].CompanyPaymentId + "'" + chk + " onclick='CheckRow(this)' />";
                        totalPaymentAmount += result.CompanyPaymentDetails[row].DueAmount;
                    }
                    else {
                        tr += "<input type='checkbox' id='pay" + result.CompanyPaymentDetails[row].CompanyPaymentId + "' onclick='CheckRow(this)' />";
                    }

                    tr += "</td>";

                    tr += "<td style='width: 43%'>" + result.CompanyPaymentDetails[row].ModuleName + "</td>";
                    tr += "<td style='width: 10%'>" + GetStringFromDateTime(result.CompanyPaymentDetails[row].PaymentDate) + "</td>";
                    tr += "<td style='width: 10%'>" + result.CompanyPaymentDetails[row].BillNumber + "</td>";
                    tr += "<td style='width: 15%'>" + result.CompanyPaymentDetails[row].DueAmount + "</td>";

                    if (isChecked == "1") {
                        tr += "<td style='width: 15%'>  <input type='text' class='form-control quantitydecimal' id='p" + result.CompanyPaymentDetails[row].CompanyPaymentId + "' value='" + result.CompanyPaymentDetails[row].PaymentAmount + "' onblur='CheckPayment(this)' /></td>";
                    }
                    else {
                        tr += "<td style='width: 15%'>  <input type='text' disabled class='form-control quantitydecimal' id='p" + result[row].CompanyPaymentId + "' value='" + result.CompanyPaymentDetails[row].PaymentAmount + "' onblur='CheckPayment(this)' /></td>";
                    }

                    tr += "<td style='display:none;'>" + result.CompanyPaymentDetails[row].PaymentDetailsId + "</td>";
                    tr += "<td style='display:none;'>" + result.CompanyPaymentDetails[row].CompanyBillId + "</td>";
                    tr += "<td style='display:none;'>" + result.CompanyPaymentDetails[row].CompanyBillDetailsId + "</td>";
                    tr += "<td style='display:none;'>" + result.CompanyPaymentDetails[row].CompanyPaymentId + "</td>";
                    tr += "<td style='display:none;'>" + result.CompanyPaymentDetails[row].BillId + "</td>";

                    tr += "</tr>";

                    $("#BillInfo tbody").append(tr);
                    tr = "";
                }
            }
            CalculatePayment();
            ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());
            $("#myTabs").tabs({ active: 0 });

            return false;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {
            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            window.location = "frmReservationBillPayment.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            return false;
        }
        function EntryPanelVisibleFalse() {
            PerformClearAction();
            return false;
        }
        //Integrated Gl Visible True/False-------------------
        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }
        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show();
        }
        function MessagePanelHide() {
            $('#MessageBox').hide();
        }

        //Integrated Gl Visible True/False-------------------
        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }

        function ValidateForm() {
            if ($.trim($("#ContentPlaceHolder1_txtTotalAmount").val()) != $.trim($("#ContentPlaceHolder1_txtTotalReceiveAmount").val())) {
                toastr.warning("Max Company Payment and Total Receive Amount is not same.");
                $("#ContentPlaceHolder1_txtLedgerAmount").focus();
                return false;
            }

            if ($("#ContentPlaceHolder1_txtRemarks").val() == "") {
                toastr.warning("Please Provide Description.");
                $("#ContentPlaceHolder1_txtRemarks").focus();
                return false;
            }

            var totalPayment = 0.00, advanceAmount = 0.00, detailsId = 0, paymentId = 0, accountingPostingHeadId = 0;
            var companyBillId = 0.00, adjustmentAccountHeadId = 0, paymentAdjustmentAmount = 0.00;
            var chequeNumber = '', chequeDate;
            var CompanyPaymentDetails = new Array();
            var CompanyPaymentDetailsEdited = new Array();
            var CompanyPaymentDetailsDeleted = new Array();

            debugger;
            paymentId = $("#ContentPlaceHolder1_hfPaymentId").val();
            companyBillId = $("#ContentPlaceHolder1_ddlCompanyBill").val();
            adjustmentAccountHeadId = $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val();

            if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cash") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCashPayment").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Bank") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlBankPayment").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Card") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlBankPayment").val();
            }
            else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Loan") {
                accountingPostingHeadId = $("#ContentPlaceHolder1_ddlCashAndCashEquivalantPayment").val();
            }

            paymentAdjustmentAmount = $("#ContentPlaceHolder1_txtAdjustmentAmount").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtAdjustmentAmount").val()) : 0.00;
            advanceAmount = $("#ContentPlaceHolder1_txtAdvanceAmount").val() == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtAdvanceAmount").val());
            chequeNumber = $("#ContentPlaceHolder1_txtChequeNumber").val();

            if ($("#ContentPlaceHolder1_txtChequeDate").val() != "") {
                chequeDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtChequeDate").val(), '/');
            }

            var CompanyPayment = {
                PaymentId: paymentId,
                CompanyBillId: companyBillId,
                PaymentFor: $("#ContentPlaceHolder1_ddlTransactionType").val(),
                CompanyId: $("#ContentPlaceHolder1_hfCmpSearch").val(),
                PaymentDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtPaymentDate2").val(), '/'),
                AdvanceAmount: advanceAmount,
                PaymentType: $("#ContentPlaceHolder1_ddlPayMode").val(),
                AccountingPostingHeadId: 1,//accountingPostingHeadId,
                Remarks: $("#ContentPlaceHolder1_txtRemarks").val(),
                ChequeNumber: $("#ContentPlaceHolder1_txtChecqueNumber").val(),
                CurrencyId: $("#ContentPlaceHolder1_ddlCurrency").val(),
                ConvertionRate: $("#ContentPlaceHolder1_txtConversionRate").val() == "" ? "0" : $("#ContentPlaceHolder1_txtConversionRate").val(),
                AdjustmentAccountHeadId: adjustmentAccountHeadId,
                PaymentAdjustmentAmount: paymentAdjustmentAmount,
                ChequeNumber: chequeNumber,
                ChequeDate: chequeDate
            };

            $("#BillInfo tbody tr").each(function () {
                detailsId = parseInt($(this).find("td:eq(6)").text(), 10);

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId == 0) {
                    CompanyPaymentDetails.push({
                        PaymentDetailsId: 0,
                        CompanyBillDetailsId: $(this).find("td:eq(8)").text(),
                        CompanyPaymentId: $(this).find("td:eq(9)").text(),
                        BillId: $(this).find("td:eq(10)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").find("input").val()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId > 0) {
                    CompanyPaymentDetailsEdited.push({
                        PaymentDetailsId: detailsId,
                        CompanyBillDetailsId: $(this).find("td:eq(8)").text(),
                        CompanyPaymentId: $(this).find("td:eq(9)").text(),
                        BillId: $(this).find("td:eq(10)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").find("input").val()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == false && detailsId > 0) {
                    CompanyPaymentDetailsDeleted.push({
                        PaymentDetailsId: detailsId,
                        CompanyBillDetailsId: $(this).find("td:eq(8)").text(),
                        CompanyPaymentId: $(this).find("td:eq(9)").text(),
                        BillId: $(this).find("td:eq(10)").text(),
                        PaymentAmount: $(this).find("td:eq(5)").find("input").val()
                    });
                }
            });

            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();

            if ($("#ReceiveInformationTbl tbody tr").length == 0) {
                toastr.warning("Please add payment details.");
                return false;
            }

            CommonHelper.SpinnerOpen();
            PageMethods.SaveCompanyBillPayment(CompanyPayment, CompanyPaymentDetails, ReceiveInformation, ReceiveInformationDeleted, CompanyPaymentDetailsEdited, CompanyPaymentDetailsDeleted, parseInt(randomDocId), deletedDoc, OnSupplierPaymentSucceeded, OnSupplierPaymentFailed);

            return false;
        }
        function OnSupplierPaymentSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                $("#ContentPlaceHolder1_RandomDocId").val(result.Data);
                ReceiveInformation = new Array();

                ReceiveInformationDeleted = new Array();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
        }
        function OnSupplierPaymentFailed(result) {
            toastr.warning("You are entered wrong data, Please provide valid data.");
            CommonHelper.SpinnerClose();
        }

        function SearchPayment() {
            var gridRecordsCount = $("#BillInfoSearch tbody tr").length;
            var dateFrom = null, dateTo = null, companyId = 0;

            companyId = $("#ContentPlaceHolder1_ddlCompanyForSearch").val();

            if ($("#ContentPlaceHolder1_txtFromDate").val() != "") {
                dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/')
            }
            if ($("#ContentPlaceHolder1_txtToDate").val() != "") {
                dateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/')
            }

            $("#BillInfoSearch tbody").html("");
            var transactionType = $("#ContentPlaceHolder1_ddlSrcTransactionType").val();
            PageMethods.GetCompanyPaymentBySearch(transactionType, companyId, dateFrom, dateTo, OnSearchPaymentSucceeded, OnSearchPaymentFailed);

            return false;
        }

        function OnSearchPaymentSucceeded(result) {
            var tr = "";
            debugger;
            $.each(result, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.LedgerNumber + "</td>";
                if (gridObject.PaymentDate != null)
                    tr += "<td style='width:10%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.PaymentDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:10%;'></td>";

                tr += "<td style='width:10%;'>" + gridObject.PaymentFor + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.CompanyName + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:30%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:30%;'></td>";
                tr += "<td style='width:10%;'>" + (gridObject.ApprovedStatus == 'Pending' ? 'Submitted' : gridObject.ApprovedStatus) + "</td>";
                tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";
                if (gridObject.IsCanEdit && IsCanEdit) {
                    tr += "<a onclick=\"javascript:return FIllForEditWithConfirmation(" + gridObject.PaymentId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                }

                if (gridObject.IsCanDelete && IsCanDelete) {
                    tr += "&nbsp;<a href='javascript:void();' onclick= 'javascript:return DeleteCompanyPayment(" + gridObject.PaymentId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                }

                if (gridObject.IsCanChecked) {
                    tr += "&nbsp;<a href='javascript:void();' onclick= 'javascript:return CheckedPayment(" + gridObject.PaymentId + ")' ><img alt='Checked' src='../Images/checked.png' /></a>";
                }

                if (gridObject.IsCanApproved) {
                    tr += "&nbsp;<a href='javascript:void();' onclick= 'javascript:return ApprovedPayment(" + gridObject.PaymentId + ")' ><img alt='approved' src='../Images/approved.png' /></a>";
                }

                tr += "&nbsp;";

                tr += "<a href='javascript:void();' onclick= 'javascript:return ShowReport(" + gridObject.PaymentId + ")' ><img alt='approved' src='../Images/ReportDocument.png' /></a>";

                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.PaymentId + "</td>";

                tr += "</tr>";

                $("#BillInfoSearch tbody").append(tr);
                tr = "";
            });
        }


        function ShowReport(paymentId) {
            var iframeid = 'printDoc';
            var url = "Reports/frmCompanyPaymentReceipt.aspx?PId=" + paymentId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayReceipt").dialog({
                autoOpen: true,
                modal: true,
                width: 800,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Transaction Receipt",
                show: 'slide'
            })
        }

        function OnSearchPaymentFailed() { }
        function EmployeePaymentInvoice(paymentId) {
            if (paymentId != "") {
                var url = "/Payroll/Reports/frmReportEmployeePaymentInvoiceVoucher.aspx?PaymentIdList=" + paymentId;
                var popup_window = "Preview";
                window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            }
            else {
                toastr.warning('Select payments to preview');
            }
            return false;
        }

        function CheckedPayment(paymentId) {
            if (confirm("Do You Want to Check? ")) {
                PageMethods.CheckedPayment(paymentId, OnCheckedSucceed, OnCheckedFailed);
                return false;
            }
        }
        function OnCheckedSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchPayment();
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnCheckedFailed(error) {
            toastr.error(error.get_message());
        }

        function ApprovedPayment(paymentId) {
            if (confirm("Do You Want to Approve? ")) {
                PageMethods.ApprovedPayment(paymentId, OnApporavalSucceed, OnApporavalFailed);
                return false;
            }
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

        function DeleteCompanyPayment(paymentId) {
            if (confirm("Want to Delete record ? ")) {
                PageMethods.DeleteCompanyPayment(paymentId, OnReceiveDeleteSucceed, OnReceiveDeleteFailed);
                return false;
            }

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

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#ReceiveInformationTbl tbody").html("");
            CalculatePaymentReceivedTotal();
            ClearBillContainer();
            $("#BillInfo tbody").html("");
            $("#ContentPlaceHolder1_hfCompanyPaymentId").val("0");
            $("#ContentPlaceHolder1_hfCmpSearch").val("0");
            $("#<%=txtLedgerAmount.ClientID %>").val('');
            $("#<%=txtDealId.ClientID %>").val('');
            $("#<%=ddlCurrency.ClientID %>").val('1');
            $("#<%=txtCalculatedLedgerAmount.ClientID %>").val('');
            $("#<%=txtConversionRate.ClientID %>").val('');
            $("#<%=txtChequeNumber.ClientID %>").val("");
            $("#<%=txtChequeDate.ClientID %>").val("");
            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');
            $("#<%=txtExpireDate.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#ContentPlaceHolder1_txtAdvanceAmount").val("");
            $("#ContentPlaceHolder1_txtTotalReceiveAmount").val("");
            $("#txtCashPayment").val("");
            $("#ContentPlaceHolder1_ddlCashPayment").val("");
            $("#ContentPlaceHolder1_ddlCashAndCashEquivalantPayment").val("");
            $("#ContentPlaceHolder1_ddlCompanyBank").val("");
            $("#txtCompanyBank").val("");
            $("#txtBankPayment").val("");
            $("#ContentPlaceHolder1_txtChecqueNumber").val("");
            $("#txtSupplierName").val("");
            $("#ContentPlaceHolder1_txtEmailAddress").val("");
            $("#ContentPlaceHolder1_txtWebAddress").val("");
            $("#ContentPlaceHolder1_txtContactNumber").val("");
            $("#ContentPlaceHolder1_txtTelephoneNumber").val("");
            $("#ContentPlaceHolder1_txtAddress").val("");
            $("#ContentPlaceHolder1_txtContactPerson").val("");
            $("#txtCompanySearch").val("");
            $("#ContentPlaceHolder1_ddlCompanyBill").val("0");
            $("#ContentPlaceHolder1_txtPaymentDate2").val("");
            $("#DocumentInfo").html("");
            $("#ContentPlaceHolder1_txtTotalAmount").val("");
            $("#ContentPlaceHolder1_txtAdjustmentAmount").val("");
            $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val('0').trigger('change');
            $("#ContentPlaceHolder1_hfPaymentId").val("0");
            $("#ContentPlaceHolder1_hfCompanyBillId").val("0");
            $("#ContentPlaceHolder1_hfCmpSearch").val("0");

            $('#ConversionPanel').hide();
            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#ContentPlaceHolder1_btnSave").hide();
            else
                $("#ContentPlaceHolder1_btnSave").val("Save").show();

            $('#ContentPlaceHolder1_txtPaymentDate2').datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_ddlPayMode").val("0");
            $('#ContentPlaceHolder1_ddlCompanyBill').empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            $("#ContentPlaceHolder1_ddlTransactionType").val('Receive').trigger('change');
            return false;
        }

        function ValidateExpireDate() {
            var isValid = true;
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            var txtExpireDate = '<%=txtExpireDate.ClientID%>'
            if ($('#' + ddlPayMode).val() == "Card") {
                if ($('#' + txtExpireDate).val() == "") {
                    isValid = false;
                }
            }
            return isValid;
        }

        function validateCard() {
            var txtCardNumber = '<%=txtCardNumber.ClientID%>'
            var ddlCardType = '<%=ddlCardType.ClientID%>'
            var cardNumber = $('#' + txtCardNumber).val();
            var cardType = $('#' + ddlCardType).val();
            var isTrue = true;
            var messege = "";

            var txtCardValidation = '<%=txtCardValidation.ClientID%>'
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() != "Card") {
                return true;
            }

            if ($('#' + txtCardValidation).val() == 0) {
                return true;
            }

            if (!cardType) {
                isTrue = false;
                messege = "Card number must not be empty.";
            }

            if (cardNumber.length == 0) {						//most of these checks are self explanitory

                //alert("Please enter a valid card number.");
                isTrue = false;
                messege = "Please enter a valid card number."

            }
            for (var i = 0; i < cardNumber.length; ++i) {		// make sure the number is all digits.. (by design)
                var c = cardNumber.charAt(i);


                if (c < '0' || c > '9') {

                    isTrue = false;
                    messege = "Please enter a valid card number. Use only digits. do not use spaces or hyphens.";
                }
            }
            var length = cardNumber.length; 		//perform card specific length and prefix tests

            switch (cardType) {
                case 'a':
                    if (length != 15) {

                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid American Express Card number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 2));

                    if (prefix != 34 && prefix != 37) {
                        isTrue = false;
                        messege = "Please enter a valid American Express Card number.";
                    }
                    break;
                case 'd':

                    if (length != 16) {

                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid Discover Card number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 4));

                    if (prefix != 6011) {

                        //alert("Please enter a valid Discover Card number.");
                        isTrue = false;
                        messege = "Please enter a valid Discover Card number.";
                    }
                    break;
                case 'm':

                    if (length != 16) {
                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid MasterCard number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 2));

                    if (prefix < 51 || prefix > 55) {

                        isTrue = false;
                        messege = "Please enter a valid MasterCard number.";
                    }
                    break;
                case 'v':

                    if (length != 16 && length != 13) {

                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid Visa Card number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 1));

                    if (prefix != 4) {
                        isTrue = false;
                        messege = "Please enter a valid Visa Card number.";
                    }
                    break;
            }
            if (!mod10(cardNumber)) {
                //alert("");
                isTrue = false;
                messege = "Sorry! this is not a valid credit card number.";
            }

            if (isTrue == false) {
                MessagePanelShow();
                var lblMessage = '<%=lblMessage.ClientID%>'
                $('#' + lblMessage).text(messege);
                alert(messege);
                return false;
            }
            else {
                MessagePanelHide();
                return true;
            }
        }

        function mod10(cardNumber) { // LUHN Formula for validation of credit card numbers.
            var ar = new Array(cardNumber.length);
            var i = 0, sum = 0;

            for (i = 0; i < cardNumber.length; ++i) {
                ar[i] = parseInt(cardNumber.charAt(i));
            }
            for (i = ar.length - 2; i >= 0; i -= 2) { // you have to start from the right, and work back.
                ar[i] *= 2; 						 // every second digit starting with the right most (check digit)
                if (ar[i] > 9) ar[i] -= 9; 		 // will be doubled, and summed with the skipped digits.
            } 									 // if the double digit is > 9, ADD those individual digits together 


            for (i = 0; i < ar.length; ++i) {
                sum += ar[i]; 					 // if the sum is divisible by 10 mod10 succeeds
            }
            return (((sum % 10) == 0) ? true : false);
        }

        function LoadPopUp() {
            //popup(1, 'DivPaymentSelect', '', 600, 500);
            $("#DivPaymentSelect").dialog({
                width: 600,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", //TODO add title
                show: 'slide'
            });
            return false;
        }

        function LoadCompanyInfo(companyId) {
            PageMethods.LoadCompanyInfo(companyId, OnLoadCompanySucceeded, OnLoadCompanyFailed);
            return false;
        }
        function OnLoadCompanySucceeded(result) {
            $("#<%=txtEmailAddress.ClientID %>").val(result.EmailAddress);
            $("#<%=txtWebAddress.ClientID %>").val(result.WebAddress);
            $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson);
            $("#<%=txtContactNumber.ClientID %>").val(result.ContactNumber);
            $("#<%=txtTelephoneNumber.ClientID %>").val(result.TelephoneNumber);
            $("#<%=txtAddress.ClientID %>").val(result.CompanyAddress);
            $("#<%=hfCmpSearch.ClientID %>").val(result.CompanyId);
        }
        function OnLoadCompanyFailed(error) {
            toastr.error(error.get_message());
        }

        function PaymentPreview() {
            var paymentIdList = "";
            $("#ContentPlaceHolder1_gvPaymentInfo tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                    var id = $.trim($(this).find("td:eq(1)").text());
                    if (paymentIdList == "") {
                        paymentIdList = id;
                    }
                    else {
                        paymentIdList += ',' + id;
                    }
                }
            });
            if (paymentIdList != "") {
                var url = "/HotelManagement/Reports/frmReportCompanyPayment.aspx?PaymentIdList=" + paymentIdList;
                var popup_window = "Preview";
                window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            }
            else {
                toastr.warning('Select payments to preview');
            }
        }

        //------------ Bill Payment

        function CheckRow(control) {
            var tr = $(control).parent().parent();

            if ($(control).is(":checked")) {
                $(tr).find("td:eq(5)").find("input").prop("disabled", false);
            }
            else {
                $(tr).find("td:eq(5)").find("input").prop("disabled", true);
            }

            CalculatePayment();
        }

        function SearchCompanyBill() {
            CommonHelper.SpinnerOpen();
            var companyId = $("#ContentPlaceHolder1_hfCmpSearch").val();
            var companyBillId = $("#ContentPlaceHolder1_ddlCompanyBill").val();

            $("#BillInfo tbody").html("");
            PageMethods.CompanyBillBySearch(companyBillId, companyId, OnLoadCompanyBillSucceeded, OnCompanyBillFailed);
        }

        function OnLoadCompanyBillSucceeded(result) {
            var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
            var totalPaymentAmount = 0.00;

            for (row = 0; row < result.length; row++) {

                isChecked = result[row].CompanyBillId > 0 ? "1" : "0";

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 7%'> ";

                if (isChecked == "1") {
                    tr += "<input type='checkbox' id='pay" + result[row].CompanyPaymentId + "'" + chk + " onclick='CheckRow(this)' />";
                    totalPaymentAmount += result[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result[row].CompanyPaymentId + "' onclick='CheckRow(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 43%'>" + result[row].ModuleName + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 10%'>" + result[row].BillNumber + "</td>";
                tr += "<td style='width: 15%'>" + result[row].DueAmount + "</td>";

                if (isChecked == "1") {
                    tr += "<td style='width: 15%'>  <input type='text' class='form-control quantitydecimal' id='p" + result[row].CompanyPaymentId + "' value='" + result[row].DueAmount + "' onblur='CheckPayment(this)' /></td>";
                }
                else {
                    tr += "<td style='width: 15%'>  <input type='text' disabled class='form-control quantitydecimal' id='p" + result[row].CompanyPaymentId + "' value='" + result[row].DueAmount + "' onblur='CheckPayment(this)' /></td>";
                }

                tr += "<td style='display:none;'>" + result[row].PaymentDetailsId + "</td>";
                tr += "<td style='display:none;'>" + result[row].CompanyBillId + "</td>";
                tr += "<td style='display:none;'>" + result[row].CompanyBillDetailsId + "</td>";
                tr += "<td style='display:none;'>" + result[row].CompanyPaymentId + "</td>";
                tr += "<td style='display:none;'>" + result[row].BillId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            $("#ContentPlaceHolder1_txtTotalAmount").val(totalPaymentAmount);
            $("#ContentPlaceHolder1_txtLedgerAmount").val(totalPaymentAmount);
            $("#ContentPlaceHolder1_txtCalculatedLedgerAmount").val(totalPaymentAmount);

            CurrencyConvertion();

            CommonHelper.ApplyDecimalValidation();

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnCompanyBillFailed(error) {
            toastr.warning("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function CalculatePayment() {

            var totalPayment = 0.00;

            $("#BillInfo tbody tr").each(function () {

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                    totalPayment += parseFloat($(this).find("td:eq(5)").find("input").val());
                }
            });

            var paymentAdjustmentAmount = $("#ContentPlaceHolder1_txtAdjustmentAmount").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtAdjustmentAmount").val()) : 0.00;
            var advanceAmount = $("#ContentPlaceHolder1_txtAdvanceAmount").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtAdvanceAmount").val()) : 0.00;

            $("#ContentPlaceHolder1_txtTotalAmount").val(toFixed((totalPayment + advanceAmount), 2));
            $("#ContentPlaceHolder1_txtLedgerAmount").val(toFixed(((totalPayment + advanceAmount) - paymentAdjustmentAmount), 2));
            $("#ContentPlaceHolder1_txtCalculatedLedgerAmount").val(toFixed(((totalPayment + advanceAmount) - paymentAdjustmentAmount), 2));


        }

        function CheckPayment(control) {

            var tr = $(control).parent().parent();
            var value = parseFloat($(control).val());
            var bdValue = parseFloat($(tr).find("td:eq(4)").text());

            if (value > bdValue) {
                $(control).val(bdValue);
                toastr.warning("Payment Amount Cannot Greater Than Due Amount.");
            }
            else if ($.trim(value) == "" || $.trim(value) == "0") {
                $(control).val(bdValue);
                toastr.warning("Payment Amount Cannot Zero(0) Or Empty.");
            }
            CalculatePayment();
        }

        function CheckAdvance(control) {
            CalculatePayment();
        }

        function LoadCompanyBill() {
            var companyId = $("#ContentPlaceHolder1_hfCmpSearch").val();
            PageMethods.GetCompanyGeneratedBillByBillStatus(companyId, OnLoadLoadCompanyBillSucceeded, OnLoadLoadCompanyBillFailed);
        }
        function OnLoadLoadCompanyBillSucceeded(result) {
            CompanyGeneratedBill = result;
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlCompanyBill');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].CompanyBillNumber + '" value="' + list[i].CompanyBillId + '">' + list[i].CompanyBillNumber + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            if ($("#ContentPlaceHolder1_hfCompanyBillId").val() != 0) {
                control.val($("#ContentPlaceHolder1_hfCompanyBillId").val()).trigger("change");
                $("#ContentPlaceHolder1_ddlCurrency").trigger("change");
            }

            return false;
        }
        function OnLoadLoadCompanyBillFailed() { }

        function CurrencyConvertion() {
            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currencyType == "Local") {
                var LedgerAmount = parseFloat($('#ContentPlaceHolder1_txtLedgerAmount').val());

                $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').val(LedgerAmount.toFixed(2));
            }
            else {
                var LedgerAmount = parseFloat($('#ContentPlaceHolder1_txtLedgerAmount').val()) * parseFloat($('#ContentPlaceHolder1_txtConversionRate').val() == "" ? 0 : $('#ContentPlaceHolder1_txtConversionRate').val());
                if (isNaN(LedgerAmount.toString())) {
                    LedgerAmount = 0;
                }
                $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').val(LedgerAmount.toFixed(2));
                $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').val(LedgerAmount.toFixed(2));
                $('#ContentPlaceHolder1_txtCalculatedLedgerAmount').attr("disabled", true);
            }
        }
        //Document Related Functions 
        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/HotelManagement/Image/";
            var category = "CompanyBillReceive";
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
            var id = $("#ContentPlaceHolder1_hfPaymentId").val();
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
        function ShowDocumentByID(id, ledgerNumber) {
            PageMethods.GetUploadedDocByWebMethod(0, id, "", OnGetUploadedDocByWebMethodSucceededForShow, OnLoadDocumentFailed);

            //PageMethods.LoadTaskDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            $("#taskDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: "70%",
                height: 300,
                closeOnEscape: true,
                resizable: false,
                title: "Documents ( " + ledgerNumber + " )",
                show: 'slide'
            });
            return false;
        }
        //function OnLoadDocumentSucceeded(result) {
        //    $("#imageDiv").html(result);

        //    $("#taskDocuments").dialog({
        //        autoOpen: true,
        //        modal: true,
        //        width: "70%",
        //        height: 300,
        //        closeOnEscape: true,
        //        resizable: false,
        //        title: "Task Assign Documents",
        //        show: 'slide'
        //    });

        //    return false;
        //}

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function OnGetUploadedDocByWebMethodSucceededForShow(result) {
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            DocTable = "";

            DocTable += "<table id='DocTableListShow' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformationq'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> </tr>";

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

                //DocTable += "<td align='left' style='width: 20%'>";
                ////DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                //DocTable += "</td>";
                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#imageDiv").html(DocTable);



            //$("#DocumentInfo").html(DocTable);

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

        function CalculatePaymentReceivedTotal() {
            debugger;
            var grandTotal = 0;
            $("#ReceiveInformationTbl tbody tr").each(function () {
                grandTotal += parseFloat($(this).find("td:eq(8)").text());
            });

            //if (grandTotal == 0) {
            //    $("#tableFoot").hide();
            //}
            //else {
            //    $("#tableFoot").show();
            //}
            $("#ContentPlaceHolder1_txtTotalReceiveAmount").val(grandTotal);
        }

        function AddItem() {

            var paymentMode = $("#ContentPlaceHolder1_ddlPaymentMode").val();
            if (paymentMode == "") {
                toastr.warning("Add Payment Mode.");
                $("#ContentPlaceHolder1_ddlPaymentMode").focus();
                return false;
            }

            var paymentDate = $("#ContentPlaceHolder1_txtPaymentDate2").val();
            if (paymentDate == "") {
                toastr.warning("Add Payment Date.");
                $("#ContentPlaceHolder1_txtPaymentDate2").focus();
                return false;
            }

            var bankPayment, chequeNumber = "", chequeDate = "", adjustmentNodeHead, paymentAmount, paymentHeadId, paymentHeadName;

            if (paymentMode == "0") {
                $("#ddlPaymentMode").focus();
                toastr.warning("Please Select Payment Mode.");
                return false;
            }
            else if (paymentMode == "Bank") {
                if ($("#ContentPlaceHolder1_ddlBankPayment").val() == "" || $("#ContentPlaceHolder1_ddlBankPayment").val() == "0" || $("#txtBankPayment").val() == "") {
                    $("#txtBankPayment").focus();
                    toastr.warning("Please Select Bank Name.");
                    return false;
                }

                paymentHeadId = $("#ContentPlaceHolder1_ddlBankPayment option:selected").val();
                paymentHeadName = $("#ContentPlaceHolder1_ddlBankPayment option:selected").text();

                chequeNumber = $("#ContentPlaceHolder1_txtChequeNumber").val();
                if (chequeNumber == "") {
                    toastr.warning("Add Cheque Number.");
                    $("#ContentPlaceHolder1_txtChequeNumber").focus();
                    return false;
                }
                chequeDate = $("#ContentPlaceHolder1_txtChequeDate").val();
                if (chequeDate == "") {
                    toastr.warning("Add Cheque Date.");
                    $("#ContentPlaceHolder1_txtChequeDate").focus();
                    return false;
                }
            }
            else if (paymentMode == "Cash") {
                if ($("#ContentPlaceHolder1_ddlCashPayment").val() == "" || $("#ContentPlaceHolder1_ddlCashPayment").val() == "0" || $("#txtCashPayment").val() == "") {
                    $("#txtCashPayment").focus();
                    toastr.warning("Please Select Cash Head.");
                    return false;
                }

                paymentHeadId = $("#ContentPlaceHolder1_ddlCashPayment option:selected").val();
                paymentHeadName = $("#ContentPlaceHolder1_ddlCashPayment option:selected").text();
            }
            else if (paymentMode == "Adjustment") {
                adjustmentNodeHead = $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val();
                if (adjustmentNodeHead == "0") {
                    toastr.warning("Add Adjustment Head.");
                    $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").focus();
                    return false;
                }

                paymentHeadId = $("#ContentPlaceHolder1_ddlAdjustmentNodeHead option:selected").val();
                paymentHeadName = $("#ContentPlaceHolder1_ddlAdjustmentNodeHead option:selected").text();
            }

            var CostCenterOBJ = _.findWhere(ReceiveInformation, { PaymentHeadId: parseInt(paymentHeadId, 10), PaymentMode: paymentMode });
            if (ItemEdited != "" && CostCenterOBJ != null) {
                if (ReceiveInformation[indexEdited].PaymentHeadId != CostCenterOBJ.PaymentHeadId && ReceiveInformation[indexEdited].PaymentMode != CostCenterOBJ.PaymentMode) {
                    toastr.warning("It's already added.");
                    return false;
                }

            }
            else if (CostCenterOBJ != null) {
                toastr.warning("It's already added.");
                return false;
            }

            paymentAmount = $("#ContentPlaceHolder1_txtLedgerAmount").val();
            if (paymentAmount == "") {
                toastr.warning("Add Payment Amount.");
                $("#ContentPlaceHolder1_txtLedgerAmount").focus();
                return false;
            }
            var totalamount = $("#ContentPlaceHolder1_txtCalculatedLedgerAmount").val();
            var MaximumAmountForTransaction = parseFloat($("#ContentPlaceHolder1_txtTotalAmount").val() == "" ? 0.0 : $("#ContentPlaceHolder1_txtTotalAmount").val());
            //ContentPlaceHolder1_txtCalculatedLedgerAmount
            var currencyTypeId = $("#ContentPlaceHolder1_ddlCurrency option:selected").val();
            var currencyTypeName = $("#ContentPlaceHolder1_ddlCurrency option:selected").text();
            var conversionRate = $("#ContentPlaceHolder1_txtConversionRate").val() == "" ? 1 : $("#ContentPlaceHolder1_txtConversionRate").val();

            debugger;

            var totalAllAmount = 0.0;
            var ReceiveInformationNewlyAdded = new Array();
            ReceiveInformationNewlyAdded = ReceiveInformation;
            var row = 0, rowCount = ReceiveInformationNewlyAdded.length;
            for (row = 0; row < rowCount; row++) {

                totalAllAmount += parseFloat(ReceiveInformationNewlyAdded[row].Totalamount);
            }
            if (ItemEdited != "") {
                totalAllAmount -= parseFloat(ReceiveInformation[indexEdited].Totalamount);
            }
            if ((totalAllAmount + parseFloat(totalamount)) > MaximumAmountForTransaction) {
                toastr.warning("Total payment amount can not be greater than company bill.");
                $("#ContentPlaceHolder1_txtLedgerAmount").focus();
                return false;
            }

            if (ItemEdited == "" && $("#btnAdd").val() != "Update") {
                var tr = "";
                tr += "<tr>";

                tr += "<td style='width:10%;'>" + paymentMode + "</td>";
                tr += "<td style='width:15%;'>" + paymentHeadName + "</td>";
                tr += "<td style='width:15%;'>" + paymentAmount + "</td>";
                tr += "<td style='width:10%;'>" + paymentDate + "</td>";
                tr += "<td style='width:5%;'>" + currencyTypeName + "</td>";
                tr += "<td style='width:5%;'>" + conversionRate + "</td>";
                tr += "<td style='width:10%;'>" + chequeDate + "</td>";
                tr += "<td style='width:10%;'>" + chequeNumber + "</td>";
                tr += "<td style='width:15%;'>" + totalamount + "</td>";


                tr += "<td style='width:5%;'>";
                tr += "<a href='javascript:void()' onclick= 'EditAdhoqItem(this)' ><img alt='Delete' src='../Images/edit.png' /></a>";
                tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";
                tr += "<td style='display:none;'>" + paymentHeadId + "</td>";
                tr += "<td style='display:none;'>" + currencyTypeId + "</td>";
                tr += "<td style='display:none;'>" + 0 + "</td>";

                tr += "</tr>";

                $("#ReceiveInformationTbl tbody").prepend(tr);

                CalculatePaymentReceivedTotal();

                tr = "";
                CommonHelper.ApplyDecimalValidation();
                ReceiveInformation.push({
                    PaymentHeadId: parseInt(paymentHeadId, 10),
                    CurrencyTypeId: parseInt(currencyTypeId, 10),
                    PaymentMode: paymentMode,
                    PaymentHeadName: paymentHeadName,
                    PaymentAmount: paymentAmount,
                    PaymentDate: CommonHelper.DateFormatToMMDDYYYY(paymentDate, '/'),
                    CurrencyTypeName: currencyTypeName,
                    ConversionRate: conversionRate,
                    ChequeDate: chequeDate == "" ? CommonHelper.DateFormatToMMDDYYYY(DayOpenDate, '/') : CommonHelper.DateFormatToMMDDYYYY(chequeDate, '/'),
                    ChequeNumber: chequeNumber,
                    Totalamount: totalamount,
                    DetailId: 0,
                    Id: 0
                });
            }
            else {

                $(ItemEdited).find("td:eq(0)").text(paymentMode);
                $(ItemEdited).find("td:eq(1)").text(paymentHeadName);
                $(ItemEdited).find("td:eq(2)").text(paymentAmount);
                $(ItemEdited).find("td:eq(3)").text(paymentDate);
                $(ItemEdited).find("td:eq(4)").text(currencyTypeName);
                $(ItemEdited).find("td:eq(5)").text(conversionRate);
                $(ItemEdited).find("td:eq(6)").text(chequeDate);
                $(ItemEdited).find("td:eq(7)").text(chequeNumber);
                $(ItemEdited).find("td:eq(8)").text(totalamount);
                $(ItemEdited).find("td:eq(10)").text(paymentHeadId);
                $(ItemEdited).find("td:eq(11)").text(currencyTypeId);


                ReceiveInformation[indexEdited].PaymentHeadId = parseInt(paymentHeadId, 10);
                ReceiveInformation[indexEdited].CurrencyTypeId = parseInt(currencyTypeId, 10);
                ReceiveInformation[indexEdited].PaymentMode = (paymentMode);
                ReceiveInformation[indexEdited].PaymentHeadName = paymentHeadName;
                ReceiveInformation[indexEdited].PaymentAmount = paymentAmount;
                ReceiveInformation[indexEdited].PaymentDate = CommonHelper.DateFormatToMMDDYYYY(paymentDate, '/');
                ReceiveInformation[indexEdited].CurrencyTypeName = (currencyTypeName);
                ReceiveInformation[indexEdited].ConversionRate = conversionRate;
                ReceiveInformation[indexEdited].ChequeDate = (chequeDate == "" ? CommonHelper.DateFormatToMMDDYYYY(DayOpenDate, '/') : CommonHelper.DateFormatToMMDDYYYY(chequeDate, '/'));
                ReceiveInformation[indexEdited].ChequeNumber = (chequeNumber);
                ReceiveInformation[indexEdited].Totalamount = (totalamount);

            }
            CalculatePaymentReceivedTotal();
            ClearBillContainer();
            $("#ContentPlaceHolder1_ddlPaymentMode").focus();
        }

        function EditAdhoqItem(control) {

            if (!confirm("Do you want to Edit?")) { return false; }

            debugger;

            var tr = $(control).parent().parent();
            ItemEdited = $(control).parent().parent();

            var costCenter = parseInt($.trim($(tr).find("td:eq(10)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(12)").text()), 10);
            var paymentMode = ($.trim($(tr).find("td:eq(0)").text()));
            debugger;
            var CostCenter = _.findWhere(ReceiveInformation, { PaymentHeadId: costCenter, PaymentMode: paymentMode });
            indexEdited = _.indexOf(ReceiveInformation, CostCenter);

            $("#ContentPlaceHolder1_ddlPaymentMode").val(CostCenter.PaymentMode).trigger('change');
            $("#ContentPlaceHolder1_txtPaymentDate2").val(CommonHelper.DateFromDateTimeToDisplay(CostCenter.PaymentDate, innBoarDateFormat));
            $("#ContentPlaceHolder1_txtLedgerAmount").val(CostCenter.PaymentAmount);
            $("#ContentPlaceHolder1_txtCalculatedLedgerAmount").val(CostCenter.Totalamount);
            $("#ContentPlaceHolder1_ddlCurrency").val(CostCenter.CurrencyTypeId).trigger('change');

            if (CostCenter.PaymentMode == "Bank") {

                //$("#txtBankPayment").val(CostCenter.paymentHeadName);
                //$("#ContentPlaceHolder1_ddlBankPayment").val(CostCenter.PaymentHeadId+"").trigger('change');

                $("#<%=ddlBankPayment.ClientID %>").val(CostCenter.PaymentHeadId).trigger('change');
                $('#txtBankPayment').val($("#<%=ddlBankPayment.ClientID %> option:selected").text());

                $("#ContentPlaceHolder1_txtChequeNumber").val(CostCenter.ChequeNumber);
                //$("#ContentPlaceHolder1_txtChequeDate").val(CostCenter.ChequeDate);

                $("#ContentPlaceHolder1_txtChequeDate").val(CommonHelper.DateFromDateTimeToDisplay(CostCenter.ChequeDate, innBoarDateFormat));


            }
            else if (CostCenter.PaymentMode == "Cash") {
                //$("#txtCashPayment").val(CostCenter.paymentHeadName);
                //$("#ContentPlaceHolder1_ddlCashPayment").val(CostCenter.PaymentHeadId + "").trigger('change');


                $("#<%=ddlCashPayment.ClientID %>").val(CostCenter.PaymentHeadId).trigger('change');
                $('#txtCashPayment').val($("#<%=ddlCashPayment.ClientID %> option:selected").text());

            }
            else if (CostCenter.PaymentMode == "Adjustment") {

                $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val(CostCenter.PaymentHeadId).trigger('change');

            }

        $("#btnAdd").val("Update");

        CalculatePaymentReceivedTotal();
    }

    function DeleteAdhoqItem(control) {
        if (!confirm("Do you want to delete?")) { return false; }
        var tr = $(control).parent().parent();

        var costCenter = parseInt($.trim($(tr).find("td:eq(10)").text()), 10);
        var detailsId = parseInt($.trim($(tr).find("td:eq(12)").text()), 10);

        var paymentMode = ($.trim($(tr).find("td:eq(0)").text()));
        debugger;
        var CostCenter = _.findWhere(ReceiveInformation, { PaymentHeadId: costCenter, PaymentMode: paymentMode });
        var index = _.indexOf(ReceiveInformation, CostCenter);

        if (parseInt(detailsId, 10) > 0)
            ReceiveInformationDeleted.push(JSON.parse(JSON.stringify(CostCenter)));

        ReceiveInformation.splice(index, 1);
        $(tr).remove();
        CalculatePaymentReceivedTotal();
    }

    function ClearBillContainer() {
        //$("#ContentPlaceHolder1_txtAdvanceAmount").val("");
        //$("#ContentPlaceHolder1_txtTotalAmount").val("");
        $("#ContentPlaceHolder1_ddlPaymentMode").val("0").change();
        $("#txtBankPayment").val("");
        $("#ContentPlaceHolder1_ddlBankPayment").val("");
        $("#txtCashPayment").val("");
        $("#ContentPlaceHolder1_ddlCashPayment").val("");
        $("#ContentPlaceHolder1_txtLedgerAmount").val("");
        $("#ContentPlaceHolder1_txtChequeNumber").val("");
        $("#ContentPlaceHolder1_txtChequeDate").val(DayOpenDate);
        $("#ContentPlaceHolder1_txtPaymentDate2").val(DayOpenDate);
        $("#btnAdd").val("Add");

        $("#ContentPlaceHolder1_ddlCurrency").val("1").change();
        $("#ContentPlaceHolder1_ddlAdjustmentNodeHead").val("0").change();

        ItemEdited = ""; indexEdited = -1;
    }

    </script>

    <style>
        .ChkAllSelect {
            padding-left: 20px;
        }

        .lblHeader {
        }

        .ShowHide {
            display: none;
        }
    </style>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfPaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyPaymentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyBill" runat="server" />
    <asp:HiddenField ID="hfCompanyBillId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <asp:HiddenField ID="hfIsUpdatePermission" runat="server" />
    <asp:HiddenField ID="hfIsDeletePermission" runat="server" />
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
        <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    </div>
    <asp:HiddenField ID="hfCmpSearch" runat="server" />
    <div id="taskDocuments" style="height: 250px; overflow-y: scroll; display: none;">
        <div id="imageDiv"></div>
    </div>
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="DivPaymentSelect" style="display: none;">
        <div id="Div1" class="panel panel-default">
            <div class="panel-body">
                <asp:HiddenField ID="hfCurrencyType" runat="server" />
                <asp:HiddenField ID="hfConversionRate" runat="server" />
                <asp:HiddenField ID="txtSelectedRoomNumbers" runat="server" />
                <asp:HiddenField ID="txtSelectedRoomId" runat="server" />
                <asp:HiddenField ID="HiddenFieldCompanyPaymentButtonInfo" runat="server"></asp:HiddenField>
                <asp:GridView ID="gvPaymentInfo" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="100"
                    CssClass="table table-bordered table-condensed table-responsive">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="05%">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ChkAllSelect" CssClass="ChkAllSelect" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ChkSelect" CssClass="Chk_Select" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IDNO" ItemStyle-CssClass="ShowHide" HeaderStyle-CssClass="ShowHide">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("CompanyPaymentId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment Date" ItemStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label ID="lblPLI_DATE" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("PaymentDate"))) %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" ItemStyle-Width="20%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CurrencyAmount" HeaderText="Payment Amount" ItemStyle-Width="20%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
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
                <asp:Button ID="btnPaymentPreview" runat="server" Text="Preview" TabIndex="3" CssClass="TransactionalButton btn btn-primary"
                    OnClientClick="javascript: return PaymentPreview();" />
            </div>
        </div>
    </div>
    <div id="btnNewBIll" class="btn-toolbar" style="display: none;">
        <button onclick="javascript: return EntryPanelVisibleTrue();" class="btn btn-primary">
            <i class="icon-plus"></i>New Payment</button>
        <div class="btn-group">
        </div>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Transaction Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Transaction</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Company Transaction
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:HiddenField ID="HiddenField2" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="HiddenField3" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblPaymentDate" runat="server" class="control-label" Text="Payment Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
                                <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" CssClass="childDivSectionDivThreeColumnDropDownList">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label17" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <div>
                                    <asp:DropDownList ID="ddlTransactionType" TabIndex="1" CssClass="form-control" runat="server">
                                        <asp:ListItem>Receive</asp:ListItem>
                                        <asp:ListItem>Payment</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcRoomNumber" runat="server" class="control-label required-field"
                                    Text="Company Name"></asp:Label>
                                <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtPaymentId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
                            </div>
                            <div class="col-md-10">
                                <input id="txtCompanySearch" type="text" class="form-control" name="cmpName" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlCompany" TabIndex="1" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-2" style="width: 20%; display: none;">
                                <asp:Button ID="btnSrcCmpPayment" runat="server" Text="Search Payment" TabIndex="2"
                                    CssClass="TransactionalButton btn btn-primary" OnClick="btnSrcCmpPayment_Click" />
                            </div>
                        </div>
                        <div id="CompanyInfo" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblEmailAddress" runat="server" class="control-label" Text="Company Email"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" TabIndex="3"
                                        disabled></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblWebAddress" runat="server" class="control-label" Text="Web Address"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtWebAddress" runat="server" CssClass="form-control" TabIndex="4"
                                        disabled></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblContactNumber" runat="server" class="control-label" Text="Contact Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContactNumber" runat="server" TabIndex="6" CssClass="form-control"
                                        disabled></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblTelephoneNumber" runat="server" class="control-label" Text="Telephone Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTelephoneNumber" runat="server" TabIndex="7" CssClass="form-control"
                                        disabled></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblContactPerson" runat="server" class="control-label" Text="Contact Person"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" TabIndex="5"
                                        disabled></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblAddress" runat="server" class="control-label" Text="Address"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                                        TabIndex="2" disabled></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="CompanyBillDiv" runat="server">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Company Bill"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCompanyBill" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="BillInfoDiv" runat="server">
                            <div class="col-md-12">
                                <table id="BillInfo" class="table table-bordered table-condensed table-hover table-responsive">
                                    <thead>
                                        <tr>
                                            <th style="width: 7%;">Select</th>
                                            <th style="width: 43%;">Description</th>
                                            <th style="width: 10%;">Bill Date</th>
                                            <th style="width: 10%;">Bill Number</th>
                                            <th style="width: 15%;">Due Amount</th>
                                            <th style="width: 15%;">Amount</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                    <%--<tfoot>
                                        <tr>
                                            <td colspan="5" style="width: 82%; text-align: right;">Advance Amount</td>
                                            <td style="width: 18%">
                                                <asp:TextBox ID="txtAdvanceAmount" runat="server" onblur="CheckAdvance(this)" CssClass="form-control quantitydecimal" TabIndex="7"> </asp:TextBox>
                                            </td>
                                        </tr>
                                    </tfoot>--%>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">Transaction Details</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAdvanceAmount" runat="server" class="control-label"
                                    Text="Advance Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAdvanceAmount" runat="server" onblur="CheckAdvance(this)" CssClass="form-control quantitydecimal" TabIndex="7"> </asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label13" runat="server" class="control-label required-field"
                                    Text="Max Payment Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTotalAmount" disabled CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label15" runat="server" class="control-label required-field" Text="Payment Mode"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control" TabIndex="5">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="Adjustment">Adjustment</asp:ListItem>
                                    <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                    <asp:ListItem Value="Bank">Bank</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPaymentDate2" runat="server" class="control-label required-field"
                                    Text="Payment Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPaymentDate2" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div id="BankPaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label7" runat="server" class="control-label required-field"
                                        Text="Bank Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <input id="txtBankPayment" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlBankPayment" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="ChequeNChequeNumber" style="display: none">
                            <div class="col-md-2">
                                <asp:Label ID="Label12" runat="server" class="control-label required-field" Text="Cheque Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtChequeNumber" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label14" runat="server" class="control-label  required-field" Text="Cheque Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtChequeDate" runat="server" CssClass="form-control" TabIndex="10"></asp:TextBox>
                            </div>
                        </div>
                        <div id="CashPaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label5" runat="server" class="control-label required-field"
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
                        <div class="form-group" id="AdjustmentDiv">
                            <div class="col-md-2">
                                <asp:Label ID="Label10" runat="server" class="control-label required-field" Text="Adjustment Head"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlAdjustmentNodeHead" runat="server" CssClass="form-control" TabIndex="5">
                                </asp:DropDownList>
                            </div>
                            <div style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="Label11" runat="server" class="control-label"
                                        Text="Adjustment Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtAdjustmentAmount" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="CurrencyAndPaymentAmountDiv">
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
                                <asp:TextBox ID="txtLedgerAmount" runat="server" CssClass="form-control quantitydecimal" TabIndex="7"> </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="display: none">
                            <div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblPayMode" runat="server" class="control-label" Text="Payment Mode"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                    </asp:DropDownList>
                                </div>
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
                                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Income Purpose"></asp:Label>
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
                                    <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control" onchange="PopulateProjects();">
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
                        <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblChecquePaymentAccountHeadId" runat="server" class="control-label required-field"
                                        Text="Company Name"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control"
                                        TabIndex="6">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblChecqueNumber" runat="server" class="control-label required-field"
                                        Text="Cheque Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
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
                                        <asp:DropDownList ID="ddlCompanyBank" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="CashAndCashEquivalantPaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label9" runat="server" class="control-label required-field"
                                        Text="Account Head"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <input id="txtCashAndCashEquivalantPayment" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlCashAndCashEquivalantPayment" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="CardPaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Card Type"></asp:Label>
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
                                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Expiry Date"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCardHolderName" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItem()" />
                                <input id="btnCancelOrder" type="button" value="Cancel" onclick="ClearBillContainer()"
                                    class="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                        <div style="height: 250px; overflow-y: scroll;">
                            <table id="ReceiveInformationTbl" class="table table-bordered table-condensed table-hover">
                                <thead>
                                    <tr>
                                        <th style="width: 10%;">Payment Mode</th>
                                        <th style="width: 15%;">Payment Head</th>
                                        <th style="width: 15%;">Payment Amount</th>
                                        <th style="width: 10%;">Payment Date</th>
                                        <th style="width: 5%;">Currency Type</th>
                                        <th style="width: 5%;">Convertion Rate</th>
                                        <th style="width: 10%;">Cheque Date</th>
                                        <th style="width: 10%;">Cheque Number</th>
                                        <th style="width: 15%;">Total Amount</th>
                                        <th style="width: 5%;">Action</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                        <hr />
                        <div class="form-group">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label16" runat="server" class="control-label required-field"
                                    Text="Total Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTotalReceiveAmount" disabled CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Attachment</label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Documents..." />
                            </div>
                        </div>
                        <div id="DocumentInfo">
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblRemarks" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                        TabIndex="11"></asp:TextBox>
                </div>
            </div>
            <div id="DocumentDialouge" style="display: none;">
                <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                    clientidmode="static" scrolling="yes"></iframe>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="12" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClick="btnSave_Click" OnClientClick="javascript: return ValidateForm();" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="13" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return PerformClearAction();" />
                    <asp:Button ID="btnCancel" runat="server" TabIndex="14" Text="Close" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return EntryPanelVisibleFalse();" Visible="False" />
                    <asp:Button ID="btnGroupPaymentPreview1" runat="server" Text="Payment Preview" TabIndex="15"
                        CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript: return LoadPopUp();" />
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Transaction Details
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCompanyForSearch" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="Transaction Date"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtFromDate" placeholder="From Date" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtToDate" placeholder="To Date" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label18" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <div>
                                    <asp:DropDownList ID="ddlSrcTransactionType" TabIndex="1" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="0">--- All ---</asp:ListItem>
                                        <asp:ListItem>Receive</asp:ListItem>
                                        <asp:ListItem>Payment</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return SearchPayment()" />
                                <asp:Button ID="btnGroupPaymentPreview" runat="server" Text="Payment Preview" TabIndex="15"
                                    CssClass="btn btn-primary btn-sm" OnClientClick="javascript: return LoadPopUp();" />
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
                                    <th style="width: 10%;">Transaction No.</th>
                                    <th style="width: 10%;">Transaction Date</th>
                                    <th style="width: 10%;">Transaction Type</th>
                                    <th style="width: 15%;">Company Name</th>
                                    <th style="width: 30%;">Description</th>
                                    <th style="width: 10%;">Status</th>
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
    <div id="displayReceipt" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="1000" height="800" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var isIntegrated = '<%=isIntegratedGeneralLedgerDiv%>';
            if (isIntegrated > -1) {
                IntegratedGeneralLedgerDivPanelShow();
            }
            else {
                IntegratedGeneralLedgerDivPanelHide();
            }

            var x = '<%=isMessageBoxEnable%>';
            if (x > -1) {
                MessagePanelShow();
                if (x == 2) {
                    $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
                }
            }
            else {
                MessagePanelHide();
            }

            var single = '<%=isSingle%>';
            if (single == "True") {
                $('#CompanyProjectPanel').hide();
            }
            else {
                $('#CompanyProjectPanel').show();
            }
        });
    </script>
</asp:Content>



