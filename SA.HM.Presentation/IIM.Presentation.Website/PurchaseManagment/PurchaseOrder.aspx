<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnBoard.Master" AutoEventWireup="true" CodeBehind="PurchaseOrder.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.PurchaseOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var vv = null;
        var triggerOrNot = 0;
        var CurrencyList = new Array();
        var RequsitionOrderList = new Array();
        var RequsitionOrderItem = new Array();
        var ItemSelected = null;
        var PurchaseOrderItem = new Array();
        var EditedTermsNConditions = new Array();
        var deletedTermsNConditions = new Array();
        var PurchaseOrderItemFromRequisition = new Array();
        var RequisitionWiseDeletedItem = new Array();
        var PurchaseOrderItemDeleted = new Array();
        var queryPurchaseOrderId = "";
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Purchase Order</li>";
            var breadCrumbs = moduleName + formName;

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            CommonHelper.ApplyIntigerValidation();
            if ($("#InnboardMessageHiddenField").val() !== "") {
                var msg = JSON.parse($("#InnboardMessageHiddenField").val());
                CommonHelper.AlertMessage(msg);
                $("#InnboardMessageHiddenField").val("");
            }
            if (IsCanSave) {
                $('#btnSave').show();
            } else {
                $('#btnSave').hide();
            }

            queryPurchaseOrderId = CommonHelper.GetParameterByName("poid");
            if (queryPurchaseOrderId != "") {
                PageMethods.EditPurchaseOrder(queryPurchaseOrderId, OnEditPurchaseOrderSucceed, OnEditPurchaseOrderFailed);
            }

            CommonHelper.ApplyDecimalValidation();

            if ($("#ContentPlaceHolder1_hfCurrencyObj").val() !== "") {
                CurrencyList = JSON.parse($("#ContentPlaceHolder1_hfCurrencyObj").val());
            }

            if ($("#ContentPlaceHolder1_hfRequsitionOrderObj").val() !== "") {
                RequsitionOrderList = JSON.parse($("#ContentPlaceHolder1_hfRequsitionOrderObj").val());
            }

            if ($("#ContentPlaceHolder1_ddlPurchaseOrderType").val() == "AdHoc") {
                $("#AdhoqPurchase").show();
                $("#AdhoqPurchaseItem").show();
                $("#RequisitionItemContainer").hide();
                $("#RequisitionWisePurchaseContainer").hide();
                $("#RequisitionCostCenterContainer").hide();
                $("#RequisitionOrderContainer").hide();
            }
            else {
                $("#AdhoqPurchase").hide();
                $("#AdhoqPurchaseItem").hide();
                $("#RequisitionItemContainer").show();
                $("#RequisitionWisePurchaseContainer").show();
                $("#RequisitionCostCenterContainer").show();
                $("#RequisitionOrderContainer").show();
            }

            $("#myTabs").tabs();

            $('#ContentPlaceHolder1_txtExpectedReceiveDate').datepicker({
                changeMonth: true,
                changeYear: true,
                minDate: 0,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            }); //.datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }); //.datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlGLCompanyId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSrcGLCompanyId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSupplier").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSearchSupplier").select2({
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

            $("#ContentPlaceHolder1_ddlRequisitionOrder").select2({
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

            $("#ContentPlaceHolder1_ddlSearchSupplier").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCurrency").change(function () {
                var currencyId = $(this).val();
                PageMethods.LoadCurrencyConversionRate(currencyId, OnLoadConversionRateSucceeded, OnLoadConversionRateFailed);
            });

            $("#ContentPlaceHolder1_ddlGLCompanyId").change(function () {
                var companyId = $(this).val();
                PageMethods.LoadAccountsCompanyInformation(companyId, OnLoadAccountsCompanyInformationSucceeded, OnLoadAccountsCompanyInformationFailed);
            });


            $("#ContentPlaceHolder1_ddlSupplier").change(function () {

                if ($("#ContentPlaceHolder1_ddlPurchaseOrderType").val() == "Requisition") {
                    RequisitionOrAdhocWiseShowHideAndLoadItem();
                }
            });

            $("#ContentPlaceHolder1_ddlPurchaseOrderType").change(function () {
                if ($("#ItemForPurchase tbody tr").length > 0 || $("#RequisitionItemForPurchase tbody tr").length > 0) {
                    if (!confirm("Data will be lost. Do you want to change Purchase Type to " + $("#ContentPlaceHolder1_ddlPurchaseOrderType option:selected").text() + "?"))
                        return false;
                }

                var purchaseOrderType = $(this).val();

                $("#ItemForPurchase tbody").html("");
                $("#RequisitionItemForPurchase tbody").html("");
                ClearAfterAdhoqPurchaseItemAdded();
                triggerOrNot = 1;
                $("#ContentPlaceHolder1_ddlRequisitionOrder").val("0").trigger('change');
                $("#ContentPlaceHolder1_ddlRequisitionCostcenter").val("0").trigger('change');
                $("#RequisitionItem tbody").html("");
                PurchaseOrderItem = new Array();
                RequsitionOrderItem = new Array();
                PurchaseOrderItemFromRequisition = new Array();

                if (purchaseOrderType == "AdHoc") {
                    $("#AdhoqPurchase").show();
                    $("#AdhoqPurchaseItem").show();
                    $("#RequisitionItemContainer").hide();
                    $("#RequisitionWisePurchaseContainer").hide();
                    $("#RequisitionCostCenterContainer").hide();
                    $("#RequisitionOrderContainer").hide();
                    $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
                    $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
                    $("#ContentPlaceHolder1_ddlGLCompanyId").attr("disabled", false);
                }
                else {
                    $("#AdhoqPurchase").hide();
                    $("#AdhoqPurchaseItem").hide();
                    $("#RequisitionItemContainer").show();
                    $("#RequisitionWisePurchaseContainer").show();
                    $("#RequisitionCostCenterContainer").show();
                    $("#RequisitionOrderContainer").show();
                }
            });

            $("#ContentPlaceHolder1_ddlRequisitionOrder").change(function () {
                if ($("#ContentPlaceHolder1_ddlPurchaseOrderType").val() == "Requisition") {
                    RequisitionOrAdhocWiseShowHideAndLoadItem();
                }
            });

            $("#ContentPlaceHolder1_txtItem").autocomplete({
                source: function (request, response) {
                    var companyId = $("#ContentPlaceHolder1_ddlGLCompanyId").val();
                    var costCenterId = $("#ContentPlaceHolder1_ddlCostCentre").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    var supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();

                    if (companyId == "0") {
                        toastr.warning("Please Select Company.");
                        $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                        return false;
                    }
                    else if (costCenterId == "0") {
                        toastr.warning("Please Select Store Form");
                        $("#ContentPlaceHolder1_ddlCategory").focus();
                        return false;
                    }
                    else if (supplierId == "0") {
                        toastr.warning("Please Select Supplier");
                        $("#ContentPlaceHolder1_ddlSupplier").focus();
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../PurchaseManagment/PurchaseOrder.aspx/ItemSearch',
                        data: JSON.stringify({ searchTerm: request.term, companyId: companyId, costCenterId: costCenterId, categoryId: categoryId, supplierId: supplierId }),
                        dataType: "json",
                        async: false,
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
                                    PurchasePrice: m.PurchasePrice,
                                    LastPurchaseDate: m.LastPurchaseDate,
                                    ManufacturerName: m.ManufacturerName,
                                    Model: m.Model,
                                    Description: m.Description,
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

                    ItemSelected = ui.item;

                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfItemId").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfCategoryId").val(ui.item.CategoryId);
                    $("#ContentPlaceHolder1_hfStockById").val(ui.item.StockBy);
                    $("#ContentPlaceHolder1_lblCurrentStockBy").text(ui.item.UnitHead);
                    var discriptionConfig = $("#ContentPlaceHolder1_hfIsItemDescriptionSuggestInPurchaseOrder").val();
                    var brand = ui.item.ManufacturerName == '' ? '' : "Brand: " + ui.item.ManufacturerName + '';
                    var model = ui.item.Model == '' ? '' : ", Model: " + ui.item.Model + '';
                    var description = ui.item.Description == '' ? '' : ", Description: " + ui.item.Description;

                    if (discriptionConfig == "1") {
                        $("#ContentPlaceHolder1_txtItemWiseRemarks").val(brand + model + description);
                    }

                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Color');
                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Size');
                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Style');
                    GetInvItemStockInfoByItemAndAttributeId();

                    $("#AttributeDiv").hide();
                    if (ui.item.IsAttributeItem) {
                        $("#AttributeDiv").show();
                    }
                }
            });

            $('#ContentPlaceHolder1_ddlSizeAttribute').change(function () {

                GetInvItemStockInfoByItemAndAttributeId();
            });

            $('#ContentPlaceHolder1_ddlStyleAttribute').change(function () {

                GetInvItemStockInfoByItemAndAttributeId();
            });


            $('#ContentPlaceHolder1_ddlColorAttribute').change(function () {
                GetInvItemStockInfoByItemAndAttributeId();
            });

            $("#ContentPlaceHolder1_ddlOrderType").change(function myfunction() {
                var type = $("#ContentPlaceHolder1_ddlOrderType").val();
                if (type != "0") {
                    LoadSupplierInfoByOrderType(type);
                }
                else {
                    $("#ContentPlaceHolder1_ddlSupplier").empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            });

            LoadTearmsNConditions();
            $("#TblTNC tbody tr").find("td:eq(0)").change(function () {
                var a = $(this).find('input');
            });
            $("[id=chkAll]").on("change", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#TblTNC tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#TblTNC tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
                }
            });

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                $("#AttributeDiv").show();
                $("#cId").show();
                $("#sId").show();
                $("#stId").show();
                $("#cIdr").show();
                $("#sIdr").show();
                $("#stIdr").show();
                $("#cIdrp").show();
                $("#sIdrp").show();
                $("#stIdrp").show();
                $("#cIdrit").show();
                $("#sIdrit").show();
                $("#stIdrit").show();
                document.getElementById("tableFootLabel").colSpan = "9";


            }
            else {
                $("#AttributeDiv").hide();
                $("#cId").hide();
                $("#sId").hide();
                $("#stId").hide();
                $("#cIdr").hide();
                $("#sIdr").hide();
                $("#stIdr").hide();
                $("#cIdrp").hide();
                $("#sIdrp").hide();
                $("#stIdrp").hide();
                $("#cIdrit").hide();
                $("#sIdrit").hide();
                $("#stIdrit").hide();
                document.getElementById("tableFootLabel").colSpan = "6";
            }

            $("#ContentPlaceHolder1_ddlCategory").change(function myfunction() {
                ClearAfterAdhoqPurchaseItemAddedByCategoryChange();
            });

        });

        function GetInvItemStockInfoByItemAndAttributeId() {
            var locationId = parseInt(0, 10);
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

            var companyId = parseInt($("#ContentPlaceHolder1_ddlGLCompanyId").val(), 10);
            var itemId = parseInt($("#ContentPlaceHolder1_hfItemId").val(), 10);

            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/PurchaseOrder.aspx/GetInvItemStockInfoByItemAndAttributeIdForPurchase',
                data: "{'itemId':'" + itemId + "','colorId':'" + colorId + "','sizeId':'" + sizeId + "','styleId':'" + styleId + "','locationId':'" + locationId + "','companyId':'" + companyId + "'}",
                dataType: "json",
                async: false,
                success: function (data) {

                    //$("#ContentPlaceHolder1_ddlCurrentStock option").remove();

                    if (data.d != null) {
                        //$("#ContentPlaceHolder1_txtCurrentStock").val(data.d.StockQuantity).attr("disabled", true);

                        var str = data.d.StockQuantity;
                        $("#ContentPlaceHolder1_lblCurrentStock").text(str);

                    }
                    else {
                        //$("#ContentPlaceHolder1_txtCurrentStock").val("0").attr("disabled", true);


                        var str = 0;
                        $("#ContentPlaceHolder1_lblCurrentStock").text(str);
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
                url: '../PurchaseManagment/frmPMRequisition.aspx/GetInvItemAttributeByItemIdAndAttributeType',
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


        function RequisitionOrAdhocWiseShowHideAndLoadItem() {
            var purchaseOrderType = $("#ContentPlaceHolder1_ddlPurchaseOrderType").val();
            var supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            var id = $("#ContentPlaceHolder1_ddlRequisitionOrder").val();
            var porderId = $("#ContentPlaceHolder1_hfPOrderId").val();

            if (supplierId == "0" && triggerOrNot == 0) {
                toastr.warning("Please Select Supplier.");
                return false;
            }
            else if (id == "0" && triggerOrNot == 0) {
                toastr.warning("Please Select Requisition Number.");
                return false;
            }

            $("#ItemForPurchase tbody").html("");
            ClearAfterAdhoqPurchaseItemAdded();

            $("#RequisitionItem tbody").html("");
            RequsitionOrderItem = new Array();

            if (id != "0")
                PageMethods.GetRequisitionItemForPurchaseById(id, supplierId, porderId, OnRequisitionItemSucceeded, OnRequisitionItemRateFailed);
        }

        function OnLoadConversionRateSucceeded(result) {
            $("#ContentPlaceHolder1_txtConversionRate").val(result.ConversionRate);
        }
        function OnLoadConversionRateFailed() {
        }
        function OnLoadAccountsCompanyInformationSucceeded(result) {
            if (result.CompanyAddress != "") {
                $("#ContentPlaceHolder1_txtDeliveryAddress").val(result.CompanyAddress);
            }
            PageMethods.LoadReceiveStoreByCompanyId(result.CompanyId, OnLoadReceiveStoreByCompanyIdSucceed, OnLoadReceiveStoreByCompanyIdFailed);
        }
        function OnLoadAccountsCompanyInformationFailed() {
        }
        function OnLoadReceiveStoreByCompanyIdSucceed(result) {
            //debugger;
            typesList = [];
            $("#ContentPlaceHolder1_ddlCostCentre").empty();
            var i = 0, fieldLength = result.length;

            if (fieldLength > 1) {
                typesList = result;
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlCostCentre');
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result[i].CostCenterId + '">' + result[i].CostCenter + '</option>').appendTo('#ContentPlaceHolder1_ddlCostCentre');
                }
            }
            else if (fieldLength > 0) {
                typesList = result;

                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result[i].CostCenterId + '">' + result[i].CostCenter + '</option>').appendTo('#ContentPlaceHolder1_ddlCostCentre');
                }
                $("#ContentPlaceHolder1_ddlCostCentre").val(result[0].CostCenterId + '').trigger("change");
            }
            else {
                $("<option value='0'>--No Stores Found--</option>").appendTo("#ContentPlaceHolder1_ddlCostCentre");
            }

            return false;
        }
        function OnLoadReceiveStoreByCompanyIdFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }
        function AddItemForPurchase() {
            if ($("#ContentPlaceHolder1_ddlPurchaseOrderType").val() == "AdHoc") {
                AddItemForAdhoqPurchase();
            }
            else {
                AddItemForPurchaseFromRequisition();
            }
        }

        function AddItemForAdhoqPurchase() {
            if (ItemSelected == null) {
                toastr.warning("Please Select Item.");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtPurchaseQuantity").val()) == "" || $.trim($("#ContentPlaceHolder1_txtPurchaseQuantity").val()) == "0") {
                toastr.warning("Please Give Purchase Quantity.");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtPurchasePrice").val()) == "" || $.trim($("#ContentPlaceHolder1_txtPurchasePrice").val()) == "0") {
                toastr.warning("Please Give Purchase Price.");
                return false;
            }


            var colorddlLength = $('#ContentPlaceHolder1_ddlColorAttribute > option').length;
            var sizeddlLength = $('#ContentPlaceHolder1_ddlSizeAttribute > option').length;
            var styleddlLength = $('#ContentPlaceHolder1_ddlStyleAttribute > option').length;
            var colorId = 0;
            if (colorddlLength > 0) {
                colorId = $("#ContentPlaceHolder1_ddlColorAttribute option:selected").val();
            }
            var colorText = $("#ContentPlaceHolder1_ddlColorAttribute option:selected").text();
            var sizeId = 0;
            if (sizeddlLength > 0) {
                sizeId = $("#ContentPlaceHolder1_ddlSizeAttribute option:selected").val();
            }
            var sizeText = $("#ContentPlaceHolder1_ddlSizeAttribute option:selected").text();
            var styleId = 0;
            if (styleddlLength > 0) {
                styleId = $("#ContentPlaceHolder1_ddlStyleAttribute option:selected").val();
            }
            var styleText = $("#ContentPlaceHolder1_ddlStyleAttribute option:selected").text();



            var itm = _.findWhere(PurchaseOrderItem, { ItemId: ItemSelected.ItemId, ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });

            if (itm != null) {
                toastr.warning("Same Item Already Added. Duplicate Item Is Not Accepted.");
                return false;
            }

            var total = 0, unitPrice = 0, quantity = 0, tr = "", remarks = "", t = "";

            unitPrice = $("#ContentPlaceHolder1_txtPurchasePrice").val();
            quantity = $("#ContentPlaceHolder1_txtPurchaseQuantity").val();
            remarks = $("#ContentPlaceHolder1_txtItemWiseRemarks").val();
            total = parseFloat(unitPrice) * parseFloat(quantity);

            tr += "<tr>";

            tr += "<td style='width:20%;'>" + ItemSelected.ItemName + "</td>";

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {

                tr += "<td style='width:10%;'>" + colorText + "</td>";
                tr += "<td style='width:10%;'>" + sizeText + "</td>";
                tr += "<td style='width:10%;'>" + styleText + "</td>";
            }
            else {
                tr += "<td style='display:none'>" + colorText + "</td>";
                tr += "<td style='display:none'>" + sizeText + "</td>";
                tr += "<td style='display:none'>" + styleText + "</td>";
            }
            tr += "<td style='display:none'>" + colorId + "</td>";
            tr += "<td style='display:none'>" + sizeId + "</td>";
            tr += "<td style='display:none'>" + styleId + "</td>";



            if (ItemSelected.LastPurchaseDate !== null)
                tr += "<td style='width:7%;'>" + CommonHelper.DateFormatDDMMMYYY(ItemSelected.LastPurchaseDate) + "</td>";
            else
                tr += "<td style='width:7%;'></td>";

            tr += "<td style='width:7%;'>" + ItemSelected.PurchasePrice + "</td>";

            tr += "<td style='width:7%;'>" +
                "<input type='text' value='" + unitPrice + "' id='pq" + ItemSelected.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                "</td>";
            tr += "<td style='width:7%;'>" +
                "<input type='text' value='" + quantity + "' id='pp" + ItemSelected.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                "</td>";

            tr += "<td style='width:7%;'>" + ItemSelected.UnitHead + "</td>";
            tr += "<td style='width:7%;'>" + total + "</td>";

            tr += "<td style='width:10%;'>" +
                "<input type='text' value='" + remarks + "' id='rm" + ItemSelected.ItemId + "' class='form-control' />" +
                "</td>";
            tr += "<td style='width:7%;'>" +
                "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>" +
                "</td>";

            tr += "<td style='display:none;'>" + ItemSelected.ItemId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.CategoryId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.StockBy + "</td>";
            tr += "<td style='display:none;'>" + unitPrice + "</td>";
            tr += "<td style='display:none;'>" + quantity + "</td>";
            tr += "<td style='display:none;'>0</td>";
            tr += "</tr>";

            $("#ItemForPurchase tbody").prepend(tr);
            tr = "";
            CalculateGrandTotal();

            PurchaseOrderItem.push({
                ItemId: parseInt(ItemSelected.ItemId, 10),
                StockById: parseInt(ItemSelected.StockBy, 10),
                Quantity: parseFloat(quantity),
                PurchasePrice: parseFloat(unitPrice),

                ColorId: parseInt(colorId, 10),
                SizeId: parseInt(sizeId, 10),
                StyleId: parseInt(styleId, 10),
                ColorText: colorText,
                SizeText: sizeText,
                StyleText: styleText,

                Remarks: remarks,
                DetailId: 0
            });

            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlGLCompanyId").attr("disabled", true);

            CommonHelper.ApplyDecimalValidation();
            ClearAfterAdhoqPurchaseItemAdded();
            $("#ContentPlaceHolder1_txtItem").focus();
        }
        function CheckRequisitionItemForPurchaseTable() {
            var length = $("#RequisitionItemForPurchase tbody tr").length;
            if (length != 0) {
                $("#ContentPlaceHolder1_ddlOrderType").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
            }
            else {
                $("#ContentPlaceHolder1_ddlOrderType").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
            }
        }
        function CalculateGrandTotal() {
            var grandTotal = 0;
            $("#ItemForPurchase tbody tr").each(function () {
                grandTotal += parseInt($(this).find("td:eq(12)").text());
            });

            if (grandTotal == 0) {
                $("#tableFoot").hide();
            }
            else {
                $("#tableFoot").show();
            }
            $("#grandTd").text(grandTotal);
        }
        function CalculateTotalForAdhoq(control) {
            var tr = $(control).parent().parent();

            var purchasePrice = $.trim($(tr).find("td:eq(9)").find("input").val());
            var quantity = $.trim($(tr).find("td:eq(10)").find("input").val());
            var oldPurchasePrice = $(tr).find("td:eq(18)").text();
            var oldQuantity = $(tr).find("td:eq(19)").text();

            if (purchasePrice == "" || purchasePrice == "0") {
                toastr.info("Purchase Price Cannot Be Zero Or Empty.");
                $(tr).find("td:eq(9)").find("input").val(oldPurchasePrice);
                return false;
            }
            else if (quantity == "" || quantity == "0") {
                toastr.info("Quantity Cannot Be Zero Or Empty.");
                $(tr).find("td:eq(10)").find("input").val(oldQuantity);
                return false;
            }

            var total = parseFloat(purchasePrice) * parseFloat(quantity);

            $(tr).find("td:eq(12)").text(total);

            var itemId = parseInt($.trim($(tr).find("td:eq(15)").text()), 10);
            var remarks = $.trim($(tr).find("td:eq(13)").find("input").val());

            var colorId = $(tr).find("td:eq(4)").text();
            var sizeId = $(tr).find("td:eq(5)").text();
            var styleId = $(tr).find("td:eq(6)").text();

            var item = _.findWhere(PurchaseOrderItem, { ItemId: itemId, ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });
            var index = _.indexOf(PurchaseOrderItem, item);

            PurchaseOrderItem[index].Quantity = parseFloat(quantity);
            PurchaseOrderItem[index].PurchasePrice = parseFloat(purchasePrice);
            PurchaseOrderItem[index].Remarks = remarks;

            $(tr).find("td:eq(18)").text(purchasePrice);
            $(tr).find("td:eq(19)").text(quantity);

            CalculateGrandTotal();

        }
        function DeleteAdhoqItem(control) {
            if (!confirm("Do you want to delete item?")) { return false; }

            var tr = $(control).parent().parent();

            var colorId = $(tr).find("td:eq(4)").text();
            var sizeId = $(tr).find("td:eq(5)").text();
            var styleId = $(tr).find("td:eq(6)").text();

            var itemId = parseInt($.trim($(tr).find("td:eq(15)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(20)").text()), 10);
            var item = _.findWhere(PurchaseOrderItem, { ItemId: itemId, ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });
            var index = _.indexOf(PurchaseOrderItem, item);

            if (parseInt(detailsId, 10) > 0)
                PurchaseOrderItemDeleted.push(JSON.parse(JSON.stringify(item)));

            PurchaseOrderItem.splice(index, 1);
            $(tr).remove();

            CalculateGrandTotal();

            if ($("#ItemForPurchase tbody tr").length == 0) {
                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlGLCompanyId").attr("disabled", false);
            }
        }
        function ClearAfterAdhoqPurchaseItemAdded() {

            $("#ContentPlaceHolder1_ddlCategory").val('0').trigger('change');
            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_lblCurrentStock").text("");
            $("#ContentPlaceHolder1_lblCurrentStockBy").text("");
            $("#ContentPlaceHolder1_txtPurchaseQuantity").val("");
            $("#ContentPlaceHolder1_txtPurchasePrice").val("");
            $("#ContentPlaceHolder1_txtItemWiseRemarks").val("");
            $("#RequisitionItem tbody").html("");
            $("#ContentPlaceHolder1_ddlColorAttribute").empty();
            $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
            $("#ContentPlaceHolder1_ddlStyleAttribute").empty();
            ItemSelected = null;
        }

        function ClearAfterAdhoqPurchaseItemAddedByCategoryChange() {
            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_lblCurrentStock").text("");
            $("#ContentPlaceHolder1_lblCurrentStockBy").text("");
            $("#ContentPlaceHolder1_txtPurchaseQuantity").val("");
            $("#ContentPlaceHolder1_txtPurchasePrice").val("");
            $("#ContentPlaceHolder1_txtItemWiseRemarks").val("");
            $("#RequisitionItem tbody").html("");
            $("#ContentPlaceHolder1_ddlColorAttribute").empty();
            $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
            $("#ContentPlaceHolder1_ddlStyleAttribute").empty();
            ItemSelected = null;
        }

        function OnRequisitionItemSucceeded(result) {
            debugger;
            if (result.length == 0) {
                toastr.info("Item Is Already Used In Another Purchase Order. Please Search and Check the Report.");
                return false;
            }

            RequsitionOrderItem = result;
            var requisition = _.findWhere(RequsitionOrderList, { RequisitionId: parseInt($("#ContentPlaceHolder1_ddlRequisitionOrder").val(), 10) });

            if (requisition != null) {
                $("#ContentPlaceHolder1_ddlRequisitionCostcenter").val(requisition.FromCostCenterId + '');
            }

            $("#OrderCheck").prop("checked", true);
            $("#RequisitionItem tbody").html("");
            var totalRow = result.length, row = 0, status = "";
            var tr = "";

            for (row = 0; row < totalRow; row++) {
                tr += "<tr>";

                if (result[row].SupplierId > 0) {
                    tr += "<td style='width:5%;'>" +
                        "<input type='checkbox' checked='checked' id='chk' " + result[row].ItemId + " />" +
                        "</td>";
                }
                else {
                    tr += "<td style='width:5%;'></td>";
                }

                tr += "<td style='width:11%;'>" + result[row].ItemName + "</td>";

                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {

                    tr += "<td style='width:7%;'>" + result[row].ColorText + "</td>";
                    tr += "<td style='width:7%;'>" + result[row].SizeText + "</td>";
                    tr += "<td style='width:7%;'>" + result[row].StyleText + "</td>";
                }
                else {
                    tr += "<td style='display:none'>" + result[row].ColorText + "</td>";
                    tr += "<td style='display:none'>" + result[row].SizeText + "</td>";
                    tr += "<td style='display:none'>" + result[row].StyleText + "</td>";
                }




                tr += "<td style='display:none;'>" + result[row].ColorId + "</td>";
                tr += "<td style='display:none;'>" + result[row].SizeId + "</td>";
                tr += "<td style='display:none;'>" + result[row].StyleId + "</td>";

                if (result[row].LastPurchaseDate != null)
                    tr += "<td style='width:7%;'>" + CommonHelper.DateFormatDDMMYYY(result[row].LastPurchaseDate) + "</td>";
                else
                    tr += "<td style='width:7%;'></td>";

                tr += "<td style='width:7%;'>" + result[row].PurchasePrice + "</td>";
                tr += "<td style='width:7%;'>" + result[row].RequisitionQuantity + "</td>";
                tr += "<td style='width:7%;'>" + result[row].ApprovedQuantity + "</td>";
                tr += "<td style='width:7%;'>" + result[row].RemainingPOQuantity + "</td>";
                tr += "<td style='width:7%;'>" + result[row].UnitName + "</td>";

                tr += "<td style='width:7%;'>" +
                    "<input type='text' value='" + result[row].RemainingPOQuantity + "' id='pq' " + result[row].ItemId + " class='form-control quantitydecimal' onblur='CheckRequisitionWiseItemQuantity(this)' />" +
                    "</td>";

                tr += "<td style='width:7%;'>" +
                    "<input type='text' value='" + result[row].PurchasePrice + "' id='pp' " + result[row].ItemId + " class='form-control quantitydecimal' onblur='CheckRequisitionWiseItemPrice(this)' />" +
                    "</td>";

                if (result[row].Remarks != null)
                    tr += "<td style='width:7%;'>" + result[row].Remarks + "</td>";
                else
                    tr += "<td style='width:7%;'></td>";

                tr += "<td style='display:none;'>" + result[row].RequisitionId + "</td>";
                tr += "<td style='display:none;'>" + result[row].RequisitionDetailsId + "</td>";
                tr += "<td style='display:none;'>" + result[row].ItemId + "</td>";
                tr += "<td style='display:none;'>" + result[row].StockById + "</td>";

                tr += "</tr>";

                $("#RequisitionItem tbody").append(tr);
                tr = "";
            }

            CommonHelper.ApplyDecimalValidation();
        }
        function OnRequisitionItemRateFailed() {
        }

        function AddItemForPurchaseFromRequisition() {
            if ($("#RequisitionItem tbody tr").find("td:eq(0)").find("input").is(":checked") == false) {
                toastr.warning("Please Select Item Before Purchase Order.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlPurchaseOrderType").val() == 'Requisition' && $("#RequisitionItem tbody tr").length == 0) {
                toastr.warning("Please Load Item From Requisition For Purchase.");
                return false;
            }

            var tr = "", existingTr = "", existingTr1 = "", existingTr2 = "", existingTr3 = "", tableLength = 0, row = 0, index = 0, indexRequisition = 0;
            var itemId = "", itemName = "", lastPoDate = "", lastPoRate = "", unitHead = "",
                purchaseQuantity = 0, purchasePrice = 0, total = 0, stockBy = 0, approvedQuantity = 0, remainQuantity = 0,
                requisitionId = 0, requisitionDetailsId = 0, remarks = null, requisitionNumber = "", checkTotal = 0;


            tableLength = $("#RequisitionItem tbody tr").length;

            var rqRow = 0, rqCount = PurchaseOrderItemFromRequisition.length, message = "";
            for (rqRow = 0; rqRow < tableLength; rqRow++) {

                if ($("#RequisitionItem tbody tr:eq(" + rqRow + ")").find("td:eq(0)").find("input").is(":checked")) {
                    itemId = $("#RequisitionItem tbody tr:eq(" + rqRow + ")").find("td:eq(19)").text();
                    purchaseQuantity = $("#RequisitionItem tbody tr:eq(" + rqRow + ")").find("td:eq(14)").find("input").val();
                    purchasePrice = $("#RequisitionItem tbody tr:eq(" + rqRow + ")").find("td:eq(15)").find("input").val();
                    requisitionId = $("#RequisitionItem tbody tr:eq(" + rqRow + ")").find("td:eq(17)").text();
                    requisitionDetailsId = $("#RequisitionItem tbody tr:eq(" + rqRow + ")").find("td:eq(18)").text();
                    approvedQuantity = $("#RequisitionItem tbody tr:eq(" + rqRow + ")").find("td:eq(11)").text();
                    remainQuantity = $("#RequisitionItem tbody tr:eq(" + rqRow + ")").find("td:eq(12)").text();
                    itemName = $("#RequisitionItem tbody tr:eq(" + rqRow + ")").find("td:eq(1)").text();
                    remarks = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(16)").text();

                    if (purchasePrice == "0") {
                        message = "Purchase Price of " + itemName + " Cannot Be zore(0)";
                        break;
                    }

                    for (row = 0; row < rqCount; row++) {
                        if (PurchaseOrderItemFromRequisition[row].RequisitionDetailsId == parseInt(requisitionDetailsId, 10)) {
                            checkTotal = parseFloat(purchaseQuantity) + PurchaseOrderItemFromRequisition[row].Quantity;

                            if (checkTotal > parseFloat(remainQuantity)) {
                                message = "Purchase Quantity Cannot Greater Than (>) Remain Quantity, Including Already Added Quantity Same Requisition Of Item - " + itemName + ".";
                                break;
                            }
                        }
                    }

                    if (message != "") { break; }
                }
            }

            if (message != "") {
                toastr.warning(message);
                return false;
            }

            itemId = "", purchaseQuantity = "", purchasePrice = "", stockBy = "", requisitionId = "", requisitionDetailsId = "",
                approvedQuantity = 0, remainQuantity = 0, itemName = "", row = 0;
            var colorId = 0, sizeId = 0, styleId = 0, colorText = "", sizeText = "", styleText = "";


            var addedItemAlready = null, addedRequisitionAlready = null;

            var requisition = _.findWhere(RequsitionOrderList, { RequisitionId: parseInt($("#ContentPlaceHolder1_ddlRequisitionOrder").val(), 10) });

            if (requisition != null) {
                requisitionNumber = requisition.PRNumber;
            }

            CommonHelper.ExactMatch();

            for (row = 0; row < tableLength; row++) {

                if ($("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(0)").find("input").is(":checked")) {

                    itemId = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(19)").text();
                    purchaseQuantity = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(14)").find("input").val();
                    purchasePrice = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(15)").find("input").val();



                    colorId = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(5)").text();
                    sizeId = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(6)").text();
                    styleId = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(7)").text();
                    colorText = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(2)").text();
                    sizeText = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(3)").text();
                    styleText = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(4)").text();


                    stockBy = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(20)").text();
                    requisitionId = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(17)").text();
                    requisitionDetailsId = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(18)").text();
                    approvedQuantity = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(11)").text();
                    remainQuantity = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(12)").text();

                    itemName = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(1)").text();

                    remarks = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(16)").text();

                    if (purchasePrice == "0") {
                        toastr.info("Purchase Price of " + itemName + " Cannot Be zore(0)");
                        break;
                    }
                    else if (purchaseQuantity == "0") {
                        toastr.info("Purchase Quantity " + itemName + " Cannot Be zore(0)");
                        break;
                    }

                    addedItemAlready = _.findWhere(PurchaseOrderItem, { ItemId: parseInt(itemId, 10), ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });

                    if (addedItemAlready != null) {

                        index = _.indexOf(PurchaseOrderItem, addedItemAlready);
                        PurchaseOrderItem[index].Quantity = PurchaseOrderItem[index].Quantity + parseFloat(purchaseQuantity);
                        PurchaseOrderItem[index].PurchasePrice = parseFloat(purchasePrice);

                        total = PurchaseOrderItem[index].Quantity * parseFloat(purchasePrice);

                        addedRequisitionAlready = _.findWhere(PurchaseOrderItemFromRequisition, { RequisitionDetailsId: parseInt(requisitionDetailsId, 10) });

                        if (addedRequisitionAlready == null) {

                            PurchaseOrderItemFromRequisition.push({
                                PRNumber: requisitionNumber,
                                RequisitionId: parseInt(requisitionId, 10),
                                RequisitionDetailsId: parseInt(requisitionDetailsId, 10),
                                POrderId: 0,
                                DetailId: 0,
                                ItemId: parseInt(itemId, 10),
                                StockById: parseInt(stockBy, 10),
                                ApprovedQuantity: approvedQuantity,
                                RemainingPOQuantity: remainQuantity,
                                Quantity: parseFloat(purchaseQuantity),
                                PurchasePrice: parseFloat(purchasePrice),


                                ColorId: parseInt(colorId, 10),
                                SizeId: parseInt(sizeId, 10),
                                StyleId: parseInt(styleId, 10),
                                ColorText: colorText,
                                SizeText: sizeText,
                                StyleText: styleText,

                                Remarks: remarks
                            });

                            var reqEditedItem = _.where(PurchaseOrderItemFromRequisition, { ItemId: parseInt(itemId, 10), ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });

                            indexRequisition = -1; rqRow = 0; rqCount = 0;
                            rqCount = reqEditedItem.length;

                            for (rqRow = 0; rqRow < rqCount; rqRow++) {

                                var alreadyAddedItem = _.findWhere(PurchaseOrderItemFromRequisition, { RequisitionDetailsId: reqEditedItem[rqRow].RequisitionDetailsId });
                                indexRequisition = _.indexOf(PurchaseOrderItemFromRequisition, alreadyAddedItem);
                                PurchaseOrderItemFromRequisition[indexRequisition].PurchasePrice = parseFloat(purchasePrice);
                            }
                        }
                        else {
                            indexRequisition = _.indexOf(PurchaseOrderItemFromRequisition, addedRequisitionAlready);
                            PurchaseOrderItemFromRequisition[indexRequisition].Quantity = PurchaseOrderItemFromRequisition[indexRequisition].Quantity + parseFloat(purchaseQuantity);
                            PurchaseOrderItemFromRequisition[indexRequisition].PurchasePrice = parseFloat(purchasePrice);
                        }

                        existingTr1 = $("#RequisitionItemForPurchase tbody tr").find("td:eq(15):textEquals('" + itemId + "')").parent();
                        existingTr2 = $(existingTr1).find("td:eq(4):textEquals('" + colorId + "')").parent();
                        existingTr3 = $(existingTr2).find("td:eq(5):textEquals('" + sizeId + "')").parent();
                        existingTr = $(existingTr3).find("td:eq(6):textEquals('" + styleId + "')").parent();


                        //existingTr = $("#RequisitionItemForPurchase tbody tr").find("td:eq(15):textEquals('" + itemId + "')").find("td:eq(4):textEquals('" + colorId + "')").find("td:eq(5):textEquals('" + sizeId + "')").find("td:eq(6):textEquals('" + styleId + "')").parent();
                        $(existingTr).find("td:eq(9)").find("input").val(purchasePrice);
                        $(existingTr).find("td:eq(10)").find("input").val(PurchaseOrderItem[index].Quantity);
                        $(existingTr).find("td:eq(12)").text(total);

                        continue;
                    }

                    lastPoDate = $.trim($("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(8)").text());
                    lastPoRate = $.trim($("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(9)").text());
                    unitHead = $("#RequisitionItem tbody tr:eq(" + row + ")").find("td:eq(13)").text();

                    total = parseFloat(purchaseQuantity) * parseFloat(purchasePrice);

                    tr = "<tr>";

                    tr += "<td style='width:20%;'>" + itemName + "</td>";

                    if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {

                        tr += "<td style='width:7%;'>" + colorText + "</td>";
                        tr += "<td style='width:7%;'>" + sizeText + "</td>";
                        tr += "<td style='width:7%;'>" + styleText + "</td>";
                    }
                    else {
                        tr += "<td style='display:none'>" + colorText + "</td>";
                        tr += "<td style='display:none'>" + sizeText + "</td>";
                        tr += "<td style='display:none'>" + styleText + "</td>";
                    }

                    tr += "<td style='display:none;'>" + colorId + "</td>";
                    tr += "<td style='display:none;'>" + sizeId + "</td>";
                    tr += "<td style='display:none;'>" + styleId + "</td>";

                    if (lastPoDate !== "")
                        tr += "<td style='width:7%;'>" + lastPoDate + "</td>";
                    else
                        tr += "<td style='width:7%;'></td>";

                    tr += "<td style='width:7%;'>" + lastPoRate + "</td>";

                    tr += "<td style='width:7%;'>" +
                        "<input type='text' value='" + purchasePrice + "' id='pq" + itemId + "' class='form-control quantitydecimal' onblur='CheckRequisitionPrice(this)' />" +
                        "</td>";
                    tr += "<td style='width:7%;'>" +
                        "<input type='text' disabled='disabled' value='" + purchaseQuantity + "' id='pp" + itemId + "' class='form-control quantitydecimal' />" +
                        "</td>";

                    tr += "<td style='width:7%;'>" + unitHead + "</td>";
                    tr += "<td style='width:7%;'>" + total + "</td>";

                    tr += "<td style='width:10%;'>" +
                        "<input type='text' value='" + remarks + "' id='rm" + itemId + "' class='form-control' />" +
                        "</td>";

                    tr += "<td style='width:7%;'>" +
                        "<a onclick=\"javascript:return EditPurchaseRequisitionItem(this);\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>" +
                        "&nbsp;" +
                        "<a href='javascript:void()' onclick= 'DeletePurchaseRequisitionItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>" +
                        "</td>";
                    tr += "<td style='display:none;'>" + itemId + "</td>";
                    tr += "<td style='display:none;'>" + stockBy + "</td>";

                    tr += "</tr>";

                    $("#RequisitionItemForPurchase tbody").prepend(tr);

                    PurchaseOrderItem.push({
                        ItemId: parseInt(itemId, 10),
                        StockById: parseInt(stockBy, 10),
                        Quantity: parseFloat(purchaseQuantity),
                        PurchasePrice: parseFloat(purchasePrice),

                        ColorId: parseInt(colorId, 10),
                        SizeId: parseInt(sizeId, 10),
                        StyleId: parseInt(styleId, 10),
                        ColorText: colorText,
                        SizeText: sizeText,
                        StyleText: styleText,

                        Remarks: remarks,
                        DetailId: 0
                    });

                    PurchaseOrderItemFromRequisition.push({
                        PRNumber: requisitionNumber,
                        RequisitionId: parseInt(requisitionId, 10),
                        RequisitionDetailsId: parseInt(requisitionDetailsId, 10),
                        POrderId: 0,
                        DetailId: 0,
                        ItemId: parseInt(itemId, 10),
                        StockById: parseInt(stockBy, 10),
                        ApprovedQuantity: approvedQuantity,
                        RemainingPOQuantity: remainQuantity,
                        Quantity: parseFloat(purchaseQuantity),
                        PurchasePrice: parseFloat(purchasePrice),

                        ColorId: parseInt(colorId, 10),
                        SizeId: parseInt(sizeId, 10),
                        StyleId: parseInt(styleId, 10),
                        ColorText: colorText,
                        SizeText: sizeText,
                        StyleText: styleText,

                        Remarks: remarks
                    });

                    itemName = ""; lastPoDate = ""; lastPoRate = ""; unitHead = "";
                    purchaseQuantity = 0; purchasePrice = 0; total = 0; tr = "";
                }
            }

            if (row == tableLength) {
                ClearAfterPurchaseItemAddedFromRequisition(requisitionId);
            }
            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
            CheckRequisitionItemForPurchaseTable();
            CommonHelper.ApplyDecimalValidation();
        }

        function ClearAfterPurchaseItemAddedFromRequisition(requisitionId) {
            $("#RequisitionItem tbody").html("");
        }

        function EditPurchaseRequisitionItem(control) {
            var tr = $(control).parent().parent();
            CommonHelper.ExactMatch();
            $("#requisitionWiseItemTable tbody").html("");

            var colorId = 0, sizeId = 0, styleId = 0, colorText = "", sizeText = "", styleText = "";




            var ctr = "";
            var itemId = $(tr).find("td:eq(15)").text();
            var itemName = $(tr).find("td:eq(0)").text();
            var purchasePrice = $(tr).find("td:eq(9)").find("input").val();
            var unitHead = $(tr).find("td:eq(11)").text();

            colorId = $(tr).find("td:eq(4)").text();
            sizeId = $(tr).find("td:eq(5)").text();
            styleId = $(tr).find("td:eq(6)").text();
            colorText = $(tr).find("td:eq(1)").text();
            sizeText = $(tr).find("td:eq(2)").text();
            styleText = $(tr).find("td:eq(3)").text();

            var requisitionWiseItem = _.where(PurchaseOrderItemFromRequisition, { ItemId: parseInt(itemId, 10), ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });

            $.each(requisitionWiseItem, function (index, obj) {

                obj.PurchasePrice = parseFloat(purchasePrice);

                ctr = "<tr>";
                ctr += "<td style='width:10%;'>" + obj.PRNumber + "</td>";
                ctr += "<td style='width:25%;'>" + itemName + "</td>";

                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {

                    ctr += "<td style='width:7%;'>" + colorText + "</td>";
                    ctr += "<td style='width:7%;'>" + sizeText + "</td>";
                    ctr += "<td style='width:7%;'>" + styleText + "</td>";
                }
                else {
                    ctr += "<td style='display:none'>" + colorText + "</td>";
                    ctr += "<td style='display:none'>" + sizeText + "</td>";
                    ctr += "<td style='display:none'>" + styleText + "</td>";
                }

                //ctr += "<td style='width:7%;'>" + colorText + "</td>";
                //ctr += "<td style='width:7%;'>" + sizeText + "</td>";
                //ctr += "<td style='width:7%;'>" + styleText + "</td>";

                ctr += "<td style='display:none;'>" + colorId + "</td>";
                ctr += "<td style='display:none;'>" + sizeId + "</td>";
                ctr += "<td style='display:none;'>" + styleId + "</td>";


                ctr += "<td style='width:10%;'>" + obj.RemainingPOQuantity + "</td>";

                ctr += "<td style='width:10%;'>" +
                    "<input type='text' disabled='disabled' value='" + purchasePrice + "' id='pq" + itemId + "' class='form-control quantitydecimal' title='Price Cannot Zero Or Empty.' />" +
                    "</td>";
                ctr += "<td style='width:10%;'>" +
                    "<input type='text' value='" + obj.Quantity + "' id='pp" + obj.ItemId + "' class='form-control quantitydecimal' onblur='CheckRequisitionQuanity(this)' title='Quanity Must Less Than Equall (<=) Remain Quanity' />" +
                    "</td>";

                ctr += "<td style='width:10%;'>" + unitHead + "</td>";

                ctr += "<td style='width:5%;'>" +
                    "<a href='javascript:void()' onclick= 'DeleteRequisitionWiseItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>" +
                    "</td>";

                ctr += "<td style='display:none;'>" + itemId + "</td>";
                ctr += "<td style='display:none;'>" + obj.RequisitionId + "</td>";
                ctr += "<td style='display:none;'>" + obj.RequisitionDetailsId + "</td>";
                ctr += "<td style='display:none;'>" + obj.POrderId + "</td>"; //purchase order
                ctr += "<td style='display:none;'>" + obj.DetailId + "</td>"; //purchase order details id

                ctr += "<td style='display:none;'>" + purchasePrice + "</td>"; //Existing Purchase Price
                ctr += "<td style='display:none;'>" + obj.Quantity + "</td>"; //Existing Quantity

                ctr += "</tr>";

                $("#requisitionWiseItemTable tbody").append(ctr);
                ctr = "";

                CommonHelper.ApplyDecimalValidation();
            });

            $("#RequisitionWiseItemDialog").dialog({
                width: 900,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                title: "Requisition Wise Item",
                show: 'slide',
                open: function (event, ui) {
                    $('#RequisitionWiseItemDialog').css('overflow', 'hidden');
                    $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                }
            });
        }

        function CheckRequisitionQuanity(control) {
            //debugger;
            var tr = $(control).parent().parent();
            var quantity = $.trim($(control).val());
            var approvedQuantity = parseFloat($(tr).find("td:eq(8)").text());
            var oldQuantity = parseFloat($(tr).find("td:eq(19)").text());

            if (quantity == "" || quantity == "0") {
                toastr.warning("Quantity Cannot Empty Or Zero");
                $(control).val(oldQuantity);
            }
            else if (parseFloat(quantity) > approvedQuantity) {
                toastr.warning("Quantity Cannot Greater Than Remain Quantity");
                $(control).val(oldQuantity);
            }
        }

        function CheckRequisitionPrice(control) {
            var tr = $(control).parent().parent();
            var price = $.trim($(control).val());
            var lastPurchasePrice = parseFloat($(tr).find("td:eq(8)").text());
            var purchaseQuantity = parseFloat($(tr).find("td:eq(10)").find("input").val());

            if (price == "" || price == "0") {
                toastr.warning("Price Cannot Empty Or Zero.");
                $(control).val(lastPurchasePrice);
                price = lastPurchasePrice;
            }

            var total = parseFloat(price) * parseFloat(purchaseQuantity);
            $(tr).find("td:eq(12)").text(total);

            var itemId = parseInt($.trim($(tr).find("td:eq(15)").text()), 10);

            var remarks = $.trim($(tr).find("td:eq(13)").find("input").val());


            var colorId = $(tr).find("td:eq(4)").text();
            var sizeId = $(tr).find("td:eq(5)").text();
            var styleId = $(tr).find("td:eq(6)").text();

            var item = _.findWhere(PurchaseOrderItem, { ItemId: itemId, ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });
            var index = _.indexOf(PurchaseOrderItem, item);

            PurchaseOrderItem[index].PurchasePrice = parseFloat(price);
            PurchaseOrderItem[index].Remarks = remarks;

            var row = 0, rowCount = 0, requisitionDetailsId = 0;

            var editedItem = _.where(PurchaseOrderItemFromRequisition, { ItemId: itemId, ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });
            rowCount = editedItem.length;

            for (row = 0; row < rowCount; row++) {
                requisitionDetailsId = editedItem[row].RequisitionDetailsId;

                var reqItem = _.findWhere(PurchaseOrderItemFromRequisition, { RequisitionDetailsId: requisitionDetailsId });
                var reqIndex = _.indexOf(PurchaseOrderItemFromRequisition, reqItem);
                PurchaseOrderItemFromRequisition[reqIndex].PurchasePrice = parseFloat(price);
            }
        }

        function CheckRequisitionWiseItemQuantity(control) {
            var tr = $(control).parent().parent();
            var quantity = $.trim($(control).val());
            var approvedQuantity = parseFloat($(tr).find("td:eq(11)").text());
            var remainQuantity = parseFloat($(tr).find("td:eq(12)").text());
            var oldQuantity = parseFloat($(tr).find("td:eq(12)").text());

            if (quantity == "" || quantity == "0") {
                toastr.warning("Quantity Cannot Empty Or Zero");
                $(control).val(oldQuantity);
            }
            else if (parseFloat(quantity) > remainQuantity) {
                toastr.warning("Quantity Cannot Greater Than Remain Quantity");
                $(control).val(oldQuantity);
            }
        }

        function CheckRequisitionWiseItemPrice(control) {
            var tr = $(control).parent().parent();
            var price = $.trim($(control).val());
            var lastPurchasePrice = parseFloat($(tr).find("td:eq(3)").text());

            if (price == "" || price == "0") {
                toastr.warning("Price Cannot Empty Or Zero");
                $(control).val(lastPurchasePrice);
            }
        }

        function DeletePurchaseRequisitionItem(control) {
            if (!confirm("Do you want to delete item?")) { return false; }

            var tr = $(control).parent().parent();
            var itemId = parseInt($.trim($(tr).find("td:eq(15)").text()), 10);
            var reqRow = 0, reqCount = 0, requisitionDetailsId = 0;


            var colorId = $(tr).find("td:eq(4)").text();
            var sizeId = $(tr).find("td:eq(5)").text();
            var styleId = $(tr).find("td:eq(6)").text();

            var deletedItem = _.where(PurchaseOrderItemFromRequisition, { ItemId: itemId, ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });

            reqCount = deletedItem.length;

            for (reqRow = 0; reqRow < reqCount; reqRow++) {

                if (deletedItem[reqRow].DetailId > 0)
                    PurchaseOrderItemDeleted.push(JSON.parse(JSON.stringify(deletedItem[reqRow])));

                requisitionDetailsId = deletedItem[reqRow].RequisitionDetailsId;

                var reqItem = _.findWhere(PurchaseOrderItemFromRequisition, { RequisitionDetailsId: requisitionDetailsId });
                var reqIndex = _.indexOf(PurchaseOrderItemFromRequisition, reqItem);
                PurchaseOrderItemFromRequisition.splice(reqIndex, 1);
            }

            var item = _.findWhere(PurchaseOrderItem, { ItemId: itemId, ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });
            var index = _.indexOf(PurchaseOrderItem, item);
            PurchaseOrderItem.splice(index, 1);

            $(tr).remove();

            if ($("#RequisitionItemForPurchase tbody tr").length == 0) {
                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlOrderType").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlGLCompanyId").attr("disabled", false);
            }
        }

        function DeleteRequisitionWiseItem(control) {
            if (!confirm("Do you want to delete item?")) { return false; }

            var tr = $(control).parent().parent();
            vv = tr;

            var itemId = $(tr).find("td:eq(13)").text();
            var requisitionId = $(tr).find("td:eq(14)").text();
            var requisitionDetailsId = $(tr).find("td:eq(15)").text();
            var purchaseOrderId = $(tr).find("td:eq(16)").text();
            var purchaseDetailsId = $(tr).find("td:eq(17)").text();
            var purchasePrice = $(tr).find("td:eq(9)").find("input").val();
            var quantity = $(tr).find("td:eq(10)").find("input").val();

            if (purchaseDetailsId != "0") {
                RequisitionWiseDeletedItem.push({
                    RequisitionId: parseInt(requisitionId, 10),
                    RequisitionDetailsId: parseInt(requisitionDetailsId, 10),
                    POrderId: parseInt(purchaseOrderId, 10),
                    DetailId: parseInt(purchaseDetailsId, 10),
                    ItemId: parseInt(itemId, 10),
                    Quantity: parseFloat(quantity),
                    PurchasePrice: parseFloat(purchasePrice)
                });
            }

            var reqItem = _.findWhere(PurchaseOrderItemFromRequisition, { RequisitionDetailsId: requisitionDetailsId });
            var reqIndex = _.indexOf(PurchaseOrderItemFromRequisition, reqItem);
            PurchaseOrderItemFromRequisition.splice(reqIndex, 1);

            $(tr).remove();
        }

        function ApplyRequisitionWiseItemPurcaseEdit() {
            debugger;
            var existingQuantity = 0, existingPurchasePrice = 0, itemId = "",
                requisitionId = 0, requisitionDetailsId = 0, purchasePrice = 0, quantity = 0,
                trCount = 0, totalQuantity = 0, deletedTotalQuantity = 0;

            var colorId = 0, sizeId = 0, styleId = 0, colorText = "", sizeText = "", styleText = "";



            var requisitionWiseItem = null, index = 0, existingTr = "", existingTr1 = "", existingTr2 = "", existingTr3 = "";

            trCount = $("#requisitionWiseItemTable tbody tr").length;

            $.each(RequisitionWiseDeletedItem, function (index, obj) {
                deletedTotalQuantity = deletedTotalQuantity + obj.Quantity;

                requisitionWiseItem = _.findWhere(PurchaseOrderItemFromRequisition, { RequisitionDetailsId: parseInt(obj.RequisitionDetailsId, 10) });
                index = _.indexOf(PurchaseOrderItemFromRequisition, requisitionWiseItem);

                itemId = requisitionWiseItem.ItemId;
                colorId = requisitionWiseItem.ColorId;
                sizeId = requisitionWiseItem.SizeId;
                styleId = requisitionWiseItem.StyleId;
                colorText = requisitionWiseItem.ColorText;
                sizeText = requisitionWiseItem.SizeText;
                styleText = requisitionWiseItem.StyleText;

                PurchaseOrderItemDeleted.push(JSON.parse(JSON.stringify(requisitionWiseItem)));
                PurchaseOrderItemFromRequisition.splice(index, 1);
            });

            if (trCount == 0) {
                CommonHelper.ExactMatch();

                existingTr1 = $("#RequisitionItemForPurchase tbody tr").find("td:eq(15):textEquals('" + itemId + "')").parent();
                existingTr2 = $(existingTr1).find("td:eq(4):textEquals('" + colorId + "')").parent();
                existingTr3 = $(existingTr2).find("td:eq(5):textEquals('" + sizeId + "')").parent();
                existingTr = $(existingTr3).find("td:eq(6):textEquals('" + styleId + "')").parent();

                //existingTr = $("#RequisitionItemForPurchase tbody tr").find("td:eq(15):textEquals('" + itemId + "')").find("td:eq(4):textEquals('" + colorId + "')").find("td:eq(5):textEquals('" + sizeId + "')").find("td:eq(6):textEquals('" + styleId + "')").parent();
                $(existingTr).remove();
            }
            else {
                $("#requisitionWiseItemTable tbody tr").each(function () {

                    itemId = $(this).find("td:eq(13)").text();
                    requisitionId = $(this).find("td:eq(14)").text();
                    requisitionDetailsId = $(this).find("td:eq(15)").text();

                    purchasePrice = $(this).find("td:eq(9)").find("input").val();
                    quantity = $(this).find("td:eq(10)").find("input").val();

                    existingPurchasePrice = $(this).find("td:eq(18)").text();
                    existingQuantity = $(this).find("td:eq(19)").text();

                    colorId = $(this).find("td:eq(5)").text();
                    sizeId = $(this).find("td:eq(6)").text();
                    styleId = $(this).find("td:eq(7)").text();

                    if ((parseFloat(quantity) != parseFloat(existingQuantity))) {

                        requisitionWiseItem = _.findWhere(PurchaseOrderItemFromRequisition, { RequisitionDetailsId: parseInt(requisitionDetailsId, 10) });
                        index = _.indexOf(PurchaseOrderItemFromRequisition, requisitionWiseItem);
                        PurchaseOrderItemFromRequisition[index].Quantity = parseFloat(quantity);
                    }

                    totalQuantity = totalQuantity + parseFloat(quantity);
                });

                CommonHelper.ExactMatch();

                existingTr1 = $("#RequisitionItemForPurchase tbody tr").find("td:eq(15):textEquals('" + itemId + "')").parent();
                existingTr2 = $(existingTr1).find("td:eq(4):textEquals('" + colorId + "')").parent();
                existingTr3 = $(existingTr2).find("td:eq(5):textEquals('" + sizeId + "')").parent();
                existingTr = $(existingTr3).find("td:eq(6):textEquals('" + styleId + "')").parent();

                $(existingTr).find("td:eq(10)").find("input").val(totalQuantity);
                $(existingTr).find("td:eq(12)").text(parseFloat(existingPurchasePrice) * parseFloat(totalQuantity));
            }

            RequisitionWiseDeletedItem = new Array();
            $("#RequisitionWiseItemDialog").dialog("close");
        }

        function CancelRequisitionWiseItemPurcaseEdit() {
            RequisitionWiseDeletedItem = new Array();
            $("#requisitionWiseItemTable tbody").html("");

            $("#RequisitionWiseItemDialog").dialog("close");
        }

        function SavePurchaseOrder() {
            if (!IsCanSave) {
                alert("You have not permitted");
                return false;
            }

            if ($("#ContentPlaceHolder1_ddlSupplier").val() == "0" || $("#ContentPlaceHolder1_ddlSupplier").val() == null) {
                toastr.warning("Please Select Supplier.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlCostCentre").val() == "0" || $("#ContentPlaceHolder1_ddlCostCentre").val() == null) {
                toastr.warning("Please Select Store From.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtExpectedReceiveDate").val() == "" || $("#ContentPlaceHolder1_txtExpectedReceiveDate").val() == null) {
                toastr.warning("Please Give Expected Receive Date.");
                return false;
            }

            if ($("#ContentPlaceHolder1_ddlPurchaseOrderType").val() == "AdHoc" && $("#RequisitionItemForPurchase tbody tr").length == 0) {
                if ($("#ItemForPurchase tbody tr").length == 0) {
                    toastr.warning("Please Add Item For Purchase.");
                    return false;
                }
            }
            else {
                if ($("#RequisitionItemForPurchase tbody tr").length == 0) {
                    toastr.warning("Please Add Item From Requisition For Purchase.");
                    return false;
                }
            }

            var EditedPurchaseOrderItem = new Array();
            var itemId = "", remarks = "", purchaseRemarks = "";
            var purchaseItem = null;

            var POrderId = "0", poType = "", receivedByDate = null, isLocalOrForeignPO = "Local", supplierId = "0", isEdited = "0",
                categoryId = "", costCenterId = "", checkedBy = '', approvedBy = '';

            var companyId = $("#ContentPlaceHolder1_ddlGLCompanyId").val();
            pOrderId = $("#ContentPlaceHolder1_hfPOrderId").val();
            supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            var orderType = $("#ContentPlaceHolder1_ddlOrderType").val();
            costCenterId = $("#ContentPlaceHolder1_ddlCostCentre").val();

            if ($("#ContentPlaceHolder1_txtExpectedReceiveDate").val() != "")
                receivedByDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtExpectedReceiveDate").val(), '/');

            purchaseRemarks = $("#ContentPlaceHolder1_txtRemarks").val();
            checkedBy = 0; //$("#ContentPlaceHolder1_ddlCheckedBy").val();
            approvedBy = 0; // $("#ContentPlaceHolder1_ddlApprovedBy").val();

            currencyId = $("#ContentPlaceHolder1_ddlCurrency").val();
            var convertionRate = $("#ContentPlaceHolder1_txtConversionRate").val();
            convertionRate = convertionRate == '' ? '0' : convertionRate;
            poType = $("#ContentPlaceHolder1_ddlPurchaseOrderType").val();

            deliveryAddress = $("#ContentPlaceHolder1_txtDeliveryAddress").val();
            poDescription = $("#ContentPlaceHolder1_txtDescription").val();


            var PurchaseOrder = {
                CompanyId: companyId,
                POrderId: pOrderId,
                ReceivedByDate: receivedByDate,
                POType: poType,
                CostCenterId: costCenterId,
                IsLocalOrForeignPO: isLocalOrForeignPO,
                SupplierId: supplierId,
                OrderType: orderType,
                Remarks: purchaseRemarks,
                CheckedBy: checkedBy,
                ApprovedBy: approvedBy,
                CurrencyId: currencyId,
                ConvertionRate: convertionRate,
                DeliveryAddress: deliveryAddress,
                PODescription: poDescription
            };

            if (poType == 'AdHoc') {
                $("#ItemForPurchase tbody tr").each(function () {

                    itemId = $.trim($(this).find("td:eq(15)").text());
                    remarks = $(this).find("td:eq(13)").find("input").val();

                    var colorId = $(this).find("td:eq(4)").text();
                    var sizeId = $(this).find("td:eq(5)").text();
                    var styleId = $(this).find("td:eq(6)").text();

                    if ($.trim(remarks) != "") {
                        purchaseItem = _.findWhere(PurchaseOrderItem, { ItemId: parseInt(itemId, 10), ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });
                        index = _.indexOf(PurchaseOrderItem, purchaseItem);
                        PurchaseOrderItem[index].Remarks = remarks;
                    }
                });
            }
            else {
                $("#RequisitionItemForPurchase tbody tr").each(function () {

                    itemId = $.trim($(this).find("td:eq(15)").text());
                    remarks = $(this).find("td:eq(13)").find("input").val();
                    var colorId = $(this).find("td:eq(4)").text();
                    var sizeId = $(this).find("td:eq(5)").text();
                    var styleId = $(this).find("td:eq(6)").text();

                    if ($.trim(remarks) != "") {
                        purchaseItem = _.findWhere(PurchaseOrderItem, { ItemId: parseInt(itemId, 10), ColorId: parseInt(colorId, 10), SizeId: parseInt(sizeId, 10), StyleId: parseInt(styleId, 10) });
                        index = _.indexOf(PurchaseOrderItem, purchaseItem);
                        PurchaseOrderItem[index].Remarks = remarks;
                    }
                });
            }
            var TermsNConditions = new Array();
            $("#TblTNC tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                    var displaySequence = $.trim($(this).find("td:eq(1)").find("input").val());
                    var title = $.trim($(this).find("td:eq(2)").find("input").val());
                    var description = $.trim($(this).find("td:eq(3)").find("input").val());
                    var termsNConditionsId = $.trim($(this).find("td:eq(4)").text());
                    var Id = $.trim($(this).find("td:eq(5)").text());
                    TermsNConditions.push({
                        Id: Id,
                        TermsNConditionsId: termsNConditionsId,
                        Title: title,
                        DisplaySequence: displaySequence,
                        Description: description
                    });
                }
            });

            $.each(EditedTermsNConditions, function (count1, obj1) {
                var count = 0, count_3 = 0;
                $.each(TermsNConditions, function (count2, obj2) {
                    count_3++;
                    if (obj1.TermsNConditionsId != obj2.TermsNConditionsId) {
                        count++;
                    }
                })
                if (count == count_3) {
                    deletedTermsNConditions.push(obj1);
                }

            });

            if (poType == 'AdHoc') {
                PageMethods.SaveAdhocPurchaseOrder(PurchaseOrder, PurchaseOrderItem, EditedPurchaseOrderItem, PurchaseOrderItemDeleted, TermsNConditions, deletedTermsNConditions, OnSavePurchaseOrderSucceed, OnSavePurchaseOrderFailed);
            }
            else {
                PageMethods.SaveRequisitionWisePurchaseOrder(PurchaseOrder, PurchaseOrderItem, PurchaseOrderItemFromRequisition, PurchaseOrderItemDeleted, TermsNConditions, deletedTermsNConditions, OnSavePurchaseOrderSucceed, OnSavePurchaseOrderFailed);
            }

            return false;
        }
        function OnSavePurchaseOrderSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                    data: JSON.stringify({ tableName: 'PMPurchaseOrder', primaryKeyName: 'POrderId', primaryKeyValue: result.PrimaryKeyValue, featuresValue: 'Purchase', statusColumnName: 'ApprovedStatus' }),
                    dataType: "json",
                    success: function (data) {
                        debugger;

                        SendSMSToUserList(data.d, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);

                    },
                    error: function (result) {
                        toastr.error("Can not load Check or Approve By List.");
                    }
                });

                triggerOrNot = 1;
                PerformClearAction();
                LoadRequisition();
                CalculateGrandTotal();
                if (queryPurchaseOrderId != "") {
                    window.location = "/PurchaseManagment/PurchaseInfo.aspx";
                }
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

        function SearchPurchaseOrder(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#PurchaseOrderGrid tbody tr").length;
            var companyId = 0, poNumber = "0", status = "", poType = "", orderType = "0", supplierId = "0", costCenterId = "", fromDate = null, toDate = null;

            companyId = $("#ContentPlaceHolder1_ddlSrcGLCompanyId").val();
            poNumber = $("#ContentPlaceHolder1_txtSPONumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            poType = $("#ContentPlaceHolder1_ddlPOType").val();
            costCenterId = $("#ContentPlaceHolder1_ddlCostCenterSearch").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            supplierId = $("#ContentPlaceHolder1_ddlSearchSupplier").val();
            orderType = $("#ContentPlaceHolder1_ddlSrcOrderType").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            if (costCenterId == "0")
                costCenterId = null;

            if (supplierId == "0")
                supplierId = null;

            $("#GridPagingContainer ul").html("");
            $("#PurchaseOrderGrid tbody tr").remove();
            $("#PurchaseOrderGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchPurchaseOrder(companyId, orderType, poType, fromDate, toDate, poNumber, status, costCenterId, supplierId,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

            return false;
        }
        function OnSearchPurchaseOrderSucceed(result) {
            var tr = "";

            $.each(result.GridData, function (count, gridObject) {
                tr += "<tr>";

                tr += "<td style='width:8%;'>" + gridObject.PONumber + "</td>";
                tr += "<td style='width:8%;'>" + gridObject.POType + "</td>";

                if (gridObject.PODate !== null)
                    tr += "<td style='width:5%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.PODate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:5%;'></td>";

                tr += "<td style='width:10%;'>" + gridObject.CostCenter + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.SupplierName + "</td>";
                if (gridObject.ApprovedStatus == "Pending") {
                    tr += "<td style='width:8%;'>" + "Submitted" + "</td>";
                }
                else {
                    tr += "<td style='width:8%;'>" + gridObject.ApprovedStatus + "</td>";
                }

                if (gridObject.ReceiveStatus != null) {
                    tr += "<td style='width:10%;'>" + gridObject.ReceiveStatus + "</td>";
                }
                else {
                    tr += "<td style='width:10%;'></td>";
                }

                tr += "<td style='width:20%;'>" + gridObject.PODescription + "</td>";
                tr += "<td style='width:8%;'>" + gridObject.PurchaseOrderAmount + "</td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";

                if (gridObject.IsCanEdit && IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return PurchaseOrderEdit('" + gridObject.POType + "'," + gridObject.POrderId + "," + gridObject.SupplierId + "," + gridObject.CostCenterId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete && IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return PurchaseOrderDelete('" + gridObject.POType + "'," + gridObject.POrderId + ",'" + gridObject.ApprovedStatus + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return PurchaseOrderApproval('" + gridObject.POType + "','" + 'Checked' + "'," + gridObject.POrderId + "," + gridObject.SupplierId + "," + gridObject.CostCenterId + ")\" alt='Check'  title='Check' border='0' />";
                }

                if (gridObject.IsCanApproved && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return PurchaseOrderApproval('" + gridObject.POType + "','" + 'Approved' + "', " + gridObject.POrderId + "," + gridObject.SupplierId + "," + gridObject.CostCenterId + ")\" alt='Approve'  title='Approve' border='0' />";
                }
                if (gridObject.IsCanPOReOpen && $("#ContentPlaceHolder1_hfIsAdminUser").val() == "1") {
                    tr += "&nbsp;&nbsp;<img src='../Images/reOpen.png' style='width:18px;height:18px' onClick= \"javascript:return PurchaseOrderReOpen(" + gridObject.POrderId + ",'" + gridObject.PONumber + "')\" alt='Re Open'  title='Re Open' border='0' />";
                }

                if (gridObject.CurrencyType == "Local") {
                    tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.LocalCurrencyId + "','" + gridObject.POType + "'," + gridObject.POrderId + ",'" + gridObject.ApprovedStatus + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Invoice' title='Purchase Order' border='0' />";
                }
                else {
                    tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.LocalCurrencyId + "','" + gridObject.POType + "'," + gridObject.POrderId + ",'" + gridObject.ApprovedStatus + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Invoice' title='Purchase Order (" + gridObject.LocalCurrencyName + ")' border='0' />";
                    tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.CurrencyId + "','" + gridObject.POType + "'," + gridObject.POrderId + ",'" + gridObject.ApprovedStatus + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Invoice' title='Purchase Order (" + gridObject.CurrencyName + ")' border='0' />";
                }

                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.POrderId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.SupplierId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.CostCenterId + "</td>";

                tr += "</tr>";

                $("#PurchaseOrderGrid tbody").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }
        function PurchaseOrderReOpen(POrderId, PONumber) {
            if (!confirm("Do you Want To Re-Open " + PONumber + "?")) {
                return false;
            }
            PageMethods.ReOpenPurchaseOrder(POrderId, OnReOpenPurchaseOrderSucceed, OnReOpenPurchaseOrderFailed);
            return false;
        }
        function OnReOpenPurchaseOrderSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //LoadRequisition();
                SearchPurchaseOrder($("#GridPagingContainer").find("li.active").index(), 1);
            }
        }
        function OnReOpenPurchaseOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }
        function OnSearchPurchaseOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }
        function ClearSearch() {
            $("#ContentPlaceHolder1_txtSPONumber").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("All");
            $("#ContentPlaceHolder1_ddlPOType").val("All");
            $("#ContentPlaceHolder1_ddlCostCenterSearch").val("0");
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");
            $("#ContentPlaceHolder1_ddlSearchSupplier").val("0").trigger("change");
        }

        function PurchaseOrderEdit(POType, POrderId, SupplierId, CostCenterId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            PageMethods.EditPurchaseOrder(POrderId, OnEditPurchaseOrderSucceed, OnEditPurchaseOrderFailed);
            return false;
        }
        function OnEditPurchaseOrderSucceed(result) {
            if (result.PurchaseOrder.POType == "AdHoc") {
                $("#AdhoqPurchase").show();
                $("#AdhoqPurchaseItem").show();
                $("#RequisitionItemContainer").hide();
                $("#RequisitionWisePurchaseContainer").hide();
                $("#RequisitionCostCenterContainer").hide();
                $("#RequisitionOrderContainer").hide();

                $("#ItemForPurchase tbody").html("");
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlGLCompanyId").attr("disabled", true);

                AdhocPurchaseOrderEdit(result);
            }
            else {

                $("#AdhoqPurchase").hide();
                $("#AdhoqPurchaseItem").hide();
                $("#RequisitionItemContainer").show();
                $("#RequisitionWisePurchaseContainer").show();
                $("#RequisitionCostCenterContainer").show();
                $("#RequisitionOrderContainer").show();
                $("#RequisitionItemForPurchase tbody").html("");
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlGLCompanyId").attr("disabled", false);

                RequisitionWisePurchaseOrderEdit(result);
            }
            LoadTermsAndConditionsForEdit(result.PurchaseOrder.TermsNConditions)
        }
        function AdhocPurchaseOrderEdit(result) {

            LoadForEditPurchaseOrder(result);
            LoadSupplierInfoByOrderType(result.PurchaseOrder.OrderType);
            $("#ContentPlaceHolder1_ddlOrderType").val(result.PurchaseOrder.OrderType);
            $("#ContentPlaceHolder1_ddlSupplier").val(result.PurchaseOrder.SupplierId).trigger('change');
            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
            var tr = "";

            $.each(result.PurchaseOrderDetails, function (count, obj) {

                tr += "<tr>";

                tr += "<td style='width:20%;'>" + obj.ItemName + "</td>";

                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {

                    tr += "<td style='width:7%;'>" + obj.ColorText + "</td>";
                    tr += "<td style='width:7%;'>" + obj.SizeText + "</td>";
                    tr += "<td style='width:7%;'>" + obj.StyleText + "</td>";
                }
                else {
                    tr += "<td style='display:none'>" + obj.ColorText + "</td>";
                    tr += "<td style='display:none'>" + obj.SizeText + "</td>";
                    tr += "<td style='display:none'>" + obj.StyleText + "</td>";
                }
                tr += "<td style='display:none'>" + obj.ColorId + "</td>";
                tr += "<td style='display:none'>" + obj.SizeId + "</td>";
                tr += "<td style='display:none'>" + obj.StyleId + "</td>";

                if (obj.LastPurchaseDate != null)
                    tr += "<td style='width:7%;'>" + CommonHelper.DateFromDateTimeToDisplay(obj.LastPurchaseDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:7%;'></td>";

                tr += "<td style='width:7%;'>" + obj.LastPurchasePrice + "</td>";

                tr += "<td style='width:7%;'>" +
                    "<input type='text' value='" + obj.PurchasePrice + "' id='pq" + obj.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                    "</td>";
                tr += "<td style='width:7%;'>" +
                    "<input type='text' value='" + obj.Quantity + "' id='pp" + obj.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                    "</td>";

                tr += "<td style='width:7%;'>" + obj.StockBy + "</td>";
                tr += "<td style='width:7%;'>" + (obj.Quantity * obj.PurchasePrice) + "</td>";

                tr += "<td style='width:10%;'>" +
                    "<input type='text' value='" + obj.Remarks + "' id='rm" + obj.ItemId + "' class='form-control' />" +
                    "</td>";
                tr += "<td style='width:7%;'>" +
                    "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>" +
                    "</td>";
                tr += "<td style='display:none;'>" + obj.ItemId + "</td>";
                tr += "<td style='display:none;'>" + obj.CategoryId + "</td>";
                tr += "<td style='display:none;'>" + obj.StockById + "</td>";

                tr += "<td style='display:none;'>" + obj.PurchasePrice + "</td>";
                tr += "<td style='display:none;'>" + obj.Quantity + "</td>";
                tr += "<td style='display:none;'>" + obj.DetailId + "</td>";

                tr += "</tr>";

                $("#ItemForPurchase tbody").append(tr);
                tr = "";
            });

            PurchaseOrderItem = result.PurchaseOrderDetails;

            CommonHelper.ApplyDecimalValidation();
            ClearAfterAdhoqPurchaseItemAdded();

            $("#myTabs").tabs({ active: 0 });
        }

        function RequisitionWisePurchaseOrderEdit(result) {
            LoadForEditPurchaseOrder(result);
            LoadSupplierInfoByOrderType(result.PurchaseOrderDetailsSummary[0].OrderType);
            $("#ContentPlaceHolder1_ddlOrderType").val(result.PurchaseOrderDetailsSummary[0].OrderType);
            $("#ContentPlaceHolder1_ddlSupplier").val(result.PurchaseOrderDetailsSummary[0].SupplierId);
            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
            var tr = "";

            $.each(result.PurchaseOrderDetailsSummary, function (count, obj) {
                tr = "<tr>";
                tr += "<td style='width:20%;'>" + obj.ItemName + "</td>";

                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    tr += "<td style='width:7%;'>" + obj.ColorText + "</td>";
                    tr += "<td style='width:7%;'>" + obj.SizeText + "</td>";
                    tr += "<td style='width:7%;'>" + obj.StyleText + "</td>";
                }
                else {
                    tr += "<td style='display:none'>" + obj.ColorText + "</td>";
                    tr += "<td style='display:none'>" + obj.SizeText + "</td>";
                    tr += "<td style='display:none'>" + obj.StyleText + "</td>";
                }

                tr += "<td style='display:none;'>" + obj.ColorId + "</td>";
                tr += "<td style='display:none;'>" + obj.SizeId + "</td>";
                tr += "<td style='display:none;'>" + obj.StyleId + "</td>";

                if (obj.LastPurchaseDate != null)
                    tr += "<td style='width:7%;'>" + CommonHelper.DateFromDateTimeToDisplay(obj.LastPurchaseDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:7%;'></td>";

                tr += "<td style='width:7%;'>" + obj.LastPurchasePrice + "</td>";

                tr += "<td style='width:7%;'>" +
                    "<input type='text' value='" + obj.PurchasePrice + "' id='pq" + obj.ItemId + "' class='form-control quantitydecimal' onblur='CheckRequisitionPrice(this)' />" +
                    "</td>";
                tr += "<td style='width:7%;'>" +
                    "<input type='text' disabled='disabled' value='" + obj.Quantity + "' id='pp" + obj.ItemId + "' class='form-control quantitydecimal' />" +
                    "</td>";

                tr += "<td style='width:7%;'>" + obj.StockBy + "</td>";
                tr += "<td style='width:7%;'>" + (obj.PurchasePrice * obj.Quantity) + "</td>";

                tr += "<td style='width:10%;'>" +
                    "<input type='text' value='" + obj.Remarks + "' id='rm" + obj.ItemId + "' class='form-control' />" +
                    "</td>";

                tr += "<td style='width:7%;'>" +
                    "<a onclick=\"javascript:return EditPurchaseRequisitionItem(this);\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>" +
                    "&nbsp;" +
                    "<a href='javascript:void()' onclick= 'DeletePurchaseRequisitionItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>" +
                    "</td>";

                tr += "<td style='display:none;'>" + obj.ItemId + "</td>";
                tr += "<td style='display:none;'>" + obj.StockById + "</td>";

                tr += "</tr>";

                $("#RequisitionItemForPurchase tbody").append(tr);
                tr = "";
            });

            PurchaseOrderItem = result.PurchaseOrderDetailsSummary;
            PurchaseOrderItemFromRequisition = result.PurchaseOrderDetails;

            CommonHelper.ApplyDecimalValidation();
            ClearAfterAdhoqPurchaseItemAdded();

            $("#myTabs").tabs({ active: 0 });
        }

        function LoadForEditPurchaseOrder(result) {
            triggerOrNot = 1;
            $("#ContentPlaceHolder1_hfPOrderId").val(result.PurchaseOrder.POrderId);
            //$("#ContentPlaceHolder1_ddlGLCompanyId").val(result.PurchaseOrder.CompanyId + '').trigger('change');
            $("#ContentPlaceHolder1_ddlGLCompanyId").val(result.PurchaseOrder.CompanyId);
            $("#ContentPlaceHolder1_ddlPurchaseOrderType").val(result.PurchaseOrder.POType);
            $("#ContentPlaceHolder1_ddlSupplier").val(result.PurchaseOrder.SupplierId + '').trigger('change');
            $("#ContentPlaceHolder1_ddlCostCentre").val(result.PurchaseOrder.CostCenterId + '').trigger('change');

            $("#ContentPlaceHolder1_ddlCurrency").val(result.PurchaseOrder.CurrencyId + '');
            if (result.PurchaseOrder.ConvertionRate != 0)
                $("#ContentPlaceHolder1_txtConversionRate").val(result.PurchaseOrder.ConvertionRate);
            else
                $("#ContentPlaceHolder1_txtConversionRate").val('');

            if (result.PurchaseOrder.ReceivedByDate != null)
                $("#ContentPlaceHolder1_txtExpectedReceiveDate").val(CommonHelper.DateFromDateTimeToDisplay(result.PurchaseOrder.ReceivedByDate, innBoarDateFormat));

            if (result.PurchaseOrder.PODescription != null)
                $("#ContentPlaceHolder1_txtDescription").val(result.PurchaseOrder.PODescription);

            if (result.PurchaseOrder.Remarks != null)
                $("#ContentPlaceHolder1_txtRemarks").val(result.PurchaseOrder.Remarks);

            if (result.PurchaseOrder.DeliveryAddress != null)
                $("#ContentPlaceHolder1_txtDeliveryAddress").val(result.PurchaseOrder.DeliveryAddress);

            if (IsCanEdit) {
                $('#btnSave').show();
            } else {
                $('#btnSave').hide();
            }
            $("#btnSave").val("Update");
        }

        function OnEditPurchaseOrderFailed() {
        }

        function PurchaseOrderDelete(POType, POrderId, ApprovedStatus, SupplierId, CostCenterId, CreatedBy) {

            if (!confirm("Do You Want To Delete.")) { return false; }

            PageMethods.PurchaseOrderDelete(POType, POrderId, ApprovedStatus, CreatedBy, OnApprovalSucceed, OnApprovalFailed);
        }

        function PurchaseOrderApproval(POType, ApprovedStatus, POrderId, SupplierId, CostCenterId) {
            if (!confirm('Do you want to ' + (ApprovedStatus == 'Checked' ? 'check' : 'approve') + " ?")) {
                return false;
            }
            PageMethods.PurchaseOrderApproval(POType, POrderId, ApprovedStatus, OnApprovalSucceed, OnApprovalFailed);
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                    data: JSON.stringify({ tableName: 'PMPurchaseOrder', primaryKeyName: 'POrderId', primaryKeyValue: result.PrimaryKeyValue, featuresValue: 'Purchase', statusColumnName: 'ApprovedStatus' }),
                    dataType: "json",
                    success: function (data) {
                        debugger;

                        SendSMSToUserList(data.d, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);

                    },
                    error: function (result) {
                        toastr.error("Can not load Check or Approve By List.");
                    }
                });

                LoadRequisition();
                SearchPurchaseOrder($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function SendSMSToUserList(UserList, PrimaryKeyValue, TransactionNo, TransactionType, TransactionStatus) {
            debugger;

            var str = '';
            if (TransactionStatus == 'Approved') {
                str += TransactionType + ' No.(' + TransactionNo + ')  is Approved.';
            }
            else if (TransactionStatus == 'Cancel') {
                str += TransactionType + ' No.(' + TransactionNo + ')  is Canceled.';
            }
            else {
                str += TransactionType + ' No.(' + TransactionNo + ') is waiting for your Approval Process.';
            }
            var CommonMessage = {
                Subjects: str,
                MessageBody: str
            };

            var messageDetails = [];
            if (UserList.length > 0) {

                for (var i = 0; i < UserList.length; i++) {
                    messageDetails.push({
                        MessageTo: UserList[i]
                    });
                }

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/HMCommon/frmCommonMessage.aspx/SendMailByID',
                    data: JSON.stringify({ CMB: CommonMessage, CMD: messageDetails }),
                    dataType: "json",
                    success: function (data) {

                        // CommonHelper.AlertMessage(data.d.AlertMessage);

                    },
                    error: function (result) {
                        //alert("Error");

                    }
                });

            }

            return false;
        }
        function OnApprovalFailed() {

        }

        function ShowReport(CurrencyId, POType, POrderId, ApprovedStatus, SupplierId, CostCenterId, CreatedBy) {
            var iframeid = 'printDoc';
            var url = "Reports/frmReportPurchaseOrderInvoice.aspx?POrderId=" + POrderId + "&SupId=" + SupplierId + "&CurrencyId=" + CurrencyId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Purchase Order",
                show: 'slide'
            });
        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfPOrderId").val("0");
            $("#ItemForPurchase tbody").html("");
            $("#RequisitionItemForPurchase tbody").html("");
            $("#ContentPlaceHolder1_ddlRequisitionOrder").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlSupplier").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlOrderType").val("0");
            $("#ContentPlaceHolder1_ddlCostCentre").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlCategory").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_lblCurrentStock").text("");
            $("#ContentPlaceHolder1_lblCurrentStockBy").text("");
            $("#ContentPlaceHolder1_txtPurchaseQuantity").val("");
            $("#ContentPlaceHolder1_txtPurchasePrice").val("");
            $("#ContentPlaceHolder1_txtItemWiseRemarks").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_txtDescription").val("");
            $("#ContentPlaceHolder1_txtDeliveryAddress").val($("#ContentPlaceHolder1_hfDeliveryAddress").val());
            $("#ContentPlaceHolder1_txtExpectedReceiveDate").val("");
            $("#ContentPlaceHolder1_ddlRequisitionCostcenter").val("-1");
            
            $("#ContentPlaceHolder1_ddlOrderType").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlGLCompanyId").attr("disabled", false);

            RequsitionOrderItem = new Array();
            ItemSelected = null;
            PurchaseOrderItem = new Array();
            PurchaseOrderItemFromRequisition = new Array();
            RequisitionWiseDeletedItem = new Array();
            PurchaseOrderItemDeleted = new Array();

            queryPurchaseOrderId = "";

            $("#btnSave").val("Save");
            triggerOrNot = 0;
            EditedTermsNConditions = new Array();
            deletedTermsNConditions = new Array();
            $("#TblTNC tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
            $("#chkAll").prop("checked", false);
            LoadTearmsNConditions();
            $("#form1")[0].reset();
        }
        function PerformClearActionWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function LoadRequisition() {
            PageMethods.LoadRequisition(OnLoadPOSucceeded, OnLoadPOFailed);
        }
        function OnLoadPOSucceeded(result) {

            var control = $('#ContentPlaceHolder1_ddlRequisitionOrder');

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

        function CheckAllOrder() {
            if ($("#OrderCheck").is(":checked")) {
                $("#RequisitionItem tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#RequisitionItem tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchPurchaseOrder(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadSupplierInfoByOrderType(supplierTypeId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/PurchaseOrder.aspx/LoadSupplierInfoByOrderType',
                data: "{'supplierTypeId':'" + supplierTypeId.trim() + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    CommonHelper.SpinnerOpen();
                    OnLoadSupplier(data);
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            //PageMethods.LoadSupplierInfoByOrderType(supplierTypeId, OnLoadSupplier, OnLoadSupplierFailed);
            return false;
        }
        function OnLoadSupplier(result) {
            var control = $('#ContentPlaceHolder1_ddlSupplier');
            CommonHelper.SpinnerClose();
            control.empty();
            if (result.d != null) {
                if (result.d.length > 0) {

                    control.empty().append('<option value="0">---Please Select---</option>');
                    for (i = 0; i < result.d.length; i++) {
                        control.append('<option title="' + result.d[i].Name + '" value="' + result.d[i].SupplierId + '">' + result.d[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }

            return false;
        }
        function OnLoadSupplierFailed(error) {

        }
        function LoadTearmsNConditions() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/PurchaseOrder.aspx/LoadTearmsNConditions',
                dataType: "json",
                success: function (data) {
                    LoadTermsAndConditionsTable(data);
                },
                error: function (result) {

                }
            });
            return false;
        }
        function LoadTermsAndConditionsTable(data) {
            if (data.d.length > 0) {
                $("#TNCDiv").show();
            }
            else {
                $("#TNCDiv").hide();
            }
            $("#TblTNC tbody").empty();
            i = 0;

            $.each(data.d, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }
                tr += "<td style='width:5%;' align='center'><input style='width:100%;' id='" + gridObject.FieldId + "' type='checkbox'> </td>";
                tr += "<td style='width:5%;' align='left'><input class='form-control quantity'  style='width:100%;' align='left' value='" + gridObject.DisplaySequence + "' type='text' > </td>";
                tr += "<td style='width:15%;' align='left'><input  style='width:100%;' align='left' value='" + gridObject.Title + "' type='text' > </td>";
                tr += "<td style='width:75%;' align='left'><input  style='width:100%; align='left' type='text' TextMode='MultiLine' Rows='4' value='" + gridObject.Description + "'> </td>";
                tr += "<td style='display:none'>" + gridObject.Id + "</td>";
                tr += "<td style='display:none'>0</td>";
                tr += "<td style='display:none'> 0 </td>";
                tr += "</tr>";

                $("#TblTNC tbody").append(tr);

                tr = "";
                i++;
            });
            CommonHelper.ApplyIntigerValidation();
            return false;
        }
        function LoadTermsAndConditionsForEdit(result) {
            EditedTermsNConditions = new Array();
            $("#TblTNC tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
            //$.each(result, function (count, obj) {
            for (var i = 0; i < result.length; i++) {
                EditedTermsNConditions.push(result[i]);
                $("#TblTNC tbody tr").each(function () {
                    if (parseFloat($(this).find("td:eq(4)").text()) == result[i].TermsNConditionsId) {
                        $.trim($(this).find("td:eq(0)").find("input").prop("checked", true));
                        $.trim($(this).find("td:eq(1)").find("input").val(result[i].DisplaySequence));
                        $.trim($(this).find("td:eq(2)").find("input").val(result[i].Title));
                        $.trim($(this).find("td:eq(3)").find("input").val(result[i].Description));
                        $.trim($(this).find("td:eq(5)").text(result[i].Id));

                    }
                });
            }
            //});
        }

    </script>
    <asp:HiddenField ID="hfCurrencyObj" runat="server" Value="" />
    <asp:HiddenField ID="hfRequsitionOrderObj" runat="server" Value="" />
    <asp:HiddenField ID="hfIsItemDescriptionSuggestInPurchaseOrder" runat="server" Value="0" />
    <asp:HiddenField ID="hfDefaultCurrencyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCategoryId" runat="server" Value="0" />



    <asp:HiddenField ID="hfIsItemAttributeEnable" runat="server" Value="0"></asp:HiddenField>

    <asp:HiddenField ID="hfStockById" runat="server" Value="0" />
    <asp:HiddenField ID="hfPOrderId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeliveryAddress" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsAdminUser" runat="server" Value="0" />
    <div id="RequisitionWiseItemDialog" style="display: none;">
        <div class="panel panel-default">
            <div class="panel-body" style="padding: 4px;">
                <table id="requisitionWiseItemTable" class="table table-bordered table-hover table-condensed">
                    <thead>
                        <tr>
                            <th style="width: 10%;">Requisition Number</th>
                            <th style="width: 25%;">Item Name</th>

                            <th id="cIdrit" style="width: 7%;">Color
                            </th>
                            <th id="sIdrit" style="width: 7%;">Size
                            </th>
                            <th id="stIdrit" style="width: 7%;">Style
                            </th>


                            <th style="width: 10%;">Max Order Quantity</th>
                            <th style="width: 10%;">Purchase Price</th>
                            <th style="width: 10%;">Quantity</th>
                            <th style="width: 10%;">Unit Head</th>
                            <th style="width: 5%;">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div class="panel-footer">
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" class="btn btn-primary" style="padding-left: 30px; padding-right: 30px;" value="Ok" onclick="ApplyRequisitionWiseItemPurcaseEdit()" />
                        <input type="button" class="btn btn-primary" value="Cancel" onclick="CancelRequisitionWiseItemPurcaseEdit()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Order Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Order</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-heading">Purchase Order Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" id="CompanyDiv" style="display: none;">
                            <div class="col-md-2">
                                <asp:HiddenField ID="hfIsSingleGLCompany" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label17" runat="server" class="control-label required-field" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlGLCompanyId" runat="server" CssClass="form-control"
                                    TabIndex="7">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Order Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList runat="server" ID="ddlOrderType" CssClass="form-control">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Local" Value="Local"></asp:ListItem>
                                    <asp:ListItem Text="Foreign" Value="Foreign"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSupplier" runat="server" class="control-label required-field" Text="Supplier"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Purchase Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPurchaseOrderType" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div id="RequisitionOrderContainer">
                                <div class="col-md-2">
                                    <asp:Label ID="Label15" runat="server" class="control-label required-field" Text="Requisition Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlRequisitionOrder" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="RequisitionCostCenterContainer">
                            <div class="col-md-2">
                                <asp:Label ID="Label13" runat="server" class="control-label" Text="Requisition From"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlRequisitionCostcenter" disabled="disabled" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label16" runat="server" class="control-label required-field" Text="Store From"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCostCentre" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <label for="" class="control-label required-field col-md-2">
                                    Currency</label>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ddlCurrency" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6 col-padding-left-none" id="convertionRateContainer">
                                        <div class="input-group">
                                            <span class="input-group-addon">C.Rate</span>
                                            <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">Item Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div id="RequisitionItemContainer" style="overflow-y: scroll;">
                            <table id="RequisitionItem" class="table table-bordered table-condensed table-hover">
                                <thead>
                                    <tr>
                                        <th style="width: 5%; text-align: center;">Select
                                            <input type="checkbox" value="" checked="checked" id="OrderCheck" onclick="CheckAllOrder()" />
                                        </th>
                                        <th style="width: 11%;">Item Name</th>
                                        <th id="cIdr" style="width: 7%;">Color
                                        </th>
                                        <th id="sIdr" style="width: 7%;">Size
                                        </th>
                                        <th id="stIdr" style="width: 7%;">Style
                                        </th>
                                        <th style="width: 7%;">Last PO Date</th>
                                        <th style="width: 7%;">Last PO Rate</th>
                                        <th style="width: 7%;">Requisition Quantity</th>
                                        <th style="width: 7%;">Approved Quantity</th>
                                        <th style="width: 7%;">Max Order Quantity</th>
                                        <th style="width: 7%;">Unit Head</th>
                                        <th style="width: 7%;">Purchase Quantity</th>
                                        <th style="width: 7%;">Purchase Price</th>
                                        <th style="width: 7%;">Remarks</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                        <div id="AdhoqPurchase">
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
                                    <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Item"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtItem" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div id="AttributeDiv" style="display: none;">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label18" runat="server" class="control-label" Text="Color"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlColorAttribute" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label19" runat="server" class="control-label" Text="Size"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSizeAttribute" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label20" runat="server" class="control-label" Text="Style"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlStyleAttribute" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label5" runat="server" class="control-label" Text="Current Stock"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblCurrentStock" runat="server" class="form-control" Text="0"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label6" runat="server" class="control-label" Text="Stock Unit"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:Label ID="lblCurrentStockBy" runat="server" class="form-control" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Quantity"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPurchaseQuantity" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label8" runat="server" class="control-label required-field" Text="Price"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPurchasePrice" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label11" runat="server" class="control-label" Text="Remarks"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtItemWiseRemarks" runat="server" MaxLength="150" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="form-group" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItemForPurchase()" />
                                <input id="btnCancelOrder" type="button" value="Cancel" onclick="ClearAfterAdhoqPurchaseItemAdded()"
                                    class="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                        <div id="AdhoqPurchaseItem" style="overflow-y: scroll;">
                            <table id="ItemForPurchase" class="table table-bordered table-condensed table-hover">
                                <thead>
                                    <tr>
                                        <th style="width: 20%;">Item Name</th>

                                        <th id="cId" style="width: 7%;">Color
                                        </th>
                                        <th id="sId" style="width: 7%;">Size
                                        </th>
                                        <th id="stId" style="width: 7%;">Style
                                        </th>
                                        <th style="width: 7%;">Last PO Date</th>
                                        <th style="width: 7%;">Last PO Rate</th>
                                        <th style="width: 7%;">Unit Price</th>
                                        <th style="width: 7%;">Unit</th>
                                        <th style="width: 7%;">Unit Head</th>
                                        <th style="width: 7%;">Total</th>
                                        <th style="width: 10%;">Remarks</th>
                                        <th style="width: 7%;">Action</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                                <tfoot id="tableFoot" style="display: none">
                                    <tr>
                                        <td id="tableFootLabel" style="text-align: right" colspan="9">Grand Total: 
                                        </td>
                                        <td id="grandTd" colspan="3"></td>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                        <div id="RequisitionWisePurchaseContainer" style="overflow-y: scroll;">
                            <table id="RequisitionItemForPurchase" class="table table-bordered table-condensed table-hover">
                                <thead>
                                    <tr>
                                        <th style="width: 20%;">Item Name</th>
                                        <th id="cIdrp" style="width: 7%;">Color
                                        </th>
                                        <th id="sIdrp" style="width: 7%;">Size
                                        </th>
                                        <th id="stIdrp" style="width: 7%;">Style
                                        </th>
                                        <th style="width: 7%;">Last PO Date</th>
                                        <th style="width: 7%;">Last PO Rate</th>
                                        <th style="width: 7%;">Unit Price</th>
                                        <th style="width: 7%;">Unit</th>
                                        <th style="width: 7%;">Unit Head</th>
                                        <th style="width: 7%;">Total</th>
                                        <th style="width: 10%;">Remarks</th>
                                        <th style="width: 7%;">Action</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-horizontal">
                <div class="panel panel-default" id="TNCDiv">
                    <div class="panel-heading">Terms & Conditions</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="form-group" id="TNCTableContainer">
                                    <div style="height: auto; padding-left: 15px; overflow-y: auto; width: 98%">
                                        <table class="table table-bordered table-condensed table-responsive" id="TblTNC"
                                            style="width: 100%;">
                                            <thead>
                                                <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                                    <th style="width: 5%; text-align: center; align-content: center">
                                                        <input id='chkAll' type='checkbox'>
                                                    </th>
                                                    <th style="width: 5%; text-align: left">Display Sequence
                                                    </th>
                                                    <th style="width: 15%; text-align: left">Title
                                                    </th>
                                                    <th style="width: 75%; text-align: left">Details
                                                    </th>
                                                    <th style="display: none">TNCId
                                                    </th>
                                                    <th style="display: none">Id
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
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label21" runat="server" class="control-label" Text="Description"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Delivery Address"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDeliveryAddress" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label9" runat="server" class="control-label required-field"
                            Text="Expected Receive Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtExpectedReceiveDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row" style="padding-top: 10px;">
                    <div class="col-md-12">
                        <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-large" value="Save" onclick="SavePurchaseOrder()" />
                        <input type="button" id="btnClear" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="PerformClearActionWithConfirmation()" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Purchase Order Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" id="SrcCompanyDiv" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="Label22" runat="server" class="control-label" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSrcGLCompanyId" runat="server" CssClass="form-control"
                                    TabIndex="7">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label14" runat="server" class="control-label" Text="Supplier"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchSupplier" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSPONumber" runat="server" class="control-label" Text="PO Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSPONumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Text="--- All ---" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                    <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                    <asp:ListItem Text="Cancel" Value="Cancel"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label10" runat="server" class="control-label" Text="PO Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPOType" CssClass="form-control" runat="server">
                                    <asp:ListItem Text="--- All ---" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="AdHoc" Value="AdHoc"></asp:ListItem>
                                    <asp:ListItem Text="Order From Requisition" Value="Requisition"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label12" runat="server" class="control-label" Text="Store Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCostCenterSearch" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList runat="server" ID="ddlSrcOrderType" CssClass="form-control">
                                    <asp:ListItem Text="--- All ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Local" Value="Local"></asp:ListItem>
                                    <asp:ListItem Text="Foreign" Value="Foreign"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchPurchaseOrder(1, 1)" />
                                <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="ClearSearch()" />
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
                    <table id="PurchaseOrderGrid" class="table table-bordered table-condensed table-responsive" width="100%">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 8%;">PO Number
                                </th>
                                <th style="width: 8%;">Purchase Type
                                </th>
                                <th style="width: 5%;">Order Date
                                </th>
                                <th style="width: 10%;">Store Name
                                </th>
                                <th style="width: 10%;">Supplier
                                </th>
                                <th style="width: 8%;">Status
                                </th>
                                <th style="width: 10%;">Receive Status
                                </th>
                                <th style="width: 20%;">Desscription
                                </th>
                                <th style="width: 8%;">PO Amount
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
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#<%=hfIsSingleGLCompany.ClientID %>").val() == "1") {
                $('#CompanyDiv').hide();
                $('#SrcCompanyDiv').hide();
            }
            else {
                $('#CompanyDiv').show();
                $('#SrcCompanyDiv').show();
            }
        });
    </script>
</asp:Content>
