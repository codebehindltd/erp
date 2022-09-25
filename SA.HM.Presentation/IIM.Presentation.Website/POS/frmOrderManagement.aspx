<%@ Page Title="" Language="C#" MasterPageFile="~/POS/RestaurantMM.Master"
    AutoEventWireup="true" CodeBehind="frmOrderManagement.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmOrderManagement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="TopHeaderContent" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div style="font-size: 18px; font-weight: bold; text-align: center; color: #fff; padding-right: 5px; vertical-align: middle; padding-top: 9px;">
        <asp:Label ID="lblOrderSourceName" runat="server" Text=""></asp:Label>:
        <asp:Label ID="lblOrderSourceNumber" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblAddedOrderSourceNumber" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .dataTables_scroll {
            overflow: auto;
        }
    </style>

    <script type="text/javascript">
        var userIdAuthorised = "";
        var userIdAuthorisedPassword = "";
        var tst = [], deletedItemList = new Array(), deletedNewItemList = [];
        var RecipeTable, NewRecipeTable;
        var newItemList = [];
        var typesList = [], previousRecipeTypesList = [];
        var vTouchId = 0;
        var vTouchControl = "";
        var IsBillOnProcess = "0";
        var IsBillOnPreview = "0";
        var IsBillOnSplit = "0";
        var KotWiseServiceChargeVatNOther = {};

        var AddedTableList = new Array();
        var DeletedTableList = new Array();

        var AddedClassificationList = new Array();
        var UpdatedClassificationList = new Array();
        var DeletedClassificationList = new Array();

        var FocusedInputControl = "";

        var AddedPromotionalDiscountList = new Array();
        var DeletedPromotionalDiscountList = new Array();
        var AlreadySaveKotItemWiseRemarks = null;

        document.addEventListener("contextmenu", function (e) {
            e.preventDefault();
        }, false);

        $(document).ready(function () {

            CommonHelper.ApplyDecimalValidation();

            var status = <% = this.IsDiscountEnable%>;
            if(status == true)
            {
                $('#discountApplicableDiv').show();
            }
            else
            {
                $('#discountApplicableDiv').hide();
            }


            $(document).bind("keydown", Disable_FuntionKey);

            CommonHelper.ApplyDecimalValidation();
            CommonHelper.ApplyIntValidation();
            $(function () {
                $("#myTabsPopUp").tabs();
            });

            $("#myTabs").tabs();

            //$("#ContentPlaceHolder1_ddlIngredient").select2({
            //    tags: "true",
            //    placeholder: "--- Please Select ---",
            //    allowClear: true,
            //    width: "99.75%"
            //});

            var sourceType = $.trim(CommonHelper.GetParameterByName("st"));
            if (sourceType == "tkn") {
                $(".tblOrder").hide();
                $(".tknOrder").show();
                $("#imgRoomWiseGuest").hide();
            }
            else if (sourceType == "tbl") {
                $(".tblOrder").show();
                $(".tknOrder").hide();
                $("#imgRoomWiseGuest").hide();

                if ($("#ContentPlaceHolder1_hfRoomId").val() != "") {
                    $("#lblGuestRoom").text("Room# " + $("#ContentPlaceHolder1_hfRoomNumber").val());
                }
            }
            else if (sourceType == "rom") {
                $("#lblGuestRoom").text("Room# " + $("#ContentPlaceHolder2_lblOrderSourceNumber").text());
                $("#imgClearRoomWiseGuest").hide();

                $("#btnBillHolpup").hide();
            }

            if ($("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "" || $("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "0") {
                $("#ContentPlaceHolder1_txtTPDiscountAmount").trigger("change");
            }

            if ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() == "0" || $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() == "") {
                $("#ContentPlaceHolder1_btnClear").attr("disabled", false);
            }
            else {
                if ($("#ContentPlaceHolder1_hfIsResumeBill").val() == "1")
                    $("#ContentPlaceHolder1_btnClear").attr("disabled", true);
                else
                    $("#ContentPlaceHolder1_btnClear").attr("disabled", false);
            }

            if ($("#ContentPlaceHolder1_hfAddedTableIdForBill").val() != "") {
                AddedTableList = JSON.parse($("#ContentPlaceHolder1_hfAddedTableIdForBill").val());
            }

            if ($("#ContentPlaceHolder1_hfAddedClassificationId").val() != "") {
                AddedClassificationList = JSON.parse($("#ContentPlaceHolder1_hfAddedClassificationId").val());
            }

            if ($("#ContentPlaceHolder1_hfIsVatEnable").val() == "1") {
                $("#ContentPlaceHolder1_cbTPVatAmount").prop("checked", true);
            }
            else {
                $("#ContentPlaceHolder1_cbTPVatAmount").prop("checked", false);
            }

            if ($("#ContentPlaceHolder1_hfIsServiceChargeEnable").val() == "1") {
                $("#ContentPlaceHolder1_cbTPServiceCharge").prop("checked", true);
            }
            else {
                $("#ContentPlaceHolder1_cbTPServiceCharge").prop("checked", false);
            }

            if ($("#ContentPlaceHolder1_hfIsSDChargeEnable").val() == "1") {
                $("#ContentPlaceHolder1_cbTPSDCharge").prop("checked", true);
            }
            else {
                $("#ContentPlaceHolder1_cbTPSDCharge").prop("checked", false);
            }

            if ($("#ContentPlaceHolder1_hfIsAdditionalChargeEnable").val() == "1") {
                $("#ContentPlaceHolder1_cbTPAdditionalCharge").prop("checked", true);
            }
            else {
                $("#ContentPlaceHolder1_cbTPAdditionalCharge").prop("checked", false);
            }

            if (AddedClassificationList.length > 0) {
                $("#ContentPlaceHolder1_txtTPDiscountAmount").attr("disabled", true);
            }

            if ($("#ContentPlaceHolder1_hfBearerCanSettleBill").val() == "1") {

                $("#BillSettlementBtnContainer").show();
                $("#ContentPlaceHolder1_btnPaymentInfo").show();
                $("#btnBillSplit").show();

                if (sourceType != 'rom' && sourceType != 'tkn')
                    $("#btnAddMoreTable").show();
                else
                    $("#btnAddMoreTable").hide();
            }
            else {
                $("#BillSettlementBtnContainer").hide();
                $("#ContentPlaceHolder1_btnPaymentInfo").hide();
                $("#btnBillSplit").hide();

                if (sourceType != 'rom' && sourceType != 'tkn')
                    $("#btnAddMoreTable").show();
                else
                    $("#btnAddMoreTable").hide();
            }

            if ($("#ContentPlaceHolder1_hfIsBearar").val() == "1") {
                $("#btnWaiterChange").hide();
                $("#btnTableOrderPriview").hide();

                if ($("#ContentPlaceHolder1_hfBearerCanSettleBill").val() == "1") {
                    $("#btnBillPreview").show();
                }
                else {
                    if ($("#ContentPlaceHolder1_hfIsBillPreviewButtonEnable").val() == "0" &&
                        ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "" && $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "0")
                    ) {
                        $("#btnBillPreview").hide();
                    }
                    else {
                        $("#btnBillPreview").show();
                    }
                }
            }
            else {
                $("#btnWaiterChange").show();
                $("#btnTableOrderPriview").show();
                $("#btnBillPreview").show();
            }

            if ($("#ContentPlaceHolder1_hfIsItemSearchEnable").val() == "1") {
                $("#ItemSearchContainer").show();
            }
            else {
                $("#ItemSearchContainer").hide();
            }

            if ($("#ContentPlaceHolder1_hfIsRestaurantIntegrateWithFrontOffice").val() == "1") {
                $("#RoomPaymentDiv").show();

                if ($("#ContentPlaceHolder1_hfIsStopChargePosting").val() == "1") {
                    $("#RoomPaymentDiv").hide();
                }
                else {
                    $("#RoomPaymentDiv").show();
                }
            }
            else {
                $("#RoomPaymentDiv").hide();
            }

            if ($("#ContentPlaceHolder1_hfIsRestaurantIntegrateWithPayroll").val() == "1") {
                $("#EmployeePaymentContainer").show();
            }
            else {
                $("#EmployeePaymentContainer").hide();
            }

            if ($("#ContentPlaceHolder1_hfIsRestaurantIntegrateWithCompany").val() == "1") {
                $("#CompanyPaymentContainer").show();
            }
            else {
                $("#CompanyPaymentContainer").hide();
            }

            if ($("#ContentPlaceHolder1_hfIsRestaurantIntegrateWithMember").val() == "1") {
                $("#MemberPaymentContainer").show();
            }
            else {
                $("#MemberPaymentContainer").hide();
            }

            $('#ContentPlaceHolder1_pnlHomeButtonInfo').hide();
            $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
            $("#ContentPlaceHolder1_btnBillHolpup").attr("disabled", true);
            $("#ContentPlaceHolder1_btnOrderSubmit").attr("disabled", true);

            if ($("#ContentPlaceHolder1_hfIsResumeBill").val() == "1") {
                $("#billPreviewForBill").show();
            }
            else {
                $("#billPreviewForBill").hide();
            }

            $("#btnItemSpecialRemarks").click(function () {

                $("#ItemWiseRecipeList").hide();
                var kotId = $("#ContentPlaceHolder1_hfKotId").val();
                var itemId = $("#ContentPlaceHolder1_hfItemId").val();
                var isPredefinedRemarksEnable = $("#ContentPlaceHolder1_hfIsPredefinedRemarksEnable").val();

                if (isPredefinedRemarksEnable == "1") {
                    CommonHelper.SpinnerOpen();
                    $("#ItemWiseSpecialRemarks").show();
                    $("#ItemWiseRemarks").hide();
                    PageMethods.GetSpecialRemarksDetails(kotId, itemId, OnGetSpecialRemarksDetailsSucceed, OnGetSpecialRemarksDetailsFailed);
                }
                else {
                    PageMethods.GetSpecialRemarksDetails(kotId, itemId, OnGetSpecialRemarksDetailsSucceed, OnGetSpecialRemarksDetailsFailed);
                    $("#ItemWiseRemarks").show();
                    $("#ItemWiseSpecialRemarks").hide();
                }
            });

            $("#ContentPlaceHolder1_ddlIngredient").change(function () {

                //debugger;
                var IngredientId = parseInt($("#ContentPlaceHolder1_ddlIngredient").val().trim());
                var itemId = $("#ContentPlaceHolder1_hfItemId").val();

                PageMethods.GetPreviousRecipeModifierTypes(IngredientId, itemId, OnLoadGetPreviousRecipeModifierTypesSucceed, OnLoadGetPreviousRecipeModifierTypesFailed);


                return false;

            });

            NewRecipeTable = $("#tblNewRecipe").DataTable({
                data: [],
                columns: [
                    { title: "Ingredient", "data": "RecipeItemName", sWidth: '40%' },
                    { title: "Quantity", "data": "HeadName", sWidth: '30%' },
                    { title: "Cost", "data": "ItemCost", sWidth: '20%' },
                    { title: "Action", "data": null, sWidth: '10%' },
                    { title: "ItemId", "data": "RecipeItemId", visible: false },
                    { title: "TypeId", "data": "TypeId", visible: false },
                    { title: "RecipeId", "data": "RecipeId", visible: false }
                ],
                columnDefs: [


                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    $('td:eq(3)', nRow).html("&nbsp;&nbsp;<a href='javascript:void();'  onclick= 'NewDeleteItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>");
                },
                filter: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                scrollCollapse: false,
                language: {
                    emptyTable: " No data in the table. "
                }
            });


            RecipeTable = $("#tblRecipe").DataTable({
                data: [],
                columns: [
                    { title: "Ingredient", "data": "Name", sWidth: '40%' },
                    { title: "Quantity", "data": "RecipeModifierTypes", sWidth: '30%' },
                    { title: "Cost", "data": "ItemCost", sWidth: '20%' },
                    { title: "Action", "data": null, sWidth: '10%' },
                    { title: "", "data": "ItemId", visible: false },
                    { title: "", "data": "RecipeId", visible: false },
                    { title: "", "data": "PreviousTypeId", visible: false }
                ],
                columnDefs: [

                    {
                        "targets": 1,
                        "render": function (data, type, full, meta) {

                            var dropDownString = '<select class="form-control" onchange="ExistingRecipeModifierTypesChange(this)">';

                            for (var count = 0; count < data.length; count++) {

                                dropDownString += '<option value="' + data[count].TypeId + '" ';
                                dropDownString += '>' + data[count].HeadName + '</option>';
                            }
                            dropDownString += '</select > ';


                            return dropDownString;
                        }
                    }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }

                    // debugger;
                    $('td:eq(1) select', nRow).val(aData.PreviousTypeId);

                    $('td:eq(3)', nRow).html("&nbsp;&nbsp;<a href='javascript:void();'  onclick= 'DeleteItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>");
                },
                filter: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                scrollCollapse: false,
                language: {
                    emptyTable: "No Data Found"
                },
                bJQueryUI: true,
                sScrollXInner: "100%",
                fnInitComplete: function () {
                    this.css("visibility", "visible");
                },
                bScrollAutoCss: true
            });

            jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');

            $("#btnEditRecipe").click(function () {
                $("#ItemWiseSpecialRemarks").hide();
                $("#OpenItemNameChangeDialog").hide();
                $("#QuantityEditDialog").hide();
                var itemId = $("#ContentPlaceHolder1_hfItemId").val();
                var kotId = $("#ContentPlaceHolder1_hfKotId").val();

                CommonHelper.SpinnerOpen();
                $("#ItemWiseRecipeList").show();
                PageMethods.GetItemRecipeList(itemId, kotId, OnGetItemRecipeDetailsSucceed, OnGetItemRecipeDetailsFailed);
                return false;

            });

            $("#btnDeleteTableInfo").click(function () {

                $("#ItemWiseSpecialRemarks").hide();
                $("#OpenItemNameChangeDialog").hide();
                $("#QuantityEditDialog").hide();

                $("#ContentPlaceHolder1_hfIsItemEditOrDelete").val("DeleteItem");

                var kotDetailsId = $("#ContentPlaceHolder1_hfKotDetailId").val();
                var kotId = $("#ContentPlaceHolder1_hfKotId").val();
                var itemId = $("#ContentPlaceHolder1_hfItemId").val();
                var isAlreadySubmitted = $("#ContentPlaceHolder1_hfIsOrderSubmit").val();

                if (isAlreadySubmitted == "1") {
                    if ($("#ContentPlaceHolder1_hfIsItemCanEditDelete").val() == "1") {
                        $("#btnDeleteTableInfo").attr("disabled", false);
                    }
                    else {

                        if ($("#ContentPlaceHolder1_hfIsAccessVarified").val() == "0") {

                            $("#txtUserId").val("");
                            $("#txtUserPassword").val("");

                            CommonHelper.TouchScreenUserKeyboard("accessvarification", "KeyBoardVarificationContainer");
                            var keyboard = $('.accessvarification').getkeyboard();
                            keyboard.reveal();

                            $("#txtUserId_keyboard").css("top", "0px");

                            $("#AccessPermissionForEditDelete").dialog({
                                width: 900,
                                height: 500,
                                autoOpen: true,
                                modal: true,
                                closeOnEscape: true,
                                resizable: false,
                                fluid: true,
                                close: CloseAccessPermissionForEditDelete,
                                title: "Access Varification",
                                show: 'slide'
                            });

                            //toastr.info("Delete -->  " + $("#ContentPlaceHolder1_hfIsAccessVarified").val());
                            return false;
                        }
                    }
                }

                //toastr.info("Delete Access Varified-->  " + $("#ContentPlaceHolder1_hfIsAccessVarified").val());

                CommonHelper.SpinnerOpen();

                $.confirm({
                    title: 'Confirm!',
                    content: 'Do you want to Delete ?',
                    buttons: {
                        confirm: function () {
                            PerformDeleteAction(kotDetailsId, kotId, itemId, isAlreadySubmitted);
                            PageMethods.DeleteRecipeDetailsAndUpdateDefaultPrice(kotId, itemId, OnDeleteRecipeDetailsAndUpdateDefaultPriceSucceeded, OnDeleteRecipeDetailsAndUpdateDefaultPriceFailed);

                            $("#ContentPlaceHolder1_hfIsAccessVarified").val("0");

                            if ($("#ContentPlaceHolder1_hfIsUserHasEditDeleteAccess").val() == "0")
                                $("#ContentPlaceHolder1_hfIsItemCanEditDelete").val("0");

                        },
                        cancel: function () {
                            if ($("#ContentPlaceHolder1_hfIsUserHasEditDeleteAccess").val() == "0")
                                $("#ContentPlaceHolder1_hfIsItemCanEditDelete").val("0");
                        }
                    }
                });

                CommonHelper.SpinnerClose();
            });

            $("#btnAccessVarification").click(function () {
                $("#AccessPermissionForEditDelete").dialog("close");

                var keyboardnum = $('.accessvarification').getkeyboard();
                keyboardnum.destroy();

                var userid = "", password = "";

                userid = $("#txtUserId").val();
                password = $("#txtUserPassword").val();

                UserAccessVarification(userid, password).done(function (response) {

                    if ($("#ContentPlaceHolder1_hfIsItemEditOrDelete").val() == "QuantityChange") {
                        $("#btnUpdateTableInfo").trigger('click');
                    }
                    else if ($("#ContentPlaceHolder1_hfIsItemEditOrDelete").val() == "UnitPriceChange") {
                        $("#btnUpdateItemPrice").trigger('click');
                    }
                    else if ($("#ContentPlaceHolder1_hfIsItemEditOrDelete").val() == "DeleteItem") {
                        $("#btnDeleteTableInfo").trigger('click');
                    }
                });

            });

            $("#btnColseDeciderWidow").click(function () {
                OptionDeciderDialogClose();
            });

            LoadGridInformation();

            $("#ContentPlaceHolder1_rbTPFixedDiscount").click(function () {
                if ($(this).is(":checked")) {

                    if (AddedClassificationList.length > 0) {
                        $("#ContentPlaceHolder1_rbTPFixedDiscount").prop("checked", false);
                        $("#ContentPlaceHolder1_rbTPPercentageDiscount").prop("checked", true);
                        toastr.info("Please Remove Catgory Wise Discount First and Try Then.");

                        return false;
                    }

                    if ($("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "" && $("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "0") {
                        //$("#ContentPlaceHolder1_txtTPDiscountAmount").val("0");
                        CalculateDiscountAmount();
                        $("#ContentPlaceHolder1_txtTPDiscountAmount").attr("disabled", false);
                    }
                }
            });

            $("#ContentPlaceHolder1_rbTPPercentageDiscount").click(function () {
                if ($(this).is(":checked")) {

                    if (AddedClassificationList.length > 0) {
                        $("#ContentPlaceHolder1_rbTPPercentageDiscount").prop("checked", true);
                        toastr.info("Please Remove Catgory Wise Discount First and Try Then.");

                        return false;
                    }

                    $("#ContentPlaceHolder1_txtTPDiscountAmount").attr("disabled", false);
                    if ($("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "" && $("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "0") {
                        //$("#ContentPlaceHolder1_txtTPDiscountAmount").val("0");
                        CalculateDiscountAmount();
                    }
                }
            });

            $("#ContentPlaceHolder1_rbTPComplementaryDiscount").click(function () {
                if ($(this).is(":checked")) {

                    if (AddedClassificationList.length > 0) {
                        $("#ContentPlaceHolder1_rbTPComplementaryDiscount").prop("checked", false);
                        $("#ContentPlaceHolder1_rbTPPercentageDiscount").prop("checked", true);

                        toastr.info("Please Remove Catgory Wise Discount First and Try Then.");

                        return false;
                    }

                    $("#ContentPlaceHolder1_txtTPDiscountAmount").val("100");
                    $("#ContentPlaceHolder1_txtTPDiscountAmount").attr("disabled", true);

                    if ($("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "" && $("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "0") {

                        $("#ContentPlaceHolder1_txtRoomPayment").val("");
                        $("#ContentPlaceHolder1_txtCompanyPayment").val("");
                        $("#ContentPlaceHolder1_txtEmployeePayment").val("");
                        $("#ContentPlaceHolder1_txtMemberPayment").val("");
                        $("#ContentPlaceHolder1_txtCash").val("");
                        $("#ContentPlaceHolder1_txtAmexCard").val("");
                        $("#ContentPlaceHolder1_txtMasterCard").val("");
                        $("#ContentPlaceHolder1_txtVisaCard").val("");
                        $("#ContentPlaceHolder1_txtDiscoverCard").val("");

                        CalculateDiscountAmount();
                    }
                }
            });

            $("#ContentPlaceHolder1_rbTpNonChargeable").click(function () {
                if ($(this).is(":checked")) {


                    if (AddedClassificationList.length > 0) {
                        $("#ContentPlaceHolder1_rbTPComplementaryDiscount").prop("checked", false);
                        $("#ContentPlaceHolder1_rbTPPercentageDiscount").prop("checked", true);

                        toastr.info("Please Remove Catgory Wise Discount First and Try Then.");

                        return false;
                    }

                    $("#ContentPlaceHolder1_txtTPDiscountAmount").val("100");
                    $("#ContentPlaceHolder1_txtTPDiscountAmount").attr("disabled", true);

                    if ($("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "" && $("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "0") {

                        $("#ContentPlaceHolder1_txtRoomPayment").val("");
                        $("#ContentPlaceHolder1_txtCompanyPayment").val("");
                        $("#ContentPlaceHolder1_txtEmployeePayment").val("");
                        $("#ContentPlaceHolder1_txtMemberPayment").val("");
                        $("#ContentPlaceHolder1_txtCash").val("");
                        $("#ContentPlaceHolder1_txtAmexCard").val("");
                        $("#ContentPlaceHolder1_txtMasterCard").val("");
                        $("#ContentPlaceHolder1_txtVisaCard").val("");
                        $("#ContentPlaceHolder1_txtDiscoverCard").val("");

                        CalculateDiscountAmount();
                    }
                }
            });

            $("#btnCategoryWiseDiscountOK").click(function () {

                AddedClassificationList = new Array();
                UpdatedClassificationList = new Array();
                DeletedClassificationList = new Array();
                var discountId = 0, billId = "0", discountAmount = "0";

                if ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "0" && $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "") {
                    billId = $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val();
                }

                $('#TableClassificationWiseDiscount tbody tr').each(function () {
                    if ($(this).find("td:eq(0)").find("input").is(':checked')) {

                        discountAmount = $.trim($(this).find("td:eq(2)").find("input").val());
                        discountAmount = (discountAmount == null || discountAmount == "") ? "0" : parseInt(discountAmount);

                        if (discountAmount == "0") {
                            $(this).find("td:eq(0)").find("input").prop("checked", false);
                        }
                    }
                });

                discountAmount = 0;

                $('#TableClassificationWiseDiscount tbody tr').each(function () {

                    discountId = parseInt($.trim($(this).find("td:eq(4)").text()), 10);
                    discountAmount = $.trim($(this).find("td:eq(2)").find("input").val());

                    discountAmount = (discountAmount == null || discountAmount == "") ? "0" : parseInt(discountAmount);

                    if ($(this).find("td:eq(0)").find("input").is(':checked')) {

                        AddedClassificationList.push({
                            DiscountAmount: discountAmount,
                            ClassificationId: parseInt($.trim($(this).find("td:eq(3)").text()), 10),
                            DiscountId: discountId,
                            BillId: billId
                        });
                    }
                    else {
                        if ($.trim($(this).find("td:eq(4)").text()) != "0") {
                            DeletedClassificationList.push({
                                DiscountAmount: discountAmount,
                                ClassificationId: parseInt($.trim($(this).find("td:eq(3)").text()), 10),
                                DiscountId: discountId,
                                BillId: billId
                            });
                        }

                        $(this).find("td:eq(0)").find("input").prop('checked', false)
                    }
                });

                $("#PercentageDiscountDialog").dialog("close");

                if (AddedClassificationList.length > 0) {
                    $("#ContentPlaceHolder1_txtTPDiscountAmount").val("0");
                    $("#ContentPlaceHolder1_txtTPDiscountAmount").attr("disabled", true);
                }
                else if (AddedClassificationList.length == 0) {
                    //$("#ContentPlaceHolder1_txtTPDiscountAmount").val("0");
                    $("#ContentPlaceHolder1_txtTPDiscountAmount").attr("disabled", false);
                }

                CalculateDiscountAmount();
                $("#ContentPlaceHolder1_txtCash").focus();
                return false;
            });

            $("#btnCategoryWiseDiscountClear").click(function () {
                $('#TableClassificationWiseDiscount tbody tr').each(function () {
                    $(this).find("td:eq(0)").find("input").prop('checked', false);
                    $(this).find("td:eq(2)").find("input").val("");
                });
            });

            $("#btnCategoryWiseDiscountCancel").click(function () {
                $("#PercentageDiscountDialog").dialog("close");
            });

            $("#btnPromotionalDiscountOK").click(function () {

                AddedPromotionalDiscountList = new Array();
                DeletedPromotionalDiscountList = new Array();

                var totalDiscount = 0.00;

                $('#TablePromotionalDiscount tbody tr').each(function () {

                    if ($(this).find("td:eq(0)").find("input").is(':checked')) {

                        AddedPromotionalDiscountList.push({
                            BusinessPromotionId: parseInt($.trim($(this).find("td:eq(3)").text()), 10),
                            DiscountId: parseInt($.trim($(this).find("td:eq(4)").text()), 10)
                        });

                        totalDiscount += parseFloat($(this).find("td:eq(2)").text());
                    }
                    else {
                        if ($.trim($(this).find("td:eq(4)").text()) != "0") {
                            DeletedPromotionalDiscountList.push({
                                BusinessPromotionId: parseInt($.trim($(this).find("td:eq(3)").text()), 10),
                                DiscountId: parseInt($.trim($(this).find("td:eq(4)").text()), 10)
                            });
                        }
                    }
                });

                $("#ContentPlaceHolder1_rbTPPercentageDiscount").prop("checked", true);
                $("#ContentPlaceHolder1_txtTPDiscountAmount").val(totalDiscount);

                $("#PromotionalDiscountDialog").dialog("close");

                $("#ContentPlaceHolder1_hfPromotionalDiscountType").val("BusinessPromotion");

                CalculateDiscountAmount();
                return false;
            });

            $("#btnPromotionalDiscountCancel").click(function () {
                $("#PromotionalDiscountDialog").dialog("close");
            });

            $("#btnBillTransferLeft").click(function () {
                var options = $("#ContentPlaceHolder1_lstvBillSplitRight option:selected");
                for (var i = 0; i < options.length; i++) {
                    $("#ContentPlaceHolder1_lstvBillSplitLeft").append($(options[i]));
                }
            });

            $("#btnBillTransferRight").click(function () {
                var options = $("#ContentPlaceHolder1_lstvBillSplitLeft option:selected");
                for (var i = 0; i < options.length; i++) {
                    $("#ContentPlaceHolder1_lstvBillSplitRight").append($(options[i]));
                }
            });

            $("#btnLeftBillSplitPrintPreview").click(function () {
                var selectedItem = "";

                $("#ContentPlaceHolder1_lstvBillSplitLeft option").each(function () {
                    if (selectedItem != "")
                        selectedItem += "," + $(this).val();
                    else
                        selectedItem = $(this).val();
                });

                var discount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();
                discount = discount == "" ? "0" : discount;

                var billId = $("#ContentPlaceHolder1_hfBillId").val();
                var url = "/POS/Reports/frmReportTPSplitBillInfo.aspx?billId=" + billId + "&disc=" + discount + "&itm=" + selectedItem;
                var popup_window = "Print Preview";
                window.open(url, popup_window, "width=770,height=680,left=300,top=50,resizable=yes");

                LoadGridInformation();

                return false;
            });

            $("#btnRightBillSplitPrintPreview").click(function () {
                var selectedItem = "";

                $("#ContentPlaceHolder1_lstvBillSplitRight option").each(function () {
                    if (selectedItem != "")
                        selectedItem += "," + $(this).val();
                    else
                        selectedItem = $(this).val();
                });

                var discount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();
                discount = discount == "" ? "0" : discount;

                var billId = $("#ContentPlaceHolder1_hfBillId").val();
                var url = "/POS/Reports/frmReportTPSplitBillInfo.aspx?billId=" + billId + "&disc=" + discount + "&itm=" + selectedItem;
                var popup_window = "Print Preview";
                window.open(url, popup_window, "width=770,height=680,left=300,top=50,resizable=yes");

                LoadGridInformation();
                return false;
            });

            $("#txtItemName").autocomplete({
                source: function (request, response) {

                    var hfCostCenterIdVal = $("#ContentPlaceHolder1_hfCostCenterId").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../../../Common/WebMethodPage.aspx/ItemSearch",
                        data: "{'itemName':'" + request.term + "','costCenterId':'" + hfCostCenterIdVal + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    ImagePath: m.ImageName,
                                    label: m.Name,
                                    value: m.ItemId,
                                    ItemName: m.ItemName
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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

                    $("#ContentPlaceHolder1_hfSearchItemId").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfSearchItemName").val(ui.item.ItemName);
                    $("#txtItemQuantity").val("1");
                    $("#txtItemQuantity").focus();
                }
            });

            $("#ContentPlaceHolder1_txtWaiter").autocomplete({
                source: function (request, response) {

                    var hfCostCenterIdVal = $("#ContentPlaceHolder1_hfCostCenterId").val()

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../../../Common/WebMethodPage.aspx/GetBearerInfoByAutoSearch",
                        data: "{'beararName':'" + request.term + "','costCenterId':'" + hfCostCenterIdVal + "'}",
                        dataType: "json",
                        success: function (data) {

                            if (data.d == '') {
                                $("#ContentPlaceHolder1_txtWaiter").val("");
                                $("#ContentPlaceHolder1_hfBillOrderTakingWaiterId").val("");
                            }

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    UserInfoId: m.UserInfoId,
                                    label: m.UserName,
                                    value: m.BearerId
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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
                    event.preventDefault();
                    $(this).val(ui.item.label);

                    $("#ContentPlaceHolder1_txtWaiter").autocomplete("enable");

                    $("#ContentPlaceHolder1_txtWaiter").val(ui.item.label);
                    $("#ContentPlaceHolder1_hfBillOrderTakingWaiterId").val(ui.item.value);
                }
            });

            $("#btnGuestSelectOK").click(function () {
                var guestList = "", guestName = "";

                $("#RoomWiseGuestList tbody tr").each(function () {
                    if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                        guestName = $(this).find("td:eq(1)").text();

                        guestList = guestList != "" ? guestList + ", " + guestName : $("#lblGuestRoom").text() + ", " + guestName;
                    }
                });

                $("#ContentPlaceHolder1_hfGuestList").val(guestList);
                $("#GuestDialog").dialog("close");

            });

            $("#btnGuestSelectCancel").click(function () {
                $("#GuestDialog").dialog("close");
            });
        });

        function OnDeleteRecipeDetailsAndUpdateDefaultPriceSucceeded(result) {

        }



        function OnDeleteRecipeDetailsAndUpdateDefaultPriceFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function CategoryWiseDiscountCheckUnCheck(control) {
            var tr = $(control).parent().parent();

            if ($(control).is(":checked") == false) {
                $(tr).find("td:eq(2)").find("input").val("");
            }
            else {
                $(tr).find("td:eq(2)").find("input").focus();
            }
        }

        function OptionDeciderDialogClose() {
            $("#TableInfoActionDecider").dialog("close");
            $("#ItemWiseSpecialRemarks").hide();
            $("#OpenItemNameChangeDialog").hide();
            $("#QuantityEditDialog").hide();
            $("#ContentPlaceHolder1_hfIsAccessVarified").val("0");
            $("#ContentPlaceHolder1_hfKotDetailId").val('');
        }

        function CheckGuestAll() {
            $("#RoomWiseGuestList tbody tr").find("td:eq(0)").find("input").prop("checked", $("#AllGuestSelect").is(":checked"));
        }

        function ExistingRecipeModifierTypesChange(result) {
            //debugger;

            var modifierId = result.value;
            var row = $(result).parents('tr');
            var itemId = RecipeTable.row(row).data().ItemId;

            var itemObject = _.findWhere(previousRecipeTypesList, { ItemId: parseInt(itemId) });

            var typeObject = _.findWhere(itemObject.RecipeModifierTypes, { TypeId: parseInt(modifierId) });
            RecipeTable.cell(row, 2).data(typeObject.TotalCost);
            var selectedValue = RecipeTable.cell(row, 1).nodes().to$().find('select').val();
            RecipeTable.cell(row, 6).data(selectedValue);
            return false;
        }

        function UpdateOrderInfo(changeType) {
            //$("#TableInfoActionDecider").dialog("close");
            $("#ItemWiseRecipeList").hide();
            $("#ContentPlaceHolder1_hfChangeType").val(changeType);
            $("#ContentPlaceHolder1_hfIsItemEditOrDelete").val(changeType);
            var isAlreadySubmitted = $("#ContentPlaceHolder1_hfIsOrderSubmit").val();

            if (isAlreadySubmitted == "1") {
                if ($("#ContentPlaceHolder1_hfIsItemCanEditDelete").val() == "1") {
                    $("#btnDeleteTableInfo").attr("disabled", false);
                }
                else {

                    if ($("#ContentPlaceHolder1_hfIsAccessVarified").val() == "0") {

                        CommonHelper.TouchScreenUserKeyboard("accessvarification", "KeyBoardVarificationContainer");
                        var keyboard = $('.accessvarification').getkeyboard();
                        keyboard.reveal();

                        $("#txtUserId").val("");
                        $("#txtUserPassword").val("");

                        $("#txtUserId_keyboard").css("top", "0px");

                        $("#AccessPermissionForEditDelete").dialog({
                            width: 900,
                            height: 500,
                            autoOpen: true,
                            modal: true,
                            closeOnEscape: true,
                            resizable: false,
                            fluid: true,
                            close: CloseAccessPermissionForEditDelete,
                            title: "Access Varification",
                            show: 'slide'
                        });

                        //toastr.info("Edit -->  " + $("#ContentPlaceHolder1_hfIsAccessVarified").val());
                        return false;
                    }
                }
            }

            //toastr.info("Edit Access Varified-->  " + $("#ContentPlaceHolder1_hfIsAccessVarified").val());

            CommonHelper.TouchScreenNumberKeyboard("numkbnotdecimal", "KeyBoardContainerQuantityChange");
            var keyboard = $('.numkbnotdecimal').getkeyboard();
            keyboard.reveal();

            $("#ContentPlaceHolder1_txtTouchKeypadResult_keyboard").css("top", "0px");

            $("#ItemWiseSpecialRemarks").hide();
            $("#OpenItemNameChangeDialog").hide();
            $("#QuantityEditDialog").show();
            $("#ContentPlaceHolder1_hfIsAccessVarified").val("0");
        }

        function UpdateOpenItemName() {

            CommonHelper.ExactMatch();

            var itemId = $("#ContentPlaceHolder1_hfItemId").val();

            var tr = $("#TableWiseItemInformation tbody tr").find("td:eq(5):textEquals('" + itemId + "')").parent();
            var selectedItemQty = "0", itemName = "";

            itemName = $(tr).first().find("td:eq(0)").text();

            $("#txtItemNameNew").val(itemName);
            $("#ItemWiseSpecialRemarks").hide();
            $("#OpenItemNameChangeDialog").show();
            $("#QuantityEditDialog").hide();
            $("#ContentPlaceHolder1_hfIsAccessVarified").val("0");

            return false;
        }

        function ChangeItemName() {

            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var sourceName = $("#ContentPlaceHolder1_hfsourceType").val();
            var itemId = $("#ContentPlaceHolder1_hfItemId").val();
            var kotDetailId = $("#ContentPlaceHolder1_hfKotDetailId").val();
            var isAlreadySubmitted = $("#ContentPlaceHolder1_hfIsOrderSubmit").val();

            var hfCostCenterIdVal = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var kotDetailsId = $("#hfKotDetailId").val();
            var updatedContent = $("#txtItemNameNew").val();

            var isItemCanEditDelete = $("#ContentPlaceHolder1_hfIsItemCanEditDelete").val();
            PageMethods.UpdateIndividualItemDetailInformation(userIdAuthorised, userIdAuthorisedPassword, kotId, sourceName, isItemCanEditDelete, 'ItemNameChange', hfCostCenterIdVal, kotDetailId, 0, updatedContent,
                OnUpdateItemNameSucceeded, OnUpdateObjectFailed);

        }

        function OnUpdateItemNameSucceeded(result) {
            userIdAuthorised = "";
            userIdAuthorisedPassword = "";
            if (result.IsBillResettled == true) {
                CommonHelper.AlertMessage(result.AlertMessage);
                setTimeout(function () { window.location = result.RedirectUrl; }, 2000);
                return false;
            }

            if ($("#ContentPlaceHolder1_hfIsUserHasEditDeleteAccess").val() == "0")
                $("#ContentPlaceHolder1_hfIsItemCanEditDelete").val("0");

            LoadGridInformation();
            toastr.success("Name Change Succeed.");
        }
        function OnUpdateObjectFailed() { }

        function GoToHomePanel() {
            window.location = "frmCostCenterSelectionForAll.aspx";
            return false;
        }

        function SpecialRemarksSave() {

            CommonHelper.SpinnerOpen();

            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var itemId = $("#ContentPlaceHolder1_hfItemId").val();
            var kotDetailId = $("#ContentPlaceHolder1_hfKotDetailId").val();

            var specialRsId = 0;
            var RKotSRemarksDetail = new Array();
            var RKotSRemarksDetailDelete = new Array();

            if ($("#ContentPlaceHolder1_hfIsPredefinedRemarksEnable").val() == "1") {

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

                if (RKotSRemarksDetail.length == 0 && RKotSRemarksDetailDelete.length == 0) {
                    toastr.info("Please Select Special Remarks.");
                    CommonHelper.SpinnerClose();
                    return false;
                }

                PageMethods.SaveKotSpecialRemarks(RKotSRemarksDetail, RKotSRemarksDetailDelete, kotDetailId, OnSaveKotSpecialRemarksSuccedd, OnSaveKotSpecialRemarksFailed);
            }
            else {

                var remarks = $("#txtItemWiseRemarks").val();
                var remarksDetailId = 0;

                if (AlreadySaveKotItemWiseRemarks != null) {
                    remarksDetailId = AlreadySaveKotItemWiseRemarks.RemarksDetailId;
                }
                var itemWiseRemark = {
                    RemarksDetailId: remarksDetailId,
                    KotId: kotId,
                    ItemId: itemId,
                    SpecialRemarksId: 0,
                    Remarks: remarks
                };

                PageMethods.SaveKotItemWiseRemarks(itemWiseRemark, kotDetailId, OnSaveItemWiseRemarksSuccedd, OnSaveKotSpecialRemarksFailed);

            }
        }

        function OnSaveKotSpecialRemarksSuccedd(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //$("#ContentPlaceHolder1_hfItemId").val("");
                alreadySaveKotRemarks = [];
                $("#remarksContainer").remove("TableWiseItemRemarksInformation");
                $("#ItemWiseSpecialRemarks").hide();
                LoadGridInformation();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
        }

        function OnSaveItemWiseRemarksSuccedd(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //$("#ContentPlaceHolder1_hfItemId").val("");
                AlreadySaveKotItemWiseRemarks = null;

                LoadGridInformation();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
        }

        function OnSaveKotSpecialRemarksFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function PerformAction(selectedItemId) {
            tst = selectedItemId;
            var hfCostCenterIdVal = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var sourceName = $("#ContentPlaceHolder1_hfsourceType").val();

            CommonHelper.ExactMatch();

            var tr = $("#TableWiseItemInformation tbody tr").find("td:eq(5):textEquals('" + selectedItemId + "')").parent();
            var selectedItemQty = "0";

            if ($(tr).length != 0) {
                selectedItemQty = $(tr).first().find("td:eq(1)").text();
                selectedItemQty = parseInt(selectedItemQty) + 1;
            }
            else {
                selectedItemQty = "1";
            }

            var isItemCanEditDelete = $("#ContentPlaceHolder1_hfIsItemCanEditDelete").val();
            PageMethods.SaveIndividualItemDetailInformation(isItemCanEditDelete, hfCostCenterIdVal, kotId, selectedItemId, selectedItemQty, sourceName, OnSaveObjectSucceeded, OnSaveObjectFailed);
            return false;
        }

        function OnSaveObjectSucceeded(result) {

            if (result.IsBillResettled == true) {
                CommonHelper.AlertMessage(result.AlertMessage);
                setTimeout(function () { window.location = result.RedirectUrl; }, 2000);
                return false;
            }

            LoadGridInformation();
            //var keyboard = $('#txtItemName').getkeyboard();
            $("#txtItemName").val("");
            $("#txtItemQuantity").val("");
            $("#txtItemName").focus();
            $("#ItemSearchDialog").dialog('close');
            return false;
        }

        function OnSaveObjectFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function LoadGridInformation() {
            CommonHelper.SpinnerOpen();

            var costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var bearerId = $("#ContentPlaceHolder1_hfBearerId").val();
            var tableId = $("#ContentPlaceHolder1_hfSourceId").val();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var kotIdList = "";

            if (AddedTableList != null) {
                if (AddedTableList.length > 0) {

                    $(AddedTableList).each(function (index, itm) {

                        if (kotIdList != "") {
                            kotIdList += "," + itm.KotId;
                        }
                        else {
                            kotIdList = itm.KotId;
                        }
                    });
                }
                else {
                    kotIdList = kotId;
                }
            }
            if (AddedTableList != null) {
                if (AddedTableList.length > 0) {
                    kotIdList += "," + kotId;
                }
            }

            //toastr.info("costCenterId: " + costCenterId + ', bearerId: ' + bearerId + ', tableId: ' + tableId + ', kotId: ' + kotId);
            //bearerId > 0 && && tableId > 0

            if (costCenterId > 0 && kotId > 0) {
                PageMethods.GenerateTableWiseItemGridInformation(costCenterId, kotIdList, OnLoadObjectSucceeded, OnLoadObjectFailed);
            }
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadObjectSucceeded(result) {

            var heightVal = $("#ContentPlaceHolder1_hfltlTableWiseItemInformationDivHeight").val();
            $("#ltlTableWiseItemInformation").height(heightVal);
            $("#ltlTableWiseItemInformation").css("max-height", heightVal + "px");

            $("#ltlTableWiseItemInformation").html(result[0]);

            if (result[2] == true) {
                $("#ContentPlaceHolder1_hfIsBillPreviewButtonEnable").val("1");
            }
            else {
                $("#ContentPlaceHolder1_hfIsBillPreviewButtonEnable").val("0");
            }

            if ($("#ContentPlaceHolder1_hfBearerCanSettleBill").val() == "1") {
                $("#btnBillPreview").show();
            }
            else {

                if ($("#ContentPlaceHolder1_hfIsBearar").val() == "1") {

                    if ($("#ContentPlaceHolder1_hfIsBillPreviewButtonEnable").val() == "0" &&
                        ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "" && $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "0")
                    ) {
                        $("#btnBillPreview").hide();
                    }
                    else {
                        $("#btnBillPreview").show();
                    }
                }
                else {
                    $("#btnBillPreview").show();
                }
            }

            var total = 0.00;
            $("#TableWiseItemInformation tbody tr").each(function () {
                total += parseFloat($(this).find("td:eq(3)").text());
            });

            total = toFixed(total, 2);

            $("#ContentPlaceHolder1_txtTotalSales").val(parseFloat(total));
            $("#ContentPlaceHolder1_txtSalesAmount").val(parseFloat(total));
            $("#ContentPlaceHolder1_txtChangeAmount").val(parseFloat(total));

            CalculateDiscountAmount();

            if ($("#ContentPlaceHolder1_txtSalesAmount").val() != "" && $("#ContentPlaceHolder1_txtSalesAmount").val() != "0") {
                $("#ContentPlaceHolder1_btnBillHolpup").attr("disabled", false);
                $("#ContentPlaceHolder1_btnOrderSubmit").attr("disabled", false);
            }

            $(".roomIsRegistered").show();

            CommonHelper.SpinnerClose();
            return false;
        }

        function OnLoadObjectFailed() {
            CommonHelper.SpinnerClose();
            // toastr.error(xhr.responseText);
        }

        function MyChagne(event, keyboard, el) {
            toastr.info('The content "' + el.value + '" was accepted!');
        }

        function LoadPaymentInformation() {


            CommonHelper.TouchScreenNumberKeyboardPayment("numkb", "KeyBoardContainer");
            var keyboard = $('.numkb').getkeyboard();
            //keyboard.reveal();

            CommonHelper.ApplyIntigerValidation();

            var total = 0.00, tt = 0.00;
            $("#TableWiseItemInformation tbody tr").each(function () {
                tt = parseFloat($(this).find("td:eq(3)").text());
                total = total + tt;
            });

            total = toFixed(total, 2);

            $("#ContentPlaceHolder1_txtTPGrandTotal").val($("#ContentPlaceHolder1_txtGrandTotal").val());
            $("#ContentPlaceHolder1_txtTotalSales").val(total);

            var discountAmount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();

            if (discountAmount == "") {
                discountAmount = 0;
            }

            if (discountAmount == 0) {
                $("#ContentPlaceHolder1_txtTPDiscountedAmount").val(total);
            }

            $("#ContentPlaceHolder1_txtCash_keyboard").css("top", "0px");

            if (total != 0) {
                $("#RestaurantPaymentInformationDiv").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 1050,
                    height: 640,
                    closeOnEscape: false,
                    resizable: false,
                    title: "Payment Information",
                    show: 'slide',
                    open: function (event, ui) {
                        $('#RestaurantPaymentInformationDiv').css('overflow', 'hidden');
                        $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                    }
                });

                $("#ContentPlaceHolder1_txtCash").focus();
                $("#ContentPlaceHolder1_txtCash_keyboard").css("left", "0px");

                CalculateDiscountAmount();

                return;
            }
        }

        function PerformTPOkButton() {
            var sourceType = $.trim(CommonHelper.GetParameterByName("st"));

            if ($("#ContentPlaceHolder1_txtAmexCard").val() != "") {
                if ($("#ContentPlaceHolder1_hfAmexCardId").val() == "" || $("#ContentPlaceHolder1_hfAmexCardId").val() == "0") {
                    toastr.warning("Please Select Amex Card To Give Mobile Bank Payment.");
                    return false;
                }
            }
            if ($("#ContentPlaceHolder1_txtMasterCard").val() != "") {
                if ($("#ContentPlaceHolder1_hfMasterCardId").val() == "" || $("#ContentPlaceHolder1_hfMasterCardId").val() == "0") {
                    toastr.warning("Please Select Master Card To Give Mobile Bank Payment.");
                    return false;
                }
            }
            if ($("#ContentPlaceHolder1_txtVisaCard").val() != "") {
                if ($("#ContentPlaceHolder1_hfVisaCardId").val() == "" || $("#ContentPlaceHolder1_hfVisaCardId").val() == "0") {
                    toastr.warning("Please Select Visa Card To Give Mobile Bank Payment.");
                    return false;
                }
            }
            if ($("#ContentPlaceHolder1_txtDiscoverCard").val() != "") {
                if ($("#ContentPlaceHolder1_hfDiscoverCardId").val() == "" || $("#ContentPlaceHolder1_hfDiscoverCardId").val() == "0") {
                    toastr.warning("Please Select Discover Card To Give Mobile Bank Payment.");
                    return false;
                }
            }
            if ($("#ContentPlaceHolder1_txtMBankingPayment").val() != "") { // && $("#ContentPlaceHolder1_txtMBankingPayment").val() != "0"
                if ($("#ContentPlaceHolder1_hfMBankId").val() == "" || $("#ContentPlaceHolder1_hfMBankId").val() == "0") {
                    toastr.warning("Please Select M-Banking To Give Mobile Bank Payment.");
                    return false;
                }
            }
            if ($("#ContentPlaceHolder1_txtCompanyPayment").val() != "") { // && $("#ContentPlaceHolder1_txtCompanyPayment").val() != "0"
                if ($("#ContentPlaceHolder1_hfCompanyId").val() == "" || $("#ContentPlaceHolder1_hfCompanyId").val() == "0") {
                    toastr.warning("Please Select Compnay To Give Company Payment.");
                    return false;
                }
            }
            if ($("#ContentPlaceHolder1_txtEmployeePayment").val() != "") { // && $("#ContentPlaceHolder1_txtEmployeePayment").val() != "0"
                if ($("#ContentPlaceHolder1_hfEmployeeId").val() == "" || $("#ContentPlaceHolder1_hfEmployeeId").val() == "0") {
                    toastr.warning("Please Select Employee To Give Employee Payment.");
                    return false;
                }
            }
            if ($("#ContentPlaceHolder1_txtMemberPayment").val() != "") { // && $("#ContentPlaceHolder1_txtMemberPayment").val() != "0"
                if ($("#ContentPlaceHolder1_hfMemberId").val() == "" || $("#ContentPlaceHolder1_hfMemberId").val() == "0") {
                    toastr.warning("Please Select Member To Give Member Payment.");
                    return false;
                }
            }
            if (sourceType != "rom" && $("#ContentPlaceHolder1_txtRoomPayment").val() != "") { // && $("#ContentPlaceHolder1_txtRoomPayment").val() != "0"
                if ($("#ContentPlaceHolder1_hfRoomId").val() == "" || $("#ContentPlaceHolder1_hfRoomId").val() == "0") {
                    toastr.warning("Please Select Room To Give Room Payment.");
                    return false;
                }
            }

            var PaymentMode = new Array();

            var amexCardBankName = $("#lblAmexCardBankName").text();
            var masterCardBankName = $("#lblMasterCardBankName").text();
            var visaCardBankName = $("#lblVisaCardBankName").text();
            var discoverCardBankName = $("#lblDiscoverCardBankName").text();

            var mBankName = $("#lblMBankName").text();
            var companyName = $("#lblCompanyName").text();
            var employeeName = $("#lblEmployeeName").text();
            var memberName = $("#txtMemberName").val();

            PaymentMode.push({ CardType: '', PaymentType: 'Advance', PaymentDescription: '', PaymentMode: 'Cash', PaymentById: '', Control: "ContentPlaceHolder1_txtCash", AlertMassage: "Cash" });
            PaymentMode.push({ CardType: 'a', PaymentType: 'Advance', PaymentDescription: 'American Express', PaymentMode: 'Card', PaymentById: $("#ContentPlaceHolder1_hfAmexCardId").val(), Control: "ContentPlaceHolder1_txtAmexCard", AlertMassage: "Card" });
            PaymentMode.push({ CardType: 'm', PaymentType: 'Advance', PaymentDescription: 'Master Card', PaymentMode: 'Card', PaymentById: $("#ContentPlaceHolder1_hfMasterCardId").val(), Control: "ContentPlaceHolder1_txtMasterCard", AlertMassage: "Card" });
            PaymentMode.push({ CardType: 'v', PaymentType: 'Advance', PaymentDescription: 'Visa Card', PaymentMode: 'Card', PaymentById: $("#ContentPlaceHolder1_hfVisaCardId").val(), Control: "ContentPlaceHolder1_txtVisaCard", AlertMassage: "Card" });
            PaymentMode.push({ CardType: 'd', PaymentType: 'Advance', PaymentDescription: 'Discover Card', PaymentMode: 'Card', PaymentById: $("#ContentPlaceHolder1_hfDiscoverCardId").val(), Control: "ContentPlaceHolder1_txtDiscoverCard", AlertMassage: "Card" });
            PaymentMode.push({ CardType: '', PaymentType: 'Advance', PaymentDescription: mBankName, PaymentMode: 'M-Banking', PaymentById: $("#ContentPlaceHolder1_hfMBankId").val(), Control: "ContentPlaceHolder1_txtMBankingPayment", AlertMassage: "M-Banking" });
            PaymentMode.push({ CardType: '', PaymentType: 'Company', PaymentDescription: companyName, PaymentMode: 'Company', PaymentById: $("#ContentPlaceHolder1_hfCompanyId").val(), Control: "ContentPlaceHolder1_txtCompanyPayment", AlertMassage: "Company" });
            PaymentMode.push({ CardType: '', PaymentType: 'GuestRoom', PaymentDescription: 'GuestRoom', PaymentMode: 'Other Room', PaymentById: $("#ContentPlaceHolder1_hfRoomId").val(), Control: "ContentPlaceHolder1_txtRoomPayment", AlertMassage: "Room" });
            PaymentMode.push({ CardType: '', PaymentType: 'Employee', PaymentDescription: employeeName, PaymentMode: 'Employee', PaymentById: $("#ContentPlaceHolder1_hfEmployeeId").val(), Control: "ContentPlaceHolder1_txtEmployeePayment", AlertMassage: "Employee" });
            PaymentMode.push({ CardType: '', PaymentType: 'Member', PaymentDescription: memberName, PaymentMode: 'Member', PaymentById: $("#ContentPlaceHolder1_hfMemberId").val(), Control: "ContentPlaceHolder1_txtMemberPayment", AlertMassage: "Member" });
            
            var paymodeCount = PaymentMode.length, row = 0;
            var isInvalid = false, isCashPayment = false;
            var dueAmount = $("#ContentPlaceHolder1_txtTPChangeAmount").val();
            var massage = "";
            for (row = 0; row < paymodeCount; row++) {
                if (PaymentMode[row].PaymentMode == "Cash" && $("#" + PaymentMode[row].Control).val() > 0) {
                    isCashPayment = true;
                    break;
                }
                else if (PaymentMode[row].PaymentMode != "Cash" && $("#" + PaymentMode[row].Control).val() > 0 && parseFloat(dueAmount) > 0) {
                    isInvalid = true;
                    $("#" + PaymentMode[row].Control).val('').focus();
                    massage = PaymentMode[row].AlertMassage;
                    break;
                }

            }
            if (!isCashPayment && isInvalid) {
                toastr.warning(massage + " Payment Amount Cannot Be Greater Than Due Amount.");
                return false;
            }

            //if ($("#ContentPlaceHolder1_rbTPComplementaryDiscount").is(":checked") || $("#ContentPlaceHolder1_rbTpNonChargeable").is(":checked")) {

            //    if ($("#ContentPlaceHolder1_txtRoomPayment").val() == "" && $("#ContentPlaceHolder1_txtCompanyPayment").val() == ""
            //        && $("#ContentPlaceHolder1_txtEmployeePayment").val() == "" && $("#ContentPlaceHolder1_txtMemberPayment").val() == ""
            //        && $("#ContentPlaceHolder1_txtCash").val() == "" && $("#ContentPlaceHolder1_txtAmexCard").val() == ""
            //        && $("#ContentPlaceHolder1_txtMasterCard").val() == "" && $("#ContentPlaceHolder1_txtVisaCard").val() == ""
            //        && $("#ContentPlaceHolder1_txtDiscoverCard").val() == ""
            //        ) {
            //        toastr.info("Please Enter Zero(0) For Complementary/Non Chargeable Payment.");
            //        return false;
            //    }
            //}

            var dueAmountText = $.trim($("#ContentPlaceHolder1_lblTPChangeAmount").text());

            //if ($("#ContentPlaceHolder1_cbTPVatAmount").is(":checked"))
            //    $("#ContentPlaceHolder1_hfIsVatEnable").val("1");
            //else
            //    $("#ContentPlaceHolder1_hfIsVatEnable").val("0");

            //if ($("#ContentPlaceHolder1_cbTPServiceCharge").is(":checked"))
            //    $("#ContentPlaceHolder1_hfIsServiceChargeEnable").val("1");
            //else
            //    $("#ContentPlaceHolder1_hfIsServiceChargeEnable").val("0");

            //if ($("#ContentPlaceHolder1_cbTPSDCharge").is(":checked")) {
            //    $("#ContentPlaceHolder1_hfIsSDChargeEnable").val("1");
            //}
            //else {
            //    $("#ContentPlaceHolder1_hfIsSDChargeEnable").val("0");
            //}

            //if ($("#ContentPlaceHolder1_cbTPAdditionalCharge").is(":checked")) {
            //    $("#ContentPlaceHolder1_hfIsAdditionalChargeEnable").val("1");
            //}
            //else {
            //    $("#ContentPlaceHolder1_hfIsAdditionalChargeEnable").val("0");
            //}

            if (dueAmount == "")
                dueAmount = "0";

            if (dueAmountText != "Due Amount" || parseFloat(dueAmount) == 0) {
                $("#ContentPlaceHolder1_btnSave").attr("disabled", false);
            }
            else {
                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
            }

            $(vTouchControl).css({ "border-color": "", "box-shadow": "" });
            vTouchControl = "";
            vTouchId = "0";

            $("#ContentPlaceHolder1_txtRemarks").trigger("focus");
            $("#RestaurantPaymentInformationDiv").dialog("close");

            var kb = $('.numkb').getkeyboard();
            if (kb.isOpen) {
                kb.switchInput(true, kb.options.autoAccept);
                kb.close();
            }

            //CalculateDiscountAmount();
        }

        function OnLoadPaymentInformationInSessionSucceeded(result) {

            $("#RestaurantPaymentInformationDiv").dialog("close");
            var keyboardnum = $('.numkb').getkeyboard();
            keyboardnum.destroy();
            return false;
        }

        function OnLoadPaymentInformationInSessionFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge(ctrl) {
            CalculateDiscountAmount();
            CalculatePayment();
        }

        function ToggleTotalSalesAmountVatServiceChargeCalculationForVat(ctrl) {
            CalculateDiscountAmount();
            CalculatePayment();
        }

        function TotalSalesAmountVatServiceChargeCalculation() {
            CalculateDiscountAmount();
        }

        function OnLoadRackRateServiceChargeVatInformationSucceeded(result) {

            if (result.RackRate != null) {

                if ($("#ContentPlaceHolder1_hfIsRestaurantBillAmountWillRound").val() == "1") {
                    $("#ContentPlaceHolder1_txtGrandTotal").val(Math.round(result.RackRate));
                    $("#ContentPlaceHolder1_txtTPGrandTotal").val($("#ContentPlaceHolder1_txtGrandTotal").val());

                    $("#ContentPlaceHolder1_txtChangeAmount").val(result.RackRate);
                }
                else {
                    $("#ContentPlaceHolder1_txtGrandTotal").val(toFixed(result.RackRate, 2));
                    $("#ContentPlaceHolder1_txtTPGrandTotal").val(toFixed(result.RackRate, 2));
                    $("#ContentPlaceHolder1_txtChangeAmount").val(toFixed(result.RackRate, 2));
                }

                $("#ContentPlaceHolder1_txtTPServiceCharge").val(toFixed(result.ServiceCharge, 2));
                $("#ContentPlaceHolder1_txtTPVatAmount").val(toFixed(result.VatAmount, 2));

                $("#ContentPlaceHolder1_txtServiceCharge").val(toFixed(result.ServiceCharge, 2));
                $("#ContentPlaceHolder1_txtVatAmount").val(toFixed(result.VatAmount, 2));

                $("#ContentPlaceHolder1_txtTPSDCharge").val(toFixed(result.SDCityCharge, 2));
                $("#ContentPlaceHolder1_txtTPAdditionalCharge").val(toFixed(result.AdditionalCharge, 2));

                $("#ContentPlaceHolder1_txtCitySDCharge").val(toFixed(result.SDCityCharge, 2));
                $("#ContentPlaceHolder1_txtAdditionalCharge").val(toFixed(result.AdditionalCharge, 2));

            }
            else {
                $("#ContentPlaceHolder1_txtGrandTotal").val('0');
                $("#ContentPlaceHolder1_txtTPGrandTotal").val('0');

                $("#ContentPlaceHolder1_txtTPServiceCharge").val('0');
                $("#ContentPlaceHolder1_txtTPVatAmount").val('0');

                $("#ContentPlaceHolder1_txtServiceCharge").val('0');
                $("#ContentPlaceHolder1_txtVatAmount").val('0');

                $("#ContentPlaceHolder1_txtTPSDCharge").val('0');
                $("#ContentPlaceHolder1_txtTPAdditionalCharge").val('0');

                $("#ContentPlaceHolder1_txtCitySDCharge").val('0');
                $("#ContentPlaceHolder1_txtAdditionalCharge").val('0');
            }

            return false;
        }

        function OnLoadRackRateServiceChargeVatInformationFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function PerformDeleteAction(kotDetailsId, actionId, itemId, isAlreadySubmitted) {
            CommonHelper.ExactMatch();

            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var sourceName = $("#ContentPlaceHolder1_hfsourceType").val();

            var tr = $("#TableWiseItemInformation tbody tr").find("td:eq(5):textEquals('" + itemId + "')").parent();
            var selectedItemQty = "0";
            if ($(tr).length != 0) {
                selectedItemQty = parseFloat($(tr).find("td:eq(1)").text());
            }

            var hfCostCenterIdVal = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var isItemCanEditDelete = $("#ContentPlaceHolder1_hfIsItemCanEditDelete").val();

            PageMethods.UpdateIndividualItemDetailInformation(userIdAuthorised, userIdAuthorisedPassword, kotId, sourceName, isItemCanEditDelete, 'DeleteQuantity', hfCostCenterIdVal, kotDetailsId,
                selectedItemQty, selectedItemQty, OnDeleteObjectSucceeded, OnDeleteObjectFailed);

            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            userIdAuthorised = "";
            userIdAuthorisedPassword = "";

            if (result.IsBillResettled == true) {
                CommonHelper.AlertMessage(result.AlertMessage);
                setTimeout(function () { window.location = result.RedirectUrl; }, 2000);
                return false;
            }

            if ($("#ContentPlaceHolder1_hfIsUserHasEditDeleteAccess").val() == "0")
                $("#ContentPlaceHolder1_hfIsItemCanEditDelete").val("0");

            OptionDeciderDialogClose();
            ClearCardPaymentFields();
            LoadGridInformation();
            setTimeout(function () { $("#ContentPlaceHolder1_btnSave").attr("disabled", true); }, 2000);

        }

        function ClearEditRecipee() {
            $("#ContentPlaceHolder1_ddllblIngredientQuantity").empty();
            $("#ContentPlaceHolder1_ddlIngredient").empty();
            NewRecipeTable.clear();
            RecipeTable.clear();
            $("#ItemWiseRecipeList").hide();



        }

        function OnDeleteObjectFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        //is Recipee Check
        function GetRecipeType(itemId) {
            PageMethods.GetRecipeType(itemId, OnGetRecipeTypeSucceeded, OnGetRecipeTypeFailed);
            return false;
        }

        function OnGetRecipeTypeSucceeded(result) {
            //$("#ContentPlaceHolder1_hfStockType").val(result);
            if (result == 1) {
                $('#btnEditRecipeDiv').show();
            }
            else {
                $('#btnEditRecipeDiv').hide();
            }
            return false;

        }

        function OnGetRecipeTypeFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }


        function AddNewItem(kotDetailId, kotId, itemId, isAlreadySubmitted, isItemEditable, rowIndex) {

            GetRecipeType(itemId);

            $("#ContentPlaceHolder1_hfKotId").val(kotId);
            $("#ContentPlaceHolder1_hfItemId").val(itemId);
            $("#ContentPlaceHolder1_hfKotDetailId").val(kotDetailId);
            $("#ContentPlaceHolder1_hfIsOrderSubmit").val(isAlreadySubmitted);
            $("#ContentPlaceHolder1_hfRowIndex").val(rowIndex);

            if (isItemEditable == 1) {
                $("#UpdateItemPriceContainer").show();

                //if ($("#ContentPlaceHolder1_hfIsBearar").val() == "1") {
                //    $("#UpdateItemNameContainer").hide();
                //}
                //else {
                $("#UpdateItemNameContainer").show();
                //}
            }
            else {
                $("#UpdateItemPriceContainer").hide();
                $("#UpdateItemNameContainer").hide();
            }

            //if (isAlreadySubmitted == "1") {
            //    $("#deleteTableInfoDiv").hide();
            //}
            //else {
            //    $("#deleteTableInfoDiv").show();
            //}
            $("#ItemWiseRecipeList").hide();
            $("#ItemWiseSpecialRemarks").hide();
            $("#ItemWiseRemarks").hide();
            $("#OpenItemNameChangeDialog").hide();
            $("#QuantityEditDialog").hide();
            $("#ContentPlaceHolder1_hfIsAccessVarified").val("0");

            $("#TableInfoActionDecider").dialog({
                width: '800',
                height: 'auto',
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                close: OptionDeciderDialogClose,
                fluid: true,
                title: "Option Decider",
                show: 'slide'
            });

            return false;
        }

        function OnLoadGetPreviousRecipeModifierTypesSucceed(result) {
            //debugger;
            typesList = [];
            $("#ContentPlaceHolder1_ddllblIngredientQuantity").empty();
            var i = 0, fieldLength = result.length;

            if (fieldLength > 0) {
                typesList = result;
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddllblIngredientQuantity');
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result[i].Id + '">' + result[i].HeadName + '</option>').appendTo('#ContentPlaceHolder1_ddllblIngredientQuantity');
                }
            }
            else {
                $("<option value='0'>--No Item Found--</option>").appendTo("#ContentPlaceHolder1_ddllblIngredientQuantity");
            }

            return false;

        }

        function OnLoadGetPreviousRecipeModifierTypesFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }


        function OnGetItemRecipeDetailsSucceed(result) {
            //debugger;
            deletedItemList = new Array();
            deletedNewItemList = [];
            previousRecipeTypesList = [];
            previousRecipeTypesList = Object.assign({}, result.PreviousRecipe);
            RecipeTable.clear().draw();
            RecipeTable.rows.add(result.PreviousRecipe);
            RecipeTable.draw();
            $("#ContentPlaceHolder1_ddllblIngredientQuantity").empty();
            $("#ContentPlaceHolder1_ddlIngredient").empty();
            var i = 0, fieldLength = result.NewItems.length;

            if (fieldLength > 0) {
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlIngredient');
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result.NewItems[i].ItemId + '">' + result.NewItems[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlIngredient');
                }
            }
            else {
                $("<option value='0'>--No Item Found--</option>").appendTo("#ContentPlaceHolder1_ddlIngredient");
            }

            fieldLength = result.NewKotRecipe.length;
            i = 0;
            newItemList = [];
            NewRecipeTable.clear().draw();


            if (fieldLength > 0) {
                $("#NewRecipeDiv").show();

                var rows = new Array();
                for (i = 0; i < fieldLength; i++) {


                    rows.push({
                        RecipeId: result.NewKotRecipe[i].RecipeId,
                        RecipeItemName: result.NewKotRecipe[i].RecipeItemName,
                        HeadName: result.NewKotRecipe[i].HeadName,
                        ItemCost: result.NewKotRecipe[i].ItemCost,
                        RecipeItemId: result.NewKotRecipe[i].RecipeItemId,
                        TypeId: result.NewKotRecipe[i].TypeId

                    });



                    // newItemList.add(result.NewKotRecipe[i]);

                }
                NewRecipeTable.rows.add(rows);
                NewRecipeTable.draw();



            }



            CommonHelper.SpinnerClose();

            return false;
        }

        function OnGetItemRecipeDetailsFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function OnGetSpecialRemarksDetailsSucceed(result) {

            if ($("#ContentPlaceHolder1_hfIsPredefinedRemarksEnable").val() == "1") {
                vvc = result;
                alreadySaveKotRemarks = result.KotRemarks;

                var table = "", tr = "", td = "", i = 0, alreadyChecked = "";
                var specialRemarksLength = result.ItemSpecialRemarks.length;

                table += "<table id=\"TableWiseItemRemarksInformation\" style=\"margin:0;\" class='table table-bordered table-condensed table-responsive' >";
                table += "<thead>" +
                    "<tr style=\"color: White; background-color: #44545E; font-weight: bold;\">" +
                    "<th align=\"center\" scope=\"col\" style=\"width: 30px\">" +
                    "Select" +
                    "</th>" +
                    "<th align=\"left\" scope=\"col\" style=\"width: 200px\">" +
                    "Remarks" +
                    "</th>" +
                    "</tr>" +
                    "</thead> <tbody>";

                for (i = 0; i < specialRemarksLength; i++) {

                    alreadyChecked = '';

                    var vc = _.findWhere(result.KotRemarks, { SpecialRemarksId: result.ItemSpecialRemarks[i].SpecialRemarksId });
                    if (vc != null) { alreadyChecked = "checked='checked'"; }

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

                    tr += td + "</tr>";

                    table += tr;
                }
                table += " </tbody> </table>";

                $("#remarksContainer").html(table);

                $("#ItemWiseSpecialRemarks").show();
                $("#ItemWiseRemarks").hide();
            }
            else {

                AlreadySaveKotItemWiseRemarks = result.KotRemarks[0];
                $("#txtItemWiseRemarks").val(result.KotRemarks[0].Remarks);

                $("#ItemWiseSpecialRemarks").hide();
                $("#ItemWiseRemarks").show();
            }

            $("#OpenItemNameChangeDialog").hide();
            $("#QuantityEditDialog").hide();
            $("#ContentPlaceHolder1_hfIsAccessVarified").val("0");

            CommonHelper.SpinnerClose();
        }

        function OnGetSpecialRemarksDetailsFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function LoadPercentageDiscountInfo() {

            if ($("#ContentPlaceHolder1_rbTPComplementaryDiscount").is(":checked") == true) {
                toastr.info("Pleae Remove Complementary and Then Try.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_rbTpNonChargeable").is(":checked") == true) {
                toastr.info("Pleae Remove Non Chargeable and Then Try.");
                return false;
            }

            CommonHelper.SpinnerOpen();

            $("#ContentPlaceHolder1_rbTPPercentageDiscount").prop("checked", true);
            $("#ContentPlaceHolder1_txtTPDiscountAmount").val("0");
            CalculateDiscountAmount();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var kotIdList = "";

            if (AddedTableList.length > 0) {

                $(AddedTableList).each(function (index, itm) {

                    if (kotIdList != "") {
                        kotIdList += "," + itm.KotId;
                    }
                    else {
                        kotIdList = itm.KotId;
                    }
                });
            }
            else {
                kotIdList = kotId;
            }

            if (AddedTableList.length > 0) {
                kotIdList += "," + kotId;
            }

            PageMethods.GetClassificationWiseDiscount(kotId, kotIdList, OnLoadDiscountSucceeded, OnLoadDiscountFailed);
        }

        function OnLoadDiscountSucceeded(result) {

            $("#PercentageDiscountContainer").html(result);

            $("#PercentageDiscountDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 350,
                height: 'auto',
                closeOnEscape: false,
                resizable: false,
                title: "Classification Wise Discount",
                show: 'slide',
                open: function (event, ui) {
                    $('#PercentageDiscountDialog').css('overflow-x', 'hidden');
                }
            });

            CommonHelper.ApplyIntigerValidation();

            if (AddedClassificationList.length > 0) {

                $("#TableClassificationWiseDiscount tbody tr").each(function () {

                    var id = _.findWhere(AddedClassificationList, { ClassificationId: parseInt($.trim($(this).find("td:eq(3)").text()), 10) });

                    if (id != null) {
                        $(this).find("td:eq(0)").find("input").attr('checked', true);
                        $(this).find("td:eq(2)").find("input").val(id.DiscountAmount == "0" ? "" : id.DiscountAmount);
                    }
                });
            }
            else {
                $("#TableClassificationWiseDiscount tbody tr").each(function () {
                    $(this).find("td:eq(0)").find("input").attr('checked', false);
                    $(this).find("td:eq(2)").find("input").val("");
                });
            }
            CommonHelper.SpinnerClose();
        }

        function LoadPromotionalDiscount() {
            CommonHelper.SpinnerOpen();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();

            PageMethods.GetPromotionalDiscount(costCenterId, kotId, OnLoadPromotionalDiscountSucceeded, OnLoadDiscountFailed);
        }
        function OnLoadPromotionalDiscountSucceeded(result) {

            $("#PromotionalDiscountContainer").html(result);

            $("#PromotionalDiscountDialog").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 300,
                minHeight: 300,
                width: 'auto',
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Promotional Discount",
                show: 'slide'
            });

            if (AddedPromotionalDiscountList.length > 0) {

                $("#TablePromotionalDiscount tbody tr").each(function () {

                    var id = _.findWhere(AddedPromotionalDiscountList, { BusinessPromotionId: parseInt($.trim($(this).find("td:eq(3)").text()), 10) });

                    if (id != null) {
                        $(this).find("td:eq(0)").find("input").attr('checked', true);
                    }
                });
            }

            CommonHelper.SpinnerClose();
        }

        function OnLoadDiscountFailed() { CommonHelper.SpinnerClose(); }

        function SaveNewRecipe() {
            //debugger;
            var newRecipeeItems = new Array();
            var recipeId, itemId, recipeItemId, recipeItemName, itemUnit, itemCost, quantity, TypeId, status;
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();

            for (var i = 0; i < NewRecipeTable.data().length; i++) {
                recipeId = NewRecipeTable.row(i).data().RecipeId;
                recipeItemName = NewRecipeTable.row(i).data().RecipeItemName;
                quantity = NewRecipeTable.row(i).data().HeadName;
                itemId = $("#ContentPlaceHolder1_hfItemId").val();
                recipeItemId = NewRecipeTable.row(i).data().RecipeItemId;
                itemCost = NewRecipeTable.row(i).data().ItemCost;
                TypeId = NewRecipeTable.row(i).data().TypeId;




                if (recipeId == 0) {
                    status = "Added";
                    newRecipeeItems.push({
                        RecipeId: recipeId,
                        ItemId: itemId,
                        RecipeItemId: recipeItemId,
                        RecipeItemName: recipeItemName,
                        ItemCost: itemCost,
                        HeadName: quantity,
                        TypeId: TypeId,
                        Status: status
                    });

                }

                //else
                //{
                //    status = "Deleted";
                //    newRecipeeItems.push({
                //        RecipeId: recipeId,
                //        ItemId: itemId,
                //        RecipeItemId: recipeItemId,
                //        RecipeItemName: recipeItemName,
                //        ItemCost: itemCost,
                //        HeadName: quantity,
                //        TypeId: TypeId,
                //        Status: status
                //    });
                //}

            }

            for (var i = 0; i < RecipeTable.data().length; i++) {
                itemId = $("#ContentPlaceHolder1_hfItemId").val();
                TypeId = $.trim(RecipeTable.cell(i, 1).nodes().to$().find('select').val());
                //TypeId = RecipeTable.row(i).data().RecipeId;
                //var itemObject = _.findWhere(previousRecipeTypesList, { ItemId: parseInt(itemId) });
                //var typeObject = _.findWhere(itemObject.RecipeModifierTypes, { TypeId: parseInt(TypeId) });
                //quantity = typeObject.HeadName;


                recipeId = RecipeTable.row(i).data().RecipeId;
                recipeItemId = RecipeTable.row(i).data().ItemId;
                itemCost = RecipeTable.row(i).data().ItemCost;
                recipeItemName = RecipeTable.row(i).data().Name;
                //quantity = RecipeTable.row(i).data().RecipeModifierTypes;
                quantity = $.trim(RecipeTable.cell(i, 1).nodes().to$().find('option:selected').text());;


                status = "Updated";




                if (TypeId > 0 || recipeId > 0) {
                    newRecipeeItems.push({
                        RecipeId: recipeId,
                        ItemId: itemId,
                        RecipeItemId: recipeItemId,
                        RecipeItemName: recipeItemName,
                        ItemCost: itemCost,
                        HeadName: quantity,
                        TypeId: TypeId,
                        Status: status
                    });


                }

            }

            for (var i = 0; i < deletedItemList.length; i++) {

                newRecipeeItems.push(deletedItemList[i]);


            }



            if (newRecipeeItems.length > 0 || deletedNewItemList.length > 0) {
                PageMethods.SaveNewRecipe(newRecipeeItems, deletedNewItemList, kotId, OnSaveNewRecipeSuccess, OnSaveNewRecipeFailed);
            }
            return false;
        }

        function OnSaveNewRecipeSuccess(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ClearEditRecipee();
            }
        }

        function OnSaveNewRecipeFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function AddNewItems() {
            //debugger;
            $('#NewRecipeDiv').show();
            var ingredientId = parseInt($('#ContentPlaceHolder1_ddlIngredient').val());
            var ingredient = $('#ContentPlaceHolder1_ddlIngredient option:selected').text();
            var typeId = $('#ContentPlaceHolder1_ddllblIngredientQuantity').val();
            var RecipeModifierTypes = $('#ContentPlaceHolder1_ddllblIngredientQuantity option:selected').text();
            if (ingredientId == 0) {
                toastr.warning("Select Ingredient Item.");
                return;
            }
            else if (typeId == 0) {
                toastr.warning("Select Ingredient Quantity.");
                return;
            }


            var duplicte = _.where(NewRecipeTable.data(), { RecipeItemId: ingredientId });

            if (duplicte.length > 0) {
                toastr.info('Ingredient is already added.');
                return;
            }
            //debugger;
            var result = _.findWhere(typesList, { Id: parseInt(typeId) });

            var rows = new Array();
            rows.push({
                RecipeId: 0,
                RecipeItemName: ingredient,
                HeadName: RecipeModifierTypes,
                ItemCost: result.TotalCost,
                RecipeItemId: ingredientId,
                TypeId: typeId

            });

            NewRecipeTable.rows.add(rows);
            NewRecipeTable.draw();

            return false;

        }



        function myFunctionTouchKeypad(val) {
            var existingValue = $("#ContentPlaceHolder1_txtTouchKeypadResult").val();
            if (val != 99) {
                $("#ContentPlaceHolder1_txtTouchKeypadResult").val(existingValue + val);
            }
            else {
                if (existingValue.length > 0) {
                    var m = existingValue.substring(0, existingValue.length - 1);
                    $("#ContentPlaceHolder1_txtTouchKeypadResult").val(m);
                }
            }
        }

        function PerformUpdateActionTouchKeypad() {
            //debugger;
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var sourceName = $("#ContentPlaceHolder1_hfsourceType").val();
            var hfCostCenterIdVal = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var selectedId = $("#ContentPlaceHolder1_hfKotDetailId").val();
            var itemQuantity = $.trim($("#ContentPlaceHolder1_txtTouchKeypadResult").val());
            var rowIndex = $("#ContentPlaceHolder1_hfRowIndex").val();

            var updatedContent = itemQuantity;
            var updateType = $("#ContentPlaceHolder1_hfChangeType").val();

            if (itemQuantity == "")
                itemQuantity = "0";

            if (parseFloat(itemQuantity) == 0) {
                toastr.warning("Please Give Valid Quantity.");
                return false;
            }
            else if (updateType == "QuantityChange" && (parseFloat(itemQuantity) < 0.25)) {
                toastr.warning("Please Give Valid Quantity.");
                return false;
            }

            //if (updateType == "UnitPriceChange") {
            //    itemQuantity = $("#TableWiseItemInformation tr:eq(" + rowIndex + ")").find("td:eq(1)").text();
            //}

            CommonHelper.SpinnerOpen();

            var isItemCanEditDelete = $("#ContentPlaceHolder1_hfIsItemCanEditDelete").val();
            ClearCardPaymentFields();
            PageMethods.UpdateIndividualItemDetailInformation(userIdAuthorised, userIdAuthorisedPassword, kotId, sourceName, isItemCanEditDelete, updateType, hfCostCenterIdVal, selectedId, itemQuantity,
                updatedContent, OnUpdateTouchKeypadObjectSucceeded, OnUpdateTouchKeypadObjectFailed);
            $("#ContentPlaceHolder1_hfIsAccessVarified").val("0");

            if ($("#ContentPlaceHolder1_hfIsUserHasEditDeleteAccess").val() == "0")
                $("#ContentPlaceHolder1_hfIsItemCanEditDelete").val("0");

            return false;
        }

        function OnUpdateTouchKeypadObjectSucceeded(result) {
            userIdAuthorised = "";
            userIdAuthorisedPassword = "";

            if (result.IsBillResettled == true) {
                CommonHelper.AlertMessage(result.AlertMessage);
                setTimeout(function () { window.location = result.RedirectUrl; }, 2000);
                return false;
            }

            $("#ContentPlaceHolder1_txtTouchKeypadResult").val('');
            $("#ContentPlaceHolder1_hfChangeType").val("");

            var keyboardnum = $('.numkbnotdecimal').getkeyboard();
            keyboardnum.destroy();
            $("#QuantityEditDialog").hide();

            LoadGridInformation();
            setTimeout(function () { $("#ContentPlaceHolder1_btnSave").attr("disabled", true); }, 2000);
            CommonHelper.SpinnerClose();

            return false;
        }

        function OnUpdateTouchKeypadObjectFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function CalculateDiscountAmount() {

            var checkDiscount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();

            if ($.trim(checkDiscount) == "")
                checkDiscount = "0";

            if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked")) {
                if (parseFloat(checkDiscount) > 100.00) {
                    toastr.info("Discount Percentage Cannot Be Greater Than (>) 100.");
                    $("#ContentPlaceHolder1_txtTPDiscountAmount").val("0");
                    $("#ContentPlaceHolder1_txtTPDiscount").val("0");
                }
            }
            else if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
                if (parseFloat(checkDiscount) > parseFloat($("#ContentPlaceHolder1_txtTotalSales").val())) {
                    toastr.info("Discount Amount Cannot Be Greater Than (>) Sales Amount.");
                    $("#ContentPlaceHolder1_txtTPDiscountAmount").val("0");
                    $("#ContentPlaceHolder1_txtTPDiscount").val("0");
                }
            }

            var isInclusiveBill = 0, vatAmount = 0.00, serviceChargeAmount = 0.00;
            var citySDCharge = 0.00, additionalCharge = 0.00, isRatePlusPlus = 0;
            var isInvoiceCitySDChargeEnable = 1, isInvoiceAdditionalChargeEnable = 1, isServiceChargeEnable = 1, isVatEnable = 1;
            var additionalChargeType = $("#ContentPlaceHolder1_hfAdditionalChargeType").val();
            var isVatOnSD = 0, isDiscountApplicableOnRackRate = 0;

            if ($("#ContentPlaceHolder1_hfIsRatePlusPlus").val() != "") { isRatePlusPlus = parseInt($("#ContentPlaceHolder1_hfIsRatePlusPlus").val(), 10); }

            if ($("#ContentPlaceHolder1_hfIsDiscountApplicableOnRackRate").val() != "") { isDiscountApplicableOnRackRate = parseInt($("#ContentPlaceHolder1_hfIsDiscountApplicableOnRackRate").val(), 10); }

            if ($("#ContentPlaceHolder1_hfIsVatOnSD").val() != "") { isVatOnSD = parseInt($("#ContentPlaceHolder1_hfIsVatOnSD").val(), 10); }

            if ($("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val() != "") { isInclusiveBill = parseInt($("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val(), 10); }

            if ($("#ContentPlaceHolder1_hfRestaurantVatAmount").val() != "")
                vatAmount = parseFloat($("#ContentPlaceHolder1_hfRestaurantVatAmount").val());

            if ($("#ContentPlaceHolder1_hfRestaurantServiceCharge").val() != "")
                serviceChargeAmount = parseFloat($("#ContentPlaceHolder1_hfRestaurantServiceCharge").val());

            if ($("#ContentPlaceHolder1_hfSDCharge").val() != "")
                citySDCharge = parseFloat($("#ContentPlaceHolder1_hfSDCharge").val());

            if ($("#ContentPlaceHolder1_hfAdditionalCharge").val() != "")
                additionalCharge = parseFloat($("#ContentPlaceHolder1_hfAdditionalCharge").val());

            if ($('#ContentPlaceHolder1_cbTPServiceCharge').is(':checked')) {
                isServiceChargeEnable = 1;
            }
            else {
                isServiceChargeEnable = 0;
                serviceChargeAmount = 0.00;
            }

            if ($('#ContentPlaceHolder1_cbTPVatAmount').is(':checked')) {
                isVatEnable = 1;
            }
            else {
                isVatEnable = 0;
                vatAmount = 0.00;
            }

            if ($("#ContentPlaceHolder1_cbTPSDCharge").is(":checked")) {
                isInvoiceCitySDChargeEnable = 1;
            }
            else {
                isInvoiceCitySDChargeEnable = 0;
                citySDCharge = 0.00;
            }

            if ($("#ContentPlaceHolder1_cbTPAdditionalCharge").is(":checked")) {
                isInvoiceAdditionalChargeEnable = 1;
            }
            else {
                isInvoiceAdditionalChargeEnable = 0;
                additionalCharge = 0.00;
            }

            var discountType = "", discount = 0.00, isComplementaryOrNonChargeable = 0;

            if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
                discountType = "Fixed"
            }
            else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked")) {
                $("#ContentPlaceHolder1_txtCash").focus();
                discountType = "Percentage"

            }
            else if ($("#ContentPlaceHolder1_rbTPComplementaryDiscount").is(":checked")) {
                $("#ContentPlaceHolder1_txtCash").focus();
                isComplementaryOrNonChargeable = 1;
                discountType = "Percentage"
            }
            else if ($("#ContentPlaceHolder1_rbTpNonChargeable").is(":checked")) {
                $("#ContentPlaceHolder1_txtCash").focus();
                isComplementaryOrNonChargeable = 1;
                discountType = "Percentage"
            }

            discount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();

            if ($.trim(discount) == "")
                discount = "0";

            var itemTableId = "TableWiseItemInformation";

            //toastr.info(discountType + "," + discount);

            var BillPaymentDetails = CommonHelper.GetKotWiseVatNSChargeNDiscountNComplementary(
                itemTableId, discountType, discount,
                serviceChargeAmount, citySDCharge, vatAmount, additionalCharge,
                additionalChargeType, isInclusiveBill, isRatePlusPlus, isVatOnSD,
                isVatEnable, isServiceChargeEnable, isInvoiceCitySDChargeEnable,
                isInvoiceAdditionalChargeEnable, isDiscountApplicableOnRackRate,
                isComplementaryOrNonChargeable
            );

            KotWiseServiceChargeVatNOther = BillPaymentDetails;
            $("#ContentPlaceHolder1_txtTPDiscount").val(BillPaymentDetails.DiscountAmount);
            //$("#ContentPlaceHolder1_txtTPDiscountedAmount").val(BillPaymentDetails.DiscountedAmount);

            if (isDiscountApplicableOnRackRate == 0) {
                $("#ContentPlaceHolder1_txtTPDiscountedAmount").val(toFixed(BillPaymentDetails.DiscountedAmount, 2));
            }
            else {
                $("#ContentPlaceHolder1_txtTPDiscountedAmount").val(toFixed(BillPaymentDetails.DiscountedAmount, 2));
            }

            if ($("#ContentPlaceHolder1_hfIsRestaurantBillAmountWillRound").val() == "1") {

                var gTotal = Math.round(BillPaymentDetails.GrandTotal);
                var roundedAmount = gTotal - BillPaymentDetails.GrandTotal;

                $("#ContentPlaceHolder1_txtGrandTotal").val(gTotal);
                $("#ContentPlaceHolder1_txtTPGrandTotal").val(gTotal);
                $("#ContentPlaceHolder1_txtChangeAmount").val(gTotal);
                $("#ContentPlaceHolder1_txtTPChangeAmount").val(gTotal);
                $("#ContentPlaceHolder1_txtTPRoundedAmount").val(roundedAmount);
                $("#ContentPlaceHolder1_txtTPSDCharge").val(toFixed(BillPaymentDetails.SDCityCharge, 2));
                $("#ContentPlaceHolder1_txtTPAdditionalCharge").val(toFixed(BillPaymentDetails.AdditionalCharge, 2));

                $("#ContentPlaceHolder1_txtCitySDCharge").val(toFixed(BillPaymentDetails.SDCityCharge, 2));
                $("#ContentPlaceHolder1_txtAdditionalCharge").val(toFixed(BillPaymentDetails.AdditionalCharge, 2));
            }
            else {

                $("#ContentPlaceHolder1_txtGrandTotal").val(toFixed(BillPaymentDetails.GrandTotal, 2));
                $("#ContentPlaceHolder1_txtTPGrandTotal").val(toFixed(BillPaymentDetails.GrandTotal, 2));
                $("#ContentPlaceHolder1_txtChangeAmount").val(toFixed(BillPaymentDetails.GrandTotal, 2));
                $("#ContentPlaceHolder1_txtTPChangeAmount").val(toFixed(BillPaymentDetails.GrandTotal, 2));

                $("#ContentPlaceHolder1_txtTPRoundedAmount").val("0");
                $("#ContentPlaceHolder1_txtTPSDCharge").val('0');
                $("#ContentPlaceHolder1_txtTPAdditionalCharge").val('0');

                $("#ContentPlaceHolder1_txtCitySDCharge").val('0');
                $("#ContentPlaceHolder1_txtAdditionalCharge").val('0');
            }

            $("#ContentPlaceHolder1_txtTPServiceCharge").val(toFixed(BillPaymentDetails.ServiceCharge, 2));
            $("#ContentPlaceHolder1_txtTPVatAmount").val(toFixed(BillPaymentDetails.VatAmount, 2));

            $("#ContentPlaceHolder1_txtTPSDCharge").val(toFixed(BillPaymentDetails.SDCityCharge, 2));
            $("#ContentPlaceHolder1_txtTPAdditionalCharge").val(toFixed(BillPaymentDetails.AdditionalCharge, 2));

            $("#ContentPlaceHolder1_txtServiceCharge").val(toFixed(BillPaymentDetails.ServiceCharge, 2));
            $("#ContentPlaceHolder1_txtVatAmount").val(toFixed(BillPaymentDetails.VatAmount, 2));

            $("#ContentPlaceHolder1_txtCitySDCharge").val(toFixed(BillPaymentDetails.SDCityCharge, 2));
            $("#ContentPlaceHolder1_txtAdditionalCharge").val(toFixed(BillPaymentDetails.AdditionalCharge, 2));

            CalculatePayment();
        }

        function CalculatePayment() {

            if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked")) {
                if (parseFloat($("#ContentPlaceHolder1_txtTPDiscountAmount").val()) > 100.00) {
                    toastr.info("Discount Percentage Cannot Be Greater Than (>) 100.");
                    $("#ContentPlaceHolder1_txtTPDiscountAmount").val("0");
                    $("#ContentPlaceHolder1_txtTPDiscount").val("0");
                }
            }
            else if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
                if (parseFloat($("#ContentPlaceHolder1_txtTPDiscountAmount").val()) > parseFloat($("#ContentPlaceHolder1_txtTotalSales").val())) {
                    toastr.info("Discount Amount Cannot Be Greater Than (>) Sales Amount.");
                    $("#ContentPlaceHolder1_txtTPDiscountAmount").val("0");
                    $("#ContentPlaceHolder1_txtTPDiscount").val("0");
                }
            }

            //toastr.info("CalculatePayment");

            var discountAmount = $("#ContentPlaceHolder1_txtTPDiscount").val();
            var salesAmount = $("#ContentPlaceHolder1_txtTotalSales").val();
            var grandTotal = parseFloat($("#ContentPlaceHolder1_txtTPGrandTotal").val());

            if (discountAmount == "") { discountAmount = "0"; }

            //var discountedAmount = 0;

            //discountedAmount = (parseFloat(salesAmount) - parseFloat(discountAmount));
            //$("#ContentPlaceHolder1_txtTPDiscountedAmount").val(discountedAmount);

            FocusedInputControl = $(document.activeElement).attr("id");
            var activeControlValue = $(document.activeElement).val();

            var cashPayment = $.trim($("#ContentPlaceHolder1_txtCash").val()) == "" ? "0" : $("#ContentPlaceHolder1_txtCash").val();
            var amexCardPayment = $.trim($("#ContentPlaceHolder1_txtAmexCard").val()) == "" ? "0" : $("#ContentPlaceHolder1_txtAmexCard").val();
            var masterCardPayment = $.trim($("#ContentPlaceHolder1_txtMasterCard").val()) == "" ? "0" : $("#ContentPlaceHolder1_txtMasterCard").val();
            var visaCardPayment = $.trim($("#ContentPlaceHolder1_txtVisaCard").val()) == "" ? "0" : $("#ContentPlaceHolder1_txtVisaCard").val();
            var discoverCardPayment = $.trim($("#ContentPlaceHolder1_txtDiscoverCard").val()) == "" ? "0" : $("#ContentPlaceHolder1_txtDiscoverCard").val();
            var mBankingAmount = $.trim($("#ContentPlaceHolder1_txtMBankingPayment").val()) == "" ? "0" : $("#ContentPlaceHolder1_txtMBankingPayment").val();
            var companyAmount = $.trim($("#ContentPlaceHolder1_txtCompanyPayment").val()) == "" ? "0" : $("#ContentPlaceHolder1_txtCompanyPayment").val();
            var roundedAmount = $.trim($("#ContentPlaceHolder1_txtTPRoundedAmount").val()) == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtTPRoundedAmount").val());
            var guestRoomPayment = $.trim($("#ContentPlaceHolder1_txtRoomPayment").val()) == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtRoomPayment").val());
            var employeePayment = $.trim($("#ContentPlaceHolder1_txtEmployeePayment").val()) == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtEmployeePayment").val());
            var memberPayment = $.trim($("#ContentPlaceHolder1_txtMemberPayment").val()) == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtMemberPayment").val());

            var totalPayment = parseFloat(cashPayment) + parseFloat(amexCardPayment) + parseFloat(mBankingAmount) +
                parseFloat(masterCardPayment) + parseFloat(visaCardPayment) + parseFloat(discoverCardPayment) + parseFloat(companyAmount) +
                parseFloat(guestRoomPayment) + parseFloat(employeePayment) + parseFloat(memberPayment);

            totalPayment = toFixed(totalPayment, 2);

            if (isNaN(totalPayment))
                return false;

            if (FocusedInputControl != "ContentPlaceHolder1_txtCash") {

                var dueAmount = $("#ContentPlaceHolder1_txtTPChangeAmount").val();

                if (amexCardPayment != "0" && amexCardPayment == activeControlValue) {
                    if (parseFloat(totalPayment) > parseFloat(grandTotal)) {
                        $("#ContentPlaceHolder1_txtAmexCard").val("");
                        toastr.warning("Card Payment Amount Cannot Be Greater Than Due Amount.");
                        amexCardPayment = "0";
                    }
                }
                else if (masterCardPayment != "0" && masterCardPayment == activeControlValue) {
                    if (parseFloat(totalPayment) > parseFloat(grandTotal)) {
                        $("#ContentPlaceHolder1_txtMasterCard").val("");
                        toastr.warning("Card Payment Amount Cannot Be Greater Than Due Amount.");
                        masterCardPayment = "0";
                    }
                }
                else if (visaCardPayment != "0" && visaCardPayment == activeControlValue) {
                    if (parseFloat(totalPayment) > parseFloat(grandTotal)) {
                        $("#ContentPlaceHolder1_txtVisaCard").val("");
                        toastr.warning("Card Payment Amount Cannot Be Greater Than Due Amount.");
                        visaCardPayment = "0";
                    }
                }
                else if (discoverCardPayment != "0" && discoverCardPayment == activeControlValue) {
                    if (parseFloat(totalPayment) > parseFloat(grandTotal)) {
                        $("#ContentPlaceHolder1_txtDiscoverCard").val("");
                        toastr.warning("Card Payment Amount Cannot Be Greater Than Due Amount.");
                        discoverCardPayment = "0";
                    }
                }
                else if (guestRoomPayment != "0" && guestRoomPayment == activeControlValue) {
                    if (parseFloat(totalPayment) > parseFloat(grandTotal)) {
                        $("#ContentPlaceHolder1_txtRoomPayment").val("");
                        toastr.warning("Room Payment Amount Cannot Be Greater Than Due Amount.");
                        guestRoomPayment = "0";
                    }
                }
                else if (mBankingAmount != "0" && mBankingAmount == activeControlValue) {
                    if (parseFloat(totalPayment) > parseFloat(grandTotal)) {
                        $("#ContentPlaceHolder1_txtMBankingPayment").val("");
                        toastr.warning("M-Banking Payment Amount Cannot Be Greater Than Due Amount.");
                        companyAmount = "0";
                    }
                }
                else if (companyAmount != "0" && companyAmount == activeControlValue) {
                    if (parseFloat(totalPayment) > parseFloat(grandTotal)) {
                        $("#ContentPlaceHolder1_txtCompanyPayment").val("");
                        toastr.warning("Company Payment Amount Cannot Be Greater Than Due Amount.");
                        companyAmount = "0";
                    }
                }
                else if (employeePayment != "0" && employeePayment == activeControlValue) {
                    if (parseFloat(totalPayment) > parseFloat(grandTotal)) {
                        $("#ContentPlaceHolder1_txtEmployeePayment").val("");
                        toastr.warning("Employee Payment Amount Cannot Be Greater Than Due Amount.");
                        employeePayment = "0";
                    }
                }
                else if (memberPayment != "0" && memberPayment == activeControlValue) {
                    if (parseFloat(totalPayment) > parseFloat(grandTotal)) {
                        $("#ContentPlaceHolder1_txtMemberPayment").val("");
                        toastr.warning("Member Payment Amount Cannot Be Greater Than Due Amount.");
                        memberPayment = "0";
                    }
                }
            }

            totalPayment = parseFloat(cashPayment) + parseFloat(amexCardPayment) + parseFloat(mBankingAmount) +
                parseFloat(masterCardPayment) + parseFloat(visaCardPayment) + parseFloat(discoverCardPayment) + parseFloat(companyAmount) +
                parseFloat(guestRoomPayment) + parseFloat(employeePayment) + parseFloat(memberPayment);

            totalPayment = toFixed(totalPayment, 2);
            billAmount = grandTotal;

            $("#ContentPlaceHolder1_txtTPTotalPaymentAmount").val(totalPayment);

            if ($("#ContentPlaceHolder1_hfIsRestaurantBillAmountWillRound").val() == "1") {
                billAmount = Math.round(billAmount);
            }

            $("#ContentPlaceHolder1_txtRoundedAmount").val(roundedAmount);

            if (toFixed((billAmount - totalPayment), 2) > 0) {
                $("#ContentPlaceHolder1_lblTPChangeAmount").text('Due Amount');
                if ($("#ContentPlaceHolder1_hfIsRestaurantBillAmountWillRound").val() == "1") {
                    $("#ContentPlaceHolder1_txtTPChangeAmount").val(toFixed(Math.round(parseFloat(billAmount - totalPayment)), 2));
                }
                else {
                    $("#ContentPlaceHolder1_txtTPChangeAmount").val(CommonHelper.Decimal2Point(toFixed((billAmount - totalPayment), 2)));
                }
            }
            else {
                $("#ContentPlaceHolder1_lblTPChangeAmount").text('Change Amount');

                if ($("#ContentPlaceHolder1_hfIsRestaurantBillAmountWillRound").val() == "1") {
                    $("#ContentPlaceHolder1_txtTPChangeAmount").val(Math.round((-1) * (toFixed(parseFloat(billAmount - totalPayment), 2))));
                }
                else {
                    $("#ContentPlaceHolder1_txtTPChangeAmount").val(CommonHelper.Decimal2Point((-1) * (parseFloat(billAmount - totalPayment))));
                }
            }

            if ($.trim(salesAmount) != "") {
                $("#ContentPlaceHolder1_txtTotalPaymentAmount").val(totalPayment);
                $("#ContentPlaceHolder1_txtChangeAmount").val($("#ContentPlaceHolder1_txtTPChangeAmount").val());
                $("#ContentPlaceHolder1_lblTotalChangeAmount").text($("#ContentPlaceHolder1_lblTPChangeAmount").text());
            }

            var dueAmountText = $.trim($("#ContentPlaceHolder1_lblTPChangeAmount").text());
            var dueAmount = $("#ContentPlaceHolder1_txtTPChangeAmount").val();

            if (dueAmount == "")
                dueAmount = "0";

            if (dueAmountText != "Due Amount" || parseFloat(dueAmount) == 0) {
                $("#ContentPlaceHolder1_btnSave").attr("disabled", false);
            }
            else {
                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
            }
        }

        function RoundedAmountCalculate() {
            if ($.trim($("#ContentPlaceHolder1_txtRoundedAmount").val()) == "" || $.trim($("#ContentPlaceHolder1_txtRoundedAmount").val()) == "0") {
                return;
            }

            var roundedAmount = parseFloat($("#ContentPlaceHolder1_txtRoundedAmount").val());
            var grandTotal = parseFloat($("#ContentPlaceHolder1_txtGrandTotal").val());

            var amountAfterRound = grandTotal - roundedAmount;

            $("#ContentPlaceHolder1_txtTPGrandTotal").val(amountAfterRound);
            $("#ContentPlaceHolder1_txtGrandTotal").val(amountAfterRound);
        }

        function OnUpdateRestaurantBillSucceed(result) {
            if (result.IsSuccess == true) {

                var iframeid = 'printDoc';
                var url = "/POS/Reports/frmReportTPBillInfo.aspx?billID=" + result.Pk;
                parent.document.getElementById(iframeid).src = url;

                $("#displayBill").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 'auto',
                    height: 'auto',
                    minWidth: 550,
                    minHeight: 580,
                    closeOnEscape: false,
                    resizable: false,
                    fluid: true,
                    title: "Invoice Preview",
                    show: 'slide'
                    //,close: ClosePrintDialog
                });

                setTimeout(function () { ScrollToBottom(); }, 1000);

            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);

            }
        }
        function OnUpdateRestaurantBillFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function DeleteItem(item) {

            var row = $(item).parents('tr');

            var id = RecipeTable.row(row).data().ItemId;

            if (id > 0) {
                var TypeId, recipeId, recipeItemId, itemCost, recipeItemName, quantity, itemId, status;

                TypeId = $.trim(RecipeTable.cell(row, 1).nodes().to$().find('select').val());
                recipeId = RecipeTable.row(row).data().RecipeId;
                recipeItemId = RecipeTable.row(row).data().ItemId;
                itemCost = RecipeTable.row(row).data().ItemCost;
                recipeItemName = RecipeTable.row(row).data().Name;
                //quantity = RecipeTable.row(row).data().RecipeModifierTypes;
                quantity = $.trim(RecipeTable.cell(row, 1).nodes().to$().find('option:selected').text());
                itemId = $("#ContentPlaceHolder1_hfItemId").val();
                status = "Deleted";

                deletedItemList.push({
                    RecipeId: recipeId,
                    ItemId: itemId,
                    RecipeItemId: recipeItemId,
                    RecipeItemName: recipeItemName,
                    ItemCost: itemCost,
                    HeadName: quantity,
                    TypeId: TypeId,
                    Status: status
                });


            }
            //debugger;
            RecipeTable.row(row).remove().page('next').draw('page');

            return false;
        }

        function NewDeleteItem(item) {
            //debugger;

            var row = $(item).parents('tr');

            var id = NewRecipeTable.row(row).data().RecipeId;

            if (id > 0) {

                deletedNewItemList.push(id);
            }

            NewRecipeTable.row(row).remove().draw(false);
        }

        function OrderSubmit() {

            if ($("#TableWiseItemInformation tbody tr").length == 0) {
                toastr.info("Please Add item Before Order Submit");
                return;
            }

            var costcenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var sourceNumber = $("#ContentPlaceHolder2_lblOrderSourceNumber").text();

            var kotIdLst = "";

            if (AddedTableList.length > 0) {
                $(AddedTableList).each(function (index, item) {
                    if (kotIdLst != "") {
                        kotIdLst += "," + item.KotId;
                    }
                    else {
                        kotIdLst = item.KotId;
                    }
                });
            }

            if (kotIdLst == "")
                kotIdLst = kotId;
            else
                kotIdLst += "," + kotId;

            PageMethods.KotSubmit(costcenterId, kotId, kotIdLst, sourceNumber, OnKotSubmitSucceed, OnKotSubmitFailed);
            return false;
        }
        function OnKotSubmitSucceed(result) {
            if (result.IsSuccess == true) {
                LoadGridInformation();
                CommonHelper.AlertMessage(result.AlertMessage);

                if ($("#ContentPlaceHolder1_hfsourceType").val() == "RestaurantTable")
                    window.location = "frmCostCenterSelectionForAll.aspx";
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);

            }
        }
        function OnKotSubmitFailed() { $loading.hide(); }

        function OrderPreview() {

            if ($("#TableWiseItemInformation tbody tr").length == 0) {
                toastr.info("No Item Added to Print KOT.");
                return false;
            }

            var sourceType = $.trim(CommonHelper.GetParameterByName("st"));
            var sourceId = CommonHelper.GetParameterByName("sid");
            var costCenterId = CommonHelper.GetParameterByName("cc");
            var sourceNo = $("#ContentPlaceHolder2_lblOrderSourceNumber").text();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();

            var kotIdLst = "";

            if (AddedTableList.length > 0) {
                $(AddedTableList).each(function (index, item) {
                    if (kotIdLst != "") {
                        kotIdLst += "," + item.KotId;
                    }
                    else {
                        kotIdLst = item.KotId;
                    }
                });
            }

            if (kotIdLst == "")
                kotIdLst = kotId;
            else
                kotIdLst += "," + kotId;

            var iframeid = 'printDoc';
            var url = "/POS/Reports/frmTReportKotBillInfo.aspx?st=" + sourceType + "&kotId=" + kotId + "&kotIdLst=" + kotIdLst + "&tno=" + sourceId + "&kbm=" + "kotb" + "&isrp=" + "rp";
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 'auto',
                height: 'auto',
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "KOT Preview",
                show: 'slide'
            });

            $("#ContentPlaceHolder1_hfIsBillAlreadyPrint").val("1");
            setTimeout(function () { ScrollToBottom(); LoadGridInformation(); }, 1000);
        }

        function BillPreview() {

            if ($("#TableWiseItemInformation tbody tr").length == 0) {
                toastr.info("Please Add item Before Bill Preview");
                return;
            }

            if (IsBillOnPreview == "1") {
                toastr.info("Bill Already In Processed For Preview.");
                return false;
            }

            IsBillOnPreview = "1";
            CommonHelper.SpinnerOpen();

            var costcenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var discountType = "Fixed", isComplementary = false, discountTransactionId = 0, isComplementary = false, isNonChargeable = false;

            if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
                discountType = "Fixed";
            }
            else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked")) {
                discountType = "Percentage";
            }
            else if ($("#ContentPlaceHolder1_rbTPComplementaryDiscount").is(":checked")) {
                discountType = "Percentage";
                isComplementary = true;
            }
            else if ($("#ContentPlaceHolder1_rbTpNonChargeable").is(":checked")) {
                discountType = "Percentage";
                isNonChargeable = true;
            }

            if ($("#ContentPlaceHolder1_hfPromotionalDiscountType").val() != "" && $("#ContentPlaceHolder1_rbTPComplementaryDiscount").is(":checked") == false) {
                discountType = $.trim($("#ContentPlaceHolder1_hfPromotionalDiscountType").val());
                discountTransactionId = AddedPromotionalDiscountList[0].BusinessPromotionId;
            }

            var customerName = "", remarks = "";
            var guestList = $("#ContentPlaceHolder1_hfGuestList").val();

            if ($.trim(guestList) == "" && $("#lblGuestRoom").text() != "Room Payment" && $("#ContentPlaceHolder1_hfRoomId").val() != "") {
                guestList = $("#lblGuestRoom").text();
                customerName = guestList;
            } else if ($.trim(guestList) != "" && $("#lblGuestRoom").text() != "Room Payment" && $("#ContentPlaceHolder1_hfRoomId").val() != "") {
                customerName = guestList;
            }

            if ($.trim($("#ContentPlaceHolder1_hfGuestList").val()) == "") {
                customerName = "";
            }

            if ($.trim($("#ContentPlaceHolder1_txtRemarks").val()) != "") {
                remarks = $.trim($("#ContentPlaceHolder1_txtRemarks").val());
            }

            var calculatedDiscountAmount = "0";
            var discountAmount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTPDiscountAmount").val();

            var salesAmountTP = $("#ContentPlaceHolder1_txtTotalSales").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTotalSales").val();
            var discountedAmountTP = $("#ContentPlaceHolder1_txtTPDiscountedAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTPDiscountedAmount").val();

            if (parseFloat(discountedAmountTP) != 0)
                calculatedDiscountAmount = parseFloat(salesAmountTP) - parseFloat(discountedAmountTP);

            if (discountType == 'Percentage' && parseFloat(discountAmount) == 100) {
                calculatedDiscountAmount = parseFloat(salesAmountTP) - parseFloat(discountedAmountTP);
            }

            var serviceCharge = $("#ContentPlaceHolder1_txtServiceCharge").val() == "" ? "0" : $("#ContentPlaceHolder1_txtServiceCharge").val();
            var vatAmount = $("#ContentPlaceHolder1_txtVatAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtVatAmount").val();

            var citySDCharge = $("#ContentPlaceHolder1_txtCitySDCharge").val() == "" ? "0" : $("#ContentPlaceHolder1_txtCitySDCharge").val();
            var additionalCharge = $("#ContentPlaceHolder1_txtAdditionalCharge").val() == "" ? "0" : $("#ContentPlaceHolder1_txtAdditionalCharge").val();
            var additionalChargeType = $("#ContentPlaceHolder1_hfAdditionalChargeType").val();

            var paxQuantity = $("#ContentPlaceHolder1_hfPaxQuantity").val();
            var sourceInfoId = $("#ContentPlaceHolder1_hfKotId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfKotId").val();

            var sourceName = $("#ContentPlaceHolder1_hfsourceType").val();
            var bearerId = $("#ContentPlaceHolder1_hfBearerId").val();
            var sourceId = $("#ContentPlaceHolder1_hfSourceId").val();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var registrationId = $("#ContentPlaceHolder1_hfRegistrationId").val();
            var roomId = $("#ContentPlaceHolder1_hfRoomId").val();
            roomId = roomId == "" ? "0" : roomId;

            if (sourceInfoId == "")
                sourceInfoId = "0";

            if (sourceInfoId != "0") {
                tableId = sourceInfoId;
            }

            var billPaidBySourceId = tableId;
            var isInvoiceServiceChargeEnable = false, isInvoiceVatAmountEnable = false;
            var isInvoiceCitySDChargeEnable = false, isInvoiceAdditionalChargeEnable = false;

            if ($("#ContentPlaceHolder1_cbTPServiceCharge").is(":checked")) {
                isInvoiceServiceChargeEnable = true;
            }
            else {
                isInvoiceServiceChargeEnable = false;
            }

            if ($("#ContentPlaceHolder1_cbTPVatAmount").is(":checked")) {
                isInvoiceVatAmountEnable = true;
            }
            else {
                isInvoiceVatAmountEnable = false;
            }

            if ($("#ContentPlaceHolder1_cbTPSDCharge").is(":checked")) {
                isInvoiceCitySDChargeEnable = true;
            }
            else {
                isInvoiceCitySDChargeEnable = false;
            }

            if ($("#ContentPlaceHolder1_cbTPAdditionalCharge").is(":checked")) {
                isInvoiceAdditionalChargeEnable = true;
            }
            else {
                isInvoiceAdditionalChargeEnable = false;
            }


            var billId = "0";

            if ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "") {
                billId = $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val();
            }

            var detailId = "0";

            if ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "" && $("#ContentPlaceHolder1_hfBillIdDetailsId").val() != "") {
                detailId = parseInt($("#ContentPlaceHolder1_hfBillIdDetailsId").val(), 10);
            }

            var BillDetail = new Array();

            if (AddedTableList.length > 0) {

                BillDetail = JSON.parse(JSON.stringify(AddedTableList));

                BillDetail.push({
                    TableId: sourceId,
                    KotId: kotId,
                    DetailId: detailId,
                    BillId: billId,
                    MainBillId: billId
                });
            }
            else {
                BillDetail.push({
                    TableId: sourceId,
                    KotId: kotId,
                    DetailId: detailId,
                    BillId: billId,
                    MainBillId: billId
                });
            }

            var isRestaurantBillInclusive = $("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val() == "" ? "0" : $("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val();
            var salesAmount = "0", grandTotal = "0", roundedAmount = "0";

            roundedAmount = $("#ContentPlaceHolder1_txtTPRoundedAmount").val();
            grandTotal = $("#ContentPlaceHolder1_txtGrandTotal").val();
            salesAmount = $("#ContentPlaceHolder1_txtSalesAmount").val();

            //if (isRestaurantBillInclusive == "0") {
            //    salesAmount = $("#ContentPlaceHolder1_txtSalesAmount").val();
            //    grandTotal = $("#ContentPlaceHolder1_txtGrandTotal").val();
            //}
            //else {
            //    salesAmount = $("#ContentPlaceHolder1_txtGrandTotal").val() == "" ? "0" : $("#ContentPlaceHolder1_txtGrandTotal").val();
            //    grandTotal = $("#ContentPlaceHolder1_txtSalesAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtSalesAmount").val();

            //    grandTotal = parseFloat(salesAmount) - parseFloat(calculatedDiscountAmount);
            //}

            var RestaurantBill = {
                CostCenterId: costcenterId,
                DiscountType: $.trim(discountType),
                IsComplementary: isComplementary,
                IsNonChargeable: isNonChargeable,
                DiscountTransactionId: discountTransactionId,
                DiscountAmount: discountAmount,
                CalculatedDiscountAmount: calculatedDiscountAmount,

                ServiceCharge: serviceCharge,
                VatAmount: vatAmount,
                CitySDCharge: citySDCharge,
                AdditionalCharge: additionalCharge,
                AdditionalChargeType: additionalChargeType,

                CustomerName: customerName,
                PaxQuantity: paxQuantity,
                TableId: sourceId,
                SourceName: sourceName,
                TransactionType: '',
                TransactionId: '',
                KotId: kotId,
                RegistrationId: registrationId,
                RoomId: roomId,
                BearerId: bearerId,
                BillPaidBySourceId: sourceId,

                IsInvoiceServiceChargeEnable: isInvoiceServiceChargeEnable,
                IsInvoiceVatAmountEnable: isInvoiceVatAmountEnable,
                IsInvoiceCitySDChargeEnable: isInvoiceCitySDChargeEnable,
                IsInvoiceAdditionalChargeEnable: isInvoiceAdditionalChargeEnable,

                InvoiceServiceRate: KotWiseServiceChargeVatNOther.RackRate,
                SalesAmount: salesAmount,
                GrandTotal: grandTotal,
                RoundedAmount: roundedAmount,
                RoundedGrandTotal: grandTotal,
                Remarks: remarks
            };

            var GuestBillPayment = new Array();

            var txtTPDiscountedAmountHiddenFieldVal = $("#ContentPlaceHolder1_txtTPDiscountedAmount").val();

            var txtCashVal = $("#ContentPlaceHolder1_txtCash").val();
            var txtAmexCardVal = $("#ContentPlaceHolder1_txtAmexCard").val();
            var txtMasterCardVal = $("#ContentPlaceHolder1_txtMasterCard").val();
            var txtVisaCardVal = $("#ContentPlaceHolder1_txtVisaCard").val();
            var txtDiscoverCardVal = $("#ContentPlaceHolder1_txtDiscoverCard").val();
            var txtTPDiscountAmountVal = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();
            var guestRoomPayment = $("#ContentPlaceHolder1_txtRoomPayment").val();

            var companyAmount = $("#ContentPlaceHolder1_txtCompanyPayment").val();
            var companyName = $("#lblCompanyName").text();
            var companyId = $("#ContentPlaceHolder1_hfCompanyId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfCompanyId").val();

            var employeePayment = $("#ContentPlaceHolder1_txtEmployeePayment").val();
            var employeeName = $("#lblEmployeeName").text();
            var employeeId = $("#ContentPlaceHolder1_hfEmployeeId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfEmployeeId").val();

            var memberPayment = $("#ContentPlaceHolder1_txtMemberPayment").val();
            var memberName = $("#txtMemberName").val();
            var memberId = $("#ContentPlaceHolder1_hfMemberId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfMemberId").val();

            if (employeeId != "0") {
                RestaurantBill.TransactionType = 'Employee';
                RestaurantBill.TransactionId = employeeId;
                RestaurantBill.CustomerName = employeeName;
            }
            else if (memberId != "0") {
                RestaurantBill.TransactionType = 'Member';
                RestaurantBill.TransactionId = memberId;
                RestaurantBill.CustomerName = memberName;
            }
            else if (companyId != "0") {
                RestaurantBill.TransactionType = 'Company';
                RestaurantBill.TransactionId = companyId;
                RestaurantBill.CustomerName = companyName;
            }
            else {
                RestaurantBill.TransactionType = null;
                RestaurantBill.TransactionId = null;
            }

            var hfLocalCurrencyId = 'ContentPlaceHolder1_hfLocalCurrencyId'
            var hfLocalCurrencyIdVal = $('#' + hfLocalCurrencyId).val();

            var DiscountTypeVal = 'Fixed'

            if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(':checked')) {
                DiscountTypeVal = "Fixed";
            }
            else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(':checked')) {
                DiscountTypeVal = "Percentage";
            }

            var PaymentMode = new Array();
            PaymentMode.push({ CardType: '', PaymentType: 'Advance', PaymentDescription: '', PaymentMode: 'Cash', PaymentById: '', Control: "ContentPlaceHolder1_txtCash" });
            PaymentMode.push({ CardType: 'a', PaymentType: 'Advance', PaymentDescription: 'American Express', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtAmexCard" });
            PaymentMode.push({ CardType: 'm', PaymentType: 'Advance', PaymentDescription: 'Master Card', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtMasterCard" });
            PaymentMode.push({ CardType: 'v', PaymentType: 'Advance', PaymentDescription: 'Visa Card', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtVisaCard" });
            PaymentMode.push({ CardType: 'd', PaymentType: 'Advance', PaymentDescription: 'Discover Card', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtDiscoverCard" });
            PaymentMode.push({ CardType: '', PaymentType: 'Company', PaymentDescription: companyName, PaymentMode: 'Company', PaymentById: $("#ContentPlaceHolder1_hfCompanyId").val(), Control: "ContentPlaceHolder1_txtCompanyPayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'GuestRoom', PaymentDescription: 'GuestRoom', PaymentMode: 'Other Room', PaymentById: $("#ContentPlaceHolder1_hfRoomId").val(), Control: "ContentPlaceHolder1_txtRoomPayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'Employee', PaymentDescription: employeeName, PaymentMode: 'Employee', PaymentById: $("#ContentPlaceHolder1_hfEmployeeId").val(), Control: "ContentPlaceHolder1_txtEmployeePayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'Member', PaymentDescription: memberName, PaymentMode: 'Member', PaymentById: $("#ContentPlaceHolder1_hfMemberId").val(), Control: "ContentPlaceHolder1_txtMemberPayment" });

            var paymodeCount = PaymentMode.length, row = 0;
            var paymentAmount = "0";
            var pDescription = "";
            var paymentCounter = 1;
            var paymentById = 0;

            for (row = 0; row < paymodeCount; row++) {

                paymentById = $.trim(PaymentMode[row].PaymentById) == "" ? "0" : PaymentMode[row].PaymentById;

                if ($("#" + PaymentMode[row].Control).val() != "" && $("#" + PaymentMode[row].Control).val() != "0") {
                    paymentAmount = $("#" + PaymentMode[row].Control).val();

                    if (paymentAmount == "" || paymentAmount == "0")
                        continue;

                    GuestBillPayment.push({
                        NodeId: 0,
                        PaymentType: PaymentMode[row].PaymentType,
                        AccountsPostingHeadId: 0,
                        BillPaidBy: 0,
                        BankId: 0,
                        RegistrationId: paymentById,
                        FieldId: hfLocalCurrencyIdVal,
                        ConvertionRate: 1,
                        CurrencyAmount: paymentAmount,
                        PaymentAmount: paymentAmount,
                        ChecqueDate: new Date(),
                        PaymentMode: PaymentMode[row].PaymentMode,
                        PaymentId: 1,
                        CardNumber: "",
                        CardType: PaymentMode[row].CardType,
                        ExpireDate: null,
                        ChecqueNumber: "",
                        CardHolderName: "",
                        PaymentDescription: PaymentMode[row].PaymentDescription,
                        CompanyId: companyId
                    });
                }
                else if ((isComplementary == true || isNonChargeable == true) && paymentCounter == 1) {
                    paymentAmount = $("#" + PaymentMode[row].Control).val();

                    if ($.trim(paymentAmount) != "") {
                        paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;

                        GuestBillPayment.push({
                            NodeId: 0,
                            PaymentType: PaymentMode[row].PaymentType,
                            AccountsPostingHeadId: 0,
                            BillPaidBy: 0,
                            BankId: 0,
                            RegistrationId: paymentById,
                            FieldId: hfLocalCurrencyIdVal,
                            ConvertionRate: 1,
                            CurrencyAmount: paymentAmount,
                            PaymentAmount: paymentAmount,
                            ChecqueDate: new Date(),
                            PaymentMode: PaymentMode[row].PaymentMode,
                            PaymentId: 1,
                            CardNumber: "",
                            CardType: PaymentMode[row].CardType,
                            ExpireDate: null,
                            ChecqueNumber: "",
                            CardHolderName: "",
                            PaymentDescription: PaymentMode[row].PaymentDescription,
                            CompanyId: companyId
                        });

                        paymentCounter = paymentCounter + 1;
                    }
                }
            }

            if ($("#ContentPlaceHolder1_txtTPRoundedAmount").val() != "0" && $("#ContentPlaceHolder1_txtTPRoundedAmount").val() != "") {

                roundedAmount = $("#ContentPlaceHolder1_txtTPRoundedAmount").val();

                if (roundedAmount != "" || roundedAmount != "0") {

                    GuestBillPayment.push({
                        NodeId: 0,
                        PaymentType: "Rounded",
                        AccountsPostingHeadId: 0,
                        BillPaidBy: 0,
                        BankId: 0,
                        RegistrationId: 0,
                        FieldId: hfLocalCurrencyIdVal,
                        ConvertionRate: 1,
                        CurrencyAmount: roundedAmount,
                        PaymentAmount: roundedAmount,
                        ChecqueDate: new Date(),
                        PaymentMode: "Rounded",
                        PaymentId: 1,
                        CardNumber: "",
                        CardType: "",
                        ExpireDate: null,
                        ChecqueNumber: "",
                        CardHolderName: "",
                        PaymentDescription: "",
                        CompanyId: companyId
                    });
                }
            }

            if ($("#ContentPlaceHolder1_lblTPChangeAmount").text() == "Change Amount" && $("#ContentPlaceHolder1_lblTotalChangeAmount").text() == "Change Amount") {
                if ($("#ContentPlaceHolder1_txtTPChangeAmount").val() != "0" && $("#ContentPlaceHolder1_txtTPChangeAmount").val() != "") {

                    var changeAmount = $("#ContentPlaceHolder1_txtTPChangeAmount").val();

                    if (changeAmount != "" || changeAmount != "0") {

                        GuestBillPayment.push({
                            NodeId: 0,
                            PaymentType: "Refund",
                            AccountsPostingHeadId: 0,
                            BillPaidBy: 0,
                            BankId: 0,
                            RegistrationId: 0,
                            FieldId: hfLocalCurrencyIdVal,
                            ConvertionRate: 1,
                            CurrencyAmount: changeAmount,
                            PaymentAmount: changeAmount,
                            ChecqueDate: new Date(),
                            PaymentMode: "Refund",
                            PaymentId: 1,
                            CardNumber: "",
                            CardType: "",
                            ExpireDate: null,
                            ChecqueNumber: "",
                            CardHolderName: "",
                            PaymentDescription: "",
                            CompanyId: companyId
                        });
                    }
                }
            }

            if ($("#ContentPlaceHolder1_hfIsResumeBill").val() == "1" && $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "") {

                //var BillDetailsNew = new Array();
                //BillDetailsNew = _.findWhere(BillDetail, { DetailId: 0 });

                $.ajax({
                    type: "POST",
                    url: "../../../Common/WebMethodPage.aspx/UpdateRestauranBillGenerationNew",
                    data: JSON.stringify({
                        RestaurantBill: RestaurantBill, GuestBillPayment: GuestBillPayment,
                        BillDetail: BillDetail, BillDeletedDetail: DeletedTableList,
                        AddedClassificationList: AddedClassificationList,
                        DeletedClassificationList: DeletedClassificationList
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {

                        if (result.d.IsBillResettled == true) {
                            CommonHelper.AlertMessage(result.d.AlertMessage);
                            setTimeout(function () { window.location = result.d.RedirectUrl; }, 2000);
                            return false;
                        }

                        if (result.d.IsSuccess == true) {

                            IsBillOnPreview = "0";
                            $("#ContentPlaceHolder1_btnClear").attr("disabled", true);

                            $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                            $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val(result.d.Pk);
                            $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);

                            if ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "") {
                                var iframeid = 'printDoc';
                                var url = "/POS/Reports/frmReportTPBillInfo.aspx?billID=" + $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() + "&kotId=" + kotId;
                                parent.document.getElementById(iframeid).src = url;

                                if ($("#ContentPlaceHolder1_hfBearerCanSettleBill").val() == "1") {
                                    $("#ContentPlaceHolder1_hfIsBillPreviewButtonEnable").val("1");
                                    $("#btnBillPreview").show();
                                }
                                else {
                                    $("#ContentPlaceHolder1_hfIsBillPreviewButtonEnable").val("0");
                                    $("#btnBillPreview").hide();
                                }

                                if (AddedClassificationList.length > 0) {
                                    AddedClassificationList = result.d.ObjectList;
                                }

                                if (AddedTableList.length > 0) {
                                    AddedTableList = result.d.ObjectList1;
                                }

                                DeletedClassificationList = new Array();

                                $("#displayBill").dialog({
                                    autoOpen: true,
                                    modal: true,
                                    width: 'auto',
                                    height: 'auto',
                                    minWidth: 550,
                                    minHeight: 580,
                                    closeOnEscape: false,
                                    resizable: false,
                                    fluid: true,
                                    title: "Invoice Preview",
                                    show: 'slide'
                                });

                                setTimeout(function () { ScrollToBottom(); LoadGridInformation(); }, 1000);
                            }
                        }
                        else {
                            CommonHelper.AlertMessage(result.d.AlertMessage);
                            IsBillOnPreview = "0";
                        }
                    },
                    error: function (xhr, err) {
                        IsBillOnPreview == "0";
                        toastr.error(xhr.responseText);
                    }
                });

            }
            else {

                $.ajax({
                    type: "POST",
                    url: "../../../Common/WebMethodPage.aspx/HoldUpRestauranBillGenerationNew",
                    data: JSON.stringify({
                        RestaurantBill: RestaurantBill, GuestBillPayment: GuestBillPayment, BillDetail: BillDetail,
                        AddedClassificationList: AddedClassificationList
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {

                        if (result.d.IsBillResettled == true) {
                            CommonHelper.AlertMessage(result.d.AlertMessage);
                            setTimeout(function () { window.location = result.d.RedirectUrl; }, 2000);
                            return false;
                        }

                        if (result.d.IsSuccess == true) {

                            $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                            $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val(result.d.Pk);
                            $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);
                            $("#ContentPlaceHolder1_hfIsResumeBill").val("1");

                            IsBillOnPreview = "0";

                            if (AddedClassificationList.length > 0) {
                                AddedClassificationList = result.d.ObjectList;
                            }

                            if (AddedTableList.length > 0) {
                                AddedTableList = result.d.ObjectList1;
                            }

                            if ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "") {
                                var iframeid = 'printDoc';
                                var url = "/POS/Reports/frmReportTPBillInfo.aspx?billID=" + $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() + "&kotId=" + kotId;
                                parent.document.getElementById(iframeid).src = url;

                                if ($("#ContentPlaceHolder1_hfBearerCanSettleBill").val() == "1") {
                                    $("#ContentPlaceHolder1_hfIsBillPreviewButtonEnable").val("1");
                                    $("#btnBillPreview").show();
                                }
                                else {
                                    $("#ContentPlaceHolder1_hfIsBillPreviewButtonEnable").val("0");
                                    $("#btnBillPreview").hide();
                                }

                                $("#displayBill").dialog({
                                    autoOpen: true,
                                    modal: true,
                                    width: 'auto',
                                    height: 'auto',
                                    minWidth: 550,
                                    minHeight: 580,
                                    closeOnEscape: false,
                                    resizable: false,
                                    fluid: true,
                                    title: "Invoice Preview",
                                    show: 'slide'
                                });

                                setTimeout(function () { ScrollToBottom(); LoadGridInformation(); }, 1000);
                            }
                        }
                        else {
                            CommonHelper.AlertMessage(result.d.AlertMessage);
                            IsBillOnPreview = "0";
                        }
                    },
                    error: function (xhr, err) {
                        IsBillOnPreview == "0";
                        toastr.error(xhr.responseText);
                    }
                });

            }

            return false;
        }



        function BillSplit() {

            if ($("#TableWiseItemInformation tbody tr").length == 0) {
                toastr.info("Please Add item Before Split");
                return;
            }

            if (IsBillOnSplit == "1") {
                toastr.info("Bill Already In Processed For Split.");
                return false;
            }

            var discountType = "Fixed", isComplementary = false, discountTransactionId = 0, isNonChargeable = false;

            IsBillOnSplit = "1";
            CommonHelper.SpinnerOpen();

            var costcenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            discountType = "Fixed"; isComplementary = false;

            if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
                discountType = "Fixed";
            }
            else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked")) {
                discountType = "Percentage";
            }
            else if ($("#ContentPlaceHolder1_rbTPComplementaryDiscount").is(":checked")) {
                discountType = "Percentage";
                isComplementary = true;
            }
            else if ($("#ContentPlaceHolder1_rbTpNonChargeable").is(":checked")) {
                discountType = "Percentage";
                isNonChargeable = true;
            }

            var customerName = "", remarks = "";
            var guestList = $("#ContentPlaceHolder1_hfGuestList").val();
            if ($.trim(guestList) == "" && $("#lblGuestRoom").text() != "Room Payment" && $("#ContentPlaceHolder1_hfRoomId").val() != "")
                guestList = $("#lblGuestRoom").text();

            if ($.trim(guestList) == "" && $("#lblGuestRoom").text() != "Room Payment" && $("#ContentPlaceHolder1_hfRoomId").val() != "") {
                guestList = $("#lblGuestRoom").text();
                customerName = guestList;
            } else if ($.trim(guestList) != "" && $("#lblGuestRoom").text() != "Room Payment" && $("#ContentPlaceHolder1_hfRoomId").val() != "") {
                customerName = guestList;
            }

            if ($.trim($("#ContentPlaceHolder1_hfGuestList").val()) == "") {
                customerName = "";
            }

            if ($.trim($("#ContentPlaceHolder1_txtRemarks").val()) != "") {
                remarks = $.trim($("#ContentPlaceHolder1_txtRemarks").val());
            }

            var calculatedDiscountAmount = "0";
            var discountAmount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTPDiscountAmount").val();

            var salesAmountTP = $("#ContentPlaceHolder1_txtTotalSales").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTotalSales").val();
            var discountedAmountTP = $("#ContentPlaceHolder1_txtTPDiscountedAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTPDiscountedAmount").val();

            if (parseFloat(discountedAmountTP) != 0)
                calculatedDiscountAmount = parseFloat(salesAmountTP) - parseFloat(discountedAmountTP);

            if (discountType == 'Percentage' && parseFloat(discountAmount) == 100) {
                calculatedDiscountAmount = parseFloat(salesAmountTP) - parseFloat(discountedAmountTP);
            }

            var serviceCharge = $("#ContentPlaceHolder1_txtServiceCharge").val() == "" ? "0" : $("#ContentPlaceHolder1_txtServiceCharge").val();
            var vatAmount = $("#ContentPlaceHolder1_txtVatAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtVatAmount").val();

            var citySDCharge = $("#ContentPlaceHolder1_txtCitySDCharge").val() == "" ? "0" : $("#ContentPlaceHolder1_txtCitySDCharge").val();
            var additionalCharge = $("#ContentPlaceHolder1_txtAdditionalCharge").val() == "" ? "0" : $("#ContentPlaceHolder1_txtAdditionalCharge").val();
            var additionalChargeType = $("#ContentPlaceHolder1_hfAdditionalChargeType").val();

            customerName = "";
            var paxQuantity = $("#ContentPlaceHolder1_hfPaxQuantity").val();
            var sourceInfoId = $("#ContentPlaceHolder1_hfKotId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfKotId").val();

            var sourceName = $("#ContentPlaceHolder1_hfsourceType").val();
            var bearerId = $("#ContentPlaceHolder1_hfBearerId").val();
            var sourceId = $("#ContentPlaceHolder1_hfSourceId").val();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var registrationId = $("#ContentPlaceHolder1_hfRegistrationId").val();
            var roomId = $("#ContentPlaceHolder1_hfRoomId").val();
            roomId = roomId == "" ? "0" : roomId;

            if (sourceInfoId == "")
                sourceInfoId = "0";

            if (sourceInfoId != "0") {
                tableId = sourceInfoId;
            }

            var billPaidBySourceId = tableId;
            var isInvoiceServiceChargeEnable = false, isInvoiceVatAmountEnable = false;
            var isInvoiceCitySDChargeEnable = false, isInvoiceAdditionalChargeEnable = false;

            if ($("#ContentPlaceHolder1_cbTPServiceCharge").is(":checked")) {
                isInvoiceServiceChargeEnable = true;
            }
            else {
                isInvoiceServiceChargeEnable = false;
            }

            if ($("#ContentPlaceHolder1_cbTPVatAmount").is(":checked")) {
                isInvoiceVatAmountEnable = true;
            }
            else {
                isInvoiceVatAmountEnable = false;
            }

            if ($("#ContentPlaceHolder1_cbTPSDCharge").is(":checked")) {
                isInvoiceCitySDChargeEnable = true;
            }
            else {
                isInvoiceCitySDChargeEnable = false;
            }

            if ($("#ContentPlaceHolder1_cbTPAdditionalCharge").is(":checked")) {
                isInvoiceAdditionalChargeEnable = true;
            }
            else {
                isInvoiceAdditionalChargeEnable = false;
            }

            var billId = "0";

            if ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "") {
                billId = $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val();
            }

            var BillDetail = new Array();

            if (AddedTableList.length > 0) {

                BillDetail = JSON.parse(JSON.stringify(AddedTableList));

                BillDetail.push({
                    TableId: sourceId,
                    KotId: kotId,
                    DetailId: 0,
                    BillId: billId,
                    MainBillId: billId
                });
            }
            else {
                BillDetail.push({
                    TableId: sourceId,
                    KotId: kotId,
                    DetailId: 0,
                    BillId: billId,
                    MainBillId: billId
                });
            }

            var isRestaurantBillInclusive = $("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val() == "" ? "0" : $("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val();
            var salesAmount = "0", grandTotal = "0", roundedAmount = "0";

            roundedAmount = $("#ContentPlaceHolder1_txtTPRoundedAmount").val();
            grandTotal = $("#ContentPlaceHolder1_txtGrandTotal").val();
            salesAmount = $("#ContentPlaceHolder1_txtSalesAmount").val();

            //if (isRestaurantBillInclusive == "0") {
            //    salesAmount = $("#ContentPlaceHolder1_txtSalesAmount").val();
            //    grandTotal = $("#ContentPlaceHolder1_txtGrandTotal").val();
            //}
            //else {
            //    //salesAmount = $("#ContentPlaceHolder1_txtGrandTotal").val() == "" ? "0" : $("#ContentPlaceHolder1_txtGrandTotal").val();
            //    //grandTotal = $("#ContentPlaceHolder1_txtSalesAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtSalesAmount").val();
            //    //grandTotal = parseFloat(salesAmount) - parseFloat(calculatedDiscountAmount);

            //    salesAmount = $("#ContentPlaceHolder1_txtSalesAmount").val();
            //    grandTotal = $("#ContentPlaceHolder1_txtGrandTotal").val();
            //}

            var RestaurantBill = {
                CostCenterId: costcenterId,
                DiscountType: $.trim(discountType),
                IsComplementary: isComplementary,
                IsNonChargeable: isNonChargeable,
                DiscountTransactionId: discountTransactionId,
                DiscountAmount: discountAmount,
                CalculatedDiscountAmount: calculatedDiscountAmount,

                ServiceCharge: serviceCharge,
                VatAmount: vatAmount,

                CitySDCharge: citySDCharge,
                AdditionalCharge: additionalCharge,
                AdditionalChargeType: additionalChargeType,

                CustomerName: customerName,
                PaxQuantity: paxQuantity,
                TableId: sourceId,
                SourceName: sourceName,
                TransactionType: '',
                TransactionId: '',
                KotId: kotId,
                RegistrationId: registrationId,
                RoomId: roomId,
                BearerId: bearerId,
                BillPaidBySourceId: sourceId,

                IsInvoiceServiceChargeEnable: isInvoiceServiceChargeEnable,
                IsInvoiceVatAmountEnable: isInvoiceVatAmountEnable,
                IsInvoiceCitySDChargeEnable: isInvoiceCitySDChargeEnable,
                IsInvoiceAdditionalChargeEnable: isInvoiceAdditionalChargeEnable,

                InvoiceServiceRate: KotWiseServiceChargeVatNOther.RackRate,
                SalesAmount: salesAmount,
                GrandTotal: grandTotal,
                RoundedAmount: roundedAmount,
                RoundedGrandTotal: grandTotal,
                Remarks: remarks
            };

            var GuestBillPayment = new Array();

            var txtTPDiscountedAmountHiddenFieldVal = $("#ContentPlaceHolder1_txtTPDiscountedAmount").val();

            var txtCashVal = $("#ContentPlaceHolder1_txtCash").val();
            var txtAmexCardVal = $("#ContentPlaceHolder1_txtAmexCard").val();
            var txtMasterCardVal = $("#ContentPlaceHolder1_txtMasterCard").val();
            var txtVisaCardVal = $("#ContentPlaceHolder1_txtVisaCard").val();
            var txtDiscoverCardVal = $("#ContentPlaceHolder1_txtDiscoverCard").val();
            var txtTPDiscountAmountVal = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();
            var guestRoomPayment = $("#ContentPlaceHolder1_txtRoomPayment").val();

            var companyAmount = $("#ContentPlaceHolder1_txtCompanyPayment").val();
            var companyName = $("#lblCompanyName").text();
            var companyId = $("#ContentPlaceHolder1_hfCompanyId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfCompanyId").val();

            var employeePayment = $("#ContentPlaceHolder1_txtEmployeePayment").val();
            var employeeName = $("#lblEmployeeName").text();
            var employeeId = $("#ContentPlaceHolder1_hfEmployeeId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfEmployeeId").val();

            var memberPayment = $("#ContentPlaceHolder1_txtMemberPayment").val();
            var memberName = $("#txtMemberName").val();
            var memberId = $("#ContentPlaceHolder1_hfMemberId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfMemberId").val();

            if (employeeId != "0") {
                RestaurantBill.TransactionType = 'Employee';
                RestaurantBill.TransactionId = employeeId;
                RestaurantBill.CustomerName = employeeName;
            }
            else if (memberId != "0") {
                RestaurantBill.TransactionType = 'Member';
                RestaurantBill.TransactionId = memberId;
                RestaurantBill.CustomerName = memberName;
            }
            else if (companyId != "0") {
                RestaurantBill.TransactionType = 'Company';
                RestaurantBill.TransactionId = companyId;
                RestaurantBill.CustomerName = companyName;
            }
            else {
                RestaurantBill.TransactionType = null;
                RestaurantBill.TransactionId = null;
            }

            var hfLocalCurrencyId = 'ContentPlaceHolder1_hfLocalCurrencyId'
            var hfLocalCurrencyIdVal = $('#' + hfLocalCurrencyId).val();

            var DiscountTypeVal = 'Fixed'

            if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(':checked')) {
                DiscountTypeVal = "Fixed";
            }
            else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(':checked')) {
                DiscountTypeVal = "Percentage";
            }

            var PaymentMode = new Array();
            PaymentMode.push({ CardType: '', PaymentType: 'Advance', PaymentDescription: '', PaymentMode: 'Cash', PaymentById: '', Control: "ContentPlaceHolder1_txtCash" });
            PaymentMode.push({ CardType: 'a', PaymentType: 'Advance', PaymentDescription: 'American Express', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtAmexCard" });
            PaymentMode.push({ CardType: 'm', PaymentType: 'Advance', PaymentDescription: 'Master Card', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtMasterCard" });
            PaymentMode.push({ CardType: 'v', PaymentType: 'Advance', PaymentDescription: 'Visa Card', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtVisaCard" });
            PaymentMode.push({ CardType: 'd', PaymentType: 'Advance', PaymentDescription: 'Discover Card', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtDiscoverCard" });
            PaymentMode.push({ CardType: '', PaymentType: 'Company', PaymentDescription: companyName, PaymentMode: 'Company', PaymentById: $("#ContentPlaceHolder1_hfCompanyId").val(), Control: "ContentPlaceHolder1_txtCompanyPayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'GuestRoom', PaymentDescription: 'GuestRoom', PaymentMode: 'Other Room', PaymentById: $("#ContentPlaceHolder1_hfRoomId").val(), Control: "ContentPlaceHolder1_txtRoomPayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'Employee', PaymentDescription: employeeName, PaymentMode: 'Employee', PaymentById: $("#ContentPlaceHolder1_hfEmployeeId").val(), Control: "ContentPlaceHolder1_txtEmployeePayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'Member', PaymentDescription: memberName, PaymentMode: 'Member', PaymentById: $("#ContentPlaceHolder1_hfMemberId").val(), Control: "ContentPlaceHolder1_txtMemberPayment" });

            var paymodeCount = PaymentMode.length, row = 0;
            var paymentAmount = "0";
            var pDescription = "";
            var paymentCounter = 1;
            var paymentById = 0;

            for (row = 0; row < paymodeCount; row++) {

                paymentById = $.trim(PaymentMode[row].PaymentById) == "" ? "0" : PaymentMode[row].PaymentById;

                if ($("#" + PaymentMode[row].Control).val() != "" && $("#" + PaymentMode[row].Control).val() != "0") {
                    paymentAmount = $("#" + PaymentMode[row].Control).val();

                    if (paymentAmount == "" || paymentAmount == "0")
                        continue;

                    GuestBillPayment.push({
                        NodeId: 0,
                        PaymentType: PaymentMode[row].PaymentType,
                        AccountsPostingHeadId: 0,
                        BillPaidBy: 0,
                        BankId: 0,
                        RegistrationId: paymentById,
                        FieldId: hfLocalCurrencyIdVal,
                        ConvertionRate: 1,
                        CurrencyAmount: paymentAmount,
                        PaymentAmount: paymentAmount,
                        ChecqueDate: new Date(),
                        PaymentMode: PaymentMode[row].PaymentMode,
                        PaymentId: 1,
                        CardNumber: "",
                        CardType: PaymentMode[row].CardType,
                        ExpireDate: null,
                        ChecqueNumber: "",
                        CardHolderName: "",
                        PaymentDescription: PaymentMode[row].PaymentDescription,
                        CompanyId: companyId
                    });
                }
                else if ((isComplementary == true || isNonChargeable == true) && paymentCounter == 1) {
                    paymentAmount = $("#" + PaymentMode[row].Control).val();

                    if ($.trim(paymentAmount) != "") {
                        paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;

                        GuestBillPayment.push({
                            NodeId: 0,
                            PaymentType: PaymentMode[row].PaymentType,
                            AccountsPostingHeadId: 0,
                            BillPaidBy: 0,
                            BankId: 0,
                            RegistrationId: paymentById,
                            FieldId: hfLocalCurrencyIdVal,
                            ConvertionRate: 1,
                            CurrencyAmount: paymentAmount,
                            PaymentAmount: paymentAmount,
                            ChecqueDate: new Date(),
                            PaymentMode: PaymentMode[row].PaymentMode,
                            PaymentId: 1,
                            CardNumber: "",
                            CardType: PaymentMode[row].CardType,
                            ExpireDate: null,
                            ChecqueNumber: "",
                            CardHolderName: "",
                            PaymentDescription: PaymentMode[row].PaymentDescription,
                            CompanyId: companyId
                        });

                        paymentCounter = paymentCounter + 1;
                    }
                }
            }

            if ($("#ContentPlaceHolder1_txtTPRoundedAmount").val() != "0" && $("#ContentPlaceHolder1_txtTPRoundedAmount").val() != "") {

                roundedAmount = $("#ContentPlaceHolder1_txtTPRoundedAmount").val();

                if (roundedAmount != "" || roundedAmount != "0") {

                    GuestBillPayment.push({
                        NodeId: 0,
                        PaymentType: "Rounded",
                        AccountsPostingHeadId: 0,
                        BillPaidBy: 0,
                        BankId: 0,
                        RegistrationId: 0,
                        FieldId: hfLocalCurrencyIdVal,
                        ConvertionRate: 1,
                        CurrencyAmount: roundedAmount,
                        PaymentAmount: roundedAmount,
                        ChecqueDate: new Date(),
                        PaymentMode: "Rounded",
                        PaymentId: 1,
                        CardNumber: "",
                        CardType: "",
                        ExpireDate: null,
                        ChecqueNumber: "",
                        CardHolderName: "",
                        PaymentDescription: "",
                        CompanyId: companyId
                    });
                }
            }

            if ($("#ContentPlaceHolder1_lblTPChangeAmount").text() == "Change Amount" && $("#ContentPlaceHolder1_lblTotalChangeAmount").text() == "Change Amount") {
                if ($("#ContentPlaceHolder1_txtTPChangeAmount").val() != "0" && $("#ContentPlaceHolder1_txtTPChangeAmount").val() != "") {

                    var changeAmount = $("#ContentPlaceHolder1_txtTPChangeAmount").val();

                    if (changeAmount != "" || changeAmount != "0") {

                        GuestBillPayment.push({
                            NodeId: 0,
                            PaymentType: "Refund",
                            AccountsPostingHeadId: 0,
                            BillPaidBy: 0,
                            BankId: 0,
                            RegistrationId: 0,
                            FieldId: hfLocalCurrencyIdVal,
                            ConvertionRate: 1,
                            CurrencyAmount: changeAmount,
                            PaymentAmount: changeAmount,
                            ChecqueDate: new Date(),
                            PaymentMode: "Refund",
                            PaymentId: 1,
                            CardNumber: "",
                            CardType: "",
                            ExpireDate: null,
                            ChecqueNumber: "",
                            CardHolderName: "",
                            PaymentDescription: "",
                            CompanyId: companyId
                        });
                    }
                }
            }

            if ($("#ContentPlaceHolder1_hfIsResumeBill").val() == "1" && $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "") {

                var BillDetailsNew = new Array();
                BillDetailsNew = BillDetail; //_.findWhere(BillDetail, { DetailId: 0 });

                $.ajax({
                    type: "POST",
                    url: "../../../Common/WebMethodPage.aspx/UpdateRestauranBillGenerationSplitNew",
                    data: JSON.stringify({
                        RestaurantBill: RestaurantBill, GuestBillPayment: GuestBillPayment,
                        BillDetail: BillDetailsNew, BillDeletedDetail: DeletedTableList,
                        AddedClassificationList: AddedClassificationList,
                        DeletedClassificationList: DeletedClassificationList
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {

                        if (result.d.IsBillResettled == true) {
                            CommonHelper.AlertMessage(result.d.AlertMessage);
                            setTimeout(function () { window.location = result.d.RedirectUrl; }, 2000);
                            return false;
                        }

                        $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                        $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val(result.d.Pk);
                        IsBillOnSplit = "0";

                        if (result.d.ObjectList != null) {
                            if (AddedClassificationList.length > 0) {
                                AddedClassificationList = result.d.ObjectList;
                            }
                        }

                        if (AddedTableList != null) {
                            if (AddedTableList.length > 0)
                                AddedTableList = result.d.ObjectList1;
                        }

                        $("#ContentPlaceHolder1_btnClear").attr("disabled", true);

                        //call split

                        var dataLength = 0, row = 0;

                        $("#ContentPlaceHolder1_lstvBillSplitLeft option").remove();
                        $("#ContentPlaceHolder1_lstvBillSplitRight option").remove();

                        if (result.d.Arr.length > 0) {

                            dataLength = result.d.Arr.length;

                            for (row = 0; row < dataLength; row++) {
                                $("#ContentPlaceHolder1_lstvBillSplitLeft").append("<option value='" + result.d.Arr[row].ItemId + "'>" + result.d.Arr[row].ItemName + "</option>");
                            }

                            $("#BillSplitDialog").dialog({
                                width: 750,
                                height: 480,
                                autoOpen: true,
                                //modal: true,
                                closeOnEscape: true,
                                resizable: false,
                                fluid: true,
                                title: "Split Bill",
                                show: 'slide'
                            });
                        }

                        $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);
                        //$('#btnPrintPreview').trigger('click');
                        $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                        setTimeout(function () { LoadGridInformation(); }, 1000);

                    },
                    error: function (xhr, err) {
                        toastr.error(xhr.responseText);
                    }
                });

            }
            else {

                $.ajax({
                    type: "POST",
                    url: "../../../Common/WebMethodPage.aspx/HoldUpRestauranBillGenerationSplitNew",
                    data: JSON.stringify({
                        RestaurantBill: RestaurantBill, GuestBillPayment: GuestBillPayment, BillDetail: BillDetail,
                        AddedClassificationList: AddedClassificationList
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {

                        if (result.d.IsBillResettled == true) {
                            CommonHelper.AlertMessage(result.d.AlertMessage);
                            setTimeout(function () { window.location = result.d.RedirectUrl; }, 2000);
                            return false;
                        }

                        $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                        $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val(result.d.Pk);
                        $("#ContentPlaceHolder1_hfIsResumeBill").val("1");

                        $("#ContentPlaceHolder1_btnClear").attr("disabled", true);

                        IsBillOnSplit = "0";

                        if (AddedClassificationList.length > 0) {
                            AddedClassificationList = result.d.ObjectList;
                        }

                        if (AddedTableList.length > 0) {
                            AddedTableList = result.d.ObjectList1;
                        }

                        //call split

                        var dataLength = 0, row = 0;

                        if (result.d.Arr.length > 0) {

                            dataLength = result.d.Arr.length;

                            for (row = 0; row < dataLength; row++) {
                                $("#ContentPlaceHolder1_lstvBillSplitLeft").append("<option value='" + result.d.Arr[row].ItemId + "'>" + result.d.Arr[row].ItemName + "</option>");
                            }

                            $("#BillSplitDialog").dialog({
                                width: 750,
                                height: 480,
                                autoOpen: true,
                                //modal: true,
                                closeOnEscape: true,
                                resizable: false,
                                fluid: true,
                                title: "Split Bill",
                                show: 'slide'
                            });
                        }
                        //call split  BillSplitDialog

                        setTimeout(function () { LoadGridInformation(); }, 1000);
                        $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);
                        $("#ContentPlaceHolder1_btnSave").attr("disabled", true);

                    },
                    error: function (xhr, err) {
                        toastr.error(xhr.responseText);
                    }
                });

            }

            return false;
        }

        function AddMoreTable() {

            var costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var sourceId = $("#ContentPlaceHolder1_hfSourceId").val();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var sourceName = $("#ContentPlaceHolder1_hfsourceType").val();

            PageMethods.LoadAlreadyOccupiedTable(costCenterId, sourceId, kotId, sourceName, OnOccupiedTableLoadSucceeded, OccupiedTableLoadFailed);
            return false;
        }
        function OnOccupiedTableLoadSucceeded(result) {

            if (result.IsBillResettled == true) {
                CommonHelper.AlertMessage(result.AlertMessage);
                setTimeout(function () { window.location = result.RedirectUrl; }, 2000);
                return false;
            }

            $("#OccupiedTableContainer").html(result.DataStr);

            $("#OccupiedTableDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 350,
                height: 450,
                closeOnEscape: false,
                resizable: false,
                title: "Occupied Table",
                show: 'slide',
                open: function (event, ui) {
                    $('#OccupiedTableDialog').css('overflow-x', 'hidden');
                }
            });

            var tableId = 0;
            if (AddedTableList.length > 0) {

                $('#OccupiedTableInformation tbody tr').each(function () {

                    var id = _.findWhere(AddedTableList, { TableId: parseInt($.trim($(this).find("td:eq(3)").text()), 10) });

                    if (id != null) {
                        $(this).find("td:eq(0)").find("input").attr('checked', true);
                    }
                });
            }

        }
        function OccupiedTableLoadFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }

        function PaxChange() {

            var sourceType = $.trim(CommonHelper.GetParameterByName("st"));
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var paxQuantity = $("#ContentPlaceHolder1_hfPaxQuantity").val();

            $("#ContentPlaceHolder1_txtPax").val(paxQuantity);

            //if (sourceType == "tkn") {
            //    $("#TActionDeciderForToken").dialog("close");
            //}
            //else if (sourceType == "tbl") {
            //    $("#TActionDeciderForTable").dialog("close");
            //}

            CommonHelper.TouchScreenNumberKeyboardWithoutDot("numkbnotdecimalpax", "KeyBoardContainerPaxChange");
            var keyboard = $('.numkbnotdecimalpax').getkeyboard();
            keyboard.reveal();

            $("#ContentPlaceHolder1_txtPax_keyboard").css("top", "0px");

            $("#PaxChangeTouchKeypad").dialog({
                width: 440,
                height: 400,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Pax Change",
                show: 'slide'
            });
        }

        function PaxChangeApply() {

            var paxQuantity = 0, costCenterId = 0, kotId = 0;
            costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            kotId = $("#ContentPlaceHolder1_hfKotId").val();

            paxQuantity = $("#ContentPlaceHolder1_txtPax").val();

            if (CommonHelper.IsInt(paxQuantity) == false) {
                toastr.info("Please Give Valid Number.");
                return;
            }

            CommonHelper.SpinnerOpen();

            if ($.trim(paxQuantity) == "") {
                toastr.warning("Pax Cannot Blank.");
                CommonHelper.SpinnerClose();
                return false;
            }
            else if (parseInt(paxQuantity) == 0) {
                toastr.warning("Pax Cannot Zero (0).");
                CommonHelper.SpinnerClose();
                return false;
            }

            $("#ContentPlaceHolder1_hfPaxQuantity").val(paxQuantity);
            $("#PaxChangeTouchKeypad").dialog("close");

            var keyboardnum = $('.numkbnotdecimalpax').getkeyboard();
            keyboardnum.destroy();

            PageMethods.UpdateKotPaxInformation(costCenterId, kotId, paxQuantity, OnUpdateObjectSucceededForPax, OnUpdateObjectFailedForPax);
            return false;
        }

        function OnUpdateObjectSucceededForPax(result) {

            if (result.AlertMessage.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#ContentPlaceHolder1_hfPaxQuantity").val($("#ContentPlaceHolder1_txtPax").val());
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();

            return false;
        }
        function OnUpdateObjectFailedForPax(xhr, err) {
            CommonHelper.SpinnerClose();
            toastr.error(xhr.responseText);
        }

        function GetAndApplySelectedTable() {

            AddedTableList = new Array();
            DeletedTableList = new Array();
            var addedTable = "", billId = "0";

            if ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "0" && $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "") {
                billId = $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val();
            }

            $('#OccupiedTableInformation tbody tr').each(function () {

                if ($(this).find("td:eq(0)").find("input").is(':checked')) {

                    AddedTableList.push({
                        TableId: parseInt($.trim($(this).find("td:eq(3)").text()), 10),
                        KotId: parseInt($.trim($(this).find("td:eq(4)").text()), 10),
                        DetailId: parseInt($.trim($(this).find("td:eq(5)").text()), 10),
                        BillId: parseInt($.trim($(this).find("td:eq(6)").text()), 10),
                        MainBillId: billId
                    });

                    if (addedTable != "") {
                        addedTable += "," + $.trim($(this).find("td:eq(1)").text());
                    }
                    else { addedTable = $.trim($(this).find("td:eq(1)").text()) }
                }
                else {
                    if ($.trim($(this).find("td:eq(5)").text()) != "0" && $(this).find("td:eq(0)").find("input").is(':input')) {
                        DeletedTableList.push({
                            TableId: parseInt($.trim($(this).find("td:eq(3)").text()), 10),
                            KotId: parseInt($.trim($(this).find("td:eq(4)").text()), 10),
                            DetailId: parseInt($.trim($(this).find("td:eq(5)").text()), 10),
                            MainBillId: billId,
                            BillId: parseInt($.trim($(this).find("td:eq(6)").text()), 10)
                        });
                    }
                }
            });

            if (addedTable != "") {
                $("#ContentPlaceHolder2_lblAddedOrderSourceNumber").text(" (" + addedTable + ")");
            }
            else { $("#ContentPlaceHolder2_lblAddedOrderSourceNumber").text(""); }

            $("#OccupiedTableDialog").dialog("close");
            LoadGridInformation();
        }

        function AddMoreTableDialogClose() {
            $("#OccupiedTableDialog").dialog("close");
        }

        function ValidateFormBeforeSettlement(settlementType) {
            // debugger;
            if (IsBillOnProcess == "1") {
                toastr.info("Bill Already In Processed.");
                return false;
            }

            IsBillOnProcess = "1";
            CommonHelper.SpinnerOpen();

            var costcenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var discountType = "Fixed", isComplementary = false, discountTransactionId = 0, isNonChargeable = false;

            if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
                discountType = "Fixed";
            }
            else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked")) {
                discountType = "Percentage";
            }
            else if ($("#ContentPlaceHolder1_rbTPComplementaryDiscount").is(":checked")) {
                discountType = "Percentage";
                isComplementary = true;
            }
            else if ($("#ContentPlaceHolder1_rbTpNonChargeable").is(":checked")) {
                discountType = "Percentage";
                isNonChargeable = true;
            }

            if ($("#ContentPlaceHolder1_hfPromotionalDiscountType").val() != "" && $("#ContentPlaceHolder1_rbTPComplementaryDiscount").is(":checked") == false) {
                discountType = $.trim($("#ContentPlaceHolder1_hfPromotionalDiscountType").val());
                discountTransactionId = AddedPromotionalDiscountList[0].BusinessPromotionId;
            }

            var customerName = "", remarks = "";
            var guestList = $("#ContentPlaceHolder1_hfGuestList").val();

            if ($.trim(guestList) == "" && $("#lblGuestRoom").text() != "Room Payment" && $("#ContentPlaceHolder1_hfRoomId").val() != "") {
                guestList = $("#lblGuestRoom").text();
                customerName = guestList;
            } else if ($.trim(guestList) != "" && $("#lblGuestRoom").text() != "Room Payment" && $("#ContentPlaceHolder1_hfRoomId").val() != "") {
                customerName = guestList;
            }

            if ($.trim($("#ContentPlaceHolder1_hfGuestList").val()) == "") {
                customerName = "";
            }

            if ($.trim($("#ContentPlaceHolder1_txtRemarks").val()) != "") {
                remarks = $.trim($("#ContentPlaceHolder1_txtRemarks").val());
            }

            var calculatedDiscountAmount = "0";
            var discountAmount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTPDiscountAmount").val();

            var salesAmountTP = $("#ContentPlaceHolder1_txtTotalSales").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTotalSales").val();
            var discountedAmountTP = $("#ContentPlaceHolder1_txtTPDiscountedAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTPDiscountedAmount").val();

            if (parseFloat(discountedAmountTP) != 0)
                calculatedDiscountAmount = parseFloat(salesAmountTP) - parseFloat(discountedAmountTP);

            if (discountType == 'Percentage' && parseFloat(discountAmount) == 100) {
                calculatedDiscountAmount = parseFloat(salesAmountTP) - parseFloat(discountedAmountTP);
                isComplementary = true;
            }

            if (discountType == 'Fixed') {
                calculatedDiscountAmount = parseFloat($("#ContentPlaceHolder1_txtTPDiscount").val());
            }

            var serviceCharge = $("#ContentPlaceHolder1_txtServiceCharge").val() == "" ? "0" : $("#ContentPlaceHolder1_txtServiceCharge").val();
            var vatAmount = $("#ContentPlaceHolder1_txtVatAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtVatAmount").val();

            var citySDCharge = $("#ContentPlaceHolder1_txtCitySDCharge").val() == "" ? "0" : $("#ContentPlaceHolder1_txtCitySDCharge").val();
            var additionalCharge = $("#ContentPlaceHolder1_txtAdditionalCharge").val() == "" ? "0" : $("#ContentPlaceHolder1_txtAdditionalCharge").val();
            var additionalChargeType = $("#ContentPlaceHolder1_hfAdditionalChargeType").val();

            var paxQuantity = $("#ContentPlaceHolder1_hfPaxQuantity").val();
            var sourceInfoId = $("#ContentPlaceHolder1_hfKotId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfKotId").val();

            var sourceName = $("#ContentPlaceHolder1_hfsourceType").val();
            var bearerId = $("#ContentPlaceHolder1_hfBearerId").val();
            var sourceId = $("#ContentPlaceHolder1_hfSourceId").val();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var registrationId = $("#ContentPlaceHolder1_hfRegistrationId").val();
            var roomId = $("#ContentPlaceHolder1_hfRoomId").val();
            roomId = roomId == "" ? "0" : roomId;

            if (sourceInfoId == "")
                sourceInfoId = "0";

            if (sourceInfoId != "0") {
                tableId = sourceInfoId;
            }

            var billPaidBySourceId = tableId;
            var isInvoiceServiceChargeEnable = false, isInvoiceVatAmountEnable = false;
            var isInvoiceCitySDChargeEnable = false, isInvoiceAdditionalChargeEnable = false;

            if ($("#ContentPlaceHolder1_cbTPServiceCharge").is(":checked")) {
                isInvoiceServiceChargeEnable = true;
            }
            else {
                isInvoiceServiceChargeEnable = false;
            }

            if ($("#ContentPlaceHolder1_cbTPVatAmount").is(":checked")) {
                isInvoiceVatAmountEnable = true;
            }
            else {
                isInvoiceVatAmountEnable = false;
            }

            if ($("#ContentPlaceHolder1_cbTPSDCharge").is(":checked")) {
                isInvoiceCitySDChargeEnable = true;
            }
            else {
                isInvoiceCitySDChargeEnable = false;
            }

            if ($("#ContentPlaceHolder1_cbTPAdditionalCharge").is(":checked")) {
                isInvoiceAdditionalChargeEnable = true;
            }
            else {
                isInvoiceAdditionalChargeEnable = false;
            }

            var billId = "0";

            if ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "") {
                billId = $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val();
            }

            var detailId = "0";

            if ($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "" && $("#ContentPlaceHolder1_hfBillIdDetailsId").val() != "") {
                detailId = $("#ContentPlaceHolder1_hfBillIdDetailsId").val();
            }

            var BillDetail = new Array();

            if (AddedTableList.length > 0) {

                BillDetail = JSON.parse(JSON.stringify(AddedTableList));

                BillDetail.push({
                    TableId: sourceId,
                    KotId: kotId,
                    DetailId: detailId,
                    BillId: billId,
                    MainBillId: billId
                });
            }
            else {
                BillDetail.push({
                    TableId: sourceId,
                    KotId: kotId,
                    DetailId: detailId,
                    MainBillId: billId,
                    BillId: billId
                });
            }

            var isRestaurantBillInclusive = $("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val() == "" ? "0" : $("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val();
            var salesAmount = "0", grandTotal = "0", roundedAmount = "0";

            roundedAmount = $("#ContentPlaceHolder1_txtTPRoundedAmount").val();
            grandTotal = $("#ContentPlaceHolder1_txtGrandTotal").val();
            salesAmount = $("#ContentPlaceHolder1_txtSalesAmount").val();

            //if (isRestaurantBillInclusive == "0") {
            //    salesAmount = $("#ContentPlaceHolder1_txtSalesAmount").val();
            //    grandTotal = $("#ContentPlaceHolder1_txtGrandTotal").val();
            //}
            //else {
            //    salesAmount = $("#ContentPlaceHolder1_txtGrandTotal").val() == "" ? "0" : $("#ContentPlaceHolder1_txtGrandTotal").val();
            //    grandTotal = $("#ContentPlaceHolder1_txtSalesAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtSalesAmount").val();

            //    grandTotal = parseFloat(salesAmount) - parseFloat(calculatedDiscountAmount);
            //}

            var totalQuantity = 0, totalPrice = 0;

            $("#TableWiseItemInformation tbody tr").each(function () {
                totalQuantity = totalQuantity + parseFloat($(this).find("td:eq(1)").text());
                totalPrice = totalPrice + parseFloat($(this).find("td:eq(3)").text());
            });

            totalPrice = toFixed(totalPrice, 2);

            var RestaurantBill = {
                CostCenterId: costcenterId,
                DiscountType: $.trim(discountType),
                IsComplementary: isComplementary,
                IsNonChargeable: isNonChargeable,
                DiscountTransactionId: discountTransactionId,
                DiscountAmount: discountAmount,
                CalculatedDiscountAmount: calculatedDiscountAmount,

                ServiceCharge: serviceCharge,
                VatAmount: vatAmount,
                CitySDCharge: citySDCharge,
                AdditionalCharge: additionalCharge,
                AdditionalChargeType: additionalChargeType,

                CustomerName: customerName,
                PaxQuantity: paxQuantity,
                TableId: sourceId,
                SourceName: sourceName,
                TransactionType: '',
                TransactionId: '',
                KotId: kotId,
                RegistrationId: registrationId,
                RoomId: roomId,
                BearerId: bearerId,
                BillPaidBySourceId: sourceId,

                IsInvoiceServiceChargeEnable: isInvoiceServiceChargeEnable,
                IsInvoiceVatAmountEnable: isInvoiceVatAmountEnable,
                IsInvoiceCitySDChargeEnable: isInvoiceCitySDChargeEnable,
                IsInvoiceAdditionalChargeEnable: isInvoiceAdditionalChargeEnable,

                SalesAmount: salesAmount,
                InvoiceServiceRate: KotWiseServiceChargeVatNOther.RackRate,
                GrandTotal: grandTotal,
                RoundedAmount: roundedAmount,
                RoundedGrandTotal: grandTotal,
                Remarks: remarks,

                TotalQuantity: totalQuantity,
                TotalPrice: totalPrice
            };

            var GuestBillPayment = new Array();

            var txtTPDiscountedAmountHiddenFieldVal = $("#ContentPlaceHolder1_txtTPDiscountedAmount").val();

            var txtCashVal = $("#ContentPlaceHolder1_txtCash").val();
            
            var txtAmexCardVal = $("#ContentPlaceHolder1_txtAmexCard").val();
            var amexCardBankName = $("#lblAmexCardBankName").text();
            var amexCardBankId = $("#ContentPlaceHolder1_hfAmexCardId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfAmexCardId").val();

            var txtMasterCardVal = $("#ContentPlaceHolder1_txtMasterCard").val();
            var masterCardBankName = $("#lblMasterCardBankName").text();
            var masterCardBankId = $("#ContentPlaceHolder1_hfMasterCardId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfMasterCardId").val();

            var txtVisaCardVal = $("#ContentPlaceHolder1_txtVisaCard").val();
            var visaCardBankName = $("#lblVisaCardBankName").text();
            var visaCardBankId = $("#ContentPlaceHolder1_hfVisaCardId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfVisaCardId").val();

            var txtDiscoverCardVal = $("#ContentPlaceHolder1_txtDiscoverCard").val();
            var discoverCardBankName = $("#lblDiscoverCardBankName").text();
            var discoverCardBankId = $("#ContentPlaceHolder1_hfDiscoverCardId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfDiscoverCardId").val();

            var txtTPDiscountAmountVal = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();
            var guestRoomPayment = $("#ContentPlaceHolder1_txtRoomPayment").val();
                        
            var mBankAmount = $("#ContentPlaceHolder1_txtMBankingPayment").val();
            var mBankName = $("#lblMBankName").text();
            var mBankId = $("#ContentPlaceHolder1_hfMBankId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfMBankId").val();
            
            var companyAmount = $("#ContentPlaceHolder1_txtCompanyPayment").val();
            var companyName = $("#lblCompanyName").text();
            var companyId = $("#ContentPlaceHolder1_hfCompanyId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfCompanyId").val();

            var employeePayment = $("#ContentPlaceHolder1_txtEmployeePayment").val();
            var employeeName = $("#lblEmployeeName").text();
            var employeeId = $("#ContentPlaceHolder1_hfEmployeeId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfEmployeeId").val();

            var memberPayment = $("#ContentPlaceHolder1_txtMemberPayment").val();
            var memberName = $("#lblMemberName").text();
            var memberId = $("#ContentPlaceHolder1_hfMemberId").val() == "" ? "0" : $("#ContentPlaceHolder1_hfMemberId").val();

            if (amexCardBankId != "0") {
                RestaurantBill.TransactionType = 'Card';
                RestaurantBill.TransactionId = amexCardBankId;
                RestaurantBill.CustomerName = 'Amex Card ('+ amexCardBankName + ')';
                RestaurantBill.CustomerName = '';
            }
            else if (masterCardBankId != "0") {
                RestaurantBill.TransactionType = 'Card';
                RestaurantBill.TransactionId = masterCardBankId;
                RestaurantBill.CustomerName = 'Master Card ('+ masterCardBankName + ')';
                RestaurantBill.CustomerName = '';
            }
            else if (visaCardBankId != "0") {
                RestaurantBill.TransactionType = 'Card';
                RestaurantBill.TransactionId = visaCardBankId;
                RestaurantBill.CustomerName = 'Visa Card ('+ visaCardBankName + ')';
                RestaurantBill.CustomerName = '';
            }
            else if (discoverCardBankId != "0") {
                RestaurantBill.TransactionType = 'Card';
                RestaurantBill.TransactionId = discoverCardBankId;
                RestaurantBill.CustomerName = 'Discover Card ('+ discoverCardBankName + ')';
                RestaurantBill.CustomerName = '';
            }
            else if (employeeId != "0") {
                RestaurantBill.TransactionType = 'Employee';
                RestaurantBill.TransactionId = employeeId;
                RestaurantBill.CustomerName = employeeName;
            }
            else if (memberId != "0") {
                RestaurantBill.TransactionType = 'Member';
                RestaurantBill.TransactionId = memberId;
                RestaurantBill.CustomerName = memberName;
            }
            else if (companyId != "0") {
                RestaurantBill.TransactionType = 'Company';
                RestaurantBill.TransactionId = companyId;
                RestaurantBill.CustomerName = companyName;
            }
            else if (mBankId != "0") {
                RestaurantBill.TransactionType = 'M-Banking';
                RestaurantBill.TransactionId = mBankId;
                RestaurantBill.CustomerName = 'M-Banking ('+ mBankName + ')';
                //RestaurantBill.CustomerName = '';
            }
            else {
                RestaurantBill.TransactionType = null;
                RestaurantBill.TransactionId = null;
            }

            var hfLocalCurrencyId = 'ContentPlaceHolder1_hfLocalCurrencyId';
            var hfLocalCurrencyIdVal = $('#' + hfLocalCurrencyId).val();

            var DiscountTypeVal = 'Fixed';

            if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(':checked')) {
                DiscountTypeVal = "Fixed";
            }
            else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(':checked')) {
                DiscountTypeVal = "Percentage";
            }

            var PaymentMode = new Array();
            PaymentMode.push({ CardType: '', PaymentType: 'Advance', PaymentDescription: '', PaymentMode: 'Cash', PaymentById: 0, Control: "ContentPlaceHolder1_txtCash" });
            PaymentMode.push({ CardType: 'a', PaymentType: 'Advance', PaymentDescription: 'American Express', PaymentMode: 'Card', PaymentById: $("#ContentPlaceHolder1_hfAmexCardId").val(), Control: "ContentPlaceHolder1_txtAmexCard" });
            PaymentMode.push({ CardType: 'm', PaymentType: 'Advance', PaymentDescription: 'Master Card', PaymentMode: 'Card', PaymentById: $("#ContentPlaceHolder1_hfMasterCardId").val(), Control: "ContentPlaceHolder1_txtMasterCard" });
            PaymentMode.push({ CardType: 'v', PaymentType: 'Advance', PaymentDescription: 'Visa Card', PaymentMode: 'Card', PaymentById: $("#ContentPlaceHolder1_hfVisaCardId").val(), Control: "ContentPlaceHolder1_txtVisaCard" });
            PaymentMode.push({ CardType: 'd', PaymentType: 'Advance', PaymentDescription: 'Discover Card', PaymentMode: 'Card', PaymentById: $("#ContentPlaceHolder1_hfDiscoverCardId").val(), Control: "ContentPlaceHolder1_txtDiscoverCard" });

            //PaymentMode.push({ CardType: '', PaymentType: 'Advance', PaymentDescription: '', PaymentMode: 'Cash', PaymentById: 1, Control: "ContentPlaceHolder1_txtCash" });
            //PaymentMode.push({ CardType: 'a', PaymentType: 'Advance', PaymentDescription: 'American Express', PaymentMode: 'Card', PaymentById: 2, Control: "ContentPlaceHolder1_txtAmexCard" });
            //PaymentMode.push({ CardType: 'm', PaymentType: 'Advance', PaymentDescription: 'Master Card', PaymentMode: 'Card', PaymentById: 3, Control: "ContentPlaceHolder1_txtMasterCard" });
            //PaymentMode.push({ CardType: 'v', PaymentType: 'Advance', PaymentDescription: 'Visa Card', PaymentMode: 'Card', PaymentById: 4, Control: "ContentPlaceHolder1_txtVisaCard" });
            //PaymentMode.push({ CardType: 'd', PaymentType: 'Advance', PaymentDescription: 'Discover Card', PaymentMode: 'Card', PaymentById: 5, Control: "ContentPlaceHolder1_txtDiscoverCard" });

            PaymentMode.push({ CardType: '', PaymentType: 'Advance', PaymentDescription: mBankName, PaymentMode: 'M-Banking', PaymentById: $("#ContentPlaceHolder1_hfMBankId").val(), Control: "ContentPlaceHolder1_txtMBankingPayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'Company', PaymentDescription: companyName, PaymentMode: 'Company', PaymentById: $("#ContentPlaceHolder1_hfCompanyId").val(), Control: "ContentPlaceHolder1_txtCompanyPayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'GuestRoom', PaymentDescription: 'GuestRoom', PaymentMode: 'Other Room', PaymentById: $("#ContentPlaceHolder1_hfRoomId").val(), Control: "ContentPlaceHolder1_txtRoomPayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'Employee', PaymentDescription: employeeName, PaymentMode: 'Employee', PaymentById: $("#ContentPlaceHolder1_hfEmployeeId").val(), Control: "ContentPlaceHolder1_txtEmployeePayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'Member', PaymentDescription: memberName, PaymentMode: 'Member', PaymentById: $("#ContentPlaceHolder1_hfMemberId").val(), Control: "ContentPlaceHolder1_txtMemberPayment" });

            var paymodeCount = PaymentMode.length, row = 0;
            var paymentAmount = "0";
            var pDescription = "";
            var paymentCounter = 1;
            var paymentById = 0;

            for (row = 0; row < paymodeCount; row++) {

                paymentById = $.trim(PaymentMode[row].PaymentById) == "" ? "0" : PaymentMode[row].PaymentById;

                if ($("#" + PaymentMode[row].Control).val() != "" && $("#" + PaymentMode[row].Control).val() != "0") {
                    paymentAmount = $("#" + PaymentMode[row].Control).val();

                    if (paymentAmount == "" || paymentAmount == "0")
                        continue;

                    GuestBillPayment.push({
                        NodeId: 0,
                        PaymentType: PaymentMode[row].PaymentType,
                        AccountsPostingHeadId: 0,
                        BillPaidBy: 0,
                        //BankId: mBankId,
                        BankId: PaymentMode[row].PaymentById,
                        RegistrationId: paymentById,
                        FieldId: hfLocalCurrencyIdVal,
                        ConvertionRate: 1,
                        CurrencyAmount: paymentAmount,
                        PaymentAmount: paymentAmount,
                        ChecqueDate: new Date(),
                        PaymentMode: PaymentMode[row].PaymentMode,
                        PaymentId: 1,
                        CardNumber: "",
                        CardType: PaymentMode[row].CardType,
                        ExpireDate: null,
                        ChecqueNumber: "",
                        CardHolderName: "",
                        PaymentDescription: PaymentMode[row].PaymentDescription,
                        CompanyId: companyId
                    });
                }
                else if ((isComplementary == true || isNonChargeable == true) && paymentCounter == 1) {
                    paymentAmount = $("#" + PaymentMode[row].Control).val();

                    if (employeeId != "0") {
                        if ($.trim(paymentAmount) == "" || $.trim(paymentAmount) == "0") {
                            paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;

                            GuestBillPayment.push({
                                NodeId: 0,
                                PaymentType: PaymentMode[row].PaymentType,
                                AccountsPostingHeadId: 0,
                                BillPaidBy: 0,
                                BankId: 0,
                                RegistrationId: paymentById,
                                FieldId: hfLocalCurrencyIdVal,
                                ConvertionRate: 1,
                                CurrencyAmount: paymentAmount,
                                PaymentAmount: paymentAmount,
                                ChecqueDate: new Date(),
                                PaymentMode: 'Employee',
                                PaymentId: 1,
                                CardNumber: "",
                                CardType: PaymentMode[row].CardType,
                                ExpireDate: null,
                                ChecqueNumber: "",
                                CardHolderName: "",
                                PaymentDescription: PaymentMode[row].PaymentDescription,
                                CompanyId: companyId
                            });

                            paymentCounter = paymentCounter + 1;
                        }
                    }
                    //else if (amexCardBankId != "0") {
                    //    if ($.trim(paymentAmount) == "" || $.trim(paymentAmount) == "0") {
                    //        paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;
                            
                    //        GuestBillPayment.push({
                    //            NodeId: 0,
                    //            PaymentType: PaymentMode[row].PaymentType,
                    //            AccountsPostingHeadId: 0,
                    //            BillPaidBy: 0,
                    //            BankId: paymentById,
                    //            RegistrationId: paymentById,
                    //            FieldId: hfLocalCurrencyIdVal,
                    //            ConvertionRate: 1,
                    //            CurrencyAmount: paymentAmount,
                    //            PaymentAmount: paymentAmount,
                    //            ChecqueDate: new Date(),
                    //            PaymentMode: PaymentMode[row].PaymentMode,
                    //            PaymentId: 1,
                    //            CardNumber: "",
                    //            CardType: PaymentMode[row].CardType,
                    //            ExpireDate: null,
                    //            ChecqueNumber: "",
                    //            CardHolderName: "",
                    //            PaymentDescription: PaymentMode[row].PaymentDescription,
                    //            CompanyId: 0
                    //        });

                    //        paymentCounter = paymentCounter + 1;
                    //    }
                    //}
                    //else if (masterCardBankId != "0") {
                    //    if ($.trim(paymentAmount) == "" || $.trim(paymentAmount) == "0") {
                    //        paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;
                            
                    //        GuestBillPayment.push({
                    //            NodeId: 0,
                    //            PaymentType: PaymentMode[row].PaymentType,
                    //            AccountsPostingHeadId: 0,
                    //            BillPaidBy: 0,
                    //            BankId: paymentById,
                    //            RegistrationId: paymentById,
                    //            FieldId: hfLocalCurrencyIdVal,
                    //            ConvertionRate: 1,
                    //            CurrencyAmount: paymentAmount,
                    //            PaymentAmount: paymentAmount,
                    //            ChecqueDate: new Date(),
                    //            PaymentMode: PaymentMode[row].PaymentMode,
                    //            PaymentId: 1,
                    //            CardNumber: "",
                    //            CardType: PaymentMode[row].CardType,
                    //            ExpireDate: null,
                    //            ChecqueNumber: "",
                    //            CardHolderName: "",
                    //            PaymentDescription: PaymentMode[row].PaymentDescription,
                    //            CompanyId: 0
                    //        });

                    //        paymentCounter = paymentCounter + 1;
                    //    }
                    //}
                    //else if (visaCardBankId != "0") {
                    //    if ($.trim(paymentAmount) == "" || $.trim(paymentAmount) == "0") {
                    //        paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;
                            
                    //        GuestBillPayment.push({
                    //            NodeId: 0,
                    //            PaymentType: PaymentMode[row].PaymentType,
                    //            AccountsPostingHeadId: 0,
                    //            BillPaidBy: 0,
                    //            BankId: paymentById,
                    //            RegistrationId: paymentById,
                    //            FieldId: hfLocalCurrencyIdVal,
                    //            ConvertionRate: 1,
                    //            CurrencyAmount: paymentAmount,
                    //            PaymentAmount: paymentAmount,
                    //            ChecqueDate: new Date(),
                    //            PaymentMode: PaymentMode[row].PaymentMode,
                    //            PaymentId: 1,
                    //            CardNumber: "",
                    //            CardType: PaymentMode[row].CardType,
                    //            ExpireDate: null,
                    //            ChecqueNumber: "",
                    //            CardHolderName: "",
                    //            PaymentDescription: PaymentMode[row].PaymentDescription,
                    //            CompanyId: 0
                    //        });

                    //        paymentCounter = paymentCounter + 1;
                    //    }
                    //}
                    //else if (discoverCardBankId != "0") {
                    //    if ($.trim(paymentAmount) == "" || $.trim(paymentAmount) == "0") {
                    //        paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;
                            
                    //        GuestBillPayment.push({
                    //            NodeId: 0,
                    //            PaymentType: PaymentMode[row].PaymentType,
                    //            AccountsPostingHeadId: 0,
                    //            BillPaidBy: 0,
                    //            BankId: paymentById,
                    //            RegistrationId: paymentById,
                    //            FieldId: hfLocalCurrencyIdVal,
                    //            ConvertionRate: 1,
                    //            CurrencyAmount: paymentAmount,
                    //            PaymentAmount: paymentAmount,
                    //            ChecqueDate: new Date(),
                    //            PaymentMode: PaymentMode[row].PaymentMode,
                    //            PaymentId: 1,
                    //            CardNumber: "",
                    //            CardType: PaymentMode[row].CardType,
                    //            ExpireDate: null,
                    //            ChecqueNumber: "",
                    //            CardHolderName: "",
                    //            PaymentDescription: PaymentMode[row].PaymentDescription,
                    //            CompanyId: 0
                    //        });

                    //        paymentCounter = paymentCounter + 1;
                    //    }
                    //}
                    else if (memberId != "0") {
                        if ($.trim(paymentAmount) == "" || $.trim(paymentAmount) == "0") {
                            paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;

                            GuestBillPayment.push({
                                NodeId: 0,
                                PaymentType: PaymentMode[row].PaymentType,
                                AccountsPostingHeadId: 0,
                                BillPaidBy: 0,
                                BankId: 0,
                                RegistrationId: paymentById,
                                FieldId: hfLocalCurrencyIdVal,
                                ConvertionRate: 1,
                                CurrencyAmount: paymentAmount,
                                PaymentAmount: paymentAmount,
                                ChecqueDate: new Date(),
                                PaymentMode: 'Member',
                                PaymentId: 1,
                                CardNumber: "",
                                CardType: PaymentMode[row].CardType,
                                ExpireDate: null,
                                ChecqueNumber: "",
                                CardHolderName: "",
                                PaymentDescription: PaymentMode[row].PaymentDescription,
                                CompanyId: companyId
                            });

                            paymentCounter = paymentCounter + 1;
                        }
                    }
                    else if (mBankId != "0") {
                        if ($.trim(paymentAmount) == "" || $.trim(paymentAmount) == "0") {
                            paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;
                            
                            GuestBillPayment.push({
                                NodeId: 0,
                                PaymentType: PaymentMode[row].PaymentType,
                                AccountsPostingHeadId: 0,
                                BillPaidBy: 0,
                                BankId: paymentById,
                                RegistrationId: paymentById,
                                FieldId: hfLocalCurrencyIdVal,
                                ConvertionRate: 1,
                                CurrencyAmount: paymentAmount,
                                PaymentAmount: paymentAmount,
                                ChecqueDate: new Date(),
                                PaymentMode: 'M-Banking',
                                PaymentId: 1,
                                CardNumber: "",
                                CardType: PaymentMode[row].CardType,
                                ExpireDate: null,
                                ChecqueNumber: "",
                                CardHolderName: "",
                                PaymentDescription: PaymentMode[row].PaymentDescription,
                                CompanyId: 0
                            });

                            paymentCounter = paymentCounter + 1;
                        }
                    }
                    else if (companyId != "0") {
                        if ($.trim(paymentAmount) == "" || $.trim(paymentAmount) == "0") {
                            paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;

                            GuestBillPayment.push({
                                NodeId: 0,
                                PaymentType: PaymentMode[row].PaymentType,
                                AccountsPostingHeadId: 0,
                                BillPaidBy: 0,
                                BankId: 0,
                                RegistrationId: paymentById,
                                FieldId: hfLocalCurrencyIdVal,
                                ConvertionRate: 1,
                                CurrencyAmount: paymentAmount,
                                PaymentAmount: paymentAmount,
                                ChecqueDate: new Date(),
                                PaymentMode: 'Company',
                                PaymentId: 1,
                                CardNumber: "",
                                CardType: PaymentMode[row].CardType,
                                ExpireDate: null,
                                ChecqueNumber: "",
                                CardHolderName: "",
                                PaymentDescription: PaymentMode[row].PaymentDescription,
                                CompanyId: companyId
                            });

                            paymentCounter = paymentCounter + 1;
                        }
                    }
                    else if (roomId != "0") {
                        if ($.trim(paymentAmount) == "" || $.trim(paymentAmount) == "0") {
                            paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;

                            GuestBillPayment.push({
                                NodeId: 0,
                                PaymentType: PaymentMode[row].PaymentType,
                                AccountsPostingHeadId: 0,
                                BillPaidBy: 0,
                                BankId: 0,
                                RegistrationId: paymentById,
                                FieldId: hfLocalCurrencyIdVal,
                                ConvertionRate: 1,
                                CurrencyAmount: paymentAmount,
                                PaymentAmount: paymentAmount,
                                ChecqueDate: new Date(),
                                PaymentMode: 'Other Room',
                                PaymentId: 1,
                                CardNumber: "",
                                CardType: PaymentMode[row].CardType,
                                ExpireDate: null,
                                ChecqueNumber: "",
                                CardHolderName: "",
                                PaymentDescription: PaymentMode[row].PaymentDescription,
                                CompanyId: companyId
                            });

                            paymentCounter = paymentCounter + 1;
                        }
                    }
                    else {
                        if ($.trim(paymentAmount) == "" || $.trim(paymentAmount) == "0") {
                            paymentAmount = $.trim(paymentAmount) == "" ? "0" : paymentAmount;

                            GuestBillPayment.push({
                                NodeId: 0,
                                PaymentType: PaymentMode[row].PaymentType,
                                AccountsPostingHeadId: 0,
                                BillPaidBy: 0,
                                BankId: 0,
                                RegistrationId: paymentById,
                                FieldId: hfLocalCurrencyIdVal,
                                ConvertionRate: 1,
                                CurrencyAmount: paymentAmount,
                                PaymentAmount: paymentAmount,
                                ChecqueDate: new Date(),
                                PaymentMode: PaymentMode[row].PaymentMode,
                                PaymentId: 1,
                                CardNumber: "",
                                CardType: PaymentMode[row].CardType,
                                ExpireDate: null,
                                ChecqueNumber: "",
                                CardHolderName: "",
                                PaymentDescription: PaymentMode[row].PaymentDescription,
                                CompanyId: companyId
                            });

                            paymentCounter = paymentCounter + 1;
                        }
                    }

                }
            }

            if ($("#ContentPlaceHolder1_txtTPRoundedAmount").val() != "0" && $("#ContentPlaceHolder1_txtTPRoundedAmount").val() != "") {

                roundedAmount = $("#ContentPlaceHolder1_txtTPRoundedAmount").val();

                if (roundedAmount != "" || roundedAmount != "0") {

                    GuestBillPayment.push({
                        NodeId: 0,
                        PaymentType: "Rounded",
                        AccountsPostingHeadId: 0,
                        BillPaidBy: 0,
                        BankId: 0,
                        RegistrationId: 0,
                        FieldId: hfLocalCurrencyIdVal,
                        ConvertionRate: 1,
                        CurrencyAmount: roundedAmount,
                        PaymentAmount: roundedAmount,
                        ChecqueDate: new Date(),
                        PaymentMode: "Rounded",
                        PaymentId: 1,
                        CardNumber: "",
                        CardType: "",
                        ExpireDate: null,
                        ChecqueNumber: "",
                        CardHolderName: "",
                        PaymentDescription: "",
                        CompanyId: companyId
                    });
                }
            }
            //debugger;
            if ($("#ContentPlaceHolder1_lblTPChangeAmount").text() == "Change Amount" && $("#ContentPlaceHolder1_lblTotalChangeAmount").text() == "Change Amount") {
                if ($("#ContentPlaceHolder1_txtTPChangeAmount").val() != "0" && $("#ContentPlaceHolder1_txtTPChangeAmount").val() != "") {

                    var changeAmount = $("#ContentPlaceHolder1_txtTPChangeAmount").val();

                    if (changeAmount != "" || changeAmount != "0") {

                        GuestBillPayment.push({
                            NodeId: 0,
                            PaymentType: "Refund",
                            AccountsPostingHeadId: 0,
                            BillPaidBy: 0,
                            BankId: 0,
                            RegistrationId: 0,
                            FieldId: hfLocalCurrencyIdVal,
                            ConvertionRate: 1,
                            CurrencyAmount: changeAmount,
                            PaymentAmount: changeAmount,
                            ChecqueDate: new Date(),
                            PaymentMode: "Refund",
                            PaymentId: 1,
                            CardNumber: "",
                            CardType: "",
                            ExpireDate: null,
                            ChecqueNumber: "",
                            CardHolderName: "",
                            PaymentDescription: "",
                            CompanyId: companyId
                        });
                    }
                }
            }

            if (settlementType == "settlement") {
                if ($("#ContentPlaceHolder1_hfIsResumeBill").val() == "1" && $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val() != "") {

                    $.ajax({
                        type: "POST",
                        url: "../../../Common/WebMethodPage.aspx/UpdateRestauranBillGenerationNewSettlement",
                        data: JSON.stringify({
                            RestaurantBill: RestaurantBill, GuestBillPayment: GuestBillPayment,
                            BillDetail: BillDetail, BillDeletedDetail: DeletedTableList,
                            AddedClassificationList: AddedClassificationList, DeletedClassificationList: DeletedClassificationList
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {

                            if (result.d.IsBillResettled == true) {
                                CommonHelper.AlertMessage(result.d.AlertMessage);
                                setTimeout(function () { window.location = result.d.RedirectUrl; }, 2000);
                                return false;
                            }

                            if (result.d.IsSuccess == true) {

                                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                                $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val(result.Pk);
                                IsBillOnProcess = "0";

                                if (result.d.BillPrintAndPreview == CommonHelper.RestaurantBillPrintAndPreview.PrintAndPreview) {
                                    $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);
                                    $('#btnPrintPreview').trigger('click');
                                    $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                                }
                                else if (result.d.BillPrintAndPreview == CommonHelper.RestaurantBillPrintAndPreview.BillPreviewOnly) {
                                    $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);
                                    $('#btnPrintPreview').trigger('click');
                                    $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                                }
                                else {
                                    if (result.d.RedirectUrl != "" && result.d.RedirectUrl != null)
                                        window.location = result.d.RedirectUrl;
                                }
                            }
                            else {
                                CommonHelper.AlertMessage(result.d.AlertMessage);
                                IsBillOnProcess = "0";
                            }
                        },
                        error: function (xhr, err) {
                            IsBillOnProcess == "0";
                            toastr.error(xhr.responseText);
                        }
                    });

                }
                else {

                    //PageMethods.SaveRestauranBillGeneration(RestaurantBill, GuestBillPayment, OnRestaurantBillSaveSucceed, OnRestaurantBillSaveFailed);
                    $.ajax({
                        type: "POST",
                        url: "../../../Common/WebMethodPage.aspx/SaveRestaurantBillForAll",
                        data: JSON.stringify({
                            RestaurantBill: RestaurantBill, GuestBillPayment: GuestBillPayment, BillDetail: BillDetail,
                            AddedClassificationList: AddedClassificationList
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {

                            if (result.d.IsBillResettled == true) {
                                CommonHelper.AlertMessage(result.d.AlertMessage);
                                setTimeout(function () { window.location = result.d.RedirectUrl; }, 2000);
                                return false;
                            }

                            if (result.d.IsSuccess == true) {

                                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                                $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val(result.Pk);
                                IsBillOnProcess = "0";

                                if (result.d.BillPrintAndPreview == CommonHelper.RestaurantBillPrintAndPreview.PrintAndPreview) {
                                    $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);
                                    $('#btnPrintPreview').trigger('click');
                                    $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                                }
                                else if (result.d.BillPrintAndPreview == CommonHelper.RestaurantBillPrintAndPreview.BillPreviewOnly) {
                                    $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);
                                    $('#btnPrintPreview').trigger('click');
                                    $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                                }
                                else {
                                    if (result.d.RedirectUrl != "" && result.d.RedirectUrl != null)
                                        window.location = result.d.RedirectUrl;
                                }
                            }
                            else {
                                CommonHelper.AlertMessage(result.d.AlertMessage);
                                IsBillOnProcess = "0";
                            }
                        },
                        error: function (xhr, err) {
                            IsBillOnProcess == "0";
                            toastr.error(xhr.responseText);
                        }
                    });

                }
            }
            else if (settlementType == "holdup") {
                if ($("#ContentPlaceHolder1_hfIsResumeBill").val() == "1") {

                    $.ajax({
                        type: "POST",
                        url: "../../../Common/WebMethodPage.aspx/UpdateHoldUpRestauranBillGenerationForAll",
                        data: JSON.stringify({ RestaurantBill: RestaurantBill, GuestBillPayment: GuestBillPayment }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {

                            if (result.d.IsSuccess == true) {
                                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                                IsBillOnProcess = "0";

                                if (result.d.IsBillHoldUp == true) {
                                    window.location = result.d.RedirectUrl;
                                }
                            }
                            else {
                                CommonHelper.AlertMessage(result.d.AlertMessage);
                                IsBillOnProcess = "0";
                            }
                        },
                        error: function (xhr, err) {
                            IsBillOnProcess == "0";
                            toastr.error(xhr.responseText);
                        }
                    });

                    //PageMethods.UpdateHoldUpRestauranBillGeneration(RestaurantBill, GuestBillPayment, OnRestaurantBillHoldupSucceed, OnRestaurantBillSaveFailed);
                }
                else {

                    $.ajax({
                        type: "POST",
                        url: "../../../Common/WebMethodPage.aspx/HoldUpRestauranBillGenerationForAll",
                        data: JSON.stringify({ RestaurantBill: RestaurantBill, GuestBillPayment: GuestBillPayment }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {

                            if (result.d.IsSuccess == true) {
                                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                                IsBillOnProcess = "0";

                                if (result.d.IsBillHoldUp == true) {
                                    if (result.d.RedirectUrl != "" && result.d.RedirectUrl != null)
                                        window.location = result.d.RedirectUrl;
                                }
                            }
                            else {
                                CommonHelper.AlertMessage(result.d.AlertMessage);
                                IsBillOnProcess = "0";
                            }
                        },
                        error: function (xhr, err) {
                            IsBillOnProcess == "0";
                            toastr.error(xhr.responseText);
                        }

                    });

                    // PageMethods.HoldUpRestauranBillGeneration(RestaurantBill, GuestBillPayment, OnRestaurantBillHoldupSucceed, OnRestaurantBillSaveFailed);
                }
            }

            return false;
        }

        function ScrollToDown() {
            document.getElementById('bottom').scrollIntoView();
        }

        function ScrollToBottom() {
            document.getElementById('bottomPrint').scrollIntoView();
        }

        function OnRestaurantBillSaveSucceed(result) {

            if (result.IsSuccess == true) {

                CommonHelper.SpinnerClose();
                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);

                $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val(result.Pk);
                IsBillOnProcess = "0";

                if (result.IsDirectPrint == false) {

                    $("#ContentPlaceHolder1_hfBillId").val(result.Pk);
                    $('#btnPrintPreview').trigger('click');
                    $("#ContentPlaceHolder1_btnSave").attr("disabled", true);


                    //                    var iframeid = 'printDoc';
                    //                    var url = "/Restaurant/Reports/frmReportTPRestaurantBillInfo.aspx?billID=" + result.Pk;
                    //                    parent.document.getElementById(iframeid).src = url;

                    //setTimeout(PrintAfterSometimes, 5000);

                    //                    ClearWithPopUpInvoicePreview();

                    //                    $("#LoadReport").dialog({
                    //                        autoOpen: true,
                    //                        modal: true,
                    //                        minWidth: 500,
                    //                        minHeight: 555,
                    //                        width: 'auto',
                    //                        closeOnEscape: false,
                    //                        resizable: false,
                    //                        height: 'auto',
                    //                        fluid: true,
                    //                        title: "Invoice Preview",
                    //                        show: 'slide',
                    //                        close: ClosePrintDialog
                    //                    });
                }
                else {
                    window.location = result.RedirectUrl;
                }
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
                IsBillOnProcess = "0";
                CommonHelper.SpinnerClose();
            }
        }

        function OnRestaurantBillHoldupSucceed(result) {
            if (result.IsSuccess == true) {
                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                CommonHelper.SpinnerClose();
                IsBillOnProcess = "0";

                if (result.IsBillHoldUp == true) {
                    window.location = result.RedirectUrl;
                }
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
                CommonHelper.SpinnerClose();
                IsBillOnProcess = "0";
            }
        }

        function ClosePrintDialog() {
            GoToHomePanel();
            CommonHelper.SpinnerClose();
            IsBillOnProcess = "0";
        }

        function ClosePrintPreviewDialog() {
            $("#displayBill").dialog('close');
        }

        function OpenTDecider() {

            var sourceType = $.trim(CommonHelper.GetParameterByName("st"));

            if (sourceType == "tkn") {
                $("#TActionDeciderForToken").dialog({
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
            }
            else if (sourceType == "tbl") {
                $("#TActionDeciderForTable").dialog({
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
            }

            return false;
        }

        function CloseClearActionDecider() {

            if ($("#TActionDeciderForTable").is(':visible')) {
                $("#TActionDeciderForTable").dialog("close");
            }

            if ($('#TActionDeciderForToken').is(':visible')) {
                $("#TActionDeciderForToken").dialog("close");
            }

            return false;
        }

        function ClearOrderedItem() {

            var billId = "", isAlreadyPrint = "";

            billId = $("#ContentPlaceHolder1_hfBillId").val() != "" ? $("#ContentPlaceHolder1_hfBillId").val() : $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val();
            isAlreadyPrint = $("#ContentPlaceHolder1_hfIsBillAlreadyPrint").val();

            if ((billId != "" && billId != "0") || (isAlreadyPrint == "1")) {
                toastr.warning("Bill already generated. Item Cannont Be Clear.");
                return false;
            }

            if ($("#TableWiseItemInformation tbody tr").length == 0) {
                toastr.info("No Item Added To Clear.");
                return;
            }

            $.confirm({
                title: 'Confirm!',
                content: 'Do you want to clear added Item ?',
                buttons: {
                    confirm: function () {
                        var costcenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
                        var kotId = $("#ContentPlaceHolder1_hfKotId").val();

                        PageMethods.ClearOrderedItem(costcenterId, kotId, OnClearOrderedItemSucceed, OnClearOrderedItemFailed);
                    },
                    cancel: function () {
                    }
                }
            });

            return false;
        }
        function OnClearOrderedItemSucceed(result) {
            if (result.IsSuccess == true) {
                LoadGridInformation();
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnClearOrderedItemFailed() { }

        function BackCostCenterSelection() {

            $.confirm({
                title: 'Confirm!',
                content: 'Do you want to back ?',
                buttons: {
                    confirm: function () {
                        if ($("#TActionDeciderForTable").is(':visible')) {
                            $("#TActionDeciderForTable").dialog("close");
                        }

                        if ($('#TActionDeciderForToken').is(':visible')) {
                            $("#TActionDeciderForToken").dialog("close");
                        }

                        if ($("#ContentPlaceHolder1_hfsourceType").val() == "RestaurantToken") {
                            // ValidateFormBeforeSettlement('holdup');
                            //if ($("#TableWiseItemInformation tbody tr").length > 0) {

                            //}
                            //else
                            window.location = "frmCostCenterSelectionForAll.aspx";
                        }
                        else if ($("#ContentPlaceHolder1_hfsourceType").val() == "RestaurantTable") {
                            window.location = "frmCostCenterSelectionForAll.aspx";
                        }
                        else if ($("#ContentPlaceHolder1_hfsourceType").val() == "GuestRoom") {
                            window.location = "frmCostCenterSelectionForAll.aspx";
                        }
                    },
                    cancel: function () {
                    }
                }
            });

            return false;
        }

        function ClearWithPopUpInvoicePreview() {
            $("#ltlTableWiseItemInformation").html('');

            $('#CategoryItemInformationDiv').hide();
            $('#MenuInformationDiv').hide();

            $("#ContentPlaceHolder1_txtSalesAmount").val('0');
            $("#ContentPlaceHolder1_txtDiscountedAmount").val('0');
            $("#ContentPlaceHolder1_txtServiceCharge").val('0');
            $("#ContentPlaceHolder1_txtVatAmount").val('0');
            $("#ContentPlaceHolder1_txtGrandTotal").val('0');
            $("#ContentPlaceHolder1_txtTotalPaymentAmount").val('0');
            $("#ContentPlaceHolder1_txtChangeAmount").val('0');
            $("#ContentPlaceHolder1_txtTotalSales").val('0');
            $("#ContentPlaceHolder1_txtSalesAmountHiddenField").val('0');

            //$("#ItemWiseSpecialRemarks").html('');

            $('#ContentPlaceHolder1_pnlSettlementAndPaymentButtonDiv').hide();
            $('#ContentPlaceHolder1_pnlHomeButtonInfo').show();
        }
        function OnRestaurantBillSaveFailed() { CommonHelper.SpinnerClose(); IsBillOnProcess = "0"; }

        function LoadCategoryNItem(itemLoadType, valuePath) {
            $("#ContentPlaceHolder1_hfValuePath").val(itemLoadType + "," + valuePath);
            $("#ContentPlaceHolder1_btnLoadItemCategory").trigger("click");
        }

        function PromotionalCheckBox(control) {
            //$("#TablePromotionalDiscount tbody tr").find("td:eq(0)").find("input[type='checkbox']").prop("checked", false);
            //$(control).prop("checked", false);
        }

        function LoadCompanyInfo() {

            var companyName = "";

            $("#CompanyInfoDialog").dialog({
                width: 600,
                height: 280,
                autoOpen: true,
                modal: true,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Company Search",
                show: 'slide',
                open: function (event, ui) {
                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                }
            });

            $("#txtSearchCompany").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../POS/frmOrderManagement.aspx/GetGuestCompanyInfo',
                        data: "{'companyName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    Balance: m.Balance,
                                    CompanyAddress: m.CompanyAddress,
                                    ContactNumber: m.ContactNumber,
                                    DiscountPercent: m.DiscountPercent,
                                    Balance: m.Balance,
                                    label: m.CompanyName,
                                    value: m.CompanyId
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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

                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.value);
                    $("#txtCompanyAddress").val(ui.item.CompanyAddress);
                    $("#txtContactNumber").val(ui.item.ContactNumber);
                    $("#txtBalance").val(ui.item.Balance);
                }
            });

        }
        function LoadAmexCardInfo() {

            var companyName = "";

            $("#AmexCardInfoDialog").dialog({
                width: 600,
                height: 280,
                autoOpen: true,
                modal: true,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Bank Search",
                show: 'slide',
                open: function (event, ui) {
                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                }
            });

            $("#txtSearchAmexCard").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../POS/frmOrderManagement.aspx/GetBankInfoForAutoComplete',
                        data: "{'bankName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    AccountName: m.AccountName,
                                    BranchName: m.BranchName,
                                    AccountNumber: m.AccountNumber,
                                    AccountType: m.AccountType,
                                    Description: m.Description,
                                    label: m.BankName,
                                    value: m.BankId
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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

                    $("#ContentPlaceHolder1_hfAmexCardId").val(ui.item.value);
                    //$("#txtCompanyAddress").val(ui.item.CompanyAddress);
                    //$("#txtContactNumber").val(ui.item.ContactNumber);
                    //$("#txtBalance").val(ui.item.Balance);
                }
            });
        }

            function LoadMasterCardInfo() {

                var companyName = "";

                $("#MasterCardInfoDialog").dialog({
                    width: 600,
                    height: 280,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: false,
                    resizable: false,
                    fluid: true,
                    title: "Bank Search",
                    show: 'slide',
                    open: function (event, ui) {
                        $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                    }
                });

                $("#txtSearchMasterCard").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: '../POS/frmOrderManagement.aspx/GetBankInfoForAutoComplete',
                            data: "{'bankName':'" + request.term + "'}",
                            dataType: "json",
                            success: function (data) {

                                var searchData = data.error ? [] : $.map(data.d, function (m) {
                                    return {
                                        AccountName: m.AccountName,
                                        BranchName: m.BranchName,
                                        AccountNumber: m.AccountNumber,
                                        AccountType: m.AccountType,
                                        Description: m.Description,
                                        label: m.BankName,
                                        value: m.BankId
                                    };
                                });
                                response(searchData);
                            },
                            error: function (xhr, err) {
                                toastr.error(xhr.responseText);
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

                        $("#ContentPlaceHolder1_hfMasterCardId").val(ui.item.value);
                        //$("#txtCompanyAddress").val(ui.item.CompanyAddress);
                        //$("#txtContactNumber").val(ui.item.ContactNumber);
                        //$("#txtBalance").val(ui.item.Balance);
                    }
                });
            }

                function LoadVisaCardInfo() {

                    var companyName = "";

                    $("#VisaCardInfoDialog").dialog({
                        width: 600,
                        height: 280,
                        autoOpen: true,
                        modal: true,
                        closeOnEscape: false,
                        resizable: false,
                        fluid: true,
                        title: "Bank Search",
                        show: 'slide',
                        open: function (event, ui) {
                            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                        }
                    });

                    $("#txtSearchVisaCard").autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: '../POS/frmOrderManagement.aspx/GetBankInfoForAutoComplete',
                                data: "{'bankName':'" + request.term + "'}",
                                dataType: "json",
                                success: function (data) {

                                    var searchData = data.error ? [] : $.map(data.d, function (m) {
                                        return {
                                            AccountName: m.AccountName,
                                            BranchName: m.BranchName,
                                            AccountNumber: m.AccountNumber,
                                            AccountType: m.AccountType,
                                            Description: m.Description,
                                            label: m.BankName,
                                            value: m.BankId
                                        };
                                    });
                                    response(searchData);
                                },
                                error: function (xhr, err) {
                                    toastr.error(xhr.responseText);
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

                            $("#ContentPlaceHolder1_hfVisaCardId").val(ui.item.value);
                            //$("#txtCompanyAddress").val(ui.item.CompanyAddress);
                            //$("#txtContactNumber").val(ui.item.ContactNumber);
                            //$("#txtBalance").val(ui.item.Balance);
                        }
                    });
                }

                    function LoadDiscoverCardInfo() {

                        var companyName = "";

                        $("#DiscoverCardInfoDialog").dialog({
                            width: 600,
                            height: 280,
                            autoOpen: true,
                            modal: true,
                            closeOnEscape: false,
                            resizable: false,
                            fluid: true,
                            title: "Bank Search",
                            show: 'slide',
                            open: function (event, ui) {
                                $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                            }
                        });

                        $("#txtSearchDiscoverCard").autocomplete({
                            source: function (request, response) {
                                $.ajax({
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    url: '../POS/frmOrderManagement.aspx/GetBankInfoForAutoComplete',
                                    data: "{'bankName':'" + request.term + "'}",
                                    dataType: "json",
                                    success: function (data) {

                                        var searchData = data.error ? [] : $.map(data.d, function (m) {
                                            return {
                                                AccountName: m.AccountName,
                                                BranchName: m.BranchName,
                                                AccountNumber: m.AccountNumber,
                                                AccountType: m.AccountType,
                                                Description: m.Description,
                                                label: m.BankName,
                                                value: m.BankId
                                            };
                                        });
                                        response(searchData);
                                    },
                                    error: function (xhr, err) {
                                        toastr.error(xhr.responseText);
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

                                $("#ContentPlaceHolder1_hfDiscoverCardId").val(ui.item.value);
                                //$("#txtCompanyAddress").val(ui.item.CompanyAddress);
                                //$("#txtContactNumber").val(ui.item.ContactNumber);
                                //$("#txtBalance").val(ui.item.Balance);
                            }
                        });
                    }

        function LoadMBankingInfo() {

            var companyName = "";

            $("#MBankingInfoDialog").dialog({
                width: 600,
                height: 280,
                autoOpen: true,
                modal: true,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Bank Search",
                show: 'slide',
                open: function (event, ui) {
                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                }
            });

            $("#txtSearchMBanking").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../POS/frmOrderManagement.aspx/GetBankInfoForAutoComplete',
                        data: "{'bankName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    AccountName: m.AccountName,
                                    BranchName: m.BranchName,
                                    AccountNumber: m.AccountNumber,
                                    AccountType: m.AccountType,
                                    Description: m.Description,
                                    label: m.BankName,
                                    value: m.BankId
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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

                    $("#ContentPlaceHolder1_hfMBankId").val(ui.item.value);
                    //$("#txtCompanyAddress").val(ui.item.CompanyAddress);
                    //$("#txtContactNumber").val(ui.item.ContactNumber);
                    //$("#txtBalance").val(ui.item.Balance);
                }
            });

        }
        
        function AmexCardPayment() {
            if ($("#ContentPlaceHolder1_hfAmexCardId").val() != "") {                
                $("#lblAmexCardBankName").text($("#txtSearchAmexCard").val());
            }
            $("#AmexCardInfoDialog").dialog('close');
            $("#ContentPlaceHolder1_txtAmexCard").focus();
        }
        function MasterCardPayment() {
            if ($("#ContentPlaceHolder1_hfMasterCardId").val() != "") {                
                $("#lblMasterCardBankName").text($("#txtSearchMasterCard").val());
            }
            $("#MasterCardInfoDialog").dialog('close');
            $("#ContentPlaceHolder1_txtMasterCard").focus();
        }
        function VisaCardPayment() {
            if ($("#ContentPlaceHolder1_hfVisaCardId").val() != "") {                
                $("#lblVisaCardBankName").text($("#txtSearchVisaCard").val());
            }
            $("#VisaCardInfoDialog").dialog('close');
            $("#ContentPlaceHolder1_txtVisaCard").focus();
        }
        function DiscoverCardPayment() {
            if ($("#ContentPlaceHolder1_hfDiscoverCardId").val() != "") {                
                $("#lblDiscoverCardBankName").text($("#txtSearchDiscoverCard").val());
            }
            $("#DiscoverCardInfoDialog").dialog('close');
            $("#ContentPlaceHolder1_txtDiscoverCard").focus();
        }
        function MBankingPayment() {
            if ($("#ContentPlaceHolder1_hfMBankId").val() != "") {
                $("#lblMBankName").text($("#txtSearchMBanking").val());
            }
            $("#MBankingInfoDialog").dialog('close');
            $("#ContentPlaceHolder1_txtMBankingPayment").focus();
        }
        function CompanyPayment() {
            if ($("#ContentPlaceHolder1_hfCompanyId").val() != "") {
                $("#lblCompanyName").text($("#txtSearchCompany").val());
            }
            $("#CompanyInfoDialog").dialog('close');
            $("#ContentPlaceHolder1_txtCompanyPayment").focus();
        }

        function LoadEmployeeInfo() {

            $("#EmployeeInfoDialog").dialog({
                width: 600,
                height: 325,
                autoOpen: true,
                modal: true,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Employee Search",
                show: 'slide',
                open: function (event, ui) {
                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                }
            });

            $("#txtEmployeeNameSearch").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../../Common/WebMethodPage.aspx/SearchEmployeeByName",
                        data: "{'employeeName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    EmpId: m.EmpId,
                                    EmployeeName: m.EmployeeName,
                                    Department: m.Department,
                                    Designation: m.Designation,
                                    label: m.EmployeeName,
                                    value: m.EmpId
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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

                    $("#ContentPlaceHolder1_hfEmployeeId").val(ui.item.EmpId);
                    $("#txtEmployeeName").val(ui.item.EmployeeName);
                    $("#txtDepartment").val(ui.item.Department);
                    $("#txtDesignation").val(ui.item.Designation);
                }
            });
        }

        function LoadMemberInfo() {

            $("#MemberInfoDialog").dialog({
                width: 600,
                height: 400,
                autoOpen: true,
                modal: true,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Member Search",
                show: 'slide',
                open: function (event, ui) {
                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                }
            });

            $("#txtMemberNameSearch").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "../../Common/WebMethodPage.aspx/GetMemberInfoByName",
                        data: "{'memberName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    MemberId: m.MemberId,
                                    FullName: m.FullName,
                                    TypeName: m.TypeName,
                                    MemberAddress: m.MemberAddress,
                                    MobileNumber: m.MobileNumber,
                                    label: m.FullName,
                                    value: m.MemberId
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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

                    $("#ContentPlaceHolder1_hfMemberId").val(ui.item.MemberId);
                    $("#txtMemberName").val(ui.item.FullName);
                    $("#txtMemberType").val(ui.item.TypeName);
                    $("#txtMemberAddress").val(ui.item.MemberAddress);
                    $("#txtMemberContactNumber").val(ui.item.MobileNumber);
                    $("#txtMemberBalance").val(0);
                }
            });
        }

        function EmployeeSearchByCode() {

            var employeeCode = $.trim($("#txtEmployeeCode").val());
            if (employeeCode == "") { toastr.info("Please Give Employee Code."); return false; }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../Common/WebMethodPage.aspx/SearchEmployee",
                data: JSON.stringify({ employeeCode: employeeCode }),
                dataType: "json",
                success: function (data) {
                    $("#ContentPlaceHolder1_hfEmployeeId").val(data.d.EmpId);
                    $("#txtEmployeeName").val(data.d.DisplayName);
                    $("#txtDepartment").val(data.d.Department);
                    $("#txtDesignation").val(data.d.Designation);
                },
                error: function (xhr, err) {
                    toastr.error(xhr.responseText);
                }
            });
        }

        function MemberSearchByCode() {

            var membershipNumber = $.trim($("#txtMemberNumber").val());
            if (membershipNumber == "") { toastr.info("Please Give Membership Number."); return false; }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../Common/WebMethodPage.aspx/GetMemberInfoByMembershipNo",
                data: JSON.stringify({ membershipNumber: membershipNumber }),
                dataType: "json",
                success: function (data) {

                    if (data.d != null) {
                        $("#ContentPlaceHolder1_hfMemberId").val(data.d.MemberId);
                        $("#txtMemberName").val(data.d.FullName);
                        $("#txtMemberType").val(data.d.TypeName);
                        $("#txtMemberAddress").val(data.d.MemberAddress);
                        $("#txtMemberContactNumber").val(data.d.MobileNumber);
                        $("#txtMemberBalance").val(0);
                        $("#ContentPlaceHolder1_rbTPPercentageDiscount").prop("checked", true);
                        $("#ContentPlaceHolder1_txtTPDiscountAmount").focus();
                        $("#ContentPlaceHolder1_txtTPDiscountAmount").val(data.d.DiscountPercent);
                        $("#ContentPlaceHolder1_txtCash").focus();
                        $("#ContentPlaceHolder1_txtTPDiscountAmount").trigger("onblur");
                    }
                },
                error: function (xhr, err) {
                    toastr.error(xhr.responseText);
                }
            });
        }

        function EmployeePayment() {
            if ($("#ContentPlaceHolder1_hfEmployeeId").val() != "")
                $("#lblEmployeeName").text($("#txtEmployeeName").val());
            $("#EmployeeInfoDialog").dialog('close')
            $("#ContentPlaceHolder1_txtEmployeePayment").focus();
        }

        function MemberPayment() {
            if ($("#ContentPlaceHolder1_hfMemberId").val() != "")
                $("#lblMemberName").text($("#txtMemberName").val() + "(" + $("#txtMemberType").val() + ") ");
            $("#MemberInfoDialog").dialog('close')
            $("#ContentPlaceHolder1_txtMemberPayment").focus();
        }

        function UserAccessVarification(userid, password) {
            userIdAuthorised = userid;
            userIdAuthorisedPassword = password;
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../Common/WebMethodPage.aspx/UserAccessVarification", //'../Restaurant/frmRestaurantOrderManagement.aspx/UserAccessVarification',
                data: "{'userid':'" + userid + "','password':'" + password + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d.IsSuccess == true) {
                        $("#ContentPlaceHolder1_hfIsItemCanEditDelete").val(data.d.IsReservationCheckInDateValidation == true ? "1" : "0");
                        $("#ContentPlaceHolder1_hfIsAccessVarified").val("1");
                    }
                    else {
                        userIdAuthorised = "";
                        userIdAuthorisedPassword = "";
                        toastr.warning('User Have No Access To Edit/Delete.');
                        $("#ContentPlaceHolder1_hfIsAccessVarified").val("0");
                    }
                },
                error: function (xhr, err) {
                    userIdAuthorised = "";
                    userIdAuthorisedPassword = "";
                    toastr.error(xhr.responseText);
                }
            });
        }

        function CloseAccessPermissionForEditDelete() {
            $("#ContentPlaceHolder1_hfIsAccessVarified").val("0");
        }

        function SearchItemAdded() {

            var itemQuantity = $("#txtItemQuantity").val();

            if ($.trim(itemQuantity) == "" || $.trim(itemQuantity) == "0") {
                toastr.info("Please Give Item Quantity");
                return false;
            }

            var itemQuantity1 = +$.trim($("#txtItemQuantity").val());
            if (isNaN((itemQuantity1))) {
                toastr.info("Please Give Proper Item Quantity");
                return false;
            }
            if (Math.sign((itemQuantity1)) == -1 || Math.sign((itemQuantity1)) == -0) {
                toastr.info("Please Give Proper Item Quantity");
                return false;
            }
            

            var itemId = $("#ContentPlaceHolder1_hfSearchItemId").val();
            var itemName = $("#ContentPlaceHolder1_hfSearchItemName").val();

            SearchAndAddItemToOrder(itemId, itemName);
        }

        function SearchAndAddItemToOrder(selectedItemId, selectedItemName) {

            var hfCostCenterIdVal = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var sourceName = $("#ContentPlaceHolder1_hfsourceType").val();

            CommonHelper.ExactMatch();

            var tr = $("#TableWiseItemInformation tbody tr").find("td:eq(5):textEquals('" + $.trim(selectedItemId) + "')").parent();
            var selectedItemQty = "0", itemIdInTable = "0", itemNameInTable = "", searchItemQuantity = "";

            searchItemQuantity = $("#txtItemQuantity").val();

            if ($(tr).length != 0) {
                selectedItemQty = $(tr).find("td:eq(1)").text();
                itemIdInTable = $(tr).find("td:eq(5)").text();
                itemNameInTable = $(tr).find("td:eq(0)").text();

                if ($.trim(searchItemQuantity) != "" && $.trim(searchItemQuantity) != "0") {
                    selectedItemQty = parseInt(selectedItemQty, 10) + parseInt(searchItemQuantity, 10);
                }
            }
            else {
                selectedItemQty = searchItemQuantity;
            }

            if (itemIdInTable != "0") {
                if ($.trim(itemNameInTable) != $.trim(selectedItemName)) {
                    toastr.error("Same item can not be added as different name.");
                    return false;
                }
            }

            var isItemCanEditDelete = $("#ContentPlaceHolder1_hfIsItemCanEditDelete").val();

            PageMethods.SaveIndividualItemDetailInformation(isItemCanEditDelete, hfCostCenterIdVal, kotId, selectedItemId,
                selectedItemQty, sourceName, OnSaveObjectSucceeded, OnSaveObjectFailed);
            return false;
        }

        function LoadRoomWiseGuestInfo() {
            CommonHelper.SpinnerOpen();
            var roomNumer = $("#ContentPlaceHolder2_lblOrderSourceNumber").text();
            var guestList = $("#ContentPlaceHolder1_hfGuestList").val();

            var sourceType = $.trim(CommonHelper.GetParameterByName("st"));
            if (sourceType == "tkn" || sourceType == "tbl") {
                var roomText = $("#lblGuestRoom").text();
                var r = roomText.split('#');
                roomNumer = $.trim(r[1]);
            }

            PageMethods.LoadRoomWiseGuestInfo(roomNumer, guestList, OnLoadGuestSucceeded, OnGuestFailed);
        }

        function OnLoadGuestSucceeded(result) {

            $("#GuestContainer").html(result);

            $("#GuestDialog").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 300,
                minHeight: 300,
                width: 'auto',
                closeOnEscape: false,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Guest Info",
                show: 'slide'
            });

            //if (AddedClassificationList.length > 0) {

            //    $("#TableClassificationWiseDiscount tbody tr").each(function () {

            //        var id = _.findWhere(AddedClassificationList, { ClassificationId: parseInt($.trim($(this).find("td:eq(2)").text()), 10) });

            //        if (id != null) {
            //            $(this).find("td:eq(0)").find("input").attr('checked', true);
            //        }
            //    });
            //}

            CommonHelper.SpinnerClose();
        }
        function OnGuestFailed(error) { }

        function SaveWithPriceUpdate() {

            var itemId = $("#ContentPlaceHolder1_hfItemId").val();
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();;
            SaveNewRecipe();
            PageMethods.SaveWithPriceUpdate(itemId, kotId, SaveWithPriceUpdateSucceeded, SaveWithPriceUpdateFailed);

        }

        function SaveWithPriceUpdateSucceeded(result) {

            LoadGridInformation();

        }

        function SaveWithPriceUpdateFailed(error) { }

        function WaiterChange() {

            var sourceType = $.trim(CommonHelper.GetParameterByName("st"));
            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            var paxQuantity = $("#ContentPlaceHolder1_hfPaxQuantity").val();

            //$("#ContentPlaceHolder1_txtPax").val(paxQuantity);

            $("#WaiterChangeDialog").dialog({
                width: 440,
                height: 400,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Waiter Change",
                show: 'slide'
            });
        }

        function WaiterChangeApply() {

            if ($("#ContentPlaceHolder1_hfBillOrderTakingWaiterId").val() == "") {
                toastr.info("Invalid Waiter. Please Give Vaild Waiter Name and Then Search.");
                return false;
            }

            CommonHelper.SpinnerOpen();

            var waiter = "", costCenterId = 0, kotId = 0;
            costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            kotId = $("#ContentPlaceHolder1_hfKotId").val();

            waiter = $("#ContentPlaceHolder1_txtWaiter").val();

            if ($.trim(waiter) == "") {
                toastr.warning("Waiter Cannot Empty.");
                CommonHelper.SpinnerClose();
                return false;
            }

            waiterId = $("#ContentPlaceHolder1_hfBillOrderTakingWaiterId").val();
            $("#WaiterChangeDialog").dialog("close");

            //var keyboard = $('.numkbnotdecimalpax').getkeyboard();
            //keyboard.destroy(); 

            PageMethods.UpdateKotWaiterInformation(costCenterId, kotId, waiterId, OnWaiterChangeSucceeded, OnWaiterChangeFailed);
            return false;
        }

        function OnWaiterChangeSucceeded(result) {

            if (result.AlertMessage.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();

            return false;
        }
        function OnWaiterChangeFailed(xhr, err) {
            CommonHelper.SpinnerClose();
            toastr.error(xhr.responseText);
        }

        function CheckInputCategoryDiscount(txt) {

            var tr = $(txt).parent().parent();

            if ($.trim($(txt).val()) != "") {
                var discount = parseFloat($(txt).val());

                if (discount > 100) {
                    toastr.info("Discount Cannot Greater Than 100%");
                    $(tr).find("td:eq(0)").find("input").prop("checked", false);
                    $(txt).val("");
                }
                else if (discount < 0) {
                    toastr.info("Discount Cannot Less Than 0");
                    $(txt).val("");
                }
                else if (discount == 0) {

                    toastr.info("Discount Cannot Be 0");
                    $(txt).val("");
                    $(tr).find("td:eq(0)").find("input").prop("checked", false);
                }
            }
        }

        function ClearPayment(options) {
            if (options == "AmexCard") {
                $("#ContentPlaceHolder1_hfAmexCardId").val("");
                $("#lblAmexCardBankName").text("");

                //$("#txtSearchCompany").val("");
                //$("#txtCompanyAddress").val("");
                //$("#txtContactNumber").val("");
                //$("#txtBalance").val("");
                //$("#ContentPlaceHolder1_txtCompanyPayment").val("");
                //$("#ContentPlaceHolder1_txtRemarks").val("");

                if ($("#AmexCardInfoDialog").is(":visible")) {
                    $("#AmexCardInfoDialog").dialog("close");
                }
            }
            else if (options == "MasterCard") {
                $("#ContentPlaceHolder1_hfMasterCardId").val("");
                $("#lblMasterCardBankName").text("");

                //$("#txtSearchCompany").val("");
                //$("#txtCompanyAddress").val("");
                //$("#txtContactNumber").val("");
                //$("#txtBalance").val("");
                //$("#ContentPlaceHolder1_txtCompanyPayment").val("");
                //$("#ContentPlaceHolder1_txtRemarks").val("");

                if ($("#MasterCardInfoDialog").is(":visible")) {
                    $("#MasterCardInfoDialog").dialog("close");
                }
            }
            else if (options == "VisaCard") {
                $("#ContentPlaceHolder1_hfVisaCardId").val("");
                $("#lblVisaCardBankName").text("");

                //$("#txtSearchCompany").val("");
                //$("#txtCompanyAddress").val("");
                //$("#txtContactNumber").val("");
                //$("#txtBalance").val("");
                //$("#ContentPlaceHolder1_txtCompanyPayment").val("");
                //$("#ContentPlaceHolder1_txtRemarks").val("");

                if ($("#VisaCardInfoDialog").is(":visible")) {
                    $("#VisaCardInfoDialog").dialog("close");
                }
            }
            else if (options == "DiscoverCard") {
                $("#ContentPlaceHolder1_hfDiscoverCardId").val("");
                $("#lblDiscoverCardBankName").text("");

                //$("#txtSearchCompany").val("");
                //$("#txtCompanyAddress").val("");
                //$("#txtContactNumber").val("");
                //$("#txtBalance").val("");
                //$("#ContentPlaceHolder1_txtCompanyPayment").val("");
                //$("#ContentPlaceHolder1_txtRemarks").val("");

                if ($("#DiscoverCardInfoDialog").is(":visible")) {
                    $("#DiscoverCardInfoDialog").dialog("close");
                }
            }
            else if (options == "Guest") {
                $("#ContentPlaceHolder1_hfRoomId").val("");
                $("#ContentPlaceHolder1_hfIsRoomIsSelectForPayment").val("");
                $("#lblGuestRoom").text("Room Payment");
                $("#ContentPlaceHolder1_txtRoomPayment").val("");
                $("#ContentPlaceHolder1_txtRemarks").val("");

                ClearRoomDetails();

                if ($("#RoomInfoDialog").is(":visible")) {
                    $("#RoomInfoDialog").dialog("close");
                }
            }
            else if (options == "Company") {
                $("#ContentPlaceHolder1_hfCompanyId").val("");
                $("#lblCompanyName").text("");

                $("#txtSearchCompany").val("");
                $("#txtCompanyAddress").val("");
                $("#txtContactNumber").val("");
                $("#txtBalance").val("");
                $("#ContentPlaceHolder1_txtCompanyPayment").val("");
                $("#ContentPlaceHolder1_txtRemarks").val("");

                if ($("#CompanyInfoDialog").is(":visible")) {
                    $("#CompanyInfoDialog").dialog("close");
                }
            }
            else if (options == "M-Banking") {
                $("#ContentPlaceHolder1_hfCompanyId").val("");
                $("#lblMBankName").text("");

                $("#txtSearchCompany").val("");
                $("#txtCompanyAddress").val("");
                $("#txtContactNumber").val("");
                $("#txtBalance").val("");
                $("#ContentPlaceHolder1_txtCompanyPayment").val("");
                $("#ContentPlaceHolder1_txtRemarks").val("");

                if ($("#MBankingInfoDialog").is(":visible")) {
                    $("#MBankingInfoDialog").dialog("close");
                }
            }
            else if (options == "Employee") {
                $("#lblEmployeeName").text("");
                $("#ContentPlaceHolder1_hfEmployeeId").val("");

                $("#txtEmployeeCode").val("");
                $("#txtEmployeeNameSearch").val("");
                $("#txtEmployeeName").val("");
                $("#txtDepartment").val("");
                $("#txtDesignation").val("");
                $("#ContentPlaceHolder1_txtEmployeePayment").val("");
                $("#ContentPlaceHolder1_txtRemarks").val("");

                if ($("#EmployeeInfoDialog").is(":visible")) {
                    $("#EmployeeInfoDialog").dialog("close");
                }

            }
            else if (options == "Member") {
                $("#ContentPlaceHolder1_hfMemberId").val("");
                $("#lblMemberName").text("");
                $("#ContentPlaceHolder1_txtRemarks").val("");

                $("#txtMemberNumber").val("");
                $("#txtMemberNameSearch").val("");
                $("#txtMemberName").val("");
                $("#txtMemberType").val("");
                $("#txtMemberAddress").val("");
                $("#txtMemberContactNumber").val("");
                $("#txtMemberBalance").val("");
                $("#ContentPlaceHolder1_txtMemberPayment").val("");

                if ($("#MemberInfoDialog").is(":visible")) {
                    $("#MemberInfoDialog").dialog("close");
                }
            }

            CalculatePayment();
            $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
        }

        function ClearCardPaymentFields() {

            var PaymentMode = new Array();


            var companyName = $("#lblCompanyName").text();
            var employeeName = $("#lblEmployeeName").text();
            var memberName = $("#txtMemberName").val();

            PaymentMode.push({ CardType: '', PaymentType: 'Advance', PaymentDescription: '', PaymentMode: 'Cash', PaymentById: '', Control: "ContentPlaceHolder1_txtCash" });
            PaymentMode.push({ CardType: 'a', PaymentType: 'Advance', PaymentDescription: 'American Express', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtAmexCard" });
            PaymentMode.push({ CardType: 'm', PaymentType: 'Advance', PaymentDescription: 'Master Card', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtMasterCard" });
            PaymentMode.push({ CardType: 'v', PaymentType: 'Advance', PaymentDescription: 'Visa Card', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtVisaCard" });
            PaymentMode.push({ CardType: 'd', PaymentType: 'Advance', PaymentDescription: 'Discover Card', PaymentMode: 'Card', PaymentById: '', Control: "ContentPlaceHolder1_txtDiscoverCard" });
            PaymentMode.push({ CardType: '', PaymentType: 'Company', PaymentDescription: companyName, PaymentMode: 'Company', PaymentById: $("#ContentPlaceHolder1_hfCompanyId").val(), Control: "ContentPlaceHolder1_txtCompanyPayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'GuestRoom', PaymentDescription: 'GuestRoom', PaymentMode: 'Other Room', PaymentById: $("#ContentPlaceHolder1_hfRoomId").val(), Control: "ContentPlaceHolder1_txtRoomPayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'Employee', PaymentDescription: employeeName, PaymentMode: 'Employee', PaymentById: $("#ContentPlaceHolder1_hfEmployeeId").val(), Control: "ContentPlaceHolder1_txtEmployeePayment" });
            PaymentMode.push({ CardType: '', PaymentType: 'Member', PaymentDescription: memberName, PaymentMode: 'Member', PaymentById: $("#ContentPlaceHolder1_hfMemberId").val(), Control: "ContentPlaceHolder1_txtMemberPayment" });


            var paymodeCount = PaymentMode.length, row = 0;

            for (row = 0; row < paymodeCount; row++) {
                if (PaymentMode[row].PaymentMode != "Cash")
                    $("#" + PaymentMode[row].Control).val("");
            }

        }

    </script>
    <asp:HiddenField ID="hfsourceType" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCostCenterId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfKotId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfBillId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfKotDetailId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsResumeBill" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfSourceId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBearerId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfBearerCanSettleBill" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsVatEnable" runat="server" />
    <asp:HiddenField ID="hfIsServiceChargeEnable" runat="server" />
    <asp:HiddenField ID="hfIsSDChargeEnable" runat="server" />
    <asp:HiddenField ID="hfIsAdditionalChargeEnable" runat="server" />
    <asp:HiddenField ID="hfAdditionalChargeType" runat="server" />
    <asp:HiddenField ID="hfIsVatOnSD" runat="server" />
    <asp:HiddenField ID="hfIsRestaurantBillInclusive" runat="server" />
    <asp:HiddenField ID="hfIsRatePlusPlus" runat="server" />
    <asp:HiddenField ID="hfIsDiscountApplicableOnRackRate" runat="server" />
    <asp:HiddenField ID="hfRestaurantVatAmount" runat="server" />
    <asp:HiddenField ID="hfRestaurantServiceCharge" runat="server" />
    <asp:HiddenField ID="hfAdditionalCharge" runat="server" />
    <asp:HiddenField ID="hfSDCharge" runat="server" />
    <asp:HiddenField ID="hfltlTableWiseItemInformationDivHeight" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfAddedTableIdForBill" runat="server" Value="" />
    <asp:HiddenField ID="hfAddedClassificationId" runat="server" Value="" />
    <asp:HiddenField ID="hfItemId" runat="server" />
    <asp:HiddenField ID="hfStockType" runat="server" />
    <asp:HiddenField ID="hfBillIdForInvoicePreview" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRestaurantBillAmountWillRound" runat="server" />
    <asp:HiddenField ID="hfLocalCurrencyId" runat="server" />
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfBillTemplate" runat="server" />
    <asp:HiddenField ID="hfValuePath" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfPromotionalDiscountType" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfAmexCardId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfMasterCardId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfVisaCardId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfDiscoverCardId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfMBankId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfEmployeeId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfMemberId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfBillIdDetailsId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfPaxQuantity" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsItemCanEditDelete" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsUserHasEditDeleteAccess" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsOrderSubmit" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsAccessVarified" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsItemEditOrDelete" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfChangeType" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfIsPrintRelatedButtonDisable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfRegistrationId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsStopChargePosting" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsBearar" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsItemSearchEnable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfGuestList" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfBillOrderTakingWaiterId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfChangeWaiterName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfRoomId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfIsRoomIsSelectForPayment" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfIsRestaurantIntegrateWithFrontOffice" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRestaurantIntegrateWithPayroll" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRestaurantIntegrateWithCompany" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRestaurantIntegrateWithMember" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsBillAlreadyPrint" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfSearchItemId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfSearchItemName" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsBillPreviewButtonEnable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsPredefinedRemarksEnable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfRoomNumber" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfRowIndex" runat="server" Value=""></asp:HiddenField>

    <div id="displayBill" style="display: none;">
        <div class="row no-gutters">
            <div class="col-sm-12">
                <div class="well well-sm col-sm-12">
                    <div class="row pull-right">
                        <div class="col-sm-12">
                            <input type="button" class="btn btn-primary btn-large" value="Close" onclick="ClosePrintPreviewDialog()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row no-gutters">
            <div class="col-sm-12">
                <iframe id="printDoc" name="printDoc" width="500" height="650" frameborder="0" style="overflow: hidden;"></iframe>
                <div id="bottomPrint">
                </div>
            </div>
        </div>
    </div>
    <div id="Div5" style="display: none;">
        <asp:Button ID="btnPrintPreview" runat="server" Text="Print Preview" ClientIDMode="Static"
            OnClick="btnPrintPreview_Click" />
        <asp:Button ID="btnBillNPrintPreview" runat="server" Text="Bill Preview" ClientIDMode="Static"
            OnClick="btnBillNPrintPreview_Click" />
    </div>
    <div id="LoadReport" style="display: none;">
        <div class="well well-small" style="text-align: right">
            <input type="button" class="btn btn-primary btn-large" value="Print" onclick="PrintDocumentFunc('1')" />
            <input type="button" class="btn btn-primary btn-large" value="Close" onclick="ClosePrintDialog()" />
        </div>
        <div style="height: 555px; overflow-y: scroll;" id="reportContainer">
            <rsweb:ReportViewer ID="rvTransactionShow" ShowFindControls="false" ShowWaitControlCancelLink="false"
                PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" ShowPageNavigationControls="true" ZoomMode="FullPage"
                Height="2000" ClientIDMode="Static" ShowRefreshButton="false">
            </rsweb:ReportViewer>
            <div id="bottom">
            </div>
        </div>
    </div>

    <div id="PercentageDiscountDialog" style="display: none;">
        <div class="form-horizontal">
            <div class="row">
                <div class="col-sm-12">
                    <div class="DataGridYScroll">
                        <div id="PercentageDiscountContainer">
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12">
                    <div style="float: left; padding-bottom: 15px; margin-top: 5px;">
                        <input type="button" id="btnCategoryWiseDiscountOK" class="TransactionalButton btn btn-primary"
                            style="width: 80px;" value="Ok" />
                        <input type="button" id="btnCategoryWiseDiscountClear" class="TransactionalButton btn btn-primary"
                            style="width: 80px;" value="Clear" />
                        <input type="button" id="btnCategoryWiseDiscountCancel" class="TransactionalButton btn btn-primary"
                            style="width: 80px;" value="Cancel" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="PromotionalDiscountDialog" style="width: 450px; display: none;">
        <div class="">
            <div class="DataGridYScroll">
                <div id="PromotionalDiscountContainer" runat="server" clientidmode="Static" style="width: 100%;">
                </div>
            </div>
            <div id="Div10" style="padding-left: 5px; width: 100%; margin-top: 5px;">
                <div style="float: left; padding-bottom: 15px;">
                    <input type="button" id="btnPromotionalDiscountOK" class="TransactionalButton btn btn-primary"
                        style="width: 80px;" value="Ok" />
                    <input type="button" id="btnPromotionalDiscountCancel" class="TransactionalButton btn btn-primary"
                        style="width: 80px;" value="Cancel" />
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <div id="AccessPermissionForEditDelete" style="display: none;">
        <div class="form-horizontal">
            <div class="row no-gutters">
                <div class="col-sm-12">
                    <div class="row" style="margin-bottom: 15px; margin-top: 15px;">
                        <div class="col-sm-2">
                            User Id
                        </div>
                        <div class="col-sm-10">
                            <input type="text" id="txtUserId" value="" autocomplete="off"
                                class="form-control accessvarification" placeholder="user id" />
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 15px;">
                        <div class="col-sm-2">
                            Password
                        </div>
                        <div class="col-sm-10">
                            <input type="password" id="txtUserPassword" value="" autocomplete="off"
                                class="form-control accessvarification" placeholder="password" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <div id="KeyBoardVarificationContainer">
                            </div>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 5px; margin-top: 5px;">
                        <div class="col-sm-4">
                        </div>
                        <div class="col-sm-4">
                            <input type="button" id="btnAccessVarification" value="Submit" style="height: 40px;"
                                class="col-sm-12 btn btn-primary btn-large" />
                        </div>
                        <div class="col-sm-4">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="TableInfoActionDecider" style="display: none;">
        <div class="form-horizontal">
            <div class="row no-gutters">
                <div class="col-sm-3" style="border-right: 1px solid #ccc;">
                    <div class="row" style="margin-bottom: 15px; margin-top: 15px;">
                        <div class="col-sm-12">
                            <input type="button" id="btnItemSpecialRemarks" class="TransactionalButton btn btn-primary"
                                style="width: 150px;" value="Special Remarks" />
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 15px;" id="UpdateItemNameContainer">
                        <div class="col-sm-12">
                            <input type="button" id="btnOpenItemName" onclick="UpdateOpenItemName()" class="TransactionalButton btn btn-primary"
                                style="width: 150px;" value="Name Change" />
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 15px;">
                        <div class="col-sm-12">
                            <input type="button" id="btnUpdateTableInfo" onclick="UpdateOrderInfo('QuantityChange')" class="TransactionalButton btn btn-primary"
                                style="width: 150px;" value="Edit Quantity" />
                        </div>
                    </div>
                    <div class="row" id="btnEditRecipeDiv" style="margin-bottom: 15px;">
                        <div class="col-sm-12">
                            <input type="button" id="btnEditRecipe" class="TransactionalButton btn btn-primary"
                                style="width: 150px;" value="Edit Recipe" />
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 15px;" id="UpdateItemPriceContainer">
                        <div class="col-sm-12">
                            <input type="button" id="btnUpdateItemPrice" onclick="UpdateOrderInfo('UnitPriceChange')" class="TransactionalButton btn btn-primary"
                                style="width: 150px;" value="Edit Item Price" />
                        </div>
                    </div>
                    <div id="deleteTableInfoDiv" class="row" style="margin-bottom: 15px;">
                        <div class="col-sm-12">
                            <input type="button" id="btnDeleteTableInfo" class="TransactionalButton btn btn-primary"
                                style="width: 150px;" value="Delete" />
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 15px;">
                        <div class="col-sm-12">
                            <input type="button" id="btnColseDeciderWidow" class="TransactionalButton btn btn-primary"
                                style="width: 150px;" value="Close" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-9">

                    <div id="ItemWiseSpecialRemarks" style="display: none;">
                        <div class="form-horizontal">
                            <div class="row no-gutter no-gutters">
                                <div class="col-sm-12">
                                    <div class="DataGridYScroll">
                                        <div id="remarksContainer" style="width: 100%;">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row no-gutter no-gutters">
                                <div class="col-sm-12">
                                    <input type="button" id="btnItemwiseSpecialRemarksSave" class="TransactionalButton btn btn-primary btn-large"
                                        value="Save" onclick="SpecialRemarksSave()" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="ItemWiseRecipeList" style="display: none;">
                        <div id="myTabs">
                            <ul id="tabPage" class="ui-style">
                                <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                                    href="#tab-1">Existing Ingredient</a></li>
                                <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                                    href="#tab-2">New Ingredient</a></li>
                            </ul>
                            <div id="tab-1">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <table id="tblRecipe" class="table table-bordered table-condensed table-responsive">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div id="tab-2">
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:Label ID="lblIngredientName" runat="server" class="control-label required-field" Text="Ingredient Name"></asp:Label>
                                    </div>
                                    <div class="col-md-7">
                                        <asp:DropDownList ID="ddlIngredient" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <asp:Label ID="lblIngredientQuantity" runat="server" class="control-label required-field" Text="Ingredient Quantity"></asp:Label>
                                    </div>
                                    <div class="col-md-7">
                                        <asp:DropDownList ID="ddllblIngredientQuantity" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <input type="button" id="btnAddNewRecipeItem" class="TransactionalButton btn btn-primary btn-large"
                                            value="Add" onclick="AddNewItems()" />
                                    </div>
                                </div>
                                <div class="form-group" id="NewRecipeDiv" style="display: none;">
                                    <div class="col-md-12">
                                        <table id="tblNewRecipe" class="table table-bordered table-condensed table-responsive">
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-12" style="padding-top: 5px;">
                                <input type="button" id="btnSaveWithPriceUpdate" class="TransactionalButton btn btn-primary btn-large"
                                    value="Save with price Update" onclick="SaveWithPriceUpdate()" />
                                <input type="button" id="btnSaveWithoutPriceUpdate" class="TransactionalButton btn btn-primary btn-large"
                                    value="Save without price update" onclick="SaveNewRecipe()" />
                            </div>
                        </div>
                    </div>
                    <div id="ItemWiseRemarks" style="display: none;">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <textarea class="form-control" id="txtItemWiseRemarks"></textarea>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <input type="button" id="btnItemwiseRemarksSave" class="TransactionalButton btn btn-primary btn-large"
                                        value="Save" onclick="SpecialRemarksSave()" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="OpenItemNameChangeDialog" style="display: none;">
                        <div class="form-horizontal">
                            <div class="form-group" style="margin-bottom: 15px; margin-top: 15px;">
                                <div class="col-sm-2">
                                    Name
                                </div>
                                <div class="col-sm-10">
                                    <input type="text" id="txtItemNameNew" value="" autocomplete="off"
                                        class="form-control" placeholder="item name" />
                                </div>
                            </div>
                            <div class="form-group" style="margin-bottom: 5px; margin-top: 5px;">
                                <div class="col-sm-4">
                                </div>
                                <div class="col-sm-4">
                                    <input type="button" id="btnChangeItemName" value="Submit" style="height: 40px;"
                                        class="col-sm-12 btn btn-primary btn-large" onclick="ChangeItemName()" />
                                </div>
                                <div class="col-sm-4">
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="QuantityEditDialog" style="display: none;">
                        <div class="form-horizontal">
                            <div class="row no-gutters">
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtTouchKeypadResult" runat="server" CssClass="numkbnotdecimal form-control"
                                        Height="40px" Font-Size="35px" Style="text-align: right;"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row no-gutters">
                                <div class="col-sm-12">
                                    <div id="KeyBoardContainerQuantityChange" style="margin-top: 5px;">
                                    </div>
                                </div>
                            </div>
                            <div class="row no-gutters pull-right">
                                <div class="col-sm-12">
                                    <input type="button" style="height: 40px; font-size: 1.5em; font-weight: bold; margin-top: 8px; width: 145px;"
                                        class="ui-keyboard-button ui-keyboard-48 ui-state-default ui-corner-all"
                                        value="OK" onclick="PerformUpdateActionTouchKeypad()" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </div>

    <div id="RestaurantPaymentInformationDiv" style="display: none;">
        <div class="row no-gutters" style="background-color: #fff;">
            <div class="col-sm-7" style="border: 1px solid #ccc; min-height: 88.4vh; padding-top: 5px;">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="control-label col-sm-3" id="lblTotalSales">
                            Total Sales</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtTotalSales" TabIndex="1" runat="server" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                            <asp:HiddenField ID="txtTPSalesAmountHiddenField" runat="server" />
                        </div>
                    </div>
                    <div id="discountApplicableDiv">
                        <div class="form-group">
                            <label class="control-label col-sm-3">Discount Type</label>
                            <div class="col-sm-9" id="discountTable">
                                <table class="table table-condensed table-responsive">
                                    <tbody>
                                        <tr>
                                            <td style="width: 60px;">
                                                <asp:RadioButton ID="rbTPFixedDiscount" runat="server" GroupName="DiscountType" />&nbsp;<span>Fixed</span>
                                            </td>
                                            <td style="width: 100px;">
                                                <asp:RadioButton ID="rbTPPercentageDiscount" runat="server" GroupName="DiscountType" Checked="True" />&nbsp;<span>Percentage</span>
                                            </td>
                                            <td style="width: 115px;">
                                                <asp:RadioButton ID="rbTPComplementaryDiscount" runat="server" GroupName="DiscountType" />&nbsp;<span>Complementary</span>
                                            </td>
                                            <td style="width: 120px;">
                                                <asp:RadioButton ID="rbTpNonChargeable" runat="server" GroupName="DiscountType" />&nbsp;<span>Non Chargeable</span>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3" id="lblTPDiscountAmountDisplay">
                                Discount Amount</label>
                            <div class="col-sm-9">
                                <div class="row">
                                    <div class="col-sm-4" style="padding-right: 5px;" id="discountContainer">
                                        <asp:TextBox ID="txtTPDiscountAmount" CssClass="form-control numkb quantity" TabIndex="1"
                                            runat="server" onblur="CalculateDiscountAmount()"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-5" style="padding-left: 0px; padding-right: 5px;">
                                        <asp:TextBox ID="txtTPDiscount" TabIndex="1" runat="server" Text="0" ReadOnly="True"
                                            CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-1" style="padding-left: 0px;">
                                        <img border="0" alt="Category Wise Discount" onclick="javascript:return LoadPercentageDiscountInfo()"
                                            style="cursor: pointer; display: inline;" title="Category Wise Discount" src="../Images/discount.png"
                                            id="ImgCategoryWiseDiscount" />
                                    </div>
                                    <div class="col-sm-1" style="padding-left: 5px; display: none;">
                                        <img border="0" alt="Category Wise Discount" onclick="javascript:return LoadPromotionalDiscount()"
                                            style="cursor: pointer; display: inline;" title="Promotional Discount" src="../Images/discountpromotion.png"
                                            id="Img1" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3">
                                Discounted Amount</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtTPDiscountedAmount" TabIndex="1" runat="server" ReadOnly="True"
                                    CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-3 text-right">
                            <label class="control-label">Service Charge</label>
                        </div>
                        <div class="col-sm-3">
                            <div class="input-group">
                                <asp:TextBox ID="txtTPServiceCharge" runat="server" TabIndex="22" CssClass="form-control" Enabled="false"></asp:TextBox>
                                <span class="input-group-addon">
                                    <asp:CheckBox ID="cbTPServiceCharge" runat="server" Text="" onclick="javascript: return ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge(this);"
                                        TabIndex="8" Checked="True" />
                                </span>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label id="lblTPSDCharge" class="control-label" runat="server">SD Charge</label>
                        </div>
                        <div class="col-sm-3">
                            <div class="input-group">
                                <asp:TextBox ID="txtTPSDCharge" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                <span class="input-group-addon">
                                    <asp:CheckBox ID="cbTPSDCharge" runat="server" onclick="javascript: return ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge(this);"
                                        Checked="True" />
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-3 text-right">
                            <label class="control-label">Vat Amount</label>
                        </div>
                        <div class="col-sm-3">
                            <div class="input-group">
                                <asp:TextBox ID="txtTPVatAmount" runat="server" TabIndex="23" CssClass="form-control"
                                    Enabled="false"></asp:TextBox>
                                <span class="input-group-addon">
                                    <asp:CheckBox ID="cbTPVatAmount" runat="server" onclick="javascript: return ToggleTotalSalesAmountVatServiceChargeCalculationForVat(this);" Checked="True" />
                                </span>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label id="lblAdditionalCharge" class="control-label">Additional Charge</label>
                        </div>
                        <div class="col-sm-3">
                            <div class="input-group">
                                <asp:TextBox ID="txtTPAdditionalCharge" runat="server" CssClass="form-control"
                                    Enabled="false"></asp:TextBox>

                                <span class="input-group-addon">
                                    <asp:CheckBox ID="cbTPAdditionalCharge" runat="server" onclick="javascript: return ToggleTotalSalesAmountVatServiceChargeCalculationForVat(this);" Checked="True" />
                                </span>
                            </div>
                        </div>
                    </div>


                    <div class="form-group">
                        <label class="control-label col-sm-3">
                            Cash</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtCash" TabIndex="1" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                        </div>
                    </div>
                    <%--<div class="form-group">
                        <label class="control-label col-sm-3">
                            Amex Card</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtAmexCard" TabIndex="2" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                        </div>
                    </div>--%>
                    <div class="form-group" id="AmexCardPaymentContainer">
                        <label class="control-label col-sm-3" id="lblAmexCard">
                            Amex Card</label>
                        <div class="col-sm-9">
                            <div class="row">
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtAmexCard" TabIndex="5" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <img border="0" alt="Amex Card Search" onclick="javascript:return LoadAmexCardInfo()"
                                        style="cursor: pointer; display: inline;" title="Amex Card Search" src="../Images/quotation.png" />
                                    <img border="0" id="imgClearAmexCardPayment" alt="Clear" onclick="javascript:return ClearPayment('AmexCard')"
                                        style="cursor: pointer; display: inline;" title="Clear Payment" src="../Images/clear.png" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-10">
                                    <asp:Label ID="lblAmexCardBankName" runat="server" Text="" CssClass="control-label" ClientIDMode="Static"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--<div class="form-group">
                        <label class="control-label col-sm-3" id="lblMasterCard">
                            Master Card</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtMasterCard" TabIndex="3" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                        </div>
                    </div>--%>
                    <div class="form-group" id="MasterCardPaymentContainer">
                        <label class="control-label col-sm-3" id="lblMasterCard">
                            Master Card</label>
                        <div class="col-sm-9">
                            <div class="row">
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtMasterCard" TabIndex="5" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <img border="0" alt="Master Card Search" onclick="javascript:return LoadMasterCardInfo()"
                                        style="cursor: pointer; display: inline;" title="Master Card Search" src="../Images/quotation.png" />
                                    <img border="0" id="imgClearMasterCardPayment" alt="Clear" onclick="javascript:return ClearPayment('MasterCard')"
                                        style="cursor: pointer; display: inline;" title="Clear Payment" src="../Images/clear.png" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-10">
                                    <asp:Label ID="lblMasterCardBankName" runat="server" Text="" CssClass="control-label" ClientIDMode="Static"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--<div class="form-group">
                        <label class="control-label col-sm-3" id="lblVisaCard">
                            Visa Card</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtVisaCard" TabIndex="4" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                        </div>
                    </div>--%>
                    <div class="form-group" id="VisaCardPaymentContainer">
                        <label class="control-label col-sm-3" id="lblVisaCard">
                            Visa Card</label>
                        <div class="col-sm-9">
                            <div class="row">
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtVisaCard" TabIndex="5" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <img border="0" alt="Visa Card Search" onclick="javascript:return LoadVisaCardInfo()"
                                        style="cursor: pointer; display: inline;" title="Visa Card Search" src="../Images/quotation.png" />
                                    <img border="0" id="imgClearVisaCardPayment" alt="Clear" onclick="javascript:return ClearPayment('VisaCard')"
                                        style="cursor: pointer; display: inline;" title="Clear Payment" src="../Images/clear.png" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-10">
                                    <asp:Label ID="lblVisaCardBankName" runat="server" Text="" CssClass="control-label" ClientIDMode="Static"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--<div class="form-group">
                        <label class="control-label col-sm-3" id="lblDiscoverCard">
                            Discover Card</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtDiscoverCard" TabIndex="4" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                        </div>
                    </div>--%>
                    <div class="form-group" id="DiscoverCardPaymentContainer">
                        <label class="control-label col-sm-3" id="lblDiscoverCard">
                            Discover Card</label>
                        <div class="col-sm-9">
                            <div class="row">
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtDiscoverCard" TabIndex="5" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <img border="0" alt="Discover Card Search" onclick="javascript:return LoadDiscoverCardInfo()"
                                        style="cursor: pointer; display: inline;" title="M-Banking Search" src="../Images/quotation.png" />
                                    <img border="0" id="imgClearDiscoverCardPayment" alt="Clear" onclick="javascript:return ClearPayment('DiscoverCard')"
                                        style="cursor: pointer; display: inline;" title="Clear Payment" src="../Images/clear.png" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-10">
                                    <asp:Label ID="lblDiscoverCardBankName" runat="server" Text="" CssClass="control-label" ClientIDMode="Static"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" id="MBankingPaymentContainer">
                        <label class="control-label col-sm-3" id="lblMBanking">
                            M-Banking</label>
                        <div class="col-sm-9">
                            <div class="row">
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtMBankingPayment" TabIndex="5" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <img border="0" alt="M-Banking Search" onclick="javascript:return LoadMBankingInfo()"
                                        style="cursor: pointer; display: inline;" title="M-Banking Search" src="../Images/quotation.png" />
                                    <img border="0" id="imgClearMBankingPayment" alt="Clear" onclick="javascript:return ClearPayment('M-Banking')"
                                        style="cursor: pointer; display: inline;" title="Clear Payment" src="../Images/clear.png" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-10">
                                    <asp:Label ID="lblMBankName" runat="server" Text="" CssClass="control-label" ClientIDMode="Static"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" id="RoomPaymentDiv">
                        <label class="control-label col-sm-3" id="lblGuestRoom">
                            Room Payment</label>
                        <div class="col-sm-9">
                            <div class="row">
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtRoomPayment" TabIndex="5" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4 no-gutter-column">
                                    <img border="0" alt="Room Search" onclick="javascript:return SearchRoom()"
                                        style="cursor: pointer; display: inline;" title="Room Search" src="../Images/roomicon.png" />
                                    <img border="0" id="imgRoomWiseGuest" alt="Guest Search" onclick="javascript:return LoadRoomWiseGuestInfo()"
                                        style="cursor: pointer; display: inline;" title="Guest Search" src="../Images/guestlist.png" />
                                    <img border="0" id="imgClearRoomWiseGuest" alt="Clear" onclick="javascript:return ClearPayment('Guest')"
                                        style="cursor: pointer; display: inline;" title="Clear Payment" src="../Images/clear.png" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" id="CompanyPaymentContainer">
                        <label class="control-label col-sm-3" id="lblCompany">
                            Company</label>
                        <div class="col-sm-9">
                            <div class="row">
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtCompanyPayment" TabIndex="5" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <img border="0" alt="Company Search" onclick="javascript:return LoadCompanyInfo()"
                                        style="cursor: pointer; display: inline;" title="Company Search" src="../Images/company.png" />
                                    <img border="0" id="imgClearCompanyPayment" alt="Clear" onclick="javascript:return ClearPayment('Company')"
                                        style="cursor: pointer; display: inline;" title="Clear Payment" src="../Images/clear.png" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-10">
                                    <asp:Label ID="lblCompanyName" runat="server" Text="" CssClass="control-label" ClientIDMode="Static"></asp:Label>

                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group" id="EmployeePaymentContainer">
                        <label class="control-label col-sm-3" id="lblEmployee">
                            Employee</label>
                        <div class="col-sm-9">
                            <div class="row">
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtEmployeePayment" TabIndex="5" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">

                                    <img border="0" alt="Company Search" onclick="javascript:return LoadEmployeeInfo()"
                                        style="cursor: pointer; display: inline;" title="Employee Search" src="../Images/employee.png" />

                                    <img border="0" id="imgClearEmployeePayment" alt="Clear" onclick="javascript:return ClearPayment('Employee')"
                                        style="cursor: pointer; display: inline;" title="Clear Payment" src="../Images/clear.png" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-10">
                                    <asp:Label ID="lblEmployeeName" runat="server" Text="" CssClass="control-label" ClientIDMode="Static"></asp:Label>

                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group" id="MemberPaymentContainer">
                        <label class="control-label col-sm-3" id="lblMember">
                            Member</label>
                        <div class="col-sm-9">
                            <div class="row">
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtMemberPayment" TabIndex="5" runat="server" CssClass="quantitydecimal numkb form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">

                                    <img border="0" alt="Member Search" onclick="javascript:return LoadMemberInfo()"
                                        style="cursor: pointer; display: inline;" title="Member Search" src="../Images/member.png" />

                                    <img border="0" id="imgClearMemberPayment" alt="Clear" onclick="javascript:return ClearPayment('Member')"
                                        style="cursor: pointer; display: inline;" title="Clear Payment" src="../Images/clear.png" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="lblMemberName" runat="server" Text="" CssClass="control-label" ClientIDMode="Static"></asp:Label>

                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group" style="display: none;">
                        <label class="control-label col-sm-3" id="lblTPRoundedAmount">
                            Rounded Amount</label>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtTPRoundedAmount" TabIndex="5" runat="server" CssClass="quantitydecimal form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" style="display: none;">
                        <label class="control-label col-sm-3" id="lblPOSTerminal">
                            POS Terminal</label>
                        <div class="col-sm-9">
                            <asp:Literal ID="ltlPOSTerminalInformation" runat="server"> </asp:Literal>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-5" style="border: 1px solid #ccc; min-height: 86vh; padding: 2px;">
                <div class="row">
                    <div class="col-sm-12">
                        <div id="KeyBoardContainer">
                        </div>
                    </div>
                </div>
                <div class="form-horizontal" style="padding-top: 5px;">
                    <div class="form-group">
                        <label class="control-label col-sm-4">
                            Grand Total</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtTPGrandTotal" TabIndex="5" runat="server" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-4">
                            Total Payment</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtTPTotalPaymentAmount" TabIndex="5" runat="server" ReadOnly="True"
                                CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblTPChangeAmount" CssClass="control-label col-sm-4" runat="server"
                            Text="Change Amount"></asp:Label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtTPChangeAmount" TabIndex="5" runat="server" CssClass="form-control"
                                ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label18" CssClass="control-label col-sm-4" runat="server"
                            Text="Remarks"></asp:Label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group pull-right">
                        <div class="col-sm-12">
                            <input type="button" class="ui-keyboard-button ui-keyboard-48 ui-state-default ui-corner-all"
                                style="width: 90px; height: 50px; font-size: 1.5em;" value="OK" onclick='javascript: return PerformTPOkButton()' />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row no-gutters no-gutter">
        <div class="col-sm-3" style="">
            <div style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger EventName="Click" ControlID="btnLoadItemCategory" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Button ID="btnLoadItemCategory" runat="server" Text="Button" OnClick="btnLoadItemCategory_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="row">
                <div class="col-sm-11">
                    <div id="ltlMenuInformation" style="min-height: 81.5vh;">
                        <div style="height: 81.5vh; overflow-y: scroll;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger EventName="SelectedNodeChanged" ControlID="tvLocations" />
                                    <asp:AsyncPostBackTrigger EventName="Click" ControlID="btnLoadItemCategory" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:TreeView ID="tvLocations" runat="server" class="MenuItemTouchTree" ShowLines="true"
                                        NodeWrap="False" NodeIndent="12" NodeStyle-NodeSpacing="4" ImageSet="Custom"
                                        CollapseImageUrl="~/Images/toggleCollapse.gif" ExpandImageUrl="/Images/toggleExpand.gif"
                                        OnSelectedNodeChanged="tvLocations_SelectedNodeChanged">
                                        <RootNodeStyle CssClass="rootNodeTouch" />
                                        <ParentNodeStyle CssClass="parentNodeTouch" />
                                        <NodeStyle CssClass="treeNodeTouch" />
                                        <LeafNodeStyle CssClass="leafNodeTouch" />
                                        <HoverNodeStyle CssClass="hoverNodeTouch" />
                                        <SelectedNodeStyle CssClass="selectNodeTouch" />
                                        <Nodes>
                                            <asp:TreeNode></asp:TreeNode>
                                        </Nodes>
                                    </asp:TreeView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-5">
            <div class="row" id="ItemSearchContainer">
                <div class="col-sm-9" style="margin-bottom: 5px; padding-right: 0px;">
                    <input type="text" id="txtItemName" class="form-control" onkeydown="if (event.keyCode == 13) {return false;}" placeholder="Item Search ..." style="height: 30px;" />
                </div>
                <div class="col-sm-3" style="margin-bottom: 5px; padding-left: 0px;">
                    <input type="text" id="txtItemQuantity" class="quantitydecimal form-control" onfocus="this.select();" onkeydown="if (event.keyCode == 13) {SearchItemAdded(); return false;}" placeholder="Quantity" style="height: 30px;" />
                </div>
            </div>

            <div class="row">
                <div class="col-sm-11">
                    <div id="CategoryItemInformationDiv" style="min-height: 76vh; border: 1px solid #ccc; overflow: hidden;">
                        <div style="height: 76vh; overflow-y: scroll;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger EventName="SelectedNodeChanged" ControlID="tvLocations" />
                                    <asp:AsyncPostBackTrigger EventName="Click" ControlID="btnLoadItemCategory" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Literal ID="literalRestaurantTemplate" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div class="col-sm-4" style="">
            <div class="row no-gutters no-gutter">
                <div class="col-sm-12">
                    <div id="ltlTableWiseItemInformation" style="overflow: scroll;">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-horizontal">
                        <div style="display: none;">
                            <div class="form-group">
                                <label id="ContentPlaceHolder1_lblSalesAmount" class="control-label col-sm-4">
                                    Sales Amount</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtSalesAmount" TabIndex="4" runat="server" Enabled="False" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="ContentPlaceHolder1_lblDiscountedAmount" class="control-label col-sm-4">
                                    After Discount</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtDiscountedAmount" TabIndex="4" runat="server" Enabled="False"
                                        CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" style="margin-top: 3px;">
                            <label id="ContentPlaceHolder1_lblServiceCharge" class="control-label col-sm-4">
                                Service Charge</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtServiceCharge" runat="server" TabIndex="22" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="margin-top: 3px;">
                            <label id="ContentPlaceHolder1_lblCitySDCharge" class="control-label col-sm-4">
                                SD Charge</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtCitySDCharge" runat="server" TabIndex="22" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label id="ContentPlaceHolder1_lblVatAmount" class="control-label col-sm-4">
                                Vat Amount</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="23" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="margin-top: 3px;">
                            <label id="ContentPlaceHolder1_lblAdditionalCharge" class="control-label col-sm-4">
                                Additional Charge</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtAdditionalCharge" runat="server" TabIndex="22" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label id="ContentPlaceHolder1_lblGrandTotal" class="control-label col-sm-4">
                                Grand Total</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtGrandTotal" TabIndex="4" runat="server" Enabled="False" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div style="display: none;">
                            <div class="form-group">
                                <label class="control-label col-sm-4">
                                    Total Payment</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtTotalPaymentAmount" TabIndex="5" runat="server" Enabled="False"
                                        CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4">
                                    Rounded Amount</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtRoundedAmount" TabIndex="5" runat="server" Enabled="False" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label id="ContentPlaceHolder1_lblTotalChangeAmount" class="control-label col-sm-4">
                                Due Amount</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtChangeAmount" TabIndex="5" runat="server" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row pull-right" style="margin-top: 10px;">
        <div class="col-sm-12" style="margin-top: 3px; margin-bottom: 3px;">
            <input type="button" id="btnBackCostCenterSelection" class="TransactionalButton btn btn-primary"
                style="padding: 8px 5px; width: 155px;" value="Back To Cost Center" onclick="BackCostCenterSelection()" />
            <input type="button" id="btnClear" class="TransactionalButton btn btn-primary"
                style="padding: 8px 5px; width: 90px;" value="Clear" onclick="ClearOrderedItem()" />
            <input type="button" id="btnTableOrderPriview" class="TransactionalButton btn btn-primary tblOrder"
                style="padding: 8px 5px; width: 90px;" value="Kot Preview" onclick="OrderPreview()" />
            <input type="button" id="btnOrderSubmit" class="TransactionalButton btn btn-primary"
                style="padding: 8px 5px; width: 100px;" value="Order Submit" onclick="OrderSubmit()" />
            <input type="button" id="Button7" class="TransactionalButton btn btn-primary" style="padding: 8px 5px; width: 90px;"
                value="Pax Change" onclick="PaxChange()" />
            <input type="button" id="btnWaiterChange" class="TransactionalButton btn btn-primary" style="padding: 8px 5px; width: 110px;"
                value="Waiter Change" onclick="WaiterChange()" />
            <input type="button" id="btnAddMoreTable" class="TransactionalButton btn btn-primary tblOrder"
                style="padding: 8px 5px; width: 130px;" value="Add More Table" onclick="AddMoreTable()" />
            <input type="button" id="btnBillSplit" class="TransactionalButton btn btn-primary"
                style="padding: 8px 5px; width: 90px" value="Bill Split" onclick="BillSplit()" />
            <input type="button" id="btnBillPreview" class="TransactionalButton btn btn-primary"
                style="padding: 8px 5px; width: 90px" value="Bill Preview" onclick="BillPreview()" />
            <input type="button" id="btnBillHolpup" class="TransactionalButton btn btn-primary tknOrder"
                style="padding: 8px 5px; width: 90px;" value="Holdup" onclick="ValidateFormBeforeSettlement('holdup')" />
            <input type="button" id="btnPaymentInfo" runat="server" onclick="LoadPaymentInformation()"
                style="padding: 8px 5px; width: 90px;" class="btn btn-primary" value="Payment" />
            <span id="BillSettlementBtnContainer">
                <input type="button" id="btnSave" runat="server" onclick="ValidateFormBeforeSettlement('settlement')"
                    style="padding: 8px 5px; width: 90px;" class="btn btn-primary" value="Settlement" />
            </span>
        </div>
    </div>
    <div id="TActionDeciderForToken" style="display: none;">
        <div class="row no-gutters">
            <div class="col-sm-6">
                <div class="row" style="margin-bottom: 15px;" id="billPreviewForBill">
                    <div class="col-sm-6">
                    </div>
                </div>
                <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-6">
                    </div>
                </div>
                <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-6">
                    </div>
                </div>
                <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-6">
                    </div>
                </div>
                <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-6">
                    </div>
                </div>
                <div class="row" style="margin-bottom: 15px;">
                    <div class="col-sm-6">
                        <input type="button" id="btnCloseClearActionDecider" class="TransactionalButton btn btn-primary"
                            style="width: 150px; height: 45px;" value="Close" onclick="CloseClearActionDecider()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="TActionDeciderForTable" style="display: none;">
        <div class="row-fluid">
            <div class="span12">
                <div class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                        <input type="button" id="btnTableOrderSubmit" class="TransactionalButton btn btn-primary"
                            style="width: 150px; height: 45px;" value="Order Submit" onclick="OrderSubmit()" />
                    </div>
                </div>
                <div class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                    </div>
                </div>
                <div class="row-fluid" style="margin-bottom: 15px;" id="Div7">
                    <div class="span6">
                        <input type="button" id="btnTableBillPreview" class="TransactionalButton btn btn-primary"
                            style="width: 150px; height: 45px;" value="Bill Preview" onclick="BillPreview()" />
                    </div>
                </div>
                <div class="row-fluid" style="margin-bottom: 15px;" id="Div6">
                    <div class="span6">
                    </div>
                </div>
                <div class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                        <input type="button" id="btnBackCostCenterForTable" class="TransactionalButton btn btn-primary"
                            style="width: 150px; height: 45px;" value="Back To Cost Center" onclick="BackCostCenterSelection()" />
                    </div>
                </div>
                <div class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                        <input type="button" id="Button6" class="TransactionalButton btn btn-primary" style="width: 150px; height: 45px;"
                            value="Pax Change" onclick="PaxChange()" />
                    </div>
                </div>
                <div class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                        <input type="button" id="Button5" class="TransactionalButton btn btn-primary" style="width: 150px; height: 45px;"
                            value="Close" onclick="CloseClearActionDecider()" />
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <div id="OccupiedTableDialog" style="display: none;">
        <div class="form-horizontal">
            <div class="row">
                <div class="col-sm-12">
                    <div style="height: 350px; overflow-y: scroll;">
                        <div id="OccupiedTableContainer">
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="margin-top: 3px;">
                <div class="col-sm-12">
                    <button type="button" onclick="javascript:return GetAndApplySelectedTable()" style="padding: 8px 5px; width: 90px;" id="btnAddCheckedTable" class="TransactionalButton btn btn-primary">OK</button>
                    <button type="button" onclick="AddMoreTableDialogClose()" style="padding: 8px 5px; width: 90px;" id="btnAddRoomId" class="TransactionalButton btn btn-primary">Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <div id="AmexCardInfoDialog" style="display: none;">
        <div class="row no-gutters">
            <div class="col-sm-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label20" runat="server" Text="Bank Name" CssClass="control-label col-sm-3"></asp:Label>
                        <div class="col-sm-9">
                            <input type="text" class="form-control" id="txtSearchAmexCard" />
                        </div>
                    </div>
                    <div class="row pull-right">
                        <div class="col-sm-12">
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnClearAmexCardInformationSubmit" value="Cancel" onclick="ClearPayment('AmexCard')" />
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnAmexCardInformationSubmit" value="Ok" onclick="AmexCardPayment()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="MasterCardInfoDialog" style="display: none;">
        <div class="row no-gutters">
            <div class="col-sm-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label21" runat="server" Text="Bank Name" CssClass="control-label col-sm-3"></asp:Label>
                        <div class="col-sm-9">
                            <input type="text" class="form-control" id="txtSearchMasterCard" />
                        </div>
                    </div>
                    <div class="row pull-right">
                        <div class="col-sm-12">
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnClearMasterCardInformationSubmit" value="Cancel" onclick="ClearPayment('MasterCard')" />
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnMasterCardInformationSubmit" value="Ok" onclick="MasterCardPayment()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="VisaCardInfoDialog" style="display: none;">
        <div class="row no-gutters">
            <div class="col-sm-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label22" runat="server" Text="Bank Name" CssClass="control-label col-sm-3"></asp:Label>
                        <div class="col-sm-9">
                            <input type="text" class="form-control" id="txtSearchVisaCard" />
                        </div>
                    </div>
                    <div class="row pull-right">
                        <div class="col-sm-12">
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnClearVisaCardInformationSubmit" value="Cancel" onclick="ClearPayment('VisaCard')" />
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnVisaCardInformationSubmit" value="Ok" onclick="VisaCardPayment()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="DiscoverCardInfoDialog" style="display: none;">
        <div class="row no-gutters">
            <div class="col-sm-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label23" runat="server" Text="Bank Name" CssClass="control-label col-sm-3"></asp:Label>
                        <div class="col-sm-9">
                            <input type="text" class="form-control" id="txtSearchDiscoverCard" />
                        </div>
                    </div>
                    <div class="row pull-right">
                        <div class="col-sm-12">
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnClearDiscoverCardInformationSubmit" value="Cancel" onclick="ClearPayment('DiscoverCard')" />
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnDiscoverCardInformationSubmit" value="Ok" onclick="DiscoverCardPayment()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="MBankingInfoDialog" style="display: none;">
        <div class="row no-gutters">
            <div class="col-sm-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label19" runat="server" Text="Bank Name" CssClass="control-label col-sm-3"></asp:Label>
                        <div class="col-sm-9">
                            <input type="text" class="form-control" id="txtSearchMBanking" />
                        </div>
                    </div>
                    <div class="row pull-right">
                        <div class="col-sm-12">
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnClearMBankingInformationSubmit" value="Cancel" onclick="ClearPayment('M-Banking')" />
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnMBankingInformationSubmit" value="Ok" onclick="MBankingPayment()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="CompanyInfoDialog" style="display: none;">
        <div class="row no-gutters">
            <div class="col-sm-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label13" runat="server" Text="Company Name" CssClass="control-label col-sm-3"></asp:Label>
                        <div class="col-sm-9">
                            <input type="text" class="form-control" id="txtSearchCompany" />
                        </div>
                    </div>
                    <fieldset>
                        <legend>Company Details</legend>
                        <div class="form-group">
                            <asp:Label ID="Label14" runat="server" Text="Company Address" CssClass="control-label col-sm-3"></asp:Label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" id="txtCompanyAddress" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label15" runat="server" Text="Contact Number" CssClass="control-label col-sm-3"></asp:Label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" id="txtContactNumber" />
                            </div>
                        </div>
                        <div class="form-group" style="display: none;">
                            <asp:Label ID="Label1511" runat="server" Text="Balance" CssClass="control-label col-sm-3"></asp:Label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" id="txtBalance" />
                            </div>
                        </div>
                    </fieldset>
                    <div class="row pull-right">
                        <div class="col-sm-12">
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnClearCompanyInformationSubmit" value="Cancel" onclick="ClearPayment('Company')" />
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnCompanyInformationSubmit" value="Ok" onclick="CompanyPayment()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="EmployeeInfoDialog" style="display: none;">
        <div class="row no-gutters">
            <div class="col-sm-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label3" runat="server" Text="Employee Code" CssClass="control-label col-sm-3"></asp:Label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="txtEmployeeCode" />
                        </div>
                        <div class="col-sm-2">
                            <input type="button" class="btn btn-primary btn-small" value="Search" onclick="EmployeeSearchByCode()" />
                        </div>
                    </div>
                    <div class="form-group" style="margin-bottom: 10px;">
                        <asp:Label ID="Label12" runat="server" Text="Employee Name" CssClass="control-label col-sm-3"></asp:Label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="txtEmployeeNameSearch" />
                        </div>
                    </div>

                    <fieldset>
                        <legend>Employee Details</legend>

                        <div class="form-group">
                            <asp:Label ID="Label11" runat="server" Text="Employee Name" CssClass="control-label col-sm-3"></asp:Label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" id="txtEmployeeName" />
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Department" CssClass="control-label col-sm-3"></asp:Label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" id="txtDepartment" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Designation" CssClass="control-label col-sm-3"></asp:Label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" id="txtDesignation" />
                            </div>
                        </div>
                    </fieldset>
                    <div class="row pull-right">
                        <div class="col-sm-12">
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnClearEmployeeInformationSubmit" value="Cancel" onclick="ClearPayment('Employee')" />
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnEmployeeInformationSubmit" value="Ok" onclick="EmployeePayment()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="MemberInfoDialog" style="display: none;">
        <div class="row no-gutters">
            <div class="col-sm-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label ID="Label6" runat="server" Text="Member Number" CssClass="control-label col-sm-3"></asp:Label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="txtMemberNumber" />
                        </div>
                        <div class="col-sm-2">
                            <input type="button" class="btn btn-primary" value="Search" onclick="MemberSearchByCode()" />
                        </div>
                    </div>
                    <div class="form-group" style="margin-bottom: 15px;">
                        <asp:Label ID="Label17" runat="server" Text="Member Name" CssClass="control-label col-sm-3"></asp:Label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="txtMemberNameSearch" />
                        </div>
                    </div>
                    <fieldset>
                        <legend>Member Details</legend>

                        <div class="form-group">
                            <asp:Label ID="Label16" runat="server" Text="Member Name" CssClass="control-label col-sm-3"></asp:Label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" id="txtMemberName" />
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label7" runat="server" Text="Member Type" CssClass="control-label col-sm-3"></asp:Label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" id="txtMemberType" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label8" runat="server" Text="Member Address" CssClass="control-label col-sm-3"></asp:Label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" id="txtMemberAddress" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label9" runat="server" Text="Contact Number" CssClass="control-label col-sm-3"></asp:Label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" id="txtMemberContactNumber" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label10" runat="server" Text="Balance" CssClass="control-label col-sm-3"></asp:Label>
                            <div class="col-sm-9">
                                <input type="text" class="form-control" id="txtMemberBalance" />
                            </div>
                        </div>
                    </fieldset>
                    <div class="row pull-right">
                        <div class="col-sm-12">
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnClearMemberInformationSubmit" value="Cancel" onclick="ClearPayment('Member')" />
                            <input type="button" class="btn btn-primary btn-large" style="width: 145px; height: 40px;"
                                id="btnMemberInformationSubmit" value="Ok" onclick="MemberPayment()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="GuestDialog" style="width: 450px; display: none;">
        <div class="">
            <div class="DataGridYScroll">
                <div id="GuestContainer" runat="server" clientidmode="Static" style="width: 100%;">
                </div>
            </div>
            <div id="Div333" style="padding-left: 5px; width: 100%; margin-top: 5px;">
                <div style="float: left; padding-bottom: 15px;">
                    <input type="button" id="btnGuestSelectOK" class="TransactionalButton btn btn-primary"
                        style="width: 80px;" value="Ok" />
                    <input type="button" id="btnGuestSelectCancel" class="TransactionalButton btn btn-primary"
                        style="width: 80px;" value="Cancel" />
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>

    <div id="PaxChangeTouchKeypad" style="display: none;">
        <div class="form-horizontal">
            <div class="row no-gutters">
                <div class="col-sm-12">
                    <asp:TextBox ID="txtPax" runat="server" CssClass="numkbnotdecimalpax form-control quantity"
                        Height="40px" Font-Size="35px" Style="text-align: right;"></asp:TextBox>
                </div>
            </div>
            <div class="row no-gutters">
                <div class="col-sm-12">
                    <div id="KeyBoardContainerPaxChange" style="margin-top: 5px;">
                    </div>
                </div>
            </div>
            <div class="row no-gutters pull-right">
                <div class="col-sm-12">
                    <input type="button" style="height: 40px; font-size: 1.5em; font-weight: bold; margin-top: 8px; width: 145px;"
                        class="ui-keyboard-button ui-keyboard-48 ui-state-default ui-corner-all"
                        value="OK" onclick="PaxChangeApply()" />
                </div>
            </div>
        </div>
    </div>
    <div id="BillSplitDialog" style="display: none;">
        <div class="form-horizontal">
            <div style="padding-bottom: 20px;">
                <div class="row no-gutters">
                    <div class="col-sm-5">
                        <asp:ListBox ID="lstvBillSplitLeft" runat="server" SelectionMode="Multiple" Width="100%"
                            Height="380px"></asp:ListBox>
                        <div style="margin-top: 5px;">
                            <input type="button" id="btnLeftBillSplitPrintPreview" value="Print Preview" class="btn btn-primary" />
                        </div>
                    </div>
                    <div class="col-sm-2">
                        <div style="padding-top: 150px;">
                            <input type="button" id="btnBillTransferRight" value=">>" style="width: 95px;" class="btn btn-primary" />
                        </div>
                        <div style="padding-top: 2px;">
                            <input type="button" id="btnBillTransferLeft" value="<<" style="width: 95px;" class="btn btn-primary" />
                        </div>
                    </div>
                    <div class="col-sm-5">
                        <asp:ListBox ID="lstvBillSplitRight" runat="server" SelectionMode="Multiple" Width="100%"
                            Height="380px"></asp:ListBox>
                        <div style="margin-top: 5px;">
                            <input type="button" id="btnRightBillSplitPrintPreview" value="Print Preview" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="WaiterChangeDialog" style="display: none;" class="ui-front">
        <div class="form-horizontal">
            <div class="row no-gutters">
                <div class="col-sm-12">
                    <asp:TextBox ID="txtWaiter" runat="server" CssClass="form-control"
                        Height="30px" Font-Size="15px"></asp:TextBox>
                </div>
            </div>
            <%-- <div class="row no-gutters">
                <div class="col-sm-12">
                    <div id="KeyBoardContainerWaiterChange" style="margin-top: 5px;">
                    </div>
                </div>
            </div>--%>
            <div class="row no-gutters pull-right">
                <div class="col-sm-12">
                    <input type="button" style="height: 40px; font-size: 1.5em; font-weight: bold; margin-top: 8px; width: 250px;"
                        class="ui-keyboard-button ui-keyboard-48 ui-state-default ui-corner-all"
                        value="Confirm Waiter Change" onclick="WaiterChangeApply()" />
                </div>
            </div>
        </div>
    </div>

    <div id="RoomInfoDialog" style="display: none; height: 550px;">

        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-10">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <label class="control-label" style="padding-top: 0px;">Room Number</label></span>
                        <input type="text" id="txtRoomNumber" class="form-control" />
                    </div>
                </div>
                <div class="col-sm-2">
                    <input type="button" value="Search" class="btn btn-primary" onclick="SearchRoomInfo()" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <div class="form-group">
                    <div id="myTabsPopUp" style="padding: 2px; margin-bottom: 10px;" class="ui-style">
                        <ul id="tabPagePopUp" class="ui-style">
                            <li id="Li1" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                                <a href="#tabGuestInfo">Guest Info</a></li>
                            <li id="Li2" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                                <a href="#tabSpecialInst">Special Instruction</a></li>
                            <%--<li id="Li3" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                            <a href="#tabPreferance">Guest Preferance</a></li>--%>
                        </ul>
                        <div id="tabGuestInfo">
                            <div class="form-horizontal">
                                <div class="form-group roomIsRegistered" style="display: none; margin: 2px;">
                                    <div class="col-sm-12" id="roomInfoContainer">
                                        <table class="table table-striped table-bordered table-condensed table-hover">
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label ID="Label1" runat="server" Text="Room Number"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblRoomNumber" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label ID="Label2" runat="server" Text="Room Type"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblRoomType" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label ID="lblLArrivalDate" runat="server" Text="Arrival Date"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblDArrivalDate"
                                                        runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label ID="lblLExpectedDepartureDate"
                                                        runat="server" Text="Expected Departure Date"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblDExpectedDepartureDate" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label
                                                        ID="lblLGuestName" runat="server" Text="Name"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblDGuestName" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label ID="lblLCountryName" runat="server"
                                                        Text="Country Name"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblDCountryName"
                                                        runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label ID="lblLGuestSex" runat="server"
                                                        Text="Gender"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblDGuestSex"
                                                        runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label ID="lblLGuestDOB" runat="server" Text="Date of Birth"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblDGuestDOB" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label ID="lblLGuestEmail"
                                                        runat="server" Text="Email"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblDGuestEmail"
                                                        runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label
                                                        ID="lblLGuestPhone" runat="server" Text="Phone Number"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblDGuestPhone" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label ID="lblLGuestAddress2" runat="server"
                                                        Text="Address"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblDGuestAddress2"
                                                        runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="col-sm-2">
                                                    <asp:Label ID="lblLGuestNationality" runat="server" Text="Nationality"></asp:Label>
                                                </td>
                                                <td class="col-sm-4">
                                                    <asp:Label ID="lblDGuestNationality" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="tabSpecialInst">
                            <div class="form-horizontal">
                                <div class="form-group roomIsRegistered" style="display: none; margin: 2px;">
                                    <div class="col-sm-12" id="roomInfoContainer2">
                                        <table class="">
                                            <tr>
                                                <td class="col-sm-12">
                                                    <asp:Label ID="lblDPOSRemarks"
                                                        runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-sm-2">
                            <input type="button" id="btnRoomSearchOk" style="height: 30px; width: 80px" value="Ok" class="btn btn-primary" onclick="RoomSearchComplete()" />
                        </div>
                        <div class="col-sm-2">
                            <input type="button" style="height: 30px; width: 80px" id="btnRoomSearchCancel" value="Cancel" class="btn btn-primary" onclick="RoomSearchCancel()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <iframe id="frmPrint" name="IframeReportPrint" width="0" height="0" runat="server"
        style="left: -1000; top: 2000;" clientidmode="static" scrolling="yes"></iframe>
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {
                var barControlId = CommonHelper.GetReportViewerControlId($("#ContentPlaceHolder1_rvTransactionShow"));
                var printTemplate = $("#ContentPlaceHolder1_hfBillTemplate").val();

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 25px; width: 50px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(' + printTemplate + '); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }

            $("#ltlTableWiseItemInformation").height($("#ContentPlaceHolder1_hfltlTableWiseItemInformationDivHeight").val());
        });

        function PrintDocumentFunc(printTemplate) {
            if (printTemplate == "1") {
                $('#btnPrintPreview').trigger('click');
            }
            else if (printTemplate == "2") {
                //$('#btnPrintReportTemplate2').trigger('click');
            }
            else if (printTemplate == "3") {
                //$("#btnPrintReportTemplate3").trigger('click');
            }
            return true;
        }

        function SearchRoom() {

            if ($("#ContentPlaceHolder1_hfIsRoomIsSelectForPayment").val() != "1") {
                $("#txtRoomNumber").val("");
                $("#ContentPlaceHolder1_hfIsRoomIsSelectForPayment").val("");
                ClearRoomDetails();
            }

            $("#RoomInfoDialog").dialog({
                width: 650,
                height: 600,
                autoOpen: true,
                modal: true,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Room Info",
                show: 'slide',
                open: function (event, ui) {
                    $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                }
            });
        }

        function SearchRoomInfo() {

            var roomNumber = $.trim($("#txtRoomNumber").val());
            var costcenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();

            if (roomNumber == "") { toastr.warning("Please Give Room Number."); return false; }

            CommonHelper.SpinnerOpen();

            PageMethods.GetRegistrationInformationForSingleGuestByRoomNumber(costcenterId, roomNumber, OnGetRegistrationInformationForSingleGuestByRoomNumberSucceeded, OnGetRegistrationInformationForSingleGuestByRoomNumberFailed);

            return false;
        }

        function OnGetRegistrationInformationForSingleGuestByRoomNumberSucceeded(result) {

            ClearRoomDetails();

            if (result.RoomId == 0) {
                CommonHelper.SpinnerClose();
                toastr.info("Please Give Valid Room Number.");
                return false;
            }
            else if (result.IsStopChargePosting) {
                CommonHelper.SpinnerClose();
                toastr.info("Stop-Charge posting for this room. Please contact with Admin.");
                return false;
            }
            if (result.RoomId != 0) {
                $(".roomIsRegistered").show();
            }

            $("#ContentPlaceHolder1_hfRoomId").val(result.RoomId);
            $("#ContentPlaceHolder1_hfRegistrationId").val(result.RegistrationId);

            $("#<%=lblRoomNumber.ClientID %>").text(result.RoomNumber);
            $("#<%=lblRoomType.ClientID %>").text(result.RoomType);

            $("#<%=lblDGuestName.ClientID %>").text(result.GuestName);
            $("#<%=lblDGuestSex.ClientID %>").text(result.GuestSex);
            $("#<%=lblDGuestEmail.ClientID %>").text(result.GuestEmail);
            $("#<%=lblDGuestPhone.ClientID %>").text(result.GuestPhone);
            $("#<%=lblDGuestAddress2.ClientID %>").text(result.GuestAddress2);
            $("#<%=lblDGuestNationality.ClientID %>").text(result.GuestNationality);
            $("#<%=lblDCountryName.ClientID %>").text(result.CountryName);
            $("#<%=lblDPOSRemarks.ClientID %>").text(result.POSRemarks);

            if (result.IsStopChargePosting) {
                $("#RoomPaymentDiv").hide();
            }
            else {
                $("#RoomPaymentDiv").show();
            }

            $("#myTabsPopUp").tabs({ active: 0 });

            var sourceType = $.trim(CommonHelper.GetParameterByName("st"));
            if (sourceType == "tkn") {
                $("#imgRoomWiseGuest").show();
            }
            else if (sourceType == "tbl") {
                $("#imgRoomWiseGuest").show();

                if ($("#ContentPlaceHolder1_hfRoomNumber").val() != "") {
                    $("#lblGuestRoom").text("Room# " + $("#ContentPlaceHolder1_hfRoomNumber").val());
                }
            }

            //$("#txtRoomNumber").text("");
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnGetRegistrationInformationForSingleGuestByRoomNumberFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function ClearRoomDetails() {

            $("#<%=lblDGuestName.ClientID %>").text("");
            $("#<%=lblDGuestSex.ClientID %>").text("");
            $("#<%=lblDGuestEmail.ClientID %>").text("");
            $("#<%=lblDGuestPhone.ClientID %>").text("");
            $("#<%=lblDGuestAddress2.ClientID %>").text("");
            $("#<%=lblDGuestNationality.ClientID %>").text("");
            $("#ContentPlaceHolder1_lblDCountryName").text("");

            $("#ContentPlaceHolder1_lblRoomNumber").text("");
            $("#ContentPlaceHolder1_lblRoomType").text("");
            $("#<%=lblDPOSRemarks.ClientID %>").text("");
        }

        function RoomSearchComplete() {
            $("#RoomInfoDialog").dialog("close");
            if ($("#ContentPlaceHolder1_hfRoomId").val() != "") {
                $("#ContentPlaceHolder1_hfIsRoomIsSelectForPayment").val("1");
                $("#lblGuestRoom").text("Room# " + $("#ContentPlaceHolder1_lblRoomNumber").text());
            }
            $("#ContentPlaceHolder1_txtRoomPayment").focus();
        }

        function RoomSearchCancel() {

            if ($("#lblGuestRoom").text() == "Room Payment") {
                $("#ContentPlaceHolder1_hfRoomId").val("");
            }
            ClearPayment('Guest');
            $("#RoomInfoDialog").dialog("close");
        }

    </script>
</asp:Content>
