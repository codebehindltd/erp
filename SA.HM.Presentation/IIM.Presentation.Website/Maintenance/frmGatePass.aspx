<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGatePass.aspx.cs" Inherits="HotelManagement.Presentation.Website.Maintenance.frmGatePass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        //Bread Crumbs Information-------------

        var editedItem = "";
        var deleteDbItem = [], editDbItem = [], newlyAddedItem = [];
        var queryRqId = "";

        $(document).ready(function () {

            queryRqId = CommonHelper.GetParameterByName("GpId");
            if (queryRqId != "") {
                FillForm(queryRqId);
            }

            CommonHelper.ApplyDecimalValidation();
            $("#myTabs").tabs();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtItemName").autocomplete({

                source: function (request, response) {

                    var costCenterId = $("#ContentPlaceHolder1_ddlCostCentre").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: './frmGatePass.aspx/ItemSearch',
                        data: "{'searchTerm':'" + request.term + "', 'costCenterId':'" + costCenterId + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    CategoryId: m.CategoryId,
                                    StockById: m.StockBy
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
                    PageMethods.LoadRelatedStockBy(ui.item.StockById, OnLoadStockBySucceeded, OnLoadStockByFailed);
                }
            });

        });

        $(document).ready(function () {
            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'
            //DivShowHideFunction();
            $('#' + txtStartDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });

            $('.select2').select2();

            <%--$("#<%=ddlDateType.ClientID %>").change(function () {
                DivShowHideFunction();
            });--%>

            var txtReturnDate = '<%=txtReturnDate.ClientID%>';
            var txtGatePassDate = '<%=txtGatePassDate.ClientID%>';
            $('#' + txtReturnDate + ', #' + txtGatePassDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            //done
            $("#btnAddItem").click(function () {
                var costCentreId = $("#ContentPlaceHolder1_ddlCostCentre").val();
                var itemId = $("#ContentPlaceHolder1_hfItemId").val();                
                var stockById = $("#ContentPlaceHolder1_ddlStockBy").val();
                var quantity = $("#ContentPlaceHolder1_txtQuantity").val();
                var description = $("#ContentPlaceHolder1_txtDescription").val().trim();
                var returnTypeId = $("#ContentPlaceHolder1_ddlReturnType").val();                
                var returnDate = $("#ContentPlaceHolder1_txtReturnDate").val();


                if (itemId == "0") {
                    toastr.warning("Please select an item.");
                    return;
                }
                else if (costCentreId == "") {
                    toastr.warning("Please select a cost centre.");
                    return;
                }
                else if (stockById == "") {
                    toastr.warning("Please select a stock by.");
                    return;
                }
                else if ($.trim(quantity) == "" || parseFloat($.trim(quantity)) == 0) {
                    toastr.warning("Please give quantity.");
                    return;
                }
                else if (CommonHelper.IsDecimal($.trim(quantity)) == false) {
                    toastr.warning("Please give valid quantity.");
                    return;
                }
                else if (description == "") {
                    toastr.warning("Please give description.");
                    return;
                }
                else if (returnDate == "") {
                    toastr.warning("Please give return date.");
                    return;
                }

                var itemName = $("#ContentPlaceHolder1_txtItemName").val();
                var costCentreName = $("#ContentPlaceHolder1_ddlCostCentre option:selected").text();
                var stockBy = $("#ContentPlaceHolder1_ddlStockBy option:selected").text();
                var returnType = $("#ContentPlaceHolder1_ddlReturnType option:selected").text();

                var duplicateItemId = 0;
                var gatePassId = "0", gatePassDetailsId = "0";

                gatePassId = $("#ContentPlaceHolder1_hfGatePassId").val();

                if (editedItem != "") {
                    var editedItemId = $.trim($(editedItem).find("td:eq(8)").text());

                    if (editedItemId != itemId) {
                        duplicateItemId = $("#ItemGrid tbody tr").find("td:eq(8):contains(" + itemId + ")").length;

                        if (duplicateItemId > 0) {
                            toastr.warning("Same Item Already Added.");
                            return false;
                        }
                    }
                }
                else {
                    duplicateItemId = $("#ItemGrid tbody tr").find("td:eq(8):contains(" + itemId + ")").length;

                    if (duplicateItemId > 0) {
                        toastr.warning("Same Item Already Added");
                        return false;
                    }
                }

                if (gatePassId != "0" && editedItem != "") {
                    gatePassDetailsId = $(editedItem).find("td:eq(7)").text();
                    EditItem(gatePassId, gatePassDetailsId, itemId, costCentreId, stockById, returnTypeId,
                                quantity, itemName, costCentreName, stockBy, returnType, description, returnDate);
                    return;
                }
                else if (gatePassId == "0" && editedItem != "") {

                    gatePassDetailsId = $(editedItem).find("td:eq(7)").text();
                    EditItem(gatePassId, gatePassDetailsId, itemId, costCentreId, stockById, returnTypeId,
                                quantity, itemName, costCentreName, stockBy, returnType, description, returnDate);
                    return;
                }

                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);

                AddGatePassItem(gatePassId, gatePassDetailsId, itemId, costCentreId, stockById, returnTypeId,
                    quantity, itemName, costCentreName, stockBy, returnType, description, returnDate);

                $("#ContentPlaceHolder1_hfItemId").val("0");
                $("#ContentPlaceHolder1_txtQuantity").val("");
                $("#ContentPlaceHolder1_txtItemName").val("");
                $("#ContentPlaceHolder1_ddlStockBy").val("0");
                $("#ContentPlaceHolder1_txtDescription").val("");
                $("#ContentPlaceHolder1_txtReturnDate").val("");
                $("#ContentPlaceHolder1_txtItemName").focus();
            });

            $("#btnClearGatePass").click(function () {
                PerformClearItems();
                return false;
            });

        });

        <%--function DivShowHideFunction() {
            if ($("#<%=ddlDateType.ClientID %>").val() == "Pending") {
                $('#StatusLabelDiv').hide("slow");
                $('#StatusDziv').hide("slow");
                $('#DateDiv').hide("slow");
            }
            else {
                $('#StatusLabelDiv').show("slow");
                $('#StatusDiv').show("slow");
                $('#DateDiv').show("slow");
            }
        }--%>
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

        //done
        function AddGatePassItem(gatePassId, gatePassDetailsId, itemId, costCentreId, stockById, returnTypeId,
            quantity, itemName, costCentreName, stockBy, returnType, description, returnDate) {
            var rtnDate;
            if (gatePassId > 0)
                rtnDate = CommonHelper.DateFromDateTimeToDisplay(returnDate, innBoarDateFormat);
            else
                rtnDate = returnDate;// CommonHelper.DateFromDateTimeToDisplay(returnDate, innBoarDateFormat);// CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(, innBoarDateFormat);
           
            var isEdited = "0";
            var rowLength = $("#ItemGrid tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:25%;'>" + costCentreName + "</td>";
            tr += "<td style='width:25%;'>" + itemName + "</td>";
            tr += "<td style='width:10%;'>" + quantity + "</td>";
            tr += "<td style='width:15%;'>" + stockBy + "</td>";
            tr += "<td style='width:10%;'>" + rtnDate + "</td>";
            tr += "<td style='width:15%;'> <a onclick=\"javascript:return FIllForEdit(this);\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
            tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteItemGatePass(this)'><img alt='Delete' src='../Images/delete.png' /></a></td>";
            tr += "<td style='display:none'>" + gatePassId + "</td>";
            tr += "<td style='display:none'>" + gatePassDetailsId + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + costCentreId + "</td>";
            tr += "<td style='display:none'>" + stockById + "</td>";
            tr += "<td style='display:none'>" + returnTypeId + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "<td style='display:none'>" + description + "</td>";
            tr += "</tr>";
            $("#ItemGrid tbody").append(tr);
        }

        //need to check again
        function EditItem(gatePassId, gatePassDetailsId, itemId, costCentreId, stockById, returnTypeId,
                            quantity, itemName, costCentreName, stockBy, returnType, description, returnDate) {
            
            $(editedItem).find("td:eq(0)").text(costCentreName);
            $(editedItem).find("td:eq(1)").text(itemName);
            $(editedItem).find("td:eq(2)").text(quantity);
            $(editedItem).find("td:eq(3)").text(stockBy);
            $(editedItem).find("td:eq(4)").text(returnDate);
            $(editedItem).find("td:eq(6)").text(gatePassId);
            $(editedItem).find("td:eq(7)").text(gatePassDetailsId);
            $(editedItem).find("td:eq(8)").text(itemId);
            $(editedItem).find("td:eq(9)").text(costCentreId);
            $(editedItem).find("td:eq(10)").text(stockById);
            $(editedItem).find("td:eq(11)").text(returnTypeId);
            $(editedItem).find("td:eq(13)").text(description);

            if (gatePassDetailsId != "0")
                $(editedItem).find("td:eq(12)").text("1");

            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_txtQuantity").val("");
            $("#ContentPlaceHolder1_txtItemName").val("");
            $("#btnAddItem").val("Add Item");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_txtItemName").focus();
            editedItem = "";
        }

        //done
        function FIllForEdit(editItem) {
            CommonHelper.SpinnerOpen();
            editedItem = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAddItem").val("Update Item");

            var costCentreId = $(tr).find("td:eq(9)").text();
            var itemId = $(tr).find("td:eq(8)").text();
            var stockById = $(tr).find("td:eq(10)").text();
            var returnDate = $(tr).find("td:eq(4)").text();
            var quantity = $(tr).find("td:eq(2)").text();
            var returnTypeId = $(tr).find("td:eq(11)").text();
            var description = $(tr).find("td:eq(13)").text();
            var itemName = $(tr).find("td:eq(1)").text();

            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", true);

            $("#ContentPlaceHolder1_txtItemName").val(itemName);
            $("#ContentPlaceHolder1_hfItemId").val(itemId);
            $("#ContentPlaceHolder1_ddlCostCentre").val(costCentreId).trigger('change');
            $("#ContentPlaceHolder1_ddlStockBy").val(stockById);
            $("#ContentPlaceHolder1_txtQuantity").val(quantity);
            $("#ContentPlaceHolder1_ddlReturnType").val(returnTypeId);
            $("#ContentPlaceHolder1_txtReturnDate").val(returnDate);
            $("#ContentPlaceHolder1_txtDescription").val(description);
            $("#ContentPlaceHolder1_txtItemName").focus();
            CommonHelper.SpinnerClose();
        }

        //done
        function DeleteItemGatePass(deleteItem) {
            if (!confirm("Do you want to delete?")) {
                return;
            }

            var gatePassId = "0", gatePassDetailsId = "0";
            var tr = $(deleteItem).parent().parent();

            gatePassDetailsId = $(tr).find("td:eq(7)").text();
            gatePassId = $("#ContentPlaceHolder1_hfGatePassId").val();

            if ((gatePassDetailsId != "0")) {
                deleteDbItem.push({
                    GatePassDetailsId: gatePassDetailsId,
                    GatePassId: gatePassId
                });
            }

            $(deleteItem).parent().parent().remove();

            if ($("#ItemGrid tbody tr").length == 0 && $("#ContentPlaceHolder1_hfGatePassId").val() == "0") {
                $("#ContentPlaceHolder1_hfGatePassId").val('0');
                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
            }
        }

        //done
        function ValidationBeforeSave() {
            if ($("#ItemGrid tbody tr").length == 0) {
                toastr.warning("Please add requisition item.");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtGatePassDate").val()) == "") {
                toastr.warning("Please give Gate Pass Date.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlSupplier").val() == "") {
                toastr.warning("Please select a supplier/vendor.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtRemarks").val() == "") {
                toastr.warning("Please give remarks.");
                return false;
            }
            
            
            CommonHelper.SpinnerOpen();

            var gatePassId = "0", gatePassDetailsId = "0", costCentreId = "0", isEdited = "0";
            var supplierId = "", itemId = "", stockById = "", quantity = "", returnType="", returnDate="";
            var gatePassDate = '', requisitionBy = '', remarks = '', responsiblePerson = '', approvedBy = '', description='';

            gatePassId = $("#ContentPlaceHolder1_hfGatePassId").val();
            gatePassDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtGatePassDate").val(), '/');
            supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            remarks = $("#ContentPlaceHolder1_txtRemarks").val();

            var gatePass = {
                GatePassId: parseInt(gatePassId, 10),
                GatePassDate: gatePassDate,
                SupplierId: parseInt(supplierId, 10),
                Remarks: remarks
            };

            newlyAddedItem = [];
            editDbItem = [];

            $("#ItemGrid tbody tr").each(function (index, item) {

                itemId = $.trim($(item).find("td:eq(8)").text());
                costCentreId = $.trim($(item).find("td:eq(9)").text());
                stockById = $.trim($(item).find("td:eq(10)").text());
                quantity = $.trim($(item).find("td:eq(2)").text());
                gatePassDetailsId = $.trim($(item).find("td:eq(7)").text());
                description = $.trim($(item).find("td:eq(13)").text());
                returnType = $.trim($(item).find("td:eq(11)").text());
                returnDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($.trim($(item).find("td:eq(4)").text()), innBoarDateFormat);
                isEdited = $.trim($(item).find("td:eq(12)").text());

                if (gatePassDetailsId == "0") {

                    newlyAddedItem.push({
                        GatePassItemId: parseInt(gatePassDetailsId, 10),
                        GatePassId: parseInt(gatePassId, 10),
                        CostCenterId: parseInt(costCentreId, 10),
                        ItemId: parseInt(itemId, 10),
                        StockById: parseInt(stockById, 10),
                        Quantity: parseFloat(quantity),
                        Description: description,
                        ReturnType: parseInt(returnType, 10),
                        ReturnDate: returnDate
                    });
                }
                else if (gatePassDetailsId != "0" && isEdited != "0") {
                    editDbItem.push({
                        GatePassItemId: parseInt(gatePassDetailsId, 10),
                        GatePassId: parseInt(gatePassId, 10),
                        CostCenterId: parseInt(costCentreId, 10),
                        ItemId: parseInt(itemId, 10),
                        StockById: parseInt(stockById, 10),
                        Quantity: parseFloat(quantity),
                        Description: description,
                        ReturnType: parseInt(returnType, 10),
                        ReturnDate:returnDate
                    });
                }
            });
            PageMethods.SaveGatePass(gatePassId, gatePass, newlyAddedItem, editDbItem, deleteDbItem, OnSaveGatePassSucceed, OnSaveGatePassFailed);

            return false;
        }

        //done
        function OnSaveGatePassSucceed(result) {

            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
            if (queryRqId != "") {
                window.location = "/Maintenance/GatePassInformation.aspx";
            }
            return false;
        }
        //done
        function OnSaveGatePassFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        //done
        function FillForm(GatePassId) {
            CommonHelper.SpinnerOpen();
            PageMethods.FillForm(GatePassId, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        //done
        function OnFillFormSucceed(result) {

            if (result != null) {

                $("#<%=btnSave.ClientID %>").val("Update");
                $("#ItemGrid tbody").html("");

                $("#ContentPlaceHolder1_hfGatePassId").val(result.GatePass.GatePassId);

                var GatePassDate = CommonHelper.DateFromDateTimeToDisplay(result.GatePass.GatePassDate, innBoarDateFormat);

                $("#ContentPlaceHolder1_txtGatePassDate").val(GatePassDate);
                $("#ContentPlaceHolder1_ddlSupplier").val(result.GatePass.SupplierId).trigger('change');
                $("#ContentPlaceHolder1_txtRemarks").val(result.GatePass.Remarks);

                var rowLength = result.GatePassDetails.length;
                var row = 0;

                for (row = 0; row < rowLength; row++) {
                    AddGatePassItem(result.GatePassDetails[row].GatePassId, result.GatePassDetails[row].GatePassItemId,
                                      result.GatePassDetails[row].ItemId, result.GatePassDetails[row].CostCenterId,
                                      result.GatePassDetails[row].StockById, result.GatePassDetails[row].ReturnType,
                                      result.GatePassDetails[row].Quantity, result.GatePassDetails[row].ItemName,
                                      result.GatePassDetails[row].CostCenter, result.GatePassDetails[row].StockBy,
                                      result.GatePassDetails[row].ReturnType, result.GatePassDetails[row].Description,
                                      result.GatePassDetails[row].ReturnDate
                                      );

                }

                //$("#myTabs").tabs('select', 0);
                $("#myTabs").tabs({ active: 0 });
                CommonHelper.SpinnerClose();
            }
        }
        //done
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }
        //Processing...
        function GatePassDetails(GatePassId) {
            CommonHelper.SpinnerOpen();
            PageMethods.GatePassDetails(GatePassId, OnFillItemDetailsSucceed, OnFillItemDetailsFailed);
            return false;
        }
        //// need to recheck with gatepass AddGatePassItem() method
        function OnFillItemDetailsSucceed(result) {

            $("#ItemDetailsGrid tbody").html("");

            var rowLength = result.GatePassDetails.length;
            var row = 0;

            for (row = 0; row < rowLength; row++) {
                AddGatePassItem(result.GatePassDetails[row].GatePassId, result.GatePassDetails[row].GatePassItemId,
                                  result.GatePassDetails[row].ItemId, result.GatePassDetails[row].CostCenterId,
                                  result.GatePassDetails[row].StockById, result.GatePassDetails[row].ReturnType,
                                  result.GatePassDetails[row].Quantity, result.GatePassDetails[row].ItemName,
                                  result.GatePassDetails[row].CostCenter, result.GatePassDetails[row].StockBy,
                                  result.GatePassDetails[row].ReturnType, result.GatePassDetails[row].Description,
                                  result.GatePassDetails[row].ReturnDate
                                  );

            }

            $("#ItemDetailsGridContainer").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 700,
                maxWidth: 800,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Gate Pass Details",
                show: 'slide'
            });
            CommonHelper.SpinnerClose();

        }
        //done
        function OnFillItemDetailsFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        //done
        function PerformClearItems() {

            $("#ContentPlaceHolder1_txtItemName").val('');
            $("#ContentPlaceHolder1_hfItemId").val('0');
            $("#ContentPlaceHolder1_ddlStockBy").val('0');
            $("#ContentPlaceHolder1_txtQuantity").val('');
            $("#ContentPlaceHolder1_txtDescription").val('');
            $("#ContentPlaceHolder1_txtReturnDate").val('');
            $("#ContentPlaceHolder1_ddlCostCentre").val('');

            if ($("#ItemGrid tbody tr").length == 0) {
                $("#ContentPlaceHolder1_ddlSupplier").val('0');
                $("#ContentPlaceHolder1_hfGatePassId").val('0');
                $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);
            }

            editedItem = "";

            // return false;
        }

        //need to check why its not working
        function PerformClearAction() {

            
            deleteDbItem = []; editDbItem = []; newlyAddedItem = [];

            $("#ContentPlaceHolder1_hfGatePassId").val('0');
            $("#ContentPlaceHolder1_txtItemName").val('');
            $("#ContentPlaceHolder1_hfItemId").val('0');
            $("#ContentPlaceHolder1_ddlCostCentre").val('');
            $("#ContentPlaceHolder1_ddlStockBy").val('');
            $("#ContentPlaceHolder1_txtQuantity").val('');
            $("#ContentPlaceHolder1_txtReturnDate").val('');
            $("#ContentPlaceHolder1_txtReturnType").val('1');
            $("#ContentPlaceHolder1_txtDescription").val('');
            $("#ContentPlaceHolder1_txtRemarks").val('');
            $("#ContentPlaceHolder1_ddlSupplier").val('');

            $("#ItemGrid tbody").html("");
            $("#ContentPlaceHolder1_ddlSupplier").attr("disabled", false);

            $("#<%=btnSave.ClientID %>").val("Save");

            return false;
        }

    </script>

    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfGatePassId" runat="server" Value="0" />


    <div style="display: none;">
        <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="customLargeDropDownSize" TabIndex="1">
            <asp:ListItem>Product</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div id="ItemDetailsGridContainer" style="display: none;">
        <table id="ItemDetailsGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                    <th style="width: 30%;">Cost Center
                    </th>
                    <th style="width: 30%;">Item
                    </th>
                    <th style="width: 10%;">Unit
                    </th>
                    <th style="width: 15%;">Stock By
                    </th>
                    <th style="width: 15%;">Return Type
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
                href="#tab-1">Gate Pass</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Gate Pass </a></li>
        </ul>
        <div id="tab-1">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div id="EntryPanel" class="panel panel-default">
                        <div class="panel-heading">
                            Item Entry
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Gate Pass Date"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtGatePassDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Vendor/Supplier"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlSupplier" CssClass="form-control select2" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Cost Center"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlCostCentre" CssClass="form-control select2" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group" id="ItemNamePanel">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblItemName" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtItemName" runat="server" TabIndex="5" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Stock By"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlStockBy" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblQuantity" runat="server" class="control-label required-field" Text="Quantity"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control quantitydecimal" TabIndex="6"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"
                                            TabIndex="8"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblReturnType" runat="server" class="control-label required-field" Text="Return Type"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlReturnType" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Returnable" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Non Returnable" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblReturnDate" runat="server" class="control-label required-field"
                                            Text="Return Date"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtReturnDate" TabIndex="7" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group" style="padding: 5px 0 5px 0;">
                                    <div class="col-md-12">
                                        <input id="btnAddItem" type="button" class="TransactionalButton btn btn-primary btn-sm"
                                            value="Add Item" />
                                        <input id="btnClearGatePass" type="button" class="TransactionalButton btn btn-primary btn-sm"
                                            value="Clear" />
                                    </div>
                                </div>
                                <div class="form-group" id="ItemTableContainer" style="overflow: scroll;">
                                    <table id="ItemGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                                        <thead>
                                            <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                                <th style="width: 25%;">Cost Center</th>
                                                <th style="width: 25%;">Item</th>
                                                <th style="width: 10%;">Unit</th>
                                                <th style="width: 15%;">Stock By</th>
                                                <th style="width: 10%;">Return Type</th>
                                                <th style="width: 15%;">Action</th>
                                                <th style="display: none">GatePass Id</th>
                                                <th style="display: none">GatePass Details Id</th>
                                                <th style="display: none">Item Id</th>
                                                <th style="display: none">Costcenter id</th>
                                                <th style="display: none">Stock By Id</th>
                                                <th style="display: none">Return Type Id</th>
                                                <th style="display: none">Is Edited</th>
                                                <th style="display: none">Description</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblRemarks" runat="server" class="control-label required-field" Text="Remarks"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                TabIndex="8"></asp:TextBox>
                        </div>
                    </div>
