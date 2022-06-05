<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="RateChartInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.RateChartInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var SearchItem = null;
        var SearchService = null;
        var ContactPeriod = new Array();
        var PriceMatrix = null;
        var DiscountTable;
        var FinalQuotationMasterTable;
        var SearchRateChart;
        var FinalQuotationMaster = null;
        var RateChartDetail = new Array();
        var RateChartItemDetail = new Array();
        var RateChartServiceDetail = new Array();
        var DeletedRateChartDetail = [];
        var DeletedRateChartDiscountDetail = [];
        var TempRateChartDetail = null;
        var RateChartDiscountDetails = new Array();
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        $(document).ready(function () {
            IsCanSave = '<%=isSavePermission%>' == 'True';
            IsCanEdit = '<%=isUpdatePermission%>' == 'True';
            IsCanDelete = '<%=isDeletePermission%>' == 'True';
            if (IsCanSave)
                $("#ContentPlaceHolder1_btnCreateNew").show();
            else
                $("#ContentPlaceHolder1_btnCreateNew").hide();
            DiscountTable = $("#tblDiscountItems").DataTable({
                data: [],
                columns: [
                    { title: "<input id='chkAllItem' type='checkbox'></input>", "data": null, sWidth: '5%' },
                    { title: "", "data": "TypeName", sWidth: '40%' },
                    { title: "Unit Price", "data": "UnitPrice", sWidth: '10%' },
                    { title: "Discount Type", "data": "DiscountType", sWidth: '15%' },
                    { title: "DiscountAmount (Local)", "data": "DiscountAmount", sWidth: '15%' },
                    { title: "Discount Amount (USD)", "data": "DiscountAmountUSD", sWidth: '15%' },
                    { title: "Offerred Price", "data": "OfferredPrice", sWidth: '15%' },
                    { title: "", "data": "Type", visible: false },
                    { title: "", "data": "Id", visible: false }
                ],
                columnDefs: [
                    {
                        "targets": 0,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            if (data.Id > 0)
                                return '<input onclick="CheckForPackage(this)" type="checkbox" checked="checked" />';
                            else
                                return '<input onclick="CheckForPackage(this)" type="checkbox" />';
                        }
                    },
                    {
                        "targets": 3,
                        "render": function (data, type, full, meta) {
                            return '<select class="form-control" onchange="ShowHideDiscountAmount(this)">' +
                                '<option value="0">---Please Select---</option>' +
                                '<option value="Fixed">Fixed</option>' +
                                '<option value="Percentage">Percentage</option>' +
                                '</select > ';
                        }
                    },
                    {
                        "targets": 4,
                        "render": function (data, type, full, meta) {
                            return '<input type="text" class="form-control quantitydecimal" value="' + data + '" />';
                        }
                    }
                    ,
                    {
                        "targets": [5],
                        "render": function (data, type, full, meta) {
                            return '<input type="text" class="form-control quantitydecimal" value="' + data + '" />';
                        }
                    }
                    ,
                    {
                        "targets": [6],
                        "render": function (data, type, full, meta) {
                            return '<input type="text" class="form-control quantitydecimal" value="' + data + '" />';
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
                    if (aData.DiscountType != null)
                        $('td:eq(2) select', nRow).val(aData.DiscountType).trigger('change');
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },
            });
            FinalQuotationMasterTable = $("#tblRateChartDetails").DataTable({
                data: [],
                columns: [
                    { title: "Service Type", "data": "ServiceTypeColumn", sWidth: '35%' },
                    { title: "", "data": "RateChartDetailId", visible: false },
                    { title: "", "data": "ServiceType", visible: false },
                    { title: "", "data": "OutLetId", visible: false },
                    { title: "Action", "data": null, sWidth: '10%' },
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    row += "&nbsp;<a href=\"javascript:void(0);\" onclick=\"EditRateChartDetails(this," + aData.RateChartDetailId + "," + aData.OutLetId + ",'" + aData.ServiceType + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    row += "&nbsp;&nbsp;<a href=\"javascript:void(0);\" onclick= \"DeleteRateChartDetails(this," + aData.RateChartDetailId + "," + aData.OutLetId + ",'" + aData.ServiceType + "');\"> <img alt='Delete' src='../Images/delete.png' /></a>";

                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },
            });
            SearchRateChart = $("#tblRateChart").DataTable({
                data: [],
                columns: [
                    { title: "Promotion / Package Name", "data": "PromotionName", sWidth: '30%' },
                    { title: "Company Name", "data": "CompanyName", sWidth: '30%' },
                    { title: "Effect From", "data": "EffectFrom", sWidth: '15%' },
                    { title: "Effect To", "data": "EffectTo", sWidth: '15%' },
                    { title: "Action", "data": null, sWidth: '10%' },
                ],
                columnDefs: [
                    {
                        "targets": 2,
                        "render": function (data, type, full, meta) {
                            return CommonHelper.DateFromStringToDisplay(data, innBoarDateFormat);
                        }
                    },
                    {
                        "targets": [3],
                        "render": function (data, type, full, meta) {
                            return CommonHelper.DateFromStringToDisplay(data, innBoarDateFormat);
                        }
                    }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    if (IsCanEdit)
                        row += "&nbsp;<a href=\"javascript:void(0);\" onclick=\"EditRateChart(" + aData.Id + ");\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    if (IsCanDelete)
                        row += "&nbsp;&nbsp;<a href=\"javascript:void(0);\" onclick= \"DeleteRateChart(" + aData.Id + ");\"> <img alt='Delete' src='../Images/delete.png' /></a>";

                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },
            });
            LoadRateChartFor();

            CommonHelper.ApplyDecimalValidation();
            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                minDate: DayOpenDate,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                minDate: DayOpenDate,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_ddlSearchCompany").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
                placeholder: "--- All ---"
            });
            $("#ContentPlaceHolder1_ddlCompany").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
                placeholder: "--- Please Select ---",
                dropdownParent: $("#CreateNewDialog")
            })
            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                },
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#txtItemCategory').blur(function () {

                if (!$(this).val())
                    $("#ContentPlaceHolder1_hfItemCategoryId").val('0');
            });

            $("#txtItemName").autocomplete({
                source: function (request, response) {
                    var categoryId = $("#ContentPlaceHolder1_hfItemCategoryId").val();
                    var isCustomerItem = 1; //$("#ContentPlaceHolder1_ddlIsCustomerItem").val();
                    var itemtype = ""; //$("#ContentPlaceHolder1_ddlItemType").val();

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

                    //if ($.trim($("#ContentPlaceHolder1_hfEditedItemId").val()) != "") {

                    //    if ($.trim($("#ContentPlaceHolder1_hfEditedItemId").val()) == ui.item.value) {
                    //        toastr.info("Same Item Cannot be Added as Recipe.");
                    //        return;
                    //    }
                    //}

                    SearchItem = ui.item;
                    $(this).val(ui.item.label);
                    $("#<%=hfItemId.ClientID %>").val(ui.item.value);
                    PageMethods.LoadRelatedStockBy(ui.item.StockById, OnLoadStockBySucceeded, OnLoadStockByFailed);
                }
            }).autocomplete("option", "appendTo", "#CreateNewDialog");

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
                                    UnitHead: m.UnitHead
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
                    LoadPackages();
                }
            });
            CommonHelper.AutoSearchClientDataSource("txtItemCategory", "ContentPlaceHolder1_ddlCategory", "ContentPlaceHolder1_hfItemCategoryId");

            $("[id=chkAllItem]").on("change", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#tblDiscountItems tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#tblDiscountItems tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
                }
            });

            $("#ContentPlaceHolder1_ddlPackageName").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
                placeholder: "--- Please Select ---"
            });

            $("#ContentPlaceHolder1_ddlPackageName").change(function () {
                var id = $("#ContentPlaceHolder1_ddlPackageName").val();
                if (id != "0")
                    GetServicePriceMatrix(id);
            });

            GridPaging(1, 1);
        });

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
        }
        function LoadPackages() {
            var itemId = $("#ContentPlaceHolder1_hfItemServiceId").val();
            PageMethods.GetPackageByItemBy(itemId, OnLoadPackageSucceeded, OnLoadPackageFailed);
            return false;
        }

        function OnLoadPackageSucceeded(result) {
            PopulateControlWithValueNTextField(result, $("#ContentPlaceHolder1_ddlPackageName"), "--- Please Select ---", "PackageName", "ServicePriceMatrixId");
            return false;
        }
        function OnLoadPackageFailed() { }

        function EditQuantity(tr) {

            tr = $(tr).parent().parent();

            var itemId = parseInt($(tr).find("td:eq(1)").text());
            var item = _.findWhere(RateChartItemDetail, { ItemId: itemId, ServiceType: "Item" });

            var quantity = parseInt($(tr).find("td:eq(4)").find("input").val());
            var unitPrice = parseInt($(tr).find("td:eq(5)").find("input").val());
            $(tr).find("td:eq(6)").text(parseFloat(quantity) * parseFloat(unitPrice));

            if (item != null) {
                item.Quantity = parseFloat(quantity);
                item.UnitPrice = parseFloat(unitPrice);
                item.TotalPrice = item.UnitPrice * item.Quantity;
            }

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
            $.each(RateChartItemDetail, function (index, itm) {
                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalCost = totalCost + itm.TotalPrice;
            });

            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text(totalCost);
        }

        function AddItem() {

            if (SearchItem == null) {
                return false;
            }

            var existingItem = _.findWhere(RateChartItemDetail, { ItemId: SearchItem.ItemId, ServiceType: "Item" });
            if (existingItem != null) {
                toastr.warning("Same Item Already Added.");
                return false;
            }

            var tr = "", totalRow = 0, deleteLink = "", itemQuantity = "0", itemCost = 0.00, id = 0, itemName = "";
            itemQuantity = $("#ContentPlaceHolder1_txtItemUnit").val();
            itemCost = parseFloat(itemQuantity) * SearchItem.UnitPriceLocal;
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
            tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + SearchItem.Name + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + SearchItem.UnitHead + "</td>";

            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" +
                " <input type=\"text\" id='itm" + SearchItem.ItemId + "' value='" + itemQuantity + "' class=\"form-control\" onblur='EditQuantity(this)' />" +
                "</td>";

            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" +
                " <input type=\"text\" " + "' value='" + SearchItem.UnitPriceLocal + "' class=\"form-control\" onblur='EditQuantity(this)' />" +
                "</td>";
            tr += "<td align='left' style='width: 15%;'>" + itemCost + "</td>";
            tr += "<td align='left' style=\"display:none;\">" + SearchItem.StockById + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#QuotationItemInformation tbody").append(tr);

            RateChartItemDetail.push({

                Id: 0,
                RateChartMasterId: 0,
                ServiceType: 'Item',
                CategoryId: SearchItem.CategoryId,
                ItemName: SearchItem.Name,
                ItemId: SearchItem.ItemId,
                StockBy: SearchItem.StockById,
                UnitHead: SearchItem.UnitHead,
                Quantity: parseFloat(itemQuantity),
                UnitPrice: SearchItem.UnitPriceLocal,
                TotalPrice: itemCost,
            });

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;

            $.each(RateChartItemDetail, function (index, itm) {
                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalCost = totalCost + itm.TotalPrice;
            });

            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text(totalCost);

            SearchItem = null;
            $("#txtItemName").val('');
            $("#ContentPlaceHolder1_ddlItemStockBy").val("0");
            $("#ContentPlaceHolder1_txtItemUnit").val("");
            $("#txtItemName").focus();
        }

        function AddServiceItem() {

            if (SearchService == null) {
                return false;
            }

            var tr = "", totalRow = 0, deleteLink = "", itemQuantity = "0", itemCost = 0.00, quoTationId = 0;
            var packageId = 0, bandwidthId = 0, serviceTypeId = 0, contractPeriodId = 0, contractPeriodValue = 0, serviceItemName = "";

            var serviceType = "", packageName = "", downLink = "", upLink = "", pricePerMonth = 0.00, priceTotalByContactPeriod = 0.00;

            serviceTypeId = $("#ContentPlaceHolder1_ddlBandwidthServiceType").val();
            contractPeriodId = $("#ContentPlaceHolder1_ddlContractPeriod").val();
            itemQuantity = $("#ContentPlaceHolder1_txtServiceQuantity").val();

            serviceTypeId = $("#ContentPlaceHolder1_ddlBandwidthServiceType").val();
            packageId = $("#ContentPlaceHolder1_ddlPackageName").val();

            pricePerMonth = $("#ContentPlaceHolder1_txtUnitPrice").val();
            serviceItemName = $("#txtServiceItem").val();

            if (contractPeriodId == "0") { toastr.warning("Please Select Contract Period."); return false; }
            else if (SearchService == null || $.trim(serviceItemName) == "") { toastr.warning("Please Select Service."); return false; }
            else if ($.trim(itemQuantity) == "0" || $.trim(itemQuantity) == "") { toastr.warning("Please Give Service Unit."); return false; }
            //else if ($.trim(serviceTypeId) == "0" || $.trim(serviceTypeId) == "") { toastr.warning("Please Select Service Type."); return false; }
            else if ($.trim(pricePerMonth) == "0" || $.trim(pricePerMonth) == "") { toastr.warning("Please Give Unit Price."); return false; }

            else if ($.trim(packageId) == "0" || $.trim(packageId) == "") { toastr.warning("Please Select package."); return false; }

            var QuotationServiceDetails = RateChartServiceDetail;
            var row = 0, rowLength = QuotationServiceDetails.length, index = -1;
            for (row = 0; row < rowLength; row++) {
                if (QuotationServiceDetails[row].ServicePackageId == parseInt(packageId, 10) &&
                    QuotationServiceDetails[row].ServiceTypeId == parseInt(serviceTypeId, 10) &&
                    QuotationServiceDetails[row].ItemId == parseInt(SearchService.ItemId, 10)) {

                    index = row;
                    break;
                }
            }

            if (index >= 0) {
                toastr.warning("Same Service Already Added.");
                return false;
            }

            if (PriceMatrix != null) {
                downLink = PriceMatrix.DownlinkFrequency != null ? PriceMatrix.DownlinkFrequency : 0;
                upLink = PriceMatrix.UplinkFrequency != null ? PriceMatrix.UplinkFrequency : 0;
            }

            if (ContactPeriod.length != 0) {
                var cp = _.findWhere(ContactPeriod, { ContractPeriodId: parseInt(contractPeriodId) });
                contractPeriodValue = cp.ContractPeriodValue;
            }

            serviceType = $("#ContentPlaceHolder1_ddlBandwidthServiceType option:selected").text();
            if ($.trim(serviceTypeId) == "0" || $.trim(serviceTypeId) == "")
                serviceType = "";
            packageName = $("#ContentPlaceHolder1_ddlPackageName option:selected").text();

            priceTotalByContactPeriod = pricePerMonth * contractPeriodValue * itemQuantity;

            totalRow = $("#QuotationServicenformation tbody tr").length;
            deleteLink = "<a href=\"javascript:void()\" onclick= \"DeleteServiceItem(this," + SearchService.ItemId + "," + packageId + "," + serviceTypeId + ")\" ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            $("#QuotationServicenformation thead tr:eq(0)").find("th:eq(6)").text("Price/ " + contractPeriodValue + " Month");

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

            tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + serviceType + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + SearchService.Name + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + packageName + "</td>";
            tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + downLink + "</td>";

            tr += "<td align='left' style=\"width:10%; text-align:Left;\">" +
                " <input type=\"text\" disabled='disabled' id='itm" + SearchService.ItemId + "' value='" + upLink + "' class=\"form-control\" />" +
                "</td>";

            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + pricePerMonth + "</td>";
            tr += "<td align='left' style='width: 15%;'>" + priceTotalByContactPeriod + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#QuotationServicenformation tbody").append(tr);

            RateChartServiceDetail.push({

                Id: 0,
                RateChartMasterId: 0,
                ServiceType: 'Service',
                CategoryId: SearchService.CategoryId,
                ServicePackageId: packageId,
                ItemId: SearchService.ItemId,
                ItemName: SearchService.Name,
                StockBy: SearchService.StockById,
                ServiceTypeId: serviceTypeId,
                Quantity: parseFloat(itemQuantity),
                UnitPrice: parseFloat(pricePerMonth),
                TotalPrice: parseFloat(priceTotalByContactPeriod)
            });

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;

            $.each(QuotationServiceDetails, function (index, itm) {
                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalCost = totalCost + itm.TotalPrice;
            });

            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text(totalUnitCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text(totalCost);

            SearchService = null;
            $("#txtServiceItem").val('');
            $("#ContentPlaceHolder1_ddlServiceStockBy").val("0");
            $("#ContentPlaceHolder1_ddlPackageName").val("0").trigger('change');

            $("#ContentPlaceHolder1_ddlBandwidthServiceType").val("0");
            $("#ContentPlaceHolder1_txtServiceQuantity").val("");
            $("#ContentPlaceHolder1_txtUnitPrice").val("");
            $("#txtServiceItem").focus();
        }

        function DeleteItem(tr) {
            tr = $(tr).parent().parent();

            var itemId = parseInt($(tr).find("td:eq(1)").text());
            var item = _.findWhere(RateChartItemDetail, { ItemId: parseInt(itemId, 10), ServiceType: "Item" });

            if (item != null) {

                if (item.Id != 0) {
                    DeletedRateChartDetail.push(item.Id);
                }
                var index = RateChartItemDetail.indexOf(item);
                RateChartItemDetail.splice(index, 1);

                var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
                $.each(RateChartItemDetail, function (index, itm) {
                    totalQuantity = totalQuantity + itm.Quantity;
                    totalUnitCost = totalUnitCost + itm.UnitPrice;
                    totalCost = totalCost + itm.TotalPrice;
                });

                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text(totalCost);

                $(tr).remove();
            }
        }

        function DeleteServiceItem(tr, itemId, servicePackageId, serviceTypeId) {
            tr = $(tr).parent().parent();

            var itemId = parseInt($(tr).find("td:eq(1)").text());
            var QuotationServiceDetails = RateChartServiceDetail;

            var item = _.findWhere(QuotationServiceDetails, { ItemId: parseInt(itemId, 10), ServiceType: "Service" });
            var row = 0, rowLength = QuotationServiceDetails.length, index = -1;

            for (row = 0; row < rowLength; row++) {
                if (QuotationServiceDetails[row].ServicePackageId == parseInt(servicePackageId, 10) &&
                    QuotationServiceDetails[row].ServiceTypeId == parseInt(serviceTypeId, 10) &&
                    QuotationServiceDetails[row].ItemId == parseInt(itemId, 10)) {

                    index = row;
                    if (QuotationServiceDetails[row].Id != 0) {
                        DeletedRateChartDetail.push(QuotationServiceDetails[row].Id);
                    }

                    break;
                }
            }

            if (index >= 0) {

                QuotationServiceDetails.splice(index, 1);

                var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
                $.each(QuotationServiceDetails, function (index, itm) {
                    totalQuantity = totalQuantity + itm.Quantity;
                    totalUnitCost = totalUnitCost + itm.UnitPrice;
                    totalCost = totalCost + itm.TotalPrice;
                });

                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text(totalUnitCost);
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text(totalCost);

                $(tr).remove();
            }

            return false;
        }

        function GetServicePriceMatrix(servicePriceMatrixId) {
            PageMethods.GetServicePriceMatrix(servicePriceMatrixId, OnLoadServicePriceMatrixSucceeded, OnLoadServicePriceMatrixFailed);
            return false;
        }

        function OnLoadServicePriceMatrixSucceeded(result) {
            PriceMatrix = result;
            $("#ContentPlaceHolder1_txtUnitPrice").val(result.UnitPrice);
            return false;
        }

        function OnLoadServicePriceMatrixFailed() { }

        function CreateNew() {
            ClearForm();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '95%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Create Rate Chart",
                show: 'slide'
            });

            return false;
        }

        function LoadService(control) {
            var isPackage = $("#ContentPlaceHolder1_ddlRateChartFor").val() == "Package";
            if (isPackage) {
                $("#ContentPlaceHolder1_ddlDiscountTo").val('0');
                $("#dvDiscountTo").hide();
            }
            else {
                $("#dvDiscountTo").show();
            }
            var serviceType = $(control).val();
            var isDetails = $("#ContentPlaceHolder1_ddlDiscountTo").val() == "0";
            if (serviceType == "0") {
                ClearDiscount();
                return true;
            }
            if (isDetails) {
                $("#dvDiscount").show();
                $("#dvDiscountAmount").hide();
            }
            else {
                $("#dvDiscount").hide();
                $("#dvDiscountAmount").show();
            }
            $("#RestaurantDropDown").hide();
            $("#BanquetDropDown").hide();
            if (serviceType == "0")
                ClearDiscount();
            if (serviceType == "Restaurant") {
                $("#discountTableHeader").text("Restaurant");
                $("#dvItem").hide();
                $("#dvService").hide();
                $("#dvDiscountTo").show();
                if (isDetails) {
                    $("#RestaurantDropDown").show();
                    LoadTypeList();
                }
            }
            else if (serviceType == "GuestRoom") {
                $("#discountTableHeader").text("Guest Room");
                $("#dvItem").hide();
                $("#dvService").hide();
                if (!isPackage)
                    $("#dvDiscountTo").show();
                if (isDetails) {
                    LoadTypeList();
                }
            }
            else if (serviceType == "Banquet") {
                $("#discountTableHeader").text("Banquet");
                $("#dvItem").hide();
                $("#dvService").hide();
                $("#dvDiscountTo").show();
                if (isDetails) {
                    $("#BanquetDropDown").show();
                    LoadTypeList();
                }
            }
            else if (serviceType == "ServiceOutlet") {
                $("#discountTableHeader").text("Service Outlet");
                $("#dvItem").hide();
                $("#dvService").hide();
                if (!isPackage)
                    $("#dvDiscountTo").show();
                if (isDetails) {
                    LoadTypeList();
                }
            }
            else if (serviceType == "Item") {
                $("#discountTableHeader").text("Item Information");
                $("#dvDiscount").hide();
                $("#dvItem").show();
                $("#dvService").hide();
                $("#dvDiscountTo").hide();
                $("#dvDiscountAmount").hide();
            }
            else if (serviceType == "Service") {
                $("#discountTableHeader").text("Service Information");
                $("#dvDiscount").hide();
                $("#dvItem").hide();
                $("#dvDiscountTo").hide();
                $("#dvService").show();
                $("#dvDiscountAmount").hide();
            }

        }

        function LoadDiscountDiv() {
            LoadService($("#ContentPlaceHolder1_ddlServiceItemType"));
            LoadTypeList();
        }

        function LoadTypeList() {
            let serviceType = $("#ContentPlaceHolder1_ddlServiceItemType").val();
            let serviceTypeId = 0;
            if (serviceType == "Banquet") {
                serviceTypeId = $("#ContentPlaceHolder1_ddlBanquet").val();
            }
            else if (serviceType == "Restaurant") {
                serviceTypeId = $("#ContentPlaceHolder1_ddlRestaurant").val();
            }

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HMCommon/RateChartInformation.aspx/GetTypeList',
                data: JSON.stringify({ serviceType }),
                dataType: "json",
                async: false,
                success: function (result) {
                    if (result.d.length > 0) {
                        var isPackage = $("#ContentPlaceHolder1_ddlRateChartFor").val() == "Package";
                        DiscountTable.clear();

                        DiscountTable.rows.add(result.d);
                        if (!isPackage) {
                            DiscountTable.columns(2).visible(false);
                            DiscountTable.columns(6).visible(false);
                            DiscountTable.columns(3).visible(true);
                            DiscountTable.columns(4).visible(true);
                            DiscountTable.columns(5).visible(true);
                            DiscountTable.columns(0).header().to$().find('input').prop('disabled', false);
                        }
                        else {
                            DiscountTable.columns(2).visible(true);
                            DiscountTable.columns(6).visible(true);
                            DiscountTable.columns(3).visible(false);
                            DiscountTable.columns(4).visible(false);
                            DiscountTable.columns(5).visible(false);
                            DiscountTable.columns(0).header().to$().find('input').prop('disabled', true);
                            //DiscountTable.columns(0).to$().find('input').setAttribute('type', 'radio');
                        }

                        DiscountTable.draw();
                        CommonHelper.ApplyDecimalValidation();
                        LoadTableHeader(serviceType);
                    }
                },
                error: function (error) {
                    toastr.error(error.d.AlertMessage);
                }
            });
        }

        function LoadTableHeader(serviceType) {
            if (serviceType == "Restaurant") {
                DiscountTable.columns(1).header().to$().text("Classification");
            }
            else if (serviceType == "GuestRoom") {
                DiscountTable.columns(1).header().to$().text("Room Type");
            }
            else if (serviceType == "Banquet") {
                DiscountTable.columns(1).header().to$().text("Discount Head");
            }
            else if (serviceType == "ServiceOutlet") {
                DiscountTable.columns(1).header().to$().text("Service Name");
            }
        }

        function AddDiscount() {

            let ServiceType = $("#ContentPlaceHolder1_ddlServiceItemType").val();
            let RateChartMasterId = parseInt($("#ContentPlaceHolder1_hfRateChartMasterId").val());
            let IsDiscountForAll = $("#ContentPlaceHolder1_ddlDiscountTo").val() == "1";
            let OutLetId = 0;
            var ServiceTypeColumn = "";
            if (ServiceType == "0") {
                $("#ContentPlaceHolder1_ddlServiceItemType").focus();
                toastr.warning("Please Select Service Type.");
                return false;
            }
            if (ServiceType == "Restaurant") {
                let RestaurantName = "";
                if (IsDiscountForAll)
                    RestaurantName = "All";
                else {
                    OutLetId = parseInt($("#ContentPlaceHolder1_ddlRestaurant").val());
                    if (!OutLetId) {
                        $("#ContentPlaceHolder1_ddlRestaurant").focus();
                        toastr.warning("Select Restaurant Name.");
                        return false;
                    }
                    RestaurantName = $("#ContentPlaceHolder1_ddlRestaurant option:selected").text();
                }
                ServiceTypeColumn = "Restaurant: " + RestaurantName;
            }
            else if (ServiceType == "GuestRoom") {
                ServiceTypeColumn = "Guest Room"
            }
            else if (ServiceType == "Banquet") {
                let BanquetName = "";
                if (IsDiscountForAll)
                    BanquetName = "All";
                else {
                    OutLetId = parseInt($("#ContentPlaceHolder1_ddlBanquet").val());
                    if (!OutLetId) {
                        $("#ContentPlaceHolder1_ddlBanquet").focus();
                        toastr.warning("Select Banquet Name.");
                        return false;
                    }
                    BanquetName = $("#ContentPlaceHolder1_ddlBanquet option:selected").text();
                }
                ServiceTypeColumn = "Banquet: " + BanquetName;
            }
            else if (ServiceType == "ServiceOutlet") {
                ServiceTypeColumn = "Service Outlet";
            }
            else if (ServiceType == "Item") {

                $("#QuotationItemInformation tbody").html("");
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text("");
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text("");
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text("");

                ServiceTypeColumn = "Item Information";

            }
            else if (ServiceType == "Service") {

                $("#QuotationServicenformation tbody").html("");
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text("");
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text("");

                ServiceTypeColumn = "Service Information";
            }

            let RateChartDetailId = 0;
            if (TempRateChartDetail != null)
                RateChartDetailId = TempRateChartDetail.Id;

            let Id = 0, Type = "", TypeId = 0, DiscountType = "", DiscountAmount = 0.00, DiscountAmountUSD = 0.00, OfferredPrice = 0.00, TotalPrice = 0.00;
            if (ServiceType != "Item" && ServiceType != "Service") {
                if (!IsDiscountForAll) {
                    var invalid = false;
                    DiscountTable.rows().every(function (rowIdx, tableLoop, rowLoop) {
                        var data = this.data();
                        var isChecked = DiscountTable.cell(rowIdx, 0).nodes().to$().find('input').is(':checked');
                        if (isChecked) {
                            var isPackage = $("#ContentPlaceHolder1_ddlRateChartFor").val() == "Package";
                            Id = data.Id;
                            Type = data.Type;
                            TypeId = data.TypeId;
                            if (!isPackage)
                                DiscountType = DiscountTable.cell(rowIdx, 3).nodes().to$().find('select').val();
                            if (DiscountType == "0" && !isPackage) {
                                DiscountTable.cell(rowIdx, 3).nodes().to$().find('select').focus();
                                toastr.warning("Select Discount Type.");
                                invalid = true;
                                return false;
                            }
                            if (!isPackage)
                                DiscountAmount = parseFloat(DiscountTable.cell(rowIdx, 4).nodes().to$().find('input').val());

                            if (!isPackage && !DiscountAmount) {
                                DiscountTable.cell(rowIdx, 4).nodes().to$().find('input').focus();
                                toastr.warning("Enter Discount Amount.");
                                invalid = true;
                                return false;
                            }
                            if (!isPackage && DiscountType == "Fixed")
                                DiscountAmountUSD = parseFloat(DiscountTable.cell(rowIdx, 5).nodes().to$().find('input').val());
                            if (isPackage) {
                                OfferredPrice = parseFloat(DiscountTable.cell(rowIdx, 6).nodes().to$().find('input').val());

                                if (!OfferredPrice) {
                                    toastr.warning("Enter Offered Price.");
                                    DiscountTable.cell(rowIdx, 6).nodes().to$().find('input').focus()
                                    invalid = true;
                                    return false;
                                }
                                TotalPrice = TotalPrice + OfferredPrice;
                            }
                            RateChartDiscountDetails.push({
                                Id, RateChartDetailId, OutLetId, Type, TypeId, DiscountType, DiscountAmount, DiscountAmountUSD, OfferredPrice
                            });
                        }
                        else {
                            if (data.Id > 0)
                                DeletedRateChartDiscountDetail.push(data.Id);
                        }
                        Id = 0, Type = "", TypeId = 0, DiscountType = "", DiscountAmount = 0.00, DiscountAmountUSD = 0.00;
                    });
                    if (invalid)
                        return false;
                    if (RateChartDiscountDetails.length == 0) {
                        toastr.warning("No Item is Selected");
                        return false;
                    }
                }
                else {
                    DiscountType = $("#ContentPlaceHolder1_ddlAllDiscountType").val();
                    if (!$("#ContentPlaceHolder1_txtAmount").val()) {
                        $("#ContentPlaceHolder1_txtAmount").focus();
                        toastr.warning("Enter Discount Amount.");
                        return false;
                    }
                    DiscountAmount = parseFloat($("#ContentPlaceHolder1_txtAmount").val());

                    if (DiscountType == "Fixed" && $("#ContentPlaceHolder1_txtAmountUSD").val())
                        DiscountAmountUSD = parseFloat($("#ContentPlaceHolder1_txtAmountUSD").val());
                }
            }
            var rateChartDetail = new Array();
            if (ServiceType != "Item" && ServiceType != "Service") {
                rateChartDetail.push({
                    RateChartDetailId,
                    ServiceType,
                    RateChartMasterId,
                    IsDiscountForAll,
                    DiscountType,
                    DiscountAmount,
                    DiscountAmountUSD,
                    TotalPrice,
                    RateChartDiscountDetails
                });
            }
            else if (ServiceType == "Item")
                rateChartDetail = RateChartItemDetail;
            else if (ServiceType == "Service")
                rateChartDetail = RateChartServiceDetail;
            var isExist = false, inValid = false;

            Totalprice = 0.00;

            $.each(RateChartDetail, function (index, i) {
                //let previousQotationDetailRow = _.findWhere(QuotationDetail, { ItemType: ServiceType });
                if (i.ServiceType == ServiceType) {
                    isExist = true;
                    if (TempRateChartDetail == null) {
                        if (IsDiscountForAll || i.IsDiscountForAll || (OutLetId > 0 && i.RateChartDiscountDetails.filter(r => r.OutLetId == OutLetId).length > 0)) {
                            toastr.warning("Same Type Service is Already Added. Please Edit.");
                            inValid = true;
                            return false;
                        }
                    }
                    if (ServiceType != "Item" && ServiceType != "Service") {
                        i.ServiceType = ServiceType;
                        i.RateChartMasterId = RateChartMasterId;
                        i.IsDiscountForAll = IsDiscountForAll;
                        i.DiscountType = DiscountType;
                        i.DiscountAmount = DiscountAmount;
                        i.DiscountAmountUSD = DiscountAmountUSD;
                        rateChartDetail[0].RateChartDiscountDetails.map(r => { r.RateChartDetailId = i.RateChartDetailId; return r; });

                        i.RateChartDiscountDetails = i.RateChartDiscountDetails.filter(r => r.OutLetId != OutLetId);
                        i.RateChartDiscountDetails.push.apply(i.RateChartDiscountDetails, rateChartDetail[0].RateChartDiscountDetails);
                        
                        _(i.RateChartDiscountDetails).each((value, key) => {
                            TotalPrice = TotalPrice + value.OfferredPrice;
                        });
                        i.Totalprice = Totalprice;
                    }
                    return i;
                }
            });

            RateChartDiscountDetails = [];
            RateChartItemDetail = [];
            RateChartServiceDetail = [];

            if (inValid)
                return false;
            if ((ServiceType == "Item" || ServiceType == "Service") && !inValid) {
                RateChartDetail = RateChartDetail.filter(i => i.ServiceType != ServiceType);
                RateChartDetail.push.apply(RateChartDetail, rateChartDetail);
            }
            else if (!isExist)
                RateChartDetail.push.apply(RateChartDetail, rateChartDetail);

            var QuotationMasterRow = {
                ServiceType,
                ServiceTypeColumn,
                OutLetId,
                RateChartDetailId
            };
            FinalQuotationMasterTable.row.add(QuotationMasterRow);
            var sortedData = _(FinalQuotationMasterTable.data()).sortBy('ServiceType');
            FinalQuotationMasterTable.clear();
            FinalQuotationMasterTable.rows.add(sortedData);
            FinalQuotationMasterTable.columns.adjust().draw();
            ClearDiscount();
            return false;
        }

        function ClearDiscount() {
            $("#ContentPlaceHolder1_ddlRestaurant").val("0").prop('disabled', false);
            $("#ContentPlaceHolder1_ddlBanquet").val("0").prop('disabled', false);
            $("#ContentPlaceHolder1_ddlDiscountTo").val("1");
            $("#ContentPlaceHolder1_ddlServiceItemType").val("0");
            $("#ContentPlaceHolder1_txtAmount").val("");
            $("#ContentPlaceHolder1_txtAmountUSD").val("");
            $("#dvItem").hide();
            $("#dvService").hide();
            TempRateChartDetail = null;
            DiscountTable.clear().draw();
        }

        function ShowHideDiscountAmount(control) {
            let value = $(control).val();

            if (value == "Fixed")
                $(control).parent().parent().find("td:eq(4) input").show();
            else
                $(control).parent().parent().find("td:eq(4) input").hide();
        }

        function EditRateChartDetails(column, RateChartDetailId, OutLetId, ServiceType) {
            if (!confirm("Do you want to edit?"))
                return true;
            var row = $(column).parents('tr');

            FinalQuotationMasterTable.row(row).remove().draw(false);
            var QuotationDetailRow = null;
            if (RateChartDetailId > 0)
                QuotationDetailRow = _.findWhere(RateChartDetail, { Id: RateChartDetailId });
            else
                QuotationDetailRow = _.findWhere(RateChartDetail, { ServiceType: ServiceType });
            TempRateChartDetail = QuotationDetailRow;

            $("#ContentPlaceHolder1_ddlServiceItemType").val(ServiceType).trigger('change');
            if (ServiceType == "Item") {
                $("#QuotationItemInformation tbody").html("");
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text("");
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text("");
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text("");
                RateChartItemDetail = RateChartDetail.filter(i => i.ServiceType == "Item");
                LoadItemTable();
            }
            else if (ServiceType == "Service") {
                $("#QuotationServicenformation tbody").html("");
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text("");
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text("");
                RateChartServiceDetail = RateChartDetail.filter(i => i.ServiceType == "Service");
                LoadServiceTable();
            }
            else {

                $("#ContentPlaceHolder1_ddlDiscountTo").val((QuotationDetailRow.IsDiscountForAll ? "1" : "0")).trigger('change');
                if (!QuotationDetailRow.IsDiscountForAll) {
                    if (ServiceType == "Restaurant")
                        $("#ContentPlaceHolder1_ddlRestaurant").val(OutLetId).prop('disabled', true);
                    else if (ServiceType == "Banquet")
                        $("#ContentPlaceHolder1_ddlBanquet").val(OutLetId).prop('disabled', true);
                }
                $("#ContentPlaceHolder1_ddlAllDiscountType").val(QuotationDetailRow.DiscountType).trigger('change');
                $("#ContentPlaceHolder1_txtAmount").val(QuotationDetailRow.DiscountAmount);
                $("#ContentPlaceHolder1_txtAmountUSD").val(QuotationDetailRow.DiscountAmountUSD);

                let QuotationDiscountDetailsList = QuotationDetailRow.RateChartDiscountDetails.filter(i => i.OutLetId == OutLetId);
                if (!QuotationDetailRow.IsDiscountForAll && QuotationDiscountDetailsList.length > 0) {
                    let DiscountData = DiscountTable.data();
                    QuotationDiscountDetailsList.map(function (value, rowIndex) {
                        let rowData = _.findWhere(DiscountData, { Type: value.Type, TypeId: value.TypeId });
                        let index = 0;
                        $.each(DiscountData, function (idx, data, node) {
                            if (data.Type == value.Type && data.TypeId == value.TypeId) {
                                index = idx;
                                return false;
                            }
                        });
                        if (rowData != null) {
                            rowData.Id = value.Id;
                            rowData.DiscountType = value.DiscountType;
                            rowData.DiscountAmount = value.DiscountAmount;
                            rowData.DiscountAmountUSD = value.DiscountAmountUSD;
                            rowData.OfferredPrice = value.OfferredPrice;
                        }
                        DiscountTable.row(index).data(rowData).draw();
                        DiscountTable.cell(index, 0).nodes().to$().find('input').prop('checked', true);
                    })
                }
            }
            return true;
        }

        function DeleteRateChartDetails(column, Id, OutLetId, ServiceType) {
            if (!confirm("Want to Delete?"))
                return true;
            var row = $(column).parents('tr');
            FinalQuotationMasterTable.row(row).remove().draw(false);
            if (ServiceType == "Item") {
                DeletedRateChartDetail.push.apply(DeletedRateChartDetail, RateChartDetail.filter(i => i.ServiceType == "Item").map(i => { return i.Id }));
            }
            else if (ServiceType == "Service")
                DeletedRateChartDetail.push.apply(DeletedRateChartDetail, RateChartDetail.filter(i => i.ServiceType == "Service").map(i => { return i.Id }));
            else if (Id > 0) {
                if (OutLetId == 0)
                    DeletedRateChartDetail.push(Id);
                else {
                    var rateChartDetail = RateChartDetail.filter(i => i.ServiceType == ServiceType);
                    var deletedDiscountDetailId = rateChartDetail[0].RateChartDiscountDetails.filter(i => i.Id > 0 && i.OutLetId == OutLetId).map(i => { return i.Id });
                    DeletedRateChartDiscountDetail.push.apply(DeletedRateChartDiscountDetail, deletedDiscountDetailId);
                }
            }
            else
                RateChartDetail = RateChartDetail.filter(i => i.ServiceType != ServiceType);
        }

        function LoadItemTable() {

            var tr = "", deleteLink = "<a href=\"javascript:void()\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            var isCopy = $.trim(CommonHelper.GetParameterByName("isCopy"));
            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
            $.each(RateChartItemDetail, function (index, itm) {

                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"display:none;\">" + (isCopy != "1" ? itm.Id : "0") + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ItemId + "</td>";
                tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + itm.ItemName + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.UnitHead + "</td>";

                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" +
                    " <input type=\"text\"  id='itm" + itm.ItemId + "' value='" + itm.Quantity + "' class=\"form-control\" onblur='EditQuantity(this)' />" +
                    "</td>";

                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" +
                    " <input type=\"text\" " + "' value='" + itm.UnitPrice + "' class=\"form-control\" onblur='EditQuantity(this)' />" +
                    "</td>";
                tr += "<td align='left' style='width: 15%;'>" + itm.TotalPrice + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.StockBy + "</td>";
                tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

                tr += "</tr>";

                $("#QuotationItemInformation tbody").append(tr);
                tr = "";

                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalCost = totalCost + itm.TotalPrice;
            });

            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text(totalCost);

        }

        function LoadServiceTable() {
            var QuotationServiceDetails = RateChartServiceDetail;

            var contractPeriodValue = 0.00;
            contractPeriodValue = QuotationServiceDetails[0].TotalPrice / QuotationServiceDetails[0].UnitPrice / QuotationServiceDetails[0].Quantity;

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
            var tr = "";
            $("#QuotationServicenformation thead tr:eq(0)").find("th:eq(6)").text("Price/ " + contractPeriodValue + " Month");

            var serviceType = "", packageName = "", bandWidthName = "", downLink = "", upLink = "";
            $.each(QuotationServiceDetails, function (index, itm) {

                serviceType = $("#ContentPlaceHolder1_ddlBandwidthServiceType option[value=" + itm.ServiceTypeId + "]").text();
                packageName = $("#ContentPlaceHolder1_ddlPackageName option[value=" + itm.ServicePackageId + "]").text();

                downLink = itm.Downlink;
                upLink = itm.Uplink;

                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                deleteLink = "<a href=\"javascript:void()\" onclick= \"DeleteServiceItem(this," + itm.ItemId + "," + itm.ServicePackageId + "," + itm.ServiceTypeId + ")\" ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

                tr += "<td align='left' style=\"display:none;\">" + itm.Id + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ItemId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.StockById + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.Quantity + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServicePackageId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServiceBandWidthId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServiceTypeId + "</td>";

                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + (serviceType || itm.ServiceType) + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.ItemName + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + (packageName || itm.PackageName) + "</td>";
                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + (downLink) + "</td>";

                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" +
                    " <input type=\"text\" disabled='disabled' id='itm" + itm.ItemId + "' value='" + upLink + "' class=\"form-control\" />" +
                    "</td>";

                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.UnitPrice + "</td>";
                tr += "<td align='left' style='width: 15%;'>" + itm.TotalPrice + "</td>";
                tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

                tr += "</tr>";

                $("#QuotationServicenformation tbody").append(tr);

                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalCost = totalCost + itm.TotalPrice;
                tr = "";
            });

            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text(totalUnitCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text(totalCost);
        }

        function LoadFinalQuotationMasterTable() {

            var QuotationMasterRow = new Array();
            var groupedDetailList = _(RateChartDetail).groupBy('ServiceType');
            groupedDetailList = _(groupedDetailList).map(function (detailData, key) {
                return {
                    ServiceType: detailData[0].ServiceType,
                    Id: detailData[0].Id,
                    IsDiscountForAll: detailData[0].IsDiscountForAll,
                    RateChartDiscountDetails: detailData[0].RateChartDiscountDetails
                }
            });
            $.each(groupedDetailList, function (index, value) {
                var ServiceTypeColumn = "";
                var ServiceType = value.ServiceType;
                if (ServiceType == "Restaurant") {
                    let RestaurantName = "";
                    if (value.IsDiscountForAll)
                        RestaurantName = "All";
                    ServiceTypeColumn = "Restaurant: " + RestaurantName;
                }
                else if (ServiceType == "GuestRoom") {
                    ServiceTypeColumn = "Guest Room"
                }
                else if (ServiceType == "Banquet") {
                    let BanquetName = "";
                    if (value.IsDiscountForAll)
                        BanquetName = "All";
                    ServiceTypeColumn = "Banquet: " + BanquetName;
                }
                else if (ServiceType == "ServiceOutlet") {
                    ServiceTypeColumn = "Service Outlet";
                }
                else if (ServiceType == "Item") {

                    ServiceTypeColumn = "Item Information";
                }
                else if (ServiceType == "Service") {

                    ServiceTypeColumn = "Service Information";
                }
                if ((ServiceType == "Restaurant" || ServiceType == "Banquet") && !value.IsDiscountForAll) {

                    var groupedList = _(value.RateChartDiscountDetails).groupBy('OutLetId');
                    groupedList = _(groupedList).map(function (value, key) {
                        return {
                            OutLetId: value[0].OutLetId,
                            OutLetName: value[0].OutLetName
                        }
                    });
                    $.each(groupedList, function (arrayIndex, data) {
                        QuotationMasterRow.push({
                            ServiceType: value.ServiceType,
                            ServiceTypeColumn: (ServiceTypeColumn + data.OutLetName),
                            OutLetId: data.OutLetId,
                            RateChartDetailId: value.Id
                        });
                    });

                }
                else {
                    QuotationMasterRow.push({
                        ServiceType: value.ServiceType,
                        ServiceTypeColumn,
                        OutLetId: 0,
                        RateChartDetailId: value.Id
                    });
                }

            });

            //QuotationDetail = QuotationDetail.filter(i => i.ItemType != "Item" && i.ItemType != "Service");

            FinalQuotationMasterTable.clear();
            FinalQuotationMasterTable.rows.add(QuotationMasterRow);
            FinalQuotationMasterTable.draw();
        }

        function ChangeDiscountType(control) {
            if ($(control).val() == "Fixed")
                $("#ContentPlaceHolder1_txtAmountUSD").show();
            else
                $("#ContentPlaceHolder1_txtAmountUSD").hide();

        }

        function ValidationNPreprocess() {
            //CommonHelper.SpinnerOpen();

            if ($("#ContentPlaceHolder1_txtFromDate").val() == "") {
                $("#ContentPlaceHolder1_txtProposalDate").focus();
                toastr.warning("Please Select Effect From");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtFromDate").val() == "") {
                $("#ContentPlaceHolder1_txtToDate").focus();
                toastr.warning("Please Select Effect To");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlRateChartFor").val() == "General" && $("#ContentPlaceHolder1_txtPromotionName").val() == "") {

                $("#ContentPlaceHolder1_txtPromotionName").focus();
                toastr.warning("Please Enter Promotion Name");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlRateChartFor").val() == "Company" && $("#ContentPlaceHolder1_ddlCompany").val() == "0") {

                $("#ContentPlaceHolder1_ddlCompany").focus();
                toastr.warning("Please Select Company.");
                return false;
            }
            else if (FinalQuotationMasterTable.data().length == 0) {
                toastr.warning("Please Add Item / Service To Master.");
                return false;
            }

            var Id = "0", CompanyId = "0", PromotionName = "", EffectFrom = "", EffectTo = "", RateChartFor = "",TotalPrice=0.00;

            Id = $("#ContentPlaceHolder1_hfRateChartMasterId").val();
            PromotionName = $("#ContentPlaceHolder1_txtPromotionName").val();
            RateChartFor = $("#ContentPlaceHolder1_ddlRateChartFor").val();
            CompanyId = $("#ContentPlaceHolder1_ddlCompany").val();
            EffectFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/');
            EffectTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/');

            if (RateChartFor == "Package")
                _(RateChartDetail).each((value, key) => {
                    TotalPrice = TotalPrice + value.TotalPrice;
                });

            var RateChartMaster = {
                Id,
                RateChartFor,
                PromotionName,
                CompanyId,
                EffectFrom,
                EffectTo,
                TotalPrice
            };

            PageMethods.SaveOrUpdateRateChart(RateChartMaster, RateChartDetail, DeletedRateChartDetail, DeletedRateChartDiscountDetail, OnSaveRateChartSucceed, OnSaveRateChartFailed);
            return false;
        }

        function OnSaveRateChartSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            if (result.IsSuccess) {
                GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
                ClearForm();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            return false;
        }

        function OnSaveRateChartFailed(error) {
            CommonHelper.AlertMessage(error);
        }

        function ClearForm() {
            //$("#form1")[0].reset();
            $("#ContentPlaceHolder1_hfRateChartMasterId").val("0");
            $("#ContentPlaceHolder1_txtPromotionName").val("");
            $("#ContentPlaceHolder1_ddlServiceItemType").val("0");
            $("#ContentPlaceHolder1_ddlCompany").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtFromDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtToDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_hfItemId").val("0");
            $("#dvDiscount").hide();
            $("#RestaurantDropDown").hide();
            $("#BanquetDropDown").hide();
            $("#QuotationItemInformation tbody").html("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text("");

            $("#QuotationServicenformation tbody").html("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text("");
            $("#btnSave").val('Save');
            DiscountTable.clear().draw();
            FinalQuotationMasterTable.clear().draw();
            FinalQuotationMaster = null;
            RateChartDetail = new Array();
            RateChartItemDetail = new Array();
            RateChartServiceDetail = new Array();
            DeletedRateChartDetail = [];
            DeletedRateChartDiscountDetail = [];
            TempRateChartDetail = null;
            RateChartDiscountDetails = new Array();
        }

        function GridPaging(pageNumber, isCurrentOrPreviousPage) {

            var gridRecordsCount = $("#tblCostAnalysis tbody tr").length;
            var promotionName, companyId, effectFrom, effectTo;

            promotionName = $("#ContentPlaceHolder1_txtSearchPromotionName").val();
            companyId = $("#ContentPlaceHolder1_ddlSearchCompany").val();
            effectFrom = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            effectTo = $("#ContentPlaceHolder1_txtSearchToDate").val();

            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './RateChartInformation.aspx/GetRateChartListWithPagination',
                data: JSON.stringify({ promotionName, companyId, effectFrom, effectTo, gridRecordsCount, pageNumber, isCurrentOrPreviousPage }),
                dataType: "json",
                success: function (data) {
                    OnSearchSuccess(data.d);
                },
                error: function (error) {
                    OnSearchFail(error.d);
                }
            });
            return false;
        }

        function OnSearchSuccess(result) {
            $("#GridPagingContainer ul").html("");
            SearchRateChart.clear();
            SearchRateChart.rows.add(result.GridData);
            SearchRateChart.draw();
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }

        function OnSearchFail(error) {
            toastr.error(error.get_message());
        }

        function EditRateChart(id) {
            if (!confirm("Do you want to edit?"))
                return false;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './RateChartInformation.aspx/GetRateChartById',
                data: JSON.stringify({ id }),
                dataType: "json",
                success: function (data) {
                    ClearForm();
                    $("#ContentPlaceHolder1_hfRateChartMasterId").val(id);
                    $("#ContentPlaceHolder1_ddlRateChartFor").val(data.d.RateChartMaster.RateChartFor).trigger('change');
                    $("#ContentPlaceHolder1_txtPromotionName").val(data.d.RateChartMaster.PromotionName);
                    $("#ContentPlaceHolder1_ddlCompany").val(data.d.RateChartMaster.CompanyId).trigger('change');
                    $("#ContentPlaceHolder1_txtFromDate").val(CommonHelper.DateFromStringToDisplay(data.d.RateChartMaster.EffectFrom, innBoarDateFormat));
                    $("#ContentPlaceHolder1_txtToDate").val(CommonHelper.DateFromStringToDisplay(data.d.RateChartMaster.EffectTo, innBoarDateFormat));
                    RateChartDetail = data.d.RateChartDetails;
                    LoadFinalQuotationMasterTable();

                    CommonHelper.ApplyIntValidation();
                    $("#CreateNewDialog").dialog({
                        autoOpen: true,
                        modal: true,
                        width: '95%',
                        closeOnEscape: true,
                        resizable: false,
                        height: 'auto',
                        fluid: true,
                        title: `Update Rate Chart - ${data.d.PromotionName}`,
                        show: 'slide'
                    });
                    $("#btnSave").val('Update');
                },
                error: function (error) {
                    OnSearchFail(error.d);
                }
            });
        }

        function DeleteRateChart(rateChartId) {
            if (!confirm("Want to Delete?"))
                return false;
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './RateChartInformation.aspx/DeleteRateChart',
                data: JSON.stringify({ rateChartId }),
                dataType: "json",
                success: function (data) {
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
                },
                error: function (error) {
                    OnSearchFail(error.d.AlertMessage);
                }
            });
            return false;
        }

        function LoadRateChartFor() {
            var RateChartFor = $("#ContentPlaceHolder1_ddlRateChartFor").val();
            if (RateChartFor != "Company") {
                $("#Company").hide();
                $("#Promotion").show();
                RateChartFor == "General" ? $("#lblPromotionName").text("Promotion Name") : $("#lblPromotionName").text("Package Name");
            }
            else {
                $("#Company").show();
                $("#Promotion").hide();
            }
            if (RateChartFor == "Package") {
                $("#ContentPlaceHolder1_ddlServiceItemType option[value='Restaurant'],#ContentPlaceHolder1_ddlServiceItemType option[value='Banquet']").hide();
            }
            else
                $("#ContentPlaceHolder1_ddlServiceItemType option[value='Restaurant'],#ContentPlaceHolder1_ddlServiceItemType option[value='Banquet']").show();
            $("#ContentPlaceHolder1_ddlServiceItemType").trigger('change');
        }

        function CheckForPackage(control) {
            var isPackage = $("#ContentPlaceHolder1_ddlRateChartFor").val() == "Package";
            var ServiceType = $("#ContentPlaceHolder1_ddlServiceItemType").val();
            if (isPackage && ServiceType == "GuestRoom") {
                var checkedLength = $("#tblDiscountItems").find('input[type="checkbox"]:checked').length;
                if (checkedLength > 1) {
                    $(control).prop('checked', false);
                    toastr.warning("Cannot check multiple Room Type.");
                    return false;
                }
            }
        }
    </script>

    <asp:HiddenField ID="hfRateChartMasterId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfItemServiceId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfContactPeriod" runat="server" Value=""></asp:HiddenField>
    <div class="panel panel-default">
        <div class="panel-heading">
            Rate Chart Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-3">
                        <label class="control-label">Promotion / Package Name</label>
                    </div>
                    <div class="col-md-9">
                        <asp:TextBox ID="txtSearchPromotionName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-3">
                        <label class="control-label">Company Name</label>
                    </div>
                    <div class="col-md-9">
                        <asp:DropDownList ID="ddlSearchCompany" runat="server" CssClass="form-control" TabIndex="1"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-3">
                        <label class="control-label">Effect From</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" TabIndex="1" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-3">
                        <label class="control-label">Effect To</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" TabIndex="1" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnCreateNew" runat="server" TabIndex="4" Text="Create New" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
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
            <table id="tblRateChart" class="table table-bordered table-condensed table-responsive">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div id="CreateNewDialog" style="display: none; overflow: unset">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Rate Chart For</label>
                </div>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlRateChartFor" runat="server" CssClass="form-control" onchange="LoadRateChartFor()">
                        <asp:ListItem Value="General">General</asp:ListItem>
                        <asp:ListItem Value="Company">Company</asp:ListItem>
                        <asp:ListItem Value="Package">Package</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group" id="Promotion">
                <div class="col-md-2">
                    <label id="lblPromotionName" class="control-label required-field">Promotion Name</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtPromotionName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                </div>
            </div>
            <div class="form-group" id="Company">
                <div class="col-md-2">
                    <label class="control-label required-field">Company Name</label>
                </div>
                <div class="col-md-10">
                    <asp:DropDownList runat="server" ID="ddlCompany" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">
                        Effect From
                    </label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="6" autocomplete="off"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label class="control-label required-field">
                        Effect To
                    </label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TabIndex="7" autocomplete="off"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label">Service Type</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlServiceItemType" TabIndex="1" CssClass="form-control" runat="server" onchange="LoadService(this)">
                    </asp:DropDownList>
                </div>
                <div id="dvDiscountTo" style="display: none;">
                    <div class="col-md-2">
                        <label class="control-label">Discount To</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDiscountTo" TabIndex="1" CssClass="form-control" runat="server" onchange="LoadDiscountDiv()">
                            <asp:ListItem Value="1">Over All</asp:ListItem>
                            <asp:ListItem Value="0">Details</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group" id="dvDiscountAmount" style="display: none;">
                <div class="col-md-2">
                    <label class="control-label">Discount Type</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlAllDiscountType" TabIndex="1" CssClass="form-control" runat="server" onchange="ChangeDiscountType(this)">
                        <asp:ListItem Value="Fixed">Fixed</asp:ListItem>
                        <asp:ListItem Value="Percentage">Percentage</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class="control-label">Discount Amount</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="txtAmount" CssClass="form-control quantitydecimal" placeholder="Local"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="txtAmountUSD" CssClass="form-control quantitydecimal" placeholder="USD"></asp:TextBox>
                </div>
            </div>
            <div class="form-group" id="RestaurantDropDown" style="display: none;">
                <div class="col-md-2">
                    <label class="control-label">Restaurant Name</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList runat="server" ID="ddlRestaurant" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group" id="BanquetDropDown" style="display: none;">
                <div class="col-md-2">
                    <label class="control-label">Banquet Name</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList runat="server" ID="ddlBanquet" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="panel panel-default" id="dvItem" style="display: none;">
                <div class="panel-heading"><span id="ItemQuotationTitle">Item Information</span></div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:HiddenField ID="hfItemCategoryId" runat="server" Value="0" />
                                <input id="txtItemCategory" type="text" class="form-control" />
                                <asp:DropDownList Style="display: none;" ID="ddlCategory" CssClass="form-control" runat="server" TabIndex="13">
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
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnAddItem" tabindex="17" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItem()">
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
                                    <th scope="col" style="width: 30%;">Item Name</th>
                                    <th scope="col" style="width: 15%;">Unit</th>
                                    <th scope="col" style="width: 15%;">Quantity</th>
                                    <th scope="col" style="width: 15%;">Unit Cost</th>
                                    <th scope="col" style="width: 15%;">Total Cost</th>
                                    <th scope="col" style="width: 10%;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="2" style="width: 45%; padding-right: 5px; text-align: right;">Total:</td>
                                    <td style="width: 15%;"></td>
                                    <td style="width: 15%;"></td>
                                    <td style="width: 15%;"></td>
                                    <td style="width: 10%;"></td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
                    </div>
                </div>
            </div>
            <div class="panel panel-default" id="dvService" style="display: none;">
                <div class="panel-heading"><span id="ItemServiceTitle">Service Information</span></div>
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
                                <asp:Label ID="Label6" runat="server" class="control-label" Text="Package Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPackageName" runat="server" CssClass="form-control" TabIndex="22">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 ">
                                <asp:Label ID="Label18" runat="server" class="control-label required-field" Text="Contract Period"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlContractPeriod" runat="server" CssClass="form-control" TabIndex="6"></asp:DropDownList>
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
                                <asp:Label ID="Label23" runat="server" class="control-label" Text="Unit Price"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtUnitPrice" TabIndex="24" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label22" runat="server" class="control-label" Text="Service Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBandwidthServiceType" runat="server" CssClass="form-control" TabIndex="25">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnAddServiceItem" tabindex="26" class="TransactionalButton btn btn-primary btn-sm" onclick="AddServiceItem()">
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
                                    <th style="width: 10%;">Service</th>
                                    <th style="width: 15%;">Prduct Details</th>
                                    <th style="width: 15%;">Package/s</th>
                                    <th style="width: 10%;">Downlink</th>
                                    <th style="width: 10%;">Uplink</th>
                                    <th style="width: 15%;">Price/ Month</th>
                                    <th style="width: 15%;">Price/ 3 Month</th>
                                    <th style="width: 10%;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="5" style="width: 45%; padding-right: 5px; text-align: right;">Total:</td>
                                    <td style="width: 15%;"></td>
                                    <td style="width: 15%;"></td>
                                    <td style="width: 10%;"></td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
                    </div>

                </div>
            </div>
            <div class="panel panel-default" id="dvDiscount" style="display: none;">
                <div class="panel-heading"><span id="discountTableHeader"></span></div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <table id="tblDiscountItems" class="table table-condensed table-bordered table-responsive">
                            </table>
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <button type="button" id="btnAddDiscount" tabindex="17" class="TransactionalButton btn btn-primary btn-sm" onclick="return AddDiscount()">
                        Add to Master</button>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-12">
                    <table id="tblRateChartDetails" class="table table-condensed table-bordered table-responsive">
                    </table>
                </div>
            </div>
        </div>
        <div class="row" style="padding-bottom: 0; padding-top: 10px;">
            <div class="col-md-12">
                <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="ValidationNPreprocess()" value="Save" />
                <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return ClearForm();" />
            </div>
        </div>
    </div>
</asp:Content>
