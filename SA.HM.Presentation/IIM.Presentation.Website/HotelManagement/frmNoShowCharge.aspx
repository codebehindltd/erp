<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmNoShowCharge.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmNoShowCharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var PaymentDetails = new Array();
        var DeletedPaymentRowId = new Array();
        var DeletedPaymentId = new Array();
        var RowId = 0;
        var totalAmount = 0, v = 0;
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>No-Show Charge</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            CommonHelper.ApplyDecimalValidation();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_btnSave').attr('disabled', true);
            $('#ContentPlaceHolder1_txtReservationDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtCompanyBank", "ContentPlaceHolder1_ddlCompanyBank", "ContentPlaceHolder1_ddlCompanyBank");

            var reservationId = $("#<%=ddlReservationNo.ClientID %>").val();
            if (reservationId != 0) {
                PageMethods.LoadNoShowChargedReservations(reservationId, OnLoadChargedSucceeded, OnLoadChargedFailed);
                LoadNoShowHoldReservations(reservationId);
                $("#SaveDiv").show();
                $("#FinalSaveDiv").show();
            }

            var ddlReservationType = '<%=ddlReservationType.ClientID%>'
            $("#" + ddlReservationType).change(function () {
                var reservationType = $("#<%=ddlReservationType.ClientID %>").val();
                if (reservationType == 2) {
                    $("#<%=ddlReservationNo.ClientID %>").html("");
                    $("#NoShowChargedDiv").hide();
                    $("#NoShowHoldDiv").hide();
                }
                else {
                    $("#<%=ddlReservationNo.ClientID %>").html("");
                    $("#NoShowChargedDiv").hide();
                    $("#NoShowHoldDiv").hide();
                }
                return false;
            });

            var ddlReservationNo = '<%=ddlReservationNo.ClientID%>'
            $("#" + ddlReservationNo).change(function () {
                var reservationId = $("#<%=ddlReservationNo.ClientID %>").val();
                if (reservationId != 0) {
                    PageMethods.LoadNoShowChargedReservations(reservationId, OnLoadChargedSucceeded, OnLoadChargedFailed);
                    LoadNoShowHoldReservations(reservationId);
                    LoadBillPayment(reservationId)
                    $("#SaveDiv").show();
                    $("#FinalSaveDiv").show();
                }
                else {
                    $("#NoShowChargedDiv").hide();
                    $("#NoShowHoldDiv").hide();
                    $("#SaveDiv").hide();
                    $("#FinalSaveDiv").hide();
                }
                return false;
            });

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            $('#' + ddlPayMode).change(function () {
                PaymentModeShowHideInformation();
            });

            CurrencyRateInfoEnable();

            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtLedgerAmount = '<%=txtLedgerAmount.ClientID%>'
            var txtCalculatedLedgerAmount = '<%=txtCalculatedLedgerAmount.ClientID%>'

            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currencyType == "Local") {
                $('#' + txtConversionRate).val("");
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
                    $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                    $("#<%=hfConversionRate.ClientID %>").val(result.ConversionRate);
                    $("#<%=txtConversionRate.ClientID %>").attr("disabled", true);
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
        });
        function CalCulateTotalAmount() {
            var rate = 0, currentRate = 0;
            var localCurrencyName = $("#<%=hfLocalCurrencyName.ClientID %>").val();
            $("#NoShowChargedTable tbody tr").each(function () {
                v = v + parseFloat($.trim($(this).find("td:eq(4)").find("input").val()));
                rate = parseFloat($.trim($(this).find("td:eq(5)").text()));
                currentRate = parseFloat($.trim($(this).find("td:eq(6)").text()));
            });
            $("#<%=hfLocalConversionRate.ClientID %>").val(currentRate);
            totalAmount = v;
            //var FormatedText = "Total Amount: " + Math.round(parseFloat(totalAmount));
            var FormatedText = "Total Amount: " + (parseFloat(totalAmount));
            if (rate != 0)
                FormatedText = "Total Amount" + "(" + localCurrencyName + ")" + ":" + Math.round(parseFloat(totalAmount) * parseFloat(rate));
            $('#TotalAmount').text(FormatedText);
            v = 0;
            $("#<%=hfTotalAmount.ClientID %>").val(totalAmount);

        }
        function OnLoadChargedSucceeded(result) {
            if (result != "") {
                $("#NoShowChargedDiv").show();
                $("#NoShowCharged").html(result);
                CommonHelper.ApplyDecimalValidation();
            }
            CalCulateTotalAmount();
        }
        function OnLoadChargedFailed() {
            toastr.error();
        }
        function LoadNoShowHoldReservations(reservationId) {
            PageMethods.LoadNoShowHoldReservations(reservationId, OnLoadHoldSucceeded, OnLoadHoldFailed);
        }
        function OnLoadHoldSucceeded(result) {
            if (result != "") {
                $("#NoShowHoldDiv").show();
                $("#NoShowHold").html(result);
            }
        }
        function OnLoadHoldFailed() {
        }

        function LoadBillPayment(reservationId) {
            PageMethods.LoadBillPayment(reservationId, OnLoadBillPaymentSucceeded, OnLoadBillPaymentFailed);
        }
        function OnLoadBillPaymentSucceeded(result) {
            $("#gvBillPayment tbody tr").remove();
            var fieldId, convertionRate;
            if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                fieldId = $("#<%=hfLocalCurrencyId.ClientID %>").val();
                convertionRate = 1;
            }
            else {
                fieldId = $("#<%=ddlCurrency.ClientID %>").val();
                convertionRate = $("#<%=hfConversionRate.ClientID %>").val();
            }
            if (result.length > 0) {
                $("#ContentPlaceHolder1_PaymentDiv").hide();
                for (var i = 0; i < result.length; i++) {
                    RowId = RowId + 1;
                    $("#BillPayment").show();
                    var tr = "", totalRow = 0;
                    totalRow = $("#gvBillPayment tbody tr").length;

                    if ((totalRow % 2) == 0) {
                        tr += "<tr style=\"background-color:#E3EAEB;\">";
                    }
                    else {
                        tr += "<tr style=\"background-color:#FFFFFF;\">";
                    }

                    tr += "<td align='right' style=\"display:none;\">" + RowId + "</td>";
                    tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + result[i].PaymentMode + "</td>";
                    tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + result[i].CurrencyType + "</td>";
                    tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + result[i].PaymentAmount + "</td>";
                    tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + (result[i].PaymentAmount * parseFloat(convertionRate)) + "</td>";
                    tr += "<td align='right' style=\"display:none;\">" + result[i].PaymentId + "</td>";
                    tr += "<td align='left' style=\"width:25%; cursor:pointer;\">";
                    if (result[i].IsBillEditable)
                        tr += "<img src='../Images/delete.png' onClick= \"javascript:return DeletePayment($(this))\" alt='Edit Information' border='0' />";
                    tr += "</td>";
                    tr += "</tr>";

                    $("#gvBillPayment tbody ").append(tr);
                    tr = "";
                }
                CalCulatePaidTotal();
                $('#btnViewInvoice').show();
                //$('#ContentPlaceHolder1_btnSave').attr('disabled', true);
            }
            else {
                $("#ContentPlaceHolder1_PaymentDiv").show();
                var FormatedText = "";
                $('#PaidTotal2').text(FormatedText);
                $('#btnViewInvoice').hide();
            }
        }
        function OnLoadBillPaymentFailed() {
        }

        function ViewInvoice() {
            var paymentIdList = "";

            $("#gvBillPayment tbody tr").each(function () {
                var id = $(this).find("td:eq(5)").text();
                if (id != 0) {
                    if (paymentIdList == "") {
                        paymentIdList = id;
                    }
                    else {
                        paymentIdList += ',' + id;
                    }
                }
            });
            if (paymentIdList) {
                var popup_window = "Invoice Preview";
                var url = "/HotelManagement/Reports/frmReportNoShowPaymentInvoice.aspx?PaymentIdList=" + paymentIdList;
                window.open(url, popup_window, "width=800,height=680,left=300,top=50,resizable=yes");
            }
        }
        function PaymentModeShowHideInformation() {
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'

            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CashReceiveAccountsInfo').show();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CardPaymentAccountHeadDiv').show();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').show();
                $('#ChequeReceiveAccountsInfo').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#CashReceiveAccountsInfo').hide();
                $('#CardReceiveAccountsInfo').hide();
                $('#ChequeReceiveAccountsInfo').show();
                $('#CompanyPaymentAccountHeadDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#CashReceiveAccountsInfo').show();
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
        //Integrated Gl Visible True/False-------------------
        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#<%=hfCurrencyType.ClientID %>").val(result.CurrencyType);
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

            $("#<%=ddlCurrency.ClientID %>").val(result.FieldId);
            $("#<%=txtLedgerAmount.ClientID %>").val(result.CurrencyAmount);
            $("#<%=ddlPaymentType.ClientID %>").val(result.PaymentType);
            $("#<%=txtLedgerAmount.ClientID %>").val(result.CurrencyAmount);
            $("#<%=txtPaymentId.ClientID %>").val(result.PaymentId);
            $("#<%=txtDealId.ClientID %>").val(result.DealId);
            $("#<%=ddlBankId.ClientID %>").val(result.BankId);
            $('#txtBankId').val($("#<%=ddlBankId.ClientID %> option:selected").text());
            $('#txtCompanyBank').val($("#<%=ddlCompanyBank.ClientID %> option:selected").text());
            $("#<%=txtCardNumber.ClientID %>").val(result.CardNumber);
            $("#<%=ddlCardType.ClientID %>").val(result.CardType);
            $("#<%=txtCardHolderName.ClientID %>").val(result.CardHolderName);
            $("#<%=txtExpireDate.ClientID %>").val(GetStringFromDateTime(result.ExpireDate));
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewBIll').hide();
            $('#EntryPanel').show();
            $("#<%=ddlPayMode.ClientID %>").val(result.PaymentMode)
            $("#<%=txtCardNumber.ClientID %>").val(result.CardNumber)
            $("#<%=txtChecqueNumber.ClientID %>").val(result.ChecqueNumber)

            if (result.PaymentMode == "Cash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }
            else if (result.PaymentMode == "Card") {
                $('#CardPaymentAccountHeadDiv').show();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                IntegratedGeneralLedgerDivPanelHide();
            }
            else if (result.PaymentMode == "Cheque") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
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

        function CheckNoShowCharge() {
            var saveObj = new Array();
            var detailId = 0, noshowCharge = 0;
            $("#NoShowChargedTable tbody tr").each(function () {
                detailId = $.trim($(this).find("td:eq(0)").text());
                noshowCharge = $.trim($(this).find("td:eq(4)").find("input").val());

                saveObj.push({
                    ReservationDetailId: detailId,
                    NoShowCharge: noshowCharge
                });
            });
            $("#NoShowHoldTable tbody tr").each(function () {
                detailId = $.trim($(this).find("td:eq(0)").text());
                noshowCharge = $.trim($(this).find("td:eq(4)").find("input").val());

                saveObj.push({
                    ReservationDetailId: detailId,
                    NoShowCharge: noshowCharge
                });
            });

            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));
            $("#<%=hfPaymentSaveObj.ClientID %>").val(JSON.stringify(PaymentDetails));
            $("#<%=hfDeletedPaymentRowId.ClientID %>").val(JSON.stringify(DeletedPaymentRowId));
            $("#<%=hfDeletedPaymentId.ClientID %>").val(JSON.stringify(DeletedPaymentId));
            return true;
        }

        function SavePayment() {
            if ($("#<%=txtLedgerAmount.ClientID %>").val() == "") {
                $("#<%=txtLedgerAmount.ClientID %>").focus();
                toastr.warning("Please Enter Payment Amount.");
                return false;
            }

            var fieldId = "", convertionRate = "", currencyAmount = "", paymentAmount = "", accountsPostingHeadId = "", checqueNumber = "", cardReference = "", cardNumber = "", branchName = "", paymentDescription = "", cardType = "", expireDate = "", cardHolderName = "", nodeId = 0;

            var regId = $("#<%=ddlReservationNo.ClientID %>").val();
            var paymentType = $("#<%=ddlPaymentType.ClientID %>").val();
            var paymentMode = $("#<%=ddlPayMode.ClientID %>").val();
            var currencyName = $("#<%=ddlCurrency.ClientID %>").find('option:selected').text();
            if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                fieldId = $("#<%=hfLocalCurrencyId.ClientID %>").val();
                convertionRate = 1;
                currencyAmount = $("#<%=txtLedgerAmount.ClientID %>").val();
            }
            else {
                fieldId = $("#<%=ddlCurrency.ClientID %>").val();
                convertionRate = $("#<%=hfConversionRate.ClientID %>").val();
                currencyAmount = $("#<%=txtLedgerAmount.ClientID %>").val();
            }
            var bankId = $("#<%=ddlBankId.ClientID %>").val();
            var serviceBillId = null;
            if (paymentMode == "Cash") {
                accountsPostingHeadId = $("#<%=ddlCashReceiveAccountsInfo.ClientID %>").val();
                checqueNumber = "";
                cardReference = "";
                cardNumber = "";
                branchName = "";
                paymentDescription = "";
            }
            else if (paymentMode == "Card") {
                accountsPostingHeadId = $("#<%=ddlCardReceiveAccountsInfo.ClientID %>").val();
                cardType = $("#<%=ddlCardType.ClientID %>").val();
                cardNumber = $("#<%=txtCardNumber.ClientID %>").val();
                expireDate = $("#<%=txtExpireDate.ClientID %>").val();
                cardHolderName = $("#<%=txtCardHolderName.ClientID %>").val();
                checqueNumber = "";
                paymentDescription = $("#<%=ddlCardType.ClientID %> option:selected").text();
            }
            else if (paymentMode == "Cheque") {
                var ddlPaidByChequeCompanyText = $("#<%=ddlChecquePaymentAccountHeadId.ClientID %> option:selected").text();
                accountsPostingHeadId = $("#<%=ddlChecquePaymentAccountHeadId.ClientID %>").val();
                bankId = $("#<%=ddlCompanyBank.ClientID %>").val();
                var bankName = $("#<%=ddlCompanyBank.ClientID %> option:selected").text();
                checqueNumber = $("#<%=txtChecqueNumber.ClientID %>").val();
                cardReference =
                    cardNumber = "";
                branchName = "";
                paymentDescription = "Company: " + ddlPaidByChequeCompanyText + ", Bank: " + bankName + ", Cheque: " + checqueNumber;
            }
            else if (paymentMode == "Company") {
                accountsPostingHeadId = $("#<%=ddlCompanyPaymentAccountHead.ClientID %>").val();
                nodeId = $("#<%=ddlCompanyPaymentAccountHead.ClientID %>").val();
                paymentDescription = "";
            }

            var moduleName = "Reservation";
            RowId = RowId + 1;

            PaymentDetails.push({
                PaymentId: RowId,
                RegistrationId: regId,
                PaymentType: paymentType,
                PaymentMode: paymentMode,
                FieldId: fieldId,
                ConvertionRate: convertionRate,
                CurrencyAmount: currencyAmount,
                BankId: bankId,
                ServiceBillId: serviceBillId,
                AccountsPostingHeadId: accountsPostingHeadId,
                ChecqueNumber: checqueNumber,
                CardReference: cardReference,
                CardNumber: cardNumber,
                BranchName: branchName,
                PaymentDescription: paymentDescription,
                CardType: cardType,
                CardHolderName: cardHolderName,
                BranchName: branchName,
                NodeId: nodeId,
                ModuleName: moduleName
            });

            $("#BillPayment").show();
            var tr = "", totalRow = 0;
            totalRow = $("#gvBillPayment tbody tr").length;

            var localCurrencyName = $("#<%=hfLocalCurrencyName.ClientID %>").val();

            $("#gvBillPayment thead tr td:eq(3)").text("Amount (" + localCurrencyName + ")");

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }

            tr += "<td align='right' style=\"display:none;\">" + RowId + "</td>";
            tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + paymentMode + "</td>";
            tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + currencyName + "</td>";
            tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + currencyAmount + "</td>";
            tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + parseFloat(currencyAmount) * parseFloat(convertionRate) + "</td>";
            tr += "<td align='right' style=\"display:none;\">" + 0 + "</td>";
            tr += "<td align='left' style=\"width:25%; cursor:pointer;\"><img src='../Images/delete.png' onClick= \"javascript:return DeleteRow($(this))\" alt='Edit Information' border='0' /></td>";
            tr += "</tr>";

            $("#gvBillPayment tbody ").append(tr);

            var paymentIdList = "";
            $("#gvBillPayment tbody tr").each(function () {
                var id = $(this).find("td:eq(5)").text();
                if (id != 0) {
                    if (paymentIdList == "") {
                        paymentIdList = id;
                    }
                    else {
                        paymentIdList += ',' + id;
                    }
                }
            });
            $("#<%=hfPaymentIdList.ClientID %>").val(paymentIdList);
            paymentIdList = "";
            tr = "";

            CalCulatePaidTotal();
            ClearDetails();
            return false;
        }
        function DeleteRow(row) {
            var id = $(row).parents('tr').find("td:eq(0)").text();
            DeletedPaymentRowId.push({
                PaymentId: id
            });
            $(row).parents('tr').remove();
            CalCulatePaidTotal();
        }
        function DeletePayment(row) {
            var paymentId = $(row).parents('tr').find("td:eq(5)").text();
            DeletedPaymentId.push({
                PaymentId: paymentId
            });
            $("#ContentPlaceHolder1_PaymentDiv").show();
            $(row).parents('tr').remove();
            CalCulatePaidTotal();
        }

        function CalCulatePaidTotal() {
            var a = 0, paidTotal = 0;
            var localCurrencyName = $("#<%=hfLocalCurrencyName.ClientID %>").val();
            $("#gvBillPayment tbody tr").each(function () {
                a = a + parseFloat($.trim($(this).find("td:eq(4)").text()));
            });
            //paidTotal = Math.round(a);
            paidTotal = a;
            var FormatedText = "Paid Amount (" + localCurrencyName + ") : " + paidTotal;
            $('#PaidTotal2').text(FormatedText);

            var chargedTotal = parseFloat($("#<%=hfTotalAmount.ClientID %>").val());
            var convertionRate = parseFloat($("#<%=hfLocalConversionRate.ClientID %>").val());
            if (convertionRate != 0) {
                chargedTotal = chargedTotal * convertionRate;
            }
            if ((chargedTotal) == paidTotal) {
                $('#ContentPlaceHolder1_btnSave').attr('disabled', false);
            }
            else {
                $('#ContentPlaceHolder1_btnSave').attr('disabled', true);
            }
        }

        function ClearDetails() {
            $("#<%=ddlPayMode.ClientID %>").val('Cash');
            $("#<%=ddlCardType.ClientID %>").val('0');
            $("#<%=txtLedgerAmount.ClientID %>").val('');
            $("#<%=ddlCurrency.ClientID %>").val('1');
            $("#<%=txtCalculatedLedgerAmount.ClientID %>").val('');
            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#txtBankId").val('');
            $("#<%=hfCurrencyType.ClientID %>").val("Local");
            $("#CardPaymentAccountHeadDiv").hide();
            $("#ConversionPanel").hide();
            $("#ChecquePaymentAccountHeadDiv").hide();
        }

        function myFunction(id) {
            var v = $(id).val();
            if (v == "1") {
                $(id).text("Show Charged Reservations");
                $(id).val("2");
                $("#ChargedReservations").hide();
            }
            else {
                $(id).text("Hide Charged Reservations");
                $(id).val("1");
                $("#ChargedReservations").show();
            };
        }
    </script>
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfConversionRate" runat="server" />
    <asp:HiddenField ID="txtPaymentId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSaveObj" runat="server" />
    <asp:HiddenField ID="hfLocalCurrencyId" runat="server" />
    <asp:HiddenField ID="hfLocalCurrencyName" runat="server" />
    <asp:HiddenField ID="hfPaymentSaveObj" runat="server" />
    <asp:HiddenField ID="hfDeletedPaymentRowId" runat="server" />
    <asp:HiddenField ID="hfTotalAmount" runat="server" />
    <asp:HiddenField ID="hfLocalConversionRate" runat="server" />
    <asp:HiddenField ID="hfIsSearch" runat="server" />
    <asp:HiddenField ID="hfDeletedPaymentId" runat="server" />
    <asp:HiddenField ID="hfPaymentIdList" runat="server" />
    <div id="SearchEntry" class="panel panel-default">
        <div class="panel-heading">
            No-Show Reservations
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label5" runat="server" class="control-label" Text="Search Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReservationType" runat="server" CssClass="form-control"
                            TabIndex="2">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="1">Charged Reservation</asp:ListItem>
                            <asp:ListItem Value="2">Settled Reservation</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="ReservationDateDivInfo">
                        <div class="col-md-1">
                            <asp:Label ID="lblDate" runat="server" class="control-label" Text="Date"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtReservationDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="Button1" runat="server" Text="Search" TabIndex="4" CssClass="btn btn-primary btn-sm"
                                OnClick="btnSearch_Click" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReservationNo" runat="server" class="control-label" Text="Reservation No"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReservationNo" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="NoShowChargedDiv" style="display: none; padding-top: 10px;" class="panel panel-default">
        <div class="panel-heading">
            No-Show Charge
        </div>
        <div id="NoShowCharged">
        </div>
        <div class="panel-body">
            <div id="PaymentDiv" runat="server">
                <div class="form-horizontal">
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
                                <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
                                    <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblPaymentType" runat="server" class="control-label" Text="Payment Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control" TabIndex="4">
                                <asp:ListItem Value="Payment">Payment</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblPayMode" runat="server" class="control-label" Text="Payment Mode"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                <asp:ListItem>Cash</asp:ListItem>
                                <asp:ListItem>Card</asp:ListItem>
                                <%--<asp:ListItem>Cheque</asp:ListItem>--%>
                                <asp:ListItem>Company</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCurrency" runat="server" class="control-label" Text="Currency Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCurrency" TabIndex="6" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                            <asp:Label ID="lblDisplayConvertionRate" runat="server" Text=""></asp:Label>
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
                    <div id="SaveDiv" class="row" style="display: none; padding-left: 7px;">
                        <div class="col-md-12">
                            <asp:Button ID="btnPaymentSave" runat="server" Text="Add" TabIndex="12" CssClass="btn btn-primary btn-sm"
                                OnClientClick="javascript: return SavePayment();" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default" style="margin-top: 10px;">
                <div class="panel-heading">
                    Bill Payment Details
                </div>
                <div id="BillPayment" style="display: none" class="panel-body">
                    <table id='gvBillPayment' class="table table-bordered table-condensed table-responsive">
                        <colgroup>
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Payment Mode
                                </td>
                                <td>Currency
                                </td>
                                <td>Amount
                                </td>
                                <td>Amount
                                </td>
                                <td>Action
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <input type="button" id="btnViewInvoice" value="View Invoice" onclick="ViewInvoice();" class="btn btn-primary btn-sm" />
                </div>
                <div>
                    <asp:GridView ID="gvGuestHouseService" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" OnPageIndexChanging="gvGuestHouseService_PageIndexChanging"
                        OnRowDataBound="gvGuestHouseService_RowDataBound" OnRowCommand="gvGuestHouseService_RowCommand"
                        PageSize="100" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("PaymentId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" ItemStyle-Width="15%">
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
                            <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CurrencyType" HeaderText="Currency" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PaymentAmount" HeaderText="Amount" ItemStyle-Width="15%">
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
                </div>
                <div id='PaidTotal' runat="server" class='totalAmout' style="padding-left: 10px; font-weight: bold;">
                </div>
                <div id='PaidTotal2' class='totalAmout' style="padding-left: 10px; font-weight: bold;">
                </div>
            </div>
        </div>
    </div>
    <div id="NoShowHoldDiv" style="display: none" class="panel panel-default">
        <div class="panel-heading">
            Holdup Charge Information
        </div>
        <div id="NoShowHold">
        </div>
    </div>
    <div id="FinalSaveDiv" class="row" style="display: none; padding-left: 7px;">
        <div class="col-md-12">
            <asp:Button ID="btnSave" runat="server" Text="Save Payment" TabIndex="12" CssClass="btn btn-primary btn-sm"
                OnClick="btnSave_Click" OnClientClick="javascript: return CheckNoShowCharge();" />
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

            var isPaymentSave = '<%=isPaymentSave%>';
            if (isPaymentSave > -1) {
                $("#NoShowChargedDiv").show();
                $("#NoShowHoldDiv").show();
            }

            var isSaveSuccessful = '<%=isSaveSuccessful%>';
            if (isSaveSuccessful == 1) {
                toastr.success("Saved Successfully");
                $("#NoShowChargedDiv").html("");
                $("#NoShowHoldDiv").html("");
                $("#NoShowChargedDiv").hide();
                $("#NoShowHoldDiv").hide();
                $("#FinalSaveDiv").hide();
                document.getElementById('NoShowChargedDiv').className = '';
                document.getElementById('NoShowHoldDiv').className = '';
            }
            else if (isSaveSuccessful == 2) {
                toastr.error("Save operation failed.");
            }

            var hfIsSearch = $("#<%=hfIsSearch.ClientID %>").val();
            if (hfIsSearch == 1) {
                $("#ChargedReservations").hide();
            }
            else {
                $("#ChargedReservations").hide();
            }
            $("#<%=hfIsSearch.ClientID %>").val(0);
        });
    </script>
</asp:Content>
