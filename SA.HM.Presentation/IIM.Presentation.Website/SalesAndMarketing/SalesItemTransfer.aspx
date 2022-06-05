<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="SalesItemTransfer.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.SalesItemTransfer" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var TransferItemAdded = new Array();
        var NewAddedSerial = new Array();
        var AddedSerialzableProduct = new Array();
        var DeletedSerialzableProduct = new Array();
        var AddedSerialCount = 0;
        var TransferItemNewlyAdded = new Array();
        var TransferItemForEdit = new Array();
        var TransferItemDeleted = new Array();

        $(document).ready(function () {
            GridPaging(1, 1);
            $("#ContentPlaceHolder1_ddlQuotationForSearch").select2({
                tags: false,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCostCenterFrom").change(function () {

                if ($(this).val() != "0") {
                    LoadLocationFrom($(this).val());
                }

            });
            $("#ContentPlaceHolder1_ddlLocationFrom").change(function () {

                $("#ContentPlaceHolder1_ddlQuotation").trigger('change');
            });
            $("#ContentPlaceHolder1_ddlQuotation").change(function () {

                quotationId = $("#ContentPlaceHolder1_ddlQuotation").val();
                costCenterId = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
                locationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();
                LoadByQuotation(quotationId, costCenterId, locationId);

            });
            $("#ContentPlaceHolder1_btnCancel").click(function () {
                $('#ContentPlaceHolder1_ddlQuotationForSearch').val('0').trigger('change.select2');
                GridPaging(1, 1);
                return false;
            });

        });

        function AddNewSalesItemTransfer() {
            ClearAction();
            $("#AddSalesTransfer").dialog({
                autoOpen: true,
                modal: true,
                width: '80%',
                maxWidth: '95%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Sales Item Transfer",
                hide: 'fold',
                show: 'slide',
                close: function (event, ui) {
                    $("#ContentPlaceHolder1_btnSave").val("Save");
                    $("#SalesOrderItemTbl tbody").empty();
                    $("#ContentPlaceHolder1_ddlQuotation").val("0").prop('disabled', false);
                    $("#ContentPlaceHolder1_hfSalesTransferId").val("0");
                    $("#ContentPlaceHolder1_ddlCostCenterFrom").val("0").prop('disabled', false);
                    $("#ContentPlaceHolder1_ddlLocationFrom").val("0").prop('disabled', false);

                    $("#ContentPlaceHolder1_hfLocationFromId").val("0");
                    $("#ContentPlaceHolder1_hfItemIdForSerial").val("0");
                }
            });
            CommonHelper.SpinnerClose();
            return false;
        }
        function LoadLocationFrom(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationFromSucceeded, OnLoadLocationFailed);
        }
        function OnLoadLocationFromSucceeded(result) {
            var control = $('#ContentPlaceHolder1_ddlLocationFrom');

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
            if (result.length == 1 && $("#ContentPlaceHolder1_hfLocationFromId").val() == "0")
                control.val($("#ContentPlaceHolder1_ddlLocationFrom option:first").val());
            else {
                control.val($("#ContentPlaceHolder1_hfLocationFromId").val());
                if ($("#ContentPlaceHolder1_hfSalesTransferId").val() == "0") {
                    $("#ContentPlaceHolder1_ddlQuotation").trigger('change');
                }

            }


            return false;
        }
        function OnLoadLocationFailed() { }

        function LoadByQuotation(quotationId, costCenterId, locationId) {
            if (costCenterId == 0 || locationId == 0) {
                toastr.warning("Please Select Cost Center And Location");
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SalesItemTransfer.aspx/GetQuotationDetalsByQuotationId',

                data: JSON.stringify({ quotationId: quotationId, costCenterId: costCenterId, locationId: locationId }),
                dataType: "json",
                success: function (data) {
                    if (data.d.count <= 0) {
                        toastr.warning("No Items of Selected Quotation are not mapped with selected Cost Center or Location");
                        return false;
                    }
                    LoadTableByQuotationId(data);

                },
                error: function (result) {

                }
            });
        }
        function LoadTableByQuotationId(data) {

            $("#SalesOrderItemTbl tbody").empty();
            var i = 0;

            $.each(data.d, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }
                tr += "<td style='width:5%; text-align: center'>"
                if (gridObject.StockQuantity > 0) {
                    tr += "<input type='checkbox' checked='checked' id='chk' " + gridObject.QuotationDetailsId + " />"
                }
                tr += "</td>";

                tr += "<td style='width:20%;'>" + gridObject.ItemName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.HeadName + "</td>";
                if (gridObject.StockQuantity > 0) {
                    tr += "<td style='width:10%;'>" + gridObject.StockQuantity + "</td>";
                } else
                    tr += "<td style='width:10%;'> 0 </td>";

                tr += "<td style='width:10%;'>" + (gridObject.Quantity - gridObject.RemainingDeliveryQuantity) + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.Quantity + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.RemainingDeliveryQuantity + "</td>";
                if (gridObject.RemainingDeliveryQuantity <= 0 || gridObject.StockQuantity <= 0) {
                    tr += "<td style='width:15%;'>" + "<input disabled type='text' value='" + gridObject.RemainingDeliveryQuantity + "' id='q' " + gridObject.Id + " class='form-control quantityint'  onchange='CheckPurchaseOrderWiseQuantity(this)'/>" + "</td>";
                }
                else {
                    tr += "<td style='width:15%;'>" + "<input type='text' value='" + gridObject.RemainingDeliveryQuantity + "' id='q' " + gridObject.Id + " class='form-control quantityint'  onchange='CheckPurchaseOrderWiseQuantity(this)'/>" + "</td>";
                }

                tr += "<td style='width:5%;'>";
                if (gridObject.ProductType == 'Serial Product' && gridObject.StockQuantity > 0) {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForPurchaseWiseItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";
                tr += "<td style='display:none'>" + gridObject.ItemId + "</td>";
                tr += "<td style='display:none'>" + gridObject.StockBy + "</td>";
                tr += "<td style='display:none'>" + gridObject.UnitPrice + "</td>";
                tr += "<td style='display:none'>" + gridObject.Quantity + "</td>";
                tr += "<td style='display:none'>" + gridObject.RemainingDeliveryQuantity + "</td>";
                tr += "<td style='display:none'>" + gridObject.SalesTransferDetailId + "</td>";
                tr += "<td style='display:none'>" + gridObject.ProductType + "</td>";

                tr += "</tr>";

                $("#SalesOrderItemTbl tbody").append(tr);

                tr = "";
                i++;
            });
            CommonHelper.ApplyIntValidation();
            return false;
        }
        function LoadBySale(id) {

            $("#ContentPlaceHolder1_hfSalesTransferId").val(id);
            $("#ContentPlaceHolder1_btnSave").val("Update");

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SalesItemTransfer.aspx/GetQuotationDetailsBySalesTransferId',

                data: "{'SalesTransferId':'" + id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit - " + data.d.SMSalesTransferBO.TransferNumber + "?")) {
                        return false;
                    }
                    AddNewSalesItemTransfer();
                    LoadTableBySaleId(data);
                    
                },
                error: function (result) {

                }
            });
        }
        function LoadTableBySaleId(data) {

            $("#SalesOrderItemTbl tbody").empty();
            var i = 0;

            $("#ContentPlaceHolder1_ddlQuotation").val(data.d.SMSalesTransferBO.QuotationId).prop('disabled', true);
            $("#ContentPlaceHolder1_ddlCostCenterFrom").val(data.d.SMSalesTransferBO.CostCenterId).trigger('change').prop('disabled', true);
            $("#ContentPlaceHolder1_hfLocationFromId").val(data.d.SMSalesTransferBO.LocationID);
            $("#ContentPlaceHolder1_ddlLocationFrom").prop('disabled', true);
            TransferItemAdded = data.d;
            $.each(data.d.SMQuotationDetailsBOList, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }
                tr += "<td style='width:5%; text-align: center'>"
                if (gridObject.StockQuantity > 0) {
                    if (gridObject.SalesTransferDetailId == 0) {
                        tr += "<input type='checkbox' id='chk' " + gridObject.QuotationDetailsId + " />"
                    }
                    else {
                        tr += "<input type='checkbox' checked='checked' id='chk' " + gridObject.QuotationDetailsId + " />"
                    }
                }
                tr += "</td>";

                tr += "<td style='width:20%;'>" + gridObject.ItemName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.HeadName + "</td>";
                if (gridObject.StockQuantity > 0) {
                    tr += "<td style='width:10%;'>" + gridObject.StockQuantity + "</td>";
                } else
                    tr += "<td style='width:10%;'> 0 </td>";
                tr += "<td style='width:10%;'>" + (gridObject.TransferedQuantity) + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.Quantity + "</td>";
                tr += "<td style='width:10%;'>" + (gridObject.RemainingDeliveryQuantity + gridObject.TransferedQuantity) + "</td>";
                if ((gridObject.RemainingDeliveryQuantity + gridObject.TransferedQuantity) <= 0 || gridObject.StockQuantity <= 0) {
                    tr += "<td style='width:15%;'>" + "<input disabled type='text' value='" + (gridObject.TransferedQuantity) + "' id='q' " + gridObject.Id + " class='form-control quantityint'  onchange='CheckPurchaseOrderWiseQuantity(this)'/>" + "</td>";
                }
                else {
                    tr += "<td style='width:15%;'>" + "<input type='text' value='" + (gridObject.TransferedQuantity) + "' id='q' " + gridObject.Id + " class='form-control quantityint'  onchange='CheckPurchaseOrderWiseQuantity(this)'/>" + "</td>";
                }

                tr += "<td style='width:5%;'>";
                if (gridObject.ProductType == 'Serial Product' && gridObject.StockQuantity > 0) {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForPurchaseWiseItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";
                tr += "<td style='display:none'>" + gridObject.ItemId + "</td>";
                tr += "<td style='display:none'>" + gridObject.StockBy + "</td>";
                tr += "<td style='display:none'>" + gridObject.UnitPrice + "</td>";
                tr += "<td style='display:none'>" + gridObject.Quantity + "</td>";
                tr += "<td style='display:none'>" + gridObject.RemainingDeliveryQuantity + "</td>";
                tr += "<td style='display:none'>" + gridObject.SalesTransferDetailId + "</td>";
                tr += "<td style='display:none'>" + gridObject.ProductType + "</td>";

                tr += "</tr>";

                $("#SalesOrderItemTbl tbody").append(tr);

                tr = "";
                i++;
            });
            LoadSerialInfo(data.d.SMSalesTransferItemSerialList);
            CommonHelper.ApplyIntValidation();
            return false;
        }

        function LoadSerialInfo(serialList) {
            AddedSerialzableProduct = serialList;

            if (serialList != null) {
                if (serialList.length > 0) {
                    $("#lblAddedQuantity").text(serialList);
                    AddedSerialCount = serialList.length;
                }
            }
        }
        function CheckPurchaseOrderWiseQuantity(control) {
            var tr = $(control).parent().parent();
            var qty = $.trim($(control).val());
            var max = parseFloat($(tr).find("td:eq(6)").text());
            var stock = parseFloat($(tr).find("td:eq(3)").text());

            if (qty > max) {
                toastr.warning("Deliver Quantity cann't greater than " + max + "");
                $(control).val(max);
                return false;
            }
            else if (qty <= 0) {
                toastr.warning("Deliver Quantity cann't be 0 or lase than 0");
                $(control).val(max);
                return false;
            }
            else if (stock <= 0) {
                toastr.warning("This Item Is Not available");
                $(control).val("0");
                return false;
            }

        }
        function AddSerialForPurchaseWiseItem(control) {
            var tr = $(control).parent().parent();
            if ($(tr).find("td:eq(0)").find("input").is(":checked") != true) {
                toastr.warning("Check The Check Box First");
                return false;
            }

            var itemName = $(tr).find("td:eq(1)").text();
            var itemId = $(tr).find("td:eq(9)").text();
            var quantity = $(tr).find("td:eq(7)").find("input").val();

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
                        tr += "<td style='display:none;'>" + addedSerial[row].SalesItemSerialTransferId + "</td>";

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
        function DeleteItemSerial(control) {

            var tr = $(control).parent().parent();
            var serialNumber = $(tr).find("td:eq(0)").text();
            var itemId = parseInt($(tr).find("td:eq(2)").text(), 10);
            var salesItemSerialTransferId = parseInt($(tr).find("td:eq(3)").text(), 10);

            var addedQuantity = $("#lblAddedQuantity").text();
            AddedSerialCount = parseInt(addedQuantity, 10) - 1;

            var item = _.findWhere(AddedSerialzableProduct, { SerialNumber: serialNumber });
            var index = _.indexOf(AddedSerialzableProduct, item);
            AddedSerialzableProduct.splice(index, 1);

            var itemNew = _.findWhere(NewAddedSerial, { SerialNumber: serialNumber });
            var indexNew = _.indexOf(NewAddedSerial, itemNew);
            NewAddedSerial.splice(indexNew, 1);

            if (salesItemSerialTransferId > 0) {
                DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(item)));
            }

            $("#lblAddedQuantity").text(AddedSerialCount);
            $(tr).remove();
        }
        function CheckAndAddedSerialWiseProduct() {
            CommonHelper.SpinnerOpen();
            SerialCheck = new Array();
            var itemId = $("#ContentPlaceHolder1_hfItemIdForSerial").val();
            var serial = $.trim($("#txtSerial").val());

            SerialCheck.push({
                OutSerialId: 0,
                ItemId: parseInt(itemId, 10),
                SerialNumber: serial
            });

            fromLocationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();

            PageMethods.SerialAvailabilityCheck(fromLocationId, SerialCheck, CheckAndAddedSerialWiseProductSucceeded, CheckAndAddedSerialWiseProductFailed);
            return false;
        }

        function CheckAndAddedSerialWiseProductSucceeded(result) {

            if (result.IsSuccess) {
                serialNumber = $.trim($("#txtSerial").val());
                AddSerialNumber();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function CheckAndAddedSerialWiseProductFailed(error) {
            toastr.error(error);
            CommonHelper.SpinnerClose();
        }
        function CancelAddSerial() {
            $("#SerialWindow").dialog("close");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("");
            AddedSerialCount = 0;
            NewAddedSerial = new Array();
        }

        function AddSerialNumber() {

            var itemId = $("#ContentPlaceHolder1_hfItemIdForSerial").val();
            var addedQuantity = $("#lblAddedQuantity").text();
            var totalQuantity = $("#lblItemQuantity").text();
            var serial = serialNumber;

            if (serial == "") {
                toastr.warning("Please Give Serial.");
                return false;
            }
            else if ((parseInt(addedQuantity, 10) + 1) > parseInt(totalQuantity, 10)) {
                toastr.warning("Number Of Serial Cannot Greater Than Item Quantity.");
                return false;
            }

            var alreadySaved = _.findWhere(AddedSerialzableProduct, { SerialNumber: serial });
            var newAlreadyAdded = _.findWhere(NewAddedSerial, { SerialNumber: serial });

            if (alreadySaved != null || newAlreadyAdded != null) {
                toastr.warning("This Serial Already Added.");
                return false;
            }

            var outId = $("#ContentPlaceHolder1_hfOutId").val();
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
                OutSerialId: 0,
                ItemId: parseInt(itemId, 10),
                SerialNumber: serial
            });

            AddedSerialCount = AddedSerialCount + 1;
            $("#lblAddedQuantity").text(AddedSerialCount);

            tr = "";
            $("#txtSerial").val("");
        }
        function ApplySerialForPurchaseItem() {
            var addedQuantity = $("#lblAddedQuantity").text();
            var totalQuantity = $("#lblItemQuantity").text();

            if (parseInt(addedQuantity, 10) < parseInt(totalQuantity, 10)) {
                toastr.warning("Number Of Serial Must Added As Equall Item Quantity.");
                return false;
            }

            $(NewAddedSerial).each(function (index, obj) {

                AddedSerialzableProduct.push({
                    SalesItemSerialTransferId: obj.OutSerialId,
                    ItemId: obj.ItemId,
                    SerialNumber: obj.SerialNumber
                });
            });

            $("#SerialWindow").dialog("close");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("");
            AddedSerialCount = 0;
            NewAddedSerial = new Array();
        }
        function PerformSave() {
            var count = 0;
            var SerialCount = 0;
            $("#SalesOrderItemTbl tbody tr").each(function () {
                if (($(this).find("td:eq(0)").find("input").is(":checked") == true))
                    count++;
            });
            if (count == 0) {
                toastr.warning("Check atlist one item.");
                return false;
            }
            var SalesTransferDetailId = parseInt($(this).find("td:eq(14)").val());
            $("#SalesOrderItemTbl tbody tr").each(function () {
                itemId = parseInt($(this).find("td:eq(9)").text(), 10);
                if (parseInt($(this).find("td:eq(14)").text()) == null) {
                    SalesTransferDetailId = 0;
                }
                else {
                    SalesTransferDetailId = parseInt($(this).find("td:eq(14)").text());
                }
                var productType = ($(this).find("td:eq(15)").text());

                if (($(this).find("td:eq(0)").find("input").is(":checked") == true)) {
                    if (SalesTransferDetailId == 0) {
                        TransferItemNewlyAdded.push({
                            SalesTransferDetailId: SalesTransferDetailId,
                            StockById: parseInt($(this).find("td:eq(10)").text()),
                            AverageCost: parseInt($(this).find("td:eq(11)").text()),
                            ItemId: itemId,
                            Quantity: parseFloat($(this).find("td:eq(7)").find("input").val()),
                            ProductType: productType,
                            ItemName: $(this).find("td:eq(1)").text()
                        });
                    }
                    else {
                        TransferItemForEdit.push({
                            SalesTransferDetailId: SalesTransferDetailId,
                            StockById: parseInt($(this).find("td:eq(10)").text()),
                            AverageCost: parseInt($(this).find("td:eq(11)").text()),
                            ItemId: itemId,
                            Quantity: parseFloat($(this).find("td:eq(7)").find("input").val()),
                            ProductType: productType,
                            ItemName: $(this).find("td:eq(1)").text()
                        });
                    }
                }
                else if (SalesTransferDetailId > 0) {
                    TransferItemDeleted.push({
                        SalesTransferDetailId: SalesTransferDetailId,
                        StockById: parseInt($(this).find("td:eq(10)").text()),
                        AverageCost: parseInt($(this).find("td:eq(11)").text()),
                        ItemId: itemId,
                        Quantity: parseFloat($(this).find("td:eq(7)").find("input").val()),
                        ProductType: productType,
                        ItemName: $(this).find("td:eq(1)").text()
                    });

                    var serialCount = 0, rowSerial = 0;
                    var itemSerial = _.where(AddedSerialzableProduct, { ItemId: itemId });
                    serialCount = itemSerial.length;

                    for (rowSerial = 0; rowSerial < serialCount; rowSerial++) {
                        if (itemSerial[rowSerial].SalesItemSerialTransferId > 0)
                            DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(itemSerial[rowSerial])));


                        var srlItem = _.findWhere(AddedSerialzableProduct, { SerialNumber: itemSerial[rowSerial].SerialNumber });
                        var srlIndex = _.indexOf(AddedSerialzableProduct, srlItem);
                        AddedSerialzableProduct.splice(srlIndex, 1);
                    }
                }


                //else if (parseInt($(this).find("td:eq(14)").text(), 10) > 0) {


                //}
                //else if (($(this).find("td:eq(0)").find("input").is(":checked") == true) && ProductType == 'Serial Product') {
                //    SerialCount = SerialCount + 1;
                //}
            });

            var row = 0, rowCount = TransferItemNewlyAdded.length;

            for (row = 0; row < rowCount; row++) {
                if (TransferItemNewlyAdded[row].Quantity > TransferItemNewlyAdded[row].StockQuantity) {
                    toastr.warning("Transfer Quantity Cannot Greater Than Stock Quantity Of Product " + TransferItemNewlyAdded[row].ItemName);
                    break;
                }
            }

            if (row != rowCount) {
                return false;
            }

            row = 0;
            for (row = 0; row < rowCount; row++) {
                if (TransferItemNewlyAdded[row].ProductType == "Serial Product") {
                    var serialTotal = _.where(AddedSerialzableProduct, { ItemId: TransferItemNewlyAdded[row].ItemId });

                    if (TransferItemNewlyAdded[row].Quantity > serialTotal.length) {
                        toastr.warning("Please Give Serial Of Product " + TransferItemNewlyAdded[row].ItemName);
                        break;
                    }
                    else if (serialTotal.length > TransferItemNewlyAdded[row].Quantity) {
                        toastr.warning("Please Remove Serial Of Product " + TransferItemNewlyAdded[row].ItemName);
                        break;
                    }
                }
            }

            if (row != rowCount) {
                return false;
            }
            var QuotationId = $("#ContentPlaceHolder1_ddlQuotation").val();
            var SalesTransferId = $("#ContentPlaceHolder1_hfSalesTransferId").val();
            var CostCenterID = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
            var LocationID = $("#ContentPlaceHolder1_ddlLocationFrom").val();
            var SalesTransfer = {
                SalesTransferId: SalesTransferId,
                QuotationId: QuotationId,
                CostCenterId: CostCenterID,
                LocationID: LocationID
            }
            if (SerialCount > 0 && AddedSerialzableProduct.length == 0) {
                toastr.warning("You Have checked SerialzableProduct but not added the serial");
                return false;
            }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SalesItemTransfer.aspx/PerformSave',

                data: JSON.stringify({ SalesTransfer: SalesTransfer, TransferItemNewlyAdded: TransferItemNewlyAdded, TransferItemForEdit: TransferItemForEdit, TransferItemDeleted: TransferItemDeleted, AddedSerialzableProduct: AddedSerialzableProduct, DeletedSerialzableProduct: DeletedSerialzableProduct }),
                dataType: "json",
                success: function (data) {

                    SalesTransfer = null;
                    ClearAction();
                    GridPaging(1, 1);
                    $('#AddSalesTransfer').dialog('close');
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                },
                error: function (result) {

                    CommonHelper.AlertMessage(result.AlertMessage);
                }
            });

        }
        function ClearAction() {
            $("#ContentPlaceHolder1_btnSave").val("Save");
            $("#ContentPlaceHolder1_ddlQuotation").val("0").prop('disabled', false);
            $("#ContentPlaceHolder1_hfSalesTransferId").val("0");
            $("#ContentPlaceHolder1_ddlCostCenterFrom").val("0").prop('disabled', false);
            $("#ContentPlaceHolder1_ddlLocationFrom").val("0").prop('disabled', false);
            $("#ContentPlaceHolder1_hfLocationFromId").val("0");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("0");

            TransferItemAdded = new Array();
            NewAddedSerial = new Array();
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();
            AddedSerialCount = 0;
            TransferItemNewlyAdded = new Array();
            TransferItemForEdit = new Array();
            TransferItemDeleted = new Array();
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }

        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var QuotationId = 0;
            var gridRecordsCount = $("#SalesTransferTable tbody tr").length;
            QuotationId = $("#ContentPlaceHolder1_ddlQuotationForSearch").val();
            QuotationId = QuotationId == null ? 0 : QuotationId;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SalesItemTransfer.aspx/SearchSalesTransfer',

                data: "{'QuotationId':'" + QuotationId + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    LoadSearchTable(data);
                },
                error: function (result) {

                }
            });
        }
        function LoadSearchTable(data) {

            $("#SalesTransferTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;
            $.each(data.d.GridData, function (count, gridObject) {
                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:25%;'>" + gridObject.TransferNumber + "</td>";
                tr += "<td style='width:35%;'>" + gridObject.DealName + "</td>";
                tr += "<td style='width:25%;'>" + gridObject.QuotationNo + "</td>";
                tr += "<td style='width:15%;'>"
                //tr += "<td align='center' style=\"width:10%; cursor:pointer;\"><img src='../Images/detailsInfo.png' onClick= \"javascript:return PerformActionIntro('" + gridObject.MemberId + "','" + gridObject.Introducer_1_id + "','" + gridObject.Introducer_2_id + "' )\" alt='Details' border='0' /></td>";

                if (!(gridObject.IsApproved)) {
                    tr += "<a onclick=\"javascript:return LoadBySale('" + gridObject.SalesTransferId + "')\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                    tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteSalesItemTransfer(" + gridObject.SalesTransferId + ")'><img alt='Delete' src='../Images/delete.png' /></a>"
                    tr += " <a onclick=\"javascript:return LoadDetails('" + gridObject.SalesTransferId + "')\" title='Approve' href='javascript:void();'><img src='../Images/detailsInfo.png' alt='Approve'></a>";

                }
                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.SalesTransferId + "')\" alt='Invoice' title='Item Transfer Info' border='0' />";
                tr += "</td>";
                tr += "<td style='display:none'>" + gridObject.SalesTransferId + "</td>";
                tr += "<td style='display:none'>" + gridObject.QuotationId + "</td>";

                tr += "</tr>";

                $("#SalesTransferTable tbody").append(tr);

                tr = "";
                i++;
            });
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.NextButton);
            return false;
        }
        function ApproveSalesTransfer(SalesTransferId, ApprovedStatus, Remarks) {
            if (!confirm("Do you want to approve?")) {
                return false;
            }
            PageMethods.ApproveSalesTransfer(SalesTransferId, ApprovedStatus, Remarks, OnApprovalSucceed, OnApprovalFailed);
        }
        function OnApprovalSucceed(result) {
            $("ContentPlaceHolder1_hfSalesTransferId").val("0");
            CommonHelper.AlertMessage(result.AlertMessage);
            GridPaging(1, 1);
            $('#QuatationNSalesTransferDetails').dialog('close');
            return false;
        }
        function OnApprovalFailed() {

        }
        function ShowDetailsWindow() {
            var iframeid = 'frmPrint';
            var QuotationId = $("#ContentPlaceHolder1_ddlQuotation").val();
            if (QuotationId == null) {
                toastr.warning("Select Cost Center First");
                return false;
            }
            var url = "./SalesNoteEntry.aspx?qid=" + QuotationId + "&isInv=1";
            parent.document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Invoice Preview",
                show: 'slide'
                //,close: ClosePrintDialog
            });
            return false;
        }
        function ShowReport(SalesTransferId) {
            var iframeid = 'printDoc';
            var url = "./Reports/SalesTransferDetailsInvoice.aspx?id=" + SalesTransferId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Sales Item Transfer",
                show: 'slide'
            });
            return false;
        }
        function CheckAllOrder() {
            if ($("#OrderCheck").is(":checked")) {
                $("#SalesOrderItemTbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#SalesOrderItemTbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }

        function DeleteSalesItemTransfer(id) {
            PageMethods.DeleteSalesItemTransfer(id, OnDeleteSucceed, OnDeleteFailed);
        }
        function OnDeleteSucceed(result) {
            GridPaging(1, 1);
            CommonHelper.AlertMessage(result.AlertMessage);
        }
        function OnDeleteFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
        }
        function DetailsSalesTransferNApprove() {
            $("#QuatationNSalesTransferDetails").dialog({
                autoOpen: true,
                modal: true,
                width: '80%',
                maxWidth: '95%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Quatation and Sales Item Transfer Details",
                hide: 'fold',
                show: 'slide',
                close: function (event, ui) {
                    $("ContentPlaceHolder1_hfSalesTransferId").val("0");
                    $("#ContentPlaceHolder1_lblCompanyNameText").html("");
                    $("#ContentPlaceHolder1_lblQuotationNoText").html("");
                    $("#ContentPlaceHolder1_lblSalesTransferNoText").html("");
                    $("#ContentPlaceHolder1_lblDealNameText").html("");
                    $("#ContentPlaceHolder1_txtRemarks").val("");
                    $("#QuatationNSalesTransferDetailsTable tbody").empty();
                    $("#QuatationNSalesTransferDetailsTable tfoot").empty();
                }
            });
            CommonHelper.SpinnerClose();
            return false;
        }
        function Approve() {
            var SalesTransferId = $("#ContentPlaceHolder1_hfSalesTransferId").val();
            var Remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            if (Remarks.trim() == "") {
                toastr.warning("Add Remarks For Approve");
                return false;
            }
            ApproveSalesTransfer(SalesTransferId, 'Approved', Remarks.trim())
        }
        function LoadDetails(SalesTransferId) {

            $("#ContentPlaceHolder1_hfSalesTransferId").val(SalesTransferId);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SalesItemTransfer.aspx/GetDetailsForApproval',

                data: "{'SalesTransferId':'" + SalesTransferId + "'}",
                dataType: "json",
                success: function (data) {
                    LoadDetailsTable(data);
                },
                error: function (result) {
                    $("ContentPlaceHolder1_hfSalesTransferId").val("0");
                }
            });
        }
        function LoadDetailsTable(data) {
            DetailsSalesTransferNApprove();
            $("#ContentPlaceHolder1_lblCompanyNameText").html(data.d.Company);
            $("#ContentPlaceHolder1_lblQuotationNoText").html(data.d.QuotationNo);
            $("#ContentPlaceHolder1_lblSalesTransferNoText").html(data.d.TransferNumber);
            $("#ContentPlaceHolder1_lblDealNameText").html(data.d.DealName);
            $("#QuatationNSalesTransferDetailsTable tbody").empty();
            var i = 0;
            var totalPrice = 0;
            var totalQuotationQty = 0;
            var totalDeliverQty = 0;
            $.each(data.d.SMQuotationDetailsBOList, function (count, gridObject) {
                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:20%;'>" + gridObject.ItemName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.HeadName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.Quantity + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.SalesQuantity + "</td>";
                tr += "<td style='width:15%;text-align: right;'>" + parseFloat(gridObject.UnitPrice) + "</td>";
                tr += "<td style='width:20%;text-align: right;'>" + parseFloat(gridObject.SalesQuantity * gridObject.UnitPrice) + "</td>";
                tr += "</tr>";

                $("#QuatationNSalesTransferDetailsTable tbody").append(tr);

                tr = "";
                i++;
                totalPrice += gridObject.SalesQuantity * gridObject.UnitPrice;
                totalQuotationQty += gridObject.Quantity;
                totalDeliverQty += gridObject.SalesQuantity
            });
            $("#QuatationNSalesTransferDetailsTable tfoot").empty();
            tr = "<tr >";
            //tr += "<td style='width:80%;'></td>";
            tr += "<td colspan=\"2\"> Total</td>";
            tr += "<td style='width:15%;'>" + totalQuotationQty + "</td>";
            tr += "<td style='width:15%;'>" + totalDeliverQty + "</td>";
            tr += "<td colspan=\"1\"></td>";
            tr += "<td style='width:20%;text-align: right;'>" + parseFloat(totalPrice) + "</td>";
            $("#QuatationNSalesTransferDetailsTable tfoot").append(tr);
            tr += "</tr>";
            tr = "";
            return false;
        }
        function CleanDetails() {
            $("#ContentPlaceHolder1_lblCompanyNameText").html("");
            $("#ContentPlaceHolder1_lblQuotationNoText").html("");
            $("#ContentPlaceHolder1_lblSalesTransferNoText").html("");
            $("#ContentPlaceHolder1_lblDealNameText").html("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#QuatationNSalesTransferDetailsTable tbody").remove();
            $("#QuatationNSalesTransferDetailsTable tfoot").remove();
        }
    </script>
    <asp:HiddenField ID="hfLocationFromId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSalesTransferId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemIdForSerial" runat="server" Value="0" />
    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="1000" height="800" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>
    <div id="AddSalesTransfer" style="display: none;">
        <div id="AddPanel" class="panel panel-default">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCostCenter" runat="server" class="control-label required-field" Text="Cost Center"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCostCenterFrom" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblLocation" runat="server" class="control-label required-field" Text="Location"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlLocationFrom" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblQuotation" runat="server" class="control-label required-field" Text="Quotation"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlQuotation" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <asp:Button ID="btnDetails" runat="server" Text="Details" OnClientClick="javascript:return ShowDetailsWindow();"
                                CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="7" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">Item Info</div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="table-responsive" id="SalesOrderItemContainer" style="height: 300px; overflow-y: scroll;">
                        <table id="SalesOrderItemTbl" class="table table-bordered table-condensed table-hover">
                            <thead>
                                <tr>
                                    <th style="width: 5%; text-align: center;">Select
                                            <input type="checkbox" value="" checked="checked" id="OrderCheck" onclick="CheckAllOrder()" />
                                    </th>
                                    <th style="width: 20%;">Item Name</th>
                                    <th style="width: 15%;">Stock By</th>
                                    <th style="width: 10%;">Stock Qty</th>
                                    <th style="width: 10%;">Already Delivered</th>
                                    <th style="width: 10%">Quotation Qty</th>
                                    <th style="width: 10%;">Max Deliver Qty</th>
                                    <th style="width: 15%">Deliver Qty</th>
                                    <th style="width: 5%">Action</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="btnSave" runat="server" Text="Save and Continue" OnClientClick="javascript:return PerformSave();"
                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="7" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                    OnClientClick="javascript: return ClearAction();" TabIndex="8" />
            </div>
        </div>
    </div>
    <div id="SerialWindow" style="display: none;">
        <div class="panel panel-default">
            <div class="panel-body" style="padding: 4px;">
                <div class="form-horizontal">
                    <div id="labelAndtxtSerialAutoComplete" class="row" style="display: none;">
                        <div class="col-md-2">
                            <label class="control-label">Serial</label>
                        </div>
                        <div class="col-md-10">
                            <input type="text" id="txtSerialAutoComplete" class="form-control" />
                        </div>
                    </div>

                    <div id="labelAndtxtSerial" class="row">
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
                    <div id="labelAndtxtSerialAddButtonAndClear" class="row">
                        <div class="col-md-12">
                            <input type="button" class="btn btn-primary" style="padding-left: 30px; padding-right: 30px;" value="Add" onclick="CheckAndAddedSerialWiseProduct()" />
                            <input type="button" class="btn btn-primary" value="Clear" onclick="ClearSerialWithConfirmation()" />
                        </div>
                    </div>
                    <hr />
                </div>

                <div class="table-responsive" style="height: 350px; overflow-y: scroll;">
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
    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Sales Transfer Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblQuotationForSearch" runat="server" class="control-label" Text="Quotation No"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlQuotationForSearch" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="javascript: return GridPaging(1,1);"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                            TabIndex="6" />
                        <asp:Button ID="btnAdd" runat="server" Text="New Sales Transfer" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return AddNewSalesItemTransfer();" TabIndex="6" />
                    </div>
                </div>
            </div>
        </div>
        <div id="SearchPanel" class="panel panel-default">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div class="table-responsive form-group" id="SalesTransferContainer" style="overflow: scroll;">
                    <table class="table table-bordered table-condensed table-responsive" id="SalesTransferTable"
                        style="width: 100%;">
                        <thead>
                            <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                <th style="width: 25%;">Sales Number
                                </th>
                                <th style="width: 35%;">Deal Name
                                </th>
                                <th style="width: 25%;">Quotation No
                                </th>
                                <th style="width: 15%;">Action
                                </th>
                                <th style="display: none">SalesTransferId
                                </th>
                                <th style="display: none">QuotationId
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
    <div id="QuatationNSalesTransferDetails" style="display: none;">
        <div class="panel panel-default">
            <div class="panel-body" style="padding: 4px;">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCompanyName" runat="server" class="control-label" Text="Company Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblCompanyNameText" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblDealName" runat="server" class="control-label" Text="Deal Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblDealNameText" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblQuotationNo" runat="server" class="control-label" Text="Quotation No"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblQuotationNoText" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblSalesTransferNo" runat="server" class="control-label" Text="Transfer No"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblSalesTransferNoText" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="table-responsive form-group" id="QuatationNSalesTransferDetailsContainer">
                    <table class="table table-bordered table-condensed table-responsive" id="QuatationNSalesTransferDetailsTable"
                        style="width: 100%;">
                        <thead>
                            <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                <th style="width: 20%;">Item Name</th>
                                <th style="width: 15%;">Stock By</th>
                                <th style="width: 15%">Quotation Qty</th>
                                <th style="width: 15%">Deliver Qty</th>
                                <th style="width: 15%; text-align: right;">Unit Price</th>
                                <th style="width: 20%; text-align: right;">Total Price</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                        <tfoot>
                        </tfoot>
                    </table>
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label required-field" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox class="span6" Style="resize: none;" ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClientClick="javascript: return Approve();"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />

                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
