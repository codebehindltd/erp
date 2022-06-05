<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPMProductReceive.aspx.cs" EnableEventValidation="false" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmPMProductReceive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var receivedItemEdited = "";
        var DeletedReceivedDetails = [];

        var vc = [];

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product Receive</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            CommonHelper.ApplyDecimalValidation();
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlProductId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            if ($("#ContentPlaceHolder1_hfReceivedProductTemplate").val() == "1") {
                $("#DetailsDiv").show();
            }

            if ($("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val() == "1") {
                FillForm($("#ContentPlaceHolder1_hfReceivedId").val(), $("#ContentPlaceHolder1_hfReceivedProductTemplate").val());
            }

            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'
            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#myTabs").tabs();

            var supplier = $("#<%=ddlSupplier.ClientID %>").val();
            var payMode = document.getElementById('ContentPlaceHolder1_ddlPayMode');
            if (supplier == 0) {
                for (i = 0; i < payMode.length; i++) {
                    if (payMode.options[i].value == 'Supplier') {
                        payMode.remove(i);
                    }
                }
            }
            $("#ContentPlaceHolder1_ddlSupplier").change(function () {
                var count = 0;
                var supplier = $("#<%=ddlSupplier.ClientID %>").val();
                var payMode = document.getElementById('ContentPlaceHolder1_ddlPayMode');
                if (supplier == 0) {
                    for (i = 0; i < payMode.length; i++) {
                        if (payMode.options[i].value == 'Supplier') {
                            payMode.remove(i);
                        }
                    }
                }
                else {
                    for (i = 0; i < payMode.length; i++) {
                        if (payMode.options[i].value == 'Supplier') {
                            count++;
                        }
                    }
                    if (count == 0) {
                        var option = document.createElement("option");
                        option.text = "Supplier Credit";
                        option.value = "Supplier";
                        payMode.add(option);
                    }
                    PageMethods.GetSupplierInfoById(supplier, OnLoadSupplierSucceeded, OnLoadSupplierFailed);
                }
            });

            function OnLoadSupplierSucceeded(result) {
                $("#<%=hfSupplierNodeId.ClientID %>").val(result.NodeId);
            }
            function OnLoadSupplierFailed(error) {
                toastr.error(error);
            }

            if ($('#ContentPlaceHolder1_ddlPOrderId').val() == "0") {
                $('#ReferenceDiv').hide();
                $("#ContentPlaceHolder1_ddlCostCenterId").attr("disabled", false);
            }
            else {
                $('#ReferenceDiv').hide();
                $("#ContentPlaceHolder1_ddlCostCenterId").attr("disabled", true);
            }

            $("#cbPaymentDetailsInformationDivEnable").click(function () {
                if ($(this).is(':checked') == true) {
                    $('#SupplierPaymentInformationBlock').show();
                }
                else {
                    $('#SupplierPaymentInformationBlock').hide();
                }
            });

            $("#btnAdd").click(function () {
                var serialNumberOrQuantity = $('#ContentPlaceHolder1_txtQuantity_Serial').val();
                var IsSerialProduct = $("#ContentPlaceHolder1_hfIsSerializableItem").val();
                var pOrderId = $('#ContentPlaceHolder1_ddlPOrderId').val();
                var IsValidSerial = "0", duplicateSerial = 0;

                var costCenterId = $('#ContentPlaceHolder1_ddlCostCenterId').val();
                var stockById = $("#ContentPlaceHolder1_ddlStockBy").val();
                var locationId = $("#ContentPlaceHolder1_ddlLocation").val();
                var supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
                var defaultStockBy = $("#lblItemUnitMeasure").text();

                if (pOrderId == "-1") {
                    toastr.warning('Please provide Order Number.');
                    return;
                }
                else if (supplierId == "0") {
                    toastr.warning("Please provide Supplier Name.");
                    return;
                }
                else if (costCenterId == "0") {
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

                if ($('#ContentPlaceHolder1_txtQuantity_Serial').val() == "0") {
                    toastr.warning("Please provide Valid Received Quantity.");
                    return;
                }

                var itemId = "0", quantityReceived = "0", poQuantity = "0", quantity = "0", serialNumber = "";
                var productName = "", stockBy = "", receiveDetailsId = "0", duplicateItemId = 0;
                var locationName = "", supplierName = "", purchasePrice = "0";

                var receivedId = "0";
                receivedId = $("#ContentPlaceHolder1_hfReceivedId").val();
                var productReceivedStatus = $("#ContentPlaceHolder1_hfProductReceiveStatus").val();

                if (IsSerialProduct == "1") {
                    quantity = "1";
                    serialNumber = $.trim(serialNumberOrQuantity);
                }
                else {
                    quantity = serialNumberOrQuantity;
                }

                if ($.trim($("#ContentPlaceHolder1_txtPurchasePrice").val()) != "") {
                    purchasePrice = $("#ContentPlaceHolder1_txtPurchasePrice").val();
                }

                if (pOrderId == "0") {
                    itemId = $("#ContentPlaceHolder1_hfItemId").val();

                    if (itemId == "0") {
                        toastr.warning("Please provide an item.");
                        return;
                    }
                    else if (CommonHelper.IsDecimal(purchasePrice) == false) {
                        toastr.warning("Please give valid purchase price.");
                        return;
                    }
                    productName = $("#ContentPlaceHolder1_txtItemName").val();
                }
                else if (pOrderId != "0") {
                    itemId = $('#ContentPlaceHolder1_ddlProductId').val();
                    poQuantity = $('#ContentPlaceHolder1_lblPOQuantity').val();
                    quantityReceived = $('#ContentPlaceHolder1_lblReceivedQuantity').val();

                    if (quantityReceived == "")
                        quantityReceived = "0";
                    else if (poQuantity == quantityReceived && productReceivedStatus != "Partial")
                        quantityReceived = "0";
                    else if (poQuantity != quantityReceived && productReceivedStatus != "Partial")
                        quantityReceived = "0";

                    if (itemId == "0") {
                        toastr.warning("Please select an item.");
                        return;
                    }
                    productName = $('#ContentPlaceHolder1_ddlProductId option:selected').text();
                }

                stockBy = $("#ContentPlaceHolder1_ddlStockBy option:selected").text();
                locationName = $("#ContentPlaceHolder1_ddlLocation option:selected").text();
                supplierName = $("#ContentPlaceHolder1_ddlSupplier option:selected").text();

                if (receivedItemEdited != "") {
                    var editedItemId = $.trim($(receivedItemEdited).find("td:eq(10)").text());

                    if (editedItemId != itemId) {
                        if (IsSerialProduct != "1") {
                            duplicateItemId = $("#ItemReceivedGrid tbody tr").find("td:eq(19):contains(" + (pOrderId + "-" + itemId) + ")").length;

                            if (duplicateItemId > 0) {
                                toastr.warning("Same item already added in same purchase order.");
                                return false;
                            }
                        }
                    }
                }
                else {
                    if (IsSerialProduct != "1") {
                        duplicateItemId = $("#ItemReceivedGrid tbody tr").find("td:eq(19):contains(" + (pOrderId + "-" + itemId) + ")").length;

                        if (duplicateItemId > 0) {
                            toastr.warning("Same item already added in same purchase order.");
                            return false;
                        }
                    }
                }

                $("#ContentPlaceHolder1_ddlPOrderId").attr("disabled", true);
                if (IsSerialProduct == "1") {

                    if (serialNumberOrQuantity == "") {
                        toastr.warning('Please fill the serial number.');
                        return;
                    }

                    if ($.trim(serialNumberOrQuantity) != $.trim($(receivedItemEdited).find("td:eq(1)").text())) {
                        SerialNumberValidationCheck(pOrderId, serialNumberOrQuantity).done(function (response) {
                            IsValidSerial = $("#ContentPlaceHolder1_hfIsValidSerial").val();

                            if (IsValidSerial == "0") {
                                return;
                            }

                            duplicateSerial = $("#ItemReceivedGrid tbody tr").find("td:eq(1):contains(" + serialNumberOrQuantity + ")").length;

                            if (duplicateSerial > 0) {
                                toastr.warning("Serial number allready added");
                                return;
                            }

                            if (receivedId != "0" && receivedItemEdited != "") {

                                receiveDetailsId = $(receivedItemEdited).find("td:eq(8)").text();
                                EditItem(productName, serialNumber, quantity, stockBy, locationName, receiveDetailsId, pOrderId, itemId, costCenterId, stockById, locationId,
                                         poQuantity, quantityReceived, supplierName, supplierId, purchasePrice, productReceivedStatus, defaultStockBy);
                                TotalCostCalculation();
                                return;
                            }
                            else if (receivedId == "0" && receivedItemEdited != "") {

                                receiveDetailsId = $(receivedItemEdited).find("td:eq(8)").text();
                                EditItem(productName, serialNumber, quantity, stockBy, locationName, receiveDetailsId, pOrderId, itemId, costCenterId, stockById, locationId,
                                         poQuantity, quantityReceived, supplierName, supplierId, purchasePrice, productReceivedStatus, defaultStockBy);
                                TotalCostCalculation();
                                return;
                            }

                            AddReceivedItem(productName, serialNumber, quantity, stockBy, locationName, receiveDetailsId, pOrderId, itemId, costCenterId, stockById, locationId,
                                            poQuantity, quantityReceived, supplierName, supplierId, purchasePrice, productReceivedStatus, defaultStockBy);

                            if (IsSerialProduct != "1") {
                                $("#ContentPlaceHolder1_hfItemId").val("0");
                                $("#ContentPlaceHolder1_txtItemName").val("");
                                $("#ContentPlaceHolder1_ddlProductId").val("0");
                                $("#ContentPlaceHolder1_ddlStockBy").val("0");
                            }
                            else {
                                $("#ContentPlaceHolder1_txtQuantity_Serial").focus();
                            }

                            $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
                            $('#ContentPlaceHolder1_lblPOQuantity').val("0");
                            $('#ContentPlaceHolder1_lblReceivedQuantity').val("0");
                            $("#ContentPlaceHolder1_txtPurchasePrice").val("");
                            $("#ContentPlaceHolder1_hfProductReceiveStatus").val("");
                            $("#lblItemUnitMeasure").text("");
                        });
                    }
                    else {
                        if (receivedId != "0" && receivedItemEdited != "") {

                            receiveDetailsId = $(receivedItemEdited).find("td:eq(8)").text();
                            EditItem(productName, serialNumber, quantity, stockBy, locationName, receiveDetailsId, pOrderId, itemId, costCenterId, stockById, locationId,
                                     poQuantity, quantityReceived, supplierName, supplierId, purchasePrice, productReceivedStatus, defaultStockBy);
                            TotalCostCalculation();
                            return;
                        }
                        else if (receivedId == "0" && receivedItemEdited != "") {

                            receiveDetailsId = $(receivedItemEdited).find("td:eq(8)").text();
                            EditItem(productName, serialNumber, quantity, stockBy, locationName, receiveDetailsId, pOrderId, itemId, costCenterId, stockById, locationId,
                                     poQuantity, quantityReceived, supplierName, supplierId, purchasePrice, productReceivedStatus, defaultStockBy);
                            TotalCostCalculation();
                            return;
                        }

                        AddReceivedItem(productName, serialNumber, quantity, stockBy, locationName, receiveDetailsId, pOrderId, itemId, costCenterId, stockById, locationId,
                                        poQuantity, quantityReceived, supplierName, supplierId, purchasePrice, productReceivedStatus, defaultStockBy);

                        if (IsSerialProduct != "1") {
                            $("#ContentPlaceHolder1_hfItemId").val("0");
                            $("#ContentPlaceHolder1_txtItemName").val("");
                            $("#ContentPlaceHolder1_ddlProductId").val("0");
                            $("#ContentPlaceHolder1_ddlStockBy").val("0");
                        }
                        else {
                            $("#ContentPlaceHolder1_txtQuantity_Serial").focus();
                        }

                        $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
                        $('#ContentPlaceHolder1_lblPOQuantity').val("0");
                        $('#ContentPlaceHolder1_lblReceivedQuantity').val("0");
                        $('#ContentPlaceHolder1_txtLocation').val("");
                        $('#ContentPlaceHolder1_hfLocationId').val("0");
                        $("#ContentPlaceHolder1_ddlLocation").val("0");
                        $("#ContentPlaceHolder1_txtPurchasePrice").val("");
                        $("#ContentPlaceHolder1_hfProductReceiveStatus").val("");
                        $("#lblItemUnitMeasure").text("");
                    }
                }
                else {
                    if (serialNumberOrQuantity == "") {
                        toastr.warning('Please fill the quantity.');
                        return;
                    }
                    else if (CommonHelper.IsDecimal(serialNumberOrQuantity) == false) {
                        toastr.warning('Please give valid quantity.');
                        return;
                    }

                    if (receivedId != "0" && receivedItemEdited != "") {

                        receiveDetailsId = $(receivedItemEdited).find("td:eq(8)").text();
                        EditItem(productName, serialNumber, quantity, stockBy, locationName, receiveDetailsId, pOrderId, itemId, costCenterId, stockById, locationId,
                                 poQuantity, quantityReceived, supplierName, supplierId, purchasePrice, productReceivedStatus, defaultStockBy);
                        TotalCostCalculation();
                        return;
                    }
                    else if (receivedId == "0" && receivedItemEdited != "") {

                        receiveDetailsId = $(receivedItemEdited).find("td:eq(8)").text();
                        EditItem(productName, serialNumber, quantity, stockBy, locationName, receiveDetailsId, pOrderId, itemId, costCenterId, stockById, locationId,
                                 poQuantity, quantityReceived, supplierName, supplierId, purchasePrice, productReceivedStatus, defaultStockBy);
                        TotalCostCalculation();
                        return;
                    }

                    AddReceivedItem(productName, serialNumber, quantity, stockBy, locationName, receiveDetailsId, pOrderId, itemId, costCenterId, stockById, locationId,
                                    poQuantity, quantityReceived, supplierName, supplierId, purchasePrice, productReceivedStatus, defaultStockBy);

                    $("#ContentPlaceHolder1_hfItemId").val("0");
                    $("#ContentPlaceHolder1_txtItemName").val("");
                    $("#ContentPlaceHolder1_ddlProductId").val("0");
                    $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
                    $("#ContentPlaceHolder1_ddlStockBy").val("0");
                    $('#ContentPlaceHolder1_lblPOQuantity').val("0");
                    $('#ContentPlaceHolder1_lblReceivedQuantity').val("0");
                    $('#ContentPlaceHolder1_txtLocation').val("");
                    $('#ContentPlaceHolder1_hfLocationId').val("0");
                    //$("#ContentPlaceHolder1_ddlLocation").val("0");
                    $("#ContentPlaceHolder1_txtPurchasePrice").val("");
                    $("#lblItemUnitMeasure").text("");
                }
                TotalCostCalculation();
                $('#ContentPlaceHolder1_txtItemName').focus();
                return false;
            });

            $("#btnClearReceivedItem").click(function () {

                var ddlProductIdVal = $("#ContentPlaceHolder1_ddlProductId").val();
                var ddlSupplierVal = $("#ContentPlaceHolder1_ddlSupplier").val();

                if (ddlProductIdVal == 0 && ddlSupplierVal == 0) {
                    $('#receivedNOrderQuantity').hide();
                }
                else {
                    $('#receivedNOrderQuantity').show();
                }
                $('#AdhocDiv').hide();
                $('#NotAdhocDropdown').hide();

                $('#ContentPlaceHolder1_txtQuantity_Serial').val("");
                $("#ContentPlaceHolder1_hfItemId").val("0");
                $("#ContentPlaceHolder1_txtItemName").val("");
                $("#ContentPlaceHolder1_ddlProductId").val("0");
                $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
                $("#ContentPlaceHolder1_ddlStockBy").val("0");
                $('#ContentPlaceHolder1_lblPOQuantity').val("0");
                $('#ContentPlaceHolder1_lblReceivedQuantity').val("0");
                $("#ContentPlaceHolder1_hfLocationId").val("0");
                $("#ContentPlaceHolder1_ddlLocation").val("0");
                $("#ContentPlaceHolder1_hfStockById").val("0");
                $("#ContentPlaceHolder1_ddlSupplier").val("0");
                $("#ContentPlaceHolder1_txtPurchasePrice").val("");
                $('#ContentPlaceHolder1_lblQuantity_Serial').text("Received Quantity");

                if ($("#ItemReceivedGrid tbody tr").length == 0 && $("#ContentPlaceHolder1_hfReceivedId").val() == "0") {
                    $("#ContentPlaceHolder1_ddlCostCentre").val('0');
                    $("#ContentPlaceHolder1_hfReceivedId").val('0');
                    $("#ContentPlaceHolder1_ddlPOrderId").val("0");
                    $("#ContentPlaceHolder1_ddlCostCenterId").attr("disabled", false);
                    $("#ContentPlaceHolder1_ddlPOrderId").attr("disabled", false);
                    $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
                }

                $("#btnAdd").val("Add");

                receivedItemEdited = "";

                return false;

            });

            $('#ContentPlaceHolder1_ddlPOrderId').change(function () {

                if ($(this).val == "") {
                    return;
                }

                PerformClearAll();

                if ($('#ContentPlaceHolder1_ddlPOrderId').val() == "0") {
                    $('#AdhocDiv').show();
                    $("#ReferenceDiv").hide();

                    var ddlProductIdVal = $("#ContentPlaceHolder1_ddlProductId").val();
                    var ddlSupplierVal = $("#ContentPlaceHolder1_ddlSupplier").val();

                    $('#receivedNOrderQuantity').hide();

                    $("#ContentPlaceHolder1_ddlSupplier").val($("#ContentPlaceHolder1_hfAdhocSupplierId").val());

                    //if (ddlProductIdVal == 0 && ddlSupplierVal == 0) {
                    //    $('#receivedNOrderQuantity').hide();
                    //}
                    //else {
                    //    $('#receivedNOrderQuantity').show();
                    //}

                    $('#NotAdhocDropdown').hide();
                    $("#ContentPlaceHolder1_txtPurchasePrice").attr("disabled", false);
                    LoadSupplierCreditInfo();
                    PageMethods.LoadStockByForAdhoc(OnLoadAllStockBySucceeded, OnLoadAllStockByFailed);
                    $("#ContentPlaceHolder1_ddlCostCenterId").attr("disabled", false);
                }
                else {
                    $('#AdhocDiv').hide();
                    $("#ReferenceDiv").hide();
                    $("#ContentPlaceHolder1_ddlCostCenterId").attr("disabled", true);
                    var ddlProductIdVal = $("#ContentPlaceHolder1_ddlProductId").val();
                    var ddlSupplierVal = $("#ContentPlaceHolder1_ddlSupplier").val();

                    if (ddlProductIdVal == 0 && ddlSupplierVal == 0) {
                        $('#receivedNOrderQuantity').hide();
                    }
                    else {
                        $('#receivedNOrderQuantity').show();
                    }

                    $('#NotAdhocDropdown').show();
                    $("#ContentPlaceHolder1_txtPurchasePrice").attr("disabled", true);
                }

                $("#ContentPlaceHolder1_hfItemId").val("0");
                $("#ContentPlaceHolder1_hfCategoryId").val("0");
                $("#ContentPlaceHolder1_hfPoOrderId").val("0");
                $("#ContentPlaceHolder1_hfReceivedId").val("0");

                $('#ContentPlaceHolder1_lblPOQuantity').val('');
                $('#ContentPlaceHolder1_lblReceivedQuantity').val('');

                var orderId = $('#ContentPlaceHolder1_ddlPOrderId').val();
                if (orderId != "0") {
                    LoadProductForNotAdhoc(orderId);
                    $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
                }
                else
                    $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
            });

            $("#ContentPlaceHolder1_ddlProductId").change(function () {

                if ($(this).val == "") {
                    return;
                }

                var poOrderId = $("#ContentPlaceHolder1_ddlPOrderId").val();
                var productId = $("#ContentPlaceHolder1_ddlProductId").val();

                LoadProductQuantityStatus(poOrderId, productId);

                var ddlProductIdVal = $("#ContentPlaceHolder1_ddlProductId").val();
                var ddlSupplierVal = $("#ContentPlaceHolder1_ddlSupplier").val();

                if (ddlProductIdVal == 0 && ddlSupplierVal == 0) {
                    $('#AdhocDiv').show();
                    $('#receivedNOrderQuantity').hide();
                }
                else {
                    $('#AdhocDiv').hide();
                    $('#receivedNOrderQuantity').show();
                }
                $('#AdhocDiv').hide();
            });

            $("#ContentPlaceHolder1_txtItemName").autocomplete({
                source: function (request, response) {

                    var costCenterId = $("#ContentPlaceHolder1_ddlCostCenterId").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmPMProductReceive.aspx/ItemSearch',
                        data: JSON.stringify({ searchTerm: request.term, costCenterId: costCenterId }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    CategoryId: m.CategoryId,
                                    ProductType: m.ProductType,
                                    StockBy: m.StockBy
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
                    $("#ContentPlaceHolder1_ddlStockBy").val(ui.item.StockBy);

                    $("#lblItemUnitMeasure").text("(" + $("#ContentPlaceHolder1_ddlStockBy option:selected").text() + ")");

                    if (ui.item.ProductType == "Serial Product") {
                        $('#ContentPlaceHolder1_lblQuantity_Serial').text("Serial Number");
                        //document.getElementById("ContentPlaceHolder1_txtQuantity_Serial").className = "cl-md-4";
                        $("#ContentPlaceHolder1_hfIsSerializableItem").val("1");
                    }
                    else {
                        //document.getElementById("ContentPlaceHolder1_txtQuantity_Serial").className = "col-md-4";
                        $('#ContentPlaceHolder1_lblQuantity_Serial').text("Received Quantity");
                        $("#ContentPlaceHolder1_hfIsSerializableItem").val("0");
                    }
                }
            });

            $("#ContentPlaceHolder1_ddlCostCenterId").change(function () {
                var costCenetrId = $("#ContentPlaceHolder1_ddlCostCenterId").val();
                if (costCenetrId == "0") {
                    toastr.info("Please select cost center");
                    return;
                }

                LoadLocationByCostCenter(costCenetrId);
                return false;
            });

            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'
            $('#' + ddlPayMode).change(function () {
                if ($('#' + ddlPayMode).val() == "Cash") {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').show();
                    $('#BankPaymentAccountHeadDiv').hide();
                    $('#' + lblPaymentAccountHead).show();
                }
                else if ($('#' + ddlPayMode).val() == "Card") {
                    $('#CardPaymentAccountHeadDiv').show();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').hide();
                    $('#BankPaymentAccountHeadDiv').hide();
                    $('#' + lblPaymentAccountHead).hide();
                }
                else if ($('#' + ddlPayMode).val() == "Cheque") {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').show();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').hide();
                    $('#BankPaymentAccountHeadDiv').hide();
                    $('#' + lblPaymentAccountHead).hide();
                }
                else if ($('#' + ddlPayMode).val() == "Supplier") {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $('#CompanyPaymentAccountHeadDiv').hide();
                    $('#CashPaymentAccountHeadDiv').show();
                    $('#BankPaymentAccountHeadDiv').hide();
                    $('#' + lblPaymentAccountHead).show();
                }
            });

            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var txtConversionRate = '<%=txtConversionRate.ClientID%>'

            var currency = $("#<%=ddlCurrency.ClientID %>").val();
            PageMethods.LoadCurrencyType(currency, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);

            $('#' + ddlCurrency).change(function () {
                var v = $("#<%=ddlCurrency.ClientID %>").val();
                PageMethods.LoadCurrencyType(v, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);
            });
            function OnLoadCurrencyTypeSucceeded(result) {
                $("#<%=hfCurrencyType.ClientID %>").val(result.CurrencyType);
                PageMethods.LoadCurrencyConversionRate(result.CurrencyId, OnLoadConversionRateSucceeded, OnLoadConversionRateFailed);
            }

            function OnLoadCurrencyTypeFailed() {
            }
            function OnLoadConversionRateSucceeded(result) {
                if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                    $("#<%=txtConversionRate.ClientID %>").val('');
                    $('#ConversionRateDivInformation').hide();
                }
                else {
                    $('#ConversionRateDivInformation').show();
                    $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                }

                var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
                if (ddlCurrency == 0) {
                    $('#ConversionRateDivInformation').hide()
                }
            }
            function OnLoadConversionRateFailed() {
            }

            $("#btnAddDetailGuestPayment").click(function () {
                var ddlPayMode = $("#<%=ddlPayMode.ClientID %>").val();
                if (ddlPayMode == 0) {
                    toastr.info('Please Select Payment Mode.');
                    return;
                }

                var ddlCurrencyType = $("#<%=hfCurrencyType.ClientID %>").val();
                if (ddlCurrencyType != "Local") {
                    var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
                    if (ddlCurrency == 0) {
                        toastr.warning('Please select currency type.');
                        return;
                    }
                    else {
                        var txtConversionRate = $("#<%=txtConversionRate.ClientID %>").val();
                        if (isNaN(txtConversionRate)) {
                            toastr.info('Entered Conversion Rate is not in correct format.');
                            return;
                        }

                        if (txtConversionRate == 0) {
                            toastr.info('Entered Conversion Rate is not in correct format.');
                            return;
                        }
                    }
                }

                var enteredAmount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
                if (isNaN(enteredAmount)) {
                    toastr.info('Entered Amount is not in correct format.');
                    return;
                }

                var txtChecqueNumber = '<%=txtChecqueNumber.ClientID%>'
                var txtReceiveLeadgerAmount = '<%=txtReceiveLeadgerAmount.ClientID%>'
                var txtCardNumber = '<%=txtCardNumber.ClientID%>'
                var amount = $('#' + txtReceiveLeadgerAmount).val();
                var number = $('#' + txtCardNumber).val();
                var ddlPayMode = '<%=ddlPayMode.ClientID%>'
                var ddlBankId = '<%=ddlBankId.ClientID%>'
                var ddlCompanyBank = '<%=ddlCompanyBank.ClientID%>'
                var ddlCardType = '<%=ddlCardType.ClientID%>'
                var isValid = true; //ValidateForm();
                var totalAmount = $("#ContentPlaceHolder1_lblTotalCalculateAmount").text();
                if (parseFloat($("#ContentPlaceHolder1_txtReceiveLeadgerAmount").val()) > parseFloat(totalAmount)) {
                    toastr.warning("Payment Amount Can Not Greater Than Total Amount.");
                    return false;
                }

                if ($('#' + ddlPayMode).val() == "Card") {
                    if ($('#' + ddlCardType).val() == "0") {
                        toastr.info('Please Select Card Type.');
                        return;
                    }
                }

                if (isValid == false) {
                    return;
                }
                else if (amount == "") {
                    toastr.info('Please provide Receive Amount.');
                    return;
                }
                else if ($('#' + ddlPayMode).val() == "Card" && $('#' + ddlBankId).val() == "0") {
                    toastr.info('Please provide Bank Name.');
                    $('#' + ddlBankId).focus();
                    return;
                }
                else if ($('#' + ddlPayMode).val() == "Cheque" && $('#' + txtChecqueNumber).val() == "") {
                    toastr.info('Please provide Cheque Number.');
                    $('#' + txtChecqueNumber).focus();
                    return;
                }
                else if ($('#' + ddlPayMode).val() == "Cheque" && $('#' + ddlCompanyBank).val() == "0") {
                    toastr.info('Please provide Bank Name.');
                    $('#' + ddlCompanyBank).focus();
                    return;
                }
                else {
                    SaveGuestPaymentDetailsInformationByWebMethod();
                }
            });
        });

        function OnLoadAllStockBySucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlStockBy');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].HeadName + '" value="' + list[i].UnitHeadId + '">' + list[i].HeadName + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            return false;
        }
        function OnLoadAllStockByFailed() { }

        function SaveGuestPaymentDetailsInformationByWebMethod() {
            var Amount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
            var floatAmout = parseFloat(Amount);
            if (floatAmout <= 0) {
                toastr.info('Receive Amount is not in correct format.');
                return;
            }

            var isEdit = false;
            if ($('#btnAddDetailGuestPayment').val() == "Edit") {
                $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("5");
                isEdit = true;
            }
            else {
                $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("0");
            }
            var ddlPayMode = $("#<%=ddlPayMode.ClientID %>").val();
            var txtReceiveLeadgerAmount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
            var ddlCashReceiveAccountsInfo = $("#<%=ddlCashReceiveAccountsInfo.ClientID %>").val();

            var txtCardNumber = $("#<%=txtCardNumber.ClientID %>").val();
            var ddlCardType = $("#<%=ddlCardType.ClientID %>").val();
            var txtExpireDate = $("#<%=txtExpireDate.ClientID %>").val();
            var txtCardHolderName = $("#<%=txtCardHolderName.ClientID %>").val();

            var txtChecqueNumber = $("#<%=txtChecqueNumber.ClientID %>").val();
            var ddlBankId = $("#<%=ddlBankId.ClientID %>").val();
            var ddlCompanyBankId = $("#<%=ddlCompanyBank.ClientID %>").val();
            var ddlChecquePaymentAccountHeadId = $("#<%=ddlChecquePaymentAccountHeadId.ClientID %>").val();
            var ddlCardPaymentAccountHeadId = $("#<%=ddlCardPaymentAccountHeadId.ClientID %>").val();

            var paymentDescription = "";

            if (ddlPayMode == "Cash") {
                paymentDescription = "";
            }
            else if (ddlPayMode == "Card") {
                var ddlCardTypeText = $("#<%=ddlCardType.ClientID %> option:selected").text();
                paymentDescription = ddlCardTypeText;
            }
            else if (ddlPayMode == "Cheque") {
                var ddlPaidByChequeCompanyText = $("#<%=ddlChecquePaymentAccountHeadId.ClientID %> option:selected").text();
                var ddlCompanyBankText = $("#<%=ddlCompanyBank.ClientID %> option:selected").text();

                paymentDescription = "Bank: " + ddlCompanyBankText + ", Cheque: " + txtChecqueNumber;
            }

    var ddlCurrency = $("#<%=ddlCurrency.ClientID %>").val();
            var conversionRate = $("#<%=txtConversionRate.ClientID %>").val();
            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            var localCurrencyId = $("#<%=hfLocalCurrencyId.ClientID %>").val();
            var supplierNodeId = $("#<%=hfSupplierNodeId.ClientID %>").val();

            $('#btnAddDetailGuestPayment').val("Add");

            if (ddlPayMode == "Cheque") {
                PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, paymentDescription, ddlCurrency, currencyType, localCurrencyId, conversionRate, ddlPayMode, ddlCompanyBankId, txtReceiveLeadgerAmount, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlChecquePaymentAccountHeadId, ddlCardPaymentAccountHeadId, supplierNodeId, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)
            }
            else {
                PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, paymentDescription, ddlCurrency, currencyType, localCurrencyId, conversionRate, ddlPayMode, ddlBankId, txtReceiveLeadgerAmount, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlChecquePaymentAccountHeadId, ddlCardPaymentAccountHeadId, supplierNodeId, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)
            }
            return false;
        }

        function OnPerformSaveGuestPaymentDetailsInformationFailed(error) {
            toastr.error(error.get_message());
        }
        function OnPerformSaveGuestPaymentDetailsInformationSucceeded(result) {
            $("#GuestPaymentDetailGrid").html(result);
            GetTotalPaidAmount();
            ClearDetailsPart();
            //$("#<%=hfSupplierNodeId.ClientID %>").val("");
        }

        function ClearDetailsPart() {

            $("#<%=txtReceiveLeadgerAmount.ClientID %>").val('');

            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('0');
            $("#<%=txtExpireDate.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');

            $("#<%=txtChecqueNumber.ClientID %>").val('');
        }

        function PerformGuestPaymentDetailDelete(paymentId) {
            PageMethods.PerformDeleteGuestPaymentByWebMethod(paymentId, OnPerformDeleteGuestPaymentDetailsSucceeded, OnPerformDeleteGuestPaymentDetailsFailed);
            return false;
        }

        function OnPerformDeleteGuestPaymentDetailsSucceeded(result) {
            $("#GuestPaymentDetailGrid").html(result);
            GetTotalPaidAmount();
            return false;
        }
        function OnPerformDeleteGuestPaymentDetailsFailed(error) {
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
        function OnLoadLocationFailed() { }

        function CancelReceivedItem(receivedId) {
            if (confirm("Are you want to cancel Received Order?")) {
                $("#DetailsReceivedGridContaiiner").dialog("close");
            }

            return false;
        }

        function ActiveReceivedItem(receivedId) {
            if (confirm("Are you want to Active Received Order?")) {
                $("#DetailsReceivedGridContaiiner").dialog("close");
            }

            return false;
        }


        function LoadProduct() {

            if ($('#ContentPlaceHolder1_ddlPOrderId').val() == "0") {
                $('#AdhocDiv').hide();

                var ddlProductIdVal = $("#ContentPlaceHolder1_ddlProductId").val();
                var ddlSupplierVal = $("#ContentPlaceHolder1_ddlSupplier").val();

                if (ddlProductIdVal == 0 && ddlSupplierVal == 0) {
                    $('#AdhocDiv').show();
                    $('#receivedNOrderQuantity').hide();
                }
                else {
                    $('#AdhocDiv').hide();
                    $('#receivedNOrderQuantity').show();
                }

                $('#NotAdhocDropdown').hide();
            }
            else {
                $('#AdhocDiv').hide();

                var ddlProductIdVal = $("#ContentPlaceHolder1_ddlProductId").val();
                var ddlSupplierVal = $("#ContentPlaceHolder1_ddlSupplier").val();

                if (ddlProductIdVal == 0 && ddlSupplierVal == 0) {
                    //$('#AdhocDiv').hide();
                    $('#receivedNOrderQuantity').hide();
                }
                else {
                    //$('#AdhocDiv').show();
                    $('#receivedNOrderQuantity').show();
                }

                $('#NotAdhocDropdown').show();
            }

            //            $("#ContentPlaceHolder1_hfItemId").val("0");
            //            $("#ContentPlaceHolder1_hfCategoryId").val("0");
            //            $("#ContentPlaceHolder1_hfPoOrderId").val("0");
            //            $("#ContentPlaceHolder1_hfReceivedId").val("0");

            //            $('#ContentPlaceHolder1_lblQuantityLabel').hide();
            //            $('#ContentPlaceHolder1_lblPOQuantity').hide();
            //            $('#ContentPlaceHolder1_lblReceivedQuantityLabel').hide();
            //            $('#ContentPlaceHolder1_lblReceivedQuantity').hide();

            //            $('#ContentPlaceHolder1_lblPOQuantity').text('');
            //            $('#ContentPlaceHolder1_lblReceivedQuantity').text('');

            var orderId = $('#ContentPlaceHolder1_ddlPOrderId').val();

            if (orderId != "0")
                LoadProductForNotAdhoc(orderId);
        }

        function FIllForEdit(editItem) {

            //$('#receivedNOrderQuantity').hide();
            var ddlProductIdVal = $("#ContentPlaceHolder1_ddlProductId").val();
            var ddlSupplierVal = $("#ContentPlaceHolder1_ddlSupplier").val();

            if (ddlProductIdVal == 0 && ddlSupplierVal == 0) {
                //$('#AdhocDiv').hide();
                $('#receivedNOrderQuantity').hide();
            }
            else {
                //$('#AdhocDiv').show();
                $('#receivedNOrderQuantity').show();
            }

            $('#ContentPlaceHolder1_lblPOQuantity').val('');
            $('#ContentPlaceHolder1_lblReceivedQuantity').val('');
            $("#ContentPlaceHolder1_txtItemName").val('');
            $("#ContentPlaceHolder1_hfItemId").val('0');
            $("#ContentPlaceHolder1_ddlProductId").val("0");
            $("#ContentPlaceHolder1_ddlLocation").val("0");
            $("#ContentPlaceHolder1_hfLocationId").val("0");

            receivedItemEdited = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAdd").val("Update");

            var pOrderId = $(tr).find("td:eq(9)").text();
            var itemId = $.trim($(tr).find("td:eq(10)").text());
            var costCenterId = $(tr).find("td:eq(11)").text();
            var stockById = $(tr).find("td:eq(12)").text();
            var locationId = $(tr).find("td:eq(13)").text();
            var defaultStockBy = $(tr).find("td:eq(20)").text();

            $("#ContentPlaceHolder1_hfLocationId").val(locationId);

            if ($("#ContentPlaceHolder1_ddlCostCenterId").val() != costCenterId) {
                LoadLocationByCostCenter(costCenterId);
            }
            else {
                $("#ContentPlaceHolder1_ddlLocation").val(locationId);
            }

            var supplierId = $(tr).find("td:eq(16)").text();
            var orderedQuantity = $(tr).find("td:eq(14)").text();
            var receivedQuantity = $(tr).find("td:eq(15)").text();
            var serialNumber = $.trim($(tr).find("td:eq(1)").text());
            var locationName = $(tr).find("td:eq(4)").text();

            var quantity = $(tr).find("td:eq(2)").text();
            var itemName = $(tr).find("td:eq(0)").text();
            var purchasePrice = $(tr).find("td:eq(5)").text();
            var productReceiveStatus = $(tr).find("td:eq(17)").text();

            $("#ContentPlaceHolder1_ddlPOrderId").val(pOrderId);
            //            $("#ContentPlaceHolder1_ddlCostCenterId").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlCostCenterId").val(costCenterId);
            $("#ContentPlaceHolder1_ddlSupplier").val(supplierId);
            $("#ContentPlaceHolder1_hfItemId").val(itemId);
            $("#ContentPlaceHolder1_hfCostCenterId").val(costCenterId);
            //$("#ContentPlaceHolder1_ddlLocation").val(locationId);
            $("#ContentPlaceHolder1_txtPurchasePrice").val(purchasePrice);
            $("#ContentPlaceHolder1_hfProductReceiveStatus").val(productReceiveStatus);
            $("#lblItemUnitMeasure").text(" (" + $.trim(defaultStockBy) + ")");

            if (pOrderId == "0") {
                // LoadProduct();
                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
                $("#ContentPlaceHolder1_txtItemName").val(itemName);
                $("#ContentPlaceHolder1_txtPurchasePrice").attr("disabled", false);
            }
            else if (pOrderId != "0") {
                //LoadProduct();
                $("#ContentPlaceHolder1_ddlProductId").val(itemId);
                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
                $("#ContentPlaceHolder1_txtPurchasePrice").attr("disabled", true);
            }

            if (serialNumber != "") {
                $('#ContentPlaceHolder1_lblQuantity_Serial').text("Serial Number");
                //document.getElementById("ContentPlaceHolder1_txtQuantity_Serial").className = "cpl-md-6 defaultLabelHeight";
                $("#ContentPlaceHolder1_txtQuantity_Serial").val(serialNumber);
                $("#ContentPlaceHolder1_hfIsSerializableItem").val("1");
            }
            else {
                //document.getElementById("ContentPlaceHolder1_txtQuantity_Serial").className = "col-md-4";
                $('#ContentPlaceHolder1_lblQuantity_Serial').text("Received Quantity");
                $("#ContentPlaceHolder1_txtQuantity_Serial").val(quantity);
                $("#ContentPlaceHolder1_hfIsSerializableItem").val("0");
            }

            $("#ContentPlaceHolder1_ddlStockBy").val(stockById);
            $("#ContentPlaceHolder1_hfPoOrderId").val(pOrderId);

            $("#ContentPlaceHolder1_lblPOQuantity").val(orderedQuantity);
            $("#ContentPlaceHolder1_lblReceivedQuantity").val(receivedQuantity);
            //$('#receivedNOrderQuantity').show();
        }

        function EditItem(productName, serialNumber, quantity, stockBy, locationName, receiveDetailsId, pOrderId, itemId, costCenterId, stockById,
                          locationId, poQuantity, quantityReceived, supplierName, supplierId, purchasePrice, productReceivedStatus, defaultStockBy) {

            $(receivedItemEdited).find("td:eq(0)").text(productName);
            $(receivedItemEdited).find("td:eq(1)").text(serialNumber);
            $(receivedItemEdited).find("td:eq(2)").text(quantity);
            $(receivedItemEdited).find("td:eq(3)").text(stockBy);

            $(receivedItemEdited).find("td:eq(4)").text(locationName);
            $(receivedItemEdited).find("td:eq(5)").text(purchasePrice);
            $(receivedItemEdited).find("td:eq(6)").text(supplierName);

            $(receivedItemEdited).find("td:eq(8)").text(receiveDetailsId);
            $(receivedItemEdited).find("td:eq(9)").text(pOrderId);
            $(receivedItemEdited).find("td:eq(10)").text(itemId);
            $(receivedItemEdited).find("td:eq(11)").text(costCenterId);

            $(receivedItemEdited).find("td:eq(12)").text(stockById);
            $(receivedItemEdited).find("td:eq(13)").text(locationId);
            $(receivedItemEdited).find("td:eq(14)").text(poQuantity);
            $(receivedItemEdited).find("td:eq(15)").text(quantityReceived);
            $(receivedItemEdited).find("td:eq(16)").text(supplierId);
            $(receivedItemEdited).find("td:eq(17)").text(productReceivedStatus);

            if (receiveDetailsId != "0")
                $(receivedItemEdited).find("td:eq(18)").text("1");

            $(receivedItemEdited).find("td:eq(19)").text((pOrderId + "-" + itemId));

            defaultStockBy = defaultStockBy.replace('(', '').replace(')', '');

            $(receivedItemEdited).find("td:eq(20)").text(defaultStockBy);

            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_txtItemName").val("");
            $("#ContentPlaceHolder1_ddlProductId").val("0");
            $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $('#ContentPlaceHolder1_lblPOQuantity').val("0");
            $('#ContentPlaceHolder1_lblReceivedQuantity').val("0");

            $("#ContentPlaceHolder1_ddlLocation").val("0");
            $("#ContentPlaceHolder1_hfLocationId").val("0");

            $("#ContentPlaceHolder1_hfStockById").val("0");
            $("#lblItemUnitMeasure").text("");

            $("#btnAdd").val("Add");

            receivedItemEdited = "";
        }

        function LoadProductForNotAdhoc(orderId) {
            PageMethods.LoadProductForNotAdhocByOrderId(orderId, OnLoadProductForNotAdhocByOrderIdSucceeded, OnLoadProductForNotAdhocByOrderIdFailed);
            return false;
        }
        function OnLoadProductForNotAdhocByOrderIdSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlProductId');
            if (result[0].CostCenterId > 0) {
                $("#ContentPlaceHolder1_ddlCostCenterId").val(result[0].CostCenterId);
                $("#ContentPlaceHolder1_ddlCostCenterId2").val(result[0].CostCenterId);
                LoadLocationByCostCenter(result[0].CostCenterId);
            }

            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    $("#ContentPlaceHolder1_ddlSupplier").val(result[0].SupplierId);
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].ItemId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
                PageMethods.GetSupplierInfoById(result[0].SupplierId, OnLoadSupplierSucceeded2, OnLoadSupplierFailed2);
            }

            LoadSupplierCreditInfo();

            if (result[0].IsLocalOrForeignPO == "Foreign") {
                $("#PaymentDetailsInformation").hide();
            }
            else {
                $("#PaymentDetailsInformation").show();
            }
            control.val($("#ContentPlaceHolder1_hfItemId").val());
            return false;
        }
        function OnLoadProductForNotAdhocByOrderIdFailed(error) {
        }

        function LoadProductQuantityStatus(POrderId, ProductId) {
            PageMethods.LoadProductQuantityStatus(POrderId, ProductId, OnLoadProductQuantityStatusSucceeded, OnLoadProductQuantityStatusFailed);
            return false;
        }

        function LoadSupplierCreditInfo() {
            var count = 0;
            var supplier = $("#<%=ddlSupplier.ClientID %>").val();
            var payMode = document.getElementById('ContentPlaceHolder1_ddlPayMode');
            if (supplier == 0) {
                for (i = 0; i < payMode.length; i++) {
                    if (payMode.options[i].value == 'Supplier') {
                        payMode.remove(i);
                    }
                }
            }
            else {
                for (i = 0; i < payMode.length; i++) {
                    if (payMode.options[i].value == 'Supplier') {
                        count++;
                    }
                }
                if (count == 0) {
                    var option = document.createElement("option");
                    option.text = "Supplier Credit";
                    option.value = "Supplier";
                    payMode.add(option);
                }
            }
        }

        function OnLoadProductQuantityStatusSucceeded(result) {
            if (result != null) {
                if ((result.Quantity + result.QuantityReceived) != 0) {
                    var ddlProductIdVal = $("#ContentPlaceHolder1_ddlProductId").val();
                    var ddlSupplierVal = $("#ContentPlaceHolder1_ddlSupplier").val();

                    if (ddlProductIdVal == 0 && ddlSupplierVal == 0) {
                        $('#AdhocDiv').show();
                        $('#receivedNOrderQuantity').hide();
                    }
                    else {
                        $('#AdhocDiv').hide();
                        $('#receivedNOrderQuantity').show();
                    }

                    $('#ContentPlaceHolder1_lblPOQuantity').val(result.Quantity);
                    $('#ContentPlaceHolder1_lblReceivedQuantity').val(result.QuantityReceived);
                    $("#ContentPlaceHolder1_txtPurchasePrice").val(result.PurchasePrice);
                    $('#ContentPlaceHolder1_txtQuantity_Serial').val(result.Quantity - result.QuantityReceived);
                }

                if (parseInt($("#ContentPlaceHolder1_ddlStockBy").val(), 10) != result.StockById) {
                    $("#ContentPlaceHolder1_ddlStockBy").val(result.StockById);
                    $("#ContentPlaceHolder1_hfSelectedStockById").val(result.StockById);
                }

                if (parseInt($("#ContentPlaceHolder1_ddlCostCenterId").val(), 10) != result.CostCenterId) {
                    $("#ContentPlaceHolder1_ddlCostCenterId").val(result.CostCenterId);
                    $("#ContentPlaceHolder1_hfLocationId").val("0");

                    LoadLocationByCostCenter(result.CostCenterId);
                }

                if (result.ProductType == "Serial Product") {
                    $('#ContentPlaceHolder1_lblQuantity_Serial').text("Serial Number");
                    $("#ContentPlaceHolder1_hfIsSerializableItem").val("1");
                }
                else {
                    $('#ContentPlaceHolder1_lblQuantity_Serial').text("Received Quantity");
                    $("#ContentPlaceHolder1_hfIsSerializableItem").val("0");
                }

                $("#ContentPlaceHolder1_hfProductReceiveStatus").val(result.ReceivedStatus);
                PageMethods.LoadRelatedStockBy(result.StockById, OnLoadStockBySucceeded, OnLoadStockByFailed);
            }
        }
        function OnLoadProductQuantityStatusFailed(error) {
        }

        function OnLoadStockBySucceeded(result) {
            var list = result;
            var ddlStockById = '<%=ddlStockBy.ClientID%>';
            var control = $('#' + ddlStockById);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].HeadName + '" value="' + list[i].UnitHeadId + '">' + list[i].HeadName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                }
            }
            control.val($("#ContentPlaceHolder1_hfSelectedStockById").val());
            return false;
        }
        function OnLoadStockByFailed(error) {
        }

        function AddReceivedItem(productName, serialNumber, quantity, stockBy, locationName, receiveDetailsId, pOrderId, itemId, costCenterId, stockById,
                                 locationId, poQuantity, quantityReceived, supplierName, supplierId, purchasePrice, productReceivedStatus, defaultStockBy) {

            var isEdited = "0";
            var rowLength = $("#ItemReceivedGrid tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:17%;'>" + productName + "</td>";
            tr += "<td style='width:12%;'>" + serialNumber + "</td>";
            tr += "<td style='width:8%;'>" + quantity + "</td>";
            tr += "<td style='width:10%;'>" + stockBy + "</td>";
            tr += "<td style='width:12%;'>" + locationName + "</td>";
            tr += "<td style='width:8%;'>" + purchasePrice + "</td>";
            tr += "<td style='width:15%;'>" + supplierName + "</td>";
            tr += "<td style='width:10%;'> <a onclick=\"javascript:return FIllForEdit(this);\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
            tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'javascript:return DeleteItemReceivedDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";
            tr += "<td style='display:none'>" + receiveDetailsId + "</td>";
            tr += "<td style='display:none'>" + pOrderId + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + costCenterId + "</td>";
            tr += "<td style='display:none'>" + stockById + "</td>";
            tr += "<td style='display:none'>" + locationId + "</td>";
            tr += "<td style='display:none'>" + poQuantity + "</td>";
            tr += "<td style='display:none'>" + quantityReceived + "</td>";
            tr += "<td style='display:none'>" + supplierId + "</td>";
            tr += "<td style='display:none'>" + productReceivedStatus + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "<td style='display:none'>" + (pOrderId + "-" + itemId) + "</td>";

            defaultStockBy = defaultStockBy.replace('(', '').replace(')', '');

            tr += "<td style='display:none'>" + $.trim(defaultStockBy) + "</td>";
            tr += "</tr>";

            $("#ItemReceivedGrid tbody").append(tr);
        }

        function OnAddProductReceiveInformationSucceeded(result) {
            $("#productReceiveGrid").html(result);
            $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
            $('#ContentPlaceHolder1_lblQuantity_Serial').text("Received Quantity");
            $("#ContentPlaceHolder1_txtItemName").val("");
        }
        function OnAddProductReceiveInformationFailed(error) {
        }

        function PerformProductReceiveDelete(ReceivedId) {
            PageMethods.PerformProductReceiveDelete(ReceivedId, OnAddProductReceiveInformationSucceeded, OnAddProductReceiveInformationFailed);
            return false;
        }

        function ValidationBeforeSave() {
            var referenceNo = $("#ContentPlaceHolder1_txtReferenceNo").val();
            var purchaseBy = $("#ContentPlaceHolder1_txtPurchaseBy").val();

            var rowCount = $('#ItemReceivedGrid tbody tr').length;
            if (rowCount == 0) {
                toastr.warning('Add atleast one Product.');
                return false;
            }

            CommonHelper.SpinnerOpen();
            var itemId = "0", costCenterId = "0", stockById = "0", locationId = "0", serialNumber = "", quantity = "0";
            var isEdit = "0", pOrderId = "0", receivedId = "0", supplierId = "0", purchasePrice = "0";

            pOrderId = $("#ContentPlaceHolder1_ddlPOrderId").val();
            receivedId = $("#ContentPlaceHolder1_hfReceivedId").val();

            var AddedReceivedDetails = [], EditedReceivedDetails = [];

            $('#ItemReceivedGrid tbody tr').each(function (index, item) {

                receivedDetailsId = $.trim($(item).find("td:eq(8)").text());
                isEdit = $.trim($(item).find("td:eq(17)").text());
                itemId = $.trim($(item).find("td:eq(10)").text());
                costCenterId = $(item).find("td:eq(11)").text();
                stockById = $(item).find("td:eq(12)").text();
                locationId = $(item).find("td:eq(13)").text();
                supplierId = $(item).find("td:eq(16)").text();
                purchasePrice = $.trim($(item).find("td:eq(5)").text());
                serialNumber = $.trim($(item).find("td:eq(1)").text());
                quantity = $(item).find("td:eq(2)").text();

                if (receivedDetailsId == "0") {

                    AddedReceivedDetails.push({
                        ReceiveDetailsId: receivedDetailsId,
                        ProductId: itemId,
                        CostCenterId: costCenterId,
                        StockById: stockById,
                        LocationId: locationId,
                        SerialNumber: serialNumber,
                        SupplierId: supplierId,
                        Quantity: quantity,
                        PurchasePrice: purchasePrice
                    });
                }
                else if (receivedDetailsId != "0" && isEdit != "0") {
                    EditedReceivedDetails.push({
                        ReceiveDetailsId: receivedDetailsId,
                        ProductId: itemId,
                        CostCenterId: costCenterId,
                        StockById: stockById,
                        LocationId: locationId,
                        SerialNumber: serialNumber,
                        SupplierId: supplierId,
                        Quantity: quantity,
                        PurchasePrice: purchasePrice
                    });
                }
            });

            var poSupplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            var poCreditAmount = $("#ContentPlaceHolder1_lblTotalCalculateAmount").text();
            PageMethods.SaveReceivedOrder(receivedId, pOrderId, referenceNo, purchaseBy, AddedReceivedDetails, EditedReceivedDetails, DeletedReceivedDetails, poSupplierId, poCreditAmount, OnSaveReceivedSucceeded, OnSaveReceivedFailed);
            return false;
        }
        function OnSaveReceivedSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                $("#ContentPlaceHolder1_lblTotalCalculateAmount").text("");
                $("#GuestPaymentDetailGrid").html('');
                $("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val("0");
                $("#ContentPlaceHolder1_hfReceivedId").val("0");

                if ($('#ContentPlaceHolder1_ddlPOrderId').val() == "0") {
                    $('#AdhocDiv').show();
                    $("#ReferenceDiv").hide();

                    var ddlProductIdVal = $("#ContentPlaceHolder1_ddlProductId").val();
                    var ddlSupplierVal = $("#ContentPlaceHolder1_ddlSupplier").val();

                    $('#receivedNOrderQuantity').hide();

                    $("#ContentPlaceHolder1_ddlSupplier").val($("#ContentPlaceHolder1_hfAdhocSupplierId").val());
                    $('#NotAdhocDropdown').hide();
                    $("#ContentPlaceHolder1_txtPurchasePrice").attr("disabled", false);
                }
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSaveReceivedFailed(error) { toastr.error(error.get_message()); CommonHelper.SpinnerClose(); }

        function SerialNumberValidationCheck(pOrderId, serialNumber) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmPMProductReceive.aspx/ValidateSerialNumber',
                data: "{'POrderId':'" + pOrderId + "','serialNumber':'" + serialNumber + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d == true) {
                        $("#ContentPlaceHolder1_hfIsValidSerial").val("1");
                    }
                    else {
                        toastr.warning('Serial Number Allready Exist.');
                        $("#ContentPlaceHolder1_hfIsValidSerial").val("0");
                    }
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }

        function FillForm(receivedId, receivedProductTemplate) {
            $("#ContentPlaceHolder1_hfReceivedProductTemplate").val(receivedProductTemplate);

            CommonHelper.SpinnerOpen();
            if (receivedProductTemplate == "1")
                PageMethods.FillForm(receivedId, OnFillFormSucceed, OnFillFormFailed);
            else if (receivedProductTemplate == "2")
                PageMethods.FillFormForTemplate2(receivedId, OnFillFormSucceed, OnFillFormFailed);

            return false;
        }
        function OnFillFormSucceed(result) {
            $("#ContentPlaceHolder1_hfReceivedId").val(result.Received.ReceivedId);
            if ($("#ContentPlaceHolder1_hfReceivedProductTemplate").val() == "1") {
                PerformClearAction();
                if (result != null) {
                    $("#ItemReceivedGrid tbody").html("");
                    $("#ContentPlaceHolder1_hfPoOrderId").val(result.Received.POrderId);
                    $("#ContentPlaceHolder1_ddlPOrderId").val(result.Received.POrderId);
                    $("#ContentPlaceHolder1_ddlSupplier").val(result.Received.SupplierId);
                    $("#ContentPlaceHolder1_ddlPOrderId").attr("disabled", true);

                    if (result.Received.POrderId != 0) {
                        $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
                        LoadProduct();
                    }
                    else {
                        $('#AdhocDiv').show();
                        $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
                    }

                    var rowLength = result.ReceivedDetails.length;
                    var row = 0;

                    for (row = 0; row < rowLength; row++) {

                        if (result.ReceivedDetails[row].SerialNumber == null)
                            result.ReceivedDetails[row].SerialNumber = '';

                        AddReceivedItem(result.ReceivedDetails[row].ProductName, result.ReceivedDetails[row].SerialNumber, result.ReceivedDetails[row].Quantity,
                                    result.ReceivedDetails[row].StockBy, result.ReceivedDetails[row].LocationName, result.ReceivedDetails[row].ReceiveDetailsId,
                                    result.ReceivedDetails[row].POrderId, result.ReceivedDetails[row].ProductId, result.ReceivedDetails[row].CostCenterId,
                                    result.ReceivedDetails[row].StockById, result.ReceivedDetails[row].LocationId, result.ReceivedDetails[row].OrderedQuantity,
                                    result.ReceivedDetails[row].QuantityReceived, result.ReceivedDetails[row].SupplierName, result.ReceivedDetails[row].SupplierId,
                                    result.ReceivedDetails[row].PurchasePrice, result.ReceivedDetails[row].ReceivedStatus, result.ReceivedDetails[row].DefaultStockBy);
                    }

                    $("#DetailsDiv").show();
                    $("#ContentPlaceHolder1_btnSave").val("Update");
                    TotalCostCalculation();
                    if ($('#ContentPlaceHolder1_ddlPOrderId').val() == "0") {
                        $('#receivedNOrderQuantity').hide();
                    }
                }
            }
            else if ($("#ContentPlaceHolder1_hfReceivedProductTemplate").val() == "2") {
                $("#ContentPlaceHolder1_ddlCostCenterId2").val(result.CostCenterId);
                $("#ContentPlaceHolder1_ddlPOrderId2").val(result.POrderId);
                $("#ContentPlaceHolder1_txtSupplierName").val(result.SupplierName);
                $("#ContentPlaceHolder1_hfSupplierId").val(result.SupplierId);
                $("#ContentPlaceHolder1_btnSave2").val("Update");
                $("#ReceivedProductGridContainer").html(result.ReceivedProductGrid);
            }

            $("#myTabs").tabs({ active: 0 });
            CommonHelper.SpinnerClose();
            PageMethods.FillPaymentInfo(result.Received.ReceivedId, OnFillPaymentInfoSucceed, OnFillPaymentInfoFailed);
        }

        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        function OnFillPaymentInfoSucceed(result) {
            $("#GuestPaymentDetailGrid").html(result);
        }
        function OnFillPaymentInfoFailed(error) {
            toastr.error(error.get_message());
        }

        function DeleteItemReceivedDetails(deleteItem) {
            if (!confirm("Do you want to delete?")) {
                return;
            }

            CommonHelper.SpinnerOpen();

            var receivedId = "0", receivedDetailsId = "0", pOrderId = "0", productId = "0", costCenterId = "0";
            var quantity = "0", serialNumber = "", locationId = "0";

            var tr = $(deleteItem).parent().parent();

            pOrderId = $("#ContentPlaceHolder1_ddlPOrderId").val();
            receivedDetailsId = $(tr).find("td:eq(8)").text();
            receivedId = $("#ContentPlaceHolder1_hfReceivedId").val();
            productId = $.trim($(tr).find("td:eq(10)").text());
            costCenterId = $(tr).find("td:eq(11)").text();
            locationId = $(tr).find("td:eq(13)").text();
            serialNumber = $.trim($(tr).find("td:eq(1)").text());
            quantity = $(tr).find("td:eq(2)").text();

            if ((receivedDetailsId != "0")) {

                DeletedReceivedDetails.push({
                    ReceiveDetailsId: receivedDetailsId,
                    ReceivedId: receivedId,
                    ProductId: productId,
                    CostCenterId: costCenterId,
                    LocationId: locationId,
                    SerialNumber: serialNumber,
                    Quantity: quantity
                });
            }

            $(deleteItem).parent().parent().remove();

            CommonHelper.SpinnerClose();
        }

        function PerformClearAction() {
            $('#AdhocDiv').hide();
            $('#NotAdhocDropdown').hide();
            var ddlProductIdVal = $("#ContentPlaceHolder1_ddlProductId").val();
            var ddlSupplierVal = $("#ContentPlaceHolder1_ddlSupplier").val();

            if (ddlProductIdVal == 0 && ddlSupplierVal == 0) {
                $('#AdhocDiv').show();
                $('#receivedNOrderQuantity').hide();
            }
            else {
                $('#AdhocDiv').hide();
                $('#receivedNOrderQuantity').show();
            }

            $('#ContentPlaceHolder1_txtQuantity_Serial').val("");
            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_txtItemName").val("");
            $("#ContentPlaceHolder1_ddlProductId").val("0");
            $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $('#ContentPlaceHolder1_lblPOQuantity').val("0");
            $('#ContentPlaceHolder1_lblReceivedQuantity').val("0");
            $("#ContentPlaceHolder1_hfLocationId").val("0");
            $("#ContentPlaceHolder1_ddlLocation").val("0");
            $("#ContentPlaceHolder1_hfStockById").val("0");
            $("#ContentPlaceHolder1_ddlPOrderId").val("0");
            $("#ContentPlaceHolder1_ddlSupplier").val("0");
            $("#ContentPlaceHolder1_ddlCostCenterId").val("0");
            $("#ContentPlaceHolder1_txtPurchasePrice").val("");
            $("#ContentPlaceHolder1_hfProductReceiveStatus").val("");
            $("#ContentPlaceHolder1_ddlCostCenterId").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlPOrderId").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
            $("#ContentPlaceHolder1_txtPurchasePrice").attr("disabled", false);
            $("#ContentPlaceHolder1_txtReferenceNo").val("");
            $("#ContentPlaceHolder1_txtPurchaseBy").val("");
            $("#ContentPlaceHolder1_btnSave").val("Save");
            $('#ItemReceivedGrid tbody').html("");
            $("#lblItemUnitMeasure").text("");
        }

        function PerformClearAll() {
            $('#AdhocDiv').hide();
            $('#NotAdhocDropdown').hide();
            var ddlProductIdVal = $("#ContentPlaceHolder1_ddlProductId").val();
            var ddlSupplierVal = $("#ContentPlaceHolder1_ddlSupplier").val();

            if (ddlProductIdVal == 0 && ddlSupplierVal == 0) {
                $('#AdhocDiv').show();
                $('#receivedNOrderQuantity').hide();
            }
            else {
                $('#AdhocDiv').hide();
                $('#receivedNOrderQuantity').show();
            }

            $('#ContentPlaceHolder1_txtQuantity_Serial').val("");
            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_txtItemName").val("");
            $("#ContentPlaceHolder1_ddlProductId").val("0");
            $("#ContentPlaceHolder1_txtQuantity_Serial").val("");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $('#ContentPlaceHolder1_lblPOQuantity').val("0");
            $('#ContentPlaceHolder1_lblReceivedQuantity').val("0");
            $("#ContentPlaceHolder1_hfProductReceiveStatus").val("");
            $("#ContentPlaceHolder1_hfLocationId").val("0");
            $("#ContentPlaceHolder1_ddlLocation").val("0");
            $("#ContentPlaceHolder1_hfStockById").val("0");
            $("#ContentPlaceHolder1_ddlSupplier").val("0");
            $("#ContentPlaceHolder1_ddlCostCenterId").val("0");
            $("#ContentPlaceHolder1_ddlCostCenterId").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlPOrderId").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
            $("#ContentPlaceHolder1_txtPurchasePrice").attr("disabled", false);
            $("#ContentPlaceHolder1_btnSave").val("Save");
            $('#ItemReceivedGrid tbody').html("");
        }

        function ProductReceiveDetails(receivedId, receiveNumber) {
            CommonHelper.SpinnerOpen();
            $("#ContentPlaceHolder1_hfReceiveNumberForPopupWindow").val(receiveNumber);
            PageMethods.ReceivedDetails(receivedId, OnFillRequisitionDetailsSucceed, OnFillRequisitionDetailsFailed);
            return false;
        }
        function OnFillRequisitionDetailsSucceed(result) {
            $("#DetailsReceivedGrid tbody").html("");
            var totalRow = result.length, row = 0;

            var tr = "";

            for (row = 0; row < totalRow; row++) {

                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:20%;'>" + result[row].ProductName + "</td>";

                if (result[row].SerialNumber == null)
                    tr += "<td style='width:20%;'>" + '' + "</td>";
                else
                    tr += "<td style='width:20%;'>" + result[row].SerialNumber + "</td>";

                tr += "<td style='width:10%;'>" + result[row].StockBy + "</td>";
                tr += "<td style='width:10%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:10%;'>" + result[row].PurchasePrice + "</td>";
                tr += "<td style='width:10%;'>" + (result[row].PurchasePrice * result[row].Quantity) + "</td>";
                tr += "<td style='width:15%;'>" + result[row].LocationName + "</td>";
                tr += "<td style='width:15%;'>" + result[row].SupplierName + "</td>";

                tr += "</tr>";

                $("#DetailsReceivedGrid tbody").append(tr);
                tr = "";
            }

            $("#DetailsReceivedGridContaiiner").dialog({
                autoOpen: true,
                modal: true,
                width: 1000,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Product Received Details for the Receive No: " + $("#ContentPlaceHolder1_hfReceiveNumberForPopupWindow").val(),
                show: 'slide'
            });

            CommonHelper.SpinnerClose();
        }
        function OnFillRequisitionDetailsFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        // ---------- Purchase Order Received Template 2

        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlCostCenterId2").change(function () {
                $("#ProductReceivedGrid tbody tr").html('')
                $("#ContentPlaceHolder1_txtSupplierName").val("");

                CommonHelper.SpinnerOpen();

                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenterId2").val();
                PageMethods.GetPurchaseOrderByCostcenter(costCenterId, OnLoadPOrderSucceed, OnLoadPOrderFailed);

                return false;
            });

            $("#ContentPlaceHolder1_ddlPOrderId2").change(function () {
                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenterId2").val();
                var orderId = $("#ContentPlaceHolder1_ddlPOrderId2").val();

                if (costCenterId == "0")
                    return false;

                CommonHelper.SpinnerOpen();

                PageMethods.LoadItemForReceivedByPurchaseOrderId(orderId, costCenterId, OnLoadPurchaseOrderSucceed, OnLoadPurchaseOrderFailed);
                return false;
            });

        });

        function OnLoadPurchaseOrderSucceed(result) {
            if (result != "") {
                $("#ReceivedProductGridContainer").html(result[0]);
                $("#ContentPlaceHolder1_txtSupplierName").val(result[1]);
                $("#ContentPlaceHolder1_hfSupplierId").val(result[2]);
            }

            CommonHelper.SpinnerClose();
            PageMethods.GetSupplierInfoById(result[2], OnLoadSupplierSucceeded2, OnLoadSupplierFailed2);
        }
        function OnLoadPurchaseOrderFailed() { CommonHelper.SpinnerClose(); }
        function OnLoadSupplierSucceeded2(result) {
            $("#<%=hfSupplierNodeId.ClientID %>").val(result.NodeId);
            CommonHelper.SpinnerClose();
        }
        function OnLoadSupplierFailed2(error) {
            toastr.error(error);
            CommonHelper.SpinnerClose();
        }

        function OnLoadPOrderSucceed(result) {

            var list = result;
            var control = $('#ContentPlaceHolder1_ddlPOrderId2');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].POrderId + '">' + list[i].PONumber + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            CommonHelper.SpinnerClose();
        }
        function OnLoadPOrderFailed() { CommonHelper.SpinnerClose(); }

        function CheckInputValue(txtQuantity) {

            var quantity = $(txtQuantity).val();

            if (CommonHelper.IsDecimal(quantity) == false) {
                toastr.warning("Please Give Valid value.");
                quantity = "0";
                return false;
            }

            var tr = $(txtQuantity).parent().parent();
            var purchaseQuantity = parseFloat($(tr).find("td:eq(4)").text());
            var receivedDetailsId = $(tr).find("td:eq(8)").text();
            var previousQuantity = $(tr).find("td:eq(12)").text();
            var receivedQuantity = $(tr).find("td:eq(5)").text();
            var remainingQuantity = 0;

            remainingQuantity = parseFloat(purchaseQuantity) - parseFloat(quantity);
            if (remainingQuantity < 0 || remainingQuantity > parseFloat(purchaseQuantity)) {
                remainingQuantity = 0;
            }
            $(tr).find("td:eq(6)").text(remainingQuantity);

            if (receivedDetailsId != "0" && parseFloat(previousQuantity) != 0 && parseFloat(previousQuantity) != parseFloat(quantity)) {
                $(tr).find("td:eq(13)").text("1");
            }
        }

        function ValidationBeforeSave2() {
            var IsPermitForSave = confirm("Do You Want To Save?");
            if (IsPermitForSave == true) {
                var rowCount = $('#ProductReceivedGrid tbody tr').length;
                if (rowCount == 0) {
                    toastr.warning('There are no Product to Receive.');
                    return false;
                }

                CommonHelper.SpinnerOpen();

                var itemId = "0", costCenterId = "0", stockById = "0", locationId = "0", serialNumber = "", quantity = "0";
                var isEdit = "0", pOrderId = "0", receivedId = "0", supplierId = "0", purchasePrice = "0", dbQuantity = "0";
                var referenceNo = "", purchaseBy = "", poCreditAmount = "0";

                pOrderId = $("#ContentPlaceHolder1_ddlPOrderId2").val();
                receivedId = $("#ContentPlaceHolder1_hfReceivedId").val();
                costCenterId = $("#ContentPlaceHolder1_ddlCostCenterId2").val();
                supplierId = $("#ContentPlaceHolder1_hfSupplierId").val();
                poCreditAmount = $('#ProductReceivedGrid tfoot tr').find("td:eq(1)").text();

                var AddedReceivedDetails = [], EditedReceivedDetails = [];

                $('#ProductReceivedGrid tbody tr').each(function (index, item) {

                    receivedDetailsId = $.trim($(item).find("td:eq(8)").text());
                    isEdit = $.trim($(item).find("td:eq(13)").text());

                    itemId = $.trim($(item).find("td:eq(9)").text());
                    stockById = $(item).find("td:eq(10)").text();
                    locationId = $(item).find("td:eq(11)").text();
                    purchasePrice = $.trim($(item).find("td:eq(3)").text());

                    quantity = $(item).find("td:eq(7)").find("input").val();
                    dbQuantity = $.trim($(item).find("td:eq(12)").text());

                    if (quantity != "" && quantity != "0") {

                        if (receivedDetailsId == "0") {

                            AddedReceivedDetails.push({
                                ReceiveDetailsId: receivedDetailsId,
                                ProductId: itemId,
                                CostCenterId: costCenterId,
                                StockById: stockById,
                                LocationId: locationId,
                                SerialNumber: serialNumber,
                                SupplierId: supplierId,
                                Quantity: quantity,
                                PurchasePrice: purchasePrice
                            });
                        }
                        else if (receivedDetailsId != "0" && isEdit != "0") {

                            if (parseFloat(quantity) != parseFloat(dbQuantity)) {

                                EditedReceivedDetails.push({
                                    ReceiveDetailsId: receivedDetailsId,
                                    ProductId: itemId,
                                    CostCenterId: costCenterId,
                                    StockById: stockById,
                                    LocationId: locationId,
                                    SerialNumber: serialNumber,
                                    SupplierId: supplierId,
                                    Quantity: quantity,
                                    PurchasePrice: purchasePrice
                                });
                            }
                        }
                    }
                    else { }
                });
                PageMethods.SaveReceivedOrder(receivedId, pOrderId, referenceNo, purchaseBy, AddedReceivedDetails, EditedReceivedDetails, DeletedReceivedDetails, supplierId, poCreditAmount, OnSaveReceivedSucceeded2, OnSaveReceivedFailed2);

                return false;
            } else {
                return false;
            }
        }
        function OnSaveReceivedSucceeded2(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#ContentPlaceHolder1_btnSave2").val("Save");
                PerformClearAction2();
                $("#ContentPlaceHolder1_lblTotalCalculateAmount").text("");
                $("#GuestPaymentDetailGrid").html('');
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSaveReceivedFailed2(error) { toastr.error(error.get_message()); CommonHelper.SpinnerClose(); }

        function PerformClearAction2() {
            $("#ContentPlaceHolder1_ddlCostCenterId2").val("0");
            $("#ContentPlaceHolder1_ddlPOrderId2").val("0");
            $("#ContentPlaceHolder1_btnSave2").val("Save");
            $('#ProductReceivedGrid tbody').html("");

            $("#ContentPlaceHolder1_ddlPOrderId2").get(0).options.length = 0;
            $("#ContentPlaceHolder1_ddlPOrderId2").get(0).options[0] = new Option($("#<%=CommonDropDownHiddenField.ClientID %>").val(), "0");
            $("#lblItemUnitMeasure").text("");
        }

        function TotalCostCalculation() {
            var totalCost = 0;
            $("#ItemReceivedGrid tbody tr").each(function () {
                totalCost += parseFloat($(this).find("td:eq(5)").text()) * parseFloat($(this).find("td:eq(2)").text());
            });

            $("#ContentPlaceHolder1_lblTotalCalculateAmount").text(totalCost);
        }

        function GetTotalPaidAmount() {
            PageMethods.PerformGetTotalPaidAmountByWebMethod(OnPerformGetTotalPaidAmountSucceeded, PerformGetTotalPaidAmountFailed);
            return false;
        }

        function PerformGetTotalPaidAmountFailed(error) {
            toastr.error(error.get_message());
        }
        function OnPerformGetTotalPaidAmountSucceeded(result) {
            var PaidTotal = parseFloat(result);
            var FormatedText = "Total Amount: " + PaidTotal;
            $('#TotalPaid').show();
            $('#TotalPaid').text(FormatedText);
        }
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfTemplate" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfLocalCurrencyId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCurrencyType" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCategoryId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCostCenterId" runat="server" Value="0" />
    <asp:HiddenField ID="hfStockById" runat="server" Value="0" />
    <asp:HiddenField ID="hfSelectedStockById" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfPoOrderId" runat="server" Value="" />
    <asp:HiddenField ID="hfReceivedId" runat="server" Value="0" />
    <asp:HiddenField ID="hfReceiveDetailsId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsSerializableItem" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsValidSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsEditedFromApprovedForm" runat="server" Value="0" />
    <asp:HiddenField ID="hfProductReceiveStatus" runat="server" Value="" />
    <asp:HiddenField ID="hfSupplierId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSupplierNodeId" runat="server" Value="0" />
    <asp:HiddenField ID="hfReceivedProductTemplate" runat="server" Value="0" />
    <asp:HiddenField ID="hfAdhocSupplierId" runat="server" Value="0" />
    <asp:HiddenField ID="hfReceiveNumberForPopupWindow" runat="server" Value="0" />
    <div id="DetailsReceivedGridContaiiner" style="display: none;">
        <table id="DetailsReceivedGrid" class="table table-bordered table-condensed table-hover">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 20%;">Product Name
                    </th>
                    <th style="width: 20%;">Serial Number
                    </th>
                    <th style="width: 10%;">Stock By
                    </th>
                    <th style="width: 10%;">Quantity
                    </th>
                    <th style="width: 10%;">Price
                    </th>
                    <th style="width: 10%;">Total
                    </th>
                    <th style="width: 15%;">Location
                    </th>
                    <th style="width: 15%;">Suppplier
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Receive Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Receive Product</a></li>
        </ul>
        <div id="tab-1">
            <div id="ProductReceivedTemplate1" runat="server">
                <div id="EntryPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Product Receive
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group" id="OrderNumberDiv">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Order Number"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlPOrderId" runat="server" CssClass="form-control" TabIndex="2">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group" id="SupplierDiv">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblSupplier" runat="server" class="control-label required-field" Text="Supplier Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlSupplier" runat="server" TabIndex="2" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblCostCenterId" runat="server" class="control-label required-field"
                                        Text="Cost Center"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlCostCenterId" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="AdhocDiv" style="display: none;">
                                <div class="form-group">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblItemName" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtItemName" TabIndex="6" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" id="NotAdhocDropdown">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblServiceType" runat="server" class="control-label required-field"
                                        Text="Product Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlProductId" runat="server" CssClass="form-control" TabIndex="7">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Location"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Stock By"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlStockBy" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblPurchasePriceLocal" runat="server" class="control-label required-field"
                                        Text="Unit Price"></asp:Label>
                                    <span id="lblItemUnitMeasure"></span>
                                    <asp:DropDownList ID="ddlPurchasePriceLocal" runat="server" CssClass="form-control"
                                        TabIndex="7" Visible="False">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPurchasePrice" TabIndex="8" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" id="receivedNOrderQuantity" style="display: none;">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblQuantityLabel" runat="server" class="control-label" Text="Quantity Ordered"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="lblPOQuantity" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblReceivedQuantityLabel" runat="server" class="control-label" Text="Already Received"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="lblReceivedQuantity" runat="server" CssClass="form-control quantitydecimal" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblQuantity_Serial" runat="server" class="control-label required-field"
                                        Text="Received Quantity"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtQuantity_Serial" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <input id="btnAdd" type="button" tabindex="9" value="Add" class="TransactionalButton btn btn-primary btn-sm" />
                                    <input id="btnClearReceivedItem" type="button" class="btn btn-primary btn-sm" value="Cancel" />
                                </div>
                            </div>
                            <div id="DetailsDiv" style="display: none">
                                <div class="panel-body">
                                    <div id="ItemReceivedGridContainer">
                                        <table id="ItemReceivedGrid" class="table table-bordered table-condensed table-responsive"
                                            style="width: 100%;">
                                            <thead>
                                                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                                    <th style="width: 17%;">Product Name
                                                    </th>
                                                    <th style="width: 12%;">Serial Number
                                                    </th>
                                                    <th style="width: 8%;">Quantity
                                                    </th>
                                                    <th style="width: 10%;">Stock By
                                                    </th>
                                                    <th style="width: 12%;">Location
                                                    </th>
                                                    <th style="width: 8%;">Price
                                                    </th>
                                                    <th style="width: 15%;">Supplier
                                                    </th>
                                                    <th style="width: 8%;">Action
                                                    </th>
                                                    <th style="display: none">ReceiveDetailsId
                                                    </th>
                                                    <th style="display: none">PurchaseOrderId
                                                    </th>
                                                    <th style="display: none">ItemId
                                                    </th>
                                                    <th style="display: none">CostCenterId
                                                    </th>
                                                    <th style="display: none">StockById
                                                    </th>
                                                    <th style="display: none">LocationId
                                                    </th>
                                                    <th style="display: none">Ordered Quantity
                                                    </th>
                                                    <th style="display: none">Received Quantity
                                                    </th>
                                                    <th style="display: none">Supplier Id
                                                    </th>
                                                    <th style="display: none">Item Receive Status
                                                    </th>
                                                    <th style="display: none">Is Edited
                                                    </th>
                                                    <th style="display: none">duplicateCheck
                                                    </th>
                                                    <th style="display: none">defaultStockBy
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="form-group" style="text-align: left; margin-top: 10px;">
                                        <asp:Label ID="lblTitleTotalAmount" runat="server" Text="Total Price :" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="lblTotalCalculateAmount" runat="server" Font-Bold="True"></asp:Label>
                                    </div>
                                </div>
                                <div id="ReferenceDiv" style="display: none;">
                                    <div class="form-group">
                                        <div class="col-md-2 label-align">
                                            <asp:Label ID="lblReferenceNo" runat="server" class="control-label" Text="Reference Number"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtReferenceNo" TabIndex="8" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2 label-align">
                                            <asp:Label ID="lblPurchaseBy" runat="server" class="control-label" Text="Purchase By"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtPurchaseBy" TabIndex="8" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div id="Template1Payment">
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="10" CssClass="btn btn-primary btn-sm"
                                            OnClientClick="javascript:return ValidationBeforeSave();" />
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="11" CssClass="btn btn-primary btn-sm"
                                            OnClientClick="javascript: return PerformClearAction();" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="ProductReceivedTemplate2" runat="server" class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCostCenterId2" CssClass="form-control" runat="server" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Order Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPOrderId2" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="Supplier"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSupplierName" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div id="ReceivedProductGridContainer">
                            </div>
                        </div>
                        <div id="Template2Payment">
                        </div>
                        <div class="row" style="padding-top: 40px;">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave2" runat="server" Text="Save" TabIndex="13" CssClass="btn btn-primary"
                                    OnClientClick="javascript: return ValidationBeforeSave2();" />
                                <asp:Button ID="btnClear2" runat="server" Text="Clear" TabIndex="14" CssClass="btn btn-primary"
                                    OnClientClick="javascript: return PerformClearAction2();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="PaymentDetailsInformation" style="display: none;">
                <div class="form-group" style="display: none;">
                    <%--<div class="col-md-2 label-align">
                        </div>--%>
                    <div class="col-md-1">
                    </div>
                    <div class="col-md-4">
                        <asp:CheckBox ID="cbPaymentDetailsInformationDivEnable" runat="server" ClientIDMode="Static" />
                        <span style="font-weight: bold;">Is Supplier Payment Enable</span>
                    </div>
                </div>
                <div id="SupplierPaymentInformationBlock" style="display: none;" class="panel panel-default">
                    <div class="panel-heading">
                        Payment Information
                    </div>
                    <div class="panel-body childDivSectionDivBlockBody">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblPayMode" runat="server" class="control-label required-field" Text="Payment Mode"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                        <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                        <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                        <asp:ListItem Value="Card">Card</asp:ListItem>
                                        <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                        <asp:ListItem Value="Supplier">Supplier Credit</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblCurrencyType" runat="server" class="control-label required-field"
                                        Text="Currency Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlCurrency" TabIndex="6" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblReceiveLeadgerAmount" runat="server" class="control-label required-field"
                                        Text="Receive Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtReceiveLeadgerAmount" runat="server" CssClass="form-control quantitydecimal"
                                        TabIndex="7"></asp:TextBox>
                                </div>
                                <div id="ConversionRateDivInformation" style="display: none;">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblConversionRate" runat="server" class="control-label required-field"
                                            Text="Conversion Rate"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control quantitydecimal" Text=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblPaymentAccountHead" runat="server" class="control-label" Text="Account Head"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <div id="CashPaymentAccountHeadDiv">
                                        <asp:DropDownList ID="ddlCashReceiveAccountsInfo" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="BankPaymentAccountHeadDiv" style="display: none;">
                                        <asp:DropDownList ID="ddlCardReceiveAccountsInfo" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="ChecquePaymentAccountHeadId" style="display: none;">
                                        <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control"
                                            TabIndex="6">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                                <div class="form-group">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblChecqueNumber" runat="server" class="control-label required-field"
                                            Text="Cheque Number"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2 label-align">
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblCompanyBank" runat="server" class="control-label required-field"
                                            Text="Bank Name"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlCompanyBank" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div id="CardPaymentAccountHeadDiv" style="display: none;">
                                <div class="form-group" style="display: none;">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblCardPaymentAccountHeadId" runat="server" class="control-label"
                                            Text="Accounts Posting Head"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlCardPaymentAccountHeadId" runat="server" CssClass="form-control"
                                            TabIndex="6">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="Label9" runat="server" class="control-label required-field" Text="Card Type"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlCardType" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                            <asp:ListItem Value="a">American Express</asp:ListItem>
                                            <asp:ListItem Value="m">Master Card</asp:ListItem>
                                            <asp:ListItem Value="v">Visa Card</asp:ListItem>
                                            <asp:ListItem Value="d">Discover Card</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblCardNumber" runat="server" class="control-label" Text="Card Number"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCardNumber" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblBankId" runat="server" class="control-label required-field" Text="Bank Name"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlBankId" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div style="display: none;">
                                    <div class="form-group">
                                        <div class="col-md-2 label-align">
                                            <asp:Label ID="lblExpiryDate" runat="server" class="control-label" Text="Expiry Date"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2 label-align">
                                            <asp:Label ID="lblCardholderName" runat="server" class="control-label" Text="Card Holder Name"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtCardHolderName" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" style="padding-left: 10px;">
                                <%--Right Left--%>
                                <input type="button" id="btnAddDetailGuestPayment" value="Add" class="btn btn-primary"
                                    onclientclick="javascript: return ValidateForm();" />
                                <asp:Label ID="lblHiddenIdDetailGuestPayment" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                            <div id="GuestPaymentDetailGrid" class="childDivSection">
                            </div>
                            <div id="TotalPaid" class="totalAmout">
                            </div>
                            <div id="dueTotal" class="totalAmout">
                            </div>
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="AlartMessege" runat="server" Style="color: Red;" Text='Grand Total and Guest Payment Amount is not Equal.'
                            CssClass="totalAmout" Visible="false"></asp:Label>
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
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchCostCenterId" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Order Number"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchPOrderId" runat="server" CssClass="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    CssClass="TransactionalButton btn btn-primary" TabIndex="5" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary"
                                    OnClientClick="javascript: return PerformClearAction();" TabIndex="6" />
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
                    <asp:GridView ID="gvProductReceiveInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowCommand="gvProductReceive_RowCommand"
                        TabIndex="9" OnRowDataBound="gvProductReceive_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("ReceivedId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PONumber" HeaderText="PR Number" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Received Date" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvReceiveDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ReceivedDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ReceiveNumber" HeaderText="Receive Number" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgDetailsRequisition" runat="server" CausesValidation="False"
                                        CommandName="CmdRequisitionDetails" CommandArgument='<%# bind("ReceivedId") %>'
                                        OnClientClick='<%#String.Format("return ProductReceiveDetails({0}, \"{1}\")", Eval("ReceivedId"), Eval("ReceiveNumber")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Requisition Details" />
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("ReceivedId") %>' OnClientClick='<%#String.Format("return FillForm({0}, {1})", Eval("ReceivedId"), Eval("ReceivedProductTemplate")) %>'
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgBtnCancelReceived" runat="server" CausesValidation="False"
                                        CommandName="CmdItemReceivedCancel" CommandArgument='<%# bind("ReceivedId") %>'
                                        OnClientClick='<%#String.Format("return CancelReceivedItem({0})", Eval("ReceivedId")) %>'
                                        ImageUrl="~/Images/cancel.png" Text="" AlternateText="Details" ToolTip="Cancel Received Item" />
                                    &nbsp;<asp:ImageButton ID="ImgBtnActiveReceived" runat="server" CausesValidation="False"
                                        CommandName="CmdItemReceivedActive" CommandArgument='<%# bind("ReceivedId") %>'
                                        OnClientClick='<%#String.Format("return ActiveReceivedItem({0})", Eval("ReceivedId")) %>'
                                        ImageUrl="~/Images/select.png" Text="" AlternateText="Details" ToolTip="Active Received Item" />
                                    &nbsp;&nbsp;<asp:ImageButton ID="ImgReportPR" runat="server" CausesValidation="False"
                                        CommandName="CmdReportPR" CommandArgument='<%# bind("ReceivedId") %>' ImageUrl="~/Images/ReportDocument.png"
                                        Text="" AlternateText="Report" ToolTip="Product Receive Report" />
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
        $(document).ready(function () {
            $("#PaymentDetailsInformation").hide();
        });

    </script>
</asp:Content>
