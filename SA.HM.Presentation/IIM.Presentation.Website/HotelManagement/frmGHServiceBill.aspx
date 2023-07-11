<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGHServiceBill.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmGHServiceBill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var AddedPaymentDetailsList = new Array(), EditPaymentDetailsList = new Array(), DeletePaymentDetailsList = new Array();
        var trr = [];
        var tst = {};

        $(function () {
            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#ContentPlaceHolder1_btnSave").hide();

            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmount = '<%=txtCalculatedLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmountHiddenField = '<%=txtCalculatedLedgerAmountHiddenField.ClientID%>'

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            CommonHelper.ApplyDecimalValidation();

            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtMBankingBankId", "ContentPlaceHolder1_ddlMBankingBankId", "ContentPlaceHolder1_ddlMBankingBankId");

            $("#ContentPlaceHolder1_txtServiceQuantity").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    $("#ContentPlaceHolder1_txtServiceQuantity").focus();
                    return false;
                }
            });

            $("#ContentPlaceHolder1_ddlServiceId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCompany").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currencyType == "Local") {
                $('#' + txtConversionRate).val("")
                $('#' + txtCalculatedLedgerAmount).val("");
                $('#' + txtCalculatedLedgerAmountHiddenField).val("");
                $('#ConversionPanel').hide();
                $('#' + txtCalculatedLedgerAmount).attr("disabled", false);
            }
            else {
                $('#ConversionPanel').show();
                $('#' + txtCalculatedLedgerAmount).val("");
                $('#' + txtCalculatedLedgerAmountHiddenField).val("");
            }

            var txtPaidServiceDate = '<%=txtPaidServiceDate.ClientID%>'
            $('#ContentPlaceHolder1_txtPaidServiceDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_hfTxtPaidServiceDate").val(selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtServiceDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $('#ContentPlaceHolder1_txtExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            var txtSFromDate = '<%=txtSFromDate.ClientID%>'
            var txtSToDate = '<%=txtSToDate.ClientID%>'
            $('#ContentPlaceHolder1_txtSFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtSToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            <%--var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            var ddlGLCompany = '<%=ddlGLCompany.ClientID%>'
            var selectedIndex = parseFloat($('#' + ddlGLCompany).prop("selectedIndex"));
            if (selectedIndex > 0) {
                $("#" + ddlGLProject).attr("disabled", false);
            }
            else {
                $("#" + ddlGLProject).attr("disabled", true);
            }--%>

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
                    $('#' + txtLedgerAmount).val("")
                    $('#' + txtConversionRate).val("")
                    $('#' + txtCalculatedLedgerAmount).val("");
                    $('#' + txtCalculatedLedgerAmountHiddenField).val("");
                    $('#ConversionPanel').hide();
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", false);
                }
                else {
                    $('#ConversionPanel').show();
                    $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                    $('#' + txtCalculatedLedgerAmount).val("");
                    $('#' + txtCalculatedLedgerAmountHiddenField).val("");
                }

                var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
                if (ddlCurrency == 0) {
                    $('#ConversionPanel').hide();
                }
            }
            function OnLoadConversionRateFailed() {
            }

            var ddlSGuestType = '<%=ddlSGuestType.ClientID%>'

            if ($('#' + ddlSGuestType).val() == "InHouseGuest") {

                $('#InhouseSearchBillPanel').show();
                $('#OutSideSearchBillPanel').hide();
            }
            else {
                $('#InhouseSearchBillPanel').hide();
                $('#OutSideSearchBillPanel').show();
            }


            $('#' + ddlSGuestType).change(function () {

                if ($('#' + ddlSGuestType).val() == "InHouseGuest") {

                    $('#InhouseSearchBillPanel').show();
                    $('#OutSideSearchBillPanel').hide();
                }
                else {
                    $('#InhouseSearchBillPanel').hide();
                    $('#OutSideSearchBillPanel').show();
                }

            })

            var ddlEmployee = '<%=ddlEmployee.ClientID%>'
            var txtGuestName = '<%=txtGuestName.ClientID%>'
            $('#' + ddlEmployee).change(function () {
                if ($('#' + ddlEmployee).val() != "") {
                    $('#' + txtGuestName).val($("#<%=ddlEmployee.ClientID %> option:selected").text());
                }
                else {
                    $('#' + txtGuestName).val("");
                }
            })

            var ddlCompany = '<%=ddlCompany.ClientID%>'
            $('#' + ddlCompany).change(function () {
                if ($('#' + ddlCompany).val() != "") {
                    $('#' + txtGuestName).val($("#<%=ddlCompany.ClientID %> option:selected").text());
                }
                else {
                    $('#' + txtGuestName).val("");
                }
            })

            var txtServiceRate = '<%=txtServiceRate.ClientID%>'
            $('#' + txtServiceRate).blur(function () {
                if (isNaN($('#' + txtServiceRate).val())) {
                    toastr.warning('Entered Amount is not in correct format.');
                    return;
                }
            });


            $('#' + txtLedgerAmount).blur(function () {
                var selectedIndex = parseFloat($('#' + ddlCurrency).prop("selectedIndex"));
                if (selectedIndex < 1) {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val());

                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val()) / parseFloat($('#' + txtConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", true);
                }
            });

            $('#' + txtConversionRate).blur(function () {
                var selectedIndex = parseFloat($('#' + ddlCurrency).prop("selectedIndex"));
                if (selectedIndex < 1) {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val());
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtLedgerAmount).val()) / parseFloat($('#' + txtConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtCalculatedLedgerAmount).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmountHiddenField).val(LedgerAmount.toFixed(2));
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", true);
                }
            });

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'

            $('#' + ddlPayMode).change(function () {
                PaymentModeShowHideInformation();
            });

        });

        $(document).ready(function () {
            var txtServiceDate = '<%=txtServiceDate.ClientID%>'

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Service Bill</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var ddlGuestType = '<%=ddlGuestType.ClientID%>'
            var btnSave = '<%=btnSave.ClientID%>'
            var hfServiceBillId = '<%=hfServiceBillId.ClientID%>'

            $('#' + ddlGuestType).change(function () {
                $('#' + btnSave).val('Save');
                var txtSrcRoomNumber = '<%=txtSrcRoomNumber.ClientID%>'
                $("#" + txtSrcRoomNumber).attr("disabled", false);

                var btnSrcRoomNumber = '<%=btnSrcRoomNumber.ClientID%>'
                $("#" + btnSrcRoomNumber).attr("disabled", false);
                $('#' + hfServiceBillId).val('');
                $("#<%=hfddlRegistrationId.ClientID %>").val('');
                if ($('#' + ddlGuestType).val() == "InHouseGuest") {
                    InhousePanelVisibleTrue();
                }
                else if ($('#' + ddlGuestType).val() == "OutSideGuest") {
                    InhousePanelVisibleFalse();
                }
                PaymentModeShowHideInformation();
                ClearAllPaymentInfo();
                $("#<%=txtSrcRoomNumber.ClientID %>").val("");
                $("#<%=txtDiscountAmount.ClientID %>").val("");
                $("#<%=txtServiceRate.ClientID %>").val("");
                $("#<%=txtServiceQuantity.ClientID %>").val("1");
                $("#ContentPlaceHolder1_txtGrandTotal").val('');
                $("#ContentPlaceHolder1_txtServiceCharge").val('');
                $("#ContentPlaceHolder1_txtVatAmount").val('');
                $("#ContentPlaceHolder1_txtNetAmount").val('');
            });

            var ddlIsComplementary = '<%=ddlIsComplementary.ClientID%>'
            $('#' + ddlIsComplementary).change(function () {
                if ($('#' + ddlIsComplementary).val() == "Yes") {
                    $("#<%=txtDiscountAmount.ClientID %>").val("100");

                    if ($('#' + ddlGuestType).val() == "OutSideGuest") {
                        $("#OutSideGuestPaymentDetailInfo").hide();
                    }
                }
                else {
                    $("#<%=txtDiscountAmount.ClientID %>").val("0");

                    if ($('#' + ddlGuestType).val() == "OutSideGuest") {
                        $("#OutSideGuestPaymentDetailInfo").show();
                    }
                }
            });

            var txtServiceRate = '<%=txtServiceRate.ClientID%>'
            var txtServiceQuantity = '<%=txtServiceQuantity.ClientID%>'
            var txtDiscountAmount = '<%=txtDiscountAmount.ClientID%>'

            $('#' + txtServiceRate).blur(function () {
                SetLedgerAmount(GenarateLedgerAmount());
            });

            $('#' + txtServiceQuantity).blur(function () {
                SetLedgerAmount(GenarateLedgerAmount());
            });

            $('#' + txtDiscountAmount).blur(function () {
                SetLedgerAmount(GenarateLedgerAmount());
            });

            $('#ContentPlaceHolder1_ddlGuestType').change(function () {

                $("#ContentPlaceHolder1_txtServiceRate").val("");
                $("#ContentPlaceHolder1_txtServiceCharge").val('');
                $("#ContentPlaceHolder1_txtSDCharge").val('');
                $("#ContentPlaceHolder1_txtVatAmount").val('');
                $("#ContentPlaceHolder1_txtAdditionalCharge").val('');
                $("#ContentPlaceHolder1_txtGrandTotal").val("");
                $("#ContentPlaceHolder1_lblGrandTotal").text("Grand Total");

                CheckDuplicateByServiceAndBillCritaria();
                $("#ContentPlaceHolder1_ddlServiceId").val("0");
            });

            $('#ContentPlaceHolder1_ddlServiceId').change(function () {
                CheckDuplicateByServiceAndBillCritaria();

                if ($(this).val() != "0") {
                    PageMethods.GetPaidServiceDetails($(this).val(), OnSucceedPaidService, OnFailedPaidService);
                }
                else {
                    $("#ContentPlaceHolder1_txtServiceRate").val("");
                    $("#ContentPlaceHolder1_txtServiceCharge").val('');
                    $("#ContentPlaceHolder1_txtSDCharge").val('');
                    $("#ContentPlaceHolder1_txtVatAmount").val('');
                    $("#ContentPlaceHolder1_txtAdditionalCharge").val('');
                    $("#ContentPlaceHolder1_txtGrandTotal").val("");
                    $("#ContentPlaceHolder1_lblGrandTotal").text("Grand Total");
                }
            });

            $('#ContentPlaceHolder1_txtServiceRate').blur(function () {
                TotalRoomRateVatServiceChargeCalculation();
                SetLedgerAmount(GenarateLedgerAmount());
            });

            $('#ContentPlaceHolder1_txtServiceQuantity').blur(function () {
                TotalRoomRateVatServiceChargeCalculation();
                SetLedgerAmount(GenarateLedgerAmount());
            });

            $('#ContentPlaceHolder1_txtServiceDate').blur(function () {
                CheckDuplicateByServiceAndBillCritaria();
            });

            $('#ContentPlaceHolder1_txtBillNumber').blur(function () {
                CheckDuplicateByServiceAndBillCritaria();
            });

            $("#btnAddPayment").click(function () {
                var Amounts = $("#<%=txtLedgerAmount.ClientID %>").val();

                if (Amounts == "") {
                    toastr.warning('Entered Amount is not in correct format.');
                    $("#<%=txtLedgerAmount.ClientID %>").focus();
                    return;
                }
                else if (isNaN(Amounts)) {
                    toastr.warning('Entered Amount is not in correct format.');
                    $("#<%=txtLedgerAmount.ClientID %>").focus();
                    return;
                }

                var PaymentMode = "", CardType = "", Amount = "", PaymentModeId = "", CurrencyTypeId = "", ConversionRate = "", AccountsPostingHeadId = "", CardTypeId = "";
                var CardNumber = "", ExpireDate = "", CardHolderName = "", EmployeeId = "", CompanyId = "", EmployeeName = "", CompanyName = "", PaymentId = "", BankId = "";
                var foreignCurrencyAmount = 0;

                PaymentMode = $("#<%=ddlPayMode.ClientID %> option:selected").text();
                Amount = parseFloat($.trim($("#<%=txtLedgerAmount.ClientID %>").val()));
                PaymentModeId = $("#<%=ddlPayMode.ClientID %>").val();
                CurrencyTypeId = $("#<%=ddlCurrency.ClientID %>").val();
                ConversionRate = $.trim($("#<%=txtConversionRate.ClientID %>").val());

                if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                    foreignCurrencyAmount = parseFloat($.trim($("#<%=txtLedgerAmount.ClientID %>").val()));
                }

                if ($("#<%=ddlPayMode.ClientID %>").val() == "Refund") {
                    if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                        toastr.warning("Refund amount should be local currency");
                        return false;
                    }
                }

                BankId = $("#<%=ddlBankId.ClientID %>").val();

                if ($("#<%=ddlPayMode.ClientID %>").val() == "Cash") {
                    AccountsPostingHeadId = $("#<%=ddlCashReceiveAccountsInfo.ClientID %>").val();
                }
                else if ($("#<%=ddlPayMode.ClientID %>").val() == "Card") {
                    AccountsPostingHeadId = $("#<%=ddlCardReceiveAccountsInfo.ClientID %>").val();
                }
                else if ($("#<%=ddlPayMode.ClientID %>").val() == "Cheque") {
                    AccountsPostingHeadId = $("#<%=ddlChequeReceiveAccountsInfo.ClientID %>").val();
                }
                else if ($("#<%=ddlPayMode.ClientID %>").val() == "M-Banking") {
                    AccountsPostingHeadId = $("#<%=ddlMBankingReceiveAccountsInfo.ClientID %>").val();
                    BankId = $("#<%=ddlMBankingBankId.ClientID %>").val();
                }
                else if ($("#<%=ddlPayMode.ClientID %>").val() == "Employee") {
                    var employeeArray = $("#<%=ddlEmployee.ClientID %>").val().split("~");
                    AccountsPostingHeadId = employeeArray[1];
                    BankId = employeeArray[0];
                }
                else if ($("#<%=ddlPayMode.ClientID %>").val() == "Company") {
                    AccountsPostingHeadId = $("#<%=ddlCompany.ClientID %>").val();
                }
                else if ($("#<%=ddlPayMode.ClientID %>").val() == "Refund") {
                    AccountsPostingHeadId = $("#<%=ddlRefundAccountHead.ClientID %>").val();
                }

                CardTypeId = $("#<%=ddlCardType.ClientID %>").val();
                CardNumber = $.trim($("#<%=txtCardNumber.ClientID %>").val());
                ExpireDate = $.trim($("#<%=txtExpireDate.ClientID %>").val());
                CardHolderName = $.trim($("#<%=txtCardHolderName.ClientID %>").val());
                EmployeeId = $("#<%=ddlEmployee.ClientID %>").val();
                CompanyId = $("#<%=ddlCompany.ClientID %>").val();
                PaymentId = $("#hfPaymentId").val();

                if (CardTypeId != "")
                    CardType = $("#<%=ddlCardType.ClientID %> option:selected").text();

                if (EmployeeId != "")
                    EmployeeName = $("#<%=ddlEmployee.ClientID %> option:selected").text();

                if (CompanyId != "")
                    CompanyName = $("#<%=ddlCompany.ClientID %> option:selected").text();

                var message = "", isValid = 1;

                var ddlServiceIdVal = $("#<%=ddlServiceId.ClientID %>").val();
                if (ddlServiceIdVal == "0") {
                    message = "Please Provide Service Name.";
                    $("#<%=ddlServiceId.ClientID %>").focus();
                    isValid = 0;
                }

                if (Amount == "") {
                    message = "Please give Payment Amount";
                    isValid = 0;
                }
                else if (Amount == "0") {
                    message = "Please give Payment Amount";
                    isValid = 0;
                }

                if (PaymentModeId == "Card") {
                    if (AccountsPostingHeadId == "") {
                        message = "Please Select Payment Receive In.";
                        isValid = 0;
                    }
                    else if (CardTypeId == "") {
                        message = "Please Select Card type.";
                        isValid = 0;
                    }
                    else if (Amount == "") {
                        message = "Please Enter Payment Amount.";
                        isValid = 0;
                    }
                    else if (Amount == "0") {
                        message = "Please Enter Payment Amount.";
                        isValid = 0;
                    }
                    else if (BankId == "0") {
                        message = "Please Provide Bank Name.";
                        isValid = 0;
                    }
                }
                else if (PaymentModeId == "Employee") {
                    if (EmployeeId == "") {
                        message = "Please select Employee.";
                        isValid = 0;
                    }
                }
                else if (PaymentModeId == "Company") {
                    if (CompanyId == "") {
                        message = "Please select Company.";
                        isValid = 0;
                    }
                }
                if ($("#<%=ddlCurrency.ClientID %>").val() == 0) {
                    message = "Please select currency type.";
                    isValid = 0;
                }

                else if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                    if (ConversionRate == "") {
                        message = "Please give Conversion Rate.";
                        isValid = 0;
                    }
                }

                if (isValid == 0) {
                    toastr.warning(message);
                    return false;
                }

                if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                    Amount *= parseFloat(ConversionRate);
                }

                var RowIndex = parseInt($("#hfEditPaymentInfoRowIndex").val(), 10);

                if (RowIndex != 0) {
                    var editedTr = $("#PaymentDetailsList tbody tr").find("td:eq(14):contains('" + RowIndex + "')").parent();
                    var th = $("#PaymentDetailsList thead tr");

                    $(editedTr).find("td:eq(0)").text(PaymentMode);
                    $(editedTr).find("td:eq(2)").text(Amount);
                    $(editedTr).find("td:eq(3)").text(PaymentModeId);
                    $(editedTr).find("td:eq(4)").text(CurrencyTypeId);
                    $(editedTr).find("td:eq(5)").text(ConversionRate);
                    $(editedTr).find("td:eq(6)").text(AccountsPostingHeadId);
                    $(editedTr).find("td:eq(7)").text(CardTypeId);
                    $(editedTr).find("td:eq(8)").text(CardNumber);
                    $(editedTr).find("td:eq(9)").text(ExpireDate);
                    $(editedTr).find("td:eq(10)").text(CardHolderName);
                    $(editedTr).find("td:eq(11)").text(EmployeeId);
                    $(editedTr).find("td:eq(12)").text(CompanyId);
                    $(editedTr).find("td:eq(13)").text(PaymentId);
                    $(editedTr).find("td:eq(14)").text(BankId);
                    $(editedTr).find("td:eq(15)").text(foreignCurrencyAmount);

                    if (PaymentId != "0") {

                        RemoveServicePayment(EditPaymentDetailsList, RowIndex);

                        EditPaymentDetailsList.push(
                {
                    Amount: Amount,
                    PaymentModeId: PaymentModeId,
                    CurrencyTypeId: CurrencyTypeId,
                    ConversionRate: ConversionRate,
                    AccountsPostingHeadId: AccountsPostingHeadId,
                    CardTypeId: CardTypeId,
                    CardNumber: CardNumber,
                    ExpireDate: ExpireDate,
                    CardHolderName: CardHolderName,
                    EmployeeId: EmployeeId,
                    CompanyId: CompanyId,
                    PaymentId: PaymentId,
                    BankId: BankId,
                    ForeignCurrencyAmount: foreignCurrencyAmount,
                    RowIndex: RowIndex
                }
                );
                    }
                    else if (PaymentId == "0") {

                        RemoveServicePayment(AddedPaymentDetailsList, RowIndex);

                        AddedPaymentDetailsList.push(
                {
                    Amount: Amount,
                    PaymentModeId: PaymentModeId,
                    CurrencyTypeId: CurrencyTypeId,
                    ConversionRate: ConversionRate,
                    AccountsPostingHeadId: AccountsPostingHeadId,
                    CardTypeId: CardTypeId,
                    CardNumber: CardNumber,
                    ExpireDate: ExpireDate,
                    CardHolderName: CardHolderName,
                    EmployeeId: EmployeeId,
                    CompanyId: CompanyId,
                    PaymentId: PaymentId,
                    BankId: BankId,
                    ForeignCurrencyAmount: foreignCurrencyAmount,
                    RowIndex: RowIndex
                }
                );
                    }

                    if (CardTypeId != "") {
                        $(th).find("td:eq(1)").text("Description");
                        $(editedTr).find("td:eq(1)").text(CardType);
                    }
                    else if (EmployeeId != "") {
                        $(th).find("td:eq(1)").text("Description");
                        $(editedTr).find("td:eq(1)").text(EmployeeName);
                    }
                    else if (CompanyId != "") {
                        $(th).find("td:eq(1)").text("Description");
                        $(editedTr).find("td:eq(1)").text(CompanyName);
                    }
                    else {
                        $(th).find("td:eq(1)").text("Description");
                        $(editedTr).find("td:eq(1)").text(CardType);
                    }

                    CalculateTotalPaiedNDueAmount();

                    $("#hfEditPaymentInfoRowIndex").val("0");
                    $("#hfPaymentId").val("0");

                    ClearPaymentInfo();

                    return false;
                }

                var duplicatePayment = 0, duplicateTr = "", alreadyAddedmount = 0;

                AddNewPaymentInfoDetails(PaymentMode, CardType, Amount, PaymentModeId, CurrencyTypeId, ConversionRate,
                AccountsPostingHeadId, CardTypeId, CardNumber, ExpireDate, CardHolderName, EmployeeId, CompanyId, EmployeeName, CompanyName, PaymentId, BankId, foreignCurrencyAmount);

                CalculateTotalPaiedNDueAmount();
                ClearPaymentInfo();
            });


            $("#PaymentDetailsList").delegate("a.EditPayment", "click", function () {
                ClearPaymentInfo();
                var tr = $(this).parent().parent();
                trr = tr;
                $("#<%=ddlPayMode.ClientID %>").val($(tr).find("td:eq(3)").text());
                PaymentModeShowHideInformation();
                $("#<%=txtLedgerAmount.ClientID %>").val($(tr).find("td:eq(2)").text());
                $("#<%=ddlCurrency.ClientID %>").val($(tr).find("td:eq(4)").text());
                $("#<%=txtConversionRate.ClientID %>").val($(tr).find("td:eq(5)").text());
                $("#<%=ddlCardReceiveAccountsInfo.ClientID %>").val($(tr).find("td:eq(6)").text());
                $("#<%=ddlCardType.ClientID %>").val($(tr).find("td:eq(7)").text());
                $("#<%=txtCardNumber.ClientID %>").val($(tr).find("td:eq(8)").text());
                $("#<%=txtExpireDate.ClientID %>").val($(tr).find("td:eq(9)").text());
                $("#<%=txtCardHolderName.ClientID %>").val($(tr).find("td:eq(10)").text());
                $("#<%=ddlEmployee.ClientID %>").val($(tr).find("td:eq(11)").text());
                $("#<%=ddlCompany.ClientID %>").val($(tr).find("td:eq(12)").text());
                $("#hfPaymentId").val($(tr).find("td:eq(13)").text());
                $("#<%=ddlBankId.ClientID %>").val($(tr).find("td:eq(14)").text());
                $('#txtBankId').val($("#<%=ddlBankId.ClientID %> option:selected").text());
                $('#txtMBankingBankId').val($("#<%=ddlMBankingBankId.ClientID %> option:selected").text());
                $("#hfEditPaymentInfoRowIndex").val($(tr).find("td:eq(15)").text());
                $("#btnAddPayment").val("Edit Payment");
            });

            $("#PaymentDetailsList").delegate("a.DeletePayment", "click", function () {
                ClearPaymentInfo();

                var tr = $(this).parent().parent();
                var deletedIndex = parseInt($.trim($(tr).find("td:eq(15)").text()), 10), paymentId = $.trim($(tr).find("td:eq(13)").text());

                if (paymentId != "0") {

                    DeletePaymentDetailsList.push(
                    {
                        Amount: $(tr).find("td:eq(2)").text(),
                        PaymentModeId: $(tr).find("td:eq(3)").text(),
                        CurrencyTypeId: $(tr).find("td:eq(4)").text(),
                        ConversionRate: $(tr).find("td:eq(5)").text(),
                        AccountsPostingHeadId: $(tr).find("td:eq(6)").text(),
                        CardTypeId: $(tr).find("td:eq(7)").text(),
                        CardNumber: $(tr).find("td:eq(8)").text(),
                        ExpireDate: $(tr).find("td:eq(9)").text(),
                        CardHolderName: $(tr).find("td:eq(10)").text(),
                        EmployeeId: $(tr).find("td:eq(11)").text(),
                        CompanyId: $(tr).find("td:eq(12)").text(),
                        PaymentId: paymentId,
                        BankId: $(tr).find("td:eq(14)").text(),
                        ForeignCurrencyAmount: $(tr).find("td:eq(15)").text(),
                        RowIndex: deletedIndex
                    }
                    );

                    RemoveServicePayment(EditPaymentDetailsList, deletedIndex);
                    RemoveServicePayment(AddedPaymentDetailsList, deletedIndex);
                }
                else if (paymentId == "0") {
                    RemoveServicePayment(AddedPaymentDetailsList, deletedIndex);
                    RemoveServicePayment(EditPaymentDetailsList, deletedIndex);
                }

                $(this).parent().parent().remove();

                CalculateTotalPaiedNDueAmount();
            });

            $("#btnCancel").click(function () {
                ClearPaymentInfo();
                $("#<%=txtDiscountAmount.ClientID %>").val("");
                $("#<%=txtServiceRate.ClientID %>").val("");
                $("#<%=txtServiceQuantity.ClientID %>").val("1");
            });
        });

        function OnSucceedPaidService(result) {
            $("#ContentPlaceHolder1_txtServiceRate").val(result.UnitPriceLocal);
            $("#ContentPlaceHolder1_txtGrandTotal").val(result.UnitPriceLocal);
            if (result.IsServiceChargeEnable == true) {
                $("#<%=cbServiceCharge.ClientID %>").prop("disabled", false);
                $("#<%=cbServiceCharge.ClientID %>").prop("checked", true);
            }
            else {
                $("#<%=cbServiceCharge.ClientID %>").prop("checked", false);
                $("#<%=cbServiceCharge.ClientID %>").prop("disabled", true);
            }
            if (result.IsCitySDChargeEnable == true) {
                $("#<%=cbSDCharge.ClientID %>").prop("disabled", false);
                $("#<%=cbSDCharge.ClientID %>").prop("checked", true);
            }
            else {
                $("#<%=cbSDCharge.ClientID %>").prop("checked", false);
                $("#<%=cbSDCharge.ClientID %>").prop("disabled", true);
            }
            if (result.IsVatEnable == true) {
                $("#<%=cbVatAmount.ClientID %>").prop("disabled", false);
                $("#<%=cbVatAmount.ClientID %>").prop("checked", true);
            }
            else {
                $("#<%=cbVatAmount.ClientID %>").prop("checked", false);
                $("#<%=cbVatAmount.ClientID %>").prop("disabled", true);
            }
            if (result.IsAdditionalChargeEnable == true) {
                $("#<%=cbAdditionalCharge.ClientID %>").prop("disabled", false);
                $("#<%=cbAdditionalCharge.ClientID %>").prop("checked", true);
            }
            else {
                $("#<%=cbAdditionalCharge.ClientID %>").prop("checked", false);
                $("#<%=cbAdditionalCharge.ClientID %>").prop("disabled", true);
            }

            TotalRoomRateVatServiceChargeCalculation();

            var guestType = $("#<%=ddlGuestType.ClientID %>").val();
            if (guestType == "InHouseGuest" && (result.IsServiceChargeEnable == true || result.IsVatEnable == true)) {
                var roomRegId = $("#<%=hfRoomRegId.ClientID %>").val();
                if (roomRegId != "") {
                    PageMethods.GetRoomRegistrationVatServieChargeInfo(roomRegId, OnSucceedRoomRegistrationVatServieChargeInfo, OnFailedRoomRegistrationVatServieChargeInfo);
                }
            }
        }
        function OnFailedPaidService() {
        }

        function OnSucceedRoomRegistrationVatServieChargeInfo(result) {
            if (result.IsServiceChargeEnable == false) {
                $("#<%=cbServiceCharge.ClientID %>").prop("checked", false);
            }
            if (result.IsCitySDChargeEnable == false) {
                $("#<%=cbSDCharge.ClientID %>").prop("checked", false);
            }
            if (result.IsVatAmountEnable == false) {
                $("#<%=cbVatAmount.ClientID %>").prop("checked", false);
            }
            if (result.IsAdditionalChargeEnable == false) {
                $("#<%=cbAdditionalCharge.ClientID %>").prop("checked", false);
            }
            TotalRoomRateVatServiceChargeCalculation();
        }
        function OnFailedRoomRegistrationVatServieChargeInfo(error) {
            toastr.error(error);
        }

        function OnServiceBillNumberDuplicateSucceeded(result) {
        }
        function OnServiceBillNumberDuplicateFailed(error) {

        }

        //CompanyProjectPanel Div Visible True/False-------------------
        function CompanyProjectPanelShow() {
            $('#CompanyProjectPanel').show("slow");
        }
        function CompanyProjectPanelHide() {
            $('#CompanyProjectPanel').hide("slow");
        }

        function AddNewPaymentInfoDetails(PaymentMode, CardType, Amount, PaymentModeId, CurrencyTypeId, ConversionRate,
                                          AccountsPostingHeadId, CardTypeId, CardNumber, ExpireDate, CardHolderName, EmployeeId,
                                          CompanyId, EmployeeName, CompanyName, PaymentId, BankId, foreignCurrencyAmount) {

            var tr = "", th = "", totalRow = 0, editLink = "", deleteLink = "";

            totalRow = $("#PaymentDetailsList tbody tr").length;
            th = $("#PaymentDetailsList thead tr");

            if ($("#hfRowCount").val() == "0") {
                $("#hfRowCount").val("1");
            }
            else {
                var count = parseInt($("#hfRowCount").val());
                $("#hfRowCount").val(count + 1);
            }

            editLink = "";
            deleteLink = "<a href=\"javascript:void();\" class=\"DeletePayment\"><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }

            tr += "<td align='left' style=\"width:25%;\">" + PaymentMode + "</td>";

            if (CardTypeId != "") {
                $(th).find("td:eq(1)").text("Description");
                tr += "<td align='left' style=\"width:60%;\">" + CardType + "</td>";
            }
            else if (EmployeeId != "") {
                $(th).find("td:eq(1)").text("Description");
                tr += "<td align='left' style=\"width:60%;\">" + EmployeeName + "</td>";
            }
            else if (CompanyId != "") {
                $(th).find("td:eq(1)").text("Description");
                tr += "<td align='left' style=\"width:60%;\">" + CompanyName + "</td>";
            }
            else {
                $(th).find("td:eq(1)").text("Description");
                tr += "<td align='left' style=\"width:60%;\">" + CardType + "</td>";
            }

            tr += "<td align='left' style=\"width:25%; text-align:Left;\">" + Amount + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + PaymentModeId + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + CurrencyTypeId + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + ConversionRate + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + AccountsPostingHeadId + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + CardTypeId + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + CardNumber + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + ExpireDate + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + CardHolderName + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + EmployeeId + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + CompanyId + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + PaymentId + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + BankId + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + $("#hfRowCount").val() + "</td>";
            tr += "<td align='left' style=\"width:25%; display:none;\">" + foreignCurrencyAmount + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + editLink + deleteLink + "</td>";

            tr += "</tr>"

            $("#PaymentDetailsList tbody ").append(tr);
            if (PaymentId == "0") {
                RemoveServicePayment(AddedPaymentDetailsList, parseInt($("#hfRowCount").val(), 10));
                AddedPaymentDetailsList.push(
                        {
                            Amount: Amount,
                            PaymentModeId: PaymentModeId,
                            CurrencyTypeId: CurrencyTypeId,
                            ConversionRate: ConversionRate,
                            AccountsPostingHeadId: AccountsPostingHeadId,
                            CardTypeId: CardTypeId,
                            CardNumber: CardNumber,
                            ExpireDate: ExpireDate,
                            CardHolderName: CardHolderName,
                            EmployeeId: EmployeeId,
                            CompanyId: CompanyId,
                            PaymentId: PaymentId,
                            BankId: BankId,
                            ForeignCurrencyAmount: foreignCurrencyAmount,
                            RowIndex: parseInt($("#hfRowCount").val(), 10)
                        }
                  );
            }
        }

        function RemoveServicePayment(paymentList, rowIndex) {

            for (var i = paymentList.length - 1; i >= 0; i--) {
                if (paymentList[i].RowIndex === rowIndex) {
                    paymentList.splice(i, 1);
                    break;
                }
            }
        }

        function CheckDuplicateByServiceAndBillCritaria() {
            var guestType = $("#<%=ddlGuestType.ClientID %>").val();
            var serviceId = $("#<%=ddlServiceId.ClientID %>").val();
            var billNumber = '';
            var serviceDate = $("#<%=txtServiceDate.ClientID %>").val();
            serviceDate = GetStringFromDateTime(serviceDate);
            var serviceBillId = $("#<%=hfServiceBillId.ClientID %>").val();

            var ddlRegistrationId = $("#<%=ddlRegistrationId.ClientID %>").val();

            if (guestType == "OutSideGuest") {
                ddlRegistrationId = 0;
            }

            if (billNumber != '') {
                PageMethods.CheckDuplicateByServiceAndBillCritaria(ddlRegistrationId, serviceBillId, guestType, billNumber, serviceId, serviceDate, CheckDuplicateByServiceAndBillCritariaSucceeded, CheckDuplicateByServiceAndBillCritariaFailed);
                return false;
            }
        }

        function CheckDuplicateByServiceAndBillCritariaSucceeded(result) {
            if (result == 0) {
                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                toastr.warning("You already added this bill information.");
                return false;
            }
            else {
                $("#ContentPlaceHolder1_btnSave").attr("disabled", false);
                return false;
            }
        }
        function CheckDuplicateByServiceAndBillCritariaFailed(error) {
        }

        function PaymentAmountDetailsCalculation() {
            if ($("#<%=ddlGuestType.ClientID %>").val() == "InHouseGuest") {
                return true;
            }

            var message = "", isValid = 1;
            if ($("#<%=txtServiceRate.ClientID %>").val() == "") {
                message = "Please give Service Rate.";
                isValid = 0;
            }
            else if ($("#<%=txtServiceQuantity.ClientID %>").val() == "") {
                message = "Please give Service Quantity.";
                isValid = 0;
            }

            if (isValid == 0) {
                toastr.warning(message);
                return false;
            }

            if ($("#<%=ddlGuestType.ClientID %>").val() == "OutSideGuest") {
                if ($("#<%=ddlIsComplementary.ClientID %>").val() == "Yes") {
                    $("#<%=hfAddedPaymentDetails.ClientID %>").val("");
                    $("#<%=hfEditPaymentDetails.ClientID %>").val("");
                    $("#<%=hfDeletePaymentDetails.ClientID %>").val("");
                    $("#<%= txtLedgerAmount.ClientID %>").val("0");

                    return true;
                }
            }

            var paymentAmount = 0, totalAmount = 0;
            paymentAmount = parseFloat($.trim($("#<%= txtLedgerAmount.ClientID %>").val()));
            totalAmount = parseFloat($.trim($("#<%= txtGrandTotal.ClientID %>").val()));
            if (!CalculatePaymentNCompareWithExactAmount()) {
                return false;
            }

            var Amount = "", PaymentModeId = "", CurrencyTypeId = "", ConversionRate = "", AccountsPostingHeadId = "", CardTypeId = "";
            var CardNumber = "", ExpireDate = "", CardHolderName = "", employeeId = "", companyId = "", paymentId = "", BankId = "";

            var addedNewPaymentDetails = "", editPaymentDetails = "", deletePaymentDetails = "";

            $.each(AddedPaymentDetailsList, function (index, item) {

                if (addedNewPaymentDetails != "") {
                    addedNewPaymentDetails += "#" + item.Amount + "," + item.PaymentModeId + "," + item.CurrencyTypeId + ","
                                              + item.ConversionRate + "," + item.AccountsPostingHeadId + "," + item.CardTypeId + ","
                                              + item.CardNumber + "," + item.ExpireDate + "," + item.CardHolderName + ","
                                              + item.EmployeeId + "," + item.CompanyId + "," + item.PaymentId + "," + item.BankId + "," + item.ForeignCurrencyAmount;
                }
                else {
                    addedNewPaymentDetails = item.Amount + "," + item.PaymentModeId + "," + item.CurrencyTypeId + ","
                                         + item.ConversionRate + "," + item.AccountsPostingHeadId + "," + item.CardTypeId + ","
                                         + item.CardNumber + "," + item.ExpireDate + "," + item.CardHolderName + ","
                                         + item.EmployeeId + "," + item.CompanyId + "," + item.PaymentId + "," + item.BankId + "," + item.ForeignCurrencyAmount;
                }
            });

            $.each(EditPaymentDetailsList, function (index, item) {
                if (editPaymentDetails != "") {
                    editPaymentDetails += "#" + item.Amount + "," + item.PaymentModeId + "," + item.CurrencyTypeId + ","
                                          + item.ConversionRate + "," + item.AccountsPostingHeadId + "," + item.CardTypeId + ","
                                          + item.CardNumber + "," + item.ExpireDate + "," + item.CardHolderName + ","
                                          + item.EmployeeId + "," + item.CompanyId + "," + item.PaymentId + "," + item.BankId + "," + item.ForeignCurrencyAmount;
                }
                else {
                    editPaymentDetails = item.Amount + "," + item.PaymentModeId + "," + item.CurrencyTypeId + ","
                                     + item.ConversionRate + "," + item.AccountsPostingHeadId + "," + item.CardTypeId + ","
                                     + item.CardNumber + "," + item.ExpireDate + "," + item.CardHolderName + ","
                                     + item.EmployeeId + "," + item.CompanyId + "," + item.PaymentId + "," + item.BankId + "," + item.ForeignCurrencyAmount;
                }
            });

            $.each(DeletePaymentDetailsList, function (index, item) {

                if (deletePaymentDetails != "") {
                    deletePaymentDetails += "#" + item.Amount + "," + item.PaymentModeId + "," + item.CurrencyTypeId + ","
                                            + item.ConversionRate + "," + item.AccountsPostingHeadId + "," + item.CardTypeId + ","
                                            + item.CardNumber + "," + item.ExpireDate + "," + item.CardHolderName + ","
                                            + item.EmployeeId + "," + item.CompanyId + "," + item.PaymentId + "," + item.BankId + "," + item.ForeignCurrencyAmount;
                }
                else {
                    deletePaymentDetails = item.Amount + "," + item.PaymentModeId + "," + item.CurrencyTypeId + ","
                                       + item.ConversionRate + "," + item.AccountsPostingHeadId + "," + item.CardTypeId + ","
                                       + item.CardNumber + "," + item.ExpireDate + "," + item.CardHolderName + ","
                                       + item.EmployeeId + "," + item.CompanyId + "," + item.PaymentId + "," + item.BankId + "," + item.ForeignCurrencyAmount;
                }
            });

            $("#<%=hfAddedPaymentDetails.ClientID %>").val(addedNewPaymentDetails);
            $("#<%=hfEditPaymentDetails.ClientID %>").val(editPaymentDetails);
            $("#<%=hfDeletePaymentDetails.ClientID %>").val(deletePaymentDetails);

            return true;
        }

        function CalculatePaymentNCompareWithExactAmount() {
            var calculateAmount = 0, totalAmount = 0, refundAmount = 0;

            calculateAmount = CalculateTotalAddedPayment();
            refundAmount = CalculateTotalRefundPayment();

            if ($("#ContentPlaceHolder1_hfInclusiveGuestServiceBill").val() == "1") {
                totalAmount = (parseFloat($.trim($("#<%= txtServiceRate.ClientID %>").val()))) * (parseFloat($.trim($("#<%= txtServiceQuantity.ClientID %>").val())));
            }
            else {
                totalAmount = parseFloat($.trim($("#<%= txtGrandTotal.ClientID %>").val()));
            }

            calculateAmount -= refundAmount;

            if (calculateAmount > totalAmount) {
                toastr.warning("Amount can not be Greater than Grand Total");
                return false;
            }
            else if (calculateAmount < totalAmount) {
                toastr.warning("Amount can not be Less than Grand Total");
                return false;
            }

            return true;
        }

        function CalculateTotalAddedPayment() {
            var calculateAmount = 0;
            $("#PaymentDetailsList tbody tr").each(function () {
                if ($.trim($(this).find("td:eq(3)").text()) != "Refund") {
                    calculateAmount += parseFloat($(this).find("td:eq(2)").text());
                }
            });

            return calculateAmount;
        }

        function CalculateTotalRefundPayment() {
            var calculateAmount = 0;
            $("#PaymentDetailsList tbody tr").each(function () {
                if ($.trim($(this).find("td:eq(3)").text()) == "Refund") {
                    calculateAmount += parseFloat($(this).find("td:eq(2)").text());
                }
            });

            return calculateAmount;
        }

        function CalculateTotalPaiedNDueAmount() {
            var calculateAmount = 0, totalAmount = 0, dueAmount = 0, refundAmount = 0;
            debugger;
            calculateAmount = CalculateTotalAddedPayment();
            refundAmount = CalculateTotalRefundPayment();

            if ($("#ContentPlaceHolder1_hfInclusiveGuestServiceBill").val() == "1") {
                totalAmount = (parseFloat($.trim($("#<%= txtServiceRate.ClientID %>").val()))) * (parseFloat($.trim($("#<%= txtServiceQuantity.ClientID %>").val())));
            }
            else {
                totalAmount = parseFloat($.trim($("#<%= txtGrandTotal.ClientID %>").val()));
            }

            dueAmount = totalAmount - calculateAmount;
            dueAmount += refundAmount;

            $("#TotalPaid").text("Total Amount: " + calculateAmount);

            var floatdueAmount = parseFloat(dueAmount) + parseFloat(refundAmount);

            if (isNaN(floatdueAmount)) {
                $("#DueTotal").text("Due Amount: " + 0);
            }
            else {
                $("#DueTotal").text("Due Amount: " + dueAmount);
            }
        }

        function ClearPaymentInfo() {

            $("#<%=txtLedgerAmount.ClientID %>").val("");
            $("#<%=txtConversionRate.ClientID %>").val("");
            $("#<%=ddlCardType.ClientID %>").val("");
            $("#<%=txtCardNumber.ClientID %>").val("");
            $("#<%=txtExpireDate.ClientID %>").val("");
            $("#<%=txtCardHolderName.ClientID %>").val("");
            $("#<%=ddlEmployee.ClientID %>").val("");
            $("#<%=ddlCompany.ClientID %>").val("");
            $("#<%=ddlBankId.ClientID %>").val("0");
            $('#txtBankId').val('');
            $("#hfEditPaymentInfoRowIndex").val("0");
            $("#hfPaymentId").val("0");
            $("#ConversionPanel").hide();
            $("#btnAddPayment").val("Add Payment");
        }

        function ClearAllPaymentInfo() {
            $("#<%=txtLedgerAmount.ClientID %>").val("0");
            $("#<%=txtConversionRate.ClientID %>").val("");
            $("#<%=ddlCardType.ClientID %>").val("");
            $("#<%=txtCardNumber.ClientID %>").val("");
            $("#<%=txtExpireDate.ClientID %>").val("");
            $("#<%=txtCardHolderName.ClientID %>").val("");
            $("#<%=ddlEmployee.ClientID %>").val("");
            $("#<%=ddlCompany.ClientID %>").val("");
            $("#<%=ddlIsComplementary.ClientID %>").val("No");
            $("#<%=ddlBankId.ClientID %>").val("0");
            $('#txtBankId').val('');
            $("#hfEditPaymentInfoRowIndex").val("0");
            $("#hfRowCount").val("0");
            $("#hfPaymentId").val("0");
            $("#PaymentDetailsList tbody tr").remove();
            $("#btnAddPayment").val("Add Payment");
            $("#hfExistingBillnumberId").val("0");
            $("#hfExistingServiceDate").val("0");
            $("#TotalPaid").text("");
            $("#DueTotal").text("");
            $('#ConversionPanel').hide();
        }

        function GenarateLedgerAmount() {
            var txtServiceRate = '<%=txtServiceRate.ClientID%>'
            var ServiceRate = $('#' + txtServiceRate).val();
            var LedgerAmount = 0;
            if (ServiceRate != "") {
                var floatServiceRate = parseFloat(ServiceRate);
                if (floatServiceRate > 0) {
                    LedgerAmount = (floatServiceRate * GetValidQuantity()) - GetDiscountPercentage();
                    return LedgerAmount;
                }
                else {
                    return 0;
                }
            }
            else {
                return 0;
            }
        }

        function SetLedgerAmount(LedgerAmount) {
            CalculateTotalPaiedNDueAmount();
        }

        function GetValidQuantity() {
            var txtServiceQuantity = '<%=txtServiceQuantity.ClientID%>'
            var ServiceQuantity = $('#' + txtServiceQuantity).val();
            if (ServiceQuantity != "") {
                var intServiceQuantity = parseInt(ServiceQuantity);
                if (ServiceQuantity > 0) {
                    return ServiceQuantity;
                }
                else {
                    $('#' + txtServiceQuantity).val("1");
                    return 1;
                }
            }
            else {
                $('#' + txtServiceQuantity).val("1");
                return 1;
            }
        }

        function GetDiscountPercentage() {
            var txtServiceRate = '<%=txtServiceRate.ClientID%>'
            var ServiceRate = $('#' + txtServiceRate).val();

            var txtDiscountAmount = '<%=txtDiscountAmount.ClientID%>'
            var DiscountAmount = $('#' + txtDiscountAmount).val();
            var discount = 0;
            if (DiscountAmount == "") {
                return 0;
            }
            else {
                var intDiscountAmount = parseInt(DiscountAmount);
                if (intDiscountAmount > 0) {
                    var discount = 0;
                    discount = parseFloat(ServiceRate) * parseFloat(DiscountAmount);
                    discount = discount / parseFloat(100);
                    return discount;
                }
                else {
                    return 0;
                }
            }
        }

        var vvs = [];

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId, roomNumber) {
            PageMethods.FillForm(actionId, roomNumber, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            vvs = result;
            $("#PaymentDetailsList > tbody").html("");
            var index = $('#myTabs a[href="#tab-1"]').parent().index();
            $("#myTabs").tabs({ active: index });
            trr = result;
            if (result.ServiceBill.RegistrationId == "0") {
                $("#<%=ddlGuestType.ClientID %>").val("OutSideGuest");
            }
            else {
                $("#<%=ddlGuestType.ClientID %>").val("InHouseGuest");
            }

            var txtSrcRoomNumber = '<%=txtSrcRoomNumber.ClientID%>'
            $("#" + txtSrcRoomNumber).val(result.ServiceBill.RoomNumber);
            $("#" + txtSrcRoomNumber).attr("disabled", true);

            $("#<%=hfddlRegistrationId.ClientID %>").val(result.ServiceBill.RegistrationId);
            $("#<%=ddlRegistrationId.ClientID %>").val(result.ServiceBill.RegistrationId);
            var date = new Date(result.ServiceBill.ServiceDate);

            $("#<%=txtServiceDate.ClientID %>").val(GetStringFromDateTime(result.ServiceBill.ServiceDate));
            $("#hfExistingServiceDate").val(result.ServiceBill.ServiceDate);
            $("#hfExistingBillnumberId").val(result.ServiceBill.BillNumber);
            $("#<%=ddlServiceId.ClientID %>").val(result.ServiceBill.ServiceId);
            $("#<%=txtServiceRate.ClientID %>").val(result.ServiceBill.ServiceRate);
            $("#<%=txtServiceQuantity.ClientID %>").val(result.ServiceBill.ServiceQuantity);
            $("#<%=txtDiscountAmount.ClientID %>").val(result.ServiceBill.DiscountAmount);
            $("#<%=txtRemarks.ClientID %>").val(result.ServiceBill.Remarks);
            $("#<%=hfServiceBillId.ClientID %>").val(result.ServiceBill.ServiceBillId);
            $("#<%=txtNetAmount.ClientID %>").val(result.ServiceBill.RackRate);
            $("#<%=txtServiceCharge.ClientID %>").val(result.ServiceBill.ServiceCharge);
            $("#<%=txtVatAmount.ClientID %>").val(result.ServiceBill.VatAmount);

            if (result.ServiceBill.IsServiceChargeEnable == true) {
                $("#<%=cbServiceCharge.ClientID %>").attr("checked", true);
            }
            else {
                $("#<%=cbServiceCharge.ClientID %>").attr("checked", false);
            }
            if (result.ServiceBill.IsVatAmountEnable == true) {
                $("#<%=cbVatAmount.ClientID %>").attr("checked", true);
            }
            else {
                $("#<%=cbVatAmount.ClientID %>").attr("checked", false);
            }

            if (result.ServiceBill.IsCitySDChargeEnable == true) {
                $("#ContentPlaceHolder1_cbSDCharge").attr("checked", true);
            }
            else {
                $("#ContentPlaceHolder1_cbSDCharge").attr("checked", false);
            }
            if (result.ServiceBill.IsAdditionalChargeEnable == true) {
                $("#ContentPlaceHolder1_cbAdditionalCharge").attr("checked", true);
            }
            else {
                $("#ContentPlaceHolder1_cbAdditionalCharge").attr("checked", false);
            }

            $("#<%=hfServiceCharge.ClientID %>").val(result.ServiceBill.ServiceCharge);
            $("#<%=hfVatAmount.ClientID %>").val(result.ServiceBill.VatAmount);
            $("#<%=hfServiceRate.ClientID %>").val(result.ServiceBill.ServiceRate);
            $("#<%=hfRackRate.ClientID %>").val(result.ServiceBill.RackRate);
            $("#<%=txtSDCharge.ClientID %>").val(result.ServiceBill.CitySDCharge);
            $("#<%=txtAdditionalCharge.ClientID %>").val(result.ServiceBill.AdditionalCharge);
            $("#<%=hfSDChargeAmount.ClientID %>").val(result.ServiceBill.CitySDCharge);
            $("#<%=hfAdditionalChargeAmount.ClientID %>").val(result.ServiceBill.AdditionalCharge);

            if (result.ServiceBill.IsComplementary) {
                $("#<%=ddlIsComplementary.ClientID %>").val("Yes");
            }
            else {
                $("#<%=ddlIsComplementary.ClientID %>").val("No");
            }

            if (result.ServiceBill.RegistrationId == 0) {
                InhousePanelVisibleFalse();
                $("#<%=txtGuestName.ClientID %>").val(result.ServiceBill.GuestName);
                $("#RegistrationNumberShowOnly").hide();
            }
            else {
                $("#ContentPlaceHolder1_txtRegistrationNumberForShowOnly").val(result.ServiceBill.RegistrationNumber);
                $("#ContentPlaceHolder1_txtRegisteredGuestName").val(result.ServiceBill.GuestName);
                $("#RegistrationNumberShowOnly").show();
                InhousePanelVisibleTrue();
            }

            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "0")
                $("#ContentPlaceHolder1_btnSave").hide();
            else
                $("#ContentPlaceHolder1_btnSave").val("Update").show();
            $('#btnNewBill').hide("slow");
            $('#EntryPanel').show("slow");


            // // Fill New Items ---------------------------------------------------------------------
            var employeeName = "", companyName = "", cardType = "", expireDate = "", BankId = "";
            var i = 0, totalRow = result.ServiceBillDetails.length;

            for (i = 0; i < totalRow; i++) {
                if ($.trim(result.ServiceBillDetails[i].PaymentMode) == "Employee") {
                    $("#<%=ddlPayMode.ClientID %>").val("Cash");
                    employeeName = result.ServiceBillDetails[i].BranchName;
                }

                if ($.trim(result.ServiceBillDetails[i].PaymentMode) == "Company") {
                    $("#<%=ddlPayMode.ClientID %>").val("Cash");
                    companyName = result.ServiceBillDetails[i].BranchName;
                    employeeName = result.ServiceBillDetails[i].BranchName;
                }

                expireDate = "";

                if ($.trim(result.ServiceBillDetails[i].CardType) != "") {
                    $("#<%=ddlCardType.ClientID %>").val(result.ServiceBillDetails[i].CardType);
                    cardType = $("#<%=ddlCardType.ClientID %> option:selected").text();
                }


                if (result.ServiceBillDetails[i].ExpireDate != null)
                    expireDate = GetStringFromDateTime(result.ServiceBillDetails[i].ExpireDate);

                AddNewPaymentInfoDetails(result.ServiceBillDetails[i].PaymentMode, cardType, result.ServiceBillDetails[i].PaymentAmount,
                                         result.ServiceBillDetails[i].PaymentMode, result.ServiceBillDetails[i].FieldId, result.ServiceBillDetails[i].ConversionRate,
                                         result.ServiceBillDetails[i].AccountsPostingHeadId, result.ServiceBillDetails[i].CardType, result.ServiceBillDetails[i].CardNumber,
                                         expireDate, result.ServiceBillDetails[i].CardHolderName, result.ServiceBillDetails[i].AccountsPostingHeadId, result.ServiceBillDetails[i].AccountsPostingHeadId,
                                         employeeName, companyName, result.ServiceBillDetails[i].PaymentId, result.ServiceBillDetails[i].BankId, result.ServiceBillDetails[i].CurrencyAmount);
                cardType = "";
            }

            SetLedgerAmount(GenarateLedgerAmount());
            var total = parseFloat(result.ServiceBill.TotalCalculatedAmount);

            $("#<%=txtGrandTotal.ClientID %>").val(total);

            var inclusiveBill = 0;
            if ($("#ContentPlaceHolder1_hfInclusiveGuestServiceBill").val() != "") {
                inclusiveBill = parseInt($("#ContentPlaceHolder1_hfInclusiveGuestServiceBill").val(), 10);
            }

            if (inclusiveBill == 1) {
                $("#lblGrandTotal").text('Rack Rate');
            }
            else {
                $("#lblGrandTotal").text('Grand Total');
            }

            CalculateTotalPaiedNDueAmount();
            ComplementaryShowHide();
        }

        function ComplementaryShowHide() {
            var ddlIsComplementary = '<%=ddlIsComplementary.ClientID%>'
            if ($('#' + ddlIsComplementary).val() == "Yes") {
                var ddlGuestType = '<%=ddlGuestType.ClientID%>'
                $("#<%=txtDiscountAmount.ClientID %>").val("100");

                if ($('#' + ddlGuestType).val() == "OutSideGuest") {
                    $("#OutSideGuestPaymentDetailInfo").hide();
                }
            }
            else {
                $("#<%=txtDiscountAmount.ClientID %>").val("0");

                if ($('#' + ddlGuestType).val() == "OutSideGuest") {
                    $("#OutSideGuestPaymentDetailInfo").show();
                }
            }
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
            window.location = "frmGHServiceBill.aspx?DeleteConfirmation=Deleted"
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            ClearAllPaymentInfo();
            var txtSrcRoomNumber = '<%=txtSrcRoomNumber.ClientID%>'
            $("#" + txtSrcRoomNumber).attr("disabled", false);

            var btnSrcRoomNumber = '<%=btnSrcRoomNumber.ClientID%>'
            $("#" + btnSrcRoomNumber).attr("disabled", false);

            $("#<%=ddlRegistrationId.ClientID %>").val(0);
            var date = new Date();
            $("#<%=txtServiceDate.ClientID %>").text(GetStringFromDateTime(date));
            $("#<%=txtGuestName.ClientID %>").val('');
            $("#<%=ddlServiceId.ClientID %>").val(0);
            $("#<%=txtServiceRate.ClientID %>").val('0');
            $("#<%=txtServiceQuantity.ClientID %>").val('1');
            $("#<%=txtDiscountAmount.ClientID %>").val('0');
            $("#<%=hfServiceBillId.ClientID %>").val('');
            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "1")
                $("#ContentPlaceHolder1_btnSave").val("Save").show();
            else
                $("#ContentPlaceHolder1_btnSave").hide();

            
            $("#<%=txtSrcRoomNumber.ClientID %>").val('');
            $("#<%=txtRegisteredGuestName.ClientID %>").val('');
            $("#<%=txtServiceCharge.ClientID %>").val('0');
            $("#<%=txtSDCharge.ClientID %>").val('0');
            $("#<%=txtVatAmount.ClientID %>").val('0');
            $("#<%=txtAdditionalCharge.ClientID %>").val('0');


            $("#<%=txtGrandTotal.ClientID %>").val('0');
            $("#<%=txtRemarks.ClientID %>").val('');
            return false;
        }
        //Div Visible True/False-------------------
        function InhousePanelVisibleTrue() {
            $('#OutSideGuestInfo').hide();
            $('#SrcBillNumberInfo').hide();
            $('#GuestRegistrationInfo').show();
            $('#InHouseGuestSearchInfo').show();
            $('#GridInformation').show();
            $('#OutSideGuestPaymentDetailInfo').hide();
            return false;
        }
        function InhousePanelVisibleFalse() {
            $('#OutSideGuestInfo').show();
            $('#SrcBillNumberInfo').show();
            $('#GuestRegistrationInfo').hide();
            $('#InHouseGuestSearchInfo').hide();
            $("#RegistrationNumberShowOnly").hide();
            $('#GridInformation').show();
            $('#OutSideGuestPaymentDetailInfo').show();
            return false;
        }

        //Hide Show Function
        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }

        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }

        function EntryPanelVisibleTrue() {
            return false;
        }
        function EntryPanelVisibleFalse() {
            PerformClearAction();
            return false;
        }

        function HideInformationForCash() {
            $('#CashReceiveAccountsInfo').show();
            $('#CardReceiveAccountsInfo').hide();
            $('#ChequeReceiveAccountsInfo').hide();
            $('#CardPaymentAccountHeadDiv').hide();
            $('#ChecquePaymentAccountHeadDiv').hide();
            $('#PaidByOtherRoomDiv').hide();
            $('#CompanyPaymentAccountHeadDiv').hide();
            $("#EmployeeListContainer").hide();
            $("#GuestCompanyContainer").hide();
            $("#PaymentDetailsContainer").show();
            ClearPaymentInfo();
        }

        function PaymentModeShowHideInformation() {
            var ddlPayMode = $("#<%=ddlPayMode.ClientID%>").val();

            if (ddlPayMode == "Cash") {
                $("#IntegratedGeneralLedgerDiv").hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                HideInformationForCash();
            }
            else if (ddlPayMode == "Card") {
                $("#IntegratedGeneralLedgerDiv").hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').show();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $("#EmployeeListContainer").hide();
                $("#GuestCompanyContainer").hide();
                $("#PaymentDetailsContainer").show();
                $('#MBankingPaymentAccountHeadDiv').hide();
            }
            else if (ddlPayMode == "M-Banking") {
                $('#MBankingPaymentAccountHeadDiv').show();
                $("#IntegratedGeneralLedgerDiv").hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $("#EmployeeListContainer").hide();
                $("#GuestCompanyContainer").hide();
                $("#PaymentDetailsContainer").show();
            }
            else if (ddlPayMode == "Cheque") {
                $("#IntegratedGeneralLedgerDiv").hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $("#EmployeeListContainer").hide();
                $("#GuestCompanyContainer").hide();
                $("#PaymentDetailsContainer").hide();
                $("#PaymentDetailsContainer").show();
                $('#MBankingPaymentAccountHeadDiv').hide();
            }
            else if (ddlPayMode == "Employee") {
                $("#EmployeeListContainer").show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $("#GuestCompanyContainer").hide();
                $("#PaymentDetailsContainer").show();
                $("#IntegratedGeneralLedgerDiv").hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
            }
            else if (ddlPayMode == "Company") {
                $("#GuestCompanyContainer").show();
                $("#EmployeeListContainer").hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $("#PaymentDetailsContainer").show();
                $("#IntegratedGeneralLedgerDiv").hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
            }
        }
        <%--function PopulateProjects() {
            $("#<%=ddlGLProject.ClientID%>").attr("disabled", "disabled");
            if ($('#<%=ddlGLCompany.ClientID%>').val() == "0") {
                $('#<%=ddlGLProject.ClientID %>').empty().append('<option selected="selected" value="0">Please select</option>');
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
                        toastr.info(response.d);
                    }
                });
            }
        }--%>

        <%--function OnProjectsPopulated(response) {
            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            $("#" + ddlGLProject).attr("disabled", false);
            PopulateControl(response.d, $("#<%=ddlGLProject.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }--%>

        $(function () {
            $("#myTabs").tabs();
        });

        function ToggleFieldVisibleForAllActiveReservation(ctrl) {
            if ($(ctrl).attr('checked')) {
                $("#ddlPaidServiceInformationDiv").show();
                $("#ddlServiceInformationDiv").hide();
                $("#WithoutPaidServiceInfoDiv").hide();
                $("#PaidServiceDateInformationDiv").dialog({
                    width: 450,
                    height: 150,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    title: "", ////TODO add title
                    show: 'slide'
                });
                var btnPaidServiceDate = '<%=btnPaidServiceDate.ClientID%>'
                $("#" + btnPaidServiceDate).focus();
            }
            else {
                $("#WithoutPaidServiceInfoDiv").show();
                $("#ddlPaidServiceInformationDiv").hide();
                $("#ddlServiceInformationDiv").show();
            }
        }

        function ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(ctrl) {
            TotalRoomRateVatServiceChargeCalculation();
        }

        function ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(ctrl) {
            TotalRoomRateVatServiceChargeCalculation();
        }

        function TotalRoomRateVatServiceChargeCalculation() {

            var inclusiveBill = 0, Vat = 0.00, ServiceCharge = 0.00, cityCharge = 0.00, additionalCharge = 0.00;
            var additionalChargeType = "Fixed", isRatePlusPlus = 1, isVatEnableOnGuestHouseCityCharge = 0, isCitySDChargeEnableOnServiceCharge = 0;
            var cbVatAmountVal = 1, cbServiceChargeVal = 1, cbCityChargeVal = 1, cbAdditionalChargeVal = 1;

            if ($("#ContentPlaceHolder1_hfIsRatePlusPlus").val() != "")
            { isRatePlusPlus = parseInt($("#ContentPlaceHolder1_hfIsRatePlusPlus").val(), 10); }

            if ($("#ContentPlaceHolder1_hfGuestServiceVat").val() != "")
                Vat = parseFloat($("#ContentPlaceHolder1_hfGuestServiceVat").val());

            if ($("#ContentPlaceHolder1_hfGuestServiceServiceCharge").val() != "")
                ServiceCharge = parseFloat($("#ContentPlaceHolder1_hfGuestServiceServiceCharge").val());

            if ($("#ContentPlaceHolder1_hfCityCharge").val() != "")
                cityCharge = parseFloat($("#ContentPlaceHolder1_hfCityCharge").val());

            if ($("#ContentPlaceHolder1_hfAdditionalCharge").val() != "")
                additionalCharge = parseFloat($("#ContentPlaceHolder1_hfAdditionalCharge").val());

            if ($("#ContentPlaceHolder1_hfAdditionalChargeType").val() != "")
                additionalChargeType = $("#ContentPlaceHolder1_hfAdditionalChargeType").val();

            if ($("#ContentPlaceHolder1_hfIsVatOnSDCharge").val() != "")
                isVatEnableOnGuestHouseCityCharge = parseInt($("#ContentPlaceHolder1_hfIsVatOnSDCharge").val(), 10);

            if ($("#<%=hfIsCitySDChargeEnableOnServiceCharge.ClientID %>").val() != "")
                isCitySDChargeEnableOnServiceCharge = parseInt($("#<%=hfIsCitySDChargeEnableOnServiceCharge.ClientID %>").val(), 10);

            if ($("#ContentPlaceHolder1_hfInclusiveGuestServiceBill").val() != "") {
                inclusiveBill = parseInt($("#ContentPlaceHolder1_hfInclusiveGuestServiceBill").val(), 10);
            }

            if ($('#ContentPlaceHolder1_cbServiceCharge').is(':checked')) {
                cbServiceChargeVal = 1;
            }
            else {
                cbServiceChargeVal = 0;
                ServiceCharge = 0.00;
            }

            if ($('#ContentPlaceHolder1_cbSDCharge').is(':checked')) {
                cbCityChargeVal = 1;
            }
            else {
                cbCityChargeVal = 0;
                cityCharge = 0.00;
            }

            if ($('#ContentPlaceHolder1_cbVatAmount').is(':checked')) {
                cbVatAmountVal = 1;
            }
            else {
                cbVatAmountVal = 0;
                Vat = 0.00;
            }

            if ($('#ContentPlaceHolder1_cbAdditionalCharge').is(':checked')) {
                cbAdditionalChargeVal = 1;
            }
            else {
                cbAdditionalChargeVal = 0;
                additionalCharge = 0.00;
                additionalChargeType = "Percentage";
            }

            var serviceRate = parseFloat($('#ContentPlaceHolder1_txtServiceRate').val());
            var txtServiceRateVal = parseFloat(serviceRate * parseInt($('#ContentPlaceHolder1_txtServiceQuantity').val()));

            var discountType = "Fixed";
            var discountAmount = 0;

            var InnboardRateCalculationInfo = CommonHelper.GetRackRateServiceChargeVatInformation(txtServiceRateVal, ServiceCharge, cityCharge,
                Vat, additionalCharge, additionalChargeType, inclusiveBill, isRatePlusPlus, isVatEnableOnGuestHouseCityCharge, isCitySDChargeEnableOnServiceCharge,
                                              parseInt(cbVatAmountVal, 10), parseInt(cbServiceChargeVal, 10), parseInt(cbCityChargeVal, 10),
                                              parseInt(cbAdditionalChargeVal, 10), discountType, discountAmount
                      );
            tst = InnboardRateCalculationInfo;
            OnLoadRackRateServiceChargeVatInformationSucceeded(InnboardRateCalculationInfo, serviceRate, inclusiveBill);
        }

        function OnLoadRackRateServiceChargeVatInformationSucceeded(result, serviceRate, inclusiveBill) {
            if (result.RackRate > 0) {
                $("#ContentPlaceHolder1_txtServiceCharge").val(toFixed(result.ServiceCharge, 2));
                $("#ContentPlaceHolder1_txtSDCharge").val(toFixed(result.SDCityCharge, 2));
                $("#ContentPlaceHolder1_txtVatAmount").val(toFixed(result.VatAmount, 2));
                $("#ContentPlaceHolder1_txtAdditionalCharge").val(toFixed(result.AdditionalCharge, 2));

                if (inclusiveBill == 1) {
                    //$("#ContentPlaceHolder1_txtGrandTotal").val(toFixed(result.RackRate, 2));
                    $("#ContentPlaceHolder1_txtNetAmount").val(toFixed(result.RackRate, 2));
                    $("#ContentPlaceHolder1_hfRackRate").val(toFixed(result.RackRate, 2));
                    $("#ContentPlaceHolder1_hfGrandTotal").val(toFixed(result.GrandTotal, 2));
                    //("#lblGrandTotal").text('MRack Rate');
                    $("#lblGrandTotal").text('Grand Total');
                    $("#ContentPlaceHolder1_txtGrandTotal").val(toFixed(result.CalculatedAmount, 2));
                }
                else {
                    $("#ContentPlaceHolder1_txtGrandTotal").val(toFixed(result.CalculatedAmount, 2));
                    $("#ContentPlaceHolder1_txtNetAmount").val(toFixed(result.CalculatedAmount, 2));
                    $("#ContentPlaceHolder1_hfRackRate").val(toFixed(result.CalculatedAmount, 2));
                    $("#ContentPlaceHolder1_hfGrandTotal").val(toFixed(result.CalculatedAmount, 2));
                    $("#lblGrandTotal").text('Grand Total');
                }

                $("#ContentPlaceHolder1_hfServiceCharge").val(toFixed(result.ServiceCharge, 2));
                $("#ContentPlaceHolder1_hfVatAmount").val(toFixed(result.VatAmount, 2));
                $("#ContentPlaceHolder1_hfServiceRate").val(toFixed(serviceRate, 2));
                $("#ContentPlaceHolder1_hfSDChargeAmount").val(toFixed(result.SDCityCharge, 2));
                $("#ContentPlaceHolder1_hfAdditionalChargeAmount").val(toFixed(result.AdditionalCharge, 2));
            }
            else {
                $("#ContentPlaceHolder1_txtServiceCharge").val('0');
                $("#ContentPlaceHolder1_txtSDCharge").val('0');
                $("#ContentPlaceHolder1_txtVatAmount").val('0');
                $("#ContentPlaceHolder1_txtAdditionalCharge").val('0');
                $("#ContentPlaceHolder1_hfServiceCharge").val('0');
                $("#ContentPlaceHolder1_hfVatAmount").val('0');
                $("#ContentPlaceHolder1_hfSDChargeAmount").val('0');
                $("#ContentPlaceHolder1_hfAdditionalChargeAmount").val('0');

                $("#ContentPlaceHolder1_txtNetAmount").val('0');
                $("#ContentPlaceHolder1_hfRackRate").val('0');
                $("#ContentPlaceHolder1_hfGrandTotal").val('0');
                $("#ContentPlaceHolder1_txtGrandTotal").val('0');
                $("#ContentPlaceHolder1_hfServiceRate").val('0');

            }

            return false;
        }
    </script>
    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <asp:HiddenField ID="hfIsUpdatePermission" runat="server" />
    <asp:HiddenField ID="hfRoomRegId" runat="server" />
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfInclusiveGuestServiceBill" runat="server" />
    <asp:HiddenField ID="hfGuestServiceVat" runat="server" />
    <asp:HiddenField ID="hfGuestServiceServiceCharge" runat="server" />
    <asp:HiddenField ID="hfCityCharge" runat="server" />
    <asp:HiddenField ID="hfAdditionalCharge" runat="server" />
    <asp:HiddenField ID="hfAdditionalChargeType" runat="server" />    
    <asp:HiddenField ID="hfIsRatePlusPlus" runat="server" />
    <asp:HiddenField ID="hfServiceRate" runat="server" />
    <asp:HiddenField ID="hfRackRate" runat="server" />
    <asp:HiddenField ID="hfGrandTotal" runat="server" />
    <asp:HiddenField ID="hfAddedPaymentDetails" runat="server" Value="" />
    <asp:HiddenField ID="hfEditPaymentDetails" runat="server" Value="" />
    <asp:HiddenField ID="hfDeletePaymentDetails" runat="server" Value="" />
    <asp:HiddenField ID="hfddlRegistrationId" runat="server" Value="" />
    <asp:HiddenField ID="hfGuestCheckInDate" runat="server" Value="" />
    <asp:HiddenField ID="hfBillingStartDate" runat="server" Value="" />
    <input id="hfRowCount" type="hidden" value="0" />
    <input id="hfEditPaymentInfoRowIndex" type="hidden" value="0" />
    <input id="hfExistingBillnumberId" type="hidden" value="0" />
    <input id="hfExistingServiceDate" type="hidden" value="0" />
    <input id="hfPaymentId" type="hidden" value="0" />
    <asp:HiddenField ID="hfIsVatOnSDCharge" runat="server" />
    <asp:HiddenField ID="hfIsCitySDChargeEnableOnServiceCharge" runat="server" />
    <asp:HiddenField ID="hfIsServiceBillWithoutInHouseGuest" runat="server" Value="" />
    <div id="PaidServiceDateInformationDiv" class="form-group" style="display: none;">
        <div style="height: 25px">
        </div>
        <div class="col-md-2">
            <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Service Date"></asp:Label>
        </div>
        <div class="col-md-2">
            <asp:HiddenField ID="hfTxtPaidServiceDate" runat="server" />
            <asp:TextBox ID="txtPaidServiceDate" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnPaidServiceDate" runat="server" Text="Ok" CssClass="btn btn-primary"
                OnClick="btnPaidServiceDate_Click" />
        </div>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Service Bill Info</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Service Bill </a></li>
        </ul>
        <div id="tab-1">
            <div id="btnNewBill" class="btn-toolbar" style="display: none;">
                <button onclick="javascript: return EntryPanelVisibleTrue();" class="btn btn-primary">
                    <i class="icon-plus"></i>New Service Add</button>
                <div class="btn-group">
                </div>
            </div>

            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Service Bill
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <asp:Panel ID="pnlGuestTypeInfo" runat="server">
                            <div class="form-group">
                                <div id="lblGuestTypeDiv" runat="server">
                                    <label for="GuestType" class="control-label col-md-2">
                                        Guest Type</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlGuestType" runat="server" CssClass="form-control" TabIndex="5">
                                        <asp:ListItem Value="InHouseGuest">In-House Guest</asp:ListItem>
                                        <asp:ListItem Value="OutSideGuest">Walk-In Guest</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div id="InHouseGuestSearchInfo">
                                    <label for="RoomNumber" class="control-label col-md-2">
                                        Room Number</label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2" style="text-align: left; padding-left: 0;">
                                        <asp:Button ID="btnSrcRoomNumber" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                                            OnClick="btnSrcRoomNumber_Click" />
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div id="GuestRegistrationInfo">
                            <div class="form-group">
                                <div runat="server" id="lblRegistrationNumberDiv">
                                    <label for="RegistrationNumber" class="control-label col-md-2">
                                        Registration Number</label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlRegistrationId" runat="server" CssClass="form-control" Enabled="False">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2" style="display: none;">
                                    <asp:CheckBox ID="chkAllActiveReservation" runat="server" CssClass="form-control customChkBox"
                                        Text="" onclick="javascript: return ToggleFieldVisibleForAllActiveReservation(this);"
                                        TabIndex="2" />
                                    <asp:Label ID="lblActivePaidServiceList" runat="server" class="control-label" Text="Active Paid Service List"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="GuestName" class="control-label col-md-2">
                                    Guest Name</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtRegisteredGuestName" runat="server" CssClass="form-control" Enabled="false"
                                        TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="RegistrationNumberShowOnly" style="display: none;">
                            <div class="form-group">
                                <label for="RegistrationNumber" class="control-label col-md-2">
                                    Registration Number</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtRegistrationNumberForShowOnly" runat="server" CssClass="form-control"
                                        Enabled="false" TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="OutSideGuestInfo" style="display: none;">
                            <div class="form-group">
                                <label for="GuestName" class="control-label col-md-2">
                                    Guest Name</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtGuestName" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" style="display: none;">
                            <label for="RoomNumber" class="control-label col-md-2">
                                Room Number</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="ServiceName" class="control-label col-md-2">
                                Service Name</label>
                            <div class="col-md-10">
                                <div id="ddlServiceInformationDiv">
                                    <asp:DropDownList ID="ddlServiceId" runat="server" CssClass="form-control" TabIndex="3">
                                    </asp:DropDownList>
                                </div>
                                <div id="ddlPaidServiceInformationDiv" style="display: none;">
                                    <asp:DropDownList ID="ddlPaidServiceId" runat="server" CssClass="form-control" TabIndex="3">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="WithoutPaidServiceInfoDiv">
                            <div class="form-group">
                                <label for="ServiceRate" class="control-label col-md-2">
                                    Service Rate</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtServiceRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="4"></asp:TextBox>
                                </div>
                                <label for="Quantity" class="control-label col-md-2">
                                    Quantity</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtServiceQuantity" runat="server" CssClass="form-control" TabIndex="5">1</asp:TextBox>
                                    <div style="display: none;">
                                        <asp:Label ID="lblDiscountAmount" runat="server" class="control-label" Text="Discount Amt(%)"></asp:Label>
                                        <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div id="ServiceChargeLabel" class="col-md-2" style="text-align: right;">
                                    <asp:Label ID="lbl" runat="server" CssClass="control-label required-field" Text="Service Charge"></asp:Label>
                                </div>
                                <div id="ServiceChargeControl" class="col-md-4">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtServiceCharge" runat="server" TabIndex="22" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfServiceCharge" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="cbServiceCharge" runat="server" Text="" onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                TabIndex="8" Checked="True" />
                                        </span>
                                    </div>
                                </div>
                                <div id="VatAmountLabel" class="col-md-2" style="text-align: right;">
                                    <asp:Label ID="lblVatAmount" runat="server" CssClass="control-label required-field" Text="Vat Amount"></asp:Label>
                                </div>
                                <div id="VatAmountControl" class="col-md-4">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="22" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfVatAmount" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="cbVatAmount" runat="server" Text="" onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                TabIndex="8" Checked="True" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div id="CityChargeLabel" class="col-md-2" style="text-align: right;">
                                    <asp:Label ID="lblCityChargeLabel" runat="server" CssClass="control-label required-field" Text="SD Charge"></asp:Label>
                                </div>
                                <div id="CityChargeControl" class="col-md-4">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtSDCharge" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfSDChargeAmount" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="cbSDCharge" runat="server" onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                Checked="True" />
                                        </span>
                                    </div>
                                </div>
                                <div id="AdditionalChargeLabel" class="col-md-2" style="text-align: right;">
                                    <asp:Label ID="lblAdditionalCharge" runat="server" CssClass="control-label required-field" Text="Additional Charge"></asp:Label>
                                </div>
                                <div id="AdditionalChargeControl" class="col-md-4">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtAdditionalCharge" runat="server" CssClass="form-control"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfAdditionalChargeAmount" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="cbAdditionalCharge" runat="server" onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);" Checked="True" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div style="display: none;">
                                    <label for="ServiceDate" class="control-label col-md-2">
                                        Service Date</label>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="hfServiceBillId" runat="server" Value=""></asp:HiddenField>
                                        <asp:TextBox ID="txtServiceDate" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="RackRateDiv" runat="server">
                                    <label for="RackRate" class="control-label col-md-2">
                                        Rack Rate</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control" TabIndex="23"
                                            Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lblGrandTotal" for="GrandTotal" class="control-label col-md-2">
                                    Grand Total</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="form-control" TabIndex="6"
                                        ReadOnly="true"></asp:TextBox>
                                </div>
                                <div id="ComplementaryItemDiv">
                                    <label for="ComplimentaryItem" class="control-label col-md-2">
                                        Complimentary Item</label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlIsComplementary" runat="server" CssClass="form-control"
                                            TabIndex="5">
                                            <asp:ListItem>No</asp:ListItem>
                                            <asp:ListItem>Yes</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="OutSideGuestPaymentDetailInfo" style="display: none;">
                            <div class="form-group">
                                <label for="PaymentMode" class="control-label col-md-2">
                                    Payment Mode</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                        <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                        <asp:ListItem Value="Card">Card</asp:ListItem>
                                        <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                        <asp:ListItem Value="M-Banking">M-Banking</asp:ListItem>
                                        <asp:ListItem Value="Company">Company</asp:ListItem>
                                        <asp:ListItem Value="Employee">Employee</asp:ListItem>
                                        <asp:ListItem Value="Refund">Refund</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <label for="CurrencyType" class="control-label col-md-2">
                                    Currency Type</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="IntegratedGeneralLedgerDiv" style="display: none;">
                                <div id="AdvanceDivPanel">
                                    <div class="form-group">
                                        <label for="PaymentReceiveIn" class="control-label col-md-2">
                                            Payment Receive In</label>
                                        <div class="col-md-10">
                                            <div id="CashReceiveAccountsInfo">
                                                <asp:DropDownList ID="ddlCashReceiveAccountsInfo" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div id="CardReceiveAccountsInfo" style="display: none;">
                                                <asp:DropDownList ID="ddlCardReceiveAccountsInfo" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div id="ChequeReceiveAccountsInfo" style="display: none;">
                                                <asp:DropDownList ID="ddlChequeReceiveAccountsInfo" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div id="MBankingReceiveAccountsInfo" style="display: none;">
                                                <asp:DropDownList ID="ddlMBankingReceiveAccountsInfo" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                            <div id="RefundAccountInfo" style="display: none;">
                                                <asp:DropDownList ID="ddlRefundAccountHead" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="form-group" id="CompanyProjectPanel" style="display: none;">
                                    <label for="Company" class="control-label col-md-2">
                                        Company</label>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                                        <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control" onchange="PopulateProjects();">
                                        </asp:DropDownList>
                                    </div>
                                    <label for="Project" class="control-label col-md-2">
                                        Project</label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlGLProject" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>--%>
                            </div>
                            <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                                <div class="form-group">
                                    <label for="ChequeNumber" class="control-label col-md-2">
                                        Cheque Number</label>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="CardPaymentAccountHeadDiv" style="display: none;">
                                <div class="form-group">
                                    <label for="CardType" class="control-label col-md-2">
                                        Card Type</label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlCardType" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="">---Please Select---</asp:ListItem>
                                            <asp:ListItem Value="a">American Express</asp:ListItem>
                                            <asp:ListItem Value="m">Master Card</asp:ListItem>
                                            <asp:ListItem Value="v">Visa Card</asp:ListItem>
                                            <asp:ListItem Value="d">Discover Card</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <label for="CardNumber" class="control-label col-md-2">
                                        Card Number</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="BankName" class="control-label col-md-2">
                                        Bank Name</label>
                                    <div class="col-md-10">
                                        <input id="txtBankId" type="text" class="form-control" />
                                        <div style="display: none;">
                                            <asp:DropDownList ID="ddlBankId" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group" style="display: none;">
                                    <label for="ExpiryDate" class="control-label col-md-2">
                                        Expiry Date</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <label for="CardHolderName" class="control-label col-md-2">
                                        Card Holder Name</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCardHolderName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="MBankingPaymentAccountHeadDiv" style="display: none;">
                                <div class="form-group">
                                    <label for="BankName" class="control-label col-md-2">
                                        Bank Name</label>
                                    <div class="col-md-10">
                                        <input id="txtMBankingBankId" type="text" class="form-control" />
                                        <div style="display: none;">
                                            <asp:DropDownList ID="ddlMBankingBankId" runat="server" CssClass="form-control" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="EmployeeListContainer" style="display: none;">
                                <div class="form-group">
                                    <label for="Employee" class="control-label col-md-2">
                                        Employee</label>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div id="GuestCompanyContainer" style="display: none;">
                                <div class="form-group">
                                    <label for="GuestCompany" class="control-label col-md-2">
                                        Guest Company</label>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="PaymentAmount" class="control-label col-md-2">
                                    Payment Amount</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtLedgerAmount" runat="server" CssClass="form-control quantitydecimal"> </asp:TextBox>
                                </div>
                                <div id="ConversionPanel">
                                    <label for="ConversionRate" class="control-label col-md-2">
                                        Conversion Rate</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div id="CalculatedAmountDiv" style="display: none;">
                                <div class="form-group">
                                    <label for="CalculatedAmount" class="control-label col-md-2">
                                        Calculated Amount</label>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="txtCalculatedLedgerAmountHiddenField" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtCalculatedLedgerAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" id="PaymentDetailsContainer">
                                <div class="row">
                                    <div class="col-md-12">
                                        <input id="btnAddPayment" type="button" value="Add Payment" class="btn btn-primary btn-sm" />
                                        <input id="btnCancel" type="button" value="Cancel" class="btn btn-primary btn-sm" />
                                    </div>
                                </div>
                                <div class="col-md-12" style="padding-top: 5px;">
                                    <table id="PaymentDetailsList" class="table table-bordered table-condensed table-responsive"
                                        width="95%">
                                        <colgroup>
                                            <col style="width: 20%;" />
                                            <col style="width: 40%;" />
                                            <col style="width: 25%;" />
                                            <col style="width: 15%;" />
                                            <col style="width: 25%; display: none;" />
                                            <col style="width: 25%; display: none;" />
                                            <col style="width: 25%; display: none;" />
                                            <col style="width: 25%; display: none;" />
                                            <col style="width: 25%; display: none;" />
                                            <col style="width: 25%; display: none;" />
                                            <col style="width: 25%; display: none;" />
                                            <col style="width: 25%; display: none;" />
                                            <col style="width: 2%; display: none;" />
                                            <col style="width: 2%; display: none;" />
                                            <col style="width: 2%; display: none;" />
                                            <col style="width: 2%; display: none;" />
                                            <col style="width: 2%; display: none;" />
                                        </colgroup>
                                        <thead>
                                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                                <td>Payment Mode
                                                </td>
                                                <td>Card Type
                                                </td>
                                                <td>Amount
                                                </td>
                                                <td>Action
                                                </td>
                                                <td style="display: none;">paymentModeId
                                                </td>
                                                <td style="display: none;">CurrencyTypeId
                                                </td>
                                                <td style="display: none;">ConversionRate
                                                </td>
                                                <td style="display: none;">BankId
                                                </td>
                                                <td style="display: none;">CardTypeId
                                                </td>
                                                <td style="display: none;">CardNumber
                                                </td>
                                                <td style="display: none;">ExpireDate
                                                </td>
                                                <td style="display: none;">Card Holder Name
                                                </td>
                                                <td style="display: none;">Employee
                                                </td>
                                                <td style="display: none;">Guest Company
                                                </td>
                                                <td style="display: none;">PaymentId
                                                </td>
                                                <td style="display: none;">RowNumber
                                                </td>
                                                <td style="display: none;">Foreign Currency
                                                </td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                                <div id="TotalPaid" class="totalAmout col-md-2">
                                </div>
                                <div id="DueTotal" class="totalAmout col-md-6">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Remarks" class="control-label col-md-2">
                                Remarks</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                                    TabIndex="8"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    TabIndex="9" OnClientClick="javascript: return PaymentAmountDetailsCalculation();"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="10" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                                <asp:Button ID="btnCancell" runat="server" TabIndex="11" Text="Close" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript: return EntryPanelVisibleFalse();" Visible="False" />
                                <asp:Button ID="btnBackToCheckOutForm" runat="server" TabIndex="11" Text="<< Back to CheckOut Form"
                                    CssClass="btn btn-primary btn-sm" OnClick="btnBackToCheckOutForm_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Service Details Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="GuestType" class="control-label col-md-2">
                                Guest Type</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSGuestType" runat="server" CssClass="form-control" TabIndex="5">
                                    <asp:ListItem Value="0">--- All ---</asp:ListItem>
                                    <asp:ListItem Value="InHouseGuest">In-House Guest</asp:ListItem>
                                    <asp:ListItem Value="OutSideGuest">Walk-In Guest</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="FromDate" class="control-label col-md-2">
                                From Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <label for="ToDate" class="control-label col-md-2">
                                To Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSToDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div id="InhouseSearchBillPanel" style="display: none;">
                            <div class="form-group">
                                <label for="RoomNumber" class="control-label col-md-2">
                                    Room Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSRoomNumber" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <label for="BillNumber" class="control-label col-md-2">
                                    Bill Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSBillNumber" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="ServiceName" class="control-label col-md-2">
                                    Service Name</label>
                                <div class="col-md-4">
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlInServiceName">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="OutSideSearchBillPanel" class="form-group" style="display: none;">
                            <label for="BillNumber" class="control-label col-md-2">
                                Bill Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSOutBillNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <label for="ServiceName" class="control-label col-md-2">
                                Service Name</label>
                            <div class="col-md-4">
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlSOutServiceName">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearchServiceBill" runat="server" Text="Search" CssClass="btn btn-primary btn-sm"
                                    TabIndex="8" OnClick="btnSearchServiceBill_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group" id="GridInformation">
                    <asp:GridView ID="gvGHServiceBill" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="50"
                        TabIndex="9" OnPageIndexChanging="gvGHServiceBill_PageIndexChanging" OnRowDataBound="gvGHServiceBill_RowDataBound"
                        OnRowCommand="gvGHServiceBill_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ApprovedStatus" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BillEdditableStatus" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblBillEdditableStatus" runat="server" Text='<%#Eval("BillEdditableStatus")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ServiceDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="GuestName" HeaderText="Guest Name" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Reference" HeaderText="Reference" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BillNumber" HeaderText="Bill No." ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ServiceName" HeaderText="Service" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ServiceRate" HeaderText="Rate" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ServiceQuantity" HeaderText="Qty" ItemStyle-Width="5%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        ImageUrl="~/Images/delete.png" Text="" AlternateText="Delete" ToolTip="Delete" />
                                    &nbsp;<asp:ImageButton ID="ImgPreview" runat="server" CausesValidation="False" CommandArgument='<%# bind("ServiceBillId") %>'
                                        CommandName="CmdPreview" ImageUrl="~/Images/ReportDocument.png" Text="" AlternateText="Preview"
                                        ToolTip="Preview" />
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
    <script type="text/javascript">
        var y = '<%=isOutSideGuestInfoEnable%>';
        if (y > -1) { InhousePanelVisibleFalse(); } else { InhousePanelVisibleTrue(); }
        var z = '<%=isGridInfoEnable%>'; if (z > -1) { $('#GridInformation').show(); } else
        { $('#GridInformation').show(); }

        var single = '<%=isSingle%>';
        //if (single == "True") {
        //    $('#CompanyProjectPanel').hide();
        //}
        //else {
        //    $('#CompanyProjectPanel').show();
        //}

        var xIsCompanyProjectPanelEnable = '<%=isCompanyProjectPanelEnable%>';
        if (xIsCompanyProjectPanelEnable > -1) {
            CompanyProjectPanelShow();
        }
        else {
            CompanyProjectPanelHide();
        }

        var IsPaidServicePanelVisible = '<%=IsPaidServicePanelVisible %>';
        if (IsPaidServicePanelVisible > -1) {
            $("#ddlPaidServiceInformationDiv").show();
            $("#ddlServiceInformationDiv").hide();
            $("#WithoutPaidServiceInfoDiv").hide();
        }
        else {
            $("#WithoutPaidServiceInfoDiv").show();
            $("#ddlPaidServiceInformationDiv").hide();
            $("#ddlServiceInformationDiv").show();
        }

        if ($("#<%=hfIsServiceBillWithoutInHouseGuest.ClientID %>").val() != "1") {
            InhousePanelVisibleTrue()
        }
        else {
            InhousePanelVisibleFalse();
        }

        var IsServiceChargeEnableConfig = '<%=IsServiceChargeEnableConfig%>';
        if (IsServiceChargeEnableConfig == 0) {
            $('#ServiceChargeLabel').hide();
            $('#ServiceChargeControl').hide();
        }
        else {
            $('#ServiceChargeLabel').show();
            $('#ServiceChargeControl').show();
        }

        var IsCitySDChargeEnableConfig = '<%=IsCitySDChargeEnableConfig%>';
        if (IsCitySDChargeEnableConfig == 0) {
            $('#CityChargeLabel').hide();
            $('#CityChargeControl').hide();
        }
        else {
            $('#CityChargeLabel').show();
            $('#CityChargeControl').show();
        }

        var IsVatEnableConfig = '<%=IsVatEnableConfig%>';
        if (IsVatEnableConfig == 0) {
            $('#VatAmountLabel').hide();
            $('#VatAmountControl').hide();
        }
        else {
            $('#VatAmountLabel').show();
            $('#VatAmountControl').show();
        }

        var IsAdditionalChargeEnableConfig = '<%=IsAdditionalChargeEnableConfig%>';
        if (IsAdditionalChargeEnableConfig == 0) {
            $('#AdditionalChargeLabel').hide();
            $('#AdditionalChargeControl').hide();
        }
        else {
            $('#AdditionalChargeLabel').show();
            $('#AdditionalChargeControl').show();
        }
    </script>
</asp:Content>
