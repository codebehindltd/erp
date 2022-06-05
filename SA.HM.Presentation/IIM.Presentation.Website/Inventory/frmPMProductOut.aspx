<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmPMProductOut.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmPMProductOut" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------

        var outItemEdited = "";
        var DeletedOutDetails = new Array();
        var RequisitionProductDetails = new Array();
        var itemStockValidation = 0;
        var SellingProductDetails = new Array();

        $(document).ready(function () {

            var ddlBillNumber = '<%=ddlBillNumber.ClientID%>'
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product Out</li>";
            var breadCrumbs = moduleName + formName;

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfProductForSales").val() != "") {
                SellingProductDetails = JSON.parse($("#ContentPlaceHolder1_hfProductForSales").val());
            }
            
            $("#RequisitionContainer").hide();
            $("#SalesOutContainer").hide();
            $("#ItemSerialInfoDiv").hide();

            $("#RequisitionContainer").show();
            $("#SalesOutContainer").hide();
            $("#ProductForInternalOut").hide();
            $("#InternalOutLocationTo").show();

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlProduct").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSalesOrderProduct").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSerialNumber").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlBillNumber").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSalesOrderProduct").change(function () {
                
                var prd = _.findWhere(SellingProductDetails, { ItemId: parseInt($(this).val()) });

                if (prd != null) {
                    $("#ContentPlaceHolder1_ddlStockBy").val(prd.StockBy + "");

                    if (prd.ProductType == 'Serial Product') {
                        $("#ContentPlaceHolder1_hfIsSerializableItem").val("1");
                        $("#ContentPlaceHolder1_hfSerialNumber").val("");
                        LoadSerialNumberByProductId($(this).val(), 0);
                        $("#<%=txtQuantity_Serial.ClientID %>").val("1");
                        $("#ItemQuantityInfoDiv").hide();
                        $("#ItemSerialInfoDiv").show();
                    }
                    else {

                        $("#ItemQuantityInfoDiv").show();
                        $("#ItemSerialInfoDiv").hide();
                        $("#ContentPlaceHolder1_hfIsSerializableItem").val("0");
                        $("#ContentPlaceHolder1_ddlSerialNumber").empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    }
                }
                else {
                    $("#ContentPlaceHolder1_ddlStockBy").val(prd.StockBy + "");
                    $("#ContentPlaceHolder1_hfIsSerializableItem").val("0");
                    $("#ContentPlaceHolder1_ddlSerialNumber").empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            });

            $("#ContentPlaceHolder1_ddlProductOutFor").change(function () {

                $("#ContentPlaceHolder1_ddlBillNumber").val("0");
                $("#ContentPlaceHolder1_ddlRequisition").val("0");
                $("#ContentPlaceHolder1_ddlProduct").val("0");
                $("#ContentPlaceHolder1_ddlCostCenterTo").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlCostCenterTo").val("0");

                if ($(this).val() == "Internal") {
                    $("#RequisitionContainer").hide();
                    $("#SalesOutContainer").hide();
                    $("#ProductForInternalOut").show();
                    $("#InternalOutLocationTo").show();
                    $("#CostCenterToDiv").show();
                }
                else if ($(this).val() == "Requisition") {
                    $("#RequisitionContainer").show();
                    $("#SalesOutContainer").hide();
                    $("#ProductForInternalOut").hide();
                    $("#InternalOutLocationTo").show();
                    $("#CostCenterToDiv").show();
                }
                else if ($(this).val() == "Receive") {
                    $("#RequisitionContainer").hide();
                    $("#SalesOutContainer").hide();
                    $("#ProductForInternalOut").show();
                    $("#InternalOutLocationTo").show();
                    $("#CostCenterToDiv").show();
                }
                else if ($(this).val() == "Sales") {
                    $("#RequisitionContainer").hide();
                    $("#SalesOutContainer").show();
                    $("#ProductForInternalOut").hide();
                    $("#InternalOutLocationTo").hide();
                    $("#CostCenterToDiv").hide();
                }

                if ($(this).val() != "Sales") {
                    $("#ItemSerialInfoDiv").hide();
                }
                else {
                    $("#ItemSerialInfoDiv").show();
                }

            });

            $("#ContentPlaceHolder1_ddlRequisition").change(function () {
                $('#ContentPlaceHolder1_hfItemId').val("0");
                LoadRequisitionProductList();
            });

            $("#ContentPlaceHolder1_ddlCostCenterFrom").change(function () {
                LoadLocationFrom($(this).val());
            });

            $("#ContentPlaceHolder1_ddlCostCenterTo").change(function () {
                LoadLocationTo($(this).val());
            });

            $("#ContentPlaceHolder1_ddlProduct").change(function () {

                if ($(this).val() == "0")
                    return;

                var productDetails = _.findWhere(RequisitionProductDetails, { ItemId: parseInt($(this).val(), 10) });

                if (productDetails != null) {
                    $("#ContentPlaceHolder1_ddlStockBy").val(productDetails.StockById);
                    $("#ContentPlaceHolder1_lblPOQuantity").val(productDetails.ApprovedQuantity);
                    $("#ContentPlaceHolder1_lblOutQuantity").val(productDetails.DeliveredQuantity);
                    if (productDetails.DeliveredQuantity == 0) {
                        $("#ContentPlaceHolder1_txtQuantity_Serial").val(productDetails.ApprovedQuantity);
                    }
                }

            });

            $("#btnAdd").click(function () {

                var serialNumber = "", quantity = "";
                var IsSerialProduct = $("#ContentPlaceHolder1_hfIsSerializableItem").val();

                if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Sales" && IsSerialProduct == "1") {
                    quantity = "1";
                    serialNumber = $("#ContentPlaceHolder1_ddlSerialNumber option:selected").text();
                }
                else if (IsSerialProduct == "1") {
                    quantity = "1";
                    serialNumber = $("#ContentPlaceHolder1_txtQuantity_Serial").val();
                }
                else {
                    quantity = $("#ContentPlaceHolder1_txtQuantity_Serial").val();
                    serialNumber = "";
                }

                var IsValidSerial = "0", duplicateSerial = 0;

                var costCenterIdFrom = $('#ContentPlaceHolder1_ddlCostCenterFrom').val();
                var costCenterIdTo = $('#ContentPlaceHolder1_ddlCostCenterTo').val();
                var stockById = $("#ContentPlaceHolder1_ddlStockBy").val();

                var locationIdFrom = $("#ContentPlaceHolder1_ddlLocationFrom").val();
                var locationIdTo = $("#ContentPlaceHolder1_ddlLocationTo").val();

                if (locationIdTo == null)
                    locationIdTo = "0";

                if (costCenterIdFrom == "0") {
                    toastr.warning('Please provide cost center from.');
                    return;
                }
                else if (stockById == "0") {
                    toastr.warning("Please provide a stock by.");
                    return;
                }
                else if (locationIdFrom == "0") {
                    toastr.warning("Please provide a location from.");
                    return;
                }
                else if (quantity == "") {
                    toastr.warning("Please provide item quantity.");
                    return;
                }
                else if (IsSerialProduct == "1" && serialNumber == "") {
                    toastr.warning("Please provide item Serial.");
                    return;
                }

                if ($("#ContentPlaceHolder1_ddlProductOutFor").val() != "Sales") {

                    if (costCenterIdTo == "0") {
                        toastr.warning('Please provide cost center to.');
                        return;
                    }
                    if (locationIdTo == "0") {
                        toastr.warning("Please provide a location to.");
                        return;
                    }
                }

                var productOutFor = "", itemId = "0", outId = "0", outDetailsId = "0", duplicateItemId = 0;
                var productName = "", stockBy = "", costCenterFrom = "", costCenterTo = "", orderedQuantity = 0, deliveredQuantity = 0;
                var requisitionOrBillId = "0", categoryId = "0", locationFrom = '', locationTo = '';

                if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Internal" || $("#ContentPlaceHolder1_ddlProductOutFor").val() == "Receive") {
                    itemId = $("#ContentPlaceHolder1_hfItemId").val();
                    productName = $("#ContentPlaceHolder1_txtItemName").val();
                }
                else if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Requisition") {
                    itemId = $("#ContentPlaceHolder1_ddlProduct").val();
                    productName = $("#ContentPlaceHolder1_ddlProduct option:selected").text();
                    requisitionOrBillId = $("#ContentPlaceHolder1_ddlRequisition").val();

                    if (requisitionOrBillId == "0") {
                        toastr.warning("Please select a Requisition Id");
                        return;
                    }
                }
                else if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Sales") {
                    itemId = $("#ContentPlaceHolder1_ddlSalesOrderProduct").val();
                    productName = $("#ContentPlaceHolder1_ddlSalesOrderProduct option:selected").text();
                    requisitionOrBillId = $("#ContentPlaceHolder1_ddlBillNumber").val();

                    //if (requisitionOrBillId = "0") {
                    //    toastr.warning("Please select a Bill Number");
                    //    return;
                    //}
                }


                if (itemId == "0") {
                    toastr.warning("Please provide an item.");
                    return;
                }

                //if (IsSerialProduct == "1") {

                //    if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Sales") {
                //        quantity = "1";
                //        serialNumber = $("#ContentPlaceHolder1_ddlSerialNumber option:selected").text();
                //    }
                //    else {
                //        quantity = "1";
                //        serialNumber = $.trim(serialNumberOrQuantity);
                //    }
                //}
                //else {
                //    quantity = serialNumberOrQuantity;
                //}

                outId = $("#ContentPlaceHolder1_hfOutId").val();

                stockBy = $("#ContentPlaceHolder1_ddlStockBy option:selected").text();
                costCenterFrom = $("#ContentPlaceHolder1_ddlCostCenterFrom option:selected").text();
                categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                locationFrom = $("#ContentPlaceHolder1_ddlLocationFrom option:selected").val();

                if ($("#ContentPlaceHolder1_ddlProductOutFor").val() != "Sales") {
                    costCenterTo = $("#ContentPlaceHolder1_ddlCostCenterTo option:selected").text();
                    locationTo = $("#ContentPlaceHolder1_ddlLocationTo option:selected").val();
                }

                var productDetails = _.findWhere(RequisitionProductDetails, { ItemId: parseInt(itemId, 10) });
                if (productDetails != null) {

                    if (productDetails.DeliverStatus == "Partial" && (parseFloat(quantity) + productDetails.DeliveredQuantity) > productDetails.ApprovedQuantity) {
                        toastr.warning("Out quantity can not be greater than ordered quantity.");
                        return false;
                    }
                    else if (productDetails.DeliverStatus != "Partial" && parseFloat(quantity) > productDetails.ApprovedQuantity) {
                        toastr.warning("Out quantity can not be greater than ordered quantity.");
                        return false;
                    }
                }

                if (outItemEdited != "") {
                    var editedItemId = $.trim($(outItemEdited).find("td:eq(10)").text());

                    if (editedItemId != itemId) {
                        duplicateItemId = $("#ProductOutGrid tbody tr").find("td:eq(19):contains(" + (requisitionOrBillId + "-" + itemId) + ")").length;

                        if (duplicateItemId > 0) {
                            toastr.warning("Same item already added.");
                            return false;
                        }
                    }
                }
                else {

                    if (serialNumber != "") {
                        duplicateItemId = $("#ProductOutGrid tbody tr").find("td:eq(19):contains(" + (requisitionOrBillId + "-" + itemId + "-" + serialNumber) + ")").length;
                    }
                    else {
                        duplicateItemId = $("#ProductOutGrid tbody tr").find("td:eq(19):contains(" + (requisitionOrBillId + "-" + itemId) + ")").length;
                    }

                    if (duplicateItemId > 0) {
                        toastr.warning("Same item already added.");
                        return false;
                    }
                }

                if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Internal") {
                    $("#ContentPlaceHolder1_ddlProductOutFor").attr("disabled", true);
                }
                else if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Requisition") {
                    $("#ContentPlaceHolder1_ddlProductOutFor").attr("disabled", true);
                    $("#ContentPlaceHolder1_ddlRequisition").attr("disabled", true);
                }
                else if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Sales") {
                    $("#ContentPlaceHolder1_ddlProductOutFor").attr("disabled", true);
                    $("#ContentPlaceHolder1_ddlBillNumber").attr("disabled", true);
                }

                ValidateItemStock(itemId, locationFrom, quantity).done(function (response) {

                    if (itemStockValidation == 1) {

                        if (IsSerialProduct == "1") {

                            if (serialNumber == "") {
                                toastr.warning('Please fill the serial number.');
                                return;
                            }

                            SerialNumberValidationCheck(itemId, serialNumber).done(function (response) {
                                IsValidSerial = $("#ContentPlaceHolder1_hfIsValidSerial").val();

                                if (IsValidSerial == "0") {
                                    toastr.warning("Invalid Serial Number.");
                                    return;
                                }

                                duplicateSerial = $("#ProductOutGrid tbody tr").find("td:eq(1):contains(" + serialNumber + ")").length;

                                if (duplicateSerial > 0) {
                                    toastr.warning("Serial number allready added");
                                    return;
                                }

                                if (outId != "0" && outItemEdited != "") {

                                    outDetailsId = $(outItemEdited).find("td:eq(7)").text();
                                    EditItem(productName, serialNumber, quantity, costCenterFrom, costCenterTo, stockBy, outDetailsId, requisitionOrBillId, categoryId, itemId, costCenterIdFrom, locationIdFrom, locationFrom, costCenterIdTo, locationIdTo, locationTo, stockById);
                                    //toastr.info("Edit db");
                                    return;
                                }
                                else if (outId == "0" && outItemEdited != "") {

                                    outDetailsId = $(outItemEdited).find("td:eq(7)").text();
                                    EditItem(productName, serialNumber, quantity, costCenterFrom, costCenterTo, stockBy, outDetailsId, requisitionOrBillId, categoryId, itemId, costCenterIdFrom, locationIdFrom, locationFrom, costCenterIdTo, locationIdTo, locationTo, stockById);
                                    //toastr.info("Edit");
                                    return;
                                }

                                AddItemForOut(productName, serialNumber, quantity, costCenterFrom, costCenterTo, stockBy, outDetailsId, requisitionOrBillId, categoryId, itemId, costCenterIdFrom, locationIdFrom, locationFrom, costCenterIdTo, locationIdTo, locationTo, stockById);

                                $("#ContentPlaceHolder1_hfItemId").val("0");
                                $("#ContentPlaceHolder1_txtItemName").val("");
                                $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
                                $("#ContentPlaceHolder1_ddlStockBy").val("0");

                                //$('#ContentPlaceHolder1_ddlCostCenterFrom').val("0");
                                //$("#ContentPlaceHolder1_hfLocationFromId").val("0");

                                //$("#ContentPlaceHolder1_ddlLocationFrom").val("0");
                                //$("#ContentPlaceHolder1_ddlLocationTo").val("0");

                                $("#ContentPlaceHolder1_hfIsSerializableItem").val("0");
                                $("#ContentPlaceHolder1_hfIsValidSerial").val("0");

                                $("#ContentPlaceHolder1_lblPOQuantity").val("");
                                $("#ContentPlaceHolder1_lblOutQuantity").val("")

                                if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Internal") {
                                    $('#ContentPlaceHolder1_ddlCostCenterTo').val("0");
                                    $("#ContentPlaceHolder1_hfLocationToId").val("0");
                                }

                                $("#ContentPlaceHolder1_ddlProduct").focus();
                            });
                        }
                        else {

                            if (quantity == "") {
                                toastr.warning('Please fill the quantity.');
                                return;
                            }
                            else if (CommonHelper.IsDecimal(quantity) == false) {
                                toastr.warning('Please give valid quantity.');
                                return;
                            }

                            if (outId != "0" && outItemEdited != "") {

                                outDetailsId = $(outItemEdited).find("td:eq(7)").text();
                                EditItem(productName, serialNumber, quantity, costCenterFrom, costCenterTo, stockBy, outDetailsId, requisitionOrBillId, categoryId, itemId, costCenterIdFrom, locationIdFrom, locationFrom, costCenterIdTo, locationIdTo, locationTo, stockById);
                                //toastr.info("Edit db");
                                return;
                            }
                            else if (outId == "0" && outItemEdited != "") {

                                outDetailsId = $(outItemEdited).find("td:eq(7)").text();
                                EditItem(productName, serialNumber, quantity, costCenterFrom, costCenterTo, stockBy, outDetailsId, requisitionOrBillId, categoryId, itemId, costCenterIdFrom, locationIdFrom, locationFrom, costCenterIdTo, locationIdTo, locationTo, stockById);
                                //toastr.info("Edit");
                                return;
                            }

                            AddItemForOut(productName, serialNumber, quantity, costCenterFrom, costCenterTo, stockBy, outDetailsId, requisitionOrBillId, categoryId, itemId, costCenterIdFrom, locationIdFrom, locationFrom, costCenterIdTo, locationIdTo, locationTo, stockById);

                            $("#ContentPlaceHolder1_hfItemId").val("0");
                            $("#ContentPlaceHolder1_txtItemName").val("");
                            $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
                            $("#ContentPlaceHolder1_ddlStockBy").val("0");

                            $("#ContentPlaceHolder1_ddlProduct").val("0");
                            $('#ContentPlaceHolder1_ddlProduct').val("0").trigger('change');

                            //$('#ContentPlaceHolder1_ddlCostCenterFrom').val("0");
                            //$("#ContentPlaceHolder1_hfLocationFromId").val("0");

                            //$("#ContentPlaceHolder1_ddlLocationFrom").val("0");
                            //$("#ContentPlaceHolder1_ddlLocationTo").val("0");

                            $("#ContentPlaceHolder1_hfIsSerializableItem").val("0");
                            $("#ContentPlaceHolder1_hfIsValidSerial").val("0");

                            $("#ContentPlaceHolder1_lblPOQuantity").val("");
                            $("#ContentPlaceHolder1_lblOutQuantity").val("");

                            if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Internal") {
                                $('#ContentPlaceHolder1_ddlCostCenterTo').val("0");
                                $("#ContentPlaceHolder1_hfLocationToId").val("0");
                            }

                            $("#ContentPlaceHolder1_ddlProduct").focus();
                        }
                    }
                    else {
                        toastr.warning("Product Out quantity exceeds current stock.");
                        return;
                    }
                });

                return false;
            });

            $("#ContentPlaceHolder1_txtItemName").autocomplete({
                source: function (request, response) {

                    var costCenetrId = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();

                    if (costCenetrId == "0") {
                        toastr.info("Please select from cost center");
                        return;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmPMProductOut.aspx/ItemSearch',
                        data: "{'searchTerm':'" + request.term + "','costCenetrId':'" + costCenetrId + "','categoryId':'" + categoryId + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    CategoryId: m.CategoryId,
                                    ProductType: m.ProductType
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
                    $("#ContentPlaceHolder1_hfItemId").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfCategoryId").val(ui.item.CategoryId);

                    if (ui.item.ProductType == "Serial Item") {
                        $('#ContentPlaceHolder1_lblQuantity_Serial').text("Serial Number");
                        document.getElementById("ContentPlaceHolder1_txtQuantity_Serial").className = "form-control";
                        $("#ContentPlaceHolder1_hfIsSerializableItem").val("1");
                    }
                    else {
                        document.getElementById("ContentPlaceHolder1_txtQuantity_Serial").className = "form-control";
                        $('#ContentPlaceHolder1_lblQuantity_Serial').text("Quantity");
                        $("#ContentPlaceHolder1_hfIsSerializableItem").val("0");
                    }
                }
            });

            $("#ContentPlaceHolder1_txtSearchLocationFrom").autocomplete({
                source: function (request, response) {

                    var costCenetrId = $("#ContentPlaceHolder1_ddlSearchCostCenterFrom").val();

                    if (costCenetrId == "0") {
                        toastr.info("Please select from cost center");
                        return;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmPMProductOut.aspx/InvLocationSearch',
                        data: "{'searchTerm':'" + request.term + "','costCenterId':'" + costCenetrId + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.LocationId
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
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfSearchLocationIdFrom").val(ui.item.value);
                }
            });

            $("#ContentPlaceHolder1_txtSearchLocationTo").autocomplete({
                source: function (request, response) {

                    var costCenetrId = $("#ContentPlaceHolder1_ddlSearchCostCenterTo").val();

                    if (costCenetrId == "0") {
                        toastr.info("Please select from cost center");
                        return;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmPMProductOut.aspx/InvLocationSearch',
                        data: "{'searchTerm':'" + request.term + "','costCenterId':'" + costCenetrId + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.LocationId
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
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfSearchLocationIdTo").val(ui.item.value);
                }
            });


            $("#ContentPlaceHolder1_txtSearchLocationFrom").blur(function () {
                if ($.trim($(this).val()) == "") {
                    $("#ContentPlaceHolder1_hfSearchLocationIdFrom").val("0");
                }
            });

            $("#ContentPlaceHolder1_txtSearchLocationTo").blur(function () {
                if ($.trim($(this).val()) == "") {
                    $("#ContentPlaceHolder1_hfSearchLocationIdTo").val("0");
                }
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

        function LoadLocationFrom(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationFromSucceeded, OnLoadLocationFailed);
        }
        function LoadLocationTo(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationToSucceeded, OnLoadLocationFailed);
        }

        function OnLoadLocationFromSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlLocationFrom');

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

            control.val($("#ContentPlaceHolder1_hfLocationFromId").val());
            return false;
        }

        function OnLoadLocationToSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlLocationTo');

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
            control.val($("#ContentPlaceHolder1_hfLocationToId").val());
            return false;
        }
        function OnLoadLocationFailed() { }

        function LoadSerialNumberByProductId(productId, outId) {
            PageMethods.GetSerialNumberByProductId(productId, outId, OnLoadSerialNumberByProductoSucceeded, OnLoadSerialNumberByProductFailed);
        }

        function OnLoadSerialNumberByProductoSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlSerialNumber');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].SerialNumber + '" value="' + list[i].SerialNumber + '">' + list[i].SerialNumber + '</option>');
                    }

                    if ($("#ContentPlaceHolder1_hfSerialNumber").val() != "")
                        control.val($("#ContentPlaceHolder1_hfSerialNumber").val());
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            return false;
        }
        function OnLoadSerialNumberByProductFailed(error) {
            toastr.info("Serial No Error.");
        }

        function OnLoadLocationToSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlLocationTo');

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
            control.val($("#ContentPlaceHolder1_hfLocationToId").val());
            return false;
        }

        function AddItemForOut(productName, serialNumber, quantity, costCenterFrom, costCenterTo, stockBy, outDetailsId, requisitionOrBillId, categoryId, itemId, costCenterIdFrom, locationIdFrom, locationFrom, costCenterIdTo, locationIdTo, locationTo, stockById) {

            var isEdited = "0";
            var rowLength = $("#ProductOutGrid tbody tr").length;

            if (costCenterTo == null)
                costCenterTo = "";

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:25%;'>" + productName + "</td>";
            tr += "<td style='width:15%;'>" + serialNumber + "</td>";
            tr += "<td style='width:10%;'>" + quantity + "</td>";
            tr += "<td style='width:15%;'>" + costCenterFrom + "</td>";
            tr += "<td style='width:15%;'>" + costCenterTo + "</td>";

            tr += "<td style='width:10%;'>" + stockBy + "</td>";

            tr += "<td style='width:10%;'>";
            if (serialNumber == "") {
                tr += "<a onclick=\"javascript:return FIllForEdit(this);\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;";
            }
            tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteItemReceivedDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";

            //  25, 15, 10, 15, 15, 10, 10
            //      0	             1           2            3                4            5        6
            //Product Name, Serial Number, Quantity, From Cost Center, To Cost Center, Stock By, ActionProductOutFor
            //OutDetailsId, 			7
            //RequisitionOrBilllNumber	8
            //categoryId 			    9
            //ItemId				    10
            //CostCenterIdFrom, 		11
            //LocationIdFrom,			12
            //CostCenterIdTo, 		    13
            //LocationIdTo,			    14
            //StockById, 			    15
            //LocationFrom,			    16
            //LocationTo,			    17
            //Is Edited, 			    18
            //duplicateCheck 			19

            tr += "<td style='display:none'>" + outDetailsId + "</td>";
            tr += "<td style='display:none'>" + requisitionOrBillId + "</td>";
            tr += "<td style='display:none'>" + categoryId + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + costCenterIdFrom + "</td>";
            tr += "<td style='display:none'>" + locationIdFrom + "</td>";
            tr += "<td style='display:none'>" + costCenterIdTo + "</td>";
            tr += "<td style='display:none'>" + locationIdTo + "</td>";
            tr += "<td style='display:none'>" + stockById + "</td>";
            tr += "<td style='display:none'>" + locationFrom + "</td>";
            tr += "<td style='display:none'>" + locationTo + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";

            if (serialNumber == "") {
                tr += "<td style='display:none'>" + (requisitionOrBillId + "-" + itemId) + "</td>";
            }
            else {
                tr += "<td style='display:none'>" + (requisitionOrBillId + "-" + itemId + "-" + serialNumber) + "</td>";
            }

            tr += "</tr>";

            $("#ProductOutGrid tbody").append(tr);
        }

        function FIllForEdit(editItem) {

            outItemEdited = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAdd").val("Update");

            var requisitionOrBillNumber = $(tr).find("td:eq(8)").text();
            var categoryId = $.trim($(tr).find("td:eq(9)").text());
            var itemId = $.trim($(tr).find("td:eq(10)").text());
            var costCenterIdFrom = $(tr).find("td:eq(11)").text();
            var locationIdFrom = $(tr).find("td:eq(12)").text();
            var costCenterIdTo = $(tr).find("td:eq(13)").text();
            var locationIdTo = $(tr).find("td:eq(14)").text();
            var stockById = $(tr).find("td:eq(15)").text();
            var locationFrom = $(tr).find("td:eq(16)").text();
            var locationTo = $(tr).find("td:eq(17)").text();

            var itemName = $(tr).find("td:eq(0)").text();
            var serialNumber = $.trim($(tr).find("td:eq(1)").text());
            var quantity = $(tr).find("td:eq(2)").text();

            var outId = $("#ContentPlaceHolder1_hfOutId").val();

            $("#ContentPlaceHolder1_hfLocationFromId").val(locationIdFrom);
            $("#ContentPlaceHolder1_hfLocationToId").val(locationIdTo);

            if ($("#ContentPlaceHolder1_ddlCostCenterFrom").val() != costCenterIdFrom) {
                LoadLocationFrom(costCenterIdFrom);
            }
            else {
                $("#ContentPlaceHolder1_ddlLocationFrom").val(locationIdFrom);
            }

            if ($("#ContentPlaceHolder1_ddlCostCenterTo").val() != costCenterIdTo) {
                LoadLocationTo(costCenterIdTo);
            }
            else {
                $("#ContentPlaceHolder1_ddlLocationTo").val(locationIdTo);
            }

            if ($("#ContentPlaceHolder1_ddlProductOutFor").val() != "Sales") {

                if (serialNumber != "") {
                    $('#ContentPlaceHolder1_lblQuantity_Serial').text("Serial Number");
                    document.getElementById("ContentPlaceHolder1_txtQuantity_Serial").className = "form-control";
                    $("#ContentPlaceHolder1_txtQuantity_Serial").val(serialNumber);
                    $("#ContentPlaceHolder1_hfIsSerializableItem").val("1");
                }
                else {
                    document.getElementById("ContentPlaceHolder1_txtQuantity_Serial").className = "form-control";
                    $('#ContentPlaceHolder1_lblQuantity_Serial').text("Quantity");
                    $("#ContentPlaceHolder1_txtQuantity_Serial").val(quantity);
                    $("#ContentPlaceHolder1_hfIsSerializableItem").val("0");
                }
            }
            else {
                if (serialNumber != "") {
                    $("#ContentPlaceHolder1_hfIsSerializableItem").val("1");
                    $("#ContentPlaceHolder1_hfSerialNumber").val(serialNumber);
                    LoadSerialNumberByProductId(itemId, outId);
                }
            }

            if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Internal") {
                $("#ContentPlaceHolder1_ddlCategory").val(categoryId);
                $("#ContentPlaceHolder1_hfItemId").val(itemId);
                $("#ContentPlaceHolder1_txtItemName").val(itemName);
            }
            else if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Requisition") {
                $("#ContentPlaceHolder1_ddlProduct").val(itemId);

                var productDetails = _.findWhere(RequisitionProductDetails, { ItemId: parseInt(itemId, 10) });
                if (productDetails != null) {
                    $("#ContentPlaceHolder1_lblPOQuantity").val(productDetails.ApprovedQuantity);
                    $("#ContentPlaceHolder1_lblOutQuantity").val(productDetails.DeliveredQuantity);
                }
            }
            else if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Sales") {
                $('#ContentPlaceHolder1_ddlSalesOrderProduct').val(itemId).trigger('change.select2');
            }

            $("#ContentPlaceHolder1_ddlCostCenterFrom").val(costCenterIdFrom);
            $("#ContentPlaceHolder1_ddlCostCenterTo").val(costCenterIdTo);
            $("#ContentPlaceHolder1_ddlStockBy").val(stockById);
        }

        function EditItem(productName, serialNumber, quantity, costCenterFrom, costCenterTo, stockBy, outDetailsId, requisitionOrBillId, categoryId, itemId, costCenterIdFrom, locationIdFrom, locationFrom, costCenterIdTo, locationIdTo, locationTo, stockById) {

            $(outItemEdited).find("td:eq(0)").text(productName);
            $(outItemEdited).find("td:eq(1)").text(serialNumber);
            $(outItemEdited).find("td:eq(2)").text(quantity);
            $(outItemEdited).find("td:eq(3)").text(costCenterFrom);
            $(outItemEdited).find("td:eq(4)").text(costCenterTo);
            $(outItemEdited).find("td:eq(5)").text(stockBy);

            $(outItemEdited).find("td:eq(7)").text(outDetailsId);
            $(outItemEdited).find("td:eq(8)").text(requisitionOrBillId);
            $(outItemEdited).find("td:eq(9)").text(categoryId);
            $(outItemEdited).find("td:eq(10)").text(itemId);

            $(outItemEdited).find("td:eq(11)").text(costCenterIdFrom);
            $(outItemEdited).find("td:eq(12)").text(locationIdFrom);
            $(outItemEdited).find("td:eq(13)").text(costCenterIdTo);
            $(outItemEdited).find("td:eq(14)").text(locationIdTo);
            $(outItemEdited).find("td:eq(15)").text(stockById);

            $(outItemEdited).find("td:eq(16)").text(locationFrom);
            $(outItemEdited).find("td:eq(17)").text(locationTo);

            var outId = $("#ContentPlaceHolder1_hfOutId").val();

            if (outDetailsId != "0")
                $(outItemEdited).find("td:eq(18)").text("1");

            $(outItemEdited).find("td:eq(19)").text((outId + "-" + itemId));

            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_txtItemName").val("");
            $("#ContentPlaceHolder1_ddlProduct").val("0");

            $("#ContentPlaceHolder1_txtQuantity_Serial").val("");

            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $('#ContentPlaceHolder1_ddlCostCenterFrom').val("0");
            $("#ContentPlaceHolder1_hfLocationFromId").val("0");

            if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Internal") {
                $('#ContentPlaceHolder1_ddlCostCenterTo').val("0");
                $("#ContentPlaceHolder1_hfLocationToId").val("0");
            }

            $("#ContentPlaceHolder1_ddlCategory").val("0");
            $("#ContentPlaceHolder1_hfStockById").val("0");

            $("#ContentPlaceHolder1_ddlLocationFrom").val("0");
            $("#ContentPlaceHolder1_ddlLocationTo").val("0");

            $("#btnAdd").val("Add");

            outItemEdited = "";
        }

        function FillForm(outId) {
            PageMethods.FillForm(outId, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {

            if (result != null) {

                PerformClearAction();

                $("#ContentPlaceHolder1_ddlProductOutFor").val(result.ProductOut.ProductOutFor);
                $("#ContentPlaceHolder1_hfOutId").val(result.ProductOut.OutId);
                $("#ContentPlaceHolder1_txtRemarks").val(result.ProductOut.Remarks);

                if (result.ProductOut.ProductOutFor == "Internal") {
                    $("#RequisitionContainer").hide();
                    $("#SalesOutContainer").hide();
                    $("#ProductForInternalOut").show();
                    $("#InternalOutLocationTo").show();
                    $("#CostCenterToDiv").show();
                    $("#ContentPlaceHolder1_ddlProductOutFor").attr("disabled", true);
                }
                else if (result.ProductOut.ProductOutFor == "Requisition") {
                    $("#RequisitionContainer").show();
                    $("#SalesOutContainer").hide();
                    $("#ProductForInternalOut").hide();
                    $("#InternalOutLocationTo").show();
                    $("#CostCenterToDiv").show();
                    $("#ContentPlaceHolder1_ddlRequisition").val(result.ProductOut.RequisitionOrSalesId);
                    $("#ContentPlaceHolder1_hfLocationToId").val("0");
                    LoadRequisitionProductListForEdit();
                    $("#ContentPlaceHolder1_ddlProductOutFor").attr("disabled", true);
                    $("#ContentPlaceHolder1_ddlRequisition").attr("disabled", true);
                }
                else if (result.ProductOut.ProductOutFor == "Sales") {
                    $("#RequisitionContainer").hide();
                    $("#SalesOutContainer").show();
                    $("#ProductForInternalOut").hide();
                    $("#InternalOutLocationTo").hide();
                    $("#CostCenterToDiv").hide();
                    $("#ContentPlaceHolder1_ddlProductOutFor").attr("disabled", true);
                    $("#ContentPlaceHolder1_ddlBillNumber").val(result.ProductOut.RequisitionOrSalesId);
                    $("#ContentPlaceHolder1_ddlBillNumber").attr("disabled", true);
                }

                $("#ProductOutGrid tbody").html("");

                var rowLength = result.ProductOutDetails.length;
                var row = 0;

                for (row = 0; row < rowLength; row++) {

                    if (result.ProductOutDetails[row].SerialNumber == null)
                        result.ProductOutDetails[row].SerialNumber = '';

                    AddItemForOut(result.ProductOutDetails[row].ProductName, result.ProductOutDetails[row].SerialNumber, result.ProductOutDetails[row].Quantity,
                                    result.ProductOutDetails[row].CostCenterFrom, result.ProductOutDetails[row].CostCenterTo, result.ProductOutDetails[row].StockBy,
                                    result.ProductOutDetails[row].OutDetailsId, result.ProductOut.RequisitionOrSalesId, 0,
                                    result.ProductOutDetails[row].ProductId, result.ProductOutDetails[row].CostCenterIdFrom,
                                    result.ProductOutDetails[row].LocationIdFrom, result.ProductOutDetails[row].LocationFrom,
                                    result.ProductOutDetails[row].CostCenterIdTo, result.ProductOutDetails[row].LocationIdTo,
                                    result.ProductOutDetails[row].LocationTo, result.ProductOutDetails[row].StockById);
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
                    OutDetailsId: $(tr).find("td:eq(7)").text(),
                    OutId: outId
                });
            }

            $(deletedItem).parent().parent().remove();
        }

        function ValidationBeforeSave() {

            var rowCount = $('#ProductOutGrid tbody tr').length;
            if (rowCount == 0) {
                toastr.warning('Add atleast one Product.');
                return false;
            }

            CommonHelper.SpinnerOpen();

            var productOutFor = "", itemId = "0", costCenterIdFrom = "0", costCenterIdTo = "0", stockById = "0";
            var serialNumber = "", quantity = "0", requisitionOrSalesId = "0", locationIdFrom = "0", locationIdTo = "0";
            var isEdit = "0", outId = "0", outDetailsId = "0", remarks = "", outFor = "0";

            productOutFor = $("#ContentPlaceHolder1_ddlProductOutFor").val();
            //requisitionOrSalesId = $("#ContentPlaceHolder1_ddlBillNumber").val();
            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            outFor = $("#ContentPlaceHolder1_ddlOutFor").val();
            outId = $("#ContentPlaceHolder1_hfOutId").val();

            if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Internal") {
                requisitionOrSalesId = "0";
            }
            else if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Requisition") {
                requisitionOrSalesId = $("#ContentPlaceHolder1_ddlRequisition").val();
            }
            else if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Sales") {
                requisitionOrSalesId = $("#ContentPlaceHolder1_ddlBillNumber").val();
            }

            var ProductOut = {
                OutId: outId,
                ProductOutFor: productOutFor,
                RequisitionOrSalesId: requisitionOrSalesId,
                OutFor: outFor,
                Remarks: remarks
            };

            var AddedOutDetails = [], EditedOutDetails = [];

            $("#ProductOutGrid tbody tr").each(function (index, item) {

                outDetailsId = $.trim($(item).find("td:eq(7)").text());
                isEdit = $.trim($(item).find("td:eq(18)").text());

                itemId = $.trim($(item).find("td:eq(10)").text());
                costCenterIdFrom = $(item).find("td:eq(11)").text();
                locationIdFrom = $(item).find("td:eq(12)").text();

                costCenterIdTo = $(item).find("td:eq(13)").text();

                if ($("#ContentPlaceHolder1_ddlProductOutFor").val() != "Sales") {
                    locationIdTo = $(item).find("td:eq(14)").text();
                }
                else {
                    locationIdTo = null;
                }

                stockById = $(item).find("td:eq(15)").text();

                serialNumber = $.trim($(item).find("td:eq(1)").text());
                quantity = $(item).find("td:eq(2)").text();

                if (outDetailsId == "0") {

                    AddedOutDetails.push({
                        OutDetailsId: outDetailsId,
                        OutId: outId,
                        CostCenterIdFrom: costCenterIdFrom,
                        LocationIdFrom: locationIdFrom,
                        CostCenterIdTo: costCenterIdTo,
                        LocationIdTo: locationIdTo,
                        StockById: stockById,
                        ProductId: itemId,
                        Quantity: quantity,
                        SerialNumber: serialNumber
                    });
                }
                else if (outDetailsId != "0" && isEdit != "0") {
                    EditedOutDetails.push({
                        OutDetailsId: outDetailsId,
                        OutId: outId,
                        CostCenterIdFrom: costCenterIdFrom,
                        LocationIdFrom: locationIdFrom,
                        CostCenterIdTo: costCenterIdTo,
                        LocationIdTo: locationIdTo,
                        StockById: stockById,
                        ProductId: itemId,
                        Quantity: quantity,
                        SerialNumber: serialNumber
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

                tr += "<td style='width:15%;'>" + result[row].ProductName + "</td>";

                if (result[row].SerialNumber == null)
                    tr += "<td style='width:12%;'>" + '' + "</td>";
                else
                    tr += "<td style='width:12%;'>" + result[row].SerialNumber + "</td>";

                tr += "<td style='width:8%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:15%;'>" + result[row].CostCenterFrom + "</td>";
                tr += "<td style='width:13%;'>" + result[row].LocationFrom + "</td>";

                if (result[row].CostCenterTo != null)
                    tr += "<td style='width:15%;'>" + result[row].CostCenterTo + "</td>";
                else
                    tr += "<td style='width:15%;'></td>";

                if (result[row].LocationTo != null)
                    tr += "<td style='width:13%;'>" + result[row].LocationTo + "</td>";
                else
                    tr += "<td style='width:13%;'></td>";

                tr += "<td style='width:8%;'>" + result[row].StockBy + "</td>";

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
                title: "Item Details",
                show: 'slide'
            });
            CommonHelper.SpinnerClose();
        }
        function OnSaveProductOutDetailsLoadFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function LoadRequisitionProductList() {
            CommonHelper.SpinnerOpen();
            var requisitionId = $('#ContentPlaceHolder1_ddlRequisition').val();
            PageMethods.LoadProductByRequisitionId(requisitionId, OnLoadProductListOnRequisitionNumberChangeSucceeded, OnLoadProductListOnRequisitionNumberChangeFailed);
            return false;
        }
        function LoadRequisitionProductListForEdit() {
            CommonHelper.SpinnerOpen();
            var requisitionId = $('#ContentPlaceHolder1_ddlRequisition').val();
            PageMethods.LoadProductByRequisitionIdForEdit(requisitionId, OnLoadProductListOnRequisitionNumberChangeSucceeded, OnLoadProductListOnRequisitionNumberChangeFailed);
            return false;
        }
        function OnLoadProductListOnRequisitionNumberChangeSucceeded(result) {

            $("#ContentPlaceHolder1_ddlCostCenterTo").val(result.Requisition.ToCostCenterId);
            $("#ContentPlaceHolder1_ddlCostCenterFrom").val(result.Requisition.FromCostCenterId);
            $("#ContentPlaceHolder1_ddlCostCenterTo").attr("disabled", true);

            var list = result.RequisitionDetails;
            var control = $("#ContentPlaceHolder1_ddlProduct");
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].ItemName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            var product = $('#ContentPlaceHolder1_hfItemId').val();
            $(control).val(product);

            RequisitionProductDetails = result.RequisitionDetails;
            LoadLocationFrom(result.Requisition.FromCostCenterId);
            LoadLocationTo(result.Requisition.ToCostCenterId);
            CommonHelper.SpinnerClose();
        }
        function OnLoadProductListOnRequisitionNumberChangeFailed(error) {
            CommonHelper.SpinnerClose();
        }

        //----------------------------------------------------------------------------------------------------------------
        function LoadProductBySales(salesId) {
            PageMethods.LoadProductBySalesId(salesId, OnLoadProductBySalesIdSucceeded, OnLoadProductBySalesIdFailed);
        }
        function OnLoadProductBySalesIdSucceeded(result) {

            var ddlProductId = "";

            ddlProductId.get(0).options.length = 0;
            ddlProductId.get(0).options[0] = new Option($("#<%=CommonDropDownHiddenField.ClientID %>").val(), "0");

            $.map(result, function (item) {
                ddlProductId.get(0).options[ddlProductId.get(0).options.length] = new Option(item.ItemName, item.ItemId);
            });
        }
        function OnLoadProductBySalesIdFailed(error) {

        }

        function CheckProductIsSerializableOrNot(productId) {
            PageMethods.IsProductSerializable(productId, OnCheckProductIsSerializableOrNotSucceed, OnCheckProductIsSerializableOrNotError);
        }
        function OnCheckProductIsSerializableOrNotSucceed(result) {

            var lblQuantitySerial = $("#<%=lblQuantity_Serial.ClientID %>");
            var txtQuantitySerial = $("#<%=txtQuantity_Serial.ClientID %>");

            if (result == "1") {
                lblQuantitySerial.text("Serial Number");
                txtQuantitySerial.removeClass("form-control").addClass("ThreeColumnTextBox");
                $("#hfIsSerializableProduct").val("1");
            }
            else {

                lblQuantitySerial.text("Quantity");
                txtQuantitySerial.removeClass("ThreeColumnTextBox").addClass("form-control");
            }
        }
        function OnCheckProductIsSerializableOrNotError(error) { }
        //--------------------------------------------------------------------------------------------------------

        function SerialNumberValidationCheck(productId, serialNumber) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmPMProductOut.aspx/ValidateSerialNumber',
                data: "{'productId':'" + productId + "','serialNumber':'" + serialNumber + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d == true) {
                        $("#ContentPlaceHolder1_hfIsValidSerial").val("1");
                    }
                    else {
                        $("#ContentPlaceHolder1_hfIsValidSerial").val("0");
                    }
                },
                error: function (result) {
                    //alert("Error");
                }
            });
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

            $("#ContentPlaceHolder1_ddlProductOutFor").val("Internal");

            $("#ContentPlaceHolder1_ddlProductOutFor").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlRequisition").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlBillNumber").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlCostCenterTo").attr("disabled", false);

            $("#ContentPlaceHolder1_hfOutId").val("0");
            $("#ContentPlaceHolder1_ddlBillNumber").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_ddlOutFor").val("0");

            $("#ContentPlaceHolder1_ddlCategory").val("0");

            $("#ContentPlaceHolder1_txtItemName").val("");
            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_ddlProduct").val("0");
            $("#ContentPlaceHolder1_ddlCostCenterTo").val("0");

            $("#ContentPlaceHolder1_txtQuantity_Serial").val("");

            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_hfStockById").val("0");

            $('#ContentPlaceHolder1_ddlCostCenterFrom').val("0");
            $("#ContentPlaceHolder1_hfLocationFromId").val("0");

            $('#ContentPlaceHolder1_ddlCostCenterTo').val("0");
            $("#ContentPlaceHolder1_hfLocationToId").val("0");

            $("#ContentPlaceHolder1_ddlLocationFrom").val("0");
            $("#ContentPlaceHolder1_ddlLocationTo").val("0");

            document.getElementById("ContentPlaceHolder1_txtQuantity_Serial").className = "form-control";
            $('#ContentPlaceHolder1_lblQuantity_Serial').text("Quantity");
            $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
            $("#ContentPlaceHolder1_hfIsSerializableItem").val("0");
            $("#ContentPlaceHolder1_hfIsValidSerial").val("0");

            $("#ContentPlaceHolder1_lblPOQuantity").val("");
            $("#ContentPlaceHolder1_lblOutQuantity").val("");

            $("#RequisitionContainer").hide();
            $("#SalesOutContainer").hide();
            $("#ProductForInternalOut").show();
            $("#InternalOutLocationTo").show();
            $("#CostCenterToDiv").show();

            $("#ProductOutGrid tbody").html("");

            outItemEdited = "";
            DeletedOutDetails = [];
            RequisitionProductDetails = [];

            $("#ContentPlaceHolder1_btnSave").val("Save");

            return false;
        }

        function CancelOutOrder() {

            $("#ContentPlaceHolder1_ddlCostCenterFrom").val("0");
            $("#ContentPlaceHolder1_ddlCategory").val("0");

            $("#ContentPlaceHolder1_txtItemName").val("");
            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_ddlProduct").val("0");

            $("#ContentPlaceHolder1_txtQuantity_Serial").val("");

            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_hfStockById").val("0");

            $('#ContentPlaceHolder1_ddlCostCenterFrom').val("0");
            $("#ContentPlaceHolder1_hfLocationFromId").val("0");

            $("#ContentPlaceHolder1_hfLocationToId").val("0");

            $("#ContentPlaceHolder1_ddlLocationFrom").val("0");
            $("#ContentPlaceHolder1_ddlLocationTo").val("0");

            document.getElementById("ContentPlaceHolder1_txtQuantity_Serial").className = "form-control";
            $('#ContentPlaceHolder1_lblQuantity_Serial').text("Quantity");
            $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
            $("#ContentPlaceHolder1_hfIsSerializableItem").val("0");
            $("#ContentPlaceHolder1_hfIsValidSerial").val("0");

            $("#ContentPlaceHolder1_lblPOQuantity").val("");
            $("#ContentPlaceHolder1_lblOutQuantity").val("");

            if ($("#ContentPlaceHolder1_ddlProductOutFor").val() == "Internal") {
                $('#ContentPlaceHolder1_ddlCostCenterTo').val("0");
                $('#ContentPlaceHolder1_ddlCostCenterTo').attr("disabled", false);
            }

            if ($("#ProductOutGrid tbody tr").length == 0 && $("#ContentPlaceHolder1_hfOutId").val() == "0") {

                $("#ContentPlaceHolder1_hfOutId").val("0");

                $("#ContentPlaceHolder1_ddlProductOutFor").val("Internal");
                $("#ContentPlaceHolder1_ddlRequisition").val("0");
                $("#ContentPlaceHolder1_ddlBillNumber").val("0");

                $("#ContentPlaceHolder1_ddlProductOutFor").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlRequisition").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlBillNumber").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlCostCenterTo").attr("disabled", false);

                $("#RequisitionContainer").hide();
                $("#SalesContainer").hide();
                $("#ProductForInternalOut").show();
                $("#InternalOutLocationTo").show();
                $("#CostCenterToDiv").show();
                DeletedOutDetails = [];
                RequisitionProductDetails = [];
            }

            outItemEdited = "";
        }

        function ValidateItemStock(itemId, locationId, productoutQunatity) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmPMProductOut.aspx/GetItemStock',
                data: "{'itemId':'" + itemId + "','locationId':'" + locationId + "','quantity':'" + productoutQunatity + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d == true) {
                        itemStockValidation = 1;
                    }
                    else {
                        itemStockValidation = 0;
                    }
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }
    </script>
    <div id="DetailsOutGridContaiiner" style="display: none;">
        <table id="DetailsOutGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 15%;">Item Name
                    </th>
                    <th style="width: 12%;">Serial Number
                    </th>
                    <th style="width: 8%;">Quantity
                    </th>
                    <th style="width: 15%;">From Cost Center
                    </th>
                    <th style="width: 13%;">From Location
                    </th>
                    <th style="width: 15%;">To Cost Center
                    </th>
                    <th style="width: 13%;">To Location
                    </th>
                    <th style="width: 8%;">Stock By
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsSerializableItem" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsValidSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfOutId" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationFromId" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationToId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSearchLocationIdFrom" runat="server" Value="0" />
    <asp:HiddenField ID="hfSearchLocationIdTo" runat="server" Value="0" />
    <asp:HiddenField ID="hfProductForSales" runat="server" Value="" />
    <asp:HiddenField ID="hfSerialNumber" runat="server" Value="" />

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
                    Imte Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label9" runat="server" class="control-label required-field" Text="Item Out For"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlProductOutFor" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <%--<div id="SalesContainer">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Bill Number"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlBillNumber" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>--%>
                        <div id="RequisitionContainer">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Requisition Number"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlRequisition" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label8" runat="server" class="control-label required-field" Text="Item"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblQuantityLabel" runat="server" class="control-label" Text="Quantity Ordered"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="lblPOQuantity" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblRequisitionQuantity" runat="server" class="control-label" Text="Already Out"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="lblOutQuantity" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="SalesOutContainer" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Bill Number"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlBillNumber" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label15" runat="server" class="control-label required-field" Text="Item"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlSalesOrderProduct" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="IsPayrollIntegrateWithInventoryVal" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCostCenterId" runat="server" class="control-label required-field"
                                    Text="From Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCostCenterFrom" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label13" runat="server" class="control-label required-field" Text="Location From"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlLocationFrom" runat="server" CssClass="form-control" TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="ProductForInternalOut">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server" TabIndex="3">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group" id="ItemNamePanel">
                                <div class="col-md-2">
                                    <asp:Label ID="lblItemName" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtItemName" runat="server" TabIndex="5" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="CostCenterToDiv">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="To Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCostCenterTo" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="InternalOutLocationTo">
                            <div class="col-md-2">
                                <asp:Label ID="Label14" runat="server" class="control-label required-field" Text="Location To"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlLocationTo" runat="server" CssClass="form-control" TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Stock By"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlStockBy" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="ItemQuantityInfoDiv">
                            <div class="col-md-2">
                                <asp:Label ID="lblQuantity_Serial" runat="server" class="control-label required-field"
                                    Text="Quantity"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtQuantity_Serial" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" id="ItemSerialInfoDiv">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Serial No."></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSerialNumber" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" style="padding: 5px 0 5px 0;">
                            <input type="button" id="btnAdd" value="Add" class="TransactionalButton btn btn-primary btn-sm"
                                tabindex="4" />
                            <input type="button" id="btnCancelOutOrder" value="Cancel" onclick="CancelOutOrder()"
                                class="TransactionalButton btn btn-primary btn-sm" tabindex="4" />
                        </div>
                        <div class="form-group">
                            <div id="SearchPanel" class="panel panel-default">
                                <div class="panel-heading">
                                    Detail Information
                                </div>
                                <div id="ProductOutGridContainer">
                                    <table id="ProductOutGrid" class="table table-bordered table-condensed table-responsive"
                                        style="width: 100%;">
                                        <thead>
                                            <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                                <th style="width: 25%;">Item Name
                                                </th>
                                                <th style="width: 15%;">Serial Number
                                                </th>
                                                <th style="width: 10%;">Quantity
                                                </th>
                                                <th style="width: 15%;">From Cost Center
                                                </th>
                                                <th style="width: 15%;">To Cost Center
                                                </th>
                                                <th style="width: 10%;">Stock By
                                                </th>
                                                <th style="width: 10%;">Action
                                                </th>
                                                <th style="display: none">ProductOutFor
                                                </th>
                                                <th style="display: none">OutDetailsId
                                                </th>
                                                <th style="display: none">RequisitionOrBilllNumber
                                                </th>
                                                <th style="display: none">categoryId
                                                </th>
                                                <th style="display: none">ItemId
                                                </th>
                                                <th style="display: none">CostCenterIdFrom
                                                </th>
                                                <th style="display: none">LocationIdFrom
                                                </th>
                                                <th style="display: none">CostCenterIdTo
                                                </th>
                                                <th style="display: none">LocationIdTo
                                                </th>
                                                <th style="display: none">StockById
                                                </th>
                                                <th style="display: none">LocationFrom
                                                </th>
                                                <th style="display: none">LocationTo
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
                        <div id="InternalPanel" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblOutFor" runat="server" class="control-label required-field" Text="Out For"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlOutFor" runat="server" CssClass="form-control" TabIndex="5">
                                    </asp:DropDownList>
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
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="InfoPanel" class="panel panel-default">
                <div class="panel-heading">
                    Receive Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label12" runat="server" class="control-label" Text="Item Out For"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchProductOutFor" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="From Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchCostCenterFrom" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Location From"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSearchLocationFrom" runat="server" TabIndex="5" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label10" runat="server" class="control-label" Text="To Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchCostCenterTo" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label11" runat="server" class="control-label" Text="Location To"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSearchLocationTo" runat="server" TabIndex="5" CssClass="form-control"></asp:TextBox>
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
                    Search Information
                </div>
                <div class="panel-body">
                  <%--  <asp:GridView ID="gvProductOutInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" TabIndex="9"
                        OnRowCommand="gvProductOutInfo_RowCommand" OnRowDataBound="gvProductOutInfo_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("OutId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Out Date" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("OutDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ProductOutFor" HeaderText="Item Out For" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>                          
                            <asp:BoundField DataField="IssueNumber" HeaderText="Number" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgDetailsRequisition" runat="server" CausesValidation="False"
                                        CommandName="CmdDetails" CommandArgument='<%# bind("OutId") %>' OnClientClick='<%#String.Format("return ProductOutDetails({0})", Eval("OutId")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Item Out Details" />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("OutId") %>' OnClientClick='<%#String.Format("return FillForm({0})", Eval("OutId")) %>'
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("OutId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" />
                                    &nbsp;<asp:ImageButton ID="ImgPOPreview" runat="server" CausesValidation="False"
                                        CommandArgument='<%# bind("OutId") %>' CommandName="CmdPOPreview" ImageUrl="~/Images/ReportDocument.png"
                                        Text="" AlternateText="PO Preview" ToolTip="Payment Preview" />
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
                    </asp:GridView>--%>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        <%--var x = '<%=btnPadding%>';
        if (x > -1) {

            $("#btnAdd").animate({ marginTop: '10px' });
        }
        else {

        }--%>
    </script>
</asp:Content>
