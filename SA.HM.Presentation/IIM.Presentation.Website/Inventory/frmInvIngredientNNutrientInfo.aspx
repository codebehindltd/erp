<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmInvIngredientNNutrientInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmInvIngredientNNutrientInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var vv = [];
        var finisItemEdited = "";
        var DeletedAccountHead = [];
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlItemName").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlNutrient").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlAccountHead").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlItemName").change(function () {
                $("#ContentPlaceHolder1_ddlAccountHead").select2({
                    tags: "true",
                    placeholder: "--- Please Select ---",
                    allowClear: true,
                    width: "99.75%"
                });
                $("#btnCancelOEAmount").click(function () {
                    ClearOverheadExpenseInfo();
                });


                $("#IngredientInformationGrid tbody").html("");
                $("#NutrientInformationGrid tbody").html("");
                $("#OEAmountGrid tbody").html("");
                var itemId = $("#ContentPlaceHolder1_ddlItemName").val();
                                
                PageMethods.GetInvFinishGoodsInformationById(itemId, OnGetInvFinishGoodsInformationByIdSucceeded, OnGetInvFinishGoodsInformationByIdFailed);
                PageMethods.GetInvNutrientInformationById(itemId, OnGetInvNutrientInformationByIdSucceeded, OnGetInvNutrientInformationByIdFailed);
                PageMethods.GetInvFGNNutrientRequiredOEById(itemId, OnGetInvFGNNutrientRequiredByIdSucceeded, OnGetInvFGNNutrientRequiredByIdFailed);
                return false;
            });
            $("#ContentPlaceHolder1_txtRawMaterial").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmInvIngredientNNutrientInfo.aspx/RawMaterialAutoSearch',
                        data: "{'itemName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    StockById: m.StockBy
                                };
                            });
                            response(searchData);

                        },
                        error: function (result) {
                            toastr.error("Please Contact With Admin");
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

                    //if ($.trim($("#ContentPlaceHolder1_hfEditedItemId").val()) != "") {

                    //    if ($.trim($("#ContentPlaceHolder1_hfEditedItemId").val()) == ui.item.value) {
                    //        toastr.info("Same Item Cannot be Added as Recipe.");
                    //        return;
                    //    }
                    //}
                    $("#ContentPlaceHolder1_hfRawMaterialId").val(ui.item.value);
                    $(this).val(ui.item.label);
                }
            });
        });

        function AddOverHeadExpense() {

            var FGItemId = $("#ContentPlaceHolder1_ddlItemName").val();
            var accountHead = $("#ContentPlaceHolder1_ddlAccountHead option:selected").text();
            var accountHeadId = $("#ContentPlaceHolder1_ddlAccountHead").val();
            var amount = $("#ContentPlaceHolder1_txtAmount").val();
            var oEDescription = $.trim($("#ContentPlaceHolder1_txtOEDescription").val());
            if (FGItemId == "0") {
                toastr.warning("Please Select Finish Goods.");
                $("#ContentPlaceHolder1_ddlItemName").focus();
                return false;
            }
            else if (accountHeadId == "0") {
                $("#ContentPlaceHolder1_ddlAccountHead").focus();
                toastr.warning("Please select Account Head.");
                return false;
            }
            else if (amount == "") {
                $("#ContentPlaceHolder1_txtAmount").focus();
                toastr.warning("Please give Amount.");
                return false;
            }
            else if (oEDescription == "") {
                $("#ContentPlaceHolder1_txtOEDescription").focus();
                toastr.warning("Please give OverHead Description.");
                return false;
            }

            var AccountHeadDetailsId = "0", isEdited = 0, editedItemId = "0";

            if (accountHeadId != "0" && finisItemEdited != "") {
                AccountHeadDetailsId = $(finisItemEdited).find("td:eq(4)").text();
                EditAccountHeadForOE(accountHead, amount, oEDescription, AccountHeadDetailsId, FGItemId, isEdited);
                return;
            }
            else if (accountHeadId == "0" && finisItemEdited != "") {
                AccountHeadDetailsId = $(finisItemEdited).find("td:eq(4)").text();
                EditAccountHeadForOE(accountHead, amount, oEDescription, AccountHeadDetailsId, FGItemId, isEdited);
                toastr.info("Edit");
                return;
            }
            if (!IsAccountHeadExistsForOE(FGItemId, accountHeadId)) {
                AddAccountHeadForOEInfo(accountHeadId, accountHead, amount, oEDescription, AccountHeadDetailsId, FGItemId, isEdited);
            }
            ClearOverheadExpenseInfo();
        }

        function ClearOverheadExpenseInfo() {
            $("#ContentPlaceHolder1_ddlAccountHead").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtAmount").val("");
            $("#ContentPlaceHolder1_txtOEDescription").val("");

            return false;

        }
        function EditAccountHeadForOE(accountHead, amount, OEDescription, AccountHeadDetailsId, FGItemId, isEdited) {
            $(finisItemEdited).find("td:eq(0)").text(accountHead);
            $(finisItemEdited).find("td:eq(1)").text(amount);
            $(finisItemEdited).find("td:eq(2)").text(OEDescription);

            $(finisItemEdited).find("td:eq(4)").text(AccountHeadDetailsId);
            $(finisItemEdited).find("td:eq(5)").text(FGItemId);

            if (AccountHeadDetailsId != "0")
                $(finisItemEdited).find("td:eq(6)").text("1");

            $(finisItemEdited).find("td:eq(7)").text((FGItemId + "-" + accountHead));

            $("#ContentPlaceHolder1_txtAmount").val("");
            $("#ContentPlaceHolder1_txtOEDescription").val("");

            $("#btnAdd").val("Add");
            finisItemEdited = "";
        }
        function IsAccountHeadExistsForOE(FGItemId, accountHeadId) {
            var IsDuplicate = false;
            $("#OEAmountGrid tr").each(function (index) {

                if (index !== 0 && !IsDuplicate) {
                    var FGItemIdValueInTable = $(this).find("td").eq(4).html();
                    var accountHeadIdValueInTable = $(this).find("td").eq(5).html();

                    FGItemIdValueInTable = parseInt(FGItemIdValueInTable);
                    FGItemId = parseInt(FGItemId);
                    var IsFGItemIdFound;
                    if (FGItemId == FGItemIdValueInTable) {
                        IsFGItemIdFound = true;
                    }
                    else {
                        IsFGItemIdFound = false;
                    }

                    accountHeadIdValueInTable = parseInt(accountHeadIdValueInTable);
                    accountHeadId = parseInt(accountHeadId);
                    var IsAccountHeadIdFound;
                    if (accountHeadId == accountHeadIdValueInTable) {
                        IsAccountHeadIdFound = true;
                    }
                    else {
                        IsAccountHeadIdFound = false;
                    }
                    if (IsFGItemIdFound && IsAccountHeadIdFound) {
                        toastr.warning('Account Head Already Added.');
                        IsDuplicate = true;
                        return true;
                    }
                }
            });

            return IsDuplicate;
        }
        function AddAccountHeadForOEInfo(accountHeadId, accountHead, amount, OEDescription, AccountHeadDetailsId, FGItemId, isEdited) {
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
            tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";
            //tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return NutrientInfoEditWithConfirmation(this)\" alt='Edit'  title='Edit' border='0' />";
            tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return DeleteAccountHeadOfOE(this)\" alt='Delete'  title='Delete' border='0' />";
            tr += "</td>";

            tr += "<td style='display:none'>" + FGItemId + "</td>";
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
            $("#ContentPlaceHolder1_txttotalAmount").val(totalAmount);
        }

        function DeleteAccountHeadOfOE(control) {
            if (!confirm("Do you want to delete?"))
                return;
            var FGId = $("#ContentPlaceHolder1_ddlItemName").val();
            var tr = $(control).parent().parent();
            var accoutHeadId = $(tr).find("td:eq(5)").text();

            if (accoutHeadId != "0") {

                DeletedAccountHead.push({
                    FinishProductId: FGId,
                    NodeId: accoutHeadId
                });
            }

            $(tr).remove();

            var amountAfterDeletion = 0;
            $("#OEAmountGrid tr").each(function (index) {
                var amount = $(this).find("td").eq(1).html();
                if (amount == undefined) {
                    amount = 0;
                }
                amountAfterDeletion = parseFloat(amountAfterDeletion) + parseFloat(amount);
            });
            amountAfterDeletion = amountAfterDeletion.toFixed(2);
            $("#ContentPlaceHolder1_txttotalAmount").val(amountAfterDeletion);
        }

        function OnGetInvFinishGoodsInformationByIdSucceeded(result) {
            var tr = "";
            $.each(result, function (count, obj) {
                if (result.length > 0) {
                    $("#btnSave").val("Update");
                }
                tr += "<tr>";

                tr += "<td style='width:40%;'>" + obj.RecipeItemName + "</td>";
                tr += "<td style='width:20%;'>" + obj.HeadName + "</td>";
                tr += "<td style='width:15%;'>" + obj.ItemUnit + "</td>";
                tr += "<td style='width:10%;'>" + obj.ItemCost + "</td>";
                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return IngredientInformationDeleteWithConfirmation(this)\" alt='Delete'  title='Delete' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return IngredientInformationEditWithConfirmation(this)\" alt='Edit'  title='Edit' border='0' />";               
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.RecipeItemId + "</td>";
                tr += "<td style='display:none;'>" + obj.UnitHeadId + "</td>";
                tr += "<td style='display:none;'>" + obj.RecipeId + "</td>";

                tr += "</tr>";

                $("#IngredientInformationGrid tbody").append(tr);

                tr = "";
            });
            return false;
        }
        function OnGetInvFinishGoodsInformationByIdFailed() {

        }

        function OnGetInvNutrientInformationByIdSucceeded(result) {
            var tr = "";
            $.each(result, function (count, obj) {
                if (result.length > 0) {
                    $("#btnSave").val("Update");
                }
                tr += "<tr>";

                tr += "<td style='width:35%;'>" + obj.NutrientName + "</td>";
                tr += "<td style='width:15%;'>" + obj.RequiredValue + "</td>";
                tr += "<td style='width:25%;'>" + obj.CalculatedValue + "</td>";
                tr += "<td style='width:10%;'>" + obj.Difference + "</td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return DeleteNutrientRequiredValueItem(this)\" alt='Delete'  title='Delete' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return NutrientInfoEditWithConfirmation(this)\" alt='Edit'  title='Edit' border='0' />";                
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.NutrientId + "</td>";

                tr += "</tr>";

                $("#NutrientInformationGrid tbody").append(tr);

                tr = "";
            });
            return false;
        }
        function OnGetInvNutrientInformationByIdFailed() {

        }
        function OnGetInvFGNNutrientRequiredByIdSucceeded(result) {
            var tr = "";
            $.each(result, function (count, obj) {
                if (result.length > 0) {
                    $("#btnSave").val("Update");
                }
                tr += "<tr>";

                tr += "<td style='width:40%;'>" + obj.AccountHead + "</td>";
                tr += "<td style='width:20%;'>" + obj.Amount + "</td>";
                tr += "<td style='width:20%;'>" + obj.Remarks + "</td>";

                tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";
                //tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return NutrientInfoEditWithConfirmation(this)\" alt='Edit'  title='Edit' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return DeleteAccountHeadOfOE(this)\" alt='Delete'  title='Delete' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.FinishProductId + "</td>";
                tr += "<td style='display:none;'>" + obj.NodeId + "</td>";
                tr += "<td style='display:none'>" + obj.IsEdited + "</td>";

                tr += "</tr>";

                $("#OEAmountGrid tbody").append(tr);

                tr = "";
            });
            return false;
        }
        function OnGetInvFGNNutrientRequiredByIdFailed() {

        }

        function ValidationBeforeSave() {
            var finishGoodId = $("#ContentPlaceHolder1_ddlItemName").val();
            var finishGoodName = $("#ContentPlaceHolder1_ddlItemName option:selected").text();
            if (finishGoodId == "0") {
                toastr.warning("Please Select Finish Goods.");
                return false;
            }
                        
            var AddedIngredientInfo = [], EditIngredientInfo = [];

            $("#IngredientInformationGrid tbody tr").each(function (index, item) {

                var ingredientName = $.trim($(item).find("td:eq(0)").text());
                var unitHeadName = $.trim($(item).find("td:eq(1)").text());
                var itemUnit = $.trim($(item).find("td:eq(2)").text());
                var itemCost = $.trim($(item).find("td:eq(3)").text());
                var ingredientId = $.trim($(item).find("td:eq(5)").text());
                var unitHeadId = $.trim($(item).find("td:eq(6)").text());
                var IsGradientCanChange = false;

                AddedIngredientInfo.push({
                    RecipeId: finishGoodId,
                    RecipeItemId: ingredientId,
                    RecipeItemName: ingredientName,
                    UnitHeadId: unitHeadId,
                    ItemUnit: itemUnit,
                    ItemCost: itemCost,
                    IsGradientCanChange: IsGradientCanChange
                });
            });

            var nutrientRequiredMasterInfo = {
                ItemId: finishGoodId,
                ItemName: finishGoodName
            }
            var AddedNutrientRequiredValueInfoDetail = [], EditNutrientRequiredValueInfo = [];

            $("#NutrientInformationGrid tbody tr").each(function (index, item) {

                var nutrientName = $.trim($(item).find("td:eq(0)").text());
                var requiredValue = $.trim($(item).find("td:eq(1)").text());
                var calculatedValue = $.trim($(item).find("td:eq(2)").text());
                var difference = $.trim($(item).find("td:eq(3)").text());
                var nutrientId = $.trim($(item).find("td:eq(5)").text());

                AddedNutrientRequiredValueInfoDetail.push({
                    Id: finishGoodId,
                    NutrientId: nutrientId,
                    NutrientName: nutrientName,
                    RequiredValue: requiredValue,
                    CalculatedValue: calculatedValue,
                    Difference: difference
                });
            });

            var AddedOverheadExpenses = [], EditedOverheadExpenses = [];
            $("#OEAmountGrid tbody tr").each(function (index, item) {
                //accoutHeadDetailsId = $.trim($(item).find("td:eq(4)").text());
                var accountHeadId = $(item).find("td:eq(5)").text();
                var amount = $(item).find("td:eq(1)").text();
                var description = $(item).find("td:eq(2)").text();
                var isEditOE = $(item).find("td:eq(7)").text();
                AddedOverheadExpenses.push({
                    FinishProductId: finishGoodId,
                    NodeId: accountHeadId,
                    Amount: amount,
                    Remarks: description
                });
            });
            PageMethods.SaveIngredientNNutrientInfo(AddedIngredientInfo, DeletedItemList, nutrientRequiredMasterInfo, AddedNutrientRequiredValueInfoDetail, deletedNutrientRequiredValueList, AddedOverheadExpenses, DeletedAccountHead, OnSaveIngredientNNutrientInfoSucceeded, OnSaveIngredientNNutrientInfoFailed);
            return false;
        }
        function OnSaveIngredientNNutrientInfoSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                $("#btnSave").val("Save");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnSaveIngredientNNutrientInfoFailed(error) {
            toastr.error(error.get_message());
        }
        function OnSaveNutrientInfoSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                $("#btnSave").val("Save");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnSaveNutrientInfoFailed(error) {
            toastr.error(error.get_message());
        }

        function AddRawMaterials() {
            var rawMaterial = $("#ContentPlaceHolder1_txtRawMaterial").val();
            var rawMaterialId = $("#ContentPlaceHolder1_hfRawMaterialId").val();
            var unitHeadId = $("#ContentPlaceHolder1_ddlStockBy").val();
            var unitHeadName = $("#ContentPlaceHolder1_ddlStockBy option:selected").text();
            var unitQuantity = $("#ContentPlaceHolder1_txtUnitQuantity").val();
            var itemCost = $("#ContentPlaceHolder1_txtCost").val();
            if (unitHeadId == "0") {
                toastr.warning("Please Select Stock By.");
                return false;
            }
            else if (unitQuantity == "") {
                toastr.warning("Please Give Unit.");
                return false;
            }
            
            if (rawMaterial != "") {
                AddNewRawMaterial(rawMaterial, rawMaterialId, unitHeadId, unitHeadName, unitQuantity, itemCost);
            }
            else {
                toastr.warning('Raw Material Not found');
                $("#hfRawMaterialId").val('');
                return;
            }
            if ($("#ContentPlaceHolder1_hfIsRawMaterialEdit").val() == 1) {
                ClearRawMaterials();
            }
            if ($("#ContentPlaceHolder1_hfIsNutrientInfoEdit").val() == 1) {
                ClearAfterNutrientRequiredValueAdded();
            }
        }

        function AddNewRawMaterial(rawMaterial, rawMaterialId, unitHeadId, unitHeadName, unitQuantity, itemCost) {
            if (!IsRawMaterialExists(rawMaterialId)) {
                var tr = "", totalRow = 0;
                totalRow = $("#IngredientInformationGrid tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"width:40%; text-align:Left;\">" + rawMaterial + "</td>";
                tr += "<td align='left' style=\"width:20%; text-align:Left;\">" + unitHeadName + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + unitQuantity + "</td>";
                tr += "<td align='left' style='width: 10%;'>" + itemCost + "</td>";
                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return IngredientInformationDeleteWithConfirmation(this)\" alt='Delete'  title='Delete' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return IngredientInformationEditWithConfirmation(this)\" alt='Edit'  title='Edit' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + rawMaterialId + "</td>";
                tr += "<td style='display:none;'>" + unitHeadId + "</td>";

                tr += "</tr>";

                $("#IngredientInformationGrid tbody").prepend(tr);
                tr = "";
            }
            else {
                $("#IngredientInformationGrid tr").each(function () {
                    var currentRawMaterialId = $(this).find("td").eq(5).html();
                    if ($("#ContentPlaceHolder1_hfRawMaterialId").val() > 0 && $("#ContentPlaceHolder1_hfIsRawMaterialEdit").val() == 1) {
                        if (currentRawMaterialId == rawMaterialId) {
                            $(this).find("td").eq(0).html(rawMaterial);
                            $(this).find("td").eq(1).html(unitHeadName);
                            $(this).find("td").eq(2).html(unitQuantity);
                            $(this).find("td").eq(3).html(itemCost);
                            $(this).find("td").eq(5).html(rawMaterialId);
                            $(this).find("td").eq(6).html(unitHeadId);
                        }
                    }
                });
            }

            
        }

        function IsRawMaterialExists(rawMaterialId) {
            var IsDuplicate = false;
            $("#IngredientInformationGrid tr").each(function (index) {
                if (rawMaterialId != "") {
                    if (index !== 0 && !IsDuplicate) {
                        var rawMaterialIdInTable = $(this).find("td").eq(5).html();
                        rawMaterialIdInTable = parseInt(rawMaterialIdInTable);
                        rawMaterialId = parseInt(rawMaterialId);
                        var IsRawMaterialIdFound;
                        if (rawMaterialId == rawMaterialIdInTable) {
                            IsRawMaterialIdFound = true;
                        }
                        else {
                            IsRawMaterialIdFound = false;
                        }
                        if (IsRawMaterialIdFound) {
                            if ($("#ContentPlaceHolder1_hfRawMaterialId").val() > 0 && $("#ContentPlaceHolder1_hfIsRawMaterialEdit").val() == 1) {
                                toastr.success('Raw Material Updated Successfully.');
                                IsDuplicate = true;
                            }
                            else {
                                toastr.warning('Raw Material Already Added.');
                                IsDuplicate = true;
                                return true;
                            }
                        }
                    }
                }
            });
            return IsDuplicate;
        }

        function ClearRawMaterials() {
            $("#ContentPlaceHolder1_txtRawMaterial").val("");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_txtUnitQuantity").val("");
            $("#ContentPlaceHolder1_txtCost").val("");
            $("#ContentPlaceHolder1_txtRawMaterial").attr("disabled", false);
            $("#btnAdd").val("Add");
            $("#ContentPlaceHolder1_hfRawMaterialId").val(0);
            $("#ContentPlaceHolder1_hfIsRawMaterialEdit").val(0);
        }
        function AddNutrientRequiredValue() {
            if ($("#ContentPlaceHolder1_ddlNutrient").val() == "0") {
                toastr.warning("Please Select Nutrient.");
                $("#ContentPlaceHolder1_ddlNutrient").focus();
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtRequiredValue").val() == "") {
                toastr.warning("Please Give Required Value.");
                $("#ContentPlaceHolder1_txtRequiredValue").focus();
                return false;
            }
            AddNutrientRequiredValueTable();
            ClearAfterNutrientRequiredValueAdded();
        }

        function AddNutrientRequiredValueTable() {
            var nutrientName = $("#ContentPlaceHolder1_ddlNutrient option:selected").text();
            var nutrientId = $("#ContentPlaceHolder1_ddlNutrient option:selected").val();
            var requiredValue = $("#ContentPlaceHolder1_txtRequiredValue").val();
            if (!IsNutrientRequiredValueExists(nutrientId)) {
                
                var tr = "";

                tr += "<tr>";
                tr += "<td style='width:35%;'>" + nutrientName + "</td>";
                tr += "<td style='width:15%;'>" + requiredValue + "</td>";
                tr += "<td style='width:25%;'>" + 0 + "</td>";
                tr += "<td style='width:10%;'>" + 0 + "</td>";
                tr += "<td style=\"width:15%;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return NutrientInfoEditWithConfirmation(this)\" alt='Edit'  title='Edit' border='0' />";
                tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'DeleteNutrientRequiredValueItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                tr += "</td>";

                tr += "<td style='display:none;'>" + nutrientId + "</td>";

                tr += "</tr>";

                $("#NutrientInformationGrid tbody").prepend(tr);
                tr = "";
                                
            }
            else {
                $("#NutrientInformationGrid tr").each(function () {
                    var currentNutrientId = $(this).find("td").eq(5).html();
                    if ($("#ContentPlaceHolder1_hfIsNutrientInfoEdit").val() == 1) {
                        if (currentNutrientId == nutrientId) {
                            $(this).find("td").eq(0).html(nutrientName);
                            $(this).find("td").eq(1).html(requiredValue);
                            var requiredValueForDiff = $(this).find("td").eq(1).html();
                            requiredValueForDiff = parseFloat(requiredValueForDiff).toFixed(5);
                            var calculatedValue = $(this).find("td").eq(2).html();
                            calculatedValue = parseFloat(calculatedValue).toFixed(5);
                            var difference = requiredValueForDiff - calculatedValue;
                            difference = parseFloat(difference).toFixed(5);
                            $(this).find("td").eq(3).html(difference);
                            $(this).find("td").eq(5).html(nutrientId);
                        }
                    }
                });
            }
        }

        function ClearAfterNutrientRequiredValueAdded() {
            $("#ContentPlaceHolder1_ddlNutrient").attr("disabled", false);
            $("#ContentPlaceHolder1_ddlNutrient").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtRequiredValue").val("");
            $("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val(0);
            $("#ContentPlaceHolder1_hfIsNutrientInfoEdit").val(0);
            $("#btnAddRequiredValue").val("Add");
        }

        function IsNutrientRequiredValueExists(nutrientId) {
            var IsDuplicate = false;
            $("#NutrientInformationGrid tr").each(function (index) {

                if (index !== 0 && !IsDuplicate) {
                    var nutrientIdValueInTable = $(this).find("td").eq(5).html();
                    nutrientIdValueInTable = parseInt(nutrientIdValueInTable);
                    nutrientId = parseInt(nutrientId);
                    var IsNutrientIdFound;
                    if (nutrientId == nutrientIdValueInTable) {
                        IsNutrientIdFound = true;
                    }
                    else {
                        IsNutrientIdFound = false;
                    }
                    if (IsNutrientIdFound) {
                        if ($("#ContentPlaceHolder1_hfIsNutrientInfoEdit").val() == 1) {
                            toastr.success('Nutrient Required Value Updated Successfully.');
                            IsDuplicate = true;
                        }
                        else {
                            toastr.warning('Nutrient Required Value Already Added.');
                            IsDuplicate = true;
                            return true;
                        }
                    }
                }
            });
            return IsDuplicate;
        }
        function NutrientRequiredValuesEditWithConfirmation(nutrientId, requiredValue) {
            if (!confirm("Do you want to edit item?")) {
                return false;
            }
            $("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val(1);
            $("#ContentPlaceHolder1_ddlNutrient").val(nutrientId).trigger('change');
            $("#ContentPlaceHolder1_txtRequiredValue").val(requiredValue);
        }
        var deletedNutrientRequiredValueList = [];
        function DeleteNutrientRequiredValueItem(control) {
            if (!confirm("Do you want to delete this item?")) { return false; }

            var tr = $(control).parent().parent();
            let nutrientId = $(tr).find("td").eq(5).html();
            var finishGoodsId = $("#ContentPlaceHolder1_ddlItemName").val();
            deletedNutrientRequiredValueList.push({
                NutrientId: nutrientId,
                ItemId: finishGoodsId
            });
            $(tr).remove();
        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfNRVMasterId").val(0);
            $("#ContentPlaceHolder1_ddlItemName").val("0").trigger('change');
            $("#NutrientInformationGrid tbody").html("");
            $("#btnSave").val("Save");
            ClearAfterNutrientRequiredValueAdded();
        }
        function PerformClearActionWithConfirmation() {

            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        var DeletedItemList = [];
        function IngredientInformationDeleteWithConfirmation(control) {
            if (!confirm("Do You Want To Delete?")) {
                return false;
            }
            var tr = $(control).parent().parent();
            var IngredientId = $(tr).find("td").eq(5).html();
            var finishGoodsId = $("#ContentPlaceHolder1_ddlItemName").val();
            DeletedItemList.push({
                ItemId: finishGoodsId,
                RecipeItemId: IngredientId
            });
            $(tr).remove();
        }

                
        function IngredientInformationEditWithConfirmation(control) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            $("#ContentPlaceHolder1_hfIsRawMaterialEdit").val(1);
            var tr = $(control).parent().parent();
            var rawMaterialName = $(tr).find("td").eq(0).html();
            var unitHeadName = $(tr).find("td").eq(1).html();
            var unitQuantity = $(tr).find("td").eq(2).html();
            var itemCost = $(tr).find("td").eq(3).html();
            var rawMaterialId = $(tr).find("td").eq(5).html();
            var unitHeadId = $(tr).find("td").eq(6).html();
            IngredientInformationEdit(rawMaterialName, unitHeadName, unitQuantity, itemCost, rawMaterialId, unitHeadId);
        }
        function IngredientInformationEdit(rawMaterialName, unitHeadName, unitQuantity, itemCost, rawMaterialId, unitHeadId) {
            $("#btnAdd").val("Update");
            $("#ContentPlaceHolder1_hfRawMaterialId").val(rawMaterialId);

            $("#ContentPlaceHolder1_txtRawMaterial").val(rawMaterialName);
            $("#ContentPlaceHolder1_txtRawMaterial").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlStockBy").val(unitHeadId).trigger('change');
            $("#ContentPlaceHolder1_txtUnitQuantity").val(unitQuantity);
            $("#ContentPlaceHolder1_txtCost").val(itemCost);
        }

        function NutrientInfoEditWithConfirmation(control) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            $("#ContentPlaceHolder1_hfIsNutrientInfoEdit").val(1);
            var tr = $(control).parent().parent();
            var nutrientName = $(tr).find("td").eq(0).html();
            var requiredValue = $(tr).find("td").eq(1).html();
            var calculatedValue = $(tr).find("td").eq(2).html();
            var difference = $(tr).find("td").eq(3).html();
            var nutrientId = $(tr).find("td").eq(5).html();
            NutrientInfoEdit(nutrientName, requiredValue, calculatedValue, difference, nutrientId)
        }
        function NutrientInfoEdit(nutrientName, requiredValue, calculatedValue, difference, nutrientId) {

            $("#btnAddRequiredValue").val("Update");
            $("#ContentPlaceHolder1_ddlNutrient").val(nutrientId).trigger('change');
            $("#ContentPlaceHolder1_ddlNutrient").attr("disabled", true);
            $("#ContentPlaceHolder1_txtRequiredValue").val(requiredValue);
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            $('#EntryPanel').show("slow");
            return false;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For Delete-------------------------        
        function PerformDeleteAction(actionId, rowIndex) {
            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
        }
        function OnDeleteObjectFailed(error) {
            //alert(error.get_message());
            toastr.error(error);
        }
        //For ClearForm-------------------------

        function PerformClearActionWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_ddlItemName").val(0).trigger('change');
            $("#ContentPlaceHolder1_hfNutritionTypeId").val(0);
            $("#ContentPlaceHolder1_hfNutrientInfoId").val(0);
            $("#ContentPlaceHolder1_hfEditId").val(0);
            $("#ContentPlaceHolder1_hfRawMaterialId").val(0);
            $("#ContentPlaceHolder1_hfNutrientInfoId").val(0);
            $("#ContentPlaceHolder1_ddlSetupType").val(0).trigger('change');
            $("#ContentPlaceHolder1_ddlNutritionType").val(0).trigger('change');
            $("#ContentPlaceHolder1_txtCode").val("");
            $("#ContentPlaceHolder1_txtName").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_txtDisplaySequence").val("");
            $("#ContentPlaceHolder1_ddlActiveStat").val(0).trigger('change');
            $("#ContentPlaceHolder1_ddlAccountHead").val("0").trigger('change');
            $("#ContentPlaceHolder1_txttotalAmount").val("");
            return false;
        }
                
    </script>
    <asp:HiddenField ID="hfRawMaterialId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsRawMaterialEdit" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsNutrientInfoEdit" runat="server" Value="0" />
    <asp:HiddenField ID="hfNutrientInfoId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfNutritionTypeId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfEditId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfAccoutHeadId" runat="server" Value="0" />
    <div id="myTabs">
        <div class="panel panel-default">
            <div class="panel-heading">
                Ingredient & Nutrient Information
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblItemName" runat="server" class="control-label required-field" Text="Finished Goods"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlItemName" runat="server" CssClass="form-control" TabIndex="2">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        Ingredient Information
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-group">
                                            <div class="col-md-3">
                                                <asp:Label ID="lblRawMaterial" runat="server" class="control-label required-field" Text="Raw Materials"></asp:Label>
                                            </div>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="txtRawMaterial" CssClass="form-control" TabIndex="51" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-3">
                                                <asp:Label ID="lblStockBy" runat="server" class="control-label required-field" Text="Stock By"></asp:Label>
                                            </div>
                                            <div class="col-md-9">
                                                <asp:DropDownList ID="ddlStockBy" runat="server" CssClass="form-control" TabIndex="52">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-3">
                                                <asp:Label ID="lblUnitQuantity" runat="server" class="control-label required-field" Text="Unit"></asp:Label>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtUnitQuantity" runat="server" CssClass="form-control" TabIndex="53"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:Label ID="lblCost" runat="server" class="control-label required-field" Text="Cost"></asp:Label>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtCost" runat="server" CssClass="form-control" TabIndex="54"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group" style="padding-left: 10px;">
                                            <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddRawMaterials()" />
                                            <input id="btnCancel" type="button" value="Cancel" onclick="ClearRawMaterials()" class="TransactionalButton btn btn-primary btn-sm" />
                                        </div>
                                        <div class="row" style="height:300px; overflow-y:scroll;">
                                            <table id="IngredientInformationGrid" class="table table-bordered table-condensed table-responsive">
                                                <thead>
                                                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                                        <th style="width: 40%;">Ingredient Name
                                                        </th>
                                                        <th style="width: 20%;">Stock By
                                                        </th>
                                                        <th style="width: 15%;">Quantity
                                                        </th>
                                                        <th style="width: 10%;">Cost
                                                        </th>
                                                        <th style="width: 15%;">Action</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        Nutrient Information
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-group">
                                            <div class="col-md-3">
                                                <asp:Label ID="lblNutrient" runat="server" class="control-label required-field" Text="Nutrient"></asp:Label>
                                            </div>
                                            <div class="col-md-9">
                                                <asp:DropDownList ID="ddlNutrient" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-3">
                                                <asp:Label ID="lblRequiredValue" runat="server" class="control-label required-field" Text="Required Value"></asp:Label>
                                            </div>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="txtRequiredValue" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group" style="padding-left: 10px;">
                                            <input id="btnAddRequiredValue" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddNutrientRequiredValue()" />
                                            <input id="btnCancelRequiredValue" type="button" value="Cancel" onclick="ClearAfterNutrientRequiredValueAdded()" class="TransactionalButton btn btn-primary btn-sm" />
                                        </div>
                                        <div class="row" style="height:300px; overflow-y:scroll;">
                                            <table id="NutrientInformationGrid" class="table table-bordered table-condensed table-responsive">
                                                <thead>
                                                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                                        <th style="width: 35%;">Nutrient
                                                        </th>
                                                        <th style="width: 15%;">Required
                                                        </th>
                                                        <th style="width: 25%;">Calculated
                                                        </th>
                                                        <th style="width: 10%;">Difference
                                                        </th>
                                                        <th style="width: 15%;">Action
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
                        <div id="OEEntryPanel" class="panel panel-default">
                            <div class="panel-heading">
                                Overhead Expense Information
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblAccountHead" runat="server" class="control-label required-field" Text="Account Head"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:DropDownList ID="ddlAccountHead" runat="server" CssClass="form-control" TabIndex="20">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblAmount" runat="server" class="control-label required-field" Text="Amount"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblOEDescription" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtOEDescription" runat="server" TabIndex="22" CssClass="form-control"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row" style="padding: 5px 0 5px 0;">
                                        <div class="col-md-12">
                                            <button type="button" id="btnAddOEAmount" tabindex="24" class="TransactionalButton btn btn-primary btn-sm" onclick="AddOverHeadExpense()">
                                                Add</button>
                                            <button type="button" id="btnCancelOEAmount" tabindex="24" class="TransactionalButton btn btn-primary btn-sm">
                                                Cancel</button>
                                        </div>
                                    </div>
                                    <div class="form-group" style="padding: 0px;">
                                        <div id="OEAmountGridContainer" style="height:300px; overflow-y:scroll;">
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
                                                        <th style="display: none">CostCenterId
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
                                            <div style="display: none;" class="row" style="padding: 5px 0 5px 0;">
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
                    <div class="row">
                        <div class="col-md-12">
                            <input id="btnSave" type="button" value="Save" onclick="ValidationBeforeSave()"
                                class="TransactionalButton btn btn-primary btn-sm" />
                            <input id="btnCancelTicket" type="button" value="Cancel" onclick="PerformClearActionWithConfirmation()"
                                class="TransactionalButton btn btn-primary btn-sm" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
</asp:Content>