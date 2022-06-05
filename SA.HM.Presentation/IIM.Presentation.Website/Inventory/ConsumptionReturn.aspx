<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="ConsumptionReturn.aspx.cs" EnableEventValidation="false" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.ConsumptionReturn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var queryOutId = "";
        var AddedSerialCount = 0;

        var ItemSelected = null;
        var NewAddedSerial = new Array();
        var AddedSerialzableProduct = new Array();
        var DeletedSerialzableProduct = new Array();
        var ReturnItemDeleted = new Array();
        var returnQty = 0;
        var IsValidSerial = false;

        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            if (Cookies.getJSON('outreturnoption') != undefined) {

                queryOutId = CommonHelper.GetParameterByName("rid");
                var returnType = CommonHelper.GetParameterByName("it");
                var transactionId = CommonHelper.GetParameterByName("tid");
                var returnId = CommonHelper.GetParameterByName("rid");

                PageMethods.EditItemOut(returnType, returnId, transactionId, OnEditPurchaseOrderSucceed, OnEditPurchaseOrderFailed);
            }

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


            $("#ContentPlaceHolder1_ddlCostCenterTo").change(function () {
                LoadLocation($(this).val());
            });
        });

        function SearchReturn() {
            issueNumber = $("#ContentPlaceHolder1_txtReferenceNumber").val();
            $("#OutOrderGrid tbody").html("");

            if ($.trim(issueNumber) == "") {
                toastr.warning("Please Give Reference Number.");
                return false;
            }

            PageMethods.SearchOutOrderForReturn(issueNumber, OnSearchOutOrderForReturnSucceed, OnSearchOutOrderForReturnFailed);
        }
        function OnSearchOutOrderForReturnSucceed(result) {

            if (result.length == 0) {
                toastr.info("Item Is Already Used By Another Return Order. Please Search and Check the Report.");
                return false;
            }

            var tr = "";

            $.each(result[0], function (count, gridObject) {

                tr += "<tr>";

                if (gridObject.ReturnQuantity > 0) {
                    tr += "<td style='width:5%;'>" +
                        "<input type='checkbox' checked='checked' id='chk" + gridObject.ItemId + "'" + " />" +
                        "</td>";
                }
                else {
                    tr += "<td style='width:5%;'></td>";
                }

                tr += "<td style='width:35%;'>" + gridObject.ItemName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.HeadName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.Quantity + "</td>";

                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + gridObject.Quantity + "' id='pp" + gridObject.ItemId + "' class='form-control quantitydecimal' onblur='CheckTotalQuantity(this)' />" +
                    "</td>";

                tr += "<td style='width:15%;'>";
                if (gridObject.ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForOutItemReturn(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.ItemId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.StockById + "</td>";
                tr += "<td style='display:none;'>" + gridObject.OutId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.OutDetailsId + "</td>";
                tr += "<td style='display:none;'>0</td>";
                tr += "<td style='display:none;'>0</td>";
                tr += "<td style='display:none;'>" + gridObject.Quantity + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ProductType + "</td>";

                tr += "</tr>";

                $("#OutOrderGrid tbody").append(tr);
                tr = "";
            });

            $("#ContentPlaceHolder1_hfLocationId").val(result[0][0].FromLocationId);
            LoadLocation(result[0][0].FromCostCenterId);

            $("#ContentPlaceHolder1_ddlCostCenterTo").val(result[0][0].FromCostCenterId);
            $("#ContentPlaceHolder1_lblConsumptionBy").text(result[1]);
            $("#hfTransactionId").val(result[0][0].OutId);
        }
        function OnSearchOutOrderForReturnFailed() { }

        function LoadLocation(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationToSucceeded, OnLoadLocationFailed);
        }
        function OnLoadLocationToSucceeded(result) {
            var control = $('#ContentPlaceHolder1_ddlLocationTo');

            control.empty();
            if (result != null) {
                if (result.length > 0) {
                    if (result.length > 1)
                        control.empty().append('<option value="0">---Please Select---</option>');
                    for (i = 0; i < result.length; i++) {
                        control.append('<option title="' + result[i].Name + '" value="' + result[i].LocationId + '">' + result[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }
            if (result.length == 1 && $("#ContentPlaceHolder1_hfLocationId").val() == "0")
                control.val($("#ContentPlaceHolder1_ddlLocationTo option:first").val());
            else
                control.val($("#ContentPlaceHolder1_hfLocationId").val());
            return false;
        }
        function OnLoadLocationFailed() { }

        function CheckTotalQuantity(control) {
            var tr = $(control).parent().parent();

            var stockQuantity = $.trim($(tr).find("td:eq(3)").text());
            var quantity = $.trim($(tr).find("td:eq(4)").find("input").val());
            var oldQuantity = $(tr).find("td:eq(12)").text();
            returnQty = quantity;
            if (quantity == "" || quantity == "0") {
                toastr.info("Quantity Cannot Be Zero Or Empty.");
                $(tr).find("td:eq(3)").find("input").val(oldQuantity);
                return false;
            }
            else if (parseFloat(quantity) > parseFloat(stockQuantity)) {
                toastr.info("Return Quantity Cannot Greater Than Max Return Quantity.");
                $(tr).find("td:eq(4)").find("input").val(oldQuantity);
                return false;
            }

            $(tr).find("td:eq(12)").text(quantity);
        }

        function AddSerialForOutItemReturn(control) {
            var tr = $(control).parent().parent();

            ItemSelected = tr;

            var itemName = $(tr).find("td:eq(1)").text();
            var itemId = $(tr).find("td:eq(6)").text();
            //var quantity = $(tr).find("td:eq(4)").text(); 
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
            var serial = $.trim($("#txtSerial").val());
            var addedQuantity = $("#lblAddedQuantity").text();
            var totalQuantity = $("#lblItemQuantity").text();
            var outOrderId = $("#hfTransactionId").val();
            var fromLocationId = $("#ContentPlaceHolder1_ddlLocationTo").val();

            if (serial == "") {
                toastr.warning("Please Give Serial.");
                return false;
            }
            else if ((parseInt(addedQuantity, 10) + 1) > parseInt(totalQuantity, 10)) {
                toastr.warning("Number Of Serial Cannot Greater Than Item Quantity.");
                return false;
            }
            CheckSerialAvailability(outOrderId, fromLocationId, serial);
            if (IsValidSerial == false) {
                toastr.warning("This Serial Is Not Valid For Return");
                IsValidSerial = false;
                return false;
            }
            var alreadySaved = _.findWhere(AddedSerialzableProduct, { SerialNumber: serial });
            var newAlreadyAdded = _.findWhere(NewAddedSerial, { SerialNumber: serial });

            if (alreadySaved != null || newAlreadyAdded != null) {
                toastr.warning("This Serial Already Added.");
                return false;
            }

            var outId = $("#hfReturnId").val();
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
        function CheckSerialAvailability(receivedId, locationId, serial) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/ConsumptionReturn.aspx/CheckSerialAvailability',

                data: "{ 'TransactionId':'" + receivedId + "','serialId':'" + serial + "','LocationId':'" + locationId + "'}",
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

        function SaveItemOutOrderReturn() {

            if ($("#ContentPlaceHolder1_ddlCostCenterTo").val() == "0") {
                toastr.warning("Please Select Cost Center.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlLocationTo").val() == "0") {
                toastr.warning("Please Select Location.");
                return false;
            }

            var itemId = 0, remarks = "";
            var purchaseItem = null;

            var returnId = 0, outOrderId = "0", returnType = "", receivedByDate = null, returnDetailsId = 0, isEdited = "0", productOutFor = '',
                stockById = "", fromCostCenterId = "", fromLocationId = 0, toCostCenterId = 0, toLocationId = "", requisitionId = '',
                quantity = 0, orderQuantity = 0, outDetailsId = 0, productType = '', itemName = '', checkQuantity = false;

            outOrderId = $("#hfTransactionId").val();
            returnId = $("#hfReturnId").val();

            fromCostCenterId = $("#ContentPlaceHolder1_ddlCostCenterTo").val();
            fromLocationId = $("#ContentPlaceHolder1_ddlLocationTo").val();
            returnType = "OutReturn";

            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            checkedBy = 0;
            approvedBy = 0;
            var ReturnItemNewlyAdded = new Array();

            currencyId = $("#ContentPlaceHolder1_ddlCurrency").val();
            convertionRate = $("#ContentPlaceHolder1_lblConversionRate").text();

            var serialCount = 0, rowSerial = 0, srlItem = null, srlIndex = -1;
            var itemSerial = null;

            $("#OutOrderGrid tbody tr").each(function () {

                returnDetailsId = parseInt($(this).find("td:eq(11)").text(), 10);
                outDetailsId = parseInt($(this).find("td:eq(9)").text(), 10);
                itemId = parseInt($(this).find("td:eq(6)").text(), 10);
                stockById = parseInt($(this).find("td:eq(7)").text(), 10);
                orderQuantity = $(this).find("td:eq(3)").text();
                quantity = $(this).find("td:eq(4)").find("input").val();
                productType = $.trim($(this).find("td:eq(13)").text());
                itemName = $.trim($(this).find("td:eq(1)").text());
                debugger;
                if (returnQty > orderQuantity) {
                    checkQuantity = true;
                    return false;
                }
                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {

                    ReturnItemNewlyAdded.push({

                        ReturnDetailsId: returnDetailsId,
                        ReturnId: returnId,
                        OutDetailsId: outDetailsId,
                        StockById: stockById,
                        ProductType: productType,
                        ItemId: itemId,
                        ItemName: itemName,
                        OrderQuantity: orderQuantity,
                        Quantity: quantity
                    });
                }
                else if (returnDetailsId > 0) {
                    ReturnItemDeleted.push({

                        ReturnDetailsId: returnDetailsId,
                        ReturnId: returnId,
                        OutDetailsId: outDetailsId,
                        StockById: stockById,
                        ProductType: productType,
                        ItemId: itemId,
                        ItemName: itemName,
                        OrderQuantity: orderQuantity,
                        Quantity: quantity
                    });

                    serialCount = 0, rowSerial = 0;
                    itemSerial = _.where(AddedSerialzableProduct, { ItemId: itemId });
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
            if (checkQuantity == true) {
                toastr.warning("Return  Quantity can not greater than Max Return Quantity.");
                returnQty = 0;
                return false;
            }
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

            var ProductOut = {

                ReturnId: returnId,
                ReturnType: returnType,
                TransactionId: outOrderId,
                FromCostCenterId: fromCostCenterId,
                FromLocationId: fromLocationId,
                Remarks: remarks
            };

            PageMethods.SaveItemOutOrder(ProductOut, ReturnItemNewlyAdded, ReturnItemDeleted, AddedSerialzableProduct, DeletedSerialzableProduct, OnSavePurchaseOrderSucceed, OnSavePurchaseOrderFailed);

            return false;
        }
        function OnSavePurchaseOrderSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                if (queryOutId != "") {
                    window.location = "/Inventory/ConsumptionReturnInformation.aspx";
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

        function PerformClearAction() {

            $("#hfReturnId").val("0");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("0");

            $("#OutOrderGrid tbody").html("");

            $("#ContentPlaceHolder1_lblConsumptionBy").text("");
            $("#ContentPlaceHolder1_ddlCostCenterTo").val("0");
            $("#ContentPlaceHolder1_ddlLocationTo").val("0");
            $("#ContentPlaceHolder1_txtReferenceNumber").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_hfLocationId").val("0");

            queryOutId = "";
            AddedSerialCount = 0;

            ItemSelected = null;
            NewAddedSerial = new Array();
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();
            ReturnItemDeleted = new Array();

            $("#ContentPlaceHolder1_txtReferenceNumber").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlCostCenterTo").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlLocationTo").attr("disabled", false);
            $("#btnReturnSearch").attr("disabled", false);

            $("#btnSave").val("Save");
        }

        function SearchOutOrder(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#OutOrderSearchGrid tbody tr").length;
            var returnNumber = "0", status = "", returnType = "", fromDate = null, toDate = null;

            returnNumber = $("#ContentPlaceHolder1_txtReturnNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            returnType = $("#ContentPlaceHolder1_ddlSearchReturnType").val();
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

            PageMethods.SearchReturnOrder(returnType, fromDate, toDate, returnNumber, status,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

            return false;
        }
        function OnSearchPurchaseOrderSucceed(result) {

            var tr = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.ReturnNumber + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.ReturnType + "</td>";

                if (gridObject.ReturnDate != null)
                    tr += "<td style='width:10%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.ReturnDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:10%;'></td>";

                tr += "<td style='width:15%;'>" + gridObject.FromCostCenter + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.FromLocation + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.Status + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:15%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:15%;'></td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";

                if (gridObject.IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditItemOut('" + gridObject.ReturnType + "'," + gridObject.ReturnId + "," + gridObject.TransactionId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return OutOrderDelete('" + gridObject.ReturnType + "'," + gridObject.ReturnId + "," + gridObject.TransactionId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return OutOrderApproval('" + gridObject.ReturnType + "','" + 'Checked' + "'," + gridObject.ReturnId + "," + gridObject.TransactionId + ")\" alt='Checked'  title='Checked' border='0' />";
                }

                if (gridObject.IsCanApproved) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return OutOrderApproval('" + gridObject.ReturnType + "','" + 'Approved' + "', " + gridObject.ReturnId + "," + gridObject.TransactionId + ")\" alt='Approved'  title='Approved' border='0' />";
                }

                //if (gridObject.Status == 'Approved') {
                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.ReturnType + "'," + gridObject.ReturnId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Invoice' title='Consumption Return Information' border='0' />";
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
            $("#ContentPlaceHolder1_txtIssueNumber").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("All");
            $("#ContentPlaceHolder1_ddlSearchProductOutFor").val("All");
            $("#ContentPlaceHolder1_txtFromDate").datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_txtToDate").datepicker("setDate", DayOpenDate);
        }

        function EditItemOut(returnType, returnId, transactionId) {
            PageMethods.EditItemOut(returnType, returnId, transactionId, OnEditPurchaseOrderSucceed, OnEditPurchaseOrderFailed);
            return false;
        }
        function OnEditPurchaseOrderSucceed(result) {

            $("#SerialItemTable tbody").html("");
            $("#OutOrderGrid tbody").html("");
            AddedSerialCount = 0;
            ItemSelected = null;
            NewAddedSerial = new Array();
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();
            ReturnItemDeleted = new Array();

            $("#hfReturnId").val(result.ProductReturn.ReturnId);
            $("#hfTransactionId").val(result.ProductReturn.TransactionId);

            $("#ContentPlaceHolder1_txtReferenceNumber").val(result.ProductReturn.IssueNumber);
            $("#ContentPlaceHolder1_ddlCostCenterTo").val(result.ProductReturn.FromCostCenterId);
            $("#ContentPlaceHolder1_lblConsumptionBy").text(result.ProductReturn.ConsumptionBy);

            $("#ContentPlaceHolder1_hfLocationId").val(result.ProductReturn.FromLocationId);
            LoadLocation(result.ProductReturn.FromCostCenterId);

            $("#ContentPlaceHolder1_txtRemarks").val(result.ProductReturn.Remarks);

            var tr = "";

            $.each(result.ProductReturnDetails, function (count, gridObject) {

                tr += "<tr>";

                if (gridObject.ReturnQuantity > 0) {

                    if (parseInt(gridObject.ReturnDetailsId, 10) > 0) {
                        tr += "<td style='width:5%;'>" +
                            "<input type='checkbox' checked='checked' id='chk" + gridObject.ItemId + "'" + " />" +
                            "</td>";
                    }
                    else {
                        tr += "<td style='width:5%;'>" +
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
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForOutItemReturn(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.ItemId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.StockById + "</td>";
                tr += "<td style='display:none;'>" + gridObject.OutId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.OutDetailsId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ReturnId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ReturnDetailsId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.Quantity + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ProductType + "</td>";

                tr += "</tr>";

                $("#OutOrderGrid tbody").append(tr);
                tr = "";
            });

            AddedSerialzableProduct = result.ProductReturnSerialInfo;

            $("#ContentPlaceHolder1_txtReferenceNumber").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlCostCenterTo").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlLocationTo").attr("disabled", true);
            $("#btnReturnSearch").attr("disabled", true);

            CommonHelper.ApplyDecimalValidation();
            $("#btnSave").val("Update");
            $("#myTabs").tabs({ active: 0 });

        }
        function OnEditPurchaseOrderFailed() { }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchOutOrder(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }

        function CheckAllOrder() {
            if ($("#OrderCheck").is(":checked")) {
                $("#OutOrderGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#OutOrderGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }

        function OutOrderApproval(ReturnType, ApprovedStatus, ReturnId, TransactionId) {
            if (!confirm("Do you Want To " + (ApprovedStatus == "Checked" ? "Check?" : "Approve?"))) {
                return false;
            }

            PageMethods.OutOrderApproval(ReturnType, ReturnId, ApprovedStatus, TransactionId, OnApprovalSucceed, OnApprovalFailed);
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchOutOrder($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }

        function OutOrderDelete(ReturnType, ReturnId, TransactionId, ApprovedStatus, CreatedBy) {

            if (!confirm("Do you Want To Delete?")) {
                return false;
            }

            PageMethods.OutOrderDelete(ReturnType, ReturnId, ApprovedStatus, TransactionId, CreatedBy, OnApprovalSucceed, OnApprovalFailed);
        }

        function ShowReport(IssueType, OutId, ApprovedStatus, CreatedBy) {
            var iframeid = 'printDoc';
            var url = "./Reports/frmReportProductReturn.aspx?ReturnId=" + OutId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Consumption Return",
                show: 'slide'
            });
            
        }

    </script>

    <input type="hidden" id="hfReturnId" value="0" />
    <input type="hidden" id="hfTransactionId" value="0" />
    <asp:HiddenField ID="hfItemIdForSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationId" runat="server" Value="0" />
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Item Return</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Item Return</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Item Return
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lbl1000" runat="server" class="control-label" Text="Reference Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtReferenceNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <input type="button" id="btnReturnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchReturn()" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label5" runat="server" class="control-label" Text="Consumption By"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:Label ID="lblConsumptionBy" runat="server" class="form-control" Text=""></asp:Label>
                                </div>
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

                    <table id="OutOrderGrid" class="table table-bordered table-condensed table-responsive">
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

                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field"
                                    Text="Return Store"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCostCenterTo" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>

                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Store Location"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLocationTo" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>

                        </div>

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
                                <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-large" value="Save" onclick="SaveItemOutOrderReturn()" />
                                <input type="button" id="btnClear" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="PerformClearAction()" />
                            </div>
                        </div>

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
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label1" runat="server" class="control-label" Text="Return Type"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlSearchReturnType" runat="server" CssClass="form-control"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
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
                                    <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchOutOrder(1, 1)" />
                                    <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="ClearSearch()" />
                                </div>
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
                                <th style="width: 10%;">Return Type
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

</asp:Content>
