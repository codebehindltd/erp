<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmExpressCheckIn.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmExpressCheckIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        #parent {
            height: 400px;
        }
    </style>
    <script src="../../JSLibrary/TableHeadFixer/tableHeadFixer.js"></script>
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

        });
        function CheckAll() {
            if ($("#chkAll").is(":checked")) {
                $("#ExpressCheckInDetailsGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#ExpressCheckInDetailsGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
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

            var roomTypeId = 0;
            var paramCheckOutType = CommonHelper.GetParameterByName("rtId");
            if (paramCheckOutType != "") {
                if (paramCheckOutType > 0) {
                    roomTypeId = paramCheckOutType;
                }
            }

            $("#ExpressCheckInnGridContainer").show();
            CommonHelper.SpinnerOpen();
            PageMethods.ExpressCheckInnGridInformation(srcReservationNumber, roomTypeId, OnLoadExpressCheckInnGridInformationSucceed, OnLoadExpressCheckInnGridInformationFailed);
            return false;
        }

        function OnLoadExpressCheckInnGridInformationSucceed(result) {
            if (result != null) {
                if (result.ExpressCheckInnDetailsGrid != null) {
                    $("#ExpressCheckInnGridContainer").html(result.ExpressCheckInnDetailsGrid);
                    $("#parent").html(result.ExpressCheckInnCalenderDetailsGrid);
                    $("#ReservationDetailGrid").html(result.ReservationDetailGrid);
                    $("#ReservationDetailGrid").show();
                    $("#ExpressCheckInDiv").show();
                    RoomTypeWiseQuantity = result.DuplicateCheck;
                    $("#ContentPlaceHolder1_pnlExpressCheckInnGrid").show();
                    $("#ContentPlaceHolder1_pnlRoomCalender").show();
                    $("#fixTable").tableHeadFixer();
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

            CommonHelper.SpinnerClose();
        }
        function OnLoadExpressCheckInnGridInformationFailed() { CommonHelper.SpinnerClose(); }

        function CheckInputValue(txtRoomNumber, reservationId, roomTypeId, detailRowId, index) {
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
                    var roomQuantity = RoomTypeWiseQuantity[parseInt(index)].RoomQuantity;
                    var PaxQuantity = RoomTypeWiseQuantity[parseInt(index)].PaxQuantity;
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

                    var duplicates = [], singles = [];

                    _.each(AssignedRoom, function (group) {
                        if (group.length > 1) {
                            duplicates.push.apply(duplicates, group);
                        } else {
                            singles.push(group[0]);
                        }
                    });

                    AssignedRoomQuantity = singles.length;

                    if (AssignedRoomQuantity > roomQuantity) {
                        toastr.info("Assigned Room quantity and reservation room quantity for " + roomType + " is not same:");
                        RoomTypeWiseQuantity[parseInt(index)].RoomNumber = "";
                        $(txtRoomNumber).val("");
                        $(txtRoomNumber).focus();
                        return false;
                    }

                    PageMethods.GetRoomStatusByRoomNumber(quantity, detailRowId, OnRoomStatusByRoomNumberSucceeded, OnRoomStatusByRoomNumberFailed);
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
            if (result.StatusId != 1) {
                $("#txt" + result.detailRowId).val("");
                toastr.warning("Your Entered Room Number " + result.RoomNumber + " is " + result.StatusName);
                $("#txt" + result.detailRowId).focus();
                RoomTypeWiseQuantity[parseInt(result.IndexId)].RoomNumber = "";
            }
            CommonHelper.SpinnerClose();
            return false;
        }

        function OnRoomStatusByRoomNumberFailed() { CommonHelper.SpinnerClose(); }

        function ValidationBeforeExpressCheckIn() {
            var IsPermitForSave = confirm("Do You Want To Check-In?");
            var IsCheckedMinimumOneGuest = false;
            if (IsPermitForSave == true) {                
                var reservationId = "0", guestName = "", roomNumber = "", roomRate = "", unitPrice = "", discountType = "", discountAmount = "", roomId = "", currencyType = "", conversionRate = "", dateIn = "", dateOut = "", transactionType = "Save"
                , roomType = "", typeWiseRoomQuantity = "", roomTypeId = "";;

                var RoomReservationDetails = [];
                $("#ExpressCheckInDetailsGrid tbody tr").each(function (index, item) {
                    guestName = $(item).find("td:eq(2)").find('input').val();
                    roomNumber = $(item).find("td:eq(3)").find('input').val();
                    roomRate = $(item).find("td:eq(4)").text();
                    unitPrice = $(item).find("td:eq(5)").text();
                    discountType = $(item).find("td:eq(6)").text();
                    discountAmount = $(item).find("td:eq(7)").text();
                    roomId = $(item).find("td:eq(8)").text();
                    currencyType = $(item).find("td:eq(9)").text();
                    conversionRate = $(item).find("td:eq(10)").text();
                    dateIn = $(item).find("td:eq(11)").text();
                    dateOut = $(item).find("td:eq(12)").text();
                    reservationDetailId = $(item).find("td:eq(13)").text();
                    gustId = $(item).find("td:eq(14)").text();
                    reservationId = $(item).find("td:eq(15)").text();
                    roomType = $(item).find("td:eq(17)").text();
                    typeWiseRoomQuantity = parseInt($(item).find("td:eq(18)").text().trim());
                    roomTypeId = parseInt($(item).find("td:eq(19)").text().trim());

                    if ($(item).find("td:eq(0)").find('input').is(':checked')) {
                        transactionType = "Save";
                        IsCheckedMinimumOneGuest = true;
                    } else {

                        transactionType = "Update";
                    }

                    if ((transactionType == "Save" && roomNumber != "" && guestName != "") || (transactionType == "Update" && guestName != "")) {
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
                            TransactionType: transactionType,
                            RoomType: roomType,
                            TypeWiseRoomQuantity: typeWiseRoomQuantity,
                            RoomTypeId: roomTypeId
                        });
                    }

                });

                if (!IsCheckedMinimumOneGuest) {
                    toastr.info("Please select a guest for check in.");
                    return false;
                }

                if ($("#ExpressCheckInDetailsGrid tbody tr").length > RoomReservationDetails.length) {
                    toastr.info("Enter All Guest Name and Room Number");
                    CommonHelper.SpinnerClose();
                    return false;
                }

                CommonHelper.SpinnerOpen();

                PageMethods.SaveExpressCheckInn(reservationId, RoomReservationDetails, OnSaveExpressCheckInnSucceeded, OnSaveExpressCheckInnFailed);
                return false;
            } else {
                CommonHelper.SpinnerClose();
                return false;
            }
        }
        function OnSaveExpressCheckInnSucceeded(result) {
            $("#ExpressCheckInDiv").hide();
            PerformSearchButton();
            toastr.success('Successfully Express Checked-In.')
            CommonHelper.SpinnerClose();
            return false;
        }

        function OnSaveExpressCheckInnFailed() { CommonHelper.SpinnerClose(); }
        function RedirectToDetails(TransectionId, reservation, roomNumber, date) {
        }
    </script>
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
                            Check-Out Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtCheckOutDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
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
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Express Check In Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblGroupName" runat="server" class="control-label required-field" Text="Group/ Company Name"></asp:Label>
                    </div>
                    <div class="col-md-8">
                        <asp:DropDownList ID="ddlGroupInformation" runat="server" CssClass="form-control" TabIndex="2">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnReservationDetailSerach" runat="server" TabIndex="4" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformSearchButton();" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSrcReservationNumber" runat="server" class="control-label required-field" Text="Reservation Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <div class="input-group">
                            <asp:TextBox ID="txtSrcReservationNumber" Style="height: 27px;" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            <span class="input-group-addon" style="padding: 2px 2px;">
                                <asp:Button ID="btnSearch" Style="height: 20px; padding-top: unset" runat="server" Text="Search" Width="80" TabIndex="2"
                                    CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformSearchButton();" />
                            </span>
                            <span class="input-group-addon" style="padding: 2px 2px; display: none;">
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
                        <asp:Panel ID="pnlRoomCalender" runat="server" Height="400px">
                            <div id="parent">
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                <div id="ExpressCheckInDiv" class="row" style="display: none">
                    <div class="col-md-12">
                        <asp:Button ID="btnExpressCheckIn" runat="server" Text="Check-In" TabIndex="24" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript:return ValidationBeforeExpressCheckIn();" />
                        <asp:Button ID="btnClear" runat="server" TabIndex="4" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" />
                    </div>
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
