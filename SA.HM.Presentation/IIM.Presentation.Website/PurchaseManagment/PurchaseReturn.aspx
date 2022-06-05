<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="PurchaseReturn.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.PurchaseReturn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var queryOutId = "";
        var AddedSerialCount = 0;

        var ItemSelected = null;
        var NewAddedSerial = new Array();
        var AddedSerialzableProduct = new Array();
        var DeletedSerialzableProduct = new Array();
        var ReturnItemDeleted = new Array();
        var IsValidSerial = false;
        var canReturn = false;
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Purchase Return</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if (Cookies.getJSON('receivereturn') != undefined) {

                queryOutId = CommonHelper.GetParameterByName("rid");
                var receivedId = CommonHelper.GetParameterByName("tid");
                var returnId = CommonHelper.GetParameterByName("rid");

                PageMethods.EditItemReturn(returnId, receivedId, OnEditPurchaseOrderSucceed, OnEditPurchaseOrderFailed);
            }

            $("#myTabs").tabs();

            $('#ContentPlaceHolder1_txtReceiveDateFrom').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtReceiveDateTo').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_ddlSupplier").select2();

        });

        function SearchReceiveOrder() {

            AddedSerialCount = 0;
            ItemSelected = null;
            NewAddedSerial = new Array();
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();
            ReturnItemDeleted = new Array();

            var receiveNumber = "0", supplierId = "0", fromDate = null, toDate = null;

            receiveNumber = $("#ContentPlaceHolder1_txtReferenceNumber").val();
            fromDate = $("#ContentPlaceHolder1_txtReceiveDateFrom").val();
            toDate = $("#ContentPlaceHolder1_txtReceiveDateTo").val();
            supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            if (supplierId == "0")
                supplierId = null;

            $("#ReceiveSummaryGrid tbody").html("");
            $("#ReceiveItemGrid tbody").html("");

            PageMethods.GetProductReceiveDetailsByReceiveOrSupplier(fromDate, toDate, receiveNumber, supplierId, OnSearchReceiveOrderSucceed, OnSearchReceiveOrderFailed);

            return false;
        }
        function OnSearchReceiveOrderSucceed(result) {
            var tr = "";

            if (result.length == 0) {
                tr += "<tr><td colspan='8'>No Record Found</td></tr>";
                $("#ReceiveSummaryGrid tbody").append(tr);
                return false;
            }

            $.each(result, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.ReceiveNumber + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.ReceiveType + "</td>";

                if (gridObject.ReceivedDate != null)
                    tr += "<td style='width:8%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.ReceivedDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:8%;'></td>";

                tr += "<td style='width:15%;'>" + gridObject.CostCenter + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.LocationName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.SupplierName + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:22%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width22%;'></td>";

                tr += "<td style=\"text-align: center; width:5%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/receiveitem.png' onClick= \"javascript:return ReceiveItemOut(this, " + gridObject.ReceivedId + ",'" + gridObject.ReceiveType + "'," + gridObject.POrderId + "," + gridObject.CostCenterId + "," + gridObject.LocationId + ")\" alt='Receive'  title='Receive' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.ReceivedId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.SupplierId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.CostCenterId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.LocationId + "</td>";

                tr += "</tr>";

                $("#ReceiveSummaryGrid tbody").append(tr);
                tr = "";
            });

            return false;
        }
        function OnSearchReceiveOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }

        function ReceiveItemOut(control, receivedId, receiveType, porderId, costCenterId, locationId) {

            $("#ContentPlaceHolder1_hfReceivedId").val(receivedId);
            $("#ContentPlaceHolder1_hfPOrderId").val(porderId);
            $("#ContentPlaceHolder1_hfCostCenterId").val(costCenterId);
            $("#ContentPlaceHolder1_hfLocationId").val(locationId);

            AddedSerialCount = 0;
            ItemSelected = null;
            NewAddedSerial = new Array();
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();
            ReturnItemDeleted = new Array();

            $("#ReceiveSummaryGrid tbody tr").removeAttr("style");

            var tr = $(control).parent().parent();
            $(tr).css("background-color", "#e6ffe6");

            $("#ReceiveItemGrid tbody").html("");

            PageMethods.SearcReceiveOrderItemForReturn(receivedId, receiveType, porderId, locationId, OnLoadReceiveItemSucceed, OnLoadReceiveItemFailed);
            return false;
        }
        function OnLoadReceiveItemSucceed(result) {

            if (result.ProductReceivedDetails.length == 0) {
                toastr.info("Item Is Already Used By Another Return Order. Please Search and Check the Report.");

                tr += "<tr><td colspan='6'>No Record Found</td></tr>";
                $("#ReceiveItemGrid tbody").append(tr);
                return false;
            }

            var tr = "";

            $.each(result.ProductReceivedDetails, function (count, gridObject) {

                tr += "<tr>";

                if (gridObject.Quantity > 0) { //ReturnQuantity
                    tr += "<td style='width:5%;text-align: center;'>" +
                        "<input type='checkbox' checked='checked' id='chk" + gridObject.ItemId + "'" + " />" +
                        "</td>";
                }
                else {
                    tr += "<td style='width:5%;'></td>";
                }

                tr += "<td style='width:35%;'>" + gridObject.ItemName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.StockBy + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.ReturnQuantity + "</td>";

                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + gridObject.ReturnQuantity + "' id='pp" + gridObject.ItemId + "' class='form-control quantitydecimal' onblur='CheckTotalQuantity(this)' />" +
                    "</td>";

                tr += "<td style='width:15%;'>";
                if (gridObject.ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForReceiveItemReturn(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.ItemId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.StockById + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ReceivedId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ReceiveDetailsId + "</td>";
                tr += "<td style='display:none;'>0</td>";
                tr += "<td style='display:none;'>0</td>";
                tr += "<td style='display:none;'>" + gridObject.Quantity + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ProductType + "</td>";

                tr += "<td style='display:none;'>" + gridObject.ColorId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.SizeId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.StyleId + "</td>";

                tr += "</tr>";

                $("#ReceiveItemGrid tbody").append(tr);
                tr = "";
            });

            $("#hfTransactionId").val(result.ProductReceivedDetails[0].ReceivedId);
        }
        function OnLoadReceiveItemFailed() { }

        function AddSerialForReceiveItemReturn(control) {
            var tr = $(control).parent().parent();

            ItemSelected = tr;

            var itemName = $(tr).find("td:eq(1)").text();
            var itemId = $(tr).find("td:eq(6)").text();
            var quantity = $(tr).find("td:eq(4)").find("input").val();

            SearialAddedWindow(itemName, itemId, quantity);
        }
        function SearialAddedWindow(itemName, itemId, quantity) {
            $("#ContentPlaceHolder1_hfItemIdForSerial").val(itemId);
            $("#lblAddedQuantity").text('0');
            $("#lblItemQuantity").text(quantity);

            $("#SerialItemTable tbody tr").remove();
            $("#SerialItemTable tbody").html("");

            if (AddedSerialzableProduct.length > 0) {
                var addedSerial = _.where(AddedSerialzableProduct, { ItemId: parseInt(itemId, 10) });
                var row = 0; rowCount = 0;
                var tr = "";

                if (addedSerial.length > 0) {
                    rowCount = addedSerial.length;
                    $("#lblAddedQuantity").text(rowCount);
                    AddedSerialCount = rowCount;

                    for (row = 0; row < rowCount; row++) {

                        tr += "<tr>";
                        tr += "<td style='width:90%;'>" + addedSerial[row].SerialNumber + "</td>";
                        tr += "<td style='width:10%;'>" +
                            "<a href='javascript:void()' onclick= 'DeleteItemSerial(this)' ><img alt='Delete' src='../Images/delete.png' /></a>" +
                            "</td>";
                        tr += "<td style='display:none;'>" + addedSerial[row].ItemId + "</td>";
                        tr += "<td style='display:none;'>" + addedSerial[row].ReturnSerialId + "</td>";

                        $("#SerialItemTable tbody").append(tr);
                        tr = "";
                    }
                }
            }

            $("#SerialWindow").dialog({
                width: 900,
                height: 650,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                title: "Serial Of Item: " + itemName,
                show: 'slide',
                open: function (event, ui) {
                    $('#SerialWindow').css('overflow', 'hidden');
                    $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                }
            });
        }

        function AddSerialNumber() {
            var itemId = $("#ContentPlaceHolder1_hfItemIdForSerial").val();
            var receivedId = $("#ContentPlaceHolder1_hfReceivedId").val();
            var locationId = $("#ContentPlaceHolder1_hfLocationId").val();
            var serial = $.trim($("#txtSerial").val());
            var addedQuantity = $("#lblAddedQuantity").text();
            var totalQuantity = $("#lblItemQuantity").text();

            if (serial == "") {
                toastr.warning("Please Give Serial.");
                return false;
            }
            else if ((parseInt(addedQuantity, 10) + 1) > parseInt(totalQuantity, 10)) {
                toastr.warning("Number Of Serial Cannot Greater Than Item Quantity.");
                return false;
            }
            CheckSerialAvailability(receivedId, locationId, serial);
            if (IsValidSerial == false) {
                toastr.warning("This Serial Is Not Valid For Return");
                IsValidSerial = false;
                return false;
            }
            else {
                var alreadySaved = _.findWhere(AddedSerialzableProduct, { SerialNumber: serial });
                var newAlreadyAdded = _.findWhere(NewAddedSerial, { SerialNumber: serial });

                if (alreadySaved != null || newAlreadyAdded != null) {
                    toastr.warning("This Serial Already Added.");
                    return false;
                }

                var outId = $("#ContentPlaceHolder1_hfReturnId").val();
                var tr = "";

                tr += "<tr>";
                tr += "<td style='width:90%;'>" + serial + "</td>";
                tr += "<td style='width:10%;'>" +
                    "<a href='javascript:void()' onclick= 'DeleteItemSerial(this)' ><img alt='Delete' src='../Images/delete.png' /></a>" +
                    "</td>";
                tr += "<td style='display:none;'>" + itemId + "</td>";
                tr += "<td style='display:none;'>0</td>";

                $("#SerialItemTable tbody").append(tr);

                NewAddedSerial.push({
                    ReturnSerialId: 0,
                    ItemId: parseInt(itemId, 10),
                    SerialNumber: serial
                });

                AddedSerialCount = AddedSerialCount + 1;
                $("#lblAddedQuantity").text(AddedSerialCount);

                tr = "";
                $("#txtSerial").val("");
                IsValidSerial = false;
            }
            
        }
        function CheckSerialAvailability(receivedId, locationId, serial) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/PurchaseReturn.aspx/CheckSerialAvailability',

                data: "{ 'receivedId':'" + receivedId + "','locationId':'" + locationId + "','serial':'" + serial + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d == true) {
                        IsValidSerial = true;
                    }
                    else {
                        IsValidSerial = false;
                    }
                },
                error: function (result) {
                    IsValidSerial = false;
                }
            });
            return false;
        }
        function ClearSerial() {
            $("#txtSerial").val("");
        }
        function ApplySerialForPurchaseItem() {
            var addedQuantity = $("#lblAddedQuantity").text();
            var totalQuantity = $("#lblItemQuantity").text();

            if (parseInt(addedQuantity, 10) < parseInt(totalQuantity, 10)) {
                toastr.warning("Number Of Serial Must Added As Equall Item Quantity.");
                return false;
            }

            $(ItemSelected).find("td:eq(3)").find("input").val(totalQuantity);

            $(NewAddedSerial).each(function (index, obj) {

                AddedSerialzableProduct.push({
                    ReturnSerialId: obj.ReturnSerialId,
                    ItemId: obj.ItemId,
                    SerialNumber: obj.SerialNumber
                });
            });

            $("#SerialWindow").dialog("close");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("");
            AddedSerialCount = 0;
            NewAddedSerial = new Array();
            ItemSelected = null;
        }
        function DeleteItemSerial(control) {

            var tr = $(control).parent().parent();

            var itemId = parseInt($(tr).find("td:eq(2)").text(), 10);
            var outSerialId = parseInt($(tr).find("td:eq(3)").text(), 10);

            var addedQuantity = $("#lblAddedQuantity").text();
            AddedSerialCount = parseInt(addedQuantity, 10) - 1;

            var item = _.findWhere(AddedSerialzableProduct, { ItemId: itemId });
            var index = _.indexOf(AddedSerialzableProduct, item);
            AddedSerialzableProduct.splice(index, 1);

            var itemNew = _.findWhere(NewAddedSerial, { ItemId: itemId });
            var indexNew = _.indexOf(NewAddedSerial, itemNew);
            NewAddedSerial.splice(indexNew, 1);

            if (outSerialId > 0) {
                DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(item)));
            }

            $("#lblAddedQuantity").text(AddedSerialCount);
            $(tr).remove();
        }
        function CancelAddSerial() {
            $("#SerialWindow").dialog("close");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("");
            AddedSerialCount = 0;
            NewAddedSerial = new Array();
        }

        function SaveItemReceiveOrderReturn() {

            var itemId = 0, colorId = 0, sizeId = 0, styleId = 0, remarks = "";
            var purchaseItem = null;

            var returnId = 0, receivedId = "0", porderId = null, returnDetailsId = 0,
                stockById = "", costCenterId = "", locationId = 0,
                quantity = 0, orderQuantity = 0, receiveDetailsId = 0, productType = '', itemName = '';

            returnId = $("#ContentPlaceHolder1_hfReturnId").val();

            receivedId = $("#ContentPlaceHolder1_hfReceivedId").val();
            porderId = $("#ContentPlaceHolder1_hfPOrderId").val();
            costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            locationId = $("#ContentPlaceHolder1_hfLocationId").val();

            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            checkedBy = 0;
            approvedBy = 0;
            var ReturnItemNewlyAdded = new Array();

            var serialCount = 0, rowSerial = 0, srlItem = null, srlIndex = -1;
            var itemSerial = null;

            $("#ReceiveItemGrid tbody tr").each(function () {

                returnDetailsId = parseInt($(this).find("td:eq(11)").text(), 10);
                receiveDetailsId = parseInt($(this).find("td:eq(9)").text(), 10);
                itemId = parseInt($(this).find("td:eq(6)").text(), 10);
                stockById = parseInt($(this).find("td:eq(7)").text(), 10);
                orderQuantity = $(this).find("td:eq(3)").text();
                quantity = $(this).find("td:eq(4)").find("input").val();
                productType = $.trim($(this).find("td:eq(13)").text());
                itemName = $.trim($(this).find("td:eq(1)").text());


                colorId = parseInt($(this).find("td:eq(14)").text(), 10);
                sizeId = parseInt($(this).find("td:eq(15)").text(), 10);
                styleId = parseInt($(this).find("td:eq(16)").text(), 10);
                

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {

                    
                    ReturnItemNewlyAdded.push({
                        ReturnDetailsId: returnDetailsId,
                        ReturnId: returnId,
                        ReceiveDetailsId: receiveDetailsId,
                        StockById: stockById,
                        ProductType: productType,
                        ItemId: itemId,
                        ColorId: colorId,
                        SizeId: sizeId,
                        StyleId: styleId,
                        ItemName: itemName,
                        OrderQuantity: orderQuantity,
                        Quantity: quantity
                    });
                }
                else if (returnDetailsId > 0) {
                    ReturnItemDeleted.push({
                        ReturnDetailsId: returnDetailsId,
                        ReturnId: returnId,
                        ReceiveDetailsId: receiveDetailsId,
                        StockById: stockById,
                        ProductType: productType,
                        ItemId: itemId,
                        ColorId: colorId,
                        SizeId: sizeId,
                        StyleId: styleId,
                        ItemName: itemName,
                        OrderQuantity: orderQuantity,
                        Quantity: quantity
                    });

                    serialCount = 0, rowSerial = 0;
                    itemSerial = _.where(AddedSerialzableProduct, { ItemId: itemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId });
                    serialCount = itemSerial.length;

                    for (rowSerial = 0; rowSerial < serialCount; rowSerial++) {

                        if (itemSerial[rowSerial].ReturnSerialId > 0)
                            DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(itemSerial[rowSerial])));

                        var srlItem = _.findWhere(AddedSerialzableProduct, { SerialNumber: itemSerial[rowSerial].SerialNumber });
                        var srlIndex = _.indexOf(AddedSerialzableProduct, srlItem);
                        AddedSerialzableProduct.splice(srlIndex, 1);
                    }
                }
            });

            var row = 0, rowCount = ReturnItemNewlyAdded.length;

            for (row = 0; row < rowCount; row++) {
                if (ReturnItemNewlyAdded[row].ProductType == "Serial Product") {
                    var serialTotal = _.where(AddedSerialzableProduct, { ItemId: ReturnItemNewlyAdded[row].ItemId });

                    if (ReturnItemNewlyAdded[row].Quantity > serialTotal.length) {
                        toastr.warning("Please Give Serial Of Product " + ReturnItemNewlyAdded[row].ItemName);
                        break;
                    }
                    else if (serialTotal.length > ReturnItemNewlyAdded[row].Quantity) {
                        toastr.warning("Please Remove Serial Of Product " + ReturnItemNewlyAdded[row].ItemName);
                        break;
                    }
                }
            }
            
            
            if (row != rowCount) {
                return false;
            }

            var ProductReturn = {
                ReturnId: returnId,
                ReceivedId: receivedId,
                POrderId: porderId,
                CostCenterId: costCenterId,
                LocationId: locationId,
                Remarks: remarks
            };

            $("#ReceiveItemGrid tbody tr").each(function () {
                orderQuantity = $(this).find("td:eq(3)").text();
                quantity = $(this).find("td:eq(4)").find("input").val();
                if (quantity == "" || quantity == "0") {
                    //toastr.info("Quantity Cannot Be Zero Or Empty.");
                    canReturn = false;
                    return ;
                }
                else if (parseFloat(quantity) > parseFloat(orderQuantity)) {
                    //toastr.info("Return Quantity Cannot Greater Than Max Return Quantity.");
                    canReturn = false;                    
                    return;
                }
                else {
                    canReturn = true;
                }
            });
            if (!canReturn) {
                toastr.warning("Return amount is not in correct quantity.");

                return false;
            }
            PageMethods.SaveItemReturn(ProductReturn, ReturnItemNewlyAdded, ReturnItemDeleted, AddedSerialzableProduct, DeletedSerialzableProduct, OnSavePurchaseOrderSucceed, OnSavePurchaseOrderFailed);

            return false;
        }
        function OnSavePurchaseOrderSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                if (queryOutId != "") {
                    window.location = "/PurchaseManagment/PurchaseReturnInformation.aspx";
                }

                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSavePurchaseOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }

        function PerfoEditItemReturnrmClearAction() {

            $("#ContentPlaceHolder1_hfReceivedId").val("0");
            $("#ContentPlaceHolder1_hfPOrderId").val("0");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("0");
            $("#ContentPlaceHolder1_hfCostCenterId").val("0");
            $("#ContentPlaceHolder1_hfLocationId").val("0");

            $("#ReceiveItemGrid tbody").html("");
            $("#ContentPlaceHolder1_txtRemarks").val("");

            queryOutId = "";
            AddedSerialCount = 0;

            ItemSelected = null;
            NewAddedSerial = new Array();
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();
            ReturnItemDeleted = new Array();

            SearchReceiveOrder();

            $("#btnSave").val("Save");
        }

        function GetReceivedOrderReturnForSearch(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#OutOrderSearchGrid tbody tr").length;
            var returnNumber = "0", status = "", returnType = "", fromDate = null, toDate = null;

            returnNumber = $("#ContentPlaceHolder1_txtReturnNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            $("#GridPagingContainer ul").html("");
            $("#OutOrderSearchGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchReturnOrder(fromDate, toDate, returnNumber, status,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

            return false;
        }
        function OnSearchPurchaseOrderSucceed(result) {

            var tr = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.ReturnNumber + "</td>";

                if (gridObject.ReturnDate != null)
                    tr += "<td style='width:10%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.ReturnDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:10%;'></td>";

                tr += "<td style='width:15%;'>" + gridObject.CostCenter + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.LocationName + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.Status + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:15%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:15%;'></td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";

                if (gridObject.IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditItemReturn(" + gridObject.ReturnId + "," + gridObject.ReceivedId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return ReceiveReturnDelete(" + gridObject.ReturnId + "," + gridObject.ReceivedId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return ReceiveReturnApproval('" + 'Checked' + "'," + gridObject.ReturnId + "," + gridObject.ReceivedId + ")\" alt='Checked'  title='Checked' border='0' />";
                }

                if (gridObject.IsCanApproved) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return ReceiveReturnApproval('" + 'Approved' + "', " + gridObject.ReturnId + "," + gridObject.ReceivedId + ")\" alt='Approved'  title='Approved' border='0' />";
                }

                //if (gridObject.Status == 'Approved') {
                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport(" + gridObject.ReturnId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Invoice' title='Receive Return Information' border='0' />";
                //}

                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.ReturnId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.TransactionId + "</td>";

                tr += "</tr>";

                $("#OutOrderSearchGrid tbody").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSearchPurchaseOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }
        function ClearSearch() {
            $("#ContentPlaceHolder1_txtReturnNumber").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("All");
            $("#ContentPlaceHolder1_txtFromDate").datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_txtToDate").datepicker("setDate", DayOpenDate);
        }

        function EditItemReturn(returnId, receivedId) {
            PageMethods.EditItemReturn(returnId, receivedId, OnEditPurchaseOrderSucceed, OnEditPurchaseOrderFailed);
            return false;
        }
        function OnEditPurchaseOrderSucceed(result) {
            $("#SerialItemTable tbody").html("");
            $("#ReceiveItemGrid tbody").html("");
            $("#ReceiveSummaryGrid tbody").html("");
            $("#ReceiveSearchContainer").hide();
            $("#btnReturnSearch").attr("disabled", true);

            LoadReceiveOrderWhenEdit(result.ProductReceive);

            AddedSerialCount = 0;
            ItemSelected = null;
            NewAddedSerial = new Array();
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();
            ReturnItemDeleted = new Array();

            $("#ContentPlaceHolder1_hfReturnId").val(result.ProductReturn.ReturnId);
            $("#ContentPlaceHolder1_hfReceivedId").val(result.ProductReturn.ReceivedId);
            $("#ContentPlaceHolder1_hfCostCenterId").val(result.ProductReturn.CostCenterId);
            $("#ContentPlaceHolder1_hfLocationId").val(result.ProductReturn.LocationId);
            $("#ContentPlaceHolder1_txtRemarks").val(result.ProductReturn.Remarks);

            var tr = "";

            $.each(result.ProductReturnDetails, function (count, gridObject) {

                tr += "<tr>";

                if (gridObject.ReturnQuantity > 0) {

                    if (parseInt(gridObject.ReturnDetailsId, 10) > 0) {
                        tr += "<td style='width:5%;text-align: center;'>" +
                            "<input type='checkbox' checked='checked' id='chk" + gridObject.ItemId + "'" + " />" +
                            "</td>";
                    }
                    else {
                        tr += "<td style='width:5%;text-align: center;'>" +
                            "<input type='checkbox' id='chk" + gridObject.ItemId + "'" + " />" +
                            "</td>";
                    }
                }
                else {
                    tr += "<td style='width:5%;'></td>";
                }

                tr += "<td style='width:40%;'>" + gridObject.ItemName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.HeadName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.ReturnQuantity + "</td>";

                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + gridObject.Quantity + "' id='pp" + gridObject.ItemId + "' class='form-control quantitydecimal' onblur='CheckTotalQuantity(this)' />" +
                    "</td>";

                tr += "<td style='width:15%;'>";
                if (gridObject.ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForReceiveItemReturn(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.ItemId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.StockById + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ReceivedId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ReceiveDetailsId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ReturnId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ReturnDetailsId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.Quantity + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ProductType + "</td>";

                tr += "<td style='display:none;'>" + gridObject.ColorId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.SizeId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.StyleId + "</td>";

                tr += "</tr>";

                $("#ReceiveItemGrid tbody").append(tr);
                tr = "";
            });

            AddedSerialzableProduct = result.ProductReturnSerialInfo;

            CommonHelper.ApplyDecimalValidation();
            $("#btnSave").val("Update");
            $("#myTabs").tabs({ active: 0 });

        }
        function OnEditPurchaseOrderFailed() { }

        function LoadReceiveOrderWhenEdit(result) {

            var tr = "";

            $.each(result, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.ReceiveNumber + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.ReceiveType + "</td>";

                if (gridObject.ReceivedDate != null)
                    tr += "<td style='width:8%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.ReceivedDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:8%;'></td>";

                tr += "<td style='width:15%;'>" + gridObject.CostCenter + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.LocationName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.SupplierName + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:22%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width22%;'></td>";

                tr += "<td style=\"text-align: center; width:5%; cursor:pointer;\">";
                //tr += "&nbsp;&nbsp;<img src='../Images/receiveitem.png' onClick= \"javascript:return ReceiveItemOut(this, " + gridObject.ReceivedId + ",'" + gridObject.ReceiveType + "'," + gridObject.POrderId + "," + gridObject.CostCenterId + "," + gridObject.LocationId + ")\" alt='Receive'  title='Receive' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.ReceivedId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.SupplierId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.CostCenterId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.LocationId + "</td>";

                tr += "</tr>";

                $("#ReceiveSummaryGrid tbody").append(tr);
                tr = "";
            });

            $("#ReceiveSummaryGrid tbody tr:eq(0)").css("background-color", "#e6ffe6");

            return false;
        }

        function CheckTotalQuantity(control) {
            var tr = $(control).parent().parent();

            var stockQuantity = $.trim($(tr).find("td:eq(3)").text());
            var quantity = $.trim($(tr).find("td:eq(4)").find("input").val());
            var oldQuantity = $(tr).find("td:eq(12)").text();

            if (quantity == "" || quantity == "0") {
                toastr.info("Quantity Cannot Be Zero Or Empty.");
                
                $(tr).find("td:eq(3)").find("input").val(oldQuantity);
                canReturn = false;
                return false;
            }
            else if (parseFloat(quantity) > parseFloat(stockQuantity)) {
                toastr.info("Return Quantity Cannot Greater Than Max Return Quantity.");
                $(tr).find("td:eq(4)").find("input").val(oldQuantity);
                canReturn = false;
                return false;
            }
            else {
                canReturn = true;
            }

            $(tr).find("td:eq(12)").text(quantity);
        }

        function PerformClearAction() {

            if ($("#ContentPlaceHolder1_hfReturnId").val() == "0") {
                SearchReceiveOrder();
            }
            else {
                $("#ReceiveSummaryGrid tbody").html("");
            }

            $("#ReceiveItemGrid tbody").html("");
            
            $("#ContentPlaceHolder1_txtReferenceNumber").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");

            $("#ContentPlaceHolder1_hfReturnId").val("0");
            $("#ContentPlaceHolder1_hfCostCenterId").val("0");
            $("#ContentPlaceHolder1_hfLocationId").val("0");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("0");
            $("#ContentPlaceHolder1_hfPOrderId").val("0");
            $("#ReceiveSearchContainer").show();

            queryOutId = "";
            AddedSerialCount = 0;

            ItemSelected = null;
            NewAddedSerial = new Array();
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();
            ReturnItemDeleted = new Array();

            $("#btnReturnSearch").attr("disabled", false);
            $("#btnSave").val("Save");
        }

        function ReceiveReturnApproval(ApprovedStatus, ReturnId, TransactionId) {
            if (!confirm("Do you Want To " + (ApprovedStatus == "Checked" ? "Check?" : "Approve?"))) {
                return false;
            }

            PageMethods.ReceiveReturnApproval(ReturnId, ApprovedStatus, TransactionId, OnApprovalSucceed, OnApprovalFailed);
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GetReceivedOrderReturnForSearch($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }

        function ReceiveReturnDelete(ReturnId, ReceivedId, ApprovedStatus, CreatedBy) {

            if (!confirm("Do you Want To Delete?")) {
                return false;
            }

            PageMethods.ReceiveReturnDelete(ReturnId, ApprovedStatus, ReceivedId, CreatedBy, OnApprovalSucceed, OnApprovalFailed);
        }

        function ShowReport(returnId, ApprovedStatus, CreatedBy) {
            var iframeid = 'printDoc';
            var url = "Reports/ReportPMPurchaseReturn.aspx?returnId=" + returnId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Purchase Return",
                show: 'slide'
            });
        }
        function CheckAllOrder() {
            if ($("#OrderCheck").is(":checked")) {
                $("#ReceiveItemGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#ReceiveItemGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }
    </script>

    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfReceivedId" runat="server" Value="0" />
    <asp:HiddenField ID="hfPOrderId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCostCenterId" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemIdForSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfReturnId" runat="server" Value="0" />

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Return Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Return</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default" id="ReceiveSearchContainer">
                <div class="panel-heading">
                    Item Return
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label12" runat="server" class="control-label" Text="Supplier"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lbl1000" runat="server" class="control-label" Text="Reference Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReferenceNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>

                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReceiveDateFrom" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReceiveDateTo" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-12">
                                <input type="button" id="btnReturnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchReceiveOrder()" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">
                    Receive Information
                </div>

                <div class="panel-body">
                    <table id="ReceiveSummaryGrid" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 10%;">Receive Number
                                </th>
                                <th style="width: 10%;">Order Type
                                </th>
                                <th style="width: 8%;">Order Date
                                </th>
                                <th style="width: 15%;">Cost Center
                                </th>
                                <th style="width: 15%;">Location
                                </th>
                                <th style="width: 15%;">Supplier
                                </th>
                                <th style="width: 22%;">Remarks
                                </th>
                                <th style="width: 5%;">Action
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">
                    Receive Item Information
                </div>
                <div class="panel-body">
                    <table id="ReceiveItemGrid" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 5%; text-align: center;">Select
                                    <input type="checkbox" value="" checked="checked" id="OrderCheck" onclick="CheckAllOrder()" />
                                </th>
                                <th style="width: 35%;">Item Name
                                </th>
                                <th style="width: 15%;">Stock By
                                </th>
                                <th style="width: 15%;">Max Return Quantity
                                </th>
                                <th style="width: 15%;">Return Quantity
                                </th>
                                <th style="width: 15%;">Action
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>

            <div class="form-horizontal">                
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>

                <div class="row" style="padding-top: 10px;">
                    <div class="col-md-12">
                        <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-large" value="Save" onclick="SaveItemReceiveOrderReturn()" />
                        <input type="button" id="btnClear" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="PerformClearActionWithConfirmation()" />
                    </div>
                </div>

            </div>

        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Item Return Search
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Return Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReturnNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                    <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                    <asp:ListItem Text="Cancel" Value="Cancel"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="GetReceivedOrderReturnForSearch(1, 1)" />
                                <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="ClearSearch()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">

                    <table id="OutOrderSearchGrid" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 10%;">Return Number
                                </th>
                                <th style="width: 10%;">Return Date
                                </th>
                                <th style="width: 15%;">From Cost Center
                                </th>
                                <th style="width: 15%;">To Cost Center
                                </th>
                                <th style="width: 10%;">Status
                                </th>
                                <th style="width: 15%;">Remarks
                                </th>
                                <th style="width: 15%;">Action
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="childDivSection">
                        <div class="text-center" id="GridPagingContainer">
                            <ul class="pagination">
                            </ul>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </div>

    <div id="SerialWindow" style="display: none;">
        <div class="panel panel-default">
            <div class="panel-body" style="padding: 4px;">

                <div class="form-horizontal">
                    <div class="row">
                        <div class="col-md-2">
                            <label class="control-label">Serial</label>
                        </div>
                        <div class="col-md-10">
                            <input type="text" id="txtSerial" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            <label class="control-label">Item Quantity</label>
                        </div>
                        <div class="col-md-3">
                            <label class="control-label" id="lblItemQuantity">0</label>
                        </div>
                        <div class="col-md-3">
                            <label class="control-label">Added Serial Quantity</label>
                        </div>
                        <div class="col-md-4">
                            <label class="control-label" id="lblAddedQuantity">0</label>
                        </div>
                    </div>
                    <hr />
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" class="btn btn-primary" style="padding-left: 30px; padding-right: 30px;" value="Add" onclick="AddSerialNumber()" />
                            <input type="button" class="btn btn-primary" value="Clear" onclick="ClearSerial()" />
                        </div>
                    </div>
                    <hr />
                </div>

                <div style="height: 350px; overflow-y: scroll;">
                    <table id="SerialItemTable" class="table table-bordered table-hover table-condensed">
                        <thead>
                            <tr>
                                <th style="width: 90%;">Serial Number</th>
                                <th style="width: 10%;">Action</th>
                                <th style="display: none;">Item Id</th>
                                <th style="display: none;">SerialId</th>
                                <th style="display: none;">Out SerialId</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>

            </div>
            <div class="panel-footer">
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" class="btn btn-primary" style="padding-left: 30px; padding-right: 30px;" value="Ok" onclick="ApplySerialForPurchaseItem()" />
                        <input type="button" class="btn btn-primary" value="Cancel" onclick="CancelAddSerial()" />
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="1000" height="800" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>

</asp:Content>
