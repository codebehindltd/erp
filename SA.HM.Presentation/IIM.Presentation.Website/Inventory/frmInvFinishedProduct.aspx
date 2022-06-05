<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmInvFinishedProduct.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmInvFinishedProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var finisItemEdited = "";
        var DeletedFinishGoods = [];
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Finished Product</li>";
            var breadCrumbs = moduleName + formName;
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val() == "1") {
                FillForm($("#ContentPlaceHolder1_hfFinishProductId").val());
            }

            $("#myTabs").tabs();

            $("#txtItemName").autocomplete({
                source: function (request, response) {
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    var costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();

                    if (costCenterId == "0") {
                        toastr.warning("Please Select a Cost Center.");
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmInvFinishedProduct.aspx/ItemNCategoryAutoSearch',
                        data: "{'itemName':'" + request.term + "','categoryId':'" + categoryId + "', 'costCenterId':'" + costCenterId + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId
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
                    $("#<%=hfItemId.ClientID %>").val(ui.item.value);
                }

            });

            $("#btnAddItem").click(function () {
                var itemId = $("#ContentPlaceHolder1_hfItemId").val();
                var costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();

                var quantity = $.trim($("#ContentPlaceHolder1_txtItemUnit").val());
                var stockById = $("#ContentPlaceHolder1_ddlItemStockBy").val();

                if (costCenterId == "0") {
                    toastr.warning("Please Select a Cost Center.");
                    return false;
                }
                else if (itemId == "0") {
                    toastr.warning("Please select item.");
                    return false;
                }
                else if (stockById == "0") {
                    toastr.warning("Please select item stock by.");
                    return false;
                }
                else if (quantity == "" || quantity == "0") {
                    toastr.warning("Please give item unit.");
                    return false;
                }

                var itemName = "", stockBy = "", finishedProductDetailsId = "0", isEdited = 0, finishProductId = "0", editedItemId = "0";

                itemName = $("#txtItemName").val();
                stockBy = $("#ContentPlaceHolder1_ddlItemStockBy option:selected").text();
                finishProductId = $("#ContentPlaceHolder1_hfFinishProductId").val();

                if (finisItemEdited != "") {
                    var editedItemId = $.trim($(finisItemEdited).find("td:eq(6)").text());

                    if (editedItemId != itemId) {

                        if ($("#FinishedProductGrid tbody > tr").find("td:eq(9):contains('" + (costCenterId + "-" + itemId) + "')").length > 0) {
                            toastr.warning('Same Item Already Added.');
                            return;
                        }
                    }
                }
                else {
                    if ($("#FinishedProductGrid tbody > tr").find("td:eq(9):contains('" + (costCenterId + "-" + itemId) + "')").length > 0) {
                        toastr.warning('Same Item Already Added.');
                        return;
                    }
                }

                if (finishProductId != "0" && finisItemEdited != "") {

                    finishedProductDetailsId = $(finisItemEdited).find("td:eq(4)").text();
                    EditItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, itemId, stockById);
                    toastr.info("Edit db");
                    return;
                }
                else if (finishProductId == "0" && finisItemEdited != "") {

                    finishedProductDetailsId = $(finisItemEdited).find("td:eq(4)").text();
                    EditItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, itemId, stockById);
                    toastr.info("Edit");
                    return;
                }

                AddItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, itemId, stockById);

                $("#ContentPlaceHolder1_hfItemId").val("0");
                $("#txtItemName").val("");
                $("#ContentPlaceHolder1_ddlItemStockBy").val("0");
                $("#ContentPlaceHolder1_txtItemUnit").val("");
                $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", true);

            });

            $("#btnCancelOrder").click(function () {
                ClearFinishGoodsProduct();
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

        function AddItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, itemId, stockById) {

            var isEdited = "0";
            var rowLength = $("#FinishedProductGrid tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:40%;'>" + itemName + "</td>";
            tr += "<td style='width:25%;'>" + stockBy + "</td>";
            tr += "<td style='width:25%;'>" + quantity + "</td>";
            tr += "<td style='width:10%;'> <a onclick=\"javascript:return FIllForEdit(this);\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
            tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'javascript:return DeleteItemDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            //     0	        1	     2	      3
            //Product Name, Stock By, Quantity, Action,
            //FinishedPrductDetailsId   4
            //CostCenterId		        5
            //ProductId		            6
            //StockById   		        7
            //Is Edited		            8
            //duplicateCheck            9

            tr += "<td style='display:none'>" + finishedProductDetailsId + "</td>";
            tr += "<td style='display:none'>" + costCenterId + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + stockById + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "<td style='display:none'>" + (costCenterId + "-" + itemId) + "</td>";

            tr += "</tr>";

            $("#FinishedProductGrid tbody").append(tr);
        }

        function FIllForEdit(editItem) {
            finisItemEdited = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAdd").val("Update");

            var finishedProductDetailsId = $(tr).find("td:eq(4)").text();

            var itemId = $.trim($(tr).find("td:eq(6)").text());
            var stockById = $(tr).find("td:eq(7)").text();

            var itemName = $(tr).find("td:eq(0)").text();
            var quantity = $(tr).find("td:eq(2)").text();

            $("#ContentPlaceHolder1_hfItemId").val(itemId);
            $("#txtItemName").val(itemName);

            $("#ContentPlaceHolder1_ddlItemStockBy").val(stockById);
            $("#ContentPlaceHolder1_txtItemUnit").val(quantity);
        }

        function EditItemForFinishGoods(itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, itemId, stockById) {

            $(finisItemEdited).find("td:eq(0)").text(itemName);
            $(finisItemEdited).find("td:eq(1)").text(stockBy);
            $(finisItemEdited).find("td:eq(2)").text(quantity);

            $(finisItemEdited).find("td:eq(4)").text(finishedProductDetailsId);
            $(finisItemEdited).find("td:eq(5)").text(costCenterId);
            $(finisItemEdited).find("td:eq(6)").text(itemId);
            $(finisItemEdited).find("td:eq(7)").text(stockById);

            if (finishedProductDetailsId != "0")
                $(finisItemEdited).find("td:eq(8)").text("1");

            $(finisItemEdited).find("td:eq(9)").text((costCenterId + "-" + itemId));

            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#txtItemName").val("");
            $("#ContentPlaceHolder1_ddlItemStockBy").val("0");
            $("#ContentPlaceHolder1_txtItemUnit").val("");

            $("#btnAdd").val("Add");
            finisItemEdited = "";
        }

        function DeleteItemDetails(deletedItem) {

            if (!confirm("Do you want to delete?"))
                return;

            var finishProductId = $("#ContentPlaceHolder1_hfFinishProductId").val();

            if (finishProductId != "0") {
                var tr = $(deletedItem).parent().parent();

                DeletedFinishGoods.push({
                    FinishedProductDetailsId: $(tr).find("td:eq(4)").text(),
                    FinishProductId: finishProductId
                });
            }

            $(deletedItem).parent().parent().remove();
        }

        function ValidationBeforeSave() {

            var rowCount = $('#FinishedProductGrid tbody tr').length;
            if (rowCount == 0) {
                toastr.warning('Add atleast one Product.');
                return false;
            }

            //itemName, stockBy, quantity, finishedProductDetailsId, costCenterId, itemId, stockById

            var costCenterId = "0", itemId = "0", stockById = "0";
            var quantity = "0", finishedProductDetailsId = "0";
            var isEdit = "0", finishProductId = "0", remarks = "";

            costCenterId = $("#ContentPlaceHolder1_ddlCostCenter").val();
            finishProductId = $("#ContentPlaceHolder1_hfFinishProductId").val();
            remarks = $.trim($("#ContentPlaceHolder1_txtRemarks").val());

            var FinishedProduct = {
                FinishProductId: finishProductId,
                CostCenterId: costCenterId,
                Remarks: remarks
            };

            var AddedFinishGoods = [], EditedFinishGoods = [];

            $("#FinishedProductGrid tbody tr").each(function (index, item) {

                finishedProductDetailsId = $.trim($(item).find("td:eq(4)").text());
                isEdit = $.trim($(item).find("td:eq(8)").text());

                itemId = $.trim($(item).find("td:eq(6)").text());
                stockById = $(item).find("td:eq(7)").text();

                quantity = $(item).find("td:eq(2)").text();

                if (finishedProductDetailsId == "0") {

                    AddedFinishGoods.push({
                        FinishedProductDetailsId: finishedProductDetailsId,
                        FinishProductId: finishProductId,
                        ProductId: itemId,
                        StockById: stockById,
                        Quantity: quantity
                    });
                }
                else if (finishedProductDetailsId != "0" && isEdit != "0") {
                    EditedFinishGoods.push({
                        FinishedProductDetailsId: finishedProductDetailsId,
                        FinishProductId: finishProductId,
                        ProductId: itemId,
                        StockById: stockById,
                        Quantity: quantity
                    });
                }
            });

            PageMethods.SaveFinishGoods(FinishedProduct, AddedFinishGoods, EditedFinishGoods, DeletedFinishGoods, OnSaveFinishGoodsSucceeded, OnSaveFinishGoodsFailed);

            return false;
        }
        function OnSaveFinishGoodsSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            return false;
        }
        function OnSaveFinishGoodsFailed(error) { toastr.error(error.get_message()); }

        function FinishProductDetails(finishProductId) {
            PageMethods.GetFinishProductDetails(finishProductId, OnFinishProductLoadSucceeded, OnSaveFinishGoodsFailed);
            return false;
        }
        function OnFinishProductLoadSucceeded(result) {

            $("#DetailsFinishProductGrid tbody").html("");
            var totalRow = result.length, row = 0;

            var tr = "";

            for (row = 0; row < totalRow; row++) {

                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:40%;'>" + result[row].ProductName + "</td>";
                tr += "<td style='width:25%;'>" + result[row].StockBy + "</td>";
                tr += "<td style='width:25%;'>" + result[row].Quantity + "</td>";

                tr += "</tr>";

                $("#DetailsFinishProductGrid tbody").append(tr);
                tr = "";
            }

            $("#DetailsFinishProductGridContainer").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 800,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Finish Item Details",
                show: 'slide'
            });
        }

        function FillForm(finishProductId) {
             if (!confirm("Do you Want To Update?")) {
                return false;
            }
            PageMethods.FillForm(finishProductId, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {

            if (result != null) {

                PerformClearAction();

                $("#ContentPlaceHolder1_ddlCostCenter").val(result.FinishedProduct.CostCenterId);
                $("#ContentPlaceHolder1_hfFinishProductId").val(result.FinishedProduct.FinishProductId);
                $("#ContentPlaceHolder1_txtRemarks").val(result.FinishedProduct.Remarks);

                $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", true);

                var rowLength = result.FinisProductDetails.length;
                var row = 0;

                for (row = 0; row < rowLength; row++) {

                    AddItemForFinishGoods(result.FinisProductDetails[row].ProductName, result.FinisProductDetails[row].StockBy, result.FinisProductDetails[row].Quantity,
                                    result.FinisProductDetails[row].FinishedProductDetailsId, result.FinishedProduct.CostCenterId, result.FinisProductDetails[row].ProductId,
                                    result.FinisProductDetails[row].StockById);
                }

                $("#ContentPlaceHolder1_btnSave").val("Update");
                //$("#myTabs").tabs('select', 0);
                $("#myTabs").tabs({ active: 0 });
            }
        }
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
        }

        function ClearFinishGoodsProduct() {

            $("#btnAddItem").val("Add");

            $("#ContentPlaceHolder1_ddlCategory").val("0");
            $("#txtItemName").val("");
            $("#ContentPlaceHolder1_ddlItemStockBy").val("");
            $("#ContentPlaceHolder1_txtItemUnit").val("");

            $("#ContentPlaceHolder1_hfItemId").val("0");

            if ($("#FinishedProductGrid tbody tr").length == 0 && $("#ContentPlaceHolder1_hfFinishProductId").val() == "0") {
                $("#ContentPlaceHolder1_ddlCostCenter").val("0");
                $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", false);
            }

            return false;
        }
        function PerformClearActionWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }
        function ConfirmatonForDelete() {
            if (!confirm("Do you Want To Delete?")) {
                return false;
            }
        }

        function PerformClearAction() {

            $("#FinishedProductGrid tbody").html("");
            $("#ContentPlaceHolder1_btnSave").val("Save");

            $("#ContentPlaceHolder1_ddlCostCenter").val("0");
            $("#ContentPlaceHolder1_ddlCategory").val("0");
            $("#txtItemName").val("");
            $("#ContentPlaceHolder1_ddlItemStockBy").val("");
            $("#ContentPlaceHolder1_txtItemUnit").val("");

            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_hfFinishProductId").val("0");
            $("#ContentPlaceHolder1_txtRemarks").val("");

            $("#ContentPlaceHolder1_hfIsEditedFromApprovedForm").val("0");

            $("#ContentPlaceHolder1_ddlCostCenter").attr("disabled", false);

            return false;
        }

    </script>

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />

    <div id="DetailsFinishProductGridContainer" style="display: none;">
        <table id="DetailsFinishProductGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 40%;">
                        Item Name
                    </th>
                    <th style="width: 25%;">
                        Stock By
                    </th>
                    <th style="width: 25%;">
                        Quantity
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfFinishProductId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsEditedFromApprovedForm" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Finished Item</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Item</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCostCenter" runat="server" class="control-label required-field"
                                Text="Cost Center"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCostCenter" CssClass="form-control" runat="server" TabIndex="20">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="EntryPanel" class="panel panel-default">
                        <div class="panel-heading">
                            Item Information</div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" TabIndex="20">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblItemName" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtItemName" CssClass="form-control" TabIndex="21" runat="server"
                                            ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblItemStockBy" runat="server" class="control-label required-field"
                                            Text="Unit Head"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlItemStockBy" runat="server" CssClass="form-control" TabIndex="22">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblItemUnit" runat="server" class="control-label required-field" Text="Unit"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtItemUnit" TabIndex="23" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row" style="padding: 5px 0 5px 0;">
                                    <div class="col-md-12">
                                        <button type="button" id="btnAddItem" tabindex="24" class="TransactionalButton btn btn-primary btn-sm">
                                            Add</button>
                                        <button type="button" id="btnCancelOrder" tabindex="24" class="TransactionalButton btn btn-primary btn-sm">
                                            Cancel</button>
                                    </div>
                                </div>
                                <div class="form-group" style="padding: 0px;">
                                    <div id="FinishedProductGridContainer">
                                        <table id="FinishedProductGrid" class="table table-bordered table-condensed table-responsive"
                                            style="width: 100%;">
                                            <thead>
                                                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                                    <th style="width: 40%;">
                                                        Item Name
                                                    </th>
                                                    <th style="width: 25%;">
                                                        Unit Head
                                                    </th>
                                                    <th style="width: 25%;">
                                                        Quantity
                                                    </th>
                                                    <th style="width: 10%;">
                                                        Action
                                                    </th>
                                                    <th style="display: none">
                                                        FinishedPrductDetailsId
                                                    </th>
                                                    <th style="display: none">
                                                        CostCenterId
                                                    </th>
                                                    <th style="display: none">
                                                        ProductId
                                                    </th>
                                                    <th style="display: none">
                                                        StockById
                                                    </th>
                                                    <th style="display: none">
                                                        Is Edited
                                                    </th>
                                                    <th style="display: none">
                                                        duplicateCheck
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
                            <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="24" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript:return ValidationBeforeSave();" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="25" OnClientClick="javascript:return PerformClearActionWithConfirmation();"
                                CssClass="TransactionalButton btn btn-primary btn-sm" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Finished Product Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchCostCenter" runat="server" CssClass="form-control"
                                    TabIndex="1">
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
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" TabIndex="6" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvFinishedProductInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" TabIndex="9"
                        OnRowCommand="gvFinishedProductInfo_RowCommand" OnRowDataBound="gvFinishedProductInfo_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("FinishProductId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Date" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("OrderDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" ItemStyle-Width="45%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ApprovedStatus" HeaderText="Status" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgDetailsRequisition" runat="server" CausesValidation="False"
                                        CommandName="CmdDetails" CommandArgument='<%# bind("OrderDate") %>' OnClientClick='<%#String.Format("return FinishProductDetails({0})", Eval("FinishProductId")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Product Details" />
                                    &nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("FinishProductId") %>' OnClientClick='<%#String.Format("return FillForm({0})", Eval("FinishProductId")) %>'
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" OnClientClick="javscript:return ConfirmatonForDelete()" runat="server" CausesValidation="False" CommandName="CmdDelete" 
                                        CommandArgument='<%# bind("FinishProductId") %>' ImageUrl="~/Images/delete.png"
                                        Text="" AlternateText="Delete" ToolTip="Delete Order" />
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
    <div class="divClear">
    </div>
</asp:Content>