<%--                    <div class="form-group" style="display: none">
                        <div class="col-md-2">
                            <asp:Label ID="lblRequisitionBy" runat="server" class="control-label required-field" Text="Requisition By"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRequisitionBy" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                        </div>
                    </div>--%>
                   
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="javascript: return ValidationBeforeSave();"
                               CssClass="TransactionalButton btn btn-primary btn-sm"
                                TabIndex="10" />
                            <asp:Button ID="btnClear" runat="server" TabIndex="11" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformClearAction();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="InfoPanel" class="panel panel-default">
                <div class="panel-heading">
                    Gate Pass Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSGatePassNumber" runat="server" class="control-label" Text="Gate Pass Number"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSGatePassNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" id="DateDiv">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="txtGatePassId" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" OnClientClick="javascript:return PerformClearItems();" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
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
                    <asp:GridView ID="gvGatePassInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnPageIndexChanging="gvGatePassInfo_PageIndexChanging"
                        OnRowCommand="gvGatePassInfo_RowCommand" TabIndex="9" OnRowDataBound="gvGatePassInfo_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("GatePassId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="GatePassNumber" HeaderText="GatePass Number" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Gate Pass Date" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblGatePassDate" runat="server" Text='<%#string.Format("{0:dd/MM/yyyy}",Eval("GatePassDate"))%>'></asp:Label>
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Supplier" HeaderText="Supplier" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ResponsiblePerson" HeaderText="Responsible Person" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ApprovedByPerson" HeaderText="Approved By" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgDetailsGatePass" runat="server" CausesValidation="False"
                                        CommandName="CmdGatePassDetails" CommandArgument='<%# bind("GatePassId") %>'
                                        OnClientClick='<%#String.Format("return GatePassDetails({0})", Eval("GatePassId")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Requisition Details" />
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("GatePassId") %>' OnClientClick='<%#String.Format("return FillForm({0})", Eval("GatePassId")) %>'
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("GatePassId") %>' ImageUrl="~/Images/delete.png"
                                        Text="" AlternateText="Delete" ToolTip="Delete" />
                                    &nbsp;&nbsp;<asp:ImageButton ID="ImgReportRI" runat="server" CausesValidation="False"
                                        CommandName="CmdReportRI" CommandArgument='<%# bind("GatePassId") %>' ImageUrl="~/Images/ReportDocument.png"
                                        Text="" AlternateText="Invoice" ToolTip="Gate Pass Invoice" />
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
