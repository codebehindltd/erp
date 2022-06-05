<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmRestaurantBill.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmRestaurantBill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var alreadyAddedTableList = [];
        var alreadyAddedKotList = [];
        var tt = [];
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Restaurant Bill</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            CommonHelper.ApplyDecimalValidation();
            $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', true);

            $("#<%=txtPaxQuantity.ClientID %>").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    $("#<%=txtPaxQuantity.ClientID %>").focus();
                    return false;
                }
            });

            $("#btnLstLeftBillSplitPrintPreview").click(function () {
                var selectedValue = "";
                var selected = $("[id*=lstLeft] option").attr("selected", "selected");
                selected.each(function () {
                    selectedValue += $(this).val() + ",";
                });

                var billId = $('#ContentPlaceHolder1_txtBillIdForInvoicePreview').val();
                var url = "/Restaurant/Reports/frmReportSplitBillInfo.aspx?billId=" + billId + "~" + selectedValue;
                var popup_window = "Print Preview";
                window.open(url, popup_window, "width=770,height=680,left=300,top=50,resizable=yes");
                return false;
            });

            $("#btnLstRightBillSplitPrintPreview").click(function () {
                var selectedValue = "";
                var selected = $("[id*=lstRight] option").attr("selected", "selected");
                selected.each(function () {
                    selectedValue += $(this).val() + ",";
                });

                var billId = $('#ContentPlaceHolder1_txtBillIdForInvoicePreview').val();
                var url = "/Restaurant/Reports/frmReportSplitBillInfo.aspx?billId=" + billId + "~" + selectedValue;
                var popup_window = "Print Preview";
                window.open(url, popup_window, "width=770,height=680,left=300,top=50,resizable=yes");
                return false;
            });

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            if (window.location.href.indexOf("RoomNumber") > -1) {
                RestaurantRoomService();
            }
            GetKotWiseVatNSChargeNDiscountNComplementary();
            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtChequeBankId", "ContentPlaceHolder1_ddlChequeBankId", "ContentPlaceHolder1_ddlChequeBankId");

            var lblComplementaryRoom = '<%=lblComplementaryRoom.ClientID%>'
            var ddlComplementaryRoomId = '<%=ddlComplementaryRoomId.ClientID%>'
            var cbIsComplementary = '<%=cbIsComplementary.ClientID%>'

            if ($('#' + cbIsComplementary).is(':checked')) {
                $('#' + lblComplementaryRoom).show();
                $('#' + ddlComplementaryRoomId).show();
            }
            else {
                $('#' + lblComplementaryRoom).hide();
                $('#' + ddlComplementaryRoomId).hide();
            }

            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            var ddlGLCompany = '<%=ddlGLCompany.ClientID%>'
            var selectedIndex = parseFloat($('#' + ddlGLCompany).prop("selectedIndex"));
            if (selectedIndex > 0) {
                $("#" + ddlGLProject).attr("disabled", false);
            }
            else {
                $("#" + ddlGLProject).attr("disabled", true);
            }

            $("#<%=txtDiscountAmount.ClientID %>").blur(function () {
                var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtGrandTotal"), "Discount Amount");
                if (checkValue == false) {
                    //$('#ContentPlaceHolder1_btnSave').attr('disabled', true);
                    return false;
                }
                GetKotWiseVatNSChargeNDiscountNComplementary();
            });

            $("#<%=ddlDiscountType.ClientID %>").change(function () {
                var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtGrandTotal"), "Discount Amount");
                if (checkValue == false) {
                    //$('#ContentPlaceHolder1_btnSave').attr('disabled', true);
                    return false;
                }
                if ($(this).val() == "Fixed") {
                    $("#ContentPlaceHolder1_txtDiscountAmount").width(175);
                    $("#ImgCategoryWiseDiscount").show();
                    $("#BusinessPromotionInfoDiv").hide();
                    $("#MemberInformationDiv").hide();
                    $("#DiscountInformationDiv").show();
                    $('#ContentPlaceHolder1_ddlBusinessPromotionId').val("0");
                    $('#ContentPlaceHolder1_ddlMemberId').val("0");
                    GetKotWiseVatNSChargeNDiscountNComplementary();
                }
                else if ($(this).val() == "Percentage") {
                    $("#ContentPlaceHolder1_txtDiscountAmount").width(175);
                    $("#ImgCategoryWiseDiscount").show();
                    $("#BusinessPromotionInfoDiv").hide();
                    $("#MemberInformationDiv").hide();
                    $("#txtMemberCode").width(175);
                    $("#ImgCategoryWiseDiscount").show();
                    $("#DiscountInformationDiv").show();
                    $('#ContentPlaceHolder1_ddlBusinessPromotionId').val("0");
                    $('#ContentPlaceHolder1_ddlMemberId').val("0");
                    GetKotWiseVatNSChargeNDiscountNComplementary();
                }
                else if ($(this).val() == "BusinessPromotion") {
                    $("#DiscountInformationDiv").hide();
                    $("#MemberInformationDiv").hide();
                    $("#ddlBusinessPromotionI").width(175);
                    $("#ImgCategoryWiseDiscount").show();
                    $("#BusinessPromotionInfoDiv").show();
                    $('#ContentPlaceHolder1_ddlMemberId').val("0");
                }
                else if ($(this).val() == "Member") {
                    $("#DiscountInformationDiv").hide();
                    $("#BusinessPromotionInfoDiv").hide();
                    $("#MemberInformationDiv").show();
                    $('#ContentPlaceHolder1_ddlBusinessPromotionId').val("0");
                }
            });

            $("#<%=ddlBusinessPromotionId.ClientID %>").change(function () {
                var selectedValue = $(this).val().split('~')[1];
                $("#ContentPlaceHolder1_txtDiscountAmount").val(selectedValue);
                GetKotWiseVatNSChargeNDiscountNComplementary();
            });

            var ddlCurrencyType = '<%=ddlCurrency.ClientID%>'
            var hfLocalCurrencyId = '<%=hfLocalCurrencyId.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            $('#' + ddlCurrencyType).change(function () {
                var v = $("#<%=ddlCurrency.ClientID %>").val();
                PageMethods.LoadCurrencyType(v, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);
            });

            $("#ContentPlaceHolder1_ddlPaidService").change(function () {
                var serviceId = "";
                if ($("#ContentPlaceHolder1_ddlPaidService").val() != null) {
                    if ($("#ContentPlaceHolder1_ddlPaidService").val() != "0") {
                        serviceId = $("#ContentPlaceHolder1_ddlPaidService").val();
                        var svid = serviceId.split(',');

                        $("#ContentPlaceHolder1_txtPaymentAmount").attr('disabled', true);
                        $("#ContentPlaceHolder1_txtPaymentAmount").val(svid[1]);
                    }
                    else {
                        $("#ContentPlaceHolder1_txtPaymentAmount").attr('disabled', false);
                    }
                }
            });

            $('#txtMemberCode').blur(function () {
                var ddlMemberIdVal = $("#<%=ddlMemberId.ClientID %>").val();
                var memberDiscountPercentage = ddlMemberIdVal.split(',');
                $("#ContentPlaceHolder1_txtDiscountAmount").val(memberDiscountPercentage[1]);
                GetKotWiseVatNSChargeNDiscountNComplementary();
            });

            $("#<%=ddlEmpId.ClientID %>").change(function () {
                var ddlEmpId = $("#<%=ddlEmpId.ClientID %>").val();
                var ddlEmpIdVal = $("#<%=ddlEmpId.ClientID %>").find('option:selected').text();
                var txtCustomerName = '<%=txtCustomerName.ClientID%>'
                SetCustomerNameTextBox(ddlEmpIdVal);
                var costCenterId = $("#<%=hfCostCenterId.ClientID %>").val();
                PageMethods.GetPaidByDetailsInformationForRestaurantInvoice(costCenterId, "Employee", ddlEmpId, OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceSucceeded, OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceFailed)
                return false;
            });

            $("#<%=ddlRoomNumberId.ClientID %>").change(function () {
                var roomId = $("#<%=ddlRoomNumberId.ClientID %>").val();
                var costCenterId = $("#<%=hfCostCenterId.ClientID %>").val();
                PageMethods.GetPaidByDetailsInformationForRestaurantInvoice(costCenterId, "GuestRoom", roomId, OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceSucceeded, OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceFailed)
                return false;
            });

            $("#<%=ddlCompanyName.ClientID %>").change(function () {
                var ddlCompanyId = $("#<%=ddlCompanyName.ClientID %>").val();
                var costCenterId = $("#<%=hfCostCenterId.ClientID %>").val();
                PageMethods.GetPaidByDetailsInformationForRestaurantInvoice(costCenterId, "GuestCompany", ddlCompanyId, OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceSucceeded, OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceFailed)
                return false;
            });

            $("#<%=ddlComplementaryRoomId.ClientID %>").change(function () {
                $("#ContentPlaceHolder1_txtCustomerName").val("");
                var roomId = $("#<%=ddlComplementaryRoomId.ClientID %>").val();
                var costCenterId = $("#<%=hfCostCenterId.ClientID %>").val();
                PageMethods.GetPaidByDetailsInformationForRestaurantInvoice(costCenterId, "GuestRoom", roomId, OnPerformForGetPaidByDetailsInformationForComplementaryRestaurantInvoiceSucceeded, OnPerformForGetPaidByDetailsInformationForComplementaryRestaurantInvoiceFailed)
                return false;
            });

            $("#btnCategoryWiseDiscountCancel").click(function () {
                $("#PercentageDiscountDialog").dialog("close");
            });

            $("#btnCategoryWiseDiscountOK").click(function () {

                GetKotWiseVatNSChargeNDiscountNComplementary();
                if ($("#PercentageDiscountDialog").is(":visible")) {
                    $("#PercentageDiscountDialog").dialog("close");
                } else {
                }

                return false;
            });

            $("#btnAddDetailGuestPayment").click(function () {
                var ddlPayModeVal = $("#<%=ddlPayMode.ClientID %>").val();
                var enteredAmount = $("#<%=txtPaymentAmount.ClientID %>").val();
                var ddlDiscountTypeVal = $("#<%=ddlDiscountType.ClientID %>").val();

                if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Other Room") {
                    var duplicateCheck = false;
                    $('#GuestPaymentDetailGrid table tbody tr').each(function () {
                        var v = $.trim($(this).find("td:eq(0)").text());
                        if ((v).indexOf("Other Room") >= 0)
                            duplicateCheck = true;
                    });

                    if (duplicateCheck == true) {
                        toastr.warning('Multiple Room Payment Restiction.');
                        return;
                    }
                }

                if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Card") {
                    var ddlCardType = $('#<%=ddlCardType.ClientID%>').val();
                    var bankId =  $.trim($("#<%=ddlBankId.ClientID %>").val());
                    var cardNumber = $.trim($("#<%=txtCardNumber.ClientID %>").val());

                    if (ddlCardType == "") {
                        toastr.warning("Please select card.");
                        return false;
                    }
                    else if (bankId == "") {                        
                        toastr.warning("Please select bank.");
                        return false;
                    }
                    else if (bankId == "0") {
                        toastr.warning("Please select bank.");
                        return false;
                    }
                }
                else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cheque") {
                    var txtBankId = $.trim($("#<%=ddlChequeBankId.ClientID %>").val());
                    var txtChecqueNumber = $.trim($("#<%=txtChecqueNumber.ClientID %>").val());

                    if (txtBankId == "") {
                        toastr.warning("Please select bank.");
                        return false;
                    }
                    if (txtBankId == "0") {
                        toastr.warning("Please select bank.");
                        return false;
                    }
                    else if (txtChecqueNumber == "") {
                        toastr.warning("Please Provide Checque Number.");
                        return false;
                    }
                }

                if (ddlPayModeVal == "0") {
                    toastr.warning('Please Select Payment Mode.');
                    return;
                }
                else if (enteredAmount == "") {
                    toastr.warning('Entered Amount is not in correct format.');
                    return;
                }
                else if (enteredAmount == "0") {
                    toastr.warning('Entered Amount is not in correct format.');
                    return;
                }
                else if (CommonHelper.IsDecimal(enteredAmount) == false) {
                    toastr.warning('Entered Amount is not in correct format.');
                    return;
                }
                else if (CommonHelper.IsDecimal(enteredAmount) == true) {
                    var payMode = $("#<%=ddlPayMode.ClientID %>").val();
                if (payMode != 'Cash') {
                    var txtGrandTotal = 0;
                    var paidTotal = 0;
                    var isRestaurantBillInclusive = $("#<%=hfIsRestaurantBillInclusive.ClientID %>").val();
                        if (isRestaurantBillInclusive == 0) {
                            txtGrandTotal = $("#<%=txtGrandTotalHiddenField.ClientID %>").val();
                        }
                        else {
                            txtGrandTotal = $("#<%=txtNetAmount.ClientID %>").val();
                        }

                        paidTotal = $("#ContentPlaceHolder1_txtPaymentAmount").val();
                        if (parseFloat(paidTotal) > parseFloat(txtGrandTotal)) {
                            toastr.warning("Payment amount cannot be greater than Grand Total except cash payment.");
                            return;
                        }
                    }

                    if (ddlPayModeVal == "Member") {
                        if (ddlDiscountTypeVal != "Member") {
                            toastr.warning('Please Provide Valid Discount Type.');
                            return;
                        }
                        SaveGuestPaymentDetailsInformationByWebMethod();
                    }
                    else {
                        SaveGuestPaymentDetailsInformationByWebMethod();
                    }

                }
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
                    $('#<%=txtConversionRate.ClientID%>').attr("disabled", true);
                    $('#' + txtConversionRate).val("")
                    $('#ConversionRateDivInformation').hide();
                    $('#' + txtCalculatedLedgerAmount).attr("disabled", false);
                }
                else {
                    $('#<%=txtConversionRate.ClientID%>').attr("disabled", false);
                    $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);

                    $('#ConversionRateDivInformation').show();
                }

                var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
                if (ddlCurrency == 0) {
                    $('#ConversionRateDivInformation').hide();
                }
            }

            function OnLoadConversionRateFailed() {
            }

            function RestaurantRoomService() {
                var roomId = $("#<%=ddlRoomNumberId.ClientID %>").val();
                var costCenterId = $("#<%=hfCostCenterId.ClientID %>").val();
                if (roomId > 0 && costCenterId > 0) {
                    PageMethods.GetPaidByDetailsInformationForRestaurantInvoice(costCenterId, "GuestRoom", roomId, OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceSucceeded, OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceFailed)
                }
                return false;
            }

            function OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceFailed(error) {
                toastr.error(error.get_message());
            }

            function SetCustomerNameTextBox(paramValue) {
                var txtCustomerName = '<%=txtCustomerName.ClientID%>'
                var ddlPayMode = '<%=ddlPayMode.ClientID%>'
                var customerNameInfo = paramValue;
                var isValueSet = 1;

                if ($('#' + ddlPayMode).val() == "Other Room") {
                    isValueSet = 0;
                }
                else if ($('#' + ddlPayMode).val() == "Employee") {
                    isValueSet = 0;
                }
                else if ($('#' + ddlPayMode).val() == "Company") {
                    isValueSet = 0;
                }
                else if ($('#' + ddlPayMode).val() == "Member") {
                    isValueSet = 0;
                }

                if (isValueSet == 1) {
                    if (($('#' + txtCustomerName).val() != '') && paramValue.length != 0) {
                        customerNameInfo = $('#' + txtCustomerName).val() + ', ' + paramValue;
                    }
                    else {
                        customerNameInfo = paramValue;
                    }
                }

                //toastr.info(paramValue.length);

                $('#' + txtCustomerName).val(customerNameInfo);
            }

            function OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceSucceeded(result) {
                //var txtCustomerName = '<%=txtCustomerName.ClientID%>'
                //$('#' + txtCustomerName).val(result.PayByDetails);                
                SetCustomerNameTextBox(result.PayByDetails);

                $("#<%= ddlPaidService.ClientID %>").attr("disabled", false);
                if (result.RegistrationPaidService.length > 0) {
                    PopulateControl(result.RegistrationPaidService, $("#<%= ddlPaidService.ClientID %>"), "---Please Select---");
                }
                else {
                    $("#<%= ddlPaidService.ClientID %>").empty().append('<option selected="selected" value="0">Not Available<option>');
                    $("#<%= ddlPaidService.ClientID %>").attr("disabled", true);
                }
            }

            function OnPerformForGetPaidByDetailsInformationForComplementaryRestaurantInvoiceFailed(error) {
                toastr.error(error.get_message());
            }
            function OnPerformForGetPaidByDetailsInformationForComplementaryRestaurantInvoiceSucceeded(result) {
                //var txtCustomerName = '<%=txtCustomerName.ClientID%>'
                //$('#' + txtCustomerName).val(result.PayByDetails);

                SetCustomerNameTextBox(result.PayByDetails);
            }
            function OnPerformPercentageDiscountInformationFailed(error) {
                toastr.error(error.get_message());
            }
            function OnPerformPercentageDiscountInformationSucceeded(result) {
                $("#<%=hfCategoryWiseTotalDiscountAmount.ClientID %>").val(result);
                CalculateDiscountAmount();

                if ($("#PercentageDiscountDialog").is(":visible")) {
                    $("#PercentageDiscountDialog").dialog("close");
                } else {
                }
            }
            function SaveGuestPaymentDetailsInformationByWebMethod() {
                var Amount = $("#<%=txtPaymentAmount.ClientID %>").val();
                var floatAmout = parseFloat(Amount);
                if (floatAmout < 0) {
                    toastr.warning('Receive Amount is not in correct format.');
                    return;
                }

                var ddlCurrencyType = '<%=ddlCurrency.ClientID%>'
                var hfLocalCurrencyId = '<%=hfLocalCurrencyId.ClientID%>'

                if ($('#' + ddlCurrencyType).val() != $('#' + hfLocalCurrencyId).val()) {
                    var txtConversionRate = $("#<%=txtConversionRate.ClientID %>").val();

                    var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
                    if (ddlCurrency == 0) {
                        toastr.warning('Please select currency type.');
                        return;
                    }
                    else {
                        if (CommonHelper.IsDecimal(txtConversionRate) == false) {
                            toastr.warning('Entered Conversion Rate is not in correct format.');
                            return;
                        }
                        if (txtConversionRate == 0) {
                            toastr.warning('Entered Conversion Rate is not in correct format.');
                            return;
                        }
                    }
                }

                var isEdit = false;
                if ($('#btnAddDetailGuestPayment').val() == "Edit") {
                    $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("5");
                    isEdit = true;
                }
                else {
                    $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("0");
                }
                var ddlRegistrationId = 0;
                var ddlPayMode = $("#<%=ddlPayMode.ClientID %>").val();
                var txtReceiveLeadgerAmount = $("#<%=txtPaymentAmount.ClientID %>").val();
                var ddlCashReceiveAccountsInfo = $("#<%=ddlCashReceiveAccountsInfo.ClientID %>").val();

                var txtCardNumber = $("#<%=txtCardNumber.ClientID %>").val();
                var ddlCardType = $("#<%=ddlCardType.ClientID %>").val();
                var txtExpireDate = $("#<%=txtExpireDate.ClientID %>").val();
                var txtCardHolderName = $("#<%=txtCardHolderName.ClientID %>").val();

                var txtChecqueNumber = $("#<%=txtChecqueNumber.ClientID %>").val();
                var ddlBankId = $("#<%=ddlBankId.ClientID %>").val();
                var ddlChequeBankId = $("#<%=ddlChequeBankId.ClientID %>").val();
                var ddlChecquePaymentAccountHeadId = $("#<%=ddlChecquePaymentAccountHeadId.ClientID %>").val();
                var ddlCardPaymentAccountHeadId = $("#<%=ddlCardPaymentAccountHeadId.ClientID %>").val();

                var ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyPaymentAccountHead.ClientID %>").val();
                var ddlPaidByRegistrationId = 0;

                var RefundAccountHead = $("#<%=ddlRefundAccountHead.ClientID %>").val();
                var ddlEmpId = $("#<%=ddlEmpId.ClientID %>").val();
                var ddlEmployeePaymentAccountHead = $("#<%=ddlEmployeePaymentAccountHead.ClientID %>").val();

                var paymentDescription = "";
                var ddlPaidByRegistrationId = 0;

                if (ddlPayMode == "Cash") {
                    paymentDescription = "";
                }
                else if (ddlPayMode == "Card") {
                    var ddlCardTypeText = $("#<%=ddlCardType.ClientID %> option:selected").text();
                    paymentDescription = ddlCardTypeText;
                }
                else if (ddlPayMode == "Other Room") {
                    ddlPaidByRegistrationId = $("#<%=ddlRoomNumberId.ClientID %>").val();
                    var ddlPaidByRegistrationIdText = $("#<%=ddlRoomNumberId.ClientID %> option:selected").text();
                    paymentDescription = "Room# " + ddlPaidByRegistrationIdText;
                }
                else if (ddlPayMode == "Refund") {
                    paymentDescription = "";
                }
                else if (ddlPayMode == "Cheque") {
                    var ddlPaidByRegistrationId = $("#<%=ddlCompanyName.ClientID %>").val();
                    var ddlPaidByRegistrationIdText = $("#<%=ddlCompanyName.ClientID %> option:selected").text();
                    ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyName.ClientID %>").val();
                    paymentDescription = "Cheque Payment, Company: " + ddlPaidByRegistrationIdText;
                    ddlChecquePaymentAccountHeadId = ddlCompanyPaymentAccountHead;
                    ddlBankId = $("#<%=ddlChequeBankId.ClientID %>").val();
                }
                else if (ddlPayMode == "Company") {
                    var ddlPaidByRegistrationId = $("#<%=ddlCompanyName.ClientID %>").val();
                    var ddlPaidByRegistrationIdText = $("#<%=ddlCompanyName.ClientID %> option:selected").text();
                    ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyName.ClientID %>").val();
                    paymentDescription = "Company: " + ddlPaidByRegistrationIdText;
                }
                else if (ddlPayMode == "Employee") {
                    var ddlPaidByRegistrationId = $("#<%=ddlEmpId.ClientID %>").val();
                    var ddlPaidByRegistrationIdText = $("#<%=ddlEmpId.ClientID %> option:selected").text();
                    paymentDescription = "Employee: " + ddlPaidByRegistrationIdText;
                }

    if (ddlPayMode == "Card") {
        if (ddlCardType == "0") {
            toastr.warning('Please Select Card Type.');
            return;
        }
        else if (ddlBankId == "0") {
            toastr.warning('Please Select Bank Name.');
            return;
        }
    }

    if (ddlPayMode == "Other Room") {
        if (ddlPaidByRegistrationId == "0") {
            toastr.warning('Please Select Guest Payment Room Number.');
            return;
        }
    }
    if (ddlPayMode == "Employee") {
        if (ddlPaidByRegistrationId == "0") {
            toastr.warning('Please Select Employee Name.');
            return;
        }
    }
    if (ddlPayMode == "Cheque") {
        if (ddlPaidByRegistrationId == "0") {
            toastr.warning('Please Select Company Name.');
            return;
        }
        else if (ddlChequeBankId == "0") {
            toastr.warning('Please Select Bank Name.');
            return;
        }
    }
    if (ddlPayMode == "Company") {
        if (ddlPaidByRegistrationId == "0") {
            toastr.warning('Please Select Company Name.');
            return;
        }
    }

    var serviceId = "";
    if ($("#ContentPlaceHolder1_ddlPaidService").val() != null) {
        if ($("#ContentPlaceHolder1_ddlPaidService").val() != "0") {
            serviceId = $("#ContentPlaceHolder1_ddlPaidService").val();
            var svid = serviceId.split(',');
            serviceId = svid[0];
        }
    }

    var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
    var hfLocalCurrencyId = $("#<%=hfLocalCurrencyId.ClientID %>").val();
                var conversionRate = $("#<%=txtConversionRate.ClientID %>").val();
                var ddlDiscountTypeVal = $("#<%=ddlDiscountType.ClientID %>").val();
                var ddlMemberIdVal = $("#<%=ddlMemberId.ClientID %>").val();

                if (ddlDiscountTypeVal == "Member" && ddlPayMode == "Member") {
                    ddlCashReceiveAccountsInfo = ddlMemberIdVal;
                    ddlChecquePaymentAccountHeadId = ddlMemberIdVal;
                    ddlCardPaymentAccountHeadId = ddlMemberIdVal;
                    ddlCompanyPaymentAccountHead = ddlMemberIdVal;
                    ddlEmployeePaymentAccountHead = ddlMemberIdVal;
                }

                $('#btnAddDetailGuestPayment').val("Add");
                PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, paymentDescription, ddlCurrency, hfLocalCurrencyId, conversionRate, ddlPayMode, ddlBankId, txtReceiveLeadgerAmount, ddlRegistrationId, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlChecquePaymentAccountHeadId, ddlCardPaymentAccountHeadId, ddlCompanyPaymentAccountHead, ddlPaidByRegistrationId, RefundAccountHead, ddlEmpId, ddlEmployeePaymentAccountHead, serviceId, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)

                return false;
            }
            function OnPerformSaveGuestPaymentDetailsInformationFailed(error) {
                toastr.error(error.get_message());
            }
            function OnPerformSaveGuestPaymentDetailsInformationSucceeded(result) {
                tt = result;
                $("#GuestPaymentDetailGrid").html(result[0]);

                if (parseInt(result[1], 10) > 0) {
                    $("#<%=hfIsPaidServiceAddedForPayment.ClientID %>").val("1");
                    $("#ContentPlaceHolder1_btnPaymentPosting").attr('disabled', false);
                }
                else {
                    $("#<%=hfIsPaidServiceAddedForPayment.ClientID %>").val("0");
                }

                ClearDetailsPart();
                GetTotalPaidAmount();
            }

            function OnPerformForRoomGuestInformationFailed(error) {
                toastr.error(error.get_message());
            }
            function OnPerformForRoomGuestInformationSucceeded(result) {
                //var txtCustomerName = '<%=txtCustomerName.ClientID%>'
                //$('#' + txtCustomerName).val(result);

                SetCustomerNameTextBox(result);
            }

            $('#ChecquePaymentAccountHeadDiv').hide();
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#BankDiv').show();
                $('#AccountsPostingDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').show();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Other Room") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').show();
                $('#EmployeeInfoDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Employee") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').show();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').show();
                $('#ChecquePaymentAccountHeadDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Include With Room") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
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

            //EnableDisable For DropDown Change event--------------
            $(function () {
                var ddlPayMode = '<%=ddlPayMode.ClientID%>'
                var txtCustomerName = '<%=txtCustomerName.ClientID%>'
                var ddlCurrency = '<%=ddlCurrency.ClientID%>'
                var hfLocalCurrencyId = '<%=hfLocalCurrencyId.ClientID%>'
                $('#BankDiv').hide();
                $('#' + ddlPayMode).change(function () {
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $("#ContentPlaceHolder1_txtPaymentAmount").attr("disabled", false);
                    $("#<%=ddlCardType.ClientID %>").val('0');
                    if ($('#' + ddlPayMode).val() == "Cash") {
                        $('#BankDiv').hide();
                        $('#EmployeeInfoDiv').hide();
                        $('#RoomNumberDiv').hide();
                        $('#AccountsPostingDiv').hide();
                        $('#CashPaymentAccountHeadDiv').show();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#CompanyInfoDiv').hide();
                    }
                    else if ($('#' + ddlPayMode).val() == "Card") {
                        $('#RoomNumberDiv').hide();
                        $('#BankDiv').show();
                        $('#EmployeeInfoDiv').hide();
                        $('#AccountsPostingDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').show();
                        $('#CompanyInfoDiv').hide();
                    }
                    else if ($('#' + ddlPayMode).val() == "Other Room") {
                        $('#BankDiv').hide();
                        $('#RoomNumberDiv').show();
                        $('#EmployeeInfoDiv').hide();
                        $('#AccountsPostingDiv').hide();
                        $('#CompanyInfoDiv').hide();
                        var roomId = $("#<%=ddlRoomNumberId.ClientID %>").val();
                        if (roomId != null) {
                            if (roomId > 0) {
                                var costCenterId = $("#<%=hfCostCenterId.ClientID %>").val();
                                PageMethods.GetPaidByDetailsInformationForRestaurantInvoice(costCenterId, "GuestRoom", roomId, OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceSucceeded, OnPerformForGetPaidByDetailsInformationForRestaurantInvoiceFailed)
                                return false;
                            }
                        }
                    }
                    else if ($('#' + ddlPayMode).val() == "Employee") {
                        $('#BankDiv').hide();
                        $('#RoomNumberDiv').hide();
                        $('#EmployeeInfoDiv').show();
                        $('#AccountsPostingDiv').hide();
                        $('#CompanyInfoDiv').hide();
                    }
                    else if ($('#' + ddlPayMode).val() == "Cheque") {
                        $('#BankDiv').hide();
                        $('#RoomNumberDiv').hide();
                        $('#EmployeeInfoDiv').hide();
                        $('#AccountsPostingDiv').hide();
                        $('#CompanyInfoDiv').show();
                        $('#ChecquePaymentAccountHeadDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "Company") {
                        $('#BankDiv').hide();
                        $('#RoomNumberDiv').hide();
                        $('#EmployeeInfoDiv').hide();
                        $('#AccountsPostingDiv').hide();
                        $('#CompanyInfoDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "Include With Room") {
                        $('#BankDiv').hide();
                        $('#RoomNumberDiv').hide();
                        $('#EmployeeInfoDiv').hide();
                        $('#AccountsPostingDiv').hide();
                        $('#CompanyInfoDiv').hide();
                    }
                    else if ($('#' + ddlPayMode).val() == "Refund") {
                        $('#' + ddlCurrency).val($('#' + hfLocalCurrencyId).val());
                        $('#BankDiv').hide();
                        $('#EmployeeInfoDiv').hide();
                        $('#RoomNumberDiv').hide();
                        $('#AccountsPostingDiv').hide();
                        $('#CashPaymentAccountHeadDiv').show();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#CompanyInfoDiv').hide();
                        $('#ConversionRateDivInformation').hide();
                    }

                    if ($("#ContentPlaceHolder1_ddlPaidService").val() != null)
                        $("#ContentPlaceHolder1_ddlPaidService").val("0");
                });
            });

            CommonHelper.AutoSearchClientDataSource("txtMemberCode", "ContentPlaceHolder1_ddlMemberId", "ContentPlaceHolder1_ddlMemberId");
        });

function PerformGuestPaymentDetailDelete(paymentId, paidServiceId) {
    PageMethods.PerformDeleteGuestPaymentByWebMethod(paymentId, paidServiceId, OnPerformDeleteGuestPaymentDetailsSucceeded, OnPerformDeleteGuestPaymentDetailsFailed);
    return false;
}
function OnPerformDeleteGuestPaymentDetailsSucceeded(result) {

    $("#GuestPaymentDetailGrid").html(result[0]);

    if (result[1] == "1") {
        $("#<%=hfIsPaidServiceAddedForPayment.ClientID %>").val("1");
        $("#ContentPlaceHolder1_btnPaymentPosting").attr('disabled', false);
    }
    else {
        $("#<%=hfIsPaidServiceAddedForPayment.ClientID %>").val("0");
    }

    GetTotalPaidAmount();
    return false;
}
function OnPerformDeleteGuestPaymentDetailsFailed(error) {
}

function GetTotalPaidAmount() {
    PageMethods.PerformGetTotalPaidAmountByWebMethod(OnPerformGetTotalPaidAmountSucceeded, PerformGetTotalPaidAmountFailed)
    return false;
}

function PerformGetTotalPaidAmountFailed(error) {
    toastr.error(error.get_message());
}
function OnPerformGetTotalPaidAmountSucceeded(result) {
    var txtGrandTotal = 0;
    var IsRestaurantBillAmountWillRound = $("#<%=hfIsRestaurantBillAmountWillRound.ClientID %>").val();
            var isRestaurantBillInclusive = $("#<%=hfIsRestaurantBillInclusive.ClientID %>").val();
            if (isRestaurantBillInclusive == 0) {
                txtGrandTotal = $("#<%=txtGrandTotalHiddenField.ClientID %>").val();
            }
            else {
                txtGrandTotal = $("#<%=txtNetAmount.ClientID %>").val();
            }

            var _grandTotal = parseFloat(txtGrandTotal);
            var GrandTotal = parseFloat(txtGrandTotal);
            var PaidTotal = parseFloat(result);

            if ($("#<%=hfIsPaidServiceAddedForPayment.ClientID %>").val() != "1") {
                if (_grandTotal == 0) {
                    if (PaidTotal != _grandTotal) {
                        $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', true);
                        $('#ContentPlaceHolder1_AlartMessege').show();
                    }
                    else {
                        $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', false);
                        $('#ContentPlaceHolder1_AlartMessege').hide();
                    }
                }
                else if (PaidTotal == GrandTotal) {
                    $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', false);
                    $('#ContentPlaceHolder1_AlartMessege').hide();
                }
                else {
                    $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', true);
                    $('#ContentPlaceHolder1_AlartMessege').show();
                }
            }

            var dueAmountTotal = GrandTotal - PaidTotal;
            dueAmountTotal = dueAmountTotal.toFixed(2);
            if (dueAmountTotal > 0) {
                if (IsRestaurantBillAmountWillRound == "1") {
                    $("#<%=txtPaymentAmount.ClientID %>").val(Math.round(dueAmountTotal));
                }
                else {
                    $("#<%=txtPaymentAmount.ClientID %>").val(dueAmountTotal);
                }
            }
            else {
                if (IsRestaurantBillAmountWillRound == "1") {
                    $("#<%=txtPaymentAmount.ClientID %>").val(Math.round((-1) * dueAmountTotal));
                }
                else {
                    $("#<%=txtPaymentAmount.ClientID %>").val((-1) * dueAmountTotal);
                }
            }

            var dueFormatedText = "";
            var FormatedText = "";

            if (dueAmountTotal >= 0) {
                if (IsRestaurantBillAmountWillRound == "1") {
                    dueFormatedText = "Due Amount   :  " + Math.round(dueAmountTotal);
                    $('#dueTotal').show();
                    $('#dueTotal').text(dueFormatedText);
                    $("#<%=hfPaidServiceDiscount.ClientID %>").val(dueFormatedText);

                    FormatedText = "Total Amount: " + PaidTotal;
                    $('#TotalPaid').show();
                    $('#TotalPaid').text(FormatedText);
                }
                else {
                    dueFormatedText = "Due Amount   :  " + dueAmountTotal;
                    $('#dueTotal').show();
                    $('#dueTotal').text(dueFormatedText);
                    $("#<%=hfPaidServiceDiscount.ClientID %>").val(dueFormatedText);

                    FormatedText = "Total Amount: " + PaidTotal;
                    $('#TotalPaid').show();
                    $('#TotalPaid').text(FormatedText);
                }
                $("#<%=hfChangeAmount.ClientID %>").val('0');
            }
            else {
                dueAmountTotal = dueAmountTotal * -1;
                if (IsRestaurantBillAmountWillRound == "1") {
                    dueFormatedText = "Change Amount   :  " + Math.round(dueAmountTotal);
                    $('#dueTotal').show();
                    $('#dueTotal').text(dueFormatedText);
                    $("#<%=hfPaidServiceDiscount.ClientID %>").val(dueFormatedText);

                    FormatedText = "Total Amount: " + PaidTotal;
                    $('#TotalPaid').show();
                    $('#TotalPaid').text(FormatedText);
                }
                else {
                    dueFormatedText = "Change Amount   :  " + dueAmountTotal;
                    $('#dueTotal').show();
                    $('#dueTotal').text(dueFormatedText);
                    $("#<%=hfPaidServiceDiscount.ClientID %>").val(dueFormatedText);

                    FormatedText = "Total Amount: " + PaidTotal;
                    $('#TotalPaid').show();
                    $('#TotalPaid').text(FormatedText);
                }
                $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', false);
                $("#<%=hfChangeAmount.ClientID %>").val(dueAmountTotal);
            }

        }

        function ClearDetailsPart() {
            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('0');
            $("#<%=txtExpireDate.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');
            $('#txtBankId').val('');
            $('#txtChequeBankId').val('');
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'

            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#BankDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#RoomNumberDiv').hide();
                $('#BankDiv').show();
                $('#EmployeeInfoDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').show();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Other Room") {
                $('#BankDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#RoomNumberDiv').show();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Employee") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').show();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Include With Room") {
                $('#BankDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
            }

            $("#ContentPlaceHolder1_txtPaymentAmount").attr("disabled", false);
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
                        toastr.info(response.d);
                    }
                });
            }
        }

        function OnProjectsPopulated(response) {
            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            $("#" + ddlGLProject).attr("disabled", false);

            PopulateControl(response.d, $("#<%=ddlGLProject.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }

        //MessageDiv Visible True/False-------------------
        function RoomNumberPanelShow() {
            $('#RoomNumberDiv').show("slow");
            $('#AccountsPostingDiv').hide();
        }
        function RoomNumberPanelHide() {
            $('#RoomNumberDiv').hide("slow");
            $('#AccountsPostingDiv').hide();
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
                isTrue = false;
                messege = "Sorry! this is not a valid credit card number.";
            }

            if (isTrue == false) {
                toastr.warning(messege);
                return false;
            }
            else {
                return true;
            }
        }

        function LoadMemberSearchInfo() {
            LoadPercentageDiscountInfo();
        }

        function LoadPercentageDiscountInfo() {
            var Amount = $("#<%=txtDiscountAmount.ClientID %>").val();
            var floatAmout = parseFloat(Amount);
            if (floatAmout <= 0) {
                toastr.warning('Please Provide Valid Discount Amount.');
                return;
            }

            $("#PercentageDiscountDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 450,
                closeOnEscape: true,
                resizable: false,
                title: "Category Wise Discount",
                show: 'slide'
            });
            return;
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

        $(function () {
            $("#myTabs").tabs();
        });

        function LoadDiscountRelatedInformation() {
            var transactionId = 0;
            var ddlDiscountTypeVal = $("#<%=ddlDiscountType.ClientID %>").val();
            var ddlBusinessPromotionIdVal = $("#<%=ddlBusinessPromotionId.ClientID %>").val();
            var ddlMemberIdVal = $("#<%=ddlMemberId.ClientID %>").val();

            if (window.location.href.indexOf("RoomNumber") < 0) {
                $("#<%=ddlPayMode.ClientID %>").val("Cash");
            }

            $("#<%=ddlPayMode.ClientID %>").attr("disabled", false);
            $("#<%=ddlBankId.ClientID %>").attr("disabled", false);
            $("#<%=ddlCardType.ClientID %>").attr("disabled", false);
            ClearDetailsPart();

            if (ddlDiscountTypeVal == "Member") {
                transactionId = ddlMemberIdVal;
            }
            else if (ddlDiscountTypeVal == "BusinessPromotion") {
                transactionId = ddlBusinessPromotionIdVal;
            }
            else
                return false;

            PageMethods.LoadDiscountRelatedInformation(ddlDiscountTypeVal, transactionId, OnLoadDiscountRelatedInformationSucceeded, OnLoadDiscountRelatedInformationFailed);
            return false;
        }

        function OnLoadDiscountRelatedInformationSucceeded(result) {
            $("#<%=txtDiscountAmount.ClientID %>").val(result.PercentAmount);
            CalculateDiscountAmount();

            if ($("#<%=ddlDiscountType.ClientID %>").val() == "BusinessPromotion") {
                if ($("#<%=ddlBusinessPromotionId.ClientID %>").val() > 0) {
                    PageMethods.LoadBusinessPromotionRelatedInformation($("#<%=ddlBusinessPromotionId.ClientID %>").val(), OnLoadBusinessPromotionRelatedInformationSucceeded, OnLoadBusinessPromotionRelatedInformationFailed);
                }
            }
            return false;
        }
        function OnLoadDiscountRelatedInformationFailed(error) {
            toastr.error(error.get_message());
        }

        function OnLoadBusinessPromotionRelatedInformationSucceeded(result) {
            if (window.location.href.indexOf("RoomNumber") < 0) {
                $("#<%=ddlPayMode.ClientID %>").val("Cash");
            }

            $("#<%=ddlPayMode.ClientID %>").attr("disabled", false);
            $("#<%=ddlBankId.ClientID %>").attr("disabled", false);
            $("#<%=ddlCardType.ClientID %>").attr("disabled", false);

            if (result[0].TransactionType == "Bank") {

                $("#<%=ddlPayMode.ClientID %>").val("Card");
                $("#<%=ddlBankId.ClientID %>").val(result[0].TransactionId);

                if (result[1].TransactionType == "CardType") {
                    if (result[1].TransactionId == 394) {
                        $("#<%=ddlCardType.ClientID %>").val("a");
                        $("#<%=ddlCardType.ClientID %>").attr("disabled", true);
                    }
                    else if (result[1].TransactionId == 395) {
                        $("#<%=ddlCardType.ClientID %>").val("m");
                        $("#<%=ddlCardType.ClientID %>").attr("disabled", true);
                    }
                    else if (result[1].TransactionId == 396) {
                        $("#<%=ddlCardType.ClientID %>").val("v");
                        $("#<%=ddlCardType.ClientID %>").attr("disabled", true);
                    }
                    else if (result[1].TransactionId == 397) {
                        $("#<%=ddlCardType.ClientID %>").val("d");
                        $("#<%=ddlCardType.ClientID %>").attr("disabled", true);
                    }
    }

    $("#<%=ddlPayMode.ClientID %>").attr("disabled", true);
                LoadInformationDynamically();
            }
            return false;
        }

        function LoadInformationDynamically() {
            $("#<%=txtPaymentAmount.ClientID %>").val('');
            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('a');
            $("#<%=txtExpireDate.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'

            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#BankDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#RoomNumberDiv').hide();
                $('#BankDiv').show();
                $('#EmployeeInfoDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').show();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Other Room") {
                $('#BankDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#RoomNumberDiv').show();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Employee") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').show();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#BankDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Include With Room") {
                $('#BankDiv').hide();
                $('#EmployeeInfoDiv').hide();
                $('#RoomNumberDiv').hide();
                $('#AccountsPostingDiv').hide();
                $('#CompanyInfoDiv').hide();
            }
            CalculateDiscountAmount();
        }
        function OnLoadBusinessPromotionRelatedInformationFailed(error) {
            toastr.error(error.get_message());
        }

        function GetKotWiseVatNSChargeNDiscountNComplementary() {
            CommonHelper.SpinnerOpen();
            var costCenterId = "0", categoryIdList = "", discountType = "", discountPercent = "0.00", kotId = "0", margeKotId = "";
            var isComplementary = false, isInvoiceVatEnable = false, isInvoiceServiceEnable = false;

            if ($("#ContentPlaceHolder1_gvPercentageDiscountCategory tbody tr").find("td:eq(1)").find("input").is(':checked')) {
                var trLength = $("#ContentPlaceHolder1_gvPercentageDiscountCategory tbody tr").length, i = 0;

                for (i = 1; i < trLength; i++) {
                    if ($("#ContentPlaceHolder1_gvPercentageDiscountCategory tbody tr:eq(" + i + ")").find("td:eq(1)").find("input").is(':checked')) {
                        if (categoryIdList != "")
                            categoryIdList += "," + $("#ContentPlaceHolder1_gvPercentageDiscountCategory tbody tr:eq(" + i + ")").find("td:eq(3)").text();
                        else
                            categoryIdList = $("#ContentPlaceHolder1_gvPercentageDiscountCategory tbody tr:eq(" + i + ")").find("td:eq(3)").text();
                    }
                }
            }

            costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            discountType = $("#ContentPlaceHolder1_ddlDiscountType").val();
            discountPercent = $("#ContentPlaceHolder1_txtDiscountAmount").val();
            kotId = $("#ContentPlaceHolder1_hftxtKotNumber").val();
            margeKotId = $("#ContentPlaceHolder1_hfNewlyAddedKotId").val();
            isComplementary = $("#ContentPlaceHolder1_cbIsComplementary").is(":checked");
            isInvoiceServiceEnable = $("#ContentPlaceHolder1_cbServiceCharge").is(":checked");
            isInvoiceVatEnable = $("#ContentPlaceHolder1_cbVatAmount").is(":checked");

            if (discountPercent == "") {
                discountPercent = 0;
            }

            PageMethods.GetKotWiseVatNSChargeNDiscountNComplementary(costCenterId, categoryIdList, discountType, discountPercent, kotId, margeKotId, isComplementary, isInvoiceVatEnable, isInvoiceServiceEnable, OnVatScDiscountProcessSucceeded, OnVatScDiscountProcessFailed);
        }
        function OnVatScDiscountProcessSucceeded(result) {
            if (result != null) {
                $('#ContentPlaceHolder1_txtCalculatedDiscountAmount').val(result.ItemWiseDiscount);

                if ($("#ContentPlaceHolder1_hfIsRestaurantBillAmountWillRound").val() == "1") {
                    $('#ContentPlaceHolder1_txtDiscountedAmount').val(Math.round(result.DiscountedAmount));
                    $('#ContentPlaceHolder1_txtNetAmountHiddenField').val(Math.round(result.DiscountedAmount));
                    $('#ContentPlaceHolder1_txtDiscountedAmountHiddenField').val(Math.round(result.DiscountedAmount));
                    $('#ContentPlaceHolder1_txtNetAmount').val(Math.round(result.DiscountedAmount));
                }
                else {
                    $('#ContentPlaceHolder1_txtDiscountedAmount').val(result.DiscountedAmount);
                    $('#ContentPlaceHolder1_txtDiscountedAmountHiddenField').val(result.DiscountedAmount);
                    $('#ContentPlaceHolder1_txtNetAmount').val(result.DiscountedAmount);
                    $('#ContentPlaceHolder1_txtNetAmountHiddenField').val(result.DiscountedAmount);
                }

                if ($("#ContentPlaceHolder1_hfIsRestaurantBillAmountWillRound").val() == "1") {

                    if ($("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val() == 0) {
                        $("#ContentPlaceHolder1_txtGrandTotal").val(Math.round(result.GrandTotal));
                        $("#ContentPlaceHolder1_txtGrandTotalHiddenField").val(Math.round(result.GrandTotal));
                        $("#ContentPlaceHolder1_txtPaymentAmount").val($("#ContentPlaceHolder1_txtGrandTotalHiddenField").val());
                    }
                    else {
                        $("#ContentPlaceHolder1_txtGrandTotal").val(Math.round(result.ServiceRate));
                        $("#ContentPlaceHolder1_txtGrandTotalHiddenField").val(Math.round(result.ServiceRate));
                        $("#ContentPlaceHolder1_txtPaymentAmount").val($("#ContentPlaceHolder1_txtNetAmountHiddenField").val());
                    }
                }
                else {
                    if ($("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val() == 0) {
                        $("#ContentPlaceHolder1_txtGrandTotal").val(result.GrandTotal);
                        $("#ContentPlaceHolder1_txtGrandTotalHiddenField").val(result.GrandTotal);
                        $("#ContentPlaceHolder1_txtPaymentAmount").val($("#ContentPlaceHolder1_txtGrandTotalHiddenField").val());
                    }
                    else {
                        $("#ContentPlaceHolder1_txtGrandTotal").val(result.ServiceRate);
                        $("#ContentPlaceHolder1_txtGrandTotalHiddenField").val(result.ServiceRate);
                        $("#ContentPlaceHolder1_txtPaymentAmount").val($("#ContentPlaceHolder1_txtNetAmountHiddenField").val());
                    }
                }
                $("#ContentPlaceHolder1_txtServiceCharge").val(result.ServiceCharge);
                $("#ContentPlaceHolder1_txtServiceChargeAmountHiddenField").val(result.ServiceCharge);
                $("#ContentPlaceHolder1_txtVatAmount").val(result.VatAmount);
                $("#ContentPlaceHolder1_txtVatAmountHiddenField").val(result.VatAmount);
            }
            else {
                $("#ContentPlaceHolder1_txtGrandTotal").val('0');
                $("#ContentPlaceHolder1_txtGrandTotalHiddenField").val('0');
                $("#ContentPlaceHolder1_<txtServiceCharge").val('0');
                $("#ContentPlaceHolder1_txtServiceChargeAmountHiddenField").val('0');
                $("#ContentPlaceHolder1_txtVatAmount").val('0');
                $("#ContentPlaceHolder1_txtVatAmountHiddenField").val('0');
                $("#ContentPlaceHolder1_txtPaymentAmount").val('0');
            }

            if (Math.round(result.DiscountedAmount) == 0) {
                $("#PaymentBillDetails").hide();
                var cbIsComplementary = '<%=cbIsComplementary.ClientID%>'

                if ($('#' + cbIsComplementary).is(':checked')) {
                    $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', false);
                }
                else {
                    $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', true);
                }
            }
            else {
                $("#PaymentBillDetails").show();
            }

            CommonHelper.SpinnerClose();
        }
        function OnVatScDiscountProcessFailed(error) { CommonHelper.SpinnerClose(); }

        function CalculateDiscountAmount() {
            var txtDiscountAmount = '<%=txtDiscountAmount.ClientID%>'
            var txtSalesAmount = '<%=txtSalesAmountHiddenField.ClientID%>'
            var txtServiceChargeAmount = '<%=txtServiceChargeAmountHiddenField.ClientID%>'
            var txtVatAmount = '<%=txtVatAmountHiddenField.ClientID%>'
            var txtAdvanceAmount = '<%=txtAdvanceAmount.ClientID%>'
            var ddlDiscountType = '<%=ddlDiscountType.ClientID%>'
            var txtGrandTotal = '<%=txtGrandTotal.ClientID%>'
            var txtCalculatedDiscountAmount = '<%=txtCalculatedDiscountAmount.ClientID%>'
            var txtDiscountedAmount = '<%=txtDiscountedAmount.ClientID%>'
            var txtDiscountedAmountHiddenField = '<%=txtDiscountedAmountHiddenField.ClientID%>'
            var ddlBusinessPromotionId = '<%=ddlBusinessPromotionId.ClientID%>'
            var ddlMemberId = '<%=ddlMemberId.ClientID%>'

            $("#<%=txtDiscountAmount.ClientID %>").width(206);
            $("#ImgCategoryWiseDiscount").hide();

            var discountType = $('#' + ddlDiscountType).val();
            var discountAmount = $('#' + txtDiscountAmount).val();

            if (discountAmount == "") {
                discountAmount = 0;
            }
            if (isNaN(discountAmount)) {
                discountAmount = 0;
            }

            var salesAmount = $('#' + txtSalesAmount).val();
            var serviceChargeAmount = $('#' + txtServiceChargeAmount).val();
            var vatAmount = $('#' + txtVatAmount).val();
            var advanceAmount = $('#' + txtAdvanceAmount).val();

            var discount = 0;
            var discountedAmount = 0;

            if (discountType == "Fixed") {
                $("#<%=txtDiscountAmount.ClientID %>").width(175);
                $("#ImgCategoryWiseDiscount").show();
                discount = parseFloat(discountAmount);
                discountedAmount = (parseFloat(salesAmount) - parseFloat(advanceAmount)) - parseFloat(discount);

                $('#' + txtCalculatedDiscountAmount).val(parseFloat(discount));
                $("#BusinessPromotionInfoDiv").hide();
                $("#MemberInformationDiv").hide();
                $("#DiscountInformationDiv").show();
                $('#' + ddlBusinessPromotionId).val("0");
                $('#' + ddlMemberId).val("0");
            }
            else if (discountType == "Percentage") {
                $("#<%=txtDiscountAmount.ClientID %>").width(175);
                $("#ImgCategoryWiseDiscount").show();

                var categoryWiseTotalDiscount = $("#<%=hfCategoryWiseTotalDiscountAmount.ClientID %>").val();

                if (categoryWiseTotalDiscount > 0) {
                    discountedAmount = (parseFloat(salesAmount) - parseFloat(advanceAmount)) - parseFloat(categoryWiseTotalDiscount);

                    $('#' + txtCalculatedDiscountAmount).val(parseFloat(categoryWiseTotalDiscount));
                }
                else {
                    var parcentAmount = parseFloat(discountAmount) / 100;

                    discount = ((parseFloat(salesAmount) - parseFloat(advanceAmount)) * parcentAmount);
                    discountedAmount = (parseFloat(salesAmount) - parseFloat(advanceAmount)) - parseFloat(discount);

                    $('#' + txtCalculatedDiscountAmount).val(parseFloat(discount));
                }

                $("#BusinessPromotionInfoDiv").hide();
                $("#MemberInformationDiv").hide();
                $("#txtMemberCode").width(175);
                $("#ImgCategoryWiseDiscount").show();
                $("#DiscountInformationDiv").show();
                $('#' + ddlBusinessPromotionId).val("0");
                $('#' + ddlMemberId).val("0");
            }
            else if (discountType == "BusinessPromotion") {
                var categoryWiseTotalDiscount = $("#<%=hfCategoryWiseTotalDiscountAmount.ClientID %>").val();

                if (categoryWiseTotalDiscount > 0) {
                    discountedAmount = (parseFloat(salesAmount) - parseFloat(advanceAmount)) - parseFloat(categoryWiseTotalDiscount);

                    $('#' + txtCalculatedDiscountAmount).val(parseFloat(categoryWiseTotalDiscount));
                }
                else {
                    var parcentAmount = parseFloat(discountAmount) / 100;

                    discount = ((parseFloat(salesAmount) - parseFloat(advanceAmount)) * parcentAmount);
                    discountedAmount = (parseFloat(salesAmount) - parseFloat(advanceAmount)) - parseFloat(discount);

                    $('#' + txtCalculatedDiscountAmount).val(parseFloat(discount));
                }

                $('#' + txtCalculatedDiscountAmount).val(parseFloat(discount));
                $("#DiscountInformationDiv").hide();
                $("#MemberInformationDiv").hide();
                $("#<%=ddlBusinessPromotionId.ClientID %>").width(175);
                $("#ImgCategoryWiseDiscount").show();
                $("#BusinessPromotionInfoDiv").show();
                $('#' + ddlMemberId).val("0");
            }
            else if (discountType == "Member") {
                var categoryWiseTotalDiscount = $("#<%=hfCategoryWiseTotalDiscountAmount.ClientID %>").val();

                    if (categoryWiseTotalDiscount > 0) {
                        discountedAmount = (parseFloat(salesAmount) - parseFloat(advanceAmount)) - parseFloat(categoryWiseTotalDiscount);

                        $('#' + txtCalculatedDiscountAmount).val(parseFloat(categoryWiseTotalDiscount));
                    }
                    else {
                        var parcentAmount = parseFloat(discountAmount) / 100;

                        discount = ((parseFloat(salesAmount) - parseFloat(advanceAmount)) * parcentAmount);
                        discountedAmount = (parseFloat(salesAmount) - parseFloat(advanceAmount)) - parseFloat(discount);

                        $('#' + txtCalculatedDiscountAmount).val(parseFloat(discount));
                    }

                    $('#' + txtCalculatedDiscountAmount).val(parseFloat(discount));
                    $("#DiscountInformationDiv").hide();
                    $("#BusinessPromotionInfoDiv").hide();
                    $("#MemberInformationDiv").show();
                    $('#' + ddlBusinessPromotionId).val("0");
                }

    var txtNetAmount = '<%=txtNetAmount.ClientID%>'
            var txtNetAmountHiddenField = '<%=txtNetAmountHiddenField.ClientID%>'
            var txtPaymentAmount = '<%=txtPaymentAmount.ClientID%>'

            if ($("#<%=hfIsRestaurantBillAmountWillRound.ClientID %>").val() == "1") {
                $('#' + txtDiscountedAmount).val(Math.round(discountedAmount));
                $('#' + txtDiscountedAmountHiddenField).val(Math.round(discountedAmount));

                $('#' + txtNetAmount).val(Math.round(discountedAmount));
                $('#' + txtNetAmountHiddenField).val(Math.round(discountedAmount));
                $('#' + txtPaymentAmount).val(Math.round(discountedAmount));
            }
            else {
                $('#' + txtDiscountedAmount).val(discountedAmount);
                $('#' + txtDiscountedAmountHiddenField).val(discountedAmount);

                $('#' + txtNetAmount).val(discountedAmount);
                $('#' + txtNetAmountHiddenField).val(discountedAmount);
                $('#' + txtPaymentAmount).val(discountedAmount);
            }


            if (Math.round(discountedAmount) == 0) {
                $("#PaymentBillDetails").hide();
            }
            else {
                $("#PaymentBillDetails").show();
            }

            TotalRoomRateVatServiceChargeCalculation();
        }

        function ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(ctrl) {
            GetKotWiseVatNSChargeNDiscountNComplementary();
        }

        function ToggleTotalRoomRateVatServiceChargeCalculationForVat(ctrl) {
            GetKotWiseVatNSChargeNDiscountNComplementary();
        }

        function ToggleWaiterInformation(ctrl) {
            if (ctrl == 1) {
                $("#ddlWaiterNameDisplayDiv").show();
                $("#txtWaiterNameDisplayDiv").hide();
                $("#<%=cbTxtWaiterNameDisplay.ClientID %>").attr('checked', false);
                $("#<%=cbDdlWaiterNameDisplay.ClientID %>").attr('checked', true);
                $("#<%=hfIsCustomBearerInfoEnable.ClientID %>").val("1");

            }
            else {
                $("#txtWaiterNameDisplayDiv").show();
                $("#ddlWaiterNameDisplayDiv").hide();
                $("#<%=cbTxtWaiterNameDisplay.ClientID %>").attr('checked', true);
                $("#<%=cbDdlWaiterNameDisplay.ClientID %>").attr('checked', false);
                $("#<%=hfIsCustomBearerInfoEnable.ClientID %>").val("0");
            }
        }

        function TotalRoomRateVatServiceChargeCalculation() {
            var txtDiscountedAmountHiddenField = '<%=txtDiscountedAmountHiddenField.ClientID%>'
            var cbServiceCharge = '<%=cbServiceCharge.ClientID%>'
            var cbVatAmount = '<%=cbVatAmount.ClientID%>'

            var cbServiceChargeVal = "1";
            if ($('#' + cbServiceCharge).is(':checked')) {
                cbServiceChargeVal = "1";
            }
            else {
                cbServiceChargeVal = "0";
            }

            var cbVatAmountVal = "1";
            if ($('#' + cbVatAmount).is(':checked')) {
                cbVatAmountVal = "1";
            }
            else {
                cbVatAmountVal = "0";
            }

            var txtRoomRateVal = parseFloat($('#' + txtDiscountedAmountHiddenField).val());
            var InclusiveBill = 0, Vat = 0.00, ServiceCharge = 0.00;

            if ($("#<%=hfIsRestaurantBillInclusive.ClientID %>").val() != "")
            { InclusiveBill = parseInt($("#<%=hfIsRestaurantBillInclusive.ClientID %>").val(), 10); }

            if ($("#<%=hfRestaurantVatAmount.ClientID %>").val() != "")
                Vat = parseFloat($("#<%=hfRestaurantVatAmount.ClientID %>").val());

            if ($("#<%=hfRestaurantServiceCharge.ClientID %>").val() != "")
                ServiceCharge = parseFloat($("#<%=hfRestaurantServiceCharge.ClientID %>").val());

            var InnboardRateCalculationInfo = CommonHelper.GetRackRateServiceChargeVatInformation(InclusiveBill, Vat, ServiceCharge, 0.00, txtRoomRateVal, parseInt(cbServiceChargeVal, 10), 0.00, parseInt(cbVatAmountVal, 10));
            OnLoadRackRateServiceChargeVatInformationSucceeded(InnboardRateCalculationInfo);
        }

        function OnLoadRackRateServiceChargeVatInformationSucceeded(result) {
            tt = result;
            if (result.RackRate != 'NaN') {
                if ($("#<%=hfIsRestaurantBillAmountWillRound.ClientID %>").val() == "1") {
                    $("#<%=txtGrandTotal.ClientID %>").val(Math.round(result.RackRate));
                    $("#ContentPlaceHolder1_txtGrandTotalHiddenField").val(Math.round(result.RackRate));

                    if ($("#<%=hfIsRestaurantBillInclusive.ClientID %>").val() == 0) {
                        $("#<%=txtPaymentAmount.ClientID %>").val(Math.round(result.RackRate));
                    }
                }
                else {
                    $("#<%=txtGrandTotal.ClientID %>").val(result.RackRate);
                    $("#ContentPlaceHolder1_txtGrandTotalHiddenField").val(result.RackRate);

                    if ($("#<%=hfIsRestaurantBillInclusive.ClientID %>").val() == 0) {
                        $("#<%=txtPaymentAmount.ClientID %>").val(result.RackRate);
                    }
                }
                $("#<%=txtServiceCharge.ClientID %>").val(result.ServiceCharge);
                $("#<%=txtServiceChargeAmountHiddenField.ClientID %>").val(result.ServiceCharge);
                $("#<%=txtVatAmount.ClientID %>").val(result.VatAmount);
                $("#<%=txtVatAmountHiddenField.ClientID %>").val(result.VatAmount);
            }
            else {
                $("#<%=txtGrandTotal.ClientID %>").val('0');
                $("#<%=txtGrandTotalHiddenField.ClientID %>").val('0');
                $("#<%=txtServiceCharge.ClientID %>").val('0');
                $("#<%=txtServiceChargeAmountHiddenField.ClientID %>").val('0');
                $("#<%=txtVatAmount.ClientID %>").val('0');
                $("#<%=txtVatAmountHiddenField.ClientID %>").val('0');
                $("#<%=txtPaymentAmount.ClientID %>").val('0');
            }
            return false;
        }

        function LoadOccupiedTable() {
            var costCenterId = $("#<%=hfCostCenterId.ClientID %>").val();
            var tableId = $("#ContentPlaceHolder1_SourceIdHiddenField").val();

            var alreadyAddedTable = $("#<%=hfAlreadyAddedTable.ClientID %>").val();
            var alreadyAddedKot = $("#<%=hfAlreadyAddedKotId.ClientID %>").val();

            if (alreadyAddedTable != "") {
                alreadyAddedTableList = alreadyAddedTable.split(',');
                alreadyAddedKotList = alreadyAddedKot.split(',');
            }

            PageMethods.GetAlreadyOccupiedTable(costCenterId, tableId, OnOccupiedTableLoadSucceeded, OccupiedTableLoadFailed);
            return false;
        }

        function OnOccupiedTableLoadSucceeded(result) {
            $("#OccupiedTableContainer").html(result);
            var tableId = $("#<%=hfSelectedTableId.ClientID %>").val();

            $("#OccupiedTableContainer").dialog({
                width: 300,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", ////TODO add title 
                show: 'slide'
            });


            if (tableId != "") {
                tableId = tableId.split(',');

                $('#TableWiseItemInformation tbody input').each(function () {

                    var id = $.inArray($(this).attr('value'), tableId);

                    if (id > -1) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }

        function OccupiedTableLoadFailed(error) {

        }

        function GoToTableDesign() {
            window.location = "frmKotBillMaster.aspx?Kot=TableAllocation";
            return false;
        }

        function GoToRoomDesign() {
            window.location = "frmGuestRoomKotBill.aspx?Kot=RoomAllocation";
            return false;
        }

        function GoToHomePanel() {
            var homePanelInfo = $("#<%=hfHomePanelInfo.ClientID %>").val();
            window.location = homePanelInfo;
            return false;
        }

        function GoToTokenSystem() {
            window.location = "frmTokenKotBill.aspx?Kot=RestaurantItemCategory:0";
            return false;
        }

        function GetCheckedTable() {
            var selectdTableId = "";
            var selectdTableNumber = "";
            var deletedTableId = "", hfDeletedKotId = "";
            var newlyAddedTableId = "";
            var alreadyAddedTableIndex = 0;

            $("#<%=hfSelectedTableId.ClientID %>").val("");

            $('#TableWiseItemInformation input').each(function () {

                if ($(this).is(':checked')) {

                    if (selectdTableId != "") {
                        selectdTableId += ',' + $(this).attr('value');
                        selectdTableNumber += ',' + $(this).attr('name');
                    }
                    else {
                        selectdTableId += $(this).attr('value');
                        selectdTableNumber += $(this).attr('name');
                    }

                    alreadyAddedTableIndex = $.inArray($(this).attr('value'), alreadyAddedTableList);

                    if (alreadyAddedTableIndex < 0) {
                        if (newlyAddedTableId != "") {
                            newlyAddedTableId += "," + $(this).attr('value');
                        }
                        else {
                            newlyAddedTableId = $(this).attr('value');
                        }
                    }

                }
                else {
                    alreadyAddedTableIndex = $.inArray($(this).attr('value'), alreadyAddedTableList);

                    if (alreadyAddedTableIndex >= 0) {

                        if (deletedTableId != "") {
                            deletedTableId += "," + $(this).attr('value');
                            hfDeletedKotId += "," + alreadyAddedKotList[alreadyAddedTableIndex];
                        }
                        else {
                            deletedTableId = $(this).attr('value');
                            hfDeletedKotId = alreadyAddedKotList[alreadyAddedTableIndex];
                        }
                    }
                }
            });

            $("#<%=hfSelectedTableNumber.ClientID %>").val(selectdTableNumber);
            $("#<%=hfSelectedTableId.ClientID %>").val(selectdTableId);
            $("#AddedNewTables").text(selectdTableNumber);

            $("#<%=hfNewlyAddedTableId.ClientID %>").val(newlyAddedTableId);
            $("#<%=hfDeletedTableId.ClientID %>").val(deletedTableId);
            $("#<%=hfDeletedKotId.ClientID %>").val(hfDeletedKotId);

            if (selectdTableNumber != "")
                $("#AlreadyAddedNewTable").show();
            else
                $("#AlreadyAddedNewTable").hide();

            $("#OccupiedTableContainer").dialog("close"); ////TODO (if popup not closed)
            var costCenterId = $("#<%=hfCostCenterId.ClientID %>").val();
            $('#btnUpdateBillPayment').trigger('click');
        }

        function ComplementaryBill(val) {
            var lblComplementaryRoom = '<%=lblComplementaryRoom.ClientID%>'
            var ddlComplementaryRoomId = '<%=ddlComplementaryRoomId.ClientID%>'

            if ($(val).is(':checked')) {
                $('#' + lblComplementaryRoom).show();
                $('#' + ddlComplementaryRoomId).show();
                $("#ContentPlaceHolder1_ddlDiscountType").val("Percentage");
                $("#ContentPlaceHolder1_txtDiscountAmount").val("100");
                $("#ContentPlaceHolder1_hfCategoryWiseTotalDiscountAmount").val("0");

                var trLength = $("#ContentPlaceHolder1_gvPercentageDiscountCategory tbody tr").length
                var i = 0;
                for (i = 1; i < trLength; i++) {
                    if ($("#ContentPlaceHolder1_gvPercentageDiscountCategory tbody tr:eq(" + i + ")").find("td:eq(1)").find("input").is(':checked')) {
                        $("#ContentPlaceHolder1_gvPercentageDiscountCategory tbody tr:eq(" + i + ")").find("td:eq(1)").find("input").prop("checked", false);
                    }
                }

                CalculateDiscountAmount();
                $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', false);
                $("#ContentPlaceHolder1_GuestPaymentInformationDiv").hide();
                $("#ContentPlaceHolder1_chkIsBillSplit").hide();
                $("#ContentPlaceHolder1_lblIsBillSplit").hide();
            }
            else {
                $('#' + lblComplementaryRoom).hide();
                $('#' + ddlComplementaryRoomId).hide();
                $("#ContentPlaceHolder1_txtDiscountAmount").val("0");
                $("#ContentPlaceHolder1_hfCategoryWiseTotalDiscountAmount").val("0");
                CalculateDiscountAmount();
                $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', true);
                $("#ContentPlaceHolder1_GuestPaymentInformationDiv").show();
                $("#ContentPlaceHolder1_chkIsBillSplit").show();
                $("#ContentPlaceHolder1_lblIsBillSplit").show();
                $("#ContentPlaceHolder1_txtCustomerName").val("");
            }

            GetKotWiseVatNSChargeNDiscountNComplementary();
        }

        function ValidateFormBeforeSettlement() {
            var paymentHas = $("#GuestPaymentDetailGrid").has('table').length;
            if (paymentHas > 0) {
                $("#<%=hfGeneratePaymentGridAfterBillPriview.ClientID %>").val("1");
            }
            else { $("#<%=hfGeneratePaymentGridAfterBillPriview.ClientID %>").val("0"); }

        }
        function ValidateForm() {
            var changeAmount = $("#<%=hfChangeAmount.ClientID %>").val();
            if (changeAmount > 0) {
                return confirm('Are you confirm to settle the bill with change amount?');
            }
            else return confirm('Are you confirm to settle the bill?');
        }

        function ClosePopup() {
            $("#OccupiedTableContainer").dialog("close");
        }

        function ToggleBillSplitFieldVisible(ctrl) {
            if ($(ctrl).is(':checked')) {
                $("#BillSplitPopUpForm").dialog({
                    width: 750,
                    height: 530,
                    autoOpen: true,
                    //modal: true,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    title: "Bill Split Information",
                    show: 'slide'
                });
            }
        }

        $(function () {
            $("#left").bind("click", function () {
                var options = $("[id*=lstRight] option:selected");
                for (var i = 0; i < options.length; i++) {
                    var opt = $(options[i]).clone();
                    $(options[i]).remove();
                    $("[id*=lstLeft]").append(opt);
                }
            });
            $("#right").bind("click", function () {
                var options = $("[id*=lstLeft] option:selected");
                for (var i = 0; i < options.length; i++) {
                    var opt = $(options[i]).clone();
                    $(options[i]).remove();
                    $("[id*=lstRight]").append(opt);
                }
            });
        });
    </script>
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfLocalCurrencyId" runat="server" />
    <asp:HiddenField ID="hfIsCustomBearerInfoEnable" runat="server" />
    <asp:HiddenField ID="hfHomePanelInfo" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCostCenterId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfComplimentaryRoomNumber" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRestaurantBillInclusive" runat="server" />
    <asp:HiddenField ID="hfIsVatServiceChargeEnable" runat="server" />
    <asp:HiddenField ID="hfRestaurantVatAmount" runat="server" />
    <asp:HiddenField ID="hfRestaurantServiceCharge" runat="server" />
    <asp:HiddenField ID="hfGeneratePaymentGridAfterBillPriview" runat="server" />
    <asp:HiddenField ID="hfIsPaidServiceAddedForPayment" runat="server" />
    <asp:HiddenField ID="hfPaidServiceDiscount" runat="server" />
    <asp:HiddenField ID="hfPaxQuantityHiddenField" runat="server" />
    <asp:HiddenField ID="hfCategoryWiseTotalDiscountAmount" runat="server" />
    <asp:HiddenField ID="hfIsRestaurantBillAmountWillRound" runat="server" />
    <asp:HiddenField ID="hfChangeAmount" runat="server" />
    <div class="childDivSection" id="BillSplitPopUpForm" style="display: none;">
        <div class="panel-body">
            <div style="padding-bottom: 20px;">
                <div class="row">
                    <div class="col-md-5">
                        <asp:ListBox ID="lstLeft" runat="server" SelectionMode="Multiple" Width="100%" Height="380px"></asp:ListBox>
                        <input type="button" id="btnLstLeftBillSplitPrintPreview" value="Print Preview" class="btn btn-primary" />
                    </div>
                    <div class="col-md-2">
                        <div style="padding-top: 150px;">
                            <input type="button" id="right" value=">>" style="width: 95px;" class="btn btn-primary" />
                        </div>
                        <div style="padding-top: 2px;">
                            <input type="button" id="left" value="<<" style="width: 95px;" class="btn btn-primary" />
                        </div>
                    </div>
                    <div class="col-md-5">
                        <asp:ListBox ID="lstRight" runat="server" SelectionMode="Multiple" Width="100%" Height="380px"></asp:ListBox>
                        <input type="button" id="btnLstRightBillSplitPrintPreview" value="Print Preview"
                            class="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="btnNewBank" class="btn-toolbar;" style="text-align: right;">
        <asp:ImageButton ID="imgBtnTableDesign" CssClass="btnBackPreviousPage" runat="server"
            ImageUrl="~/Images/TableDesign.png" OnClientClick="javascript:return GoToTableDesign()"
            ToolTip="Table Design" />
        &nbsp;
        <asp:ImageButton ID="imgBtnRoomDesign" CssClass="btnBackPreviousPage" runat="server"
            ImageUrl="~/Images/Home.png" OnClientClick="javascript:return GoToRoomDesign()"
            ToolTip="Room Design" />
        <div class="btn-group">
        </div>
    </div>
    <div id="PercentageDiscountDialog" style="width: 450px; display: none;">
        <div class="">
            <div class="DataGridYScroll">
                <div id="PercentageDiscountContainer" style="width: 100%;">
                </div>
                <asp:GridView ID="gvPercentageDiscountCategory" Width="100%" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="300" AllowSorting="True"
                    ForeColor="#333333" OnRowDataBound="gvPercentageDiscountCategory_RowDataBound"
                    CssClass="table table-bordered table-condensed table-responsive">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="IDNO" HeaderStyle-CssClass="invisible" ItemStyle-CssClass="invisible">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("CategoryId") %>'></asp:Label>
                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("ActiveStat") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Checked" ItemStyle-Width="05%">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ChkChecked" CssClass="ChkCreate" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsCheckedPermission" CssClass="chk_View" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Category" ItemStyle-Width="40%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CategoryId" HeaderStyle-CssClass="invisible" ItemStyle-CssClass="invisible"></asp:BoundField>
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
            <div id="Div4" style="padding-left: 5px; width: 100%; margin-top: 5px;">
                <div style="float: left; padding-bottom: 15px;">
                    <input type="button" id="btnCategoryWiseDiscountOK" class="btn btn-primary" style="width: 80px;"
                        value="Ok" />
                    <input type="button" id="btnCategoryWiseDiscountCancel" class="btn btn-primary" style="width: 80px;"
                        value="Cancel" />
                </div>
                <div id="ItemWiseSpecialRemarksContainer" class="alert alert-info" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert">
                        ×</button>
                    <asp:Label ID='ItemWiseSpecialRemarksMessage' Font-Bold="True" runat="server" ClientIDMode="Static"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnUpdateBillPayment" runat="server" Text="Button" ClientIDMode="Static"
            OnClick="btnUpdateBillPayment_Click" />
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Bill Detailed Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div id="myTabs">
                    <ul id="tabPage" class="ui-style">
                        <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-1">Bill Summary</a></li>
                        <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-2">Item Information</a></li>
                    </ul>
                    <div id="tab-1">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblKotNumberDisplay" runat="server" class="control-label" Text="KOT Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtKotNumberDisplay" TabIndex="1" CssClass="form-control" runat="server"
                                    ReadOnly="True"></asp:TextBox>
                                <asp:HiddenField ID="hftxtKotNumber" runat="server"></asp:HiddenField>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblWaiterName" runat="server" class="control-label" Text="Waiter Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <div id="txtWaiterNameDisplayDiv">
                                    <div class="row">
                                        <div class="col-md-11">
                                            <asp:TextBox ID="txtWaiterNameDisplay" CssClass="form-control" TabIndex="1" runat="server"
                                                ReadOnly="True"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1 col-padding-left-none">
                                            <asp:CheckBox ID="cbTxtWaiterNameDisplay" runat="server" Text="" onclick="javascript: return ToggleWaiterInformation(1);"
                                                TabIndex="8" />
                                        </div>
                                    </div>
                                </div>
                                <div id="ddlWaiterNameDisplayDiv" style="display: none;">
                                    <div class="row">
                                        <div class="col-md-11">
                                            <asp:DropDownList ID="ddlWaiterNameDisplay" runat="server" CssClass="form-control"
                                                TabIndex="16">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-1 col-padding-left-none">
                                            <asp:CheckBox ID="cbDdlWaiterNameDisplay" runat="server" Text="" onclick="javascript: return ToggleWaiterInformation(0);"
                                                TabIndex="8" Checked="True" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="SourceIdHiddenField" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblName" runat="server" class="control-label" Text="Sales Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSalesAmount" TabIndex="1" runat="server" CssClass="form-control"
                                    ReadOnly="True"></asp:TextBox>
                                <asp:HiddenField ID="txtSalesAmountHiddenField" runat="server"></asp:HiddenField>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblAdvanceAmount" runat="server" class="control-label" Text="Advance Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAdvanceAmount" TabIndex="1" runat="server" CssClass="form-control"
                                    ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                        <asp:Panel ID="pnlDiscountInformation" runat="server">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblDiscountType" runat="server" class="control-label" Text="Discount Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control" TabIndex="16">
                                        <asp:ListItem>Fixed</asp:ListItem>
                                        <asp:ListItem>Percentage</asp:ListItem>
                                        <asp:ListItem>Member</asp:ListItem>
                                        <asp:ListItem Value="BusinessPromotion">Business Promotion</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div id="DiscountInformationDiv">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblDiscountAmount" runat="server" class="control-label" Text="Discount Amount"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtDiscountAmount" TabIndex="3" CssClass="form-control quantitydecimal" runat="server"> </asp:TextBox>
                                                <asp:HiddenField ID="txtCalculatedDiscountAmount" runat="server"></asp:HiddenField>
                                            </div>
                                            <div class="col-md-2 col-padding-left-none">
                                                <span style="margin-left: 3px;">
                                                    <img id="" src='../Images/service.png' title='Category Wise Discount' style="cursor: pointer;"
                                                        onclick='javascript:return LoadPercentageDiscountInfo()' alt='Category Wise Discount'
                                                        border='0' /></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="BusinessPromotionInfoDiv" style="display: none;">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Promotion Name"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-10">
                                                <asp:DropDownList ID="ddlBusinessPromotionId" runat="server" CssClass="form-control"
                                                    TabIndex="16">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2 col-padding-left-none">
                                                <span style="margin-left: 3px;">
                                                    <img id="Img1" src='../Images/service.png' title='Category Wise Discount' style="cursor: pointer;"
                                                        onclick='javascript:return LoadPercentageDiscountInfo()' alt='Category Wise Discount'
                                                        border='0' /></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="MemberInformationDiv" style="display: none;">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblMemberId" runat="server" class="control-label required-field" Text="Member Code"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-10">
                                                <input id="txtMemberCode" class="form-control" type="text" />
                                                <div style="display: none;">
                                                    <asp:HiddenField ID="hfTxtMemberCode" runat="server"></asp:HiddenField>
                                                    <asp:DropDownList ID="ddlMemberId" runat="server" CssClass="form-control" TabIndex="16">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-2 col-padding-left-none">
                                                <span style="margin-left: 3px;">
                                                    <img src='../Images/service.png' title='Member Detail Search' style="cursor: pointer;"
                                                        onclick='javascript:return LoadMemberSearchInfo()' alt='Member Detail Search'
                                                        border='0' /></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDiscountedAmount" runat="server" class="control-label" Text="After Discount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="txtDiscountedAmountHiddenField" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtDiscountedAmount" TabIndex="4" runat="server" CssClass="form-control"
                                    Enabled="False"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPaxQuantity" runat="server" class="control-label" Text="Pax"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPaxQuantity" TabIndex="1" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <asp:Panel ID="pnlRackRateServiceChargeVatInformation" runat="server">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:HiddenField ID="txtKotId" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblServiceCharge" runat="server" class="control-label" Text="Service Charge"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-11">
                                            <asp:TextBox ID="txtServiceCharge" runat="server" TabIndex="22" CssClass="form-control"
                                                Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="txtServiceChargeAmountHiddenField" runat="server"></asp:HiddenField>
                                        </div>
                                        <div class="col-md-1 col-padding-left-none">
                                            <asp:CheckBox ID="cbServiceCharge" runat="server" Text="" onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                TabIndex="8" Checked="True" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblVatAmount" runat="server" class="control-label" Text="Vat Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-11">
                                            <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="23" CssClass="form-control"
                                                Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="txtVatAmountHiddenField" runat="server"></asp:HiddenField>
                                        </div>
                                        <div class="col-md-1 col-padding-left-none">
                                            <asp:CheckBox ID="cbVatAmount" runat="server" Text="" onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForVat(this);"
                                                TabIndex="8" Checked="True" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblGrandTotal" runat="server" class="control-label" Text="Net Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="txtGrandTotalHiddenField" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtGrandTotal" TabIndex="4" runat="server" CssClass="form-control"
                                    Enabled="False"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Panel ID="pnlAlreadyAddedNewTable" runat="server">
                                    <div id="AlreadyAddedNewTable" runat="server" clientidmode="Static" style="display: none;">
                                        Table Added: <span id="AddedNewTables" runat="server" clientidmode="Static" class="ThreeColumnTextBox"></span>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="col-md-4">
                                <asp:Panel ID="AddMoreTableForBillPanel" runat="server">
                                    <input type="button" id="addMoreTableForBill" onclick="LoadOccupiedTable()" class="btn btn-primary"
                                        value="Add More Table" />
                                </asp:Panel>
                            </div>
                        </div>
                        <div id="NetAmountDivInfo" runat="server">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNetAmount" runat="server" class="control-label" Text="Grand Total"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNetAmount" TabIndex="4" runat="server" CssClass="form-control"
                                        Enabled="False"></asp:TextBox>
                                    <asp:HiddenField ID="txtNetAmountHiddenField" runat="server"></asp:HiddenField>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                </div>
                                <div class="col-md-4">
                                    <asp:Panel ID="pnlHomeButtonInfo" runat="server">
                                        <div id="Div2" class="btn-toolbar;" style="text-align: right;">
                                            <asp:ImageButton ID="ImageButton1" CssClass="btnBackPreviousPage" runat="server"
                                                ImageUrl="~/Images/Home.png" OnClientClick="javascript:return GoToHomePanel()"
                                                ToolTip="Home" />
                                            &nbsp;
                                            <div class="btn-group">
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblIsComplementary" runat="server" class="control-label" Text="Is Complementary"></asp:Label>
                            </div>
                            <div class="col-md-1">
                                <asp:CheckBox ID="cbIsComplementary" runat="server" Style="margin-top: 2px;" onclick="javascript: return ComplementaryBill(this);" />
                                <asp:Label ID="lblComplementaryRoom" runat="server" class="control-label" Text="For"></asp:Label>
                            </div>
                            <div class="col-md-3" style="padding-left: 0;">
                                <asp:DropDownList ID="ddlComplementaryRoomId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="tab-2">
                        <asp:GridView ID="gvRestaurentBill" Width="100%" runat="server" AutoGenerateColumns="False"
                            CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowDataBound="gvRestaurentBill_RowDataBound"
                            CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("KotId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-Width="30%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ItemUnit" HeaderText="Unit" ItemStyle-Width="8%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Unit Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitRate" runat="server" Text='<%# bind("UnitRate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="12%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S. Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# bind("ServiceRate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="14%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S. Charge">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceCharge" runat="server" Text='<%# bind("ServiceCharge") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="12%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vat">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVatAmount" runat="server" Text='<%# bind("VatAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="12%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line Total">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# bind("ItemLineTotal") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="14%"></ItemStyle>
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
                        <div style="text-align: right; padding-top: 10px;">
                            <asp:Button ID="btnAddMoreBill" runat="server" TabIndex="12" Text="Add More Item"
                                CssClass="btn btn-primary btn-sm" OnClientClick="javascript:return GoToTokenSystem()" />
                        </div>
                    </div>
                </div>
                <div class="form-group" style="padding-top: 5px; padding-bottom: 5px;">
                    <div class="col-md-2">
                        <asp:HiddenField ID="txtBillId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtBillIdForInvoicePreview" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfIsRestaurantIntegrateWithFrontOffice" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblCustomerName" runat="server" class="control-label" Text="Guest Information"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtCustomerName" runat="server" TabIndex="5" CssClass="form-control"
                            TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" Width="150px" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"
                            TabIndex="23" OnClick="btnSave_Click" OnClientClick="javascript: return ValidateFormBeforeSettlement();" />
                        <asp:Button ID="btnClear" runat="server" TabIndex="24" Text="Clear" CssClass="btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" OnClick="btnClear_Click"
                            Visible="False" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="chkIsBillSplit" runat="Server" Text="" Font-Bold="true" onclick="javascript: return ToggleBillSplitFieldVisible(this);"
                            TextAlign="right" />
                        &nbsp;&nbsp;<asp:Label ID="lblIsBillSplit" runat="server" class="control-label" Text="Is Bill Split?"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div id="GuestPaymentInformationDiv" runat="server" class="panel panel-default">
            <div class="panel-heading">
                Guest Payment Information
            </div>
            <div id="PaymentBillDetails" class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblPayMode" runat="server" class="control-label required-field" Text="Payment Mode"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlPayMode" runat="server" TabIndex="6" CssClass="form-control">
                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem>Cash</asp:ListItem>
                                <asp:ListItem>Card</asp:ListItem>
                                <asp:ListItem>Cheque</asp:ListItem>
                                <asp:ListItem Value="Other Room">Guest Room</asp:ListItem>
                                <asp:ListItem>Employee</asp:ListItem>
                                <asp:ListItem>Company</asp:ListItem>
                                <asp:ListItem>Member</asp:ListItem>
                                <%--<asp:ListItem>Refund</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblCurrencyType" runat="server" class="control-label required-field"
                                Text="Currency Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCurrency" TabIndex="6" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                            <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                        </div>
                    </div>
                    <div id="RoomNumberDiv" style="display: none;">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblRoomNumber" runat="server" class="control-label required-field" Text="Room Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomId" TabIndex="12" runat="server" CssClass="form-control"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSrcRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomNumberId" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblPaidService" runat="server" class="control-label" Text="Paid Service"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPaidService" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblPaymentAmount" runat="server" class="control-label required-field"
                                Text="Payment Amount"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtPaymentAmount" TabIndex="9" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
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
                    <div id="BankDiv">
                        <div style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblBankName" runat="server" class="control-label" Text="Bank Name"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlBankName" TabIndex="7" runat="server" CssClass="form-control"
                                        AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Card Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCardType" TabIndex="8" runat="server" CssClass="form-control">
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
                                <asp:TextBox ID="txtCardNumber" TabIndex="9" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblBankId" runat="server" class="control-label required-field" Text="Bank Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <input id="txtBankId" type="text" class="form-control" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlBankId" CssClass="form-control" runat="server" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label2" runat="server" class="control-label" Text="Expiry Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtExpireDate" TabIndex="10" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label3" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCardHolderName" TabIndex="11" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="EmployeeInfoDiv" style="display: none;">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="IsRestaurantIntegrateWithPayrollVal" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblEmployeeName" runat="server" class="control-label required-field" Text="Employee Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlEmpId" TabIndex="12" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="CompanyInfoDiv" style="display: none;">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCompanyName" runat="server" class="control-label required-field" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCompanyName" TabIndex="12" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblChequeBankId" runat="server" class="control-label required-field"
                                    Text="Bank Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <input id="txtChequeBankId" class="form-control" type="text" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlChequeBankId" runat="server" CssClass="form-control" AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control"
                                        TabIndex="6">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblChecqueNumber" runat="server" class="control-label required-field" Text="Cheque Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
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
                    </div>
                    <div id="EmployeePaymentAccountHeadDiv" style="display: none;">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Accounts Posting Head"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEmployeePaymentAccountHead" runat="server" CssClass="form-control"
                                    TabIndex="6">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="Label4" runat="server" class="control-label" Text="Account Head"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <div id="ddlCashReceiveAccountsInfoDiv">
                                <asp:DropDownList ID="ddlCashReceiveAccountsInfo" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div id="ddlCardReceiveAccountsInfoDiv" style="display: none;">
                                <asp:DropDownList ID="ddlCardReceiveAccountsInfo" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
                                <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="RefundDiv" style="display: none;">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRefundAccountHead" runat="server" class="control-label" Text="Account Head"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRefundAccountHead" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" tabindex="37" id="btnAddDetailGuestPayment" value="Add" class="btn btn-primary btn-sm" />
                            <asp:Label ID="lblHiddenIdDetailGuestPayment" runat="server" class="control-label"
                                Text='' Visible="False"></asp:Label>
                        </div>
                    </div>
                    <div id="GuestPaymentDetailGrid" class="childDivSection">
                    </div>
                    <div id="TotalPaid" class="totalAmout">
                    </div>
                    <div id="dueTotal" class="totalAmout">
                    </div>
                    <div>
                        <asp:Label ID="AlartMessege" runat="server" Style="color: Red;" Text='Grand Total and Guest Payment Amount is not Equal.'
                            CssClass="totalAmout" Visible="true"></asp:Label>
                    </div>
                    <div class="childDivSection">
                        <div id="AccountsPostingDiv" class="panel panel-default" style="display: none">
                            <div class="panel-heading">
                                Accounts Posting Information
                            </div>
                            <div class="panel-body">
                                <div id="ActionPanel">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblPaymentAccountHead" runat="server" class="control-label" Text="Account Head"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <div id="CashPaymentAccountHeadDiv">
                                                    <asp:DropDownList ID="ddlCashPaymentAccountHead" TabIndex="17" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div id="BankPaymentAccountHeadDiv" style="display: none;">
                                                    <asp:DropDownList ID="ddlBankPaymentAccountHead" TabIndex="18" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblReceiveAmount" runat="server" class="control-label" Text="Receive Amount"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtReceiveAmount" runat="server" Enabled="false" CssClass="form-control"
                                                    TabIndex="19"> </asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblConvertionRate" runat="server" class="control-label" Text="Convertion Rate"
                                                    Visible="false"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtConvertionRate" runat="server" Visible="false" CssClass="form-control"
                                                    TabIndex="20"> </asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblGLCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlGLCompany" TabIndex="21" CssClass="form-control" runat="server"
                                                    onchange="PopulateProjects();">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblGLProject" runat="server" class="control-label" Text="Project"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlGLProject" runat="server" CssClass="form-control" TabIndex="22">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="btnPaymentPosting" Width="150px" runat="server" Text="Bill Settlement"
                    CssClass="btn btn-primary btn-sm" TabIndex="23" OnClick="btnFinalSave_Click"
                    OnClientClick="javascript: return ValidateForm();" />
            </div>
        </div>
    </div>
    <div id="DivTableSelect" style="display: none;">
        <div id="Div1" class="panel panel-default">
            <asp:HiddenField ID="hfSelectedTableNumber" runat="server" />
            <asp:HiddenField ID="hfSelectedTableId" runat="server" />
            <asp:HiddenField ID="hfAlreadyAddedTable" runat="server" />
            <asp:HiddenField ID="hfAlreadyAddedKotId" runat="server" />
            <asp:HiddenField ID="hfNewlyAddedTableId" runat="server" />
            <asp:HiddenField ID="hfNewlyAddedKotId" runat="server" />
            <asp:HiddenField ID="hfDeletedTableId" runat="server" />
            <asp:HiddenField ID="hfDeletedKotId" runat="server" />
            <div style="height: 425px; overflow-y: scroll; text-align: left;">
                <div id="OccupiedTableContainer">
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var rn = '<%=isRoomNumberBoxEnable%>';
        if (rn > -1) {
            RoomNumberPanelShow();
        }
        else {
            RoomNumberPanelHide();
        }

        var xIsBillProcessedBoxEnable = '<%=isBillProcessedBoxEnable%>';
        if (xIsBillProcessedBoxEnable > -1) {
            $('#addMoreTableForBill').hide();
        }
        else {
            $('#addMoreTableForBill').show();
        }

        var rnisBillProcessedForTokenSystem = '<%=isBillProcessedForTokenSystem%>';
        if (rnisBillProcessedForTokenSystem > -1) {
            $("#PaymentBillDetails").show();
        }

        var isBillProcessedForMember = '<%=isBillProcessedForMember%>';
        if (isBillProcessedForMember > -1) {
            $("#txtMemberCode").val($("#<%=hfTxtMemberCode.ClientID %>").val());
        }

        $(document).ready(function () {
            var cbIsComplementary = '<%=cbIsComplementary.ClientID%>'

            if ($('#' + cbIsComplementary).is(':checked')) {
                $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', false);
            }
            else {
                $('#ContentPlaceHolder1_btnPaymentPosting').attr('disabled', true);
            }

            if ($("#ContentPlaceHolder1_txtGrandTotalHiddenField").val() == "0") {
                $("#PaymentBillDetails").hide();
            }
            else {
                $("#PaymentBillDetails").show();
            }

            var ddlDiscountTypeVal = $("#<%=ddlDiscountType.ClientID %>").val();
            if (ddlDiscountTypeVal == "BusinessPromotion") {
                $("#DiscountInformationDiv").hide();
                $("#MemberInformationDiv").hide();
                $("#ddlBusinessPromotionI").width(175);
                $("#ImgCategoryWiseDiscount").show();
                $("#BusinessPromotionInfoDiv").show();
                $('#ContentPlaceHolder1_ddlMemberId').val("0");
            }
        });
    </script>
</asp:Content>
