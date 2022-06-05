<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmProductOutForRoom.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmProductOutForRoom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------

        var outItemEdited = "";
        var DeletedOutDetails = [];
        var RequisitionProductDetails = [];
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product Out</li>";
            var breadCrumbs = moduleName + formName;

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            if (IsCanSave) {
                $('#btnSave').show();
                } else {
                        $('#btnSave').hide();
                }

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlCostCenter").change(function () {
                LoadLocation($(this).val());
            });

            $("#ContentPlaceHolder1_ddlCategory").change(function () {

                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
                var categoryId = $(this).val();

                LoadProductByCategoryNCostcenterId(costCenterId, categoryId);
            });

            $("#ContentPlaceHolder1_ddlRoomType").change(function () {
                var roomTypeId = $(this).val();

                LoadRoomNumberByRoomType(roomTypeId);
            });


            $("#btnAdd").click(function () {

                var quantity = $('#ContentPlaceHolder1_txtQuantity').val();
                var duplicateSerial = 0;

                var costCenterId = $('#ContentPlaceHolder1_ddlCostCenter').val();
                var stockById = $("#ContentPlaceHolder1_ddlStockBy").val();
                var locationId = $("#ContentPlaceHolder1_ddlLocation").val();
                var itemId = $("#ContentPlaceHolder1_ddlProduct").val();

                if (costCenterId == "0") {
                    toastr.warning('Please provide cost center.');
                    return;
                }
                else if (stockById == "0") {
                    toastr.warning("Please provide a stock by.");
                    return;
                }
                else if (locationId == "0") {
                    toastr.warning("Please provide a location.");
                    return;
                }
                else if (itemId == "0") {
                    toastr.warning("Please provide an item.");
                    return;
                }
                else if ($.trim(quantity) == "") {
                    toastr.warning("Please provide quantity.");
                    return;
                }

                var quantity = "0", outId = "0", outDetailsId = "0", duplicateItemId = 0;
                var productName = "", stockBy = "", costCenter = "";
                var categoryId = "0", location = '';

                outId = $("#ContentPlaceHolder1_hfOutId").val();

                productName = $("#ContentPlaceHolder1_ddlProduct option:selected").text();
                stockBy = $("#ContentPlaceHolder1_ddlStockBy option:selected").text();
                costCenter = $("#ContentPlaceHolder1_ddlCostCenter option:selected").text();
                categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                location = $("#ContentPlaceHolder1_ddlLocation option:selected").text();
                quantity = $("#ContentPlaceHolder1_txtQuantity").val();

                if (outItemEdited != "") {
                    var editedItemId = $.trim($(outItemEdited).find("td:eq(8)").text());

                    if (editedItemId != itemId) {
                        duplicateItemId = $("#ProductOutGrid tbody tr").find("td:eq(13):contains(" + (locationId + '-' + itemId) + ")").length;

                        if (duplicateItemId > 0) {
                            toastr.warning("Same item already added.");
                            return false;
                        }
                    }
                }
                else {
                    duplicateItemId = $("#ProductOutGrid tbody tr").find("td:eq(13):contains(" + (locationId + '-' + itemId) + ")").length;

                    if (duplicateItemId > 0) {
                        toastr.warning("Same item already added.");
                        return false;
                    }
                }

                //                Product Name      0
                //                 Quantity         1
                //                 Cost Center      2
                //                 Location        3
                //                 Unit Measure   4
                //                 Action         5
                //                 OutDetailsId   6
                //                 CategoryId     7
                //                 ItemId         8
                //                 CostCenterId   9
                //                 LocationId     10
                //                 StockById      11
                //                 Is Edited      12
                //                duplicateCheck  13

                //productName, quantity, costCenter, location, stockBy, outDetailsId, categoryId, itemId, costCenterId, locationId, stockById

                if (outId != "0" && outItemEdited != "") {

                    outDetailsId = $(outItemEdited).find("td:eq(6)").text();
                    EditItem(productName, quantity, costCenter, location, stockBy, outDetailsId, categoryId, itemId, costCenterId, locationId, stockById);
                    //toastr.info("Edit db");
                    return;
                }
                else if (outId == "0" && outItemEdited != "") {

                    outDetailsId = $(outItemEdited).find("td:eq(6)").text();
                    EditItem(productName, quantity, costCenter, location, stockBy, outDetailsId, categoryId, itemId, costCenterId, locationId, stockById);
                    //toastr.info("Edit");
                    return;
                }

                AddItemForOut(productName, quantity, costCenter, location, stockBy, outDetailsId, categoryId, itemId, costCenterId, locationId, stockById);

                $("#ContentPlaceHolder1_hfItemId").val("0");
                $("#ContentPlaceHolder1_txtQuantity").val("");
                $("#ContentPlaceHolder1_ddlStockBy").val("0");
                $("#ContentPlaceHolder1_ddlProduct").val("0");

                return false;
            });

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'

            $('#' + txtFromDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtToDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtToDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtFromDate).datepicker("option", "maxDate", selectedDate);
                }
            });

        });

        function LoadLocation(costCenetrId) {
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
        function OnLoadLocationFailed() { }

        function LoadProductByCategoryNCostcenterId(costCenterId, categoryId) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmProductOutForRoom.aspx/LoadProductByCategoryNCostcenterId',
                data: "{'costCenterId':'" + costCenterId + "','categoryId':'" + categoryId + "'}",
                dataType: "json",
                success: function (data) {

                    var list = data.d;
                    var control = $('#ContentPlaceHolder1_ddlProduct');

                    control.empty();
                    if (list != null) {
                        if (list.length > 0) {

                            control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                            for (i = 0; i < list.length; i++) {
                                control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].ItemName + '</option>');
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                        }
                    }

                    control.val($("#ContentPlaceHolder1_hfItemId").val());
                },
                error: function (result) {
                }
            });

        }

        function LoadRoomNumberByRoomType(roomTypeId) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmProductOutForRoom.aspx/LoadRoomNumberByRoomType',
                data: "{'roomTypeId':'" + roomTypeId + "'}",
                dataType: "json",
                success: function (data) {

                    var list = data.d;
                    var control = $('#ContentPlaceHolder1_ddlRoomId');

                    control.empty();
                    if (list != null) {
                        if (list.length > 0) {

                            control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                            for (i = 0; i < list.length; i++) {
                                control.append('<option title="' + list[i].RoomNumber + '" value="' + list[i].RoomId + '">' + list[i].RoomNumber + '</option>');
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                        }
                    }

                    control.val($("#ContentPlaceHolder1_hfRoomId").val());
                },
                error: function (result) {
                }
            });

        }

        function AddItemForOut(productName, quantity, costCenter, location, stockBy, outDetailsId, categoryId, itemId, costCenterId, locationId, stockById) {

            var isEdited = "0";
            var rowLength = $("#ProductOutGrid tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:30%;'>" + productName + "</td>";
            tr += "<td style='width:10%;'>" + quantity + "</td>";
            tr += "<td style='width:20%;'>" + costCenter + "</td>";
            tr += "<td style='width:20%;'>" + location + "</td>";
            tr += "<td style='width:10%;'>" + stockBy + "</td>";
            tr += "<td style='width:10%;'> <span onclick=\"javascript:return FIllForEdit(this);\" title='Edit'><img src='../Images/edit.png' alt='Edit'></span>"
            tr += "&nbsp;&nbsp;<span onclick= 'javascript:return DeleteItemReceivedDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></span>";
            tr += "<td style='display:none'>" + outDetailsId + "</td>";
            tr += "<td style='display:none'>" + categoryId + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + costCenterId + "</td>";
            tr += "<td style='display:none'>" + locationId + "</td>";
            tr += "<td style='display:none'>" + stockById + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "<td style='display:none'>" + (locationId + '-' + itemId) + "</td>";

            tr += "</tr>";

            $("#ProductOutGrid tbody").append(tr);
        }

        function FIllForEdit(editItem) {

            outItemEdited = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAdd").val("Update");

            var categoryId = $.trim($(tr).find("td:eq(7)").text());
            var itemId = $.trim($(tr).find("td:eq(8)").text());
            var costCenterId = $(tr).find("td:eq(9)").text();
            var locationId = $(tr).find("td:eq(10)").text();
            var stockById = $(tr).find("td:eq(11)").text();

            var itemName = $(tr).find("td:eq(0)").text();
            var quantity = $(tr).find("td:eq(1)").text();

            $("#ContentPlaceHolder1_hfLocationId").val(locationId);
            $("#ContentPlaceHolder1_hfItemId").val(itemId);

            if ($("#ContentPlaceHolder1_ddlCostCenter").val() != costCenterId) {
                LoadLocation(costCenterId);
            }
            else {
                $("#ContentPlaceHolder1_ddlLocation").val(locationId);
            }

            if ($("#ContentPlaceHolder1_ddlCategory").val() != $.trim(categoryId)) {
                LoadProductByCategoryNCostcenterId(costCenterId, categoryId);
            }
            else {
                $("#ContentPlaceHolder1_ddlLocation").val(locationId);
            }

            $("#ContentPlaceHolder1_ddlCategory").val(categoryId);
            $("#ContentPlaceHolder1_ddlCostCenter").val(costCenterId);
            $("#ContentPlaceHolder1_ddlStockBy").val(stockById);
            $("#ContentPlaceHolder1_txtQuantity").val(quantity);
            $("#ContentPlaceHolder1_ddlProduct").val(itemId);
        }

        function EditItem(productName, quantity, costCenter, location, stockBy, outDetailsId, categoryId, itemId, costCenterId, locationId, stockById) {

            $(outItemEdited).find("td:eq(0)").text(productName);
            $(outItemEdited).find("td:eq(1)").text(quantity);
            $(outItemEdited).find("td:eq(2)").text(costCenter);
            $(outItemEdited).find("td:eq(3)").text(location);
            $(outItemEdited).find("td:eq(4)").text(stockBy);

            $(outItemEdited).find("td:eq(6)").text(outDetailsId);
            $(outItemEdited).find("td:eq(7)").text(categoryId);
            $(outItemEdited).find("td:eq(8)").text(itemId);

            $(outItemEdited).find("td:eq(9)").text(costCenterId);
            $(outItemEdited).find("td:eq(10)").text(locationId);
            $(outItemEdited).find("td:eq(11)").text(stockById);

            var outId = $("#ContentPlaceHolder1_hfOutId").val();

            if (outDetailsId != "0")
                $(outItemEdited).find("td:eq(12)").text("1");

            $(outItemEdited).find("td:eq(13)").text((locationId + '-' + itemId));

            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_ddlProduct").val("0");
            $("#ContentPlaceHolder1_txtQuantity").val("");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_ddlCostCenter").val("0");
            $("#ContentPlaceHolder1_hfLocationId").val("0");

            $("#ContentPlaceHolder1_ddlCategory").val("0");
            $("#ContentPlaceHolder1_hfStockById").val("0");
            $("#ContentPlaceHolder1_ddlLocation").val("0");

            $("#btnAdd").val("Add");

            outItemEdited = "";
        }

        function FillForm(outId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            PageMethods.FillForm(outId, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {

            if (result != null) {

                PerformClearAction();

                $("#ContentPlaceHolder1_ddlRoomType").val(result.ProductOut.RoomTypeId);
                $("#ContentPlaceHolder1_ddlRoomId").val(result.ProductOut.RoomId);
                $("#ContentPlaceHolder1_hfRoomId").val(result.ProductOut.RoomId);
                $("#ContentPlaceHolder1_hfOutId").val(result.ProductOut.InventoryOutId);
                $("#ContentPlaceHolder1_txtRemarks").val(result.ProductOut.Remarks);
                $("#ProductOutGrid tbody").html("");

                var roomId = result.ProductOut.RoomId;

                if ($("#ContentPlaceHolder1_ddlRoomId").val() != roomId) {
                    LoadRoomNumberByRoomType(result.ProductOut.RoomTypeId);
                }
                else {
                    $("#ContentPlaceHolder1_ddlRoomId").val(roomId);
                }

                var rowLength = result.ProductOutDetails.length;
                var row = 0;

                for (row = 0; row < rowLength; row++) {

                    //productName, quantity, costCenter, location, stockBy, outDetailsId, categoryId, itemId, costCenterId, locationId, stockById

                    AddItemForOut(result.ProductOutDetails[row].ProductName, result.ProductOutDetails[row].Quantity,
                                    result.ProductOutDetails[row].CostCenter, result.ProductOutDetails[row].Location,
                                    result.ProductOutDetails[row].StockBy, result.ProductOutDetails[row].OutDetailsId,
                                    result.ProductOutDetails[row].CategoryId, result.ProductOutDetails[row].ProductId,
                                    result.ProductOutDetails[row].CostCenterId, result.ProductOutDetails[row].LocationId,
                                    result.ProductOutDetails[row].StockById);
                }
                if (IsCanEdit) {
                $('#btnSave').show();
                } else {
                        $('#btnSave').hide();
                }
                $("#ContentPlaceHolder1_btnSave").val("Update");
                //$("#myTabs").tabs('select', 0);
                $("#myTabs").tabs({ active: 0 });
            }
        }
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
        }

        function DeleteItemReceivedDetails(deletedItem) {

            if (!confirm("Do you want to delete?"))
                return;

            var outId = $("#ContentPlaceHolder1_hfOutId").val();

            if (outId != "0") {
                var tr = $(deletedItem).parent().parent();

                DeletedOutDetails.push({
                    OutDetailsId: $(tr).find("td:eq(6)").text(),
                    OutId: outId
                });
            }

            $(deletedItem).parent().parent().remove();
        }

        function ValidationBeforeSave() {

            if ($("#ContentPlaceHolder1_ddlRoomType").val() == "0") {
                toastr.info("Please Select Room Type.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlRoomId").val() == null || $("#ContentPlaceHolder1_ddlRoomId").val() == "0") {
                toastr.info("Please Select Room.");
                return false;
            }

            var rowCount = $('#ProductOutGrid tbody tr').length;
            if (rowCount == 0) {
                toastr.warning('Add atleast one Product.');
                return false;
            }

            CommonHelper.SpinnerOpen();

            var itemId = "0", costCenterId = "0", stockById = "0";
            var roomTypeId = "0", roomId = "", quantity = "0", locationId = "0";
            var isEdit = "0", outId = "0", outDetailsId = "0", remarks = "", outFor = "0";

            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            outId = $("#ContentPlaceHolder1_hfOutId").val();
            roomTypeId = $("#ContentPlaceHolder1_ddlRoomType").val();
            roomId = $("#ContentPlaceHolder1_ddlRoomId").val();

            var ProductOut = {
                InventoryOutId: outId,
                RoomTypeId: roomTypeId,
                RoomId: roomId,
                Remarks: remarks
            };

            var AddedOutDetails = [], EditedOutDetails = [];

            $("#ProductOutGrid tbody tr").each(function (index, item) {

                outDetailsId = $.trim($(item).find("td:eq(6)").text());
                isEdit = $.trim($(item).find("td:eq(12)").text());

                itemId = $.trim($(item).find("td:eq(8)").text());
                costCenterId = $(item).find("td:eq(9)").text();
                locationId = $(item).find("td:eq(10)").text();
                stockById = $(item).find("td:eq(11)").text();
                quantity = $(item).find("td:eq(1)").text();

                if (outDetailsId == "0") {

                    AddedOutDetails.push({
                        OutDetailsId: outDetailsId,
                        InventoryOutId: outId,
                        CostCenterId: costCenterId,
                        LocationId: locationId,
                        StockById: stockById,
                        ProductId: itemId,
                        Quantity: quantity
                    });
                }
                else if (outDetailsId != "0" && isEdit != "0") {
                    EditedOutDetails.push({
                        OutDetailsId: outDetailsId,
                        InventoryOutId: outId,
                        CostCenterId: costCenterId,
                        LocationId: locationId,
                        StockById: stockById,
                        ProductId: itemId,
                        Quantity: quantity
                    });
                }
            });

            PageMethods.SaveProductOut(ProductOut, AddedOutDetails, EditedOutDetails, DeletedOutDetails, OnSaveProductOutSucceeded, OnSaveProductOutFailed);

            return false;
        }
        function OnSaveProductOutSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSaveProductOutFailed(error) { toastr.error(error.get_message()); CommonHelper.SpinnerClose(); }

        function ProductOutDetails(outId) {
            CommonHelper.SpinnerOpen();
            PageMethods.GetProductOutDetails(outId, OnProductOutDetailsLoadSucceeded, OnSaveProductOutDetailsLoadFailed);
            return false;
        }
        function OnProductOutDetailsLoadSucceeded(result) {

            $("#DetailsOutGrid tbody").html("");
            var totalRow = result.length, row = 0;

            var tr = "";

            for (row = 0; row < totalRow; row++) {

                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:25%;'>" + result[row].ProductName + "</td>";

                tr += "<td style='width:10%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:25%;'>" + result[row].CostCenter + "</td>";
                tr += "<td style='width:25%;'>" + result[row].Location + "</td>";
                tr += "<td style='width:15%;'>" + result[row].StockBy + "</td>";

                tr += "</tr>";

                $("#DetailsOutGrid tbody").append(tr);
                tr = "";
            }

            $("#DetailsOutGridContaiiner").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 800,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Out Item Details",
                show: 'slide'
            });
            CommonHelper.SpinnerClose();
        }
        function OnSaveProductOutDetailsLoadFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function EntryPanelVisibleTrue() {
            $('#btnNewProduct').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewProduct').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        function ClearControl() {
            $("#frmHotelManagement")[0].reset();
        }

        function PerformClearAction() {

            $("#ContentPlaceHolder1_hfOutId").val("0");
            $("#ContentPlaceHolder1_txtRemarks").val("");

            $("#ContentPlaceHolder1_ddlRoomType").val("0");
            $("#ContentPlaceHolder1_ddlRoomId").val("0");
            $("#ContentPlaceHolder1_ddlCategory").val("0");
            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_ddlProduct").val("0");

            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_hfStockById").val("0");

            $('#ContentPlaceHolder1_ddlCostCenter').val("0");
            $("#ContentPlaceHolder1_hfLocationId").val("0");
            $("#ContentPlaceHolder1_ddlLocation").val("0");

            $("#ProductOutGrid tbody").html("");

            outItemEdited = "";
            DeletedOutDetails = [];
            RequisitionProductDetails = [];

            $("#ContentPlaceHolder1_btnSave").val("Save");

            $("#ContentPlaceHolder1_ddlSearchCostCenter").val("0");
            $("#ContentPlaceHolder1_ddlSearchLocation").val("0");

            return false;
        }

        function CancelOutOrder() {

            $("#ContentPlaceHolder1_ddlCostCenter").val("0");
            $("#ContentPlaceHolder1_ddlCategory").val("0");
            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_ddlProduct").val("0");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_hfStockById").val("0");
            $("#ContentPlaceHolder1_ddlLocation").val("0");

            if ($("#ProductOutGrid tbody tr").length == 0 && $("#ContentPlaceHolder1_hfOutId").val() == "0") {
                $("#ContentPlaceHolder1_hfOutId").val("0");
                DeletedOutDetails = [];
            }

            outItemEdited = "";
        }

        function PerformClearActionWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }
        function ConfirmationForDelete() {
            if (!confirm("Do you Want To Delete?")) {
                return false;
            }
        }

    </script>
    <div id="DetailsOutGridContaiiner" style="display: none;">
        <table id="DetailsOutGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 25%;">
                        Item Name
                    </th>
                    <th style="width: 10%;">
                        Quantity
                    </th>
                    <th style="width: 25%;">
                        Cost Center
                    </th>
                    <th style="width: 25%;">
                        Location
                    </th>
                    <th style="width: 15%;">
                        Unit Measure
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfRoomId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsSerializableItem" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsValidSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfOutId" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSearchLocationId" runat="server" Value="0" />

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
   
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Item Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Item Out</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Item Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Room Type"></asp:Label>                               
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomType" runat="server" TabIndex="17" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblRoomId" runat="server" class="control-label required-field" Text="Room Number"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomId" runat="server" CssClass="form-control"
                                    TabIndex="19">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCostCenterId" runat="server" class="control-label required-field" Text="From Cost Center"></asp:Label>                               
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label13" runat="server" class="control-label required-field" Text="Location From"></asp:Label>                               
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server"
                                    TabIndex="3">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label8" runat="server" class="control-label required-field" Text="Item"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Unit Measure"></asp:Label>                              
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlStockBy" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblQuantity" runat="server" class="control-label required-field" Text="Quantity"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="padding:5px 0 5px 0;">
                            <input type="button" id="btnAdd" value="Add" class="TransactionalButton btn btn-primary btn-sm"
                                tabindex="4" />
                            <input type="button" id="btnCancelOutOrder" value="Cancel" onclick="CancelOutOrder()"
                                class="TransactionalButton btn btn-primary btn-sm" tabindex="4" />
                        </div>
                        <div class="form-group">
                            <div id="SearchPanel" class="panel panel-default">                               
                                <div class="panel-heading">Detail Information</div>
                                <div id="ProductOutGridContainer">
                                    <table id="ProductOutGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                                        <thead>
                                            <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                                <th style="width: 30%;">
                                                    Item Name
                                                </th>
                                                <th style="width: 10%;">
                                                    Quantity
                                                </th>
                                                <th style="width: 20%;">
                                                    Cost Center
                                                </th>
                                                <th style="width: 20%;">
                                                    Location
                                                </th>
                                                <th style="width: 10%;">
                                                    Unit Measure
                                                </th>
                                                <th style="width: 10%;">
                                                    Action
                                                </th>
                                                <th style="display: none">
                                                    OutDetailsId
                                                </th>
                                                <th style="display: none">
                                                    CategoryId
                                                </th>
                                                <th style="display: none">
                                                    ItemId
                                                </th>
                                                <th style="display: none">
                                                    CostCenterId
                                                </th>
                                                <th style="display: none">
                                                    LocationId
                                                </th>
                                                <th style="display: none">
                                                    StockById
                                                </th>
                                                <th style="display: none">
                                                    Is Edited
                                                </th>
                                                <th style="display: none">
                                                    duplicateCheck
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label required-field" Text="Remarks"></asp:Label>                               
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" TabIndex="6" CssClass="form-control"
                                    TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="7" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return ValidationBeforeSave();" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="8" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearActionWithConfirmation();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="InfoPanel" class="panel panel-default">
                <div class="panel-heading">
                    Receive Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Room Type"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchRoomType" runat="server" TabIndex="17" CssClass="form-control"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlSearchRoomType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Room Number"></asp:Label>                              
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchRoomId" runat="server" CssClass="form-control"
                                    TabIndex="19">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchCostCenter" runat="server" CssClass="form-control"
                                    TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchCostCenter_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Location"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchLocation" runat="server" CssClass="form-control"
                                    TabIndex="5">
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
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" TabIndex="6" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Div1" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvProductOutInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" TabIndex="9"
                        OnRowCommand="gvProductOutInfo_RowCommand" OnRowDataBound="gvProductOutInfo_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("InventoryOutId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Out Date" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("OutDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="RoomType" HeaderText="Room Type" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RoomNumber" HeaderText="Room Number" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgDetailsRequisition" runat="server" CausesValidation="False"
                                        CommandName="CmdDetails" CommandArgument='<%# bind("InventoryOutId") %>' OnClientClick='<%#String.Format("return ProductOutDetails({0})", Eval("InventoryOutId")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Product Out Details" />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("InventoryOutId") %>' OnClientClick='<%#String.Format("return FillForm({0})", Eval("InventoryOutId")) %>'
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("InventoryOutId") %>' OnClientClick="ConfirmationForDelete()" ImageUrl="~/Images/delete.png"
                                        Text="" AlternateText="Delete" ToolTip="Delete" />
                                    &nbsp;<asp:ImageButton ID="ImgDetailsApproved" runat="server" CausesValidation="False"
                                        CommandName="CmdItemOutApproved" CommandArgument='<%# bind("InventoryOutId") %>'
                                        OnClientClick='<%#String.Format("return ProductOutApproved({0})", Eval("InventoryOutId")) %>'
                                        ImageUrl="~/Images/approved.png" Text="" AlternateText="Details" ToolTip="Approve Out Item" />
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
    <script type="text/javascript">
        var x = '<%=btnPadding%>';
        if (x > -1) {

            $("#btnAdd").animate({ marginTop: '10px' });
        }
        else {

        }        
    </script>
</asp:Content>
