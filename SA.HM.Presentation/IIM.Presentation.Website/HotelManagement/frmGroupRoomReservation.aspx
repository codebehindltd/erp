<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGroupRoomReservation.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmGroupRoomReservation" %>

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

            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtFromDate").datepicker("option", "maxDate", selectedDate);
                }
            });

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

        function PerformRoomReservationSearchButton() {
           <%-- var guestName = $.trim($("#<%=txtResvGuestName.ClientID %>").val());
            var companyName = $.trim($("#<%=txtResvCompanyName.ClientID %>").val());
            var reservNumber = $.trim($("#<%=txtReservationNo.ClientID %>").val());--%>
            var checkInDate = $.trim($("#<%=txtFromDate.ClientID %>").val());
            var checkOutDate = $.trim($("#<%=txtToDate.ClientID %>").val());

            if (checkInDate == "") {
                toastr.warning("Please Provide Search Date (From Date).");
                $("#ContentPlaceHolder1_txtFromDate").val("");
                $("#ContentPlaceHolder1_txtFromDate").focus();
                return false;
            }

            if (checkOutDate == "") {
                toastr.warning("Please Provide Search Date (To Date).");
                $("#ContentPlaceHolder1_txtToDate").val("");
                $("#ContentPlaceHolder1_txtToDate").focus();
                return false;
            }

            $("#RoomReservationGridContainer").show();
            CommonHelper.SpinnerOpen();
            PageMethods.GetRoomReservationInfoByStringSearchCriteria(checkInDate, checkOutDate, OnLoadRoomReservationGridInformationSucceed, OnLoadRoomReservationGridInformationFailed);
            return false;
        }

        function OnLoadRoomReservationGridInformationSucceed(result) {
            $("#GroupReservationDetailDiv").show();
            $("#RoomReservationGridContainer").html(result.RoomReservationGrid);
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadRoomReservationGridInformationFailed(error) {

            CommonHelper.SpinnerClose();
        }

        function ValidationBeforeGroupReservationPosting() {
            
            var IsPermitForSave = confirm("Do You Want To Psting Group Reservation?");
            var IsCheckedMinimumOneReservation = false;
            if (IsPermitForSave == true) {
                var groupId = "0", reservationId = "0", guestName = "", roomNumber = "", roomRate = "", unitPrice = "", discountType = "", discountAmount = "", roomId = "", currencyType = "", conversionRate = "", dateIn = "", dateOut = "", transactionType = "Save"
                    , roomType = "", typeWiseRoomQuantity = "", roomTypeId = "";

                groupId = 1;

                var GroupRoomReservationDetails = [];
                $("#RoomReservationGrid tbody tr").each(function (index, item) {
                    reservationId = $(item).find("td:eq(7)").text();

                    if ($(item).find("td:eq(0)").find('input').is(':checked')) {
                        transactionType = "Save";
                        IsCheckedMinimumOneReservation = true;
                    } else {

                        transactionType = "Update";
                    }

                    if ((transactionType == "Save") || (transactionType == "Update")) {
                        GroupRoomReservationDetails.push({                            
                            ReservationId: reservationId
                        });
                    }

                });

                if (!IsCheckedMinimumOneReservation) {
                    toastr.info("Please select minimum one reservation.");
                    return false;
                }

                if ($("#RoomReservationGrid tbody tr").length > GroupRoomReservationDetails.length) {
                    toastr.info("Please select minimum one reservation.");
                    CommonHelper.SpinnerClose();
                    return false;
                }
                debugger;
                CommonHelper.SpinnerOpen();

                PageMethods.SaveOrUpdateGroupRoomReservation(groupId, GroupRoomReservationDetails, OnSaveGroupReservationPostingSucceeded, OnSaveGroupReservationPostingFailed);
                return false;
            } else {
                CommonHelper.SpinnerClose();
                return false;
            }
        }
        function OnSaveGroupReservationPostingSucceeded(result) {
            $("#GroupReservationDetailDiv").hide();
            toastr.success('Successfully Updated Group Reservation.')
            CommonHelper.SpinnerClose();
            return false;
        }

        function OnSaveGroupReservationPostingFailed() { CommonHelper.SpinnerClose(); }
        function RedirectToDetails(TransectionId, reservation, roomNumber, date) {
        }
        function ToggleListedGroupInfo() {
            var ctrl = '#<%=chkIsLitedGroup.ClientID%>';

            if ($(ctrl).is(':checked')) {
                $('#ReservedContact').hide("slow");
                $('#ListedContact').show("slow");
            }
            else {

                $('#ListedContact').hide("slow");
                $('#ReservedContact').show("slow");
                $(ctrl).prop('checked', false)
                $("#ContentPlaceHolder1_hfContactId").val("0");
            }
        }
    </script>
    <div class="panel panel-default">
        <div class="panel-heading">
            Search Room Reservation
        </div>
        <div class="panel-body">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label for="FromDate" class="control-label col-md-2">
                            Search Date</label>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" placeholder="From Date" TabIndex="63"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="64" CssClass="form-control" placeholder="To Date"></asp:TextBox>
                        </div>
                        <label for="ReservationNumber" class="control-label col-md-2">
                            Reservation Number</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSearchReservationNumber" runat="server" CssClass="form-control"
                                TabIndex="66"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="GuestName" class="control-label col-md-2">
                            Guest Name</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcReservationGuest" runat="server" CssClass="form-control" TabIndex="65"></asp:TextBox>
                        </div>
                        <label for="CompanyName" class="control-label col-md-2">
                            Company Name</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSearchCompanyName" runat="server" CssClass="form-control" TabIndex="66"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="ContactPerson" class="control-label col-md-2">
                            Contact Person</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtCntPerson" runat="server" CssClass="form-control" TabIndex="67"></asp:TextBox>
                        </div>
                        <label for="MobileNumber" class="control-label col-md-2">
                            Phone/ Mobile No.</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcContactPhone" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="Reference" class="control-label col-md-2">
                            Contact Email</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcContactEmail" runat="server" CssClass="form-control" TabIndex="41"></asp:TextBox>
                        </div>
                        <label for="GuestSource" class="control-label col-md-2">
                            Guest Source</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSrcGuestSource" runat="server" CssClass="form-control" TabIndex="64">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <%--<div class="form-group">
                        <label for="Reference" class="control-label col-md-2">
                            Market Segment</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSrcMarketSegment" runat="server" CssClass="form-control" TabIndex="20">
                            </asp:DropDownList>
                        </div>
                        <label for="Reference" class="control-label col-md-2">
                            Reference</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSrcReferenceId" runat="server" CssClass="form-control" TabIndex="20">
                            </asp:DropDownList>
                        </div>
                    </div>--%>
                    <%--<div class="form-group">
                        <label for="SearchOrdering" class="control-label col-md-2">
                            Search Ordering</label>
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlOrderCriteria" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="CheckInDate">Check In Date</asp:ListItem>
                                        <asp:ListItem Value="ReservationNo">Reservation No</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4 col-padding-left-none">
                                    <asp:DropDownList ID="ddlorderOption" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="DESC">DESC</asp:ListItem>
                                        <asp:ListItem Value="ASC">ASC</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <label for="Status" class="control-label col-md-2">
                            Reservation Status</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSearchByStatus" runat="server" CssClass="form-control"
                                TabIndex="21">
                                <asp:ListItem Value="All">All</asp:ListItem>
                                <asp:ListItem Value="Confirmed">Confirmed</asp:ListItem>
                                <asp:ListItem Value="Waiting">Waiting</asp:ListItem>
                                <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>--%>
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Button ID="btnReservationDetailSerach" runat="server" TabIndex="4" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformRoomReservationSearchButton();" />
                            <button type="button" id="btnClearSearch" class="btn btn-primary btn-sm" onclick="PerformClearSearchAction()">Clear</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="GroupReservationDetailDiv" style="display:none;">
        <div class="panel panel-default">
            <div class="panel-heading">
                Reservation List
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group" style="padding-top: 5px;">
                        <div class="col-md-12">
                            <asp:Panel ID="pnlRoomReservationList" runat="server" ScrollBars="Both" Height="400px">
                                <div id="RoomReservationGridContainer">
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Group Information
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div id="ListedGroupInfo" class="form-group">
                        <div id="GroupLabel" class="col-md-2" style="text-align: right;">
                            <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Group Name"></asp:Label>
                        </div>
                        <div id="GroupControl" class="col-md-10">
                            <div class="input-group col-md-12">
                                <span id="chkIsLitedGroupDiv" class="input-group-addon" style="text-align: left;">
                                    <asp:CheckBox ID="chkIsLitedGroup" runat="server" Text="" onclick="javascript: return ToggleListedGroupInfo();"
                                        TabIndex="8" />
                                </span>
                                <div id="ListedContact" style="display: none; width: 100%;">
                                    <input id="txtListedGroupName" type="text" class="form-control" runat="server" />
                                    <div style="display: none;">
                                        <asp:HiddenField runat="server" ID="hfGroupId" Value="0" />
                                    </div>
                                </div>
                                <div id="ReservedContact" style="display: none; width: 100%;">
                                    <asp:TextBox ID="txtGroupName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="CheckInDate" class="control-label required-field col-md-2">
                            Date</label>
                        <div class="col-md-2">
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" placeholder="Check In Date" TabIndex="63"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="TextBox3" runat="server" TabIndex="64" CssClass="form-control" placeholder="Check Out Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="CheckInDate" class="control-label required-field col-md-2">
                            Description</label>
                        <div class="col-md-10">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="txtGroupReservationDescription" TextMode="MultiLine" CssClass="form-control" runat="server" TabIndex="1" placeholder="Description"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="padding-bottom:10px;">
            <div class="col-md-12">
                <asp:Button ID="btnUpdate" runat="server" TabIndex="4" Text="Update" CssClass="TransactionalButton btn btn-primary btn-sm"
                    OnClientClick="javascript: return ValidationBeforeGroupReservationPosting();" />
                <button type="button" id="btnClearForm" class="btn btn-primary btn-sm" onclick="PerformClearSearchAction()">Clear</button>
            </div>
        </div>
        </div>
    </div>
    
    
    <script type="text/javascript">
        <%--var rsvnReservationId = '<%=rsvnReservationId%>';
        if (rsvnReservationId > -1) {
            PerformSearchButton();
        }--%>
    </script>
</asp:Content>
