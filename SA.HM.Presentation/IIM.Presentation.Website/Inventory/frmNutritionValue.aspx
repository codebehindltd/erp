<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmNutritionValue.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmNutritionValue" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .nutritionTable tbody tr td span:nth-child(1) input {
            margin-left: 0 !important;
        }

        .nutritionTable tbody tr td span input {
            margin: 0 !important;
            padding: 0 !important;
        }

        .nutritionTable {
            position: relative;
        }

        .nutritionTable thead {
            position: sticky;
            z-index: 2;
            top: -2px;
         }

        .nutritionTable thead tr td {
            position: relative;

        }
        .nutritionTable thead tr td::before {
            content: "";
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 1px;
            z-index: 2;
            background: #fff;
        }

        .nutritionTable tbody tr td:first-child, .nutritionTable thead tr td:first-child {
            position: sticky;
            left: -2px;
            z-index: 1;
            background: #fff;
            vertical-align: middle;
            -moz-box-shadow: 0px 5px 15px rgba(0, 0, 0, 0.35);
            -webkit-box-shadow: 0px 5px 15px rgba(0, 0, 0, 0.35);
            box-shadow: 0px 5px 15px rgba(0, 0, 0, 0.35);
        }

        .nutritionTable thead tr td:first-child {
            background: #7a919f;
            z-index: 3;
        }

        .table-bordered > thead > tr > th, .table-bordered > thead > tr > td {
            border-bottom-width: 0px;
            border-top-width: 0px;
        }

        .nutritionTable thead td span, .nutritionTable tbody tr td span {
            display: inline-block;
            position: relative;
            margin-right: 8px;
            padding-right: 8px;
        }
        .nutritionTable thead td span {
            padding-right: 8px !important;
            margin-right: 8px !important;
            width: 153px !important;
        }
        .nutritionTable thead td span::before,
        .nutritionTable tbody tr td span::before {
            content: "";
            position: absolute;
            right: 0;
            top: -6px;
            height: calc(100% + 12px);
            width: 1px;
            background: #ddd;
        }

        .nutritionTable thead td span::before {
            right: 0;
            top: -5px;
            height: calc(100% + 10px);
        }

        .nutritionTable tbody tr td span.valueNo {
            display: flex;
            flex-direction: row-reverse;
            align-items: center;
            justify-content: space-between;
        }
        .nutritionTable tbody tr td span.valueNo::before,
        .nutritionTable tbody tr td span.valueNo span:nth-child(2)::before {
            display: none;
        }
        .nutritionTable tbody tr td span.valueNo span:nth-child(1) {
            margin: 0 0 0 8px;
            padding: 0 0 0  8px;
        }
        .nutritionTable tbody tr td span.valueNo span:nth-child(1)::before {
            display: block;
            right: auto;
            left: 0;
            height: calc(100% + 16px);
        }

    </style>
    <script type="text/javascript">
        
        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            $("#ContentPlaceHolder1_txtFormula").attr("disabled", true);

            LoadNutritionType();
            
            $("#myTabs").tabs();         
        });
        var AddedNutrients = [];
        function getNutrientAmount(itemId, nutritionTypeId, nutrientId) {
            let boxId = "";
            let nutrientColumnName = intToString(nutrientId);
            boxId = boxId + itemId + nutritionTypeId + nutrientColumnName;
            let secItemId =$("#ContentPlaceHolder1_hfSecondItemId").val();
            let secNutritionTypeId = $("#ContentPlaceHolder1_hfSecondNutritionTypeId").val();
            let secNutrientId = $("#ContentPlaceHolder1_hfSecondNutrientId").val();
            let deselectBoxId = "";
            if (secNutrientId != "0" && secItemId != "0") {
                secNutrientId = parseInt(secNutrientId);
                secItemId = parseInt(secItemId);
                secNutritionTypeId = parseInt(secNutritionTypeId);
                let deselectNutrientColumnName = intToString(secNutrientId);
                deselectBoxId = deselectBoxId + secItemId + secNutritionTypeId + deselectNutrientColumnName;
            }
            
            $("#ContentPlaceHolder1_hfSelectedBoxId").val(boxId);
            $("#ContentPlaceHolder1_hfItemId").val(itemId);
            $("#ContentPlaceHolder1_hfNutritionTypeId").val(nutritionTypeId);
            $("#ContentPlaceHolder1_hfNutrientId").val(nutrientId);
            var boxCurrentVal = $("#" + boxId + "").val();
            $("#" + boxId + "").blur(function () {
                $("#" + boxId + "").css({ color: 'red', border: '2px solid blue' });
                if (deselectBoxId != "" && boxId != deselectBoxId) {
                    $("#" + deselectBoxId + "").css({ color: 'black', border: '1px solid gray' });
                }
            });

            $("#ContentPlaceHolder1_hfSecondItemId").val($("#ContentPlaceHolder1_hfItemId").val());
            $("#ContentPlaceHolder1_hfSecondNutritionTypeId").val($("#ContentPlaceHolder1_hfNutritionTypeId").val());
            $("#ContentPlaceHolder1_hfSecondNutrientId").val($("#ContentPlaceHolder1_hfNutrientId").val());

            var boxCurrentFormula = "";
            $.each(AddedNutrients, function (count, obj) {
                if (obj.ItemId == itemId && obj.NutrientId == nutrientId && obj.NutritionTypeId == nutritionTypeId) {
                    boxCurrentFormula = obj.Formula;
                }
            });
            $("#ContentPlaceHolder1_txtFormula").attr("disabled", false);
            if (boxCurrentFormula == "" || boxCurrentFormula == undefined || boxCurrentFormula == 0.00) {
                $("#ContentPlaceHolder1_txtFormula").val(boxCurrentVal);
            } else {
                $("#ContentPlaceHolder1_txtFormula").val("="+boxCurrentFormula);
            }
            
            $("#ContentPlaceHolder1_txtFormula").focus();
            $("#ContentPlaceHolder1_txtFormula").select();
            return false;
        }
        var nutritionTypeList = new Array();
        var nutrientInfoList = new Array();

        function LoadNutritionType() {
            PageMethods.GetNutritionType(OnGetNutritionTypeSucceed, OnGetNutritionTypeFailed);
            return false;
        }
        function OnGetNutritionTypeSucceed(result) {
            nutritionTypeList = result;
            var tr = "";
            tr += "<tr>";
            var lengthOfResult = result.length;
            tr += "<td style='white-space: nowrap;'></td>";
            $.each(result, function (count, obj) {
                tr += "<td style='white-space: nowrap; text-align: center;'>" + obj.Name + "</td>";
            });
            tr += "</tr>";
            $("#NutritionValueTbl thead").append(tr);
            tr = "";
            LoadNutrientInformation();
        }
        function OnGetNutritionTypeFailed() {
            toastr.error(error.get_message());
        }

        function LoadNutrientInformation() {
            PageMethods.GetNutrientInformation(OnGetNutrientInformationSucceed, OnGetNutrientInformationFailed);
            return false;
        }
        function OnGetNutrientInformationSucceed(result) {
            nutrientInfoList = result;
            var tr = "<tr>";
            tr += "<td style='text-align: center; white-space: nowrap;'>" + "Ingredient Name</td>";
            $.each(nutritionTypeList, function (ncount, nobj) {
                var td = "";
                td += "<td style='white-space: nowrap; padding-left: 5px;'>";
                $.each(result, function (count, obj) {
                    if (nobj.NutritionTypeId == obj.NutritionTypeId) {
                        td += "<span style='width: 150px; text-align: center; display: inline-block; margin: 0 5px;'>" + obj.Name + "</span>";
                    }
                });
                td += "</td>";
                tr += td;
            });
            tr += "</tr>";
            $("#NutritionValueTbl thead").append(tr);
            td = "";
            tr = "<tr>";
            tr += "<td style='text-align: center; white-space: nowrap;'></td>";
            $.each(nutritionTypeList, function (ncount, nobj) {
                td = "";
                td += "<td style='white-space: nowrap; padding-left: 5px;'>";
                $.each(result, function (count, obj) {
                    if (nobj.NutritionTypeId == obj.NutritionTypeId) {
                        let currentCol = intToString(obj.NutrientId);
                        td += "<span style='width: 150px; text-align: center; display: inline-block; margin: 0 5px;'>" + currentCol + "</span>";
                    }
                });
                td += "</td>";
                tr += td;
            });
            tr += "</tr>";
            $("#NutritionValueTbl thead").append(tr);
            td = "";
            tr = "";
            LoadIngredients();
        }
        function OnGetNutrientInformationFailed() {
            toastr.error(error.get_message());
        }
        function LoadIngredients() {
            PageMethods.GetIngredients(OnGetIngredientsSucceed, OnGetIngredientsFailed);
            return false;
        }
        function OnGetIngredientsSucceed(result) {
            $.each(result, function (count, obj) {
                let newCount = parseInt(count + 1);
                var tr = "";
                tr += "<tr>";
                var td = "";
                td += "<td style='white-space: nowrap;'><span class='valueNo'><span style='width: 50px; text-align: center;'>" + obj.ItemId + "</span><span>" + obj.Name + "</span></span></td>";
                tr += td;
                
                $.each(nutritionTypeList, function (ncount, nobj) {
                    td = "<td style='white-space: nowrap;'>";
                    $.each(nutrientInfoList, function (icount, iobj) {
                        if (nobj.NutritionTypeId == iobj.NutritionTypeId) {
                            let nutrientColumnName = intToString(iobj.NutrientId);
                            td += "<span><input type='text' style='width: 150px; margin: 0 5px;' onclick='getNutrientAmount(" + obj.ItemId + "," + nobj.NutritionTypeId + "," + iobj.NutrientId + ");' id='" + obj.ItemId + nobj.NutritionTypeId + nutrientColumnName + "' readonly /></span>";
                        }
                    });
                    td += "</td>";
                    tr += td;
                });
                tr += "</tr>";
                $("#NutritionValueTbl tbody").append(tr);
                tr = "";
            });
            LoadNutrientAmount();
        }
        function OnGetIngredientsFailed() {

        }

        function LoadNutrientAmount() {
            PageMethods.GetNutrientAmounts(OnGetNutrientAmountsSucceed, OnGetNutrientAmountsFailed);
            return false;
        }
        function OnGetNutrientAmountsSucceed(result) {
            if (result.length > 0) {
                $("#btnSave").val("Update");
            }
            $.each(result, function (count, obj) {
                AddedNutrients.push({
                    ItemId: obj.ItemId,
                    NutritionTypeId: obj.NutritionTypeId,
                    NutrientId: obj.NutrientId,
                    NutrientAmount: obj.NutrientAmount,
                    Formula: obj.Formula
                });
                var boxId = "";
                let nutrientColumnName = intToString(obj.NutrientId);
                boxId = boxId + obj.ItemId + obj.NutritionTypeId + nutrientColumnName;
                formulaValue = parseFloat(obj.NutrientAmount).toFixed(5);
                $("#" + boxId + "").val(formulaValue);
            });
        }
        function OnGetNutrientAmountsFailed() {

        }
        function stringToInt(str) {
            let val;
            if (str.length == 1) {
                val = str.charCodeAt(0) - 64;
            }
            else if (str.length == 2) {
                let tens = str.charCodeAt(0) - 64;
                let ones = str.charCodeAt(1) - 64;
                val = tens * 26 + ones;
            }
            return val;
        }
        function intToString(theInt) {
            let str = "";
            if (theInt <= 26) {
                str += String.fromCharCode(64 + theInt);
            }
            else {
                let tens = Math.floor(theInt / 26);
                let ones = theInt % 26;
                if (ones == 0) {
                    str = String.fromCharCode(63 + tens) + String.fromCharCode(64 + 26);
                }
                else {
                    str += String.fromCharCode(64 + tens);
                    str += String.fromCharCode(64 + ones);
                }
            }
            return str;
        }

        function PerformCancel() {
            let deselectBoxId = $("#ContentPlaceHolder1_hfSelectedBoxId").val();
            $("#" + deselectBoxId + "").css({ color: 'black', border: '1px solid gray' });
            $("#ContentPlaceHolder1_hfSelectedBoxId").val(0);
            $("#ContentPlaceHolder1_hfItemId").val(0);
            $("#ContentPlaceHolder1_hfNutritionTypeId").val(0);
            $("#ContentPlaceHolder1_hfNutrientId").val(0);
            $("#ContentPlaceHolder1_txtFormula").val("");
            $("#ContentPlaceHolder1_txtFormula").attr("disabled", true);
        }
        
        function ValidationBeforeAdd() {
            var formulaOrValue = $("#ContentPlaceHolder1_txtFormula").val();
            let boxId = $("#ContentPlaceHolder1_hfSelectedBoxId").val();
            let itemId = $("#ContentPlaceHolder1_hfItemId").val();
            let nutritionTypeId = $("#ContentPlaceHolder1_hfNutritionTypeId").val();
            let nutrientId = $("#ContentPlaceHolder1_hfNutrientId").val();
            let formula;
            let formulaValue;
            let inputFormula;
            let lengthOfFormulaOrValue = formulaOrValue.length;

            if (formulaOrValue[0] == "=") {
                inputFormula = formulaOrValue.slice(1, formulaOrValue.length);
            }
            if (formulaOrValue == "") {
                formulaValue = 0;
                $("#" + boxId + "").val(formulaOrValue);
            }
            else {
                if (formulaOrValue[0] == "=") {

                    formula = formulaOrValue.slice(1, formulaOrValue.length);
                    let currentVal = "";
                    let currentChar = "";
                    let replacedValue = 0.0;
                    let equation = "";
                    for (let i = 0; i < inputFormula.length; i++) {
                        if (inputFormula.charCodeAt(i) == 37 ||
                            inputFormula.charCodeAt(i) == 40 ||
                            inputFormula.charCodeAt(i) == 41 ||
                            inputFormula.charCodeAt(i) == 42 ||
                            inputFormula.charCodeAt(i) == 43 ||
                            inputFormula.charCodeAt(i) == 45 ||
                            inputFormula.charCodeAt(i) == 47 ||
                            i == inputFormula.length - 1) {
                            if (formulaOrValue.length == 1) {
                                currentVal = inputFormula[i];
                            }
                            let currentNutrientId = stringToInt(currentChar);
                            if (currentChar != "" && i == inputFormula.length - 1 && inputFormula.charCodeAt(i) != 41) {
                                currentVal += inputFormula[i];
                            }
                            let currentItemId = parseInt(currentVal);
                            $.each(AddedNutrients, function (count, obj) {
                                if (obj.NutrientId == currentNutrientId && obj.ItemId == currentItemId) {
                                    replacedValue = obj.NutrientAmount;
                                }
                            });
                            if (currentChar != "" && currentVal != "") {
                                var charVal = currentChar + currentVal;
                                formula = formula.replace(charVal, replacedValue);
                            }
                            replacedValue = 0.0;

                            currentChar = "";
                            currentVal = "";
                        }
                        else {
                            if (inputFormula.charCodeAt(i) >= 65 && inputFormula.charCodeAt(i) <= 90) {
                                currentChar += inputFormula[i];
                            }
                            else if (inputFormula.charCodeAt(i) >= 48 && inputFormula.charCodeAt(i) <= 57) {
                                currentVal += inputFormula[i];
                            }
                        }
                    }
                    try {
                        formulaValue = eval(formula);
                    }
                    catch (err) {
                        toastr.warning("Enter a correct formula.");
                        $("#ContentPlaceHolder1_txtFormula").focus();
                        return false;
                    }
                    if (eval(formula) == "Infinity") {
                        toastr.warning("Infinite Value Is Not Acceptable.");
                        $("#ContentPlaceHolder1_txtFormula").focus();
                        return false;
                    }
                    
                    formulaValue = parseFloat(formulaValue).toFixed(5);
                    $("#" + boxId + "").val(formulaValue);
                }
                else {
                    formulaOrValue = parseFloat(formulaOrValue).toFixed(5);
                    $("#" + boxId + "").val(formulaOrValue);
                    formulaValue = $("#" + boxId + "").val();
                }
            }
            
            itemId = parseInt(itemId);
            nutritionTypeId = parseInt(nutritionTypeId);
            nutrientId = parseInt(nutrientId);
            formulaValue = parseFloat(formulaValue).toFixed(5);

            var isUpdate = false;
            $.each(AddedNutrients, function (count, obj) {
                if (obj.ItemId == itemId && obj.NutritionTypeId == nutritionTypeId && obj.NutrientId == nutrientId) {
                    obj.NutrientAmount = formulaValue;
                    obj.Formula = inputFormula;
                    obj.FormulaWithDecimal = formula;
                    isUpdate = true;
                }
            });

            if (isUpdate != true) {
                AddedNutrients.push({
                    ItemId: itemId,
                    NutritionTypeId: nutritionTypeId,
                    NutrientId: nutrientId,
                    NutrientAmount: formulaValue,
                    Formula: inputFormula,
                    FormulaWithDecimal: formula
                });
            }
            $("#ContentPlaceHolder1_txtFormula").val("");
            $("#ContentPlaceHolder1_txtFormula").attr("disabled", true);
            nutrientId = parseInt(nutrientId);
            let currentColumn = intToString(nutrientId) + itemId;
            changeFieldsWithTheChangedValueOfCurrentField(currentColumn, itemId, nutrientId, formulaValue);
            $("#ContentPlaceHolder1_hfSelectedBoxId").val(0);
            $("#ContentPlaceHolder1_hfItemId").val(0);
            $("#ContentPlaceHolder1_hfNutritionTypeId").val(0);
            $("#ContentPlaceHolder1_hfNutrientId").val(0);
        }
        function changeFieldsWithTheChangedValueOfCurrentField(currentColumn, itemId, nutrientId, formulaValue) {
            $.each(AddedNutrients, function (count, obj) {
                
                if (obj.Formula != undefined && obj.Formula.includes(currentColumn)) {
                    
                    let currentChar = "";
                    let currentVal = "";
                    let replacedValue = 0.0;
                    var formula = obj.Formula;
                    var replacedFormula = obj.Formula;
                    for (let i = 0; i < formula.length; i++) {
                        
                        if (formula.charCodeAt(i) == 37 ||
                            formula.charCodeAt(i) == 40 ||
                            formula.charCodeAt(i) == 41 ||
                            formula.charCodeAt(i) == 42 ||
                            formula.charCodeAt(i) == 43 ||
                            formula.charCodeAt(i) == 45 ||
                            formula.charCodeAt(i) == 47 ||
                            i == formula.length - 1) {
                            let currentNutrientId = stringToInt(currentChar);
                            if (currentChar != "" && i == formula.length - 1 && formula.charCodeAt(i) != 41) {
                                currentVal += formula[i];
                            }
                            let currentItemId = parseInt(currentVal);
                            $.each(AddedNutrients, function (ncount, nobj) {
                                if (nobj.NutrientId == currentNutrientId && nobj.ItemId == currentItemId) {
                                    replacedValue = nobj.NutrientAmount;
                                }
                            });
                            

                            if (currentChar != "" && currentVal != "") {
                                replacedFormula = replacedFormula.replace(currentChar + currentVal, replacedValue);
                            }

                            if (i == formula.length - 1) {
                                let finalValue = eval(replacedFormula);
                                finalValue = parseFloat(finalValue).toFixed(5);
                                let boxId = "";
                                let nutrientColumnName = intToString(obj.NutrientId);
                                boxId = boxId + obj.ItemId + obj.NutritionTypeId + nutrientColumnName;
                                $("#" + boxId + "").val(finalValue);

                                $.each(AddedNutrients, function (changeAmountcount, changeAmountobj) {
                                    if (changeAmountobj.NutrientId == obj.NutrientId && changeAmountobj.ItemId == obj.ItemId) {
                                        changeAmountobj.NutrientAmount = finalValue;
                                        newNutrientId = parseInt(changeAmountobj.NutrientId);
                                        let currentColumnAgain = intToString(newNutrientId) + changeAmountobj.ItemId;
                                        changeFieldsWithTheChangedValueOfCurrentField(currentColumnAgain, changeAmountobj.ItemId, changeAmountobj.NutrientId, changeAmountobj.NutrientAmount);
                                    }
                                });
                            }
                            replacedValue = 0.0;
                            
                            currentChar = "";
                            currentVal = "";
                        }
                        else {
                            if (formula.charCodeAt(i) >= 65 && formula.charCodeAt(i) <= 90) {
                                currentChar += formula[i];
                            }
                            else if ((formula.charCodeAt(i) >= 48 && formula.charCodeAt(i) <= 57) || formula.charCodeAt(i) == 46) {
                                currentVal += formula[i];
                            }
                        }
                    }
                }
            });
        }
        function ClearAfterSave() {
            $("#ContentPlaceHolder1_hfSelectedBoxId").val(0);
            $("#ContentPlaceHolder1_hfItemId").val(0);
            $("#ContentPlaceHolder1_hfNutritionTypeId").val(0);
            $("#ContentPlaceHolder1_hfNutrientId").val(0);

            $("#ContentPlaceHolder1_hfSecondItemId").val(0);
            $("#ContentPlaceHolder1_hfSecondNutritionTypeId").val(0);
            $("#ContentPlaceHolder1_hfSecondNutrientId").val(0);
        }
        function ValidationBeforeSave() {
            if (AddedNutrients.length == 0) {
                toastr.warning("Please Give At Least One Nutrient Amount.");
                return false;
            }
            PageMethods.SaveNutrientsAmount(AddedNutrients, OnSaveNutrientsAmountSucceed, OnSaveNutrientsAmountFailed);
            return false;
        }
        function OnSaveNutrientsAmountSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnSaveNutrientsAmountFailed() {

        }
    </script>
    
    <asp:HiddenField ID="hfSelectedBoxId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfNutritionTypeId" runat="server" Value="0" />
    <asp:HiddenField ID="hfNutrientId" runat="server" Value="0" />

    <asp:HiddenField ID="hfSecondItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSecondNutritionTypeId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSecondNutrientId" runat="server" Value="0" />

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Nutrition Value</a></li>
            <%--<li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Nutrition Value</a></li>--%>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-heading">New Nutrition Value</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFormula" runat="server" class="control-label" Text="Formula"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFormula" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <input id="btnAdd" type="button" value="Update" onclick="ValidationBeforeAdd()"
                                    class="TransactionalButton btn btn-primary btn-sm" />
                                <input id="btnCancel" type="button" value="Cancel" onclick="PerformCancel()"
                                    class="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                        <div class="table-responsive" id="NutritionValue" style="overflow: scroll; max-height: 600px;">
                            <table id="NutritionValueTbl" class="table table-bordered table-condensed table-hover nutritionTable">
                                <thead>
                                </thead>
                                <tbody></tbody>
                                <tfoot></tfoot>
                            </table>
                        </div>
                        <div class="form-group" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <input id="btnSave" type="button" value="Save" onclick="ValidationBeforeSave()"
                                    class="TransactionalButton btn btn-primary btn-sm" />
                                <%--<input id="btnCancelTicket" type="button" value="Cancel" onclick="PerformClearActionWithConfirmation()"
                                    class="TransactionalButton btn btn-primary btn-sm" />--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--<div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Nutrition Value
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchTicketInformation(1, 1)" />
                                <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="ClearSearch()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
    </div>
</asp:Content>