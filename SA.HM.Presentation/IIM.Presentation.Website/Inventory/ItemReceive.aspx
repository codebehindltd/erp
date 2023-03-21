<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ItemReceive.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.ItemReceive" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="companyProjectUserControlSrc" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>


<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        var queryReceiveOrderId = "";
        var AddedSerialCount = 0;

        var CurrencyList = new Array();
        var PurchaseOrderList = new Array();
        var LCList = new Array();

        var ItemSelected = null;
        var ReceiveOrderItem = new Array();
        var ReceiveOrderItemDeleted = new Array();
        var ReceiveOrderItemFromPurchase = new Array();
        var NewAddedSerial = new Array();
        var AddedSerialzableProduct = new Array();
        var DeletedSerialzableProduct = new Array();
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            $("#ContentPlaceHolder1_ddlAccountHead").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlPMAccountHead").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            if (IsCanSave) {

                $('#btnSave').show();

            } else {

                $('#btnSave').hide();
            }

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                $("#AttributeDiv").show();
                $("#cId").show();
                $("#cPOId").show();
                $("#sId").show();
                $("#sPOId").show();
                $("#stId").show();
                $("#stPOId").show();
                $("#cIdd").show();
                $("#sIdd").show();
                $("#stIdd").show();
            }
            else {
                $("#AttributeDiv").hide();
                $("#cId").hide();
                $("#cPOId").hide();
                $("#sId").hide();
                $("#sPOId").hide();
                $("#stId").hide();
                $("#stPOId").hide();
                $("#cIdd").hide();
                $("#sIdd").hide();
                $("#stIdd").hide();
            }

            $('#ContentPlaceHolder1_ddlSizeAttribute').change(function () {

                GetInvItemStockInfoByItemAndAttributeId();
            });

            $('#ContentPlaceHolder1_ddlStyleAttribute').change(function () {

                GetInvItemStockInfoByItemAndAttributeId();
            });


            $('#ContentPlaceHolder1_ddlColorAttribute').change(function () {
                GetInvItemStockInfoByItemAndAttributeId();
            });

            $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany').change(function () {
                debugger;
                var CompanyId = $(this).val();
                if (CompanyId != null) {

                    PageMethods.LoadReceiveStoreByCompanyId(CompanyId, OnLoadReceiveStoreByCompanyIdSucceed, OnLoadReceiveStoreByCompanyIdFailed);
                }

                return false;
            });

            if ($('#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany').val() > 0) {
                var CompanyId = $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany').val();
                PageMethods.LoadReceiveStoreByCompanyId(CompanyId, OnLoadReceiveStoreByCompanyIdSucceed, OnLoadReceiveStoreByCompanyIdFailed);
            }

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfPurchaseOrderObj").val() !== "") {
                PurchaseOrderList = JSON.parse($("#ContentPlaceHolder1_hfPurchaseOrderObj").val());
            }
            if ($("#ContentPlaceHolder1_hfLcInfoObj").val() !== "") {
                LCList = JSON.parse($("#ContentPlaceHolder1_hfLcInfoObj").val());
            }

            if ($("#ContentPlaceHolder1_hfCurrencyObj").val() !== "") {
                CurrencyList = JSON.parse($("#ContentPlaceHolder1_hfCurrencyObj").val());
            }

            if ($("#ContentPlaceHolder1_ddlReceiveType").val() == "AdHoc") {
                $("#PurchaseType").show();
                $("#AdhocReceive").show();
                $("#AdhocReceiveItem").show();
                $("#ReceiveOrderItemContainer").hide();
                $("#PurchaseOrderCostCenterContainer").hide();
                $("#ContentPlaceHolder1_ddlCurrency").attr('disabled', false);
                if ($("#ContentPlaceHolder1_ddlPaymentType").val() == "CashNBank") {
                    $("#PMEntryPanel").show();
                    $("#SupplierPanel").hide();
                } else {
                    $("#PMEntryPanel").hide();
                    $("#SupplierPanel").show();
                }
            }
            else {
                $("#PurchaseType").hide();
                $("#AdhocReceive").hide();
                $("#AdhocReceiveItem").hide();
                $("#ReceiveOrderItemContainer").show();
                $("#PurchaseOrderCostCenterContainer").show();
                $("#ContentPlaceHolder1_ddlCurrency").attr('disabled', true);
                if ($("#ContentPlaceHolder1_ddlReceiveType").val() == "Purchase") {
                    $("#PONumber").show();
                    $("#LCNumber").hide();
                }
                else {
                    $("#PONumber").hide();
                    $("#LCNumber").show();
                }
            }

            $("#myTabs").tabs();

            queryReceiveOrderId = CommonHelper.GetParameterByName("rid");
            if (queryReceiveOrderId != "") {
                var pid = CommonHelper.GetParameterByName("pid");
                PageMethods.EditReceiveOrder(queryReceiveOrderId, pid, OnEditPurchaseOrderSucceed, OnEditPurchaseOrderFailed);
            }

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

            $("#ContentPlaceHolder1_ddlPurchaseOrderNumber").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlLCNumber").select2({
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

            $("#ContentPlaceHolder1_ddlReceiveLocation").select2({
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

            $("#ContentPlaceHolder1_ddlPaymentType").change(function () {
                if ($("#ContentPlaceHolder1_ddlPaymentType").val() == "CashNBank") {
                    $("#PMEntryPanel").show();
                    $("#SupplierPanel").hide();
                } else {
                    $("#PMEntryPanel").hide();
                    $("#SupplierPanel").show();
                }
            });

            $("#ContentPlaceHolder1_ddlReceiveType").change(function () {
                var id = $(this).val();
                if (id == "AdHoc") {
                    $("#PurchaseType").show();
                    $("#AdhocReceive").show();
                    $("#AdhocReceiveItem").show();
                    $("#ReceiveOrderItemContainer").hide();
                    $("#PurchaseOrderCostCenterContainer").hide();
                    $("#PONumber").hide();
                    $("#LCNumber").hide();
                    $("#ContentPlaceHolder1_ddlCurrency").attr('disabled', false);
                    $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false).val('0').trigger('change');
                    $("#ContentPlaceHolder1_ddlPaymentType").change(function () {
                        if ($("#ContentPlaceHolder1_ddlPaymentType").val() == "CashNBank") {
                            $("#PMEntryPanel").show();
                            $("#SupplierPanel").hide();
                        } else {
                            $("#PMEntryPanel").hide();
                            $("#SupplierPanel").show();
                        }
                    });
                }
                else {
                    $("#PurchaseType").hide();
                    $("#AdhocReceive").hide();
                    $("#AdhocReceiveItem").hide();
                    $("#ReceiveOrderItemContainer").show();
                    $("#PurchaseOrderCostCenterContainer").show();
                    $("#ContentPlaceHolder1_ddlCurrency").attr('disabled', true);
                    if ($("#ContentPlaceHolder1_ddlReceiveType").val() == "Purchase") {
                        $("#PONumber").show();
                        $("#LCNumber").hide();
                    }
                    else {
                        $("#PONumber").hide();
                        $("#LCNumber").show();
                    }
                }
            });

            $("#btnCancelOEAmount").click(function () {
                ClearOverheadExpenseInfo();
            });

            $("#btnAddOEAmount").click(function () {
                var accountHead = $("#ContentPlaceHolder1_ddlAccountHead option:selected").text();
                var accountHeadId = $("#ContentPlaceHolder1_ddlAccountHead").val();
                var amount = $.trim($("#ContentPlaceHolder1_txtAmount").val());
                var oEDescription = $.trim($("#ContentPlaceHolder1_txtOEDescription").val());
                if (accountHeadId == "0") {
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

                if (!IsAccountHeadExistsForOE(accountHeadId)) {
                    AddAccountHeadForOEInfo(accountHeadId, accountHead, amount, oEDescription, AccountHeadDetailsId, isEdited);

                    $("#ContentPlaceHolder1_txtAmount").val("");
                    $("#ContentPlaceHolder1_txtOEDescription").val("");
                    $("#ContentPlaceHolder1_ddlAccountHead").val("0").trigger('change');
                    $("#ContentPlaceHolder1_ddlAccountHead").focus();
                }
            });

            function IsAccountHeadExistsForOE(accountHeadId) {
                var IsDuplicate = false;
                $("#OEAmountGrid tr").each(function (index) {

                    if (index !== 0 && !IsDuplicate) {
                        var accountHeadIdValueInTable = $(this).find("td").eq(5).html();

                        var IsAccountHeadIdFound = accountHeadIdValueInTable.indexOf(accountHeadId) > -1;
                        if (IsAccountHeadIdFound) {
                            toastr.warning('Account Head Already Added.');
                            IsDuplicate = true;
                            return true;
                        }
                    }
                });

                return IsDuplicate;
            }

            $("#btnCancelPMAmount").click(function () {
                ClearPaymentMethodInfo();
            });

            $("#btnAddPMAmount").click(function () {
                var accountHeadPM = $("#ContentPlaceHolder1_ddlPMAccountHead option:selected").text();
                var accountHeadPMId = $("#ContentPlaceHolder1_ddlPMAccountHead").val();
                var amountPM = $.trim($("#ContentPlaceHolder1_txtPMAmount").val());
                var pMDescription = $.trim($("#ContentPlaceHolder1_txtPMDescription").val());
                if (accountHeadPMId == "0") {
                    toastr.warning("Please select Payment Method Account Head.");
                    $("#ContentPlaceHolder1_ddlPMAccountHead").focus();
                    return false;
                }
                else if (amountPM == "") {
                    toastr.warning("Please give Amount from Payment Method.");
                    $("#ContentPlaceHolder1_txtPMAmount").focus();
                    return false;
                }
                else if (pMDescription == "") {
                    toastr.warning("Please Give Payment Method Description.");
                    $("#ContentPlaceHolder1_txtPMDescription").focus();
                    return false;
                }

                var AccountHeadPMDetailsId = "0", isEditedPM = 0, editedItemIdPM = "0";

                if (!IsAccountHeadExistsForPM(accountHeadPMId)) {
                    AddAccountHeadForPMInfo(accountHeadPMId, accountHeadPM, amountPM, pMDescription, AccountHeadPMDetailsId, isEditedPM);

                    $("#ContentPlaceHolder1_txtPMAmount").val("");
                    $("#ContentPlaceHolder1_txtPMDescription").val("");
                    $("#ContentPlaceHolder1_ddlPMAccountHead").val("0").trigger('change');
                    $("#ContentPlaceHolder1_ddlPMAccountHead").focus();
                }
            });

            function IsAccountHeadExistsForPM(accountHeadPMId) {
                var IsDuplicate = false;
                $("#PMAmountGrid tr").each(function (index) {

                    if (index !== 0 && !IsDuplicate) {
                        var accountHeadIdValueInTable = $(this).find("td").eq(5).html();

                        var IsAccountHeadIdFound = accountHeadIdValueInTable.indexOf(accountHeadPMId) > -1;
                        if (IsAccountHeadIdFound) {
                            toastr.warning('Account Head Already Added.');
                            IsDuplicate = true;
                            return true;
                        }
                    }
                });

                return IsDuplicate;
            }


            $("#ContentPlaceHolder1_ddlPurchaseOrderNumber").change(function () {

                LoadItemForReceive(this);
            });
            $("#ContentPlaceHolder1_ddlLCNumber").change(function () {

                LoadItemForReceive(this);
            });

            $("#ContentPlaceHolder1_ddlCostCentre").change(function () {
                if ($(this).val() != "0")
                    LoadStoreLocationByCostCenter($(this).val());
            });

            $("#ContentPlaceHolder1_ddlCostCentre").trigger('change');

            $("#ContentPlaceHolder1_txtItem").autocomplete({
                minLength: 3,
                source: function (request, response) {
                    var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                    var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
                    var costCenterId = $("#ContentPlaceHolder1_ddlCostCentre").val();
                    var locationId = $("#ContentPlaceHolder1_ddlReceiveLocation").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    var supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
                    var purchaseType = $("#ContentPlaceHolder1_ddlPaymentType").val();

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
                        toastr.warning("Please Select Store");
                        $("#ContentPlaceHolder1_ddlCategory").focus();
                        return false;
                    }
                    else if (locationId == "0") {
                        toastr.warning("Please Seelct Store Location");
                        $("#ContentPlaceHolder1_ddlReceiveLocation").focus();
                        return false;
                    }
                    else if (supplierId == "0" && purchaseType == "Credit") {
                        toastr.warning("Please Select Supplier");
                        $("#ContentPlaceHolder1_ddlSupplier").focus();
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/ItemReceive.aspx/ItemSearch',
                        data: JSON.stringify({ searchTerm: request.term, companyId: companyId, projectId: projectId, costCenterId: costCenterId, locationId: locationId, categoryId: categoryId, supplierId: supplierId }),
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
                                    LastPurchaseDate: m.LastPurchaseDate,
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

                    $("#AttributeDiv").hide();
                    if (ui.item.IsAttributeItem) {
                        $("#AttributeDiv").show();
                    }

                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Color');
                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Size');
                    GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Style');

                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfItemId").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfCategoryId").val(ui.item.CategoryId);
                    $("#ContentPlaceHolder1_hfStockById").val(ui.item.StockBy);
                    $("#ContentPlaceHolder1_txtCurrentStock").val(ui.item.StockQuantity).attr("disabled", "disabled");
                    $("#ContentPlaceHolder1_txtCurrentStockBy").val(ui.item.UnitHead).attr("disabled", "disabled");
                    GetInvItemStockInfoByItemAndAttributeId();

                }
            });

            $("#ContentPlaceHolder1_ddlCurrency").change(function () {
                var currencyId = $(this).val();
                PageMethods.LoadCurrencyConversionRate(currencyId, OnLoadConversionRateSucceeded, OnLoadConversionRateFailed);
            });

            $("#ContentPlaceHolder1_txtReferenceBillDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_ddlReceiveType').change(function () {
                if ($('#ContentPlaceHolder1_ddlReceiveType').val() == "AdHoc" || $('#ContentPlaceHolder1_ddlReceiveType').val() == "Purchase") {
                    //$("#OEEntryPanel").show();
                    if ($("#ContentPlaceHolder1_hfIsItemReceiveOverheadExpenseEnable").val() == "1") {
                        $("#OEEntryPanel").show();
                    } else {
                        $("#OEEntryPanel").hide();
                    }
                }
                else {
                    $("#OEEntryPanel").hide();
                }
                return false;
            });
        });


        function ClearOverheadExpenseInfo() {
            $("#ContentPlaceHolder1_ddlAccountHead").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtAmount").val("");
            $("#ContentPlaceHolder1_txtOEDescription").val("");
            return false;
        }

        function ClearPaymentMethodInfo() {
            $("#ContentPlaceHolder1_ddlPMAccountHead").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtPMAmount").val("");
            $("#ContentPlaceHolder1_txtPMDescription").val("");
            return false;
        }

        function DeleteAccountHeadOfOE(deletedItem) {
            if (!confirm("Do you want to delete?"))
                return;
            var accoutHeadPMId = $("#ContentPlaceHolder1_hfAccoutHeadId").val();

            if (accoutHeadPMId != "0") {
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
            $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val(amountAfterDeletion);
            $("#ContentPlaceHolder1_txttotalAmount").val(amountAfterDeletion);

            var paymentTotal = $("#ContentPlaceHolder1_hftotalForPaymentInfos").val();
            paymentTotal = parseFloat(paymentTotal);
            paymentTotal = paymentTotal.toFixed(2);
            var itemTotal = $("#ContentPlaceHolder1_hfTotalForItems").val();
            itemTotal = parseFloat(itemTotal);
            itemTotal = itemTotal.toFixed(2);
            var totalRM = parseFloat(itemTotal) + parseFloat(amountAfterDeletion);
            var totalDue = parseFloat(totalRM) - parseFloat(paymentTotal);

            totalRM = totalRM.toFixed(2);
            totalDue = totalDue.toFixed(2);
            $("#ContentPlaceHolder1_txtPMTotalAmount").val(totalRM);
            $("#ContentPlaceHolder1_txtPaymentAmount").val(paymentTotal);
            $("#ContentPlaceHolder1_txtDueAmount").val(totalDue);

            $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(paymentTotal);
            $("#ContentPlaceHolder1_hfTotalForItems").val(itemTotal);
        }

        function DeleteAccountHeadOfPM(deletedItem) {
            if (!confirm("Do you want to delete?"))
                return;
            var accoutHeadPMId = $("#ContentPlaceHolder1_hfAccoutHeadPMId").val();

            if (accoutHeadPMId != "0") {
                var tr = $(deletedItem).parent().parent();

                DeletedAccountHeadPM.push({
                    AccountHeadDetailsIdPM: $(tr).find("td:eq(4)").text(),
                    AccoutHeadPMId: accoutHeadPMId
                });
            }

            $(deletedItem).parent().parent().remove();

            var amountAfterDeletion = 0;
            $("#PMAmountGrid tr").each(function (index) {
                var amount = $(this).find("td").eq(1).html();
                if (amount == undefined) {
                    amount = 0;
                }
                amountAfterDeletion = parseFloat(amountAfterDeletion) + parseFloat(amount);
            });
            amountAfterDeletion = amountAfterDeletion.toFixed(2);

            var itemTotal = $("#ContentPlaceHolder1_hfTotalForItems").val();
            itemTotal = parseFloat(itemTotal);
            itemTotal = itemTotal.toFixed(2);
            var oeTotal = $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val();
            oeTotal = parseFloat(oeTotal);
            oeTotal = oeTotal.toFixed(2);
            var totalRM = parseFloat(itemTotal) + parseFloat(oeTotal);
            var totalDue = parseFloat(totalRM) - parseFloat(amountAfterDeletion);

            totalRM = totalRM.toFixed(2);
            totalDue = totalDue.toFixed(2);
            $("#ContentPlaceHolder1_txtPMTotalAmount").val(totalRM);
            $("#ContentPlaceHolder1_txtPaymentAmount").val(amountAfterDeletion);
            $("#ContentPlaceHolder1_txtDueAmount").val(totalDue);

            $("#ContentPlaceHolder1_hfTotalForItems").val(itemTotal);
            $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val(oeTotal);
            $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(amountAfterDeletion);
        }

        function AddAccountHeadForOEInfo(accountHeadId, accountHead, amount, OEDescription, AccountHeadDetailsId, isEdited) {
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
            tr += "<td style='display:none'>" + accountHeadId + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "</tr>";

            $("#OEAmountGrid tbody").append(tr);
            var totalAmount = 0;
            $("#OEAmountGrid tr").each(function () {
                var amount = $(this).find("td").eq(1).html();
                if (amount == undefined) {
                    amount = 0;
                }
                totalAmount = parseFloat(totalAmount) + parseFloat(amount);
            });
            totalAmount = totalAmount.toFixed(2);
            $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val(totalAmount);
            $("#ContentPlaceHolder1_txttotalAmount").val(totalAmount);

            var paymentTotal = $("#ContentPlaceHolder1_hftotalForPaymentInfos").val();
            paymentTotal = parseFloat(paymentTotal);
            paymentTotal = paymentTotal.toFixed(2);
            var itemTotal = $("#ContentPlaceHolder1_hfTotalForItems").val();
            itemTotal = parseFloat(itemTotal);
            itemTotal = itemTotal.toFixed(2);
            var totalRM = parseFloat(itemTotal) + parseFloat(totalAmount);
            var totalDue = parseFloat(totalRM) - parseFloat(paymentTotal);

            totalRM = totalRM.toFixed(2);
            totalDue = totalDue.toFixed(2);
            $("#ContentPlaceHolder1_txtPMTotalAmount").val(totalRM);
            $("#ContentPlaceHolder1_txtPaymentAmount").val(paymentTotal);
            $("#ContentPlaceHolder1_txtDueAmount").val(totalDue);

            $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(paymentTotal);
            $("#ContentPlaceHolder1_hfTotalForItems").val(itemTotal);
        }

        function AddAccountHeadForPMInfo(accountHeadPMId, accountHeadPM, amountPM, pMDescription, AccountHeadPMDetailsId, isEditedPM) {
            var isEdited = "0";
            var rowLength = $("#PMAmountGrid tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:30%;'>" + accountHeadPM + "</td>";
            tr += "<td style='width:10%;'>" + amountPM + "</td>";
            tr += "<td style='width:50%;'>" + pMDescription + "</td>";
            tr += "<td style='width:10%;'><a href='javascript:void();' onclick= 'javascript:return DeleteAccountHeadOfPM(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            tr += "<td style='display:none'>" + AccountHeadPMDetailsId + "</td>";
            tr += "<td style='display:none'>" + accountHeadPMId + "</td>";
            tr += "<td style='display:none'>" + isEditedPM + "</td>";
            tr += "</tr>";

            $("#PMAmountGrid tbody").append(tr);
            var totalAmount = 0;
            $("#PMAmountGrid tr").each(function () {
                var amount = $(this).find("td").eq(1).html();
                if (amount == undefined) {
                    amount = 0;
                }
                totalAmount = parseFloat(totalAmount) + parseFloat(amount);
            });
            totalAmount = totalAmount.toFixed(2);

            var itemTotal = $("#ContentPlaceHolder1_hfTotalForItems").val();
            itemTotal = parseFloat(itemTotal);
            itemTotal = itemTotal.toFixed(2);
            var oeTotal = $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val();
            oeTotal = parseFloat(oeTotal);
            oeTotal = oeTotal.toFixed(2);
            var totalRM = parseFloat(itemTotal) + parseFloat(oeTotal);
            var totalDue = parseFloat(totalRM) - parseFloat(totalAmount);
            totalRM = totalRM.toFixed(2);
            totalDue = totalDue.toFixed(2);
            $("#ContentPlaceHolder1_txtPMTotalAmount").val(totalRM);
            $("#ContentPlaceHolder1_txtPaymentAmount").val(totalAmount);
            $("#ContentPlaceHolder1_txtDueAmount").val(totalDue);

            $("#ContentPlaceHolder1_hfTotalForItems").val(itemTotal);
            $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val(oeTotal);
            $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(totalAmount);
        }

        function GetInvItemStockInfoByItemAndAttributeId() {
            var locationId = parseInt($('#ContentPlaceHolder1_ddlReceiveLocation').val(), 10);
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
            var itemId = parseInt($("#ContentPlaceHolder1_hfItemId").val(), 10);

            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/ItemReceive.aspx/GetInvItemStockInfoByItemAndAttributeId',
                data: "{'itemId':'" + itemId + "','colorId':'" + colorId + "','sizeId':'" + sizeId + "','styleId':'" + styleId + "','locationId':'" + locationId + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != null) {
                        $("#ContentPlaceHolder1_txtCurrentStock").val(data.d.StockQuantity);
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
                url: '../Inventory/ItemReceive.aspx/GetInvItemAttributeByItemIdAndAttributeType',
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
        function OnLoadReceiveStoreByCompanyIdSucceed(result) {
            $("#BagAndBonusDiv").hide();
            if (result[0].CompanyType == "RiceMill") {
                $("#BagAndBonusDiv").show();
            }

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

            var costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            if (costCenterId != "0") {
                $("#ContentPlaceHolder1_ddlCostCentre").val(costCenterId);
            }

            return false;
        }
        function OnLoadReceiveStoreByCompanyIdFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function LoadItemForReceive(control) {
            var id = $(control).val();
            var supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();

            $("#ItemForReceiveTbl tbody").html("");
            ClearAfterAdhoqReceiveItemAdded();

            $("#ReceiveOrderItemTbl tbody").html("");
            ReceiveOrderItem = new Array();

            if (id != "0") {
                var receiveType = $("#ContentPlaceHolder1_ddlReceiveType").val();
                if ($("#ContentPlaceHolder1_hfReceiveOrderId").val() == "0") {

                    var purchase = null;

                    if (receiveType == "Purchase") {
                        purchase = _.findWhere(PurchaseOrderList, { POrderId: parseInt(id, 10) });
                    }
                    else if (receiveType == "LC") {
                        purchase = _.findWhere(LCList, { LCId: parseInt(id, 10) });
                    }
                    if (purchase != null) {
                        $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(purchase.CompanyId).trigger('change');
                        $("#ContentPlaceHolder1_ddlSupplier").val(purchase.SupplierId + '').trigger('change');
                        $("#ContentPlaceHolder1_ddlSupplier").attr('disabled', true);
                        supplierId = purchase.SupplierId;
                        PageMethods.LoadItemFromPurchaseOrderForReceivedByPurchaseOrderId(id, purchase.POType, supplierId, OnPurchaseOrderItemSucceeded, OnPurchaseOrderItemRateFailed);
                    }
                }

                $("#AdhocReceive").hide();
                $("#AdhocReceiveItem").hide();
                $("#ReceiveOrderItemContainer").show();
                $("#PurchaseOrderCostCenterContainer").hide();
                $("#ContentPlaceHolder1_ddlCurrency").attr('disabled', true);
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", true);

            }
        }

        function OnLoadConversionRateSucceeded(result) {
            $("#ContentPlaceHolder1_lblConversionRate").text(result.ConversionRate);
            //CurrencyRateInfoEnable();
            //CurrencyConvertion();
        }
        function OnLoadConversionRateFailed() {
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

            PageMethods.SerialAvailabilityCheck(SerialCheck, CheckAndAddedSerialWiseProductSucceeded, CheckAndAddedSerialWiseProductFailed);
            return false;
        }

        function CheckAndAddedSerialWiseProductSucceeded(result) {

            if (result.IsSuccess) {
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

        function AddItemForReceive() {
            if ($("#ContentPlaceHolder1_ddlSupplier").val() == "0" && $("#ContentPlaceHolder1_ddlPaymentType").val() == "Credit") {
                toastr.warning("Please Select Supplier.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlCostCentre").val() == "0") {
                toastr.warning("Please Select Store.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlReceiveLocation").val() == "0") {
                toastr.warning("Please Select Store Location.");
                return false;
            }

            if ($("#ContentPlaceHolder1_ddlReceiveType").val() == "AdHoc") {
                AddItemForAdhoqReceive();
            }
            else {
                AddItemForReceiveFromPurchase();
            }
        }
        function AddItemForAdhoqReceive() {

            if (ItemSelected == null) {
                toastr.warning("Please Select Item.");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtReceiveQuantity").val()) == "" || $.trim($("#ContentPlaceHolder1_txtReceiveQuantity").val()) == "0") {
                toastr.warning("Please Give Receive Quantity.");
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

            var IntColorId = parseInt(colorId, 10);
            var IntSizeId = parseInt(sizeId, 10);
            var IntStyleId = parseInt(styleId, 10);

            var itm = _.findWhere(ReceiveOrderItem, { ItemId: ItemSelected.ItemId, ColorId: IntColorId, SizeId: IntSizeId, StyleId: IntStyleId });

            if (itm != null) {
                toastr.warning("Same Item Already Added. Duplicate Item Is Not Accepted.");
                return false;
            }

            var total = 0, unitPrice = 0, quantity = 0, tr = "", remarks = "", bagQuantity = 0, bonusAmount = 0;

            unitPrice = $("#ContentPlaceHolder1_txtPurchasePrice").val();
            quantity = $("#ContentPlaceHolder1_txtReceiveQuantity").val();
            remarks = $("#ContentPlaceHolder1_txtItemWiseRemarks").val();
            total = toFixed((parseFloat(unitPrice) * parseFloat(quantity)), 2);

            bagQuantity = $("#ContentPlaceHolder1_txtBagQuantity").val();
            bonusAmount = $("#ContentPlaceHolder1_txtBonusAmount").val();

            if (bagQuantity == "") {
                bagQuantity = 0;
            }

            if (bonusAmount == "") {
                bonusAmount = 0;
            }

            tr += "<tr>";
            tr += "<td style='width:25%;'>" + ItemSelected.ItemName + "</td>";

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

            tr += "<td style='width:15%;'>" +
                "<input type='text' value='" + unitPrice + "' id='pq" + ItemSelected.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                "</td>";
            tr += "<td style='width:10%;'>" +
                "<input type='text' value='" + quantity + "' id='pp" + ItemSelected.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                "</td>";

            tr += "<td style='width:10%;'>" + ItemSelected.UnitHead + "</td>";
            tr += "<td style='width:15%;'>" + total + "</td>";
            tr += "<td style='width:15%;'>" +

                "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";


            if (ItemSelected.ProductType == 'Serial Product') {
                tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForAdHocItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
            }
            tr += "</td>";

            tr += "<td style='display:none;'>" + ItemSelected.ItemId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.CategoryId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.StockBy + "</td>";

            tr += "<td style='display:none;'>" + unitPrice + "</td>";
            tr += "<td style='display:none;'>" + quantity + "</td>";

            tr += "<td style='display:none'>" + colorId + "</td>";
            tr += "<td style='display:none'>" + sizeId + "</td>";
            tr += "<td style='display:none'>" + styleId + "</td>";

            tr += "<td style='display:none'>" + bagQuantity + "</td>";
            tr += "<td style='display:none'>" + bonusAmount + "</td>";

            tr += "<td style='display:none;'>0</td>";

            tr += "</tr>";

            $("#ItemForReceiveTbl tbody").prepend(tr);
            tr = "";

            ReceiveOrderItem.push({
                ItemId: parseInt(ItemSelected.ItemId, 10),
                ColorId: parseInt(colorId, 10),
                SizeId: parseInt(sizeId, 10),
                StyleId: parseInt(styleId, 10),
                ItemName: ItemSelected.ItemName,
                StockById: parseInt(ItemSelected.StockBy, 10),
                Quantity: parseFloat(quantity),
                PurchasePrice: parseFloat(unitPrice),
                ProductType: ItemSelected.ProductType,
                Remarks: remarks,
                BagQuantity: bagQuantity,
                BonusAmount: bonusAmount,
                DetailId: 0
            });

            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlReceiveLocation").attr('disabled', true);

            CommonHelper.ApplyDecimalValidation();
            ClearAfterAdhoqReceiveItemAdded();
            $("#ContentPlaceHolder1_txtItem").focus();
            var total = 0;
            $("#ItemForReceiveTbl tbody tr").each(function () {
                total += toFixed(parseFloat($(this).find("td:eq(7)").text()), 2);
            });
            $("#ContentPlaceHolder1_hfTotalForItems").val(total);

            var paymentTotal = $("#ContentPlaceHolder1_hftotalForPaymentInfos").val();
            paymentTotal = parseFloat(paymentTotal);
            paymentTotal = paymentTotal.toFixed(2);
            var oeTotal = $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val();
            oeTotal = parseFloat(oeTotal);
            oeTotal = oeTotal.toFixed(2);
            var totalRM = parseFloat(total) + parseFloat(oeTotal);
            var totalDue = parseFloat(totalRM) - parseFloat(paymentTotal);

            totalRM = totalRM.toFixed(2);
            totalDue = totalDue.toFixed(2);
            $("#ContentPlaceHolder1_txtPMTotalAmount").val(totalRM);
            $("#ContentPlaceHolder1_txtPaymentAmount").val(paymentTotal);
            $("#ContentPlaceHolder1_txtDueAmount").val(totalDue);

            $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val(oeTotal);
            $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(paymentTotal);

            $("#ItemForReceiveTbl tfoot").find("tr:eq(0)").remove();
            tr += "<tr>";
            tr += "<td style='width:25%;'> </td>";

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                tr += "<td style='width:10%;'> </td>";
                tr += "<td style='width:10%;'> </td>";
                tr += "<td style='width:10%;'> </td>";
            }
            else {
                tr += "<td style='display:none'></td>";
                tr += "<td style='display:none'></td>";
                tr += "<td style='display:none'></td>";
            }

            tr += "<td style='width:10%;'> </td>";
            tr += "<td style='width:10%;'> </td>";
            tr += "<td style='width:10%;font-weight:bold;'> Total </td>";
            tr += "<td style='width:15%;font-weight:bold;'>" + total + "</td>";

            tr += "<td style='display:none'>" + 0 + "</td>";
            tr += "<td style='display:none'>" + 0 + "</td>";
            tr += "<td style='display:none'>" + 0 + "</td>";

            tr += "<td style='display:none'>" + 0 + "</td>";
            tr += "<td style='display:none'>" + 0 + "</td>";

            tr += "<td style='width:15%;'> </td>";
            tr += "</tr>";

            $("#ItemForReceiveTbl tfoot").prepend(tr);
            tr = "";
        }
        function CalculateTotalForAdhoq(control) {
            var tr = $(control).parent().parent();

            var purchasePrice = 0;
            var quantity = 0;
            var oldPurchasePrice = 0;
            var oldQuantity = 0;
            var itemId = 0;
            var colorId = 0;
            var sizeId = 0;
            var styleId = 0;
            var total = 0;

            //if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
            purchasePrice = $.trim($(tr).find("td:eq(4)").find("input").val());
            quantity = $.trim($(tr).find("td:eq(5)").find("input").val());
            oldPurchasePrice = $(tr).find("td:eq(12)").text();
            oldQuantity = $(tr).find("td:eq(13)").text();
            //}
            //else
            //{
            //    purchasePrice = $.trim($(tr).find("td:eq(1)").find("input").val());
            //    quantity = $.trim($(tr).find("td:eq(2)").find("input").val());
            //    oldPurchasePrice = $(tr).find("td:eq(9)").text();
            //    oldQuantity = $(tr).find("td:eq(10)").text();
            //}

            if (purchasePrice == "" || purchasePrice == "0") {
                toastr.info("Purchase Price Cannot Be Zero Or Empty.");
                //if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                $(tr).find("td:eq(6)").find("input").val(oldPurchasePrice);
                //}
                //else
                //{
                //    $(tr).find("td:eq(3)").find("input").val(oldPurchasePrice);
                //}

                return false;
            }
            else if (quantity == "" || quantity == "0") {
                toastr.info("Quantity Cannot Be Zero Or Empty.");
                //if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                $(tr).find("td:eq(7)").find("input").val(oldQuantity);
                //}
                //else
                //{
                //    $(tr).find("td:eq(4)").find("input").val(oldQuantity);
                //}

                return false;
            }

            var lineTotal = toFixed(parseFloat(purchasePrice) * parseFloat(quantity), 2);

            //if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
            $(tr).find("td:eq(7)").text(lineTotal);
            itemId = parseInt($.trim($(tr).find("td:eq(9)").text()), 10);

            colorId = parseInt($.trim($(tr).find("td:eq(15)").text()), 10);
            sizeId = parseInt($.trim($(tr).find("td:eq(16)").text()), 10);
            styleId = parseInt($.trim($(tr).find("td:eq(17)").text()), 10);
            //}
            //else
            //{
            //    $(tr).find("td:eq(4)").text(lineTotal);
            //    itemId = parseInt($.trim($(tr).find("td:eq(6)").text()), 10);

            //    colorId = parseInt($.trim($(tr).find("td:eq(12)").text()), 10);
            //    sizeId = parseInt($.trim($(tr).find("td:eq(13)").text()), 10);
            //    styleId = parseInt($.trim($(tr).find("td:eq(14)").text()), 10);
            //}

            var item = _.findWhere(ReceiveOrderItem, { ItemId: itemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId });
            var index = _.indexOf(ReceiveOrderItem, item);

            ReceiveOrderItem[index].Quantity = parseFloat(quantity);
            ReceiveOrderItem[index].PurchasePrice = parseFloat(purchasePrice);

            //if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
            $(tr).find("td:eq(12)").text(purchasePrice);
            $(tr).find("td:eq(13)").text(quantity);

            $("#ItemForReceiveTbl tbody tr").each(function () {
                total += toFixed(parseFloat($(this).find("td:eq(7)").text()), 2);
            });
            //}
            //else
            //{
            //    $(tr).find("td:eq(9)").text(purchasePrice);
            //    $(tr).find("td:eq(10)").text(quantity);

            //    $("#ItemForReceiveTbl tbody tr").each(function () {
            //        total += toFixed(parseFloat($(this).find("td:eq(4)").text()), 2);
            //    });    
            //}

            $("#ItemForReceiveTbl tfoot").find("tr:eq(0)").remove();
            tr += "<tr>";
            tr += "<td style='width:25%;'> </td>";

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                tr += "<td style='width:10%;'> </td>";
                tr += "<td style='width:10%;'> </td>";
                tr += "<td style='width:10%;'> </td>";
            }
            else {
                tr += "<td style='display:none'></td>";
                tr += "<td style='display:none'></td>";
                tr += "<td style='display:none'></td>";
            }

            tr += "<td style='width:10%;'> </td>";
            tr += "<td style='width:10%;'> </td>";
            tr += "<td style='width:10%;font-weight:bold;'> Total </td>";
            tr += "<td style='width:15%;font-weight:bold;'>" + total + "</td>";

            tr += "<td style='display:none'>" + colorId + "</td>";
            tr += "<td style='display:none'>" + sizeId + "</td>";
            tr += "<td style='display:none'>" + styleId + "</td>";

            tr += "<td style='width:15%;'> </td>";
            tr += "</tr>";

            $("#ItemForReceiveTbl tfoot").prepend(tr);
            tr = "";
        }
        function DeleteAdhoqItem(control) {
            if (!confirm("Do you want to delete item?")) { return false; }

            var tr = $(control).parent().parent();

            var itemId = 0, colorId = 0, sizeId = 0, styleId = 0;
            var receiveDetailsId = 0;

            itemId = parseInt($.trim($(tr).find("td:eq(9)").text()), 10);
            receiveDetailsId = parseInt($.trim($(tr).find("td:eq(14)").text()), 10);

            colorId = parseInt($.trim($(tr).find("td:eq(15)").text()), 10);
            sizeId = parseInt($.trim($(tr).find("td:eq(16)").text()), 10);
            styleId = parseInt($.trim($(tr).find("td:eq(17)").text()), 10);

            var item = _.findWhere(ReceiveOrderItem, { ItemId: itemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId });
            var index = _.indexOf(ReceiveOrderItem, item);

            if (parseInt(receiveDetailsId, 10) > 0)
                ReceiveOrderItemDeleted.push(JSON.parse(JSON.stringify(item)));

            ReceiveOrderItem.splice(index, 1);
            $(tr).remove();

            var serialCount = 0, rowSerial = 0;
            var itemSerial = _.where(AddedSerialzableProduct, { ItemId: itemId });
            serialCount = itemSerial.length;

            for (rowSerial = 0; rowSerial < serialCount; rowSerial++) {

                if (itemSerial[rowSerial].SerialId > 0)
                    DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(itemSerial[rowSerial])));

                var srlItem = _.findWhere(AddedSerialzableProduct, { SerialNumber: itemSerial[rowSerial].SerialNumber });
                var srlIndex = _.indexOf(AddedSerialzableProduct, srlItem);
                AddedSerialzableProduct.splice(srlIndex, 1);
            }

            if ($("#ItemForReceiveTbl tbody tr").length == 0) {
                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlReceiveLocation").attr("disabled", false);
            }
            var total = 0;
            $("#ItemForReceiveTbl tbody tr").each(function () {
                total += toFixed(parseFloat($(this).find("td:eq(7)").text()), 2);
            });
            $("#ContentPlaceHolder1_hfTotalForItems").val(total);


            var paymentTotal = $("#ContentPlaceHolder1_hftotalForPaymentInfos").val();
            paymentTotal = parseFloat(paymentTotal);
            paymentTotal = paymentTotal.toFixed(2);
            var oeTotal = $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val();
            oeTotal = parseFloat(oeTotal);
            oeTotal = oeTotal.toFixed(2);
            var totalRM = parseFloat(total) + parseFloat(oeTotal);
            var totalDue = parseFloat(totalRM) - parseFloat(paymentTotal);

            totalRM = totalRM.toFixed(2);
            totalDue = totalDue.toFixed(2);
            $("#ContentPlaceHolder1_txtPMTotalAmount").val(totalRM);
            $("#ContentPlaceHolder1_txtPaymentAmount").val(paymentTotal);
            $("#ContentPlaceHolder1_txtDueAmount").val(totalDue);

            $("#ContentPlaceHolder1_hfTotalForItems").val(total);
            $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val(oeTotal);
            $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(paymentTotal);


            $("#ItemForReceiveTbl tfoot").find("tr:eq(0)").remove();
            tr += "<tr>";
            tr += "<td style='width:25%;'> </td>";

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                tr += "<td style='width:10%;'> </td>";
                tr += "<td style='width:10%;'> </td>";
                tr += "<td style='width:10%;'> </td>";
            }
            else {
                tr += "<td style='display:none'></td>";
                tr += "<td style='display:none'></td>";
                tr += "<td style='display:none'></td>";
            }

            tr += "<td style='width:10%;'> </td>";
            tr += "<td style='width:10%;'> </td>";
            tr += "<td style='width:10%;font-weight:bold;'> Total </td>";
            tr += "<td style='width:15%;font-weight:bold;'>" + total + "</td>";

            tr += "<td style='display:none'>" + colorId + "</td>";
            tr += "<td style='display:none'>" + sizeId + "</td>";
            tr += "<td style='display:none'>" + styleId + "</td>";

            tr += "<td style='width:15%;'> </td>";
            tr += "</tr>";

            $("#ItemForReceiveTbl tfoot").prepend(tr);
            tr = "";
        }

        function ClearAfterAdhoqReceiveItemAdded() {
            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "0") {
                $("#ContentPlaceHolder1_txtItem").val("");
                $("#ContentPlaceHolder1_ddlColorAttribute").empty();
                $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
                $("#ContentPlaceHolder1_ddlStyleAttribute").empty();
                $("#ContentPlaceHolder1_txtPurchasePrice").val("");
                $("#ContentPlaceHolder1_txtCurrentStock").val("").prop("disabled", false);
                $("#ContentPlaceHolder1_txtCurrentStockBy").val("").prop("disabled", false);
                ItemSelected = null;
            }
            $("#ContentPlaceHolder1_txtPurchasePrice").val("");
            $("#ContentPlaceHolder1_txtReceiveQuantity").val("");
            $("#ContentPlaceHolder1_txtBagQuantity").val("");
            $("#ContentPlaceHolder1_txtBonusAmount").val("");

            $("#PurchaseOrderItemTbl tbody").html("");
        }

        function OnPurchaseOrderItemSucceeded(result) {

            var receiveType = $("#ContentPlaceHolder1_ddlReceiveType").val();
            var purchase = null;

            if (receiveType == "Purchase") {
                var id = $("#ContentPlaceHolder1_ddlPurchaseOrderNumber").val();
                purchase = _.findWhere(PurchaseOrderList, { POrderId: parseInt(id, 10) });
            }
            else if (receiveType == "LC") {
                var id = $("#ContentPlaceHolder1_ddlLCNumber").val();
                purchase = _.findWhere(LCList, { POrderId: parseInt(id, 10) });
            }
            ReceiveOrderItem = result;
            debugger;
            if (purchase != null) {
                $("#ContentPlaceHolder1_ddlPurchaseOrderCostcenter").val(purchase.CostCenterId + '');
                $("#ContentPlaceHolder1_ddlCostCentre").val(purchase.CostCenterId + '').trigger("change");
            }

            $("#OrderCheck").prop("checked", true);

            $("#PurchaseOrderItemTbl tbody").html("");
            var totalRow = result.length, row = 0;
            var tr = "";

            for (row = 0; row < totalRow; row++) {

                tr += "<tr>";

                if (result[row].SupplierId > 0) {
                    tr += "<td style='width:5%; text-align: center;'>" +
                        "<input type='checkbox' checked='checked' id='chk' " + result[row].ItemId + " />" +
                        "</td>";
                }
                else {
                    tr += "<td style='width:5%;'></td>";
                }

                tr += "<td style='width:25%;'>" + result[row].ItemName + "</td>";

                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    tr += "<td style='width:10%;'>" + result[row].ColorText + "</td>";
                    tr += "<td style='width:10%;'>" + result[row].SizeText + "</td>";
                    tr += "<td style='width:10%;'>" + result[row].StyleText + "</td>";
                }
                else {
                    tr += "<td style='display:none'>" + result[row].ColorText + "</td>";
                    tr += "<td style='display:none'>" + result[row].SizeText + "</td>";
                    tr += "<td style='display:none'>" + result[row].StyleText + "</td>";
                }

                tr += "<td style='width:13%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:13%;'>" + result[row].QuantityReceived + "</td>";
                tr += "<td style='width:13%;'>" + result[row].RemainingReceiveQuantity + "</td>";
                tr += "<td style='width:13%;'>" + result[row].MessureUnit + "</td>";

                tr += "<td style='width:13%;'>" +
                    "<input type='text' value='" + result[row].RemainingReceiveQuantity + "' id='q' " + result[row].ItemId + " class='form-control quantitydecimal' onblur='CheckPurchaseOrderWiseQuantity(this)' />" +
                    "</td>";
                tr += "<td style='width:10%; display: none;'>" +
                    "<input type='text' disabled='disabled' value='" + result[row].PurchasePrice + "' id='pp' " + result[row].ItemId + " class='form-control quantitydecimal' onblur='CheckPurchaseOrderPriceWithConfirmation(this)' />" +
                    "</td>";

                tr += "<td style='width:10%; display: none;'>" + result[row].PurchasePrice + "</td>";

                tr += "<td style='width:5%;'>";
                if (result[row].ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForPurchaseWiseItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + result[row].POrderId + "</td>";
                tr += "<td style='display:none;'>" + result[row].ItemId + "</td>";
                tr += "<td style='display:none;'>" + result[row].StockById + "</td>";
                tr += "<td style='display:none;'>" + result[row].RemainingReceiveQuantity + "</td>";
                tr += "<td style='display:none;'>" + result[row].PurchasePrice + "</td>";
                tr += "<td style='display:none;'>" + result[row].ProductType + "</td>";
                tr += "<td style='display:none;'>" + result[row].ReceiveDetailsId + "</td>";

                tr += "<td style='display:none'>" + result[row].ColorId + "</td>";
                tr += "<td style='display:none'>" + result[row].SizeId + "</td>";
                tr += "<td style='display:none'>" + result[row].StyleId + "</td>";

                tr += "</tr>";

                $("#PurchaseOrderItemTbl tbody").append(tr);
                tr = "";
            }

            CommonHelper.ApplyDecimalValidation();

        }
        function OnPurchaseOrderItemRateFailed() {
        }
        function ClearAfterReceiveItemAddedFromPurchase() {
            $("#PurchaseOrderItemTbl tbody").html("");
        }

        function CheckPurchaseOrderQuantity(control) {
            var tr = $(control).parent().parent();
            var quantity = $.trim($(control).val());
            var purchaseOrderQuantity, alreadyReceivedQuantity, oldQuantity;

            //if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
            purchaseOrderQuantity = parseFloat($(tr).find("td:eq(5)").text());
            alreadyReceivedQuantity = parseFloat($(tr).find("td:eq(6)").text());
            oldQuantity = parseFloat($(tr).find("td:eq(14)").text());
            //}
            //else
            //{
            //    purchaseOrderQuantity = parseFloat($(tr).find("td:eq(2)").text());
            //    alreadyReceivedQuantity = parseFloat($(tr).find("td:eq(3)").text());
            //    oldQuantity = parseFloat($(tr).find("td:eq(11)").text());
            //}

            var approvedQuantity = parseFloat(purchaseOrderQuantity) - (parseFloat(alreadyReceivedQuantity) + parseFloat(quantity));

            if (quantity == "" || quantity == "0") {
                toastr.warning("Quantity Cannot Empty Or Zero");
                $(control).val(oldQuantity);
            }
            else if (parseFloat(quantity) > approvedQuantity) {
                toastr.warning("Quantity Cannot Greater Than Purchase Order Quantity");
                $(control).val(oldQuantity);
            }
        }
        function CheckPurchaseOrderPrice(control) {

            var tr = $(control).parent().parent();
            var price = $.trim($(control).val());
            var lastPurchasePrice = 0;

            //if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
            lastPurchasePrice = parseFloat($(tr).find("td:eq(14)").text());
            //}
            //else
            //{
            //    lastPurchasePrice = parseFloat($(tr).find("td:eq(11)").text());
            //}

            if (price == "" || price == "0") {
                toastr.warning("Price Cannot Empty Or Zero");
                $(control).val(lastPurchasePrice);
            }
        }
        function CheckPurchaseOrderPriceWithConfirmation(control) {
            if (!confirm("Do you Want To Check?")) {
                return false;
            }
            CheckPurchaseOrderPrice(control);
        }

        function CheckPurchaseOrderWiseQuantity(control) {
            var tr = $(control).parent().parent();
            var receiveQuantity = $.trim($(control).val());
            var remainQuantity, oldQuantity, price;

            //if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
            remainQuantity = parseFloat($(tr).find("td:eq(7)").text());
            oldQuantity = parseFloat($(tr).find("td:eq(16)").text());
            price = parseFloat($(tr).find("td:eq(10)").find("input").val());
            //}
            //else
            //{
            //    remainQuantity = parseFloat($(tr).find("td:eq(4)").text());
            //    oldQuantity = parseFloat($(tr).find("td:eq(13)").text());
            //    price = parseFloat($(tr).find("td:eq(7)").find("input").val());
            //}

            if (receiveQuantity == "" || receiveQuantity == "0") {
                toastr.warning("Quantity Cannot Empty Or Zero.");
                $(control).val(oldQuantity);
                receiveQuantity = oldQuantity;
            }
            else if (receiveQuantity > remainQuantity) {
                toastr.warning("Receive Quantity Cannot Greater Than Max Receive Quantiy.");
                $(control).val(oldQuantity);
                receiveQuantity = oldQuantity;
            }

            var total = parseFloat(price) * parseFloat(receiveQuantity);

            var itemId = 0;
            //if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
            //$(tr).find("td:eq(11)").text(total);
            itemId = parseInt($.trim($(tr).find("td:eq(14)").text()), 10);
            //}
            //else
            //{
            //    $(tr).find("td:eq(8)").text(total);
            //    itemId = parseInt($.trim($(tr).find("td:eq(11)").text()), 10);
            //}

            var item = _.findWhere(ReceiveOrderItem, { ItemId: itemId });
            var index = _.indexOf(ReceiveOrderItem, item);

            ReceiveOrderItem[index].Quantity = parseFloat(receiveQuantity);
        }


        function CheckPurchaseOrderWisePrice(control) {
            var tr = $(control).parent().parent();
            var price = $.trim($(control).val());
            var purchaseOrderQuantity, alreadyReceivedQuantity, oldPrice, receiveQuantity, total, itemId;

            //if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
            purchaseOrderQuantity = parseFloat($(tr).find("td:eq(1)").text());
            alreadyReceivedQuantity = parseFloat($(tr).find("td:eq(5)").text());
            oldPrice = parseFloat($(tr).find("td:eq(13)").text());
            receiveQuantity = parseFloat($(tr).find("td:eq(7)").find("input").val());

            if (price == "" || price == "0") {
                toastr.warning("Price Cannot Empty Or Zero.");
                $(control).val(oldPrice);
                price = oldPrice;
            }

            total = toFixed(parseFloat(price) * parseFloat(receiveQuantity), 2);
            $(tr).find("td:eq(9)").text(total);

            itemId = parseInt($.trim($(tr).find("td:eq(11)").text()), 10);
            //}
            //else
            //{
            //    purchaseOrderQuantity = parseFloat($(tr).find("td:eq(1)").text());
            //    alreadyReceivedQuantity = parseFloat($(tr).find("td:eq(2)").text());
            //    oldPrice = parseFloat($(tr).find("td:eq(10)").text());
            //    receiveQuantity = parseFloat($(tr).find("td:eq(4)").find("input").val());

            //    if (price == "" || price == "0") {
            //        toastr.warning("Price Cannot Empty Or Zero.");
            //        $(control).val(oldPrice);
            //        price = oldPrice;
            //    }

            //    total = toFixed(parseFloat(price) * parseFloat(receiveQuantity), 2);
            //    $(tr).find("td:eq(6)").text(total);

            //    itemId = parseInt($.trim($(tr).find("td:eq(8)").text()), 10);
            //}

            var item = _.findWhere(ReceiveOrderItem, { ItemId: itemId });
            var index = _.indexOf(ReceiveOrderItem, item);

            ReceiveOrderItem[index].PurchasePrice = parseFloat(price);

            var row = 0, rowCount = 0;

            var editedItem = _.where(ReceiveOrderItemFromPurchase, { ItemId: itemId });
            rowCount = editedItem.length;

            for (row = 0; row < rowCount; row++) {
                var reqItem = _.findWhere(ReceiveOrderItemFromPurchase, { ItemId: itemId });
                var reqIndex = _.indexOf(ReceiveOrderItemFromPurchase, reqItem);
                ReceiveOrderItemFromPurchase[reqIndex].PurchasePrice = parseFloat(price);
            }
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

                if (itemSerial[rowSerial].SerialId > 0)
                    DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(itemSerial[rowSerial])));

                var srlItem = _.findWhere(AddedSerialzableProduct, { SerialNumber: itemSerial[rowSerial].SerialNumber });
                var srlIndex = _.indexOf(AddedSerialzableProduct, srlItem);
                AddedSerialzableProduct.splice(srlIndex, 1);
            }

            $(tr).remove();
        }

        function LoadStoreLocationByCostCenter(costCenetrId) {
            PageMethods.StoreLocationByCostCenter(costCenetrId, OnLoadLocationSucceeded, OnLoadLocationFailed);
        }

        function OnLoadLocationSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlReceiveLocation');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    if (list.length > 1) {
                        control.empty().append('<option value="0">---Please Select---</option>');
                    }
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].LocationId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }

            if (list.length == 1 && $("#ContentPlaceHolder1_hfLocationId").val() == "0")
                $("#ContentPlaceHolder1_ddlReceiveLocation").val($("#ContentPlaceHolder1_ddlReceiveLocation option:first").val());
            else
                $("#ContentPlaceHolder1_ddlReceiveLocation").val($("#ContentPlaceHolder1_hfLocationId").val()).trigger("change");
            return false;
        }
        function OnLoadLocationFailed() { }

        function LoadNotReceivedPurchaseOrder() {
            PageMethods.LoadNotReceivedPurchaseOrder(OnLoadPOSucceeded, OnLoadPOFailed);
        }
        function OnLoadPOSucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlPurchaseOrderNumber');
            PurchaseOrderList = result;

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">---Please Select---</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].PONumber + '" value="' + list[i].POrderId + '">' + list[i].PONumber + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }

            return false;
        }
        function OnLoadPOFailed() { }

        function SaveReceiveOrder() {

            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            if (companyId == "0") {
                toastr.warning("Select a company.");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                return false;
            }
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            $("#ContentPlaceHolder1_hfCompanyId").val(companyId);
            $("#ContentPlaceHolder1_hfProjectId").val(projectId);

            if ($("#ContentPlaceHolder1_ddlSupplier").val() == "0" && $("#ContentPlaceHolder1_ddlPaymentType").val() == "Credit") {
                toastr.warning("Please Select Supplier.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlCostCentre").val() == "0") {
                toastr.warning("Please Select Store.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlReceiveLocation").val() == "0") {
                toastr.warning("Please Select Store Location.");
                return false;
            }

            if ($("#ContentPlaceHolder1_ddlReceiveType").val() == "AdHoc" && $("#PurchaseItemForPReceiveTbl tbody tr").length == 0) {
                if ($("#ItemForReceiveTbl tbody tr").length == 0) {
                    toastr.warning("Please Add Item For Receive.");
                    return false;
                }
            }
            else {
                if ($("#PurchaseOrderItemTbl tbody tr").length == 0) {
                    toastr.warning("Please Add Item From Purchase Order For Receive.");
                    return false;
                }
            }

            if ($("#ContentPlaceHolder1_txtRemarks").val() == "") {
                toastr.warning("Please Provide Description.");
                $("#ContentPlaceHolder1_txtRemarks").focus();
                return false;
            }

            var EditedPurchaseOrderItem = new Array();
            var itemId = "", remarks = "";
            var purchaseItem = null;

            var receiveOrderId = "0", receiveType = "", receivedByDate = null, supplierId = "0", isEdited = "0",
                categoryId = "", costCenterId = "", locationId = "", porderId = "0", receiveDetailsId = 0, referenceBillDate = "", referenceBillNo = "", paymentType = "";

            receiveType = $("#ContentPlaceHolder1_ddlReceiveType").val();
            paymentType = $("#ContentPlaceHolder1_ddlPaymentType").val();
            receiveOrderId = $("#ContentPlaceHolder1_hfReceiveOrderId").val();
            supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            costCenterId = $("#ContentPlaceHolder1_ddlCostCentre").val();
            locationId = $("#ContentPlaceHolder1_ddlReceiveLocation").val();
            if (receiveType == "Purchase")
                porderId = $("#ContentPlaceHolder1_ddlPurchaseOrderNumber").val();
            else if (receiveType == "LC")
                porderId = $("#ContentPlaceHolder1_ddlLCNumber").val();
            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            checkedBy = 0;
            approvedBy = 0;
            currencyId = $("#ContentPlaceHolder1_ddlCurrency").val();
            convertionRate = $("#ContentPlaceHolder1_lblConversionRate").text();
            referenceBillDate = $("#ContentPlaceHolder1_txtReferenceBillDate").val();
            if (referenceBillDate != '') {
                referenceBillDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(referenceBillDate, innBoarDateFormat);
            }
            referenceBillNo = $("#ContentPlaceHolder1_txtReferenceBillNo").val();

            if ($("#ContentPlaceHolder1_ddlReceiveType").val() == "AdHoc" && $("#PurchaseItemForPReceiveTbl tbody tr").length == 0) {
                ReceiveOrderItemFromPurchase = ReceiveOrderItem;
            }
            else {
                ReceiveOrderItem = new Array();
                debugger;
                $("#PurchaseOrderItemTbl tbody tr").each(function () {

                    receiveDetailsId = parseInt($(this).find("td:eq(19)").text(), 10);
                    itemId = parseInt($(this).find("td:eq(14)").text(), 10);

                    var colorId = parseInt($(this).find("td:eq(20)").text());
                    var sizeId = parseInt($(this).find("td:eq(21)").text());
                    var styleId = parseInt($(this).find("td:eq(22)").text());

                    //var bagQuantity = parseInt($(this).find("td:eq(23)").text());
                    //var bonusAmount = parseInt($(this).find("td:eq(24)").text());

                    var bagQuantity = parseInt(0);
                    var bonusAmount = parseInt(0);


                    if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                        ReceiveOrderItem.push({
                            ReceiveDetailsId: receiveDetailsId,
                            ReceivedId: parseInt(receiveOrderId),
                            ItemId: itemId,
                            ColorId: colorId,
                            SizeId: sizeId,
                            StyleId: styleId,
                            ItemName: $(this).find("td:eq(1)").text(),
                            StockById: parseInt($(this).find("td:eq(15)").text()),
                            ProductType: $(this).find("td:eq(18)").text(),
                            Quantity: parseFloat($(this).find("td:eq(9)").find("input").val()),
                            PurchasePrice: parseFloat($(this).find("td:eq(11)").text()),
                            BagQuantity: bagQuantity,
                            BonusAmount: bonusAmount
                        });
                    }
                    else if (receiveDetailsId > 0) {
                        ReceiveOrderItemDeleted.push({
                            ReceiveDetailsId: receiveDetailsId,
                            ReceivedId: parseInt(receiveOrderId),
                            ItemId: itemId,
                            ColorId: colorId,
                            SizeId: sizeId,
                            StyleId: styleId,
                            ItemName: $(this).find("td:eq(1)").text(),
                            StockById: parseInt($(this).find("td:eq(15)").text()),
                            ProductType: $(this).find("td:eq(18)").text(),
                            Quantity: parseFloat($(this).find("td:eq(9)").find("input").val()),
                            PurchasePrice: parseFloat($(this).find("td:eq(11)").find("input").val()),
                            BagQuantity: bagQuantity,
                            BonusAmount: bonusAmount
                        });

                        var serialCount = 0, rowSerial = 0;
                        //var itemSerial = _.where(AddedSerialzableProduct, { ItemId: itemId, ColorId: colorId, SizeId: sizeId, StyleId: styleId });
                        var itemSerial = _.where(AddedSerialzableProduct, { ItemId: itemId });
                        serialCount = itemSerial.length;

                        for (rowSerial = 0; rowSerial < serialCount; rowSerial++) {

                            if (itemSerial[rowSerial].SerialId > 0)
                                DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(itemSerial[rowSerial])));

                            var srlItem = _.findWhere(AddedSerialzableProduct, { SerialNumber: itemSerial[rowSerial].SerialNumber });
                            var srlIndex = _.indexOf(AddedSerialzableProduct, srlItem);
                            AddedSerialzableProduct.splice(srlIndex, 1);
                        }
                    }
                });
            }

            var row = 0, rowCount = ReceiveOrderItem.length;
            for (row = 0; row < rowCount; row++) {
                if (ReceiveOrderItem[row].ProductType == "Serial Product") {
                    //var serialTotal = _.where(AddedSerialzableProduct, { ItemId: ReceiveOrderItem[row].ItemId, ColorId: ReceiveOrderItem[row].ColorId, SizeId: ReceiveOrderItem[row].SizeId, StyleId: ReceiveOrderItem[row].StyleId });
                    var serialTotal = _.where(AddedSerialzableProduct, { ItemId: ReceiveOrderItem[row].ItemId });

                    if (ReceiveOrderItem[row].Quantity > serialTotal.length) {
                        toastr.warning("Please Give Serial Of Product " + ReceiveOrderItem[row].ItemName);
                        break;
                    }
                    else if (serialTotal.length > ReceiveOrderItem[row].Quantity) {
                        toastr.warning("Please Remove Serial Of Product " + ReceiveOrderItem[row].ItemName);
                        break;
                    }
                }
            }

            if (row != rowCount) {
                return false;
            }

            var ProductReceived = {
                ReceivedId: receiveOrderId,
                POrderId: porderId,
                ReceiveType: receiveType,
                PaymentType: paymentType,
                CostCenterId: costCenterId,
                LocationId: locationId,
                SupplierId: supplierId,
                Remarks: remarks,
                CurrencyId: currencyId,
                ConvertionRate: convertionRate,
                ReferenceBillDate: referenceBillDate,
                ReferenceNumber: referenceBillNo,
                CompanyId: companyId,
                ProjectId: projectId
            };

            var accountHeadId = "0", amount = "0", description = "", accoutHeadDetailsId = "0";
            var isEditOE = "0", finishProductIdOE = "0";

            var AddedOverheadExpenses = [], EditedOverheadExpenses = [];
            $("#OEAmountGrid tbody tr").each(function (index, item) {
                accoutHeadDetailsId = $.trim($(item).find("td:eq(4)").text());
                accountHeadId = $(item).find("td:eq(5)").text();
                amount = $(item).find("td:eq(1)").text();
                description = $(item).find("td:eq(2)").text();
                isEditOE = $(item).find("td:eq(6)").text();
                if (receiveDetailsId == "0") {
                    AddedOverheadExpenses.push({
                        ReceiveDetailsId: receiveDetailsId,
                        ReceivedId: parseInt(receiveOrderId),
                        NodeId: accountHeadId,
                        Amount: amount,
                        Remarks: description
                    });
                }
                else if (receiveDetailsId != "0") {
                    EditedOverheadExpenses.push({
                        ReceiveDetailsId: receiveDetailsId,
                        ReceivedId: parseInt(receiveOrderId),
                        NodeId: accountHeadId,
                        Amount: amount,
                        Remarks: description
                    });
                }
            });

            var accountHeadPMId = "0", amountPM = "0", descriptionPM = "", accoutHeadDetailsIdPM = "0";
            var isEditPM = "0", finishProductIdPM = "0";

            var AddedPaymentMethodInfos = [], EditedPaymentMethodInfos = [];
            $("#PMAmountGrid tbody tr").each(function (index, item) {
                accoutHeadDetailsIdPM = $.trim($(item).find("td:eq(4)").text());
                accountHeadPMId = $(item).find("td:eq(5)").text();
                amountPM = $(item).find("td:eq(1)").text();
                descriptionPM = $(item).find("td:eq(2)").text();
                isEditPM = $(item).find("td:eq(6)").text();
                if (receiveDetailsId == "0") {
                    AddedPaymentMethodInfos.push({
                        ReceiveDetailsId: receiveDetailsId,
                        ReceivedId: parseInt(receiveOrderId),
                        NodeId: accountHeadPMId,
                        Amount: amountPM,
                        Remarks: descriptionPM
                    });
                }
                else if (receiveDetailsId != "0") {
                    EditedOverheadExpenses.push({
                        ReceiveDetailsId: receiveDetailsId,
                        ReceivedId: parseInt(receiveOrderId),
                        NodeId: accountHeadPMId,
                        Amount: amountPM,
                        Remarks: descriptionPM
                    });
                }
            });

            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            debugger;
            if ($("#ContentPlaceHolder1_ddlPaymentType").val() == "CashNBank") {
                var totalForItems = $("#ContentPlaceHolder1_hfTotalForItems").val();
                var totalForPayment = $("#ContentPlaceHolder1_hftotalForPaymentInfos").val();
                var totalForOE = $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val();
                totalForItems = parseFloat(totalForItems);
                totalForPayment = parseFloat(totalForPayment);
                totalForOE = parseFloat(totalForOE);
                totalForItems = totalForItems.toFixed(2);
                totalForPayment = totalForPayment.toFixed(2);
                totalForOE = totalForOE.toFixed(2);
                var totalRM = parseFloat(totalForItems) + parseFloat(totalForOE);
                totalRM = totalRM.toFixed(2);
                debugger;
                if (totalRM != totalForPayment) {
                    toastr.warning("Total item price & Total Payment are not same.");
                    $("#ContentPlaceHolder1_txtPMAmount").focus();
                    return false;
                }
            }

            PageMethods.SavePurchaseWiseReceiveOrder(ProductReceived, ReceiveOrderItem, ReceiveOrderItemDeleted, AddedSerialzableProduct, DeletedSerialzableProduct, parseInt(randomDocId), deletedDoc, AddedOverheadExpenses, EditedOverheadExpenses, AddedPaymentMethodInfos, EditedPaymentMethodInfos, OnSavePurchaseOrderSucceed, OnSavePurchaseOrderFailed);

            return false;
        }
        function OnSavePurchaseOrderSucceed(result) {
            if (result.IsSuccess) {

                //$.ajax({
                //    type: "POST",
                //    contentType: "application/json; charset=utf-8",
                //    url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                //    data: JSON.stringify({ tableName: 'PMProductReceived', primaryKeyName: 'ReceivedId', primaryKeyValue: result.PrimaryKeyValue, featuresValue: 'Receive', statusColumnName: 'Status' }),
                //    dataType: "json",
                //    success: function (data) {
                //        debugger;

                //        SendSMSToUserList(data.d, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);

                //    },
                //    error: function (result) {
                //        toastr.error("Can not load Check or Approve By List.");
                //    }
                //});

                CommonHelper.AlertMessage(result.AlertMessage);

                if (queryReceiveOrderId != "") {
                    window.location = "/Inventory/ItemReceiveInfo.aspx";
                }

                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            $("#ContentPlaceHolder1_RandomDocId").val(result.Data);
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSavePurchaseOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }

        function PerformClearAction() {
            $('#doctablelist tbody tr').each(function (i, row) {
                $(this).find("td:eq(2) img").trigger('click')
            });
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './ItemReceive.aspx/ChangeRandomId',
                dataType: "json",
                async: false,
                success: function (data) {
                    $("#ContentPlaceHolder1_RandomDocId").val(data.d);
                },
                error: function (error) {
                }
            });

            $("#ContentPlaceHolder1_hfReceiveOrderId").val("0");
            $("#ContentPlaceHolder1_hfLocationId").val("0").trigger('change');

            $("#OEAmountGrid tbody").html("");
            $("#PMAmountGrid tbody").html("");
            $("#ItemForReceiveTbl tbody").html("");
            $("#ItemForReceiveTbl tfoot").html("");
            $("#PurchaseOrderItemTbl tbody").html("");
            $("#PurchaseItemForPReceiveTbl tbody").html("");
            $("#DocumentInfo").html("");
            $("#ContentPlaceHolder1_txtPMAmount").val("");
            $("#ContentPlaceHolder1_txtPMDescription").val("");
            $("#ContentPlaceHolder1_ddlAccountHead").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlPMAccountHead").val("0").trigger('change');
            $("#ContentPlaceHolder1_txttotalAmount").val("0");
            $("#ContentPlaceHolder1_txtPMTotalAmount").val("0");
            $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(0);
            $("#ContentPlaceHolder1_hfTotalForItems").val(0);
            $("#ContentPlaceHolder1_hfTotalOverheadExpenseAmount").val(0);
            $("#ContentPlaceHolder1_ddlSupplier").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlPurchaseOrderNumber").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlLCNumber").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlReceiveType").val("All").trigger('change');
            $("#ContentPlaceHolder1_ddlCostCentre").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlReceiveLocation").val("0").trigger('change');

            $("#ContentPlaceHolder1_ddlCategory").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_txtCurrentStock").val("").prop("disabled", false);
            $("#ContentPlaceHolder1_txtCurrentStockBy").val("").prop("disabled", false);
            $("#ContentPlaceHolder1_txtReceiveQuantity").val("");
            $("#ContentPlaceHolder1_txtPurchasePrice").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_ddlPaymentType").val("All").trigger('change');
            $("#ContentPlaceHolder1_ddlPaymentType").attr("disabled", false);

            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlPurchaseOrderNumber").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlReceiveLocation").attr("disabled", false);

            $("#ContentPlaceHolder1_txtReferenceBillDate").val("");
            $("#ContentPlaceHolder1_txtReferenceBillNo").val("");

            var x = document.getElementById("ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").length;
            if (x > 1) {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val("0").trigger('change');
            }

            var y = document.getElementById("ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").length;
            if (y > 1) {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val("0").trigger('change');
            }

            $("#ContentPlaceHolder1_ddlReceiveType").attr("disabled", false);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").attr("disabled", false);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").attr("disabled", false);
            //$("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val("0").trigger('change');


            queryReceiveOrderId = "";
            CurrencyList = new Array();
            ItemSelected = null;
            ReceiveOrderItem = new Array();
            ReceiveOrderItemDeleted = new Array();
            ReceiveOrderItemFromPurchase = new Array();
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();

            $("#btnSave").val("Save");
        }
        function PerformClearActionWithConfirmation() {

            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function SearchReceiveOrder(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#ReceiveOrderGrid tbody tr").length;
            var receiveNumber = "0", status = "", receiveType = "", supplierId = "0", costCenterId = "", fromDate = null, toDate = null;

            var companyId = $("#ContentPlaceHolder1_companyProjectUserControlSrc_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControlSrc_ddlGLProject").val();

            if (companyId == "0") {
                companyId = 0;
                projectId = 0;
            }

            receiveNumber = $("#ContentPlaceHolder1_txtSReceiveNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            receiveType = $("#ContentPlaceHolder1_ddlSearchReceiveType").val();
            costCenterId = $("#ContentPlaceHolder1_ddlCostCenterSearch").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            supplierId = $("#ContentPlaceHolder1_ddlSearchSupplier").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            if (costCenterId == "0")
                costCenterId = null;

            if (supplierId == "0")
                supplierId = null;

            $("#GridPagingContainer ul").html("");
            $("#ReceiveOrderGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchReceiveOrder(companyId, projectId, receiveType, fromDate, toDate, receiveNumber, status, costCenterId, supplierId,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

            return false;
        }
        function OnSearchPurchaseOrderSucceed(result) {

            var tr = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.ReceiveNumber + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.ReceiveType + "</td>";

                if (gridObject.ReceivedDate != null)
                    tr += "<td style='width:10%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.ReceivedDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:10%;'></td>";

                tr += "<td style='width:10%;'>" + gridObject.CostCenter + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.LocationName + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.SupplierName + "</td>";

                tr += "<td style='width:10%;'>" + (gridObject.Status == 'Pending' ? 'Submitted' : gridObject.Status) + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:15%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:15%;'></td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";


                if (gridObject.IsCanEdit && IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return ReceiveOrderEditWithConfirmation('" + gridObject.ReceiveType + "'," + gridObject.ReceivedId + "," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.POrderId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete && IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return ReceiveOrderDelete('" + gridObject.ReceiveType + "'," + gridObject.ReceivedId + ",'" + gridObject.Status + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked && IsCanSave) {

                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return ReceiveOrderCheckWithConfirmation('" + gridObject.ReceiveType + "','" + 'Checked' + "'," + gridObject.ReceivedId + "," + gridObject.SupplierId + "," + gridObject.POrderId + ")\" alt='Check'  title='Check' border='0' />";
                }

                if (gridObject.IsCanApproved && IsCanSave) {

                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return ReceiveOrderApprovalWithConfirmation('" + gridObject.ReceiveType + "','" + 'Approved' + "', " + gridObject.ReceivedId + "," + gridObject.SupplierId + "," + gridObject.POrderId + ")\" alt='Approve'  title='Approve' border='0' />";
                }

                //if (gridObject.Status == 'Approved') {
                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.ReceiveType + "'," + gridObject.ReceivedId + ",'" + gridObject.Status + "'," + gridObject.SupplierId + "," + gridObject.CostCenterId + "," + gridObject.CreatedBy + ")\" alt='Invoice' title='Receive Order Info' border='0' />";
                //}
                tr += "&nbsp;&nbsp;<img src='../Images/note.png'  onClick= \"javascript:return ShowDealDocuments('" + gridObject.ReceivedId + "')\" alt='Invoice' title='Receive Order Info' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.ReceivedId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.SupplierId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.CostCenterId + "</td>";

                tr += "</tr>";

                $("#ReceiveOrderGrid tbody").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }

        function ShowDealDocuments(id) {
            console.log("abc");
            PageMethods.LoadDealDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#dealDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                minHeight: 400,
                closeOnEscape: true,
                resizable: false,
                title: "Received Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        //function ShowUploadedOthersDocument(id) {
        //    console.log(id)
        //    PageMethods.GetUploadedDocumentsByWebMethod(id, "ReceiveOrderDocuments", OnGetUploadedOthersDocumentByWebMethodSucceeded, OnGetUploadedOthersDocumentByWebMethodFailed);
        //    return false;
        //}

        //function OnGetUploadedOthersDocumentByWebMethodSucceeded(result) {

        //    var totalDoc = result.length;
        //    var row = 0;
        //    var imagePath = "";
        //    DocTable = "";

        //    DocTable += "<table id='DocTableList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
        //    DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

        //    for (row = 0; row < totalDoc; row++) {
        //        if (row % 2 == 0) {
        //            DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
        //        }
        //        else {
        //            DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
        //        }
        //        DocTable += "<td align='left' style='width: 50%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

        //        if (result[row].Path != "") {
        //            if (result[row].Extention == ".jpg" || result[row].Extention == ".png") {
        //                imagePath = "<img src='" + result[row].Path + result[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
        //            }
        //            else {
        //                imagePath = "<img src='" + result[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
        //            }
        //        }
        //        else
        //            imagePath = "";

        //        DocTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + imagePath + "</td>";

        //        DocTable += "<td align='left' style='width: 20%'>";
        //        DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
        //        DocTable += "</td>";
        //        DocTable += "</tr>";
        //    }
        //    DocTable += "</table>";

        //    docc = DocTable;

        //    $("#ContentPlaceHolder1_DocumentInfo").html(DocTable);
        //    $('#OthersDocDiv').html(result);
        //    return false;
        //}

        function ShowDocument(path, name) {
            console.log("show document");
            var iframeid = 'fileIframe';
            document.getElementById(iframeid).src = path;
            $("#ShowDocumentDiv").dialog({
                autoOpen: true,
                modal: true,
                width: "82%",
                height: 1000,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Document - " + name,
                show: 'slide'
            });
            return false;
        }

        //function OnGetUploadedOthersDocumentByWebMethodFailed(error) {
        //    toastr.error(error.get_message());
        //}


        function OnSearchPurchaseOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }
        function ClearSearch() {

            $("#ContentPlaceHolder1_txtSReceiveNumber").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("All");
            $("#ContentPlaceHolder1_ddlSearchReceiveType").val("All");
            $("#ContentPlaceHolder1_ddlCostCenterSearch").val("0");
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");
            $("#ContentPlaceHolder1_ddlSearchSupplier").val("0").trigger("change");
        }

        function ReceiveOrderEdit(ReceiveType, ReceivedId, SupplierId, CostCenterId, POrderId) {

            PageMethods.EditReceiveOrder(ReceivedId, POrderId, OnEditPurchaseOrderSucceed, OnEditPurchaseOrderFailed);
            return false;
        }
        function ReceiveOrderEditWithConfirmation(ReceiveType, ReceivedId, SupplierId, CostCenterId, POrderId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            ReceiveOrderEdit(ReceiveType, ReceivedId, SupplierId, CostCenterId, POrderId);
        }
        function OnEditPurchaseOrderSucceed(result) {
            debugger;
            $("#SerialItemTable tbody").html("");
            $("#OEAmountGrid tbody").html("");
            $("#PMAmountGrid tbody").html("");
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();
            AddedSerialCount = 0;
            $("#ContentPlaceHolder1_txtReferenceBillNo").val(result.ProductReceived.ReferenceNumber);
            if (result.ProductReceived.ReferenceBillDate != null) {
                $("#ContentPlaceHolder1_txtReferenceBillDate").val(GetStringFromDateTime(result.ProductReceived.ReferenceBillDate));
            }

            $("#ContentPlaceHolder1_hfReceiveOrderId").val(result.ProductReceived.ReceivedId);

            if (result.ProductReceived.ReceiveType == "AdHoc" || result.ProductReceived.ReceiveType == "Purchase") {
                OverheadExpenseEdit(result.OverheadExpenseInfoList);
            }

            PaymentMethodInformationEdit(result.PaymentInformationList);

            $("#ContentPlaceHolder1_ddlPaymentType").val(result.ProductReceived.PaymentType);
            $("ContentPlaceHolder1_ddlReceiveType").val(result.ProductReceived.ReceiveType);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(result.ProductReceived.CompanyId).trigger('change');
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(result.ProductReceived.ProjectId).trigger('change');

            $("#ContentPlaceHolder1_ddlPaymentType").attr("disabled", true);
            $("ContentPlaceHolder1_ddlReceiveType").attr("disabled", true);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").attr("disabled", true);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").attr("disabled", true);

            if (result.ProductReceived.ReceiveType == "AdHoc") {
                if ($("#ContentPlaceHolder1_ddlPaymentType").val() == "CashNBank") {
                    $("#PMEntryPanel").show();
                    $("#SupplierPanel").hide();
                } else {
                    $("#PMEntryPanel").hide();
                    $("#SupplierPanel").show();
                }

                $("#AdhocReceive").show();
                $("#AdhocReceiveItem").show();
                $("#ReceiveOrderItemContainer").hide();
                $("#PurchaseOrderCostCenterContainer").hide();

                $("#ItemForReceiveTbl tbody").html("");
                $("#PurchaseOrderItemTbl tbody").html("");
                $("#PurchaseItemForPReceiveTbl tbody").html("");

                AdhocReceiveOrderEdit(result);

                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlReceiveLocation").attr("disabled", true);
            }
            else {

                $("#AdhocReceive").hide();
                $("#AdhocReceiveItem").hide();
                $("#ReceiveOrderItemContainer").show();
                $("#PurchaseOrderCostCenterContainer").show();
                $("#ItemForReceiveTbl tbody").html("");
                $("#PurchaseOrderItemTbl tbody").html("");
                $("#PurchaseItemForPReceiveTbl tbody").html("");

                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);

                PurchaseOrderWiseReceiveOrderEdit(result);
            }
            ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());
        }
        function OnEditPurchaseOrderFailed() { }

        function OverheadExpenseEdit(result) {
            $.each(result, function (count, obj) {
                var isEdited = "0";
                var rowLength = $("#OEAmountGrid tbody tr").length;

                var tr = "";

                if (rowLength % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:30%;'>" + obj.AccountHead + "</td>";
                tr += "<td style='width:10%;'>" + obj.Amount + "</td>";
                tr += "<td style='width:50%;'>" + obj.Remarks + "</td>";
                tr += "<td style='width:10%;'><a href='javascript:void();' onclick= 'javascript:return DeleteAccountHeadOfOE(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

                tr += "<td style='display:none'>" + obj.ReceivedId + "</td>";
                tr += "<td style='display:none'>" + obj.NodeId + "</td>";
                tr += "<td style='display:none'>" + isEdited + "</td>";
                tr += "</tr>";

                $("#OEAmountGrid tbody").append(tr);
                var totalAmount = 0;
                $("#OEAmountGrid tr").each(function () {
                    var amount = $(this).find("td").eq(1).html();
                    if (amount == undefined) {
                        amount = 0;
                    }
                    totalAmount = parseFloat(totalAmount) + parseFloat(amount);
                });
                totalAmount = totalAmount.toFixed(2);
                $("#ContentPlaceHolder1_txttotalAmount").val(totalAmount);
            });
        }

        function PaymentMethodInformationEdit(result) {
            $.each(result, function (count, obj) {
                var isEdited = "0";
                var rowLength = $("#PMAmountGrid tbody tr").length;

                var tr = "";

                if (rowLength % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:30%;'>" + obj.AccountHead + "</td>";
                tr += "<td style='width:10%;'>" + obj.Amount + "</td>";
                tr += "<td style='width:50%;'>" + obj.Remarks + "</td>";
                tr += "<td style='width:10%;'><a href='javascript:void();' onclick= 'javascript:return DeleteAccountHeadOfPM(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

                tr += "<td style='display:none'>" + obj.ReceivedId + "</td>";
                tr += "<td style='display:none'>" + obj.NodeId + "</td>";
                tr += "<td style='display:none'>" + isEdited + "</td>";
                tr += "</tr>";

                $("#PMAmountGrid tbody").append(tr);
                var totalAmount = 0;
                $("#PMAmountGrid tr").each(function () {
                    var amount = $(this).find("td").eq(1).html();
                    if (amount == undefined) {
                        amount = 0;
                    }
                    totalAmount = parseFloat(totalAmount) + parseFloat(amount);
                });
                totalAmount = totalAmount.toFixed(2);
                $("#ContentPlaceHolder1_txtPMTotalAmount").val(totalAmount);
                $("#ContentPlaceHolder1_hftotalForPaymentInfos").val(totalAmount);
            });
        }

        function AdhocReceiveOrderEdit(result) {
            debugger;
            LoadForEditReceiveOrder(result);
            var tr = "";

            $.each(result.ProductReceivedDetails, function (count, obj) {
                tr += "<tr>";
                tr += "<td style='width:25%;'>" + obj.ItemName + "</td>";

                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    tr += "<td style='width:10%;'>" + obj.ColorText + "</td>";
                    tr += "<td style='width:10%;'>" + obj.SizeText + "</td>";
                    tr += "<td style='width:10%;'>" + obj.StyleText + "</td>";
                }
                else {
                    tr += "<td style='display:none'>" + obj.ColorText + "</td>";
                    tr += "<td style='display:none'>" + obj.SizeText + "</td>";
                    tr += "<td style='display:none'>" + obj.StyleText + "</td>";
                }

                tr += "<td style='width:8%;'>" +
                    "<input type='text' value='" + obj.PurchasePrice + "' id='pq" + obj.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                    "</td>";
                tr += "<td style='width:8%;'>" +
                    "<input type='text' value='" + obj.Quantity + "' id='pp" + obj.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                    "</td>";

                tr += "<td style='width:8%;'>" + obj.StockBy + "</td>";
                tr += "<td style='width:8%;'>" + toFixed((obj.Quantity * obj.PurchasePrice), 2) + "</td>";

                tr += "<td style='width:5%;'>" +
                    "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

                if (obj.ProductType == "Serial Product") {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForAdHocItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.ItemId + "</td>";
                tr += "<td style='display:none;'>0</td>";
                tr += "<td style='display:none;'>" + obj.StockById + "</td>";

                tr += "<td style='display:none;'>" + obj.PurchasePrice + "</td>";
                tr += "<td style='display:none;'>" + obj.Quantity + "</td>";
                tr += "<td style='display:none;'>" + obj.ReceiveDetailsId + "</td>";

                tr += "<td style='display:none'>" + obj.ColorId + "</td>";
                tr += "<td style='display:none'>" + obj.SizeId + "</td>";
                tr += "<td style='display:none'>" + obj.StyleId + "</td>";

                tr += "<td style='display:none'>" + obj.BagQuantity + "</td>";
                tr += "<td style='display:none'>" + obj.BonusAmount + "</td>";

                tr += "</tr>";

                $("#ItemForReceiveTbl tbody").append(tr);
                tr = "";
            });

            //ReceiveOrderItem.push({
            //    ItemId: parseInt(ItemSelected.ItemId, 10),
            //    ColorId: parseInt(colorId, 10),
            //    SizeId: parseInt(sizeId, 10),
            //    StyleId: parseInt(styleId, 10),
            //    ItemName: ItemSelected.ItemName,
            //    StockById: parseInt(ItemSelected.StockBy, 10),
            //    Quantity: parseFloat(quantity),
            //    PurchasePrice: parseFloat(unitPrice),
            //    ProductType: ItemSelected.ProductType,
            //    Remarks: remarks,
            //    BagQuantity: bagQuantity,
            //    BonusAmount: bonusAmount,
            //    DetailId: 0
            //});

            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlReceiveLocation").attr('disabled', true);

            ReceiveOrderItem = result.ProductReceivedDetails;

            CommonHelper.ApplyDecimalValidation();
            ClearAfterAdhoqReceiveItemAdded();

            CommonHelper.ApplyDecimalValidation();
            ClearAfterAdhoqReceiveItemAdded();
            $("#ContentPlaceHolder1_txtItem").focus();
            var total = 0;
            $("#ItemForReceiveTbl tbody tr").each(function () {
                total += toFixed(parseFloat($(this).find("td:eq(7)").text()), 2);
            });
            $("#ContentPlaceHolder1_hfTotalForItems").val(total);
            $("#ItemForReceiveTbl tfoot").find("tr:eq(0)").remove();
            tr += "<tr>";
            tr += "<td style='width:25%;'> </td>";

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                tr += "<td style='width:10%;'> </td>";
                tr += "<td style='width:10%;'> </td>";
                tr += "<td style='width:10%;'> </td>";
            }
            else {
                tr += "<td style='display:none'></td>";
                tr += "<td style='display:none'></td>";
                tr += "<td style='display:none'></td>";
            }

            tr += "<td style='width:10%;'> </td>";
            tr += "<td style='width:10%;'> </td>";
            tr += "<td style='width:10%;font-weight:bold;'> Total </td>";
            tr += "<td style='width:15%;font-weight:bold;'>" + total + "</td>";

            tr += "<td style='display:none'>" + 0 + "</td>";
            tr += "<td style='display:none'>" + 0 + "</td>";
            tr += "<td style='display:none'>" + 0 + "</td>";

            tr += "<td style='display:none'>" + 0 + "</td>";
            tr += "<td style='display:none'>" + 0 + "</td>";

            tr += "<td style='width:15%;'> </td>";
            tr += "</tr>";

            $("#ItemForReceiveTbl tfoot").prepend(tr);
            tr = "";


            $("#myTabs").tabs({ active: 0 });
        }

        function PurchaseOrderWiseReceiveOrderEdit(result) {

            LoadForEditReceiveOrder(result);
            var tr = "";

            $.each(result.ProductReceivedDetailsSummary, function (count, obj) {
                tr += "<tr>";

                if (obj.ReceiveDetailsId > 0) {
                    tr += "<td style='width:5%;'>" +
                        "<input type='checkbox' checked='checked' />" +
                        "</td>";
                }
                else {
                    tr += "<td style='width:5%;'>" +
                        "<input type='checkbox' />" +
                        "</td>";
                }
                tr += "<td style='width:25%;'>" + obj.ItemName + "</td>";

                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    tr += "<td style='width:10%;'>" + obj.ColorText + "</td>";
                    tr += "<td style='width:10%;'>" + obj.SizeText + "</td>";
                    tr += "<td style='width:10%;'>" + obj.StyleText + "</td>";
                }
                else {
                    tr += "<td style='display:none'>" + obj.ColorText + "</td>";
                    tr += "<td style='display:none'>" + obj.SizeText + "</td>";
                    tr += "<td style='display:none'>" + obj.StyleText + "</td>";
                }

                tr += "<td style='width:13%;'>" + obj.PurchaseQuantity + "</td>";
                tr += "<td style='width:13%;'>" + obj.QuantityReceived + "</td>";
                tr += "<td style='width:13%;'>" + obj.RemainingReceiveQuantity + "</td>";
                tr += "<td style='width:13%;'>" + obj.StockBy + "</td>";

                tr += "<td style='width:13%;'>" +
                    "<input type='text' value='" + obj.Quantity + "' id='q' " + obj.ItemId + " class='form-control quantitydecimal' onblur='CheckPurchaseOrderWiseQuantity(this)' />" +
                    "</td>";
                tr += "<td style='width:10%; display: none;'>" +
                    "<input type='text' disabled='disabled' value='" + obj.PurchasePrice + "' id='pp' " + obj.ItemId + " class='form-control quantitydecimal' onblur='CheckPurchaseOrderPriceWithConfirmation(this)' />" +
                    "</td>";

                tr += "<td style='width:10%; display: none;'>" + obj.PurchasePrice + "</td>";

                tr += "<td style='width:5%;'>";
                if (obj.ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'AddSerialForPurchaseWiseItem(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.POrderId + "</td>";
                tr += "<td style='display:none;'>" + obj.ItemId + "</td>";
                tr += "<td style='display:none;'>" + obj.StockById + "</td>";
                tr += "<td style='display:none;'>" + obj.RemainingReceiveQuantity + "</td>";
                tr += "<td style='display:none;'>" + obj.PurchasePrice + "</td>";
                tr += "<td style='display:none;'>" + obj.ProductType + "</td>";
                tr += "<td style='display:none;'>" + obj.ReceiveDetailsId + "</td>";

                tr += "<td style='display:none'>" + obj.ColorId + "</td>";
                tr += "<td style='display:none'>" + obj.SizeId + "</td>";
                tr += "<td style='display:none'>" + obj.StyleId + "</td>";

                tr += "</tr>";

                $("#PurchaseOrderItemTbl tbody").append(tr);
                tr = "";
            });

            ReceiveOrderItem = result.ProductReceivedDetails;
            ReceiveOrderItemFromPurchase = result.ProductReceivedDetailsSummary;

            CommonHelper.ApplyDecimalValidation();

            $("#myTabs").tabs({ active: 0 });
        }

        function LoadForEditReceiveOrder(result) {
            debugger;
            AddedSerialzableProduct = result.ProductSerialInfo;

            if (result.ProductSerialInfo != null) {
                if (result.ProductSerialInfo.length > 0) {
                    $("#lblAddedQuantity").text(result.ProductSerialInfo.length);
                    AddedSerialCount = result.ProductSerialInfo.length;
                }
            }

            $("#ContentPlaceHolder1_hfCostCenterId").val(result.ProductReceived.CostCenterId);

            $("#ContentPlaceHolder1_ddlReceiveType").val(result.ProductReceived.ReceiveType).trigger('change');

            $("#ContentPlaceHolder1_hfReceiveOrderId").val(result.ProductReceived.ReceivedId);
            if (result.ProductReceived.ReceiveType == "Purchase") {
                $("#ContentPlaceHolder1_ddlPurchaseOrderNumber").val(result.ProductReceived.POrderId + '').trigger('change');

                $("#ContentPlaceHolder1_ddlPurchaseOrderNumber").attr("disabled", true);
            }
            else if (result.ProductReceived.ReceiveType == "LC") {
                $("#ContentPlaceHolder1_ddlLCNumber").val(result.ProductReceived.POrderId + '').trigger('change');

                $("#ContentPlaceHolder1_ddlLCNumber").attr("disabled", true);
            }
            $("#ContentPlaceHolder1_ddlReceiveType").attr("disabled", true);

            //$("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(result.ProductReceived.CompanyId).trigger('change');
            //$("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(result.ProductReceived.CompanyId).trigger('change');
            //$("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(result.ProductReceived.ProjectId).trigger('change');

            $("#ContentPlaceHolder1_ddlSupplier").val(result.ProductReceived.SupplierId + '').trigger('change');
            $("#ContentPlaceHolder1_ddlCostCentre").val(result.ProductReceived.CostCenterId + '').trigger('change');
            $("#ContentPlaceHolder1_ddlCurrency").val(result.ProductReceived.CurrencyId + '');
            $("#ContentPlaceHolder1_lblConversionRate").text(result.ProductReceived.ConvertionRate);

            $("#ContentPlaceHolder1_hfLocationId").val(result.ProductReceived.LocationId);
            LoadStoreLocationByCostCenter(result.ProductReceived.CostCenterId);

            if (result.ProductReceived.Remarks != null)
                $("#ContentPlaceHolder1_txtRemarks").val(result.ProductReceived.Remarks);


            if (IsCanEdit) {
                $('#btnSave').show();
            } else {
                $('#btnSave').hide();
            }
            $("#btnSave").val("Update");
        }

        function ReceiveOrderDelete(ReceiveType, ReceivedId, ApprovedStatus, SupplierId, CostCenterId, CreatedBy) {

            if (!confirm("Do you Want To Delete?")) {
                return false;
            }

            PageMethods.ReceiveOrderDelete(ReceiveType, ReceivedId, ApprovedStatus, CreatedBy, OnApprovalSucceed, OnApprovalFailed);
        }

        function ReceiveOrderApproval(ReceiveType, ApprovedStatus, ReceivedId, SupplierId, POrderId) {

            PageMethods.ReceiveOrderApproval(ReceiveType, ReceivedId, ApprovedStatus, POrderId, OnApprovalSucceed, OnApprovalFailed);
        }
        function ReceiveOrderApprovalWithConfirmation(ReceiveType, ReceivedId, ApprovedStatus, POrderId, OnApprovalSucceed, OnApprovalFailed) {

            if (!confirm("Do you Want To Approve?")) {
                return false;
            }
            ReceiveOrderApproval(ReceiveType, ReceivedId, ApprovedStatus, POrderId, OnApprovalSucceed, OnApprovalFailed);
        }
        function ReceiveOrderCheckWithConfirmation(ReceiveType, ReceivedId, ApprovedStatus, POrderId, OnApprovalSucceed, OnApprovalFailed) {
            if (!confirm("Do you Want To Check?")) {
                return false;
            }
            ReceiveOrderApproval(ReceiveType, ReceivedId, ApprovedStatus, POrderId, OnApprovalSucceed, OnApprovalFailed);

        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                    data: JSON.stringify({ tableName: 'PMProductReceived', primaryKeyName: 'ReceivedId', primaryKeyValue: result.PrimaryKeyValue, featuresValue: 'Receive', statusColumnName: 'Status' }),
                    dataType: "json",
                    success: function (data) {
                        debugger;

                        SendSMSToUserList(data.d, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);

                    },
                    error: function (result) {
                        toastr.error("Can not load Check or Approve By List.");
                    }
                });

                LoadNotReceivedPurchaseOrder();
                SearchReceiveOrder($("#GridPagingContainer").find("li.active").index(), 1);
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

        function ShowReport(ReceiveType, ReceivedId, ApprovedStatus, SupplierId, CostCenterId, CreatedBy) {
            var iframeid = 'printDoc';
            var url = "Reports/frmReportProductReceive.aspx?PRId=" + ReceivedId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Receive Invoice",
                show: 'slide'
            });
        }

        function AddSerialForAdHocItem(control) {
            var tr = $(control).parent().parent();

            var itemName = $(tr).find("td:eq(0)").text();
            var itemId = $(tr).find("td:eq(9)").text();
            var quantity = $(tr).find("td:eq(5)").find("input").val();

            SearialAddedWindow(itemName, itemId, quantity);
        }
        function AddSerialForPurchaseWiseItem(control) {
            var tr = $(control).parent().parent();
            var itemName = $(tr).find("td:eq(1)").text();
            var itemId = $(tr).find("td:eq(14)").text();
            //var quantity = $(tr).find("td:eq(6)").find("input").val();
            var quantity = $(tr).find("td:eq(9)").find("input").val();
            debugger;
            SearialAddedWindow(itemName, itemId, quantity);
        }
        function SearialAddedWindow(itemName, itemId, quantity) {
            ClearSerial();
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
                        tr += "<td style='display:none;'>" + addedSerial[row].SerialId + "</td>";

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
            var serial = $.trim($("#txtSerial").val());
            var addedQuantity = $("#lblAddedQuantity").text();
            var totalQuantity = $("#lblItemQuantity").text();

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

            var receiveOrderId = $("#ContentPlaceHolder1_hfReceiveOrderId").val();
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
                SerialId: 0,
                ReceivedId: parseInt(receiveOrderId, 10),
                ItemId: parseInt(itemId, 10),
                SerialNumber: serial
            });

            AddedSerialCount = AddedSerialCount + 1;
            $("#lblAddedQuantity").text(AddedSerialCount);

            tr = "";
            $("#txtSerial").val("");
            $("#txtSerial").focus();
        }
        function ClearSerial() {
            $("#txtSerial").val("");
        }
        function ClearSerialWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            ClearSerial();
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
                    SerialId: obj.SerialId,
                    ReceivedId: obj.ReceivedId,
                    ItemId: obj.ItemId,
                    SerialNumber: obj.SerialNumber
                });
            });

            $("#SerialWindow").dialog("close");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("");
            AddedSerialCount = 0;
            NewAddedSerial = new Array();
        }
        function ApplySerialForPurchaseItemWithConfirmation() {
            if (!confirm("Do you Want To Apply?")) {
                return false;
            }

            ApplySerialForPurchaseItem();
        }
        function DeleteItemSerial(control) {
            if (!confirm("Do you Want To Delete?")) {
                return false;
            }

            var tr = $(control).parent().parent();

            var itemId = parseInt($(tr).find("td:eq(2)").text(), 10);
            var serialId = parseInt($(tr).find("td:eq(3)").text(), 10);

            var addedQuantity = $("#lblAddedQuantity").text();
            AddedSerialCount = parseInt(addedQuantity, 10) - 1;

            var item = _.findWhere(AddedSerialzableProduct, { ItemId: itemId });
            var index = _.indexOf(AddedSerialzableProduct, item);
            AddedSerialzableProduct.splice(index, 1);

            var itemNew = _.findWhere(NewAddedSerial, { ItemId: itemId });
            var indexNew = _.indexOf(NewAddedSerial, itemNew);
            NewAddedSerial.splice(indexNew, 1);

            if (serialId > 0) {
                DeletedSerialzableProduct.push(JSON.parse(JSON.stringify(item)));
            }

            $("#lblAddedQuantity").text(AddedSerialCount);
            $(tr).remove();
        }

        function CancelAddSerial() {

            $("#SerialWindow").dialog("close");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("");
            AddedSerialCount = 0;
        }

        function CheckAllOrder() {
            if ($("#OrderCheck").is(":checked")) {
                $("#PurchaseOrderItemTbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#PurchaseOrderItemTbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchReceiveOrder(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadDocUploader() {
            //$("#popUpImage").dialog({
            //    width: 650,
            //    height: 300,
            //    autoOpen: true,
            //    modal: true,
            //    closeOnEscape: true,
            //    resizable: false,
            //    fluid: true,
            //    title: "", // TODO add title
            //    show: 'slide'
            //});

            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/Inventory/Images/Receive/";
            var category = "ReceiveOrderDocuments";
            var iframeid = 'frmPrint';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#DocumentDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 300,
                closeOnEscape: false,
                resizable: false,
                title: "Documents Upload",
                show: 'slide'
            });

            return false;
        }
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocument(randomId);
        }

        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfReceiveOrderId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.GetUploadedDocByWebMethod(randomId, id, deletedDoc, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
        }

        function OnGetUploadedDocByWebMethodSucceeded(result) {
            console.log("abc");
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            DocTable = "";

            DocTable += "<table id='DocTableList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                DocTable += "<td align='left' style='width: 50%'>" + result[row].Name + "</td>";


                if (result[row].Path != "") {
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                DocTable += "<td align='left' style='width: 20%'>";
                DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                DocTable += "</td>";
                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#DocumentInfo").html(DocTable);

            return false;
        }
        function DeleteDoc(docId, rowIndex) {
            var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();

            if (deletedDoc != "")
                deletedDoc += "," + docId;
            else
                deletedDoc = docId;

            $("#<%=hfDeletedDoc.ClientID %>").val(deletedDoc);

            $("#trdoc" + rowIndex).remove();
        }
        function OnGetUploadedDocByWebMethodFailed(error) {
            alert(error.get_message());
        }

    </script>
    <div id="dealDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <asp:HiddenField ID="hfTotalForItems" runat="server" Value="0" />
    <asp:HiddenField ID="hfTotalOverheadExpenseAmount" runat="server" Value="0" />
    <asp:HiddenField ID="hftotalForPaymentInfos" runat="server" Value="0" />
    <asp:HiddenField ID="hfCostCenterId" runat="server" Value="0" />
    <asp:HiddenField ID="hfAccoutHeadId" runat="server" Value="0" />
    <asp:HiddenField ID="hfAccoutHeadPMId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="hfCurrencyObj" runat="server" Value="" />
    <asp:HiddenField ID="hfPurchaseOrderObj" runat="server" Value="" />
    <asp:HiddenField ID="hfLcInfoObj" runat="server" Value="" />
    <asp:HiddenField ID="hfDefaultCurrencyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCategoryId" runat="server" Value="0" />
    <asp:HiddenField ID="hfStockById" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hfReceiveOrderId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemIdForSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfSerialQuantity" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsItemAttributeEnable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsItemReceiveOverheadExpenseEnable" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Receive Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Item Receive</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-heading">Receive Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Receive Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReceiveType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="--- Please Select ---" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="AdHoc" Value="AdHoc"></asp:ListItem>
                                    <asp:ListItem Text="Purchase Order" Value="Purchase"></asp:ListItem>
                                    <asp:ListItem Text="LC" Value="LC"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="PurchaseType">
                                <div class="col-md-2">
                                    <asp:Label ID="lblPurchaseType" runat="server" class="control-label required-field" Text="Payment Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="--- Please Select ---" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="Credit Payment" Value="Credit"></asp:ListItem>
                                        <asp:ListItem Text="Cash/Bank Payment" Value="CashNBank"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="PONumber">
                                <div class="col-md-2">
                                    <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="PO Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlPurchaseOrderNumber" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="LCNumber">
                                <div class="col-md-2">
                                    <asp:Label ID="Label11" runat="server" class="control-label required-field" Text="LC Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlLCNumber" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                        <div class="form-group">
                            <div id="SupplierPanel">
                                <div class="col-md-2">
                                    <asp:Label ID="lblSupplier" runat="server" class="control-label required-field" Text="Supplier"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Receive Store"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCostCentre" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label for="" class="control-label required-field">
                                    Order Currency</label>
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
                                            <asp:Label ID="lblConversionRate" runat="server" class="form-control" Text="0"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label9" runat="server" class="control-label required-field" Text="Store Location"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlReceiveLocation" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReferenceBill" runat="server" class="control-label " Text="Reference Bill No:"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReferenceBillNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblBillDate" runat="server" class="control-label " Text="Reference Bill Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReferenceBillDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" id="PurchaseOrderCostCenterContainer">
                            <div class="col-md-2">
                                <asp:Label ID="Label13" runat="server" class="control-label" Text="Purchase Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlPurchaseOrderCostcenter" disabled="disabled" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading">Item Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div id="ReceiveOrderItemContainer" style="height: 300px; overflow-y: scroll;">
                            <table id="PurchaseOrderItemTbl" class="table table-bordered table-condensed table-hover">
                                <thead>
                                    <tr>
                                        <th style="width: 5%; text-align: center;">Select
                                            <input type="checkbox" value="" checked="checked" id="OrderCheck" onclick="CheckAllOrder()" />
                                        </th>
                                        <th style="width: 25%;">Item Name</th>
                                        <th id="cPOId" style="width: 10%;">Color</th>
                                        <th id="sPOId" style="width: 10%;">Size</th>
                                        <th id="stPOId" style="width: 10%;">Style</th>
                                        <th style="width: 13%;">Purchase Order Quantity</th>
                                        <th style="width: 13%;">Pre. Received Quantity</th>
                                        <th style="width: 13%;">Max Quantity Can Receive</th>
                                        <th style="width: 13%;">Unit Head</th>
                                        <th style="width: 13%;">Receive Quantity</th>
                                        <th style="width: 10%; display: none;">Price</th>
                                        <th style="width: 10%; display: none;">Total</th>
                                        <th style="width: 5%;">Action</th>
                                        <th style="display: none;">POrderId</th>
                                        <th style="display: none;">ItemId</th>
                                        <th style="display: none;">StockById</th>
                                        <th style="display: none;">Quantity</th>
                                        <th style="display: none">Color Id</th>
                                        <th style="display: none">Size Id</th>
                                        <th style="display: none">Style Id</th>
                                        <th style="display: none;">PurchasePrice</th>
                                        <th style="display: none;">ProductType</th>
                                        <th style="display: none;">ReceiveDetailsId</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                        <div id="AdhocReceive">
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
                                    <asp:TextBox ID="txtItem" runat="server" placeholder="Enter minimum 3 characters" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div id="AttributeDiv" style="display: none;">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label15" runat="server" class="control-label" Text="Color"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlColorAttribute" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label16" runat="server" class="control-label" Text="Size"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlSizeAttribute" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label17" runat="server" class="control-label" Text="Style"></asp:Label>
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
                                    <asp:TextBox ID="txtCurrentStock" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label6" runat="server" class="control-label" Text="Unit"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCurrentStockBy" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Quantity"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtReceiveQuantity" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label8" runat="server" class="control-label required-field" Text="Unit Price"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPurchasePrice" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" id="BagAndBonusDiv" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="Label18" runat="server" class="control-label" Text="Bag Quantity"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtBagQuantity" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label19" runat="server" class="control-label" Text="Bonus Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtBonusAmount" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="padding-top: 10px;">
                                <div class="col-md-12">
                                    <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItemForReceive()" />
                                    <input id="btnCancelOrder" type="button" value="Cancel" onclick="ClearAfterAdhoqReceiveItemAdded()"
                                        class="TransactionalButton btn btn-primary btn-sm" />
                                </div>
                            </div>
                        </div>
                        <div id="AdhocReceiveItem" style="overflow-y: scroll;">
                            <table id="ItemForReceiveTbl" class="table table-bordered table-condensed table-hover">
                                <thead>
                                    <tr>
                                        <th style="width: 25%;">Item Name</th>
                                        <th id="cId" style="width: 10%;">Color</th>
                                        <th id="sId" style="width: 10%;">Size</th>
                                        <th id="stId" style="width: 10%;">Style</th>
                                        <th style="width: 15%;">Unit Price</th>
                                        <th style="width: 10%;">Quantity</th>
                                        <th style="width: 10%;">Unit Head</th>
                                        <th style="width: 15%;">Total</th>
                                        <th style="width: 15%;">Action</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                                <tfoot></tfoot>
                            </table>
                        </div>
                    </div>
                    <hr />
                    <div id="OEEntryPanel" class="form-group" style="display: none">
                        <div class="panel panel-default">
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
                                            <asp:DropDownList ID="ddlAccountHead" runat="server" CssClass="form-control" TabIndex="70">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblAmount" runat="server" class="control-label required-field" Text="Amount"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtAmount" runat="server" TabIndex="71" CssClass="form-control quantitydecimal"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblOEDescription" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtOEDescription" runat="server" TabIndex="72" CssClass="form-control"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 5px 0 5px 0;">
                                        <div class="col-md-12">
                                            <button type="button" id="btnAddOEAmount" tabindex="73" class="TransactionalButton btn btn-primary btn-sm">
                                                Add</button>
                                            <button type="button" id="btnCancelOEAmount" class="TransactionalButton btn btn-primary btn-sm">
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
                    <hr />
                    <div id="PMEntryPanel" class="form-group" style="display: none">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Payment Method Information
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblPMAccountHead" runat="server" class="control-label required-field" Text="Account Head"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlPMAccountHead" runat="server" CssClass="form-control" TabIndex="74">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblPMAmount" runat="server" class="control-label required-field" Text="Amount"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtPMAmount" runat="server" TabIndex="75" CssClass="form-control quantitydecimal"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblPMDescription" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtPMDescription" runat="server" TabIndex="76" CssClass="form-control"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 5px 0 5px 0;">
                                        <div class="col-md-12">
                                            <button type="button" id="btnAddPMAmount" tabindex="77" class="TransactionalButton btn btn-primary btn-sm">
                                                Add</button>
                                            <button type="button" id="btnCancelPMAmount" class="TransactionalButton btn btn-primary btn-sm">
                                                Cancel</button>
                                        </div>
                                    </div>
                                    <div class="form-group" style="padding: 0px;">
                                        <div id="PMAmountGridContainer">
                                            <table id="PMAmountGrid" class="table table-bordered table-condensed table-responsive"
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
                                                        <th style="display: none">AccountHeadId
                                                        </th>
                                                        <th style="display: none">IsEdited
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
                                                    <asp:Label ID="lblPMTotalAmount" runat="server" class="control-label" Text="Total Amount :"></asp:Label>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:TextBox ID="txtPMTotalAmount" runat="server" ReadOnly="true" CssClass="form-control quantitydecimal"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label ID="lblPaymentAmount" runat="server" class="control-label" Text="Payment Amount :"></asp:Label>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:TextBox ID="txtPaymentAmount" runat="server" ReadOnly="true" CssClass="form-control quantitydecimal"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row" style="padding: 5px 0 5px 0;">
                                                <div class="col-md-3">
                                                    <asp:Label ID="lblDueAmount" runat="server" class="control-label" Text="Due Amount :"></asp:Label>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:TextBox ID="txtDueAmount" runat="server" ReadOnly="true" CssClass="form-control quantitydecimal"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Attachment</label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Item Receive Doc..." />
                            </div>
                        </div>
                        <div id="DocumentInfo">
                        </div>
                        <div class="row" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-large" value="Save" onclick="SaveReceiveOrder()" />
                                <input type="button" id="btnClear" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="PerformClearActionWithConfirmation()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Item Receive Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:CompanyProjectUserControl ID="companyProjectUserControlSrc" runat="server" />
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
                                <asp:Label ID="lbl1000" runat="server" class="control-label" Text="Receive Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSReceiveNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
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
                                <asp:Label ID="Label10" runat="server" class="control-label" Text="Receive Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchReceiveType" CssClass="form-control" runat="server">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="AdHoc" Value="AdHoc"></asp:ListItem>
                                    <asp:ListItem Text="Receive From Purchase" Value="Purchase"></asp:ListItem>
                                    <asp:ListItem Text="Receive From LC" Value="LC"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label12" runat="server" class="control-label" Text="Cost Center"></asp:Label>
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
                                <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchReceiveOrder(1, 1)" />
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
                    <table id="ReceiveOrderGrid" class="table table-bordered table-condensed table-responsive" width="100%">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 10%;">Receive Number
                                </th>
                                <th style="width: 10%;">Order Type
                                </th>
                                <th style="width: 10%;">Order Date
                                </th>
                                <th style="width: 10%;">Cost Center
                                </th>
                                <th style="width: 10%;">Location
                                </th>
                                <th style="width: 10%;">Supplier
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
                    <div class="row">
                        <div class="col-md-2">
                            <label class="control-label">Serial</label>
                        </div>
                        <div class="col-md-10">
                            <input type="text" id="txtSerial" onkeydown="if (event.keyCode == 13) {CheckAndAddedSerialWiseProduct(); return false;}" class="form-control" />
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
                    <div class="row">
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
                        <input type="button" class="btn btn-primary" style="padding-left: 30px; padding-right: 30px;" value="Ok" onclick="ApplySerialForPurchaseItemWithConfirmation()" />
                        <input type="button" class="btn btn-primary" value="Cancel" onclick="CancelAddSerial()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="popUpImage" style="display: none">
        <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
            <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
        </asp:Panel>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>

    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
</asp:Content>
