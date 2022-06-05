<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmLCSettlement.aspx.cs" Inherits="HotelManagement.Presentation.Website.LCManagement.frmLCSettlement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var minCheckInDate = "";
        var deleteObj = [];
        var editObj = [];
        var categoryWiseDiscount = new Array();
        var categoryWiseDiscountDeleted = new Array();
        var savedCategoryWiseDiscount = new Array();

        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtChequeBankId", "ContentPlaceHolder1_ddlChequeBankId", "ContentPlaceHolder1_ddlChequeBankId");
            CommonHelper.AutoSearchClientDataSource("txtMBankingBankId", "ContentPlaceHolder1_ddlMBankingBankId", "ContentPlaceHolder1_ddlMBankingBankId");

            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            //$('#' + txtConversionRate).attr('disabled', true);

            $("#<%=txtDiscountAmount.ClientID %>").blur(function () {
                CalculateDiscountAmount();
            });

            $("#<%=ddlDiscountType.ClientID %>").change(function () {
                CalculateDiscountAmount();
            });

            $('#TotalPaid').hide();

            $("#ContentPlaceHolder1_txtExpireDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            var total = $("#<%=txtSalesTotal.ClientID %>").val();
            var registrationId = $("#<%=ddlReservationId.ClientID %>").val();
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
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).show();
                $('#ComPaymentDiv').hide();
                $('#RefundDiv').hide();
                $('#PrintPreviewDiv').show();
                $('#CompanyInfoDiv').hide();
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
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "M-Banking") {
                $('#MBankingPaymentAccountHeadDiv').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#' + lblPaymentAccountHead).hide();
                $('#ComPaymentDiv').hide();
                $('#RefundDiv').hide();
                $('#PrintPreviewDiv').show();
                $('#CompanyInfoDiv').hide();
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
                $('#CompanyInfoDiv').hide();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').show();
                $('#MBankingPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#lblPaymentAccountHead').hide();
                $('#' + lblPaymentAccountHead).show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#ComPaymentDiv').show();
                $('#PrintPreviewDiv').hide();
                $('#RefundDiv').hide();
                $('#CompanyInfoDiv').show();
                //popup(1, 'BillSplitPopUpForm', '', 600, 532);
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
                $('#CompanyInfoDiv').hide();
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

            $('#ComPaymentDiv').hide();
            $('#PrintPreviewDiv').show();

            $("#btnCancelAddMoreItemInfo").click(function () {
                $("#AddMoreItemInformationDiv").dialog("close");
            });

            $("#btnBillPrintPreview").click(function () {
                var iframeid = 'printDoc';
                var url = '/Banquet/Reports/frmReportReservationBillInfo.aspx?ReservationId='
                           + $("#<%=ddlReservationId.ClientID %>").val();
                parent.document.getElementById(iframeid).src = url;

                $("#displayBill").dialog({
                    autoOpen: true,
                    modal: true,
                    minWidth: 800,
                    minHeight: 555,
                    width: 'auto',
                    closeOnEscape: false,
                    resizable: false,
                    height: 'auto',
                    fluid: true,
                    title: "Invoice Preview",
                    show: 'slide'
                });
            });

            $("#btnBillPreview").click(function () {

                var txtConversionRate = '<%=txtConversionRate.ClientID%>'
                var txtConversionRateVal = $('#' + txtConversionRate).val();

                if (txtConversionRateVal > 0) {
                    //$('#BillPreviewRelatedInformation').show("slow");
                    popup(1, 'BillPreviewRelatedInformation', '', 335, 100);
                }
                else {
                    $('#BillPreviewRelatedInformation').hide("slow");
                    var SelectdServiceId = "0";
                    var SelectdRoomId = "0"; ;
                    var HiddenStartDate = '<%=HiddenStartDate.ClientID%>'
                    var HiddenEndDate = '<%=HiddenEndDate.ClientID%>'
                    var ddlReservationId = '<%=ddlReservationId.ClientID%>'
                    var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

                    var SelectdServiceApprovedId = "0";
                    var SelectdRoomApprovedId = "0";
                    var SelectdPaymentId = "0";
                    var SelectdIndividualPaymentId = "0";
                    var SelectdIndividualTransferedPaymentId = "0";


                    var StartDate = $('#' + HiddenStartDate).val();
                    var EndDate = $('#' + HiddenEndDate).val();
                    var RegistrationId = $('#' + ddlReservationId).val();
                    var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();

                    var isIsplite = "0";
                    popup(-1);
                    PageMethods.PerformBillSplitePrintPreview(txtConversionRateVal, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
                    return false;
                }

            });

            $("#btnAddDetailGuestPayment").click(function () {
                SaveGuestPaymentDetailsInformationByWebMethod();
                /*
                var enteredAmount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
                if (isNaN(enteredAmount)) {
                toastr.info('Entered Amount is not in correct format.');
                return;
                }

                var ddlCurrencyType = '<%=ddlCurrency.ClientID%>'

                if ($('#' + ddlCurrencyType).val() != "45") {
                var txtConversionRate = $("#<%=txtConversionRateHiddenField.ClientID %>").val();
                if (isNaN(txtConversionRate)) {
                toastr.info('Entered Conversion Rate is not in correct format.');
                return;
                }

                if (txtConversionRate == 0) {
                toastr.info('Entered Conversion Rate is not in correct format.');
                return;
                }
                }

                var ddlPayMode = $("#<%=ddlPayMode.ClientID %>").val();
                if (ddlPayMode == 0) {
                toastr.info('Please Select Valid Payment Mode.');
                return;
                }

                var txtReceiveLeadgerAmount = '<%=txtReceiveLeadgerAmount.ClientID%>'
                var txtCardNumber = '<%=txtCardNumber.ClientID%>'
                var amount = $('#' + txtReceiveLeadgerAmount).val();
                var number = $('#' + txtCardNumber).val()
                var ddlReservationId = '<%=ddlReservationId.ClientID%>'
                var ddlPayMode = '<%=ddlPayMode.ClientID%>'
                var ddlBankId = '<%=ddlBankId.ClientID%>'
                var amount = $('#' + txtReceiveLeadgerAmount).val();
                var regId = $('#' + ddlReservationId).val();
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
                else {
                SaveGuestPaymentDetailsInformationByWebMethod();
                }
                }

                else {
                toastr.info('Please provide a valid Room Number.');
                return;
                }
                */

            });



            $(document).ready(function () {

                var v = $("#<%=ddlCurrency.ClientID %>").val();
                PageMethods.LoadCurrencyType(v, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);

                var ddlCurrencyType = '<%=ddlCurrency.ClientID%>'
                var txtConversionRate = '<%=txtConversionRate.ClientID%>'
                $('#' + ddlCurrencyType).change(function () {
                    var v = $("#<%=ddlCurrency.ClientID %>").val();
                    PageMethods.LoadCurrencyType(v, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);
                    //                    if ($('#' + ddlCurrencyType).val() == "45") {
                    //                        //$('#' + txtConversionRate).attr('disabled', true);
                    //                        $('#ConversionRateDivInformation').hide();
                    //                    }
                    //                    else {
                    //                        //$('#' + txtConversionRate).attr('disabled', false);
                    //                        $('#ConversionRateDivInformation').show();
                    //                    }                  
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
                        //$('#<%=txtConversionRate.ClientID%>').attr("disabled", true);
                        $('#ConversionRateDivInformation').hide();
                    }
                    else {
                        $('#ConversionRateDivInformation').show();
                        //$('#<%=txtConversionRate.ClientID%>').attr("disabled", false);
                        $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                    }

                    var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
                    if (ddlCurrency == 0) {
                        $('#ConversionRateDivInformation').hide();
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
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').show();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#RefundDiv').hide();
                        $('#PrintPreviewDiv').show();
                        $('#CompanyInfoDiv').hide();
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
                        $('#CompanyInfoDiv').hide();
                    }
                    else if ($('#' + ddlPayMode).val() == "M-Banking") {
                        $('#MBankingPaymentAccountHeadDiv').show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#RefundDiv').hide();
                        $('#PrintPreviewDiv').show();
                        $('#CompanyInfoDiv').hide();
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
                        $('#CompanyInfoDiv').hide();
                    }
                    else if ($('#' + ddlPayMode).val() == "Company") {
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').show();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#lblPaymentAccountHead').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#ComPaymentDiv').show();
                        $('#PrintPreviewDiv').hide();
                        $('#RefundDiv').hide();
                        $('#CompanyInfoDiv').show();
                        //popup(1, 'BillSplitPopUpForm', '', 600, 532);
                    }
                    else if ($('#' + ddlPayMode).val() == "Other Room") {
                        $('#PaidByOtherRoomDiv').show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#MBankingPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                        $('#RefundDiv').hide();
                        $('#CompanyInfoDiv').hide();
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
                        $('#CompanyInfoDiv').hide();
                    }

                    //var ddlCurrencyType = '<%=ddlCurrency.ClientID%>'
                    var ddlCurrencyType = $("#<%=hfCurrencyType.ClientID %>").val();
                    if (ddlCurrencyType == "Local") {
                        $('#ConversionRateDivInformation').hide();
                        //$('#' + txtConversionRate).attr('disabled', true);

                    }
                    else {
                        //$('#' + txtConversionRate).attr('disabled', false);
                        $('#ConversionRateDivInformation').show();
                    }
                });
            });


            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Banquet Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Bill Settlement</li>";
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

            var txtUnitPrice = '<%=txtUnitPrice.ClientID%>'
            var chkIscomplementary = '<%=chkIscomplementary.ClientID%>'
            $('#' + chkIscomplementary).change(function () {
                if ($('#' + chkIscomplementary).is(':checked')) {
                    $('#' + txtUnitPrice).val("0")
                }
            });

            $("#btnAddMoreItem").click(function () {
                //$("#ItemDetails").show();
                var costCenterId = $("#<%=hfCostcenterId.ClientID %>").val();
                PageMethods.LoadCategory(costCenterId, OnLoadCategorySucceeded, OnLoadCategoryFailed);
                return false;
            });

            var ddlItemType = '<%=ddlItemType.ClientID%>'
            var ddlItemId = '<%=ddlItemId.ClientID%>'

            $('#' + ddlItemType).change(function () {
                var itemType = $('#' + ddlItemType).val();
                LoadProductItem(itemType);
            });
            $('#' + ddlItemId).change(function () {
                var ddlItemId = '<%=ddlItemId.ClientID%>'
                var ddlItemId = $('#' + ddlItemId).val();
                //alert(ddlItemId);
                LoadProductData(ddlItemId);
            });
            $("#btnAddDetailItem").click(function () {
                var add = false;
                var reservationId = $("#ContentPlaceHolder1_hfReservationId").val();
                if (reservationId == "") {
                    reservationId = 0;
                }
                var x = document.getElementById("ContentPlaceHolder1_chkIscomplementary").checked;
                //alert(x);                

                if ($("#<%=ddlItemType.ClientID %>").val() == "0") {
                    toastr.warning("Please Select Category Name.");
                    return;
                }
                else if ($("#<%=ddlItemId.ClientID %>").val() == "0") {
                    toastr.warning("Please Select Item Name.");
                    return;
                }
                if ($("#<%=txtUnitPrice.ClientID %>").val() == "0") {
                    if (x == true) {
                        add = true;
                    }
                    else {
                        toastr.warning("Please Provide Unit Price.");
                        return;
                    }
                }
                if ($("#<%=txtUnitPrice.ClientID %>").val() != "0") {
                    var decimalOnly = /^\s*-?[1-9]\d*(\.\d{1,2})?\s*$/;
                    if (decimalOnly.test($("#<%=txtUnitPrice.ClientID %>").val())) {
                        add = true;
                    }
                    else {
                        toastr.warning("Wrong input for Unit Price.");
                        return;
                    }
                }
                if ($("#<%=txtItemUnit.ClientID %>").val() == "") {
                    toastr.warning("Please Provide Item Unit.");
                    return;
                }
                if ($("#<%=txtItemUnit.ClientID %>").val() != "") {
                    var numbersOnly = /^\d+$/;
                    if (numbersOnly.test($("#<%=txtItemUnit.ClientID %>").val())) {
                        add = true;
                    }
                    else {
                        toastr.warning("Wrong input for Item Unit.");
                        return;
                    }
                }
                if (add == true) {
                    AddItemDetailsInfo(0, reservationId);
                }
            });
        });

        function AddItemDetailsInfo(id, reservationId) {

            var itemTypeId = $("#<%=ddlItemType.ClientID %>").val();
            var itemId = $("#<%=ddlItemId.ClientID %>").val();
            var unitPrice = $("#<%=txtUnitPrice.ClientID %>").val();
            var itemUnit = $("#<%=txtItemUnit.ClientID %>").val();

            if ($('#' + unitPrice).val() != 0) {

                //var calculatedAmount = parseFloat($('#' + itemUnit).val()) * parseFloat($('#' + unitPrice).val());
                var calculatedAmount = itemUnit * unitPrice;

                if ($("#ltlTableWiseItemInformation > table").length > 0 && reservationId == 0) {
                    AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount);
                    return false;
                }
                else if ($("#ltlTableWiseItemInformation > table").length > 0 && reservationId > 0) {
                    AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount);
                    return false;
                }

                var table = "", deleteLink = "", editLink = "";

                deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                //editLink = "<a href=\"#\" onclick= 'EditReservationDetail(this)' ><img alt=\"Edit\" src=\"../Images/edit.png\" /></a>";
                table += "<table id='RecipeItemInformation' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                table += "<th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th align='left' scope='col' style='width: 45%;'>Item Name</th><th align='left' scope='col' style='width: 15%;'>Unit Price</th><th align='left' scope='col' style='width: 15%;'>Unit</th><th align='left' scope='col' style='width: 15%;'>Amount</th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";

                table += "<tbody>";
                table += "<tr style=\"background-color:#E3EAEB;\">";

                table += "<td align='left' style=\"display:none;\">" + id + "</td>";
                table += "<td align='left' style=\"display:none;\">" + reservationId + "</td>";
                table += "<td align='left' style=\"display:none;\">" + itemTypeId + "</td>";
                table += "<td align='left' style=\"display:none;\">" + $("#<%=ddlItemType.ClientID %> option:selected").text() + "</td>";
                table += "<td align='left' style=\"display:none;\">" + itemId + "</td>";
                table += "<td align='left' style=\"width:45%; text-align:Left;\">" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "</td>";
                table += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitPrice + "</td>";
                table += "<td align='left' style=\"width:15%; text-align:Left;\">" + itemUnit + "</td>";
                table += "<td align='left' style=\"width:15%; text-align:Left;\">" + calculatedAmount + "</td>";
                table += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

                table += "</tr>";
                table += "</tbody>";
                table += "</table>";

                $("#ltlTableWiseItemInformation").html(table);

                CalculateTotalPaiedNDueAmount();

                $("#<%=ddlItemType.ClientID %>").val('');
                $("#<%=ddlItemId.ClientID %>").val('');
                $("#<%=txtUnitPrice.ClientID %>").val('');
                $("#<%=txtItemUnit.ClientID %>").val('');
                $("#ContentPlaceHolder1_chkIscomplementary").attr('checked', false);
            }
        }
        function AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount) {
            var tr = "", totalRow = 0, deleteLink = "", editLink = "";
            totalRow = $("#RecipeItemInformation tbody tr").length;

            deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            //editLink = "<a href=\"#\" onclick= 'EditReservationDetail(this)' ><img alt=\"Edit\" src=\"../Images/edit.png\" /></a>";

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }
            tr += "<td align='left' style=\"display:none;\">" + id + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + reservationId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + itemTypeId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + $("#<%=ddlItemType.ClientID %> option:selected").text() + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + itemId + "</td>";
            tr += "<td align='left' style=\"width:45%; text-align:Left;\">" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitPrice + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itemUnit + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + calculatedAmount + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#RecipeItemInformation tbody").append(tr);
            CalculateTotalPaiedNDueAmount();
            $("#<%=ddlItemType.ClientID %>").val('');
            $("#<%=ddlItemId.ClientID %>").val('');
            $("#<%=txtUnitPrice.ClientID %>").val('');
            $("#<%=txtItemUnit.ClientID %>").val('');
            $("#ContentPlaceHolder1_chkIscomplementary").attr('checked', false);
        }
        function CalculateTotalPaiedNDueAmount() {
            var calculateAmount = 0;

            calculateAmount = CalculateTotalAddedPayment();

            $("#TotalPaid").text("Total Amount: " + calculateAmount);

            //TotalRoomRateVatServiceChargeCalculation();
        }
        function CalculateTotalAddedPayment() {
            var calculateAmount = 0;
            $("#RecipeItemInformation tbody tr").each(function () {
                calculateAmount += parseFloat($.trim($(this).find("td:eq(8)").text(), 10));
            });
            return calculateAmount;
        }

        function OnLoadCategorySucceeded(result) {
            var list = result;
            var ddlCategoryId = '<%=ddlItemType.ClientID%>';
            var control = $('#' + ddlCategoryId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].MatrixInfo + '" value="' + list[i].CategoryId + '">' + list[i].MatrixInfo + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                }
            }

            $("#AddMoreItemInformationDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Add More Item Information",
                show: 'slide'
            });

            return false;
        }

        function OnLoadCategoryFailed() {
            toastr.error(error.get_message());
        }

        function LoadProductItem(itemType) {
            var costCenterId = $("#<%=hfCostcenterId.ClientID %>").val();
            PageMethods.GetInvItemByCategoryNCostCenter(costCenterId, itemType, OnFillServiceSucceeded, OnFillServiceFailed);
            return false;
        }
        function OnFillServiceSucceeded(result) {
            var list = result;
            var ddlItemId = '<%=ddlItemId.ClientID%>';
            var control = $('#' + ddlItemId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].ItemId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                }
            }
            return false;
        }

        function OnFillServiceFailed(error) {
            toastr.error(error.get_message());
        }
        function LoadProductData(ddlItemId) {
            var costCenterId = $("#<%=hfCostcenterId.ClientID %>").val();
            var categoryId = $("#<%=ddlItemType.ClientID %>").val();
            PageMethods.GetProductDataByCriteria(categoryId, costCenterId, ddlItemId, LoadProductDataSucceeded, LoadProductDataFailed);
            return false;
        }
        function LoadProductDataSucceeded(result) {
            var txtUnitPrice = '<%=txtUnitPrice.ClientID%>'
            var txtHiddenProductId = '<%=txtHiddenProductId.ClientID%>'
            $('#' + txtHiddenProductId).val(result.ItemId);
            $('#' + txtUnitPrice).val(result.UnitPriceLocal);
            return false;
        }
        function LoadProductDataFailed(error) {
            toastr.error(error.get_message());
        }

        function DeleteDetailInfo(anchor) {
            ff = anchor;
            var tr = $(anchor).parent().parent();

            var detailId = $.trim($(tr).find("td:eq(0)").text());
            var reservationId = $.trim($(tr).find("td:eq(1)").text());

            if (parseInt(detailId, 10) != 0) {
                deleteObj.push({
                    DetailId: detailId,
                    ReservationId: reservationId
                });
            }

            $(tr).remove();
            CalculateTotalPaiedNDueAmount();
            return false;
        }

        function ValidateForm()
        { }

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
            var ddlReservationId = $("#<%=ddlReservationId.ClientID %>").val();
            var ddlPayMode = $("#<%=ddlPayMode.ClientID %>").val();
            var txtReceiveLeadgerAmount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
            var ddlCashReceiveAccountsInfo = $("#<%=ddlCashReceiveAccountsInfo.ClientID %>").val();

            var txtCardNumber = $("#<%=txtCardNumber.ClientID %>").val();
            var ddlCardType = $("#<%=ddlCardType.ClientID %>").val();
            var txtExpireDate = $("#<%=txtExpireDate.ClientID %>").val();
            var txtCardHolderName = $("#<%=txtCardHolderName.ClientID %>").val();

            var txtChecqueNumber = $("#<%=txtChecqueNumber.ClientID %>").val();
            var ddlBankId = $("#<%=ddlBankId.ClientID %>").val();
            var ddlMBankingBankId = $("#<%=ddlMBankingBankId.ClientID %>").val();
            var ddlMBankingReceiveAccountsInfo = $("#<%=ddlMBankingReceiveAccountsInfo.ClientID %>").val();
            var ddlChecquePaymentAccountHeadId = $("#<%=ddlChequeReceiveAccountsInfo.ClientID %>").val();
            var ddlCardPaymentAccountHeadId = $("#<%=ddlCardPaymentAccountHeadId.ClientID %>").val();

            var ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyName.ClientID %>").val();
            var ddlPaidByRegistrationId = $("#<%=ddlPaidByRegistrationId.ClientID %>").val();

            var RefundAccountHead = $("#<%=ddlRefundAccountHead.ClientID %>").val();
            var ddlChequeBankId = $("#<%=ddlChequeBankId.ClientID %>").val();

            var paymentDescription = "";

            //            if (ddlPayMode == "Cash") {
            //                paymentDescription = "";
            //            }
            //            else if (ddlPayMode == "Card") {
            //                var ddlCardTypeText = $("#<%=ddlCardType.ClientID %> option:selected").text();
            //                paymentDescription = ddlCardTypeText;
            //            }
            //            else if (ddlPayMode == "Other Room") {
            //                var ddlPaidByRegistrationIdText = $("#<%=ddlPaidByRegistrationId.ClientID %> option:selected").text();
            //                paymentDescription = "Room# " + ddlPaidByRegistrationIdText;
            //            }
            //            else if (ddlPayMode == "Refund") {
            //                paymentDescription = "";
            //            }
            //            else if (ddlPayMode == "Company") {
            //                paymentDescription = hfGuestCompanyInformation;
            //            }

            if ($("#<%=txtReceiveLeadgerAmount.ClientID %>").val() == "") {
                toastr.info("Please provide Receive Amount.");
                return false;
            }

            if (ddlPayMode == "Cash") {
                paymentDescription = "";
            }
            else if (ddlPayMode == "M-Banking") {
                paymentDescription = $("#<%=ddlMBankingBankId.ClientID %> option:selected").text(); ;
            }
            else if (ddlPayMode == "Card") {
                var ddlCardTypeText = $("#<%=ddlCardType.ClientID %> option:selected").text();
                paymentDescription = ddlCardTypeText;
            }
            else if (ddlPayMode == "Other Room") {
                var ddlPaidByRegistrationIdText = $("#<%=ddlPaidByRegistrationId.ClientID %> option:selected").text();
                paymentDescription = "Room# " + ddlPaidByRegistrationIdText;
            }
            else if (ddlPayMode == "Refund") {
                paymentDescription = "";
            }
            else if (ddlPayMode == "Cheque") {
                /*var ddlPaidByRegistrationId = $("#<%=ddlCompanyName.ClientID %>").val();
                var ddlPaidByRegistrationIdText = $("#<%=ddlCompanyName.ClientID %> option:selected").text();
                ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyName.ClientID %>").val();
                paymentDescription = "Cheque Payment, Company: " + ddlPaidByRegistrationIdText;
                ddlChecquePaymentAccountHeadId = ddlCompanyPaymentAccountHead;*/

                var ddlPaidByRegistrationId = ddlChecquePaymentAccountHeadId
                var ddlPaidByRegistrationIdText = $("#<%=txtChecqueNumber.ClientID %>").val();
                ddlCompanyPaymentAccountHead = ddlChecquePaymentAccountHeadId;
                paymentDescription = $("#<%=txtChecqueNumber.ClientID %>").val();
                ddlChecquePaymentAccountHeadId = ddlChecquePaymentAccountHeadId;
                ddlBankId = $("#<%=ddlChequeBankId.ClientID %>").val();
            }
            else if (ddlPayMode == "Company") {
                var ddlPaidByRegistrationId = $("#<%=ddlCompanyName.ClientID %>").val();
                var ddlPaidByRegistrationIdText = $("#<%=ddlCompanyName.ClientID %> option:selected").text();
                ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyName.ClientID %>").val();
                paymentDescription = ddlPaidByRegistrationIdText;
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

            if (ddlPayMode == "Cheque") {
                var ddlCompanyName = $('#<%=ddlCompanyName.ClientID%>').val();
                var bankId = $("#<%=ddlChequeBankId.ClientID %>").val();
                var txtChecqueNumber = $.trim($("#<%=txtChecqueNumber.ClientID %>").val());

                /*if (ddlCompanyName == "0") {
                toastr.warning("Please select Company Name.");
                return false;
                }
                else 
                */
                if (bankId == "") {
                    toastr.warning("Please provide Bank Name.");
                    return false;
                }
                else if (txtChecqueNumber == "") {
                    toastr.warning("Please provide Checque Number.");
                    return false;
                }
                /*else if (ddlPaidByRegistrationId == "0") {
                toastr.warning('Please Select Company Name.');
                return;
                }*/
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
            if (ddlPayMode == "Company") {
                if (ddlPaidByRegistrationId == "0") {
                    toastr.warning('Please Select Company Name.');
                    return;
                }
            }

            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            var conversionRate = $("#<%=txtConversionRate.ClientID %>").val();
            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            var localCurrencyId = $("#<%=hfLocalCurrencyId.ClientID %>").val();

            if (ddlCurrency == 0) {
                toastr.warning('Please Select Currency Type.');
                return;
            }

            $('#btnAddDetailGuestPayment').val("Add");
            if (IsValidRegistrationNumber) {
                PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, paymentDescription, ddlCurrency, currencyType, localCurrencyId, conversionRate, ddlPayMode, ddlBankId, txtReceiveLeadgerAmount, ddlReservationId, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlChecquePaymentAccountHeadId, ddlCardPaymentAccountHeadId, ddlCompanyPaymentAccountHead, ddlMBankingBankId, ddlMBankingReceiveAccountsInfo, ddlPaidByRegistrationId, RefundAccountHead, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)
            }
            return false;
        }
        function OnPerformSaveGuestPaymentDetailsInformationFailed(error) {
            toastr.error(error.get_message());
        }
        function OnPerformSaveGuestPaymentDetailsInformationSucceeded(result) {
            $("#GuestPaymentDetailGrid").html(result);
            ClearDetailsPart();
            GetTotalPaidAmount()
        }

        function GetTotalPaidAmount() {
            PageMethods.PerformGetTotalPaidAmountByWebMethod(OnPerformGetTotalPaidAmountSucceeded, PerformGetTotalPaidAmountFailed)
            return false;
        }

        function PerformGetTotalPaidAmountFailed(error) {
            alert(error.get_message());
        }
        function OnPerformGetTotalPaidAmountSucceeded(result) {
            var txtGrandTotal = $("#<%=txtGrandTotal.ClientID %>").val();
            var _registrationId = $("#<%=ddlReservationId.ClientID %>").val();
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

            var dueFormatedText = "Due Amount   :  " + dueAmountTotal;
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
            $('#txtChequeBankId').val('');
            $('#txtMBankingBankId').val('');
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

            var ddlReservationId = '<%=txtSrcRegistrationIdList.ClientID%>'
            var RegistrationId = $('#' + ddlReservationId).val();

            //PageMethods.GetTotalBillAmountByWebMethod(RegistrationId, SelectdRoomId, SelectdServiceId, StartDate, EndDate, OnGetTotalBillAmountByWebMethodSucceeded, OnGetTotalBillAmountByWebMethodFailed);
            return false;
        }

        function OnGetTotalBillAmountByWebMethodSucceeded(result) {
        }
        function OnGetTotalBillAmountByWebMethodFailed(error) {
        }

        function OnPerformCompanyPayBillSucceeded(result) {
        }
        function OnPerformCompanyPayBillFailed(error) {

        }

        function OnPerformBillSplitePrintPreviewSucceeded(result) {
            $('#ContentPlaceHolder1_chkIsBillSplit').attr('checked', false)
            var url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx";
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=770,height=680,left=300,top=50,resizable=yes");
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


        //MessageDiv Visible True/False-------------------
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
            var ddlReservationId = '<%=ddlReservationId.ClientID%>'
            var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

            var SelectdServiceApprovedId = "0";
            var SelectdRoomApprovedId = "0";
            var SelectdPaymentId = "0";
            var SelectdIndividualPaymentId = "0";
            var SelectdIndividualTransferedPaymentId = "0";

            var StartDate = $('#' + HiddenStartDate).val();
            var EndDate = $('#' + HiddenEndDate).val();
            var RegistrationId = $('#' + ddlReservationId).val();
            var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();

            var isIsplite = "0";
            popup(-1);



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
            var ddlReservationId = '<%=ddlReservationId.ClientID%>'
            var txtSrcRegistrationIdList = '<%=txtSrcRegistrationIdList.ClientID%>'

            var SelectdServiceApprovedId = "0";
            var SelectdRoomApprovedId = "0";
            var SelectdPaymentId = "0";
            var SelectdIndividualPaymentId = "0";
            var SelectdIndividualTransferedPaymentId = "0";

            var StartDate = $('#' + HiddenStartDate).val();
            var EndDate = $('#' + HiddenEndDate).val();
            var RegistrationId = $('#' + ddlReservationId).val();
            var SrcRegistrationIdList = $('#' + txtSrcRegistrationIdList).val();

            var isIsplite = "0";
            popup(-1);

            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            var txtConversionRateVal = $('#' + txtConversionRate).val();

            PageMethods.PerformBillSplitePrintPreview(txtConversionRateVal, isIsplite, ddlServiceTypeVal, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdServiceApprovedId, SelectdRoomApprovedId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, RegistrationId, SrcRegistrationIdList, OnPerformBillSplitePrintPreviewSucceeded, OnPerformBillSplitePrintPreviewFailed);
            return false;
        }

        function ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(ctrl) {
            TotalRoomRateVatServiceChargeCalculation();
        }

        function ToggleTotalRoomRateVatServiceChargeCalculationForVat(ctrl) {
            TotalRoomRateVatServiceChargeCalculation();
        }
        function ValidationNPreprocess() {
            var noOfAdult = $.trim($("#<%=txtNumberOfPersonAdult.ClientID %>").val());
            var noOfChild = $.trim($("#<%=txtNumberOfPersonChild.ClientID %>").val());

            if (noOfAdult != "") {
                var numbersOnly = /^\d+$/;
                if (numbersOnly.test(noOfAdult)) {
                }
                else {
                    toastr.warning("Provide correct number of Adult.");
                    return false;
                }
            }

            var saveObj = [];
            var detailId = 0, reservationId = 0, itemTypeId = 0, itemType = "", itemId = 0, itemName = "", itemUnit = 0, unitPrice = 0, totalPrice = 0;

            var rowLength = $("#ltlTableWiseItemInformation > table tbody tr").length;

            $("#ltlTableWiseItemInformation > table tbody tr").each(function () {
                detailId = parseInt($.trim($(this).find("td:eq(0)").text(), 10));
                reservationId = parseInt($.trim($(this).find("td:eq(1)").text(), 10));
                itemTypeId = parseInt($.trim($(this).find("td:eq(2)").text(), 10));
                itemType = $(this).find("td:eq(3)").text();
                itemId = parseInt($.trim($(this).find("td:eq(4)").text(), 10));
                itemName = $(this).find("td:eq(5)").text();
                unitPrice = parseFloat($.trim($(this).find("td:eq(6)").text(), 10));
                itemUnit = parseFloat($.trim($(this).find("td:eq(7)").text(), 10));
                totalPrice = parseFloat($.trim($(this).find("td:eq(8)").text(), 10));

                if (detailId == 0) {

                    saveObj.push({
                        DetailId: detailId,
                        ReservationId: reservationId,
                        ItemTypeId: itemTypeId,
                        ItemType: itemType,
                        ItemId: itemId,
                        ItemName: itemName,
                        ItemUnit: itemUnit,
                        UnitPrice: unitPrice,
                        TotalPrice: totalPrice
                    });
                }
            });

            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));
            $("#<%=hfDeleteObj.ClientID %>").val(JSON.stringify(deleteObj));
            $("#ContentPlaceHolder1_hfClassificationDiscountSave").val(JSON.stringify(categoryWiseDiscount));
            $("#ContentPlaceHolder1_hfClassificationDiscountDelete").val(JSON.stringify(categoryWiseDiscountDeleted));

            $("#<%=hfNumberOfPersonAdult.ClientID %>").val(noOfAdult);
            $("#<%=hfNumberOfPersonChild.ClientID %>").val(noOfChild);

            $("#AddMoreItemInformationDiv").dialog("close");

            $("#ContentPlaceHolder1_btnUpdateReservation").trigger("click");

            return false;
        }    
    </script>
    <asp:HiddenField ID="hfSaveObj" runat="server" />
    <asp:HiddenField ID="hfEditObj" runat="server" />
    <asp:HiddenField ID="hfDeleteObj" runat="server" />
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfCostcenterId" runat="server" />
    <asp:HiddenField ID="hfReservationId" runat="server" />
    <asp:HiddenField ID="hfLocalCurrencyId" runat="server" />
    <asp:HiddenField ID="hfClassificationDiscountSave" runat="server" />
    <asp:HiddenField ID="hfClassificationDiscountDelete" runat="server" />
    <asp:HiddenField ID="hfNumberOfPersonAdult" runat="server" />
    <asp:HiddenField ID="hfNumberOfPersonChild" runat="server" />
    <asp:HiddenField ID="hfDiscountType" runat="server" />
    <asp:HiddenField ID="hfDiscountAmount" runat="server" />
    <div style="display: none;">
        <asp:Button ID="btnUpdateReservation" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
            TabIndex="31" OnClick="btnUpdateReservation_Click" /></div>
    <div id="MessageBox" class="alert alert-info" style="display: none">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
        <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfIsEnableBillPreviewOption" runat="server"></asp:HiddenField>
    </div>
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="800" height="650" frameborder="0" style="overflow: hidden;">
        </iframe>
        <div id="bottomPrint">
        </div>
    </div>
    <div class="form-group" id="BillPreviewRelatedInformation" style="display: none;
        padding-top: 10px;">
        <div class="col-md-2">
            <asp:Button ID="btnLocalBillPreview" runat="server" TabIndex="4" Text="Bill Preview"
                CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript: return PerformLocalBillPreviewAction();" />
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnUSDBillPreview" runat="server" TabIndex="4" Text="Bill Preview (USD)"
                CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript: return PerformUSDBillPreviewAction();" />
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            LC Settlement Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div style="height: 15px">
                </div>
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfGuestCompanyInformation" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenStartDate" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenEndDate" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblRoomNumber" runat="server" class="control-label" Text="Bill Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblSrcRoomNumber" runat="server" class="control-label"
                            Text="LC Number"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:HiddenField ID="txtSrcRegistrationIdList" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtSrcBillNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-1" style="padding-left: 0;">
                        <asp:Button ID="btnSrcBillNumber" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSrcBillNumber_Click" />
                    </div>
                    <div class="col-md-2" style="display: none;">
                        <asp:Label ID="lblRegistrationNumber" runat="server" class="control-label" Text="Reservation Number"></asp:Label>
                    </div>
                    <div class="col-md-4" style="display: none;">
                        <asp:DropDownList ID="ddlReservationId" runat="server" CssClass="form-control" TabIndex="3"
                            Enabled="False">
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="padding: 3px;">
                    <div id="myTabs">
                        <ul id="tabPage" class="ui-style">
                            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                                href="#tab-1">Settlement Information</a></li>
                            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                                href="#tab-2">Item Details </a></li>
                            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                                href="#tab-3">Overhead Details</a></li>
                            <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                                href="#tab-4">Payment Details</a></li>
                        </ul>
                        <div id="tab-1">
                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblddlBanquetId" runat="server" class="control-label" Text="LC Number"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtBanquetId" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblBanquetRate" runat="server" class="control-label" Text="LC Open Date"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtLCOpenDate" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblddlSeatingId" runat="server" class="control-label" Text="LC Mature Date"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtLCMatureDate" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label10" runat="server" class="control-label" Text="Supplier Name"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label11" runat="server" class="control-label" Text="Total LC Cost"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtTotalLCCost" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group" style="display: none;">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblServiceChargeTotal" runat="server" class="control-label" Text="Service Charge"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtServiceChargeTotal" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                                <asp:CheckBox ID="cbServiceCharge" Visible="false" runat="server" Text="" CssClass="customChkBox"
                                                    onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                    TabIndex="8" Checked="True" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblVatTotal" runat="server" class="control-label" Text="Vat Amount"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtVatTotal" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                                <asp:CheckBox ID="cbVatAmount" runat="server" Visible="false" Text="" CssClass="customChkBox"
                                                    onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForVat(this);"
                                                    TabIndex="8" Checked="True" />
                                            </div>
                                        </div>
                                        <div class="form-group" style="display: none;">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblDiscountAmountTotal" runat="server" class="control-label" Text="Discount Amount"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtDiscountAmountTotal" runat="server" CssClass="form-control" ReadOnly="true">0</asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblAdvancePaymentAmount" runat="server" class="control-label" Text="Advance Amount"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:HiddenField ID="hfAdvancePaymentAmount" runat="server"></asp:HiddenField>
                                                <asp:TextBox ID="txtAdvancePaymentAmount" runat="server" CssClass="form-control"
                                                    ReadOnly="true">0</asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group" style="display: none;">
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
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Panel ID="pnlBillPrintPreview" runat="server">
                                                    <input type="button" id="btnAddMoreItem" value="Add More Item" class="TransactionalButton btn btn-primary btn-sm" />
                                                    <input type="button" id="btnBillPrintPreview" value="Bill Preview" class="btn btn-primary btn-sm" />
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div class="form-group" style="display: none;">
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
                                                        <asp:Label ID="lblGLCompany" runat="server" Text="Company"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control" onchange="PopulateProjects();">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:Label ID="lblGLProject" runat="server" Text="Project"></asp:Label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ddlGLProject" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="display: none;">
                                            <div id="Div2" class="childDivSection">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        Rebate Information</div>
                                                    <div class="panel-body">
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
                                                                    <asp:TextBox ID="txtDiscountAmount" TabIndex="3" CssClass="form-control" runat="server"> </asp:TextBox>
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
                                                                <div class="col-md-2" style="display: none;">
                                                                    <asp:Label ID="lblGrandTotalUsd" runat="server" class="control-label" Text="Grand Total"></asp:Label>
                                                                </div>
                                                                <div class="col-md-4" style="display: none;">
                                                                    <asp:HiddenField ID="hfGrandTotalUsd" runat="server"></asp:HiddenField>
                                                                    <asp:TextBox ID="txtGrandTotalUsd" TabIndex="3" runat="server" CssClass="form-control"
                                                                        Enabled="false"> </asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="col-md-2">
                                                                    <asp:Label ID="lblRebateRemarks" runat="server" class="control-label" Text="Rebate Details"></asp:Label>
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
                                        <div id="GuestPaymentDetailsInformationDiv" runat="server" class="panel panel-default">
                                            <div class="panel-body">
                                                <div class="form-horizontal">
                                                <div style="display: none;">
                                                    <div id="PaymentDetailsInformation" class="childDivSection">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading">
                                                                Due Payment Information</div>
                                                            <div class="panel-body">
                                                                <div class="form-horizontal">
                                                                    <div class="form-group">
                                                                        <div class="col-md-2">
                                                                            <asp:Label ID="lblPayMode" runat="server" class="control-label" Text="Payment Mode"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                                                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                                                                <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                                                                <asp:ListItem Value="Card">Card</asp:ListItem>
                                                                                <asp:ListItem Value="M-Banking">M-Banking</asp:ListItem>
                                                                                <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                                                                <asp:ListItem Value="Other Room">Guest Room</asp:ListItem>
                                                                                <asp:ListItem Value="Employee">Employee</asp:ListItem>
                                                                                <asp:ListItem Value="Company">Company</asp:ListItem>
                                                                                <%--<asp:ListItem Value="Member">Member</asp:ListItem>--%>
                                                                                <asp:ListItem>Refund</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <asp:Label ID="lblCurrencyType" runat="server" class="control-label required-field"
                                                                                Text="Currency Type"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:DropDownList ID="ddlCurrency" TabIndex="6" CssClass="form-control" runat="server">
                                                                            </asp:DropDownList>
                                                                            <asp:Label ID="lblDisplayConvertionRate" runat="server" class="control-label" Text=""></asp:Label>
                                                                            <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                                                                        </div>
                                                                    </div>
                                                                    <div id="RoomNumberDiv" style="display: none;">
                                                                        <div class="form-group" style="display: none;">
                                                                            <div class="col-md-2">
                                                                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Room Number"></asp:Label>
                                                                            </div>
                                                                            <div class="col-md-4">
                                                                                <asp:DropDownList ID="DropDownList1" TabIndex="12" runat="server" CssClass="form-control"
                                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlRoomId_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <div class="col-md-2">
                                                                                <asp:Label ID="Label7" runat="server" class="control-label" Text="Room Number"></asp:Label>
                                                                            </div>
                                                                            <div class="col-md-4">
                                                                                <asp:DropDownList ID="ddlRoomNumberId" runat="server" CssClass="form-control">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div class="col-md-2">
                                                                                <asp:Label ID="lblPaidService" runat="server" class="control-label" Text="Paid Service"></asp:Label>
                                                                            </div>
                                                                            <div class="col-md-4">
                                                                                <asp:DropDownList ID="ddlPaidService" runat="server" CssClass="form-control">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <div class="col-md-2">
                                                                            <asp:Label ID="lblReceiveLeadgerAmount" runat="server" class="control-label required-field"
                                                                                Text="Receive Amount"></asp:Label>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:TextBox ID="txtReceiveLeadgerAmount" runat="server" CssClass="form-control"
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
                                                                            <div id="ChequeReceiveAccountsInfo" style="display: none;">
                                                                                <asp:DropDownList ID="ddlChequeReceiveAccountsInfo" CssClass="form-control" runat="server">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                            <div id="MBankingReceiveAccountsInfo" style="display: none;">
                                                                                <asp:DropDownList ID="ddlMBankingReceiveAccountsInfo" runat="server">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div id="EmployeeInfoDiv" style="display: none;">
                                                                        <div class="form-group">
                                                                            <div class="col-md-2">
                                                                                <asp:Label ID="lblEmployeeName" runat="server" class="control-label" Text="Employee Name"></asp:Label>
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
                                                                                <asp:Label ID="lblCompanyName" runat="server" class="control-label" Text="Company Name"></asp:Label>
                                                                            </div>
                                                                            <div class="col-md-10">
                                                                                <asp:DropDownList ID="ddlCompanyName" TabIndex="12" runat="server" CssClass="form-control">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div id="PaidByOtherRoomDiv" style="display: none">
                                                                        <div class="form-group">
                                                                            <div class="col-md-2">
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
                                                                            <div class="col-md-2">
                                                                                <asp:Label ID="lblChecqueNumber" runat="server" class="control-label" Text="Cheque Number"></asp:Label>
                                                                            </div>
                                                                            <div class="col-md-10">
                                                                                <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <div class="col-md-2">
                                                                                <asp:Label ID="lblChequeBankId" runat="server" class="control-label required-field"
                                                                                    Text="Bank Name"></asp:Label>
                                                                            </div>
                                                                            <div class="col-md-10">
                                                                                <input id="txtChequeBankId" type="text" class="form-control" />
                                                                                <div style="display: none;">
                                                                                    <asp:DropDownList ID="ddlChequeBankId" runat="server" CssClass="form-control" AutoPostBack="false">
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
                                                                    <div class="row" style="padding-left: 10px;">
                                                                        <div class="col-md-12">
                                                                            <input type="button" id="btnAddDetailGuestPayment" value="Add" class="TransactionalButton btn btn-primary btn-sm"
                                                                                onclientclick="javascript: return ValidateForm();" />
                                                                            <asp:Label ID="lblHiddenIdDetailGuestPayment" runat="server" Text='' Visible="False"></asp:Label>
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
                                                                            CssClass="totalAmout" Visible="false"></asp:Label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-md-2">
                                                            <asp:Label ID="lbl" runat="server" class="control-label" Text="Remarks"></asp:Label>
                                                        </div>
                                                        <div class="col-md-10">
                                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                                                TabIndex="27"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="padding-top: 10px;">
                                                        <div class="col-md-12">
                                                            <asp:HiddenField ID="hfGuestPaymentDetailsInformationDiv" runat="server"></asp:HiddenField>
                                                            <asp:Button ID="btnSave" runat="server" TabIndex="13" Text="Settlement" CssClass="TransactionalButton btn btn-primary btn-sm"
                                                                OnClick="btnSave_Click" OnClientClick="javascript: return ConfirmCheckOut();" />
                                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                                                                TabIndex="14" OnClick="btnCancel_Click" />
                                                        </div>
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
                                    Item Detail Information</div>
                                <div class="panel-body">
                                    <div>
                                        <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                                        <asp:GridView ID="gvItemDetail" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="200"
                                            CssClass="table table-bordered table-condensed table-responsive">
                                            <RowStyle BackColor="#E3EAEB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="ReservationId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReservationId" runat="server" Text='<%#Eval("LCDetailId") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName") %>'></asp:Label></ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemUnit" runat="server" Text='<%# bind("Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="10%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitPrice" runat="server" Text='<%# bind("PurchasePrice") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="6%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Line Total">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemTotal" runat="server" Text='<%# bind("ItemTotal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="10%"></ItemStyle>
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
                                    <asp:Label ID="lblTotalItemAmount" runat="server" class="control-label" Text=''></asp:Label>
                                    <asp:HiddenField ID="hfTotalItemAmount" runat="server" />
                                </div>
                            </div>
                            <div id="AddMoreItemInformationDiv" style="display: none;" class="panel panel-default">
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblNumberOfPersonAdult" runat="server" class="control-label required-field"
                                                    Text="Number Of Adult"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtNumberOfPersonAdult" runat="server" CssClass="form-control" TabIndex="17"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblNumberOfPersonChild" runat="server" class="control-label" Text="Number Of Child"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtNumberOfPersonChild" runat="server" CssClass="form-control" TabIndex="18"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="ItemDetailsInformation" class="panel panel-default">
                                            <div class="panel-heading">
                                                Item Information</div>
                                            <div class="panel-body">
                                                <div class="form-horizontal">
                                                    <div id="ItemDetails">
                                                        <div class="form-group">
                                                            <div class="col-md-2">
                                                                <asp:Label ID="lblItemType" runat="server" class="control-label required-field" Text="Category Name"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-control" TabIndex="20">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:Label ID="lblItemId" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:HiddenField ID="txtHiddenProductId" runat="server" />
                                                                <asp:DropDownList ID="ddlItemId" runat="server" CssClass="form-control" TabIndex="21">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-2">
                                                                <asp:Label ID="lblUnitPrice" runat="server" class="control-label required-field"
                                                                    Text="Unit Price"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" TabIndex="22"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:Label ID="lblItemUnit" runat="server" class="control-label required-field" Text="Item Unit"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtItemUnit" runat="server" CssClass="form-control" TabIndex="23"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-2">
                                                                &nbsp;
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div style="float: left">
                                                                    <asp:CheckBox ID="chkIscomplementary" TabIndex="24" runat="Server" Text="" CssClass="mycheckbox" />
                                                                    <span style="font-size: 1.0em; text-align: right;">Is it a complementary Item?</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="row">
                                                                <div class="col-md-2">
                                                                </div>
                                                                <div class="col-md-10">
                                                                    <input type="button" id="btnAddDetailItem" value="Add" class="TransactionalButton btn btn-primary btn-sm" />
                                                                    <asp:Label ID="Label8" runat="server" class="control-label" Text='' Visible="False"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="panel-body">
                                                                <div id="ltlTableWiseItemInformation" runat="server" clientidmode="Static">
                                                                </div>
                                                            </div>
                                                            <div id="Div3" class="totalAmout">
                                                            </div>
                                                            <div id="Div4" class="totalAmout">
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <asp:Button ID="btnAddMoreProcess" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                                                TabIndex="31" OnClientClick="javascript:return ValidationNPreprocess();" />
                                                            <input type="button" id="btnCancelAddMoreItemInfo" class="TransactionalButton btn btn-primary btn-sm"
                                                                style="width: 80px;" value="Cancel" />
                                                            <%--OnClick="btnCancel_Click"--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="display: none;">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblIndividualRoomVatAmount" runat="server" class="control-label" Text="Vat Amount"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtIndividualRoomVatAmount" runat="server" CssClass="form-control"
                                                        ReadOnly="True">0</asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblIndividualRoomServiceCharge" runat="server" class="control-label"
                                                        Text="Service Charge"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtIndividualRoomServiceCharge" runat="server" CssClass="form-control"
                                                        ReadOnly="True">0</asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblIndividualRoomDiscountAmount" runat="server" class="control-label"
                                                        Text="Discount Amount"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtIndividualRoomDiscountAmount" runat="server" CssClass="form-control"
                                                        ReadOnly="True">0</asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="lblIndividualRoomGrandTotal" runat="server" class="control-label"
                                                        Text="Total Amount"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtIndividualRoomGrandTotal" runat="server" CssClass="form-control"
                                                        ReadOnly="true">0</asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="tab-3">
                            <div id="Div1" class="panel panel-default">
                                <div class="panel-heading">
                                    Overhead Detail Information</div>
                                <div class="panel-body">
                                    <div>
                                        <asp:Label ID="Label2" runat="server" class="control-label" Text='' Visible="False"></asp:Label>
                                        <asp:GridView ID="gvOverheadDetail" Width="100%" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                            ForeColor="#333333" PageSize="200" CssClass="table table-bordered table-condensed table-responsive">
                                            <RowStyle BackColor="#E3EAEB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="ExpenseId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExpenseId" runat="server" Text='<%#Eval("ExpenseId") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Over Head Name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOverHeadName" runat="server" Text='<%#Eval("OverHeadName") %>'></asp:Label></ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Expense Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# bind("ExpenseAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="10%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Currency">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurrencyName" runat="server" Text='<%# bind("CurrencyName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="10%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="C. Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblConvertionRate" runat="server" Text='<%# bind("ConvertionRate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="10%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Expense">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExpenseAmount" runat="server" Text='<%# bind("LocalCurrencyAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="10%"></ItemStyle>
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
                                    <asp:Label ID="lblTotalOverHeadExpenseAmount" runat="server" class="control-label" Text=''></asp:Label>
                                    <asp:HiddenField ID="hfTotalOverHeadExpenseAmount" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div id="tab-4">
                            <div id="Div6" class="panel panel-default">
                                <div class="panel-heading">
                                    Payment Detail Information</div>
                                <div class="panel-body">
                                    <div>
                                        <asp:Label ID="Label9" runat="server" class="control-label" Text='' Visible="False"></asp:Label>
                                        <asp:GridView ID="gvPaymentDetail" Width="100%" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                            ForeColor="#333333" PageSize="200" CssClass="table table-bordered table-condensed table-responsive">
                                            <RowStyle BackColor="#E3EAEB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="PaymentId" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPaymentId" runat="server" Text='<%#Eval("PaymentId") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Account Head" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccountHeadName" runat="server" Text='<%#Eval("AccountHeadName") %>'></asp:Label></ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPaymentAmount" runat="server" Text='<%# bind("Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="10%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Currency">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurrencyName" runat="server" Text='<%# bind("CurrencyName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="10%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="C. Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblConvertionRate" runat="server" Text='<%# bind("ConvertionRate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="10%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Payment">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# bind("LocalCurrencyAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="10%"></ItemStyle>
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
                                    <asp:Label ID="lblTotalPaymentAmount" runat="server" class="control-label" Text=''></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var paymentDetails = $("#<%=hfGuestPaymentDetailsInformationDiv.ClientID %>").val();
        if (paymentDetails > 0) {
            $('#PaymentDetailsInformation').show();
        }
        else {
            $('#PaymentDetailsInformation').hide();
        }
    </script>
</asp:Content>
