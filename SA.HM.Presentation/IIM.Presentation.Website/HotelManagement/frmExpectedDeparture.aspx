<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmExpectedDeparture.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmExpectedDeparture" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {

            $("#btnRegistrationSearch").click(function () {
                LoadRegistrationInformation();;
            });

            $('#ContentPlaceHolder1_txtRsvCheckInDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtCheckOutDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtCheckOutDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtRsvCheckInDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtCheckOutDate").val(GetStringFromDateTime(new Date));
            $("#btnRegistrationSearch").trigger('click');
        });

        function LoadRegistrationInformation() {
            var guestName = $("#<%=txtRgtrGuestName.ClientID %>").val();
            var companyName = $("#<%=txtRgtrCompanyName.ClientID %>").val();
            var regNumber = $("#<%=txtRegistrationNo.ClientID %>").val();
            var checkInDate = $("#<%=txtRsvCheckInDate.ClientID %>").val();
            var checkOutDate = $("#<%=txtCheckOutDate.ClientID %>").val();

            var roomNumber = $("#<%=txtSrcRoomNumber.ClientID %>").val();

            PageMethods.SearchNLoadRegistrationInfo(guestName, companyName, regNumber, checkInDate, checkOutDate, roomNumber,  1, 1, 1, OnLoadRegistrationInfoSucceeded, OnLoadRegistrationInfoFailed);
            return false;
        }
        function OnLoadRegistrationInfoSucceeded(result) {
            $("#tbRegistrationDetails tbody tr").remove();
            $("#GridPagingContainer ul").html("");
            var strTable = "", totalRow = 0;
            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#tbRegistrationDetails tbody ").append(emptyTr);
                return false;
            }

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tbRegistrationDetails tbody tr").length;
                //if ((totalRow % 2) == 0) {
                //    strTable += "<tr style=\"background-color:#E3EAEB;\">";
                //}
                //else {
                //    strTable += "<tr style=\"background-color:#FFFFFF;\">";
                //}


                if ((totalRow % 2) == 0) {
                    if (gridObject.IsStopChargePosting == 1)
                    {
                        strTable += "<tr style=\"background-color:#c3c3b0;\">";
                    }
                    else
                    {
                        strTable += "<tr style=\"background-color:#E3EAEB;\">";
                    }                    
                }
                else {
                    if (gridObject.IsStopChargePosting == 1) {
                        strTable += "<tr style=\"background-color:#c3c3b0;\">";
                    }
                    else {
                        strTable += "<tr style=\"background-color:#FFFFFF;\">";
                    }                    
                }

                strTable += "<td onClick='javascript:return LoadRoomPossiblePathPopUp(" + gridObject.RoomNumberList + ");' align='left'  style=\"width:20%;cursor:pointer\">" + gridObject.RRNumber + "</td>";
                strTable += "<td onClick='javascript:return LoadRoomPossiblePathPopUp(" + gridObject.RoomNumberList + ");' align='left'  style=\"width:30%;cursor:pointer\">" + gridObject.GuestName + "</td>";
                strTable += "<td onClick='javascript:return LoadRoomPossiblePathPopUp(" + gridObject.RoomNumberList + ");' align='left'  style=\"width:30%;cursor:pointer\">" + gridObject.RoomNumberList + "</td>";
                strTable += "<td onClick='javascript:return LoadRoomPossiblePathPopUp(" + gridObject.RoomNumberList + ");' align='left'  style=\"width:20%;cursor:pointer\">" + gridObject.DateIn + "</td>";

                strTable += "</tr>";

                $("#tbRegistrationDetails tbody").append(strTable);
                strTable = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            //$("#ltlRegistrationInformation").html(result);
            $("#RegistrationPopSearchPanel").show();
            return false;
        }
        function OnLoadRegistrationInfoFailed(error) {
        }
        function LoadRoomPossiblePathPopUp(roomNumber) {
            //toastr.info("Hi, Developer working on it...");
            var pageTitle = "Ocupied Room Possible Path";
            //var roomNumber = $(this).next(".RoomNumberDiv").text();
            PageMethods.LoadRegistrationPossiblePath(roomNumber, pageTitle, OnLoadPossiblePathSucceeded, OnLoadPossiblePathSucceededFailed);
            return false;
        }
        function OnLoadPossiblePathSucceeded(result) {
            var pageTitle = "Ocupied Room Possible Path";
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
        function OnLoadPossiblePathSucceededFailed(error) {

        }
        function CountTotalNumberOfGuestByRoomNumber(roomNumber, regId) {
            PageMethods.CountTotalNumberOfGuestByRoomNumber(roomNumber, regId, OnCountTotalNumberOfGuestByRoomNumberSucceeded, OnCountTotalNumberOfGuestByRoomNumberFailed);
        }
        function OnCountTotalNumberOfGuestByRoomNumberSucceeded(result) {
            if (result.NumberOfGuest == 1) {
                GetRegistrationInformationForSingleGuestByRoomNumber(result.RoomNumber, result.RegistrationId);
            }
            else {
                GetRegistrationInformationByRoomNumber(result.RoomNumber, result.RegistrationId);
            }
        }

        function GetRegistrationInformationForSingleGuestByRoomNumber(roomNumber, regId) {
            PageMethods.GetRegistrationInformationForSingleGuestByRoomNumber(roomNumber, regId, OnGetRegistrationInformationForSingleGuestByRoomNumberSucceeded, OnGetRegistrationInformationForSingleGuestByRoomNumberFailed);
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
            $("#<%=lblDGuestAddress1.ClientID %>").text(result.CompanyName);
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
            $("#<%=lblDMealPlan.ClientID %>").text(result.MealPlan);
            $("#tabsPopMyTabs").tabs({ active: 0 });

            $("#TouchKeypad").dialog({
                autoOpen: true,
                modal: true,
                width: 1000,
                height: 666,
                closeOnEscape: true,
                resizable: false,
                title: "Occupied Room Possible Path",
                show: 'slide'
            });

            $("#popUpDiv").css('height', '657px');

            PageMethods.LoadGuestPreferences(result.GuestId, OnLoadGuestPreferencesSucceeded, OnLoadGuestPreferencesFailed);
            return false;
        }
        function OnLoadGuestPreferencesSucceeded(result) {
            $("#Preference").html(result);
        }
        function OnLoadGuestPreferencesFailed(error) {
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
        function OnCountTotalNumberOfGuestByRoomNumberFailed(error) {
        }
        function LoadGuestImage(guestId) {

            PageMethods.GetDocumentsByUserTypeAndUserId(guestId, OnLoadImagesSucceeded, OnLoadImagesFailed);
            return false;
        }
        function OnLoadAirportDropFailed(error) {
            toastr.error(error.get_message());
        }

        function OnLoadImagesSucceeded(result) {
            $("#imageDiv").html(result);
            return false;
        }
        function OnLoadImagesFailed(error) {
            toastr.error(error.get_message());
        }
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
            $("#<%=lblDGuestAddress1.ClientID %>").text(result.CompanyName);
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
            $("#<%=lblDMealPlan.ClientID %>").val(result.MealPlan);


            $("#PopMyTabs").tabs({ active: 0 });

            $("#TouchKeypad").dialog({
                autoOpen: true,
                modal: true,
                width: 1000,
                height: 666,
                closeOnEscape: true,
                resizable: false,
                title: "Occupied Room Possible Path",
                show: 'slide'
            });

            PageMethods.LoadGuestPreferences(result.GuestId, OnLoadGuestPreferencesSucceeded, OnLoadGuestPreferencesFailed);
        }
        function OnGetRegistrationInformationByRoomNumberFailed(error) {

        }

        function GetRegistrationInformationByRoomNumber(roomNumber, regId) {
            PageMethods.GetRegistrationInformationListByRoomNumber(roomNumber, regId, OnGetRegistrationInformationByRoomNumberSucceeded, OnGetRegistrationInformationByRoomNumberFailed);

        }
        function OnGetRegistrationInformationByRoomNumberSucceeded(result) {

            $('#GuestList').html(result);
            $("#GuestListingPopUp").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 300,
                closeOnEscape: true,
                resizable: false,
                title: "Occupied Room Possible Path",
                show: 'slide'
            });

            return false;
        }
        function OnGetRegistrationInformationByRoomNumberFailed(error) {
        }

        function LoadBillPreview() {
            $("#serviceDecider").dialog('close');
            $('#btnBillPreview').trigger('click');
        }

        function LoadBillPreviewAndBillLock(registrationId) {
            $("#<%=hfPaxInRegistrationId.ClientID %>").val(registrationId);
            $("#serviceDecider").dialog('close');
            $('#btnBillPreviewAndBillLock').trigger('click');
        }

        function ClearDetails() {
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
            $("#<%=lblDMealPlan.ClientID %>").val("");
        }
    </script>
    <div style="display: none;">
        <asp:Button ID="btnBillPreview" runat="server" ClientIDMode="Static" Text="Bill Preview" OnClick="btnBillPreview_Click" />
        <asp:Button ID="btnBillPreviewAndBillLock" runat="server" ClientIDMode="Static" Text="Bill Lock & Preview" OnClick="btnBillPreviewAndBillLock_Click" />
    </div>
    <div id="serviceDecider" style="display: none;">
        <div id="serviceDeciderHtml">
        </div>
    </div>
    <%--<div id="ReservationPopUp" style="display: none;">
        <div id="PopReservation">
            <ul id="Ul1" class="ui-style">
                <li id="Li1" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#PopupReservationInfo">Reservation Information</a></li>
                <li id="Li2" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#PopupReservationGuestInfo">Guest Information</a></li>
            </ul>
            <div id="PopupReservationInfo">
                <div id="ReservationDetails" class="panel panel-default" style="font-weight: bold">
                    <div class="panel-heading">Reservation Information</div>
                    <div id="ReservationInfo" class="panel-body">
                    </div>
                </div>
            </div>
            <div id="PopupReservationGuestInfo">
                <div id="ReservationGuest" class="panel panel-default" style="font-weight: bold">
                    <div class="panel-heading">Guest Information</div>
                    <div id="ReservationGuestInfo" class="panel-body">
                    </div>
                </div>
            </div>
            <button type="button" id="btnReservationBack" onclick="javascript:return popup(-1)"
                class="TransactionalButton btn btn-primary">
                Back</button>
        </div>

    </div>--%>
    <div id="TouchKeypad" style="display: none;">
        <div id="PopMyTabs">
            <ul id="PoptabPage" class="ui-style">
                <li id="PopA" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab-1">Registration Information</a></li>
                <li id="PopB" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab-2">Registration Document</a></li>
                <li id="PopC" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab-3">Preferences</a></li>
                <li id="PopD" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab-4">Airport Drop Information</a></li>
            </ul>
            <div id="Poptab-1">
                <div id="GuestInfo" class="block" style="font-weight: bold">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Registration Information
                    </a>
                    <div class="HMBodyContainer">
                        <table class="table table-striped table-bordered table-condensed table-hover">
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="lblLArrivalDate" runat="server" Text="Arrival Date"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblDArrivalDate" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="lblLExpectedDepartureDate" runat="server" Text="Expected Departure Date"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblDExpectedDepartureDate" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="lblLRoomRate" runat="server" Text="Room Tariff"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblDRoomRate" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="lblLCountryName" runat="server" Text="Country Name"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblDCountryName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="lblLGuestName" runat="server" Text="Name"></asp:Label>
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
                                    <asp:Label ID="lblLGuestAddress2" runat="server" Text="Address"></asp:Label>
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
                                    <asp:Label ID="lblLGuestNationality" runat="server" Text="Nationality"></asp:Label>
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
                                    <asp:Label ID="lblGuestProfession" runat="server" Text="Profession"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblDGuestProfession" runat="server"></asp:Label>
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
                                    <asp:Label ID="lblVReferanceName" runat="server" Text="Referance"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblDReferanceName" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="lblVMealPlan" runat="server" Text="Meal Plan"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblDMealPlan" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="lblVRegistrationRemarks" runat="server" Text="Remarks"></asp:Label>
                                </td>
                                <td class="span4" colspan="3">
                                    <asp:Label ID="lblDRegistrationRemarks" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div class="divClear">
                        </div>
                        <div id="PaxInRateInfoDiv" class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblPaxInRate" runat="server" Text="Pax In Rate"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:HiddenField ID="hfPaxInRegistrationId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hfPaxInGuestId" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtPaxInRate" runat="server" Width="140px" TabIndex="1"></asp:TextBox>
                                <asp:Button ID="btnPaxInRateUpdate" runat="server" TabIndex="4" Text="Update" CssClass="TransactionalButton btn btn-primary" OnClick="btnPaxInRateUpdate_Click" />
                            </div>
                        </div>
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
            <div class="divClear">
            </div>
            <div id="Poptab-4">
                <div id="Div4" class="block" style="font-weight: bold">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Drop Information
                    </a>
                    <div class="HMBodyContainer">
                        <table class="table table-striped table-bordered table-condensed table-hover">
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="lblAirlineName" runat="server" Text="Airline Name"></asp:Label>
                                </td>
                                <td class="span4" colspan="3">
                                    <asp:Label ID="lblDAirlineName" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="span2">
                                    <asp:Label ID="lblFlightNumber" runat="server" Text="Flight Number"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblDFlightNumber" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="span2">
                                    <asp:Label ID="lblDepartureTime" runat="server" Text="Departure Time"></asp:Label>
                                </td>
                                <td class="span4">
                                    <asp:Label ID="lblDDepartureTime" runat="server" Text=""></asp:Label>
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
            <div id="Poptab-3">
                <div id="Div3" class="block" style="font-weight: bold">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Preferences </a>
                    <div id="Preference">
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divBox">
                <div class="divSection divSectionLeftLeft">
                </div>
                <div class="divSection divSectionLeftRight" style="margin-top: 10px; text-align: right; float: right;">
                    <input type="button" value="Back" class="btn btn-primary" id="btnBackToDecisionMaker" />
                </div>
            </div>
            <div class='divClear'>
            </div>
        </div>
    </div>
    <div id="GuestListingPopUp" style="display: none;">
        <div id="GuestList">
        </div>
    </div>
    <div id="registrationPopup">
        <%--<div id="Div2" class="alert alert-info" style="display: none;">
            <button type="button" class="close" data-dismiss="alert">
                ×</button>
            <asp:Label ID='Label17' Font-Bold="True" runat="server"></asp:Label>
        </div>--%>
        <div id="registrationPopEntryPanel" class="panel panel-default">
            <div class="panel-heading">
                Search Registration
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="control-label col-md-2">
                            Registration No.</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtRegistrationNo" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                        </div>
                        <label class="control-label col-md-2">
                            Room Number</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="Country" class="control-label col-md-2">
                            Guest Name</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRgtrGuestName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="Country" class="control-label col-md-2">
                            Company Name</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRgtrCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="Country" class="control-label col-md-2">
                            Check-In Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtRsvCheckInDate" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                        </div>
                        <label for="Country" class="control-label col-md-2">
                            Check-Out Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtCheckOutDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-md-12">
                            <button type="button" id="btnRegistrationSearch" class="btn btn-primary btn-sm">
                                Search</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="RegistrationPopSearchPanel" class="panel panel-default" style="display: none;">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div id="ltlRegistrationInformation">
                    <table class="table table-bordered table-condensed table-responsive" id='tbRegistrationDetails'
                        width="100%">
                        <colgroup>
                            <col style="width: 20%;" />
                            <col style="width: 30%;" />
                            <col style="width: 30%;" />
                            <col style="width: 20%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td >Reservation No
                                </td>
                                <td >Guest Name
                                </td>
                                <td >Room No
                                </td>
                                <td >Check-In
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
        </div>
    </div>
</asp:Content>
