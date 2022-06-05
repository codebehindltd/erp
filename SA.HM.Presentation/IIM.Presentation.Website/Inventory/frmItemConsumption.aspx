<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmItemConsumption.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmItemConsumption" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var queryOutId = "";
        var AddedSerialCount = 0;
        var AddedSerialzableProduct = new Array();
        var DeletedSerialzableProduct = new Array();
        var NewAddedSerial = new Array();
        var SelectedItem = new Array();
        var outItemEdited = "";
        var DeletedOutDetails = [];
        var RequisitionProductDetails = [];
        var cancelItem = [], approvedItem = [];
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false, IsSerialAutoField = false;
        var serialNumber = "";

        $(document).ready(function () {
            $("#ContentPlaceHolder1_companyProjectUserControl_hfDropdownFirstValue").val("select");

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Item Out</li>";
            var breadCrumbs = moduleName + formName;
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            IsSerialAutoField = $('#ContentPlaceHolder1_hfIsItemSerialFillWithAutoSearch').val() == '1' ? true : false;

            if (IsCanSave) {
                $('#ContentPlaceHolder1_btnSave').show();
            }
            else {
                $('#ContentPlaceHolder1_btnSave').hide();
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

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfIsInventoryIntegrateWithAccounts").val() != "") {
                if ($("#ContentPlaceHolder1_hfIsInventoryIntegrateWithAccounts").val() != "0") {
                    if ($("#ContentPlaceHolder1_hfIsInventoryIntegratationWithAccountsAutomated").val() == "0")
                        $("#accountHeadContainer").show();
                    else
                        $("#accountHeadContainer").hide();
                }
                else {
                    $("#accountHeadContainer").hide();
                }
            }

            $("#myTabs").tabs();

            CommonHelper.ApplyDecimalValidation();

            if (Cookies.getJSON('consumption') != undefined) {

                queryOutId = CommonHelper.GetParameterByName("oid");
                var issueType = CommonHelper.GetParameterByName("it");
                var outId = CommonHelper.GetParameterByName("oid");

                PageMethods.FillForm(outId, OnFillFormSucceed, OnFillFormFailed);
            }

            $("#ContentPlaceHolder1_ddlCostCenter").change(function () {
                LoadLocation($(this).val());

                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
                LoadProductByCategoryNCostcenterId(costCenterId, 0);
            });

            $("#ContentPlaceHolder1_ddlProduct").change(function () {
                var productId = $("#ContentPlaceHolder1_ddlProduct").val();
                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
                var locationId = $("#ContentPlaceHolder1_ddlLocation").val();

                //if (costCenterId == "0") {
                //    toastr.warning('Please provide Consumption From.');
                //    return;
                //}
                //else

                if (locationId == "0") {
                    toastr.warning("Please provide a location.");
                    return;
                }

                if (productId != null) {
                    PageMethods.LoadRelatedStockBy(productId, OnLoadStockBySucceeded, OnLoadStockByFailed);

                    if (locationId != null && productId != null)
                        LoadCurrentStockQuantity(costCenterId, locationId, productId);
                }
            });

            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlProduct").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlOutFor").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCostCenter").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlAccountExpenseHead").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCategory").change(function () {
                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
                var categoryId = $(this).val();
                LoadProductByCategoryNCostcenterId(costCenterId, categoryId);
            });

            $("#ContentPlaceHolder1_ddlIssueType").change(function () {

                var issueType = $(this).val();
                if (issueType == "Costcenter") {
                    $("#ContentPlaceHolder1_lblOutFor").text("Cost Center/ Store");
                }
                else {
                    $("#ContentPlaceHolder1_lblOutFor").text(issueType + " Name");
                }
                LoadIssueForDetails(issueType);
            });

            $("#btnAdd").click(function () {
                var quantityConsumption = $('#ContentPlaceHolder1_txtQuantity').val();
                var stockQuantity = $('#ContentPlaceHolder1_lblCurrentStock').text();
                var duplicateSerial = 0;

                var costCenterId = $('#ContentPlaceHolder1_ddlCostCenter').val();
                var stockById = $("#ContentPlaceHolder1_ddlStockBy").val();
                var stockByText = $("#ContentPlaceHolder1_ddlStockBy option:selected").text();
                var locationId = $("#ContentPlaceHolder1_ddlLocation").val();
                var itemId = $("#ContentPlaceHolder1_hfProductId").val();
                var toCostCenterId = $('#ContentPlaceHolder1_ddlOutFor').val();
                var unitHead = $("#ContentPlaceHolder1_lblCurrentStockBy").text();
                debugger;
                if (toCostCenterId == "0") {
                    toastr.warning('Please Consumption For (Employee/Store Name).');
                    return;
                }
                else if (costCenterId == "0") {
                    toastr.warning('Please provide Consumption From.');
                    return;
                }
                if (itemId == "0" || itemId == "" || itemId == null) {
                    toastr.warning("Please provide an item.");
                    return false;
                }
                else if (stockById == "0") {
                    toastr.warning("Please provide a stock by.");
                    return;
                }
                else if (locationId == "0") {
                    toastr.warning("Please provide a location.");
                    return;
                }
                else if (quantityConsumption == "" || quantityConsumption == "0") {
                    toastr.warning("Please provide Consumption Qty.");
                    return;
                }
                else if (stockQuantity == "0" || stockQuantity == "") {
                    toastr.warning("Consumption Quantity Cannot Greater Than Stock Quantity.");
                    return;
                }

                else if ((parseFloat(quantityConsumption) > parseFloat(stockQuantity)) && (unitHead == stockByText)) {
                    toastr.warning("Consumption Quantity Cannot Greater Than Stock Quantity.");
                    return;
                }

                var quantity = "0", averageCost = "0", outId = "0", outDetailsId = "0", duplicateItemId = 0;
                var productName = "", stockBy = "", costCenter = "", productType = "Non Serial Product";
                var categoryId = "0", location = '';
                var _item, _color, _size, _style;

                outId = $("#ContentPlaceHolder1_hfOutId").val();

                var itm = _.findWhere(SelectedItem, { ItemId: parseInt(itemId, 10) });
                if (itm != null) {
                    productType = itm.ProductType;
                }
                else {
                    toastr.warning("Please provide an item.");
                    return false;
                }

                productName = $("#ContentPlaceHolder1_txtItem").val();
                stockBy = $("#ContentPlaceHolder1_ddlStockBy option:selected").text();
                costCenter = $("#ContentPlaceHolder1_ddlCostCenter option:selected").text();
                categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                location = $("#ContentPlaceHolder1_ddlLocation option:selected").text();
                quantity = $("#ContentPlaceHolder1_txtQuantity").val();
                averageCost = $('#ContentPlaceHolder1_txtAverageCost').val();

                var colorddlLength = $('#ContentPlaceHolder1_ddlColorAttribute > option').length;
                var sizeddlLength = $('#ContentPlaceHolder1_ddlSizeAttribute > option').length;
                var styleddlLength = $('#ContentPlaceHolder1_ddlStyleAttribute > option').length;
                var colorId = 0;
                if (colorddlLength > 0) {
                    colorId = $("#ContentPlaceHolder1_ddlColorAttribute option:selected").val();
                }
                var color = $("#ContentPlaceHolder1_ddlColorAttribute option:selected").text();
                var sizeId = 0;
                if (sizeddlLength > 0) {
                    sizeId = $("#ContentPlaceHolder1_ddlSizeAttribute option:selected").val();
                }
                var size = $("#ContentPlaceHolder1_ddlSizeAttribute option:selected").text();
                var styleId = 0;
                if (styleddlLength > 0) {
                    styleId = $("#ContentPlaceHolder1_ddlStyleAttribute option:selected").val();
                }
                var style = $("#ContentPlaceHolder1_ddlStyleAttribute option:selected").text();

                //var itm = _.findWhere(TransferItemAdded, { ItemId: ItemSelected.ItemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId });

                CommonHelper.ExactMatch();
                if (outItemEdited != "") {
                    var editedItemId = $.trim($(outItemEdited).find("td:eq(10)").text());
                    var editedColorId = $.trim($(outItemEdited).find("td:eq(15)").text());
                    var editedSizeId = $.trim($(outItemEdited).find("td:eq(16)").text());
                    var editedStyleId = $.trim($(outItemEdited).find("td:eq(17)").text());

                    if (editedItemId != itemId || editedColorId != colorId || editedSizeId != sizeId || editedStyleId != styleId) {
                        var _break = 0;
                        $('#ProductOutGrid tbody tr').each(function (i, el) {
                            _item = $(el).children().eq(10).text();
                            _color = $(el).children().eq(15).text();
                            _size = $(el).children().eq(16).text();
                            _style = $(el).children().eq(17).text();
                            if (_item == itemId && _color == colorId && _size == sizeId && _style == styleId) {
                                _break = 1;
                                return false;
                            }
                        });
                        if (_break == 1) {
                            toastr.warning("Same item already added.");
                            return false;
                        }
                    }
                }
                else {
                    var _break = 0;
                    $('#ProductOutGrid tbody tr').each(function (i, el) {
                        _item = $(el).children().eq(10).text();
                        _color = $(el).children().eq(15).text();
                        _size = $(el).children().eq(16).text();
                        _style = $(el).children().eq(17).text();
                        if (_item == itemId && _color == colorId && _size == sizeId && _style == styleId) {
                            _break = 1;
                            return false;
                        }
                    });
                    if (_break == 1) {
                        toastr.warning("Same item already added.");
                        return false;
                    }
                }
                $("#ContentPlaceHolder1_ddlIssueType").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlLocation").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlOutFor").attr("disabled", true);

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").attr("disabled", true);
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").attr("disabled", true);

                CommonHelper.SpinnerOpen();

                if (outId != "0" && outItemEdited != "") {

                    outDetailsId = $(outItemEdited).find("td:eq(8)").text();
                    EditItem(productName, quantity, stockBy, outDetailsId, categoryId, itemId, stockById, productType, averageCost, color, size, style, colorId, sizeId, styleId);
                    //toastr.info("Edit db");
                    return;
                }
                else if (outId == "0" && outItemEdited != "") {

                    outDetailsId = $(outItemEdited).find("td:eq(7)").text();
                    EditItem(productName, quantity, stockBy, outDetailsId, categoryId, itemId, stockById, productType, averageCost, color, size, style, colorId, sizeId, styleId);
                    //toastr.info("Edit");
                    return;
                }

                AddItemForOut(productName, quantity, stockBy, outDetailsId, categoryId, itemId, stockById, productType, averageCost, color, size, style, colorId, sizeId, styleId);

                $("#ContentPlaceHolder1_hfItemId").val("0");
                $("#ContentPlaceHolder1_txtQuantity").val("");
                $("#ContentPlaceHolder1_ddlStockBy").val("0");
                $("#ContentPlaceHolder1_ddlProduct").val("0").trigger('change');
                $("#ContentPlaceHolder1_lblCurrentStock").text("");
                $("#ContentPlaceHolder1_lblCurrentStockBy").text("");
                $("#ContentPlaceHolder1_txtItem").val("");
                $("#ContentPlaceHolder1_txtAverageCost").text("");
                $("#ContentPlaceHolder1_hfProductId").val("");
                $("#ContentPlaceHolder1_ddlColorAttribute").empty();
                $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
                $("#ContentPlaceHolder1_ddlStyleAttribute").empty();

                return false;
            });

            var txtFromDate = '<%=txtFromDate.ClientID%>';
            var txtToDate = '<%=txtToDate.ClientID%>';

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

            $("#txtSerialAutoComplete").autocomplete({
                minLength: 1,
                source: function (request, response) {
                    debugger;
                    var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                    var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
                    var itemId = $("#ContentPlaceHolder1_hfItemIdForSerial").val();
                    var locationId = $("#ContentPlaceHolder1_ddlLocation").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmItemConsumption.aspx/SerialSearch',
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

            $("#ContentPlaceHolder1_txtItem").autocomplete({

                source: function (request, response) {
                    var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                    var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
                    var costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    var locationId = $("#ContentPlaceHolder1_ddlLocation").val();

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
                        url: '../Inventory/frmItemConsumption.aspx/ItemSearch',
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
                    $("#ContentPlaceHolder1_hfProductId").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfCategoryId").val(ui.item.CategoryId);
                    $("#ContentPlaceHolder1_hfStockById").val(ui.item.StockBy);
                    //$("#ContentPlaceHolder1_txtCurrentStock").val(ui.item.StockQuantity).prop("disabled", true);
                    $("#ContentPlaceHolder1_txtCurrentStockBy").val(ui.item.UnitHead).prop("disabled", true);
                    PageMethods.LoadRelatedStockBy(ui.item.value, OnLoadStockBySucceeded, OnLoadStockByFailed);
                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Color');
                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Size');
                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Style');

                    var companyId = 0;
                    companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                    var projectId = 0;
                    projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

                    var locationId = parseInt($('#ContentPlaceHolder1_ddlLocation').val(), 10);
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
                StockOnChange();
                return false;
            });
            $('#ContentPlaceHolder1_ddlSizeAttribute').change(function () {
                StockOnChange();
                return false;
            });
            $('#ContentPlaceHolder1_ddlStyleAttribute').change(function () {
                StockOnChange();
                return false;
            });
            $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany').change(function () {
                StockOnChange();
                return false;
            });
            $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject').change(function () {
                StockOnChange();
                return false;
            });

            $("#btnApprove").click(function () {

                var outId = "0", outDetailsId = "0";
                var quantity = "", approvedQuantity = "";

                cancelItem = []; approvedItem = [];

                outId = $("#ContentPlaceHolder1_hfOutId").val();

                $("#DetailsRequisitionGrid tbody tr").each(function (index, item) {

                    if ($(item).find("td:eq(0)").find("input").is(':checkbox')) {

                        quantity = $.trim($(item).find("td:eq(2)").text());
                        approvedQuantity = $.trim($(item).find("td:eq(4)").find("input").val());
                        outDetailsId = $.trim($(item).find("td:eq(5)").text());

                        if ($(item).find("td:eq(0)").find("input").is(":checked") && !$(item).find("td:eq(0)").find("input").is(":disabled")) {

                            if (approvedQuantity == "" || approvedQuantity == "0") {
                                approvedQuantity = quantity;
                            }

                            approvedItem.push({
                                outDetailsId: parseInt(outDetailsId, 10),
                                outId: parseInt(outId, 10),
                                Quantity: parseFloat(quantity),
                                ApprovedQuantity: parseFloat(approvedQuantity),

                            });
                        }
                        else if (!$(item).find("td:eq(0)").find("input").is(":disabled")) {
                            cancelItem.push({
                                outDetailsId: parseInt(outDetailsId, 10),
                                outId: parseInt(outId, 10),
                                ApprovedQuantity: parseFloat(0),
                                ApprovalStatus: "Cancel"
                            });
                        }
                    }
                });

                var IsProductOutApprovalEnable = $("#ContentPlaceHolder1_hfIsProductOutApprovalEnable").val();

                PageMethods.CheckOrApproveRequsition(IsProductOutApprovalEnable, outId, approvedItem, cancelItem, OnApprovedRequsitionSucceed, OnApprovedRequsitionFailed);

                return false;
            });


            $("#ContentPlaceHolder1_ddlOutForLocation").hide();

            var single = '<%=isSingle%>';
            if (single == "True") {
                $('#CompanyProjectPanel').hide();
            }
            else {
                $('#CompanyProjectPanel').show();
            }
            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                $("#AttributeDiv").show();
                $("#cId").show();
                $("#sId").show();
                $("#stId").show();
            }
            else {
                $("#AttributeDiv").hide();
                $("#cId").hide();
                $("#sId").hide();
                $("#stId").hide();

            }
        });

        function OnApprovedRequsitionSucceed(result) {
            if (result.IsSuccess) {
                $("#detailsConsumptionDialog").dialog("close");
                CommonHelper.AlertMessage(result.AlertMessage);

                $("#ContentPlaceHolder1_hfOutId").val("0");
                pendingItem = [];
                approvedItem = [];
                //PerformClearAction();
                setTimeout(clickSearch, 1000);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

        }
        function StockOnChange() {
            var companyId = 0;
            companyId = parseInt($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(), 10);
            var projectId = 0;
            projectId = parseInt($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(), 10);

            var locationId = parseInt($('#ContentPlaceHolder1_ddlLocation').val(), 10);
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
            if (companyId > 0 && projectId > 0 && locationId > 0 && sizeId > 0 && styleId > 0 && colorId > 0)
                GetInvItemStockInfoByItemAndAttributeId(companyId, projectId, parseInt($("#ContentPlaceHolder1_hfProductId").val(), 10), colorId, sizeId, styleId, locationId);
            return false;
        }
        function clickSearch() {
            $("#ContentPlaceHolder1_btnSearch").trigger("click");
        }
        function OnApprovedRequsitionFailed(error) {
            CommonHelper.AlertMessage(result.AlertMessage);
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
            return false;
        }
        function OnLoadStockByFailed(error) {
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

            fromLocationId = $("#ContentPlaceHolder1_ddlLocation").val();

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

        function LoadLocation(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationSucceeded, OnLoadLocationFailed);
        }
        function OnLoadLocationSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlLocation');

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
            if (list.length == 1 && $("#ContentPlaceHolder1_hfLocationId").val() == '0')
                control.val(control.find('option:first').val());
            else
                control.val($("#ContentPlaceHolder1_hfLocationId").val());
            return false;
        }
        function OnLoadLocationFailed() { }

        function LoadProductByCategoryNCostcenterId(costCenterId, categoryId) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmItemConsumption.aspx/LoadProductByCategoryNCostcenterId',
                data: "{'costCenterId':'" + costCenterId + "','categoryId':'" + categoryId + "'}",
                dataType: "json",
                success: function (data) {

                    SelectedItem = data.d;
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

        function LoadIssueForDetails(issueFor) {

            if (issueFor == "Others") {
                $('#ContentPlaceHolder1_ddlOutFor').empty();
                $('#ContentPlaceHolder1_ddlOutFor').empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');

                $('#ContentPlaceHolder1_ddlOutForLocation').empty();
                $('#ContentPlaceHolder1_ddlOutForLocation').empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');

                return false;
            }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmItemConsumption.aspx/IssueForDetails',
                data: "{'issueFor':'" + issueFor + "'}",
                dataType: "json",
                success: function (data) {

                    var list = data.d;
                    var control = $('#ContentPlaceHolder1_ddlOutFor');

                    control.empty();
                    if (list != null) {
                        if (list.length > 0) {

                            control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                            for (i = 0; i < list.length; i++) {
                                control.append('<option title="' + list[i].IssueName + '" value="' + list[i].Id + '">' + list[i].IssueName + '</option>');
                                $('#ContentPlaceHolder1_ddlOutForLocation').append('<option title="' + list[i].IssueName + '" value="' + list[i].Id + '">' + list[i].DefaultStockLocationId + '</option>');
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                            $('#ContentPlaceHolder1_ddlOutForLocation').empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                        }
                    }

                    control.val($("#ContentPlaceHolder1_hfIssueForId").val()).trigger("change");
                },
                error: function (result) {
                }
            });

        }

        function AddItemForOut(productName, quantity, stockBy, outDetailsId, categoryId, itemId, stockById, productType, averageCost, color, size, style, colorId, sizeId, styleId) {

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
            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                tr += "<td style='width:10%;'>" + color + "</td>";
                tr += "<td style='width:10%;'>" + size + "</td>";
                tr += "<td style='width:20%;'>" + style + "</td>";
            }
            else {
                tr += "<td style='display:none'>" + color + "</td>";
                tr += "<td style='display:none'>" + size + "</td>";
                tr += "<td style='display:none'>" + style + "</td>";
            }
            tr += "<td style='width:10%;'>" + quantity + "</td>";
            tr += "<td style='width:10%;'>" + stockBy + "</td>";
            tr += "<td style='width:10%;'>" + averageCost + "</td>";

            tr += "<td style='width:10%;'> <span onclick=\"javascript:return FIllForEdit(this);\" title='Edit'><img src='../Images/edit.png' alt='Edit'></span>";
            tr += "&nbsp;&nbsp;<span onclick= 'javascript:return DeleteItemReceivedDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></span>";

            if (productType == 'Serial Product') {
                tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForAdHocItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
            }
            tr += "</td>";

            tr += "<td style='display:none'>" + outDetailsId + "</td>";
            tr += "<td style='display:none'>" + categoryId + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + stockById + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + productType + "</td>";
            tr += "<td style='display:none'>" + colorId + "</td>";
            tr += "<td style='display:none'>" + sizeId + "</td>";
            tr += "<td style='display:none'>" + styleId + "</td>";

            tr += "</tr>";

            $("#ProductOutGrid tbody").append(tr);

            CommonHelper.SpinnerClose();
        }

        function FIllForEdit(editItem) {
            CommonHelper.SpinnerOpen();
            outItemEdited = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAdd").val("Update");

            var categoryId = $.trim($(tr).find("td:eq(9)").text());
            var itemId = $.trim($(tr).find("td:eq(10)").text());
            var ColorId = $.trim($(tr).find("td:eq(15)").text());
            var SizeId = $.trim($(tr).find("td:eq(16)").text());
            var StyleId = $.trim($(tr).find("td:eq(17)").text());
            var Color = $.trim($(tr).find("td:eq(1)").text());
            var Size = $.trim($(tr).find("td:eq(2)").text());
            var Style = $.trim($(tr).find("td:eq(3)").text());
            var itemName = $.trim($(tr).find("td:eq(0)").text());
            var stockById = $(tr).find("td:eq(11)").text();
            var itemName = $(tr).find("td:eq(0)").text();
            var quantity = $(tr).find("td:eq(4)").text();
            var averageCost = $(tr).find("td:eq(6)").text();

            $("#ContentPlaceHolder1_hfItemId").val(itemId);
            $("#ContentPlaceHolder1_ddlCategory").val(categoryId);
            $("#ContentPlaceHolder1_ddlStockBy").val(stockById);
            $("#ContentPlaceHolder1_txtQuantity").val(quantity);

            if ($("#ContentPlaceHolder1_hfIsAverageCostEnableInItemConsumption").val() == "0") {
                $("#ContentPlaceHolder1_txtAverageCost").val(averageCost).attr("disabled", true);
            }
            else {
                $("#ContentPlaceHolder1_txtAverageCost").val(averageCost);
            }

            $('#ContentPlaceHolder1_ddlProduct').val(itemId).trigger('change');
            $('#ContentPlaceHolder1_hfProductId').val(itemId);
            PageMethods.LoadRelatedStockBy(itemId, OnLoadStockBySucceeded, OnLoadStockByFailed);
            GetInvItemAttributeByItemIdAndAttributeType(itemId, 'Color');
            GetInvItemAttributeByItemIdAndAttributeType(itemId, 'Size');
            GetInvItemAttributeByItemIdAndAttributeType(itemId, 'Style');

            $('#ContentPlaceHolder1_ddlColorAttribute').val(ColorId);
            $('#ContentPlaceHolder1_ddlSizeAttribute').val(SizeId);
            $('#ContentPlaceHolder1_ddlStyleAttribute').val(StyleId);

            var companyId = 0;
            companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var projectId = 0;
            projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            var locationId = parseInt($('#ContentPlaceHolder1_ddlLocation').val(), 10);
            GetInvItemStockInfoByItemAndAttributeId(companyId, projectId, itemId, ColorId, SizeId, StyleId, locationId);

            $('#ContentPlaceHolder1_txtItem').val(itemName);
            CommonHelper.SpinnerClose();
        }

        function EditItem(productName, quantity, stockBy, outDetailsId, categoryId, itemId, stockById, productType, averageCost, color, size, style, colorId, sizeId, styleId) {

            var editedItemId = $.trim($(outItemEdited).find("td:eq(10)").text());
            var editedColorId = $.trim($(outItemEdited).find("td:eq(15)").text());
            var editedSizeId = $.trim($(outItemEdited).find("td:eq(16)").text());
            var editedStyleId = $.trim($(outItemEdited).find("td:eq(17)").text());
            var _break = 0;
            $('#ProductOutGrid tbody tr').each(function (i, el) {
                var _item = $(el).children().eq(10).text();
                var _color = $(el).children().eq(15).text();
                var _size = $(el).children().eq(16).text();
                var _style = $(el).children().eq(17).text();
                if (editedItemId == _item && editedColorId == _color && editedSizeId == _size && editedStyleId == _style) {
                    $(el).children().eq(0).text(productName);
                    $(el).children().eq(4).text(quantity);
                    $(el).children().eq(6).text(averageCost);
                    $(el).children().eq(1).text(color);
                    $(el).children().eq(2).text(size);
                    $(el).children().eq(3).text(style);
                    $(el).children().eq(10).text(itemId);
                    $(el).children().eq(15).text(colorId);
                    $(el).children().eq(16).text(sizeId);
                    $(el).children().eq(17).text(styleId);
                    _break = 1;
                    return false;
                }
            });

            var outId = $("#ContentPlaceHolder1_hfOutId").val();

            if (outDetailsId != "0")
                $(outItemEdited).find("td:eq(12)").text("1");

            $(outItemEdited).find("td:eq(13)").text(itemId);

            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_AverageCost").val("");
            $("#ContentPlaceHolder1_lblCurrentStock").text("");
            $("#ContentPlaceHolder1_ddlProduct").val("0");
            $("#ContentPlaceHolder1_txtQuantity").val("");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_hfLocationId").val("0");
            $("#ContentPlaceHolder1_ddlCategory").val("0");
            $("#ContentPlaceHolder1_hfStockById").val("0");

            $('#ContentPlaceHolder1_ddlColorAttribute').empty();
            $('#ContentPlaceHolder1_ddlSizeAttribute').empty();
            $('#ContentPlaceHolder1_ddlStyleAttribute').empty();

            $("#btnAdd").val("Add");

            outItemEdited = "";

            CommonHelper.SpinnerClose();
        }

        function FillForm(outId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            CommonHelper.SpinnerOpen();
            PageMethods.FillForm(outId, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {

            if (result != null) {

                PerformClearAction();

                if (result.ProductOut.IssueType != "Others") {
                    $("#ContentPlaceHolder1_hfIssueForId").val(result.ProductOut.ToCostCenterId);
                    LoadIssueForDetails(result.ProductOut.IssueType);
                }

                $("#SerialItemTable tbody").html("");
                AddedSerialzableProduct = new Array();
                DeletedSerialzableProduct = new Array();
                NewAddedSerial = new Array();
                AddedSerialCount = 0;

                $("#ContentPlaceHolder1_hfLocationId").val(result.ProductOut.FromLocationId);
                $("#ContentPlaceHolder1_ddlIssueType").val(result.ProductOut.IssueType);
                $("#ContentPlaceHolder1_ddlIssueType").trigger('change');
                $("#ContentPlaceHolder1_hfOutId").val(result.ProductOut.OutId);
                $("#ContentPlaceHolder1_txtRemarks").val(result.ProductOut.Remarks);
                $('#ContentPlaceHolder1_ddlAccountExpenseHead').val(result.ProductOut.AccountPostingHeadId + '').trigger('change');

                $("#ContentPlaceHolder1_ddlOutFor").val(result.ProductOut.ToCostCenterId);
                if (result.ProductOut.GLCompanyId > 0)
                    $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(result.ProductOut.GLCompanyId).trigger("change");
                if (result.ProductOut.GLProjectId > 0)
                    $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(result.ProductOut.GLProjectId).trigger("change");

                $("#ContentPlaceHolder1_ddlCostCenter").val(result.ProductOut.FromCostCenterId);
                $("#ContentPlaceHolder1_ddlCostCenter").trigger('change');
                $("#ContentPlaceHolder1_ddlIssueType").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlLocation").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlOutFor").attr("disabled", true);

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").attr("disabled", true);
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").attr("disabled", true);

                $("#ProductOutGrid tbody").html("");

                AddedSerialzableProduct = result.ProductSerialInfo;

                if (result.ProductSerialInfo != null) {
                    if (result.ProductSerialInfo.length > 0) {
                        $("#lblAddedQuantity").text(result.ProductSerialInfo.length);
                        AddedSerialCount = result.ProductSerialInfo.length;
                    }
                }

                var rowLength = result.ProductOutDetails.length;
                var row = 0;

                for (row = 0; row < rowLength; row++) {
                    AddItemForOut(
                        result.ProductOutDetails[row].ItemName,
                        result.ProductOutDetails[row].Quantity,
                        result.ProductOutDetails[row].StockBy,
                        result.ProductOutDetails[row].OutDetailsId,
                        result.ProductOutDetails[row].CategoryId,
                        result.ProductOutDetails[row].ProductId,
                        result.ProductOutDetails[row].StockById,
                        result.ProductOutDetails[row].ProductType,
                        result.ProductOutDetails[row].AverageCost,
                        result.ProductOutDetails[row].ColorText,
                        result.ProductOutDetails[row].SizeText,
                        result.ProductOutDetails[row].StyleText,
                        result.ProductOutDetails[row].ColorId,
                        result.ProductOutDetails[row].SizeId,
                        result.ProductOutDetails[row].StyleId
                    );
                }
                if (IsCanEdit) {
                    $('#ContentPlaceHolder1_btnSave').show();
                }
                else {
                    $('#ContentPlaceHolder1_btnSave').hide();
                }

                $("#ContentPlaceHolder1_btnSave").val("Update");
                $("#myTabs").tabs({ active: 0 });
            }
            CommonHelper.SpinnerClose();
        }
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        function DeleteItemReceivedDetails(deletedItem) {

            if (!confirm("Do you want to delete?"))
                return;

            var tr = $(deletedItem).parent().parent();

            var outId = $("#ContentPlaceHolder1_hfOutId").val();
            var itemId = parseInt($(tr).find("td:eq(6)").text(), 10);

            if (outId != "0") {
                DeletedOutDetails.push({
                    OutDetailsId: $(tr).find("td:eq(4)").text(),
                    OutId: outId
                });
            }

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

        function ValidationBeforeSave() {

            var rowCount = $('#ProductOutGrid tbody tr').length;
            if (rowCount == 0) {
                toastr.warning('Add at least one Product.');
                return false;
            }
            if ($("#ContentPlaceHolder1_hfIsInventoryIntegrateWithAccounts").val() != "") {
                if ($("#ContentPlaceHolder1_hfIsInventoryIntegrateWithAccounts").val() != "0" && $("#ContentPlaceHolder1_hfIsInventoryIntegratationWithAccountsAutomated").val() == "0") {
                    if ($("#ContentPlaceHolder1_ddlAccountExpenseHead").val() == 0) {
                        toastr.warning('Add Account Head.');
                        return false;
                    }
                }
            }

            var productOutFor = "DirectOut", itemId = "0", costCenterIdFrom = "0", costCenterIdTo = "0", stockById = "0", employeeId = "0";
            var serialNumber = "", quantity = "0", averageCost = "0", requisitionOrSalesId = "0", locationIdFrom = "0", locationIdTo = "0";
            var isEdit = "0", outId = "0", outDetailsId = "0", remarks = "", outFor = "0", issueType = null, issueForId = null,
                accountPostingHeadId = "0", productType = "", itemName = "", GLCompanyId = "0", GLProjectId = "0", colorId = 0, sizeId = 0, styleId = 0;

            issueType = $("#ContentPlaceHolder1_ddlIssueType").val();
            costCenterIdTo = $("#ContentPlaceHolder1_ddlOutFor").val();
            var index = $("#ContentPlaceHolder1_ddlOutFor").val();

            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            outId = $("#ContentPlaceHolder1_hfOutId").val();
            accountPostingHeadId = $("#ContentPlaceHolder1_ddlAccountExpenseHead").val();
            costCenterIdFrom = $("#ContentPlaceHolder1_ddlCostCenter").val();
            locationIdFrom = $("#ContentPlaceHolder1_ddlLocation").val();
            GLCompanyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            GLProjectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            if (GLCompanyId == null)
                GLCompanyId = "0";
            if (GLProjectId == null)
                GLProjectId = "0";

            if (issueType != "Employee") {
                $("#ContentPlaceHolder1_ddlOutForLocation").val(costCenterIdTo);
                locationIdTo = $.trim($("#ContentPlaceHolder1_ddlOutForLocation option:selected").text());
            }
            else {
                employeeId = $("#ContentPlaceHolder1_ddlOutFor").val();
            }

            if (locationIdFrom == "") {
                toastr.warning("Please Select Location.");
                return false;
            }
            if (GLCompanyId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                toastr.warning("Please Select Company.");
                return false;
            }
            if (GLProjectId == "0") {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                toastr.warning("Please Select Project.");
                return false;
            }
            var ProductOut = {
                OutId: outId,
                ProductOutFor: productOutFor,
                RequisitionOrSalesId: 0,
                OutFor: 0,
                IssueType: issueType,
                FromCostCenterId: costCenterIdFrom,
                FromLocationId: locationIdFrom,
                ToCostCenterId: costCenterIdTo,
                ToLocationId: locationIdTo,
                AccountPostingHeadId: accountPostingHeadId,
                Remarks: remarks,
                EmployeeId: employeeId,
                GLCompanyId,
                GLProjectId
            };

            var AddedOutDetails = [], EditedOutDetails = [];

            $("#ProductOutGrid tbody tr").each(function (index, item) {

                outDetailsId = $.trim($(item).find("td:eq(8)").text());
                isEdit = $.trim($(item).find("td:eq(12)").text());

                itemId = parseInt($.trim($(item).find("td:eq(10)").text()), 10);
                stockById = parseInt($(item).find("td:eq(11)").text(), 10);
                quantity = parseFloat($(item).find("td:eq(4)").text());
                averageCost = parseFloat($(item).find("td:eq(6)").text());
                productType = $(item).find("td:eq(14)").text();
                itemName = $(item).find("td:eq(0)").text();

                if (outDetailsId == "0") {

                    AddedOutDetails.push({
                        OutDetailsId: outDetailsId,
                        OutId: outId,
                        StockById: stockById,
                        ItemId: itemId,
                        Quantity: quantity,
                        AverageCost: averageCost,
                        CostCenterIdFrom: costCenterIdFrom,
                        LocationIdFrom: locationIdFrom,
                        CostCenterIdTo: costCenterIdTo,
                        LocationIdTo: locationIdTo,
                        ProductType: productType,
                        ItemName: itemName,
                        ColorId: parseFloat($(item).find("td:eq(15)").text()),
                        SizeId: parseFloat($(item).find("td:eq(16)").text()),
                        StyleId: parseFloat($(item).find("td:eq(17)").text())
                    });
                }
                else if (outDetailsId != "0" && isEdit != "0") {
                    EditedOutDetails.push({
                        OutDetailsId: outDetailsId,
                        OutId: outId,
                        StockById: stockById,
                        ItemId: itemId,
                        Quantity: quantity,
                        AverageCost: averageCost,
                        CostCenterIdFrom: costCenterIdFrom,
                        LocationIdFrom: locationIdFrom,
                        CostCenterIdTo: costCenterIdTo,
                        LocationIdTo: locationIdTo,
                        ProductType: productType,
                        ItemName: itemName,
                        ColorId: parseFloat($(item).find("td:eq(15)").text()),
                        SizeId: parseFloat($(item).find("td:eq(16)").text()),
                        StyleId: parseFloat($(item).find("td:eq(17)").text())
                    });
                }
            });

            var row = 0;
            rowCount = AddedOutDetails.length;

            for (row = 0; row < rowCount; row++) {
                if (AddedOutDetails[row].ProductType == "Serial Product") {
                    var serialTotal = _.where(AddedSerialzableProduct, { ItemId: AddedOutDetails[row].ItemId });

                    if (AddedOutDetails[row].Quantity > serialTotal.length) {
                        toastr.warning("Please Give Serial Of Product " + AddedOutDetails[row].ItemName);
                        break;
                    }
                    else if (serialTotal.length > AddedOutDetails[row].Quantity) {
                        toastr.warning("Please Remove Serial Of Product " + AddedOutDetails[row].ItemName);
                        break;
                    }
                }
            }

            if (row != rowCount) {
                return false;
            }

            row = 0;
            rowCount = EditedOutDetails.length;

            for (row = 0; row < rowCount; row++) {
                if (EditedOutDetails[row].ProductType == "Serial Product") {
                    serialTotal = _.where(AddedSerialzableProduct, { ItemId: EditedOutDetails[row].ItemId });

                    if (EditedOutDetails[row].Quantity > serialTotal.length) {
                        toastr.warning("Please Give Serial Of Product " + EditedOutDetails[row].ItemName);
                        break;
                    }
                    else if (serialTotal.length > EditedOutDetails[row].Quantity) {
                        toastr.warning("Please Remove Serial Of Product " + EditedOutDetails[row].ItemName);
                        break;
                    }
                }
            }

            if (row != rowCount) {
                return false;
            }

            CommonHelper.SpinnerOpen();

            PageMethods.SaveProductOut(ProductOut, AddedOutDetails, EditedOutDetails, DeletedOutDetails, AddedSerialzableProduct,
                DeletedSerialzableProduct, OnSaveProductOutSucceeded, OnSaveProductOutFailed);

            return false;
        }
        function OnSaveProductOutSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();

                if (queryOutId != "") {
                    window.location = "/Inventory/ItemConsumptionInformation.aspx";
                }
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSaveProductOutFailed(error) { toastr.error(error.get_message()); CommonHelper.SpinnerClose(); }

        function ProductConsumptionReport(outId, rType) {

            var iframeid = 'printDoc';
            var url = "Reports/frmReportProductConsumption.aspx?poOutId=" + outId + '&rType=' + rType;
            document.getElementById(iframeid).src = url;

            if (rType == "cn") {
                $("#displayBill").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 900,
                    height: 600,
                    closeOnEscape: false,
                    resizable: false,
                    fluid: true,
                    title: "Delivery Challan",
                    show: 'slide'
                });
            }
            else {
                $("#displayBill").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 900,
                    height: 600,
                    closeOnEscape: false,
                    resizable: false,
                    fluid: true,
                    title: "Item Consumption",
                    show: 'slide'
                });
            }
        }

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

                tr += "<td style='width:25%;'>" + result[row].ItemName + "</td>";
                tr += "<td style='width:10%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:15%;'>" + result[row].StockBy + "</td>";
                tr += "<td style='width:10%;'>" + result[row].SerialNumber + "</td>";
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

        function OutOrderApproval(ProductOutFor, ApprovedStatus, OutId, RequisitionOrSalesId) {
            if (!confirm("Do you want to " + (ApprovedStatus == 'Checked' ? 'Check' : 'Aprrove') + " ?")) {
                return false;
            }
            $("#ContentPlaceHolder1_hfOutId").val(OutId);
            CommonHelper.SpinnerOpen();
            PageMethods.OutOrderApproval(ProductOutFor, OutId, ApprovedStatus, RequisitionOrSalesId, OnProductOutDetailsForApproveLoadSucceeded, OnProductOutDetailsForApproveLoadFailed);
            return false;
        }
        function OnProductOutDetailsForApproveLoadSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                LoadConsumptionList($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
        }
        function OnProductOutDetailsForApproveLoadFailed() {
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

        function PerformClearActionOnButtonClick() {
            if (!confirm("Do you want To clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function PerformClearAction() {


            $("#ContentPlaceHolder1_hfOutId").val("0");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_ddlOutFor").val("0").trigger("change");

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val("0").trigger("change");
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val("0").trigger("change");

            $("#ContentPlaceHolder1_ddlCategory").val("0");
            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_ddlProduct").val("0");

            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_hfStockById").val("0");

            $('#ContentPlaceHolder1_ddlCostCenter').val("0").trigger('change');
            $("#ContentPlaceHolder1_hfLocationId").val("0");
            $("#ContentPlaceHolder1_ddlLocation").val("0");
            $("#ContentPlaceHolder1_hfIssueForId").val("0");
            $("#ContentPlaceHolder1_lblCurrentStock").text("0");
            $("#ContentPlaceHolder1_lblCurrentStockBy").text("");
            $("#ContentPlaceHolder1_txtAverageCost").val("");

            $('#ContentPlaceHolder1_ddlAccountExpenseHead').val('0').trigger('change');
            $("#ContentPlaceHolder1_ddlProduct").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlOutForLocation").val("0");

            $("#ProductOutGrid tbody").html("");

            outItemEdited = "";
            DeletedOutDetails = [];
            RequisitionProductDetails = [];

            $("#ContentPlaceHolder1_btnSave").val("Save");

            $("#ContentPlaceHolder1_ddlSearchCostCenter").val("0");
            $("#ContentPlaceHolder1_ddlSearchLocation").val("0");

            $("#ContentPlaceHolder1_ddlIssueType").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlLocation").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlOutFor").attr("disabled", false);

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").attr("disabled", false);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").attr("disabled", false);
            //ContentPlaceHolder1_ddlCostCenter
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

        function LoadCurrentStockQuantity(costcenterId, locationId, itemId) {
            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            PageMethods.LoadCurrentStockQuantity(companyId, projectId, costcenterId, locationId, itemId, OnLoadCurrentStockSucceeded, OnLoadCurrentStockFailed);
        }
        function OnLoadCurrentStockSucceeded(result) {
            if (result != null) {
                $("#ContentPlaceHolder1_lblCurrentStock").text(result.StockQuantity);
                $("#ContentPlaceHolder1_lblCurrentStockBy").text(result.HeadName);

                if ($("#ContentPlaceHolder1_hfIsAverageCostEnableInItemConsumption").val() == "0") {
                    $("#ContentPlaceHolder1_txtAverageCost").text(result.AverageCost).attr("disabled", true);
                }
                else {
                    $("#ContentPlaceHolder1_txtAverageCost").text(result.AverageCost);
                }
            }
        }
        function OnLoadCurrentStockFailed(error) {
        }

        function ConfirmDeleteOrCancel(outId) {
            var itemStatus = "";
            var status = confirm("Want to cancel consumption???");
            if (status) {
                $("#tbConsumptionList tbody tr").each(function () {
                    itemStatus = $.trim($(this).find("td:eq(5)").text());
                });

                PageMethods.CancelConsumption(outId, OnsuccessCancel, OnfailCancel);
            }
            return false;
        }
        function OnsuccessCancel(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#ContentPlaceHolder1_hfRequisitionId").val("0");
                LoadConsumptionList($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnfailCancel(error) {
            toastr.error(error.get_message());
        }

        function GridPagingForSearchProductOut(pageNumber, isCurrentOrPreviousPage) {
            //window.location = "frmItemConsumption.aspx?pn=" + pageNumber + "&grc=" + ($("#ContentPlaceHolder1_gvProductOutInfo tbody tr").length + 1) + "&icp=" + isCurrentOrPreviousPage;

        }

        function LoadConsumptionList(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#tbConsumptionList tbody tr").length;
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            var status = $("#ContentPlaceHolder1_ddlStatus").val();
            var issueType = $("#ContentPlaceHolder1_ddlSearchIssueType").val();
            var issueNumber = "";

            if (fromDate == "") {
                $("#ContentPlaceHolder1_txtFromDate").val(GetStringFromDateTime(new Date));
                fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(GetStringFromDateTime(new Date()), innBoarDateFormat);
            }
            else {
                fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(fromDate, innBoarDateFormat);
            }

            if (toDate == "") {
                $("#ContentPlaceHolder1_txtToDate").val(GetStringFromDateTime(new Date));
                toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(GetStringFromDateTime(new Date()), innBoarDateFormat);
            }
            else {
                toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(toDate, innBoarDateFormat);
            }

            PageMethods.GetConsumptionList(fromDate, toDate, status, issueType, issueNumber, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnsuccessLoading, OnfailLoading);
            return false;
        }

        function OnsuccessLoading(result) {

            $("#tbConsumptionList tbody tr").remove();
            $("#GridPagingContainer ul").html("");
            var tr = "", totalRow = 0, editLink = "", deleteLink = "", invoiceLink = "", approvalLink = "", infoLink = "";
            var editPermission = true, deletePermission = true, approvalPermission = true;

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
                $("#tbConsumptionList tbody ").append(emptyTr);
                return false;
            }


            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tbConsumptionList tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:15%;\">" + gridObject.IssueNumber + "</td>";
                tr += "<td align='left' style=\"width:15%;\">" + GetStringFromDateTime(gridObject.OutDate) + "</td>";
                tr += "<td align='left' style=\"width:15%\">" + gridObject.IssueType + "</td>";
                tr += "<td align='left' style=\"width:15%\">" + gridObject.FromCostCenter + "</td>";
                tr += "<td align='left' style=\"width:15%\">" + gridObject.ToCostCenter + "</td>";
                tr += "<td align='left' style=\"width:10%\">" + gridObject.Status + "</td>";

                if (gridObject.IsCanEdit && IsCanEdit)
                    editLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return FillForm('" + gridObject.OutId + "');\"> <img alt=\"Edit\" title='Edit' src=\"../Images/edit.png\" /> </a>";

                if (gridObject.IsCanDelete && IsCanDelete)
                    deleteLink = "&nbsp;&nbsp;<a href=\"javascript:void();\"  onclick=\"javascript:return ConfirmDeleteOrCancel('" + gridObject.OutId + "');\"><img alt=\"Cancel\" title='Cancel' src=\"../Images/delete.png\" /></a>";

                if (gridObject.IsCanChecked && IsCanSave) {
                    approvalLink += "&nbsp;&nbsp;<img src='../Images/checked.png'  onClick= \"javascript:return OutOrderApproval('" + gridObject.ProductOutFor + "','" + 'Checked' + "'," + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Check'  title='Check' border='0' />";
                }

                if (gridObject.IsCanApproved && IsCanSave) {
                    approvalLink += "&nbsp;&nbsp;<img src='../Images/approved.png' AlternateText=\"Approve\" onClick= \"javascript:return OutOrderApproval('" + gridObject.ProductOutFor + "','" + 'Approved' + "', " + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Approve'  title='Approve' border='0' />";
                }

                invoiceLink = "&nbsp;&nbsp;<a href=\"javascript:void();\"  onclick=\"javascript:return ProductOutDetails('" + gridObject.OutId + "');\"><img alt=\"Invoice\" src=\"../Images/detailsInfo.png\" title='Details'/></a>";

                infoLink = "&nbsp;&nbsp;<a href=\"javascript:void();\"  onclick=\"javascript:return ProductConsumptionReport('" + gridObject.OutId + "', 'cm');\"><img alt=\"Consumption\" src=\"../Images/ReportDocument.png\" title='Consumption Info'/></a>";

                if ($('#ContentPlaceHolder1_hfIsItemConsumptionDeliveryChallanEnable').val() == "1") {
                    infoLink = infoLink + "&nbsp;&nbsp;<a href=\"javascript:void();\"  onclick=\"javascript:return ProductConsumptionReport('" + gridObject.OutId + "', 'cn');\"><img alt=\"Delivery Challan\" src=\"../Images/ReportDocument.png\" title='Delivery Challan'/></a>";
                }

                tr += "<td align='center' style=\"width:15%;\">" + editLink + deleteLink + approvalLink + invoiceLink + infoLink + "</td>";


                tr += "</tr>";

                $("#tbConsumptionList tbody").append(tr);
                tr = "";
                editLink = "";
                deleteLink = "";
                approvalLink = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
        }

        function OnfailLoading() {

        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadConsumptionList(pageNumber, IsCurrentOrPreviousPage);
        }

        function AddSerialForAdHocItem(control) {
            var tr = $(control).parent().parent();

            var itemName = $(tr).find("td:eq(0)").text();
            var itemId = $(tr).find("td:eq(10)").text();
            var quantity = $(tr).find("td:eq(1)").text();

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

        function ClearSearch() {
            //$("#ContentPlaceHolder1_txtIssueNumber").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("");
            $("#ContentPlaceHolder1_ddlSearchIssueType").val("");
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");

            return false;
        }

        function PopulateProjects(control) {
            let companyId = $(control).val();
            if (companyId == 0) {
                PopulateControlWithValueNTextField([], $("#ContentPlaceHolder1_ddlGLProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
            }
            $("#ContentPlaceHolder1_ddlGLProject").empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "../Inventory/frmItemConsumption.aspx/GetGLProjectByGLCompanyId",
                data: JSON.stringify({ companyId: companyId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    PopulateControlWithValueNTextField(response.d, $("#ContentPlaceHolder1_ddlGLProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
            }
            function GetInvItemAttributeByItemIdAndAttributeType(itemId, type) {
                return $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../Inventory/frmItemConsumption.aspx/GetInvItemAttributeByItemIdAndAttributeType',
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
        function GetInvItemStockInfoByItemAndAttributeId(companyId, projectId, itemId, colorId, sizeId, styleId, locationId) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmItemConsumption.aspx/GetInvItemStockInfoByItemAndAttributeId',
                data: "{'companyId':'" + companyId + "','projectId':'" + projectId + "','itemId':'" + itemId + "','colorId':'" + colorId + "','sizeId':'" + sizeId + "','styleId':'" + styleId + "','locationId':'" + locationId + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != null) {
                        $("#ContentPlaceHolder1_lblCurrentStock").text(data.d.StockQuantity);//.attr("disabled", true);
                        if ($("#btnAdd").val() != "Update") {
                            if ($("#ContentPlaceHolder1_hfIsAverageCostEnableInItemConsumption").val() == "0") {
                                $("#ContentPlaceHolder1_txtAverageCost").val(data.d.AverageCost).attr("disabled", true);
                            }
                            else {
                                $("#ContentPlaceHolder1_txtAverageCost").val(data.d.AverageCost);
                            }
                        }
                    }
                    else {
                        $("#ContentPlaceHolder1_lblCurrentStock").text("0");//.attr("disabled", true);
                        $("#ContentPlaceHolder1_txtAverageCost").text("0");
                    }
                },
                error: function (result) {
                    toastr.error("Please Contact With Admin");
                }
            });
        }
    </script>
    <div id="detailsConsumptionDialog" style="display: none;">
        <div id="detailsConsumptionGridContainer">
        </div>
        <div class="HMContainerRowButton" style="padding-bottom: 0; padding-top: 10px;">
            <input id="btnApprove" type="button" class="TransactionalButton btn btn-primary btn-sm"
                value="Approve Requisition" />
        </div>
    </div>
    <div id="DetailsOutGridContaiiner" style="display: none;">
        <table id="DetailsOutGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 20%;">Item Name
                    </th>
                    <th style="width: 10%;">Quantity
                    </th>
                    <th style="width: 15%;">Stock By
                    </th>
                    <th style="width: 10%;">Serial Number
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIssueForId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsSerializableItem" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsValidSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfOutId" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSearchLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsInventoryIntegrateWithAccounts" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsInventoryIntegratationWithAccountsAutomated" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsProductOutApprovalEnable" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemIdForSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsItemSerialFillWithAutoSearch" runat="server" Value="0" />
    <asp:HiddenField ID="hfProductId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCategoryId" runat="server" Value="0" />
    <asp:HiddenField ID="txtCurrentStockBy" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsItemAttributeEnable" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsAverageCostEnableInItemConsumption" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsItemConsumptionDeliveryChallanEnable" runat="server" Value="0" />
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

    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Item Consumption</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Consumption</a></li>
        </ul>
        <div id="tab-1">

            <div class="panel panel-default">
                <div class="panel-heading">
                    Consumption Information
                </div>
                <div class="panel-body">

                    <div class="panel panel-default">
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Consumtion Type"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlIssueType" runat="server" CssClass="form-control">
                                            <%--<asp:ListItem Text="Others" Value="Others"></asp:ListItem>--%>
                                            <asp:ListItem Text="For Employee" Value="Employee"></asp:ListItem>
                                            <asp:ListItem Text="For Cost Center" Value="Costcenter"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblOutFor" runat="server" class="control-label required-field" Text="Employee"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlOutFor" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="---Please Select---" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlOutForLocation" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCostCenterId" runat="server" class="control-label required-field"
                                    Text="Consumtion From"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label13" runat="server" class="control-label required-field" Text="Store Location"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">
                    Consumption Item Details
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" style="display: none">
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
                                <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Item"></asp:Label>
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
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Current Stock"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="lblCurrentStock" runat="server" class="form-control" Text="0"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label9" runat="server" class="control-label" Text="Average Cost"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAverageCost" runat="server" CssClass="form-control quantitydecimal" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2" style="display: none;">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Unit"></asp:Label>
                            </div>
                            <div class="col-md-4" style="display: none;">
                                <asp:Label ID="lblCurrentStockBy" runat="server" class="form-control" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblQuantity" runat="server" class="control-label required-field" Text="Consumption Qty"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control quantitydecimal" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Unit"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlStockBy" runat="server" CssClass="form-control">
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
                                <div id="ProductOutGridContainer">
                                    <table id="ProductOutGrid" class="table table-bordered table-condensed table-responsive"
                                        style="width: 100%;">
                                        <thead>
                                            <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                                <th style="width: 30%;">Item Name
                                                </th>
                                                <th style="width: 10%;" id="cId">Color
                                                </th>
                                                <th style="width: 10%;" id="sId">Size
                                                </th>
                                                <th style="width: 20%;" id="stId">Style
                                                </th>
                                                <th style="width: 10%;">Quantity
                                                </th>
                                                <th style="width: 10%;">Unit Measure
                                                </th>
                                                <th style="width: 10%;">Average Cost
                                                </th>
                                                <th style="width: 10%;">Action
                                                </th>
                                                <th style="display: none">OutDetailsId
                                                </th>
                                                <th style="display: none">CategoryId
                                                </th>
                                                <th style="display: none">ItemId
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
                        <div class="form-group" id="accountHeadContainer">
                            <div class="col-md-2">
                                <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Account Head"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlAccountExpenseHead" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="---Please Select---" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
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
                                    OnClientClick="javascript: return PerformClearActionOnButtonClick();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="InfoPanel" class="panel panel-default">
                <div class="panel-heading">
                    Consumption Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
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
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label CssClass="control-label" runat="server" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList CssClass="form-control" ID="ddlStatus" runat="server">
                                    <asp:ListItem Text="---All---" Value=""></asp:ListItem>
                                    <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                    <asp:ListItem Value="Checked">Checked</asp:ListItem>
                                    <asp:ListItem Value="Approved">Approved</asp:ListItem>
                                    <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Consumtion Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchIssueType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="---All---" Value=""></asp:ListItem>
                                    <asp:ListItem Text="For Employee" Value="Employee"></asp:ListItem>
                                    <asp:ListItem Text="For Cost Center" Value="Costcenter"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="return LoadConsumptionList(1,1);"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return ClearSearch();" TabIndex="6" />
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
                    <table class="table table-bordered table-condensed table-responsive" id='tbConsumptionList'>
                        <colgroup>
                            <col style="width: 15%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 15%;" />
                            <col style="width: 15%;" />
                            <col style="width: 10%;" />
                            <col style="width: 15%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Consumption Number
                                </td>
                                <td>Out Date
                                </td>
                                <td>Consumption Type
                                </td>
                                <th>From
                                </th>
                                <th>For
                                </th>
                                <td>Status
                                </td>
                                <td style="text-align: center;">Action
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                <div class="childDivSection">
                    <div class="text-center" id="GridPagingContainer">
                        <ul class="pagination">
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=btnPadding%>';
        if (x > -1) {
            $("#btnAdd").animate({ marginTop: '10px' });
        }
    </script>
</asp:Content>
