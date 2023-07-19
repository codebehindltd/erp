<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmHoldRoomCheckOut.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmHoldRoomCheckOut" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .mycheckbox input[type="checkbox"]
        {
            margin-right: 10px;
            padding-top: 5px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
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

            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtCompanyBank", "ContentPlaceHolder1_ddlCompanyBank", "ContentPlaceHolder1_ddlCompanyBank");

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

            $('#btnDayLetsClear').click(function () {
                $('#' + ddlDayLetDiscount).val('Percentage');
                $('#' + txtDayLetDiscount).val(0);
                $("#DayLetsDiv").dialog("close");
            });

            var chkIsDaysLet = '<%=chkIsDaysLet.ClientID%>'

            $('#' + chkIsDaysLet).change(function () {
                if ($('#' + chkIsDaysLet).is(':checked')) {
                    $("#DayLetsDiv").dialog({
                        width: 700,
                        height: 300,
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
                    $("#DayLetsDiv").dialog({
                        width: 700,
                        height: 300,
                        autoOpen: true,
                        modal: true,
                        closeOnEscape: true,
                        resizable: false,
                        fluid: true,
                        title: "", ////TODO add title
                        show: 'slide'
                    });
                }
            });

            $("#<%=txtDiscountAmount.ClientID %>").blur(function () {
                CalculateDiscountAmount();
            });

            $("#<%=ddlDiscountType.ClientID %>").change(function () {
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
                $('#' + lblPaymentAccountHead).show();
                $('#ComPaymentDiv').hide();
                $('#RefundDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "bKash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
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
                $('#' + lblPaymentAccountHead).show();
                $('#ComPaymentDiv').hide();
                $('#PrintPreviewDiv').show();
                $('#RefundDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Refund") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
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

                if (txtConversionRateVal > 0) {
                    $("#BillPreviewRelatedInformation").dialog({
                        width: 335,
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
                    var SelectdRoomId = "0"; ;
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

                //$('#ReservationDetailGrid tbody > tr > td:nth-child(1)').filter(function (index) {
                //    if ($.trim($(this).text()) == ddlPayMode)
                //        duplicateCheck = true;
                //});

                if (duplicateCheck == true) {
                    toastr.warning('Duplicate Payment Mode');
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
                var isValid = true; //ValidateForm();

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
                        $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
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
                        $('#CashPaymentAccountHeadDiv').show();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#RefundDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "bKash") {
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').show();
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
                        $('#ComPaymentDiv').show();
                        $('#PrintPreviewDiv').hide();
                        $('#RefundDiv').hide();
                        $("#CompanyPaymentPopUpForm").dialog({
                            width: 600,
                            height: 532,
                            autoOpen: true,
                            modal: true,
                            closeOnEscape: true,
                            resizable: false,
                            fluid: true,
                            title: "", ////TODO add title
                            show: 'slide'
                        });
                        $("#<%=HiddenFieldCompanyPaymentButtonInfo.ClientID %>").val('1');
                    }
                    else if ($('#' + ddlPayMode).val() == "Other Room") {
                        $('#PaidByOtherRoomDiv').show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                        $('#RefundDiv').hide();

                    }
                    else if ($('#' + ddlPayMode).val() == "Refund") {
                        $('#' + lblReceiveLeadgerAmount).text("Paid Out");
                        $('#' + ddlCurrency).val(localCurrencyId);
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
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
            var formName = "<span class='divider'>/</span><li class='active'>Hold Room Check-Out</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var ddlGLProject = '<%=ddlGLProject.ClientID%>'
            var ddlGLCompany = '<%=ddlGLCompany.ClientID%>'
            var selectedIndex = parseFloat($('#' + ddlGLCompany).prop("selectedIndex"));
            if (selectedIndex > 0) {
                $("#" + ddlGLProject).attr("disabled", false);
            }
            else {
                $("#" + ddlGLProject).attr("disabled", true);
            }
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
            var amount = $('#' + txtDayLetDiscount).val();
            var floatAmount = parseFloat(amount);
            if (amount == "") {
                $('#ContentPlaceHolder1_lblDayLetsPopUp').text('Day Lets Discount Amount is not in correct Format.');
                $('#messegeDayLetsPopUp').show("slow");
                return false;
            }
            else if (floatAmount < 0) {
                $('#ContentPlaceHolder1_lblDayLetsPopUp').text('Day Lets Discount Amount is not in correct Format.');
                $('#messegeDayLetsPopUp').show("slow");
                return false;
            }
            else {
                return true;
            }
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
            var ddlChecquePaymentAccountHeadId = $("#<%=ddlChecquePaymentAccountHeadId.ClientID %>").val();
            var ddlCardPaymentAccountHeadId = $("#<%=ddlCardPaymentAccountHeadId.ClientID %>").val();

            var ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyPaymentAccountHead.ClientID %>").val();
            var ddlPaidByRegistrationId = $("#<%=ddlPaidByRegistrationId.ClientID %>").val();

            var RefundAccountHead = $("#<%=ddlRefundAccountHead.ClientID %>").val();

            var paymentDescription = "";

            if (ddlPayMode == "Cash") {
                paymentDescription = "";
            }
            else if (ddlPayMode == "bKash") {
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

                paymentDescription = "Company: " + ddlPaidByChequeCompanyText + ", Bank: " + ddlCompanyBankText + ", Cheque: " + txtChecqueNumber;
            }

            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            var conversionRate = $("#<%=txtConversionRate.ClientID %>").val();
            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            var localCurrencyId = $("#<%=hfLocalCurrencyId.ClientID %>").val();

            $('#btnAddDetailGuestPayment').val("Add");
            if (IsValidRegistrationNumber) {
                if (ddlPayMode == "Cheque") {
                    PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, paymentDescription, ddlCurrency, currencyType, localCurrencyId, conversionRate, ddlPayMode, ddlCompanyBankId, txtReceiveLeadgerAmount, ddlRegistrationId, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlChecquePaymentAccountHeadId, ddlCardPaymentAccountHeadId, ddlCompanyPaymentAccountHead, ddlPaidByRegistrationId, RefundAccountHead, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)
                }
                else {
                    PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, paymentDescription, ddlCurrency, currencyType, localCurrencyId, conversionRate, ddlPayMode, ddlBankId, txtReceiveLeadgerAmount, ddlRegistrationId, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlChecquePaymentAccountHeadId, ddlCardPaymentAccountHeadId, ddlCompanyPaymentAccountHead, ddlPaidByRegistrationId, RefundAccountHead, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)
                }
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
        }

        function IsValidRegistrationNumber() {
            var txtRegistrationId = $("#<%=txtSrcRegistrationIdList.ClientID %>").val();
            if (txtRegistrationId == "" || parseInt(txtRegistrationId) <= 0)
            { return false; }
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

        //Integrated Gl Visible True/False-------------------
        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }

        function ToggleBillSplitFieldVisible(ctrl) {
            if ($(ctrl).is(':checked')) {
                var btnLocalBillPreview = '<%=btnLocalBillPreview.ClientID%>'
                var btnLocalBillPreviewVal = $('#' + btnLocalBillPreview).val();
                var txtConversionRate = '<%=txtConversionRate.ClientID%>'
                var txtConversionRateVal = $('#' + txtConversionRate).val();
                var txtConversionRateVal = 0;
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

                $('#ContentPlaceHolder1_txtEndDate').datepicker("option", {
                    minDate: $('#' + txtStartDate).val(),
                    dateFormat: innBoarDateFormat
                });

                $("#BillSplitPopUpForm").dialog({
                    width: 600,
                    height: 530,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    title: "Guest Bill Split Information",
                    show: 'slide'
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
                $('#ComPaymentDiv').show();
                $('#PrintPreviewDiv').hide();
            }
        }
-
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
                var result = confirm("Are Your Sure To Check Out?");
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
                toastr.info(messege);
                return false;
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

        function CalculateDiscountAmount() {
            var txtDiscountAmount = '<%=txtDiscountAmount.ClientID%>'
            var txtSalesAmount = '<%=HiddenFieldSalesTotal.ClientID%>'
            var ddlDiscountType = '<%=ddlDiscountType.ClientID%>'

            var txtGrandTotal = '<%=txtGrandTotal.ClientID%>'
            var txtHFGrandTotal = '<%=HiddenFieldGrandTotal.ClientID%>'

            var txtGrandTotalUsd = '<%=txtGrandTotalUsd.ClientID%>'
            var hfGrandTotalUsd = '<%=hfGrandTotalUsd.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'

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

            if ($('#' + txtHFGrandTotal).val() < 1) {
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
            $('#BillPreviewRelatedInformation').hide("slow");
            var SelectdServiceId = "0";
            var SelectdRoomId = "0"; ;
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

            PageMethods.PerformBillSplitePrintPreview(0, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
            return false;

        }

        function PerformUSDBillPreviewAction() {
            $("#<%=hfIsEnableBillPreviewOption.ClientID %>").val("1");
            $('#BillPreviewRelatedInformation').hide("slow");
            var SelectdServiceId = "0";
            var SelectdRoomId = "0"; ;
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
    </div>
    <div class="panel-body">
        <div class="form-group" id="BillPreviewRelatedInformation" style="display: none;
            padding-top: 10px;">
            <div class="col-md-2 label-align">
                <asp:Button ID="btnLocalBillPreview" runat="server" TabIndex="4" Text="Bill Preview"
                    CssClass="btn btn-primary btn-sm" OnClientClick="javascript: return PerformLocalBillPreviewAction();" />
            </div>
            <div class="col-md-2 label-align">
                <asp:Button ID="btnUSDBillPreview" runat="server" TabIndex="4" Text="Bill Preview (USD)"
                    CssClass="btn btn-primary btn-sm" OnClientClick="javascript: return PerformUSDBillPreviewAction();" />
            </div>
        </div>
    </div>
    <div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Room Check-Out Information</div>
            <div style="height: 15px">
            </div>
            <div class="form-horizontal">
                <div class="form-group" style="display: none;">
                    <div class="col-md-2 label-align">
                        <asp:HiddenField ID="hfCurrencyType" runat="server" />
                        <asp:HiddenField ID="hfLocalCurrencyId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfGuestCompanyInformation" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenStartDate" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenEndDate" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 label-align">
                        <asp:Label ID="lblRegistrationNumber" runat="server" class="control-label" Text="Registration Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRegistrationId" runat="server" CssClass="form-control" TabIndex="3"
                            Enabled="False">
                        </asp:DropDownList>
                        <div style="display: none;">
                            <asp:HiddenField ID="txtSrcRegistrationIdList" runat="server"></asp:HiddenField>
                            <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            <asp:Button ID="btnSrcRoomNumber" runat="server" Text="Search" TabIndex="2" CssClass="btn btn-primary btn-sm"
                                OnClick="btnSrcRoomNumber_Click" />
                        </div>
                    </div>
                    <div class="col-md-2 label-align">
                    </div>
                    <div class="col-md-4">
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div id="myTabs">
                    <ul id="tabPage" class="ui-style">
                        <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-1">Check-Out Information</a></li>
                        <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-2">Room Details </a></li>
                        <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                            href="#tab-3">Service Details</a></li>
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
                            <div class="form-group" style="display:none;">
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
                                <div id="DayLetDiscountInputDiv" runat="server">
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-4">
                                        <div style="float: left">
                                            <asp:CheckBox ID="chkIsDaysLet" runat="Server" Text=" Is Late Check-Out?" Font-Bold="true"
                                                CssClass="mycheckbox" onclick="javascript: return ToggleLetCheckOutFieldVisible(this);" TextAlign="right" />
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
                            <div class="form-group">
                                <div class="aspBoxText col-md-12">
                                    <asp:CheckBox ID="chkIsBillSplit" runat="Server" Text="" Font-Bold="true" onclick="javascript: return ToggleBillSplitFieldVisible(this);"
                                        TextAlign="right" />
                                    &nbsp;&nbsp;Is Bill Split?
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <input type="button" id="btnBillPreview" value="Bill Preview" class="btn btn-primary btn-sm" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <input type="button" id="btnBillPreviewAndBillLock" value="Bill Lock & Preview" class="btn btn-primary btn-sm" />
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
                                <div id="IntegratedGeneralLedgerDiv" style="display: none;">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                        </div>
                                        <div class="col-md-2">
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
                                <div class="childDivSection" id="BillSplitPopUpForm" style="display: none;">
                                    <%--<div class="panel panel-default" style="height: 478px;">--%>
                                    <%--<div class="panel-heading">
                                        Guest Bill Split Information</div>--%>
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
                                <asp:Button ID="btnAddMoreBill" runat="server" TabIndex="12" Text="Add More Room Bill"
                                    CssClass="btn btn-primary btn-sm" OnClick="btnAddMoreBill_Click" />
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
                                                <asp:Label ID="lblIncomeNodeId" runat="server" Text='<%#Eval("IncomeNodeId") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId") %>'></asp:Label></ItemTemplate>
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
                                                <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServiceType") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ServiceId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label></ItemTemplate>
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
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIndividualRestaurantVatAmount" runat="server" class="control-label"
                                    Text="Vat Amount"></asp:Label>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:TextBox ID="txtIndividualRestaurantVatAmount" runat="server" CssClass="customLargeTextBoxSize"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIndividualRestaurantServiceCharge" runat="server" class="control-label"
                                    Text="Service Charge"></asp:Label>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:TextBox ID="txtIndividualRestaurantServiceCharge" runat="server" CssClass="customLargeTextBoxSize"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIndividualRestaurantDiscountAmount" runat="server" class="control-label"
                                    Text="Discount Amount"></asp:Label>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:TextBox ID="txtIndividualRestaurantDiscountAmount" runat="server" CssClass="customLargeTextBoxSize"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIndividualRestaurantGrandTotal" runat="server" class="control-label"
                                    Text="Total Amount"></asp:Label>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:TextBox ID="txtIndividualRestaurantGrandTotal" runat="server" CssClass="customLargeTextBoxSize"
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
                                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("CheckOutId") %>'></asp:Label></ItemTemplate>
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
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIndividualExtraRoomVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:TextBox ID="txtIndividualExtraRoomVatAmount" runat="server" CssClass="customLargeTextBoxSize"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIndividualExtraRoomServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:TextBox ID="txtIndividualExtraRoomServiceCharge" runat="server" CssClass="customLargeTextBoxSize"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIndividualExtraRoomDiscountAmount" runat="server" Text="Discount Amount"></asp:Label>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:TextBox ID="txtIndividualExtraRoomDiscountAmount" runat="server" CssClass="customLargeTextBoxSize"
                                    ReadOnly="True">0</asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIndividualExtraRoomGrandTotal" runat="server" Text="Total Amount"></asp:Label>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:TextBox ID="txtIndividualExtraRoomGrandTotal" runat="server" CssClass="customLargeTextBoxSize"
                                    ReadOnly="true">0</asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div id="Div2" class="childDivSection">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Rebate Information</div>
                        <div class="panel-body childDivSectionDivBlockBody">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblDiscountType" runat="server" class="control-label" Text="Rebate Type"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control" TabIndex="16">
                                            <asp:ListItem>Fixed</asp:ListItem>
                                            <asp:ListItem>Percentage</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblDiscountAmount" runat="server" class="control-label" Text="Rebate Amount"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtDiscountAmount" TabIndex="3" runat="server"> </asp:TextBox>
                                        <div style="display: none;">
                                            <asp:DropDownList ID="ddlCashPaymentAccountHeadForDiscount" runat="server" CssClass="form-control"
                                                TabIndex="16">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblGrandTotalLocal" runat="server" class="control-label" Text="Grand Total"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="HiddenFieldGrandTotal" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtGrandTotal" TabIndex="3" runat="server" CssClass="form-control"
                                            Enabled="false"> </asp:TextBox>
                                    </div>
                                    <div style="display: none;">
                                        <div class="col-md-2 label-align">
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
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblRebateRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtRebateRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                            TabIndex="27"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div id="PaymentDetailsInformation" class="childDivSection">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Guest Payment Information</div>
                        <div class="panel-body childDivSectionDivBlockBody">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblPayMode" runat="server" class="control-label required-field" Text="Payment Mode"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                            <asp:ListItem>Cash</asp:ListItem>
                                            <asp:ListItem>Card</asp:ListItem>
                                            <asp:ListItem>bKash</asp:ListItem>
                                            <asp:ListItem>Cheque</asp:ListItem>
                                            <asp:ListItem>Company</asp:ListItem>
                                            <asp:ListItem Value="Other Room">Guest Room</asp:ListItem>
                                            <asp:ListItem>Refund</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblCurrencyType" runat="server" class="control-label required-field"
                                            Text="Currency Type"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlCurrency" CssClass="form-control" TabIndex="6" runat="server">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblDisplayConvertionRate" runat="server" Text=""></asp:Label>
                                        <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblReceiveLeadgerAmount" runat="server" class="control-label required-field"
                                            Text="Receive Amount"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtReceiveLeadgerAmount" runat="server" CssClass="form-control"
                                            TabIndex="7"></asp:TextBox>
                                    </div>
                                    <div id="ConversionRateDivInformation" style="display: none;">
                                        <div class="col-md-2 label-align">
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
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblPaymentAccountHead" runat="server" class="control-label required-field"
                                            Text="Account Head"></asp:Label>
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
                                    </div>
                                </div>
                                <div id="PaidByOtherRoomDiv" style="display: none">
                                    <div class="form-group">
                                        <div class="col-md-2 label-align">
                                            <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Room Number"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:DropDownList ID="ddlPaidByRegistrationId" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                                    <div class="form-group">
                                        <div class="col-md-2 label-align">
                                            <asp:Label ID="lblChecquePaymentAccountHeadId" runat="server" class="control-label required-field"
                                                Text="Company Name"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control"
                                                TabIndex="6">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 label-align">
                                            <asp:Label ID="lblChecqueNumber" runat="server" class="control-label required-field"
                                                Text="Cheque Number"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 label-align">
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
                                    <div class="form-group" style="display: none;">
                                        <div class="col-md-2 label-align">
                                            <asp:Label ID="lblCardPaymentAccountHeadId" runat="server" class="control-label"
                                                Text="Accounts Posting Head"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:DropDownList ID="ddlCardPaymentAccountHeadId" runat="server" CssClass="form-control"
                                                TabIndex="6">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 label-align">
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
                                        <div class="col-md-2 label-align">
                                            <asp:Label ID="lblCardNumber" runat="server" class="control-label" Text="Card Number"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtCardNumber" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 label-align">
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
                                            <div class="col-md-2 label-align">
                                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Expiry Date"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2 label-align">
                                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtCardHolderName" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="RefundDiv">
                                    <div class="form-group" style="display: none;">
                                        <div class="col-md-2 label-align">
                                            <asp:Label ID="lblRefundAccountHead" runat="server" class="control-label" Text="Account Head"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:DropDownList ID="ddlRefundAccountHead" CssClass="form-control" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group" style="padding-left: 10px;">
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
            <div style="padding-top: 10px;">
                <asp:HiddenField ID="hfGuestPaymentDetailsInformationDiv" runat="server"></asp:HiddenField>
                <asp:Button ID="btnSave" runat="server" TabIndex="13" Text="Check Out" CssClass="btn btn-primary btn-sm"
                    OnClick="btnSave_Click" OnClientClick="javascript: return ConfirmCheckOut();" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary btn-sm"
                    TabIndex="14" OnClick="btnCancel_Click" />
            </div>
        </div>
        <div id="DayLetsDiv" style="display: none" class="block">
            <div id="messegeDayLetsPopUp" class="alert alert-info" style="display: none">
                <button type="button" class="close" data-dismiss="alert">
                    ×</button>
                <asp:Label ID='lblDayLetsPopUp' Font-Bold="True" runat="server"></asp:Label>
                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
            </div>
            <div class="panel-heading">
                Late Check-Out Information</div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2 label-align">
                            Discount Type
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDayLetDiscount" runat="server" CssClass="form-control">
                                <asp:ListItem>Percentage</asp:ListItem>
                                <asp:ListItem>Fixed</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2 label-align">
                            Discount Amount
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDayLetDiscount" runat="server" CssClass="form-control">0</asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnDayLetsOk" runat="server" TabIndex="13" Text="Ok" CssClass="btn btn-primary btn-sm"
                        OnClick="btnDayLetsOk_Click" OnClientClick="javascript: return ValidateDayLetsAmount();" />
                    <input id="btnDayLetsClear" type="button" value="Cancel" class="btn btn-primary btn-sm" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var isIntegrated = '<%=isIntegratedGeneralLedgerDiv%>';
        if (isIntegrated > -1) {
            IntegratedGeneralLedgerDivPanelShow();
        }
        else {
            IntegratedGeneralLedgerDivPanelHide();
        }

        var single = '<%=isSingle%>';
        if (single == "True") {
            $('#CompanyProjectPanel').hide();
        }
        else {
            $('#CompanyProjectPanel').show();
        }


        var paymentDetails = $("#<%=hfGuestPaymentDetailsInformationDiv.ClientID %>").val();
        if (paymentDetails > 0) {
            $('#PaymentDetailsInformation').show();
        }
        else {
            $('#PaymentDetailsInformation').hide();
        }
    </script>
</asp:Content>
