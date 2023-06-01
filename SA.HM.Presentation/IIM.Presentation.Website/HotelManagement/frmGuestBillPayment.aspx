<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGuestBillPayment.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmGuestBillPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(function () {
            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmount = '<%=txtCalculatedLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmountHiddenField = '<%=txtCalculatedLedgerAmountHiddenField.ClientID%>'
            var lblDisplayConvertionRate = '<%=lblDisplayConvertionRate.ClientID%>'

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

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
            CommonHelper.AutoSearchClientDataSource("txtMBankingBankId", "ContentPlaceHolder1_ddlMBankingBankId", "ContentPlaceHolder1_ddlMBankingBankId");

            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currencyType == "Local") {
                $('#' + txtConversionRate).val("")
                $('#' + txtCalculatedLedgerAmount).val("");
                $('#ConversionPanel').hide();
                $('#' + txtCalculatedLedgerAmount).attr("disabled", false);
            }
            else {
                $('#ConversionPanel').show();
                $('#' + txtCalculatedLedgerAmount).val("");
            }

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
                    $('#' + txtCalculatedLedgerAmount).val("");
                    $('#ConversionPanel').hide();
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", false);
                }
                else {
                    var registrationWiseConversionRate = 0;
                    if ($("#<%=txtConversionRateHiddenField.ClientID %>").val() == "") {
                        registrationWiseConversionRate = result.ConversionRate;
                    }
                    else if (parseFloat($("#<%=txtConversionRateHiddenField.ClientID %>").val()) == 0) {
                        registrationWiseConversionRate = result.ConversionRate;
                    }
                    else {
                        registrationWiseConversionRate = $("#<%=txtConversionRateHiddenField.ClientID %>").val();
                    }

                    if ($("#<%=ddlCurrency.ClientID %>").val() == "2") {
                        $("#<%=txtConversionRate.ClientID %>").val(registrationWiseConversionRate);
                    $("#<%=hfConversionRate.ClientID %>").val(registrationWiseConversionRate);
                }
                else {
                    $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                    $("#<%=hfConversionRate.ClientID %>").val(result.ConversionRate);
                    }

                    $('#ConversionPanel').show();
                    $('#' + txtCalculatedLedgerAmount).val("");
                }
                CurrencyRateInfoEnable();
            }

            function OnLoadConversionRateFailed() {
            }

            $('#' + txtLedgerAmount).blur(function () {
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

            $('#' + txtConversionRate).blur(function () {
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

            var ddlPaymentType = '<%=ddlPaymentType.ClientID%>'
            $('#' + ddlPaymentType).change(function () {
                if ($('#' + ddlPaymentType).val() == "PaidOut") {
                    $('#' + ddlPayMode).val('Cash');
                    $('#' + ddlCurrency).attr("disabled", true);
                    $('#' + ddlCurrency).val('1');
                    $('#' + ddlPayMode).attr("disabled", true);
                    $('#CashOutDivPanel').show();
                    $('#AdvanceDivPanel').hide();
                }
                else if ($('#' + ddlPaymentType).val() == "Refund") {
                    $('#' + ddlPayMode).val('Cash');
                    $('#' + ddlCurrency).attr("disabled", true);
                    $('#' + ddlCurrency).val('1');
                    $('#' + ddlPayMode).attr("disabled", true);
                    $('#CashOutDivPanel').show();
                    $('#AdvanceDivPanel').hide();
                }
                else {
                    $('#' + ddlPayMode).attr("disabled", false);
                    $('#' + ddlCurrency).attr("disabled", false);
                    $('#CashOutDivPanel').hide();
                    $('#AdvanceDivPanel').show();
                }
                PaymentModeShowHideInformation();
            });

            $('#' + ddlPayMode).change(function () {
                PaymentModeShowHideInformation();
            });

            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#ContentPlaceHolder1_btnSave").hide();
        });

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Guest Bill Payment</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            //EnableDisable For DropDown Change event--------------

            $('#ContentPlaceHolder1_txtExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').show();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "M-Banking") {
                $('#MBankingPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }

            <%--ddlGLProject = '<%=ddlGLProject.ClientID%>'
            var ddlGLCompany = '<%=ddlGLCompany.ClientID%>'
            var selectedIndex = parseFloat($('#' + ddlGLCompany).prop("selectedIndex"));
            if (selectedIndex > 0) {
                $("#" + ddlGLProject).attr("disabled", false);
            }
            else {
                $("#" + ddlGLProject).attr("disabled", true);
            }--%>

            var txtCardNumber = '<%=txtCardNumber.ClientID%>'
            $('#' + txtCardNumber).blur(function () {
            });
            var ddlCardType = '<%=ddlCardType.ClientID%>'
            $('#' + ddlCardType).change(function () {
            });
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

            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').show();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "M-Banking") {
                $('#MBankingPaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
        }
        <%--function PopulateProjects() {
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
        }--%>

       <%-- function OnProjectsPopulated(response) {
            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            $("#" + ddlGLProject).attr("disabled", false);

            PopulateControl(response.d, $("#<%=ddlGLProject.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }--%>
        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#<%=hfCurrencyType.ClientID %>").val(result.CurrencyType);
            MessagePanelHide();
            if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                $('#ConversionPanel').show();
                $("#<%=txtCalculatedLedgerAmount.ClientID %>").val(result.PaymentAmount);
                $("#<%=txtCalculatedLedgerAmount.ClientID %>").attr("disabled", true);
                $("#<%=txtConversionRate.ClientID %>").val(result.PaymentAmount / result.CurrencyAmount);
            }
            else {
                $('#ConversionPanel').hide();
                $("#<%=txtCalculatedLedgerAmount.ClientID %>").attr("disabled", false);
                $("#<%=txtConversionRate.ClientID %>").val('');
            }

            $('#ChangedRoomInfoDiv').show();
            $("#<%=ddlCurrency.ClientID %>").val(result.FieldId);
            $("#<%=txtLedgerAmount.ClientID %>").val(result.CurrencyAmount);
            $("#<%=ddlPaymentType.ClientID %>").val(result.PaymentType);
            $("#<%=txtLedgerAmount.ClientID %>").val(result.CurrencyAmount);
            $("#<%=ddlRegistrationId.ClientID %>").val(result.RegistrationId);
            $("#<%=txtPaymentId.ClientID %>").val(result.PaymentId);
            $("#<%=txtDealId.ClientID %>").val(result.DealId);
            $("#<%=ddlBankId.ClientID %>").val(result.BankId);

            $('#txtBankId').val($("#<%=ddlBankId.ClientID %> option:selected").text());
            $('#txtCompanyBank').val($("#<%=ddlCompanyBank.ClientID %> option:selected").text());
            $('#txtMBankingBankId').val($("#<%=ddlMBankingBankId.ClientID %> option:selected").text());

            $("#<%=txtCardNumber.ClientID %>").val(result.CardNumber);
            $("#<%=ddlCardType.ClientID %>").val(result.CardType);
            $("#<%=txtCardHolderName.ClientID %>").val(result.CardHolderName);
            $("#<%=txtExpireDate.ClientID %>").val(GetStringFromDateTime(result.ExpireDate));
            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);

            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "0")
                $("#ContentPlaceHolder1_btnSave").hide();
            else
                $("#ContentPlaceHolder1_btnSave").val("Update").show();
            $('#btnNewBIll').hide();
            $('#EntryPanel').show();
            $("#<%=ddlPayMode.ClientID %>").val(result.PaymentMode)
            $("#<%=txtCardNumber.ClientID %>").val(result.CardNumber)
            $("#<%=txtChecqueNumber.ClientID %>").val(result.ChecqueNumber)

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            var ddlPaymentType = '<%=ddlPaymentType.ClientID%>'
            if ($('#' + ddlPaymentType).val() == "PaidOut") {
                $('#' + ddlPayMode).val('Cash');
                $("#<%=ddlCurrency.ClientID %>").attr("disabled", true);
                //$('#' + ddlCurrency).val('1');
                $('#' + ddlPayMode).attr("disabled", true);
                $('#CashOutDivPanel').show();
                $('#AdvanceDivPanel').hide();
            }
            else if ($('#' + ddlPaymentType).val() == "Refund") {
                $('#' + ddlPayMode).val('Cash');
                $('#' + ddlCurrency).attr("disabled", true);
                //$('#' + ddlCurrency).val('1');
                $('#' + ddlPayMode).attr("disabled", true);
                $('#CashOutDivPanel').show();
                $('#AdvanceDivPanel').hide();
            }
            else {
                $('#' + ddlPayMode).attr("disabled", false);
                $("#<%=ddlCurrency.ClientID %>").attr("disabled", false);
                $('#CashOutDivPanel').hide();
                $('#AdvanceDivPanel').show();
            }

            if (result.PaymentMode == "Cash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }
            else if (result.PaymentMode == "Card") {
                $('#CardPaymentAccountHeadDiv').show();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }
            else if (result.PaymentMode == "Cheque") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }

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
        //For ClearForm-------------------------
        function PerformClearAction() {
            //$("#frmHotelManagement")[0].reset();
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtLedgerAmount.ClientID %>").val('');
            //$("#<%=ddlRegistrationId.ClientID %>").val(0);
            $("#<%=txtPaymentId.ClientID %>").val('');
            $("#<%=txtDealId.ClientID %>").val('');
            $("#<%=ddlCurrency.ClientID %>").val('');
            $("#<%=ddlPaymentType.ClientID %>").val('0');
            $("#<%=txtCalculatedLedgerAmount.ClientID %>").val('');
            $("#<%=txtConversionRate.ClientID %>").val('');
            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');
            $("#<%=txtExpireDate.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            $('#ConversionPanel').hide();
            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#ContentPlaceHolder1_btnSave").hide();
            else
                $("#ContentPlaceHolder1_btnSave").val("Save").show();
            MessagePanelHide();

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            var ddlPaymentType = '<%=ddlPaymentType.ClientID%>'
            if ($('#' + ddlPaymentType).val() == "PaidOut") {
                $('#' + ddlPayMode).val('Cash');
                $("#<%=ddlCurrency.ClientID %>").attr("disabled", true);
                //$('#' + ddlCurrency).val('1');
                $('#' + ddlPayMode).attr("disabled", true);
                $('#CashOutDivPanel').show();
                $('#AdvanceDivPanel').hide();
            }
            else if ($('#' + ddlPaymentType).val() == "Refund") {
                $('#' + ddlPayMode).val('Cash');
                $('#' + ddlCurrency).attr("disabled", true);
                //$('#' + ddlCurrency).val('1');
                $('#' + ddlPayMode).attr("disabled", true);
                $('#CashOutDivPanel').show();
                $('#AdvanceDivPanel').hide();
            }
            else {
                $('#' + ddlPayMode).attr("disabled", false);
                $("#<%=ddlCurrency.ClientID %>").attr("disabled", false);
                $('#CashOutDivPanel').hide();
                $('#AdvanceDivPanel').show();
            }
            PaymentModeShowHideInformation();

            return false;
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
            //var isCardValid = validateCard();
            var isDateValid = true; // ValidateExpireDate();
            if (isCardValid != true) {
                return false;
            }
            else if (isDateValid != true) {
                var lblMessage = '<%=lblMessage.ClientID%>'
                $('#' + lblMessage).text("Please fill the Expiry Date");
                MessagePanelShow();
                return false;
            }
            else {
                return true;
            }
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
            $("#DivPaymentSelect").dialog({
                width: 600,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", ////TODO add title
                show: 'slide'
            });
            return false;
        }

        function PaymentPreview() {
            var paymentIdList = "";
            $("#ContentPlaceHolder1_gvPaymentInfo tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                    var id = $(this).find("td:eq(1)").text();
                    if (paymentIdList == "") {
                        paymentIdList = id;
                    }
                    else {
                        paymentIdList += ',' + id;
                    }
                }
            });
            if (paymentIdList != "") {
                
                var url = "/HotelManagement/Reports/frmReportGuestPaymentInvoice.aspx?PaymentIdList=" + paymentIdList;
                var popup_window = "Preview";
                window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            }
            else {
                toastr.warning('Select payments to preview');
            }
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
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
        <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="HiddenField2" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="HiddenField3" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtPaymentId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <asp:HiddenField ID="hfIsUpdatePermission" runat="server" />
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
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("PaymentId") %>'></asp:Label>
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
                        <asp:BoundField DataField="PaymentAmount" HeaderText="Payment Amount" ItemStyle-Width="20%">
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
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnPaymentPreview" runat="server" Text="Preview" TabIndex="3" CssClass="btn btn-primary btn-sm"
                            OnClientClick="javascript: return PaymentPreview();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="btnNewBIll" class="btn-toolbar" style="display: none;">
        <button onclick="javascript: return EntryPanelVisibleTrue();" class="btn btn-primary">
            <i class="icon-plus"></i>New Payment</button>
        <div class="btn-group">
        </div>
    </div>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Guest Bill Payment Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group" style="display: none;">
                    <label for="RoomNumber" class="control-label col-md-2">
                        Room Number</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <label for="PaymentDate" class="control-label col-md-2">
                        Payment Date</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div id="CompanyPaymentAccountHeadDiv" class="col-md-4" style="display: none;">
                        <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label for="RoomNumber" class="control-label col-md-2 required-field">
                        Room Number</label>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2" style="text-align: left; padding-left: 0;">
                        <asp:Button ID="btnSrcRoomNumber" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSrcRoomNumber_Click" />
                    </div>
                    <div id="lblRegistrationNumberDiv" runat="server">
                        <label for="RegistrationNumber" class="control-label col-md-2">
                            Registration Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRegistrationId" runat="server" CssClass="form-control" TabIndex="3"
                            Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label for="PaymentType" class="control-label col-md-2 required-field">
                        Payment Type</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control" TabIndex="4">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="Payment">Payment</asp:ListItem>
                            <asp:ListItem Value="PaidOut">Paid Out</asp:ListItem>
                            <asp:ListItem Value="Refund">Refund</asp:ListItem>
                            <asp:ListItem Value="Rebate">Rebate</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <label for="PaymentMode" class="control-label col-md-2 required-field">
                        Payment Mode</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="Cash">Cash</asp:ListItem>
                            <asp:ListItem Value="Card">Card</asp:ListItem>
                            <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                            <asp:ListItem Value="Company">Company</asp:ListItem>
                            <asp:ListItem Value="M-Banking">M-Banking</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="IntegratedGeneralLedgerDiv" style="display: none;">
                    <div id="AdvanceDivPanel" style="display: none;">
                        <div class="form-group">
                            <label for="PaymentReceiveIn" class="control-label col-md-2">
                                Payment Receive In</label>
                            <div class="col-md-4">
                                <div id="CashReceiveAccountsInfo">
                                    <asp:DropDownList ID="ddlCashReceiveAccountsInfo" runat="server" TabIndex="6">
                                    </asp:DropDownList>
                                </div>
                                <div id="CardReceiveAccountsInfo" style="display: none;">
                                    <asp:DropDownList ID="ddlCardReceiveAccountsInfo" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div id="ChequeReceiveAccountsInfo" style="display: none;">
                                    <asp:DropDownList ID="ddlChequeReceiveAccountsInfo" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div id="MBankingReceiveAccountsInfo" style="display: none;">
                                    <asp:DropDownList ID="ddlMBankingReceiveAccountsInfo" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <label for="IncomePurpose" class="control-label col-md-2">
                                Income Purpose</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIncomeSourceAccountsInfo" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="CashOutDivPanel" style="display: none;">
                        <div class="form-group" style="display: none;">
                            <label for="PaymentFrom" class="control-label col-md-2">
                                Payment From</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPaymentFromAccountsInfo" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <label for="PaymentTo" class="control-label col-md-2">
                                Payment To</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPaymentToAccountsInfo" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <%--<div class="form-group" id="CompanyProjectPanel" style="display: none;">
                        <label for="Company" class="control-label col-md-2">
                            Company</label>
                        <div class="col-md-4">
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
                <div class="form-group">
                    <label for="CurrencyType" class="control-label col-md-2 required-field">
                        Currency Type</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCurrency" TabIndex="6" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:Label ID="lblDisplayConvertionRate" runat="server" Text=""></asp:Label>
                        <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                    </div>
                    <label for="PaymentAmount" class="control-label col-md-2 required-field">
                        Payment Amount</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtLedgerAmount" runat="server" CssClass="form-control quantitydecimal" TabIndex="7"> </asp:TextBox>
                    </div>
                </div>
                <div id="ConversionPanel" class="form-group">
                    <label for="ConversionRate" class="control-label col-md-2">
                        Conversion Rate</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:HiddenField ID="txtConversionRateHiddenField" runat="server"></asp:HiddenField>
                    </div>
                    <label for="CalculatedAmount" class="control-label col-md-2">
                        Calculated Amount</label>
                    <div class="col-md-4">
                        <asp:HiddenField ID="txtCalculatedLedgerAmountHiddenField" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtCalculatedLedgerAmount" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                    <div class="form-group" style="display: none;">
                        <label for="CompanyName" class="control-label col-md-2">
                            Company Name</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control"
                                TabIndex="6">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="ChequeNumber" class="control-label col-md-2 required-field">
                            Cheque Number</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="BankName" class="control-label col-md-2 required-field">
                            Bank Name</label>
                        <div class="col-md-10">
                            <input id="txtCompanyBank" type="text" class="form-control" />
                            <div style="display: none;">
                                <asp:DropDownList ID="ddlCompanyBank" runat="server" CssClass="form-control" AutoPostBack="false">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="CardPaymentAccountHeadDiv" style="display: none;">
                    <div class="form-group">
                        <label for="CardType" class="control-label col-md-2 required-field">
                            Card Type</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCardType" runat="server" TabIndex="8" CssClass="form-control">
                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem Value="a">American Express</asp:ListItem>
                                <asp:ListItem Value="m">Master Card</asp:ListItem>
                                <asp:ListItem Value="v">Visa Card</asp:ListItem>
                                <asp:ListItem Value="d">Discover Card</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <label for="CardNumber" class="control-label col-md-2">
                            Card Number</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="BankName" class="control-label col-md-2 required-field">
                            Bank Name</label>
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
                </div>
                <div id="MBankingPaymentAccountHeadDiv" style="display: none;">
                    <div class="form-group">
                        <label for="BankName" class="control-label col-md-2 required-field">
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
                <div class="form-group">
                    <label for="Remarks" class="control-label col-md-2">
                        Remarks</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                            TabIndex="11"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="ChangedRoomInfoDiv" style="display: none;">
                    <label for="ChangedRoom" class="control-label col-md-2">
                        Room Number</label>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlChangedRegistrationId" runat="server" CssClass="form-control" AutoPostBack="false">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="12" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSave_Click" OnClientClick="javascript: return ValidateForm();" />
                        <%--<asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="13" CssClass="btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" />--%>
                        <input type="button" tabindex="18" id="btnClear" value="Clear" class="btn btn-primary btn-sm"
                                    onclick="javascript: return PerformClearAction()" />
                        <asp:Button ID="btnCancel" runat="server" TabIndex="14" Text="Close" CssClass="btn btn-primary btn-sm"
                            OnClientClick="javascript: return EntryPanelVisibleFalse();" Visible="False" />
                        <asp:Button ID="btnGroupPaymentPreview" runat="server" Text="Payment Preview" TabIndex="15"
                            CssClass="btn btn-primary btn-sm" OnClientClick="javascript: return LoadPopUp();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Bill Payment Details
        </div>
        <div class="panel-body">
            <asp:GridView ID="gvGuestHouseService" Width="100%" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                ForeColor="#333333" OnPageIndexChanging="gvGuestHouseService_PageIndexChanging"
                OnRowDataBound="gvGuestHouseService_RowDataBound" OnRowCommand="gvGuestHouseService_RowCommand"
                PageSize="100" CssClass="table table-bordered table-condensed table-responsive">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IsBillEditable" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblIsBillEditable" runat="server" Text='<%#Eval("IsBillEditable")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("PaymentId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblPLI_DATE" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("PaymentDate"))) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="BillNumber" HeaderText="Invoice No." ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CurrencyType" HeaderText="Currency" ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CurrencyAmount" HeaderText="Amount" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CreatedByName" HeaderText="Received By" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                ImageUrl="~/Images/delete.png" Text="" AlternateText="Delete" ToolTip="Delete" />
                            &nbsp;<asp:ImageButton ID="ImgPaymentPreview" runat="server" CausesValidation="False"
                                CommandArgument='<%# bind("PaymentId") %>' CommandName="CmdPaymentPreview" ImageUrl="~/Images/ReportDocument.png"
                                Text="" AlternateText="Payment Preview" ToolTip="Payment Preview" />
                        </ItemTemplate>
                        <ControlStyle Font-Size="Small" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
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

            var x = '<%=isSearchPanelEnable%>';
            if (x > -1) {
                $('#SearchPanel').show();

            }
            else {
                $('#SearchPanel').hide();
            }

            var single = '<%=isSingle%>';
            //if (single == "True") {
            //    //$('#CompanyProjectPanel').hide();
            //}
            //else {
            //    $('#CompanyProjectPanel').show();
            //}
        });
    </script>
</asp:Content>
