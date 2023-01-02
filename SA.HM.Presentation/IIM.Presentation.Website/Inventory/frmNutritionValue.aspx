<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmNutritionValue.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmNutritionValue" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        
        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            $("#ContentPlaceHolder1_txtFormula").attr("disabled", true);

            
            LoadNutritionType();

            //LoadNutrientInformation();

            //LoadIngredients();

            //LoadNutrientAmount();
                    
            $("#myTabs").tabs();         
        });
        var AddedNutrients = [];
        function getNutrientAmount(itemId, nutritionTypeId, nutrientId) {
            let boxId = "";
            boxId = boxId + itemId + nutritionTypeId + nutrientId;
            $("#ContentPlaceHolder1_hfSelectedBoxId").val(boxId);
            $("#ContentPlaceHolder1_hfItemId").val(itemId);
            $("#ContentPlaceHolder1_hfNutritionTypeId").val(nutritionTypeId);
            $("#ContentPlaceHolder1_hfNutrientId").val(nutrientId);
            var boxCurrentVal = $("#" + boxId + "").val();
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
            // var widthOfNType = 00 / lengthOfResult;
            // style='width:"+ widthOfNType +"px;'
            tr += "<td style='width:200px;'></td>";
            $.each(result, function (count, obj) {
                tr += "<td>" + obj.Name + "</td>";
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
            var tr = "";
            tr += "<td style='width:200px;'>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Ingredient Name</td>";
            $.each(nutritionTypeList, function (ncount, nobj) {
                var td = "";
                td += "<td>";
                $.each(result, function (count, obj) {
                    if (nobj.NutritionTypeId == obj.NutritionTypeId) {
                        td += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + obj.Name;
                    }
                });
                td += "</td>";
                tr += td;
            });
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
            let first10 = result.slice(0, 10);
            
            $.each(first10, function (count, obj) {
                var tr = "";
                tr += "<tr>";
                var td = "";
                td += "<td style='width:200px;'>" + obj.Name + "</td>";
                tr += td;
                
                $.each(nutritionTypeList, function (ncount, nobj) {
                    td = "<td>";
                    $.each(nutrientInfoList, function (icount, iobj) {
                        if (nobj.NutritionTypeId == iobj.NutritionTypeId) {
                            td += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "<input type='text' size='2' onclick='getNutrientAmount(" + obj.ItemId + "," + nobj.NutritionTypeId + "," + iobj.NutrientId + ");' id='" + obj.ItemId + nobj.NutritionTypeId + iobj.NutrientId + "' \>";
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
                boxId = boxId + obj.ItemId + obj.NutritionTypeId + obj.NutrientId;
                formulaValue = parseFloat(obj.NutrientAmount).toFixed(2);
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

        function ValidationBeforeAdd() {
            var formulaOrValue = $("#ContentPlaceHolder1_txtFormula").val();
            let boxId = $("#ContentPlaceHolder1_hfSelectedBoxId").val();
            let itemId = $("#ContentPlaceHolder1_hfItemId").val();
            let nutritionTypeId = $("#ContentPlaceHolder1_hfNutritionTypeId").val();
            let nutrientId = $("#ContentPlaceHolder1_hfNutrientId").val();
            let formula;
            let formulaValue;
            let inputFormula;
            if(formulaOrValue[0] == "="){
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
                            if (currentChar != "" && i == formula.length - 1) {
                                currentVal += formula[i];
                            }
                            let currentItemId = parseInt(currentVal);
                            $.each(AddedNutrients, function (count, obj) {
                                if (obj.NutrientId == currentNutrientId && obj.ItemId == currentItemId) {
                                    replacedValue = obj.NutrientAmount;
                                }
                            });
                            if (replacedValue > 0) {
                                formula = formula.replace(currentChar + currentVal, replacedValue);
                            }
                            replacedValue = 0.0;

                            currentChar = "";
                            currentVal = "";
                        }
                        else {
                            if (formula.charCodeAt(i) >= 65 && formula.charCodeAt(i) <= 90) {
                                currentChar += formula[i];
                            }
                            else if (formula.charCodeAt(i) >= 48 && formula.charCodeAt(i) <= 57) {
                                currentVal += formula[i];
                            }
                        }
                    }
                    formulaValue = eval(formula);
                    formulaValue = parseFloat(formulaValue).toFixed(2);
                    $("#" + boxId + "").val(formulaValue);
                }
                else {
                    formulaOrValue = parseFloat(formulaOrValue).toFixed(2);
                    $("#" + boxId + "").val(formulaOrValue);
                    formulaValue = $("#" + boxId + "").val();
                    formulaValue = parseFloat(formulaValue).toFixed(2);
                }
            }
            
            
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
        }
        function changeFieldsWithTheChangedValueOfCurrentField(currentColumn, itemId, nutrientId, formulaValue) {
            
            $.each(AddedNutrients, function (count, obj) {
                if (obj.Formula.includes(currentColumn)) {
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
                            if (currentChar != "" && i == formula.length - 1) {
                                currentVal += formula[i];
                            }
                            let currentItemId = parseInt(currentVal);
                            $.each(AddedNutrients, function (ncount, nobj) {
                                if (nobj.NutrientId == currentNutrientId && nobj.ItemId == currentItemId) {
                                    replacedValue = nobj.NutrientAmount;
                                }
                            });
                            if (replacedValue > 0) {
                                replacedFormula = replacedFormula.replace(currentChar + currentVal, replacedValue);
                            }

                            if (i == formula.length - 1) {
                                let finalValue = eval(replacedFormula);
                                finalValue = parseFloat(finalValue).toFixed(2);
                                let boxId = "";
                                boxId = boxId + obj.ItemId + obj.NutritionTypeId + obj.NutrientId;
                                $("#" + boxId + "").val(finalValue);
                            }
                            
                            replacedValue = 0.0;

                            currentChar = "";
                            currentVal = "";
                        }
                        else {
                            if (formula.charCodeAt(i) >= 65 && formula.charCodeAt(i) <= 90) {
                                currentChar += formula[i];
                            }
                            else if (formula.charCodeAt(i) >= 48 && formula.charCodeAt(i) <= 57) {
                                currentVal += formula[i];
                            }
                        }
                    }
                }
            });
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
                //$("#btnSave").val("Save");
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
                            </div>
                        </div>
                        <div class="table-responsive" id="NutritionValue">
                            <table id="NutritionValueTbl" class="table table-bordered table-condensed table-hover">
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