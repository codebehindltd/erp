<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="FixedAssetTransfer.aspx.cs" Inherits="HotelManagement.Presentation.Website.FixedAsset.FixedAssetTransfer" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControlVertical.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>

        var ItemSelected = null;
        var AddedSerialCount = 0;
        var TransferItemAdded = new Array();
        var TransferItemDeleted = new Array();
        var NewAddedSerial = new Array();
        var AddedSerialzableProduct = new Array();
        var DeletedSerialzableProduct = new Array();

        $(document).ready(function () {

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_companyProjectUserControl_lblGLCompany").text("From Company");
            $("#ContentPlaceHolder1_companyProjectUserControl_lblGLProject").text("From Project");

            $("#ContentPlaceHolder1_companyProjectUserControlTwo_lblGLCompany").text("To Company");
            $("#ContentPlaceHolder1_companyProjectUserControlTwo_lblGLProject").text("To Project");

            IsSerialAutoField = $('#ContentPlaceHolder1_hfIsItemSerialFillWithAutoSearch').val() == '1' ? true : false;
            if (IsSerialAutoField) {
                $('#labelAndtxtSerialAddButtonAndClear').hide();
                $('#labelAndtxtSerial').hide();
                $('#labelAndtxtSerialAutoComplete').show();
            } else {
                $('#labelAndtxtSerialAddButtonAndClear').show();
                $('#labelAndtxtSerial').show();
                $('#labelAndtxtSerialAutoComplete').hide();

            }

            $("#ContentPlaceHolder1_ddlCostCenterFrom").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlCostCenterTo").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlOutFor").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });
            $("#ContentPlaceHolder1_ddlDepartment").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });
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
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_ddlCostCenterFrom").change(function () {
                if ($(this).val() != "0") {
                    LoadLocationFrom($(this).val());
                }

            });
            $("#ContentPlaceHolder1_ddlTransferFor").change(function () {
                if ($(this).val() == "employee") {
                    $('#DepartmentLabelDiv').hide()
                    $('#DepartmentControlDiv').hide()
                    $('#EmployeeLabelDiv').show()
                    $('#EmployeeControlDiv').show()
                }
                else if ($(this).val() == "department") {
                    $('#DepartmentLabelDiv').show()
                    $('#DepartmentControlDiv').show()
                    $('#EmployeeLabelDiv').hide()
                    $('#EmployeeControlDiv').hide()
                }
            });
            $("#ContentPlaceHolder1_ddlCostCenterFrom").val('0').trigger('change');
            $("#ContentPlaceHolder1_ddlCostCenterTo").change(function () {
                if ($(this).val() != "0") {
                    LoadLocationTo($(this).val());
                }
            });
            $("#ContentPlaceHolder1_ddlCostCenterTo").val('0').trigger('change');
            $("#ContentPlaceHolder1_txtItem").autocomplete({

                source: function (request, response) {
                    var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                    var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
                    var costCenterId = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    var locationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();

                    if (companyId == "0") {
                        toastr.warning("Please Select From Company.");
                        $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                        return false;
                    }
                    else if (projectId == "0") {
                        toastr.warning("Please Select From Project.");
                        $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                        return false;
                    }
                    else if (costCenterId == "0") {
                        toastr.warning("Please Select Store");
                        return false;
                    }
                    else if (locationId == "0") {
                        toastr.warning("Please Select Store Location");
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../FixedAsset/FixedAssetTransfer.aspx/ItemSearch',
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
                    $("#ContentPlaceHolder1_txtCurrentStock").val(ui.item.StockQuantity).prop("disabled", true);
                    $("#ContentPlaceHolder1_txtCurrentStockBy").val(ui.item.UnitHead).prop("disabled", true);
                }
            })
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
                        url: '../FixedAsset/FixedAssetTransfer.aspx/SerialSearch',
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

            SearchOutOrder(1, 1);
        });
        function LoadLocationFrom(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationFromSucceeded, OnLoadLocationFailed);
        }
        function LoadLocationTo(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationToSucceeded, OnLoadLocationFailed);
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
                }
                else {
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }
            if (result.length == 1 && $("#ContentPlaceHolder1_hfLocationFromId").val() == "0")
                control.val($("#ContentPlaceHolder1_ddlLocationFrom option:first").val());
            else
                control.val($("#ContentPlaceHolder1_hfLocationFromId").val());
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
            if (result.length == 1 && $("#ContentPlaceHolder1_hfLocationFromId").val() == "0")
                control.val($("#ContentPlaceHolder1_ddlLocationTo option:first").val());
            else
                control.val($("#ContentPlaceHolder1_hfLocationToId").val());
            return false;
        }
        function OnLoadLocationFailed() {
        }

        function AddItemForTransfer() {
            GLCompanyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            GLProjectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            if (GLCompanyId == null)
                GLCompanyId = "0";
            if (GLProjectId == null)
                GLProjectId = "0";

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

            var quantity = 0, tr = "";
            quantity = $("#ContentPlaceHolder1_txtTransferQuantity").val();

            if (ItemSelected == null) {
                toastr.warning("Please Select Item.");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtTransferQuantity").val()) == "" || $.trim($("#ContentPlaceHolder1_txtTransferQuantity").val()) == "0") {
                toastr.warning("Please Give Transfer Quantity.");
                return false;
            }
            else if (ItemSelected.StockQuantity < 0) {
                toastr.warning("Transfer Quantity Does Not Greater Than Stock Quantity.");
                return false;
            }
            else if (parseFloat(quantity) > ItemSelected.StockQuantity) {
                toastr.warning("Transfer Quantity Does Not Greater Than Stock Quantity.");
                return false;
            }

            var itm = _.findWhere(TransferItemAdded, { ItemId: ItemSelected.ItemId });

            if (itm != null) {
                toastr.warning("Same Item Already Added. Duplicate Item Is Not Accepted.");
                return false;
            }

            tr += "<tr>";

            tr += "<td style='width:55%;'>" + ItemSelected.ItemName + "</td>";
            tr += "<td style='width:10%;'>" + ItemSelected.StockQuantity + "</td>";
            tr += "<td style='width:15%;'>" +
                "<input type='text' value='" + quantity + "' id='pp" + ItemSelected.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                "</td>";

            tr += "<td style='width:10%;'>" + ItemSelected.UnitHead + "</td>";

            tr += "<td style='width:10%;'>";
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

            tr += "</tr>";

            $("#ItemForTransferTbl tbody").prepend(tr);
            tr = "";

            TransferItemAdded.push({
                ItemId: parseInt(ItemSelected.ItemId, 10),
                ItemName: ItemSelected.ItemName,
                ProductType: ItemSelected.ProductType,
                StockById: parseInt(ItemSelected.StockBy, 10),
                Quantity: parseFloat(quantity),
                StockQuantity: ItemSelected.StockQuantity,
                DetailId: 0
            });

            CommonHelper.ApplyDecimalValidation();
            ClearAfterAdhoqPurchaseItemAdded();
            $("#ContentPlaceHolder1_txtItem").focus();
        }
        function CalculateTotalForAdhoq(control) {

            var tr = $(control).parent().parent();

            var stockQuantity = $.trim($(tr).find("td:eq(1)").text());
            var quantity = $.trim($(tr).find("td:eq(2)").find("input").val());
            var oldQuantity = $(tr).find("td:eq(8)").text();

            if (quantity == "" || quantity == "0") {
                toastr.info("Quantity Cannot Be Zero Or Empty.");
                $(tr).find("td:eq(2)").find("input").val(oldQuantity);
                return false;
            }
            else if (parseFloat(quantity) > parseFloat(stockQuantity)) {
                toastr.info("Transfer Quantity Cannot Greater Than Stock Quantity.");
                $(tr).find("td:eq(2)").find("input").val(oldQuantity);
                return false;
            }

            var itemId = parseInt($.trim($(tr).find("td:eq(5)").text()), 10);

            var item = _.findWhere(TransferItemAdded, { ItemId: itemId });
            var index = _.indexOf(TransferItemAdded, item);

            TransferItemAdded[index].Quantity = parseFloat(quantity);
            $(tr).find("td:eq(8)").text(quantity);
        }
        function DeleteAdhoqItem(control) {

            if (!confirm("Do you want to delete item?")) { return false; }

            var tr = $(control).parent().parent();

            var itemId = parseInt($.trim($(tr).find("td:eq(5)").text()), 10);
            var outDetailsId = parseInt($.trim($(tr).find("td:eq(9)").text()), 10);

            var item = _.findWhere(TransferItemAdded, { ItemId: itemId });
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

        function AddSerialForAdHocItem(control) {
            var tr = $(control).parent().parent();

            var itemName = $(tr).find("td:eq(0)").text();
            var itemId = $(tr).find("td:eq(5)").text();
            var quantity = $(tr).find("td:eq(2)").find("input").val();

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
        function ClearAfterAdhoqPurchaseItemAdded() {
            $("#ContentPlaceHolder1_ddlCategory").val("0");
            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_txtCurrentStock").val("");
            $("#ContentPlaceHolder1_txtCurrentStockBy").val("");
            $("#ContentPlaceHolder1_txtTransferQuantity").val("");
            ItemSelected = null;
        }
        function ShowReport(IssueType, OutId, ApprovedStatus, CreatedBy) {
            var iframeid = 'printDoc';
            var url = "/Inventory/Reports/frmReportProductOut.aspx?poOutId=" + OutId;
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

            var outOrderId = "0", issueType = "FixedAsset", isEdited = "0", fromCostCenterId = "", fromLocationId = 0, toCostCenterId = 0,
                toLocationId = "", transferFor = "", GLCompanyId = "0", GLProjectId = "0", toGLCompanyId = "0", toGLProjectId = "0";

            transferFor = $("#ContentPlaceHolder1_ddlTransferFor").val();
            outOrderId = $("#ContentPlaceHolder1_hfOutId").val();
            if (transferFor == 'employee')
                outFor = $("#ContentPlaceHolder1_ddlOutFor").val();
            else if (transferFor == 'department')
                outFor = $("#ContentPlaceHolder1_ddlDepartment").val();
            fromCostCenterId = $("#ContentPlaceHolder1_ddlCostCenterFrom").val();
            fromLocationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();
            toCostCenterId = $("#ContentPlaceHolder1_ddlCostCenterTo").val();
            toLocationId = $("#ContentPlaceHolder1_ddlLocationTo").val();

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

            if (fromCostCenterId == "" || fromCostCenterId == "0") {
                toastr.warning("Please Select Store From.");
                return false;
            }
            else if (fromLocationId == "" || fromLocationId == 0) {
                toastr.warning("Please Select From Location.");
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

            TransferItemNewlyAdded = TransferItemAdded;

            var row = 0, rowCount = TransferItemNewlyAdded.length;

            for (row = 0; row < rowCount; row++) {
                if (TransferItemNewlyAdded[row].Quantity > TransferItemNewlyAdded[row].StockQuantity) {
                    toastr.warning("Transfer Quantity Cannot Greater Than Stock Quantity Of Product " + TransferItemNewlyAdded[row].ItemName);
                    break;
                }
            }

            if (row != rowCount) {
                return false;
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

            var ProductOut = {
                OutId: outOrderId,
                ProductOutFor: issueType,
                RequisitionOrSalesId: 0,
                TransferFor: transferFor,
                OutFor: outFor,
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
                //LoadNotReceivedRequisitionOrder();

                //if (queryOutId != "") {
                //    window.location = "/Inventory/ItemTransferInformation.aspx";
                //}

                PerformClearAction();
                $('#CreateNewDialog').dialog('close')
                SearchOutOrder(1, 1);
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
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchOutOrder(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function SearchOutOrder(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#OutOrderGrid tbody tr").length;
            var issueNumber = "0", status = "", issueType = "", fromDate = null, toDate = null;

            issueNumber = $("#ContentPlaceHolder1_txtIssueNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            issueType = "FixedAsset";
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');
            else
                fromDate = CommonHelper.DateFormatToMMDDYYYY('01/01/1753', '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            $("#GridPagingContainer ul").html("");
            $("#OutOrderGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchOutOrder(issueType, fromDate, toDate, issueNumber, status, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

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
                if (gridObject.IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditItemOutWithConfirmation('" + gridObject.ProductOutFor + "'," + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return OutOrderDelete('" + gridObject.ProductOutFor + "'," + gridObject.OutId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return OutOrderApprovalWithConfirmation('" + gridObject.ProductOutFor + "','" + 'Checked' + "'," + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Checked'  title='Checked' border='0' />";
                }

                if (gridObject.IsCanApproved) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return OutOrderApprovalWithConfirmation('" + gridObject.ProductOutFor + "','" + 'Approved' + "', " + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Approved'  title='Approved' border='0' />";
                }

                //if (gridObject.Status == 'Approved') {
                // tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.ProductOutFor + "'," + gridObject.OutId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Invoice' title='Item Transfer Info' border='0' />";
                //}

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
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");
        }
        function CreateNew() {
            PerformClearAction();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '75%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Fixet Asset Transfer",
                show: 'slide'
            });


            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });


            $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });

            $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateNewDialog")
            });

            return false;
        }
        function PerformClearActionWithComfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }
        function PerformClearAction() {

            $("#ContentPlaceHolder1_hfOutId").val("0");

            $("#ItemForTransferTbl tbody").html("");

            $("#ContentPlaceHolder1_ddlTransferFor").val("employee").trigger('change');
            $("#ContentPlaceHolder1_ddlOutFor").val("0").prop('disabled', false).trigger('change');
            $("#ContentPlaceHolder1_ddlCostCenterFrom").val("0").prop('disabled', false).trigger('change');
            $("#ContentPlaceHolder1_ddlLocationFrom").val("0").prop('disabled', false);
            $("#ContentPlaceHolder1_ddlCostCenterTo").val("0").prop('disabled', false).trigger('change');
            $("#ContentPlaceHolder1_ddlLocationTo").val("0").prop('disabled', false);

            $("#ContentPlaceHolder1_ddlCategory").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_txtCurrentStock").val("").prop("disabled", false);
            $("#ContentPlaceHolder1_txtCurrentStockBy").val("").prop("disabled", false);
            $("#ContentPlaceHolder1_txtTransferQuantity").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").prop('disabled', false);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").prop('disabled', false);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val("0").trigger("change");
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val("0").trigger("change");

            $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").prop('disabled', false);
            $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").prop('disabled', false);
            $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").val("0").trigger("change");
            $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").val("0").trigger("change");

            AddedSerialCount = 0;
            NotTriggerChange = 0;
            ItemSelected = null;
            TransferItemAdded = new Array();
            TransferItemDeleted = new Array();
            NewAddedSerial = new Array();
            AddedSerialzableProduct = new Array();
            DeletedSerialzableProduct = new Array();

            $("#btnSave").val("Save");
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

            $("#RequisitionWiseItemContainer").hide();
            $("#RequisitionNumberContainer").hide();
            $("#StockTransferContainer").show();
            $("#remarksDiv").show();
            $("#btnDiv").show();

            $("#ItemForTransferTbl tbody").html("");
            $("#RequisitionWiseItemTbl tbody").html("");
            $("#SalesOrderItemTbl tbody").html("");

            StockTransferOrderEdit(result);
            SearchOutOrder(1, 1);

        }
        function OnEditPurchaseOrderFailed() {
        }
        function StockTransferOrderEdit(result) {

            LoadForEditOutOrder(result);

            var tr = "";

            $.each(result.ProductOutDetails, function (count, obj) {

                tr += "<tr>";

                tr += "<td style='width:55%;'>" + obj.ItemName + "</td>";
                tr += "<td style='width:10%;'>" + obj.StockQuantity + "</td>";
                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + obj.Quantity + "' id='pp" + obj.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                    "</td>";

                tr += "<td style='width:10%;'>" + obj.StockBy + "</td>";

                tr += "<td style='width:10%;'>";
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

                tr += "</tr>";

                $("#ItemForTransferTbl tbody").append(tr);
                tr = "";
            });

            TransferItemAdded = result.ProductOutDetails;

            CommonHelper.ApplyDecimalValidation();
            //ClearAfterAdhoqPurchaseItemAdded();

            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '75%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Fixet Asset Transfer",
                show: 'slide'
            });
            return false;
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


            if (result.ProductOut.TransferFor == 'department') {
                $("#ContentPlaceHolder1_ddlDepartment").val(result.ProductOut.OutFor).trigger('change');
                $("#ContentPlaceHolder1_ddlTransferFor").val(result.ProductOut.TransferFor).trigger('change');
            }
            else {
                $("#ContentPlaceHolder1_ddlOutFor").val(result.ProductOut.OutFor).trigger('change');
                $("#ContentPlaceHolder1_ddlTransferFor").val('employee').trigger('change');
            }


            if (result.ProductOut.GLCompanyId > 0) {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(result.ProductOut.GLCompanyId).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").prop('disabled', true);
            }
            if (result.ProductOut.GLProjectId > 0) {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(result.ProductOut.GLProjectId).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").prop('disabled', true);
            }

            if (result.ProductOut.ToGLCompanyId > 0) {
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").val(result.ProductOut.ToGLCompanyId).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLCompany").prop('disabled', true);
            }
            if (result.ProductOut.ToGLProjectId > 0) {
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").val(result.ProductOut.ToGLProjectId).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControlTwo_ddlGLProject").prop('disabled', true);
            }

            //if (result.ProductOut.TransferFor == null || result.ProductOut.TransferFor != '')
            //    $("#ContentPlaceHolder1_ddlTransferFor").val('employee').trigger('change');
            //else
            //    $("#ContentPlaceHolder1_ddlTransferFor").val(result.ProductOut.TransferFor).trigger('change');
            //if (result.ProductOut.TransferFor == 'employee' || result.ProductOut.TransferFor == null || result.ProductOut.TransferFor != '')
            //    $("#ContentPlaceHolder1_ddlOutFor").val(result.ProductOut.OutFor).trigger('change');
            //else if (result.ProductOut.TransferFor == 'department')
            //    $("#ContentPlaceHolder1_ddlDepartment").val(result.ProductOut.OutFor).trigger('change');

            $("#ContentPlaceHolder1_hfLocationFromId").val(result.ProductOut.FromLocationId);
            $("#ContentPlaceHolder1_hfLocationToId").val(result.ProductOut.ToLocationId);
            $("#ContentPlaceHolder1_ddlCostCenterFrom").val(result.ProductOut.FromCostCenterId + '').trigger('change');
            $("#ContentPlaceHolder1_ddlCostCenterTo").val(result.ProductOut.ToCostCenterId + '').trigger('change');

            LoadLocationFrom(result.ProductOut.FromCostCenterId);
            LoadLocationTo(result.ProductOut.ToCostCenterId);

            if (result.ProductOut.Remarks != null)
                $("#ContentPlaceHolder1_txtRemarks").val(result.ProductOut.Remarks);

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

                //LoadNotReceivedRequisitionOrder();
                SearchOutOrder($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }

    </script>
    <asp:HiddenField ID="hfOutId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemIdForSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationFromId" runat="server" Value="0" />
    <asp:HiddenField ID="hfLocationToId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsItemSerialFillWithAutoSearch" runat="server" Value="0" />
    <div class="panel panel-default">
        <div class="panel-heading">
            Fixed Asset Transfer
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lbl1000" runat="server" class="control-label" Text="Issue Number"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtIssueNumber" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Status"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
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
                            <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return SearchOutOrder(1, 1);" />
                            <asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return ClearSearch();" />
                            <asp:Button ID="btnCreateNew" runat="server" Text="New Fixed Asset Transfer" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return CreateNew();" />
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
                            <th style="width: 10%;">Issue Number
                            </th>
                            <th style="width: 10%;">Issue Type
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
    <div id="CreateNewDialog" style="overflow: unset; display: none">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblTransferFor" runat="server" class="control-label required-field" Text="Transfer For"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlTransferFor" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Employee" Value="employee"></asp:ListItem>
                        <asp:ListItem Text="Department" Value="department"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2" id="EmployeeLabelDiv">
                    <asp:Label ID="lblOutFor" runat="server" class="control-label required-field" Text="Employee Name"></asp:Label>
                </div>
                <div class="col-md-4" id="EmployeeControlDiv">
                    <asp:DropDownList ID="ddlOutFor" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2" id="DepartmentLabelDiv" style="display: none;">
                    <asp:Label ID="lblDepartment" runat="server" class="control-label required-field" Text="Department Name"></asp:Label>
                </div>
                <div class="col-md-4" id="DepartmentControlDiv" style="display: none;">
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="Label3" runat="server" class="control-label required-field"
                        Text="From Store"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlCostCenterFrom" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div id="dvCostCenterFrom">
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="To Store"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCostCenterTo" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="Label13" runat="server" class="control-label required-field" Text="From Location"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlLocationFrom" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div id="dvCostCenterTo">
                    <div class="col-md-2">
                        <asp:Label ID="Label14" runat="server" class="control-label required-field" Text="To Location"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLocationTo" runat="server" CssClass="form-control">
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
                                <th style="width: 55%;">Item Name</th>
                                <th style="width: 10%;">Stock Quantity</th>
                                <th style="width: 15%;">Unit</th>
                                <th style="width: 10%;">Unit Head</th>
                                <th style="width: 10%;">Action</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
            </div>
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
                    <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-sm" value="Save" onclick="SaveItemOutOrder()" />
                    <input type="button" id="btnClear" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="PerformClearActionWithComfirmation()" />
                </div>
            </div>
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
                            <input type="text" id="txtSerialAutoComplete" class="form-control" placeholder="Minimum 3 characters" />
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
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="1000" height="800" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>
</asp:Content>
