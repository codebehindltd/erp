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

            $("#ContentPlaceHolder1_ddlCompany").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_txtReservationDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_txtSrcCheckInDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSrcCheckOutDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtSrcCheckOutDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSrcCheckInDate").datepicker("option", "maxDate", selectedDate);
                }
            });

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

            //$("#ContentPlaceHolder1_txtCheckInDate").datepicker({
            //    changeMonth: true,
            //    changeYear: true,
            //    dateFormat: innBoarDateFormat,
            //    onClose: function (selectedDate) {
            //        $("#ContentPlaceHolder1_txtCheckOutDate").datepicker("option", "minDate", selectedDate);
            //    }
            //});

            //$("#ContentPlaceHolder1_txtCheckOutDate").datepicker({
            //    changeMonth: true,
            //    changeYear: true,
            //    dateFormat: innBoarDateFormat,
            //    onClose: function (selectedDate) {
            //        $("#ContentPlaceHolder1_txtCheckInDate").datepicker("option", "maxDate", selectedDate);
            //    }
            //});

            var ddlCompany = '<%=ddlCompany.ClientID%>'
            $('#' + ddlCompany).change(function () {
                if ($('#' + ddlCompany).val() != "0") {
                    $("#ContentPlaceHolder1_txtGroupName").val($("#<%=ddlCompany.ClientID %> option:selected").text());
                }
                else {
                    $("#ContentPlaceHolder1_txtGroupName").val("");

                }
            })

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
                $("#RoomReservationGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#RoomReservationGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }

        function PerformGroupRoomReservationSearchButton() {
            var groupName = "";
            var reservationNumber = $("#ContentPlaceHolder1_txtSearchReservationNumber").val();
            var checkInDate = $.trim($("#<%=txtSrcCheckInDate.ClientID %>").val());
            var checkOutDate = $.trim($("#<%=txtSrcCheckOutDate.ClientID %>").val());

            if (checkInDate == "") {
                toastr.warning("Please Provide Search Date (From Date).");
                $("#ContentPlaceHolder1_txtSrcCheckInDate").val("");
                $("#ContentPlaceHolder1_txtSrcCheckInDate").focus();
                return false;
            }

            if (checkOutDate == "") {
                toastr.warning("Please Provide Search Date (To Date).");
                $("#ContentPlaceHolder1_txtSrcCheckOutDate").val("");
                $("#ContentPlaceHolder1_txtSrcCheckOutDate").focus();
                return false;
            }

            $("#GroupRoomReservationGridContainer").show();
            CommonHelper.SpinnerOpen();
            PageMethods.GetGroupRoomReservationInfoByStringSearchCriteria(groupName, reservationNumber, checkInDate, checkOutDate, OnLoadGroupRoomReservationGridInformationSucceed, OnLoadGroupRoomReservationGridInformationFailed);
            return false;
        }

        function OnLoadGroupRoomReservationGridInformationSucceed(result) {
            //$("#GroupReservationDetailDiv").show();
            $("#GroupRoomReservationGridContainer").html(result.RoomReservationGrid);
            CommonHelper.SpinnerClose();
            return false;
        }
        function PerformBillPreviewAction(reservationId) {
            var reportType = "room";
            var url = "/HotelManagement/Reports/frmReportGroupReservationBillInfo.aspx?rt=" + reportType + "&rid=" + reservationId;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }
        function OnLoadGroupRoomReservationGridInformationFailed(error) {

            CommonHelper.SpinnerClose();
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

            var IsPermitForSave = confirm("Do You Want To Posting Group Reservation?");
            var IsCheckedMinimumOneReservation = false;
            if (IsPermitForSave == true) {
                var groupMasterId = 0, companyId = 0, groupName = "", reservationId = 0, reservationDetails = "", reservationDate, checkInDate = "", checkOutDate = "";

                <%--companyId = $("#<%=ddlCompany.ClientID %>").val();
                if (companyId == "0") {
                    toastr.info("Please select Company Name.");
                    $("#<%=ddlCompany.ClientID %>").focus();
                    return false;
                }--%>

                groupName = $("#ContentPlaceHolder1_txtGroupName").val();
                if (groupName == "") {
                    toastr.info("Please enter Group Name.");
                    $("#ContentPlaceHolder1_txtGroupName").focus();
                    return false;
                }

                reservationDate = $("#ContentPlaceHolder1_txtReservationDate").val();
                if (reservationDate == "") {
                    toastr.info("Please enter Reservation Date.");
                    $("#ContentPlaceHolder1_txtReservationDate").focus();
                    return false;
                }

                //checkInDate = $("#ContentPlaceHolder1_txtCheckInDate").val();
                //if (checkInDate == "") {
                //    toastr.info("Please enter Check In Date.");
                //    $("#ContentPlaceHolder1_txtCheckInDate").focus();
                //    return false;
                //}

                //checkOutDate = $("#ContentPlaceHolder1_txtCheckOutDate").val();
                //if (checkOutDate == "") {
                //    toastr.info("Please enter Check Out Date.");
                //    $("#ContentPlaceHolder1_txtCheckOutDate").focus();
                //    return false;
                //}

                reservationDetails = $("#ContentPlaceHolder1_txtGroupReservationDescription").val();

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

                PageMethods.SaveOrUpdateGroupRoomReservation(groupMasterId, companyId, groupName, reservationDate, reservationDetails, GroupRoomReservationDetails, OnSaveGroupReservationPostingSucceeded, OnSaveGroupReservationPostingFailed);
                return false;
            } else {
                CommonHelper.SpinnerClose();
                return false;
            }
        }
        function OnSaveGroupReservationPostingSucceeded(result) {
            $("#GroupReservationDetailDiv").hide();
            toastr.success('Updated Group Reservation Successfully.')
            CloseCompanyDialog();
            $("#<%=ddlCompany.ClientID %>").val("0");
            CommonHelper.SpinnerClose();
            return false;
        }

        function OnSaveGroupReservationPostingFailed() { CommonHelper.SpinnerClose(); }
        function RedirectToDetails(TransectionId, reservation, roomNumber, date) {
        }
        function ToggleListedGroupInfo() {
            <%--var ctrl = '#<%=chkIsLitedGroup.ClientID%>';

            if ($(ctrl).is(':checked')) {
                $('#ReservedContact').hide("slow");
                $('#ListedContact').show("slow");
            }
            else {

                $('#ListedContact').hide("slow");
                $('#ReservedContact').show("slow");
                $(ctrl).prop('checked', false)
                $("#ContentPlaceHolder1_hfContactId").val("0");
            }--%>
        }
        function NewGroupRoomReservation() {
            $("#GroupRoomReservationDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 990,
                height: 690,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                title: "New Group Room Reservation",
                show: 'slide'
            });
            return;
        }
        function CloseCompanyDialog() {
            $('#GroupRoomReservationDiv').dialog('close');
            return false;
        }
        function PerformCancelAction(reservationId) {            
            $("#<%=hfCancelReservationId.ClientID%>").val(reservationId);
            $("#reservationCancelPopUp").dialog({
                autoOpen: true,
                modal: true,
                width: 725,
                closeOnEscape: true,
                resizable: false,
                title: "Cancel Reason",
                show: 'slide'

            });
            return false;
        }

        function ConfirmCancelReservation() {
            var reservationId = parseInt($("#<%=hfCancelReservationId.ClientID%>").val());
            var reason = $("#ContentPlaceHolder1_txtCancelReason").val();
            if (reason == "") {
                toastr.warning("Please provide reason.");
            }
            else {
                var isConfirm = confirm('Do you want to Cancel this Group Reservation?');
                if (isConfirm)
                    PageMethods.CancelGroupReservation(reservationId, reason, OnSuccessCancelReservation, OnFailCancelReservation);
            }

            return false;
        }

        function OnSuccessCancelReservation(returnInfo) {
            if (returnInfo.IsSuccess) {
                toastr.success(returnInfo.AlertMessage.Message);
                setTimeout(function () {
                    var possiblePath = "frmGroupRoomReservation.aspx";
                    window.location = possiblePath;
                }, 1500);
            }

            else
                toastr.error(returnInfo.AlertMessage.Message);
        }

        function OnFailCancelReservation(error) {
            toastr.error(error.get_message());
        }
    </script>    
    <div id="reservationCancelPopUp" style="display: none;" class="panel panel-default">
        <asp:HiddenField ID="hfCancelReservationId" runat="server" Value="0" />
        <div class="panel-body">
            <div class="form-horizontal">                
                <div class="form-group">
                    <div class="col-md-3">
                        <asp:Label runat="server" class="control-label required-field" Text="Cancel Reason"></asp:Label>
                    </div>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtCancelReason" runat="server" CssClass="form-control" TextMode="MultiLine"
                            TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <button class="btn btn-primary btn-sm" type="button" onclick="return ConfirmCancelReservation();">
                            Confirm</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Search Group Room Reservation
        </div>
        <div class="panel-body">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label for="GuestCompany" class="control-label col-md-2">
                            Company Name</label>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlSrcCompany" runat="server" CssClass="form-control" AutoPostBack="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2" style="text-align: right;">
                            <asp:Label ID="Label1" runat="server" class="control-label" Text="Group Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <div class="input-group col-md-12">
                                <div style="width: 100%;">
                                    <asp:TextBox ID="txtSrcGroupName" CssClass="form-control" runat="server" TabIndex="1" placeholder="Group Name"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="CheckInDate" class="control-label col-md-2 required-field">
                            Date</label>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtSrcCheckInDate" runat="server" CssClass="form-control" placeholder="Check In Date" TabIndex="63"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtSrcCheckOutDate" runat="server" TabIndex="64" CssClass="form-control" placeholder="Check Out Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Button ID="btnSrcSearch" runat="server" TabIndex="4" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformGroupRoomReservationSearchButton();" />
                            <button type="button" id="btnSrcClearSearch" class="btn btn-primary btn-sm" onclick="PerformClearSearchAction()">Clear</button>
                            <button type="button" id="btnNewGroupRoomReservation" class="btn btn-primary btn-sm" onclick="javascript: return NewGroupRoomReservation();">New Group Reservation</button>
                        </div>
                    </div>
                    <div class="form-group" style="padding-top: 5px;">
                        <div class="col-md-12">
                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Height="400px">
                                <div id="GroupRoomReservationGridContainer">
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="GroupRoomReservationDiv" class="panel panel-default" style="display: none;">
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
        <div id="GroupReservationDetailDiv" style="display: none;">
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

                        <div class="form-group">
                            <label for="GuestCompany" class="control-label col-md-2">
                                Company Name</label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" AutoPostBack="false">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="ListedGroupInfo" class="form-group">
                            <div id="GroupLabel" class="col-md-2" style="text-align: right;">
                                <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Group Name"></asp:Label>
                            </div>
                            <div id="GroupControl" class="col-md-10">
                                <div class="input-group col-md-12">
                                    <div id="ReservedContact" style="width: 100%;">
                                        <asp:TextBox ID="txtGroupName" CssClass="form-control" runat="server" TabIndex="1" placeholder="Group Name"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="CheckInDate" class="control-label required-field col-md-2">
                                Reservation Date</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReservationDate" runat="server" CssClass="form-control" placeholder="Check In Date" TabIndex="63"></asp:TextBox>
                            </div>
                            <%--<label for="CheckInDate" class="control-label required-field col-md-2">
                                Check In/ Out Date</label>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtCheckInDate" runat="server" CssClass="form-control" placeholder="Check In Date" TabIndex="63"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtCheckOutDate" runat="server" TabIndex="64" CssClass="form-control" placeholder="Check Out Date"></asp:TextBox>
                            </div>--%>
                        </div>
                        <div class="form-group">
                            <label for="CheckInDate" class="control-label col-md-2">
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
            <div class="row" style="padding-bottom: 10px;">
                <div class="col-md-12">
                    <asp:Button ID="btnUpdate" runat="server" TabIndex="4" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return ValidationBeforeGroupReservationPosting();" />
                    <button type="button" id="btnClearForm" class="btn btn-primary btn-sm" onclick="PerformClearSearchAction()">Clear</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
