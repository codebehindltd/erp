<%@ Page Title="" Language="C#" MasterPageFile="~/SalesAndMarketing/RestaurantSalesOrderMM.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmSalesOrder.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmSalesOrder" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var tt = {};
        var lastFocused = null;
        var ItemDetails = null;
        var AddedItemList = new Array();
        var EditedItemList = new Array();
        var DeletedItemList = new Array();
        var PreviousBillItemList = new Array();

        var AddedSerialCount = 0;
        var AddedSerialzableProduct = new Array();
        var DeletedSerialzableProduct = new Array();
        var NewAddedSerial = new Array();

        var previousTotal;
        var previousPoint = 0.0;
        var DiscountDetails = new Array();
        var IsMembershipPaymentEnableFlag = false;

        var discountedAmount = 0;

        $(document).ready(function () {
            if ($("#ContentPlaceHolder1_hfBillId").val() != '' && $("#ContentPlaceHolder1_hfBillIdControl").val() != '1') {
                $('#btnPrintPreview').trigger('click');
            }
            $('#txtEstimatedTaskDoneDate').keypress(function (event) {
                event.preventDefault();
                return false;
            });

            $("#ContentPlaceHolder1_ddlInclusiveOrExclusive").change(function () {
                SalesNDiscountCalculation();
                PaymentCalculation(0);
            });

            $("#ContentPlaceHolder1_ddlDeliveredBy").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlProject").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#txtSerialAutoComplete").autocomplete({
                minLength: 1,
                source: function (request, response) {
                    var costcenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();
                    var projectId = $("#ContentPlaceHolder1_ddlProject").val();
                    var itemId = $("#ContentPlaceHolder1_hfItemIdForSerial").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/frmSalesOrder.aspx/SerialSearch',
                        data: JSON.stringify({ serialNumber: request.term, costcenterId: costcenterId, projectId: projectId, itemId: itemId }),
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

            if ($("#ContentPlaceHolder1_hfIsItemCodeHideForBilling").val() == '1') {
                document.getElementById("codeCol").style.display = "none";
                document.getElementById("codeARMCol").style.display = "none";
                document.getElementById("itemCodeInputCol").style.display = "none";
                document.getElementById("itemCodeInputNameCol").style.display = "none";
            }
            if ($("#ContentPlaceHolder1_hfIsStockHideForBilling").val() == '1') {
                document.getElementById("stockCol").style.display = "none";
                document.getElementById("stockARMCol").style.display = "none";
            }
            if ($("#ContentPlaceHolder1_hfIsStockByHideForBilling").val() == '1') {
                document.getElementById("stockByCol").style.display = "none";
                document.getElementById("stockByARMCol").style.display = "none";
            }
            if ($("#ContentPlaceHolder1_hfIsRemarksHideForBilling").val() == '1') {
                document.getElementById("remarksCol").style.display = "none";
            }

            if ($("#ContentPlaceHolder1_hfIsTaskAutoGenarate").val() == '1') {
                $("#txtEstimatedTaskDoneDate").show();
                $("#ContentPlaceHolder1_lblEstimatedTaskDoneDate").show();
            }
            else {
                $("#txtEstimatedTaskDoneDate").hide();
                $("#ContentPlaceHolder1_lblEstimatedTaskDoneDate").hide();
            }

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == '0') {
                document.getElementById("cIdd").style.display = "none";
                document.getElementById("sIdd").style.display = "none";
                document.getElementById("stIdd").style.display = "none";
            }

            //if ($("#ContentPlaceHolder1_hfIsCashPaymentShow").val() == "1") {
            //    $("#cashDiv").show();
            //}
            //else {
            //    $("#cashDiv").hide();
            //}

            if ($("#ContentPlaceHolder1_hfIsAmexCardPaymentShow").val() == "1") {
                $("#amexCardDiv").show();
            }
            else {
                $("#amexCardDiv").hide();
            }
            if ($("#ContentPlaceHolder1_hfIsMasterCardPaymentShow").val() == "1") {
                $("#masterCardDiv").show();
            }
            else {
                $("#masterCardDiv").hide();
            }
            if ($("#ContentPlaceHolder1_hfIsVisaCardPaymentShow").val() == "1") {
                $("#visaCardDiv").show();
            }
            else {
                $("#visaCardDiv").hide();
            }
            if ($("#ContentPlaceHolder1_hfIsDiscoverCardPaymentShow").val() == "1") {
                $("#discoverCardDiv").show();
            }
            else {
                $("#discoverCardDiv").hide();
            }
            if ($("#ContentPlaceHolder1_hfIsRestaurantIntegrateWithCompany").val() == "1" && $("#ContentPlaceHolder1_hfIsCompanyPaymentShow").val() == "1") {
                $("#CompanyPaymentContainer").show();
            }
            else {
                $("#CompanyPaymentContainer").hide();
            }

            if ($("#ContentPlaceHolder1_hfIsSubjectShow").val() == "1") {
                $("#ContentPlaceHolder1_txtSubject").show();
            }
            else {
                $("#ContentPlaceHolder1_txtSubject").hide();
            }
            if ($("#ContentPlaceHolder1_hfIsRemarkShow").val() == "1") {
                $("#ContentPlaceHolder1_txtRemarks").show();
            }
            else {
                $("#ContentPlaceHolder1_txtRemarks").hide();
            }

            if ($("#ContentPlaceHolder1_hfIsCustomerDetailsEnable").val() == "1") {
                $("#CustomerDetailsDiv").show();
            }
            else {
                $("#CustomerDetailsDiv").hide();
            }

            if ($("#ContentPlaceHolder1_hfIsDeliveredByEnable").val() == "1") {
                $("#DeliveredByDiv").show();
            }
            else {
                $("#DeliveredByDiv").hide();
            }

            $('#txtEstimatedTaskDoneDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: 0,
                dateFormat: innBoarDateFormat,
                minDate: 0
            }).datepicker("setDate", 0);

            $('#PointWiseMoney').val('0');
            $('#PointsInAmounts').val('0');
            $('#CustomerCode').val("");
            $('#CustomerPoints').val('0');


            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
            }
            else {
                document.getElementById("itemColorInputNameCol").style.display = "none";
                document.getElementById("itemSizeInputNameCol").style.display = "none";
                document.getElementById("itemStyleInputNameCol").style.display = "none";
                document.getElementById("itemColorInputCol").style.display = "none";
                document.getElementById("itemSizeInputCol").style.display = "none";
                document.getElementById("itemStyleInputCol").style.display = "none";
            }

            IsMembershipPaymentEnableFlag = ($("#ContentPlaceHolder1_hfIsMembershipPaymentEnable").val() == "1") ? true : false;

            if (!IsMembershipPaymentEnableFlag) {
                $("#RetaurantTableTopCustomer").hide();
            }
            else {
                $("#RetaurantTableTopCustomer").show();
            }

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");

                setTimeout(function () {
                    window.location = "/Login.aspx";
                }, 2000);
            }

            var costcenter = $("#ContentPlaceHolder1_hfCostcenterId").val();
            //var costcenter = 11;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/frmSalesOrder.aspx/GetAllDiscount',
                data: "{ 'costcenter':'" + costcenter + "'}",
                dataType: "json",
                success: function (data) {
                    DiscountDetails = data.d;

                    //$("#txtDiscountAmount").val(data.d.Discount);

                },
                error: function (result) {
                    //alert("Error");
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

            $("#PointsInAmounts").change(function () {


                var Amount = parseFloat($('#PointsInAmounts').val());
                var point = parseFloat($('#PointWiseMoney').val());

                var code = ($('#CustomerCode').val());
                $('#txtPointAmount').val("");
                $('#PointsInAmounts').val("");

                (previousPoint != 0) ? $('#txtPointAmount').val(previousPoint) : $('#txtPointAmount').val("");
                if (code == "")
                    toastr.info("Please input the customer code.")
                else if (Amount > point) {
                    toastr.info("Amount can not be grater than Point.")
                }
                else if (Amount == 0) {
                    toastr.info("Adjust amount can not be zero.")
                }
                else if (Amount < 0) {
                    toastr.info("Adjust amount can not be negative.")
                }
                else {
                    if ($("#ContentPlaceHolder1_hfIsBillExchange").val() == "1") {
                        Amount = Amount + previousPoint;
                    }

                    $('#txtPointAmount').val(Amount);

                    SalesNDiscountCalculation();
                    PaymentCalculation(0);
                }

            });

            $("#spendPoint").change(function () {
                if ($(this).is(":checked")) {
                    $("#PointsInAmounts").show();
                    $('#PointsInAmountsDiv').show();
                    previousTotal = $("#txtRoundedGrandTotal").val();
                }
                else {
                    $("#PointsInAmounts").val('');
                    $('#txtPointAmount').val('');


                    $("#PointsInAmounts").hide();
                    $('#PointsInAmountsDiv').hide();
                    $("#txtRoundedGrandTotal").val(previousTotal);
                    PaymentCalculation(0);
                }
            });

            $("#ItemCode").focus();

            CommonHelper.ApplyDecimalValidation();
            CommonHelper.ApplyIntigerValidation();

            $("#ItemCode").focus();

            $("#ItemCode").autocomplete({
                source: function (request, response) {
                    var itemName = "";
                    var categoryName = "";
                    var costCenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();
                    if (costCenterId == "") {
                        costCenterId = 0;
                    }

                    if ($.trim(request.term) == "") { return false; }
                                       
                    costCenterId = parseInt(costCenterId);
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../SalesAndMarketing/frmSalesOrder.aspx/GetItemByCodeCategoryNameWiseItemDetailsForAutoSearch",
                        data: "{'itemCode':'" + $.trim(request.term) + "','itemName':'" + itemName + "','categoryName':'" + categoryName + "','costCenterId':'" + costCenterId + "'}",
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Code,
                                    value: m.ItemId,
                                    Name: m.Name,
                                    ItemId: m.ItemId,
                                    Code: m.Code,
                                    UnitHead: m.UnitHead,
                                    UnitPriceLocal: m.UnitPriceLocal,
                                    StockBy: m.StockBy,
                                    CategoryId: m.CategoryId,
                                    DiscountType: m.DiscountType,
                                    DiscountAmount: m.DiscountAmount,
                                    StockQuantity: m.StockQuantity,
                                    IsItemEditable: m.IsItemEditable,
                                    ServiceCharge: m.ServiceCharge,
                                    SDCharge: m.SDCharge,
                                    VatAmount: m.VatAmount,
                                    AdditionalCharge: m.AdditionalCharge,
                                    IsAttributeItem: m.IsAttributeItem,
                                    ProductType: m.ProductType
                                };
                            });

                            if (data.d == "") {
                                //toastr.info("Item Not Found. Please Give Valid Item Name");
                                ItemDetails = null;
                            }

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
                    ItemDetails = ui.item;

                    $("#ItemName").val(ui.item.Name);
                    $("#ContentPlaceHolder1_ddlProductType").val(ui.item.ProductType);

                    if (ui.item.IsAttributeItem) {
                        $("#itemColorInputNameCol").show();
                        $("#itemColorInputCol").show();
                        $("#itemSizeInputNameCol").show();
                        $("#itemSizeInputCol").show();
                        $("#itemStyleInputNameCol").show();
                        $("#itemStyleInputCol").show();
                        GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Color');
                        GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Size');
                        GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Style');
                        GetInvItemStockInfoByItemAndAttributeId();
                        $("#ContentPlaceHolder1_hfIsAttributeItem").val('1');
                    }
                    else {
                        $("#itemColorInputNameCol").hide();
                        $("#itemColorInputCol").hide();
                        $("#itemSizeInputNameCol").hide();
                        $("#itemSizeInputCol").hide();
                        $("#itemStyleInputNameCol").hide();
                        $("#itemStyleInputCol").hide();
                        $("#ContentPlaceHolder1_hfIsAttributeItem").val('0');
                    }
                }
            });

            $("#ItemName").autocomplete({
                source: function (request, response) {

                    var itemCode = "";
                    var categoryName = "";
                    var costCenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();

                    if ($.trim(request.term) == "") { return false; }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/frmSalesOrder.aspx/GetItemByCodeCategoryNameWiseItemDetailsForAutoSearch',
                        data: "{'itemCode':'" + itemCode + "','itemName':'" + $.trim(request.term) + "','categoryName':'" + categoryName + "','costCenterId':'" + costCenterId + "'}",
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    Name: m.Name,
                                    ItemId: m.ItemId,
                                    Code: m.Code,
                                    UnitHead: m.UnitHead,
                                    UnitPriceLocal: m.UnitPriceLocal,
                                    StockBy: m.StockBy,
                                    CategoryId: m.CategoryId,
                                    DiscountType: m.DiscountType,
                                    DiscountAmount: m.DiscountAmount,
                                    StockQuantity: m.StockQuantity,
                                    IsItemEditable: m.IsItemEditable,
                                    ServiceCharge: m.ServiceCharge,
                                    SDCharge: m.SDCharge,
                                    VatAmount: m.VatAmount,
                                    AdditionalCharge: m.AdditionalCharge,
                                    IsAttributeItem: m.IsAttributeItem,
                                    ProductType: m.ProductType
                                };
                            });

                            if (data.d == "") {
                                //toastr.info("Item Not Found. Please Give Valid Item Name");
                                ItemDetails = null;
                            }

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
                    ItemDetails = ui.item;

                    $("#ItemCode").val(ui.item.Code);
                    $("#ContentPlaceHolder1_ddlProductType").val(ui.item.ProductType);

                    if (ui.item.IsAttributeItem) {
                        $("#itemColorInputNameCol").show();
                        $("#itemColorInputCol").show();
                        $("#itemSizeInputNameCol").show();
                        $("#itemSizeInputCol").show();
                        $("#itemStyleInputNameCol").show();
                        $("#itemStyleInputCol").show();
                        GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Color');
                        GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Size');
                        GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Style');
                        GetInvItemStockInfoByItemAndAttributeId();
                        $("#ContentPlaceHolder1_hfIsAttributeItem").val('1');
                    }
                    else {
                        $("#itemColorInputNameCol").hide();
                        $("#itemColorInputCol").hide();
                        $("#itemSizeInputNameCol").hide();
                        $("#itemSizeInputCol").hide();
                        $("#itemStyleInputNameCol").hide();
                        $("#itemStyleInputCol").hide();
                        GetInvItemStockInfoByItemAndAttributeId();
                        $("#ContentPlaceHolder1_hfIsAttributeItem").val('0');
                    }
                }
            });

            $("#ItemCode").keypress(function (event) {
                var keycode = event.keyCode || event.which;
                if (keycode == '13') {
                    if ($("#ContentPlaceHolder1_hfIsAttributeItem").val() == "0") {
                        AddItem();
                    }
                }
            });

            $("#ItemName").keypress(function (event) {
                var keycode = event.keyCode || event.which;
                if (keycode == '13') {
                    if ($("#ContentPlaceHolder1_hfIsAttributeItem").val() == "0") {
                        AddItem();
                    }
                }
            });

            $("#ContentPlaceHolder1_ddlStyleAttribute").keypress(function (event) {
                var keycode = event.keyCode || event.which;
                if (keycode == '13') {
                    if ($("#ContentPlaceHolder1_hfIsAttributeItem").val() == "1") {
                        AddItem();
                    }
                }
            });

            $("#CustomerCode").keypress(function (event) {
                var customerCode = $('#CustomerCode').val();
                var keycode = event.keyCode || event.which;
                if (keycode == '13') {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/frmSalesOrder.aspx/GetPointsByCustomerCode',
                        data: "{'customerCode':'" + customerCode + "'}",
                        dataType: "json",
                        success: function (data) {

                            if (data.d.MemberId == 0) {
                                toastr.info("Customer Code is not valid!");
                                $('#CustomerCode').val('');
                            }
                            else {

                                $('#CustomerPoints').val(data.d.AchievePoint);
                                $('#PointWiseMoney').val(data.d.PointWiseAmount);
                                $("#ContentPlaceHolder1_hfMemberId").val(data.d.MemberId);
                                $("#ContentPlaceHolder1_hfMemberName").val(data.d.MemberName);
                            }

                        },
                        error: function (result) {
                            //alert("Error");

                        }
                    });
                }
            });

            $("#AddedItem").delegate(':input', 'keyup', function (event) {
                var keycode = event.keyCode || event.which;
                if (keycode == '13') {
                    CheckQuantity(this);
                    $("#ItemCode").focus();
                }
            });

            $("#rbFixedDiscount").click(function () {
                if ($(this).is(":checked")) {
                    if ($("#txtDiscountAmount").val() != "")
                        CalculateDiscount();
                }
            });

            $("#rbPercentageDiscount").click(function () {
                if ($(this).is(":checked")) {
                    if ($("#txtDiscountAmount").val() != "")
                        CalculateDiscount();
                }
            });

            $('input[type!=hidden]:enabled, input[type!=hidden]:visible, select:enabled, select:visible, options:selected').on('keypress', function (e) {

                var code = (e.keyCode ? e.keyCode : e.which);
                var currentClassName = "", idName = "";

                if (typeof $(this).attr('class') != 'undefined') {
                    currentClassName = $(this).attr('class').split(' ')[0];
                }

                if (typeof $(this).attr('id') != 'undefined') {
                    idName = $(this).attr('id');
                }

                if (code == 13) {

                    if (currentClassName != "TransactionalButton") {
                        e.preventDefault();

                        if (idName == "txtRoomSearchGlobal") {
                            RoomSearchAndShowGlobal();
                        }

                        var inputs = $(this).closest('form').find(':focusable');
                        inputs.eq(inputs.index(this) + 1).focus();
                    }
                    else {
                        return true;
                    }
                    return false;
                }

            });

        });

        document.onkeyup = function (e) {
            var e = e || window.event;

            //toastr.info(e.keyCode);
            //67 = c, 78 = n, 68 = d, 80 = p

            if (e.altKey && e.which == 67) {
                $("#ItemCode").focus();
                return false;
            }
            else if (e.altKey && e.which == 78) {
                $("#ItemName").focus();
                return false;
            }
            else if (e.altKey && e.which == 80) {
                $("#txtCash").focus();
                return false;
            }
            else if (e.altKey && e.which == 68) {
                var focusedControl = document.activeElement;
                if (focusedControl.id == "txtCash" || focusedControl.id == "txtAmexCard" || focusedControl.id == "txtMasterCard" || focusedControl.id == "txtVisaCard" || focusedControl.id == "txtDiscoverCard") {
                    $(focusedControl).val($("#txtRoundedGrandTotal").val());
                }
                return false;
            }
        }

        function GetInvItemStockInfoByItemAndAttributeId() {
            var itemCode = $("#ItemCode").val();
            //var itemName = $("#ItemName").val();
            var itemName = $("#ItemName").val().replace("'", "");

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
            var itemId = parseInt($("#ContentPlaceHolder1_hfProductId").val(), 10);

            var categoryName = "";
            var costCenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();

            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/frmSalesOrder.aspx/GetItemByCodeColorSizeStyleCategoryNameWiseItemDetailsForAutoSearch',
                data: "{'itemCode':'" + $.trim(itemCode) + "','itemName':'" + $.trim(itemName) + "','colorId':'" + colorId + "','sizeId':'" + sizeId + "','styleId':'" + styleId + "','categoryName':'" + categoryName + "','costCenterId':'" + costCenterId + "'}",
                dataType: "json",
                success: function (data) {
                    tt = data;
                    if (data.d.length == 0) { toastr.info("Item Not Found. Please Give Valid Item Code"); return false; }


                    if (data.d != null) {
                        var str = data.d[0].StockQuantity;
                        $("#ContentPlaceHolder1_lblCurrentStock").text(str);
                    }
                    else {
                        var str = 0;
                        $("#ContentPlaceHolder1_lblCurrentStock").text(str);
                    }


                    ItemDetails = {
                        label: data.d[0].Name,
                        value: data.d[0].ItemId,
                        Name: data.d[0].Name,
                        ItemId: data.d[0].ItemId,
                        Code: data.d[0].Code,
                        ColorId: data.d[0].ColorId,
                        ColorText: data.d[0].ColorText,
                        SizeId: data.d[0].SizeId,
                        SizeText: data.d[0].SizeText,
                        StyleId: data.d[0].StyleId,
                        StyleText: data.d[0].StyleText,
                        UnitHead: data.d[0].UnitHead,
                        UnitPriceLocal: data.d[0].UnitPriceLocal,
                        StockBy: data.d[0].StockBy,
                        CategoryId: data.d[0].CategoryId,
                        DiscountType: data.d[0].DiscountType,
                        DiscountAmount: data.d[0].DiscountAmount,
                        StockQuantity: data.d[0].StockQuantity,
                        IsItemEditable: data.d[0].IsItemEditable,
                        ServiceCharge: data.d[0].ServiceCharge,
                        SDCharge: data.d[0].SDCharge,
                        VatAmount: data.d[0].VatAmount,
                        AdditionalCharge: data.d[0].AdditionalCharge,
                        IsAttributeItem: data.d[0].IsAttributeItem
                    };

                },
                error: function (result) {
                    //alert("Error");
                }
            });

            return false;
        }

        function GetInvItemAttributeByItemIdAndAttributeType(itemId, type) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/frmSalesOrder.aspx/GetInvItemAttributeByItemIdAndAttributeType',
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
        function PerformEdit(id) {
            id = parseInt(id);
            PageMethods.GetBillById(id, OnSuccessLoading, OnFailLoading)
            return false;
        }
        function OnSuccessLoading(result) {
            dataForEditForBillingBillId = result.RestaurantKotBill;
            var str = result;
            debugger;
            var tr = "";
            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                for (var i = 0; i < result.KotBillDetails.length; i++) {
                    tr += "<tr>";

                    if ($("#ContentPlaceHolder1_hfIsItemCodeHideForBilling").val() == '0') {
                        tr += "<td style='width:12%;'>" + result.KotBillDetails[i].ItemCode + "</td>";
                    }
                    else {
                        tr += "<td style='display:none; width:12%;'>" + result.KotBillDetails[i].ItemCode + "</td>";
                    }


                    tr += "<td style='width:20%;'>" + result.KotBillDetails[i].ItemName + "</td>";
                    tr += "<td style='width:5%;'>" + result.KotBillDetails[i].ColorName + "</td>";
                    tr += "<td style='width:5%;'>" + result.KotBillDetails[i].SizeName + "</td>";
                    tr += "<td style='width:5%;'>" + result.KotBillDetails[i].StyleName + "</td>";

                    if ($("#ContentPlaceHolder1_hfIsStockHideForBilling").val() == '0') {
                        tr += "<td style='width:10%;'>" + '' + "</td>";
                    }
                    else {
                        tr += "<td style='display:none; width:10%;'>" + '' + "</td>";
                    }
                    if ($("#ContentPlaceHolder1_hfIsStockByHideForBilling").val() == '0') {
                        tr += "<td style='width:10%;'>" + result.KotBillDetails[i].UnitHead + "</td>";
                    }
                    else {
                        tr += "<td style='display:none; width:10%;'>" + result.KotBillDetails[i].UnitHead + "</td>";
                    }

                    tr += "<td style='width:8%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + result.KotBillDetails[i].ItemUnit + "' onblur='CheckQuantity(this)' />" + "</td>";
                    tr += "<td style='width:12%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + result.KotBillDetails[i].UnitRate + "' onblur='CheckQuantity(this)' />" + "</td>";

                    tr += "<td style='width:12%;' class='text-right'>" + result.KotBillDetails[i].Amount + "</td>";
                    if ($("#ContentPlaceHolder1_hfIsRemarksHideForBilling").val() == '0') {
                        tr += "<td style='width:10%;'>" + "<textarea type='text' style='width:100%;'  class='form-control '  value='" + "' onblur='CheckQuantity(this)' >" + result.KotBillDetails[i].Remarks + "</textarea> </td>";
                    }
                    else {
                        tr += "<td style='display:none; width:10%;'>" + "<textarea type='text' style='width:100%;'  class='form-control ' value='" + + "' onblur='CheckQuantity(this)' >" + result.KotBillDetails[i].Remarks + "</textarea></td>";
                    }


                    tr += "<td style='display:none'>" + result.KotBillDetails[i].ItemId + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].StockBy /*(ItemDetails == null ? 0 : ItemDetails.StockBy)*/ + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].CategoryId + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].KotDetailId + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].ItemUnit + "</td>";
                    tr += "<td style='display:none'>0</td>";//unit discount
                    tr += "<td style='display:none'>0</td>";//total discount
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].ServiceCharge + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].CitySDCharge + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].VatAmount + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].AdditionalCharge + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].ColorId + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].SizeId + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].StyleId + "</td>";


                    tr += "<td style='width:6%;' class='text-center'>"
                    tr += "<a href='javascript:void(0)' onclick= 'DeleteItemOrder(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "</td>";

                    tr += "</td>";
                    tr += "</tr>";

                    AddedItemList.push({
                        KotDetailId: result.KotBillDetails[i].KotDetailId,
                        KotId: result.KotBillDetails[i].KotId,
                        ItemId: result.KotBillDetails[i].ItemId,
                        ItemName: result.KotBillDetails[i].ItemName,
                        Code: result.KotBillDetails[i].ItemCode,
                        UnitHead: result.KotBillDetails[i].UnitHead,
                        UnitPriceLocal: result.KotBillDetails[i].UnitRate,
                        StockBy: result.KotBillDetails[i].StockBy,
                        CategoryId: result.KotBillDetails[i].CategoryId,
                        DiscountType: '',
                        DiscountAmount: 0.0,
                        Remarks: result.KotBillDetails[i].Remarks,
                        ServiceCharge: result.KotBillDetails[i].ServiceCharge,
                        SDCharge: result.KotBillDetails[i].CitySDCharge,
                        VatAmount: result.KotBillDetails[i].VatAmount,
                        AdditionalCharge: result.KotBillDetails[i].AdditionalCharge
                    });

                }

                $("#AddedItem tbody").append(tr);
            }
            else {
                for (var i = 0; i < result.KotBillDetails.length; i++) {
                    tr += "<tr>";

                    if ($("#ContentPlaceHolder1_hfIsItemCodeHideForBilling").val() == '0') {
                        tr += "<td style='width:12%;'>" + result.KotBillDetails[i].ItemCode + "</td>";
                    }
                    else {
                        tr += "<td style='display:none; width:12%;'>" + result.KotBillDetails[i].ItemCode + "</td>";
                    }


                    tr += "<td style='width:20%;'>" + result.KotBillDetails[i].ItemName + "</td>";
                    tr += "<td style='display:none; width:10%;'>" + '' + "</td>";
                    tr += "<td style='display:none; width:10%;'>" + '' + "</td>";
                    tr += "<td style='width:10%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + result.KotBillDetails[i].BagWeight + "' onblur='CheckQuantity(this)' />" + "</td>";
                    tr += "<td style='width:10%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + result.KotBillDetails[i].NoOfBag + "' onblur='CheckQuantity(this)' />" + "</td>";
                    //tr += "<td style='width:10%;'>" + result.KotBillDetails[i].Quantity + "</td>";

                    //if ($("#ContentPlaceHolder1_hfIsStockHideForBilling").val() == '0') {
                    //    tr += "<td style='width:10%;'>" + '' + "</td>";
                    //}
                    //else {
                    //    tr += "<td style='display:none; width:10%;'>" + '' + "</td>";
                    //}
                    //if ($("#ContentPlaceHolder1_hfIsStockByHideForBilling").val() == '0') {
                    //    tr += "<td style='width:10%;'>" + result.KotBillDetails[i].UnitHead + "</td>";
                    //}
                    //else {
                    //    tr += "<td style='display:none; width:10%;'>" + result.KotBillDetails[i].UnitHead + "</td>";
                    //}

                    tr += "<td style='width:8%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + result.KotBillDetails[i].ItemUnit + "' onblur='CheckQuantity(this)' />" + "</td>";
                    tr += "<td style='width:12%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + result.KotBillDetails[i].UnitRate + "' onblur='CheckQuantity(this)' />" + "</td>";

                    tr += "<td style='width:12%;' class='text-right'>" + result.KotBillDetails[i].Amount + "</td>";
                    //if ($("#ContentPlaceHolder1_hfIsRemarksHideForBilling").val() == '0') {
                    //    tr += "<td style='width:10%;'>" + "<textarea type='text' style='width:100%;'  class='form-control '  value='" + "' onblur='CheckQuantity(this)' >" + result.KotBillDetails[i].Remarks + "</textarea> </td>";
                    //}
                    //else {
                    //    tr += "<td style='display:none; width:10%;'>" + "<textarea type='text' style='width:100%;'  class='form-control ' value='" + + "' onblur='CheckQuantity(this)' >" + result.KotBillDetails[i].Remarks + "</textarea></td>";
                    //}


                    tr += "<td style='display:none'>" + result.KotBillDetails[i].ItemId + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].StockBy /*(ItemDetails == null ? 0 : ItemDetails.StockBy)*/ + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].CategoryId + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].KotDetailId + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].ItemUnit + "</td>";
                    tr += "<td style='display:none'>0</td>";//unit discount
                    tr += "<td style='display:none'>0</td>";//total discount
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].ServiceCharge + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].CitySDCharge + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].VatAmount + "</td>";
                    tr += "<td style='display:none'>" + result.KotBillDetails[i].AdditionalCharge + "</td>";


                    tr += "<td style='width:6%;' class='text-center'>"
                    tr += "<a href='javascript:void(0)' onclick= 'DeleteItemOrder(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "</td>";

                    tr += "</td>";
                    tr += "</tr>";

                    AddedItemList.push({
                        KotDetailId: result.KotBillDetails[i].KotDetailId,
                        KotId: result.KotBillDetails[i].KotId,
                        ItemId: result.KotBillDetails[i].ItemId,
                        ItemName: result.KotBillDetails[i].ItemName,
                        Code: result.KotBillDetails[i].ItemCode,
                        UnitHead: result.KotBillDetails[i].UnitHead,
                        UnitPriceLocal: result.KotBillDetails[i].UnitRate,
                        StockBy: result.KotBillDetails[i].StockBy,
                        CategoryId: result.KotBillDetails[i].CategoryId,
                        DiscountType: '',
                        DiscountAmount: 0.0,
                        Remarks: result.KotBillDetails[i].Remarks,
                        ServiceCharge: result.KotBillDetails[i].ServiceCharge,
                        SDCharge: result.KotBillDetails[i].CitySDCharge,
                        VatAmount: result.KotBillDetails[i].VatAmount,
                        AdditionalCharge: result.KotBillDetails[i].AdditionalCharge
                    });
                }
                $("#AddedRiceMillBillingItem tbody").append(tr);
            }
            if (result.RestaurantKotBill.IsInvoiceVatAmountEnable) {
                $("#" + "ContentPlaceHolder1_cbTPVatAmount").prop('checked', true);
            }
            else {
                $("#" + "ContentPlaceHolder1_cbTPVatAmount").prop('checked', false);
            }

            $("#txtTotalSales").val(result.RestaurantKotBill.GrandTotal - result.RestaurantKotBill.VatAmount - result.RestaurantKotBill.DiscountAmount);
            if (result.RestaurantKotBill.DiscountType == "Fixed") {
                $("#rbFixedDiscount").prop("checked", true);
                $("#rbPercentageDiscount").prop("checked", false);
            }
            else if (result.RestaurantKotBill.DiscountType == "Percentage") {
                $("#rbFixedDiscount").prop("checked", false);
                $("#rbPercentageDiscount").prop("checked", true);
            }
            debugger;
            $("#txtDiscountAmount").val(result.RestaurantKotBill.DiscountAmount);
            $("#txtAfterDiscountAmount").val(result.RestaurantKotBill.GrandTotal - result.RestaurantKotBill.VatAmount);
            $("#txtVat").val(result.RestaurantKotBill.VatAmount);
            $("#txtGrandTotal").val(result.RestaurantKotBill.GrandTotal);
            $("#txtRoundedAmount").val(result.RestaurantKotBill.RoundedAmount);
            $("#txtRoundedGrandTotal").val(result.RestaurantKotBill.RoundedGrandTotal);
            $("#ContentPlaceHolder1_ddlProject").val(result.RestaurantKotBill.ProjectId).change();
            $("#ContentPlaceHolder1_ddlInclusiveOrExclusive").val(result.RestaurantKotBill.BillType).change();
            $("#ContentPlaceHolder1_txtRemarks").val(result.RestaurantKotBill.Remarks);
            $("#ContentPlaceHolder1_txtSubject").val(result.RestaurantKotBill.Subject);
            $("#ContentPlaceHolder1_ddlBillingType").val(result.RestaurantKotBill.BillingType);
            $("#ContentPlaceHolder1_hfBillId").val(result.RestaurantKotBill.BillId);
            $("#ContentPlaceHolder1_hfResumedKotId").val(result.KotBillMaster.KotId);
            $("#ContentPlaceHolder1_txtCustomerName").val(result.RestaurantKotBill.CustomerName);
            $("#ContentPlaceHolder1_txtCustomerMobile").val(result.RestaurantKotBill.CustomerMobile);
            $("#ContentPlaceHolder1_txtCustomerAddress").val(result.RestaurantKotBill.CustomerAddress);
            
            $("#ContentPlaceHolder1_hfCompanyId").val(result.RestaurantKotBill.TransactionId);
            $("#lblCompanyName").text(result.RestaurantKotBill.CustomerName);
            $("#ContentPlaceHolder1_hfPaymentId").val(result.RestaurantKotBill.PaymentInstructionId);
            $("#ContentPlaceHolder1_hfContactId").val(result.RestaurantKotBill.ContactId);

            PaymentCalculation(0);

            return false;
        }
        function OnFailLoading(error) {
            toastr.error(error.get_message());
        }

        function AddRemarks() {

            var refundRemarks = $('#ContentPlaceHolder1_txtRemarksForFullRefund').val();

            if (refundRemarks == "") {
                toastr.info("Please input the Remarks.")
                return false;
            }
            var billId;
            if ($("#ContentPlaceHolder1_hfBillId").val() != "") {
                billId = $("#ContentPlaceHolder1_hfBillId").val();
            }

            var companyAmount = $("#txtCompanyPayment").val();
            var companyName = $("#lblCompanyName").text();
            var companyId = $("#ContentPlaceHolder1_hfCompanyId").val() == "" ? 0 : +$("#ContentPlaceHolder1_hfCompanyId").val();

            var costCenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();
            var totalSales = $("#txtTotalSales").val();
            var discountAmount = $("#txtDiscountAmount").val() == "" ? 0.00 : $("#txtDiscountAmount").val();

            var pointAmount = $('#txtPointAmount').val() == '' ? 0 : parseFloat($('#txtPointAmount').val());

            //if ($("#ContentPlaceHolder1_hfIsBillExchange").val() == "1" && pointAmount > 0) {
            //    pointAmount = pointAmount - previousPoint;
            //}
            var discountType;
            if ($("#rbFixedDiscount").is(":checked")) {
                discountType = "Fixed";
            }
            else if ($("#rbPercentageDiscount").is(":checked")) {
                discountType = "Percentage";
            }

            var vat = $("#txtVat").val();
            var grandTotal = $("#txtGrandTotal").val();
            var roundedAmount = $("#txtRoundedAmount").val() == "" ? "0" : $("#txtRoundedAmount").val();
            var roundedGrandTotal = $("#txtRoundedGrandTotal").val();
            var afterDiscountAmount = $("#txtAfterDiscountAmount").val();


            var isBillReSettlement = true;
            var exchangeItemVatAmount = $("#txtReturnItemVatTotal").val();
            var exchangeItemTotal = $("#txtReturnItemTotal").val();
            var referenceBillId = $("#ContentPlaceHolder1_hfBillId").val();
            var calculatedDiscountAmount = 0;
            if (parseFloat(discountAmount) != "0" && parseFloat(discountAmount) != "")
                calculatedDiscountAmount = parseFloat(totalSales) - parseFloat(afterDiscountAmount);

            var IsVatEnable = true;
            if ($("#ContentPlaceHolder1_cbTPVatAmount").is(":checked")) {
                IsVatEnable = true;
            }
            else {
                IsVatEnable = false;
            }

            var RestaurantBill = {

                BillId: billId,
                IsBillSettlement: false,
                SourceName: 'RestaurantToken',
                BillPaidBySourceId: 0,
                CostCenterId: costCenterId,
                PaxQuantity: "1",
                CustomerName: $("#ContentPlaceHolder1_hfMemberId").val(),
                SalesAmount: totalSales,
                DiscountType: discountType,
                DiscountAmount: discountAmount,
                CalculatedDiscountAmount: calculatedDiscountAmount,

                ServiceCharge: 0.00,
                VatAmount: vat,
                CitySDCharge: 0.00,
                AdditionalCharge: 0.00,
                AdditionalChargeType: 'Percentage',

                IsInvoiceServiceChargeEnable: false,
                //IsInvoiceVatAmountEnable: ($("#ContentPlaceHolder1_hfIsVatEnable").val() == "0" ? false : true),
                IsInvoiceVatAmountEnable: IsVatEnable,
                IsInvoiceCitySDChargeEnable: false,
                IsInvoiceAdditionalChargeEnable: false,

                InvoiceServiceRate: grandTotal,
                GrandTotal: grandTotal,
                RoundedAmount: roundedAmount,
                RoundedGrandTotal: roundedGrandTotal,

                IsBillReSettlement: isBillReSettlement,
                ExchangeItemVatAmount: exchangeItemVatAmount,
                ExchangeItemTotal: exchangeItemTotal,
                ReferenceBillId: referenceBillId,
                PointAmount: pointAmount,
                RefundId: 1,
                RefundRemarks: refundRemarks
            };

            var changeAmount = $("#txtDueRChnage").val();

            var BillPayment = {
                NodeId: 0,
                PaymentType: "Refund",
                AccountsPostingHeadId: 0,
                BillPaidBy: 0,
                BankId: 0,
                RegistrationId: 0,
                FieldId: 1,
                ConvertionRate: 1,
                CurrencyAmount: changeAmount,
                PaymentAmount: changeAmount,
                ChecqueDate: new Date(),
                PaymentMode: "Refund",
                PaymentId: 1,
                CardNumber: "",
                CardType: "",
                ExpireDate: null,
                ChecqueNumber: "",
                CardHolderName: "",
                PaymentDescription: ""
            };

            if (companyId != "0") {
                RestaurantBill.TransactionType = 'Company';
                RestaurantBill.TransactionId = companyId;
                RestaurantBill.CustomerName = companyName;
            }
            var MemberId = $("#ContentPlaceHolder1_hfMemberId").val() == '' ? 0 : +$("#ContentPlaceHolder1_hfMemberId").val();

            $.ajax({
                type: "POST",
                url: "frmSalesOrder.aspx/FullBillRefundSettlement",
                data: JSON.stringify({
                    memberId: MemberId,
                    RestaurantBill: RestaurantBill, BillPayment: BillPayment
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d.IsSuccess == true) {


                        CloseRemarks();
                        ClearAll();
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                    else {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                },
                error: function (error) {
                    toastr.error("error");
                }
            });


        }
        function CloseRemarks() {
            $("#FullRefundRemarksDialog").dialog("close");
        }


        function ClearPayment(options) {

            if (options == "Company") {
                $("#ContentPlaceHolder1_hfCompanyId").val("");
                $("#lblCompanyName").text("");

                $("#txtSearchBank").val("");

                $("#ContentPlaceHolder1_hfPaymentId").val("");
                $("#txtBranchName").val("");
                $("#txtAccountName").val("");
                $("#txtAccountNumber").val("");
                $("#txtAccountType").val("");



                $("#ContentPlaceHolder1_hfContactId").val("");
                $("#txtSearchContact").val("");
                $("#txtDesignation").val("");
                $("#txtDepartment").val("");

                $("#txtSearchCompany").val("");
                $("#txtCompanyAddress").val("");
                $("#txtCompanyEmailAddress").val("");
                $("#txtCompanyWebAddress").val("");
                $("#txtBalance").val("");
                $("#txtCompanyPayment").val("");
                if ($("#ContentPlaceHolder1_hfIsRemarkHasDefaultValue").val() == "0") {
                    $("#ContentPlaceHolder1_txtRemarks").val("");
                }
                $("#ContentPlaceHolder1_txtSubject").val("");

                PaymentCalculation(0);
                if ($("#CompanyInfoDialog").is(":visible")) {
                    $("#CompanyInfoDialog").dialog("close");
                }

            }

        }

        function IsVatEnableCheckOrUncheck(ctrl) {
            SalesNDiscountCalculation();
            PaymentCalculation(0);
        }

        function LoadCompanyInfo() {

            var companyName = "";

            $("#CompanyInfoDialog").dialog({
                width: 1200,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Company Search",
                show: 'slide',
                open: function (event, ui) {
                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                }
            });

            $("#txtSearchCompany").autocomplete({
                source: function (request, response) {
                    var costcenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: 'frmSalesOrder.aspx/GetGLCompanyWiseGuestCompanyInfo',
                        data: "{'companyName':'" + request.term.replace("'", "\\'") + "', 'costcenterId':'" + costcenterId + "'}",
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    Balance: m.Balance,
                                    BillingStreet: m.BillingStreet,
                                    ContactNumber: m.ContactNumber,
                                    DiscountPercent: m.DiscountPercent,
                                    Balance: m.Balance,
                                    label: m.CompanyName,
                                    value: m.CompanyId
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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

                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.value);
                    $("#txtCompanyAddress").val(ui.item.BillingStreet);
                    $("#txtCompanyEmailAddress").val(ui.item.EmailAddress);
                    $("#txtCompanyWebAddress").val(ui.item.WebAddress);
                    $("#txtBalance").val(ui.item.Balance);
                }
            });

            $("#txtSearchBank").autocomplete({
                source: function (request, response) {
                    var companyId = $("#ContentPlaceHolder1_hfCompanyId").val() == "" ? 0 : +$("#ContentPlaceHolder1_hfCompanyId").val();
                    if (companyId == 0) {
                        $("#txtSearchBank").val("");
                        toastr.warning("Select A Company First.");
                        $("#txtEstimatedTaskDoneDate").focus();
                        return false;

                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: 'frmSalesOrder.aspx/GetBankInfoForAutoComplete',
                        data: "{'bankName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    AccountName: m.AccountName,
                                    BranchName: m.BranchName,
                                    AccountNumber: m.AccountNumber,
                                    AccountType: m.AccountType,
                                    Description: m.Description,
                                    label: m.BankName,
                                    value: m.BankId
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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

                    $("#ContentPlaceHolder1_hfPaymentId").val(ui.item.value);
                    $("#txtBranchName").val(ui.item.BranchName);
                    $("#txtAccountName").val(ui.item.AccountName);
                    $("#txtAccountNumber").val(ui.item.AccountNumber);
                    $("#txtAccountType").val(ui.item.AccountType);
                }
            });
            $("#txtSearchContact").autocomplete({
                source: function (request, response) {


                    var companyId = $("#ContentPlaceHolder1_hfCompanyId").val() == "" ? 0 : +$("#ContentPlaceHolder1_hfCompanyId").val();
                    if (companyId == 0) {
                        $("#txtSearchContact").val("");
                        toastr.warning("Select A Company First.");
                        $("#txtEstimatedTaskDoneDate").focus();
                        return false;

                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: 'frmSalesOrder.aspx/GetContactInfoForAutoComplete',
                        data: "{'contactName':'" + request.term + "', 'companyId':'" + companyId + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    JobTitle: m.JobTitle,
                                    Department: m.Department,
                                    label: m.Name,
                                    value: m.Id
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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

                    $("#ContentPlaceHolder1_hfContactId").val(ui.item.value);
                    $("#txtDesignation").val(ui.item.JobTitle);
                    $("#txtDepartment").val(ui.item.Department);
                }
            });

        }

        function AdditemByCodeOrBarCode() {
            var itemCode = "", itemName = "", categoryName = "";
            itemCode = $("#ItemCode").val();
            var costCenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();

            if ($.trim(itemCode) == "") { return false; }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/frmSalesOrder.aspx/GetItemByCodeCategoryNameWiseItemDetailsForAutoSearch',
                //data: "{'itemCode':'" + itemCode + "','itemName':'" + itemName + "','categoryName':'" + categoryName + "'}",
                data: "{'itemCode':'" + itemCode + "','itemName':'" + itemName + "','categoryName':'" + categoryName + "','costCenterId':'" + costCenterId + "'}",
                dataType: "json",
                success: function (data) {
                    tt = data;
                    if (data.d.length == 0) { toastr.info("Item Not Found. Please Give Valid Item Code"); return false; }

                    ItemDetails = {
                        label: data.d[0].Name,
                        value: data.d[0].ItemId,
                        Name: data.d[0].Name,
                        ItemId: data.d[0].ItemId,
                        Code: data.d[0].Code,
                        UnitHead: data.d[0].UnitHead,
                        UnitPriceLocal: data.d[0].UnitPriceLocal,
                        StockBy: data.d[0].StockBy,
                        CategoryId: data.d[0].CategoryId,
                        DiscountType: data.d[0].DiscountType,
                        DiscountAmount: data.d[0].DiscountAmount,
                        StockQuantity: data.d[0].StockQuantity,
                        IsItemEditable: data.d[0].IsItemEditable,
                        ServiceCharge: data.d[0].ServiceCharge,
                        SDCharge: data.d[0].SDCharge,
                        VatAmount: data.d[0].VatAmount,
                        AdditionalCharge: data.d[0].AdditionalCharge,
                        IsAttributeItem: data.d[0].IsAttributeItem,
                        ProductType: data.d[0].ProductType
                    };

                    AddItem();
                },
                error: function (result) {
                    //alert("Error");
                }
            });

            return false;
        }

        function AddItem() {
            var alreadyAddedItem, itemId, itemName, colorId = 0, colorText = '', sizeId = 0, sizeText = '', styleId = 0, styleText = '', productType = 'Non Serial Product';

            productType = $("#ContentPlaceHolder1_ddlProductType option:selected").val();

            var IsItemAutoSave = $("#<%=hfIsItemAutoSave.ClientID %>").val();
            if (IsItemAutoSave == '0' && ItemDetails == null) {
                return false;
            }

            if (ItemDetails == null)
                return;

            //if (ItemDetails == null) {
            itemId = 0;
            itemId = ItemDetails == null ? itemId : ItemDetails.ItemId
            itemName = $("#ItemName").val();

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                var colorddlLength = $('#ContentPlaceHolder1_ddlColorAttribute > option').length;
                var sizeddlLength = $('#ContentPlaceHolder1_ddlSizeAttribute > option').length;
                var styleddlLength = $('#ContentPlaceHolder1_ddlStyleAttribute > option').length;

                colorId = 0;
                if (colorddlLength > 0) {
                    colorId = $("#ContentPlaceHolder1_ddlColorAttribute option:selected").val();
                }
                colorText = $("#ContentPlaceHolder1_ddlColorAttribute option:selected").text();

                sizeId = 0;
                if (sizeddlLength > 0) {
                    sizeId = $("#ContentPlaceHolder1_ddlSizeAttribute option:selected").val();
                }
                sizeText = $("#ContentPlaceHolder1_ddlSizeAttribute option:selected").text();

                styleId = 0;
                if (styleddlLength > 0) {
                    styleId = $("#ContentPlaceHolder1_ddlStyleAttribute option:selected").val();
                }
                styleText = $("#ContentPlaceHolder1_ddlStyleAttribute option:selected").text();

                if (colorText != "") {
                    itemName += " (Color: " + colorText;
                }

                if (sizeText != "") {
                    itemName += ", Size: " + sizeText;
                }

                if (styleText != "") {
                    itemName += ", Style: " + styleText + ")";
                }
            }

            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    alreadyAddedItem = _.findWhere(AddedItemList, { ItemId: itemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId, ItemName: itemName });
                }
                else {
                    alreadyAddedItem = _.findWhere(AddedItemList, { ItemId: itemId, ItemName: itemName });
                }
            }

            var tr = "", quantity = 1, totalQuantity = 0.00, itemCode = "", kotId = "0", kotDetailId = "0";
            itemCode = $("#ItemCode").val();

            var index = 0;

            if ($("#ContentPlaceHolder1_hfResumedKotId").val() != "") {
                kotId = $("#ContentPlaceHolder1_hfResumedKotId").val();
            }

            if (alreadyAddedItem != null) {
                if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                    if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                        index = _.findIndex(AddedItemList, { ItemId: itemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId, ItemName: itemName });
                    }
                    else {
                        index = _.findIndex(AddedItemList, { ItemId: itemId, ItemName: itemName });
                    }

                    var trAlreadyAdded = $("#AddedItem tbody tr:eq(" + index + ")");
                    var addedQuantity = 0.00, addedUnitPrice = 0.00, addedTotalPrice = 0.00, totalDiscount = 0.0, unitDiscount = 0.0;

                    addedQuantity = parseFloat($(trAlreadyAdded).find("td:eq(7)").find("input").val()) + quantity;
                    addedUnitPrice = parseFloat($.trim($(trAlreadyAdded).find("td:eq(8)").find("input").val()));
                    addedTotalPrice = toFixed((addedQuantity * addedUnitPrice), 2);
                    unitDiscount = parseFloat($.trim($(trAlreadyAdded).find("td:eq(16)").text()));

                    if (isNaN(unitDiscount)) {
                        unitDiscount = 0;
                    }
                    totalDiscount = toFixed((addedQuantity * unitDiscount), 2);

                    $(trAlreadyAdded).find("td:eq(17)").text(totalDiscount);
                    $(trAlreadyAdded).find("td:eq(7)").find("input").val(addedQuantity);
                    $(trAlreadyAdded).find("td:eq(9)").text(addedTotalPrice);
                }
                else {
                    var trAlreadyAddedItem = $("#AddedRiceMillBillingItem tbody tr:eq(" + index + ")");
                    var addedRiceMillBillingBagWaaightQuantity = 0.00, addedRiceMillBillingBagQuantity = 0.00, addedRiceMillBillingQuantity = 0.00, addedRiceMillBillingUnitPrice = 0.00, addedRiceMillBillingTotalPrice = 0.00, totalRiceMillBillingDiscount = 0.0, unitRiceMillBillingDiscount = 0.0;

                    addedRiceMillBillingBagWaaightQuantity = parseFloat($(trAlreadyAddedItem).find("td:eq(3)").find("input").val()) + quantity;
                    addedRiceMillBillingBagQuantity = parseFloat($(trAlreadyAddedItem).find("td:eq(4)").find("input").val()) + quantity;

                    addedQuantity = (addedRiceMillBillingBagWaaightQuantity * addedRiceMillBillingBagQuantity) + quantity;

                    addedRiceMillBillingUnitPrice = parseFloat($.trim($(trAlreadyAddedItem).find("td:eq(6)").find("input").val()));
                    addedRiceMillBillingTotalPrice = toFixed((addedQuantity * addedUnitPrice), 2);
                    unitRiceMillBillingDiscount = parseFloat(0);

                    if (isNaN(unitDiscount)) {
                        unitDiscount = 0;
                    }
                    totalDiscount = toFixed((addedQuantity * unitDiscount), 2);

                    //$(trAlreadyAddedItem).find("td:eq(17)").text(totalDiscount);
                    //$(trAlreadyAddedItem).find("td:eq(7)").find("input").val(addedQuantity);
                    //$(trAlreadyAddedItem).find("td:eq(9)").text(addedRiceMillBillingTotalPrice);
                }

                ClearItemSearchNAddItemDetails();
                //SalesNDiscountCalculation();
                //PaymentCalculation(0);

                return false;
            }

            if (ItemDetails == null) {
                AddedItemList.push({
                    KotDetailId: kotDetailId,
                    KotId: kotId,
                    ItemId: itemId,
                    ColorId: colorId,
                    SizeId: sizeId,
                    StyleId: styleId,
                    ItemName: itemName,
                    Code: itemCode,
                    ColorId: colorId,
                    ColorText: colorText,
                    SizeId: sizeId,
                    SizeText: sizeText,
                    StyleId: styleId,
                    StyleText: styleText,
                    UnitHead: '',
                    UnitPriceLocal: 0.0,
                    StockBy: 0,
                    CategoryId: 0,
                    DiscountType: '',
                    DiscountAmount: 0.0,
                    Remarks: '',
                    ServiceCharge: 0.0,
                    SDCharge: 0.0,
                    VatAmount: 0.0,
                    AdditionalCharge: 0.0
                });
            }
            else {
                AddedItemList.push({
                    KotDetailId: kotDetailId,
                    KotId: kotId,
                    ItemId: itemId,
                    ColorId: colorId,
                    ColorText: colorText,
                    SizeId: sizeId,
                    SizeText: sizeText,
                    StyleId: styleId,
                    StyleText: styleText,
                    ItemName: itemName,
                    Code: ItemDetails.Code,
                    UnitHead: ItemDetails.UnitHead,
                    UnitPriceLocal: ItemDetails.UnitPriceLocal,
                    StockBy: ItemDetails.StockBy,
                    CategoryId: ItemDetails.CategoryId,
                    DiscountType: ItemDetails.DiscountType,
                    DiscountAmount: ItemDetails.DiscountAmount,
                    Remarks: ItemDetails.Remarks,
                    ServiceCharge: ItemDetails.ServiceCharge,
                    SDCharge: ItemDetails.SDCharge,
                    VatAmount: ItemDetails.VatAmount,
                    AdditionalCharge: ItemDetails.AdditionalCharge
                });
            }
            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                var rowLength = $("#AddedItem tbody tr").length;
                totalQuantity = quantity * (ItemDetails == null ? 0.00 : ItemDetails.UnitPriceLocal);
                tr += "<tr>";

                if ($("#ContentPlaceHolder1_hfIsItemCodeHideForBilling").val() == '0') {
                    tr += "<td style='width:12%;'>" + (ItemDetails == null ? itemCode : ItemDetails.Code) + "</td>";
                }
                else {
                    tr += "<td style='display:none; width:12%;'>" + (ItemDetails == null ? itemCode : ItemDetails.Code) + "</td>";
                }

                tr += "<td style='width:20%;'>" + (ItemDetails == null ? itemName : ItemDetails.label) + "</td>";

                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    tr += "<td style='width:10%;'>" + colorText + "</td>";
                    tr += "<td style='width:10%;'>" + sizeText + "</td>";
                    tr += "<td style='width:10%;'>" + styleText + "</td>";
                }

                if ($("#ContentPlaceHolder1_hfIsStockHideForBilling").val() == '0') {
                    tr += "<td style='width:10%;'>" + (ItemDetails == null ? 0.0 : ItemDetails.StockQuantity) + "</td>";
                }
                else {
                    tr += "<td style='display:none; width:10%;'>" + (ItemDetails == null ? 0.0 : ItemDetails.StockQuantity) + "</td>";
                }

                if ($("#ContentPlaceHolder1_hfIsStockByHideForBilling").val() == '0') {
                    tr += "<td style='width:10%;'>" + (ItemDetails == null ? '' : ItemDetails.UnitHead) + "</td>";
                }
                else {
                    tr += "<td style='display:none; width:10%;'>" + (ItemDetails == null ? '' : ItemDetails.UnitHead) + "</td>";
                }

                tr += "<td style='width:8%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + quantity + "' onblur='CheckQuantity(this)' />" + "</td>";
                tr += "<td style='width:12%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + (ItemDetails == null ? 0.0 : ItemDetails.UnitPriceLocal) + "' onblur='CheckQuantity(this)' />" + "</td>";
                tr += "<td style='width:12%;' class='text-right'>" + totalQuantity + "</td>";

                if ($("#ContentPlaceHolder1_hfIsRemarksHideForBilling").val() == '0') {
                    if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                        tr += "<td style='width:10%;' class='text-right'>" + "<textarea type='text' style='width:100%;'  class='form-control ' sty value='" + "" + "' onblur='CheckQuantity(this)' />" + "</td>";
                    }
                    else {
                        tr += "<td style='width:10%;' class='text-right'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + "" + "' onblur='CheckQuantity(this)' />" + "</td>";
                    }
                }
                else {
                    if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                        tr += "<td style='display:none; width:10%;' class='text-right'>" + "<textarea type='text' style='width:100%;'  class='form-control ' value='" + "" + "' onblur='CheckQuantity(this)' />" + "</td>";
                    }
                    else {
                        tr += "<td style='width:10%;' class='text-right'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + "" + "' onblur='CheckQuantity(this)' />" + "</td>";
                    }
                }

                tr += "<td style='display:none'>" + (ItemDetails == null ? itemId : ItemDetails.ItemId) + "</td>";
                tr += "<td style='display:none'>" + (ItemDetails == null ? 0 : ItemDetails.StockBy) + "</td>";
                tr += "<td style='display:none'>" + (ItemDetails == null ? 0 : ItemDetails.CategoryId) + "</td>";
                tr += "<td style='display:none'>" + kotDetailId + "</td>";
                tr += "<td style='display:none'>0</td>";
                tr += "<td style='display:none'>0</td>";//unit discount
                tr += "<td style='display:none'>0</td>";//total discount
                tr += "<td style='display:none'>" + (ItemDetails == null ? 0.0 : ItemDetails.ServiceCharge) + "</td>";
                tr += "<td style='display:none'>" + (ItemDetails == null ? 0.0 : ItemDetails.SDCharge) + "</td>";
                tr += "<td style='display:none'>" + (ItemDetails == null ? 0.0 : ItemDetails.VatAmount) + "</td>";
                tr += "<td style='display:none'>" + (ItemDetails == null ? 0.0 : ItemDetails.AdditionalCharge) + "</td>";

                tr += "<td style='display:none'>" + colorId + "</td>";
                tr += "<td style='display:none'>" + sizeId + "</td>";
                tr += "<td style='display:none'>" + styleId + "</td>";
                tr += "<td style='width:6%;' class='text-center'>"

                if (productType == "Non Serial Product") {
                    tr += "<a href='javascript:void(0)' onclick= 'DeleteItemOrder(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                }
                else {
                    tr += "<a href='javascript:void(0)' onclick= 'DeleteItemOrder(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForAdHocItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }

                tr += "</td>";
                tr += "</td>";
                tr += "</tr>";

                $("#AddedItem tbody").append(tr);
            }
            else {
                var rowLength = $("#AddedRiceMillBillingItem tbody tr").length;
                totalQuantity = quantity * (ItemDetails == null ? 0.00 : ItemDetails.UnitPriceLocal);
                tr += "<tr>";

                if ($("#ContentPlaceHolder1_hfIsItemCodeHideForBilling").val() == '0') {
                    tr += "<td style='width:12%;'>" + (ItemDetails == null ? itemCode : ItemDetails.Code) + "</td>";
                }
                else {
                    tr += "<td style='display:none; width:12%;'>" + (ItemDetails == null ? itemCode : ItemDetails.Code) + "</td>";
                }

                tr += "<td style='width:20%;'>" + (ItemDetails == null ? itemName : ItemDetails.label) + "</td>";

                if ($("#ContentPlaceHolder1_hfIsStockHideForBilling").val() == '0') {
                    tr += "<td style='width:10%;'>" + (ItemDetails == null ? 0.0 : ItemDetails.StockQuantity) + "</td>";
                }
                else {
                    tr += "<td style='display:none; width:10%;'>" + (ItemDetails == null ? 0.0 : ItemDetails.StockQuantity) + "</td>";
                }

                if ($("#ContentPlaceHolder1_hfIsStockByHideForBilling").val() == '0') {
                    tr += "<td style='width:10%;'>" + (ItemDetails == null ? '' : ItemDetails.UnitHead) + "</td>";
                }
                else {
                    tr += "<td style='display:none; width:10%;'>" + (ItemDetails == null ? '' : ItemDetails.UnitHead) + "</td>";
                }

                var bagWaight = 1, bagQuantity = 1;
                tr += "<td style='width:8%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + bagWaight + "' onblur='CheckQuantity(this)' />" + "</td>";
                tr += "<td style='width:8%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + bagQuantity + "' onblur='CheckQuantity(this)' />" + "</td>";

                tr += "<td style='width:12%;' class='text-right'>" + (bagWaight * bagQuantity) + "</td>";
                tr += "<td style='width:12%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + (ItemDetails == null ? 0.0 : ItemDetails.UnitPriceLocal) + "' onblur='CheckQuantity(this)' />" + "</td>";
                tr += "<td style='width:12%;' class='text-right'>" + totalQuantity + "</td>";

                tr += "<td style='display:none'>" + (ItemDetails == null ? itemId : ItemDetails.ItemId) + "</td>";
                tr += "<td style='display:none'>" + (ItemDetails == null ? 0 : ItemDetails.StockBy) + "</td>";
                tr += "<td style='display:none'>" + (ItemDetails == null ? 0 : ItemDetails.CategoryId) + "</td>";
                tr += "<td style='display:none'>" + kotDetailId + "</td>";

                tr += "<td style='width:6%;' class='text-center'>"

                if (productType == "Non Serial Product") {
                    tr += "<a href='javascript:void(0)' onclick= 'DeleteItemOrder(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                }
                else {
                    tr += "<a href='javascript:void(0)' onclick= 'DeleteItemOrder(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForAdHocItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }

                tr += "</td>";
                tr += "</td>";
                tr += "</tr>";

                $("#AddedRiceMillBillingItem tbody").append(tr);
            }

            CommonHelper.ApplyDecimalValidation();
            IsDiscountApplicable();
            ClearItemSearchNAddItemDetails();
            SalesNDiscountCalculation();
            PaymentCalculation(0);
            return false;
        }

        function IsDiscountApplicable() {
            if (ItemDetails == null) {
                return false;
            }
            var DiscountItem = _.where(DiscountDetails, { DiscountForId: ItemDetails.ItemId, DiscountFor: 'Item' });
            var discountAmount = 0.0;

            if (DiscountItem != null) {
                if (DiscountItem.length > 1) {
                    $.each(DiscountItem, function (index, item) {
                        var discount = 0.0;
                        if (item.DiscountType == 'Percentage') {
                            discount = parseFloat(ItemDetails.UnitPriceLocal) * item.Discount / 100;
                        }
                        else {
                            discount = item.Discount;
                        }

                        if (discountAmount < discount)
                            discountAmount = discount;
                    });
                }
                else if (DiscountItem.length == 1) {
                    var type = DiscountItem[0].DiscountType;

                    var discount = 0.0;
                    if (DiscountItem[0].DiscountType == 'Percentage') {
                        discount = parseFloat(ItemDetails.UnitPriceLocal) * DiscountItem[0].Discount / 100;
                    }
                    else {
                        discount = DiscountItem[0].Discount;
                    }

                    if (discountAmount < discount)
                        discountAmount = discount;
                }
            }

            var DiscountCategory = _.where(DiscountDetails, { DiscountForId: ItemDetails.CategoryId, DiscountFor: 'Category' });

            if (DiscountCategory != null) {
                if (DiscountCategory.length > 1) {
                    $.each(DiscountCategory, function (index, item) {
                        var discount = 0.0;
                        if (item.DiscountType == 'Percentage') {
                            discount = parseFloat(ItemDetails.UnitPriceLocal) * item.Discount / 100;
                        }
                        else {
                            discount = item.Discount;
                        }

                        if (discountAmount < discount)
                            discountAmount = discount;
                    });
                }
                else if (DiscountCategory.length == 1) {
                    var type = DiscountCategory[0].DiscountType;

                    var discount = 0.0;
                    if (DiscountCategory[0].DiscountType == 'Percentage') {
                        discount = parseFloat(ItemDetails.UnitPriceLocal) * DiscountCategory[0].Discount / 100;
                    }
                    else {
                        discount = DiscountCategory[0].Discount;
                    }

                    if (discountAmount < discount)
                        discountAmount = discount;
                }
            }

            index = _.findIndex(AddedItemList, { ItemId: ItemDetails.ItemId });
            var AddedItemDiscount = $("#AddedItem tbody tr:eq(" + index + ")");

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                $(AddedItemDiscount).find("td:eq(16)").text(discountAmount);
                $(AddedItemDiscount).find("td:eq(17)").text(discountAmount);
            }
            else {
                $(AddedItemDiscount).find("td:eq(13)").text(discountAmount);
                $(AddedItemDiscount).find("td:eq(14)").text(discountAmount);
            }
        }

        function calculateTotalQuantity() {
            debugger;
            var totalQuantity = 0.0, totalItem = 0.0;
            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                $("#AddedItem > tbody > tr").each(function () {
                    if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                        totalQuantity = totalQuantity + parseFloat($(this).find("td:eq(7)").find("input").val() == null ? 0 : $(this).find("td:eq(7)").find("input").val());
                    }
                    else {
                        totalQuantity = totalQuantity + parseFloat($(this).find("td:eq(4)").find("input").val() == null ? 0 : $(this).find("td:eq(4)").find("input").val());
                    }
                    totalItem++;
                });
            }
            else {
                $("#AddedRiceMillBillingItem tbody tr").each(function () {
                    debugger;
                    totalQuantity = totalQuantity + parseFloat($(this).find("td:eq(6)").text() == null ? 0 : $(this).find("td:eq(6)").text());
                    if (totalQuantity == 'NaN') {
                        totalQuantity = totalQuantity + parseFloat($(this).find("td:eq(6)").find("input").val() == null ? 0 : $(this).find("td:eq(6)").find("input").val());
                    }
                    totalItem++;
                });
            }

            document.getElementById('ContentPlaceHolder1_lblTotalItemTypes').innerHTML = "Total Item : " + totalItem + "";
            document.getElementById('ContentPlaceHolder1_lblTotalItemQuantity').innerHTML = "Total Quantity : " + totalQuantity + "";
        }

        function SalesNDiscountCalculation() {
            /*if ($("#AddedItem tbody tr").length == 0) {
                toastr.info("Please Add Item First.");
                return false;
            }*/
            calculateTotalQuantity();

            var totalAmount = 0.00, amount = 0.00, index = 0;
            var discountAmount = 0.00, afterDiscountAmount = 0.00;
            var vat = 0.00, ServiceCharge = 0.00, totalDiscountAmount = 0.00, grandTotal = 0.00, exchangeTotal = 0.00;
            var discountType = "Percentage", discount = 0.00;

            var discountType = "Percentage", discount = 0.00, roundedGrandTotal = 0.00;
            discount = $("#txtDiscountAmount").val() == "" ? 0.00 : parseFloat($("#txtDiscountAmount").val());
            roundedGrandTotal = $("#txtRoundedGrandTotal").val() == "" ? 0.00 : parseFloat($("#txtRoundedGrandTotal").val());

            //if ($("#rbPercentageDiscount").is(":checked") == true && discount > 100) {
            //    toastr.info("Discount Percentage Cannot Greater Than (>) 100.");
            //    $("#txtDiscountAmount").val("");
            //    discount = 0.00;
            //}
            //else if ($("#rbFixedDiscount").is(":checked") == true && discount > roundedGrandTotal) {
            //    toastr.info("Fixed Discount Cannot Greater Than Grand Total.");
            //    $("#txtDiscountAmount").val("");
            //    discount = 0.00;
            //}


            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                $("#AddedItem tbody tr").each(function () {
                    if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                        amount = parseFloat($(this).find("td:eq(9)").text());
                    }
                    else {
                        amount = parseFloat($(this).find("td:eq(6)").text());
                    }

                    if (AddedItemList[index].DiscountType == "Fixed") {
                        discountAmount = AddedItemList[index].DiscountAmount;
                    }
                    else if (AddedItemList[index].DiscountType == "Percentage") {
                        discountAmount = amount * (AddedItemList[index].DiscountAmount / 100.00);
                    }
                    totalAmount += amount;
                    totalDiscountAmount += discountAmount;
                    afterDiscountAmount += (amount - discountAmount);
                    index++;
                    discountAmount = 0.00;
                });
            }
            else {
                $("#AddedRiceMillBillingItem tbody tr").each(function () {
                    amount = parseFloat($(this).find("td:eq(8)").text());

                    if (AddedItemList[index].DiscountType == "Fixed") {
                        discountAmount = AddedItemList[index].DiscountAmount;
                    }
                    else if (AddedItemList[index].DiscountType == "Percentage") {
                        discountAmount = amount * (AddedItemList[index].DiscountAmount / 100.00);
                    }

                    totalAmount += amount;
                    totalDiscountAmount += discountAmount;
                    afterDiscountAmount += (amount - discountAmount);
                    index++;
                    discountAmount = 0.00;
                });
            }

            if ($("#rbPercentageDiscount").is(":checked") == true && discount != 0) {
                discountType = "Percentage";
                discountAmount = totalAmount * (discount / 100.00);
            }
            else if ($("#rbFixedDiscount").is(":checked") == true && discount != 0) {
                discountType = "Fixed";
                discountAmount = discount;
            }
            totalDiscountAmount = discountAmount;

            //var MaxItemDiscountTotal = 0.0;

            //if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
            //    $("#AddedItem tbody tr").each(function () {
            //        var itemDiscountAmount = parseFloat($(this).find("td:eq(14)").text());
            //        MaxItemDiscountTotal = MaxItemDiscountTotal + itemDiscountAmount;
            //    });
            //}
            //else {
            //    $("#AddedRiceMillBillingItem tbody tr").each(function () {
            //        var itemDiscountAmount = parseFloat(0);
            //        MaxItemDiscountTotal = MaxItemDiscountTotal + itemDiscountAmount;
            //    });
            //}

            //if (MaxItemDiscountTotal > 0) {
            //    totalDiscountAmount = MaxItemDiscountTotal;
            //}

            afterDiscountAmount = totalAmount - totalDiscountAmount;
            var IsInclusiveOrExclusiveBill = '', IsInclusiveBill = 0, vatAmount = 0.00, IsVatEnable = 1;
            var vatPercentAmount = 0.0, totalItemAmount = 0.0, totalItemDiscount = 0.00, totalAmountAfterDiscount = 0.0;

            IsInclusiveOrExclusiveBill = $("#ContentPlaceHolder1_ddlInclusiveOrExclusive").val();

            if ($("#ContentPlaceHolder1_cbTPVatAmount").is(":checked")) {
                IsVatEnable = true;
            }
            else {
                IsVatEnable = false;
            }

            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                $("#AddedItem tbody tr").each(function () {
                    if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "0") {
                        vatPercentAmount = parseFloat($(this).find("td:eq(17)").text());
                        totalItemAmount = parseFloat($(this).find("td:eq(6)").text());
                        totalItemDiscount = parseFloat($(this).find("td:eq(14)").text());
                    }
                    else {
                        vatPercentAmount = parseFloat($(this).find("td:eq(20)").text());
                        totalItemAmount = parseFloat($(this).find("td:eq(9)").text());
                        totalItemDiscount = parseFloat($(this).find("td:eq(17)").text());
                    }

                    totalAmountAfterDiscount = totalItemAmount - totalItemDiscount;

                    if (IsInclusiveOrExclusiveBill == 'Exclusive') {
                        vat += parseFloat((((totalAmountAfterDiscount) * vatPercentAmount) / (100)) * IsVatEnable);
                        grandTotal = afterDiscountAmount + vat;

                    }
                    else {
                        vat += parseFloat(((totalAmountAfterDiscount * vatPercentAmount) / (100 + vatPercentAmount)) * IsVatEnable);
                        grandTotal = afterDiscountAmount;
                        ServiceRate = parseFloat(discountedAmount - ServiceCharge - vat);
                    }

                });
            }
            else {
                $("#AddedRiceMillBillingItem tbody tr").each(function () {
                    vatPercentAmount = parseFloat(0);
                    totalItemAmount = parseFloat($(this).find("td:eq(8)").text());
                    totalItemDiscount = parseFloat(0);
                    totalAmountAfterDiscount = totalItemAmount - totalItemDiscount;

                    if (IsInclusiveOrExclusiveBill == 'Exclusive') {
                        vat += parseFloat((((totalAmountAfterDiscount) * vatPercentAmount) / (100)) * IsVatEnable);
                        grandTotal = afterDiscountAmount + vat;
                    }
                    else {
                        vat += parseFloat(((totalAmountAfterDiscount * vatPercentAmount) / (100 + vatPercentAmount)) * IsVatEnable);
                        grandTotal = afterDiscountAmount;
                        ServiceRate = parseFloat(discountedAmount - ServiceCharge - vat);
                    }
                });
            }

            $("#txtTotalSales").val(toFixed(totalAmount, 2));
            if ($("#rbPercentageDiscount").is(":checked") == false) {
                $("#txtDiscountAmount").val(toFixed(totalDiscountAmount, 2));
                $("#ContentPlaceHolder1_hfTotalDiscountAmount").val(toFixed(totalDiscountAmount, 2));
            }

            $("#ContentPlaceHolder1_hfTotalDiscountAmount").val(toFixed(totalDiscountAmount, 2));
            $("#txtAfterDiscountAmount").val(toFixed(afterDiscountAmount, 2));
            $("#txtVat").val(toFixed(vat, 2));
            console.log(grandTotal);
            $("#txtGrandTotal").val(toFixed(grandTotal, 2));

            exchangeTotal = parseFloat($("#txtReturnItemTotal").val() == "" ? 0 : ($("#txtReturnItemTotal").val()));
            exchangeTotal += parseFloat($("#txtPointAmount").val() == "" ? 0 : ($("#txtPointAmount").val()));
            grandTotal = grandTotal - exchangeTotal;

            $("#txtRoundedGrandTotal").val(toFixed(grandTotal, 2));

            var isRestaurantBillAmountWillRound = $("#ContentPlaceHolder1_hfIsRestaurantBillAmountWillRound").val();
            var fractionalAmount = 0.00, roundedGrandTotal = toFixed(grandTotal, 2);

            if (isRestaurantBillAmountWillRound == "1") {
                var roundedGrandTotal = Math.round(grandTotal)
                var fractionalAmount = toFixed((roundedGrandTotal - toFixed(grandTotal, 2)), 2);

                if (fractionalAmount > 0) {
                    $("#roundedSign").text("+");
                    $("#txtRoundedAmount").val(fractionalAmount);
                }
                else {
                    $("#roundedSign").text("-");
                    $("#txtRoundedAmount").val((fractionalAmount * -1.00));
                }

                $("#txtRoundedGrandTotal").val(roundedGrandTotal);
            }

            if ($("#ContentPlaceHolder1_hfBillId").val() != "") {
                PaymentCalculation(roundedGrandTotal);
            }
        }

        function CheckQuantity(control) {
            tt = control;
            var tr = $(control).parent().parent();
            debugger;
            var bagWaight = 1;
            var bagQuantity = 1;
            var quantity = 0;
            var unitPrice = 0;

            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    quantity = $.trim($(tr).find("td:eq(7)").find("input").val());
                    unitPrice = $.trim($(tr).find("td:eq(8)").find("input").val());
                }
                else {
                    quantity = $.trim($(tr).find("td:eq(4)").find("input").val());
                    unitPrice = $.trim($(tr).find("td:eq(5)").find("input").val());
                }
            }
            else {
                console.log($.trim($(tr).find("td:eq(0)").find("input").val()));
                console.log($.trim($(tr).find("td:eq(1)").find("input").val()));
                console.log($.trim($(tr).find("td:eq(2)").find("input").val()));
                console.log($.trim($(tr).find("td:eq(3)").find("input").val()));

                bagWaight = $.trim($(tr).find("td:eq(4)").find("input").val());
                bagQuantity = $.trim($(tr).find("td:eq(5)").find("input").val());
                quantity = parseFloat(bagWaight) * parseFloat(bagQuantity);
                unitPrice = $.trim($(tr).find("td:eq(7)").find("input").val());
            }

            if (quantity == "" || quantity == "0") {
                toastr.info("Please Give Proper Quantity.");
                $(control).val("1");
            }

            var totalPrice = parseFloat(unitPrice) * parseFloat(quantity);

            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    $(tr).find("td:eq(9)").text(toFixed(totalPrice, 2));
                    ItemDetails = {
                        ItemId: +$.trim($(tr).find("td:eq(11)").text()),
                        UnitPriceLocal: unitPrice,
                        CategoryId: +$.trim($(tr).find("td:eq(13)").text())
                    };
                }
                else {
                    $(tr).find("td:eq(6)").text(toFixed(totalPrice, 2));
                    ItemDetails = {
                        ItemId: +$.trim($(tr).find("td:eq(8)").text()),
                        UnitPriceLocal: unitPrice,
                        CategoryId: +$.trim($(tr).find("td:eq(10)").text())
                    };
                }
            }
            else {
                $(tr).find("td:eq(6)").text(toFixed(quantity, 2));
                $(tr).find("td:eq(8)").text(toFixed(totalPrice, 2));
                ItemDetails = {
                    ItemId: +$.trim($(tr).find("td:eq(9)").text()),
                    UnitPriceLocal: unitPrice,
                    CategoryId: +$.trim($(tr).find("td:eq(11)").text())
                };
            }

            IsDiscountApplicable();
            ItemDetails = null;

            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    unitDiscount = $.trim($(tr).find("td:eq(16)").text());
                    totalDiscount = parseFloat(unitDiscount) * parseFloat(quantity);
                    $(tr).find("td:eq(17)").text(toFixed(totalDiscount, 2));
                }
                else {
                    unitDiscount = $.trim($(tr).find("td:eq(13)").text());
                    totalDiscount = parseFloat(unitDiscount) * parseFloat(quantity);
                    $(tr).find("td:eq(14)").text(toFixed(totalDiscount, 2));
                }
            }
            else {
                unitDiscount = parseFloat(0);
                totalDiscount = parseFloat(0);
            }

            SalesNDiscountCalculation();
            PaymentCalculation(0);
        }

        function PaymentCalculation(paymentAmount) {
            if (PaymentCheck() == false) { return false; }
            var grandTotal = 0.00, paymentTotal = 0.00, dueOrChangeAmount = 0.00, payment = 0.00,
                exchangeTotal = 0.00;

            $("#PaymentArea input").each(function () {
                payment = $(this).val() == "" ? 0.00 : parseFloat($(this).val());
                paymentTotal = paymentTotal + payment;
            });

            var companyAmount = $.trim($("#txtCompanyPayment").val()) == "" ? "0" : $("#txtCompanyPayment").val();
            grandTotal = parseFloat($("#txtRoundedGrandTotal").val() == "" ? 0 : ($("#txtRoundedGrandTotal").val()));
            if (companyAmount != "0") {
                if ($("#ContentPlaceHolder1_hfCompanyId").val() == "" || $("#ContentPlaceHolder1_hfCompanyId").val() == "0") {
                    $("#txtCompanyPayment").val("");
                    toastr.warning("Select a Company First.");
                    companyAmount = "0";
                    PaymentCalculation(0);
                    return false;
                }
                if (parseFloat(paymentTotal) > parseFloat(grandTotal)) {
                    $("#txtCompanyPayment").val("");
                    toastr.warning("Company Payment Amount Cannot Be Greater Than Due Amount.");
                    companyAmount = "0";
                    PaymentCalculation(0);
                    return false;
                }
            }
            dueOrChangeAmount = grandTotal - paymentTotal;

            if (dueOrChangeAmount < 0) {
                $("#DueRChnageAmount").text("Change Amount");
                dueOrChangeAmount *= -1;
            }
            else {
                $("#DueRChnageAmount").text("Due Amount");
            }

            $("#txtDueRChnage").val(toFixed(dueOrChangeAmount, 2));
        }

        function PaymentCheck() {
            var grandTotal = 0.00, paymentTotal = 0.00, dueOrChangeAmount = 0.00, payment = 0.00,
                exchangeTotal = 0.00, paymentIndex = 1, isPaymentExceded = false, cashPayment = 0.00, cardPayment = 0.00;

            grandTotal = parseFloat($("#txtRoundedGrandTotal").val() == "" ? 0 : ($("#txtRoundedGrandTotal").val()));

            $("#PaymentArea input").each(function () {
                payment = $(this).val() == "" ? 0.00 : parseFloat($(this).val());
                paymentTotal = paymentTotal + payment;

                if (paymentIndex == 1 && payment > 0) {
                    cashPayment = payment
                }
                else if (paymentIndex > 1 && payment > 0) {
                    cardPayment = cardPayment + payment;
                }

                paymentIndex = paymentIndex + 1;
                payment = 0.00;
            });

            paymentIndex = 1;
            if (cashPayment > 0 && cardPayment > 0) {
                if (cardPayment > grandTotal) {

                    $("#PaymentArea input").each(function () {
                        if (paymentIndex > 1) {
                            $(this).val("");
                        }
                        paymentIndex = paymentIndex + 1;
                    });

                    toastr.info("Card Payment Cannot be Greater Than Due Amount/Grand Total.");
                }
            }
            else if (cashPayment == 0 && cardPayment > 0) {
                dueOrChangeAmount = grandTotal - paymentTotal;
            }

            if (dueOrChangeAmount < 0) {
                $(this).val("");
                isPaymentExceded = true;
            }

            if (isPaymentExceded) {
                toastr.info("Payment Cannot be Greater Than Grand Total.");
                isPaymentExceded = false;
                return false;
            }

            return true;
        }

        function DueAmountProcess() {
            var dueVal = $.trim($("#DueRChnageAmount").text());

            if (dueVal == 'Due Amount') {
                var dueRChangeAmount = $("#txtDueRChnage").val();

                if (lastFocused != null) {
                    var previousValue = parseFloat($(lastFocused).val() == "" ? 0 : $(lastFocused).val());
                    var dueChangeAmount = parseFloat(dueRChangeAmount == "" ? 0 : dueRChangeAmount);

                    $(lastFocused).val(previousValue + dueChangeAmount);
                    $("#txtDueRChnage").val("0");
                    CalculateDiscount();
                }
            }
        }

        function CalculateDiscount() {
            SalesNDiscountCalculation();
            PaymentCalculation(0);
        }

        function RoundedAmountCalculation(roundedAmount) {
            if (CommonHelper.IsDecimal(roundedAmount) == false) {
                return;
            }

            var grandTotal = 0.00, roundedTotal = 0.00, roundedGrandTotal = 0.00;
            roundedTotal = parseFloat(roundedAmount);
            grandTotal = parseFloat($("#txtGrandTotal").val());
            roundedGrandTotal = grandTotal - roundedTotal;
            $("#txtRoundedGrandTotal").val(roundedGrandTotal);
        }

        function DeleteItemOrder(control) {
            var index = 0;
            var tr = $(control).parent().parent();
            var itemId = 0;

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                itemId = parseInt($.trim($(tr).find("td:eq(11)").text()), 10);
            }
            else {
                itemId = parseInt($.trim($(tr).find("td:eq(8)").text()), 10);
            }

            index = _.findIndex(AddedItemList, { ItemId: itemId });

            if (AddedItemList[index].KotDetailId != "0") {
                DeletedItemList.push(JSON.parse(JSON.stringify(AddedItemList[index])));
            }

            $(tr).remove();
            AddedItemList.splice(index, 1);
            SalesNDiscountCalculation();
            PaymentCalculation(0);

            if (AddedItemList.length == 0) {
                ClearAfterAllItemDelete();
            }
        }
        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }
        function SaveSalesOrder() {
            PaymentCalculation(0);
            if ($("#ContentPlaceHolder1_ddlProject").val() == "0") {
                toastr.warning("Please Select Project.");
                $("#ContentPlaceHolder1_ddlProject").focus();
                return false;
            }
            if ($("#ContentPlaceHolder1_hfBillId").val() != "") {
                billId = $("#ContentPlaceHolder1_hfBillId").val();
            }

            var costCenterId = 0, salesOrderId = 0, projectId = 0, totalSales = 0.0, discountType = "", discountAmount = 0.0, afterDiscountAmount = 0.0, vat = 0.0
            grandTotal = 0.0, roundedAmount = 0.0, roundedGrandTotal = 0.0, customerName = "", customerMobile = "", customerAddress = "", remarks = "", calculatedDiscountAmount = 0.0
            BillDetails = [];

            var parameters = getUrlVars();
            if (parameters.length == 2) {                
                $("#ContentPlaceHolder1_hfIsUpdate").val(1);
                salesOrderId = parameters.salesOrderID;
            }
            costCenterId = parameters.cid;

            projectId = $("#ContentPlaceHolder1_ddlProject").val();
            totalSales = $("#txtTotalSales").val();
            if ($("#rbFixedDiscount").is(":checked")) {
                discountType = "Fixed";
            }
            else if ($("#rbPercentageDiscount").is(":checked")) {
                discountType = "Percentage";
            }
            discountAmount = $("#ContentPlaceHolder1_hfTotalDiscountAmount").val() == "" ? 0.00 : $("#ContentPlaceHolder1_hfTotalDiscountAmount").val();

            afterDiscountAmount = $("#txtAfterDiscountAmount").val();
            vat = $("#txtVat").val();
            grandTotal = $("#txtGrandTotal").val();
            roundedAmount = $("#txtRoundedAmount").val() == "" ? "0" : $("#txtRoundedAmount").val();
            roundedGrandTotal = $("#txtRoundedGrandTotal").val();
            customerName = $("#ContentPlaceHolder1_txtCustomerName").val();
            customerMobile = $("#ContentPlaceHolder1_txtCustomerMobile").val();
            customerAddress = $("#ContentPlaceHolder1_txtCustomerAddress").val();
            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            if (parseFloat(discountAmount) != "0" && parseFloat(discountAmount) != "")
                calculatedDiscountAmount = parseFloat(totalSales) - parseFloat(afterDiscountAmount);

            var companyId = $("#ContentPlaceHolder1_hfCompanyId").val() == "" ? 0 : +$("#ContentPlaceHolder1_hfCompanyId").val();
            var paymentId = $("#ContentPlaceHolder1_hfPaymentId").val() == "" ? 0 : +$("#ContentPlaceHolder1_hfPaymentId").val();
            var contactId = $("#ContentPlaceHolder1_hfContactId").val() == "" ? 0 : +$("#ContentPlaceHolder1_hfContactId").val();
                        
            var RestaurantBill = {
                BillId: salesOrderId,
                CostCenterId: costCenterId,
                ProjectId: projectId,
                SalesAmount: totalSales,
                DiscountType: discountType,
                DiscountAmount: discountAmount,
                VatAmount: vat,
                GrandTotal: grandTotal,
                RoundedAmount: roundedAmount,
                RoundedGrandTotal: roundedGrandTotal,
                CustomerMobile: customerMobile,
                CustomerAddress: customerAddress,
                Remarks: remarks,
                CalculatedDiscountAmount: calculatedDiscountAmount
            };

            if (companyId != "0") {
                RestaurantBill.TransactionType = "Company";
                RestaurantBill.TransactionId = companyId;

                if ($("#ContentPlaceHolder1_hfIsCustomerDetailsEnable").val() == "1") {
                    RestaurantBill.CustomerName = customerName;
                }
                else {
                    RestaurantBill.CustomerName = companyName;
                }

                RestaurantBill.PaymentInstructionId = paymentId;
                RestaurantBill.ContactId = contactId;
            }

            var kotDetailId = 0, itemId = 0, colorId = 0, sizeId = 0, styleId = 0, itemCode, itemName, itemType, stockBy, quantity, unitPrice, totalPrice, unitPrice, discountAmount, remarks,
                BagWeight = 0, NoOfBag = 0;

            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                $("#AddedItem tbody tr").each(function () {

                    if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                        kotDetailId = $(this).find("td:eq(14)").text();
                        itemId = $(this).find("td:eq(11)").text();

                        colorId = $(this).find("td:eq(22)").text();
                        sizeId = $(this).find("td:eq(23)").text();
                        styleId = $(this).find("td:eq(24)").text();

                        itemCode = $(this).find("td:eq(0)").text();
                        itemName = $(this).find("td:eq(1)").text();
                        stockBy = $(this).find("td:eq(12)").text();
                        quantity = $(this).find("td:eq(7)").find("input").val();
                        unitPrice = $(this).find("td:eq(8)").find("input").val();

                        if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '1') {
                            remarks = $(this).find("td:eq(10)").find("input").val();
                        }
                        else {
                            remarks = $(this).find("td:eq(10)").find("textarea").val();
                        }

                        totalPrice = $(this).find("td:eq(9)").text();
                        dbQuantity = $(this).find("td:eq(15)").text();
                        discountAmount = $(this).find("td:eq(17)").text();
                    }
                    else {
                        kotDetailId = $(this).find("td:eq(11)").text();
                        itemId = $(this).find("td:eq(8)").text();

                        colorId = 0;
                        sizeId = 0;
                        styleId = 0;

                        itemCode = $(this).find("td:eq(0)").text();
                        itemName = $(this).find("td:eq(1)").text();
                        stockBy = $(this).find("td:eq(9)").text();
                        quantity = $(this).find("td:eq(4)").find("input").val();
                        unitPrice = $(this).find("td:eq(5)").find("input").val();

                        if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '1') {
                            remarks = $(this).find("td:eq(7)").find("input").val();
                        }
                        else {
                            remarks = $(this).find("td:eq(7)").find("textarea").val();
                        }

                        totalPrice = $(this).find("td:eq(6)").text();
                        dbQuantity = $(this).find("td:eq(12)").text();
                        discountAmount = $(this).find("td:eq(14)").text();
                    }

                    BillDetails.push({
                        KotDetailId: kotDetailId,
                        KotId: salesOrderId,
                        ItemId: itemId,
                        ColorId: colorId,
                        SizeId: sizeId,
                        StyleId: styleId,
                        Code: itemCode,
                        ItemName: itemName,
                        ItemType: 'IndividualItem',
                        StockBy: stockBy,
                        ItemUnit: quantity,
                        UnitRate: unitPrice,
                        Amount: totalPrice,
                        ItemCost: unitPrice,
                        InvoiceDiscount: discountAmount,
                        Remarks: remarks,
                        BagWeight: BagWeight,
                        NoOfBag: NoOfBag
                    });
                });
            }
            else {
                $("#AddedRiceMillBillingItem tbody tr").each(function () {
                    kotDetailId = $(this).find("td:eq(12)").text();
                    itemId = $(this).find("td:eq(9)").text();

                    colorId = 0;
                    sizeId = 0;
                    styleId = 0;

                    itemCode = $(this).find("td:eq(0)").text();
                    itemName = $(this).find("td:eq(1)").text();
                    stockBy = $(this).find("td:eq(10)").text();

                    BagWeight = $(this).find("td:eq(4)").find("input").val();
                    NoOfBag = $(this).find("td:eq(5)").find("input").val();

                    quantity = $(this).find("td:eq(6)").text();
                    if (quantity == "") {
                        quantity = $(this).find("td:eq(6)").find("input").val();
                    }
                    unitPrice = $(this).find("td:eq(7)").find("input").val();
                    remarks = $(this).find("td:eq(4)").find("input").val();

                    totalPrice = $(this).find("td:eq(8)").text();
                    dbQuantity = $(this).find("td:eq(6)").find("input").val();
                    discountAmount = 0;
                    console.log("Bag Weight: " + BagWeight + " No Of Bag: " + NoOfBag + " Quantity: " + quantity);

                    BillDetails.push({
                        KotDetailId: kotDetailId,
                        KotId: salesOrderId,
                        ItemId: itemId,
                        ColorId: colorId,
                        SizeId: sizeId,
                        StyleId: styleId,
                        Code: itemCode,
                        ItemName: itemName,
                        ItemType: 'IndividualItem',
                        StockBy: stockBy,
                        ItemUnit: quantity,
                        UnitRate: unitPrice,
                        Amount: totalPrice,
                        ItemCost: unitPrice,
                        InvoiceDiscount: discountAmount,
                        Remarks: remarks,
                        BagWeight: BagWeight,
                        NoOfBag: NoOfBag
                    });
                });
            }
            console.log(RestaurantBill, BillDetails);
            PageMethods.SaveSalesOrder(RestaurantBill, BillDetails, OnSaveSalesOrderSucceeded, OnSaveSalesOrderFailed);
            return false;
        }
        function OnSaveSalesOrderSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ClearAll();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSaveSalesOrderFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        function BillSettlement() {
            PaymentCalculation(0);

            if ($("#ContentPlaceHolder1_ddlProject").val() == "0") {
                toastr.warning("Please Select Project.");
                $("#ContentPlaceHolder1_ddlProject").focus();
                return false;
            }

            if ($("#txtCompanyPayment").val() != "") { // && $("#txtCompanyPayment").val() != "0"
                if ($("#ContentPlaceHolder1_hfCompanyId").val() == "" || $("#ContentPlaceHolder1_hfCompanyId").val() == "0") {
                    toastr.warning("Please Select Compnay To Give Company Payment.");
                    return false;
                }
            }

            var estimatedDoneDate = $("#txtEstimatedTaskDoneDate").val();
            var isTaskAutoGenarate = $("#ContentPlaceHolder1_hfIsTaskAutoGenarate").val();

            if ($("#ContentPlaceHolder1_hfIsTaskAutoGenarate").val() == '1') {
                if (estimatedDoneDate == "") {
                    toastr.warning("Select Estimated Done Date");
                    $("#txtEstimatedTaskDoneDate").focus();
                    return false;
                }
            }

            if (estimatedDoneDate != "")
                estimatedDoneDate = CommonHelper.DateFormatToMMDDYYYY(estimatedDoneDate, '/');

            isBillExchange = $("#ContentPlaceHolder1_hfIsBillExchange").val() == "0" ? false : true;
            if (!isBillExchange) {
                if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                    if ($("#AddedItem tbody tr").length == 0) { toastr.warning("Please Add Item For Settlement."); return false; }
                }
                else {
                    if ($("#AddedRiceMillBillingItem tbody tr").length == 0) { toastr.warning("Please Add Item For Settlement."); return false; }
                }
            }
            //if ($.trim($("#DueRChnageAmount").text()) == "Due/Change") { toastr.warning("Please Payment For The Bill."); return false; }
            //else if ($.trim($("#DueRChnageAmount").text()) == "Due Amount" && parseFloat($("#txtDueRChnage").val()) != 0) { toastr.warning("Please Payment For The Bill."); return false; }

            var grandTotal = $("#txtGrandTotal").val() == "" ? 0 : parseFloat($("#txtGrandTotal").val());
            var roundedTotal = $("#txtRoundedGrandTotal").val() == "" ? 0 : parseFloat($("#txtRoundedGrandTotal").val());

            if ($("#ContentPlaceHolder1_hfPOSRefundConfiguration").val() == "2") {
                if (roundedTotal < 0 && grandTotal == 0) { toastr.info("Full excahnge amount cannot be refunded."); return false; }
            }
            else if ($("#ContentPlaceHolder1_hfPOSRefundConfiguration").val() == "3") {
                if (roundedTotal < 0) { toastr.info("Excahnge amount cannot be refunded."); return false; }
            }
            var refundId = 1;
            var refundRemarks;
            if (roundedTotal < 0 && grandTotal != 0) {
                refundId = 2;
                refundRemarks = 'Pertial Amount Refund';
            }
            else if (roundedTotal >= 0) {
                refundId = 3;
            }

            if (refundId == 1) {
                $("#FullRefundRemarksDialog").dialog({
                    width: 600,
                    height: 280,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: false,
                    resizable: false,
                    fluid: true,
                    title: "Remarks For Full Refund",
                    show: 'slide',
                    open: function (event, ui) {
                        $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                    }
                });
                //ok te ajax call kore update kore dite hobe.../.....
                var remarks = $('#ContentPlaceHolder1_txtRemarksForFullRefund').val("");
                refundRemarks = remarks;
                return false;
            }

            var isBillExchange = false;
            isBillExchange = $("#ContentPlaceHolder1_hfIsBillExchange").val() == "0" ? false : true;

            if (isBillExchange) {
                var exchangeVal = 0, total = 0.00;
                $("#PreviousBillItem tbody > tr").each(function () {
                    if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                        exchangeVal = $(this).find("td:eq(7)").find('input').val();
                    }
                    else {
                        exchangeVal = $(this).find("td:eq(4)").find('input').val();
                    }

                    total = total + (exchangeVal == "" ? 0.00 : parseFloat(exchangeVal));
                });

                if (total == 0) {
                    toastr.info("Please Give Exchange Quantity.");
                    return false;
                }
            }

            var kotDetailId = "0", kotId = "0", itemId, colorId, sizeId, styleId, itemCode, itemName, stockBy, quantity, unitPrice, remarks,
                totalPrice, totalSales, discountAmount, afterDiscountAmount, vat, grandTotal, roundedAmount,
                roundedGrandTotal, paymodeCount = 0, row = 0, paymentAmount = "0", dbQuantity;

            var billId = "0", defaultCurrency = 1, discountType = 'Percentage', calculatedDiscountAmount = 0,
                isBillReSettlement = false, exchangeItemVatAmount = 0, exchangeItemTotal = 0, referenceBillId = 0,
                costCenterId = 0;

            var PaymentMode = new Array();
            var BillPayment = new Array();
            var BillDetails = new Array();
            var SalesReturnItem = new Array();
            EditedItemList = new Array();

            costCenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();

            if ($("#rbFixedDiscount").is(":checked")) {
                discountType = "Fixed";
            }
            else if ($("#rbPercentageDiscount").is(":checked")) {
                discountType = "Percentage";
            }

            if ($("#ContentPlaceHolder1_hfResumedKotId").val() != "") {
                kotId = $("#ContentPlaceHolder1_hfResumedKotId").val();
            }

            if ($("#ContentPlaceHolder1_hfBillId").val() != "") {
                billId = $("#ContentPlaceHolder1_hfBillId").val();
            }

            var discountAmount = 0.0;
            var companyAmount = $("#txtCompanyPayment").val();
            var companyName = $("#lblCompanyName").text();

            var companyId = $("#ContentPlaceHolder1_hfCompanyId").val() == "" ? 0 : +$("#ContentPlaceHolder1_hfCompanyId").val();
            var paymentId = $("#ContentPlaceHolder1_hfPaymentId").val() == "" ? 0 : +$("#ContentPlaceHolder1_hfPaymentId").val();
            var contactId = $("#ContentPlaceHolder1_hfContactId").val() == "" ? 0 : +$("#ContentPlaceHolder1_hfContactId").val();

            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '0') {
                $("#AddedItem tbody tr").each(function () {

                    if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                        kotDetailId = $(this).find("td:eq(14)").text();
                        itemId = $(this).find("td:eq(11)").text();

                        colorId = $(this).find("td:eq(22)").text();
                        sizeId = $(this).find("td:eq(23)").text();
                        styleId = $(this).find("td:eq(24)").text();

                        itemCode = $(this).find("td:eq(0)").text();
                        itemName = $(this).find("td:eq(1)").text();
                        stockBy = $(this).find("td:eq(12)").text();
                        quantity = $(this).find("td:eq(7)").find("input").val();
                        unitPrice = $(this).find("td:eq(8)").find("input").val();

                        if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '1') {
                            remarks = $(this).find("td:eq(10)").find("input").val();
                        }
                        else {
                            remarks = $(this).find("td:eq(10)").find("textarea").val();
                        }

                        totalPrice = $(this).find("td:eq(9)").text();
                        dbQuantity = $(this).find("td:eq(15)").text();
                        discountAmount = $(this).find("td:eq(17)").text();
                    }
                    else {
                        kotDetailId = $(this).find("td:eq(11)").text();
                        itemId = $(this).find("td:eq(8)").text();

                        colorId = 0;
                        sizeId = 0;
                        styleId = 0;

                        itemCode = $(this).find("td:eq(0)").text();
                        itemName = $(this).find("td:eq(1)").text();
                        stockBy = $(this).find("td:eq(9)").text();
                        quantity = $(this).find("td:eq(4)").find("input").val();
                        unitPrice = $(this).find("td:eq(5)").find("input").val();

                        if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '1') {
                            remarks = $(this).find("td:eq(7)").find("input").val();
                        }
                        else {
                            remarks = $(this).find("td:eq(7)").find("textarea").val();
                        }

                        totalPrice = $(this).find("td:eq(6)").text();
                        dbQuantity = $(this).find("td:eq(12)").text();
                        discountAmount = $(this).find("td:eq(14)").text();
                    }

                    if (kotDetailId == "0") {
                        BillDetails.push({
                            KotDetailId: kotDetailId,
                            KotId: kotId,
                            ItemId: itemId,
                            ColorId: colorId,
                            SizeId: sizeId,
                            StyleId: styleId,
                            Code: itemCode,
                            ItemName: itemName,
                            ItemType: 'IndividualItem',
                            StockBy: stockBy,
                            ItemUnit: quantity,
                            UnitRate: unitPrice,
                            Amount: totalPrice,
                            ItemCost: unitPrice,
                            InvoiceDiscount: discountAmount,
                            Remarks: remarks
                        });
                    }
                    else if (kotDetailId != "0" && quantity != dbQuantity) {
                        EditedItemList.push({
                            KotDetailId: kotDetailId,
                            KotId: kotId,
                            ItemId: itemId,
                            ColorId: colorId,
                            SizeId: sizeId,
                            StyleId: styleId,
                            Code: itemCode,
                            ItemName: itemName,
                            ItemType: 'IndividualItem',
                            StockBy: stockBy,
                            ItemUnit: quantity,
                            UnitRate: unitPrice,
                            Amount: totalPrice,
                            ItemCost: unitPrice,
                            InvoiceDiscount: discountAmount,
                            Remarks: remarks
                        });
                    }
                });
            }
            else {
                $("#AddedRiceMillBillingItem tbody tr").each(function () {
                    kotDetailId = $(this).find("td:eq(12)").text();
                    itemId = $(this).find("td:eq(9)").text();

                    colorId = 0;
                    sizeId = 0;
                    styleId = 0;

                    itemCode = $(this).find("td:eq(0)").text();
                    itemName = $(this).find("td:eq(1)").text();
                    stockBy = $(this).find("td:eq(10)").text();

                    quantity = $(this).find("td:eq(4)").find("input").val();
                    quantity = $(this).find("td:eq(5)").find("input").val();

                    quantity = $(this).find("td:eq(6)").text();
                    unitPrice = $(this).find("td:eq(7)").find("input").val();
                    remarks = $(this).find("td:eq(4)").find("input").val();

                    totalPrice = $(this).find("td:eq(8)").text();
                    dbQuantity = $(this).find("td:eq(6)").find("input").val();
                    discountAmount = 0;

                    if (kotDetailId == "0") {
                        BillDetails.push({
                            KotDetailId: kotDetailId,
                            KotId: kotId,
                            ItemId: itemId,
                            ColorId: colorId,
                            SizeId: sizeId,
                            StyleId: styleId,
                            Code: itemCode,
                            ItemName: itemName,
                            ItemType: 'IndividualItem',
                            StockBy: stockBy,
                            ItemUnit: quantity,
                            UnitRate: unitPrice,
                            Amount: totalPrice,
                            ItemCost: unitPrice,
                            InvoiceDiscount: discountAmount,
                            Remarks: remarks
                        });
                    }
                    else if (kotDetailId != "0" && quantity != dbQuantity) {
                        EditedItemList.push({
                            KotDetailId: kotDetailId,
                            KotId: kotId,
                            ItemId: itemId,
                            ColorId: colorId,
                            SizeId: sizeId,
                            StyleId: styleId,
                            Code: itemCode,
                            ItemName: itemName,
                            ItemType: 'IndividualItem',
                            StockBy: stockBy,
                            ItemUnit: quantity,
                            UnitRate: unitPrice,
                            Amount: totalPrice,
                            ItemCost: unitPrice,
                            InvoiceDiscount: discountAmount,
                            Remarks: remarks
                        });
                    }
                });
            }

            var invoiceDiscount = 0.0;

            if ($("#PreviousBillItem tbody tr").length > 0) {
                var index = 0;
                $("#PreviousBillItem tbody tr").each(function () {

                    kotDetailId = $(this).find("td:eq(9)").text();
                    itemId = $(this).find("td:eq(7)").text();

                    colorId = 0;
                    sizeId = 0;
                    styleId = 0;

                    stockBy = $(this).find("td:eq(8)").text();
                    itemCode = $(this).find("td:eq(0)").text();
                    itemName = $(this).find("td:eq(1)").text();
                    quantity = $(this).find("td:eq(4)").find("input").val();
                    unitPrice = $(this).find("td:eq(5)").text();
                    totalPrice = $(this).find("td:eq(6)").text();
                    dbQuantity = $(this).find("td:eq(10)").text();
                    invoiceDiscount = PreviousBillItemList[index].InvoiceDiscount / PreviousBillItemList[index].Quantity * quantity;

                    if (quantity != "0" && quantity != '') {
                        SalesReturnItem.push({
                            KotDetailId: kotDetailId,
                            KotId: kotId,
                            ItemId: itemId,

                            ColorId: colorId,
                            SizeId: sizeId,
                            StyleId: styleId,

                            Code: itemCode,
                            ItemName: itemName,
                            ItemType: 'IndividualItem',
                            StockBy: stockBy,
                            ItemUnit: quantity,
                            UnitRate: unitPrice,
                            Amount: totalPrice,
                            ItemCost: unitPrice,
                            InvoiceDiscount: invoiceDiscount
                        });
                    }
                    index++;
                });

            }

            totalSales = $("#txtTotalSales").val();
            discountAmount = $("#ContentPlaceHolder1_hfTotalDiscountAmount").val() == "" ? 0.00 : $("#ContentPlaceHolder1_hfTotalDiscountAmount").val();

            afterDiscountAmount = $("#txtAfterDiscountAmount").val();
            vat = $("#txtVat").val();
            grandTotal = $("#txtGrandTotal").val();
            roundedAmount = $("#txtRoundedAmount").val() == "" ? "0" : $("#txtRoundedAmount").val();
            roundedGrandTotal = $("#txtRoundedGrandTotal").val();

            var pointAmount = $('#txtPointAmount').val() == '' ? 0 : parseFloat($('#txtPointAmount').val());

            if ($("#ContentPlaceHolder1_hfIsBillExchange").val() == "1" && pointAmount > 0) {
                pointAmount = pointAmount - previousPoint;
            }

            isBillExchange = $("#ContentPlaceHolder1_hfIsBillExchange").val() == "0" ? false : true;
            if (isBillExchange) {
                isBillReSettlement = true;
                exchangeItemVatAmount = $("#txtReturnItemVatTotal").val();
                exchangeItemTotal = $("#txtReturnItemTotal").val();
                referenceBillId = $("#ContentPlaceHolder1_hfBillId").val();
            }

            var remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            var subject = $("#ContentPlaceHolder1_txtSubject").val();

            if (parseFloat(discountAmount) != "0" && parseFloat(discountAmount) != "")
                calculatedDiscountAmount = parseFloat(totalSales) - parseFloat(afterDiscountAmount);

            var projectId = $("#ContentPlaceHolder1_ddlProject").val();
            var billType = $("#ContentPlaceHolder1_ddlInclusiveOrExclusive").val();

            var IsVatEnable = true;
            if ($("#ContentPlaceHolder1_cbTPVatAmount").is(":checked")) {
                IsVatEnable = true;
            }
            else {
                IsVatEnable = false;
            }

            var customerName = $("#ContentPlaceHolder1_txtCustomerName").val();
            var customerMobile = $("#ContentPlaceHolder1_txtCustomerMobile").val();
            var customerAddress = $("#ContentPlaceHolder1_txtCustomerAddress").val();

            var deliveredBy = 0;
            if ($("#ContentPlaceHolder1_hfIsDeliveredByEnable").val() == "1") {
                deliveredBy = $("#ContentPlaceHolder1_ddlDeliveredBy").val();
            }

            var billingType = "";
            if ($("#ContentPlaceHolder1_hfIsBillingTypeEnable").val() == "1") {

                if ($("#ContentPlaceHolder1_ddlBillingType").val() == "--- Please Select ---") {
                    toastr.warning("Please Select Billing Type.");
                    return false;
                }

                billingType = $("#ContentPlaceHolder1_ddlBillingType").val();
            }

            var RestaurantBill = {
                BillId: billId,
                IsBillSettlement: false,
                SourceName: 'RestaurantToken',
                BillPaidBySourceId: 0,
                CostCenterId: costCenterId,
                PaxQuantity: "1",
                CustomerName: customerName,
                CustomerMobile: customerMobile,
                CustomerAddress: customerAddress,
                SalesAmount: totalSales,
                DiscountType: discountType,
                DiscountAmount: discountAmount,
                CalculatedDiscountAmount: calculatedDiscountAmount,
                Remarks: remarks,
                Subject: subject,
                ServiceCharge: 0.00,
                VatAmount: vat,
                CitySDCharge: 0.00,
                AdditionalCharge: 0.00,
                AdditionalChargeType: 'Percentage',
                IsInvoiceServiceChargeEnable: false,
                IsInvoiceVatAmountEnable: IsVatEnable,
                IsInvoiceCitySDChargeEnable: false,
                IsInvoiceAdditionalChargeEnable: false,

                InvoiceServiceRate: grandTotal,
                GrandTotal: grandTotal,
                RoundedAmount: roundedAmount,
                RoundedGrandTotal: roundedGrandTotal,

                IsBillReSettlement: isBillReSettlement,
                ExchangeItemVatAmount: exchangeItemVatAmount,
                ExchangeItemTotal: exchangeItemTotal,
                ReferenceBillId: referenceBillId,
                PointAmount: pointAmount,
                RefundId: refundId,
                RefundRemarks: refundRemarks,
                ProjectId: projectId,
                BillType: billType,
                DeliveredBy: deliveredBy,
                BillingType: billingType
            };

            if (companyId != "0") {
                RestaurantBill.TransactionType = 'Company';
                RestaurantBill.TransactionId = companyId;

                if ($("#ContentPlaceHolder1_hfIsCustomerDetailsEnable").val() == "1") {
                    RestaurantBill.CustomerName = customerName;
                }
                else {
                    RestaurantBill.CustomerName = companyName;
                }

                RestaurantBill.PaymentInstructionId = paymentId;
                RestaurantBill.ContactId = contactId;
            }

            PaymentMode.push({ CardType: '', PaymentMode: 'Cash', Control: "txtCash" });
            PaymentMode.push({ CardType: 'a', PaymentMode: 'Card', Control: "txtAmexCard" });
            PaymentMode.push({ CardType: 'm', PaymentMode: 'Card', Control: "txtMasterCard" });
            PaymentMode.push({ CardType: 'v', PaymentMode: 'Card', Control: "txtVisaCard" });
            PaymentMode.push({ CardType: 'd', PaymentMode: 'Card', Control: "txtDiscoverCard" });
            PaymentMode.push({ CardType: '', PaymentMode: 'Company', Control: "txtCompanyPayment" });

            paymodeCount = PaymentMode.length, row = 0;
            paymentAmount = "0";
            var paymentCounter = 1;

            for (row = 0; row < paymodeCount; row++) {

                if ($("#" + PaymentMode[row].Control).val() != "" && $("#" + PaymentMode[row].Control).val() != "0") {
                    paymentAmount = $("#" + PaymentMode[row].Control).val();

                    if (paymentAmount == "" || paymentAmount == "0")
                        continue;

                    BillPayment.push({
                        NodeId: 0,
                        PaymentType: "Advance",
                        AccountsPostingHeadId: 0,
                        BillPaidBy: 0,
                        BankId: 0,
                        RegistrationId: 0,
                        FieldId: defaultCurrency,
                        ConvertionRate: 1,
                        CurrencyAmount: paymentAmount,
                        PaymentAmount: paymentAmount,
                        ChecqueDate: new Date(),
                        PaymentMode: PaymentMode[row].PaymentMode,
                        PaymentId: 1,
                        CardNumber: "",
                        CardType: PaymentMode[row].CardType,
                        ExpireDate: null,
                        ChecqueNumber: "",
                        CardHolderName: "",
                        PaymentDescription: "",
                        CompanyId: companyId
                    });
                }
            }

            if ($("#txtRoundedAmount").val() != "0" && $("#txtRoundedAmount").val() != "") {
                roundedAmount = $("#txtRoundedAmount").val();
                var roundedSign = $("#roundedSign").text();
                roundedAmount = roundedSign == "-" ? parseFloat(roundedAmount) * (-1) : parseFloat(roundedAmount);

                if (roundedAmount != "" || roundedAmount != "0") {
                    BillPayment.push({
                        NodeId: 0,
                        PaymentType: "Rounded",
                        AccountsPostingHeadId: 0,
                        BillPaidBy: 0,
                        BankId: 0,
                        RegistrationId: 0,
                        FieldId: defaultCurrency,
                        ConvertionRate: 1,
                        CurrencyAmount: roundedAmount,
                        PaymentAmount: roundedAmount,
                        ChecqueDate: new Date(),
                        PaymentMode: "Rounded",
                        PaymentId: 1,
                        CardNumber: "",
                        CardType: "",
                        ExpireDate: null,
                        ChecqueNumber: "",
                        CardHolderName: "",
                        PaymentDescription: ""
                    });
                }
            }

            if ($.trim($("#DueRChnageAmount").text()) == "Change Amount") {
                if ($("#txtDueRChnage").val() != "0" && $("#txtDueRChnage").val() != "") {

                    var changeAmount = $("#txtDueRChnage").val();

                    if (changeAmount != "" || changeAmount != "0") {

                        BillPayment.push({
                            NodeId: 0,
                            PaymentType: "Refund",
                            AccountsPostingHeadId: 0,
                            BillPaidBy: 0,
                            BankId: 0,
                            RegistrationId: 0,
                            FieldId: defaultCurrency,
                            ConvertionRate: 1,
                            CurrencyAmount: changeAmount,
                            PaymentAmount: changeAmount,
                            ChecqueDate: new Date(),
                            PaymentMode: "Refund",
                            PaymentId: 1,
                            CardNumber: "",
                            CardType: "",
                            ExpireDate: null,
                            ChecqueNumber: "",
                            CardHolderName: "",
                            PaymentDescription: ""
                        });
                    }
                }
            }

            var row = 0;
            rowCount = BillDetails.length;

            for (row = 0; row < rowCount; row++) {
                if (BillDetails[row].ProductType == "Serial Product") {
                    var serialTotal = _.where(AddedSerialzableProduct, { ItemId: BillDetails[row].ItemId });

                    if (BillDetails[row].Quantity > serialTotal.length) {
                        toastr.warning("Please Give Serial Of Product " + BillDetails[row].ItemName);
                        break;
                    }
                    else if (serialTotal.length > BillDetails[row].Quantity) {
                        toastr.warning("Please Remove Serial Of Product " + BillDetails[row].ItemName);
                        break;
                    }
                }
            }

            if (row != rowCount) {
                return false;
            }

            var MemberId = $("#ContentPlaceHolder1_hfMemberId").val() == '' ? 0 : +$("#ContentPlaceHolder1_hfMemberId").val();



            //$.ajax({
            //    type: "POST",
            //    url: "frmSalesOrder.aspx/BillSettlement",
            //    data: JSON.stringify({
            //        kotId: kotId, memberId: MemberId,
            //        RestaurantBill: RestaurantBill, BillPayment: BillPayment, BillDetails: BillDetails,
            //        EditeDetails: EditedItemList, DeletedDetails: DeletedItemList, SalesReturnItem: SalesReturnItem, EstimatedDoneDate: estimatedDoneDate, IsTaskAutoGenarate: isTaskAutoGenarate,
            //        AddedSerialzableProduct: AddedSerialzableProduct, DeletedSerialzableProduct: DeletedSerialzableProduct
            //    }),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (result) {
            //        console.log(result);
            //        if (result.d.IsSuccess == true) {
            //            $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);
            //            CommonHelper.AlertMessage(result.d.AlertMessage);
            //            $("#companyInfo").show();
            //            $("#companyPaymentDiv").removeClass("col-md-12").addClass("col-md-6");

            //            if ($("#ContentPlaceHolder1_hfIsBillExchange").val() == "1") {
            //                $('#btnPrinReturnBillPreview').trigger('click');
            //            }
            //            else {
            //                $('#btnPrintPreview').trigger('click');
            //            }

            //            ClearAll();
            //        }
            //        else {
            //            CommonHelper.AlertMessage(result.d.AlertMessage);
            //        }
            //    },
            //    error: function (error) {
            //        toastr.error("error");
            //    }
            //});
        }

        function CompanyPayment() {
            if ($("#ContentPlaceHolder1_hfCompanyId").val() != "") {
                $("#lblCompanyName").text($("#txtSearchCompany").val());
                $("#ContentPlaceHolder1_txtCustomerName").val($("#txtSearchCompany").val());
            }
            $("#CompanyInfoDialog").dialog('close');
            $("#txtCompanyPayment").focus();
        }

        function BillHoldup() {
            if ($("#AddedItem tbody tr").length == 0) { toastr.info("Please Add Item For Holdup."); return false; }

            var kotDetailId = "0", kotId = "0", itemId, itemCode, itemName, stockBy, quantity, unitPrice, totalPrice, totalSales, discountAmount, afterDiscountAmount,
                vat, grandTotal, roundedAmount, roundedGrandTotal, paymodeCount = 0, row = 0, paymentAmount = "0", dbQuantity;

            var defaultCurrency = 1; //$("#ContentPlaceHolder1_hfLocalCurrencyId").val();
            var PaymentMode = new Array();
            var BillPayment = new Array();
            var BillDetails = new Array();

            if ($("#ContentPlaceHolder1_hfResumedKotId").val() != "") {
                kotId = $("#ContentPlaceHolder1_hfResumedKotId").val();
            }

            $("#AddedItem tbody tr").each(function () {

                kotDetailId = $(this).find("td:eq(11)").text();
                itemId = $(this).find("td:eq(8)").text();
                itemCode = $(this).find("td:eq(0)").text();
                itemName = $(this).find("td:eq(1)").text();
                stockBy = $(this).find("td:eq(9)").text();
                quantity = $(this).find("td:eq(4)").find("input").val();
                unitPrice = $(this).find("td:eq(5)").find("input").val();
                totalPrice = $(this).find("td:eq(6)").text();
                dbQuantity = $(this).find("td:eq(12)").text();
                discountAmount = $(this).find("td:eq(14)").text();

                if (kotDetailId == "0") {
                    BillDetails.push({
                        KotDetailId: kotDetailId,
                        KotId: kotId,
                        ItemId: itemId,
                        Code: itemCode,
                        ItemName: itemName,
                        ItemType: 'IndividualItem',
                        StockBy: stockBy,
                        ItemUnit: quantity,
                        UnitRate: unitPrice,
                        Amount: totalPrice,
                        ItemCost: unitPrice,
                        InvoiceDiscount: discountAmount
                    });
                }
                else if (kotDetailId != "0") {
                    EditedItemList.push({
                        KotDetailId: kotDetailId,
                        KotId: kotId,
                        ItemId: itemId,
                        Code: itemCode,
                        ItemName: itemName,
                        ItemType: 'IndividualItem',
                        StockBy: stockBy,
                        ItemUnit: quantity,
                        UnitRate: unitPrice,
                        Amount: totalPrice,
                        ItemCost: unitPrice,
                        InvoiceDiscount: discountAmount
                    });
                }

            });

            totalSales = $("#txtTotalSales").val();
            discountAmount = $("#txtDiscountAmount").val();
            afterDiscountAmount = $("#txtAfterDiscountAmount").val();
            vat = $("#txtVat").val();
            grandTotal = $("#txtGrandTotal").val();
            roundedAmount = $("#txtRoundedAmount").val();
            roundedGrandTotal = $("#txtRoundedGrandTotal").val();

            $.ajax({
                type: "POST",
                url: "frmSalesOrder.aspx/BillHoldup",
                data: JSON.stringify({
                    kotId: kotId,
                    BillDetails: BillDetails,
                    EditeDetails: EditedItemList,
                    DeletedDetails: DeletedItemList
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d.IsSuccess == true) {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                        ClearAll();
                    }
                    else {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                },
                error: function (error) {
                    toastr.error("error");
                }
            });
        }

        function GetHoldUpPosInfo() {

            $("#PosHoldUpList tbody").html("");

            $.ajax({
                type: "POST",
                url: "frmSalesOrder.aspx/GetHoldUpPosInfo",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d.length > 0) {
                        var row = 0; rowCount = result.d.length;
                        var tr = "";

                        for (row = 0; row < rowCount; row++) {
                            tr = "<tr>"
                            tr += "<td style='width:100%;'> <a href='javascript:void(0)' style='display:block;' onclick=ResumeHoldUpBill(" + result.d[row].KotId + ")>" + result.d[row].KotId + "</a></td>";
                            tr += "</tr>";

                            $("#PosHoldUpList tbody").append(tr);
                            tr = "";
                        }

                        $("#PosHoldUpDialog").dialog({
                            autoOpen: true,
                            modal: true,
                            width: 200,
                            height: 350,
                            closeOnEscape: true,
                            resizable: false,
                            fluid: true,
                            title: "Hold Up Bill",
                            show: 'slide'
                            //close: ClosePrintDialog
                        });

                    }
                },
                error: function (error) {
                    toastr.error("error");
                }
            });
        }

        function ResumeHoldUpBill(kotId) {
            $("#PosHoldUpDialog").dialog("close");
            $("#AddedItem tbody").html("");

            $.ajax({
                type: "POST",
                url: "frmSalesOrder.aspx/GetOrderedItemByKotId",
                data: JSON.stringify({
                    kotId: kotId
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d.length > 0) {

                        $("#ContentPlaceHolder1_hfResumedKotId").val(kotId);
                        $("#headerinfo").text("Resume Id No. : " + kotId);

                        var tr = "";
                        var row = 0, rowLength = result.d.length;

                        for (row = 0; row < rowLength; row++) {

                            AddedItemList.push({

                                KotDetailId: result.d[row].KotDetailId,
                                KotId: kotId,
                                ItemId: result.d[row].ItemId,
                                ItemName: result.d[row].Name,
                                Code: result.d[row].Code,
                                UnitHead: result.d[row].UnitHead,
                                UnitPriceLocal: result.d[row].UnitPriceLocal,
                                StockBy: result.d[row].StockBy,
                                CategoryId: result.d[row].CategoryId,
                                DiscountType: result.d[row].DiscountType,
                                DiscountAmount: result.d[row].DiscountAmount
                            });

                            tr += "<tr>";

                            tr += "<td style='width:12%;'>" + result.d[row].Code + "</td>";
                            tr += "<td style='width:30%;'>" + result.d[row].Name + "</td>";
                            tr += "<td style='width:10%;'>" + result.d[row].StockQuantity + "</td>";
                            tr += "<td style='width:10%;'>" + result.d[row].UnitHead + "</td>";
                            tr += "<td style='width:8%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + result.d[row].Quantity + "' onblur='CheckQuantity(this)' />" + "</td>";
                            tr += "<td style='width:12%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + result.d[row].UnitPriceLocal + "' onblur='CheckQuantity(this)' />" + "</td>";

                            tr += "<td style='width:12%;' class='text-right'>" + (result.d[row].Quantity * result.d[row].UnitPriceLocal) + "</td>";

                            tr += "<td style='width:6%;' class='text-center'>"
                            tr += "<a href='javascript:void(0)' onclick= 'DeleteItemOrder(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                            tr += "</td>";

                            tr += "<td style='display:none'>" + result.d[row].ItemId + "</td>";
                            tr += "<td style='display:none'>" + result.d[row].StockBy + "</td>";
                            tr += "<td style='display:none'>" + result.d[row].CategoryId + "</td>";
                            tr += "<td style='display:none'>" + result.d[row].KotDetailId + "</td>";
                            tr += "<td style='display:none'>0</td>";
                            tr += "<td style='display:none'>" + (result.d[row].InvoiceDiscount / result.d[row].Quantity) + "</td>";
                            tr += "<td style='display:none'>" + result.d[row].InvoiceDiscount + "</td>";

                            tr += "</td>";
                            tr += "</tr>";

                            $("#AddedItem tbody").append(tr);
                            tr = "";
                        }

                        CommonHelper.ApplyDecimalValidation();

                        ClearItemSearchNAddItemDetails();
                        SalesNDiscountCalculation();
                    }
                },
                error: function (error) {
                    toastr.error("error");
                }
            });
        }

        function ClearItemSearchNAddItemDetails() {
            $("#ItemCode").val("");
            $("#ItemName").val("");
            $("#ContentPlaceHolder1_lblCurrentStock").text("00");
            $("#ItemCode").focus();
            ItemDetails = null;

            $("#ContentPlaceHolder1_ddlColorAttribute").empty();
            $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
            $("#ContentPlaceHolder1_ddlStyleAttribute").empty();
        }

        function BillPrint() {
            $("#ContentPlaceHolder1_txtBillId").val("");

            if ($("#ContentPlaceHolder1_hfIsCostCenterWiseBillNumberGenerate").val() == "0") {
                $("#TouchKeypad").dialog({
                    width: 420,
                    height: 150,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: true,
                    resizable: false,
                    title: "Bill Number",
                    show: 'slide',
                    open: function (event, ui) {
                        $('#TouchKeypad').css('overflow', 'hidden');
                    }
                });
            }
            else {
                $("#TouchKeypad").dialog({
                    width: 420,
                    height: 150,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    title: "Bill Number",
                    show: 'slide',
                    open: function (event, ui) {
                        $('#TouchKeypad').css('overflow', 'hidden');
                    }
                });
            }
        }

        function BillReprint() {

            $("#TouchKeypad").dialog("close");

            if ($("#ContentPlaceHolder1_txtBillId").val() == "") {
                toastr.warning("Please Provide Bill Id.");
                return;
            }

            if ($("#ContentPlaceHolder1_hfIsCostCenterWiseBillNumberGenerate").val() == "1") {
                if ($("#ContentPlaceHolder1_hfCostcenterIdForBillReprint").val() == "0") {
                    toastr.warning("Please Select Cost-Center From Left Side.");
                    return;
                }
            }

            var billPrefix = $("#ContentPlaceHolder1_hfBillPrefixCostcentrwise").val();
            var billNo = $("#ContentPlaceHolder1_txtBillId").val();
            var billNumber = billPrefix + CommonHelper.padLeft(billNo, 8, '0');

            var iframeid = 'printDoc';
            var url = "/POS/Reports/frmReportBillForRetailPos.aspx?billID=" + billNumber;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 450,
                height: 'auto',
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                close: ClosePrintPreviewDialog,
                title: "Invoice Preview",
                show: 'slide'
            });

            return false;
        }

        function ClosePrintPreviewDialog() {
            $("#displayBill").dialog('close');

            var iframe = document.getElementById("IframeReportPrint");
            var html = "";
            iframe.contentWindow.document.open();
            iframe.contentWindow.document.write(html);
            iframe.contentWindow.document.close();
        }

        //function PrintDocumentFunc(printTemplate) {
        //    if (printTemplate == "1") {
        //        //$('#btnPrintPreview').trigger('click');
        //    }
        //    else if (printTemplate == "2") {
        //        //$('#btnPrintReportTemplate2').trigger('click');
        //    }
        //    else if (printTemplate == "3") {
        //        //$("#btnPrintReportTemplate3").trigger('click');
        //    }
        //    return true;
        //}

        function ClosePrintDialog() {
            window.location = "";
        }

        function EditBill() {
            var rowLength = $("#AddedItem tbody tr").length;

            if (rowLength > 0) {
                if (confirm("You have added Item(s).Do you want to clear?") == true) {
                    ClearAll();
                    BillEditDialogShow();
                }
            }
            else {
                BillEditDialogShow();
            }
        }

        function BillEditDialogShow() {
            $("#BillEditDialog").dialog({
                width: 420,
                height: 150,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                title: "Bill Number",
                show: 'slide',
                open: function (event, ui) {
                    $('#BillEditDialog').css('overflow', 'hidden');
                }
            });
        }

        function BillLodForEdit() {
            $("#BillEditDialog").dialog("close");

            $("#PreviousBillItem tbody").empty();

            if ($("#ContentPlaceHolder1_txtBillNoForEdit").val() == "") {
                toastr.warning("Please Provide Bill Id.");
                return;
            }

            var billPrefix = $("#ContentPlaceHolder1_hfBillPrefixCostcentrwise").val();
            var billNo = $("#ContentPlaceHolder1_txtBillNoForEdit").val();
            var billNumber = billPrefix + CommonHelper.padLeft(billNo, 8, '0');

            $.ajax({
                type: "POST",
                url: "frmSalesOrder.aspx/BillEdit",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    billNumberOrId: billNumber
                }),
                success: function (result) {

                    if (result.d.IsSuccess == false) {
                        toastr.info("Bill Is Not Valid / Already Return. Please Give Valid Bill Number.");
                        return;
                    }
                    if (result.d.RestaurantKotBill.TransactionType == "Company") {
                        $("#ContentPlaceHolder1_hfCompanyId").val(result.d.guestCompanyBO.CompanyId);
                        $("#txtCompanyAddress").val(result.d.guestCompanyBO.BillingStreet);
                        $("#txtCompanyEmailAddress").val(result.d.guestCompanyBO.EmailAddress);
                        $("#txtCompanyWebAddress").val(result.d.guestCompanyBO.WebAddress);
                        $("#txtBalance").val(result.d.guestCompanyBO.Balance);
                        $("#txtSearchCompany").val(result.d.guestCompanyBO.CompanyName);
                        if ($("#ContentPlaceHolder1_hfCompanyId").val() != "") {
                            $("#lblCompanyName").text(result.d.guestCompanyBO.CompanyName);
                        }
                        //arpon
                        $("#companyInfo").hide();
                        $("#companyPaymentDiv").removeClass("col-md-6").addClass("col-md-12");
                    }

                    $("#btnHoldUpList").attr("disabled", true);
                    $("#btnHoldUp").attr("disabled", true);

                    $("#ContentPlaceHolder1_hfBillId").val(result.d.RestaurantKotBill.BillId);
                    $("#ContentPlaceHolder1_hfResumedKotId").val(result.d.KotBillMaster.KotId);

                    //$("#txtDiscountAmount").val(result.d.RestaurantKotBill.DiscountAmount);

                    //if (result.d.RestaurantKotBill.DiscountType == "Percentage") {
                    //    $("#rbFixedDiscount").prop("checked", false);
                    //    $("#rbPercentageDiscount").prop("checked", true);
                    //}
                    //else if (result.d.RestaurantKotBill.DiscountType == "Fixed") {
                    //    $("#rbPercentageDiscount").prop("checked", false);
                    //    $("#rbFixedDiscount").prop("checked", true);
                    //}

                    //$("#txtDiscountAmount").val(result.d.RestaurantKotBill.DiscountAmount);

                    $("#ContentPlaceHolder1_hfReturnResumeBillNumber").val(result.d.RestaurantKotBill.BillNumber);
                    $("#headerinfo").text("Exchange Bill No. : " + result.d.RestaurantKotBill.BillNumber);

                    var row = 0, tr = "";

                    if (result.d.OrderItem.length > 0) {

                        var rowLength = result.d.OrderItem.length;

                        for (row = 0; row < rowLength; row++) {

                            PreviousBillItemList.push({
                                KotDetailId: result.d.OrderItem[row].KotDetailId,
                                KotId: result.d.KotBillMaster.KotId,
                                ItemId: result.d.OrderItem[row].ItemId,
                                ItemName: result.d.OrderItem[row].Name,
                                Code: result.d.OrderItem[row].Code,
                                UnitHead: result.d.OrderItem[row].UnitHead,
                                UnitPriceLocal: result.d.OrderItem[row].UnitPriceLocal,
                                StockBy: result.d.OrderItem[row].StockBy,
                                CategoryId: result.d.OrderItem[row].CategoryId,
                                DiscountType: result.d.OrderItem[row].DiscountType,
                                DiscountAmount: result.d.OrderItem[row].DiscountAmount,
                                Quantity: result.d.OrderItem[row].Quantity,
                                VatAmount: result.d.OrderItem[row].VatAmount,
                                InvoiceDiscount: result.d.OrderItem[row].InvoiceDiscount
                            });

                            tr += "<tr>";

                            tr += "<td style='width:12%;'>" + result.d.OrderItem[row].Code + "</td>";
                            tr += "<td style='width:36%;'>" + result.d.OrderItem[row].Name + "</td>";
                            tr += "<td style='width:10%;'>" + result.d.OrderItem[row].UnitHead + "</td>";
                            tr += "<td style='width:8%;'>" + result.d.OrderItem[row].Quantity + "</td>";
                            tr += "<td style='width:10%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='' onblur='CheckPreviousBillQuantity(this)' />" + "</td>";
                            tr += "<td style='width:10%;'>" + result.d.OrderItem[row].UnitPriceLocal + "</td>";
                            tr += "<td style='width:12%;' class='text-right'>0</td>";

                            tr += "<td style='display:none'>" + result.d.OrderItem[row].ItemId + "</td>";
                            tr += "<td style='display:none'>" + result.d.OrderItem[row].StockBy + "</td>";
                            tr += "<td style='display:none'>" + result.d.OrderItem[row].KotDetailId + "</td>";
                            tr += "<td style='display:none'>" + result.d.OrderItem[row].Quantity + "</td>";
                            tr += "<td style='display:none'>" + (result.d.OrderItem[row].InvoiceDiscount / result.d.OrderItem[row].Quantity) + "</td>";
                            tr += "<td style='display:none'>" + result.d.OrderItem[row].InvoiceDiscount + "</td>";

                            tr += "</td>";
                            tr += "</tr>";

                            $("#PreviousBillItem tbody").append(tr);
                            tr = "";
                        }

                        $("#ContentPlaceHolder1_hfIsBillExchange").val("1");

                        CommonHelper.ApplyDecimalValidation();
                        ClearItemSearchNAddItemDetails();
                    }

                    var PaymentMode = new Array();
                    var paymodeCount = 0;
                    row = 0;

                    //PaymentMode.push({ CardType: '', PaymentMode: 'Cash', Control: "txtCash" });
                    //PaymentMode.push({ CardType: 'a', PaymentMode: 'Card', Control: "txtAmexCard" });
                    //PaymentMode.push({ CardType: 'm', PaymentMode: 'Card', Control: "txtMasterCard" });
                    //PaymentMode.push({ CardType: 'v', PaymentMode: 'Card', Control: "txtVisaCard" });
                    //PaymentMode.push({ CardType: 'd', PaymentMode: 'Card', Control: "txtDiscoverCard" });

                    //paymodeCount = result.d.RestaurantKotBillPayment.length;

                    //if (paymodeCount > 0) {

                    //    for (row = 0; row < paymodeCount; row++) {

                    //        if (result.d.RestaurantKotBillPayment[row].PaymentMode == "Cash") {
                    //            $("#txtCash").val(result.d.RestaurantKotBillPayment[row].PaymentAmount);
                    //        }
                    //        else if (result.d.RestaurantKotBillPayment[row].PaymentMode == "Card" && result.d.RestaurantKotBillPayment[row].CardType == "a") {
                    //            $("#txtAmexCard").val(result.d.RestaurantKotBillPayment[row].PaymentAmount);
                    //        }
                    //        else if (result.d.RestaurantKotBillPayment[row].PaymentMode == "Card" && result.d.RestaurantKotBillPayment[row].CardType == "m") {
                    //            $("#txtMasterCard").val(result.d.RestaurantKotBillPayment[row].PaymentAmount);
                    //        }
                    //        else if (result.d.RestaurantKotBillPayment[row].PaymentMode == "Card" && result.d.RestaurantKotBillPayment[row].CardType == "v") {
                    //            $("#txtVisaCard").val(result.d.RestaurantKotBillPayment[row].PaymentAmount);
                    //        }
                    //        else if (result.d.RestaurantKotBillPayment[row].PaymentMode == "Card" && result.d.RestaurantKotBillPayment[row].CardType == "d") {
                    //            $("#txtDiscoverCard").val(result.d.RestaurantKotBillPayment[row].PaymentAmount);
                    //        }
                    //        else if (result.d.RestaurantKotBillPayment[row].PaymentMode == "Refund") {
                    //            $("#txtDueRChnage").val(result.d.RestaurantKotBillPayment[row].PaymentAmount);
                    //            $("#DueRChnageAmount").text("Change Amount");
                    //        }


                    //    }
                    //}

                    //SalesNDiscountCalculation();


                    if (result.d.membershipPointDetails != null) {
                        if (result.d.membershipPointDetails.PointWiseAmount > 0) {

                            $("#spendPoint").prop("checked", true);
                            $("#PointsInAmounts").show();
                            $('#PointsInAmountsDiv').show();
                            $('#CustomerCode').val(result.d.membershipPointDetails.MemberCode);

                            $("#ContentPlaceHolder1_hfMemberId").val(result.d.membershipPointDetails.MemberID);
                            $('#CustomerPoints').val(result.d.membershipPointDetails.TotalPoint);
                            $('#PointWiseMoney').val(result.d.membershipPointDetails.PointWiseAmount);
                            $('#txtPointAmount').val(result.d.membershipPointDetails.RedeemedAmount);
                            $('#PointsInAmounts').val('');
                            previousPoint = result.d.membershipPointDetails.RedeemedAmount;
                        }
                    }

                    PreviousBillDialog();

                },
                error: function (error) {
                    toastr.error("error");
                }
            });
        }

        function CheckPreviousBillQuantity(control) {

            var tr = $(control).parent().parent();
            var previousQuantity = $.trim($(tr).find("td:eq(3)").text());
            var quantity = $.trim($(tr).find("td:eq(4)").find("input").val());
            var unitPrice = $.trim($(tr).find("td:eq(5)").text());

            if (quantity == "") { quantity = '0'; }

            if (parseFloat(quantity) > parseFloat(previousQuantity)) {
                toastr.warning("Return Quantity Cannot Greater Than Previous Quantity.");
                quantity = "0";
                $(control).val("");
            }

            var totalPrice = parseFloat(unitPrice) * parseFloat(quantity);
            $(tr).find("td:eq(6)").text(toFixed(totalPrice, 2));
            PreviousBillSalesNDiscountCalculation();
            PaymentCalculation(0);
        }

        function PreviousBillSalesNDiscountCalculation() {
            if ($("#PreviousBillItem tbody tr").length == 0) {
                toastr.info("Please Load Previous Bill Item First.");
                return false;
            }

            var totalAmount = 0.00, amount = 0.00, index = 0;
            var discountAmount = 0.00, afterDiscountAmount = 0.00;
            var vat = 0.00, totalDiscountAmount = 0.00, grandTotal = 0.00, exchangeTotal = 0.00;
            var unit = 0.00, totalVatAmount = 0.00;
            var IsInclusiveBill = 0, IsVatEnable = 1;

            IsInclusiveBill = parseInt($("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val(), 10);
            vatAmount = parseFloat($("#ContentPlaceHolder1_hfRestaurantVatAmount").val());
            //IsVatEnable = parseFloat($("#ContentPlaceHolder1_hfIsVatEnable").val());

            if ($("#ContentPlaceHolder1_cbTPVatAmount").is(":checked")) {
                IsVatEnable = true;
            }
            else {
                IsVatEnable = false;
            }

            $("#PreviousBillItem tbody tr").each(function () {

                amount = parseFloat($(this).find("td:eq(6)").text());
                unit = parseFloat($(this).find("td:eq(4) input").val());

                if (isNaN(unit))
                    return;

                //if (PreviousBillItemList[index].DiscountType == "Fixed") {
                //    discountAmount = PreviousBillItemList[index].DiscountAmount / PreviousBillItemList[index].Quantity * unit;
                //}
                //else if (PreviousBillItemList[index].DiscountType == "Percentage") {
                //    discountAmount = amount * (PreviousBillItemList[index].DiscountAmount / PreviousBillItemList[index].Quantity * unit / 100.00);
                //}

                discountAmount = PreviousBillItemList[index].InvoiceDiscount / PreviousBillItemList[index].Quantity * unit;

                totalAmount += amount;
                totalDiscountAmount += discountAmount;
                afterDiscountAmount += (amount - discountAmount);

                index++;
                discountAmount = 0.00, vat = 0.00;
            });
            //arpon
            if (IsInclusiveBill == 0) {
                vat = parseFloat((((afterDiscountAmount) * vatAmount) / (100)) * IsVatEnable);

            }
            else {
                vat = parseFloat(((afterDiscountAmount * vatAmount) / (100 + vatAmount)) * IsVatEnable);
            }

            //vat = PreviousBillItemList[index].VatAmount / PreviousBillItemList[index].Quantity * unit;
            totalVatAmount += vat;

            exchangeTotal = afterDiscountAmount + totalVatAmount;
            exchangeTotal -= parseFloat($("#txtPointAmount").val() == "" ? 0 : ($("#txtPointAmount").val()));
            grandTotal = parseFloat($("#txtGrandTotal").val() == "" ? 0 : ($("#txtGrandTotal").val()));

            if (grandTotal > 0)
                grandTotal = grandTotal - exchangeTotal;

            $("#txtReturnItemVatTotal").val(vat);
            $("#txtRoundedGrandTotal").val(toFixed(grandTotal, 2));
            $("#txtReturnItemTotal").val(toFixed(exchangeTotal, 2));

            var isRestaurantBillAmountWillRound = $("#ContentPlaceHolder1_hfIsRestaurantBillAmountWillRound").val();
            var fractionalAmount = 0.00, roundedGrandTotal = toFixed(grandTotal, 2);

            if (isRestaurantBillAmountWillRound == "1") {

                var roundedGrandTotal = Math.round(grandTotal)
                var fractionalAmount = toFixed((roundedGrandTotal - toFixed(grandTotal, 2)), 2);

                if (fractionalAmount > 0) {
                    $("#roundedSign").text("+");
                    $("#txtRoundedAmount").val(fractionalAmount);
                }
                else {
                    $("#roundedSign").text("-");
                    $("#txtRoundedAmount").val((fractionalAmount * -1.00));
                }

                $("#txtRoundedGrandTotal").val(roundedGrandTotal);
            }
        }

        function PreviousBillDialog() {

            $("#exchaneItemTotalContainer").show();
            $("#btnPreviousBillDialog").show();

            $("#PreviousBillItemDialog").dialog({
                width: 1200,
                height: 550,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                title: "Previous Bill Details : " + $("#ContentPlaceHolder1_hfReturnResumeBillNumber").val(),
                show: 'slide',
                open: function (event, ui) {
                    $('#PreviousBillItemDialog').css('overflow', 'hidden');
                    $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                }
            });

        }

        function PreviousBillItemDialogOkay() {
            var exchangeVal = 0, total = 0.00;
            $("#PreviousBillItem tbody > tr").each(function () {
                exchangeVal = $(this).find("td:eq(4)").find('input').val();
                total = total + (exchangeVal == "" ? 0.00 : parseFloat(exchangeVal));
            });

            if (total == 0) {
                toastr.info("Please Give Exchange Quantity.");
                return false;
            }

            $("#PreviousBillItemDialog").dialog("close");
        }

        function ClosePreviousBillItemDialog() {
            $("#PreviousBillItemDialog").dialog("close");
        }

        function ScrollToDown() {
            //document.getElementById('bottom').scrollIntoView();
        }

        function ClearForm() {
            if (confirm("Do you want to clear?") == true) {
                ClearAll();
            }
        }

        function ClearAll() {
            document.getElementById('ContentPlaceHolder1_lblTotalItemTypes').innerHTML = "";
            document.getElementById('ContentPlaceHolder1_lblTotalItemQuantity').innerHTML = "";

            $("#ItemCode").val("");
            $("#ItemName").val("");
            $("#ContentPlaceHolder1_hfIsBillExchange").val("0");

            $("#ItemCode").focus();

            ItemDetails = null;
            AddedItemList = new Array();
            EditedItemList = new Array();
            DeletedItemList = new Array();
            PreviousBillItemList = new Array();

            $("#AddedRiceMillBillingItem tbody").html("");
            $("#AddedItem tbody").html("");
            $("#PreviousBillItem  tbody").html("");
            $("#ContentPlaceHolder1_hfResumedKotId").val("");
            $("#ContentPlaceHolder1_hfBillId").val("");
            $("#headerinfo").text("");

            $("#PaymentArea input").each(function () {
                $(this).val("");
            });

            $("#SalesCalculationArea input").each(function () {
                $(this).val("");
            });

            $("#DueRChnageAmount").text("Due/Change");
            $("#txtDueRChnage").val("");
            if ($("#ContentPlaceHolder1_hfIsRemarkHasDefaultValue").val() == "0") {
                $("#ContentPlaceHolder1_txtRemarks").val("");
            }

            $("#ContentPlaceHolder1_txtSubject").val("");
            $("#txtReturnItemTotal").val("");
            $("#txtReturnItemVatTotal").val("");

            $("#btnHoldUpList").attr("disabled", false);
            $("#btnHoldUp").attr("disabled", false);
            $("#exchaneItemTotalContainer").hide();
            $("#btnPreviousBillDialog").hide();
            $("#CustomerCode").val("");
            $("#CustomerPoints").val("");
            $("#PointWiseMoney").val("");
            $("#spendPoint").prop("checked", false);
            $("#ContentPlaceHolder1_hfMemberId").val("");
            $("#ContentPlaceHolder1_hfMemberName").val("");
            ClearPayment('Company');
        }

        function ClearAfterAllItemDelete() {
            $("#PaymentArea input").each(function () {
                $(this).val("");
            });

            $("#SalesCalculationArea input").each(function () {
                $(this).val("");
            });

            $("#DueRChnageAmount").text("Due/Change");
            $("#txtDueRChnage").val("");
            if ($("#ContentPlaceHolder1_hfIsRemarkHasDefaultValue").val() == "0") {
                $("#ContentPlaceHolder1_txtRemarks").val("");
            }
            $("#ContentPlaceHolder1_txtSubject").val("");
            $("#txtReturnItemTotal").val("");
            $("#txtReturnItemVatTotal").val("");
        }

        function AddSerialForAdHocItem(control) {
            var tr = $(control).parent().parent();

            if ($("#ContentPlaceHolder1_ddlProject").val() == "0") {
                toastr.warning("Please Select Project.");
                $("#ContentPlaceHolder1_ddlProject").focus();
                return false;
            }

            var itemId = 0, quantity = 0;
            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                itemId = parseInt($.trim($(tr).find("td:eq(11)").text()), 10);
                quantity = $(tr).find("td:eq(7)").find("input").val();
            }
            else {
                itemId = parseInt($.trim($(tr).find("td:eq(8)").text()), 10);
                quantity = $(tr).find("td:eq(4)").find("input").val();
            }

            var itemName = $(tr).find("td:eq(1)").text();
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

    </script>
    <asp:HiddenField ID="hfIsUpdate" runat="server" Value="0" />

    <asp:HiddenField ID="hfItemIdForSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsSerializableItem" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsValidSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsAttributeItem" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsItemAttributeEnable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRiceMillBillingEnable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRestaurantIntegrateWithCompany" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsItemAutoSave" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsTaskAutoGenarate" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsItemCodeHideForBilling" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsStockHideForBilling" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsStockByHideForBilling" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRemarksHideForBilling" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsCashPaymentShow" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsAmexCardPaymentShow" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsMasterCardPaymentShow" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsVisaCardPaymentShow" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsDiscoverCardPaymentShow" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsCompanyPaymentShow" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsSubjectShow" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRemarkShow" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRemarkHasDefaultValue" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsVatEnable" runat="server" />
    <asp:HiddenField ID="hfIsMembershipPaymentEnable" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfPOSRefundConfiguration" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsBillingTypeEnable" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfRestaurantVatAmount" runat="server" />
    <asp:HiddenField ID="hfIsRestaurantBillInclusive" runat="server" />
    <asp:HiddenField ID="hfIsRestaurantBillAmountWillRound" runat="server" />
    <asp:HiddenField ID="hfLocalCurrencyId" runat="server" />
    <asp:HiddenField ID="hfResumedKotId" runat="server" />
    <asp:HiddenField ID="hfBillId" runat="server" />
    <asp:HiddenField ID="hfBillIdControl" runat="server" />
    <asp:HiddenField ID="hfMemberId" runat="server" />
    <asp:HiddenField ID="hfMemberName" runat="server" />
    <asp:HiddenField ID="hfPrintBillId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsCostCenterWiseBillNumberGenerate" runat="server" Value="0" />
    <asp:HiddenField ID="hfBillPrefixCostcentrwise" runat="server" Value="" />
    <asp:HiddenField ID="hfCostcenterId" runat="server" />
    <asp:HiddenField ID="hfIsBillExchange" runat="server" Value="0" />
    <asp:HiddenField ID="hfReturnResumeBillNumber" runat="server" Value="" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfPaymentId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfContactId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsCustomerDetailsEnable" runat="server" />
    <asp:HiddenField ID="hfIsDeliveredByEnable" runat="server" />

    <div style="height: 555px; overflow-y: scroll; display: none" id="reportContainer">
        <rsweb:ReportViewer ID="rvTransactionShow" ShowFindControls="false" ShowWaitControlCancelLink="false"
            PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
            Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt" ShowPageNavigationControls="true" ZoomMode="FullPage"
            Height="2000" ClientIDMode="Static" ShowRefreshButton="false">
        </rsweb:ReportViewer>
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
                    <div id="labelAndtxtSerial" class="row" style="display: none;">
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
    <div style="display: none;">
        <%--<asp:Button ID="btnPrintPreview" runat="server" OnClick="btnPrintPreview_Click" Text="Print Preview" ClientIDMode="Static" />
        <asp:Button ID="btnPrinReturnBillPreview" runat="server" Text="Return Bill Print" ClientIDMode="Static" OnClick="btnPrinReturnBillPreview_Click" />--%>
    </div>
    <div class="row">
        <div class="col-sm-9 pos-border-y">
            <div class="panel panel-default">
                <div class="panel-body" style="padding: 0;">
                    <div class="row">
                        <div class="col-md-12">
                            <table id="RetaurantTableTop" class="table table-responsive">
                                <tbody>
                                    <tr>
                                        <td id="itemCodeInputNameCol" style="width: 15%;">Item Code / BarCode</td>
                                        <td style="width: 35%;">Item Name</td>
                                        <td id="itemColorInputNameCol" style="width: 15%; display: none;">Color</td>
                                        <td id="itemSizeInputNameCol" style="width: 15%; display: none;">Size</td>
                                        <td id="itemStyleInputNameCol" style="width: 15%; display: none;">Style</td>
                                        <td id="itemStyleInputQtyCol" style="width: 15%;">Qty.</td>
                                    </tr>
                                    <tr>
                                        <td id="itemCodeInputCol" style="width: 15%;">
                                            <input type="text" class="form-control" id="ItemCode" placeholder="Code" /></td>
                                        <td style="width: 30%;">
                                            <input type="text" class="form-control" id="ItemName" placeholder="Name" /></td>
                                        <td id="itemColorInputCol" style="width: 10%; display: none;">
                                            <asp:DropDownList ID="ddlColorAttribute" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="itemSizeInputCol" style="width: 10%; display: none;">
                                            <asp:DropDownList ID="ddlSizeAttribute" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="itemStyleInputCol" style="width: 15%; display: none;">
                                            <asp:DropDownList ID="ddlStyleAttribute" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="itemCurrentStock" style="width: 10%;">
                                            <asp:Label ID="lblCurrentStock" runat="server" class="form-control" Text="00"></asp:Label>
                                        </td>
                                        <td id="itemProductTypeCol" style="width: 10%; display: none;">
                                            <asp:DropDownList ID="ddlProductType" runat="server" CssClass="form-control">
                                                <asp:ListItem>Non Serial Product</asp:ListItem>
                                                <asp:ListItem>Serial Product</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="col-md-5" style="display: none;">
                            <table id="RetaurantTableTopCustomer" class="table table-responsive">
                                <tbody>
                                    <tr>
                                        <td style="width: 40%;">Customer Code</td>
                                        <td style="width: 20%;">Points</td>
                                        <td style="width: 20%;">Money</td>
                                        <td style="width: 20%; text-align: center; vertical-align: central;">
                                            <input type="checkbox" id="spendPoint" style="margin: 0px;" />
                                            <span>Adjust</span></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%;">
                                            <input type="text" class="form-control" id="CustomerCode" placeholder="Customer Code" /></td>
                                        <td style="width: 20%;">
                                            <input type="text" class="form-control" id="CustomerPoints" placeholder="Points" disabled="disabled" /></td>
                                        <td style="width: 20%;">
                                            <input type="text" class="form-control" id="PointWiseMoney" placeholder="Money" disabled="disabled" /></td>
                                        <td style="width: 20%;">
                                            <input type="text" class="form-control" id="PointsInAmounts" placeholder="Amount" style="display: none;" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="PosDivider">
                <hr />
            </div>
            <div class="row">
                <div class="col-md-12" style="height: 320px;">
                    <div id="BillingItemDiv" style="height: 310px; overflow-y: scroll;">
                        <table id="AddedItem" class="table table-bordered table-condensed table-hover table-responsive">
                            <thead>
                                <tr>
                                    <th id="codeCol" style="width: 12%;">Code</th>
                                    <th style="width: 20%;">Item Name</th>
                                    <th id="cIdd" style="width: 10%;">Color</th>
                                    <th id="sIdd" style="width: 10%;">Size</th>
                                    <th id="stIdd" style="width: 10%;">Style</th>
                                    <th id="stockCol" style="width: 10%;">Stock</th>
                                    <th id="stockByCol" style="width: 10%;">Stock By</th>
                                    <th style="width: 8%;">Quantity</th>
                                    <th style="width: 12%;">Unit Price</th>
                                    <th style="width: 12%;">Total Price</th>
                                    <th id="remarksCol" style="width: 10%;">Remarks</th>
                                    <th style='display: none'>ItemId</th>
                                    <th style='display: none'>StockById</th>
                                    <th style='display: none'>CategoryId</th>
                                    <th style='display: none'>KotDetailId</th>
                                    <th style='display: none'>DBQuantity</th>
                                    <th style="width: 6%;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <div id="RiceMillBillingItemDiv" style="height: 310px; overflow-y: scroll;">
                        <table id="AddedRiceMillBillingItem" class="table table-bordered table-condensed table-hover table-responsive">
                            <thead>
                                <tr>
                                    <th id="codeARMCol" style="width: 12%;">Code</th>
                                    <th style="width: 20%;">Item Name</th>
                                    <th id="stockARMCol" style="width: 10%;">Stock</th>
                                    <th id="stockByARMCol" style="width: 10%;">Stock By</th>
                                    <th style="width: 8%;">Bag Waight</th>
                                    <th style="width: 8%;">Bag</th>
                                    <th style="width: 8%;">Quantity</th>
                                    <th style="width: 12%;">Unit Price</th>
                                    <th style="width: 12%;">Total Price</th>
                                    <th style='display: none'>ItemId</th>
                                    <th style='display: none'>StockById</th>
                                    <th style='display: none'>CategoryId</th>
                                    <th style='display: none'>KotDetailId</th>
                                    <th style="width: 6%;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <hr />
                <div class="col-md-4 col-md-offset-2">
                    <asp:Label ID="lblTotalItemTypes" runat="server" class="control-label" Text=""></asp:Label>
                </div>
                <div class="col-md-6">
                    <asp:Label ID="lblTotalItemQuantity" runat="server" class="control-label" Text=""></asp:Label>
                </div>
            </div>
        </div>
        <div class="col-sm-3">
            <div class="row">
                <div class="col-md-12">
                    <div id="PosControlRight">
                        <div id="SalesCalculationArea">
                            <div id="ProjectDiv" class="form-group no-gutter">
                                <div class="col-md-2">
                                    <label for="TotalSales" class="required-field">
                                        Project</label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-12">
                                    <div class="PosDivider">
                                        <hr />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-5">
                                    <label for="TotalSales">
                                        Total Sales</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control" id="txtTotalSales" placeholder="Total Sales" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-12" id="discountTable">
                                    <table class="table table-condensed table-responsive" style="margin-bottom: 2px;">
                                        <tbody>
                                            <tr>
                                                <td style="width: 60px;">
                                                    <input type="radio" id="rbFixedDiscount" name="DiscountType" />&nbsp;<span>Fixed</span>
                                                </td>
                                                <td style="width: 100px;">
                                                    <input type="radio" id="rbPercentageDiscount" name="DiscountType" checked="checked" />&nbsp;<span>Percentage</span>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-5">
                                    <label for="DiscountAmount">
                                        Discount Amount</label>
                                </div>
                                <div class="col-md-7">
                                    <asp:HiddenField ID="hfTotalDiscountAmount" runat="server" Value="0"></asp:HiddenField>
                                    <input type="text" class="form-control quantity" id="txtDiscountAmount" tabindex="1" placeholder="Discount Amount"
                                        onblur="CalculateDiscount()" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-5">
                                    <label for="AfterDiscountAmount">
                                        After Discount</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control" id="txtAfterDiscountAmount" placeholder="After Discount Amount" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-5">
                                    <label for="Vat">
                                        Vat Amount</label>
                                </div>
                                <div class="col-md-1">
                                    <asp:CheckBox ID="cbTPVatAmount" runat="server" onclick="javascript: return IsVatEnableCheckOrUncheck(this);" />
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlInclusiveOrExclusive" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="Inclusive">Inc.</asp:ListItem>
                                        <asp:ListItem Value="Exclusive">Exc.</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <input type="text" class="form-control" id="txtVat" placeholder="Vat Amount" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-5">
                                    <label for="GrandTotal">
                                        Grand Total</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control" id="txtGrandTotal" placeholder="Grand Total" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" style="display: none;">
                                <div class="col-md-5">
                                    <label for="txtReturnItemTotal">
                                        Exchange Item Vat Total</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control" id="txtReturnItemVatTotal" placeholder="Exchange Item Vat Total" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" id="exchaneItemTotalContainer" style="display: none;">
                                <div class="col-md-5">
                                    <label for="txtReturnItemTotal">
                                        Exchange Item Total</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control" id="txtReturnItemTotal" placeholder="Exchange Item Total" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" id="PointsInAmountsDiv" style="display: none;">
                                <div class="col-md-5">
                                    <label for="txtPointAmount">
                                        Point Amount</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control" id="txtPointAmount" placeholder="Point Amount" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" style="display: none;">
                                <div class="col-md-5">
                                    <label for="RoundedAmount" id="lblRoundedAmount">
                                        Rounded Amount(<span id="roundedSign">+/-</span>)</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control quantitydecimal" id="txtRoundedAmount" disabled="disabled" placeholder="Rounded Amount" onblur="RoundedAmountCalculation(this.value)" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" style="display: none;">
                                <div class="col-md-5">
                                    <label for="DiscoverCard">
                                        Rounded Grand Total</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control quantitydecimal" id="txtRoundedGrandTotal" disabled="disabled" placeholder="Grand Total" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group no-gutter">
                            <div class="col-md-12">
                                <div class="PosDivider">
                                    <hr />
                                </div>
                            </div>
                        </div>
                        <div id="PaymentArea">
                            <div class="form-group no-gutter" id="cashDiv" style="display:none;">
                                <div class="col-md-5">
                                    <label for="Cash">
                                        Cash</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control quantitydecimal" tabindex="2" id="txtCash" placeholder="Cash" onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" id="amexCardDiv">
                                <div class="col-md-5">
                                    <label for="AmexCard">
                                        Amex Card</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control quantitydecimal" tabindex="3" id="txtAmexCard" placeholder="Amex Card" onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" id="masterCardDiv">
                                <div class="col-md-5">
                                    <label for="MasterCard">
                                        Master Card</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control quantitydecimal" tabindex="4" id="txtMasterCard" placeholder="Master Card" onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" id="visaCardDiv">
                                <div class="col-md-5">
                                    <label for="VisaCard">
                                        Visa Card</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control quantitydecimal" tabindex="5" id="txtVisaCard" placeholder="Visa Card" onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" id="discoverCardDiv">
                                <div class="col-md-5">
                                    <label for="DiscoverCard">
                                        Discover Card</label>
                                </div>
                                <div class="col-md-7">
                                    <input type="text" class="form-control quantitydecimal" tabindex="6" id="txtDiscoverCard" placeholder="Discover Card" onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" id="CompanyPaymentContainer">
                                <label class="control-label col-sm-5" id="lblCompany">
                                    Company</label>
                                <div class="col-sm-7">
                                    <div class="row">
                                        <div class="col-sm-7" id="companyPaymentDiv" style="display:none;">
                                            <input type="text" class="form-control quantitydecimal" disabled="disabled" tabindex="7" id="txtCompanyPayment" placeholder="Com." onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                        </div>
                                        <div class="col-sm-5" id="companyInfo">
                                            <img border="0" alt="Company Search" onclick="javascript:return LoadCompanyInfo()"
                                                style="cursor: pointer; display: inline;" title="Company Search" src="../Images/company.png" />
                                            <img border="0" id="imgClearCompanyPayment" alt="Clear" onclick="javascript:return ClearPayment('Company')"
                                                style="cursor: pointer; display: inline;" title="Clear Company Payment" src="../Images/clear.png" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <asp:Label ID="lblCompanyName" runat="server" Text="" CssClass="control-label" ClientIDMode="Static"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="CompanyInfoDialog" style="display: none;">
                            <div id="CompanyInformationDiv" class="panel panel-default col-sm-4" style="height: 333px;">
                                <div class="panel-heading">
                                    Company Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <asp:Label ID="Label13" runat="server" Text="Company" CssClass="control-label col-sm-3"></asp:Label>
                                            <div class="col-sm-9">
                                                <input type="text" class="form-control" id="txtSearchCompany" />
                                            </div>
                                        </div>
                                        <fieldset>
                                            <legend>Company Details</legend>
                                            <div class="form-group">
                                                <asp:Label ID="Label14" runat="server" Text="Address" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <textarea class="form-control" id="txtCompanyAddress" readonly="readonly" rows="4" cols="50"></textarea>
                                                </div>
                                            </div>
                                            <div class="form-group" style="display: none;">
                                                <asp:Label ID="Label15" runat="server" Text="Email ddress" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtCompanyEmailAddress" readonly="readonly" />
                                                </div>
                                            </div>
                                            <div class="form-group" style="display: none;">
                                                <asp:Label ID="Label9" runat="server" Text="Web ddress" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtCompanyWebAddress" readonly="readonly" />
                                                </div>
                                            </div>
                                            <div class="form-group" style="display: none;">
                                                <asp:Label ID="Label1511" runat="server" Text="Balance" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtBalance" />
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                            <div id="AttentionDiv" class="panel panel-default col-sm-4" style="height: 333px;">
                                <div class="panel-heading">
                                    Attention Detail
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <asp:Label ID="Label1" runat="server" Text="Contact" CssClass="control-label col-sm-3"></asp:Label>
                                            <div class="col-sm-9">
                                                <input type="text" class="form-control" id="txtSearchContact" />
                                            </div>
                                        </div>
                                        <fieldset>
                                            <legend>Contact Details</legend>
                                            <div class="form-group">
                                                <asp:Label ID="Label2" runat="server" Text="Designation" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtDesignation" readonly="readonly" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="Label3" runat="server" Text="Department" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtDepartment" readonly="readonly" />
                                                </div>
                                            </div>

                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                            <div id="PaymentInstructionDiv" class="panel panel-default col-sm-4" style="height: 333px;">
                                <div class="panel-heading">
                                    Payment Instruction
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <asp:Label ID="Label5" runat="server" Text="Bank" CssClass="control-label col-sm-3"></asp:Label>
                                            <div class="col-sm-9">
                                                <input type="text" class="form-control" id="txtSearchBank" />
                                            </div>
                                        </div>
                                        <fieldset>
                                            <legend>Payment Details</legend>
                                            <div class="form-group">
                                                <asp:Label ID="Label6" runat="server" Text="Branch Name" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtBranchName" readonly="readonly" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="Label7" runat="server" Text="Account Name" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtAccountName" readonly="readonly" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="Label4" runat="server" Text="Account Number" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtAccountNumber" readonly="readonly" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="Label8" runat="server" Text="Account Type" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtAccountType" readonly="readonly" />
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                            <div class="row no-gutters">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="row pull-right">
                                            <div class="col-sm-12">
                                                <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                                    id="btnClearCompanyInformationSubmit" value="Cancel" onclick="ClearPayment('Company')" />
                                                <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                                    id="btnCompanyInformationSubmit" value="Ok" onclick="CompanyPayment()" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="FullRefundRemarksDialog" style="display: none;">
                            <div class="row no-gutters">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblRemarksForFullRefund" CssClass="control-label required-field" runat="server" Text="Remarks"></asp:Label>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:TextBox ID="txtRemarksForFullRefund" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <input type="button" id="AddRemarksForFullRefund" class="TransactionalButton btn btn-primary btn-sm" onclick="AddRemarks()" value="Add" />
                                                <input type="button" id="CancelRemarksForFullRefund" class="TransactionalButton btn btn-primary btn-sm" onclick="CloseRemarks()" value="Cancel" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group no-gutter" style="display: none;">
                            <div class="col-md-5">
                                <label id="DueRChnageAmount" for="DueRChnage">
                                    Due/Change</label>
                            </div>
                            <div class="col-md-7">
                                <input type="text" class="form-control quantitydecimal" id="txtDueRChnage" disabled="disabled" placeholder="Due/Change Amount" />
                            </div>
                        </div>
                        <div class="form-group no-gutter">
                            <div class="col-md-12">
                                <div class="PosDivider">
                                    <hr />
                                </div>
                            </div>
                        </div>
                        <div class="form-group no-gutter">
                            <div class="col-md-12">
                                <asp:TextBox ID="txtSubject" TabIndex="2" Style="height: 25px;" TextMode="MultiLine" placeholder="Subject" CssClass="form-control"
                                    runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-12" style="padding-top: 5px;">
                                <asp:TextBox ID="txtRemarks" TabIndex="2" TextMode="MultiLine" placeholder="Remarks" CssClass="form-control"
                                    runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-12">
                                <asp:Label ID="lblEstimatedTaskDoneDate" runat="server" Text="Estimated Task Done Date" CssClass="control-label required-field"></asp:Label>
                            </div>
                            <div class="col-md-12">
                                <input type="text" class="form-control" id="txtEstimatedTaskDoneDate" />
                            </div>
                        </div>

                        <div id="BillingTypeUpperDividerDiv" runat="server" class="form-group no-gutter">
                            <div class="col-md-12">
                                <div class="PosDivider">
                                    <hr />
                                </div>
                            </div>
                        </div>
                        <div id="BillingTypeDiv" runat="server" class="form-group no-gutter" style="display: none;">
                            <div class="col-md-4">
                                <label for="TotalSales" class="required-field">
                                    Billing Type</label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlBillingType" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="BillingTypeBottomDividerDiv" runat="server" class="form-group no-gutter">
                            <div class="col-md-12">
                                <div class="PosDivider">
                                    <hr />
                                </div>
                            </div>
                        </div>

                        <div class="form-group no-gutter" id="DeliveredByDiv" runat="server">
                            <div class="col-md-5">
                                <label id="DeliveredBy" for="Delivered By">
                                    Delivery By</label>
                            </div>
                            <div class="col-md-7">
                                <asp:DropDownList ID="ddlDeliveredBy" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-md-2" style="display: none;">
        <input type="button" id="BillReturnExchangeButton" value="Bill Return/Exchange" class="btn btn-primary" onclick="EditBill()" />
        <input type="button" id="btnPreviousBillDialog" value="Previous Bill" class="btn btn-primary" onclick="PreviousBillDialog()" style="display: none;" />
        <input type="button" id="BillPrintButton" value="Bill Print" class="btn btn-primary" onclick="BillPrint()" />
    </div>
    <div id="posfooter">
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-9" id="CustomerDetailsDiv" style="display: none;">
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCustomerName" Width="100%" TabIndex="2" placeholder="Customer Name" CssClass="form-control"
                                        runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCustomerMobile" Width="100%" TabIndex="2" placeholder="Customer Mobile" CssClass="form-control"
                                        runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCustomerAddress" Width="100%" TabIndex="2" TextMode="MultiLine" placeholder="Customer Address" CssClass="form-control"
                                        runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3 pull-right">
                                <div class="pull-right">
                                    <input type="button" id="btnHoldUpList" value="Hold-Up List" class="btn btn-primary" onclick="GetHoldUpPosInfo()" />
                                    <input type="button" id="btnHoldUp" value="Hold-Up" class="btn btn-primary" onclick="BillHoldup()" />
                                    <input type="button" value="Clear" class="btn btn-primary" style="width: 100px;" onclick="ClearForm()" />
                                    <input type="button" id="btnSettlement" value="Save" style="width: 100px;" class="btn btn-primary" onclick="SaveSalesOrder()" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="PosHoldUpDialog" style="display: none;">
        <div class="row">
            <div class="col-md-12">
                <table id="PosHoldUpList" class="table table-bordered table-condensed">
                    <thead>
                        <tr>
                            <th style="width: 100%;">Bill Number</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div id="TouchKeypad" style="display: none;">
        <div class="form-horizontal">
            <div class="row no-gutters">
                <div class="col-md-12">
                    <div id="TouchKeypadResultDiv">
                        <asp:TextBox ID="txtBillId" runat="server" Style="text-align: right;" CssClass="numkbnotdecimal form-control" Height="40px"
                            Font-Size="35px"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group pull-right" style="padding-right: 14px; padding-top: 5px;">
                <div class="col-md-12">
                    <input type="button" class="btn btn-primary btn-large"
                        style="width: 90px; height: 40px; font-size: 1.5em;" value="OK" onclick='BillReprint()' />
                </div>
            </div>
        </div>
    </div>
    <div id="BillEditDialog" style="display: none;">
        <div class="form-horizontal">
            <div class="row no-gutters">
                <div class="col-md-12">
                    <asp:TextBox ID="txtBillNoForEdit" runat="server" Style="text-align: right;"
                        CssClass="numkbnotdecimal form-control" Height="40px"
                        Font-Size="35px"></asp:TextBox>
                </div>
            </div>
            <div class="form-group pull-right" style="padding-right: 14px; padding-top: 5px;">
                <div class="col-md-12">
                    <input type="button" class="btn btn-primary btn-large"
                        style="width: 90px; height: 40px; font-size: 1.5em;" value="OK" onclick='BillLodForEdit()' />
                </div>
            </div>
        </div>
    </div>
    <div id="displayBill" style="display: none;">
        <div class="row no-gutters">
            <div class="col-md-12">
                <div class="well well-sm col-md-12">
                    <div class="row pull-right">
                        <div class="col-md-12">
                            <input type="button" class="btn btn-primary btn-large" value="Close" onclick="ClosePrintPreviewDialog()" />
                        </div>
                    </div>
                </div>
                <iframe id="printDoc" name="printDoc" frameborder="0" style="overflow: hidden; width: 410px; height: 550px"></iframe>
                <div id="bottomPrint">
                </div>
            </div>
        </div>
    </div>
    <div id="PreviousBillItemDialog" style="display: none;">
        <div class="panel panel-default">
            <div class="panel-body">
                <div style="height: 300px; overflow-y: scroll;">
                    <table id="PreviousBillItem" class="table table-bordered table-condensed table-hover table-responsive">
                        <thead>
                            <tr>
                                <th style="width: 12%;">Code
                                </th>
                                <th style="width: 36%;">Item Name
                                </th>
                                <th style="width: 10%;">Stock By
                                </th>
                                <th style="width: 8%;">Quantity
                                </th>
                                <th style="width: 10%;">Exchange Quantity</th>
                                <th style="width: 12%;">Unit Price
                                </th>
                                <th style="width: 12%;">Exchange Total
                                </th>
                                <th style='display: none'>ItemId
                                </th>
                                <th style='display: none'>StockById
                                </th>
                                <th style='display: none'>KotDetailId
                                </th>
                                <th style='display: none'>DBQuantity
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="panel-footer">
                <input type="button" class="btn btn-primary" style="width: 84px; height: 38px; font-size: 1.3em;" value="Ok" onclick="PreviousBillItemDialogOkay()" />
                <input type="button" class="btn btn-primary" style="width: 84px; height: 38px; font-size: 1.3em;" value="Close" onclick="ClosePreviousBillItemDialog()" />
            </div>
        </div>
    </div>
    <iframe id="IframeReportPrint" name="IframeReportPrint" width="0" height="0" runat="server"
        style="left: -1000; top: 2000;" clientidmode="static" scrolling="yes"></iframe>
</asp:Content>
<asp:Content ID="contentHeader" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="row">
        <div class="col-md-12" style="font-size: 18px; font-weight: bold; text-align: center; color: #fff; padding-right: 5px; vertical-align: middle; padding-top: 9px;">
            <label id="headerinfo"></label>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            var billingBillID = $.trim(CommonHelper.GetParameterByName("salesOrderID"));
            if (billingBillID != "") {
                PerformEdit(billingBillID);
            }

            var IsShowBillReturnExchangeButton = false;
            if (IsShowBillReturnExchangeButton == true) {
                $("#BillReturnExchangeButton").show();
            }
            else {
                $("#BillReturnExchangeButton").hide();
            }

            var IsShowBillPrintButton = false;
            if (IsShowBillReturnExchangeButton == true) {
                $("#BillPrintButton").show();
            }
            else {
                $("#BillPrintButton").hide();
            }

            var IsShowbtnHoldUpList = false;
            if (IsShowbtnHoldUpList == true) {
                $("#btnHoldUpList").show();
            }
            else {
                $("#btnHoldUpList").hide();
            }

            var IsShowbtnHoldUp = false;
            if (IsShowbtnHoldUp == true) {
                $("#btnHoldUp").show();
            }
            else {
                $("#btnHoldUp").hide();
            }
        });
    </script>
    <asp:HiddenField ID="hfBillTemplate" runat="server" />
    <div style="display: none;">
        <asp:Button ID="btnPrintReportTemplate1" runat="server" Text="template1" OnClick="btnPrintReportTemplate1_Click1"
            ClientIDMode="Static" />
        <asp:Button ID="btnPrintReportTemplate2" runat="server" Text="template2" OnClick="btnPrintReportTemplate2_Click1"
            ClientIDMode="Static" />
    </div>
    <iframe id="frmPrint" name="IframeName" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
        clientidmode="static" scrolling="yes"></iframe>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#ContentPlaceHolder1_hfIsRiceMillBillingEnable").val() == '1') {
                document.getElementById("remarksCol").innerText = "Bag";
                $("#BillingItemDiv").hide();
                $("#RiceMillBillingItemDiv").show();
            }
            else {
                $("#BillingItemDiv").show();
                $("#RiceMillBillingItemDiv").hide();
            }

            //$("#BillingItemDiv").show();
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {
                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransactionShow.ClientID %>"));
                var printTemplate = $("#<%=hfBillTemplate.ClientID %>").val();

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(' + printTemplate + '); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }
        });

        function PrintDocumentFunc(printTemplate) {
            if (printTemplate == "1") {
                $('#btnPrintReportTemplate1').trigger('click');
            }
            else if (printTemplate == "5") {
                $('#btnPrintReportTemplate2').trigger('click');
            }

            return true;
        }
    </script>
</asp:Content>
