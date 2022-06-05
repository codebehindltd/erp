<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmHoldReCheckIn.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmHoldReCheckIn"
    EnableEventValidation="false" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var vv = [];
        var docv = [];
        var docc = [];
        var SelectdPreferenceId = "";

        var optionData = [];
        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $(function () {
                $("#myTabs").tabs();
            });

            $('#ContentPlaceHolder1_txtDepartureHour').timepicker({
                showPeriod: is12HourFormat
            });

            GetTempRegistrationDetailByWM();
            TotalRoomRateVatServiceChargeCalculation();

            if ($("#ContentPlaceHolder1_hfRegistrationId").val() != "") {
                $("#hfPaidServiceDialogDisplayOrNot").val("0");
                AddServiceCharge();
            }

            var qsReservationId = $("#<%=QSReservationId.ClientID %>").val();

            //CurrencyRateInfoEnable();

            // //-- Airport Pick Up Information Div ------------------
            if ($("#<%=ddlAirportPickUp.ClientID %>").val() == "NO") {
                $('#AirportPickUpInformationDiv').hide();
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

            var txtDiscountAmount = '<%=txtDiscountAmount.ClientID%>'
            $('#' + txtDiscountAmount).blur(function () {
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

            if (qsReservationId != 0) {
                $('#divReservationGuest').show();

                //SetRelatedDataByReservationId(qsReservationId);
                LoadGridViewByWebMethod(qsReservationId);
                LoadComplementaryItemByWM(qsReservationId);
                //PopulateGuestList();
                //PopulatePaidServiceFormReservation(qsReservationId);

                $('#divReservationGuest').show();
                var roomType = $('#<%=ddlRoomType.ClientID%>').val();
                SetRegistrationInfoByRoomTypeId(roomType, "");
            }
            else {
                $('#divReservationGuest').hide();
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
                PopulateRooms();
                PerformFillFormActionByTypeId($('#<%=ddlRoomType.ClientID%>').val());
                UpdateTotalCostWithDiscount();
            });

            function PerformFillFormActionByTypeId(actionId) {

                PageMethods.PerformFillFormActionByTypeId(actionId, OnFillFormObjectSucceededByTypeId, OnFillFormObjectFailedByTypeId);
                return false;
            }
            function OnFillFormObjectSucceededByTypeId(result) {

                $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRate);
                $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRate);
                $("#<%=txtRoomRate.ClientID %>").val(result.RoomRate.toFixed(2));

                //                vv = result;
                //                if ($("#<%=hfCurrencyType.ClientID %>").val() != 'Local') {
                //                    $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRateUSD);
                //                    $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRateUSD);
                //                    $("#<%=txtRoomRate.ClientID %>").val(result.RoomRateUSD.toFixed(2));
                //                }
                //                else {
                //                    
                //                }
                UpdateTotalCostWithDiscount();
                TotalRoomRateVatServiceChargeCalculation();
                return false;
            }
            function OnFillFormObjectFailedByTypeId(error) {
                toastr.error(error.get_message());
            }

            $("#btnAddDetailGuest").click(function () {
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

            var ddlRoomId = '<%=ddlRoomId.ClientID%>'
            var roomId = $('#' + ddlRoomId).val();

            $("#<%=txtRoomRate.ClientID %>").blur(function () {
                TotalRoomRateVatServiceChargeCalculation();
            });

            $("#<%=ddlDiscountType.ClientID %>").change(function () {
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
            var txtReCheckInDate = '<%=txtReCheckInDate.ClientID%>'

            $('#ContentPlaceHolder1_txtCheckInDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat                
            });
            $('#ContentPlaceHolder1_txtDisplayCheckInDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat                
            });

            $('#ContentPlaceHolder1_txtReCheckInDate').datepicker({
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
                minDate: $("#ContentPlaceHolder1_txtDisplayCheckInDate").val(),
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtReCheckInDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            var txtGuestDOB = '<%=txtGuestDOB.ClientID%>'
            $('#ContentPlaceHolder1_txtGuestDOB').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                yearRange: "-100:+0"
            });
            $('#ContentPlaceHolder1_txtSrcDateOfBirth').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                yearRange: "-100:+0"
            });
            $('#ContentPlaceHolder1_txtExpiryDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
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
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtPIssueDate').datepicker("option", "maxDate", selectedDate);
                }
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
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtVIssueDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtSrcFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSrcToDate').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtSrcToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSrcFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Registration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var txtEntiteledRoomType = '<%=txtEntiteledRoomType.ClientID%>'

            function PerformClearRelatedFields() {
                $("#<%=QSReservationId.ClientID %>").val(0);
                $('#divReservationGuest').hide();

                return false;
            }

            function OnCountrySucceeded(result) {
                $("#ContentPlaceHolder1_txtGuestNationality").val(result);
            }
            function OnCountryFailed() { }

            //            function PopulateGuestList() {

            //                var idReservation = -1;               
            //                var QSReservationId = $('#<%=QSReservationId.ClientID%>').val();
            //                if (QSReservationId != "") {
            //                    idReservation = parseInt(QSReservationId);
            //                }
            //                else {
            //                    idReservation = parseInt(ReservationId);
            //                }


            //                $("#<%=ddlReservationGuest.ClientID%>").attr("disabled", "disabled");

            //                if (idReservation == 0) {

            //                    $('#<%=ddlReservationGuest.ClientID %>').empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            //                }
            //                else {

            //                    $('#<%=ddlReservationGuest.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
            //                    $.ajax({
            //                        type: "POST",
            //                        url: "/HotelManagement/frmRoomRegistration.aspx/PopulateGuest",
            //                        data: '{ReservationId: ' + idReservation + '}',
            //                        contentType: "application/json; charset=utf-8",
            //                        dataType: "json",
            //                        success: OnGuestPopulated,
            //                        failure: function (response) {
            //                            toastr.info(response.d);
            //                        }
            //                    });
            //                }
            //            }
            //            function OnGuestPopulated(response) {
            //                var Data = response.d;
            //                var length = Data.length;
            //                if (length > 0) {
            //                    $('#divReservationGuest').show();
            //                    var ddlReservationGuest = '<%=ddlReservationGuest.ClientID%>'
            //                    $("#" + ddlReservationGuest).attr("disabled", false);
            //                    PopulateControl(response.d, $("#<%=ddlReservationGuest.ClientID %>"), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
            //                }
            //                else {
            //                    $('#divReservationGuest').hide();
            //                }

            //                var QSReservationId = $('#<%=QSReservationId.ClientID%>').val();                
            //            }

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
        });

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

        //        function OnGetPaidServiceReservationSucceed(result) {
        //            if (result.RegistrationPaidService.length > 0) {
        //                $("#<%=hfPaidServiceSaveObj.ClientID %>").val(JSON.stringify(result.RegistrationPaidService));
        //            }
        //        }
        //        function OnGetPaidReservationServiceFailed(error) {

        //        }

        //Complementary Item
        //        function LoadComplementaryItemByWM(reservationId) {
        //            PageMethods.LoadComplementaryItemByWM(reservationId, LoadComplementaryItemByWMSucceeded, LoadComplementaryItemByWMFailed);
        //            return false;
        //        }
        //        function LoadComplementaryItemByWMSucceeded(result) {
        //            var Item = "";
        //            $.each(result, function () {
        //                SetChecked(this['Text']);
        //                Item = Item + this['Text'];
        //            });

        //        }

        function SetChecked(id) {
            $('#ContentPlaceHolder1_chkComplementaryItem :checkbox').each(function () {
                var chkBoxId = $(this).attr('id');
                var chkBoxValue = $(this).val();
                if (id == chkBoxValue) {
                    $('#' + chkBoxId).attr("checked", true);
                }

            });
        }

        //        function LoadComplementaryItemByWMFailed(error) {
        //            toastr.error(error.get_message());
        //        }


        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewReservation').hide();
            $('#EntryPanel').show();
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewReservation').show();
            $('#EntryPanel').hide();
            //PerformClearAction();
            return false;
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

        function ToggleAdvancePaymentFieldVisible(ctrl) {
            if ($(ctrl).is(':checked')) {
                EntryPanelVisibleTrue();
            }
            else {
                EntryPanelVisibleFalse();
            }
        }

        //        function GetTotalCostWithCompanyOrPersonalDiscountWithOutMessege() {                                 
        //            if (companyId <= 0) { companyId = 0; }            
        //            if (companyId != 0) {
        //                PageMethods.GetCalculatedDiscount(companyId, 0, GetCalculatedDiscountWithOutMessegeObjectSucceeded, GetCalculatedDiscountObjectFailed);
        //                return false;
        //            }
        //            else {
        //                return true;
        //            }
        //        }

        function GetCalculatedDiscountWithOutMessegeObjectSucceeded(result) {
            $("#<%=txtContactNumber.ClientID %>").val(result.ContactNumber)
            $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson)
            var btnSaveValue = $("#<%=btnSave.ClientID %>").val();

            return false;
        }
        function GetCalculatedDiscountObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //        function GetTotalCostWithCompanyOrPersonalDiscount() {                                   
        //            if (companyId <= 0) { companyId = 0; }            
        //            if (companyId != 0) {
        //                PageMethods.GetCalculatedDiscount(companyId, 0, GetCalculatedDiscountObjectSucceeded, GetCalculatedDiscountObjectFailed);
        //                return false;
        //            }
        //            else {
        //                return true;
        //            }
        //        }
        var vdf = [];
        function GetCalculatedDiscountObjectSucceeded(result) {

            vdf = result;
            var btnSaveValue = $("#<%=btnSave.ClientID %>").val();

            //            if (company != "0") {
            //                $("#<%=txtContactNumber.ClientID %>").val(result.ContactNumber)
            //                $("#<%=txtContactPerson.ClientID %>").val(result.ContactPerson)
            //            }

            //if (company != "0") {
            //                if (btnSaveValue != "Save") {
            var answer = confirm("Do you want to recalculate Room Rent ?")
            if (answer) {

                $("#<%=ddlDiscountType.ClientID %>").val('Percentage');
                $("#<%=txtDiscountAmount.ClientID %>").val(result.DiscountPercent);

                //                var prevDiscount = parseFloat($("#<%=txtDiscountAmount.ClientID %>").val());
                //                if (isNaN(prevDiscount)) {
                //                    prevDiscount = 0;
                //                }
                //                $("#<%=ddlDiscountType.ClientID %>").val('Percentage');
                //                var resultFloat = parseFloat(result.DiscountPercent);
                //                if (resultFloat > prevDiscount) {
                //                    $("#<%=txtDiscountAmount.ClientID %>").val(resultFloat);
                //                }
                //                else {
                //                    $("#<%=txtDiscountAmount.ClientID %>").val(prevDiscount);
                //                }
                //UpdateDiscountAmount();
                UpdateTotalCostWithDiscount();
                //}
                //}
            }
            return false;
        }
        function GetCalculatedDiscountObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //        function UpdateTotalCostWithDiscount() {

        //            var discountAmount = 0, txtUnitPrice = 0, txtUnitPriceHiddenField = 0;

        //            discountType = $("#<%=ddlDiscountType.ClientID %>").val();

        //            if ($("#<%=txtDiscountAmount.ClientID %>").val() != "")
        //                discountAmount = parseFloat($("#<%=txtDiscountAmount.ClientID %>").val());

        //            if ($("#<%=txtUnitPrice.ClientID %>").val() != "")
        //                txtUnitPrice = parseFloat($("#<%=txtUnitPrice.ClientID %>").val());

        //            if ($("#<%=txtUnitPriceHiddenField.ClientID %>").val() != "")
        //                txtUnitPriceHiddenField = parseFloat($("#<%=txtUnitPriceHiddenField.ClientID %>").val());

        //            if (discountAmount != 0) {
        //                if (discountType == 'Fixed') {
        //                    var FinalAmount = txtUnitPriceHiddenField - discountAmount;
        //                    $("#<%=txtRoomRate.ClientID %>").val(FinalAmount.toFixed(2));
        //                }
        //                else {
        //                    if (txtUnitPriceHiddenField != 0) {
        //                        var percentage = parseFloat(txtUnitPriceHiddenField) * parseFloat(discountAmount) / 100;
        //                        var FinalAmount = parseFloat(txtUnitPriceHiddenField) - percentage;
        //                        $("#<%=txtRoomRate.ClientID %>").val(FinalAmount.toFixed(2));
        //                    }
        //                    else {
        //                        $("#<%=txtRoomRate.ClientID %>").val('0.00');
        //                    }
        //                }
        //            }
        //        }


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

        //Aireport Pickup Div Visible True/False-------------------
        function AireportPickupInformationPanelVisibleTrue() {
            $('#AireportArrivalInformation').show("slow");
        }
        function AireportPickupInformationPanelVisibleFalse() {
            $('#AireportArrivalInformation').hide("slow");
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            //    $('#AccountsPostingPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            //  $('#AccountsPostingPanel').hide("slow");
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
            $("#<%=txtRoomRate.ClientID %>").val(result.RoomRate.toFixed(2));

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
            $('#ContentPlaceHolder1_txtRoomRate').val(roomRate.toFixed(2));
            UpdateTotalCostWithDiscount();
            TotalRoomRateVatServiceChargeCalculation();
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
                LoadGridInformation();
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

                //popup(-1);
                $("#TouchKeypad").dialog("close");
            });
            $("#btnSearchCancel").click(function () {
                $("#<%=hiddenGuestName.ClientID %>").val('');
                $("#<%=hiddenGuestId.ClientID %>").text('');
                $("#PopSearchPanel").hide();
                $("#PopTabPanel").hide();
                //popup(-1);
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
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);

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
            //popup(1, 'TouchKeypad', '', 935, 500);
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

        function LoadDataOnParentForm() {
            var guestId = $("#<%=hiddenGuestId.ClientID %>").val();
            //popup(-1);
            $("#TouchKeypad").dialog("close");
            PageMethods.LoadDetailInformation(guestId, OnLoadParentFromDetailObjectSucceeded, OnLoadParentFromDetailObjectFailed);
            return false;
        }
        function OnLoadParentFromDetailObjectSucceeded(result) {
            if (result.GuestPreferences != null) {
                $("#GuestPreferenceDiv").show();
            }
            else $("#GuestPreferenceDiv").hide();
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
            $("#hfSearchDetailsFireOrNot").val("0");
            toastr.error(error.get_message());
        }


        function SetRegistrationInfoByRoomTypeId(RoomTypeId, DirtyRoomNumber) {

            if ($.trim(DirtyRoomNumber) != "") {
                toastr.info("Before Registration Please Clean Dirty Room No. " + DirtyRoomNumber);
            }
            $('#<%=ddlRoomType.ClientID%>').val(RoomTypeId);
            PopulateRooms();
            //PerformFillFormActionByTypeIdForReservation(resId, RoomTypeId);
        }

        function PerformFillFormActionByTypeIdForReservation(resId, roomTypeId) {

            PageMethods.PerformFillFormActionByTypeIdTest(resId, roomTypeId, PerformFillFormActionByTypeIdForReservationSucceeded, PerformFillFormActionByTypeIdForReservationFailed);
            return false;
        }
        function PerformFillFormActionByTypeIdForReservationSucceeded(result) {
            $("#<%=txtUnitPrice.ClientID %>").val(result.UnitPrice);
            $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.UnitPrice);

            var roundedRoomRate = Math.round(result.UnitPrice.toFixed(2));

            $("#<%=txtRoomRate.ClientID %>").val(roundedRoomRate);

            $("#<%=ddlRoomId.ClientID %>").val(result.RoomId);
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
                    $("#<%=txtRoomRate.ClientID %>").val(FinalAmount.toFixed(2));
                }
                else {
                    if (txtUnitPriceHiddenField != '') {
                        var percentage = parseFloat(txtUnitPriceHiddenField) * parseFloat(discountAmount) / 100;
                        var FinalAmount = parseFloat(txtUnitPriceHiddenField) - percentage;

                        var roundedRoomRate = Math.round(FinalAmount.toFixed(2));

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

        function LoadDetailGridInformation() {
            //Guest Detail
            var txtGuestName = $("#<%=txtGuestName.ClientID %>").val();
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
            var txtGuestZipCode = $("#<%=txtGuestZipCode.ClientID %>").val();
            var txtNationalId = $("#<%=txtNationalId.ClientID %>").val();
            var txtPassportNumber = $("#<%=txtPassportNumber.ClientID %>").val();
            var txtPExpireDate = $("#<%=txtPExpireDate.ClientID %>").val();
            var txtPIssueDate = $("#<%=txtPIssueDate.ClientID %>").val();
            var txtPIssuePlace = $("#<%=txtPIssuePlace.ClientID %>").val();
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

            if (enteredCountry.toString() != txtGuestCountrySearch.toString()) {
                toastr.warning('Please Enter Valid Country Name.');
                $("#txtGuestCountrySearch").focus();
                return;
            }

            if (txtGuestCountrySearch == "") {
                toastr.warning('Please Enter Country Name.');
                $("#txtGuestCountrySearch").focus();
                return;
            }

            if (defaultCountry != ddlGuestCountry) {

                /*if ($.trim($("#ContentPlaceHolder1_txtVisaNumber").val()) == "") {
                toastr.warning('Please Enter Visa Number.');
                $("#ContentPlaceHolder1_txtVisaNumber").focus();
                return;
                }
                else if ($.trim($("#ContentPlaceHolder1_txtVExpireDate").val()) == "") {
                toastr.warning('Please Enter Visa Expire Date.');
                $("#ContentPlaceHolder1_txtVExpireDate").focus();
                return;
                }
                else*/
                if ($.trim($("#ContentPlaceHolder1_txtPassportNumber").val()) == "") {
                    toastr.warning('Please Enter Passport Number.');
                    $("#ContentPlaceHolder1_txtPassportNumber").focus();
                    return;
                }
            }

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
            //            if (txtGuestPhone != "") {
            //                var phnformat = /[1-9](?:\d{0,2})(?:,\d{3})*(?:\.\d*[1-9])?|0?\.\d*[1-9]|0/;
            //                if (txtGuestPhone.match(phnformat)) {
            //                }
            //                else {
            //                    toastr.warning("You have entered an invalid Phone Number!");
            //                    return;
            //                }
            //            }

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

            var RegId = parseInt(tempRegId);

            var guestDeletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();

            PageMethods.SaveGuestInformationAsDetail(RegId, IntOwner, isEdit, txtGuestName, txtGuestEmail, hiddenGuestId, txtGuestDrivinlgLicense, txtGuestDOB, txtGuestAddress1, txtGuestAddress2, ddlProfessionId, txtGuestCity, ddlGuestCountry, txtGuestNationality, txtGuestPhone, ddlGuestSex, txtGuestZipCode, txtNationalId, txtPassportNumber, txtPExpireDate, txtPIssueDate, txtPIssuePlace, txtVExpireDate, txtVisaNumber, txtVIssueDate, 0, guestDeletedDoc, deletedGuestId, SelectdPreferenceId, OnLoadDetailGridInformationSucceeded, OnLoadDetailGridInformationFailed);
            return false;
        }

        function OnLoadDetailGridInformationSucceeded(result) {
            $("#ltlGuestDetailGrid").html(result);
            $("#<%=EditId.ClientID %>").val("");
            SelectdPreferenceId = "";
            clearUserDetailsControl();
            return false;
        }
        function OnLoadDetailGridInformationFailed(error) {
            $("#<%=EditId.ClientID %>").val("");
            if (error.toString == "2")
                toastr.warning('Provide Valid Email');

            toastr.error(error.get_message());
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
            $('#<%=txtContactPerson.ClientID%>').val(result.ContactPerson);
            $('#<%=txtContactNumber.ClientID%>').val(result.ContactNumber);
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
            // alert(StartDate);
            var EndDate = $('#<%=txtDepartureDate.ClientID%>').val();
            $.ajax({
                type: "POST",
                url: "/HotelManagement/frmHoldReCheckIn.aspx/PopulateRooms",
                data: '{RoomTypeId: ' + $('#<%=ddlRoomType.ClientID%>').val() + ',ResevationId:0,FromDate:"' + StartDate + '",ToDate:"' + EndDate + '"}',
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

        //        function PerformFillFormActionByRoomTypeId(actionId) {
        //            var action = $("#<%=ddlRoomType.ClientID %>").val();
        //            PageMethods.PerformFillFormActionByTypeId(action, OnFillRoomObjectSucceededByTypeId, OnFillRoomObjectFailedByTypeId);            
        //            return false;
        //        }
        //        function OnFillRoomObjectSucceededByTypeId(result) {
        //            $("#<%=txtRoomRate.ClientID %>").val(result.RoomRate);
        //            $("#<%=txtUnitPrice.ClientID %>").val(result.RoomRate);
        //            $("#<%=txtUnitPriceHiddenField.ClientID %>").val(result.RoomRate);            
        //            return false;
        //        }
        //        function OnFillRoomObjectFailedByTypeId(error) {
        //            alert(error.get_message());
        //        }

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

            PageMethods.PerformEditActionForGuestDetailByWM(GuestId, RegistrationId, OnEditGuestInformationSucceeded, OnEditGuestInformationFailed);
            return false;
        }

        function OnEditGuestInformationSucceeded(result) {
            $("#GuestPreferenceDiv").show();
            var guestInfo = result.GuestInfo;
            var guestDoc = result.GuestDoc;

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
            $("#<%=lblGstPreference.ClientID %>").text(result.GuestPreference);
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result.GuestPreferenceId);

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
            $("#<%=txtPIssuePlace.ClientID %>").val(guestInfo.PIssuePlace);

            var totalDoc = guestDoc.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='guestDocList' style='width:100%' cellspacing='0' cellpadding='4' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

                if (result.GuestDoc[row].Path != "") {
                    imagePath = "<img src='" + guestDoc[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

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

        function DeleteGuestDoc(docId, rowIndex) {

            var deletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();

            if (deletedDoc != "")
                deletedDoc += "," + docId;
            else
                deletedDoc = docId;

            $("#<%=hfGuestDeletedDoc.ClientID %>").val(deletedDoc);

            $("#trdoc" + rowIndex).remove();
        }

    </script>
    <!-- Upload  Script -->
    <script type="text/javascript">

        var alreadySavePaidServices = [];

        $(document).ready(function () {
            $('#TotalPaid').hide();

            $("#btnItemwiseSpecialRemarksCancel").click(function () {
                //CalculateServiceBill();
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
            var SelectedPaidServiceAll = []; //DetailServiceId

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

            //            if (alreadySavePaidServices.length != 0 && SelectedPaidServiceAll.length == 0) {
            //                SelectedPaidServiceAll = alreadySavePaidServices;
            //            }

            if (registrationId == 0) {

                var queryReservationId = $("#<%=QSReservationId.ClientID %>").val();

                //                if (queryReservationId != "" || ddlReservationId != "0") {
                //                    $("#<%= hfPaidServiceDeleteObj.ClientID %>").val("0");
                //                }
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

        function TotalRoomRateVatServiceChargeCalculation() {
            //UpdateDiscountAmount();
            var txtRoomRate = '<%=txtRoomRate.ClientID%>'
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

            var txtRoomRateVal = parseFloat($('#' + txtRoomRate).val());

            var InclusiveBill = 0, Vat = 0.00, ServiceCharge = 0.00;

            if ($("#<%=hfInclusiveHotelManagementBill.ClientID %>").val() != "")
            { InclusiveBill = parseInt($("#<%=hfInclusiveHotelManagementBill.ClientID %>").val(), 10); }

            if ($("#<%=hfGuestHouseVat.ClientID %>").val() != "")
                Vat = parseFloat($("#<%=hfGuestHouseVat.ClientID %>").val());

            if ($("#<%=hfGuestHouseServiceCharge.ClientID %>").val() != "")
                ServiceCharge = parseFloat($("#<%=hfGuestHouseServiceCharge.ClientID %>").val());

            //alert($('#' + txtRoomRate).val() + "b");

            var InnboardRateCalculationInfo = CommonHelper.GetRackRateServiceChargeVatInformation(InclusiveBill, Vat, ServiceCharge, 0, txtRoomRateVal, parseInt(cbServiceChargeVal, 10), 0, parseInt(cbVatAmountVal, 10));
            OnLoadRackRateServiceChargeVatInformationSucceeded(InnboardRateCalculationInfo);

            //PageMethods.GetRackRateServiceChargeVatInformation(txtRoomRateVal, cbServiceChargeVal, cbVatAmountVal, OnLoadRackRateServiceChargeVatInformationSucceeded, OnLoadRackRateServiceChargeVatInformationFailed);
        }

        function OnLoadRackRateServiceChargeVatInformationSucceeded(result) {

            if (result.RackRate > 0) {
                $("#<%=txtTotalRoomRate.ClientID %>").val(result.RackRate);
                $("#<%=txtServiceCharge.ClientID %>").val(result.ServiceCharge);
                $("#<%=txtVatAmount.ClientID %>").val(result.VatAmount);
            }
            else {
                $("#<%=txtTotalRoomRate.ClientID %>").val('0');
                $("#<%=txtServiceCharge.ClientID %>").val('0');
                $("#<%=txtVatAmount.ClientID %>").val('0');
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
                $('#' + txtRoomRate).val(unitPrice.toFixed(2));
                return;
            }

            var totalPaidServiceAmount = CalculatePaidServiceTotal();
            unitPrice += totalPaidServiceAmount;

            var discount = 0.0;
            if (discountType == "Fixed") {
                discount = parseFloat(txtDiscountAmount);
                //roomRate = roomRate - discount;
                unitPrice = unitPrice - discount;
            }
            else {
                discount = parseFloat(txtDiscountAmount);
                //roomRate = roomRate - ((roomRate * discount) / 100);
                unitPrice = unitPrice - ((unitPrice * discount) / 100);
            }

            //$('#' + txtRoomRate).val(roomRate.toFixed(2));
            $('#' + txtRoomRate).val(unitPrice.toFixed(2));
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
            var isDateValid = true; //ValidateExpireDate();
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

        function OnLoadSalesDetailGridViewSucceeded(result) {
            $('#productDetailGrid').html(result);
            GetTotalPaidAmount();
            return false;
        }

        function OnLoadSalesDetailGridViewFailed(error) {
        }

        function PerformValidationForSave() {

            if (CommonHelper.IsDecimal($('#ContentPlaceHolder1_txtRoomRate').val()) == false) {
                toastr.warning('Entered Negotiated Rate is not in correct format.');
                return false;
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

            //            if ($("#ContentPlaceHolder1_txtDepartureHour").val() != "" && $("#ContentPlaceHolder1_txtDepartureMin").val() != "") {
            //                if (CommonHelper.IsInt($("#ContentPlaceHolder1_txtDepartureHour").val()) == false) {
            //                    toastr.warning("Please give valid Departure Hour.");
            //                    return false;
            //                }
            //                else if (CommonHelper.IsInt($("#ContentPlaceHolder1_txtDepartureMin").val()) == false) {
            //                    toastr.warning("Please give valid Departure Minute.");
            //                    return false;
            //                }
            //            }

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

            //            if ($("#ContentPlaceHolder1_txtDepartureHour").val() != "" || $("#ContentPlaceHolder1_txtDepartureMin").val() != "") {

            //                if (CommonHelper.IsInt($("#ContentPlaceHolder1_txtDepartureHour").val()) == false) {
            //                    toastr.warning("Please give valid Departure Hour.");
            //                    return false;
            //                }
            //                else if (CommonHelper.IsInt($("#ContentPlaceHolder1_txtDepartureMin").val()) == false) {
            //                    toastr.warning("Please give valid Departure Minute.");
            //                    return false;
            //                }
            //            }

            if ($('#btnAddDetailGuest').val() == 'Save') {
                toastr.warning("Please Click Save Button First to Update Guest Details.");
                return false;
            }

            //            if ($("#<%=hfIsPaidServiceAlreadySavedDb.ClientID %>").val() != "0" && $("#<%=hfPaidServiceSaveObj.ClientID %>").val("") != "")
            //                $("#<%= hfPaidServiceDeleteObj.ClientID %>").val("1");

            var validationStatus = false;

            if ($('#btnAddDetailGuest').val() == 'Save') {
                toastr.warning("Please Click Save Button First to Update Guest Details.");
                return false;
            }

            var commingForm = $("#<%=txtCommingFrom.ClientID %>").val();
            var departureFlightName = $("#<%=txtDepartureFlightName.ClientID %>").val();
            var rowCount = $('#TableWiseItemInformation tr').length;
            var NumberOfPersonAdult = $("#<%=txtNumberOfPersonAdult.ClientID %>").val();

            var cbFamilyOrCouple = '<%=cbFamilyOrCouple.ClientID%>'
            var rate = 0;

            if (rowCount <= 1) {
                toastr.warning("Please add at least one guest.");
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

            var buttonText = ('#ContentPlaceHolder1_btnSave').val();

            if (buttonText == 'Save') {
                if (commingForm == "" || departureFlightName == "") {
                    var answer = confirm("Do you want to save with out Others Information?")
                    if (answer) {
                        validationStatus = true;
                    }
                    else {
                        toastr.warning("Please provide Others Information.");
                        validationStatus = false;
                        return validationStatus;
                    }
                }

                if (selectedValues == "") {
                    var answer = confirm("Do you want to save with out any complementary item?")
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

            return false;
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
            else
            { isGuestNameGiven = true; }

            if (guestCountry == "") {
                return false;
            }
            else
            { isCountryNameGiven = true; }

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
            //popup(1, 'TouchKeypad', '', 935, 500);
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

            if (alreadyLoad != "0" && paidServiceDisplayOrNot != "0") {
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

            var registrationId = $("#<%=hfRegistrationId.ClientID %>").val();

            if (registrationId == "")
                registrationId = 0;

            var reservationId = 0;
            var paidServiceLoadFromRegistrationRReservation = true;

            if (registrationId == 0) {

                var queryReservationId = $("#<%=QSReservationId.ClientID %>").val();
                //                if (queryReservationId == "" && ddlReservationId == "0") {
                //                    paidServiceLoadFromRegistrationRReservation = true;
                //                }

                //                if (queryReservationId != "" || ddlReservationId != "0") {
                //                    paidServiceLoadFromRegistrationRReservation = false;
                //                    reservationId = (queryReservationId == "" ? ddlReservationId : queryReservationId);
                //                }
            }

            var isCeomplementaryService = $("#ContentPlaceHolder1_hfIsComplementaryPaidService").val();

            if (paidServiceLoadFromRegistrationRReservation) {
                $("#<%=hfPaidServiceSaveObj.ClientID %>").val("");
                alreadySavePaidServices = [];
                PageMethods.GetPaidServiceDetails(registrationId, currencyId, currencyType, convertionRate, isCeomplementaryService, OnGetPaidServiceSucceed, OnGetPaidServiceFailed);
            }
            else if (!paidServiceLoadFromRegistrationRReservation) {
                $("#<%=hfPaidServiceSaveObj.ClientID %>").val("");

                alreadySavePaidServices = [];
                //PageMethods.GetPaidServiceDetailsFromReservation(reservationId, currencyId, currencyType, convertionRate, OnGetPaidServiceSucceed, OnGetPaidServiceFailed);
            }

            return false;
        }

        var vvc = [], vvd = [];

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
            window.location = "frmRoomRegistration.aspx?pn=" + pageNumber + "&grc=" + ($("#ContentPlaceHolder1_gvRoomRegistration tbody tr").length + 1) + "&icp=" + isCurrentOrPreviousPage;

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
            alert(SelectdPreferenceName)
            $("#<%=lblGstPreference.ClientID %>").text(SelectdPreferenceName);
            //popup(-1);
            $("#DivGuestReference").dialog("close");
        }

        function ClosePreferenceDialog() {
            $("#DivGuestReference").dialog("close");
        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <%--<button type="button" class="close" data-dismiss="alert">
            ×</button>--%>
        <strong>Notificasion:</strong>
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
                        style="width: 80px;" value="Ok" />
                    <input type="button" id="btnItemwiseSpecialRemarksCancel" class="btn btn-primary btn-sm"
                        style="width: 80px;" value="Cancel" />
                </div>
                <div id="ItemWiseSpecialRemarksContainer" class="alert alert-info" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert">
                        ×</button>
                    <asp:Label ID='ItemWiseSpecialRemarksMessage' Font-Bold="True" runat="server" ClientIDMode="Static"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <input id="hfSearchDetailsFireOrNot" value="0" type="hidden" />
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfPaymentCurrencyType" runat="server" />
    <asp:HiddenField ID="hfConversionRate" runat="server" />
    <asp:HiddenField ID="hfIsRoomOverbookingEnable" runat="server" />
    <asp:HiddenField ID="hfInclusiveHotelManagementBill" runat="server" />
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
    <asp:HiddenField ID="hfPreviousCurrencyType" runat="server" />
    <asp:HiddenField ID="hfIsCurrencyChange" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsComplementaryPaidService" runat="server" Value="0" />
    <asp:HiddenField ID="hfGuestPreferenceId" runat="server" />
    <asp:HiddenField ID="hfHoldRegistrationId" runat="server" />
    <input id="hfPaidServiceDialogDisplayOrNot" value="1" type="hidden" />
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
            <%--<li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-5">Advance Payment</a></li>--%>
            <%--<li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search</a></li>--%>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <input id="EditId" runat="server" type="hidden" />
                <div class="panel-body">
                    <div class="form-horizontal">
                        <asp:HiddenField ID="RandomOwnerId" runat="server" />
                        <asp:HiddenField ID="tempRegId" runat="server" />
                        <asp:HiddenField ID="hiddendReservationId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="QSReservationId" runat="server"></asp:HiddenField>
                        <%--<div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:CheckBox ID="chkIsFromReservation" runat="server" CssClass="customChkBox" Text=""
                            onclick="javascript: return LoadTodaysReservation(); ToggleFieldVisible();" TabIndex="7" />
                        <asp:Label ID="lblCompany" runat="server" Text="Reservation"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlReservationId" runat="server" CssClass="TwoColumnTextBox"
                            TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:CheckBox ID="chkAllActiveReservation" runat="server" CssClass="customChkBox"
                            Text="" onclick="javascript: return ToggleFieldVisibleForAllActiveReservation(this);"
                            TabIndex="2" />
                        <asp:Label ID="Label12" runat="server" Text="All Active Reservation"></asp:Label>
                    </div>
                </div>--%>
                        <div id="ReservationInformation" style="padding-left: 15px; padding-right: 32px;">
                            <div class="divRightSectionWithThreeDvie">
                                <div id="ltlTableWiseItemInformation">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCheckInDate" runat="server" class="control-label required-field"
                                    Text="Check-In Date"></asp:Label>
                            </div>
                            <div class="col-md-4">                                
                                <asp:TextBox ID="txtCheckInDate" runat="server" CssClass="form-control" TabIndex="5"
                                    Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Re Check-In Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCheckInDateHiddenField" runat="server" CssClass="form-control"
                                    TabIndex="3" Visible="false"></asp:TextBox>
                                <div style="display: none;">
                                    <asp:TextBox ID="txtReCheckInDate" CssClass="form-control" runat="server"
                                        TabIndex="4"></asp:TextBox>
                                </div>
                                <asp:TextBox ID="txtDisplayCheckInDate" runat="server" CssClass="form-control"
                                    TabIndex="5" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblDepartureDate" runat="server" class="control-label required-field"
                                    Text="Departure Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDepartureDate" CssClass="form-control" runat="server"
                                    TabIndex="6"></asp:TextBox>
                            </div>
                        </div>
                        <%-- <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblBusinessPromotionId" runat="server" Text="Business Promotion"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlBusinessPromotionId" runat="server" CssClass="ThreeColumnDropDownList"
                            TabIndex="7">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="ListedCompanyInfo" class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:CheckBox ID="chkIsLitedCompany" runat="server" Text="" CssClass="customChkBox"
                            onclick="javascript: return ToggleFieldVisibleForListedCompany(this);" TabIndex="8" />
                        <asp:Label ID="Label1" runat="server" Text="Listed Company"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <div id="ListedCompany" style="display: none;">
                            <asp:DropDownList ID="ddlCompanyName" runat="server" TabIndex="8" CssClass="ThreeColumnDropDownList">
                            </asp:DropDownList>
                        </div>
                        <div id="ReservedCompany">
                            <asp:TextBox ID="txtReservedCompany" runat="server" TabIndex="10" CssClass="ThreeColumnTextBox"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>--%>
                        <div id="PaymentInformation" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblContactPerson" runat="server" class="control-label" Text="Contact Person"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblContactNumber" runat="server" class="control-label" Text="Mobile Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" TabIndex="12"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="Label8" runat="server" class="control-label" Text="Payment Mode"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPaymentMode" runat="server" TabIndex="13">
                                        <asp:ListItem Value="Company">Company</asp:ListItem>
                                        <asp:ListItem Value="Self">Self</asp:ListItem>
                                        <asp:ListItem Value="TBA">Before C/O (Company/ Host)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblPayFor" runat="server" class="control-label" Text="Pay For"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPayFor" runat="server" TabIndex="14">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <%--<div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblCurrency" runat="server" Text="Currency Type"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlCurrency" runat="server" TabIndex="15">
                        </asp:DropDownList>
                    </div>
                    <div id="CurrencyAmountInformationDiv" style="display: none">
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblCurrencyAmount" runat="server" Text="Conversion Rate"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtConversionRate" runat="server" TabIndex="16"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>--%>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Room Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomType" runat="server" TabIndex="17" CssClass="form-control">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtViewRoomType" runat="server" CssClass="form-control" TabIndex="18"
                                    Visible="false"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="HiddenCompanyId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtEntiteledRoomType" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblRoomId" runat="server" class="control-label required-field" Text="Room Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="ddlRoomIdHiddenField" runat="server"></asp:HiddenField>
                                <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control" TabIndex="19"
                                    Enabled="False">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblDiscountType" runat="server" class="control-label" Text="Discount Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control" TabIndex="20">
                                    <asp:ListItem>Fixed</asp:ListItem>
                                    <asp:ListItem>Percentage</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblDiscountAmount" runat="server" class="control-label" Text="Discount Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="form-control" TabIndex="21"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Rack Rate"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" TabIndex="22"
                                    Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="txtUnitPriceHiddenField" runat="server"></asp:HiddenField>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblRoomRate" runat="server" class="control-label required-field" Text="Negotiated Rate"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRoomRate" runat="server" CssClass="form-control" TabIndex="23"
                                    onblur="CalculateDiscount()"></asp:TextBox>
                                <%--<img src='../Images/service.png' title='Add Service' style="cursor: pointer;" onclick='javascript:return LoadPaidService()'
                                alt='Add Service' border='0' /></span>--%>
                            </div>
                        </div>
                        <asp:Panel ID="pnlRackRateServiceChargeVatInformation" runat="server">
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblServiceCharge" runat="server" class="control-label required-field"
                                        Text="Service Charge"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-11">
                                            <asp:TextBox ID="txtServiceCharge" runat="server" TabIndex="22" CssClass="form-control"
                                                Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hfServiceCharge" runat="server"></asp:HiddenField>
                                        </div>
                                        <div class="col-padding-left-none col-md-1">
                                            <asp:CheckBox ID="cbServiceCharge" runat="server" Text="" onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForServiceCharge(this);"
                                                TabIndex="8" Checked="True" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblVatAmount" runat="server" class="control-label required-field"
                                        Text="Vat Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-11">
                                            <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="23" CssClass="form-control"
                                                Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hfVatAmount" runat="server"></asp:HiddenField>
                                        </div>
                                        <div class="col-padding-left-none col-md-1">
                                            <asp:CheckBox ID="cbVatAmount" runat="server" Text="" onclick="javascript: return ToggleTotalRoomRateVatServiceChargeCalculationForVat(this);"
                                                TabIndex="8" Checked="True" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblTotalRoomRate" runat="server" class="control-label required-field"
                                        Text="Total Room Rate"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTotalRoomRate" runat="server" CssClass="form-control" TabIndex="22"
                                        Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblEntitleRoomType" runat="server" class="control-label required-field"
                                    Text="Entitle Room Type" Visible="false"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEntitleRoomType" runat="server" TabIndex="24" CssClass="form-control"
                                    Visible="false">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblEntitleRoomRate" runat="server" class="control-label" Text="Entitle Room Rate"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEntitleRoomRate" runat="server" CssClass="form-control" TabIndex="25"
                                    Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                            </div>
                            <div class="col-md-4">
                            </div>
                            <div class="col-md-2 label-align">
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>
                        <div class="form-group" style="display: none;">
                            <div style="float: left;">
                                <asp:CheckBox TabIndex="26" ID="chkAdvancePayment" runat="Server" Text="Is Advance Payment?"
                                    onclick="javascript: return ToggleAdvancePaymentFieldVisible(this);" Font-Bold="true"
                                    class="form-control" TextAlign="right" Visible="false" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div style="float: right; padding-right: 10px">
                                <input id="btnNext1" tabindex="27" type="button" value="Next" class="btn btn-primary btn-sm"
                                    style="display: none;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--<div id="tab-2">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblRoomNumber" runat="server" Text="Room No."></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtSearchRoomNumber" runat="server" CssClass="customMediumTextBoxSize"
                        TabIndex="32"></asp:TextBox>
                </div>
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="Label15" runat="server" Text="Registration No."></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtSearchRegistrationNumber" runat="server" CssClass="customMediumTextBoxSize"
                        TabIndex="33"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblSearchCompanyName" runat="server" Text="Company Name"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtSearchCompanyName" runat="server" CssClass="customMediumTextBoxSize"
                        TabIndex="30"></asp:TextBox>
                </div>
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblSearchCountry" runat="server" Text="Country"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlSearchCountry" runat="server" TabIndex="31">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblCheckInDate" runat="server" Text="Check-In Date"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtChkInDate" runat="server" CssClass="datepicker" TabIndex="28"></asp:TextBox>
                </div>
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblSearchGuestName" runat="server" Text="Guest Name"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtSearchGuestName" runat="server" CssClass="customMediumTextBoxSize"
                        TabIndex="29"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                    TabIndex="34" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" TabIndex="35" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                    Visible="false" OnClientClick="javascript: return PerformClearAction();" />
            </div>
            <asp:GridView ID="gvRoomRegistration" Width="100%" runat="server" AutoGenerateColumns="False"
                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowCommand="gvRoomRegistration_RowCommand"
                OnRowDataBound="gvRoomRegistration_RowDataBound">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label></ItemTemplate>
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
                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("RegistrationId") %>'
                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                ToolTip="Edit" />
                            &nbsp;<asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False"
                                CommandArgument='<%# bind("RegistrationId") %>' CommandName="CmdPreview" ImageUrl="~/Images/ReportDocument.png"
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
            <div class="divClear">
            </div>
            <div class="childDivSection">
                <div class="pagination pagination-centered" id="GridPagingContainer">
                    <ul>
                        <asp:Literal ID="gridPaging" runat="server"></asp:Literal>
                    </ul>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div id="ImageDivTest">
            </div>
        </div>--%>
        <div id="tab-3">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblNumberOfPersonAdult" runat="server" class="control-label required-field"
                                    Text="Person(Adult)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtNumberOfPersonAdult" runat="server" CssClass="form-control" TabIndex="35">1</asp:TextBox>
                                    </div>
                                    <div class="col-md-1 col-padding-left-none">
                                        <asp:CheckBox ID="cbFamilyOrCouple" runat="server" Text="" TabIndex="9" />
                                    </div>
                                    <div class="col-md-5 col-padding-left-none">
                                        <asp:Label ID="Label16" runat="server" class="control-label" Text="Family/ Couple"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblNumberOfPersonChild" runat="server" class="control-label" Text="Person(Child)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNumberOfPersonChild" runat="server" CssClass="form-control" TabIndex="36"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="SearchPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Individual Guest Information</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group" id="divReservationGuest">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblReservationGuest" runat="server" class="control-label" Text="Guest Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlReservationGuest" TabIndex="37" CssClass="form-control"
                                        runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestName" runat="server" class="control-label required-field"
                                        Text="Name"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtGuestName" CssClass="form-control" runat="server" TabIndex="38"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <button type="button" id="btnAddPerson" class="btn btn-primary btn-sm">
                                        Search Guest</button>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestDOB" runat="server" class="control-label" Text="Date Of Birth"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestDOB" runat="server" CssClass="form-control" TabIndex="39"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestSex" runat="server" class="control-label required-field" Text="Gender"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlGuestSex" runat="server" CssClass="form-control" TabIndex="40">
                                        <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                        <asp:ListItem>Male</asp:ListItem>
                                        <asp:ListItem>Female</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestAddress1" runat="server" class="control-label" Text="Company Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtGuestAddress1" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestAddress2" runat="server" class="control-label" Text="Address"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtGuestAddress2" runat="server" CssClass="form-control" TabIndex="42"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestEmail" runat="server" class="control-label" Text="Email Address"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtGuestEmail" runat="server" TabIndex="43" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblProfession" runat="server" class="control-label" Text="Profession"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlProfessionId" runat="server" TabIndex="40" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestPhone" runat="server" class="control-label" Text="Phone Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestPhone" runat="server" CssClass="form-control" TabIndex="44"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestCity" runat="server" class="control-label" Text="City"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestCity" runat="server" CssClass="form-control" TabIndex="45"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestZipCode" runat="server" class="control-label" Text="Zip Code"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestZipCode" runat="server" CssClass="form-control" TabIndex="46"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestCountry" runat="server" class="control-label required-field"
                                        Text="Country"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <input id="txtGuestCountrySearch" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlGuestCountry" runat="server" CssClass="form-control" TabIndex="47">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestNationality" runat="server" class="control-label" Text="Nationality"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestNationality" runat="server" CssClass="form-control" TabIndex="48"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGuestDrivinlgLicense" runat="server" class="control-label" Text="Driving License"></asp:Label>
                                    <%--<asp:Label ID="lblGuestAuthentication" runat="server" Text="Authentication"></asp:Label>--%>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtGuestDrivinlgLicense" runat="server" CssClass="form-control"
                                        TabIndex="49"></asp:TextBox>
                                    <%--<asp:DropDownList ID="ddlGuestAuthentication" runat="server" TabIndex="28">
                                        </asp:DropDownList>--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblNationalId" runat="server" class="control-label" Text="National ID"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNationalId" runat="server" CssClass="form-control" TabIndex="50"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblVisaNumber" runat="server" class="control-label" Text="Visa Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtVisaNumber" runat="server" CssClass="form-control" TabIndex="51"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblVIssueDate" runat="server" class="control-label" Text="Visa Issue Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtVIssueDate" runat="server" CssClass="form-control"
                                        TabIndex="52"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblVExpireDate" runat="server" class="control-label" Text="Visa Expiry Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtVExpireDate" runat="server" CssClass="form-control"
                                        TabIndex="53"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblPassportNumber" runat="server" class="control-label" Text="Passport Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPassportNumber" runat="server" CssClass="form-control" TabIndex="54"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblPIssuePlace" runat="server" class="control-label" Text="Pass. Issue Place"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPIssuePlace" runat="server" CssClass="form-control" TabIndex="55"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblPIssueDate" runat="server" class="control-label" Text="Pass. Issue Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPIssueDate" runat="server" CssClass="form-control"
                                        TabIndex="56"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblPExpireDate" runat="server" class="control-label" Text="Pass. Expiry Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPExpireDate" runat="server" CssClass="form-control"
                                        TabIndex="57"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" id="GuestPreferenceDiv" style="display: none">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblGstPreferences" runat="server" class="control-label" Text="Guest Preferences"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <%--<asp:TextBox ID="txtGstPreferences" runat="server" CssClass="ThreeColumnTextBox"
                                    TextMode="MultiLine" TabIndex="7" ReadOnly></asp:TextBox>--%>
                                    <asp:Label ID="lblGstPreference" runat="server" class="control-label"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <input type="button" tabindex="18" id="btnGuestReferences" value="Preferences" class="btn btn-primary btn-sm"
                                        onclick="javascript:return LoadGuestReference()" />
                                </div>
                            </div>
                            <div class="childDivSection">
                                <div id="GuestocumentsInformation" class="panel panel-default" style="height: 270px;">
                                    <div class="panel-heading">
                                        Guest Documents Information</div>
                                    <div class="panel-body">
                                        <div class="form-group">
                                            <div class="form-group">
                                                <div class="col-md-2 label-align">
                                                    <asp:Label ID="lblItemImage" runat="server" class="control-label" Text="Guest Document"
                                                        Visible="false"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                                                        <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                                                            FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="GuestDocumentInfo">
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <input id="btnAddDetailGuest" type="button" value="Add" class="btn btn-primary btn-sm" />
                                    <asp:Button ID="btnCancelDetailGuest" runat="server" Text="Cancel" CssClass="btn btn-primary btn-sm"
                                        TabIndex="58" OnClick="btnCancelDetailGuest_Click" />
                                    <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div style="float: right; padding-right: 30px">
                                    <input id="btnNext2" type="button" tabindex="59" value="Next" class="btn btn-primary btn-sm"
                                        style="display: none;" />
                                </div>
                                <div style="float: right; padding-right: 10px">
                                    <input id="btnPrev1" type="button" value="Prev" tabindex="60" class="btn btn-primary btn-sm"
                                        style="display: none;" />
                                </div>
                            </div>
                            <div id="ltlGuestDetailGrid">
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
                    Others Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCommingFrom" runat="server" class="control-label" Text="Coming From"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCommingFrom" runat="server" CssClass="form-control" TabIndex="61"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblNextDestination" runat="server" class="control-label" Text="Next Destination"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtNextDestination" runat="server" CssClass="form-control" TabIndex="62"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblVisitPurpose" runat="server" class="control-label" Text="Visit Purpose"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtVisitPurpose" runat="server" CssClass="form-control" TabIndex="63"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblGuestSource" runat="server" class="control-label" Text="Guest Source"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlGuestSource" runat="server" CssClass="form-control" TabIndex="64">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Reference"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReferenceId" runat="server" CssClass="form-control" TabIndex="65">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIsCompanyGuest" runat="server" class="control-label" Text="Complimentary Guest"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlIsCompanyGuest" runat="server" CssClass="form-control" TabIndex="70">
                                            <asp:ListItem>No</asp:ListItem>
                                            <asp:ListItem>Yes</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-inline col-md-4">
                                        <asp:Label ID="lblIsHouseUseRoom" runat="server" class="control-label" Text="House Use"></asp:Label>
                                    </div>
                                    <div class="form-inline col-md-4">
                                        <asp:DropDownList ID="ddlIsHouseUseRoom" runat="server" CssClass="form-control" TabIndex="70">
                                            <asp:ListItem>No</asp:ListItem>
                                            <asp:ListItem>Yes</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblRoomOwner" runat="server" class="control-label" Text="Room Owner"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomOwner" runat="server" CssClass="form-control" TabIndex="71">
                                    <asp:ListItem>No</asp:ListItem>
                                    <asp:ListItem>Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Visited Type"></asp:Label>
                            </div>
                            <div class="col-md-4 checkbox" style="padding-left: 35px;">
                                <asp:CheckBox ID="chkIsReturnedGuest" TabIndex="72" runat="Server" Text="Is previously visited guest?"
                                    Font-Bold="true" TextAlign="right" />
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblVIPType" runat="server" class="control-label" Text="VIP Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-6 checkbox" style="padding-left: 35px;">
                                        <asp:CheckBox ID="chkIsVIPGuest" TabIndex="73" runat="Server" Text="Is VIP guest?"
                                            Font-Bold="true" TextAlign="right" />
                                    </div>
                                    <div class="form-inline col-md-6" style="padding-left: 11px;">
                                        <asp:DropDownList ID="ddlVIPGuestType" runat="server" CssClass="form-control" TabIndex="65">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TabIndex="63"
                                    TextMode="MultiLine"></asp:TextBox>
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
                    Arrival Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblAirportPickUp" runat="server" class="control-label" Text="Airport Pick-Up"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAirportPickUp" CssClass="form-control" runat="server">
                                    <asp:ListItem>NO</asp:ListItem>
                                    <asp:ListItem>YES</asp:ListItem>
                                    <asp:ListItem>TBA</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="AirportPickUpInformationDiv">
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblArrivalFlightName" runat="server" class="control-label" Text="Airline Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtArrivalFlightName" runat="server" CssClass="form-control" TabIndex="74"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblArrivalFlightNumber" runat="server" class="control-label" Text="Flight Number"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtArrivalFlightNumber" runat="server" CssClass="form-control" TabIndex="75"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblArrivalTime" runat="server" class="control-label" Text="Arrival Time (ETA)"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtArrivalHour" placeholder="12" CssClass="form-control" runat="server"
                                                TabIndex="76"></asp:TextBox>&nbsp;:
                                        </div>
                                        <div class="col-md-4 col-padding-left-none">
                                            <asp:TextBox ID="txtArrivalMin" placeholder="00" CssClass="form-control" TabIndex="77"
                                                runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4 col-padding-left-none">
                                            <asp:DropDownList ID="ddlArrivalAmPm" CssClass="form-control" runat="server" TabIndex="78">
                                                <asp:ListItem>AM</asp:ListItem>
                                                <asp:ListItem>PM</asp:ListItem>
                                            </asp:DropDownList>
                                            (12:00AM)
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    Departure Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblAirportDrop" runat="server" class="control-label" Text="Airport Drop"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAirportDrop" CssClass="form-control" runat="server">
                                    <asp:ListItem>NO</asp:ListItem>
                                    <asp:ListItem>YES</asp:ListItem>
                                    <asp:ListItem>TBA</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="AirportDropInformationDiv">
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblDepartureFlightName" runat="server" class="control-label" Text="Airline Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtDepartureFlightName" runat="server" CssClass="form-control" TabIndex="79"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblDepartureFlightNumber" runat="server" class="control-label" Text="Flight Number"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtDepartureFlightNumber" runat="server" CssClass="form-control"
                                        TabIndex="80"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblDepartureTime" runat="server" class="control-label" Text="Departure Time (ETD)"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDepartureHour" placeholder="12" runat="server" CssClass="form-control"
                                        TabIndex="81"></asp:TextBox>
                                    <%--<asp:TextBox ID="txtDepartureMin" placeholder="00" CssClass="CustomMinuteSize" TabIndex="82"
                                    runat="server"></asp:TextBox>
                                <asp:DropDownList ID="ddlDepartureAmPm" CssClass="CustomAMPMSize" runat="server"
                                    TabIndex="83">
                                    <asp:ListItem>AM</asp:ListItem>
                                    <asp:ListItem>PM</asp:ListItem>
                                </asp:DropDownList>
                                (12:00AM)--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default" id="CreditCardInfo" runat="server">
                <div class="panel-heading">
                    Credit Card Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCardType" runat="server" class="control-label" Text="Card Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCreditCardType" TabIndex="93" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="AmericanExpress">American Express</asp:ListItem>
                                    <asp:ListItem Value="MasterCard">Master Card</asp:ListItem>
                                    <asp:ListItem Value="VisaCard">Visa Card</asp:ListItem>
                                    <asp:ListItem Value="DiscoverCard">Discover Card</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCardNo" runat="server" class="control-label" Text="Card Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCardNo" TabIndex="94" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCardHolder" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCardHolder" TabIndex="96" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblExpiryDate" runat="server" class="control-label" Text="Expiry Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtExpiryDate" TabIndex="95" CssClass="form-control"
                                    runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCardRef" runat="server" class="control-label" Text="Card Reference"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCardRef" TabIndex="96" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--<div id="tab-5">
            <div id="PaymentDetailsInformation" class="childDivSection">
                <div class="block" style="min-height: 160px">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Payment Information
                    </a>
                    <div class="childDivSectionDivBlockBody">
                        <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblPaymentType" runat="server" Text="Payment Type"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:DropDownList ID="ddlPaymentType" runat="server" TabIndex="4">
                                    <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                    <asp:ListItem Value="PaidOut">Paid Out</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblPayMode" runat="server" Text="Payment Mode"></asp:Label>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:DropDownList ID="ddlPayMode" runat="server" TabIndex="5">
                                    <asp:ListItem>Cash</asp:ListItem>
                                    <asp:ListItem>Card</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="Label13" runat="server" Text="Currency Type"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:DropDownList ID="ddlPaymentCurrency" TabIndex="6" runat="server">
                                </asp:DropDownList>
                                <asp:Label ID="lblDisplayConvertionRate" runat="server" Text=""></asp:Label>
                                <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblReceiveLeadgerAmount" runat="server" Text="Receive Amount"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:TextBox ID="txtReceiveLeadgerAmount" runat="server" TabIndex="85"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div id="ConversionPanel" class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblConversionRate" runat="server" Text="Conversion Rate"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtPaymentConversionRate" runat="server"></asp:TextBox>
                                <asp:HiddenField ID="txtPaymentConversionRateHiddenField" runat="server"></asp:HiddenField>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="Label14" runat="server" Text="Calculated Amount"></asp:Label>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:HiddenField ID="txtPaymentCalculatedLedgerAmountHiddenField" runat="server">
                                </asp:HiddenField>
                                <asp:TextBox ID="txtPaymentCalculatedLedgerAmount" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection" style="display: none;">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblPaymentAccountHead" runat="server" Text="Account Head"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <div id="CashPaymentAccountHeadDiv">
                                    <asp:DropDownList ID="ddlCashReceiveAccountsInfo" runat="server" TabIndex="86" CssClass="childDivSectionDivThreeColumnDropDownList">
                                    </asp:DropDownList>
                                </div>
                                <div id="BankPaymentAccountHeadDiv" style="display: none;">
                                    <asp:DropDownList ID="ddlCardReceiveAccountsInfo" runat="server" TabIndex="87" CssClass="childDivSectionDivThreeColumnDropDownList">
                                    </asp:DropDownList>
                                </div>
                                <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
                                    <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" TabIndex="88"
                                        CssClass="childDivSectionDivThreeColumnDropDownList">
                                    </asp:DropDownList>
                                </div>
                                <div id="PaidByOtherRoomDiv" style="display: none">
                                    <asp:DropDownList ID="ddlPaidByRegistrationId" runat="server" TabIndex="89" CssClass="childDivSectionDivThreeColumnDropDownList">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                            <div class="divClear">
                            </div>
                            <div class="divSection" style="display: none;">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblAccountsPostingHeadId" runat="server" Text="AccountsPosting Head"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlAccountsPostingHeadId" runat="server" TabIndex="90" AutoPostBack="True">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblChecqueNumber" runat="server" Text="Cheque Number"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtChecqueNumber" runat="server" TabIndex="91" CssClass="ThreeColumnTextBox"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                        </div>
                        <div id="CardPaymentAccountHeadDiv" style="display: none;">
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="Label9" runat="server" Text="Card Type"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlCardType" TabIndex="93" runat="server" CssClass="tdLeftAlignWithSize">
                                        <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                        <asp:ListItem Value="a">American Express</asp:ListItem>
                                        <asp:ListItem Value="m">Master Card</asp:ListItem>
                                        <asp:ListItem Value="v">Visa Card</asp:ListItem>
                                        <asp:ListItem Value="d">Discover Card</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblCardNumber" runat="server" Text="Card Number"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtCardNumber" TabIndex="94" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection" style="display: none;">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="Label10" runat="server" Text="Expiry Date"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtExpireDate" TabIndex="95" CssClass="datepicker" runat="server"></asp:TextBox>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="Label11" runat="server" Text="Card Holder Name"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtCardHolderName" TabIndex="96" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblBankId" runat="server" Text="Bank Name"></asp:Label>
                                    <span class="MandatoryField">*</span>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <input id="txtBankId" type="text" class="ThreeColumnTextBox" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlBankId" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection" style="padding-left: 10px;">
                            <input type="button" id="btnAddDetailGuestPayment" tabindex="97" value="Add" class="TransactionalButton btn btn-primary" />
                            <asp:Label ID="lblHiddenIdDetailGuestPayment" runat="server" Text='' Visible="False"></asp:Label>
                        </div>
                        <div class="divClear">
                        </div>
                        <div id="GuestPaymentDetailGrid" class="childDivSection">
                        </div>
                        <div id="TotalPaid" class="totalAmout">
                        </div>
                        <div class="divClear">
                        </div>
                        <div>
                        </div>
                        <div class="divClear">
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
        </div>--%>
        <div id="tab-6">
            <div class="panel-body">
                <div class="checkbox checkboxlist col-md-12">
                    <asp:CheckBoxList ID="chkComplementaryItem" TabIndex="98" runat="server">
                    </asp:CheckBoxList>
                </div>
            </div>
        </div>
    </div>
    <div class="row" id="SubmitButtonDiv" style="padding: 10px 0 0 25px;">
        <div class="col-md-12">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btn-sm"
                TabIndex="99" OnClick="btnSave_Click" PostBackUrl="~/HotelManagement/frmHoldReCheckIn.aspx"
                OnClientClick="javascript: return PerformValidationForSave();" />
            <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="btn btn-primary btn-sm"
                TabIndex="100" OnClick="btnCancel_Click" />
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
                Guest Information</div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2 label-align">
                            <asp:HiddenField ID="hiddenGuestId" runat="server" />
                            <asp:HiddenField ID="hiddenGuestName" runat="server" />
                            <asp:Label ID="lblSrcGuestName" runat="server" class="control-label" Text="Guest Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <div class="row">
                                <div class="col-md-11">
                                    <asp:TextBox ID="txtSrcGuestName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                                <div class="form-inline col-md-1">
                                    <div style="float: right; margin-left: 150px">
                                        <img id="imgCollapse" width="30px" src="/HotelManagement/Image/expand_alt.png" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="ExtraSearch">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSrcCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSrcRoomNumber" runat="server" class="control-label" Text="Room No."></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSrcRegistrationNumber" runat="server" class="control-label" Text="Registration No."></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcRegistrationNumber" runat="server" CssClass="form-control"
                                    TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSrcFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcFromDate" runat="server" CssClass="form-control"
                                    TabIndex="5"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSrcToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcToDate" runat="server" CssClass="form-control"
                                    TabIndex="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSrcEmailAddress" runat="server" class="control-label" Text="Email Address"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcEmailAddress" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSrcMobileNumber" runat="server" class="control-label required-field"
                                    Text="Mobile Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcMobileNumber" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSrcNationalId" runat="server" class="control-label required-field"
                                    Text="National ID"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcNationalId" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSrcDateOfBirth" runat="server" class="control-label required-field"
                                    Text="Date of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSrcDateOfBirth" runat="server" CssClass="form-control"
                                    TabIndex="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSrcPassportNumber" runat="server" class="control-label required-field"
                                    Text="Passport Number"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSrcPassportNumber" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
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
                Search Information</div>
            <div class="panel-body">
                <div id="ltlTableSearchGuest">
                </div>
            </div>
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
                            Guest Information</div>
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
                                            <asp:Label ID="lblLGuestDOB" runat="server" Text="Date
    of Birth"></asp:Label>
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
                                        <td class="span2">
                                            Visited Type
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
                </div>
                <div id="Poptab-2">
                    <div id="imageDiv">
                    </div>
                </div>
                <div id="Poptab-3">
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div id="guestHistoryDiv">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Poptab-4">
                    <div id="Div5" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Preferences</div>
                        <div id="Preference" class="panel-body">
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
        <div class='divClear'>
        </div>
    </div>
    <!-- End Pop Up Guest Search -->
    <!--Guest Reference PopUp -->
    <div id="DivGuestReference" style="display: none;">
        <div id="Div1">
            <div id="ltlGuestReference">
            </div>
        </div>
    </div>
    <!--End Guest Reference PopUp -->
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
        if (aireportPickupInformationPanelEnable > -1)
        { AireportPickupInformationPanelVisibleTrue(); }
        else { AireportPickupInformationPanelVisibleFalse(); }

        /*var accountsPosting = '<%=isAccountsPostingPanelEnable%>';
        if (accountsPosting > -1) { EntryPanelVisibleTrue(); } else {
        EntryPanelVisibleFalse();
        }*/


        var isRoomAvailableForRegistrationEnable = '<%=IsRoomAvailableForRegistrationEnable%>';
        if (isRoomAvailableForRegistrationEnable > -1) {
            IsRoomAvailableForRegistrationShow();
        }

        $(document).ready(function () {
            if ($("#<%=btnSave.ClientID %>").val() != 'Save') {
                $($("#myTabs").find("li")[4]).hide();
            }
        });
    </script>
</asp:Content>
