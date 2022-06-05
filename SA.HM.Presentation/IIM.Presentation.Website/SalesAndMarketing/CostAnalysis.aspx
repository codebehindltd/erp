<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="CostAnalysis.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.CostAnalysis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var SearchItem = null;
        var SearchService = null;
        var ItemDetails = new Array();
        var DeletedItemDetails = new Array();

        var ServiceDetails = new Array();
        var DeletedServiceDetails = new Array();
        var DownloadBandwidth = new Array();
        var ContactPeriod = new Array();
        var isFromQuotation = false;
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: false,
                placeholder: "--- ALL ---",
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateCostAnalysis")
            });

            $("#ContentPlaceHolder1_ddlCategoryService").select2({
                tags: false,
                placeholder: "--- ALL ---",
                allowClear: true,
                width: "99.75%",
                dropdownParent: $("#CreateCostAnalysis")
            });

            $("#txtItemName").autocomplete({
                source: function (request, response) {
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    var isCustomerItem = 1;
                    var itemtype = "";

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/frmSMQuotation.aspx/ItemNCategoryAutoSearch',
                        data: "{'itemName':'" + request.term + "','categoryId':'" + categoryId + "','isCustomerItem':'" + isCustomerItem + "', 'itemType':'" + itemtype + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,

                                    Name: m.Name,
                                    CategoryId: m.CategoryId,
                                    ItemId: m.ItemId,
                                    StockById: m.StockBy,
                                    UnitPriceLocal: m.UnitPriceLocal,
                                    UnitHead: m.UnitHead,
                                    AverageCost: m.AverageCost
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

                    SearchItem = ui.item;
                    $(this).val(ui.item.label);
                    $("#<%=hfItemId.ClientID %>").val(ui.item.value);
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/frmSMQuotation.aspx/LoadRelatedStockBy',
                        data: "{'stockById':'" + ui.item.StockById + "'}",
                        dataType: "json",
                        success: function (data) {
                            OnLoadStockBySucceeded(data.d);
                        },
                        error: function (error) {
                            OnLoadStockByFailed(error);
                        }
                    });
                }
            }).autocomplete("option", "appendTo", "#CreateCostAnalysis");

            $("#txtServiceItem").autocomplete({
                source: function (request, response) {
                    var categoryId = $("#ContentPlaceHolder1_ddlCategoryService").val();
                    var isCustomerItem = 0;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/frmSMQuotation.aspx/GetItemServiceByCategory',
                        data: "{'itemName':'" + request.term + "','categoryId':'" + categoryId + "','isCustomerItem':'" + isCustomerItem + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,

                                    Name: m.Name,
                                    CategoryId: m.CategoryId,
                                    ItemId: m.ItemId,
                                    StockById: m.StockBy,
                                    UnitPriceLocal: m.UnitPriceLocal,
                                    UnitHead: m.UnitHead,
                                    AverageCost: m.AverageCost
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
                    event.preventDefault();
                },
                select: function (event, ui) {
                    event.preventDefault();
                    SearchService = ui.item;

                    $("#ContentPlaceHolder1_ddlServiceStockBy").val(ui.item.StockById);
                    $(this).val(ui.item.label);
                    $("#<%=hfItemServiceId.ClientID %>").val(ui.item.value);

                    GetServicePriceMatrix();
                }
            }).autocomplete("option", "appendTo", "#CreateCostAnalysis");

            $("#ContentPlaceHolder1_ddlPackage").change(function () {
                GetServicePriceMatrix();
            });


            $("#ContentPlaceHolder1_ddlServiceBandwidth").change(function () {
                GetServicePriceMatrix();
            });

            if ($("#ContentPlaceHolder1_hfDownloadBandwidth").val() != "") {
                DownloadBandwidth = JSON.parse($("#ContentPlaceHolder1_hfDownloadBandwidth").val());
            }
            if ($("#ContentPlaceHolder1_hfContactPeriod").val() != "") {
                ContactPeriod = JSON.parse($("#ContentPlaceHolder1_hfContactPeriod").val());
            }
            CommonHelper.ApplyDecimalValidation();

            var costAnalysisId = Math.trunc($.trim(CommonHelper.GetParameterByName("caid")));
            var isFromQuotation = $.trim(CommonHelper.GetParameterByName("isfrmqtn")) == "1";
            if (costAnalysisId > 0 && !isFromQuotation)
                PerformEditAction(costAnalysisId);
            if (isFromQuotation)
                LoadQuotation();
        });


        function GetServicePriceMatrix() {

            var itemId = $("#ContentPlaceHolder1_hfItemServiceId").val();
            var servicePackageId = $("#ContentPlaceHolder1_ddlPackage").val();
            var serviceBandWidthId = $("#ContentPlaceHolder1_ddlServiceBandwidth").val();

            if (itemId == "0" || servicePackageId == "0" || serviceBandWidthId == "0") {
                return false;
            }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/frmSMQuotation.aspx/GetServicePriceMatrix',
                data: "{'itemId':'" + itemId + "','servicePackageId':'" + servicePackageId + "','serviceBandWidthId':'" + serviceBandWidthId + "'}",
                dataType: "json",
                success: function (data) {
                    OnLoadServicePriceMatrixSucceeded(data.d);
                },
                error: function (error) {
                    OnLoadServicePriceMatrixFailed(error);
                }
            });
            return false;
        }

        function OnLoadServicePriceMatrixSucceeded(result) {
            $("#ContentPlaceHolder1_txtUnitPrice").val(result.UnitPrice);
            return false;
        }

        function OnLoadServicePriceMatrixFailed() { }

        function OnLoadStockBySucceeded(result) {

            var list = result;
            var ddlStockById = '<%=ddlItemStockBy.ClientID%>';
            var control = $('#' + ddlStockById);
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
            toastr.error("Please Contact With Admin");
        }

        function AddItem() {

            if (SearchItem == null) {
                return false;
            }

            var existingItem = _.findWhere(ItemDetails, { ItemId: SearchItem.ItemId });
            if (existingItem != null) {
                toastr.warning("Same Item Already Added.");
                return false;
            }

            var tr = "", totalRow = 0, deleteLink = "", itemQuantity = "0", itemCost = 0.00, itemOfferedPrice = 0.00, itemTotalPrice = 0.00, id = 0, itemName = "", itemAdditionalCost = 0.00;
            itemQuantity = $("#ContentPlaceHolder1_txtItemUnit").val();
            itemAdditionalCost = $("#ContentPlaceHolder1_txtAdditionalCost").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtAdditionalCost").val()) : 0;
            itemOfferedPrice = parseFloat(itemQuantity) * SearchItem.UnitPriceLocal;
            itemCost = parseFloat(itemQuantity) * SearchItem.AverageCost;
            itemTotalPrice = itemCost + itemAdditionalCost;
            totalRow = $("#QuotationItemInformation tbody tr").length;
            itemName = $("#txtItemName").val();

            if ($.trim(itemQuantity) == "0" || $.trim(itemQuantity) == "") { toastr.warning("Please Give Item Unit."); return false; }
            else if (SearchItem == null || $.trim(itemName) == "") { toastr.warning("Please Select Item."); return false; }

            deleteLink = "<a href=\"javascript:void()\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }
            tr += "<td align='left' style=\"display:none;\">" + id + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + SearchItem.ItemId + "</td>";
            tr += "<td align='left' style=\"width:25%; text-align:Left;\">" + SearchItem.Name + "</td>";

            tr += "<td align='left' style=\"width:5%; text-align:Left;\">" +
                " <input type=\"text\" id='itm" + SearchItem.ItemId + "' value='" + itemQuantity + "' class=\"form-control\" onblur='EditQuantity(this)' />" +
                "</td>";

            tr += "<td align='left' style=\"width:10%; text-align:Left;\">" +
                " <input type=\"text\" " + "' value='" + SearchItem.UnitPriceLocal + "' class=\"form-control\" onblur='EditQuantity(this)' />" +
                "</td>";
            tr += "<td align='left' style='width: 10%;'>" + itemOfferedPrice + "</td>";
            tr += "<td align='left' style=\"width:10%;\">" + SearchItem.AverageCost + "</td>";
            tr += "<td align='left' style='width: 10%;'>" + itemCost + "</td>";
            tr += "<td align='left' style=\"width: 10%\">" + itemAdditionalCost + "</td>";
            tr += "<td align='left' style=\"width: 15%\">" + itemTotalPrice + "</td>";
            tr += "<td align='center' style=\"width:5%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#QuotationItemInformation tbody").append(tr);

            ItemDetails.push({

                Id: 0,
                SMCostAnalysisId: 0,
                ItemType: 'Item',
                CategoryId: SearchItem.CategoryId,

                ServicePackageId: 0,
                ServiceBandWidthId: 0,
                ServiceTypeId: 0,

                ItemId: SearchItem.ItemId,
                StockBy: SearchItem.StockById,
                Quantity: parseFloat(itemQuantity),
                UnitPrice: SearchItem.UnitPriceLocal,
                TotalOfferedPrice: itemOfferedPrice,
                AverageCost: SearchItem.AverageCost,
                TotalCost: itemCost,
                AdditionalCost: itemAdditionalCost,
                TotalProjetcedCost: itemTotalPrice
            });

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00, totalAverageCost = 0.00, totalItemOfferedPrice = 0.00, totalAdditionalCost = 0.00, totalPrice = 0.00;

            $.each(ItemDetails, function (index, itm) {
                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalItemOfferedPrice = totalItemOfferedPrice + itm.TotalOfferedPrice;
                totalAverageCost = totalAverageCost + itm.AverageCost;
                totalCost = totalCost + itm.TotalCost;
                totalAdditionalCost = totalAdditionalCost + itm.AdditionalCost;
                totalPrice = totalPrice + itm.TotalProjetcedCost;
            });

            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text(totalItemOfferedPrice);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(4)").text(totalAverageCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(5)").text(totalCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(6)").text(totalAdditionalCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(7)").text(totalPrice);
            UpdateGrandTotal();
            SearchItem = null;
            $("#txtItemName").val('');
            $("#ContentPlaceHolder1_ddlItemStockBy").val("0");
            $("#ContentPlaceHolder1_txtItemUnit").val("");
            $("#ContentPlaceHolder1_txtAdditionalCost").val("");
            $("#txtItemName").focus();
        }

        function AddServiceItem() {

            if (SearchService == null) {
                return false;
            }

            var tr = "", totalRow = 0, deleteLink = "", itemQuantity = "0", itemCost = 0.00, quoTationId = 0;
            var packageId = 0, bandwidthId = 0, serviceTypeId = 0, contractPeriodId = 0, contractPeriodValue = 0, serviceItemName = "";

            var serviceType = "", packageName = "", downLink = "", upLink = "", pricePerMonth = 0.00, priceTotalByContactPeriod = 0.00, itemOfferedPrice = 0.00, itemAdditionalCost = 0.00, itemTotalPrice = 0.00;

            packageId = $("#ContentPlaceHolder1_txtServiceQuantity").val();
            bandwidthId = $("#ContentPlaceHolder1_ddlServiceBandwidth").val();
            serviceTypeId = $("#ContentPlaceHolder1_ddlBandwidthServiceType").val();
            contractPeriodId = $("#ContentPlaceHolder1_ddlContractPeriod").val();
            itemQuantity = $("#ContentPlaceHolder1_txtServiceQuantity").val();

            serviceTypeId = $("#ContentPlaceHolder1_ddlBandwidthServiceType").val();
            packageId = $("#ContentPlaceHolder1_ddlPackage").val();
            bandwidthId = $("#ContentPlaceHolder1_ddlServiceBandwidth").val();
            pricePerMonth = $("#ContentPlaceHolder1_txtUnitPrice").val();
            serviceItemName = $("#txtServiceItem").val();

            if (contractPeriodId == "0") { toastr.warning("Please Select Contract Period."); return false; }
            else if (SearchService == null || $.trim(serviceItemName) == "") { toastr.warning("Please Select Service."); return false; }
            else if ($.trim(itemQuantity) == "0" || $.trim(itemQuantity) == "") { toastr.warning("Please Give Service Unit."); return false; }
            else if ($.trim(serviceTypeId) == "0" || $.trim(serviceTypeId) == "") { toastr.warning("Please Select Service Type."); return false; }
            else if ($.trim(pricePerMonth) == "0" || $.trim(pricePerMonth) == "") { toastr.warning("Please Give Unit Price."); return false; }
            else if ($.trim(bandwidthId) == "0" || $.trim(bandwidthId) == "") { toastr.warning("Please Select Bandwidth"); return false; }
            else if ($.trim(packageId) == "0" || $.trim(packageId) == "") { toastr.warning("Please Select package."); return false; }

            var row = 0, rowLength = ServiceDetails.length, index = -1;

            for (row = 0; row < rowLength; row++) {
                if (ServiceDetails[row].ServicePackageId == parseInt(packageId, 10) &&
                    ServiceDetails[row].ServiceTypeId == parseInt(serviceTypeId, 10) &&
                    ServiceDetails[row].ItemId == parseInt(SearchService.ItemId, 10)) {

                    index = row;
                    break;
                }
            }

            if (index >= 0) {
                toastr.warning("Same Service Already Added.");
                return false;
            }

            if (DownloadBandwidth.length != 0) {
                var dl = _.findWhere(DownloadBandwidth, { ServiceBandWidthId: parseInt(bandwidthId) });
                downLink = dl.Downlink != null ? dl.Downlink : 0;
                upLink = dl.Uplink;
            }

            if (ContactPeriod.length != 0) {
                var cp = _.findWhere(ContactPeriod, { ContractPeriodId: parseInt(contractPeriodId) });
                contractPeriodValue = cp.ContractPeriodValue;
            }

            serviceType = $("#ContentPlaceHolder1_ddlBandwidthServiceType option:selected").text();
            packageName = $("#ContentPlaceHolder1_ddlPackage option:selected").text();

            priceTotalByContactPeriod = pricePerMonth * contractPeriodValue;
            itemOfferedPrice = priceTotalByContactPeriod * parseFloat(itemQuantity);
            itemCost = SearchService.AverageCost * parseFloat(itemQuantity);
            itemAdditionalCost = $("#ContentPlaceHolder1_txtServiceAdditionalCost").val() != "" ? parseFloat($("#ContentPlaceHolder1_txtServiceAdditionalCost").val()) : 0;
            itemTotalPrice = itemCost + itemAdditionalCost;

            totalRow = $("#QuotationServicenformation tbody tr").length;
            deleteLink = "<a href=\"javascript:void()\" onclick= \"DeleteServiceItem(this," + SearchService.ItemId + "," + packageId + "," + serviceTypeId + ")\" ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            //$("#QuotationServicenformation thead tr:eq(0)").find("th:eq(6)").text("Price/ " + contractPeriodValue + " Month");

            if ((totalRow % 2) == 0) {
                tr += "<tr style=\"background-color:#E3EAEB;\">";
            }
            else {
                tr += "<tr style=\"background-color:#FFFFFF;\">";
            }
            tr += "<td align='left' style=\"display:none;\">" + quoTationId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + SearchService.ItemId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + SearchService.StockById + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + itemQuantity + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + packageId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + bandwidthId + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + serviceTypeId + "</td>";

            tr += "<td align='left' style=\"width:25%; text-align:Left;\">" + SearchService.Name + "</td>";

            tr += "<td align='left' style=\"width:5%; text-align:Left;\">" +
                " <input type=\"text\" id='itm" + SearchService.ItemId + "' value='" + itemQuantity + "' class=\"form-control\" onblur='EditServiceQuantity(this)' />" +
                "</td>";

            tr += "<td align='left' style=\"width:10%; text-align:Left;\">" +
                " <input type=\"text\" disabled='disabled' " + "' value='" + pricePerMonth + "' class=\"form-control\" onblur='EditServiceQuantity(this)' />" +
                "</td>";
            tr += "<td align='left' style='width: 10%;'>" + itemOfferedPrice + "</td>";
            tr += "<td align='left' style=\"width:10%;\">" + SearchService.AverageCost + "</td>";
            tr += "<td align='left' style='width: 10%;'>" + itemCost + "</td>";
            tr += "<td align='left' style=\"width: 10%\">" + itemAdditionalCost + "</td>";
            tr += "<td align='left' style=\"width: 15%\">" + itemTotalPrice + "</td>";
            tr += "<td align='center' style=\"width:5%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#QuotationServicenformation tbody").append(tr);

            ServiceDetails.push({

                Id: 0,
                SMCostAnalysisId: 0,
                ItemType: 'Service',
                CategoryId: SearchService.CategoryId,
                ServicePackageId: packageId,
                ServiceBandWidthId: bandwidthId,
                ServiceTypeId: serviceTypeId,
                ItemId: SearchService.ItemId,
                StockBy: SearchService.StockById,
                Quantity: parseFloat(itemQuantity),
                UnitPrice: parseFloat(pricePerMonth),
                TotalPrice: parseFloat(priceTotalByContactPeriod),
                UpLink: upLink,
                TotalOfferedPrice: itemOfferedPrice,
                AverageCost: SearchService.AverageCost,
                TotalCost: itemCost,
                AdditionalCost: itemAdditionalCost,
                TotalProjetcedCost: itemTotalPrice
            });

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00, totalAverageCost = 0.00, totalItemOfferedPrice = 0.00, totalAdditionalCost = 0.00, totalPrice = 0.00;

            $.each(ServiceDetails, function (index, itm) {
                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalItemOfferedPrice = totalItemOfferedPrice + itm.TotalOfferedPrice;
                totalAverageCost = totalAverageCost + itm.AverageCost;
                totalCost = totalCost + itm.TotalCost;
                totalAdditionalCost = totalAdditionalCost + itm.AdditionalCost;
                totalPrice = totalPrice + itm.TotalProjetcedCost;
            });

            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(3)").text(totalItemOfferedPrice);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(4)").text(totalAverageCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(5)").text(totalCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(6)").text(totalAdditionalCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(7)").text(totalPrice);
            UpdateGrandTotal();

            SearchService = null;
            $("#txtServiceItem").val('');
            $("#ContentPlaceHolder1_ddlServiceStockBy").val("0");
            $("#ContentPlaceHolder1_ddlPackage").val("0");
            $("#ContentPlaceHolder1_ddlServiceBandwidth").val("0");
            $("#ContentPlaceHolder1_ddlBandwidthServiceType").val("0");
            $("#ContentPlaceHolder1_ddlContractPeriod").val("0");
            $("#ContentPlaceHolder1_txtServiceQuantity").val("");
            $("#ContentPlaceHolder1_txtUnitPrice").val("");
            $("#ContentPlaceHolder1_txtServiceAdditionalCost").val("");
            $("#txtServiceItem").focus();
        }

        function DeleteItem(tr) {
            tr = $(tr).parent().parent();

            var itemId = parseInt($(tr).find("td:eq(1)").text());
            var item = _.findWhere(ItemDetails, { ItemId: parseInt(itemId, 10) });

            if (item != null) {

                if (item.QuotationDetailsId != 0) {
                    DeletedItemDetails.push(item);
                }
                var index = ItemDetails.indexOf(item);
                ItemDetails.splice(index, 1);

                var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00, totalAverageCost = 0.00, totalItemOfferedPrice = 0.00, totalAdditionalCost = 0.00, totalPrice = 0.00;

                $.each(ItemDetails, function (index, itm) {
                    totalQuantity = totalQuantity + itm.Quantity;
                    totalUnitCost = totalUnitCost + itm.UnitPrice;
                    totalItemOfferedPrice = totalItemOfferedPrice + itm.TotalOfferedPrice;
                    totalAverageCost = totalAverageCost + itm.AverageCost;
                    totalCost = totalCost + itm.TotalCost;
                    totalAdditionalCost = totalAdditionalCost + itm.AdditionalCost;
                    totalPrice = totalPrice + itm.TotalProjetcedCost;
                });

                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text(totalItemOfferedPrice);
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(4)").text(totalAverageCost);
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(5)").text(totalCost);
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(6)").text(totalAdditionalCost);
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(7)").text(totalPrice);
                UpdateGrandTotal();

                $(tr).remove();
            }
        }

        function DeleteServiceItem(tr, itemId, servicePackageId, serviceTypeId) {
            tr = $(tr).parent().parent();

            var itemId = parseInt($(tr).find("td:eq(1)").text());
            var item = _.findWhere(ServiceDetails, { ItemId: parseInt(itemId, 10) });
            var row = 0, rowLength = ServiceDetails.length, index = -1;

            for (row = 0; row < rowLength; row++) {
                if (ServiceDetails[row].ServicePackageId == parseInt(servicePackageId, 10) &&
                    ServiceDetails[row].ServiceTypeId == parseInt(serviceTypeId, 10) &&
                    ServiceDetails[row].ItemId == parseInt(itemId, 10)) {

                    index = row;
                    if (ServiceDetails[row].QuotationDetailsId != 0) {
                        DeletedItemDetails.push(ServiceDetails[row]);
                    }

                    break;
                }
            }

            if (index >= 0) {

                ServiceDetails.splice(index, 1);

                var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00, totalAverageCost = 0.00, totalItemOfferedPrice = 0.00, totalAdditionalCost = 0.00, totalPrice = 0.00;

                $.each(ItemDetails, function (index, itm) {
                    totalQuantity = totalQuantity + itm.Quantity;
                    totalUnitCost = totalUnitCost + itm.UnitPrice;
                    totalItemOfferedPrice = totalItemOfferedPrice + itm.TotalOfferedPrice;
                    totalAverageCost = totalAverageCost + itm.AverageCost;
                    totalCost = totalCost + itm.TotalCost;
                    totalAdditionalCost = totalAdditionalCost + itm.AdditionalCost;
                    totalPrice = totalPrice + itm.TotalProjetcedCost;
                });

                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(3)").text(totalItemOfferedPrice);
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(4)").text(totalAverageCost);
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(5)").text(totalCost);
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(6)").text(totalAdditionalCost);
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(7)").text(totalPrice);
                UpdateGrandTotal();

                $(tr).remove();
            }

            return false;
        }

        function EditQuantity(tr) {

            tr = $(tr).parent().parent();

            var itemId = parseInt($(tr).find("td:eq(1)").text());
            var item = _.findWhere(ItemDetails, { ItemId: itemId });

            var quantity = parseInt($(tr).find("td:eq(3)").find("input").val());
            var unitPrice = parseInt($(tr).find("td:eq(4)").find("input").val());

            if (item != null) {
                item.Quantity = parseFloat(quantity);
                item.UnitPrice = parseFloat(unitPrice);
                item.TotalOfferedPrice = item.UnitPrice * item.Quantity;
            }
            $(tr).find("td:eq(5)").text(item.TotalOfferedPrice);

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00, totalAverageCost = 0.00, totalItemOfferedPrice = 0.00, totalAdditionalCost = 0.00, totalPrice = 0.00;

            $.each(ItemDetails, function (index, itm) {
                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalItemOfferedPrice = totalItemOfferedPrice + itm.TotalOfferedPrice;
                totalAverageCost = totalAverageCost + itm.AverageCost;
                totalCost = totalCost + itm.TotalCost;
                totalAdditionalCost = totalAdditionalCost + itm.AdditionalCost;
                totalPrice = totalPrice + itm.TotalProjetcedCost;
            });

            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text(totalItemOfferedPrice);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(4)").text(totalAverageCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(5)").text(totalCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(6)").text(totalAdditionalCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(7)").text(totalPrice);
            UpdateGrandTotal();

        }

        function EditServiceQuantity(tr) {

            tr = $(tr).parent().parent();

            var itemId = parseInt($(tr).find("td:eq(1)").text());
            var item = _.findWhere(ServiceDetails, { ItemId: itemId });

            var quantity = parseInt($(tr).find("td:eq(8)").find("input").val());
            var unitPrice = parseInt($(tr).find("td:eq(9)").find("input").val());

            if (item != null) {
                item.TotalPrice = item.TotalOfferedPrice / item.Quantity;
                item.Quantity = parseFloat(quantity);
                item.TotalOfferedPrice = item.TotalPrice * item.Quantity;
            }
            $(tr).find("td:eq(10)").text(item.TotalOfferedPrice);

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00, totalAverageCost = 0.00, totalItemOfferedPrice = 0.00, totalAdditionalCost = 0.00, totalPrice = 0.00;

            $.each(ServiceDetails, function (index, itm) {
                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalItemOfferedPrice = totalItemOfferedPrice + itm.TotalOfferedPrice;
                totalAverageCost = totalAverageCost + itm.AverageCost;
                totalCost = totalCost + itm.TotalCost;
                totalAdditionalCost = totalAdditionalCost + itm.AdditionalCost;
                totalPrice = totalPrice + itm.TotalProjetcedCost;
            });

            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(3)").text(totalItemOfferedPrice);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(4)").text(totalAverageCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(5)").text(totalCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(6)").text(totalAdditionalCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(7)").text(totalPrice);
            UpdateGrandTotal();

        }

        function UpdateGrandTotal() {
            var totalItemQuantity = 0.00, totalItemUnitCost = 0.00, totalItemOfferedPrice = 0.00, totalItemAverageCost = 0.00, totalItemCost = 0.00, totalItemAdditionalCost = 0.00, totalItemPrice = 0.00;
            var totalServiceQuantity = 0.00, totalServiceUnitCost = 0.00, totalServiceOfferedPrice = 0.00, totalServiceAverageCost = 0.00, totalServiceCost = 0.00, totalServiceAdditionalCost = 0.00, totalServicePrice = 0.00;

            totalItemQuantity = $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text() != "" ? parseFloat($("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text()) : 0;
            totalItemUnitCost = $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text() != "" ? parseFloat($("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text()) : 0;
            totalItemOfferedPrice = $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text() != "" ? parseFloat($("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text()) : 0;
            totalItemAverageCost = $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(4)").text() != "" ? parseFloat($("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(4)").text()) : 0;
            totalItemCost = $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(5)").text() != "" ? parseFloat($("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(5)").text()) : 0;
            totalItemAdditionalCost = $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(6)").text() != "" ? parseFloat($("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(6)").text()) : 0;
            totalItemPrice = $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(7)").text() != "" ? parseFloat($("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(7)").text()) : 0;

            totalServiceQuantity = $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text() != "" ? parseFloat($("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text()) : 0;
            totalServiceUnitCost = $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text() != "" ? parseFloat($("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text()) : 0;
            totalServiceOfferedPrice = $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(3)").text() != "" ? parseFloat($("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(3)").text()) : 0;
            totalServiceAverageCost = $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(4)").text() != "" ? parseFloat($("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(4)").text()) : 0;
            totalServiceCost = $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(5)").text() != "" ? parseFloat($("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(5)").text()) : 0;
            totalServiceAdditionalCost = $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(6)").text() != "" ? parseFloat($("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(6)").text()) : 0;
            totalServicePrice = $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(7)").text() != "" ? parseFloat($("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(7)").text()) : 0;

            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(1)").text(totalItemQuantity + totalServiceQuantity);
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(2)").text(totalItemUnitCost + totalServiceUnitCost);
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(3)").text(totalItemOfferedPrice + totalServiceOfferedPrice);
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(4)").text(totalItemAverageCost + totalServiceAverageCost);
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(5)").text(totalItemCost + totalServiceCost);
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(6)").text(totalItemAdditionalCost + totalServiceAdditionalCost);
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(7)").text(totalItemPrice + totalServicePrice);

            CalculateDiscount();
            UpdateProfit();
        }

        function ClearForm() {
            //$("#form1")[0].reset();
            isFromQuotation = false;
            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#ContentPlaceHolder1_hfCostAnalysisId").val("0");
            $("#ContentPlaceHolder1_txtName").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_ddlDiscountType").val("Fixed");
            $("#ContentPlaceHolder1_txtDiscountAmount").val("");

            $("#QuotationItemInformation tbody").html("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(4)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(5)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(6)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(7)").text("");

            $("#QuotationServicenformation tbody").html("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(3)").text("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(4)").text("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(5)").text("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(6)").text("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(7)").text("");

            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(1)").text("");
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(2)").text("");
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(3)").text("");
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(4)").text("");
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(5)").text("");
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(6)").text("");
            $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(7)").text("");

            $("#tblGrandTotal tfoot tr:eq(1)").find("td:eq(2)").text("");
            $("#tblGrandTotal tfoot tr:eq(2)").find("td:eq(2)").text("");

            $("#ContentPlaceHolder1_txtAdditionalCost").val("");
            $("#ContentPlaceHolder1_txtServiceAdditionalCost").val("");
            $("#ContentPlaceHolder1_lblProfitOnCost").text("");
            $("#ContentPlaceHolder1_lblProfitPercentage").text("");
            $("#btnSave").val("Save");

            ItemDetails = new Array();
            DeletedItemDetails = new Array();

            ServiceDetails = new Array();
            DeletedServiceDetails = new Array();
        }

        function CalculateDiscount() {
            var transactionalAmount = 0.00, discountType = '', discount = 0.00, discountAmount = 0.00, discountedAmount = 0.00;

            discountType = $("#ContentPlaceHolder1_ddlDiscountType").val();
            discount = $("#ContentPlaceHolder1_txtDiscountAmount").val() == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtDiscountAmount").val());

            if ($.trim($("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(3)").text()) != "")
                transactionalAmount = parseFloat($("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(3)").text());

            if (discountType == "Percentage" && discount > 100) {
                toastr.warning("Discount Cannot be Greater Than 100%.");
                $("#ContentPlaceHolder1_txtDiscountAmount").focus();
                return false;
            }
            else if (discount > transactionalAmount) {
                toastr.warning("Discount Cannot be Grand Total.");
                $("#ContentPlaceHolder1_txtDiscountAmount").focus();
                return false;
            }

            discountAmount = discountType == 'Percentage' ? transactionalAmount * (discount / 100.00) : discount;

            discountedAmount = transactionalAmount - discountAmount;
            $("#tblGrandTotal tfoot tr:eq(1)").find("td:eq(2)").text(discountAmount);
            $("#tblGrandTotal tfoot tr:eq(2)").find("td:eq(2)").text(discountedAmount);
            UpdateProfit();
        }

        function UpdateProfit() {
            var discountedAmount = $("#tblGrandTotal tfoot tr:eq(2)").find("td:eq(2)").text();
            discountedAmount = discountedAmount != "" ? parseFloat(discountedAmount) : 0.00;

            var totalProjectedCost = $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(7)").text();
            totalProjectedCost = totalProjectedCost != "" ? parseFloat(totalProjectedCost) : 0.00;
            var profit = discountedAmount - totalProjectedCost;

            $("#ContentPlaceHolder1_lblProfitOnCost").text(profit);
            if (discountedAmount != 0)
                $("#ContentPlaceHolder1_lblProfitPercentage").text((profit / discountedAmount * 100).toFixed(2));
        }

        function SaveCostAnalysis() {

            var id = "0", name, grandTotal = 0.00;

            id = $("#ContentPlaceHolder1_hfCostAnalysisId").val();
            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            name = $("#ContentPlaceHolder1_txtName").val();

            if (!name) {
                toastr.warning("Please Enter Name.");
                $("#ContentPlaceHolder1_txtName").focus();
                return false;
            }
            if (ItemDetails.length == 0 && ServiceDetails.length == 0) {
                toastr.warning("Please Add Item / Service");
                return false;
            }

            grandTotal = parseFloat($("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(3)").text());
            discountType = $("#ContentPlaceHolder1_ddlDiscountType").val();
            discountAmount = $("#ContentPlaceHolder1_txtDiscountAmount").val() == "" ? 0.00 : parseFloat($("#ContentPlaceHolder1_txtDiscountAmount").val());

            calculatedDiscountAmount = parseFloat($("#tblGrandTotal tfoot tr:eq(1)").find("td:eq(2)").text());
            discountedAmount = parseFloat($("#tblGrandTotal tfoot tr:eq(2)").find("td:eq(2)").text());

            var totalProjectedCost = $("#tblGrandTotal tfoot tr:eq(0)").find("td:eq(7)").text();
            totalProjectedCost = totalProjectedCost != "" ? parseFloat(totalProjectedCost) : 0.00;


            var CostAnalysis = {
                Id: id,
                Name: name,
                GrandTotal: grandTotal,
                DiscountType: discountType,
                DiscountAmount: discountAmount,
                CalculatedDiscountAmount: calculatedDiscountAmount,
                DiscountedAmount: discountedAmount,
                TotalCost: totalProjectedCost,
                Remarks: remarks
            };

            PageMethods.SaveCostAnalysis(CostAnalysis, ItemDetails, ServiceDetails, DeletedItemDetails, DeletedServiceDetails, OnSaveQuotationSucceed, OnSaveQuotationFailed);

            return false;
        }

        function OnSaveQuotationSucceed(result) {
            if (result.IsSuccess) {
                if (typeof parent.ShowAlert === "function")
                    parent.ShowAlert(result.AlertMessage);
                ClearForm();
                if (typeof parent.GridPaging === "function") {
                    var activeLink = Math.trunc($(parent.WebForm_GetElementById("GridPagingContainer")).find("ul li.active").text());
                    parent.GridPaging(activeLink, 1);
                }
                if (typeof parent.CloseDialog === "function")
                    parent.CloseDialog();
            }
            else {
                if (typeof parent.ShowAlert === "function")
                    parent.ShowAlert(result.AlertMessage);
            }
            return false;
        }

        function OnSaveQuotationFailed(error) {
            CommonHelper.AlertMessage(error);
        }

        function PerformEditAction(id) {

            PageMethods.GetCostAnalysisById(id, OnLoadCostAnalysisSuccess, OnLoadCostAnalysisFail);
            return false;
        }

        function OnLoadCostAnalysisSuccess(result) {
            //ClearForm();

            $("#ContentPlaceHolder1_hfCostAnalysisId").val(result.CostAnalysis.Id);
            $("#ContentPlaceHolder1_txtName").val(result.CostAnalysis.Name);
            $("#ContentPlaceHolder1_txtRemarks").val(result.CostAnalysis.Remarks);
            $("#ContentPlaceHolder1_ddlDiscountType").val(result.CostAnalysis.DiscountType);
            $("#ContentPlaceHolder1_txtDiscountAmount").val(result.CostAnalysis.DiscountAmount);

            var tr = "", deleteLink = "<a href=\"javascript:void()\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            ItemDetails = result.Items;
            ServiceDetails = result.Services;

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00, totalAverageCost = 0.00, totalItemOfferedPrice = 0.00, totalAdditionalCost = 0.00, totalPrice = 0.00;
            $.each(result.Items, function (index, itm) {

                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"display:none;\">" + itm.Id + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ItemId + "</td>";
                tr += "<td align='left' style=\"width:25%; text-align:Left;\">" + itm.ItemName + "</td>";

                tr += "<td align='left' style=\"width:5%; text-align:Left;\">" +
                    " <input type=\"text\" id='itm" + itm.ItemId + "' value='" + itm.Quantity + "' class=\"form-control\" onblur='EditQuantity(this)' />" +
                    "</td>";

                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" +
                    " <input type=\"text\" " + "' value='" + itm.UnitPrice + "' class=\"form-control\" onblur='EditQuantity(this)' />" +
                    "</td>";
                tr += "<td align='left' style='width: 10%;'>" + itm.TotalOfferedPrice + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + itm.AverageCost + "</td>";
                tr += "<td align='left' style='width: 10%;'>" + itm.TotalCost + "</td>";
                tr += "<td align='left' style=\"width: 10%\">" + itm.AdditionalCost + "</td>";
                tr += "<td align='left' style=\"width: 15%\">" + itm.TotalProjetcedCost + "</td>";
                tr += "<td align='center' style=\"width:5%; cursor:pointer;\">" + deleteLink + "</td>";

                tr += "</tr>";

                $("#QuotationItemInformation tbody").append(tr);
                tr = "";

                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalItemOfferedPrice = totalItemOfferedPrice + itm.TotalOfferedPrice;
                totalAverageCost = totalAverageCost + itm.AverageCost;
                totalCost = totalCost + itm.TotalCost;
                totalAdditionalCost = totalAdditionalCost + itm.AdditionalCost;
                totalPrice = totalPrice + itm.TotalProjetcedCost;
            });

            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text(totalItemOfferedPrice);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(4)").text(totalAverageCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(5)").text(totalCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(6)").text(totalAdditionalCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(7)").text(totalPrice);


            totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00, totalAverageCost = 0.00, totalItemOfferedPrice = 0.00, totalAdditionalCost = 0.00, totalPrice = 0.00;

            $.each(result.Services, function (index, itm) {
                deleteLink = "<a href=\"javascript:void()\" onclick= \"DeleteServiceItem(this," + itm.ItemId + "," + itm.ServicePackageId + "," + itm.ServiceTypeId + ")\" ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"display:none;\">" + itm.Id + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ItemId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.StockBy + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.Quantity + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServicePackageId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServiceBandWidthId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServiceTypeId + "</td>";
                tr += "<td align='left' style=\"width:25%; text-align:Left;\">" + itm.ItemName + "</td>";

                tr += "<td align='left' style=\"width:5%; text-align:Left;\">" +
                    " <input type=\"text\" id='itm" + itm.ItemId + "' value='" + itm.Quantity + "' class=\"form-control\" onblur='EditServiceQuantity(this)' />" +
                    "</td>";

                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" +
                    " <input type=\"text\" disabled='disabled'" + "' value='" + itm.UnitPrice + "' class=\"form-control\" onblur='EditServiceQuantity(this)' />" +
                    "</td>";
                tr += "<td align='left' style='width: 10%;'>" + itm.TotalOfferedPrice + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + itm.AverageCost + "</td>";
                tr += "<td align='left' style='width: 10%;'>" + itm.TotalCost + "</td>";
                tr += "<td align='left' style=\"width: 10%\">" + itm.AdditionalCost + "</td>";
                tr += "<td align='left' style=\"width: 15%\">" + itm.TotalProjetcedCost + "</td>";
                tr += "<td align='center' style=\"width:5%; cursor:pointer;\">" + deleteLink + "</td>";

                tr += "</tr>";

                $("#QuotationServicenformation tbody").append(tr);

                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalItemOfferedPrice = totalItemOfferedPrice + itm.TotalOfferedPrice;
                totalAverageCost = totalAverageCost + itm.AverageCost;
                totalCost = totalCost + itm.TotalCost;
                totalAdditionalCost = totalAdditionalCost + itm.AdditionalCost;
                totalPrice = totalPrice + itm.TotalProjetcedCost;
                tr = "";
            });

            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(3)").text(totalItemOfferedPrice);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(4)").text(totalAverageCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(5)").text(totalCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(6)").text(totalAdditionalCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(7)").text(totalPrice);

            UpdateGrandTotal();
            if (!isFromQuotation)
                $("#btnSave").val("Update");
        }

        function OnLoadCostAnalysisFail(error) {
            toastr.error(error.get_message());
        }

        function LoadQuotation() {
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/CostAnalysis.aspx/GetCostAnalysisFromQuotation',
                dataType: "json",
                async: false,
                success: function (data) {
                    isFromQuotation = true;
                    OnLoadCostAnalysisSuccess(data.d);
                },
                error: function (error) {
                    OnLoadCostAnalysisFail(error.d);
                }
            });
        }

        function LaodQuotaionItemAndService(result) {
            $.each(result.Items, function (index, itm) {
                SearchItem = {
                    label: itm.ItemName,
                    value: itm.ItemId,

                    Name: itm.ItemName,
                    CategoryId: itm.CategoryId,
                    ItemId: itm.ItemId,
                    StockById: itm.StockBy,
                    UnitPriceLocal: itm.UnitPrice,
                    UnitHead: itm.StockBy,
                    AverageCost: itm.AverageCost
                }
                $("#txtItemName").val(itm.ItemName);
                $("#ContentPlaceHolder1_txtItemUnit").val(itm.Quantity);
                $("#ContentPlaceHolder1_txtAdditionalCost").val('');
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../SalesAndMarketing/frmSMQuotation.aspx/LoadRelatedStockBy',
                    data: "{'stockById':'" + itm.StockBy + "'}",
                    dataType: "json",
                    success: function (data) {
                        OnLoadStockBySucceeded(data.d);
                    },
                    error: function (error) {
                        OnLoadStockByFailed(error);
                    }
                });
                AddItem();
            });

            $.each(result.Services, function (index, itm) {
                SearchItem = {
                    label: itm.Name,
                    value: itm.ItemId,

                    Name: itm.Name,
                    CategoryId: itm.CategoryId,
                    ItemId: itm.ItemId,
                    StockById: itm.StockBy,
                    UnitPriceLocal: itm.UnitPriceLocal,
                    UnitHead: itm.UnitHead
                }


                $("#ContentPlaceHolder1_txtServiceQuantity").val(itm.Quantity);
                $("#ContentPlaceHolder1_ddlServiceBandwidth").val(itm.ServiceBandWidthId);
                $("#ContentPlaceHolder1_ddlBandwidthServiceType").val(itm.ServiceTypeId);
                //$("#ContentPlaceHolder1_ddlContractPeriod").val(itm.);

                $("#ContentPlaceHolder1_ddlPackage").val(itm.ServicePackageId);
                GetServicePriceMatrix();
                //$("#ContentPlaceHolder1_txtUnitPrice").val(itm.UnitPrice);
                $("#txtServiceItem").val();

                if (ContactPeriod.length != 0) {
                    var cp = _.findWhere(ContactPeriod, { ContractPeriodId: parseInt(contractPeriodId) });
                    contractPeriodValue = cp.ContractPeriodValue;
                }

                AddServiceItem();
            });
        }

    </script>
    <asp:HiddenField ID="hfDownloadBandwidth" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfCostAnalysisId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfContactPeriod" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfItemServiceId" runat="server" Value="0"></asp:HiddenField>
    <div id="CreateCostAnalysis">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2 text-right">
                    <label class="control-label required-field">Name</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2 text-right">
                    <asp:Label ID="Label19" runat="server" class="control-label" Text="Remarks"></asp:Label>
                </div>
                <div class="col-md-8">
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                        TabIndex="12"></asp:TextBox>
                </div>
            </div>
            &nbsp;
            <div class="panel panel-default">
                <div class="panel-heading"><span id="ItemQuotationTitle">Product</span></div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:HiddenField ID="hfItemCategoryId" runat="server" Value="0" />
                                <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblItemName" runat="server" class="control-label" Text="Item Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtItemName" CssClass="form-control" TabIndex="14" runat="server"
                                    ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblItemStockBy" runat="server" class="control-label" Text="Unit"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlItemStockBy" runat="server" CssClass="form-control" TabIndex="15">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblItemUnit" runat="server" class="control-label" Text="Quantity"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtItemUnit" TabIndex="16" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Additional Cost"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAdditionalCost" TabIndex="16" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnAddItem" tabindex="17" class="btn btn-primary btn-sm" onclick="AddItem()">
                                    Add</button>
                            </div>
                        </div>
                        &nbsp;
                        <div class="form-group" style="padding: 0px;">
                            <div id="ItemTableContainer">
                                <table id="QuotationItemInformation" class="table table-condensed table-bordered table-responsive">
                                    <thead>
                                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                            <th style="display: none"></th>
                                            <th style="display: none"></th>
                                            <th scope="col" style="width: 25%;">Item Name</th>
                                            <th scope="col" style="width: 5%;">Quantity</th>
                                            <th scope="col" style="width: 10%;">Unit Price</th>
                                            <th scope="col" style="width: 10%;">Total Offered</th>
                                            <th scope="col" style="width: 10%;">Average Cost</th>
                                            <th scope="col" style="width: 10%;">Total Cost</th>
                                            <th scope="col" style="width: 10%;">Additional Cost</th>
                                            <th scope="col" style="width: 15%;">Total Projected Cost</th>
                                            <th scope="col" style="width: 5%;">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                    <tfoot style="background-color: #f0f7fa; color: black;">
                                        <tr>
                                            <td style="width: 25%; padding-right: 5px; text-align: right;">Product Total:</td>
                                            <td style="width: 5%;"></td>
                                            <td style="width: 10%;"></td>
                                            <td style="width: 10%;"></td>
                                            <td style="width: 10%;"></td>
                                            <td style="width: 10%;"></td>
                                            <td style="width: 10%;"></td>
                                            <td colspan="2" style="width: 20%;"></td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading"><span id="ItemServiceTitle">Service</span></div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Category"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCategoryService" CssClass="form-control" runat="server" TabIndex="18">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Item Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtServiceItem" CssClass="form-control" TabIndex="19" runat="server"
                                    ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Unit"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlServiceStockBy" runat="server" CssClass="form-control" TabIndex="20">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label4" runat="server" class="control-label" Text="Quantity"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtServiceQuantity" TabIndex="21" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label20" runat="server" class="control-label" Text="Package"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPackage" runat="server" CssClass="form-control" TabIndex="22">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label21" runat="server" class="control-label" Text="Bandwidth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlServiceBandwidth" runat="server" CssClass="form-control" TabIndex="23">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label18" runat="server" class="control-label required-field" Text="Contract Period"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlContractPeriod" runat="server" CssClass="form-control" TabIndex="6"></asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label22" runat="server" class="control-label" Text="Service Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBandwidthServiceType" runat="server" CssClass="form-control" TabIndex="25">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label23" runat="server" class="control-label" Text="Unit Price"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtUnitPrice" TabIndex="24" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Additional Cost"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtServiceAdditionalCost" TabIndex="16" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnAddServiceItem" tabindex="26" class="btn btn-primary btn-sm" onclick="AddServiceItem()">
                                    Add</button>
                            </div>
                        </div>
                        &nbsp;
                        <div class="form-group" style="padding: 0px;">
                            <div id="ServiceTableContainer">
                                <table id="QuotationServicenformation" class="table table-condensed table-bordered table-responsive">
                                    <thead>
                                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                            <th style="display: none">Id</th>
                                            <th style="display: none">Service Id</th>
                                            <th style="display: none">StockBy Id</th>
                                            <th style="display: none">Quantity</th>
                                            <th style="display: none">PackageId</th>
                                            <th style="display: none">BandwidthId</th>
                                            <th style="display: none">ServiceTypeId</th>
                                            <th scope="col" style="width: 25%;">Service Name</th>
                                            <th scope="col" style="width: 5%;">Quantity</th>
                                            <th scope="col" style="width: 10%;">Unit Price</th>
                                            <th scope="col" style="width: 10%;">Total Offered</th>
                                            <th scope="col" style="width: 10%;">Average Cost</th>
                                            <th scope="col" style="width: 10%;">Total Cost</th>
                                            <th scope="col" style="width: 10%;">Additional Cost</th>
                                            <th scope="col" style="width: 15%;">Total Projected Cost</th>
                                            <th scope="col" style="width: 5%;">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                    <tfoot style="background-color: #f0f7fa; color: black;">
                                        <tr>
                                            <td style="width: 25%; padding-right: 5px; text-align: right;">Service Total:</td>
                                            <td style="width: 5%;"></td>
                                            <td style="width: 10%;"></td>
                                            <td style="width: 10%;"></td>
                                            <td style="width: 10%;"></td>
                                            <td style="width: 10%;"></td>
                                            <td style="width: 10%;"></td>
                                            <td colspan="2" style="width: 20%;"></td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblDiscountType" runat="server" class="control-label" Text="Discount Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDiscountType" runat="server" CssClass="form-control" TabIndex="20" onchange="CalculateDiscount()">
                                <asp:ListItem Value="Fixed">Fixed</asp:ListItem>
                                <asp:ListItem Value="Percentage">Percentage</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblDiscountAmount" runat="server" class="control-label" Text="Discount Amount"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDiscountAmount" runat="server" TabIndex="21" CssClass="form-control quantitydecimal"
                                onblur="CalculateDiscount()"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" style="padding: 0px;">
                        <table id="tblGrandTotal" class="table table-condensed table-bordered table-responsive">
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                    <th scope="col" style="width: 25%;"></th>
                                    <th scope="col" style="width: 5%;">Quantity</th>
                                    <th scope="col" style="width: 10%;">Unit Price</th>
                                    <th scope="col" style="width: 10%;">Total Offered</th>
                                    <th scope="col" style="width: 10%;">Average Cost</th>
                                    <th scope="col" style="width: 10%;">Total Cost</th>
                                    <th scope="col" style="width: 10%;">Additional Cost</th>
                                    <th scope="col" style="width: 20%;">Total Projected Cost</th>
                                </tr>
                            </thead>
                            <tfoot style="background-color: #f0f7fa; color: black;">
                                <tr>
                                    <td style="width: 25%; text-align: right;">Grand Total:</td>
                                    <td style="width: 5%;"></td>
                                    <td style="width: 10%;"></td>
                                    <td style="width: 10%;"></td>
                                    <td style="width: 10%;"></td>
                                    <td style="width: 10%;"></td>
                                    <td style="width: 10%;"></td>
                                    <td colspan="2" style="width: 20%;"></td>
                                </tr>
                                <tr>
                                    <td style="width: 25%; text-align: right;">Less Discount (if any)</td>
                                    <td colspan="2" style="width: 15%;"></td>
                                    <td style="width: 10%;"></td>
                                </tr>
                                <tr>
                                    <td style="width: 25%; text-align: right;">Final Offered Price:</td>
                                    <td colspan="2" style="width: 15%;"></td>
                                    <td style="width: 10%;"></td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 label-align">
                            <asp:Label ID="Label7" runat="server" class="control-label" Text="Profit On Cost :"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblProfitOnCost" runat="server" class="control-label"></asp:Label>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="col-md-2 label-align">
                            <asp:Label ID="Label8" runat="server" class="control-label" Text="% of Profit :"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblProfitPercentage" runat="server" class="control-label"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-sm" value="Save" onclick="SaveCostAnalysis()" />
                    <input type="button" class="btn btn-primary btn-sm" value="Clear" onclick="ClearForm()" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
