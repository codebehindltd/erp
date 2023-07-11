<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Common/Innboard.Master"
    AutoEventWireup="true" CodeBehind="frmRoomReservationNew.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmRoomReservationNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var minCheckInDate = "", editedRowIndex = "";
        var minCheckOutDate = "";
        var alreadySavePaidServices = [];
        var deletedRoomByType = new Array();
        var editedRow = "";
        var cvt = "";
        var hfCurrentReservationGuestId = "";
        var SelectdPreferenceId = "";
        var DeletedAirportPickupDrop = new Array();
        var RoomTypeList = new Array();
        var TotalRoomRateGlobalValue = 0;
        var MandaoryFieldsList = "";
        var currentRoomReservationNumber = "";

        $(document).ready(function () {

            if ($("#ContentPlaceHolder1_hfMandatoryFields").val() != "") {
                // debugger;
                MandaoryFieldsList = JSON.parse($("#ContentPlaceHolder1_hfMandatoryFields").val());
                for (var key in MandaoryFieldsList) {
                    //$("#ContentPlaceHolder1_" + MandaoryFieldsList[key].FieldId).prev().addClass('required-field');
                    var id = MandaoryFieldsList[key].FieldId;
                    if (!($(id).length)) {
                        id = "#ContentPlaceHolder1_" + id;
                    }
                    if (id == "#ContentPlaceHolder1_txtMobileNumber" || id == "#ContentPlaceHolder1_ddlMarketSegment" || id == "#ContentPlaceHolder1_txtBookersName" || id == "#ContentPlaceHolder1_ddlGuestSource" || id == "#ContentPlaceHolder1_ddlReferenceId") {
                        var tr = $(id).parent().parent();
                        $(tr).find("label").addClass("required-field");
                        if (id == "#ContentPlaceHolder1_ddlMarketSegment") {
                            $('#lblGuestSource').removeClass("required-field");
                        }
                        else if (id == "#ContentPlaceHolder1_txtMobileNumber") {
                            $('#lblEmailAddress').removeClass("required-field");
                        }
                    }

                }
            }

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfRoomType").val() != "") {
                RoomTypeList = JSON.parse($("#ContentPlaceHolder1_hfRoomType").val());
            }

            $("#chkYesBlock").bind("click", false);

            var strDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtDateIn").val(), '/');
            minCheckOutDate = GetStringFromDateTime(CommonHelper.DaysAdd(strDate, 1));

            $("#ContentPlaceHolder1_txtDateOut").datepicker("option", {
                minDate: minCheckOutDate
            });            

            $("#ContentPlaceHolder1_ddlGroupName").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSrcReferenceId").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlArrivalFlightName").select2({
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

            $("#ContentPlaceHolder1_ddlReferenceId").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlTitle").select({
                tags: "true",
                //placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlGuestTitle").select({
                tags: "true",
                //placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });



            if ($("#ContentPlaceHolder1_ddlClassification").val() == "453") {
                $("#VIPTypeDiv").show();
            }
            $("#<%=ddlClassification.ClientID %>").change(function () {
                if ($("#ContentPlaceHolder1_ddlClassification").val() == "453") {
                    $("#VIPTypeDiv").show();
                }
                else {
                    $("#VIPTypeDiv").hide();
                }
            });

            $("#ContentPlaceHolder1_txtPaxQuantity").change(function () {

                if ($(this).val() == "" || $(this).val() == "0") { return false; }

                var roomQuantity = $("#ContentPlaceHolder1_txtRoomId").val();
                if (roomQuantity == "" || roomQuantity == "0") { return false; }

                var roomType = _.findWhere(RoomTypeList, { RoomTypeId: parseInt($("#ContentPlaceHolder1_ddlRoomTypeId").val()) });
                var pax = parseInt($(this).val(), 10);

                if (roomType != null) {
                    if (pax > (roomType.PaxQuantity * parseInt(roomQuantity, 10))) {
                        toastr.warning("Pax Quantity Can Given For " + roomType.RoomType + " Room, Maximum " + (roomType.PaxQuantity * parseInt(roomQuantity, 10)));
                        $(this).val("");
                        $(this).focus();
                    }
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

            CommonHelper.ApplyDecimalValidation();
            var hfCurrentReservationGuestId = $("#<%=hfCurrentReservationGuestId.ClientID %>").val();

            minCheckInDate = $("#<%=hfMinCheckInDate.ClientID %>").val();
            var vvc = [];

            $("#myTabs").tabs();

            $($("#myTabs").find("li")[1]).hide();

            if ($("#ContentPlaceHolder1_hfReservationId").val() != "") {
                $("#ReservationRoomGrid tbody").append($("#ContentPlaceHolder1_RoomDetailsTemp").text());
                $("#PickupTable tbody").append($("#ContentPlaceHolder1_AirportPickupDetails").text());
                $("#DepartureTable tbody").append($("#ContentPlaceHolder1_AirportDropDetails").text());
                $('#MultiplePickup').show();
                $('#MultipleGuest').show();
                $($("#myTabs").find("li")[1]).show();
                GetTempRegistrationDetailByWM();
                var resId = $("#ContentPlaceHolder1_hfReservationId").val();
                PageMethods.LoadRoomNumbers(resId, OnLoadRoomNumbersSucceeded, OnLoadRoomNumbersFailed);
            }

            //multiple airport pickup drop 
            var ctrl = '#<%=chkMultiplePickup.ClientID%>'
            if ($(ctrl).is(':checked')) {
                $("#MultiplePickup").show();
                $("#MultipleGuest").show();
                $("#addMultiplePickup").show();
                var hfReservationId = $("#<%=hfReservationId.ClientID %>").val();
                var reservationId = parseInt(hfReservationId);
                PageMethods.LoadMultipleGuest(reservationId, OnLoadMultipleGuestSucceeded, OnLoadMultipleGuestFailed);
            }
            else {
                $("#MultiplePickup").hide();
                $("#MultipleGuest").hide();
                $("#addMultiplePickup").hide();
            }

            var ctrl = '#<%=chkMultipleDrop.ClientID%>'
            if ($(ctrl).is(':checked')) {
                $("#MultipleDrop").show();
                $("#MultipleDropGuest").show();
                $("#addMultipleDrop").show();
                var hfReservationId = $("#<%=hfReservationId.ClientID %>").val();
                var reservationId = parseInt(hfReservationId);
                PageMethods.LoadMultipleGuest(reservationId, OnLoadMultipleGuestSucceeded, OnLoadMultipleGuestFailed);
            }
            else {
                $("#MultipleDrop").hide();
                $("#MultipleDropGuest").hide();
                $("#addMultipleDrop").hide();
            }

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Reservation</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#ContentPlaceHolder1_hfInclusiveHotelManagementBill").val() == "1") {
                $("#btnRoomRateCalculation").hide();
            }

            $("#ContentPlaceHolder1_ddlGuestTitle").blur(function () {
                var title = $("#<%=ddlGuestTitle.ClientID %>").val();
                var titleText = $("#<%=ddlGuestTitle.ClientID %> option:selected").text();
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
                var firstName = $("#<%=txtGuestFirstName.ClientID %>").val();
                var lastName = $("#<%=txtGuestLastName.ClientID %>").val();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);
                }
                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            $("#ContentPlaceHolder1_txtGuestFirstName").blur(function () {
                var title = $("#<%=ddlGuestTitle.ClientID %>").val();
                var titleText = $("#<%=ddlGuestTitle.ClientID %> option:selected").text();
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
                var firstName = $("#<%=txtGuestFirstName.ClientID %>").val();
                var lastName = $("#<%=txtGuestLastName.ClientID %>").val();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);
                }
                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            $("#ContentPlaceHolder1_txtGuestLastName").blur(function () {
                var title = $("#<%=ddlGuestTitle.ClientID %>").val();
                var titleText = $("#<%=ddlGuestTitle.ClientID %> option:selected").text();
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
                var firstName = $("#<%=txtGuestFirstName.ClientID %>").val();
                var lastName = $("#<%=txtGuestLastName.ClientID %>").val();
                if (title != "0") {
                    $("#<%=txtGuestName.ClientID %>").val(titleText + " " + firstName + " " + lastName);
                }
                else $("#<%=txtGuestName.ClientID %>").val(firstName + " " + lastName);
            });

            $("#<%=ddlRoomTypeId.ClientID %>").change(function () {
                RoomDetailsByRoomTypeId($(this).val());
                //$('#<%=txtDiscountAmount.ClientID%>').val("");
                PerformFillFormActionByTypeId($('#<%=ddlRoomTypeId.ClientID%>').val());
                UpdateTotalCostWithDiscount();
                ClearRoomNumberAndId();
            });
            $("#<%=ddlGuestTitle.ClientID %>").change(function () {
                AutoGuestGenderLoadInfo();
            });
            $("#<%=ddlTitle.ClientID %>").change(function () {
                AutoGenderLoadInfo();
            });

            VisibleListedCompany();
            ToggleListedCompanyInfo();
            ToggleListedContactInfo();

            $('#ContentPlaceHolder1_A').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_B').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_C').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_D').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_E').click(function () {
                $('#SubmitButtonDiv').hide();
            });

            $('#ContentPlaceHolder1_txtProbableArrivalTime').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtProbableDepartureTime').timepicker({
                showPeriod: is12HourFormat
            });

            $('#ContentPlaceHolder1_txtProbableArrivalPendingTime').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtArrivalTime').timepicker({
                showPeriod: is12HourFormat
            });
            $('#ContentPlaceHolder1_txtDepartureTime').timepicker({
                showPeriod: is12HourFormat
            });

            $("#<%=txtRoomId.ClientID %>").click(function () {
                var txtFromDate = $("#<%=txtDateIn.ClientID %>").val();
                var txtToDate = $("#<%=txtDateOut.ClientID %>").val();

                if (txtFromDate == "") {
                    toastr.warning("Check In Date should not be empty.");
                    document.getElementById("<%=txtDateIn.ClientID%>").focus();
                    return false;
                }
                if (txtToDate == "") {
                    toastr.warning("Check Out Date should not be empty.");
                    document.getElementById("<%=txtToDate.ClientID%>").focus();
                    return false;
                }
                $("#<%=DateInHiddenField.ClientID %>").val(txtFromDate);
                $("#<%=DateOutHiddenField.ClientID %>").val(txtToDate);
                $("#<%=hfCurrencyHiddenField.ClientID %>").val($("#ContentPlaceHolder1_ddlCurrency").val());
            });

            $("#txtReservationDuration").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    return false;
                }
            });

            $("#ContentPlaceHolder1_txtReservationDuration").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    return false;
                }
            });

            $("#ContentPlaceHolder1_txtRoomId").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    return false;
                }
            });

            $("#ContentPlaceHolder1_txtPaxQuantity").keypress(function (e) {
                //if the letter is not digit then display error and don't type anything
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    toastr.warning("Digits Only");
                    return false;
                }
            });

            var ddlCurrencyId = $("#<%=ddlCurrency.ClientID %>").val();
            PageMethods.LoadCurrencyType(ddlCurrencyId, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);

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
            function AutoGenderLoadInfo() {
                var titleSex = $('#<%=ddlTitle.ClientID%>').val();
                //var titleGuestSex = $('#<%=ddlGuestTitle.ClientID%>').val();

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
            function AutoGuestGenderLoadInfo() {
                var titleGuestSex = $('#<%=ddlGuestTitle.ClientID%>').val();
                if (titleGuestSex == "Mr.") {
                    $('#<%=ddlGuestSex.ClientID%>').val("Male");
                }
                else if (titleGuestSex == "MrNMrs.") {
                    $('#<%=ddlGuestSex.ClientID%>').val("0");
                }
                else if (titleGuestSex == "Dr.") {
                    $('#<%=ddlGuestSex.ClientID%>').val("0");
                }
                else if (titleGuestSex == "N/A") {
                    $('#<%=ddlGuestSex.ClientID%>').val("0");
                }
                else if (titleGuestSex == "0") {
                    $('#<%=ddlGuestSex.ClientID%>').val("0");
                }
                else {
                    $('#<%=ddlGuestSex.ClientID%>').val("Female");
                }
            }

            function OnLoadConversionRateSucceeded(result) {
                if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                    //$("#<%=txtConversionRate.ClientID %>").val('');                    
                }
                else {
                    $("#<%=txtConversionRate.ClientID %>").val(result.BillingConversionRate);
                }

                RoomDetailsByRoomTypeId($('#<%=ddlRoomTypeId.ClientID%>').val());
                CurrencyRateInfoEnable();
            }

            function OnLoadConversionRateFailed() {
            }

            $("#<%=ddlDiscountType.ClientID %>").change(function () {
                var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtUnitPrice"), "Discount Amount");
                if (checkValue == false) {
                    return false;
                }
                UpdateDiscountAmount();
                TotalRoomRateVatServiceChargeCalculation();
            });
            $("#<%=txtDiscountAmount.ClientID %>").blur(function () {
                var checkValue = CheckDiscountValidationForFixedRPercentage($("#ContentPlaceHolder1_ddlDiscountType"), $("#ContentPlaceHolder1_txtDiscountAmount"), $("#ContentPlaceHolder1_txtUnitPrice"), "Discount Amount");
                if (checkValue == false) {
                    return false;
                }
                //UpdateTotalCostWithDiscount();
                UpdateDiscountAmount();
                TotalRoomRateVatServiceChargeCalculation();
            });

            $("#<%=txtRoomRate.ClientID %>").blur(function () {
                var discountAcount = $("#ContentPlaceHolder1_txtDiscountAmount").val();
                if (discountAcount < 0) {
                    toastr.warning("Negotiated Rate Can Not Greater Than Rack Rate.");
                    $("#<%=txtRoomRate.ClientID %>").val($("#ContentPlaceHolder1_txtUnitPriceHiddenField").val());
                    $("#ContentPlaceHolder1_txtDiscountAmount").val("0");
                    $("#<%=txtRoomRate.ClientID %>").focus();
                    TotalRoomRateVatServiceChargeCalculation();
                    return false;

                }
                TotalRoomRateVatServiceChargeCalculation();
            });

            $("#<%=ddlCompanyName.ClientID %>").change(function () {
                GetTotalCostWithCompanyOrPersonalDiscount();
                DiscountPolicyByCompanyNRoomType();
            });

            $("#txtCompany").blur(function () {
                GetTotalCostWithCompanyOrPersonalDiscount();
                DiscountPolicyByCompanyNRoomType();
                var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
                if ($(ctrl).is(':checked')) {
                    var companyId = $("#<%=ddlCompanyName.ClientID %>").val();
                    if (companyId = 0) {
                        toastr.warning("Please select an enlisted company.");
                        return false;
                    }
                }
            });

            $("#<%=ddlBusinessPromotionId.ClientID %>").change(function () {
                GetTotalCostWithCompanyOrPersonalDiscount();
            });

            CommonHelper.AutoSearchClientDataSource("txtGuestCountrySearch", "ContentPlaceHolder1_ddlGuestCountry", "ContentPlaceHolder1_ddlGuestCountry");
            CommonHelper.AutoSearchClientDataSource("txtCountry", "ContentPlaceHolder1_ddlCountry", "ContentPlaceHolder1_ddlCountry");
            CommonHelper.AutoSearchClientDataSource("txtPrevGstCountry", "ContentPlaceHolder1_ddlPrevGstCountry", "ContentPlaceHolder1_ddlPrevGstCountry");
            CommonHelper.AutoSearchClientDataSource("txtCompany", "ContentPlaceHolder1_ddlCompanyName", "ContentPlaceHolder1_ddlCompanyName");

            $("#txtGuestCountrySearch").blur(function () {
                var countryId = $("#<%=ddlGuestCountry.ClientID %>").val();
                $("#<%=ddlCountry.ClientID %>").val(countryId);
                $("#txtCountry").val($(this).val());
                PageMethods.GetNationality(countryId, OnCountrySucceeded, OnCountryFailed);
            });
            $("#txtCountry").blur(function () {
                var countryId = $("#<%=ddlCountry.ClientID %>").val();
                $("#<%=ddlGuestCountry.ClientID %>").val(countryId);
                PageMethods.GetNationality(countryId, OnCountrySucceeded, OnCountryFailed);
            });

            $("#ContentPlaceHolder1_txtListedContactPerson").autocomplete({

                source: function (request, response) {
                    let url = 'frmRoomReservationNew.aspx/GetContactInformationByCompanyIdNSearchText';
                    let companyId = $("#ContentPlaceHolder1_ddlCompanyName").val();
                    let searchText = $("#ContentPlaceHolder1_txtListedContactPerson").val();

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
                                    PersonalAddress: m.WorkAddress,
                                    MobilePersonal: m.ContactNo,
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
                    $("#<%=txtContactAddress.ClientID %>").val(ui.item.PersonalAddress);
                    $("#<%=txtContactPerson.ClientID %>").val(ui.item.Name);
                    $("#<%=txtMobileNumber.ClientID %>").val(ui.item.MobilePersonal);
                    $("#<%=txtContactEmail.ClientID %>").val(ui.item.Email);
                }
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

            if ($("#<%=ddlReservationStatus.ClientID %>").val() == "Pending") {
                $('#PendingDiv').show("slow");
                $('#ReasonDiv').hide("slow");
            }

            else if ($("#<%=ddlReservationStatus.ClientID %>").val() == "Cancel") {
                $('#ReasonDiv').show("slow");
                $('#PendingDiv').hide("slow");
            }
            else {
                $('#PendingDiv').hide("slow");
                $('#ReasonDiv').hide("slow");
            }

            var txtStartDate = '<%=txtDateIn.ClientID%>'
            var txtEndDate = '<%=txtDateOut.ClientID%>'

            $("#ContentPlaceHolder1_txtDateIn").datepicker({
                //defaultDate: "+1w",
                changeMonth: true,
                changeYear: true,
                minDate: minCheckInDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtDateOut").datepicker("option", "minDate", selectedDate);

                    var strDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtDateIn").val(), '/');
                    minCheckOutDate = GetStringFromDateTime(CommonHelper.DaysAdd(strDate, 1));

                    $("#ContentPlaceHolder1_txtDateOut").datepicker("option", {
                        minDate: minCheckOutDate
                    });

                    if ($("#ContentPlaceHolder1_txtDateOut").val() != "")
                        $("#txtReservationDuration").val(CommonHelper.DateDifferenceInDays($("#ContentPlaceHolder1_txtDateIn").val(), $("#ContentPlaceHolder1_txtDateOut").val()));
                    else
                        $("#txtReservationDuration").val("");
                }
            });

            $("#ContentPlaceHolder1_txtDateOut").datepicker({
                changeMonth: true,
                changeYear: true,
                minDate: minCheckOutDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtDateIn").datepicker("option", "maxDate", selectedDate);

                    if ($("#ContentPlaceHolder1_txtDateOut").val() != "")
                        $("#txtReservationDuration").val(CommonHelper.DateDifferenceInDays($("#ContentPlaceHolder1_txtDateIn").val(), $("#ContentPlaceHolder1_txtDateOut").val()));
                    else
                        $("#txtReservationDuration").val("");
                }
            });

            $("#txtReservationDuration").blur(function () {
                if ($(this).val() == "0") {
                    $("#txtReservationDuration").val("");
                    toastr.info("Please Enter Valid Number.");

                    return;
                }
                if (CommonHelper.IsInt($(this).val()) == false) {
                    toastr.info("Please Enter Valid Number.");
                    return;
                }

                if ($.trim($(this).val()) != "") {
                    var strDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtDateIn").val(), '/');
                    $("#ContentPlaceHolder1_txtDateOut").val(GetStringFromDateTime(CommonHelper.DaysAdd(strDate, $(this).val())));
                }
                else {
                    $(this).val("");
                }
            });

            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtFromDate").datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtDOB").datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: minCheckInDate,
                dateFormat: innBoarDateFormat,
                yearRange: "-100:+0",
            });

            $("#ContentPlaceHolder1_txtGuestDOB").datepicker({
                changeMonth: true,
                changeYear: true,
                maxDate: minCheckInDate,
                dateFormat: innBoarDateFormat,
                yearRange: "-100:+0",
            });

            $("#ContentPlaceHolder1_txtVIssueDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtVExpireDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtVExpireDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtVIssueDate").datepicker("option", "maxDate", selectedDate);
                }

            });

            $("#ContentPlaceHolder1_txtPIssueDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtPExpireDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtPExpireDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtPIssueDate").datepicker("option", "maxDate", selectedDate);
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
            $("#ContentPlaceHolder1_txtNoShowCharge").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtConfirmationDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#<%=ddlReservationStatus.ClientID %>").change(function () {

                if ($(this).val() == "Active") {
                    $('#PendingDiv').hide("slow");
                    $('#ReasonDiv').hide("slow");

                    $("#ContentPlaceHolder1_txtConfirmationDate").val("");
                    $("#ContentPlaceHolder1_txtProbableArrivalPendingTime").val("12:00");
                    $("#ContentPlaceHolder1_txtReason").val("");
                }
                else if ($("#<%=ddlReservationStatus.ClientID %>").val() == "Pending") {
                    $('#PendingDiv').show("slow");
                    $('#ReasonDiv').hide("slow");

                    $("#ContentPlaceHolder1_txtConfirmationDate").val("");
                    $("#ContentPlaceHolder1_txtProbableArrivalPendingTime").val("12:00");
                    $("#ContentPlaceHolder1_txtReason").val("");

                }
                else if ($("#<%=ddlReservationStatus.ClientID %>").val() == "NoShow") {
                    $('#PendingDiv').hide("slow");
                    $('#ReasonDiv').hide("slow");

                    $("#ContentPlaceHolder1_txtConfirmationDate").val("");
                    $("#ContentPlaceHolder1_txtProbableArrivalPendingTime").val("12:00");
                    $("#ContentPlaceHolder1_txtReason").val("");
                }
            });

            // //-- Airport Pick Up Information Div ------------------
            if (($("#<%=ddlAirportPickUp.ClientID %>").val() == "0") || ($("#<%=ddlAirportPickUp.ClientID %>").val() == "NO")) {
                $('#AirportPickUpInformationDiv').hide();
                $('#AirportPickUpInformationDiv').hide();
                $("#ContentPlaceHolder1_ddlArrivalFlightName").val("0");
                $("#<%=txtArrivalFlightNumber.ClientID %>").val("");
            }
            else {
                $('#AirportPickUpInformationDiv').show();
                if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "TBA") {
                    $('#lblAirlineNameMandatory').hide();
                    $('#lblAirlineNameNoMandatory').show();

                    $('#lblFlightNumberMandatory').hide();
                    $('#lblFlightNumberNoMandatory').show();
                }
                else {
                    $('#lblAirlineNameMandatory').show();
                    $('#lblAirlineNameNoMandatory').hide();

                    $('#lblFlightNumberMandatory').show();
                    $('#lblFlightNumberNoMandatory').hide();
                }
            }
            $("#<%=ddlAirportPickUp.ClientID %>").change(function () {
                var pickUp = $("#<%=ddlAirportPickUp.ClientID %>").val();
                if ((pickUp == "NO") || (pickUp == "0")) {
                    $('#AirportPickUpInformationDiv').hide();
                    $("#ContentPlaceHolder1_ddlArrivalFlightName").val("0");
                    $("#<%=txtArrivalFlightNumber.ClientID %>").val("");
                    $('#MultiplePickup').hide();
                    $('#MultipleGuest').hide();
                    $('#addMultiplePickup').hide();
                    $("#ContentPlaceHolder1_chkMultiplePickup").attr("checked", false);
                    $("#PickupTable tbody").append("");
                }
                else {
                    $('#AirportPickUpInformationDiv').show();
                    var rowLength = $("#ReservedGuestInformation tbody tr").length;
                    if (rowLength > 1) {
                        $('#MultiplePickup').show();
                    }

                    if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "TBA") {
                        $('#lblAirlineNameMandatory').hide();
                        $('#lblAirlineNameNoMandatory').show();

                        $('#lblFlightNumberMandatory').hide();
                        $('#lblFlightNumberNoMandatory').show();
                    }
                    else {
                        $('#lblAirlineNameMandatory').show();
                        $('#lblAirlineNameNoMandatory').hide();

                        $('#lblFlightNumberMandatory').show();
                        $('#lblFlightNumberNoMandatory').hide();
                    }
                }
            });

            // //-- Airport Drop Information Div ------------------
            if (($("#<%=ddlAirportDrop.ClientID %>").val() == "NO") || ($("#<%=ddlAirportDrop.ClientID %>").val() == "0")) {
                $('#AirportDropInformationDiv').hide();
                $("#<%=ddlDepartureFlightName.ClientID %>").val("0");
                $("#<%=txtDepartureFlightNumber.ClientID %>").val("");
            }
            else {
                $('#AirportDropInformationDiv').show();

                if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "TBA") {
                    $('#lblAirlineNameMandatory').hide();
                    $('#lblAirlineNameNoMandatory').show();

                    $('#lblFlightNumberMandatory').hide();
                    $('#lblFlightNumberNoMandatory').show();
                }
                else {
                    $('#lblAirlineNameMandatory').show();
                    $('#lblAirlineNameNoMandatory').hide();

                    $('#lblFlightNumberMandatory').show();
                    $('#lblFlightNumberNoMandatory').hide();
                }
            }
            $("#<%=ddlAirportDrop.ClientID %>").change(function () {
                var drop = $("#<%=ddlAirportDrop.ClientID %>").val();
                if ((drop == "NO") || (drop == "0")) {
                    $('#AirportDropInformationDiv').hide();
                    $("#<%=ddlDepartureFlightName.ClientID %>").val("0");
                    $("#<%=txtDepartureFlightNumber.ClientID %>").val("");
                    $('#MultipleDrop').hide();
                    $('#MultipleDropGuest').hide();
                    $('#addMultipleDrop').hide();
                    $("#ContentPlaceHolder1_chkMultipleDrop").attr("checked", false);
                    $("#DepartureTable tbody").append("");
                }
                else {
                    $('#AirportDropInformationDiv').show();
                    var rowLength = $("#ReservedGuestInformation tbody tr").length;
                    if (rowLength > 1) {
                        $('#MultipleDrop').show();
                    }

                    if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "TBA") {
                        $('#lblAirlineNameMandatory').hide();
                        $('#lblAirlineNameNoMandatory').show();

                        $('#lblFlightNumberMandatory').hide();
                        $('#lblFlightNumberNoMandatory').show();
                    }
                    else {
                        $('#lblAirlineNameMandatory').show();
                        $('#lblAirlineNameNoMandatory').hide();

                        $('#lblFlightNumberMandatory').show();
                        $('#lblFlightNumberNoMandatory').hide();
                    }
                }
            });

            //EnableDisable For DropDown Change event--------------
            $('#' + '<%=ddlReservedMode.ClientID%>').change(function () {
                $("#ContentPlaceHolder1_txtContactPerson").val("");
                $("#ContentPlaceHolder1_txtContactAddress").val("");
                $("#ContentPlaceHolder1_txtMobileNumber").val("");
                $("#ContentPlaceHolder1_txtContactEmail").val("");
                $("#ContentPlaceHolder1_txtFaxNumber").val("");

                if ($('#' + '<%=ddlReservedMode.ClientID%>').val() == "Self") {
                    var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
                    $(ctrl).prop('checked', false)
                    ToggleListedCompanyInfo();
                    $('#GroupDiv').hide();
                    $('#PersonDiv').show();
                    $('#ContactPersonDiv').show();
                    $('#MobileNEmailDiv').show();
                    $('#CurrencyDiv').show();
                    $("#<%=ddlCurrency.ClientID %>").val("1");
                    $('#CurrencyAmountInformationDiv').hide();
                }
                else if ($('#' + '<%=ddlReservedMode.ClientID%>').val() == "Group") {
                    var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
                    $(ctrl).prop('checked', false);
                    ToggleListedCompanyInfo();
                    $('#GroupDiv').show();
                    $('#PersonDiv').show();
                    $('#ContactPersonDiv').show();
                    $('#MobileNEmailDiv').show();
                    $('#CurrencyDiv').show();
                    $("#<%=ddlCurrency.ClientID %>").val("1");
                    $('#CurrencyAmountInformationDiv').hide();
                }
                else if ($('#' + '<%=ddlReservedMode.ClientID%>').val() == "Company") {
                    $('#PersonDiv').hide();
                    $('#GroupDiv').hide();
                    $('#ContactPersonDiv').show();
                    $('#MobileNEmailDiv').show();
                    $('#CurrencyDiv').show();
                    $("#<%=ddlCurrency.ClientID %>").val("1");
                    $('#CurrencyAmountInformationDiv').hide();
                }
                else {
                    var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
                    $(ctrl).prop('checked', false);
                    ToggleListedCompanyInfo();
                    $('#GroupDiv').hide();
                    $('#PersonDiv').hide();
                    $('#ContactPersonDiv').hide();
                    $('#MobileNEmailDiv').hide();
                    $('#CurrencyDiv').hide();
                    $("#<%=ddlCurrency.ClientID %>").val("1");
                    $('#CurrencyAmountInformationDiv').hide();
                }
                VisibleListedCompany();
            });

            $("#btnAddDetailGuest").click(function () {
                if ($("#ContentPlaceHolder1_ddlRoomTypeId").val() == "0") {
                    toastr.warning("Please Select Room Type.");
                    return;
                }
                else if ($("#ContentPlaceHolder1_txtRoomId").val() == "") {
                    toastr.warning("Please Provide Room Quantity.");
                    return;
                }
                else if ($("#ContentPlaceHolder1_txtPaxQuantity").val() == "" || $("#ContentPlaceHolder1_txtPaxQuantity").val() == "0") {
                    toastr.warning("Please Provide Pax Quantity.");
                    return;
                }

                if ($("#ContentPlaceHolder1_txtDiscountAmount").val() != "") {
                    if (CommonHelper.IsDecimal($("#ContentPlaceHolder1_txtDiscountAmount").val()) == false) {
                        toastr.warning("Please Provide Valid Number in Discount Amount.");
                        $("#ContentPlaceHolder1_txtDiscountAmount").focus();
                        return;
                    }
                }

                if (CommonHelper.IsDecimal($("#ContentPlaceHolder1_txtRoomRate").val()) == false) {
                    toastr.warning("Please Provide Valid Number in Negotiated Rate.");
                    $("#ContentPlaceHolder1_txtRoomRate").focus();
                    return;
                }

                PerformRoomAvailableChecking();
            });

            $("#PopSearchPanel").hide();
            $("#PopTabPanel").hide();
            $("#ExtraSearch").hide();
            ClearRoomNumberAndId();
            $("#btnPopSearch").click(function () {
                $("#PopSearchPanel").show('slow');
                $("#PopTabPanel").hide('slow');
                GridPaging(1, 1, 1);
            });
            $("#btnSrchRsrvtn").click(function () {
                $("#SearchPanel").show('slow');
                GridPagingForSearchReservation(1, 1);
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

            });
            $("#btnAddPerson").click(function () {
                $("#PopSearchPanel").show('slow');
                $("#PopTabPanel").hide('slow');
                GridPaging(1, 1, 0);
                AddNewItem();
            });
            $("#btnSearchSuccess").click(function () {
                $("#<%=txtGuestName.ClientID %>").val($("#<%=hiddenGuestName.ClientID %>").val());
                $("#PopSearchPanel").hide();
                $("#PopTabPanel").hide();
                $("#TouchKeypad").dialog("close");
                LoadDataOnParentForm();
            });
            $("#btnSearchCancel").click(function () {
                $("#<%=hiddenGuestName.ClientID %>").val('');
                $("#<%=hiddenGuestId.ClientID %>").val('');
                $("#PopSearchPanel").hide();
                $("#PopTabPanel").hide();
                $("#TouchKeypad").dialog("close");
            });

            $("#btnPrintDocument").click(function () {
                $("#<%=hiddenGuestName.ClientID %>").val('');
                $("#PopSearchPanel").hide();
                $("#PopTabPanel").hide();
                $("#TouchKeypad").dialog("close");
                window.location.href = '/HotelManagement/Reports/frmReportPrintImage.aspx?GuestId='
                    + $("#<%=hiddenGuestId.ClientID %>").val();
            });

            $("#btnProfile").click(function () {
                if ($("#ReservedGuestInformation tbody tr").length == 0) {
                    var title = $("#<%=ddlTitle.ClientID %>").val();
                    var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();

                    if (title == "0") {
                        toastr.warning("Please Select Title.");
                        $("#<%=ddlTitle.ClientID %>").focus();
                        return;
                    }

                    //if (title == "MrNMrs.") {
                    //    title = "Mr. & Mrs.";
                    //}
                    if (title == "N/A") {
                        titleText = "";
                    }
                    //else {
                    //    title = titleText;
                    //}
                    var firstName = $("#<%=txtFirstName.ClientID %>").val().trim();
                    var lastName = $("#<%=txtLastName.ClientID %>").val().trim();
                    var country = $('#<%=ddlCountry.ClientID%>').val();

                    if (firstName == "") {
                        toastr.warning('Please fill mandatory fields.');
                        return;
                    }

                    <%--$('#<%=ddlGuestTitle.ClientID%>').val($('#<%=ddlTitle.ClientID%>').val());
                    if ($('#<%=ddlTitle.ClientID%>').val() == "Mr.") {
                        $('#<%=ddlGuestSex.ClientID%>').val("Male");
                    }
                    else if ($('#<%=ddlTitle.ClientID%>').val() == "MrNMrs.") {
                        $('#<%=ddlGuestSex.ClientID%>').val("0");
                    }
                    else if ($('#<%=ddlTitle.ClientID%>').val() == "Dr.") {
                        $('#<%=ddlGuestSex.ClientID%>').val("0");
                    }
                    else if ($('#<%=ddlTitle.ClientID%>').val() == "N/A") {
                        $('#<%=ddlGuestSex.ClientID%>').val("0");
                    }
                    else {
                        $('#<%=ddlGuestSex.ClientID%>').val("Female");
                    }--%>

                    $('#<%=ddlGuestTitle.ClientID%>').val($('#<%=ddlTitle.ClientID%>').val());

                    var titleSex = $('#<%=ddlGuestTitle.ClientID%>').val();
                    //toastr.info("Gender :" + titleSex);
                    if (titleSex == "Mr.") {
                        $('#<%=ddlGuestSex.ClientID%>').val("Male");
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

                    $("#<%=ddlGuestCountry.ClientID %>").val($('#<%=ddlCountry.ClientID%>').val());
                    var countryId = $("#<%=ddlGuestCountry.ClientID %>").val();
                    if (countryId > 0) {
                        PageMethods.GetNationality(countryId, OnCountrySucceeded, OnCountryFailed);
                    }
                    var name = '';
                    if (title != '0') {
                        name = titleText + " " + firstName + " " + lastName;
                    }
                    else {
                        name = firstName + " " + lastName;
                    }
                    var phone = $('#<%=txtPhone.ClientID%>').val();
                    var countryname = $("#txtCountry").val();
                    var email = $('#<%=txtEmail.ClientID%>').val();

                    $('#<%=txtGuestFirstName.ClientID%>').val(firstName);
                    $('#<%=txtGuestLastName.ClientID%>').val(lastName);
                    $('#<%=txtGuestName.ClientID%>').val(name);
                    $('#<%=txtGuestPhone.ClientID%>').val(phone);
                    $('#<%=ddlGuestCountry.ClientID%>').val(country);
                    $("#txtGuestCountrySearch").val(countryname);
                    $('#<%=txtGuestEmail.ClientID%>').val(email);
                    $("#ContentPlaceHolder1_hfProfileOrAddMore").val("Profile");
                }
                $($("#myTabs").find("li")[1]).show();
                $("#myTabs").tabs({ active: 1 });
            });

            $("#btnAddmore").click(function () {
                $("#GuestDetailsAdd").show();
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();
                var firstName = $("#<%=txtFirstName.ClientID %>").val().trim();
                var lastName = $("#<%=txtLastName.ClientID %>").val().trim();
                var txtGuestName = "";
                if (title == "N/A") {
                    txtGuestName = firstName + " " + lastName;
                    //title = "";
                }
                else {
                    txtGuestName = titleText + " " + firstName + " " + lastName;
                }

                var txtGuestEmail = $("#<%=txtEmail.ClientID %>").val();
                var txtGuestDOB = '';
                var hiddenGuestId = $("#<%=hiddenGuestId.ClientID %>").val();
                var txtGuestDrivinlgLicense = $("#<%=txtGuestDrivinlgLicense.ClientID %>").val();
                var txtGuestAddress1 = $("#<%=txtGuestAddress1.ClientID %>").val();
                var txtGuestAddress2 = $("#<%=txtGuestAddress2.ClientID %>").val();
                var ddlProfessionId = $("#<%=ddlProfessionId.ClientID %>").val();
                var txtGuestCity = $("#<%=txtGuestCity.ClientID %>").val();
                var ddlGuestCountry = $("#<%=ddlCountry.ClientID %>").val();
                var txtGuestNationality = $("#<%=txtGuestNationality.ClientID %>").val();
                var txtGuestCountrySearch = $.trim($("#txtGuestCountrySearch").val());
                var txtGuestPhone = $("#<%=txtPhone.ClientID %>").val();
                var ddlGuestSex = $("#<%=ddlGuestSex.ClientID %>").val();
                //toastr.info(ddlGuestSex);
                var txtGuestZipCode = $("#<%=txtGuestZipCode.ClientID %>").val();
                var txtNationalId = $("#<%=txtNationalId.ClientID %>").val();
                var txtPassportNumber = $("#<%=txtPassportNumber.ClientID %>").val();
                var txtPExpireDate = $("#<%=txtPExpireDate.ClientID %>").val();
                var txtPIssueDate = $("#<%=txtPIssueDate.ClientID %>").val();
                var txtPIssuePlace = '';
                var txtVExpireDate = $("#<%=txtVExpireDate.ClientID %>").val();
                var txtVisaNumber = $("#<%=txtVisaNumber.ClientID %>").val();
                var txtVIssueDate = $("#<%=txtVIssueDate.ClientID %>").val();
                var txtNumberOfPersonAdult = $("#<%=txtNumberOfPersonAdult.ClientID %>").val();
                //var ddlClassificationId = $("#<%=ddlClassificationId.ClientID %>").val();
                var ddlClassificationId = $("#<%=ddlClassification.ClientID %>").val();

                var roomId = $("#ContentPlaceHolder1_hfFstAsndRoomId").val();
                if (roomId == "") {
                    roomId = 0;
                }
                var prevGuestId = $("#<%=hfPrevGuestId.ClientID %>").val();
                if (prevGuestId == "") {
                    prevGuestId = 0;
                }

                var IsEditFromAddMore = 0;
                var hfCurrentResGuestId = hfCurrentReservationGuestId;
                if (prevGuestId == 0 && hfCurrentReservationGuestId != "") {
                    prevGuestId = hfCurrentResGuestId;
                    IsEditFromAddMore = 1;
                }

                var txtGuestCountrySearch = $.trim($("#txtGuestCountrySearch").val());
                if (title == "0") {
                    toastr.warning("Please Select Title.");
                    $("#ddlTitle").focus();
                    return;
                }
                //else {
                //    toastr.info(title);
                //}
                if (txtGuestEmail != "") {
                    var mailformat = new RegExp('^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$', 'i');
                    if (txtGuestEmail.match(mailformat)) {
                    }
                    else {
                        toastr.warning("You have entered an invalid email address!", "", { timeOut: 10000 });
                        $("#txtGuestEmail").focus();
                        return;
                    }
                }
                if (txtGuestPhone != "") {
                    var phnformat = /[1-9](?:\d{0,2})(?:,\d{3})*(?:\.\d*[1-9])?|0?\.\d*[1-9]|0/;
                    if (txtGuestPhone.match(phnformat)) {
                    }
                    else {
                        toastr.warning("You have entered an invalid Phone Number!");
                        return;
                    }
                }

                if (firstName == "") {
                    toastr.warning('Please provide First Name.');
                }
                else {
                    $($("#myTabs").find("li")[1]).show();
                    $("#myTabs").tabs({ active: 1 });

                    var isEdit = "";
                    if ($("#<%=EditId.ClientID %>").val() == "") {
                        isEdit = "";
                    }
                    else {
                        isEdit = $("#<%=EditId.ClientID %>").val();
                    }
                    $('#btnAddGuest').val('Add');

                    var hfReservationIdTemp = $("#<%=hfReservationIdTemp.ClientID %>").val();
                    var reservationId = parseInt(hfReservationIdTemp);

                    if ($.trim(roomId) == "")
                        roomId = "0";

                    $("#ContentPlaceHolder1_hfProfileOrAddMore").val("AddMore");

                    var additionalRemarks = "";
                    if (typeof $("#ContentPlaceHolder1_txtAdditionalRemarks").val() !== "undefined") {
                        additionalRemarks = $("#ContentPlaceHolder1_txtAdditionalRemarks").val();
                    }

                    PageMethods.SaveGuestInformationAsDetail(reservationId, prevGuestId, isEdit, IsEditFromAddMore, title, firstName, lastName, txtGuestName, txtGuestEmail, hiddenGuestId, txtGuestDrivinlgLicense, txtGuestDOB, txtGuestAddress1, txtGuestAddress2, ddlProfessionId, txtGuestCity, ddlGuestCountry, txtGuestNationality, txtGuestPhone, ddlGuestSex, txtGuestZipCode, txtNationalId, txtPassportNumber, txtPExpireDate, txtPIssueDate, txtPIssuePlace, txtVExpireDate, txtVisaNumber, txtVIssueDate, roomId, "", ddlClassificationId, additionalRemarks, OnLoadDetailGridInformationSucceeded, OnLoadDetailGridInformationFailed);
                    return false;
                }
            });

            $("#btnAddGuest").click(function () {
                PageMethods.LoadReservationNRegistrationXtraValidation(OnLoadXtraValidationSucceeded, OnLoadXtraValidationFailed);
            });

            $("#btnaddMultiplePickup").click(function () {
                var guestId = $("#<%=ddlGuest.ClientID %>").val();
                var guest = $("#ContentPlaceHolder1_ddlGuest").find(":selected").text();
                var airline = $("#ContentPlaceHolder1_ddlArrivalFlightName").find(":selected").text();
                var flightNo = $("#<%=txtArrivalFlightNumber.ClientID %>").val();
                var arrivetime = $("#<%=txtArrivalTime.ClientID %>").val();

                if (guestId == 0) {
                    toastr.warning("Please Select a guest.");
                    return false;
                }

                var rowLength = $("#PickupTable tbody tr").length;
                var tr = "";

                if (rowLength % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='display:none'>" + 0 + "</td>";
                tr += "<td style='display:none'>" + 0 + "</td>";
                tr += "<td style='display:none'>" + guestId + "</td>";
                tr += "<td style='width:25%;'>" + guest + "</td>";
                tr += "<td style='width:25%;'>" + airline + "</td>";
                tr += "<td style='width:20%;'>" + flightNo + "</td>";
                tr += "<td style='width:20%;'>" + arrivetime + "</td>";
                tr += "<td style='width:10%;'> <a href='#' onclick= 'DeletePickup(this)' ><img alt='Delete' src='../Images/delete.png' /></a></td>"

                tr += "</tr>";

                $("#PickupTable tbody").append(tr);

                tr = "";

                $("#<%=txtArrivalFlightNumber.ClientID %>").val("");
                $("#<%=txtArrivalTime.ClientID %>").val("");
            });
            $("#btnaddMultipleDrop").click(function () {
                var guestId = $("#<%=ddlDropGuest.ClientID %>").val();
                var guest = $("#ContentPlaceHolder1_ddlDropGuest").find(":selected").text();
                var airline = $("#ContentPlaceHolder1_ddlDepartureFlightName").find(":selected").text();
                var flightNo = $("#<%=txtDepartureFlightNumber.ClientID %>").val();
                var arrivetime = $("#<%=txtDepartureTime.ClientID %>").val();

                if (guestId == 0) {
                    toastr.warning("Please Select a guest.");
                    return false;
                }

                var rowLength = $("#DepartureTable tbody tr").length;

                var tr = "";

                if (rowLength % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='display:none'>" + 0 + "</td>";
                tr += "<td style='display:none'>" + 0 + "</td>";
                tr += "<td style='display:none'>" + guestId + "</td>";
                tr += "<td style='width:25%;'>" + guest + "</td>";
                tr += "<td style='width:25%;'>" + airline + "</td>";
                tr += "<td style='width:20%;'>" + flightNo + "</td>";
                tr += "<td style='width:20%;'>" + arrivetime + "</td>";
                tr += "<td style='width:10%;'> <a href='#' onclick= 'DeletePickup(this)' ><img alt='Delete' src='../Images/delete.png' /></a></td>"

                tr += "</tr>";

                $("#DepartureTable tbody").append(tr);

                tr = "";

                $("#<%=txtDepartureFlightNumber.ClientID %>").val("");
                $("#<%=txtDepartureTime.ClientID %>").val("12:00");
            });

            $("#tblRoomReserve").delegate("td > img.RoomreservationDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var reservationId = $.trim($(this).parent().parent().find("td:eq(8)").text());
                    var params = JSON.stringify({ pkId: reservationId });
                    var $row = $(this).parent().parent();

                    $.ajax({
                        type: "POST",
                        url: "/HotelManagement/frmRoomReservation.aspx/DeleteReservationRecord",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            CommonHelper.AlertMessage(data.d.AlertMessage);
                            $row.remove();
                            $("#myTabs").tabs('load', 5);
                        },
                        error: function (error) {
                        }
                    });
                }
            });

            $("#btnItemwiseSpecialRemarksCancel").click(function () {
                $("#PaidServiceDialog").dialog("close");
            });

            $("#btnItemwiseSpecialRemarksOk").click(function () {
                CalculateServiceBill();
            });

            $("#ContentPlaceHolder1_cbReservationDuration").click(function () {
                if ($(this).is(':checked') == true) {
                    $("#txtReservationDuration").attr('readonly', false);
                }
                else {
                    $("#txtReservationDuration").attr('readonly', true);
                }
            });

            $("#ReservationRoomGrid").delegate("td > img.deleteroom", "click", function () {
                var answer = confirm("Do you want to delete this room?")
                if (answer) {
                    var row = $(this).parent().parent();
                    var roomTypeId = $.trim($(row).find("td:eq(4)").text());
                    var reservationDetailsId = $.trim($(row).find("td:eq(3)").text());

                    if (reservationDetailsId != "" && reservationDetailsId != "0") {
                        deletedRoomByType.push({
                            RoomTypeId: roomTypeId
                        });
                    }

                    $(row).remove();
                }
            });

            $("#ReservationRoomGrid").delegate("td > img.editroom", "click", function () {
                var row = $(this).parent().parent();
                editedRow = row;

                var roomType = "", roomTypeId = "", roomIds = "", roomNumbers = "", roomNumbersDisplay = "", roomCount = 0, totalReservedRooms = 0;
                var discountType = "", discountAmount = "0", unitPriceRackRate = "0", roomRateNegotiatedRate = "0", reservationDetailsId = "0", pax = "";
                var serviceCharge, cityCharge, vat, additionalCharge;

                pax = $.trim($(row).find("td:eq(3)").text());
                roomTypeId = $.trim($(row).find("td:eq(6)").text());
                reservationDetailsId = $.trim($(row).find("td:eq(5)").text());
                roomIds = $.trim($(row).find("td:eq(7)").text());
                roomNumbers = $.trim($(row).find("td:eq(8)").text());
                discountType = $.trim($(row).find("td:eq(10)").text());
                discountAmount = $.trim($(row).find("td:eq(11)").text());
                unitPriceRackRate = $.trim($(row).find("td:eq(12)").text());
                roomRateNegotiatedRate = $.trim($(row).find("td:eq(13)").text());

                totalReservedRooms = $.trim($(row).find("td:eq(9)").text());
                additionalCharge = $(row).find("td:eq(14)").text();
                cityCharge = $(row).find("td:eq(15)").text();
                serviceCharge = $(row).find("td:eq(16)").text();
                vat = $(row).find("td:eq(17)").text();

                $("#ContentPlaceHolder1_txtPaxQuantity").val(pax);
                $("#ContentPlaceHolder1_hfSelectedRoomNumbers").val(roomNumbers);
                $("#ContentPlaceHolder1_hfSelectedRoomId").val(roomIds);
                $("#ContentPlaceHolder1_hfSelectedRoomReservedId").val(reservationDetailsId);

                $("#ContentPlaceHolder1_ddlRoomTypeId").val(roomTypeId);
                $("#ContentPlaceHolder1_ddlDiscountType").val(discountType);
                $("#ContentPlaceHolder1_txtDiscountAmount").val(discountAmount);
                $("#ContentPlaceHolder1_txtUnitPrice").val(unitPriceRackRate);

                $("#ContentPlaceHolder1_txtRoomRate").val(roomRateNegotiatedRate);
                $("#ContentPlaceHolder1_txtRoomId").val(totalReservedRooms);
                $("#ContentPlaceHolder1_lblAddedRoomNumber").text(roomNumbers);

                if (serviceCharge == "1")
                    $("#<%=cbServiceCharge.ClientID%>").prop('checked', true);
                else
                    $("#<%=cbServiceCharge.ClientID%>").prop('checked', false);

                if (cityCharge == "1")
                    $("#<%=cbCityCharge.ClientID%>").prop('checked', true);
                else
                    $("#<%=cbCityCharge.ClientID%>").prop('checked', false);

                if (vat.toLowerCase() == "1")
                    $("#<%=cbVatAmount.ClientID%>").prop('checked', true);
                else
                    $("#<%=cbVatAmount.ClientID%>").prop('checked', false);

                if (additionalCharge == "1")
                    $("#<%=cbAdditionalCharge.ClientID%>").prop('checked', true);
                else
                    $("#<%=cbAdditionalCharge.ClientID%>").prop('checked', false);

                $('#DivAddedRoom').show();
                $("#btnAddDetailGuest").val("Update");
                PerformFillFormActionByTypeId($('#<%=ddlRoomTypeId.ClientID%>').val());

                UpdateTotalCostWithDiscount();
                TotalRoomRateVatServiceChargeCalculation();
            });
            $("#btnReservationSearch").on('click', function (event) {
                LoadReservationInformation();
            });

            //$("#ContentPlaceHolder1_btnCancelDetailGuest").click(function () {
            //    clearUserDetailsControl();
            //});
        });

        function SearchGuestForReservation() {
            $("#ReservationPopEntryPanel").show('slow');
            $("#ReservationPopup").dialog({
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

        function LoadReservationInformation() {
            CommonHelper.SpinnerOpen();
            var guestName = $("#<%=txtPrevGstName.ClientID %>").val();
            var countryId = $("#<%=ddlPrevGstCountry.ClientID %>").val();
            var phn = $("#<%=txtPrevGstPhn.ClientID %>").val();
            var email = $("#<%=txtPrevGstEmail.ClientID %>").val();

            var popCompanyId = 0;
            var popPassportNumber = $("#<%=txtSrcPopPassportNumber.ClientID %>").val();
            var popNationalId = $("#<%=txtSrcPopNationalId.ClientID %>").val();

            PageMethods.SearchNLoadReservationInfo(guestName, countryId, phn, email, popPassportNumber, popNationalId, popCompanyId, OnLoadReservInfoSucceeded, OnLoadReservInfoFailed);
            return false;
        }

        function OnLoadReservInfoSucceeded(result) {
            $("#ltlReservationInformation").html(result);
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadReservInfoFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function CalculateServiceBill() {
            var reservationId = $("#<%=hfReservationId.ClientID %>").val();

            if (reservationId == "") {
                reservationId = 0;
            }

            var pidServiceId = 0, unitPrice = 0.0, totalPrice = 0.0, updatedPrice = 0.0, deletedPrice = 0.0;
            var previousSelectedPrice = 0.0;
            var HtlRegnPaidServiceDetails = [];
            var HtlRegnPaidServiceDelete = [];
            var SelectedPaidServiceAll = [];

            var serviceSelected = "", unitPriceTxt = "", savedUnitPriceTxt = "";

            $("#TableWisePaidServiceInfo tbody tr").each(function (index, item) {

                serviceSelected = $(this).find('td:eq(1)').find("input");
                unitPriceTxt = $(this).find('td:eq(3)').find("input");
                savedUnitPriceTxt = $(this).find('td:eq(4)').text();

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
                        ReservationId: reservationId,
                        ServiceId: pidServiceId,
                        UnitPrice: unitPrice,
                        IsAchieved: false
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

                            DetailPaidServiceId: notOld.DetailPaidServiceId,
                            ReservationId: reservationId,
                            ServiceId: pidServiceId,
                            UnitPrice: unitPrice,
                            IsAchieved: false

                        });

                        deletedPrice += parseFloat(savedUnitPriceTxt);
                        $("#<%= hfPaidServiceDeleteObj.ClientID %>").val("1");
                    }
                }
            });

            var roomRate = parseFloat($("#ContentPlaceHolder1_txtRoomRate").val());
            roomRate = (roomRate - (previousSelectedPrice + updatedPrice + deletedPrice)) + totalPrice;
            $("#ContentPlaceHolder1_txtRoomRate").val(roomRate);
            alreadySavePaidServices = SelectedPaidServiceAll;
            $("#<%=hfPaidServiceSaveObj.ClientID %>").val(JSON.stringify(SelectedPaidServiceAll));
            $("#PaidServiceDialog").dialog("close");
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

        //------End Document Ready--------
        function CurrencyRateInfoEnable() {
            if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                $("#<%=txtConversionRate.ClientID %>").val('');
                $('#CurrencyAmountInformationDiv').hide()
            }
            else {
                $('#CurrencyAmountInformationDiv').show()
            }
        }

        function VisibleListedCompany() {
            if ($('#' + '<%=ddlReservedMode.ClientID%>').val() == "Company") {
                ListedCompanyVisibleTrue();
            }
            else {
                ListedCompanyVisibleFalse();
            }
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
            TotalRoomRateVatServiceChargeCalculation();
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
            var isCheckMinimumRoomRate = $("#<%=hfIsMinimumRoomRateCheckingEnable.ClientID %>").val() == "1";

            if (isCheckMinimumRoomRate) {
                var minimumRoomRate = parseFloat($("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val());

                if (toFixed(roomRate, 2) < toFixed(minimumRoomRate, 2)) {

                    toastr.warning(`Minimum Room Rate For ${$("#<%=ddlRoomTypeId.ClientID %> :selected").text()} : ${minimumRoomRate}`);
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

        function DiscountPolicyByCompanyNRoomType() {
            var companyId = "0";
            var roomTypeId = "0";

            if ($("#ContentPlaceHolder1_ddlReservedMode").val() == "Company") {
                if ($("#ContentPlaceHolder1_chkIsLitedCompany").is(":checked")) {
                    companyId = $("#ContentPlaceHolder1_ddlCompanyName").val()
                    roomTypeId = $("#ContentPlaceHolder1_ddlRoomTypeId").val();
                }
            }

            if (companyId != "0" && roomTypeId != "0") {
                PageMethods.GetDiscountPolicyByCompanyNRoomType(companyId, roomTypeId, OnDiscountPolicyLoadSucceeded, OnDiscountPolicyLoadFailed);
            }

            return false;
        }
        function OnDiscountPolicyLoadSucceeded(result) {
            if (result != null) {
                $("#ContentPlaceHolder1_ddlDiscountType").attr('disabled', true);
                $("#ContentPlaceHolder1_txtDiscountAmount").attr('disabled', true);

                $("#ContentPlaceHolder1_ddlDiscountType").val(result.DiscountType);
                $("#ContentPlaceHolder1_txtDiscountAmount").val(result.DiscountAmount);

                UpdateTotalCostWithDiscount();
            }
            else {
                $("#ContentPlaceHolder1_ddlDiscountType").attr('disabled', false);
                $("#ContentPlaceHolder1_txtDiscountAmount").attr('disabled', false);
            }
        }
        function OnDiscountPolicyLoadFailed() {

        }

        function RoomDetailsByRoomTypeId(roomtypeId) {
            PageMethods.RoomDetailsByRoomTypeId(roomtypeId, OnRoomDetailsLoadSucceeded, OnRoomDetailsLoadFailed);
            return false;
        }
        function OnRoomDetailsLoadSucceeded(result) {
            if ($("#<%=hfCurrencyType.ClientID %>").val() != 'Local') {
                $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRateUSD);
                $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRateUSD);
                $("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val(result.MinimumRoomRateUSD);
                $("#<%=txtRoomRate.ClientID %>").val(result.RoomRateUSD);
            }
            else {
                $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRate);
                $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRate);
                $("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val(result.MinimumRoomRate);
                $("#<%=txtRoomRate.ClientID %>").val(result.RoomRate);
            }
            DiscountPolicyByCompanyNRoomType();
            UpdateTotalCostWithDiscount();
            return false;
        }
        function OnRoomDetailsLoadFailed(error) {
            toastr.error(error.get_message());
        }

        function OnCountrySucceeded(result) {
            $("#ContentPlaceHolder1_txtGuestNationality").val(result);
            var country = $("#<%=ddlGuestCountry.ClientID %>").val();
            if (country == 19) {

                //for (var key in MandaoryFieldsList) {
                //    var id = MandaoryFieldsList[key].FieldId;
                //    if (!($(id).length)) {
                //        id = "#ContentPlaceHolder1_" + id;
                //    }

                //    if (id != "#ContentPlaceHolder1_ddlMarketSegment" && id != "#ContentPlaceHolder1_txtBookersName") {
                //        var tr = $(id).parent().parent();
                //        $(tr).find("label").addClass("required-field");
                //    }
                //}
            }
        }
        function OnCountryFailed() { }

        function IsEmail(email) {
            var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            return regex.test(email);
        }
        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewReservationPanel').hide("slow");
            $('#EntryPanel').show("slow");
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewReservationPanel').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }
        function PerformClearSearchAction() {
            $("#<%=txtFromDate.ClientID %>").val("");
            $("#<%=txtToDate.ClientID %>").val("");
            $("#<%=txtSrcReservationGuest.ClientID %>").val("");
            $("#<%=txtSearchReservationNumber.ClientID %>").val("");
            $("#<%=txtSearchCompanyName.ClientID %>").val("");
            $("#<%=txtCntPerson.ClientID %>").val("");
            $("#<%=ddlOrderCriteria.ClientID %>").val("CheckInDate");
            $("#<%=ddlorderOption.ClientID %>").val("DESC");
            $("#<%=ddlSearchByStatus.ClientID %>").val("All");
        }

        function ToggleListedCompanyInfo() {
            var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
            if ($(ctrl).is(':checked')) {
                $('#ReservedCompany').hide("slow");
                $('#ListedCompany').show("slow");
                $('#PaymentInformation').show("slow");
            }
            else {
                if ($("#ContentPlaceHolder1_btnSave").val() == "Save") {
                    //$("#ContentPlaceHolder1_txtContactPerson").val('');
                    //$("#ContentPlaceHolder1_txtContactAddress").val('');
                    //$("#ContentPlaceHolder1_txtMobileNumber").val('');
                    //$("#ContentPlaceHolder1_txtContactEmail").val('');
                    //$("#txtCompany").val('');
                }

                $('#ListedCompany').hide("slow");
                $('#ReservedCompany').show("slow");
                $('#PaymentInformation').hide("slow");
                $(ctrl).prop('checked', false)
                $("#ContentPlaceHolder1_ddlCompanyName").val("0");
            }
        }
        function ToggleListedContactInfo() {
            var ctrl = '#<%=chkIsLitedContact.ClientID%>';

            if ($(ctrl).is(':checked')) {
                $('#ReservedContact').hide("slow");
                $('#ListedContact').show("slow");
                //$('#PaymentInformation').show("slow");
            }
            else {

                $('#ListedContact').hide("slow");
                $('#ReservedContact').show("slow");
                //$('#PaymentInformation').hide("slow");
                $(ctrl).prop('checked', false)
                $("#ContentPlaceHolder1_hfContactId").val("0");
            }
        }
        function GetCheckedRoomCheckBox() {
            var SelectdRoomId = "";
            var SelectdRoomNumber = "";
            var SelectdRoomReservationId = "";
            var totalRoomAdded = 0;

            $('#TableRoomInformation tbody tr').each(function () {
                var chkBox = $(this).find("td:eq(0)").find("input");
                if ($(chkBox).is(":checked") == true) {
                    if (SelectdRoomId != "") {
                        SelectdRoomId += ',' + $(chkBox).attr('value');
                        SelectdRoomNumber += ',' + $(chkBox).attr('name');
                        SelectdRoomReservationId += ',' + $(this).find("td:eq(2)").text();
                    }
                    else {
                        SelectdRoomId = $(chkBox).attr('value');
                        SelectdRoomNumber = $(chkBox).attr('name');
                        SelectdRoomReservationId = $(this).find("td:eq(2)").text();
                    }

                    totalRoomAdded++;
                }
            });

            if (totalRoomAdded > 0) {
                $("#ContentPlaceHolder1_hfSelectedRoomNumbers").val(SelectdRoomNumber);
                $("#ContentPlaceHolder1_hfSelectedRoomId").val(SelectdRoomId);
                $("#ContentPlaceHolder1_hfSelectedRoomReservedId").val(SelectdRoomReservationId);
                $("#ContentPlaceHolder1_lblAddedRoomNumber").text(SelectdRoomNumber);
                $("#ContentPlaceHolder1_txtRoomId").val(totalRoomAdded);
                $('#DivAddedRoom').show();
                $("#DivRoomSelect").dialog("close");
            }
            else {
                $("#ContentPlaceHolder1_hfSelectedRoomNumbers").val("");
                $("#ContentPlaceHolder1_hfSelectedRoomId").val("");
                $("#ContentPlaceHolder1_hfSelectedRoomReservedId").val("");
                $("#ContentPlaceHolder1_lblAddedRoomNumber").text("");
                $("#ContentPlaceHolder1_txtRoomId").val("");

                if ($("#ContentPlaceHolder1_lblAddedRoomNumber").text() == "") { $('#DivAddedRoom').hide(); }
                else { $('#DivAddedRoom').show(); }

                $("#DivRoomSelect").dialog("close");
            }
        }
        function SaveRoomDetailsInformationByWebMethod() {
            var roomType = $("#ContentPlaceHolder1_ddlRoomTypeId option:selected").text();
            var roomTypeId = $("#ContentPlaceHolder1_ddlRoomTypeId").val();
            var reservationDetailsId = $("#ContentPlaceHolder1_hfSelectedRoomReservedId").val();
            var pax = $("#ContentPlaceHolder1_txtPaxQuantity").val();
            var cbServiceChargeVal = +$("#<%=cbServiceCharge.ClientID%>").is(":checked");
            var cbCityChargeVal = +$("#<%=cbCityCharge.ClientID%>").is(":checked");
            var cbVatAmountVal = +$("#<%=cbVatAmount.ClientID%>").is(":checked");
            var cbAdditionalChargeVal = +$("#<%=cbAdditionalCharge.ClientID%>").is(":checked");
            var roomIds = "", roomNumbers = "", roomCount = 0, totalReservedRooms = 0;
            var roomNumbersDisplay = "";
            var totalRoomRate = $("#ContentPlaceHolder1_txtRoomId").val() * $("#ContentPlaceHolder1_txtTotalRoomRate").val();

            if ($("#ContentPlaceHolder1_txtRoomId").val() != "" && $("#ContentPlaceHolder1_txtRoomId").val() != "0") {
                totalReservedRooms = parseInt($("#ContentPlaceHolder1_txtRoomId").val(), 10);
            }

            if (totalReservedRooms == 0) {
                toastr.warning("Please Give Room Quantity.");
                return false;
            }

            roomNumbers = $("#ContentPlaceHolder1_hfSelectedRoomNumbers").val();
            roomIds = $("#ContentPlaceHolder1_hfSelectedRoomId").val();

            if (roomNumbers == "") {
                $("#RoomNumberDiv").hide();
            }
            else {
                $("#RoomNumberDiv").show();
            }

            var roomIdsArr = roomIds.split(',');
            if (roomIdsArr.length > 0) {
                $("#ContentPlaceHolder1_hfFstAsndRoomId").val(roomIdsArr[0]);
            }
            var j = 0, roomIdLength = roomIdsArr.length;
            var isUnassignedRooms = false;

            for (var i = 0; i < roomIdLength; i++) {
                if (roomIdsArr[i] == "0") {
                    j++;
                }
                else {
                    break;
                }
            }

            if (j == roomIdLength) {
                isUnassignedRooms = true;
            }
            else {
                isUnassignedRooms = false;
            }

            //if (roomIds != "") {
            //    var totalRoomAdded = roomIds.split(',').length;

            //    if (isUnassignedRooms == false && parseInt(totalRoomAdded, 10) > 0 && (parseInt(totalRoomAdded, 10) < parseInt(totalReservedRooms, 10))) {
            //        toastr.warning("Room Quantiy Must Equall With Selected Room Quantity.");
            //        return false;
            //    }
            //}

            if (totalReservedRooms > 0 && $.trim(roomIds) != "" && isUnassignedRooms == false) {
                roomNumbersDisplay = totalReservedRooms + "(" + roomNumbers + ")";
            }
            else {
                roomNumbersDisplay = totalReservedRooms + "(Unassigned)";
            }

            var discountType = "", discountAmount = "0", unitPriceRackRate = "0", roomRateNegotiatedRate = "0";

            discountType = $("#ContentPlaceHolder1_ddlDiscountType").val();
            discountAmount = $("#ContentPlaceHolder1_txtDiscountAmount").val();
            unitPriceRackRate = $("#ContentPlaceHolder1_txtUnitPrice").val();
            roomRateNegotiatedRate = $("#ContentPlaceHolder1_txtRoomRate").val();

            if (editedRow != "") {
                $(editedRow).find("td:eq(0)").text(roomType);
                $(editedRow).find("td:eq(1)").text(roomNumbersDisplay);
                $(editedRow).find("td:eq(2)").text(totalRoomRate);
                $(editedRow).find("td:eq(3)").text(pax);
                $(editedRow).find("td:eq(5)").text(reservationDetailsId);
                $(editedRow).find("td:eq(6)").text(roomTypeId);
                $(editedRow).find("td:eq(7)").text(roomIds);
                $(editedRow).find("td:eq(8)").text(roomNumbers);
                $(editedRow).find("td:eq(9)").text(totalReservedRooms);
                $(editedRow).find("td:eq(10)").text(discountType);
                $(editedRow).find("td:eq(11)").text(discountAmount);
                $(editedRow).find("td:eq(12)").text(unitPriceRackRate);
                $(editedRow).find("td:eq(13)").text(roomRateNegotiatedRate);
                $(editedRow).find("td:eq(14)").text(cbAdditionalChargeVal);
                $(editedRow).find("td:eq(15)").text(cbCityChargeVal);
                $(editedRow).find("td:eq(16)").text(cbServiceChargeVal);
                $(editedRow).find("td:eq(17)").text(cbVatAmountVal);


                var reservationId = $("#ContentPlaceHolder1_hfReservationId").val();

                if (reservationId != "") {

                }

                ClearReservationDeatil();
                ClearRoomNumberAndId();
                ClearRommTypeAfterAdded();

                editedRow = "";
                RoomNumberDropDown();

                $("#ContentPlaceHolder1_hfFstAsndRoomId").val($("#ContentPlaceHolder1_ddlRoomNumber").val());
                return false;
            }

            var alreadyExists;
            $("#ReservationRoomGrid tbody tr").each(function () {
                var aeRoomTypeId = $.trim($(this).find("td:eq(6)").text());
                if (roomTypeId == aeRoomTypeId) {
                    alreadyExists = true;
                    // break the loop once found                   
                    return false;
                }
            });

            if (alreadyExists > 0) {
                toastr.info("Same Room Type Already Added. Please Edit To Change.");
                return false;
            }


            //var alreadyExists = $("#ReservationRoomGrid tbody tr").find("td:eq(6):contains('" + roomTypeId + "')").length;
            //if (alreadyExists > 0) {
            //    toastr.info("Same Room Type Already Added. Please Edit To Change.");
            //    return false;
            //}

            var grid = "", counter = 0;
            counter = $("#ReservationRoomGrid tbody tr").length;

            if (counter % 2 == 0) {
                grid += "<tr style='background-color:#E3EAEB;'>";
            }
            else {
                grid += "<tr style='background-color:White;'>";
            }

            grid += "<td align='left' style='width: 30%;'>" + roomType + "</td>";
            grid += "<td align='left' style='width: 38%;'>" + roomNumbersDisplay + "</td>";
            grid += "<td align='left' style='width: 12%;display:none;'>" + totalRoomRate + "</td>";
            grid += "<td align='left' style='width: 12%;'>" + pax + "</td>";

            grid += "<td align='center' style='width: 8%;'>";
            grid += "&nbsp;<img src='../Images/delete.png' class='deleteroom' alt='Delete Room' border='0' />";
            grid += "&nbsp;<img src='../Images/edit.png' class='editroom' alt='Edit Room' border='0' />";
            grid += "</td>";
            grid += "<td align='left' style='display:none;'>" + reservationDetailsId + "</td>";
            grid += "<td align='left' style='display:none;'>" + roomTypeId + "</td>";
            grid += "<td align='left' style='display:none;'>" + roomIds + "</td>";
            grid += "<td align='left' style='display:none;'>" + roomNumbers + "</td>";
            grid += "<td align='left' style='display:none;'>" + totalReservedRooms + "</td>";
            grid += "<td align='left' style='display:none;'>" + discountType + "</td>";
            grid += "<td align='left' style='display:none;'>" + discountAmount + "</td>";
            grid += "<td align='left' style='display:none;'>" + unitPriceRackRate + "</td>";
            grid += "<td align='left' style='display:none;'>" + roomRateNegotiatedRate + "</td>";
            grid += "<td align='left' style='display:none;'>" + cbAdditionalChargeVal + "</td>";
            grid += "<td align='left' style='display:none;'>" + cbCityChargeVal + "</td>";
            grid += "<td align='left' style='display:none;'>" + cbServiceChargeVal + "</td>";
            grid += "<td align='left' style='display:none;'>" + cbVatAmountVal + "</td>";

            grid += "</tr>";

            $("#ReservationRoomGrid tbody").append(grid);

            ClearReservationDeatil();
            ClearRoomNumberAndId();
            ClearRommTypeAfterAdded();

            RoomNumberDropDown();
            $("#ContentPlaceHolder1_hfFstAsndRoomId").val($("#ContentPlaceHolder1_ddlRoomNumber").val());

            return false;

            //------------------------------------------------------------------
            var isEdit = false;

            if ($('#btnAddDetailGuest').val() == "Save") {
                isEdit = true;
            }

            var duplicateRoomType = 0, editedRoomType = 0, isDbType = 0;
            var ddlRoomTypeId = $("#<%=ddlRoomTypeId.ClientID %>").val();
            duplicateRoomType = $("#ReservationDetailGrid tbody tr").find("td:eq(0):contains(" + ddlRoomTypeId + ")").length;

            var td = $("#ReservationDetailGrid tbody tr").find("td:eq(0):contains(" + ddlRoomTypeId + ")");
            var tr = $(td).parent();

            isDbType = parseInt($(tr).find("td:eq(1)").text(), 10);

            if (isEdit == true) {

                editedRoomType = $("#ReservationDetailGrid tbody tr:eq(" + editedRowIndex + ")").find("td:eq(0)").text();

                if (editedRoomType != ddlRoomTypeId) {
                    toastr.info("Please, first delete this room type to choose another type of room.");
                    return false;
                }
            }
            else if (isEdit == false && duplicateRoomType > 0 && isDbType > 0) {
                toastr.info("Please, first take this room type as edit mode and add additional room.");
                return false;
            }

            var hfSelectedRoomNumbers = $("#<%=hfSelectedRoomNumbers.ClientID %>").val();
            var hfSelectedRoomId = $("#<%=hfSelectedRoomId.ClientID %>").val();
            var txtUnitPriceHiddenField = $("#<%=txtUnitPriceHiddenField.ClientID %>").val();
            var txtRoomRate = $("#<%=txtRoomRate.ClientID %>").val();
            var txtRoomId = $("#<%=txtRoomId.ClientID %>").val();

            if (ddlRoomTypeId == 0) {
                toastr.warning('Please Provide Room Type.');
                return;
            }

            if (txtRoomId == "") {
                toastr.warning('Please Provide Room Quantity.');
                return;
            }

            var ddlRoomTypeIdText = $("#<%=ddlRoomTypeId.ClientID %> option:selected").text();
            var lblHiddenId = $("#<%=lblHiddenId.ClientID %>").text();
            var txtDiscountAmount = $("#<%=txtDiscountAmount.ClientID %>").val();
            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            var ddlDiscountType = $("#<%=ddlDiscountType.ClientID %>").val();
            var ddlReservedMode = $("#<%=ddlReservedMode.ClientID %>").val();
            var prevRoomTypeId = $("#<%=ddlRoomTypeIdHiddenField.ClientID %>").val();
            var paxQuantity = $("#ContentPlaceHolder1_txtPaxQuantity").val();

            $('#btnAddDetailGuest').val("Add");

            if (Isvalid) {
                PageMethods.PerformSaveRoomDetailsInformationByWebMethod(isEdit, hfSelectedRoomNumbers, hfSelectedRoomId, txtUnitPriceHiddenField, txtRoomRate, txtRoomId, prevRoomTypeId, ddlRoomTypeId, ddlRoomTypeIdText, lblHiddenId, txtDiscountAmount, ddlCurrency, ddlDiscountType, ddlReservedMode, paxQuantity, OnPerformSaveRoomDetailsSucceeded, OnPerformSaveRoomDetailsFailed);
            }
            return false;
        }
        function OnPerformSaveRoomDetailsFailed(error) {
            toastr.error(error.get_message());
        }
        function OnPerformSaveRoomDetailsSucceeded(result) {
            $("#<%=ddlRoomTypeIdHiddenField.ClientID %>").val('');
            $("#<%=hfSelectedRoomId.ClientID %>").val('');
            $("#<%=hfSelectedRoomNumbers.ClientID %>").val('');
            $("#ReservationDetailGrid").html(result);
            ClearReservationDeatil();
            $("#<%=txtEditedRoom.ClientID %>").val('');
            ClearRoomNumberAndId();
            ClearRommTypeAfterAdded();
            editedRowIndex = "";
        }

        function CancelRoomAddEdit() {
            $("#<%=ddlRoomTypeIdHiddenField.ClientID %>").val('');
            $("#<%=hfSelectedRoomId.ClientID %>").val('');
            $("#<%=hfSelectedRoomNumbers.ClientID %>").val('');
            ClearReservationDeatil();
            $("#<%=txtEditedRoom.ClientID %>").val('');
            ClearRoomNumberAndId();
            ClearRommTypeAfterAdded();
            editedRowIndex = "";
            editedRow = "";
            return false;
        }

        function Isvalid() {
            var txtRoomId = $("#<%=txtRoomId.ClientID %>").val();
            if (txtRoomId == "" || parseInt(txtRoomId) <= 0) { return false; }
            else {
                return true;
            }
        }
        function ClearReservationDeatil() {
            $("#<%=ddlRoomTypeId.ClientID %>").val('1');
            $("#<%=txtRoomId.ClientID %>").val('');
            $("#<%=lblAddedRoomNumber.ClientID %>").text('');
            $("#<%=txtRoomRate.ClientID %>").val($("#<%=txtUnitPrice.ClientID %>").val());
            $("#btnAddDetailGuest").val('Add');
            $("#TableRoomInformation tbody tr").find("td:eq(0)").find("input").prop("checked", false);
        }
        function PerformReservationDetailDelete(roomTypeId) {
            if (!confirm("Do you want to delete?")) {
                return false;
            }
            else {
                $("#<%=txtRoomId.ClientID %>").val("");
                $("#<%=ddlRoomTypeId.ClientID %>").val("");
                ClearReservationDeatil();
                $('#DivAddedRoom').hide();
            }

            PageMethods.PerformDeleteByWebMethod(roomTypeId, OnPerformDeleteRoomDetailsSucceeded, OnPerformDeleteRoomDetailsFailed);
            return false;
        }

        function OnPerformDeleteRoomDetailsSucceeded(result) {
            $("#<%=ddlRoomTypeId.ClientID %>").val("0");
            $("#<%=txtUnitPrice.ClientID %>").val("0");
            $("#<%=txtRoomRate.ClientID %>").val("0");
            $("#<%=txtUnitPriceHiddenField.ClientID %>").val("0");
            $("#<%=ddlDiscountType.ClientID %>").val("Fixed");
            $("#<%=txtDiscountAmount.ClientID %>").val("0");
            $("#<%=txtRoomId.ClientID %>").val("");
            $("#ReservationDetailGrid").html(result);
            return false;
        }
        function OnPerformDeleteRoomDetailsFailed(error) {
        }

        function PerformReservationDetailEdit(roomTypeId, rowIndex) {
            editedRowIndex = rowIndex;
            $('#btnAddDetailGuest').val("Save");
            PageMethods.PerformReservationDetailEditByWebMethod(roomTypeId, OnPerformReservationDetailEditSucceeded, OnPerformReservationDetailEditFailed);
            return false;
        }
        function OnPerformReservationDetailEditSucceeded(result) {
            var RoomIdList = result.RoomNumberIdList;
            var RoomNumberList = result.RoomNumberList;
            var res = "";
            var RoomArray = RoomIdList.split(",");
            $("#<%=hfSelectedRoomNumbers.ClientID %>").val(RoomNumberList);
            $("#<%=hfSelectedRoomId.ClientID %>").val(RoomIdList);
            $("#<%=txtRoomId.ClientID %>").val(result.RoomQuantity);
            $("#<%=ddlRoomTypeId.ClientID %>").val(result.RoomTypeId);
            $("#<%=ddlRoomTypeIdHiddenField.ClientID %>").val(result.RoomTypeId);
            $("#<%=lblAddedRoomNumber.ClientID %>").text(RoomNumberList);
            $("#<%=txtUnitPrice.ClientID %>").val(result.UnitPrice);
            $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.UnitPrice);
            $("#<%=ddlDiscountType.ClientID %>").val(result.DiscountType);
            $("#<%=txtDiscountAmount.ClientID %>").val(result.DiscountAmount);
            $("#<%=txtRoomRate.ClientID %>").val(result.RoomRate);

            $('#DivAddedRoom').show();
            RoomDetailsByRoomTypeId($('#<%=ddlRoomTypeId.ClientID%>').val());
            return false;
        }

        function OnPerformReservationDetailEditFailed(error) {
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewReservationPanel').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewReservationPanel').hide("slow");
        }
        //Listed Company True/False-------------------
        function ListedCompanyVisibleTrue() {
            $('#CompanyInformation').show();
            $('#ContactAddress').show();
        }
        function ListedCompanyVisibleFalse() {
            $('#CompanyInformation').hide();
            $('#ContactAddress').hide();
        }

        //Listed Company DropDown True/False-------------------
        function ListedCompanyDropDownVisibleTrue() {
            $('#ListedCompany').show();
            $('#ReservedCompany').hide();
        }
        function ListedCompanyDropDownVisibleFalse() {
            $('#ReservedCompany').show();
            $('#ListedCompany').hide();
        }

        function GetTotalCostWithCompanyOrPersonalDiscount() {
            var promId = $("#<%=ddlBusinessPromotionId.ClientID %>").val();
            var companyId = $("#<%=ddlCompanyName.ClientID %>").val();
            var ddlReservedMode = $("#<%=ddlReservedMode.ClientID %>").val();
            var isCheked = false;
            if ($("#<%=chkIsLitedCompany.ClientID %>").is(":checked") == true) { isCheked = true; }
            else { isCheked = false; }

            if (ddlReservedMode != "Company" || isCheked == false && companyId <= 0) { companyId = 0; }

            if (promId <= 0) { promId = 0; }

            if (companyId != 0 || promId != 0) {
                PageMethods.GetCalculatedDiscount(companyId, promId, GetCalculatedDiscountObjectSucceeded, GetCalculatedDiscountObjectFailed);
                return false;
            }
        }
        function GetCalculatedDiscountObjectSucceeded(result) {
            $("#<%=txtContactAddress.ClientID %>").val(result.BillingAddress);
            $("#<%=txtContactEmail.ClientID %>").val(result.EmailAddressWithoutLabel);
            $("#<%=txtMobileNumber.ClientID %>").val(result.ContactNumberWithoutLabel);
            var prevDiscount = parseFloat($("#<%=txtDiscountAmount.ClientID %>").val());
            if (isNaN(prevDiscount)) {
                prevDiscount = 0;
            }
            $("#<%=ddlDiscountType.ClientID %>").val('Percentage');
            var resultFloat = parseFloat(result.DiscountPercent);

            $("#<%=txtDiscountAmount.ClientID %>").val(resultFloat);

            UpdateTotalCostWithDiscount();
            return false;
        }
        function GetCalculatedDiscountObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadDataOnParentForm() {
            var guestId = $("#<%=hiddenGuestId.ClientID %>").val();
            PageMethods.LoadDetailInformation(guestId, OnLoadParentFromDetailObjectSucceeded, OnLoadParentFromDetailObjectFailed);
            return false;

        }
        function OnLoadParentFromDetailObjectSucceeded(result) {

            if (result.GuestDOB) {
                var date1 = new Date(result.GuestDOB);
                $("#<%=txtGuestDOB.ClientID %>").val(GetStringFromDateTime(result.GuestDOB));
            }
            if (result.PIssueDate) {
                var date2 = new Date(result.PIssueDate);
                $("#<%=txtPIssueDate.ClientID %>").val(GetStringFromDateTime(result.PIssueDate));
            }
            if (result.PExpireDate) {
                var date3 = new Date(result.PExpireDate);
                $("#<%=txtPExpireDate.ClientID %>").val(GetStringFromDateTime(result.PExpireDate));
            }
            if (result.VIssueDate) {
                var date4 = new Date(result.VIssueDate);
                $("#<%=txtVIssueDate.ClientID %>").val(GetStringFromDateTime(result.VIssueDate));
            }
            if (result.VExpireDate) {
                var date5 = new Date(result.VExpireDate);
                $("#<%=txtVExpireDate.ClientID %>").val(GetStringFromDateTime(result.VExpireDate));
            }

            $("#<%=hiddenGuestName.ClientID %>").val(result.GuestName);
            $("#<%=hiddenGuestId.ClientID %>").val(result.GuestId);

            $("#<%=txtGuestName.ClientID %>").val(result.GuestName);
            $("#<%=txtGuestFirstName.ClientID %>").val(result.FirstName);
            $("#<%=txtGuestLastName.ClientID %>").val(result.LastName);
            $("#<%=ddlGuestTitle.ClientID %>").val(result.Title);
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
            $("#<%=ddlClassificationId.ClientID %>").val(result.ClassificationId);
            $("#<%=ddlGuestCountry.ClientID %>").val(result.GuestCountryId);
            $("#txtGuestCountrySearch").val(result.CountryName);
            var guestPreferences = "";
            if (result.GuestPreferences != null)
                guestPreferences += result.GuestPreferences;
            if (result.AdditionalRemarks != "")
                guestPreferences += (guestPreferences != "" ? ", " : "") + result.AdditionalRemarks;
            $("#<%=lblGstPreference.ClientID %>").text(guestPreferences);

            SelectdPreferenceId = result.GuestPreferenceId != null ? result.GuestPreferenceId : "";
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);
            $("#ContentPlaceHolder1_hfAdditionalRemarks").val(result.AdditionalRemarks);
            if (typeof $("#ContentPlaceHolder1_txtAdditionalRemarks").val() !== "undefined") {
                $("#ContentPlaceHolder1_txtAdditionalRemarks").val(result.AdditionalRemarks);
            }
            if (guestPreferences != "") {
                $("#GuestPreferenceDiv").show();
            }
            else $("#GuestPreferenceDiv").hide();
            $("#<%=hfIsBlockGuest.ClientID %>").val(result.GuestBlock);

            var isBlock = $("#ContentPlaceHolder1_hfIsBlockGuest").val();

            if (isBlock == "true") {
                //toastr.info("hi");
                $("#chkYesBlock").prop("checked", true);
            }
            else {
                //toastr.info("hello");
                $("#chkYesBlock").prop("checked", false);
            }

            return false;
        }
        function OnLoadParentFromDetailObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage, searchoption) {
            var gridRecordsCount = $("#gvGustIngormation tbody tr").length;
            var GuestName = "";
            if (searchoption == 0) {
                GuestName = $("#<%=txtGuestName.ClientID %>").val();
            }
            else {
                GuestName = $("#<%=txtSrcGuestName.ClientID %>").val();
            }
            var companyName = $("#<%=txtSrcCompanyName.ClientID %>").val();
            var DateOfBirth = $("#<%=txtSrcDateOfBirth.ClientID %>").val();
            var EmailAddress = $("#<%=txtSrcEmailAddress.ClientID %>").val();
            var FromDate = $("#<%=txtSrcFromDate.ClientID %>").val();
            var ToDate = $("#<%=txtSrcToDate.ClientID %>").val();
            var MobileNumber = $("#<%=txtSrcMobileNumber.ClientID %>").val();
            var NationalId = $("#<%=txtSrcNationalId.ClientID %>").val();
            var PassportNumber = $("#<%=txtSrcPassportNumber.ClientID %>").val();
            var RegistrationNumber = $("#<%=txtSrcRegistrationNumber.ClientID %>").val();
            var RoomNumber = $("#<%=txtSrcRoomNumber.ClientID %>").val();
            PageMethods.SearchGuestAndLoadGridInformation(companyName, DateOfBirth, EmailAddress, FromDate, ToDate, GuestName, MobileNumber, NationalId, PassportNumber, RegistrationNumber, RoomNumber, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvGustIngormation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvGustIngormation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#gvGustIngormation tbody tr").length;
                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:40%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.GuestId + "')\">" + gridObject.GuestName + "</td>";
                tr += "<td align='left' style=\"width:25%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.GuestId + "')\">" + gridObject.CountryName + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.GuestId + "')\">" + gridObject.GuestPhone + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.GuestId + "')\">" + gridObject.GuestEmail + "</td>";

                tr += "</tr>"

                $("#gvGustIngormation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function GridPagingForSearchReservation(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#tblRoomReserve tbody tr").length;
            var fromDate = $("#<%=txtFromDate.ClientID %>").val();
            var toDate = $("#<%=txtToDate.ClientID %>").val();
            var guestName = $("#<%=txtSrcReservationGuest.ClientID %>").val();
            var reserveNo = $("#<%=txtSearchReservationNumber.ClientID %>").val();
            var companyName = $("#<%=txtSearchCompanyName.ClientID %>").val();
            var contactPerson = $("#<%=txtCntPerson.ClientID %>").val();
            var orderCriteria = $("#<%=ddlOrderCriteria.ClientID %>").val();
            var orderOption = $("#<%=ddlorderOption.ClientID %>").val();
            PageMethods.SearchResevationAndLoadGridInformation(fromDate, toDate, guestName, reserveNo, companyName, contactPerson, orderCriteria, orderOption, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSuccess, OnFail);
            return false;
        }
        function OnSuccess(result) {
            var format = innBoarDateFormat.replace('mm', 'MM');
            var format = format.replace('yy', 'yyyy');

            vvc = result;
            $("#tblRoomReserve tbody tr").remove();
            $("#GridPagingContainerForSearchReservation ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#tblRoomReserve tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#tblRoomReserve tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:16px; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.ReservationNumber + "</td>";
                tr += "<td align='left' style=\"width:25px; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.GuestName + "</td>";
                tr += "<td align='left' style=\"width:17px; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.CompanyName + "</td>";
                tr += "<td align='left' style=\"width:18px; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.RoomInformation + "</td>";
                //tr += "<td align='left' style=\"width:5%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.ReservationDate.format(format) + "</td>";
                tr += "<td align='left' style=\"width:8px; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.DateIn.format(format) + "</td>";
                tr += "<td align='left' style=\"width:8px; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.DateOut.format(format) + "</td>";
                tr += "<td align='left' style=\"width:9px; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.ReservationMode + "</td>";

                if ($.trim(gridObject.ReservationMode) != "Registered")
                    tr += "<td align='right' style=\"width:45%; cursor:pointer;\"><img src='../Images/edit.png' ToolTip='Edit' onClick= \"javascript:return PerformEditAction('" + gridObject.ReservationId + "')\" alt='Edit Information' border='0' />&nbsp;<img src='../Images/delete.png' ToolTip='Delete' class= 'RoomreservationDelete'  alt='Delete Information' border='0' />&nbsp;<img src='../Images/ReportDocument.png' ToolTip='Preview' onClick= \"javascript:return PerformBillPreviewAction('" + gridObject.ReservationId + "')\" alt='Preview Information' border='0' /></td>";
                else
                    tr += "<td align='right' style=\"width:45%; cursor:pointer;\"><img src='../Images/ReportDocument.png' ToolTip='Preview' onClick= \"javascript:return PerformBillPreviewAction('" + gridObject.ReservationId + "')\" alt='Preview Information' border='0' /></td>";

                tr += "<td align='right' style=\"width:5%; display:none;\">" + gridObject.ReservationId + "</td>";

                tr += "</tr>"

                $("#tblRoomReserve tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainerForSearchReservation ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainerForSearchReservation ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainerForSearchReservation ul").append(result.GridPageLinks.NextButton);

            return false;
        }
        function OnFail(error) {
            toastr.error(error.get_message());
        }

        function PerformEditAction(reservationId) {
            var possiblePath = "frmRoomReservation.aspx?editId=" + reservationId;
            window.location = possiblePath;
        }
        function PerformBillPreviewAction(reservationId) {
            var apdId = $("#ContentPlaceHolder1_hfAPDId").val();
            var url = "/HotelManagement/Reports/frmReportReservationBillInfo.aspx?GuestBillInfo=" + reservationId + "&APDInfo=" + apdId;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function SelectGuestInformation(GuestId) {
            $("#PopSearchPanel").hide('slow');
            $("#PopTabPanel").show('slow');
            PageMethods.LoadDetailInformation(GuestId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            LoadGuestImage(GuestId);
            LoadGuestHistory(GuestId);
            return false;
        }
        function OnLoadDetailObjectSucceeded(result) {
            if (result.GuestDOB) {

                $("#<%=lblDGuestDOB.ClientID %>").text(GetStringFromDateTime(result.GuestDOB));
            }
            if (result.PIssueDate) {

                $("#<%=lblDPIssueDate.ClientID %>").text(GetStringFromDateTime(result.PIssueDate));
            }
            if (result.PExpireDate) {

                $("#<%=lblDPExpireDate.ClientID %>").text(GetStringFromDateTime(result.PExpireDate));
            }
            if (result.VIssueDate) {

                $("#<%=lblDVIssueDate.ClientID %>").text(GetStringFromDateTime(result.VIssueDate));
            }
            if (result.VExpireDate) {

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
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);
            var additionalRemarks = result.AdditionalRemarks != "" ? `<ul><li>${result.AdditionalRemarks}</li></ul>` : "";
            $("#AdditionalRemarks").html(additionalRemarks);

            $('#imageDiv').text('');
            var img = document.createElement("IMG");
            img.src = result.Path;
            document.getElementById('imageDiv').appendChild(img);

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

        $(function () {
            $("#PopMyTabs").tabs();
        });
        function AddNewItem() {
            $("#TouchKeypad").dialog({
                width: 900,
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

        function PopUpHotelPositionReportInfo() {
            var txtFromDate = $("#<%=txtDateIn.ClientID %>").val();
            var txtDateOut = $("#<%=txtDateOut.ClientID %>").val();
            if (txtFromDate == "") {
                toastr.warning('The Check-In Date should not be empty.');
                document.getElementById("<%=txtDateIn.ClientID%>").focus();
                return false;
            }
            else if (txtDateOut == "") {
                toastr.warning('The Check Out Date should not be empty.');
                document.getElementById("<%=txtDateOut.ClientID%>").focus();
                return false;
            }
            else {
                var iframeid = 'frmRoomControlChartDialogue';
                var url = "./Reports/frmReportRoomControlChart.aspx?FromDate=" + txtFromDate + "&ToDate=" + txtDateOut;
                parent.document.getElementById(iframeid).src = url;

                $("#ReservationRoomControlChartDialogue").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 1100,
                    height: 600,
                    minWidth: 550,
                    minHeight: 580,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    title: "Hotel Position",
                    show: 'slide'
                });
            }
            return false;
        }

        // Room Select
        function LoadRoomNumber() {
            var txtFromDate = $("#<%=txtDateIn.ClientID %>").val();
        var txtToDate = $("#<%=txtDateOut.ClientID %>").val();

        if (txtFromDate == "") {
            //CustomAlert('The Check-In Date should not be empty.', 'Check In', 'Ok')
            toastr.warning('The Check-In Date should not be empty.');
            document.getElementById("<%=txtDateIn.ClientID%>").focus();
                return false;
            }
            else if (txtToDate == "") {
                //CustomAlert('The Check-Out Date should not be empty.', 'Check Out', 'Ok')
                toastr.warning('The Check-Out Date should not be empty.');
                document.getElementById("<%=txtToDate.ClientID%>").focus();
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlRoomTypeId").val() == "0") {
                toastr.warning("Please Select Room Type.");
                return false;
            }

            LoadRoomInformationWithControl();
            $("#DivRoomSelect").dialog({
                width: 250,
                height: 400,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Room Number",
                show: 'slide'
            });
            return false;
        }
        function LoadRoomInformationWithControl() {
            var RoomTypeId = $("#<%=ddlRoomTypeId.ClientID %>").val();

        var txtFromDate = $("#<%=txtDateIn.ClientID %>").val();
        var txtToDate = $("#<%=txtDateOut.ClientID %>").val();

        $("#<%=DateInHiddenField.ClientID %>").val(txtFromDate);
        $("#<%=DateOutHiddenField.ClientID %>").val(txtToDate);
        $("#<%=hfCurrencyHiddenField.ClientID %>").val($("#ContentPlaceHolder1_ddlCurrency").val());

            PageMethods.LoadRoomInformationWithControl(RoomTypeId, txtFromDate, txtToDate, OnLoadRoomInformationWithControlSucceeded, OnLoadRoomInformationWithControlFailed);

            return false;
        }
        function OnLoadRoomInformationWithControlSucceeded(result) {
            $("#ltlRoomNumberInfo").html(result);

            var RoomIdList = "";

            if (editedRow != "" && $("#ContentPlaceHolder1_hfSelectedRoomId").val() == "") {
                RoomIdList = $.trim($(editedRow).find("td:eq(5)").text());
            }
            else {
                RoomIdList = $("#ContentPlaceHolder1_hfSelectedRoomId").val();
            }

            if (RoomIdList.length > 0) {
                var RoomArray = RoomIdList.split(",");

                if (RoomArray.length > 0) {
                    for (var i = 0; i < RoomArray.length; i++) {
                        var roomId = RoomArray[i].trim();
                        $("#" + roomId).attr("checked", true);
                    }
                }
            }

            return false;
        }
        function OnLoadRoomInformationWithControlFailed(error) {
            toastr.error(error.get_message());
        }

        function ShowRoomNumberAndId() {
            var ids = $("#<%=hfSelectedRoomId.ClientID %>").val();

        var numbers = $("#<%=hfSelectedRoomNumbers.ClientID %>").val();
        var addedRoom = $("#<%=lblAddedRoomNumber.ClientID %>").val()
        var splitedNumbers = numbers.split(",");
        var flag = "";
        for (var i = 0; i < splitedNumbers.length; i++) {
            if (splitedNumbers[i] != '') {
                flag = splitedNumbers[i] + " , " + flag;
            }
        }

        flag = RemoveFirstCommas(flag);
        flag = RomoveLastCommas(flag);


        $("#<%=hfSelectedRoomNumbers.ClientID %>").val(flag);
        if (splitedNumbers.length > 0) {
            $("#<%=lblAddedRoomNumber.ClientID %>").text(flag);
            $('#DivAddedRoom').show();
        }
        else {
            $("#<%=lblAddedRoomNumber.ClientID %>").text('No Room Is Added.')
            $('#DivAddedRoom').hide();
        }


        var roomIds = $("#<%=hfSelectedRoomId.ClientID %>").val();
        var splitedRoomId = roomIds.split(",");
        var roomIdFlag = "";
        for (var i = 0; i < splitedRoomId.length; i++) {
            if (splitedNumbers[i] != '') {
                roomIdFlag = splitedRoomId[i] + " , " + roomIdFlag;
            }
        }
        roomIdFlag = RemoveFirstCommas(roomIdFlag);
        roomIdFlag = RomoveLastCommas(roomIdFlag);
        var roomsArray = roomIdFlag.split(',');
        var roomLength = roomsArray.length;
        $("#<%=hfSelectedRoomId.ClientID %>").val(roomIdFlag);
        $("#<%=txtRoomId.ClientID %>").val(roomLength);
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
        function ClearRoomNumberAndId() {
            $("#<%=hfSelectedRoomNumbers.ClientID %>").val('');

        $("#<%=hfSelectedRoomId.ClientID %>").val('');
        $("#<%=hfSelectedRoomReservedId.ClientID %>").val('');
        $("#<%=lblAddedRoomNumber.ClientID %>").text('')
            $("#ContentPlaceHolder1_txtRoomId").val('');
            $("#ltlRoomNumberInfo").html('');
            $('#DivAddedRoom').hide();
        }

        function ClearRommTypeAfterAdded() {
            $("#ContentPlaceHolder1_ddlRoomTypeId").val('0');
            $("#ContentPlaceHolder1_txtUnitPrice").val("");
            $("#ContentPlaceHolder1_txtRoomRate").val("");
            $("#ContentPlaceHolder1_txtPaxQuantity").val("");
            $("#ContentPlaceHolder1_txtServiceCharge").val("");
            $("#ContentPlaceHolder1_txtVatAmount").val("");
            $("#ContentPlaceHolder1_txtCityCharge").val("");
            $("#ContentPlaceHolder1_txtAdditionalCharge").val("");
            $("#ContentPlaceHolder1_txtTotalRoomRate").val("");

            var ctrl = '#<%=chkIsLitedCompany.ClientID%>'

        if ($(ctrl).is(':checked')) {
            var companyId = $("#<%=ddlCompanyName.ClientID %>").val();
                if (companyId == 0 || companyId == "") {
                    $("#ContentPlaceHolder1_txtDiscountAmount").val("");
                }
            }
            else {
                $("#ContentPlaceHolder1_txtDiscountAmount").val("");
            }

        }

        function AddRoomNumberAndIdTemporary(RoomId, RoomNumber) {
            if ($('#' + RoomId).is(":checked")) {
                // Add Room Ids
                var ids = $("#<%=hfSelectedRoomId.ClientID %>").val();
                var splitedIds = ids.split(",");
                var flag = -1;
                for (var i = 0; i < splitedIds.length; i++) {
                    if (splitedIds[i].trim() == RoomId) {
                        flag = 1;
                    }
                }
                if (flag == -1) {
                    ids = RoomId + ',' + ids;
                }
                $("#<%=hfSelectedRoomId.ClientID %>").val(ids);
                //Add Room Numbers

                var numbers = $("#<%=hfSelectedRoomNumbers.ClientID %>").val();
                var splitedNumbers = numbers.split(",");
                var roomFlag = -1;
                for (var i = 0; i < splitedNumbers.length; i++) {
                    if (splitedNumbers[i].trim() == RoomNumber) {
                        roomFlag = 1;
                    }
                }
                if (roomFlag == -1) {
                    numbers = RoomNumber + ',' + numbers;
                }
                $("#<%=hfSelectedRoomNumbers.ClientID %>").val(numbers)
            }
            else {
                //Delete Room Ids
                var ids = $("#<%=hfSelectedRoomId.ClientID %>").val();
                var splitedIds = ids.split(",");
                var activeIds = "";
                for (var i = 0; i < splitedIds.length; i++) {
                    if (splitedIds[i].trim() == RoomId) {
                        splitedIds[i] = -1;
                    }
                }
                for (var i = 0; i < splitedIds.length; i++) {
                    if (splitedIds[i].trim() != -1) {
                        activeIds = splitedIds[i].trim() + ',' + activeIds;
                    }
                }
                $("#<%=hfSelectedRoomId.ClientID %>").val(activeIds);
                //Delete Room Numbers
                var rooms = $("#<%=hfSelectedRoomNumbers.ClientID %>").val();
                var splitedRooms = rooms.split(",");
                var activeIds = "";
                for (var i = 0; i < splitedRooms.length; i++) {
                    if (splitedRooms[i].trim() == RoomNumber) {
                        splitedRooms[i] = -1;
                    }
                }
                for (var i = 0; i < splitedRooms.length; i++) {
                    if (splitedRooms[i].trim() != -1) {
                        activeIds = splitedRooms[i] + ',' + activeIds;
                    }
                }
                $("#<%=hfSelectedRoomNumbers.ClientID %>").val(activeIds);
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
                    //var totalRoomRate = parseFloat(toFixed(result.RackRate, 2)) + parseFloat(toFixed(result.AdditionalCharge, 2));
                    var totalRoomRate = result.CalculatedAmount;
                    $("#<%=txtTotalRoomRate.ClientID %>").val(totalRoomRate);
                }
                else {
                    $("#<%=txtTotalRoomRate.ClientID %>").val(result.RackRate);
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

            var isCheckMinimumRoomRate = $("#<%=hfIsMinimumRoomRateCheckingEnable.ClientID %>").val() == "1";

            if (isCheckMinimumRoomRate) {
                var minimumRoomRate = parseFloat($("#<%=txtMinimumUnitPriceHiddenField.ClientID %>").val());

                if (toFixed(unitPrice, 2) < toFixed(minimumRoomRate, 2)) {
                    var actualRoomRate = parseFloat($('#' + txtUnitPrice).val());
                    var maximumDiscount = actualRoomRate - minimumRoomRate;
                    toastr.warning(`Minimum Room Rate For ${$("#<%=ddlRoomTypeId.ClientID %> :selected").text()} : ${minimumRoomRate}. Discount Amount Cannot Greater than ${maximumDiscount}.`);
                    $('#<%=txtDiscountAmount.ClientID%>').val(maximumDiscount).trigger('blur').focus();
                    return true;
                }
            }

            $('#' + txtRoomRate).val(toFixed(unitPrice, 2));
        }

        function ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(ctrl) {
            TotalRoomRateVatServiceChargeCalculation();
        }

        function ToggleTotalRoomRateVatServiceChargeCalculationForVat(ctrl) {
            TotalRoomRateVatServiceChargeCalculation();
        }

        function CustomAlert(message, title, buttonText) {
            buttonText = (buttonText == undefined) ? "Ok" : buttonText;
            title = (title == undefined) ? "The page says:" : title;
            var div = $('<div>');
            div.html(message);
            div.attr('title', title);
            div.dialog({
                autoOpen: true,
                modal: true,
                draggable: false,
                resizable: false,
                buttons: [{
                    text: buttonText,
                    click: function () {
                        $(this).dialog("close");
                        div.remove();
                    }
                }]
            });
        }

        function GetRoomDetailInformationByWM() {
            PageMethods.GetRoomDetailGridInformationByWM(RoomDetailInformationSucceeded, RoomDetailInformationFailed);
            return false;
        }
        function RoomDetailInformationSucceeded(result) {
            $("#ReservationDetailGrid").html(result);
            return false;
        }
        function RoomDetailInformationFailed(error) {

            toastr.error(error.get_message());
        }

        function GetTempRegistrationDetailByWM() {
            PageMethods.GetTempRegistration(OnLoadDetailGridInformationSucceeded, OnLoadDetailGridInformationFailed);
            return false;
        }
        function OnLoadDetailGridInformationSucceeded(result) {
            $("#ltlGuestDetailGrid").html(result);
            $("#<%=EditId.ClientID %>").val("");
            $("#<%=hfPrevGuestId.ClientID %>").val("");
            $("#<%=hfCurrentReservationGuestId.ClientID %>").val("");
            SelectdPreferenceId = "";
            $("#ContentPlaceHolder1_hfAdditionalRemarks").val("");
            $("#ContentPlaceHolder1_txtAdditionalRemarks").val("");
            clearUserDetailsControl();

            return false;
        }
        function OnLoadDetailGridInformationFailed(error) {
            $("#<%=EditId.ClientID %>").val("");
            toastr.error(error.get_message());
        }
        function LoadDetailGridInformation() {
            //Guest Detail
            var txtTitle = $("#<%=ddlGuestTitle.ClientID %>").val();
            var txtTitleText = $("#<%=ddlGuestTitle.ClientID %> option:selected").text();
            //if (txtTitle == "MrNMrs.") {
            //    txtTitle = "Mr. & Mrs.";
            //}
            //else 
            if (txtTitle == "N/A") {
                txtTitleText = "";
            }
            //else {
            //        txtTitle = txtTitleText;
            //     }
            var txtFirstName = $("#<%=txtGuestFirstName.ClientID %>").val().trim();
            var txtLastName = $("#<%=txtGuestLastName.ClientID %>").val().trim();
            var txtGuestName = $("#<%=txtGuestName.ClientID %>").val().trim();
            var txtGuestEmail = $("#<%=txtGuestEmail.ClientID %>").val();
            var hiddenGuestId = $("#<%=hiddenGuestId.ClientID %>").val();
            var txtGuestDrivinlgLicense = $("#<%=txtGuestDrivinlgLicense.ClientID %>").val();
            var txtGuestDOB = $("#<%=txtGuestDOB.ClientID %>").val();

            var txtGuestAddress1 = $("#<%=txtGuestAddress1.ClientID %>").val();
            var txtGuestAddress2 = $("#<%=txtGuestAddress2.ClientID %>").val();
            var ddlProfessionId = $("#<%=ddlProfessionId.ClientID %>").val();
            var txtGuestCity = $("#<%=txtGuestCity.ClientID %>").val();
            var ddlGuestCountry = $("#<%=ddlGuestCountry.ClientID %>").val();
            var txtGuestNationality = $("#<%=txtGuestNationality.ClientID %>").val();
            var txtGuestCountrySearch = $.trim($("#txtGuestCountrySearch").val());

            var guestClassificationId = $("#<%=ddlClassificationId.ClientID %>").val() == null ? 0 : $("#<%=ddlClassificationId.ClientID %>").val();
            var txtGuestPhone = $("#<%=txtGuestPhone.ClientID %>").val();

            var ddlGuestSex = $("#<%=ddlGuestSex.ClientID %>").val();
            var txtGuestZipCode = $("#<%=txtGuestZipCode.ClientID %>").val();
            var txtNationalId = $("#<%=txtNationalId.ClientID %>").val();
            var txtPassportNumber = $("#<%=txtPassportNumber.ClientID %>").val();
            var txtPExpireDate = $("#<%=txtPExpireDate.ClientID %>").val();
            var txtPIssueDate = $("#<%=txtPIssueDate.ClientID %>").val();
            var txtPIssuePlace = '';
            var txtVExpireDate = $("#<%=txtVExpireDate.ClientID %>").val();
            var txtVisaNumber = $("#<%=txtVisaNumber.ClientID %>").val();
            var txtVIssueDate = $("#<%=txtVIssueDate.ClientID %>").val();
            var txtNumberOfPersonAdult = $("#<%=txtNumberOfPersonAdult.ClientID %>").val();
            var roomId = $("#<%=ddlRoomNumber.ClientID %>").val();
            if (roomId == "" || roomId == null) {
                roomId = 0;
            }
            var prevGuestId = $("#<%=hfPrevGuestId.ClientID %>").val();
            if (prevGuestId == "") {
                prevGuestId = 0;
            }

            var txtGuestCountrySearch = $.trim($("#txtGuestCountrySearch").val());

            if (txtTitle == "0") {
                toastr.warning("Please Select Title.");
                $("#ddlGuestTitle").focus();
                return;
            }
            //if (txtGuestEmail != "") {
            //    var mailformat = new RegExp('^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$', 'i');
            //    if (txtGuestEmail.match(mailformat)) {
            //    }
            //    else {
            //        toastr.warning("You have entered an invalid email address!");
            //        $("#txtGuestEmail").focus();
            //        return;
            //    }
            //}
            if (txtGuestPhone != "") {
                var phnformat = /[1-9](?:\d{0,2})(?:,\d{3})*(?:\.\d*[1-9])?|0?\.\d*[1-9]|0/;
                if (txtGuestPhone.match(phnformat)) {
                }
                else {
                    toastr.warning("You have entered an invalid Phone Number!");
                    return;
                }
            }

            if ($("#<%=ddlGuestSex.ClientID%>").val() == "0") {
                toastr.warning("Please Enter Valid Gender.");
                return false;
            }

            var enteredCountry = $.trim($("#<%=ddlGuestCountry.ClientID %>").find('option:selected').text());
            if (enteredCountry.toString() != txtGuestCountrySearch.toString()) {
                toastr.warning('Please Enter Valid Country Name.');
                $("#txtGuestCountrySearch").focus();
                return;
            }

            if ($("#ContentPlaceHolder1_txtGuestName").val() != "") {
                if ($("#ContentPlaceHolder1_txtGuestFirstName").val() == "") {
                    toastr.warning("Please Enter Guest First Name.");
                    return;
                }
            }
            else {
                if ($("#ContentPlaceHolder1_txtGuestFirstName").val() == "") {
                    toastr.warning("Please Enter Guest First Name.");
                    return;
                }
            }

            if (txtGuestName == "" || txtNumberOfPersonAdult == "") {
                toastr.warning('Please fill mandatory fields.');
            }
            if (txtGuestName == "") {
                toastr.warning('Please fill mandatory fields.');
            }
            else {
                // Document Detail
                var isEdit = "";
                if ($("#<%=EditId.ClientID %>").val() == "") {
                    isEdit = "";
                }
                else {
                    isEdit = $("#<%=EditId.ClientID %>").val();
                }
                $('#btnAddGuest').val('Add');

                var hfReservationIdTemp = $("#<%=hfReservationIdTemp.ClientID %>").val();
                var reservationId = parseInt(hfReservationIdTemp);

                var isBlock = $("#ContentPlaceHolder1_hfIsBlockGuest").val();
                var additionalRemarks = "";
                additionalRemarks = $("#ContentPlaceHolder1_hfAdditionalRemarks").val();
                if (typeof $("#ContentPlaceHolder1_txtAdditionalRemarks").val() != "undefined") {
                    additionalRemarks = $("#ContentPlaceHolder1_txtAdditionalRemarks").val();
                }
                if (isBlock == "true") {
                    //toastr.info("2nd Okay");
                    toastr.warning("Your entered Guest is blocked for this Hotel");
                    return;
                }
                //else {
                //     toastr.info("Not Okay");
                //}                

                PageMethods.SaveGuestInformationAsDetail(reservationId, prevGuestId, isEdit, 0, txtTitle, txtFirstName, txtLastName, txtGuestName, txtGuestEmail, hiddenGuestId, txtGuestDrivinlgLicense, txtGuestDOB, txtGuestAddress1, txtGuestAddress2, ddlProfessionId, txtGuestCity, ddlGuestCountry, txtGuestNationality, txtGuestPhone, ddlGuestSex, txtGuestZipCode, txtNationalId, txtPassportNumber, txtPExpireDate, txtPIssueDate, txtPIssuePlace, txtVExpireDate, txtVisaNumber, txtVIssueDate, roomId, SelectdPreferenceId, guestClassificationId, additionalRemarks, OnLoadDetailGridInformationSucceeded, OnLoadDetailGridInformationFailed);
                return false;
            }

            //var isBlock = $("#ContentPlaceHolder1_hfIsBlockGuest").val();

            //if (isBlock == "true") {
            //    toastr.info("3rd hi");
            //    $("#chkYesBlock").prop("checked", true);
            //}
            //else {
            //    toastr.info("hello");
            //    $("#chkYesBlock").prop("checked", false);
            //}
        }
        function clearUserDetailsControl() {
            $("#<%=ddlGuestTitle.ClientID %>").val("0");
            $("#<%=ddlTitle.ClientID %>").val("0");
            $("#<%=txtGuestFirstName.ClientID %>").val('');
            $("#<%=txtFirstName.ClientID %>").val('');
            $("#<%=txtGuestLastName.ClientID %>").val('');
            $("#<%=txtGuestName.ClientID %>").val('');
            $("#<%=txtGuestEmail.ClientID %>").val('');
            $("#<%=hiddenGuestId.ClientID %>").val('0');
            $("#<%=txtGuestDrivinlgLicense.ClientID %>").val('');
            $("#<%=txtGuestDOB.ClientID %>").val('');
            $("#<%=txtGuestAddress1.ClientID %>").val('');
            $("#<%=txtGuestAddress2.ClientID %>").val('');
            $("#<%=ddlProfessionId.ClientID %>").val('0');
            $("#<%=txtGuestCity.ClientID %>").val('');
            $("#<%=ddlGuestCountry.ClientID %>").val('0');
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
            $("#<%=ddlClassificationId.ClientID %>").val('0');
            $("#<%=txtVIssueDate.ClientID %>").val('');
            $("#<%=lblGstPreference.ClientID %>").text('');
            $("#GuestPreferenceDiv").hide();
            $("#txtGuestCountrySearch").val("");

            $("#<%=hfIsBlockGuest.ClientID %>").val('');
            $("#chkYesBlock").prop("checked", false);
        }
        function PerformEditActionForGuestDetail(GuestId) {
            $("#<%=EditId.ClientID %>").val(GuestId);
            $('#btnAddGuest').val('Update');
            PageMethods.PerformEditActionForGuestDetailByWM(GuestId, OnEditGuestInformationSucceeded, OnEditGuestInformationFailed);
            return false;
        }
        function OnEditGuestInformationSucceeded(result) {

            $("#<%=ddlGuestTitle.ClientID %>").val(result.Title);
            $("#<%=txtGuestFirstName.ClientID %>").val(result.FirstName);
            $("#<%=txtGuestLastName.ClientID %>").val(result.LastName);
            $("#<%=txtGuestName.ClientID %>").val(result.GuestName);
            var date = new Date(result.GuestDOB);

            var shortDate = "";
            if (!result.GuestDOB) {
                shortDate = "";
            }
            else {
                shortDate = GetStringFromDateTime(date);
            }
            $("#<%=txtGuestDOB.ClientID %>").val(shortDate);
            $("#<%=ddlGuestSex.ClientID %>").val(result.GuestSex);
            $("#<%=txtGuestAddress1.ClientID %>").val(result.GuestAddress1);
            $("#<%=txtGuestAddress2.ClientID %>").val(result.GuestAddress2);
            $("#<%=txtGuestEmail.ClientID %>").val(result.GuestEmail);
            $("#<%=txtGuestPhone.ClientID %>").val(result.GuestPhone);
            $("#<%=ddlProfessionId.ClientID %>").val(result.ProfessionId);
            $("#<%=txtGuestCity.ClientID %>").val(result.GuestCity);
            $("#<%=txtGuestZipCode.ClientID %>").val(result.GuestZipCode);

            $("#<%=txtGuestNationality.ClientID %>").val(result.GuestNationality);
            $("#<%=txtGuestDrivinlgLicense.ClientID %>").val(result.GuestDrivinlgLicense);
            $("#<%=txtNationalId.ClientID %>").val(result.NationalId);
            $("#<%=txtVisaNumber.ClientID %>").val(result.VisaNumber);

            $("#<%=ddlClassificationId.ClientID %>").val(result.ClassificationId);

            var dateVIssue = new Date(result.VIssueDate);
            var shortDateVIssue = "";
            if (!result.VIssueDate) {
                shortDateVIssue = "";
            }
            else {
                shortDateVIssue = GetStringFromDateTime(dateVIssue);
            }

            $("#<%=txtVIssueDate.ClientID %>").val(shortDateVIssue);
            var dateVExpire = new Date(result.VExpireDate);
            var shortDateVExpire = "";
            if (!result.VExpireDate) {
                shortDateVExpire = "";
            }
            else {
                shortDateVExpire = GetStringFromDateTime(dateVExpire);
            }

            $("#<%=txtVExpireDate.ClientID %>").val(shortDateVExpire);
            $("#<%=txtPassportNumber.ClientID %>").val(result.PassportNumber);

            var datePIssue = new Date(result.PIssueDate);
            var shortDatePIssue = "";
            if (!result.PIssueDate) {
                shortDatePIssue = "";
            }
            else {
                shortDatePIssue = GetStringFromDateTime(datePIssue);
            }

            $("#<%=txtPIssueDate.ClientID %>").val(shortDatePIssue);

            var datePExpire = new Date(result.PExpireDate);
            var shortDatePExpire = "";
            if (!result.PExpireDate) {
                shortDatePExpire = "";
            }
            else {
                shortDatePExpire = GetStringFromDateTime(datePExpire);
            }
            $("#<%=txtPExpireDate.ClientID %>").val(shortDatePExpire);
            $("#txtGuestCountrySearch").val(result.CountryName);
            $("#<%=ddlGuestCountry.ClientID %>").val(result.GuestCountryId);
            $("#<%=ddlRoomNumber.ClientID %>").val(result.RoomId);
            if (result.RoomId > 0) {
                $("#RoomNumberDiv").show();
            }
            var guestPreferences = "";
            if (result.GuestPreferences != null)
                guestPreferences += result.GuestPreferences;
            if (result.AdditionalRemarks != "")
                guestPreferences += (guestPreferences != "" ? ", " : "") + result.AdditionalRemarks;
            $("#<%=lblGstPreference.ClientID %>").text(guestPreferences);

            SelectdPreferenceId = result.GuestPreferenceId != null ? result.GuestPreferenceId : "";
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);
            $("#ContentPlaceHolder1_hfAdditionalRemarks").val(result.AdditionalRemarks);
            if (typeof $("#ContentPlaceHolder1_txtAdditionalRemarks").val() !== "undefined") {
                $("#ContentPlaceHolder1_txtAdditionalRemarks").val(result.AdditionalRemarks);
            }
            if (guestPreferences != "") {
                $("#GuestPreferenceDiv").show();
            }
            else $("#GuestPreferenceDiv").hide();
            return false;
        }
        function OnEditGuestInformationFailed(error) {
            toastr.error(error.get_message());
        }
        function PerformDeleteActionForGuestDetail(GuestId) {
            $("#<%=EditId.ClientID %>").val("")
            PageMethods.PerformDeleteActionForGuestDetailByWM(GuestId, OnLoadDetailGridInformationSucceeded, OnLoadDetailGridInformationFailed);
            return false;
        }

        function LoadPrevGuestInfo(guestId) {
            PageMethods.LoadPrevGuestInfo(guestId, OnLoadPrevGuestInfoSucceeded, OnLoadPrevGuestInfoFailed);
            return false;
        }
        function OnLoadPrevGuestInfoSucceeded(result) {
            //popup(-1);
            $("#ReservationPopup").dialog("close");
            $("#<%=hfPrevGuestId.ClientID %>").val(result.GuestId);

            $("#<%=ddlTitle.ClientID %>").val(result.Title);
            $("#<%=txtFirstName.ClientID %>").val(result.FirstName);
            $("#<%=txtLastName.ClientID %>").val(result.LastName);
            $("#<%=txtEmail.ClientID %>").val(result.GuestEmail);
            document.getElementById("txtCountry").value = result.CountryName;
            $("#<%=ddlCountry.ClientID %>").val(result.GuestCountryId);
            $("#<%=txtPhone.ClientID %>").val(result.GuestPhone);
            $("#<%=txtGuestDOB.ClientID %>").val(result.ShowDOB);
            $("#<%=txtGuestAddress1.ClientID %>").val(result.GuestAddress1);
            $("#<%=txtGuestAddress2.ClientID %>").val(result.GuestAddress2);
            $("#<%=ddlProfessionId.ClientID %>").val(result.ProfessionId);
            $("#<%=txtGuestCity.ClientID %>").val(result.GuestCity);
            $("#<%=txtGuestZipCode.ClientID %>").val(result.GuestZipCode);
            $("#<%=txtGuestDrivinlgLicense.ClientID %>").val(result.GuestDrivinlgLicense);
            $("#<%=txtNationalId.ClientID %>").val(result.NationalId);
            $("#<%=txtVisaNumber.ClientID %>").val(result.VisaNumber);
            $("#<%=ddlClassificationId.ClientID %>").val(result.ClassificationId);
            $("#<%=txtVIssueDate.ClientID %>").val(result.ShowVIssueDate);
            $("#<%=txtVExpireDate.ClientID %>").val(result.ShowVExpireDate);
            $("#<%=txtPassportNumber.ClientID %>").val(result.PassportNumber);
            $("#<%=txtPIssueDate.ClientID %>").val(result.ShowPIssueDate);
            $("#<%=txtPExpireDate.ClientID %>").val(result.ShowPExpireDate);
            $("#<%=hfIsBlockGuest.ClientID %>").val(result.GuestBlock);
            var guestPreferences = "";
            if (result.GuestPreferences != null)
                guestPreferences += result.GuestPreferences;
            if (result.AdditionalRemarks != "")
                guestPreferences += (guestPreferences != "" ? ", " : "") + result.AdditionalRemarks;
            $("#<%=lblGstPreference.ClientID %>").text(guestPreferences);
            SelectdPreferenceId = result.GuestPreferenceId != null ? result.GuestPreferenceId : "";
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);
            $("#ContentPlaceHolder1_hfAdditionalRemarks").val(result.AdditionalRemarks);
            if (typeof $("#ContentPlaceHolder1_txtAdditionalRemarks").val() !== "undefined") {
                $("#ContentPlaceHolder1_txtAdditionalRemarks").val(result.AdditionalRemarks);
            }
            if (guestPreferences != "") {
                $("#GuestPreferenceDiv").show();
            }
            else $("#GuestPreferenceDiv").hide();
            var isBlock = $("#ContentPlaceHolder1_hfIsBlockGuest").val();

            if (isBlock == "true") {
                //toastr.info("hi");
                $("#chkYesBlock").prop("checked", true);
            }
            else {
                //toastr.info("hello");
                $("#chkYesBlock").prop("checked", false);
            }

        }
        function OnLoadPrevGuestInfoFailed(error) {
        }

        function ValidateGuestNumber() {
            document.getElementById("ContentPlaceHolder1_btnSave").disabled = true;
            var dateIn = "", dateOut = "";
            dateIn = $("#ContentPlaceHolder1_txtDateIn").val();
            dateOut = $("#ContentPlaceHolder1_txtDateOut").val();

            dateIn = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(dateIn, innBoarDateFormat);
            dateOut = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(dateOut, innBoarDateFormat);

            if ($("#ContentPlaceHolder1_txtDateIn").val() == "") {
                toastr.warning("Please Provide Check In Date.");
                $("#ContentPlaceHolder1_txtDateIn").focus();
                document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtDateOut").val() == "") {
                toastr.warning("Please Provide Expected Check Out Date.");
                $("#ContentPlaceHolder1_txtDateOut").focus();
                document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                return false;
            }

            var validationMobileNumber = _.findWhere(MandaoryFieldsList, { FieldId: "txtMobileNumber" });
            if (validationMobileNumber != null) {
                var mobileNumberValue = $("#ContentPlaceHolder1_txtMobileNumber").val();
                if (mobileNumberValue == "") {
                    toastr.warning("Please Provide Mobile Number");
                    $("#ContentPlaceHolder1_MobileNumber").focus();
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
            }

            var rowCount = $('#ReservedGuestInformation tbody tr').length;
            if (rowCount == 0) {
                if ($("#<%=txtFirstName.ClientID %>").val() == "") {
                    var answer = confirm("No Guest Added, Do you want to continue the Reservation Process ?")
                    if (!answer) {
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return false;
                    }
                }
            }

            var validationMarketSegment = _.findWhere(MandaoryFieldsList, { FieldId: "ddlMarketSegment" });
            if (validationMarketSegment != null) {
                var marketSegmentValue = $("#ContentPlaceHolder1_ddlMarketSegment").val();
                if (marketSegmentValue == '0') {
                    toastr.warning("Please Select Market Segment");
                    $("#ContentPlaceHolder1_ddlMarketSegment").focus();
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
            }

            var validationGuestSource = _.findWhere(MandaoryFieldsList, { FieldId: "ddlGuestSource" });
            if (validationGuestSource != null) {
                var guestSourceValue = $("#ContentPlaceHolder1_ddlGuestSource").val();
                if (guestSourceValue == '0') {
                    toastr.warning("Please Select Guest Source");
                    $("#ContentPlaceHolder1_ddlGuestSource").focus();
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
            }

            var validationReference = _.findWhere(MandaoryFieldsList, { FieldId: "ddlReferenceId" });
            if (validationReference != null) {
                var referenceValue = $("#ContentPlaceHolder1_ddlReferenceId").val();
                if (referenceValue == "0") {
                    toastr.warning("Please Provide Reference");
                    $("#ContentPlaceHolder1_ddlReferenceId").focus();
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
            }

            var validationBookersName = _.findWhere(MandaoryFieldsList, { FieldId: "txtBookersName" });
            if (validationBookersName != null) {
                var bookersNameValue = $("#ContentPlaceHolder1_txtBookersName").val();
                if (bookersNameValue == "") {
                    toastr.warning("Please Provide Bookers Name");
                    $("#ContentPlaceHolder1_ddlMarketSegment").focus();
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
            }

            var reservationId = "0", reservationNumber = "", reservationDate = "", probableArrivalTime = "", probableDepartureTime = "", confirmationDate = "",
                reservedCompany = "", guestId = "", contactAddress = "", contactPerson = "", contactNumber = "", mobileNumber = "", faxNumber = "", contactEmail = "",
                totalRoomNumber = "0", reservedMode = "", reservationType = "", reservationMode = "", pendingDeadline = "", isListedCompany = false, companyId = "", isListedContact = false, contactId = "0",
                businessPromotionId = "", referenceId = "", paymentMode = "", payFor = "", currencyType = "", conversionRate = "", reason = "", remarks = "", guestremarks = "", posRemarks = "",
                numberOfPersonAdult = "", numberOfPersonChild = "", isFamilyOrCouple = "", airportPickUp = "", airportDrop = "", isAirportPickupDropExist = false,
                reservationTempId = "0", reservationDuration = "", gId = 0, roomType = "", roomNumbers = "", displayRoomNumberNType = "", marketSegmentId = "", guestSourceId = "",
                isRoomRateShowInPreRegistrationCard = false;

            var arrivalAirlineId = 0, departureAirlineId = 0, arrivalFlightName = "", arrivalFlightNumber = "", arrivalTime = "", departureFlightName = "", departureFlightNumber = "", departureTime = "", classificationId = 0;
            var IsArrivalChargable = false, IsDepartureChargable = false;

            var apdId = '', APId = '', ADId = '', reservId = '', gstId = '', guestName = '', fligtName = '', flightNo = '', arrivtime = '';
            var AddedAirportPickupDrop = new Array();
            var AireportPickupDrop = new Array();

            var mealPlanId = 0, isVIPGuest = 0, vipGuestTypeId = 0, isComplementaryGuest = false, classification = "", bookersName = "";

            reservationId = $("#ContentPlaceHolder1_hfReservationId").val();
            reservationTempId = $("#ContentPlaceHolder1_hfReservationIdTemp").val();
            APId = $("#ContentPlaceHolder1_hfAPId").val();
            ADId = $("#ContentPlaceHolder1_hfADId").val();
            if (APId == "") {
                APId = "0";
            }
            if (ADId == "") {
                ADId = "0";
            }
            if (reservationId == "") {
                reservationId = "0";
            }
            if (reservationTempId == "") {
                reservationTempId = "0";
            }

            reservationDuration = $("#txtReservationDuration").val();
            probableArrivalTime = $("#ContentPlaceHolder1_txtProbableArrivalTime").val();
            probableDepartureTime = $("#ContentPlaceHolder1_txtProbableDepartureTime").val();
            reservedMode = $("#ContentPlaceHolder1_ddlReservedMode").val();
            reservationType = $("#ContentPlaceHolder1_ddlReservationType").val();
            contactAddress = $("#ContentPlaceHolder1_txtContactAddress").val();
            contactPerson = $("#ContentPlaceHolder1_txtContactPerson").val();
            contactNumber = $("#ContentPlaceHolder1_txtPhone").val();
            mobileNumber = $("#ContentPlaceHolder1_txtMobileNumber").val();
            faxNumber = $("#ContentPlaceHolder1_txtFaxNumber").val();
            contactEmail = $("#ContentPlaceHolder1_txtContactEmail").val();
            currencyType = $("#ContentPlaceHolder1_ddlCurrency").val();
            payFor = $("#ContentPlaceHolder1_ddlPayFor").val();
            businessPromotionId = $("#ContentPlaceHolder1_ddlBusinessPromotionId").val();
            referenceId = $("#ContentPlaceHolder1_ddlReferenceId").val();
            conversionRate = $("#ContentPlaceHolder1_txtConversionRate").val() == "" ? "0" : $("#ContentPlaceHolder1_txtConversionRate").val();
            numberOfPersonAdult = $("#ContentPlaceHolder1_txtNumberOfPersonAdult").val() == "" ? "0" : $("#ContentPlaceHolder1_txtNumberOfPersonAdult").val();
            isFamilyOrCouple = $("#ContentPlaceHolder1_cbFamilyOrCouple").is(":checked");

            isRoomRateShowInPreRegistrationCard = $("#ContentPlaceHolder1_chkIsRoomRateShowInPreRegistrationCard").is(":checked");

            numberOfPersonChild = $("#ContentPlaceHolder1_txtNumberOfPersonChild").val() == "" ? "0" : $("#ContentPlaceHolder1_txtNumberOfPersonChild").val();
            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            guestremarks = $("#ContentPlaceHolder1_txtGuestRemarks").val();
            posRemarks = $("#ContentPlaceHolder1_txtPosRemarks").val();


            marketSegmentId = $("#ContentPlaceHolder1_ddlMarketSegment").val();
            guestSourceId = $("#ContentPlaceHolder1_ddlGuestSource").val();

            airportPickUp = $("#ContentPlaceHolder1_ddlAirportPickUp").val();
            airportDrop = $("#ContentPlaceHolder1_ddlAirportDrop").val();

            classification = $("#ContentPlaceHolder1_ddlClassification").val();

            var validRefId = CommonHelper.IsInt(referenceId);
            if (validRefId == false) {
                toastr.warning("Please select Listed referance.");
                $("#ContentPlaceHolder1_ddlReferenceId").focus();
                document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                return false;
            }
            if (classification == 453) {
                if ($("#ContentPlaceHolder1_ddlVIPGuestType").val() == 0) {
                    $("#ContentPlaceHolder1_ddlVIPGuestType").focus();
                    toastr.warning("Please Provide VIP Type.");
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
            }

            if (numberOfPersonAdult == "0") {
                toastr.warning("Adult person can not be less than 1.");
                document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                return false;
            }
            else if (reservationDuration == "") {
                toastr.warning("Please Provide Valid Number For Number of Nights.");
                document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                return false;
            }
            else if (probableArrivalTime == "") {
                toastr.warning("Please Provide Probable Arrival Time.");
                document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                return false;
            }
            else if (contactPerson == "" && contactNumber == "" && mobileNumber == "") {
                toastr.warning("Please Provide Contact Person/ Telephone Number/ mobile number.");
                document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                return false;
            }

            var txtGuestCountrySearch = $.trim($("#txtCountry").val());
            var enteredCountry = $.trim($("#<%=ddlCountry.ClientID %>").find('option:selected').text());

            if (txtGuestCountrySearch != '') {
                if (enteredCountry.toString() != txtGuestCountrySearch.toString()) {
                    toastr.warning('Please Enter Valid Country Name.');
                    $("#txtGuestCountrySearch").focus();
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
            }

            var txtContactEmail = $("#<%=txtContactEmail.ClientID %>").val();
            if (txtContactEmail != "") {
                var mailformat = new RegExp('^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$', 'i');
                if (txtContactEmail.match(mailformat)) {
                }
                else {
                    toastr.warning("You have entered an invalid email address!", "", { timeOut: 10000 });
                    $("#ContentPlaceHolder1_txtContactEmail").focus();
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
            }

            if (reservedMode == "--- Please Select ---") {
                toastr.warning("Please Select Reservation Mode.");
                document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                return false;
            }

            if (reservedMode == "Company") {
                if ($("#ContentPlaceHolder1_chkIsLitedCompany").is(":checked")) {
                    var companyId = $("#ContentPlaceHolder1_ddlCompanyName").val();
                    if (companyId == "0") {
                        toastr.warning("Please Select Company Name.");
                        $("#ContentPlaceHolder1_ddlCompanyName").focus();
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return false;
                    }

                    if ($("#ContentPlaceHolder1_chkIsLitedContact").is(":checked")) {
                        isListedContact = true;
                        contactId = $("#ContentPlaceHolder1_hfContactId").val();
                        if ($("#ContentPlaceHolder1_txtListedContactPerson").val() == "") {
                            toastr.warning("Please Provide Contact Person.");
                            $("#ContentPlaceHolder1_txtListedContactPerson").focus();
                            document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                            return false;
                        }
                    }
                    else {
                        isListedContact = false;
                        contactId = "0";

                        if ($("#ContentPlaceHolder1_txtContactPerson").val() == "") {
                            toastr.warning("Please Provide Contact Person.");
                            $("#ContentPlaceHolder1_txtContactPerson").focus();
                            document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                            return false;
                        }
                    }

                }
                else {
                    if ($("#ContentPlaceHolder1_txtReservedCompany").val() == "") {
                        toastr.warning("Please Provide Company Name.");
                        $("#ContentPlaceHolder1_txtReservedCompany").focus();
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return false;
                    }

                    if ($("#ContentPlaceHolder1_chkIsLitedContact").is(":checked")) {
                        isListedContact = true;
                        contactId = $("#ContentPlaceHolder1_hfContactId").val();
                        if ($("#ContentPlaceHolder1_txtListedContactPerson").val() == "") {
                            toastr.warning("Please Provide Contact Person.");
                            $("#ContentPlaceHolder1_txtListedContactPerson").focus();
                            document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                            return false;
                        }
                    }
                    else {
                        isListedContact = false;
                        contactId = "0";

                        if ($("#ContentPlaceHolder1_txtContactPerson").val() == "") {
                            toastr.warning("Please Provide Contact Person.");
                            $("#ContentPlaceHolder1_txtContactPerson").focus();
                            document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                            return false;
                        }
                    }
                }
            }
            else if (reservedMode == "Self") {
                if ($("#ContentPlaceHolder1_chkIsLitedContact").is(":checked")) {
                    isListedContact = true;
                    contactId = $("#ContentPlaceHolder1_hfContactId").val();
                    if ($("#ContentPlaceHolder1_txtListedContactPerson").val() == "") {
                        toastr.warning("Please Provide Contact Person.");
                        $("#ContentPlaceHolder1_txtListedContactPerson").focus();
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return false;
                    }
                }
                else {
                    isListedContact = false;
                    contactId = "0";

                    if ($("#ContentPlaceHolder1_txtContactPerson").val() == "") {
                        toastr.warning("Please Provide Contact Person.");
                        $("#ContentPlaceHolder1_txtContactPerson").focus();
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return false;
                    }
                }

                //if ($("#ContentPlaceHolder1_txtMobileNumber").val() == "") {
                //    toastr.warning("Please Provide Mobile Number.");
                //    $("#ContentPlaceHolder1_txtMobileNumber").focus();
                //    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                //    return false;
                //}
            }
            else if (reservedMode == "Group") {
                //if ($("#ContentPlaceHolder1_txtGroupName").val() == "") {
                var groupMasterId = $("#ContentPlaceHolder1_ddlGroupName").val();
                if (groupMasterId == "0") {
                    toastr.warning("Please Select Group Name.");
                    //$("#ContentPlaceHolder1_txtGroupName").focus();
                    $("#ContentPlaceHolder1_ddlGroupName").focus();
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
                if ($("#ContentPlaceHolder1_txtContactPerson").val() == "") {
                    toastr.warning("Please Provide Contact Person.");
                    $("#ContentPlaceHolder1_txtContactPerson").focus();
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
            }

            if (airportPickUp == "0") {
                $("#myTabs").tabs({ active: 2 });
                toastr.warning("Please Select Pick Up.");
                $("#ContentPlaceHolder1_ddlAirportPickUp").focus();
                document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                return false;
            }
            if (airportPickUp == "YES") {
                var pickuptablerowLength = $("#PickupTable tbody tr").length;
                if (pickuptablerowLength == 0) {
                    if ($("#ContentPlaceHolder1_ddlArrivalFlightName").val() == "0") {
                        toastr.warning("Please Provide Arrival Vehicle Name.");
                        $("#ContentPlaceHolder1_ddlArrivalFlightName").focus();
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return false;
                    }
                    var checkAirName = CommonHelper.IsInt($("#ContentPlaceHolder1_ddlArrivalFlightName").val());

                    if (checkAirName == false) {
                        toastr.warning("Please Provide Arrival Vehicle Name.");
                        $("#ContentPlaceHolder1_ddlArrivalFlightName").focus();
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return false;
                    }

                    //if ($("#ContentPlaceHolder1_txtArrivalFlightNumber").val() == "") {
                    //    toastr.warning("Please Provide Arrival Flight Number.");
                    //    $("#ContentPlaceHolder1_txtArrivalFlightNumber").focus();
                    //    return false;
                    //}

                    //if ($("#ContentPlaceHolder1_txtArrivalTime").val() == "") {
                    //    toastr.warning("Please Provide Arrival Time (ETA).");
                    //    $("#ContentPlaceHolder1_txtArrivalTime").focus();
                    //    return false;
                    //}
                }
            }

            var currency = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currency != "Local") {
                if ($("#ContentPlaceHolder1_ddlCurrency").val() == "0") {
                    toastr.warning("Please Select Currency Type.");
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtConversionRate").val() == "" || $("#ContentPlaceHolder1_txtConversionRate").val() == "0") {
                    toastr.warning("Please Provide Conversion Rate.");
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
            }

            if ($("#ReservationRoomGrid tbody tr").length == 0) {
                toastr.warning("Please add at least one room.");
                document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                return false;
            }

            if (airportDrop == "YES") {
                var departuretablerowLength = $("#DepartureTable tbody tr").length;
                if (departuretablerowLength == 0) {
                    var checkAirName = CommonHelper.IsInt($("#ContentPlaceHolder1_ddlDepartureFlightName").val());

                    if (checkAirName == false) {
                        toastr.warning("Please Provide Departure Flight Name");
                        $("#ContentPlaceHolder1_ddlDepartureFlightName").focus();
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return false;
                    }
                }
            }

            if (airportPickUp == "YES" || airportPickUp == "TBA") {
                isAirportPickupDropExist = true;

                var ctrl = '#<%=chkMultiplePickup.ClientID%>'
                if ($(ctrl).is(':checked')) {
                    $("#PickupTable tbody tr").each(function () {
                        apdId = $(this).find("td:eq(0)").text();
                        reservId = $(this).find("td:eq(1)").text();
                        gstId = $(this).find("td:eq(2)").text();
                        guestName = $(this).find("td:eq(3)").text();
                        fligtName = $(this).find("td:eq(4)").text();
                        flightNo = $(this).find("td:eq(5)").text();
                        arrivtime = $(this).find("td:eq(6)").text();
                    });
                    if (APId > "0") {
                        //DeletedAirportPickupDrop.push({
                        //    APDId: APId
                        //});
                    }
                }
            }

            if (airportDrop == "YES" || airportDrop == "TBA") {
                isAirportPickupDropExist = true;

                var ctrl = '#<%=chkMultipleDrop.ClientID%>'
                if ($(ctrl).is(':checked')) {
                    $("#DepartureTable tbody tr").each(function () {
                        apdId = $(this).find("td:eq(0)").text();
                        reservId = $(this).find("td:eq(1)").text();
                        gstId = $(this).find("td:eq(2)").text();
                        guestName = $(this).find("td:eq(3)").text();
                        fligtName = $(this).find("td:eq(4)").text();
                        flightNo = $(this).find("td:eq(5)").text();
                        arrivtime = $(this).find("td:eq(6)").text();
                    });
                    if (ADId > "0") {
                        //DeletedAirportPickupDrop.push({
                        //    APDId: ADId
                        //});
                    }
                }
            }

            var apdId = "0";

            if ($("#ContentPlaceHolder1_hfAPId").val() != "") {
                apdId = $("#ContentPlaceHolder1_hfAPId").val();
            }

            if ($("#ContentPlaceHolder1_ddlArrivalFlightName") != "0") {
                arrivalFlightName = $("#ContentPlaceHolder1_ddlArrivalFlightName").find(":selected").text();
            }
            else {
                arrivalFlightName = "";
            }
            arrivalAirlineId = $("#ContentPlaceHolder1_ddlArrivalFlightName").val();
            arrivalFlightNumber = $("#ContentPlaceHolder1_txtArrivalFlightNumber").val();
            arrivalTime = $("#ContentPlaceHolder1_txtArrivalTime").val();
            if ($("#ContentPlaceHolder1_ddlDepartureFlightName") != "0") {
                departureFlightName = $("#ContentPlaceHolder1_ddlDepartureFlightName").find(":selected").text();
            }
            else {
                departureFlightName = "";
            }
            departureAirlineId = $("#ContentPlaceHolder1_ddlDepartureFlightName").val();
            departureFlightNumber = $("#ContentPlaceHolder1_txtDepartureFlightNumber").val();
            departureTime = $("#ContentPlaceHolder1_txtDepartureTime").val();

            if (($("#<%=ddlAirportPickUp.ClientID %>").val() == "YES") || ($("#<%=ddlAirportPickUp.ClientID %>").val() == "TBA")) {
                if ($("#ContentPlaceHolder1_chkIsArrivalChargable").is(":checked")) {
                    IsArrivalChargable = true;
                }
            }

            if (($("#<%=ddlAirportDrop.ClientID %>").val() == "YES") || ($("#<%=ddlAirportDrop.ClientID %>").val() == "TBA")) {
                if ($("#ContentPlaceHolder1_chkIsDepartureChargable").is(":checked")) {
                    IsDepartureChargable = true;
                }
            }

            AireportPickupDrop.push({
                APDId: apdId,
                ReservationId: reservationId,
                GuestId: gId,
                ArrivalAirlineId: arrivalAirlineId,
                ArrivalFlightName: arrivalFlightName,
                ArrivalFlightNumber: arrivalFlightNumber,
                ArrivalTime: arrivalTime,
                DepartureAirlineId: departureAirlineId,
                DepartureFlightName: departureFlightName,
                DepartureFlightNumber: departureFlightNumber,
                DepartureTime: departureTime,
                PickupDropType: 'PicupDrop',
                IsArrivalChargable: IsArrivalChargable,
                IsDepartureChargable: IsDepartureChargable
            });

            if (reservedMode == "Company") {
                if ($("#ContentPlaceHolder1_chkIsLitedCompany").is(":checked")) {
                    isListedCompany = true;
                    companyId = $("#ContentPlaceHolder1_ddlCompanyName").val();
                    reservedCompany = null;
                    paymentMode = $("#ContentPlaceHolder1_ddlPaymentMode").val();
                    if ($("#ContentPlaceHolder1_chkIsLitedContact").is(":checked")) {
                        isListedContact = true;
                        contactPerson = $("#ContentPlaceHolder1_txtListedContactPerson").val();
                        contactId = $("#ContentPlaceHolder1_hfContactId").val();
                    }
                    else {
                        isListedContact = false;
                        contactId = "0";
                    }
                }
                else {
                    isListedCompany = false;
                    companyId = "0";
                    reservedCompany = $("#ContentPlaceHolder1_txtReservedCompany").val();
                    paymentMode = "Self";
                }
            }
            else if (reservedMode == "Group") {
                isListedCompany = false;
                //companyId = "0";
                companyId = $("#ContentPlaceHolder1_ddlGroupName").val();
                $("#ContentPlaceHolder1_txtGroupName").val($("#<%=ddlGroupName.ClientID %> option:selected").text());
                reservedCompany = $("#ContentPlaceHolder1_txtGroupName").val();
                paymentMode = "Self";
            }
            else {
                isListedCompany = false;
                paymentMode = "Self";
                companyId = "0";
            }

            if ($("#ContentPlaceHolder1_hiddenGuestId").val() == "") {
                guestId = "0";
            }
            else {
                guestId = $("#ContentPlaceHolder1_hiddenGuestId").val();
            }

            if ($("#ContentPlaceHolder1_ddlReservationStatus").val() == "Confirmed") {
                reservationMode = $("#ContentPlaceHolder1_ddlReservationStatus").val();
            }
            else if ($("#ContentPlaceHolder1_ddlReservationStatus").val() == "Waiting") {
                confirmationDate = $("#ContentPlaceHolder1_txtConfirmationDate").val();
                reservationMode = $("#ContentPlaceHolder1_ddlReservationStatus").val();
            }
            else if ($("#ContentPlaceHolder1_ddlReservationStatus").val() == "Cancel") {
                reservationMode = $("#ContentPlaceHolder1_ddlReservationStatus").val();
                reason = $("#ContentPlaceHolder1_txtReason").val();
            }
            else if ($("#ContentPlaceHolder1_ddlReservationStatus").val() == "Registered") {
                reservationMode = $("#ContentPlaceHolder1_ddlReservationStatus").val();
                reason = $("#ContentPlaceHolder1_txtReason").val();
            }

            if (reservationId == "")
                reservationId = "0";

            var dateInHiddenFieldEdit = $("#ContentPlaceHolder1_DateInHiddenFieldEdit").val();
            var minCheckInDate = $("#ContentPlaceHolder1_hfMinCheckInDate").val();

            classification = $("#ContentPlaceHolder1_ddlClassification").val();
            mealPlanId = $("#ContentPlaceHolder1_ddlMealPlanId").val();
            bookersName = $("#ContentPlaceHolder1_txtBookersName").val();

            isVIPGuest = false;
            vipGuestTypeId = 0;

            if (classification == 453) {
                isVIPGuest = true;
                if ($("#ContentPlaceHolder1_ddlVIPGuestType").val() != 0) {
                    vipGuestTypeId = $("#ContentPlaceHolder1_ddlVIPGuestType").val();
                }

                if ($("#ContentPlaceHolder1_ddlIsComplementary").val() == "1") {
                    isComplementaryGuest = true;
                }
                else {
                    isComplementaryGuest = false;
                }
            }
            if (reservedMode == "Company") {
                if ($("#ContentPlaceHolder1_chkIsLitedCompany").is(":checked")) {
                    if (paymentMode == "0") {
                        toastr.warning("Please Select Payment Mode.");
                        $("#myTabs").tabs({ active: 0 });
                        $("#ContentPlaceHolder1_ddlPaymentMode").focus();
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return false;
                    }
                }
            }

            var RoomReservation = {
                ReservationId: reservationId,
                ReservationTempId: reservationTempId,
                DateIn: dateIn,
                DateOut: dateOut,
                DateInStr: dateIn,
                DateInFieldEdit: dateInHiddenFieldEdit,
                MinCheckInDate: minCheckInDate,
                ProbableArrivalTime: probableArrivalTime,
                ProbableDepartureTime: probableDepartureTime,
                ConfirmationDate: confirmationDate,
                ReservedCompany: reservedCompany,
                GuestId: 0,
                ContactAddress: contactAddress,
                ContactPerson: contactPerson,
                ContactNumber: contactNumber,
                MobileNumber: mobileNumber,
                FaxNumber: faxNumber,
                ContactEmail: contactEmail,
                TotalRoomNumber: totalRoomNumber,
                ReservedMode: reservedMode,
                ReservationType: reservationType,
                ReservationMode: reservationMode,
                PendingDeadline: confirmationDate,
                IsListedCompany: isListedCompany,
                CompanyId: companyId,
                IsListedContact: isListedContact,
                ContactId: contactId,
                BusinessPromotionId: businessPromotionId,
                ReferenceId: referenceId,
                MarketSegmentId: marketSegmentId,
                GuestSourceId: guestSourceId,
                PaymentMode: paymentMode,
                PayFor: payFor,
                CurrencyType: currencyType,
                ConversionRate: conversionRate,
                Reason: reason,
                Remarks: remarks,
                GuestRemarks: guestremarks,
                POSRemarks: posRemarks,
                NumberOfPersonAdult: numberOfPersonAdult,
                NumberOfPersonChild: numberOfPersonChild,
                IsFamilyOrCouple: isFamilyOrCouple,
                IsRoomRateShowInPreRegistrationCard: isRoomRateShowInPreRegistrationCard,
                IsAirportPickupDropExist: (isAirportPickupDropExist == true ? 1 : 0),
                AirportPickUp: airportPickUp,
                AirportDrop: airportDrop,
                ClassificationId: classification,
                MealPlanId: mealPlanId,
                IsVIPGuest: isVIPGuest,
                VipGuestTypeId: vipGuestTypeId,
                IsComplementaryGuest: isComplementaryGuest,
                BookersName: bookersName
            };

            var ComplementaryItem = new Array();

            $("#ContentPlaceHolder1_chkComplementaryItem tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                    ComplementaryItem.push({
                        RCItemId: 0,
                        ReservationId: 0,
                        ComplementaryItemId: $(this).find("td:eq(0)").find("input").val()
                    });
                }
            });

            var PaidServiceDetails = new Array();
            var paidServiceDeleted = false;

            if ($("#ContentPlaceHolder1_hfPaidServiceSaveObj").val() != "")
                PaidServiceDetails = JSON.parse($("#ContentPlaceHolder1_hfPaidServiceSaveObj").val());

            if ($("#ContentPlaceHolder1_hfPaidServiceDeleteObj").val() == "1")
                paidServiceDeleted = true;

            var reservationDetailId = "0", reservationDetailIds = "", roomTypeId = "", roomIds = "", unitPrice = "", discountType = "",
                roomId = "0", discountAmount = "", roomRate = "", totalRoom = 0, i = 0, j = 0, paxQuantity = 0;
            var RoomReservationDetail = new Array();
            var RoomReservationSummary = new Array();

            var cbServiceChargeVal, cbCityChargeVal, cbVatAmountVal, cbAdditionalChargeVal, totalRoomRate;

            $("#ReservationRoomGrid tbody tr").each(function () {
                roomType = $.trim($(this).find("td:eq(0)").text());
                roomNumbers = $.trim($(this).find("td:eq(1)").text());

                if (displayRoomNumberNType != "")
                    displayRoomNumberNType += ", " + roomType + ": " + roomNumbers;
                else
                    displayRoomNumberNType = roomType + ": " + roomNumbers;
                totalRoomRate = $.trim($(this).find("td:eq(2)").text());
                paxQuantity = $.trim($(this).find("td:eq(3)").text());
                roomTypeId = $.trim($(this).find("td:eq(6)").text());
                reservationDetailIds = $.trim($(this).find("td:eq(5)").text());
                roomIds = $.trim($(this).find("td:eq(7)").text());
                discountType = $.trim($(this).find("td:eq(10)").text());
                discountAmount = $.trim($(this).find("td:eq(11)").text());
                unitPrice = $.trim($(this).find("td:eq(12)").text());
                roomRate = $.trim($(this).find("td:eq(13)").text());
                totalRoom = parseInt($.trim($(this).find("td:eq(9)").text()), 10);
                cbAdditionalChargeVal = $.trim($(this).find("td:eq(14)").text());
                cbCityChargeVal = $.trim($(this).find("td:eq(15)").text());
                cbServiceChargeVal = $.trim($(this).find("td:eq(16)").text());
                cbVatAmountVal = $.trim($(this).find("td:eq(17)").text());

                if (cbAdditionalChargeVal == "1") {
                    cbAdditionalChargeVal = true;
                }
                else {
                    cbAdditionalChargeVal = false;
                }

                if (cbCityChargeVal == "1") {
                    cbCityChargeVal = true;
                }
                else {
                    cbCityChargeVal = false;
                }

                if (cbServiceChargeVal == "1") {
                    cbServiceChargeVal = true;
                }
                else {
                    cbServiceChargeVal = false;
                }
                if (cbVatAmountVal == "1") {
                    cbVatAmountVal = true;
                }
                else {
                    cbVatAmountVal = false;
                }


                if (discountAmount == "")
                    discountAmount = "0";

                var roomIdList = roomIds.split(",");
                var reservationRoomIdList = reservationDetailIds.split(",");

                // // // Will Update Later For Reservation Pax Wise---------------------------------Pax Wise Extra Room generated
                //for (i = 0; i < paxQuantity; i++) {
                for (i = 0; i < totalRoom; i++) {
                    roomId = (roomIdList[i] == null ? "0" : roomIdList[i]);
                    reservationDetailId = (reservationRoomIdList[i] == null ? "0" : reservationRoomIdList[i]);

                    roomId = (roomId == "" ? "0" : roomId);
                    reservationDetailId = (reservationDetailId == "" ? "0" : reservationDetailId);

                    RoomReservationDetail.push({
                        ReservationDetailId: reservationDetailId,
                        ReservationId: reservationId,
                        RoomTypeId: roomTypeId,
                        RoomId: roomId,
                        UnitPrice: unitPrice,
                        DiscountType: discountType,
                        DiscountAmount: discountAmount,
                        RoomRate: roomRate,
                        RoomTypeWisePaxQuantity: paxQuantity,
                        IsRegistered: false,
                        IsCityChargeEnable: cbCityChargeVal,
                        IsServiceChargeEnable: cbServiceChargeVal,
                        IsVatAmountEnable: cbVatAmountVal,
                        IsAdditionalChargeEnable: cbAdditionalChargeVal,
                        TotalCalculatedAmount: totalRoomRate
                    });
                }

                RoomReservationSummary.push({
                    ReservationId: reservationId,
                    RoomTypeId: roomTypeId,
                    PaxQuantity: paxQuantity,
                    RoomQuantity: totalRoom
                });

                reservationDetailId = "0";
            });

            var GuestDetails = null;
            var rowLength = $("#ReservedGuestInformation tbody tr").length;
            if (rowLength == 0) {
                if (reservationId > 0) {
                    toastr.warning("Please add atleast one guest.");
                    document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                    return false;
                }
                else {
                    var prevguestId = $("#<%=hfPrevGuestId.ClientID %>").val();
                    if (prevguestId == "") {
                        prevguestId = 0;
                    }

                    //title change
                    var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();
                    var title = $("#<%=ddlTitle.ClientID %>").val();

                    if (title == "0") {
                        toastr.warning("Please select Title.");
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return false;
                    }

                    var firstName = $("#<%=txtFirstName.ClientID %>").val().trim();
                    var lastName = $("#<%=txtLastName.ClientID %>").val().trim();
                    var txtGuestName = "";


                    if (title == "N/A") {
                        txtGuestName = firstName + " " + lastName;
                        titleText = "";
                    }
                    else {
                        txtGuestName = titleText + " " + firstName + " " + lastName;
                    }
                    var txtGuestEmail = $("#<%=txtEmail.ClientID %>").val();
                    var hiddenGuestId = $("#<%=hiddenGuestId.ClientID %>").val();
                    var txtGuestDrivinlgLicense = $("#<%=txtGuestDrivinlgLicense.ClientID %>").val();
                    var txtGuestAddress1 = $("#<%=txtGuestAddress1.ClientID %>").val();
                    var txtGuestAddress2 = $("#<%=txtGuestAddress2.ClientID %>").val();
                    var ddlProfessionId = $("#<%=ddlProfessionId.ClientID %>").val();
                    var txtGuestCity = $("#<%=txtGuestCity.ClientID %>").val();
                    var ddlGuestCountry = $("#<%=ddlCountry.ClientID %>").val();
                    var txtGuestNationality = $("#<%=txtGuestNationality.ClientID %>").val();
                    var txtGuestCountrySearch = $.trim($("#txtGuestCountrySearch").val());
                    var txtGuestPhone = $("#<%=txtPhone.ClientID %>").val();
                    var ddlGuestSex = $("#<%=ddlGuestSex.ClientID %>").val();
                    var txtGuestZipCode = $("#<%=txtGuestZipCode.ClientID %>").val();
                    var txtNationalId = $("#<%=txtNationalId.ClientID %>").val();
                    var txtPassportNumber = $("#<%=txtPassportNumber.ClientID %>").val();
                    var txtPExpireDate = $("#<%=txtPExpireDate.ClientID %>").val();
                    var txtPIssueDate = $("#<%=txtPIssueDate.ClientID %>").val();
                    var txtPIssuePlace = '';
                    var txtVExpireDate = $("#<%=txtVExpireDate.ClientID %>").val();
                    var txtVisaNumber = $("#<%=txtVisaNumber.ClientID %>").val();
                    var txtVIssueDate = $("#<%=txtVIssueDate.ClientID %>").val();
                    var txtGuestDOB = $("#ContentPlaceHolder1_txtGuestDOB").val();

                    var isCopyTo = false;
                    isCopyTo = $("#ContentPlaceHolder1_hfIsCopyTo").val();

                    txtGuestDOB = txtGuestDOB == "" ? null : CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(txtGuestDOB, innBoarDateFormat);
                    txtPIssueDate = txtPIssueDate == "" ? null : CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(txtPIssueDate, innBoarDateFormat);
                    txtPExpireDate = txtPExpireDate == "" ? null : CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(txtPExpireDate, innBoarDateFormat);
                    txtVExpireDate = txtVExpireDate == "" ? null : CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(txtVExpireDate, innBoarDateFormat);
                    txtVIssueDate = txtVIssueDate == "" ? null : CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(txtVIssueDate, innBoarDateFormat);

                    var roomId = $("#ContentPlaceHolder1_hfFstAsndRoomId").val();
                    if (roomId == "") {
                        roomId = 0;
                    }

                    if (txtGuestEmail != "") {
                        var mailformat = new RegExp('^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$', 'i');
                        if (txtGuestEmail.match(mailformat)) {
                        }
                        else {
                            toastr.warning("You have entered an invalid email address!");
                            document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                            return;
                        }
                    }
                    if (txtGuestPhone != "") {
                        var phnformat = /[1-9](?:\d{0,2})(?:,\d{3})*(?:\.\d*[1-9])?|0?\.\d*[1-9]|0/;
                        if (txtGuestPhone.match(phnformat)) {
                        }
                        else {
                            toastr.warning("You have entered an invalid Phone Number!");
                            document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                            return;
                        }
                    }

                    if (firstName == "") {
                        toastr.warning('Please provide guest name.');
                        document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;
                        return;
                    }

                    GuestDetails = {
                        tempOwnerId: reservationTempId,
                        GuestAddress1: txtGuestAddress1,
                        GuestAddress2: txtGuestAddress2,
                        GuestAuthentication: '',
                        ProfessionId: ddlProfessionId,
                        GuestCity: txtGuestCity,
                        GuestCountryId: ddlGuestCountry,
                        GuestDrivinlgLicense: txtGuestDrivinlgLicense,
                        GuestEmail: txtGuestEmail,
                        Title: title,
                        FirstName: firstName,
                        LastName: lastName,
                        GuestName: txtGuestName,
                        GuestDOB: txtGuestDOB,
                        GuestId: hiddenGuestId == "" ? 0 : hiddenGuestId,
                        GuestNationality: txtGuestNationality,
                        GuestPhone: txtGuestPhone,
                        GuestSex: ddlGuestSex,
                        GuestZipCode: txtGuestZipCode,
                        NationalId: txtNationalId,
                        PassportNumber: txtPassportNumber,
                        PExpireDate: txtPExpireDate,
                        PIssueDate: txtPIssueDate,
                        PIssuePlace: txtPIssuePlace,
                        VExpireDate: txtVExpireDate,
                        VisaNumber: txtVisaNumber,
                        VIssueDate: txtVIssueDate,
                        GuestPreferences: SelectdPreferenceId,
                        RoomId: roomId,
                        PreviousGuestId: prevguestId,
                        ClassificationId: classificationId,
                        IsCopyTo: isCopyTo
                    };
                }
            }
            PageMethods.SaveReservation(RoomReservation, GuestDetails, RoomReservationDetail, RoomReservationSummary, AireportPickupDrop, AddedAirportPickupDrop, DeletedAirportPickupDrop, ComplementaryItem,
                PaidServiceDetails, paidServiceDeleted, displayRoomNumberNType, OnSaveReservationSucceed, OnSaveReservationFailed);

            return false;
        }

        function OnSaveReservationSucceed(result) {
            document.getElementById("ContentPlaceHolder1_btnSave").disabled = false;

            if (result.IsSuccess) {
                $("#ContentPlaceHolder1_hfIsSaveRUpdate").val(result.Pk);
                $("#ContentPlaceHolder1_hfAPId").val("");
                $("#ContentPlaceHolder1_hfADId").val("");
                $("#ContentPlaceHolder1_hfPrevGuestId").val("");

                if ($("#ContentPlaceHolder1_btnSave").val() == "Save") {
                    CommonHelper.AlertMessage(result.AlertMessage);
                    $("#ContentPlaceHolder1_hfAPDId").val(result.PKey[0].APDId);
                    PerformBillPreviewAction(result.PKey[0].Pk);
                }

                ClearForm();
            }
            else {
                if (result.IsReservationCheckInDateValidation == false) {
                    $("#ContentPlaceHolder1_hfMinCheckInDate").val($("#ContentPlaceHolder1_DateInHiddenField").val());
                    $("#ContentPlaceHolder1_hfIsSaveRUpdate").val("");
                }

                CommonHelper.AlertMessage(result.AlertMessage);

            }
        }
        function OnSaveReservationFailed(error) { }

        function ClearForm() {
            $("#form1")[0].reset();
            $('#AirportPickUpInformationDiv').hide();
            $('#AirportDropInformationDiv').hide();
            $("ContentPlaceHolder1_hfReservationId").val("");
            $("#ReservationRoomGrid tbody").html('');
            ClearFormFromServer();
        }

        function ClearFormFromServer() {
            $("#ContentPlaceHolder1_btnCancel").trigger("click");
        }

        function activaTab() {
            $('#tabPage a[href="#tab-2"]').tab('show')
        };

        function AddServiceCharge() {
            var alreadyLoad = $.trim($("#<%=hfIsPaidServiceAlreadyLoded.ClientID %>").val());

            if (alreadyLoad != "0") {
                $("#PaidServiceDialog").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 450,
                    closeOnEscape: true,
                    resizable: false,
                    title: "Paid Service",
                    show: 'slide'
                });
                return;
            }

            var reservationId = $("#<%=hfReservationId.ClientID %>").val();
            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            var currencyId = $("#<%=ddlCurrency.ClientID %>").val();
            var convertionRate = $("#<%=txtConversionRate.ClientID %>").val();

            if (currencyType != "Local") {
                if (convertionRate == "") {
                    toastr.error("Please give conversion rate");
                    return;
                }
            }

            if (reservationId == "")
                reservationId = 0;

            $("#<%=hfPaidServiceSaveObj.ClientID %>").val("");
            PageMethods.GetPaidServiceDetails(reservationId, currencyId, currencyType, convertionRate, OnGetPaidServiceSucceed, OnGetPaidServiceFailed);
            return false;
        }

        var vvc = [], vvd = [];
        function OnGetPaidServiceSucceed(result) {
            $("#<%=hfIsPaidServiceAlreadyLoded.ClientID %>").val("1");
            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();

            if (result.RegistrationPaidService.length == 0) {
                if ($("#<%=hfPaidServiceSaveObj.ClientID %>").val() != "") {
                    alreadySavePaidServices = JSON.parse($("#<%=hfPaidServiceSaveObj.ClientID %>").val());
                    result.RegistrationPaidService = alreadySavePaidServices;
                }
            }
            else {
                alreadySavePaidServices = result.RegistrationPaidService;

                if ($("#<%=hfPaidServiceSaveObj.ClientID %>").val() != "") {
                    alreadySavePaidServices = JSON.parse($("#<%=hfPaidServiceSaveObj.ClientID %>").val());
                    result.RegistrationPaidService = alreadySavePaidServices;
                }

                $("#<%= hfPaidServiceDeleteObj.ClientID %>").val("1");
            }

            vvc = result;

            var table = "", tr = "", td = "", i = 0, alreadyChecked = "", serviceCost = 0;
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
                if (currencyType == "Local") {
                    serviceCost = (result.PaidService[i].UnitPriceLocal == null ? 0 : result.PaidService[i].UnitPriceLocal);
                }
                else {
                    serviceCost = (result.PaidService[i].UnitPriceUsd == null ? 0 : result.PaidService[i].UnitPriceUsd);
                }

                var vc = _.findWhere(result.RegistrationPaidService, { ServiceId: result.PaidService[i].ServiceId });
                if (vc != null) {
                    alreadyChecked = "checked='checked'";
                    if (currencyType == "Local") {
                        serviceCost = (vc.UnitPrice == null ? serviceCost : vc.UnitPrice);
                    }
                    else {
                        serviceCost = (vc.UnitPrice == null ? serviceCost : vc.UnitPrice);
                    }
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
                    + "<input type=\"text\" value=\"" + serviceCost + "\" id=\"txt" + result.PaidService[i].ServiceId + "\" onblur = 'UpdateServiceCost(this, " + result.PaidService[i].ServiceId + ")' disabled = 'disabled' style=\"width:60px; margin-bottom: 1px;\" />" +
                    "</td>" +
                    "<td align=\"left\" style=\"display:none; width: 65px\">" +
                    serviceCost +
                    "</td>";

                tr += td + "</tr>";

                table += tr;
            }
            table += " </tbody> </table>";

            vvd = table;

            $("#paidServiceContainer").html(table);

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

        function OnGetPaidServiceFailed(error) {

        }

        function ChangeServiceCost(serviceId) {
            if ($("#ch" + serviceId).is(':checked') == true) {
                $("#txt" + serviceId).attr('disabled', false);
            }
            else if ($("#ch" + serviceId).is(':checked') == false) {
                $("#txt" + serviceId).attr('disabled', true);
            }
        }

        function PerformRoomAvailableChecking() {
            var RoomTypeId = $("#<%=ddlRoomTypeId.ClientID %>").val();
            var txtFromDate = $("#<%=txtDateIn.ClientID %>").val();
            var txtToDate = $("#<%=txtDateOut.ClientID %>").val();
            var txtRoomQuantityEntered = $("#<%=txtRoomId.ClientID %>").val();
            var reservationId = 0;
            if ($("#<%=hfReservationId.ClientID %>").val() != "") {
                reservationId = $("#<%=hfReservationId.ClientID %>").val();
            }
            PageMethods.PerformGetRoomAvailableChecking(reservationId, RoomTypeId, txtFromDate, txtToDate, txtRoomQuantityEntered, OnPerformRoomAvailableCheckingSucceeded, OnPerformRoomAvailableCheckingFailed);
            return false;
        }
        function OnPerformRoomAvailableCheckingSucceeded(result) {
            //debugger;
            //SaveRoomDetailsInformationByWebMethod();
            //return false;

            var txtRoomIdVal = $("#<%=txtRoomId.ClientID %>").val();
            if (parseFloat(result) < parseFloat(txtRoomIdVal)) {
                var hfIsRoomOverbookingEnableVal = $("#<%=hfIsRoomOverbookingEnable.ClientID %>").val();
                if (hfIsRoomOverbookingEnableVal == "1") {
                    var r = confirm("This type of room is not available, if you continue then it will be a Overbooking. Do You Want to Continue?");
                    if (r == true) {
                        SaveRoomDetailsInformationByWebMethod();
                    }
                }
                else {
                    toastr.info("This type of room is not available.");
                }
            }
            else {
                SaveRoomDetailsInformationByWebMethod();
            }
            return false;
        }
        function OnPerformRoomAvailableCheckingFailed(error) {
            toastr.error(error.get_message());
        }
        function GridPagingForSearchRegistration(pageNumber, isCurrentOrPreviousPage) {
            var gridRecordCounts = ($("#ContentPlaceHolder1_gvRoomRegistration tbody tr").length + 1);
            $("#ContentPlaceHolder1_hfPageNumber").val(pageNumber);
            $("#ContentPlaceHolder1_hfGridRecordCounts").val(gridRecordCounts);
            $("#ContentPlaceHolder1_hfIsCurrentRPreviouss").val(isCurrentOrPreviousPage);
            $("#ContentPlaceHolder1_btnPagination").trigger("click");
        }
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
                else {
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
                    //else if (passIssuePlace == "") {
                    //    toastr.warning("Please Provide Passport Issue Place.");
                    //    return;
                    //}
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
                    else if (email != "") {
                        var mailformat = new RegExp('^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$', 'i');
                        if (txtGuestEmail.match(mailformat)) {
                        }
                        else {
                            toastr.warning("You have entered an invalid email address!");
                            $("#txtGuestEmail").focus();
                            return;
                        }
                    }
                }
                var dob = $("#<%=txtGuestDOB.ClientID %>").val();
                var address = $("#<%=txtGuestAddress2.ClientID %>").val();
                var phoneNo = $("#<%=txtGuestPhone.ClientID %>").val();

                if (dob == "") {
                    toastr.warning("Please Provide Date of Birth.");
                    $("#ContentPlaceHolder1_txtGuestDOB").focus();
                    return;
                }
                else if (address == "") {
                    toastr.warning("Please Provide Address.");
                    $("#ContentPlaceHolder1_txtGuestAddress2").focus();
                    return;
                }
                else if (phoneNo == "") {
                    toastr.warning("Please Provide Phone No.");
                    $("#ContentPlaceHolder1_txtGuestPhone").focus();
                    return;
                }
            }
            LoadDetailGridInformation();

        }
        function OnLoadXtraValidationFailed(error) {
        }
        function SameAsGuest() {
            var ctrl = '#<%=IsSameasGuest.ClientID%>'
            if ($(ctrl).is(':checked')) {
                var title = $("#<%=ddlTitle.ClientID %>").val();
                var titleText = $("#<%=ddlTitle.ClientID %> option:selected").text();

                if (title == "0") {
                    $("#ContentPlaceHolder1_IsSameasGuest").prop('checked', false);
                    toastr.warning("Please Select Title.");
                    $("#<%=ddlTitle.ClientID %>").focus();
                    return false;
                }

                if (title == "MrNMrs.") {
                    title = "Mr. & Mrs.";
                }
                else if (title == "N/A") {
                    title = "";
                }
                else {
                    title = titleText;
                }
                var firstName = $("#<%=txtFirstName.ClientID %>").val().trim();
                var lastName = $("#<%=txtLastName.ClientID %>").val().trim();
                var name = title + " " + firstName + " " + lastName;
                var phone = $('#<%=txtPhone.ClientID%>').val();
                var email = $('#<%=txtEmail.ClientID%>').val();
                $('#<%=txtContactPerson.ClientID%>').val(name);
                $('#<%=txtMobileNumber.ClientID%>').val(phone);
                $('#<%=txtContactEmail.ClientID%>').val(email);
            }
            else {
                $('#<%=txtContactPerson.ClientID%>').val("");
                $('#<%=txtMobileNumber.ClientID%>').val("");
                $('#<%=txtContactEmail.ClientID%>').val("");
            }
        }
        function OnMultiplePickup() {
            var ctrl = '#<%=chkMultiplePickup.ClientID%>'
            if ($(ctrl).is(':checked')) {
                $("#MultipleGuest").show();
                $("#addMultiplePickup").show();
                var hfReservationIdTemp = $("#<%=hfReservationIdTemp.ClientID %>").val();
                var reservationId = parseInt(hfReservationIdTemp);
                PageMethods.LoadMultipleGuest(reservationId, OnLoadMultipleGuestSucceeded, OnLoadMultipleGuestFailed);
            }
            else {
                $("#MultipleGuest").hide();
                $("#addMultiplePickup").hide();
            }
        }
        function OnMultipleDrop() {
            var ctrl = '#<%=chkMultipleDrop.ClientID%>'
            if ($(ctrl).is(':checked')) {
                $("#MultipleDropGuest").show();
                $("#addMultipleDrop").show();
                var hfReservationIdTemp = $("#<%=hfReservationIdTemp.ClientID %>").val();
                var reservationId = parseInt(hfReservationIdTemp);
                PageMethods.LoadMultipleGuest(reservationId, OnLoadMultipleGuestSucceeded, OnLoadMultipleGuestFailed);
            }
            else {
                $("#MultipleDropGuest").hide();
                $("#addMultipleDrop").hide();
            }
        }
        function OnLoadMultipleGuestSucceeded(result) {
            var list = result;
            var ddlGuestId = '<%=ddlGuest.ClientID%>';
            var control = $('#' + ddlGuestId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].GuestName + '" value="' + list[i].GuestId + '">' + list[i].GuestName + '</option>');
                    }
                }
            }
            var ddlDropGuest = '<%=ddlDropGuest.ClientID%>';
            var control2 = $('#' + ddlDropGuest);
            control2.empty();

            if (list != null) {
                if (list.length > 0) {
                    control2.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control2.append('<option title="' + list[i].GuestName + '" value="' + list[i].GuestId + '">' + list[i].GuestName + '</option>');
                    }
                }
            }
            return false;
        }
        function OnLoadMultipleGuestFailed() {
        }
        function DeletePickup(deleteItem) {
            var tr = $(deleteItem).parent().parent();
            var apdId = $.trim($(tr).find("td:eq(0)").text());
            if (apdId != "") {
                DeletedAirportPickupDrop.push({
                    APDId: apdId
                });
            }
            $(deleteItem).parent().parent().remove();
        }
        function RoomNumberDropDown() {
            var roomNumbersArr = []; // roomNumbers.split(',');
            var roomIdsArr = []; // roomIds.split(',');
            var i = 0;

            var ddlRoomNumber = '<%=ddlRoomNumber.ClientID%>';
            var control = $('#' + ddlRoomNumber);
            control.empty();

            $("#ReservationRoomGrid tbody tr").each(function () {
                roomIdsArr = $.trim($(this).find("td:eq(7)").text()).split(","); // id
                roomNumbersArr = $.trim($(this).find("td:eq(8)").text()).split(","); //number

                if (roomIdsArr != null) {
                    if (roomIdsArr.length > 0) {
                        for (i = 0; i < roomIdsArr.length; i++) {
                            if (roomIdsArr[i] != "0") {
                                if (roomNumbersArr[i] != "") {
                                    control.append('<option title="' + roomNumbersArr[i] + '" value="' + roomIdsArr[i] + '">' + roomNumbersArr[i] + '</option>');
                                }
                            }
                        }
                    }
                }

                roomIdsArr = [];
                roomNumbersArr = [];
            });
        }
        function OnLoadRoomNumbersSucceeded(result) {
            var list = result;
            var ddlRoomNumber = '<%=ddlRoomNumber.ClientID%>';
            var control = $('#' + ddlRoomNumber);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].RoomNumber + '" value="' + list[i].RoomId + '">' + list[i].RoomNumber + '</option>');
                    }
                }
            }
        }
        function OnLoadRoomNumbersFailed(error) {
        }

        function MultiplePickUpDropInfo(reservationId) {

            PageMethods.MultipleAireportPickupDropInfo(reservationId, OnLoadMultipleAireportPickupDropInfoSucceeded, OnLoadMultipleAireportPickupDropInfoFailed);
            return false;
        }
        function OnLoadMultipleAireportPickupDropInfoSucceeded(result) {
            $("#MultipleAireportPickupDropInfo").html(result);
            $("#MultiplePickUpDropInfoDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 550,
                closeOnEscape: true,
                resizable: false,
                title: "Multiple PickUp Drop Info",
                show: 'slide'
            });
        }
        function OnLoadMultipleAireportPickupDropInfoFailed() { }

        function ShowReport(reservationId, aPDId) {
            var iframeid = 'printDoc';
            var url = "/HotelManagement/Reports/frmReportReservationBillInfo.aspx?GuestBillInfo=" + reservationId + "&APDInfo=" + aPDId;
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
                title: "Confirmation Letter",
                show: 'slide'
            });
        }

        // Guest Reference
        function LoadGuestReference() {
            LoadGuestReferenceInfo();
            //popup(1, 'DivGuestReference', '', 600, 525);
            $("#DivGuestReference").dialog({
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

        function LoadGuestReferenceInfo() {
            PageMethods.LoadGuestReferenceInfo(OnLoadGuestReferenceSucceeded, OnLoadGuestReferenceFailed);
            return false;
        }
        function OnLoadGuestReferenceSucceeded(result) {
            $("#ltlGuestReference").html(result);

            var PreferenceIdList = "";

            PreferenceIdList = $("#ContentPlaceHolder1_hfGuestPreferenceId").val();
            //alert(PreferenceIdList);  
            //alert(SelectdPreferenceId);

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
            $("#ContentPlaceHolder1_txtAdditionalRemarks").val($("#ContentPlaceHolder1_hfAdditionalRemarks").val());
            return false;
        }
        function OnLoadGuestReferenceFailed() {
        }

        function GetCheckedGuestPreference() {
            $("#GuestPreferenceDiv").show();
            var SelectdPreferenceName = "";

            $('#GuestReferenceInformation tbody tr').each(function () {
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
                SelectdPreferenceName += (SelectdPreferenceName == "" ? '' : ', ') + $("#ContentPlaceHolder1_txtAdditionalRemarks").val();
            }
            $("#<%=lblGstPreference.ClientID %>").text(SelectdPreferenceName);
            //popup(-1);
            $("#DivGuestReference").dialog("close");
        }

        function CloseRoomDialog() {
            $("#DivRoomSelect").dialog("close");
        }

        function ClosePreferenceDialog() {
            $("#DivGuestReference").dialog("close");
        }

        <%--function ToggleLitedArrivalFlightNameInfo() {
            var ctrl = '#<%=chkIsLitedArrivalFlightName.ClientID%>'
            if ($(ctrl).is(':checked')) {
                $('#ReservedArrivalFlightName').hide("slow");
                $('#ListedArrivalFlightName').show("slow");
            }
            else {
                $('#ListedArrivalFlightName').hide("slow");
                $('#ReservedArrivalFlightName').show("slow");
                $('#PaymentInformation').hide("slow");
                $(ctrl).prop('checked', false)
                $("#ContentPlaceHolder1_ddlArrivalFlightName").val("0");
            }
        }

        function ToggleLitedDepartureFlightNameInfo() {
            var ctrl = '#<%=chkIsLitedDepartureFlightName.ClientID%>'
            if ($(ctrl).is(':checked')) {
                $('#ReservedDepartureFlightName').hide("slow");
                $('#ListedDepartureFlightName').show("slow");
            }
            else {
                $('#ListedDepartureFlightName').hide("slow");
                $('#ReservedDepartureFlightName').show("slow");
                $(ctrl).prop('checked', false)
                $("#ContentPlaceHolder1_ddlDepartureFlightName").val("0");
            }
        }--%>

        function CalculateRoomRateInclusively() {

            if ($.trim($("#ContentPlaceHolder1_txtTotalRoomRate").val()) == "0" || $.trim($("#ContentPlaceHolder1_txtTotalRoomRate").val()) == "") {
                toastr.warning("Room Rate 0/empty Is Not Acceptable.");
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

            //if ($("#ContentPlaceHolder1_cbServiceCharge").is(":checked") == false)
            //    $("#ContentPlaceHolder1_cbCalculateServiceCharge").prop("checked", false);
            //else
            //    $("#ContentPlaceHolder1_cbCalculateServiceCharge").prop("checked", true);

            //if ($("#ContentPlaceHolder1_cbCityCharge").is(":checked") == false)
            //    $("#ContentPlaceHolder1_cbCalculateCityCharge").prop("checked", false);
            //else
            //    $("#ContentPlaceHolder1_cbCalculateCityCharge").prop("checked", true);

            //if ($("#ContentPlaceHolder1_cbVatAmount").is(":checked") == false)
            //    $("#ContentPlaceHolder1_cbCalculateVatCharge").prop("checked", false);
            //else
            //    $("#ContentPlaceHolder1_cbCalculateVatCharge").prop("checked", true);

            //if ($("#ContentPlaceHolder1_cbAdditionalCharge").is(":checked") == false)
            //    $("#ContentPlaceHolder1_cbCalculateAdditionalCharge").prop("checked", false);
            //else
            //    $("#ContentPlaceHolder1_cbCalculateAdditionalCharge").prop("checked", true);

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
                    //$(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
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
            $("#ContentPlaceHolder1_txtTotalRoomRate").val(Math.round(CalculatedTotalRoomRate).toFixed(2));
            $("#ContentPlaceHolder1_txtRoomRate").val($("#ContentPlaceHolder1_txtCalculateRackRate").val());
            $("#CalculateRackRateInclusivelyDialog").dialog("close");

            $("#ContentPlaceHolder1_cbServiceCharge").prop("checked", $("#ContentPlaceHolder1_cbCalculateServiceCharge").is(":checked"));
            $("#ContentPlaceHolder1_cbCityCharge").prop("checked", $("#ContentPlaceHolder1_cbCalculateCityCharge").is(":checked"));
            $("#ContentPlaceHolder1_cbVatAmount").prop("checked", $("#ContentPlaceHolder1_cbCalculateVatCharge").is(":checked"));
            $("#ContentPlaceHolder1_cbAdditionalCharge").prop("checked", $("#ContentPlaceHolder1_cbCalculateAdditionalCharge").is(":checked"));

            //CalculateDiscount();
            ClearRRC();
        }

        function ClearRRC() {
            $("#ContentPlaceHolder1_txtCalculatedTotalRoomRate").val("");
            $("#ContentPlaceHolder1_txtCalculateServiceCharge").val("");
            $("#ContentPlaceHolder1_txtCalculateRackRate").val("");
            $("#ContentPlaceHolder1_txtCalculateCityCharge").val("");
            $("#ContentPlaceHolder1_txtCalculateVatCharge").val("");
            $("#ContentPlaceHolder1_txtCalculateDiscountAmount").val("");
        }

        function CheckReservationCanBeCancel(reservationId) {
            $.ajax({
                type: "POST",
                url: "./frmRoomReservationNew.aspx/IsCanCancelReservation",
                data: JSON.stringify({ reservationId: reservationId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: (data) => {
                    if (data.d)
                        CancelReservation(reservationId);
                    else {
                        toastr.warning("Room partially checked-in from this reservation, you can't cancel it.");
                    }
                },
                error: function (error) {
                    if (error.responseJSON != undefined)
                        toastr.error("Contact With Admin.");
                }
            });
            return false;
        }

        function CancelReservation(reservationId, reservatioStatus) {
            if (reservatioStatus == "Partially Registered") {
                toastr.warning("Room partially checked-in from this reservation, you can't cancel it.");
                return false;
            }
            $("#<%=hfCancelReservationId.ClientID%>").val(reservationId);
            var optionStr = "<option value=\"" + "Cancel" + "\">" + "Cancel" + "</option>";
            $("#sCancelStatus").html(optionStr);
            $("#reservationCancelPopUp").dialog({
                autoOpen: true,
                modal: true,
                width: 725,
                closeOnEscape: true,
                resizable: false,
                title: "Cancel Remarks",
                show: 'slide'

            });
            return false;
        }
        function ConfirmCancelReservation() {
            var reservationId = parseInt($("#<%=hfCancelReservationId.ClientID%>").val());
            var reason = $("#ContentPlaceHolder1_tbRemarks").val();
            var mode = $("#sCancelStatus").val();
            if (reason == "") {
                toastr.warning("Please provide reason.");
            }
            else {
                var isConfirm = confirm('Do you want to Cancel this Reservation?');
                if (isConfirm)
                    PageMethods.CancelOrActiveReservation(reservationId, reason, mode, OnSuccessCancelOrActiveReservation, OnFailCancelOrActiveReservation);
            }

            return false;
        }

        function ShowReservationCurrentRoomStatus(reservationId, reservationNumber) {
            var isConfirm = confirm('Do you want to see Current Room Status for the Reservation Number: ' + reservationNumber + '?');
            if (!isConfirm) {
                return false;
            }
            else {
                currentRoomReservationNumber = reservationNumber;
                PageMethods.ShowReservationCurrentRoomStatus(reservationId, OnSuccessLoading, OnFailLoading);
                return false;
            }
        }

        function OnSuccessLoading(result) {
            var iframeid = 'frmPrint';
            document.getElementById('ReservationCurrentRoomStatusDialougeDiv').innerHTML = result;

            $("#ShowReservationCurrentRoomStatusDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "75%",
                height: 450,
                closeOnEscape: false,
                resizable: false,
                title: "Current Room Status for the Reservation Number: " + currentRoomReservationNumber,
                show: 'slide'
            });

            return false;
        }

        function OnFailLoading(error) {
            toastr.error(error.get_message());
            return false;
        }

        function EditRoomReservation() {
            var isConfirm = confirm('Do you want to edit this Reservation?');
            if (!isConfirm) {
                return false;
            }
            else {
                return true;
            }
        }

        function CopyToRoomReservation() {
            var isConfirm = confirm('Do you want to Copy this Reservation?');
            if (!isConfirm) {
                return false;
            }
            else {
                return true;
            }
        }

        function ActiveReservation(reservationId) {
            var isConfirm = confirm('Do you want to active this Reservation?');
            var reason = "";
            if (isConfirm) {
                var mode = "Confirmed";
                PageMethods.CancelOrActiveReservation(reservationId, reason, mode, OnSuccessCancelOrActiveReservation, OnFailCancelOrActiveReservation);
            }
            return false;
        }

        function OnSuccessCancelOrActiveReservation(returnInfo) {
            if (returnInfo.IsSuccess) {
                toastr.success(returnInfo.AlertMessage.Message);
                setTimeout(function () {
                    var possiblePath = "frmRoomReservationNew.aspx";
                    window.location = possiblePath;
                }, 1500);
            }

            else
                toastr.error(returnInfo.AlertMessage.Message);
        }
        function OnFailCancelOrActiveReservation(error) {
            toastr.error(error.get_message());
        }
        function PerformGroupBillPreviewAction(groupMasterId) {
            var reportType = "room";
            var url = "/HotelManagement/Reports/frmReportGroupReservationBillInfo.aspx?rt=" + reportType + "&gmid=" + groupMasterId;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }
    </script>
    <asp:HiddenField ID="hfInclusiveHotelManagementBill" runat="server" />
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfMinCheckInDate" runat="server" />
    <asp:HiddenField ID="hfIsRoomOverbookingEnable" runat="server" Value="0" />
    <asp:HiddenField ID="hfReservationId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsSaveRUpdate" runat="server" Value="" />
    <asp:HiddenField ID="hfReservationIdTemp" runat="server" />
    <asp:HiddenField ID="hfReservationEditId" runat="server" Value="" />
    <asp:HiddenField ID="hfPaidServiceSaveObj" runat="server" />
    <asp:HiddenField ID="hfPaidServiceDeleteObj" runat="server" />
    <asp:HiddenField ID="hfIsPaidServiceAlreadyLoded" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsPaidServiceAlreadySavedDb" runat="server" Value="0" />
    <asp:HiddenField ID="hfPageNumber" runat="server" />
    <asp:HiddenField ID="hfGridRecordCounts" runat="server" />
    <asp:HiddenField ID="hfIsCurrentRPreviouss" runat="server" />
    <asp:HiddenField ID="hfProfileOrAddMore" runat="server" />
    <asp:HiddenField ID="hfFstAsndRoomId" runat="server" />
    <asp:HiddenField ID="hfAPId" runat="server" />
    <asp:HiddenField ID="hfADId" runat="server" />
    <asp:HiddenField ID="hfPrevGuestId" runat="server" Value="" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCurrentReservationGuestId" runat="server" />
    <asp:HiddenField ID="hfIsSameasContactPersonAsGuest" runat="server" />
    <asp:HiddenField ID="hfGuestPreferenceId" runat="server" />
    <asp:HiddenField ID="hfArrivalAirlineId" runat="server" />
    <asp:HiddenField ID="hfDepartureAirlineId" runat="server" />
    <asp:HiddenField ID="hfAPDId" runat="server" />
    <asp:HiddenField ID="hfRoomType" runat="server" Value="" />
    <asp:HiddenField ID="hfIsDiscountApplicableOnRackRate" runat="server" />
    <asp:HiddenField ID="hfGuestHouseVat" runat="server" />
    <asp:HiddenField ID="hfGuestHouseServiceCharge" runat="server" />
    <asp:HiddenField ID="hfIsRatePlusPlus" runat="server" />
    <asp:HiddenField ID="hfIsBlockGuest" runat="server" />
    <asp:HiddenField ID="hfIsCopyTo" runat="server" />
    <asp:HiddenField ID="hfMandatoryFields" runat="server" />
    <asp:HiddenField ID="hfIsMinimumRoomRateCheckingEnable" runat="server" />
    <asp:HiddenField ID="hfAdditionalRemarks" runat="server" Value="" />
    <asp:HiddenField ID="hfDefaultFrontOfficeMealPlanHeadId" runat="server" Value="0" />
    <asp:HiddenField ID="hfDefaultFrontOfficeMarketSegmentHeadId" runat="server" Value="0" />
    <div id="ShowReservationCurrentRoomStatusDialouge" style="display: none">
        <div class="form-horizontal">
            <div class="form-group">
                <div id="ReservationCurrentRoomStatusDialougeDiv"></div>
            </div>
        </div>
    </div>
    <div id="ReservationRoomControlChartDialogue" style="display: none;">
        <iframe id="frmRoomControlChartDialogue" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="CalculateRackRateInclusivelyDialog" style="display: none;">
        <div class="form-horizontal">
            <div class="form-group">
                <label class="control-label col-md-2 required-field">
                    Total Room Rate</label>
                <div class="col-md-4">
                    <asp:TextBox ID="txtCalculatedTotalRoomRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="22"
                        onblur="CalculateRateInclusively()"></asp:TextBox>
                </div>
                <label class="control-label col-md-2 required-field">
                    Rack Rate</label>
                <div class="col-md-4">
                    <asp:TextBox ID="txtCalculateRackRate" runat="server" CssClass="form-control" TabIndex="23" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div id="rrcServiceChargeDiv">
                    <div class="col-md-2" style="text-align: right;">
                        <asp:Label ID="Label3" runat="server" CssClass="control-label required-field" Text="Service Charge"></asp:Label>
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
                <div id="rrcCityChargeDiv">
                    <div class="col-md-2" style="text-align: right;">
                        <asp:Label ID="Label4" runat="server" CssClass="control-label required-field" Text="City/SD Charge"></asp:Label>
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
                <div id="rrcVatChargeDiv">
                    <div class="col-md-2" style="text-align: right;">
                        <asp:Label ID="Label5" runat="server" CssClass="control-label required-field" Text="Vat Amount"></asp:Label>
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
                <div id="rrcAdditionalChargeDiv">
                    <div class="col-md-2" style="text-align: right;">
                        <asp:Label ID="Label6" runat="server" CssClass="control-label required-field" Text="Additional Charge"></asp:Label>
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
            <div class="form-group">
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
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="800" height="650" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnPagination" runat="server" Text="pagination" OnClick="btnPagination_Click" />
    </div>
    <div style="display: none;">
        <div runat="server" id="RoomDetailsTemp">
        </div>
    </div>
    <div style="display: none;">
        <div runat="server" id="AirportPickupDetails">
        </div>
    </div>
    <div style="display: none;">
        <div runat="server" id="AirportDropDetails">
        </div>
    </div>
    <div style="display: none;" id="MultiplePickUpDropInfoDiv">
        <div id="MultipleAireportPickupDropInfo">
        </div>
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
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Reservation</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Guest Details</a></li>
            <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Pick Up/Drop Off</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-4">Complimentary Item</a></li>
            <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-5">Search Reservation</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="CheckInDate" class="control-label required-field col-md-2">
                                Check In Date</label>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="DateInHiddenField" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="DateInHiddenFieldEdit" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtDateIn" CssClass="form-control" runat="server" TabIndex="1" placeholder="Check In Date"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4 col-padding-left-none">
                                        <asp:TextBox ID="txtProbableArrivalTime" placeholder="12" runat="server" CssClass="form-control"
                                            TabIndex="23"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <label for="CheckOutDate" class="control-label required-field col-md-2">
                                Check Out Date</label>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:HiddenField ID="DateOutHiddenField" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtEditedRoom" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtEditedRoomNumber" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtDateOut" runat="server" CssClass="form-control" TabIndex="5" placeholder="Check Out Date"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4 col-padding-left-none">
                                        <asp:TextBox ID="txtProbableDepartureTime" runat="server" CssClass="form-control"
                                            TabIndex="23"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnlReservationEditTimePanelwillHide" runat="server">
                            <div class="form-group">
                                <label for="Title" class="control-label required-field col-md-2">
                                    Title</label>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <asp:DropDownList ID="ddlTitle" runat="server" CssClass="form-control" TabIndex="2">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 col-padding-left-none">
                                            <asp:ImageButton ID="imgReservationSearch" Width="25" runat="server" OnClientClick="javascript:return SearchGuestForReservation()"
                                                ImageUrl="~/Images/SearchItem.png" ToolTip="Search Guest" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-offset-2 col-md-4">
                                    <div class="row">
                                        <div class="col-md-offset-8 col-md-4 col-padding-left-none">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtReservationDuration" CssClass="form-control" runat="server" ReadOnly="true"
                                                    ClientIDMode="Static"></asp:TextBox>
                                                <span class="input-group-addon">
                                                    <asp:CheckBox ID="cbReservationDuration" runat="server" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="FirstName" class="control-label required-field col-md-2">
                                    First Name</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFirstName" CssClass="form-control" runat="server" placeholder="First Name"></asp:TextBox>
                                </div>
                                <label for="LastName" class="control-label col-md-2">
                                    Last Name</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtLastName" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="MobileNumber" class="control-label col-md-2">
                                    Phone/ Mobile No.</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                                </div>
                                <label for="Email" class="control-label col-md-2">
                                    Email</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="Country" class="control-label col-md-2">
                                    Country</label>
                                <div class="col-md-4">
                                    <input id="txtCountry" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlCountry" runat="server" TabIndex="40">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                </div>
                                <div class="col-md-4">
                                    <input id="btnProfile" type="button" value="More Guest Details" class="btn btn-primary btn-sm"
                                        tabindex="51" />
                                    <input id="btnAddmore" type="button" value="Profile" class="btn btn-primary btn-sm"
                                        tabindex="51" style="display: none;" />
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group">
                            <label for="ReservationMode" class="control-label required-field col-md-2">
                                Reservation Mode</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReservedMode" runat="server" CssClass="form-control" TabIndex="7">
                                    <asp:ListItem>--- Please Select ---</asp:ListItem>
                                    <asp:ListItem>Self</asp:ListItem>
                                    <asp:ListItem>Company</asp:ListItem>
                                    <asp:ListItem>Group</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="PersonDiv" style="display: none">
                                <div class="col-md-6 aspBoxText" style="width: 250px;">
                                    <asp:Panel ID="pnlIsSameasGuest" runat="server">
                                        <asp:CheckBox ID="IsSameasGuest" runat="server" OnClick="SameAsGuest();" CssClass="aspBoxAlign" />&nbsp;
                                        Guest as Contact Person
                                    </asp:Panel>
                                </div>
                            </div>
                            <div style="display: none;">
                                <label for="BusinessPromotion" class="control-label col-md-2">
                                    Business Promotion</label>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlBusinessPromotionId" runat="server" CssClass="form-control"
                                        TabIndex="8">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="GroupDiv" style="display: none;">
                            <label for="GroupName" class="control-label col-md-2 required-field">
                                Group Name</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlGroupName" runat="server" CssClass="form-control" AutoPostBack="false">
                                </asp:DropDownList>
                                <div style="display: none;">
                                    <asp:TextBox ID="txtGroupName" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="CompanyInformation" style="display: none;">
                            <div id="ListedCompanyInfo" class="form-group">
                                <div id="CompanyLabel" class="col-md-2" style="text-align: right;">
                                    <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Listed Company"></asp:Label>
                                </div>
                                <div id="CompanyControl" class="col-md-10">
                                    <div class="input-group col-md-12">
                                        <span id="chkIsLitedCompanyDiv" class="input-group-addon">
                                            <asp:CheckBox ID="chkIsLitedCompany" runat="server" Text="" onclick="javascript: return ToggleListedCompanyInfo();"
                                                TabIndex="8" />
                                        </span>
                                        <div id="ListedCompany" style="display: none; width: 100%;">
                                            <input id="txtCompany" type="text" class="form-control" />
                                            <div style="display: none;">
                                                <asp:DropDownList ID="ddlCompanyName" runat="server" CssClass="form-control" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div id="ReservedCompany" style="display: none; width: 100%;">
                                            <asp:TextBox ID="txtReservedCompany" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="ContactPersonDiv" style="display: none">
                            <div id="ListedContactInfo" class="form-group">
                                <div id="ContactLabel" class="col-md-2" style="text-align: right;">
                                    <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Contact Person"></asp:Label>
                                </div>
                                <div id="ContactControl" class="col-md-10">
                                    <div class="input-group col-md-12">
                                        <span id="chkIsLitedContactDiv" class="input-group-addon">
                                            <asp:CheckBox ID="chkIsLitedContact" runat="server" Text="" onclick="javascript: return ToggleListedContactInfo();"
                                                TabIndex="8" />
                                        </span>
                                        <div id="ListedContact" style="display: none; width: 100%;">
                                            <input id="txtListedContactPerson" type="text" class="form-control" runat="server" />
                                            <div style="display: none;">
                                                <asp:HiddenField runat="server" ID="hfContactId" Value="0" />
                                            </div>
                                        </div>
                                        <div id="ReservedContact" style="display: none; width: 100%;">
                                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="ContactAddress" style="display: none;">
                            <div class="form-group">
                                <label for="ContactAddress" class="control-label col-md-2">
                                    Contact Address</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtContactAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                                        TabIndex="11" MaxLength="300"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="MobileNEmailDiv" style="display: none">
                            <div class="form-group">
                                <label for="MobileNumber" class="control-label col-md-2">
                                    Mobile Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="form-control" TabIndex="14"></asp:TextBox>
                                </div>
                                <label for="EmailAddress" class="control-label col-md-2" id="lblEmailAddress">
                                    Email Address</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContactEmail" runat="server" CssClass="form-control" TabIndex="13"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="PaymentInformation" style="display: none;">
                            <div class="form-group">
                                <label for="PaymentMode" class="control-label col-md-2 required-field">
                                    Payment Mode</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control" TabIndex="16">
                                        <asp:ListItem Value="0">-- Please Select --</asp:ListItem>
                                        <asp:ListItem Value="Company">Company</asp:ListItem>
                                        <asp:ListItem Value="Self">Self</asp:ListItem>
                                        <asp:ListItem Value="TBA">Before C/O (Company/ Host)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <label for="PayFor" class="control-label col-md-2">
                                    Pay For</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPayFor" runat="server" CssClass="form-control" TabIndex="17">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="CurrencyDiv" style="display: none">
                            <label for="CurrencyType" class="control-label col-md-2">
                                Currency Type</label>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfCurrencyHiddenField" runat="server" />
                                <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" TabIndex="16">
                                </asp:DropDownList>
                            </div>
                            <div id="CurrencyAmountInformationDiv" style="display: none;">
                                <label for="ConversionRate" class="control-label col-md-2">
                                    Conversion Rate</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtConversionRate" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="childDivSection">
                            <div id="RoomDetailsInformation" class="panel panel-default">
                                <div class="panel-heading">
                                    Room Detailed Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label for="RoomType" class="control-label required-field col-md-2">
                                                Room Type</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlRoomTypeId" runat="server" CssClass="form-control" TabIndex="16">
                                                </asp:DropDownList>
                                            </div>
                                            <label for="RoomQuantity" class="control-label required-field col-md-2">
                                                Room Quantity</label>
                                            <div class="col-md-4">
                                                <div class="row">
                                                    <div class="col-md-3 col-padding-rigth-none">
                                                        <asp:TextBox ID="txtRoomId" runat="server" placeholder="Quantity" CssClass="form-control" TabIndex="17"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-3 col-padding-rigth-none">
                                                        <asp:TextBox ID="txtPaxQuantity" runat="server" placeholder="Pax" CssClass="form-control" TabIndex="17"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:HiddenField ID="hfIsReservationRoomListButtonWillHide" runat="server"></asp:HiddenField>
                                                        <input type="button" tabindex="18" id="btnChange" value="Room List" class="btn btn-primary btn-sm"
                                                            onclick="javascript: return LoadRoomNumber();" />
                                                        <input type="button" tabindex="18" id="btnHotelPosition" value="HP..." class="btn btn-primary btn-sm"
                                                            onclick="javascript: return PopUpHotelPositionReportInfo();" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="RackRate" class="control-label col-md-2">
                                                Rack Rate</label>
                                            <div class="col-md-4">
                                                <asp:HiddenField ID="txtMinimumUnitPriceHiddenField" runat="server"></asp:HiddenField>
                                                <asp:HiddenField ID="txtUnitPriceHiddenField" runat="server"></asp:HiddenField>
                                                <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" TabIndex="17"
                                                    Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="DiscountType" class="control-label col-md-2">
                                                Discount Type</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control" TabIndex="16">
                                                    <asp:ListItem>Fixed</asp:ListItem>
                                                    <asp:ListItem>Percentage</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <label for="DiscountAmount" class="control-label col-md-2">
                                                Discount Amount</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="form-control quantitydecimal" TabIndex="17"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="NegotiatedRate" class="control-label col-md-2 required-field">
                                                Negotiated Rate</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtRoomRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="17"
                                                    onblur="CalculateDiscount()"></asp:TextBox>
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnlRackRateServiceChargeVatInformation" runat="server">
                                            <asp:HiddenField ID="hfIsServiceChargeEnableConfig" runat="server" />
                                            <asp:HiddenField ID="hfIsCitySDChargeEnableConfig" runat="server" />
                                            <asp:HiddenField ID="hfIsVatEnableConfig" runat="server" />
                                            <asp:HiddenField ID="hfIsAdditionalChargeEnableConfig" runat="server" />
                                            <div class="form-group">
                                                <div id="ServiceChargeLabel" class="col-md-2" style="text-align: right;">
                                                    <asp:Label ID="lbl" runat="server" CssClass="control-label" Text="Service Charge"></asp:Label>
                                                </div>
                                                <div id="ServiceChargeControl" class="col-md-4">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtServiceCharge" runat="server" TabIndex="22" CssClass="form-control"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="hfServiceCharge" runat="server"></asp:HiddenField>
                                                        <span class="input-group-addon">
                                                            <asp:CheckBox ID="cbServiceCharge" runat="server" Text="" CssClass="customChkBox"
                                                                onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                                TabIndex="8" Checked="True" />
                                                        </span>
                                                    </div>
                                                </div>
                                                <div id="VatAmountLabel" class="col-md-2" style="text-align: right;">
                                                    <asp:Label ID="lblVatAmount" runat="server" CssClass="control-label" Text="Vat Amount"></asp:Label>
                                                </div>
                                                <div id="VatAmountControl" class="col-md-4">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="23" CssClass="form-control"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="hfVatAmount" runat="server"></asp:HiddenField>
                                                        <span class="input-group-addon">
                                                            <asp:CheckBox ID="cbVatAmount" runat="server" Text="" CssClass="customChkBox" onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForVat(this);"
                                                                TabIndex="8" Checked="True" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div id="CityChargeLabel" class="col-md-2" style="text-align: right;">
                                                    <asp:Label ID="lblCityChargeLabel" runat="server" CssClass="control-label" Text="Label"></asp:Label>
                                                </div>
                                                <div id="CityChargeControl" class="col-md-4">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtCityCharge" runat="server" TabIndex="22" CssClass="form-control"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="hfCityCharge" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hfIsVatEnableOnGuestHouseCityCharge" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hfIsCitySDChargeEnableOnServiceCharge" runat="server"></asp:HiddenField>
                                                        <span class="input-group-addon">
                                                            <asp:CheckBox ID="cbCityCharge" runat="server" Text="" CssClass="customChkBox"
                                                                onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                                TabIndex="8" Checked="True" />
                                                        </span>
                                                    </div>
                                                </div>
                                                <div id="AdditionalChargeLabel" class="col-md-2" style="text-align: right;">
                                                    <asp:Label ID="lblAdditionalCharge" runat="server" CssClass="control-label" Text="Additional Charge"></asp:Label>
                                                </div>
                                                <div id="AdditionalChargeControl" class="col-md-4">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtAdditionalCharge" runat="server" TabIndex="22" CssClass="form-control"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="hfAdditionalCharge" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hfAdditionalChargeType" runat="server"></asp:HiddenField>
                                                        <span class="input-group-addon">
                                                            <asp:CheckBox ID="cbAdditionalCharge" runat="server" Text="" CssClass="customChkBox"
                                                                onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                                TabIndex="8" Checked="True" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2" style="text-align: right;">
                                                    <asp:Label ID="lblTotalRoomRateOrRoomTariff" runat="server" CssClass="control-label" Text="Label"></asp:Label>
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
                                        <div class="form-group" id="DivAddedRoom" style="display: none">
                                            <label for="RoomNumber" class="control-label col-md-2">
                                                Added Room</label>
                                            <div class="col-md-10">
                                                <asp:Label ID="lblAddedRoomNumber" class="control-label" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <input type="button" id="btnAddDetailGuest" tabindex="19" value="Add" class="TransactionalButton btn btn-primary btn-sm" />
                                                <input type="button" id="btnDiscard" onclick="CancelRoomAddEdit()" tabindex="19"
                                                    value="Cancel" class="btn btn-primary btn-sm" />
                                                <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                                                <asp:HiddenField ID="ddlRoomTypeIdHiddenField" runat="server"></asp:HiddenField>
                                            </div>
                                        </div>
                                        <div id="ReservationDetailGrid" style="padding-top: 10px;">
                                            <table class="table table-bordered table-condensed table-responsive" id='ReservationRoomGrid'>
                                                <thead>
                                                    <tr style='color: White; background-color: #44545E; font-weight: bold;'>
                                                        <th style="width: 30%;">Room Type
                                                        </th>
                                                        <th style="width: 38%;">Room Numbers
                                                        </th>
                                                        <th style="width: 12%; display: none;">Total Room Rate
                                                        </th>
                                                        <th style="width: 12%;">Pax
                                                        </th>
                                                        <th style="width: 8%;">Action
                                                        </th>
                                                        <th style="display: none;">ReserVation Details Id
                                                        </th>
                                                        <th style="display: none;">Room Type Id
                                                        </th>
                                                        <th style="display: none;">Room Id
                                                        </th>
                                                        <th style="display: none;">Room Number
                                                        </th>
                                                        <th style="display: none;">Total Room
                                                        </th>
                                                        <th style="display: none;">Discount Type
                                                        </th>
                                                        <th style="display: none;">Discount Amount
                                                        </th>
                                                        <th style="display: none;">Unit Price Rack Rate
                                                        </th>
                                                        <th style="display: none;">Room Rate Negotiated Rate
                                                        </th>
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
                        <div class="form-group">
                            <label for="Reference" class="control-label col-md-2">
                                Market Segment</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlMarketSegment" runat="server" CssClass="form-control" TabIndex="20">
                                </asp:DropDownList>
                            </div>
                            <label for="GuestSource" class="control-label col-md-2" id="lblGuestSource">
                                Guest Source</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGuestSource" runat="server" CssClass="form-control" TabIndex="64">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Reference" class="control-label col-md-2" id="lblReferenceId">
                                Reference</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlReferenceId" runat="server" CssClass="form-control" TabIndex="20">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Reference" class="control-label col-md-2">
                                Bookers Name</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtBookersName" CssClass="form-control" runat="server" TabIndex="22"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="ReservationStatus" class="control-label col-md-2">
                                Classification</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlClassification" runat="server" CssClass="form-control"
                                    TabIndex="21">
                                </asp:DropDownList>
                            </div>
                            <div id="VIPTypeDiv" style="display: none;">
                                <label for="VIPType" class="control-label col-md-2">
                                    VIP Type</label>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlVIPGuestType" runat="server" CssClass="form-control" TabIndex="65">
                                    </asp:DropDownList>
                                    <div style="display: none;">
                                        <asp:CheckBox ID="chkIsVIPGuest" TabIndex="73" runat="Server" Text="Is VIP guest?"
                                            TextAlign="right" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList ID="ddlIsComplementary" runat="server" CssClass="form-control" TabIndex="65">
                                        <asp:ListItem Value="0">Non Complementary</asp:ListItem>
                                        <asp:ListItem Value="1">Complementary</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Reference" class="control-label col-md-2">
                                Meal Plan</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlMealPlanId" runat="server" CssClass="form-control" TabIndex="65">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="NoShowChargeDiv" style="display: none;">
                            <label for="ChargeAmount" class="control-label col-md-2">
                                Charge Amount</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNoShowCharge" CssClass="form-control" runat="server" TabIndex="22"></asp:TextBox>
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlCashPaymentAccountHeadForNoShow" runat="server" CssClass="form-control"
                                        TabIndex="21">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="PendingDiv">
                            <label for="RoomNumber" class="control-label col-md-2">
                                Confirmation Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtConfirmationDate" CssClass="form-control" runat="server" TabIndex="22"></asp:TextBox>
                            </div>
                            <label for="ProbableArrivalTime" class="control-label col-md-2">
                                Probable Arrival Time</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtProbableArrivalPendingTime" placeholder="12" runat="server" CssClass="form-control"
                                    TabIndex="23"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" id="ReasonDiv">
                            <label for="CancelReason" class="control-label col-md-2">
                                Cancel Reason</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="26"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Remarks" class="control-label col-md-2">
                                Hotel Remarks</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="27"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Remarks" class="control-label col-md-2">
                                Guest Remarks</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtGuestRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="27"></asp:TextBox>
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
                        <div class="form-group">
                            <label for="ReservationStatus" class="control-label col-md-2">
                                Reservation Status</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReservationStatus" runat="server" CssClass="form-control"
                                    TabIndex="21">
                                    <asp:ListItem Value="Confirmed">Confirmed</asp:ListItem>
                                    <asp:ListItem Value="Waiting">Waiting</asp:ListItem>
                                    <asp:ListItem Enabled="false" Value="Cancel">Cancel</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-6 aspBoxText" style="width: 500px;">
                                <asp:CheckBox ID="chkIsRoomRateShowInPreRegistrationCard" runat="server" Checked="true" CssClass="aspBoxAlign" />&nbsp;
                                        Is Room Rate Show in Room Reservation Letter and Pre Registration Card                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="childDivSection">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div style="display: none;">
                            <div class="form-group">
                                <label for="Person Adult" class="control-label col-md-2">
                                    Person (Adult)</label>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <input id="EditId" runat="server" type="hidden" />
                                            <asp:TextBox ID="txtNumberOfPersonAdult" runat="server" Width="110px" CssClass="form-control quantitydecimal"
                                                TabIndex="28">1</asp:TextBox>
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
                                    <asp:TextBox ID="txtNumberOfPersonChild" runat="server" CssClass="form-control quantitydecimal" TabIndex="29"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Title" class="control-label required-field col-md-2">
                                Title</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGuestTitle" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="FirstName" class="control-label required-field col-md-2">
                                First Name</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGuestFirstName" runat="server" CssClass="form-control" TabIndex="30"></asp:TextBox>
                            </div>
                            <label for="LastName" class="control-label col-md-2">
                                Last Name</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGuestLastName" runat="server" CssClass="form-control" TabIndex="30"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Name" class="control-label col-md-2">
                                Name</label>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtGuestName" CssClass="form-control" runat="server" TabIndex="30"
                                    disabled="disabled"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <button type="button" id="btnAddPerson" tabindex="31" class="btn btn-primary btn-sm">
                                    Search Guest</button>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="DateOfBirth" class="control-label col-md-2">
                                Date Of Birth</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGuestDOB" runat="server" CssClass="form-control" TabIndex="32"></asp:TextBox>
                            </div>
                            <label for="Gender" class="control-label required-field col-md-2">
                                Gender</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGuestSex" runat="server" CssClass="form-control" TabIndex="33">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem>Male</asp:ListItem>
                                    <asp:ListItem>Female</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="CompanyName" class="control-label col-md-2">
                                Company Name</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtGuestAddress1" runat="server" CssClass="form-control" TabIndex="34"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Address" class="control-label col-md-2">
                                Address</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtGuestAddress2" runat="server" CssClass="form-control" TabIndex="35"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Profession" class="control-label col-md-2">
                                Profession</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlProfessionId" runat="server" TabIndex="40" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="EmailAddress" class="control-label col-md-2">
                                Email Address</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtGuestEmail" runat="server" TabIndex="36" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="display: none;">
                            <label for="Profession1" class="control-label col-md-2">
                                Classification</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlClassificationId" runat="server" TabIndex="40" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="MobileNumber" class="control-label col-md-2">
                                Mobile Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGuestPhone" runat="server" CssClass="form-control" TabIndex="37"></asp:TextBox>
                            </div>
                            <label for="City" class="control-label col-md-2">
                                City</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGuestCity" runat="server" CssClass="form-control" TabIndex="38"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="ZipCode" class="control-label col-md-2">
                                Zip Code</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGuestZipCode" runat="server" CssClass="form-control" TabIndex="39"></asp:TextBox>
                            </div>
                            <label for="Country" class="control-label required-field col-md-2">
                                Country</label>
                            <div class="col-md-4">
                                <input id="txtGuestCountrySearch" type="text" class="form-control" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlGuestCountry" runat="server" CssClass="form-control" TabIndex="40">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Nationality" class="control-label col-md-2">
                                Nationality</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGuestNationality" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                            </div>
                            <label for="DrivingLicense" class="control-label col-md-2">
                                Driving License</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGuestDrivinlgLicense" runat="server" CssClass="form-control"
                                    TabIndex="42"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="NationalID" class="control-label col-md-2">
                                National ID</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNationalId" runat="server" CssClass="form-control" TabIndex="43"></asp:TextBox>
                            </div>
                            <label for="VisaNumber" class="control-label col-md-2">
                                Visa Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtVisaNumber" runat="server" CssClass="form-control" TabIndex="44"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="VisaIssueDate" class="control-label col-md-2">
                                Visa Issue Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtVIssueDate" CssClass="form-control" runat="server" TabIndex="45"></asp:TextBox>
                            </div>
                            <label for="VisaExpiryDate" class="control-label col-md-2">
                                Visa Expiry Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtVExpireDate" CssClass="form-control" runat="server" TabIndex="46"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="PassportNumber" class="control-label col-md-2">
                                Passport Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassportNumber" runat="server" CssClass="form-control" TabIndex="47"></asp:TextBox>
                            </div>
                            <label for="PassIssueDate" class="control-label col-md-2">
                                Pass. Issue Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPIssueDate" runat="server" CssClass="form-control" TabIndex="49"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="PassExpiryDate" class="control-label col-md-2">
                                Pass. Expiry Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPExpireDate" runat="server" CssClass="form-control" TabIndex="50"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-md-offset-2">
                                <input class="checkBlock" type="checkbox" id="chkYesBlock" />
                                Blocked Guest
                            </div>
                        </div>
                        <div class="form-group" id="GuestPreferenceDiv" style="display: none">
                            <label for="GuestPreferences" class="control-label col-md-2">
                                Guest Preferences</label>
                            <div class="col-md-10">
                                <asp:Label ID="lblGstPreference" runat="server" class="control-label"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div id="RoomNumberDiv" style="display: none">
                                <label for="RoomNumber" class="control-label col-md-2">
                                    Room Number</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlRoomNumber" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <input type="button" tabindex="18" id="btnGuestReferences" value="Preferences" class="btn btn-primary btn-sm"
                                    onclick="javascript: return LoadGuestReference()" />
                            </div>
                        </div>
                        <div id="GuestDetailsAdd" class="row">
                            <div class="col-md-12">
                                <input id="btnAddGuest" type="button" value="Add" class="btn btn-primary btn-sm"
                                    tabindex="51" />
                                <input type="button" tabindex="18" id="btnCancelDetailGuest" value="Clear" class="btn btn-primary btn-sm"
                                    onclick="javascript: return clearUserDetailsControl()" />
                                <asp:Label ID="Label2" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div style="float: right; padding-right: 30px">
                                <input id="btnNext2" type="button" tabindex="54" value="Next" class="btn btn-primary btn-sm"
                                    style="display: none;" />
                            </div>
                            <div style="float: right; padding-right: 10px">
                                <input id="btnPrev1" type="button" tabindex="53" value="Prev" class="btn btn-primary btn-sm"
                                    style="display: none;" />
                            </div>
                        </div>
                        <div id="ltlGuestDetailGrid">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Arrival Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="AirportPick-Up" class="control-label col-md-2 required-field">
                                Pick Up</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAirportPickUp" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="0">--Please Select--</asp:ListItem>
                                    <asp:ListItem>NO</asp:ListItem>
                                    <asp:ListItem>YES</asp:ListItem>
                                    <asp:ListItem>TBA</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="MultiplePickup" style="display: none">
                                <div class="col-md-2" style="display: none">
                                    <asp:CheckBox ID="chkMultiplePickup" runat="server" OnClick="OnMultiplePickup();" />&nbsp;&nbsp;&nbsp;&nbsp;Multiple
                                    Pickup
                                </div>
                            </div>
                            <div id="MultipleGuest" style="display: none">
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlGuest" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="AirportPickUpInformationDiv">
                            <div class="form-group">
                                <label for="AirlineName" id="lblAirlineNameMandatory" class="control-label col-md-2 required-field">Vehicle Name</label>
                                <label for="AirlineName" id="lblAirlineNameNoMandatory" style="display: none;" class="control-label col-md-2">Vehicle Name</label>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlArrivalFlightName" runat="server" TabIndex="10" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="FlightNumber" id="lblFlightNumberMandatory" class="control-label col-md-2">Flight Number</label>
                                <label for="FlightNumber" id="lblFlightNumberNoMandatory" style="display: none;" class="control-label col-md-2">Flight Number</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtArrivalFlightNumber" runat="server" CssClass="form-control" TabIndex="55"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="ArrivalTime" class="control-label col-md-2">
                                    Arrival Time (ETA)</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtArrivalTime" placeholder="12" runat="server" CssClass="form-control"
                                        TabIndex="56"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="ArrivalTime" class="control-label col-md-2">
                                    &nbsp;</label>
                                <div class="col-md-4">
                                    <asp:CheckBox ID="chkIsArrivalChargable" runat="server" Text="" onclick="javascript: return ToggleFieldVisibleForAllActiveReservation(this);"
                                        TabIndex="2" />&nbsp;&nbsp;
                                    <asp:Label ID="Label12" runat="server" class="control-label" Text="Is Chargable"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="addMultiplePickup" style="display: none">
                    <div class="row">
                        <div class="col-md-12">
                            <input id="btnaddMultiplePickup" type="button" value="Add" class="btn btn-primary btn-sm"
                                tabindex="51" />
                        </div>
                    </div>
                    <div class="form-group" id="ArrivalTableContainer" style="overflow: scroll;">
                        <table id="PickupTable" class="table table-bordered table-condensed table-responsive">
                            <thead>
                                <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                    <th style="width: 25%;">Guest Name
                                    </th>
                                    <th style="width: 25%;">Vehicle Name
                                    </th>
                                    <th style="width: 20%;">Flight Number
                                    </th>
                                    <th style="width: 20%;">Arrival Time
                                    </th>
                                    <th style="width: 10%;">Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
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
                                    <asp:ListItem Value="0">-- Please Select --</asp:ListItem>
                                    <asp:ListItem>NO</asp:ListItem>
                                    <asp:ListItem>YES</asp:ListItem>
                                    <asp:ListItem>TBA</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="MultipleDrop" style="display: none">
                                <div class="col-md-2" style="display: none">
                                    <asp:CheckBox ID="chkMultipleDrop" runat="server" OnClick="OnMultipleDrop();" />&nbsp;&nbsp;&nbsp;&nbsp;Multiple
                                    Drop
                                </div>
                            </div>
                            <div id="MultipleDropGuest" style="display: none">
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlDropGuest" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="AirportDropInformationDiv">
                            <div class="form-group">
                                <label for="AirlineName" class="control-label col-md-2">
                                    Vehicle Name</label>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlDepartureFlightName" runat="server" TabIndex="10" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="FlightNumber" class="control-label col-md-2">
                                    Flight Number</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtDepartureFlightNumber" runat="server" CssClass="form-control"
                                        TabIndex="60"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="DepartureTime" class="control-label col-md-2">
                                    Departure Time (ETD)</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDepartureTime" placeholder="12" runat="server" CssClass="form-control"
                                        TabIndex="61"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="ArrivalTime" class="control-label col-md-2">
                                    &nbsp;</label>
                                <div class="col-md-4">
                                    <asp:CheckBox ID="chkIsDepartureChargable" runat="server" Text="" onclick="javascript: return ToggleFieldVisibleForAllActiveReservation(this);"
                                        TabIndex="2" />&nbsp;&nbsp;
                                    <asp:Label ID="Label1" runat="server" class="control-label" Text="Is Chargable"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div id="addMultipleDrop" style="display: none">
                            <div class="row">
                                <div class="col-md-12">
                                    <input id="btnaddMultipleDrop" type="button" value="Add" class="btn btn-primary btn-sm"
                                        tabindex="51" />
                                </div>
                            </div>
                            <div class="divSection" id="DepartureTableContainer" style="overflow: scroll;">
                                <table id="DepartureTable" class="table table-bordered table-condensed table-responsive">
                                    <thead>
                                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                            <th style="width: 25%;">Guest Name
                                            </th>
                                            <th style="width: 25%;">Vehicle Name
                                            </th>
                                            <th style="width: 20%;">Flight Number
                                            </th>
                                            <th style="width: 20%;">Departure Time
                                            </th>
                                            <th style="width: 10%;">Action
                                            </th>
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
        </div>
        <div id="tab-4">
            <div class="panel-body">
                <div class="checkboxlistHeader">
                    <asp:CheckBox ID="chkAll" Text="Select All" runat="server" Font-Bold="True"
                        ForeColor="#009933" />&nbsp;
                    <asp:Label ID="lblTotalSelectedEmailCount" runat="server" ForeColor="Red"
                        Style="font-weight: 700" Text=""></asp:Label>
                </div>
                <div class="checkbox checkboxlist col-md-12">
                    <asp:CheckBoxList ID="chkComplementaryItem" runat="server">
                    </asp:CheckBoxList>
                </div>
            </div>
        </div>
        <div id="tab-5">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="FromDate" class="control-label col-md-2">
                                    Search Date</label>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" placeholder="From Date" TabIndex="63"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtToDate" runat="server" TabIndex="64" CssClass="form-control" placeholder="To Date"></asp:TextBox>
                                </div>
                                <label for="ReservationNumber" class="control-label col-md-2">
                                    Reservation Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSearchReservationNumber" runat="server" CssClass="form-control"
                                        TabIndex="66"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="GuestName" class="control-label col-md-2">
                                    Guest Name</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSrcReservationGuest" runat="server" CssClass="form-control" TabIndex="65"></asp:TextBox>
                                </div>
                                <label for="CompanyName" class="control-label col-md-2">
                                    Company Name</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSearchCompanyName" runat="server" CssClass="form-control" TabIndex="66"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="ContactPerson" class="control-label col-md-2">
                                    Contact Person</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCntPerson" runat="server" CssClass="form-control" TabIndex="67"></asp:TextBox>
                                </div>
                                <label for="MobileNumber" class="control-label col-md-2">
                                    Phone/ Mobile No.</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSrcContactPhone" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="Reference" class="control-label col-md-2">
                                    Contact Email</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSrcContactEmail" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                                </div>
                                <label for="GuestSource" class="control-label col-md-2">
                                    Guest Source</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlSrcGuestSource" runat="server" CssClass="form-control" TabIndex="64">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="Reference" class="control-label col-md-2">
                                    Market Segment</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlSrcMarketSegment" runat="server" CssClass="form-control" TabIndex="20">
                                    </asp:DropDownList>
                                </div>
                                <label for="Reference" class="control-label col-md-2">
                                    Reference</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlSrcReferenceId" runat="server" CssClass="form-control" TabIndex="20">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="SearchOrdering" class="control-label col-md-2">
                                    Search Ordering</label>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-8">
                                            <asp:DropDownList ID="ddlOrderCriteria" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="CheckInDate">Check In Date</asp:ListItem>
                                                <asp:ListItem Value="ReservationNo">Reservation No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4 col-padding-left-none">
                                            <asp:DropDownList ID="ddlorderOption" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="DESC">DESC</asp:ListItem>
                                                <asp:ListItem Value="ASC">ASC</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <label for="Status" class="control-label col-md-2">
                                    Reservation Status</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlSearchByStatus" runat="server" CssClass="form-control"
                                        TabIndex="21">
                                        <asp:ListItem Value="All">All</asp:ListItem>
                                        <asp:ListItem Value="Confirmed">Confirmed</asp:ListItem>
                                        <asp:ListItem Value="Waiting">Waiting</asp:ListItem>
                                        <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                        TabIndex="5" OnClick="btnSearch_Click" />
                                    <button type="button" id="btnClearSearch" class="btn btn-primary btn-sm" onclick="PerformClearSearchAction()">Clear</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvRoomRegistration" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="50" OnRowCommand="gvRoomRegistration_RowCommand"
                        OnRowDataBound="gvRoomRegistration_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("ReservationId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ReservationNumber" HeaderText="Reserv. No." ItemStyle-Width="9%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GuestName" HeaderText="Guest Name" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CompanyName" HeaderText="Company" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RoomInformation" HeaderText="Room Info" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Check In" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvDateIn" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("DateIn"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check Out" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvDateOut" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("DateOut"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ReservationMode" HeaderText="Status" ItemStyle-Width="7%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="ReservationStatusInfo" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblReservationStatusInfo" runat="server" Text='<%#Eval("ReservationMode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PickUpDropStatusInfo" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblPickUpDropStatusInfo" runat="server" Text='<%#Eval("PickUpDropCount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GroupMasterId" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblGroupMasterId" runat="server" Text='<%#Eval("GroupMasterId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="31%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imageActive" ToolTip="Reservation Active" AlternateText="Reservation Active"
                                        ImageUrl="~/Images/select.png" runat="server" OnClientClick='<%#String.Format("return ActiveReservation({0})", Eval("ReservationId")) %>' />
                                    <asp:ImageButton ID="imgChangeStatus" ToolTip="Reservation Cancel" AlternateText="Reservation Cancel"
                                        ImageUrl="~/Images/delete.png" runat="server" OnClientClick='<%#String.Format("return CancelReservation({0},\"{1}\")", Eval("ReservationId"),Eval("ReservationMode")) %>' />
                                    <asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("ReservationId") %>'
                                        CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to save?');"
                                        Text="" AlternateText="Delete" ToolTip="Delete" />
                                    &nbsp;<asp:ImageButton ID="ImgRoomCurrentStatus" runat="server" CausesValidation="False" CommandArgument='<%# bind("ReservationId") %>'
                                        CommandName="CmdRoomCurrentStatus" ImageUrl="~/Images/salesQuotation.png" Text="" AlternateText="Current Room Status"
                                        ToolTip="Current Room Status" OnClientClick='<%#String.Format("return ShowReservationCurrentRoomStatus({0},\"{1}\")", Eval("ReservationId"),Eval("ReservationNumber")) %>' />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("ReservationId") %>'
                                        CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                        ToolTip="Edit" OnClientClick='<%#String.Format("return EditRoomReservation()") %>' />
                                    &nbsp<asp:ImageButton ID="ImgCopy" runat="server" CausesValidation="False" CommandArgument='<%# bind("ReservationId") %>'
                                        CommandName="CmdCopy" ImageUrl="~/Images/copy.png" Text="" AlternateText="Edit"
                                        ToolTip="Copy To" OnClientClick='<%#String.Format("return CopyToRoomReservation()") %>' />
                                    &nbsp;<asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False"
                                        CommandArgument='<%# bind("ReservationId") %>' CommandName="CmdBillPreview" ImageUrl="~/Images/ReportDocument.png"
                                        Text="" AlternateText="Reservation Letter" ToolTip="Reservation Letter" />
                                    &nbsp;<asp:ImageButton ID="ImgGroupReservationLetter" runat="server" CausesValidation="False"
                                        CommandName="CmdDetails" CommandArgument='<%# bind("GroupMasterId") %>' OnClientClick='<%#String.Format("return PerformGroupBillPreviewAction({0})", Eval("GroupMasterId")) %>'
                                        ImageUrl="~/Images/ReportDocument.png" Text="" AlternateText="Group Reservation Letter" ToolTip="Group Reservation Letter" />
                                    &nbsp;<asp:ImageButton ID="ImgPreRegistrationCard" runat="server" CausesValidation="False"
                                        CommandArgument='<%# bind("ReservationId") %>' CommandName="CmdPreRegistrationCard" ImageUrl="~/Images/ReportDocument.png"
                                        Text="" AlternateText="Pre Registration Card" ToolTip="Pre Registration Card" />
                                    &nbsp;<asp:ImageButton ID="ImgBillPreviewForMultiplePickUpDrop" runat="server" CausesValidation="False"
                                        CommandName="CmdDetails" CommandArgument='<%# bind("ReservationId") %>' OnClientClick='<%#String.Format("return MultiplePickUpDropInfo({0})", Eval("ReservationId")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Bill Preview" />                                    
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
                    <div class="childDivSection">
                        <div class="text-center" id="Div2">
                            <ul class="pagination">
                                <asp:Literal ID="gridPaging" runat="server"></asp:Literal>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" id="SubmitButtonDiv" style="padding: 10px 0 0 25px;">
            <div class="col-md-12">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                    TabIndex="80" OnClientClick="javascript: return ValidateGuestNumber();" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="btn btn-primary btn-sm"
                    TabIndex="81" OnClick="btnCancel_Click" />
            </div>
        </div>
        <div id="TouchKeypad" style="display: none;">
            <div id="PopMessageBox" class="alert alert-info" style="display: none;">
                <button type="button" class="close" data-dismiss="alert">
                    ×</button>
                <asp:Label ID='lblPopMessageBox' Font-Bold="True" runat="server"></asp:Label>
            </div>
            <div id="PopEntryPanel" class="panel panel-default" style="width: 875px">
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
                                    <div class="col-md-11">
                                        <asp:HiddenField ID="hiddenGuestId" runat="server" />
                                        <asp:HiddenField ID="hiddenGuestName" runat="server" />
                                        <asp:TextBox ID="txtSrcGuestName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                    </div>
                                    <div class="form-inline col-md-1">
                                        <img id="imgCollapse" width="30px" src="/HotelManagement/Image/expand_alt.png" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="ExtraSearch">
                            <div class="form-group">
                                <label for="CompanyName" class="control-label col-md-2">
                                    Company Name</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtSrcCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="RoomNumber" class="control-label col-md-2">
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
                            <div class="form-group">
                                <label for="FromDate" class="control-label col-md-2">
                                    From Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSrcFromDate" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                </div>
                                <label for="ToDate" class="control-label col-md-2">
                                    To Date</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSrcToDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="EmailAddress" class="control-label col-md-2">
                                    Email Address</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSrcEmailAddress" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                </div>
                                <label for="MobileNumber" class="control-label col-md-2">
                                    Mobile Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSrcMobileNumber" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="NationalID" class="control-label col-md-2">
                                    National ID</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSrcNationalId" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                </div>
                                <label for="DateOfBirth" class="control-label col-md-2">
                                    Date of Birth</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSrcDateOfBirth" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="PassportNumber" class="control-label col-md-2">
                                    Passport Number</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtSrcPassportNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnPopSearch" class="btn btn-primary btn-sm">
                                    Search</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="PopSearchPanel" class="panel panel-default" style="width: 875px">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <table id='gvGustIngormation' class="table table-bordered table-condensed table-responsive">
                        <colgroup>
                            <col style="width: 40%;" />
                            <col style="width: 25%;" />
                            <col style="width: 15%;" />
                            <col style="width: 20%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Guest Name
                                </td>
                                <td>Country Name
                                </td>
                                <td>Phone
                                </td>
                                <td>Email
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="childDivSection">
                        <div class="text-center" id="GridPagingContainer">
                            <ul class="pagination">
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div style="height: 45px">
            </div>
            <div id="PopTabPanel" style="width: 900px">
                <div id="PopMyTabs">
                    <ul id="PoptabPage" class="ui-style">
                        <li id="PopA" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                            <a href="#Poptab-1">Guest Information</a></li>
                        <li id="PopB" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                            <a href="#Poptab-2">Guest Documents</a></li>
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
                                            <asp:Label ID="lblLPassportNumber" runat="server" Text="Passport Number"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPassportNumber" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLPIssueDate" runat="server" Text="Pasport Issue Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPIssueDate" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLPIssuePlace" runat="server" Text="Passport Issue Place"></asp:Label>
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
                                            <asp:Label ID="lblLVisaNumber" runat="server" Text="Visa Number"></asp:Label>
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
                                        <td class="span2">
                                            <%--<asp:Label ID="lblPreferences" runat="server" Text="Guest Preferences"></asp:Label>--%>
                                        </td>
                                        <td class="span4">
                                            <%--<asp:Label ID="lblGuestPreferences" runat="server" Text=""></asp:Label>--%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div id="Poptab-2">
                        <div id="imageDiv">
                        </div>
                    </div>
                    <div id="Poptab-3">
                        <div class="HMBodyContainer">
                            <div id="guestHistoryDiv">
                            </div>
                        </div>
                    </div>
                    <div id="Poptab-4">
                        <div id="Div6" class="panel panel-default" style="font-weight: bold">

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
                    <button type="button" id="btnPrintDocument" class="btn btn-primary btn-sm">
                        Print</button>
                </div>
            </div>
        </div>
        <!--  End Pop Up Guest Search  -->
        <!--Room Load PopUp -->
        <div id="DivRoomSelect" style="display: none;">
            <div id="Div1">
                <asp:HiddenField ID="hfSelectedRoomNumbers" runat="server" />
                <asp:HiddenField ID="hfSelectedRoomId" runat="server" />
                <asp:HiddenField ID="hfSelectedRoomReservedId" runat="server" />
                <div style="height: 300px; overflow-y: scroll" id="ltlRoomNumberInfo">
                </div>
                <div style='margin-top: 12px;'>
                    <button type='button' onclick='javascript:return GetCheckedRoomCheckBox()' id='btnAddRoom' class='btn btn-primary' style="width: 65px">OK</button>
                    <button type='button' onclick='javascript:return CloseRoomDialog()' id='btnCancelRoom' class='btn btn-primary'>Cancel</button>
                </div>
            </div>
        </div>
        <!-- Pop Up Guest Search -->
        <div id="ReservationPopup" style="display: none;">
            <div id="Div3" class="alert alert-info" style="display: none;">
                <button type="button" class="close" data-dismiss="alert">
                    ×</button>
                <asp:Label ID='Label17' Font-Bold="True" runat="server"></asp:Label>
            </div>
            <div id="ReservationPopEntryPanel" class="panel panel-default" style="width: 875px">
                <div class="panel-heading">
                    Search Guest
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="GuestName" class="control-label col-md-2">
                                Guest Name</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPrevGstName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Country" class="control-label col-md-2">
                                Country</label>
                            <div class="col-md-4">
                                <input id="txtPrevGstCountry" type="text" class="form-control" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlPrevGstCountry" runat="server" CssClass="form-control" TabIndex="40">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <label for="Phone" class="control-label col-md-2">
                                Phone</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPrevGstPhn" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="Email" class="control-label col-md-2">
                                Email</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPrevGstEmail" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="PassportNumber" class="control-label col-md-2">
                                Passport Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcPopPassportNumber" runat="server" CssClass="form-control"
                                    TabIndex="1"></asp:TextBox>
                            </div>
                            <label for="NationalID" class="control-label col-md-2">
                                National ID</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcPopNationalId" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="display: none;">
                            <label for="Company" class="control-label col-md-2">
                                Company</label>
                            <div class="col-md-10">
                                <input id="txtSrcPopCompany" type="text" class="form-control" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlSrcPopCompany" runat="server" CssClass="form-control" TabIndex="40">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <button type="button" id="btnReservationSearch" class="btn btn-primary btn-sm">
                                Search</button>
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
                </div>
            </div>
        </div>
    </div>
    <!-- End Pop Up Guest Search -->
    <!--Guest Reference PopUp -->
    <div id="DivGuestReference" style="display: none;">
        <div id="Div5">
            <div id="ltlGuestReference">
            </div>
        </div>
    </div>
    <!--End Guest Reference PopUp -->
    <div id="reservationCancelPopUp" style="display: none;" class="panel panel-default">
        <asp:HiddenField ID="hfCancelReservationId" runat="server" Value="0" />
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group" style="display: none;">
                    <div class="col-md-3">
                        <asp:Label runat="server" class="control-label" Text="Reservation Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <select id="sCancelStatus" class="form-control">
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-3">
                        <asp:Label runat="server" class="control-label required-field" Text="Remarks"></asp:Label>
                    </div>
                    <div class="col-md-9">
                        <asp:TextBox ID="tbRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                            TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <button class="btn btn-primary btn-sm" type="button" onclick="return ConfirmCancelReservation();">
                            Confirm</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="ReservationRoomStatusPopUp" style="display: none;" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group" style="display: none;">
                    <div class="col-sm-12">
                        <div id="ltlReservationRoomStatusPopUp" style="overflow: scroll;">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=_RoomReservationId%>';
        if (x > -1)
            EntryPanelVisibleTrue();

        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
            if (parseInt(xNewAdd) == 2) {
                $('#btnNewReservationPanel').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }

        var isListedCompanyVisible = '<%=isListedCompanyVisible%>';
        if (isListedCompanyVisible > -1) {
            ListedCompanyVisibleTrue();
        }
        else {
            ListedCompanyVisibleFalse();
        }

        var isListedCompanyDropDownVisible = '<%=isListedCompanyDropDownVisible%>';
        if (isListedCompanyDropDownVisible > -1) {
            ListedCompanyDropDownVisibleTrue();
        }
        else {
            ListedCompanyDropDownVisibleFalse();
        }

        $(document).ready(function () {
            //active Tabs Id
            var active = $("#myTabs .ui-tabs-panel:visible").attr("id");;

            if (active == "tab-5") {
                $('#SubmitButtonDiv').hide();
            }

            if ($("#<%=ddlCompanyName.ClientID %>").val() == "0") {
                $('#txtCompany').val("");
            }
            else {
                $('#txtCompany').val($("#<%=ddlCompanyName.ClientID %> option:selected").text());
            }

            if ($("#<%=ddlCountry.ClientID %>").val() != "0") {
                $('#txtCountry').val($("#<%=ddlCountry.ClientID %> option:selected").text());
            }

            if ($("#<%=hfIsSameasContactPersonAsGuest.ClientID %>").val() == "1") {
                $("#ContentPlaceHolder1_IsSameasGuest").attr('checked', true);
            }

            if ($('#' + '<%=ddlReservedMode.ClientID%>').val() == "Self") {
                var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
                $(ctrl).prop('checked', false);
                ToggleListedCompanyInfo();
                $('#GroupDiv').hide();
                $('#PersonDiv').show();
                $('#ContactPersonDiv').show();
                $('#MobileNEmailDiv').show();
                $('#CurrencyDiv').show();
                $('#CurrencyAmountInformationDiv').hide();
            }
            else if ($('#' + '<%=ddlReservedMode.ClientID%>').val() == "Group") {
                var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
                $(ctrl).prop('checked', false);
                ToggleListedCompanyInfo();
                $('#GroupDiv').show();
                $('#PersonDiv').show();
                $('#ContactPersonDiv').show();
                $('#MobileNEmailDiv').show();
                $('#CurrencyDiv').show();
                $('#CurrencyAmountInformationDiv').hide();
            }
            else if ($('#' + '<%=ddlReservedMode.ClientID%>').val() == "Company") {
                $('#PersonDiv').hide();
                $('#GroupDiv').hide();
                $('#ContactPersonDiv').show();
                $('#MobileNEmailDiv').show();
                $('#CurrencyDiv').show();
                //$("#<%=ddlCurrency.ClientID %>").val("1");
                $('#CurrencyAmountInformationDiv').hide();
            }
            else {
                var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
                $(ctrl).prop('checked', false);
                ToggleListedCompanyInfo();
                $('#GroupDiv').hide();
                $('#PersonDiv').hide();
                $('#ContactPersonDiv').hide();
                $('#MobileNEmailDiv').hide();
                $('#CurrencyDiv').hide();
                //$("#<%=ddlCurrency.ClientID %>").val("1");
                $('#CurrencyAmountInformationDiv').hide();
            }
            VisibleListedCompany();

            var rcRoomTypeId = 0;
            if ($.trim(CommonHelper.GetParameterByName("SRT")) != "") {
                rcRoomTypeId = parseInt($.trim(CommonHelper.GetParameterByName("SRT")), 10);
            }

            if (rcRoomTypeId != 0) {
                $("#<%=txtDiscountAmount.ClientID %>").val(0);
                $("#<%=ddlRoomTypeId.ClientID %>").val(rcRoomTypeId);
                RoomDetailsByRoomTypeId(rcRoomTypeId);
                PerformFillFormActionByTypeId($('#<%=ddlRoomTypeId.ClientID%>').val());
                UpdateTotalCostWithDiscount();
                ClearRoomNumberAndId();
            }
        });

        if ($('#<%=hfIsServiceChargeEnableConfig.ClientID%>').val() == "0") {
            $('#ServiceChargeLabel').hide();
            $('#ServiceChargeControl').hide();
            $('#rrcServiceChargeDiv').hide();
        }
        else {
            $('#ServiceChargeLabel').show();
            $('#ServiceChargeControl').show();
            $('#rrcServiceChargeDiv').show();
        }

        if ($('#<%=hfIsCitySDChargeEnableConfig.ClientID%>').val() == "0") {
            $('#CityChargeLabel').hide();
            $('#CityChargeControl').hide();
            $('#rrcCityChargeDiv').hide();
        }
        else {
            $('#CityChargeLabel').show();
            $('#CityChargeControl').show();
            $('#rrcCityChargeDiv').show();
        }

        if ($('#<%=hfIsVatEnableConfig.ClientID%>').val() == "0") {
            $('#VatAmountLabel').hide();
            $('#VatAmountControl').hide();
            $('#rrcVatChargeDiv').hide();
        }
        else {
            $('#VatAmountLabel').show();
            $('#VatAmountControl').show();
            $('#rrcVatChargeDiv').show();
        }

        if ($('#<%=hfIsAdditionalChargeEnableConfig.ClientID%>').val() == "0") {
            $('#AdditionalChargeLabel').hide();
            $('#AdditionalChargeControl').hide();
            $('#rrcAdditionalChargeDiv').hide();
        }
        else {
            $('#AdditionalChargeLabel').show();
            $('#AdditionalChargeControl').show();
            $('#rrcAdditionalChargeDiv').show();
        }

        if ($('#<%=hfIsReservationRoomListButtonWillHide.ClientID%>').val() == "1") {
            $('#btnChange').hide();
        }
        else {
            $('#btnChange').show();
        }

    </script>

</asp:Content>
