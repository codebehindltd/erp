<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmRestaurantReservation.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmRestaurantReservation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var minCheckInDate = "";
        var deleteObj = [];
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Restaurant Reservation</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            minCheckInDate = $("#<%=hfMinCheckInDate.ClientID %>").val();

            $("#SearchPanel").hide('slow');
            $("#myTabs").tabs();

            GetTableDetailInformationByWM();

            $('#ContentPlaceHolder1_A').click(function () {
                $('#SubmitButtonDiv').show();
            });
            $('#ContentPlaceHolder1_B').click(function () {
                $('#SubmitButtonDiv').hide();
            });

            if ($("#<%=ddlReservationMode.ClientID %>").val() == "Pending") {
                $('#PendingDiv').show("slow");
                $('#ReasonDiv').hide("slow");
            }

            else if ($("#<%=ddlReservationMode.ClientID %>").val() == "Cancel") {
                $('#ReasonDiv').show("slow");
                $('#PendingDiv').hide("slow");
            }
            else {
                $('#PendingDiv').hide("slow");
                $('#ReasonDiv').hide("slow");
            }

            var txtStartDate = '<%=txtDateIn.ClientID%>'
            var txtEndDate = '<%=txtDateOut.ClientID%>'
            $('#' + txtStartDate).datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: minCheckInDate,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });

            //Item Details Information
            var ddlItemType = '<%=ddlItemType.ClientID%>'
            var itemType = $('#' + ddlItemType).val();
            var txtItemUnit = '<%=txtItemUnit.ClientID%>'
            var txtUnitPrice = '<%=txtUnitPrice.ClientID%>'
            //LoadProductItem(itemType);
            $('#' + ddlItemType).change(function () {
                var itemType = $('#' + ddlItemType).val();
                var txtHiddenItemId = '<%=txtHiddenItemId.ClientID%>'

                //alert(itemType);
                $('#' + txtHiddenItemId).val("");
                $('#' + txtItemUnit).val('');
                $('#' + txtUnitPrice).val('');
                LoadProductItem(itemType);
            });

            $("#btnAddDetailItem").click(function () {
                var add = false;
                var reservationId = $("#ContentPlaceHolder1_txtReservationId").val();
                if (reservationId == "") {
                    reservationId = 0;
                }
                var x = document.getElementById("ContentPlaceHolder1_chkIscomplementary").checked;

                if ($("#<%=ddlItemType.ClientID %>").val() == "0") {
                    toastr.warning("Please Select Category Name.");
                    return;
                }
                else if ($("#<%=ddlItemId.ClientID %>").val() == "0") {
                    toastr.warning("Please Select Item Name.");
                    return;
                }
                if ($("#<%=txtUnitPrice.ClientID %>").val() == "0") {
                    if (x == true) {
                        add = true;
                    }
                    else {
                        toastr.warning("Please Provide Unit Price.");
                        return;
                    }
                }
                if ($("#<%=txtUnitPrice.ClientID %>").val() != "0") {
                    var decimalOnly = /^\s*-?[1-9]\d*(\.\d{1,2})?\s*$/;
                    if (decimalOnly.test($("#<%=txtUnitPrice.ClientID %>").val())) {
                        add = true;
                    }
                    else {
                        toastr.warning("Wrong input for Unit Price.");
                        return;
                    }
                }
                if ($("#<%=txtItemUnit.ClientID %>").val() == "") {
                    toastr.warning("Please Provide Item Unit.");
                    return;
                }
                if ($("#<%=txtItemUnit.ClientID %>").val() != "") {
                    var numbersOnly = /^\d+$/;
                    if (numbersOnly.test($("#<%=txtItemUnit.ClientID %>").val())) {
                        add = true;
                    }
                    else {
                        toastr.warning("Wrong input for Item Unit.");
                        return;
                    }
                }
                if (add == true) {
                    AddItemDetailsInfo(0, reservationId);
                }
            });
            //End Item Details

            $("#<%=txtTableId.ClientID %>").click(function () {
                var txtFromDate = $("#<%=txtDateIn.ClientID %>").val();
                var txtToDate = $("#<%=txtDateOut.ClientID %>").val();

                if (txtFromDate == "") {
                    CustomAlert('The DateIn should not be empty.', 'DateIn', 'Ok')
                    document.getElementById("<%=txtDateIn.ClientID%>").focus();
                    return false;
                }
                if (txtToDate == "") {
                    CustomAlert('The Expected DateOut should not be empty.', 'Expected DateOut', 'Ok')
                    document.getElementById("<%=txtToDate.ClientID%>").focus();
                    return false;
                }
                $("#<%=DateInHiddenField.ClientID %>").val(txtFromDate);
                $("#<%=DateOutHiddenField.ClientID %>").val(txtToDate);

            });
            $("#<%=ddlReservationMode.ClientID %>").change(function () {
                if ($("#<%=ddlReservationMode.ClientID %>").val() == "Pending") {
                    $('#PendingDiv').show("slow");
                    $('#ReasonDiv').hide("slow");

                }
                else if ($("#<%=ddlReservationMode.ClientID %>").val() == "Cancel") {
                    //ReasonDiv
                    $('#ReasonDiv').show("slow");
                    $('#PendingDiv').hide("slow");
                }
                else {
                    $('#PendingDiv').hide("slow");
                    $('#ReasonDiv').hide("slow");
                }
            });

            $("#btnAddDetailGuest").click(function () {
                SaveTableDetailsInformationByWebMethod();
            });

            $('#' + '<%=ddlReservedMode.ClientID%>').change(function () {
                if ($('#' + '<%=ddlReservedMode.ClientID%>').val() == "Personal") {
                    var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
                    $(ctrl).attr('checked', false);
                    ToggleFieldVisible();
                }
                VisibleListedCompany();
            });
            VisibleListedCompany();
            ToggleFieldVisible();

            CurrencyRateInfoEnable();

            $("#<%=ddlCurrency.ClientID %>").change(function () {
                CurrencyRateInfoEnable();
            });

            $("#btnSrchRsrvtn").click(function () {
                $("#SearchPanel").show('slow');
                GridPagingForSearchReservation(1, 1);
            });
            $("#<%=txtProbableArriveHour.ClientID %>").blur(function () {
                var hourString = $("#<%=txtProbableArriveHour.ClientID %>").val();
                var hour = parseInt(hourString);
                if (hour > 12) {
                    hour = hour % 12;
                    $("#<%=txtProbableArriveHour.ClientID %>").val(hour);
                    $("#<%=ddlProbableArriveAMPM.ClientID %>").val('PM');
                }
                else {
                    $("#<%=ddlProbableArriveAMPM.ClientID %>").val('AM');
                }
            });
            $("#<%=txtPendingDeadlineHour.ClientID %>").blur(function () {
                var hourString = $("#<%=txtPendingDeadlineHour.ClientID %>").val();
                var hour = parseInt(hourString);
                if (hour > 12) {
                    hour = hour % 12;
                    $("#<%=txtPendingDeadlineHour.ClientID %>").val(hour);
                    $("#<%=ddlPendingDeadlineAmPm.ClientID %>").val('PM');
                }
                else {
                    $("#<%=ddlPendingDeadlineAmPm.ClientID %>").val('AM');
                }
            });
            $("#tblTableReserve").delegate("td > img.TablereservationDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var reservationId = $.trim($(this).parent().parent().find("td:eq(8)").text());
                    var params = JSON.stringify({ pkId: reservationId });

                    var $row = $(this).parent().parent();
                    //$(this).parent().parent().remove();
                    $.ajax({
                        type: "POST",
                        url: "/POS/frmRestaurantReservation.aspx/DeleteReservationRecord",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $row.remove();
                            //$(this).parent().parent().remove();
                            $("#myTabs").tabs('load', 5);
                        },
                        error: function (error) {
                        }
                    });
                }
            });
        });
        //------End Document Ready--------
        function CustomAlert(message, title, buttonText) {

            buttonText = (buttonText == undefined) ? "Ok" : buttonText;
            title = (title == undefined) ? "The page says:" : title;
            var div = $('<div>');
            div.html(message);
            div.attr('title', title);
            div.dialog({
                autoOpen: true,
                modal: true,
                draggable: false,
                resizable: false,
                buttons: [{
                    text: buttonText,
                    click: function () {
                        $(this).dialog("close");
                        div.remove();
                    }
                }]
            });
        }
        function GridPagingForSearchReservation(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#tblTableReserve tbody tr").length;

            var fromDate = $("#<%=txtFromDate.ClientID %>").val();
            var toDate = $("#<%=txtToDate.ClientID %>").val();
            var contactPerson = $("#<%=txtSrcContactPerson.ClientID %>").val();
            var reserveNo = $("#<%=txtSearchReservationNumber.ClientID %>").val();
            PageMethods.SearchResevationAndLoadGridInformation(fromDate, toDate, contactPerson, reserveNo, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSuccess, OnFail);
            return false;
        }
        function OnSuccess(result) {
            // alert('success');                    
            //$("#ltlTableWiseItemInformation").html(result);

            var format = innBoarDateFormat.replace('mm', 'MM');
            var format = format.replace('yy', 'yyyy');

            $("#tblTableReserve tbody tr").remove();
            $("#GridPagingContainerForSearchReservation ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#tblTableReserve tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#tblTableReserve tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:15%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.ReservationNumber + "</td>";
                tr += "<td align='left' style=\"width:25%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.ContactPerson + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.ReservationDate.format(format) + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.DateIn.format(format) + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.DateOut.format(format) + "</td>";
                tr += "<td align='left' style=\"width:10%; cursor:pointer;\" onClick= \"javascript:return SelectGuestInformation('" + gridObject.ReservationId + "')\">" + gridObject.ReservationMode + "</td>";
                tr += "<td align='right' style=\"width:5%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.ReservationId + "')\" alt='Edit Information' border='0' /></td>";
                tr += "<td align='right' style=\"width:5%; cursor:pointer;\"><img src='../Images/delete.png' class= 'TablereservationDelete'  alt='Delete Information' border='0' /></td>";
                tr += "<td align='right' style=\"width:5%; display:none;\">" + gridObject.ReservationId + "</td>";

                tr += "</tr>"

                $("#tblTableReserve tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainerForSearchReservation ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainerForSearchReservation ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainerForSearchReservation ul").append(result.GridPageLinks.NextButton);


            return false;
        }
        function OnFail(error) {
            alert(error.get_message());
        }
        function PerformEditAction(reservationId) {
            //alert('edit');
            var possiblePath = "frmRestaurantReservation.aspx?editId=" + reservationId;
            window.location = possiblePath;
        }

        function VisibleListedCompany() {
            if ($('#' + '<%=ddlReservedMode.ClientID%>').val() == "Company") {
                ListedCompanyVisibleTrue();
            }
            else {
                ListedCompanyVisibleFalse();
            }
        }

        function ToggleFieldVisible() {
            var ctrl = '#<%=chkIsLitedCompany.ClientID%>'
            if ($(ctrl).is(':checked')) {
                $('#ReservedCompany').hide("slow");
                $('#ListedCompany').show("slow");
                $('#PaymentInformation').show("slow");
            }
            else {
                $('#ListedCompany').hide("slow");
                $('#ReservedCompany').show("slow");
                $('#PaymentInformation').hide("slow");
            }
        }
        function ListedCompanyVisibleTrue() {
            $('#CompanyInformation').show();
        }
        function ListedCompanyVisibleFalse() {
            $('#CompanyInformation').hide();
        }
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        function CurrencyRateInfoEnable() {
            if ($("#<%=ddlCurrency.ClientID %>").val() == "45") {
                $("#<%=txtConversionRate.ClientID %>").val('');
                $('#CurrencyAmountInformationDiv').hide()
            }
            else {
                $('#CurrencyAmountInformationDiv').show()
            }
        }
        function LoadPopUp() {
            var txtFromDate = $("#<%=txtDateIn.ClientID %>").val();
            var txtToDate = $("#<%=txtDateOut.ClientID %>").val();

            if (txtFromDate == "") {
                CustomAlert('The Dining Date should not be empty.', 'Check In', 'Ok')
                document.getElementById("<%=txtDateIn.ClientID%>").focus();
                return false;
            }
            if (txtToDate == "") {
                CustomAlert('The Check Out Date should not be empty.', 'Check Out', 'Ok')
                document.getElementById("<%=txtToDate.ClientID%>").focus();
                return false;
            }

            // ClearRoomNumberAndId();
            LoadTableInformationWithControl();
            popup(1, 'DivTableSelect', '', 300, 500);
            return false;
        }
        function LoadTableInformationWithControl() {
            var CostCentreId = $("#<%=ddlCostCentreId.ClientID %>").val();
            var txtFromDate = $("#<%=txtDateIn.ClientID %>").val();
            var txtToDate = $("#<%=txtDateOut.ClientID %>").val();

            var arriveHourString = $("#<%=txtProbableArriveHour.ClientID %>").val();
            var arriveHourAMPM = $("#<%=ddlProbableArriveAMPM.ClientID %>").val();
            var arriveHour = parseInt(arriveHourString);
            var departHourString = $("#<%=txtProbableDepartHour.ClientID %>").val();
            var departHourAMPM = $("#<%=ddlProbableDepartAMPM.ClientID %>").val();
            var departHour = parseInt(departHourString);

            $("#<%=DateInHiddenField.ClientID %>").val(txtFromDate);
            $("#<%=DateOutHiddenField.ClientID %>").val(txtToDate);

            PageMethods.LoadTableInformationWithControl(CostCentreId, txtFromDate, txtToDate, arriveHour, departHour, arriveHourAMPM, departHourAMPM, OnLoadTableInformationWithControlSucceeded, OnLoadTableInformationWithControlFailed);

            return false;
        }
        function OnLoadTableInformationWithControlSucceeded(result) {
            //alert('success');
            $("#ltlTableNumberInfo").html(result);
            var res = "";
            var RoomIdList = $("#<%=txtSelectedTableId.ClientID %>").val();
            var RoomNumberList = $("#<%=txtSelectedTableNumbers.ClientID %>").val();

            var RoomNumberArray = RoomNumberList.split(",");
            var RoomArray = RoomIdList.split(",");
            if (RoomArray.length > 0) {

                for (var i = 0; i < RoomArray.length; i++) {
                    var roomId = "#" + RoomArray[i].trim();
                    var roomNumber = RoomNumberArray[i].trim();
                    $(roomId).attr("checked", true);
                    AddTableNumberAndIdTemporary(RoomArray[i].trim(), roomNumber);
                    res = RoomArray[i].toString() + res;
                }
            }
            return false;
        }
        function OnLoadTableInformationWithControlFailed(error) {
            toastr.error(error.get_message());
        }

        function SaveTableDetailsInformationByWebMethod() {
            var isEdit = false;
            if ($('#btnAddDetailGuest').val() == "Edit") {
                isEdit = true;
            }
            var txtSelectedTableNumbers = $("#<%=txtSelectedTableNumbers.ClientID %>").val();
            var txtSelectedTableId = $("#<%=txtSelectedTableId.ClientID %>").val();

            var txtTableId = $("#<%=txtTableId.ClientID %>").val();
            var ddlCostCentreId = $("#<%=ddlCostCentreId.ClientID %>").val();

            if (ddlCostCentreId == 0) {
                //MessagePanelShow();
                //$('#MessageBox').text('Please Provide Cost Centre.');
                toastr.warning('Please Provide Cost Centre.');
                return;
            }

            if (txtTableId == "") {
                //MessagePanelShow();
                //$('#MessageBox').text('Please Provide Table Quantity.');
                toastr.warning('Please Provide Table Quantity.');
                return;
            }

            var ddlCostCentreIdText = $("#<%=ddlCostCentreId.ClientID %>").text();
            var lblHiddenId = $("#<%=lblHiddenId.ClientID %>").text();
            var txtDiscountAmount = $("#<%=txtDiscountAmount.ClientID %>").val();
            var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();

            var ddlDiscountType = $("#<%=ddlDiscountType.ClientID %>").val();
            var ddlReservedMode = $("#<%=ddlReservedMode.ClientID %>").val();

            var prevCostCentreId = $("#<%=ddlCostCentreIdHiddenField.ClientID %>").val();

            $('#btnAddDetailGuest').val("Add");
            if (Isvalid) {
                //MessagePanelHide();
                //$('#MessageBox').text('');
                PageMethods.PerformSaveTableDetailsInformationByWebMethod(isEdit, txtSelectedTableNumbers, txtSelectedTableId, txtTableId, prevCostCentreId, ddlCostCentreId, ddlCostCentreIdText, lblHiddenId, txtDiscountAmount, ddlCurrency, ddlDiscountType, ddlReservedMode, OnPerformSaveTableDetailsSucceeded, OnPerformSaveTableDetailsFailed);
            }
            return false;
        }
        function OnPerformSaveTableDetailsFailed(error) {
            toastr.error(error.get_message());
        }
        function OnPerformSaveTableDetailsSucceeded(result) {
            //alert('data saved in session');
            $("#<%=ddlCostCentreIdHiddenField.ClientID %>").val('');
            $("#<%=txtSelectedTableId.ClientID %>").val('');
            $("#<%=txtSelectedTableNumbers.ClientID %>").val('');
            $("#ReservationDetailGrid").html(result);
            ClearReservationDeatil();
            $("#<%=txtEditedRoom.ClientID %>").val('');
            ClearRoomNumberAndId();
        }
        function Isvalid() {
            var txtTableId = $("#<%=txtTableId.ClientID %>").val();
            if (txtTableId == "" || parseInt(txtTableId) <= 0)
            { return false; }
            else {
                return true;
            }
        }

        function ClearReservationDeatil() {
            $("#<%=ddlCostCentreId.ClientID %>").val('1');
            $("#<%=txtDiscountAmount.ClientID %>").val('');
            $("#<%=txtTableId.ClientID %>").val('');
            $("#<%=lblAddedTableNumber.ClientID %>").text('');
            $("#btnAddDetailGuest").val('Add');
        }
        function ClearRoomNumberAndId() {
            popup(-1);
            $("#<%=txtSelectedTableNumbers.ClientID %>").val('');
            $("#<%=txtSelectedTableId.ClientID %>").val('');
            $("#<%=lblAddedTableNumber.ClientID %>").text('');
            $('#DivAddedTable').hide();
        }
        function AddTableNumberAndIdTemporary(TableId, TableNumber) {


            if ($('#' + TableId).is(":checked")) {
                // Add Room Ids
                var ids = $("#<%=txtSelectedTableId.ClientID %>").val();
                var splitedIds = ids.split(",");
                var flag = -1;
                for (var i = 0; i < splitedIds.length; i++) {
                    if (splitedIds[i].trim() == TableId) {
                        flag = 1;
                    }
                }
                if (flag == -1) {
                    ids = TableId + ',' + ids;
                }
                $("#<%=txtSelectedTableId.ClientID %>").val(ids);

                //Add Room Numbers
                var numbers = $("#<%=txtSelectedTableNumbers.ClientID %>").val();
                var splitedNumbers = numbers.split(",");
                var tableFlag = -1;
                for (var i = 0; i < splitedNumbers.length; i++) {
                    if (splitedNumbers[i].trim() == TableNumber) {
                        tableFlag = 1;
                    }
                }
                if (tableFlag == -1) {
                    numbers = TableNumber + ',' + numbers;
                }
                $("#<%=txtSelectedTableNumbers.ClientID %>").val(numbers)
            }
            else {
                //Delete Room Ids
                var ids = $("#<%=txtSelectedTableId.ClientID %>").val();
                var splitedIds = ids.split(",");
                var activeIds = "";
                for (var i = 0; i < splitedIds.length; i++) {
                    if (splitedIds[i].trim() == TableId) {
                        splitedIds[i] = -1;
                    }
                }
                for (var i = 0; i < splitedIds.length; i++) {
                    if (splitedIds[i].trim() != -1) {
                        activeIds = splitedIds[i].trim() + ',' + activeIds;
                    }
                }
                $("#<%=txtSelectedTableId.ClientID %>").val(activeIds);
                //Delete Room Numbers
                var tables = $("#<%=txtSelectedTableNumbers.ClientID %>").val();
                var splitedTables = tables.split(",");
                var activeIds = "";
                for (var i = 0; i < splitedTables.length; i++) {
                    if (splitedTables[i].trim() == TableNumber) {
                        splitedTables[i] = -1;
                    }
                }
                for (var i = 0; i < splitedTables.length; i++) {
                    if (splitedTables[i].trim() != -1) {
                        activeIds = splitedTables[i] + ',' + activeIds;
                    }
                }
                $("#<%=txtSelectedTableNumbers.ClientID %>").val(activeIds);
            }
        }
        function GetCheckedTableCheckBox() {
            var selectedTableIdArray = new Array();
            var selectedTableNumberArray = new Array();
            var SelectdTableId = "";
            var SelectdTableNumber = "";
            $('#TableWiseItemInformation input:checked').each(function () {
                SelectdTableId = SelectdTableId + $(this).attr('value') + ',';
                SelectdTableNumber = SelectdTableNumber + $(this).attr('name') + ',';
            });
            $("#<%=txtSelectedTableNumbers.ClientID %>").val(RomoveLastCommas(SelectdTableNumber));
            $("#<%=txtSelectedTableId.ClientID %>").val(RomoveLastCommas(SelectdTableId));
            ShowTableNumberAndId();
        }
        function ShowTableNumberAndId() {
            popup(-1);
            var ids = $("#<%=txtSelectedTableId.ClientID %>").val();
            var numbers = $("#<%=txtSelectedTableNumbers.ClientID %>").val();
            var addedTable = $("#<%=lblAddedTableNumber.ClientID %>").val()
            var splitedNumbers = numbers.split(",");
            var flag = "";
            for (var i = 0; i < splitedNumbers.length; i++) {
                if (splitedNumbers[i] != '') {
                    flag = splitedNumbers[i] + " , " + flag;
                }
            }
            flag = RemoveFirstCommas(flag);
            flag = RomoveLastCommas(flag);
            $("#<%=txtSelectedTableNumbers.ClientID %>").val(flag);
            if (splitedNumbers.length > 0) {
                $("#<%=lblAddedTableNumber.ClientID %>").text(flag);
                $('#DivAddedTable').show();
            }
            else {
                $("#<%=lblAddedTableNumber.ClientID %>").text('No Room Is Added.')
                $('#DivAddedTable').hide();
            }


            var tableIds = $("#<%=txtSelectedTableId.ClientID %>").val();
            var splitedTableId = tableIds.split(",");
            var tableIdFlag = "";
            for (var i = 0; i < splitedTableId.length; i++) {
                if (splitedNumbers[i] != '') {
                    tableIdFlag = splitedTableId[i] + " , " + tableIdFlag;
                }
            }
            tableIdFlag = RemoveFirstCommas(tableIdFlag);
            tableIdFlag = RomoveLastCommas(tableIdFlag);
            var tableArray = tableIdFlag.split(',');
            var tableLength = tableArray.length;
            $("#<%=txtSelectedTableId.ClientID %>").val(tableIdFlag);
            $("#<%=txtTableId.ClientID %>").val(tableLength);
        }
        function RemoveFirstCommas(flag) {
            var length = flag.length;
            var Index = 0;
            for (var j = 0; j < length; j++) {
                if (flag.charAt(j) == '0' || flag.charAt(j) == '1' || flag.charAt(j) == '2' || flag.charAt(j) == '3' || flag.charAt(j) == '4' || flag.charAt(j) == '5' || flag.charAt(j) == '6' || flag.charAt(j) == '7' || flag.charAt(j) == '8' || flag.charAt(j) == '9') {
                    Index = j;
                    break;
                }
            }
            flag = flag.substring(Index, length - Index);

            return flag;
        }
        function RomoveLastCommas(flag) {
            var length = flag.length;
            var Index = 0;
            var lastIndex = flag.lastIndexOf(',');
            flag = flag.substring(0, length - (length - lastIndex));
            return flag;
        }
        function PerformReservationDetailDelete(reservationDetailId) {
            PageMethods.PerformDeleteByWebMethod(reservationDetailId, OnPerformDeleteTableDetailsSucceeded, OnPerformDeleteTableDetailsFailed);
            return false;
        }

        function OnPerformDeleteTableDetailsSucceeded(result) {
            $("#ReservationDetailGrid").html(result);
            return false;
        }
        function OnPerformDeleteTableDetailsFailed(error) {
        }
        function PerformReservationDetailEdit(reservationDetailId) {
            $('#btnAddDetailGuest').val("Edit");
            PageMethods.PerformReservationDetailEditByWebMethod(reservationDetailId, OnPerformReservationDetailEditSucceeded, OnPerformReservationDetailEditFailed);
            return false;
        }
        function OnPerformReservationDetailEditSucceeded(result) {
            var TableIdList = result.TableNumberIdList;
            var TableNumberList = result.TableNumberList;
            var res = "";
            var TableArray = TableIdList.split(",");
            $("#<%=txtSelectedTableNumbers.ClientID %>").val(TableNumberList);
            $("#<%=txtSelectedTableId.ClientID %>").val(TableIdList);
            $("#<%=txtTableId.ClientID %>").val(result.TableQuantity);
            $("#<%=ddlCostCentreId.ClientID %>").val(result.CostCenterId);

            $("#<%=ddlCostCentreIdHiddenField.ClientID %>").val(result.CostCenterId);
            $("#<%=lblAddedTableNumber.ClientID %>").text(TableNumberList);

            $("#<%=ddlDiscountType.ClientID %>").val(result.DiscountType);
            $("#<%=txtDiscountAmount.ClientID %>").val(result.Amount);

            $('#DivAddedTable').show();
            // PerformFillFormActionByRoomTypeId($('#<%=ddlCostCentreId.ClientID%>').val());
            return false;
        }
        function OnPerformReservationDetailEditFailed(error) {
            toastr.error(error.get_message());
        }

        function GetTableDetailInformationByWM() {
            PageMethods.GetTableDetailGridInformationByWM(TableDetailInformationSucceeded, TableDetailInformationFailed);
            return false;
        }
        function TableDetailInformationSucceeded(result) {
            $("#ReservationDetailGrid").html(result);
            return false;
        }
        function TableDetailInformationFailed(error) {

            toastr.error(error.get_message());
        }

        //Item Details Information
        function LoadProductItem(itemType) {
            PageMethods.GetServiceByCriteria(itemType, OnFillServiceSucceeded, OnFillServiceFailed);
            return false;
        }
        function OnFillServiceSucceeded(result) {
            var list = result;
            var ddlItemId = '<%=ddlItemId.ClientID%>';
            var control = $('#' + ddlItemId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].ItemId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                }
            }

            return false;
        }
        function OnFillServiceFailed(error) {
            toastr.error(error.get_message());
        }

        function AddItemDetailsInfo(id, reservationId) {

            var itemTypeId = $("#<%=ddlItemType.ClientID %>").val();
            var itemId = $("#<%=ddlItemId.ClientID %>").val();
            var unitPrice = $("#<%=txtUnitPrice.ClientID %>").val();
            var itemUnit = $("#<%=txtItemUnit.ClientID %>").val();

            if ($('#' + unitPrice).val() != 0) {

                //var calculatedAmount = parseFloat($('#' + itemUnit).val()) * parseFloat($('#' + unitPrice).val());
                var calculatedAmount = itemUnit * unitPrice;

                if ($("#ltlTableWiseItemInformation > table").length > 0 && reservationId == 0) {
                    AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount);
                    return false;
                }
                else if ($("#ltlTableWiseItemInformation > table").length > 0 && reservationId > 0) {
                    AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount);
                    return false;
                }

                var table = "", deleteLink = "", editLink = "";

                deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                //editLink = "<a href=\"#\" onclick= 'EditReservationDetail(this)' ><img alt=\"Edit\" src=\"../Images/edit.png\" /></a>";
                table += "<table cellspacing='0' cellpadding='4' id='RecipeItemInformation' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                table += "<th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th align='left' scope='col' style='width: 45%;'>Item Name</th><th align='left' scope='col' style='width: 15%;'>Unit Price</th><th align='left' scope='col' style='width: 15%;'>Unit</th><th align='left' scope='col' style='width: 15%;'>Amount</th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";

                table += "<tbody>";
                table += "<tr style=\"background-color:#E3EAEB;\">";

                table += "<td align='left' style=\"display:none;\">" + id + "</td>";
                table += "<td align='left' style=\"display:none;\">" + reservationId + "</td>";
                table += "<td align='left' style=\"display:none;\">" + itemTypeId + "</td>";
                table += "<td align='left' style=\"display:none;\">" + $("#<%=ddlItemType.ClientID %> option:selected").text() + "</td>";
                table += "<td align='left' style=\"display:none;\">" + itemId + "</td>";
                table += "<td align='left' style=\"width:45%; text-align:Left;\">" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "</td>";
                table += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitPrice + "</td>";
                table += "<td align='left' style=\"width:15%; text-align:Left;\">" + itemUnit + "</td>";
                table += "<td align='left' style=\"width:15%; text-align:Left;\">" + calculatedAmount + "</td>";
                table += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

                table += "</tr>";
                table += "</tbody>";
                table += "</table>";

                $("#ltlTableWiseItemInformation").html(table);

                //CalculateTotalAddedPayment();

                $("#<%=ddlItemType.ClientID %>").val('');
                $("#<%=ddlItemId.ClientID %>").val('');
                $("#<%=txtUnitPrice.ClientID %>").val('');
                $("#<%=txtItemUnit.ClientID %>").val('');
                $("#ContentPlaceHolder1_chkIscomplementary").attr('checked', false);
            }
        }
        function AddNewRow(id, reservationId, itemTypeId, itemId, unitPrice, itemUnit, calculatedAmount) {
            var tr = "", totalRow = 0, deleteLink = "", editLink = "";
            totalRow = $("#RecipeItemInformation tbody tr").length;

            deleteLink = "<a href=\"#\" onclick= 'DeleteDetailInfo(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            //editLink = "<a href=\"#\" onclick= 'EditReservationDetail(this)' ><img alt=\"Edit\" src=\"../Images/edit.png\" /></a>";

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }
            tr += "<td align='left' style=\"display:none;\">" + id + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + reservationId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + itemTypeId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + $("#<%=ddlItemType.ClientID %> option:selected").text() + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + itemId + "</td>";
            tr += "<td align='left' style=\"width:45%; text-align:Left;\">" + $("#<%=ddlItemId.ClientID %> option:selected").text() + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitPrice + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itemUnit + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + calculatedAmount + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#RecipeItemInformation tbody").append(tr);
            //CalculateTotalAddedPayment();
            $("#<%=ddlItemType.ClientID %>").val('');
            $("#<%=ddlItemId.ClientID %>").val('');
            $("#<%=txtUnitPrice.ClientID %>").val('');
            $("#<%=txtItemUnit.ClientID %>").val('');
            $("#ContentPlaceHolder1_chkIscomplementary").attr('checked', false);
        }

        function DeleteDetailInfo(anchor) {
            ff = anchor;
            var tr = $(anchor).parent().parent();

            var detailId = $.trim($(tr).find("td:eq(0)").text());
            var reservationId = $.trim($(tr).find("td:eq(1)").text());

            if (parseInt(detailId, 10) != 0) {
                deleteObj.push({
                    DetailId: detailId,
                    ReservationId: reservationId
                });
            }

            $(tr).remove();            
            return false;
        }

        function ValidationNPreprocess() {            
            var saveObj = [];
            var detailId = 0, reservationId = 0, itemTypeId = 0, itemType = "", itemId = 0, itemName = "", itemUnit = 0, unitPrice = 0, totalPrice = 0;
            //var isComplementary = $("#<%=chkIscomplementary.ClientID %>").val();

            var rowLength = $("#ltlTableWiseItemInformation > table tbody tr").length;

            $("#ltlTableWiseItemInformation > table tbody tr").each(function () {
                detailId = parseInt($.trim($(this).find("td:eq(0)").text(), 10));
                reservationId = parseInt($.trim($(this).find("td:eq(1)").text(), 10));
                itemTypeId = parseInt($.trim($(this).find("td:eq(2)").text(), 10));
                itemType = $(this).find("td:eq(3)").text();
                itemId = parseInt($.trim($(this).find("td:eq(4)").text(), 10));
                itemName = $(this).find("td:eq(5)").text();
                unitPrice = parseFloat($.trim($(this).find("td:eq(6)").text(), 10));
                itemUnit = parseFloat($.trim($(this).find("td:eq(7)").text(), 10));
                totalPrice = parseFloat($.trim($(this).find("td:eq(8)").text(), 10));

                if (detailId == 0) {
                    saveObj.push({
                        DetailId: detailId,
                        ReservationId: reservationId,
                        ItemTypeId: itemTypeId,
                        ItemType: itemType,
                        ItemId: itemId,
                        ItemName: itemName,
                        ItemUnit: itemUnit,
                        UnitPrice: unitPrice,
                        TotalPrice: totalPrice
                    });
                }               
            });

            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveObj));            
            $("#<%=hfDeleteObj.ClientID %>").val(JSON.stringify(deleteObj));
        }
        
        //End Item Details
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div style="height: 45px">
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Reservation</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Reservation</a></li>
        </ul>
        <div id="tab-1">
            <asp:HiddenField ID="hfMinCheckInDate" runat="server" />
            <asp:HiddenField ID="txtHiddenItemId" runat="server" />
            <asp:HiddenField ID="txtReservationId" runat="server" />
            <asp:HiddenField ID="hfSaveObj" runat="server" />            
            <asp:HiddenField ID="hfDeleteObj" runat="server" />
            <div class="HMBodyContainer">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblDateIn" runat="server" Text="Dining Date"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:HiddenField ID="DateInHiddenField" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtDateIn" CssClass="datepicker" runat="server" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="Label1" runat="server" Text="Probable Arrival Time"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtProbableArriveHour" placeholder="12" CssClass="CustomTimeSize"
                            runat="server" TabIndex="2"></asp:TextBox>&nbsp;:
                        <asp:TextBox ID="txtProbableArriveMinute" placeholder="00" CssClass="CustomMinuteSize"
                            TabIndex="3" runat="server" disabled></asp:TextBox>
                        <asp:DropDownList ID="ddlProbableArriveAMPM" CssClass="CustomAMPMSize" runat="server"
                            TabIndex="4">
                            <asp:ListItem>AM</asp:ListItem>
                            <asp:ListItem>PM</asp:ListItem>
                        </asp:DropDownList>
                        (12:00AM)
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblDateOut" runat="server" Text="Check Out Date"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:HiddenField ID="DateOutHiddenField" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtEditedRoom" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtEditedRoomNumber" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtDateOut" runat="server" CssClass="datepicker" TabIndex="5"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="Label2" runat="server" Text="Probable Arrival Time"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtProbableDepartHour" placeholder="12" CssClass="CustomTimeSize"
                            runat="server" TabIndex="2"></asp:TextBox>&nbsp;:
                        <asp:TextBox ID="txtProbableDepartMinute" placeholder="00" CssClass="CustomMinuteSize"
                            TabIndex="3" runat="server" disabled></asp:TextBox>
                        <asp:DropDownList ID="ddlProbableDepartAMPM" CssClass="CustomAMPMSize" runat="server"
                            TabIndex="4">
                            <asp:ListItem>AM</asp:ListItem>
                            <asp:ListItem>PM</asp:ListItem>
                        </asp:DropDownList>
                        (12:00AM)
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblReservedMode" runat="server" Text="Reservation Mode"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlReservedMode" runat="server" TabIndex="7">
                            <asp:ListItem>Personal</asp:ListItem>
                            <asp:ListItem>Company</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblReservationType" runat="server" Text="Reserve Type"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:DropDownList ID="ddlReservationType" runat="server" TabIndex="6">
                            <asp:ListItem>Online</asp:ListItem>
                            <asp:ListItem>Telephone</asp:ListItem>
                            <asp:ListItem>Fax</asp:ListItem>
                            <asp:ListItem>Email</asp:ListItem>
                            <asp:ListItem>Direct</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="CompanyInformation" style="display: none;">
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:CheckBox ID="chkIsLitedCompany" runat="server" Text="" onclick="javascript: return ToggleFieldVisible();"
                                TabIndex="9" />
                            <asp:Label ID="lblCompany" runat="server" Text="Listed Company"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <div id="ListedCompany" style="display: none;">
                                <asp:DropDownList ID="ddlCompanyName" runat="server" TabIndex="10" CssClass="ThreeColumnDropDownList">
                                </asp:DropDownList>
                            </div>
                            <div id="ReservedCompany">
                                <asp:TextBox ID="txtReservedCompany" runat="server" TabIndex="11" CssClass="ThreeColumnTextBox"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblBusinessPromotionId" runat="server" Text="Business Promotion"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlBusinessPromotionId" runat="server" CssClass="ThreeColumnDropDownList"
                            TabIndex="8" MinLength="300">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblContactAddress" runat="server" Text="Contact Address"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtContactAddress" runat="server" CssClass="ThreeColumnTextBox"
                                TextMode="MultiLine" TabIndex="11" MaxLength="300"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblContactPerson" runat="server" Text="Contact Person"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="ThreeColumnTextBox" TabIndex="11"
                                MaxLength="150"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblContactNumber" runat="server" Text="Contact Number"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtContactNumber" runat="server" TabIndex="12"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblContactEmail" runat="server" Text="Contact Email"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtContactEmail" runat="server" TabIndex="13"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="Label3" runat="server" Text="Mobile Number"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtMobileNumber" runat="server" TabIndex="14"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="Label4" runat="server" Text="Fax Number"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtFaxNumber" runat="server" TabIndex="15"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="PaymentInformation" style="display: none;">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblPaymentMode" runat="server" Text="Payment Mode"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlPaymentMode" runat="server" TabIndex="16">
                                <asp:ListItem>Company</asp:ListItem>
                                <asp:ListItem>Self</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblPayFor" runat="server" Text="Pay For"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:DropDownList ID="ddlPayFor" runat="server" TabIndex="17">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblCurrency" runat="server" Text="Currency Type"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlCurrency" runat="server" TabIndex="16">
                        </asp:DropDownList>
                    </div>
                    <div id="CurrencyAmountInformationDiv" style="display: none;">
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblCurrencyAmount" runat="server" Text="Conversion Rate"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtConversionRate" runat="server" Text="81"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="childDivSection">
                    <div id="TableDetailsInformation" class="block">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Table Detailed Information
                        </a>
                        <div class="HMBodyContainer">
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblCostCentreId" runat="server" Text="Cost Center"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlCostCentreId" runat="server" TabIndex="16">
                                    </asp:DropDownList>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblNumberOfTable" runat="server" Text="Table Quantity"></asp:Label>
                                    <span class="MandatoryField">*</span>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtTableId" runat="server" CssClass="CustomTimeSize" TabIndex="17"></asp:TextBox>
                                    <input type="button" tabindex="18" id="btnChange" value="Table Number" class="TransactionalButton btn btn-primary"
                                        onclick="javascript:return LoadPopUp()" />
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblDiscountType" runat="server" Text="Discount Type"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlDiscountType" runat="server" TabIndex="16">
                                        <asp:ListItem>Fixed</asp:ListItem>
                                        <asp:ListItem>Percentage</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblAmount" runat="server" Text="Discount Amount"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtDiscountAmount" runat="server" TabIndex="17"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection" id="DivAddedTable" style="display: none">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblAddedTable" runat="server" Text="Added Table"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:Label ID="lblAddedTableNumber" CssClass="ThreeColumnTextBox" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <%--Right Left--%>
                                <input type="button" id="btnAddDetailGuest" tabindex="19" value="Add" class="TransactionalButton btn btn-primary" />
                                <asp:Label ID="lblHiddenId" runat="server" Text='' Visible="False"></asp:Label>
                                <asp:HiddenField ID="ddlCostCentreIdHiddenField" runat="server"></asp:HiddenField>
                            </div>
                            <div class="divClear">
                            </div>
                            <div id="ReservationDetailGrid">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="childDivSection">
                    <div id="ItemDetailsInformation" class="block">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Item Detail Information
                        </a>
                        <div class="HMBodyContainer">
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server" />
                                    <asp:Label ID="lblItemType" runat="server" Text="Category Name"></asp:Label>
                                    <span class="MandatoryField">*</span>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlItemType" runat="server" TabIndex="20">
                                    </asp:DropDownList>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblItemId" runat="server" Text="Item Name"></asp:Label>
                                    <span class="MandatoryField">*</span>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:HiddenField ID="txtHiddenProductId" runat="server" />
                                    <asp:DropDownList ID="ddlItemId" runat="server" TabIndex="21">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblUnitPrice" runat="server" Text="Unit Price"></asp:Label>
                                    <span class="MandatoryField">*</span>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtUnitPrice" runat="server" TabIndex="22"></asp:TextBox>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblItemUnit" runat="server" Text="Item Unit"></asp:Label>
                                    <span class="MandatoryField">*</span>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtItemUnit" runat="server" TabIndex="23"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <div style="float: left">
                                        <asp:CheckBox ID="chkIscomplementary" TabIndex="24" runat="Server" Text=" Is it a complementary Item?"
                                            Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="HMContainerRowButton">
                                    <%--Right Left--%>
                                    <input type="button" id="btnAddDetailItem" value="Add" class="TransactionalButton btn btn-primary" />
                                    <%--<asp:Button ID="btnAddDetailGuest" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary"
                                            TabIndex="25" OnClick="btnAddDetailGuest_Click" />--%>
                                    <asp:Label ID="Label6" runat="server" Text='' Visible="False"></asp:Label>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="block-body collapse in">
                                    <div id="ltlTableWiseItemInformation" runat="server" clientidmode="Static">
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div id="TotalPaid" class="totalAmout">
                                </div>
                                <div class="divClear">
                                </div>
                                <div id="DueTotal" class="totalAmout">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="Label5" runat="server" Text="Reference"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlReferenceId" runat="server" TabIndex="20">
                        </asp:DropDownList>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblReservationMode" runat="server" Text="Reservation Status"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:DropDownList ID="ddlReservationMode" runat="server" CssClass="customMediupXLDropDownSize"
                            TabIndex="21">
                            <asp:ListItem Value="Active">Active</asp:ListItem>
                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                            <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection" id="PendingDiv">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblPendingDeadlineDate" runat="server" Text="Confirmation Date"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtConfirmationDate" CssClass="datepicker" runat="server" TabIndex="22"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblPendingDeadlineTime" runat="server" Text="Probable Arrival Time"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtPendingDeadlineHour" placeholder="12" CssClass="CustomTimeSize"
                            runat="server" TabIndex="23"></asp:TextBox>&nbsp;:
                        <asp:TextBox ID="txtPendingDeadlineMin" placeholder="00" CssClass="CustomMinuteSize"
                            TabIndex="24" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="ddlPendingDeadlineAmPm" CssClass="CustomAMPMSize" runat="server"
                            TabIndex="25">
                            <asp:ListItem>AM</asp:ListItem>
                            <asp:ListItem>PM</asp:ListItem>
                        </asp:DropDownList>
                        (12:00AM)
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection" id="ReasonDiv">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblReason" runat="server" Text="Cancel Reason "></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtReason" runat="server" CssClass="ThreeColumnTextBox" TextMode="MultiLine"
                            TabIndex="26"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="ThreeColumnTextBox" TextMode="MultiLine"
                            TabIndex="27"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="divClear">
        </div>
        <div id="tab-2">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker" TabIndex="63"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtToDate" runat="server" TabIndex="64" CssClass="datepicker"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblSrcContactPerson" runat="server" Text="Contact Person"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtSrcContactPerson" runat="server" TabIndex="65"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblSearchReservationNumber" runat="server" Text="Reservation Number"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtSearchReservationNumber" runat="server" TabIndex="66"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <button type="button" id="btnSrchRsrvtn" class="TransactionalButton btn btn-primary">
                    Search</button>
                <asp:Button ID="btnClearSearch" runat="server" TabIndex="67" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                    OnClientClick="javascript: return PerformClearSearchAction();" />
            </div>
            <div class="divClear">
            </div>
            <div id="SearchPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
                </a>
                <div class="block-body collapse in">
                    <table cellspacing='0' cellpadding='4' id='tblTableReserve' width="100%">
                        <colgroup>
                            <col style="width: 15%;" />
                            <col style="width: 29%;" />
                            <col style="width: 12%;" />
                            <col style="width: 12%;" />
                            <col style="width: 12%;" />
                            <col style="width: 10%;" />
                            <col style="width: 5%;" />
                            <col style="width: 5%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    Reservation No.
                                </td>
                                <td>
                                    Contact Person
                                </td>
                                <td>
                                    Reservation Date
                                </td>
                                <td>
                                    Date-In
                                </td>
                                <td>
                                    Date-Out
                                </td>
                                <td>
                                    Status
                                </td>
                                <td style="text-align: right;">
                                    Edit
                                </td>
                                <td style="text-align: right;">
                                    Delete
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="divClear">
                    </div>
                    <div class="childDivSection">
                        <div class="pagination pagination-centered" id="GridPagingContainerForSearchReservation">
                            <ul>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
    <div class="HMContainerRowButton" id="SubmitButtonDiv">
        <%--Right Left--%>
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
            TabIndex="80" OnClick="btnSave_Click" OnClientClick="javascript:return ValidationNPreprocess();" />
        <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
            TabIndex="81" />
    </div>
    <div id="DivTableSelect" style="display: none;">
        <div id="Div1" class="block">
            <asp:HiddenField ID="txtSelectedTableNumbers" runat="server" />
            <asp:HiddenField ID="txtSelectedTableId" runat="server" />
            <div id="ltlTableNumberInfo">
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }         
    </script>
</asp:Content>
