<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmInhouseGroupManagement.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmInhouseGroupManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
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

        function PerformSearchButton() {
            var reservationId = $("#ContentPlaceHolder1_ddlGroupInformation").val();
            $("#ExpressCheckInnGridContainer").show();
            CommonHelper.SpinnerOpen();
            PageMethods.ExpressCheckInnGridInformation(reservationId, OnLoadExpressCheckInnGridInformationSucceed, OnLoadExpressCheckInnGridInformationFailed);
            return false;
        }
        function OnLoadExpressCheckInnGridInformationSucceed(result) {
            if (result != null) {
                if (result.ExpressCheckInnDetailsGrid != null) {
                    $("#ExpressCheckInnGridContainer").html(result.ExpressCheckInnDetailsGrid);
                    $("#ltlCalenderControl").html(result.ExpressCheckInnCalenderDetailsGrid);
                    $("#ExpressCheckInDiv").show();

                    $("#ContentPlaceHolder1_pnlExpressCheckInnGrid").show();
                    $("#ContentPlaceHolder1_pnlRoomCalender").show();
                }
                else {
                    $("#ExpressCheckInnGridContainer").html("");
                    $("#ltlCalenderControl").html("");
                    $("#ExpressCheckInDiv").hide();
                    $("#ContentPlaceHolder1_pnlExpressCheckInnGrid").hide();
                    $("#ContentPlaceHolder1_pnlRoomCalender").hide();
                }
            }
            CommonHelper.SpinnerClose();
        }
        function OnLoadExpressCheckInnGridInformationFailed() { CommonHelper.SpinnerClose(); }

        function CheckInputValue(txtQuantity, detailRowId) {
            if ($(txtQuantity).val() != "") {
                var quantity = $(txtQuantity).val();
                if (CommonHelper.IsInt(quantity) == false) {
                    toastr.warning("Please Provide Valid Room Number.");
                    $(txtQuantity).val("");
                    quantity = "";
                    return false;
                }
                else {
                    PageMethods.GetRoomStatusByRoomNumber(quantity, detailRowId, OnRoomStatusByRoomNumberSucceeded, OnRoomStatusByRoomNumberFailed);
                }
            }
        }

        function OnRoomStatusByRoomNumberSucceeded(result) {
            if (result.StatusId == 2) {
                $("#txtGuest" + result.detailRowId).val(result.GuestName);
                //$("#txt" + result.detailRowId).focus();
            }
            else if (result.StatusId != 2) {
                $("#txt" + result.detailRowId).val("");
                $("#txt" + result.detailRowId).val("");
                toastr.warning("Your Entered Room Number " + result.RoomNumber + " is " + result.StatusName);
                $("#txt" + result.detailRowId).focus();
            }
            CommonHelper.SpinnerClose();
            return false;
        }

        function OnRoomStatusByRoomNumberFailed() { CommonHelper.SpinnerClose(); }

        function ValidationBeforeExpressCheckIn() {
            var IsPermitForSave = confirm("Do You Want To Save?");
            if (IsPermitForSave == true) {
                CommonHelper.SpinnerOpen();
                var reservationId = "0", guestName = "", roomNumber = "", roomRate = "", unitPrice = "", discountType = "", discountAmount = "", roomId = "", currencyType = "", conversionRate = "", dateIn = "", dateOut = "";
                reservationId = $("#ContentPlaceHolder1_ddlGroupInformation").val();

                var RoomReservationDetails = [];

                $("#ExpressCheckInDetailsGrid tbody tr").each(function (index, item) {
                    var chkBox = $(item).find("td:eq(1)").find('input');                    
                    roomNumber = $(item).find("td:eq(2)").find('input').val();
                    guestName = $(item).find("td:eq(3)").text();
                    roomRate = $(item).find("td:eq(4)").text();
                    unitPrice = $(item).find("td:eq(5)").text();
                    discountType = $(item).find("td:eq(6)").text();
                    discountAmount = $(item).find("td:eq(7)").text();
                    roomId = $(item).find("td:eq(8)").text();
                    currencyType = $(item).find("td:eq(9)").text();
                    conversionRate = $(item).find("td:eq(10)").text();
                    dateIn = $(item).find("td:eq(11)").text();
                    dateOut = $(item).find("td:eq(12)").text();

                    if ($(chkBox).is(":checked")) {
                        if (roomNumber != "") {
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
                                DateOut: dateOut
                            });
                        }
                    }
                });

                //if (RoomReservationDetails.length == 0 && reservationId == "0") {
                //    toastr.info("Please Provide Valid Check-In Information");
                //    CommonHelper.SpinnerClose();
                //    return false;
                //}

                PageMethods.SaveUpdateGroupAssignment(reservationId, RoomReservationDetails, OnSaveUpdateGroupAssignmentSucceeded, OnSaveUpdateGroupAssignmentFailed);
                return false;
            } else {
                CommonHelper.SpinnerClose();
                return false;

            }
        }
        function OnSaveUpdateGroupAssignmentSucceeded(result) {
            PerformSearchButton();
            toastr.info('Successfully Group/ Company Updated.')
            CommonHelper.SpinnerClose();
            return false;
        }

        function OnSaveUpdateGroupAssignmentFailed() { CommonHelper.SpinnerClose(); }

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
    </script>
     <div id="serviceDecider" style="display: none;">
        <div id="serviceDeciderHtml">
        </div>
    </div>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            In-house Group Management
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
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
                <%--<div class="form-group">
                    <div id="ExpressCheckInnGridContainer">
                    </div>
                </div>--%>
                <div class="form-group">
                    <div class="col-md-4">
                        <asp:Panel ID="pnlExpressCheckInnGrid" runat="server" ScrollBars="Both" Height="1000px">
                            <div id="ExpressCheckInnGridContainer">
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="col-md-8">
                        <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="1000px">
                            <div id="ltlCalenderControl">
                            </div>
                        </asp:Panel>
                    </div>

                </div>
                <div id="ExpressCheckInDiv" class="row" style="display: none">
                    <div class="col-md-12">
                        <asp:Button ID="btnExpressCheckIn" runat="server" Text="Update Group" TabIndex="24" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript:return ValidationBeforeExpressCheckIn();" />
                        <asp:Button ID="btnClear" runat="server" TabIndex="4" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
