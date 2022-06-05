<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmInvItem.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmInvItem" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var deleteItemObj = [];
        var x = 0;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        ////Bread Crumbs Information-------------
        $(document).ready(function () {

            //debugger;
            var IsItemOriginHide = $("#ContentPlaceHolder1_hfIsItemOriginHide").val();

            if (IsItemOriginHide == "1") {
                $('#originDiv').hide();
            }
            else {
                $('#originDiv').show();
            }

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Item Information</li>";
            var breadCrumbs = moduleName + formName;
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_ddlOrigin").select2({
                tags: false,
                allowClear: true,
                placeholder: "",
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlManufacturer").select2({
                tags: false,
                allowClear: true,
                placeholder: "",
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlAttributeName").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSetupType").change(function () {
                var setupTypeId = $("#ContentPlaceHolder1_ddlSetupType").val();
                GetAttributeName(setupTypeId);
            });

            if ($("#InnboardMessageHiddenField").val() != "") {
                var alertMessage = JSON.parse($("#InnboardMessageHiddenField").val());

                CommonHelper.AlertMessage(alertMessage);
                $("#InnboardMessageHiddenField").val("");

                if (alertMessage.IsSuccess == 1) {
                    document.forms[0].reset();
                }
            }
            $('#ImagePanel').hide();
            if ($("#ContentPlaceHolder1_hfEditedItemId").val() != "") {
                debugger;
                UploadComplete();
            }

            if ($("#ContentPlaceHolder1_ddlIsCustomerItem").val() == "0") {
                $("#ItemTypeDiv").hide();
            }
            else {
                $("#ItemTypeDiv").show();
            }

            if ($("#ContentPlaceHolder1_ddlIsSupplierItem").val() == "1") {
                $("#SupplierDiv").show();
            }
            else {
                $("#SupplierDiv").hide();
            }

            if ($("#ContentPlaceHolder1_ddlIsAttributeItem").val() == "1") {
                $("#AttributeDiv").show();
            }
            else {
                $("#AttributeDiv").hide();
            }

            $("#myTabs").tabs();
            $("#childTabs").tabs();

            $("#ContentPlaceHolder1_ddlCategoryId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });


            $("#ContentPlaceHolder1_ddlPCategoryId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            var ddlStockType = '<%=ddlStockType.ClientID%>'
            if ($('#' + ddlStockType).val() == "KitchenItem") {
                $('#StockItemInformationDiv').hide("slow");
                $('#StockItemCostCenterInformationDiv').hide("slow");
                $('#KitchenItemCostCenterInformationDiv').show("slow");
            }
            else if ($('#' + ddlStockType).val() == "IngredientText") {
                $('#StockItemInformationDiv').hide("slow");
                $('#StockItemCostCenterInformationDiv').hide("slow");
                $('#KitchenItemCostCenterInformationDiv').show("slow");
            }
            else {
                $('#StockItemInformationDiv').show("slow");
                $('#StockItemCostCenterInformationDiv').show("slow");
                $('#KitchenItemCostCenterInformationDiv').hide("slow");
            }

            $("#ContentPlaceHolder1_ddlIsSupplierItem").change(function () {
                if ($(this).val() == "1") {
                    $("#SupplierDiv").show();
                }
                else {
                    $("#SupplierDiv").hide();
                }
            });
            $("#ContentPlaceHolder1_ddlIsAttributeItem").change(function () {
                if ($(this).val() == "1") {
                    $("#AttributeDiv").show();
                }
                else {
                    $("#AttributeDiv").hide();
                }
            });

            var RecipeDetailText = '<%=RecipeDetailText.ClientID%>'
            $('#' + RecipeDetailText).text($("#ContentPlaceHolder1_hfIngredientsText").val());

            var ddlStockType = '<%=ddlStockType.ClientID%>'
            var ddlIsCustomerItemVal = '<%=ddlIsCustomerItem.ClientID%>'

            $('#' + ddlStockType).change(function () {
                if ($('#' + ddlStockType).val() == "KitchenItem") {
                    $('#StockItemInformationDiv').hide("slow");
                    $('#StockItemCostCenterInformationDiv').hide("slow");
                    $('#KitchenItemCostCenterInformationDiv').show("slow");

                    if ($('#' + ddlIsCustomerItemVal).val() == "0") {
                        $('#ItemTypeDiv').hide();
                    }
                    else {
                        $('#ItemTypeDiv').show();
                    }
                }
                else if ($('#' + ddlStockType).val() == "IngredientText") {
                    $('#StockItemInformationDiv').hide("slow");
                    $('#StockItemCostCenterInformationDiv').hide("slow");
                    $('#KitchenItemCostCenterInformationDiv').show("slow");

                    if ($('#' + ddlIsCustomerItemVal).val() == "0") {
                        $('#ItemTypeDiv').hide();
                    }
                    else {
                        $('#ItemTypeDiv').show();
                    }
                }
                else {
                    $('#StockItemInformationDiv').show("slow");
                    $('#StockItemCostCenterInformationDiv').show("slow");
                    $('#KitchenItemCostCenterInformationDiv').hide("slow");
                    $('#ItemTypeDiv').hide();
                }
            });

            $('#' + ddlIsCustomerItemVal).change(function () {
                if ($('#' + ddlIsCustomerItemVal).val() == "0") {
                    $('#ItemTypeDiv').hide();
                }
                else {
                    $('#ItemTypeDiv').show();
                }
            });

            var ddlItemType = '<%=ddlItemType.ClientID%>'
            $('#' + ddlItemType).change(function () {
                var RecipeDetailText = '<%=RecipeDetailText.ClientID%>'
                if ($('#' + ddlItemType).val() == "IndividualItem") {
                    $('#' + RecipeDetailText).text($("#ContentPlaceHolder1_hfIngredientsText").val());
                }
                else if ($('#' + ddlItemType).val() == "ComboItem") {
                    $('#' + RecipeDetailText).text($("#ContentPlaceHolder1_hfIngredientsText").val());
                }
                else if ($('#' + ddlItemType).val() == "BuffetItem") {
                    $('#' + RecipeDetailText).text($("#ContentPlaceHolder1_hfIngredientsText").val());
                }
            });

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#gvInvItemInformation").delegate("td > img.InvitemDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var itemId = $.trim($(this).parent().parent().find("td:eq(4)").text());
                    var params = JSON.stringify({ sEmpId: itemId });

                    var $row = $(this).parent().parent();
                    $.ajax({
                        type: "POST",
                        url: "/Inventory/frmInvItem.aspx/DeleteInvItem",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $row.remove();
                            toastr.info('Item Deleted Successfully');
                            $("#myTabs").tabs('load', 1);
                        },
                        error: function (error) {
                        }
                    });
                }
            });

            $("#btnAddItem").click(function () {
                var itemName = txtItemName.value;
                var itemId = $.trim($("#ContentPlaceHolder1_hfItemId").val());
                var productId = $("#ContentPlaceHolder1_txtProductId").val();
                var itemCost = 0;

                var itemUnit = "";
                var unitHeadId = "";

                if (itemId == "") {
                    toastr.warning("Please select item.");
                    return false;
                }
                else if ($("#<%=ddlItemStockBy.ClientID %>").val() == "0") {
                    toastr.warning("Please select item stock by.");
                    return false;
                }
                else if ($("#<%=txtItemUnit.ClientID %>").val() == "") {
                    toastr.warning("Please give item unit.");
                    return false;
                }

                unitHeadId = $("#<%=ddlItemStockBy.ClientID %>").val();
                itemUnit = $("#<%=txtItemUnit.ClientID %>").val();

                var duplicateCheck = false;

                $('#RecipeItemInformation tbody > tr > td:nth-child(2)').filter(function (index) {
                    if (parseInt($.trim($(this).text()), 10) === parseInt(itemId, 10))
                        duplicateCheck = true;
                });

                if (itemId != "") {

                    if (duplicateCheck == false) {

                        GetItemCost(itemId, unitHeadId, itemUnit).done(function (response) {

                            itemCost = $("#ContentPlaceHolder1_hfItemCost").val();

                            AddNewItem(itemName, itemCost, itemId, 0, productId);
                            GetTotalReceipeCost();

                            $("#<%=hfItemId.ClientID %>").val('');
                            $("#txtItemName").val('');
                            $("#<%=ddlItemStockBy.ClientID %>").val("0");
                            $("#ContentPlaceHolder1_txtItemUnit").val("");

                            $("#txtItemName").focus();
                        });
                    }
                    else {
                        toastr.warning('Duplicate Item');
                        return;
                    }
                }
                else {
                    toastr.warning('Item not found');
                    $("#hfItemId").val('');
                    return;
                }
            });

            $("#txtItemName").autocomplete({
                source: function (request, response) {
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    var isCustomerItem = $("#ContentPlaceHolder1_ddlIsCustomerItem").val();
                    var itemtype = $("#ContentPlaceHolder1_ddlItemType").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../Inventory/frmInvItem.aspx/ItemNCategoryAutoSearch',
                        data: "{'itemName':'" + request.term + "','categoryId':'" + categoryId + "','isCustomerItem':'" + isCustomerItem + "', 'itemType':'" + itemtype + "'}",
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

                    if ($.trim($("#ContentPlaceHolder1_hfEditedItemId").val()) != "") {

                        if ($.trim($("#ContentPlaceHolder1_hfEditedItemId").val()) == ui.item.value) {
                            toastr.info("Same Item Cannot be Added as Recipe.");
                            return;
                        }
                    }

                    $(this).val(ui.item.label);
                    $("#<%=hfItemId.ClientID %>").val(ui.item.value);
                    PageMethods.LoadRelatedStockBy(ui.item.StockById, OnLoadStockBySucceeded, OnLoadStockByFailed);
                }
            });

            var EditId = $("#ContentPlaceHolder1_hfEditedItemId").val();
            PageMethods.GetAttributeByWebMethod(EditId, OnGetAttributeByWebMethodSucceeded, OnGetAttributeByWebMethodFailed);

            var isReceipeInclude = $("#<%=hfIsRecipeIncludedInInventory.ClientID %>").val();

            if (isReceipeInclude == 1) {
                $($("#childTabs").find("li")[1]).show();
            }
            else {
                $($("#childTabs").find("li")[1]).hide();
            }

            $("[id=ContentPlaceHolder1_gvSupplierInfo_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvSupplierInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvSupplierInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

        });

        function OnGetAttributeByWebMethodSucceeded(result) {
            console.log(result);
            console.log(attributeIds);
            var removeSetupType;
            $.each(result, function (key, value) {
                attributeIds.push('' + value.Id);
                if (value.SetupType == 'Color') {
                    setupTypeIds.push('1');
                    removeSetupType = '1';
                } else if (value.SetupType == 'Size') {
                    setupTypeIds.push('2');
                    removeSetupType = '2';
                } else {
                    setupTypeIds.push('3');
                    removeSetupType = '3';
                }
                var randomNumber = Math.floor((Math.random() * 9999999999) + 1);
                $('#tbody').append(`<tr id="${randomNumber}">
                  <td class="row-index text-left">
                        <p>${value.SetupType}</p></td>
                   <td class ="row-index text-left">
                        <p>${value.Name}</p></td>
                   <td class="text-center">
                    <button onclick="btnRemove_Click('${randomNumber}','${value.Id}','${removeSetupType}')" style="border: none; background-color: transparent"
                        type="button"><img alt="Delete" src="../Images/delete.png" /></button>
                    </td>
                   </tr>`);
            });

        }

        function OnGetAttributeByWebMethodFailed(error) {

        }

        function OnLoadStockBySucceeded(result) {
            var list = result;
            var ddlStockById = '<%=ddlItemStockBy.ClientID%>';
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

        function AddNewItem(itemName, itemCost, itemId, id, productId) {
            var itemUnit = $("#<%=txtItemUnit.ClientID %>").val();
            var unitHeadId = $("#<%=ddlItemStockBy.ClientID %>").val();
            var unitHeadName = $("#ContentPlaceHolder1_ddlItemStockBy option:selected").text();

            if ($("#ltlTableWiseItemInformation > table").length > 0 && productId == 0) {
                AddNewRow(itemName, itemId, id, unitHeadId, unitHeadName, itemUnit, itemCost);
                return false;
            }
            else if ($("#ltlTableWiseItemInformation > table").length > 0 && productId > 0) {
                AddNewRow(itemName, itemId, id, unitHeadId, unitHeadName, itemUnit, itemCost);
                return false;
            }

            var table = "", deleteLink = "";

            deleteLink = "<a href=\"javascript:void();\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            table += "<table cellspacing='0' cellpadding='4' id='RecipeItemInformation' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            table += "<th style='display:none'></th><th style='display:none'></th><th align='left' scope='col' style='width: 25%;'>Item Name</th>";
            table += "<th align='left' scope='col' style='width: 12%;'>Stock By</th> <th align='left' scope='col' style='width: 12%;'>Quantity</th>";
            table += "<th align='left' scope='col' style='width: 15%;'>Cost</th><th align='center' scope='col' style='width: 26%;'>Is Gradient Can Change?</th>";
            table += "<th style='display:none'></th><th align='center' scope='col' style='width: 10%;'>Action</th></tr></thead>";

            table += "<tbody>";
            table += "<tr style=\"background-color:#E3EAEB;\">";

            table += "<td align='left' style=\"display:none;\">" + id + "</td>";
            table += "<td align='left' style=\"display:none;\">" + itemId + "</td>";
            table += "<td align='left' style=\"width:25%; text-align:Left;\">" + itemName + "</td>";
            table += "<td align='left' style=\"width:12%; text-align:Left;\">" + unitHeadName + "</td>";
            table += "<td align='left' style=\"width:12%; text-align:Left;\">" + itemUnit + "</td>";
            table += "<td align='left' style='width: 15%;'>" + itemCost + "</td>";
            table += "<td align='center' style='width: 26%;'>" + "<input type='checkbox' />" + "</td>";
            table += "<td align='left' style=\"display:none;\">" + unitHeadId + "</td>";
            table += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            table += "</tr>";
            table += "</tbody>";
            table += "</table>";

            $("#ltlTableWiseItemInformation").html(table);
        }
        function AddNewRow(itemName, itemId, id, unitHeadId, unitHeadName, itemUnit, itemCost) {
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
            tr += "<td align='left' style=\"display:none;\">" + itemId + "</td>";
            tr += "<td align='left' style=\"width:25%; text-align:Left;\">" + itemName + "</td>";
            tr += "<td align='left' style=\"width:12%; text-align:Left;\">" + unitHeadName + "</td>";
            tr += "<td align='left' style=\"width:12%; text-align:Left;\">" + itemUnit + "</td>";
            tr += "<td align='left' style='width: 15%;'>" + itemCost + "</td>";
            tr += "<td align='center' style='width: 26%;'>" + "<input type='checkbox' />" + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + unitHeadId + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#RecipeItemInformation tbody").append(tr);
        }

        var ff = [];

        function DeleteItem(anchor) {
            ff = anchor;
            var tr = $(anchor).parent().parent();

            var id = $.trim($(tr).find("td:eq(0)").text());
            var itemId = $.trim($(tr).find("td:eq(1)").text());

            if (parseInt(id, 10) != 0) {
                deleteItemObj.push({
                    RecipeId: id,
                    RecipeItemId: itemId
                });
            }

            $(tr).remove();

            GetTotalReceipeCost();
            return false;
        }

        function ValidationNPreprocess() {
            console.log(attributeIds);
            console.log(setupTypeIds);
            var attributeIdList = "";
            for (let i = 0; i < attributeIds.length; i++) {
                if (attributeIdList == "") {
                    attributeIdList = attributeIdList + attributeIds[i];
                } else {
                    attributeIdList = attributeIdList + "," + attributeIds[i];
                }
            }
            console.log(attributeIdList);

            if ($("#<%=ddlIsAttributeItem.ClientID %>").val() == "1") {
                if (setupTypeIds.indexOf('1') == -1) {
                    toastr.warning('Please Select a Color.');
                    return false;
                } else if (setupTypeIds.indexOf('2') == -1) {
                    toastr.warning('Please Select a Size');
                    return false;
                } else if (setupTypeIds.indexOf('3') == -1) {
                    toastr.warning('Please Select a Style');
                    return false;
                }

            }

            if ($("#<%=ddlCategoryId.ClientID %>").val() == "0") {
                toastr.warning('Please Select Category.');
                return false;
            }
            else if ($("#<%=txtName.ClientID %>").val() == "") {
                toastr.warning('Please Provide Item Name.');
                return false;
            }
            else if ($("#<%=txtCode.ClientID %>").val() == "") {
                var isInvItemCodeAutoGenerate = $("#<%=hfIsInvItemCodeAutoGenerate.ClientID %>").val();
                if (isInvItemCodeAutoGenerate == 0) {
                    toastr.warning('Please Provide Code Name.');
                    return false;
                }
            }
            else if ($("#<%=ddlStockType.ClientID %>").val() == "0") {
                toastr.warning('Please Select Stock Type.');
                return false;
            }
    if ($("#<%=ddlClassification.ClientID %>").val() == "0") {
                toastr.warning('Please Select Item Classification.');
                return false;
            }
            else if ($("#<%=ddlCategory.ClientID %>").val() != "0") {
                if ($("#<%=ddlStockType.ClientID %>").val() == "KitchenItem") {
                    if (rowLength == 0 && saveObj.length == 0 && deleteSalesObj.length == 0) {
                        toastr.warning('Please add at least one recipe item.');
                        return false;
                    }
                }
            }
            else if ($("#ContentPlaceHolder1_ddlIsSupplierItem").val() == "1") {
                if ($("#ContentPlaceHolder1_ddlSupplier").val() == "0") {
                    toastr.warning("Please Select a Supplier");
                    return false;
                }
            }

        if ($("#<%=ddlIsCustomerItem.ClientID %>").val() == "-1") {
                toastr.warning('Please Select Is Customer Item.');
                return false;
            }

            if ($("#<%=ddlIsSupplierItem.ClientID %>").val() == "-1") {
                toastr.warning('Please Select Is Supplier Item.');
                return false;
            }
            else if ($("#<%=ddlIsSupplierItem.ClientID %>").val() == "1") {

                var isChecked = false;
                isChecked = $("#ContentPlaceHolder1_gvSupplierInfo tbody tr").find("td:eq(0) > span").find("input").is(":checked");
                if (!isChecked) {
                    toastr.warning('Please Select a Supplier Item.');
                    return false;
                }
            }
            if ($("#<%=ddlStockBy.ClientID %>").val() == "0") {
                toastr.warning('Please Select Inventory Unit.');
                return false;
            }
            if ($("#<%=ddlSalesStockBy.ClientID %>").val() == "0") {
                toastr.warning('Please Select Sales Unit.');
                return false;
            }

            var costCenterName = "";

            if ($("#ContentPlaceHolder1_ddlStockType").val() == "KitchenItem") {

                var length = $("#ContentPlaceHolder1_gvKitchenItemCostCenterInfo tbody > tr").length;
                var i = 0;

                for (i = 1; i < length; i++) {

                    if ($("#ContentPlaceHolder1_gvKitchenItemCostCenterInfo tbody > tr:eq(" + i + ")").find("td:eq(0)").find("input").is(":checked")) {

                        var vv = $("#ContentPlaceHolder1_gvKitchenItemCostCenterInfo tbody > tr:eq(" + i + ")").find("td:eq(2)").find("select").val();

                        if (vv == null || vv == "0") {
                            costCenterName = $("#ContentPlaceHolder1_gvKitchenItemCostCenterInfo tbody > tr:eq(" + i + ")").find("td:eq(1)").text();
                            break;
                        }
                    }
                }

                if (i < length) {
                    toastr.info("Please Select Kitchen for Cost Center  '" + costCenterName + "'");
                    return false;
                }
            }

            var saveItemObj = [];
            var id = 0, itemId = 0, itemName = '', unitHeadId = 0, itemUnit = 0, itemCost = 0;
            var IsGradientCanChange;

            var rowLength = $("#ltlTableWiseItemInformation > table tbody tr").length;

            $("#ltlTableWiseItemInformation > table tbody tr").each(function () {

                id = parseInt($.trim($(this).find("td:eq(0)").text(), 10));
                itemId = parseInt($.trim($(this).find("td:eq(1)").text(), 10));
                itemName = $.trim($(this).find("td:eq(2)").text());
                itemUnit = $.trim($(this).find("td:eq(4)").text());
                itemCost = $.trim($(this).find("td:eq(5)").text());
                unitHeadId = parseInt($.trim($(this).find("td:eq(7)").text(), 10));
                IsGradientCanChange = $.trim($(this).find("td:eq(6)").find("input").is(":checked"));


                saveItemObj.push({
                    RecipeId: id,
                    RecipeItemId: itemId,
                    RecipeItemName: itemName,
                    UnitHeadId: unitHeadId,
                    ItemUnit: itemUnit,
                    ItemCost: itemCost,
                    IsGradientCanChange: IsGradientCanChange
                });

            });

            if (rowLength > 0)
                $("#<%=hfIsReceipeExist.ClientID %>").val("1");
            else
                $("#<%=hfIsReceipeExist.ClientID %>").val("0");

            $("#<%=hfSaveObj.ClientID %>").val(JSON.stringify(saveItemObj));
            $("#<%=hfDeleteObj.ClientID %>").val(JSON.stringify(deleteItemObj));
            var attributeList = new Array();
            var colortblLength = $("#Colortbl tbody > tr").length;
            var sizetblLength = $("#Sizetbl tbody > tr").length;
            var styletblLength = $("#Styletbl tbody > tr").length;
            for (var i = 1; i < colortblLength; i++) {

                if ($("#Colortbl tbody > tr:eq(" + i + ")").find("td:eq(0)").find("input").is(":checked")) {

                    attributeList.push($("#Colortbl tbody > tr:eq(" + i + ")").find("td:eq(2)").text());
                }
            }
            for (var i = 1; i < sizetblLength; i++) {

                if ($("#Sizetbl tbody > tr:eq(" + i + ")").find("td:eq(0)").find("input").is(":checked")) {

                    attributeList.push($("#Sizetbl tbody > tr:eq(" + i + ")").find("td:eq(2)").text());
                }
            }
            for (var i = 1; i < styletblLength; i++) {

                if ($("#Styletbl tbody > tr:eq(" + i + ")").find("td:eq(0)").find("input").is(":checked")) {

                    attributeList.push($("#Styletbl tbody > tr:eq(" + i + ")").find("td:eq(2)").text());
                }
            }

            //console.log(attributeIdList);

            $("#ContentPlaceHolder1_hfItemAttributeList").val(attributeIdList);
            // $("#ContentPlaceHolder1_RandomProductId").val($("#ContentPlaceHolder1_hfDeletedDocId").val());
        }

        function GetTotalReceipeCost() {

            var totalCost = 0, itemCost = "";

            $("#ltlTableWiseItemInformation > table tbody tr").each(function () {
                itemCost = $.trim($(this).find("td:eq(5)").text());
                totalCost += parseFloat(itemCost);
            });

            totalCost = totalCost.toFixed(2);

            $("#lblTotalReceipeCost").text(totalCost);
            $("#ContentPlaceHolder1_txtPurchasePrice").val(totalCost);
        }

        function GetItemCost(itemId, stockById, quantity) {

            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmInvItem.aspx/GetReceipeItemCost',
                data: "{'itemId':'" + itemId + "','stockById':'" + stockById + "','quantity':'" + quantity + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d != null) {
                        $("#ContentPlaceHolder1_hfItemCost").val(data.d);
                    }
                    else {
                        $("#ContentPlaceHolder1_hfItemCost").val("0");
                    }
                },
                error: function (result) {
                    toastr.error("Please Contact With Admin");
                }
            });
        }

        function GetAttributeName(setupTypeId) {

            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Inventory/frmInvItem.aspx/GetAttributeName',
                data: "{'setupTypeId':'" + setupTypeId + "'}",
                dataType: "json",
                success: function (data) {
                    $('#ContentPlaceHolder1_ddlAttributeName').empty();
                    for (var i = 0; i < data.d.length; i++) {
                        $('#ContentPlaceHolder1_ddlAttributeName').append(new Option(data.d[i].Name, data.d[i].Id));
                    }
                    //if (data.d != null) {
                    //    $("#ContentPlaceHolder1_hfItemCost").val(data.d);
                    //}
                    //else {
                    //    $("#ContentPlaceHolder1_hfItemCost").val("0");
                    //}
                },
                error: function (result) {
                    toastr.error("Please Contact With Admin");
                }
            });
        }

        function LoadImageUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var path = "/Inventory/Images/Product/";
            var category = "InventoryProduct";
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

        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }

        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomProductId").val();
            ShowUploadedDocument(randomId);
        }


        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_txtProductId").val();
            if (id == "") {
                id = "0";
            }

            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.GetUploadedDocByWebMethod(randomId, id, deletedDoc, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
        }

        function OnGetUploadedDocByWebMethodSucceeded(result) {
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
                DocTable += "<td align='left' style='width: 50%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + imagePath + "</td>";

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

        function ShowDocument(path, name) {
            var iframeid = 'fileIframe';
            document.getElementById(iframeid).src = path;
            $("#ShowDocumentDiv").dialog({
                autoOpen: true,
                modal: true,
                width: "82%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Document - " + name,
                show: 'slide'
            });
            return false;
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=ddlCategoryId.ClientID %>").val(0);
            $("#<%=ddlManufacturer.ClientID %>").val(0);
            $("#<%=ddlProductType.ClientID %>").val(0);
            $("#<%=txtPurchasePrice.ClientID %>").val('');
            $("#<%=ddlSellingPriceLocal.ClientID %>").val(45);
            $("#<%=txtSellingPriceLocal.ClientID %>").val('');
            $("#<%=ddlSellingPriceUsd.ClientID %>").val(46);
            $("#<%=txtSellingPriceUsd.ClientID %>").val('');
            $("#<%=txtDescription.ClientID %>").val('');
            $("#<%=txtCode.ClientID %>").val('');
            $("#<%=hfItemCode.ClientID %>").val('');
            $("#<%=txtName.ClientID %>").val("");
            $("#<%=txtProductId.ClientID %>").val("0");
            $("#<%=ddlServiceWarranty.ClientID %>").val(0);
            $("#<%=btnSave.ClientID %>").val("Save");
            MessagePanelHide();
            return false;
        }


        //For Delete-------------------------        


        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewProduct').hide("slow");

            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewProduct').show("slow");

            PerformClearAction();
            return false;
        }
        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewProduct').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewProduct').hide("slow");
        }


        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvInvItemInformation tbody tr").length;

            var itemName = $("#<%=txtPName.ClientID %>").val();
            var displayName = $("#<%=txtSDisplayName.ClientID %>").val();
            var itemCode = $("#<%=txtPCode.ClientID %>").val();
            var categoryId = $("#<%=ddlPCategoryId.ClientID %>").val();
            var classification = $("#<%=ddlSClassification.ClientID %>").val();
            var gridOrderBy = $("#<%=ddlSrcOrderBy.ClientID %>").val();

            PageMethods.SearchInvItemAndLoadGridInformation(gridOrderBy, itemName, displayName, itemCode, categoryId, classification, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvInvItemInformation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvInvItemInformation tbody ").append(emptyTr);
                return false;
            }
            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvInvItemInformation tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:40%; cursor:pointer;\">" + gridObject.Name + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.Code + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.Model + "</td>";
                if (IsCanEdit) {
                    tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.ItemId + "')\" alt='Edit Information' border='0' /></td>";
                    //tr += "<td align='right' style=\"width:8%; cursor:pointer;\"><img src='../Images/delete.png' class= 'InvitemDelete'  alt='Delete Information' border='0' /></td>";

                }
                else {
                    tr += "<td align='right'></td>";
                }
                tr += "<td align='right' style=\"width:8%; display:none;\">" + gridObject.ItemId + "</td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><i class=\"glyphicon glyphicon-barcode\" style='font-size:20px;' onClick= \"javascript:return PrintBarCodeById('" + gridObject.ItemId + "')\"></i></td>";

                tr += "</tr>"

                $("#gvInvItemInformation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.error("Please Contact With Admin");
        }
        function PerformEditAction(itemId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }

            $("#ContentPlaceHolder1_hfEditedItemId").val(itemId);
            $("#btnEditInServer").trigger("click");
        }
        function OnEditIdSaveSucceeded(result) {
            window.location = "frmInvItem.aspx";
        }
        function OnEditIdSaveFailed() { }

        function btnRemove_Click(row, newAttributeId, removeSetupType) {
            $('#' + row).remove();
            var id = attributeIds.indexOf(newAttributeId);
            var sId = setupTypeIds.indexOf(removeSetupType);
            setupTypeIds.splice(sId, 1);
            //console.log(id);
            attributeIds.splice(id, 1);
            console.log(attributeIds);
        }

        var attributeIds = [];
        var setupTypeIds = [];
        function btnAddAttribute_Click() {
            //var addAttributeId = $("#ContentPlaceHolder1_hfAddAttribute").val();
            var attributeName = $("#ContentPlaceHolder1_ddlAttributeName :selected").text();
            var setupTypeId = $("#ContentPlaceHolder1_ddlSetupType").val();
            var setup = $("#ContentPlaceHolder1_ddlSetupType :selected").text();
            var newAttributeId = $("#ContentPlaceHolder1_ddlAttributeName").val();
            if (attributeIds.indexOf(newAttributeId) == -1 && newAttributeId != null) {
                setupTypeIds.push(setupTypeId);
                attributeIds.push(newAttributeId);
                console.log(attributeIds);
                console.log(setupTypeIds);
                //if (addAttributeId == "") {
                //    $("#ContentPlaceHolder1_hfAddAttribute").val(addAttributeId + newAttributeId);
                //} else {
                //    $("#ContentPlaceHolder1_hfAddAttribute").val(addAttributeId + "," + newAttributeId);
                //}
                var randomNumber = Math.floor((Math.random() * 9999999999) + 1);
                $('#tbody').append(`<tr id="${randomNumber}">
                  <td class="row-index text-left">
                        <p>${setup}</p></td>
                   <td class ="row-index text-left">
                        <p>${attributeName}</p></td>
                   <td class="text-center">
                    <button onclick="btnRemove_Click('${randomNumber}','${newAttributeId}','${setupTypeId}')" style="border: none; background-color: transparent"
                        type="button"><img alt="Delete" src="../Images/delete.png" /></button>
                    </td>
                   </tr>`);
                //console.log($("#ContentPlaceHolder1_hfAddAttribute").val());
            } else {
                if (newAttributeId == null) {
                    alert("Please Select an Attribute");
                } else {
                    alert("Already Added");
                }
            }

        }

        //        function LoadRecipeItem(itemId) {
        //            PageMethods.LoadRecipeItem(itemId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
        //            return false;
        //        }
        //        function OnLoadDetailObjectSucceeded(result) {
        //            var itemLength = 0, row = 0;
        //            itemLength = result.length;

        //            $("#ItemList tbody tr").remove();

        //            for (row = 0; row < itemLength; row++) {
        //                AddNewItem(result[row].RecipeItemName, result[row].RecipeItemId, result[row].RecipeId);
        //            }
        //            return false;
        //        }
        //        function OnLoadDetailObjectFailed(error) {
        //            toastr.error(error.get_message());
        //        }
        //        function DeleteRecipeItem(recipeId) {

        //            var answer = confirm("Do you want to delete this record?")
        //            if (answer) {
        //                PageMethods.DeleteRecipeItem(recipeId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
        //            }
        //            return false;
        //        }
        //        function OnDeleteObjectSucceeded(result) {
        //            CommonHelper.AlertMessage(result.AlertMessage);            
        //            var tr = $("#RecipeItemInformation tbody tr");
        //            $(tr).remove();             
        //        }
        //        function OnDeleteObjectFailed(error) {            
        //            toastr.error(error);
        //        }

        $(document).ready(function () {

            $("[id=ContentPlaceHolder1_gvKitchenItemCostCenterInfo_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvKitchenItemCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvKitchenItemCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

            $("[id=ContentPlaceHolder1_gvStockItemCostCenterInfo_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvStockItemCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvStockItemCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });
            $("[id=CheckAllSize]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#Sizetbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#Sizetbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
                }
            });
            $("[id=CheckAllColor]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#Colortbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#Colortbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
                }
            });
            $("[id=CheckAllStyle]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#Styletbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#Styletbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
                }
            });
            debugger;
            var str = $("#ContentPlaceHolder1_hfItemAttributeList").val();
            var array = new Array();
            $.each(str.split(","), function () {
                array.push($.trim(this));
            });

            var colortblLength = $("#Colortbl tbody > tr").length;
            var sizetblLength = $("#Sizetbl tbody > tr").length;
            var styletblLength = $("#Styletbl tbody > tr").length;
            for (var i = 1; i < colortblLength; i++) {
                var a = ($("#Colortbl tbody > tr:eq(" + i + ")").find("td:eq(2)").text()).trim();
                if (jQuery.inArray((($("#Colortbl tbody > tr:eq(" + i + ")").find("td:eq(2)").text()).trim()), array) != -1) {

                    $("#Colortbl tbody > tr:eq(" + i + ")").find("td:eq(0)").find("input").prop("checked", true);
                }
            }
            for (var i = 1; i < sizetblLength; i++) {
                var a = ($("#Sizetbl tbody > tr:eq(" + i + ")").find("td:eq(2)").text()).trim();
                if (jQuery.inArray((($("#Sizetbl tbody > tr:eq(" + i + ")").find("td:eq(2)").text()).trim()), array) != -1) {

                    $("#Sizetbl tbody > tr:eq(" + i + ")").find("td:eq(0)").find("input").prop("checked", true);
                }
            }
            for (var i = 1; i < styletblLength; i++) {
                var a = ($("#Styletbl tbody > tr:eq(" + i + ")").find("td:eq(2)").text()).trim();
                if (jQuery.inArray((($("#Styletbl tbody > tr:eq(" + i + ")").find("td:eq(2)").text()).trim()), array) != -1) {

                    $("#Styletbl tbody > tr:eq(" + i + ")").find("td:eq(0)").find("input").prop("checked", true);
                }
            }
        });

        function PrintBarcode(itemId, index) {
            var itemCode = $("#ContentPlaceHolder1_hfItemCode").val(), itemName = $("#ContentPlaceHolder1_txtName").val(), price = 200.00;
            price = $("#ContentPlaceHolder1_gvStockItemCostCenterInfo tr:eq(" + index + ")").find("td:eq(3)").find("input").val();

            if (itemCode == "") {
                toastr.info("Please Load Item For BarCode Generate.");
                return false;
            }

            PageMethods.PrintBarCode(itemCode, itemName, price, OnBarCodeSucceed, OnBarCodeFailed);

            return false;
        }

        function PrintBarCodeById(itemId) {
            PageMethods.PrintBarCodeById(itemId, OnBarCodeSucceed, OnBarCodeFailed);
            return false;
        }

        function OnBarCodeSucceed(result) {
            //toastr.info(result);

            var baseUrl = "http://" + window.location.host + "/";
            var url = baseUrl + result;

            $("#DetailsRequisitionGridContaiiner").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 700,
                maxWidth: 800,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Bar Code",
                show: 'slide'
            });

            var myframe = document.getElementById("ifrmReportViewer");
            if (myframe !== null) {
                if (myframe.src) {
                    myframe.src = url;
                }
                else if (myframe.contentWindow !== null && myframe.contentWindow.location !== null) {
                    myframe.contentWindow.location = url;
                }
                else { myframe.setAttribute('src', url); }
            }
        }

        function OnBarCodeFailed() {

        }

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>

    </div>


    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />

    <asp:HiddenField ID="hfAddAttribute" runat="server" />
    <asp:HiddenField ID="hfItemId" runat="server" />
    <asp:HiddenField ID="hfEditedItemId" runat="server" />
    <asp:HiddenField ID="hfItemCost" runat="server" />
    <asp:HiddenField ID="hfSaveObj" runat="server" />
    <asp:HiddenField ID="hfDeleteObj" runat="server" />
    <asp:HiddenField ID="hfIngredientsText" runat="server" Value="" />
    <asp:HiddenField ID="hfIsReceipeExist" runat="server" Value="" />
    <asp:HiddenField ID="hfIsRecipeIncludedInInventory" runat="server" />
    <asp:HiddenField ID="hfSoftwareModulePermissionListInformation" runat="server" />
    <asp:HiddenField ID="hfIsInvItemCodeAutoGenerate" runat="server" />
    <asp:HiddenField ID="hfIsItemOriginHide" runat="server" />
    <asp:HiddenField ID="hfItemCode" runat="server" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletedDocId" runat="server" Value="" />
    <asp:HiddenField ID="hfItemAttributeList" runat="server" Value="" />
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnEditInServer" runat="server" Text="Button" ClientIDMode="Static"
            OnClick="btnEditInServer_Click" />
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Item Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Item Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="ProductInputPAnel" class="panel panel-default">
                <div class="panel-heading">
                    Item Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblServiceType" runat="server" class="control-label required-field"
                                    Text="Category"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCategoryId" CssClass="form-control" runat="server" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:HiddenField ID="txtProductId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblDispalyName" runat="server" class="control-label" Text="Dispaly Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" TabIndex="1"
                                    MaxLength="65"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align" id="CodeModelLabel" runat="server">
                                <asp:Label ID="lblCode" runat="server" class="control-label required-field" Text="Item Code"></asp:Label>
                            </div>
                            <div class="col-md-4" id="CodeModelControl" runat="server">
                                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="15" TabIndex="2"></asp:TextBox>
                            </div>

                            <div id="originDiv">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblOrigin" runat="server" class="control-label"
                                        Text="Country of Origin"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlOrigin" runat="server" CssClass="form-control" TabIndex="4">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align" id="Div1">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Model Number"></asp:Label>
                            </div>
                            <div class="col-md-4" id="Div2" runat="server">
                                <asp:TextBox ID="txtModelNumber" runat="server" CssClass="form-control" MaxLength="15" TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:HiddenField ID="hfExistingStockType" runat="server" />
                                <asp:Label ID="lblStockType" runat="server" class="control-label required-field"
                                    Text="Stock Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlStockType" runat="server" CssClass="form-control" TabIndex="4">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="StockItemInformationDiv">
                            <div id="ServiceWarrantyAndProductTypeDiv" runat="server" class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblProductType" runat="server" class="control-label" Text="Item Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlProductType" runat="server" CssClass="form-control" TabIndex="4">
                                        <asp:ListItem>Non Serial Product</asp:ListItem>
                                        <asp:ListItem>Serial Product</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblServiceWarranty" runat="server" class="control-label" Text="Service Warranty"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlServiceWarranty" runat="server" CssClass="form-control"
                                        TabIndex="13">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblManufacturer" runat="server" class="control-label" Text="Manufacturer/Brand"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlManufacturer" runat="server" CssClass="form-control" TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblAccFrequency" runat="server" class="control-label" Text="Access Frequancy"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlAccessFrequancy" CssClass="form-control" runat="server">
                                        <asp:ListItem Text="--- Please Select ---" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                                        <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                                        <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div style="display: none;">
                            <div id="USDCurrencyInfo" runat="server">
                                <div class="form-group">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblSellingPriceLocal" runat="server" class="control-label" Text="Selling Price One"></asp:Label>
                                        <asp:DropDownList ID="ddlSellingPriceLocal" runat="server" CssClass="form-control"
                                            TabIndex="7" Visible="False">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtSellingPriceLocal" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="lblSellingPriceUsd" runat="server" class="control-label" Text="Selling Price Two"></asp:Label>
                                        <asp:DropDownList ID="ddlSellingPriceUsd" runat="server" CssClass="form-control"
                                            TabIndex="10" Visible="False">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtSellingPriceUsd" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblStockBy" runat="server" class="control-label required-field" Text="Inventory Unit"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlStockBy" runat="server" CssClass="form-control" TabIndex="13">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Sales Unit"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSalesStockBy" runat="server" CssClass="form-control" TabIndex="13">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblClassification" runat="server" class="control-label required-field"
                                    Text="Item Classification"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlClassification" runat="server" CssClass="form-control" TabIndex="13">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblPurchasePrice" runat="server" class="control-label" Text="Purchase Price"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPurchasePrice" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIsCustomerItem" runat="server" class="control-label required-field"
                                    Text="Is Customer Item"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsCustomerItem" runat="server" CssClass="form-control" TabIndex="14">
                                    <asp:ListItem Value="-1">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIsSupplierItem" runat="server" class="control-label required-field"
                                    Text="Is Supplier Item"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsSupplierItem" runat="server" CssClass="form-control" TabIndex="15">
                                    <asp:ListItem Value="-1">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="SupplierDiv">
                            <div id="SupplierInfoDiv" class="panel panel-default">
                                <div class="panel-body" style='overflow-x: hidden; overflow-y: scroll; width: 100%; height: 300px'>
                                    <asp:GridView ID="gvSupplierInfo" runat="server" AllowPaging="True"
                                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                        ForeColor="#333333" PageSize="500000" CssClass="table table-bordered table-condensed table-responsive">
                                        <RowStyle BackColor="#E3EAEB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierId" runat="server" Text='<%#Eval("SupplierId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Supplier Information" ItemStyle-Width="55%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvCostCentre" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
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
                        <div id="IsAttributeDiv" runat="server">
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblIsAttributeItem" runat="server" class="control-label required-field"
                                        Text="Is Attribute Item"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlIsAttributeItem" runat="server" CssClass="form-control" TabIndex="15">
                                        <asp:ListItem Value="-1">--- Please Select ---</asp:ListItem>
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="AttributeDiv">
                                <div id="AttributeInfoDiv" class="panel panel-default">
                                    <div class="panel-body" style='overflow-x: hidden; overflow-y: scroll; width: 100%; height: 300px'>
                                        <div class="form-group">
                                            <div class="col-md-2 label-align">
                                                <asp:Label ID="lblSetupType" runat="server" class="control-label required-field"
                                                    Text="Attribute Type"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlSetupType" runat="server" CssClass="form-control" TabIndex="15">
                                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                                    <asp:ListItem Value="1">Color</asp:ListItem>
                                                    <asp:ListItem Value="2">Size</asp:ListItem>
                                                    <asp:ListItem Value="3">Style</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2 label-align">
                                                <asp:Label ID="lblAttributeName" runat="server" class="control-label required-field"
                                                    Text="Name"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlAttributeName" runat="server" CssClass="form-control" TabIndex="15">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <%--<div id="AttributeTableDiv" runat="server">
                                        </div>--%>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Button ID="btnAddAttribute" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary btn-sm"
                                                    OnClientClick="btnAddAttribute_Click();return false;" UseSubmitBehavior="false" />
                                                <asp:Label ID="hfEducationId" runat="server" class="control-label" Text='' Visible="False"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row" style="padding: 10px">
                                            <div class="col-md-12">
                                                <div class="table-responsive">
                                                    <table class="table table-bordered">
                                                        <thead>
                                                            <tr>
                                                                <th class="text-left" style="width: 20%">Attribute</th>
                                                                <th class="text-left" style="width: 70%">Name</th>
                                                                <th class="text-center" style="width: 10%">Action</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tbody">
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Is Open Item"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsItemEditable" runat="server" CssClass="form-control" TabIndex="15">
                                    <asp:ListItem Value="-1">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="ItemTypeDiv">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblItemType" runat="server" class="control-label" Text="Item Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-control" TabIndex="15">
                                        <asp:ListItem Value="IndividualItem">Individual</asp:ListItem>
                                        <asp:ListItem Value="ComboItem">Combo</asp:ListItem>
                                        <asp:ListItem Value="BuffetItem">Buffet</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div id="UpImageDiv">
                                <div class="col-md-2 label-align">
                                    <asp:HiddenField ID="RandomProductId" runat="server" Value="0"></asp:HiddenField>
                                    <asp:HiddenField ID="tempProductId" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lbComImageImage" runat="server" class="control-label" Text="Item Image"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <input id="btnImageUp" tabindex="3" type="button" onclick="javascript: return LoadImageUploader();"
                                        class="btn btn-primary" value="Item Image..." />
                                </div>
                            </div>
                            <div class="col-md-2 label-align">
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>
                        <div class="form-group">
                            <div id="DocumentInfo">
                            </div>
                        </div>

                        <div id="StockItemCostCenterInformationDiv" style="display: none;">
                            <div class="panel-body">
                                <asp:GridView ID="gvStockItemCostCenterInfo" Width="100%" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" OnRowCommand="gvStockItemCostCenterInfo_RowCommand" AllowSorting="True"
                                    ForeColor="#333333" PageSize="200" CssClass="table table-bordered table-condensed table-responsive">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IDNO" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCostCentreId" runat="server" Text='<%#Eval("CostCenterId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MappingId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMappingId" runat="server" Text='<%#Eval("MappingId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cost Center" ItemStyle-Width="28%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvCostCentre" runat="server" Text='<%# Bind("CostCenter") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Minimum Stock" ShowHeader="False" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMinimumStockLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit Price" ShowHeader="False" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUnitPriceLocal" runat="server" CssClass="form-control"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit Price(USD)" ShowHeader="False" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUnitPriceUsd" runat="server" CssClass="form-control"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Charge" ShowHeader="False" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtServiceCharge" runat="server" Text='<%# Bind("ServiceCharge") %>' CssClass="form-control"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SD Charge" ShowHeader="False" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSDCharge" runat="server" Text='<%# Bind("CitySDCharge") %>' CssClass="form-control"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vat Amount" ShowHeader="False" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtVatAmount" runat="server" Text='<%# Bind("VatAmount") %>' CssClass="form-control"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Additional Charge" ShowHeader="False" ItemStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAdditionalCharge" runat="server" Text='<%# Bind("AdditionalCharge") %>' CssClass="form-control"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount" ShowHeader="False" ItemStyle-Width="15%" Visible="false">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control">
                                                    <asp:ListItem>Fixed</asp:ListItem>
                                                    <asp:ListItem>Percentage</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount Amount" ShowHeader="False" ItemStyle-Width="10%" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Font-Size="Small" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="ImgBarCodePrintInfo" Text="<i aria-hidden='true' class='glyphicon glyphicon-barcode'></i>"
                                                    runat="server" CommandName="CmdBarCodePrintInfo"
                                                    CommandArgument='<%# bind("CostCenterId") %>'
                                                    OnClientClick='<%#String.Format("return PrintBarcode({0}, {1})", Eval("CostCenterId"), Eval("Index")) %>'
                                                    ToolTip="Bar Code Print"></asp:LinkButton>
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
                        <div id="KitchenItemCostCenterInformationDiv" style="display: none;">
                            <div id="childTabs">
                                <ul id="ChildTabpage" class="ui-style">
                                    <li id="P" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                                        href="#childtab-1">Cost Center</a></li>
                                    <li id="Q" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                                        href="#childtab-2">
                                        <asp:Label ID="RecipeDetailText" runat="server" Text="Ingredients"></asp:Label></a></li>
                                </ul>
                                <div id="childtab-1" class="panel panel-default">
                                    <div class="panel-body">
                                        <asp:GridView ID="gvKitchenItemCostCenterInfo" Width="100%" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                            ForeColor="#333333" PageSize="200" OnRowDataBound="gvKitchenItemCostCenterInfo_RowDataBound"
                                            CssClass="table table-bordered table-condensed table-responsive">
                                            <RowStyle BackColor="#E3EAEB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="IDNO" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCostCentreId" runat="server" Text='<%#Eval("CostCenterId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MappingId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMappingId" runat="server" Text='<%#Eval("MappingId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cost Center" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvCostCentre" runat="server" Text='<%# Bind("CostCenter") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location" ShowHeader="False" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlKitchen" runat="server" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ControlStyle Font-Size="Small" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Min. Stock" ShowHeader="False" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMinimumStockLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Font-Size="Small" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Price" ShowHeader="False" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtUnitPriceLocal" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Font-Size="Small" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Price(USD)" ShowHeader="False" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtUnitPriceUsd" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Font-Size="Small" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Service Charge" ShowHeader="False" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtServiceCharge" runat="server" Text='<%# Bind("ServiceCharge") %>' CssClass="form-control"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Font-Size="Small" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SD Charge" ShowHeader="False" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSDCharge" runat="server" Text='<%# Bind("CitySDCharge") %>' CssClass="form-control"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Font-Size="Small" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vat Amount" ShowHeader="False" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtVatAmount" runat="server" Text='<%# Bind("VatAmount") %>' CssClass="form-control"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Font-Size="Small" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Additional Charge" ShowHeader="False" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAdditionalCharge" runat="server" Text='<%# Bind("AdditionalCharge") %>' CssClass="form-control"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Font-Size="Small" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Discount" ShowHeader="False" ItemStyle-Width="15%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control">
                                                            <asp:ListItem>Fixed</asp:ListItem>
                                                            <asp:ListItem>Percentage</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ControlStyle Font-Size="Small" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Discount Amount" ShowHeader="False" ItemStyle-Width="8%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Font-Size="Small" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
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
                                <div id="childtab-2" class="panel panel-default">
                                    <div class="panel-body">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-md-2 label-align">
                                                    <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server" TabIndex="20">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 label-align">
                                                    <asp:Label ID="lblItemName" runat="server" class="control-label" Text="Item Name"></asp:Label>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:TextBox ID="txtItemName" CssClass="form-control" TabIndex="21" runat="server"
                                                        ClientIDMode="Static"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 label-align">
                                                    <asp:Label ID="lblItemStockBy" runat="server" class="control-label" Text="Stock By"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ddlItemStockBy" runat="server" CssClass="form-control" TabIndex="22">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-2 label-align">
                                                    <asp:Label ID="lblItemUnit" runat="server" class="control-label" Text="Unit"></asp:Label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtItemUnit" TabIndex="23" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <button type="button" id="btnAddItem" tabindex="24" class="btn btn-primary">
                                                        Add</button>
                                                </div>
                                            </div>
                                            <div class="form-group" style="padding: 0px;">
                                                <div id="ltlTableWiseItemInformation" runat="server" clientidmode="Static">
                                                </div>
                                                <div style="width: 100%; margin-top: 5px; font-weight: bold; font-size: 14px;">
                                                    <span>Total Cost :&nbsp;&nbsp;
                                                        <asp:Label ID="lblTotalReceipeCost" runat="server" class="control-label" Text=""
                                                            ClientIDMode="Static"></asp:Label>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" style="padding-top: 10px;">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="24" CssClass="btn btn-primary"
                                OnClick="btnSave_Click" OnClientClick="javascript:return ValidationNPreprocess();" />
                            <asp:Button ID="btnClear" OnClientClick="return confirm('Do you want to clear?');" runat="server" Text="Clear" TabIndex="25" CssClass="btn btn-primary"
                                OnClick="btnClear_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="EntryPanel" class="panel panel-default" style="">
                <div class="panel-heading">
                    Search Item Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCategoryId" runat="server" class="control-label" Text="Category"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlPCategoryId" TabIndex="1" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblPName" runat="server" class="control-label" Text="Item Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSDisplayName" runat="server" class="control-label" Text="Display Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSDisplayName" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblPCode" runat="server" class="control-label" Text="Item Code"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPCode" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSClassification" runat="server" class="control-label" Text="Item Classification"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSClassification" TabIndex="1" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Order By (Asc)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSrcOrderBy" runat="server" CssClass="form-control" TabIndex="15">
                                    <asp:ListItem Value="Name">Name</asp:ListItem>
                                    <asp:ListItem Value="Code">Code</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnSearch" class="btn btn-primary">
                                    Search</button>
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
                    <table id='gvInvItemInformation' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 40%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Item Name
                                </td>
                                <td>Item Code
                                </td>
                                <td>Model number
                                </td>
                                <td style="text-align: right;">Action
                                </td>
                                <td style="text-align: right;">Bar Code
                                </td>
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

    <%-- <div id="popUpImage" style="display: none">
        <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
            <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
        </asp:Panel>
    </div>--%>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>


    <div id="DetailsRequisitionGridContaiiner">
        <div class="row">
            <div class="col-md-12">
                <iframe id="ifrmReportViewer" name="ifrmReportViewer" frameborder="0" style="width: 100%; height: 800px" scrolling="yes"></iframe>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }

        var xKitchenInfo = '<%=isKitchenItemCostCenterInformationDivEnable%>';
        if (xKitchenInfo > -1) {
            if (parseInt(xKitchenInfo) == 1) {
                $('#StockItemCostCenterInformationDiv').hide("slow");
                $('#KitchenItemCostCenterInformationDiv').show("slow");
            }
        }
        else {
            $('#StockItemCostCenterInformationDiv').show("slow");
            $('#KitchenItemCostCenterInformationDiv').hide("slow");
        }

        var ddlStockType = '<%=ddlStockType.ClientID%>'
        if ($('#' + ddlStockType).val() == "KitchenItem") {
            $('#StockItemInformationDiv').hide("slow");
            $('#StockItemCostCenterInformationDiv').hide("slow");
            $('#KitchenItemCostCenterInformationDiv').show("slow");
        }
        else if ($('#' + ddlStockType).val() == "IngredientText") {
            $('#StockItemInformationDiv').hide("slow");
            $('#StockItemCostCenterInformationDiv').hide("slow");
            $('#KitchenItemCostCenterInformationDiv').show("slow");
        }
        else {
            $('#StockItemInformationDiv').show("slow");
            $('#StockItemCostCenterInformationDiv').show("slow");
            $('#KitchenItemCostCenterInformationDiv').hide("slow");
        }

        $(document).ready(function () {
            var RecipeDetailText = '<%=RecipeDetailText.ClientID%>'
            $('#' + RecipeDetailText).text($("#ContentPlaceHolder1_hfIngredientsText").val());
        });
    </script>
</asp:Content>
