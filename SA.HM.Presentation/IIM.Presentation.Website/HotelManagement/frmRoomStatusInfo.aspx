<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmRoomStatusInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmRoomStatusInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var pageTitle = '';

        $(document).ready(function () {
            CommonHelper.ApplyIntigerValidation();

            var param = CommonHelper.GetParameterByName("ExpectedDeparture");
            if (param != "") {
                $("#hideForParam").hide();
                $("#LegendContainerDiv").hide();
            }
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Status</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_txtProbableCleanTime').timepicker({
                showPeriod: is12HourFormat,

            });

            
            $(".ToolTipClass").tooltip({
                items: "div",
                relative: true,
                content: function () {
                    if($(this).attr('id'))
                    {
                        var $variable = $('#ContentDiv' + $(this).attr('id')).html();
                        return $variable;
                    }
                },
                classes: {
                    "ui-tooltip": "highlight ui-corner-all ui-widget-shadow"
                }
            });
            $(".ToolTipClass").tooltip('option', 'position', { my: 'center top', at: 'center bottom+10', collision: 'none' });
            $(".ToolTipClass").tooltip('option', 'tooltipClass', 'bottom_tooltip');

            $("#PopMyTabs").tabs();
            $("#PopMyOutOfService").tabs();
            $("#PopReservation").tabs();

            var floorValue = $("#<%=ddlSrcFloorId.ClientID %>").val();
            var blockValue = $("#<%=ddlFloorBlock.ClientID %>").val();


            function shoHideDesign() {
                if ((floorValue != 0 && blockValue != 0)) {

                    if ($("#<%=ddlFloorDesign.ClientID%>").val() == 2) {
                        $("#GraphicalPanel").show();
                        $("#SearchPanel").hide();

                    }
                    else if ($("#<%=ddlFloorDesign.ClientID%>").val() == 1) {
                        $("#GraphicalPanel").hide();
                        $("#SearchPanel").show();

                    }
                    $("#ContentPlaceHolder1_btnFloorView").hide();
                }
                else {
                    $("#showDesign").hide();
                    $("#GraphicalPanel").hide();
                    $("#SearchPanel").show();
                    $("#ContentPlaceHolder1_btnFloorView").show();
                }
            }

            shoHideDesign();

            $("#<%=ddlSrcFloorId.ClientID %>").change(function () {
                if ($("#<%=ddlSrcFloorId.ClientID %>").val() != 0 && $("#<%=ddlFloorBlock.ClientID %>").val() == 0) {
                    $("#showDesign").hide("slow");
                    $("#ContentPlaceHolder1_btnFloorView").show("slow");
                }
                else if ($("#<%=ddlSrcFloorId.ClientID %>").val() == 0 && $("#<%=ddlFloorBlock.ClientID %>").val() != 0) {
                    $("#showDesign").hide("slow");
                    $("#ContentPlaceHolder1_btnFloorView").show("slow");
                }
                else if ($("#<%=ddlSrcFloorId.ClientID %>").val() == 0 && $("#<%=ddlFloorBlock.ClientID %>").val() == 0) {
                    $("#showDesign").hide("slow");
                    $("#ContentPlaceHolder1_btnFloorView").show("slow");
                }
                else {
                    $("#showDesign").show("slow");
                    $("#ContentPlaceHolder1_btnFloorView").hide();
                }


            });

            $("#<%=ddlFloorBlock.ClientID %>").change(function () {
                if ($("#<%=ddlFloorBlock.ClientID %>").val() == 0 && $("#<%=ddlSrcFloorId.ClientID %>").val() != 0) {
                    $("#showDesign").hide("slow");
                    $("#ContentPlaceHolder1_btnFloorView").show();
                }
                else if ($("#<%=ddlFloorBlock.ClientID %>").val() != 0 && $("#<%=ddlSrcFloorId.ClientID %>").val() == 0) {
                    $("#showDesign").hide("slow");
                    $("#ContentPlaceHolder1_btnFloorView").show();
                }
                else if ($("#<%=ddlSrcFloorId.ClientID %>").val() == 0 && $("#<%=ddlFloorBlock.ClientID %>").val() == 0) {
                    $("#showDesign").hide("slow");
                    $("#ContentPlaceHolder1_btnFloorView").show("slow");
                }
                else {
                    $("#showDesign").show("slow");
                    $("#ContentPlaceHolder1_btnFloorView").hide();
                }
            });

            if ($("#<%=ddlStatusType.ClientID %>").val() == "3") {
                $('#FloorInformation').show("slow");
                // $("#ContentPlaceHolder1_btnFloorView").show();
                $('#GroupInformation').hide("slow");
                $('#ViewStatusDiv').hide("slow");
                //$('#GuestNameDisplayFlagDiv').hide("slow");
            }
           <%-- else if ($("#<%=ddlStatusType.ClientID %>").val() == "6") {
                $('#FloorInformation').hide("slow");
                //$("#GraphicalPanel").hide();
                $('#GroupInformation').show("slow");
                $('#ViewStatusDiv').hide("slow");
                $('#GuestNameDisplayFlagDiv').show("slow");

            }--%>
            else {
                $('#FloorInformation').hide();
                //$("#GraphicalPanel").hide();
                $('#GroupInformation').hide("slow");
                $('#ViewStatusDiv').show();
                //$('#GuestNameDisplayFlagDiv').show("slow");
            }

            $('.RoomVacantDirtyDiv').on('click', function (e) {
                var roomNumber = $(this).next(".RoomNumberDiv").text();
                pageTitle = "Vacant and Dirty Room Possible Path";
                PageMethods.LoadVacantDirtyPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;

            });
            $('.RoomReservedDiv').on('click', function (e) {
                pageTitle = "Reserved Room Possible Path";
                var roomNumber = $(this).next(".RoomNumberDiv").text();

                PageMethods.LoadReservedPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;

            });
            $('.RoomTodaysCheckInDiv').on('click', function (e) {
                pageTitle = "Today's Checked In Room Possible Path";
                var roomNumber = $(this).next(".RoomNumberDiv").text();
                var linkStatus = $("#ContentPlaceHolder1_hfLinkStatus").val();

                PageMethods.LoadOccupiedPossiblePath(roomNumber, pageTitle, linkStatus, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;
            });
            $('.RoomOccupaiedDiv').on('click', function (e) {
                pageTitle = "Occupied Room Possible Path";
                var roomNumber = $(this).next(".RoomNumberDiv").text();
                var linkStatus = $("#ContentPlaceHolder1_hfLinkStatus").val();
                $("#ContentPlaceHolder1_hfViewStatus").val("0");
                PageMethods.LoadOccupiedPossiblePath(roomNumber, pageTitle, linkStatus, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;
            });
            $('.RoomLongStayingDiv').on('click', function (e) {
                pageTitle = "Long Staying Room Possible Path";
                var roomNumber = $(this).next(".RoomNumberDiv").text();
                var linkStatus = $("#ContentPlaceHolder1_hfLinkStatus").val();

                //CallRoomFeaturesInfo(roomNumber);
                PageMethods.LoadOccupiedPossiblePath(roomNumber, pageTitle, linkStatus, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;
            });
            $('.RoomPossibleVacantDiv').on('click', function (e) {
                pageTitle = "Expected Departure Room Possible Path";
                var roomNumber = $(this).next(".RoomNumberDiv").text();
                var linkStatus = $("#ContentPlaceHolder1_hfLinkStatus").val();

                PageMethods.LoadExpectedDeparturePath(roomNumber, pageTitle, linkStatus, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;
            });
            $('.RoomOutOfOrderDiv').on('click', function (e) {
                pageTitle = "Out Of Order Room Possible Path";
                var roomNumber = $(this).next(".RoomNumberDiv").text();
                //CallRoomFeaturesInfo(roomNumber);
                PageMethods.LoadOutOfOrderPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;
            });
            $('.RoomOutOfServiceDiv').on('click', function (e) {
                pageTitle = "Out Of Service Room Possible Path";
                var roomNumber = $(this).next(".RoomNumberDiv").text();
                PageMethods.LoadOutOfServicePossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;
            });
            $('.RoomVacantDiv').on('click', function (e) {
                pageTitle = "Vacant Room Possible Path";
                var roomNumber = $(this).next(".RoomNumberDiv").text();
                PageMethods.LoadVacantPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;
            });
            $('.HoldBillRegistrationDiv').on('click', function (e) {
                pageTitle = "Hold Bill Registration Possible Path";
                var regId = $(this).next(".RoomNumberDiv").text();
                PageMethods.LoadHoldBillRegistrationPossiblePath(regId, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;
            });
            $('.BlankRegistrationDiv').on('click', function (e) {
                pageTitle = "Blank Registration Possible Path";
                var regId = $(this).next(".RoomNumberDiv").text();
                PageMethods.LoadBlankRegistrationPossiblePath(regId, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;
            });

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

            function ClearOutOfService() {
                $("#<%=lblDRemarks.ClientID %>").text('');
                $("#<%=lblDFromDate.ClientID %>").text('');
                $("#<%=lblDToDate.ClientID %>").text('');
                $("#<%=lblDRoomStatus.ClientID %>").text('');
                $("#<%=lblDRoomType.ClientID %>").text('');
                $("#<%=lblDRoomNumber.ClientID %>").text('');
                pageTitle = '';
            }

            $("#<%=ddlStatusType.ClientID %>").change(function () {
                if ($("#<%=ddlStatusType.ClientID %>").val() == "3") {
                    $('#LegendContainerDiv').hide();
                    $('#FloorInformation').show("slow");
                    $('#GroupInformation').hide("slow");
                    $('#ViewStatusDiv').hide("slow");
                    //$('#GuestNameDisplayFlagDiv').hide("slow");
                    $("#<%=ltlRoomTemplate.ClientID %>").hide();

                }
                <%--else if ($("#<%=ddlStatusType.ClientID %>").val() == "6") {
                    $('#LegendContainerDiv').hide();
                    $('#FloorInformation').hide("slow");
                    //$("#GraphicalPanel").hide();
                    $('#GroupInformation').show("slow");
                    $('#ViewStatusDiv').hide("slow");
                    //$('#GuestNameDisplayFlagDiv').show("slow");
                    $("#<%=ltlRoomTemplate.ClientID %>").hide();
                }--%>
                else {
                    $('#LegendContainerDiv').hide();
                    $('#FloorInformation').hide();
                    //$("#GraphicalPanel").hide();
                    $('#GroupInformation').hide("slow");
                    $('#ViewStatusDiv').show();
                    //$('#GuestNameDisplayFlagDiv').show("slow");
                    $("#<%=ltlRoomTemplate.ClientID %>").hide();
                }
                $("#ContentPlaceHolder1_hfMasterId").val("0");
                $("#ContentPlaceHolder1_hfLinkStatus").val("1");
                $("#ContentPlaceHolder1_hfViewStatus").val("1");

            });
            $("#btnReservationBack").click(function () {
                $("#ReservationPopUp").dialog("close");
                $("#serviceDecider").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 600,
                    closeOnEscape: true,
                    resizable: false,
                    title: pageTitle,
                    show: 'slide'
                });
            });

            $("#btnBackToDecisionMaker").click(function () {
                $("#TouchKeypad").dialog("close");
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

        });

        function FloorViewStatusCheck() {
            if ($("#<%=ddlSrcFloorId.ClientID %>").val() == 0 && $("#<%=ddlFloorBlock.ClientID %>").val() != 0) {
                toastr.warning("Please select a floor");
                $("#ContentPlaceHolder1_ddlSrcFloorId").focus();
                return false;
            }
        }

        function LoadBillPreview() {
            $("#serviceDecider").dialog('close');
            $('#btnBillPreview').trigger('click');

        }
        function LoadLinkedBillPreview() {
            $("#serviceDecider").dialog('close');
            $('#btnLinkedBillPreview').trigger('click');
        }
        function LoadLinkedRoom(masterId) {
            $("#serviceDecider").dialog('close');
            var roomNumber = $("#roomNumber").text();
            var status = 0;

            if (masterId != null) {
                $("#ContentPlaceHolder1_hfMasterId").val(masterId);
            }


            $("#ContentPlaceHolder1_hfLinkStatus").val("0");
            $("#ContentPlaceHolder1_hfViewStatus").val("0");


            $("#LegendContainerDiv").hide();
            $("#ContentPlaceHolder1_ltlRoomTemplate").hide();


            $("#ContentPlaceHolder1_btnViewStatus").trigger("click");//trigger btn ViewStatus


        }

        function PerformViewActionForGuestDetail(guestId, registrationId) {
            PageMethods.PerformViewActionForGuestDetail(guestId, registrationId, OnPerformViewActionForGuestDetailSucceeded, OnGetRegistrationInformationByRoomNumberFailed);
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
            var additionalRemarks = result.AdditionalRemarks != "" ? `<ul><li>${result.AdditionalRemarks}</li></ul>` : "";
            $("#AdditionalRemarks").html(additionalRemarks);

            $("#PopMyTabs").tabs({ active: 0 });
            $("#PopMyOutOfService").tabs({ active: 0 });

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
            //CallRoomFeaturesInfo(result.roomNumber);
            PageMethods.LoadGuestPreferences(result.GuestId, OnLoadGuestPreferencesSucceeded, OnLoadGuestPreferencesFailed);
            PageMethods.LoadGuestAirportDrop(result.RegistrationId, OnLoadAirportDropSucceeded, OnLoadAirportDropFailed);

        }
        function OnGetRegistrationInformationByRoomNumberFailed(error) {

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
            $("#<%=lblDMealPlan.ClientID %>").val("");
        }

        function CountTotalNumberOfGuestByRoomNumber(roomNumber, regId) {
            CallRoomFeaturesInfo(roomNumber);
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
        function OnCountTotalNumberOfGuestByRoomNumberFailed(error) {
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
            var additionalRemarks = result.AdditionalRemarks != "" ? `<ul><li>${result.AdditionalRemarks}</li></ul>` : "";
            $("#AdditionalRemarks").html(additionalRemarks);
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

            //CallRoomFeaturesInfo(result.roomNumber);

            PageMethods.LoadGuestPreferences(result.GuestId, OnLoadGuestPreferencesSucceeded, OnLoadGuestPreferencesFailed);
            return false;
        }
        function OnLoadGuestPreferencesSucceeded(result) {
            $("#Preference").html(result);
        }
        function OnLoadGuestPreferencesFailed(error) {
            toastr.error(error.get_message());
        }

        function CallRoomFeaturesInfo(roomNumber) {
            PageMethods.LoadRoomFeaturesInfo(roomNumber, OnLoadRoomFeaturesSucceed, OnLoadRoomFeaturesFailed)
        }
        function OnLoadRoomFeaturesSucceed(result) {
            $("#RoomFeaturesInfo").html(result);
            $("#RoomFeaturesInfo2").html(result);

        }
        function OnLoadRoomFeaturesFailed(error) {
            toastr.error(error.get_message());
        }

        function ShowRoomFeaturesInfo(roomNumber) {
            CallRoomFeaturesInfo(roomNumber);

            PageMethods.LoadRoomFeaturesInfo(roomNumber, OnLoadShowRoomFeaturesSucceed, OnLoadShowRoomFeaturesFailed)
        }
        function OnLoadShowRoomFeaturesSucceed(result) {

            $("#RoomFeaturesInfo3").html(result);
            $("#VacantPopUp").dialog({
                autoOpen: true,
                modal: true,
                width: 750,
                closeOnEscape: true,
                resizable: false,
                title: pageTitle,
                show: 'Vacant'
            });
        }
        function OnLoadShowRoomFeaturesFailed(error) {
            toastr.error(error.get_message());
        }

        function ShowOutOfServiceRoomInformation(roomNumber) {
            CallRoomFeaturesInfo(roomNumber);
            PageMethods.ShowOutOfServiceRoomInformation(roomNumber, OnShowOutOfServiceRoomInformationSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }

        function OnShowOutOfServiceRoomInformationSucceeded(result) {
            $("#<%=lblDRemarks.ClientID %>").text(result.Remarks);
            $("#<%=lblDFromDate.ClientID %>").text(GetStringFromDateTime(result.FromDate));
            $("#<%=lblDToDate.ClientID %>").text(GetStringFromDateTime(result.ToDate));
            $("#<%=lblDRoomStatus.ClientID %>").text(result.StatusName);
            $("#<%=lblDRoomType.ClientID %>").text(result.RoomType);
            $("#<%=lblDRoomNumber.ClientID %>").text(result.RoomNumber);
            $("#OutOfServicePopUp").dialog({
                autoOpen: true,
                modal: true,
                width: 750,
                closeOnEscape: true,
                resizable: false,
                title: pageTitle,
                show: 'Out of Order'
            });
        }
        function OnShowOutOfServiceRoomInformationFailed(error) {
        }



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
                width: 1000,
                height: 500,
                closeOnEscape: true,
                resizable: false,
                title: "Reservation Details",
                show: 'slide'
            });

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
                    toastr.warning("Please Provide Clean Date");
                    return;
                }
            }
            if (cleanTime == "") {

                toastr.warning("Please Provide Clean Time");
                $("#ContentPlaceHolder1_txtProbableCleanTime").focus();
                return;
            }

            if ($("#<%=ddlCleanStatus.ClientID %>").val().length > 0) {
                PageMethods.ChangeRoomStatus(roomId, cleanupStatus, remarks, fromDate, toDate, cleanDate, cleanTime, lastCleanDate, OnChangeRoomStatusSucceeded, OnChangeRoomStatusFailed);
            }
            else {
                toastr.warning("Please Provide Clean Status");
                return;
            }
            //PageMethods.ChangeRoomStatus(roomId, cleanupStatus, remarks, fromDate, toDate, cleanDate, cleanTime, lastCleanDate, OnChangeRoomStatusSucceeded, OnChangeRoomStatusFailed);
            return false;
        }

        function OnChangeRoomStatusSucceeded(result) {
            $("#<%=hfRoomId.ClientID %>").val("");
            $("#<%=txtRemarks.ClientID %>").val("");
            CommonHelper.AlertMessage(result.AlertMessage);
            $("#RoomCleanUpInfo").dialog("close");
            $("#serviceDecider").dialog("close");
            window.location = "frmRoomStatusInfo.aspx";
            // document.location.reload(true);
            $('#ContentPlaceHolder1_btnViewStatus').trigger('click');

        }

        function OnChangeRoomStatusFailed(error) {
        }

        function LoadCleanUpInfo(roomId) {
            PageMethods.GetRoomCleanUpInfo(roomId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            var guest = eval(result);
            var dateNow = new Date();
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
            $("#<%=txtFromDate.ClientID %>").attr('readonly', true);



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
    </script>
    <div style="display: none;">
        <asp:Button ID="btnBillPreview" runat="server" ClientIDMode="Static" Text="Bill Preview"
            OnClick="btnBillPreview_Click" />
    </div>
    <div style="display: none;">
        <asp:Button ID="btnLinkedBillPreview" runat="server" ClientIDMode="Static" Text="Bill Preview" OnClick="btnLinkedBillPreview_Click" />
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">Room Status Information</div>
        <asp:HiddenField ID="hfMasterId" Value="0" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfLinkStatus" Value="1" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfViewStatus" Value="0" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfTimeNow" runat="server"></asp:HiddenField>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div id="hideForParam" class="col-md-12">
                        <div class="col-sm-2">
                            <asp:Label ID="lblActiveStat" runat="server" CssClass="control-label" Text="Status Type"></asp:Label>
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlStatusType" runat="server" CssClass="form-control"
                                TabIndex="2">
                                <asp:ListItem Value="1">Room Number Wise</asp:ListItem>
                                <asp:ListItem Value="2">Room Type Wise</asp:ListItem>
                                <asp:ListItem Value="3">Floor Wise</asp:ListItem>
                                <asp:ListItem Value="4">Hold Up List</asp:ListItem>
                                <asp:ListItem Value="5">Blank Registration List</asp:ListItem>
                                <asp:ListItem Value="6">Expected Departure List</asp:ListItem>

                            </asp:DropDownList>
                        </div>
                        <%--<div class="col-md-4">
                            <div id="GuestNameDisplayFlagDiv" style="display:none">
                                <asp:DropDownList ID="ddlGuestNameDisplayFlag" runat="server" Width="170px" CssClass="form-control"
                                    TabIndex="2">
                                    <asp:ListItem Value="1">With Guest Name</asp:ListItem>
                                    <asp:ListItem Value="0">Without Guest Name</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>--%>
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" CssClass="control-label" Text="Room Number "></asp:Label>
                        </div>
                        <div class="col-md-2 quantity">
                            <asp:TextBox ID="txtSrchRoomNumber" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <div id="ViewStatusDiv">
                                <asp:Button ID="btnViewStatus" runat="server" TabIndex="3" Text="View Status" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnViewStatus_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group" id="FloorInformation">
                    <div class="col-md-12">
                        <div class="col-md-2">
                            <asp:Label ID="lblSrcFloorId" runat="server" CssClass="control-label" Text="Floor Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSrcFloorId" CssClass="form-control" runat="server" onse>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <asp:Label ID="Label2" runat="server" CssClass="control-label" Text="Block"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlFloorBlock" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Button Style="display: none" ID="btnFloorView" runat="server" TabIndex="3" Text="View Status"
                                CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnFloorViewStatus_Click" OnClientClick="return FloorViewStatusCheck()" />
                        </div>
                    </div>
                    <div class="col-md-12" style="margin-top: 4px;" id="showDesign">
                        <div class="col-md-2">
                            <asp:Label ID="Label3" runat="server" CssClass="control-label" Text="Design Block"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFloorDesign" CssClass="form-control" runat="server">
                                <asp:ListItem Value="1">Without Floor Design</asp:ListItem>
                                <asp:ListItem Value="2">With Floor Design</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2 col-md-offset-1">
                            <asp:Button ID="btnFloorViewStatus" runat="server" TabIndex="3" Text="View Status"
                                CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnFloorViewStatus_Click" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div id="LegendContainerDiv" class="col-md-12" style="display: none;">
                        <fieldset>
                            <legend>
                                <asp:Label ID="lblTotalRoomCount" runat="server" Text=""></asp:Label></legend>
                            <div id="ltlRoomStatusLegent" runat="server" style="padding-top: 10px;">
                            </div>
                        </fieldset>
                    </div>
                    <div class="col-md-12">
                        <div id="ltlRoomTemplate" runat="server">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel-body panel panel-default" id="GraphicalPanel" style="display: none;">
        <div class="panel-heading">Floor Management Information</div>
        <div class="row">
            <br />
            <div class="col-md-12">
                <div class="" style="height: 800px; overflow-y: scroll;">
                    <asp:Literal ID="ltlRoomTemplate_new" runat="server">
                    </asp:Literal>
                </div>
            </div>

        </div>
    </div>
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
                <li id="PopRoomFeatures" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab-5">Room Features</a></li>
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
                                    <asp:Label ID="lblLPassportNumber" runat="server" Text="Passport
    Number"></asp:Label>
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
                                    <asp:Label ID="lblVRegistrationRemarks" runat="server" Text="Hotel Remarks"></asp:Label>
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
                                <asp:Button ID="btnPaxInRateUpdate" runat="server" TabIndex="4" Text="Update" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnPaxInRateUpdate_Click" />
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
                <div id="Div3" class="block col-md-6" style="font-weight: bold">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Preferences </a>
                    <div id="Preference">
                    </div>
                </div>
                <div class="block col-md-6" style="font-weight: bold">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Additional Remarks </a>
                    <div id="AdditionalRemarks">
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div id="Poptab-5">
                <div id="Div5" class="block" style="font-weight: bold">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Room Features: </a>
                    <div id="RoomFeaturesInfo">
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
    <div id="OutOfServicePopUp" style="display: none;">
        <div id="PopMyOutOfService">
            <ul id="PoptabPage2" class="ui-style">
                <li id="popRegInfo" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab2-1">Room Information</a></li>
                <li id="popRoomFt" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab2-2">Room Features</a></li>
                <li id="popRoomForecast" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                    <a href="#Poptab2-3">Room Forecast</a></li>
            </ul>
            <div id="Poptab2-1">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLRoomType" runat="server" CssClass="control-label" Text="Room Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDRoomType" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblLRoomNumber" runat="server" CssClass="control-label" Text="Room Number"></asp:Label>
                        </div>
                        <div class=" col-md-4">
                            <asp:Label ID="lblDRoomNumber" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLRoomStatus" runat="server" CssClass="control-label" Text="Room Status"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:Label ID="lblDRoomStatus" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLFromDate" runat="server" CssClass="control-label" Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:Label ID="lblDFromDate" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLToDate" runat="server" CssClass="control-label" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:Label ID="lblDToDate" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLRemarks" runat="server" CssClass="control-label" Text="Remarks"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:Label ID="lblDRemarks" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div id="Poptab2-2">
                <div id="DivRoomFeaturesInfo2" class="block" style="font-weight: bold">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Room Features: </a>
                    <div id="RoomFeaturesInfo2">
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div id="Poptab2-3">
                <div id="DivOOORoomForecastInfo" class="block" style="font-weight: bold">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Room Forecast: </a>
                    <div id="OOORoomForecastInfo">
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>

    </div>
    <div id="VacantPopUp" style="display: none;">
        <div id="PopMyVacant">
            <div id="DivPopMyVacant" class="block" style="font-weight: bold">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Room Features: </a>
                <div id="RoomFeaturesInfo3">
                </div>
            </div>
        </div>
    </div>
    <div id="serviceDecider" style="display: none;">
        <div id="serviceDeciderHtml">
        </div>
    </div>

    <div id="ReservationPopUp" style="display: none;">
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
            <button type="button" id="btnReservationBack"
                class="TransactionalButton btn btn-primary">
                Back</button>
        </div>
    </div>
    <div id="RoomCleanUpInfo" style="display: none;">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <asp:HiddenField ID="hfRoomId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="txtLastCleanDate" runat="server"></asp:HiddenField>
                    <asp:Label ID="lblRoomNumber" runat="server" CssClass="control-label" Text="Room Number"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtRoomNumber" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblCleanStatus" runat="server" CssClass="control-label" Text="Status"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlCleanStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Value="Cleaned">Cleaned</asp:ListItem>
                        <asp:ListItem Value="Dirty">Dirty</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div id="OutOfServiceDivInfo" style="display: none;">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" CssClass="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker form-control" TabIndex="4"></asp:TextBox>
                    </div>
                    <div class="col-md-2 required-field">
                        <asp:Label ID="lblToDate" runat="server" CssClass="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div id="WithoutOutOfServiceDivInfo">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblApprovedDate" runat="server" CssClass="control-label" Text="Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtApprovedDate" runat="server" CssClass="datepicker form-control" Enabled="false"
                            TabIndex="4"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblProbableInTime" runat="server" CssClass="control-label" Text="Time"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtProbableCleanTime" Width="80" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblCleanRemarks" runat="server" CssClass="control-label" Text="Remarks"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Process" TabIndex="4" CssClass="TransactionalButton btn btn-primary"
                        OnClientClick="javascript:return ChangeRoomStatus()" />
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var isFloorInformationEnable = '<%=isFloorInformationEnable%>';
        if (isFloorInformationEnable > -1) {
            $('#FloorInformation').show();
            $('#GroupInformation').hide();
            $('#ViewStatusDiv').hide();
            $('#GuestNameDisplayFlagDiv').hide();
        }

        var isGroupInformationEnable = '<%=isGroupInformationEnable%>';
        if (isGroupInformationEnable > -1) {
            $('#FloorInformation').hide();
            $("#GraphicalPanel").hide();
            $('#GroupInformation').show();
            $('#ViewStatusDiv').hide();
            $('#GuestNameDisplayFlagDiv').show();
        }

        var isLegendContainerDivEnableVal = '<%=isLegendContainerDivEnable%>';
        if (isLegendContainerDivEnableVal > -1) {
            $('#LegendContainerDiv').show();
        }

    </script>
</asp:Content>
