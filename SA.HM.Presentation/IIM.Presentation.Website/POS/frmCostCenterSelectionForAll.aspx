<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCostCenterSelectionForAll.aspx.cs" MasterPageFile="~/POS/RestaurantMM.Master"
    Inherits="HotelManagement.Presentation.Website.POS.frmCostCenterSelectionForAll" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Header" ContentPlaceHolderID="head" runat="server">
    <%--<meta http-equiv="refresh" content="5" />--%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        
        var innBoarDateFormat = "";
        var OrderDetailsFlag = null;
        var vc = [];
        var UserInfoFromDB = null;

        //Page Refresh when idle for 10 seconds
        var time = new Date().getTime();
        $(document.body).bind("mousemove keypress", function (e) {
            time = new Date().getTime();
        });
        function refresh() {
            if (new Date().getTime() - time >= 30000)
                window.location.reload(true);
            else
                setTimeout(refresh, 30000);
        }
        setTimeout(refresh, 30000);
        //ended Page Refresh when idle for 10 seconds

        $(document).ready(function () {
            //debugger;
            //if ($("#ContentPlaceHolder1_hfOrderType").val() == "Table") {
            //    debugger;
            //    LoadTableInfo($("#ContentPlaceHolder1_hfOrderCostcenterId").val());
            //}
            if ($("#hfUserInfoObj").val() !== "") {
                UserInfoFromDB = JSON.parse($("#hfUserInfoObj").val());
                $("#hfUserInfoObj").val("");
            }

            
            if (Cookies.getJSON('OrderDetails') != undefined) {

                if ($("#ContentPlaceHolder1_hfIsChef").val() == "0") {
                    var OrderDetailsCookies = Cookies.getJSON('OrderDetails');
                    if (OrderDetailsCookies.OrderType == "tbl") {
                        LoadTableInfo(OrderDetailsCookies.OrderCostcenterId);
                    }
                    else if (OrderDetailsCookies.OrderType == "rom") {

                        LoadRoomInfo(OrderDetailsCookies.OrderCostcenterId, 'Room Service');

                    }
                    Cookies.remove('OrderDetails');

                }

            }

            $("#myTabs").tabs();
            $("#myTabsPopUp").tabs();
            $(".roomChangeContainer").hide();

            $("#btnTokenList").click(function () {
                CommonHelper.SpinnerOpen();
                PageMethods.LoadTokenInformation(OnLoadTokenInformationSucceeded, OnLoadTokenInformationFailed);
                return false;
            });

            if ($("#ContentPlaceHolder1_hfIsBearar").val() == "1") {
                $("#btnBillReprint").hide();
            }

            $("#btnBillReprint").click(function () {

                $("#ContentPlaceHolder1_txtBillId").val("");

                CommonHelper.TouchScreenNumberKeyboardWithoutDot("numkbnotdecimal", "KeyBoardContainer");
                var keyboard = $('.numkbnotdecimal').getkeyboard();
                keyboard.reveal();

                if ($("#ContentPlaceHolder1_hfIsCostCenterWiseBillNumberGenerate").val() == "0") {
                    $("#BillReprintCostcenterPanel").hide();

                    $("#BillNoKeyContainer").removeClass("col-sm-7");
                    $("#BillNoKeyContainer").addClass("col-sm-12");

                    $("#TouchKeypad").dialog({
                        width: 420,
                        height: 400,
                        autoOpen: true,
                        modal: true,
                        closeOnEscape: true,
                        resizable: false,
                        title: "Bill Number",
                        show: 'slide',
                        open: function (event, ui) {
                            $('#TouchKeypad').css('overflow', 'hidden');
                        }
                    });
                }
                else {
                    $("#TouchKeypad").dialog({
                        width: 800,
                        autoOpen: true,
                        modal: true,
                        closeOnEscape: true,
                        resizable: false,
                        fluid: true,
                        height: 'auto',
                        title: "Bill Number",
                        show: 'slide',
                        open: function (event, ui) {
                            $('#TouchKeypad').css('overflow', 'hidden');
                        }
                    });
                }

            });
            function showTab() {
                $('#ContentPlaceHolder1_Li2').show();
                $('#ContentPlaceHolder1_Li3').show();
                $('#roomInfoContainer2').show();
            }
            function hideTab() {
                $('#ContentPlaceHolder1_Li2').hide();
                $('#ContentPlaceHolder1_Li3').hide();
                $('#roomInfoContainer2').hide();
            }

            function changeClassSize() {
                if ($("#roomInfoContainer").hasClass('col-sm-6')) {
                    $("#roomInfoContainer").removeClass("col-sm-6");
                    $("#roomInfoContainer").addClass("col-sm-12");
                }
                else if ($("#roomInfoContainer").hasClass('col-sm-12')) {
                    $("#roomInfoContainer").removeClass("col-sm-12");
                    $("#roomInfoContainer").addClass("col-sm-6");
                }
            }
            $("#chkMakeOrder").change(function () {
                if ($(this).is(":checked")) {
                    $(".roomChangeContainer").hide();
                    $("#chkChangeRoom").prop("checked", false);
                    showTab();
                    changeClassSize();
                }
                else {
                    $("#chkChangeRoom").prop("checked", true);
                    $(".roomChangeContainer").show();
                    hideTab();
                    changeClassSize();
                }
            });
            $("#chkChangeRoom").change(function () {
                if ($(this).is(":checked")) {
                    $(".roomChangeContainer").show();
                    $("#chkMakeOrder").prop("checked", false);
                    hideTab();
                    changeClassSize();
                }
                else {
                    $(".roomChangeContainer").hide();
                    $("#chkMakeOrder").prop("checked", true);
                    showTab();
                    changeClassSize();
                }
            });

            $("#ContentPlaceHolder1_txtWaiter").autocomplete({
                source: function (request, response) {

                    var hfCostCenterIdVal = $("#ContentPlaceHolder1_hfCostcenterId").val()

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../../../Common/WebMethodPage.aspx/GetBearerInfoByAutoSearch",
                        data: "{'beararName':'" + request.term + "','costCenterId':'" + hfCostCenterIdVal + "'}",
                        dataType: "json",
                        success: function (data) {

                            if (data.d == '') {
                                $("#ContentPlaceHolder1_txtWaiter").val("");
                                $("#ContentPlaceHolder1_hfBillOrderTakingWaiterId").val("");
                            }

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    UserInfoId: m.UserInfoId,
                                    label: m.UserName,
                                    value: m.BearerId
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    $(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);

                    $("#ContentPlaceHolder1_txtWaiter").autocomplete("enable");

                    $("#ContentPlaceHolder1_txtWaiter").val(ui.item.label);
                    $("#ContentPlaceHolder1_hfBillOrderTakingWaiterId").val(ui.item.value);
                }

            }).autocomplete("option", "appendTo", "#PaxAndWaiterDialog");

        });

        function PaxConfirmation(sourceType, sourceId, costCenterId, kotId) {


            if ($("#ContentPlaceHolder1_IsRestaurantPaxConfirmationEnable").val() == "0" && $("#ContentPlaceHolder1_hfIsBearar").val() == "1") {
                ResumeBill(sourceType, sourceId, costCenterId, kotId);
                return true;
            }

            $("#ContentPlaceHolder1_hfCostcenterId").val(costCenterId);
            $("#ContentPlaceHolder1_hfRouteDetails").val(sourceType + "," + sourceId + "," + costCenterId + "," + kotId);

            CommonHelper.TouchScreenNumberKeyboardWithoutDot("numkbnotdecimalpax", "KeyBoardContainerQuantityChange");
            var keyboard = $('.numkbnotdecimalpax').getkeyboard();
            keyboard.reveal();

            var width = 900, height = 425, titles = 'Waiter & Pax Selection';

            if ($("#ContentPlaceHolder1_IsRestaurantPaxConfirmationEnable").val() == "0") {
                $("#PaxConfirmationDivPanel").hide();
                width = 500;
                //height = 200;
                height = 425;
                titles = "Waiter Selection";

                if ($("#WaiterChangeDialog").hasClass('col-md-6')) {
                    $("#WaiterChangeDialog").removeClass("col-md-6");
                    $("#WaiterChangeDialog").addClass("col-md-12");
                }
                else if ($("#PaxConfirmationDivPanel").hasClass('col-sm-12')) {
                    $("#PaxConfirmationDivPanel").removeClass("col-sm-12");
                }
            }

            if ($("#ContentPlaceHolder1_hfIsBearar").val() == "1") {
                $("#WaiterChangeDialog").hide();
                width = 500;
                titles = "Pax Confirmation";

                if ($("#PaxConfirmationDivPanel").hasClass('col-md-6')) {
                    $("#PaxConfirmationDivPanel").removeClass("col-md-6");
                    $("#PaxConfirmationDivPanel").addClass("col-md-12");
                }
                else if ($("#WaiterChangeDialog").hasClass('col-md-12')) {
                    $("#WaiterChangeDialog").removeClass("col-md-12");
                }
            }
            else {
                if (($("#ContentPlaceHolder1_IsRestaurantWaiterConfirmationEnable").val() == "0") && ($("#ContentPlaceHolder1_IsRestaurantPaxConfirmationEnable").val() == "0")) {
                    ResumeBill(sourceType, sourceId, costCenterId, kotId);
                    return true;
                }
            }

            if ($("#ContentPlaceHolder1_IsRestaurantWaiterConfirmationEnable").val() == "0") {
                $("#WaiterChangeDialog").hide();
                if ($("#ContentPlaceHolder1_IsRestaurantPaxConfirmationEnable").val() == "1") {
                    titles = "Pax Confirmation";
                    if ($("#PaxConfirmationDivPanel").hasClass('col-md-6')) {
                        $("#PaxConfirmationDivPanel").removeClass("col-md-6");
                        $("#PaxConfirmationDivPanel").addClass("col-md-12");
                    }
                }


            }
            if ($("#ContentPlaceHolder1_IsRestaurantPaxConfirmationEnable").val() == "0") {
                $("#PaxConfirmationDivPanel").hide();
            }

            $("#PaxAndWaiterDialog").dialog({
                autoOpen: true,
                modal: true,
                width: width,
                height: height,
                fluid: true,
                closeOnEscape: true,
                resizable: false,
                title: titles,
                show: 'slide',
                open: function (event, ui) {
                    $('#PaxAndWaiterDialog').css('overflow', 'hidden');
                    //$(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                }
            });
            return false;
        }
        function PaxConfirmationOk() {


            if ($("#ContentPlaceHolder1_IsRestaurantPaxConfirmationEnable").val() == "1") {

                if ($.trim($("#ContentPlaceHolder1_txtConfirmPax").val()) == "") {
                    toastr.warning("Pax Cannot Blank.");
                    CommonHelper.SpinnerClose();
                    return false;
                }
                else if (parseInt($("#ContentPlaceHolder1_txtConfirmPax").val()) == 0) {
                    toastr.warning("Pax Cannot Zero (0).");
                    CommonHelper.SpinnerClose();
                    return false;
                }

                $("#ContentPlaceHolder1_hfPaxConfirmationQuantity").val($("#ContentPlaceHolder1_txtConfirmPax").val());
            }

            //if ($("#ContentPlaceHolder1_IsRestaurantWaiterConfirmationEnable").val() == "1") {
            if ($("#ContentPlaceHolder1_IsRestaurantWaiterConfirmationEnable").val() != "0" && $("#ContentPlaceHolder1_hfIsBearar").val() == "0") {
                if ($("#ContentPlaceHolder1_hfBillOrderTakingWaiterId").val() == "") {
                    toastr.warning("Invalid Waiter. Please Give Vaild Waiter Name and Then Search.");
                    return false;
                }
            }

            $("#PaxAndWaiterDialog").dialog("close");
            var keyboard = $('.numkbnotdecimalpax').getkeyboard();
            keyboard.destroy();

            var rd = $("#ContentPlaceHolder1_hfRouteDetails").val();
            var route = rd.split(',');

            var sourceType = route[0];
            var sourceId = route[1];
            var costCenterId = route[2];
            var kotId = route[3];

            ResumeBill(sourceType, sourceId, costCenterId, kotId);
        }
        function PaxConfirmationDialogClose() {
            $("#PaxAndWaiterDialog").dialog("close");
            var keyboard = $('.numkbnotdecimalpax').getkeyboard();
            keyboard.destroy();
        }


        function OnLoadTokenInformationSucceeded(result) {

            $("#TokenListContainer").html(result);

            $("#TokenListDialog").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 820,
                minHeight: 550,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Token List",
                show: 'slide'
            });

            CommonHelper.SpinnerClose();

            return false;
        }
        function OnLoadTokenInformationFailed(result) {
            CommonHelper.SpinnerClose();
        }

        function LoadTokenInfo(sourceType, costCenterId, costcenter) {
            CommonHelper.SpinnerOpen();
            $("#ContentPlaceHolder1_hfCostCenter").val(costcenter);
            PageMethods.LoadTokenInfo(sourceType, costCenterId, OnLoadTokenInfoSucceeded, OnShowOutOfServiceRoomInformationFailed);
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadTokenInfoSucceeded(result) {
            vc = result;
            if (result != "") {


                var isBearar = ($("#ContentPlaceHolder1_hfIsBearar").val() == "0" ? false : true);
                var beararId = "0";

                if (isBearar == false) {
                    beararId = $("#ContentPlaceHolder1_hfBillOrderTakingWaiterId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfBillOrderTakingWaiterId").val();
                }

                window.location = "/POS/frmOrderManagement.aspx?st= " + result[0].SourceType + "&sid=" + result[0].TokenNumber + "&cc=" + result[0].CostcenterId + "&kid=0" + "&bid=" + beararId;
            }
        }

        function RestaurantBillOptions(sourceType, sourceId, costCenterId, kotId) {
            PageMethods.LoadOccupiedPossiblePath(sourceType, sourceId, costCenterId, kotId, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
            return false;
        }

        function ResumeBill(sourceType, sourceId, costCenterId, kotId) {

            OrderDetailsFlag = {
                SourceType: $.trim(sourceType),
                SourceId: sourceId,
                CostCenterId: costCenterId,
                KotId: parseInt(kotId, 0)
            };

            if (sourceType == 'tbl' && kotId == "0" && OrderDetailsFlag == null) {

                if ($("#ContentPlaceHolder1_IsRestaurantPaxConfirmationEnable").val() == "0") {
                    $.confirm({
                        title: 'Confirm!',
                        content: 'Do you want to make order ?',
                        buttons: {
                            Yes: function () {
                                ResumeBill(OrderDetailsFlag.SourceType, OrderDetailsFlag.SourceId, OrderDetailsFlag.CostCenterId, OrderDetailsFlag.KotId);
                            },
                            No: function () {
                                OrderDetailsFlag = null;
                            }
                        }
                    });
                }
                else {
                    ResumeBill(OrderDetailsFlag.SourceType, OrderDetailsFlag.SourceId, OrderDetailsFlag.CostCenterId, OrderDetailsFlag.KotId);
                }
                return false;
            }

            if ($.trim(sourceType) == 'rom') {
                costCenterId = parseInt($("#ContentPlaceHolder1_hfCostcenterId").val(), 10);
                sourceId = parseInt($("#ContentPlaceHolder1_hfRoomId").val(), 10);
                kotId = parseInt($("#ContentPlaceHolder1_hfKotId").val(), 10);

                OrderDetailsFlag = {
                    SourceType: $.trim(sourceType),
                    SourceId: sourceId,
                    CostCenterId: costCenterId,
                    KotId: parseInt(kotId, 0)
                };
            }

            if (sourceType == 'tbl') {
                PageMethods.CheckOccupiedTableInfo(costCenterId, sourceId, kotId, OnCheckOccupiedSucceeded, OnCheckOccupiedFailed);
            }
            else {
                var v = { IsSuccess: true }
                OnCheckOccupiedSucceeded(v);
            }

            return false;
        }
        function OnCheckOccupiedSucceeded(result) {
            if (result.IsSuccess) {

                //OrderDetailsFlag.SourceType, OrderDetailsFlag.SourceId, OrderDetailsFlag.CostCenterId, OrderDetailsFlag.KotId
                var OrderDetailsCookies = {
                    OrderType: OrderDetailsFlag.SourceType,
                    OrderCostcenterId: OrderDetailsFlag.CostCenterId
                };

                Cookies.set('OrderDetails', OrderDetailsCookies, { expires: 1 });

                if (OrderDetailsFlag.KotId == 0) {

                    var isBearar = ($("#ContentPlaceHolder1_hfIsBearar").val() == "0" ? false : true);
                    var beararId = "0";

                    if (isBearar == false) { beararId = $("#ContentPlaceHolder1_hfBillOrderTakingWaiterId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfBillOrderTakingWaiterId").val(); }

                    if ($("#ContentPlaceHolder1_IsRestaurantPaxConfirmationEnable").val() == "1") {

                        var paxQuantity = $("#ContentPlaceHolder1_hfPaxConfirmationQuantity").val();
                        if (paxQuantity == "") { paxQuantity = "1"; }

                        window.location = "/POS/frmOrderManagement.aspx?ot=no&st= " + $.trim(OrderDetailsFlag.SourceType) + "&sid=" + OrderDetailsFlag.SourceId + "&cc=" + OrderDetailsFlag.CostCenterId + "&kid=" + OrderDetailsFlag.KotId + "&pax=" + paxQuantity + "&bid=" + beararId;
                    }
                    else {
                        window.location = "/POS/frmOrderManagement.aspx?ot=no&st= " + $.trim(OrderDetailsFlag.SourceType) + "&sid=" + OrderDetailsFlag.SourceId + "&cc=" + OrderDetailsFlag.CostCenterId + "&kid=" + OrderDetailsFlag.KotId + "&bid=" + beararId;
                    }
                }
                else {
                    PageMethods.LoadHoldupBill($.trim(OrderDetailsFlag.SourceType), OrderDetailsFlag.SourceId, OrderDetailsFlag.CostCenterId, OrderDetailsFlag.KotId, OnLoadHoldupBillSucceeded, OnHoldupBillFailed);
                    //window.location = "/Restaurant/frmRestaurantOrderManagement.aspx?st= " + sourceType + "&sid=" + sourceId + "&cc=" + costCenterId + "&kid=" + kotId;
                }
            }
            else {
                toastr.warning("Table Is Occupied In Different Way. Window Is Refreshing. Plesae Wait.");
                setTimeout(function () { window.location = "frmCostCenterSelectionForAll.aspx"; }, 2000);
            }

        }
        function OnCheckOccupiedFailed() {
        }

        function OnLoadHoldupBillSucceeded(result) {
            if (result.IsSuccess == true) {
                window.location = result.RedirectUrl;
            }
            else {
                CommonHelper.SpinnerOpen();
                CommonHelper.AlertMessage(result.AlertMessage);

                if ($('#TokenListDialog').is(':visible') == true)
                    $("#TokenListDialog").dialog("close");

                PageMethods.LoadTokenInformation(OnLoadTokenInformationSucceeded, OnLoadTokenInformationFailed);
            }
        }

        function OnHoldupBillFailed() { }

        function GoToLogOutPanel() {
            window.location = "/Logout.aspx?LoginType=RestaurentLogin";
            return false;
        }
        function RetailPosInfo(costcenterId, costcenter) {
            window.location = "/POS/frmRetailPos.aspx?cid=" + costcenterId;
            return false;
        }

        function PosBillingInfo(costcenterId, costcenter) {
            window.location = "/POS/Billing.aspx?cid=" + costcenterId;
            return false;
        }
        function SOBillingInfo(costcenterId, costcenter) {
            window.location = "/POS/frmSOBilling.aspx?cid=" + costcenterId;
            return false;
        }
        function LoadTableInfo(costcenterId, costcenter) {

            $("#ContentPlaceHolder1_hfCostCenter").val(costcenter);

            PageMethods.LoadTableInfo(costcenterId, OnLoadTableInfoSucceeded, OnLoadTableInfoFailed);
            return false;
        }
        function OnLoadTableInfoSucceeded(result) {
            if (result.IsWaiter) {
                $("#TableContainer").html(result.TableInfo);
                $("#OccupiedTableList").html(result.OccupiedTableInfo);

                $("#TableInfoDialog").dialog({
                    width: 1000,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    height: 'auto',
                    title: $("#ContentPlaceHolder1_hfCostCenter").val(),
                    show: 'slide'
                });
            }
            else {
                $("#TableContainerForCashier").html(result.TableInfo);
                $("#OccupiedTableListForCashier").html(result.OccupiedTableInfo);
                $("#OccupiedTableInfoByWaiter").html(result.OccupiedTableInfoByWaiter);

                $("#TableInfoDialogCashier").dialog({
                    width: 1000,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    height: 'auto',
                    title: $("#ContentPlaceHolder1_hfCostCenter").val(),
                    show: 'slide'
                });
            }
        }
        function OnLoadTableInfoFailed() { }

        




        function LoadRoomInfo(costcenterId, costcenter) {
            $("#ContentPlaceHolder1_hfCostcenterId").val(costcenterId);
            $("#ContentPlaceHolder1_hfCostCenter").val(costcenter);

            PageMethods.LoadRoomAllocation(OnLoadRoomInfoSucceeded, OnLoadRoomInfoFailed);
            return false;
        }
        function OnLoadRoomInfoSucceeded(result) {
            $("#RoomListInfo").html(result);

            ClearRoomDetails();
            $("#RoomInfoDialog").dialog({
                width: 1100,
                height: 600,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                close: RoomDialogClose,
                fluid: true,
                title: "Room Info",
                show: 'slide'
            });
            //$("#myTabsPopUp").tabs({ active: 0 });
        }
        function OnLoadRoomInfoFailed() { }

        function RoomDialogClose() {

            if ($("#chkChangeRoom").is(":checked") == true) {
                if ($("#txtAlterRoomNumber").val() == "") {
                    $("#chkChangeRoom").prop("checked", false);
                    $(".roomChangeContainer").hide();
                    $("#chkMakeOrder").prop("checked", true);

                    if ($("#roomInfoContainer").hasClass('col-sm-6')) {
                        $("#roomInfoContainer").removeClass("col-sm-6");
                        $("#roomInfoContainer").addClass("col-sm-12");
                    }
                    else if ($("#roomInfoContainer").hasClass('col-sm-12')) {
                        $("#roomInfoContainer").removeClass("col-sm-12");
                        $("#roomInfoContainer").addClass("col-sm-6");
                    }
                }
            }
        }

        function OnLoadOutOfOrderPossiblePathSucceeded(result) {

            if (result.IsSuccess == true) {
                $('#serviceDeciderHtml').html(result.DataStr);

                $("#serviceDecider").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 600,
                    height: 200,
                    closeOnEscape: true,
                    resizable: false,
                    title: "Occupied Table Possible Path",
                    show: 'slide'
                });
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
                setTimeout(function () { window.location = "frmCostCenterSelectionForAll.aspx"; }, 2000);
            }

        }

        function OnShowOutOfServiceRoomInformationFailed() { }

        function OpenTableCleanPanel(costCenterId, tableId) {
            debugger;
            $("#TableShiftInfo").hide("slow");
            $("#TableCleanInfo").show("slow");
            var strTable = "";

            strTable += "<div style='float:left;padding-left:28px; padding-bottom:5px; margin-top:10px; margin-bottom:10px;'> <fieldset> <legend>Table Clean Info</legend>";

            strTable += "<input type='button' style='width:150px' value='" + "Clean Table" + "' class='TransactionalButton btn btn-primary'";
            strTable += " onclick=\"return UpdateTableStatus('" + costCenterId + "', '" + tableId + "' );\"  />";
            strTable += "</div>";
            strTable += "</fieldset> </div> ";

            document.getElementById("TableCleanInfo").innerHTML=(strTable);

            
        }

        function OpenTableShiptPanel(costCenterId, tableId) {

            var strTable = "";


            document.getElementById("TableCleanInfo").innerHTML=(strTable);

            $("#TableCleanInfo").hide("slow");
            $("#TableShiftInfo").show("slow");
        }

        function UpdateTableStatus(costCenterId, tableId) {
            //debugger;
            var answer = confirm("Do you want to Table Clean?")
            if (answer) {
                PageMethods.UpdateRestaurantTableStatus(costCenterId, tableId, 1, OnUpdateTableStatusSucceeded, OnUpdateTableStatusFailed);
            }
            return false;
        }

        function OnUpdateTableStatusSucceeded(result) {
            if (result.IsSuccess == true) {
                CommonHelper.AlertMessage(result.AlertMessage);

                if ($('#TableInfoDialog').is(':visible')) {
                    $("#serviceDecider").dialog("close");
                }
                if ($('#TableInfoDialog').is(':visible')) {
                    $("#TableInfoDialog").dialog("close");
                }

                LoadTableInfo(result.Pk);
            }
        }

        function OnUpdateTableStatusFailed() { }

        function UpdateTableShift(costCenterId, tableId) {
            var unassignedTable = $("#ddlAvailableTable").val();

            PageMethods.UpdateRestaurantTableShift(costCenterId, tableId, unassignedTable, OnUpdateTableShiftSucceeded, OnUpdateTableShiftFailed);
            return false;
        }

        function OnUpdateTableShiftSucceeded(result) {
            if (result.IsSuccess == true) {
                CommonHelper.AlertMessage(result.AlertMessage);

                if ($('#serviceDecider').is(':visible') == true) {
                    $("#serviceDecider").dialog("close");
                }
                if ($('#TableInfoDialog').is(':visible') == true) {
                    $("#TableInfoDialog").dialog("close");
                }

                LoadTableInfo(result.Pk);
            }
        }
        function OnUpdateTableShiftFailed() { }

        function BillReprint() {

            $("#TouchKeypad").dialog("close");

            if ($("#ContentPlaceHolder1_txtBillId").val() == "") {
                toastr.warning("Please Provide Bill Id.");
                return;
            }

            if ($("#ContentPlaceHolder1_hfIsCostCenterWiseBillNumberGenerate").val() == "1") {
                if ($("#ContentPlaceHolder1_hfCostcenterIdForBillReprint").val() == "0") {
                    toastr.warning("Please Select Cost-Center From Left Side.");
                    return;
                }
            }

            var billPrefix = $("#ContentPlaceHolder1_hfBillPrefixCostcentrwise").val();
            var billNo = $("#ContentPlaceHolder1_txtBillId").val();
            var billNumber = billPrefix + CommonHelper.padLeft(billNo, 8, '0');

            var iframeid = 'printDoc';
            var url = "/POS/Reports/frmReportTPBillInfo.aspx?billID=" + billNumber + "&RePrint=1";
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 450,
                height: 'auto',
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                close: ClosePrintPreviewDialog,
                title: "Invoice Preview",
                show: 'slide'
            });

            return false;
        }

        function PrintDocumentFunc(printTemplate) {
            if (printTemplate == "1") {
                $('#btnPrintPreview').trigger('click');
            }
            else if (printTemplate == "2") {
                //$('#btnPrintReportTemplate2').trigger('click');
            }
            else if (printTemplate == "3") {
                //$("#btnPrintReportTemplate3").trigger('click');
            }
            return true;
        }

        function ClosePrintDialog() { }

        function KotPrintPreview(kotId, sourceId, sourceType) {

            var iframeid = 'printDoc';
            var url = "/POS/Reports/frmTReportKotBillInfo.aspx?st=" + sourceType + "&kotId=" + kotId + "&kotIdLst=" + kotId + "&tno=" + sourceId + "&kbm=" + "kotb" + "&isrp=" + "rp";
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 'auto',
                height: 'auto',
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                close: ClosePrintPreviewDialog,
                title: "Invoice Preview",
                show: 'slide'
            });

            $("#serviceDecider").dialog("close");
        }

        function ClosePrintPreviewDialog() {
            $("#displayBill").dialog('close');

            var iframe = document.getElementById("printDoc");
            var html = "";

            iframe.contentWindow.document.open();
            iframe.contentWindow.document.write(html);
            iframe.contentWindow.document.close();
        }

        function SearchRoomInfo() {

            if ($("#chkChangeRoom").is(":checked") == false && $("#chkMakeOrder").is(":checked") == false) {
                toastr.warning("Please Select Make Order Or Change Room Option.");
                return false;
            }

            var roomNumber = $.trim($("#txtRoomNumber").val());
            var newRoomNumber = $.trim($("#txtAlterRoomNumber").val());
            var costcenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();

            if ($("#chkMakeOrder").is(":checked") == true) {
                if (roomNumber == "") { toastr.warning("Please Give Room Number."); return false; }
            }

            if ($("#chkChangeRoom").is(":checked") == true) {
                if (roomNumber == "") {
                    toastr.warning("Please Give From Room Number."); return false;
                }
                else if (newRoomNumber == "") {
                    toastr.warning("Please Give To Room Number For Change."); return false;
                }
                else if (newRoomNumber == roomNumber) {
                    toastr.warning("Same Room Cannot Be Changed.");
                    return false;
                }
            }

            CommonHelper.SpinnerOpen();

            if ($("#chkMakeOrder").is(":checked") == true) {
                PageMethods.GetRegistrationInformationForSingleGuestByRoomNumber(costcenterId, roomNumber, OnGetRegistrationInformationForSingleGuestByRoomNumberSucceeded, OnGetRegistrationInformationForSingleGuestByRoomNumberFailed);
            }
            else if ($("#chkChangeRoom").is(":checked") == true) {
                PageMethods.GetRegistrationInformationForRoomChangeByRoomNumber(costcenterId, roomNumber, newRoomNumber, OnGetRegistrationInformationForChangeByRoomNumberSucceeded, OnGetRegistrationInformationForSingleGuestByRoomNumberFailed);
            }

            return false;
        }

        function SearchRoomInfoFromList(roomNumner) {
            $("#txtRoomNumber").val(roomNumner);
            SearchRoomInfo();
        }

        function OnGetRegistrationInformationForSingleGuestByRoomNumberSucceeded(result) {
            if (result.RoomId != 0) {
                $(".roomIsRegistered").show();
            }
            else {
                $(".roomIsRegistered").hide();
                toastr.warning("Please Provide Valid Room Number.");
            }

            ClearRoomDetails();

            $("#ContentPlaceHolder1_hfCostcenterId").val(result.CostCenterId);
            $("#ContentPlaceHolder1_hfRoomId").val(result.RoomId);
            $("#ContentPlaceHolder1_hfKotId").val(result.KotId);
            $("#ContentPlaceHolder1_hfBillId").val(result.BillId);

            if (result.KotId != 0) {
                $("#RoomRestaurantOrder").css("background", "#D63CC4");
                $("#kotCleanForRoom").show();
            }

            else {
                $("#RoomRestaurantOrder").css("background", "");
                $("#kotCleanForRoom").hide();
            }

            $("#RoomRestaurantOrder").show();

            //if (result.IsStopChargePosting == true) {
            //    $("#RoomRestaurantOrder").hide();
            //}
            //else {
            //    $("#RoomRestaurantOrder").show();
            //}

            if ($("#roomInfoContainer").hasClass('col-sm-6')) {
                $("#roomInfoContainer").removeClass("col-sm-6");
                $("#roomInfoContainer").addClass("col-sm-12");
            }
            //else if ($("#roomInfoContainer").hasClass('col-sm-12')) {
            //    $("#roomInfoContainer").removeClass("col-sm-12");
            //    $("#roomInfoContainer").addClass("col-sm-6");
            //}

            $("#<%=lblRoomNumber.ClientID %>").text(result.RoomNumber);
            $("#<%=lblRoomType.ClientID %>").text(result.RoomType);

            $("#<%=lblDGuestName.ClientID %>").text(result.GuestName);
            $("#<%=lblDGuestSex.ClientID %>").text(result.GuestSex);
            $("#<%=lblDGuestEmail.ClientID %>").text(result.GuestEmail);
            $("#<%=lblDGuestPhone.ClientID %>").text(result.GuestPhone);
            $("#<%=lblDGuestAddress2.ClientID %>").text(result.GuestAddress2);
            $("#<%=lblDGuestNationality.ClientID %>").text(result.GuestNationality);
            $("#<%=lblDCountryName.ClientID %>").text(result.CountryName);
            <%--$("#<%=lblDRemarks.ClientID %>").text(result.Remarks);--%>
            $("#<%=lblDPOSRemarks.ClientID %>").text(result.POSRemarks);

            $("#myTabsPopUp").tabs({ active: 0 });
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnGetRegistrationInformationForSingleGuestByRoomNumberFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function OnGetRegistrationInformationForChangeByRoomNumberSucceeded(result) {

            if (result[0].RoomId != 0) {
                $(".roomIsRegistered").show();
            }
            else {
                $(".roomIsRegistered").hide();
                //$("#chkMakeOrder").prop("checked", true);                
                toastr.warning("Please provide valid room for change.");
                ClearRoomDetails();
                $("#chkChangeRoom").prop("checked", true);
                CommonHelper.SpinnerClose();
                return false;
            }

            ClearRoomDetails();

            $("#ContentPlaceHolder1_hfCostcenterId").val(result[0].CostCenterId);
            $("#ContentPlaceHolder1_hfRoomId").val(result[0].RoomId);
            $("#ContentPlaceHolder1_hfKotId").val(result[0].KotId);
            $("#ContentPlaceHolder1_hfBillId").val(result[0].BillId);

            if (result[0].KotId != 0) {
                $("#RoomRestaurantOrder").css("background", "#D63CC4");
                $(".roomChangeContainer").show();
            }

            else {
                $("#RoomRestaurantOrder").css("background", "");
                $(".roomChangeContainer").hide();
            }

            if ($("#roomInfoContainer").hasClass('col-sm-12')) {
                $("#roomInfoContainer").removeClass("col-sm-12");
                $("#roomInfoContainer").addClass("col-sm-6");
            }

            $("#<%=lblRoomNumber.ClientID %>").text(result[0].RoomNumber);
            $("#<%=lblRoomType.ClientID %>").text(result[0].RoomType);

            $("#<%=lblDGuestName.ClientID %>").text(result[0].GuestName);
            $("#<%=lblDGuestSex.ClientID %>").text(result[0].GuestSex);
            $("#<%=lblDGuestEmail.ClientID %>").text(result[0].GuestEmail);
            $("#<%=lblDGuestPhone.ClientID %>").text(result[0].GuestPhone);
            $("#<%=lblDGuestAddress2.ClientID %>").text(result[0].GuestAddress2);
            $("#<%=lblDGuestNationality.ClientID %>").text(result[0].GuestNationality);
            $("#<%=lblDCountryName.ClientID %>").text(result[0].CountryName);
            <%--$("#<%=lblDRemarks.ClientID %>").text(result[0].Remarks);--%>
            $("#<%=lblDPOSRemarks.ClientID %>").text(result[0].POSRemarks);

            $("#RoomRestaurantOrder").show();
            $("#btnRoomChangeInfo").hide();

            if (result.length > 1) {
                if (result[0].IsStopChargePosting == true && result[1].IsStopChargePosting == true) {
                    $("#RoomRestaurantOrder").hide();
                    $("#btnRoomChangeInfo").hide();
                }
                else {
                    $("#RoomRestaurantOrder").show();
                    $("#btnRoomChangeInfo").show();
                }
            }

            //----------------------

            if (result.length > 1) {

                $("#ContentPlaceHolder1_hfChangeRoomId").val(result[1].RoomId);

                $("#<%=lblRoomNumberToChange.ClientID %>").text(result[1].RoomNumber);
                $("#<%=lblRoomTypeToChange.ClientID %>").text(result[1].RoomType);

                $("#<%=lblDGuestNameToChange.ClientID %>").text(result[1].GuestName);
                $("#<%=lblDGuestSexToChange.ClientID %>").text(result[1].GuestSex);
                $("#<%=lblDGuestEmailToChange.ClientID %>").text(result[1].GuestEmail);
                $("#<%=lblDGuestPhoneToChange.ClientID %>").text(result[1].GuestPhone);
                $("#<%=lblDGuestAddress2ToChange.ClientID %>").text(result[1].GuestAddress2);
                $("#<%=lblDGuestNationalityToChange.ClientID %>").text(result[1].GuestNationality);
                $("#<%=lblDCountryNameToChange.ClientID %>").text(result[1].CountryName);

                $("#RoomRestaurantOrder").show();
                $("#btnRoomChangeInfo").hide();

                //if (result[0].IsStopChargePosting == true && result[1].IsStopChargePosting == true) {
                //    $("#RoomRestaurantOrder").show();
                //    $("#btnRoomChangeInfo").show();
                //}
                //else {
                //    $("#RoomRestaurantOrder").hide();
                //    $("#btnRoomChangeInfo").hide();
                //}
            }
            else {
                //$("#chkMakeOrder").prop("checked", true);
                $("#chkChangeRoom").prop("checked", true);
                toastr.warning("Please provide valid room for change.");
            }

            CommonHelper.SpinnerClose();

            return false;
        }

        function ChangeRoomInfo() {

            var newRoomNumber = $.trim($("#txtAlterRoomNumber").val());
            if (newRoomNumber == "") { toastr.warning("Please Give Room Number To Change."); return false; }

            var costcenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();
            var oldRoomId = $("#ContentPlaceHolder1_hfRoomId").val();
            var oldRoomNumber = $.trim($("#ContentPlaceHolder1_lblRoomNumber").text());
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();

            if (newRoomNumber == oldRoomNumber) {
                toastr.warning("Same Room Cannot Be Changed.");
                return false;
            }

            CommonHelper.SpinnerOpen();

            PageMethods.RoomNumberShift(costcenterId, oldRoomId, oldRoomNumber, kotId, newRoomNumber, OnRoomNumberShiftSucceeded, OnRoomNumberShiftFailed);
            return false;
        }

        function OnRoomNumberShiftSucceeded(result) {
            if (result.IsSuccess == true) {
                CommonHelper.AlertMessage(result.AlertMessage);

                $("#ContentPlaceHolder1_hfRoomId").val($("#ContentPlaceHolder1_hfChangeRoomId").val());

                ResumeBill("rom", 0, 0, 0);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
        }

        function OnRoomNumberShiftFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function ClearRoomKot() {
            var costCenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();
            var roomId = $("#ContentPlaceHolder1_hfRoomId").val();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var billId = $("#ContentPlaceHolder1_hfBillId").val();

            if (billId != "0") {
                toastr.info("Bill Already Generated. Cannot Clean the Bill. Please Settled and Void The Bill.");
                return false;
            }

            PageMethods.ClearRoomKot(costCenterId, kotId, roomId, OnClearRoomKotSucceeded, OnClearRoomKotFailed);
        }
        function OnClearRoomKotSucceeded(result) {

            if (result.IsSuccess == true) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ClearRoomDetails();
                PageMethods.LoadRoomAllocation(OnLoadRoomInfoSucceeded, OnLoadRoomInfoFailed);
                SearchRoomInfo();
            }

        }

        function LoadKitchenInfo(KitchenId, Kitchen) {
            //var pageNumber = 1;
            //var IsCurrentOrPreviousPage = 1;
            //var gridRecordsCount = KotInformationTable.data().length;
            //$("#ContentPlaceHolder1_hfRestaurantKitchen").val(Kitchen);

            //$("#ContentPlaceHolder1_hfRestaurantKitchenId").val(KitchenId);
            //PageMethods.LoadKitchenInfo(KitchenId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadKitchenInfoSucceeded, OnLoadKitchenInfoFailed);

            window.location = "/POS/frmKitchenInformationDetails.aspx?kid=" + KitchenId + "&k=" + Kitchen;
            return false;
        }

        function OnClearRoomKotFailed() { }

        function ClearRoomDetails() {

            $("#<%=lblDGuestName.ClientID %>").text("");
            $("#<%=lblDGuestSex.ClientID %>").text("");
            $("#<%=lblDGuestEmail.ClientID %>").text("");
            $("#<%=lblDGuestPhone.ClientID %>").text("");
            $("#<%=lblDGuestAddress2.ClientID %>").text("");
            $("#<%=lblDGuestNationality.ClientID %>").text("");
            $("#ContentPlaceHolder1_lblDCountryName").text("");
            <%--$("#<%=lblDRemarks.ClientID %>").text("");--%>
            $("#<%=lblDPOSRemarks.ClientID %>").text("");


            $("#txtRoomNumber").text("");
            $("#ContentPlaceHolder1_lblRoomNumber").text("");
            $("#ContentPlaceHolder1_lblRoomType").text("");
        }

        function CostcenterSelectionForReprint(control, costcentrId, billNumberPrefix) {

            $("#costcentertableforreprint tbody tr td").removeClass('progress-bar-info');
            $("#costcentertableforreprint tbody tr td").addClass('progress-bar-success');

            var td = $(control).parent();
            $(td).removeClass('progress-bar-success');
            $(td).addClass('progress-bar-info');

            $("#ContentPlaceHolder1_hfCostcenterIdForBillReprint").val(costcentrId);
            $("#ContentPlaceHolder1_hfBillPrefixCostcentrwise").val(billNumberPrefix);
        }

        var p = null;

    </script>

    <asp:HiddenField ID="hfBillId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfOrderType" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfOrderCostcenterId" runat="server"></asp:HiddenField>

    <asp:HiddenField ID="hfCostcenterId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfRoomId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfChangeRoomId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfKotId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="IsRestaurantPaxConfirmationEnable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="IsRestaurantWaiterConfirmationEnable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCostCenter" runat="server" Value=""></asp:HiddenField>
    
    <asp:HiddenField ID="hfRestaurantKOTId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfItemName" runat="server" Value=""></asp:HiddenField>

    <asp:HiddenField ID="hfCostcenterIdForBillReprint" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillPrefixCostcentrwise" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfIsCostCenterWiseBillNumberGenerate" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsBearar" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBillOrderTakingWaiterId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfIsChef" runat="server" Value=""></asp:HiddenField>

    <asp:HiddenField ID="hfUserInfoObj" runat="server" Value="" ClientIDMode="Static" />

    <div id="displayBill" style="display: none;">
        <div class="row no-gutters">
            <div class="col-sm-12">
                <div class="well well-sm col-sm-12">
                    <div class="row pull-right">
                        <div class="col-sm-12">
                            <input type="button" class="btn btn-primary btn-large" value="Close" onclick="ClosePrintPreviewDialog()" />
                        </div>
                    </div>
                </div>

                <iframe id="printDoc" name="printDoc" frameborder="0" style="overflow: hidden; width: 410px; height: 550px"></iframe>
                <div id="bottomPrint">
                </div>
            </div>
        </div>
    </div>

    <div id="serviceDecider" style="display: none;">
        <div class="form-horizontal">
            <div class="row no-gutters">
                <div class="col-sm-12" id="serviceDeciderHtml">
                </div>
            </div>
        </div>
    </div>

    <div class="row no-gutters" id="CostCenterListDiv" runat="server">
        <div class="col-sm-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Cost Center Information
                </div>
                <div class="panel-body">
                    <asp:Literal ID="literalCostCenterInformation" runat="server"> </asp:Literal>
                </div>
            </div>
        </div>
    </div>
    <div class="row no-gutters" id="KitchenListDiv" runat="server">
        <div class="col-sm-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Kitchen Information
                </div>
                <div class="panel-body">
                    <asp:Literal ID="literalKitchenInformation" runat="server"> </asp:Literal>
                </div>
            </div>
        </div>
    </div>

    <div class="row no-gutters" id="TokenListDiv" runat="server">
        <div class="col-sm-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Token List
                </div>
                <div class="panel-body">
                    <input type="button" id="btnTokenList" value="Token List" style="margin-top: 5px;"
                        class="btn btn-primary btn-large" />
                    <input type="button" id="btnBillReprint" value="Bill Re-Print" style="margin-top: 5px;"
                        class="btn btn-primary btn-large" />
                </div>
            </div>
        </div>
    </div>
    <div id="TokenListDialog" style="display: none;">
        <div style="height: 550px;">
            <div style="height: 550px; overflow-y: scroll;">
                <div id="TokenListContainer">
                </div>
            </div>
        </div>
    </div>

    <div id="TableInfoDialogCashier" style="display: none;">
        <div id="TableContainerCahsierWise">
            <div id="myTabs">
                <ul id="tabPage" class="ui-style">
                    <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                        href="#tab-1">Table Info</a></li>
                    <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                        href="#tab-2">Waiter Wise Occupied Table</a></li>
                </ul>
                <div id="tab-1">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-sm-2">
                                <div style="height: 550px; overflow-y: scroll; width: 160px">
                                    <div class="alert-info" style="padding: 5px; font-weight: bold;">Pending Kot</div>
                                    <div id="OccupiedTableListForCashier">
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-10">
                                <div style="height: 550px;">
                                    <div style="height: 550px; overflow-y: scroll;">
                                        <div id="TableContainerForCashier">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="tab-2">
                    <div style="height: 550px;">
                        <div style="height: 550px; overflow-y: scroll;">
                            <div id="OccupiedTableInfoByWaiter">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="TableInfoDialog" style="display: none;">
        <div class="form-horizontal">
            <div class="row">
                <div class="col-sm-2">
                    <div style="height: 550px; overflow-y: scroll;">
                        <div class="alert-info" style="padding: 5px; font-weight: bold;">Pending Kot</div>
                        <div id="OccupiedTableList">
                        </div>
                    </div>
                </div>
                <div class="col-sm-10">
                    <div style="height: 550px;">
                        <div style="height: 550px; overflow-y: scroll;">
                            <div id="TableContainer">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    
    <div id="RoomInfoDialog" style="display: none;">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-2">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <input type="checkbox" id="chkMakeOrder" checked="checked" />
                        </span>
                        <label class="form-control">Make Order</label>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <input type="checkbox" id="chkChangeRoom" />
                        </span>
                        <label class="form-control">Change Room</label>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <label class="control-label" style="padding-top: 0px;">Room Number</label></span></span>
                        <input type="text" id="txtRoomNumber" class="form-control" />
                    </div>
                </div>
                <div class="col-sm-3 roomChangeContainer">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <label class="control-label" style="padding-top: 0px;">To Room</label></span>
                        <input type="text" id="txtAlterRoomNumber" class="form-control" />
                    </div>
                </div>
                <div class="col-sm-2">
                    <input type="button" value="Search" class="btn btn-primary" onclick="SearchRoomInfo()" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-2">
                <div style="height: 450px; overflow-y: scroll;">
                    <div id="RoomListInfo">
                    </div>
                </div>
            </div>
            <div class="col-sm-10">
                <div id="myTabsPopUp" style="padding: 2px; margin-bottom: 10px;" class="ui-style">
                    <ul id="tabPagePopUp" class="ui-style">
                        <li id="Li1" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                            <a href="#tabGuestInfo">Guest Info</a></li>
                        <li id="Li2" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                            <a href="#tabSpecialInst">Special Instruction</a></li>
                        <%--<li id="Li3" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                            <a href="#tabPreferance">Guest Preferance</a></li>--%>
                    </ul>
                    <div id="tabGuestInfo">
                        <div class="form-horizontal">
                            <div class="form-group roomIsRegistered" style="display: none; margin: 2px;">
                                <div class="col-sm-6" id="roomInfoContainer">
                                    <table class="table table-striped table-bordered table-condensed table-hover">
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label1" runat="server" Text="Room Number"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblRoomNumber" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label2" runat="server" Text="Room Type"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblRoomType" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="lblLArrivalDate" runat="server" Text="Arrival Date"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDArrivalDate"
                                                    runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="lblLExpectedDepartureDate"
                                                    runat="server" Text="Expected Departure Date"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDExpectedDepartureDate" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label
                                                    ID="lblLGuestName" runat="server" Text="Name"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestName" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="lblLCountryName" runat="server"
                                                    Text="Country Name"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDCountryName"
                                                    runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="lblLGuestSex" runat="server"
                                                    Text="Gender"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestSex"
                                                    runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="lblLGuestDOB" runat="server" Text="Date of Birth"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestDOB" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="lblLGuestEmail"
                                                    runat="server" Text="Email"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestEmail"
                                                    runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label
                                                    ID="lblLGuestPhone" runat="server" Text="Phone Number"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestPhone" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="lblLGuestAddress2" runat="server"
                                                    Text="Address"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestAddress2"
                                                    runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="lblLGuestNationality" runat="server" Text="Nationality"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestNationality" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-sm-6 roomChangeContainer" id="roomChangeContainerTable">
                                    <table class="table table-striped table-bordered table-condensed table-hover">
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label3" runat="server" Text="Room Number"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblRoomNumberToChange" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label4" runat="server" Text="Room Type"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblRoomTypeToChange" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label5" runat="server" Text="Arrival Date"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDArrivalDateToChange"
                                                    runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label6"
                                                    runat="server" Text="Expected Departure Date"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDExpectedDepartureDateToChange" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label
                                                    ID="Label7" runat="server" Text="Name"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestNameToChange" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label8" runat="server"
                                                    Text="Country Name"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDCountryNameToChange"
                                                    runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label9" runat="server"
                                                    Text="Gender"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestSexToChange"
                                                    runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label10" runat="server" Text="Date of Birth"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestDOBToChange" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label11"
                                                    runat="server" Text="Email"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestEmailToChange"
                                                    runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label
                                                    ID="Label12" runat="server" Text="Phone Number"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestPhoneToChange" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label13" runat="server"
                                                    Text="Address"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestAddress2ToChange"
                                                    runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="col-sm-2">
                                                <asp:Label ID="Label14" runat="server" Text="Nationality"></asp:Label>
                                            </td>
                                            <td class="col-sm-4">
                                                <asp:Label ID="lblDGuestNationalityToChange" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="tabSpecialInst">
                        <div class="form-horizontal">
                            <div class="form-group roomIsRegistered" style="display: none; margin: 2px;">
                                <div class="col-sm-12" id="roomInfoContainer2">
                                    <table class="">
                                        <tr>
                                            <td class="col-sm-12">
                                                <%--<asp:Label ID="lblDRemarks"
                                                    runat="server" Text=""></asp:Label>
                                                --%><asp:Label ID="lblDPOSRemarks"
                                                    runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>

                    </div>
                    <%--<div id="tabPreferance">

                    </div>--%>
                </div>

                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-sm-2 roomIsRegistered" style="display: none;">
                            <input type="button" id="RoomRestaurantOrder" value="Restaurant Order" class="btn btn-primary" onclick="ResumeBill('rom', 0, 0, 0)" />
                        </div>
                        <div class="col-sm-3 roomChangeContainer" style="display: none;">
                            <input type="button" id="btnRoomChangeInfo" value="Change Room & Continue To Bill" class="btn btn-primary" onclick="ChangeRoomInfo()" />
                        </div>
                        <div class="col-sm-2" id="kotCleanForRoom" style="display: none;">
                            <input type="button" value="Clean Order" class="TransactionalButton btn btn-primary" onclick="ClearRoomKot();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="TouchKeypad" style="display: none;">
        <div class="form-horizontal">
            <div class="row no-gutters">
                <div class="col-sm-5" id="BillReprintCostcenterPanel">
                    <asp:Literal ID="literalCostCenterForReprint" runat="server"> </asp:Literal>
                </div>
                <div class="col-sm-7" id="BillNoKeyContainer">
                    <div id="TouchKeypadResultDiv">
                        <asp:TextBox ID="txtBillId" runat="server" Style="text-align: right;" CssClass="numkbnotdecimal form-control" Height="40px"
                            Font-Size="35px"></asp:TextBox>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <div id="KeyBoardContainer">
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group pull-right" style="padding-right: 14px;">
                <div class="col-sm-12">
                    <input type="button" class="ui-keyboard-button ui-keyboard-48 ui-state-default ui-corner-all"
                        style="width: 90px; height: 50px; font-size: 1.5em;" value="OK" onclick='BillReprint()' />
                </div>
            </div>
        </div>
    </div>


    <div id="PaxAndWaiterDialog" style="display: none;">

        <div class="form-horizontal">
            <div class="panel panel-default">
                <div class="row" style="height: 292px;">
                    <div class="col-md-6" id="WaiterChangeDialog">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:TextBox ID="txtWaiter" runat="server" CssClass="form-control"
                                    Height="30px" Font-Size="15px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6" id="PaxConfirmationDivPanel">
                        <div class="form-group">
                            <div class="col-sm-12">
                                <asp:HiddenField ID="hfPaxConfirmationQuantity" runat="server" />
                                <asp:HiddenField ID="hfRouteDetails" runat="server" Value="" />
                                <asp:TextBox ID="txtConfirmPax" TabIndex="1" CssClass="numkbnotdecimalpax form-control" Style="text-align: right;" Height="40px"
                                    Font-Size="35px" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <div id="KeyBoardContainerQuantityChange">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="panel panel-footer" style="margin-top: 10px;">
                    <div class="row" style="margin-top: 5px;">
                        <div class="col-md-3"></div>
                        <div class="col-md-6 text-center">
                            <input type="button" id="btnPaxConfirmationOk" onclick="PaxConfirmationOk()" class="ui-keyboard-button ui-keyboard-48 ui-state-default ui-corner-all"
                                style="width: 100px; height: 50px; font-size: 1.5em;" value="Ok" />
                            <input type="button" id="btnPaxConfirmationCancel" class="ui-keyboard-button ui-keyboard-48 ui-state-default ui-corner-all"
                                style="width: 100px; height: 50px; font-size: 1.5em;"
                                value="Cancel" onclick="PaxConfirmationDialogClose()" />
                        </div>
                        <div class="col-md-3"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>

