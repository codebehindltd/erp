<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmRoomRegistrationNew.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmRoomRegistrationNew"
    EnableEventValidation="false" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var vv = [];
        var docv = [];
        var docc = [];
        var GG = {};
        var minCheckInDate = "";

        var SelectdPreferenceId = "";
        var TotalRoomRateGlobalValue = 0;
        var MandaoryFieldsList = "";

        var optionData = [];

        var SearchRateChart;
        $(document).ready(function () {
            $("#ContentPlaceHolder1_hfInitialCurrencyType").val($("#<%=ddlCurrency.ClientID %>").val());
            $("#ContentPlaceHolder1_hfPreviousCurrencyType").val($("#<%=ddlCurrency.ClientID %>").val());

            //$("#chkYesBlock").bind("click", false);

            minCheckInDate = $("#<%=txtDisplayCheckInDate.ClientID %>").val();

            $("#btnClearGuest").click(function () {
                LoadGuestClear();
            });

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            if ($("#ContentPlaceHolder1_hfMandatoryFields").val() != "") {
                MandaoryFieldsList = JSON.parse($("#ContentPlaceHolder1_hfMandatoryFields").val());

            }

            CommonHelper.ApplyDecimalValidation();
            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtChequeBankId", "ContentPlaceHolder1_ddlChequeBankId", "ContentPlaceHolder1_ddlChequeBankId");
            CommonHelper.AutoSearchClientDataSource("txtCompany", "ContentPlaceHolder1_ddlCompanyName", "ContentPlaceHolder1_ddlCompanyName");
            CommonHelper.AutoSearchClientDataSource("txtDepartureAireLineFlightName", "ContentPlaceHolder1_ddlDepartureFlightName", "ContentPlaceHolder1_ddlDepartureFlightName");

            if ($("#ContentPlaceHolder1_hfInclusiveHotelManagementBill").val() == "1") {
                $("#btnRoomRateCalculation").hide();
            }

            if ($("#<%=chkIsExpectedCheckOutTimeEnable.ClientID %>").is(':checked')) {
                $("#<%=txtProbableDepartureTime.ClientID %>").attr("disabled", false);
            }
            else {
                $("#<%=txtProbableDepartureTime.ClientID %>").attr("disabled", true);
                $("#<%=txtProbableDepartureTime.ClientID %>").val("");
            }
            
            $("#ContentPlaceHolder1_ddlReferenceId").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlMarketSegment").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSearchCountry").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlGuestSource").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlMealPlanId").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlProfessionId").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlDepartureFlightName").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtProbableDepartureTime').timepicker({
                showPeriod: is12HourFormat
            });

            $("#ContentPlaceHolder1_txtListedContactPerson").autocomplete({

                source: function (request, response) {
                    let url = 'frmRoomReservationNew.aspx/GetContactInformationByCompanyIdNSearchText';
                    let companyId = $("#ContentPlaceHolder1_ddlCompanyName").val();
                    let searchText = $("#ContentPlaceHolder1_txtListedContactPerson").val();

                    if (companyId == "0") {
                        toastr.warning("Please Select Comapany.");
                        $("#txtCompany").focus();
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: url,
                        data: JSON.stringify({ companyId: companyId, searchText: searchText }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.Id,
                                    PersonalAddress: m.PersonalAddress,
                                    MobilePersonal: m.MobilePersonal,
                                    Email: m.Email
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfContactId").val(ui.item.value);
                    $("#<%=txtContactPerson.ClientID %>").val(ui.item.Name);
                    $("#<%=txtContactNumber.ClientID %>").val(ui.item.MobilePersonal);
                }
            });
           $(function checkUncheck() {
                $("[id*=chkAll]").bind("click", function () {
                    if ($(this).is(":checked")) {
                        $("[id*=chkComplementaryItem] input").attr("checked", "checked");
                    } else {
                        $("[id*=chkComplementaryItem] input").removeAttr("checked");
                    }
                    document.getElementById("<%= lblTotalSelectedEmailCount.ClientID %>").innerHTML = $("[id*=chkComplementaryItem] input:checked").length + " item(s) selected";
                });
                $("[id*=chkComplementaryItem] input").bind("click", function () {
                    if ($("[id*=chkComplementaryItem] input:checked").length == $("[id*=cblSubscribedUsers] input").length) {
                        $("[id*=chkAll]").attr("checked", "checked");
                    } else {
                        $("[id*=chkAll]").removeAttr("checked");
                    }
                    document.getElementById("<%= lblTotalSelectedEmailCount.ClientID %>").innerHTML = $("[id*=chkComplementaryItem] input:checked").length + " item(s) selected";
                });
            });

            $("#myTabs").tabs();
            $("#txtReservationId").attr("disabled", true);
            $("#<%=imgReservationSearch.ClientID %>").attr("disabled", true);
            CommonHelper.AutoSearchClientDataSource("txtReservationId", "ContentPlaceHolder1_ddlReservationId", "ContentPlaceHolder1_ddlReservationId");
            $('#ContentPlaceHolder1_txtDepartureHour').timepicker({
                showPeriod: is12HourFormat
            });

            $("#txtReservationId").blur(function () {
                ReservationDropDownChangeEvent();
            });

            $("#<%=ddlTitle.ClientID %>").change(function () {
                AutoGenderLoadInfo();
            });

            $("#ContentPlaceHolder1_ddlTitle").select({
                tags: "true",
                //placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#<%=txtNumberOfPersonAdult.ClientID %>").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    $("#<%=txtNumberOfPersonAdult.ClientID %>").focus();
                    return false;
                }
            });

            $("#<%=txtNumberOfPersonChild.ClientID %>").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    $("#<%=txtNumberOfPersonChild.ClientID %>").focus();
                    return false;
                }
            });

            $("#ContentPlaceHolder1_ddlTitle").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();
                //if (title == "MrNMrs.") {
                //    title = "Mr. & Mrs.";
                //}
                //else 
                if (title == "N/A") {
                    titleText = "";
                }
                //else {
                //    title = titleText;
                //}
                var firstName = $("#<%=txtFirstName.ClientID %>").val().trim();
                var lastName = $("#<%=txtLastName.ClientID %>").val().trim();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);
                }
                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            $("#ContentPlaceHolder1_txtFirstName").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();
                //if (title == "MrNMrs.") {
                //    title = "Mr. & Mrs.";
                //}
                //else 
                if (title == "N/A") {
                    titleText = "";
                }
                //else {
                //    title = titleText;
                //}
                var firstName = $("#<%=txtFirstName.ClientID %>").val().trim();
                var lastName = $("#<%=txtLastName.ClientID %>").val().trim();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);

                }
                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            $("#ContentPlaceHolder1_txtLastName").blur(function () {
                var title = $("#<%=ddlTitle.ClientID %>").val();

                var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();
                //if (title == "MrNMrs.") {
                //    title = "Mr. & Mrs.";
                //}
                //else
                if (title == "N/A") {
                    titleText = "";
                }
                //else {
                //    title = titleText;
                //}
                var firstName = $("#<%=txtFirstName.ClientID %>").val().trim();
                var lastName = $("#<%=txtLastName.ClientID %>").val().trim();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);
                }

                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            GetTempRegistrationDetailByWM();
            TotalRoomRateVatServiceChargeCalculation();

            if ($("#ContentPlaceHolder1_hfRegistrationId").val() != "") {
                $("#hfPaidServiceDialogDisplayOrNot").val("0");
                AddServiceCharge();
            }

            var qsReservationId = $("#<%=QSReservationId.ClientID %>").val().split('~')[0];;

            var ddlReservationId = '<%=ddlReservationId.ClientID%>'
            var chkIsFromReservation = '<%=chkIsFromReservation.ClientID%>'

            $('#' + chkIsFromReservation).change(function () {
                if ($('#' + chkIsFromReservation).is(':checked')) {
                    $("#BtnSearchReservation").show();
                }
                else {
                    $("#BtnSearchReservation").hide();
                }
            });

            CurrencyRateInfoEnable();

            // //-- Airport Pick Up Information Div ------------------
            if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "NO") {

                $('#AirportPickUpInformationDiv').hide();

            }

            else if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "0") {
                $('#AirportPickUpInformationDiv').hide();

            }
            else {
                $('#AirportPickUpInformationDiv').show();
            }
            $("#<%=ddlAirportPickUp.ClientID %>").change(function () {
                if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "0") {
                    $('#AirportPickUpInformationDiv').hide();
                    $("#<%=txtArrivalFlightName.ClientID %>").val("");
                    $("#<%=txtArrivalFlightNumber.ClientID %>").val("");
                }
                else if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "NO") {
                    $('#AirportPickUpInformationDiv').hide();
                    $("#<%=txtArrivalFlightName.ClientID %>").val("");
                    $("#<%=txtArrivalFlightNumber.ClientID %>").val("");
                }
                else {
                    $('#AirportPickUpInformationDiv').show();
                }

            });

            // //-- Airport Drop Information Div ------------------
            if ($("#<%=ddlAirportDrop.ClientID %>").val() == "NO") {
                $('#AirportDropInformationDiv').hide();
                $("#<%=ddlDepartureFlightName.ClientID %>").val("0");

                $("#<%=txtDepartureFlightNumber.ClientID %>").val("");
            }
            else if ($("#<%=ddlAirportDrop.ClientID %>").val() == "0") {
                $('#AirportDropInformationDiv').hide();
                $("#<%=ddlDepartureFlightName.ClientID %>").val("0");
                $("#<%=txtDepartureFlightNumber.ClientID %>").val("");
            }
            else {
                $('#AirportDropInformationDiv').show();
            }
            $("#<%=ddlAirportDrop.ClientID %>").change(function () {
                if ($("#<%=ddlAirportDrop.ClientID %>").val() == "0") {

                    $('#AirportDropInformationDiv').hide();
                    $("#<%=ddlDepartureFlightName.ClientID %>").val("0");
                    $("#<%=txtDepartureFlightNumber.ClientID %>").val("");

                }
                else if ($("#<%=ddlAirportDrop.ClientID %>").val() == "NO") {
                    $('#AirportDropInformationDiv').hide();
                    $("#<%=ddlDepartureFlightName.ClientID %>").val("0");
                    $("#<%=txtDepartureFlightNumber.ClientID %>").val("");
                }
                else {
                    $('#AirportDropInformationDiv').show();
                }

            });

            var ddlIsCompanyGuest = '<%=ddlIsCompanyGuest.ClientID%>'
            var ddlIsHouseUseRoom = '<%=ddlIsHouseUseRoom.ClientID%>'
            if ($('#' + ddlIsCompanyGuest).val() == "Yes") {
                $('#' + ddlIsHouseUseRoom).attr("disabled", false);
            }
            else {
                $('#' + ddlIsHouseUseRoom).val("No");
                $('#' + ddlIsHouseUseRoom).attr("disabled", true);
            }

            $('#' + ddlIsCompanyGuest).change(function () {
                if ($('#' + ddlIsCompanyGuest).val() == "Yes") {
                    $('#' + ddlIsHouseUseRoom).attr("disabled", false);
                }
                else {
                    $('#' + ddlIsHouseUseRoom).val("No");
                    $('#' + ddlIsHouseUseRoom).attr("disabled", true);
                }
            });

            var ddlPaymentType = '<%=ddlPaymentType.ClientID%>'

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            $('#' + ddlPaymentType).change(function () {
                if ($('#' + ddlPaymentType).val() == "PaidOut") {
                    $('#' + ddlPayMode).val('Cash');
                    $('#' + ddlPayMode).attr("disabled", true);

                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#PaidByOtherRoomDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').show();
                    $('#BankPaymentAccountHeadDiv').hide();
                    $('#ComPaymentDiv').hide();
                    $('#PrintPreviewDiv').show();
                }
                else {
                    $('#' + ddlPayMode).attr("disabled", false);
                    if ($('#' + ddlPayMode).val() == "Cash") {
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').show();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else {
                        $('#CardPaymentAccountHeadDiv').show();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                }
            });

            if ($("#<%= chkIsVIPGuest.ClientID %>").prop("checked") == true) {

                $("#<%= ddlVIPGuestType.ClientID %>").show();
            }
            else {
                $("#<%= ddlVIPGuestType.ClientID %>").hide();
            }

            $("#<%= chkIsVIPGuest.ClientID %>").click(function () {
                if ($(this).prop("checked") === true) {
                    $("#<%= ddlVIPGuestType.ClientID %>").show();
                }
                else {
                    $("#<%= ddlVIPGuestType.ClientID %>").hide();
                }
            });

            var txtConversionRate = '<%=txtConversionRate.ClientID%>'
            $('#' + txtConversionRate).blur(function () {
                if (isNaN($('#' + txtConversionRate).val())) {
                    toastr.warning('Entered Conversion Rate is not in correct format.');
                    return;
                }
            });

            var txtDiscountAmount = '<%=txtDiscountAmount.ClientID%>'

            $('#' + txtDiscountAmount).blur(function () {
                var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtUnitPrice"), "Discount Amount");
                if (checkValue == false) {
                    return false;
                }


                if (isNaN($('#' + txtDiscountAmount).val())) {
                    toastr.warning('Entered Discount Amount is not in correct format.');
                    return;
                }
                UpdateDiscountAmount();
                TotalRoomRateVatServiceChargeCalculation();
            });


            var txtRoomRate = '<%=txtRoomRate.ClientID%>'
            $('#' + txtRoomRate).blur(function () {
                if (isNaN($('#' + txtRoomRate).val())) {
                    toastr.warning('Entered Negotiated Rate is not in correct format.');
                    return;
                }

                if (CommonHelper.IsDecimal($('#' + txtRoomRate).val()) == false) {
                    toastr.warning('Entered Negotiated Rate is not in correct format.');
                    return;
                }
            });

            var txtReceiveLeadgerAmount = '<%=txtReceiveLeadgerAmount.ClientID%>'

            $('#' + txtReceiveLeadgerAmount).blur(function () {
                if (isNaN($('#' + txtReceiveLeadgerAmount).val())) {
                    toastr.warning('Entered Negotiated Rate is not in correct format.');
                    return;
                }
            });


            var txtPaymentConversionRate = '<%=txtPaymentConversionRate.ClientID%>'
            $('#' + txtPaymentConversionRate).blur(function () {
                if (isNaN($('#' + txtPaymentConversionRate).val())) {
                    toastr.warning('Entered Negotiated Rate is not in correct format.');
                    return;
                }
            });

            $("#<%=txtRoomRate.ClientID %>").blur(function () {

                var isPackage = $("#ContentPlaceHolder1_hfPackageId").val() != "0";
                var discountAcount = $("#ContentPlaceHolder1_txtDiscountAmount").val();
                if (isPackage && discountAcount < 0)
                    $("#ContentPlaceHolder1_txtDiscountAmount").val("0");
                if (!isPackage && discountAcount < 0) {
                    toastr.warning("Negotiated Rate Can Not Greater Than Rack Rate.");
                    $("#<%=txtRoomRate.ClientID %>").val($("#ContentPlaceHolder1_txtUnitPriceHiddenField").val());
                    $("#ContentPlaceHolder1_txtDiscountAmount").val("0");
                    $("#<%=txtRoomRate.ClientID %>").focus();

                    return false;
                }
            });

            var ddlPaymentCurrency = '<%=ddlPaymentCurrency.ClientID%>'
            var txtPaymentConversionRate = '<%=txtPaymentConversionRate.ClientID%>'
            var txtReceiveLeadgerAmount = '<%=txtReceiveLeadgerAmount.ClientID%>'
            var txtPaymentCalculatedLedgerAmount = '<%=txtPaymentCalculatedLedgerAmount.ClientID%>'
            var txtPaymentCalculatedLedgerAmountHiddenField = '<%=txtPaymentCalculatedLedgerAmountHiddenField.ClientID%>'

            $('#' + txtReceiveLeadgerAmount).blur(function () {
                if ($('#' + ddlPaymentCurrency).val() == "1") {
                    var LedgerAmount = parseFloat($('#' + txtReceiveLeadgerAmount).val());

                    $('#' + txtPaymentCalculatedLedgerAmount).val(toFixed(LedgerAmount, 2));
                    $('#' + txtPaymentCalculatedLedgerAmountHiddenField).val(toFixed(LedgerAmount, 2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtReceiveLeadgerAmount).val()) * parseFloat($('#' + txtPaymentConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtPaymentCalculatedLedgerAmount).val(toFixed(LedgerAmount, 2));
                    $('#' + txtPaymentCalculatedLedgerAmountHiddenField).val(toFixed(LedgerAmount, 2));
                    $('#' + txtPaymentCalculatedLedgerAmount).attr("disabled", true);
                }
            });

            $('#' + txtPaymentConversionRate).blur(function () {
                if ($('#' + ddlPaymentCurrency).val() == "1") {
                    var LedgerAmount = parseFloat($('#' + txtReceiveLeadgerAmount).val());
                    $('#' + txtPaymentCalculatedLedgerAmount).val(toFixed(LedgerAmount, 2));
                    $('#' + txtPaymentCalculatedLedgerAmountHiddenField).val(toFixed(LedgerAmount, 2));
                }
                else {
                    var LedgerAmount = parseFloat($('#' + txtReceiveLeadgerAmount).val()) * parseFloat($('#' + txtPaymentConversionRate).val());
                    if (isNaN(LedgerAmount.toString())) {
                        LedgerAmount = 0;
                    }
                    $('#' + txtPaymentCalculatedLedgerAmount).val(toFixed(LedgerAmount, 2));
                    $('#' + txtPaymentCalculatedLedgerAmountHiddenField).val(toFixed(LedgerAmount, 2));
                    $('#' + txtPaymentCalculatedLedgerAmount).attr("disabled", true);
                }
            });

            var selectedIndex = parseFloat($('#' + ddlPaymentCurrency).prop("selectedIndex"));
            if (selectedIndex < 1) {
                $('#' + txtPaymentConversionRate).val("")
                $('#' + txtPaymentCalculatedLedgerAmount).val("");
                $('#ConversionPanel').hide();
                $('#' + txtPaymentCalculatedLedgerAmount).attr("disabled", false);
            }
            else {
                $('#ConversionPanel').hide();
                $('#' + txtPaymentCalculatedLedgerAmount).val("");
            }

            $('#' + ddlPaymentCurrency).change(function () {
                var v = $("#<%=ddlPaymentCurrency.ClientID %>").val();

                PageMethods.LoadCurrencyType(v, OnLoadPaymentCurrencyTypeSucceeded, OnLoadPaymentCurrencyTypeFailed);
            });

            if (qsReservationId != 0) {
                var strQSReservationIdValue = '<%=QSReservationIdValue %>';

                ToggleFieldVisible();
                $('#divReservationGuest').show();
                $('#' + chkIsFromReservation).prop("checked", true);
                PopulateReservation(0);


                if (strQSReservationIdValue == "0") {
                    SetRelatedDataByReservationId(qsReservationId);
                    LoadComplementaryItemByWM(qsReservationId);


                    PopulateGuestList();
                    $('#divReservationGuest').show();
                    var roomType = $('#<%=ddlRoomType.ClientID%>').val();
                    SetRegistrationInfoByRoomTypeId(roomType, "");
                }
                else {

                    CommonHelper.SpinnerClose();
                    var ss = strQSReservationIdValue.split('~');

                    SetRelatedDataByReservationId(ss[0]);
                    LoadComplementaryItemByWM(ss[0]);

                    var reservationId = ss[0];
                    var reservationDetailId = ss[1];
                    var roomId = ss[2];
                    LoadReservationGuestInformation(reservationId, reservationDetailId, roomId, 1);

                    PopulateGuestList();
                    $('#divReservationGuest').show();
                }
            }
            else {
                $('#divReservationGuest').hide();
            }

            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            //PageMethods.LoadCurrencyType(ddlCurrency, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);

            $("#<%=ddlCurrency.ClientID %>").change(function () {
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
                PerformFillFormActionByTypeId($('#<%=ddlRoomType.ClientID%>').val());
                if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                }
                else {
                    $("#<%=txtConversionRate.ClientID %>").val(result.BillingConversionRate);

                    $("#<%=hfConversionRate.ClientID %>").val(result.BillingConversionRate);
                }


                $('#ContentPlaceHolder1_hfIsPaidServiceAlreadyLoded').val('0');
                $("#<%=hfPaidServiceSaveObj.ClientID %>").val("");
                alreadySavePaidServices = [];
                $("#ContentPlaceHolder1_hfIsCurrencyChange").val("1");
                TotalRoomRateVatServiceChargeCalculation();
                CurrencyRateInfoEnable();
            }
            function OnLoadConversionRateFailed() {
            }

            function AutoGenderLoadInfo() {
                var titleSex = $('#<%=ddlTitle.ClientID%>').val();

                //toastr.info(titleSex);


                if (titleSex == "Mr.") {
                    $('#<%=ddlGuestSex.ClientID%>').val("Male");
                }
                else if (titleSex == "0") {
                    $('#<%=ddlGuestSex.ClientID%>').val("0");
                }
                else if (titleSex == "MrNMrs.") {
                    $('#<%=ddlGuestSex.ClientID%>').val("0");
                }
                else if (titleSex == "Dr.") {
                    $('#<%=ddlGuestSex.ClientID%>').val("0");
                }
                else if (titleSex == "N/A") {
                    $('#<%=ddlGuestSex.ClientID%>').val("0");
                }
                else {
                    $('#<%=ddlGuestSex.ClientID%>').val("Female");
                }

            }
            function OnLoadPaymentCurrencyTypeSucceeded(result) {
                $("#<%=hfPaymentCurrencyType.ClientID %>").val(result.CurrencyType);

                PageMethods.LoadCurrencyConversionRate(result.CurrencyId, OnLoadPaymentConversionRateSucceeded, OnLoadPaymentConversionRateFailed);
            }

            function OnLoadPaymentCurrencyTypeFailed() {
            }

            function OnLoadPaymentConversionRateSucceeded(result) {
                if ($("#<%=hfPaymentCurrencyType.ClientID %>").val() == "Local") {
                    $('#' + txtPaymentConversionRate).val("")
                    $('#' + txtPaymentCalculatedLedgerAmount).val("");
                    $('#ConversionPanel').hide();
                    $('#' + txtPaymentCalculatedLedgerAmount).attr("disabled", false);
                }
                else {
                    $('#ConversionPanel').show();
                    $('#' + txtPaymentConversionRate).val(result.BillingConversionRate);
                    $('#' + txtPaymentConversionRate).attr("disabled", true);
                }
            }
            function OnLoadPaymentConversionRateFailed() {
            }
            $("#ContentPlaceHolder1_ddlIsCompanyGuest").change(function () {


                if ($(this).val() == 'No') {
                    if ($("#ContentPlaceHolder1_txtRoomRate").val() == "0.00" || $("#ContentPlaceHolder1_txtRoomRate").val() == "0") {
                        $("#ContentPlaceHolder1_txtDiscountAmount").val("0");
                        PerformFillFormActionByTypeId($('#<%=ddlRoomType.ClientID%>').val());

                        if ($("#ContentPlaceHolder1_hfRegistrationId").val() != "") {
                            $("#ContentPlaceHolder1_hfIsComplementaryPaidService").val("0");
                            $("#hfPaidServiceDialogDisplayOrNot").val("0");
                            AddServiceCharge();
                            setTimeout(CalcualteServiceTotalAfterComplementaryNo, 1000);
                        }
                    }
                }

                else if ($(this).val() == 'Yes') {
                }
            });

            $("#txtGuestCountrySearch").blur(function () {
                var countryId = $("#<%=ddlGuestCountry.ClientID %>").val();

                PageMethods.GetNationality(countryId, OnCountrySucceeded, OnCountryFailed);
            });
            //auto search country name
            CommonHelper.AutoSearchClientDataSource("txtGuestCountrySearch", "ContentPlaceHolder1_ddlGuestCountry", "ContentPlaceHolder1_ddlGuestCountry");

            $('#ContentPlaceHolder1_A').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_B').click(function () {
                $('#SubmitButtonDiv').hide();
            });
            $('#ContentPlaceHolder1_C').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_D').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_E').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_F').click(function () {
                $('#SubmitButtonDiv').show();
            });

            var ddlRoomId = '<%=ddlRoomId.ClientID%>'

            var ddlRoomIdHiddenField = '<%=ddlRoomIdHiddenField.ClientID%>'
            $('#' + ddlRoomId).change(function (event) {
                var roomId = $('#' + ddlRoomId).val();
                $('#' + ddlRoomIdHiddenField).val(roomId);
            });

            var ddlRoomType = '<%=ddlRoomType.ClientID%>'
            $('#' + ddlRoomType).change(function (event) {
                //$('#<%=txtDiscountAmount.ClientID%>').val("");
                PopulateRooms();
                PerformFillFormActionByTypeId($('#<%=ddlRoomType.ClientID%>').val());
                UpdateTotalCostWithDiscount();
            });

            function PerformFillFormActionByTypeId(actionId) {
                PageMethods.PerformFillFormActionByTypeId(actionId, OnFillFormObjectSucceededByTypeId, OnFillFormObjectFailedByTypeId);
                return false;
            }
            function OnFillFormObjectSucceededByTypeId(result) {
                vv = result;
                if ($("#<%=ddlCurrency.ClientID %>").val() != "1") {

                    $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRateUSD);
                    $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRateUSD);
                    $("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val(result.MinimumRoomRateUSD);
                    $("#<%=txtRoomRate.ClientID %>").val(toFixed(result.RoomRateUSD, 2));
                }
                else {
                    $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRate);
                    $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRate);
                    $("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val(result.MinimumRoomRate);
                    $("#<%=txtRoomRate.ClientID %>").val(toFixed(result.RoomRate, 2));
                }
                UpdateTotalCostWithDiscount();
                TotalRoomRateVatServiceChargeCalculation();
                return false;
            }

            function OnFillFormObjectFailedByTypeId(error) {
                toastr.error(error.get_message());
            }

            $("#<%=ddlBusinessPromotionId.ClientID %>").change(function () {

                GetTotalCostWithCompanyOrPersonalDiscount();
            });


            $("#btnAddDetailGuest").click(function () {
                debugger;
                // Xtra Validation..............................
                PageMethods.LoadReservationNRegistrationXtraValidation(OnLoadXtraValidationSucceeded, OnLoadXtraValidationFailed);
            });


            var ddlReservationGuest = '<%=ddlReservationGuest.ClientID%>'
            $('#' + ddlReservationGuest).change(function (event) {
                var guestId = $('#' + ddlReservationGuest).val();

                if (guestId != 0) {


                    $("#<%=EditId.ClientID %>").val(guestId);
                    $("#hfSearchDetailsFireOrNot").val("0");
                    PageMethods.LoadDetailInformation(guestId, OnLoadParentFromDetailObjectSucceeded, OnLoadParentFromDetailObjectFailed);
                    return false;
                }
                else {
                    clearUserDetailsControl();
                }
            });

            var hiddendReservationId = '<%=hiddendReservationId.ClientID%>'
            var ReservationId = $('#' + hiddendReservationId).val();
            if (parseInt(ReservationId) > 0) {
                $("#chkIsFromReservation").prop("checked", true);
                var targetControl = $('#<%= ddlReservationId.ClientID %>');

                targetControl.attr("disabled", false);
            }
            else {
                var ddlReservationId = '<%=ddlReservationId.ClientID%>'
                $("#chkIsFromReservation").prop("checked", false);
                $("#" + ddlReservationId).val('0~0~0');
                var targetControl = $('#<%= ddlReservationId.ClientID %>');
                targetControl.attr("disabled", true);
            }

            var ddlRoomId = '<%=ddlRoomId.ClientID%>'
            var roomId = $('#' + ddlRoomId).val();

            var chkIsFromReservation = '<%=chkIsFromReservation.ClientID%>'

            var ddlReservationId = '<%=ddlReservationId.ClientID%>'

            if ($('#' + chkIsFromReservation).is(':checked')) {
                var isRegistrationEddited = '<%=isRegistrationEddited%>';


                if (isRegistrationEddited > -1) {
                    $('#' + ddlReservationId).attr("disabled", true)
                    $('#' + chkIsFromReservation).attr("disabled", true)
                }
            }

            $("#<%=ddlCompanyName.ClientID %>").change(function () {
                GetTotalCostWithCompanyOrPersonalDiscount();
            });

            $("#txtCompany").blur(function () {
                var ctrl = '#<%=chkIsLitedCompany.ClientID%>'

                if ($(ctrl).is(':checked')) {
                    var companyId = $("#<%=ddlCompanyName.ClientID %>").val();
                    if (companyId > 0) {
                        GetTotalCostWithCompanyOrPersonalDiscount();
                    }
                    else {
                        toastr.warning("Please select an enlisted company.");
                        return false;
                    }
                }

            });

            $("#<%=txtRoomRate.ClientID %>").blur(function () {
                TotalRoomRateVatServiceChargeCalculation();
            });

            $("#<%=txtConversionRate.ClientID %>").blur(function () {
                UpdateDiscountAmount();
                TotalRoomRateVatServiceChargeCalculation();
            });

            $("#<%=ddlDiscountType.ClientID %>").change(function () {
                var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtUnitPrice"), "Discount Amount");
                if (checkValue == false) {
                    return false;
                }
                UpdateDiscountAmount();
                TotalRoomRateVatServiceChargeCalculation();
            });

            $("#<%=ddlPaymentMode.ClientID %>").change(function () {
                if ($("#<%=ddlPaymentMode.ClientID %>").val() != "Company") {

                    $("#<%=ddlPayFor.ClientID %>").val('0');
                    $("#<%=ddlPayFor.ClientID %>").attr("disabled", true);
                }
                else {
                    $("#<%=ddlPayFor.ClientID %>").attr("disabled", false);
                }
            });

            var txtDepartureDate = '<%=txtDepartureDate.ClientID%>'
            var txtCheckInDateHiddenField = '<%=txtCheckInDateHiddenField.ClientID%>'
            var txtCheckInDate = '<%=txtCheckInDate.ClientID%>'

            $('#ContentPlaceHolder1_txtCheckInDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtDepartureDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtDepartureDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: $("#ContentPlaceHolder1_txtDepartureDate").val(),
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtCheckInDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            var txtGuestDOB = '<%=txtGuestDOB.ClientID%>'

            $('#ContentPlaceHolder1_txtGuestDOB').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                maxDate: 0,
                yearRange: "-100:+0"
            });
            $('#ContentPlaceHolder1_txtSrcDateOfBirth').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                yearRange: "-100:+0"
            });


            var txtVIssueDate = '<%=txtVIssueDate.ClientID%>'
            $('#ContentPlaceHolder1_txtVIssueDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtVExpireDate').datepicker("option", "minDate", selectedDate);
                }
            });
            var txtVExpireDate = '<%=txtVExpireDate.ClientID%>'
            $('#ContentPlaceHolder1_txtVExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                minDate: minCheckInDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtVIssueDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            var txtPIssueDate = '<%=txtPIssueDate.ClientID%>'
            $('#ContentPlaceHolder1_txtPIssueDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtPExpireDate').datepicker("option", "minDate", selectedDate);
                }
            });
            var txtPExpireDate = '<%=txtPExpireDate.ClientID%>'
            $('#ContentPlaceHolder1_txtPExpireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                minDate: minCheckInDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtPIssueDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $("#ContentPlaceHolder1_txtSrcFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSrcToDate").datepicker("option", "minDate", selectedDate);
                }
            });


            $("#ContentPlaceHolder1_txtSrcToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSrcFromDate").datepicker("option", "maxDate", selectedDate);
                }
            });


            $("#ContentPlaceHolder1_txtChkInDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtExpiryDate").datepicker({
                changeMonth: true,
                changeYear: true,
                minDate: minCheckInDate,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtExpireDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtDisplayCheckInDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtRsvCheckInDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtCheckOutDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });


            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Registration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);


            var ddlReservationId = '<%=ddlReservationId.ClientID%>'
            var txtEntiteledRoomType = '<%=txtEntiteledRoomType.ClientID%>'

            $('#' + ddlReservationId).change(function (event) {
                ReservationDropDownChangeEvent();
            });

            function ReservationDropDownChangeEvent() {
                var reservationId = $('#' + ddlReservationId).val().split('~')[0];
                var reservationDetailId = $('#' + ddlReservationId).val().split('~')[1];
                var roomId = $('#' + ddlReservationId).val().split('~')[2];
                SetRelatedDataByReservationId(reservationId);
                LoadReservationGuestInformation(reservationId, reservationDetailId, roomId, 1);
                LoadComplementaryItemByWM(reservationId);
                PopulateGuestListForReservationDropDownChangeEvent(reservationId);
                ////-----//PopulatePaidServiceFormReservation(reservationId);
                $('#divReservationGuest').show();
            }

            function SetRelatedDataByReservationId(reservationId) {
                PageMethods.GetRelatedDataByReservationId(reservationId, SetRelatedDataByReservationIdSucceeded, SetRelatedDataByReservationIdFailed);
                return false;
            }
            function SetRelatedDataByReservationIdSucceeded(result) {
                debugger;
                vv = result;
                var txtDepartureDate = '<%=txtDepartureDate.ClientID%>'

                var ddlCurrency = '<%=ddlCurrency.ClientID%>'
                var ddlBusinessPromotionId = '<%=ddlBusinessPromotionId.ClientID%>'
                var date = new Date(result.DateOut);
                $("#<%=txtReservedCompany.ClientID %>").val(result.ReservedCompany);                

                $('#' + ddlCurrency).val(result.CurrencyType);
                $('#' + ddlBusinessPromotionId).val(result.BusinessPromotionId);
                //$("#<%=ddlDiscountType.ClientID %>").val('Percentage');
                
                $("#<%=ddlReferenceId.ClientID %>").val(result.ReferenceId).trigger('change');
                $("#<%=ddlMarketSegment.ClientID %>").val(result.MarketSegmentId).trigger('change');
                $("#<%=ddlGuestSource.ClientID %>").val(result.GuestSourceId).trigger('change');
                $("#<%=ddlMealPlanId.ClientID %>").val(result.MealPlanId).trigger('change');

                if (result.ClassificationId == "453") {
                    $("#<%=chkIsVIPGuest.ClientID %>").attr("checked", true);
                    if ($("#<%= chkIsVIPGuest.ClientID %>").prop("checked") == true) {
                        $("#<%= ddlVIPGuestType.ClientID %>").show();
                        $("#<%=ddlVIPGuestType.ClientID %>").val(result.VipGuestTypeId);
                        if (result.IsComplementaryGuest)
                        {
                            $("#<%=ddlIsCompanyGuest.ClientID %>").val("Yes");
                        }
                        else
                        {
                            $("#<%=ddlIsCompanyGuest.ClientID %>").val("No");
                        }
                    }
                }
                else if (result.ClassificationId == "454") {
                    $("#<%=ddlIsCompanyGuest.ClientID %>").val("Yes");
                    $("#<%=ddlIsHouseUseRoom.ClientID %>").attr("disabled", false);
                    $("#<%=ddlIsHouseUseRoom.ClientID %>").val("No");
                }
                else if (result.ClassificationId == "455") {
                    $("#<%=ddlIsCompanyGuest.ClientID %>").val("Yes");
                    $("#<%=ddlIsHouseUseRoom.ClientID %>").attr("disabled", false);
                    $("#<%=ddlIsHouseUseRoom.ClientID %>").val("Yes");
                }

                if (result.Remarks.replace("null", "") != "") {
                    $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
                    //toastr.info(result.Remarks);
                }

                if (result.POSRemarks.replace("null", "") != "") {
                    $("#<%=txtPosRemarks.ClientID %>").val(result.POSRemarks);
                }

                var v = $("#<%=ddlCurrency.ClientID %>").val();
                PageMethods.LoadCurrencyType(v, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);

                //-------Aireport Pickup/ Drop Information-----------
                //Pickup Information-----------
                $("#<%=txtArrivalFlightName.ClientID %>").val("");
                $("#<%=txtArrivalFlightNumber.ClientID %>").val("");
                $("#<%=txtArrivalHour.ClientID %>").val("12");
                $("#<%=txtArrivalMin.ClientID %>").val("00");
                $("#<%=ddlArrivalAmPm.ClientID %>").val("AM");
                $("#<%=ddlPaymentMode.ClientID %>").val(result.PaymentMode);

                if ($("#<%=ddlPaymentMode.ClientID %>").val() != "Company") {
                    $("#<%=ddlPayFor.ClientID %>").val('0');
                    $("#<%=ddlPayFor.ClientID %>").attr("disabled", true);
                }
                else {
                    $("#<%=ddlPayFor.ClientID %>").attr("disabled", false);
                }

                if (CommonHelper.IsVaildDate(result.ArrivalTime) == true) {
                    var dateArrival = new Date(result.ArrivalTime);
                    $("#<%=ddlAirportPickUp.ClientID %>").val(result.AirportPickUp);
                    $("#<%=txtArrivalFlightName.ClientID %>").val(result.ArrivalFlightName);
                    $("#<%=txtArrivalFlightNumber.ClientID %>").val(result.ArrivalFlightNumber);

                    var arrivalHour = dateArrival.getHours();
                    if (arrivalHour < 12) {
                        $("#<%=ddlArrivalAmPm.ClientID %>").val("AM");
                    }
                    else {
                        $("#<%=ddlArrivalAmPm.ClientID %>").val("PM");
                    }

                    arrivalHour = arrivalHour % 12;
                    if (arrivalHour == 0) {
                        arrivalHour = 12;
                    }

                    $("#<%=txtArrivalHour.ClientID %>").val(arrivalHour);
                    $("#<%=txtArrivalMin.ClientID %>").val(dateArrival.getMinutes());

                    //Drop Information-----------
                    var dateDeparture = new Date(result.DepartureTime);
                    $("#<%=ddlAirportDrop.ClientID %>").val(result.AirportDrop);
                    $("#<%=hfDepartureAirlineId.ClientID %>").val(result.DepartureAirlineId);
                    $("#<%=ddlDepartureFlightName.ClientID %>").val(result.DepartureAirlineId);
                    $("#<%=txtDepartureFlightNumber.ClientID %>").val(result.DepartureFlightNumber);
                    $("#<%=txtDepartureHour.ClientID %>").val(result.DepartureTimeString);
                    $("#<%=ddlDepartureFlightName.ClientID%>").val(result.DepartureAirlineId).trigger("change");
                }

                if (result.IsListedCompany == true) {
                    $("#<%=chkIsLitedCompany.ClientID %>").attr("checked", true);
                    $("#<%=ddlPayFor.ClientID %>").val(result.PayFor);
                    $('#ListedCompany').show();
                    $('#ReservedCompany').hide();
                    $("#<%=ddlCompanyName.ClientID %>").val(result.CompanyId);
                    $('#txtCompany').val($("#<%=ddlCompanyName.ClientID %> option:selected").text());
                    $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson);
                    $("#<%=txtContactNumber.ClientID %>").val(result.MobileNumber);

                    if (result.IsListedContact) {
                        $("#<%=chkIsLitedContact.ClientID %>").attr("checked", true);
                        $("#<%=hfContactId.ClientID %>").val(result.ContactId);
                        $("#<%=txtListedContactPerson.ClientID %>").val(result.ContactPerson);
                        $("#<%=txtContactNumber.ClientID %>").val(result.MobileNumber);
                    }
                    else {
                        $("#<%=chkIsLitedContact.ClientID %>").attr("checked", false);
                        $("#<%=hfContactId.ClientID %>").val("0");
                        $("#<%=txtListedContactPerson.ClientID %>").val("");
                    }
                }
                else {
                    $("#<%=chkIsLitedCompany.ClientID %>").attr("checked", false);
                    $('#ListedCompany').hide();
                    $('#ReservedCompany').show();
                    $("#<%=ddlCompanyName.ClientID %>").val(0);
                    $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson);
                    $("#<%=txtContactNumber.ClientID %>").val(result.MobileNumber);
                    $("#<%=txtGuestPhone.ClientID %>").val(result.MobileNumber);
                    $("#<%=txtGuestEmail.ClientID %>").val(result.ContactEmail);
                }

                var chkIsLitedCompany = '<%=chkIsLitedCompany.ClientID%>'

                if ($(('#' + chkIsLitedCompany)).attr('checked')) {
                    $('#ReservedCompany').hide();
                    $('#ListedCompany').show();
                    $('#PaymentInformation').show();
                }
                else {
                    $("#<%=txtContactNumber.ClientID %>").val('')
                    $("#<%=txtContactPerson.ClientID %>").val('')
                    $('#ListedCompany').hide();
                    $('#ReservedCompany').show();
                    $('#PaymentInformation').hide();
                }

                if (result.IsFamilyOrCouple == true) {
                    $("#ContentPlaceHolder1_cbFamilyOrCouple").attr('checked', true)
                }


                //$("#ContentPlaceHolder1_txtNumberOfPersonAdult").val(result.NumberOfPersonAdult);
                //$("#ContentPlaceHolder1_txtNumberOfPersonChild").val(result.NumberOfPersonChild);
                $("#ContentPlaceHolder1_txtNumberOfPersonAdult").val("1");
                $("#ContentPlaceHolder1_txtNumberOfPersonChild").val("0");


                // //-- Airport Pick Up Information Div ------------------
                if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "0") {
                    $('#AirportPickUpInformationDiv').hide();
                }
                else if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "NO") {
                    $('#AirportPickUpInformationDiv').hide();
                }
                else {
                    $('#AirportPickUpInformationDiv').show();
                }


                // //-- Airport Drop Information Div ------------------
                if ($("#<%=ddlAirportDrop.ClientID %>").val() == "0") {

                    $('#AirportDropInformationDiv').hide();
                    $("#<%=ddlDepartureFlightName.ClientID %>").val("0");
                    $("#<%=txtDepartureFlightNumber.ClientID %>").val("");

                }
                else if ($("#<%=ddlAirportDrop.ClientID %>").val() == "NO") {
                    $('#AirportDropInformationDiv').hide();
                    $("#<%=ddlDepartureFlightName.ClientID %>").val("0");
                    $("#<%=txtDepartureFlightNumber.ClientID %>").val("");
                }
                else {
                    $('#AirportDropInformationDiv').show();
                }


                $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
                $("#<%=txtDepartureDate.ClientID %>").val(GetStringFromDateTime(result.DateOut));
                $("#<%=txtProbableDepartureTime.ClientID %>").val(GetTimeFromDateTime(result.DateOut));
                return false;
            }

            function SetRelatedDataByReservationIdFailed(error) {
                toastr.error(error.get_message());
            }

            function PerformClearRelatedFields() {
                $("#<%=QSReservationId.ClientID %>").val(0);

                var ddlReservationId = '<%=ddlReservationId.ClientID%>'
                var chkIsFromReservation = '<%=chkIsFromReservation.ClientID%>'
                $('#divReservationGuest').hide();
                $('#' + chkIsFromReservation).prop("checked", false);
                return false;
            }

            function OnCountrySucceeded(result) {
                $("#ContentPlaceHolder1_txtGuestNationality").val(result);
                var country = $("#<%=ddlGuestCountry.ClientID %>").val();

                if (country == 19) {


                    for (var key in MandaoryFieldsList) {
                        var id = MandaoryFieldsList[key].FieldId;
                        if (!($(id).length)) {
                            id = "#ContentPlaceHolder1_" + id;
                        }
                        var tr = $(id).parent().parent();
                        $(tr).find("label").addClass("required-field");
                    }

                }
            }

            function OnCountryFailed() { }

            function PopulateGuestListForReservationDropDownChangeEvent(reservationId) {
                var idReservation = reservationId;
                //toastr.info(reservationId);
                $("#<%=ddlReservationGuest.ClientID%>").attr("disabled", "disabled");


                if (idReservation == 0) {


                    $('#<%=ddlReservationGuest.ClientID %>').empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
                else {


                    $('#<%=ddlReservationGuest.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                    $.ajax({
                        type: "POST",
                        url: "/HotelManagement/frmRoomRegistrationNew.aspx/PopulateGuest",
                        data: '{ReservationId: ' + idReservation + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: OnGuestPopulated,
                        failure: function (response) {
                            toastr.info(response.d);
                        }
                    });
                }
            }
            function PopulateGuestList() {
                if ($('#<%=QSReservationId.ClientID%>').val() != "") {
                    $('#<%=ddlReservationId.ClientID%>').val($('#<%=QSReservationId.ClientID%>').val());
                }

                var idReservation = -1;
                var strQSReservationIdValue = '<%=QSReservationIdValue %>';

                if (strQSReservationIdValue != "0") {
                    $('#<%=ddlReservationId.ClientID%>').val(strQSReservationIdValue);
                }

                var ReservationId = 0;
                if ($('#<%=ddlReservationId.ClientID%>').val() == "") {
                    ReservationId = $('#<%=ddlReservationId.ClientID%>').val().split('~')[0];
                }
                else {
                    ReservationId = $('#<%=QSReservationId.ClientID%>').val().split('~')[0];
                }

                var QSReservationId = ReservationId;

                if (QSReservationId != "") {
                    idReservation = parseInt(QSReservationId);
                }
                else {
                    idReservation = parseInt(ReservationId);
                }

                $("#<%=ddlReservationGuest.ClientID%>").attr("disabled", "disabled");

                if (idReservation == 0) {

                    $('#<%=ddlReservationGuest.ClientID %>').empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
                else {

                    $('#<%=ddlReservationGuest.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                    $.ajax({
                        type: "POST",
                        url: "/HotelManagement/frmRoomRegistrationNew.aspx/PopulateGuest",
                        data: '{ReservationId: ' + idReservation + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: OnGuestPopulated,
                        failure: function (response) {
                            toastr.info(response.d);
                        }
                    });
                }
            }
            function OnGuestPopulated(response) {
                var Data = response.d;
                var length = Data.length;
                if (length > 0) {
                    $('#divReservationGuest').show();
                    var ddlReservationGuest = '<%=ddlReservationGuest.ClientID%>'

                    $("#" + ddlReservationGuest).attr("disabled", false);
                    PopulateControl(response.d, $("#<%=ddlReservationGuest.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());

                    //toastr.info($('#<%=QSReservationId.ClientID%>').val());
                    var guestId = $('#<%=QSReservationId.ClientID%>').val().split('~')[3];

                    if (guestId)
                        $("#<%=ddlReservationGuest.ClientID %>").val(guestId).trigger('change');
                }
                else {
                    $('#divReservationGuest').hide();
                }
            }


            function PopulatePaidServiceFormReservation(qsReservationId) {
                var currencyType = $("#<%=ddlCurrency.ClientID %>").val();
                var convertionRate = $("#<%=txtConversionRate.ClientID %>").val();
            }

            $("#<%=txtGuestName.ClientID %>").blur(function () {
                if (GuestDetailsSearchValidationCheck() == true) {
                    GuestDetailsSearch();
                }
            });

            $("#txtGuestCountrySearch").blur(function () {
                if (GuestDetailsSearchValidationCheck() == true) {
                    GuestDetailsSearch();
                }
            });

            $("#<%=txtGuestDOB.ClientID %>").blur(function () {

                if (GuestDetailsSearchValidationCheck() == true) {
                    GuestDetailsSearch();
                }
            });


            $("#<%=txtGuestEmail.ClientID %>").blur(function () {
                if (GuestDetailsSearchValidationCheck() == true) {
                    GuestDetailsSearch();
                }
            });

            $("#<%=txtGuestPhone.ClientID %>").blur(function () {
                if (GuestDetailsSearchValidationCheck() == true) {
                    GuestDetailsSearch();
                }
            });

            $("#<%=txtNationalId.ClientID %>").blur(function () {
                if (GuestDetailsSearchValidationCheck() == true) {
                    GuestDetailsSearch();
                }
            });

            $("#<%=txtPassportNumber.ClientID %>").blur(function () {
                if (GuestDetailsSearchValidationCheck() == true) {
                    GuestDetailsSearch();
                }
            });

            SearchRateChart = $("#tblRateChart").DataTable({
                data: [],
                columns: [
                    { title: "Promotion / Package Name", "data": "PromotionName", sWidth: '30%' },
                    { title: "Company Name", "data": "CompanyName", sWidth: '30%' },
                    { title: "Effect From", "data": "EffectFrom", sWidth: '15%' },
                    { title: "Effect To", "data": "EffectTo", sWidth: '15%' },
                    { title: "Select", "data": null, sWidth: '10%' },
                ],
                columnDefs: [
                    {
                        "targets": 2,
                        "render": function (data, type, full, meta) {
                            return CommonHelper.DateFromStringToDisplay(data, innBoarDateFormat);
                        }
                    },
                    {
                        "targets": [3],
                        "render": function (data, type, full, meta) {
                            return CommonHelper.DateFromStringToDisplay(data, innBoarDateFormat);
                        }
                    }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    row += "&nbsp;<a href=\"javascript:void(0);\" onclick=\"return LoadPackagePriceNRoomType(" + aData.Id + ");\"> <img alt=\"Select\" src=\"../Images/select.png\" title='Select' /> </a>";

                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },
            });
        });

        function OnLoadXtraValidationSucceeded(result) {
            if (result.SetupValue == "1") {
                var country = $("#<%=ddlGuestCountry.ClientID %>").val();
                var txtGuestCountrySearch = $.trim($("#txtGuestCountrySearch").val());
                var enteredCountry = $.trim($("#<%=ddlGuestCountry.ClientID %>").find('option:selected').text());

                if (txtGuestCountrySearch == "") {
                    toastr.warning('Please Enter Valid Country Name.');
                    $("#txtGuestCountrySearch").focus();
                    return;
                }
                if (country == 19) {
                    var validationEmailAddress = _.findWhere(MandaoryFieldsList, { FieldId: "txtGuestEmail" });
                    if (validationEmailAddress != null) {
                        var emailAddress = $("#ContentPlaceHolder1_txtGuestEmail").val();
                        if (emailAddress == "") {
                            toastr.warning("Please Enter a Email Address");
                            $("#ContentPlaceHolder1_txtGuestEmail").focus();
                            return false;
                        }
                    }
                    var txtGuestEmail = $("#ContentPlaceHolder1_txtGuestEmail").val();
                    if (txtGuestEmail != "") {
                        var mailformat = new RegExp('^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$', 'i');
                        if (txtGuestEmail.match(mailformat)) {
                        }
                        else {
                            toastr.warning("You have entered an invalid email address!");
                            document.getElementById("txtGuestEmail>").focus();
                            return;
                        }
                    }

                    var validationPassportNumber = _.findWhere(MandaoryFieldsList, { FieldId: "txtPassportNumber" });
                    if (validationPassportNumber != null) {
                        var PassportNumberValue = $("#ContentPlaceHolder1_txtPassportNumber").val();
                        if (PassportNumberValue == "") {
                            toastr.warning("Please Enter a Passport Number");
                            $("#ContentPlaceHolder1_txtPassportNumber").focus();
                            return false;
                        }
                    }

                    var validationGuestDrivinlgLicense = _.findWhere(MandaoryFieldsList, { FieldId: "txtGuestDrivinlgLicense" });
                    if (validationGuestDrivinlgLicense != null) {
                        var GuestDrivinlgLicenseValue = $("#ContentPlaceHolder1_txtGuestDrivinlgLicense").val();
                        if (GuestDrivinlgLicenseValue == "") {
                            toastr.warning("Please Enter a Drivinlg License Number");
                            $("#ContentPlaceHolder1_txtGuestDrivinlgLicense").focus();
                            return false;
                        }
                    }

                    var validationNationalId = _.findWhere(MandaoryFieldsList, { FieldId: "txtNationalId" });
                    if (validationNationalId != null) {
                        var NationalIdValue = $("#ContentPlaceHolder1_txtNationalId").val();
                        if (NationalIdValue == "") {
                            toastr.warning("Please Enter a National Id");
                            $("#ContentPlaceHolder1_txtNationalId").focus();
                            return false;
                        }
                    }
                }
                else { //if country is not Bangladesh
                    var visaNumber = $("#<%=txtVisaNumber.ClientID %>").val();
                    var visaIssueDate = $("#<%=txtVIssueDate.ClientID %>").val();
                    var visaExpiryDate = $("#<%=txtVExpireDate.ClientID %>").val();
                    var passportNumber = $("#<%=txtPassportNumber.ClientID %>").val();
                    var passIssuePlace = '';
                    var passIssueDate = $("#<%=txtPIssueDate.ClientID %>").val();
                    var passExpiryDate = $("#<%=txtPExpireDate.ClientID %>").val();
                    var email = $("#<%=txtGuestEmail.ClientID %>").val();

                    if (visaNumber == "") {
                        toastr.warning("Please Provide Visa Number.");
                        $("#ContentPlaceHolder1_txtVisaNumber").focus();
                        return;
                    }
                    else if (visaIssueDate == "") {
                        toastr.warning("Please Provide Visa Issue Date.");
                        $("#ContentPlaceHolder1_txtVIssueDate").focus();
                        return;
                    }
                    else if (visaExpiryDate == "") {
                        toastr.warning("Please Provide Visa Expiry Date.");
                        $("#ContentPlaceHolder1_txtVExpireDate").focus();
                        return;
                    }
                    else if (passportNumber == "") {
                        toastr.warning("Please Provide Passport Number.");
                        $("#ContentPlaceHolder1_txtPassportNumber").focus();
                        return;
                    }
                    else if (passIssueDate == "") {
                        toastr.warning("Please Provide Passport Issue Date.");
                        $("#ContentPlaceHolder1_txtPIssueDate").focus();
                        return;
                    }
                    else if (passExpiryDate == "") {
                        toastr.warning("Please Provide Passport Expiry Date.");
                        $("#ContentPlaceHolder1_txtPExpireDate").focus();
                        return;
                    }
                    else if (email == "") {
                        toastr.warning("Please Provide Email Address.");
                        $("#<%=txtGuestEmail.ClientID %>").focus();
                        return;
                    }
                }

                var dob = $("#<%=txtGuestDOB.ClientID %>").val();
                var address = $("#<%=txtGuestAddress2.ClientID %>").val();

                var phoneNo = $("#<%=txtGuestPhone.ClientID %>").val();


                if (dob == "") {
                    toastr.warning("Please Provide Date of Birth.");
                    $("#<%=txtGuestDOB.ClientID %>").focus();
                    return;
                }
                else if (address == "") {
                    toastr.warning("Please Provide Address.");
                    $("#<%=txtGuestAddress2.ClientID %>").focus();
                    return;
                }
                else if (phoneNo == "") {
                    toastr.warning("Please Provide Phone No.");
                    $("#<%=txtGuestPhone.ClientID %>").focus();
                    return;
                }
                //if (isBlock == true) {
                //     toastr.warning('This Guest is Blocked, Cannot be registered ');
                //     return;
                //}
            }
            LoadDetailGridInformation();
        }
        function OnLoadXtraValidationFailed(error) {
        }

        function CurrencyRateInfoEnable() {
            if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                $("#<%=txtConversionRate.ClientID %>").val('');
                $('#CurrencyAmountInformationDiv').hide()
            }

            else {
                $('#CurrencyAmountInformationDiv').show()
            }
            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            if (ddlCurrency == 0) {
                $('#CurrencyAmountInformationDiv').hide()
            }
        }

        function OnGetPaidServiceReservationSucceed(result) {
            if (result.RegistrationPaidService.length > 0) {
                $("#<%=hfPaidServiceSaveObj.ClientID %>").val(JSON.stringify(result.RegistrationPaidService));
            }
        }
        function OnGetPaidReservationServiceFailed(error) {

        }

        function LoadReservationGuestInformationForPopUp(reservationId, reservationDetailId, roomId) {
            window.location.href = '/HotelManagement/frmRoomRegistrationNew.aspx?SelectedRoomNumber=' + roomId + '&source=Reservation'
            return false;
        }

        //Complementary Item
        function LoadComplementaryItemByWM(reservationId) {
            PageMethods.LoadComplementaryItemByWM(reservationId, LoadComplementaryItemByWMSucceeded, LoadComplementaryItemByWMFailed);
            return false;
        }
        function LoadComplementaryItemByWMSucceeded(result) {
            var Item = "";
            $.each(result, function () {
                SetChecked(this['Text']);
                Item = Item + this['Text'];
            });
        }

        function SetChecked(id) {
            $('#ContentPlaceHolder1_chkComplementaryItem :checkbox').each(function () {
                var chkBoxId = $(this).attr('id');
                var chkBoxValue = $(this).val();
                if (id == chkBoxValue) {
                    $('#' + chkBoxId).attr("checked", true);
                }
            });
        }

        function LoadComplementaryItemByWMFailed(error) {
            toastr.error(error.get_message());
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewReservation').hide();
            $('#EntryPanel').show();
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewReservation').show();
            $('#EntryPanel').hide();
            PerformClearAction();
            return false;
        }

        function LoadTodaysReservation() {
            PopulateReservation(0);
        }

        function ToggleFieldVisible() {
            var ctrl = '#<%=chkIsFromReservation.ClientID%>'
            var targetControl = $('#<%= ddlReservationId.ClientID %>');
            if ($(ctrl).is(':checked')) {
                targetControl.attr("disabled", false);
                var ddlReservationId = '<%=ddlReservationId.ClientID%>'

                if ($("#<%=QSReservationId.ClientID %>").val() != "") {
                    if ($("#<%=QSReservationId.ClientID %>").val().split('~')[0] != 0) {
                        $('#divReservationGuest').show();
                    }
                }

                $("#<%=imgReservationSearch.ClientID %>").attr("disabled", false);
                $("#txtReservationId").attr("disabled", false);
            }
            else {
                var ddlReservationId = '<%=ddlReservationId.ClientID%>'
                $("#" + ddlReservationId).val('0~0~0');
                targetControl.attr("disabled", true);
                $('#divReservationGuest').hide();


                if ($("#<%=HiddenCompanyId.ClientID %>").val() == "") {
                    $("#<%=chkIsLitedCompany.ClientID %>").attr("checked", false);
                    $("#<%=ddlCompanyName.ClientID %>").val(0);

                    $('#ListedCompany').hide();
                    $('#ReservedCompany').show();
                    $('#PaymentInformation').hide();
                }
                else {
                    $('#ReservedCompany').hide();
                    $('#ListedCompany').show();
                    $('#PaymentInformation').show();
                    GetTotalCostWithCompanyOrPersonalDiscountWithOutMessege();
                }

                $("#<%=imgReservationSearch.ClientID %>").attr("disabled", true);
                $("#txtReservationId").attr("disabled", true);
            }
        }

        function ToggleFieldVisibleForListedCompany(ctrl) {
            if ($(ctrl).is(':checked')) {
                $('#ReservedCompany').hide();
                $('#ListedCompany').show();
                $('#PaymentInformation').show();
            }
            else {
                $("#<%=txtContactNumber.ClientID %>").val('')
                $("#<%=txtContactPerson.ClientID %>").val('')
                $('#ListedCompany').hide();
                $('#ReservedCompany').show();
                $('#PaymentInformation').hide();
            }
        }
        function ToggleFieldVisibleForListedContact(ctrl) {
            if ($(ctrl).is(':checked')) {
                $('#ReservedContact').hide();
                $('#ListedContact').show();
                //$('#PaymentInformation').show();
            }
            else {
                $("#<%=txtContactNumber.ClientID %>").val('')
                $("#<%=txtContactPerson.ClientID %>").val('')
                $('#ListedContact').hide();
                $('#ReservedContact').show();
                //$('#PaymentInformation').hide();
            }
        }
        function ToggleAdvancePaymentFieldVisible(ctrl) {
            if ($(ctrl).is(':checked')) {
                EntryPanelVisibleTrue();
            }
            else {
                EntryPanelVisibleFalse();
            }
        }

        function ToggleFieldVisibleForAllActiveReservation(ctrl) {
            PopulateReservation(0);
        }

        function PopulateReservation(IsAllActiveReservation) {
            $.ajax({
                type: "POST",
                url: "/HotelManagement/frmRoomRegistrationNew.aspx/PopulateReservationDropDown",
                data: '{IsAllActiveReservation: ' + IsAllActiveReservation + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnPopulateReservationPopulated,
                failure: function (response) {
                    toastr.info(response.d);
                }
            });
        }

        function GetTotalCostWithCompanyOrPersonalDiscountWithOutMessege() {
            var promId = $("#<%=ddlBusinessPromotionId.ClientID %>").val();
            var companyId = $("#<%=ddlCompanyName.ClientID %>").val();
            var isCheked = false;
            if ($("#<%=chkIsLitedCompany.ClientID %>").is(":checked") == true) { isCheked = true; }

            else { isCheked = false; }
            if (isCheked == false && companyId <= 0) { companyId = 0; }
            if (promId <= 0) { promId = 0; }
            if (companyId != 0 || promId != 0) {
                PageMethods.GetCalculatedDiscount(companyId, promId, GetCalculatedDiscountWithOutMessegeObjectSucceeded, GetCalculatedDiscountObjectFailed);
                return false;
            }
            else {
                return true;
            }
        }

        function GetCalculatedDiscountWithOutMessegeObjectSucceeded(result) {
            $("#<%=txtContactNumber.ClientID %>").val(result.ContactNumber)
            $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson)
            var company = $("#<%=ddlCompanyName.ClientID %>").val();
            var btnSaveValue = $("#<%=btnSave.ClientID %>").val();
            return false;
        }
        function GetCalculatedDiscountObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function GetTotalCostWithCompanyOrPersonalDiscount() {
            var promId = $("#<%=ddlBusinessPromotionId.ClientID %>").val();
            var companyId = $("#<%=ddlCompanyName.ClientID %>").val();
            var isCheked = false;
            if ($("#<%=chkIsLitedCompany.ClientID %>").is(":checked") == true) { isCheked = true; }
            else { isCheked = false; }
            if (isCheked == false && companyId <= 0) { companyId = 0; }
            if (promId <= 0) { promId = 0; }
            if (companyId != 0 || promId != 0) {
                PageMethods.GetCalculatedDiscount(companyId, promId, GetCalculatedDiscountObjectSucceeded, GetCalculatedDiscountObjectFailed);
                return false;
            }
            else {
                return true;
            }
        }
        var vdf = [];
        function GetCalculatedDiscountObjectSucceeded(result) {

            vdf = result;
            var company = $("#<%=ddlCompanyName.ClientID %>").val();
            var btnSaveValue = $("#<%=btnSave.ClientID %>").val();
            $("#<%=txtContactNumber.ClientID %>").val(result.ContactNumberWithoutLabel);

            var answer = confirm("Do you want to recalculate Room Rent ?")
            if (answer) {
                if (company > 0) {
                    $("#<%=ddlDiscountType.ClientID %>").val('Percentage');
                }

                $("#<%=txtDiscountAmount.ClientID %>").val(result.DiscountPercent);
                UpdateTotalCostWithDiscount();
            }
            return false;
        }
        function GetCalculatedDiscountObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function UpdateTotalCostWithDiscount() {
            var txtDiscountAmount = $('#<%=txtDiscountAmount.ClientID%>').val();
            var txtUnitPrice = '<%=txtUnitPrice.ClientID%>'
            var txtRoomRate = '<%=txtRoomRate.ClientID%>'
            var ddlDiscountType = '<%=ddlDiscountType.ClientID%>'

            var discountType = $('#' + ddlDiscountType).val();
            var unitPrice = 0.0, roomRate = 0.0;

            if (txtDiscountAmount == "") {
                $('#<%=txtDiscountAmount.ClientID%>').val(txtDiscountAmount);
                return;
            }


            if ($('#' + txtUnitPrice).val() != "")
                unitPrice = parseFloat($('#' + txtUnitPrice).val());

            if ($('#' + txtRoomRate).val() != "")
                roomRate = parseFloat($('#' + txtRoomRate).val());

            if (txtDiscountAmount == "") {
                txtDiscountAmount = 0;
            }

            var totalPaidServiceAmount = CalculatePaidServiceTotal();
            unitPrice += totalPaidServiceAmount;

            var discount = 0.00;
            discount = parseFloat(txtDiscountAmount);

            if (discountType == "Fixed") {
                unitPrice -= discount;
            }
            else {

                discount = ((unitPrice * discount) / 100);
                unitPrice -= discount;
            }

            $('#' + txtRoomRate).val(toFixed(unitPrice, 2));
        }

        function CalculateDiscount() {

            var txtUnitPrice = '<%=txtUnitPrice.ClientID%>'
            var txtRoomRate = '<%=txtRoomRate.ClientID%>'
            var ddlDiscountType = '<%=ddlDiscountType.ClientID%>'

            var discountType = $('#' + ddlDiscountType).val();
            var unitPrice = 0.0, roomRate = 0.0;

            if ($('#' + txtUnitPrice).val() != "")
                unitPrice = parseFloat($('#' + txtUnitPrice).val());

            if ($('#' + txtRoomRate).val() != "")
                roomRate = parseFloat($('#' + txtRoomRate).val());
            var isPackage = $("#ContentPlaceHolder1_hfPackageId").val() != "0";
            var isCheckMinimumRoomRate = $("#<%=hfIsMinimumRoomRateCheckingEnable.ClientID %>").val() == "1";

            if (!isPackage && isCheckMinimumRoomRate) {
                var minimumRoomRate = parseFloat($("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val());

                if (toFixed(roomRate, 2) < toFixed(minimumRoomRate, 2)) {

                    toastr.warning(`Minimum Room Rate For ${$("#<%=ddlRoomType.ClientID %> :selected").text()} : ${minimumRoomRate}`);
                    $('#' + txtRoomRate).val(minimumRoomRate).focus();
                    if ($('#' + txtRoomRate).val() != "")
                        roomRate = parseFloat($('#' + txtRoomRate).val());
                }
            }

            var totalPaidServiceAmount = CalculatePaidServiceTotal();
            unitPrice += totalPaidServiceAmount;

            var discount = 0.0;

            if (discountType == "Fixed") {
                discount = unitPrice - roomRate;
            }
            else {
                discount = unitPrice - roomRate;
                discount = toFixed(((discount / unitPrice) * 100), 2);
            }

            $('#ContentPlaceHolder1_txtDiscountAmount').val(toFixed(discount, 2));
        }

        function OnPopulateReservationPopulated(response) {
            PopulateControlWithOutDefaultOnPage(response.d, $("#<%=ddlReservationId.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
            ToggleFieldVisible();
        }

        //documents upload

        function UploadComplete() {
            //var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            debugger;
            var randomId = +$("#<%=RandomOwnerId.ClientID %>").val();
            //var id = +$("#ContentPlaceHolder1_hfRoomRegistrationId").val();
            var id = +$("#<%=EditId.ClientID %>").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.LoadDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {

            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='DocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th><th align='left' scope='col'>Action</th> </tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%; cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";

            // docc = guestDocumentTable;

            $("#GuestDocumentInfo").html(guestDocumentTable);
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function AttachFile() {

            //var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var randomId = +$("#<%=RandomOwnerId.ClientID %>").val();
            var path = "/HotelManagement/Image/";
            var category = "Guest";
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


            //$("#documentsDiv").dialog({
            //    autoOpen: true,
            //    modal: true,
            //    width: 700,
            //    closeOnEscape: true,
            //    resizable: false,
            //    title: "Add Documents",
            //    show: 'slide'
            //});
        }
        function ShowDocument(path, name) {
            $("#fileIframe").contents().find("body").html("");
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
        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }
        function DeleteGuestDoc(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                var deletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();

                if (deletedDoc != "")
                    deletedDoc += "," + docId;
                else
                    deletedDoc = docId;

                $("#<%=hfGuestDeletedDoc.ClientID %>").val(deletedDoc);

                $("#trdoc" + rowIndex).remove();
            }
        }

        //

        function PopulateControlWithOutDefaultOnPage(list, control, defaultSelectedValue) {
            if (list.length > 0) {
                control.empty().append('<option selected="selected" value="0">' + defaultSelectedValue + '</option>');
                $.each(list, function () {
                    control.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
            }
            else {
                control.empty().append('<option selected="selected" value="0">Not available</option>');
            }

            var qsReservationId = $("#<%=QSReservationId.ClientID %>").val().split('~')[0];
            if (qsReservationId > 0 || qsReservationId != "") {
                var ddlReservationId = '<%=ddlReservationId.ClientID%>'
                $('#' + ddlReservationId).val($("#<%=QSReservationId.ClientID %>").val());
            }

            CommonHelper.SpinnerClose();
        }

        //Aireport Pickup Div Visible True/False-------------------
        function AireportPickupInformationPanelVisibleTrue() {
            $('#AireportArrivalInformation').show("slow");
        }
        function AireportPickupInformationPanelVisibleFalse() {
            $('#AireportArrivalInformation').hide("slow");
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            return false;
        }
        function EntryPanelVisibleFalse() {
            return false;
        }

        function PerformFillFormAction(actionId) {
            PageMethods.FillFormByRoom(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            var txtEntiteledRoomType = '<%=txtEntiteledRoomType.ClientID%>'
            var hiddenRoomType = $('#' + txtEntiteledRoomType).val();
            $("#<%=txtViewRoomType.ClientID %>").val(result.RoomType);
            $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRate);
            $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRate);
            $("#<%=ddlEntitleRoomType.ClientID %>").val(result.RoomTypeId);
            $("#<%=txtRoomRate.ClientID %>").val(toFixed(result.RoomRate, 2));
            return true;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadGridViewByWebMethod(actionId) {
            PageMethods.LoadGrid(actionId, LoadGridSucceeded, LoadGridFailed);
            return false;
        }
        function LoadGridSucceeded(result) {
            $("#ltlTableWiseItemInformation").html(result);
            return false;
        }
        function LoadGridFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadReservationGuests(reservationId) {
            PageMethods.SearchNLoadReservationInfo(reservationId, null, null, null, null, null, OnLoadReservationGuestsSucceeded, OnLoadReservationGuestsFailed);
            return false;
        }
        function OnLoadReservationGuestsSucceeded(result) {
            $("#ltlTableWiseItemInformation").html(result);
            return false;
        }
        function OnLoadReservationGuestsFailed(error) {
            toastr.error(error.get_message());
        }

        //Integrated Gl Visible True/False-------------------
        function IntegratedGeneralLedgerDivPanelShow() {
            $('#IntegratedGeneralLedgerDiv').show();
        }
        function IntegratedGeneralLedgerDivPanelHide() {
            $('#IntegratedGeneralLedgerDiv').hide();
        }

        //Integrated Gl Visible True/False-------------------
        function IsRoomAvailableForRegistrationShow() {
            $('#IntegratedGeneralLedgerDiv').show();
            var hfIsRoomOverbookingEnableVal = $("#<%=hfIsRoomOverbookingEnable.ClientID %>").val();

            if (hfIsRoomOverbookingEnableVal == "1") {
                var r = confirm("This type of room is not available, if you continue then it will be a Overbooking. Do You Want to Continue?");
                if (r == true) {

                } else {
                    window.location = "frmRoomStatusInfo.aspx"
                }
            }
            else {
                var ra = confirm("This type of room is not available.");
                if (ra == true) {
                    window.location = "frmRoomStatusInfo.aspx"
                } else {
                    window.location = "frmRoomStatusInfo.aspx"
                }
            }
        }

        function CalcualteServiceTotalAfterComplementaryNo() {
            var totalPaidServicePrice = CalculatePaidServiceTotal();
            var roomRate = parseFloat($('#ContentPlaceHolder1_txtRoomRate').val());
            roomRate += totalPaidServicePrice;
            $('#ContentPlaceHolder1_txtRoomRate').val(toFixed(roomRate, 2));
            UpdateTotalCostWithDiscount();
            TotalRoomRateVatServiceChargeCalculation();
        }


        function CalculateRoomRateInclusively() {

            if ($.trim($("#ContentPlaceHolder1_txtTotalRoomRate").val()) == "0") {
                toastr.warning("Room Rate 0 Is Not Acceptable.");
                return false;
            }

            CommonHelper.ApplyDecimalValidation();

            $("#ContentPlaceHolder1_cbCalculateServiceCharge").prop("checked", $("#ContentPlaceHolder1_cbServiceCharge").is(":checked"));
            $("#ContentPlaceHolder1_cbCalculateCityCharge").prop("checked", $("#ContentPlaceHolder1_cbCityCharge").is(":checked"));
            $("#ContentPlaceHolder1_cbCalculateVatCharge").prop("checked", $("#ContentPlaceHolder1_cbVatAmount").is(":checked"));
            $("#ContentPlaceHolder1_cbCalculateAdditionalCharge").prop("checked", $("#ContentPlaceHolder1_cbAdditionalCharge").is(":checked"));

            if (TotalRoomRateGlobalValue == 0) {
                TotalRoomRateGlobalValue = parseFloat($("#ContentPlaceHolder1_txtTotalRoomRate").val());
            }

            $("#CalculateRackRateInclusivelyDialog").dialog({
                width: 935,
                height: 250,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Calculate Room Rate Inclusively",
                show: 'slide',
                open: function (event, ui) {
                    $('#CalculateRackRateInclusivelyDialog').css('overflow', 'hidden');
                }
            });
        }

        function CalculateRateInclusively() {

            if ($.trim($("#ContentPlaceHolder1_txtCalculatedTotalRoomRate").val()) == "0") {
                toastr.warning("0 is not acceptable.");
                return false;
            }

            var txtUnitPrice = '<%=txtCalculatedTotalRoomRate.ClientID%>'
            var txtRoomRate = '<%=txtRoomRate.ClientID%>'
            var cbServiceCharge = '<%=cbCalculateServiceCharge.ClientID%>'
            var cbCityCharge = '<%=cbCalculateCityCharge.ClientID%>'
            var cbVatAmount = '<%=cbCalculateVatCharge.ClientID%>'
            var cbAdditionalCharge = '<%=cbCalculateAdditionalCharge.ClientID%>'

            var inclusiveBill = 1, Vat = 0.00, ServiceCharge = 0.00, cityCharge = 0.00, additionalCharge = 0.00;
            var additionalChargeType = "Fixed", isRatePlusPlus = 1, isVatEnableOnGuestHouseCityCharge = 0, isCitySDChargeEnableOnServiceCharge = 0;
            var cbVatAmountVal = 1, cbServiceChargeVal = 1, cbCityChargeVal = 1, cbAdditionalChargeVal = 1;
            var isDiscountApplicableOnRackRate = 0;

            if ($("#ContentPlaceHolder1_hfIsRatePlusPlus").val() != "") { isRatePlusPlus = parseInt($("#ContentPlaceHolder1_hfIsRatePlusPlus").val(), 10); }

            if ($("#<%=hfGuestHouseVat.ClientID %>").val() != "")
                Vat = parseFloat($("#<%=hfGuestHouseVat.ClientID %>").val());

            if ($("#<%=hfGuestHouseServiceCharge.ClientID %>").val() != "")
                ServiceCharge = parseFloat($("#<%=hfGuestHouseServiceCharge.ClientID %>").val());

            if ($("#<%=hfCityCharge.ClientID %>").val() != "")
                cityCharge = parseFloat($("#<%=hfCityCharge.ClientID %>").val());

            if ($("#<%=hfAdditionalCharge.ClientID %>").val() != "")
                additionalCharge = parseFloat($("#<%=hfAdditionalCharge.ClientID %>").val());


            if ($("#<%=hfAdditionalChargeType.ClientID %>").val() != "")
                additionalChargeType = $("#<%=hfAdditionalChargeType.ClientID %>").val();

            if ($("#<%=hfIsVatEnableOnGuestHouseCityCharge.ClientID %>").val() != "")
                isVatEnableOnGuestHouseCityCharge = parseInt($("#<%=hfIsVatEnableOnGuestHouseCityCharge.ClientID %>").val(), 10);

            if ($("#<%=hfIsCitySDChargeEnableOnServiceCharge.ClientID %>").val() != "")
                isCitySDChargeEnableOnServiceCharge = parseInt($("#<%=hfIsCitySDChargeEnableOnServiceCharge.ClientID %>").val(), 10);

            if ($('#' + cbServiceCharge).is(':checked')) {
                cbServiceChargeVal = 1;
            }
            else {
                cbServiceChargeVal = 0;
                ServiceCharge = 0.00;
            }

            if ($('#' + cbCityCharge).is(':checked')) {
                cbCityChargeVal = 1;
            }
            else {
                cbCityChargeVal = 0;
                cityCharge = 0.00;
            }

            if ($('#' + cbVatAmount).is(':checked')) {
                cbVatAmountVal = 1;
            }
            else {
                cbVatAmountVal = 0;
                Vat = 0.00;
            }

            if ($('#' + cbAdditionalCharge).is(':checked')) {
                cbAdditionalChargeVal = 1;
            }
            else {
                cbAdditionalChargeVal = 0;
                additionalCharge = 0.00;
                additionalChargeType = "Percentage";
            }

            var txtRoomRateVal = parseFloat($('#' + txtUnitPrice).val());

            <%--if ($("#<%=hfInclusiveHotelManagementBill.ClientID %>").val() != "") {
                inclusiveBill = parseInt($("#<%=hfInclusiveHotelManagementBill.ClientID %>").val(), 10);
            }--%>

            if ($("#<%=hfIsDiscountApplicableOnRackRate.ClientID %>").val() != "") {
                isDiscountApplicableOnRackRate = parseInt($("#<%=hfIsDiscountApplicableOnRackRate.ClientID %>").val(), 10);
            }

            var unitPrice = parseFloat($("#ContentPlaceHolder1_txtUnitPrice").val());
            var discountType = "";
            var discountAmount = 0;

            //if (isDiscountApplicableOnRackRate == 1) {                
            //}
            //else {
            //    discountType = "Fixed";
            //    discountAmount = 0.00;
            //}

            var RoomRateGlobal = CommonHelper.GetRackRateServiceChargeVatInformation(unitPrice, ServiceCharge, cityCharge,
                Vat, additionalCharge, additionalChargeType, 0, isRatePlusPlus, isVatEnableOnGuestHouseCityCharge, isCitySDChargeEnableOnServiceCharge,
                parseInt(cbVatAmountVal, 10), parseInt(cbServiceChargeVal, 10), parseInt(cbCityChargeVal, 10),
                parseInt(cbAdditionalChargeVal, 10), isDiscountApplicableOnRackRate, 'Fixed', 0.00);

            txtRoomRateVal = RoomRateGlobal.CalculatedAmount;  //parseFloat($('#' + txtUnitPrice).val());
            discountType = "Fixed";
            discountAmount = txtRoomRateVal - parseFloat($('#' + txtUnitPrice).val());

            var RoomRate = CommonHelper.GetRackRateServiceChargeVatInformation(txtRoomRateVal, ServiceCharge, cityCharge,
                Vat, additionalCharge, additionalChargeType, inclusiveBill, isRatePlusPlus, isVatEnableOnGuestHouseCityCharge, isCitySDChargeEnableOnServiceCharge,
                parseInt(cbVatAmountVal, 10), parseInt(cbServiceChargeVal, 10), parseInt(cbCityChargeVal, 10),
                parseInt(cbAdditionalChargeVal, 10), isDiscountApplicableOnRackRate, discountType, discountAmount);

            if (RoomRate.RackRate > 0) {
                $("#ContentPlaceHolder1_txtCalculateServiceCharge").val(toFixed(RoomRate.ServiceCharge, 2));
                $("#ContentPlaceHolder1_txtCalculateVatCharge").val(toFixed(RoomRate.VatAmount, 2));
                $("#ContentPlaceHolder1_txtCalculateCityCharge").val(toFixed(RoomRate.SDCityCharge, 2));
                $("#ContentPlaceHolder1_txtCalculateAdditionalCharge").val(toFixed(RoomRate.AdditionalCharge, 2));
                $("#ContentPlaceHolder1_txtCalculateRackRate").val(RoomRate.RackRate);
                $("#ContentPlaceHolder1_txtCalculateDiscountAmount").val(RoomRate.DiscountAmount);
            }
            else {
                $("#ContentPlaceHolder1_txtCalculateServiceCharge").val('0');
                $("#ContentPlaceHolder1_txtCalculateVatCharge").val('0');
                $("#ContentPlaceHolder1_txtCalculateCityCharge").val('0');
                $("#ContentPlaceHolder1_txtCalculateAdditionalCharge").val('0');
                $("#ContentPlaceHolder1_txtCalculateDiscountAmount").val('0');
            }
        }

        function ApplyCalculateCharges() {

            if ($.trim($("#ContentPlaceHolder1_txtCalculatedTotalRoomRate").val()) == "" || $.trim($("#ContentPlaceHolder1_txtCalculatedTotalRoomRate").val()) == "0") {
                toastr.warning("0 is not acceptable.");
                return false;
            }

            var originalDiscount = toFixed((parseFloat($("#ContentPlaceHolder1_txtUnitPrice").val()) - parseFloat($("#ContentPlaceHolder1_txtCalculateRackRate").val())), 2);

            $("#ContentPlaceHolder1_txtServiceCharge").val($("#ContentPlaceHolder1_txtCalculateServiceCharge").val());
            $("#ContentPlaceHolder1_txtCityCharge").val($("#ContentPlaceHolder1_txtCalculateCityCharge").val());
            $("#ContentPlaceHolder1_txtVatAmount").val($("#ContentPlaceHolder1_txtCalculateVatCharge").val());
            $("#ContentPlaceHolder1_txtAdditionalCharge").val($("#ContentPlaceHolder1_txtCalculateAdditionalCharge").val());

            $("#ContentPlaceHolder1_txtDiscountAmount").val(originalDiscount);
            $("#ContentPlaceHolder1_ddlDiscountType").val("Fixed");
            
            var CalculatedTotalRoomRate = $("#ContentPlaceHolder1_txtCalculatedTotalRoomRate").val();
            //$("#ContentPlaceHolder1_txtTotalRoomRate").val(Math.round(CalculatedTotalRoomRate).toFixed(2));

            $("#ContentPlaceHolder1_txtTotalRoomRate").val(CalculatedTotalRoomRate);

            $("#ContentPlaceHolder1_txtRoomRate").val($("#ContentPlaceHolder1_txtCalculateRackRate").val());
            $("#CalculateRackRateInclusivelyDialog").dialog("close");

            $("#ContentPlaceHolder1_cbServiceCharge").prop("checked", $("#ContentPlaceHolder1_cbCalculateServiceCharge").is(":checked"));
            $("#ContentPlaceHolder1_cbCityCharge").prop("checked", $("#ContentPlaceHolder1_cbCalculateCityCharge").is(":checked"));
            $("#ContentPlaceHolder1_cbVatAmount").prop("checked", $("#ContentPlaceHolder1_cbCalculateVatCharge").is(":checked"));
            $("#ContentPlaceHolder1_cbAdditionalCharge").prop("checked", $("#ContentPlaceHolder1_cbCalculateAdditionalCharge").is(":checked"));

            ClearRRC();
            //CalculateDiscount();
        }

        function ClearRRC() {
            $("#ContentPlaceHolder1_txtCalculatedTotalRoomRate").val("");
            $("#ContentPlaceHolder1_txtCalculateServiceCharge").val("");
            $("#ContentPlaceHolder1_txtCalculateRackRate").val("");
            $("#ContentPlaceHolder1_txtCalculateCityCharge").val("");
            $("#ContentPlaceHolder1_txtCalculateVatCharge").val("");
            $("#ContentPlaceHolder1_txtCalculateDiscountAmount").val("");
        }

    </script>
    <!-- Pop Up Script -->
    <script type="text/javascript">
        $(document).ready(function () {
            $("#PopSearchPanel").hide();
            $("#PopTabPanel").hide();
            $("#ExtraSearch").hide();
            $("#btnPopSearch").click(function () {
                $("#PopSearchPanel").show('slow');
                $("#PopTabPanel").hide('slow');
                //LoadGridInformation();
                GridPagingForGuestSearch(1, 1);
            });

            $("#btnReservationSearch").click(function () {
                PopulateReservation(0);
                //LoadReservationInformation();
                GridPaging(1, 1);
            });

            $('#imgCollapse').click(function () {
                var imageSrc = $('#imgCollapse').attr("src");
                if (imageSrc == '/HotelManagement/Image/expand_alt.png') {
                    $("#ExtraSearch").show('slow');
                    $('#imgCollapse').attr("src", '/HotelManagement/Image/collapse_alt.png');
                }
                else {
                    $("#ExtraSearch").hide('slow');
                    $('#imgCollapse').attr("src", '/HotelManagement/Image/expand_alt.png');
                }
            })

            $("#btnAddPerson").click(function () {
                $("#PopEntryPanel").show();
                AddNewItem();
            });
            $("#btnSearchSuccess").click(function () {
                $("#<%=txtGuestName.ClientID %>").val($("#<%=hiddenGuestName.ClientID %>").val());
                $("#PopSearchPanel").hide();
                $("#PopTabPanel").hide();
                $("#btnSearchSuccess").val("1");
                LoadDataOnParentForm();

                //PageMethods.LoadDocument(0, +($("#<%=hiddenGuestId.ClientID %>").val()), "", OnLoadDocumentSucceeded, OnLoadDocumentFailed);
                //UploadComplete();
                $("#TouchKeypad").dialog("close");
            });
            $("#btnSearchCancel").click(function () {
                $("#<%=hiddenGuestName.ClientID %>").val('');
                $("#<%=hiddenGuestId.ClientID %>").text('');
                $("#PopSearchPanel").hide();
                $("#PopTabPanel").hide();
                $("#TouchKeypad").dialog("close");
            });
            $("#btnPrintDocument").click(function () {
                var guestId = $("#<%=hiddenGuestId.ClientID %>").val();
                window.location.href = '/HotelManagement/Reports/frmReportPrintImage.aspx?GuestId='
                    + guestId + '&OwnerType=Guest';
            });

            $("#btnNext1").click(function () {
                document.getElementById("ContentPlaceHolder1_A").className = "";
                document.getElementById("ContentPlaceHolder1_A").className = "ui-state-default ui-corner-top";
                $('#tab-1').hide();

                document.getElementById("ContentPlaceHolder1_B").className = "";
                document.getElementById("ContentPlaceHolder1_B").className = "ui-state-default ui-corner-top";
                $('#tab-2').hide();

                document.getElementById("ContentPlaceHolder1_C").className = "";
                document.getElementById("ContentPlaceHolder1_C").className = "ui-state-default ui-corner-top ui-tabs-active ui-state-active";
                $('#tab-3').show();
                $(function () {
                    $('#ContentPlaceHolder1_C').trigger('click');
                });

                document.getElementById("ContentPlaceHolder1_D").className = "";
                document.getElementById("ContentPlaceHolder1_D").className = "ui-state-default ui-corner-top";
                $('#tab-4').hide();
            });
            $("#btnNext2").click(function () {
                document.getElementById("ContentPlaceHolder1_A").className = "";
                document.getElementById("ContentPlaceHolder1_A").className = "ui-state-default ui-corner-top";
                $('#tab-1').hide();

                document.getElementById("ContentPlaceHolder1_B").className = "";
                document.getElementById("ContentPlaceHolder1_B").className = "ui-state-default ui-corner-top";
                $('#tab-2').hide();

                document.getElementById("ContentPlaceHolder1_D").className = "";
                document.getElementById("ContentPlaceHolder1_D").className = "ui-state-default ui-corner-top ui-tabs-active ui-state-active";
                $('#tab-4').show();

                $(function () {
                    $('#ContentPlaceHolder1_D').trigger('click');
                });

                document.getElementById("ContentPlaceHolder1_C").className = "";
                document.getElementById("ContentPlaceHolder1_C").className = "ui-state-default ui-corner-top";
                $('#tab-3').hide();
            });
            $("#btnPrev1").click(function () {
                document.getElementById("ContentPlaceHolder1_C").className = "";
                document.getElementById("ContentPlaceHolder1_C").className = "ui-state-default ui-corner-top";
                $('#tab-3').hide();

                document.getElementById("ContentPlaceHolder1_B").className = "";
                document.getElementById("ContentPlaceHolder1_B").className = "ui-state-default ui-corner-top";
                $('#tab-2').hide();

                document.getElementById("ContentPlaceHolder1_A").className = "";
                document.getElementById("ContentPlaceHolder1_A").className = "ui-state-default ui-corner-top ui-tabs-active ui-state-active";
                $('#tab-1').show();
                $(function () {
                    $('#ContentPlaceHolder1_A').trigger('click');
                });

                document.getElementById("ContentPlaceHolder1_D").className = "";
                document.getElementById("ContentPlaceHolder1_D").className = "ui-state-default ui-corner-top";
                $('#tab-4').hide();
            });
            $("#btnPrev2").click(function () {
                document.getElementById("ContentPlaceHolder1_A").className = "";
                document.getElementById("ContentPlaceHolder1_A").className = "ui-state-default ui-corner-top";
                $('#tab-1').hide();

                document.getElementById("ContentPlaceHolder1_B").className = "";
                document.getElementById("ContentPlaceHolder1_B").className = "ui-state-default ui-corner-top";
                $('#tab-2').hide();

                document.getElementById("ContentPlaceHolder1_C").className = "";
                document.getElementById("ContentPlaceHolder1_C").className = "ui-state-default ui-corner-top ui-tabs-active ui-state-active";
                $('#tab-3').show();
                $(function () {
                    $('#ContentPlaceHolder1_C').trigger('click');
                });

                document.getElementById("ContentPlaceHolder1_D").className = "";
                document.getElementById("ContentPlaceHolder1_D").className = "ui-state-default ui-corner-top";
                $('#tab-4').hide();
            });

        });
        function image(thisImg) {
            var img = document.createElement("IMG");
            img.src = "images/" + thisImg;
            document.getElementById('imageDiv').appendChild(img);
        }
        function GridPagingForGuestSearch(pageNumber, IsCurrentOrPreviousPage) {
            var companyName = $("#<%=txtSrcCompanyName.ClientID %>").val();
            var DateOfBirth = $("#<%=txtSrcDateOfBirth.ClientID %>").val();
            var EmailAddress = $("#<%=txtSrcEmailAddress.ClientID %>").val();
            var FromDate = $("#<%=txtSrcFromDate.ClientID %>").val();
            var ToDate = $("#<%=txtSrcToDate.ClientID %>").val();
            var GuestName = $("#<%=txtSrcGuestName.ClientID %>").val();
            var MobileNumber = $("#<%=txtSrcMobileNumber.ClientID %>").val();
            var NationalId = $("#<%=txtSrcNationalId.ClientID %>").val();
            var PassportNumber = $("#<%=txtSrcPassportNumber.ClientID %>").val();
            var RegistrationNumber = $("#<%=txtSrcRegistrationNumber.ClientID %>").val();
            var RoomNumber = $("#<%=txtSrcRoomNumber.ClientID %>").val();
            var gridRecordsCount = $("#tblGuestInfo3 tbody tr").length;

            CommonHelper.SpinnerOpen();
            PageMethods.SearchGuestAndLoadGridInformationPaging(companyName, DateOfBirth, EmailAddress, FromDate, ToDate, GuestName, MobileNumber, NationalId, PassportNumber, RegistrationNumber, RoomNumber, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadGuestPagingSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadGuestPagingSucceeded(result) {
            CommonHelper.SpinnerClose();

            var gridRecordsCount = $("#tblGuestInfo3 tbody tr").length;
            if (gridRecordsCount < 0) {
                $("#tblGuestInfo3 tbody").html("");
            }
            else {
                $("#tblGuestInfo3 tbody tr").remove();
            }
            $("#GridPagingContainer3 ul").html("");
            var tr = "", totalRow = 0;
            var i = 0;
            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"5\" >No Data Found</td> </tr>";
                $("#tblGuestInfo3 tbody ").append(emptyTr);
                return false;
            }
            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tblGuestInfo3 tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                //strTable += "<tr style='background-color:White;'><td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.GuestName + "</td>";
                //strTable += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.CountryName + "</td>";
                //strTable += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.CountryName + "</td>";
                //strTable += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.GuestPhone + "</td>";
                //strTable += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.GuestEmail + "</td>";
                //strTable += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + dr.GuestId + ")'>" + dr.RoomNumber + "</td>";
                tr += "<td align='left' style='width: 25%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + gridObject.GuestId + ")'>" + gridObject.GuestName + "</td>";
                tr += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + gridObject.GuestId + ")'>" + gridObject.CountryName + "</td>";
                tr += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + gridObject.GuestId + ")'>" + gridObject.GuestPhone + "</td>";
                tr += "<td align='left' style='width: 20%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + gridObject.GuestId + ")'>" + gridObject.GuestEmail + "</td>";
                //tr += "<td align='left' style='width: 15%;cursor:pointer' onClick='javascript:return SelectGuestInformation(" + gridObject.GuestId + ")'>" + gridObject.RoomNumber + "</td>";
                tr += "</td></tr>";

                $("#tblGuestInfo3 tbody").append(tr);
                tr = "";
            });
            $("#GridPagingContainer3 ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer3 ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer3 ul").append(result.GridPageLinks.NextButton);

            CommonHelper.ApplyIntigerValidation();
            return false;
        }
        function OnLoadObjectFailed(error) {
            alert(error.get_message());
        }
        function LoadGridInformation() {
            var companyName = $("#<%=txtSrcCompanyName.ClientID %>").val();
            var DateOfBirth = $("#<%=txtSrcDateOfBirth.ClientID %>").val();
            var EmailAddress = $("#<%=txtSrcEmailAddress.ClientID %>").val();
            var FromDate = $("#<%=txtSrcFromDate.ClientID %>").val();
            var ToDate = $("#<%=txtSrcToDate.ClientID %>").val();
            var GuestName = $("#<%=txtSrcGuestName.ClientID %>").val();
            var MobileNumber = $("#<%=txtSrcMobileNumber.ClientID %>").val();
            var NationalId = $("#<%=txtSrcNationalId.ClientID %>").val();
            var PassportNumber = $("#<%=txtSrcPassportNumber.ClientID %>").val();
            var RegistrationNumber = $("#<%=txtSrcRegistrationNumber.ClientID %>").val();
            var RoomNumber = $("#<%=txtSrcRoomNumber.ClientID %>").val();

            PageMethods.SearchGuestAndLoadGridInformation(companyName, DateOfBirth, EmailAddress, FromDate, ToDate, GuestName, MobileNumber, NationalId, PassportNumber, RegistrationNumber, RoomNumber, OnLoadGridViewObjectSucceeded, OnLoadGridViewObjectFailed);
            return false;
        }
        function OnLoadGridViewObjectSucceeded(result) {
            $("#ltlTableSearchGuest").html(result);
            return false;
        }
        function OnLoadGridViewObjectFailed(error) {
            toastr.error(error.get_message());
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var guestName = $("#<%=txtResvGuestName.ClientID %>").val();
            var companyName = $("#<%=txtResvCompanyName.ClientID %>").val();
            var reservNumber = $("#<%=txtReservationNo.ClientID %>").val();
            var checkInDate = $("#<%=txtRsvCheckInDate.ClientID %>").val();
            var checkOutDate = $("#<%=txtCheckOutDate.ClientID %>").val();

            var gridRecordsCount = $("#tblGuestInfo tbody tr").length;
            CommonHelper.SpinnerOpen();
            PageMethods.SearchNLoadReservationInfoWithPagination(0, guestName, companyName, reservNumber, checkInDate, checkOutDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectPagingSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectPagingSucceeded(result) {
            CommonHelper.SpinnerClose();

            var gridRecordsCount = $("#tblGuestInfo tbody tr").length;
            if (gridRecordsCount < 0) {
                $("#tblGuestInfo tbody").html("");
            }
            else {
                $("#tblGuestInfo tbody tr").remove();
            }
            $("#GridPagingContainer2 ul").html("");
            var tr = "", totalRow = 0;
            var i = 0;
            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#tblGuestInfo tbody ").append(emptyTr);
                return false;
            }

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tblGuestInfo tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style='width: 10%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + gridObject.ReservationId + "," + gridObject.ReservationDetailId + "," + gridObject.RoomId + "," + 0 + ")'>" + gridObject.ReservationNumber + "</td>";
                tr += "<td align='left' style='width: 30%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + gridObject.ReservationId + "," + gridObject.ReservationDetailId + "," + gridObject.RoomId + "," + 0 + ")'>" + gridObject.GuestName + "</td>";
                tr += "<td align='left' style='width: 10%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + gridObject.ReservationId + "," + gridObject.ReservationDetailId + "," + gridObject.RoomId + "," + 0 + ")'>" + gridObject.RoomType + "</td>";
                tr += "<td align='left' style='width: 30%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + gridObject.ReservationId + "," + gridObject.ReservationDetailId + "," + gridObject.RoomId + "," + 0 + ")'>" + gridObject.RoomNumber + "</td>";
                tr += "<td align='left' style='width: 10%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + gridObject.ReservationId + "," + gridObject.ReservationDetailId + "," + gridObject.RoomId + "," + 0 + ")'>" + gridObject.CheckIn + "</td>";
                tr += "<td align='left' style='width: 10%;cursor:pointer' onClick='javascript:return LoadReservationGuestInformationForPopUp(" + gridObject.ReservationId + "," + gridObject.ReservationDetailId + "," + gridObject.RoomId + "," + 0 + ")'>" + gridObject.DateOut + "</td>";
                tr += "</td></tr>";

                $("#tblGuestInfo tbody").append(tr);
                tr = "";
            });
            $("#GridPagingContainer2 ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer2 ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer2 ul").append(result.GridPageLinks.NextButton);

            CommonHelper.ApplyIntigerValidation();
            return false;
        }
        function OnLoadObjectFailed(error) {
            alert(error.get_message());
        }
        function LoadReservationInformation() {
            var guestName = $("#<%=txtResvGuestName.ClientID %>").val();
            var companyName = $("#<%=txtResvCompanyName.ClientID %>").val();
            var reservNumber = $("#<%=txtReservationNo.ClientID %>").val();
            var checkInDate = $("#<%=txtRsvCheckInDate.ClientID %>").val();
            var checkOutDate = $("#<%=txtCheckOutDate.ClientID %>").val();

            PageMethods.SearchNLoadReservationInfo(0, guestName, companyName, reservNumber, checkInDate, checkOutDate, OnLoadReservInfoSucceeded, OnLoadReservInfoFailed);
            return false;
        }
        function OnLoadReservInfoSucceeded(result) {
            $("#ltlReservationInformation").html(result);
            return false;
        }
        function OnLoadReservInfoFailed(error) {
        }

        function SelectGuestInformation(GuestId) {
            $("#PopSearchPanel").hide('slow');
            $("#PopTabPanel").show('slow');
            PageMethods.LoadDetailInformation(GuestId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            LoadGuestImage(GuestId)
            LoadGuestHistory(GuestId)
            return false;
        }
        function OnLoadDetailObjectSucceeded(result) {
            if (result.GuestDOB) {
                var date1 = new Date(result.GuestDOB);
                $("#<%=lblDGuestDOB.ClientID %>").text(GetStringFromDateTime(result.GuestDOB));
            }
            if (result.PIssueDate) {
                var date2 = new Date(result.PIssueDate);
                $("#<%=lblDPIssueDate.ClientID %>").text(GetStringFromDateTime(result.PIssueDate));
            }
            if (result.PExpireDate) {
                var date3 = new Date(result.PExpireDate);
                $("#<%=lblDPExpireDate.ClientID %>").text(GetStringFromDateTime(result.PExpireDate));
            }
            if (result.VIssueDate) {
                var date4 = new Date(result.VIssueDate);
                $("#<%=lblDVIssueDate.ClientID %>").text(GetStringFromDateTime(result.VIssueDate));
            }
            if (result.VExpireDate) {
                var date5 = new Date(result.VExpireDate);
                $("#<%=lblDVExpireDate.ClientID %>").text(GetStringFromDateTime(result.VExpireDate));
            }

            $("#<%=hiddenGuestName.ClientID %>").val(result.GuestName);
            $("#<%=hiddenGuestId.ClientID %>").val(result.GuestId);
            $("#<%=lblDGuestName.ClientID %>").text(result.GuestName);
            $("#<%=lblDGuestSex.ClientID %>").text(result.GuestSex);
            $("#<%=lblDGuestEmail.ClientID %>").text(result.GuestEmail);
            $("#<%=lblDGuestPhone.ClientID %>").text(result.GuestPhone);
            $("#<%=lblDGuestAddress1.ClientID %>").text(result.GuestAddress1);
            $("#<%=lblDGuestAddress2.ClientID %>").text(result.GuestAddress2);
            $("#<%=lblDGuestCity.ClientID %>").text(result.GuestCity);
            $("#<%=lblDGuestZipCode.ClientID %>").text(result.GuestZipCode);
            $("#<%=lblDGuestNationality.ClientID %>").text(result.GuestNationality);
            $("#<%=lblDGuestDrivinlgLicense.ClientID %>").text(result.GuestDrivinlgLicense);
            $("#<%=lblDGuestAuthentication.ClientID %>").text(result.GuestAuthentication);
            $("#<%=lblDNationalId.ClientID %>").text(result.NationalId);
            $("#<%=lblDPassportNumber.ClientID %>").text(result.PassportNumber);
            $("#<%=lblDPIssuePlace.ClientID %>").text(result.PIssuePlace);
            $("#<%=lblDVisaNumber.ClientID %>").text(result.VisaNumber);
            $("#<%=lblDCountryName.ClientID %>").text(result.CountryName);
            var additionalRemarks = result.AdditionalRemarks != "" ? `<ul><li>${result.AdditionalRemarks}</li></ul>` : "";
            $("#AdditionalRemarks").html(additionalRemarks);
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);
            //after load guest detail
            //toastr.info("Load hoise 1");
            //$("#ContentPlaceHolder1_hfIsBlockGuest").val(result.GuestBlock);
            //var isBlock = $("#ContentPlaceHolder1_hfIsBlockGuest").val();

            //if (isBlock == true) {
            //    toastr.info("Ok");
            //     $("#chkYesBlock").prop("checked", true);
            //}
            //else {
            //     toastr.info("not OK");
            //     $("#chkYesBlock").prop("checked", false);
            //}

            PageMethods.LoadGuestPreferences(result.GuestId, OnLoadGuestPreferencesSucceeded, OnLoadGuestPreferencesFailed);
            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            toastr.error(error.get_message());
        }
        function OnLoadGuestPreferencesSucceeded(result) {
            $("#Preference").html(result);
        }
        function OnLoadGuestPreferencesFailed(error) {
        }

        function LoadGuestHistory(guestId) {
            PageMethods.GetGuestRegistrationHistoryGuestId(guestId, OnLoadGuestHistorySucceeded, OnLoadGuestHistoryFailed);
            return false;
        }
        function OnLoadGuestHistorySucceeded(result) {
            $("#guestHistoryDiv").html(result);
            return false;
        }
        function OnLoadGuestHistoryFailed(error) {
            toastr.error(error.get_message());
        }

        $(function () {
            $("#PopMyTabs").tabs();
        });
        function AddNewItem() {
            $("#TouchKeypad").dialog({
                width: 935,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Search Guest",
                show: 'slide'
            });
            return false;
        }
        function SearchReservation() {
            $("#ReservationPopEntryPanel").show('slow');

            $("#ReservationPopup").dialog({
                autoOpen: true,
                modal: true,
                width: 935,
                closeOnEscape: true,
                resizable: false,
                title: "Reservation Information",
                show: 'slide'
            });

            return false;
        }

        function LoadDataOnParentForm() {
            var guestId = $("#<%=hiddenGuestId.ClientID %>").val();
            $("#TouchKeypad").dialog("close");
            PageMethods.LoadDetailInformation(guestId, OnLoadParentFromDetailObjectSucceeded, OnLoadParentFromDetailObjectFailed);
            return false;
        }
        //load guest info for reserved guest
        function OnLoadParentFromDetailObjectSucceeded(result) {

            $("#ContentPlaceHolder1_chkIsReturnedGuest").attr("checked", true);

            if ($("#hfSearchDetailsFireOrNot").val() != "0") {
                $("#hfSearchDetailsFireOrNot").val("1");
            }
            else {
                $("#hfSearchDetailsFireOrNot").val("0");
            }

            if (result.GuestDOB) {

                $("#<%=txtGuestDOB.ClientID %>").val(GetStringFromDateTime(result.GuestDOB));

            }
            if (result.PIssueDate) {
                $("#<%=txtPIssueDate.ClientID %>").val(GetStringFromDateTime(result.PIssueDate));
            }
            if (result.PExpireDate) {
                $("#<%=txtPExpireDate.ClientID %>").val(GetStringFromDateTime(result.PExpireDate));
            }
            if (result.VIssueDate) {
                $("#<%=txtVIssueDate.ClientID %>").val(GetStringFromDateTime(result.VIssueDate));
            }
            if (result.VExpireDate) {
                $("#<%=txtVExpireDate.ClientID %>").val(GetStringFromDateTime(result.VExpireDate));
            }

            $("#<%=hiddenGuestName.ClientID %>").val(result.GuestName);
            $("#<%=hiddenGuestId.ClientID %>").val(result.GuestId);
            if (result.Title == "") {
                result.Title = "N/A";
            }

            //OnLoadParentFromDetailObjectSucceeded
            //toastr.info("Load hoise 2");
            $("#<%=hfIsBlockGuest.ClientID %>").val(result.GuestBlock);

            var isBlock = $("#ContentPlaceHolder1_hfIsBlockGuest").val();

            if (isBlock == "true") {
                $("#btnAddDetailGuest").attr("disabled", true);
                $("#<%=lblIsGuestBlocked.ClientID %>").text("Guest Blocked.");
                ////$("#chkYesBlock").prop("checked", true);
            }
            else {
                $("#btnAddDetailGuest").attr("disabled", false);
                ////$("#chkYesBlock").prop("checked", false);
                $("#<%=lblIsGuestBlocked.ClientID %>").text("");
            }

            $("#<%=ddlTitle.ClientID %>").val(result.Title);
            $("#<%=txtFirstName.ClientID %>").val(result.FirstName);
            $("#<%=txtLastName.ClientID %>").val(result.LastName);
            $("#<%=txtGuestName.ClientID %>").val(result.GuestName);
            $("#<%=ddlGuestSex.ClientID %>").val(result.GuestSex);
            $("#<%=txtGuestEmail.ClientID %>").val(result.GuestEmail);
            $("#<%=txtGuestPhone.ClientID %>").val(result.GuestPhone);
            $("#<%=txtGuestAddress1.ClientID %>").val(result.GuestAddress1);
            $("#<%=txtGuestAddress2.ClientID %>").val(result.GuestAddress2);
            $("#<%=ddlProfessionId.ClientID %>").val(result.ProfessionId);
            $("#<%=txtGuestCity.ClientID %>").val(result.GuestCity);
            $("#<%=txtGuestZipCode.ClientID %>").val(result.GuestZipCode);

            $("#<%=txtGuestDrivinlgLicense.ClientID %>").val(result.GuestDrivinlgLicense);
            $("#<%=txtNationalId.ClientID %>").val(result.NationalId);
            $("#<%=txtPassportNumber.ClientID %>").val(result.PassportNumber);
            $("#<%=txtVisaNumber.ClientID %>").val(result.VisaNumber);
            $("#<%=ddlGuestCountry.ClientID %>").val(result.GuestCountryId);
            $("#<%=txtGuestNationality.ClientID %>").val(result.GuestNationality);
            $("#txtGuestCountrySearch").val(result.CountryName);
            var guestPreferences = "";
            if (result.GuestPreferences != null)
                guestPreferences += result.GuestPreferences;
            if (result.AdditionalRemarks != "")
                guestPreferences += (guestPreferences != "" ? ", " : "") + result.AdditionalRemarks;
            $("#<%=lblGstPreference.ClientID %>").text(guestPreferences);
            if (guestPreferences != "") {
                $("#GuestPreferenceDiv").show();
            }
            else $("#GuestPreferenceDiv").hide();
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);
            $("#ContentPlaceHolder1_hfAdditionalRemarks").val(result.AdditionalRemarks);


            SelectdPreferenceId = result.GuestPreferenceId != null ? result.GuestPreferenceId : "";
            //$("#<%=txtGuestNationality.ClientID %>").val(result.GuestNationality);
            $("#txtGuestCountrySearch").focus();

            return false;
        }
        function OnLoadParentFromDetailObjectFailed(error) {
            $("#hfSearchDetailsFireOrNot").val("0");
            toastr.error(error.get_message());
        }

        function OnLoadGuestDetailsSucceeded(result) {
            <%--if (result.GuestDOB) {
            $("#<%=txtGuestDOB.ClientID %>").val(GetStringFromDateTime(result.GuestDOB));
        }
        if (result.PIssueDate) {
            $("#<%=txtPIssueDate.ClientID %>").val(GetStringFromDateTime(result.PIssueDate));
        }
        if (result.PExpireDate) {
            $("#<%=txtPExpireDate.ClientID %>").val(GetStringFromDateTime(result.PExpireDate));
        }
        if (result.VIssueDate) {
            $("#<%=txtVIssueDate.ClientID %>").val(GetStringFromDateTime(result.VIssueDate));
        }
        if (result.VExpireDate) {
            $("#<%=txtVExpireDate.ClientID %>").val(GetStringFromDateTime(result.VExpireDate));
        }

        $("#<%=hiddenGuestName.ClientID %>").val(result.GuestName);
        $("#<%=hiddenGuestId.ClientID %>").val(result.GuestId);
        $("#<%=ddlTitle.ClientID %>").val(result.Title);
        $("#<%=txtFirstName.ClientID %>").val(result.FirstName);
        $("#<%=txtLastName.ClientID %>").val(result.LastName);
        $("#<%=txtGuestName.ClientID %>").val(result.GuestName);
        $("#<%=ddlGuestSex.ClientID %>").val(result.GuestSex);
        $("#<%=txtGuestEmail.ClientID %>").val(result.GuestEmail);
        $("#<%=txtGuestPhone.ClientID %>").val(result.GuestPhone);
        $("#<%=txtGuestAddress1.ClientID %>").val(result.GuestAddress1);
        $("#<%=txtGuestAddress2.ClientID %>").val(result.GuestAddress2);
        $("#<%=ddlProfessionId.ClientID %>").val(result.ProfessionId);
        $("#<%=txtGuestCity.ClientID %>").val(result.GuestCity);
        $("#<%=txtGuestZipCode.ClientID %>").val(result.GuestZipCode);
        $("#<%=txtGuestNationality.ClientID %>").val(result.GuestNationality);
        $("#<%=txtGuestDrivinlgLicense.ClientID %>").val(result.GuestDrivinlgLicense);

        $("#<%=txtNationalId.ClientID %>").val(result.NationalId);
        $("#<%=txtPassportNumber.ClientID %>").val(result.PassportNumber);

        $("#<%=txtVisaNumber.ClientID %>").val(result.VisaNumber);

        $("#<%=ddlGuestCountry.ClientID %>").val(result.GuestCountryId);

        $("#txtGuestCountrySearch").val(result.CountryName);
        $("#<%=lblGstPreference.ClientID %>").text(result.GuestPreferences);
        $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);--%>

            $("#<%=ddlReservationGuest.ClientID %>").val(result.GuestId);
            PageMethods.LoadDetailInformation(guestId, OnLoadParentFromDetailObjectSucceeded, OnLoadParentFromDetailObjectFailed);
            return false;
        }
        function OnLoadGuestDetailsFailed(error) {
            toastr.error(error.get_message());
        }

        function SetRegistrationInfoByRoomTypeId(RoomTypeId, DirtyRoomNumber) {
            if ($.trim(DirtyRoomNumber) != "") {
                toastr.info("Before Registration Please Clean Dirty Room No. " + DirtyRoomNumber);
            }

            var resId = $("#<%=ddlReservationId.ClientID %>").val().split('~')[0];
            $('#<%=ddlRoomType.ClientID%>').val(RoomTypeId);
            $("#<%=hiddendReservationId.ClientID %>").val(resId);
            PopulateRooms();
            PerformFillFormActionByTypeIdForReservation(resId, RoomTypeId);
        }

        function PerformFillFormActionByTypeIdForReservation(resId, roomTypeId) {
            PageMethods.PerformFillFormActionByTypeIdTest(resId, roomTypeId, PerformFillFormActionByTypeIdForReservationSucceeded, PerformFillFormActionByTypeIdForReservationFailed);
            return false;
        }
        function PerformFillFormActionByTypeIdForReservationSucceeded(result) {
            if (result.CurrencyType != "Local") {
                $("#<%=txtConversionRate.ClientID %>").attr("disabled", false);
            }

            //toastr.info(result.RoomId);
            $("#<%=txtUnitPrice.ClientID %>").val(result.UnitPrice);
            $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.UnitPrice);
            var roundedRoomRate = Math.round(result.UnitPrice);
            $("#<%=txtRoomRate.ClientID %>").val(roundedRoomRate);
            $("#<%=ddlCurrency.ClientID %>").val(result.CurrencyType);
            $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
            $("#<%=ddlRoomId.ClientID %>").val(result.RoomId);
            $("#<%=ddlRoomIdHiddenField.ClientID %>").val(result.RoomId);
            $("#<%=ddlReferenceId.ClientID %>").val(result.ReferenceId);
            $("#<%=ddlDiscountType.ClientID %>").val(result.DiscountType);
            $("#<%=txtDiscountAmount.ClientID %>").val(result.DiscountAmount);
            var discountAmount = $("#<%=txtDiscountAmount.ClientID %>").val();
            var discountType = $("#<%=ddlDiscountType.ClientID %>").val();
            var txtUnitPrice = $("#<%=txtUnitPrice.ClientID %>").val();
            var txtUnitPriceHiddenField = $("#<%=txtUnitPriceHiddenField.ClientID %>").val();
            if (discountAmount != '') {
                if (discountType == 'Fixed') {
                    var FinalAmount = parseFloat(txtUnitPriceHiddenField) - parseFloat(discountAmount);
                    $("#<%=txtRoomRate.ClientID %>").val(toFixed(FinalAmount, 2));
                }
                else {
                    if (txtUnitPriceHiddenField != '') {
                        var percentage = parseFloat(txtUnitPriceHiddenField) * parseFloat(discountAmount) / 100;
                        var FinalAmount = parseFloat(txtUnitPriceHiddenField) - percentage;

                        var roundedRoomRate = Math.round(FinalAmount);

                        $("#<%=txtRoomRate.ClientID %>").val(roundedRoomRate);
                    }
                    else {
                        $("#<%=txtRoomRate.ClientID %>").val('0.00');
                    }
                }
            }

            TotalRoomRateVatServiceChargeCalculation();
            return false;
        }
        function PerformFillFormActionByTypeIdForReservationFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadReservationGuestInformation(reservationId, reservationDetailId, roomId, loadType) {
            var formDate = $("#<%=txtDisplayCheckInDate.ClientID %>").val();
            var toDate = $("#<%=txtDepartureDate.ClientID %>").val();

            PageMethods.LoadReservationGuestInformation(formDate, toDate, reservationId, reservationDetailId, roomId, loadType, OnLoadReservationGuestInfoSucceeded, OnLoadReservationGuestInfoFailed);
            return false;
        }

        function OnLoadReservationGuestInfoSucceeded(result) {
            PopulateControlWithOutDefault(result[0].roomList, $("#<%=ddlRoomId.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());

            if (result[0].reservationDetails.LoadType == 0) {
                $("#ReservationPopup").dialog("close");
            }

            if (result[0].reservationDetails.CurrencyType != "Local") {
                $("#<%=txtConversionRate.ClientID %>").attr("disabled", false);
            }
            $("#<%=txtUnitPrice.ClientID %>").val(result[0].reservationDetails.UnitPrice);
            $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result[0].reservationDetails.UnitPrice);
            var roundedRoomRate = Math.round(result[0].reservationDetails.UnitPrice);
            $("#<%=txtRoomRate.ClientID %>").val(roundedRoomRate);
            $("#<%=ddlCurrency.ClientID %>").val(result[0].reservationDetails.CurrencyType);
            $("#<%=txtConversionRate.ClientID %>").val(result[0].reservationDetails.ConversionRate);
            $("#<%=ddlRoomType.ClientID %>").val(result[0].reservationDetails.RoomTypeId);
            $("#<%=ddlRoomId.ClientID %>").attr("disabled", false);
            $("#<%=ddlRoomId.ClientID %>").val(result[0].reservationDetails.RoomId);
            $("#<%=ddlRoomIdHiddenField.ClientID %>").val(result[0].reservationDetails.RoomId);
            $("#<%=ddlReferenceId.ClientID %>").val(result[0].reservationDetails.ReferenceId);
            $("#<%=ddlDiscountType.ClientID %>").val(result[0].reservationDetails.DiscountType);
            $("#<%=txtDiscountAmount.ClientID %>").val(result[0].reservationDetails.DiscountAmount);
            $("#<%=ddlReservationId.ClientID %>").val(result[0].reservationDetails.ReservationNDetailNRoomId);
            $('#<%=QSReservationId.ClientID%>').val(result[0].reservationDetails.ReservationNDetailNRoomId);
            $('#txtReservationId').val($("#<%=ddlReservationId.ClientID %> option:selected").text());

            var discountAmount = $("#<%=txtDiscountAmount.ClientID %>").val();
            var discountType = $("#<%=ddlDiscountType.ClientID %>").val();
            var txtUnitPrice = $("#<%=txtUnitPrice.ClientID %>").val();
            var txtUnitPriceHiddenField = $("#<%=txtUnitPriceHiddenField.ClientID %>").val();

            if (discountAmount != '') {
                if (discountType == 'Fixed') {
                    var FinalAmount = parseFloat(txtUnitPriceHiddenField) - parseFloat(discountAmount);
                    $("#<%=txtRoomRate.ClientID %>").val(toFixed(FinalAmount, 2));
                }
                else {
                    if (txtUnitPriceHiddenField != '') {
                        var percentage = parseFloat(txtUnitPriceHiddenField) * parseFloat(discountAmount) / 100;
                        var FinalAmount = parseFloat(txtUnitPriceHiddenField) - percentage;

                        var roundedRoomRate = Math.round(FinalAmount);

                        $("#<%=txtRoomRate.ClientID %>").val(roundedRoomRate);
                    }
                    else {
                        $("#<%=txtRoomRate.ClientID %>").val('0.00');
                    }
                }
            }

            TotalRoomRateVatServiceChargeCalculation();

            if (result[0].reservationDetails.GuestId > 0) {
                PageMethods.LoadDetailInformationFromReservationSearch(result[0].reservationDetails.GuestId, OnLoadGuestDetailsSucceeded, OnLoadGuestDetailsFailed);
            }
            else {
                clearUserDetailsControl();
            }
            return false;
        }

        function OnLoadReservationGuestInfoFailed(error) {
        }

        function LoadGuestImage(guestId) {
            PageMethods.GetDocumentsByUserTypeAndUserId(guestId, OnLoadImagesSucceeded, OnLoadImagesFailed);
            return false;
        }
        function OnLoadImagesSucceeded(result) {
            $("#imageDiv").html(result);
            return false;
        }
        function OnLoadImagesFailed(error) {
            toastr.error(error.get_message());
        }

        //Add guest for save 
        function LoadDetailGridInformation() {
            var ReservationId = 0;
            var reservationId = $("#<%=ddlReservationId.ClientID %>").val();
            if (reservationId != null) {
                ReservationId = $("#<%=ddlReservationId.ClientID %>").val().split('~')[0];
            }
            //Guest Detail            
            var txtTitle = $("#<%=ddlTitle.ClientID %>").val();
            var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();
            //if (txtTitle == "MrNMrs.") {
            //    txtTitle = "Mr. & Mrs.";
            //}
            //else 
            if (txtTitle == "N/A") {
                titleText = "";
            }
            //else {
            //    txtTitle = titleText;
            //    }
            var txtFirstName = $("#<%=txtFirstName.ClientID %>").val().trim();
            var txtLastName = $("#<%=txtLastName.ClientID %>").val().trim();
            var txtGuestName = $("#<%=txtGuestName.ClientID %>").val().trim();
            txtGuestName = txtGuestName.trim();
            var txtGuestEmail = $("#<%=txtGuestEmail.ClientID %>").val();
            var hiddenGuestId = $("#<%=hiddenGuestId.ClientID %>").val();
            var txtGuestDrivinlgLicense = $("#<%=txtGuestDrivinlgLicense.ClientID %>").val();
            var txtGuestDOB = $("#<%=txtGuestDOB.ClientID %>").val();
            var txtGuestAddress1 = $("#<%=txtGuestAddress1.ClientID %>").val();
            var txtGuestAddress2 = $("#<%=txtGuestAddress2.ClientID %>").val();
            var ddlProfessionId = $("#<%=ddlProfessionId.ClientID %>").val();
            var txtGuestCity = $("#<%=txtGuestCity.ClientID %>").val();
            var txtGuestNationality = $("#<%=txtGuestNationality.ClientID %>").val();
            var txtNumberOfPersonAdult = $("#<%=txtNumberOfPersonAdult.ClientID %>").val();
            var txtGuestPhone = $("#<%=txtGuestPhone.ClientID %>").val();

            var ddlGuestSex = $("#<%=ddlGuestSex.ClientID %>").val();
            if (ddlGuestSex == "0") {
                ddlGuestSex = "Male";
            }
            var txtGuestZipCode = $("#<%=txtGuestZipCode.ClientID %>").val();
            var txtNationalId = $("#<%=txtNationalId.ClientID %>").val();
            var txtPassportNumber = $("#<%=txtPassportNumber.ClientID %>").val();
            var txtPExpireDate = $("#<%=txtPExpireDate.ClientID %>").val();
            var txtPIssueDate = $("#<%=txtPIssueDate.ClientID %>").val();
            var txtPIssuePlace = '';
            var txtVExpireDate = $("#<%=txtVExpireDate.ClientID %>").val();
            var txtVisaNumber = $("#<%=txtVisaNumber.ClientID %>").val();
            var txtVIssueDate = $("#<%=txtVIssueDate.ClientID %>").val();

            if (txtGuestName == "" || txtNumberOfPersonAdult == "") {
                if (txtGuestName == "") {
                    toastr.warning("Please provide Guest Name.");
                    $("#<%=txtGuestName.ClientID %>").focus();
                }
                else if (txtNumberOfPersonAdult == "") {
                    toastr.warning("Please Enter Person(Adult).");
                    $("#<%=txtNumberOfPersonAdult.ClientID %>").focus();
                }

                return;
            }



            var rowCount = $('#TableWiseItemInformation tr').length;
            if ($('#btnAddDetailGuest').val() != 'Save') {
                if (txtNumberOfPersonAdult == rowCount - 1) {
                    toastr.warning('Number Of Guest  And Person(Adult) is not Same.');
                    return;
                }
            }

            var ddlGuestCountry = $("#<%=ddlGuestCountry.ClientID %>").val();
            var txtGuestCountrySearch = $.trim($("#txtGuestCountrySearch").val());
            var defaultCountry = $.trim($("#<%= hfDefaultCountryId.ClientID %>").val());
            var enteredCountry = $.trim($("#<%=ddlGuestCountry.ClientID %>").find('option:selected').text());

            if ($("#ContentPlaceHolder1_txtFirstName").val() == "") {
                toastr.warning("Please Provide Guest Name.");
                $("#ContentPlaceHolder1_txtFirstName").focus();
                return;
            }

            if (enteredCountry.toString() != txtGuestCountrySearch.toString()) {
                toastr.warning('Please Enter Valid Country Name.');
                $("#txtGuestCountrySearch").focus();
                return;
            }

            if (txtTitle == "0") {
                toastr.warning('Please Select Title.');
                $("#ddlTitle").focus();
                return;
            }

            if (txtGuestCountrySearch == "") {
                toastr.warning('Please Enter Country Name.');
                $("#txtGuestCountrySearch").focus();
                return;
            }

            //if (defaultCountry != ddlGuestCountry) {
            //    if ($.trim($("#ContentPlaceHolder1_txtPassportNumber").val()) == "") {
            //        toastr.warning('Please Enter Passport Number.');
            //        $("#ContentPlaceHolder1_txtPassportNumber").focus();
            //        return;
            //    }
            //}

            //if (txtGuestEmail != "") {
            //    var mailformat = new RegExp('^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$', 'i');
            //    if (txtGuestEmail.match(mailformat)) {
            //    }
            //    else {
            //        toastr.warning("You have entered an invalid email address!");
            //        document.getElementById("txtGuestEmail>").focus();
            //        return;
            //    }
            //}

            if (ddlGuestSex == "0") {
                toastr.warning('Please Select Gender');
                return;
            }

            // Document Detail
            var isEdit = "";
            if ($("#<%=EditId.ClientID %>").val() == "") {
                isEdit = "";

            }
            else {
                isEdit = $("#<%=EditId.ClientID %>").val();
            }
            $('#btnAddDetailGuest').val('Add');

            var RandomOwnerId = $("#<%=RandomOwnerId.ClientID %>").val();
            var IntOwner = parseInt(RandomOwnerId);

            var IsEditAfterRegistration = $.trim($("#<%=hfIsEditAfterRegistration.ClientID %>").val());

            var tempRegId = "", deletedGuestId = "";

            if (IsEditAfterRegistration != "") {
                tempRegId = $("#<%=hfRegistrationId.ClientID %>").val();
                deletedGuestId = $("#<%=hfDeletedGuest.ClientID %>").val();
            }
            else {
                tempRegId = $("#<%=tempRegId.ClientID %>").val();
                deletedGuestId = "";
            }

            var randomId = parseInt($("#<%=tempRegId.ClientID %>").val());

            var RegId = parseInt(tempRegId);
            //add button click, prevent block user
            var guestDeletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();
            var isBlock = $("#ContentPlaceHolder1_hfIsBlockGuest").val();

            var additionalRemarks = "";

            additionalRemarks = $("#ContentPlaceHolder1_hfAdditionalRemarks").val();
            if (typeof $("#ContentPlaceHolder1_txtAdditionalRemarks").val() !== "undefined") {
                additionalRemarks = $("#ContentPlaceHolder1_txtAdditionalRemarks").val();
            }
            if (isBlock == "true") {
                toastr.warning("Your entered Guest is blocked for this Hotel");
                return;
            }
            //$("#ContentPlaceHolder1_hfIsBlockGuest").val("0");

            PageMethods.SaveGuestInformationAsDetail(RegId, IntOwner, isEdit, txtTitle, txtFirstName, txtLastName, txtGuestName, txtGuestEmail, hiddenGuestId, txtGuestDrivinlgLicense, txtGuestDOB, txtGuestAddress1, txtGuestAddress2, ddlProfessionId, txtGuestCity, ddlGuestCountry, txtGuestNationality, txtGuestPhone, ddlGuestSex, txtGuestZipCode, txtNationalId, txtPassportNumber, txtPExpireDate, txtPIssueDate, txtPIssuePlace, txtVExpireDate, txtVisaNumber, txtVIssueDate, ReservationId, guestDeletedDoc, deletedGuestId, SelectdPreferenceId, additionalRemarks, randomId, OnLoadDetailGridInformationSucceeded, OnLoadDetailGridInformationFailed);
            return false;
        }

        function OnLoadDetailGridInformationSucceeded(result) {

            $("#ltlGuestDetailGrid").html(result);
            $("#<%=EditId.ClientID %>").val("");
            SelectdPreferenceId = "";

            $("#ContentPlaceHolder1_hfAdditionalRemarks").val("");
            $("#ContentPlaceHolder1_txtAdditionalRemarks").val("");
            //LoadComplementaryItem();
            clearUserDetailsControl();
            return false;
        }
        function OnLoadDetailGridInformationFailed(error) {
            $("#<%=EditId.ClientID %>").val("");
            if (error.toString == "2")
                toastr.warning('Provide Valid Email');

            toastr.error(error.get_message());
        }

        function LoadGuestClear() {

            $("#<%=ddlTitle.ClientID %>").val('0');
            $("#<%=txtFirstName.ClientID %>").val('');
            $("#<%=txtLastName.ClientID %>").val('');
            $("#<%=txtGuestName.ClientID %>").val('');
            $("#<%=txtGuestEmail.ClientID %>").val('');
            $("#<%=hiddenGuestId.ClientID %>").val('0');
            $("#<%=txtGuestDrivinlgLicense.ClientID %>").val('');
            $("#<%=txtGuestDOB.ClientID %>").val('');
            $("#<%=txtGuestAddress1.ClientID %>").val('');
            $("#<%=txtGuestAddress2.ClientID %>").val('');
            $("#<%=ddlProfessionId.ClientID %>").val('0');
            $("#<%=txtGuestCity.ClientID %>").val('');
            $("#<%=ddlGuestCountry.ClientID %>").val('');
            $("#<%=txtGuestNationality.ClientID %>").val('');
            $("#<%=txtGuestPhone.ClientID %>").val('');
            $("#<%=ddlGuestSex.ClientID %>").val('0');
            $("#<%=txtGuestZipCode.ClientID %>").val('');
            $("#<%=txtNationalId.ClientID %>").val('');
            $("#<%=txtPassportNumber.ClientID %>").val('');
            $("#<%=txtPExpireDate.ClientID %>").val('');
            $("#<%=txtPIssueDate.ClientID %>").val('');
            $("#<%=txtVExpireDate.ClientID %>").val('');
            $("#<%=txtVisaNumber.ClientID %>").val('');
            $("#<%=txtVIssueDate.ClientID %>").val('');
            $("#<%=lblGstPreference.ClientID %>").text('');
            $("#<%=hfIsBlockGuest.ClientID %>").val('');
            //$("#chkYesBlock").prop("checked", false);
            $("#btnAddDetailGuest").attr("disabled", false);
            $("#<%=lblIsGuestBlocked.ClientID %>").text("")

            $("#GuestPreferenceDiv").hide();
            $("#txtGuestCountrySearch").val("");
            $("#hfSearchDetailsFireOrNot").val("0");
            $("#<%= hfGuestDeletedDoc.ClientID %>").val("");
            $("#GuestDocumentInfo").html('');
        }

        function clearUserDetailsControl() {
            $("#<%=txtFirstName.ClientID %>").val('');
            $("#<%=txtLastName.ClientID %>").val('');
            $("#<%=txtGuestName.ClientID %>").val('');
            $("#<%=txtGuestEmail.ClientID %>").val('');
            $("#<%=hiddenGuestId.ClientID %>").val('0');
            $("#<%=txtGuestDrivinlgLicense.ClientID %>").val('');
            $("#<%=txtGuestDOB.ClientID %>").val('');
            $("#<%=txtGuestAddress1.ClientID %>").val('');
            $("#<%=txtGuestAddress2.ClientID %>").val('');
            $("#<%=ddlProfessionId.ClientID %>").val('0');
            $("#<%=ddlTitle.ClientID %>").val('0');
            $("#<%=txtGuestCity.ClientID %>").val('');
            $("#<%=ddlGuestCountry.ClientID %>").val('');
            $("#<%=txtGuestNationality.ClientID %>").val('');
            $("#<%=txtGuestPhone.ClientID %>").val('');
            $("#<%=ddlGuestSex.ClientID %>").val('0');
            $("#<%=txtGuestZipCode.ClientID %>").val('');
            $("#<%=txtNationalId.ClientID %>").val('');
            $("#<%=txtPassportNumber.ClientID %>").val('');
            $("#<%=txtPExpireDate.ClientID %>").val('');
            $("#<%=txtPIssueDate.ClientID %>").val('');
            $("#<%=txtVExpireDate.ClientID %>").val('');
            $("#<%=txtVisaNumber.ClientID %>").val('');
            $("#<%=txtVIssueDate.ClientID %>").val('');
            <%--$("#<%=txtReservedCompany.ClientID %>").val('');--%>
            $("#<%=lblGstPreference.ClientID %>").text('');
            $("#GuestPreferenceDiv").hide();
            $("#txtGuestCountrySearch").val("");
            $("#hfSearchDetailsFireOrNot").val("0");
            $("#<%= hfGuestDeletedDoc.ClientID %>").val("");
            $("#GuestDocumentInfo").html('');
            var HiddenCompanyId = $("#<%=HiddenCompanyId.ClientID %>").val();

            if (HiddenCompanyId != 0) {
                PageMethods.GetCompanyInformationByCompanyId(HiddenCompanyId, OnGetCompanyInformationByCompanyIdSucceeded, OnGetCompanyInformationByCompanyIdFailed);
                return false;
            }
        }

        function OnGetCompanyInformationByCompanyIdSucceeded(result) {
            $('#ReservedCompany').hide();
            $('#ListedCompany').show();
            $('#PaymentInformation').show();
            //$('#<%=txtContactPerson.ClientID%>').val(result.ContactPerson);
            //$('#<%=txtContactNumber.ClientID%>').val(result.ContactNumber);
            $('#<%=ddlCompanyName.ClientID%>').val(result.CompanyId);
        }
        function OnGetCompanyInformationByCompanyIdFailed(error) {
        }

        function PopulateRooms() {
            $('#<%=ddlEntitleRoomType.ClientID%>').val($('#<%=ddlRoomType.ClientID%>').val());
            if ($('#<%=ddlRoomType.ClientID%>').val() == 0) {
                $("#<%=ddlRoomId.ClientID%>").attr("disabled", "disabled");
            }
            else {
                $("#<%=ddlRoomId.ClientID%>").attr("disabled", false);
            }

            $('#<%=ddlRoomId.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            var StartDate = $('#ContentPlaceHolder1_txtDisplayCheckInDate').val();
            var EndDate = $('#<%=txtDepartureDate.ClientID%>').val();
            var reservationId = 0;
            var chkIsFromReservation = '<%=chkIsFromReservation.ClientID%>'
            if ($('#' + chkIsFromReservation).is(':checked')) {
                reservationId = $('#<%=ddlReservationId.ClientID%>').val().split('~')[0];
            }
            $.ajax({
                type: "POST",
                url: "/HotelManagement/frmRoomRegistrationNew.aspx/PopulateRooms",
                data: '{RoomTypeId: ' + $('#<%=ddlRoomType.ClientID%>').val() + ',ResevationId:' + reservationId + ',FromDate:"' + StartDate + '",ToDate:"' + EndDate + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnRoomPopulated,
                failure: function (response) {
                    toastr.info(response.d);
                }
            });
        }
        function OnRoomPopulated(response) {
            var ddlRoomId = '<%=ddlRoomId.ClientID%>'
            PopulateControlWithOutDefault(response.d, $("#<%=ddlRoomId.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }

        function PerformDeleteActionForGuestDetail(GuestId, RegistrationId) {
            $("#<%=EditId.ClientID %>").val("")
            var IsEditAfterRegistration = $("#<%=hfIsEditAfterRegistration.ClientID %>").val();

            if ($.trim(IsEditAfterRegistration) != "") {
                var deleteId = GuestId + "," + RegistrationId;
                var deletedGuest = $.trim($("#<%=hfDeletedGuest.ClientID %>").val());

                if (deletedGuest != "") {
                    deletedGuest += "#" + deleteId;
                }
                else {
                    deletedGuest = deleteId;
                }

                $("#<%=hfDeletedGuest.ClientID %>").val(deletedGuest);

                $("#tr" + GuestId).remove();
            }
            else {
                PageMethods.PerformDeleteActionForGuestDetailByWM(GuestId, RegistrationId, OnLoadDetailGridInformationSucceeded, OnLoadDetailGridInformationFailed);
            }
            return false;
        }

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        function GetTempRegistrationDetailByWM() {
            PageMethods.GetTempRegistration(OnLoadDetailGridInformationSucceeded, OnLoadDetailGridInformationFailed);
            return false;
        }
        function PerformEditActionForGuestDetail(GuestId, RegistrationId) {
            $("#<%=EditId.ClientID %>").val(GuestId);
            $('#btnAddDetailGuest').val('Save');
            //$('#btnAddDetailGuest').val('Update');
            PageMethods.PerformEditActionForGuestDetailByWM(GuestId, RegistrationId, OnEditGuestInformationSucceeded, OnEditGuestInformationFailed);
            return false;
        }

        function OnEditGuestInformationSucceeded(result) {
            $("#GuestPreferenceDiv").show();
            var guestInfo = result.GuestInfo;
            var guestDoc = result.GuestDoc;

            $("#<%=ddlTitle.ClientID %>").val(guestInfo.Title);
            $("#<%=txtFirstName.ClientID %>").val(guestInfo.FirstName);
            $("#<%=txtLastName.ClientID %>").val(guestInfo.LastName);
            $("#<%=txtGuestName.ClientID %>").val(guestInfo.GuestName);
            var date = new Date(guestInfo.GuestDOB);
            var shortDate = "";
            if (!guestInfo.GuestDOB) {
                shortDate = "";
            }
            else {
                shortDate = GetStringFromDateTime(date);
            }
            $("#<%=txtGuestDOB.ClientID %>").val(shortDate);
            $("#<%=ddlGuestSex.ClientID %>").val(guestInfo.GuestSex);
            $("#<%=txtGuestAddress1.ClientID %>").val(guestInfo.GuestAddress1);
            $("#<%=txtGuestAddress2.ClientID %>").val(guestInfo.GuestAddress2);
            $("#<%=txtGuestEmail.ClientID %>").val(guestInfo.GuestEmail);
            $("#<%=txtGuestPhone.ClientID %>").val(guestInfo.GuestPhone);
            $("#<%=ddlProfessionId.ClientID %>").val(guestInfo.ProfessionId);
            $("#<%=txtGuestCity.ClientID %>").val(guestInfo.GuestCity);
            $("#<%=txtGuestZipCode.ClientID %>").val(guestInfo.GuestZipCode);
            $("#txtGuestCountrySearch").val(guestInfo.CountryName);
            $("#<%=ddlGuestCountry.ClientID %>").val(guestInfo.GuestCountryId);
            $("#<%=txtGuestNationality.ClientID %>").val(guestInfo.GuestNationality);
            $("#<%=txtGuestDrivinlgLicense.ClientID %>").val(guestInfo.GuestDrivinlgLicense);
            $("#<%=txtNationalId.ClientID %>").val(guestInfo.NationalId);
            $("#<%=txtVisaNumber.ClientID %>").val(guestInfo.VisaNumber);
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);
            $("#ContentPlaceHolder1_hfAdditionalRemarks").val(guestInfo.AdditionalRemarks);
            SelectdPreferenceId = result.GuestPreferenceId != null ? result.GuestPreferenceId : "";
            var guestPreferences = "";
            if (result.GuestPreference != null)
                guestPreferences += result.GuestPreference;
            if (guestInfo.AdditionalRemarks != "")
                guestPreferences += (guestPreferences != "" ? ", " : "") + guestInfo.AdditionalRemarks;
            $("#<%=lblGstPreference.ClientID %>").text(guestPreferences);

            var dateVIssue = new Date(guestInfo.VIssueDate);
            var shortDateVIssue = "";
            if (!guestInfo.VIssueDate) {
                shortDateVIssue = "";
            }
            else {
                shortDateVIssue = GetStringFromDateTime(dateVIssue);
            }

            $("#<%=txtVIssueDate.ClientID %>").val(shortDateVIssue);

            var dateVExpire = new Date(guestInfo.VExpireDate);
            var shortDateVExpire = "";
            if (!guestInfo.VExpireDate) {
                shortDateVExpire = "";
            }
            else {
                shortDateVExpire = GetStringFromDateTime(dateVExpire);
            }

            $("#<%=txtVExpireDate.ClientID %>").val(shortDateVExpire);

            var datePIssue = new Date(guestInfo.PIssueDate);
            var shortDatePIssue = "";
            if (!guestInfo.PIssueDate) {
                shortDatePIssue = "";
            }
            else {
                shortDatePIssue = GetStringFromDateTime(datePIssue);
            }

            $("#<%=txtPIssueDate.ClientID %>").val(shortDatePIssue);

            var datePExpire = new Date(guestInfo.PExpireDate);
            var shortDatePExpire = "";
            if (!guestInfo.PExpireDate) {
                shortDatePExpire = "";
            }
            else {
                shortDatePExpire = GetStringFromDateTime(datePExpire);
            }
            $("#<%=txtPExpireDate.ClientID %>").val(shortDatePExpire);

            $("#<%=txtPassportNumber.ClientID %>").val(guestInfo.PassportNumber);

            var totalDoc = guestDoc.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='guestDocList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th><th align='left' scope='col'>Action</th> </tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                //guestDocumentTable += "<td align='left' style='width: 50%' onclick= \"ShowDocument('" + guestDoc[row].Path + "','" + guestDoc[row].Name + "');\">" + guestDoc[row].Name + "</td>";

                //if (result.GuestDoc[row].Path != "") {
                //    imagePath = "<img src='" + guestDoc[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                //}
                //else
                //    imagePath = "";

                //guestDocumentTable += "<td align='left' style='width: 30%' onclick= \"ShowDocument('" + guestDoc[row].Path + "','" + guestDoc[row].Name + "');\">" + imagePath + "</td>";

                guestDocumentTable += "<td align='left' style='width: 50%; cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + guestDoc[row].Path + guestDoc[row].Name + "','" + guestDoc[row].Name + "');\">" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + guestDoc[row].Path + guestDoc[row].Name + "','" + guestDoc[row].Name + "');\">" + imagePath + "</td>";

                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";

            docc = guestDocumentTable;

            $("#GuestDocumentInfo").html(guestDocumentTable);

            return false;
        }
        function OnEditGuestInformationFailed(error) {
            toastr.error(error.get_message());
        }

        <%-- function DeleteGuestDoc(docId, rowIndex) {
            var deletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();

            if (deletedDoc != "")
                deletedDoc += "," + docId;
            else
                deletedDoc = docId;

            $("#<%=hfGuestDeletedDoc.ClientID %>").val(deletedDoc);

            $("#trdoc" + rowIndex).remove();
        }--%>

</script>
    <!-- Upload  Script -->
    <script type="text/javascript">
        var alreadySavePaidServices = [];
        $(document).ready(function () {
            $('#TotalPaid').hide();
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() == "Cash") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').show();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#ComPaymentDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Card") {
                $('#CardPaymentAccountHeadDiv').show();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#ComPaymentDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Cheque") {
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').show();
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#ComPaymentDiv').hide();
                $('#PrintPreviewDiv').show();
            }
            else if ($('#' + ddlPayMode).val() == "Company") {
                $('#PaidByOtherRoomDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').show();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#BankPaymentAccountHeadDiv').hide();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#ComPaymentDiv').show();
                $('#PrintPreviewDiv').hide();
                //popup(1, 'BillSplitPopUpForm', '', 600, 518);
                $("#BillSplitPopUpForm").dialog({
                    width: 600,
                    height: 518,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    title: "Split Bill",
                    show: 'slide'
                });
            }
            else if ($('#' + ddlPayMode).val() == "Other Room") {
                $('#PaidByOtherRoomDiv').show();
                $('#CardPaymentAccountHeadDiv').hide();
                $('#ChecquePaymentAccountHeadDiv').hide();
                $('#CompanyPaymentAccountHeadDiv').hide();
                $('#CashPaymentAccountHeadDiv').hide();
                $('#ComPaymentDiv').hide();
                $('#PrintPreviewDiv').show();
            }


            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            $('#' + ddlPayMode).change(function () {

                if ($('#' + ddlPayMode).val() == "Cash") {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#PaidByOtherRoomDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').show();
                    $('#BankPaymentAccountHeadDiv').hide();
                    $('#ComPaymentDiv').hide();
                    $('#PrintPreviewDiv').show();
                }
                else if ($('#' + ddlPayMode).val() == "Card") {
                    $('#CardPaymentAccountHeadDiv').show();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#PaidByOtherRoomDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').hide();
                    $('#BankPaymentAccountHeadDiv').hide();
                    $('#ComPaymentDiv').hide();
                    $('#PrintPreviewDiv').show();
                }
                else if ($('#' + ddlPayMode).val() == "Cheque") {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').show();
                    $('#PaidByOtherRoomDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').hide();
                    $('#BankPaymentAccountHeadDiv').hide();
                    $('#ComPaymentDiv').hide();
                    $('#PrintPreviewDiv').show();
                }
                else if ($('#' + ddlPayMode).val() == "Company") {
                    $('#PaidByOtherRoomDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').show();
                    $('#CashPaymentAccountHeadDiv').hide();
                    $('#BankPaymentAccountHeadDiv').hide();
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#ComPaymentDiv').show();
                    $('#PrintPreviewDiv').hide();
                    //popup(1, 'BillSplitPopUpForm', '', 600, 518);
                    $("#BillSplitPopUpForm").dialog({
                        width: 600,
                        height: 518,
                        autoOpen: true,
                        modal: true,
                        closeOnEscape: true,
                        resizable: false,
                        fluid: true,
                        title: "Split Bill",
                        show: 'slide'
                    });
                }
                else if ($('#' + ddlPayMode).val() == "Other Room") {
                    $('#PaidByOtherRoomDiv').show();
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').hide();
                    $('#ComPaymentDiv').hide();
                    $('#PrintPreviewDiv').show();
                }
            });

            $("#btnAddDetailGuestPayment").click(function () {
                var ddlCardType = '<%=ddlCardType.ClientID%>'
                var txtReceiveLeadgerAmount = '<%=txtReceiveLeadgerAmount.ClientID%>'
                var txtCardNumber = '<%=txtCardNumber.ClientID%>'
                var ddlPayMode = '<%=ddlPayMode.ClientID%>'
                var ddlBankId = '<%=ddlBankId.ClientID%>'
                var ddlChequeBankId = '<%=ddlChequeBankId.ClientID%>'
                var amount = $('#' + txtReceiveLeadgerAmount).val();
                var number = $('#' + txtCardNumber).val()

                //if ($("#<%=hfCurrencyType.ClientID %>").val() != "Local") {
                if ($("#ContentPlaceHolder1_ddlPaymentCurrency").val() == "0") {
                    toastr.warning("Please Select Currency Type.");
                    return false;
                }

                //else if ($("#ContentPlaceHolder1_txtPaymentConversionRate").val() == "") {
                //    toastr.warning("Please Give Conversion Rate.");
                //    return false;
                //}
                //}
                else if (amount == "") {
                    toastr.warning('Please provide Receive Amount.');
                    $('#' + txtReceiveLeadgerAmount).focus();
                    return;
                }
                else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Card") {
                    var ddlCardType = $.trim($('#<%=ddlCardType.ClientID%>').val());
                    var bankId = $.trim($("#<%=ddlBankId.ClientID %>").val());
                    var cardNumber = $.trim($("#<%=txtCardNumber.ClientID %>").val());

                    if (ddlCardType == "") {
                        toastr.warning("Please select Card Type.");
                        return false;
                    }
                    else if (ddlCardType == "0") {
                        toastr.warning("Please select Card Type.");
                        return false;
                    }
                    else if (bankId == "") {
                        toastr.warning("Please Select Bank Name.");
                        return false;
                    }
                    else if (bankId == "0") {
                        toastr.warning("Please Select Bank Name.");
                        return false;
                    }
                }
                else if ($("#ContentPlaceHolder1_ddlPayMode").val() == "Cheque") {
                    var txtBankId = $.trim($("#<%=ddlChequeBankId.ClientID %>").val());
                    var txtChecqueNumber = $.trim($("#<%=txtChecqueNumber.ClientID %>").val());

                    if (txtChecqueNumber == "") {
                        toastr.warning("Please Provide Checque Number.");
                        return false;
                    }
                    else if (txtBankId == "") {
                        toastr.warning("Please Select Bank Name.");
                        return false;
                    }
                    else if (txtBankId == "0") {
                        toastr.warning("Please Select Bank Name.");
                        return false;
                    }
                }
                //else if ($('#' + ddlPayMode).val() == "Card" && $('#' + ddlCardType).val() == "0") {
                //    toastr.warning('Please Select Card Type.');
                //    $('#' + ddlCardType).focus();
                //    return;
                //}
                //else if ($('#' + ddlPayMode).val() == "Card" && $('#' + ddlBankId).val() == "0") {
                //    toastr.warning('Please provide Bank Name.');
                //    $('#' + ddlBankId).focus();
                //    return;
                //}
                //else if ($('#' + ddlPayMode).val() == "Cheque" && $('#' + ddlChequeBankId).val() == "0") {
                //    toastr.warning('Please provide Bank Name.');
                //    $('#' + ddlBankId).focus();
                //    return;
                //}
                //else {
                SaveGuestPaymentDetailsInformationByWebMethod();
                //}

            });

            $("#btnItemwiseSpecialRemarksCancel").click(function () {
                $("#PaidServiceDialog").dialog("close");
            });

            $("#btnItemwiseSpecialRemarksOk").click(function () {
                CalculateServiceBill();
            });
        });


        function CalculateServiceBill() {
            var registrationId = $("#<%=hfRegistrationId.ClientID %>").val();


            if (registrationId == "") {
                registrationId = 0;
            }


            var pidServiceId = 0, unitPrice = 0.0, totalPrice = 0.0, updatedPrice = 0.0, deletedPrice = 0.0, detailServiceId = 0;
            var previousSelectedPrice = 0.0;
            var HtlRegnPaidServiceDetails = [];
            var HtlRegnPaidServiceDelete = [];
            var SelectedPaidServiceAll = [];


            var serviceSelected = "", unitPriceTxt = "", savedUnitPriceTxt = "";
            $("#TableWisePaidServiceInfo tbody tr").each(function (index, item) {
                serviceSelected = $(this).find('td:eq(1)').find("input");
                unitPriceTxt = $(this).find('td:eq(3)').find("input");
                savedUnitPriceTxt = $(this).find('td:eq(4)').text();
                detailServiceId = $(this).find('td:eq(5)').text();
                pidServiceId = parseInt($(this).find('td:eq(0)').text(), 10);
                unitPrice = parseFloat(unitPriceTxt.val());


                if (serviceSelected.is(':checked')) {
                    var notNew = _.findWhere(alreadySavePaidServices, { ServiceId: pidServiceId });
                    if (notNew != null) {
                        notNew.UnitPrice = unitPrice;


                        if (parseFloat(unitPriceTxt.val()) == parseFloat(savedUnitPriceTxt)) {
                            previousSelectedPrice += parseFloat(savedUnitPriceTxt);
                        }
                    }


                    SelectedPaidServiceAll.push({
                        DetailServiceId: detailServiceId,
                        RegistrationId: registrationId,
                        ServiceId: pidServiceId,
                        UnitPrice: unitPrice,
                        IsAchieved: 0
                    });


                    if (parseFloat(unitPriceTxt.val()) != parseFloat(savedUnitPriceTxt)) {


                        if (notNew != null)
                            updatedPrice += parseFloat(savedUnitPriceTxt);


                        $(this).find('td:eq(4)').text(unitPriceTxt.val());


                        if ($("#<%=hfIsPaidServiceAlreadySavedDb.ClientID %>").val() != "0")
                    $("#<%= hfPaidServiceDeleteObj.ClientID %>").val("1");
            }


            totalPrice += parseFloat(unitPriceTxt.val());
        }
        else {
            var notOld = _.findWhere(alreadySavePaidServices, { ServiceId: pidServiceId });


            if (notOld != null) {
                HtlRegnPaidServiceDelete.push({


                    DetailServiceId: notOld.DetailServiceId,
                    RegistrationId: registrationId,
                    ServiceId: pidServiceId,
                    UnitPrice: unitPrice,
                    IsAchieved: 0


                });


                deletedPrice += parseFloat(savedUnitPriceTxt);
                $("#<%= hfPaidServiceDeleteObj.ClientID %>").val("1");
            }
        }
    });

            var roomRate = parseFloat($("#ContentPlaceHolder1_txtRoomRate").val());
            roomRate = (roomRate - (previousSelectedPrice + updatedPrice + deletedPrice)) + totalPrice;
            $("#ContentPlaceHolder1_txtRoomRate").val(roomRate);

            TotalRoomRateVatServiceChargeCalculation();

            if (registrationId == 0) {
                var queryReservationId = $("#<%=QSReservationId.ClientID %>").val().split('~')[0];;
        var ddlReservationId = $("#<%=ddlReservationId.ClientID %>").val().split('~')[0];

        if (queryReservationId != "" || ddlReservationId != "0") {
            $("#<%= hfPaidServiceDeleteObj.ClientID %>").val("0");
                }
            }

            alreadySavePaidServices = SelectedPaidServiceAll;
            $("#<%=hfPaidServiceSaveObj.ClientID %>").val(JSON.stringify(SelectedPaidServiceAll));

            $("#PaidServiceDialog").dialog("close");
        }


        function ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(ctrl) {
            TotalRoomRateVatServiceChargeCalculation();
        }

        function ToggleTotalRoomRateVatServiceChargeCalculationForVat(ctrl) {
            TotalRoomRateVatServiceChargeCalculation();
        }

        function ToggleIsExpectedCheckOutTimeEnable() {
            var cbIsExpectedCheckOutTimeEnable = '<%=chkIsExpectedCheckOutTimeEnable.ClientID%>'
            if ($('#' + cbIsExpectedCheckOutTimeEnable).is(':checked')) {
                $("#<%=txtProbableDepartureTime.ClientID %>").attr("disabled", false);
            }
            else {
                $("#<%=txtProbableDepartureTime.ClientID %>").attr("disabled", true);
                $("#<%=txtProbableDepartureTime.ClientID %>").val("");
            }
        }

        function TotalRoomRateVatServiceChargeCalculation() {


            var txtUnitPrice = '<%=txtUnitPrice.ClientID%>'
            var txtRoomRate = '<%=txtRoomRate.ClientID%>'
            var cbServiceCharge = '<%=cbServiceCharge.ClientID%>'
            var cbCityCharge = '<%=cbCityCharge.ClientID%>'
            var cbVatAmount = '<%=cbVatAmount.ClientID%>'
            var cbAdditionalCharge = '<%=cbAdditionalCharge.ClientID%>'


            var inclusiveBill = 0, Vat = 0.00, ServiceCharge = 0.00, cityCharge = 0.00, additionalCharge = 0.00;
            var additionalChargeType = "Fixed", isRatePlusPlus = 1, isVatEnableOnGuestHouseCityCharge = 0, isCitySDChargeEnableOnServiceCharge = 0;
            var cbVatAmountVal = 1, cbServiceChargeVal = 1, cbCityChargeVal = 1, cbAdditionalChargeVal = 1;
            var isDiscountApplicableOnRackRate = 0;


            if ($("#ContentPlaceHolder1_hfIsRatePlusPlus").val() != "") { isRatePlusPlus = parseInt($("#ContentPlaceHolder1_hfIsRatePlusPlus").val(), 10); }


            if ($("#<%=hfGuestHouseVat.ClientID %>").val() != "")
                Vat = parseFloat($("#<%=hfGuestHouseVat.ClientID %>").val());


            if ($("#<%=hfGuestHouseServiceCharge.ClientID %>").val() != "")
                ServiceCharge = parseFloat($("#<%=hfGuestHouseServiceCharge.ClientID %>").val());

            if ($("#<%=hfCityCharge.ClientID %>").val() != "")
                cityCharge = parseFloat($("#<%=hfCityCharge.ClientID %>").val());

            if ($("#<%=hfAdditionalCharge.ClientID %>").val() != "")
                additionalCharge = parseFloat($("#<%=hfAdditionalCharge.ClientID %>").val());

            if ($("#<%=hfAdditionalChargeType.ClientID %>").val() != "")
                additionalChargeType = $("#<%=hfAdditionalChargeType.ClientID %>").val();

            if ($("#<%=hfIsVatEnableOnGuestHouseCityCharge.ClientID %>").val() != "")
                isVatEnableOnGuestHouseCityCharge = parseInt($("#<%=hfIsVatEnableOnGuestHouseCityCharge.ClientID %>").val(), 10);

            if ($("#<%=hfIsCitySDChargeEnableOnServiceCharge.ClientID %>").val() != "")
                isCitySDChargeEnableOnServiceCharge = parseInt($("#<%=hfIsCitySDChargeEnableOnServiceCharge.ClientID %>").val(), 10);

            if ($('#' + cbServiceCharge).is(':checked')) {
                cbServiceChargeVal = 1;
            }
            else {
                cbServiceChargeVal = 0;
                ServiceCharge = 0.00;
            }

            if ($('#' + cbCityCharge).is(':checked')) {
                cbCityChargeVal = 1;
            }
            else {
                cbCityChargeVal = 0;
                cityCharge = 0.00;
            }

            if ($('#' + cbVatAmount).is(':checked')) {
                cbVatAmountVal = 1;
            }
            else {
                cbVatAmountVal = 0;
                Vat = 0.00;
            }

            if ($('#' + cbAdditionalCharge).is(':checked')) {
                cbAdditionalChargeVal = 1;
            }
            else {
                cbAdditionalChargeVal = 0;
                additionalCharge = 0.00;
                additionalChargeType = "Percentage";
            }

            var txtRoomRateVal = parseFloat($('#' + txtRoomRate).val());

            if ($("#<%=hfInclusiveHotelManagementBill.ClientID %>").val() != "") {
                inclusiveBill = parseInt($("#<%=hfInclusiveHotelManagementBill.ClientID %>").val(), 10);
            }

            if ($("#<%=hfIsDiscountApplicableOnRackRate.ClientID %>").val() != "") {
                isDiscountApplicableOnRackRate = parseInt($("#<%=hfIsDiscountApplicableOnRackRate.ClientID %>").val(), 10);
            }

            var discountType = "";
            var discountAmount = 0;
            if (isDiscountApplicableOnRackRate == 1) {
                txtRoomRateVal = parseFloat($('#' + txtUnitPrice).val());
                discountType = $("#<%=ddlDiscountType.ClientID %>").val();
                discountAmount = $("#<%=txtDiscountAmount.ClientID %>").val();
            }
            else {
                discountType = "Fixed";
                discountAmount = 0;
            }

            var InnboardRateCalculationInfo = CommonHelper.GetRackRateServiceChargeVatInformation(txtRoomRateVal, ServiceCharge, cityCharge,
                Vat, additionalCharge, additionalChargeType, inclusiveBill, isRatePlusPlus, isVatEnableOnGuestHouseCityCharge, isCitySDChargeEnableOnServiceCharge,
                parseInt(cbVatAmountVal, 10), parseInt(cbServiceChargeVal, 10), parseInt(cbCityChargeVal, 10),
                parseInt(cbAdditionalChargeVal, 10), isDiscountApplicableOnRackRate, discountType, discountAmount);

            OnLoadRackRateServiceChargeVatInformationSucceeded(InnboardRateCalculationInfo);
        }

        function OnLoadRackRateServiceChargeVatInformationSucceeded(result) {
            if (result.RackRate > 0) {
                $("#<%=txtServiceCharge.ClientID %>").val(toFixed(result.ServiceCharge, 2));
                $("#<%=txtCityCharge.ClientID %>").val(toFixed(result.SDCityCharge, 2));
                $("#<%=txtVatAmount.ClientID %>").val(toFixed(result.VatAmount, 2));
                $("#<%=txtAdditionalCharge.ClientID %>").val(toFixed(result.AdditionalCharge, 2));

                if ($("#ContentPlaceHolder1_lblTotalRoomRateOrRoomTariff").text() == "Total Room Rate") {
                    var totalRoomRate = toFixed(result.CalculatedAmount, 2);                    
                    $("#<%=txtTotalRoomRate.ClientID %>").val(totalRoomRate);
                }
                else {
                    $("#<%=txtTotalRoomRate.ClientID %>").val(toFixed(result.RackRate, 2));
                }
            }
            else {
                $("#<%=txtTotalRoomRate.ClientID %>").val('0');
                $("#<%=txtServiceCharge.ClientID %>").val('0');
                $("#<%=txtCityCharge.ClientID %>").val('0');
                $("#<%=txtVatAmount.ClientID %>").val('0');
                $("#<%=txtAdditionalCharge.ClientID %>").val('0');
            }

            return false;
        }

        function OnLoadRackRateServiceChargeVatInformationFailed(error) {
            //alert(error.get_message());
        }

        function UpdateDiscountAmount() {
            var txtDiscountAmount = $('#<%=txtDiscountAmount.ClientID%>').val();
            var txtUnitPrice = '<%=txtUnitPrice.ClientID%>'
            var txtRoomRate = '<%=txtRoomRate.ClientID%>'
            var ddlDiscountType = '<%=ddlDiscountType.ClientID%>'

            var discountType = $('#' + ddlDiscountType).val();
            var unitPrice = 0.0, roomRate = 0.0;

            if ($('#' + txtUnitPrice).val() != "")
                unitPrice = parseFloat($('#' + txtUnitPrice).val());

            if ($('#' + txtRoomRate).val() != "")
                roomRate = parseFloat($('#' + txtRoomRate).val());

            if (txtDiscountAmount == "") {
                txtDiscountAmount = 0;
            }

            if (parseFloat(txtDiscountAmount) == 0) {
                var unitPricePs = 0.0, totalPrice = 0.0;
                var serviceSelected = "", unitPriceTxt = "";

                totalPrice = CalculatePaidServiceTotal();
                unitPrice += totalPrice;
                $('#' + txtRoomRate).val(toFixed(unitPrice, 2));
                return;
            }

            var totalPaidServiceAmount = CalculatePaidServiceTotal();
            unitPrice += totalPaidServiceAmount;

            var discount = 0.0;
            if (discountType == "Fixed") {
                discount = parseFloat(txtDiscountAmount);
                unitPrice = unitPrice - discount;
            }
            else {
                discount = parseFloat(txtDiscountAmount);
                unitPrice = unitPrice - ((unitPrice * discount) / 100);
            }

            var isPackage = $("#ContentPlaceHolder1_hfPackageId").val() != "0";
            var isCheckMinimumRoomRate = $("#<%=hfIsMinimumRoomRateCheckingEnable.ClientID %>").val() == "1";

            if (!isPackage && isCheckMinimumRoomRate) {
                var minimumRoomRate = parseFloat($("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val());

                if (toFixed(unitPrice, 2) < toFixed(minimumRoomRate, 2)) {
                    var actualRoomRate = parseFloat($('#' + txtUnitPrice).val());
                    var maximumDiscount = actualRoomRate - minimumRoomRate;
                    toastr.warning(`Minimum Room Rate For ${$("#<%=ddlRoomType.ClientID %> :selected").text()} : ${minimumRoomRate}. Discount Amount Cannot Greater than ${maximumDiscount}.`);
                    $('#<%=txtDiscountAmount.ClientID%>').val(maximumDiscount).trigger('blur').focus();
                    return true;
                }
            }
            $('#' + txtRoomRate).val(toFixed(unitPrice, 2));
        }

        function CalculatePaidServiceTotal() {
            var totalPrice = 0.0;
            var serviceSelected = "", unitPriceTxt = "";

            if ($("#paidServiceContainer").has("table").length > 0) {
                $("#TableWisePaidServiceInfo tbody tr").each(function (index, item) {
                    serviceSelected = $(this).find('td:eq(1)').find("input");
                    unitPriceTxt = $(this).find('td:eq(3)').find("input");

                    if (serviceSelected.is(':checked')) {
                        totalPrice += parseFloat(unitPriceTxt.val());
                    }
                });
            }
            return totalPrice;
        }

        function DiscountAssignNCalculationForCompanNPromotion() {

        }

        function ValidateForm() {
            var isCardValid = validateCard();
            var isDateValid = true;
            if (isCardValid != true) {
                return false;
            }
            else if (isDateValid != true) {
                toastr.warning("Please fill the Expiry Date.");
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
                toastr.warning(messege);
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

        function SaveGuestPaymentDetailsInformationByWebMethod() {
            var Amount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
            var floatAmout = parseFloat(Amount);
            if (floatAmout <= 0) {
                toastr.warning('Receive Amount is not in correct format.');
                return;
            }

            var isEdit = false;
            if ($('#btnAddDetailGuestPayment').val() == "Save") {
                $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("5");
                isEdit = true;
            }
            else {
                $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("0");
            }

            var ddlPayMode = $("#<%=ddlPayMode.ClientID %>").val();
            var txtReceiveLeadgerAmount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
            var ddlCashReceiveAccountsInfo = $("#<%=ddlCashReceiveAccountsInfo.ClientID %>").val();
            var ddlCardReceiveAccountsInfo = $("#<%=ddlCardReceiveAccountsInfo.ClientID %>").val();
            var ddlChecquePaymentAccountHeadId = $("#<%=ddlChecquePaymentAccountHeadId.ClientID %>").val();
            var txtCardNumber = $("#<%=txtCardNumber.ClientID %>").val();
            var ddlCardType = $("#<%=ddlCardType.ClientID %>").val();
            var txtExpireDate = $("#<%=txtExpireDate.ClientID %>").val();
            var ddlBankId = $("#<%=ddlBankId.ClientID %>").val();
            var txtCardHolderName = $("#<%=txtCardHolderName.ClientID %>").val();
            var txtChecqueNumber = $("#<%=txtChecqueNumber.ClientID %>").val();
            var ddlPaymentType = $("#<%=ddlPaymentType.ClientID %>").val();
            var ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyPaymentAccountHead.ClientID %>").val();
            var ddlCurrency = $("#<%=ddlPaymentCurrency.ClientID %>").val();
            var ddlCurrencyType = $("#<%=hfPaymentCurrencyType.ClientID %>").val();
            var txtConversionRate = $("#<%=txtPaymentConversionRate.ClientID %>").val();

            var paymentDescription = "";
            if (ddlPayMode == "Card") {
                paymentDescription = $("#<%=ddlCardType.ClientID %> option:selected").text();
            }
            else if (ddlPayMode == "Cheque") {
                paymentDescription = txtChecqueNumber;
                var ddlBankId = $("#<%=ddlChequeBankId.ClientID %>").val();
            }


            $('#btnAddDetailGuestPayment').val("Add");
            PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, ddlCurrency, ddlCurrencyType, txtConversionRate, ddlPaymentType, ddlPayMode, txtReceiveLeadgerAmount, ddlCashReceiveAccountsInfo, ddlBankId, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, ddlChecquePaymentAccountHeadId, txtChecqueNumber, ddlCardReceiveAccountsInfo, ddlCompanyPaymentAccountHead, paymentDescription, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)


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
            toastr.error(error.get_message());
        }
        function OnPerformGetTotalPaidAmountSucceeded(result) {
            var txtGrandTotal = 12;


            var _grandTotal = parseFloat(txtGrandTotal);
            var GrandTotal = parseFloat(txtGrandTotal);
            var PaidTotal = parseFloat(result);


            if (_grandTotal == 0) {
                if (PaidTotal != _grandTotal) {
                }
                else {
                }
            }
            else if (PaidTotal == GrandTotal) {

            }
            else {

            }
            var FormatedText = "Total Amount: " + PaidTotal;
            $('#TotalPaid').show();
            $('#TotalPaid').text(FormatedText);
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


        function ClearDetailsPart() {
            $("#<%=txtReceiveLeadgerAmount.ClientID %>").val('');
            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('0');
            $("#<%=txtExpireDate.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');
            $("#<%=txtChecqueNumber.ClientID %>").val('');
            $("#<%=EditId.ClientID %>").val('');
            $("#txtChequeBankId").val('');
            $("#txtBankId").val('');
        }

        function OnLoadSalesDetailGridViewSucceeded(result) {
            $('#productDetailGrid').html(result);
            GetTotalPaidAmount();
            return false;
        }

        function OnLoadSalesDetailGridViewFailed(error) {
        }

        function PerformValidBlankRegistration() {
            validationStatus = false;
            var answer = confirm("Do you want to Generate Blank Registration Card?")
            if (answer) {
                validationStatus = true;
            }
            else {
                validationStatus = false;
                return validationStatus;
            }
        }

        function PerformValidationForSave() {
            
            if ($("#<%=ddlRoomId.ClientID %>").val() == "0") {
                toastr.warning("Please Select Room Number.");
                validationStatus = false;
                
                return validationStatus;
            }



            var rowLength = $('#TableWiseItemInformation tr').length;
            if (rowLength <= 1) {
                $("#<%=hfIsGuestAddFromRegistration.ClientID %>").val(0);

            }
            else {
                $("#<%=hfIsGuestAddFromRegistration.ClientID %>").val(1);
            }


            if (CommonHelper.IsDecimal($('#ContentPlaceHolder1_txtRoomRate').val()) == false) {
                toastr.warning('Entered Negotiated Rate is not in correct format.');
                
                return false;
            }

            if ($("#ContentPlaceHolder1_txtDiscountAmount").val() != "") {
                if (CommonHelper.IsDecimal($("#ContentPlaceHolder1_txtDiscountAmount").val()) == false) {
                    toastr.warning("Please Provide Valid Number in Discount Amount.");
                    $("#ContentPlaceHolder1_txtDiscountAmount").focus();
                    
                    return false;
                }
            }


            var v = $("#ContentPlaceHolder1_txtRoomRate").val();
            var m = $("#ContentPlaceHolder1_ddlIsCompanyGuest").val();


            if ($.trim($("#ContentPlaceHolder1_txtRoomRate").val()) != "0" || $.trim($("#ContentPlaceHolder1_txtRoomRate").val()) != "0.00") {
                var totalPaidSericeCost = CalculatePaidServiceTotal();
                var roomCost = parseFloat($("#ContentPlaceHolder1_txtRoomRate").val());

                if (roomCost < totalPaidSericeCost) {
                    toastr.warning("Negotiated Rate cannot be less than the total Paid Service amount.");
                    
                    return false;
                }
            }

            if ($("#ContentPlaceHolder1_txtArrivalHour").val() != "" || $("#ContentPlaceHolder1_txtArrivalMin").val() != "") {


                if (CommonHelper.IsInt($("#ContentPlaceHolder1_txtArrivalHour").val()) == false) {
                    toastr.warning("Please give valid Arrival Hour.");
                    
                    return false;
                }
                else if (CommonHelper.IsInt($("#ContentPlaceHolder1_txtArrivalMin").val()) == false) {
                    toastr.warning("Please give valid Arrival Minute.");
                    
                    return false;
                }
            }


            if ($('#btnAddDetailGuest').val() == 'Save') {
                toastr.warning("Please Click Save Button First to Update Guest Details.");
                
                return false;
            }

            //  Airport drop validation ------ start


            var airportDrop = $("#<%=ddlAirportDrop.ClientID %>").val();
            $("#<%=hfIsDepartureChargable.ClientID %>").val("0");
            //if ($("#ContentPlaceHolder1_ddlAirportDrop").val() == "0") {
            //     $("#myTabs").tabs({ active: 3 });
            //    toastr.warning("Please Select Airport Drop.");
            //    $("#ContentPlaceHolder1_ddlAirportDrop").focus();
            //    return false;
            //}
            <%--else if ($("#<%=ddlAirportDrop.ClientID %>").val() == "YES") {
                var checkValue = CheckDateTimeValidation("Time", $("#ContentPlaceHolder1_txtDepartureHour"), "Departure Time (ETD)");
                if (checkValue == false) {
                    return false;
                }
            }--%>

            if ((airportDrop == "YES")) {
                if ($("#ContentPlaceHolder1_txtDepartureFlightNumber").val() == "") {
                    toastr.warning("Please Provide Departure Flight Number");
                    $("#myTabs").tabs({ active: 3 });
                    $("#ContentPlaceHolder1_txtDepartureFlightNumber").focus();
                    
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlDepartureFlightName").val() == "0") {
                    toastr.warning("Please Provide Departure Vehicle Name");
                    $("#myTabs").tabs({ active: 3 });
                    $("#ContentPlaceHolder1_ddlDepartureFlightName").focus();
                    
                    return false;
                }
            }
            var checkAirName = CommonHelper.IsInt($("#ContentPlaceHolder1_ddlDepartureFlightName").val());
            if (checkAirName == false) {
                toastr.warning("Please Provide Departure Vehicle Name");
                $("#ContentPlaceHolder1_ddlDepartureFlightName").focus();
                
                return false;
            }

            if (($("#<%=ddlAirportDrop.ClientID %>").val() == "YES") || ($("#<%=ddlAirportDrop.ClientID %>").val() == "TBA")) {

                if ($("#ContentPlaceHolder1_chkIsDepartureChargable").is(":checked")) {
                    $("#ContentPlaceHolder1_hfIsDepartureChargable").val("1");
                }
            }


            //end


            if ($("#ContentPlaceHolder1_hfInitialCurrencyType").val() != $("#<%=ddlCurrency.ClientID %>").val()) {
                if ($('#ContentPlaceHolder1_hfIsPaidServiceAlreadyLoded').val() == "0") {

                    var registrationId = $("#<%=hfRegistrationId.ClientID %>").val();

                    var currencyType = "Local";
                    var currencyId = $("#<%=ddlCurrency.ClientID %>").val();


                    if (currencyId == 1) {
                        currencyType = "Local";
                    }
                    else {
                        currencyType = "Foreign";
                    }


                    var convertionRate = $("#<%=txtConversionRate.ClientID %>").val();
                    var isCeomplementaryService = $("#ContentPlaceHolder1_hfIsComplementaryPaidService").val();


                    if (convertionRate == "")
                        convertionRate = 0;


                    PageMethods.GetPaidServiceDetails(registrationId, currencyId, currencyType, convertionRate, isCeomplementaryService, OnGetPaidServiceForCheckSucceed, OnGetPaidServiceFailed);


                    if ($("#ContentPlaceHolder1_hfIsPaidServiceAlreadySavedDbForCheck").val() == "1") {
                        $("#<%=hfPaidServiceSaveObj.ClientID %>").val("");
                        $("#<%=hfPaidServiceDeleteObj.ClientID %>").val("1");


                        toastr.warning("Your Paid Service Will Be Changed As You Changed Currency");
                    }
                }

                else if ($('#ContentPlaceHolder1_hfIsPaidServiceAlreadyLoded').val() == "1") {
                    CalculateServiceBill();


                    if ($("#<%=hfIsPaidServiceAlreadySavedDb.ClientID %>").val() == "1") {
                        $("#<%=hfPaidServiceDeleteObj.ClientID %>").val(1);
                    }
                }
            }



            var validationStatus = false;


            if ($('#btnAddDetailGuest').val() == 'Save') {
                toastr.warning("Please Click Save Button First to Update Guest Details.");
                
                return false;
            }


            var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
            if ($(ctrl).is(':checked')) {
                var companyId = $("#<%=ddlCompanyName.ClientID %>").val();
                if (companyId > 0) {
                }
                else if (companyId == 0) {
                    toastr.warning("Please select a company.");
                    
                    return false;
                }
                else {
                    toastr.warning("Please select an enlisted company.");
                    
                    return false;
                }
            }

            var commingForm = $("#<%=txtCommingFrom.ClientID %>").val();
            var departureFlightName = $("#ContentPlaceHolder1_ddlDepartureFlightName").find(":selected").text();
            var rowCount = $('#TableWiseItemInformation tr').length;
            var NumberOfPersonAdult = $("#<%=txtNumberOfPersonAdult.ClientID %>").val();


            var currencyIndex = $("#ContentPlaceHolder1_ddlCurrency option:selected").index();


            var txtConversionRate = $("#<%=txtConversionRate.ClientID %>").val();
            var cbFamilyOrCouple = '<%=cbFamilyOrCouple.ClientID%>'
            var chkIsFromReservation = '<%=chkIsFromReservation.ClientID%>'
            var ConversionRate = parseFloat(txtConversionRate);
            var rate = 0;

            if (!isNaN(ConversionRate)) {
                rate = ConversionRate;
            }

            if (currencyIndex > 1) {
                if (rate <= 0) {
                    toastr.warning("Please provide a valid Conversion Rate.");
                    validationStatus = false;
                    
                    return false;
                }
            }

            if (rowCount <= 1) {
                toastr.warning("Please add at least one guest.");
                $("#myTabs").tabs({ active: 1 });
                validationStatus = false;
                
                return false;
            }

            if (NumberOfPersonAdult != rowCount - 1) {
                if ($('#' + cbFamilyOrCouple).is(':checked')) {

                }
                else {
                    toastr.warning('Number Of Guest Added And Person(Adult) is not Same.');
                    validationStatus = false;
                    
                    return false;
                }
            }

            var validationStatus = true;
            var selectedValues = "";
            $("[id*=chkComplementaryItem] input:checked").each(function () {
                if (selectedValues == "") {
                }
                selectedValues += $(this).val() + "\r\n";
            });

            var buttonText = $('#ContentPlaceHolder1_btnSave').val();

            if (buttonText == 'Check-In') {
                if (selectedValues == "") {
                    var answer = confirm("Do you want to save without any complementary item?")
                    if (answer) {
                        validationStatus = true;
                    }
                    else {
                        toastr.warning("Please check at least one complementary item.");
                        validationStatus = false;
                        return validationStatus;
                    }
                }
            }

            return true;
        }

        function OnGetPaidServiceForCheckSucceed(result) {
            if (result.RegistrationPaidService.length > 0) {
                $("#ContentPlaceHolder1_hfIsPaidServiceAlreadySavedDbForCheck").val("1");
            }
            else {
                $("#ContentPlaceHolder1_hfIsPaidServiceAlreadySavedDbForCheck").val("0");
            }
        }

        function GuestDetailsSearchValidationCheck() {
            var isGuestNameGiven = false, isCountryNameGiven = false;

            var guestName = $.trim($("#<%=txtGuestName.ClientID %>").val());
            var guestCountry = $.trim($("#txtGuestCountrySearch").val());

            var guestDOB = $.trim($("#<%=txtGuestDOB.ClientID %>").val());

            var guestEmail = $.trim($("#<%=txtGuestEmail.ClientID %>").val());
            var guestPhone = $.trim($("#<%=txtGuestPhone.ClientID %>").val());

            var nationalId = $.trim($("#<%=txtNationalId.ClientID %>").val());
            var passportNumber = $.trim($("#<%=txtPassportNumber.ClientID %>").val());

            if ($("#hfSearchDetailsFireOrNot").val() == "1") {
                return false;
            }

            if (guestName == "") {
                return false;
            }
            else { isGuestNameGiven = true; }

            if (guestCountry == "") {
                return false;
            }
            else { isCountryNameGiven = true; }

            if (isGuestNameGiven == true && isCountryNameGiven == true) {
                if (isGuestNameGiven == true) {
                    if (guestDOB == "" && guestEmail == "" && guestPhone == "") {
                        return false;
                    }
                }

                if (isCountryNameGiven == true) {
                    if (nationalId == "" && passportNumber == "") {
                        return false;
                    }
                }
            }
            return true;
        }

        function GuestDetailsSearch() {
            var guestName = $.trim($("#<%=txtGuestName.ClientID %>").val());
            var guestCountry = $.trim($("#<%=ddlGuestCountry.ClientID %>").val());
            var guestDOB = $.trim($("#<%=txtGuestDOB.ClientID %>").val());
            var guestEmail = $.trim($("#<%=txtGuestEmail.ClientID %>").val());
            var guestPhone = $.trim($("#<%=txtGuestPhone.ClientID %>").val());
            var nationalId = $.trim($("#<%=txtNationalId.ClientID %>").val());
            var passportNumber = $.trim($("#<%=txtPassportNumber.ClientID %>").val());
            PageMethods.GuestDetailsSearchForAutoPopup(guestName, guestDOB, guestEmail, guestPhone, guestCountry, nationalId, passportNumber, OnLoadGuestDetailsSearchSucceeded, OnLoadGuestDetailsSearchFailed);
        }

        function OnLoadGuestDetailsSearchSucceeded(result) {

            if (result.GuestInfo == null) {
                return false;
            }

            if (!confirm("This guest stayed here before. Do you want to add the guest details?")) {
                return false;
            }

            OnLoadDetailObjectSucceeded(result.GuestInfo);
            OnLoadImagesSucceeded(result.GuestDocuments);
            OnLoadGuestHistorySucceeded(result.GuestRegistrationHistory);

            $("#PopEntryPanel").hide();
            $("#PopSearchPanel").hide();
            $("#TouchKeypad").dialog({
                width: 935,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Search Guest",
                show: 'slide'
            });
            $("#PopTabPanel").show('slow');

            return false;
        }

        function OnLoadGuestDetailsSearchFailed(error) {
            //alert(error.get_message());
        }

        function LoadPaidService() {
            $("#hfPaidServiceDialogDisplayOrNot").val("1");
            AddServiceCharge();
        }

        function AddServiceCharge() {
            var alreadyLoad = $.trim($("#<%=hfIsPaidServiceAlreadyLoded.ClientID %>").val());
            var paidServiceDisplayOrNot = $("#hfPaidServiceDialogDisplayOrNot").val();

    <%--if (paidServiceLoadFromRegistrationRReservation) {
        $("#<%=hfPaidServiceSaveObj.ClientID %>").val("");
        alreadySavePaidServices = [];
        PageMethods.GetPaidServiceDetails(registrationId, currencyId, currencyType, convertionRate, isCeomplementaryService, OnGetPaidServiceSucceed, OnGetPaidServiceFailed);
    }
    else if (!paidServiceLoadFromRegistrationRReservation) {
        $("#<%=hfPaidServiceSaveObj.ClientID %>").val("");
            alreadySavePaidServices = [];
        // Paid Service Temporay Off
        //PageMethods.GetPaidServiceDetailsFromReservation(reservationId, currencyId, currencyType, convertionRate, OnGetPaidServiceSucceed, OnGetPaidServiceFailed);
        }--%>
            return false;
        }

        var vvc = [], vvd = [];
        function OnGetPaidServiceSucceed(result) {
            vvc = result;
            $("#<%=hfIsPaidServiceAlreadyLoded.ClientID %>").val("1");
            var currencyType = $("#<%=ddlCurrency.ClientID %>").val();

            var currencyType = "Local";
            var currencyId = $("#<%=ddlCurrency.ClientID %>").val();

            if (currencyId != 1) {
                currencyType = "Foreign";
            }
            var convertionRate = $("#<%=txtConversionRate.ClientID %>").val();


            if (currencyType == "Foreign") {
                if (convertionRate == "") {
                    toastr.warning("Please give conversion rate");
                    return;
                }
            }

            if (registrationId == "")
                registrationId = 0;


            var reservationId = 0;
            var paidServiceLoadFromRegistrationRReservation = true;


            if (registrationId == 0) {
                var queryReservationId = $("#<%=QSReservationId.ClientID %>").val().split('~')[0];;
                var ddlReservationId = $("#<%=ddlReservationId.ClientID %>").val().split('~')[0];

                if (queryReservationId == "" && ddlReservationId == "0") {
                    paidServiceLoadFromRegistrationRReservation = true;
                }

                if (queryReservationId != "" || ddlReservationId != "0") {
                    paidServiceLoadFromRegistrationRReservation = false;
                    reservationId = (queryReservationId == "" ? ddlReservationId : queryReservationId);
                }
            }

            var isCeomplementaryService = $("#ContentPlaceHolder1_hfIsComplementaryPaidService").val();

            <%--if (paidServiceLoadFromRegistrationRReservation) {
                $("#<%=hfPaidServiceSaveObj.ClientID %>").val("");
                alreadySavePaidServices = [];
                PageMethods.GetPaidServiceDetails(registrationId, currencyId, currencyType, convertionRate, isCeomplementaryService, OnGetPaidServiceSucceed, OnGetPaidServiceFailed);
            }
            else if (!paidServiceLoadFromRegistrationRReservation) {
                $("#<%=hfPaidServiceSaveObj.ClientID %>").val("");
                alreadySavePaidServices = [];
                // Paid Service Temporay Off
                //PageMethods.GetPaidServiceDetailsFromReservation(reservationId, currencyId, currencyType, convertionRate, OnGetPaidServiceSucceed, OnGetPaidServiceFailed);
            }--%>
            return false;
        }

        var vvc = [], vvd = [];
        function OnGetPaidServiceSucceed(result) {
            vvc = result;
            $("#<%=hfIsPaidServiceAlreadyLoded.ClientID %>").val("1");
            var currencyType = $("#<%=ddlCurrency.ClientID %>").val();

            if (result.RegistrationPaidService.length == 0) {
                if ($("#<%=hfPaidServiceSaveObj.ClientID %>").val() != "") {
                    alreadySavePaidServices = JSON.parse($("#<%=hfPaidServiceSaveObj.ClientID %>").val());
                    result.RegistrationPaidService = alreadySavePaidServices;
                    vvd = alreadySavePaidServices;
                }
            }
            else {
                alreadySavePaidServices = result.RegistrationPaidService;
                if ($("#<%=hfPaidServiceSaveObj.ClientID %>").val() != "") {
                    alreadySavePaidServices = JSON.parse($("#<%=hfPaidServiceSaveObj.ClientID %>").val());
                    result.RegistrationPaidService = alreadySavePaidServices;
                }

                $("#<%=hfIsPaidServiceAlreadySavedDb.ClientID %>").val("1");
            }

            if (result.RegistrationPaidService.length > 0) {
                if (currencyType == result.RegistrationPaidService[0].CurrencyType && $("#ContentPlaceHolder1_hfPreviousCurrencyType").val() == currencyType && $("#ContentPlaceHolder1_hfIsCurrencyChange").val() == "0") {
                    alreadySavePaidServices = result.RegistrationPaidService;
                }
                else {
                    alreadySavePaidServices = [];
                    $("#ContentPlaceHolder1_hfPreviousCurrencyType").val(currencyType);
                    $("#ContentPlaceHolder1_hfIsCurrencyChange").val("0");
                }
            }

            vvc = result;

            var table = "", tr = "", td = "", i = 0, alreadyChecked = "", serviceCost = 0, detailServiceId = 0;
            var paidServiceLength = result.PaidService.length;

            table = "<table cellspacing=\"2\" cellpadding=\"2\" id=\"TableWisePaidServiceInfo\" style=\"margin:0;\" >";

            table += "<thead>" +
                "<tr style=\"color: White; background-color: #44545E; font-weight: bold;\">" +
                "<th align=\"center\" scope=\"col\" style=\"width: 30px\">" +
                "Select" +
                "</th>" +
                "<th align=\"center\" scope=\"col\" style=\"width: 350px\">" +
                "Service" +
                "</th>" +
                "<th align=\"center\" scope=\"col\" style=\"width: 65px\">" +
                "Cost (" + $("#<%=ddlCurrency.ClientID %>").find("option:selected").text() + ")" + "</th>" +
                "</tr>" +
                "</thead> <tbody>";

            for (i = 0; i < paidServiceLength; i++) {

                alreadyChecked = '';
                detailServiceId = 0;

                if (currencyType == $("#<%=hfCurrencyType.ClientID %>").val()) {
                    serviceCost = (result.PaidService[i].UnitPriceLocal == null ? 0 : result.PaidService[i].UnitPriceLocal);
                }
                else {
                    serviceCost = (result.PaidService[i].UnitPriceUsd == null ? 0 : result.PaidService[i].UnitPriceUsd);
                }

                var vc = _.findWhere(result.RegistrationPaidService, { ServiceId: result.PaidService[i].ServiceId });
                if (vc != null) {
                    alreadyChecked = "checked='checked'";
                    if (currencyType == $("#<%=hfCurrencyType.ClientID %>").val()) {
                        serviceCost = (vc.UnitPrice == null ? serviceCost : vc.UnitPrice);
                    }
                    else {
                        serviceCost = (vc.UnitPrice == null ? serviceCost : vc.UnitPrice);
                    }

                    detailServiceId = vc.DetailServiceId;
                }
                else
                    alreadyChecked = '';

                if ((i % 2) == 0)
                    tr = "<tr id='trsr" + result.PaidService[i].ServiceId + "' style=\"background-color:#ffffff;\">";
                else
                    tr = "<tr id='trsr" + result.PaidService[i].ServiceId + "' style=\"background-color:#E3EAEB;\">";

                td = "<td style=\"display:none\">" + result.PaidService[i].ServiceId + "</td>" +
                    "<td align=\"center\" style=\"width: 30px\">" +
                    "&nbsp;<input type='checkbox' value=\"" + result.PaidService[i].ServiceId + "\"" + alreadyChecked + " id=\"ch" + result.PaidService[i].ServiceId + "\" onchange = 'ChangeServiceCost (" + result.PaidService[i].ServiceId + ")'" + "\">" +
                    "</td>" +
                    "<td align=\"left\" style=\"width: 350px\">" +
                    result.PaidService[i].ServiceName +
                    "</td>" +
                    "<td align=\"left\" style=\"width: 65px\">"
                    + "<input type=\"text\" value=\"" + serviceCost + "\" id=\"txt" + result.PaidService[i].ServiceId + "\" onblur = 'UpdateServiceCost(this, " + result.PaidService[i].ServiceId + ")' disabled = 'disabled' style=\"width:60px; margin-bottom: 1px; \" />" +
                    "</td>" +
                    "<td align=\"left\" style=\"display:none; width: 65px\">" +
                    serviceCost +
                    "</td>" +
                    "<td align=\"left\" style=\"display:none; width: 65px\">" +
                    detailServiceId +
                    "</td>"

                    ;

                tr += td + "</tr>";

                table += tr;
            }
            table += " </tbody> </table>";

            vvd = table;

            $("#paidServiceContainer").html(table);

            var paidServiceDisplayOrNot = $("#hfPaidServiceDialogDisplayOrNot").val();

            if (paidServiceDisplayOrNot != "0") {
                $("#PaidServiceDialog").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 450,
                    closeOnEscape: true,
                    resizable: false,
                    title: "Paid Service",
                    show: 'slide'
                });
            }
        }

        function OnGetPaidServiceFailed(error) {

        }

        function UpdateServiceCost(cost, serviceId) {
        }

        function ChangeServiceCost(serviceId) {
            if ($("#ch" + serviceId).is(':checked') == true) {
                $("#txt" + serviceId).attr('disabled', false);
            }
            else if ($("#ch" + serviceId).is(':checked') == false) {
                $("#txt" + serviceId).attr('disabled', true);
            }
        }
        function GridPagingForSearchRegistration(pageNumber, isCurrentOrPreviousPage) {
            window.location = "frmRoomRegistrationNew.aspx?pn=" + pageNumber + "&grc=" + ($("#ContentPlaceHolder1_gvRoomRegistration tbody tr").length + 1) + "&icp=" + isCurrentOrPreviousPage;

        }

        // Guest Preference
        function LoadGuestPreference() {
            LoadGuestPreferenceInfo();
            //popup(1, 'DivGuestReference', '', 600, 525);
            $("#DivGuestPreference").dialog({
                width: 600,
                height: 525,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Guest Preference",
                show: 'slide'
            });
            return false;
        }

        function LoadGuestPreferenceInfo() {
            PageMethods.LoadGuestPreferenceInfo(OnLoadGuestPreferenceSucceeded, OnLoadGuestPreferenceFailed);
            return false;
        }
        function OnLoadGuestPreferenceSucceeded(result) {
            $("#ltlGuestPreference").html(result);

            var PreferenceIdList = "";

            PreferenceIdList = $("#ContentPlaceHolder1_hfGuestPreferenceId").val();

            if (SelectdPreferenceId != "") {
                var PreferenceArray = SelectdPreferenceId.split(",");
                if (PreferenceArray.length > 0) {
                    for (var i = 0; i < PreferenceArray.length; i++) {
                        var preferenceId = "#" + PreferenceArray[i].trim();
                        $(preferenceId).attr("checked", true);
                    }
                }
                SelectdPreferenceId = "";
            }

            if (PreferenceIdList != "") {
                var SavedPreferenceArray = PreferenceIdList.split(",");
                if (SavedPreferenceArray.length > 0) {
                    for (var i = 0; i < SavedPreferenceArray.length; i++) {
                        var preferenceId = "#" + SavedPreferenceArray[i].trim();
                        $(preferenceId).attr("checked", true);
                    }
                }
            }
            var additionalRemarks = $("#ContentPlaceHolder1_hfAdditionalRemarks").val();
            $("#ContentPlaceHolder1_txtAdditionalRemarks").val(additionalRemarks);
            return false;
        }
        function OnLoadGuestPreferenceFailed() {
        }

        function GetCheckedGuestPreference() {
            $("#GuestPreferenceDiv").show();
            var SelectdPreferenceName = "";

            $('#GuestPreferenceInformation tbody tr').each(function () {
                var chkBox = $(this).find("td:eq(1)").find("input");

                if ($(chkBox).is(":checked") == true) {
                    if (SelectdPreferenceId != "") {
                        SelectdPreferenceId += ',' + $(chkBox).attr('value');
                        SelectdPreferenceName += ', ' + $(chkBox).attr('name');
                    }
                    else {
                        SelectdPreferenceId = $(chkBox).attr('value');
                        SelectdPreferenceName = $(chkBox).attr('name');
                    }
                }
            });
            if ($("#ContentPlaceHolder1_txtAdditionalRemarks").val() != "") {
                SelectdPreferenceName += (SelectdPreferenceName != "" ? ', ' : '') + $("#ContentPlaceHolder1_txtAdditionalRemarks").val();
            }
            $("#<%=lblGstPreference.ClientID %>").text(SelectdPreferenceName);
            $("#DivGuestPreference").dialog("close");
        }

        function ClosePreferenceDialog() {
            $("#DivGuestPreference").dialog("close");
        }
        function PopUpHotelPositionReportInfo() {
            var url = "";
            var reportType = "HotelPosition";
            var popup_window = "Hotel Position Report";
            url = "/HotelManagement/Reports/frmRoomStatisticsReport.aspx?ReportType=" + reportType;
            window.open(url, popup_window, "width=1100,height=680,left=130,top=5,resizable=yes");
        }

        function PopUpSegmentRateChartInfo() {
        }

        function PerformGuestBillInfoShow(RegistrationId, ConvertionRate) {
            PageMethods.SetSessionValueForGuestBill(RegistrationId, ConvertionRate, OnSessionValueForGuestBillSucceeded, OnSessionValueForGuestBillFailed);
            return false;
        }

        function OnSessionValueForGuestBillSucceeded(result) {
            var RegistrationId = result;
            var url = "/HotelManagement/Reports/frmReportGuestBillInfo.aspx?GuestBillInfo=" + RegistrationId;
            var popup_window = "Bill Preview";
            window.open(url, popup_window, "width=830,height=680,left=300,top=50,resizable=yes");
        }
        function OnSessionValueForGuestBillFailed(error) {
            alert(error.get_message());
        }

        function PerformOtherInformationShow(RegistrationId) {
            PageMethods.PerformOtherInformationByRegistrationId(RegistrationId, OnOtherInformationSucceeded, OnOtherInformationFailed);
            return false;
        }

        function OnOtherInformationSucceeded(result) {
            $("#ContentPlaceHolder1_txtHotelRemarksDisplay").val(result.Remarks);
            $("#ContentPlaceHolder1_txtGuestRemarksDisplay").val(result.GuestRemarks);
            $("#ContentPlaceHolder1_txtPOSRemarksDisplay").val(result.Remarks);
            $("#OtherInformationPanel").dialog({
                width: 900,
                height: 550,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                title: "Other Information",
                show: 'slide'
            });
        }

        function OnOtherInformationFailed(error) {
            alert(error.get_message());
        }

        function LoadPackage(pageNumber, isCurrentOrPreviousPage) {

            var CheckInDate = $("#ContentPlaceHolder1_txtDisplayCheckInDate").val();
            var CheckOutDate = $("#ContentPlaceHolder1_txtDepartureDate").val();
            var companyId = $("#ContentPlaceHolder1_ddlCompanyName").val();
            var gridRecordsCount = SearchRateChart.data().length;

            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './frmRoomRegistrationNew.aspx/GetRateChartListWithPagination',
                data: JSON.stringify({ promotionName: "", companyId, effectFrom: CheckInDate, effectTo: CheckOutDate, gridRecordsCount, pageNumber, isCurrentOrPreviousPage }),
                dataType: "json",
                success: function (data) {
                    OnSearchSuccess(data.d);
                },
                error: function (error) {
                    OnSearchFail(error.d);
                }
            });
            return false;

        }

        function OnSearchSuccess(result) {
            $("#PackageGridPagingContainer ul").html("");
            SearchRateChart.clear();
            SearchRateChart.rows.add(result.GridData);
            SearchRateChart.draw();
            $("#PackageGridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#PackageGridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#PackageGridPagingContainer ul").append(result.GridPageLinks.NextButton);

            $("#PackageDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '95%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: `Package`,
                show: 'slide'
            });
            return false;
        }

        function OnSearchFail(error) {
            toastr.error(error.get_message());
        }

        function LoadPackagePriceNRoomType(id) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './frmRoomRegistrationNew.aspx/GetRateChartById',
                data: JSON.stringify({ id }),
                dataType: "json",
                success: function (data) {
                    $("#ContentPlaceHolder1_ddlRoomType").val(data.d.RateChartDetails[0].RateChartDiscountDetails[0].TypeId).trigger('change').prop('readonly', true);
                    var OfferredPrice = data.d.RateChartMaster.TotalPrice;
                    setTimeout(function () {

                        $("#ContentPlaceHolder1_txtRoomRate").val(OfferredPrice).trigger('blur').prop('readonly', true);
                    }, 2000);

                    $("#ContentPlaceHolder1_txtDiscountAmount").prop('readonly', true);
                    $("#ContentPlaceHolder1_hfPackageId").val(id);
                    $("#PackageDialog").dialog('close');
                },
                error: function (error) {
                    OnSearchFail(error.d);
                }
            });
            return true;
        }
    </script>
    <div id="OtherInformationPanel" class="panel panel-default" style="display:none;">        
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="GuestName" class="control-label col-md-2">
                        Hotel Remarks</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtHotelRemarksDisplay" runat="server" Height="100" TextMode="MultiLine" Enabled="false" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="GuestName" class="control-label col-md-2">
                        Guest Remarks</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtGuestRemarksDisplay" runat="server" Height="100" TextMode="MultiLine" Enabled="false" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="CompanyName" class="control-label col-md-2">
                        POS Remarks</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtPOSRemarksDisplay" runat="server" Height="100" TextMode="MultiLine" Enabled="false" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="PackageDialog" style="width: 450px; display: none;">
        <div  class="panel panel-default">
        <div class="panel-body">
            <table id="tblRateChart" class="table table-bordered table-condensed table-responsive">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="PackageGridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
    </div>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <strong>Notification:</strong>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="PaidServiceDialog" style="width: 450px; display: none;">
        <div class="">
            <div class="DataGridYScroll">
                <div id="paidServiceContainer" style="width: 100%;">
                </div>
            </div>
            <div id="Div4" style="padding-left: 5px; width: 100%; margin-top: 5px;">
                <div style="float: left; padding-bottom: 15px;">
                    <input type="button" id="btnItemwiseSpecialRemarksOk" class="btn btn-primary btn-sm"
                        value="Ok" />
                    <input type="button" id="btnItemwiseSpecialRemarksCancel" class="btn btn-primary btn-sm"
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
    
    
    <asp:HiddenField ID="hfRoomRegistrationId" Value="0" runat="server" />
    <asp:HiddenField ID="hfPackageId" runat="server" value="0"/>
    <asp:HiddenField ID="hfIsMinimumRoomRateCheckingEnable" runat="server" />
    <asp:HiddenField ID="hfDepartureAirlineId" runat="server" />
    <asp:HiddenField ID="hfIsDepartureChargable" runat="server" />
    <input id="hfSearchDetailsFireOrNot" value="0" type="hidden" /> 
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfPaymentCurrencyType" runat="server" />
    <asp:HiddenField ID="hfConversionRate" runat="server" />
    <asp:HiddenField ID="hfIsRoomOverbookingEnable" runat="server" />
    <asp:HiddenField ID="hfInclusiveHotelManagementBill" runat="server" />
    <asp:HiddenField ID="hfIsDiscountApplicableOnRackRate" runat="server" />
    <asp:HiddenField ID="hfGuestHouseVat" runat="server" />
    <asp:HiddenField ID="hfGuestHouseServiceCharge" runat="server" />
    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" />
    <asp:HiddenField ID="hfDefaultCountryId" runat="server" />
    <asp:HiddenField ID="hfRegistrationId" runat="server" />
    <asp:HiddenField ID="hfPaidServiceSaveObj" runat="server" />
    <asp:HiddenField ID="hfPaidServiceDeleteObj" runat="server" />
    <asp:HiddenField ID="hfIsPaidServiceAlreadyLoded" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsPaidServiceAlreadySavedDb" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsPaidServiceAlreadySavedDbForCheck" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletedGuest" runat="server" />
    <asp:HiddenField ID="hfIsEditAfterRegistration" runat="server" />
    <asp:HiddenField ID="hfInitialCurrencyType" runat="server" />
    <asp:HiddenField ID="hfDefaultFrontOfficeMealPlanHeadId" runat="server" Value="0" />
    <asp:HiddenField ID="hfDefaultFrontOfficeMarketSegmentHeadId" runat="server" Value="0" />
    <asp:HiddenField ID="hfPreviousCurrencyType" runat="server" />
    <asp:HiddenField ID="hfIsCurrencyChange" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsComplementaryPaidService" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsGuestAddFromRegistration" runat="server" />
    <asp:HiddenField ID="hfGuestPreferenceId" runat="server" />
    <input id="hfPaidServiceDialogDisplayOrNot" value="1" type="hidden" />
    <asp:HiddenField ID="RandomOwnerId" runat="server" />
    <asp:HiddenField ID="tempRegId" runat="server" />
    <asp:HiddenField ID="hiddendReservationId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="QSReservationId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRatePlusPlus" runat="server" />
    <asp:HiddenField ID="hfIsBlockGuest" runat="server" />
    <asp:HiddenField ID="hfMandatoryFields" runat="server" />
    <asp:HiddenField ID="hfAdditionalRemarks" runat="server" />

    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Registration</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Guest Details</a></li>
            <li id="F" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-6">Complimentary Item</a></li>
            <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-4">Others Information</a></li>
            <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-5">Advance Payment</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-body">
                    <input id="EditId" runat="server" type="hidden" />
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <div class="row">
                                    <label for="Reservation" class="control-label col-md-11">
                                        Reservation</label>
                                </div>
                            </div>
                            <div class="col-md-10" style="padding-bottom:1px;"> 
                                <div class="row" style="padding-left:15px; padding-right:15px;">                                
                                    <div class="input-group">
                                        <span class="input-group-addon" style="padding: 2px 2px;">
                                            <asp:CheckBox ID="chkIsFromReservation" runat="server" Style="height: 16px" Width="25" Text="" onclick="javascript: return LoadTodaysReservation(); ToggleFieldVisible();"
                                                TabIndex="7" />
                                        </span>
                                        <span class="input-group-addon" style="padding: 2px 2px; width: 485px;">
                                        <asp:DropDownList ID="ddlReservationId" runat="server" CssClass="form-control" TabIndex="1">
                                        </asp:DropDownList>
                                              </span>
                                        <div style="display: none;">
                                            <input id="txtReservationId" type="text" style="width: 485px" />
                                        </div>                                  
                                        <span class="input-group-addon" style="padding: 2px 2px;">
                                             &nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="imgReservationSearch" Style="height: 16px" Width="25" runat="server"
                                                OnClientClick="javascript:return SearchReservation()"
                                                ImageUrl="~/Images/SearchItem.png" ToolTip="More Search" />
                                        </span>                                
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="ReservationInformation" style="padding-left: 15px; padding-right: 32px;">
                            <div class="divRightSectionWithThreeDvie">
                                <div id="ltlTableWiseItemInformation">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="CheckInDate" class="control-label col-md-2 required-field">
                                Check-In Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCheckInDateHiddenField" runat="server" CssClass="form-control"
                                    TabIndex="3" Visible="false"></asp:TextBox>
                                <div style="display: none;">
                                    <asp:TextBox ID="txtCheckInDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                                </div>
                                <asp:TextBox ID="txtDisplayCheckInDate" runat="server" CssClass="form-control" TabIndex="5"
                                    Enabled="false"></asp:TextBox>
                            </div>
                            <label for="DepartureDate" class="control-label col-md-2 required-field">
                                Departure Date</label>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtDepartureDate" CssClass="form-control" runat="server" TabIndex="6"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <div class="input-group">
                                    <asp:TextBox ID="txtProbableDepartureTime" runat="server" CssClass="form-control"
                                        TabIndex="7"></asp:TextBox>
                                    <span class="input-group-addon" style="padding: 2px 2px;">
                                        <asp:CheckBox ID="chkIsExpectedCheckOutTimeEnable" runat="server" Text="" CssClass="customChkBox" onclick="javascript: return ToggleIsExpectedCheckOutTimeEnable();"
                                            TabIndex="8" />
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" style="display: none">
                            <label for="BusinessPromotion" class="control-label col-md-2">
                                Business Promotion</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlBusinessPromotionId" runat="server" CssClass="form-control" TabIndex="8">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="ListedCompanyInfo" class="form-group">
                            <div class="col-md-2">
                                <div class="row">
                                    <label for="ListedCompany" class="control-label col-md-11" style="width:100%;">
                                        Listed Company</label>
                                </div>
                            </div>
                            <div class="col-md-10""> 
                                <div class="row" style="padding-left:15px;">                                
                                    <div class="input-group">
                                        <span class="input-group-addon" style="padding: 2px 2px;">
                                            <asp:CheckBox ID="chkIsLitedCompany" runat="server" Style="height: 19px" Width="25" Text="" onclick="javascript: return ToggleFieldVisibleForListedCompany(this);"                                            
                                            TabIndex="9" />
                                        </span>                                    
                                        <div id="ListedCompany" style="display: none; width:98%;">
                                            <input id="txtCompany" type="text" class="form-control" />
                                            <div style="display: none; width:98%;">
                                                <asp:DropDownList ID="ddlCompanyName" runat="server" TabIndex="10" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div id="ReservedCompany" style="width:98%;">
                                            <asp:TextBox ID="txtReservedCompany" runat="server" TabIndex="11" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="PaymentInformation" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                     <div class="row">
                                    <label for="ContactPerson" class="control-label col-md-11" style="width:100%;">
                                        Contact Person</label>
                                         </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="row" style="padding-left:15px;">                                
                                    <div class="input-group">
                                        <span class="input-group-addon" style="padding: 2px 2px;">
                                            <asp:CheckBox ID="chkIsLitedContact" runat="server" Style="height: 19px" Width="25" Text="" onclick="javascript: return ToggleFieldVisibleForListedContact(this);"
                                            
                                            TabIndex="9" />
                                        </span>                                    
                                        <div id="ListedContact" style="display: none; width:98%;">
                                            <input id="txtListedContactPerson" runat="server" type="text" class="form-control" />
                                           <asp:HiddenField  runat="server" ID="hfContactId" Value="0"/>
                                        </div>
                                        <div id="ReservedContact" style="width:98%;">
                                           <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>                                    
                                </div>
                                <label for="MobileNumber" class="control-label col-md-2">
                                    Mobile Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="PaymentMode" class="control-label col-md-2">
                                    Payment Mode</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control" TabIndex="13">
                                        <asp:ListItem Value="Company">Company</asp:ListItem>
                                        <asp:ListItem Value="Self">Self</asp:ListItem>
                                        <asp:ListItem Value="TBA">Before C/O (Company/ Host)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <label for="PayFor" class="control-label col-md-2">
                                    Pay For</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPayFor" runat="server" CssClass="form-control" TabIndex="14">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="CurrencyType" class="control-label col-md-2 required-field">
                                Currency Type</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" TabIndex="15">
                                </asp:DropDownList>
                            </div>
                            <div id="CurrencyAmountInformationDiv" style="display: none">
                                <label for="ConversionRate" class="control-label col-md-2">
                                    Conversion Rate</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control" TabIndex="16"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="RoomType" class="control-label col-md-2 required-field">
                                Room Type</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomType" runat="server" TabIndex="17" CssClass="form-control">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtViewRoomType" runat="server" CssClass="form-control" TabIndex="18"
                                    Visible="false"></asp:TextBox>
                            </div>
                            <div class="col-md-1 col-padding-left-none" style="display:none;">
                                        <input type="button" value="Package" class="btn btn-primary btn-sm"
                                            onclick="javascript: return LoadPackage(1, 1);" />
                                    </div>
                            <label for="RoomNumber" class="control-label col-md-2 required-field">
                                Room Number</label>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-10">
                                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="HiddenCompanyId" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtEntiteledRoomType" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="ddlRoomIdHiddenField" runat="server"></asp:HiddenField>
                                        <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control" TabIndex="19"
                                            Enabled="False">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 col-padding-left-none">
                                        <input type="button" tabindex="18" id="btnHotelPosition" value="HP" class="btn btn-primary btn-sm"
                                            onclick="javascript: return PopUpHotelPositionReportInfo();" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="DiscountType" class="control-label col-md-2">
                                Discount Type</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control" TabIndex="20">
                                    <asp:ListItem>Fixed</asp:ListItem>
                                    <asp:ListItem>Percentage</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <label for="DiscountAmount" class="control-label col-md-2">
                                Discount Amount</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="form-control quantitydecimal" TabIndex="21"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="RackRate" class="control-label col-md-2 required-field">
                                Rack Rate</label>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" TabIndex="22"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="txtUnitPriceHiddenField" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtMinimumUnitPriceHiddenField" runat="server"></asp:HiddenField>
                                    </div>
                                    <div class="col-md-2 col-padding-left-none" style="display: none;">
                                        <input type="button" tabindex="18" id="btnSegmentRateChart" value="..." class="btn btn-primary btn-sm"
                                            onclick="javascript: return PopUpSegmentRateChartInfo();" />
                                    </div>
                                </div>
                            </div>
                            <label for="NegotiatedRate" class="control-label col-md-2 required-field">
                                Negotiated Rate</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRoomRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="23"
                                    onblur="CalculateDiscount()"></asp:TextBox>
                            </div>
                        </div>
                        <asp:Panel ID="pnlRackRateServiceChargeVatInformation" runat="server">
                            <%-- service charge & Vat amount --%>
                            <div class="form-group">
                                <div id="ServiceChargeLabel" class="col-md-2" style="text-align: right;">
                                    <asp:Label ID="lbl" runat="server" CssClass="control-label required-field" Text="Service Charge"></asp:Label>
                                </div>
                                <div id="ServiceChargeControl" class="col-md-4">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtServiceCharge" runat="server" TabIndex="22" CssClass="form-control"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfServiceCharge" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon" style="padding: 2px 2px;">
                                            <asp:CheckBox ID="cbServiceCharge" runat="server" Text="" CssClass="customChkBox"
                                                onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                TabIndex="8" Checked="True" />
                                        </span>
                                    </div>
                                </div>                                
                                <div id="VatAmountLabel" class="col-md-2" style="text-align: right;">
                                    <asp:Label ID="lblVatAmount" runat="server" CssClass="control-label required-field" Text="Vat Amount"></asp:Label>
                                </div>
                                <div id="VatAmountControl" class="col-md-4">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="23" CssClass="form-control"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfVatAmount" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon" style="padding: 2px 2px;">
                                            <asp:CheckBox ID="cbVatAmount" runat="server" Text="" CssClass="customChkBox" onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForVat(this);"
                                                TabIndex="8" Checked="True" />
                                        </span>
                                    </div>
                                </div>
                            </div>                           
                            <%-- city charge & Additional charge --%>
                            <div class="form-group">
                                <div id="CityChargeLabel" class="col-md-2" style="text-align: right;">
                                    <asp:Label ID="lblCityChargeLabel" runat="server" CssClass="control-label required-field" Text="Label"></asp:Label>
                                </div>
                                <div id="CityChargeControl" class="col-md-4">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtCityCharge" runat="server" TabIndex="22" CssClass="form-control"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfCityCharge" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hfIsVatEnableOnGuestHouseCityCharge" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hfIsCitySDChargeEnableOnServiceCharge" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon" style="padding: 2px 2px;">
                                            <asp:CheckBox ID="cbCityCharge" runat="server" Text="" CssClass="customChkBox"
                                                onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                TabIndex="8" Checked="True" />
                                        </span>
                                    </div>
                                </div>
                                <div id="AdditionalChargeLabel" class="col-md-2" style="text-align: right;">
                                    <asp:Label ID="lblAdditionalCharge" runat="server" CssClass="control-label required-field" Text="Additional Charge"></asp:Label>
                                </div>
                                <div id="AdditionalChargeControl" class="col-md-4">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtAdditionalCharge" runat="server" TabIndex="22" CssClass="form-control"
                                            Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="hfAdditionalCharge" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hfAdditionalChargeType" runat="server"></asp:HiddenField>
                                        <span class="input-group-addon" style="padding: 2px 2px;">
                                            <asp:CheckBox ID="cbAdditionalCharge" runat="server" Text="" CssClass="customChkBox"
                                                onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                TabIndex="8" Checked="True" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <%-- total room rate and button --%>
                            <div class="form-group">
                                <div class="col-md-2" style="text-align: right;">
                                    <asp:Label ID="lblTotalRoomRateOrRoomTariff" runat="server" CssClass="control-label required-field" Text="Label"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtTotalRoomRate" runat="server" CssClass="form-control" TabIndex="22"
                                                Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <input type="button" tabindex="18" id="btnRoomRateCalculation" value="RRC" class="btn btn-primary btn-sm"
                                                onclick="javascript: return CalculateRoomRateInclusively();" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group" style="display: none;">
                            <label for="EntitleRoomType" class="control-label col-md-2 required-field">
                                Entitle Room Type</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEntitleRoomType" runat="server" TabIndex="24" CssClass="form-control"
                                    Visible="false">
                                </asp:DropDownList>
                            </div>
                            <label for="EntitleRoomRate" class="control-label col-md-2">
                                Entitle Room Rate</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEntitleRoomRate" runat="server" CssClass="form-control" TabIndex="25"
                                    Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="display: none;">
                            <div style="float: left;">
                                <asp:CheckBox TabIndex="26" ID="chkAdvancePayment" runat="Server" Text="Is Advance Payment?"
                                    onclick="javascript: return ToggleAdvancePaymentFieldVisible(this);" Font-Bold="true"
                                    CssClass="customChkBox" TextAlign="right" Visible="false" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div style="float: right; padding-right: 10px">
                                <input id="btnNext1" tabindex="27" type="button" value="Next" class="btn btn-primary btn-sm"
                                    style="display: none;" />
                            </div>
                        </div>
                        <div class="form-group">
                             <label for="MarketSegment" class="control-label col-md-2">
                                Market Segment</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlMarketSegment" runat="server" CssClass="form-control" TabIndex="28">
                                </asp:DropDownList>
                            </div>
                            <label for="GuestSource" class="control-label col-md-2">
                                Guest Source</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGuestSource" runat="server" CssClass="form-control" TabIndex="29">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Reference" class="control-label col-md-2">
                                Meal Plan</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlMealPlanId" runat="server" CssClass="form-control" TabIndex="30">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Reference" class="control-label col-md-2">
                                Reference</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlReferenceId" runat="server" CssClass="form-control" TabIndex="65">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Remarks" class="control-label col-md-2">
                                Hotel Remarks</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TabIndex="63"
                                    TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Remarks" class="control-label col-md-2">
                                POS Remarks</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPosRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="27"></asp:TextBox>
                            </div>
                        </div>
                        <div id="EarlyCheckInInformationDiv" class="form-group" runat="server">
                            <label for="Remarks" class="control-label col-md-2">
                                Early Check In Charge</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlEarlyCheckInInformation" runat="server" CssClass="form-control" TabIndex="65">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="RoomNo" class="control-label col-md-2">
                                Room Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchRoomNumber" runat="server" CssClass="form-control" TabIndex="32"></asp:TextBox>
                            </div>
                            <label for="RegistrationNo" class="control-label col-md-2">
                                Registration No.</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchRegistrationNumber" runat="server" CssClass="form-control"
                                    TabIndex="33"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="CompanyName" class="control-label col-md-2">
                                Company Name</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchCompanyName" runat="server" CssClass="form-control" TabIndex="30"></asp:TextBox>
                            </div>
                            <label for="Country" class="control-label col-md-2">
                                Country</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchCountry" runat="server" CssClass="form-control" TabIndex="31">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="CheckInDate" class="control-label col-md-2">
                                Check-In Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtChkInDate" runat="server" CssClass="form-control" TabIndex="28"></asp:TextBox>
                            </div>
                            <label for="GuestName" class="control-label col-md-2">
                                Guest Name</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchGuestName" runat="server" CssClass="form-control" TabIndex="29"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                                    TabIndex="34" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="35" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                                    Visible="false" OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                        <div class="panel-body">
                            <asp:GridView ID="gvRoomRegistration" Width="100%" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowCommand="gvRoomRegistration_RowCommand"
                                OnRowDataBound="gvRoomRegistration_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RegistrationNumber" HeaderText="Registration Number" ItemStyle-Width="40%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GuestName" HeaderText="Guest Name" ItemStyle-Width="40%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%#Bind("RegistrationId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False"
                                                CommandArgument='<%# Bind("RegistrationId") %>' CommandName="CmdPreview" ImageUrl="~/Images/ReportDocument.png"
                                                Text="" AlternateText="Preview" ToolTip="Preview" />
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
                        <div class="childDivSection">
                            <div class="text-center" id="GridPagingContainer">
                                <ul class="pagination">
                                    <asp:Literal ID="gridPaging" runat="server"></asp:Literal>
                                </ul>
                            </div>
                        </div>
                        <div id="ImageDivTest">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
            <div class="childDivSection">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label for="PersonAdult" class="control-label col-md-2 required-field">
                            Person (Adult)</label>
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtNumberOfPersonAdult" runat="server" CssClass="form-control" TabIndex="35" MaxLength="2">1</asp:TextBox>
                                </div>
                                <div class="col-md-1 col-padding-left-none">
                                    <asp:CheckBox ID="cbFamilyOrCouple" runat="server" Text="" TabIndex="9" />
                                </div>
                                <div class="col-md-5 col-padding-left-none">
                                    <asp:Label ID="Label16" runat="server" class="control-label" Text="Family/ Couple/ Group"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <label for="PersonChild" class="control-label col-md-2">
                            Person (Child)</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtNumberOfPersonChild" runat="server" CssClass="form-control" TabIndex="36" MaxLength="2"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div id="SearchPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Individual Guest Information
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group" id="divReservationGuest">
                                <label for="Guest Name" class="control-label col-md-2">
                                    Guest Name</label>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlReservationGuest" TabIndex="37" CssClass="form-control"
                                        runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="Title" class="control-label required-field col-md-2">
                                    Title</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlTitle" runat="server" CssClass="form-control" TabIndex="2">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="FirstName" class="control-label col-md-2 required-field">
                                    First Name</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFirstName" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <label for="LastName" class="control-label col-md-2">
                                    Last Name</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtLastName" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="FullName" class="control-label col-md-2 required-field">
                                    Full Name</label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtGuestName" CssClass="form-control" runat="server" TabIndex="38"
                                        ReadOnly></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <button type="button" id="btnAddPerson" class="btn btn-primary btn-sm">
                                        Search Guest</button>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="DateOfBirth" class="control-label col-md-2">
                                    Date Of Birth</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestDOB" runat="server" CssClass="form-control" TabIndex="39"></asp:TextBox>
                                </div>
                                <label for="Gender" class="control-label col-md-2">
                                    Gender</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlGuestSex" runat="server" CssClass="form-control" TabIndex="40">
                                        <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                        <asp:ListItem>Male</asp:ListItem>
                                        <asp:ListItem>Female</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="Company Name" class="control-label col-md-2">
                                    Company Name</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtGuestAddress1" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="Address" class="control-label col-md-2">
                                    Address</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtGuestAddress2" runat="server" CssClass="form-control" TabIndex="42"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="EmailAddress" class="control-label col-md-2">
                                    Email Address</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestEmail" runat="server" TabIndex="43" CssClass="form-control"></asp:TextBox>
                                </div>
                                <label for="Profession" class="control-label col-md-2">
                                    Profession</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlProfessionId" runat="server" TabIndex="40" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="PhoneNumber" class="control-label col-md-2">
                                    Phone Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestPhone" runat="server" CssClass="form-control" TabIndex="44"></asp:TextBox>
                                </div>
                                <label for="City" class="control-label col-md-2">
                                    City</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestCity" runat="server" CssClass="form-control" TabIndex="45"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="ZipCode" class="control-label col-md-2">
                                    Zip Code</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestZipCode" runat="server" CssClass="form-control" TabIndex="46"></asp:TextBox>
                                </div>
                                <label for="Country" class="control-label col-md-2 required-field">
                                    Country</label>
                                <div class="col-md-4">
                                    <input id="txtGuestCountrySearch" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlGuestCountry" runat="server" CssClass="form-control" TabIndex="47">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="Nationality" class="control-label col-md-2">
                                    Nationality</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestNationality" runat="server" CssClass="form-control" TabIndex="48"></asp:TextBox>
                                </div>
                                <label for="DrivingLicense" class="control-label col-md-2">
                                    Driving License</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestDrivinlgLicense" runat="server" CssClass="form-control"
                                        TabIndex="49"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="NationalID" class="control-label col-md-2">
                                    National ID</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNationalId" runat="server" CssClass="form-control" TabIndex="50"></asp:TextBox>
                                </div>
                                <label for="VisaNumber" class="control-label col-md-2">
                                    Visa Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtVisaNumber" runat="server" CssClass="form-control" TabIndex="51"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="VisaIssueDate" class="control-label col-md-2">
                                    Visa Issue Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtVIssueDate" runat="server" CssClass="form-control" TabIndex="52"></asp:TextBox>
                                </div>
                                <label for="VisaExpiryDate" class="control-label col-md-2">
                                    Visa Expiry Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtVExpireDate" runat="server" CssClass="form-control" TabIndex="53"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="PassportNumber" class="control-label col-md-2">
                                    Passport Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPassportNumber" runat="server" CssClass="form-control" TabIndex="54"></asp:TextBox>
                                </div>
                                <%--<label for="PassIssuePlace" class="control-label col-md-2">
                                    Pass. Issue Place</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPIssuePlace" runat="server" CssClass="form-control" TabIndex="55"></asp:TextBox>
                                </div>--%>
                                <label for="PassIssueDate" class="control-label col-md-2">
                                    Pass. Issue Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPIssueDate" runat="server" CssClass="form-control" TabIndex="56"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="PassExpiryDate" class="control-label col-md-2">
                                    Pass. Expiry Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPExpireDate" runat="server" CssClass="form-control" TabIndex="57"></asp:TextBox>
                                </div>
                                <%--<div class="col-md-2">
                                    <asp:Label runat="server" Visible="false" Text="Blocked Guest"></asp:Label>  
                                </div>
                                <div class="col-md-2">
                                     <input class="checkBlock" type="checkbox" id="chkYesBlock" /> Blocked Guest
                                </div>--%>
                            </div>
                            <div class="form-group" id="GuestPreferenceDiv" style="display: none">
                                <label for="GuestPreferences" class="control-label col-md-2">
                                    Guest Preferences</label>
                                <div class="col-md-10">
                                    <asp:Label ID="lblGstPreference" runat="server" class="control-label"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <input type="button" tabindex="18" id="btnGuestReferences" value="Preferences" class="btn btn-primary btn-sm"
                                        onclick="javascript: return LoadGuestPreference()" />
                                </div>
                            </div>
                            <div class="childDivSection">
                                <div id="GuestocumentsInformation" class="panel panel-default" style="height: 100px;">
                                    <div class="panel-heading">
                                        Guest Documents Information
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-horizontal">                                           

                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <label class="control-label">Attachment</label>
                                                </div>
                                                <div class="col-md-10">
                                                    <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>                            
                            <div id="GuestDocumentInfo">
                            </div>
                            <div id="DocumentDialouge" style="display: none;">
                                <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                                    clientidmode="static" scrolling="yes"></iframe>
                            </div>
                        </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <input id="btnAddDetailGuest" type="button" value="Add" class="btn btn-primary btn-sm" />
                                    <input id="btnClearGuest" type="button" value="Cancel" class="btn btn-primary btn-sm" />
                                    <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                                    <asp:Label ID="lblIsGuestBlocked" runat="server" Text='' ForeColor="#CC3300" Font-Bold="True"></asp:Label>
                                </div>
                            </div>
                            <div class="divSection">
                                <div style="float: right; padding-right: 30px">
                                    <input id="btnNext2" type="button" tabindex="59" value="Next" class="btn btn-primary btn-sm"
                                        style="display: none;" />
                                </div>
                                <div style="float: right; padding-right: 10px">
                                    <input id="btnPrev1" type="button" value="Prev" tabindex="60" class="btn btn-primary btn-sm"
                                        style="display: none;" />
                                </div>
                            </div>
                            <div id="ltlGuestDetailGrid" style="padding-top: 10px;">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <div>
                </div>
            </div>
        </div>
        <div id="tab-4">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Others Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="ComingFrom" class="control-label col-md-2">
                                Coming From</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCommingFrom" runat="server" CssClass="form-control" TabIndex="61"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="NextDestination" class="control-label col-md-2">
                                Next Destination</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtNextDestination" runat="server" CssClass="form-control" TabIndex="62"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="VisitPurpose" class="control-label col-md-2">
                                Visit Purpose</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtVisitPurpose" runat="server" CssClass="form-control" TabIndex="63"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="ComplimentaryGuest" class="control-label col-md-2">
                                Complimentary Guest</label>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlIsCompanyGuest" runat="server" CssClass="form-control" TabIndex="70">
                                            <asp:ListItem>No</asp:ListItem>
                                            <asp:ListItem>Yes</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4 col-padding-left-none">
                                        <asp:Label ID="lblIsHouseUseRoom" runat="server" class="control-label" Text="House Use"></asp:Label>
                                    </div>
                                    <div class="col-md-4 col-padding-left-none">
                                        <asp:DropDownList ID="ddlIsHouseUseRoom" runat="server" CssClass="form-control" TabIndex="70">
                                            <asp:ListItem>No</asp:ListItem>
                                            <asp:ListItem>Yes</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <label for="VIPType" class="control-label col-md-2">
                                VIP Type</label>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-6 checkbox" style="padding-left: 35px;">
                                        <asp:CheckBox ID="chkIsVIPGuest" TabIndex="73" runat="Server" Text="Is VIP guest?"
                                            TextAlign="right" />
                                    </div>
                                    <div class="col-md-6 form-inline" style="padding-left: 11px;">
                                        <span style="margin-left: 3px;">
                                            <asp:DropDownList ID="ddlVIPGuestType" runat="server" CssClass="form-control" TabIndex="65">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                </div>
                            </div>                            
                        </div>
                        <div class="form-group">
                            <label for="VisitedType" class="control-label col-md-2">
                                Visited Type</label>
                            <div class="col-md-4 checkbox" style="padding-left: 35px;">
                                <asp:CheckBox ID="chkIsReturnedGuest" TabIndex="72" runat="Server" Text=" Is previously visited guest?"
                                    TextAlign="right" />
                            </div>
                            <div style="display:none;">
                                <label for="RoomOwner" class="control-label col-md-2">
                                    Room Owner</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlRoomOwner" runat="server" CssClass="form-control" TabIndex="71">
                                        <asp:ListItem>No</asp:ListItem>
                                        <asp:ListItem>Yes</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                           </div>
                        </div>
                        <div class="form-group">
                            <div style="float: right;">
                                <input id="btnPrev2" type="button" tabindex="73" value="Prev" class="btn btn-primary btn-sm"
                                    style="display: none;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="AireportArrivalInformation" class="panel panel-default" style="display: none;">
                <div class="panel-heading">
                    Arrival Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="AirportPickUp" class="control-label col-md-2">
                                Pick Up</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAirportPickUp" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="0">--Please Select--</asp:ListItem>
                                    <asp:ListItem>NO</asp:ListItem>
                                    <asp:ListItem>YES</asp:ListItem>
                                    <asp:ListItem>TBA</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="AirportPickUpInformationDiv">
                            <div class="form-group">
                                <label for="FlightNumber" class="control-label col-md-2">
                                    Vehicle Name</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtArrivalFlightName" runat="server" CssClass="form-control" TabIndex="75"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="FlightNumber" class="control-label col-md-2">
                                    Flight Number</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtArrivalFlightNumber" runat="server" CssClass="form-control" TabIndex="75"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="ArrivalTimeETA" class="control-label col-md-2">
                                    Arrival Time (ETA)</label>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtArrivalHour" placeholder="12" CssClass="form-control" runat="server"
                                                TabIndex="76"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3 col-padding-left-none">
                                            <asp:TextBox ID="txtArrivalMin" placeholder="00" CssClass="form-control" TabIndex="77"
                                                runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3 col-padding-left-none">
                                            <asp:DropDownList ID="ddlArrivalAmPm" CssClass="form-control" runat="server" TabIndex="78">
                                                <asp:ListItem>AM</asp:ListItem>
                                                <asp:ListItem>PM</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        (12:00AM)
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    Departure Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="AirportDrop" class="control-label col-md-2">
                                Drop Off</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAirportDrop" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="0">--Please Select--</asp:ListItem>
                                    <asp:ListItem>NO</asp:ListItem>
                                    <asp:ListItem>YES</asp:ListItem>
                                    <asp:ListItem>TBA</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="AirportDropInformationDiv">
                            <%--<div class="form-group">
                                <div class="col-md-2">
                                    <div class="row">
                                        <div class="col-md-3" style="padding-top: 5px;">
                                            <asp:CheckBox ID="chkIsLitedDepartureFlightName" runat="server" Text="" onclick="javascript: return ToggleLitedDepartureFlightNameInfo();"
                                                TabIndex="9" />
                                        </div>
                                        <label for="AirlineName" class="control-label col-md-9">
                                            Listed Airline</label>
                                    </div>
                                </div>
                                <div class="col-md-10">
                                    <div id="ListedDepartureFlightName" style="display: none;">
                                        <input id="txtDepartureAireLineFlightName" type="text" class="form-control" />
                                        <div style="display: none;">
                                            <asp:DropDownList ID="ddlDepartureFlightName" runat="server" TabIndex="10" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div id="ReservedDepartureFlightName">
                                        <asp:TextBox ID="txtDepartureFlightName" runat="server" TabIndex="11" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>--%>
                            <div class="form-group">
                                <label for="AirlineName" class="control-label col-md-2 required-field">
                                    Vehicle Name</label>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlDepartureFlightName" runat="server" TabIndex="10" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="FlightNumber" class="control-label col-md-2 required-field">
                                    Flight Number</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtDepartureFlightNumber" runat="server" CssClass="form-control"
                                        TabIndex="80"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="DepartureTimeETD" class="control-label col-md-2">
                                    Departure Time (ETD)</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDepartureHour" placeholder="12:00" runat="server" CssClass="form-control"
                                        TabIndex="81"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display:none;">
                                <label for="ArrivalTime" class="control-label col-md-2">
                                    &nbsp;</label>
                                <div class="col-md-4">
                                    <asp:CheckBox ID="chkIsDepartureChargable" runat="server" Text="" onclick="javascript: return ToggleFieldVisibleForAllActiveReservation(this);"
                                        TabIndex="2" />&nbsp;&nbsp;
                                    <asp:Label ID="Label5" runat="server" class="control-label" Text="Is Chargable"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default" id="CreditCardInfo" runat="server">
                <div class="panel-heading">
                    Credit Card Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="CardType" class="control-label col-md-2">
                                Card Type</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCreditCardType" TabIndex="93" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="AmericanExpress">American Express</asp:ListItem>
                                    <asp:ListItem Value="MasterCard">Master Card</asp:ListItem>
                                    <asp:ListItem Value="VisaCard">Visa Card</asp:ListItem>
                                    <asp:ListItem Value="DiscoverCard">Discover Card</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <label for="CardNumber" class="control-label col-md-2">
                                Card Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCardNo" TabIndex="94" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="CardHolderName" class="control-label col-md-2">
                                Card Holder Name</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCardHolder" TabIndex="96" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <label for="ExpiryDate" class="control-label col-md-2">
                                Expiry Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtExpiryDate" TabIndex="95" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="CardReference" class="control-label col-md-2">
                                Card Reference</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCardRef" TabIndex="96" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-5">
            <div id="PaymentDetailsInformation" class="childDivSection">
                <div class="panel panel-default" style="min-height: 160px">
                    <div class="panel-heading">
                        Payment Information
                    </div>
                    <div class="childDivSectionDivBlockBody">
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
                                <div class="form-group">
                                    <label for="PaymentType" class="control-label col-md-2 required-field">
                                        Payment Type</label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control" TabIndex="4">
                                            <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                            <asp:ListItem Value="PaidOut">Paid Out</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <label for="PaymentMode" class="control-label col-md-2 required-field">
                                        Payment Mode</label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                            <asp:ListItem>Cash</asp:ListItem>
                                            <asp:ListItem>Card</asp:ListItem>
                                            <asp:ListItem>Cheque</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="CurrencyType" class="control-label col-md-2 required-field">
                                        Currency Type</label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPaymentCurrency" TabIndex="6" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblDisplayConvertionRate" runat="server" Text=""></asp:Label>
                                        <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                                    </div>
                                    <label for="ReceiveAmount" class="control-label col-md-2 required-field">
                                        Receive Amount</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtReceiveLeadgerAmount" runat="server" CssClass="form-control quantitydecimal"
                                            TabIndex="85"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="ConversionPanel" class="form-group">
                                    <label for="ConversionRate" class="control-label col-md-2 required-field">
                                        Conversion Rate</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtPaymentConversionRate" CssClass="form-control" runat="server"></asp:TextBox>
                                        <asp:HiddenField ID="txtPaymentConversionRateHiddenField" runat="server"></asp:HiddenField>
                                    </div>
                                    <label for="CalculatedAmount" class="control-label col-md-2">
                                        Calculated Amount</label>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="txtPaymentCalculatedLedgerAmountHiddenField" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtPaymentCalculatedLedgerAmount" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group" style="display: none;">
                                    <label for="PaymentReceiveIn" class="control-label col-md-2">
                                        Payment Receive In</label>
                                    <div class="col-md-10">
                                        <div id="CashPaymentAccountHeadDiv">
                                            <asp:DropDownList ID="ddlCashReceiveAccountsInfo" runat="server" TabIndex="86" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div id="BankPaymentAccountHeadDiv" style="display: none;">
                                            <asp:DropDownList ID="ddlCardReceiveAccountsInfo" runat="server" TabIndex="87" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
                                            <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" TabIndex="88"
                                                CssClass="childDivSectionDivThreeColumnDropDownList">
                                            </asp:DropDownList>
                                        </div>
                                        <div id="PaidByOtherRoomDiv" style="display: none">
                                            <asp:DropDownList ID="ddlPaidByRegistrationId" runat="server" TabIndex="89" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                                    <div class="form-group" style="display: none;">
                                        <label for="AccountsPostingHead" class="control-label col-md-2">
                                            Accounts Posting Head</label>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control"
                                                TabIndex="90" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="ChequeNumber" class="control-label col-md-2 required-field">
                                            Cheque Number</label>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtChecqueNumber" runat="server" TabIndex="91" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="BankName" class="control-label col-md-2 required-field">
                                            Bank Name</label>
                                        <div class="col-md-10">
                                            <input id="txtChequeBankId" type="text" class="form-control" />
                                            <div style="display: none;">
                                                <asp:DropDownList ID="ddlChequeBankId" CssClass="form-control" runat="server" AutoPostBack="false">
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
                                            <asp:DropDownList ID="ddlCardType" TabIndex="93" runat="server" CssClass="form-control">
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
                                            <asp:TextBox ID="txtCardNumber" TabIndex="94" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group" style="display: none;">
                                        <label for="ExpiryDate" class="control-label col-md-2">
                                            Expiry Date</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtExpireDate" TabIndex="95" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <label for="CardHolderName" class="control-label col-md-2">
                                            Card Holder Name</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtCardHolderName" TabIndex="96" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="BankName" class="control-label col-md-2 required-field">
                                            Bank Name</label>
                                        <div class="col-md-10">
                                            <input id="txtBankId" type="text" class="form-control" />
                                            <div style="display: none;">
                                                <asp:DropDownList ID="ddlBankId" CssClass="form-control" runat="server" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group" style="padding-left: 10px;">
                                    <input type="button" id="btnAddDetailGuestPayment" tabindex="97" value="Add" class="btn btn-primary btn-sm" />
                                    <asp:Label ID="lblHiddenIdDetailGuestPayment" runat="server" Text='' Visible="False"></asp:Label>
                                </div>
                                <div id="GuestPaymentDetailGrid" class="childDivSection">
                                </div>
                                <div id="TotalPaid" class="totalAmout">
                                </div>
                                <div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-6">
            <div class="panel-body">
                <div class="checkboxlistHeader">
                    <asp:CheckBox ID="chkAll" Text="Select All" runat="server" Font-Bold="True"
                        ForeColor="#009933" />&nbsp;
                    <asp:Label ID="lblTotalSelectedEmailCount" runat="server" ForeColor="Red"
                        Style="font-weight: 700" Text=""></asp:Label>
                </div>
                <div class="checkbox checkboxlist col-md-12">
                    <asp:CheckBoxList ID="chkComplementaryItem" TabIndex="98" runat="server">
                    </asp:CheckBoxList>
                </div>
            </div>
        </div>
    </div>
    <div class="row" id="SubmitButtonDiv" style="padding: 10px 0 0 25px;">
        <div class="col-md-9">
            <asp:Button ID="btnSave" runat="server" Text="Check-In" CssClass="TransactionalButton btn btn-primary btn-sm"
                TabIndex="99" OnClick="btnSave_Click" PostBackUrl="~/HotelManagement/frmRoomRegistrationNew.aspx"
                OnClientClick="return PerformValidationForSave();" />
            <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                TabIndex="100" OnClick="btnCancel_Click" />
        </div>
        <div class="col-md-3">
            <asp:Button ID="btnBlankRegistration" runat="server" Text="Blank Registration Card"
                CssClass="btn btn-primary btn-sm" TabIndex="99" OnClick="btnBlankRegistration_Click"
                PostBackUrl="~/HotelManagement/frmRoomRegistrationNew.aspx" OnClientClick="javascript: return PerformValidBlankRegistration();" />
        </div>
    </div>
    <!-- Pop Up Guest Search -->
    <div id="TouchKeypad" style="display: none;">
        <div id="PopMessageBox" class="alert alert-info" style="display: none;">
            <button type="button" class="close" data-dismiss="alert">
                ×</button>
            <asp:Label ID='lblPopMessageBox' Font-Bold="True" runat="server"></asp:Label>
        </div>
        <div id="PopEntryPanel" class="panel panel-default" style="width: 900px">
            <div class="panel-heading">
                Guest Information
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label for="GuestName" class="control-label col-md-2">
                            Guest Name</label>
                        <div class="col-md-10">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:HiddenField ID="hiddenGuestId" runat="server" />
                                    <asp:HiddenField ID="hiddenGuestName" runat="server" />
                                    
                                    <asp:TextBox ID="txtSrcGuestName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                                <div class="form-inline col-md-1" style="display: none;">
                                    <div style="float: right; margin-left: 150px">
                                        <img id="imgCollapse" width="30px" src="/HotelManagement/Image/expand_alt.png" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                            <label for="CompanyName" class="control-label col-md-2">
                                Company Name</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSrcCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                    <div class="form-group">
                            <label for="EmailAddress" class="control-label col-md-2">
                                Email Address</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcEmailAddress" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                            </div>
                            <label for="MobileNumber" class="control-label col-md-2">
                                Mobile Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcMobileNumber" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="NationalID" class="control-label col-md-2">
                                National ID</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcNationalId" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                            </div>
                            <label for="DateOfBirth" class="control-label col-md-2">
                                Date of Birth</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcDateOfBirth" runat="server" CssClass="form-control" TabIndex="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="PassportNumber" class="control-label col-md-2">
                                Passport Number</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSrcPassportNumber" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                            </div>
                        </div>
                    <div id="ExtraSearch">                        
                        <div class="form-group" style="display:none;">
                            <label for="RoomNo" class="control-label col-md-2">
                                Room Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                            <label for="RegistrationNo" class="control-label col-md-2">
                                Registration No.</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcRegistrationNumber" runat="server" CssClass="form-control"
                                    TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="display:none;">
                            <label for="FromDate" class="control-label col-md-2">
                                From Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcFromDate" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                            <label for="ToDate" class="control-label col-md-2">
                                To Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcToDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                        </div>                        
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <button type="button" tabindex="12" id="btnPopSearch" class="btn btn-primary btn-sm">
                                Search</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="PopSearchPanel" class="panel panel-default" style="width: 902px">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div id="ltlTableSearchGuest">
                </div>
                <div class="form-group" style="">
                <table id="tblGuestInfo3" style="width: 100%;" class="table table-bordered table-condensed table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                            <th style="width: 25%;">Guest Name
                            </th>
                            <th style="width: 20%;">Country Name
                            </th>
                            <th style="width: 20%;">Phone
                            </th>
                            <th style="width: 20%;">Email
                            </th>
                            <%--<th style="width: 15%;">Room Number
                            </th>--%>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div class="childDivSection">
                    <div class="text-center" id="GridPagingContainer3">
                        <ul class="pagination">
                        </ul>
                    </div>
                </div>
            </div>
            </div>
        </div>
        <div style="height: 45px">
        </div>
        <div id="PopTabPanel" style="width: 935px">
            <div id="PopMyTabs">
                <ul id="PoptabPage" class="ui-style">
                    <li id="PopA" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-1">Guest Information</a></li>
                    <li id="PopB" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-2">Guest Document</a></li>
                    <li id="PopC" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-3">Guest History</a></li>
                    <li id="PopD" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-4">Guest Preferences</a></li>
                </ul>
                <div id="Poptab-1">
                    <div id="GuestInfo" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Guest Information
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <table class="table table-striped table-bordered table-condensed table-hover">
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestName" runat="server" Text="Guest Name"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestName" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestDOB" runat="server" Text="Date of Birth"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestDOB" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestSex" runat="server" Text="Gender"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestSex" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestEmail" runat="server" Text="Email"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestEmail" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestPhone" runat="server" Text="Phone Number"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestPhone" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestAddress1" runat="server" Text="Company Name"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestAddress1" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestAddress2" runat="server" Text="Guest Address"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestAddress2" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestCity" runat="server" Text="City "></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestCity" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestZipCode" runat="server" Text="Zip Code"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestZipCode" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestNationality" runat="server" Text="Guest Nationality"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestNationality" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestDrivinlgLicense" runat="server" Text="Driving License No"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestDrivinlgLicense" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestAuthentication" runat="server" Text="Authentication"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestAuthentication" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLNationalId" runat="server" Text="National ID"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDNationalId" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLPassportNumber" runat="server" Text="Passport
    Number"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPassportNumber" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">Visited Type
                                            <asp:Label ID="lblLPIssueDate" runat="server" Text="Pasport Issue Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPIssueDate" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLPIssuePlace" runat="server" Text="Passport
    Issue Place"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPIssuePlace" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLPExpireDate" runat="server" Text="Passport Expiry Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPExpireDate" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLVisaNumber" runat="server" Text="Visa
    Number"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDVisaNumber" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLVIssueDate" runat="server" Text="Visa Issue Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDVIssueDate" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLVExpireDate" runat="server" Text="Visa Expiry Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDVExpireDate" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLCountryName" runat="server" Text="Country Name"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDCountryName" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Poptab-2">
                    <div id="imageDiv">
                    </div>
                </div>
                <div id="Poptab-3">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div id="guestHistoryDiv">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Poptab-4">
                    <div id="Div5" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Preferences
                        </div>
                        <div class="panel-body">
                            <div id="Preference">
                            </div>
                        </div>
                         <div class="panel-heading">
                                Additional Remarks
                            </div>
                            <div class="panel-body">
                                <div id="AdditionalRemarks">
                                </div>
                            </div>
                    </div>
                </div>
                <button type="button" id="btnSearchSuccess" class="btn btn-primary btn-sm">
                    OK</button>
                <button type="button" id="btnSearchCancel" class="btn btn-primary btn-sm">
                    Cancel</button>
                <button type="button" id="btnPrintDocument" class="btn btn-primary btn-sm" style="display: none;">
                    Print Preview</button>
            </div>
        </div>
    </div>
    <!-- End Pop Up Guest Search -->
    <!-- Pop Up Guest Search -->
    <div id="ReservationPopup" style="display: none;">
        <div id="Div2" class="alert alert-info" style="display: none;">
            <button type="button" class="close" data-dismiss="alert">
                ×</button>
            <asp:Label ID='Label17' Font-Bold="True" runat="server"></asp:Label>
        </div>
        <div id="ReservationPopEntryPanel" class="panel panel-default" style="width: 875px">
            <div class="panel-heading">
                Search Reservation
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <%-- <div class="col-md-2">
                            <asp:Label ID="lblResvGuestName" runat="server" class="control-label" Text="Guest Name"></asp:Label>
                        </div>--%>
                        <label for="Country" class="control-label col-md-2">
                            Guest Name</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtResvGuestName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <%--<div class="col-md-2">
                            <asp:Label ID="lblResvCompanyName" runat="server" class="control-label" Text="Company Name"></asp:Label>
                        </div>--%>
                        <label for="Country" class="control-label col-md-2">
                            Company Name</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtResvCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <%-- <div class="col-md-2">
                            <asp:Label ID="lblRsvCheckInDate" runat="server" class="control-label" Text="Check-In Date"></asp:Label>
                        </div>--%>
                        <label for="Country" class="control-label col-md-2">
                            Check-In Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtRsvCheckInDate" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                        </div>
                        <%--<div class="col-md-2">
                            <asp:Label ID="lblCheckOutDate" runat="server" class="control-label" Text="Check-Out Date"></asp:Label>
                        </div>--%>
                        <label for="Country" class="control-label col-md-2">
                            Check-Out Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtCheckOutDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <%-- <div class="col-md-2">
                            <asp:Label ID="lblReservationNo" runat="server" class="control-label" Text="Reservation No."></asp:Label>
                        </div>--%>
                        <label for="Country" class="control-label col-md-2">
                            Reservation No.</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtReservationNo" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <button type="button" id="btnReservationSearch" class="TransactionalButton btn btn-primary btn-sm">
                                Search</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="ReservationPopSearchPanel" class="panel panel-default" style="width: 875px">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div id="ltlReservationInformation">
                </div>
                <div class="form-group">
                    <table id="tblGuestInfo" style="width: 100%;" class="table table-bordered table-condensed table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                            <th style="width: 10%;">Reservation Number 
                            </th>
                            <th style="width: 25%;">Guest Name 
                            </th>
                            <th style="width: 20%;">Room Type 
                            </th>
                            <th style="width: 25%;">Room Number 
                            </th>
                            <th style="width: 10%;">Check In 
                            </th>
                            <th style="width: 10%;">Check Out 
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div class="childDivSection">
                    <div class="text-center" id="GridPagingContainer2">
                        <ul class="pagination">
                        </ul>
                    </div>
                </div>
                </div>
            </div>
        </div>
    </div>
    <!-- End Pop Up Guest Search -->
    <!--Guest Preference PopUp -->
    <div id="DivGuestPreference" style="display: none;">
        <div id="Div1">
            <div id="ltlGuestPreference">
            </div>
        </div>
    </div>
    <!--End Guest Preference PopUp -->


    <!--Start Calculate Rack Rate Inclusively PopUp -->
    <div id="CalculateRackRateInclusivelyDialog" style="display: none;">
        <asp:HiddenField ID="hfIsServiceChargeEnableConfig" runat="server" />
        <asp:HiddenField ID="hfIsCitySDChargeEnableConfig" runat="server" />
        <asp:HiddenField ID="hfIsVatEnableConfig" runat="server" />
        <asp:HiddenField ID="hfIsAdditionalChargeEnableConfig" runat="server" />
        <div class="form-horizontal">
            <div class="form-group">
                <div id="RRCtotalRoomRateDiv">
                    <label class="control-label col-md-2 required-field">
                        Total Room Rate</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCalculatedTotalRoomRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="22"
                            onblur="CalculateRateInclusively()"></asp:TextBox>
                    </div>
                </div>
                <div id="RRCrackRateDiv">
                    <label class="control-label col-md-2 required-field">
                        Rack Rate</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCalculateRackRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="23" Enabled="false"></asp:TextBox>
                    </div>
                </div>

            </div>

            <div class="form-group">
                <div id="RRCserviceChargeDiv">
                    <div class="col-md-2" style="text-align: right;">
                        <asp:Label ID="Label1" runat="server" CssClass="control-label required-field" Text="Service Charge"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <div class="input-group">
                            <asp:TextBox ID="txtCalculateServiceCharge" runat="server" TabIndex="22" CssClass="form-control"
                                Enabled="false"></asp:TextBox>
                            <span class="input-group-addon">
                                <asp:CheckBox ID="cbCalculateServiceCharge" runat="server" Text="" CssClass="customChkBox"
                                    onclick="javascript: return CalculateRateInclusively(this);"
                                    TabIndex="8" Checked="True" />
                            </span>
                        </div>
                    </div>
                </div>

                <div id="RRCcitySdChargeDiv">
                    <div class="col-md-2" style="text-align: right;">
                        <asp:Label ID="Label3" runat="server" CssClass="control-label required-field" Text="City/SD Charge"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <div class="input-group">
                            <asp:TextBox ID="txtCalculateCityCharge" runat="server" TabIndex="22" CssClass="form-control"
                                Enabled="false"></asp:TextBox>
                            <span class="input-group-addon">
                                <asp:CheckBox ID="cbCalculateCityCharge" runat="server" Text="" CssClass="customChkBox"
                                    onclick="javascript: return CalculateRateInclusively(this);"
                                    TabIndex="8" Checked="True" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div id="RRCvatChargeDiv">
                    <div class="col-md-2" style="text-align: right;">
                        <asp:Label ID="Label2" runat="server" CssClass="control-label required-field" Text="Vat Amount"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <div class="input-group">
                            <asp:TextBox ID="txtCalculateVatCharge" runat="server" TabIndex="23" CssClass="form-control"
                                Enabled="false"></asp:TextBox>
                            <span class="input-group-addon">
                                <asp:CheckBox ID="cbCalculateVatCharge" runat="server" Text="" CssClass="customChkBox"
                                    onclick="javascript: return CalculateRateInclusively(this);"
                                    TabIndex="8" Checked="True" />
                            </span>
                        </div>
                    </div>
                </div>
                <div id="RRCadditionalChargeDiv">
                    <div class="col-md-2" style="text-align: right;">
                        <asp:Label ID="Label4" runat="server" CssClass="control-label required-field" Text="Additional Charge"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <div class="input-group">
                            <asp:TextBox ID="txtCalculateAdditionalCharge" runat="server" TabIndex="22" CssClass="form-control"
                                Enabled="false"></asp:TextBox>
                            <span class="input-group-addon">
                                <asp:CheckBox ID="cbCalculateAdditionalCharge" runat="server" Text="" CssClass="customChkBox"
                                    onclick="javascript: return CalculateRateInclusively(this);"
                                    TabIndex="8" Checked="True" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group" id="discountAmountDiv">
                <%--<label for="DiscountType" class="control-label col-md-2">
                    Discount Type</label>
                <div class="col-md-4">
                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control" TabIndex="20">
                        <asp:ListItem>Fixed</asp:ListItem>
                        <asp:ListItem>Percentage</asp:ListItem>
                    </asp:DropDownList>
                </div>--%>
                <label for="DiscountAmount" class="control-label col-md-2">
                    Discount Amount</label>
                <div class="col-md-4">
                    <asp:TextBox ID="txtCalculateDiscountAmount" runat="server" CssClass="form-control quantitydecimal" Enabled="false" TabIndex="21"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-9"></div>
                <div class="col-md-2">
                    <input type="button" id="btnCalculatedDiscountOkey" value="Apply Rate"
                        class="col-sm-12 btn btn-primary btn-large" onclick="ApplyCalculateCharges()" />
                </div>
            </div>
        </div>
    </div>
    <!--End Calculate Rack Rate Inclusively PopUp -->

    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
        }
        else {
            MessagePanelHide();
        }

        var isIntegrated = '<%=isIntegratedGeneralLedgerDiv%>';
        if (isIntegrated > -1) {
            IntegratedGeneralLedgerDivPanelShow();
        }
        else { IntegratedGeneralLedgerDivPanelHide(); }

        var aireportPickupInformationPanelEnable = '<%=aireportPickupInformationPanelEnable%>';
        if (aireportPickupInformationPanelEnable > -1) { AireportPickupInformationPanelVisibleTrue(); }
        else { AireportPickupInformationPanelVisibleFalse(); }

        var isRoomAvailableForRegistrationEnable = '<%=IsRoomAvailableForRegistrationEnable%>';
        if (isRoomAvailableForRegistrationEnable > -1) {
            IsRoomAvailableForRegistrationShow();
        }

        $(document).ready(function () {
            if ($("#<%=ddlCompanyName.ClientID %>").val() == "0") {
                $('#txtCompany').val("");
            }
            else {
                $('#txtCompany').val($("#<%=ddlCompanyName.ClientID %> option:selected").text());
            }
            if ($("#<%=btnSave.ClientID %>").val() != 'Check-In') {
                $($("#myTabs").find("li")[4]).hide();
            }

            if ($("#ContentPlaceHolder1_ddlReservationGuest option:selected").text() == "Loading...") {
                $("#divReservationGuest").hide();
            }
        });

        //RRC show/hide
        <%--//var IsServiceChargeEnableConfig = '<%=IsServiceChargeEnableConfig%>';--%>
        if ($('#<%=hfIsServiceChargeEnableConfig.ClientID%>').val() == "0") {
            $('#ServiceChargeLabel').hide();
            $('#ServiceChargeControl').hide();
            $('#RRCserviceChargeDiv').hide();
        }
        else {
            $('#ServiceChargeLabel').show();
            $('#ServiceChargeControl').show();
            $('#RRCserviceChargeDiv').show();
        }

        <%--//var IsCitySDChargeEnableConfig = '<%=IsCitySDChargeEnableConfig%>';--%>
        if ($('#<%=hfIsCitySDChargeEnableConfig.ClientID%>').val() == "0") {
            $('#CityChargeLabel').hide();
            $('#CityChargeControl').hide();
            $('#RRCcitySdChargeDiv').hide();

        }
        else {
            $('#CityChargeLabel').show();
            $('#CityChargeControl').show();
            $('#RRCcitySdChargeDiv').show();
        }

       <%-- //var IsVatEnableConfig = '<%=IsVatEnableConfig%>';--%>
        if ($('#<%=hfIsVatEnableConfig.ClientID%>').val() == "0") {
            $('#VatAmountLabel').hide();
            $('#VatAmountControl').hide();
            $('#RRCvatChargeDiv').hide();
        }
        else {
            $('#VatAmountLabel').show();
            $('#VatAmountControl').show();
            $('#RRCvatChargeDiv').show();
        }

        <%--//var IsAdditionalChargeEnableConfig = '<%=IsAdditionalChargeEnableConfig%>';--%>
        if ($('#<%=hfIsAdditionalChargeEnableConfig.ClientID%>').val() == "0") {
            $('#AdditionalChargeLabel').hide();
            $('#AdditionalChargeControl').hide();
            $('#RRCadditionalChargeDiv').hide();
        }
        else {
            $('#AdditionalChargeLabel').show();
            $('#AdditionalChargeControl').show();
            $('#RRCadditionalChargeDiv').show();
        }

        if ($("#<%=ddlPaymentMode.ClientID %>").val() != "Company") {
            $("#<%=ddlPayFor.ClientID %>").val('0');
            $("#<%=ddlPayFor.ClientID %>").attr("disabled", true);
        }
        else {
            $("#<%=ddlPayFor.ClientID %>").attr("disabled", false);
        }
    </script>
</asp:Content>
