<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmRoomCalender.aspx.cs" Inherits="HotelManagement.Presentation.Website.frmRoomCalender" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
			#ContentPlaceHolder1_parent {
				height: 500px;
			}
		</style>
    
    <script src="../../JSLibrary/TableHeadFixer/tableHeadFixer.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#fixTable").tableHeadFixer();
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Calendar</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            var txtCurrentDate = '<%=txtCurrentDate.ClientID%>'

            $('#ContentPlaceHolder1_txtCurrentDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#fixTable .ToolTipClass").tooltip({
                items: "td",
                relative: true,
                content: function () {
                    if ($(this).closest('tr').prop('id')) {
                        var $variable = $('#ContentDiv' + $(this).closest('tr').prop('id')).html();
                        return $variable;
                    }
                    /*if ($(this).attr('id')) {
                        var $variable = $('#ContentDiv' + $(this).attr('id')).html();
                        return $variable;
                    }*/
                },
                classes: {
                    "ui-tooltip": "highlight ui-corner-all ui-widget-shadow"
                }
            });
            $(".ToolTipClass").tooltip('option', 'position', { my: 'center top', at: 'center bottom+10', collision: 'none' });
            $(".ToolTipClass").tooltip('option', 'tooltipClass', 'bottom_tooltip');
        });

        function PopUpReservationInformation() {
            //$("#PopReservation").tabs('select', 0);
            //popup(1, 'ReservationPopUp', '', 900, 500);
            $("#ReservationPopUp").dialog({
                width: 900,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", ////TODO add title
                show: 'slide'
            });
            $("#popUpDiv").css('height', '496px');
            return false;
        }

        function PopUpRegistrationInformation() {
            $("#RegistrationPopUp").dialog({
                width: 920,
                height: 500,
                autoOpen: true,
                //modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", ////TODO add title
                show: 'slide'
            });
            $("#popUpDiv").css('height', '496px');
            return false;
        }

        $(function () {
            $("#PopReservation").tabs();
        });
        $(function () {
            $("#PopRegistration").tabs();
        });


        function RedirectToDetails(TransectionId, reservation, roomNumber, date) {
            if (reservation == 1) {
                //Registration
                PopUpRegistrationInformation();
                GetRegistrationInformation(TransectionId);
                GetRegistrationGuestInformation(TransectionId);
                GetRegistrationAirportPicUpInformation(TransectionId);
                GetRegistrationComplementaryInformation(TransectionId);
            }
            else if (reservation == 2) {
                //reservation
                PopUpReservationInformation();
                GetReservationInformation(TransectionId);
                GetReservationGuestInformation(TransectionId);
                GetReservationAirportPicUpInformation(TransectionId)
                GetReservationComplementaryInformation(TransectionId);
                LoadRegistration(roomNumber, TransectionId, date);
            }
            else if (reservation == 3) {
                //Out Of Service
                var pageTitle = 'Out Of Order Room';
                PageMethods.LoadOutOfOrderPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;
            }
            else if (reservation == 4) {
                //Out Of Service
                var pageTitle = 'Out Of Service Room';
                PageMethods.LoadOutOfOrderPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                return false;
            }
        }

        function OnLoadOutOfOrderPossiblePathSucceeded(result) {
            $('#serviceDeciderHtml').html(result);
            $("#serviceDecider").dialog({
                autoOpen: true,
                modal: true,
                width: 600,
                closeOnEscape: true,
                resizable: false,
                title: 'Out Of Order Room',
                show: 'slide'
            });
        }

        function ShowOutOfServiceRoomInformation(roomNumber) {
            PageMethods.ShowOutOfServiceRoomInformation(roomNumber, OnShowOutOfServiceRoomInformationSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }
        function OnShowOutOfServiceRoomInformationSucceeded(result) {
            popup(-1);

            $("#<%=lblDRemarks.ClientID %>").text(":   " + result.Remarks);
            $("#<%=lblDFromDate.ClientID %>").text(":   " + GetStringFromDateTime(result.FromDate));
            $("#<%=lblDToDate.ClientID %>").text(":   " + GetStringFromDateTime(result.ToDate));
            $("#<%=lblDRoomStatus.ClientID %>").text(":   " + result.StatusName);
            $("#<%=lblDRoomType.ClientID %>").text(":   " + result.RoomType);
            $("#<%=lblDRoomNumber.ClientID %>").text(":   " + result.RoomNumber);
            $("#OutOfServicePopUp").dialog({
                width: 600,
                height: 280,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", ////TODO add title
                show: 'slide'
            });
            $("#popUpDiv").css('height', '270px');
        }
        function OnShowOutOfServiceRoomInformationFailed(error) {
        }

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        //Get Registration Information
        function GetRegistrationInformation(TransectionId) {
            PageMethods.GetRegistrationInformationByRegistrationId(TransectionId, GetRegistrationInformationSucceeded, OnOperationFailed);
            return false;
        }
        function GetRegistrationGuestInformation(TransectionId) {
            PageMethods.GetRegistrationGuestInformationByRegistrationId(TransectionId, GetRegistrationGuestInformationSucceeded, OnOperationFailed);
            return false;
        }
        function GetRegistrationAirportPicUpInformation(TransectionId) {
            PageMethods.GetRegistrationAirportPicUpInformationByRegistrationId(TransectionId, GetRegistrationAirportPicUpInformationSucceeded, OnOperationFailed);
            return false;
        }
        function GetRegistrationComplementaryInformation(TransectionId) {
            PageMethods.GetRegistrationComplementaryInformationByRegistrationId(TransectionId, GetRegistrationComplementaryInformationSucceeded, OnOperationFailed);
            return false;
        }

        function GetRegistrationInformationSucceeded(result) {
            $("#RegistrationInfo").html(result)
        }
        function GetRegistrationGuestInformationSucceeded(result) {
            $("#RegistrationGuestInfo").html(result)
        }
        function GetRegistrationAirportPicUpInformationSucceeded(result) {
            var lblRegDepartureFlightNameText = '<%=lblRegDepartureFlightNameText.ClientID%>'
            var lblRegDepartureFlightNumberText = '<%=lblRegDepartureFlightNumberText.ClientID%>'
            var lblRegDepartureTimeText = '<%=lblRegDepartureTimeText.ClientID%>'
            var lblRegCommingFromText = '<%=lblRegCommingFromText.ClientID%>'
            var lblRegNextDestinationText = '<%=lblRegNextDestinationText.ClientID%>'
            var lblRegVisitPurposeText = '<%=lblRegVisitPurposeText.ClientID%>'

            $('#' + lblRegDepartureFlightNameText).text(':' + result.DepartureFlightName);
            $('#' + lblRegDepartureFlightNumberText).text(':' + result.DepartureFlightNumber);
            if (result.DepartureFlightNumber.length > 0) {
                $('#' + lblRegDepartureTimeText).text(':' + GetHourMinutes(result.DepartureTime));
            }
            else {
                $('#' + lblRegDepartureTimeText).text(':');
            }

            $('#' + lblRegCommingFromText).text(':' + result.CommingFrom);
            $('#' + lblRegNextDestinationText).text(':' + result.NextDestination);
            $('#' + lblRegVisitPurposeText).text(':' + result.VisitPurpose);
        }
        function GetRegistrationComplementaryInformationSucceeded(result) {
            $("#RegistrationComplementaryInfo").html(result)
        }

        //Get Reservation Information
        function GetReservationInformation(TransectionId) {
            PageMethods.GetReservationformationByReservationId(TransectionId, GetReservationInformationSucceeded, OnOperationFailed);
            return false;
        }
        function GetReservationGuestInformation(TransectionId) {
            PageMethods.GetReservationGuestInformationByReservationId(TransectionId, GetReservationGuestInformationSucceeded, OnOperationFailed);
            return false;
        }
        function GetReservationAirportPicUpInformation(TransectionId) {
            PageMethods.GetReservationAirportPicUpInformationByReservationId(TransectionId, GetReservationAirportPicUpInformationSucceeded, OnOperationFailed);
            return false;
        }
        function GetReservationComplementaryInformation(TransectionId) {
            PageMethods.GetReservationComplementaryInformationByReservationId(TransectionId, GetReservationComplementaryInformationSucceeded, OnOperationFailed);
            return false;
        }

        function GetReservationInformationSucceeded(result) {
            $("#ReservationInfo").html(result)
        }
        function GetReservationGuestInformationSucceeded(result) {
            $("#ReservationGuestInfo").html(result)
        }
        function GetReservationAirportPicUpInformationSucceeded(result) {
            var lblDepartaureFlightText = '<%=lblResDepartaureFlightText.ClientID%>'
            var lblDepartureFlightNumbertxt = '<%=lblResDepartureFlightNumbertxt.ClientID%>'
            var lblDepartureTimeText = '<%=lblResDepartureTimeText.ClientID%>'

            var lblAraivalFlightText = '<%=lblResAraivalFlightText.ClientID%>'
            var lblAraivalFlightNumberText = '<%=lblResAraivalFlightNumberText.ClientID%>'
            var lblAraivalTimeText = '<%=lblResAraivalTimeText.ClientID%>'

            $('#' + lblDepartaureFlightText).text(result.DepartureFlightName);
            $('#' + lblDepartureFlightNumbertxt).text(result.DepartureFlightNumber);

            if (result.DepartureFlightName == "") {
                $('#' + lblDepartureTimeText).text("");
            }
            else {
                $('#' + lblDepartureTimeText).text(GetHourMinutes(result.DepartureTime));
            }

            $('#' + lblAraivalFlightText).text(result.ArrivalFlightName);
            $('#' + lblAraivalFlightNumberText).text(result.ArrivalFlightNumber);

            if (result.ArrivalFlightName == "") {
                $('#' + lblAraivalTimeText).text("");
            }
            else {
                $('#' + lblAraivalTimeText).text(GetHourMinutes(result.ArrivalTime));
            }
        }
        function GetReservationComplementaryInformationSucceeded(result) {
            $("#ReservationComplementaryInfo").html(result)
        }

        function OnOperationFailed(error) {
            alert(error.get_message());
        }

        function GetHourMinutes(dateTime) {
            var dt = new Date(dateTime);
            var h = dt.getHours();
            var m = dt.getMinutes();

            if (isNaN(h) || isNaN(m)) {
                return "";
            }
            else {
                if (m <= 9) m = "0" + m;
                if (h <= 9) h = "0" + h;
                var HourMinutes = h + ':' + m;
                return HourMinutes;
            }

        }

        //new task
        function LoadReservation(dayCount, roomNumber) {
            var pageTitle = "Vacant Room Possible Path", type = "reservation";
            //PageMethods.LoadReservedPossiblePath(roomNumber, 0, pageTitle, type, OnLoadReservationSucceeded, OnLoadReservationFailed);
            PageMethods.LoadVacantPossiblePath(dayCount, roomNumber, pageTitle, OnLoadReservationSucceeded, OnLoadReservationFailed);
            return false;
        }
        function OnLoadReservationSucceeded(result) {
            $('#serviceDeciderHtml').html(result);

            $("#serviceDecider").dialog({
                autoOpen: true,
                modal: true,
                width: 600,
                closeOnEscape: true,
                resizable: false,
                title: "",
                show: 'slide'
            });
        }
        function OnLoadReservationFailed(error) {
        }
        function LoadRegistration(roomNumber, reservationId, date) {
            var pageTitle = "", type = "registration";
            PageMethods.LoadReservedPossiblePath(roomNumber, reservationId, pageTitle, type, date, OnLoadRegistrationSucceeded, OnLoadRegistrationFailed);
            return false;
        }
        function OnLoadRegistrationSucceeded(result) {
            $('#Div3').html(result);
        }
        function OnLoadRegistrationFailed(error) {
        }

        function CloseReservationPopUp() {
            $("#ReservationPopUp").dialog("close");
        }
        function CloseRegistrationPopUp() {
            $("#RegistrationPopUp").dialog("close");
        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="serviceDecider" style="display: none;">
        <div id="serviceDeciderHtml">
        </div>
    </div>
    <div id="OutOfServicePopUp" class="panel panel-default" style="display: none;">
        <div id="OutOfServiceDetails">
            <div class="panel-heading">
                Out Of Order Room Information
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLRoomType" runat="server" class="control-label" CssClass="bold"
                                Text="Room Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDRoomType" class="control-label" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLRoomNumber" runat="server" class="control-label" CssClass="bold"
                                Text="Room Number"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDRoomNumber" class="control-label" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLRoomStatus" runat="server" class="control-label" CssClass="bold"
                                Text="Room Status"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDRoomStatus" class="control-label" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLFromDate" runat="server" class="control-label" CssClass="bold"
                                Text="From Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDFromDate" class="control-label" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLToDate" runat="server" class="control-label" CssClass="bold" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDToDate" class="control-label" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblLRemarks" runat="server" class="control-label" CssClass="bold"
                                Text="Remarks"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDRemarks" class="control-label" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Room Calendar
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="StartDate" class="control-label col-md-2">
                        Start Date</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCurrentDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <label for="CalendarDuration" class="control-label col-md-2">
                        Calendar Duration</label>
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-6">
                                <asp:DropDownList ID="ddlDuration" runat="server" TabIndex="2" CssClass="form-control">
                                    <asp:ListItem Text="1 Week" Value="7" />
                                    <asp:ListItem Text="2 Week" Value="14" />
                                    <asp:ListItem Text="1 Month" Value="30" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6 col-padding-left-none">
                                <asp:Button ID="btnViewCalender" runat="server" class="TransactionalButton btn btn-primary btn-sm" TabIndex="3"
                                    Text="View Calendar" OnClick="btnViewCalender_Click" />
                                <input id="btnView" type="button" class="btn btn-primary btn-sm" value="View Calendar"
                                    style="display: none;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div style="display: none;">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblFilterBy" runat="server" class="control-label" Text="Filter By"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFilterBy" CssClass="form-control" runat="server">
                                <asp:ListItem Text="Available" Value="0" />
                                <asp:ListItem Text="Reserved" Value="0" />
                                <asp:ListItem Text="Out Of Service" Value="0" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="HMContainerRowButton">
                    </div>
                </div>
            </div>
        </div>
        <div class="childDivSection">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Room Calendar Information
                </div>
                <div class="panel-body">
                    <asp:Panel ID="pnlRoomCalender" runat="server">
                        <div id="parent" runat="server">
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <div id="ReservationPopUp" style="display: none;">
        <div id="PopTabPanel" style="width: 900px">
            <div id="PopReservation">
                <ul id="PoptabPage" class="ui-style">
                    <li id="PopA" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-1">Reservation Information</a></li>
                    <li id="PopB" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-2">Guest Information</a></li>
                    <li id="PopC" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-3">Airport Pick Up & Drop</a></li>
                    <li id="PopD" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-4">Complimentary Item</a></li>
                </ul>
                <div id="Poptab-1">
                    <div id="ReservationDetails" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Reservation Information
                        </div>
                        <div id="ReservationInfo">
                        </div>
                    </div>
                </div>
                <div id="Poptab-2">
                    <div id="ReservationGuest" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Guest Information
                        </div>
                        <div id="ReservationGuestInfo">
                        </div>
                    </div>
                </div>
                <div id="Poptab-3">
                    <div id="ReservationPicUp" class="row">
                        <div class="col-md-6">
                            <div id="MonthlySalaryDateSchedulePanel" class="panel panel-default">
                                <div class="panel-heading">
                                    Airport Pick Up Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblResAraivalFlight" runat="server" class="control-label" Text="Airline Name"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblResAraivalFlightText" runat="server" class="control-label" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblResAraivalFlightNumber" runat="server" class="control-label" Text="Flight Number"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblResAraivalFlightNumberText" class="control-label" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblResAraivalTime" runat="server" class="control-label" Text="Time"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblResAraivalTimeText" class="control-label" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div id="EmployeeBasicPanel" class="panel panel-default">
                                <div class="panel-heading">
                                    Airport Drop Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblResDepartaureFlight" runat="server" class="control-label" Text="Airline Name"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblResDepartaureFlightText" runat="server" class="control-label" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblResDepartureFlightNumber" runat="server" class="control-label"
                                                    Text="Flight Number"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblResDepartureFlightNumbertxt" class="control-label" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblResDepartureTime" runat="server" class="control-label" Text="Time"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblResDepartureTimeText" class="control-label" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Poptab-4">
                    <div id="ReservationComplementary" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Complementary Item Information
                        </div>
                        <div id="ReservationComplementaryInfo">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <button type="button" id="btnReservationBack" onclick="javascript:return CloseReservationPopUp()"
                            class="btn btn-primary btn-sm">
                            Back</button>
                    </div>
                    <div class="col-md-6" style="text-align: right;">
                        <div id="Div3">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="RegistrationPopUp" style="display: none;">
        <div id="PopRegistrationParent" style="width: 890px; margin-left: 5px; margin-top: 5px;">
            <div id="PopRegistration">
                <ul id="Ul1" class="ui-style">
                    <li id="Li1" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                        href="#RegPoptab-1">Registration Information</a></li>
                    <li id="Li2" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                        href="#RegPoptab-2">Guest Information</a></li>
                    <li id="Li3" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                        href="#RegPoptab-3">Airport Pick Up & Drop</a></li>
                    <li id="Li4" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                        href="#RegPoptab-4">Complimentary Item</a></li>
                </ul>
                <div id="RegPoptab-1">
                    <div id="RegistrationDetails" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Registration Information
                        </div>
                        <div id="RegistrationInfo">
                        </div>
                    </div>
                </div>
                <div id="RegPoptab-2">
                    <div id="RegistrationGuest" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Guest Information
                        </div>
                        <div id="RegistrationGuestInfo">
                        </div>
                    </div>
                </div>
                <div id="RegPoptab-3">
                    <div class="row">
                        <div class="span4 col-md-6">
                            <div id="Div1" class="panel panel-default">
                                <div class="panel-heading">
                                    Airport Pick Up Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Airline Name"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="Label2" runat="server" class="control-label" Text="">:</asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Flight Number"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="Label4" class="control-label" runat="server">:</asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Time"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="Label6" class="control-label" runat="server">:</asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="span4 col-md-6">
                            <div id="Div2" class="panel panel-default">
                                <div class="panel-heading">
                                    Airport Drop Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblRegDepartureFlightName" runat="server" class="control-label" Text="Airline Name"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblRegDepartureFlightNameText" runat="server" class="control-label"
                                                    Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblRegDepartureFlightNumber" runat="server" class="control-label"
                                                    Text="Flight Number"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblRegDepartureFlightNumberText" class="control-label" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblRegDepartureTime" runat="server" class="control-label" Text="Time"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblRegDepartureTimeText" class="control-label" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="RegPoptab-4">
                    <div id="RegistrationComplementary" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">
                            Complementary Item Information
                        </div>
                        <div id="RegistrationComplementaryInfo">
                        </div>
                    </div>
                </div>
                <button type="button" id="btnRegistrationBack" onclick="javascript:return CloseRegistrationPopUp()"
                    class="btn btn-primary btn-sm" style="float: right; margin-top: 7px;">
                    Back</button>
            </div>

            <div class="panel-body" style="display: none">
                <div class="form-horizontal" style="display: none">
                    <div class="form-group">
                        <div class="col-md-4">
                            <asp:Label ID="lblRegCommingFrom" runat="server" class="control-label" Text="Comming Form"></asp:Label>
                        </div>
                        <div class="col-md-8">
                            <asp:Label ID="lblRegCommingFromText" runat="server" class="control-label" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4">
                            <asp:Label ID="lblRegNextDestination" runat="server" class="control-label" Text="Next Destination"></asp:Label>
                        </div>
                        <div class="col-md-8">
                            <asp:Label ID="lblRegNextDestinationText" class="control-label" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4">
                            <asp:Label ID="lblRegVisitPurpose" runat="server" class="control-label" Text="Visit Purpose"></asp:Label>
                        </div>
                        <div class="col-md-8">
                            <asp:Label ID="lblRegVisitPurposeText" class="control-label" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
