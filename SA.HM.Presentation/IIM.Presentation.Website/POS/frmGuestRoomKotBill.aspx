<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGuestRoomKotBill.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmGuestRoomKotBill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var alreadySaveKotRemarks = [];
        $('#QuantityChangeContainer').hide();
        $('#ItemChangeContainer').hide();
        $('#UnitPriceChangeContainer').hide();

        var pageTitle = '';

        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Restaurant Kot Bill (Room Wise)</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#txtTouchKeypadResultForPax").val('');

            CommonHelper.TouchScreenNumberKeyboardWithoutDot("numkb", "KeyBoardContainer");
            var keyboard = $('.numkb').getkeyboard();
            keyboard.reveal();

            $('#OpenKeyboardForTextArea').click(function () {
                CommonHelper.TouchScreenKeyboard("ContentPlaceHolder1_txtSpecialRemarks");
                var keyboard = $('#ContentPlaceHolder1_txtSpecialRemarks').getkeyboard();
                keyboard.reveal();
            });

            $("#btnItemwiseSpecialRemarksCancel").click(function () {
                $("#ItemWiseSpecialRemarks").dialog("close");
            });

            $('.RoomOccupaiedDiv').on('click', function (e) {
                pageTitle = "Occupied Room Possible Path";
                var KotNTableNumber = $(this).find(".KOTNTableNumberDiv").text();
                if (KotNTableNumber != "") {
                    PageMethods.LoadOccupiedPossiblePath(KotNTableNumber, pageTitle, OnLoadOutOfOrderPossiblePathSucceeded, OnShowOutOfServiceRoomInformationFailed);
                }
                return false;
            });

            $("#btnItemwiseSpecialRemarksSave").click(function () {
                $('#MessageBox').hide();
                var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();
                var itemId = $("#<%=txtItemIdInformation.ClientID %>").val();
                var kotDetailId = $("#hfKotDetailId").val();

                var specialRsId = 0;
                var RKotSRemarksDetail = [];
                var RKotSRemarksDetailDelete = [];

                $("#TableWiseItemRemarksInformation tbody tr").each(function (index, item) {

                    var remarksSelected = $(this).find('td:eq(1)').find("input");
                    specialRsId = parseInt($(this).find('td:eq(0)').text(), 10);

                    if (remarksSelected.is(':checked')) {

                        var notNew = _.findWhere(alreadySaveKotRemarks, { SpecialRemarksId: specialRsId });

                        if (notNew == null) {

                            RKotSRemarksDetail.push({
                                KotId: kotId,
                                ItemId: itemId,
                                SpecialRemarksId: specialRsId
                            });
                        }

                    }
                    else {
                        var notOld = _.findWhere(alreadySaveKotRemarks, { SpecialRemarksId: specialRsId });

                        if (notOld != null) {
                            RKotSRemarksDetailDelete.push({
                                RemarksDetailId: notOld.RemarksDetailId,
                                KotId: kotId,
                                ItemId: itemId,
                                SpecialRemarksId: specialRsId
                            });
                        }
                    }

                });

                PageMethods.SaveKotSpecialRemarks(RKotSRemarksDetail, RKotSRemarksDetailDelete, kotDetailId, OnSaveKotSpecialRemarksSuccedd, OnSaveKotSpecialRemarksFailed);
            });

            $("#txtItemName").autocomplete({
                source: function (request, response) {

                    var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '/POS/frmKotBillMaster.aspx/ItemSearch',
                        data: "{'itemName':'" + request.term + "','costCenterId':'" + hfCostCenterIdVal + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    ImagePath: m.ImageName,
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
                    $(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);

                    var itemDiv = "<div class=\"DivRestaurantItemContainer\" style='clear:both; margin-left:165px;'>" +
                      "<div class=\"RestaurantItemDiv\">" +
                        "<img src=\"" + ui.item.ImagePath + "\" onclick=\"return PerformAction(" + ui.item.value + ",'" + ui.item.label + "');\" id=\"img" + ui.item.value + "\" class=\"ItemImageSize\">" +
                      "</div>" +
                      "<div class=\"ItemNameDiv\">" + ui.item.label + "</div>" +
                    "</div>"
                    $("#itemDetailsContainer").html(itemDiv);
                }
            });

        });

        function OnLoadOutOfOrderPossiblePathSucceeded(result) {
            if (result.IsSuccess == true) {
                $('#serviceDeciderHtml').html(result.DataStr);
                $("#serviceDecider").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 580,
                    closeOnEscape: true,
                    resizable: false,
                    title: pageTitle,
                    show: 'slide'
                });
            }
            else {
                $('#serviceDeciderMessage').html("Bill Already Settled.");
                $("#serviceDeciderAlert").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 580,
                    closeOnEscape: false,
                    close: ServiceDeciderAlertDialogClose,
                    resizable: false,
                    title: pageTitle,
                    show: 'slide'
                });
            }
        }

        function OnShowOutOfServiceRoomInformationFailed() { }

        function KotPrintPreview(tableId) {
            $("#<%=hfTableIdForPrint.ClientID %>").val(tableId);
            $("#serviceDecider").dialog("close");
            $("#btnKotRePrintPreview").trigger("click");
            return false;
        }

        function GoToCategoryHome() {
            window.location = "frmGuestRoomKotBill.aspx?Kot=RestaurantItemCategory:0";
            return false;
        }
        function GoToRoomDesign() {
            window.location = "frmGuestRoomKotBill.aspx?Kot=RoomAllocation";
            return false;
        }
        function AddSpecialRemarks() {
            $("#<%=txtSpecialRemarks.ClientID %>").val();
            //popup(1, 'SpecialRemarksDiv', '', 732, 230);
            $("#SpecialRemarksDiv").dialog({
                width: 732,
                height: 230,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", ////TODO add title
                show: 'slide'
            });
            return false;
        }
        function QuantityChange() {
            $('#QuantityChangeContainer').show();
            $('#ItemChangeContainer').hide();
            $('#UnitPriceChangeContainer').hide();
            //$("#txtChangeItemName").val("");
            $("#txtUnitPriceChange").val("");
        }

        function ItemNameChange() {
            $('#QuantityChangeContainer').hide();
            $('#ItemChangeContainer').show();
            $('#UnitPriceChangeContainer').hide();
            $("#txtUnitPriceChange").val("");
            $("#txtQuantityChange").val("");
        }

        function UnitPriceChange() {
            $('#QuantityChangeContainer').hide();
            $('#ItemChangeContainer').hide();
            $('#UnitPriceChangeContainer').show();
            //$("#txtChangeItemName").val("");
            $("#txtQuantityChange").val("");
        }

        function PerformGoToRestaurantBillAction() {
            var answer = confirm("Do you want to Process Restaurant Bill?")
            if (answer) {
                var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
                var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();
                PageMethods.GetRestaurantGoToRestaurantBillAction(kotId, OnPerformGoToRestaurantBillActionSucceeded, OnPerformGoToRestaurantBillActionFailed);
            }
            return false;
        }

        function OnPerformGoToRestaurantBillActionSucceeded(result) {
            window.location = "/POS/frmRestaurantBill.aspx?RoomNumber=" + result;
            return false;
        }

        function OnPerformGoToRestaurantBillActionFailed(error) {
            toastr.error(error.get_message());
        }

        function UpdateItemDetails(updateType) {

            var updateContent = "";

            if (updateType == "QuantityChange") {
                updateContent = $("#txtQuantityChange").val();

                if (!CommonHelper.IsDecimal(updateContent)) {
                    toastr.warning("Quantity Must be Numeric");
                    return false;
                }

                toastr.success("Quantity Update Successfully");
            }
            else if (updateType == "ItemNameChange") {
                updateContent = $("#txtChangeItemName").val();

                CommonHelper.ExactMatch();

                if ($("#TableWiseItemInformation tbody tr").find("td:eq(0):textEquals('" + updateContent + "')").length > 0) {
                    toastr.error("Different item can not be added as same name.");
                    return false;
                }

                toastr.success("Item Name Update Successfully");
            }
            else if (updateType == "UnitPriceChange") {
                updateContent = $("#txtUnitPriceChange").val();

                if (!CommonHelper.IsInt(updateContent)) {
                    toastr.warning("Unit Price Must be Numeric");
                    return false;
                }

                toastr.success("Unit Price Update Successfully");
            }

            PerformUpdateAction(updateType, updateContent);
        }

        function ItemAutoSearch() {
            $("#txtItemName").val('');
            $("#itemDetailsContainer").html('');

            $("#ItemSearchDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 500,
                height: 280,
                closeOnEscape: true,
                resizable: false,
                title: "Item Search & Add",
                show: 'slide'
            });

            $("#txtItemName").focus();
            return false;
        }

        function AddNewItem(kotDetailsId, itemId, rowId) {
            $('#QuantityChangeContainer').hide();
            $('#ItemChangeContainer').hide();
            $('#UnitPriceChangeContainer').hide();
            $("#hfKotDetailId").val(kotDetailsId);
            $("#hfEditRowId").val(rowId);

            var itemName = $("#TableWiseItemInformation tbody tr:eq(" + rowId + ")").find("td:eq(0)").text();
            $("#txtChangeItemName").val(itemName);

            //popup(1, 'editServiceDecider', '', 580, 400);
            $("#editServiceDecider").dialog({
                autoOpen: true,
                modal: true,
                width: 580,
                closeOnEscape: true,
                resizable: false,
                title: "Update Decider For Order Item",
                show: 'slide'
            });
            return false;
        }

        function PerformUpdateAction(updateType, updatedContent) {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var kotDetailsId = $("#hfKotDetailId").val();
            var rowId = $("#hfEditRowId").val();
            var quantity = $("#TableWiseItemInformation > tbody tr:eq(" + rowId + ")").find("td:eq(1)").text();
            quantity = parseInt($.trim(quantity));
            PageMethods.UpdateIndividualItemDetailInformation(updateType, hfCostCenterIdVal, kotDetailsId, quantity, updatedContent, OnUpdateObjectSucceeded, OnUpdateObjectFailed);
            return false;
        }

        function OnUpdateObjectSucceeded(result) {
            $("#<%=txtItemIdInformation.ClientID %>").val('');
            $("#<%=txtTouchKeypadResult.ClientID %>").val('');
            $('#QuantityChangeContainer').hide();
            $('#ItemChangeContainer').hide();
            $('#UnitPriceChangeContainer').hide();
            $("#txtChangeItemName").val("");
            $("#txtUnitPriceChange").val("");
            $("#txtQuantityChange").val("");

            LoadGridInformation();
            return false;
        }

        function OnUpdateObjectFailed(error) {
            toastr.error(error.get_message());
        }
        function myFunction(val) {
            var existingValue = $("#<%=txtTouchKeypadResult.ClientID %>").val();
            if (val != 99) {
                $("#<%=txtTouchKeypadResult.ClientID %>").val(existingValue + val);
            }
            else {
                if (existingValue.length > 0) {
                    var m = existingValue.substring(0, existingValue.length - 1);
                    $("#<%=txtTouchKeypadResult.ClientID %>").val(m);
                }
            }
        }
        function AddPAXInfo(val) {
            $("#<%=txtItemIdInformation.ClientID %>").val(val);
            $("#TouchKeypadForPax1").dialog({
                width: 420,
                height: 410,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", ////TODO add title
                show: 'slide'
            });
            return false;
        }
        function myFunctionForPax(val) {
            var existingValue = $("#<%=txtTouchKeypadResultForPax.ClientID %>").val();
            if (val != 99) {
                $("#<%=txtTouchKeypadResultForPax.ClientID %>").val(existingValue + val);
            }
            else {
                if (existingValue.length > 0) {
                    var m = existingValue.substring(0, existingValue.length - 1);
                    $("#<%=txtTouchKeypadResultForPax.ClientID %>").val(m);
                }
            }
        }
        //---Update Data---------
        function PerformUpdateActionForPax() {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var itemQuantity = $("#<%=txtTouchKeypadResultForPax.ClientID %>").val();

            if (itemQuantity == "0" || $.trim(itemQuantity) == "") {
                toastr.warning("Pax Cannot Zero (0).");
                return false;
            }

            PageMethods.UpdateTablePaxInformation(hfCostCenterIdVal, itemQuantity, OnUpdateObjectSucceededForPax, OnUpdateObjectFailedForPax);
            return false;
        }
        function OnUpdateObjectSucceededForPax(result) {

            $("#TouchKeypadForPax1").dialog("close");
            if (result) {
                toastr.success("Pax Update Succeed.");
            }
            else {
                toastr.error("Pax Update Unsucceed.");
            }

            return false;
        }

        function OnUpdateObjectFailedForPax(error) {
            toastr.error(error.get_message());
        }

        //---Save Individual Data---------
        function PerformAction(selectedItemId, selectedItemName) {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();

            CommonHelper.ExactMatch();

            var tr = $("#TableWiseItemInformation tbody tr").find("td:eq(5):textEquals('" + $.trim(selectedItemId) + "')").parent();
            var selectedItemQty = "0", itemIdInTable = "0", itemNameInTable = "";
            if ($(tr).length != 0) {
                selectedItemQty = $(tr).find("td:eq(1)").text();
                itemIdInTable = $(tr).find("td:eq(5)").text();
                itemNameInTable = $(tr).find("td:eq(0)").text();
            }

            if (itemIdInTable != "0") {
                if ($.trim(itemNameInTable) != $.trim(selectedItemName)) {
                    toastr.error("Same item can not be added as different name.");
                    return false;
                }
            }

            PageMethods.SaveIndividualItemDetailInformation(hfCostCenterIdVal, kotId, selectedItemId, selectedItemQty, OnSaveObjectSucceeded, OnSaveObjectFailed);
            return false;
        }

        function OnSaveObjectSucceeded(result) {
            LoadGridInformation();

            if ($("#ItemSearchDialog").is(':visible') == true)
                $("#ItemSearchDialog").dialog('close');

            return false;
        }

        function OnSaveObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadGridInformation() {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var bearerId = $("#<%=txtBearerIdInformation.ClientID %>").val();
            var tableId = $("#<%=txtTableIdInformation.ClientID %>").val();
            var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();
            PageMethods.GenerateTableWiseItemGridInformation(hfCostCenterIdVal, tableId, kotId, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#ltlTableWiseItemInformation").html(result);
            return false;
        }

        function OnLoadObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //For Delete-------------------------        
        function PerformDeleteAction(actionId, kotId, itemId) {

            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                CommonHelper.ExactMatch();
                var tr = $("#TableWiseItemInformation tbody tr").find("td:eq(5):textEquals('" + itemId + "')").parent();
                var selectedItemQty = "0";
                if ($(tr).length != 0) {
                    selectedItemQty = $(tr).find("td:eq(1)").text();
                }

                var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
                var kotDetailsId = $("#hfKotDetailId").val();

                PageMethods.UpdateIndividualItemDetailInformation('QuantityChange', hfCostCenterIdVal, actionId, selectedItemQty, 0, OnUpdateObjectSucceeded, OnUpdateObjectFailed);

                //PageMethods.DeleteData(actionId, kotId, itemId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }
        //        function PerformDeleteAction(actionId) {

        //            var answer = confirm("Do you want to delete this record?")
        //            if (answer) {
        //                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
        //            }
        //            return false;
        //        }

        function OnDeleteObjectSucceeded(result) {
            LoadGridInformation();
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //---Save Combo Data---------
        function PerformActionForCombo(selectedItemId) {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();
            PageMethods.SaveComboItemDetailInformation(hfCostCenterIdVal, kotId, selectedItemId, OnSaveComboObjectSucceeded, OnSaveComboObjectFailed);
            return false;
        }

        function OnSaveComboObjectSucceeded(result) {
            //window.location = "/Restaurant/frmKotBill.aspx?Kot=RestaurantItem:1"
            LoadGridInformation();
            return false;
        }

        function OnSaveComboObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //---Save Buffet Data---------
        function PerformActionForBuffet(selectedItemId) {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();
            PageMethods.SaveBuffetItemDetailInformation(hfCostCenterIdVal, kotId, selectedItemId, OnSaveBuffetObjectSucceeded, OnSaveBuffetObjectFailed);
            return false;
        }

        function OnSaveBuffetObjectSucceeded(result) {
            //window.location = "/Restaurant/frmKotBill.aspx?Kot=RestaurantItem:1"
            LoadGridInformation();
            return false;
        }

        function OnSaveBuffetObjectFailed(error) {
            toastr.error(error.get_message());
        }
        function PerformUpdateActionForSpecialRemarks() {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var specialRemarks = $("#<%=txtSpecialRemarks.ClientID %>").val();
            PageMethods.UpdateTableSpecialRemarksInformation(hfCostCenterIdVal, specialRemarks, OnUpdateObjectSucceededForSpecialRemarks, OnUpdateObjectFailedForSpecialRemarks);
            return false;
        }
        function OnUpdateObjectSucceededForSpecialRemarks(result) {
            //popup(-1);
            $("#SpecialRemarksDiv").dialog("close"); //// dialog id may change
            return false;
        }

        function OnUpdateObjectFailedForSpecialRemarks(error) {
            toastr.error(error.get_message());
        }

        //Grid Information Visible True/False-------------------
        function LoadGridInformationShow() {
            $("#ltlTableWiseItemInformation").show("slow");
            LoadGridInformation();
        }
        function LoadGridInformationHide() {
            $("#ltlTableWiseItemInformation").hide("slow");
        }
        function CheckAddedItem() {

            var itemCount = $("#TableWiseItemInformation tbody tr").length;

            if (itemCount > 1)
                $("#<%=hfIsItemAdded.ClientID %>").val("1");
            else
                $("#<%=hfIsItemAdded.ClientID %>").val("0");
        }
        function AddItemWiseRemarks(kotId, itemId, kotDetailId) {
            $("#<%=txtKotIdInformation.ClientID %>").val(kotId);
            $("#<%=txtItemIdInformation.ClientID %>").val(itemId);
            $("#hfKotDetailId").val(kotDetailId);

            PageMethods.GetSpecialRemarksDetails(kotId, itemId, OnGetSpecialRemarksDetailsSucceed, OnGetSpecialRemarksDetailsFailed);
        }

        function OnGetSpecialRemarksDetailsSucceed(result) {

            vvc = result;
            alreadySaveKotRemarks = result.KotRemarks;

            var table = "", tr = "", td = "", i = 0, alreadyChecked = "";
            var specialRemarksLength = result.ItemSpecialRemarks.length;

            table = "<table class='table table-bordered table-condensed table-responsive' id=\"TableWiseItemRemarksInformation\" style=\"margin:0;\" >";
            table += "<thead>" +
                            "<tr style=\"color: White; background-color: #44545E; font-weight: bold;\">" +
                                "<th align=\"center\" scope=\"col\" style=\"width: 30px\">" +
                                    "Select" +
                                "</th>" +
                                "<th align=\"left\" scope=\"col\" style=\"width: 290px\">" +
                                    "Remarks" +
                                "</th>" +
                            "</tr>" +
                        "</thead> <tbody>";

            for (i = 0; i < specialRemarksLength; i++) {

                alreadyChecked = '';

                var vc = _.findWhere(result.KotRemarks, { SpecialRemarksId: result.ItemSpecialRemarks[i].SpecialRemarksId });
                if (vc != null)
                { alreadyChecked = "checked='checked'"; }

                if ((i % 2) == 0)
                    tr = "<tr style=\"background-color:#ffffff;\">";
                else
                    tr = "<tr style=\"background-color:#E3EAEB;\">";

                td = "<td style=\"display:none\">" + result.ItemSpecialRemarks[i].SpecialRemarksId + "</td>" +
                     "<td align=\"center\" style=\"width: 30px\">" +
                     "&nbsp;<input type=\"checkbox\" value=\"" + result.ItemSpecialRemarks[i].SpecialRemarksId + "\" " + alreadyChecked + " id=\"ch" + result.ItemSpecialRemarks[i].SpecialRemarksId + "\">" +
                      "</td>" +
                      "<td align=\"left\" style=\"width: 200px\">" +
                       result.ItemSpecialRemarks[i].SpecialRemarks +
                      "</td>";

                tr += td + "</tr>";

                table += tr;
            }
            table += " </tbody> </table>";

            $("#remarksContainer").html(table);

            $("#ItemWiseSpecialRemarks").dialog({
                autoOpen: true,
                modal: true,
                width: 400,
                closeOnEscape: true,
                resizable: false,
                title: "Item Special Remarks",
                show: 'slide'
            });
        }

        function OnGetSpecialRemarksDetailsFailed(error) {

        }
        function OnSaveKotSpecialRemarksSuccedd(result) {
            if (result.AlertMessage.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#<%=txtKotDetailsIdInformation.ClientID %>").val("");
                $("#<%=txtItemIdInformation.ClientID %>").val("");
                alreadySaveKotRemarks = [];
                $("#remarksContainer").remove("TableWiseItemRemarksInformation");
                $("#ItemWiseSpecialRemarks").dialog("close");

                LoadGridInformation();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }

        function OnSaveKotSpecialRemarksFailed(error) {
            alert(error.get_message());
        }
    </script>
    <div id="serviceDecider" style="display: none;">
        <div id="serviceDeciderHtml">
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnKotRePrintPreview" runat="server" Text="Kot Print Preview" ClientIDMode="Static"
            OnClick="btnKotRePrintPreview_Click" />
    </div>
    <asp:HiddenField ID="hfTableIdForPrint" runat="server" />
    <div id="editServiceDecider" style="display: none;">
        <input type="hidden" id="hfKotDetailId" value="0" />
        <input type="hidden" id="hfEditRowId" value="" />
        <div id="editServiceDeciderHtml">
            <div style="padding: 10px">
                <div style="float: left; padding-left: 10px; padding-bottom: 15px; width: 150px">
                    <input type="button" onclick="return QuantityChange();" class="btn btn-primary" value="Quantity Change"
                        style="width: 150px" />
                </div>
                <div style="float: left; padding-left: 10px; padding-bottom: 15px; width: 150px">
                    <input type="button" onclick="return ItemNameChange();" class="btn btn-primary" value="Name Change"
                        style="width: 150px" />
                </div>
                <div style="float: left; padding-left: 10px; padding-bottom: 15px; width: 150px">
                    <input type="button" onclick="return UnitPriceChange();" class="btn btn-primary"
                        value="Unit Price Change" style="width: 150px" />
                </div>
                <div id="QuantityChangeContainer" class="block" style="font-weight: bold; display: none;
                    height: 100px;">
                    <div style="float: left; padding-left: 28px; padding-bottom: 5px; margin-top: 10px;
                        width: 310px">
                        Quantity&nbsp;&nbsp;<input type="text" id="txtQuantityChange" />
                    </div>
                    <div style="float: left; padding-left: 28px; padding-bottom: 5px; margin-top: 10px;
                        width: 150px">
                        <input type="button" onclick="return UpdateItemDetails('QuantityChange');" class="btn btn-primary"
                            value="Change" style="width: 150px" />
                    </div>
                </div>
                <div id="ItemChangeContainer" class="block" style="font-weight: bold; display: none;
                    height: 100px;">
                    <div style="float: left; padding-left: 28px; padding-bottom: 5px; margin-top: 10px;
                        width: 550px">
                        Name&nbsp;&nbsp;<input type="text" id="txtChangeItemName" style="width: 400px;" />
                    </div>
                    <div style="float: left; padding-left: 28px; padding-bottom: 5px; margin-top: 10px;
                        width: 150px">
                        <input type="button" onclick="return UpdateItemDetails('ItemNameChange');" class="btn btn-primary"
                            value="Change" style="width: 150px" />
                    </div>
                </div>
                <div id="UnitPriceChangeContainer" class="block" style="font-weight: bold; display: none;
                    height: 100px;">
                    <div style="float: left; padding-left: 28px; padding-bottom: 5px; margin-top: 10px;
                        width: 310px">
                        Unit Price&nbsp;&nbsp;<input type="text" id="txtUnitPriceChange" />
                    </div>
                    <div style="float: left; padding-left: 28px; padding-bottom: 5px; margin-top: 10px;
                        width: 310px">
                        <input type="button" onclick="return UpdateItemDetails('UnitPriceChange');" class="btn btn-primary"
                            value="Change" style="width: 150px" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- <asp:Panel ID="pnlTableWiseKotBill" runat="server">
        <div class="btn-toolbar;" style="text-align: right;">
            <asp:ImageButton ID="imgBtnTableWiseKotBill" runat="server" ImageUrl="~/StyleSheet/images/TableWiseKotBill.png"
                OnClick="imgBtnTableWiseKotBill_Click" />
        </div>
    </asp:Panel>--%>
    <!--Popup div container  ends-->
    <div style="display: none;">
        <asp:TextBox ID="txtKotDetailsIdInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
    </div>
    <div id="ItemWiseSpecialRemarks" style="width: 350px; display: none;">
        <div class="">
            <div class="DataGridYScroll">
                <div id="remarksContainer" style="width: 100%;">
                </div>
            </div>
            <div id="Div4" style="padding-left: 5px; width: 100%;">
                <div style="float: left; padding-bottom: 15px;">
                    <input type="button" id="btnItemwiseSpecialRemarksSave" class="btn btn-primary" style="width: 150px;"
                        value="Save" />
                    <input type="button" id="btnItemwiseSpecialRemarksCancel" class="btn btn-primary"
                        style="width: 150px;" value="Cancel" />
                </div>
                <div id="ItemWiseSpecialRemarksContainer" class="alert alert-info" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert">
                        ×</button>
                    <asp:Label ID='ItemWiseSpecialRemarksMessage' Font-Bold="True" runat="server" ClientIDMode="Static"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <div id="TouchKeypad" style="display: none;">
        <%--<input type="text" name="txtTouchKeypad" id="txtTouchKeypad" placeholder="Item Quantity"
            style="width: 396px;" />--%>
        <div id="TouchKeypadResultDiv">
            <asp:HiddenField ID="hfCostCenterId" runat="server"></asp:HiddenField>
            <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtTouchKeypadResult" runat="server" CssClass="TouchKeypadResult"
                Height="40px" Font-Size="50px"></asp:TextBox>
        </div>
        <div class='panel-body'>
            <div class='DivNumericContainer'>
                <%-- <a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='ContentPlaceHolder1_img1' onclick='myFunction(0)' class="NumericItemImageDiv"
                        src="/Images/0.jpg" /></div>
                <%--  </a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%-- <a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='ContentPlaceHolder1_img2' onclick='myFunction(1)' class="NumericItemImageDiv"
                        src='/Images/1.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%-- <a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='ContentPlaceHolder1_img3' onclick='myFunction(2)' class="NumericItemImageDiv"
                        src='/Images/2.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%-- <a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img1' onclick='myFunction(3)' class="NumericItemImageDiv" src='/Images/3.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img2' onclick='myFunction(4)' class="NumericItemImageDiv" src='/Images/4.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img3' onclick='myFunction(5)' class="NumericItemImageDiv" src='/Images/5.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img4' onclick='myFunction(6)' class="NumericItemImageDiv" src='/Images/6.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%-- <a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img5' onclick='myFunction(7)' class="NumericItemImageDiv" src='/Images/7.jpg' /></div>
                <%-- </a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img8' onclick='myFunction(8)' class="NumericItemImageDiv" src='/Images/8.jpg' /></div>
                <%-- </a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img9' onclick='myFunction(9)' class="NumericItemImageDiv" src='/Images/9.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img8' onclick='myFunction(99)' class="NumericItemImageDiv" src='/Images/Backspace.jpg' /></div>
                <%-- </a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='ImgOk' onclick='javascript:return PerformUpdateAction()' class="NumericItemImageDiv"
                        src='/Images/Ok.jpg' /></div>
                <%--</a>--%>
            </div>
        </div>
    </div>
    <div id="TouchKeypadForPax1" style="display: none;">
        <div id="TouchKeypadForBearerLogin" style="height: 365px;">
            <div id="TouchKeypadResultDiv1" style="padding-top: 5px;">
                <asp:TextBox ID="txtTouchKeypadResultForPax" runat="server" CssClass="numkb TouchKeypadResult"
                    Height="40px" Font-Size="36px"></asp:TextBox>
            </div>
            <div id="KeyBoardContainer" style="margin-top: 5px;">
            </div>
            <div style="padding-top: 15px;">
                <input type="button" style="height: 38px; width: 99.6%; font-size: 1.5em; font-weight: bold;"
                    class="ui-keyboard-button ui-keyboard-48 ui-state-default ui-corner-all" value="Ok"
                    onclick="PerformUpdateActionForPax()" />
            </div>
        </div>
    </div>
    <div id="SpecialRemarksDiv" style="display: none;">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-12">
                        <asp:TextBox ID="txtSpecialRemarks" runat="server" CssClass="ThreeColumnTextBox"
                            TextMode="MultiLine" TabIndex="1"></asp:TextBox>
                    </div>
                    <span id="OpenKeyboardForTextArea" style="cursor: pointer; cursor: hand;">
                        <img src="../StyleSheet/images/keyboard.png" style="width: 35px; height: 25px;" alt="Keyboard" /></span>
                </div>
                <div class="form-group">
                    <div class="col-md-4">
                        <div class='NumericItemDiv'>
                            <img id='Img19' onclick='javascript:return PerformUpdateActionForSpecialRemarks()'
                                class="NumericItemImageDiv" src='/Images/Ok.jpg' /></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="btnNewBank" class="btn-toolbar;" style="text-align: right;">
        <asp:ImageButton ID="imgBtnInnboardBill" CssClass="btnBackPreviousPage" runat="server"
            ImageUrl="~/Images/RestaurantBill.png" OnClientClick="javascript:return PerformGoToRestaurantBillAction()"
            ToolTip="Restaurant Bill" />
        &nbsp;
        <asp:ImageButton ID="imgBtnHome" CssClass="btnBackPreviousPage" runat="server" ImageUrl="~/Images/Home.png"
            OnClientClick="javascript:return GoToRoomDesign()" ToolTip="Home" />
        &nbsp;&nbsp;
        <asp:ImageButton ID="imgBtnRestaurantCookBook" CssClass="btnBackPreviousPage" runat="server"
            ImageUrl="~/Images/RestaurantCookBook.png" OnClientClick="javascript:return GoToCategoryHome()"
            ToolTip="Category List" />
        &nbsp;&nbsp;
        <asp:ImageButton ID="imgBtnSpecialRemarks" CssClass="btnBackPreviousPage" runat="server"
            ImageUrl="~/StyleSheet/images/SpecialRemarks.png" OnClientClick="javascript:return AddSpecialRemarks()"
            ToolTip="Special Remarks" />
        &nbsp;&nbsp;
        <asp:ImageButton ID="imgBtnPaxInfo" CssClass="btnBackPreviousPage" runat="server"
            ImageUrl="~/StyleSheet/images/PAX_Icon.png" OnClientClick="javascript:return AddPAXInfo()"
            ToolTip="PAX" />
        &nbsp;&nbsp;
        <asp:ImageButton ID="imgItemSearch" CssClass="btnBackPreviousPage" runat="server"
            ImageUrl="~/Images/SearchItem.png" OnClientClick="javascript:return ItemAutoSearch()"
            ToolTip="Item Search" />
        &nbsp;&nbsp;
        <asp:ImageButton ID="btnOrderSubmit" CssClass="btnBackPreviousPage" runat="server"
            ImageUrl="~/StyleSheet/images/process_order_button.gif" OnClick="btnOrderSubmit_Click"
            OnClientClick="javascript:return CheckAddedItem();" />
        &nbsp;&nbsp;
        <asp:ImageButton ID="btnBackPreviousPage" CssClass="btnBackPreviousPage" runat="server"
            ImageUrl="~/StyleSheet/images/backButton.gif" OnClick="btnBackPreviousPage_Click" />
        <div class="btn-group">
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <%--<div class="panel-body">--%>
        <asp:Literal ID="ltlRoomTemplate" runat="server"> </asp:Literal>
        <%--</div>--%>
    </div>
    <%--<asp:Literal ID="ltlRoomTemplate" runat="server"> </asp:Literal>--%>
    <div style="display: none;">
        <asp:TextBox ID="txtBearerIdInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtTableIdInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtTableNumberInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtKotIdInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtCategoryInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtItemIdInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
    </div>
    <div id="Div1" class="panel panel-default">
        <div class="panel-heading">
            Guest Room Information</div>
        <div class="panel-body">
            <asp:HiddenField ID="hfIsItemAdded" runat="server" />
            <div id="ltlTableWiseItemInformation">
            </div>
        </div>
    </div>
    <div id="ItemSearchDialog" style="display: none;">
        <div id="Div5">
            <div style="float: left; padding-bottom: 15px; width: 70px">
                <strong>Item Name</strong>
            </div>
            <div style="float: left; padding-left: 2px; padding-bottom: 15px; width: 390px">
                <input type="text" id="txtItemName" style="width: 385px;" />
            </div>
            <div id="itemDetailsContainer" style="width: 290px">
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var xNewAdd = '<%=isLoadGridInformation%>';
        if (xNewAdd > -1) {
            LoadGridInformationShow();
        }
        else {
            LoadGridInformationHide();
        }
        
    </script>
</asp:Content>
