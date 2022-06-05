<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmSMQuotation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmSMQuotation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var v = null;
        var deleteSalesObj = [];
        var SearchItem = null;
        var SearchService = null;

        var QuotationItemDetails = new Array();
        var QuotationDeletedItemDetails = new Array();

        var QuotationServiceDetails = new Array();
        var QuotationDeletedServiceDetails = new Array();

        var PriceMatrix = null;
        var ContactPeriod = new Array();
        var isClose, flagValidation;
        var isLoadFromCostAnalysis = false;
        var DiscountTable;
        var FinalQuotationMasterTable;
        var FinalQuotationMaster = null;
        var QuotationDetail = new Array();
        var DeletedQuotationDetail = [];
        var DeletedQuotationDiscountDetail = [];
        var TempQuotationDetail = null;
        var QuotationDiscountDetails = new Array();

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Sales & Marketing</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Sales Call</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            DiscountTable = $("#tblDiscountItems").DataTable({
                data: [],
                columns: [
                    { title: "<input id='chkAllItem' type='checkbox'></input>", "data": null, sWidth: '5%' },
                    { title: "", "data": "TypeName", sWidth: '35%' },
                    { title: "Discount Type", "data": "DiscountType", sWidth: '15%' },
                    { title: "DiscountAmount (Local)", "data": "DiscountAmount", sWidth: '15%' },
                    { title: "Discount Amount (USD)", "data": "DiscountAmountUSD", sWidth: '15%' },
                    { title: "", "data": "Type", sWidth: '15%', visible: false },
                    { title: "", "data": "Id", visible: false }
                ],
                columnDefs: [
                    {
                        "targets": 0,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            if (data.Id > 0)
                                return '<input type="checkbox" checked="checked" />';
                            else
                                return '<input type="checkbox" />';
                        }
                    },
                    {
                        "targets": 2,
                        "render": function (data, type, full, meta) {
                            return '<select class="form-control" onchange="ShowHideDiscountAmount(this)">' +
                                '<option value="0">---Please Select---</option>' +
                                '<option value="Fixed">Fixed</option>' +
                                '<option value="Percentage">Percentage</option>' +
                                '</select > ';
                        }
                    },
                    {
                        "targets": [3],
                        "render": function (data, type, full, meta) {
                            return '<input type="text" class="form-control quantitydecimal" value="' + data + '" />';
                        }
                    }
                    ,
                    {
                        "targets": [4],
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
            FinalQuotationMasterTable = $("#tblQuotationDetails").DataTable({
                data: [],
                columns: [
                    { title: "Service Type", "data": "ServiceTypeColumn", sWidth: '35%' },
                    { title: "", "data": "QuotationDetailsId", visible: false },
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
                    row += "&nbsp;<a href=\"javascript:void(0);\" onclick=\"EditDiscountDetails(this," + aData.QuotationDetailsId + "," + aData.OutLetId + ",'" + aData.ServiceType + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    row += "&nbsp;&nbsp;<a href=\"javascript:void(0);\" onclick= \"DeleteDiscountDetails(this," + aData.QuotationDetailsId + "," + aData.OutLetId + ",'" + aData.ServiceType + "');\"> <img alt='Delete' src='../Images/delete.png' /></a>";

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

            CommonHelper.ApplyDecimalValidation();
            CommonHelper.ApplyIntValidation();
            $("[id=chkAllItem]").on("change", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#tblDiscountItems tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#tblDiscountItems tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
                }
            });
            $("#ContentPlaceHolder1_ddlCostAnalysis").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
                placeholder: "--- Please Select ---"
            });

            $("#ContentPlaceHolder1_txtSearchFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {

                    var strDate = CommonHelper.DateFormatToMMDDYYYY($(this).val(), '/');
                    minEndDate = GetStringFromDateTime(CommonHelper.DaysAdd(strDate, 1));

                    $("#ContentPlaceHolder1_txtSearchToDate").datepicker("option", {
                        minDate: minEndDate
                    });
                }
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_txtSearchToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: DayOpenDate,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtSearchFromDate").datepicker("option", "maxDate", selectedDate);

                }
            }).datepicker("setDate", DayOpenDate);


            if ($("#ContentPlaceHolder1_hfContactPeriod").val() != "") {
                ContactPeriod = JSON.parse($("#ContentPlaceHolder1_hfContactPeriod").val());
            }

            $("#ContentPlaceHolder1_txtProposalDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $('#ContentPlaceHolder1_txtProposalDate').datepicker('setDate', 'today');

            CommonHelper.AutoSearchClientDataSource("txtCompanySearch", "ContentPlaceHolder1_ddlCompany", "ContentPlaceHolder1_hfCompanyId");
            CommonHelper.AutoSearchClientDataSource("txtSCompanySearch", "ContentPlaceHolder1_ddlSCompany", "ContentPlaceHolder1_hfSCompanyId");
            CommonHelper.AutoSearchClientDataSource("txtItemCategory", "ContentPlaceHolder1_ddlCategory", "ContentPlaceHolder1_hfItemCategoryId");

            //$("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });


            $('#txtItemCategory').blur(function () {

                if (!$(this).val())
                    $("#ContentPlaceHolder1_hfItemCategoryId").val('0');
            });
            $('#txtCompanySearch').blur(function () {
                if ($(this).val() != "") {
                    var cmpId = $("#ContentPlaceHolder1_hfCompanyId").val();
                    if (cmpId != "") {
                        LoadCompanyInfo(cmpId);
                        //LoadSiteByCompanyId(cmpId);
                    }
                }
            });

            $('#txtSCompanySearch').blur(function () {
                if ($(this).val() != "") {
                    var cmpId = $("#ContentPlaceHolder1_hfSCompanyId").val();
                    if (cmpId != "") {
                        LoadSiteBySCompanyId(cmpId);
                    }
                }
            });

            ShowOrHideCostAnalysis($("#chkIsCreateFromCostAnalysis"));

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
            });

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

            var quotationId = $.trim(CommonHelper.GetParameterByName("Id"));
            var dealId = $.trim(CommonHelper.GetParameterByName("did"));
            var companyId = $.trim(CommonHelper.GetParameterByName("cid"));
            var contactId = $.trim(CommonHelper.GetParameterByName("conid"));

            if (quotationId != "") {

                PerformEditAction(parseInt(quotationId));
                $("#dvCustomer *").attr("disabled", true);
            }

            if (dealId != "" && companyId != "" && contactId != "") {
                var siteServeyId = $.trim(CommonHelper.GetParameterByName("sid")) == "" ? 0 : parseInt($.trim(CommonHelper.GetParameterByName("sid")));

                var isQuotationCreateFromSiteServeyFeedback = $("#ContentPlaceHolder1_hfIsQuotationCreateFromSiteServeyFeedback").val();

                if (siteServeyId == 0)
                    siteServeyId = GetSiteServeyId(dealId, companyId, contactId);

                if (siteServeyId == -1)
                    return false;
                else if (siteServeyId > 0) {
                    GetSiteServeyFor(siteServeyId);
                    if (isQuotationCreateFromSiteServeyFeedback == "1")
                        GetSiteServeyFeedBackId(siteServeyId);
                }
                LoadSetupData(parseInt(dealId), parseInt(companyId), parseInt(contactId));
            }
            if (UserInfoFromDB.IsAdminUser) {
                //$("#btnCostAnalysis").show();
                //$("#dvChkCostAnalysis").show();
                $("#btnCostAnalysis").hide();
                $("#dvChkCostAnalysis").hide();
            }
            else {
                $("#btnCostAnalysis").hide();
                $("#dvChkCostAnalysis").hide();
            }

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
        });

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


        function ShowOrHideCostAnalysis(control) {
            if ($(control).is(":checked")) {
                GridPaging(1, 1);
                $("#CostAnalysisSearch").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 800,
                    height: 600,
                    minWidth: 550,
                    minHeight: 580,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    title: "Cost Analysis",
                    show: 'slide'
                });
            }
        }

        function GetSiteServeyId(dealId, companyId, contactId) {

            var SiteServeyId = 0;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SiteSurveyInformation.aspx/LoadSiteSurveyForSearch',
                data: "{'dealId':'" + dealId + "','companyId':'" + companyId + "','contactId':'" + contactId + "', 'gridRecordsCount':'" + 0 + "', 'pageNumber':'" + 1 + "', 'isCurrentOrPreviousPage':'" + 1 + "'}",
                dataType: "json",
                async: false,
                success: function (data) {

                    const count = data.d.GridData.length;

                    if (count == 1) {
                        let SiteServey = data.d.GridData[0];
                        SiteServeyId = SiteServey.Id;
                    }
                    //For multiple site servey need to select site servey
                    if (count > 1) {
                        SiteServeyId = -1;
                        var iframeid = 'frmPrint';
                        var url = "./SiteSurveyInformation.aspx?dId=" + dealId + "&cId=" + companyId + "&conId=" + contactId + "&isFromQuotation=1";
                        parent.document.getElementById(iframeid).src = url;

                    }
                },
                error: function (result) {
                }
            });

            return SiteServeyId;
        }

        function LoadSetupData(dealId, companyId, contactId) {
            $("#ContentPlaceHolder1_hfQDealId").val(dealId);
            $("#ContentPlaceHolder1_hfCompanyId").val(companyId);
            $("#ContentPlaceHolder1_hfContactId").val(contactId);
            $("#ContentPlaceHolder1_ddlDealName").attr('disabled', true);
            $("#ContentPlaceHolder1_txtCompanyOrContactName").attr('disabled', true);

            LoadDealName(companyId, contactId);

            if (companyId != 0) {

                CommonHelper.AutoSearchClientDataSource("ContentPlaceHolder1_txtCompanyOrContactName", "ContentPlaceHolder1_ddlCompany", "ContentPlaceHolder1_hfCompanyId");

                $("#ContentPlaceHolder1_lblCompanyOrContactName").text("Company Name");
                $("#pnlContact").hide();
                $("#pnlCompany").show();

                LoadCompanyInfo(companyId);
                //LoadSiteByCompanyId(companyId);
            }
            else {
                CommonHelper.AutoSearchClientDataSource("ContentPlaceHolder1_txtCompanyOrContactName", "ContentPlaceHolder1_ddlIndependentContact", "ContentPlaceHolder1_hfCompanyId");
                GetContactInfo(contactId);
                $("#ContentPlaceHolder1_txtCompanyOrContactName").val($("#ContentPlaceHolder1_ddlIndependentContact").find("option[value=" + contactId + "]").text());
                $("#ContentPlaceHolder1_lblCompanyOrContactName").text("Contact Name");
                $("#pnlContact").show();
                $("#pnlCompany").hide();
            }
            $("#dvCustomer *").attr("disabled", true);
        }

        function GetSiteServeyFeedBackId(siteServeyId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SiteSurveyFeedback.aspx/CheckFeedback',
                data: "{'siteSurveyId':'" + siteServeyId + "'}",
                dataType: "json",
                async: false,
                success: function (result) {
                    if (result.d > 0) {
                        GetSiteServeyFeedbackItem(result.d);
                    }
                },
                error: function (error) {
                    toastr.error(error.d.AlertMessage);
                }
            });
        }

        function GetSiteServeyFor(siteServeyId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SiteSurveyInformation.aspx/LoadSurveyDetailsById',
                data: "{'id':'" + siteServeyId + "'}",
                dataType: "json",
                async: false,
                success: function (result) {
                    if (result.d.SiteSurveyNote != null)
                        $("#ContentPlaceHolder1_ddlServiceType").val(result.d.SiteSurveyNote.SegmentId);
                },
                error: function (error) {
                    toastr.error(error.d.AlertMessage);
                }
            });
        }

        function GetSiteServeyFeedbackItem(siteServeyFeebackId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/frmSMQuotation.aspx/LoadQuotationDetailsFromSiteServeyFeedback',
                async: false,
                data: "{'feedbackId':'" + siteServeyFeebackId + "'}",
                dataType: "json",
                success: function (result) {
                    LoadSiteServeyFeedbackItem(result.d);
                },
                error: function (error) {
                    toastr.error(error.d.AlertMessage);
                }
            });
        }


        function LoadSiteServeyFeedbackItem(result) {

            $.each(result, function (index, itm) {
                SearchItem = {
                    label: itm.Name,
                    value: itm.ItemId,

                    Name: itm.Name,
                    CategoryId: itm.CategoryId,
                    ItemId: itm.ItemId,
                    StockById: itm.StockBy,
                    UnitPriceLocal: itm.UnitPriceLocal,
                    UnitHead: itm.UnitHead
                };
                $("#txtItemName").val(itm.Name);
                $("#ContentPlaceHolder1_txtItemUnit").val(itm.Quantity);
                PageMethods.LoadRelatedStockBy(itm.StockBy, OnLoadStockBySucceeded, OnLoadStockByFailed);
                AddItem();
            });
            $("#ContentPlaceHolder1_ddlServiceItemType").val("Item").trigger('change');
        }

        function LoadDealName(companyId, contactId) {

            var cmpId = parseInt(companyId);
            var cntId = parseInt(contactId);

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../SalesAndMarketing/frmSMQuotation.aspx/GetAllDealByCompanyIdNContactId",
                dataType: "json",
                data: JSON.stringify({ companyId: cmpId, contactId: cntId }),
                async: false,
                success: (data) => {
                    LoadDealDropDown(data.d);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
        }

        function GetContactInfo(contactId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../SalesAndMarketing/frmSMQuotation.aspx/GetContactInfoById",
                dataType: "json",
                data: JSON.stringify({ id: contactId }),
                async: false,
                success: (data) => {
                    LoadContactInfo(data.d);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });


        }

        function LoadContactInfo(result) {
            //$("#ContentPlaceHolder1_txtCompanyOrContactName").val(result.Name);
            $("#ContentPlaceHolder1_txtContactName").val(result.Name);
            $("#ContentPlaceHolder1_lblContactTitle").text(result.JobTitle);
            $("#ContentPlaceHolder1_lblDepartment").text(result.Department);
            $("#ContentPlaceHolder1_lblContactMobile").text(result.MobilePersonal);
            $("#ContentPlaceHolder1_lblContactPhone").text(result.PhonePersonal);
            $("#ContentPlaceHolder1_lblConatctEmail").text(result.EmailWork);
            $("#ContentPlaceHolder1_lblPersonalAddress").text(result.PersonalAddress);
            $("#ContentPlaceHolder1_lblContactLifeCycleStage").text(result.ContactLifeCycleStage);

        }

        function LoadDealDropDown(results) {

            $("#ContentPlaceHolder1_ddlDealName").empty();

            var i = 0, fieldLength = results.length;

            if (fieldLength > 0) {
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + results[i].Id + '">' + results[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlDealName');
                }
            }
            else {
                $("<option value='0'>--No Deal Found--</option>").appendTo("#ContentPlaceHolder1_ddlDealName");
            }
            if (fieldLength == 1 && $("#ContentPlaceHolder1_hfQDealId").val() != "0")
                $("#ContentPlaceHolder1_ddlDealName").val($("#ContentPlaceHolder1_ddlDealName option:first").val());
            if ($("#ContentPlaceHolder1_hfQDealId").val() != "0")
                $("#ContentPlaceHolder1_ddlDealName").val($("#ContentPlaceHolder1_hfQDealId").val());
        }

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

        function PopulateLocations(city, location) {
            if ($("#" + city).val() == "0") {
                $("#" + location).empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }
            else {
                $("#" + location).empty().append('<option selected="selected" value="0">Loading...</option>');
                $.ajax({
                    type: "POST",
                    url: "/SalesAndMarketing/frmSalesCall.aspx/PopulateLocations",
                    data: '{cityId: ' + $("#" + city).val() + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        OnLocationsPopulated(response, location);
                    },
                    failure: function (response) {
                        toastr.error(response.d);
                    }
                });
            }
        }

        function OnLocationsPopulated(response, Project) {
            $("#" + Project).attr("disabled", false);
            PopulateControl(response.d, $("#" + Project), $("#<%=CommonDropDownHiddenField.ClientID %>").val());
        }


        function PerformEditAction(quotationId) {

            PageMethods.LoadQuotation(quotationId, OnLoadQuotationSucceeded, OnLoadQuotationFailed);
            return false;
        }
        function PerformApproval(quotationId, dealId) {
            PageMethods.PerformApproval(quotationId, dealId, OnLoadApprovalSucceed, OnLoadApprovalFailed);
            return false;
        }
        function OnLoadApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnLoadApprovalFailed() {

        }

        function OnLoadQuotationSucceeded(result) {

            var isCopy = $.trim(CommonHelper.GetParameterByName("isCopy"));
            if (!isLoadFromCostAnalysis) {
                $("#dvChkCostAnalysis").hide();
                if (isCopy == "1")
                    $("#ContentPlaceHolder1_hfQuotationId").val("0");
                else
                    $("#ContentPlaceHolder1_hfQuotationId").val(result.Quotation.QuotationId);
                QuotationDetail = result.QuotationDetails;
                QuotationItemDetails = result.QuotationItemDetails;
                QuotationServiceDetails = result.QuotationServiceDetails;
                LoadFinalQuotationMasterTable();
                //if Item and Service load from previous Cost Analysis dont need this section

                $("#ContentPlaceHolder1_hfQDealId").val(result.Quotation.DealId);
                $("#ContentPlaceHolder1_hfCompanyId").val(result.Quotation.CompanyId);
                $("#ContentPlaceHolder1_hfContactId").val(result.Quotation.CompanyId);
                if (result.Quotation.DealId > 0)
                    LoadSetupData(result.Quotation.DealId, result.Quotation.CompanyId, result.Quotation.ContactId);

                $("#ContentPlaceHolder1_txtProposalDate").val(GetStringFromDateTime(result.Quotation.ProposalDate));
                $("#ContentPlaceHolder1_ddlServiceType").val(result.Quotation.ServiceTypeId);
                $("#ContentPlaceHolder1_txtDeviceOrUser").val(result.Quotation.TotalDeviceOrUser);
                $("#ContentPlaceHolder1_ddlContractPeriod").val(result.Quotation.ContractPeriodId);
                $("#ContentPlaceHolder1_ddlBillingPeriod").val(result.Quotation.BillingPeriodId);
                $("#ContentPlaceHolder1_ddlDeliveryBy").val(result.Quotation.ItemServiceDeliveryId);
                $("#ContentPlaceHolder1_txtPriceValidity").val(result.Quotation.PriceValidity);
                $("#ContentPlaceHolder1_txtWhereToDeploy").val(result.Quotation.DeployLocation);
                $("#ContentPlaceHolder1_ddlCurrentVendor").val(result.Quotation.CurrentVendorId);
                $("#ContentPlaceHolder1_txtRemarks").val(result.Quotation.Remarks);
                if (result.Quotation.CompanyId > 0)
                    LoadCompanyInfo(result.Quotation.CompanyId);
                //LoadSiteByCompanyId(result.Quotation.CompanyId);
            }
            //$("#ContentPlaceHolder1_ddlServiceItemType").val("Item").trigger('change');
            var tr = "", deleteLink = "<a href=\"javascript:void()\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            QuotationItemDetails = result.QuotationItemDetails;
            QuotationServiceDetails = result.QuotationServiceDetails;

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
            $.each(result.QuotationItemDetails, function (index, itm) {

                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"display:none;\">" + (isCopy != "1" ? itm.QuotationDetailsId : "0") + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ItemId + "</td>";
                tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + itm.ItemName + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.HeadName + "</td>";

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

            var contractPeriodValue = 0.00;

            if (ContactPeriod.length != 0 && result.Quotation.ContractPeriodId > 0) {
                var cp = _.findWhere(ContactPeriod, { ContractPeriodId: result.Quotation.ContractPeriodId });
                contractPeriodValue = cp.ContractPeriodValue;
            }

            tr = ""; totalQuantity = 0.00; totalUnitCost = 0.00; totalCost = 0.00;
            $("#QuotationServicenformation thead tr:eq(0)").find("th:eq(6)").text("Price/ " + contractPeriodValue + " Month");

            var serviceType = "", packageName = "", bandWidthName = "", downLink = "", upLink = "";
            $.each(result.QuotationServiceDetails, function (index, itm) {

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

                tr += "<td align='left' style=\"display:none;\">" + itm.QuotationDetailsId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ItemId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.StockById + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.Quantity + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServicePackageId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServiceBandWidthId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServiceTypeId + "</td>";

                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + (itm.ServiceType || serviceType) + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.ItemName + "</td>";
                //tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + (itm.PackageName || packageName) + "</td>";
                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + (downLink) + "</td>";

                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" +
                //    " <input type=\"text\" disabled='disabled' id='itm" + itm.ItemId + "' value='" + upLink + "' class=\"form-control\" />" +
                //    "</td>";

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

            //$("#myTabs").tabs({ active: 0 });
            if (!isLoadFromCostAnalysis) {
                $("#btnSaveNContinue").hide();
                $("#ContentPlaceHolder1_btnCancel").hide();
                $("#ContentPlaceHolder1_btnSave").val('Update & Close');
            }
            return false;
        }
        function OnLoadQuotationFailed(error) {
            toastr.error(error);
        }

        function EditQuantity(tr) {

            tr = $(tr).parent().parent();

            var itemId = parseInt($(tr).find("td:eq(1)").text());
            var item = _.findWhere(QuotationItemDetails, { ItemId: itemId, ItemType: "Item" });

            var quantity = parseInt($(tr).find("td:eq(4)").find("input").val());
            var unitPrice = parseInt($(tr).find("td:eq(5)").find("input").val());
            $(tr).find("td:eq(6)").text(parseFloat(quantity) * parseFloat(unitPrice));

            if (item != null) {
                item.Quantity = parseFloat(quantity);
                item.UnitPrice = parseFloat(unitPrice);
                item.TotalPrice = item.UnitPrice * item.Quantity;
            }

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
            $.each(QuotationItemDetails, function (index, itm) {
                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalCost = totalCost + itm.TotalPrice;
            });

            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text(totalCost);
        }

        function LoadCompanyInfo(companyId) {
            PageMethods.LoadCompanyInfo(companyId, OnLoadCompanySucceeded, OnLoadCompanyFailed);
            return false;
        }
        function OnLoadCompanySucceeded(result) {

            if ($.trim($("#txtCompanySearch").val()) == "") {
                $("#txtCompanySearch").val(result.CompanyName);
            }
            $("#ContentPlaceHolder1_txtCompanyType").val(result.TypeName);
            $("#ContentPlaceHolder1_txtIndustry").val(result.IndustryName);
            $("#ContentPlaceHolder1_txtOwnership").val(result.OwnershipName);
            $("#ContentPlaceHolder1_lblContactNo").text(result.ContactNumber);
            $("#ContentPlaceHolder1_lblEMail").text(result.EmailAddress);
            $("#ContentPlaceHolder1_lblWebsite").text(result.WebAddress);
            $("#ContentPlaceHolder1_lblLifeCycleStage").text(result.LifeCycleStage);
            $("#ContentPlaceHolder1_txtCompanyOrContactName").val(result.CompanyName);

            var adress = "";
            adress += (result.BillingStreet != null && result.BillingStreet != "") ? result.BillingStreet + " , " : "";
            adress += (result.BillingState != null && result.BillingStreet != "") ? result.BillingState + " , " : "";
            adress += (result.BillingCity != null && result.BillingStreet != "") ? result.BillingCity + " , " : "";
            adress += (result.BillingPostCode != null && result.BillingStreet != "") ? "PostCode: " + result.BillingPostCode + " , " : "";
            adress += (result.BillingCountry != null && result.BillingStreet != "") ? result.BillingCountry + " , " : "";

            $("#ContentPlaceHolder1_lblCompanyAddress").val(adress);
            $("#ContentPlaceHolder1_hfCompanyId").val(result.CompanyId);

        }
        function OnLoadCompanyFailed(error) {
            toastr.error(error);
        }

        function LoadSiteByCompanyId(companyId) {
            PageMethods.LoadCompanySite(companyId, OnLoadSiteByCompanySucceeded, OnLoadSiteByCompanyFailed);
        }

        function OnLoadSiteByCompanySucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlCompanySite');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].SiteId + '">' + list[i].SiteName + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            control.val($("#ContentPlaceHolder1_hfCompanySiteId").val());
            return false;
        }
        function OnLoadSiteByCompanyFailed() { }

        function LoadSiteBySCompanyId(companyId) {
            PageMethods.LoadCompanySite(companyId, OnLoadSiteBySCompanySucceeded, OnLoadSiteBySCompanyFailed);
        }
        function OnLoadSiteBySCompanySucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlSCompanySite');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].SiteId + '">' + list[i].SiteName + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            // control.val($("#ContentPlaceHolder1_hfCompanySiteId").val());
            return false;
        }
        function OnLoadSiteBySCompanyFailed() { }

        function AddItem() {

            if (SearchItem == null) {
                return false;
            }

            var existingItem = _.findWhere(QuotationItemDetails, { ItemId: SearchItem.ItemId, ItemType: "Item" });
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

            QuotationItemDetails.push({

                QuotationDetailsId: 0,
                QuotationId: 0,
                ItemType: 'Item',
                CategoryId: SearchItem.CategoryId,

                ServicePackageId: 0,
                ServiceBandWidthId: 0,
                ServiceTypeId: 0,
                ItemName: SearchItem.Name,
                ItemId: SearchItem.ItemId,
                StockBy: SearchItem.StockById,
                UnitHead: SearchItem.UnitHead,
                Quantity: parseFloat(itemQuantity),
                UnitPrice: SearchItem.UnitPriceLocal,
                TotalPrice: itemCost,
                AverageCost: SearchItem.AverageCost
            });

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;

            $.each(QuotationItemDetails, function (index, itm) {
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

            //else if ($.trim(packageId) == "0" || $.trim(packageId) == "") { toastr.warning("Please Select package."); return false; }

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

            //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + serviceType + "</td>";
            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + SearchService.Name + "</td>";
            //tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + packageName + "</td>";
            //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + downLink + "</td>";

            //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" +
            //    " <input type=\"text\" disabled='disabled' id='itm" + SearchService.ItemId + "' value='" + upLink + "' class=\"form-control\" />" +
            //    "</td>";

            tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + pricePerMonth + "</td>";
            tr += "<td align='left' style='width: 15%;'>" + priceTotalByContactPeriod + "</td>";
            tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

            tr += "</tr>";

            $("#QuotationServicenformation tbody").append(tr);

            QuotationServiceDetails.push({

                QuotationDetailsId: 0,
                QuotationId: 0,
                ItemType: 'Service',
                CategoryId: SearchService.CategoryId,
                ServicePackageId: packageId,
                ServiceBandWidthId: bandwidthId,
                ServiceTypeId: serviceTypeId,
                ItemId: SearchService.ItemId,
                ItemName: SearchService.Name,
                StockBy: SearchService.StockById,
                Quantity: parseFloat(itemQuantity),
                UnitPrice: parseFloat(pricePerMonth),
                TotalPrice: parseFloat(priceTotalByContactPeriod)
                //, UpLink: upLink
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
            var item = _.findWhere(QuotationItemDetails, { ItemId: parseInt(itemId, 10), ItemType: "Item" });

            if (item != null) {

                if (item.QuotationDetailsId != 0) {
                    QuotationDeletedItemDetails.push(item);
                }
                var index = QuotationItemDetails.indexOf(item);
                QuotationItemDetails.splice(index, 1);

                var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
                $.each(QuotationItemDetails, function (index, itm) {
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
            var item = _.findWhere(QuotationServiceDetails, { ItemId: parseInt(itemId, 10) });
            var row = 0, rowLength = QuotationServiceDetails.length, index = -1;

            for (row = 0; row < rowLength; row++) {
                if (QuotationServiceDetails[row].ServicePackageId == parseInt(servicePackageId, 10) &&
                    QuotationServiceDetails[row].ServiceTypeId == parseInt(serviceTypeId, 10) &&
                    QuotationServiceDetails[row].ItemId == parseInt(itemId, 10)) {

                    index = row;
                    if (QuotationServiceDetails[row].QuotationDetailsId != 0) {
                        QuotationDeletedItemDetails.push(QuotationServiceDetails[row]);
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

        function ValidationNPreprocess() {
            //CommonHelper.SpinnerOpen();

            if ($("#ContentPlaceHolder1_hfCompanyId").val() != "0" && $("#txtCompanySearch").val() == "") {
                flagValidation = true;
                toastr.warning("Please Select Company");
                return false;
            }

            else if ($("#ContentPlaceHolder1_txtProposalDate").val() == "") {
                $("#ContentPlaceHolder1_txtProposalDate").focus();
                flagValidation = true;
                toastr.warning("Please Select Proposal Date");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlServiceType").val() == "" || $("#ContentPlaceHolder1_ddlServiceType").val() == "0") {
                flagValidation = true;
                $("#ContentPlaceHolder1_ddlServiceType").focus();
                toastr.warning("Please Select Proposal For");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtDeviceOrUser").val() == "" || $.trim($("#ContentPlaceHolder1_txtDeviceOrUser").val()) == "0") {
                flagValidation = true;
                $("#ContentPlaceHolder1_txtDeviceOrUser").focus();
                toastr.warning("Please Give Device User Quantity");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlContractPeriod").val() == "" || $("#ContentPlaceHolder1_ddlContractPeriod").val() == "0") {
                flagValidation = true;
                $("#ContentPlaceHolder1_ddlContractPeriod").focus();
                toastr.warning("Please Select Contract Period");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlBillingPeriod").val() == "" || $("#ContentPlaceHolder1_ddlBillingPeriod").val() == "0") {
                flagValidation = true;
                $("#ContentPlaceHolder1_ddlBillingPeriod").focus();
                toastr.warning("Please Select Billing Period");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlDeliveryBy").val() == "" || $("#ContentPlaceHolder1_ddlDeliveryBy").val() == "0") {
                flagValidation = true;
                $("#ContentPlaceHolder1_ddlDeliveryBy").focus();
                toastr.warning("Please Select Delivery");
                return false;
            }
            else if (FinalQuotationMasterTable.data().length == 0) {
                flagValidation = true;
                toastr.warning("Please Add Item / Service To Master.");
                return false;
            }
            //else if (QuotationItemDetails.length == 0 && QuotationServiceDetails.length == 0 && QuotationDetail.length==0) {
            //    flagValidation = true;
            //    toastr.warning("Please Add Item / Service");
            //    return false;
            //}
            //else if ($("#ContentPlaceHolder1_ddlCurrentVendor").val() == "" || $("#ContentPlaceHolder1_ddlCurrentVendor").val() == "0") {
            //    toastr.info("Please Select Current Vendor");
            //    return false;
            //}

            var quotationId = "0", companyId = "0", proposalDate = "", serviceTypeId = "0", locationId = 0, dealId = "0";
            var siteId = 0, totalDeviceOrUser = "", contractPeriodId = "", billingPeriodId = "", itemServiceDeliveryId = "0";
            var currentVendorId = "0", remarks = "0", priceValidity = "", deployLocation = "", contactId = "";

            quotationId = $("#ContentPlaceHolder1_hfQuotationId").val();
            dealId = +$("#ContentPlaceHolder1_hfQDealId").val();
            companyId = $("#ContentPlaceHolder1_hfCompanyId").val();
            proposalDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtProposalDate").val(), '/');
            serviceTypeId = $("#ContentPlaceHolder1_ddlServiceType").val();
            totalDeviceOrUser = $("#ContentPlaceHolder1_txtDeviceOrUser").val();
            contractPeriodId = $("#ContentPlaceHolder1_ddlContractPeriod").val();
            billingPeriodId = $("#ContentPlaceHolder1_ddlBillingPeriod").val();
            itemServiceDeliveryId = $("#ContentPlaceHolder1_ddlDeliveryBy").val();
            currentVendorId = $("#ContentPlaceHolder1_ddlCurrentVendor").val();
            priceValidity = $("#ContentPlaceHolder1_txtPriceValidity").val();
            deployLocation = $("#ContentPlaceHolder1_txtWhereToDeploy").val();
            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            contactId = parseInt($("#ContentPlaceHolder1_hfContactId").val());

            var Quotation = {
                QuotationId: quotationId,
                CompanyId: companyId,
                DealId: dealId,
                ContactId: contactId,
                ProposalDate: proposalDate,
                ServiceTypeId: serviceTypeId,
                LocationId: locationId,
                SiteId: siteId,
                TotalDeviceOrUser: totalDeviceOrUser,
                ContractPeriodId: contractPeriodId,
                BillingPeriodId: billingPeriodId,
                ItemServiceDeliveryId: itemServiceDeliveryId,
                CurrentVendorId: currentVendorId,
                PriceValidity: priceValidity,
                DeployLocation: deployLocation,
                Remarks: remarks
            };

            PageMethods.SaveQuotation(Quotation, QuotationItemDetails, QuotationServiceDetails, QuotationDeletedItemDetails, QuotationDeletedServiceDetails, QuotationDetail, DeletedQuotationDetail, DeletedQuotationDiscountDetail, OnSaveQuotationSucceed, OnSaveQuotationFailed);

            return false;
        }

        function OnSaveQuotationSucceed(result) {
            if (result.IsSuccess) {
                if (isClose)
                    parent.CloseDialog();
                if (typeof parent.ShowAlert === "function")
                    parent.ShowAlert(result.AlertMessage);
                //if (typeof parent.GridPaging === "function") {
                //    parent.GridPaging($(parent.document.getElementsByClassName("childDivSection").find("li active").text()), 1);
                //}
                $(window.parent.document.getElementById("btnSearch")).trigger('click');
                ClearForm();
            }
            else {
                parent.ShowAlert(result.AlertMessage);
            }

            //CommonHelper.SpinnerClose();
            return false;
        }
        function OnSaveQuotationFailed(error) {
            //CommonHelper.SpinnerClose();
            parent.ShowAlert(error);
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

        function ClearForm() {
            $("#form1")[0].reset();

            $("#ContentPlaceHolder1_hfCompanyId").val("0");
            $("#ContentPlaceHolder1_hfQuotationId").val("0");
            $("#ContentPlaceHolder1_hfQDealId").val("0");
            $("#ContentPlaceHolder1_hfCompanySiteId").val("0");
            $("#ContentPlaceHolder1_hfItemId").val("0");

            //$("#ContentPlaceHolder1_lblEMail").text("");
            //$("#ContentPlaceHolder1_lblBusinessContact").text("");
            //$("#ContentPlaceHolder1_lblContactNo").text("");
            //$("#ContentPlaceHolder1_lblCompanyAddress").val("");
            //$("#ContentPlaceHolder1_lblDesignation").text("");

            $("#QuotationItemInformation tbody").html("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text("");

            $("#QuotationServicenformation tbody").html("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text("");
            isClose = false;
            QuotationItemDetails = new Array();
            QuotationDeletedItemDetails = new Array();

            QuotationServiceDetails = new Array();
            QuotationDeletedServiceDetails = new Array();
            isLoadFromCostAnalysis = false;

            DiscountTable.clear().draw();
            FinalQuotationMasterTable.clear().draw();
            FinalQuotationMaster = null;
            QuotationDetail = new Array();
            DeletedQuotationDetail = [];
            DeletedQuotationDiscountDetail = [];
            TempQuotationDetail = null;
            QuotationDiscountDetails = new Array();
            $("#dvChkCostAnalysis").show();
        }

        function Clear() {

            //$("#ContentPlaceHolder1_hfCompanyId").val("0");
            //$("#ContentPlaceHolder1_hfQuotationId").val("0");
            //$("#ContentPlaceHolder1_hfQDealId").val("0");
            //$("#ContentPlaceHolder1_hfCompanySiteId").val("0");
            $("#ContentPlaceHolder1_hfItemId").val("0");

            //$("#ContentPlaceHolder1_lblEMail").text("");
            //$("#ContentPlaceHolder1_lblBusinessContact").text("");
            //$("#ContentPlaceHolder1_lblContactNo").text("");
            //$("#ContentPlaceHolder1_lblCompanyAddress").val("");
            //$("#ContentPlaceHolder1_lblDesignation").text("");

            $("#QuotationItemInformation tbody").html("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text("");

            $("#QuotationServicenformation tbody").html("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text("");
            isClose = false;
            QuotationItemDetails = new Array();
            QuotationDeletedItemDetails = new Array();

            QuotationServiceDetails = new Array();
            QuotationDeletedServiceDetails = new Array();
            isLoadFromCostAnalysis = false;

            DiscountTable.clear().draw();
            FinalQuotationMasterTable.clear().draw();
            FinalQuotationMaster = null;
            QuotationDetail = new Array();
            DeletedQuotationDetail = [];
            DeletedQuotationDiscountDetail = [];
            TempQuotationDetail = null;
            QuotationDiscountDetails = new Array();
        }

        function SaveNClose() {
            isClose = true;
            ValidationNPreprocess();
            return false;
        }

        function SaveNContinue() {
            flagValidation = false;
            ValidationNPreprocess();
            if (!flagValidation) {
                var dealId = $.trim(CommonHelper.GetParameterByName("did"));
                var companyId = $.trim(CommonHelper.GetParameterByName("cid"));
                var contactId = $.trim(CommonHelper.GetParameterByName("conid"));

                LoadSetupData(dealId, companyId, contactId);
            }
            return false;
        }

        function CostAnalysis() {

            PageMethods.SaveItemNServiceForCostAnalysis(QuotationItemDetails, QuotationServiceDetails, OnSuccessSaving, OnFailedSaving);

            return false;
        }

        function OnSuccessSaving(result) {

            var iframeid = 'frmPrintForCostAnalysis';
            var url = "./CostAnalysis.aspx?isfrmqtn=1";
            document.getElementById(iframeid).src = url;

            $("#CreateCostAnalysis").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Cost Analysis",
                show: 'slide'
            });
        }

        function OnFailedSaving(error) {
            parent.ShowAlert(error);
        }

        function LaodQuotaionItemAndServiceFromCostAnalysis(id) {
            $("#CostAnalysisSearch").dialog('close');
            isLoadFromCostAnalysis = true;
            $("#QuotationItemInformation tbody").html("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text("");
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text("");

            $("#QuotationServicenformation tbody").html("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text("");
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text("");
            isClose = false;
            QuotationItemDetails = new Array();
            QuotationDeletedItemDetails = new Array();

            QuotationServiceDetails = new Array();
            QuotationDeletedServiceDetails = new Array();

            PageMethods.LoadCostAnalysis(id, OnLoadQuotationSucceeded, OnLoadQuotationFailed);
            return false;
        }

        function GridPaging(pageNumber, isCurrentOrPreviousPage) {

            var gridRecordsCount = $("#tblCostAnalysis tbody tr").length;
            var name, fromDate, toDate;

            name = $("#ContentPlaceHolder1_txtSearchName").val();
            fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();

            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/CostAnalysisInformation.aspx/GetCostAnalysisWithPagination',
                data: JSON.stringify({ name: name, fromDate: fromDate, toDate: toDate, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, isCurrentOrPreviousPage: isCurrentOrPreviousPage }),
                dataType: "json",
                success: function (data) {
                    OnSearchSuccess(data.d);
                },
                error: function (error) {
                    OnSearchFail(error.d);
                }
            });
            //PageMethods.GetCostAnalysisWithPagination(name, fromDate, toDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchSuccess, OnSearchFail);
            return false;
        }

        function OnSearchSuccess(result) {

            $("#tblCostAnalysis tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#tblCostAnalysis tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#tblCostAnalysis tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;cursor:pointer;\" onclick=\"LaodQuotaionItemAndServiceFromCostAnalysis(" + gridObject.Id + ")\" >";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;cursor:pointer; \" onclick=\"LaodQuotaionItemAndServiceFromCostAnalysis(" + gridObject.Id + ")\" >";
                }

                tr += "<td align='left' style=\"width:70%; \">" + gridObject.Name + "</td>";
                tr += "<td align='left' style=\"width:15%; \">" + CommonHelper.DateFromStringToDisplay(gridObject.CreatedDate, innBoarDateFormat) + "</td>";

                tr += "</tr>";

                $("#tblCostAnalysis tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }

        function OnSearchFail(error) {
            toastr.error(error.get_message());
        }

        function ShowAlert(Message) {
            parent.ShowAlert(Message);
        }

        function CloseDialog() {
            $("#CreateCostAnalysis").dialog('close');
        }

        function LoadService(control) {
            var serviceType = $(control).val();
            var isDetails = $("#ContentPlaceHolder1_ddlDiscountTo").val() == "0";
            if (serviceType == "0")
                ClearDiscount();
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
                url: '../SalesAndMarketing/frmSMQuotation.aspx/GetTypeList',
                data: JSON.stringify({ serviceType }),
                dataType: "json",
                async: false,
                success: function (result) {
                    if (result.d.length > 0) {
                        DiscountTable.clear();
                        DiscountTable.rows.add(result.d);
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
            let QuotationId = parseInt($("#ContentPlaceHolder1_hfQuotationId").val());
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

            let QuotationDetailsId = 0;
            if (TempQuotationDetail != null)
                QuotationDetailsId = TempQuotationDetail.QuotationDetailsId;
            if (ServiceType != "Item" && ServiceType != "Service") {
                let Id = 0, Type = "", TypeId = 0, DiscountType = "", DiscountAmount = 0.00, DiscountAmountUSD = 0.00;
                if (!IsDiscountForAll) {
                    var invalid = false;
                    DiscountTable.rows().every(function (rowIdx, tableLoop, rowLoop) {
                        var data = this.data();
                        var isChecked = DiscountTable.cell(rowIdx, 0).nodes().to$().find('input').is(':checked');
                        if (isChecked) {
                            Id = data.Id;
                            Type = data.Type;
                            TypeId = data.TypeId;
                            DiscountType = DiscountTable.cell(rowIdx, 2).nodes().to$().find('select').val();
                            if (DiscountType == "0") {
                                DiscountTable.cell(rowIdx, 2).nodes().to$().find('select').focus();
                                toastr.warning("Select Discount Type.");
                                invalid = true;
                                return false;
                            }
                            DiscountAmount = parseFloat(DiscountTable.cell(rowIdx, 3).nodes().to$().find('input').val());

                            if (!DiscountAmount) {
                                DiscountTable.cell(rowIdx, 3).nodes().to$().find('input').focus();
                                toastr.warning("Enter Discount Amount.");
                                invalid = true;
                                return false;
                            }
                            if (DiscountType == "Fixed")
                                DiscountAmountUSD = parseFloat(DiscountTable.cell(rowIdx, 4).nodes().to$().find('input').val());


                            QuotationDiscountDetails.push({
                                Id, SMQuotationDetailsId: QuotationDetailsId, OutLetId, Type, TypeId, DiscountType, DiscountAmount, DiscountAmountUSD
                            });
                        }
                        else {
                            if (data.Id > 0)
                                DeletedQuotationDiscountDetail.push(data.Id);
                        }
                        Id = 0, Type = "", TypeId = 0, DiscountType = "", DiscountAmount = 0.00, DiscountAmountUSD = 0.00;
                    });
                    if (invalid)
                        return false;
                    if (QuotationDiscountDetails.length == 0) {
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
                var quotationDetail = {
                    QuotationDetailsId,
                    ItemType: ServiceType,
                    QuotationId,
                    IsDiscountForAll,
                    DiscountType,
                    DiscountAmount,
                    DiscountAmountUSD,
                    QuotationDiscountDetails
                }
                var isExist = false, inValid = false;
                QuotationDetail.map(i => {
                    //let previousQotationDetailRow = _.findWhere(QuotationDetail, { ItemType: ServiceType });
                    if (i.ItemType == ServiceType) {
                        isExist = true;
                        if (TempQuotationDetail == null) {
                            if (IsDiscountForAll || i.IsDiscountForAll || (OutLetId > 0 && i.QuotationDiscountDetails.filter(r => r.OutLetId == OutLetId).length > 0)) {
                                toastr.warning("Same Type Service is Already Added. Please Edit.");
                                inValid = true;
                                return false;
                            }
                        }
                        i.ItemType = ServiceType;
                        i.QuotationId = QuotationId;
                        i.IsDiscountForAll = IsDiscountForAll;
                        i.DiscountType = DiscountType;
                        i.DiscountAmount = DiscountAmount;
                        i.DiscountAmountUSD = DiscountAmountUSD;

                        quotationDetail.QuotationDiscountDetails.map(r => { r.SMQuotationDetailsId = i.QuotationDetailsId; return r; });

                        i.QuotationDiscountDetails = i.QuotationDiscountDetails.filter(r => r.OutLetId != OutLetId);
                        i.QuotationDiscountDetails.push.apply(i.QuotationDiscountDetails, quotationDetail.QuotationDiscountDetails);


                        return i;
                    }
                });
                if (inValid)
                    return false;
                if (!isExist)
                    QuotationDetail.push(quotationDetail);

                QuotationDiscountDetails = [];
            }
            var QuotationMasterRow = {
                ServiceType,
                ServiceTypeColumn,
                OutLetId,
                QuotationDetailsId
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
            TempQuotationDetail = null;
            DiscountTable.clear().draw();
        }

        function ShowHideDiscountAmount(control) {
            let value = $(control).val();

            if (value == "Fixed")
                $(control).parent().parent().find("td:eq(4) input").show();
            else
                $(control).parent().parent().find("td:eq(4) input").hide();
        }

        function EditDiscountDetails(column, QuotationDetailsId, OutLetId, ServiceType) {
            if (!confirm("Do you want to edit?"))
                return true;
            var row = $(column).parents('tr');

            FinalQuotationMasterTable.row(row).remove().draw(false);

            $("#ContentPlaceHolder1_ddlServiceItemType").val(ServiceType).trigger('change');
            if (ServiceType == "Item") {
                $("#QuotationItemInformation tbody").html("");
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text("");
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text("");
                $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text("");

                LoadItemTable();
            }
            else if (ServiceType == "Service") {
                $("#QuotationServicenformation tbody").html("");
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text("");
                $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text("");

                LoadServiceTable();
            }
            else {

                var QuotationDetailRow = null;
                if (QuotationDetailsId > 0)
                    QuotationDetailRow = _.findWhere(QuotationDetail, { QuotationDetailsId });
                else
                    QuotationDetailRow = _.findWhere(QuotationDetail, { ItemType: ServiceType });
                TempQuotationDetail = QuotationDetailRow;

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

                let QuotationDiscountDetailsList = QuotationDetailRow.QuotationDiscountDetails.filter(i => i.OutLetId == OutLetId);
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
                        }
                        DiscountTable.row(index).data(rowData).draw();
                        DiscountTable.cell(index, 0).nodes().to$().find('input').prop('checked', true);
                    })
                }
            }
            return true;
        }

        function DeleteDiscountDetails(column, QuotationDetailsId, OutLetId, ServiceType) {
            if (!confirm("Want to Delete?"))
                return true;
            var row = $(column).parents('tr');
            FinalQuotationMasterTable.row(row).remove().draw(false);
            if (ServiceType == "Item") {
                QuotationDeletedItemDetails.push.apply(QuotationDeletedItemDetails, QuotationItemDetails);
            }
            else if (ServiceType == "Service")
                QuotationDeletedServiceDetails.push.apply(QuotationDeletedServiceDetails, QuotationServiceDetails);
            else if (QuotationDetailsId > 0) {
                if (OutLetId > 0) {
                    var quotaionDetail = QuotationDetail.filter(i => i.ItemType == ServiceType);
                    var deletedDiscountDetailId = quotaionDetail[0].QuotationDiscountDetails.filter(i => i.Id > 0 && i.OutLetId == OutLetId).map(i => { return i.Id });
                    DeletedQuotationDiscountDetail.push.apply(DeletedQuotationDiscountDetail, deletedDiscountDetailId);
                }
                else
                    DeletedQuotationDetail.push(QuotationDetailsId);

            }
            else
                QuotationDetail = QuotationDetail.filter(i => i.ItemType != ServiceType);
        }

        function LoadItemTable() {

            var tr = "", deleteLink = "<a href=\"javascript:void()\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            var isCopy = $.trim(CommonHelper.GetParameterByName("isCopy"));
            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
            $.each(QuotationItemDetails, function (index, itm) {

                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"display:none;\">" + (isCopy != "1" ? itm.QuotationDetailsId : "0") + "</td>";
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
            //var QuotationItemList = QuotationItemDetails;
            //QuotationItemDetails = new Array();
            //$.each(QuotationItemList, function (index, itm) {
            //    $("#txtItemName").val(itm.ItemName);
            //    $("#ContentPlaceHolder1_txtItemUnit").val(itm.Quantity);
            //    SearchItem = {
            //        label: itm.ItemName,
            //        value: itm.ItemId,

            //        Name: itm.ItemName,
            //        CategoryId: itm.CategoryId,
            //        ItemId: itm.ItemId,
            //        StockById: itm.StockBy,
            //        UnitPriceLocal: itm.UnitPrice,
            //        UnitHead: itm.HeadName
            //    };

            //    AddItem();
            //});

        }

        function LoadServiceTable() {

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

                tr += "<td align='left' style=\"display:none;\">" + itm.QuotationDetailsId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ItemId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.StockById + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.Quantity + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServicePackageId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServiceBandWidthId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServiceTypeId + "</td>";

                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + (itm.ServiceType || serviceType) + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.ItemName + "</td>";
                //tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + (itm.PackageName || packageName) + "</td>";
                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + (downLink) + "</td>";

                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" +
                //    " <input type=\"text\" disabled='disabled' id='itm" + itm.ItemId + "' value='" + upLink + "' class=\"form-control\" />" +
                //    "</td>";

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

            <%--var QuotationServiceList = QuotationServiceDetails;
            QuotationServiceDetails = new Array();
            $.each(QuotationServiceList, function (index, m) {
                SearchService = {
                    label: m.ItemName,
                    value: m.ItemId,

                    Name: m.ItemName,
                    CategoryId: m.CategoryId,
                    ItemId: m.ItemId,
                    StockById: m.StockBy,
                    UnitHead: m.StockBy
                };
                $("#txtServiceItem").val(m.ItemName);

                $("#ContentPlaceHolder1_ddlServiceStockBy").val(m.StockBy);
                $("#<%=hfItemServiceId.ClientID %>").val(m.ItemId);
                $("#ContentPlaceHolder1_txtServiceQuantity").val(m.Quantity);
                var contactPeriodValue = m.TotalPrice / m.UnitPrice / m.Quantity;
                var cp = _.findWhere(ContactPeriod, { ContractPeriodValue: parseInt(contactPeriodValue) });
                
                $("#ContentPlaceHolder1_ddlContractPeriod").val(cp.ContractPeriodId);
                $("#ContentPlaceHolder1_ddlServiceStockBy").val(m.StockById);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: './frmSMQuotation.aspx/GetPackageByItemBy',
                    data: "{'itemId':'" + m.ItemId + "'}",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (!OnLoadPackageSucceeded(data.d))
                            $("#ContentPlaceHolder1_ddlPackageName").val(m.ServicePackageId).trigger('change');
                    },
                    error: function (result) {
                        OnLoadPackageFailed(result);
                    }
                });

                $("#ContentPlaceHolder1_txtUnitPrice").val(m.UnitPrice);
                AddServiceItem();
            });--%>
        }

        function LoadFinalQuotationMasterTable() {

            var QuotationMasterRow = new Array();
            var groupedDetailList = _(QuotationDetail).groupBy('ItemType');
            groupedDetailList = _(groupedDetailList).map(function (detailData, key) {
                return {
                    ItemType: detailData[0].ItemType,
                    QuotationDetailsId: detailData[0].QuotationDetailsId,
                    IsDiscountForAll: detailData[0].IsDiscountForAll,
                    QuotationDiscountDetails: detailData[0].QuotationDiscountDetails
                }
            });
            $.each(groupedDetailList, function (index, value) {
                var ServiceTypeColumn = "";
                var ServiceType = value.ItemType;
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

                    var groupedList = _(value.QuotationDiscountDetails).groupBy('OutLetId');
                    groupedList = _(groupedList).map(function (value, key) {
                        return {
                            OutLetId: value[0].OutLetId,
                            OutLetName: value[0].OutLetName
                        }
                    });
                    $.each(groupedList, function (arrayIndex, data) {
                        QuotationMasterRow.push({
                            ServiceType: value.ItemType,
                            ServiceTypeColumn: (ServiceTypeColumn + data.OutLetName),
                            OutLetId: data.OutLetId,
                            QuotationDetailsId: value.QuotationDetailsId
                        });
                    });

                }
                else {
                    QuotationMasterRow.push({
                        ServiceType: value.ItemType,
                        ServiceTypeColumn,
                        OutLetId: 0,
                        QuotationDetailsId: value.QuotationDetailsId
                    });
                }

            });

            QuotationDetail = QuotationDetail.filter(i => i.ItemType != "Item" && i.ItemType != "Service");

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
    </script>
    <asp:HiddenField ID="hfIsQuotationCreateFromSiteServeyFeedback" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfQuotationId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfQDealId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfContactId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfSCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanySiteId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfItemServiceId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfDownloadBandwidth" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfContactPeriod" runat="server" Value=""></asp:HiddenField>

    <div id="CostAnalysisSearch" style="display: none;">
        <div class="panel panel-default">
            <div class="panel-heading">
                Cost Analysis
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Name</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">From Date</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">To Date</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" class="TransactionalButton btn btn-primary btn-sm" value="Search" onclick="GridPaging(1, 1)" />
                            <input type="button" class="btn btn-primary btn-sm" value="Clear" onclick="Clear()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="SearchPanel" class="panel panel-default">
            <div class="panel-heading">Search Information</div>
            <div class="panel-body">
                <table id='tblCostAnalysis' class="table table-bordered table-condensed table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <td style="width: 70%;">Name
                            </td>
                            <td style="width: 15%;">Date
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
    <div id="CreateCostAnalysis" style="display: none;">
        <iframe id="frmPrintForCostAnalysis" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">Proposal Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-4  text-right">
                                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                                <asp:Label ID="Label10" runat="server" class="control-label required-field" Text="Proposal Date"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtProposalDate" runat="server" class="form-control" TabIndex="1" Text=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label11" runat="server" class="control-label required-field" Text="Proposal For"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlServiceType" runat="server" class="form-control" TabIndex="2">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="lblCompanyOrContactName" runat="server" class="control-label required-field" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtCompanyOrContactName" runat="server" class="form-control"></asp:TextBox>
                                <asp:DropDownList Style="display: none;" ID="ddlIndependentContact" runat="server" class="form-control" TabIndex="3"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label26" runat="server" class="control-label required-field" Text="Deal Name"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlDealName" runat="server" class="form-control" TabIndex="4">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="dvDeviceNUser" runat="server">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label16" runat="server" class="control-label required-field" Text="Device/User(Qtn)"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtDeviceOrUser" runat="server" class="form-control quantityint" TabIndex="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label18" runat="server" class="control-label required-field" Text="Contract Period"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlContractPeriod" runat="server" CssClass="form-control" TabIndex="6"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label13" runat="server" class="control-label required-field" Text="Billing Period"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlBillingPeriod" runat="server" CssClass="form-control" TabIndex="7"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="dvDelivery" runat="server">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label15" runat="server" class="control-label required-field" Text="Delivery"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlDeliveryBy" runat="server" CssClass="form-control" TabIndex="8"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label25" runat="server" class="control-label" Text="Price Validity"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtPriceValidity" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label27" runat="server" class="control-label" Text="Where to Deploy"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtWhereToDeploy" runat="server" CssClass="form-control" TabIndex="10"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label17" runat="server" class="control-label" Text="Present Vendor"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlCurrentVendor" runat="server" CssClass="form-control" TabIndex="11"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label19" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="dvCustomer" class="col-md-6">
            <div id="pnlCompany" class="panel panel-default">
                <div class="panel-heading">Company Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-4  text-right">
                                <asp:HiddenField ID="hfSalesCallId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCompany" runat="server" class="control-label required-field" Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <input id="txtCompanySearch" type="text" class="form-control" name="cmpName" />
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlCompany" TabIndex="1" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label38" runat="server" class="control-label" Text="Company Type"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtCompanyType" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label39" runat="server" class="control-label" Text="Industry"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtIndustry" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label40" runat="server" class="control-label" Text="Ownership"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtOwnership" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label9" runat="server" class="control-label" Text="Phone"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblContactNo" runat="server" class="form-control" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="E-mail"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblEMail" runat="server" class="form-control" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label41" runat="server" class="control-label" Text="Website"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblWebsite" runat="server" class="form-control"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label42" runat="server" class="control-label" Text="Life Cycle Stage"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblLifeCycleStage" runat="server" class="form-control" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Address"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox TextMode="MultiLine" ID="lblCompanyAddress" runat="server" class="form-control" Text=""></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="pnlContact" style="display: none" class="panel panel-default">
                <div class="panel-heading">Contact Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-4  text-right">
                                <asp:Label ID="Label28" runat="server" class="control-label required-field" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtContactName" TabIndex="1" CssClass="form-control" runat="server">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label29" runat="server" class="control-label" Text="Title"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblContactTitle" runat="server" class="form-control"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label30" runat="server" class="control-label" Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblDepartment" runat="server" class="form-control"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label32" runat="server" class="control-label" Text="Mobile(Personal)"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblContactMobile" runat="server" class="form-control" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label31" runat="server" class="control-label" Text="Phone(Personal)"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblContactPhone" runat="server" class="form-control"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label34" runat="server" class="control-label" Text="E-mail(Personal)"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblConatctEmail" runat="server" class="form-control" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label36" runat="server" class="control-label" Text="Personal Address"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblPersonalAddress" runat="server" class="form-control" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4 text-right">
                                <asp:Label ID="Label33" runat="server" class="control-label" Text="Life Cycle Stage"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:Label ID="lblContactLifeCycleStage" runat="server" class="form-control" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="form-horizontal">
        <div class="form-group" id="dvChkCostAnalysis">
            <div class="col-md-offset-2 col-md-4">
                <input id="chkIsCreateFromCostAnalysis" type="checkbox" onchange="ShowOrHideCostAnalysis(this)" />
                &nbsp;<label class="control-label">Is Create From CostAnalysis ?</label>
            </div>
        </div>
    </div>
    <div class="form-horizontal">
        <div class="form-group">
            <div class="col-md-2 text-right">
                <label class="control-label">Service Type</label>
            </div>
            <div class="col-md-4">
                <asp:DropDownList ID="ddlServiceItemType" TabIndex="1" CssClass="form-control" runat="server" onchange="LoadService(this)">
                </asp:DropDownList>
            </div>
            <div id="dvDiscountTo" style="display: none;">
                <div class="col-md-2 text-right">
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
            <div class="col-md-2 text-right">
                <label class="control-label">Discount Type</label>
            </div>
            <div class="col-md-4">
                <asp:DropDownList ID="ddlAllDiscountType" TabIndex="1" CssClass="form-control" runat="server" onchange="ChangeDiscountType(this)">
                    <asp:ListItem Value="Fixed">Fixed</asp:ListItem>
                    <asp:ListItem Value="Percentage">Percentage</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2 text-right">
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
            <div class="col-md-2 text-right">
                <label class="control-label">Restaurant Name</label>
            </div>
            <div class="col-md-4">
                <asp:DropDownList runat="server" ID="ddlRestaurant" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group" id="BanquetDropDown" style="display: none;">
            <div class="col-md-2 text-right">
                <label class="control-label">Banquet Name</label>
            </div>
            <div class="col-md-4">
                <asp:DropDownList runat="server" ID="ddlBanquet" CssClass="form-control"></asp:DropDownList>
            </div>
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
                <div class="form-group" style="display:none;">
                    <div class="col-md-2 label-align">
                        <asp:Label ID="Label6" runat="server" class="control-label" Text="Package Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlPackageName" runat="server" CssClass="form-control" TabIndex="22">
                        </asp:DropDownList>
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
                    <div class="col-md-2 label-align" style="display:none;">
                        <asp:Label ID="Label22" runat="server" class="control-label" Text="Service Type"></asp:Label>
                    </div>
                    <div class="col-md-4" style="display:none;">
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
                                    <%--<th style="width: 10%;">Service</th>--%>
                                    <th style="width: 15%;">Prduct Details</th>
                                    <%--<th style="width: 15%;">Package/s</th>--%>
                                    <%--<th style="width: 10%;">Downlink</th>--%>
                                    <%--<th style="width: 10%;">Uplink</th>--%>
                                    <th style="width: 15%;">Price/ Month</th>
                                    <th style="width: 15%;">Price/ 3 Month</th>
                                    <th style="width: 10%;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="1" style="width: 45%; padding-right: 5px; text-align: right;">Total:</td>
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
    <div class="form-horizontal">
        <div class="form-group">
            <div class="col-md-2 text-right">
                <button type="button" id="btnAddDiscount" tabindex="17" class="TransactionalButton btn btn-primary btn-sm" onclick="return AddDiscount()">
                    Add to Master</button>
            </div>
        </div>
    </div>
    <div class="form-horizontal">
        <table id="tblQuotationDetails" class="table table-condensed table-bordered table-responsive">
        </table>
        &nbsp;
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:Button ID="btnSave" runat="server" Text="Save & Close" CssClass="TransactionalButton btn btn-primary btn-sm"
                TabIndex="27" OnClick="btnSave_Click" OnClientClick="javascript:return SaveNClose();" />
            <input type="button" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="Clear()" />
            <input tabindex="29" id="btnSaveNContinue" type="button" value="Save & Continue" onclick="javascript: return SaveNContinue();" class="TransactionalButton btn btn-primary btn-sm" />
            <input id="btnCostAnalysis" type="button" class="TransactionalButton btn btn-primary btn-sm" style="display:none;" value="Cost Analysis" onclick="CostAnalysis()" />
        </div>
    </div>
</asp:Content>
