<%@ Page Title="" Language="C#" MasterPageFile="~/POS/RestaurantMaster.Master"
    AutoEventWireup="true" CodeBehind="frmKotBill.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmKotBill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var alreadySaveKotRemarks = [];
        var kkb = [];

        var time = new Date().getTime();
        $(document.body).bind("mousemove keypress", function (e) {
            time = new Date().getTime();
        });

        function RefreshTable() {
            if ((new Date().getTime() - time) >= 900000) {
                CheckIdleTimeToSignOut();
                setTimeout(RefreshTable, 5000);
            }
            else {
                setTimeout(RefreshTable, 5000);
            }
        }

        setTimeout(RefreshTable, 5000);

        function CheckIdleTimeToSignOut() {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: 'frmKotBill.aspx/CheckIdleTimeToSignOut',
                data: "{'ss':'" + 1 + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d == "0") {
                        window.location = "Login.aspx";
                    }
                },
                error: function (result) {
                    toastr.error(error.get_message());
                }
            });
        }

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>KOT Bill</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#OpenKeyboardForTextArea').click(function () {
                CommonHelper.TouchScreenKeyboard("ContentPlaceHolder1_txtSpecialRemarks");
                var keyboard = $('#ContentPlaceHolder1_txtSpecialRemarks').getkeyboard();
                keyboard.reveal();
            });

            $("#<%=btnOrderSubmit.ClientID %>").click(function () {
                if (confirm('Do you want to Submit Order?')) {
                    PageMethods.GetIsLocalNetworkRServerPrint(OnLoadNetworkPrintingSucceeded, OnNetworkPrintingFailed);
                }
                return false;
            });

            $("#btnItemSpecialRemarks").click(function () {
                $("#TableInfoActionDecider").dialog('close');
                var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();
                var itemId = $("#<%=txtItemId.ClientID %>").val();

                PageMethods.GetSpecialRemarksDetails(kotId, itemId, OnGetSpecialRemarksDetailsSucceed, OnGetSpecialRemarksDetailsFailed);
            });

            $("#btnUpdateTableInfo").click(function () {
                $("#TableInfoActionDecider").dialog("close");
                popup(1, 'TouchKeypad', '', 412, 445);
            });

            $("#btnDeleteTableInfo").click(function () {
                var kotDetailsId = $("#<%=txtKotDetailsIdInformation.ClientID %>").val();
                var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();
                var itemId = $("#<%=txtItemId.ClientID %>").val();
                $("#TableInfoActionDecider").dialog("close");
                PerformDeleteAction(kotDetailsId, kotId, itemId);
            });

            $("#btnColseDeciderWidow").click(function () {
                $("#TableInfoActionDecider").dialog("close");
            });

            $("#btnItemwiseSpecialRemarksCancel").click(function () {
                $("#ItemWiseSpecialRemarks").dialog("close");
            });

            $("#btnItemwiseSpecialRemarksSave").click(function () {

                var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();
                var itemId = $("#<%=txtItemId.ClientID %>").val();
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

                var kotDetailId = $("#hfKotDetailId").val();

                PageMethods.SaveKotSpecialRemarks(RKotSRemarksDetail, RKotSRemarksDetailDelete, kotDetailId, OnSaveKotSpecialRemarksSuccedd, OnSaveKotSpecialRemarksFailed);
            });

        });

        function OnSaveKotSpecialRemarksSuccedd(result) {
            if (result.AlertMessage.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#<%=txtItemId.ClientID %>").val("");
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
            toastr.error(error.get_message());
        }

        var vvc = [];

        function OnGetSpecialRemarksDetailsSucceed(result) {

            vvc = result;
            alreadySaveKotRemarks = result.KotRemarks;

            var table = "", tr = "", td = "", i = 0, alreadyChecked = "";
            var specialRemarksLength = result.ItemSpecialRemarks.length;

            table = "<div id='no-more-tables'> ";
            table += "<table cellspacing=\"0\" cellpadding=\"4\" id=\"TableWiseItemRemarksInformation\" style=\"margin:0;\" >";
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
                     "<td data-title='Select' align=\"center\" style=\"width: 30px\">" +
                     "&nbsp;<input type=\"checkbox\" value=\"" + result.ItemSpecialRemarks[i].SpecialRemarksId + "\" " + alreadyChecked + " id=\"ch" + result.ItemSpecialRemarks[i].SpecialRemarksId + "\">" +
                      "</td>" +
                      "<td data-title='Remarks' align=\"left\" style=\"width: 200px\">" +
                       result.ItemSpecialRemarks[i].SpecialRemarks +
                      "</td>";

                tr += td + "</tr> </div>";

                table += tr;
            }
            table += " </tbody> </table>";

            $("#remarksContainer").html(table);

            $("#ItemWiseSpecialRemarks").dialog({
                autoOpen: true,
                modal: true,
                maxWidth: 500,
                width: 'auto',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Item Special Remarks",
                show: 'slide'
            });
        }

        function OnGetSpecialRemarksDetailsFailed(error) {
            toastr.error(error.get_message());
        }

        function OnLoadNetworkPrintingSucceeded(result) {

            if (result) {
                //popup(1, 'serverOrLocalPrinting', '', 300, 200);
                $('#btnPrintFromServer').trigger('click');
            }
            else {
                $('#btnKotPrintPreview').trigger('click');
                return true;
            }

        }
        function OnNetworkPrintingFailed(error) {
            toastr.error(error.get_message());
        }

        function GoToTableDesign() {
            window.location = "frmKotBill.aspx?Kot=TableAllocation";
            return false;
        }

        function GoToCategoryHome() {
            window.location = "frmKotBill.aspx?Kot=RestaurantItemCategory:0";
            return false;
        }
        function AddSpecialRemarks() {
            $("#<%=txtSpecialRemarks.ClientID %>").val();
            var viewPortWidth = $(window).width();
            var dialogWidth = 500;

            if (viewPortWidth < 800) {
                dialogWidth = viewPortWidth;
            }

            $("#SpecialRemarksDiv").dialog({
                width: dialogWidth,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                height: 'auto',
                //position: { my: 'left', at: 'left' },
                title: "Special Remarks",
                show: 'slide'
            });

            return false;
        }
        function AddNewItem(kotDetailId, kotId, itemId, isAlreadySubmitted) {
            $("#<%=txtKotDetailsIdInformation.ClientID %>").val(kotDetailId);
            $("#<%=txtKotIdInformation.ClientID %>").val(kotId);
            $("#<%=txtItemId.ClientID %>").val(itemId);
            $("#hfKotDetailId").val(kotDetailId);

            if (isAlreadySubmitted == "1") {
                $("#deleteTableInfoDiv").hide();
            }
            else {
                $("#deleteTableInfoDiv").show();
            }

            $("#TableInfoActionDecider").dialog({
                width: 'auto',
                maxWidth: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                height: 'auto',
                //position: { my: 'left', at: 'left' },
                title: "Option Decider",
                show: 'slide'
            });

            return false;
        }
        function AddPAXInfo(val) {
            $("#<%=txtItemIdInformation.ClientID %>").val(val);
            popup(1, 'TouchKeypadForPax', '', 412, 445);
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
        //---Update Data---------
        function PerformUpdateActionForPax() {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var itemQuantity = $("#<%=txtTouchKeypadResultForPax.ClientID %>").val();
            var kotDetailsId = $("#hfKotDetailId").val();
            PageMethods.UpdateTablePaxInformation(hfCostCenterIdVal, itemQuantity, OnUpdateObjectSucceededForPax, OnUpdateObjectFailedForPax);
            return false;
        }
        function OnUpdateObjectSucceededForPax(result) {
            popup(-1);
            return false;
        }

        function OnUpdateObjectFailedForPax(error) {
            toastr.error(error.get_message());
        }
        //function PerformUpdateAction() {
        function PerformUpdateAction(updateType, updatedContent) {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var selectedId = $("#<%=txtKotDetailsIdInformation.ClientID %>").val();
            var itemQuantity = $("#<%=txtTouchKeypadResult.ClientID %>").val();
            var updatedContent = itemQuantity;
            var updateType = "QuantityChange"

            //PageMethods.UpdateIndividualItemDetailInformation(hfCostCenterIdVal, selectedId, itemQuantity, OnUpdateObjectSucceeded, OnUpdateObjectFailed);
            PageMethods.UpdateIndividualItemDetailInformation(updateType, hfCostCenterIdVal, selectedId, itemQuantity, updatedContent, OnUpdateObjectSucceeded, OnUpdateObjectFailed);
            return false;
        }
        function OnUpdateObjectSucceeded(result) {
            $("#<%=txtKotDetailsIdInformation.ClientID %>").val('');
            $("#<%=txtTouchKeypadResult.ClientID %>").val('');
            popup(-1);
            LoadGridInformation();
            return false;
        }
        function OnUpdateObjectFailed(error) {
            toastr.error(error.get_message());
        }
        function PerformUpdateActionForSpecialRemarks() {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var specialRemarks = $("#<%=txtSpecialRemarks.ClientID %>").val();
            PageMethods.UpdateTableSpecialRemarksInformation(hfCostCenterIdVal, specialRemarks, OnUpdateObjectSucceededForSpecialRemarks, OnUpdateObjectFailedForSpecialRemarks);
            return false;
        }
        function OnUpdateObjectSucceededForSpecialRemarks(result) {
            $("#SpecialRemarksDiv").dialog('close');
            return false;
        }

        function OnUpdateObjectFailedForSpecialRemarks(error) {
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
            var keyboard = $('#txtItemName').getkeyboard();

            if (keyboard === undefined) { }
            else { keyboard.close(); }

            if ($("#ItemSearchDialog").is(":visible"))
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
        function PerformDeleteAction(kotDetailsId, actionId, itemId) {

            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                CommonHelper.ExactMatch();
                var tr = $("#TableWiseItemInformation tbody tr").find("td:eq(5):textEquals('" + itemId + "')").parent();
                var selectedItemQty = "0";
                if ($(tr).length != 0) {
                    selectedItemQty = $(tr).find("td:eq(1)").text();
                }

                var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
                //var kotDetailsId = $("#hfKotDetailId").val();

                PageMethods.UpdateIndividualItemDetailInformation('QuantityChange', hfCostCenterIdVal, kotDetailsId, selectedItemQty, 0, OnUpdateObjectSucceeded, OnUpdateObjectFailed);

                //PageMethods.DeleteData(kotDetailsId, actionId, itemId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                LoadGridInformation();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
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
            LoadGridInformation();
            return false;
        }

        function OnSaveBuffetObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function ItemAutoSearch() {
            $("#txtItemName").val('');
            $("#itemDetailsContainer").html('');

            var viewPortWidth = $(window).width();
            var dialogWidth = 500;

            if (viewPortWidth < 800) {
                dialogWidth = viewPortWidth;
            }

            $("#ItemSearchDialog").dialog({
                autoOpen: true,
                modal: true,
                width: dialogWidth,
                height: 280,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Item Search & Add",
                show: 'slide'
            });

            CommonHelper.TouchScreenKeyboard("txtItemName");
            $("#txtItemName").autocomplete({
                source: function (request, response) {

                    var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../POS/frmKotBillMaster.aspx/ItemSearch',
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
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);

                    var itemDiv = "";
                    itemDiv += "<figure> ";
                    itemDiv += "<img src=\"" + ui.item.ImagePath + "\" onclick=\"return PerformAction(" + ui.item.value + ",'" + ui.item.label + "');\" id=\"img" + ui.item.value + "\" class=\"ItemImageSize\">" +
                                  "<figcaption> " + ui.item.label + " </figcaption> </figure>";

                    $("#itemDetailsContainer").html(itemDiv);
                }
            }).addAutocomplete();
            var keyboard = $('#txtItemName').getkeyboard();
            keyboard.reveal();

            return false;
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

    </script>
    <input type="hidden" id="hfKotDetailId" value="0" />
    <asp:HiddenField ID="hfIsItemAdded" runat="server" />
    <!--Popup div container  ends-->
    <div id="TouchKeypadForPax" style="display: none;">
        <div id="TouchKeypadResultDivForPax">
            <asp:TextBox ID="txtTouchKeypadResultForPax" runat="server" CssClass="TouchKeypadResult"
                Height="40px" Font-Size="50px" MaxLength="3"></asp:TextBox>
        </div>
        <div class='divClear'>
        </div>
        <div class='block-body collapse in'>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img6' onclick='myFunctionForPax(0)' class="NumericItemImageDiv" src="/Images/0.jpg" /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img7' onclick='myFunctionForPax(1)' class="NumericItemImageDiv" src='/Images/1.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img10' onclick='myFunctionForPax(2)' class="NumericItemImageDiv" src='/Images/2.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img11' onclick='myFunctionForPax(3)' class="NumericItemImageDiv" src='/Images/3.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img12' onclick='myFunctionForPax(4)' class="NumericItemImageDiv" src='/Images/4.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img13' onclick='myFunctionForPax(5)' class="NumericItemImageDiv" src='/Images/5.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img14' onclick='myFunctionForPax(6)' class="NumericItemImageDiv" src='/Images/6.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img15' onclick='myFunctionForPax(7)' class="NumericItemImageDiv" src='/Images/7.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img16' onclick='myFunctionForPax(8)' class="NumericItemImageDiv" src='/Images/8.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img17' onclick='myFunctionForPax(9)' class="NumericItemImageDiv" src='/Images/9.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img18' onclick='myFunctionForPax(99)' class="NumericItemImageDiv" src='/Images/Backspace.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='ImgOkForPax' onclick='javascript:return PerformUpdateActionForPax()' class="NumericItemImageDiv"
                        src='/Images/Ok.jpg' /></div>
            </div>
        </div>
    </div>
    <div id="TouchKeypad" style="display: none;">
        <div id="TouchKeypadResultDiv">
            <asp:HiddenField ID="hfCostCenterId" runat="server"></asp:HiddenField>
            <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtTouchKeypadResult" runat="server" CssClass="TouchKeypadResult"
                Height="40px" Font-Size="50px"></asp:TextBox>
        </div>
        <div class='divClear'>
        </div>
        <div class='block-body collapse in'>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='ContentPlaceHolder1_img1' onclick='myFunction(0)' class="NumericItemImageDiv"
                        src="/Images/0.jpg" /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='ContentPlaceHolder1_img2' onclick='myFunction(1)' class="NumericItemImageDiv"
                        src='/Images/1.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='ContentPlaceHolder1_img3' onclick='myFunction(2)' class="NumericItemImageDiv"
                        src='/Images/2.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img1' onclick='myFunction(3)' class="NumericItemImageDiv" src='/Images/3.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img2' onclick='myFunction(4)' class="NumericItemImageDiv" src='/Images/4.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img3' onclick='myFunction(5)' class="NumericItemImageDiv" src='/Images/5.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img4' onclick='myFunction(6)' class="NumericItemImageDiv" src='/Images/6.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img5' onclick='myFunction(7)' class="NumericItemImageDiv" src='/Images/7.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img8' onclick='myFunction(8)' class="NumericItemImageDiv" src='/Images/8.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img9' onclick='myFunction(9)' class="NumericItemImageDiv" src='/Images/9.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='Img8' onclick='myFunction(99)' class="NumericItemImageDiv" src='/Images/Backspace.jpg' /></div>
            </div>
            <div class='DivNumericContainer'>
                <div class='NumericItemDiv'>
                    <img id='ImgOk' onclick='javascript:return PerformUpdateAction()' class="NumericItemImageDiv"
                        src='/Images/Ok.jpg' /></div>
            </div>
        </div>
    </div>
    <div id="SpecialRemarksDiv" style="display: none;">
        <div class="row-fluid">
            <div class="span10" style="padding-left: 2px; padding-top: 2px;">
                <asp:TextBox ID="txtSpecialRemarks" runat="server" CssClass="span12" TextMode="MultiLine"
                    TabIndex="1"></asp:TextBox>
            </div>
            <div class="span2">
                <span id="OpenKeyboardForTextArea" style="cursor: pointer; cursor: hand;">
                    <img src="../StyleSheet/images/keyboard.png" style="width: 35px; height: 25px;" alt="Keyboard" /></span>
            </div>
            <div class="divClear">
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <div class='NumericItemDiv'>
                        <img id='Img19' onclick='javascript:return PerformUpdateActionForSpecialRemarks()'
                            class="NumericItemImageDiv" src='/Images/Ok.jpg' /></div>
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <div class="row-fluid" style="margin: 0px; padding: 0px;">
        <div class="span12" style="margin: 0px; padding: 0px;">
            <div id="btnNewBank" class="btn-toolbar;" style="text-align: right; margin-top: 2px;
                margin-bottom: 2px;">
                <asp:ImageButton ID="imgBtnTableDesign" CssClass="btnBackPreviousPage" runat="server"
                    ImageUrl="~/Images/TableDesign.png" OnClientClick="javascript:return GoToTableDesign()"
                    ToolTip="Table Design" />
                &nbsp;
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
                &nbsp&nbsp;
                <asp:ImageButton ID="btnOrderSubmit" CssClass="btnBackPreviousPage" runat="server"
                    ImageUrl="~/StyleSheet/images/process_order_button.gif" ToolTip="Order Submit"
                    OnClientClick="javascript:return CheckAddedItem();" />
                &nbsp;&nbsp;
                <asp:ImageButton ID="btnBackPreviousPage" CssClass="btnBackPreviousPage" runat="server"
                    ImageUrl="~/StyleSheet/images/backButton.gif" OnClick="btnBackPreviousPage_Click"
                    ToolTip="Back" />
                &nbsp;
                <div class="btn-group">
                </div>
            </div>
        </div>
    </div>
    <div class=" row-fluid block" style="overflow: hidden; margin: 0px; padding: 0px;
        margin-top: 2px; margin-bottom: 5px; border: 0px;">
        <div class="span12" style="margin: 0px; padding: 0px;">
            <asp:Literal ID="ltlRoomTemplate" runat="server"> </asp:Literal>
        </div>
    </div>
    <div style="display: none;">
        <asp:TextBox ID="txtBearerIdInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtTableIdInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtTableNumberInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtKotIdInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtKotDetailsIdInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtCategoryInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtItemIdInformation" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
        <asp:TextBox ID="txtItemId" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
    </div>
    <div id="serverOrLocalPrinting" style="display: none;">
        <input type="hidden" id="Hidden1" value="0" />
        <input type="hidden" id="Hidden2" value="" />
        <div id="Div3">
            <a data-toggle="collapse" class="block-heading" href="#page-stats">Network or Server
                Print Decider</a>
            <div style="padding: 10px">
                <div style="float: left; padding-left: 10px; padding-bottom: 15px; width: 150px">
                    <asp:Button ID="btnPrintFromServer" runat="server" Text="Print" CssClass="TransactionalButton btn btn-primary"
                        Width="150px" ClientIDMode="Static" OnClick="btnPrintFromServer_Click" OnClientClick="javascript:return CheckAddedItem();" />
                </div>
                <div style="float: left; padding-left: 10px; padding-bottom: 15px; width: 150px">
                    <asp:Button ID="btnKotPrintPreview" runat="server" Text="Preview" CssClass="TransactionalButton btn btn-primary"
                        Width="150px" ClientIDMode="Static" OnClick="btnKotPrintPreview_Click" />
                </div>
            </div>
            <div id="Div7" class="alert alert-info" style="display: none;">
                <button type="button" class="close" data-dismiss="alert">
                    ×</button>
                <asp:Label ID='Label1' Font-Bold="True" runat="server" ClientIDMode="Static"></asp:Label>
            </div>
        </div>
    </div>
    <div id="TableInfoActionDecider" style="display: none;">
        <div class="row-fluid">
            <div class="span12">
                <div class="row-fluid" style="margin-bottom: 15px; margin-top: 15px;">
                    <div class="span6">
                        <input type="button" id="btnItemSpecialRemarks" class="TransactionalButton btn btn-primary"
                            style="width: 150px;" value="Special Remarks" />
                    </div>
                </div>
                <div class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                        <input type="button" id="btnUpdateTableInfo" class="TransactionalButton btn btn-primary"
                            style="width: 150px;" value="Edit" />
                    </div>
                </div>
                <div id="deleteTableInfoDiv" class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                        <input type="button" id="btnDeleteTableInfo" class="TransactionalButton btn btn-primary"
                            style="width: 150px;" value="Delete" />
                    </div>
                </div>
                <div class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                        <input type="button" id="btnColseDeciderWidow" class="TransactionalButton btn btn-primary"
                            style="width: 150px;" value="Close" />
                    </div>
                </div>
                <div id="Div2" class="alert alert-info" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert">
                        ×</button>
                    <asp:Label ID='Label2' Font-Bold="True" runat="server" ClientIDMode="Static"></asp:Label>
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <div id="ItemWiseSpecialRemarks" style="width: 350px; display: none;">
        <div class="">
            <div class="DataGridYScroll">
                <div id="remarksContainer" style="width: 100%;">
                </div>
            </div>
            <div id="Div4" style="padding-left: 5px; width: 100%;">
                <div style="float: left; padding-bottom: 15px;">
                    <input type="button" id="btnItemwiseSpecialRemarksSave" class="TransactionalButton btn btn-primary"
                        style="width: 150px;" value="Save" />
                    <input type="button" id="btnItemwiseSpecialRemarksCancel" class="TransactionalButton btn btn-primary"
                        style="width: 150px;" value="Cancel" />
                </div>
                <div id="ItemWiseSpecialRemarksContainer" class="alert alert-info" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert">
                        ×</button>
                    <asp:Label ID='ItemWiseSpecialRemarksMessage' Font-Bold="True" runat="server" ClientIDMode="Static"></asp:Label>
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <div id="ItemSearchDialog" style="display: none;">
        <div class="row-fluid">
            <div class="span12">
                <div class="span2">
                    <strong>Item Name</strong>
                </div>
                <div class="span10">
                    <input type="text" id="txtItemName" style="width: 385px;" />
                </div>
            </div>
            <div class="row-fluid">
                <div class="span12">
                    <div id="itemDetailsContainer" style="text-align: center;">
                    </div>
                </div>
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
