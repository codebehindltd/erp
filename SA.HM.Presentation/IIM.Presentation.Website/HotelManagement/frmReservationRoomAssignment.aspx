<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmReservationRoomAssignment.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmReservationRoomAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        #parent {
            height: 400px;
        }
    </style>
    <script type="text/javascript">

        var RoomTypeWiseQuantity = new Array();

        $(document).ready(function () {
            $("#ContentPlaceHolder1_pnlExpressCheckInnGrid").hide();
            $("#ContentPlaceHolder1_pnlRoomCalender").hide();
            $(document).on('keypress', 'input,select', function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    var $next = $('[tabIndex=' + (+this.tabIndex + 1) + ']');
                    console.log($next.length);
                    if (!$next.length) {
                        $next = $('[tabIndex=1]');
                    }
                    $next.focus();
                }
            });

            $("#btnReservationSearch").click(function () {
                var check = true;
                $("#ContentPlaceHolder1_chkAllActiveReservation").attr("checked", true);
                //PopulateReservation(1);
                var guestName = $("#<%=txtResvGuestName.ClientID %>").val();
                var companyName = $("#<%=txtResvCompanyName.ClientID %>").val();
                var reservNumber = $("#<%=txtReservationNo.ClientID %>").val();
                var checkInDate = $("#<%=txtRsvCheckInDate.ClientID %>").val();
                var checkOutDate = $("#<%=txtCheckOutDate.ClientID %>").val();

                if (guestName == "" && companyName == "" && reservNumber == "" && checkInDate == "" && checkOutDate == "") {
                    check = false;
                }
                if (!check) {
                    toastr.warning("Please Give Atleast One Information");
                    return check;
                }

                LoadReservationInformation();
            });

            $("#ContentPlaceHolder1_txtRsvCheckInDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,

            });
            $("#ContentPlaceHolder1_txtCheckOutDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,

            });
        });

        function LoadReservationInformation() {
            var guestName = $.trim($("#<%=txtResvGuestName.ClientID %>").val());
            var companyName = $.trim($("#<%=txtResvCompanyName.ClientID %>").val());
            var reservNumber = $.trim($("#<%=txtReservationNo.ClientID %>").val());
            var checkInDate = $.trim($("#<%=txtRsvCheckInDate.ClientID %>").val());
            var checkOutDate = $.trim($("#<%=txtCheckOutDate.ClientID %>").val());

            PageMethods.SearchNLoadReservationInfo(0, guestName, companyName, reservNumber, checkInDate, checkOutDate, OnLoadReservInfoSucceeded, OnLoadReservInfoFailed);
            return false;
        }
        function OnLoadReservInfoSucceeded(result) {
            $("#ltlReservationInformation").html(result);
            return false;
        }
        function OnLoadReservInfoFailed(error) {
        }

        function PerformSearchButton() {
            var srcReservationNumber = $.trim($("#ContentPlaceHolder1_txtSrcReservationNumber").val());

            if (srcReservationNumber == "") {
                toastr.warning("Please Provide Valid Reservation Number.");
                $("#ContentPlaceHolder1_txtSrcReservationNumber").val("");
                $("#ContentPlaceHolder1_txtSrcReservationNumber").focus();
                return false;
            }

            $("#ExpressCheckInnGridContainer").show();
            CommonHelper.SpinnerOpen();
            PageMethods.ExpressCheckInnGridInformation(srcReservationNumber, OnLoadExpressCheckInnGridInformationSucceed, OnLoadExpressCheckInnGridInformationFailed);
            return false;
        }
        function OnLoadExpressCheckInnGridInformationSucceed(result) {
            if (result.ReservationId == 0) {
                CommonHelper.SpinnerClose();
                toastr.warning("Please enter valid reservation number.");
                $("#ContentPlaceHolder1_txtSrcReservationNumber").val("");
                return false;
            }
            if (result != null) {
                if (result.ReservationNumber) {
                    $("#ContentPlaceHolder1_lblReservationNumber").text("Reservation No.: " + result.ReservationNumber);

                    if (result.Remarks != "") {
                        $("#ContentPlaceHolder1_lblHotelRemarks").text("=> Hotel Remarks: " + result.Remarks);
                    }
                    else {
                        $("#ContentPlaceHolder1_lblHotelRemarks").text("");
                    }
                }
                else {
                    $("#ContentPlaceHolder1_lblReservationNumber").text("");
                }

                if (result.ExpressCheckInnDetailsGrid != null) {
                    $("#ExpressCheckInnGridContainer").html(result.ExpressCheckInnDetailsGrid);
                    $("#parent").html(result.ExpressCheckInnCalenderDetailsGrid);
                    $("#ReservationDetailGrid").html(result.ReservationDetailGrid);
                    $("#ReservationDetailGrid").show();
                    $("#ExpressCheckInDiv").show();
                    RoomTypeWiseQuantity = result.DuplicateCheck;

                    $("#ContentPlaceHolder1_pnlExpressCheckInnGrid").show();
                    $("#ContentPlaceHolder1_pnlRoomCalender").show();
                }
                else {
                    $("#ExpressCheckInnGridContainer").html("");
                    $("#parent").html("");
                    $("#ReservationDetailGrid").html("");
                    $("#ReservationDetailGrid").hide();
                    $("#ExpressCheckInDiv").hide();
                    $("#ContentPlaceHolder1_pnlExpressCheckInnGrid").hide();
                    $("#ContentPlaceHolder1_pnlRoomCalender").hide();
                }
            }

            $("#fixTable").tableHeadFixer();
            CommonHelper.SpinnerClose();
        }
        function OnLoadExpressCheckInnGridInformationFailed() { CommonHelper.SpinnerClose(); }

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

        function CheckInputValue(txtRoomNumber, reservationId, roomTypeId, detailRowId, index) {
            $("#fixTable tbody tr").each(function (index, item) {

            }).show();
            if ($(txtRoomNumber).val() != "") {

                var quantity = $(txtRoomNumber).val();
                if (CommonHelper.IsInt(quantity) == false) {
                    toastr.warning("Please Provide Valid Room Number.");
                    $(txtRoomNumber).val("");
                    quantity = "";
                    return false;
                }
                else {

                    if ($.trim($(txtRoomNumber).val()) == "") {
                        RoomTypeWiseQuantity[parseInt(index)].RoomNumber = "";
                        return false;
                    }

                    RoomTypeWiseQuantity[parseInt(index)].RoomNumber = $(txtRoomNumber).val();
                    var PaxQuantity = RoomTypeWiseQuantity[parseInt(index)].PaxQuantity;
                    var roomQuantity = RoomTypeWiseQuantity[parseInt(index)].RoomQuantity;
                    var roomType = RoomTypeWiseQuantity[parseInt(index)].RoomType + " (" + RoomTypeWiseQuantity[parseInt(index)].RoomTypeCode + ")";

                    var AssignedRoom = _.where(RoomTypeWiseQuantity, { RoomNumber: $.trim($(txtRoomNumber).val()) });
                    var totalAssignedRoom = AssignedRoom.length;

                    if (totalAssignedRoom > PaxQuantity) {
                        toastr.info("In " + roomType + " Room, Maximum " + PaxQuantity + " Guest Can Stay. Room Assignment Exceeded The Max Quantity.");
                        RoomTypeWiseQuantity[parseInt(index)].RoomNumber = "";
                        $(txtRoomNumber).val("");
                        $(txtRoomNumber).focus();
                        return false;
                    }

                    ///check assigned room quantity
                    var AssignedRoomQuantity = 0;
                    AssignedRoom = _.filter(RoomTypeWiseQuantity, function (obj) { if (obj.RoomNumber != "" && obj.RoomNumber != null && obj.RoomTypeId == roomTypeId) return obj; });

                    AssignedRoom = _.groupBy(AssignedRoom, function (item) { return [item.RoomNumber].sort(); });

                    var countAssignedRoom = 0;

                    _.each(AssignedRoom, function (group) {
                        countAssignedRoom++;
                    });

                    if (countAssignedRoom > roomQuantity) {
                        toastr.info("Assigned Room quantity and reservation room quantity for " + roomType + " is not same:");
                        RoomTypeWiseQuantity[parseInt(index)].RoomNumber = "";
                        $(txtRoomNumber).val("");
                        $(txtRoomNumber).focus();
                        return false;
                    }

                    PageMethods.LoadRoomInformationWithControl($(txtRoomNumber).val(), reservationId, roomTypeId, detailRowId, index, OnRoomStatusByRoomNumberSucceeded, OnRoomStatusByRoomNumberFailed);
                }
            }
            else {
                if ($.trim($(txtRoomNumber).val()) == "") {
                    RoomTypeWiseQuantity[parseInt(index)].RoomNumber = "";
                    return false;
                }
            }
        }

        function OnRoomStatusByRoomNumberSucceeded(result) {
            if (result.RoomNumber != "Invalid") {
                $("#txt" + result.detailRowId).val("");
                toastr.info("Your Entered Room Number " + result.RoomNumber + " is Invalid for this Field.");
                $("#txt" + result.detailRowId).focus();
                RoomTypeWiseQuantity[parseInt(result.IndexId)].RoomNumber = "";
            }
            else {
                // RoomTypeWiseQuantity;
            }

            //if (result.StatusId != 1) {
            //    $("#txt" + result.detailRowId).val("");
            //    toastr.warning("Your Entered Room Number " + result.RoomNumber + " is Invalid for this Field.");
            //    $("#txt" + result.detailRowId).focus();
            //}
            CommonHelper.SpinnerClose();
            return false;
        }

        function OnRoomStatusByRoomNumberFailed() { CommonHelper.SpinnerClose(); }

        function ValidationBeforeExpressCheckIn() {
            var IsPermitForSave = confirm("Do You Want To Update Reservation?");
            if (IsPermitForSave == true) {

                CommonHelper.SpinnerOpen();
                var reservationId = "0", reservationDetailId = "", gustId = "", guestName = "", roomNumber = "", roomRate = "", unitPrice = "", discountType = "", discountAmount = "", roomId = "", currencyType = "", conversionRate = "", dateIn = "", dateOut = ""
                , roomType = "", typeWiseRoomQuantity = "", roomTypeId = "";
                //reservationId = $("#ContentPlaceHolder1_ddlGroupInformation").val();

                var RoomReservationDetails = [];

                $("#ExpressCheckInDetailsGrid tbody tr").each(function (index, item) {
                    guestName = $(item).find("td:eq(1)").find('input').val();
                    roomNumber = $(item).find("td:eq(2)").find('input').val();
                    roomRate = $(item).find("td:eq(3)").text();
                    unitPrice = $(item).find("td:eq(4)").text();
                    discountType = $(item).find("td:eq(5)").text();
                    discountAmount = $(item).find("td:eq(6)").text();
                    roomId = $(item).find("td:eq(7)").text();
                    currencyType = $(item).find("td:eq(8)").text();
                    conversionRate = $(item).find("td:eq(9)").text();
                    dateIn = $(item).find("td:eq(10)").text();
                    dateOut = $(item).find("td:eq(11)").text();
                    reservationDetailId = $(item).find("td:eq(12)").text();
                    gustId = $(item).find("td:eq(13)").text();
                    reservationId = $(item).find("td:eq(14)").text();
                    roomType = $(item).find("td:eq(16)").text();
                    typeWiseRoomQuantity = parseInt($(item).find("td:eq(17)").text().trim());
                    roomTypeId = parseInt($(item).find("td:eq(18)").text().trim());
                    ////toastr.info("Check");
                    //var validDateIn = CommonHelper.DateFormatToMMDDYYYY(dateIn, '/');
                    //var validDateOut = CommonHelper.DateFormatToMMDDYYYY(dateOut, '/');

                    if (guestName != "") {
                        RoomReservationDetails.push({
                            GuestName: guestName,
                            RoomNumber: roomNumber,
                            RoomRate: roomRate,
                            UnitPrice: unitPrice,
                            DiscountType: discountType,
                            DiscountAmount: discountAmount,
                            RoomId: roomId,
                            CurrencyType: currencyType,
                            ConversionRate: conversionRate,
                            DateIn: dateIn,
                            DateOut: dateOut,
                            ReservationDetailId: reservationDetailId,
                            GuestId: gustId,
                            ReservationId: reservationId,
                            RoomType: roomType,
                            TypeWiseRoomQuantity: typeWiseRoomQuantity,
                            RoomTypeId: roomTypeId
                        });
                    }
                });

                if (RoomReservationDetails.length == 0 && reservationId == "0") {
                    toastr.info("Please Provide Valid Check-In Information");
                    CommonHelper.SpinnerClose();
                    return false;
                }
                if ($("#ExpressCheckInDetailsGrid tbody tr").length > RoomReservationDetails.length) {
                    toastr.info("Enter All Guest Name");
                    CommonHelper.SpinnerClose();
                    return false;
                }
                PageMethods.SaveExpressCheckInn(RoomReservationDetails, OnSaveExpressCheckInnSucceeded, OnSaveExpressCheckInnFailed);
                return false;
            } else {
                CommonHelper.SpinnerClose();
                return false;

            }
        }
        function OnSaveExpressCheckInnSucceeded(result) {
            $("#ExpressCheckInDiv").hide();
            toastr.success('Room Assignment Successfully.')
            PerformSearchButton();
            CommonHelper.SpinnerClose();
            return false;
        }

        function OnSaveExpressCheckInnFailed() { CommonHelper.SpinnerClose(); }
        function RedirectToDetails(TransectionId, reservation, roomNumber, date) {
            if (reservation == 1) {
                ////Registration
                //PopUpRegistrationInformation();
                //GetRegistrationInformation(TransectionId);
                //GetRegistrationGuestInformation(TransectionId);
                //GetRegistrationAirportPicUpInformation(TransectionId);
                //GetRegistrationComplementaryInformation(TransectionId);

            }
            else if (reservation == 2) {
                ////reservation
                //PopUpReservationInformation();
                //GetReservationInformation(TransectionId);
                //GetReservationGuestInformation(TransectionId);
                //GetReservationAirportPicUpInformation(TransectionId)
                //GetReservationComplementaryInformation(TransectionId);
                //LoadRegistration(roomNumber, TransectionId, date);
            }
            else if (reservation == 3) {
                ////Out Of Service
                //var pageTitle = 'Out Of Order Room';
                //PageMethods.LoadOutOfOrderPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                //return false;
            }
            else if (reservation == 4) {
                ////Out Of Service
                //var pageTitle = 'Out Of Service Room';
                //PageMethods.LoadOutOfOrderPossiblePath(roomNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                //return false;
            }
        }

        function AssignReservationNumber(resultInfo) {
            $("#ContentPlaceHolder1_txtSrcReservationNumber").val(resultInfo);
            $("#ReservationPopup").dialog("close");
            PerformSearchButton();

        }

        function FilterRoomCalender(RoomType) {

            var RoomTypeName = $(RoomType).attr('placeholder');
            var IsHeaderVisible = false, lastHeaderIndex = -1, rowRoomType, length;

            if (RoomTypeName != "") {
                length = $("#fixTable tbody tr").length;
                $("#fixTable tbody tr").each(function (index, item) {

                    if ($(this).attr("id") == ("roomNumber" + (index + 1)));
                    {
                        rowRoomType = $(item).find("td:eq(1)").text();

                        if (rowRoomType != "") {
                            if (rowRoomType != RoomTypeName) {
                                $(item).hide();
                            }
                            else {
                                $(item).show();
                                IsHeaderVisible = true;
                            }
                        }
                        else {
                            if (!IsHeaderVisible)
                                $("#fixTable tbody tr:eq(" + lastHeaderIndex + ")").hide();
                            lastHeaderIndex = index;
                            IsHeaderVisible = false;
                        }

                    }

                    if (index == (length - 1) && !IsHeaderVisible)
                        $("#fixTable tbody tr:eq(" + lastHeaderIndex + ")").hide();
                });
            }


        }


    </script>

    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Reservation Room Assignment
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSrcReservationNumber" runat="server" class="control-label required-field" Text="Reservation Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <div class="input-group">
                            <asp:TextBox ID="txtSrcReservationNumber" Style="height: 27px;" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            <span class="input-group-addon" style="padding: 2px 2px;">
                                <asp:Button ID="btnReservationDetailSerach" Style="height: 20px; padding-top: unset" runat="server" Text="Search" Width="80" TabIndex="2"
                                    CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformSearchButton();" />
                            </span>
                            <span class="input-group-addon" style="padding: 2px 2px;">
                                <asp:ImageButton ID="imgReservationSearch" Style="height: 19px" Width="25" runat="server"
                                    OnClientClick="javascript:return SearchReservation()"
                                    ImageUrl="~/Images/SearchItem.png" ToolTip="More Search" />
                            </span>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="lblReservationNumber" runat="server" class="control-label" ForeColor="Red" Text=""></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-12" style="text-align:left;">
                        <asp:Label ID="lblHotelRemarks" runat="server" ForeColor="blue" Text=""></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-12">
                        <div id="ReservationDetailGrid" style="padding-top: 10px; display: none;">
                            <table class="table table-bordered table-condensed table-responsive" id='ReservationRoomGrid'>
                                <thead>
                                    <tr style='color: White; background-color: #44545E; font-weight: bold;'>
                                        <th style="width: 30%;">Room Type
                                        </th>
                                        <th style="width: 50%;">Room Numbers
                                        </th>
                                        <th style="width: 12%;">Pax
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4">
                        <asp:Panel ID="pnlExpressCheckInnGrid" runat="server" ScrollBars="Both" Height="400px">
                            <div id="ExpressCheckInnGridContainer">
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="col-md-8">
                        <asp:Panel ID="pnlRoomCalender" runat="server">
                            <div id="parent">
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                <div id="ExpressCheckInDiv" class="row" style="display: none">
                    <div class="col-md-12">
                        <asp:Button ID="btnExpressCheckIn" runat="server" Text="Update Reservation" TabIndex="24" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript:return ValidationBeforeExpressCheckIn();" />
                        <asp:Button ID="btnClear" runat="server" TabIndex="4" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
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
                        <label for="Country" class="control-label col-md-2">
                            Guest Name</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtResvGuestName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="Country" class="control-label col-md-2">
                            Company Name</label>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtResvCompanyName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="Country" class="control-label col-md-2">
                            Check-In Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtRsvCheckInDate" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                        </div>
                        <label for="Country" class="control-label col-md-2">
                            Reservation No.</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtReservationNo" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                        </div>
                        <label for="Country" class="control-label col-md-2" style="display: none;">
                            Check-Out Date</label>
                        <div class="col-md-4" style="display: none;">
                            <asp:TextBox ID="txtCheckOutDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
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
    <script type="text/javascript">
        var rsvnReservationId = '<%=rsvnReservationId%>';
        if (rsvnReservationId > -1) {
            PerformSearchButton();
        }
    </script>
</asp:Content>
