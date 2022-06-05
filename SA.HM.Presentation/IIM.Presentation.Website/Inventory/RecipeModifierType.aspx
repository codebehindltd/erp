<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="true" CodeBehind="RecipeModifierType.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.RecipeModifierType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        var deleteItemObj = [];
        var flag = false;
        $(document).ready(function () {

            $("#ContentPlaceHolder1_txtUnitQuantity").change(function () {
                //debugger;

                var itemId = $('#ContentPlaceHolder1_ddlIngredient').val();
                var unitHeadId = $('#ContentPlaceHolder1_ddlStockBy').val();
                var itemUnit = $('#ContentPlaceHolder1_txtUnitQuantity').val();
                var id = 'ContentPlaceHolder1_txtItemCost';

                GetItemCost(itemId, unitHeadId, itemUnit, id);
                //debugger;
                var itemCost = parseFloat($('#ContentPlaceHolder1_txtItemCost').val());
                //$('#ContentPlaceHolder1_txtNewTotalCost').val(itemCost.toFixed(2));

                var aditionalCost = $('#ContentPlaceHolder1_txtAdditionalCost').val() == '' ? 0 : parseFloat($('#ContentPlaceHolder1_txtAdditionalCost').val());
                var totalCost = (itemCost + (aditionalCost * itemCost / 100));
                $("#ContentPlaceHolder1_txtNewTotalCost").val(totalCost.toFixed(2));


            });


            $("#ContentPlaceHolder1_txtUnitQuantity").keypress(function (event) {
                //debugger;
                var keycode = event.keyCode || event.which;
                if (keycode == '13') {
                    var itemId = $('#ContentPlaceHolder1_ddlIngredient').val();
                    var unitHeadId = $('#ContentPlaceHolder1_ddlStockBy').val();
                    var itemUnit = $('#ContentPlaceHolder1_txtUnitQuantity').val();
                    var id = 'ContentPlaceHolder1_txtItemCost';

                    GetItemCost(itemId, unitHeadId, itemUnit, id);
                    //debugger;
                    var itemCost = parseFloat($('#ContentPlaceHolder1_txtItemCost').val());
                    //$('#ContentPlaceHolder1_txtNewTotalCost').val(itemCost.toFixed(2));

                    var aditionalCost = $('#ContentPlaceHolder1_txtAdditionalCost').val() == '' ? 0 : parseFloat($('#ContentPlaceHolder1_txtAdditionalCost').val());
                    var totalCost = (itemCost + (aditionalCost * itemCost / 100));
                    $("#ContentPlaceHolder1_txtNewTotalCost").val(totalCost.toFixed(2));

                }
            });

            $("#ContentPlaceHolder1_txtDefaultQuantity").change(function () {
                if ($("#ContentPlaceHolder1_ddlStockBy").val() == "0") {
                    toastr.warning('Please Select Stock By.');
                    return false;
                }
                
                    var itemId = $('#ContentPlaceHolder1_ddlIngredient').val();
                    var unitHeadId = $('#ContentPlaceHolder1_ddlStockBy').val();
                    var itemUnit = $('#ContentPlaceHolder1_txtDefaultQuantity').val();
                    var id = 'ContentPlaceHolder1_txtTotalCost';

                    GetItemCost(itemId, unitHeadId, itemUnit, id);
                
            });

            $("#ContentPlaceHolder1_txtDefaultQuantity").keypress(function (event) {
                var keycode = event.keyCode || event.which;
                if (keycode == '13') {
                    var itemId = $('#ContentPlaceHolder1_ddlIngredient').val();
                    var unitHeadId = $('#ContentPlaceHolder1_ddlStockBy').val();
                    var itemUnit = $('#ContentPlaceHolder1_txtDefaultQuantity').val();
                    var id = 'ContentPlaceHolder1_txtTotalCost';

                    GetItemCost(itemId, unitHeadId, itemUnit, id);
                }
            });

            $("#ContentPlaceHolder1_txtAdditionalCost").keypress(function (event) {
                var keycode = event.keyCode || event.which;
                if (keycode == '13') {

                    var itemCost = parseFloat($('#ContentPlaceHolder1_txtItemCost').val());
                    if (itemCost == '') {
                        toastr.warning('Add Item Cost.');
                        $('#ContentPlaceHolder1_txtNewTotalCost').val('');
                        $('#ContentPlaceHolder1_txtAdditionalCost').val('');
                        $('#ContentPlaceHolder1_txtNewTotalCost').val('');
                        return;
                    }

                    var aditionalCost = parseFloat($('#ContentPlaceHolder1_txtAdditionalCost').val());
                    var totalCost = itemCost + (aditionalCost * itemCost / 100);
                    $('#ContentPlaceHolder1_txtNewTotalCost').val(totalCost.toFixed(2));
                }

            });

            $("#ContentPlaceHolder1_txtAdditionalCost").change(function () {


                var itemCost = parseFloat($('#ContentPlaceHolder1_txtItemCost').val());
                if (itemCost == '') {
                    toastr.warning('Add Item Cost.');
                    $('#ContentPlaceHolder1_txtNewTotalCost').val('');
                    $('#ContentPlaceHolder1_txtAdditionalCost').val('');
                    $('#ContentPlaceHolder1_txtNewTotalCost').val('');
                    return;
                }

                var aditionalCost = parseFloat($('#ContentPlaceHolder1_txtAdditionalCost').val());
                var totalCost = itemCost + (aditionalCost * itemCost / 100);
                $('#ContentPlaceHolder1_txtNewTotalCost').val(totalCost.toFixed(2));


            });


            $("#ContentPlaceHolder1_ddlSetupType").change(function () {
                var type = ($("#ContentPlaceHolder1_ddlSetupType").val().trim());
                $('#ContentPlaceHolder1_txtDefaultQuantity').val('');
                $('#ContentPlaceHolder1_txtTotalCost').val('');
                $('#ContentPlaceHolder1_ddlStockBy').val(0);
                $('#ContentPlaceHolder1_txtAverageCost').val('');
                $('#ContentPlaceHolder1_txtAverageCostUnit').text('');
                ClearDefault();
                ClearRecipeeModifierPanel();
                //debugger;


                $('#ContentPlaceHolder1_txtAverageCost').attr("disabled", true);
                $('#ContentPlaceHolder1_txtTotalCost').attr("disabled", true);
                if (type == "ExistingItem") {
                    var itemid = parseInt($("#ContentPlaceHolder1_ddlItemName").val().trim());
                    $("#ContentPlaceHolder1_txtDefaultQuantity,#ContentPlaceHolder1_ddlStockBy").attr("disabled", true);
                    PageMethods.GetIChangeableRecipeItemByItemID(itemid, OnLoadGetItemsSucceed, OnLoadGetItemsFailed);
                }
                else {
                    $("#ContentPlaceHolder1_txtDefaultQuantity,#ContentPlaceHolder1_ddlStockBy").attr("disabled", false);
                    PageMethods.GetAllItemForRecipe(OnLoadGetItemsSucceed, OnLoadGetItemsFailed);
                }


                return false;

            });
            $("#ContentPlaceHolder1_ddlItemName").change(function () {
                ClearDefault();
                ClearRecipeeModifierPanel();
                $('#ContentPlaceHolder1_ddlSetupType').val('');
                $("#ContentPlaceHolder1_ddlIngredient").empty();
                return false;
            });

            $("#ContentPlaceHolder1_ddlIngredient").change(function () {

                ClearDefault();
                ClearRecipeeModifierPanel();
                debugger;

                var type = ($("#ContentPlaceHolder1_ddlSetupType").val().trim());
                var IngredientId = parseInt($("#ContentPlaceHolder1_ddlIngredient").val().trim());
                var itemid = parseInt($("#ContentPlaceHolder1_ddlItemName").val().trim());
                //debugger;
                if (type == "ExistingItem") {

                    PageMethods.GetChangeableRecipeItemDetails(IngredientId, itemid, OnLoadGetIngredientItemDetailsSucceed, OnLoadGetIngredientItemDetailsFailed);

                }
                else {
                    flag = false;
                    // PageMethods.GetChangeableRecipeItemDetails(IngredientId, itemid, OnDuplicateCheck, OnDuplicateFalse);

                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/RecipeModifierType.aspx/GetChangeableRecipeItemDetails',
                        data: "{'IngredientId':'" + IngredientId + "','itemid':'" + itemid + "'}",
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            //debugger;
                            if (data.d.length > 0) {
                                flag = true;
                            }
                        },
                        error: function (result) {
                            toastr.error("Please Contact With Admin");
                        }
                    });


                    //debugger;
                    if (flag == true) {
                        toastr.warning("Ingredient item is an existing item.");
                        return;
                    }

                    PageMethods.GetChangeableRecipeItemDetails(IngredientId, 0, OnLoadGetIngredientItemDetailsSucceed, OnLoadGetIngredientItemDetailsFailed);
                }
                PageMethods.GetPreviousRecipeModifierTypes(IngredientId, itemid, OnLoadGetPreviousRecipeModifierTypesSucceed, OnLoadGetPreviousRecipeModifierTypesFailed);


                return false;

            });




            $("#ContentPlaceHolder1_ddlItemName").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlIngredient").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });



            $("#btnAdd").click(function () {


                var duplicateCheck = false;
                var UnitHead = ($("#ContentPlaceHolder1_txtUnitHead").val());
                if ($("#ContentPlaceHolder1_txtUnitHead").val() == "") {
                    toastr.warning('Please Add Unit Head.');
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtUnitQuantity").val() == "") {
                    toastr.warning('Please Add Unit Quantity.');
                    return false;
                }
                else if ($("#ContentPlaceHolder1_txtItemCost").val() == "") {
                    toastr.warning('Please Add Item Cost.');
                    return false;
                }
                //else if ($("#ContentPlaceHolder1_txtAdditionalCost").val() == "") {
                //    toastr.warning('Please Add Additional Cost.');
                //    return false;
                //}

                $('#RecipeItemInformation tbody > tr > td:nth-child(2)').filter(function (index) {
                    if (($.trim($(this).text())) === UnitHead)
                        duplicateCheck = true;
                });
                if (!duplicateCheck) {
                    $('#ContentPlaceHolder1_ddlIngredient').attr("disabled", true);
                    $('#ContentPlaceHolder1_ddlSetupType').attr("disabled", true);
                    $('#ContentPlaceHolder1_ddlItemName').attr("disabled", true);
                    AddNewItem();

                }
                else {
                    toastr.warning('Duplicate Item');
                    return;
                }
                ClearRecipeeModifierPanel();
                return false;
            });

            $("#btnRecipeModifierPanelClear").click(function () {

                if (!confirm("Do you want to clear?")) {
                    return false;
                }

                ClearRecipeeModifierPanel();
                return false;
            });


            $("#btnClear").click(function () {

                if (!confirm("Do you want to clear?")) {
                    return false;
                }
                ClearDefault();
                ClearRecipeeModifierPanel();
                Clear();
                EnableItemModification();
                return false;
            });




        });

        function ClearRecipeeModifierPanel() {
            $('#ContentPlaceHolder1_txtUnitHead').val('');
            $('#ContentPlaceHolder1_txtUnitQuantity').val('');
            $('#ContentPlaceHolder1_txtItemCost').val('');
            $('#ContentPlaceHolder1_txtAdditionalCost').val('');
            $('#ContentPlaceHolder1_txtNewTotalCost').val('');
        }

        function Clear() {

            $('#ContentPlaceHolder1_ddlItemName').val('0').trigger('change');;
            $('#ContentPlaceHolder1_ddlSetupType').val('');
            $("#ContentPlaceHolder1_ddlIngredient").empty();





        }

        function EnableItemModification() {
            $('#ContentPlaceHolder1_ddlIngredient').attr("disabled", false);
            $('#ContentPlaceHolder1_ddlSetupType').attr("disabled", false);
            $('#ContentPlaceHolder1_ddlItemName').attr("disabled", false);
        }

        function ClearDefault() {
            var Table = document.getElementById("RecipeItemInformation");
            if (Table != null) {
                Table.innerHTML = "";
            }
            $('#RecipeModifierPanel').hide("slow");
            deleteItemObj = [];
            $('#ContentPlaceHolder1_txtDefaultQuantity').val('');
            $('#ContentPlaceHolder1_txtTotalCost').val('');
            $('#ContentPlaceHolder1_ddlStockBy').val('');
            $('#ContentPlaceHolder1_txtAverageCost').val('');
            $('#ContentPlaceHolder1_txtAverageCostUnit').text('');
        }

        function AddNewItem() {
            var id = 0;
            //debugger;
            var unitHead = $('#ContentPlaceHolder1_txtUnitHead').val();
            var unitQuantity = $('#ContentPlaceHolder1_txtUnitQuantity').val();
            var additionalPrice = $('#ContentPlaceHolder1_txtAdditionalCost').val() == '' ? 0 : $('#ContentPlaceHolder1_txtAdditionalCost').val();
            var totalCost = $('#ContentPlaceHolder1_txtNewTotalCost').val();

            if ($("#TableWiseRecipeModifier > table").length > 0) {
                AddNewRow(id, unitHead, unitQuantity, additionalPrice, totalCost);
                return false;
            }


            var table = "", deleteLink = "";

            deleteLink = "<a href=\"javascript:void();\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            table += "<table cellspacing='0' cellpadding='4' id='RecipeItemInformation' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            table += "<th style='display:none'></th><th align='left' scope='col' style='width: 20%;'>Unit Head</th>";
            table += "<th align='left' scope='col' style='width: 20%;'>Unit Quantity</th> <th align='left' scope='col' style='width: 20%;'>Additional Cost</th>";
            table += "<th align='left' scope='col' style='width: 20%;'>Total Cost</th>";
            table += "<th align='left' scope='col' style='width: 20%;'>Action</th></tr></thead>";

            table += "<tbody>";
            table += "<tr style=\"background-color:#E3EAEB;\">";

            table += "<td align='left' style=\"display:none;\">" + id + "</td>";
            table += "<td align='left' style=\"width:20%; text-align:left;\">" + unitHead + "</td>";
            table += "<td align='left' style=\"width:20%; text-align:left;\">" + unitQuantity + "</td>";
            table += "<td align='left' style=\"width:20%; text-align:left;\">" + additionalPrice + "</td>";
            table += "<td align='left' style='width: 20%;'>" + totalCost + "</td>";
            table += "<td align='left' style=\"width:20%; cursor:pointer;\">" + deleteLink + "</td>";

            table += "</tr>";
            table += "</tbody>";
            table += "</table>";

            $("#TableWiseRecipeModifier").html(table);
        }

        function AddNewRow(id, unitHead, unitQuantity, additionalPrice, totalCost) {
            var tr = "", totalRow = 0, deleteLink = "";
            totalRow = $("#RecipeItemInformation tbody tr").length;

            deleteLink = "<a href=\"#\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }

            tr += "<td align='left' style=\"display:none;\">" + id + "</td>";
            tr += "<td align='left' style=\"width:20%; text-align:left;\">" + unitHead + "</td>";
            tr += "<td align='left' style=\"width:20%; text-align:left;\">" + unitQuantity + "</td>";
            tr += "<td align='left' style=\"width:20%; text-align:left;\">" + additionalPrice + "</td>";
            tr += "<td align='left' style='width: 20%;'>" + totalCost + "</td>";
            tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#RecipeItemInformation tbody").append(tr);
        }

        function DeleteItem(anchor) {
            ff = anchor;
            var tr = $(anchor).parent().parent();

            //debugger;
            var id = $.trim($(tr).find("td:eq(0)").text());
            var itemId = $.trim($(tr).find("td:eq(1)").text());

            if (parseInt(id, 10) != 0) {
                deleteItemObj.push({
                    RecipeId: id
                });
            }

            $(tr).remove();

            return false;
        }

        function OnLoadGetItemsSucceed(results) {

            $("#ContentPlaceHolder1_ddlIngredient").empty();
            var i = 0, fieldLength = results.length;

            if (fieldLength > 0) {
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlIngredient');
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + results[i].ItemId + '">' + results[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlIngredient');
                }
            }
            else {
                $("<option value='0'>--No Item Found--</option>").appendTo("#ContentPlaceHolder1_ddlIngredient");
            }

        }

        function OnLoadGetItemsFailed() {

        }


        function OnLoadGetPreviousRecipeModifierTypesSucceed(results) {
            //debugger;

            var table = "", deleteLink = "";

            deleteLink = "<a href=\"javascript:void();\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            table += "<table cellspacing='0' cellpadding='4' id='RecipeItemInformation' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            table += "<th style='display:none'></th><th align='left' scope='col' style='width: 20%;'>Unit Head</th>";
            table += "<th align='left' scope='col' style='width: 20%;'>Unit Quantity</th> <th align='left' scope='col' style='width: 20%;'>Additional Cost</th>";
            table += "<th align='left' scope='col' style='width: 20%;'>Total Cost</th>";
            table += "<th align='left' scope='col' style='width: 20%;'>Action</th></tr></thead>";

            table += "<tbody>";
            //table += "<tr style=\"background-color:#E3EAEB;\">";

            //table += "<td align='left' style=\"display:none;\">" + id + "</td>";
            //table += "<td align='left' style=\"width:20%; text-align:Left;\">" + unitHead + "</td>";
            //table += "<td align='left' style=\"width:20%; text-align:Left;\">" + unitQuantity + "</td>";
            //table += "<td align='left' style=\"width:20%; text-align:Left;\">" + additionalPrice + "</td>";
            //table += "<td align='left' style='width: 20%;'>" + totalCost + "</td>";
            //table += "<td align='left' style=\"width:20%; cursor:pointer;\">" + deleteLink + "</td>";

            //table += "</tr>";
            //table += "</tbody>";
            //table += "</table>";




            //var counter = 0;
            if (results != null) {
                for (var counter = 0; counter < results.length; counter++) {
                    //totalRecipeCost += dr.ItemCost;




                    if (counter % 2 == 0) {
                        // It's even
                        table += "<tr style='background-color:White;'>";
                    }
                    else {
                        // It's odd
                        table += "<tr style='background-color:#E3EAEB;'>";
                    }

                    table += "<td align='left' style=\"display:none;\">" + results[counter].Id + "</td>";
                    table += "<td align='left' style=\"width:20%; text-align:Left;\">" + results[counter].HeadName + "</td>";
                    table += "<td align='left' style=\"width:20%; text-align:Left;\">" + results[counter].ItemUnit + "</td>";
                    table += "<td align='left' style=\"width:20%; text-align:Left;\">" + results[counter].AditionalCost + "</td>";
                    table += "<td align='left' style='width: 20%;'>" + results[counter].TotalCost + "</td>";
                    table += "<td align='left' style=\"width:20%; cursor:pointer;\">" + deleteLink + "</td>";

                    table += "</tr>";

                }
            }
            table += "</tbody>";
            table += "</table>";
            //if (table == "") {
            //    strTable = "<tr><td colspan='4' align='left'>No Record Available!</td></tr>";
            //}

            $("#TableWiseRecipeModifier").html(table);


        }

        function OnLoadGetPreviousRecipeModifierTypesFailed() {

        }



        function OnLoadGetIngredientItemDetailsSucceed(results) {
            $('#RecipeModifierPanel').show("slow");
            debugger;

            $('#ContentPlaceHolder1_txtDefaultQuantity').val(results[0].ItemUnit);
            $('#ContentPlaceHolder1_txtTotalCost').val((results[0].ItemCost).toFixed(2));
            $('#ContentPlaceHolder1_ddlStockBy').val(results[0].UnitHeadId);
            $('#ContentPlaceHolder1_txtAverageCost').val((results[0].AverageCost).toFixed(2));
            $('#ContentPlaceHolder1_txtAverageCostUnit').text(('/ ' + results[0].AverageUnitHead));


        }

        function OnLoadGetIngredientItemDetailsFailed() {

        }
        function ValidationNPreprocess() {

            var IsDefault = false;
            $('#RecipeItemInformation tbody > tr > td:nth-child(2)').filter(function (index) {
                if (($.trim($(this).text())) === 'default')
                    IsDefault = true;
            });

            var saveItemObj = [];
            if ($("#ContentPlaceHolder1_ddlItemName").val() == "0") {
                toastr.warning('Please Select Item Name.');
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlSetupType").val() == "") {
                toastr.warning('Please Select Item Type.');
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlIngredient").val() == "0") {
                toastr.warning('Please Select Ingredient.');
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlSetupType").val() == "NewItem") {

                if (!IsDefault) {

                    if ($("#ContentPlaceHolder1_txtDefaultQuantity").val() == "") {
                        toastr.warning('Please Add Default Quantity.');
                        return false;
                    }
                    else if ($("#ContentPlaceHolder1_txtTotalCost").val() == "") {
                        toastr.warning('Please Add Total Cost.');
                        return false;
                    }
                    else if ($("#ContentPlaceHolder1_ddlStockBy").val() == "0") {
                        toastr.warning('Please Add Stock By.');
                        return false;
                    }
                }

            }

            var rowLength = $("#TableWiseRecipeModifier > table tbody tr").length;

            $("#TableWiseRecipeModifier > table tbody tr").each(function () {

                var id = parseInt($.trim($(this).find("td:eq(0)").text(), 10));
                var headName = ($.trim($(this).find("td:eq(1)").text(), 10));
                var ItemUnit = parseFloat($.trim($(this).find("td:eq(2)").text()));
                var AditionalCost = parseFloat($.trim($(this).find("td:eq(3)").text()));
                var TotalCost = parseFloat($.trim($(this).find("td:eq(4)").text()));
                var UnitHeadId = parseFloat($('#ContentPlaceHolder1_ddlStockBy').val());
                if (id == 0) {
                    saveItemObj.push({
                        Id: id,
                        HeadName: headName,
                        ItemUnit: ItemUnit,
                        AditionalCost: AditionalCost,
                        TotalCost: TotalCost,
                        UnitHeadId: UnitHeadId
                    });
                }



            });

            if ($("#ContentPlaceHolder1_ddlSetupType").val() == "NewItem") {


                if (!IsDefault) {

                    var id = 0;
                    var headName = 'default';
                    var ItemUnit = parseFloat($('#ContentPlaceHolder1_txtDefaultQuantity').val());
                    var AditionalCost = 0.0;
                    var TotalCost = parseFloat($('#ContentPlaceHolder1_txtTotalCost').val());
                    var UnitHeadId = parseFloat($('#ContentPlaceHolder1_ddlStockBy').val());

                    saveItemObj.push({
                        Id: id,
                        HeadName: headName,
                        ItemUnit: ItemUnit,
                        AditionalCost: AditionalCost,
                        TotalCost: TotalCost,
                        UnitHeadId: UnitHeadId
                    });
                }
            }

            var itemId = parseFloat($('#ContentPlaceHolder1_ddlItemName').val());
            var recipeItemId = parseFloat($('#ContentPlaceHolder1_ddlIngredient').val());
            //debugger;
            var IsAddedChange = true;

            if (saveItemObj.length == 0) {
                IsAddedChange = false;
            }
            var IsDeletedChange = true;
            if (deleteItemObj.length == 0) {
                IsDeletedChange = false;
            }

            if (!IsDeletedChange && !IsAddedChange) {
                toastr.warning('Nothing is changed.');
                return false;
            }


            PageMethods.SaveAndDelete(saveItemObj, deleteItemObj, itemId, recipeItemId, SaveAndDeleteSucceed, SaveAndDeleteFailed);
            return false;

        }

        function GetItemCost(itemId, stockById, quantity, id) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmInvItem.aspx/GetReceipeItemCost',
                data: "{'itemId':'" + itemId + "','stockById':'" + stockById + "','quantity':'" + quantity + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != null) {
                        $("#" + id + "").val((data.d).toFixed(2));
                    }
                    else {
                        $("#" + id + "").val("0");
                    }
                },
                error: function (result) {
                    toastr.error("Please Contact With Admin");
                }
            });

            return false;
        }

        function SaveAndDeleteSucceed(results) {
            if (results == true) {
                toastr.success('Recipe Types Updated Successfully.');
                //debugger;
                ClearRecipeeModifierPanel();
                Clear();
                ClearDefault();
                EnableItemModification();
            }
            else {
                toastr.error('Contact with admin.');

            }



        }

        function SaveAndDeleteFailed(error) {

        }
    </script>


    <div id="ProductInputPAnel" class="panel panel-default">
        <div class="panel-heading">
            Ingredient Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2 label-align">
                        <asp:Label ID="lblItemName" runat="server" class="control-label required-field"
                            Text="Item Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlItemName" CssClass="form-control" runat="server" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 label-align">
                        <asp:Label ID="lblSetupType" runat="server" class="control-label required-field"
                            Text="Item Type"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSetupType" CssClass="form-control" runat="server" TabIndex="2">
                            <asp:ListItem Text="--- Please Select ---" Value=""></asp:ListItem>
                            <asp:ListItem Text="Existing Item" Value="ExistingItem"></asp:ListItem>
                            <asp:ListItem Text="New Item" Value="NewItem"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div>
                    <div class="form-group">
                        <div class="col-md-2 label-align">
                            <asp:Label ID="lblIngredient" runat="server" class="control-label required-field"
                                Text="Ingredient"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlIngredient" CssClass="form-control" runat="server" TabIndex="3">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 label-align">
                            <asp:Label ID="lblStockBy" runat="server" class="control-label required-field"
                                Text="Stock By"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStockBy" CssClass="form-control" runat="server" TabIndex="6">
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-2 label-align">
                            <asp:Label ID="lblAverageCost" runat="server" class="control-label required-field"
                                Text="Average Cost"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtAverageCost" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="txtAverageCostUnit" runat="server" class="control-label"
                                Text=""></asp:Label>
                        </div>

                    </div>

                    <div class="form-group">
                        <div class="col-md-2 label-align">
                            <asp:Label ID="lblDefaultQuantity" runat="server" class="control-label required-field"
                                Text="Default Quantity"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDefaultQuantity" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                        </div>

                        <div class="col-md-2 label-align">
                            <asp:Label ID="lblTotalCost" runat="server" class="control-label required-field"
                                Text="Total Cost"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtTotalCost" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                        </div>


                    </div>


                    <div id="RecipeModifierPanel" class="panel panel-default" style="display: none;">

                        <div class="panel-body">
                            <div class="form-group">

                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblUnitHead" runat="server" class="control-label required-field"
                                        Text="Unit Head"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtUnitHead" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                                </div>

                            </div>

                            <div class="form-group">

                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblUnitQuantity" runat="server" class="control-label required-field"
                                        Text="Unit Quantity"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtUnitQuantity" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                                </div>

                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblItemCost" runat="server" class="control-label required-field"
                                        Text="Item Cost"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtItemCost" runat="server" CssClass="form-control" TabIndex="10" ReadOnly></asp:TextBox>
                                </div>

                            </div>
                            <div class="form-group">

                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblAdditionalCost" runat="server" class="control-label"
                                        Text="Additional Cost"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtAdditionalCost" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>

                                </div>
                                <div class="col-md-1 label-align">
                                    <asp:Label ID="lblPercentage" runat="server" class="control-label"
                                        Text="%"></asp:Label>
                                </div>

                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblNewTotalCost" runat="server" class="control-label required-field"
                                        Text="Total Cost"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNewTotalCost" runat="server" CssClass="form-control" TabIndex="12" ReadOnly></asp:TextBox>
                                </div>
                            </div>




                            <div class="row">
                                <div class="col-md-12">
                                    <button type="button" id="btnAdd" tabindex="13" class="TransactionalButton btn btn-primary btn-sm">
                                        Add</button>
                                    <button type="button" id="btnRecipeModifierPanelClear" tabindex="14" class="TransactionalButton btn btn-primary btn-sm">
                                        Clear</button>
                                </div>
                            </div>
                            &nbsp;
                            <div class="row">

                                <div class="col-md-12">
                                    <div id="TableWiseRecipeModifier" runat="server" clientidmode="Static">
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="15" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript:return ValidationNPreprocess();" />
                        <button type="button" id="btnClear" tabindex="16" class="TransactionalButton btn btn-primary btn-sm">
                            Clear</button>
                    </div>
                </div>


            </div>

        </div>

    </div>




</asp:Content>
