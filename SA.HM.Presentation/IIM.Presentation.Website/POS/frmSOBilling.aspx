<%@ Page Title="" Language="C#" MasterPageFile="~/POS/RestaurantMM.Master" AutoEventWireup="true"
    CodeBehind="frmSOBilling.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmSOBilling" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var orderItemTr = "";
        var remarksTr = "";
        var GuestPayment = new Array();
        var PaymentRowId = 0;
        var hfExtraButtonShowHide = 0;
        $('#QuantityChangeContainer').hide();
        $('#ItemChangeContainer').hide();
        $('#UnitPriceChangeContainer').hide();
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Point Of Sales</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Pay First Bill</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            CommonHelper.ApplyDecimalValidation();
            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var ddlCurrencyId = $("#<%=ddlCurrency.ClientID %>").val();
            PageMethods.LoadCurrencyType(ddlCurrencyId, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);

            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'

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

            $('#' + ddlCurrency).change(function () {
                var v = $("#<%=ddlCurrency.ClientID %>").val();
                PageMethods.LoadCurrencyType(v, OnLoadCurrencyTypeSucceeded, OnLoadCurrencyTypeFailed);
            });

            function OnLoadCurrencyTypeSucceeded(result) {
                $("#<%=hfCurrencyType.ClientID %>").val(result.CurrencyType);
                PageMethods.LoadCurrencyConversionRate(result.CurrencyId, OnLoadConversionRateSucceeded, OnLoadConversionRateFailed);
            }

            function OnLoadCurrencyTypeFailed() {

            }

            function OnLoadConversionRateSucceeded(result) {
                if ($("#<%=hfCurrencyType.ClientID %>").val() == "Local") {
                    $("#<%=txtConversionRate.ClientID %>").val('');
                    $("#<%=txtConvertedAmount.ClientID %>").val('');
                    $('#ConversionRateDivInformation').hide();
                }
                else {
                    $("#<%=txtConversionRate.ClientID %>").val(result.ConversionRate);
                    $("#<%=hfConversionRate.ClientID %>").val(result.ConversionRate);
                    $('#ConversionRateDivInformation').show();
                }
            }

            function OnLoadConversionRateFailed() {
            }

            $("#<%=txtReceiveLeadgerAmount.ClientID %>").blur(function () {
                CalculateConvertedAmount();
            });
            $("#<%=txtConversionRate.ClientID %>").blur(function () {
                CalculateConvertedAmount();
            });

            $("#CmpButton").hide();
            hfExtraButtonShowHide = $("#<%=hfExtraButtonShowHide.ClientID %>").val();
            //            if (hfExtraButtonShowHide == 1) {
            //                alert(hfExtraButtonShowHide);
            //                $("ContentPlaceHolder1_btnChallan").show();
            //            }
            //            else {
            //                $("ContentPlaceHolder1_btnChallan").hide();
            //            }

            var paymentMode = '<%=ddlPayMode.ClientID%>'
            $('#' + paymentMode).change(function () {
                if ($('#' + paymentMode).val() == 2) {
                    $('#CardPaymentAccountHeadDiv').show();
                    $("#<%=ddlCardType.ClientID %>").val(0);
                    $('#CompanyDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $("#CmpButton").hide();
                }
                else if ($('#' + paymentMode).val() == 3) {
                    $('#ChecquePaymentAccountHeadDiv').show();
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#CompanyDiv').hide();
                    $("#CmpButton").hide();
                }
                else if ($('#' + paymentMode).val() == 4) {
                    $('#CompanyDiv').show();
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $("#CmpButton").hide();
                }
                else if ($('#' + paymentMode).val() == 5) {
                    $('#CompanyDiv').show();
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $("#CmpButton").hide();
                }
                else {
                    $('#CardPaymentAccountHeadDiv').hide();
                    $('#CompanyDiv').hide();
                    $('#ChecquePaymentAccountHeadDiv').hide();
                    $("#CmpButton").hide();
                }
            });

            $("#ContentPlaceHolder1_ddlCategory").change(function () {
                $("#<%=txtItemName.ClientID %>").val("");
                $("#<%=txtUnitPrice.ClientID %>").val("");
                $("#<%=txtQuantity.ClientID %>").val("");
                $("#<%=ddlStockBy.ClientID %>").val(0);
            });

            $("#txtItemName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../PointOfSales/frmBilling.aspx/ItemNCategoryAutoSearch',
                        data: "{'itemName':'" + request.term + "', costcenterId: '" + $("#<%=ddlCostCenterId.ClientID %>").val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    StockBy: m.StockBy,
                                    StockQuantity: m.StockQuantity,
                                    UnitPrice: m.UnitPrice
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
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#<%=hfItemId.ClientID %>").val(ui.item.value);
                    $("#ContentPlaceHolder1_ddlStockBy").val(ui.item.StockBy);
                    $("#ContentPlaceHolder1_txtStockQuantity").val('Stock: ' + ui.item.StockQuantity);
                    $("#ContentPlaceHolder1_txtUnitPrice").val(ui.item.UnitPrice);
                }
            });

            $("#btnAddItem").click(function () {
                var itemId = $("#<%=hfItemId.ClientID %>").val();
                if (itemId == "") {
                    toastr.warning('Please Select an Item.');
                    return false;
                }
                AddNewItem(itemId);
                $("#txtItemName").focus();
                $("#AppraisalAccordion").accordion({ active: 0 });
            });

            $("#btnAddDetailGuestPayment").click(function () {
                var paymentId = $("#<%=hfPaymentId.ClientID %>").val();
                AddNewPayment(paymentId);
                $("#AppraisalAccordion").accordion({ active: 1 });
            });

            $("#btnItemwiseSpecialRemarksSave").click(function () {

                $("#TableWiseItemRemarksInformation tbody tr").each(function (index, item) {
                    var remarksSelected = $(this).find('td:eq(0)').find("input");
                    if (remarksSelected.is(':checked')) {
                        //return false;
                    }
                });
                return false;
            });

            CommonHelper.AutoSearchClientDataSource("txtBankId", "ContentPlaceHolder1_ddlBankId", "ContentPlaceHolder1_ddlBankId");
            CommonHelper.AutoSearchClientDataSource("txtChequeBankId", "ContentPlaceHolder1_ddlChequeBankId", "ContentPlaceHolder1_ddlChequeBankId");
            CommonHelper.AutoSearchClientDataSource("txtCmpany", "ContentPlaceHolder1_ddlCompany", "ContentPlaceHolder1_ddlCompany");
            $("#ContentPlaceHolder1_btnSave").prop("disabled", true);
            $("#ContentPlaceHolder1_btnPreview").prop("disabled", true);
            $("#ContentPlaceHolder1_btnChallan").prop("disabled", true);

            $("#txtCmpany").change(function () {
                //$("#CmpButton").show(); 
                var companyId = $("#ContentPlaceHolder1_ddlCompany").val();
                //var grandTotal = $("#ContentPlaceHolder1_txtGrandTotal").val();
                //alert(companyId);
                CalculateTotalAmount();
                if (hfExtraButtonShowHide == 1) {
                    if (companyId >= 1) {
                        $("#CmpButton").show();
                    }
                    else $("#CmpButton").hide();
                }
                else {
                    $("#CmpButton").hide();
                }
            });

        });

        function CalculateConvertedAmount() {
            var currencyType = $("#<%=hfCurrencyType.ClientID %>").val();
            if (currencyType == "Local") {
                var LedgerAmount = parseFloat($("#<%=txtReceiveLeadgerAmount.ClientID %>").val());
                $("#<%=txtConvertedAmount.ClientID %>").val(LedgerAmount.toFixed(2));
            }
            else {
                var txtLedgerAmount = parseFloat($("#<%=txtReceiveLeadgerAmount.ClientID %>").val());
                var txtConversionRate = parseFloat($("#<%=txtConversionRate.ClientID %>").val());
                var LedgerAmount = txtLedgerAmount * txtConversionRate;
                if (isNaN(LedgerAmount.toString())) {
                    LedgerAmount = 0;
                }
                $("#<%=txtConvertedAmount.ClientID %>").val(LedgerAmount.toFixed(2));
                $("#<%=txtConvertedAmount.ClientID %>").attr("disabled", true);
            }
        }

        function AddNewItem(itemId) {
            var costCenterId = $("#ContentPlaceHolder1_ddlCostCenterId").val();
            var categoryId = $("#<%=ddlCategory.ClientID %>").val();
            var itemName = $("#<%=txtItemName.ClientID %>").val();
            var itemUnit = $("#<%=txtUnitPrice.ClientID %>").val();
            var quantity = $("#<%=txtQuantity.ClientID %>").val();
            var stockBy = $("#<%=ddlStockBy.ClientID %>").val();
            var total = parseFloat(itemUnit) * parseFloat(quantity);

            if (stockBy == 0) {
                toastr.warning('Please Select Stock Type.');
                return false;
            }

            if (costCenterId == 0) {
                toastr.warning('Please Select a Cost Center.');
                return false;
            }
            else if (itemId == 0) {
                toastr.warning('Please Select an Item.');
                return false;
            }

            if (itemName == "") {
                toastr.warning('Please Provide Item Name.');
                return false;
            }
            else if (quantity == "") {
                toastr.warning('Please Provide Item Quantity.');
                return false;
            }
            else if (itemUnit == "") {
                toastr.warning('Please Provide MRP.');
                return false;
            }

            var duplicate = 0;
            if ($("#ltlTableWiseItemInformation > table").length > 0) {
                $('#ItemInformation tbody tr').each(function () {
                    var existitemId = $(this).find("td:eq(0)").text();
                    if (existitemId == itemId) {
                        duplicate = 1;
                    }
                });
                if (duplicate == 1) {
                    toastr.warning('This Item already added. For any change edit or delete it.');
                    return false;
                }
                else {
                    AddNewRow(itemId, categoryId, itemName, itemUnit, quantity, total);
                    return false;
                }
            }

            var table = "", deleteLink = "";
            table += "<table id='ItemInformation' style='width: 100%;' class='table table-bordered table-condensed table-responsive'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            table += "<th style='display:none'>RowId</th><th style='display:none'>ItemId</th><th style='display:none'>CategoryId</th><th align='left' scope='col' style='width: 35%;'>Item Name</th>";
            table += "<th align='left' scope='col' style='width: 10%;'>Qty.</th>";
            table += "<th align='left' scope='col' style='width: 15%;'>MRP</th>";
            table += "<th align='left' scope='col' style='width: 20%;'>Total</th>";
            table += "<th align='center' scope='col' style='width: 20%;'>Action</th></tr></thead>";
            table += "<tbody>";
            table += "<tr style=\"background-color:#E3EAEB;\">";
            table += "<td align='left' style=\"display:none;\">" + itemId + "</td>";
            table += "<td align='left' style=\"display:none;\">" + categoryId + "</td>";
            table += "<td align='left' style=\"width:35%; text-align:Left;\">" + itemName + "</td>";
            table += "<td align='left' style=\"width:10%; text-align:Left;\">" + quantity + "</td>";
            table += "<td align='left' style='width: 15%;'>" + itemUnit + "</td>";
            table += "<td align='left' style='width: 20%;'>" + Math.round(total, 2) + "</td>";
            table += "<td align='center' style=\"width:20%; cursor:pointer;\">";
            //table += "<img alt=\"Edit\" src=\"../Images/edit.png\" onclick= 'EditItem(this)'/>";
            table += "&nbsp;<img alt=\"Delete\" src=\"../Images/delete.png\" onclick= 'DeleteItem(this)'/>";
            //table += "&nbsp;<img alt=\"Add Remarks\" src=\"../Images/remarksadd.png\" onclick= 'AddItemWiseRemarks(this)'/>";
            table += "</td>";
            table += "</tr>";
            table += "</tbody>";
            table += "</table>";

            $("#<%=hfItemId.ClientID %>").val(itemId);
            $("#ltlTableWiseItemInformation").html(table);
            $("#<%=txtItemName.ClientID %>").val("");
            $("#<%=txtUnitPrice.ClientID %>").val("");
            $("#<%=txtQuantity.ClientID %>").val("");
            $("#<%=ddlStockBy.ClientID %>").val(0);
            $("#<%=txtStockQuantity.ClientID %>").val("");
            $("#ContentPlaceHolder1_txtItemName").focus();
            TotalSalesAmountVatServiceChargeCalculation();
            //CalculateTotalAmount();           
        }

        function AddNewRow(itemId, categoryId, itemName, itemUnit, quantity, total) {
            var tr = "", totalRow = 0, deleteLink = "";
            totalRow = $("#ItemInformation tbody tr").length;

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }
            tr += "<td align='left' style=\"display:none;\">" + itemId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + categoryId + "</td>";
            tr += "<td align='left' style=\"width:35%; text-align:Left;\">" + itemName + "</td>";
            tr += "<td align='left' style='width: 10%;'>" + quantity + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itemUnit + "</td>";
            tr += "<td align='left' style='width: 20%;'>" + total + "</td>";
            tr += "<td align='center' style=\"width:20%; cursor:pointer;\">";
            tr += "<img alt=\"Delete\" src=\"../Images/edit.png\" onclick= 'EditItem(this)'/>";
            tr += "&nbsp;<img alt=\"Delete\" src=\"../Images/delete.png\" onclick= 'DeleteItem(this)'/>";
            tr += "&nbsp;<img alt=\"Add Remarks\" src=\"../Images/remarksadd.png\" onclick= 'AddItemWiseRemarks(this)'/>";
            tr += "</td>";
            tr += "</tr>";

            $("#ItemInformation tbody").append(tr);
            $("#<%=txtItemName.ClientID %>").val("");
            $("#<%=txtUnitPrice.ClientID %>").val("");
            $("#<%=txtQuantity.ClientID %>").val("");
            $("#<%=txtStockQuantity.ClientID %>").val("");
            $("#ContentPlaceHolder1_txtItemName").focus();
            TotalSalesAmountVatServiceChargeCalculation();
            CalculateTotalAmount();
        }

        function EditItem(item) {
            var itemId = $(item).parent().parent().find("td:eq(0)").text();
            $("#<%=hfItemRow.ClientID %>").val(itemId);
            $('#QuantityChangeContainer').hide();
            $('#ItemChangeContainer').hide();
            $('#UnitPriceChangeContainer').hide();
            $("#editServiceDecider").dialog({
                autoOpen: true,
                modal: true,
                width: 580,
                closeOnEscape: true,
                resizable: false,
                title: "Edit Item",
                show: 'slide'
            });
            return false;
        }

        function DeleteItem(anchor) {
            var tr = $(anchor).parent().parent();
            $(tr).remove();
            TotalSalesAmountVatServiceChargeCalculation();
            CalculateTotalAmount();
        }

        function QuantityChange() {
            $('#QuantityChangeContainer').show();
            $('#ItemChangeContainer').hide();
            $('#UnitPriceChangeContainer').hide();
            $("#txtChangeItemName").val("");
            $("#txtUnitPriceChange").val("");
            $("#txtQuantityChange").val("");
        }

        function ItemNameChange() {
            $('#QuantityChangeContainer').hide();
            $('#ItemChangeContainer').show();
            $('#UnitPriceChangeContainer').hide();
            $("#txtUnitPriceChange").val("");
            $("#txtQuantityChange").val("");
            $("#txtChangeItemName").val("");
        }

        function UnitPriceChange() {
            $('#QuantityChangeContainer').hide();
            $('#ItemChangeContainer').hide();
            $('#UnitPriceChangeContainer').show();
            $("#txtChangeItemName").val("");
            $("#txtQuantityChange").val("");
            $("#txtUnitPriceChange").val("");
        }

        function UpdateItemDetails(updateType) {
            var updateContent = "";
            if (updateType == "QuantityChange") {
                updateContent = $("#txtQuantityChange").val();

                if (!CommonHelper.IsDecimal(updateContent)) {
                    toastr.warning("Quantity Must be Numeric");
                    return false;
                }

                toastr.success("Quantity Updated Successfully");
            }
            else if (updateType == "ItemNameChange") {
                updateContent = $("#txtChangeItemName").val();
                toastr.success("Item Name Updated Successfully");
            }
            else if (updateType == "UnitPriceChange") {
                updateContent = $("#txtUnitPriceChange").val();
                if (!CommonHelper.IsInt(updateContent)) {
                    toastr.warning("MRP Must be Numeric");
                    return false;
                }

                toastr.success("MRP Updated Successfully");
            }
            PerformUpdateAction(updateType, updateContent);
        }

        function PerformUpdateAction(updateType, updatedContent) {
            var itemId = $("#<%=hfItemRow.ClientID %>").val();
            var tr = $("#ItemInformation tbody tr").find("td:eq(0):contains('" + itemId + "')").parent();

            if (updateType == "QuantityChange") {
                $(tr).find("td:eq(3)").text(updatedContent);
            }
            else if (updateType == "ItemNameChange") {
                $(tr).find("td:eq(2)").text(updatedContent);
            }
            else if (updateType == "UnitPriceChange") {
                $(tr).find("td:eq(4)").text(updatedContent);
            }

            $("editServiceDecider").on('dialogclose', function (event) {
                tr = "";
            });

            var unit = $(tr).find("td:eq(3)").text();
            var unitrate = $(tr).find("td:eq(4)").text();
            var total = parseFloat(unit) * parseFloat(unitrate);
            $(tr).find("td:eq(5)").text(total);
            TotalSalesAmountVatServiceChargeCalculation();
            CalculateTotalAmount();
            return false;
        }

        function AddItemWiseRemarks() {
            var table = "", tr = "", td = "", i = 0;
            var remarks = ["Cold", "Hot", "Spicy", "Normal"];

            table = "<table id=\"TableWiseItemRemarksInformation\" style=\"margin:0;\" class=\"table table-bordered table-condensed table-responsive\" >";
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
            for (i = 0; i < 4; i++) {
                if ((i % 2) == 0)
                    tr = "<tr style=\"background-color:#ffffff;\">";
                else
                    tr = "<tr style=\"background-color:#E3EAEB;\">";

                td = "<td align=\"center\" style=\"width: 30px\">" +
                     "&nbsp;<input type=\"checkbox\">" +
                      "</td>" +
                      "<td align=\"left\" style=\"width: 200px\">" +
                       remarks[i] +
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

        function AddNewPayment(itemId) {
            var cardType = "", cardNumber = "", bankId = 0, chequebankId = 0, txtChecqueNumber = "";
            var paymentMode = $("#<%=ddlPayMode.ClientID %>").val();
            var paymentModeText = $("#<%=ddlPayMode.ClientID %>").find('option:selected').text();
            var currencyType = $("#<%=ddlCurrency.ClientID %>").find('option:selected').text();
            var receiveAmount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
            var conversionRate = $("#<%=txtConversionRate.ClientID %>").val();

            var companyName = $('#txtCmpany').val();
            if (paymentMode == 4) {
                if (companyName == "") {
                    toastr.warning("Please provide Dealer Information.");
                    return false;
                }
            }

            if (paymentMode == 2) {
                var cardType = $("#<%=ddlCardType.ClientID %>").val();
                var cardNumber = $("#<%=txtCardNumber.ClientID %>").val();
                var bankId = $("#<%=ddlBankId.ClientID %>").val();
            }
            if (paymentMode == 3) {
                var chequebankId = $("#<%=ddlChequeBankId.ClientID %>").val();
                var txtChecqueNumber = $("#<%=txtChecqueNumber.ClientID %>").val();
            }
            var hfLocalCurrencyId = $("#ContentPlaceHolder1_hfLocalCurrencyId").val();

            if ($("#ltlPaymentDetailInformation > table").length > 0) {
                AddPaymentRow(paymentModeText, paymentMode, currencyType, receiveAmount, conversionRate, cardType, cardNumber, bankId, chequebankId, txtChecqueNumber);
                return false;
            }

            var table = "", deleteLink = "";
            deleteLink = "<a href=\"javascript:void();\" onclick= 'DeletePayment(" + PaymentRowId + ", " + receiveAmount + ", this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            table += "<table style='width:100%' id='PaymentDetailGrid' class='table table-bordered table-condensed table-responsive'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            table += "<th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Currency Type</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr></thead>";
            table += "<tbody>";
            table += "<tr style=\"background-color:#E3EAEB;\">";
            table += "<td align='left' style='width: 40%;'>" + paymentModeText + "</td>";
            table += "<td align='left' style='width: 40%;'>" + currencyType + "</td>";
            table += "<td align='left' style='width: 20%;'>" + receiveAmount + "</td>";
            table += "<td align='left' style=\"display:none;\">" + conversionRate + "</td>";
            table += "<td align='left' style=\"display:none;\">" + cardType + "</td>";
            table += "<td align='left' style=\"display:none;\">" + cardNumber + "</td>";
            table += "<td align='left' style=\"display:none;\">" + bankId + "</td>";
            table += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";
            table += "</tr>";
            table += "</tbody>";
            table += "</table>";

            if (paymentMode == 3) {
                GuestPayment.push({
                    NodeId: 0,
                    PaymentType: 'Advance',
                    AccountsPostingHeadId: 0,
                    BillPaidBy: 0,
                    BankId: chequebankId,
                    RegistrationId: 0,
                    FieldId: hfLocalCurrencyId,
                    ConvertionRate: 1,
                    CurrencyAmount: receiveAmount,
                    PaymentAmount: receiveAmount,
                    ChecqueDate: null,
                    PaymentMode: paymentModeText,
                    PaymentId: paymentMode,
                    CardNumber: cardNumber,
                    CardType: cardType,
                    ExpireDate: null,
                    ChecqueNumber: txtChecqueNumber,
                    CardHolderName: '',
                    PaymentDescription: ''
                });
            }
            else {
                GuestPayment.push({
                    NodeId: 0,
                    PaymentType: 'Advance',
                    AccountsPostingHeadId: 0,
                    BillPaidBy: 0,
                    BankId: bankId,
                    RegistrationId: 0,
                    FieldId: hfLocalCurrencyId,
                    ConvertionRate: 1,
                    CurrencyAmount: receiveAmount,
                    PaymentAmount: receiveAmount,
                    ChecqueDate: null,
                    PaymentMode: paymentModeText,
                    PaymentId: paymentMode,
                    CardNumber: cardNumber,
                    CardType: cardType,
                    ExpireDate: null,
                    ChecqueNumber: '',
                    CardHolderName: '',
                    PaymentDescription: ''
                });
            }

            $("#<%=hfPaymentId.ClientID %>").val('1');
            $("#ltlPaymentDetailInformation").html(table);
            $("#<%=txtReceiveLeadgerAmount.ClientID %>").val("");
            $("#<%=txtConversionRate.ClientID %>").val("");
            $("#<%=ddlCardType.ClientID %>").val("0");
            $("#<%=txtCardNumber.ClientID %>").val("");
            $("#<%=ddlBankId.ClientID %>").val("");
            $("#<%=txtChecqueNumber.ClientID %>").val("");
            $("#<%=ddlChequeBankId.ClientID %>").val("");
            $("#<%=ddlCompany.ClientID %>").val("");
            $("#<%=ddlPayMode.ClientID %>").val("0");
            $('#txtBankId').val('');
            $('#txtChequeBankId').val('');
            $('#txtCmpany').val('');
            CalculateTotalAmount();
        }

        function AddPaymentRow(paymentModeText, paymentMode, currencyType, receiveAmount, conversionRate, cardType, cardNumber, bankId, chequebankId, txtChecqueNumber) {
            var hfLocalCurrencyId = $("#ContentPlaceHolder1_hfLocalCurrencyId").val();

            var tr = "", totalRow = 0, deleteLink = "";
            totalRow = $("#PaymentDetailGrid tbody tr").length;
            PaymentRowId = PaymentRowId + 1;
            deleteLink = "<a href=\"#\" onclick= 'DeletePayment(" + PaymentRowId + "," + receiveAmount + ", this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }
            tr += "<td align='left' style='width: 40%;'>" + paymentModeText + "</td>";
            tr += "<td align='left' style='width: 40%;'>" + currencyType + "</td>";
            tr += "<td align='left' style='width: 20%;'>" + receiveAmount + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + conversionRate + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + cardType + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + cardNumber + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + bankId + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";
            tr += "</tr>";

            $("#PaymentDetailGrid tbody").append(tr);

            if (paymentMode == 3) {
                GuestPayment.push({
                    NodeId: 0,
                    PaymentType: 'Advance',
                    AccountsPostingHeadId: 0,
                    BillPaidBy: 0,
                    BankId: chequebankId,
                    RegistrationId: 0,
                    FieldId: hfLocalCurrencyId,
                    ConvertionRate: 1,
                    CurrencyAmount: receiveAmount,
                    PaymentAmount: receiveAmount,
                    ChecqueDate: null,
                    PaymentMode: paymentModeText,
                    PaymentId: paymentMode,
                    CardNumber: "",
                    CardType: "",
                    ExpireDate: null,
                    ChecqueNumber: txtChecqueNumber,
                    CardHolderName: '',
                    PaymentDescription: ''
                });
            }
            else {
                GuestPayment.push({
                    NodeId: 0,
                    PaymentType: 'Advance',
                    AccountsPostingHeadId: 0,
                    BillPaidBy: 0,
                    BankId: bankId,
                    RegistrationId: 0,
                    FieldId: hfLocalCurrencyId,
                    ConvertionRate: 1,
                    CurrencyAmount: receiveAmount,
                    PaymentAmount: receiveAmount,
                    ChecqueDate: null,
                    PaymentMode: paymentModeText,
                    PaymentId: paymentMode,
                    CardNumber: cardNumber,
                    CardType: cardType,
                    ExpireDate: null,
                    ChecqueNumber: '',
                    CardHolderName: '',
                    PaymentDescription: ''
                });
            }

            $("#<%=txtReceiveLeadgerAmount.ClientID %>").val("");
            $("#<%=txtConversionRate.ClientID %>").val("");
            $("#<%=ddlCardType.ClientID %>").val("");
            $("#<%=txtCardNumber.ClientID %>").val("0");
            $("#<%=ddlBankId.ClientID %>").val("");
            $("#<%=txtChecqueNumber.ClientID %>").val("");
            $("#<%=ddlChequeBankId.ClientID %>").val("");
            $("#<%=ddlCompany.ClientID %>").val("");
            $("#<%=ddlPayMode.ClientID %>").val("0");
            $('#txtBankId').val('');
            $('#txtChequeBankId').val('');
            $('#txtCmpany').val('');
            CalculateTotalAmount();
        }

        function DeletePayment(id, amount, anchor) {
            var tr = $(anchor).parent().parent();
            $(tr).remove();
            //delete GuestPayment[id];
            //alert(id);
            GuestPayment.splice(id, 1);
            CalculateTotalAmount();
        }

        function TotalSalesAmountVatServiceChargeCalculation() {
            debugger;
            var cbServiceChargeVal = "1";
            var cbVatAmountVal = "1";

            var totalAmount = 0;
            $('#ItemInformation tbody tr').each(function () {
                totalAmount += parseFloat($.trim($(this).find("td:eq(7)").text()), 10)
            });

            var InclusiveBill = 0, Vat = 0, ServiceCharge = 0;
            var result = CommonHelper.GetDefaultRackRateServiceChargeVatCalculation(InclusiveBill, Vat, ServiceCharge, 0, totalAmount, parseInt(cbServiceChargeVal, 10), 0, parseInt(cbVatAmountVal, 10));
            //var result = CommonHelper.GetRackRateServiceChargeVatInformation(InclusiveBill, Vat, ServiceCharge, 0, totalAmount, parseInt(cbServiceChargeVal, 10), 0, parseInt(cbVatAmountVal, 10));
            //toastr.info(result.RackRate);
            $("#ContentPlaceHolder1_txtServiceCharge").val(result.ServiceCharge);
            $("#ContentPlaceHolder1_txtVatAmount").val(result.VatAmount);
            $("#ContentPlaceHolder1_txtGrandTotal").val(result.RackRate);
            //$("#ContentPlaceHolder1_txtChangeAmount").val(result.RackRate);

            var cashIncentive = $("#ContentPlaceHolder1_txtCashIncentive").val();

            $("#ContentPlaceHolder1_txtChangeAmount").val(parseFloat(result.RackRate) - parseFloat(cashIncentive));
        }

        function Settlement(savetype) {
            var ItemDetails = new Array(), RestaurantBill = {}, ItemMaster = {};
            var salesOrderId = $("#ContentPlaceHolder1_hfSalesOrderId").val();
            var hfLocalCurrencyId = $("#ContentPlaceHolder1_hfLocalCurrencyId").val();
            var billId = $("#ContentPlaceHolder1_hfBillId").val();
            if (billId == "") {
                billId = 0;
            }

            var kotId = $("#ContentPlaceHolder1_hfKotId").val();
            if (kotId == "") {
                kotId = 0;
            }
            var costCenterId = $("#ContentPlaceHolder1_ddlCostCenterId").val();
            ItemMaster = {
                SourceId: 1,
                PaxQuantity: 1,
                SourceName: 'RestaurantToken',
                CostCenterId: costCenterId
            };

            $("#ItemInformation tbody tr").each(function () {
                var itemId = parseInt($.trim($(this).find("td:eq(0)").text(), 10));
                var itemUnit = parseFloat($.trim($(this).find("td:eq(3)").text(), 10));
                var unitRate = parseFloat($.trim($(this).find("td:eq(4)").text(), 10));
                var discountAmount = parseFloat($.trim($(this).find("td:eq(6)").text(), 10));

                ItemDetails.push({
                    ItemType: 'IndividualItem',
                    ItemId: itemId,
                    ItemUnit: itemUnit,
                    UnitRate: unitRate,
                    Amount: itemUnit * unitRate,
                    DiscountAmount: discountAmount
                });
            });


            var isInvoiceServiceChargeEnable = true, isInvoiceVatAmountEnable = true;
            var grandTotal = $("#ContentPlaceHolder1_txtGrandTotal").val();
            var cashIncentive = $("#ContentPlaceHolder1_txtCashIncentive").val();
            var cashDiscount = $("#ContentPlaceHolder1_txtCashDiscount").val();
            var serviceCharge = $("#ContentPlaceHolder1_txtServiceCharge").val();
            var vatAmount = $("#ContentPlaceHolder1_txtVatAmount").val();
            var costCenterId = $("#ContentPlaceHolder1_ddlCostCenterId").val();
            var hfCreditSale = $("#<%=hfCreditSale.ClientID %>").val();
            var companyId = $("#<%=ddlCompany.ClientID %>").val();
            var customerInfo = '';
            if (hfCreditSale == 1) {
                customerInfo = $('#txtCmpany').val();
            }

            if (cashIncentive == "") {
                cashIncentive = 0;
            }

            RestaurantBill = {
                CostCenterId: costCenterId,
                DiscountType: '',
                IsComplementary: false,
                DiscountTransactionId: 0,
                DiscountAmount: 0,
                CalculatedDiscountAmount: 0,
                ServiceCharge: serviceCharge,
                VatAmount: vatAmount,
                CustomerName: customerInfo,
                PaxQuantity: 1,
                TableId: 1,
                SourceName: 'RestaurantToken',
                BillPaidBySourceId: companyId,
                IsInvoiceServiceChargeEnable: isInvoiceServiceChargeEnable,
                IsInvoiceVatAmountEnable: isInvoiceVatAmountEnable,
                SalesAmount: grandTotal,
                GrandTotal: grandTotal,
                CashDiscount: cashDiscount,
                CashIncentive: cashIncentive
            };

            var companyName = $('#txtCmpany').val();
            var payMode = $("#<%=ddlPayMode.ClientID %>").val();
            if (savetype == "Settlement") {
                if (hfCreditSale == 1) {
                    if (companyName == "") {
                        toastr.warning("Please provide Dealer Information.");
                        return false;
                    }
                    GuestPayment.push({
                        NodeId: 0,
                        PaymentType: 'Company',
                        AccountsPostingHeadId: 0,
                        BillPaidBy: 0,
                        BankId: 0,
                        RegistrationId: 0,
                        FieldId: 1,
                        ConvertionRate: 1,
                        CurrencyAmount: (grandTotal - cashIncentive),
                        PaymentAmount: (grandTotal - cashIncentive),
                        ChecqueDate: null,
                        PaymentMode: 'Company',
                        PaymentId: 0,
                        CardNumber: null,
                        CardType: null,
                        ExpireDate: null,
                        ChecqueNumber: null,
                        CardHolderName: '',
                        PaymentDescription: companyName,
                        CompanyId: companyId
                    });
                }

                PageMethods.SaveSettlement(salesOrderId, savetype, kotId, billId, ItemMaster, ItemDetails, RestaurantBill, GuestPayment, OnSettlementSuccess, OnSettlementFail);
            }
            else if (savetype == "Challan") {
                PageMethods.SavePreview(salesOrderId, kotId, billId, ItemMaster, ItemDetails, RestaurantBill, OnChallanSaveSuccess, OnChallanSaveFail);
            }
            else {
                if (hfCreditSale == 1) {
                    if (companyName == "") {
                        toastr.warning("Please provide Dealer Information.");
                        return false;
                    }
                }
                PageMethods.SavePreview(salesOrderId, kotId, billId, ItemMaster, ItemDetails, RestaurantBill, OnSaveSuccess, OnSaveFail);
            }
            return false;
        }

        function OnSettlementSuccess(result) {
            $("#ContentPlaceHolder1_hfKotId").val("");
            $("#ContentPlaceHolder1_hfBillId").val("");
            $("#ContentPlaceHolder1_ddlStockBy").val(0);
            $("#ContentPlaceHolder1_ddlCompany").val(0);

            $("#ContentPlaceHolder1_txtCashDiscount").val("");
            $("#ContentPlaceHolder1_txtCashIncentive").val("");

            $("#ContentPlaceHolder1_txtGrandTotal").val("");
            $("#ContentPlaceHolder1_txtServiceCharge").val("");
            $("#ContentPlaceHolder1_txtVatAmount").val("");
            $("#ContentPlaceHolder1_txtChangeAmount").val("");
            $("#ContentPlaceHolder1_ddlCostCenterId").val(2);
            $('#txtCmpany').val('');
            $("#ltlTableWiseItemInformation").html("");
            $("#ltlPaymentDetailInformation").html("");
            GuestPayment = [];
            //            var hfCreditSale = $("#<%=hfCreditSale.ClientID %>").val();
            //            if (hfCreditSale == 1) {
            //                $("#ContentPlaceHolder1_btnSave").prop("disabled", false);
            //                $("#ContentPlaceHolder1_btnPreview").prop("disabled", false);
            //                $("#ContentPlaceHolder1_btnChallan").prop("disabled", false);
            //            }
            //            else {
            //                $("#ContentPlaceHolder1_btnSave").prop("disabled", true);
            //                $("#ContentPlaceHolder1_btnPreview").prop("disabled", true);
            //                $("#ContentPlaceHolder1_btnChallan").prop("disabled", true);
            //            }

            if (result[1] != "" || result[1] != 0) {
                if (result[1] != "" || result[1] != 0) {
                    var url = "/POS/Reports/frmReportBillInfo.aspx?billID=" + result[1];
                    var popup_window = "Invoice";
                    window.open(url, popup_window, "width=825,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
                }
            }
        }
        function OnSettlementFail(error) {
            //CommonHelper.AlertMessage(result.AlertMessage);
        }

        function OnChallanSaveSuccess(result) {
            $("#ContentPlaceHolder1_hfKotId").val(result[0]);
            $("#ContentPlaceHolder1_hfBillId").val(result[1]);

            if (result[1] != "" || result[1] != 0) {
                var url = "/POS/Reports/frmReportBillInfo.aspx?Challan=" + result[1];
                var popup_window = "Challan";
                window.open(url, popup_window, "width=825,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            }
        }
        function OnChallanSaveFail(error) {
        }

        function OnSaveSuccess(result) {
            $("#ContentPlaceHolder1_hfKotId").val(result[0]);
            $("#ContentPlaceHolder1_hfBillId").val(result[1]);

            if (result[1] != "" || result[1] != 0) {
                var url = "/PointOfSales/Reports/frmReportBillInfo.aspx?billID=" + result[1];
                var popup_window = "Preview";
                window.open(url, popup_window, "width=825,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            }
        }
        function OnSaveFail(error) {
        }

        function CalculateTotalAmount() {
            var grandTotal = $("#ContentPlaceHolder1_txtGrandTotal").val();
            var cmpId = $("#ContentPlaceHolder1_ddlCompany").val();
            var cmpName = $('#txtCmpany').val();
            var totalAmount = 0;

            var hfCreditSale = $("#<%=hfCreditSale.ClientID %>").val();
            if (hfCreditSale == 1) {
                //alert(cmpId);
                if (cmpId >= 1 && grandTotal >= 1 && cmpName != "") {
                    $("#ContentPlaceHolder1_btnSave").prop("disabled", false);
                    $("#ContentPlaceHolder1_btnPreview").prop("disabled", false);
                    $("#ContentPlaceHolder1_btnChallan").prop("disabled", false);
                }
                else {
                    $("#ContentPlaceHolder1_btnSave").prop("disabled", true);
                    $("#ContentPlaceHolder1_btnPreview").prop("disabled", true);
                    $("#ContentPlaceHolder1_btnChallan").prop("disabled", true);
                }
            }
            else {
                $("#PaymentDetailGrid tbody tr").each(function () {
                    var amount = parseFloat($.trim($(this).find("td:eq(2)").text(), 10));
                    totalAmount = totalAmount + amount;
                });
                if (totalAmount == parseFloat(grandTotal)) {
                    $("#ContentPlaceHolder1_btnSave").prop("disabled", false);
                    $("#ContentPlaceHolder1_btnPreview").prop("disabled", false);
                    $("#ContentPlaceHolder1_btnChallan").prop("disabled", false);
                }
                else {
                    $("#ContentPlaceHolder1_btnSave").prop("disabled", true);
                    $("#ContentPlaceHolder1_btnPreview").prop("disabled", true);
                    $("#ContentPlaceHolder1_btnChallan").prop("disabled", true);
                }

                var CashIncentive = $("#ContentPlaceHolder1_txtCashIncentive").val();

                $("#ContentPlaceHolder1_txtChangeAmount").val((parseFloat(grandTotal) - parseFloat(totalAmount)) - parseFloat(CashIncentive));
            }
        }
        function EnvelopeGenerate() {
            var sourceId = $("#ContentPlaceHolder1_ddlCompany").val();
            var url = "/SalesAndMarketing/Reports/frmReportGuestEnvelope.aspx?GOCId=" + sourceId;
            var popup_window = "Envelope Print";
            window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }
        function LoadSalesdOrder() {
            if ($("#txtCmpany").val() != "") {
                var companyId = $("#ContentPlaceHolder1_ddlCompany").val();
                PageMethods.LoadSalesOrder(companyId, OnLoadSalesOrderSuccess, OnLoadSalesOrderFail);
            }
            else {
                toastr.info("Please Provide Dealer Information");
                $("#txtCmpany").focus();
            }
        }
        function OnLoadSalesOrderSuccess(result) {
            $("#SalesOrderDiv").html(result);
            $("#SalesOrderDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Sales Order",
                show: 'slide'
            });
        }
        function OnLoadSalesOrderFail(error) {
        }
        function LoadSalesdOrderDetailsInfo() {
            $("#SalesOrderDetailDiv").html("");
            $("#ApprovedSalesOrderInformationDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Sales Order Information",
                show: 'slide'
            });
        }
        function LoadSalesdOrderDetails() {
            var searchFromDate = $("#<%=txtFromDate.ClientID %>").val();
            var searchToDate = $("#<%=txtToDate.ClientID %>").val();
            var costCenterId = $("#<%=ddlSrcCostCenter.ClientID %>").val();
            var billNo = $("#<%=txtSPONumber.ClientID %>").val();
            PageMethods.LoadSalesOrderInformation(searchFromDate, searchToDate, costCenterId, billNo, OnLoadSalesdOrderDetailsSuccess, OnLoadSalesdOrderDetailsFail);
        }
        function OnLoadSalesdOrderDetailsSuccess(result) {
            $("#SalesOrderDetailDiv").html(result);
        }
        function OnLoadSalesdOrderDetailsFail(error) {
        }
        function UpdateSalesdOrder() {
            var SalesOrederList = new Array();
            $("#SalesOrder tbody tr").each(function () {
                if ($(this).find("td:eq(1)").find("input").is(":checked")) {
                    var id = $(this).find("td:eq(0)").text();
                    var status = $(this).find("td:eq(3)").find("select").val();
                    SalesOrederList.push({
                        SOrderId: id,
                        DeliveryStatus: status
                    });
                }
            });
            PageMethods.UpdateSalesdOrder(SalesOrederList, OnUpdateSalesdOrderSuccess, OnUpdateSalesdOrderFail);
        }
        function OnUpdateSalesdOrderSuccess(result) {
            $("#SalesOrderDiv").dialog("close");
            CommonHelper.AlertMessage(result.AlertMessage);
        }
        function OnUpdateSalesdOrderFail(error) {
        }
        function CheckUncheckAll() {
            $("#SalesOrder thead tr").each(function () {
                if ($(this).find("th:eq(1)").find("input").is(":checked")) {
                    $("#SalesOrder tbody tr").find("td:eq(1)").find("input").prop("checked", true);
                }
                else {
                    $("#SalesOrder tbody tr").find("td:eq(1)").find("input").prop("checked", false);
                }
            });
        }

        function GenerateInvoice(SOrderId, CompanyId) {
            var url = "/SalesAndMarketing/Reports/frmReportSalesOrderInvoice.aspx?POrderId=" + SOrderId + "&SupId=" + CompanyId;
            var popup_window = "Product Order Invoice";
            window.open(url, popup_window, "width=825,height=680,left=300,top=50,resizable=yes");
        }

        function BillingProcessFromSalesOrder(sOrderId) {
            CommonHelper.SpinnerOpen();
            $("#ContentPlaceHolder1_hfSalesOrderId").val(sOrderId);
            PageMethods.PerformLoadPMProductDetailOnDisplayMode(sOrderId, OnLoadPODetailsSucceeded, OnLoadPODetailsFailed);
            $("#SalesOrderDiv").dialog("close");
            $("#ApprovedSalesOrderInformationDiv").dialog("close");
            return false;
        }
        function OnLoadPODetailsSucceeded(result) {
            var totalRow = result.length, row = 0, status = "";
            //$("#SalesOrderDiv").dialog("close");

            var itemId = "1", categoryId = "1", itemName = "Pen", itemUnit = "10", quantity = "2", total = "20";
            var cashDiscountAmount = 0, cashIncentiveAmount = 0;
            var table = "", deleteLink = "";
            table += "<table id='ItemInformation' style='width: 100%;' class='table table-bordered table-condensed table-responsive'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            table += "<th style='display:none'>RowId</th><th style='display:none'>ItemId</th><th style='display:none'>CategoryId</th><th align='left' scope='col' style='width: 35%;'>Item Name</th>";
            table += "<th align='left' scope='col' style='width: 10%;'>Qty.</th>";
            table += "<th align='left' scope='col' style='width: 10%;'>MRP</th>";
            table += "<th align='left' scope='col' style='width: 10%;'>Total</th>";
            table += "<th align='left' scope='col' style='width: 10%;'>Discount</th>";
            table += "<th align='left' scope='col' style='width: 20%;'>Discounted Total</th>";
            table += "<tbody>";

            for (row = 0; row < totalRow; row++) {
                itemId = result[row].ProductId;
                table += "<tr style=\"background-color:#E3EAEB;\">";
                table += "<td align='left' style=\"display:none;\">" + result[row].ProductId + "</td>";
                table += "<td align='left' style=\"display:none;\">" + result[row].CategoryId + "</td>";
                table += "<td align='left' style=\"width:35%; text-align:Left;\">" + result[row].ProductName + "</td>";
                table += "<td align='left' style=\"width:10%; text-align:Left;\">" + result[row].Quantity + "</td>";
                table += "<td align='left' style='width: 10%;'>" + result[row].PurchasePrice + "</td>";
                table += "<td align='left' style='width: 10%;'>" + ((result[row].PurchasePrice) * (result[row].Quantity)) + "</td>";
                table += "<td align='left' style='width: 10%;'>" + result[row].DiscountAmount + "</td>";
                table += "<td align='left' style='width: 20%;'>" + (((result[row].PurchasePrice) * (result[row].Quantity)) - result[row].DiscountAmount) + "</td>";
                table += "</tr>";

                cashDiscountAmount = parseFloat(cashDiscountAmount) + parseFloat(result[row].DiscountAmount);
                cashIncentiveAmount = parseFloat(cashIncentiveAmount) + parseFloat(result[row].CashIncentive);

                $("#<%=ddlCostCenterId.ClientID %>").val(result[row].CostCenterId);
                $("#ContentPlaceHolder1_hfBillId").val(result[row].BillId);
            }

            table += "</tbody>";
            table += "</table>";

            $("#<%=hfCreditSale.ClientID %>").val("1");
            $("#ContentPlaceHolder1_btnSave").prop("disabled", false);
            $("#ContentPlaceHolder1_btnPreview").prop("disabled", false);
            $("#ContentPlaceHolder1_btnChallan").prop("disabled", false);

            $("#<%=hfItemId.ClientID %>").val(itemId);
            $("#ltlTableWiseItemInformation").html(table);
            $("#<%=txtItemName.ClientID %>").val("");
                $("#<%=txtUnitPrice.ClientID %>").val("");
            $("#<%=txtQuantity.ClientID %>").val("");
            $("#<%=ddlStockBy.ClientID %>").val(0);
            $("#<%=txtStockQuantity.ClientID %>").val("");
            $("#ContentPlaceHolder1_txtItemName").focus();

            $("#ContentPlaceHolder1_txtCashDiscount").val(cashDiscountAmount.toFixed(2));
            $("#ContentPlaceHolder1_txtCashIncentive").val(cashIncentiveAmount.toFixed(2));

            TotalSalesAmountVatServiceChargeCalculation();

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadPODetailsFailed() { CommonHelper.SpinnerClose(); }

        function BillingProcess2FromSalesOrder(sOrderId) {
            CommonHelper.SpinnerOpen();
            $("#ContentPlaceHolder1_hfSalesOrderId").val(sOrderId);
            PageMethods.PerformLoadPMProductDetailOnDisplayMode(sOrderId, OnLoadPODetails2Succeeded, OnLoadPODetails2Failed);
            $("#ApprovedSalesOrderInformationDiv").dialog("close");
            return false;
        }
        function OnLoadPODetails2Succeeded(result) {
            var totalRow = result.length, row = 0, status = "";
            //$("#SalesOrderDiv").dialog("close");

            $("#<%=ddlCostCenterId.ClientID %>").val(result[0].CostCenterId);

            $("#<%=ddlCompany.ClientID %>").val(result[0].CompanyId);
            $('#txtCmpany').val($("#<%=ddlCompany.ClientID %> option:selected").text());

            var itemId = "1", categoryId = "1", itemName = "Pen", itemUnit = "10", quantity = "2", total = "20";
            var cashDiscountAmount = 0, cashIncentiveAmount = 0;
            var table = "", deleteLink = "";
            table += "<table id='ItemInformation' style='width: 100%;' class='table table-bordered table-condensed table-responsive'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            table += "<th style='display:none'>RowId</th><th style='display:none'>ItemId</th><th style='display:none'>CategoryId</th><th align='left' scope='col' style='width: 35%;'>Item Name</th>";
            table += "<th align='left' scope='col' style='width: 10%;'>Qty.</th>";
            table += "<th align='left' scope='col' style='width: 15%;'>MRP</th>";
            table += "<th align='left' scope='col' style='width: 20%;'>Total</th>";
            table += "<tbody>";

            for (row = 0; row < totalRow; row++) {
                itemId = result[row].ProductId;
                table += "<tr style=\"background-color:#E3EAEB;\">";
                table += "<td align='left' style=\"display:none;\">" + result[row].ProductId + "</td>";
                table += "<td align='left' style=\"display:none;\">" + result[row].CategoryId + "</td>";
                table += "<td align='left' style=\"width:35%; text-align:Left;\">" + result[row].ProductName + "</td>";
                table += "<td align='left' style=\"width:10%; text-align:Left;\">" + result[row].Quantity + "</td>";
                table += "<td align='left' style='width: 15%;'>" + result[row].PurchasePrice + "</td>";
                table += "<td align='left' style='width: 20%;'>" + ((result[row].PurchasePrice) * (result[row].Quantity)) + "</td>";
                table += "</tr>";

                cashDiscountAmount = 0; //cashDiscountAmount + result[row].DiscountAmount;
                cashIncentiveAmount = parseFloat(cashIncentiveAmount) + parseFloat(result[row].CashIncentive);
            }

            table += "</tbody>";
            table += "</table>";

            $("#<%=hfCreditSale.ClientID %>").val("1");
            $("#ContentPlaceHolder1_btnSave").prop("disabled", false);
            $("#ContentPlaceHolder1_btnPreview").prop("disabled", false);
            $("#ContentPlaceHolder1_btnChallan").prop("disabled", false);

            $("#<%=hfItemId.ClientID %>").val(itemId);
            $("#ltlTableWiseItemInformation").html(table);
            $("#<%=txtItemName.ClientID %>").val("");
                $("#<%=txtUnitPrice.ClientID %>").val("");
            $("#<%=txtQuantity.ClientID %>").val("");
            $("#<%=ddlStockBy.ClientID %>").val(0);
            $("#<%=txtStockQuantity.ClientID %>").val("");
            $("#ContentPlaceHolder1_txtItemName").focus();
            TotalSalesAmountVatServiceChargeCalculation();

            $("#ContentPlaceHolder1_txtCashDiscount").val(cashDiscountAmount.toFixed(2));
            $("#ContentPlaceHolder1_txtCashIncentive").val(cashIncentiveAmount.toFixed(2));

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadPODetails2Failed() { CommonHelper.SpinnerClose(); }
    </script>
    <style>
        .FullWidth {
            height: 400px;
        }

        .HalfWidth {
            height: 440px;
        }

        .hidden {
            display: none;
        }

        .visible {
            display: block;
        }

        .ui-state-default, .ui-widget-content .ui-state-default, .ui-widget-header .ui-state-default, .ui-button, html .ui-button.ui-state-disabled:hover, html .ui-button.ui-state-disabled:active {
            border: 1px solid #d8dcdf;
            font-weight: bold;
            color: white;
            background: url(/StyleSheet/css/images/header.jpg) 50% 50% repeat-x;
        }
    </style>
    <asp:HiddenField ID="hfItemId" runat="server" />
    <asp:HiddenField ID="hfPaymentId" runat="server" />
    <asp:HiddenField ID="hfCurrencyType" runat="server" />
    <asp:HiddenField ID="hfConversionRate" runat="server" />
    <asp:HiddenField ID="hfItemRow" runat="server" />
    <asp:HiddenField ID="hfLocalCurrencyId" runat="server" />
    <asp:HiddenField ID="hfBillId" runat="server" />
    <asp:HiddenField ID="hfKotId" runat="server" />
    <asp:HiddenField ID="hfCreditSale" runat="server" />
    <asp:HiddenField ID="hfExtraButtonShowHide" runat="server" />
    <asp:HiddenField ID="hfSalesOrderId" runat="server" />
    <div id="ApprovedSalesOrderInformationDiv" style="display:none;">
        <div id="InfoPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">                
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSPONumber" runat="server" class="control-label" Text="SO Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSPONumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <asp:Panel ID="pnlStatus" runat="server">
                        <div class="col-md-2">
                        <asp:Label ID="lblSrcCostCenter" runat="server" class="control-label" Text="Depo Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSrcCostCenter" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    </asp:Panel>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="2"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                </div>              
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return LoadSalesdOrderDetails();" TabIndex="5" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="Div1" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div id="SalesOrderDetailDiv">
    </div>
    </div>
    </div>
    <div class="row">
        <div id="AppraisalAccordion" class="col-md-6" style="padding: 0 5px 0 15px;">
            <h3>
                Item Information</h3>
            <div id="ItemListDiv" class="panel panel-default" overflow: scroll; overflow-x: hidden;>
                <div id="ItemBody" style="height: 100%;">
                    <div id="ltlTableWiseItemInformation">
                    </div>
                </div>
            </div>
            <h3 id="PaymentHead">
                Payment Details</h3>
            <div id="PaymentListDiv" class="panel panel-default" style="height: 440px; overflow: scroll;
                overflow-x: hidden;">
                <div style="height: 100%;">
                    <div id="ltlPaymentDetailInformation">
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6" style="padding-left: 5px;">
            <div id="ItemDetailsDiv" class="panel panel-default" style="padding-bottom: 5px;">
                <div class="panel-heading">
                    Item Information</div>
                <div class="panel-body">
                    <div class="form-horizontal" id="CostCenterDivInformation" runat="server">
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblCostCenter" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-9">
                                <asp:DropDownList ID="ddlCostCenterId" runat="server" CssClass="form-control" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" style="display: none;">
                            <div class="col-md-3">
                                <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                            </div>
                            <div class="col-md-9">
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblItemName" runat="server" class="control-label" Text="Item Name"></asp:Label>
                            </div>
                            <div class="col-md-9">
                                <asp:TextBox ID="txtItemName" TabIndex="18" runat="server" CssClass="form-control"
                                    ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblItemUnit" runat="server" class="control-label" Text="MRP"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtUnitPrice" TabIndex="18" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-5">
                                <asp:TextBox ID="txtStockQuantity" TabIndex="18" CssClass="form-control" runat="server"
                                    ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblQuantity" runat="server" class="control-label" Text="Quantity"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtQuantity" TabIndex="18" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-5">
                                <asp:DropDownList ID="ddlStockBy" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" style="padding-left: 15px; padding-top: 10px;">
                            <button type="button" id="btnAddItem" class="TransactionalButton btn btn-primary btn-sm">
                                Add</button>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchSalesOrderDiv" class="panel panel-default" style="padding-bottom: 5px;">
                <div class="panel-heading">
                    Company Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div id="CompanyDiv" class="form-group" style="display: none;">
                            <div class="col-md-3">
                                <asp:Label ID="lblCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-9">
                                <input id="txtCmpany" type="text" class="form-control" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-left: 15px; padding-top: 10px;">
                            <div class="col-md-6" style="padding-top: 10px;">
                                <input type="button" id="btnCmpSearch" value="Search Order" class="TransactionalButton btn btn-primary btn-sm"
                                    onclick="javascript: return LoadSalesdOrder();" />
                                <input type="button" id="btnSlsOrderSearch" value="..." class="TransactionalButton btn btn-primary btn-sm"
                                    onclick="javascript: return LoadSalesdOrderDetailsInfo();" style="display:none;" /> 
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="PaymentDiv" class="panel panel-default" style="padding-bottom: 5px;">
                <div class="panel-heading">
                    Bill Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">                        
                        <div class="form-group" style="display:none;">
                            <div class="col-md-3">
                                <asp:Label ID="lblServiceCharge" runat="server" class="control-label" Text="Service Charge"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtServiceCharge" runat="server" TabIndex="22" CssClass="form-control"
                                    Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblVatAmount" runat="server" class="control-label" Text="Vat Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="23" CssClass="form-control"
                                    Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblCashDiscount" runat="server" class="control-label" Text="Cash Discount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtCashDiscount" runat="server" TabIndex="22" CssClass="form-control"
                                    Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblCashIncentive" runat="server" class="control-label" Text="Cash Incentive"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtCashIncentive" runat="server" TabIndex="22" CssClass="form-control"
                                    Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-3">
                                <asp:Label ID="lblGrnadTotal" runat="server" class="control-label" Text="Grand Total"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtGrandTotal" TabIndex="4" runat="server" CssClass="form-control"
                                    Enabled="False"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblDueAmount" runat="server" class="control-label" Text="Due Amount"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtChangeAmount" TabIndex="5" runat="server" CssClass="form-control"
                                    Enabled="False"></asp:TextBox>
                            </div>
                        </div>
                        <div id="PayModeDiv">
                            <div class="form-group">
                                <div class="col-md-3">
                                    <asp:Label ID="lblPayMode" runat="server" class="control-label" Text="Payment Mode"></asp:Label>
                                </div>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlPayMode" runat="server" CssClass="form-control" TabIndex="5">
                                        <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                        <asp:ListItem Value="1">Cash</asp:ListItem>
                                        <asp:ListItem Value="2">Card</asp:ListItem>
                                        <asp:ListItem Value="3">Cheque</asp:ListItem>
                                        <asp:ListItem Value="4">Dealer</asp:ListItem>
                                        <asp:ListItem Value="5">Refund</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3">
                                    <asp:Label ID="lblReceiveLeadgerAmount" runat="server" class="control-label required-field"
                                        Text="Rec. Amount"></asp:Label>
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="txtReceiveLeadgerAmount" runat="server" CssClass="form-control quantitydecimal"
                                        TabIndex="7"></asp:TextBox>
                                </div>
                            </div>
                        </div>                        
                        <div class="form-group" style="display: none;">
                            <div class="col-md-3">
                                <asp:Label ID="lblCurrencyType" runat="server" class="control-label required-field"
                                    Text="Currency Type"></asp:Label>
                            </div>
                            <div class="col-md-9">
                                <asp:DropDownList ID="ddlCurrency" TabIndex="6" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                                <asp:Label ID="lblDisplayConvertionRate" runat="server" class="control-label" Text=""></asp:Label>
                                <asp:HiddenField ID="ddlCurrencyHiddenField" runat="server"></asp:HiddenField>
                            </div>
                        </div>
                        <div id="ConversionRateDivInformation" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="lblConversionRate" runat="server" class="control-label required-field"
                                        Text="Conversion Rate"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control" Text=""></asp:TextBox>
                                    <asp:HiddenField ID="txtConversionRateHiddenField" runat="server"></asp:HiddenField>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Label ID="lblConvertedAmount" runat="server" class="control-label" Text="Converted Amount"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtConvertedAmount" runat="server" CssClass="form-control" Text=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="CardPaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-3">
                                    <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Card Type"></asp:Label>
                                </div>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlCardType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                        <asp:ListItem Value="a">American Express</asp:ListItem>
                                        <asp:ListItem Value="m">Master Card</asp:ListItem>
                                        <asp:ListItem Value="v">Visa Card</asp:ListItem>
                                        <asp:ListItem Value="d">Discover Card</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3">
                                    <asp:Label ID="lblCardNumber" runat="server" class="control-label" Text="Card Number"></asp:Label>
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="txtCardNumber" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3">
                                    <asp:Label ID="lblBankId" runat="server" class="control-label required-field" Text="Bank Name"></asp:Label>
                                </div>
                                <div class="col-md-9">
                                    <input id="txtBankId" type="text" class="form-control" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlBankId" CssClass="form-control" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-3">
                                    <asp:Label ID="lblChequeBankId" runat="server" class="control-label required-field"
                                        Text="Bank Name"></asp:Label>
                                </div>
                                <div class="col-md-9">
                                    <input id="txtChequeBankId" class="form-control" type="text" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlChequeBankId" runat="server" CssClass="form-control" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlChecquePaymentAccountHeadId" runat="server" CssClass="form-control"
                                            TabIndex="6">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3">
                                    <asp:Label ID="lblChecqueNumber" runat="server" class="control-label" Text="Cheque Number"></asp:Label>
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="txtChecqueNumber" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-left: 15px; padding-top: 10px;">
                            <div id="SaveButton" class="col-md-9">
                                <input type="button" id="btnAddDetailGuestPayment" value="Add" class="TransactionalButton btn btn-primary btn-sm" />
                                <asp:Label ID="lblHiddenIdDetailGuestPayment" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="padding-left: 5px;">
                <div class="col-md-3">
                    <input type="button" id="btnEnvelope" value="Envelop Print" style="width:106px;" class="TransactionalButton btn btn-primary btn-sm"
                                    onclick="javascript: return EnvelopeGenerate();" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnPreview" runat="server" TabIndex="14" Text="Bill View" style="width:106px;" CssClass="TransactionalButton btn btn-primary btn-sm samewidthbutton"
                        OnClientClick="javascript: return Settlement('Preview');" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnChallan" runat="server" TabIndex="14" Text="Challan" style="width:106px;" CssClass="TransactionalButton btn btn-primary btn-sm samewidthbutton"
                        OnClientClick="javascript: return Settlement('Challan');" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnSave" runat="server" TabIndex="13" Text="Settlement" style="width:106px;" CssClass="TransactionalButton btn btn-primary btn-sm samewidthbutton"
                        OnClientClick="javascript: return Settlement('Settlement');" />
                </div>
            </div>
        </div>
    </div>
    <div id="editServiceDecider" style="display: none;">
        <input type="hidden" id="hfKotDetailId" value="0" />
        <input type="hidden" id="hfEditRowId" value="" />
        <div id="editServiceDeciderHtml">
            <div style="padding: 10px">
                <div style="float: left; padding-left: 10px; padding-bottom: 15px; width: 150px">
                    <input type="button" onclick="return QuantityChange();" class="TransactionalButton btn btn-primary btn-sm"
                        value="Quantity Change" />
                </div>
                <div style="float: left; padding-left: 10px; padding-bottom: 15px; width: 150px">
                    <input type="button" onclick="return ItemNameChange();" class="TransactionalButton btn btn-primary btn-sm"
                        value="Name Change" />
                </div>
                <div style="float: left; padding-left: 10px; padding-bottom: 15px; width: 150px">
                    <input type="button" onclick="return UnitPriceChange();" class="TransactionalButton btn btn-primary btn-sm"
                        value="MRP Change" />
                </div>
                <div id="QuantityChangeContainer" class="panel-body" style="font-weight: bold; display: none;
                    height: 100px;">
                    <div class="row" style="padding-top: 50px;">
                        <div class="col-md-2">
                            Quantity</div>
                        <div class="col-md-7">
                            <input type="text" id="txtQuantityChange" class="form-control" /></div>
                        <div class="col-md-3">
                            <input type="button" onclick="return UpdateItemDetails('QuantityChange');" class="TransactionalButton btn btn-primary btn-sm"
                                value="Change" /></div>
                    </div>
                </div>
                <div id="ItemChangeContainer" class="panel-body" style="font-weight: bold; display: none;
                    height: 100px;">
                    <div class="row" style="padding-top: 50px;">
                        <div class="col-md-2">
                            Name</div>
                        <div class="col-md-7">
                            <input type="text" id="txtChangeItemName" class="form-control" /></div>
                        <div class="col-md-3">
                            <input type="button" onclick="return UpdateItemDetails('ItemNameChange');" class="TransactionalButton btn btn-primary btn-sm"
                                value="Change" />
                        </div>
                    </div>
                </div>
                <div id="UnitPriceChangeContainer" class="panel-body" style="font-weight: bold; display: none;
                    height: 100px;">
                    <div class="row" style="padding-top: 50px;">
                        <div class="col-md-2">
                            MRP</div>
                        <div class="col-md-7">
                            <input type="text" id="txtUnitPriceChange" class="form-control" /></div>
                        <div class="col-md-3">
                            <input type="button" onclick="return UpdateItemDetails('UnitPriceChange');" class="TransactionalButton btn btn-primary btn-sm"
                                value="Change" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="ItemWiseSpecialRemarks" style="width: 350px; display: none;">
        <div class="">
            <div>
                <div id="remarksContainer" style="width: 100%;">
                </div>
            </div>
            <div id="Div4" style="padding-left: 5px; padding-top: 10px; width: 100%;">
                <div style="float: left; padding-bottom: 15px;">
                    <input type="button" id="btnItemwiseSpecialRemarksSave" class="TransactionalButton btn btn-primary btn-sm"
                        value="Save" />
                    <input type="button" id="btnItemwiseSpecialRemarksCancel" class="TransactionalButton btn btn-primary btn-sm"
                        value="Cancel" />
                </div>
                <div id="ItemWiseSpecialRemarksContainer" class="alert alert-info" style="display: none;">
                    <button type="button" class="close" data-dismiss="alert">
                        ×</button>
                    <asp:Label ID='ItemWiseSpecialRemarksMessage' Font-Bold="True" runat="server" ClientIDMode="Static"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <div id="reportFrame" runat="server">
        <iframe id="IframeReportPrint" name="IframeReportPrint" width="0" height="0" runat="server"
            style="left: -1000; top: 2000;" clientidmode="static" scrolling="yes"></iframe>
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
    <div id="SalesOrderDiv" style="display: none;">
    </div>    
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {
                var barControlId = CommonHelper.GetReportViewerControlId($("#ContentPlaceHolder1_rvTransactionShow"));
                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 25px; width: 50px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(' + barControlId + '); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }

            var hfCreditSale = $("#<%=hfCreditSale.ClientID %>").val();
            //alert(hfCreditSale);
            if (hfCreditSale == 1) {
                $("#PaymentHead").hide();
                $("#PaymentListDiv").hide();
                $("#PayModeDiv").hide();
                $("#CardPaymentAccountHeadDiv").hide();
                $("#ChecquePaymentAccountHeadDiv").hide();
                $("#SaveButton").hide();
                $("#CompanyDiv").show();
                //$("#ContentPlaceHolder1_btnSave").prop("disabled", false);
                //$("#ContentPlaceHolder1_btnPreview").prop("disabled", false);
                //$("#ContentPlaceHolder1_btnChallan").prop("disabled", false);
                $("#ItemListDiv").addClass("FullWidth");
                $("#ItemDetailsDiv").hide();
            }
            else {
                $("#PaymentHead").show();
                $("#PaymentListDiv").show();
                $("#PayModeDiv").show();
                $("#SaveButton").show();
                //                $("#ContentPlaceHolder1_btnSave").prop("disabled", true);
                //                $("#ContentPlaceHolder1_btnPreview").prop("disabled", true);
                //                $("#ContentPlaceHolder1_btnChallan").prop("disabled", true);
                $("#ItemListDiv").addClass("HalfWidth");
            }
            $("#ContentPlaceHolder1_btnSave").prop("disabled", true);
            $("#ContentPlaceHolder1_btnPreview").prop("disabled", true);
            $("#ContentPlaceHolder1_btnChallan").prop("disabled", true);
            $("#AppraisalAccordion").accordion();
            $("#ItemBody").addClass("visible");

            $("#ItemHead").click(function () {
                if ($("#ItemBody").hasClass("hidden")) {
                    $("#ItemBody").removeClass("hidden").addClass("visible");

                } else {
                    $("#ItemBody").removeClass("visible").addClass("hidden");
                }
            });
        });
    </script>
</asp:Content>
