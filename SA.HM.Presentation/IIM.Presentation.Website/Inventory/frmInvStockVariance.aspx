<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmInvStockVariance.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmInvStockVariance" %>
<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var AdjustmentItemEdited = "";
        var DeletedAdjustItem = [];
        var itemArry = null;

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Item Wastage</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val() == "1") {
                FillForm($("#ContentPlaceHolder1_hfStockAdjustmentId").val());
            }

            $("#myTabs").tabs();

            $("#btnAddAdjustment").click(function () {

                var costCenterId = $("#<%=ddlCostCenter.ClientID %>").val();
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

            $("#ContentPlaceHolder1_ddlStoreLocation").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#txtItemName").autocomplete({
                source: function (request, response) {
                    var costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
                    var locationId = $("#ContentPlaceHolder1_ddlLocation").val();

                    if (costCenterId == "0") {
                        toastr.warning("Please Select a Cost Center.");
                        return false;
                    }
                    else if (locationId == "0") {
                        toastr.warning("Please Select a Location");
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmInvStockVariance.aspx/ItemNCategoryAutoSearch',
                        data: "{'itemName':'" + request.term + "','" + "costCenterId':'" + costCenterId + "','" + "locationId':'" + locationId + "'}",
                        dataType: "json",
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

            $("#ContentPlaceHolder1_ddlCostCenter").change(function () {

                var costCenetrId = $("#ContentPlaceHolder1_ddlCostCenter").val();

                if (costCenetrId == "0") {
                    toastr.info("Please select cost center");
                    return;
                }

                LoadLocationByCostCenter(costCenetrId);
                return false;
            });

            $("#ContentPlaceHolder1_txtAdjustQuantity").change(function () {
                var adjustmentQuantity = $("#ContentPlaceHolder1_txtAdjustQuantity").val();
            });

        });

        function ItemCost() {
            var itemId = $("#ContentPlaceHolder1_hfItemId").val();
            var stockById = $("#<%=ddlStockBy.ClientID %>").val();
            var quantity = $("#<%=txtAdjustQuantity.ClientID %>").val();

            GetItemCost(itemId, stockById, quantity);
        }

        function AddItem(itemName, locationName, stockBy, dbQuantity, quantity, value, transactionMode, reason, stockAdjustmentDetailsId, itemId, locationId, stockById, transactionModeId, isEdited) {

            var tr = "", rowCount = $("#ItemVarianceGridTemplate1 tbody tr").length;

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

            $("#ItemVarianceGridTemplate1 tbody").append(tr);
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

            var rowCount = $('#ItemVarianceGridTemplate1 tbody tr').length;
            if (rowCount == 0) {
                toastr.warning('Add atleast one Product.');
                return false;
            }

            var costCenterId = "0", itemId = "0", stockById = "0", previousQuantity = 0.0;
            var quantity = "0", stockVarianceDetailsId = "0", Reason = '', quantityValue = 0, unitPrice = 0.0;
            var isEdit = "0", stockVariancetId = "0", locationId = "0", tModeId = "0", usageQuantity = 0.0, usageCost = 0.0;

            costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
            stockVariancetId = $("#ContentPlaceHolder1_hfStockAdjustmentId").val();

            companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            if (companyId == null)
                companyId = "0";
            if (projectId == null)
                projectId = "0";

            if (companyId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                toastr.warning("Please Select Company.");
                return false;
            }
            if (projectId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                toastr.warning("Please Select Project.");
                return false;
            }

            var ItemStockVariance = {
                StockVarianceId: stockVariancetId,
                CostCenterId: costCenterId,
                CompanyId: companyId,
                ProjectId: projectId
            };

            var StockVarianceDetails = [], StockVarianceDetailsEdit = [];

            $("#ItemVarianceGridTemplate1 tbody tr").each(function (index, item) {

                stockVarianceDetailsId = $.trim($(item).find("td:eq(8)").text());
                isEdit = $.trim($(item).find("td:eq(14)").text());

                itemId = $.trim($(item).find("td:eq(9)").text());
                locationId = $.trim($(item).find("td:eq(10)").text());
                stockById = $(item).find("td:eq(11)").text();
                tModeId = $(item).find("td:eq(12)").text();
                Reason = $(item).find("td:eq(6)").text();

                quantity = $(item).find("td:eq(3)").text();
                usageQuantity = '0.00';  //$(item).find("td:eq(3)").text();
                usageCost = $(item).find("td:eq(4)").text();
                unitPrice = '0.00'; //$(item).find("td:eq(14)").text();
                previousQuantity = $(item).find("td:eq(13)").text();

                if (quantity == "")
                    quantity = "0";

                if (parseFloat(quantity) != parseFloat(previousQuantity) && parseFloat(quantity) != 0) {
                    if (stockVarianceDetailsId == "0") {

                        StockVarianceDetails.push({
                            StockVarianceDetailsId: parseInt(stockVarianceDetailsId, 10),
                            StockVarianceId: parseInt(stockVariancetId, 10),
                            ItemId: parseInt(itemId, 10),
                            LocationId: parseInt(locationId, 10),
                            StockById: parseInt(stockById, 10),
                            TModeId: parseInt(tModeId, 10),
                            UsageQuantity: usageQuantity,
                            UnitPrice: unitPrice,
                            UsageCost: usageCost,
                            VarianceQuantity: quantity,
                            PreviousQuantity: parseFloat(quantity),
                            Reason: Reason
                        });
                    }
                    else if (stockVarianceDetailsId != "0" && isEdit == "1") {
                        StockVarianceDetailsEdit.push({
                            StockVarianceDetailsId: parseInt(stockVarianceDetailsId, 10),
                            StockVarianceId: parseInt(stockVariancetId, 10),
                            ItemId: parseInt(itemId, 10),
                            LocationId: parseInt(locationId, 10),
                            StockById: parseInt(stockById, 10),
                            TModeId: parseInt(tModeId, 10),
                            UsageQuantity: usageQuantity,
                            UnitPrice: unitPrice,
                            UsageCost: usageCost,
                            VarianceQuantity: quantity,
                            PreviousQuantity: parseFloat(quantity),
                            Reason: Reason
                        });
                    }
                }
            });

            PageMethods.SaveStockVariance(ItemStockVariance, StockVarianceDetails, StockVarianceDetailsEdit, DeletedAdjustItem, OnSaveStockAdjustmentSucceeded, OnSaveStockAdjustmentFailed);
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
        function OnSaveStockAdjustmentFailed(error) { toastr.error(error.get_message()); }

        function VarianceProductDetails(stockVarianceId) {
            PageMethods.GetVarianceProductDetails(stockVarianceId, OnVarianceProductLoadSucceeded, OnSaveAdjustmentProductFailed);
            return false;
        }
        function OnVarianceProductLoadSucceeded(result) {

            $("#ProductVarianceGridDetails tbody").html("");
            var totalRow = result.length, row = 0;

            var tr = "";

            for (row = 0; row < totalRow; row++) {

                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:20%;'>" + result[row].ItemName + "</td>";
                tr += "<td style='width:10%;'>" + result[row].StockByName + "</td>";
                tr += "<td style='width:13%;'>" + result[row].UsageQuantity + "</td>";
                tr += "<td style='width:13%;'>" + result[row].UsageCost + "</td>";
                tr += "<td style='width:12%;'>" + result[row].TransactionMode + "</td>";
                tr += "<td style='width:12%;'>" + result[row].VarianceQuantity + "</td>";
                tr += "<td style='width:20%;'>" + result[row].Reason + "</td>";

                tr += "</tr>";

                $("#ProductVarianceGridDetails tbody").append(tr);
                tr = "";
            }

            $("#DetailsVarianceProductGridContainer").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 800,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Variance Product Details",
                show: 'slide'
            });
        }
        function OnSaveAdjustmentProductFailed(error) { }

        function FillForm(stockAdjustmentId) {
             if (!confirm("Do you want to edit?")) {
                return false;
            }

            PageMethods.FIllForm(stockAdjustmentId, OnFillFormSucceeded, OnFillFormFailed);
            return false;
        }

        function OnFillFormSucceeded(result) {

            if (result.AdjustmentItem != null) {
                $("#ContentPlaceHolder1_ddlCostCenter").val(result.StockVariance.CostCenterId);
                $("#ContentPlaceHolder1_hfStockAdjustmentId").val(result.StockVariance.StockVarianceId);
                $("#ContentPlaceHolder1_ddlCostCenter").attr('disabled', true);
            }

            LoadLocationByCostCenter(result.StockVariance.CostCenterId);

            var rowLength = 0, row = 0, isEdited = "0";
            rowLength = result.StockVarianceDetails.length;

            if (rowLength > 0) {

                for (row = 0; row < rowLength; row++) {

                    AddItem(result.StockVarianceDetails[row].ItemName, result.StockVarianceDetails[row].LocationName, result.StockVarianceDetails[row].StockByName,
                            result.StockVarianceDetails[row].PreviousQuantity, result.StockVarianceDetails[row].AdjustmentQuantity, result.StockVarianceDetails[row].AdjustmentCost,
                            result.StockVarianceDetails[row].TransactionMode, result.StockVarianceDetails[row].Reason, result.StockVarianceDetails[row].StockAdjustmentDetailsId,
                            result.StockVarianceDetails[row].ItemId, result.StockVarianceDetails[row].LocationId, result.StockVarianceDetails[row].StockById,
                            result.StockVarianceDetails[row].TModeId, isEdited)
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
                url: '../Inventory/frmInvStockVariance.aspx/GetReceipeItemCost',
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

            $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", true);
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

            $("#ItemVarianceGridTemplate1 tbody").html("");
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

            $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", false);

            return false;
        }

        //-------------------Item Variance Template 2 ------------------------------------

        $(document).ready(function () {

            $("#ContentPlaceHolder1_ddlStoreLocation").change(function () {

                if ($(this).val() == "0")
                    return;

                var locationId = $(this).val();
                var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();

                $("#VarianceItemGridContainer").show();
                if (categoryId == "-1") {
                    toastr.warning("Please Select Category.");
                    $("#VarianceItemGridContainer").hide();
                    return false;
                }
                else {
                    var isCustomerItem = false, isSupplierItem = false;
                    var isCustomerRSupplierItem = $("#ContentPlaceHolder1_ddlItemType").val();

                    if (isCustomerRSupplierItem == "CustomerItem") {
                        isCustomerItem = true
                    }
                    else if (isCustomerRSupplierItem == "SupplierItem") {
                        isSupplierItem = true;
                    }

                    CommonHelper.SpinnerOpen();
                    PageMethods.CostcenterLocationWiseItemStock(locationId, categoryId, isCustomerItem, isSupplierItem, OnLoadLocationWiseItemStockSucceed, OnLoadLocationWiseItemStockFailed);
                }
            });

            $("#ContentPlaceHolder1_ddlItemType").change(function () {

                if ($(this).val() == "0")
                    return;

                var locationId = $("#ContentPlaceHolder1_ddlStoreLocation").val();
                var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();

                $("#VarianceItemGridContainer").show();
                if (categoryId == "-1") {
                    toastr.warning("Please Select Category.");
                    $("#VarianceItemGridContainer").hide();
                    return false;
                }
                else {
                    if (locationId == "0")
                    { return; }

                    var isCustomerItem = false, isSupplierItem = false;
                    var isCustomerRSupplierItem = $("#ContentPlaceHolder1_ddlItemType").val();

                    if (isCustomerRSupplierItem == "CustomerItem") {
                        isCustomerItem = true
                    }
                    else if (isCustomerRSupplierItem == "SupplierItem") {
                        isSupplierItem = true;
                    }

                    CommonHelper.SpinnerOpen();
                    PageMethods.CostcenterLocationWiseItemStock(locationId, categoryId, isCustomerItem, isSupplierItem, OnLoadLocationWiseItemStockSucceed, OnLoadLocationWiseItemStockFailed);
                }
            });

            $("#ContentPlaceHolder1_ddlCategory").change(function () {

                var locationId = $("#ContentPlaceHolder1_ddlStoreLocation").val();
                var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();

                $("#VarianceItemGridContainer").show();
                if (categoryId == "-1") {
                    toastr.warning("Please Select Category.");
                    $("#VarianceItemGridContainer").hide();
                    return false;
                }
                else {
                    if (locationId == "0")
                    { return; }

                    var isCustomerItem = false, isSupplierItem = false;
                    var isCustomerRSupplierItem = $("#ContentPlaceHolder1_ddlItemType").val();

                    if (isCustomerRSupplierItem == "CustomerItem") {
                        isCustomerItem = true
                    }
                    else if (isCustomerRSupplierItem == "SupplierItem") {
                        isSupplierItem = true;
                    }

                    CommonHelper.SpinnerOpen();
                    PageMethods.CostcenterLocationWiseItemStock(locationId, categoryId, isCustomerItem, isSupplierItem, OnLoadLocationWiseItemStockSucceed, OnLoadLocationWiseItemStockFailed);
                }
            });

        });

        function OnLoadLocationWiseItemStockSucceed(result) {
            if (result != "")
                $("#VarianceItemGridContainer").html(result);

            CommonHelper.SpinnerClose();
        }
        function OnLoadLocationWiseItemStockFailed() { CommonHelper.SpinnerClose(); }

        function ValidationBeforeSave2() {


            var rowCount = $('#ItemVarianceGrid tbody tr').length;
            if (rowCount == 0) {
                toastr.warning('No Product Found.');
                return false;
            }

            var costCenterId = "0", itemId = "0", stockById = "0", previousQuantity = 0.0;
            var quantity = "0", stockVarianceDetailsId = "0", Reason = '', quantityValue = 0, unitPrice = 0.0;
            var isEdit = "0", stockVariancetId = "0", locationId = "0", tModeId = "0", usageQuantity = 0.0, usageCost = 0.0;

            costCenterId = $("#ContentPlaceHolder1_ddlCostCenter2").val();
            locationId = $("#ContentPlaceHolder1_ddlStoreLocation").val();
            stockVariancetId = $("#ContentPlaceHolder1_hfStockAdjustmentId").val();

            companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            if (companyId == null)
                companyId = "0";
            if (projectId == null)
                projectId = "0";

            if (companyId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                toastr.warning("Please Select Company.");
                return false;
            }
            if (projectId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                toastr.warning("Please Select Project.");
                return false;
            }

            var ItemStockVariance = {
                StockVarianceId: stockVariancetId,
                CostCenterId: costCenterId,
                CompanyId: companyId,
                ProjectId: projectId
            };

            var StockVarianceDetails = [], StockVarianceDetailsEdit = [];

            $("#ItemVarianceGrid tbody tr").each(function (index, item) {

                stockVarianceDetailsId = $.trim($(item).find("td:eq(8)").text());
                isEdit = $.trim($(item).find("td:eq(14)").text());

                itemId = $.trim($(item).find("td:eq(9)").text());
                stockById = $(item).find("td:eq(11)").text();
                tModeId = $(item).find("td:eq(5)").find('select option:selected').val();
                Reason = $(item).find("td:eq(7)").find('input').val();

                quantity = $(item).find("td:eq(6)").find('input').val();
                usageQuantity = $(item).find("td:eq(3)").text();
                usageCost = $(item).find("td:eq(4)").text();
                unitPrice = $(item).find("td:eq(14)").text();
                previousQuantity = $(item).find("td:eq(13)").text();

                if (quantity == "")
                    quantity = "0";

                if (parseFloat(quantity) != parseFloat(previousQuantity) && parseFloat(quantity) != 0) {
                    if (stockVarianceDetailsId == "0") {

                        StockVarianceDetails.push({
                            StockVarianceDetailsId: parseInt(stockVarianceDetailsId, 10),
                            StockVarianceId: parseInt(stockVariancetId, 10),
                            ItemId: parseInt(itemId, 10),
                            LocationId: parseInt(locationId, 10),
                            StockById: parseInt(stockById, 10),
                            TModeId: parseInt(tModeId, 10),
                            UsageQuantity: usageQuantity,
                            UnitPrice: unitPrice,
                            UsageCost: usageCost,
                            VarianceQuantity: quantity,
                            PreviousQuantity: parseFloat(quantity),
                            Reason: Reason
                        });
                    }
                    else if (stockVarianceDetailsId != "0" && isEdit == "1") {
                        StockVarianceDetailsEdit.push({
                            StockVarianceDetailsId: parseInt(stockVarianceDetailsId, 10),
                            StockVarianceId: parseInt(stockVariancetId, 10),
                            ItemId: parseInt(itemId, 10),
                            LocationId: parseInt(locationId, 10),
                            StockById: parseInt(stockById, 10),
                            TModeId: parseInt(tModeId, 10),
                            UsageQuantity: usageQuantity,
                            UnitPrice: unitPrice,
                            UsageCost: usageCost,
                            VarianceQuantity: quantity,
                            PreviousQuantity: parseFloat(quantity),
                            Reason: Reason
                        });
                    }
                }
            });

            if (StockVarianceDetails.length == 0) {
                toastr.info("Please Give Variance Quantity");
                return false;
            }

            CommonHelper.SpinnerOpen();
            PageMethods.SaveStockVariance(ItemStockVariance, StockVarianceDetails, StockVarianceDetailsEdit, DeletedAdjustItem, OnSaveStockAdjustmentSucceeded2, OnSaveStockAdjustmentFailed);
            return false;
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
            if (!confirm("Do you want to clear?")) {
                return false;
            }
            PerformClearAction2();
        }

        function PerformClearAction2() {
            $("#ContentPlaceHolder1_ddlCostCenter2").val("0");
            $("#ContentPlaceHolder1_ddlStoreLocation").val("0");

            $("#ItemVarianceGrid tbody").html("");
        }

        function CheckInputValue(vv) { }

    </script>
    <div id="DetailsVarianceProductGridContainer" style="display: none;">
        <table id='ProductVarianceGridDetails' class="table table-bordered table-condensed table-responsive"
            style='width: 100%;'>
            <thead>
                <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                    <th style='width: 20%;'>
                        Product Name
                    </th>
                    <th style='width: 10%;'>
                        Stock By
                    </th>
                    <th style='width: 13%;'>
                        Actual Usage
                    </th>
                    <th style='width: 13%;'>
                        Usage Cost
                    </th>
                    <th style='width: 12%;'>
                        Reason
                    </th>
                    <th style='width: 12%;'>
                        Variance Quantity
                    </th>
                    <th style='width: 20%;'>
                        Remarks
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfStockAdjustmentId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsEditedFromApprovedForm" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Item Wastage</a></li>
            <%--<li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Wastage</a></li>--%>
        </ul>
        <div id="tab-1">
            <div id="StockVarianceTemplate1" runat="server" class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">                        
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCostCenter" runat="server" class="control-label required-field"
                                    Text="Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCostCenter" CssClass="form-control" runat="server"
                                    TabIndex="20">
                                </asp:DropDownList>
                            </div>
                        </div>
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
                                <asp:TextBox ID="txtAdjustmentValue" TabIndex="8" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Reason"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlAdjustmentReason" CssClass="form-control" runat="server"
                                    TabIndex="20">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" style="padding-top: 5px;">
                            <button type="button" id="btnAddAdjustment" tabindex="24" class="TransactionalButton btn btn-primary btn-sm">
                                Add</button>
                        </div>
                        <div class="form-group" style="margin-top: 10px; width: 100%;">
                            <div id="ItemVarianceGridTemplate1Container">
                                <table id='ItemVarianceGridTemplate1' class="table table-bordered table-condensed table-responsive"
                                    style='width: 100%;'>
                                    <thead>
                                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                            <th style='width: 20%;'>
                                                Product Name
                                            </th>
                                            <th style='width: 12%;'>
                                                Location
                                            </th>
                                            <th style='width: 10%;'>
                                                Stock By
                                            </th>
                                            <th style='width: 10%;'>
                                                Quantity
                                            </th>
                                            <th style='width: 10%;'>
                                                Value
                                            </th>
                                            <th style='width: 12%;'>
                                                Allowance
                                            </th>
                                            <th style='width: 18%;'>
                                                Reason
                                            </th>
                                            <th style='width: 7%;'>
                                                Action
                                            </th>
                                            <th style='display: none'>
                                                StockAdjustmentDetailsId
                                            </th>
                                            <th style='display: none'>
                                                ItemId
                                            </th>
                                            <th style='display: none'>
                                                LocationId
                                            </th>
                                            <th style='display: none'>
                                                StockById
                                            </th>
                                            <th style='display: none'>
                                                TransactionModeId
                                            </th>
                                            <th style='display: none'>
                                                DBQuantity
                                            </th>
                                            <th style='display: none'>
                                                IsEdited
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
                                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="25" OnClientClick="javascript:return PerformClearAction();"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="StockVarianceTemplate2" runat="server" class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label9" runat="server" class="control-label required-field" Text="Location"></asp:Label>                                
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlStoreLocation" runat="server" TabIndex="20" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCategory" runat="server" class="control-label required-field" Text="Category"></asp:Label>                                
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCategory" runat="server" TabIndex="5" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label8" runat="server" class="control-label required-field" Text="Item Type"></asp:Label>                               
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-control" TabIndex="5">
                                    <asp:ListItem Text="Customer Item" Value="CustomerItem"></asp:ListItem>
                                    <asp:ListItem Text="Supplier Item" Value="SupplierItem"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div id="VarianceItemGridContainer">
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
            <div style="display: none">
                <div id="SearchEntry" class="panel panel-default">
                    <div class="panel-heading">
                        Item Wastage Information</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label3" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlSearchCostCenter" runat="server" CssClass="form-control"
                                        TabIndex="1">
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
                <div id="SearchPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Search Information</div>
                    <div class="panel-body">
                        <asp:GridView ID="gvFinishedProductInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                            CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" TabIndex="9"
                            OnRowCommand="gvFinishedProductInfo_RowCommand" OnRowDataBound="gvFinishedProductInfo_RowDataBound"
                            CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("StockVarianceId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Adjustment Date" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("StockVarianceDate"))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" ItemStyle-Width="45%">
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
                                            CommandName="CmdDetails" CommandArgument='<%# bind("StockVarianceId") %>' OnClientClick='<%#String.Format("return VarianceProductDetails({0})", Eval("StockVarianceId")) %>'
                                            ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Product Details" />
                                        &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                            CommandArgument='<%# bind("StockVarianceId") %>' OnClientClick='<%#String.Format("return FillForm({0})", Eval("StockVarianceId")) %>'
                                            ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                        &nbsp;<asp:ImageButton ID="ImgDelete" OnClientClick="return confirm('Do you want to delete?');" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                            CommandArgument='<%# bind("StockVarianceId") %>' ImageUrl="~/Images/delete.png"
                                            Text="" AlternateText="Delete" ToolTip="Delete Order" />
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
    </div>
    <div class="divClear">
    </div>
</asp:Content>
