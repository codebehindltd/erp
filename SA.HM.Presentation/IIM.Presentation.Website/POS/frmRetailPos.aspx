<%@ Page Title="" Language="C#" MasterPageFile="~/POS/RestaurantMM.Master"
    AutoEventWireup="true" CodeBehind="frmRetailPos.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmRetailPos" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var tt = {};
        var lastFocused = null;
        var ItemDetails = null;
        var AddedItemList = new Array();
        var EditedItemList = new Array();
        var DeletedItemList = new Array();
        var PreviousBillItemList = new Array();
        var previousTotal;
        var previousPoint = 0.0;
        var DiscountDetails = new Array();
        var IsMembershipPaymentEnableFlag = false;



        $(document).ready(function () {
            $('#PointWiseMoney').val('0');
            $('#PointsInAmounts').val('0');
            $('#CustomerCode').val("");
            $('#CustomerPoints').val('0');


            if ($("#ContentPlaceHolder1_hfIsRestaurantIntegrateWithCompany").val() == "1") {
                $("#CompanyPaymentContainer").show();
            }
            else {
                $("#CompanyPaymentContainer").hide();
            }


            //$("#companyInfo").hide();

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
                    window.location = "/POS/Login.aspx";
                }, 2000);
            }

            //debugger;
            var costcenter = $("#ContentPlaceHolder1_hfCostcenterId").val();
            //var costcenter = 11;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../POS/frmRetailPos.aspx/GetAllDiscount',
                data: "{ 'costcenter':'" + costcenter + "'}",
                dataType: "json",
                success: function (data) {
                    //debugger;
                    DiscountDetails = data.d;

                    //$("#txtDiscountAmount").val(data.d.Discount);

                },
                error: function (result) {
                    //alert("Error");
                }
            });



            //$("#PointsInAmounts").keypress(function (event) {

            //    var keycode = event.keyCode || event.which;
            //    if (keycode == '13') {
            //        var Amount = parseFloat($('#PointsInAmounts').val());
            //        var point = parseFloat($('#PointWiseMoney').val());
            //        $('#txtPointAmount').val("");
            //        debugger;
            //        if(Amount > point )
            //            toastr.info("Amount can not be grater than Point.")
            //        else
            //        {
            //            $('#txtPointAmount').val(Amount);
            //        }
            //    }
            //});



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

            $("#ItemName").autocomplete({
                source: function (request, response) {

                    var itemCode = "";
                    var categoryName = "";

                    if ($.trim(request.term) == "") { return false; }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../POS/frmRetailPos.aspx/GetItemByCodeCategoryNameWiseItemDetailsForAutoSearch',
                        data: "{'itemCode':'" + itemCode + "','itemName':'" + $.trim(request.term) + "','categoryName':'" + categoryName + "'}",
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
                                    IsItemEditable: m.IsItemEditable
                                };
                            });

                            if (data.d == "") {
                                toastr.info("Item Not Found. Please Give Valid Item Name");
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
                }
            });

            $("#ItemName").keypress(function (event) {

                var keycode = event.keyCode || event.which;
                if (keycode == '13') {
                    AddItem();
                }
            });


            $("#CustomerCode").keypress(function (event) {
                var customerCode = $('#CustomerCode').val();
                var keycode = event.keyCode || event.which;
                if (keycode == '13') {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../POS/frmRetailPos.aspx/GetPointsByCustomerCode',
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

            $("#ItemCode").keypress(function (event) {
                //debugger;
                var keycode = event.keyCode || event.which;
                if (keycode == '13') {

                    var barCode = $.trim($("#ItemCode").val());

                    if ($.trim(barCode) == "") { return false; }

                    var alreadyAddedItem = _.findWhere(AddedItemList, { Code: barCode });

                    if (alreadyAddedItem == null) {
                        AdditemByCodeOrBarCode();
                    }
                    else {
                        ItemDetails = alreadyAddedItem;
                        AddItem();
                    }
                    setTimeout(function () { $("#ItemCode").focus(); }, 50);

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

        function AddRemarks() {

            // debugger;
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
                IsInvoiceVatAmountEnable: ($("#ContentPlaceHolder1_hfIsVatEnable").val() == "0" ? false : true),
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
                url: "frmRetailPos.aspx/FullBillRefundSettlement",
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

                $("#txtSearchCompany").val("");
                $("#txtCompanyAddress").val("");
                $("#txtContactNumber").val("");
                $("#txtBalance").val("");
                $("#txtCompanyPayment").val("");
                $("#ContentPlaceHolder1_txtRemarks").val("");
                PaymentCalculation(0);
                if ($("#CompanyInfoDialog").is(":visible")) {
                    $("#CompanyInfoDialog").dialog("close");
                }

            }

        }

        function LoadCompanyInfo() {

            var companyName = "";

            $("#CompanyInfoDialog").dialog({
                width: 600,
                height: 280,
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
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: 'frmOrderManagement.aspx/GetGuestCompanyInfo',
                        data: "{'companyName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    Balance: m.Balance,
                                    CompanyAddress: m.CompanyAddress,
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
                    $("#txtCompanyAddress").val(ui.item.CompanyAddress);
                    $("#txtContactNumber").val(ui.item.ContactNumber);
                    $("#txtBalance").val(ui.item.Balance);
                }
            });

        }

        function AdditemByCodeOrBarCode() {

            var itemCode = "", itemName = "", categoryName = "";
            itemCode = $("#ItemCode").val();

            if ($.trim(itemCode) == "") { return false; }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../POS/frmRetailPos.aspx/GetItemByCodeCategoryNameWiseItemDetailsForAutoSearch',
                data: "{'itemCode':'" + itemCode + "','itemName':'" + itemName + "','categoryName':'" + categoryName + "'}",
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
                        StockQuantity: data.d[0].StockQuantity
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
            //debugger;

            if (ItemDetails == null)
                return;

            var tr = "", quantity = 1, totalQuantity = 0.00, itemCode = "", kotId = "0", kotDetailId = "0";
            itemCode = $("#ItemCode").val();

            var alreadyAddedItem = _.findWhere(AddedItemList, { ItemId: ItemDetails.ItemId });
            var index = 0;

            if ($("#ContentPlaceHolder1_hfResumedKotId").val() != "") {
                kotId = $("#ContentPlaceHolder1_hfResumedKotId").val();
            }

            if (alreadyAddedItem != null) {
                index = _.findIndex(AddedItemList, { ItemId: ItemDetails.ItemId });

                var trAlreadyAdded = $("#AddedItem tbody tr:eq(" + index + ")");
                var addedQuantity = 0.00, addedUnitPrice = 0.00, addedTotalPrice = 0.00, totalDiscount = 0.0, unitDiscount = 0.0;

                addedQuantity = parseFloat($(trAlreadyAdded).find("td:eq(4)").find("input").val()) + quantity;
                addedUnitPrice = parseFloat($.trim($(trAlreadyAdded).find("td:eq(5)").find("input").val()));
                addedTotalPrice = toFixed((addedQuantity * addedUnitPrice), 2);
                unitDiscount = parseFloat($.trim($(trAlreadyAdded).find("td:eq(13)").text()));

                if (isNaN(unitDiscount)) {
                    unitDiscount = 0;
                }
                totalDiscount = toFixed((addedQuantity * unitDiscount), 2);
                $(trAlreadyAdded).find("td:eq(14)").text(totalDiscount);

                $(trAlreadyAdded).find("td:eq(4)").find("input").val(addedQuantity);
                $(trAlreadyAdded).find("td:eq(6)").text(addedTotalPrice);

                ClearItemSearchNAddItemDetails();
                SalesNDiscountCalculation();
                PaymentCalculation(0);


                return false;
            }

            AddedItemList.push({
                KotDetailId: kotDetailId,
                KotId: kotId,
                ItemId: ItemDetails.ItemId,
                ItemName: ItemDetails.Name,
                Code: ItemDetails.Code,
                UnitHead: ItemDetails.UnitHead,
                UnitPriceLocal: ItemDetails.UnitPriceLocal,
                StockBy: ItemDetails.StockBy,
                CategoryId: ItemDetails.CategoryId,
                DiscountType: ItemDetails.DiscountType,
                DiscountAmount: ItemDetails.DiscountAmount
            });

            var rowLength = $("#AddedItem tbody tr").length;
            totalQuantity = quantity * ItemDetails.UnitPriceLocal;

            //if (rowLength % 2 == 0) {
            //    tr += "<tr style='background-color:#FFFFFF;'>";
            //}
            //else {
            //    tr += "<tr style='background-color:#E3EAEB;'>";
            //}

            tr += "<tr>";

            tr += "<td style='width:12%;'>" + ItemDetails.Code + "</td>";
            tr += "<td style='width:30%;'>" + ItemDetails.label + "</td>";
            tr += "<td style='width:10%;'>" + ItemDetails.StockQuantity + "</td>";
            tr += "<td style='width:10%;'>" + ItemDetails.UnitHead + "</td>";
            tr += "<td style='width:8%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + quantity + "' onblur='CheckQuantity(this)' />" + "</td>";
            tr += "<td style='width:12%;'>" + "<input type='text' class='form-control text-right quantitydecimal' value='" + ItemDetails.UnitPriceLocal + "' onblur='CheckQuantity(this)' />" + "</td>";

            tr += "<td style='width:12%;' class='text-right'>" + totalQuantity + "</td>";

            tr += "<td style='width:6%;' class='text-center'>"
            tr += "<a href='javascript:void(0)' onclick= 'DeleteItemOrder(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";

            tr += "<td style='display:none'>" + ItemDetails.ItemId + "</td>";
            tr += "<td style='display:none'>" + ItemDetails.StockBy + "</td>";
            tr += "<td style='display:none'>" + ItemDetails.CategoryId + "</td>";
            tr += "<td style='display:none'>" + kotDetailId + "</td>";
            tr += "<td style='display:none'>0</td>";
            tr += "<td style='display:none'>0</td>";//unit discount
            tr += "<td style='display:none'>0</td>";//total discount

            tr += "</td>";
            tr += "</tr>";

            $("#AddedItem tbody").append(tr);
            CommonHelper.ApplyDecimalValidation();
            //debugger;
            IsDiscountApplicable();
            ClearItemSearchNAddItemDetails();
            SalesNDiscountCalculation();
            PaymentCalculation(0);

            return false;
        }

        function IsDiscountApplicable() {
            //debugger;
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

            $(AddedItemDiscount).find("td:eq(13)").text(discountAmount);
            $(AddedItemDiscount).find("td:eq(14)").text(discountAmount);


        }

        function calculateTotalQuantity() {
            //debugger;
            var totalQuantity = 0.0, totalItem = 0.0;
            $("#AddedItem tbody tr").each(function () {
                totalQuantity = totalQuantity + parseFloat($(this).find("td:eq(4)").find("input").val() == null ? 0 : $(this).find("td:eq(4)").find("input").val());
                totalItem++;
            });
            //$('#lblTotalItemTypes').Text = "Total Item = " + totalItem;
            //$('#lblTotalItemQuantity').Text = "Total Quantity = " + totalQuantity;
            document.getElementById('ContentPlaceHolder1_lblTotalItemTypes').innerHTML = "Total Item : " + totalItem + "";
            document.getElementById('ContentPlaceHolder1_lblTotalItemQuantity').innerHTML = "Total Quantity : " + totalQuantity + "";

        }

        function SalesNDiscountCalculation() {
            calculateTotalQuantity();

            //if ($("#AddedItem tbody tr").length == 0) {
            //    toastr.info("Please Add Item First.");
            //    return false;
            //}

            var totalAmount = 0.00, amount = 0.00, index = 0;
            var discountAmount = 0.00, afterDiscountAmount = 0.00;
            var vat = 0.00, totalDiscountAmount = 0.00, grandTotal = 0.00, exchangeTotal = 0.00;
            var discountType = "Percentage", discount = 0.00;

            var discountType = "Percentage", discount = 0.00, roundedGrandTotal = 0.00;
            //discount = $("#txtDiscountAmount").val() == "" ? 0.00 : parseFloat($("#txtDiscountAmount").val());
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

            $("#AddedItem tbody tr").each(function () {

                amount = parseFloat($(this).find("td:eq(6)").text());

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

            if ($("#rbPercentageDiscount").is(":checked") == true && discount != 0) {
                discountType = "Percentage";
                discountAmount = totalAmount * (discount / 100.00);
            }
            else if ($("#rbFixedDiscount").is(":checked") == true && discount != 0) {
                discountType = "Fixed";
                discountAmount = discount;
            }
            totalDiscountAmount = discountAmount;


            var MaxItemDiscountTotal = 0.0;

            $("#AddedItem tbody tr").each(function () {

                var itemDiscountAmount = parseFloat($(this).find("td:eq(14)").text());
                MaxItemDiscountTotal = MaxItemDiscountTotal + itemDiscountAmount;

            });
            totalDiscountAmount = MaxItemDiscountTotal;


            //afterDiscountAmount = totalAmount - discountAmount;
            afterDiscountAmount = totalAmount - totalDiscountAmount;

            var IsInclusiveBill = 0, vatAmount = 0.00, IsVatEnable = 1;

            IsInclusiveBill = parseInt($("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val(), 10);
            vatAmount = parseFloat($("#ContentPlaceHolder1_hfRestaurantVatAmount").val());
            IsVatEnable = parseFloat($("#ContentPlaceHolder1_hfIsVatEnable").val());

            if (IsInclusiveBill == 0) {
                vat = parseFloat((((afterDiscountAmount) * vatAmount) / (100)) * IsVatEnable);
                grandTotal = afterDiscountAmount + vat;
            }
            else {
                vat = parseFloat(((afterDiscountAmount * vatAmount) / (100 + vatAmount)) * IsVatEnable);
                grandTotal = afterDiscountAmount;
                //ServiceRate = parseFloat(discountedAmount - ServiceCharge - vat);
            }

            $("#txtTotalSales").val(toFixed(totalAmount, 2));
            $("#txtDiscountAmount").val(toFixed(totalDiscountAmount, 2));
            $("#txtAfterDiscountAmount").val(toFixed(afterDiscountAmount, 2));
            $("#txtVat").val(toFixed(vat, 2));
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
            if ($.trim($(control).val()) == "" || $.trim($(control).val()) == "0") {
                toastr.info("Please Give Proper Quantity.");
                $(control).val("1");
            }

            tt = control;

            var tr = $(control).parent().parent();
            var quantity = $.trim($(tr).find("td:eq(4)").find("input").val());
            var unitPrice = $.trim($(tr).find("td:eq(5)").find("input").val());

            var totalPrice = parseFloat(unitPrice) * parseFloat(quantity);
            $(tr).find("td:eq(6)").text(toFixed(totalPrice, 2));
            ItemDetails = {
                ItemId: +$.trim($(tr).find("td:eq(8)").text()),
                UnitPriceLocal: unitPrice,
                CategoryId: +$.trim($(tr).find("td:eq(10)").text())
            };


            IsDiscountApplicable();
            ItemDetails = null;

            unitDiscount = $.trim($(tr).find("td:eq(13)").text());
            totalDiscount = parseFloat(unitDiscount) * parseFloat(quantity);
            $(tr).find("td:eq(14)").text(toFixed(totalDiscount, 2));

            SalesNDiscountCalculation();
            PaymentCalculation(0);

            //toastr.info("CheckQuantity");
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
                    return false;
                }
                if (parseFloat(paymentTotal) > parseFloat(grandTotal)) {
                    $("#txtCompanyPayment").val("");
                    toastr.warning("Company Payment Amount Cannot Be Greater Than Due Amount.");
                    companyAmount = "0";
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
                    $(lastFocused).val(dueRChangeAmount);
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
            var itemId = parseInt($.trim($(tr).find("td:eq(8)").text()), 10);

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

        function BillSettlement() {

            PaymentCalculation(0);

            if ($("#txtCompanyPayment").val() != "") { // && $("#txtCompanyPayment").val() != "0"
                if ($("#ContentPlaceHolder1_hfCompanyId").val() == "" || $("#ContentPlaceHolder1_hfCompanyId").val() == "0") {
                    toastr.warning("Please Select Compnay To Give Company Payment.");
                    return false;
                }
            }

            isBillExchange = $("#ContentPlaceHolder1_hfIsBillExchange").val() == "0" ? false : true;
            if (!isBillExchange) {
                if ($("#AddedItem tbody tr").length == 0) { toastr.info("Please Add Item For Settlement."); return false; }
            }
            if ($.trim($("#DueRChnageAmount").text()) == "Due/Change") { toastr.warning("Please Payment For The Bill."); return false; }
            else if ($.trim($("#DueRChnageAmount").text()) == "Due Amount" && parseFloat($("#txtDueRChnage").val()) != 0) { toastr.warning("Please Payment For The Bill."); return false; }

            var grandTotal = $("#txtGrandTotal").val() == "" ? 0 : parseFloat($("#txtGrandTotal").val());
            var roundedTotal = $("#txtRoundedGrandTotal").val() == "" ? 0 : parseFloat($("#txtRoundedGrandTotal").val());
            //if (roundedTotal < 0) { toastr.info("Excahnge amount cannot be refunded."); return false; }
            //


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

                //if (remarks == "") {
                //    toastr.info("Please input the Remarks.")
                //    return false;
                //}
                refundRemarks = remarks;
                return false;
            }


            //



            var isBillExchange = false;
            isBillExchange = $("#ContentPlaceHolder1_hfIsBillExchange").val() == "0" ? false : true;

            if (isBillExchange) {

                var exchangeVal = 0, total = 0.00;
                $("#PreviousBillItem tbody > tr").each(function () {
                    exchangeVal = $(this).find("td:eq(4)").find('input').val();
                    total = total + (exchangeVal == "" ? 0.00 : parseFloat(exchangeVal));
                });

                if (total == 0) {
                    toastr.info("Please Give Exchange Quantity.");
                    return false;
                }
            }

            var kotDetailId = "0", kotId = "0", itemId, itemCode, itemName, stockBy, quantity, unitPrice,
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
                else if (kotDetailId != "0" && quantity != dbQuantity) {
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

            var invoiceDiscount = 0.0;

            if ($("#PreviousBillItem tbody tr").length > 0) {
                var index = 0;
                $("#PreviousBillItem tbody tr").each(function () {

                    kotDetailId = $(this).find("td:eq(9)").text();
                    itemId = $(this).find("td:eq(7)").text();
                    stockBy = $(this).find("td:eq(8)").text();

                    itemCode = $(this).find("td:eq(0)").text();
                    itemName = $(this).find("td:eq(1)").text();

                    quantity = $(this).find("td:eq(4)").find("input").val();
                    unitPrice = $(this).find("td:eq(5)").text();
                    totalPrice = $(this).find("td:eq(6)").text();
                    dbQuantity = $(this).find("td:eq(10)").text();
                    invoiceDiscount = PreviousBillItemList[index].InvoiceDiscount / PreviousBillItemList[index].Quantity * quantity;
                    //arpon11
                    if (quantity != "0" && quantity != '') {
                        SalesReturnItem.push({
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
                            InvoiceDiscount: invoiceDiscount
                        });
                    }
                    index++;
                });

            }

            totalSales = $("#txtTotalSales").val();
            discountAmount = $("#txtDiscountAmount").val() == "" ? 0.00 : $("#txtDiscountAmount").val();
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

            var remarks = $("#txtRemarks").val();

            if (parseFloat(discountAmount) != "0" && parseFloat(discountAmount) != "")
                calculatedDiscountAmount = parseFloat(totalSales) - parseFloat(afterDiscountAmount);

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
                IsInvoiceVatAmountEnable: ($("#ContentPlaceHolder1_hfIsVatEnable").val() == "0" ? false : true),
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
                RefundRemarks: refundRemarks
            };

            if (companyId != "0") {
                RestaurantBill.TransactionType = 'Company';
                RestaurantBill.TransactionId = companyId;
                RestaurantBill.CustomerName = companyName;
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
                //var paymentAmount = $("#" + PaymentMode[row].Control).val();

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


            var MemberId = $("#ContentPlaceHolder1_hfMemberId").val() == '' ? 0 : +$("#ContentPlaceHolder1_hfMemberId").val();

            $.ajax({
                type: "POST",
                url: "frmRetailPos.aspx/BillSettlement",
                data: JSON.stringify({
                    kotId: kotId, memberId: MemberId,
                    RestaurantBill: RestaurantBill, BillPayment: BillPayment, BillDetails: BillDetails,
                    EditeDetails: EditedItemList, DeletedDetails: DeletedItemList, SalesReturnItem: SalesReturnItem
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d.IsSuccess == true) {
                        $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                        $("#companyInfo").show();
                        $("#companyPaymentDiv").removeClass("col-md-12").addClass("col-md-6");

                        if ($("#ContentPlaceHolder1_hfIsBillExchange").val() == "1") {
                            $('#btnPrinReturnBillPreview').trigger('click');
                        }
                        else {
                            $('#btnPrintPreview').trigger('click');
                        }

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

        function CompanyPayment() {
            //toastr.info($("#ContentPlaceHolder1_hfCompanyId").val());
            if ($("#ContentPlaceHolder1_hfCompanyId").val() != "") {
                $("#lblCompanyName").text($("#txtSearchCompany").val());
            }
            $("#CompanyInfoDialog").dialog('close');
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
                url: "frmRetailPos.aspx/BillHoldup",
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
                url: "frmRetailPos.aspx/GetHoldUpPosInfo",
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
                url: "frmRetailPos.aspx/GetOrderedItemByKotId",
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

            $("#ItemCode").focus();
            ItemDetails = null;
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

        function PrintDocumentFunc(printTemplate) {
            if (printTemplate == "1") {
                //$('#btnPrintPreview').trigger('click');
            }
            else if (printTemplate == "2") {
                //$('#btnPrintReportTemplate2').trigger('click');
            }
            else if (printTemplate == "3") {
                //$("#btnPrintReportTemplate3").trigger('click');
            }
            return true;
        }

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
                url: "frmRetailPos.aspx/BillEdit",
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
                        $("#txtCompanyAddress").val(result.d.guestCompanyBO.CompanyAddress);
                        $("#txtContactNumber").val(result.d.guestCompanyBO.ContactNumber);
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
            //debugger;
            PreviousBillSalesNDiscountCalculation();
            PaymentCalculation(0);
        }

        function PreviousBillSalesNDiscountCalculation() {
            //debugger;
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
            IsVatEnable = parseFloat($("#ContentPlaceHolder1_hfIsVatEnable").val());

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
            // debugger;
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
            //$('#lblTotalItemTypes').Text = "";
            //$('#lblTotalItemQuantity').Text = "";
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
            $("#txtRemarks").val("");
            $("#txtReturnItemTotal").val("");
            $("#txtReturnItemVatTotal").val("");

            $("#btnHoldUpList").attr("disabled", false);
            $("#btnHoldUp").attr("disabled", false);
            $("#exchaneItemTotalContainer").hide();
            $("#btnPreviousBillDialog").hide();
            //arpon123

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
            $("#txtRemarks").val("");
            $("#txtReturnItemTotal").val("");
            $("#txtReturnItemVatTotal").val("");
        }

    </script>
    <asp:HiddenField ID="hfIsRestaurantIntegrateWithCompany" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsVatEnable" runat="server" />
    <asp:HiddenField ID="hfIsMembershipPaymentEnable" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfPOSRefundConfiguration" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfRestaurantVatAmount" runat="server" />
    <asp:HiddenField ID="hfIsRestaurantBillInclusive" runat="server" />
    <asp:HiddenField ID="hfIsRestaurantBillAmountWillRound" runat="server" />
    <asp:HiddenField ID="hfLocalCurrencyId" runat="server" />
    <asp:HiddenField ID="hfResumedKotId" runat="server" />
    <asp:HiddenField ID="hfBillId" runat="server" />
    <asp:HiddenField ID="hfMemberId" runat="server" />
    <asp:HiddenField ID="hfMemberName" runat="server" />
    <asp:HiddenField ID="hfPrintBillId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsCostCenterWiseBillNumberGenerate" runat="server" Value="0" />
    <asp:HiddenField ID="hfBillPrefixCostcentrwise" runat="server" Value="" />
    <asp:HiddenField ID="hfCostcenterId" runat="server" />
    <asp:HiddenField ID="hfIsBillExchange" runat="server" Value="0" />
    <asp:HiddenField ID="hfReturnResumeBillNumber" runat="server" Value="" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value=""></asp:HiddenField>

    <div style="height: 555px; overflow-y: scroll; display: none" id="reportContainer">
        <rsweb:ReportViewer ID="rvTransactionShow" ShowFindControls="false" ShowWaitControlCancelLink="false"
            PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
            Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
            WaitMessageFont-Size="14pt" ShowPageNavigationControls="true" ZoomMode="FullPage"
            Height="2000" ClientIDMode="Static" ShowRefreshButton="false">
        </rsweb:ReportViewer>
    </div>

    <div style="display: none;">
        <asp:Button ID="btnPrintPreview" runat="server" OnClick="btnPrintPreview_Click" Text="Print Preview" ClientIDMode="Static" />
        <asp:Button ID="btnPrinReturnBillPreview" runat="server" Text="Return Bill Print" ClientIDMode="Static" OnClick="btnPrinReturnBillPreview_Click" />
    </div>

    <div class="row">
        <div class="col-sm-9 pos-border-y">

            <div class="panel panel-default">
                <div class="panel-body" style="padding: 0;">
                    <div class="row">

                        <div class="col-md-7">
                            <table id="RetaurantTableTop" class="table table-responsive">
                                <tbody>
                                    <tr>
                                        <td style="width: 35%;">Item Code / BarCode</td>
                                        <td style="width: 65%;">Item Name</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 35%;">
                                            <input type="text" class="form-control" id="ItemCode" placeholder="Code" /></td>
                                        <td style="width: 65%;">
                                            <input type="text" class="form-control" id="ItemName" placeholder="Name" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="col-md-5">
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
                <div class="col-md-12" style="height: 410px;">
                    <%--style="height: 70vh;"--%>
                    <div style="height: 400px; overflow-y: scroll;">
                        <table id="AddedItem" class="table table-bordered table-condensed table-hover table-responsive">
                            <thead>
                                <tr>
                                    <th style="width: 12%;">Code
                                    </th>
                                    <th style="width: 30%;">Item Name
                                    </th>
                                    <th style="width: 10%;">Stock</th>
                                    <th style="width: 10%;">Stock By
                                    </th>
                                    <th style="width: 8%;">Quantity
                                    </th>
                                    <th style="width: 12%;">Unit Price
                                    </th>
                                    <th style="width: 12%;">Total Price
                                    </th>
                                    <th style="width: 6%;">Action
                                    </th>
                                    <th style='display: none'>ItemId
                                    </th>
                                    <th style='display: none'>StockById
                                    </th>
                                    <th style='display: none'>CategoryId
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
                            <div class="form-group no-gutter">
                                <div class="col-md-6">
                                    <label for="TotalSales">
                                        Total Sales</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txtTotalSales" placeholder="Total Sales" disabled="disabled" />
                                </div>
                            </div>

                            <div class="form-group no-gutter" style="display: none">
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
                                <div class="col-md-6">
                                    <label for="DiscountAmount">
                                        Discount Amount</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control quantity" id="txtDiscountAmount" tabindex="1" placeholder="Discount Amount"
                                        onblur="CalculateDiscount()" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-6">
                                    <label for="AfterDiscountAmount">
                                        After Discount</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txtAfterDiscountAmount" placeholder="After Discount Amount" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-6">
                                    <label for="Vat">
                                        Vat Amount</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txtVat" placeholder="Vat Amount" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-6">
                                    <label for="GrandTotal">
                                        Grand Total</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txtGrandTotal" placeholder="Grand Total" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" style="display: none;">
                                <div class="col-md-6">
                                    <label for="txtReturnItemTotal">
                                        Exchange Item Vat Total</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txtReturnItemVatTotal" placeholder="Exchange Item Vat Total" disabled="disabled" />
                                </div>
                            </div>

                            <div class="form-group no-gutter" id="exchaneItemTotalContainer" style="display: none;">
                                <div class="col-md-6">
                                    <label for="txtReturnItemTotal">
                                        Exchange Item Total</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txtReturnItemTotal" placeholder="Exchange Item Total" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter" id="PointsInAmountsDiv" style="display: none;">
                                <div class="col-md-6">
                                    <label for="txtPointAmount">
                                        Point Amount</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control" id="txtPointAmount" placeholder="Point Amount" disabled="disabled" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-6">
                                    <label for="RoundedAmount" id="lblRoundedAmount">
                                        Rounded Amount(<span id="roundedSign">+/-</span>)</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control quantitydecimal" id="txtRoundedAmount" disabled="disabled" placeholder="Rounded Amount" onblur="RoundedAmountCalculation(this.value)" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-6">
                                    <label for="DiscoverCard">
                                        Rounded Grand Total</label>
                                </div>
                                <div class="col-md-6">
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
                            <div class="form-group no-gutter">
                                <div class="col-md-6">
                                    <label for="Cash">
                                        Cash</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control quantitydecimal" tabindex="2" id="txtCash" placeholder="Cash" onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-6">
                                    <label for="AmexCard">
                                        Amex Card</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control quantitydecimal" tabindex="3" id="txtAmexCard" placeholder="Amex Card" onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-6">
                                    <label for="MasterCard">
                                        Master Card</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control quantitydecimal" tabindex="4" id="txtMasterCard" placeholder="Master Card" onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-6">
                                    <label for="VisaCard">
                                        Visa Card</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control quantitydecimal" tabindex="5" id="txtVisaCard" placeholder="Visa Card" onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                </div>
                            </div>
                            <div class="form-group no-gutter">
                                <div class="col-md-6">
                                    <label for="DiscoverCard">
                                        Discover Card</label>
                                </div>
                                <div class="col-md-6">
                                    <input type="text" class="form-control quantitydecimal" tabindex="6" id="txtDiscoverCard" placeholder="Discover Card" onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                </div>
                            </div>


                            <div class="form-group no-gutter" id="CompanyPaymentContainer">
                                <label class="control-label col-sm-6" id="lblCompany">
                                    Company</label>
                                <div class="col-sm-6">
                                    <div class="row">
                                        <div class="col-sm-6" id="companyPaymentDiv">
                                            <input type="text" class="form-control quantitydecimal" tabindex="7" id="txtCompanyPayment" placeholder="Com." onfocus="lastFocused=this;" onblur="PaymentCalculation(this.value)" />
                                        </div>
                                        <div class="col-sm-6" id="companyInfo">
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
                            <div class="row no-gutters">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <asp:Label ID="Label13" runat="server" Text="Company Name" CssClass="control-label col-sm-3"></asp:Label>
                                            <div class="col-sm-9">
                                                <input type="text" class="form-control" id="txtSearchCompany" />
                                            </div>
                                        </div>
                                        <fieldset>
                                            <legend>Company Details</legend>
                                            <div class="form-group">
                                                <asp:Label ID="Label14" runat="server" Text="Company Address" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtCompanyAddress" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="Label15" runat="server" Text="Contact Number" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtContactNumber" />
                                                </div>
                                            </div>
                                            <div class="form-group" style="display: none;">
                                                <asp:Label ID="Label1511" runat="server" Text="Balance" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtBalance" />
                                                </div>
                                            </div>
                                        </fieldset>
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

                        <div class="form-group no-gutter">
                            <div class="col-md-6">
                                <label id="DueRChnageAmount" for="DueRChnage">
                                    Due/Change</label>
                            </div>
                            <div class="col-md-6">
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
                            <div class="col-md-2">
                                <input type="button" value="Due Amount" class="btn btn-primary" onclick="DueAmountProcess()" />
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
                                <input type="text" class="form-control" id="txtRemarks" placeholder="Remarks" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="posfooter">
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-4">
                                <input type="button" id="BillReturnExchangeButton" value="Bill Return/Exchange" class="btn btn-primary" onclick="EditBill()" />
                                <input type="button" id="btnPreviousBillDialog" value="Previous Bill" class="btn btn-primary" onclick="PreviousBillDialog()" style="display: none;" />
                                <input type="button" id="BillPrintButton" value="Bill Print" class="btn btn-primary" onclick="BillPrint()" />
                            </div>
                            <div class="col-md-4" style="display: none;">
                            </div>
                            <div class="col-md-4 pull-right">
                                <div class="pull-right">
                                    <input type="button" id="btnHoldUpList" value="Hold-Up List" class="btn btn-primary" onclick="GetHoldUpPosInfo()" />
                                    <input type="button" id="btnHoldUp" value="Hold-Up" class="btn btn-primary" onclick="BillHoldup()" />
                                    <input type="button" value="Clear" class="btn btn-primary" onclick="ClearForm()" />
                                    <input type="button" id="btnSettlement" value="Settlement" class="btn btn-primary" onclick="BillSettlement()" />
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
                <div style="height: 400px; overflow-y: scroll;">
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

        var IsShowBillReturnExchangeButton = false;
        //if ($("#ContentPlaceHolder1_hfIsRestaurantIntegrateWithCompany").val() == "1") {
        if(IsShowBillReturnExchangeButton == true){
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
    </asp:Content>