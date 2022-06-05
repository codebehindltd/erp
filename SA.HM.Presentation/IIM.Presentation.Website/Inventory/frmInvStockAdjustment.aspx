<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmInvStockAdjustment.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmInvStockAdjustment" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var AdjustmentItemEdited = "";
        var DeletedAdjustItem = [];
        var itemArry = null;
        var NewAddedSerial = new Array();
        var AddedSerialzableProduct = new Array();
        var DeletedSerialzableProduct = new Array();
        var serialNumber = "";

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Stock Adjustment</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            //$($("#myTabs").find("li")[1]).hide();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val() == "1") {
                FillForm($("#ContentPlaceHolder1_hfStockAdjustmentId").val());
            }
            IsSerialAutoField = $('#ContentPlaceHolder1_hfIsItemSerialFillWithAutoSearch').val() == '1' ? true : false;
            if (IsSerialAutoField) {
                $('#labelAndtxtSerialAddButtonAndClear').hide();
                $('#labelAndtxtSerial').hide();
                $('#labelAndtxtSerialAutoComplete').show();
            } else {
                $('#labelAndtxtSerialAddButtonAndClear').show();
                $('#labelAndtxtSerial').show();
                $('#labelAndtxtSerialAutoComplete').hide();

            }

            $("#myTabs").tabs();

            $("#btnAddAdjustment").click(function () {

                var locationId = $("#<%=ddlLocation.ClientID %>").val();
                var itemName = $("#<%=txtItemName.ClientID %>").val();
                var stockById = $("#<%=ddlStockBy.ClientID %>").val();
                var transactionModeId = $("#<%=ddlInvTransactionMode.ClientID %>").val();
                var quantity = $("#<%=txtAdjustQuantity.ClientID %>").val();
                var value = $("#<%=txtAdjustmentValue.ClientID %>").val();
                var reason = $("#<%=ddlAdjustmentReason.ClientID %> option:selected").text();
                var stockAdjustmentDetailsId = "0", isEdited = "0";

                var itemId = $("#ContentPlaceHolder1_hfItemId").val();
                var locationName = $("#<%=ddlLocation.ClientID %> option:selected").text();
                var stockBy = $("#<%=ddlStockBy.ClientID %> option:selected").text();
                var transactionMode = $("#<%=ddlInvTransactionMode.ClientID %> option:selected").text();

                var dbQuantity = 0;
                if (itemArry != null) { dbQuantity = itemArry.StockQuantity };

                if (AdjustmentItemEdited != "")
                    stockAdjustmentDetailsId = $(AdjustmentItemEdited).find("td:eq(8)").text();

                if (AdjustmentItemEdited != "" && stockAdjustmentDetailsId != "0") {
                    isEdited = "1";
                    EditItem(itemName, locationName, stockBy, dbQuantity, quantity, value, transactionMode, reason, stockAdjustmentDetailsId, itemId, locationId, stockById, transactionModeId, isEdited);
                    PerformClearActionAfterAdd();

                    return;
                }
                else if (AdjustmentItemEdited != "" && stockAdjustmentDetailsId == "0") {
                    isEdited = "0";
                    EditItem(itemName, locationName, stockBy, dbQuantity, quantity, value, transactionMode, reason, stockAdjustmentDetailsId, itemId, locationId, stockById, transactionModeId, isEdited);
                    PerformClearActionAfterAdd();

                    return;
                }

                AddItem(itemName, locationName, stockBy, dbQuantity, quantity, value, transactionMode, reason, stockAdjustmentDetailsId, itemId, locationId, stockById, transactionModeId, isEdited);
                PerformClearActionAfterAdd();
            });

            $("#txtItemName").blur(function () {
                if ($("#ContentPlaceHolder1_ddlStockBy").val() != 0 && $("#ContentPlaceHolder1_txtAdjustQuantity").val() != "") {
                    ItemCost();
                }
            });

            $("#ContentPlaceHolder1_ddlStockBy").change(function () {
                if ($("#ContentPlaceHolder1_ddlStockBy").val() != 0 && $("#ContentPlaceHolder1_txtAdjustQuantity").val() != "") {
                    ItemCost();
                }
            });

            $("#ContentPlaceHolder1_txtAdjustQuantity").blur(function () {
                if ($("#ContentPlaceHolder1_ddlStockBy").val() != 0 && $("#ContentPlaceHolder1_txtAdjustQuantity").val() != "") {
                    ItemCost();
                }
            });

            $("#txtItemName").autocomplete({
                source: function (request, response) {
                    var costCenterId = 0;
                    var locationId = $("#ContentPlaceHolder1_ddlLocation").val();

                    //                    if (costCenterId == "0") {
                    //                        toastr.warning("Please Select a Cost Center.");
                    //                        return false;
                    //                    }
                    //                    else 
                    if (locationId == "0") {
                        toastr.warning("Please Select a Location");
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmInvStockAdjustment.aspx/ItemNCategoryAutoSearch',
                        data: "{'itemName':'" + request.term + "','" + "costCenterId':'" + costCenterId + "','" + "locationId':'" + locationId + "'}",
                        dataType: "json",
                        async: false,
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    StockQuantity: m.StockQuantity
                                };
                            });
                            response(searchData);

                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    event.preventDefault();
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);

                    itemArry = ui.item;

                    $("#<%=hfItemId.ClientID %>").val(ui.item.value);
                }

            });

            $("#ContentPlaceHolder1_txtAdjustQuantity").change(function () {
                var adjustmentQuantity = $("#ContentPlaceHolder1_txtAdjustQuantity").val();
            });
            $("#txtSerialAutoComplete").autocomplete({
                minLength: 1,
                source: function (request, response) {



                    var itemId = $("#ContentPlaceHolder1_hfItemIdForSerial").val();


                    var locationId = $("#ContentPlaceHolder1_ddlStoreLocation").val();


                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmInvStockAdjustment.aspx/SerialSearch',
                        data: JSON.stringify({ serialNumber: request.term, locationId: locationId, itemId: itemId }),
                        dataType: "json",
                        async: false,
                        success: function (data) {


                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.SerialNumber,
                                    value: m.SerialNumber
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field

                    ItemSelected = ui.item;
                    serialNumber = ui.item.value;
                    AddSerialNumber();
                    $("#txtSerialAutoComplete").val("");
                }
            }).autocomplete("option", "appendTo", "#SerialWindow");


        });
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
            $("#txtSerialAutoComplete").val("");
        }
        function DeleteItemSerial(control) {

            var tr = $(control).parent().parent();

            var serialNumber = $(tr).find("td:eq(0)").text();
            var itemId = parseInt($(tr).find("td:eq(2)").text(), 10);
            var outSerialId = parseInt($(tr).find("td:eq(3)").text(), 10);

            var addedQuantity = $("#lblAddedQuantity").text();
            AddedSerialCount = parseInt(addedQuantity, 10) - 1;

            var item = _.findWhere(AddedSerialzableProduct, { SerialNumber: serialNumber });
            var index = _.indexOf(AddedSerialzableProduct, item);
            AddedSerialzableProduct.splice(index, 1);

            var itemNew = _.findWhere(NewAddedSerial, { SerialNumber: serialNumber });
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
        function ApplySerialForStockAdjustment() {
            var addedQuantity = $("#lblAddedQuantity").text();
            var totalQuantity = $("#lblItemQuantity").text();

            if (parseInt(addedQuantity, 10) < parseInt(totalQuantity, 10)) {
                toastr.warning("Number Of Serial Must Added As Equall Item Quantity.");
                return false;
            }

            $(NewAddedSerial).each(function (index, obj) {

                AddedSerialzableProduct.push({
                    StockAdjustmentId: obj.OutSerialId,
                    ItemId: obj.ItemId,
                    SerialNumber: obj.SerialNumber
                });
            });

            $("#SerialWindow").dialog("close");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("");
            AddedSerialCount = 0;
            NewAddedSerial = new Array();
        }
        function ItemCost() {
            var itemId = $("#ContentPlaceHolder1_hfItemId").val();
            var stockById = $("#<%=ddlStockBy.ClientID %>").val();
            var quantity = $("#<%=txtAdjustQuantity.ClientID %>").val();

            GetItemCost(itemId, stockById, quantity);
        }

        function AddItem(itemName, locationName, stockBy, dbQuantity, quantity, value, transactionMode, reason, stockAdjustmentDetailsId, itemId, locationId, stockById, transactionModeId, isEdited) {

            var tr = "", rowCount = $("#ProductAdjustmentGrid tbody tr").length;

            if (rowCount % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:20%;'>" + itemName + "</td>";
            tr += "<td style='width:12%;'>" + locationName + "</td>";
            tr += "<td style='width:10%;'>" + stockBy + "</td>";
            tr += "<td style='width:10%;'>" + quantity + "</td>";
            tr += "<td style='width:10%;'>" + value + "</td>";
            tr += "<td style='width:12%;'>" + transactionMode + "</td>";
            tr += "<td style='width:18%;'>" + reason + "</td>";

            tr += "<td style='width:7%;'> <a onclick=\"javascript:return FIllForEdit(this);\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
            tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'javascript:return DeleteItemDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            tr += "<td style='display:none'>" + stockAdjustmentDetailsId + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + locationId + "</td>";
            tr += "<td style='display:none'>" + stockById + "</td>";
            tr += "<td style='display:none'>" + transactionModeId + "</td>";
            tr += "<td style='display:none'>" + dbQuantity + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";

            $("#ProductAdjustmentGrid tbody").append(tr);
        }

        function EditItem(itemName, locationName, stockBy, dbQuantity, quantity, value, transactionMode, reason, stockAdjustmentDetailsId, itemId, locationId, stockById, transactionModeId, isEdited) {

            var tr = AdjustmentItemEdited;

            $(tr).find("td:eq(0)").text(itemName);
            $(tr).find("td:eq(1)").text(locationName);
            $(tr).find("td:eq(2)").text(stockBy);
            $(tr).find("td:eq(3)").text(quantity);
            $(tr).find("td:eq(4)").text(value);
            $(tr).find("td:eq(5)").text(transactionMode);
            $(tr).find("td:eq(6)").text(reason);

            $(tr).find("td:eq(8)").text(stockAdjustmentDetailsId);
            $(tr).find("td:eq(9)").text(itemId);
            $(tr).find("td:eq(10)").text(locationId);
            $(tr).find("td:eq(11)").text(stockById);
            $(tr).find("td:eq(12)").text(transactionModeId);
            $(tr).find("td:eq(13)").text(dbQuantity);
            $(tr).find("td:eq(14)").text(isEdited);

            AdjustmentItemEdited = "";
        }

        function FIllForEdit(editedRow) {

            var tr = $(editedRow).parent().parent();
            AdjustmentItemEdited = tr;

            var costCenterId = "0", itemId = "0", stockById = "0", previousQuantity = 0.0, itemName = "", reasonId = 0;
            var quantity = "0", stockAdjustmentDetailsId = "0", stockQuantity = 0.0, Reason = '', quantityValue = 0;
            var isEdit = "0", stockAdjustmentId = "0", locationId = "0", tModeId = "0", adjustmentCost = 0.0;

            stockAdjustmentDetailsId = $.trim($(tr).find("td:eq(8)").text());
            isEdit = $.trim($(tr).find("td:eq(11)").text());

            itemName = $.trim($(tr).find("td:eq(0)").text());

            itemId = $.trim($(tr).find("td:eq(9)").text());
            locationId = $(tr).find("td:eq(10)").text();
            stockById = $(tr).find("td:eq(11)").text();
            tModeId = $(tr).find("td:eq(12)").text();
            Reason = $(tr).find("td:eq(6)").text();

            reasonId = $("#ContentPlaceHolder1_ddlAdjustmentReason").find("option:contains('" + Reason + "')").val();

            stockQuantity = "0.0";

            quantity = $(tr).find("td:eq(3)").text();
            quantityValue = $(tr).find("td:eq(4)").text();

            $("#ContentPlaceHolder1_hfItemId").val(itemId);

            $("#ContentPlaceHolder1_ddlLocation").val(locationId);
            $("#txtItemName").val(itemName);
            $("#ContentPlaceHolder1_ddlStockBy").val(stockById);
            $("#ContentPlaceHolder1_ddlInvTransactionMode").val(tModeId);
            $("#ContentPlaceHolder1_txtAdjustQuantity").val(quantity);
            $("#ContentPlaceHolder1_txtAdjustmentValue").val(quantityValue);
            $("#ContentPlaceHolder1_ddlAdjustmentReason").val(reasonId);
        }

        function DeleteItemDetails(deleteItem) {
            var tr = $(deleteItem).parent().parent();
            var stockAdjustmentDetailsId = $.trim($(tr).find("td:eq(8)").text());
            var stockAdjustmentId = $("#ContentPlaceHolder1_hfStockAdjustmentId").val();

            DeletedAdjustItem.push({
                StockAdjustmentDetailsId: parseInt(stockAdjustmentDetailsId, 10),
                StockAdjustmentId: parseInt(stockAdjustmentId, 10)
            });

            $(deleteItem).parent().parent().remove();
        }

        function ValidationBeforeSave() {

            var rowCount = $('#ProductAdjustmentGrid tbody tr').length;
            if (rowCount == 0) {
                toastr.warning('Add atleast one Product.');
                return false;
            }

            var costCenterId = "0", itemId = "0", stockById = "0", previousQuantity = 0.0;
            var quantity = "0", stockAdjustmentDetailsId = "0", stockQuantity = 0.0, Reason = '', quantityValue = 0;
            var isEdit = "0", stockAdjustmentId = "0", locationId = "0", tModeId = "0", adjustmentCost = 0.0;

            costCenterId = 0;
            stockAdjustmentId = $("#ContentPlaceHolder1_hfStockAdjustmentId").val();

            var ItemStockAdjustment = {
                StockAdjustmentId: stockAdjustmentId,
                CostCenterId: costCenterId
            };

            var StockAdjustmentDetails = [], StockAdjustmentDetailsEdit = [];

            $("#ProductAdjustmentGrid tbody tr").each(function (index, item) {

                stockAdjustmentDetailsId = $.trim($(item).find("td:eq(8)").text());
                isEdit = $.trim($(item).find("td:eq(14)").text());

                itemId = $.trim($(item).find("td:eq(9)").text());
                locationId = $(item).find("td:eq(10)").text();
                stockById = $(item).find("td:eq(11)").text();
                tModeId = $(item).find("td:eq(12)").text();
                Reason = $(item).find("td:eq(6)").text();

                stockQuantity = $(item).find("td:eq(13)").text();
                quantity = $(item).find("td:eq(3)").text();
                quantityValue = $(item).find("td:eq(4)").text();

                if (parseFloat(quantity) != parseFloat(previousQuantity)) {
                    if (stockAdjustmentDetailsId == "0") {

                        stockQuantity = "0.0"; //parseFloat($(item).find("td:eq(2)").text());

                        StockAdjustmentDetails.push({
                            StockAdjustmentDetailsId: parseInt(stockAdjustmentDetailsId, 10),
                            StockAdjustmentId: parseInt(stockAdjustmentId, 10),
                            ItemId: parseInt(itemId, 10),
                            LocationId: parseInt(locationId, 10),
                            StockById: parseInt(stockById, 10),
                            PreviousQuantity: parseFloat(stockQuantity),
                            AdjustmentQuantity: parseFloat(quantity),
                            TModeId: parseInt(tModeId, 10),
                            //AdjustmentCost: parseFloat(quantityValue),
                            AdjustmentCost: 0,
                            Reason: Reason
                        });
                    }
                    else if (stockAdjustmentDetailsId != "0" && isEdit == "1") {
                        StockAdjustmentDetailsEdit.push({
                            StockAdjustmentDetailsId: parseInt(stockAdjustmentDetailsId, 10),
                            StockAdjustmentId: parseInt(stockAdjustmentId, 10),
                            ItemId: parseInt(itemId, 10),
                            LocationId: parseInt(locationId, 10),
                            StockById: parseInt(stockById, 10),
                            PreviousQuantity: parseFloat(stockQuantity),
                            AdjustmentQuantity: parseFloat(quantity),
                            TModeId: parseInt(tModeId, 10),
                            //AdjustmentCost: parseFloat(quantityValue),
                            AdjustmentCost: 0,
                            Reason: Reason
                        });
                    }
                }
            });
            PageMethods.SaveStockAdjustment(ItemStockAdjustment, StockAdjustmentDetails, StockAdjustmentDetailsEdit, DeletedAdjustItem, OnSaveStockAdjustmentSucceeded, OnSaveStockAdjustmentFailed);
            return false;
        }
        function OnSaveStockAdjustmentSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            return false;
        }
        function OnSaveStockAdjustmentFailed(error) { CommonHelper.SpinnerClose(); toastr.error(error.get_message()); }

        function AdjustmentProductDetails(stockAdjustmentId) {
            CommonHelper.SpinnerOpen();
            PageMethods.GetAdjustmentProductDetails(stockAdjustmentId, OnAdjustmentProductLoadSucceeded, OnSaveAdjustmentProductFailed);
            return false;
        }
        function OnAdjustmentProductLoadSucceeded(result) {

            $("#ItemAdjustmentGridDetails tbody").html("");
            var totalRow = result.length, row = 0;

            var tr = "";

            for (row = 0; row < totalRow; row++) {

                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                // StockAdjustmentDetailsId	StockAdjustmentId	ItemId	LocationId	StockById	OpeningQuantity	ReceiveQuantity	
                // ActualUsage	WastageQuantity	StockQuantity	ActualQuantity	ItemName	CategoryName	LocationName	StockByName

                tr += "<td style='width:15%;'>" + result[row].ItemName + "</td>";
                tr += "<td style='width:13%;'>" + result[row].StockByName + "</td>";
                tr += "<td style='width:10%;'>" + result[row].OpeningQuantity + "</td>";
                tr += "<td style='width:10%;'>" + result[row].ReceiveQuantity + "</td>";
                tr += "<td style='width:10%;'>" + result[row].ActualUsage + "</td>";
                tr += "<td style='width:10%;'>" + result[row].WastageQuantity + "</td>";
                tr += "<td style='width:13%;'>" + result[row].StockQuantity + "</td>";
                tr += "<td style='width:12%;'>" + result[row].ActualQuantity + "</td>";

                tr += "</tr>";

                $("#ItemAdjustmentGridDetails tbody").append(tr);
                tr = "";
            }

            CommonHelper.SpinnerClose();

            $("#DetailsProductAdjustmentGridContainer").dialog({
                autoOpen: true,
                modal: true,
                width: 800,
                height: 550,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Adjustment Product Details",
                show: 'slide'
            });
        }
        function OnSaveAdjustmentProductFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function FillForm(stockAdjustmentId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }

            PageMethods.FIllForm(stockAdjustmentId, OnFillFormSucceeded, OnFillFormFailed);
            return false;
        }

        function OnFillFormSucceeded(result) {

            if (result.AdjustmentItem != null) {
                $("#ContentPlaceHolder1_hfStockAdjustmentId").val(result.AdjustmentItem.StockAdjustmentId);
            }

            LoadLocationByCostCenter(result.AdjustmentItem.CostCenterId);

            var rowLength = 0, row = 0, isEdited = "0";
            rowLength = result.AdjustmentItemDetails.length;

            if (rowLength > 0) {

                for (row = 0; row < rowLength; row++) {

                    AddItem(result.AdjustmentItemDetails[row].ItemName, result.AdjustmentItemDetails[row].LocationName, result.AdjustmentItemDetails[row].StockByName,
                        result.AdjustmentItemDetails[row].PreviousQuantity, result.AdjustmentItemDetails[row].AdjustmentQuantity, result.AdjustmentItemDetails[row].AdjustmentCost,
                        result.AdjustmentItemDetails[row].TransactionMode, result.AdjustmentItemDetails[row].Reason, result.AdjustmentItemDetails[row].StockAdjustmentDetailsId,
                        result.AdjustmentItemDetails[row].ItemId, result.AdjustmentItemDetails[row].LocationId, result.AdjustmentItemDetails[row].StockById,
                        result.AdjustmentItemDetails[row].TModeId, isEdited)
                }
            }

            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });
        }

        function LoadLocationByCostCenter(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationSucceeded, OnLoadLocationFailed);
        }

        function OnLoadLocationSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlLocation');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].LocationId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            control.val($("#ContentPlaceHolder1_hfLocationId").val());
            return false;
        }

        function LoadLocationByCostCenter2(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationSucceeded2, OnLoadLocationFailed);
        }

        function OnLoadLocationSucceeded2(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlStoreLocation');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].LocationId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            control.val($("#ContentPlaceHolder1_hfLocationId").val());
            return false;
        }
        function OnLoadLocationFailed() { }

        function OnFillFormFailed(error) { }

        function GetItemCost(itemId, stockById, quantity) {

            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmInvProductAdjustment.aspx/GetReceipeItemCost',
                data: "{'itemId':'" + itemId + "','stockById':'" + stockById + "','quantity':'" + quantity + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d != null) {
                        $("#ContentPlaceHolder1_txtAdjustmentValue").val(data.d);
                    }
                    else {
                        $("#ContentPlaceHolder1_txtAdjustmentValue").val("0");
                    }
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }

        function PerformClearActionAfterAdd() {
            $("#btnAddAdjustment").val("Save");
            $("#ContentPlaceHolder1_ddlLocation").val("0");
            $("#txtItemName").val("");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_ddlInvTransactionMode").val("0");
            $("#ContentPlaceHolder1_txtAdjustQuantity").val("");
            $("#ContentPlaceHolder1_txtAdjustmentValue").val("");
            $("#ContentPlaceHolder1_ddlAdjustmentReason").val("");

            $("#ContentPlaceHolder1_hfItemId").val("0");

            itemArry = null;
        }

        function PerformClearAction() {

            $("#ProductAdjustmentGrid tbody").html("");
            $("#ContentPlaceHolder1_btnSave").val("Save");

            $("#ContentPlaceHolder1_ddlLocation").val("0");
            $("#txtItemName").val("");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_ddlInvTransactionMode").val("0");
            $("#ContentPlaceHolder1_txtAdjustQuantity").val("");
            $("#ContentPlaceHolder1_txtAdjustmentValue").val("");
            $("#ContentPlaceHolder1_ddlAdjustmentReason").val("");

            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_hfStockAdjustmentId").val("0");
            $("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val("0");
            return false;
        }

        //-------------------Item Variance Template 2 ------------------------------------

        $(document).ready(function () {

            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlStoreLocation").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCostCentre").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlItem").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCostCentre").change(function () {
                var costCenetrId = $("#ContentPlaceHolder1_ddlCostCentre").val();
                if (costCenetrId == "0") {
                    toastr.info("Please select cost center");
                    return;
                }

                LoadLocationByCostCenter(costCenetrId);
                return false;
            });

            $("#ContentPlaceHolder1_ddlStoreLocation").change(function () {

                if ($(this).val() == "0")
                    return;

                var costCenterId = 0;
                var locationId = $(this).val();
                var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                var adjustmentFrequence = $("#ContentPlaceHolder1_ddlAdjustmentFrequency").val();
                var itemId = $("#ContentPlaceHolder1_ddlItem").val();

                if (categoryId == "0") {
                    toastr.info("Please Select Category.");
                    $("#AdjustmentItemGridContainer").hide();
                    return false;
                }
                else if (adjustmentFrequence == "0") {
                    toastr.info("Please Select Adjustment Frequency.");
                    return;
                }

                $("#AdjustmentItemGridContainer").show();

                CommonHelper.SpinnerOpen();
                //PageMethods.CostcenterLocationWiseItemStock(costCenterId, locationId, categoryId, adjustmentFrequence, itemId, OnLoadLocationWiseItemStockSucceed, OnLoadLocationWiseItemStockFailed);
                $("#ContentPlaceHolder1_ddlFiletrBy").trigger('change');
            });

            $("#ContentPlaceHolder1_ddlAdjustmentFrequency").change(function () {

                if ($(this).val() == "0")
                    return;

                var costCenterId = 0;
                var locationId = $("#ContentPlaceHolder1_ddlStoreLocation").val();
                var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                var filterBy = $("#ContentPlaceHolder1_ddlFiletrBy").val();

                if (locationId == "0") {
                    toastr.info("Please Select Location.");
                    return false;
                }
                else if (categoryId == "0") {
                    $("#AdjustmentItemGridContainer").hide();
                    toastr.info("Please Select Category.");
                    return false;
                }

                $("#AdjustmentItemGridContainer").show();

                var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

                var adjustmentFrequence = $(this).val();

                CommonHelper.SpinnerOpen();
                PageMethods.LoadItems(companyId, projectId, costCenterId, locationId, categoryId, adjustmentFrequence, OnLoadItemSucceed, OnLoadItemFailed);

            });


            function OnLoadItemSucceed(result) {
                if (result != null) {
                    var list = result;
                    var control = $('#ContentPlaceHolder1_ddlItem');

                    control.empty();
                    if (list.length > 0) {

                        control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                        for (i = 0; i < list.length; i++) {
                            control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].ItemName + '</option>');
                        }
                    }
                    else {
                        control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    }
                    $('#ContentPlaceHolder1_ddlItem').trigger('change');
                    CommonHelper.SpinnerClose();
                    return false;
                }
            }
            function OnLoadItemFailed() { CommonHelper.SpinnerClose(); }

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").change(function () {

                var projectId = $(this).val();

                if (projectId == "0") {
                    $("#AdjustmentItemGridContainer").hide();
                    return;
                }
                $("#AdjustmentItemGridContainer").show();


                $("#ContentPlaceHolder1_ddlFiletrBy").trigger('change');
            });

            $("#ContentPlaceHolder1_ddlCategory").change(function () {

                var categoryId = $(this).val();

                if (categoryId == "0") {
                    $("#AdjustmentItemGridContainer").hide();
                    return;
                }

                var costCenterId = 0;
                var locationId = $("#ContentPlaceHolder1_ddlStoreLocation").val();
                var adjustmentFrequence = $("#ContentPlaceHolder1_ddlAdjustmentFrequency").val();
                var itemId = $("#ContentPlaceHolder1_ddlItem").val();

                if (locationId == "0") {
                    toastr.info("Please Select Location.");
                    return;
                }
                else if (adjustmentFrequence == "0") {
                    toastr.info("Please Select Adjustment Frequency.");
                    return;
                }

                $("#AdjustmentItemGridContainer").show();


                $("#ContentPlaceHolder1_ddlFiletrBy").trigger('change');
            });

            $("#ContentPlaceHolder1_ddlItem").change(function () {
                var costCenterId = 0;
                var locationId = $("#ContentPlaceHolder1_ddlStoreLocation").val();
                var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                var adjustmentFrequence = $("#ContentPlaceHolder1_ddlAdjustmentFrequency").val();
                var itemId = $(this).val();

                var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

                CommonHelper.SpinnerOpen();
                PageMethods.CostcenterLocationWiseItemStock(companyId, projectId, costCenterId, locationId, categoryId, adjustmentFrequence, itemId, OnLoadLocationWiseItemStockSucceed, OnLoadLocationWiseItemStockFailed);
            });


            $("#ContentPlaceHolder1_ddlFiletrBy").change(function () {
                if ($(this).val() == "1") {
                    $("#lblItem").hide();
                    $("#dvItem").hide();

                    var costCenterId = 0;
                    var locationId = $("#ContentPlaceHolder1_ddlStoreLocation").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    var adjustmentFrequence = $("#ContentPlaceHolder1_ddlAdjustmentFrequency").val();
                    var itemId = 0;

                    var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                    var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

                    $("#AdjustmentItemGridContainer").show();

                    CommonHelper.SpinnerOpen();

                    PageMethods.CostcenterLocationWiseItemStock(companyId, projectId, costCenterId, locationId, categoryId, adjustmentFrequence, itemId, OnLoadLocationWiseItemStockSucceed, OnLoadLocationWiseItemStockFailed);
                }
                else if ($(this).val() == "2") {
                    $("#lblItem").show();
                    $("#dvItem").show();
                }


                $("#ContentPlaceHolder1_ddlAdjustmentFrequency").trigger('change');
            });


        });

        function OnLoadLocationWiseItemStockSucceed(result) {
            if (result != null) {
                $("#AdjustmentItemGridContainer").html(result.AdjustmentItemDetailsGrid);

                if (result.AdjustmentItem != null)
                    $("#ContentPlaceHolder1_hfStockAdjustmentId").val(result.AdjustmentItem.StockAdjustmentId);
            }

            CommonHelper.SpinnerClose();
        }
        function OnLoadLocationWiseItemStockFailed() { CommonHelper.SpinnerClose(); }

        function LoadLocationByCostCenter(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationSucceeded, OnLoadLocationFailed);
        }

        function OnLoadLocationSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlStoreLocation');
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    if (list.length > 1)
                        control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].LocationId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            //control.val($("#ContentPlaceHolder1_hfLocationId").val());
            return false;
        }
        function OnLoadLocationFailed() { }

        function ValidationBeforeSave2() {

            var IsPermitForSave = confirm("Do You Want To Save?");
            if (IsPermitForSave == true) {

                CommonHelper.SpinnerOpen();

                var rowCount = $('#ItemAdjustmentGrid tbody tr').length;
                if (rowCount == 0) {
                    toastr.warning('No Product Found.');
                    CommonHelper.SpinnerClose();
                    return false;
                }

                var adjustmentFrequence = '', costCenterId = "0", itemId = "0", stockById = "0", previousQuantity = 0.0;
                var quantity = "0", stockAdjustmentDetailsId = "0", openingStock = 0, receivedQuantity = 0.0, actualUsageQuantity = "0",colorId='0', sizeId = '0', styleId = '0';
                var isEdit = "0", stockAdjustmentId = "0", locationId = "0", wastageQuantity = 0.0, stockQuantity = 0.0, actualQuantity = 0.0;

                adjustmentFrequence = $("#ContentPlaceHolder1_ddlAdjustmentFrequency").val();
                costCenterId = $("#ContentPlaceHolder1_ddlCostCentre").val();
                locationId = $("#ContentPlaceHolder1_ddlStoreLocation").val();
                stockAdjustmentId = $("#ContentPlaceHolder1_hfStockAdjustmentId").val();

                companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

                if (costCenterId == "0") {
                    toastr.warning("Please select a Store Name");
                    $("#ContentPlaceHolder1_ddlCostCentre").focus();
                    return false;
                }
                if (locationId == "0") {
                    toastr.warning("Please select a Store Location");
                    $("#ContentPlaceHolder1_ddlStoreLocation").focus();
                    return false;
                } if (adjustmentFrequence == "0") {
                    toastr.warning("Please select Frequency");
                    $("#ContentPlaceHolder1_ddlAdjustmentFrequency").focus();
                    return false;
                }
                var ItemStockAdjustment = {
                    StockAdjustmentId: stockAdjustmentId,
                    CostCenterId: costCenterId,
                    LocationId: locationId,
                    AdjustmentFrequency: adjustmentFrequence,
                    CompanyId: companyId,
                    ProjectId: projectId,
                };

                var StockAdjustDetails = [], StockAdjustDetailsEdit = [];

                $("#ItemAdjustmentGrid tbody tr").each(function (index, item) {

                    stockAdjustmentDetailsId = $.trim($(item).find("td:eq(10)").text());
                    isEdit = $.trim($(item).find("td:eq(15)").text());

                    itemId = $.trim($(item).find("td:eq(11)").text());
                    stockById = $(item).find("td:eq(13)").text();

                    openingStock = $(item).find("td:eq(3)").text();
                    receivedQuantity = $(item).find("td:eq(3)").text();
                    actualUsageQuantity = $(item).find("td:eq(4)").text();
                    wastageQuantity = $(item).find("td:eq(5)").text();

                    stockQuantity = $(item).find("td:eq(6)").text();
                    actualQuantity = $(item).find("td:eq(7)").find('input').val();

                    previousQuantity = $(item).find("td:eq(14)").text();
                    colorId = $(item).find("td:eq(16)").text();
                    sizeId = $(item).find("td:eq(17)").text();
                    styleId = $(item).find("td:eq(18)").text();

                    if (actualQuantity == "") {
                        return;
                    }

                    if (parseFloat(actualQuantity) >= 0) { //parseFloat(actualQuantity) != parseFloat(previousQuantity) && parseFloat(actualQuantity) != 0
                        if (stockAdjustmentDetailsId == "0") {

                            StockAdjustDetails.push({
                                StockAdjustmentDetailsId: stockAdjustmentDetailsId,
                                StockAdjustmentId: stockAdjustmentId,
                                ItemId: itemId,
                                LocationId: locationId,
                                StockById: stockById,
                                OpeningQuantity: openingStock,
                                ReceiveQuantity: receivedQuantity,
                                ActualUsage: actualUsageQuantity,
                                WastageQuantity: wastageQuantity,
                                StockQuantity: stockQuantity,
                                ActualQuantity: actualQuantity,
                                ColorId: colorId,
                                SizeId: sizeId,
                                StyleId: styleId
                            });
                        }
                        else if (stockAdjustmentDetailsId != "0" && isEdit == "1") {
                            StockAdjustDetailsEdit.push({
                                StockAdjustmentDetailsId: stockAdjustmentDetailsId,
                                StockAdjustmentId: stockAdjustmentId,
                                ItemId: itemId,
                                LocationId: locationId,
                                StockById: stockById,
                                OpeningQuantity: openingStock,
                                ReceiveQuantity: receivedQuantity,
                                ActualUsage: actualUsageQuantity,
                                WastageQuantity: wastageQuantity,
                                StockQuantity: stockQuantity,
                                ActualQuantity: actualQuantity,
                                ColorId: colorId,
                                SizeId: sizeId,
                                StyleId: styleId
                            });
                        }
                    }
                });

                if (StockAdjustDetails.length == 0 && stockAdjustmentId == "0") {
                    toastr.info("Please Give Adjustment Quantity");
                    CommonHelper.SpinnerClose();
                    return false;
                }

                PageMethods.SaveStockAdjustment2(ItemStockAdjustment, StockAdjustDetails, StockAdjustDetailsEdit, DeletedAdjustItem, AddedSerialzableProduct, DeletedSerialzableProduct, OnSaveStockAdjustmentSucceeded2, OnSaveStockAdjustmentFailed);
                return false;
            } else {
                CommonHelper.SpinnerClose();
                return false;

            }
        }
        function OnSaveStockAdjustmentSucceeded2(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction2();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
            return false;
        }

        function CheckInputValue(txtQuantity) {

            var quantity = $(txtQuantity).val();

            if (quantity != "" && CommonHelper.IsDecimal(quantity) == false) {
                toastr.warning("Please Give Valid value.");
                $(txtQuantity).val("");
                quantity = "";
                return false;
            }

            var tr = $(txtQuantity).parent().parent();
            var stockQuantity = parseFloat($(tr).find("td:eq(6)").text());
            var varianceQuantity = 0.00;

            if (quantity != "") {
                //varianceQuantity = stockQuantity - parseFloat(quantity);

                if (stockQuantity < 0) {
                    varianceQuantity = parseFloat(quantity) + stockQuantity;
                }
                else {
                    varianceQuantity = parseFloat(quantity) - stockQuantity;
                }

                $(tr).find("td:eq(8)").text(varianceQuantity.toFixed(2));
            }
        }

        function LoadLocationByCostCenter2(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationSucceeded2, OnLoadLocationFailed);
        }

        function OnLoadLocationSucceeded2(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlStoreLocation');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].LocationId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            control.val($("#ContentPlaceHolder1_hfLocationId").val());
            return false;
        }
        function PerformClearActionOnButtonClick() {
            if (!confirm("Do you want To clear?")) {
                return false;
            }
            PerformClearAction2();
        }
        function PerformClearAction2() {

            $("#ItemAdjustmentGrid tbody").html("");
            $("#ContentPlaceHolder1_btnSave2").val("Save");
            $("#ContentPlaceHolder1_ddlFiletrBy").trigger('change');
            return false;
        }
        function AddSerialForStockAdjustment(control) {
            var tr = $(control).parent().parent();

            var itemName = $(tr).find("td:eq(0)").text();
            var quantity = Math.abs($(tr).find("td:eq(8)").text());
            var itemId = $(tr).find("td:eq(11)").text();
            var locationId = $(tr).find("td:eq(12)").find("input").val();


            SearialAddedWindow(itemName, itemId, quantity, locationId);
        }
        function SearialAddedWindow(itemName, itemId, quantity, locationId) {

            ClearSerial();
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
                        tr += "<td style='display:none;'>" + addedSerial[row].SerialId + "</td>";

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

        function ClearSerial() {
            $("#txtSerial").val("");
        }
        function ClearSerialWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            ClearSerial();
        }
    </script>
    <div id="DetailsProductAdjustmentGridContainer" style="display: none; height: 500px; overflow-y: scroll">
        <table id='ItemAdjustmentGridDetails' class="table table-bordered table-condensed table-responsive"
            style='width: 100%;'>
            <thead>
                <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                    <th style='width: 15%;'>Item Name
                    </th>
                    <th style='width: 10%;'>Stock By
                    </th>
                    <th style='width: 12%;'>Opening Stock
                    </th>
                    <th style='width: 12%;'>Received Quantity
                    </th>
                    <th style='width: 12%;'>Usage Quantity
                    </th>
                    <th style='width: 13%;'>Wastage Quantity
                    </th>
                    <th style='width: 12%;'>Stock Quantity
                    </th>
                    <th style='width: 14%;'>Actual Quantity
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemIdForSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfStockAdjustmentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsEditedFromApprovedForm" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hfAdjustmentProductTemplate" runat="server" Value="0" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsItemSerialFillWithAutoSearch" runat="server" Value="0" />

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Stock Adjustment</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Approve Adjustment</a></li>
        </ul>
        <div id="tab-1">
            <div id="StockVarianceTemplate1" runat="server">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Location"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblItemName" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtItemName" CssClass="form-control" TabIndex="21" runat="server"
                                    ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Stock By"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlStockBy" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Transaction Mode"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlInvTransactionMode" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Quantity"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAdjustQuantity" TabIndex="8" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Value"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAdjustmentValue" TabIndex="8" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Reason"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlAdjustmentReason" CssClass="ThreeColumnDropDownList" runat="server"
                                    TabIndex="20">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" style="padding: 5px 0 5px 0;">
                            <button type="button" id="btnAddAdjustment" tabindex="24" class="TransactionalButton btn btn-primary btn-sm">
                                Add</button>
                        </div>
                        <div class="form-group" style="padding-bottom: 5px;">
                            <div id="ProductAdjustmentGridContainer">
                                <table id='ProductAdjustmentGrid' class="table table-bordered table-condensed table-responsive" style='width: 100%;'>
                                    <thead>
                                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                            <th style='width: 20%;'>Item Name
                                            </th>
                                            <th style='width: 12%;'>Location
                                            </th>
                                            <th style='width: 10%;'>Stock By
                                            </th>
                                            <th style='width: 10%;'>Quantity
                                            </th>
                                            <th style='width: 10%;'>Value
                                            </th>
                                            <th style='width: 12%;'>Allowance
                                            </th>
                                            <th style='width: 18%;'>Reason
                                            </th>
                                            <th style='width: 7%;'>Action
                                            </th>
                                            <th style='display: none'>StockAdjustmentDetailsId
                                            </th>
                                            <th style='display: none'>ItemId
                                            </th>
                                            <th style='display: none'>LocationId
                                            </th>
                                            <th style='display: none'>StockById
                                            </th>
                                            <th style='display: none'>TransactionModeId
                                            </th>
                                            <th style='display: none'>DBQuantity
                                            </th>
                                            <th style='display: none'>IsEdited
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="24" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript:return ValidationBeforeSave();" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="25" OnClientClick="javascript:return PerformClearAction2();"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="StockVarianceTemplate2" runat="server">
                <div class="panel-body">
                    <div class="form-horizontal">                        
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Store Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCostCentre" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label9" runat="server" class="control-label required-field" Text="Store Location"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlStoreLocation" runat="server" TabIndex="5" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label10" runat="server" class="control-label required-field" Text="Frequency"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAdjustmentFrequency" runat="server" CssClass="form-control" TabIndex="20">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                                    <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                                    <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <label class="control-label">Filter By</label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlFiletrBy" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="All Item" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Individual Item" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2" id="lblItem" style="display: none">
                                <label class="control-label required-field">Item Name</label>
                            </div>
                            <div class="col-md-10" id="dvItem" style="display: none">
                                <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                        <div class="form-group">
                            <div id="AdjustmentItemGridContainer">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave2" runat="server" Text="Save" TabIndex="24" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript:return ValidationBeforeSave2();" />
                                <asp:Button ID="btnClear2" runat="server" Text="Clear" TabIndex="25" OnClientClick="javascript:return PerformClearActionOnButtonClick();"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Adjustment Product Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label355" runat="server" Text="Store Name" CssClass="control-label"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchCostCenter" runat="server" CssClass="form-control"
                                    TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchCostCenter_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label8" runat="server" class="control-label required-field" Text="Store Location"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchLocation" runat="server" TabIndex="5" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search For Approval" OnClick="btnSearch_Click"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                                <%-- <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" TabIndex="6" />--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvFinishedProductInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" TabIndex="9"
                        OnRowCommand="gvFinishedProductInfo_RowCommand" OnRowDataBound="gvFinishedProductInfo_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("StockAdjustmentId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Adjustment Date" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("AdjustmentDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="CostCenter" HeaderText="Store Name" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AdjustmentFrequency" HeaderText="Adjustment Frequency" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ApprovedStatus" HeaderText="Status" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgDetailsRequisition" runat="server" CausesValidation="False"
                                        CommandName="CmdDetails" CommandArgument='<%# bind("StockAdjustmentId") %>' OnClientClick='<%#String.Format("return AdjustmentProductDetails({0})", Eval("StockAdjustmentId")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Adjustment Details" />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("StockAdjustmentId") %>' OnClientClick='<%#String.Format("return FillForm({0})", Eval("StockAdjustmentId")) %>'
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" OnClientClick="return confirm('Do you want to delete?');" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("StockAdjustmentId") %>' ImageUrl="~/Images/delete.png"
                                        Text="" AlternateText="Delete" ToolTip="Delete Order" />
                                    &nbsp;<asp:ImageButton ID="ImgDetailsApproved" OnClientClick="return confirm('Do you want to approve?');" runat="server" CausesValidation="False"
                                        CommandName="CmdAdjustmentApproved" CommandArgument='<%# bind("StockAdjustmentId") %>'
                                        ImageUrl="~/Images/approved.png" Text="" AlternateText="Approval" ToolTip="Approve Item Adjustment" />
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
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
            </div>
        </div>
    </div>
    <div id="SerialWindow" style="display: none;">
        <div class="panel panel-default">
            <div class="panel-body" style="padding: 4px;">

                <div class="form-horizontal">
                    <div id="labelAndtxtSerialAutoComplete" class="row">
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
                        <input type="button" class="btn btn-primary" style="padding-left: 30px; padding-right: 30px;" value="Ok" onclick="ApplySerialForStockAdjustment()" />
                        <input type="button" class="btn btn-primary" value="Cancel" onclick="CancelAddSerial()" />
                    </div>
                </div>

            </div>
        </div>
    </div>
     <script type="text/javascript">
         $(document).ready(function () {
             if ($("#ContentPlaceHolder1_companyProjectUserControl_hfIsSingle").val() != "1") {

                 if ($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val() == "0") {
                     $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");
                 }
             }
         });
     </script>
</asp:Content>
