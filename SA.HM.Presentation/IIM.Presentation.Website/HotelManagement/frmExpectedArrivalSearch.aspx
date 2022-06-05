<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmExpectedArrivalSearch.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmExpectedArrivalSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            
            $("#btnReservationSearch").click(function () {
                LoadReservationInformation();
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

            $("#ContentPlaceHolder1_txtRsvCheckInDate").val(GetStringFromDateTime(new Date));
            $("#btnReservationSearch").trigger('click');
        });

        function LoadReservationInformation() {
            var guestName = $("#<%=txtResvGuestName.ClientID %>").val();
            var companyName = $("#<%=txtResvCompanyName.ClientID %>").val();
            var reservNumber = $("#<%=txtReservationNo.ClientID %>").val();
            var checkInDate = $("#<%=txtRsvCheckInDate.ClientID %>").val();

            PageMethods.SearchNLoadReservationInfo(0, guestName, companyName, reservNumber, checkInDate, OnLoadReservInfoSucceeded, OnLoadReservInfoFailed);
            return false;
        }
        function OnLoadReservInfoSucceeded(result) {
            $("#ltlReservationInformation").html(result);
            $("#ReservationPopSearchPanel").show();
            return false;
        }
        function OnLoadReservInfoFailed(error) {
        }
        function LoadReservationGuestInformationForPopUp(reservationId, reservationDetailId, roomId) {
            pageTitle = "Reserved Room Possible Path";

            PageMethods.LoadReservedPossiblePath(reservationId, roomId, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }
        function ActionForExpressCheckInn(reservationId, roomTypeId) {
            $.confirm({
                title: 'Confirm!',
                content: 'Are you sure for Express Check Inn?',
                buttons: {
                    Yes: function () {
                        window.location = "frmExpressCheckIn.aspx?rsvnId=" + reservationId + "&rtId=" + roomTypeId;
                    },
                    No: function () {
                        OrderDetailsFlag = null;
                    }
                }
            });

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
        function PreRegistrationCardGenerate(resultURL) {
            var url = resultURL;
            var popup_window = "Pre Registration Card";
            window.open(url, popup_window, "width=770,height=680,left=300,top=50,resizable=yes");
        }
    </script>
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
            <button type="button" id="btnReservationBack" onclick="javascript:return popup(-1)"
                class="TransactionalButton btn btn-primary">
                Back</button>
        </div>

    </div>
    <div id="ReservationPopup">
        <%--<div id="Div2" class="alert alert-info" style="display: none;">
            <button type="button" class="close" data-dismiss="alert">
                ×</button>
            <asp:Label ID='Label17' Font-Bold="True" runat="server"></asp:Label>
        </div>--%>
        <div id="ReservationPopEntryPanel" class="panel panel-default">
            <div class="panel-heading">
                Search Reservation
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group" style="display:none;">
                        <label for="Country" class="control-label col-md-2">
                            Guest Name</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtResvGuestName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" style="display:none;">
                        <label for="Country" class="control-label col-md-2">
                            Company Name</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtResvCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" style="display:none;">
                        <label for="Country" class="control-label col-md-2">
                            Check-In Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtRsvCheckInDate" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
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
        <div id="ReservationPopSearchPanel" class="panel panel-default" style="display: none;">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div id="ltlReservationInformation">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
