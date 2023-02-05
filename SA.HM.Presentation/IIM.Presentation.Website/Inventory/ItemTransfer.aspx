<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="ItemTransfer.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.ItemTransfer" %>



<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControlVertical.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var vc = null;

        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false, IsSerialAutoField = false;

        var queryOutId = "";
        var AddedSerialCount = 0;

        var ItemSelected = null;
        var TransferItemAdded = new Array();
        var TransferItemDeleted = new Array();
        var NewAddedSerial = new Array();
        var AddedSerialzableProduct = new Array();

        var DeletedSerialzableProduct = new Array();
        var serialNumber = "";
        var NotTriggerChange = 0;
        $(document).ready(function () {

            $("#ContentPlaceHolder1_companyProjectUserControl_lblGLCompany").text("From Company");
            $("#ContentPlaceHolder1_companyProjectUserControl_lblGLProject").text("From Project");

            $("#ContentPlaceHolder1_companyProjectUserControlTwo_lblGLCompany").text("To Company");
            $("#ContentPlaceHolder1_companyProjectUserControlTwo_lblGLProject").text("To Project");

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }


            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            IsSerialAutoField = $('#ContentPlaceHolder1_hfIsItemSerialFillWithAutoSearch').val() == '1' ? true : false;


            if (IsCanSave) {
                $('#btnSave').show();
            } else {
                $('#btnSave').hide();
            }

            if (IsSerialAutoField) {
                $('#labelAndtxtSerialAddButtonAndClear').hide();
                $('#labelAndtxtSerial').hide();
                $('#labelAndtxtSerialAutoComplete').show();
            } else {
                $('#labelAndtxtSerialAddButtonAndClear').show();
                $('#labelAndtxtSerial').show();
                $('#labelAndtxtSerialAutoComplete').hide();

            }

            $("#ContentPlaceHolder1_ddlOutType").val("Requisition");

            if ($("#ContentPlaceHolder1_ddlOutType").val() == "Requisition") {
                $("#RequisitionWiseItemContainer").show();
                $("#RequisitionNumberContainer").show();
                $("#StockTransferContainer").hide();
                $("#QuotationNumberContainer").hide();
                $("#DivBillNo").hide();
                $("#SalesOrderItemContainer").hide();
                $("#BillingOrderItemContainer").hide();
                $("#dvCostCenterTo").show();
                $("#dvCostCenterFrom").show();
            }
            else if ($("#ContentPlaceHolder1_ddlOutType").val() == "StockTransfer") {
                $("#RequisitionWiseItemContainer").hide();
                $("#RequisitionNumberContainer").hide();
                $("#StockTransferContainer").show();
                $("#QuotationNumberContainer").hide();
                $("#DivBillNo").hide();
                $("#SalesOrderItemContainer").hide();
                $("#BillingOrderItemContainer").hide();
                $("#dvCostCenterTo").show();
                $("#dvCostCenterFrom").show();
            }
            else if ($("#ContentPlaceHolder1_ddlOutType").val() == "SalesOut") {
                $("#RequisitionWiseItemContainer").hide();
                $("#RequisitionNumberContainer").hide();
                $("#StockTransferContainer").hide();
                $("#QuotationNumberContainer").show();
                $("#DivBillNo").hide();
                $("#SalesOrderItemContainer").show();
                $("#BillingOrderItemContainer").hide();
                $("#dvCostCenterTo").hide();
                $("#dvCostCenterFrom").hide();
            }
            else if ($("#ContentPlaceHolder1_ddlOutType").val() == "Billing") {
                $("#RequisitionWiseItemContainer").hide();
                $("#RequisitionNumberContainer").hide();
                $("#StockTransferContainer").hide();
                $("#QuotationNumberContainer").hide();
                $("#DivBillNo").show();
                $("#SalesOrderItemContainer").hide();
                $("#BillingOrderItemContainer").show();
                $("#dvCostCenterTo").hide();
                $("#dvCostCenterFrom").hide();
            }

            $("#myTabs").tabs();

            if (Cookies.getJSON('outsoption') != undefined) {

                queryOutId = CommonHelper.GetParameterByName("oid");
                var issueType = CommonHelper.GetParameterByName("it");
                var requisitionOrSalesId = CommonHelper.GetParameterByName("rosid");
                var outId = CommonHelper.GetParameterByName("oid");

                PageMethods.EditItemOut(issueType, outId, requisitionOrSalesId, OnEditPurchaseOrderSucceed, OnEditPurchaseOrderFailed);
            }

            $("#ContentPlaceHolder1_txtBillNo").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '/Inventory/ItemTransfer.aspx/GetBillNoByText',
                        //data: "{'searchTerm':'" + request.term + "'}",
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.BillNumber,
                                    value: m.BillId
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
                    $("#ContentPlaceHolder1_hfBillId").val(ui.item.value);
                    if (NotTriggerChange == 0) {
                        const BillId = ui.item.value;
                        const costCenterId = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
                        const locationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();

                        if (BillId != 0 && costCenterId == 0) {
                            toastr.warning("Please Select Cost Center.");
                            $("#ContentPlaceHolder1_ddlCostCenterFrom").focus();
                            return false;
                        }
                        else if (BillId != 0 && locationId == 0) {
                            toastr.warning("Please Select Location.");
                            $("#ContentPlaceHolder1_ddlLocationFrom").focus();
                            return false;
                        }
                        if (BillId != 0)
                            LoadByBilling(BillId, costCenterId, locationId);
                    }
                }
            });

            $("#ContentPlaceHolder1_ddlCostCenterFrom").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCostCenterTo").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlRequisition").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlQuotation").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });



            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCategory").change(function () {
                $("#ContentPlaceHolder1_txtItem").val('');
                $("#ContentPlaceHolder1_hfCategoryId").val('0');
                $("#ContentPlaceHolder1_hfItemId").val('0');
                $("#ContentPlaceHolder1_txtCurrentStock").val('');
                $("#ContentPlaceHolder1_txtCurrentStockBy").val('');
                $("#ContentPlaceHolder1_txtTransferQuantity").val('');

                $("#ContentPlaceHolder1_ddlColorAttribute").empty();
                $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
                $("#ContentPlaceHolder1_ddlStyleAttribute").empty();
                return false;
            });

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

            $("#ContentPlaceHolder1_ddlOutType").change(function () {
                TransferItemAdded = new Array();
                $("#ItemForTransferTbl tbody").html("");
                $("#RequisitionWiseItemTbl tbody").html("");
                $("ContentPlaceHolder1_ddlOutType").val("0");
                $("#ContentPlaceHolder1_ddlCostCenterFrom").prop('disabled', false).val("0").trigger("change");
                $("#ContentPlaceHolder1_ddlLocationFrom").prop('disabled', false).val("0");

                $("#ContentPlaceHolder1_ddlCostCenterTo").prop('disabled', false).val("0").trigger("change");
                $("#ContentPlaceHolder1_ddlLocationTo").prop('disabled', false).val("0");
                $("#ContentPlaceHolder1_ddlRequisition").val("0").trigger("change");

                //$("#ContentPlaceHolder1_companyProjectUserControl_ddlLocationTo").attr("disabled", false);

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").attr("disabled", false);
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").attr("disabled", false);
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").attr("disabled", false);
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").attr("disabled", false);

                $("#remarksDiv").show();
                $("#btnDiv").show();

                if ($(this).val() == "Requisition") {
                    $("#RequisitionWiseItemContainer").show();
                    $("#RequisitionNumberContainer").show();
                    $("#StockTransferContainer").hide();
                    $("#QuotationNumberContainer").hide();
                    $("#SalesOrderItemContainer").hide();
                    $("#BillingOrderItemContainer").hide();
                    $("#dvCostCenterTo").show();
                    $("#dvCostCenterFrom").show();
                    $("#DivBillNo").hide();
                }
                else if ($(this).val() == "StockTransfer") {
                    $("#RequisitionWiseItemContainer").hide();
                    $("#RequisitionNumberContainer").hide();
                    $("#StockTransferContainer").show();
                    $("#QuotationNumberContainer").hide();
                    $("#SalesOrderItemContainer").hide();
                    $("#BillingOrderItemContainer").hide();
                    $("#dvCostCenterTo").show();
                    $("#dvCostCenterFrom").show();
                    $("#DivBillNo").hide();
                }
                else if ($(this).val() == "SalesOut") {
                    $("#RequisitionWiseItemContainer").hide();
                    $("#RequisitionNumberContainer").hide();
                    $("#StockTransferContainer").hide();
                    $("#QuotationNumberContainer").show();
                    $("#SalesOrderItemContainer").show();
                    $("#BillingOrderItemContainer").hide();
                    $("#dvCostCenterTo").hide();
                    $("#dvCostCenterFrom").hide();
                    $("#DivBillNo").hide();
                }
                else if ($(this).val() == "Billing") {
                    $("#RequisitionWiseItemContainer").hide();
                    $("#RequisitionNumberContainer").hide();
                    $("#StockTransferContainer").hide();
                    $("#QuotationNumberContainer").hide();
                    $("#SalesOrderItemContainer").hide();
                    $("#BillingOrderItemContainer").show();
                    $("#DivBillNo").show();
                    $("#dvCostCenterTo").hide();
                    $("#dvCostCenterFrom").hide();
                }
                else {
                    $("#RequisitionWiseItemContainer").hide();
                    $("#RequisitionNumberContainer").hide();
                    $("#StockTransferContainer").hide();
                    $("#QuotationNumberContainer").hide();
                    $("#SalesOrderItemContainer").hide();
                    $("#BillingOrderItemContainer").hide();
                    $("#DivBillNo").hide();
                    $("#remarksDiv").hide();
                    $("#btnDiv").hide();
                }
            });

            $("#ContentPlaceHolder1_ddlOutType").val("0").trigger('change');

            $("#ContentPlaceHolder1_ddlRequisition").change(function () {

                if ($(this).val() == "0") { return false; }

                var requisitionId = $(this).val();
                var fromCostcenterId = 0, fromLocationId = 0;

                var outId = $("#ContentPlaceHolder1_hfOutId").val();
                fromCostcenterId = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
                fromLocationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();

                if (requisitionId != "0" && outId == "0") {
                    //if (fromCostcenterId == "0") {
                    //    toastr.warning("Please Select From Cost Center.");
                    //    return false;
                    //}
                    //else if (fromLocationId == "0") {
                    //    toastr.warning("Please Select From Location.");
                    //    return false;
                    //}

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/ItemTransfer.aspx/LoadProductByRequisitionId',

                        data: JSON.stringify({ requisitionId: requisitionId }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            OnLoadRequisitionWiseItemSucceeded(data.d);
                        },
                        error: function (result) {
                            OnLoadRequisitionWiseItemFailed(result.d)
                        }
                    });
                    // PageMethods.LoadProductByRequisitionId(requisitionId, fromLocationId, OnLoadRequisitionWiseItemSucceeded, OnLoadRequisitionWiseItemFailed);
                }
                return false;
            });



            $("#ContentPlaceHolder1_ddlCostCenterFrom").change(function () {
                if ($(this).val() != "0") {
                    LoadLocationFrom($(this).val());
                }

            });
            $("#ContentPlaceHolder1_ddlCostCenterFrom").val('0').trigger('change');
            $("#ContentPlaceHolder1_ddlCostCenterTo").change(function () {
                if ($(this).val() != "0") {
                    LoadLocationTo($(this).val());
                }
            });
            $("#ContentPlaceHolder1_ddlCostCenterTo").val('0').trigger('change');

            $("#ContentPlaceHolder1_ddlLocationFrom").change(function () {
                if ($("#ContentPlaceHolder1_ddlOutType").val() == "SalesOut")
                    $("#ContentPlaceHolder1_ddlQuotation").trigger('change');
                else if ($("#ContentPlaceHolder1_ddlOutType").val() == "Billing" && NotTriggerChange == 0) {
                    const BillId = $("#ContentPlaceHolder1_hfBillId").val();
                    const costCenterId = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
                    const locationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();

                    if (BillId != 0 && costCenterId == 0) {
                        toastr.warning("Please Select Cost Center.");
                        $("#ContentPlaceHolder1_ddlCostCenterFrom").focus();
                        return false;
                    }
                    else if (BillId != 0 && locationId == 0) {
                        toastr.warning("Please Select Location.");
                        $("#ContentPlaceHolder1_ddlLocationFrom").focus();
                        return false;
                    }
                    if (BillId != 0)
                        LoadByBilling(BillId, costCenterId, locationId);
                }
            });

            $("#ContentPlaceHolder1_ddlQuotation").change(function () {

                if (NotTriggerChange == 0) {
                    const quotationId = $("#ContentPlaceHolder1_ddlQuotation").val();
                    const costCenterId = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
                    const locationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();

                    if (quotationId != 0 && costCenterId == 0) {
                        toastr.warning("Please Select Cost Center.");
                        $("#ContentPlaceHolder1_ddlCostCenterFrom").focus();
                        return false;
                    }
                    else if (quotationId != 0 && locationId == 0) {
                        toastr.warning("Please Select Location.");
                        $("#ContentPlaceHolder1_ddlLocationFrom").focus();
                        return false;
                    }
                    if (quotationId != 0)
                        LoadByQuotation(quotationId, costCenterId, locationId);
                }
            });

            $("#ContentPlaceHolder1_txtItem").autocomplete({

                source: function (request, response) {
                    var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                    var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
                    var costCenterId = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    var locationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();

                    if (companyId == "0") {
                        toastr.warning("Please Select Company.");
                        $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                        return false;
                    }
                    else if (projectId == "0") {
                        toastr.warning("Please Select Project.");
                        $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                        return false;
                    }
                    else if (costCenterId == "0") {
                        toastr.warning("Please Select Store.");
                        $("#ContentPlaceHolder1_ddlCategory").focus();
                        return false;
                    }
                    else if (locationId == "0") {
                        toastr.warning("Please Select Store Location.");
                        $("#ContentPlaceHolder1_ddlLocationFrom").focus();
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/ItemTransfer.aspx/ItemSearch',
                        data: JSON.stringify({ searchTerm: request.term, companyId: companyId, projectId: projectId, costCenterId: costCenterId, categoryId: categoryId, locationId: locationId }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    ItemName: m.Name,
                                    ItemId: m.ItemId,
                                    CategoryId: m.CategoryId,
                                    ProductType: m.ProductType,
                                    StockBy: m.StockBy,
                                    UnitHead: m.UnitHead,
                                    StockQuantity: m.StockQuantity,
                                    PurchasePrice: m.PurchasePrice,
                                    LastPurchaseDate: m.LastPurchaseDate
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

                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfItemId").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfCategoryId").val(ui.item.CategoryId);
                    $("#ContentPlaceHolder1_hfStockById").val(ui.item.StockBy);
                    //$("#ContentPlaceHolder1_txtCurrentStock").val(ui.item.StockQuantity).prop("disabled", true);
                    $("#ContentPlaceHolder1_txtCurrentStockBy").val(ui.item.UnitHead).prop("disabled", true);

                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Color');
                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Size');
                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Style');

                    var companyId = 0;
                    companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                    var projectId = 0;
                    projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

                    var locationId = parseInt($('#ContentPlaceHolder1_ddlLocationFrom').val(), 10);
                    var colorddlLength = $('#ContentPlaceHolder1_ddlColorAttribute > option').length;
                    var sizeddlLength = $('#ContentPlaceHolder1_ddlSizeAttribute > option').length;
                    var styleddlLength = $('#ContentPlaceHolder1_ddlStyleAttribute > option').length;
                    var colorId = 0;
                    if (colorddlLength > 0) {
                        colorId = parseInt($("#ContentPlaceHolder1_ddlColorAttribute option:selected").val(), 10);
                    }
                    var sizeId = 0;
                    if (sizeddlLength > 0) {
                        sizeId = parseInt($("#ContentPlaceHolder1_ddlSizeAttribute option:selected").val(), 10);
                    }
                    var styleId = 0;
                    if (styleddlLength > 0) {
                        styleId = parseInt($("#ContentPlaceHolder1_ddlStyleAttribute option:selected").val(), 10);
                    }
                    GetInvItemStockInfoByItemAndAttributeId(companyId, projectId, ui.item.value, colorId, sizeId, styleId, locationId);
                }
            })
            $('#ContentPlaceHolder1_ddlColorAttribute').change(function () {
                var companyId = 0;
                companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                var projectId = 0;
                projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

                var locationId = parseInt($('#ContentPlaceHolder1_ddlLocationFrom').val(), 10);
                var colorddlLength = $('#ContentPlaceHolder1_ddlColorAttribute > option').length;
                var sizeddlLength = $('#ContentPlaceHolder1_ddlSizeAttribute > option').length;
                var styleddlLength = $('#ContentPlaceHolder1_ddlStyleAttribute > option').length;
                var colorId = 0;
                if (colorddlLength > 0) {
                    colorId = parseInt($("#ContentPlaceHolder1_ddlColorAttribute option:selected").val(), 10);
                }
                var sizeId = 0;
                if (sizeddlLength > 0) {
                    sizeId = parseInt($("#ContentPlaceHolder1_ddlSizeAttribute option:selected").val(), 10);
                }
                var styleId = 0;
                if (styleddlLength > 0) {
                    styleId = parseInt($("#ContentPlaceHolder1_ddlStyleAttribute option:selected").val(), 10);
                }
                GetInvItemStockInfoByItemAndAttributeId(companyId, projectId, parseInt($("#ContentPlaceHolder1_hfItemId").val(), 10), colorId, sizeId, styleId, locationId);
            });
            $('#ContentPlaceHolder1_ddlSizeAttribute').change(function () {
                var companyId = 0;
                companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                var projectId = 0;
                projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

                var locationId = parseInt($('#ContentPlaceHolder1_ddlLocationFrom').val(), 10);
                var colorddlLength = $('#ContentPlaceHolder1_ddlColorAttribute > option').length;
                var sizeddlLength = $('#ContentPlaceHolder1_ddlSizeAttribute > option').length;
                var styleddlLength = $('#ContentPlaceHolder1_ddlStyleAttribute > option').length;
                var colorId = 0;
                if (colorddlLength > 0) {
                    colorId = parseInt($("#ContentPlaceHolder1_ddlColorAttribute option:selected").val(), 10);
                }
                var sizeId = 0;
                if (sizeddlLength > 0) {
                    sizeId = parseInt($("#ContentPlaceHolder1_ddlSizeAttribute option:selected").val(), 10);
                }
                var styleId = 0;
                if (styleddlLength > 0) {
                    styleId = parseInt($("#ContentPlaceHolder1_ddlStyleAttribute option:selected").val(), 10);
                }
                GetInvItemStockInfoByItemAndAttributeId(companyId, projectId, parseInt($("#ContentPlaceHolder1_hfItemId").val(), 10), colorId, sizeId, styleId, locationId);
            });
            $('#ContentPlaceHolder1_ddlStyleAttribute').change(function () {
                var companyId = 0;
                companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                var projectId = 0;
                projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

                var locationId = parseInt($('#ContentPlaceHolder1_ddlLocationFrom').val(), 10);
                var colorddlLength = $('#ContentPlaceHolder1_ddlColorAttribute > option').length;
                var sizeddlLength = $('#ContentPlaceHolder1_ddlSizeAttribute > option').length;
                var styleddlLength = $('#ContentPlaceHolder1_ddlStyleAttribute > option').length;
                var colorId = 0;
                if (colorddlLength > 0) {
                    colorId = parseInt($("#ContentPlaceHolder1_ddlColorAttribute option:selected").val(), 10);
                }
                var sizeId = 0;
                if (sizeddlLength > 0) {
                    sizeId = parseInt($("#ContentPlaceHolder1_ddlSizeAttribute option:selected").val(), 10);
                }
                var styleId = 0;
                if (styleddlLength > 0) {
                    styleId = parseInt($("#ContentPlaceHolder1_ddlStyleAttribute option:selected").val(), 10);
                }
                GetInvItemStockInfoByItemAndAttributeId(companyId, projectId, parseInt($("#ContentPlaceHolder1_hfItemId").val(), 10), colorId, sizeId, styleId, locationId);
            });
            $("#txtSerialAutoComplete").autocomplete({
                minLength: 3,
                source: function (request, response) {
                    var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                    var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
                    var itemId = $("#ContentPlaceHolder1_hfItemIdForSerial").val();
                    var locationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/ItemTransfer.aspx/SerialSearch',
                        data: JSON.stringify({ serialNumber: request.term, companyId: companyId, projectId: projectId, locationId: locationId, itemId: itemId }),
                        dataType: "json",
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
            var isfrmSNote = CommonHelper.GetParameterByName("isfrmSNote") == "1";
            if (isfrmSNote) {
                $("#ContentPlaceHolder1_ddlOutType").val("SalesOut").trigger('change').attr('disabled', true);
                var mainStoreId = $("#ContentPlaceHolder1_hfMainStoreId").val();
                if (mainStoreId != "0")
                    $("#ContentPlaceHolder1_ddlCostCenterFrom").val(mainStoreId).trigger('change').attr('disabled', true);
            }
            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                $("#AttributeDiv").show();
                $("#cId").show();
                $("#sId").show();
                $("#stId").show();
                $("#stcId").show();
                $("#stsId").show();
                $("#ststId").show();
            }
            else {
                $("#AttributeDiv").hide();
                $("#cId").hide();
                $("#sId").hide();
                $("#stId").hide();
                $("#stcId").hide();
                $("#stsId").hide();
                $("#ststId").hide();
            }
        });


        function LoadByQuotation(quotationId, costCenterId, locationId) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/ItemTransfer.aspx/GetQuotationDetailsByQuotationId',

                data: JSON.stringify({ quotationId: quotationId, costCenterId: costCenterId, locationId: locationId }),
                dataType: "json",
                success: function (data) {
                    $("#SalesOrderItemTbl tbody").empty();
                    if (data.d.length <= 0) {
                        toastr.info("Item Is Already Used In Another Transfer Order. Please Search and Check the Report.");
                        return false;
                    }
                    LoadTableByQuotationId(data);

                },
                error: function (result) {

                }
            });
        }

        function LoadByBilling(billingId, costCenterId, locationId) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/ItemTransfer.aspx/GetBillingDetailsByBillingId',

                data: JSON.stringify({ BillingId: billingId, costCenterId: costCenterId, locationId: locationId }),
                dataType: "json",
                success: function (data) {
                    $("#BillingOrderItemTbl tbody").empty();
                    if (data.d.length <= 0) {
                        toastr.info("Item Is Already Used In Another Transfer Order. Please Search and Check the Report.");
                        return false;
                    }
                    LoadTableByBillingId(data);

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
                tr += "<td style='width:10%;'>" + gridObject.Quantity + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.DeliveredQuantity + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.RemainingDeliveryQuantity + "</td>";

                if (gridObject.StockQuantity > 0) {
                    tr += "<td style='width:10%;'>" + gridObject.StockQuantity + "</td>";
                } else
                    tr += "<td style='width:10%;'> 0 </td>";
                tr += "<td style='width:15%;'>" + gridObject.HeadName + "</td>";

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
        function LoadTableByBillingId(data) {

            $("#BillingOrderItemTbl tbody").empty();
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

                tr += "<input type='checkbox' checked='checked' id='chk' " + gridObject.KotDetailId + " />"

                tr += "</td>";

                tr += "<td style='width:20%;'>" + gridObject.ItemName + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.Quantity + "</td>";
                var alreayDelivered = (gridObject.Quantity - gridObject.RemainingDeliveryQuantity).toFixed(2);
                tr += "<td style='width:10%;'>" + alreayDelivered + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.RemainingDeliveryQuantity + "</td>";

                if (gridObject.StockQuantity > 0) {
                    tr += "<td style='width:10%;'>" + gridObject.StockQuantity + "</td>";
                } else
                    tr += "<td style='width:10%;'> 0 </td>";
                tr += "<td style='width:15%;'>" + gridObject.HeadName + "</td>";

                if (gridObject.RemainingDeliveryQuantity <= 0) {
                    tr += "<td style='width:15%;'>" + "<input disabled type='text' value='" + gridObject.RemainingDeliveryQuantity + "' id='q' " + gridObject.Id + " class='form-control quantitydecimal' onchange='CheckBillingOrderWiseQuantity(this)' '/>" + "</td>";
                }
                else {
                    tr += "<td style='width:15%;'>" + "<input type='text' value='" + gridObject.RemainingDeliveryQuantity + "' id='q' " + gridObject.Id + " class='form-control quantitydecimal' onchange='CheckBillingOrderWiseQuantity(this)' '/>" + "</td>";
                }

                tr += "<td style='width:5%;'>";
                if (gridObject.ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForPurchaseWiseItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";
                tr += "<td style='display:none'>" + gridObject.ItemId + "</td>";
                tr += "<td style='display:none'>" + gridObject.StockBy + "</td>";
                tr += "<td style='display:none'>" + gridObject.UnitPrice + "</td>";
                tr += "<td style='display:none'>" + gridObject.Quantity + "</td>";
                tr += "<td style='display:none'>" + gridObject.RemainingDeliveryQuantity + "</td>";
                tr += "<td style='display:none'>" + gridObject.ProductType + "</td>";

                tr += "</tr>";

                $("#BillingOrderItemTbl tbody").append(tr);

                tr = "";
                i++;
            });
            CommonHelper.ApplyDecimalValidation();
            CommonHelper.ApplyIntValidation();
            return false;
        }

        function CheckPurchaseOrderWiseQuantity(control) {
            var tr = $(control).parent().parent();
            var qty = $.trim($(control).val());
            var max = parseFloat($(tr).find("td:eq(4)").text());
            var stock = parseFloat($(tr).find("td:eq(5)").text());

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
        function CheckBillingOrderWiseQuantity(control) {
            var tr = $(control).parent().parent();
            var qty = $.trim($(control).val());
            var max = parseFloat($(tr).find("td:eq(4)").text());
            var stock = parseFloat($(tr).find("td:eq(5)").text());

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

        }

        function LoadLocationFrom(costCenetrId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/ItemTransfer.aspx/InvLocationByCostCenter',

                data: JSON.stringify({ costCenterId: costCenetrId }),
                dataType: "json",
                async: false,
                success: function (data) {
                    OnLoadLocationFromSucceeded(data.d);
                },
                error: function (result) {
                    OnLoadLocationFailed(result.d);
                }
            });
            //PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationFromSucceeded, OnLoadLocationFailed);
        }
        function LoadLocationTo(costCenetrId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/ItemTransfer.aspx/InvLocationByCostCenter',

                data: JSON.stringify({ costCenterId: costCenetrId }),
                dataType: "json",
                async: false,
                success: function (data) {
                    OnLoadLocationToSucceeded(data.d);
                },
                error: function (result) {
                    OnLoadLocationFailed(result.d);
                }
            });
            //PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationToSucceeded, OnLoadLocationFailed);
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
                    if (result.length == 1)
                        $("#ContentPlaceHolder1_ddlLocationFrom").val(result[0].LocationId).trigger('change');
                }
                else {
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }
            var a = $("#ContentPlaceHolder1_hfLocationFromId").val();
            if (result.length != 1 && $("#ContentPlaceHolder1_hfLocationFromId").val() == "0")
                control.val($("#ContentPlaceHolder1_ddlLocationFrom option:first").val());
            else
                control.val($("#ContentPlaceHolder1_hfLocationFromId").val());
            var isfrmSNote = CommonHelper.GetParameterByName("isfrmSNote") == "1";
            if (isfrmSNote) {

                var defaultLocationId = $("#ContentPlaceHolder1_hfDefaultLocationId").val();

                if (defaultLocationId != "0") {
                    $("#ContentPlaceHolder1_ddlLocationFrom").val(defaultLocationId).trigger('change').attr('disabled', true);
                }
                var qid = CommonHelper.GetParameterByName("qid");
                $("#ContentPlaceHolder1_ddlQuotation").val(qid).trigger('change').attr('disabled', true);
            }

            return false;
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
            var location = $("#ContentPlaceHolder1_hfLocationFromId").val();
            if (result.length > 1 && $("#ContentPlaceHolder1_hfLocationFromId").val() == "0")
                control.val($("#ContentPlaceHolder1_ddlLocationTo option:first").val());
            else
                control.val($("#ContentPlaceHolder1_hfLocationToId").val());
            return false;
        }
        function OnLoadLocationFailed() { }

        function OnLoadRequisitionWiseItemSucceeded(result) {

            if (result.RequisitionDetails.length == 0) {
                toastr.info("Item Is Already Used In Another Transfer Order. Please Search and Check the Report.");
                $("#RequisitionWiseItemTbl tbody").html("");
                return false;
            }
            $("#ContentPlaceHolder1_hfLocationToId").val(result.Requisition.FromLocationId);
            $("#ContentPlaceHolder1_hfLocationFromId").val(result.Requisition.ToLocationId);
            $("#ContentPlaceHolder1_ddlCostCenterTo").val(result.Requisition.FromCostCenterId).trigger('change');
            $("#ContentPlaceHolder1_ddlCostCenterFrom").val(result.Requisition.ToCostCenterId).trigger('change');

            if (result.Requisition.CompanyId > 0) {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(result.Requisition.CompanyId).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").prop('disabled', true);

                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").val(result.Requisition.CompanyId).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").prop('disabled', true);
            }
            if (result.Requisition.ProjectId > 0) {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(result.Requisition.ProjectId).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").prop('disabled', true);

                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").val(result.Requisition.ProjectId).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").prop('disabled', true);
            }

            $("#ContentPlaceHolder1_ddlCostCenterTo").prop('disabled', true);
            $("#ContentPlaceHolder1_ddlCostCenterFrom").prop('disabled', true);
            $("#ContentPlaceHolder1_ddlLocationFrom").prop('disabled', false);
            $("#ContentPlaceHolder1_ddlLocationTo").prop('disabled', true);

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").attr("disabled", false);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").attr("disabled", false);

            $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").attr("disabled", true);
            $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").attr("disabled", true);

            //$.ajax({
            //    type: "POST",
            //    contentType: "application/json; charset=utf-8",
            //    url: '../Inventory/ItemTransfer.aspx/LoadProductByRequisitionId',

            //    data: JSON.stringify({ requisitionId: result.Requisition.RequisitionId, locationId: $("#ContentPlaceHolder1_ddlLocationFrom").val() }),
            //    dataType: "json",
            //    async: false,
            //    success: function (data) {
            //        $("#OrderCheck").prop("checked", true);
            $("#RequisitionWiseItemTbl tbody").html("");
            var totalRow = result.RequisitionDetails.length, row = 0;
            var tr = "";

            TransferItemAdded = result.RequisitionDetails;

            for (row = 0; row < totalRow; row++) {

                tr += "<tr>";

                if (result.RequisitionDetails[row].ApprovedQuantity > 0) {
                    tr += "<td style='width:5%;'>" +
                        "<input type='checkbox' checked='checked' id='chk" + result.RequisitionDetails[row].ItemId + "'" + " />" +
                        "</td>";
                }
                else {
                    tr += "<td style='width:5%;'></td>";
                }

                tr += "<td style='width:15%;'>" + result.RequisitionDetails[row].ItemName + "</td>";
                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    tr += "<td style='width:10%;'>" + result.RequisitionDetails[row].ColorText + "</td>";
                    tr += "<td style='width:10%;'>" + result.RequisitionDetails[row].SizeText + "</td>";
                    tr += "<td style='width:10%;'>" + result.RequisitionDetails[row].StyleText + "</td>";
                } else {
                    tr += "<td style='display:none;'>" + result.RequisitionDetails[row].ColorText + "</td>";
                    tr += "<td style='display:none;'>" + result.RequisitionDetails[row].SizeText + "</td>";
                    tr += "<td style='display:none;'>" + result.RequisitionDetails[row].StyleText + "</td>";
                }

                tr += "<td style='width:7%;'>" + result.RequisitionDetails[row].ApprovedQuantity + "</td>";
                tr += "<td style='width:7%;'>" + result.RequisitionDetails[row].DeliveredQuantity + "</td>";
                tr += "<td style='width:7%;'>" + result.RequisitionDetails[row].RemainingTransferQuantity + "</td>";
                tr += "<td style='width:7%;'>" + result.RequisitionDetails[row].StockQuantity + "</td>";
                tr += "<td style='width:7%;'>" + result.RequisitionDetails[row].HeadName + "</td>";

                tr += "<td style='width:7%;'>" +
                    "<input type='text' value='" + result.RequisitionDetails[row].RemainingTransferQuantity + "' id='q' " + result.RequisitionDetails[row].ItemId + " class='form-control quantitydecimal' onblur='CheckTransferQuantity(this)' />" +
                    "</td>";

                tr += "<td style='width:5%;'>";
                if (result.RequisitionDetails[row].ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForPurchaseWiseItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].ItemId + "</td>";
                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].StockById + "</td>";
                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].RequisitionDetailsId + "</td>";
                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].OutDetailsId + "</td>";
                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].ProductType + "</td>";
                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].RemainingTransferQuantity + "</td>";
                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].ColorId + "</td>";
                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].SizeId + "</td>";
                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].StyleId + "</td>";

                tr += "</tr>";

                $("#RequisitionWiseItemTbl tbody").append(tr);
                tr = "";
            }

            CommonHelper.ApplyDecimalValidation();
            //    },
            //    error: function (result) {
            //        OnLoadRequisitionWiseItemFailed(result.d)
            //    }
            //});

        }
        function OnLoadRequisitionWiseItemFailed(result) { }

        function AddItemForTransfer() {

            var quantity = 0, tr = "", colorId = 0, sizeId = 0, styleId = 0, color, size, style;
            quantity = $("#ContentPlaceHolder1_txtTransferQuantity").val();
            var stockQty = $("#ContentPlaceHolder1_txtCurrentStock").val();

            if (ItemSelected == null) {
                toastr.warning("Please Select Item.");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtTransferQuantity").val()) == "" || $.trim($("#ContentPlaceHolder1_txtTransferQuantity").val()) == "0") {
                toastr.warning("Please Give Transfer Quantity.");
                return false;
            }
            else if (stockQty < 0) {
                toastr.warning("Transfer Quantity Does Not Greater Than Stock Quantity.");
                return false;
            }
            else if (parseFloat(quantity) > stockQty) {
                toastr.warning("Transfer Quantity Does Not Greater Than Stock Quantity.");
                return false;
            }
            else if (parseFloat(stockQty) <= 0) {
                toastr.warning("Stock Quantity Can Not Zero.");
                return false;
            }

            var colorddlLength = $('#ContentPlaceHolder1_ddlColorAttribute > option').length;
            var sizeddlLength = $('#ContentPlaceHolder1_ddlSizeAttribute > option').length;
            var styleddlLength = $('#ContentPlaceHolder1_ddlStyleAttribute > option').length;
            var colorId = 0;
            if (colorddlLength > 0) {
                colorId = parseInt($("#ContentPlaceHolder1_ddlColorAttribute option:selected").val(), 10);
            }
            var color = $("#ContentPlaceHolder1_ddlColorAttribute option:selected").text();
            var sizeId = 0;
            if (sizeddlLength > 0) {
                sizeId = parseInt($("#ContentPlaceHolder1_ddlSizeAttribute option:selected").val(), 10);
            }
            var size = $("#ContentPlaceHolder1_ddlSizeAttribute option:selected").text();
            var styleId = 0;
            if (styleddlLength > 0) {
                styleId = parseInt($("#ContentPlaceHolder1_ddlStyleAttribute option:selected").val(), 10);
            }
            var style = $("#ContentPlaceHolder1_ddlStyleAttribute option:selected").text();

            var itm = _.findWhere(TransferItemAdded, { ItemId: ItemSelected.ItemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId });

            if (itm != null) {
                toastr.warning("Same Item Already Added. Duplicate Item Is Not Accepted.");
                return false;
            }

            tr += "<tr>";

            tr += "<td style='width:24%;'>" + ItemSelected.ItemName + "</td>";
            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                tr += "<td style='width:7%;'>" + color + "</td>";
                tr += "<td style='width:7%;'>" + size + "</td>";
                tr += "<td style='width:15%;'>" + style + "</td>";
            }
            else {
                tr += "<td style='display:none'>" + color + "</td>";
                tr += "<td style='display:none'>" + size + "</td>";
                tr += "<td style='display:none'>" + style + "</td>";
            }
            tr += "<td style='width:10%;'>" + stockQty + "</td>";
            tr += "<td style='width:12%;'>" +
                "<input type='text' value='" + quantity + "' id='pp" + ItemSelected.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                "</td>";

            tr += "<td style='width:10%;'>" + ItemSelected.UnitHead + "</td>";

            tr += "<td style='width:5%;'>";
            tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            if (ItemSelected.ProductType == 'Serial Product') {
                tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForAdHocItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
            }
            tr += "</td>";

            tr += "<td style='display:none;'>" + ItemSelected.ItemId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.CategoryId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.StockBy + "</td>";
            tr += "<td style='display:none;'>" + quantity + "</td>";
            tr += "<td style='display:none;'>0</td>";
            tr += "<td style='display:none;'>" + colorId + "</td>";
            tr += "<td style='display:none;'>" + sizeId + "</td>";
            tr += "<td style='display:none;'>" + styleId + "</td>";

            tr += "</tr>";

            $("#ItemForTransferTbl tbody").prepend(tr);
            tr = "";

            TransferItemAdded.push({
                ItemId: parseInt(ItemSelected.ItemId, 10),
                ItemName: ItemSelected.ItemName,
                ProductType: ItemSelected.ProductType,
                StockById: parseInt(ItemSelected.StockBy, 10),
                Quantity: parseFloat(quantity),
                StockQuantity: stockQty,
                ColorId: parseInt(colorId, 10),
                SizeId: parseInt(sizeId, 10),
                StyleId: parseInt(styleId, 10),
                DetailId: 0
            });

            CommonHelper.ApplyDecimalValidation();
            ClearAfterTransferItemAdded();
            $("#ContentPlaceHolder1_txtItem").focus();
        }
        function ClearAfterTransferItemAdded() {
            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_txtCurrentStock").val("").prop("disabled", false);
            $("#ContentPlaceHolder1_txtCurrentStockBy").val("").prop("disabled", false);
            $("#ContentPlaceHolder1_txtTransferQuantity").val("");
            $("#ContentPlaceHolder1_ddlColorAttribute").empty();
            $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
            $("#ContentPlaceHolder1_ddlStyleAttribute").empty();
            ItemSelected = null;
        }

        function AddSerialForAdHocItem(control) {
            var tr = $(control).parent().parent();

            var itemName = $(tr).find("td:eq(0)").text();
            var itemId = $(tr).find("td:eq(8)").text();
            var quantity = $(tr).find("td:eq(5)").find("input").val();

            SearialAddedWindow(itemName, itemId, quantity);
        }
        function AddSerialForPurchaseWiseItem(control) {
            var tr = $(control).parent().parent();

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
                        tr += "<td style='display:none;'>" + addedSerial[row].OutSerialId + "</td>";

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

            $(NewAddedSerial).each(function (index, obj) {

                AddedSerialzableProduct.push({
                    OutSerialId: obj.OutSerialId,
                    ItemId: obj.ItemId,
                    SerialNumber: obj.SerialNumber
                });
            });

            $("#SerialWindow").dialog("close");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("");
            AddedSerialCount = 0;
            NewAddedSerial = new Array();
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

        function CalculateTotalForAdhoq(control) {
            var tr = $(control).parent().parent();

            var stockQuantity = $.trim($(tr).find("td:eq(4)").text());
            var quantity = $.trim($(tr).find("td:eq(5)").find("input").val());
            var oldQuantity = $(tr).find("td:eq(11)").text();
            var colorId = parseInt($(tr).find("td:eq(13)").text(), 10);
            var sizeId = parseInt($(tr).find("td:eq(14)").text(), 10);
            var styleId = parseInt($(tr).find("td:eq(15)").text(), 10);

            if (quantity == "" || quantity == "0") {
                toastr.info("Quantity Cannot Be Zero Or Empty.");
                $(tr).find("td:eq(5)").find("input").val(oldQuantity);
                return false;
            }
            else if (parseFloat(quantity) > parseFloat(stockQuantity)) {
                toastr.info("Transfer Quantity Cannot Greater Than Stock Quantity.");
                $(tr).find("td:eq(5)").find("input").val(oldQuantity);
                return false;
            }

            var itemId = parseInt($.trim($(tr).find("td:eq(8)").text()), 10);

            var item = _.findWhere(TransferItemAdded, { ItemId: itemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId });
            // var itm = _.findWhere(TransferItemAdded, { ItemId: ItemSelected.ItemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId });

            var index = _.indexOf(TransferItemAdded, item);

            TransferItemAdded[index].Quantity = parseFloat(quantity);
            $(tr).find("td:eq(11)").text(quantity);
        }
        function DeleteAdhoqItem(control) {

            if (!confirm("Do you want to delete item?")) { return false; }

            var tr = $(control).parent().parent();
            var itemId = parseInt($.trim($(tr).find("td:eq(8)").text()), 10);
            var outDetailsId = parseInt($.trim($(tr).find("td:eq(12)").text()), 10);
            var colorId = parseInt($(tr).find("td:eq(13)").text(), 10);
            var sizeId = parseInt($(tr).find("td:eq(14)").text(), 10);
            var styleId = parseInt($(tr).find("td:eq(15)").text(), 10);

            var item = _.findWhere(TransferItemAdded, { ItemId: itemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId });
            var index = _.indexOf(TransferItemAdded, item);

            if (parseInt(outDetailsId, 10) > 0)
                TransferItemDeleted.push(JSON.parse(JSON.stringify(item)));

            TransferItemAdded.splice(index, 1);
            $(tr).remove();

            var serialCount = 0, rowSerial = 0;
            var itemSerial = _.where(AddedSerialzableProduct, { ItemId: itemId });
            serialCount = itemSerial.length;

            for (rowSerial = 0; rowSerial < serialCount; rowSerial++) {

                if (itemSerial[rowSerial].OutSerialId > 0)
                    DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(itemSerial[rowSerial])));

                var srlItem = _.findWhere(AddedSerialzableProduct, { SerialNumber: itemSerial[rowSerial].SerialNumber });
                var srlIndex = _.indexOf(AddedSerialzableProduct, srlItem);
                AddedSerialzableProduct.splice(srlIndex, 1);
            }
        }

        function CheckTransferQuantity(control) {

            var tr = $(control).parent().parent();
            var outQuantity = $.trim($(control).val());
            var remainingQuantity = parseFloat($(tr).find("td:eq(7)").text());
            var oldQuantity = parseFloat($(tr).find("td:eq(17)").text());
            var stockQuantity = parseFloat($(tr).find("td:eq(8)").text());

            var colorId = parseInt($(tr).find("td:eq(18)").text(), 10);
            var sizeId = parseInt($(tr).find("td:eq(19)").text(), 10);
            var styleId = parseInt($(tr).find("td:eq(20)").text(), 10);

            if (outQuantity == "" || outQuantity == "0") {
                toastr.warning("Quantity Cannot Empty Or Zero.");
                $(control).val(oldQuantity);
                outQuantity = oldQuantity;
            }
            else if (outQuantity > parseFloat(remainingQuantity)) {
                toastr.warning("Out Quantity Cannot Greater Than Max Transfer Quantiy.");
                $(control).val(oldQuantity);
                outQuantity = oldQuantity;
            }
            else if (outQuantity > stockQuantity) {
                toastr.warning("Out Quantity Cannot Greater Than Stock Quantiy.");
                $(control).val(oldQuantity);
                outQuantity = oldQuantity;
            }

            var itemId = parseInt($.trim($(tr).find("td:eq(12)").text()), 10);

            var item = _.findWhere(TransferItemAdded, { ItemId: itemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId });
            var index = _.indexOf(TransferItemAdded, item);
            TransferItemAdded[index].Quantity = parseFloat(outQuantity);

            $(tr).find("td:eq(17)").text(outQuantity);

        }
        function DeleteReceiveFromPurchaseItem(control) {

            if (!confirm("Do you want to delete item?")) { return false; }

            var tr = $(control).parent().parent();
            var itemId = parseInt($.trim($(tr).find("td:eq(8)").text()), 10);
            var reqRow = 0, reqCount = 0;

            var deletedItem = _.where(ReceiveOrderItemFromPurchase, { ItemId: itemId });

            reqCount = deletedItem.length;

            for (reqRow = 0; reqRow < reqCount; reqRow++) {

                if (deletedItem[reqRow].ReceiveDetailsId > 0)
                    ReceiveOrderItemDeleted.push(JSON.parse(JSON.stringify(deletedItem[reqRow])));

                var reqItem = _.findWhere(ReceiveOrderItemFromPurchase, { ItemId: itemId });
                var reqIndex = _.indexOf(ReceiveOrderItemFromPurchase, reqItem);
                ReceiveOrderItemFromPurchase.splice(reqIndex, 1);
            }

            var serialCount = 0, rowSerial = 0;
            var itemSerial = _.where(AddedSerialzableProduct, { ItemId: itemId });
            serialCount = itemSerial.length;

            for (rowSerial = 0; rowSerial < serialCount; rowSerial++) {

                if (itemSerial[rowSerial].OutSerialId > 0)
                    DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(itemSerial[rowSerial])));

                var srlItem = _.findWhere(AddedSerialzableProduct, { SerialNumber: itemSerial[rowSerial].SerialNumber });
                var srlIndex = _.indexOf(AddedSerialzableProduct, srlItem);
                AddedSerialzableProduct.splice(srlIndex, 1);
            }

            $(tr).remove();
        }


        function SaveItemOutOrder() {

            if ($("#ContentPlaceHolder1_ddlOutType").val() == "0" || $("#ContentPlaceHolder1_ddlOutType").val() == "") {
                toastr.warning("Please Select Out Type.");
                return false;
            }

            if ($("#ContentPlaceHolder1_ddlOutType").val() == "StockTransfer") {
                if ($("#ItemForTransferTbl tbody tr").length == 0) {
                    toastr.warning("Please Add Item For Transfer.");
                    return false;
                }
            }
            else if ($("#ContentPlaceHolder1_ddlOutType").val() == "Requisition") {
                if ($("#RequisitionWiseItemTbl tbody tr").length == 0) {
                    toastr.warning("Please Add Item From Requisition Order For Transfer.");
                    return false;
                }
            }

            var itemId = 0, remarks = "";
            var purchaseItem = null;

            var outOrderId = "0", issueType = "", receivedByDate = null, outDetailsId = 0, isEdited = "0", productOutFor = '',
                categoryId = "", fromCostCenterId = "", fromLocationId = 0, toCostCenterId = 0, toLocationId = "", requisitionId = '',
                quotationId = 0, GLCompanyId = "0", GLProjectId = "0", toGLCompanyId = "0", toGLProjectId = "0";

            outOrderId = $("#ContentPlaceHolder1_hfOutId").val();
            fromCostCenterId = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
            fromLocationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();
            toCostCenterId = $("#ContentPlaceHolder1_ddlCostCenterTo").val();
            toLocationId = $("#ContentPlaceHolder1_ddlLocationTo").val();
            requisitionId = $("#ContentPlaceHolder1_ddlRequisition").val();
            quotationId = $("#ContentPlaceHolder1_ddlQuotation").val();
            issueType = $("#ContentPlaceHolder1_ddlOutType").val();
            productOutFor = $("#ContentPlaceHolder1_ddlOutType").val();

            GLCompanyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            GLProjectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            if (GLCompanyId == null)
                GLCompanyId = "0";
            if (GLProjectId == null)
                GLProjectId = "0";

            toGLCompanyId = $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").val();
            toGLProjectId = $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").val();
            if (toGLCompanyId == null)
                toGLCompanyId = "0";
            if (toGLProjectId == null)
                toGLProjectId = "0";

            if (productOutFor == 'Requisition') {
                issueType = 'Requisition Transfer';
            }
            else if (productOutFor == 'StockTransfer') {
                issueType = 'Stock Transfer';
            }
            else if (productOutFor == 'SalesOut') {
                issueType = 'Sales Out';
            }

            if (fromCostCenterId == "" || fromCostCenterId == "0") {
                toastr.warning("Please Select From Store.");
                return false;
            }
            else if (fromLocationId == "" || fromLocationId == 0) {
                toastr.warning("Please Select From Location.");
                return false;
            }
            else if (productOutFor != 'SalesOut' && productOutFor != 'Billing' && (toCostCenterId == 0 || toCostCenterId == "")) {
                toastr.warning("Please Select To Store.");
                return false;
            }
            else if (productOutFor != 'SalesOut' && productOutFor != 'Billing' && (toLocationId == "" || toLocationId == "0")) {
                toastr.warning("Please Select To Location.");
                return false;
            }

            if (GLCompanyId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                toastr.warning("Please Select From Company.");
                return false;
            }
            if (GLProjectId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                toastr.warning("Please Select From Project.");
                return false;
            }

            if (toGLCompanyId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").focus();
                toastr.warning("Please Select To Company.");
                return false;
            }
            if (toGLProjectId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").focus();
                toastr.warning("Please Select To Project.");
                return false;
            }

            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            checkedBy = 0;
            approvedBy = 0;
            var TransferItemNewlyAdded = new Array();

            currencyId = $("#ContentPlaceHolder1_ddlCurrency").val();
            convertionRate = $("#ContentPlaceHolder1_lblConversionRate").text();

            if (productOutFor == "Requisition") {

                $("#RequisitionWiseItemTbl tbody tr").each(function () {

                    outDetailsId = parseInt($(this).find("td:eq(15)").text(), 10);
                    itemId = parseInt($(this).find("td:eq(12)").text(), 10);

                    if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                        TransferItemNewlyAdded.push({
                            RequisitionDetailsId: parseInt($(this).find("td:eq(14)").text()),
                            OutDetailsId: outDetailsId,
                            OutId: parseInt(outOrderId),
                            StockById: parseInt($(this).find("td:eq(13)").text()),
                            ItemId: itemId,
                            ItemName: $(this).find("td:eq(1)").text(),
                            ProductType: $(this).find("td:eq(16)").text(),
                            Quantity: parseFloat($(this).find("td:eq(10)").find("input").val()),
                            StockQuantity: parseFloat($(this).find("td:eq(8)").text()),
                            ColorId: parseFloat($(this).find("td:eq(18)").text()),
                            SizeId: parseFloat($(this).find("td:eq(19)").text()),
                            StyleId: parseFloat($(this).find("td:eq(20)").text())
                        });
                    }
                    else if (outDetailsId > 0) {
                        TransferItemDeleted.push({
                            RequisitionDetailsId: parseInt($(this).find("td:eq(14)").text()),
                            OutDetailsId: outDetailsId,
                            OutId: parseInt(outOrderId),
                            StockById: parseInt($(this).find("td:eq(13)").text()),
                            ItemId: itemId,
                            ItemName: $(this).find("td:eq(1)").text(),
                            ProductType: $(this).find("td:eq(16)").text(),
                            Quantity: parseFloat($(this).find("td:eq(10)").find("input").val()),
                            ColorId: parseFloat($(this).find("td:eq(18)").text()),
                            SizeId: parseFloat($(this).find("td:eq(19)").text()),
                            StyleId: parseFloat($(this).find("td:eq(20)").text())
                        });

                        var serialCount = 0, rowSerial = 0;
                        var itemSerial = _.where(AddedSerialzableProduct, { ItemId: itemId });
                        serialCount = itemSerial.length;

                        for (rowSerial = 0; rowSerial < serialCount; rowSerial++) {

                            if (itemSerial[rowSerial].OutSerialId > 0)
                                DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(itemSerial[rowSerial])));

                            var srlItem = _.findWhere(AddedSerialzableProduct, { SerialNumber: itemSerial[rowSerial].SerialNumber });
                            var srlIndex = _.indexOf(AddedSerialzableProduct, srlItem);
                            AddedSerialzableProduct.splice(srlIndex, 1);
                        }
                    }
                });
            }
            else if (productOutFor == "SalesOut") {
                $("#SalesOrderItemTbl tbody tr").each(function () {

                    outDetailsId = parseInt($(this).find("td:eq(12)").text(), 10);
                    itemId = parseInt($(this).find("td:eq(9)").text(), 10);

                    if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                        TransferItemNewlyAdded.push({
                            RequisitionDetailsId: parseInt($(this).find("td:eq(11)").text()),
                            OutDetailsId: outDetailsId,
                            OutId: parseInt(outOrderId),
                            StockById: parseInt($(this).find("td:eq(10)").text()),
                            ItemId: itemId,
                            ItemName: $(this).find("td:eq(1)").text(),
                            ProductType: $(this).find("td:eq(13)").text(),
                            Quantity: parseFloat($(this).find("td:eq(7)").find("input").val()),
                            StockQuantity: parseFloat($(this).find("td:eq(5)").text())
                        });
                    }
                    else if (outDetailsId > 0) {
                        TransferItemDeleted.push({
                            RequisitionDetailsId: 0,
                            OutDetailsId: outDetailsId,
                            OutId: parseInt(outOrderId),
                            StockById: parseInt($(this).find("td:eq(10)").text()),
                            ItemId: itemId,
                            ItemName: $(this).find("td:eq(1)").text(),
                            ProductType: $(this).find("td:eq(13)").text(),
                            Quantity: parseFloat($(this).find("td:eq(7)").find("input").val())
                        });

                        var serialCount = 0, rowSerial = 0;
                        var itemSerial = _.where(AddedSerialzableProduct, { ItemId: itemId });
                        serialCount = itemSerial.length;

                        for (rowSerial = 0; rowSerial < serialCount; rowSerial++) {

                            if (itemSerial[rowSerial].OutSerialId > 0)
                                DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(itemSerial[rowSerial])));

                            var srlItem = _.findWhere(AddedSerialzableProduct, { SerialNumber: itemSerial[rowSerial].SerialNumber });
                            var srlIndex = _.indexOf(AddedSerialzableProduct, srlItem);
                            AddedSerialzableProduct.splice(srlIndex, 1);
                        }
                    }
                });
            }
            else if (productOutFor == "Billing") {
                $("#BillingOrderItemTbl tbody tr").each(function () {

                    outDetailsId = parseInt($(this).find("td:eq(12)").text(), 10);
                    itemId = parseInt($(this).find("td:eq(9)").text(), 10);

                    if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                        TransferItemNewlyAdded.push({
                            RequisitionDetailsId: parseInt($(this).find("td:eq(11)").text()),
                            OutDetailsId: outDetailsId,
                            OutId: parseInt(outOrderId),
                            StockById: parseInt($(this).find("td:eq(10)").text()),
                            ItemId: itemId,
                            ItemName: $(this).find("td:eq(1)").text(),
                            ProductType: $(this).find("td:eq(13)").text(),
                            Quantity: parseFloat($(this).find("td:eq(7)").find("input").val()),
                            StockQuantity: parseFloat($(this).find("td:eq(5)").text())
                        });
                    }
                    else if (outDetailsId > 0) {
                        TransferItemDeleted.push({
                            RequisitionDetailsId: 0,
                            OutDetailsId: outDetailsId,
                            OutId: parseInt(outOrderId),
                            StockById: parseInt($(this).find("td:eq(10)").text()),
                            ItemId: itemId,
                            ItemName: $(this).find("td:eq(1)").text(),
                            ProductType: $(this).find("td:eq(13)").text(),
                            Quantity: parseFloat($(this).find("td:eq(7)").find("input").val())
                        });

                        var serialCount = 0, rowSerial = 0;
                        var itemSerial = _.where(AddedSerialzableProduct, { ItemId: itemId });
                        serialCount = itemSerial.length;

                        for (rowSerial = 0; rowSerial < serialCount; rowSerial++) {

                            if (itemSerial[rowSerial].OutSerialId > 0)
                                DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(itemSerial[rowSerial])));

                            var srlItem = _.findWhere(AddedSerialzableProduct, { SerialNumber: itemSerial[rowSerial].SerialNumber });
                            var srlIndex = _.indexOf(AddedSerialzableProduct, srlItem);
                            AddedSerialzableProduct.splice(srlIndex, 1);
                        }
                    }
                });
            }
            else {
                TransferItemNewlyAdded = TransferItemAdded;
            }

            var row = 0, rowCount = TransferItemNewlyAdded.length;
            if (productOutFor != 'Billing') {

                for (row = 0; row < rowCount; row++) {
                    if (TransferItemNewlyAdded[row].Quantity > TransferItemNewlyAdded[row].StockQuantity) {
                        toastr.warning("Transfer Quantity Cannot Greater Than Stock Quantity Of Product " + TransferItemNewlyAdded[row].ItemName);
                        break;
                    }
                }
                if (row != rowCount) {
                    return false;
                }
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

            if (productOutFor == 'StockTransfer') {
                requisitionId = 0;
            }
            else if (productOutFor == 'SalesOut') {
                requisitionId = quotationId;
            }
            else if (productOutFor == 'Billing') {
                requisitionId = $("#ContentPlaceHolder1_hfBillId").val();
            }
            var ProductOut = {
                OutId: outOrderId,
                ProductOutFor: productOutFor,
                RequisitionOrSalesId: requisitionId,
                OutFor: 0,
                IssueType: issueType,
                Remarks: remarks,
                GLCompanyId: GLCompanyId,
                GLProjectId: GLProjectId,
                FromLocationId: fromLocationId,
                FromCostCenterId: fromCostCenterId,
                ToCostCenterId: toCostCenterId,
                ToLocationId: toLocationId,
                ToGLCompanyId: toGLCompanyId,
                ToGLProjectId: toGLProjectId
            };

            PageMethods.SaveItemOutOrder(ProductOut, TransferItemNewlyAdded, TransferItemDeleted, AddedSerialzableProduct, DeletedSerialzableProduct, OnSavePurchaseOrderSucceed, OnSavePurchaseOrderFailed);

            return false;
        }
        function OnSavePurchaseOrderSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                LoadNotReceivedRequisitionOrder();

                if (queryOutId != "") {
                    window.location = "/Inventory/ItemTransferInformation.aspx";
                }
                var isfrmSNote = CommonHelper.GetParameterByName("isfrmSNote") == "1";
                if (isfrmSNote) {
                    setTimeout(function () {
                        window.location = "../SalesAndMarketing/SalesNote.aspx";
                    }, 2000);
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
            $("#ContentPlaceHolder1_hfOutId").val("0");
            $("#ItemForTransferTbl tbody").html("");
            $("#RequisitionWiseItemTbl tbody").html("");
            $("#SalesOrderItemTbl tbody").html("");
            $("#BillingOrderItemTbl tbody").html("");
            $("#ContentPlaceHolder1_ddlCostCenterFrom").val("0").prop('disabled', false).trigger('change');
            $("#ContentPlaceHolder1_ddlLocationFrom").val("0").prop('disabled', false);
            $("#ContentPlaceHolder1_ddlCostCenterTo").val("0").prop('disabled', false).trigger('change');
            $("#ContentPlaceHolder1_ddlLocationTo").val("0").prop('disabled', false);
            $("#ContentPlaceHolder1_ddlRequisition").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlQuotation").val("0").prop('disabled', false).trigger('change');
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").prop('disabled', false);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").prop('disabled', false);
            $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").prop('disabled', false);
            $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").prop('disabled', false);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val("0").trigger("change");
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val("0").trigger("change");
            //$("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").val("0").trigger("change");
            //$("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").val("0").trigger("change");
            $("#ContentPlaceHolder1_txtBillNo").val("");
            $("#ContentPlaceHolder1_hfBillId").val("0");
            $("#ContentPlaceHolder1_ddlCategory").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_txtCurrentStock").val("").prop("disabled", false);
            $("#ContentPlaceHolder1_txtCurrentStockBy").val("").prop("disabled", false);
            $("#ContentPlaceHolder1_txtTransferQuantity").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_ddlOutType").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlRequisition").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlQuotation").attr("disabled", false);

            queryOutId = "";
            AddedSerialCount = 0;
            NotTriggerChange = 0;
            ItemSelected = null;
            TransferItemAdded = new Array();
            TransferItemDeleted = new Array();
            NewAddedSerial = new Array();
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();

            if (IsCanSave) {
                $('#btnSave').show();
            } else {
                $('#btnSave').hide();
            }

            $("#btnSave").val("Save");
        }

        function SearchOutOrder(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#OutOrderGrid tbody tr").length;
            var issueNumber = "0", status = "", issueType = "", fromDate = null, toDate = null;

            issueNumber = $("#ContentPlaceHolder1_txtIssueNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            issueType = $("#ContentPlaceHolder1_ddlSearchProductOutFor").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            $("#GridPagingContainer ul").html("");
            $("#OutOrderGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchOutOrder(issueType, fromDate, toDate, issueNumber, status,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

            return false;
        }
        function OnSearchPurchaseOrderSucceed(result) {
            var tr = "";

            $.each(result.GridData, function (count, gridObject) {
                tr += "<tr>";
                tr += "<td style='width:10%;'>" + gridObject.IssueNumber + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.IssueType + "</td>";

                if (gridObject.OutDate != null)
                    tr += "<td style='width:10%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.OutDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:10%;'></td>";

                tr += "<td style='width:15%;'>" + gridObject.FromCostCenter + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.ToCostCenter + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.Status + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:15%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:15%;'></td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";


                if (gridObject.IsCanEdit && IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditItemOutWithConfirmation('" + gridObject.ProductOutFor + "'," + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete && IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return OutOrderDelete('" + gridObject.ProductOutFor + "'," + gridObject.OutId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return OutOrderApprovalWithConfirmation('" + gridObject.ProductOutFor + "','" + 'Checked' + "'," + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Checked'  title='Checked' border='0' />";

                }

                if (gridObject.IsCanApproved && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return OutOrderApprovalWithConfirmation('" + gridObject.ProductOutFor + "','" + 'Approved' + "', " + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Approved'  title='Approved' border='0' />";
                }

                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.ProductOutFor + "'," + gridObject.OutId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Invoice' title='Item Transfer Info' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.OutId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.RequisitionOrSalesId + "</td>";

                tr += "</tr>";

                $("#OutOrderGrid tbody").append(tr);
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
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");
        }

        function EditItemOut(issueType, outId, requisitionOrSalesId) {
            PageMethods.EditItemOut(issueType, outId, requisitionOrSalesId, OnEditPurchaseOrderSucceed, OnEditPurchaseOrderFailed);
            return false;
        }
        function EditItemOutWithConfirmation(issueType, outId, requisitionOrSalesId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            EditItemOut(issueType, outId, requisitionOrSalesId);
        }
        function OnEditPurchaseOrderSucceed(result) {

            $("#SerialItemTable tbody").html("");
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();
            NewAddedSerial = new Array();
            AddedSerialCount = 0;

            if (result.ProductOut.GLCompanyId > 0)
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(result.ProductOut.GLCompanyId).trigger("change");
            if (result.ProductOut.GLProjectId > 0)
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(result.ProductOut.GLProjectId).trigger("change");

            if (result.ProductOut.ToGLCompanyId > 0)
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").val(result.ProductOut.ToGLCompanyId).trigger("change");
            if (result.ProductOut.ToGLProjectId > 0)
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").val(result.ProductOut.ToGLProjectId).trigger("change");

            if (result.ProductOut.ProductOutFor == "StockTransfer") {
                $("#RequisitionWiseItemContainer").hide();
                $("#RequisitionNumberContainer").hide();
                $("#StockTransferContainer").show();
                $("#remarksDiv").show();
                $("#btnDiv").show();

                $("#ItemForTransferTbl tbody").html("");
                $("#RequisitionWiseItemTbl tbody").html("");
                $("#SalesOrderItemTbl tbody").html("");
                $("#BillingOrderItemTbl tbody").html("");

                StockTransferOrderEdit(result);
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").prop('disabled', false);
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").prop('disabled', false);

                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").prop('disabled', false);
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").prop('disabled', false);
            }
            else if (result.ProductOut.ProductOutFor == "Requisition") {

                $("#RequisitionWiseItemContainer").show();
                $("#RequisitionNumberContainer").show();
                $("#remarksDiv").show();
                $("#btnDiv").show();
                $("#StockTransferContainer").hide();

                $("#ItemForTransferTbl tbody").html("");
                $("#RequisitionWiseItemTbl tbody").html("");
                $("#SalesOrderItemTbl tbody").html("");
                $("#BillingOrderItemTbl tbody").html("");

                $("#ContentPlaceHolder1_ddlOutType").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlRequisition").attr("disabled", true);

                RequisitionWiseOutOrderEdit(result);
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").prop('disabled', false);
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").prop('disabled', false);

                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").prop('disabled', true);
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").prop('disabled', true);
                $("#ContentPlaceHolder1_ddlCostCenterTo").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlLocationTo").attr("disabled", true);
            }
            else if (result.ProductOut.ProductOutFor == "SalesOut") {
                $("#ContentPlaceHolder1_ddlOutType").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlQuotation").attr("disabled", true);
                $("#ItemForTransferTbl tbody").html("");
                $("#RequisitionWiseItemTbl tbody").html("");
                $("#SalesOrderItemTbl tbody").html("");
                $("#BillingOrderItemTbl tbody").html("");
                QuotationWiseOutOrderEdit(result)
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").prop('disabled', false);
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").prop('disabled', false);
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").prop('disabled', false);
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").prop('disabled', false);
            }
            else if (result.ProductOut.ProductOutFor == "Billing") {
                $("#ContentPlaceHolder1_ddlOutType").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlQuotation").attr("disabled", true);
                $("#ItemForTransferTbl tbody").html("");
                $("#RequisitionWiseItemTbl tbody").html("");
                $("#SalesOrderItemTbl tbody").html("");
                $("#BillingOrderItemTbl tbody").html("");
                BillingWiseOutOrderEdit(result)
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").prop('disabled', false);
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").prop('disabled', false);
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").prop('disabled', false);
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").prop('disabled', false);
            }

        }
        function OnEditPurchaseOrderFailed() { }

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

        function StockTransferOrderEdit(result) {

            LoadForEditOutOrder(result);

            var tr = "";

            $.each(result.ProductOutDetails, function (count, obj) {

                tr += "<tr>";

                tr += "<td style='width:24%;'>" + obj.ItemName + "</td>";
                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    tr += "<td style='width:7%;'>" + obj.ColorText + "</td>";
                    tr += "<td style='width:7%;'>" + obj.SizeText + "</td>";
                    tr += "<td style='width:15%;'>" + obj.StyleText + "</td>";
                }
                else {
                    tr += "<td style='display:none'>" + obj.ColorText + "</td>";
                    tr += "<td style='display:none'>" + obj.SizeText + "</td>";
                    tr += "<td style='display:none'>" + obj.StyleText + "</td>";
                }
                tr += "<td style='width:10%;'>" + obj.StockQuantity + "</td>";
                tr += "<td style='width:12%;'>" +
                    "<input type='text' value='" + obj.Quantity + "' id='pp" + obj.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                    "</td>";

                tr += "<td style='width:10%;'>" + obj.StockBy + "</td>";

                tr += "<td style='width:5%;'>";
                tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                if (obj.ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForAdHocItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.ItemId + "</td>";
                tr += "<td style='display:none;'>0</td>";
                tr += "<td style='display:none;'>" + obj.StockById + "</td>";
                tr += "<td style='display:none;'>" + obj.Quantity + "</td>";
                tr += "<td style='display:none;'>" + obj.OutDetailsId + "</td>";
                tr += "<td style='display:none;'>" + obj.ColorId + "</td>";
                tr += "<td style='display:none;'>" + obj.SizeId + "</td>";
                tr += "<td style='display:none;'>" + obj.StyleId + "</td>";

                tr += "</tr>";

                $("#ItemForTransferTbl tbody").append(tr);
                tr = "";
            });

            TransferItemAdded = result.ProductOutDetails;

            CommonHelper.ApplyDecimalValidation();
            ClearAfterTransferItemAdded();

            $("#myTabs").tabs({ active: 0 });
        }

        function RequisitionWiseOutOrderEdit(result) {

            LoadForEditOutOrder(result);

            var tr = "";

            $.each(result.ProductOutDetails, function (count, obj) {

                tr += "<tr>";

                if (obj.ApprovedQuantity > 0) {

                    if (parseInt(obj.OutDetailsId, 10) > 0) {
                        tr += "<td style='width:5%;'>" +
                            "<input type='checkbox' checked='checked' id='chk" + obj.ItemId + "'" + " />" +
                            "</td>";
                    }
                    else {
                        tr += "<td style='width:5%;'>" +
                            "<input type='checkbox' id='chk" + obj.ItemId + "'" + " />" +
                            "</td>";
                    }
                }
                else {
                    tr += "<td style='width:5%;'></td>";
                }

                tr += "<td style='width:18%;'>" + obj.ItemName + "</td>";
                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    tr += "<td style='width:10%;'>" + obj.ColorText + "</td>";
                    tr += "<td style='width:10%;'>" + obj.SizeText + "</td>";
                    tr += "<td style='width:10%;'>" + obj.StyleText + "</td>";
                }
                else {
                    tr += "<td style='display:none;'>" + obj.ColorText + "</td>";
                    tr += "<td style='display:none;'>" + obj.SizeText + "</td>";
                    tr += "<td style='display:none;'>" + obj.StyleText + "</td>";
                }
                tr += "<td style='width:12%;'>" + obj.ApprovedQuantity + "</td>";
                tr += "<td style='width:12%;'>" + obj.DeliveredQuantity + "</td>";
                tr += "<td style='width:12%;'>" + obj.RemainingTransferQuantity + "</td>";
                tr += "<td style='width:10%;'>" + obj.StockQuantity + "</td>";
                tr += "<td style='width:12%;'>" + obj.StockBy + "</td>";

                if (obj.Quantity > 0) {
                    tr += "<td style='width:10%;'>" +
                        "<input type='text' value='" + obj.Quantity + "' id='q' " + obj.ItemId + " class='form-control quantitydecimal' onblur='CheckTransferQuantity(this)' />" +
                        "</td>";
                }
                else {
                    tr += "<td style='width:10%;'>" +
                        "<input type='text' value='" + (obj.ApprovedQuantity - obj.DeliveredQuantity) + "' id='q' " + obj.ItemId + " class='form-control quantitydecimal' onblur='CheckTransferQuantity(this)' />" +
                        "</td>";
                }

                tr += "<td style='width:5%;'>";
                if (obj.ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForPurchaseWiseItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.ItemId + "</td>";
                tr += "<td style='display:none;'>" + obj.StockById + "</td>";
                tr += "<td style='display:none;'>" + obj.RequisitionDetailsId + "</td>";
                tr += "<td style='display:none;'>" + obj.OutDetailsId + "</td>";
                tr += "<td style='display:none;'>" + obj.ProductType + "</td>";

                if (obj.Quantity > 0) {
                    tr += "<td style='display:none;'>" + obj.Quantity + "</td>";
                }
                else {
                    tr += "<td style='display:none;'>" + obj.RemainingTransferQuantity + "</td>";
                }
                tr += "<td style='display:none;'>" + obj.ColorId + "</td>";
                tr += "<td style='display:none;'>" + obj.SizeId + "</td>";
                tr += "<td style='display:none;'>" + obj.StyleId + "</td>";
                tr += "</tr>";
                $("#RequisitionWiseItemTbl tbody").append(tr);
                tr = "";
            });

            TransferItemAdded = result.ProductOutDetails;

            CommonHelper.ApplyDecimalValidation();
            ClearAfterTransferItemAdded();

            $("#myTabs").tabs({ active: 0 });
        }

        function QuotationWiseOutOrderEdit(result) {

            LoadForEditOutOrder(result);

            var tr = "";

            $.each(result.ProductOutDetails, function (count, obj) {

                tr += "<tr>";

                if (obj.ApprovedQuantity > 0) {

                    if (parseInt(obj.OutDetailsId, 10) > 0) {
                        tr += "<td style='width:5%;'>" +
                            "<input type='checkbox' checked='checked' id='chk" + obj.ItemId + "'" + " />" +
                            "</td>";
                    }
                    else {
                        tr += "<td style='width:5%;'>" +
                            "<input type='checkbox' id='chk" + obj.ItemId + "'" + " />" +
                            "</td>";
                    }
                }
                else {
                    tr += "<td style='width:5%;'></td>";
                }

                tr += "<td style='width:18%;'>" + obj.ItemName + "</td>";
                tr += "<td style='width:12%;'>" + obj.ApprovedQuantity + "</td>";
                tr += "<td style='width:12%;'>" + obj.DeliveredQuantity + "</td>";
                tr += "<td style='width:12%;'>" + obj.RemainingTransferQuantity + "</td>";
                tr += "<td style='width:10%;'>" + obj.StockQuantity + "</td>";
                tr += "<td style='width:12%;'>" + obj.StockBy + "</td>";

                if (obj.Quantity > 0) {
                    tr += "<td style='width:10%;'>" +
                        "<input type='text' value='" + obj.Quantity + "' id='q' " + obj.ItemId + " class='form-control quantitydecimal' onchange='CheckBillingOrderWiseQuantity(this)' />" +
                        "</td>";
                }
                else {
                    tr += "<td style='width:10%;'>" +
                        "<input type='text' value='" + (obj.ApprovedQuantity - obj.DeliveredQuantity) + "' id='q' " + obj.ItemId + " class='form-control quantitydecimal' onchange='CheckBillingOrderWiseQuantity(this)' />" +
                        "</td>";
                }

                tr += "<td style='width:5%;'>";
                if (obj.ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForPurchaseWiseItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.ItemId + "</td>";
                tr += "<td style='display:none;'>" + obj.StockById + "</td>";
                tr += "<td style='display:none;'>" + obj.RequisitionDetailsId + "</td>";
                tr += "<td style='display:none;'>" + obj.OutDetailsId + "</td>";
                tr += "<td style='display:none;'>" + obj.ProductType + "</td>";

                if (obj.Quantity > 0) {
                    tr += "<td style='display:none;'>" + obj.Quantity + "</td>";
                }
                else {
                    tr += "<td style='display:none;'>" + obj.RemainingTransferQuantity + "</td>";
                }

                tr += "</tr>";
                $("#BillingOrderItemTbl tbody").append(tr);
                tr = "";
            });

            TransferItemAdded = result.ProductOutDetails;

            CommonHelper.ApplyDecimalValidation();
            ClearAfterTransferItemAdded();

            $("#myTabs").tabs({ active: 0 });
        }

        function BillingWiseOutOrderEdit(result) {

            LoadForEditOutOrder(result);

            var tr = "";

            $.each(result.ProductOutDetails, function (count, obj) {

                tr += "<tr>";

                if (obj.ApprovedQuantity > 0) {

                    if (parseInt(obj.OutDetailsId, 10) > 0) {
                        tr += "<td style='width:5%;'>" +
                            "<input type='checkbox' checked='checked' id='chk" + obj.ItemId + "'" + " />" +
                            "</td>";
                    }
                    else {
                        tr += "<td style='width:5%;'>" +
                            "<input type='checkbox' id='chk" + obj.ItemId + "'" + " />" +
                            "</td>";
                    }
                }
                else {
                    tr += "<td style='width:5%;'></td>";
                }

                tr += "<td style='width:18%;'>" + obj.ItemName + "</td>";
                tr += "<td style='width:12%;'>" + obj.ApprovedQuantity + "</td>";
                var alreayDelivered = (obj.ApprovedQuantity - obj.RemainingTransferQuantity).toFixed(2);
                tr += "<td style='width:10%;'>" + alreayDelivered + "</td>";

                tr += "<td style='width:12%;'>" + obj.RemainingTransferQuantity + "</td>";
                tr += "<td style='width:10%;'>" + obj.StockQuantity + "</td>";
                tr += "<td style='width:12%;'>" + obj.StockBy + "</td>";

                if (obj.Quantity > 0) {
                    tr += "<td style='width:10%;'>" +
                        "<input type='text' value='" + obj.Quantity + "' id='q' " + obj.ItemId + " class='form-control quantitydecimal' onchange='CheckBillingOrderWiseQuantity(this)'  />" +
                        "</td>";
                }
                else {
                    tr += "<td style='width:10%;'>" +
                        "<input type='text' value='" + (obj.ApprovedQuantity - obj.DeliveredQuantity) + "' id='q' " + obj.ItemId + " class='form-control quantitydecimal' onchange='CheckBillingOrderWiseQuantity(this)' />" +
                        "</td>";
                }

                tr += "<td style='width:5%;'>";
                if (obj.ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForPurchaseWiseItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.ItemId + "</td>";
                tr += "<td style='display:none;'>" + obj.StockById + "</td>";
                tr += "<td style='display:none;'>" + obj.RequisitionDetailsId + "</td>";
                tr += "<td style='display:none;'>" + obj.OutDetailsId + "</td>";
                tr += "<td style='display:none;'>" + obj.ProductType + "</td>";

                if (obj.Quantity > 0) {
                    tr += "<td style='display:none;'>" + obj.Quantity + "</td>";
                }
                else {
                    tr += "<td style='display:none;'>" + obj.RemainingTransferQuantity + "</td>";
                }

                tr += "</tr>";
                $("#BillingOrderItemTbl tbody").append(tr);
                tr = "";
            });

            TransferItemAdded = result.ProductOutDetails;

            CommonHelper.ApplyDecimalValidation();
            ClearAfterTransferItemAdded();

            $("#myTabs").tabs({ active: 0 });
        }

        function LoadForEditOutOrder(result) {

            AddedSerialzableProduct = result.ProductSerialInfo;

            if (result.ProductSerialInfo != null) {
                if (result.ProductSerialInfo.length > 0) {
                    $("#lblAddedQuantity").text(result.ProductSerialInfo.length);
                    AddedSerialCount = result.ProductSerialInfo.length;
                }
            }

            $("#ContentPlaceHolder1_hfOutId").val(result.ProductOut.OutId);
            $("#ContentPlaceHolder1_ddlOutType").val(result.ProductOut.ProductOutFor).trigger('change');

            $("#ContentPlaceHolder1_hfLocationFromId").val(result.ProductOut.FromLocationId);
            $("#ContentPlaceHolder1_hfLocationToId").val(result.ProductOut.ToLocationId);
            $("#ContentPlaceHolder1_ddlCostCenterFrom").val(result.ProductOut.FromCostCenterId + '').trigger('change');
            $("#ContentPlaceHolder1_ddlCostCenterTo").val(result.ProductOut.ToCostCenterId + '').trigger('change');

            LoadLocationFrom(result.ProductOut.FromCostCenterId);
            LoadLocationTo(result.ProductOut.ToCostCenterId);

            if (result.ProductOut.ProductOutFor == "Requisition")
                $("#ContentPlaceHolder1_ddlRequisition").val(result.ProductOut.RequisitionOrSalesId + '').trigger('change');
            else if (result.ProductOut.ProductOutFor == "SalesOut") {
                NotTriggerChange = 1;
                $("#ContentPlaceHolder1_ddlQuotation").val(result.ProductOut.RequisitionOrSalesId + '').trigger('change');

            }
            else if (result.ProductOut.ProductOutFor == "Billing") {
                NotTriggerChange = 1;
                $("#ContentPlaceHolder1_hfBillId").val(result.ProductOut.RequisitionOrSalesId);
                $("#ContentPlaceHolder1_txtBillNo").val(result.ProductOut.BillNo);

                //$("#ContentPlaceHolder1_ddlQuotation").val(result.ProductOut.RequisitionOrSalesId + '').trigger('change');

            }

            if (result.ProductOut.Remarks != null)
                $("#ContentPlaceHolder1_txtRemarks").val(result.ProductOut.Remarks);

            if (IsCanEdit) {
                $('#btnSave').show();
            } else {
                $('#btnSave').hide();
            }

            $("#btnSave").val("Update");
        }

        function OutOrderDelete(IssueType, OutId, ApprovedStatus, CreatedBy) {

            if (!confirm("Do you Want To Delete?")) {
                return false;
            }
            PageMethods.OutOrderDelete(IssueType, OutId, ApprovedStatus, CreatedBy, OnApprovalSucceed, OnApprovalFailed);
        }

        function OutOrderApproval(ProductOutFor, ApprovedStatus, OutId, RequisitionOrSalesId) {

            PageMethods.OutOrderApproval(ProductOutFor, OutId, ApprovedStatus, RequisitionOrSalesId, OnApprovalSucceed, OnApprovalFailed);
        }
        function OutOrderApprovalWithConfirmation(ProductOutFor, ApprovedStatus, OutId, RequisitionOrSalesId) {
            if (!confirm('Do you want to ' + (ApprovedStatus == 'Checked' ? 'check' : 'approve') + " ?")) {
                return false;
            }
            OutOrderApproval(ProductOutFor, ApprovedStatus, OutId, RequisitionOrSalesId);
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                LoadNotReceivedRequisitionOrder();
                SearchOutOrder($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }

        function LoadNotReceivedRequisitionOrder() {
            PageMethods.LoadNotReceivedRequisitionOrder(OnLoadPOSucceeded, OnLoadPOFailed);
        }
        function OnLoadPOSucceeded(result) {

            var control = $('#ContentPlaceHolder1_ddlRequisition');

            control.empty();
            if (result != null) {
                if (result.length > 0) {

                    control.empty().append('<option value="0">---Please Select---</option>');
                    for (i = 0; i < result.length; i++) {
                        control.append('<option title="' + result[i].PRNumber + '" value="' + result[i].RequisitionId + '">' + result[i].PRNumber + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }

            return false;
        }
        function OnLoadPOFailed() { }

        function ShowReport(IssueType, OutId, ApprovedStatus, CreatedBy) {
            var iframeid = 'printDoc';
            var url = "Reports/frmReportProductOut.aspx?poOutId=" + OutId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Item Transfer",
                show: 'slide'
            });
        }

        function CheckAllOrder() {
            if ($("#OrderCheck").is(":checked")) {
                $("#RequisitionWiseItemTbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#RequisitionWiseItemTbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }
        function CheckAllSalesOrder() {

            if ($("#chkAllSales").is(":checked")) {
                $("#SalesOrderItemTbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#SalesOrderItemTbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }
        function CheckAllBillingOrder() {

            if ($("#chkAllBills").is(":checked")) {
                $("#BillingOrderItemTbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#BillingOrderItemTbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchOutOrder(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function PerformClearActionWithComfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function ClearSerialWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            ClearSerial();
        }

        function OnLoadAttributeStyleSucceeded(result) {
            var list = result;
            var ddlStyleAttributeId = '<%=ddlStyleAttribute.ClientID%>';
            var control = $('#' + ddlStyleAttributeId);
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
        function OnLoadAttributeStyleFailed(error) {
        }
        function OnLoadAttributeSizeSucceeded(result) {
            var list = result;
            var ddlSizeAttributeId = '<%=ddlSizeAttribute.ClientID%>';
            var control = $('#' + ddlSizeAttributeId);
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
        function OnLoadAttributeSizeFailed(error) {
        }

        function OnLoadAttributeColorSucceeded(result) {
            var list = result;
            var ddlColorAttributeId = '<%=ddlColorAttribute.ClientID%>';
            var control = $('#' + ddlColorAttributeId);
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
        function OnLoadAttributeColorFailed(error) {
        }
        function ClearAfterAdhoqPurchaseItemAdded() {
            $("#ContentPlaceHolder1_ddlCategory").val('0').trigger('change');
            $("#ContentPlaceHolder1_txtItem").val('');
            $("#ContentPlaceHolder1_hfCategoryId").val('0');
            $("#ContentPlaceHolder1_hfItemId").val('0');
            $("#ContentPlaceHolder1_txtCurrentStock").val('');
            $("#ContentPlaceHolder1_txtCurrentStockBy").val('');
            $("#ContentPlaceHolder1_txtTransferQuantity").val('');


            $("#ContentPlaceHolder1_ddlColorAttribute").empty();
            $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
            $("#ContentPlaceHolder1_ddlStyleAttribute").empty();
            return false;
        }
        function GetInvItemStockInfoByItemAndAttributeId(companyId, projectId, itemId, colorId, sizeId, styleId, locationId) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/ItemTransfer.aspx/GetInvItemStockInfoByItemAndAttributeId',
                data: "{'companyId':'" + companyId + "','projectId':'" + projectId + "','itemId':'" + itemId + "','colorId':'" + colorId + "','sizeId':'" + sizeId + "','styleId':'" + styleId + "','locationId':'" + locationId + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != null) {
                        $("#ContentPlaceHolder1_txtCurrentStock").val(data.d.StockQuantity).attr("disabled", true);
                    }
                    else {
                        $("#ContentPlaceHolder1_txtCurrentStock").val("0").attr("disabled", true);
                    }
                },
                error: function (result) {
                    toastr.error("Please Contact With Admin");
                }
            });
        }
        function GetInvItemAttributeByItemIdAndAttributeType(itemId, type) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/ItemTransfer.aspx/GetInvItemAttributeByItemIdAndAttributeType',
                data: "{'ItemId':'" + itemId + "','attributeType':'" + type + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != null) {
                        if (type == 'Color')
                            OnLoadAttributeColorSucceeded(data.d);
                        if (type == 'Size')
                            OnLoadAttributeSizeSucceeded(data.d);
                        if (type == 'Style')
                            OnLoadAttributeStyleSucceeded(data.d);
                    }
                },
                error: function (result) {
                    toastr.error("Please Contact With Admin");
                }
            });
        }

    </script>
    <asp:HiddenField ID="hfOutId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemIdForSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationFromId" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationToId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsItemSerialFillWithAutoSearch" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfMainStoreId" runat="server" Value="0" />
    <asp:HiddenField ID="hfDefaultLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hfBillId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsItemAttributeEnable" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Item Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Item Transfer</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Item Transfer Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Transfer Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlOutType" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div id="RequisitionNumberContainer">
                                <div class="col-md-2">
                                    <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Requisition Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlRequisition" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field"
                                    Text="From Store"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCostCenterFrom" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                            <div id="dvCostCenterFrom">
                                <div class="col-md-2">
                                    <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="To Store"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlCostCenterTo" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label13" runat="server" class="control-label required-field" Text="From Location"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLocationFrom" runat="server" CssClass="form-control" TabIndex="5">
                                </asp:DropDownList>
                            </div>
                            <div id="dvCostCenterTo">
                                <div class="col-md-2">
                                    <asp:Label ID="Label14" runat="server" class="control-label required-field" Text="To Location"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlLocationTo" runat="server" CssClass="form-control" TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-6">
                                <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                            </div>
                            <div class="col-md-6">
                                <UserControl:CompanyProjectUserControl ID="companyProjectUserControlTwo" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div id="QuotationNumberContainer">
                                <div class="col-md-2">
                                    <asp:Label ID="Label9" runat="server" class="control-label required-field" Text="Quotation Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlQuotation" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="DivBillNo" style="display: none">
                            <div class="col-md-2">
                                <label class="control-label required-field">Bill No</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBillNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="RequisitionWiseItemContainer">
                            <div class="col-md-12" style="height: 300px; overflow-y: scroll;">
                                <table id="RequisitionWiseItemTbl" class="table table-bordered table-condensed table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width: 5%; text-align: center;">Select
                                            <input type="checkbox" value="" checked="checked" id="OrderCheck" onclick="CheckAllOrder()" />
                                            </th>
                                            <th style="width: 15%;">Item Name</th>
                                            <th style="width: 10%;" id="cId">Color</th>
                                            <th style="width: 10%;" id="sId">Size</th>
                                            <th style="width: 10%;" id="stId">Style</th>
                                            <th style="width: 7%;">Requisition Quantity</th>
                                            <th style="width: 7%;">Already Transferred</th>
                                            <th style="width: 7%;">Max Transfer Quantity</th>
                                            <th style="width: 7%;">Stock Quantity</th>
                                            <th style="width: 10%;">Unit</th>
                                            <th style="width: 7%;">Transfer Quantity</th>
                                            <th style="width: 5%;">Action</th>
                                            <th style="display: none;">Item Id</th>
                                            <th style="display: none;">Stock By Id</th>
                                            <th style="display: none;">RequisitionDetailsId</th>
                                            <th style="display: none;">OutDetailsId</th>
                                            <th style="display: none;">ProductType</th>
                                            <th style="display: none;">Old Quantity</th>
                                            <th style="display: none;">ColorId</th>
                                            <th style="display: none;">SizeId</th>
                                            <th style="display: none;">StyleId</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                        <div class="row" id="SalesOrderItemContainer">
                            <div class="col-md-12" style="height: 300px; overflow-y: scroll;">
                                <table id="SalesOrderItemTbl" class="table table-bordered table-condensed table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width: 5%; text-align: center;">Select
                                            <input type="checkbox" id="chkAllSales" checked="checked" onclick="CheckAllSalesOrder()" />
                                            </th>
                                            <th style="width: 20%;">Item Name</th>
                                            <th style="width: 10%">Quotation Qty</th>
                                            <th style="width: 10%;">Already Delivered</th>
                                            <th style="width: 10%;">Max Deliver Qty</th>
                                            <th style="width: 10%;">Stock Qty</th>
                                            <th style="width: 15%;">Unit</th>
                                            <th style="width: 15%">Deliver Qty</th>
                                            <th style="width: 5%">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                        <div class="row" id="BillingOrderItemContainer">
                            <div class="col-md-12" style="height: 300px; overflow-y: scroll;">
                                <table id="BillingOrderItemTbl" class="table table-bordered table-condensed table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width: 5%; text-align: center;">Select
                                            <input type="checkbox" id="chkAllBills" checked="checked" onclick="CheckAllBillingOrder()" />
                                            </th>
                                            <th style="width: 20%;">Item Name</th>
                                            <th style="width: 10%">Billing Qty</th>
                                            <th style="width: 10%;">Already Delivered</th>
                                            <th style="width: 10%;">Max Deliver Qty</th>
                                            <th style="width: 10%;">Stock Qty</th>
                                            <th style="width: 15%;">Unit</th>
                                            <th style="width: 15%">Deliver Qty</th>
                                            <th style="width: 5%">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                        <div id="StockTransferContainer">
                            <hr />
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Item"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtItem" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div id="AttributeDiv">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label10" runat="server" class="control-label" Text="Color"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlColorAttribute" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label11" runat="server" class="control-label" Text="Size"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSizeAttribute" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label15" runat="server" class="control-label" Text="Style"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlStyleAttribute" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label6" runat="server" class="control-label" Text="Current Stock"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCurrentStock" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label7" runat="server" class="control-label" Text="Unit"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCurrentStockBy" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label8" runat="server" class="control-label required-field" Text="Quantity"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTransferQuantity" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="padding-top: 10px;">
                                <div class="col-md-12">
                                    <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItemForTransfer()" />
                                    <input id="btnCancelOrder" type="button" value="Cancel" onclick="ClearAfterAdhoqPurchaseItemAdded()"
                                        class="TransactionalButton btn btn-primary btn-sm" />
                                </div>
                            </div>
                            <div style="height: 300px; overflow-y: scroll;">
                                <table id="ItemForTransferTbl" class="table table-bordered table-condensed table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width: 24%;">Item Name</th>
                                            <th style="width: 7%;" id="stcId">Color</th>
                                            <th style="width: 7%;" id="stsId">Size</th>
                                            <th style="width: 15%;" id="ststId">Style</th>
                                            <th style="width: 10%;">Stock Quantity</th>
                                            <th style="width: 12%;">Unit</th>
                                            <th style="width: 10%;">Unit Head</th>
                                            <th style="width: 5%;">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                        <hr />

                        <div id="remarksDiv" class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div id="btnDiv" class="row" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-large" value="Save" onclick="SaveItemOutOrder()" />
                                <input type="button" id="btnClear" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="PerformClearActionWithComfirmation()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Item Transfer Search
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label12" runat="server" class="control-label" Text="Transfer Type"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlSearchProductOutFor" runat="server" CssClass="form-control"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lbl1000" runat="server" class="control-label" Text="Issue Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtIssueNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
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
                    <table id="OutOrderGrid" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 10%;">Transfer Number
                                </th>
                                <th style="width: 10%;">Transfer Type
                                </th>
                                <th style="width: 10%;">Out Date
                                </th>
                                <th style="width: 15%;">Store From
                                </th>
                                <th style="width: 15%;">Store To
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
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="1000" height="800" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
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
                        <input type="button" class="btn btn-primary" style="padding-left: 30px; padding-right: 30px;" value="Ok" onclick="ApplySerialForPurchaseItem()" />
                        <input type="button" class="btn btn-primary" value="Cancel" onclick="CancelAddSerial()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
