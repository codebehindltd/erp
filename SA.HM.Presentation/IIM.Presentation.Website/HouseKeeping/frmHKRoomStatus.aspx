<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmHKRoomStatus.aspx.cs" Inherits="HotelManagement.Presentation.Website.HouseKeeping.frmHKRoomStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var SelectdPreferenceId = "";
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>House Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Room Status</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#PopReservation").tabs();

            var strDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtCurrentDate").val(), '/');
            minCheckOutDate = GetStringFromDateTime(CommonHelper.DaysAdd(strDate, 0));

            $("#ContentPlaceHolder1_txtFromDateForAll").datepicker({
                changeMonth: true,
                changeYear: true,
                minDate: minCheckOutDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtToDateForAll").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#ContentPlaceHolder1_txtFromTimeForAll").timepicker({
                showPeriod: is12HourFormat
            });
            $("#ContentPlaceHolder1_txtToDateForAll").datepicker({
                changeMonth: true,
                changeYear: true,
                //minDate: minCheckOutDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtFromDateForAll").datepicker("option", "maxDate", selectedDate);

                    if ($("#ContentPlaceHolder1_txtToDateForAll").val() != "")
                        $("#txtReservationDuration").val(CommonHelper.DateDifferenceInDays(CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtCurrentDate").val(), '/'), CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDateForAll").val(), '/')));
                    else
                        $("#txtReservationDuration").val("");
                }
            });

            $("#ContentPlaceHolder1_txtToTimeForAll").timepicker({
                showPeriod: is12HourFormat
            });
            $("[id=ContentPlaceHolder1_gvRoomStatus_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvRoomStatus tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvRoomStatus tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

            var roomGrid = document.getElementById("<%=gvRoomStatus.ClientID %>");

            var rows = roomGrid.getElementsByTagName("tr");
            for (var i = 0; i < rows.length - 1; i++) {
                $("#ContentPlaceHolder1_gvRoomStatus_txtFromDate_" + i).datepicker({
                    changeMonth: true,
                    changeYear: true,
                    minDate: minCheckOutDate,
                    dateFormat: innBoarDateFormat,
                    onBlur: function (selectedDate) {
                        $("#ContentPlaceHolder1_gvRoomStatus_txtToDate_" + i).datepicker("option", "minDate", selectedDate);
                    }

                });


                $("#ContentPlaceHolder1_gvRoomStatus_txtFromTime_" + i).timepicker({
                    showPeriod: is12HourFormat
                });
                $("#ContentPlaceHolder1_gvRoomStatus_txtToDate_" + i).datepicker({
                    changeMonth: true,
                    changeYear: true,
                    minDate: minCheckOutDate,
                    dateFormat: innBoarDateFormat,
                    onClose: function (selectedDate) {
                        $("#ContentPlaceHolder1_gvRoomStatus_txtFromDate_" + i).datepicker("option", "maxDate", selectedDate);
                    }
                });

                $("#ContentPlaceHolder1_gvRoomStatus_txtToTime_" + i).timepicker({

                    showPeriod: is12HourFormat
                });
            }
            for (var i = 0; i < rows.length - 1; i++) {
                var hkstatus = $("#ContentPlaceHolder1_gvRoomStatus_ddlHKRoomStatus_" + i).val();
                if (hkstatus == 3 || hkstatus == 4) {
                    $("#ContentPlaceHolder1_gvRoomStatus_txtToDate_" + i).show();
                    $("#ContentPlaceHolder1_gvRoomStatus_txtReason_" + i).show();
                    $("#FromDateDiv" + i).show();
                    $("#ToDateDiv" + i).show();
                    $("#ReasonDiv" + i).show();
                }
                else {
                    $("#ContentPlaceHolder1_gvRoomStatus_txtToDate_" + i).hide();
                    $("#ContentPlaceHolder1_gvRoomStatus_txtReason_" + i).hide();
                    $("#FromDateDiv" + i).hide();
                    $("#ToDateDiv" + i).hide();
                    $("#ReasonDiv" + i).hide();
                }
            }
        });

        // Guest Preference
        function GetGuestIdFMRoomId(roomId) {
            PageMethods.GetGuestIdFMRoomId(roomId, OnLoadGuestIdFMRoomIdSucceeded, OnLoadGuestIdFMRoomIdFailed);
            return false;
        }
        function OnLoadGuestIdFMRoomIdSucceeded(result) {
            $("#ContentPlaceHolder1_hfGuestId").val(result.GuestId);
            PageMethods.GetGuestSavedPreference(result.GuestId, OnLoadGuestSavedPreferenceSucceeded, OnLoadGuestSavedPreferenceFailed);
            return false;
        }
        function OnLoadGuestIdFMRoomIdFailed() {
        }
        function OnLoadGuestSavedPreferenceSucceeded(result) {
            $("#ContentPlaceHolder1_hfGuestPreferenceId").val(result);
            LoadGuestPreference();
        }
        function OnLoadGuestSavedPreferenceFailed() {
        }

        function LoadGuestPreference() {
            LoadGuestPreferenceInfo();
            $("#DivGuestPreference").dialog({
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

        function LoadGuestPreferenceInfo() {
            PageMethods.LoadGuestPreferenceInfo(OnLoadGuestPreferenceSucceeded, OnLoadGuestPreferenceFailed);
            return false;
        }
        function OnLoadGuestPreferenceSucceeded(result) {
            $("#ltlGuestPreference").html(result);

            var PreferenceIdList = "";

            PreferenceIdList = $("#ContentPlaceHolder1_hfGuestPreferenceId").val();

            if (PreferenceIdList != "") {
                var SavedPreferenceArray = PreferenceIdList.split(",");
                if (SavedPreferenceArray.length > 0) {
                    for (var i = 0; i < SavedPreferenceArray.length; i++) {
                        var preferenceId = "#" + SavedPreferenceArray[i].trim();
                        $(preferenceId).attr("checked", true);
                    }
                }
            }
            return false;
        }
        function OnLoadGuestPreferenceFailed() {
        }
        function StatusValidation() {

            var rowCount = <%=gvRoomStatus.Rows.Count%>;
            var grid = document.getElementById('<%=gvRoomStatus.ClientID %>');
            var sameStatus = $("#ContentPlaceHolder1_ddlAllHKSelect").val();
            var txtRemarks = $("#ContentPlaceHolder1_txtRemarks").val();
            var txtFromDateForAll = $("#ContentPlaceHolder1_txtFromDateForAll").val();
            var txtToDateForAll = $("#ContentPlaceHolder1_txtToDateForAll").val();
            var txtFromTimeForAll = $("#ContentPlaceHolder1_txtFromTimeForAll").val();
            var txtToTimeForAll = $("#ContentPlaceHolder1_txtToTimeForAll").val();
            var count = 0;

            if (sameStatus == "3" || sameStatus == "4") {

                if (txtRemarks == "") {
                    toastr.warning("Please Give Valid Reason");
                    $("#ContentPlaceHolder1_txtRemarks").focus();
                    return false;
                }
                else if (txtFromDateForAll == "") {
                    toastr.warning("Please confirm From Date for all.");
                    $("#ContentPlaceHolder1_txtFromDateForAll").focus();
                    return false;
                }
                else if (txtFromTimeForAll == "") {
                    toastr.warning("Please confirm From Time for all ");
                    $("#ContentPlaceHolder1_txtFromTimeForAll").focus();
                    return false;
                }
                else if (txtToDateForAll == "") {
                    toastr.warning("Please confirm To Date for all");
                    $("#ContentPlaceHolder1_txtToDateForAll").focus();
                    return false;
                }
                else if (txtToTimeForAll == "") {
                    toastr.warning("Please confirm To Time for all");
                    $("#ContentPlaceHolder1_txtToTimeForAll").focus();
                    return false;
                }

            }
            else {
                for (var i = 0; i < rowCount; i++) {
                    var checkBoxes = $("#ContentPlaceHolder1_gvRoomStatus_chkIsSavePermission_" + i).is(":checked");
                    if (checkBoxes == true) {
                        
                        count++;
                        var ddlHKRoomStatus = $("#ContentPlaceHolder1_gvRoomStatus_ddlHKRoomStatus_" + i).val();
                        var fromDate = $("#ContentPlaceHolder1_gvRoomStatus_txtFromDate_" + i).val();
                        var fromTime = $("#ContentPlaceHolder1_gvRoomStatus_txtFromTime_" + i).val();
                        var toDate = $("#ContentPlaceHolder1_gvRoomStatus_txtToDate_" + i).val();
                        var toTime = $("#ContentPlaceHolder1_gvRoomStatus_txtToTime_" + i).val();
                        var reason = $("#ContentPlaceHolder1_gvRoomStatus_txtReason_" + i).val();
                        if (ddlHKRoomStatus == "3" || ddlHKRoomStatus == "4") {
                            if (fromDate == "") {
                                toastr.warning("Please provide From Date");
                                $("#ContentPlaceHolder1_gvRoomStatus_txtFromDate_" + i).focus();
                                return false;
                            }
                            else if (fromTime == "") {
                                toastr.warning("Please provide From Time");
                                $("#ContentPlaceHolder1_gvRoomStatus_txtFromTime_" + i).focus();
                                return false;
                            }
                            else if (toDate == "") {
                                toastr.warning("Please provide To Date");
                                $("#ContentPlaceHolder1_gvRoomStatus_txtToDate_" + i).focus();
                                return false;
                            }
                            else if (toTime == "") {
                                toastr.warning("Please provide To Time");
                                $("#ContentPlaceHolder1_gvRoomStatus_txtToTime_" + i).focus();
                                return false;
                            }
                            else if (reason == "") {
                                toastr.warning("Please provide a valid reason");
                                $("#ContentPlaceHolder1_gvRoomStatus_txtReason_" + i).focus();
                                return false;
                            }
                        }
                        
                        var startTime = moment(fromTime, "HH:mm a");
                        var endTime = moment(toTime, "HH:mm a");
                        var flag = endTime.isBefore(startTime);
                        var today = moment();
                        
                        //var now = moment();
                        //var time = now.hour() + ':' + now.minutes();
                        //time = time + ((now.hour()) >= 12 ? ' PM' : ' AM');
                        //var fromTimeFlag = startTime.isBefore(time); moment('2010-10-20').isBefore('2010-10-21');
                        var startDate = CommonHelper.DateFormatToYYYYMMDD(fromDate, '/');
                        var endDate = CommonHelper.DateFormatToYYYYMMDD(toDate, '/');
                       
                        var validDate = moment(endDate).isBefore(startDate);
                        
                        if ((fromDate == toDate) && (flag == true)) {
                            toastr.warning("To Time has passed. Please provide a valid Time");
                            return false;
                        }
                        else if (validDate) {
                            toastr.warning("To Date has passed. Please provide a valid Date");
                            return false;
                        }
                        //else if ((today.isSame(fromDate)) && (fromTimeFlag)) {
                        //    toastr.warning("From Time has passed. Please provide a valid Time");
                        //    return false;
                        //}

                    }

                }
                if (count <= 0) {
                    toastr.warning("Please Select at least one room");
                    return false;
                }
            }

            return true;
        }
        function GetCheckedGuestPreference() {
            $("#GuestPreferenceDiv").show();
            var SelectdPreferenceName = "";

            $('#GuestPreferenceInformation tbody tr').each(function () {
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

            var guestId = PreferenceIdList = $("#ContentPlaceHolder1_hfGuestId").val();
            PageMethods.SaveGuestPreference(SelectdPreferenceId, guestId, OnSaveGuestPreferenceSucceeded, OnSaveGuestPreferenceFailed);
        }

        function OnSaveGuestPreferenceSucceeded(result) {
            $("#DivGuestPreference").dialog("close");
            $("#ContentPlaceHolder1_hfGuestId").val("");
            SelectdPreferenceId = "";
        }
        function OnSaveGuestPreferenceFailed() {
        }

        function ClosePreferenceDialog() {
            $("#DivGuestPreference").dialog("close");
        }

        //Reservation Details
        function GetReservationIdFMRoomId(roomId) {
            PageMethods.GetReservationIdFMRoomId(roomId, OnLoadReservationInfoSucceeded, OnLoadReservationInfoFailed);
            return false;
        }
        function OnLoadReservationInfoSucceeded(result) {
            LoadReservationDetails(result.ReservationId);
        }
        function OnLoadReservationInfoFailed() {
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
                width: 900,
                height: 500,
                closeOnEscape: true,
                resizable: false,
                title: "Reservation Details", ////TODO add title
                show: 'slide'
            });

            $("#popUpDiv").css('height', '496px');
            return false;
        }

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

        function ShowHideDetails(rowIndex, reason) {
            var hkstatus = $("#ContentPlaceHolder1_gvRoomStatus_ddlHKRoomStatus_" + rowIndex).val();
            var fromDate, fromTime, toDate, toTime;
            fromDate = $("#ContentPlaceHolder1_gvRoomStatus_hfTxtFromDate_" + rowIndex).val();
            fromTime = $("#ContentPlaceHolder1_gvRoomStatus_hfTxtFromTime_" + rowIndex).val();
            toDate = $("#ContentPlaceHolder1_gvRoomStatus_hfTxtToDate_" + rowIndex).val();
            toTime = $("#ContentPlaceHolder1_gvRoomStatus_hfTxtToTime_" + rowIndex).val();
            if (hkstatus == 3 || hkstatus == 4) {
                $("#ContentPlaceHolder1_gvRoomStatus_txtToDate_" + rowIndex).show();
                $("#ContentPlaceHolder1_gvRoomStatus_txtReason_" + rowIndex).show();
                $("#FromDateDiv" + rowIndex).show();
                $("#ToDateDiv" + rowIndex).show();
                $("#ReasonDiv" + rowIndex).show();
            }
            else {
                $("#ContentPlaceHolder1_gvRoomStatus_txtToDate_" + rowIndex).hide();
                $("#ContentPlaceHolder1_gvRoomStatus_txtReason_" + rowIndex).hide();
                $("#FromDateDiv" + rowIndex).hide();
                $("#ToDateDiv" + rowIndex).hide();
                $("#ReasonDiv" + rowIndex).hide();
            }
            if (fromDate != "")
                $("#ContentPlaceHolder1_gvRoomStatus_txtFromDate_" + rowIndex).val(fromDate);
            else
                $("#ContentPlaceHolder1_gvRoomStatus_txtFromDate_" + rowIndex).val('');

            if (fromTime != "")
                $("#ContentPlaceHolder1_gvRoomStatus_txtFromTime_" + rowIndex).val(fromTime);
            else
                $("#ContentPlaceHolder1_gvRoomStatus_txtFromTime_" + rowIndex).val('');

            if (toDate != "")
                $("#ContentPlaceHolder1_gvRoomStatus_txtToDate_" + rowIndex).val(toDate);
            else
                $("#ContentPlaceHolder1_gvRoomStatus_txtToDate_" + rowIndex).val('');

            if (toTime != "")
                $("#ContentPlaceHolder1_gvRoomStatus_txtToTime_" + rowIndex).val(toTime);
            else
                $("#ContentPlaceHolder1_gvRoomStatus_txtToTime_" + rowIndex).val('');

            if (reason != "")
                $("#ContentPlaceHolder1_gvRoomStatus_txtReason_" + rowIndex).val(reason);
            else $("#ContentPlaceHolder1_gvRoomStatus_txtReason_" + rowIndex).val('');
        }
    </script>
    <asp:HiddenField ID="hfGuestId" runat="server" />
    <asp:HiddenField ID="hfGuestPreferenceId" runat="server" />
    <%--<asp:HiddenField ID="hfHouseKeepingMorningDirtyHour" runat="server" />--%>
    <asp:HiddenField ID="txtCurrentDate" runat="server" />
    <asp:HiddenField ID="hfIsFloorNameValidationActive" runat="server" />
    <div id="ReservationPopUp" style="display: none;">
        <div id="Div2" style="width: 900px">
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
                        <div id="ReservationInfo">
                        </div>
                    </div>
                </div>
                <div id="PopupReservationGuestInfo">
                    <div id="ReservationGuest" class="panel panel-default" style="font-weight: bold">
                        <div class="panel-heading">Guest Information</div>
                        <div id="ReservationGuestInfo">
                        </div>
                    </div>
                </div>
                <button type="button" id="btnReservationBack" onclick="javascript:return popup(-1)"
                    class="TransactionalButton btn btn-primary btn-sm">
                    Back</button>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-4">
                        <fieldset style="border: 1px solid #c0c0c0; padding: 10px;">
                            <legend style="border-bottom: 0; margin-bottom: 0; width: 24%; font-size: 15px;">FO
                                Status</legend>
                            <div class="col-md-6">
                                <div class="checkbox checkboxlist">
                                    <asp:CheckBoxList ID="chkFORoomStatus1" TabIndex="1" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="checkbox checkboxlist">
                                    <asp:CheckBoxList ID="chkFORoomStatus2" TabIndex="1" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="col-md-4">
                        <fieldset style="border: 1px solid #c0c0c0; padding: 10px;">
                            <legend style="border-bottom: 0; margin-bottom: 0; width: 24%; font-size: 15px;">HK
                                Status</legend>
                            <div class="col-md-6">
                                <div class="checkbox checkboxlist">
                                    <asp:CheckBoxList ID="chkHKRoomStatus1" Height="20px" TabIndex="2" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="checkbox checkboxlist">
                                    <asp:CheckBoxList ID="chkHKRoomStatus2" Height="20px" TabIndex="2" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="col-md-4">
                        <fieldset style="border: 1px solid #c0c0c0; padding: 10px;">
                            <legend style="border-bottom: 0; margin-bottom: 0; width: 40%; font-size: 15px;">Reservation
                                Status</legend>
                            <div class="col-md-8">
                                <div class="checkbox checkboxlist">
                                    <asp:CheckBoxList ID="chkReservationStatus" Height="50px" TabIndex="2" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="form-group">
                    <fieldset style="border: 1px solid #c0c0c0; padding: 10px;">
                        <legend>Floor/Block</legend>
                        <div class="col-md-2 text-right">
                            <asp:Label ID="lblFloorId" runat="server" CssClass="control-label" Text="Floor Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFloorId" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2 text-right">
                            <asp:Label ID="Label2" runat="server" CssClass="control-label" Text="Block"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFloorBlock" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                    </fieldset>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <asp:CheckBox ID="chkAllActiveReservation" runat="server" Text=""
                        TabIndex="2" />
                    <asp:Label ID="Label12" runat="server" class="control-label" Text="Search With Room Occupancy Color"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="4" CssClass="btn btn-primary btn-sm"
                        OnClick="btnSearch_Click" />
                </div>
                <div id="SelectAllHK" class="col-md-6" style="text-align: right; display: none;">
                    <div class="col-md-7">
                        <asp:Label ID="lblAllHKSelect" runat="server" class="control-label" Text="Select Same HK status for all Room"></asp:Label>
                    </div>
                    <div class="col-md-5">
                        <asp:DropDownList ID="ddlAllHKSelect" runat="server" CssClass="form-control" TabIndex="1"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlAllHKSelect_Change">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-horizontal" id="RemarksForAll" style="padding-top: 10px; padding-bottom: 10px; display: none;">
                <div class="form-group">
                    <label for="Remarks" class="control-label col-md-2">
                        Remarks</label>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                            TabIndex="11"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="ToDate" class="control-label col-md-2">
                        From Date</label>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtFromDateForAll" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtFromTimeForAll" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                    </div>
                    <label for="ToDate" class="control-label col-md-2">
                        To Date</label>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtToDateForAll" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtToTimeForAll" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                    </div>

                </div>
            </div>
            <div id="RoomStatus" runat="server" style="padding-top: 20px;">
                <asp:GridView ID="gvRoomStatus" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="500"
                    OnPageIndexChanging="gvRoomStatus_PageIndexChanging" OnRowDataBound="gvRoomStatus_RowDataBound"
                    CssClass="table table-bordered table-condensed table-responsive">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblRoomId" runat="server" Text='<%#Eval("RoomId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="02%">
                            <HeaderTemplate>
                                <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="RoomNumber" HeaderText="Room" ItemStyle-Width="5%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RoomType" HeaderText="Type" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="FO Status" ShowHeader="False" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblFORoomStatus" runat="server" Text='<%# Eval("FORoomStatus") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DateIn" HeaderText="Arrival Date" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DateOut" HeaderText="Departure Date" ItemStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Reservation" ShowHeader="False" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblReservationStatus" runat="server" Text='<%# Eval("ReservationStatus") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HK Status" ShowHeader="False" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lblHKRoomStatus" runat="server" Text='<%# Eval("HKRoomStatus") %>'
                                    Visible="false" />
                                <asp:DropDownList ID="ddlHKRoomStatus" runat="server" CssClass="form-control" OnClientClick="GetAlert2();">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LastCleanDate" ShowHeader="False" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblLastCleanDate" runat="server" Text='<%# Eval("LastCleanDate") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Details" ShowHeader="False" ItemStyle-Width="23%">
                            <ItemTemplate>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            LCD:
                                        </div>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtShowLastCleanDate" runat="server" CssClass="form-control" Enabled="false"
                                                Text='<%#Eval("ShowLastCleanDate") %>'></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="FromDateDiv<%# Container.DisplayIndex %>" class="form-group">
                                        <div class="col-md-3">
                                            From Date:
                                        </div>
                                        <div class="col-md-9">
                                            <div class="row">
                                                <div class="col-md-7">
                                                    <asp:HiddenField runat="server" ID="hfTxtFromDate" />
                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-5" style="padding-left: unset">
                                                    <asp:HiddenField runat="server" ID="hfTxtFromTime" />
                                                    <asp:TextBox ID="txtFromTime" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div id="ToDateDiv<%# Container.DisplayIndex %>" class="form-group">
                                        <div class="col-md-3">
                                            To Date:
                                        </div>
                                        <div class="col-md-9">
                                            <div class="row">
                                                <div class="col-md-7">
                                                    <asp:HiddenField runat="server" ID="hfTxtToDate" />
                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-5" style="padding-left: unset">
                                                    <asp:HiddenField runat="server" ID="hfTxtToTime" />
                                                    <asp:TextBox ID="txtToTime" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="ReasonDiv<%# Container.DisplayIndex %>" class="form-group">
                                        <div class="col-md-3">
                                            Reason:
                                        </div>
                                        <div class="col-md-9">
                                            <asp:Label ID="lblOutoforderReason" runat="server" Text='<%# Eval("Reason") %>' Visible="false" />
                                            <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" Text='<%#Eval("Reason") %>'></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Option" ShowHeader="False" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnPreference" runat="server" CausesValidation="False" CommandArgument='<%# bind("RoomId") %>'
                                    CommandName="CmdPreference" ImageUrl="~/Images/remarksadd.png" Text="" AlternateText="Preference"
                                    ToolTip="Preference" />
                                <asp:ImageButton ID="btnDetails" runat="server" CausesValidation="False" CommandArgument='<%# bind("RoomId") %>'
                                    CommandName="CmdDetails" ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Preference"
                                    ToolTip="Preference" />
                            </ItemTemplate>
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
            </div>
            <div id="SaveBtn" style="display: none;">
                <div class="row" runat="server">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="5" CssClass="btn btn-primary btn-sm"
                            OnClick="btnSave_Click" OnClientClick="javascript: return StatusValidation()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--Guest Preference PopUp -->
    <div id="DivGuestPreference" style="display: none;">
        <div id="Div1">
            <div id="ltlGuestPreference">
            </div>
        </div>
    </div>
    <!--End Guest Preference PopUp -->
    <script type="text/javascript">
        var x = '<%=IsSuccess%>';
        if (x > -1) {
            $('#SaveBtn').show();
            $('#SelectAllHK').show();
        }

        $(document).ready(function () {
            var ddlAllHKSelect = $("#<%=ddlAllHKSelect.ClientID %>").val();

            $('#RemarksForAll').hide();
            if (ddlAllHKSelect == "3") {
                $('#RemarksForAll').show();
            }
            else if (ddlAllHKSelect == "4") {
                $('#RemarksForAll').show();
            }

            $('#' + ddlAllHKSelect).change(function () {
                if (ddlAllHKSelect == "3") {
                    $('#RemarksForAll').show();
                }
                else if (ddlAllHKSelect == "4") {
                    $('#RemarksForAll').show();
                }
            });
        });

    </script>
</asp:Content>
