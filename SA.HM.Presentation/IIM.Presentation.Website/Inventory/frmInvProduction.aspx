<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmInvProduction.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmInvProduction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var finisItemEdited = "";
        var DeletedFinishGoods = [];
        var DeletedRMGoods = [];
        var DeletedAccountHead = [];

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Finished Product</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val() == "1") {
                FillForm($("#ContentPlaceHolder1_hfFinishProductId").val());
            }

            $("#ContentPlaceHolder1_ddlCostCenter").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlAccountHead").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#myTabs").tabs();

            var ddlCostCenter = '<%=ddlCostCenter.ClientID%>'
            $('#' + ddlCostCenter).change(function () {
                LoadLocation($('#' + ddlCostCenter).val());
            });

            $('#ContentPlaceHolder1_ddlRMSizeAttribute').change(function () {
                GetRMInvItemStockInfoByItemAndAttributeId();
            });

            $('#ContentPlaceHolder1_ddlRMStyleAttribute').change(function () {
                GetRMInvItemStockInfoByItemAndAttributeId();
            });
            
            $('#ContentPlaceHolder1_ddlRMColorAttribute').change(function () {
                GetRMInvItemStockInfoByItemAndAttributeId();
            });

            $('#ContentPlaceHolder1_ddlFGSizeAttribute').change(function () {
                GetFGInvItemStockInfoByItemAndAttributeId();
            });

            $('#ContentPlaceHolder1_ddlFGStyleAttribute').change(function () {
                GetFGInvItemStockInfoByItemAndAttributeId();
            });

            $('#ContentPlaceHolder1_ddlFGColorAttribute').change(function () {
                GetFGInvItemStockInfoByItemAndAttributeId();
            });

            $("#txtRMItemName").autocomplete({
                source: function (request, response) {
                    var costcenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlRMCategory").val();
                    var locationId = $("#ContentPlaceHolder1_ddlRMLocationId").val();

                    if (costcenterId == "0") {
                        toastr.warning("Please Select Cost Center.");
                        return false;
                    }

                    if (locationId == "0") {
                        toastr.warning("Please Select a Store.");
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmInvProduction.aspx/ItemNCategoryAutoSearchForRMGoods',
                        data: "{'costCenterId':'" + costcenterId + "','itemName':'" + request.term + "','categoryId':'" + categoryId + "', 'locationId':'" + locationId + "'}",
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    UnitHead: m.UnitHead,
                                    StockBy: m.StockBy,
                                    IsAttributeItem: m.IsAttributeItem
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
                    $(this).val(ui.item.label);
                    $("#<%=hfRMItemId.ClientID %>").val(ui.item.value);
                    
                    //var list = result;
                    var ddlRMStockById = '<%=ddlRMItemStockBy.ClientID%>';
                    var control = $('#' + ddlRMStockById);
                    control.empty();
                    control.removeAttr("disabled");
                    control.append('<option title="' + ui.item.UnitHead + '" value="' + ui.item.StockBy + '">' + ui.item.UnitHead + '</option>');

                    GetInvItemRMAttributeByItemIdAndAttributeType(ui.item.value, 'Color');
                    GetInvItemRMAttributeByItemIdAndAttributeType(ui.item.value, 'Size');
                    GetInvItemRMAttributeByItemIdAndAttributeType(ui.item.value, 'Style');
                    GetRMInvItemStockInfoByItemAndAttributeId();

                    $("#AttributeRMDiv").hide();
                    if (ui.item.IsAttributeItem) {
                        $("#AttributeRMDiv").show();
                    }
                }
            });
            
            $("#txtItemName").autocomplete({
                source: function (request, response) {
                    var costcenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    var locationId = $("#ContentPlaceHolder1_ddlFGLocationId").val();

                    if (costcenterId == "0") {
                        toastr.warning("Please Select Cost Center.");
                        return false;
                    }
                    if (locationId == "0") {
                        toastr.warning("Please Select a Store.");
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmInvProduction.aspx/ItemNCategoryAutoSearchForFinishedGoods',
                        data: "{'costCenterId':'" + costcenterId + "','itemName':'" + request.term + "','categoryId':'" + categoryId + "', 'locationId':'" + locationId + "'}",
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    UnitHead: m.UnitHead,
                                    StockBy: m.StockBy,
                                    IsAttributeItem: m.IsAttributeItem
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
                    $(this).val(ui.item.label);
                    $("#<%=hfItemId.ClientID %>").val(ui.item.value);

                    var ddlStockById = '<%=ddlItemStockBy.ClientID%>';
                    var control = $('#' + ddlStockById);
                    control.empty();
                    control.removeAttr("disabled");
                    control.append('<option title="' + ui.item.UnitHead + '" value="' + ui.item.StockBy + '">' + ui.item.UnitHead + '</option>');

                    GetInvItemFGAttributeByItemIdAndAttributeType(ui.item.value, 'Color');
                    GetInvItemFGAttributeByItemIdAndAttributeType(ui.item.value, 'Size');
                    GetInvItemFGAttributeByItemIdAndAttributeType(ui.item.value, 'Style');
                    GetFGInvItemStockInfoByItemAndAttributeId();

                    $("#AttributeFGDiv").hide();
                    if (ui.item.IsAttributeItem) {
                        $("#AttributeFGDiv").show();
                    }
                }
            });

            <%--$("#txtItemName").blur(function () {
                //toastr.info("HIIIIIIIIIIIIIIIIIIIIIII");
                var costCentreId = $("#<%=ddlCostCenter.ClientID %>").val();
                var ProductName = $("#txtItemName").val();
                PageMethods.GetItemNameForAutoSearch(ProductName, costCentreId, OnLoadProductInfoSucceeded, OnLoadProductInfoFailed);
            });--%>

            $("#btnCancel").click(function () {
                return false;
            });

            $("#btnAddRMItem").click(function () {

                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();

                var itemId = $("#ContentPlaceHolder1_hfRMItemId").val();

                var colorddlLength = $('#ContentPlaceHolder1_ddlRMColorAttribute > option').length;
                var sizeddlLength = $('#ContentPlaceHolder1_ddlRMSizeAttribute > option').length;
                var styleddlLength = $('#ContentPlaceHolder1_ddlRMStyleAttribute > option').length;
                var colorId = 0;
                if (colorddlLength > 0) {
                    colorId = parseInt($("#ContentPlaceHolder1_ddlRMColorAttribute option:selected").val(), 10);
                }
                var sizeId = 0;
                if (sizeddlLength > 0) {
                    sizeId = parseInt($("#ContentPlaceHolder1_ddlRMSizeAttribute option:selected").val(), 10);
                }
                var styleId = 0;
                if (styleddlLength > 0) {
                    styleId = parseInt($("#ContentPlaceHolder1_ddlRMStyleAttribute option:selected").val(), 10);
                }

                var quantity = $.trim($("#ContentPlaceHolder1_txtRMItemUnit").val());
                var stockById = $("#ContentPlaceHolder1_ddlRMItemStockBy").val();

                if (costCenterId == "0") {
                    toastr.warning("Please Select a Depo Name.");
                    return false;
                }
                else if (itemId == "0") {
                    toastr.warning("Please select item.");
                    return false;
                }
                else if (stockById == "0") {
                    toastr.warning("Please select item stock by.");
                    return false;
                }
                else if (quantity == "" || quantity == "0") {
                    toastr.warning("Please give item unit.");
                    return false;
                }

                var itemName = "", stockBy = "", locationId = "0", finishedProductDetailsId = "0", isEdited = 0, finishProductId = "0", editedItemId = "0";

                //itemName = $("#txtRMItemName").val();

                itemName = $("#txtRMItemName").val();
                if ($("#ContentPlaceHolder1_ddlRMColorAttribute option:selected").text() != "") {
                    itemName = itemName + " [Color: " + $("#ContentPlaceHolder1_ddlRMColorAttribute option:selected").text();
                    itemName = itemName + ", Size: " + $("#ContentPlaceHolder1_ddlRMSizeAttribute option:selected").text();
                    itemName = itemName + ", Style: " + $("#ContentPlaceHolder1_ddlRMStyleAttribute option:selected").text();
                    itemName = itemName + "]";
                }
                stockBy = $("#ContentPlaceHolder1_ddlRMItemStockBy option:selected").text();
                finishProductId = $("#ContentPlaceHolder1_hfRMProductId").val();
                locationId = $("#ContentPlaceHolder1_ddlRMLocationId").val();

                //if (finisItemEdited != "") {
                //    var editedItemId = $.trim($(finisItemEdited).find("td:eq(6)").text());

                //    if (editedItemId != itemId) {
                //        if ($("#RMProductGrid tbody > tr").find("td:eq(9):contains('" + (costCenterId + "-" + itemId) + "')").length > 0) {
                //            toastr.warning('Same Item Already Added.');
                //            return;
                //        }
                //    }
                //}
                //else {
                //    if ($("#RMProductGrid tbody > tr").find("td:eq(9):contains('" + (costCenterId + "-" + itemId) + "')").length > 0) {
                //        toastr.warning('Same Item Already Added.');
                //        return;
                //    }
                //}

                if (finishProductId != "0" && finisItemEdited != "") {

                    finishedProductDetailsId = $(finisItemEdited).find("td:eq(4)").text();
                    EditItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, locationId, itemId, stockById);
                    //toastr.info("Edit db");
                    return;
                }
                else if (finishProductId == "0" && finisItemEdited != "") {

                    finishedProductDetailsId = $(finisItemEdited).find("td:eq(4)").text();
                    EditItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, locationId, itemId, stockById);
                    toastr.info("Edit");
                    return;
                }

                if (!IsItemExistsForRawMaterials(costCenterId, locationId, itemId, colorId, sizeId, styleId)) {
                    AddRMItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, locationId, itemId, colorId, sizeId, styleId, stockById);

                    if ($("#ContentPlaceHolder1_ddlRMColorAttribute option:selected").text() == "") {
                        $("#ContentPlaceHolder1_hfRMItemId").val("0");
                        $("#txtRMItemName").val("");
                        $("#ContentPlaceHolder1_ddlRMItemStockBy").val("0");
                    }
                    ClearRMFinishGoodsProduct();
                    $("#ContentPlaceHolder1_txtRMItemUnit").val("");
                    $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", true);
                    $("#txtRMItemName").focus();
                }

                $("#ContentPlaceHolder1_lblRMCurrentStock").text("00");
            });

            $("#btnCancelRMOrder").click(function () {
                ClearRMFinishGoodsProduct();
            });

            $("#btnCancelOEAmount").click(function () {
                ClearOverheadExpenseInfo();
            });

            $("#btnAddOEAmount").click(function () {
                
                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
                var accountHead = $("#ContentPlaceHolder1_ddlAccountHead option:selected").text();
                var accountHeadId = $("#ContentPlaceHolder1_ddlAccountHead").val();
                var amount = $.trim($("#ContentPlaceHolder1_txtAmount").val());
                var oEDescription = $.trim($("#ContentPlaceHolder1_txtOEDescription").val());
                if (costCenterId == "0") {
                    toastr.warning("Please Select a Depo Name.");
                    $("#ContentPlaceHolder1_ddlCostCenter").focus();
                    return false;
                }
                else if (accountHeadId == "0") {
                    toastr.warning("Please select Account Head.");
                    $("#ContentPlaceHolder1_ddlAccountHead").focus();
                    return false;
                }
                else if (amount == "") {
                    toastr.warning("Please give Amount.");
                    $("#ContentPlaceHolder1_txtAmount").focus();
                    return false;
                }
                else if (oEDescription == "") {
                    toastr.warning("Please give OverHead Description.");
                    $("#ContentPlaceHolder1_txtOEDescription").focus();
                    return false;
                }

                var AccountHeadDetailsId = "0", isEdited = 0, editedItemId = "0";
                // accountHeadId = $("#ContentPlaceHolder1_hfAccoutHeadId").val();
                
                if (accountHeadId != "0" && finisItemEdited != "") {
                    AccountHeadDetailsId = $(finisItemEdited).find("td:eq(4)").text();
                    EditAccountHeadForOE(accountHead, amount, oEDescription, AccountHeadDetailsId, costCenterId, isEdited);
                    return;
                }
                else if (accountHeadId == "0" && finisItemEdited != "") {
                    AccountHeadDetailsId = $(finisItemEdited).find("td:eq(4)").text();
                    EditAccountHeadForOE(accountHead, amount, oEDescription, AccountHeadDetailsId, costCenterId, isEdited);
                    toastr.info("Edit");
                    return;
                }
                if (!IsAccountHeadExistsForOE(costCenterId, accountHeadId)) {
                    AddAccountHeadForOEInfo(accountHeadId, accountHead, amount, oEDescription, AccountHeadDetailsId, costCenterId, isEdited);

                    $("#ContentPlaceHolder1_txtAmount").val("");
                    $("#ContentPlaceHolder1_txtOEDescription").val("");
                    $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", true);
                    $("#ContentPlaceHolder1_ddlAccountHead").focus();
                }
            });

            $("#btnAddItem").click(function () {

                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
                var itemId = $("#ContentPlaceHolder1_hfItemId").val();

                var colorddlLength = $('#ContentPlaceHolder1_ddlFGColorAttribute > option').length;
                var sizeddlLength = $('#ContentPlaceHolder1_ddlFGSizeAttribute > option').length;
                var styleddlLength = $('#ContentPlaceHolder1_ddlFGStyleAttribute > option').length;
                var colorId = 0;
                if (colorddlLength > 0) {
                    colorId = parseInt($("#ContentPlaceHolder1_ddlFGColorAttribute option:selected").val(), 10);
                }
                var sizeId = 0;
                if (sizeddlLength > 0) {
                    sizeId = parseInt($("#ContentPlaceHolder1_ddlFGSizeAttribute option:selected").val(), 10);
                }
                var styleId = 0;
                if (styleddlLength > 0) {
                    styleId = parseInt($("#ContentPlaceHolder1_ddlFGStyleAttribute option:selected").val(), 10);
                }

                var quantity = $.trim($("#ContentPlaceHolder1_txtItemUnit").val());
                var stockById = $("#ContentPlaceHolder1_ddlItemStockBy").val();

                if (costCenterId == "0") {
                    toastr.warning("Please Select a Depo Name.");
                    return false;
                }
                else if (itemId == "0") {
                    toastr.warning("Please select item.");
                    return false;
                }
                else if (stockById == "0") {
                    toastr.warning("Please select item stock by.");
                    return false;
                }
                else if (quantity == "" || quantity == "0") {
                    toastr.warning("Please give item unit.");
                    return false;
                }

                var unitPrice = $.trim($("#ContentPlaceHolder1_txtUnitPrice").val());
                var bagQuantity = $("#ContentPlaceHolder1_txtBagQuantity").val();

                if (unitPrice == "") {
                    toastr.warning("Please Enter S. Price.");
                    $("#ContentPlaceHolder1_txtUnitPrice").focus();
                    return false;
                }

                if ($("#ContentPlaceHolder1_hfIsRiceMillProduct").val() == "1") {
                    if (bagQuantity == "") {
                        toastr.warning("Please Enter Bag.");
                        $("#ContentPlaceHolder1_txtBagQuantity").focus();
                        return false;
                    }
                }
                else {
                    bagQuantity = "0";
                }

                var itemName = "", stockBy = "", finishedProductDetailsId = "0", isEdited = 0, finishProductId = "0", editedItemId = "0";

                itemName = $("#txtItemName").val();
                if ($("#ContentPlaceHolder1_ddlFGColorAttribute option:selected").text() != "") {
                    itemName = itemName + " [Color: " + $("#ContentPlaceHolder1_ddlFGColorAttribute option:selected").text();
                    itemName = itemName + ", Size: " + $("#ContentPlaceHolder1_ddlFGSizeAttribute option:selected").text();
                    itemName = itemName + ", Style: " + $("#ContentPlaceHolder1_ddlFGStyleAttribute option:selected").text();
                    itemName = itemName + "]";
                }

                stockBy = $("#ContentPlaceHolder1_ddlItemStockBy option:selected").text();
                finishProductId = $("#ContentPlaceHolder1_hfFinishProductId").val();
                locationId = $("#ContentPlaceHolder1_ddlFGLocationId").val();

                if (finishProductId != "0" && finisItemEdited != "") {
                    finishedProductDetailsId = $(finisItemEdited).find("td:eq(4)").text();
                    EditItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, locationId, itemId, stockById);
                    return;
                }
                else if (finishProductId == "0" && finisItemEdited != "") {
                    finishedProductDetailsId = $(finisItemEdited).find("td:eq(4)").text();
                    EditItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, locationId, itemId, stockById);
                    toastr.info("Edit");
                    return;
                }

                if (!IsItemExistsForFinishGoods(costCenterId, locationId, itemId, colorId, sizeId, styleId)) {
                    AddItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, locationId, itemId, colorId, sizeId, styleId, stockById, unitPrice, bagQuantity);

                    if ($("#ContentPlaceHolder1_ddlFGColorAttribute option:selected").text() == "") {
                        $("#ContentPlaceHolder1_hfItemId").val("0");
                        $("#txtItemName").val("");
                        $("#ContentPlaceHolder1_ddlItemStockBy").val("0");
                    }
                    ClearFinishGoodsProduct();
                    $("#ContentPlaceHolder1_txtItemUnit").val("");
                    $("#ContentPlaceHolder1_txtUnitPrice").val("");
                    $("#ContentPlaceHolder1_txtBagQuantity").val("");
                    $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", true);
                    $("#txtItemName").focus();
                }
                $("#ContentPlaceHolder1_lblFGCurrentStock").text("0");
            });

            function GetRMInvItemStockInfoByItemAndAttributeId() {
                var locationId = parseInt($("#ContentPlaceHolder1_ddlRMLocationId").val(), 10);
                var colorddlLength = $('#ContentPlaceHolder1_ddlRMColorAttribute > option').length;
                var sizeddlLength = $('#ContentPlaceHolder1_ddlRMSizeAttribute > option').length;
                var styleddlLength = $('#ContentPlaceHolder1_ddlRMStyleAttribute > option').length;
                var colorId = 0;
                if (colorddlLength > 0) {
                    colorId = parseInt($("#ContentPlaceHolder1_ddlRMColorAttribute option:selected").val(), 10);
                }
                var sizeId = 0;
                if (sizeddlLength > 0) {
                    sizeId = parseInt($("#ContentPlaceHolder1_ddlRMSizeAttribute option:selected").val(), 10);
                }
                var styleId = 0;
                if (styleddlLength > 0) {
                    styleId = parseInt($("#ContentPlaceHolder1_ddlRMStyleAttribute option:selected").val(), 10);
                }

                var companyId = parseInt($("#ContentPlaceHolder1_hfCompanyId").val(), 10);
                var itemId = parseInt($("#ContentPlaceHolder1_hfRMItemId").val(), 10);

                return $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../Inventory/frmInvProduction.aspx/GetInvItemStockInfoByItemAndAttributeIdForPurchase',
                    data: "{'itemId':'" + itemId + "','colorId':'" + colorId + "','sizeId':'" + sizeId + "','styleId':'" + styleId + "','locationId':'" + locationId + "','companyId':'" + companyId + "'}",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.d != null) {
                            var str = data.d.StockQuantity;
                            $("#ContentPlaceHolder1_lblRMCurrentStock").text(str);
                        }
                        else {
                            var str = 0;
                            $("#ContentPlaceHolder1_lblRMCurrentStock").text(str);
                        }
                    },
                    error: function (result) {
                        toastr.error("Please Contact With Admin");
                    }
                });
            }

            function GetFGInvItemStockInfoByItemAndAttributeId() {
                var locationId = parseInt($("#ContentPlaceHolder1_ddlFGLocationId").val(), 10);
                var colorddlLength = $('#ContentPlaceHolder1_ddlFGColorAttribute > option').length;
                var sizeddlLength = $('#ContentPlaceHolder1_ddlFGSizeAttribute > option').length;
                var styleddlLength = $('#ContentPlaceHolder1_ddlFGStyleAttribute > option').length;
                var colorId = 0;
                if (colorddlLength > 0) {
                    colorId = parseInt($("#ContentPlaceHolder1_ddlFGColorAttribute option:selected").val(), 10);
                }
                var sizeId = 0;
                if (sizeddlLength > 0) {
                    sizeId = parseInt($("#ContentPlaceHolder1_ddlFGSizeAttribute option:selected").val(), 10);
                }
                var styleId = 0;
                if (styleddlLength > 0) {
                    styleId = parseInt($("#ContentPlaceHolder1_ddlFGStyleAttribute option:selected").val(), 10);
                }

                var companyId = parseInt($("#ContentPlaceHolder1_hfCompanyId").val(), 10);
                var itemId = parseInt($("#ContentPlaceHolder1_hfItemId").val(), 10);

                return $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../Inventory/frmInvProduction.aspx/GetInvItemStockInfoByItemAndAttributeIdForPurchase',
                    data: "{'itemId':'" + itemId + "','colorId':'" + colorId + "','sizeId':'" + sizeId + "','styleId':'" + styleId + "','locationId':'" + locationId + "','companyId':'" + companyId + "'}",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.d != null) {
                            var str = data.d.StockQuantity;
                            $("#ContentPlaceHolder1_lblFGCurrentStock").text(str);
                        }
                        else {
                            var str = 0;
                            $("#ContentPlaceHolder1_lblFGCurrentStock").text(str);
                        }
                    },
                    error: function (result) {
                        toastr.error("Please Contact With Admin");
                    }
                });
            }

            function IsItemExistsForRawMaterials(costCenterId, locationId, itemId, colorId, sizeId, styleId) {
                var IsDuplicate = false;

                $("#RMProductGrid tr").each(function (index) {

                    if (index !== 0 && !IsDuplicate) {
                        var costCenterIdValueInTable = $(this).find("td").eq(5).html();

                        var locationIdValueInTable = $(this).find("td").eq(10).html();
                        var itemIdValueInTable = $(this).find("td").eq(6).html();

                        var colorIdValueInTable = $(this).find("td").eq(11).html();
                        var sizeIdValueInTable = $(this).find("td").eq(12).html();
                        var styleIdValueInTable = $(this).find("td").eq(13).html();

                        var IsCostCenterIdFound = costCenterIdValueInTable.indexOf(costCenterId) > -1;
                        var IsLocationFound = locationIdValueInTable.indexOf(locationId) > -1;
                        var IsItemFound = itemIdValueInTable.indexOf(itemId) > -1;

                        if (colorId === 0 && sizeId === 0 && styleId === 0) {

                            if (IsCostCenterIdFound && IsLocationFound && IsItemFound) {
                                toastr.warning('Same Item Already Added.');
                                IsDuplicate = true;
                                return true;
                            }
                        }
                        else if (colorId !== 0 && sizeId !== 0 && styleId !== 0) {
                            var IsColorIdFound = colorIdValueInTable.indexOf(colorId) > -1;
                            var IsSizeFound = sizeIdValueInTable.indexOf(sizeId) > -1;
                            var IsStyleFound = styleIdValueInTable.indexOf(styleId) > -1;

                            if (IsCostCenterIdFound && IsLocationFound && IsItemFound && IsColorIdFound && IsSizeFound && IsStyleFound) {
                                toastr.warning('Same Item Already Added.');
                                IsDuplicate = true;
                                return true;
                            }
                        }
                    }
                });

                return IsDuplicate;
            }

            function IsItemExistsForFinishGoods(costCenterId, locationId, itemId, colorId, sizeId, styleId) {
                var IsDuplicate = false;

                $("#FinishedProductGrid tr").each(function (index) {

                    if (index !== 0 && !IsDuplicate) {
                        var costCenterIdValueInTable = $(this).find("td").eq(5).html();

                        var locationIdValueInTable = $(this).find("td").eq(10).html();
                        var itemIdValueInTable = $(this).find("td").eq(6).html();

                        var colorIdValueInTable = $(this).find("td").eq(11).html();
                        var sizeIdValueInTable = $(this).find("td").eq(12).html();
                        var styleIdValueInTable = $(this).find("td").eq(13).html();

                        var IsCostCenterIdFound = costCenterIdValueInTable.indexOf(costCenterId) > -1;
                        var IsLocationFound = locationIdValueInTable.indexOf(locationId) > -1;
                        var IsItemFound = false;
                        if (itemId === itemIdValueInTable) {
                            IsItemFound = true;
                        }
                        //var IsItemFound = itemIdValueInTable.indexOf(itemId) > -1;

                        if (colorId === 0 && sizeId === 0 && styleId === 0) {
                            debugger;
                            if (IsCostCenterIdFound && IsLocationFound && IsItemFound) {
                                toastr.warning('Same Item Already Added.');
                                IsDuplicate = true;
                                return true;
                            }
                        }
                        else if (colorId !== 0 && sizeId !== 0 && styleId !== 0) {
                            var IsColorIdFound = colorIdValueInTable.indexOf(colorId) > -1;
                            var IsSizeFound = sizeIdValueInTable.indexOf(sizeId) > -1;
                            var IsStyleFound = styleIdValueInTable.indexOf(styleId) > -1;

                            if (IsCostCenterIdFound && IsLocationFound && IsItemFound && IsColorIdFound && IsSizeFound && IsStyleFound) {
                                toastr.warning('Same Item Already Added.');
                                IsDuplicate = true;
                                return true;
                            }
                        }
                    }
                });

                return IsDuplicate;
            }

            function IsAccountHeadExistsForOE(costCenterId, accountHeadId) {
                var IsDuplicate = false;
                $("#OEAmountGrid tr").each(function (index) {

                    if (index !== 0 && !IsDuplicate) {
                        var costCenterIdValueInTable = $(this).find("td").eq(5).html();
                        var accountHeadIdValueInTable = $(this).find("td").eq(6).html();

                        var IsCostCenterIdFound = costCenterIdValueInTable.indexOf(costCenterId) > -1;
                        var IsAccountHeadIdFound = accountHeadIdValueInTable.indexOf(accountHeadId) > -1;
                        if (IsCostCenterIdFound && IsAccountHeadIdFound) {
                            toastr.warning('Account Head Already Added.');
                            IsDuplicate = true;
                            return true;
                        }
                    }
                });

                return IsDuplicate;
            }

            $("#btnCancelOrder").click(function () {
                ClearFinishGoodsProduct();
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
        function GetInvItemRMAttributeByItemIdAndAttributeType(itemId, type) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/ItemReceive.aspx/GetInvItemAttributeByItemIdAndAttributeType',
                data: "{'ItemId':'" + itemId + "','attributeType':'" + type + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != null) {
                        if (type == 'Color')
                            OnLoadRMAttributeColorSucceeded(data.d);
                        if (type == 'Size')
                            OnLoadRMAttributeSizeSucceeded(data.d);
                        if (type == 'Style')
                            OnLoadRMAttributeStyleSucceeded(data.d);
                    }
                },
                error: function (result) {
                    toastr.error("Please Contact With Admin");
                }
            });
        }
        function OnLoadRMAttributeColorSucceeded(result) {
            var list = result;
            var ddlRMColorAttributeId = '<%=ddlRMColorAttribute.ClientID%>';
            var control = $('#' + ddlRMColorAttributeId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.attr("disabled", false);
                    //control.removeAttr("disabled");
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {

                    control.attr("disabled", true);
                    //control.removeAttr("disabled");
                }
            }
            return false;
        }
        function OnLoadRMAttributeColorFailed(error) {
        }
        function OnLoadRMAttributeSizeSucceeded(result) {
            var list = result;
            var ddlRMSizeAttributeId = '<%=ddlRMSizeAttribute.ClientID%>';
            var control = $('#' + ddlRMSizeAttributeId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {

                    control.attr("disabled", false);
                    //control.removeAttr("disabled");
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {

                    control.attr("disabled", true);
                    //control.removeAttr("disabled");
                }
            }
            return false;
        }
        function OnLoadRMAttributeSizeFailed(error) {
        }
        function OnLoadRMAttributeStyleSucceeded(result) {
            var list = result;
            var ddlRMStyleAttributeId = '<%=ddlRMStyleAttribute.ClientID%>';
            var control = $('#' + ddlRMStyleAttributeId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {

                    control.attr("disabled", false);
                    //control.removeAttr("disabled");
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.attr("disabled", true);
                }
            }
            return false;
        }
        function OnLoadRMAttributeStyleFailed(error) {
        }
        function GetInvItemFGAttributeByItemIdAndAttributeType(itemId, type) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/ItemReceive.aspx/GetInvItemAttributeByItemIdAndAttributeType',
                data: "{'ItemId':'" + itemId + "','attributeType':'" + type + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != null) {
                        if (type == 'Color')
                            OnLoadFGAttributeColorSucceeded(data.d);
                        if (type == 'Size')
                            OnLoadFGAttributeSizeSucceeded(data.d);
                        if (type == 'Style')
                            OnLoadFGAttributeStyleSucceeded(data.d);
                    }
                },
                error: function (result) {
                    toastr.error("Please Contact With Admin");
                }
            });
        }
        function OnLoadFGAttributeColorSucceeded(result) {
            var list = result;
            var ddlFGColorAttributeId = '<%=ddlFGColorAttribute.ClientID%>';
            var control = $('#' + ddlFGColorAttributeId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.attr("disabled", false);
                    //control.removeAttr("disabled");
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {

                    control.attr("disabled", true);
                    //control.removeAttr("disabled");
                }
            }
            return false;
        }
        function OnLoadFGAttributeColorFailed(error) {
        }
        function OnLoadFGAttributeSizeSucceeded(result) {
            var list = result;
            var ddlFGSizeAttributeId = '<%=ddlFGSizeAttribute.ClientID%>';
            var control = $('#' + ddlFGSizeAttributeId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {

                    control.attr("disabled", false);
                    //control.removeAttr("disabled");
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {

                    control.attr("disabled", true);
                    //control.removeAttr("disabled");
                }
            }
            return false;
        }
        function OnLoadFGAttributeSizeFailed(error) {
        }
        function OnLoadFGAttributeStyleSucceeded(result) {
            var list = result;
            var ddlFGStyleAttributeId = '<%=ddlFGStyleAttribute.ClientID%>';
            var control = $('#' + ddlFGStyleAttributeId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {

                    control.attr("disabled", false);
                    //control.removeAttr("disabled");
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {

                    control.attr("disabled", true);
                    //control.removeAttr("disabled");
                }
            }
            return false;
        }
        function OnLoadFGAttributeStyleFailed(error) {
        }
        function OnLoadProductInfoFailed(error) {
        }

        function LoadLocation(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationToSucceeded, OnLoadLocationFailed);
        }
        function OnLoadLocationToSucceeded(result) {
            $("#ContentPlaceHolder1_hfIsRiceMillProduct").val("0");
            
            $("#ContentPlaceHolder1_hfCompanyId").val(result[0].CompanyId);
            $("#BagLabelDiv").hide();
            $("#BagControlDiv").hide();
            if (result[0].CompanyType == "RiceMill") {
                $("#BagLabelDiv").show();
                $("#BagControlDiv").show();
                $("#ContentPlaceHolder1_hfIsRiceMillProduct").val("1");
            }

            var controlRM = $('#ContentPlaceHolder1_ddlRMLocationId');
            var controlFG = $('#ContentPlaceHolder1_ddlFGLocationId');

            controlRM.empty();
            if (result != null) {
                if (result.length > 0) {
                    if (result.length > 1)
                        controlRM.empty().append('<option value="0">---Please Select---</option>');
                    for (i = 0; i < result.length; i++) {
                        controlRM.append('<option title="' + result[i].Name + '" value="' + result[i].LocationId + '">' + result[i].Name + '</option>');
                    }
                }
                else {
                    controlRM.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }
            if (result.length == 1 && $("#ContentPlaceHolder1_hfRMLocationId").val() == "0")
                controlRM.val($("#ContentPlaceHolder1_ddlRMLocationId option:first").val());
            else
                controlRM.val($("#ContentPlaceHolder1_hfRMLocationId").val());


            controlFG.empty();
            if (result != null) {
                if (result.length > 0) {
                    if (result.length > 1)
                        controlFG.empty().append('<option value="0">---Please Select---</option>');
                    for (i = 0; i < result.length; i++) {
                        controlFG.append('<option title="' + result[i].Name + '" value="' + result[i].LocationId + '">' + result[i].Name + '</option>');
                    }
                }
                else {
                    controlFG.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }
            if (result.length == 1 && $("#ContentPlaceHolder1_hfFGLocationId").val() == "0")
                controlFG.val($("#ContentPlaceHolder1_ddlFGLocationId option:first").val());
            else
                controlFG.val($("#ContentPlaceHolder1_hfFGLocationId").val());

            return false;
        }
        function OnLoadLocationFailed() { }

        function AddRMItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, locationId, itemId, colorId, sizeId, styleId, stockById) {

            var isEdited = "0";
            var rowLength = $("#RMProductGrid tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:65%;'>" + itemName + "</td>";
            tr += "<td style='width:10%;'>" + stockBy + "</td>";
            tr += "<td style='width:15%;'>" + quantity + "</td>";
            tr += "<td style='width:10%;'><a href='javascript:void();' onclick= 'javascript:return DeleteRMItemDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            tr += "<td style='display:none'>" + finishedProductDetailsId + "</td>";
            tr += "<td style='display:none'>" + costCenterId + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + stockById + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "<td style='display:none'>" + (costCenterId + "-" + itemId) + "</td>";
            tr += "<td style='display:none'>" + locationId + "</td>";

            tr += "<td style='display:none'>" + colorId + "</td>";
            tr += "<td style='display:none'>" + sizeId + "</td>";
            tr += "<td style='display:none'>" + styleId + "</td>";

            tr += "</tr>";

            $("#RMProductGrid tbody").append(tr);
        }
        function AddItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, locationId, itemId, colorId, sizeId, styleId, stockById, unitPrice, bagQuantity) {
            var isEdited = "0";
            var rowLength = $("#FinishedProductGrid tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:65%;'>" + itemName + "</td>";
            tr += "<td style='width:10%;'>" + stockBy + "</td>";
            tr += "<td style='width:15%;'>" + quantity + "</td>";
            tr += "<td style='width:10%;'><a href='javascript:void();' onclick= 'javascript:return DeleteItemDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            tr += "<td style='display:none'>" + finishedProductDetailsId + "</td>";
            tr += "<td style='display:none'>" + costCenterId + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + stockById + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "<td style='display:none'>" + (costCenterId + "-" + itemId) + "</td>";
            tr += "<td style='display:none'>" + locationId + "</td>";

            tr += "<td style='display:none'>" + colorId + "</td>";
            tr += "<td style='display:none'>" + sizeId + "</td>";
            tr += "<td style='display:none'>" + styleId + "</td>";

            tr += "<td style='display:none'>" + unitPrice + "</td>";
            tr += "<td style='display:none'>" + bagQuantity + "</td>";

            tr += "</tr>";

            $("#FinishedProductGrid tbody").append(tr);
        }
        function AddAccountHeadForOEInfo(accountHeadId, accountHead, amount, OEDescription, AccountHeadDetailsId, costCenterId, isEdited) {
            var isEdited = "0";
            var rowLength = $("#OEAmountGrid tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:30%;'>" + accountHead + "</td>";
            tr += "<td style='width:10%;'>" + amount + "</td>";
            tr += "<td style='width:50%;'>" + OEDescription + "</td>";
            tr += "<td style='width:10%;'><a href='javascript:void();' onclick= 'javascript:return DeleteAccountHeadOfOE(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            tr += "<td style='display:none'>" + AccountHeadDetailsId + "</td>";
            tr += "<td style='display:none'>" + costCenterId + "</td>";
            tr += "<td style='display:none'>" + accountHeadId + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "</tr>";

            $("#OEAmountGrid tbody").append(tr);
            var totalAmount = 0;
            $("#OEAmountGrid tr").each(function () {
                var amount = $(this).find("td").eq(1).html();
                if(amount == undefined)
                {
                    amount = 0;
                }
                totalAmount = parseFloat(totalAmount) + parseFloat(amount);
            });
            totalAmount = totalAmount.toFixed(2);
            $("#ContentPlaceHolder1_txttotalAmount").val(totalAmount);
        }
        function FIllForEdit(editItem) {

            finisItemEdited = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAdd").val("Update");

            var finishedProductDetailsId = $(tr).find("td:eq(4)").text();

            var itemId = $.trim($(tr).find("td:eq(6)").text());
            var stockById = $(tr).find("td:eq(7)").text();

            var itemName = $(tr).find("td:eq(0)").text();
            var quantity = $(tr).find("td:eq(2)").text();

            $("#ContentPlaceHolder1_hfItemId").val(itemId);
            $("#txtItemName").val(itemName);

            $("#ContentPlaceHolder1_ddlItemStockBy").val(stockById);
            $("#ContentPlaceHolder1_txtItemUnit").val(quantity);
        }

        function FIllForRMEdit(editItem) {
            finisItemEdited = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAddRMItem").val("Update");

            var finishedProductDetailsId = $(tr).find("td:eq(4)").text();

            var itemId = $.trim($(tr).find("td:eq(6)").text());
            var stockById = $(tr).find("td:eq(7)").text();

            var itemName = $(tr).find("td:eq(0)").text();
            var quantity = $(tr).find("td:eq(2)").text();

            $("#ContentPlaceHolder1_hfRMItemId").val(itemId);
            $("#txtRMItemName").val(itemName);

            $("#ContentPlaceHolder1_ddlRMItemStockBy").val(stockById);
            $("#ContentPlaceHolder1_txtRMItemUnit").val(quantity);
        }

        function EditItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, locationId, itemId, stockById) {

            $(finisItemEdited).find("td:eq(0)").text(itemName);
            $(finisItemEdited).find("td:eq(1)").text(stockBy);
            $(finisItemEdited).find("td:eq(2)").text(quantity);

            $(finisItemEdited).find("td:eq(4)").text(finishedProductDetailsId);
            $(finisItemEdited).find("td:eq(5)").text(costCenterId);
            $(finisItemEdited).find("td:eq(6)").text(itemId);
            $(finisItemEdited).find("td:eq(7)").text(stockById);

            if (finishedProductDetailsId != "0")
                $(finisItemEdited).find("td:eq(8)").text("1");

            $(finisItemEdited).find("td:eq(9)").text((costCenterId + "-" + itemId));
            $(finisItemEdited).find("td:eq(10)").text(locationId);

            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#txtItemName").val("");
            $("#ContentPlaceHolder1_ddlItemStockBy").val("0");
            $("#ContentPlaceHolder1_txtItemUnit").val("");

            $("#btnAdd").val("Add");
            finisItemEdited = "";
        }

        function EditAccountHeadForOE(accountHead, amount, OEDescription, AccountHeadDetailsId, costCenterId, isEdited) {
            $(finisItemEdited).find("td:eq(0)").text(accountHead);
            $(finisItemEdited).find("td:eq(1)").text(amount);
            $(finisItemEdited).find("td:eq(2)").text(OEDescription);

            $(finisItemEdited).find("td:eq(4)").text(AccountHeadDetailsId);
            $(finisItemEdited).find("td:eq(5)").text(costCenterId);

            if (AccountHeadDetailsId != "0")
                $(finisItemEdited).find("td:eq(6)").text("1");

            $(finisItemEdited).find("td:eq(7)").text((costCenterId + "-" + accountHead));

            $("#ContentPlaceHolder1_txtAmount").val("");
            $("#ContentPlaceHolder1_txtOEDescription").val("");

            $("#btnAdd").val("Add");
            finisItemEdited = "";
        }

        function DeleteItemDetails(deletedItem) {

            if (!confirm("Do you want to delete?"))
                return;

            var finishProductId = $("#ContentPlaceHolder1_hfFinishProductId").val();

            if (finishProductId != "0") {
                var tr = $(deletedItem).parent().parent();

                DeletedFinishGoods.push({
                    FinishedProductDetailsId: $(tr).find("td:eq(4)").text(),
                    FinishProductId: finishProductId
                });
            }

            $(deletedItem).parent().parent().remove();
        }
                
        function DeleteAccountHeadOfOE(deletedItem) {
            if (!confirm("Do you want to delete?"))
                return;
            var accoutHeadId = $("#ContentPlaceHolder1_hfAccoutHeadId").val();

            if (accoutHeadId != "0") {
                var tr = $(deletedItem).parent().parent();

                DeletedAccountHead.push({
                    AccountHeadDetailsId: $(tr).find("td:eq(4)").text(),
                    AccountHeadId: accoutHeadId
                });
            }

            $(deletedItem).parent().parent().remove();

            var amountAfterDeletion = 0;
            $("#OEAmountGrid tr").each(function (index) {
                var amount = $(this).find("td").eq(1).html();
                if (amount == undefined) {
                    amount = 0;
                }
                amountAfterDeletion = parseFloat(amountAfterDeletion) + parseFloat(amount);
            });
            amountAfterDeletion = amountAfterDeletion.toFixed(2);
            $("#ContentPlaceHolder1_txttotalAmount").val(amountAfterDeletion);
        }

        function DeleteRMItemDetails(deletedItem) {

            if (!confirm("Do you want to delete?"))
                return;

            var finishProductIdRM = $("#ContentPlaceHolder1_hfFinishProductId").val();

            if (finishProductIdRM != "0") {
                var tr = $(deletedItem).parent().parent();

                DeletedRMGoods.push({
                    FinishedProductDetailsId: $(tr).find("td:eq(4)").text(),
                    FinishProductId: finishProductIdRM
                });
            }

            $(deletedItem).parent().parent().remove();
        }

        function ValidationBeforeSave() {
            var rowCountRM = $('#RMProductGrid tbody tr').length;
            if (rowCountRM == 0) {
                toastr.warning('Add atleast one Raw Materials Information.');
                $("#txtRMItemName").focus();
                return false;
            }

            var rowCount = $('#FinishedProductGrid tbody tr').length;
            if (rowCount == 0) {
                toastr.warning('Add atleast one Finished Goods Information.');
                $("#txtItemName").focus();
                return false;
            }

            
            //itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, itemId, stockById

            var costCenterId = "0", itemId = "0", colorId = "0", sizeId = "0", styleId = "0", stockById = "0", unitPrice = 0, bagQuantity = 0;
            var quantity = "0", finishedProductDetailsId = "0"; 
            var isEdit = "0", finishProductId = "0", remarks = "";

            var itemIdRM = "0", colorIdRM = "0", sizeIdRM = "0", styleIdRM = "0", stockByIdRM = "0";
            var quantityRM = "0", finishedProductDetailsIdRM = "0";
            var isEditRM = "0", finishProductIdRM = "0";

            var accountHeadId = "0", amount = "0", description = "", accoutHeadDetailsId = "0";
            var isEditOE = "0", finishProductIdOE = "0";

            costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
            finishProductId = $("#ContentPlaceHolder1_hfFinishProductId").val();
            remarks = $.trim($("#ContentPlaceHolder1_txtRemarks").val());

            if (remarks == "") {
                toastr.warning('Please Enter Description.');
                $("#ContentPlaceHolder1_txtRemarks").focus();
                return false;
            }

            var FinishedProduct = {
                FinishProductId: finishProductId,
                CostCenterId: costCenterId,
                Remarks: remarks
            };

            var AddedRMGoods = [], EditedRMGoods = [];

            $("#RMProductGrid tbody tr").each(function (index, item) {
                finishedProductDetailsIdRM = $.trim($(item).find("td:eq(4)").text());
                isEditRM = $.trim($(item).find("td:eq(8)").text());

                itemIdRM = $.trim($(item).find("td:eq(6)").text());

                colorIdRM = $.trim($(item).find("td:eq(11)").text());
                sizeIdRM = $.trim($(item).find("td:eq(12)").text());
                styleIdRM = $.trim($(item).find("td:eq(13)").text());

                stockByIdRM = $(item).find("td:eq(7)").text();

                quantityRM = $(item).find("td:eq(2)").text();
                rmLocationId = $(item).find("td:eq(10)").text();

                if (finishedProductDetailsIdRM == "0") {

                    AddedRMGoods.push({
                        FinishedProductDetailsId: finishedProductDetailsIdRM,
                        FinishProductId: finishProductId,
                        LocationId: rmLocationId,
                        ItemId: itemIdRM,
                        ColorId: colorIdRM,
                        SizeId: sizeIdRM,
                        StyleId: styleIdRM,
                        StockById: stockByIdRM,
                        Quantity: quantityRM
                    });
                }
                else if (finishedProductDetailsIdRM != "0" && isEditRM != "0") {
                    EditedRMGoods.push({
                        FinishedProductDetailsId: finishedProductDetailsIdRM,
                        FinishProductId: finishProductId,
                        LocationId: rmLocationId,
                        ItemId: itemIdRM,
                        ColorId: colorIdRM,
                        SizeId: sizeIdRM,
                        StyleId: styleIdRM,
                        StockById: stockByIdRM,
                        Quantity: quantityRM
                    });
                }
            });

            var AddedFinishGoods = [], EditedFinishGoods = [];

            $("#FinishedProductGrid tbody tr").each(function (index, item) {

                finishedProductDetailsId = $.trim($(item).find("td:eq(4)").text());
                isEdit = $.trim($(item).find("td:eq(8)").text());

                itemId = $.trim($(item).find("td:eq(6)").text());

                colorId = $.trim($(item).find("td:eq(11)").text());
                sizeId = $.trim($(item).find("td:eq(12)").text());
                styleId = $.trim($(item).find("td:eq(13)").text());

                unitPrice = $.trim($(item).find("td:eq(14)").text());
                bagQuantity = $.trim($(item).find("td:eq(15)").text());

                stockById = $(item).find("td:eq(7)").text();
                quantity = $(item).find("td:eq(2)").text();
                fgLocationId = $(item).find("td:eq(10)").text();

                if (finishedProductDetailsId == "0") {

                    AddedFinishGoods.push({
                        FinishedProductDetailsId: finishedProductDetailsId,
                        FinishProductId: finishProductId,
                        LocationId: fgLocationId,
                        ItemId: itemId,
                        ColorId: colorId,
                        SizeId: sizeId,
                        StyleId: styleId,
                        StockById: stockById,
                        Quantity: quantity,
                        UnitPrice: unitPrice,
                        BagQuantity: bagQuantity
                    });
                }
                else if (finishedProductDetailsId != "0" && isEdit != "0") {
                    EditedFinishGoods.push({
                        FinishedProductDetailsId: finishedProductDetailsId,
                        FinishProductId: finishProductId,
                        LocationId: fgLocationId,
                        ItemId: itemId,
                        ColorId: colorId,
                        SizeId: sizeId,
                        StyleId: styleId,
                        StockById: stockById,
                        Quantity: quantity,
                        UnitPrice: unitPrice,
                        BagQuantity: bagQuantity
                    });
                }
            });

            var AddedOverheadExpenses = [], EditedOverheadExpenses = [];
            $("#OEAmountGrid tbody tr").each(function (index, item) {
                accoutHeadDetailsId = $.trim($(item).find("td:eq(4)").text());
                accountHeadId = $(item).find("td:eq(6)").text();
                amount = $(item).find("td:eq(1)").text();
                description = $(item).find("td:eq(2)").text();
                costCenterId = $(item).find("td:eq(5)").text();
                isEditOE = $(item).find("td:eq(7)").text();
                if (accoutHeadDetailsId == "0") {
                    AddedOverheadExpenses.push({
                        FinishedProductDetailsId: accoutHeadDetailsId,
                        FinishProductId: finishProductId,
                        NodeId: accountHeadId,
                        Amount: amount,
                        Remarks: description,
                        CostCenterId: costCenterId
                    });
                }
            });
            PageMethods.SaveFinishGoods(FinishedProduct, AddedRMGoods, EditedRMGoods, DeletedRMGoods, AddedFinishGoods, EditedFinishGoods, DeletedFinishGoods, AddedOverheadExpenses, OnSaveFinishGoodsSucceeded, OnSaveFinishGoodsFailed);

            return false;
        }
        function OnSaveFinishGoodsSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //toastr.success('Save Operation Successful.');
                PerformClearAction();
                //GoToProductionPage();
            }
            else {
                //toastr.warning('Save Operation Failed.');
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnSaveFinishGoodsFailed(error) { toastr.error(error.get_message()); }
        function FinishProductApproval(productionId) {
            PageMethods.FinishProductApproval(productionId, OnFinishProductApprovalSucceeded, OnFinishProductApprovalFailed);
            return false;
        }
        function OnFinishProductApprovalSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GoToProductionPage();
            }
        }
        function GoToProductionPage() {
            window.location = "/Inventory/frmInvProduction.aspx";
            return false;
        }
        function OnFinishProductApprovalFailed(result) { }
        
        function FinishProductCheck(productionId) {
            PageMethods.FinishProductCheck(productionId, OnFinishProductCheckSucceded, OnFinishProductCheckFailed);
            return false;
        }
        function OnFinishProductCheckSucceded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GoToProductionPage();
            }
        }
        function OnFinishProductCheckFailed(result) {

        }

        function FinishProductDetails(finishProductId) {
            PageMethods.GetFinishProductDetails(finishProductId, OnFinishProductLoadSucceeded, OnSaveFinishGoodsFailed);
            return false;
        }
        function OnFinishProductLoadSucceeded(result) {

            $("#DetailsFinishProductGrid tbody").html("");
            var totalRow = result.length, row = 0;

            var tr = "";

            for (row = 0; row < totalRow; row++) {

                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:40%;'>" + result[row].ProductName + "</td>";
                tr += "<td style='width:25%;'>" + result[row].StockBy + "</td>";
                tr += "<td style='width:25%;'>" + result[row].Quantity + "</td>";

                tr += "</tr>";

                $("#DetailsFinishProductGrid tbody").append(tr);
                tr = "";
            }

            $("#DetailsFinishProductGridContainer").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 800,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Finish Product Details",
                show: 'slide'
            });
        }
        function ConfirmDeletion() {
            if (!confirm("Do You Want To Delete?")) {
                return false;
            }
        }

        function FillForm(finishProductId) {
            if (!confirm("Do You Want To Edit?")) {
                return false;
            }
            PageMethods.FillForm(finishProductId, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {
            if (result != null) {
                PerformClearAction();
                $("#ContentPlaceHolder1_ddlCostCenter").val(result.FinishedProduct.CostCenterId).trigger('change');
                $("#ContentPlaceHolder1_hfFinishProductId").val(result.FinishedProduct.ProductId);
                $("#ContentPlaceHolder1_txtRemarks").val(result.FinishedProduct.Remarks);

                var ddlCostCenter = '<%=ddlCostCenter.ClientID%>'
                LoadLocation($('#' + ddlCostCenter).val());

                $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlRMLocationId").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlFGLocationId").attr("disabled", true);

                var rowLengthRM = result.FinisProductRMDetails.length;
                var rowRM = 0;

                for (rowRM = 0; rowRM < rowLengthRM; rowRM++) {
                    var itemName = "";
                    itemName = result.FinisProductRMDetails[rowRM].ItemCode + " - " + result.FinisProductRMDetails[rowRM].ItemName + " [Color: " + result.FinisProductRMDetails[rowRM].ColorName;
                    itemName = itemName + ", Size: " + result.FinisProductRMDetails[rowRM].SizeName;
                    itemName = itemName + ", Style: " + result.FinisProductRMDetails[rowRM].StyleName;
                    itemName = itemName + "]";

                    AddRMItemForFinishGoods(itemName, result.FinisProductRMDetails[rowRM].UnitName, result.FinisProductRMDetails[rowRM].Quantity,
                                    result.FinisProductRMDetails[rowRM].FinishedProductDetailsId, result.FinishedProduct.CostCenterId, result.FinisProductRMDetails[rowRM].LocationId, result.FinisProductRMDetails[rowRM].ItemId,
                                    result.FinisProductRMDetails[rowRM].ColorId, result.FinisProductRMDetails[rowRM].SizeId, result.FinisProductRMDetails[rowRM].StyleId, result.FinisProductRMDetails[rowRM].StockById);
                }

                var rowLength = result.FinisProductDetails.length;
                var row = 0;

                for (row = 0; row < rowLength; row++) {
                    var itemName = "";
                    itemName = result.FinisProductDetails[row].ItemCode + " - " + result.FinisProductDetails[row].ItemName + " [Color: " + result.FinisProductDetails[row].ColorName;
                    itemName = itemName + ", Size: " + result.FinisProductDetails[row].SizeName;
                    itemName = itemName + ", Style: " + result.FinisProductDetails[row].StyleName;
                    itemName = itemName + "]";
                    
                    AddItemForFinishGoods(itemName, result.FinisProductDetails[row].UnitName, result.FinisProductDetails[row].Quantity, result.FinisProductDetails[row].FinishedProductDetailsId, 
                        result.FinishedProduct.CostCenterId, result.FinisProductDetails[row].LocationId, result.FinisProductDetails[row].ItemId, result.FinisProductDetails[row].ColorId, 
                        result.FinisProductDetails[row].SizeId, result.FinisProductDetails[row].StyleId, result.FinisProductDetails[row].StockById, result.FinisProductDetails[row].UnitPrice, result.FinisProductDetails[row].BagQuantity);
                }

                var rowLengthOE = result.FinishProductOEDetails.length;
                var rowOE = 0;

                for (rowOE = 0; rowOE < rowLengthOE; rowOE++) {
                    var isEdited = "";
                    AddAccountHeadForOEInfo(result.FinishProductOEDetails[rowOE].NodeId, result.FinishProductOEDetails[rowOE].AccountHead, result.FinishProductOEDetails[rowOE].Amount, result.FinishProductOEDetails[rowOE].Remarks, 
                        result.FinishProductOEDetails[rowOE].FinishedProductDetailsId, result.FinishedProduct.CostCenterId, isEdited);
                }

                $("#ContentPlaceHolder1_btnSave").val("Update");
                //$("#myTabs").tabs('select', 0);
                $("#myTabs").tabs({ active: 0 });
            }
        }
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
        }

        function ClearRMFinishGoodsProduct() {

            $("#btnAddRMItem").val("Add");

            $("#ContentPlaceHolder1_ddlRMCategory").val("0");
            $("#txtRMItemName").val("");
            $("#AttributeRMDiv").hide();
            $("#ContentPlaceHolder1_ddlRMItemStockBy").val("");
            $("#ContentPlaceHolder1_txtRMItemUnit").val("");
            $("#ContentPlaceHolder1_ddlRMLocationId").attr("disabled", false);
            $("#ContentPlaceHolder1_lblRMCurrentStock").text("0");

            $("#ContentPlaceHolder1_hfRMItemId").val("0");

            if ($("#RMProductGrid tbody tr").length == 0 && $("#ContentPlaceHolder1_hfRMProductId").val() == "0") {
                $("#ContentPlaceHolder1_ddlCostCenter").val("0").trigger('change');
                $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", false);
            }

            return false;
        }
        function ClearFinishGoodsProduct() {
            $("#btnAddItem").val("Add");
            $("#ContentPlaceHolder1_ddlCategory").val("0");
            $("#txtItemName").val("");
            $("#AttributeFGDiv").hide();
            $("#ContentPlaceHolder1_ddlItemStockBy").val("");
            $("#ContentPlaceHolder1_txtItemUnit").val("");
            $("#ContentPlaceHolder1_txtUnitPrice").val("");
            $("#ContentPlaceHolder1_txtBagQuantity").val("");
            $("#ContentPlaceHolder1_lblFGCurrentStock").text("0");
            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_ddlFGLocationId").attr("disabled", false);

            if ($("#FinishedProductGrid tbody tr").length == 0 && $("#ContentPlaceHolder1_hfFinishProductId").val() == "0") {
                $("#ContentPlaceHolder1_ddlCostCenter").val("0").trigger('change');
                $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", false);
            }

            return false;
        }

        function ClearOverheadExpenseInfo() {
            $("#ContentPlaceHolder1_ddlAccountHead").val("0");
            $("#ContentPlaceHolder1_txtAmount").val("");
            $("#ContentPlaceHolder1_txtOEDescription").val("");

            if ($("#OEAmountGrid tbody tr").length == 0 && $("#ContentPlaceHolder1_hfAccoutHeadId").val() == "0") {
                $("#ContentPlaceHolder1_ddlCostCenter").val("0");
                $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", false);
            }
            return false;
        
        }

        function PerformClearForSearchTab() {
            $("#ContentPlaceHolder1_ddlSearchCostCenter").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");
            $("#ContentPlaceHolder1_txtProductionId").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("0").trigger('change');
        }

        function PerformClearAction() {
            $("#RMProductGrid tbody").html("");
            $("#FinishedProductGrid tbody").html("");
            $("#OEAmountGrid tbody").html("");
            $("#ContentPlaceHolder1_btnSave").val("Save");
            $("#ContentPlaceHolder1_txttotalAmount").val("0");
            $("#ContentPlaceHolder1_ddlAccountHead").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtAmount").val("");
            $("#ContentPlaceHolder1_txtOEDescription").val("");

            $("#ContentPlaceHolder1_ddlCostCenter").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlCategory").val("0");
            $("#txtItemName").val("");
            $("#AttributeFGDiv").hide();
            $("#ContentPlaceHolder1_ddlItemStockBy").val("");
            $("#ContentPlaceHolder1_txtItemUnit").val("");
            $("#ContentPlaceHolder1_txtUnitPrice").val("");

            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_hfFinishProductId").val("0");

            $("#ContentPlaceHolder1_lblFGCurrentStock").text("0");
            $("#ContentPlaceHolder1_lblRMCurrentStock").text("0");
            
            $("#ContentPlaceHolder1_hfAccoutHeadId").val("0");

            $("#ContentPlaceHolder1_ddlRMCategory").val("0");
            $("#txtRMItemName").val("");
            $("#AttributeRMDiv").hide();
            $("#ContentPlaceHolder1_ddlRMItemStockBy").val("");
            $("#ContentPlaceHolder1_txtRMItemUnit").val("");

            $("#ContentPlaceHolder1_hfRMItemId").val("0");
            $("#ContentPlaceHolder1_hfRMFinishProductId").val("0");

            $("#ContentPlaceHolder1_txtRemarks").val("");

            $("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val("0");

            $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlRMLocationId").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlFGLocationId").attr("disabled", false);

            return false;
        }
    </script>
    <div id="DetailsFinishProductGridContainer" style="display: none;">
        <table id="DetailsFinishProductGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 40%;">Product Name
                    </th>
                    <th style="width: 25%;">Stock By
                    </th>
                    <th style="width: 25%;">Quantity
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfFinishProductId" runat="server" Value="0" />
    <asp:HiddenField ID="hfAccoutHeadId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsEditedFromApprovedForm" runat="server" Value="0" />
    <asp:HiddenField ID="hfRMItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfRMProductId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsRiceMillProduct" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsRMEditedFromApprovedForm" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Production</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Production</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCostCenter" runat="server" class="control-label required-field"
                                Text="Cost Center"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlCostCenter" CssClass="form-control" runat="server" TabIndex="20">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-6">
                            <div id="RMEntryPanel" class="panel panel-default">
                                <div class="panel-heading">
                                    Raw Materials Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Store"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:HiddenField ID="hfRMLocationId" runat="server" Value="0" />
                                                <asp:DropDownList ID="ddlRMLocationId" runat="server" CssClass="form-control" TabIndex="20">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group" style="display: none">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblRMCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:DropDownList ID="ddlRMCategory" runat="server" CssClass="form-control" TabIndex="20">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblRMItemName" runat="server" class="control-label required-field" Text="Item"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtRMItemName" CssClass="form-control" TabIndex="21" runat="server"
                                                    ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="AttributeRMDiv" style="display: none;">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label15" runat="server" class="control-label" Text="Color"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlRMColorAttribute" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label16" runat="server" class="control-label" Text="Size"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlRMSizeAttribute" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label17" runat="server" class="control-label" Text="Style"></asp:Label>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:DropDownList ID="ddlRMStyleAttribute" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblRMItemStockBy" runat="server" class="control-label required-field"
                                                    Text="Stock"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlRMItemStockBy" runat="server" CssClass="form-control" TabIndex="22">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblRMItemUnit" runat="server" class="control-label required-field" Text="Unit"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtRMItemUnit" TabIndex="23" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label7" runat="server" class="control-label" Text="C. Stock"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="lblRMCurrentStock" runat="server" class="form-control" Text="0"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row" style="padding: 5px 0 5px 0;">
                                            <div class="col-md-12">
                                                <button type="button" id="btnAddRMItem" tabindex="24" class="TransactionalButton btn btn-primary btn-sm">
                                                    Add</button>
                                                <button type="button" id="btnCancelRMOrder" tabindex="24" class="TransactionalButton btn btn-primary btn-sm">
                                                    Cancel</button>
                                            </div>
                                        </div>
                                        <div class="form-group" style="padding: 0px;">
                                            <div id="RMProductGridContainer">
                                                <table id="RMProductGrid" class="table table-bordered table-condensed table-responsive"
                                                    style="width: 100%;">
                                                    <thead>
                                                        <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                                            <th style="width: 40%;">Product Name
                                                            </th>
                                                            <th style="width: 25%;">Stock By
                                                            </th>
                                                            <th style="width: 25%;">Quantity
                                                            </th>
                                                            <th style="width: 10%;">Action
                                                            </th>
                                                            <th style="display: none">FinishedPrductDetailsId
                                                            </th>
                                                            <th style="display: none">CostCenterId
                                                            </th>
                                                            <th style="display: none">ProductId
                                                            </th>
                                                            <th style="display: none">StockById
                                                            </th>
                                                            <th style="display: none">Is Edited
                                                            </th>
                                                            <th style="display: none">duplicateCheck
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div id="EntryPanel" class="panel panel-default">
                                <div class="panel-heading">
                                    Finished Goods Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Store"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:HiddenField ID="hfFGLocationId" runat="server" Value="0" />
                                                <asp:DropDownList ID="ddlFGLocationId" runat="server" CssClass="form-control" TabIndex="20">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group" style="display: none">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" TabIndex="20">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblItemName" runat="server" class="control-label required-field" Text="Item"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtItemName" CssClass="form-control" TabIndex="21" runat="server"
                                                    ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="AttributeFGDiv" style="display: none;">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label4" runat="server" class="control-label" Text="Color"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlFGColorAttribute" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label5" runat="server" class="control-label" Text="Size"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlFGSizeAttribute" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <asp:Label ID="Label6" runat="server" class="control-label" Text="Style"></asp:Label>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:DropDownList ID="ddlFGStyleAttribute" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblItemStockBy" runat="server" class="control-label required-field"
                                                    Text="Stock"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlItemStockBy" runat="server" CssClass="form-control" TabIndex="22">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label ID="lblItemUnit" runat="server" class="control-label required-field" Text="Unit"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtItemUnit" TabIndex="23" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label8" runat="server" class="control-label" Text="C. Stock"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="lblFGCurrentStock" runat="server" class="form-control" Text="0"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label18" runat="server" class="control-label required-field" Text="S. Price"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2" id="BagLabelDiv" style="display: none;">
                                                <asp:Label ID="Label19" runat="server" class="control-label required-field" Text="Bag"></asp:Label>
                                            </div>
                                            <div class="col-md-4" id="BagControlDiv" style="display: none;">
                                                <asp:TextBox ID="txtBagQuantity" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row" style="padding: 5px 0 5px 0;">
                                            <div class="col-md-12">
                                                <button type="button" id="btnAddItem" tabindex="24" class="TransactionalButton btn btn-primary btn-sm">
                                                    Add</button>
                                                <button type="button" id="btnCancelOrder" tabindex="24" class="TransactionalButton btn btn-primary btn-sm">
                                                    Cancel</button>
                                            </div>
                                        </div>
                                        <div class="form-group" style="padding: 0px;">
                                            <div id="FinishedProductGridContainer">
                                                <table id="FinishedProductGrid" class="table table-bordered table-condensed table-responsive"
                                                    style="width: 100%;">
                                                    <thead>
                                                        <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                                            <th style="width: 40%;">Product Name
                                                            </th>
                                                            <th style="width: 25%;">Stock By
                                                            </th>
                                                            <th style="width: 25%;">Quantity
                                                            </th>
                                                            <th style="width: 10%;">Action
                                                            </th>
                                                            <th style="display: none">FinishedPrductDetailsId
                                                            </th>
                                                            <th style="display: none">CostCenterId
                                                            </th>
                                                            <th style="display: none">ProductId
                                                            </th>
                                                            <th style="display: none">StockById
                                                            </th>
                                                            <th style="display: none">Is Edited
                                                            </th>
                                                            <th style="display: none">duplicateCheck
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div id="OEEntryPanel" class="panel panel-default">
                            <div class="panel-heading">
                                Overhead Expense Information
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblAccountHead" runat="server" class="control-label required-field" Text="Account Head"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlAccountHead" runat="server" CssClass="form-control" TabIndex="20">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblAmount" runat="server" class="control-label required-field" Text="Amount"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblOEDescription" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtOEDescription" runat="server" TabIndex="22" CssClass="form-control"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 5px 0 5px 0;">
                                        <div class="col-md-12">
                                            <button type="button" id="btnAddOEAmount" tabindex="24" class="TransactionalButton btn btn-primary btn-sm">
                                                Add</button>
                                            <button type="button" id="btnCancelOEAmount" tabindex="24" class="TransactionalButton btn btn-primary btn-sm">
                                                Cancel</button>
                                        </div>
                                    </div>
                                    <div class="form-group" style="padding: 0px;">
                                        <div id="OEAmountGridContainer">
                                            <table id="OEAmountGrid" class="table table-bordered table-condensed table-responsive"
                                                style="width: 100%;">
                                                <thead>
                                                    <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                                        <th style="width: 30%;">Account Head
                                                        </th>
                                                        <th style="width: 10%;">Amount
                                                        </th>
                                                        <th style="width: 50%">Description
                                                        </th>
                                                        <th style="width: 10%;">Action
                                                        </th>
                                                        <th style="display: none">AccountHeadDetailsId
                                                        </th>
                                                        <th style="display: none">CostCenterId
                                                        </th>
                                                        <th style="display: none">AccountHeadId
                                                        </th>
                                                        <th style="display: none">Is Edited
                                                        </th>
                                                        <th style="display: none">DuplicateCheck
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                            <div class="row" style="padding: 5px 0 5px 0;">
                                                <div class="col-md-3">
                                                    <asp:Label ID="lbltotalAmount" runat="server" class="control-label" Text="Total Amount :"></asp:Label>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:TextBox ID="txttotalAmount" runat="server" ReadOnly="true" CssClass="form-control quantitydecimal"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblRemarks" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRemarks" runat="server" TabIndex="6" CssClass="form-control"
                                TextMode="MultiLine"></asp:TextBox>
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
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Finished Product Information
                </div>
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
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="Date"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" placeholder="From Date" runat="server" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" placeholder="To Date" runat="server" TabIndex="4"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblProductionId" runat="server" class="control-label" Text="Production Id"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtProductionId" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">                            
                            <div class="col-md-2">
                                <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Text="--- All ---" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                    <asp:ListItem Text="Checked" Value="Checked"></asp:ListItem>
                                    <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                    <asp:ListItem Text="Cancel" Value="Cancel"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearForSearchTab();" TabIndex="6" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading" pagersettings-mode="NumericFirstLast">
                    Search Information
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvFinishedProductInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" TabIndex="9"
                        AllowPaging ="true" PageSize ="30" OnPageIndexChanging="gvFinishedProductInfo_PageIndexChanging"
                        OnRowCommand="gvFinishedProductInfo_RowCommand" OnRowDataBound="gvFinishedProductInfo_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="P.Id">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ProductionDateDisplay" HeaderText="Date" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ApprovedStatus" HeaderText="Status" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgEdit" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("Id") %>' OnClientClick='<%#String.Format("return FillForm({0})", Eval("Id")) %>'
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit Order" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("Id") %>' OnClientClick='<%#String.Format("return ConfirmDeletion()") %>'
                                        ImageUrl="~/Images/delete.png" Text="" AlternateText="Delete" ToolTip="Delete Order" />
                                    &nbsp;<asp:ImageButton ID="ImgCheck" runat="server" CausesValidation="False"
                                        CommandName="CmdCheck" CommandArgument='<%# bind("OrderDate") %>' OnClientClick='<%#String.Format("return FinishProductCheck({0})", Eval("Id")) %>'
                                        ImageUrl="~/Images/checked.png" Text="" AlternateText="Check" ToolTip="Check" />
                                    &nbsp;<asp:ImageButton ID="ImgApproval" runat="server" CausesValidation="False"
                                        CommandName="CmdApproval" CommandArgument='<%# bind("OrderDate") %>' OnClientClick='<%#String.Format("return FinishProductApproval({0})", Eval("Id")) %>'
                                        ImageUrl="~/Images/approved.png" Text="" AlternateText="Approval" ToolTip="Approval" />
                                    &nbsp;&nbsp;<asp:ImageButton ID="ImgReportPO" runat="server" CausesValidation="False"
                                        CommandName="CmdReportRI" CommandArgument='<%# bind("Id") %>' ImageUrl="~/Images/ReportDocument.png"
                                        Text="" AlternateText="Invoice" ToolTip="Production Report" />
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                        <%--<PagerSettings Mode="NextPrevious" PageButtonCount="4" PreviousPageText="Previous" NextPageText="Next" 
                            
                             BackColor="#666666" ForeColor="White" HorizontalAlign="Center"
                            
                            />--%>
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last"/>
                        <%--<PagerSettings Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev" />--%>
                        <PagerStyle CssClass="ProductionPagination" />
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
    <div class="divClear">
    </div>
</asp:Content>
