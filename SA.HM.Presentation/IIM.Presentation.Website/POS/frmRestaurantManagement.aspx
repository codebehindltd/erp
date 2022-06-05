<%@ Page Title="" Language="C#" MasterPageFile="~/POS/Restaurant.Master" AutoEventWireup="true"
    CodeBehind="frmRestaurantManagement.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmRestaurantManagement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="TopHeaderContent" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div style="font-size: 18px; font-weight: bold; text-align: center; color: #fff;
        padding-right: 5px; vertical-align: middle; padding-top: 9px;">
        Token Number:
        <asp:Label ID="lblTokenNumber" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var vTouchId = 0;
        var vTouchControl = "";
        var IsBillOnProcess = "0";

        $(document).ready(function () {

            if ($("#ContentPlaceHolder1_hfVat").val() == "1") {
                $("#ContentPlaceHolder1_cbTPVatAmount").prop("checked", true);
            }

            if ($("#ContentPlaceHolder1_hfSc").val() == "1") {
                $("#ContentPlaceHolder1_cbTPServiceCharge").prop("checked", true);
            }

            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Cost Center Choose</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            $('#ContentPlaceHolder1_pnlHomeButtonInfo').hide();
            $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
            $("#ContentPlaceHolder1_btnBillHolpup").attr("disabled", true);
            $("#ContentPlaceHolder1_btnOrderSubmit").attr("disabled", true);
            //$("#CategoryWiseDiscountPercentageList").hide();

            if ($("#ContentPlaceHolder1_hfIsResumeBill").val() == "1") {
                $("#billPreviewForBill").show();
            }
            else {
                $("#billPreviewForBill").hide();
            }

            $("#btnItemSpecialRemarks").click(function () {
                $("#TableInfoActionDecider").dialog('close');
                var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();
                var itemId = $("#<%=txtItemId.ClientID %>").val();

                PageMethods.GetSpecialRemarksDetails(kotId, itemId, OnGetSpecialRemarksDetailsSucceed, OnGetSpecialRemarksDetailsFailed);
            });

            $("#btnUpdateTableInfo").click(function () {
                $("#TableInfoActionDecider").dialog("close");

                $("#TouchKeypad").dialog({
                    width: '440',
                    maxWidth: 440,
                    autoOpen: true,
                    modal: true,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    height: 'auto',
                    title: "Item Quantity",
                    show: 'slide'
                });
            });

            $("#btnDeleteTableInfo").click(function () {
                //var kotDetailsId = $("#<%=txtKotDetailsIdInformation.ClientID %>").val();
                var kotDetailsId = $("#<%=hfKotDetailId.ClientID %>").val();
                var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();
                var itemId = $("#<%=txtItemId.ClientID %>").val();
                $("#TableInfoActionDecider").dialog("close");
                PerformDeleteAction(kotDetailsId, kotId, itemId);
            });

            $("#btnColseDeciderWidow").click(function () {
                $("#TableInfoActionDecider").dialog("close");
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

                var kotDetailId = $("#<%=hfKotDetailId.ClientID %>").val();

                PageMethods.SaveKotSpecialRemarks(RKotSRemarksDetail, RKotSRemarksDetailDelete, kotDetailId, OnSaveKotSpecialRemarksSuccedd, OnSaveKotSpecialRemarksFailed);
            });

            LoadGridInformation();

            $("#<%=txtTPDiscountAmount.ClientID %>").click(function () {

                vTouchId = 1;
                vTouchControl != "" ? $(vTouchControl).css({ "border-color": "", "box-shadow": "" }) : "";
                vTouchControl = $(this);
                $(vTouchControl).css({
                    "border-color": "rgba(82, 168, 236, 0.8)",
                    "box-shadow": "0 1px 1px rgba(0, 0, 0, 0.075) inset, 0 0 8px rgba(82, 168, 236, 0.6)"
                });
            });
            $("#<%=txtCash.ClientID %>").click(function () {
                vTouchId = 2;
                vTouchControl != "" ? $(vTouchControl).css({ "border-color": "", "box-shadow": "" }) : "";
                vTouchControl = $(this);
                $(vTouchControl).css({
                    "border-color": "rgba(82, 168, 236, 0.8)",
                    "box-shadow": "0 1px 1px rgba(0, 0, 0, 0.075) inset, 0 0 8px rgba(82, 168, 236, 0.6)"
                });
            });
            $("#<%=txtAmexCard.ClientID %>").click(function () {
                vTouchId = 3;
                vTouchControl != "" ? $(vTouchControl).css({ "border-color": "", "box-shadow": "" }) : "";
                vTouchControl = $(this);
                $(vTouchControl).css({
                    "border-color": "rgba(82, 168, 236, 0.8)",
                    "box-shadow": "0 1px 1px rgba(0, 0, 0, 0.075) inset, 0 0 8px rgba(82, 168, 236, 0.6)"
                });
            });
            $("#<%=txtMasterCard.ClientID %>").click(function () {
                vTouchId = 4;
                vTouchControl != "" ? $(vTouchControl).css({ "border-color": "", "box-shadow": "" }) : "";
                vTouchControl = $(this);
                $(vTouchControl).css({
                    "border-color": "rgba(82, 168, 236, 0.8)",
                    "box-shadow": "0 1px 1px rgba(0, 0, 0, 0.075) inset, 0 0 8px rgba(82, 168, 236, 0.6)"
                });
            });
            $("#<%=txtVisaCard.ClientID %>").click(function () {
                vTouchId = 5;
                vTouchControl != "" ? $(vTouchControl).css({ "border-color": "", "box-shadow": "" }) : "";
                vTouchControl = $(this);
                $(vTouchControl).css({
                    "border-color": "rgba(82, 168, 236, 0.8)",
                    "box-shadow": "0 1px 1px rgba(0, 0, 0, 0.075) inset, 0 0 8px rgba(82, 168, 236, 0.6)"
                });
            });
            $("#<%=txtDiscoverCard.ClientID %>").click(function () {
                vTouchId = 6;
                vTouchControl != "" ? $(vTouchControl).css({ "border-color": "", "box-shadow": "" }) : "";
                vTouchControl = $(this);
                $(vTouchControl).css({
                    "border-color": "rgba(82, 168, 236, 0.8)",
                    "box-shadow": "0 1px 1px rgba(0, 0, 0, 0.075) inset, 0 0 8px rgba(82, 168, 236, 0.6)"
                });
            });
            $("#<%=txtTPRoundedAmount.ClientID %>").click(function () {
                vTouchId = 7;
                vTouchControl != "" ? $(vTouchControl).css({ "border-color": "", "box-shadow": "" }) : "";
                vTouchControl = $(this);
                $(vTouchControl).css({
                    "border-color": "rgba(82, 168, 236, 0.8)",
                    "box-shadow": "0 1px 1px rgba(0, 0, 0, 0.075) inset, 0 0 8px rgba(82, 168, 236, 0.6)"
                });
            });

            $("[id=ContentPlaceHolder1_gvCategoryCostCenterInfo_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);
                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvCategoryCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvCategoryCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

            $("#ContentPlaceHolder1_rbTPFixedDiscount").click(function () {
                if ($(this).is(":checked")) {
                    //$("#CategoryWiseDiscountPercentageList").hide();
                    if ($("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "" && $("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "0") {
                        CalculateDiscountAmount();
                        $("#ContentPlaceHolder1_txtTPDiscountAmount").attr("disabled", false);
                    }
                }
            });

            $("#ContentPlaceHolder1_rbTPPercentageDiscount").click(function () {
                if ($(this).is(":checked")) {

                    //$("#CategoryWiseDiscountPercentageList").show();
                    $("#ContentPlaceHolder1_txtTPDiscountAmount").attr("disabled", false);
                    if ($("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "" && $("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "0") {
                        CalculateDiscountAmount();
                    }
                }
                else {
                    //$("#CategoryWiseDiscountPercentageList").hide();
                }
            });

            $("#ContentPlaceHolder1_rbTPComplementaryDiscount").click(function () {
                if ($(this).is(":checked")) {

                    $("#ContentPlaceHolder1_txtTPDiscountAmount").val("100");
                    $("#ContentPlaceHolder1_txtTPDiscountAmount").attr("disabled", true);
                    //$("#CategoryWiseDiscountPercentageList").show();

                    if ($("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "" && $("#ContentPlaceHolder1_txtTPDiscountAmount").val() != "0") {
                        CalculateDiscountAmount();
                    }
                }
                else {
                    //$("#CategoryWiseDiscountPercentageList").hide();
                }
            });

            $("#btnCategoryWiseDiscountOK").click(function () {
                var strCategoryIdList = new Array();
                var trLength = $("#TableCategoryWiseDiscount tbody tr").length
                var i = 0;
                for (i = 0; i < trLength; i++) {
                    if ($("#TableCategoryWiseDiscount tbody tr:eq(" + i + ")").find("td:eq(1)").find("input").is(':checked')) {
                        strCategoryIdList.push($("#TableCategoryWiseDiscount tbody tr:eq(" + i + ")").find("td:eq(0)").text());
                    }
                }

                var kotId = $("#ContentPlaceHolder1_txtKotIdInformation").val();
                var discountAmount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();
                var costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();

                PageMethods.PerformPercentageDiscountInformation(kotId, costCenterId, strCategoryIdList, discountAmount, OnPerformPercentageDiscountInformationSucceeded, OnPerformPercentageDiscountInformationFailed);

                return false;
            });

            $("#btnCategoryWiseDiscountCancel").click(function () {
                $("#PercentageDiscountDialog").dialog("close");
            });

        });

        function CustomDiscount() {
            var strCategoryIdList = new Array();
            var trLength = $("#TableCategoryWiseDiscount tbody tr").length
            var i = 0;
            for (i = 0; i < trLength; i++) {
                if ($("#TableCategoryWiseDiscount tbody tr:eq(" + i + ")").find("td:eq(1)").find("input").is(':checked')) {
                    strCategoryIdList.push($("#TableCategoryWiseDiscount tbody tr:eq(" + i + ")").find("td:eq(0)").text());
                }
            }

            var margeKot = "", discountType = "";

            if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked")) {
                discountType = "Percentage";
            }
            else if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
                discountType = "Fixed";
            }

            var kotId = $("#ContentPlaceHolder1_txtKotIdInformation").val();
            var discountAmount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();
            var costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();

            PageMethods.PerformPercentageDiscountInformation(kotId, margeKot, costCenterId, strCategoryIdList, discountType, discountAmount, OnPerformPercentageDiscountInformation1Succeeded, OnPerformPercentageDiscountInformationFailed)
        }

        function OnPerformPercentageDiscountInformation1Succeeded(result) {
            $("#<%=hfCategoryWiseTotalDiscountAmount.ClientID%>").val(result);
            $("#ContentPlaceHolder1_txtTPDiscount").val(result);

            $("#ContentPlaceHolder1_hfIsCategoryWiseDiscountCalculatedByUser").val("1");

            $("#PercentageDiscountDialog").dialog("close");
        }
        function OnPerformPercentageDiscountInformationSucceeded(result) {
            $("#<%=hfCategoryWiseTotalDiscountAmount.ClientID%>").val(result);
            $("#ContentPlaceHolder1_txtTPDiscount").val(result);

            $("#ContentPlaceHolder1_hfIsCategoryWiseDiscountCalculatedByUser").val("1");

            CalculateDiscountAmount();
            $("#PercentageDiscountDialog").dialog("close");
        }
        function OnPerformPercentageDiscountInformationFailed(error) {
            toastr.error(error.get_message());
        }

        function GoToHomePanel() {
            window.location = "frmCostCenterSelection.aspx";
            return false;
        }

        function OnSaveKotSpecialRemarksSuccedd(result) {
            $("#ItemWiseSpecialRemarks").dialog("close");

            if (result.AlertMessage.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#<%=txtItemId.ClientID %>").val("");
                alreadySaveKotRemarks = [];
                $("#remarksContainer").remove("TableWiseItemRemarksInformation");
                //$("#ItemWiseSpecialRemarks").dialog("close");
                LoadGridInformation();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }

        function OnSaveKotSpecialRemarksFailed(error) {
            toastr.error(error.get_message());
        }
        //---Save Individual Data---------
        function PerformAction(selectedItemId) {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var kotId = $("#<%=txtKotIdInformation.ClientID %>").val();

            CommonHelper.ExactMatch();

            var tr = $("#TableWiseItemInformation tbody tr").find("td:eq(5):textEquals('" + selectedItemId + "')").parent();
            var selectedItemQty = "0";
            if ($(tr).length != 0)
                selectedItemQty = $(tr).find("td:eq(1)").text();

            PageMethods.SaveIndividualItemDetailInformation(hfCostCenterIdVal, kotId, selectedItemId, selectedItemQty, OnSaveObjectSucceeded, OnSaveObjectFailed);
            return false;
        }

        function OnSaveObjectSucceeded(result) {
            LoadGridInformation();
            var keyboard = $('#txtItemName').getkeyboard();
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

            if (hfCostCenterIdVal > 0 && bearerId > 0 && tableId > 0 && kotId > 0) {
                PageMethods.GenerateTableWiseItemGridInformation(hfCostCenterIdVal, tableId, kotId, OnLoadObjectSucceeded, OnLoadObjectFailed);
            }
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            var heightVal = $("#<%=hfltlTableWiseItemInformationDivHeight.ClientID %>").val();
            $("#ltlTableWiseItemInformation").height(heightVal);
            $("#ltlTableWiseItemInformation").css("max-height", heightVal + "px");

            $("#ltlTableWiseItemInformation").html(result);

            var total = 0;
            $("#TableWiseItemInformation tbody tr").each(function () {
                var tt = parseInt($(this).find("td:eq(3)").text());
                total = total + tt;
            });

            $("#<%=txtTotalSales.ClientID %>").val(total);
            $("#<%=txtSalesAmountHiddenField.ClientID %>").val(total);
            $("#<%=txtChangeAmount.ClientID %>").val(total);

            CalculateSalesSummery(total);

            if ($("#ContentPlaceHolder1_txtSalesAmount").val() != "" && $("#ContentPlaceHolder1_txtSalesAmount").val() != "0") {
                $("#ContentPlaceHolder1_btnBillHolpup").attr("disabled", false);
                $("#ContentPlaceHolder1_btnOrderSubmit").attr("disabled", false);
            }

            return false;
        }

        function OnLoadObjectFailed(error) {
            toastr.error(error.get_message());
        }

        ///utility functions
        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var nodeClick = src.tagName.toLowerCase() == "a";
            if (nodeClick) {
                selectedNode = $(src).parents('table:first');
                //innerText works in IE but fails in Firefox (I'm sick of browser anomalies), so use innerHTML as well
                var nodeText = src.innerText || src.innerHTML;
                var nodeValue = GetNodeValue(src);

                $("#<%=tvLocations.ClientID %>").find("td").each(function () {
                    var tdClass = $(this).attr('class');
                    if (tdClass.indexOf('treeNodeSelected') > -1)
                        $(this).attr('class', tdClass.replace('treeNodeSelected', ''))
                });

                $("#" + src.id).parent().attr('class', $("#" + src.id).parent().attr('class') + ' treeNodeSelected');
                //$("#"+src.id).parent().className = "treeNodeSelected"
                //alert("Text: " + nodeText + "," + "Value: " + nodeValue);
                $(txtLocationId).val(nodeValue);
                OpenItem(nodeValue);
                return false; //comment this if you want postback on node click
            }
            else if (src.tagName.toLowerCase() == "td" && $(src).attr("class").indexOf("treeNode") > -1) {
                selectedNode = $(src).parents('table:first');
                //innerText works in IE but fails in Firefox (I'm sick of browser anomalies), so use innerHTML as well
                //var nodeText = $(src).children("a").innerText || $(src).children("a").innerHTML;
                var nodeValue = GetNodeValue($(src).children("a"));

                $("#<%=tvLocations.ClientID %>").find("td").each(function () {
                    var tdClass = $(this).attr('class');
                    if (tdClass.indexOf('treeNodeSelected') > -1)
                        $(this).attr('class', tdClass.replace('treeNodeSelected', ''))
                });

                $(src).attr('class', $(src).attr('class') + ' treeNodeSelected');
                $(txtLocationId).val(nodeValue);
                OpenItem(nodeValue);
                return false; //comment this if you want postback on node click
            }
            nodeClick = src.tagName.toLowerCase() == "img" && $(src).attr("src").indexOf("house.png") > 0
            if (nodeClick) {
                selectedNode = $(src).parents('table:first');
                return false; //comment this if you want postback on node click
            }
        }

        function GetNodeValue(node) {
            var nodeValue = "";
            var hrefValue = node.href == null ? node.attr('href') : node.href;
            var nodePath = hrefValue.substring(hrefValue.indexOf(",") + 2, hrefValue.length - 2);
            var nodeValues = nodePath.split("\\");
            if (nodeValues.length > 1)
                nodeValue = nodeValues[nodeValues.length - 1];
            else
                nodeValue = nodeValues[0].substr(1);
            return nodeValue;
        }

        function LoadPaymentInformation() {
            var total = 0;
            $("#TableWiseItemInformation tbody tr").each(function () {
                var tt = parseInt($(this).find("td:eq(3)").text());
                total = total + tt;
            });

            //            if ($("#<%=hfIsRestaurantBillInclusive.ClientID %>").val() != "0") {
            //                $("#ContentPlaceHolder1_txtTPGrandTotal").val($("#ContentPlaceHolder1_txtGrandTotal").val());
            //                $("#<%=txtTotalSales.ClientID %>").val(total);
            //                $("#<%=txtSalesAmountHiddenField.ClientID %>").val(total);
            //                $("#<%=txtTPSalesAmountHiddenField.ClientID %>").val(total);
            //            }
            //            else {
            //                $("#ContentPlaceHolder1_txtTPGrandTotal").val($("#ContentPlaceHolder1_txtGrandTotal").val());
            //                $("#<%=txtTotalSales.ClientID %>").val(total);
            //                $("#<%=txtSalesAmountHiddenField.ClientID %>").val(total);
            //                $("#<%=txtTPSalesAmountHiddenField.ClientID %>").val(total);
            //            }

            $("#ContentPlaceHolder1_txtTPGrandTotal").val($("#ContentPlaceHolder1_txtGrandTotal").val());
            $("#<%=txtTotalSales.ClientID %>").val(total);
            $("#<%=txtSalesAmountHiddenField.ClientID %>").val(total);
            $("#<%=txtTPSalesAmountHiddenField.ClientID %>").val(total);
            $("#ContentPlaceHolder1_txtTPVatAmountHiddenField").val($("#ContentPlaceHolder1_txtVatAmount").val());

            var discountAmount = $("#<%=txtTPDiscountAmount.ClientID %>").val();

            if (discountAmount == "") {
                discountAmount = 0;
            }

            if (discountAmount == 0) {
                $("#<%=txtTPDiscountedAmount.ClientID %>").val(total);
                $("#<%=txtTPDiscountedAmountHiddenField.ClientID %>").val(total);
            }

            CalculateDiscountAmount();

            if (total != 0) {
                $("#RestaurantPaymentInformationDiv").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 950,
                    height: 600,
                    closeOnEscape: true,
                    resizable: false,
                    title: "Payment Information",
                    show: 'slide'
                });
                return;
            }
        }

        function PerformTPOkButton() {

            var dueAmountText = $.trim($("#ContentPlaceHolder1_lblTPChangeAmount").text());
            var dueAmount = $("#ContentPlaceHolder1_txtTPChangeAmount").val();

            if ($("#ContentPlaceHolder1_cbTPVatAmount").is(":checked"))
                $("#ContentPlaceHolder1_hfVat").val("1");
            else
                $("#ContentPlaceHolder1_hfVat").val("0");

            if ($("#ContentPlaceHolder1_cbTPServiceCharge").is(":checked"))
                $("#ContentPlaceHolder1_hfSc").val("1");
            else
                $("#ContentPlaceHolder1_hfSc").val("0");

            if (dueAmount == "")
                dueAmount = "0";

            if (dueAmountText != "Due Amount") {
                $("#ContentPlaceHolder1_btnSave").attr("disabled", false);
            }
            else {
                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
            }

            $(vTouchControl).css({ "border-color": "", "box-shadow": "" });
            vTouchControl = "";
            vTouchId = "0";

            $("#RestaurantPaymentInformationDiv").dialog("close");

            //PageMethods.SavePaymentInformationInSession(DiscountTypeVal, txtTPDiscountAmountVal, txtCashVal, txtAmexCardVal, txtMasterCardVal, txtVisaCardVal, txtDiscoverCardVal, OnLoadPaymentInformationInSessionSucceeded, OnLoadPaymentInformationInSessionFailed);
        }

        function OnLoadPaymentInformationInSessionSucceeded(result) {
            $("#RestaurantPaymentInformationDiv").dialog("close");
            return false;
        }

        function OnLoadPaymentInformationInSessionFailed(error) {
            toastr.error(error.get_message());
        }

        function myFunction(val) {
            if (vTouchId == 0) {
                toastr.warning("Please Select a Payment Type.");
            }
            else {
                if (val != 10 && val != 11 && val != 12) {
                    if (vTouchId == 1) {
                        var existingValue = $("#<%=txtTPDiscountAmount.ClientID %>").val();
                        $("#<%=txtTPDiscountAmount.ClientID %>").val(existingValue + val);
                    }
                    else if (vTouchId == 2) {
                        var existingValue = $("#<%=txtCash.ClientID %>").val();
                        $("#<%=txtCash.ClientID %>").val(existingValue + val);
                    }
                    else if (vTouchId == 3) {
                        var existingValue = $("#<%=txtAmexCard.ClientID %>").val();
                        $("#<%=txtAmexCard.ClientID %>").val(existingValue + val);
                    }
                    else if (vTouchId == 4) {
                        var existingValue = $("#<%=txtMasterCard.ClientID %>").val();
                        $("#<%=txtMasterCard.ClientID %>").val(existingValue + val);
                    }
                    else if (vTouchId == 5) {
                        var existingValue = $("#<%=txtVisaCard.ClientID %>").val();
                        $("#<%=txtVisaCard.ClientID %>").val(existingValue + val);
                    }
                    else if (vTouchId == 6) {
                        var existingValue = $("#<%=txtDiscoverCard.ClientID %>").val();
                        $("#<%=txtDiscoverCard.ClientID %>").val(existingValue + val);
                    }
                    else if (vTouchId == 7) {
                        var existingValue = $("#<%=txtTPRoundedAmount.ClientID %>").val();
                        $("#<%=txtTPRoundedAmount.ClientID %>").val(existingValue + val);
                    }
                }
                else if (val == 10) {
                    if (vTouchId == 1) {
                        var existingValue = $("#<%=txtTPDiscountAmount.ClientID %>").val();
                        var shortenedString = existingValue.substr(0, (existingValue.length - 1));
                        $("#<%=txtTPDiscountAmount.ClientID %>").val(shortenedString);
                    }
                    else if (vTouchId == 2) {
                        var existingValue = $("#<%=txtCash.ClientID %>").val();
                        var shortenedString = existingValue.substr(0, (existingValue.length - 1));
                        $("#<%=txtCash.ClientID %>").val(shortenedString);
                    }
                    else if (vTouchId == 3) {
                        var existingValue = $("#<%=txtAmexCard.ClientID %>").val();
                        var shortenedString = existingValue.substr(0, (existingValue.length - 1));
                        $("#<%=txtAmexCard.ClientID %>").val(shortenedString);
                    }
                    else if (vTouchId == 4) {
                        var existingValue = $("#<%=txtMasterCard.ClientID %>").val();
                        var shortenedString = existingValue.substr(0, (existingValue.length - 1));
                        $("#<%=txtMasterCard.ClientID %>").val(shortenedString);
                    }
                    else if (vTouchId == 5) {
                        var existingValue = $("#<%=txtVisaCard.ClientID %>").val();
                        var shortenedString = existingValue.substr(0, (existingValue.length - 1));
                        $("#<%=txtVisaCard.ClientID %>").val(shortenedString);
                    }
                    else if (vTouchId == 6) {
                        var existingValue = $("#<%=txtDiscoverCard.ClientID %>").val();
                        var shortenedString = existingValue.substr(0, (existingValue.length - 1));
                        $("#<%=txtDiscoverCard.ClientID %>").val(shortenedString);
                    }
                    else if (vTouchId == 7) {
                        var existingValue = $("#<%=txtTPRoundedAmount.ClientID %>").val();
                        var shortenedString = existingValue.substr(0, (existingValue.length - 1));
                        $("#<%=txtTPRoundedAmount.ClientID %>").val(shortenedString);
                    }
                }
                else if (val == 11) {
                    if (vTouchId == 1) {
                        var existingValue = $("#<%=txtTPDiscountAmount.ClientID %>").val();
                        $("#<%=txtTPDiscountAmount.ClientID %>").val(existingValue + '.');
                    }
                    else if (vTouchId == 2) {
                        var existingValue = $("#<%=txtCash.ClientID %>").val();
                        $("#<%=txtCash.ClientID %>").val(existingValue + '.');
                    }
                    else if (vTouchId == 3) {
                        var existingValue = $("#<%=txtAmexCard.ClientID %>").val();
                        $("#<%=txtAmexCard.ClientID %>").val(existingValue + '.');
                    }
                    else if (vTouchId == 4) {
                        var existingValue = $("#<%=txtMasterCard.ClientID %>").val();
                        $("#<%=txtMasterCard.ClientID %>").val(existingValue + '.');
                    }
                    else if (vTouchId == 5) {
                        var existingValue = $("#<%=txtVisaCard.ClientID %>").val();
                        $("#<%=txtVisaCard.ClientID %>").val(existingValue + '.');
                    }
                    else if (vTouchId == 6) {
                        var existingValue = $("#<%=txtDiscoverCard.ClientID %>").val();
                        $("#<%=txtDiscoverCard.ClientID %>").val(existingValue + '.');
                    }
                    else if (vTouchId == 7) {
                        var existingValue = $("#<%=txtTPRoundedAmount.ClientID %>").val();
                        $("#<%=txtTPRoundedAmount.ClientID %>").val(existingValue + '.');
                    }
                }
                else if (val == 12) {
                    if (vTouchId == 1) {
                        $("#<%=txtTPDiscountAmount.ClientID %>").val('0');
                    }
                    else if (vTouchId == 2) {
                        $("#<%=txtCash.ClientID %>").val('');
                    }
                    else if (vTouchId == 3) {
                        $("#<%=txtAmexCard.ClientID %>").val('');
                    }
                    else if (vTouchId == 4) {
                        $("#<%=txtMasterCard.ClientID %>").val('');
                    }
                    else if (vTouchId == 5) {
                        $("#<%=txtVisaCard.ClientID %>").val('');
                    }
                    else if (vTouchId == 6) {
                        $("#<%=txtDiscoverCard.ClientID %>").val('');
                    }
                    else if (vTouchId == 7) {
                        $("#<%=txtTPRoundedAmount.ClientID %>").val('');
                    }
                }
            }

            if (vTouchId == 1) {
                CalculateDiscountAmount();
            }
            else {
                CalculatePayment();
            }
        }

        function CalculateSalesSummery(total) {
            $("#<%=txtSalesAmount.ClientID %>").val(total);
            $("#<%=txtDiscountedAmount.ClientID %>").val(total);
            $("#<%=txtDiscountedAmountHiddenField.ClientID %>").val(total);
            $("#<%=txtCalculatedDiscountAmount.ClientID %>").val(0);

            //$("#<%=txtTPDiscountAmount.ClientID %>").val(0);
            $("#<%=txtTPDiscountedAmountHiddenField.ClientID %>").val(total);

            TotalSalesAmountVatServiceChargeCalculation();
        }

        function ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge(ctrl) {
            TotalSalesAmountVatServiceChargeCalculation();
            CalculatePayment();
        }

        function ToggleTotalSalesAmountVatServiceChargeCalculationForVat(ctrl) {
            TotalSalesAmountVatServiceChargeCalculation();
            CalculatePayment();
        }

        function TotalSalesAmountVatServiceChargeCalculation() {
            var txtDiscountedAmountHiddenField = '<%=txtTPDiscountedAmountHiddenField.ClientID%>'
            var cbServiceCharge = '<%=cbTPServiceCharge.ClientID%>'
            var cbVatAmount = '<%=cbTPVatAmount.ClientID%>'

            var cbServiceChargeVal = "1";
            if ($('#' + cbServiceCharge).is(':checked')) {
                cbServiceChargeVal = "1";
            }
            else {
                cbServiceChargeVal = "0";
            }

            var cbVatAmountVal = "1";
            if ($('#' + cbVatAmount).is(':checked')) {
                cbVatAmountVal = "1";
            }
            else {
                cbVatAmountVal = "0";
            }

            var txtRoomRateVal = parseFloat($('#' + txtDiscountedAmountHiddenField).val());

            var InclusiveBill = 0, Vat = 0.00, ServiceCharge = 0.00;

            if ($("#<%=hfIsRestaurantBillInclusive.ClientID %>").val() != "")
            { InclusiveBill = parseInt($("#<%=hfIsRestaurantBillInclusive.ClientID %>").val(), 10); }

            if ($("#<%=hfRestaurantVatAmount.ClientID %>").val() != "")
                Vat = parseFloat($("#<%=hfRestaurantVatAmount.ClientID %>").val());

            if ($("#<%=hfRestaurantServiceCharge.ClientID %>").val() != "")
                ServiceCharge = parseFloat($("#<%=hfRestaurantServiceCharge.ClientID %>").val());

            var InnboardRateCalculationInfo = CommonHelper.GetRackRateServiceChargeVatInformation(InclusiveBill, Vat, ServiceCharge, 0, txtRoomRateVal, parseInt(cbServiceChargeVal, 10), 0, parseInt(cbVatAmountVal, 10));
            OnLoadRackRateServiceChargeVatInformationSucceeded(InnboardRateCalculationInfo);
        }

        function OnLoadRackRateServiceChargeVatInformationSucceeded(result) {
            if (result.RackRate != 'NaN') {

                var InclusiveBill = 0
                if ($("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val() != "")
                { InclusiveBill = parseInt($("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val(), 10); }

                if (InclusiveBill == 1) {
                    result.RackRate = parseFloat(result.RackRate) + parseFloat(result.ServiceCharge) + parseFloat(result.VatAmount);
                }

                if ($("#<%=hfIsRestaurantBillAmountWillRound.ClientID %>").val() == "1") {
                    $("#<%=txtGrandTotal.ClientID %>").val(Math.round(result.RackRate));
                    $("#<%=txtTPGrandTotal.ClientID %>").val($("#<%=txtGrandTotal.ClientID %>").val());
                    $("#<%=txtGrandTotalHiddenField.ClientID %>").val(Math.round(result.RackRate));

                    $("#<%=txtChangeAmount.ClientID %>").val(result.RackRate);
                    $("#<%=txtTPChangeAmountHiddenField.ClientID %>").val(result.RackRate);
                }
                else {
                    $("#<%=txtGrandTotal.ClientID %>").val(result.RackRate);
                    $("#<%=txtTPGrandTotal.ClientID %>").val($("#<%=txtGrandTotal.ClientID %>").val());
                    $("#<%=txtGrandTotalHiddenField.ClientID %>").val(result.RackRate);
                    $("#<%=txtChangeAmount.ClientID %>").val(result.RackRate);
                    $("#<%=txtTPChangeAmountHiddenField.ClientID %>").val(result.RackRate);
                }

                $("#<%=txtTPServiceCharge.ClientID %>").val(result.ServiceCharge);
                $("#<%=txtTPServiceChargeAmountHiddenField.ClientID %>").val(result.ServiceCharge);
                $("#<%=txtTPVatAmount.ClientID %>").val(result.VatAmount);
                $("#<%=txtTPVatAmountHiddenField.ClientID %>").val(result.VatAmount);

                $("#<%=txtServiceCharge.ClientID %>").val(result.ServiceCharge);
                $("#<%=txtServiceChargeAmountHiddenField.ClientID %>").val(result.ServiceCharge);
                $("#<%=txtVatAmount.ClientID %>").val(result.VatAmount);
                $("#<%=txtVatAmountHiddenField.ClientID %>").val(result.VatAmount);

                if ($("#<%=hfIsRestaurantBillInclusive.ClientID %>").val() == 0) {
                }
            }
            else {
                $("#<%=txtGrandTotal.ClientID %>").val('0');
                $("#<%=txtTPGrandTotal.ClientID %>").val('0');
                $("#<%=txtGrandTotalHiddenField.ClientID %>").val('0');

                $("#<%=txtTPServiceCharge.ClientID %>").val('0');
                $("#<%=txtTPServiceChargeAmountHiddenField.ClientID %>").val('0');
                $("#<%=txtTPVatAmount.ClientID %>").val('0');
                $("#<%=txtTPVatAmountHiddenField.ClientID %>").val('0');

                $("#<%=txtServiceCharge.ClientID %>").val('0');
                $("#<%=txtServiceChargeAmountHiddenField.ClientID %>").val('0');
                $("#<%=txtVatAmount.ClientID %>").val('0');
                $("#<%=txtVatAmountHiddenField.ClientID %>").val('0');
            }

            return false;
        }

        function OnLoadRackRateServiceChargeVatInformationFailed(error) {
            //alert(error.get_message());
        }

        function PerformDeleteAction(kotDetailsId, actionId, itemId) {

            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                var tr = $("#TableWiseItemInformation tbody tr").find("td:eq(5):contains('" + itemId + "')").parent();
                var selectedItemQty = "0";
                if ($(tr).length != 0) {
                    selectedItemQty = $(tr).find("td:eq(1)").text();
                }

                var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
                //var kotDetailsId = $("#hfKotDetailId").val();

                PageMethods.UpdateIndividualItemDetailInformation('QuantityChange', hfCostCenterIdVal, kotDetailsId, selectedItemQty, 0, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
                //PageMethods.DeleteData(kotDetailsId, actionId, itemId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            LoadGridInformation();
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function AddNewItem(kotDetailId, kotId, itemId, isAlreadySubmitted) {
            $("#<%=txtKotDetailsIdInformation.ClientID %>").val(kotDetailId);
            $("#<%=txtKotIdInformation.ClientID %>").val(kotId);
            $("#<%=txtItemId.ClientID %>").val(itemId);
            $("#<%=hfKotDetailId.ClientID %>").val(kotDetailId);

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

        function LoadPercentageDiscountInfo() {

            if ($("#ContentPlaceHolder1_hfIsCategoryWiseDiscountAlreadyLoad").val() == "1") {

                $("#PercentageDiscountDialog").dialog({
                    autoOpen: true,
                    modal: true,
                    maxWidth: 500,
                    width: 'auto',
                    closeOnEscape: true,
                    resizable: false,
                    height: 'auto',
                    fluid: true,
                    title: "Category Wise Discount",
                    show: 'slide'
                });

                return false;
            }

            var kotId = $("#ContentPlaceHolder1_txtKotIdInformation").val();
            PageMethods.GetCategoryWiseDiscount(kotId, OnLoadDiscountSucceeded, OnLoadDiscountFailed);
        }

        function OnLoadDiscountSucceeded(result) {

            if (result.length <= 0)
                return false;

            var table = "", tr = "", td = "", i = 0, alreadyChecked = "";
            var categoryWiseDiscountLength = result.length;

            $("#ContentPlaceHolder1_hfIsCategoryWiseDiscountAlreadyLoad").val("1");

            table = "<div id='no-more-tables'> ";
            table += "<table cellspacing=\"0\" cellpadding=\"4\" id=\"TableCategoryWiseDiscount\" style=\"margin:0;\" >";
            table += "<thead>" +
                            "<tr style=\"color: White; background-color: #44545E; font-weight: bold;\">" +
                                "<th align=\"center\" scope=\"col\" style=\"width: 50px\">" +
                                    "Select" +
                                "</th>" +
                                "<th align=\"left\" scope=\"col\" style=\"width: 270px\">" +
                                    "Category" +
                                "</th>" +
                            "</tr>" +
                        "</thead> <tbody>";

            for (i = 0; i < categoryWiseDiscountLength; i++) {

                if ((i % 2) == 0)
                    tr = "<tr style=\"background-color:#ffffff;\">";
                else
                    tr = "<tr style=\"background-color:#E3EAEB;\">";

                td = "<td style=\"display:none\">" + result[i].CategoryId + "</td>" +
                     "<td data-title='Select' align=\"center\" style=\"width: 50px\">";

                //                if (result[i].ActiveStat == true)
                //                    td += "&nbsp;<input type=\"checkbox\" checked='checked' value=\"" + result[i].CategoryId + " id=\"ch" + result[i].CategoryId + "\">";
                //                else
                td += "&nbsp;<input type=\"checkbox\" value=\"" + result[i].CategoryId + " id=\"ch" + result[i].CategoryId + "\">";

                td += "</td>" +
                      "<td data-title='Remarks' align=\"left\" style=\"width: 270px\">" +
                       result[i].Name +
                      "</td>";

                tr += td + "</tr> </div>";

                table += tr;
            }
            table += " </tbody> </table>";

            $("#PercentageDiscountContainer").html(table);

            $("#PercentageDiscountDialog").dialog({
                autoOpen: true,
                modal: true,
                maxWidth: 500,
                width: 'auto',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Category Wise Discount",
                show: 'slide'
            });
        }

        function OnLoadDiscountFailed() { }

        function myFunctionTouchKeypad(val) {
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

        function PerformUpdateActionTouchKeypad(updateType, updatedContent) {
            var hfCostCenterIdVal = $("#<%=hfCostCenterId.ClientID %>").val();
            var selectedId = $("#<%=txtKotDetailsIdInformation.ClientID %>").val();
            var itemQuantity = $("#<%=txtTouchKeypadResult.ClientID %>").val();
            var updatedContent = itemQuantity;
            var updateType = "QuantityChange"

            //PageMethods.UpdateIndividualItemDetailInformation(hfCostCenterIdVal, selectedId, itemQuantity, OnUpdateObjectSucceeded, OnUpdateObjectFailed);
            PageMethods.UpdateIndividualItemDetailInformation(updateType, hfCostCenterIdVal, selectedId, itemQuantity, updatedContent, OnUpdateTouchKeypadObjectSucceeded, OnUpdateTouchKeypadObjectFailed);
            return false;
        }

        function OnUpdateTouchKeypadObjectSucceeded(result) {
            $("#<%=txtKotDetailsIdInformation.ClientID %>").val('');
            $("#<%=txtTouchKeypadResult.ClientID %>").val('');
            $("#TouchKeypad").dialog("close");
            LoadGridInformation();
            return false;
        }

        function OnUpdateTouchKeypadObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function CalculateDiscountAmount() {

            if ($("#ContentPlaceHolder1_txtTPDiscountAmount").val() == "")
                return;

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

            CalculatePayment();

            if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
                ToggleTotalSalesAmountVatServiceChargeCalculationForVat($("#ContentPlaceHolder1_rbTPFixedDiscount"));
                ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge($("#ContentPlaceHolder1_rbTPFixedDiscount"));
            }
            else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked") || $("#ContentPlaceHolder1_rbTPComplementaryDiscount").is(":checked")) {
                ToggleTotalSalesAmountVatServiceChargeCalculationForVat($("#ContentPlaceHolder1_rbTPFixedDiscount"));
                ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge($("#ContentPlaceHolder1_rbTPFixedDiscount"));
            }

            //            if ($("#ContentPlaceHolder1_hfIsCategoryWiseDiscountCalculatedByUser").val() == "1") {

            //                CustomDiscount();

            //                var discountAmount = $("#ContentPlaceHolder1_hfCategoryWiseTotalDiscountAmount").val();

            //                if (discountAmount != "0") {
            //                    $("#ContentPlaceHolder1_txtTPDiscount").val(discountAmount);
            //                }

            //                CalculatePayment();

            //                if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
            //                    ToggleTotalSalesAmountVatServiceChargeCalculationForVat($("#ContentPlaceHolder1_rbTPFixedDiscount"));
            //                    ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge($("#ContentPlaceHolder1_rbTPFixedDiscount"));
            //                }
            //                else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked")) {
            //                    ToggleTotalSalesAmountVatServiceChargeCalculationForVat($("#ContentPlaceHolder1_rbTPFixedDiscount"));
            //                    ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge($("#ContentPlaceHolder1_rbTPFixedDiscount"));
            //                }

            //                return false;
            //            }

            //            CalculateCategoryWiseDiscountAmount().done(function (response) {

            //                var discountAmount = $("#ContentPlaceHolder1_hfCategoryWiseTotalDiscountAmount").val();

            //                if (discountAmount != "0") {
            //                    $("#ContentPlaceHolder1_txtTPDiscount").val(discountAmount);
            //                }

            //                CalculatePayment();

            //                if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
            //                    ToggleTotalSalesAmountVatServiceChargeCalculationForVat($("#ContentPlaceHolder1_rbTPFixedDiscount"));
            //                    ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge($("#ContentPlaceHolder1_rbTPFixedDiscount"));
            //                }
            //                else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked")) {
            //                    ToggleTotalSalesAmountVatServiceChargeCalculationForVat($("#ContentPlaceHolder1_rbTPFixedDiscount"));
            //                    ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge($("#ContentPlaceHolder1_rbTPFixedDiscount"));
            //                }

            //            });
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

            var discountAmount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();
            var salesAmount = $("#ContentPlaceHolder1_txtTPSalesAmountHiddenField").val();

            if (discountAmount == "")
            { discountAmount = "0"; }

            var discountedAmount = 0;

            if ($('#ContentPlaceHolder1_rbTPFixedDiscount').is(':checked')) {
                discountedAmount = (parseFloat(salesAmount) - parseFloat(discountAmount));
                $("#ContentPlaceHolder1_txtTPDiscount").val(discountAmount);
            }
            else if ($('#ContentPlaceHolder1_rbTPPercentageDiscount').is(':checked') || $('#ContentPlaceHolder1_rbTPComplementaryDiscount').is(':checked')) {

                var categoryWiseTotalDiscount = $("#ContentPlaceHolder1_hfCategoryWiseTotalDiscountAmount").val();

                if (categoryWiseTotalDiscount != "0" && categoryWiseTotalDiscount != "") {
                    discountedAmount = (parseFloat(salesAmount) - parseFloat(categoryWiseTotalDiscount));
                }
                else {
                    var parcentAmount = parseFloat(discountAmount) / 100;
                    var discount = ((parseFloat(salesAmount) * parcentAmount));

                    var discountTruncate = discount.toString();

                    if (discountTruncate.contains('.')) {
                        var dd = discountTruncate.split('.');

                        if (dd[1].length > 3) {
                            discount = parseFloat(dd[0] + '.' + dd[1].substring(0, 2));
                        }
                    }
                    discountedAmount = (parseFloat(salesAmount) - parseFloat(discount));

                    $("#ContentPlaceHolder1_txtTPDiscount").val(discount);
                }
            }

            $("#ContentPlaceHolder1_txtTPDiscountedAmount").val(discountedAmount);
            $("#ContentPlaceHolder1_txtTPDiscountedAmountHiddenField").val(discountedAmount);

            var cashPayment = $("#ContentPlaceHolder1_txtCash").val() == "" ? "0" : $("#ContentPlaceHolder1_txtCash").val();
            var amexCardPayment = $("#ContentPlaceHolder1_txtAmexCard").val() == "" ? "0" : $("#ContentPlaceHolder1_txtAmexCard").val();
            var masterCardPayment = $("#ContentPlaceHolder1_txtMasterCard").val() == "" ? "0" : $("#ContentPlaceHolder1_txtMasterCard").val();
            var visaCardPayment = $("#ContentPlaceHolder1_txtVisaCard").val() == "" ? "0" : $("#ContentPlaceHolder1_txtVisaCard").val();
            var discoverCardPayment = $("#ContentPlaceHolder1_txtDiscoverCard").val() == "" ? "0" : $("#ContentPlaceHolder1_txtDiscoverCard").val();
            var roundedAmount = $("#ContentPlaceHolder1_txtTPRoundedAmount").val() == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtTPRoundedAmount").val());

            var totalPayment = parseFloat(cashPayment) + parseFloat(amexCardPayment) +
                               parseFloat(masterCardPayment) + parseFloat(visaCardPayment) + parseFloat(discoverCardPayment);

            totalPayment = parseFloat(totalPayment).toFixed(2);

            $("#ContentPlaceHolder1_txtTPTotalPaymentAmount").val(totalPayment);
            $("#ContentPlaceHolder1_txtTPTotalPaymentAmountHiddenField").val(totalPayment);

            $("#ContentPlaceHolder1_txtTPVatAmountHiddenField").val($("#ContentPlaceHolder1_txtVatAmount").val());

            var vatAmount = $("#ContentPlaceHolder1_txtTPVatAmountHiddenField").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTPVatAmountHiddenField").val();
            var serviceChargeAmount = $("#ContentPlaceHolder1_txtTPServiceChargeAmountHiddenField").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTPServiceChargeAmountHiddenField").val();

            var isInclusive = $("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val();
            var billAmount = 0.00;

            if (isInclusive == "1") {
                billAmount = discountedAmount; // - parseFloat(vatAmount) - parseFloat(serviceChargeAmount);
            }
            else {
                billAmount = discountedAmount + parseFloat(vatAmount) + parseFloat(serviceChargeAmount);
            }

            // var billAmount = discountedAmount + parseFloat(vatAmount) + parseFloat(serviceChargeAmount);

            //$("#ContentPlaceHolder1_txtTPGrandTotal").val(billAmount - totalPayment);
            $("#ContentPlaceHolder1_txtTPGrandTotalHiddenField").val(billAmount - totalPayment);
            $("#ContentPlaceHolder1_txtTPRoundedAmountHiddenField").val(roundedAmount);
            $("#ContentPlaceHolder1_txtRoundedAmount").val(roundedAmount);

            if (Math.round((billAmount - totalPayment - roundedAmount)) > 0) {
                $("#ContentPlaceHolder1_lblTPChangeAmount").text('Due Amount');
                if ($("#<%=hfIsRestaurantBillAmountWillRound.ClientID %>").val() == "1") {
                    $("#ContentPlaceHolder1_txtTPChangeAmount").val(Math.round(parseFloat(billAmount - totalPayment - roundedAmount).toFixed(2)));
                    $("#ContentPlaceHolder1_txtTPChangeAmountHiddenField").val(Math.round(parseFloat(billAmount - totalPayment - roundedAmount).toFixed(2)));
                }
                else {
                    $("#ContentPlaceHolder1_txtTPChangeAmount").val(parseFloat(billAmount - totalPayment - roundedAmount).toFixed(2));
                    $("#ContentPlaceHolder1_txtTPChangeAmountHiddenField").val(parseFloat(billAmount - totalPayment - roundedAmount).toFixed(2));
                }
            }
            else {
                $("#ContentPlaceHolder1_lblTPChangeAmount").text('Change Amount');

                if ($("#<%=hfIsRestaurantBillAmountWillRound.ClientID %>").val() == "1") {
                    $("#ContentPlaceHolder1_txtTPChangeAmount").val(Math.round((-1) * (parseFloat(billAmount - totalPayment - roundedAmount).toFixed(2))));
                    $("#ContentPlaceHolder1_txtTPChangeAmountHiddenField").val(Math.round((-1) * parseFloat(billAmount - totalPayment - roundedAmount).toFixed(2)));
                }
                else {
                    $("#ContentPlaceHolder1_txtTPChangeAmount").val((-1) * (parseFloat(billAmount - totalPayment - roundedAmount).toFixed(2)));
                    $("#ContentPlaceHolder1_txtTPChangeAmountHiddenField").val((-1) * parseFloat(billAmount - totalPayment - roundedAmount).toFixed(2));
                }
            }

            if ($.trim(salesAmount) != "") {
                $("#ContentPlaceHolder1_txtTotalPaymentAmount").val(totalPayment);
                $("#ContentPlaceHolder1_txtChangeAmount").val($("#ContentPlaceHolder1_txtTPChangeAmount").val());
                $("#ContentPlaceHolder1_lblTotalChangeAmount").text($("#ContentPlaceHolder1_lblTPChangeAmount").text());
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

        function CalculateCategoryWiseDiscountAmount() {

            var kotId = $("#ContentPlaceHolder1_txtKotIdInformation").val();
            var discountAmount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();
            var costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();

            var discountType = "Fixed";

            if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
                discountType = "Fixed";
            }
            else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked")) {
                discountType = " Percentage";
            }

            $("#ContentPlaceHolder1_hfCategoryWiseTotalDiscountAmount").val("0");

            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../POS/frmRestaurantManagement.aspx/GetCategoryWiseDiscountAmountByDefaultSetting',
                data: "{'kotId':'" + kotId + "','costCenterId':'" + costCenterId + "','discountAmount':'" + discountAmount + "','discountType':'" + discountType + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d != null) {
                        $("#ContentPlaceHolder1_hfCategoryWiseTotalDiscountAmount").val(data.d);
                    }
                    else {
                        $("#ContentPlaceHolder1_hfCategoryWiseTotalDiscountAmount").val("0");
                    }
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }

        function BillPreview() {
            var discountType = "", costCenterId = "", discountAmount = "", categoryIdList = "", billId = "";

            billId = $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val();
            discountAmount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTPDiscountAmount").val();

            if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(":checked")) {
                discountType = "Fixed";
            }
            else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(":checked")) {
                discountType = "Percentage";
            }

            costCenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            $("#ContentPlaceHolder1_hfBillId").val($("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val());

            //$('#btnBillNPrintPreview').trigger('click');

            PageMethods.UpdateRestaurantBillSummary(billId, discountType, costCenterId, discountAmount, categoryIdList, OnUpdateRestaurantBillSucceed, OnUpdateRestaurantBillFailed);

            return false;
        }
        function OnUpdateRestaurantBillSucceed(result) {
            if (result.IsSuccess == true) {

                var iframeid = 'printDoc';
                var url = "/POS/Reports/frmReportTPBillInfo.aspx?billID=" + result.Pk;
                parent.document.getElementById(iframeid).src = url;

                // $("#displayBill").show();

                //setTimeout(PrintAfterSometimes, 5000);

                //ClearWithPopUpInvoicePreview();

                $("#displayBill").dialog({
                    autoOpen: true,
                    modal: true,
                    minWidth: 500,
                    minHeight: 555,
                    width: 'auto',
                    closeOnEscape: false,
                    resizable: false,
                    height: 'auto',
                    fluid: true,
                    title: "Invoice Preview",
                    show: 'slide'
                    //,close: ClosePrintDialog
                });

                setTimeout(function () { ScrollToBottom(); }, 1000);

                //CommonHelper.AlertMessage(result.AlertMessage);

            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);

            }
        }
        function OnUpdateRestaurantBillFailed(error) {

        }


        function KotSubmit() {
            var costcenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var kotId = $("#ContentPlaceHolder1_txtKotIdInformation").val();

            PageMethods.KotSubmit(costcenterId, kotId, OnKotSubmitSucceed, OnKotSubmitFailed);
            return false;
        }
        function OnKotSubmitSucceed(result) {
            if (result.IsSuccess == true) {
                LoadGridInformation();
                CommonHelper.AlertMessage(result.AlertMessage);

            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);

            }
        }
        function OnKotSubmitFailed() { $loading.hide(); }

        function ValidateFormBeforeSettlement(settlementType) {            
            if (IsBillOnProcess == "1") {
                toastr.info("Bill Already In Processed.");
                return false;
            }

            IsBillOnProcess = "1";
            CommonHelper.SpinnerOpen();

            var costcenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var discountType = "Fixed", isComplementary = false;

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

            var calculatedDiscountAmount = "0";
            var discountAmount = $("#ContentPlaceHolder1_txtTPDiscountAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTPDiscountAmount").val();

            var salesAmountTP = $("#ContentPlaceHolder1_txtTotalSales").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTotalSales").val();
            var discountedAmountTP = $("#ContentPlaceHolder1_txtTPDiscountedAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtTPDiscountedAmount").val();

            if (parseFloat(discountedAmountTP) != 0)
                calculatedDiscountAmount = parseFloat(salesAmountTP) - parseFloat(discountedAmountTP);

            if (discountType == 'Percentage' && parseFloat(discountAmount) == 100) {
                calculatedDiscountAmount = parseFloat(salesAmountTP) - parseFloat(discountedAmountTP);
            }

            var serviceCharge = $("#ContentPlaceHolder1_txtServiceChargeAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtServiceChargeAmount").val();
            var vatAmount = $("#ContentPlaceHolder1_txtVatAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtVatAmount").val();
            var customerName = "";
            var paxQuantity = 1;
            var sourceInfoId = $("#ContentPlaceHolder1_txtKotIdInformation").val() == "" ? "0" : $("#ContentPlaceHolder1_txtKotIdInformation").val();
            var sourceName = "RestaurantToken";
            var tableId = $("#ContentPlaceHolder1_txtTableIdInformation").val();
            var kotId = $("#ContentPlaceHolder1_txtKotIdInformation").val();

            if (sourceInfoId == "")
                sourceInfoId = "0";

            if (sourceInfoId != "0") {
                tableId = sourceInfoId;
            }

            var billPaidBySourceId = tableId;
            var isInvoiceServiceChargeEnable = false, isInvoiceVatAmountEnable = false;

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

            var isRestaurantBillInclusive = $("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val() == "" ? "0" : $("#ContentPlaceHolder1_hfIsRestaurantBillInclusive").val();
            var salesAmount = "0", grandTotal = "0";


            if (isRestaurantBillInclusive == "0") {
                salesAmount = $("#ContentPlaceHolder1_txtSalesAmount").val();
                grandTotal = $("#ContentPlaceHolder1_txtGrandTotal").val();
            }
            else {
                salesAmount = $("#ContentPlaceHolder1_txtGrandTotal").val() == "" ? "0" : $("#ContentPlaceHolder1_txtGrandTotal").val();
                grandTotal = $("#ContentPlaceHolder1_txtSalesAmount").val() == "" ? "0" : $("#ContentPlaceHolder1_txtSalesAmount").val();

                grandTotal = parseFloat(salesAmount) - parseFloat(calculatedDiscountAmount);
            }
            
            var RestaurantBill = {
                CostCenterId: costcenterId,
                DiscountType: $.trim(discountType),
                IsComplementary: isComplementary,
                DiscountTransactionId: 0,
                DiscountAmount: discountAmount,
                CalculatedDiscountAmount: calculatedDiscountAmount,
                ServiceCharge: serviceCharge,
                VatAmount: vatAmount,
                CustomerName: customerName,
                PaxQuantity: paxQuantity,
                TableId: tableId,
                SourceName: sourceName,
                KotId: kotId,
                BillPaidBySourceId: tableId,
                IsInvoiceServiceChargeEnable: isInvoiceServiceChargeEnable,
                IsInvoiceVatAmountEnable: isInvoiceVatAmountEnable,
                SalesAmount: salesAmount,
                GrandTotal: grandTotal
            };

            var GuestBillPayment = new Array();

            var txtTPDiscountedAmountHiddenFieldVal = $("#ContentPlaceHolder1_txtTPDiscountedAmount").val();

            var txtCashVal = $("#ContentPlaceHolder1_txtCash").val();
            var txtAmexCardVal = $("#ContentPlaceHolder1_txtAmexCard").val();
            var txtMasterCardVal = $("#ContentPlaceHolder1_txtMasterCard").val();
            var txtVisaCardVal = $("#ContentPlaceHolder1_txtVisaCard").val();
            var txtDiscoverCardVal = $("#ContentPlaceHolder1_txtDiscoverCard").val();
            var txtTPDiscountAmountVal = $("#ContentPlaceHolder1_txtTPDiscountAmount").val();

            var hfLocalCurrencyId = '<%=hfLocalCurrencyId.ClientID%>'
            var hfLocalCurrencyIdVal = $('#' + hfLocalCurrencyId).val();

            var DiscountTypeVal = 'Fixed'

            if ($("#ContentPlaceHolder1_rbTPFixedDiscount").is(':checked')) {
                DiscountTypeVal = "Fixed";
            }
            else if ($("#ContentPlaceHolder1_rbTPPercentageDiscount").is(':checked')) {
                DiscountTypeVal = "Percentage";
            }

            var PaymentMode = new Array();
            PaymentMode.push({ CardType: '', PaymentMode: 'Cash', Control: "ContentPlaceHolder1_txtCash" });
            PaymentMode.push({ CardType: 'a', PaymentMode: 'Card', Control: "ContentPlaceHolder1_txtAmexCard" });
            PaymentMode.push({ CardType: 'm', PaymentMode: 'Card', Control: "ContentPlaceHolder1_txtMasterCard" });
            PaymentMode.push({ CardType: 'v', PaymentMode: 'Card', Control: "ContentPlaceHolder1_txtVisaCard" });
            PaymentMode.push({ CardType: 'd', PaymentMode: 'Card', Control: "ContentPlaceHolder1_txtDiscoverCard" });

            var paymodeCount = PaymentMode.length, row = 0;
            var paymentAmount = "0";

            for (row = 0; row < paymodeCount; row++) {

                if ($("#" + PaymentMode[row].Control).val() != "" && $("#" + PaymentMode[row].Control).val() != "0") {
                    paymentAmount = $("#" + PaymentMode[row].Control).val();

                    if (paymentAmount == "" || paymentAmount == "0")
                        continue;

                    GuestBillPayment.push({
                        NodeId: 0,
                        PaymentType: "Advance",
                        AccountsPostingHeadId: 0,
                        BillPaidBy: 0,
                        BankId: 0,
                        RegistrationId: 0,
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
                        PaymentDescription: ""
                    });
                }
                else if ($("#ContentPlaceHolder1_hfIsResumeBill").val() == "1") {
                    paymentAmount = "0";

                    if (paymentAmount == "" || paymentAmount == "0")
                        continue;

                    GuestBillPayment.push({
                        NodeId: 0,
                        PaymentType: "Advance",
                        AccountsPostingHeadId: 0,
                        BillPaidBy: 0,
                        BankId: 0,
                        RegistrationId: 0,
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
                        PaymentDescription: ""
                    });
                }
            }


            if ($("#ContentPlaceHolder1_txtTPRoundedAmount").val() != "0" && $("#ContentPlaceHolder1_txtTPRoundedAmount").val() != "") {

                var roundedAmount = $("#ContentPlaceHolder1_txtTPRoundedAmount").val();

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
                        PaymentDescription: ""
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
                            PaymentDescription: ""
                        });
                    }
                }
            }

            if (settlementType == "settlement") {
                if ($("#ContentPlaceHolder1_hfIsResumeBill").val() == "1") {
                    //PageMethods.UpdateRestauranBillGeneration(RestaurantBill, GuestBillPayment, OnRestaurantBillSaveSucceed, OnRestaurantBillSaveFailed);

                    $.ajax({
                        type: "POST",
                        url: "../../../Common/WebMethodPage.aspx/UpdateRestauranBillGeneration",
                        data: JSON.stringify({ RestaurantBill: RestaurantBill, GuestBillPayment: GuestBillPayment }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {

                            $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                            $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val(result.Pk);
                            IsBillOnProcess = "0";

                            if (result.d.IsDirectPrint == false) {

                                $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);
                                $('#btnPrintPreview').trigger('click');
                                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);

                            }
                            else {
                                window.location = result.d.RedirectUrl;
                            }
                        },
                        error: function (error) {
                            toastr.error("error");
                        }
                    });

                }
                else {

                    //PageMethods.SaveRestauranBillGeneration(RestaurantBill, GuestBillPayment, OnRestaurantBillSaveSucceed, OnRestaurantBillSaveFailed);
                    $.ajax({
                        type: "POST",
                        url: "../../../Common/WebMethodPage.aspx/SaveRestauranBillGeneration",
                        data: JSON.stringify({ RestaurantBill: RestaurantBill, GuestBillPayment: GuestBillPayment }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {

                            $("#ContentPlaceHolder1_btnSave").attr("disabled", true);
                            $("#ContentPlaceHolder1_hfBillIdForInvoicePreview").val(result.Pk);
                            IsBillOnProcess = "0";

                            if (result.d.IsDirectPrint == false) {

                                $("#ContentPlaceHolder1_hfBillId").val(result.d.Pk);
                                $('#btnPrintPreview').trigger('click');
                                $("#ContentPlaceHolder1_btnSave").attr("disabled", true);

                            }
                            else {
                                window.location = result.d.RedirectUrl;
                            }
                        },
                        error: function (error) {
                            toastr.error("error");
                        }
                    });

                }
            }
            else if (settlementType == "holdup") {
                if ($("#ContentPlaceHolder1_hfIsResumeBill").val() == "1") {

                    $.ajax({
                        type: "POST",
                        url: "../../../Common/WebMethodPage.aspx/UpdateHoldUpRestauranBillGeneration",
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
                        error: function (error) {
                            toastr.error("error");
                        }
                    });

                    //PageMethods.UpdateHoldUpRestauranBillGeneration(RestaurantBill, GuestBillPayment, OnRestaurantBillHoldupSucceed, OnRestaurantBillSaveFailed);
                }
                else {

                    $.ajax({
                        type: "POST",
                        url: "../../../Common/WebMethodPage.aspx/HoldUpRestauranBillGeneration",
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
                        error: function (error) {
                            toastr.error("error");
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

            $("#TActionDecider").dialog({
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

        function CloseClearActionDecider() {
            $("#TActionDecider").dialog("close");
            return false;
        }

        function ClearOrderedItem() {
            if ($("#TableWiseItemInformation tbody tr").length == 0) {
                toastr.info("No Item Added To Clear.");
                return;
            }

            if (!confirm("Do you want to clear added Item ?")) {
                return false;
            }

            var costcenterId = $("#ContentPlaceHolder1_hfCostCenterId").val();
            var kotId = $("#ContentPlaceHolder1_txtKotIdInformation").val();

            PageMethods.ClearOrderedItem(costcenterId, kotId, OnClearOrderedItemSucceed, OnClearOrderedItemFailed);
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

            if (!confirm("Do you want to back ?")) {
                return false;
            }

            $("#TActionDecider").dialog("close");
            window.location = "frmCostCenterSelection.aspx";
            return false;
        }

        function ClearWithPopUpInvoicePreview() {
            $("#ltlTableWiseItemInformation").html('');

            $('#CategoryItemInformationDiv').hide();
            $('#MenuInformationDiv').hide();

            $("#<%=txtSalesAmount.ClientID %>").val('0');
            $("#<%=txtDiscountedAmount.ClientID %>").val('0');
            $("#<%=txtServiceCharge.ClientID %>").val('0');
            $("#<%=txtVatAmount.ClientID %>").val('0');
            $("#<%=txtGrandTotal.ClientID %>").val('0');
            $("#<%=txtTotalPaymentAmount.ClientID %>").val('0');
            $("#<%=txtChangeAmount.ClientID %>").val('0');
            $("#<%=txtTotalSales.ClientID %>").val('0');
            $("#<%=txtSalesAmountHiddenField.ClientID %>").val('0');

            //$("#ItemWiseSpecialRemarks").html('');

            $('#ContentPlaceHolder1_pnlSettlementAndPaymentButtonDiv').hide();
            $('#ContentPlaceHolder1_pnlHomeButtonInfo').show();
        }
        function OnRestaurantBillSaveFailed() { CommonHelper.SpinnerClose(); IsBillOnProcess = "0"; }

        function LoadCategoryNItem(itemLoadType, valuePath) {
            $("#ContentPlaceHolder1_hfValuePath").val(itemLoadType + "," + valuePath);
            $("#ContentPlaceHolder1_btnLoadItemCategory").trigger("click");
        }

    </script>
    <asp:HiddenField ID="hfVat" runat="server" />
    <asp:HiddenField ID="hfSc" runat="server" />
    <asp:HiddenField ID="hfBillTemplate" runat="server" />
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfLocalCurrencyId" runat="server" />
    <asp:HiddenField ID="hfIsResumeBill" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsCategoryWiseDiscountAlreadyLoad" runat="server" Value="0">
    </asp:HiddenField>
    <asp:HiddenField ID="hfIsCategoryWiseDiscountCalculatedByUser" runat="server" Value="0">
    </asp:HiddenField>
    <asp:HiddenField ID="hfKotDetailId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="SourceIdHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCostCenterId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfBillId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRestaurantBillInclusive" runat="server" />
    <asp:HiddenField ID="hfBillIdForInvoicePreview" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRestaurantIntegrateWithFrontOffice" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSelectedTableNumber" runat="server" />
    <asp:HiddenField ID="hfSelectedTableId" runat="server" />
    <asp:HiddenField ID="hfAlreadyAddedTable" runat="server" />
    <asp:HiddenField ID="hfAlreadyAddedKotId" runat="server" />
    <asp:HiddenField ID="hfNewlyAddedTableId" runat="server" />
    <asp:HiddenField ID="hfNewlyAddedKotId" runat="server" />
    <asp:HiddenField ID="hfDeletedTableId" runat="server" />
    <asp:HiddenField ID="hfDeletedKotId" runat="server" />
    <asp:HiddenField ID="hfRestaurantVatAmount" runat="server" />
    <asp:HiddenField ID="hfRestaurantServiceCharge" runat="server" />
    <asp:HiddenField ID="hfIsVatServiceChargeEnable" runat="server" />
    <asp:HiddenField ID="hfCategoryWiseTotalDiscountAmount" runat="server" />
    <asp:HiddenField ID="hfIsRestaurantBillAmountWillRound" runat="server" />
    <asp:HiddenField ID="txtCalculatedDiscountAmount" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfValuePath" runat="server" Value=""></asp:HiddenField>
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
    <iframe id="IframeReportPrint" name="IframeReportPrint" width="0" height="0" runat="server"
        style="left: -1000; top: 2000;" clientidmode="static" scrolling="yes"></iframe>
    <div id="displayBill" style="display: none;">
        <div class="well well-small" style="text-align: right">
            <input type="button" class="btn btn-primary btn-large" value="Close" onclick="ClosePrintPreviewDialog()" />
        </div>
        <iframe id="printDoc" name="printDoc" width="500" height="650" frameborder="0" style="overflow: hidden;">
        </iframe>
        <div id="bottomPrint">
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
    <div id="PercentageDiscountDialog" style="width: 450px; display: none;">
        <div class="">
            <div class="DataGridYScroll">
                <div id="PercentageDiscountContainer" runat="server" clientidmode="Static" style="width: 100%;">
                </div>
            </div>
            <div id="Div3" style="padding-left: 5px; width: 100%; margin-top: 5px;">
                <div style="float: left; padding-bottom: 15px;">
                    <input type="button" id="btnCategoryWiseDiscountOK" class="TransactionalButton btn btn-primary"
                        style="width: 80px;" value="Ok" />
                    <input type="button" id="btnCategoryWiseDiscountCancel" class="TransactionalButton btn btn-primary"
                        style="width: 80px;" value="Cancel" />
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <div id="TouchKeypad" style="display: none;">
        <div id="TouchKeypadResultDiv">
            <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
            <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
            <asp:TextBox ID="txtTouchKeypadResult" runat="server" CssClass="TouchKeypadResult"
                Height="40px" Font-Size="50px"></asp:TextBox>
        </div>
        <div class='divClear'>
        </div>
        <div class='block-body collapse in'>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img6' onclick='myFunctionTouchKeypad(0)' class="TouchNumericItemImageDiv"
                        src="/Images/0.jpg" /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img7' onclick='myFunctionTouchKeypad(1)' class="TouchNumericItemImageDiv"
                        src='/Images/1.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img10' onclick='myFunctionTouchKeypad(2)' class="TouchNumericItemImageDiv"
                        src='/Images/2.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img11' onclick='myFunctionTouchKeypad(3)' class="TouchNumericItemImageDiv"
                        src='/Images/3.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img12' onclick='myFunctionTouchKeypad(4)' class="TouchNumericItemImageDiv"
                        src='/Images/4.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img13' onclick='myFunctionTouchKeypad(5)' class="TouchNumericItemImageDiv"
                        src='/Images/5.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img14' onclick='myFunctionTouchKeypad(6)' class="TouchNumericItemImageDiv"
                        src='/Images/6.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img15' onclick='myFunctionTouchKeypad(7)' class="TouchNumericItemImageDiv"
                        src='/Images/7.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img16' onclick='myFunctionTouchKeypad(8)' class="TouchNumericItemImageDiv"
                        src='/Images/8.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img17' onclick='myFunctionTouchKeypad(9)' class="TouchNumericItemImageDiv"
                        src='/Images/9.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img18' onclick='myFunctionTouchKeypad(99)' class="TouchNumericItemImageDiv"
                        src='/Images/Backspace.jpg' /></div>
            </div>
            <div class='TouchDivNumericContainer'>
                <div class='TouchNumericItemDiv'>
                    <img id='Img19' onclick='javascript:return PerformUpdateActionTouchKeypad()' class="TouchNumericItemImageDiv"
                        src='/Images/Ok.jpg' /></div>
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
                    <asp:Label ID='Label7' Font-Bold="True" runat="server" ClientIDMode="Static"></asp:Label>
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
    <div id="RestaurantPaymentInformationDiv" style="display: none;">
        <div class="row-fluid" style="background-color: #fff;">
            <div class="span6" style="border: 1px solid #ccc; min-height: 80vh; padding-top: 5px;">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblTotalSales" runat="server" Text="Total Sales"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtTotalSales" TabIndex="1" runat="server" ReadOnly="True"></asp:TextBox>
                        <asp:HiddenField ID="txtTPSalesAmountHiddenField" runat="server" />
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="Label1" runat="server" Text="Discount Type"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight" style="padding-bottom: 10px; width: 32%;">
                        <asp:RadioButton ID="rbTPFixedDiscount" runat="server" GroupName="DiscountType" Checked="True" />
                        <asp:Label ID="Label12" runat="server" Text="Fixed"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="rbTPPercentageDiscount" runat="server" GroupName="DiscountType" />
                        <asp:Label ID="Label2" runat="server" Text="Percentage"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="rbTPComplementaryDiscount" runat="server" GroupName="DiscountType" />
                        <asp:Label ID="Label10" runat="server" Text="Complementary"></asp:Label>
                        <span style="margin-left: 5px; display: none;" id="CategoryWiseDiscountPercentageList">
                            <img border="0" alt="Category Wise Discount" onclick="javascript:return LoadPercentageDiscountInfo()"
                                style="cursor: pointer; display: inline;" title="Category Wise Discount" src="../Images/discount.png"
                                id="ImgCategoryWiseDiscount" />
                        </span>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblTPDiscountAmountDisplay" runat="server" Text="Discount Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtTPDiscountAmount" TabIndex="1" runat="server" Text="0" onblur="CalculateDiscountAmount()"
                            Width="68px"></asp:TextBox>
                        <asp:TextBox ID="txtTPDiscount" TabIndex="1" runat="server" Text="0" Width="120px"
                            ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="Label8" runat="server" Text="Discounted Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtTPDiscountedAmount" TabIndex="1" runat="server" ReadOnly="True"
                            CssClass="Custom90PercentNormalTextBox"></asp:TextBox>
                        <asp:HiddenField ID="txtTPDiscountedAmountHiddenField" runat="server" />
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="Label5" runat="server" Text="Service Charge"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtTPServiceCharge" runat="server" TabIndex="22" CssClass="Custom90PercentNormalTextBox"
                            Enabled="false"></asp:TextBox>
                        <asp:HiddenField ID="txtTPServiceChargeAmountHiddenField" runat="server"></asp:HiddenField>
                        <asp:CheckBox ID="cbTPServiceCharge" runat="server" Text="" CssClass="customChkBox"
                            onclick="javascript: return ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge(this);"
                            TabIndex="8" Checked="True" />
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="Label6" runat="server" Text="Vat Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtTPVatAmount" runat="server" TabIndex="23" CssClass="Custom90PercentNormalTextBox"
                            Enabled="false"></asp:TextBox>
                        <asp:HiddenField ID="txtTPVatAmountHiddenField" runat="server"></asp:HiddenField>
                        <asp:CheckBox ID="cbTPVatAmount" runat="server" Text="" CssClass="customChkBox" onclick="javascript: return ToggleTotalSalesAmountVatServiceChargeCalculationForVat(this);"
                            TabIndex="8" Checked="True" /></div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblCash" runat="server" Text="Cash"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtCash" TabIndex="1" runat="server" onblur="CalculatePayment()"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblAmexCard" runat="server" Text="Amex Card"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtAmexCard" TabIndex="2" runat="server" onblur="CalculatePayment()"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblMasterCard" runat="server" Text="Master Card"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtMasterCard" TabIndex="3" runat="server" onblur="CalculatePayment()"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblVisaCard" runat="server" Text="Visa Card"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtVisaCard" TabIndex="4" runat="server" onblur="CalculatePayment()"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblDiscoverCard" runat="server" Text="Discover Card"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtDiscoverCard" TabIndex="5" runat="server" onblur="CalculatePayment()"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection" style="display: none;">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblTPRoundedAmount" runat="server" Text="Rounded Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtTPRoundedAmount" TabIndex="5" runat="server" onblur="CalculatePayment()"></asp:TextBox>
                        <asp:HiddenField ID="txtTPRoundedAmountHiddenField" runat="server" />
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection" style="display: none;">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblPOSTerminal" runat="server" Text="POS Terminal"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:Literal ID="ltlPOSTerminalInformation" runat="server"> </asp:Literal>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
            <div class="span6" style="border: 1px solid #ccc; min-height: 81.7vh; padding-top: 5px;">
                <div class="row-fluid">
                    <div class="span12">
                        <div style="margin-left: 30px;">
                            <div class='TouchDivNumericContainer'>
                                <%-- <a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='Img5' onclick='myFunction(7)' class="TouchNumericItemImageDiv" src='/Images/7.jpg' /></div>
                                <%-- </a>--%>
                            </div>
                            <div class='TouchDivNumericContainer'>
                                <%--<a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='Img5' onclick='myFunction(8)' class="TouchNumericItemImageDiv" src='/Images/8.jpg' /></div>
                                <%-- </a>--%>
                            </div>
                            <div class='TouchDivNumericContainer'>
                                <%-- <a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='Img9' onclick='myFunction(9)' class="TouchNumericItemImageDiv" src='/Images/9.jpg' /></div>
                                <%--  </a>--%>
                            </div>
                            <div class='TouchDivNumericContainer'>
                                <%--<a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='Img2' onclick='myFunction(4)' class="TouchNumericItemImageDiv" src='/Images/4.jpg' /></div>
                                <%--</a>--%>
                            </div>
                            <div class='TouchDivNumericContainer'>
                                <%--<a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='Img3' onclick='myFunction(5)' class="TouchNumericItemImageDiv" src='/Images/5.jpg' /></div>
                                <%--</a>--%>
                            </div>
                            <div class='TouchDivNumericContainer'>
                                <%--<a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='Img4' onclick='myFunction(6)' class="TouchNumericItemImageDiv" src='/Images/6.jpg' /></div>
                                <%--</a>--%>
                            </div>
                            <div class='TouchDivNumericContainer'>
                                <%-- <a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='ContentPlaceHolder1_img2' onclick='myFunction(1)' class="TouchNumericItemImageDiv"
                                        src='/Images/1.jpg' /></div>
                                <%--</a>--%>
                            </div>
                            <div class='TouchDivNumericContainer'>
                                <%-- <a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='ContentPlaceHolder1_img3' onclick='myFunction(2)' class="TouchNumericItemImageDiv"
                                        src='/Images/2.jpg' /></div>
                                <%--</a>--%>
                            </div>
                            <div class='TouchDivNumericContainer'>
                                <%-- <a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='Img1' onclick='myFunction(3)' class="TouchNumericItemImageDiv" src='/Images/3.jpg'
                                        alt='3' /></div>
                                <%--</a>--%>
                            </div>
                            <div class='TouchDivNumericContainer'>
                                <%--<a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='ContentPlaceHolder1_img1' onclick='myFunction(0)' class="TouchNumericItemImageDiv"
                                        src="/Images/0.jpg" /></div>
                                <%--</a>--%>
                            </div>
                            <div class='TouchDivNumericContainer'>
                                <%--<a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='ImgDot' onclick='myFunction(11)' class="TouchNumericItemImageDiv" src='/Images/TPDotIcon.jpg'
                                        alt='Dot' /></div>
                                <%-- </a>--%>
                            </div>
                            <div class='TouchDivNumericContainer'>
                                <%--<a onclick='myFunction()' href='#'>--%>
                                <div class='TouchNumericItemDiv'>
                                    <img id='Img8' onclick='myFunction(10)' class="TouchNumericItemImageDiv" src='/Images/Backspace.jpg' /></div>
                                <%-- </a>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class='block-body collapse in'>
                    <div id="ButtonInformationDiv">
                    </div>
                    <div class="divClear">
                    </div>
                    <div style="padding-top: 05px;">
                        <div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="Label3" runat="server" Text="Grand Total"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtTPGrandTotal" TabIndex="5" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                    <asp:HiddenField ID="txtTPGrandTotalHiddenField" runat="server" />
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="Label4" runat="server" Text="Total Payment"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtTPTotalPaymentAmount" TabIndex="5" runat="server" Width="200px"
                                    ReadOnly="True"></asp:TextBox>
                                <asp:HiddenField ID="txtTPTotalPaymentAmountHiddenField" runat="server" />
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblTPChangeAmount" runat="server" Text="Change Amount"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtTPChangeAmount" TabIndex="5" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                <asp:HiddenField ID="txtTPChangeAmountHiddenField" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div style="text-align: right; padding-left: 127px;">
                        <div class='TouchDivNumericContainer'>
                            <%--<a onclick='myFunction()' href='#'>--%>
                            <div class='TouchNumericItemDiv'>
                                <img id='ImgClean' onclick='myFunction(12)' class="TouchNumericItemImageDiv" src='/Images/TPClean.jpg'
                                    alt='Clear' /></div>
                            <%-- </a>--%>
                        </div>
                        <div class='TouchDivNumericContainer'>
                            <%--<a onclick='myFunction()' href='#'>--%>
                            <div class='TouchNumericItemDiv'>
                                <%--<asp:ScriptManager ID="scriptManagerClist" runat="server" EnablePageMethods="true">
                                        </asp:ScriptManager>--%>
                                <img id='ImgOk' onclick='javascript:return PerformTPOkButton()' class="TouchNumericItemImageDiv"
                                    src='/Images/Ok.jpg' /></div>
                            <%--</a>--%>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div>
        <div class="span3" style="margin-top: 3px; margin-left: 1px; margin-right: 1px; padding: 0px;
            width: 21.07%;">
            <div id="MenuInformationDiv">
                <a href="javascript:void()" class="block-heading"><span style="color: #ffffff;">Menu
                    Information </span></a>
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
                <div id="ltlMenuInformation" style="min-height: 81vh;">
                    <div style="height: 80.5vh; overflow: scroll;">
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
        <div class="span5" style="margin-top: 3px; margin-left: 1px; margin-right: 1px; padding: 0px;
            width: 46.7%;">
            <div id="CategoryItemInformationDiv" style="min-height: 86vh; border: 1px solid #ccc;
                overflow: hidden;">
                <div style="height: 86vh; overflow-y: scroll;">
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
        <div class="span4" style="margin-top: 3px; margin-left: 1px; margin-right: 1px; padding: 0px;">
            <div>
                <a href="javascript:void()" class="block-heading"><span style="color: #ffffff;">Order
                    Item Information </span></a>
                <asp:HiddenField ID="hfltlTableWiseItemInformationDivHeight" runat="server"></asp:HiddenField>
                <div id="ltlTableWiseItemInformation" style="overflow: scroll;">
                </div>
            </div>
            <div class="divClear">
            </div>
            <div style="display: none;">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblSalesAmount" runat="server" Text="Sales Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:HiddenField ID="txtSalesAmountHiddenField" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtSalesAmount" TabIndex="4" runat="server" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblDiscountedAmount" runat="server" Text="After Discount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:HiddenField ID="txtDiscountedAmountHiddenField" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtDiscountedAmount" TabIndex="4" runat="server" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
            <asp:Panel ID="pnlRackRateServiceChargeVatInformation" runat="server">
                <asp:Panel ID="pnlServiceChargeInformation" runat="server">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblServiceCharge" runat="server" Text="Service Charge"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtServiceCharge" runat="server" TabIndex="22" Enabled="false"></asp:TextBox>
                            <asp:HiddenField ID="txtServiceChargeAmountHiddenField" runat="server"></asp:HiddenField>
                            <div style="display: none;">
                                <asp:CheckBox ID="cbServiceCharge" runat="server" Text="" CssClass="customChkBox"
                                    onclick="javascript: return ToggleTotalSalesAmountVatServiceChargeCalculationForServiceCharge(this);"
                                    TabIndex="8" Checked="True" />
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlVatInformation" runat="server">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="23" Enabled="false"></asp:TextBox>
                            <asp:HiddenField ID="txtVatAmountHiddenField" runat="server"></asp:HiddenField>
                            <div style="display: none;">
                                <asp:CheckBox ID="cbVatAmount" runat="server" Text="" CssClass="customChkBox" onclick="javascript: return ToggleTotalSalesAmountVatServiceChargeCalculationForVat(this);"
                                    TabIndex="8" Checked="True" /></div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                </asp:Panel>
            </asp:Panel>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:HiddenField ID="txtGrandTotalHiddenField" runat="server"></asp:HiddenField>
                    <asp:TextBox ID="txtGrandTotal" TabIndex="4" runat="server" Enabled="False"></asp:TextBox>
                </div>
            </div>
            <div id="NetAmountDivInfo" runat="server">
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblNetAmount" runat="server" Text="Grand Total"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:HiddenField ID="txtNetAmountHiddenField" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtNetAmount" TabIndex="4" runat="server" Enabled="False"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div style="display: none;">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblTotalPaymentAmount" runat="server" Text="Total Payment"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtTotalPaymentAmount" TabIndex="5" runat="server" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="Label9" runat="server" Text="Rounded Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtRoundedAmount" TabIndex="5" runat="server" Enabled="False"></asp:TextBox>
                        <asp:HiddenField ID="txtRoundedAmountHiddenField" runat="server" />
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblTotalChangeAmount" runat="server" Text="Due Amount"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtChangeAmount" TabIndex="5" runat="server" ReadOnly="True"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div style="padding-top: 10px;">
                <%--Right Left--%>
                <asp:Panel ID="pnlSettlementAndPaymentButtonDiv" runat="server">
                    <input type="button" id="btnPaymentInfo" runat="server" onclick="LoadPaymentInformation()"
                        style="padding: 8px 5px; width: 90px;" class="btn btn-primary" value="Payment" />
                    <asp:Button ID="btnSave" runat="server" Text="Settlement" CssClass="TransactionalButton btn btn-primary"
                        Style="padding: 8px 5px; width: 90px;" TabIndex="23" OnClientClick="javascript: return ValidateFormBeforeSettlement('settlement');" />
                    <asp:Button ID="btnClear" runat="server" TabIndex="24" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                        Style="padding: 8px 5px; width: 90px;" OnClientClick="javascript: return ClearOrderedItem();" />
                    <asp:Button ID="btnMoreOptions" runat="server" TabIndex="24" Text="More" CssClass="TransactionalButton btn btn-primary"
                        Style="padding: 8px 5px; width: 90px;" OnClientClick="javascript: return OpenTDecider();" />
                </asp:Panel>
                <asp:Panel ID="pnlHomeButtonInfo" runat="server">
                    <div id="Div1" class="btn-toolbar;" style="text-align: right;">
                        <asp:ImageButton ID="ImageButton1" CssClass="btnBackPreviousPage" runat="server"
                            ImageUrl="~/Images/Home.png" OnClientClick="javascript:return GoToHomePanel()"
                            ToolTip="Home" />
                        &nbsp;
                        <div class="btn-group">
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <div id="TActionDecider" style="display: none;">
        <div class="row-fluid">
            <div class="span12">
                <div class="row-fluid" style="margin-bottom: 15px;" id="billPreviewForBill">
                    <div class="span6">
                        <input type="button" id="btnBillPreview" class="TransactionalButton btn btn-primary"
                            style="width: 150px; height: 45px;" value="Bill Preview" onclick="BillPreview()" />
                    </div>
                </div>
                <div class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                        <input type="button" id="btnBillHolpup" class="TransactionalButton btn btn-primary"
                            style="width: 150px; height: 45px;" value="Holdup" onclick="ValidateFormBeforeSettlement('holdup')" />
                    </div>
                </div>
               <%-- <div class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                        <input type="button" id="btnOrderSubmit" class="TransactionalButton btn btn-primary"
                            style="width: 150px; height: 45px;" value="Order Submit" onclick="KotSubmit()" />
                    </div>
                </div>--%>
                <div class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                        <input type="button" id="btnBackCostCenterSelection" class="TransactionalButton btn btn-primary"
                            style="width: 150px; height: 45px;" value="Back To Cost Center" onclick="BackCostCenterSelection()" />
                    </div>
                </div>
                <div class="row-fluid" style="margin-bottom: 15px;">
                    <div class="span6">
                        <input type="button" id="btnCloseClearActionDecider" class="TransactionalButton btn btn-primary"
                            style="width: 150px; height: 45px;" value="Close" onclick="CloseClearActionDecider()" />
                    </div>
                </div>
                <%--
                <asp:Button ID="" runat="server" Text="Holdup" CssClass="TransactionalButton btn btn-primary"
                        Style="padding: 4px 4px; width: 70px;" TabIndex="23" OnClientClick="javascript: return ValidateFormBeforeSettlement('holdup');" />
                    <asp:Button ID="" runat="server" TabIndex="24" Text="Order Submit"
                        Style="padding: 4px 4px; width: 90px;" CssClass="TransactionalButton btn btn-primary"
                        OnClientClick="javascript: return KotSubmit();" />--%>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransactionShow.ClientID %>"));
                var printTemplate = $("#<%=hfBillTemplate.ClientID %>").val();

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 25px; width: 50px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(' + printTemplate + '); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }

            $("#ltlTableWiseItemInformation").height($("#<%=hfltlTableWiseItemInformationDivHeight.ClientID %>").val());
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
   
    </script>
</asp:Content>
