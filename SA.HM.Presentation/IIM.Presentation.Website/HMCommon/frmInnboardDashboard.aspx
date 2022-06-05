<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmInnboardDashboard.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmInnboardDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        ol {
            list-style-type: none;
        }

        body.dragging, body.dragging * {
            cursor: move !important;
        }

        .dragged {
            position: absolute;
            opacity: 0.5;
            z-index: 2000;
        }

        ol.example li.placeholder {
            position: relative; /** More li styles **/
        }

            ol.example li.placeholder:before {
                position: absolute; /** Define arrowhead **/
            }

        ol.example {
            min-height: 200px;
        }

            ol.example li {
                height: 405px;
                border: 1px solid #999999;
            }
    </style>
    <script type="text/javascript">
        var vv = [];
        var div1 = "", div2 = "", div3 = "", div4 = "", div5 = "", div6 = "", div7 = "", div8 = "", div9 = "", div10 = "", div11 = "", div12 = "", div13 = "", div14 = "";

        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "";
            var formName = "<li class='active'>Dashboard</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $('#ContentPlaceHolder1_txtProbableCleanTime').timepicker({
                showPeriod: is12HourFormat
            });

            PageMethods.GetManagement(OnManagementSuccess, OnManagementFail);

            $("#PopMyTabs").tabs();
            $("#PopReservation").tabs();

            $("#btnBackToDecisionMaker").click(function () {
                $("#serviceDecider").dialog("close");
                $("#serviceDecider").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 600,
                    closeOnEscape: true,
                    resizable: false,
                    title: "Occupied Room Possible Path",
                    show: 'slide'
                });
            });

            var ddlCleanStatus = '<%=ddlCleanStatus.ClientID%>'
            $('#' + ddlCleanStatus).change(function () {
                var status = $("#<%=ddlCleanStatus.ClientID %>").val();
                if (status == "OutOfOrder") {
                    $('#OutOfServiceDivInfo').show("slow");
                    $('#WithoutOutOfServiceDivInfo').hide("slow");
                }
                else {
                    if ($('#' + ddlCleanStatus).val() == "Cleaned") {
                        $("#<%=txtRemarks.ClientID %>").val("");
                    }
                    $('#OutOfServiceDivInfo').hide("slow");
                    $('#WithoutOutOfServiceDivInfo').show("slow");
                }
            });

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'
            var txtApprovedDate = '<%=txtApprovedDate.ClientID%>'
            $('#' + txtFromDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtToDate).datepicker("option", "minDate", selectedDate);
                }
            });
            $('#' + txtToDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtFromDate).datepicker("option", "maxDate", selectedDate);
                }
            });

            $('#' + txtApprovedDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });

        function PerformViewActionForGuestDetail(guestId) {
            PageMethods.PerformViewActionForGuestDetail(guestId, OnPerformViewActionForGuestDetailSucceeded, OnGetRegistrationInformationByRoomNumberFailed);
            return false;
        }

        function OnPerformViewActionForGuestDetailSucceeded(result) {
            LoadGuestImage(result.GuestId);
            ClearDetails();

            $("#<%=hfPaxInRegistrationId.ClientID %>").val(result.RegistrationId);
            $("#<%=hfPaxInGuestId.ClientID %>").val(result.GuestId);

            if ($.trim(result.PaxInRate) > 0) {
                $("#PaxInRateInfoDiv").show();
            }
            else {
                $("#PaxInRateInfoDiv").hide();
            }

            if ($.trim(result.ArriveDate) != "") {
                $("#<%=lblDArrivalDate.ClientID %>").text(GetStringFromDateTime(result.ArriveDate));
            }

            if ($.trim(result.ExpectedCheckOutDate) != "") {
                $("#<%=lblDExpectedDepartureDate.ClientID %>").text(GetStringFromDateTime(result.ExpectedCheckOutDate));
            }

            if (result.RoomRate != "") {
                $("#<%=lblDRoomRate.ClientID %>").text(result.CurrencyTypeHead + " " + result.RoomRate);
            }
            else {
                $("#<%=lblDRoomRate.ClientID %>").text(result.CurrencyTypeHead + " 0.00");
            }

            if ($.trim(result.GuestDOB) != "") {

                $("#<%=lblDGuestDOB.ClientID %>").text(GetStringFromDateTime(result.GuestDOB));
            }
            if ($.trim(result.PIssueDate) != "") {

                $("#<%=lblDPIssueDate.ClientID %>").text(GetStringFromDateTime(result.PIssueDate));
            }
            if ($.trim(result.PExpireDate) != "") {

                $("#<%=lblDPExpireDate.ClientID %>").text(GetStringFromDateTime(result.PExpireDate));
            }
            if ($.trim(result.VIssueDate) != "") {

                $("#<%=lblDVIssueDate.ClientID %>").text(GetStringFromDateTime(result.VIssueDate));
            }
            if ($.trim(result.VExpireDate) != "") {
                $("#<%=lblDVExpireDate.ClientID %>").text(GetStringFromDateTime(result.VExpireDate));
            }

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
            $("#<%=lblDGuestProfession.ClientID %>").text(result.ProfessionName);
            $("#<%=lblDNationalId.ClientID %>").text(result.NationalId);
            $("#<%=lblDPassportNumber.ClientID %>").text(result.PassportNumber);
            $("#<%=lblDReferanceName.ClientID %>").text(result.ReferanceName);
            $("#<%=lblDRegistrationRemarks.ClientID %>").text(result.Remarks);
            $("#<%=lblDPIssuePlace.ClientID %>").text(result.PIssuePlace);
            $("#<%=lblDVisaNumber.ClientID %>").text(result.VisaNumber);
            $("#<%=lblDCountryName.ClientID %>").text(result.CountryName);
            $("#<%=txtPaxInRate.ClientID %>").val(result.PaxInRate);

            $("#PopMyTabs").tabs({ active: 0 });
            $("#TouchKeypad").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "", //TODO add title
                show: 'slide'
            });
            $("#popUpDiv").css('height', '657px');

            PageMethods.LoadGuestPreferences(result.GuestId, OnLoadGuestPreferencesSucceeded, OnLoadGuestPreferencesFailed);
        }
        function OnGetRegistrationInformationByRoomNumberFailed(error) {

        }
        // Room click functions
        function RoomVacantDirtyDiv(roomNumber) {
            pageTitle = "Vacant and Dirty Room Possible Path";
            PageMethods.LoadVacantDirtyPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }
        function RoomReservedDiv(roomNumber) {
            pageTitle = "Reserved Room Possible Path";
            PageMethods.LoadReservedPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }
        function RoomOccupaiedDiv(roomNumber) {
            pageTitle = "Occupied Room Possible Path";
            PageMethods.LoadOccupiedPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }
        function RoomTodaysCheckInDiv(roomNumber) {
            pageTitle = "Today's Check In Room Possible Path";
            PageMethods.LoadOccupiedPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }
        function RoomPossibleVacantDiv(roomNumber) {
            pageTitle = "Expected Departure Room Possible Path";
            PageMethods.LoadExpectedDeparturePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }
        function RoomOutOfOrderDiv(roomNumber) {
            pageTitle = "Out Of Order Room Possible Path";
            PageMethods.LoadOutOfOrderPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }
        function RoomOutOfServiceDiv(roomNumber) {
            pageTitle = "Out Of Service Room Possible Path";
            PageMethods.LoadOutOfServicePossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }
        function RoomVacantDiv(roomNumber) {
            pageTitle = "Vacant Room Possible Path";
            PageMethods.LoadVacantPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }
        function OnLoadOutOfOrderPossiblePathSucceeded(result) {
            $('#serviceDeciderHtml').html(result);

            $("#serviceDecider").dialog({
                autoOpen: true,
                modal: true,
                width: 600,
                closeOnEscape: true,
                resizable: false,
                title: pageTitle,
                show: 'slide'
            });
        }
        function OnShowOutOfServiceRoomInformationFailed(error) {
        }
        //Registration Details
        function CountTotalNumberOfGuestByRoomNumber(roomNumber) {
            $("#serviceDecider").dialog("close");
            PageMethods.CountTotalNumberOfGuestByRoomNumber(roomNumber, OnCountTotalNumberOfGuestByRoomNumberSucceeded, OnCountTotalNumberOfGuestByRoomNumberFailed);
        }
        function OnCountTotalNumberOfGuestByRoomNumberSucceeded(result) {
            if (result.NumberOfGuest == 1) {

                GetRegistrationInformationForSingleGuestByRoomNumber(result.RoomNumber);
            }
            else {
                GetRegistrationInformationByRoomNumber(result.RoomNumber);
            }
        }
        function OnCountTotalNumberOfGuestByRoomNumberFailed(error) {
        }
        function GetRegistrationInformationByRoomNumber(roomNumber) {
            PageMethods.GetRegistrationInformationListByRoomNumber(roomNumber, OnGetRegistrationInformationByRoomNumberSucceeded, OnGetRegistrationInformationByRoomNumberFailed);
        }
        function OnGetRegistrationInformationByRoomNumberSucceeded(result) {
            $('#GuestList').html(result);
            $("#GuestListingPopUp").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "", //TODO add title
                show: 'slide'
            });
            $("#popUpDiv").css('height', '657px');
            return false;
        }
        function OnGetRegistrationInformationByRoomNumberFailed(error) {
        }
        function GetRegistrationInformationForSingleGuestByRoomNumber(roomNumber) {
            PageMethods.GetRegistrationInformationForSingleGuestByRoomNumber(roomNumber, OnGetRegistrationInformationForSingleGuestByRoomNumberSucceeded, OnGetRegistrationInformationForSingleGuestByRoomNumberFailed);
        }
        function OnGetRegistrationInformationForSingleGuestByRoomNumberFailed(error) {
        }
        function OnGetRegistrationInformationForSingleGuestByRoomNumberSucceeded(result) {
            PageMethods.LoadGuestAirportDrop(result.RegistrationId, OnLoadAirportDropSucceeded, OnLoadAirportDropFailed);
            ClearDetails();
            LoadGuestImage(result.GuestId);
            if ($.trim(result.PaxInRate) > 0) {
                $("#PaxInRateInfoDiv").show();
            }
            else {
                $("#PaxInRateInfoDiv").hide();
            }
            if ($.trim(result.ArriveDate) != "") {
                $("#<%=lblDArrivalDate.ClientID %>").text(GetStringFromDateTime(result.ArriveDate));
        }
        if ($.trim(result.ExpectedCheckOutDate) != "") {
            $("#<%=lblDExpectedDepartureDate.ClientID %>").text(GetStringFromDateTime(result.ExpectedCheckOutDate));
        }
        if (result.RoomRate != "") {
            $("#<%=lblDRoomRate.ClientID %>").text(result.CurrencyTypeHead + " " + result.RoomRate);
        }
        else {
            $("#<%=lblDRoomRate.ClientID %>").text(result.CurrencyTypeHead + " 0.00");
        }
        if ($.trim(result.GuestDOB) != "") {

            $("#<%=lblDGuestDOB.ClientID %>").text(GetStringFromDateTime(result.GuestDOB));
        }
        if ($.trim(result.PIssueDate) != "") {

            $("#<%=lblDPIssueDate.ClientID %>").text(GetStringFromDateTime(result.PIssueDate));
        }
        if ($.trim(result.PExpireDate) != "") {

            $("#<%=lblDPExpireDate.ClientID %>").text(GetStringFromDateTime(result.PExpireDate));
        }
        if ($.trim(result.VIssueDate) != "") {

            $("#<%=lblDVIssueDate.ClientID %>").text(GetStringFromDateTime(result.VIssueDate));
        }
        if ($.trim(result.VExpireDate) != "") {
            $("#<%=lblDVExpireDate.ClientID %>").text(GetStringFromDateTime(result.VExpireDate));
        }

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
            $("#<%=lblDGuestProfession.ClientID %>").text(result.ProfessionName);
            $("#<%=lblDNationalId.ClientID %>").text(result.NationalId);
            $("#<%=lblDPassportNumber.ClientID %>").text(result.PassportNumber);
            $("#<%=lblDReferanceName.ClientID %>").text(result.ReferanceName);
            $("#<%=lblDRegistrationRemarks.ClientID %>").text(result.Remarks);
            $("#<%=lblDPIssuePlace.ClientID %>").text(result.PIssuePlace);
            $("#<%=lblDVisaNumber.ClientID %>").text(result.VisaNumber);
            $("#<%=lblDCountryName.ClientID %>").text(result.CountryName);
            $("#PopMyTabs").tabs({ active: 0 });
            $("#TouchKeypad").dialog({
                width: 950,
                height: 600,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", //TODO add title
                show: 'slide'
            });
            $("#popUpDiv").css('height', '657px');

            PageMethods.LoadGuestPreferences(result.GuestId, OnLoadGuestPreferencesSucceeded, OnLoadGuestPreferencesFailed);
            return false;
        }
        function OnLoadAirportDropSucceeded(result) {
            $("#<%=lblDAirlineName.ClientID %>").text(result.FlightName);
            $("#<%=lblDFlightNumber.ClientID %>").text(result.FlightNumber);
            $("#<%=lblDDepartureTime.ClientID %>").text(result.TimeString);
            return false;
        }
        function OnLoadAirportDropFailed(error) {
            toastr.error(error.get_message());
        }
        function OnLoadGuestPreferencesSucceeded(result) {
            $("#Preference").html(result);
        }
        function OnLoadGuestPreferencesFailed(error) {
        }
        function ClearDetails() {
            pageTitle = '';

            $("#<%=lblDGuestDOB.ClientID %>").text("");
            $("#<%=lblDPIssueDate.ClientID %>").text("");
            $("#<%=lblDPExpireDate.ClientID %>").text("");
            $("#<%=lblDVIssueDate.ClientID %>").text("");
            $("#<%=lblDVExpireDate.ClientID %>").text("");
            $("#<%=lblDGuestName.ClientID %>").text("");
            $("#<%=lblDGuestSex.ClientID %>").text("");
            $("#<%=lblDGuestEmail.ClientID %>").text("");
            $("#<%=lblDGuestPhone.ClientID %>").text("");
            $("#<%=lblDGuestAddress1.ClientID %>").text("");
            $("#<%=lblDGuestAddress2.ClientID %>").text("");
            $("#<%=lblDGuestCity.ClientID %>").text("");
            $("#<%=lblDGuestZipCode.ClientID %>").text("");
            $("#<%=lblDGuestNationality.ClientID %>").text("");
            $("#<%=lblDGuestDrivinlgLicense.ClientID %>").text("");
            $("#<%=lblDGuestProfession.ClientID %>").text("");
            $("#<%=lblDNationalId.ClientID %>").text("");
            $("#<%=lblDPassportNumber.ClientID %>").text("");
            $("#<%=lblDPIssuePlace.ClientID %>").text("");
            $("#<%=lblDVisaNumber.ClientID %>").text("");
            $("#<%=lblDCountryName.ClientID %>").text("");
            $("#<%=lblDReferanceName.ClientID %>").text("");
            $("#<%=lblDRegistrationRemarks.ClientID %>").text("");
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
        function LoadBillPreview() {
            $("#serviceDecider").dialog('close');
            $('#btnBillPreview').trigger('click');
        }
        // Reservation Details
        function LoadReservationDetails(reservationId) {
            GetReservationInformation(reservationId);
            GetReservationGuestInformation(reservationId);
            PopUpReservationInformation();
        }
        function PopUpReservationInformation() {
            $("#PopReservation").tabs({ active: 0 });
            $("#ReservationPopUp").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "", //TODO add title
                show: 'slide'
            });
            $("#popUpDiv").css('height', '496px');
            return false;
        }
        //Get Reservation Information
        function GetReservationInformation(ReservationId) {
            PageMethods.GetReservationformationByReservationId(ReservationId, GetReservationInformationSucceeded, OnOperationFailed);
            return false;
        }
        function GetReservationGuestInformation(ReservationId) {
            PageMethods.GetReservationGuestInformationByReservationId(ReservationId, GetReservationGuestInformationSucceeded, OnOperationFailed);
            return false;
        }
        function GetReservationInformationSucceeded(result) {
            $("#ReservationInfo").html(result)
        }
        function GetReservationGuestInformationSucceeded(result) {
            $("#ReservationGuestInfo").html(result)
        }
        function OnOperationFailed() { }
        // Out of order details
        function ShowOutOfServiceRoomInformation(roomNumber) {
            PageMethods.ShowOutOfServiceRoomInformation(roomNumber, OnShowOutOfServiceRoomInformationSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }
        function OnShowOutOfServiceRoomInformationSucceeded(result) {
            $("#<%=lblDRemarks.ClientID %>").text(":   " + result.Remarks);
            $("#<%=lblDFromDate.ClientID %>").text(":   " + GetStringFromDateTime(result.FromDate));
            $("#<%=lblDToDate.ClientID %>").text(":   " + GetStringFromDateTime(result.ToDate));
            $("#<%=lblDRoomStatus.ClientID %>").text(":   " + result.StatusName);
            $("#<%=lblDRoomType.ClientID %>").text(":   " + result.RoomType);
            $("#<%=lblDRoomNumber.ClientID %>").text(":   " + result.RoomNumber);
            $("#OutOfServicePopUp").dialog({
                autoOpen: true,
                modal: true,
                width: 600,
                closeOnEscape: true,
                resizable: false,
                title: pageTitle,
                show: 'Out of Order'
            });
        }
        function OnShowOutOfServiceRoomInformationFailed(error) {
        }

        function PopUpStatisticsReportInfo(reportType) {
            var url = "";
            var popup_window = "Room Statistics Report";
            url = "/HotelManagement/Reports/frmRoomStatisticsReport.aspx?ReportType=" + reportType;
            window.open(url, popup_window, "width=1100,height=680,left=130,top=5,resizable=yes");
        }

        function LoadCleanUpInfo(roomId) {
            PageMethods.GetRoomCleanUpInfo(roomId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            var guest = eval(result);
            var today = new Date();
            var time = today.getHours() + ":" + today.getMinutes();
            

            $("#<%=hfRoomId.ClientID %>").val(guest.RoomId);
            $("#<%=txtLastCleanDate.ClientID %>").val(guest.LastCleanDate);
            $("#<%=txtRoomNumber.ClientID %>").val(guest.RoomNumber);
            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
            $("#<%=txtProbableCleanTime.ClientID %>").val(time);
            PopulateddlCleanStatusControl(guest.StatusId, guest.CleanupStatus, $("#<%=ddlCleanStatus.ClientID %>"));
            $("#<%=txtRoomNumber.ClientID %>").attr('readonly', true);
            $("#<%=txtApprovedDate.ClientID %>").attr('readonly', true);
            var ddlCleanStatus = '<%=ddlCleanStatus.ClientID%>'
            var txtRemarks = '<%=txtRemarks.ClientID%>'
            if ($('#' + ddlCleanStatus).val() == "Cleaned") {
                $('#' + txtRemarks).val("");
            }

            $("#RoomCleanUpInfo").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: pageTitle,
                show: 'Room CleanUp'
            });
            $('#OutOfServiceDivInfo').hide("slow");
            $('#WithoutOutOfServiceDivInfo').show("slow");
        }
        function OnFillFormObjectFailed(error) {
            toastr.error(error);
        }
        //-- Common Populate Control -----------------------------
        function PopulateddlCleanStatusControl(statusId, cleanupStatus, control) {
            control.empty();
            if (statusId == 1) {
                if (cleanupStatus == "Dirty") {
                    control.append($("<option></option>").val("Cleaned").html("Cleaned"));
                }
                else {
                    control.append($("<option></option>").val("Cleaned").html("Cleaned"));
                    control.append($("<option></option>").val("Dirty").html("Dirty"));
                }
            }
            else if (statusId == 2) {
                control.append($("<option></option>").val("Cleaned").html("Cleaned"));
            }
        }
        function ChangeRoomStatus() {
            var roomId = $("#<%=hfRoomId.ClientID %>").val();
            var cleanupStatus = $("#<%=ddlCleanStatus.ClientID %>").val();
            var remarks = $("#<%=txtRemarks.ClientID %>").val();
            var cleanDate = $("#<%=txtApprovedDate.ClientID %>").val();
            var fromDate = $("#<%=txtFromDate.ClientID %>").val();
            var toDate = $("#<%=txtToDate.ClientID %>").val();
            var lastCleanDate = $("#<%=txtLastCleanDate.ClientID %>").val();
            var cleanTime = $("#<%=txtProbableCleanTime.ClientID %>").val();

            if (cleanupStatus == "OutOfOrder") {
                if (toDate == "") {
                    toastr.warning("Please Provide To Date");
                    return;
                }
            }

            PageMethods.ChangeRoomStatus(roomId, cleanupStatus, remarks, fromDate, toDate, cleanDate, cleanTime, lastCleanDate, OnChangeRoomStatusSucceeded, OnChangeRoomStatusFailed);
            return false;
        }
        function OnChangeRoomStatusSucceeded(result) {
            $("#<%=hfRoomId.ClientID %>").val("");
            $("#<%=txtRemarks.ClientID %>").val("");
            CommonHelper.AlertMessage(result.AlertMessage);
            $("#RoomCleanUpInfo").dialog("close");
            $("#serviceDecider").dialog("close");
            window.location = "frmInnboardDashboard.aspx"
        }
        function OnChangeRoomStatusFailed(error) {
        }
        function ClosePopUP() {
            //popup(-1); //TODO close popup
        }
        //Dashboard fuctions
        function SaveDashboardDesign() {
            var items1 = [];
            var items2 = [];
            $("#column1 ol li").each(function () {
                var item = $(this).find("div:first").attr("id");
                items1.push(item);
            });

            $("#column2 ol li").each(function () {
                var item = $(this).find("div:first").attr("id");
                items2.push(item);
            });

            $.ajax({
                url: '/HMCommon/frmInnboardDashboard.aspx/SaveManagement',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ column1: items1, column2: items2 }),
                success: function (result) {
                }
            });
        }
        function OnManagementSuccess(result) {
            if (result.length > 0) {
                LoadManagement(result);
            }
            else {
                GenerateManagement();
            }
        }
        function OnManagementFail(error) {
        }
        function GenerateManagement() {
            PageMethods.GenerateManagement(OnSucceeded, OnFailed);
        }
        function OnSucceeded(result) {
            var m = new Array(), mm = new Array();

            for (i = 0; i < result.length; i++) {
                if (result[i].Panel == 1) {
                    m.push("<li> <div id = div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "></div></li><br>");
                }
                else {
                    mm.push("<li> <div id = div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "></div></li><br>");
                }
            }

            $("#a").append(m);
            $("#b").append(mm);

            for (i = 0; i < result.length; i++) {
                if (result[i].ItemId == 1) {
                    div1 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.LoadDepartmentWiseLeaveBalance(OnLoadLeaveBalanceSucceeded, OnLoadLeaveBalanceFailed);
                }
                else if (result[i].ItemId == 2) {
                    div2 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.LoadEmpTypeWiseEmpNo(OnLoadEmpTypeSucceeded, OnLoadEmpTypeFailed);
                }
                else if (result[i].ItemId == 3) {
                    div3 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.LoadTrainingList(OnLoadTrainingListSucceeded, OnLoadTrainingListFailed);
                }
                else if (result[i].ItemId == 4) {
                    div4 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.LoadLeaveBalance(OnLoadLeaveBalanceListSucceeded, OnLoadLeaveBalanceListFailed);
                }
                else if (result[i].ItemId == 5) {
                    div5 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.TodaysRoomStatus(OnLoadRoomStatusSucceeded, OnLoadRoomStatusFailed);
                }
                else if (result[i].ItemId == 6) {
                    div6 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.ExpectedArrivalList(OnLoadArrivalListSucceeded, OnLoadArrivalListFailed);
                }
                else if (result[i].ItemId == 7) {
                    div7 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.ExpectedDepartureList(OnLoadDepartureListSucceeded, OnLoadDepartureListFailed);
                }
                else if (result[i].ItemId == 8) {
                    div8 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.CheckedInList(OnLoadCheckedInListSucceeded, OnLoadCheckedInListFailed);
                }
                else if (result[i].ItemId == 9) {
                    div9 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.CheckedOutList(OnLoadCheckedOutListSucceeded, OnLoadCheckedOutListFailed);
                }
                else if (result[i].ItemId == 10) {
                    div10 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.InHouseGuestList(OnLoadInHouseGuestListSucceeded, OnLoadInHouseGuestListFailed);
                }
                else if (result[i].ItemId == 11) {
                    div11 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.AirportPickUpDropList(OnLoadAirportPickUpDropSucceeded, OnLoadAirportPickUpDropFailed);
                }
                else if (result[i].ItemId == 12) {
                    div12 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.VIPGuestList(OnLoadVIPGuestListSucceeded, OnLoadVIPGuestListFailed);
                }
                else if (result[i].ItemId == 13) {
                    div13 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.DailyStatisticalReport(OnLoadDailyStatisticalReportSucceeded, OnLoadDailyStatisticalReportFailed);
                }
                else if (result[i].ItemId == 14) {
                    div14 = "div" + result[i].Panel + result[i].Div + "S" + result[i].ItemId + "";
                    PageMethods.DailyStatisticalPieChartReport(OnLoadDailyStatisticalPieChartSucceeded, OnLoadDailyStatisticalPieChartFailed);
                }
            }
        }
        function OnFailed() {
        }
        function LoadManagement(result) {
            var m = [], mm = [];

            for (i = 0; i < result.length; i++) {
                if (result[i].Panel == 1) {
                    m.push("<li> <div id =" + result[i].DivName + "></div></li><br>");
                }
                else {
                    mm.push("<li> <div id =" + result[i].DivName + "></div></li><br>");
                }
            }

            $("#a").append(m);
            $("#b").append(mm);

            for (i = 0; i < result.length; i++) {
                if (result[i].ItemId == 1) {
                    div1 = result[i].DivName;
                    PageMethods.LoadDepartmentWiseLeaveBalance(OnLoadLeaveBalanceSucceeded, OnLoadLeaveBalanceFailed);
                }
                else if (result[i].ItemId == 2) {
                    div2 = result[i].DivName;
                    PageMethods.LoadEmpTypeWiseEmpNo(OnLoadEmpTypeSucceeded, OnLoadEmpTypeFailed);
                }
                else if (result[i].ItemId == 3) {
                    div3 = result[i].DivName;
                    PageMethods.LoadTrainingList(OnLoadTrainingListSucceeded, OnLoadTrainingListFailed);
                }
                else if (result[i].ItemId == 4) {
                    div4 = result[i].DivName;
                    PageMethods.LoadLeaveBalance(OnLoadLeaveBalanceListSucceeded, OnLoadLeaveBalanceListFailed);
                }
                else if (result[i].ItemId == 5) {
                    div5 = result[i].DivName;
                    PageMethods.TodaysRoomStatus(OnLoadRoomStatusSucceeded, OnLoadRoomStatusFailed);
                }
                else if (result[i].ItemId == 6) {
                    div6 = result[i].DivName;
                    PageMethods.ExpectedArrivalList(OnLoadArrivalListSucceeded, OnLoadArrivalListFailed);
                }
                else if (result[i].ItemId == 7) {
                    div7 = result[i].DivName;
                    PageMethods.ExpectedDepartureList(OnLoadDepartureListSucceeded, OnLoadDepartureListFailed);
                }
                else if (result[i].ItemId == 8) {
                    div8 = result[i].DivName;
                    PageMethods.CheckedInList(OnLoadCheckedInListSucceeded, OnLoadCheckedInListFailed);
                }
                else if (result[i].ItemId == 9) {
                    div9 = result[i].DivName;
                    PageMethods.CheckedOutList(OnLoadCheckedOutListSucceeded, OnLoadCheckedOutListFailed);
                }
                else if (result[i].ItemId == 10) {
                    div10 = result[i].DivName;
                    PageMethods.InHouseGuestList(OnLoadInHouseGuestListSucceeded, OnLoadInHouseGuestListFailed);
                }
                else if (result[i].ItemId == 11) {
                    div11 = result[i].DivName;
                    PageMethods.AirportPickUpDropList(OnLoadAirportPickUpDropSucceeded, OnLoadAirportPickUpDropFailed);
                }
                else if (result[i].ItemId == 12) {
                    div12 = result[i].DivName;
                    PageMethods.VIPGuestList(OnLoadVIPGuestListSucceeded, OnLoadVIPGuestListFailed);
                }
                else if (result[i].ItemId == 13) {
                    div13 = result[i].DivName;
                    PageMethods.DailyStatisticalReport(OnLoadDailyStatisticalReportSucceeded, OnLoadDailyStatisticalReportFailed);
                }
                else if (result[i].ItemId == 14) {
                    div14 = result[i].DivName;
                    PageMethods.DailyStatisticalPieChartReport(OnLoadDailyStatisticalPieChartSucceeded, OnLoadDailyStatisticalPieChartFailed);
                }
            }
        }
        /*Leave Balance Section*/
        function OnLoadLeaveBalanceSucceeded(data) {
            vv = data;
            var xAxisCategories = _.pluck(data, "LeaveTypeName");
            xAxisCategories = _.uniq(xAxisCategories);

            var dataSeries = [];
            dataSeries.push({
                name: 'TotalLeave',
                data: _.pluck(vv, "TotalLeave")
            });

            dataSeries.push({
                name: 'Remaining',
                data: _.pluck(vv, "RemainingLeave")
            });

            dataSeries.push({
                name: 'Taken',
                data: _.pluck(vv, "TotalTakenLeave")
            });

            $("#" + div1 + "").highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: ' '
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories: xAxisCategories,
                    title: {
                        text: null
                    },
                    labels: {
                        rotation: -45,
                        style: {
                            fontSize: '11px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                },

                yAxis: {
                    min: 1,
                    max: 200,
                    tickInterval: 1,
                    title: {
                        text: 'Leave (Days)',
                        align: 'high'
                    },
                    labels: {
                        rotation: 0,
                        overflow: 'justify',
                        style: {
                            fontSize: '9px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                },
                tooltip: {
                    valueSuffix: ' days',
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                            '<td style="padding:0"><b>{point.y:.1f} Days</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true,
                            crop: false
                        }
                    },
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        crop: false
                    }
                },
                legend: {
                    layout: 'horzintal',
                    align: 'right',
                    verticalAlign: 'top',
                    x: -40,
                    y: 1,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                    shadow: true
                },
                credits: {
                    enabled: false
                },
                series: dataSeries
            });
        }
        function OnLoadLeaveBalanceFailed() {
        }

        /* Emp Type Pie Chart*/
        function OnLoadEmpTypeSucceeded(data) {
            v = data;

            var dataSeries = [];

            for (var i = 0; i < v.length; i++) {
                dataSeries.push({
                    name: v[i].TypeName,
                    y: v[i].NoOfEmp
                });
            }

            $("#" + div2 + "").highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: ' '
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    name: "Brands",
                    colorByPoint: true,
                    data: dataSeries
                }]
            });
        }
        function OnLoadEmpTypeFailed() {
        }

        /* Emp Type Pie Chart*/
        function OnLoadDailyStatisticalPieChartSucceeded(data) {
            v = data;

            var dataSeries = [];

            for (var i = 0; i < v.length; i++) {
                dataSeries.push({
                    name: v[i].Details,
                    y: v[i].TotalNo
                });
            }

            $("#" + div14 + "").highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: ' '
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    name: "Brands",
                    colorByPoint: true,
                    data: dataSeries
                }]
            });
        }
        function OnLoadDailyStatisticalPieChartFailed() {
        }

        /*Training List*/
        function OnLoadTrainingListSucceeded(result) {
            $("#" + div3 + "").append(result);
        }

        function OnLoadTrainingListFailed() {
        }

        /*Leave Balance*/
        function OnLoadLeaveBalanceListSucceeded(result) {
            $("#" + div4 + "").append(result);
        }

        function OnLoadLeaveBalanceListFailed() {
        }

        /*Todays Room Status*/
        function OnLoadRoomStatusSucceeded(result) {
            $("#" + div5 + "").append(result);
        }

        function OnLoadRoomStatusFailed() {
        }

        /*Expected Arrival List*/
        function OnLoadArrivalListSucceeded(result) {
            $("#" + div6 + "").append(result);
        }

        function OnLoadArrivalListFailed() {
        }

        /*Expected Departure List*/
        function OnLoadDepartureListSucceeded(result) {
            $("#" + div7 + "").append(result);
        }

        function OnLoadDepartureListFailed() {
        }

        /*Checked In List*/
        function OnLoadCheckedInListSucceeded(result) {
            $("#" + div8 + "").append(result);
        }

        function OnLoadCheckedInListFailed() {
        }

        /*Checked Out List*/
        function OnLoadCheckedOutListSucceeded(result) {
            $("#" + div9 + "").append(result);
        }

        function OnLoadCheckedOutListFailed() {
        }

        /*In House Guest list*/
        function OnLoadInHouseGuestListSucceeded(result) {
            $("#" + div10 + "").append(result);
        }

        function OnLoadInHouseGuestListFailed() {
        }

        /*Airport PickUp/Drop list*/
        function OnLoadAirportPickUpDropSucceeded(result) {
            $("#" + div11 + "").append(result);
        }

        function OnLoadAirportPickUpDropFailed() {
        }

        /*VIP Guest list*/
        function OnLoadVIPGuestListSucceeded(result) {
            $("#" + div12 + "").append(result);
        }

        function OnLoadVIPGuestListFailed() {
        }

        /*Daily Statistical Report*/
        function OnLoadDailyStatisticalReportSucceeded(result) {
            $("#" + div13 + "").append(result);
        }
        function OnLoadDailyStatisticalReportFailed() {
        }

    </script>
    <style>
        .row.display-flex {
            display: flex;
            flex-wrap: wrap;
        }

            .row.display-flex > [class*='col-'] {
                display: flex;
                flex-direction: column;
            }
    </style>
    <div style="display: none;">
        <asp:Button ID="btnBillPreview" runat="server" ClientIDMode="Static" Text="Bill Preview"
            OnClick="btnBillPreview_Click" />
    </div>
    <%--<asp:Panel ID="pnlMenuSearchRoomSearchRoomStatisticsInfo" runat="server">
        <div id="RoomStatusJustified">
            <div class="btn-group btn-group-justified" role="group" aria-label="...">
                <div class="btn-group text-center" role="group" onclick="PopUpStatisticsReportInfo('ExpectedArrival');" style="cursor: pointer;">
                    <div class="thumbnail">
                        <asp:Label ID="lblExpectedArrival" runat="server" class="StatisticsQuantity" Text=""></asp:Label>
                        <div class="caption">
                            Expected Arrival
                        </div>
                    </div>
                </div>
                <div class="btn-group text-center" role="group" onclick="PopUpStatisticsReportInfo('ExpectedDeparture');" style="cursor: pointer;">
                    <div class="thumbnail">
                        <asp:Label ID="lblExpectedDeparture" runat="server" class="StatisticsQuantity" Text=""></asp:Label>
                        <div class="caption">
                            Expected Departure
                        </div>
                    </div>
                </div>
                <div class="btn-group text-center" role="group" onclick="PopUpStatisticsReportInfo('CheckInRoom');" style="cursor: pointer;">
                    <div class="thumbnail">
                        <asp:Label ID="lblCheckInRoom" runat="server" class="StatisticsQuantity" Text=""></asp:Label>
                        <div class="caption">
                            Check in Room
                        </div>
                    </div>
                </div>
                <div class="btn-group text-center" role="group">
                    <div class="thumbnail">
                        <asp:Label ID="lblWalkInRoom" runat="server" class="StatisticsQuantity" Text=""></asp:Label>
                        <div class="caption">
                            Walk in Room
                        </div>
                    </div>
                </div>
                <div class="btn-group text-center" role="group">
                    <div class="thumbnail">
                        <asp:Label ID="lblRoomToSell" runat="server" class="StatisticsQuantity" Text=""></asp:Label>
                        <div class="caption">
                            Room to Sell
                        </div>
                    </div>
                </div>
                <div class="btn-group text-center" role="group">
                    <div class="thumbnail">
                        <asp:Label ID="lblRegisterComplaint" runat="server" class="StatisticsQuantity" Text=""></asp:Label>
                        <div class="caption">
                            Register Complaint
                        </div>
                    </div>
                </div>
                <div class="btn-group text-center" role="group" onclick="PopUpStatisticsReportInfo('InhouseRoomGuest');" style="cursor: pointer;">
                    <div class="thumbnail">
                        <asp:Label ID="lblInhouseRoomOrGuest" runat="server" class="StatisticsQuantity" Text=""></asp:Label>
                        <div class="caption">
                            Inhouse Room / Guest
                        </div>
                    </div>
                </div>
                <div class="btn-group text-center" role="group">
                    <div class="thumbnail">
                        <asp:Label ID="lblExtraAdultsOrChild" runat="server" class="StatisticsQuantity" Text=""></asp:Label>
                        <div class="caption">
                            Extra Adult / Child
                        </div>
                    </div>
                </div>
                <div class="btn-group text-center" role="group" onclick="PopUpStatisticsReportInfo('InhouseForeigner');" style="cursor: pointer;">
                    <div class="thumbnail">
                        <asp:Label ID="lblInhouseForeigners" runat="server" class="StatisticsQuantity" Text=""></asp:Label>
                        <div class="caption">
                            Inhouse Foreigner
                        </div>
                    </div>
                </div>
                <div class="btn-group text-center" role="group">
                    <div class="thumbnail">
                        <asp:Label ID="lblGuestBlock" runat="server" class="StatisticsQuantity" Text=""></asp:Label>
                        <div class="caption">
                            Guest Block
                        </div>
                    </div>
                </div>
                <div class="btn-group text-center" role="group" onclick="PopUpStatisticsReportInfo('AirportPickupDrop');" style="cursor: pointer;">
                    <div class="thumbnail">
                        <asp:Label ID="lblAirportPickupDrop" runat="server" class="StatisticsQuantity" Text=""></asp:Label>
                        <div class="caption">
                            Airport Pickup/ Drop
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>--%>
    <div id="TouchKeypad" style="display: none;">
        <div id="PopTabPanel" style="width: 900px">
            <div id="PopMyTabs">
                <ul id="PoptabPage"
                    class="ui-style">
                    <li id="PopA" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a href="#Poptab-1">Registration Information</a></li>
                    <li
                        id="PopB" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-2">Registration Document</a></li>
                    <li id="PopC" runat="server"
                        style="border: 1px solid #AAAAAA; border-bottom: none"><a href="#Poptab-3">Preferences</a></li>
                    <li id="PopD" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-4">Airport Drop Information</a></li>
                </ul>
                <div id="Poptab-1">
                    <div id="GuestInfo" class="panel panel-default" style="font-weight: bold">
                        <div
                            class="panel-heading">
                            Registration Information
                        </div>
                        <div class="panel-body">
                            <div
                                class="form-horizontal">
                                <table class="table table-striped table-bordered table-condensed
        table-hover">
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLArrivalDate" runat="server"
                                                Text="Arrival Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDArrivalDate"
                                                runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLExpectedDepartureDate"
                                                runat="server" Text="Expected Departure Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDExpectedDepartureDate" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLRoomRate" runat="server" Text="Room
        Tariff"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDRoomRate" runat="server"
                                                Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLCountryName" runat="server"
                                                Text="Country Name"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDCountryName"
                                                runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label
                                                ID="lblLGuestName" runat="server" Text="Name"></asp:Label>
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
                                            <asp:Label ID="lblLGuestSex" runat="server"
                                                Text="Gender"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestSex"
                                                runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestEmail"
                                                runat="server" Text="Email"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestEmail"
                                                runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label
                                                ID="lblLGuestPhone" runat="server" Text="Phone Number"></asp:Label>
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
                                            <asp:Label ID="lblLGuestAddress2" runat="server"
                                                Text="Address"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestAddress2"
                                                runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestCity"
                                                runat="server" Text="City "></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestCity"
                                                runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label
                                                ID="lblLGuestZipCode" runat="server" Text="Zip Code"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestZipCode" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestNationality" runat="server" Text="Nationality"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestNationality" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLGuestDrivinlgLicense" runat="server"
                                                Text="Driving License No"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDGuestDrivinlgLicense"
                                                runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblGuestProfession"
                                                runat="server" Text="Profession"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label
                                                ID="lblDGuestProfession" runat="server"></asp:Label>
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
                                            <asp:Label ID="lblDPassportNumber"
                                                runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label
                                                ID="lblLPIssueDate" runat="server" Text="Pasport Issue Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPIssueDate" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLPIssuePlace" runat="server" Text="Passport
        Issue Place"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPIssuePlace"
                                                runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label
                                                ID="lblLPExpireDate" runat="server" Text="Passport Expiry Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDPExpireDate" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2">
                                            <asp:Label ID="lblLVisaNumber" runat="server" Text="Visa
        Number"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDVisaNumber" runat="server"
                                                Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblLVIssueDate"
                                                runat="server" Text="Visa Issue Date"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label
                                                ID="lblDVIssueDate" runat="server" Text=""></asp:Label>
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
                                            <asp:Label ID="lblVReferanceName" runat="server"
                                                Text="Referance"></asp:Label>
                                        </td>
                                        <td class="span4">
                                            <asp:Label ID="lblDReferanceName"
                                                runat="server" Text=""></asp:Label>
                                        </td>
                                        <td class="span2"></td>
                                        <td class="span4"></td>
                                    </tr>
                                    <tr>
                                        <td class="span2">
                                            <asp:Label ID="lblVRegistrationRemarks" runat="server"
                                                Text="Remarks"></asp:Label>
                                        </td>
                                        <td class="span4" colspan="3">
                                            <asp:Label ID="lblDRegistrationRemarks"
                                                runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div id="PaxInRateInfoDiv"
                                    class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblPaxInRate" runat="server"
                                            class="control-label" Text="Pax In Rate"></asp:Label>
                                    </div>
                                    <div class="col-md-2"
                                        style="padding-right: 0;">
                                        <asp:HiddenField ID="hfPaxInRegistrationId" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hfPaxInGuestId" runat="server"></asp:HiddenField>
                                        <asp:TextBox
                                            ID="txtPaxInRate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2" style="padding-left: 5px;">
                                        <asp:Button ID="btnPaxInRateUpdate"
                                            runat="server" TabIndex="4" Text="Update" CssClass="TransactionalButton btn btn-primary
        btn-sm"
                                            OnClick="btnPaxInRateUpdate_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Poptab-2">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <div id="imageDiv"></div>
                        </div>
                    </div>
                </div>
                <div id="Poptab-3">
                    <div id="Div3"
                        class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Preferences
                        </div>
                        <div id="Preference"></div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="Poptab-4">
                    <div id="Div2" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">Drop Information</div>
                        <div class="HMBodyContainer">
                            <table class="table table-striped table-bordered table-condensed table-hover">
                                <tr>
                                    <td class="span2">
                                        <asp:Label ID="lblAirlineName" runat="server" Text="Airline Name"></asp:Label>
                                    </td>
                                    <td class="span4" colspan="3">
                                        <asp:Label ID="lblDAirlineName" runat="server"
                                            Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="span2">
                                        <asp:Label ID="lblFlightNumber"
                                            runat="server" Text="Flight Number"></asp:Label>
                                    </td>
                                    <td class="span4">
                                        <asp:Label
                                            ID="lblDFlightNumber" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td class="span2">
                                        <asp:Label ID="lblDepartureTime" runat="server" Text="Departure Time"></asp:Label>
                                    </td>
                                    <td class="span4">
                                        <asp:Label ID="lblDDepartureTime" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="divClear"></div>
                        </div>
                    </div>
                </div>
                <div class="row"
                    style="margin-top: -20px;">
                    <div class="col-md-2"></div>
                    <div class="col-md-4"
                        style="margin-top: 10px; margin-right: 15px; text-align: right; float: right;">
                        <input type="button" value="Back" class="btn btn-primary btn-sm" id="btnBackToDecisionMaker" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="GuestListingPopUp" style="display: none;">
        <div id="GuestList">
        </div>
    </div>
    <div id="serviceDecider" style="display: none;">
        <div id="serviceDeciderHtml">
        </div>
    </div>
    <div id="OutOfServicePopUp" style="display: none;" class="panel-body">
        <div id="OutOfServiceDetails" class="form-horizontal">
            <div class="form-group">
                <div class="col-md-3">
                    <asp:Label ID="lblLRoomType" runat="server" class="control-label" Text="Room Type"></asp:Label>
                </div>
                <div class="col-md-6">
                    <asp:Label ID="lblDRoomType" class="control-label" runat="server"></asp:Label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-3">
                    <asp:Label ID="lblLRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                </div>
                <div class="col-md-6">
                    <asp:Label ID="lblDRoomNumber" class="control-label" runat="server"></asp:Label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-3">
                    <asp:Label ID="lblLRoomStatus" runat="server" class="control-label" Text="Room Status"></asp:Label>
                </div>
                <div class="col-md-6">
                    <asp:Label ID="lblDRoomStatus" class="control-label" runat="server"></asp:Label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-3">
                    <asp:Label ID="lblLFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                </div>
                <div class="col-md-6">
                    <asp:Label ID="lblDFromDate" class="control-label" runat="server"></asp:Label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-3">
                    <asp:Label ID="lblLToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                </div>
                <div class="col-md-6">
                    <asp:Label ID="lblDToDate" class="control-label" runat="server"></asp:Label>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-3">
                    <asp:Label ID="lblLRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                </div>
                <div class="col-md-6">
                    <asp:Label ID="lblDRemarks" class="control-label" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <div id="ReservationPopUp" style="display: none;">
        <div id="Div1">
            <div id="PopReservation">
                <ul id="Ul1" class="ui-style">
                    <li id="Li1" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                        href="#PopupReservationInfo">Reservation Information</a></li>
                    <li id="Li2" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                        href="#PopupReservationGuestInfo">Guest Information</a></li>
                </ul>
                <div id="PopupReservationInfo">
                    <div id="ReservationDetails" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Reservation Information
                        </div>
                        <div id="ReservationInfo">
                        </div>
                    </div>
                </div>
                <div id="PopupReservationGuestInfo">
                    <div id="ReservationGuest" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Guest Information
                        </div>
                        <div id="ReservationGuestInfo">
                        </div>
                    </div>
                </div>
                <div class="row" style="padding-top: 5px; display: none;">
                    <button type="button" id="btnReservationBack" onclick="javascript:return ClosePopUP()"
                        class="TransactionalButton btn btn-primary">
                        Back</button>
                </div>
            </div>
        </div>
    </div>
    <div id="RoomCleanUpInfo" style="display: none;" class="panel-body">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <asp:HiddenField ID="hfRoomId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="txtLastCleanDate" runat="server"></asp:HiddenField>
                    <asp:Label ID="lblRoomNumber" runat="server" class="control-label" Text="Room Number"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtRoomNumber" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblCleanStatus" runat="server" class="control-label" Text="Clean Status"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlCleanStatus" CssClass="form-control" runat="server">
                        <asp:ListItem Value="Cleaned">Cleaned</asp:ListItem>
                        <asp:ListItem Value="Dirty">Dirty</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div id="OutOfServiceDivInfo" style="display: none;">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div id="WithoutOutOfServiceDivInfo">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblApprovedDate" runat="server" class="control-label" Text="Clean Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtApprovedDate" runat="server" CssClass="form-control" Enabled="false"
                            TabIndex="4"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblProbableInTime" runat="server" class="control-label" Text="Clean Time"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtProbableCleanTime" Width="80" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblCleanRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript:return ChangeRoomStatus()" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div id="column1" class="col-md-6" style="padding-right: 4px;">
            <ol id="a" class="example">
            </ol>
        </div>
        <div id="column2" class="col-md-6" style="padding-left: 4px;">
            <ol id="b" class="example">
            </ol>
        </div>
    </div>
    <%--<div class="row">
        <div id="DashboardManagementDiInfo" class="col-md-12" style="text-align: right;">
            <asp:LinkButton ID="lbtnDashboardManagement" runat="server" OnClick="lbtnDashboardManagement_Click">Dashboard Management</asp:LinkButton>
        </div>
    </div>--%>
</asp:Content>
