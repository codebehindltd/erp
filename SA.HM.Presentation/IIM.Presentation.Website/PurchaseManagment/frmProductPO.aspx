<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmProductPO.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmProductPO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var editedItem = "";
        var DeletedPurchaseOrderDetails = [];

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product Purchase Order</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                var msg = JSON.parse($("#InnboardMessageHiddenField").val());
                CommonHelper.AlertMessage(msg);
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();
        });

        $(document).ready(function () {

            if ($("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val() == "1") {
                CommonHelper.SpinnerOpen();
                FillForm($("#ContentPlaceHolder1_hfPOrderId").val(), $("#ContentPlaceHolder1_hfpurchaseOrderTemplate").val());
            }

            var txtReceivedByDate = '<%=txtReceivedByDate.ClientID%>'
            var txtReceivedByDate2 = '<%=txtReceivedByDate2.ClientID%>'
            var ddlProductId = '<%=ddlProductId.ClientID%>'
            var txtPurchasePrice = '<%=txtPurchasePrice.ClientID%>'
            var ddlPRNumber = '<%=ddlPRNumber.ClientID%>'
            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var lblCategory = '<%=lblCategory.ClientID%>'
            var prNumber = $('#' + ddlPRNumber).val();

            if (prNumber == "0") {
                $('#' + ddlCategory).val("0");
                $('#' + lblCategory).show()
                $('#' + ddlCategory).show()
            }
            else {
                $('#' + lblCategory).hide()
                $('#' + ddlCategory).hide()
            }

            if ($("#ContentPlaceHolder1_hfpurchaseOrderTemplate").val() == "1") {
                CommonHelper.SpinnerOpen();
                LoadProductList();
                CommonHelper.SpinnerClose();
            }

            $('#' + ddlPRNumber).change(function () {
                var prNumber = $('#' + ddlPRNumber).val();

                if (prNumber == "0") {
                    $('#' + ddlCategory).val("0");
                    $('#' + lblCategory).show()
                    $('#' + ddlCategory).show()

                    $("#ContentPlaceHolder1_ddlCostCentre").val("0");
                    $("#ContentPlaceHolder1_ddlStockBy").val("0");

                    $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
                    $("#ContentPlaceHolder1_ddlStockBy").attr("disabled", false);
                }
                else {
                    $('#' + lblCategory).hide()
                    $('#' + ddlCategory).hide()

                    $("#ContentPlaceHolder1_ddlCostCentre").val("0");
                    $("#ContentPlaceHolder1_ddlStockBy").val("0");

                    $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", true);
                    $("#ContentPlaceHolder1_ddlStockBy").attr("disabled", true);
                }
                LoadProductList();
            });

            $('#' + ddlCategory).change(function () {
                LoadProductList();
            });

            $('#' + ddlProductId).change(function () {
                var ProductId = $('#' + ddlProductId).val();
                SetSelectedItem(ProductId);
            });

            $('#btnAdd').click(function () {

                var receivedByDate = $('#ContentPlaceHolder1_txtReceivedByDate').val();
                var supplierId = $('#ContentPlaceHolder1_ddlSupplier').val();
                var itemId = $('#ContentPlaceHolder1_ddlProductId').val();
                var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                var purchasePrice = $('#ContentPlaceHolder1_txtPurchasePrice').val();
                var requsitionId = $('#ContentPlaceHolder1_ddlPRNumber').val();
                var quantity = $('#ContentPlaceHolder1_txtQuantity').val();
                var costCentreId = $("#ContentPlaceHolder1_ddlCostCentre").val();
                var stockById = $("#ContentPlaceHolder1_ddlStockBy").val();

                if (receivedByDate == "") {
                    toastr.warning('Please Provide Received Date.');
                    return;
                }
                else if (supplierId == "0") {
                    toastr.warning('Please Select Supplier Information.');
                    return;
                }
                else if (itemId == "0") {
                    toastr.warning('Please Select Product Id.');
                    return;
                }
                else if (purchasePrice == "") {
                    toastr.warning('Please give purchase price.');
                    return;
                }
                else if (quantity == "") {
                    toastr.warning('Please give quantity.');
                    return;
                }
                else if (parseInt(quantity, 10) <= 0) {
                    toastr.warning('Please give quantity.');
                    return;
                }
                else if (purchasePrice <= 0) {
                    toastr.warning('Please give purchase price.');
                    return;
                }
                else if (costCentreId == "0") {
                    toastr.warning('Please Select Cost Center.');
                    return false;
                }
                else if (stockById == "0") {
                    toastr.warning('Please Select Stock By Id.');
                    return false;
                }

                CommonHelper.SpinnerOpen();

                var stockBy = $("#ContentPlaceHolder1_ddlStockBy option:selected").text();
                var itemName = $("#ContentPlaceHolder1_ddlProductId option:selected").text();
                var orderType = '', totalPrice = 0, purchaseOrderId = "0", purchaseOrderDetailsId = "0";

                totalPrice = parseFloat(purchasePrice) * parseFloat(quantity);

                if (requsitionId == '0') {
                    orderType = 'Ad Hoc';
                }
                else if (requsitionId != '0') {
                    orderType = $("#ContentPlaceHolder1_ddlPRNumber option:selected").text();
                }

                purchaseOrderId = $("#ContentPlaceHolder1_hfPOrderId").val();

                if (purchaseOrderId != "0" && editedItem != "") {

                    purchaseOrderDetailsId = $(editedItem).find("td:eq(7)").text();
                    EditItem(itemName, quantity, stockBy, purchasePrice, totalPrice, orderType, purchaseOrderDetailsId, requsitionId, categoryId, itemId, costCentreId, stockById);
                    return;
                }
                else if (purchaseOrderId == "0" && editedItem != "") {

                    purchaseOrderDetailsId = $(editedItem).find("td:eq(7)").text();
                    EditItem(itemName, quantity, stockBy, purchasePrice, totalPrice, orderType, purchaseOrderDetailsId, requsitionId, categoryId, itemId, costCentreId, stockById);
                    return;
                }

                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);

                AddPurchaseOrderItem(itemName, quantity, stockBy, purchasePrice, totalPrice, orderType, purchaseOrderDetailsId, requsitionId, categoryId, itemId, costCentreId, stockById);

                $("#ContentPlaceHolder1_hfProductId").val("0");
                $('#ContentPlaceHolder1_ddlProductId').val("0");
                $("#ContentPlaceHolder1_ddlCostCentre").val("0");
                $("#ContentPlaceHolder1_ddlStockBy").val("0");
                $('#ContentPlaceHolder1_txtPurchasePrice').val("");
                $("#ContentPlaceHolder1_txtQuantity").val("");
                $('#ContentPlaceHolder1_txtlblRequisitionQuantity').val("");
                $('#ContentPlaceHolder1_txtlblPurchaseQuantity').val("");

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

            $('#' + txtReceivedByDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat               
            });

            $('#' + txtReceivedByDate2).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat               
            });            
        });

        function AddPurchaseOrderItem(itemName, quantity, stockBy, purchasePrice, totalPrice, orderType, purchaseOrderDetailsId, requsitionId, categoryId, itemId, costCentreId, stockById) {

            var isEdited = "0";
            var rowLength = $("#POrderProductGrid tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:24%;'>" + itemName + "</td>";
            tr += "<td style='width:12%;'>" + quantity + "</td>";
            tr += "<td style='width:15%;'>" + stockBy + "</td>";
            tr += "<td style='width:14%;'>" + purchasePrice + "</td>";
            tr += "<td style='width:15%;'>" + totalPrice + "</td>";
            tr += "<td style='width:12%;'>" + orderType + "</td>";
            tr += "<td style='width:8%;'> <a onclick=\"javascript:return FIllForEdit(this);\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
            tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteItemOrder(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            tr += "<td style='display:none'>" + purchaseOrderDetailsId + "</td>";
            tr += "<td style='display:none'>" + requsitionId + "</td>";
            tr += "<td style='display:none'>" + categoryId + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + costCentreId + "</td>";
            tr += "<td style='display:none'>" + stockById + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";

            tr += "</tr>";

            $("#POrderProductGrid tbody").append(tr);
            TotalCostCalculation();

            CommonHelper.SpinnerClose();
        }

        function EditItem(itemName, quantity, stockBy, purchasePrice, totalPrice, orderType, purchaseOrderDetailsId, requsitionId, categoryId, itemId, costCentreId, stockById) {

            $(editedItem).find("td:eq(0)").text(itemName);
            $(editedItem).find("td:eq(1)").text(quantity);
            $(editedItem).find("td:eq(2)").text(stockBy);
            $(editedItem).find("td:eq(3)").text(purchasePrice);
            $(editedItem).find("td:eq(4)").text(totalPrice);
            $(editedItem).find("td:eq(5)").text(orderType);

            $(editedItem).find("td:eq(7)").text(purchaseOrderDetailsId);
            $(editedItem).find("td:eq(8)").text(requsitionId);
            $(editedItem).find("td:eq(9)").text(categoryId);
            $(editedItem).find("td:eq(10)").text(itemId);
            $(editedItem).find("td:eq(11)").text(costCentreId);
            $(editedItem).find("td:eq(12)").text(stockById);

            if (purchaseOrderDetailsId != "0")
                $(editedItem).find("td:eq(13)").text("1");

            $("#ContentPlaceHolder1_hfProductId").val("0");
            $('#ContentPlaceHolder1_ddlProductId').val("0");
            $("#ContentPlaceHolder1_ddlCostCentre").val("0");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $('#ContentPlaceHolder1_txtPurchasePrice').val("");
            $("#ContentPlaceHolder1_txtQuantity").val("");
            $('#ContentPlaceHolder1_txtlblRequisitionQuantity').val("");
            $('#ContentPlaceHolder1_txtlblPurchaseQuantity').val("");

            editedItem = "";
            $("#btnAdd").val("Add");
            TotalCostCalculation();

            CommonHelper.SpinnerClose();
        }

        function FIllForEdit(editItem) {

            CommonHelper.SpinnerOpen();

            editedItem = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAdd").val("Update Order");

            var requsitionId = $(tr).find("td:eq(8)").text();
            var categoryId = $(tr).find("td:eq(9)").text();
            var itemId = $(tr).find("td:eq(10)").text();
            var costCentreId = $(tr).find("td:eq(11)").text();
            var stockById = $(tr).find("td:eq(12)").text();
            var quantity = $.trim($(tr).find("td:eq(1)").text());
            var purchasePrice = $.trim($(tr).find("td:eq(3)").text());

            if (requsitionId == "0") {
                $('#ContentPlaceHolder1_ddlCategory').val(categoryId);
                $('#ContentPlaceHolder1_lblCategory').show();
                $('#ContentPlaceHolder1_ddlCategory').show();

                $("#ContentPlaceHolder1_ddlCostCentre").val("0");
                $("#ContentPlaceHolder1_ddlStockBy").val("0");
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlStockBy").attr("disabled", false);
            }
            else {
                $('#ContentPlaceHolder1_lblCategory').hide();
                $('#ContentPlaceHolder1_ddlCategory').hide();
                $('#ContentPlaceHolder1_ddlCategory').val("0");
                $("#ContentPlaceHolder1_ddlCostCentre").val("0");
                $("#ContentPlaceHolder1_ddlStockBy").val("0");
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlStockBy").attr("disabled", true);
            }

            $("#ContentPlaceHolder1_ddlCategory").val(categoryId);
            $("#ContentPlaceHolder1_hfProductId").val(itemId);
            $('#ContentPlaceHolder1_ddlProductId').val(itemId);
            $("#ContentPlaceHolder1_ddlCostCentre").val(costCentreId);
            $("#ContentPlaceHolder1_ddlStockBy").val(stockById);
            $('#ContentPlaceHolder1_txtPurchasePrice').val(purchasePrice);
            $("#ContentPlaceHolder1_txtQuantity").val(quantity);

            if ($("#ContentPlaceHolder1_ddlPRNumber").val() != requsitionId) {
                $("#ContentPlaceHolder1_ddlPRNumber").val(requsitionId);
                LoadProductList();
            }

            CommonHelper.SpinnerClose();
        }

        function TotalCostCalculation() {

            var totalCost = 0;

            $("#POrderProductGrid tbody tr").each(function () {
                totalCost += parseFloat($(this).find("td:eq(4)").text());
            });

            $("#ContentPlaceHolder1_lblTotalCalculateAmount").text(totalCost);
        }

        function DeleteItemOrder(deleteItem) {

            if (!confirm("Do you want to delete?")) {
                return;
            }

            var pOrderId = "0", purchaseOrderDetailsId = "0";
            var tr = $(deleteItem).parent().parent();

            purchaseOrderDetailsId = $(tr).find("td:eq(7)").text();
            pOrderId = $("#ContentPlaceHolder1_hfPOrderId").val();

            if ((purchaseOrderDetailsId != "0")) {
                DeletedPurchaseOrderDetails.push({
                    DetailId: purchaseOrderDetailsId,
                    POrderId: pOrderId
                });
            }

            $(deleteItem).parent().parent().remove();

            if ($("#POrderProductGrid tbody tr").length == 0 && $("#ContentPlaceHolder1_hfPOrderId").val() == "0") {
                $("#ContentPlaceHolder1_ddlCostCentre").val('0');
                $("#ContentPlaceHolder1_hfPOrderId").val('0');
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
            }
        }

        function FillForm(pOrderId, purchaseOrderTemplate) {

            $("#ContentPlaceHolder1_hfpurchaseOrderTemplate").val(purchaseOrderTemplate);

            if (purchaseOrderTemplate == "1")
                PageMethods.FillForms(pOrderId, OnFillFormSucceed, OnFillFormFailed);
            else if (purchaseOrderTemplate == "2")
                PageMethods.FillFormForTemplate2(pOrderId, OnFillFormSucceed, OnFillFormFailed);

            return false;
        }
        function OnFillFormSucceed(result) {

            CommonHelper.SpinnerOpen();
            if (result != null) {

                $("#ContentPlaceHolder1_hfPOrderId").val(result.PurchaseOrder.POrderId);

                if ($("#ContentPlaceHolder1_hfpurchaseOrderTemplate").val() == "1") {

                    $("#ContentPlaceHolder1_txtRemarks").val(result.PurchaseOrder.Remarks);

                    $("#<%=btnSave.ClientID %>").val("Update");
                    $("#POrderProductGrid tbody").html("");

                    $('#ContentPlaceHolder1_ddlSupplier').val(result.PurchaseOrder.SupplierId);
                    $("#ContentPlaceHolder1_txtReceivedByDate").val(GetStringFromDateTime(result.PurchaseOrder.ReceivedByDate));

                    var rowLength = result.PurchaseOrderDetails.length;
                    var row = 0, orderType = "";

                    for (row = 0; row < rowLength; row++) {

                        $('#ContentPlaceHolder1_ddlPRNumber option').filter(function (index) {
                            if ($.trim($(this).val()) === result.PurchaseOrderDetails[row].RequisitionId.toString()) {
                                orderType = $(this).text();
                            }
                        });

                        if (result.PurchaseOrderDetails[row].RequisitionId.toString() == "0") {
                            orderType = "Ad Hoc";
                        }

                        AddPurchaseOrderItem(result.PurchaseOrderDetails[row].ProductName, result.PurchaseOrderDetails[row].Quantity,
                                      result.PurchaseOrderDetails[row].StockBy, result.PurchaseOrderDetails[row].PurchasePrice,
                                      (result.PurchaseOrderDetails[row].Quantity * result.PurchaseOrderDetails[row].PurchasePrice),
                                      orderType, result.PurchaseOrderDetails[row].DetailId, result.PurchaseOrderDetails[row].RequisitionId,
                                      0, result.PurchaseOrderDetails[row].ProductId, result.PurchaseOrderDetails[row].CostCenterId,
                                      result.PurchaseOrderDetails[row].StockById);

                    }

                    $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
                }
                else if ($("#ContentPlaceHolder1_hfpurchaseOrderTemplate").val() == "2") {

                    $("#ContentPlaceHolder1_txtRemarks2").val(result.PurchaseOrder.Remarks);
                    $("#ContentPlaceHolder1_ddlSupplier2").attr("disabled", true);
                    $("#<%=btnSave2.ClientID %>").val("Update");

                    $('#ContentPlaceHolder1_ddlSupplier2').val(result.PurchaseOrder.SupplierId);
                    $("#ContentPlaceHolder1_txtReceivedByDate2").val(GetStringFromDateTime(result.PurchaseOrder.ReceivedByDate));
                    $("#ContentPlaceHolder1_ddlCostCentre2").val(result.CostCenterId);

                    $("#PuchaseOrderGridContainer").html(result.PurchaseOrderGrid);
                }

                //$("#myTabs").tabs('select', 0);
                $("#myTabs").tabs({ active: 0 });
            }
            CommonHelper.SpinnerClose();
        }
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        function ValidationBeforeSave() {

            if ($("#POrderProductGrid tbody tr").length == 0) {
                toastr.warning("Please add purchase item.");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtReceivedByDate").val()) == "") {
                toastr.warning("Please give received date.");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_ddlSupplier").val()) == "0") {
                toastr.warning("Please select supplier.");
                return false;
            }

            CommonHelper.SpinnerOpen();

            var pOrderId = "0", receivedByDate = "0", supplierId = "0", isEdited = "0";
            var categoryId = "", itemId = "", costCenterId = "", stockById = "", requsitionId = "0";
            var purchaseOrderDetailsId = "0", quantity = "0", purchasePrice = "0", remarks = "";

            pOrderId = $("#ContentPlaceHolder1_hfPOrderId").val();
            supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            receivedByDate = $("#ContentPlaceHolder1_txtReceivedByDate").val();
            remarks = $("#ContentPlaceHolder1_txtRemarks").val();

            var AddedPurchaseOrderDetails = [], EditedPurchaseOrderDetails = [];

            var PurchaseOrder = {
                POrderId: pOrderId,
                ReceivedByDate: receivedByDate,
                SupplierId: supplierId,
                Remarks: remarks
            };

            $("#POrderProductGrid tbody tr").each(function (index, item) {

                purchaseOrderDetailsId = $.trim($(item).find("td:eq(7)").text());
                requsitionId = $.trim($(item).find("td:eq(8)").text());
                categoryId = $.trim($(item).find("td:eq(9)").text());
                itemId = $.trim($(item).find("td:eq(10)").text());
                costCenterId = $.trim($(item).find("td:eq(11)").text());
                stockById = $.trim($(item).find("td:eq(12)").text());

                quantity = $.trim($(item).find("td:eq(1)").text());
                purchasePrice = $.trim($(item).find("td:eq(3)").text());

                isEdited = $.trim($(item).find("td:eq(13)").text());

                if (purchaseOrderDetailsId == "0") {

                    AddedPurchaseOrderDetails.push({
                        DetailId: parseInt(purchaseOrderDetailsId, 10),
                        POrderId: parseInt(pOrderId, 10),
                        RequisitionId: parseInt(requsitionId, 10),
                        CostCenterId: parseInt(costCenterId, 10),
                        StockById: parseInt(stockById, 10),
                        ProductId: parseInt(itemId, 10),
                        PurchasePrice: parseFloat(purchasePrice),
                        Quantity: parseFloat(quantity)
                    });
                }
                else if (purchaseOrderDetailsId != "0" && isEdited != "0") {
                    EditedPurchaseOrderDetails.push({
                        DetailId: parseInt(purchaseOrderDetailsId, 10),
                        POrderId: parseInt(pOrderId, 10),
                        RequisitionId: parseInt(requsitionId, 10),
                        CostCenterId: parseInt(costCenterId, 10),
                        StockById: parseInt(stockById, 10),
                        ProductId: parseInt(itemId, 10),
                        PurchasePrice: parseFloat(purchasePrice),
                        Quantity: parseFloat(quantity)
                    });
                }
            });

            PageMethods.SavePurchaseOrder(PurchaseOrder, AddedPurchaseOrderDetails, EditedPurchaseOrderDetails, DeletedPurchaseOrderDetails, OnSavePurchaseOrderSucceed, OnSavePurchaseOrderFailed);

            return false;
        }
        function OnSavePurchaseOrderSucceed(result) {

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
        function OnSavePurchaseOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }

        function LoadProductList() {

            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var Category = $('#' + ddlCategory).val();
            var ddlPRNumber = '<%=ddlPRNumber.ClientID%>'
            var PRNumber = $('#' + ddlPRNumber).val();
            PageMethods.LoadProductListOnPONumberChange(Category, PRNumber, OnLoadProductListOnPONumberChangeSucceeded, OnLoadProductListOnPONumberChangeFailed);
            CommonHelper.SpinnerClose();
            return false;
        }

        function OnLoadProductListOnPONumberChangeSucceeded(result) {
            var list = result;
            var controlId = '<%=ddlProductId.ClientID%>';
            var control = $('#' + controlId);
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

            var product = $("#ContentPlaceHolder1_hfProductId").val();
            $('#' + controlId).val(product);
            CommonHelper.SpinnerClose();
            SetSelectedItem(product);
        }
        function OnLoadProductListOnPONumberChangeFailed(error) {
        }
        function SetSelectedItem(itemId) {

            var requisitionId = "0";
            if ($("#ContentPlaceHolder1_ddlPRNumber").val() != "0") {
                requisitionId = $("#ContentPlaceHolder1_ddlPRNumber").val();
            }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/PurchaseManagment/frmPMProductPO.aspx/GetPurchasePrice",
                data: "{'itemId':'" + itemId + "','requisitionId':'" + requisitionId + "'}",
                dataType: "json",
                success: OnSetSelected,
                error: function (result) {
                }
            });
        }
        function OnSetSelected(response) {
            if (response.d.PurchasePrice > 0) {
                $("#<%=txtPurchasePrice.ClientID %>").val(response.d.PurchasePrice);
            }

            if ($('#ContentPlaceHolder1_ddlPRNumber').val() != "0") {
                $("#<%=txtlblRequisitionQuantity.ClientID %>").val(response.d.Quantity);
                $("#<%=txtlblPurchaseQuantity.ClientID %>").val(response.d.PurchaseQuantity);

                $("#ContentPlaceHolder1_ddlCostCentre").val(response.d.CostCenterId);
                $("#ContentPlaceHolder1_ddlStockBy").val(response.d.StockById);

            }
            else {
                $("#<%=txtlblRequisitionQuantity.ClientID %>").val('');
                $("#<%=txtlblPurchaseQuantity.ClientID %>").val('');
                $("#ContentPlaceHolder1_ddlCostCentre").val(response.d.CostCenterId);
                $("#ContentPlaceHolder1_ddlStockBy").val(response.d.StockById);
            }
            var productId = $("#ContentPlaceHolder1_ddlProductId").val();
            if (productId > 0) {
                LoadCurrentStockQuantity(response.d.CostCenterId, productId);
            }
        }
        function LoadCurrentStockQuantity(costcenterId, productId) {
            PageMethods.LoadCurrentStockQuantity(costcenterId, productId, OnLoadCurrentStockSucceeded, OnLoadCurrentStockFailed);
        }
        function OnLoadCurrentStockSucceeded(result) {
            $("#ContentPlaceHolder1_txtCurrentStock").val(result[0].StockQuantity);
        }
        function OnLoadCurrentStockFailed(error) {
        }

        function ProductPODetails(pOrderId) {
            CommonHelper.SpinnerOpen();
            PageMethods.PerformLoadPMProductDetailOnDisplayMode(pOrderId, OnLoadPODetailsSucceeded, OnLoadPODetailsFailed);
            return false;
        }
        function OnLoadPODetailsSucceeded(result) {
            $("#DetailsPOGrid tbody").html("");
            var totalRow = result.length, row = 0, status = "";
            var tr = "";

            var status = result[row].Status;

            if (status == "Approved") {
                $("#buttonContainer").hide();
            }
            else {
                $("#buttonContainer").show();
            }

            for (row = 0; row < totalRow; row++) {

                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:30%;'>" + result[row].ProductName + "</td>";
                tr += "<td style='width:20%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:20%;'>" + result[row].StockBy + "</td>";
                tr += "<td style='width:15%;'>" + result[row].PurchasePrice + "</td>";
                tr += "<td style='width:15%;'>" + ((result[row].PurchasePrice) * (result[row].Quantity)) + "</td>";

                tr += "</tr>";

                $("#DetailsPOGrid tbody").append(tr);
                tr = "";
            }

            $("#DetailsPOGridContaiiner").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 800,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Purchase Order Details",
                show: 'slide'
            });

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadPODetailsFailed() { CommonHelper.SpinnerClose(); }

        function ClearPurchaseOrder() {
            $("#ContentPlaceHolder1_hfProductId").val("0");
            $('#ContentPlaceHolder1_ddlProductId').val("0");
            $("#ContentPlaceHolder1_ddlCostCentre").val("0");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $('#ContentPlaceHolder1_txtPurchasePrice').val("");
            $("#ContentPlaceHolder1_txtQuantity").val("");
            $('#ContentPlaceHolder1_txtlblRequisitionQuantity').val("");
            $('#ContentPlaceHolder1_txtlblPurchaseQuantity').val("");

            $("#btnAdd").val("Add");
        }

        function PerformClearAction() {

            $("#<%=txtReceivedByDate.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#ContentPlaceHolder1_ddlPRNumber").val('0');

            DeletedPurchaseOrderDetails = [];

            $("#ContentPlaceHolder1_hfProductId").val("0");
            $('#ContentPlaceHolder1_ddlProductId').val("0");
            $("#ContentPlaceHolder1_ddlCostCentre").val("0");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $('#ContentPlaceHolder1_txtPurchasePrice').val("");
            $("#ContentPlaceHolder1_txtQuantity").val("");
            $('#ContentPlaceHolder1_txtlblRequisitionQuantity').val("");
            $('#ContentPlaceHolder1_txtlblPurchaseQuantity').val("");
            $('#ContentPlaceHolder1_hfIsEditedFromApprovedForm').val("0");
            $("#ContentPlaceHolder1_lblTotalCalculateAmount").text("");

            $("#POrderProductGrid tbody").html("");
            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);

            $("#<%=btnSave.ClientID %>").val("Save");

            return false;
        }

        // -------------------------------------------------------- Template 2 ---------------------------------------------------
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlCostCentre2").change(function () {
                OnSerchCriteriaChange();
            });
            $("#ContentPlaceHolder1_ddlSupplier2").change(function () {
                OnSerchCriteriaChange();
            });
            $("#ContentPlaceHolder1_ddlPRNumber2").change(function () {
                OnSerchCriteriaChange();
            });
            $("#ContentPlaceHolder1_ddlCategory2").change(function () {
                OnSerchCriteriaChange();
            });
        });

        function OnSerchCriteriaChange() {
            var supplierId = $("#ContentPlaceHolder1_ddlSupplier2").val();
            if (supplierId == "0") {
                ClearTableOnSearchCriteriaChange();
                toastr.warning("Please Select a Supplier.");
                return false;
            }
            var costCenterId = $("#ContentPlaceHolder1_ddlCostCentre2").val();
            if (costCenterId == "0") {
                ClearTableOnSearchCriteriaChange();
                toastr.warning("Please Select a Cost Center.");
                return false;
            }
            var categoryId = $("#ContentPlaceHolder1_ddlCategory2").val();
            var prNumber = $("#ContentPlaceHolder1_ddlPRNumber2").val();
            CommonHelper.SpinnerOpen();

            PageMethods.LoadItemForPurchaseBySupplier(supplierId, costCenterId, categoryId, prNumber, OnLoadItemForPurchaseSucceeded, OnLoadItemForPurchaseFailed);
            return false;
        }

        function OnLoadItemForPurchaseSucceeded(result) {
            if (result != "")
                $("#PuchaseOrderGridContainer").html(result);
            else
                toastr.info("Nothing Is found. Please Try later.");

            CommonHelper.SpinnerClose();
            return false;
        }

        function OnLoadItemForPurchaseFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error("Error Occured.");
        }

        function CheckInputValue(txtThis) {

            var tr = $(txtThis).parent().parent();
            var purchasePrice = parseFloat($(tr).find("td:eq(3)").find("input").val());
            var quantity = parseFloat($(tr).find("td:eq(4)").find("input").val());

            if (CommonHelper.IsDecimal(purchasePrice) == false) {
                toastr.warning("Please give valid purchase price value.");
                return false;
            }

            if (CommonHelper.IsDecimal(quantity) == false) {
                toastr.warning("Please give valid quantity value.");
                return false;
            }

            var pOrderDetailsId = $(tr).find("td:eq(6)").text();
            var previousQuantity = $(tr).find("td:eq(9)").text();


            var totalPrice = purchasePrice * quantity;
            $(tr).find("td:eq(5)").text(totalPrice.toFixed(2));

            if (pOrderDetailsId != "0" && parseFloat(previousQuantity) != 0 && parseFloat(previousQuantity) != quantity) {
                $(tr).find("td:eq(10)").text("1");
            }

            GrandTotalCalculation();
        }

        function GrandTotalCalculation() {

            var quantity = "", grandTotal = 0;

            $("#ProductPurchaseGrid tbody tr").each(function () {

                quantity = $(this).find("td:eq(5)").text();

                if (quantity != "") {
                    grandTotal += parseFloat(quantity);
                }
            });

            $("#ProductPurchaseGrid tfoot tr").find("td:eq(1)").text(grandTotal);
        }

        function ValidationBeforeSave2() {

            var IsPermitForSave = confirm("Do You Want To Save?");
            if (IsPermitForSave == true) {
                if ($("#ProductPurchaseGrid tbody tr").length == 0) {
                    toastr.warning("Please add purchase item.");
                    return false;
                }
                else if ($.trim($("#ContentPlaceHolder1_ddlCostCentre2").val()) == "") {
                    toastr.warning("Please select Costcenter.");
                    return false;
                }
                else if ($.trim($("#ContentPlaceHolder1_txtReceivedByDate2").val()) == "") {
                    toastr.warning("Please give received date.");
                    return false;
                }
                else if ($.trim($("#ContentPlaceHolder1_ddlSupplier2").val()) == "0") {
                    toastr.warning("Please select supplier.");
                    return false;
                }

                CommonHelper.SpinnerOpen();

                var pOrderId = "0", receivedByDate = "0", supplierId = "0", isEdited = "0";
                var categoryId = "", itemId = "", costCenterId = "", stockById = "", requsitionId = "0";
                var purchaseOrderDetailsId = "0", quantity = "0", purchasePrice = "0", remarks = "";

                pOrderId = $("#ContentPlaceHolder1_hfPOrderId").val();
                supplierId = $("#ContentPlaceHolder1_ddlSupplier2").val();
                costCenterId = $("#ContentPlaceHolder1_ddlCostCentre2").val();
                receivedByDate = $("#ContentPlaceHolder1_txtReceivedByDate2").val();
                remarks = $("#ContentPlaceHolder1_txtRemarks2").val();

                var AddedPurchaseOrderDetails = [], EditedPurchaseOrderDetails = [];

                var PurchaseOrder = {
                    POrderId: pOrderId,
                    ReceivedByDate: receivedByDate,
                    SupplierId: supplierId,
                    Remarks: remarks
                };

                $("#ProductPurchaseGrid tbody tr").each(function (index, item) {

                    purchaseOrderDetailsId = $.trim($(item).find("td:eq(6)").text());
                    itemId = $.trim($(item).find("td:eq(7)").text());
                    stockById = $.trim($(item).find("td:eq(8)").text());

                    quantity = $.trim($(item).find("td:eq(4)").find("input").val());

                    purchasePrice = $.trim($(item).find("td:eq(3)").find("input").val());
                    isEdited = $.trim($(item).find("td:eq(10)").text());

                    if (quantity != "" && quantity != "0") {
                        if (purchaseOrderDetailsId == "0") {

                            AddedPurchaseOrderDetails.push({
                                DetailId: parseInt(purchaseOrderDetailsId, 10),
                                POrderId: parseInt(pOrderId, 10),
                                CostCenterId: parseInt(costCenterId, 10),
                                StockById: parseInt(stockById, 10),
                                ProductId: parseInt(itemId, 10),
                                PurchasePrice: parseFloat(purchasePrice),
                                Quantity: parseFloat(quantity)
                            });
                        }
                        else if (purchaseOrderDetailsId != "0" && isEdited != "0") {
                            EditedPurchaseOrderDetails.push({
                                DetailId: parseInt(purchaseOrderDetailsId, 10),
                                POrderId: parseInt(pOrderId, 10),
                                CostCenterId: parseInt(costCenterId, 10),
                                StockById: parseInt(stockById, 10),
                                ProductId: parseInt(itemId, 10),
                                PurchasePrice: parseFloat(purchasePrice),
                                Quantity: parseFloat(quantity)
                            });
                        }
                    }
                    else { }

                });

                PageMethods.SavePurchaseOrder(PurchaseOrder, AddedPurchaseOrderDetails, EditedPurchaseOrderDetails, DeletedPurchaseOrderDetails, OnSavePurchaseOrderSucceed2, OnSavePurchaseOrderFailed2);

                return false;
            } else {
                return false;
            }
        }

        function OnSavePurchaseOrderSucceed2(result) {

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
        function OnSavePurchaseOrderFailed2(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        function PerformClearAction2() {

            $("#ContentPlaceHolder1_ddlCostCentre2").val("0");
            $("#ContentPlaceHolder1_ddlSupplier2").val("0");
            $("#ContentPlaceHolder1_txtReceivedByDate2").val("");
            $("#ContentPlaceHolder1_txtRemarks2").val("");

            $("#ProductPurchaseGrid tbody").html("");
            $("#ProductPurchaseGrid tfoot tr").find("td:eq(1)").text("0");
            $("#ProductPurchaseGrid").hide();
        }
        function ClearTableOnSearchCriteriaChange() {
            $("#ProductPurchaseGrid tbody").html("");
            $("#ProductPurchaseGrid tfoot tr").find("td:eq(1)").text("0");
            $("#ProductPurchaseGrid").hide();
        }

    </script>
    <div id="DetailsPOGridContaiiner" style="display: none;">
        <table id="DetailsPOGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 30%;">
                        Product Name
                    </th>
                    <th style="width: 15%;">
                        Quantity
                    </th>
                    <th style="width: 15%;">
                        Stock By
                    </th>
                    <th style="width: 20%;">
                        Purchase Price
                    </th>
                    <th style="width: 20%;">
                        Total Price
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hfPOrderId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfProductId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCostcenterId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsEditedFromApprovedForm" runat="server" Value="0" />
    <asp:HiddenField ID="hfpurchaseOrderTemplate" runat="server" Value="0" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Order Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Order</a></li>
        </ul>
        <div id="tab-1">
            <div id="POrderTemplate1" runat="server" class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblReceivedByDate" runat="server" class="control-label required-field" Text="Received By Date"></asp:Label>                            
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtReceivedByDate" CssClass="form-control" runat="server" TabIndex="1"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblSupplier" runat="server" class="control-label required-field" Text="Supplier"></asp:Label>                           
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control" TabIndex="2">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="Commissionformation" class="panel panel-default">
                        <div class="panel-heading">
                            Order Detail Information</div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblRNumber" runat="server" class="control-label" Text="Requisition Number"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlPRNumber" runat="server" CssClass="form-control" TabIndex="3">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Product Category"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" TabIndex="4">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblProduct" runat="server" class="control-label required-field" Text="Product"></asp:Label>                                       
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlProductId" runat="server" CssClass="form-control" TabIndex="5">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblRequisitionQuantity" runat="server" class="control-label" Text="Requisition Quantity"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtlblRequisitionQuantity" TabIndex="6" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group" id="costCenterNStockBy">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Cost Center"></asp:Label>                                        
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlCostCentre" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2" style="width: 158px;">
                                        <asp:Label ID="lblPurchaseQuantity" runat="server" class="control-label" Text="Already Purchase Quantity"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtlblPurchaseQuantity" TabIndex="9" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Stock By"></asp:Label>                                       
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlStockBy" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblPurchasePriceLocal" runat="server" class="control-label required-field" Text="Selling Price One"></asp:Label>                                        
                                        <asp:DropDownList ID="ddlPurchasePriceLocal" runat="server" CssClass="form-control"
                                            TabIndex="7" Visible="False">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtPurchasePrice" TabIndex="8" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblQuantity" runat="server" class="control-label required-field" Text="Purchase Quantity"></asp:Label>                                        
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TabIndex="10"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblCurrentStock" runat="server" class="control-label" Text="Current Stock Quantity"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtCurrentStock" TabIndex="8" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group" style="padding:5px 0 5px 0;">
                                    <%--Right Left--%>
                                    <input id="btnAdd" type="button" value="Add" tabindex="11" class="TransactionalButton btn btn-primary btn-sm" />
                                    <input id="btnCancelOrder" type="button" value="Cancel" tabindex="11" onclick="ClearPurchaseOrder()"
                                        class="TransactionalButton btn btn-primary btn-sm" />
                                </div>
                                <div class="form-group" id="POorderTableContainer">
                                    <table id="POrderProductGrid" class="table table-bordered table-condensed table-responsive"
                                        style="width: 100%;">
                                        <thead>
                                            <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                                <th style="width: 24%;">
                                                    Product Name
                                                </th>
                                                <th style="width: 12%;">
                                                    Quantity
                                                </th>
                                                <th style="width: 15%;">
                                                    Stock By
                                                </th>
                                                <th style="width: 14%;">
                                                    Purchase Price
                                                </th>
                                                <th style="width: 15%;">
                                                    Total Price
                                                </th>
                                                <th style="width: 12%;">
                                                    Order Type
                                                </th>
                                                <th style="width: 8%;">
                                                    Action
                                                </th>
                                                <th style="display: none">
                                                    hfDetailId
                                                </th>
                                                <th style="display: none">
                                                    Requsiotion Id
                                                </th>
                                                <th style="display: none">
                                                    Category Id
                                                </th>
                                                <th style="display: none">
                                                    Product Id
                                                </th>
                                                <th style="display: none">
                                                    Costcenter id
                                                </th>
                                                <th style="display: none">
                                                    Stock By Id
                                                </th>
                                                <th style="display: none">
                                                    Is Edited
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
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                TabIndex="12"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="13" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return ValidationBeforeSave();" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="14" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformClearAction();" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="POrderTemplate2" runat="server" class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Cost Center"></asp:Label>                            
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCostCentre2" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblReceivedByDate2" runat="server" class="control-label required-field" Text="Received By Date"></asp:Label>                            
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtReceivedByDate2" CssClass="form-control" runat="server" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSupplier1" runat="server" class="control-label required-field" Text="Supplier"></asp:Label>                            
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlSupplier2" runat="server" TabIndex="2" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblRNumber2" runat="server" class="control-label" Text="Requisition Number"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlPRNumber2" runat="server" CssClass="form-control" TabIndex="3">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblCategory2" runat="server" class="control-label" Text="Product Category"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCategory2" runat="server" CssClass="form-control" TabIndex="4">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <%--<div class="divClear">
                </div>--%>
                    <div class="form-group" style="padding:5px 0 5px 0;">
                        <div id="PuchaseOrderGridContainer">
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label5" runat="server" class="control-label" Text="Remarks"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRemarks2" runat="server" CssClass="form-control" TextMode="MultiLine"
                                TabIndex="12"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSave2" runat="server" Text="Save" TabIndex="13" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return ValidationBeforeSave2();" />
                            <asp:Button ID="btnClear2" runat="server" Text="Clear" TabIndex="14" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformClearAction2();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%-- </div>
            </div>--%>
        <div id="tab-2">
            <div id="InfoPanel" class="panel panel-default">
                <div class="panel-heading">
                    Order Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
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
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Submitted" Value="Pending"></asp:ListItem>
                                    <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
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
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    TabIndex="5" OnClick="btnSearch_Click" />
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
                    <asp:GridView ID="gvOrderInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                        OnRowCommand="gvOrderInfo_RowCommand" TabIndex="7" OnRowDataBound="gvOrderInfo_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("POrderId") %>'></asp:Label>
                                    <asp:Label ID="lblSupplierId" runat="server" Text='<%#Eval("SupplierId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PONumber" HeaderText="PO Number" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Order Date" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvOrderDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("CreatedDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Received By Date" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvReceivedByDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ReceivedByDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgDetailsRequisition" runat="server" CausesValidation="False"
                                        CommandName="CmdRequisitionDetails" CommandArgument='<%# bind("POrderId") %>'
                                        OnClientClick='<%#String.Format("return ProductPODetails({0})", Eval("POrderId")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Receive Details" />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("POrderId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        OnClientClick='<%#String.Format("return FillForm({0}, {1})", Eval("POrderId"), Eval("PurchaseOrderTemplate")) %>'
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("POrderId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
                                    &nbsp;&nbsp;<asp:ImageButton ID="ImgReportPO" runat="server" CausesValidation="False"
                                        CommandName="CmdReportPO" CommandArgument='<%# bind("POrderId") %>' ImageUrl="~/Images/ReportDocument.png"
                                        Text="" AlternateText="Invoice" ToolTip="Purchase Order Invoice" />
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
</asp:Content>
