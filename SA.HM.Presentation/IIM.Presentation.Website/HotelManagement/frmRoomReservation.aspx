<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmRoomReservation.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmRoomReservation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var minCheckInDate = "", editedRowIndex = "";
        var minCheckOutDate = "";
        var alreadySavePaidServices = [];
        var deletedRoomByType = new Array();
        var editedRow = "";
        var cvt = "";
        var SelectdPreferenceId = "";

        $(document).ready(function () {

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            minCheckInDate = $("#<%=hfMinCheckInDate.ClientID %>").val();
            var vvc = [];

            if ($("#ContentPlaceHolder1_hfReservationId").val() != "") {
                $("#ReservationRoomGrid tbody").append($("#ContentPlaceHolder1_RoomDetailsTemp").text());
                GetTempRegistrationDetailByWM();
            }

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Reservation</li>";
            var breadCrumbs = moduleName + formName;
            //$("#SearchPanel").hide('slow');
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#<%=ddlRoomTypeId.ClientID %>").change(function () {

                RoomDetailsByRoomTypeId($(this).val());
                ClearRoomNumberAndId();
            });

            VisibleListedCompany();
            ToggleListedCompanyInfo();

            $("#myTabs").tabs();

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
                    toastr.warning("The DateIn should not be empty.");
                    //CustomAlert('The DateIn should not be empty.', 'DateIn', 'Ok')
                    document.getElementById("<%=txtDateIn.ClientID%>").focus();
                    return false;
                }
                if (txtToDate == "") {
                    toastr.warning("The Expected DateOut should not be empty.");
                    //CustomAlert('The Expected DateOut should not be empty.', 'Expected DateOut', 'Ok')
                    document.getElementById("<%=txtToDate.ClientID%>").focus();
                    return false;
                }
                $("#<%=DateInHiddenField.ClientID %>").val(txtFromDate);
                $("#<%=DateOutHiddenField.ClientID %>").val(txtToDate);
                $("#<%=hfCurrencyHiddenField.ClientID %>").val($("#ContentPlaceHolder1_ddlCurrency").val());
            });

            //            $("#<%=ddlCurrency.ClientID %>").change(function () {
            //                RoomDetailsByRoomTypeId($('#<%=ddlRoomTypeId.ClientID%>').val());                
            //            });    

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

            function OnLoadConversionRateSucceeded(result) {
                if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                    //$("#<%=txtConversionRate.ClientID %>").val('');
                    //                    $('#<%=txtConversionRate.ClientID%>').attr("disabled", true);
                }
                else {
                    //$('#<%=txtConversionRate.ClientID%>').attr("disabled", false);
                    $("#<%=txtConversionRate.ClientID %>").val(result.BillingConversionRate);
                }

                RoomDetailsByRoomTypeId($('#<%=ddlRoomTypeId.ClientID%>').val());
                CurrencyRateInfoEnable();
            }

            function OnLoadConversionRateFailed() {
            }


            $("#<%=ddlDiscountType.ClientID %>").change(function () {
                UpdateTotalCostWithDiscount();
            });
            $("#<%=txtDiscountAmount.ClientID %>").blur(function () {
                UpdateTotalCostWithDiscount();
            });

            $("#<%=ddlCompanyName.ClientID %>").change(function () {
                GetTotalCostWithCompanyOrPersonalDiscount();
                DiscountPolicyByCompanyNRoomType();
            });
            $("#<%=ddlBusinessPromotionId.ClientID %>").change(function () {
                GetTotalCostWithCompanyOrPersonalDiscount();
            });

            $("#txtGuestCountrySearch").blur(function () {
                var countryId = $("#<%=ddlGuestCountry.ClientID %>").val();
                PageMethods.GetNationality(countryId, OnCountrySucceeded, OnCountryFailed);
            });

            CommonHelper.AutoSearchClientDataSource("txtGuestCountrySearch", "ContentPlaceHolder1_ddlGuestCountry", "ContentPlaceHolder1_ddlGuestCountry");

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

            $("#ContentPlaceHolder1_txtDateIn").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                minDate: minCheckInDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDate").datepicker("option", "maxDate", selectedDate);

                    var strDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtDateIn").val(), '/');
                    minCheckOutDate = GetStringFromDateTime(CommonHelper.DaysAdd(strDate, 1));

                    $("#ContentPlaceHolder1_txtDateOut").datepicker("option", {
                        minDate: minCheckOutDate
                    });
                }
            });

            $("#ContentPlaceHolder1_txtDateOut").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                minDate: minCheckOutDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDate").datepicker("option", "maxDate", selectedDate);

                    if ($("#ContentPlaceHolder1_txtDateOut").val() != "")
                        $("#txtReservationDuration").val(CommonHelper.DateDifferenceInDays(CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtDateIn").val(), '/'), CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtDateOut").val(), '/')));
                    else
                        $("#txtReservationDuration").val("");
                }
            });

            $("#txtReservationDuration").blur(function () {

                if (CommonHelper.IsInt($(this).val()) == false) {
                    toastr.info("Please Insert Valid Number.");
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

            $("#ContentPlaceHolder1_txtFromDate").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtGuestDOB").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                maxDate: minCheckInDate,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtVIssueDate").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtVExpireDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtVExpireDate").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtVIssueDate").datepicker("option", "maxDate", selectedDate);
                }

            });

            $("#ContentPlaceHolder1_txtPIssueDate").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtPExpireDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtPExpireDate").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtPIssueDate").datepicker("option", "maxDate", selectedDate);
                }
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
                else if ($("#<%=ddlReservationStatus.ClientID %>").val() == "Cancel") {
                    $('#ReasonDiv').show("slow");
                    $('#PendingDiv').hide("slow");
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
            if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "NO") {
                $('#AirportPickUpInformationDiv').hide();
                $('#AirportPickUpInformationDiv').hide();
                $("#<%=txtArrivalFlightName.ClientID %>").val("");
                $("#<%=txtArrivalFlightNumber.ClientID %>").val("");
            }
            else {
                $('#AirportPickUpInformationDiv').show();
            }
            $("#<%=ddlAirportPickUp.ClientID %>").change(function () {
                if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "NO") {
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
                $("#<%=txtDepartureFlightName.ClientID %>").val("");
                $("#<%=txtDepartureFlightNumber.ClientID %>").val("");
            }
            else {
                $('#AirportDropInformationDiv').show();
            }
            $("#<%=ddlAirportDrop.ClientID %>").change(function () {
                if ($("#<%=ddlAirportDrop.ClientID %>").val() == "NO") {
                    $('#AirportDropInformationDiv').hide();
                    $("#<%=txtDepartureFlightName.ClientID %>").val("");
                    $("#<%=txtDepartureFlightNumber.ClientID %>").val("");
                }
                else {
                    $('#AirportDropInformationDiv').show();
                }
            });

            //EnableDisable For DropDown Change event--------------
            $('#' + '<%=ddlReservedMode.ClientID%>').change(function () {
                $("#ContentPlaceHolder1_txtContactPerson").val("");
                $("#ContentPlaceHolder1_txtContactAddress").val("");
                $("#ContentPlaceHolder1_txtContactNumber").val("");
                $("#ContentPlaceHolder1_txtMobileNumber").val("");
                $("#ContentPlaceHolder1_txtContactEmail").val("");
                $("#ContentPlaceHolder1_txtFaxNumber").val("");

                if ($('#' + '<%=ddlReservedMode.ClientID%>').val() == "Personal") {
                    var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
                    $(ctrl).attr('checked', false);
                    ToggleListedCompanyInfo();
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
                PerformRoomAvailableChecking();
            });

            $("#PopSearchPanel").hide();
            $("#PopTabPanel").hide();
            $("#ExtraSearch").hide();
            ClearRoomNumberAndId();
            $("#btnPopSearch").click(function () {
                $("#PopSearchPanel").show('slow');
                $("#PopTabPanel").hide('slow');
                GridPaging(1, 1);
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
                AddNewItem();
            });
            $("#btnSearchSuccess").click(function () {
                $("#<%=txtGuestName.ClientID %>").val($("#<%=hiddenGuestName.ClientID %>").val());
                $("#PopSearchPanel").hide();
                $("#PopTabPanel").hide();
                popup(-1);
                LoadDataOnParentForm();
            });
            $("#btnSearchCancel").click(function () {
                $("#<%=hiddenGuestName.ClientID %>").val('');
                $("#<%=hiddenGuestId.ClientID %>").val('');
                $("#PopSearchPanel").hide();
                $("#PopTabPanel").hide();
                popup(-1);
            });

            $("#btnPrintDocument").click(function () {
                $("#<%=hiddenGuestName.ClientID %>").val('');
                $("#PopSearchPanel").hide();
                $("#PopTabPanel").hide();
                popup(-1);
                window.location.href = '/HotelManagement/Reports/frmReportPrintImage.aspx?GuestId='
                           + $("#<%=hiddenGuestId.ClientID %>").val();

            });
            $("#btnAddGuest").click(function () {
                // Xtra Validation..............................
                PageMethods.LoadReservationNRegistrationXtraValidation(OnLoadXtraValidationSucceeded, OnLoadXtraValidationFailed);
            });

            $("#tblRoomReserve").delegate("td > img.RoomreservationDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var reservationId = $.trim($(this).parent().parent().find("td:eq(8)").text());
                    var params = JSON.stringify({ pkId: reservationId });

                    var $row = $(this).parent().parent();
                    //$(this).parent().parent().remove();
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

            $("#cbReservationDuration").click(function () {
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
                var discountType = "", discountAmount = "0", unitPriceRackRate = "0", roomRateNegotiatedRate = "0", reservationDetailsId = "0";

                roomTypeId = $.trim($(row).find("td:eq(4)").text());
                reservationDetailsId = $.trim($(row).find("td:eq(3)").text());
                roomIds = $.trim($(row).find("td:eq(5)").text());
                roomNumbers = $.trim($(row).find("td:eq(6)").text());
                discountType = $.trim($(row).find("td:eq(8)").text());
                discountAmount = $.trim($(row).find("td:eq(9)").text());
                unitPriceRackRate = $.trim($(row).find("td:eq(10)").text());
                roomRateNegotiatedRate = $.trim($(row).find("td:eq(11)").text());
                totalReservedRooms = $.trim($(row).find("td:eq(7)").text());

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
                $('#DivAddedRoom').show();
                $("#btnAddDetailGuest").val("Edit");
            });
        });

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
                $('#CurrencyAmountInformationDiv').show();
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

            if (txtDiscountAmount == "")
                return;

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

            $('#' + txtRoomRate).val(unitPrice.toFixed(2));
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

            var totalPaidServiceAmount = CalculatePaidServiceTotal();
            unitPrice += totalPaidServiceAmount;

            var discount = 0.0;

            if (discountType == "Fixed") {
                discount = unitPrice - roomRate;
            }
            else {
                discount = unitPrice - roomRate;
                discount = ((discount / unitPrice) * 100).toFixed(2);
            }

            $('#ContentPlaceHolder1_txtDiscountAmount').val(discount);
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
                $("#<%=txtRoomRate.ClientID %>").val(result.RoomRateUSD);
            }
            else {
                $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRate);
                $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRate);
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
            var txtFromDate = '<%=txtFromDate.ClientID%>'
            ("#" + txtFromDate).val("");
            var txtToDate = '<%=txtToDate.ClientID%>'
            ("#" + txtToDate).val("");
        }

        function ToggleListedCompanyInfo() {
            var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
            if ($(ctrl).is(':checked')) {
                $('#ReservedCompany').hide("slow");
                $('#ListedCompany').show("slow");
                $('#PaymentInformation').show("slow");
            }
            else {
                $('#ListedCompany').hide("slow");
                $('#ReservedCompany').show("slow");
                $('#PaymentInformation').hide("slow");
                $(ctrl).prop('checked', false)
                $("#ContentPlaceHolder1_ddlCompanyName").val("0");
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
                popup(-1);
            }
            else {
                $('#DivAddedRoom').hide();
            }
        }
        function SaveRoomDetailsInformationByWebMethod() {

            var roomType = $("#ContentPlaceHolder1_ddlRoomTypeId option:selected").text();
            var roomTypeId = $("#ContentPlaceHolder1_ddlRoomTypeId").val();
            var reservationDetailsId = $("#ContentPlaceHolder1_hfSelectedRoomReservedId").val();

            var roomIds = "", roomNumbers = "", roomCount = 0, totalReservedRooms = 0;
            var roomNumbersDisplay = "";

            if ($("#ContentPlaceHolder1_txtRoomId").val() != "" && $("#ContentPlaceHolder1_txtRoomId").val() != "0") {
                totalReservedRooms = parseInt($("#ContentPlaceHolder1_txtRoomId").val(), 10);
            }

            if (totalReservedRooms == 0) {
                toastr.warning("Please Give Room Quantity.");
                return false;
            }

            roomNumbers = $("#ContentPlaceHolder1_hfSelectedRoomNumbers").val();
            roomIds = $("#ContentPlaceHolder1_hfSelectedRoomId").val();

            var roomIdsArr = roomIds.split(',');
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

            if (roomIds != "") {
                var totalRoomAdded = roomIds.split(',').length;

                if (isUnassignedRooms == false && parseInt(totalRoomAdded, 10) > 0 && (parseInt(totalRoomAdded, 10) < parseInt(totalReservedRooms, 10))) {
                    toastr.warning("Room Quantiy Must Equall With Selected Room Quantity.");
                    return false;
                }
            }

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
                $(editedRow).find("td:eq(3)").text(reservationDetailsId);
                $(editedRow).find("td:eq(4)").text(roomTypeId);
                $(editedRow).find("td:eq(5)").text(roomIds);
                $(editedRow).find("td:eq(6)").text(roomNumbers);
                $(editedRow).find("td:eq(7)").text(totalReservedRooms);
                $(editedRow).find("td:eq(8)").text(discountType);
                $(editedRow).find("td:eq(9)").text(discountAmount);
                $(editedRow).find("td:eq(10)").text(unitPriceRackRate);
                $(editedRow).find("td:eq(11)").text(roomRateNegotiatedRate);

                var reservationId = $("#ContentPlaceHolder1_hfReservationId").val();

                if (reservationId != "") {

                }

                ClearReservationDeatil();
                ClearRoomNumberAndId();
                ClearRommTypeAfterAdded();

                editedRow = "";

                return false;
            }

            var alreadyExists = $("#ReservationRoomGrid tbody tr").find("td:eq(4):contains('" + roomTypeId + "')").length;

            if (alreadyExists > 0) {
                toastr.info("Same Room Type Already Added. Please Edit To Change.");
                return false;
            }

            var grid = "", counter = 0;
            counter = $("#ReservationRoomGrid tbody tr").length;

            if (counter % 2 == 0) {
                grid += "<tr style='background-color:#E3EAEB;'>";
            }
            else {
                grid += "<tr style='background-color:White;'>";
            }

            grid += "<td align='left' style='width: 42%;'>" + roomType + "</td>";
            grid += "<td align='left' style='width: 50%;'>" + roomNumbersDisplay + "</td>";

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

            grid += "</tr>";

            $("#ReservationRoomGrid tbody").append(grid);

            ClearReservationDeatil();
            ClearRoomNumberAndId();
            ClearRommTypeAfterAdded();

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

            $('#btnAddDetailGuest').val("Add");
            if (Isvalid) {
                PageMethods.PerformSaveRoomDetailsInformationByWebMethod(isEdit, hfSelectedRoomNumbers, hfSelectedRoomId, txtUnitPriceHiddenField, txtRoomRate, txtRoomId, prevRoomTypeId, ddlRoomTypeId, ddlRoomTypeIdText, lblHiddenId, txtDiscountAmount, ddlCurrency, ddlDiscountType, ddlReservedMode, OnPerformSaveRoomDetailsSucceeded, OnPerformSaveRoomDetailsFailed);
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
            if (txtRoomId == "" || parseInt(txtRoomId) <= 0)
            { return false; }
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
        }
        function ListedCompanyVisibleFalse() {
            $('#CompanyInformation').hide();
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
            $("#<%=txtContactAddress.ClientID %>").val(result.CompanyAddress)
            $("#<%=txtContactNumber.ClientID %>").val(result.ContactNumber)
            $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson)
            $("#<%=txtMobileNumber.ClientID %>").val(result.TelephoneNumber)
            var prevDiscount = parseFloat($("#<%=txtDiscountAmount.ClientID %>").val());
            if (isNaN(prevDiscount)) {
                prevDiscount = 0;
            }
            $("#<%=ddlDiscountType.ClientID %>").val('Percentage');
            var resultFloat = parseFloat(result.DiscountPercent);

            $("#<%=txtDiscountAmount.ClientID %>").val(resultFloat);
            $("#<%=txtContactEmail.ClientID %>").val(result.EmailAddress)

            UpdateTotalCostWithDiscount();
            return false;
        }
        function GetCalculatedDiscountObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadDataOnParentForm() {
            var guestId = $("#<%=hiddenGuestId.ClientID %>").val();
            popup(-1);
            PageMethods.LoadDetailInformation(guestId, OnLoadParentFromDetailObjectSucceeded, OnLoadParentFromDetailObjectFailed);
            return false;

        }
        function OnLoadParentFromDetailObjectSucceeded(result) {
            if (result.GuestPreferences != null) {
                $("#GuestPreferenceDiv").show();
            }
            else $("#GuestPreferenceDiv").hide();
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
            $("#<%=txtPIssuePlace.ClientID %>").text(result.PIssuePlace);
            $("#<%=txtVisaNumber.ClientID %>").val(result.VisaNumber);
            $("#<%=ddlGuestCountry.ClientID %>").val(result.GuestCountryId);
            $("#txtGuestCountrySearch").val(result.CountryName);
            $("#<%=lblGstPreference.ClientID %>").text(result.GuestPreferences);
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);

            return false;
        }
        function OnLoadParentFromDetailObjectFailed(error) {
            toastr.error(error.get_message());
        }


        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#gvGustIngormation tbody tr").length;
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
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

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
            var url = "/HotelManagement/Reports/frmReportReservationBillInfo.aspx?GuestBillInfo=" + reservationId;
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
            popup(1, 'TouchKeypad', '', 900, 500);
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
            popup(1, 'DivRoomSelect', '', 300, 525);
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

            var RoomArray = RoomIdList.split(",");

            if (RoomArray.length > 0) {
                for (var i = 0; i < RoomArray.length; i++) {
                    var roomId = "#" + RoomArray[i].trim();
                    $(roomId).attr("checked", true);
                }
            }
            return false;
        }
        function OnLoadRoomInformationWithControlFailed(error) {
            toastr.error(error.get_message());
        }

        function ShowRoomNumberAndId() {
            popup(-1);
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
            popup(-1);
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
            $("#ContentPlaceHolder1_txtUnitPrice").val('');
            $("#ContentPlaceHolder1_txtRoomRate").val('');
            $("#ContentPlaceHolder1_txtDiscountAmount").val('');
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
            // alert('hi');
            $("#ltlGuestDetailGrid").html(result);
            $("#<%=EditId.ClientID %>").val("");
            SelectdPreferenceId = "";
            clearUserDetailsControl();
            return false;
        }
        function OnLoadDetailGridInformationFailed(error) {
            $("#<%=EditId.ClientID %>").val("");
            toastr.error(error.get_message());
        }
        function LoadDetailGridInformation() {
            //Guest Detail
            var txtGuestName = $("#<%=txtGuestName.ClientID %>").val();
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
            var txtGuestPhone = $("#<%=txtGuestPhone.ClientID %>").val();
            var ddlGuestSex = $("#<%=ddlGuestSex.ClientID %>").val();
            var txtGuestZipCode = $("#<%=txtGuestZipCode.ClientID %>").val();
            var txtNationalId = $("#<%=txtNationalId.ClientID %>").val();
            var txtPassportNumber = $("#<%=txtPassportNumber.ClientID %>").val();
            var txtPExpireDate = $("#<%=txtPExpireDate.ClientID %>").val();
            var txtPIssueDate = $("#<%=txtPIssueDate.ClientID %>").val();
            var txtPIssuePlace = $("#<%=txtPIssuePlace.ClientID %>").val();
            var txtVExpireDate = $("#<%=txtVExpireDate.ClientID %>").val();
            var txtVisaNumber = $("#<%=txtVisaNumber.ClientID %>").val();
            var txtVIssueDate = $("#<%=txtVIssueDate.ClientID %>").val();
            var txtNumberOfPersonAdult = $("#<%=txtNumberOfPersonAdult.ClientID %>").val();

            var txtGuestCountrySearch = $.trim($("#txtGuestCountrySearch").val());

            if (txtGuestEmail != "") {
                var mailformat = new RegExp('^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$', 'i');
                if (txtGuestEmail.match(mailformat)) {
                }
                else {
                    toastr.warning("You have entered an invalid email address!");
                    $("#txtGuestEmail>").focus();
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

            if (txtGuestName == "" || txtNumberOfPersonAdult == "") {
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

                PageMethods.SaveGuestInformationAsDetail(reservationId, isEdit, txtGuestName, txtGuestEmail, hiddenGuestId, txtGuestDrivinlgLicense, txtGuestDOB, txtGuestAddress1, txtGuestAddress2, ddlProfessionId, txtGuestCity, ddlGuestCountry, txtGuestNationality, txtGuestPhone, ddlGuestSex, txtGuestZipCode, txtNationalId, txtPassportNumber, txtPExpireDate, txtPIssueDate, txtPIssuePlace, txtVExpireDate, txtVisaNumber, txtVIssueDate, SelectdPreferenceId, OnLoadDetailGridInformationSucceeded, OnLoadDetailGridInformationFailed);
                return false;
            }
        }
        function clearUserDetailsControl() {
            $("#<%=txtGuestName.ClientID %>").val('');
            $("#<%=txtGuestEmail.ClientID %>").val('');
            $("#<%=hiddenGuestId.ClientID %>").val('0');
            $("#<%=txtGuestDrivinlgLicense.ClientID %>").val('');
            $("#<%=txtGuestDOB.ClientID %>").val('');
            $("#<%=txtGuestAddress1.ClientID %>").val('');
            $("#<%=txtGuestAddress2.ClientID %>").val('');
            $("#<%=ddlProfessionId.ClientID %>").val('1');
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
            $("#<%=txtPIssuePlace.ClientID %>").val('');
            $("#<%=txtVExpireDate.ClientID %>").val('');
            $("#<%=txtVisaNumber.ClientID %>").val('');
            $("#<%=txtVIssueDate.ClientID %>").val('');
            $("#<%=lblGstPreference.ClientID %>").text('');
            $("#GuestPreferenceDiv").hide();
            $("#txtGuestCountrySearch").val("");
        }
        function PerformEditActionForGuestDetail(GuestId) {
            $("#<%=EditId.ClientID %>").val(GuestId);
            $('#btnAddGuest').val('Save');
            PageMethods.PerformEditActionForGuestDetailByWM(GuestId, OnEditGuestInformationSucceeded, OnEditGuestInformationFailed);
            return false;
        }
        function OnEditGuestInformationSucceeded(result) {
            $("#GuestPreferenceDiv").show();
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
            $("#<%=txtPIssuePlace.ClientID %>").val(result.PIssuePlace);
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
            $("#<%=lblGstPreference.ClientID %>").text(result.GuestPreferences);
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);
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

        function ValidateGuestNumber() {
            var rowCount = $('#ReservedGuestInformation tbody tr').length;

            if (rowCount == 0) {
                var answer = confirm("No Guest Added, Do you want to continue the Reservation Process ?")
                if (!answer) {
                    return false;
                }
            }

            var reservationId = "0", reservationNumber = "", reservationDate = "", dateIn = "", dateOut = "", probableArrivalTime = "", confirmationDate = "",
                reservedCompany = "", guestId = "", contactAddress = "", contactPerson = "", contactNumber = "", mobileNumber = "", faxNumber = "", contactEmail = "",
                totalRoomNumber = "0", reservedMode = "", reservationType = "", reservationMode = "", pendingDeadline = "", isListedCompany = false, companyId = "",
                businessPromotionId = "", referenceId = "", paymentMode = "", payFor = "", currencyType = "", conversionRate = "", reason = "", remarks = "",
                numberOfPersonAdult = "", numberOfPersonChild = "", isFamilyOrCouple = "", airportPickUp = "", airportDrop = "", isAirportPickupDropExist = false,
                reservationTempId = "0", reservationDuration = "";

            var arrivalFlightName = "", arrivalFlightNumber = "", arrivalTime = "", departureFlightName = "", departureFlightNumber = "", departureTime = "";

            reservationId = $("#ContentPlaceHolder1_hfReservationId").val();
            reservationTempId = $("#ContentPlaceHolder1_hfReservationIdTemp").val();

            dateIn = $("#ContentPlaceHolder1_txtDateIn").val();
            dateOut = $("#ContentPlaceHolder1_txtDateOut").val();

            dateIn = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(dateIn, innBoarDateFormat);
            dateOut = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(dateOut, innBoarDateFormat);

            reservationDuration = $("#txtReservationDuration").val();
            probableArrivalTime = $("#ContentPlaceHolder1_txtProbableArrivalTime").val();
            reservedMode = $("#ContentPlaceHolder1_ddlReservedMode").val();
            reservationType = $("#ContentPlaceHolder1_ddlReservationType").val();
            contactAddress = $("#ContentPlaceHolder1_txtContactAddress").val();
            contactPerson = $("#ContentPlaceHolder1_txtContactPerson").val();
            contactNumber = $("#ContentPlaceHolder1_txtContactNumber").val();
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
            numberOfPersonChild = $("#ContentPlaceHolder1_txtNumberOfPersonChild").val() == "" ? "0" : $("#ContentPlaceHolder1_txtNumberOfPersonChild").val();
            remarks = $("#ContentPlaceHolder1_txtRemarks").val();

            airportPickUp = $("#ContentPlaceHolder1_ddlAirportPickUp").val();
            airportDrop = $("#ContentPlaceHolder1_ddlAirportDrop").val();

            if (dateIn == "") {
                toastr.warning("Please Provide Check In Date.");
                return false;
            }
            else if (dateOut == "") {
                toastr.warning("Please Provide Expected Check Out Date.");
                return false;
            }
            else if (reservationDuration == "") {
                toastr.warning("Please Provide Valid Number For Number of Nights.");
                return false;
            }
            else if (probableArrivalTime == "") {
                toastr.warning("Please Provide Probable Arrival Time.");
                return false;
            }
            else if (contactPerson == "" && contactNumber == "" && mobileNumber == "") {
                toastr.warning("Please Provide Contact Person/ Telephone Number/ mobile number.");
                return false;
            }

            if (reservedMode == "Company") {
                if ($("#ContentPlaceHolder1_chkIsLitedCompany").is(":checked")) {
                    if ($("#ContentPlaceHolder1_ddlCompanyName").val() == "0") {
                        toastr.warning("Please Select Company Name.");
                        return false;
                    }
                }
                else {
                    if ($("#ContentPlaceHolder1_txtReservedCompany").val() == "") {
                        toastr.warning("Please Provide Company Name.");
                        return false;
                    }
                }
            }

            var currency = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currency != "Local") {
                if ($("#ContentPlaceHolder1_ddlCurrency").val() == "0") {
                    toastr.warning("Please Select Currency Type.");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtConversionRate").val() == "" || $("#ContentPlaceHolder1_txtConversionRate").val() == "0") {
                    toastr.warning("Please Provide Conversion Rate.");
                    return false;
                }
            }

            if ($("#ReservationRoomGrid tbody tr").length == 0) {
                toastr.warning("Please add at least one room.");
                return false;
            }

            if (airportPickUp == "YES") {

                if ($("#ContentPlaceHolder1_txtArrivalFlightName").val() == "") {
                    toastr.warning("Please Provide Arrival Flight Name");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtArrivalFlightNumber").val() == "") {
                    toastr.warning("Please Provide Arrival Flight Number");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtArrivalTime").val() == "") {
                    toastr.warning("Please Provide Arrival Flight Time");
                    return false;
                }
            }

            if (airportDrop == "YES") {

                if ($("#ContentPlaceHolder1_txtDepartureFlightName").val() == "") {
                    toastr.warning("Please Provide Departure Flight Name");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtDepartureFlightNumber").val() == "") {
                    toastr.warning("Please Provide Departure Flight Number");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtDepartureTime").val() == "") {
                    toastr.warning("Please Provide Departure Flight Time");
                    return false;
                }
            }

            if (airportPickUp == "YES" || airportPickUp == "TBA") {
                isAirportPickupDropExist = true;
                arrivalFlightName = $("#ContentPlaceHolder1_txtArrivalFlightName").val();
                arrivalFlightNumber = $("#ContentPlaceHolder1_txtArrivalFlightNumber").val();
                arrivalTime = $("#ContentPlaceHolder1_txtArrivalTime").val();
            }
            else { isAirportPickupDropExist = false; }


            if (airportDrop == "YES" || airportDrop == "TBA") {
                isAirportPickupDropExist = true;
                departureFlightName = $("#ContentPlaceHolder1_txtDepartureFlightName").val();
                departureFlightNumber = $("#ContentPlaceHolder1_txtDepartureFlightNumber").val();
                departureTime = $("#ContentPlaceHolder1_txtDepartureTime").val()
            }
            else if (isAirportPickupDropExist == false) { isAirportPickupDropExist = false; }

            if (reservedMode == "Company") {

                if ($("#ContentPlaceHolder1_chkIsLitedCompany").is(":checked")) {
                    isListedCompany = true;
                    companyId = $("#ContentPlaceHolder1_ddlCompanyName").val();
                    reservedCompany = null;
                    paymentMode = $("#ContentPlaceHolder1_ddlPaymentMode").val();
                }
                else {
                    isListedCompany = false;
                    companyId = "0";
                    reservedCompany = $("#ContentPlaceHolder1_txtReservedCompany").val();
                    paymentMode = "Self";
                }
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

            if ($("#ContentPlaceHolder1_ddlReservationStatus").val() == "Active") {
                reservationMode = $("#ContentPlaceHolder1_ddlReservationStatus").val();
            }
            else if ($("#ContentPlaceHolder1_ddlReservationStatus").val() == "Pending") {
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

            var RoomReservation = {
                ReservationId: reservationId,
                ReservationTempId: reservationTempId,
                DateIn: dateIn,
                DateOut: dateOut,
                DateInStr: dateIn,
                DateInFieldEdit: dateInHiddenFieldEdit,
                MinCheckInDate: minCheckInDate,
                ProbableArrivalTime: probableArrivalTime,
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
                BusinessPromotionId: businessPromotionId,
                ReferenceId: referenceId,
                PaymentMode: paymentMode,
                PayFor: payFor,
                CurrencyType: currencyType,
                ConversionRate: conversionRate,
                Reason: reason,
                Remarks: remarks,
                NumberOfPersonAdult: numberOfPersonAdult,
                NumberOfPersonChild: numberOfPersonChild,
                IsFamilyOrCouple: isFamilyOrCouple,
                IsAirportPickupDropExist: (isAirportPickupDropExist == true ? 1 : 0),
                AirportPickUp: airportPickUp,
                AirportDrop: airportDrop
            };

            var AireportPickupDrop = {
                APDId: "0",
                ReservationId: "0",
                ArrivalFlightName: arrivalFlightName,
                ArrivalFlightNumber: arrivalFlightNumber,
                ArrivalTime: arrivalTime,
                DepartureFlightName: departureFlightName,
                DepartureFlightNumber: departureFlightNumber,
                DepartureTime: departureTime
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
                roomId = "0", discountAmount = "", roomRate = "", totalRoom = 0, i = 0, j = 0;
            var RoomReservationDetail = new Array();

            $("#ReservationRoomGrid tbody tr").each(function () {

                roomTypeId = $.trim($(this).find("td:eq(4)").text());
                reservationDetailIds = $.trim($(this).find("td:eq(3)").text());
                roomIds = $.trim($(this).find("td:eq(5)").text());
                discountType = $.trim($(this).find("td:eq(8)").text());
                discountAmount = $.trim($(this).find("td:eq(9)").text());
                unitPrice = $.trim($(this).find("td:eq(10)").text());
                roomRate = $.trim($(this).find("td:eq(11)").text());
                totalRoom = parseInt($.trim($(this).find("td:eq(7)").text()), 10);

                if (discountAmount == "")
                    discountAmount = "0";

                var roomIdList = roomIds.split(",");
                var reservationRoomIdList = reservationDetailIds.split(",");

                for (i = 0; i < roomIdList.length; i++) {

                    roomId = (roomIdList[i] == null ? "0" : roomIdList[i]);
                    reservationDetailId = (reservationRoomIdList[i] == null ? "0" : reservationRoomIdList[i]);

                    roomId = (roomId == "" ? "0" : roomId);
                    reservationDetailId = (reservationDetailId == "" ? "0" : reservationDetailId);

                    if (roomId == "0") {

                        for (j = 0; j < totalRoom; j++) {
                            RoomReservationDetail.push({
                                ReservationDetailId: ((reservationRoomIdList[j] == null || reservationRoomIdList[j] == "") ? "0" : reservationRoomIdList[j]),
                                ReservationId: reservationId,
                                RoomTypeId: roomTypeId,
                                RoomId: ((roomIdList[j] == null || roomIdList[j] == "") ? "0" : roomIdList[j]),
                                UnitPrice: unitPrice,
                                DiscountType: discountType,
                                DiscountAmount: discountAmount,
                                RoomRate: roomRate,
                                IsRegistered: false
                            });
                        }
                    }
                    else {
                        RoomReservationDetail.push({
                            ReservationDetailId: reservationDetailId,
                            ReservationId: reservationId,
                            RoomTypeId: roomTypeId,
                            RoomId: roomId,
                            UnitPrice: unitPrice,
                            DiscountType: discountType,
                            DiscountAmount: discountAmount,
                            RoomRate: roomRate,
                            IsRegistered: false
                        });
                    }
                }

                reservationDetailId = "0";

            });

            PageMethods.SaveReservation(RoomReservation, RoomReservationDetail, AireportPickupDrop, ComplementaryItem,
                                        PaidServiceDetails, paidServiceDeleted, OnSaveReservationSucceed, OnSaveReservationFailed);

            return false;
        }

        function OnSaveReservationSucceed(result) {
            if (result.IsSuccess) {
                $("#ContentPlaceHolder1_hfIsSaveRUpdate").val(result.Pk);

                if ($("#ContentPlaceHolder1_btnSave").val() == "Save") {
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
            $("#frmHotelManagement")[0].reset();
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
            PageMethods.PerformGetRoomAvailableChecking(RoomTypeId, txtFromDate, txtToDate, OnPerformRoomAvailableCheckingSucceeded, OnPerformRoomAvailableCheckingFailed);
            return false;
        }
        function OnPerformRoomAvailableCheckingSucceeded(result) {
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
            //window.location = "frmRoomReservation.aspx?pn=" + pageNumber + "&grc=" + ($("#ContentPlaceHolder1_gvRoomRegistration tbody tr").length + 1) + "&icp=" + isCurrentOrPreviousPage;

            var gridRecordCounts = ($("#ContentPlaceHolder1_gvRoomRegistration tbody tr").length + 1);
            $("#ContentPlaceHolder1_hfPageNumber").val(pageNumber);
            $("#ContentPlaceHolder1_hfGridRecordCounts").val(gridRecordCounts);
            $("#ContentPlaceHolder1_hfIsCurrentRPreviouss").val(isCurrentOrPreviousPage);
            $("#ContentPlaceHolder1_btnPagination").trigger("click");
        }
        function OnLoadXtraValidationSucceeded(result) {
            if (result.SetupValue == "1") {
                var country = $("#<%=ddlGuestCountry.ClientID %>").val();
                if (country == 19) {
                    var nationalId = $("#<%=txtNationalId.ClientID %>").val();
                    if (nationalId == "") {
                        toastr.warning("Please Provide National Id.");
                        return;
                    }
                }
                else {
                    var visaNumber = $("#<%=txtVisaNumber.ClientID %>").val();
                    var visaIssueDate = $("#<%=txtVIssueDate.ClientID %>").val();
                    var visaExpiryDate = $("#<%=txtVExpireDate.ClientID %>").val();
                    var passportNumber = $("#<%=txtPassportNumber.ClientID %>").val();
                    var passIssuePlace = $("#<%=txtPIssuePlace.ClientID %>").val();
                    var passIssueDate = $("#<%=txtPIssueDate.ClientID %>").val();
                    var passExpiryDate = $("#<%=txtPExpireDate.ClientID %>").val();

                    if (visaNumber == "") {
                        toastr.warning("Please Provide Visa Number.");
                        return;
                    }
                    else if (visaIssueDate == "") {
                        toastr.warning("Please Provide Visa Issue Date.");
                        return;
                    }
                    else if (visaExpiryDate == "") {
                        toastr.warning("Please Provide Visa Expiry Date.");
                        return;
                    }
                    else if (passportNumber == "") {
                        toastr.warning("Please Provide Passport Number.");
                        return;
                    }
                    else if (passIssuePlace == "") {
                        toastr.warning("Please Provide Passport Issue Place.");
                        return;
                    }
                    else if (passIssueDate == "") {
                        toastr.warning("Please Provide Passport Issue Date.");
                        return;
                    }
                    else if (passExpiryDate == "") {
                        toastr.warning("Please Provide Passport Expiry Date.");
                        return;
                    }
                }
                var dob = $("#<%=txtGuestDOB.ClientID %>").val();
                var address = $("#<%=txtGuestAddress2.ClientID %>").val();
                var email = $("#<%=txtGuestEmail.ClientID %>").val();
                var phoneNo = $("#<%=txtGuestPhone.ClientID %>").val();

                if (dob == "") {
                    toastr.warning("Please Provide Date of Birth.");
                    return;
                }
                else if (address == "") {
                    toastr.warning("Please Provide Address.");
                    return;
                }
                else if (email == "") {
                    toastr.warning("Please Provide Email Address.");
                    return;
                }
                else if (phoneNo == "") {
                    toastr.warning("Please Provide Phone No.");
                    return;
                }
            }
            LoadDetailGridInformation();
        }
        function OnLoadXtraValidationFailed(error) {
        }

        // Guest Reference
        function LoadGuestReference() {
            LoadGuestReferenceInfo();
            popup(1, 'DivGuestReference', '', 600, 525);
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

            var PreferenceArray = PreferenceIdList.split(",");

            if (PreferenceArray.length > 0) {
                for (var i = 0; i < PreferenceArray.length; i++) {
                    var preferenceId = "#" + PreferenceArray[i].trim();
                    $(preferenceId).attr("checked", true);
                }
            }

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
            $("#<%=lblGstPreference.ClientID %>").text(SelectdPreferenceName);
            popup(-1);
        }
    </script>
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfMinCheckInDate" runat="server" />
    <asp:HiddenField ID="hfIsRoomOverbookingEnable" runat="server" />
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
    <asp:HiddenField ID="hfGuestPreferenceId" runat="server" />
    <div style="height: 45px">
    </div>
    <div style="display: none;">
        <asp:Button ID="btnPagination" runat="server" Text="pagination" OnClick="btnPagination_Click" /></div>
    <div style="display: none;">
        <div runat="server" id="RoomDetailsTemp">
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
                    <input type="button" id="btnItemwiseSpecialRemarksOk" class="TransactionalButton btn btn-primary"
                        style="width: 80px;" value="Ok" />
                    <input type="button" id="btnItemwiseSpecialRemarksCancel" class="TransactionalButton btn btn-primary"
                        style="width: 80px;" value="Cancel" />
                </div>
                <div id="ItemWiseSpecialRemarksContainer" class="alert alert-info" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert">
                        ×</button>
                    <asp:Label ID='ItemWiseSpecialRemarksMessage' Font-Bold="True" runat="server" ClientIDMode="Static"></asp:Label>
                </div>
            </div>
            <div class="divClear">
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
                href="#tab-3">Airport Pick-Up/Drop</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-4">Complimentary Item</a></li>
            <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-5">Search Reservation</a></li>
        </ul>
        <div id="tab-1">
            <div class="HMBodyContainer">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblDateIn" runat="server" Text="Check In Date"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:HiddenField ID="DateInHiddenField" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="DateInHiddenFieldEdit" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtDateIn" CssClass="datepicker" runat="server" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="Label1" runat="server" Text="Probable Arrival Time"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtProbableArrivalTime" placeholder="12" runat="server" TabIndex="23"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblDateOut" runat="server" Text="Check Out Date"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:HiddenField ID="DateOutHiddenField" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtEditedRoom" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtEditedRoomNumber" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtDateOut" runat="server" CssClass="datepicker" TabIndex="5"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="Label6" runat="server" Text="Number of Nights"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtReservationDuration" runat="server" Width="130px" ReadOnly="true"
                            ClientIDMode="Static"></asp:TextBox>
                        <span>
                            <asp:CheckBox ID="cbReservationDuration" runat="server" ClientIDMode="Static" /></span>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblReservedMode" runat="server" Text="Reservation Mode"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlReservedMode" runat="server" TabIndex="7">
                            <asp:ListItem>Personal</asp:ListItem>
                            <asp:ListItem>Company</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblReservationType" runat="server" Text="Reserve Type"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:DropDownList ID="ddlReservationType" runat="server" TabIndex="6">
                            <asp:ListItem>Online</asp:ListItem>
                            <asp:ListItem>Telephone</asp:ListItem>
                            <asp:ListItem>Fax</asp:ListItem>
                            <asp:ListItem>Email</asp:ListItem>
                            <asp:ListItem>Direct</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div style="display: none;">
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblBusinessPromotionId" runat="server" Text="Business Promotion"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlBusinessPromotionId" runat="server" CssClass="tdLeftAlignWithSize"
                                TabIndex="8">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="CompanyInformation" style="display: none;">
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:CheckBox ID="chkIsLitedCompany" runat="server" Text="" onclick="javascript: return ToggleListedCompanyInfo();"
                                TabIndex="9" />
                            <asp:Label ID="lblCompany" runat="server" Text="Listed Company"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <div id="ListedCompany" style="display: none;">
                                <asp:DropDownList ID="ddlCompanyName" runat="server" TabIndex="10" CssClass="ThreeColumnDropDownList">
                                </asp:DropDownList>
                            </div>
                            <div id="ReservedCompany">
                                <asp:TextBox ID="txtReservedCompany" runat="server" TabIndex="11" CssClass="ThreeColumnTextBox"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblContactPerson" runat="server" Text="Contact Person"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="ThreeColumnTextBox" TabIndex="11"
                                MaxLength="150"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblContactAddress" runat="server" Text="Contact Address"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtContactAddress" runat="server" CssClass="ThreeColumnTextBox"
                                TextMode="MultiLine" TabIndex="11" MaxLength="300"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblContactNumber" runat="server" Text="Telephone Number"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtContactNumber" runat="server" TabIndex="12"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblContactEmail" runat="server" Text="Email Address"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtContactEmail" runat="server" TabIndex="13"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="Label3" runat="server" Text="Mobile Number"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtMobileNumber" runat="server" TabIndex="14"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="Label4" runat="server" Text="Fax Number"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtFaxNumber" runat="server" TabIndex="15"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="PaymentInformation" style="display: none;">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblPaymentMode" runat="server" Text="Payment Mode"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlPaymentMode" runat="server" TabIndex="16">
                                <asp:ListItem Value="Company">Company</asp:ListItem>
                                <asp:ListItem Value="Self">Self</asp:ListItem>
                                <asp:ListItem Value="TBA">Before C/O (Company/ Host)</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblPayFor" runat="server" Text="Pay For"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlPayFor" runat="server" TabIndex="17">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblCurrency" runat="server" Text="Currency Type"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:HiddenField ID="hfCurrencyHiddenField" runat="server" />
                        <asp:DropDownList ID="ddlCurrency" runat="server" TabIndex="16">
                        </asp:DropDownList>
                    </div>
                    <div id="CurrencyAmountInformationDiv" style="display: none;">
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblCurrencyAmount" runat="server" Text="Conversion Rate"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtConversionRate" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="childDivSection">
                    <div id="RoomDetailsInformation" class="block">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Room Detailed Information
                        </a>
                        <div class="HMBodyContainer">
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblRoomTypeId" runat="server" Text="Room Type"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlRoomTypeId" runat="server" TabIndex="16">
                                    </asp:DropDownList>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblUnitPrice" runat="server" Text="Rack Rate"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:HiddenField ID="txtUnitPriceHiddenField" runat="server"></asp:HiddenField>
                                    <asp:TextBox ID="txtUnitPrice" runat="server" TabIndex="17" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblDiscountType" runat="server" Text="Discount Type"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlDiscountType" runat="server" TabIndex="16">
                                        <asp:ListItem>Fixed</asp:ListItem>
                                        <asp:ListItem>Percentage</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblAmount" runat="server" Text="Discount Amount"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtDiscountAmount" runat="server" TabIndex="17"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblRoomRate" runat="server" Text="Negotiated Rate"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtRoomRate" runat="server" TabIndex="17" onblur="CalculateDiscount()"></asp:TextBox><span
                                        style="margin-left: 3px;">
                                        <%--<img src='../Images/service.png' title='Add Service' style="cursor: pointer;" onclick='javascript:return AddServiceCharge()'
                                            alt='Add Service' border='0' /></span>--%>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblNumberOfRoom" runat="server" Text="Room Quantity"></asp:Label>
                                    <span class="MandatoryField">*</span>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtRoomId" runat="server" CssClass="CustomTimeSize" TabIndex="17"></asp:TextBox>
                                    <input type="button" tabindex="18" id="btnChange" value="Room Number" class="TransactionalButton btn btn-primary"
                                        onclick="javascript:return LoadRoomNumber()" />
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection" id="DivAddedRoom" style="display: none">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblAddedRoom" runat="server" Text="Added Room"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:Label ID="lblAddedRoomNumber" CssClass="ThreeColumnTextBox" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <%--Right Left--%>
                                <input type="button" id="btnAddDetailGuest" tabindex="19" value="Add" class="TransactionalButton btn btn-primary" />
                                <input type="button" id="btnDiscard" onclick="CancelRoomAddEdit()" tabindex="19"
                                    value="Cancel" class="TransactionalButton btn btn-primary" />
                                <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                                <asp:HiddenField ID="ddlRoomTypeIdHiddenField" runat="server"></asp:HiddenField>
                            </div>
                            <div class="divClear">
                            </div>
                            <div id="ReservationDetailGrid">
                                <table style='width: 100%' cellspacing='0' cellpadding='4' id='ReservationRoomGrid'>
                                    <thead>
                                        <tr style='color: White; background-color: #44545E; font-weight: bold;'>
                                            <th align='left' style="width: 42%;">
                                                Room Type
                                            </th>
                                            <th align='left' style="width: 50%;">
                                                Room Numbers
                                            </th>
                                            <th align='center' style="width: 8%;">
                                                Action
                                            </th>
                                            <th style="display: none;">
                                                ReserVation Details Id
                                            </th>
                                            <th style="display: none;">
                                                Room Type Id
                                            </th>
                                            <th style="display: none;">
                                                Room Id
                                            </th>
                                            <th style="display: none;">
                                                Room Number
                                            </th>
                                            <th style="display: none;">
                                                Total Room
                                            </th>
                                            <th style="display: none;">
                                                Discount Type
                                            </th>
                                            <th style="display: none;">
                                                Discount Amount
                                            </th>
                                            <th style="display: none;">
                                                Unit Price Rack Rate
                                            </th>
                                            <th style="display: none;">
                                                Room Rate Negotiated Rate
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
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="Label5" runat="server" Text="Reference"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlReferenceId" runat="server" TabIndex="20">
                        </asp:DropDownList>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblReservationMode" runat="server" Text="Reservation Status"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:DropDownList ID="ddlReservationStatus" runat="server" CssClass="customMediupXLDropDownSize"
                            TabIndex="21">
                            <asp:ListItem Value="Active">Active</asp:ListItem>
                            <%--<asp:ListItem Value="Pending">Pending</asp:ListItem>--%>
                            <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection" id="NoShowChargeDiv" style="display: none;">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblNoShowCharge" runat="server" Text="Charge Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtNoShowCharge" CssClass="datepicker" runat="server" TabIndex="22"></asp:TextBox>
                        <div style="display: none;">
                            <asp:DropDownList ID="ddlCashPaymentAccountHeadForNoShow" runat="server" CssClass="customMediupXLDropDownSize"
                                TabIndex="21">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection" id="PendingDiv">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblPendingDeadlineDate" runat="server" Text="Confirmation Date"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtConfirmationDate" CssClass="datepicker" runat="server" TabIndex="22"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblPendingDeadlineTime" runat="server" Text="Probable Arrival Time"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtProbableArrivalPendingTime" placeholder="12" runat="server" TabIndex="23"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection" id="ReasonDiv">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblReason" runat="server" Text="Cancel Reason "></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtReason" runat="server" CssClass="ThreeColumnTextBox" TextMode="MultiLine"
                            TabIndex="26"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="ThreeColumnTextBox" TextMode="MultiLine"
                            TabIndex="27"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="divClear">
        </div>
        <div id="tab-2">
            <div class="childDivSection">
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <input id="EditId" runat="server" type="hidden" />
                            <asp:Label ID="lblNumberOfPersonAdult" runat="server" Text="Person(Adult)"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtNumberOfPersonAdult" runat="server" Width="110px" CssClass="customMediupXLTextBoxSize"
                                TabIndex="28">1</asp:TextBox>
                            <asp:CheckBox ID="cbFamilyOrCouple" runat="server" Text="" TabIndex="9" />
                            <asp:Label ID="Label16" runat="server" Text="Family/ Couple"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblNumberOfPersonChild" runat="server" Text="Person(Child)"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtNumberOfPersonChild" runat="server" CssClass="customMediupXLTextBoxSize"
                                TabIndex="29"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblGuestName" runat="server" Text="Name"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtGuestName" CssClass="TwoColumnTextBox" runat="server" TabIndex="30"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <button type="button" id="btnAddPerson" tabindex="31" class="TransactionalButton btn btn-primary">
                                Search Guest</button>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblGuestDOB" runat="server" Text="Date Of Birth"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtGuestDOB" runat="server" CssClass="datepicker" TabIndex="32"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblGuestSex" runat="server" Text="Gender"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlGuestSex" runat="server" TabIndex="33" Style="align-items:center">
                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem>Male</asp:ListItem>
                                <asp:ListItem>Female</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblGuestAddress1" runat="server" Text="Company Name"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtGuestAddress1" runat="server" CssClass="ThreeColumnTextBox" TabIndex="34"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblGuestAddress2" runat="server" Text="Address"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtGuestAddress2" runat="server" CssClass="ThreeColumnTextBox" TabIndex="35"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblGuestEmail" runat="server" Text="Email Address"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtGuestEmail" runat="server" TabIndex="36" CssClass="ThreeColumnTextBox"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblProfession" runat="server" Text="Profession"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlProfessionId" runat="server" TabIndex="40" CssClass="ThreeColumnDropDownList">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblGuestPhone" runat="server" Text="Phone Number"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtGuestPhone" runat="server" TabIndex="37"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblGuestCity" runat="server" Text="City"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtGuestCity" runat="server" TabIndex="38"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblGuestZipCode" runat="server" Text="ZipCode"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtGuestZipCode" runat="server" TabIndex="39"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblGuestCountry" runat="server" Text="Country"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <input id="txtGuestCountrySearch" type="text" class="customMediumTextBoxSize" />
                            <div style="display: none;">
                                <asp:DropDownList ID="ddlGuestCountry" runat="server" TabIndex="40">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblGuestNationality" runat="server" Text="Nationality"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtGuestNationality" runat="server" TabIndex="41"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblGuestDrivinlgLicense" runat="server" Text="Driving License"></asp:Label>
                            <%--<asp:Label ID="lblGuestAuthentication" runat="server" Text="Authentication"></asp:Label>--%>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtGuestDrivinlgLicense" runat="server" TabIndex="42"></asp:TextBox>
                            <%--<asp:DropDownList ID="ddlGuestAuthentication" runat="server" TabIndex="28">
                                        </asp:DropDownList>--%>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblNationalId" runat="server" Text="National ID"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtNationalId" runat="server" TabIndex="43"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblVisaNumber" runat="server" Text="Visa Number"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtVisaNumber" runat="server" TabIndex="44"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblVIssueDate" runat="server" Text="Visa Issue Date"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtVIssueDate" CssClass="datepicker" runat="server" TabIndex="45"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblVExpireDate" runat="server" Text="Visa Expiry Date"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtVExpireDate" CssClass="datepicker" runat="server" TabIndex="46"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblPassportNumber" runat="server" Text="Passport Number"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtPassportNumber" runat="server" TabIndex="47"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblPIssuePlace" runat="server" Text="Pass. Issue Place"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtPIssuePlace" runat="server" TabIndex="48"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblPIssueDate" runat="server" Text="Pass. Issue Date"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtPIssueDate" runat="server" CssClass="datepicker" TabIndex="49"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblPExpireDate" runat="server" Text="Pass. Expiry Date"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtPExpireDate" runat="server" CssClass="datepicker" TabIndex="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection" id="GuestPreferenceDiv" style="display: none">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblGstPreferences" runat="server" Text="Guest Preferences"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <%--<asp:TextBox ID="txtGstPreferences" runat="server" CssClass="ThreeColumnTextBox"
                                TextMode="MultiLine" TabIndex="7" ReadOnly></asp:TextBox>--%>
                            <asp:Label ID="lblGstPreference" runat="server" Width="648"></asp:Label>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionRightRight" style="text-align: right; margin-right: 38px;">
                            <input type="button" tabindex="18" id="btnGuestReferences" value="Preferences" class="TransactionalButton btn btn-primary"
                                onclick="javascript:return LoadGuestReference()" />
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <%--Right Left--%>
                        <input id="btnAddGuest" type="button" value="Add" class="TransactionalButton btn btn-primary"
                            tabindex="51" />
                        <asp:Button ID="btnCancelDetailGuest" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary"
                            TabIndex="52" />
                        <asp:Label ID="Label2" runat="server" Text='' Visible="False"></asp:Label>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div style="float: right; padding-right: 30px">
                            <input id="btnNext2" type="button" tabindex="54" value="Next" class="TransactionalButton btn btn-primary"
                                style="display: none;" />
                        </div>
                        <div style="float: right; padding-right: 10px">
                            <input id="btnPrev1" type="button" tabindex="53" value="Prev" class="TransactionalButton btn btn-primary"
                                style="display: none;" />
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div id="ltlGuestDetailGrid">
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
        <div class="divClear">
        </div>
        <div id="tab-3">
            <div class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Arrival Information
                </a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblAirportPickUp" runat="server" Text="Airport Pick-Up"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlAirportPickUp" runat="server">
                                <asp:ListItem>NO</asp:ListItem>
                                <asp:ListItem>YES</asp:ListItem>
                                <asp:ListItem>TBA</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="AirportPickUpInformationDiv">
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblArrivalFlightName" runat="server" Text="Airline Name"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtArrivalFlightName" runat="server" CssClass="ThreeColumnTextBox"
                                    TabIndex="54"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblArrivalFlightNumber" runat="server" Text="Flight Number"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtArrivalFlightNumber" runat="server" CssClass="ThreeColumnTextBox"
                                    TabIndex="55"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblArrivalTime" runat="server" Text="Arrival Time (ETA)"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtArrivalTime" placeholder="12" runat="server" TabIndex="56"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Departure Information
                </a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblAirportDrop" runat="server" Text="Airport Drop"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlAirportDrop" runat="server">
                                <asp:ListItem>NO</asp:ListItem>
                                <asp:ListItem>YES</asp:ListItem>
                                <asp:ListItem>TBA</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="AirportDropInformationDiv">
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblDepartureFlightName" runat="server" Text="Airline Name"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtDepartureFlightName" runat="server" CssClass="ThreeColumnTextBox"
                                    TabIndex="59"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblDepartureFlightNumber" runat="server" Text="Flight Number"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtDepartureFlightNumber" runat="server" CssClass="ThreeColumnTextBox"
                                    TabIndex="60"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblDepartureTime" runat="server" Text="Departure Time (ETD)"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtDepartureTime" placeholder="12" runat="server" TabIndex="61"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </div>
            </div>
        </div>
        <div class="divClear">
        </div>
        <div id="tab-4">
            <asp:CheckBoxList ID="chkComplementaryItem" runat="server" CssClass="customChkBox">
            </asp:CheckBoxList>
        </div>
        <div class="divClear">
        </div>
        <div id="tab-5">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker" TabIndex="63"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtToDate" runat="server" TabIndex="64" CssClass="datepicker"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblSrcReservationGuest" runat="server" Text="Guest Name"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtSrcReservationGuest" runat="server" TabIndex="65"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblSearchReservationNumber" runat="server" Text="Reservation Number"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtSearchReservationNumber" runat="server" TabIndex="66"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblCmpName" runat="server" Text="Company Name"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtSearchCompanyName" runat="server" TabIndex="66"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblCntPerson" runat="server" Text="Contact Person"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtCntPerson" runat="server" TabIndex="67"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblOrderCriteris" runat="server" Text="Search Ordering"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlOrderCriteria" runat="server" Width="140px">
                        <asp:ListItem Value="ReservationNo">Reservation No</asp:ListItem>
                        <asp:ListItem Value="CheckInDate">Check In Date</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlorderOption" runat="server" Width="76px">
                        <asp:ListItem Value="DESC">DESC</asp:ListItem>
                        <asp:ListItem Value="ASC">ASC</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <%--<button type="button" id="btnSrchRsrvtn" class="TransactionalButton btn btn-primary">
                    Search</button>--%>
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                    TabIndex="5" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClearSearch" runat="server" TabIndex="68" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                    OnClientClick="javascript: return PerformClearSearchAction();" />
            </div>
            <div class="divClear">
            </div>
            <div id="SearchPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
                </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvRoomRegistration" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="20" OnRowCommand="gvRoomRegistration_RowCommand"
                        OnRowDataBound="gvRoomRegistration_RowDataBound">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("ReservationId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ReservationNumber" HeaderText="Reserv. No." ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GuestName" HeaderText="Guest Name" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CompanyName" HeaderText="Company" ItemStyle-Width="14%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RoomInformation" HeaderText="Room Info" ItemStyle-Width="14%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <%--<asp:TemplateField HeaderText="Reservation" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvReservationDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ReservationDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Check In" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvDateIn" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("DateIn"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Check Out" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvDateOut" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("DateOut"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ReservationMode" HeaderText="Status" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="ReservationStatusInfo" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblReservationStatusInfo" runat="server" Text='<%#Eval("ReservationMode") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("ReservationId") %>'
                                        CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                        ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("ReservationId") %>'
                                        CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to save?');"
                                        Text="" AlternateText="Delete" ToolTip="Delete" />
                                    &nbsp;<asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False"
                                        CommandArgument='<%# bind("ReservationId") %>' CommandName="CmdBillPreview" ImageUrl="~/Images/ReportDocument.png"
                                        Text="" AlternateText="Bill Preview" ToolTip="Bill Preview" />
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
                    <div class="divClear">
                    </div>
                    <div class="childDivSection">
                        <div class="pagination pagination-centered" id="Div2">
                            <ul>
                                <asp:Literal ID="gridPaging" runat="server"></asp:Literal>
                            </ul>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <%-- <table cellspacing='0' cellpadding='4' id='tblRoomReserve' width="100%">
                        <colgroup>
                            <col style="width: 15%;" />
                            <col style="width: 25%;" />
                            <col style="width: 14%;" />
                            <col style="width: 14%;" />
                            <col style="width: 8%;" />
                            <col style="width: 9%;" />
                            <col style="width: 5%;" />
                            <col style="width: 5%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    Reserv. No.
                                </td>
                                <td>
                                    Guest Name
                                </td>
                                <td>
                                    Company
                                </td>
                                <td>
                                    Room Info
                                </td>
                                <td>
                                    Check In
                                </td>
                                <td>
                                    Check Out
                                </td>
                                <td>
                                    Status
                                </td>
                                <td style="text-align: right;">
                                    Action
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="divClear">
                    </div>
                    <div class="childDivSection">
                        <div class="pagination pagination-centered" id="GridPagingContainerForSearchReservation">
                            <ul>
                            </ul>
                        </div>
                    </div>--%>
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
    <div class="HMContainerRowButton" id="SubmitButtonDiv">
        <%--Right Left--%>
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
            TabIndex="80" OnClientClick="javascript: return ValidateGuestNumber();" />
        <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
            TabIndex="81" OnClick="btnCancel_Click" />
    </div>
    <!--    Pop Up Guest Search   -->
    <div id="TouchKeypad" style="display: none;">
        <div id="PopMessageBox" class="alert alert-info" style="display: none;">
            <button type="button" class="close" data-dismiss="alert">
                ×</button>
            <asp:Label ID='lblPopMessageBox' Font-Bold="True" runat="server"></asp:Label>
        </div>
        <div id="PopEntryPanel" class="block" style="width: 875px">
            <a href="#page-stats" class="block-heading" data-toggle="collapse">Guest Information
            </a>
            <div class="divClear">
            </div>
            <div class="HMBodyContainer">
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:HiddenField ID="hiddenGuestId" runat="server" />
                        <asp:HiddenField ID="hiddenGuestName" runat="server" />
                        <asp:Label ID="lblSrcGuestName" runat="server" Text="Guest Name"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtSrcGuestName" runat="server" CssClass="ThreeColumnTextBox" TabIndex="1"></asp:TextBox>
                    </div>
                    <div style="float: right; margin-left: 150px">
                        <img id="imgCollapse" width="40px" src="/HotelManagement/Image/expand_alt.png" />
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="ExtraSearch">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblCompanyName" runat="server" Text="Company Name"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSrcCompanyName" runat="server" CssClass="ThreeColumnTextBox"
                                TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSrcRoomNumber" runat="server" Text="Room No."></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="customMediumTextBoxSize"
                                TabIndex="3"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblSrcRegistrationNumber" runat="server" Text="Registration No."></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtSrcRegistrationNumber" runat="server" CssClass="customMediumTextBoxSize"
                                TabIndex="4"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSrcFromDate" runat="server" Text="From Date"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSrcFromDate" runat="server" CssClass="datepicker" TabIndex="3"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblSrcToDate" runat="server" Text="To Date"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtSrcToDate" runat="server" CssClass="datepicker" TabIndex="4"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSrcEmailAddress" runat="server" Text="Email Address"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSrcEmailAddress" runat="server" CssClass="customMediumTextBoxSize"
                                TabIndex="3"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblSrcMobileNumber" runat="server" Text="Mobile Number"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtSrcMobileNumber" runat="server" CssClass="customMediumTextBoxSize"
                                TabIndex="4"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSrcNationalId" runat="server" Text="National ID"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSrcNationalId" runat="server" CssClass="customMediumTextBoxSize"
                                TabIndex="3"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblSrcDateOfBirth" runat="server" Text="Date of Birth"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtSrcDateOfBirth" runat="server" CssClass="customMediumTextBoxSize"
                                TabIndex="4"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSrcPassportNumber" runat="server" Text="Passport Number"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSrcPassportNumber" runat="server" CssClass="ThreeColumnTextBox"
                                TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </div>
                <div class="HMContainerRowButton">
                    <button type="button" id="btnPopSearch" class="TransactionalButton btn btn-primary">
                        Search</button>
                </div>
            </div>
        </div>
        <div class="divClear">
        </div>
        <div id="PopSearchPanel" class="block" style="width: 875px">
            <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
            </a>
            <div class="block-body collapse in">
                <table cellspacing='0' cellpadding='4' id='gvGustIngormation' width="100%">
                    <colgroup>
                        <col style="width: 40%;" />
                        <col style="width: 25%;" />
                        <col style="width: 15%;" />
                        <col style="width: 20%;" />
                    </colgroup>
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <td>
                                Guest Name
                            </td>
                            <td>
                                Country Name
                            </td>
                            <td>
                                Phone
                            </td>
                            <td>
                                Email
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div class="divClear">
                </div>
                <div class="childDivSection">
                    <div class="pagination pagination-centered" id="GridPagingContainer">
                        <ul>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div class="divClear">
        </div>
        <div style="height: 45px">
        </div>
        <div class="divClear">
        </div>
        <div id="PopTabPanel" style="width: 900px">
            <div id="PopMyTabs">
                <ul id="PoptabPage" class="ui-style">
                    <li id="PopA" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-1">Guest information</a></li>
                    <li id="PopB" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-2">Guest Documents</a></li>
                    <li id="PopC" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-3">Guest History</a></li>
                    <li id="PopD" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-4">Guest Preferences</a></li>
                </ul>
                <div id="Poptab-1">
                    <div id="GuestInfo" class="block" style="font-weight: bold">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Guest Information
                        </a>
                        <div class="HMBodyContainer">
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
                            <div class="divClear">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="Poptab-2">
                    <div class="divClear">
                    </div>
                    <div id="imageDiv">
                    </div>
                    <div class="divClear">
                    </div>
                </div>
                <div id="Poptab-3">
                    <div class="HMBodyContainer">
                        <div id="guestHistoryDiv">
                        </div>
                    </div>
                </div>
                <div id="Poptab-4">
                    <div id="Div3" class="block" style="font-weight: bold">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Preferences </a>
                        <div id="Preference">
                        </div>
                    </div>
                </div>
                <button type="button" id="btnSearchSuccess" class="TransactionalButton btn btn-primary">
                    OK</button>
                <button type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary">
                    Cancel</button>
                <button type="button" id="btnPrintDocument" class="TransactionalButton btn btn-primary">
                    Print</button>
            </div>
        </div>
        <div class='divClear'>
        </div>
    </div>
    <!--  End Pop Up Guest Search  -->
    <!--Room Load PopUp -->
    <div id="DivRoomSelect" style="display: none;">
        <div id="Div1" class="block">
            <asp:HiddenField ID="hfSelectedRoomNumbers" runat="server" />
            <asp:HiddenField ID="hfSelectedRoomId" runat="server" />
            <asp:HiddenField ID="hfSelectedRoomReservedId" runat="server" />
            <div id="ltlRoomNumberInfo">
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <!-- End Room Load PopUp -->
    <!--Guest Reference PopUp -->
    <div id="DivGuestReference" style="display: none;">
        <div id="Div5" class="block">
            <div id="ltlGuestReference">
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <!--End Guest Reference PopUp -->
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
    </script>
</asp:Content>
