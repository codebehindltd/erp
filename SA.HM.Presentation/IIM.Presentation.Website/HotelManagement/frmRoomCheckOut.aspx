<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmRoomCheckOut.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmRoomCheckOut" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .mycheckbox input[type="checkbox"] {
            margin-right: 10px;
            padding-top: 5px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
            }

            $("#CheckOutDetailsDiv").show();
            $("#RebateInformationDiv").show();
            $("#PaymentDetailsInformation").show();
            $("#TransactionalButtonDiv").show();
            $("#BillPreviewWithoutRebateDiv").show();
            $("#BillPreviewWithRebateDiv").hide();
            $("#GrandTotalPaymentDetailsDiv").hide();
            $('#btnBillPreviewAndBillLock').hide();
            var paramCheckOutType = CommonHelper.GetParameterByName("cot");
            if (paramCheckOutType != "") {
                if (paramCheckOutType == "rlp") {
                    $("#CheckOutDetailsDiv").show();
                    $("#RebateInformationDiv").show();
                    $("#PaymentDetailsInformation").hide();
                    $("#TransactionalButtonDiv").hide();
                    $("#BillPreviewWithoutRebateDiv").hide();
                    $("#BillPreviewWithRebateDiv").show();
                }
                else if (paramCheckOutType == "co") {
                    $("#CheckOutDetailsDiv").hide();
                    $("#RebateInformationDiv").hide();
                    $("#PaymentDetailsInformation").show();
                    $("#TransactionalButtonDiv").show();
                    $("#BillPreviewWithoutRebateDiv").show();
                    $("#BillPreviewWithRebateDiv").hide();
                    $("#GrandTotalPaymentDetailsDiv").show();
                }
            }

            var ddlcurrency = $("#<%=ddlCurrency.ClientID %>").val();
            PageMethods.LoadCurrencyType(ddlcurrency, OnLoadCurrencyTypeSucceess, OnLoadCurrencyTypeFail);

            function OnLoadCurrencyTypeSucceess(result) {
                $("#<%=hfCurrencyType.ClientID %>").val(result.CurrencyType);
            }

            function OnLoadCurrencyTypeFail() {
            }

            var currencyId = $("#<%=ddlCurrency.ClientID %>").val();
            if (currencyId == 0) {
                $('#ConversionRateDivInformation').hide();
            }
            CommonHelper.ApplyDecimalValidation();
            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtCompanyBank", "ContentPlaceHolder1_ddlCompanyBank", "ContentPlaceHolder1_ddlCompanyBank");
            CommonHelper.AutoSearchClientDataSource("txtMBankingBankId", "ContentPlaceHolder1_ddlMBankingBankId", "ContentPlaceHolder1_ddlMBankingBankId");

            var btnDayLetsOk = '<%=btnDayLetsOk.ClientID%>'
            var txtDayLetDiscount = '<%=txtDayLetDiscount.ClientID%>'
            var ddlDayLetDiscount = '<%=ddlDayLetDiscount.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            $('#' + txtConversionRate).attr('disabled', true);

            $('#' + txtDayLetDiscount).blur(function () {
                if (isNaN($('#' + txtDayLetDiscount).val())) {
                    $('#' + btnDayLetsOk).attr('disabled', true);
                    return;
                } else {
                    $('#' + btnDayLetsOk).attr('disabled', false);
                }
            });

            $("#<%=txtDiscountAmount.ClientID %>").blur(function () {
                var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtGrandTotal"), "Discount Amount");
                if (checkValue == false) {
                    $('#ContentPlaceHolder1_btnSave').attr('disabled', true);
                    return false;
                }
                CalculateDiscountAmount();
            });

            $("#<%=ddlDiscountType.ClientID %>").change(function () {
                var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtGrandTotal"), "Discount Amount");
                if (checkValue == false) {
                    $('#ContentPlaceHolder1_btnSave').attr('disabled', true);
                    return false;
                }
                CalculateDiscountAmount();
            });

            $('#TotalPaid').hide();

            var total = $("#<%=txtSalesTotal.ClientID %>").val();
            var registrationId = $("#<%=ddlRegistrationId.ClientID %>").val();
            var grandTotal = parseFloat(total);
            if (total == 0 && !isNaN(registrationId)) {
                $('#ContentPlaceHolder1_btnSave').attr('disabled', false);
                $('#ContentPlaceHolder1_AlartMessege').hide('slow');
            }
            else {
                $('#ContentPlaceHolder1_btnSave').attr('disabled', true);
                $('#ContentPlaceHolder1_AlartMessege').show('slow');
            }

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'

            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).show();
                $('#ComPaymentDiv').hide();
                $('#RefundDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "M-Banking") {
                $('#MBankingPaymentAccountHeadDiv').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).show();
                $('#ComPaymentDiv').hide();
                $('#RefundDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CardPaymentAccountHeadDiv').show();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).hide();
                $('#ComPaymentDiv').hide();
                $('#RefundDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).hide();
                $('#ComPaymentDiv').hide();
                $('#RefundDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').show();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#lblPaymentAccountHead').hide();
                $('#' + lblPaymentAccountHead).show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#ComPaymentDiv').show();
                $('#PrintPreviewDiv').hide();
                $('#RefundDiv').hide();
                $("#<%=HiddenFieldCompanyPaymentButtonInfo.ClientID %>").val('1');
            }
            else if ($('#' + ddlPayMode).val() == "Other Room") {
                $('#PaidByOtherRoomDiv').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).show();
                $('#ComPaymentDiv').hide();
                $('#PrintPreviewDiv').show();
                $('#RefundDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Refund") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).hide();
                $('#ComPaymentDiv').hide();
                $('#PrintPreviewDiv').show();
                $('#RefundDiv').show();
            }

            var txtExpireDate = '<%=txtExpireDate.ClientID%>'
            var txtCardNumber = '<%=txtCardNumber.ClientID%>'
            $('#' + txtCardNumber).blur(function () {
                validateCard();
            });
            var ddlCardType = '<%=ddlCardType.ClientID%>'
            $('#' + ddlCardType).change(function () {
                validateCard();
            });

            $('#ComPaymentDiv').hide();
            $('#PrintPreviewDiv').show();
            $("#<%=chkBillSpliteRoomItem.ClientID%> input").click(function () {
                CalculateTotalBillAmount();
            });
            $("#<%=chkBillSpliteServiceItem.ClientID%> input").click(function () {
                CalculateTotalBillAmount();
            });

            $("#closeSpan").click(function () {
                $('#ContentPlaceHolder1_chkIsBillSplit').prop("checked", false);

            });

            $("#btnBillPreview").click(function () {
                var txtConversionRate = '<%=txtConversionRate.ClientID%>'
                var txtConversionRateVal = $('#' + txtConversionRate).val();
                var txtConversionRateVal = 0;

                var isFOMultiInvoicePreviewOption = '<%=isFOMultiInvoicePreviewOptionEnable%>';
                if (isFOMultiInvoicePreviewOption > 0) {
                    txtConversionRateVal = $('#' + txtConversionRate).val();
                }

                if (txtConversionRateVal > 0) {
                    $("#BillPreviewRelatedInformation").dialog({
                        width: 380,
                        height: 100,
                        autoOpen: true,
                        modal: true,
                        closeOnEscape: true,
                        resizable: false,
                        fluid: true,
                        title: "", ////TODO add title
                        show: 'slide'
                    });
                }
                else {
                    $('#BillPreviewRelatedInformation').hide("slow");
                    var SelectdServiceId = "0";
                    var SelectdRoomId = "0";;
                    var HiddenStartDate = '<%=HiddenStartDate.ClientID%>'
                    var HiddenEndDate = '<%=HiddenEndDate.ClientID%>'
                    var ddlRegistrationId = '<%=ddlRegistrationId.ClientID%>'
                    var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

                    var SelectdServiceApprovedId = "0";
                    var SelectdRoomApprovedId = "0";
                    var SelectdPaymentId = "0";
                    var SelectdIndividualPaymentId = "0";
                    var SelectdIndividualTransferedPaymentId = "0";

                    var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                    var ddlServiceTypeVal = $('#' + ddlServiceType).val();

                    var StartDate = $('#' + HiddenStartDate).val();
                    var EndDate = $('#' + HiddenEndDate).val();
                    var RegistrationId = $('#' + ddlRegistrationId).val();
                    var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();
                    var isIsplite = "0";
                    //popup(-1); ////TODO close popup
                    PageMethods.PerformBillSplitePrintPreview(txtConversionRateVal, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
                    return false;
                }

            });

            $("#btnBillPreviewWithRebate").click(function () {
                var txtConversionRate = '<%=txtConversionRate.ClientID%>'
                var txtConversionRateVal = $('#' + txtConversionRate).val();
                var txtConversionRateVal = 0;

                var isFOMultiInvoicePreviewOption = '<%=isFOMultiInvoicePreviewOptionEnable%>';
                if (isFOMultiInvoicePreviewOption > 0) {
                    txtConversionRateVal = $('#' + txtConversionRate).val();
                }

                if (txtConversionRateVal > 0) {
                    $("#BillPreviewRelatedInformation").dialog({
                        width: 380,
                        height: 100,
                        autoOpen: true,
                        modal: true,
                        closeOnEscape: true,
                        resizable: false,
                        fluid: true,
                        title: "", ////TODO add title
                        show: 'slide'
                    });
                }
                else {
                    $('#BillPreviewRelatedInformation').hide("slow");
                    var SelectdServiceId = "0";
                    var SelectdRoomId = "0";;
                    var HiddenStartDate = '<%=HiddenStartDate.ClientID%>'
                    var HiddenEndDate = '<%=HiddenEndDate.ClientID%>'
                    var ddlRegistrationId = '<%=ddlRegistrationId.ClientID%>'
                    var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

                    var SelectdServiceApprovedId = "0";
                    var SelectdRoomApprovedId = "0";
                    var SelectdPaymentId = "0";
                    var SelectdIndividualPaymentId = "0";
                    var SelectdIndividualTransferedPaymentId = "0";

                    var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                    var ddlServiceTypeVal = $('#' + ddlServiceType).val();

                    var StartDate = $('#' + HiddenStartDate).val();
                    var EndDate = $('#' + HiddenEndDate).val();
                    var RegistrationId = $('#' + ddlRegistrationId).val();
                    var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();
                    var isIsplite = "0";
                    PageMethods.PerformBillSplitePrintPreview(txtConversionRateVal, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);

                    $("#BillPreviewWithoutRebateDiv").hide();
                    $("#BillPreviewWithRebateDiv").show();

                    return false;
                }
            });

            $("#btnBillPreviewAndBillLock").click(function () {
                var txtConversionRate = '<%=txtConversionRate.ClientID%>'
                var txtConversionRateVal = $('#' + txtConversionRate).val();
                var txtConversionRateVal = 0;

                var isFOMultiInvoicePreviewOption = '<%=isFOMultiInvoicePreviewOptionEnable%>';
                if (isFOMultiInvoicePreviewOption > 0) {
                    txtConversionRateVal = $('#' + txtConversionRate).val();
                }

                if (txtConversionRateVal > 0) {
                    $("#BillPreviewRelatedInformation").dialog({
                        width: 380,
                        height: 100,
                        autoOpen: true,
                        modal: true,
                        closeOnEscape: true,
                        resizable: false,
                        fluid: true,
                        title: "", ////TODO add title
                        show: 'slide'
                    });
                }
                else {
                    $('#BillPreviewRelatedInformation').hide("slow");
                    var SelectdServiceId = "0";
                    var SelectdRoomId = "0";;
                    var HiddenStartDate = '<%=HiddenStartDate.ClientID%>'
                    var HiddenEndDate = '<%=HiddenEndDate.ClientID%>'
                    var ddlRegistrationId = '<%=ddlRegistrationId.ClientID%>'
                    var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

                    var SelectdServiceApprovedId = "0";
                    var SelectdRoomApprovedId = "0";
                    var SelectdPaymentId = "0";
                    var SelectdIndividualPaymentId = "0";
                    var SelectdIndividualTransferedPaymentId = "0";

                    var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                    var ddlServiceTypeVal = $('#' + ddlServiceType).val();

                    var StartDate = $('#' + HiddenStartDate).val();
                    var EndDate = $('#' + HiddenEndDate).val();
                    var RegistrationId = $('#' + ddlRegistrationId).val();
                    var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();
                    var isIsplite = "0";

                    $('#btnBillPreviewAndBillLock').val("Bill Locked");
                    $('#btnBillPreviewAndBillLock').attr('disabled', true);

                    PageMethods.PerformBillSplitePrintPreviewAndBillLock("0", "0", "", txtConversionRateVal, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceededForFinalBill, OnPerformBillSplitePrintPreviewFailed);
                    return false;
                }
            });

            $("#btnBillPreviewAndBillLockWithRebate").click(function () {
                var txtConversionRate = '<%=txtConversionRate.ClientID%>'
                var txtConversionRateVal = $('#' + txtConversionRate).val();
                var txtConversionRateVal = 0;

                var isFOMultiInvoicePreviewOption = '<%=isFOMultiInvoicePreviewOptionEnable%>';
                if (isFOMultiInvoicePreviewOption > 0) {
                    txtConversionRateVal = $('#' + txtConversionRate).val();
                }

                if (txtConversionRateVal > 0) {
                    $("#BillPreviewRelatedInformation").dialog({
                        width: 380,
                        height: 100,
                        autoOpen: true,
                        modal: true,
                        closeOnEscape: true,
                        resizable: false,
                        fluid: true,
                        title: "", ////TODO add title
                        show: 'slide'
                    });
                }
                else {
                    $('#BillPreviewRelatedInformation').hide("slow");
                    var SelectdServiceId = "0";
                    var SelectdRoomId = "0";;
                    var HiddenStartDate = '<%=HiddenStartDate.ClientID%>'
                    var HiddenEndDate = '<%=HiddenEndDate.ClientID%>'
                    var ddlRegistrationId = '<%=ddlRegistrationId.ClientID%>'
                    var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

                    var SelectdServiceApprovedId = "0";
                    var SelectdRoomApprovedId = "0";
                    var SelectdPaymentId = "0";
                    var SelectdIndividualPaymentId = "0";
                    var SelectdIndividualTransferedPaymentId = "0";

                    var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                    var ddlServiceTypeVal = $('#' + ddlServiceType).val();

                    var StartDate = $('#' + HiddenStartDate).val();
                    var EndDate = $('#' + HiddenEndDate).val();
                    var RegistrationId = $('#' + ddlRegistrationId).val();
                    var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();
                    var isIsplite = "0";

                    $('#btnBillPreviewAndBillLock').val("Bill Locked");
                    $('#btnBillPreviewAndBillLock').attr('disabled', true);

                    var salesTotal = $("#<%=txtSalesTotal.ClientID %>").val();
                    var grandTotal = $("#<%=txtGrandTotal.ClientID %>").val();

                    PageMethods.PerformBillSplitePrintPreviewAndBillLock(salesTotal, grandTotal, "", txtConversionRateVal, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceededForFinalBill, OnPerformBillSplitePrintPreviewFailed);

                    $("#BillPreviewWithoutRebateDiv").hide();
                    $("#BillPreviewWithRebateDiv").hide();

                    return false;
                }
            });

            $("#btnAddDetailGuestPayment").click(function () {
                var enteredAmount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
                if (isNaN(enteredAmount)) {
                    toastr.info('Entered Amount is not in correct format.');
                    return;
                }

                var ddlCurrencyType = $("#<%=hfCurrencyType.ClientID %>").val();

                if (ddlCurrencyType != "Local") {
                    var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
                    if (ddlCurrency == 0) {
                        toastr.warning('Please select currency type.');
                        return;
                    }
                    else {
                        var txtConversionRate = $("#<%=txtConversionRate.ClientID %>").val();
                        if (isNaN(txtConversionRate)) {
                            toastr.info('Entered Conversion Rate is not in correct format.');
                            return;
                        }

                        if (txtConversionRate == 0) {
                            toastr.info('Entered Conversion Rate is not in correct format.');
                            return;
                        }
                    }
                }

                var ddlPayMode = $("#<%=ddlPayMode.ClientID %>").val();
                if (ddlPayMode == 0) {
                    toastr.info('Please Select Valid Payment Mode.');
                    return;
                }

                var duplicateCheck = false;

                $('#ReservationDetailGrid tbody > tr > td:nth-child(1)').filter(function (index) {
                    if (ddlPayMode == "Other Room") {
                        if ($.trim($(this).text()) == "Guest Room")
                            duplicateCheck = true;
                    }
                });

                if (duplicateCheck == true) {
                    toastr.warning('Duplicate Payment Mode (Guest Room) Not Possible.');
                    return;
                }

                var txtChecqueNumber = '<%=txtChecqueNumber.ClientID%>'
                var txtReceiveLeadgerAmount = '<%=txtReceiveLeadgerAmount.ClientID%>'
                var txtCardNumber = '<%=txtCardNumber.ClientID%>'
                var amount = $('#' + txtReceiveLeadgerAmount).val();
                var number = $('#' + txtCardNumber).val()
                var ddlRegistrationId = '<%=ddlRegistrationId.ClientID%>'
                var ddlPayMode = '<%=ddlPayMode.ClientID%>'
                var ddlBankId = '<%=ddlBankId.ClientID%>'
                var ddlCompanyBank = '<%=ddlCompanyBank.ClientID%>'
                var amount = $('#' + txtReceiveLeadgerAmount).val();
                var regId = $('#' + ddlRegistrationId).val();
                var ddlCardType = '<%=ddlCardType.ClientID%>'
                var ddlPaidByRegistrationId = '<%=ddlPaidByRegistrationId.ClientID%>'
                var isValid = true;

                if (regId > 0) {

                    if ($('#' + ddlPayMode).val() == "Card") {
                        if ($('#' + ddlCardType).val() == "0") {
                            toastr.info('Please Select Card Type.');
                            return;
                        }
                    }

                    if ($('#' + ddlPayMode).val() == "Other Room") {
                        if ($('#' + ddlPaidByRegistrationId).val() == "0") {
                            toastr.info('Please Select Guest Payment Room Number.');
                            return;
                        }
                    }

                    if (isValid == false) {
                        return;
                    }
                    else if (amount == "") {
                        toastr.info('Please provide Receive Amount.');
                        return;
                    }
                    else if ($('#' + ddlPayMode).val() == "Card" && $('#' + ddlBankId).val() == "0") {
                        toastr.info('Please provide Bank Name.');
                        $('#' + ddlBankId).focus();
                        return;
                    }
                    else if ($('#' + ddlPayMode).val() == "Cheque" && $('#' + txtChecqueNumber).val() == "") {
                        toastr.info('Please provide Cheque Number.');
                        $('#' + txtChecqueNumber).focus();
                        return;
                    }
                    else if ($('#' + ddlPayMode).val() == "Cheque" && $('#' + ddlCompanyBank).val() == "0") {
                        toastr.info('Please provide Bank Name.');
                        $('#' + ddlCompanyBank).focus();
                        return;
                    }
                    else {
                        SaveGuestPaymentDetailsInformationByWebMethod();
                    }
                }

                else {
                    toastr.info('Please provide a valid Room Number.');
                    return;
                }
            });

            $("#btnCompanyPayment").click(function () {
                $("#CompanyPaymentPopUpForm").dialog("close");

                var paymentServiceId = "", sPaymentServiceId = "", aPaymentServiceId = "", roomServiceId = "", srvServiceId = "";
                var toalPayment = 0.00;

                $("#ContentPlaceHolder1_chkCompanyPaymentBillSpliteRoomItem tbody tr").each(function () {

                    if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                        paymentServiceId = $(this).find("td:eq(0)").find("input").val();


                        $("#ContentPlaceHolder1_gvRoomDetail tbody tr").each(function () {

                            roomServiceId = $(this).find("td:eq(1)").find("span").text();

                            if (roomServiceId != "" && roomServiceId == paymentServiceId) {
                                toalPayment += parseFloat($(this).find("td:eq(7)").find("span").text());
                            }

                        });
                    }

                });

                $("#ContentPlaceHolder1_chkCompanyPaymentBillSpliteServiceItem tbody tr").each(function () {

                    if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                        sPaymentServiceId = $(this).find("td:eq(0)").find("input").val();

                        $("#ContentPlaceHolder1_gvServiceDetail tbody tr").each(function () {

                            srvServiceId = $(this).find("td:eq(1)").find("span").text();

                            if (srvServiceId != "" && srvServiceId == sPaymentServiceId) {
                                toalPayment += parseFloat($(this).find("td:eq(8)").find("span").text());
                            }

                        });
                    }

                });

                $("#ContentPlaceHolder1_chkCompanyPaymentBillSplitePaymentItem tbody tr").each(function () {

                    if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                        aPaymentServiceId = 1;
                    }

                });

                var advancePayment = $("#<%=txtAdvancePaymentAmount.ClientID %>").val();
                if (aPaymentServiceId > 0) {
                    $("#<%=txtReceiveLeadgerAmount.ClientID %>").val(Math.round(toalPayment - advancePayment));
                }
                else {
                    $("#<%=txtReceiveLeadgerAmount.ClientID %>").val(Math.round(toalPayment));
                }
            });

            $("#btnCancelBillSplitPrintPreview").click(function () {
                $('#ContentPlaceHolder1_chkIsBillSplit').prop("checked", false);
                $("#BillSplitPopUpForm").dialog("close");
            });

            $("#btnCloseTodaysRoomCharge").click(function () {
                $("#TodaysRoomCharge").dialog('close');
            });

            $("#btnCloseAddMoreRoomInfo").click(function () {
                $("#MultipleRoomInfoDiv").dialog('close');
            });

            $("#btnBillSplitPrintPreview").click(function () {
                var selectedServiceIdArray = new Array();
                var selectedServiceArray = new Array();
                var SelectdServiceId = "";
                var SelectdRoomId = "";

                var SelectdServiceApprovedId = "";
                var SelectdRoomApprovedId = "";
                var SelectdPaymentId = "";
                var SelectdIndividualPaymentId = "";
                var SelectdIndividualTransferedPaymentId = "";

                var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                var ddlServiceTypeVal = $('#' + ddlServiceType).val();

                if (ddlServiceTypeVal == "GroupService") {
                    $('#checkboxServiceList input:checked').each(function () {
                        SelectdServiceId = SelectdServiceId + $(this).attr('value') + ',';
                    });

                    $('#checkboxRoomList input:checked').each(function () {
                        SelectdRoomId = SelectdRoomId + $(this).attr('value') + ',';
                    });

                    $('#checkboxPaymentList input:checked').each(function () {
                        SelectdPaymentId = SelectdPaymentId + $(this).attr('value') + ',';
                    });

                    SelectdServiceId = RemoveFirstCommas(SelectdServiceId);
                    SelectdServiceId = RomoveLastCommas(SelectdServiceId);
                    SelectdRoomId = RemoveFirstCommas(SelectdRoomId);
                    SelectdRoomId = RomoveLastCommas(SelectdRoomId);
                }
                else {
                    $('#checkboxIndividualServiceList input:checked').each(function () {
                        SelectdServiceApprovedId = SelectdServiceApprovedId + $(this).attr('value') + ',';
                    });

                    $('#checkboxIndividualRoomList input:checked').each(function () {
                        SelectdRoomApprovedId = SelectdRoomApprovedId + $(this).attr('value') + ',';
                    });

                    $('#checkboxIndividualPaymentList input:checked').each(function () {
                        SelectdIndividualPaymentId = SelectdIndividualPaymentId + $(this).attr('value') + ',';
                    });

                    $('#checkboxIndividualTransferedPaymentList input:checked').each(function () {
                        SelectdIndividualTransferedPaymentId = SelectdIndividualTransferedPaymentId + $(this).attr('value') + ',';
                    });
                }

                SelectdServiceApprovedId = RemoveFirstCommas(SelectdServiceApprovedId);
                SelectdServiceApprovedId = RomoveLastCommas(SelectdServiceApprovedId);
                SelectdRoomApprovedId = RemoveFirstCommas(SelectdRoomApprovedId);
                SelectdRoomApprovedId = RomoveLastCommas(SelectdRoomApprovedId);
                SelectdPaymentId = RemoveFirstCommas(SelectdPaymentId);
                SelectdPaymentId = RomoveLastCommas(SelectdPaymentId);
                SelectdIndividualPaymentId = RemoveFirstCommas(SelectdIndividualPaymentId);
                SelectdIndividualPaymentId = RomoveLastCommas(SelectdIndividualPaymentId);
                SelectdIndividualTransferedPaymentId = RemoveFirstCommas(SelectdIndividualTransferedPaymentId);
                SelectdIndividualTransferedPaymentId = RomoveLastCommas(SelectdIndividualTransferedPaymentId);

                var txtStartDate = '<%=txtStartDate.ClientID%>'
                var txtEndDate = '<%=txtEndDate.ClientID%>'
                var ddlRegistrationId = '<%=ddlRegistrationId.ClientID%>'
                var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

                var StartDate = $('#' + txtStartDate).val();
                var EndDate = $('#' + txtEndDate).val();
                var RegistrationId = $('#' + ddlRegistrationId).val();
                var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();
                var isIsplite = "1";
                //popup(-1); ////TODO close popup

                PageMethods.PerformBillSplitePrintPreview(0, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
                return false;
            });

            $("#btnBillSplitPrintPreviewForUsd").click(function () {
                var selectedServiceIdArray = new Array();
                var selectedServiceArray = new Array();
                var SelectdServiceId = "";
                var SelectdRoomId = "";

                $('#checkboxServiceList input:checked').each(function () {
                    SelectdServiceId = SelectdServiceId + $(this).attr('value') + ',';
                });

                $('#checkboxRoomList input:checked').each(function () {
                    SelectdRoomId = SelectdRoomId + $(this).attr('value') + ',';
                });

                SelectdServiceId = RemoveFirstCommas(SelectdServiceId);
                SelectdServiceId = RomoveLastCommas(SelectdServiceId);
                SelectdRoomId = RemoveFirstCommas(SelectdRoomId);
                SelectdRoomId = RomoveLastCommas(SelectdRoomId);

                var SelectdServiceApprovedId = "";
                var SelectdRoomApprovedId = "";
                var SelectdPaymentId = "";
                var SelectdIndividualPaymentId = "";
                var SelectdIndividualTransferedPaymentId = "";
                $('#checkboxIndividualServiceList input:checked').each(function () {
                    SelectdServiceApprovedId = SelectdServiceApprovedId + $(this).attr('value') + ',';
                });

                $('#checkboxIndividualRoomList input:checked').each(function () {
                    SelectdRoomApprovedId = SelectdRoomApprovedId + $(this).attr('value') + ',';
                });

                $('#checkboxPaymentList input:checked').each(function () {
                    SelectdPaymentId = SelectdPaymentId + $(this).attr('value') + ',';
                });

                $('#checkboxIndividualPaymentList input:checked').each(function () {
                    SelectdIndividualPaymentId = SelectdIndividualPaymentId + $(this).attr('value') + ',';
                });

                $('#checkboxIndividualTransferedPaymentList input:checked').each(function () {
                    SelectdIndividualTransferedPaymentId = SelectdIndividualTransferedPaymentId + $(this).attr('value') + ',';
                });

                SelectdServiceApprovedId = RemoveFirstCommas(SelectdServiceApprovedId);
                SelectdServiceApprovedId = RomoveLastCommas(SelectdServiceApprovedId);
                SelectdRoomApprovedId = RemoveFirstCommas(SelectdRoomApprovedId);
                SelectdRoomApprovedId = RomoveLastCommas(SelectdRoomApprovedId);
                SelectdPaymentId = RemoveFirstCommas(SelectdPaymentId);
                SelectdPaymentId = RomoveLastCommas(SelectdPaymentId);
                SelectdIndividualPaymentId = RemoveFirstCommas(SelectdIndividualPaymentId);
                SelectdIndividualPaymentId = RomoveLastCommas(SelectdIndividualPaymentId);
                SelectdIndividualTransferedPaymentId = RemoveFirstCommas(SelectdIndividualTransferedPaymentId);
                SelectdIndividualTransferedPaymentId = RomoveLastCommas(SelectdIndividualTransferedPaymentId);

                var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                var ddlServiceTypeVal = $('#' + ddlServiceType).val();

                var txtStartDate = '<%=txtStartDate.ClientID%>'
                var txtEndDate = '<%=txtEndDate.ClientID%>'
                var ddlRegistrationId = '<%=ddlRegistrationId.ClientID%>'
                var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

                var StartDate = $('#' + txtStartDate).val();
                var EndDate = $('#' + txtEndDate).val();
                var RegistrationId = $('#' + ddlRegistrationId).val();
                var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();
                var isIsplite = "1";
                var txtConversionRate = '<%=txtConversionRate.ClientID%>'
                var txtConversionRateVal = $('#' + txtConversionRate).val();

                PageMethods.PerformBillSplitePrintPreview(txtConversionRateVal, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
                return false;
            });

            var txtBillDate = '<%=txtBillDate.ClientID%>'
            $('#ContentPlaceHolder1_txtBillDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtBillDate).datepicker("option", "minDate", selectedDate);
                    CalculateTotalBillAmount();
                }
            });

            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'
            $('#ContentPlaceHolder1_txtStartDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtEndDate').datepicker("option", "minDate", selectedDate);
                    CalculateTotalBillAmount();
                }
            });

            $('#ContentPlaceHolder1_txtEndDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtStartDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtGuestCheckInDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $('#ContentPlaceHolder1_txtExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            //EnableDisable For DropDown Change event--------------
            $(function () {
                var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                var HiddenFieldCompanyPaymentButtonInfo = '<%=HiddenFieldCompanyPaymentButtonInfo.ClientID%>'
                $('#' + ddlServiceType).change(function () {
                    if ($('#' + ddlServiceType).val() == "IndividualService") {
                        $('#GroupBillSpliteCheckBoxListDiv').hide();
                        $('#IndividualBillSpliteCheckBoxListDiv').show();
                        if ($('#' + HiddenFieldCompanyPaymentButtonInfo).val() == "1") {
                            $('#ComPaymentDiv').hide();
                        }
                        else {
                            $('#ComPaymentDiv').hide();
                        }
                    }
                    else {
                        $('#GroupBillSpliteCheckBoxListDiv').show();
                        $('#IndividualBillSpliteCheckBoxListDiv').hide();
                        if ($('#' + HiddenFieldCompanyPaymentButtonInfo).val() == "1") {
                            $('#ComPaymentDiv').show();
                        }
                        else {
                            $('#ComPaymentDiv').hide();
                        }
                    }
                });

                var ddlCurrencyType = '<%=ddlCurrency.ClientID%>'
                var txtConversionRate = '<%=txtConversionRate.ClientID%>'
                $('#' + ddlCurrencyType).change(function () {
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
                        $('#ConversionRateDivInformation').hide();
                    }
                    else {
                        $('#ConversionRateDivInformation').show();

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
                    }
                    else {
                        $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                        }
                    }

                    var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
                    if (ddlCurrency == 0) {
                        $('#ConversionRateDivInformation').hide()
                    }
                }
                function OnLoadConversionRateFailed() {
                }

                var ddlPayMode = '<%=ddlPayMode.ClientID%>'
                var lblReceiveLeadgerAmount = '<%=lblReceiveLeadgerAmount.ClientID%>'
                var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'
                var ddlCurrency = '<%=ddlCurrency.ClientID%>'
                var localCurrencyId = $("#<%=hfLocalCurrencyId.ClientID %>").val();

                $('#' + ddlPayMode).change(function () {
                    $('#' + lblReceiveLeadgerAmount).text("Receive Amount");
                    if ($('#' + ddlPayMode).val() == "Cash") {
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').show();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#RefundDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "M-Banking") {
                        $('#MBankingPaymentAccountHeadDiv').show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#RefundDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "Card") {
                        $('#CardPaymentAccountHeadDiv').show();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#RefundDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "Cheque") {
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').show();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#RefundDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "Company") {
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').show();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#lblPaymentAccountHead').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#ComPaymentDiv').show();
                        $('#PrintPreviewDiv').hide();
                        $('#RefundDiv').hide();
                        $("#<%=HiddenFieldCompanyPaymentButtonInfo.ClientID %>").val('1');
                    }
                    else if ($('#' + ddlPayMode).val() == "Other Room") {
                        $('#PaidByOtherRoomDiv').show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                        $('#RefundDiv').hide();

                    }
                    else if ($('#' + ddlPayMode).val() == "Refund") {
                        $('#' + lblReceiveLeadgerAmount).text("Paid Out");
                        //$('#' + ddlCurrency).val("45");
                        $('#' + ddlCurrency).val(localCurrencyId);
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                        $('#RefundDiv').show();
                    }
                    ShowhideCompanyPayButton();
                });
            });

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Check-Out</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });

        function ShowhideCompanyPayButton() {
            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            var HiddenFieldCompanyPaymentButtonInfo = '<%=HiddenFieldCompanyPaymentButtonInfo.ClientID%>'
            if ($('#' + ddlServiceType).val() == "IndividualService") {
                $('#GroupBillSpliteCheckBoxListDiv').hide();
                $('#IndividualBillSpliteCheckBoxListDiv').show();
                if ($('#' + HiddenFieldCompanyPaymentButtonInfo).val() == "1") {
                    $('#ComPaymentDiv').hide();
                }
                else {
                    $('#ComPaymentDiv').hide();
                }
            }
            else {
                $('#GroupBillSpliteCheckBoxListDiv').show();
                $('#IndividualBillSpliteCheckBoxListDiv').hide();
                if ($('#' + HiddenFieldCompanyPaymentButtonInfo).val() == "1") {
                    $('#ComPaymentDiv').show();
                }
                else {
                    $('#ComPaymentDiv').hide();
                }
            }
        }

        function ValidateDayLetsAmount() {

            var txtDayLetDiscount = '<%=txtDayLetDiscount.ClientID%>'
            if ($('#' + txtDayLetDiscount).val() == "") {
                $('#' + txtDayLetDiscount).val("0");
            }

            var amount = $('#' + txtDayLetDiscount).val();
            var floatAmount = parseFloat(amount);
            if (amount == "") {
                toastr.info('Let Checkout Discount Amount Cannot Be Empty.');
                $('#messegeDayLetsPopUp').show("slow");
                return false;
            }
            else if (floatAmount < 0) {
                toastr.info('Let Checkout Discount Amount is not in correct Format.');
                $('#messegeDayLetsPopUp').show("slow");
                return false;
            }
            else if (floatAmount > 100 && $("#<%=ddlDayLetDiscount.ClientID %>").val() == "Percentage") {

                toastr.info('Let Checkout Discount Amount Cannot Be Greater Than 100.');
                $('#messegeDayLetsPopUp').show("slow");
                return false;

            }
            else {
                $("#DayLetsDiv").dialog('close');
                $("#ContentPlaceHolder1_DayLetDiscountTypeHiddenField").val($("#ContentPlaceHolder1_ddlDayLetDiscount").val());
                $("#ContentPlaceHolder1_TodaysRoomBillHiddenField").val($("#ContentPlaceHolder1_txtDayLetDiscount").val());
                $("#btnDayLetsOkProcess").trigger("click");
                return false;
            }
        }

        function ValidateForTodayRoomCharge() {
            $("#TodaysRoomCharge").dialog('close');
            $("#btnOKTodaysRoomChargeProcess").trigger("click");
            return false;
        }
        function CheckAllRooms() {
            if ($("#chkAllRooms").is(":checked")) {
                $("#TableRoomInformation tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#TableRoomInformation tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }
        function ValidateForAddMoreRoomInfo() {
            var SelectdRoomId = "";
            var SelectdRoomNumber = "";
            var SelectdRegistrationId = "";
            var totalRoomAdded = 0;

            $('#TableRoomInformation tbody tr').each(function () {
                var chkBox = $(this).find("td:eq(0)").find("input");
                if ($(chkBox).is(":checked") == true) {
                    if (SelectdRoomId != "") {
                        SelectdRoomId += ',' + $(chkBox).attr('value');
                        SelectdRoomNumber += ',' + $(chkBox).attr('name');
                        SelectdRegistrationId += ',' + $(this).find("td:eq(2)").text();
                    }
                    else {
                        SelectdRoomId = $(chkBox).attr('value');
                        SelectdRoomNumber = $(chkBox).attr('name');
                        SelectdRegistrationId = $(this).find("td:eq(2)").text();
                    }

                    totalRoomAdded++;
                }
            });

            if (totalRoomAdded > 0) {
                $("#ContentPlaceHolder1_hfSelectedRoomId").val(SelectdRoomId);
                $("#ContentPlaceHolder1_txtAddedMultipleRoomId").val(SelectdRegistrationId);
            }
            else {
                $("#ContentPlaceHolder1_hfSelectedRoomId").val("");
                $("#ContentPlaceHolder1_txtAddedMultipleRoomId").val("");
            }

            $("#MultipleRoomInfoDiv").dialog('close');
            $("#ToggleMultipleRoomInfo").dialog('close');
            $("#btnOKAddMoreRoomProcess").trigger("click");
            return false;
        }

        function SaveGuestPaymentDetailsInformationByWebMethod() {
            var Amount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
        var hfGuestCompanyInformation = $("#<%=hfGuestCompanyInformation.ClientID %>").val();
        var floatAmout = parseFloat(Amount);
        if (floatAmout <= 0) {
            toastr.info('Receive Amount is not in correct format.');
            return;
        }

        var isEdit = false;
        if ($('#btnAddDetailGuestPayment').val() == "Edit") {
            $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("5");
            isEdit = true;
        }
        else {
            $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("0");
        }
        var ddlRegistrationId = $("#<%=ddlRegistrationId.ClientID %>").val();
        var ddlPayMode = $("#<%=ddlPayMode.ClientID %>").val();
        var txtReceiveLeadgerAmount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
        var ddlCashReceiveAccountsInfo = $("#<%=ddlCashReceiveAccountsInfo.ClientID %>").val();

        var txtCardNumber = $("#<%=txtCardNumber.ClientID %>").val();
        var ddlCardType = $("#<%=ddlCardType.ClientID %>").val();
        var txtExpireDate = $("#<%=txtExpireDate.ClientID %>").val();
        var txtCardHolderName = $("#<%=txtCardHolderName.ClientID %>").val();

        var txtChecqueNumber = $("#<%=txtChecqueNumber.ClientID %>").val();
        var ddlBankId = $("#<%=ddlBankId.ClientID %>").val();
        var ddlCompanyBankId = $("#<%=ddlCompanyBank.ClientID %>").val();
        var ddlMBankingBankId = $("#<%=ddlMBankingBankId.ClientID %>").val();
        var ddlMBankingReceiveAccountsInfo = $("#<%=ddlMBankingReceiveAccountsInfo.ClientID %>").val();
        var ddlChecquePaymentAccountHeadId = $("#<%=ddlChecquePaymentAccountHeadId.ClientID %>").val();
        var ddlCardPaymentAccountHeadId = $("#<%=ddlCardPaymentAccountHeadId.ClientID %>").val();

        var ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyPaymentAccountHead.ClientID %>").val();
        var ddlPaidByRegistrationId = $("#<%=ddlPaidByRegistrationId.ClientID %>").val();

        var RefundAccountHead = $("#<%=ddlRefundAccountHead.ClientID %>").val();

        var paymentDescription = "";

        if (ddlPayMode == "Cash") {
            paymentDescription = "";
        }
        else if (ddlPayMode == "M-Banking") {
            paymentDescription = "";
        }
        else if (ddlPayMode == "Card") {
            var ddlCardTypeText = $("#<%=ddlCardType.ClientID %> option:selected").text();
            paymentDescription = $("#<%=ddlCardType.ClientID %> option:selected").text();
        }
        else if (ddlPayMode == "Other Room") {
            var ddlPaidByRegistrationIdText = $("#<%=ddlPaidByRegistrationId.ClientID %> option:selected").text();
            paymentDescription = "Room# " + ddlPaidByRegistrationIdText;
        }
        else if (ddlPayMode == "Refund") {
            paymentDescription = "";
        }
        else if (ddlPayMode == "Company") {
            paymentDescription = hfGuestCompanyInformation;
        }
        else if (ddlPayMode == "Cheque") {
            var ddlPaidByChequeCompanyText = $("#<%=ddlChecquePaymentAccountHeadId.ClientID %> option:selected").text();
            var ddlCompanyBankText = $("#<%=ddlCompanyBank.ClientID %> option:selected").text();

            paymentDescription = "Bank: " + ddlCompanyBankText + ", Cheque: " + txtChecqueNumber;
        }

        var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
        //var conversionRate = $("#<%=txtConversionRateHiddenField.ClientID %>").val();
        var conversionRate = $("#<%=txtConversionRate.ClientID %>").val();
        var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
        var localCurrencyId = $("#<%=hfLocalCurrencyId.ClientID %>").val();

            $('#btnAddDetailGuestPayment').val("Add");
            if (IsValidRegistrationNumber) {
                if (ddlPayMode == "Cheque") {
                    PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, paymentDescription, ddlCurrency, currencyType, localCurrencyId, conversionRate, ddlPayMode, ddlCompanyBankId, txtReceiveLeadgerAmount, ddlRegistrationId, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlChecquePaymentAccountHeadId, ddlCardPaymentAccountHeadId, ddlCompanyPaymentAccountHead, ddlMBankingBankId, ddlMBankingReceiveAccountsInfo, ddlPaidByRegistrationId, RefundAccountHead, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)
                }
                else {
                    PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, paymentDescription, ddlCurrency, currencyType, localCurrencyId, conversionRate, ddlPayMode, ddlBankId, txtReceiveLeadgerAmount, ddlRegistrationId, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlChecquePaymentAccountHeadId, ddlCardPaymentAccountHeadId, ddlCompanyPaymentAccountHead, ddlMBankingBankId, ddlMBankingReceiveAccountsInfo, ddlPaidByRegistrationId, RefundAccountHead, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)
                }

                //PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, paymentDescription, ddlCurrency, conversionRate, ddlPayMode, ddlBankId, txtReceiveLeadgerAmount, ddlRegistrationId, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlChecquePaymentAccountHeadId, ddlCardPaymentAccountHeadId, ddlCompanyPaymentAccountHead, ddlPaidByRegistrationId, RefundAccountHead, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)
            }
            return false;
        }
        function OnPerformSaveGuestPaymentDetailsInformationFailed(error) {
            toastr.error(error.get_message());
        }
        function OnPerformSaveGuestPaymentDetailsInformationSucceeded(result) {
            $("#GuestPaymentDetailGrid").html(result);
            ClearDetailsPart();
            GetTotalPaidAmount();
        }

        function GetTotalPaidAmount() {
            PageMethods.PerformGetTotalPaidAmountByWebMethod(OnPerformGetTotalPaidAmountSucceeded, PerformGetTotalPaidAmountFailed)
            return false;
        }

        function PerformGetTotalPaidAmountFailed(error) {
            toastr.error(error.get_message());
        }
        function OnPerformGetTotalPaidAmountSucceeded(result) {
            var txtGrandTotal = $("#<%=txtGrandTotal.ClientID %>").val();
        var _registrationId = $("#<%=ddlRegistrationId.ClientID %>").val();
        var _grandTotal = parseFloat(txtGrandTotal);

        var GrandTotal = parseFloat(txtGrandTotal);
        var PaidTotal = parseFloat(result);

        //var GrandTotal = parseFloat(Math.round(txtGrandTotal));
        //var PaidTotal = parseFloat(Math.round(result));

        if (_grandTotal == 0 && !isNaN(_registrationId)) {
            if (PaidTotal != _grandTotal) {
                $('#ContentPlaceHolder1_btnSave').attr('disabled', true);
                $('#ContentPlaceHolder1_AlartMessege').show();
            }
            else {
                $('#ContentPlaceHolder1_btnSave').attr('disabled', false);
                $('#ContentPlaceHolder1_AlartMessege').hide();
            }
        }
        else if (PaidTotal == GrandTotal) {
            $('#ContentPlaceHolder1_btnSave').attr('disabled', false);
            $('#ContentPlaceHolder1_AlartMessege').hide();
        }
        else {
            $('#ContentPlaceHolder1_btnSave').attr('disabled', true);
            $('#ContentPlaceHolder1_AlartMessege').show();
        }

        var dueAmountTotal = GrandTotal - PaidTotal;
        $("#<%=HiddenFieldTotalPaid.ClientID %>").val(PaidTotal);
        $("#<%=HiddenFieldGrandTotal.ClientID %>").val(txtGrandTotal);

            var dueFormatedText = "";
            if (dueAmountTotal > 0) {
                dueFormatedText = "Due Amount   :  " + dueAmountTotal;
            }
            else {
                dueFormatedText = "Change Amount   :  " + (-1) * dueAmountTotal;
            }

            $('#dueTotal').show();
            $('#dueTotal').text(dueFormatedText);


            var FormatedText = "Total Amount: " + PaidTotal;
            $('#TotalPaid').show();
            $('#TotalPaid').text(FormatedText);
        }

        function ClearDetailsPart() {
        $("#<%=txtReceiveLeadgerAmount.ClientID %>").val('');
        $("#<%=txtCardNumber.ClientID %>").val('');
        $("#<%=ddlCardType.ClientID %>").val('0');
        $("#<%=txtExpireDate.ClientID %>").val('');
        $("#<%=txtCardHolderName.ClientID %>").val('');
        $("#<%=txtChecqueNumber.ClientID %>").val('');
        $('#txtBankId').val('');
        $('#txtCompanyBank').val('');
        $('#txtMBankingBankId').val('');
        $('#ConversionRateDivInformation').hide();
        $("#<%=ddlCurrency.ClientID %>").val('1').trigger('change');

            $("#DayLetDiscountInputDiv").hide();
            $("#AdvanceTodaysRoomChargeDiv").hide();
            $("#BillPreviewWithRebateDiv").hide();
            $("#BillPreviewWithoutRebateDiv").show();

            var paramCheckOutType = CommonHelper.GetParameterByName("cot");
            if (paramCheckOutType != "") {
                $("#BillPreviewWithoutRebateDiv").hide();
                $("#BillPreviewWithRebateDiv").show();
            }

            $('#btnTodaysRoomCharge').show();
            $('#btnAddMoreBill').hide();
            $('#ContentPlaceHolder1_btnSrcRoomNumber').attr('disabled', true);
        }

        function IsValidRegistrationNumber() {
            var txtRegistrationId = $("#<%=txtSrcRegistrationIdList.ClientID %>").val();
            if (txtRegistrationId == "" || parseInt(txtRegistrationId) <= 0) { return false; }
            else {
                return true;
            }
        }

        function PerformGuestPaymentDetailDelete(paymentId) {
            PageMethods.PerformDeleteGuestPaymentByWebMethod(paymentId, OnPerformDeleteGuestPaymentDetailsSucceeded, OnPerformDeleteGuestPaymentDetailsFailed);
            return false;
        }

        function OnPerformDeleteGuestPaymentDetailsSucceeded(result) {
            $("#ReservationDetailGrid").html(result);
            GetTotalPaidAmount();
            return false;
        }
        function OnPerformDeleteGuestPaymentDetailsFailed(error) {
        }

        function CalculateTotalBillAmount() {
            var selectedServiceIdArray = new Array();
            var selectedServiceArray = new Array();
            var SelectdServiceId = "";
            var SelectdRoomId = "";
            $('#checkboxServiceList input:checked').each(function () {
                SelectdServiceId = SelectdServiceId + $(this).attr('value') + ',';
            });

            $('#checkboxRoomList input:checked').each(function () {
                SelectdRoomId = SelectdRoomId + $(this).attr('value') + ',';
            });
            SelectdServiceId = RemoveFirstCommas(SelectdServiceId);
            SelectdServiceId = RomoveLastCommas(SelectdServiceId);
            SelectdRoomId = RemoveFirstCommas(SelectdRoomId);
            SelectdRoomId = RomoveLastCommas(SelectdRoomId);

            var ddlRegistrationId = '<%=txtSrcRegistrationIdList.ClientID%>'
        var RegistrationId = $('#' + ddlRegistrationId).val();
        var txtStartDate = '<%=txtStartDate.ClientID%>'
        var txtEndDate = '<%=txtEndDate.ClientID%>'

            var StartDate = $('#' + txtStartDate).val();
            var EndDate = $('#' + txtEndDate).val();

            PageMethods.GetTotalBillAmountByWebMethod(RegistrationId, SelectdRoomId, SelectdServiceId, StartDate, EndDate, OnGetTotalBillAmountByWebMethodSucceeded, OnGetTotalBillAmountByWebMethodFailed);
            return false;
        }

        function OnGetTotalBillAmountByWebMethodSucceeded(result) {
            var txtTotalBillAmount = '<%=txtTotalBillAmount.ClientID%>'
            $('#' + txtTotalBillAmount).val(result);
        }
        function OnGetTotalBillAmountByWebMethodFailed(error) {
        }

        function OnPerformCompanyPayBillSucceeded(result) {
            var txtTotalBillAmount = '<%=txtTotalBillAmount.ClientID%>'
        var hfAdvanceOrCashOutCalculatedAmountVal = '<%=hfAdvanceOrCashOutCalculatedAmount.ClientID%>'
        $("#<%=txtReceiveLeadgerAmount.ClientID %>").val($('#' + txtTotalBillAmount).val() - result);
        }
        function OnPerformCompanyPayBillFailed(error) {

        }

        function OnPerformBillSplitePrintPreviewSucceeded(result) {
            $('#ContentPlaceHolder1_chkIsBillSplit').attr('checked', false)
            var url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx";
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=825,height=680,left=300,top=50,resizable=yes");
            ClearDetailsPart();
        }

        function OnPerformBillSplitePrintPreviewSucceededForFinalBill(result) {
            $('#ContentPlaceHolder1_chkIsBillSplit').attr('checked', false)
            var url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx";
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=825,height=680,left=300,top=50,resizable=yes");
            ClearDetailsPart();

            $("#BillPreviewWithoutRebateDiv").hide();
            $("#BillPreviewWithRebateDiv").hide();
        }
        function OnPerformBillSplitePrintPreviewFailed(error) {

        }

        function RemoveFirstCommas(flag) {
            var length = flag.length;
            var Index = 0;
            for (var j = 0; j < length; j++) {
                if (flag.charAt(j) == '0' || flag.charAt(j) == '1' || flag.charAt(j) == '2' || flag.charAt(j) == '3' || flag.charAt(j) == '4' || flag.charAt(j) == '5' || flag.charAt(j) == '6' || flag.charAt(j) == '7' || flag.charAt(j) == '8' || flag.charAt(j) == '9') {
                    Index = j;
                    break;
                }
            }
            flag = flag.substring(Index, length - Index);

            return flag;
        }
        function RomoveLastCommas(flag) {
            var length = flag.length;
            var Index = 0;
            var lastIndex = flag.lastIndexOf(',');
            flag = flag.substring(0, length - (length - lastIndex));
            return flag;
        }


        //Integrated Gl Visible True/False-------------------
        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }

        function ToggleLetCheckOutFieldVisible(ctrl) {
            //if ($(ctrl).is(':checked')) {
            $("#DayLetsDiv").dialog({
                width: 600,
                height: 250,
                autoOpen: true,
                modal: true,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Late Check Out Discount Information",
                show: 'slide',
                open: function (event, ui) {
                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                }
            });
            //}
        }

        function ToggleTodaysRoomChargeInfo(ctrl) {
            //if ($(ctrl).is(':checked')) {
            $("#TodaysRoomCharge").dialog({
                width: 600,
                height: 250,
                autoOpen: true,
                modal: true,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Today's Room Charge Information",
                show: 'slide',
                open: function (event, ui) {
                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                }
            });
            //}
        }

        function ToggleMultipleRoomInfo(ctrl) {
            LoadRoomInformationWithControl();
            $("#MultipleRoomInfoDiv").dialog({
                width: 250,
                height: 400,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Expected Check Out Room List",
                show: 'slide',
                open: function (event, ui) {
                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                }
            });
            //}
        }

        function LoadRoomInformationWithControl() {
            var registrationId = $("#<%=ddlRegistrationId.ClientID %>").val();
            PageMethods.LoadRoomInformationWithControl(registrationId, OnLoadRoomInformationWithControlSucceeded, OnLoadRoomInformationWithControlFailed);

            return false;
        }
        function OnLoadRoomInformationWithControlSucceeded(result) {
            $("#ltlRoomNumberInfo").html(result);

            var RoomIdList = "";
            RoomIdList = $("#ContentPlaceHolder1_hfSelectedRoomId").val();
            var RoomArray = RoomIdList.split(",");

            if (RoomArray.length > 0) {
                for (var i = 0; i < RoomArray.length; i++) {
                    var roomId = "#" + RoomArray[i].trim();
                    $(roomId).prop("checked", true);
                }
            }
            return false;
        }
        function OnLoadRoomInformationWithControlFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformBillPreviewAction(serviceType, serviceBillId) {
            var url = "";
            var popup_window = "Invoice Preview";

            if (serviceType == "GuestHouseService") {
                url = "/HotelManagement/Reports/frmReportServiceBillInfo.aspx?billID=" + serviceBillId;
            }
            else if (serviceType == "RestaurantService") {
                url = "/POS/Reports/frmReportBillInfo.aspx?billID=" + serviceBillId;
            }
            else if (serviceType == "BanquetService") {
                url = "/Banquet/Reports/frmReportReservationBillInfo.aspx?Id=" + serviceBillId;
            }

            window.open(url, popup_window, "width=800,height=680,left=300,top=50,resizable=yes");
        }
        function ToggleBillSplitFieldVisible(ctrl) {
            if ($(ctrl).is(':checked')) {
                var btnLocalBillPreview = '<%=btnLocalBillPreview.ClientID%>'
                var btnLocalBillPreviewVal = $('#' + btnLocalBillPreview).val();

                //var txtConversionRate = '<%=txtConversionRateHiddenField.ClientID%>'
                var txtConversionRate = '<%=txtConversionRate.ClientID%>'
                var txtConversionRateVal = $('#' + txtConversionRate).val();
                //var txtConversionRateVal = 0;
                if (txtConversionRateVal > 0) {
                    $('#btnBillSplitPrintPreview').val(btnLocalBillPreviewVal);
                    $('#btnBillSplitPrintPreviewForUsd').show();
                }
                else {
                    $('#btnBillSplitPrintPreviewForUsd').hide();
                }

                var txtStartDate = '<%=txtStartDate.ClientID%>'
                var txtEndDate = '<%=txtEndDate.ClientID%>'

                $('#' + txtStartDate).val($("#<%=HiddenStartDate.ClientID %>").val());
                $('#' + txtEndDate).val($("#<%=HiddenEndDate.ClientID %>").val());

                $('#' + txtEndDate).datepicker("option", {
                    minDate: $('#' + txtStartDate).val(),
                    dateFormat: innBoarDateFormat
                });

                //popup(1, 'BillSplitPopUpForm', '', 600, 530);
                $("#BillSplitPopUpForm").dialog({
                    width: 600,
                    height: 530,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    title: "Guest Bill Split Information",
                    show: 'slide',
                    open: function (event, ui) {
                        $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                    }
                });
                $("#<%=HiddenFieldCompanyPaymentButtonInfo.ClientID %>").val('0');
                //// Group Service -----------
                $('#checkboxServiceList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                $('#checkboxRoomList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                $('#checkboxPaymentList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });


                //// Individual Service -----------
                $('#checkboxIndividualRoomList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                $('#checkboxIndividualServiceList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                $('#checkboxIndividualPaymentList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                $('#checkboxIndividualTransferedPaymentList input[type=checkbox]').each(function () {
                    $(this).prop("checked", true);
                });

                $("#<%=ddlPayMode.ClientID %>").val('Cash');
                $('#ComPaymentDiv').hide();
                $('#PrintPreviewDiv').show();

            }
            else {
                //popup(-1); ////TODO close popup
                $('#ComPaymentDiv').show();
                $('#PrintPreviewDiv').hide();
            }
        }

        //MessageDiv Visible True/False-------------------
        function GuestBillSplitPanelShow() {
            $('#GuestBillSplit').show("slow");
        }
        function GuestBillSplitPanelHide() {
            $('#GuestBillSplit').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });

        //CompanyProjectPanel Div Visible True/False-------------------
        function CompanyProjectPanelShow() {
            $('#CompanyProjectPanel').show("slow");
        }
        function CompanyProjectPanelHide() {
            $('#CompanyProjectPanel').hide("slow");
        }

        function ConfirmCheckOut() {
            var BillDate = $("#<%=txtBillDate.ClientID %>").val();
            var CheckOutDate = $("#<%=txtExpectedCheckOutDate.ClientID %>").val();
            var dateBill = GetDateTimeFromString(BillDate);
            var dateCheck = GetDateTimeFromString(CheckOutDate);

            if (dateCheck > dateBill) {
                var result = confirm("Are You Sure To Check Out?");
                if (result == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        function ConfirmHoldUp() {
            var BillDate = $("#<%=txtBillDate.ClientID %>").val();
            var CheckOutDate = $("#<%=txtExpectedCheckOutDate.ClientID %>").val();
            var dateBill = GetDateTimeFromString(BillDate);
            var dateCheck = GetDateTimeFromString(CheckOutDate);

            if (dateCheck > dateBill) {
                var result = confirm("Are Your Sure To Hold Up?");
                if (result == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        function validateCard() {
            var txtCardValidation = '<%=txtCardValidation.ClientID%>'
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() != "Card") {
                return true;
            }

            if ($('#' + txtCardValidation).val() == 0) {
                return true;
            }


            var txtCardNumber = '<%=txtCardNumber.ClientID%>'
            var ddlCardType = '<%=ddlCardType.ClientID%>'
            var cardNumber = $('#' + txtCardNumber).val();
            var cardType = $('#' + ddlCardType).val();
            var isTrue = true;
            var messege = "";

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
                //MessagePanelShow();
                //var lblMessage = '<%=lblMessage.ClientID%>'
                //$('#' + lblMessage).text(messege);
                //alert(messege);
                toastr.info(messege);
                return false;
            }
            //            else {
            //                MessagePanelHide();
            //                return true;
            //            }
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

        function CalculateDiscountAmount() {
            var txtDiscountAmount = '<%=txtDiscountAmount.ClientID%>'
            var txtSalesAmount = '<%=HiddenFieldSalesTotal.ClientID%>'
            var ddlDiscountType = '<%=ddlDiscountType.ClientID%>'

            var txtGrandTotal = '<%=txtGrandTotal.ClientID%>'
            var txtHFGrandTotal = '<%=HiddenFieldGrandTotal.ClientID%>'

            var txtGrandTotalUsd = '<%=txtGrandTotalUsd.ClientID%>'
            var hfGrandTotalUsd = '<%=hfGrandTotalUsd.ClientID%>'
            //var txtConversionRate = '<%=txtConversionRateHiddenField.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtGrandTotalInfo = '<%=txtGrandTotalInfo.ClientID %>';

            var discountAmount = 0;
            if ($('#' + txtDiscountAmount).val() != '') {
                discountAmount = $('#' + txtDiscountAmount).val();
            }

            var discountType = $('#' + ddlDiscountType).val();
            var salesAmount = $('#' + txtSalesAmount).val();

            var discount = 0;
            var grandTotal = 0;
            if (discountType == "Fixed") {
                discount = parseFloat(discountAmount);
                grandTotal = parseFloat(salesAmount) - parseFloat(discount);
            }
            else {
                var parcentAmount = parseFloat(discountAmount) / 100;
                discount = (parseFloat(salesAmount) * parcentAmount);

                grandTotal = parseFloat(salesAmount) - parseFloat(discount);
            }

            $('#' + txtGrandTotal).val(Math.round(grandTotal));
            $('#' + txtHFGrandTotal).val(Math.round(grandTotal));

            if ($('#' + txtConversionRate).val() > 0) {
                $('#' + txtGrandTotalUsd).val(Math.round(grandTotal) / $('#' + txtConversionRate).val());
                $('#' + hfGrandTotalUsd).val(Math.round(grandTotal) / $('#' + txtConversionRate).val());
            }


            $('#' + txtGrandTotalInfo).val(Math.round(grandTotal));

            if (parseFloat($('#' + txtHFGrandTotal).val()) < 1) {
                $('#PaymentDetailsInformation').hide("slow");
                $('#ContentPlaceHolder1_btnSave').attr('disabled', false);
            }
            else {
                $('#PaymentDetailsInformation').show("slow");
                $('#ContentPlaceHolder1_btnSave').attr('disabled', true);
            }
        }

        function PerformLocalBillPreviewAction() {
            $("#<%=hfIsEnableBillPreviewOption.ClientID %>").val("0");
            $('#BillPreviewRelatedInformation').dialog("close");
            var SelectdServiceId = "0";
            var SelectdRoomId = "0";;
            var HiddenStartDate = '<%=HiddenStartDate.ClientID%>'
            var HiddenEndDate = '<%=HiddenEndDate.ClientID%>'
            var ddlRegistrationId = '<%=ddlRegistrationId.ClientID%>'
            var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

            var SelectdServiceApprovedId = "0";
            var SelectdRoomApprovedId = "0";
            var SelectdPaymentId = "0";
            var SelectdIndividualPaymentId = "0";
            var SelectdIndividualTransferedPaymentId = "0";


            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            var ddlServiceTypeVal = $('#' + ddlServiceType).val();

            var StartDate = $('#' + HiddenStartDate).val();
            var EndDate = $('#' + HiddenEndDate).val();
            var RegistrationId = $('#' + ddlRegistrationId).val();
            var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();

            var isIsplite = "0";
            //popup(-1); ////TODO close popup



            PageMethods.PerformBillSplitePrintPreview(0, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
            return false;

        }

        function PerformUSDBillPreviewAction() {
            $("#<%=hfIsEnableBillPreviewOption.ClientID %>").val("1");
            $('#BillPreviewRelatedInformation').dialog("close");
            var SelectdServiceId = "0";
            var SelectdRoomId = "0";;
            var HiddenStartDate = '<%=HiddenStartDate.ClientID%>'
            var HiddenEndDate = '<%=HiddenEndDate.ClientID%>'
            var ddlRegistrationId = '<%=ddlRegistrationId.ClientID%>'
            var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

            var SelectdServiceApprovedId = "0";
            var SelectdRoomApprovedId = "0";
            var SelectdPaymentId = "0";
            var SelectdIndividualPaymentId = "0";
            var SelectdIndividualTransferedPaymentId = "0";


            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            var ddlServiceTypeVal = $('#' + ddlServiceType).val();

            var StartDate = $('#' + HiddenStartDate).val();
            var EndDate = $('#' + HiddenEndDate).val();
            var RegistrationId = $('#' + ddlRegistrationId).val();
            var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();

            var isIsplite = "0";
            //popup(-1); ////TODO close popup

            //var txtConversionRate = '<%=txtConversionRateHiddenField.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtConversionRateVal = $('#' + txtConversionRate).val();

            PageMethods.PerformBillSplitePrintPreview(txtConversionRateVal, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
            return false;

        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
        <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfIsEnableBillPreviewOption" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtAddedMultipleRoomId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfSelectedRoomId" runat="server" />
    </div>
    <%-- <div id="MultipleRoomInfoDiv" style="display: none;">
        <div class="block">
            <div class="block-body collapse in">
                <asp:GridView ID="gvMultipleRoomList" Width="100%" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                    ForeColor="#333333">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblGroupRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBoxAccept" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Room Number">
                            <ItemTemplate>
                                <asp:Label ID="lblgvRoomNumber" runat="server" Text='<%# Bind("RoomNumber") %>'></asp:Label>
                            </ItemTemplate>
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
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="Button1" runat="server" TabIndex="13" Width="100px" Text="Ok" CssClass="btn btn-primary btn-sm"
                        OnClick="btnOKAddMoreRoomProcess_Click" OnClientClick="javascript: return ValidateForAddMoreRoomInfo();" />
                    <input type="button" id="btnCloseAddMoreRoomInfo" style="width: 100px;" value="Close" class="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>--%>

    <div id="MultipleRoomInfoDiv" style="display: none;">
        <div id="RoomNumberInfoDiv">
            <asp:HiddenField ID="hfSelectedRegistrationId" runat="server" />
            <div style="height: 300px; overflow-y: scroll" id="ltlRoomNumberInfo">
            </div>
            <div style='margin-top: 12px;'>
                <button type='button' onclick='javascript:return ValidateForAddMoreRoomInfo()' id='btnAddRoom' class='btn btn-primary' style="width: 65px">OK</button>
                <button type='button' id='btnCloseAddMoreRoomInfo' class='btn btn-primary'>Close</button>
            </div>
        </div>
    </div>

    <div id="BillPreviewRelatedInformation" style="display: none; padding-top: 10px; overflow:hidden;">
        <div class="row">
            <div class="col-md-6">
                <asp:Button ID="btnLocalBillPreview" runat="server" Width="100%"  TabIndex="4" Text="Bill Preview"
                    CssClass="btn btn-primary btn-sm" OnClientClick="javascript: return PerformLocalBillPreviewAction();" />
            </div>
            <div class="col-md-6">
                <asp:Button ID="btnUSDBillPreview" runat="server" Width="100%"  TabIndex="4" Text="Bill Preview (USD)"
                    CssClass="btn btn-primary btn-sm" OnClientClick="javascript: return PerformUSDBillPreviewAction();" />
            </div>
        </div>
    </div>
    <div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Room Check-Out Information
            </div>
            <div style="height: 15px">
            </div>
            <div class="form-horizontal">
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:HiddenField ID="hfCurrencyType" runat="server" />
                        <asp:HiddenField ID="hfLocalCurrencyId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfGuestCompanyInformation" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenStartDate" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenEndDate" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfIsStopChargePosting" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblSrcRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-8">
                                <asp:HiddenField ID="txtSrcRegistrationIdList" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-4 form-inline">
                                <asp:Button ID="btnSrcRoomNumber" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSrcRoomNumber_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblRegistrationNumber" runat="server" class="control-label" Text="Registration Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRegistrationId" runat="server" CssClass="form-control" TabIndex="3"
                            Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group" id="CheckOutDetailsDiv">
                <div id="myTabs">
                    <ul id="tabPage" class="ui-style">
                        <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-1">Check-Out Information</a></li>
                        <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-2">Room Details </a></li>
                        <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-3">Service Details</a></li>
                        <%-- <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-4">Restaurant Details</a></li>--%>
                        <%--  <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-5">More Room </a></li>--%>
                    </ul>
                    <div id="tab-1">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblGuestCheckInDate" runat="server" class="control-label" Text="Check In Date"></asp:Label>
                                    <div style="display: none;">
                                        <asp:Label ID="lblBillDate" runat="server" class="control-label" Text="Check Out Date"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestCheckInDate" runat="server" ReadOnly="true" CssClass="form-control"
                                        TabIndex="4"></asp:TextBox>
                                    <div style="display: none;">
                                        <asp:TextBox ID="txtBillDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblExpectedCheckOutDate" runat="server" class="control-label" Text="Expected Check Out"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtExpectedCheckOutDate" runat="server" ReadOnly="true" CssClass="form-control"
                                        TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblServiceChargeTotal" runat="server" class="control-label" Text="Service Charge"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtServiceChargeTotal" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblCitySDChargeTotal" runat="server" class="control-label" Text="City/ SD Charge"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCitySDChargeTotal" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblVatTotal" runat="server" class="control-label" Text="Vat Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtVatTotal" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblAdditionalChargeTotal" runat="server" class="control-label" Text="Additional Charge"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtAdditionalChargeTotal" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblAdvancePaymentAmount" runat="server" class="control-label" Text="Advance Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtAdvancePaymentAmount" runat="server" CssClass="form-control"
                                        ReadOnly="true">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblAdvancePaymentAmountUSD" runat="server" class="control-label" Text="Advance Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtAdvancePaymentAmountUSD" runat="server" CssClass="form-control"
                                        ReadOnly="true">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblDiscountAmountTotal" runat="server" class="control-label" Text="Discount Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDiscountAmountTotal" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlSalesTotalLocal" runat="server" CssClass="form-control"
                                        TabIndex="7" Visible="False">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblSalesTotalLocal" runat="server" class="control-label" Text="Sales Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:HiddenField ID="HiddenFieldSalesTotal" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="HiddenFieldTotalPaid" runat="server"></asp:HiddenField>
                                    <asp:TextBox ID="txtSalesTotal" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                </div>
                                <div>
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-4">
                                        <div style="float: left">
                                            <div id="DayLetDiscountInputDiv" runat="server">
                                                <asp:CheckBox ID="chkIsDaysLet" runat="Server" Text=" Is Late Check-Out?" Font-Bold="true"
                                                    CssClass="mycheckbox" onclick="javascript: return ToggleLetCheckOutFieldVisible(this);" TextAlign="right" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                            </div>
                                            <div id="AdvanceTodaysRoomChargeDiv" runat="server">
                                                <input type="button" id="btnTodaysRoomCharge" onclick="javascript: return ToggleTodaysRoomChargeInfo(this);" value="Add Today's Room Charge" class="btn btn-primary btn-sm" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="USDInformationDiv" runat="server">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlSalesTotalUsd" runat="server" CssClass="form-control" TabIndex="10"
                                            Visible="False">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblSalesTotalUsd" runat="server" class="control-label" Text="Sales Amount (USD)"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="hfTxtSalesTotalUsd" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtSalesTotalUsd" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 aspBoxText" style="padding-top: 6px;">
                                        </div>
                                        <div class="col-md-4 col-padding-left-none">
                                        </div>
                                    </div>
                                </div>
                                <div id="DayLetDiscountOutputDiv" runat="server">
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="TodaysRoomBillHiddenField" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="DayLetDiscountAmountHiddenField" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="DayLetDiscountTypeHiddenField" runat="server"></asp:HiddenField>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" id="BillPreviewWithoutRebateDiv">
                                <div class="aspBoxText col-md-12">
                                    <asp:CheckBox ID="chkIsBillSplit" runat="Server" Text="" Font-Bold="true" onclick="javascript: return ToggleBillSplitFieldVisible(this);"
                                        TextAlign="right" />
                                    &nbsp;&nbsp;Is Bill Split?
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <input type="button" id="btnBillPreview" value="Bill Preview" class="btn btn-primary btn-sm" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <input type="button" id="btnBillPreviewAndBillLock" value="Final Bill" class="btn btn-primary btn-sm" />
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblReceivedAmount" runat="server" class="control-label" Text="Received Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtReceivedAmount" runat="server" CssClass="form-control" TabIndex="11">0</asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblDueAmount" runat="server" class="control-label" Text="Due Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDueAmount" runat="server" CssClass="form-control" TabIndex="12">0</asp:TextBox>
                                </div>
                            </div>
                            <div id="AccountsPostingPanel">
                                <div class="form-group" style="display: none;">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblReceiveAmount" runat="server" class="control-label" Text="Receive Amount"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtReceiveAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblConvertionRate" runat="server" class="control-label" Text="Convertion Rate"
                                            Visible="false"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtConvertionRate" runat="server" CssClass="form-control" Visible="false"> </asp:TextBox>
                                    </div>
                                </div>
                                <div class="childDivSection" id="BillSplitPopUpForm" style="display: none;">
                                    <div class="panel-body">
                                        <div class="form-horizontal" style="padding-bottom: 20px;">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" TabIndex="-1"></asp:TextBox><input
                                                        type="hidden" id="hidFromDate" />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" TabIndex="-1"></asp:TextBox><input
                                                        type="hidden" id="hidToDate" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblServiceType" runat="server" class="control-label" Text="Split Type"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control" TabIndex="-1">
                                                        <asp:ListItem Value="GroupService">Group Service</asp:ListItem>
                                                        <asp:ListItem Value="IndividualService">Individual Service</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <%--<div class="childDivSection">--%>
                                            <div style="display: none">
                                                <div id="IndividualServiceDivInfo" style="display: none">
                                                    <asp:GridView ID="gvIndividualServiceInformationForBillSplit" Width="100%" runat="server"
                                                        AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" GridLines="None"
                                                        AllowSorting="True" ForeColor="#333333" OnRowDataBound="gvIndividualServiceInformationForBillSplit_RowDataBound"
                                                        CssClass="table table-bordered table-condensed table-responsive">
                                                        <RowStyle BackColor="#E3EAEB" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="05%">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkIsSelected" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="95%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvMenuHead" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Service Type" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvServiceType" runat="server" Text='<%# Bind("ServiceType") %>'></asp:Label>
                                                                </ItemTemplate>
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
                                                <div id="GroupServiceDivInfo" style="display: none">
                                                    <asp:GridView ID="gvGroupServiceInformationForBillSplit" Width="100%" runat="server"
                                                        AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" GridLines="None"
                                                        AllowSorting="True" ForeColor="#333333" OnRowDataBound="gvGroupServiceInformationForBillSplit_RowDataBound"
                                                        CssClass="table table-bordered table-condensed table-responsive">
                                                        <RowStyle BackColor="#E3EAEB" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="05%">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkIsSelected" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="95%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvMenuHead" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Service Type" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvServiceType" runat="server" Text='<%# Bind("ServiceType") %>'></asp:Label>
                                                                </ItemTemplate>
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
                                        </div>
                                        <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="350px">
                                            <div id="GroupBillSpliteCheckBoxListDiv">
                                                <div class="checkbox checkboxList" id="checkboxRoomList">
                                                    <asp:CheckBoxList ID="chkBillSpliteRoomItem" runat="server" CssClass="customChkBox">
                                                    </asp:CheckBoxList>
                                                </div>
                                                <div class="checkbox checkboxList" id="checkboxServiceList" style="margin-top: -13px">
                                                    <asp:CheckBoxList ID="chkBillSpliteServiceItem" runat="server" CssClass="customChkBox">
                                                    </asp:CheckBoxList>
                                                </div>
                                                <div class="checkbox checkboxList" id="checkboxPaymentList" style="margin-top: -13px">
                                                    <asp:CheckBoxList ID="chkBillSplitePaymentItem" runat="server" CssClass="customChkBox">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                            <div id="IndividualBillSpliteCheckBoxListDiv" style="display: none;">
                                                <div class="checkbox checkboxList" id="checkboxIndividualRoomList">
                                                    <asp:CheckBoxList ID="chkBillSpliteIndividualRoomItem" runat="server" CssClass="customChkBox">
                                                    </asp:CheckBoxList>
                                                </div>
                                                <div class="checkbox checkboxList" id="checkboxIndividualServiceList" style="margin-top: -13px">
                                                    <asp:CheckBoxList ID="chkBillSpliteIndividualServiceItem" runat="server" CssClass="customChkBox">
                                                    </asp:CheckBoxList>
                                                </div>
                                                <div class="checkbox checkboxList" id="checkboxIndividualPaymentList" style="margin-top: -13px">
                                                    <asp:CheckBoxList ID="chkBillSpliteIndividualPaymentItem" runat="server" CssClass="customChkBox">
                                                    </asp:CheckBoxList>
                                                </div>
                                                <div class="checkbox checkboxList" id="checkboxIndividualTransferedPaymentList" style="margin-top: -13px">
                                                    <asp:CheckBoxList ID="chkBillSpliteIndividualTransferedPaymentItem" runat="server"
                                                        CssClass="customChkBox">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <div class="form-horizontal">
                                            <div class="form-group" style="display: none;">
                                                <div class="col-md-2">
                                                    Total Bill Amount
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:TextBox ID="txtTotalBillAmount" CssClass="form-control" runat="server" Width="190px"></asp:TextBox>
                                                    <asp:HiddenField ID="hfAdvanceOrCashOutCalculatedAmount" runat="server"></asp:HiddenField>
                                                </div>
                                                <div class="col-md-2">
                                                </div>
                                            </div>
                                            <%-- </div>--%>
                                            <div class="form-group" style="padding-top: 4px;">
                                                <div id="PrintPreviewDiv">
                                                    <input type="button" id="btnBillSplitPrintPreview" value="Print Preview" class="btn btn-primary" />
                                                    <input type="button" id="btnBillSplitPrintPreviewForUsd" value="Print Preview (USD)"
                                                        class="btn btn-primary" />
                                                    <input type="button" id="btnCancelBillSplitPrintPreview" value="Close" class="btn btn-primary" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--</div>--%>
                                </div>
                                <%--<div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                
                            </div>--%>
                                <div class="childDivSection" id="CompanyPaymentPopUpForm" style="display: none;">
                                    <div class="panel panel-default" style="height: 478px;">
                                        <div class="panel-body">
                                            <div class="panel-body" style="display: none">
                                                <div id="Div3">
                                                    <asp:GridView ID="gvCompanyPaymentGroupServiceInformationForBillSplit" Width="100%"
                                                        runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4"
                                                        GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowDataBound="gvGroupServiceInformationForBillSplit_RowDataBound"
                                                        CssClass="table table-bordered table-condensed table-responsive">
                                                        <RowStyle BackColor="#E3EAEB" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkIsSelected" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="90%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvMenuHead" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Service Type" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgvServiceType" runat="server" Text='<%# Bind("ServiceType") %>'></asp:Label>
                                                                </ItemTemplate>
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
                                            <asp:Panel ID="pnlCompanyPaymentRoomCalender" runat="server" ScrollBars="Both" Height="350px">
                                                <div id="CompanyPaymentGroupBillSpliteCheckBoxListDiv">
                                                    <div class="checkboxList" id="CompanyPaymentcheckboxRoomList">
                                                        <asp:CheckBoxList ID="chkCompanyPaymentBillSpliteRoomItem" runat="server" CssClass="customChkBox">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                    <div class="checkboxList" id="CompanyPaymentcheckboxServiceList" style="margin-top: -13px">
                                                        <asp:CheckBoxList ID="chkCompanyPaymentBillSpliteServiceItem" runat="server" CssClass="customChkBox">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                    <div class="checkboxList" id="CompanyPaymentcheckboxPaymentList" style="margin-top: -13px;">
                                                        <asp:CheckBoxList ID="chkCompanyPaymentBillSplitePaymentItem" runat="server" CssClass="customChkBox">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <div class="form-group" style="padding-top: 4px;">
                                                <asp:HiddenField ID="HiddenFieldCompanyPaymentButtonInfo" runat="server"></asp:HiddenField>
                                                <input type="button" id="btnCompanyPayment" value="Company Pay" class="btn btn-primary" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="RebateInformationDiv">
                            <div id="Div2" class="childDivSection">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        Rebate Information
                                    </div>
                                    <div class="panel-body childDivSectionDivBlockBody">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblDiscountType" runat="server" class="control-label" Text="Rebate Type"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control" TabIndex="16">
                                                        <asp:ListItem>Fixed</asp:ListItem>
                                                        <asp:ListItem>Percentage</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblDiscountAmount" runat="server" class="control-label" Text="Rebate Amount"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtDiscountAmount" TabIndex="3" CssClass="form-control quantitydecimal" runat="server"> </asp:TextBox>
                                                    <div style="display: none;">
                                                        <asp:DropDownList ID="ddlCashPaymentAccountHeadForDiscount" runat="server" CssClass="form-control"
                                                            TabIndex="16">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblGrandTotalLocal" runat="server" class="control-label" Text="Grand Total"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:HiddenField ID="HiddenFieldGrandTotal" runat="server"></asp:HiddenField>
                                                    <asp:TextBox ID="txtGrandTotal" TabIndex="3" runat="server" CssClass="form-control"
                                                        Enabled="false"> </asp:TextBox>
                                                </div>
                                                <div style="display: none;">
                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblGrandTotalUsd" runat="server" class="control-label" Text="Grand Total"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:HiddenField ID="hfGrandTotalUsd" runat="server"></asp:HiddenField>
                                                        <asp:TextBox ID="txtGrandTotalUsd" TabIndex="3" runat="server" CssClass="form-control"
                                                            Enabled="false"> </asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblRebateRemarks" runat="server" class="control-label" Text="Rebate Description"></asp:Label>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:TextBox ID="txtRebateRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                        TabIndex="27"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group" id="BillPreviewWithRebateDiv">
                                                <div class="aspBoxText col-md-12">
                                                    <asp:CheckBox ID="chkIsBillSplitWithRebate" runat="Server" Text="" Font-Bold="true" onclick="javascript: return ToggleBillSplitFieldVisible(this);"
                                                        TextAlign="right" />
                                                    &nbsp;&nbsp;Is Bill Split?
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <input type="button" id="btnBillPreviewWithRebate" value="Bill Preview" class="btn btn-primary btn-sm" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <input type="button" id="btnBillPreviewAndBillLockWithRebate" value="Final Bill" class="btn btn-primary btn-sm" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="tab-2">
                        <div id="SearchPanel" class="panel panel-default">
                            <div class="panel-heading">
                                Room Detail Information
                            </div>
                            <div class="panel-body">
                                <div>
                                    <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                                    <asp:GridView ID="gvRoomDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowDataBound="gvRoomDetail_RowDataBound"
                                        CssClass="table table-bordered table-condensed table-responsive">
                                        <RowStyle BackColor="#E3EAEB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="RegistrationId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IncomeNodeId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIncomeNodeId" runat="server" Text='<%#Eval("IncomeNodeId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ServiceDate"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ServiceType" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServiceType") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ServiceId" ShowHeader="false" ItemStyle-CssClass="hidden"
                                                HeaderStyle-CssClass="hidden">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="RoomNumber" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRoomNumber" runat="server" Text='<%#Eval("RoomNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceRate" runat="server" Text='<%# bind("ServiceRate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="15%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceQuantity" runat="server" Text='<%# bind("ServiceQuantity") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="6%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S. Charge">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceCharge" runat="server" Text='<%# bind("ServiceCharge") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="8%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="City Charge">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCitySDCharge" runat="server" Text='<%# bind("CitySDCharge") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="8%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vat">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVatAmount" runat="server" Text='<%# bind("VatAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="8%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Additional">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdditionalCharge" runat="server" Text='<%# bind("AdditionalCharge") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="8%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sales Commission" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvReferenceSalesCommission" runat="server" Text='<%# Bind("ReferenceSalesCommission") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiscountAmount" runat="server" Text='<%# bind("DiscountAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="8%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%# bind("TotalAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="20%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NightAudit" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNightAuditApproved" runat="server" Text='<%# bind("NightAuditApproved") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="20%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UsdTotalAmount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalAmountUSD" runat="server" Text='<%# Bind("UsdTotalAmount") %>'></asp:Label>
                                                </ItemTemplate>
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
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualRoomServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualRoomServiceCharge" runat="server" CssClass="form-control"
                                        ReadOnly="True">0</asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualRoomCitySDCharge" runat="server" Text="City Charge"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualRoomCitySDCharge" runat="server" CssClass="form-control"
                                        ReadOnly="True">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualRoomVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualRoomVatAmount" runat="server" CssClass="form-control"
                                        ReadOnly="True">0</asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualRoomAdditionalCharge" runat="server" Text="Additional Charge"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualRoomAdditionalCharge" runat="server" CssClass="form-control"
                                        ReadOnly="True">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualRoomGrandTotal" runat="server" Text="Total Amount"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualRoomGrandTotal" runat="server" CssClass="form-control"
                                        ReadOnly="true">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualRoomGrandTotalUSD" runat="server" Text="Total Amount"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualRoomGrandTotalUSD" runat="server" CssClass="form-control"
                                        ReadOnly="true">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualRoomDiscountAmount" runat="server" Text="Discount Amount"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualRoomDiscountAmount" runat="server" CssClass="form-control"
                                        ReadOnly="True">0</asp:TextBox>
                                </div>
                            </div>
                            <div style="text-align: right">
                                <input type="button" id="btnAddMoreBill" onclick="javascript: return ToggleMultipleRoomInfo(this);" value="Add More Room Bill" class="btn btn-primary btn-sm" />
                            </div>
                        </div>
                    </div>
                    <div id="tab-3">
                        <div id="Div1" class="panel panel-default">
                            <div class="panel-heading">
                                Service Detail Information
                            </div>
                            <div class="panel-body">
                                <div>
                                    <asp:Label ID="Label2" runat="server" Text='' Visible="False"></asp:Label>
                                    <asp:GridView ID="gvServiceDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowDataBound="gvServiceDetail_RowDataBound"
                                        CssClass="table table-bordered table-condensed table-responsive">
                                        <RowStyle BackColor="#E3EAEB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="RegistrationId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IncomeNodeId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIncomeNodeId" runat="server" Text='<%#Eval("IncomeNodeId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ServiceDate"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ServiceType" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServiceType") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ServiceId" ShowHeader="false" ItemStyle-CssClass="hidden"
                                                HeaderStyle-CssClass="hidden">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceQuantity" runat="server" Text='<%# bind("ServiceQuantity") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="6%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceRate" runat="server" Text='<%# bind("ServiceRate") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="10%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S. Charge">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceCharge" runat="server" Text='<%# bind("ServiceCharge") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="10%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SD">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCitySDCharge" runat="server" Text='<%# bind("CitySDCharge") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="5%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vat">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVatAmount" runat="server" Text='<%# bind("VatAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="8%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Additional">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAdditionalCharge" runat="server" Text='<%# bind("AdditionalCharge") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="8%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDiscountAmount" runat="server" Text='<%# bind("DiscountAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="8%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%# bind("TotalAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="20%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NightAudit" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNightAuditApproved" runat="server" Text='<%# bind("NightAuditApproved") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="20%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IsPaidService" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvIsPaidService" runat="server" Text='<%# Bind("IsPaidService") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UsdTotalAmount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalAmountUSD" runat="server" Text='<%# Bind("UsdTotalAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <img id='preview' src='../Images/ReportDocument.png' style="cursor: pointer;" onclick="javascript:return PerformBillPreviewAction('<%# Eval("ServiceType") %>','<%# Eval("ServiceBillId") %>')" alt='Invoice Preview' title='Invoice Preview' border='0' />
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
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualServiceServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualServiceServiceCharge" runat="server" CssClass="form-control"
                                        ReadOnly="True">0</asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualServiceCitySDCharge" runat="server" Text="SD Charge"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualServiceCitySDCharge" runat="server" CssClass="form-control"
                                        ReadOnly="True">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualServiceVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualServiceVatAmount" runat="server" CssClass="form-control"
                                        ReadOnly="True">0</asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualServiceAdditionalCharge" runat="server" Text="Additional Charge"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualServiceAdditionalCharge" runat="server" CssClass="form-control"
                                        ReadOnly="True">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualServiceGrandTotal" runat="server" Text="Total Amount"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualServiceGrandTotal" runat="server" CssClass="form-control"
                                        ReadOnly="true">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualServiceGrandTotalUSD" runat="server" Text="Total Amount"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualServiceGrandTotalUSD" runat="server" CssClass="form-control"
                                        ReadOnly="true">0</asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblIndividualServiceDiscountAmount" runat="server" Text="Discount Amount"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtIndividualServiceDiscountAmount" runat="server" CssClass="form-control"
                                        ReadOnly="True">0</asp:TextBox>
                                </div>
                            </div>
                            <div style="text-align: right">
                                <asp:Button ID="btnAddDetailGuest" runat="server" TabIndex="11" Text="Add More Service"
                                    CssClass="btn btn-primary btn-sm" OnClick="btnAddDetailGuest_Click" />
                            </div>
                        </div>
                    </div>
                    <div id="tab-4" style="display: none;">
                        <div id="RestaurantDivPanel" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Restaurant Detail
                                Information </a>
                            <div>
                                <asp:Label ID="Label1" runat="server" Text='' Visible="False"></asp:Label>
                                <asp:GridView ID="gvRestaurantDetail" Width="100%" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                    ForeColor="#333333">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IncomeNodeId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIncomeNodeId" runat="server" Text='<%#Eval("IncomeNodeId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ServiceDate"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ServiceType" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServiceType") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ServiceId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceRate" runat="server" Text='<%# bind("ServiceRate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="10%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceQuantity" runat="server" Text='<%# bind("ServiceQuantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="6%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vat">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVatAmount" runat="server" Text='<%# bind("VatAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="8%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S. Charge">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceCharge" runat="server" Text='<%# bind("ServiceCharge") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="10%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiscountAmount" runat="server" Text='<%# bind("DiscountAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="8%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# bind("TotalAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="20%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NightAudit" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNightAuditApproved" runat="server" Text='<%# bind("NightAuditApproved") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="20%"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UsdTotalAmount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmountUSD" runat="server" Text='<%# Bind("UsdTotalAmount") %>'></asp:Label>
                                            </ItemTemplate>
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
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="col-md-2">
                                <asp:Label ID="lblIndividualRestaurantVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtIndividualRestaurantVatAmount" runat="server" CssClass="form-control"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblIndividualRestaurantServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtIndividualRestaurantServiceCharge" runat="server" CssClass="form-control"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="col-md-2">
                                <asp:Label ID="lblIndividualRestaurantDiscountAmount" runat="server" Text="Discount Amount"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtIndividualRestaurantDiscountAmount" runat="server" CssClass="form-control"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblIndividualRestaurantGrandTotal" runat="server" Text="Total Amount"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtIndividualRestaurantGrandTotal" runat="server" CssClass="form-control"
                                    ReadOnly="true">0</asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblIndividualRestaurantGrandTotalUSD" runat="server" Text="Total Amount"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtIndividualRestaurantGrandTotalUSD" runat="server" CssClass="form-control"
                                    ReadOnly="true">0</asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                    </div>
                    <div id="tab-5" style="display: none;">
                        <div id="RoomDeltailInformation" class="block">
                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Room Detail Information
                            </a>
                            <div class="block-body collapse in">
                                <div>
                                    <asp:GridView ID="gvExtraRoomDetail" Width="100%" runat="server" AllowPaging="True"
                                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                        ForeColor="#333333">
                                        <RowStyle BackColor="#E3EAEB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("CheckOutId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="RoomNumber" HeaderText="Room Number" ItemStyle-Width="20%">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Vat Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtraVatAmount" runat="server" Text='<%# bind("VatAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="20%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service Charge">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtraServiceCharge" runat="server" Text='<%# bind("ServiceCharge") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="20%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Discount Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtraDiscountAmount" runat="server" Text='<%# bind("DiscountAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="20%"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Total">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExtraTotalAmount" runat="server" Text='<%# bind("TotalAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle Width="25%"></ItemStyle>
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
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="col-md-2">
                                <asp:Label ID="lblIndividualExtraRoomVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtIndividualExtraRoomVatAmount" runat="server" CssClass="form-control"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblIndividualExtraRoomServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtIndividualExtraRoomServiceCharge" runat="server" CssClass="form-control"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="col-md-2">
                                <asp:Label ID="lblIndividualExtraRoomDiscountAmount" runat="server" Text="Discount Amount"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtIndividualExtraRoomDiscountAmount" runat="server" CssClass="form-control"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblIndividualExtraRoomGrandTotal" runat="server" Text="Total Amount"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtIndividualExtraRoomGrandTotal" runat="server" CssClass="form-control"
                                    ReadOnly="true">0</asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                    </div>
                </div>
            </div>


            <%--<div id="GuestPaymentDetailsInformationDiv" runat="server">--%>
            <div>
                <div id="PaymentDetailsInformation" class="childDivSection">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Guest Payment Information
                        </div>
                        <div class="panel-body childDivSectionDivBlockBody">
                            <div class="form-horizontal">
                                <div class="form-group" id="GrandTotalPaymentDetailsDiv">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Grand Total"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtGrandTotalInfo" TabIndex="3" runat="server" CssClass="form-control"
                                            Enabled="false"> </asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblPayMode" runat="server" class="control-label required-field" Text="Payment Mode"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                            <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                            <asp:ListItem Value="Card">Card</asp:ListItem>
                                            <asp:ListItem Value="M-Banking">M-Banking</asp:ListItem>
                                            <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                            <asp:ListItem Value="Company">Company</asp:ListItem>
                                            <asp:ListItem Value="Other Room">Guest Room</asp:ListItem>
                                            <asp:ListItem Value="Refund">Refund</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblCurrencyType" runat="server" class="control-label required-field"
                                            Text="Currency Type"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlCurrency" TabIndex="6" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblDisplayConvertionRate" runat="server" Text=""></asp:Label>
                                        <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblReceiveLeadgerAmount" runat="server" class="control-label required-field"
                                            Text="Receive Amount"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtReceiveLeadgerAmount" runat="server" CssClass="form-control quantitydecimal"
                                            TabIndex="7"></asp:TextBox>
                                    </div>
                                    <div id="ConversionRateDivInformation" style="display: none;">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblConversionRate" runat="server" class="control-label required-field"
                                                Text="Conversion Rate"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control" Text=""></asp:TextBox>
                                            <asp:HiddenField ID="txtConversionRateHiddenField" runat="server"></asp:HiddenField>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group" style="display: none;">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblPaymentAccountHead" runat="server" class="control-label" Text="Account Head"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <div id="CashPaymentAccountHeadDiv">
                                            <asp:DropDownList ID="ddlCashReceiveAccountsInfo" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div id="BankPaymentAccountHeadDiv" style="display: none;">
                                            <asp:DropDownList ID="ddlCardReceiveAccountsInfo" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
                                            <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div id="MBankingReceiveAccountsInfo" style="display: none;">
                                            <asp:DropDownList ID="ddlMBankingReceiveAccountsInfo" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="PaidByOtherRoomDiv" style="display: none">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Room Number"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlPaidByRegistrationId" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                                    <div class="form-group" style="display: none;">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblChecquePaymentAccountHeadId" runat="server" class="control-label required-field"
                                                Text="Company Name"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control"
                                                TabIndex="6">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
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
                                    <div class="form-group" style="display: none;">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblCardPaymentAccountHeadId" runat="server" class="control-label"
                                                Text="Accounts Posting Head"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlCardPaymentAccountHeadId" runat="server" CssClass="form-control"
                                                TabIndex="6">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Card Type"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlCardType" runat="server" CssClass="form-control">
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
                                            <asp:TextBox ID="txtCardNumber" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--<div class="divSection">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblBankId" runat="server" Text="Bank Name"></asp:Label>
                                        <span class="MandatoryField">*</span>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlBankId" runat="server" CssClass="ThreeColumnDropDownList"
                                            AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>--%>
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
                                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Expiry Date"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtCardHolderName" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="MBankingPaymentAccountHeadDiv" style="display: none;">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblMBankingBankName" runat="server" class="control-label" Text="Bank Name"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <input id="txtMBankingBankId" type="text" class="form-control" />
                                            <div style="display: none;">
                                                <asp:DropDownList ID="ddlMBankingBankId" runat="server" CssClass="form-control" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="RefundDiv">
                                    <div class="form-group" style="display: none;">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblRefundAccountHead" runat="server" class="control-label" Text="Account Head"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlRefundAccountHead" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group" style="padding-left: 10px;">
                                    <%--Right Left--%>
                                    <input type="button" id="btnAddDetailGuestPayment" value="Add" class="btn btn-primary btn-sm"
                                        onclientclick="javascript: return ValidateForm();" />
                                    <asp:Label ID="lblHiddenIdDetailGuestPayment" runat="server" Text='' Visible="False"></asp:Label>
                                </div>
                                <div id="GuestPaymentDetailGrid" class="childDivSection">
                                </div>
                                <div id="TotalPaid" class="totalAmout">
                                </div>
                                <div id="dueTotal" class="totalAmout">
                                </div>
                            </div>
                        </div>
                        <div>
                            <asp:Label ID="AlartMessege" runat="server" Style="color: Red;" Text='Grand Total and Guest Payment Amount is not Equal.'
                                CssClass="totalAmout" Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div style="padding-top: 10px;" id="TransactionalButtonDiv">
                <asp:HiddenField ID="hfGuestPaymentDetailsInformationDiv" runat="server"></asp:HiddenField>
                <asp:Button ID="btnSave" runat="server" TabIndex="13" Text="Check Out" CssClass="btn btn-primary btn-sm"
                    OnClick="btnSave_Click" OnClientClick="javascript: return ConfirmCheckOut();" />
                <asp:Button ID="btnHoldUp" runat="server" TabIndex="13" Text="Hold Up" CssClass="btn btn-primary btn-sm"
                    OnClick="btnHoldUp_Click" OnClientClick="javascript: return ConfirmHoldUp();" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary btn-sm"
                    TabIndex="14" OnClick="btnCancel_Click" />
            </div>
        </div>
        <asp:HiddenField ID="hfRoomChargeType" Value="50" runat="server" />
        <div class="childDivSection" id="TodaysRoomCharge" style="display: none;">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            Charge
                        </div>
                        <div class="col-md-6">
                            <asp:DropDownList ID="ddlRoomChargeType" runat="server" CssClass="form-control" Width="110px">
                                <asp:ListItem Value="50">Half Day</asp:ListItem>
                                <asp:ListItem Value="0">Full Day</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnOKTodaysRoomCharge" runat="server" TabIndex="13" Width="100px" Text="Ok" CssClass="btn btn-primary btn-sm"
                        OnClick="btnOKTodaysRoomChargeProcess_Click" OnClientClick="javascript: return ValidateForTodayRoomCharge();" />
                    <input type="button" id="btnCloseTodaysRoomCharge" value="Close" class="btn btn-primary" />
                </div>
            </div>
        </div>
        <div class="childDivSection" id="DayLetsDiv" style="display: none;">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            Type
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDayLetDiscount" runat="server" CssClass="form-control" Width="110px">
                                <asp:ListItem>Percentage</asp:ListItem>
                                <asp:ListItem>Fixed</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            Amount
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDayLetDiscount" CssClass="form-control quantitydecimal" runat="server" Width="90px">0</asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnDayLetsOk" runat="server" TabIndex="13" Width="100px" Text="Ok" CssClass="btn btn-primary btn-sm"
                        OnClick="btnDayLetsOk_Click" OnClientClick="javascript: return ValidateDayLetsAmount();" />
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnDayLetsOkProcess" runat="server" Text="Button" OnClick="btnDayLetsOkProcess_Click"
            ClientIDMode="Static" />
        <asp:Button ID="btnOKTodaysRoomChargeProcess" runat="server" Text="Button" OnClick="btnOKTodaysRoomChargeProcess_Click"
            ClientIDMode="Static" />
        <asp:Button ID="btnOKAddMoreRoomProcess" runat="server" Text="Button" OnClick="btnOKAddMoreRoomProcess_Click"
            ClientIDMode="Static" />
    </div>
    <script type="text/javascript">
        var isIntegrated = '<%=isIntegratedGeneralLedgerDiv%>';
        if (isIntegrated > -1) {
            IntegratedGeneralLedgerDivPanelShow();
        }
        else {
            IntegratedGeneralLedgerDivPanelHide();
        }

        <%--var single = '<%=isSingle%>';
        if (single == "True") {
            $('#CompanyProjectPanel').hide();
        }
        else {
            $('#CompanyProjectPanel').show();
        }--%>
        $('#CompanyProjectPanel').hide();

        var paymentDetails = $("#<%=hfGuestPaymentDetailsInformationDiv.ClientID %>").val();
        if (paymentDetails > 0) {
            $('#PaymentDetailsInformation').show();
        }
        else {
            $('#PaymentDetailsInformation').hide();
        }

        var isInValidRoomNumberForTodaysCheckOut = '<%=isInValidRoomNumberForTodaysCheckOut%>';
        if (isInValidRoomNumberForTodaysCheckOut > 0) {

            $('#btnTodaysRoomCharge').hide();
            $('#btnBillPreview').hide();
            $('#btnBillPreviewAndBillLock').hide();
            $('#btnAddDetailGuestPayment').hide();
            $('#ContentPlaceHolder1_btnAddMoreBill').hide();
            $('#ContentPlaceHolder1_chkIsBillSplit').hide();
            $('#ContentPlaceHolder1_btnSave').hide();
            $('#ContentPlaceHolder1_btnHoldUp').hide();
            $('#ContentPlaceHolder1_btnCancel').hide();
        }
        else {
            $('#btnTodaysRoomCharge').show();
            $('#btnBillPreview').show();
            $('#btnBillPreviewAndBillLock').show();
            $('#btnAddDetailGuestPayment').show();
            $('#ContentPlaceHolder1_btnAddMoreBill').show();
            $('#ContentPlaceHolder1_chkIsBillSplit').show();
            $('#ContentPlaceHolder1_btnSave').show();
            $('#ContentPlaceHolder1_btnHoldUp').show();
            $('#ContentPlaceHolder1_btnCancel').show();
        }


        $(document).ready(function () {
            if ($("#<%=hfIsStopChargePosting.ClientID %>").val() == "1") {
                $('#btnBillPreviewAndBillLock').val("Bill Locked");
                $('#btnBillPreviewAndBillLock').attr('disabled', true);
            }
            else {
                $('#btnBillPreviewAndBillLock').val("Final Bill");
                $('#btnBillPreviewAndBillLock').attr('disabled', false);
            }

            $("#ContentPlaceHolder1_ddlRoomChargeType").change(function () {
                $("#ContentPlaceHolder1_hfRoomChargeType").val($(this).val());
            });
            $("#ContentPlaceHolder1_ddlRoomChargeType").trigger('change');
        });

    </script>
</asp:Content>
