<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmPMRequisition.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmPMRequisition" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        var editedItem = "";
        var deleteDbItem = [], editDbItem = [], newlyAddedItem = [];
        var queryRqId = "";


        var ItemList = new Array();


        $(document).ready(function () {
            debugger;

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").change(function () {
                $("#ContentPlaceHolder1_companyProjectUserControl_hfDropdownFirstValue").val("select");
            });
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").change(function () {
                $("#ContentPlaceHolder1_companyProjectUserControl2hfDropdownFirstValue").val("all");
            });
            
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product Requisition</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            CommonHelper.ApplyDecimalValidation();
            $("#myTabs").tabs();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlCostCentre").change(function myfunction() {
                if ($(this).val() != null && $(this).val() != "0") {

                    LoadLocationFrom($(this).val());
                    LoadCategoryFromCC($(this).val());
                    $("#categoryDiv").show("slow");
                }
                else {
                    var control = $('#ContentPlaceHolder1_ddlLocationFrom');
                    control.empty();
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                    //$("#categoryDiv").hide("slow");
                }
            });
            $("#ContentPlaceHolder1_ddlCategory").change(function myfunction() {
                PerformClearRequisitionByCategoryChange();
            });
            //$("#ContentPlaceHolder1_ddlCostCentre").val('0').trigger('change');

            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_txtItemName").autocomplete({
                minLength: 3,
                source: function (request, response) {
                    var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                    var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
                    var costCenterId = $("#ContentPlaceHolder1_ddlCostCentre").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../PurchaseManagment/frmPMRequisition.aspx/ItemSearch',
                        //data: "{'searchTerm':'" + request.term + "', 'companyId:'" + companyId + "', 'projectId:'" + projectId + "', 'costCenterId':'" + costCenterId + "', 'categoryId':'" + categoryId + "'}",
                        data: JSON.stringify({ searchTerm: request.term, companyId: companyId, projectId: projectId, costCenterId: costCenterId, categoryId: categoryId }),
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    CategoryId: m.CategoryId,
                                    StockById: m.StockBy
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
                    $("#ContentPlaceHolder1_hfProductId").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfCategoryId").val(ui.item.CategoryId);
                    //LoadCurrentStockQuantity(ui.item.value);
                    PageMethods.LoadRelatedStockBy(ui.item.StockById, OnLoadStockBySucceeded, OnLoadStockByFailed);

                    if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                        GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Color');
                        GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Size');
                        GetInvItemAttributeByItemIdAndAttributeType(ui.item.value, 'Style');

                        GetInvItemStockInfoByItemAndAttributeId();
                    }
                    else {
                        LoadCurrentStockQuantity(ui.item.value);
                    }
                }
            });


            $('#ContentPlaceHolder1_ddlSizeAttribute').change(function () {
                GetInvItemStockInfoByItemAndAttributeId();
            });
                
            $('#ContentPlaceHolder1_ddlStyleAttribute').change(function () {
                GetInvItemStockInfoByItemAndAttributeId();
            });
            
            $('#ContentPlaceHolder1_ddlColorAttribute').change(function () {
                GetInvItemStockInfoByItemAndAttributeId();
            });

            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                $("#AttributeDiv").show();
                $("#cId").show();
                $("#sId").show();
                $("#stId").show();
                $("#cIdd").show();
                $("#sIdd").show();
                $("#stIdd").show();
            }
            else {
                $("#AttributeDiv").hide();
                $("#cId").hide();
                $("#sId").hide();
                $("#stId").hide();
                $("#cIdd").hide();
                $("#sIdd").hide();
                $("#stIdd").hide();
            }
        });
        
        function OnLoadAttributeStyleSucceeded(result) {
            var list = result;
            var ddlStyleAttributeId = '<%=ddlStyleAttribute.ClientID%>';
            var control = $('#' + ddlStyleAttributeId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.attr("disabled", false);
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.attr("disabled", true);
                }
            }
            return false;
        }
        function OnLoadAttributeStyleFailed(error) {
        }
        function OnLoadAttributeSizeSucceeded(result) {
            var list = result;
            var ddlSizeAttributeId = '<%=ddlSizeAttribute.ClientID%>';
            var control = $('#' + ddlSizeAttributeId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.attr("disabled", false);
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.attr("disabled", true);
                }
            }
            return false;
        }
        function OnLoadAttributeSizeFailed(error) {
        }

        function OnLoadAttributeColorSucceeded(result) {
            var list = result;
            var ddlColorAttributeId = '<%=ddlColorAttribute.ClientID%>';
            var control = $('#' + ddlColorAttributeId);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.attr("disabled", false);
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.attr("disabled", true);
                }
            }
            return false;
        }
        function OnLoadAttributeColorFailed(error) {
        }
        
        $(document).ready(function () {
            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'
            DivShowHideFunction();
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

            $("#<%=ddlDateType.ClientID %>").change(function () {
                DivShowHideFunction();
            });

            var txtReceivedByDate = '<%=txtReceivedByDate.ClientID%>'
            $('#' + txtReceivedByDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: new Date()
            });

            $("#btnAddRequsition").click(function () {
                if ($("#ContentPlaceHolder1_ddlRequisitionTo").val() == "0") {
                    toastr.warning("Please add requisition To.");
                    return false;
                }

                var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
                var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
                if (companyId == "0") {
                    toastr.warning("Please Select Company.");
                    $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                    return false;
                }
                else if (projectId == "0") {
                    toastr.warning("Please Select Project.");
                    $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                    return false;
                }


                var itemId = $("#ContentPlaceHolder1_hfProductId").val();
                var categoryId = $("#ContentPlaceHolder1_hfCategoryId").val();
                var costCentreId = $("#ContentPlaceHolder1_ddlCostCentre").val();
                var toCostCenterId = $("#ContentPlaceHolder1_ddlRequisitionTo").val();
                var stockById = $("#ContentPlaceHolder1_ddlStockBy").val();
                var quantity = $("#ContentPlaceHolder1_txtQuantity").val();
                var remarks = $("#ContentPlaceHolder1_tbItemRemarks").val();

                CommonHelper.ExactMatch();
                if (costCentreId == "0") {
                    toastr.warning("Please select a cost centre.");
                    return;
                }

                if (costCentreId == toCostCenterId) {
                    toastr.warning("Requisition From and Requisition To should be Different.");
                    return;
                }

                if (itemId == "0") {
                    toastr.warning("Please select an item.");
                    return;
                }
                else if (stockById == "0" || stockById == null) {
                    toastr.warning("Please select a stock by.");
                    return;
                }
                else if ($.trim(quantity) == "" || parseFloat($.trim(quantity)) == 0) {
                    toastr.warning("Please give quantity.");
                    return;
                }
                else if (CommonHelper.IsDecimal($.trim(quantity)) == false) {
                    toastr.warning("Please give valid quantity.");
                    return;
                }

                var itemName = $("#ContentPlaceHolder1_txtItemName").val();
                var costCentreName = $("#ContentPlaceHolder1_ddlCostCentre option:selected").text();
                var stockBy = $("#ContentPlaceHolder1_ddlStockBy option:selected").text();

                debugger; 
                var colorddlLength = $('#ContentPlaceHolder1_ddlColorAttribute > option').length; 
                var sizeddlLength = $('#ContentPlaceHolder1_ddlSizeAttribute > option').length;
                var styleddlLength = $('#ContentPlaceHolder1_ddlStyleAttribute > option').length;
                var colorId = 0;
                if (colorddlLength > 0) {
                    colorId = $("#ContentPlaceHolder1_ddlColorAttribute option:selected").val();
                }
                var colorText = $("#ContentPlaceHolder1_ddlColorAttribute option:selected").text();
                var sizeId = 0;
                if (sizeddlLength > 0) {
                    sizeId = $("#ContentPlaceHolder1_ddlSizeAttribute option:selected").val();
                }
                var sizeText = $("#ContentPlaceHolder1_ddlSizeAttribute option:selected").text();
                var styleId = 0;
                if (styleddlLength > 0) {
                    styleId = $("#ContentPlaceHolder1_ddlStyleAttribute option:selected").val();
                }
                var styleText = $("#ContentPlaceHolder1_ddlStyleAttribute option:selected").text();
                
                var duplicateItemId = 0;
                var duplicatecolorId = 0;
                var duplicatesizeId = 0;
                var duplicatestyleId = 0;
                var requsitionId = "0", requsitionDetailsId = "0";

                requsitionId = $("#ContentPlaceHolder1_hfRequisitionId").val();

                var IntItemId = parseInt(itemId, 10);

                var IntColorId = parseInt(colorId, 10);
                var IntSizeId = parseInt(sizeId, 10);
                var IntStyleId = parseInt(styleId, 10);

                debugger;

                if (editedItem != "") {
                    var ItemId = parseInt($(editedItem).find("td:eq(13)").text(), 10);
                    var ColorId = parseInt($(editedItem).find("td:eq(8)").text(), 10);
                    var SizeId = parseInt($(editedItem).find("td:eq(9)").text(), 10);
                    var StyleId = parseInt($(editedItem).find("td:eq(10)").text(), 10);

                    if (ItemId != IntItemId || ColorId != IntColorId || SizeId != IntSizeId || StyleId != IntStyleId) {
                        var CostCenterOBJ = _.findWhere(ItemList, { ItemId: IntItemId, ColorId: IntColorId, SizeId: IntSizeId, StyleId: IntStyleId });
                        if (CostCenterOBJ != null) {
                            toastr.warning("Same Item Already Added.");
                            return false;
                        }
                    }
                }
                else {
                    var CostCenterOBJ = _.findWhere(ItemList, { ItemId: IntItemId, ColorId: IntColorId, SizeId: IntSizeId, StyleId: IntStyleId });
                    if (CostCenterOBJ != null) {
                        toastr.warning("Same Item Already Added.");
                        return false;
                    }
                }

                if (requsitionId != "0" && editedItem != "") {
                    requsitionDetailsId = $(editedItem).find("td:eq(11)").text();
                    EditItem(requsitionId, requsitionDetailsId, categoryId, itemId, costCentreId, stockById, quantity, itemName, stockBy, remarks, toCostCenterId, colorId, colorText, sizeId, sizeText, styleId, styleText);
                    return;
                }
                else if (requsitionId == "0" && editedItem != "") {
                    requsitionDetailsId = $(editedItem).find("td:eq(11)").text();
                    EditItem(requsitionId, requsitionDetailsId, categoryId, itemId, costCentreId, stockById, quantity, itemName, stockBy, remarks, toCostCenterId, colorId, colorText, sizeId, sizeText, styleId, styleText);
                    return;
                }

                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", true);
                $("#ContentPlaceHolder1_ddlRequisitionTo").attr("disabled", true);
                AddRequsitionitem(requsitionId, requsitionDetailsId, categoryId, itemId, costCentreId, stockById, quantity, itemName, costCentreName, stockBy, remarks, toCostCenterId, colorId, colorText, sizeId, sizeText, styleId, styleText);

                $("#ContentPlaceHolder1_hfProductId").val("0");
                $("#ContentPlaceHolder1_hfCategoryId").val("0");
                $("#ContentPlaceHolder1_txtQuantity").val("");
                $("#ContentPlaceHolder1_txtItemName").val("");
                $("#ContentPlaceHolder1_tbItemRemarks").val("");
                $("#ContentPlaceHolder1_ddlStockBy").val("0");
                $("#ContentPlaceHolder1_txtItemName").focus();
                $("#ContentPlaceHolder1_ddlColorAttribute").empty();
                $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
                $("#ContentPlaceHolder1_ddlStyleAttribute").empty();
                $("#ContentPlaceHolder1_ddlCategory").val('0').trigger('change');
                $("#ContentPlaceHolder1_ddlCurrentStock option").remove();
            });

            $("#ContentPlaceHolder1_ddlCostCentre").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlRequisitionTo").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#btnClearRequisition").click(function () {
                PerformClearRequisition();
                return false;
            });
            $("#ContentPlaceHolder1_ddlLocation").hide();
            $("#ContentPlaceHolder1_ddlRequisitionTo").val($("#ContentPlaceHolder1_ddlRequisitionTo option:first").val()).trigger('change');
            $("#ContentPlaceHolder1_ddlCostCentre").val($("#ContentPlaceHolder1_ddlCostCentre option:first").val()).trigger('change');
            
            var single = $("#ContentPlaceHolder1_hfIsSingle").val();
            if (single == "1") {
                $('#CompanyProjectDiv').hide();
                $('#CompanyProjectSrcDiv').hide();

            }
            else if (!IsCanView) {
                $('#CompanyProjectDiv').hide();
                $('#CompanyProjectSrcDiv').hide();
            }
            else {
                $('#CompanyProjectDiv').show();
                $('#CompanyProjectSrcDiv').show();

            }
            $("#ContentPlaceHolder1_btnSearch").click(function () {
                $("#ContentPlaceHolder1_hfSrcCompanyId").val($('#ContentPlaceHolder1_companyProjectUserControl2_ddlGLCompany').val());
                $("#ContentPlaceHolder1_hfSrcProjectId").val($('#ContentPlaceHolder1_companyProjectUserControl2_ddlGLProject').val());
            });

            $("#ContentPlaceHolder1_ddlCostCentre").val('0').trigger('change');
            $("#ContentPlaceHolder1_ddlRequisitionTo").change(function () {
                if ($(this).val() != "0") {
                    LoadLocationTo($(this).val());
                }
                else
                {
                    var control = $('#ContentPlaceHolder1_ddlLocationTo');
                    control.empty();
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');                    
                }
            });
            $("#ContentPlaceHolder1_ddlRequisitionTo").val('0').trigger('change');

            queryRqId = CommonHelper.GetParameterByName("RqId");
            if (queryRqId != "") {
                FillForm(queryRqId);
            }
        });

        function GetInvItemAttributeByItemIdAndAttributeType(itemId, type) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/frmPMRequisition.aspx/GetInvItemAttributeByItemIdAndAttributeType',
                data: "{'ItemId':'" + itemId + "','attributeType':'" + type + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.d != null) {
                        if (type == 'Color')
                            OnLoadAttributeColorSucceeded(data.d);
                        if (type == 'Size')
                            OnLoadAttributeSizeSucceeded(data.d);
                        if (type == 'Style')
                            OnLoadAttributeStyleSucceeded(data.d);
                    }
                },
                error: function (result) {
                    toastr.error("Please Contact With Admin");
                }
            });
        }

        function LoadLocationFrom(costCenetrId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/frmPMRequisition.aspx/InvLocationByCostCenter',

                data: JSON.stringify({ costCenterId: costCenetrId }),
                dataType: "json",
                async: false,
                success: function (data) {
                    OnLoadLocationFromSucceeded(data.d);
                },
                error: function (result) {
                    OnLoadLocationFailed(result.d);
                }
            });
        }

        function LoadLocationTo(costCenetrId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/frmPMRequisition.aspx/InvLocationByCostCenter',

                data: JSON.stringify({ costCenterId: costCenetrId }),
                dataType: "json",
                async: false,
                success: function (data) {
                    OnLoadLocationToSucceeded(data.d);
                },
                error: function (result) {
                    OnLoadLocationFailed(result.d);
                }
            });
        }

        function OnLoadLocationFromSucceeded(result) {
            var control = $('#ContentPlaceHolder1_ddlLocationFrom');

            control.empty();
            if (result != null) {
                if (result.length > 0) {
                    if (result.length > 1)
                        control.empty().append('<option value="0">---Please Select---</option>');
                    for (i = 0; i < result.length; i++) {
                        control.append('<option title="' + result[i].Name + '" value="' + result[i].LocationId + '">' + result[i].Name + '</option>');
                    }
                    if (result.length == 1)
                        $("#ContentPlaceHolder1_ddlLocationFrom").val(result[0].LocationId).trigger('change');
                }
                else {
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }

            if (result.length > 1)
                control.val($("#ContentPlaceHolder1_ddlLocationFrom option:first").val());

            return false;
        }
        function OnLoadLocationToSucceeded(result) {
            var control = $('#ContentPlaceHolder1_ddlLocationTo');

            control.empty();
            if (result != null) {
                if (result.length > 0) {
                    if (result.length > 1)
                        control.empty().append('<option value="0">---Please Select---</option>');
                    for (i = 0; i < result.length; i++) {
                        control.append('<option title="' + result[i].Name + '" value="' + result[i].LocationId + '">' + result[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }

            if (result.length > 1)
                control.val($("#ContentPlaceHolder1_ddlLocationTo option:first").val());

            return false;
        }
        function OnLoadLocationFailed() {
        }
        function PopulateProjects(control) {
            let companyId = $(control).val();

            $("#ContentPlaceHolder1_ddlGLProject").empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "frmPMRequisition.aspx/GetGLProjectByGLCompanyId",
                data: JSON.stringify({ companyId: companyId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    PopulateControlWithValueNTextField(response.d, $("#ContentPlaceHolder1_ddlGLProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }
        
        function LoadCategoryFromCC(costCenetrId) {
            PageMethods.LoadCategoryFromCostCenter(costCenetrId, OnLoadCategoryFromCostCenterSucceeded, OnLoadCategoryFromCostCenterFailed);
            return false;
        }
        function OnLoadCategoryFromCostCenterSucceeded(result) {

            var control = $('#ContentPlaceHolder1_ddlCategory');
            if (result != null) {
                if (result.length > 0) {
                    if (result.length > 1)
                        control.empty().append('<option value="0">---Please Select---</option>');
                    for (i = 0; i < result.length; i++) {
                        control.append('<option title="' + result[i].Name + '" value="' + result[i].CategoryId + '">' + result[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">---Please Select---</option>');
                }
            }

            return false;

        }
        function OnLoadCategoryFromCostCenterFailed(error) {
            toastr.warning(error);
            return false;
        }

        function DivShowHideFunction() {
            if ($("#<%=ddlDateType.ClientID %>").val() == "Pending") {
                $('#StatusLabelDiv').hide("slow");
                $('#StatusDiv').hide("slow");
                $('#DateDiv').hide("slow");
            }
            else {
                $('#StatusLabelDiv').show("slow");
                $('#StatusDiv').show("slow");
                $('#DateDiv').show("slow");
            }
        }
        function OnLoadStockBySucceeded(result) {
            var list = result;
            var ddlStockById = '<%=ddlStockBy.ClientID%>';
            var control = $('#' + ddlStockById);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    //control.removeAttr("disabled");
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].HeadName + '" value="' + list[i].UnitHeadId + '">' + list[i].HeadName + '</option>');
                    }
                }
                else {
                    //control.removeAttr("disabled");
                }
            }
            return false;
        }
        function OnLoadStockByFailed(error) {
        }

        function AddRequsitionitem(requsitionId, requsitionDetailsId, categoryId, itemId, costCentreId, stockById, quantity, itemName, costCentreName, stockBy, remarks, toCostCenterId, colorId, colorText, sizeId, sizeText, styleId, styleText) {

            var isEdited = "0";
            var rowLength = $("#RequsitionGrid tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:20%;display:none'>" + costCentreName + "</td>";
            tr += "<td style='width:20%;'>" + itemName + "</td>";
            tr += "<td style='width:10%;'>" + stockBy + "</td>";
            tr += "<td style='width:10%;'>" + quantity + "</td>";
            if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {

                tr += "<td style='width:10%;'>" + colorText + "</td>";
                tr += "<td style='width:10%;'>" + sizeText + "</td>";
                tr += "<td style='width:10%;'>" + styleText + "</td>";
            }
            else {
                tr += "<td style='display:none'>" + colorText + "</td>";
                tr += "<td style='display:none'>" + sizeText + "</td>";
                tr += "<td style='display:none'>" + styleText + "</td>";
            }

            tr += "<td style='width:10%;'> <a onclick=\"javascript:return FIllForEdit(this);\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
            tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteItemRequsition(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";

            debugger;

            tr += "<td style='display:none'>" + colorId + "</td>";
            tr += "<td style='display:none'>" + sizeId + "</td>";
            tr += "<td style='display:none'>" + styleId + "</td>";
            tr += "<td style='display:none'>" + requsitionDetailsId + "</td>";
            tr += "<td style='display:none'>" + categoryId + "</td>";
            tr += "<td style='display:none'>" + itemId + "</td>";
            tr += "<td style='display:none'>" + costCentreId + "</td>";
            tr += "<td style='display:none'>" + stockById + "</td>";
            tr += "<td style='display:none'>" + isEdited + "</td>";
            tr += "<td style='display:none'>" + remarks + "</td>";
            tr += "<td style='display:none'>" + toCostCenterId + "</td>";
            tr += "</tr>";
            $("#RequsitionGrid tbody").append(tr);

            ItemList.push({
                ItemId: parseInt(itemId, 10),
                ColorId: parseInt(colorId, 10),
                SizeId: parseInt(sizeId, 10),
                StyleId: parseInt(styleId, 10),
            });
        }

        function EditItem(requsitionId, requsitionDetailsId, categoryId, itemId, costCentreId, stockById, quantity, itemName, stockBy, remarks, toCostCenterId, colorId, colorText, sizeId, sizeText, styleId, styleText) {
            debugger;
            var ItemId = parseInt($(editedItem).find("td:eq(13)").text(), 10);
            var ColorId = parseInt($(editedItem).find("td:eq(8)").text(), 10);
            var SizeId = parseInt($(editedItem).find("td:eq(9)").text(), 10);
            var StyleId = parseInt($(editedItem).find("td:eq(10)").text(), 10);
            var CostCenter = _.findWhere(ItemList, { ItemId: ItemId, ColorId: ColorId, SizeId: SizeId, StyleId: StyleId});
            var indexEdited = _.indexOf(ItemList, CostCenter);

            ItemList[indexEdited].ItemId = parseInt(itemId, 10);
            ItemList[indexEdited].ColorId = parseInt(colorId, 10);
            ItemList[indexEdited].SizeId = parseInt(sizeId, 10);
            ItemList[indexEdited].StyleId = parseInt(styleId, 10);            

            $(editedItem).find("td:eq(12)").text(categoryId);
            $(editedItem).find("td:eq(13)").text(itemId);
            $(editedItem).find("td:eq(14)").text(costCentreId);
            $(editedItem).find("td:eq(15)").text(stockById);
            $(editedItem).find("td:eq(17)").text(remarks);
            $(editedItem).find("td:eq(18)").text(toCostCenterId);
            $(editedItem).find("td:eq(1)").text(itemName);
            $(editedItem).find("td:eq(2)").text(stockBy);
            $(editedItem).find("td:eq(3)").text(quantity);
            $(editedItem).find("td:eq(8)").text(colorId);
            $(editedItem).find("td:eq(9)").text(sizeId);
            $(editedItem).find("td:eq(10)").text(styleId);
            $(editedItem).find("td:eq(4)").text(colorText);
            $(editedItem).find("td:eq(5)").text(sizeText);
            $(editedItem).find("td:eq(6)").text(styleText);

            if (requsitionDetailsId != "0")
                $(editedItem).find("td:eq(16)").text("1");

            $("#ContentPlaceHolder1_hfProductId").val("0");
            $("#ContentPlaceHolder1_hfCategoryId").val("0");
            $("#ContentPlaceHolder1_txtQuantity").val("");
            $("#ContentPlaceHolder1_txtItemName").val("");
            $("#btnAddRequsition").val("Add Requsition");
            $("#ContentPlaceHolder1_ddlStockBy").val("0");
            $("#ContentPlaceHolder1_txtItemName").focus();
            $("#ContentPlaceHolder1_tbItemRemarks").val('');
            $("#ContentPlaceHolder1_ddlCurrentStock option").remove();
            $("#ContentPlaceHolder1_ddlColorAttribute").empty();
            $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
            $("#ContentPlaceHolder1_ddlStyleAttribute").empty();
            editedItem = "";
        }

        function FIllForEdit(editItem) {
            CommonHelper.SpinnerOpen();
            editedItem = $(editItem).parent().parent();
            var tr = $(editItem).parent().parent();

            $("#btnAddRequsition").val("Update Requsition");

            var categoryId = $(tr).find("td:eq(12)").text();
            var itemId = $(tr).find("td:eq(13)").text();
            var costCentreId = $(tr).find("td:eq(14)").text();
            var stockById = $(tr).find("td:eq(15)").text();
            var remarks = $(tr).find("td:eq(17)").text();
            var toCostCenterId = $(tr).find("td:eq(18)").text();
            var quantity = $(tr).find("td:eq(3)").text();
            var itemName = $(tr).find("td:eq(1)").text();
            var colorId = $(tr).find("td:eq(8)").text();
            var sizeId = $(tr).find("td:eq(9)").text();
            var styleId = $(tr).find("td:eq(10)").text();
            $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", true);
            $("#ContentPlaceHolder1_ddlRequisitionTo").attr("disabled", true);
            $("#ContentPlaceHolder1_txtItemName").val(itemName);
            $("#ContentPlaceHolder1_hfCategoryId").val(categoryId);
            $("#ContentPlaceHolder1_hfProductId").val(itemId);

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/frmPMRequisition.aspx/LoadRelatedStockBy',

                data: JSON.stringify({ stockById: stockById }),
                dataType: "json",
                async: false,
                success: function (data) {

                    OnLoadStockBySucceeded(data.d);
                },
                error: function (result) {
                    OnLoadStockByFailed(result);
                }
            });

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/frmPMRequisition.aspx/GetInvItemAttributeByItemIdAndAttributeType',

                data: JSON.stringify({ ItemId: itemId, attributeType: 'Style'}),
                dataType: "json",
                async: false,
                success: function (data) {

                    OnLoadAttributeStyleSucceeded(data.d);
                },
                error: function (result) {
                    OnLoadAttributeStyleFailed(result);
                }
            });
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/frmPMRequisition.aspx/GetInvItemAttributeByItemIdAndAttributeType',

                data: JSON.stringify({ ItemId: itemId, attributeType: 'Size'}),
                dataType: "json",
                async: false,
                success: function (data) {

                    OnLoadAttributeSizeSucceeded(data.d);
                },
                error: function (result) {
                    OnLoadAttributeSizeFailed(result);
                }
            });
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/frmPMRequisition.aspx/GetInvItemAttributeByItemIdAndAttributeType',

                data: JSON.stringify({ ItemId: itemId, attributeType: 'Color' }),
                dataType: "json",
                async: false,
                success: function (data) {

                    OnLoadAttributeColorSucceeded(data.d);
                },
                error: function (result) {
                    OnLoadAttributeColorFailed(result);
                }
            });

            $("#ContentPlaceHolder1_ddlCostCentre").val(costCentreId + '').trigger('change');
            $("#ContentPlaceHolder1_ddlStockBy").val(stockById).trigger('change');
            $("#ContentPlaceHolder1_txtQuantity").val(quantity);
            $("#ContentPlaceHolder1_tbItemRemarks").val(remarks);
            $("#ContentPlaceHolder1_ddlRequisitionTo").val(toCostCenterId + '').trigger('change');
            debugger;

            if (colorId != 0) {
                $("#ContentPlaceHolder1_ddlColorAttribute").val(colorId).trigger('change');
            }
            if (sizeId != 0) {
                $("#ContentPlaceHolder1_ddlSizeAttribute").val(sizeId).trigger('change');
            }
            if (styleId != 0) {
                $("#ContentPlaceHolder1_ddlStyleAttribute").val(styleId).trigger('change');
            }

            GetInvItemStockInfoByItemAndAttributeId();

            $("#ContentPlaceHolder1_txtItemName").focus();
            CommonHelper.SpinnerClose();
        }

        function DeleteItemRequsition(deleteItem) {
            if (!confirm("Do you want to delete?")) {
                return;
            }

            var requsitionId = "0", requsitionDetailsId = "0";
            var tr = $(deleteItem).parent().parent();

            requsitionDetailsId = $(tr).find("td:eq(11)").text();
            requsitionId = $("#ContentPlaceHolder1_hfRequisitionId").val();

            if ((requsitionDetailsId != "0")) {
                deleteDbItem.push({
                    RequisitionDetailsId: requsitionDetailsId,
                    RequisitionId: requsitionId
                });
            }

            var ItemId = parseInt($(tr).find("td:eq(13)").text(), 10);
            var ColorId = parseInt($(tr).find("td:eq(8)").text(), 10);
            var SizeId = parseInt($(tr).find("td:eq(9)").text(), 10);
            var StyleId = parseInt($(tr).find("td:eq(10)").text(), 10);
            var CostCenter = _.findWhere(ItemList, { ItemId: ItemId, ColorId: ColorId, SizeId: SizeId, StyleId: StyleId });
            var indexEdited = _.indexOf(ItemList, CostCenter);
            ItemList.splice(indexEdited, 1);            

            $(deleteItem).parent().parent().remove();

            if ($("#RequsitionGrid tbody tr").length == 0 && $("#ContentPlaceHolder1_hfRequisitionId").val() == "0") {
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlRequisitionTo").attr("disabled", false);
            }
        }

        function ValidationBeforeSave() {
            debugger;
            if ($("#ContentPlaceHolder1_ddlCostCentre").val() == "0") {
                toastr.warning("Please add requisition From.");
                return false;
            }
            if ($("#ContentPlaceHolder1_ddlRequisitionTo").val() == "0") {
                toastr.warning("Please add requisition To.");
                return false;
            }

            if ($("#RequsitionGrid tbody tr").length == 0) {
                toastr.warning("Please add requisition item.");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtReceivedByDate").val()) == "") {
                toastr.warning("Please give received by date.");
                return false;
            }

            var requsitionId = "0", requsitionDetailsId = "0", costCentreId = "0", isEdited = "0";
            var companyId = "0", projectId = "0";
            var categoryId = "", itemId = "", stockById = "", quantity = "";
            var receivedByDate = '', requisitionBy = '', remarks = '', toCostCenterId = "", itemRemarks = "";

            requsitionId = $("#ContentPlaceHolder1_hfRequisitionId").val();
            costCentreId = $("#ContentPlaceHolder1_ddlCostCentre").val();
            toCostCenterId = $("#ContentPlaceHolder1_ddlRequisitionTo").val();
            fromLocationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();
            toLocationId = $("#ContentPlaceHolder1_ddlLocationTo").val();
            receivedByDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtReceivedByDate").val(), '/');
            requisitionBy = $("#ContentPlaceHolder1_txtRequisitionBy").val();
            remarks = $("#ContentPlaceHolder1_txtRemarks").val();

            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            if (companyId == "0") {
                toastr.warning("Please Select Company.");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                return false;
            }
            else if (projectId == "0") {
                toastr.warning("Please Select Project.");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                return false;
            }

            if (fromLocationId == "0" || fromLocationId == null) {
                toastr.warning("Please Select From Location.");
                $("#ContentPlaceHolder1_ddlLocationFrom").focus();
                return false;
            }
            if (toLocationId == "0" || toLocationId == null) {
                toastr.warning("Please Select to Location.");
                $("#ContentPlaceHolder1_ddlLocationTo").focus();
                return false;
            }            

            var requsition = {
                RequisitionId: parseInt(requsitionId, 10),
                FromCostCenterId: parseInt(costCentreId, 10),
                ToCostCenterId: parseInt(toCostCenterId, 10),
                FromLocationId: parseInt(fromLocationId, 10),
                ToLocationId: parseInt(toLocationId, 10),
                ReceivedByDate: receivedByDate,
                RequisitionBy: requisitionBy,
                Remarks: remarks,
                CompanyId: companyId,
                ProjectId: projectId
            };
            newlyAddedItem = [];
            editDbItem = [];
            debugger;
            $("#RequsitionGrid tbody tr").each(function (index, item) {
                categoryId = $.trim($(item).find("td:eq(12)").text());
                itemId = $.trim($(item).find("td:eq(13)").text());
                costCentreId = $.trim($(item).find("td:eq(14)").text());
                stockById = $.trim($(item).find("td:eq(15)").text());
                quantity = $.trim($(item).find("td:eq(3)").text());
                requsitionDetailsId = $.trim($(item).find("td:eq(11)").text());
                isEdited = $.trim($(item).find("td:eq(16)").text());
                itemRemarks = $.trim($(item).find("td:eq(17)").text());
                toCostCenterId = $.trim($(item).find("td:eq(18)").text());
                colorId = $.trim($(item).find("td:eq(8)").text());
                sizeId = $.trim($(item).find("td:eq(9)").text());
                styleId = $.trim($(item).find("td:eq(10)").text());

                if (requsitionDetailsId == "0") {
                    newlyAddedItem.push({
                        RequisitionDetailsId: parseInt(requsitionDetailsId, 10),
                        RequisitionId: parseInt(requsitionId, 10),
                        CategoryId: parseInt(categoryId, 10),
                        ItemId: parseInt(itemId, 10),
                        ColorId: parseInt(colorId, 10),
                        SizeId: parseInt(sizeId, 10),
                        StyleId: parseInt(styleId, 10),
                        StockById: parseInt(stockById, 10),
                        Quantity: parseFloat(quantity),
                        ItemRemarks: itemRemarks
                    });
                }
                else if (requsitionDetailsId != "0" && isEdited != "0") {

                    editDbItem.push({
                        RequisitionDetailsId: parseInt(requsitionDetailsId, 10),
                        RequisitionId: parseInt(requsitionId, 10),
                        CategoryId: parseInt(categoryId, 10),
                        ItemId: parseInt(itemId, 10),
                        ColorId: parseInt(colorId, 10),
                        SizeId: parseInt(sizeId, 10),
                        StyleId: parseInt(styleId, 10),
                        StockById: parseInt(stockById, 10),
                        Quantity: parseFloat(quantity),
                        ItemRemarks: itemRemarks
                    });
                }
            });
            debugger;
            CommonHelper.SpinnerOpen();
            PageMethods.SaveRequsition(requsitionId, requsition, newlyAddedItem, editDbItem, deleteDbItem, OnSaveRequsitionSucceed, OnSaveRequsitionFailed);

            return false;
        }

        function OnSaveRequsitionSucceed(result) {
            debugger;
            if (result.IsSuccess) {
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
                $("#ContentPlaceHolder1_ddlRequisitionTo").attr("disabled", false);
                CommonHelper.AlertMessage(result.AlertMessage);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/Common/WebMethodPage.aspx/GetCommonCheckByApproveByListForSMS',
                    data: JSON.stringify({ tableName: 'PMRequisition', primaryKeyName: 'RequisitionId', primaryKeyValue: result.PrimaryKeyValue, featuresValue: 'Requisition', statusColumnName: 'ApprovedStatus' }),
                    dataType: "json",
                    success: function (data) {

                        SendSMSToUserList(data.d, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);

                    },
                    error: function (result) {
                        toastr.error("Can not load Check or Approve By List.");
                    }
                });
                PerformClearAction();
                //ItemList = new Array();
                if (queryRqId != "") {
                    setTimeout(function () {
                        window.location = "/PurchaseManagment/ItemRequisitionInformation.aspx";
                    }, 1000);
                }
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();

            return false;
        }

        function SendSMSToUserList(UserList, PrimaryKeyValue, TransactionNo, TransactionType, TransactionStatus) {

            var str = '';
            if (TransactionStatus == 'Approved') {
                str += TransactionType + ' No.(' + TransactionNo + ')  is Approved.';
            }
            else if (TransactionStatus == 'Cancel') {
                str += TransactionType + ' No.(' + TransactionNo + ')  is Canceled.';
            }
            else {
                str += TransactionType + ' No.(' + TransactionNo + ') is waiting for your Approval Process.';
            }
            var CommonMessage = {
                Subjects: str,
                MessageBody: str
            };

            var messageDetails = [];
            if (UserList.length > 0) {

                for (var i = 0; i < UserList.length; i++) {
                    messageDetails.push({
                        MessageTo: UserList[i]
                    });
                }

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '/HMCommon/frmCommonMessage.aspx/SendMailByID',
                    data: JSON.stringify({ CMB: CommonMessage, CMD: messageDetails }),
                    dataType: "json",
                    success: function (data) {

                        //CommonHelper.AlertMessage(data.d.AlertMessage);

                    },
                    error: function (result) {
                        //alert("Error");

                    }
                });

            }

            return false;
        }

        function OnSaveRequsitionFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        function FillFormEdit(requisitionId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }

            FillForm(requisitionId);
            return false;
        }

        function ConfirmApprovalStatus(ApprovalStatus) {
            //debugger;
            var status = (ApprovalStatus == "Submit") ? "Check" : "Approve";
            if (!confirm("Do you want to " + status + "?")) {
                return false;
            }
        }

        function FillForm(requisitionId) {
            debugger; // qqqq

            CommonHelper.SpinnerOpen();

            PageMethods.FillForm(requisitionId, OnFillFormSucceed, OnFillFormFailed);

            return false;
        }
        function OnFillFormSucceed(result) {

            if (result != null) {

                $("#<%=btnSave.ClientID %>").val("Update");
                $("#RequsitionGrid tbody").html("");

                $("#ContentPlaceHolder1_hfRequisitionId").val(result.Requisition.RequisitionId);
                $("#ContentPlaceHolder1_ddlCostCentre").val(result.Requisition.FromCostCenterId).trigger('change');
                $("#ContentPlaceHolder1_ddlRequisitionTo").val(result.Requisition.ToCostCenterId).trigger('change');
                $("#ContentPlaceHolder1_txtReceivedByDate").val(GetStringFromDateTime(result.Requisition.ReceivedByDate));
                $("#ContentPlaceHolder1_txtRequisitionBy").val(result.Requisition.RequisitionBy);
                $("#ContentPlaceHolder1_txtRemarks").val(result.Requisition.Remarks);
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(result.Requisition.CompanyId).trigger('change');
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(result.Requisition.ProjectId).trigger('change');

                var rowLength = result.RequisitionDetails.length;
                var row = 0;

                for (row = 0; row < rowLength; row++) {
                    AddRequsitionitem(result.RequisitionDetails[row].RequisitionId, result.RequisitionDetails[row].RequisitionDetailsId,
                        result.RequisitionDetails[row].CategoryId, result.RequisitionDetails[row].ItemId,
                        result.Requisition.FromCostCenterId, result.RequisitionDetails[row].StockById,
                        result.RequisitionDetails[row].Quantity, result.RequisitionDetails[row].ItemName,
                        result.Requisition.FromCostCenter, result.RequisitionDetails[row].HeadName, result.RequisitionDetails[row].ItemRemarks, result.Requisition.ToCostCenterId, result.RequisitionDetails[row].ColorId, result.RequisitionDetails[row].ColorText, result.RequisitionDetails[row].SizeId, result.RequisitionDetails[row].SizeText, result.RequisitionDetails[row].StyleId, result.RequisitionDetails[row].StyleText);

                }

                //$("#myTabs").tabs('select', 0);
                $("#myTabs").tabs({ active: 0 });
                CommonHelper.SpinnerClose();
                return false;
            }
        }
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
            return false;
        }

        function RequisitionDetails(requisitionId) {
            CommonHelper.SpinnerOpen();
            PageMethods.RequisitionDetails(requisitionId, OnFillRequisitionDetailsSucceed, OnFillRequisitionDetailsFailed);
            return false;
        }
        function OnFillRequisitionDetailsSucceed(result) {

            $("#DetailsRequisitionGrid tbody").html("");
            var totalRow = result.length, row = 0;

            var tr = "";

            for (row = 0; row < totalRow; row++) {

                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:16%;'>" + result[row].ItemName + "</td>";

                if ($("#ContentPlaceHolder1_hfIsItemAttributeEnable").val() == "1") {
                    tr += "<td style='width:10%;'>" + result[row].ColorText + "</td>";
                    tr += "<td style='width:10%;'>" + result[row].SizeText + "</td>";
                    tr += "<td style='width:10%;'>" + result[row].StyleText + "</td>";
                }
                else {
                    tr += "<td style='display:none'>" + result[row].ColorText + "</td>";
                    tr += "<td style='display:none'>" + result[row].SizeText + "</td>";
                    tr += "<td style='display:none'>" + result[row].StyleText + "</td>";
                }

                tr += "<td style='width:10%;'>" + result[row].HeadName + "</td>";

                if (result[row].ApprovedQuantity != null)
                    tr += "<td style='width:10%;'>" + result[row].ApprovedQuantity + "</td>";
                else
                    tr += "<td style='width:10%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:10%;'>" + result[row].ItemRemarks + "</td>";

                tr += "<td style='width:10%;'>" + result[row].CurrentStockFromStore + "</td>";
                tr += "<td style='width:7%;'>" + result[row].LastRequisitionQuantity + "</td>";
                if (result[row].LastTransferQuantity != 0)
                    tr += "<td style='width:7%; '>" + result[row].LastTransferQuantity + "(" + result[row].LastTransferType + ")</td>";
                else
                    tr += "<td style='width:7%; '>" + result[row].LastTransferQuantity + "</td>";

                tr += "</tr>";
                $("#DetailsRequisitionGrid tbody").append(tr);
                tr = "";
            }
            var reqStatus = "";
            reqStatus += "<div><b>Requisition By: " + result[0].RequisitionBy + "</b></ div >";
            if (result[0].CheckedBy != null) {
                reqStatus += "<div><b>Checked By: " + result[0].CheckedBy + "</b></ div >";
            }

            document.getElementById("requisitionDiv").innerHTML = reqStatus;

            $("#DetailsRequisitionGridContaiiner").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 1000,
                maxWidth: 1100,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Requisition Details",
                show: 'slide'
            });
            CommonHelper.SpinnerClose();
        }
        function OnFillRequisitionDetailsFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        function PerformClearRequisition() {
            $("#ContentPlaceHolder1_ddlCategory").val('0').trigger('change');
            $("#ContentPlaceHolder1_txtItemName").val('');
            $("#ContentPlaceHolder1_hfCategoryId").val('0');
            $("#ContentPlaceHolder1_hfProductId").val('0');
            $("#ContentPlaceHolder1_ddlStockBy").val('0');
            $("#ContentPlaceHolder1_txtQuantity").val('');
            $("#ContentPlaceHolder1_tbItemRemarks").val('');
            $("#ContentPlaceHolder1_ddlCurrentStock option").remove();
            $("#ContentPlaceHolder1_ddlColorAttribute").empty();
            $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
            $("#ContentPlaceHolder1_ddlStyleAttribute").empty();

            if ($("#RequsitionGrid tbody tr").length == 0) {
                $("#ContentPlaceHolder1_ddlCostCentre").val('0');
                $("#ContentPlaceHolder1_hfRequisitionId").val('0');
                $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
            }

            editedItem = "";
            var filter = $("#<%=ddlDateType.ClientID %>").val();
            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl2_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl2_ddlGLProject").val();            
        }


        function PerformClearRequisitionByCategoryChange() {
            $("#ContentPlaceHolder1_txtItemName").val('');
            $("#ContentPlaceHolder1_hfProductId").val('0');
            $("#ContentPlaceHolder1_ddlStockBy").val('0');
            $("#ContentPlaceHolder1_txtQuantity").val('');
            $("#ContentPlaceHolder1_tbItemRemarks").val('');
            $("#ContentPlaceHolder1_ddlCurrentStock option").remove();
            $("#ContentPlaceHolder1_ddlColorAttribute").empty();
            $("#ContentPlaceHolder1_ddlSizeAttribute").empty();
            $("#ContentPlaceHolder1_ddlStyleAttribute").empty();            
        }

        function PerformClearAction() {
            $("#<%=txtReceivedByDate.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#<%=txtRequisitionBy.ClientID %>").val('');

            deleteDbItem = []; editDbItem = []; newlyAddedItem = [];

            ItemList = new Array();

            var x = document.getElementById("ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").length;
            if (x > 1) {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val("0").trigger('change');
            }

            var y = document.getElementById("ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").length;
            if (y > 1) {
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val("0").trigger('change');
            }

            $("#ContentPlaceHolder1_hfRequisitionId").val('0');
            $("#ContentPlaceHolder1_txtItemName").val('');
            $("#ContentPlaceHolder1_hfCategoryId").val('0');
            $("#ContentPlaceHolder1_hfProductId").val('0');
            $("#ContentPlaceHolder1_ddlCostCentre").val('0').trigger('change');
            $("#ContentPlaceHolder1_ddlRequisitionTo").val('0').trigger('change');
            $("#ContentPlaceHolder1_ddlStockBy").val('0');
            $("#ContentPlaceHolder1_txtQuantity").val('');

            $("#RequsitionGrid tbody").html("");
            $("#ContentPlaceHolder1_ddlCostCentre").attr("disabled", false);
            $("#<%=btnSave.ClientID %>").val("Save");

            return false;
        }
        function LoadCurrentStockQuantity(itemId) {
            debugger;
            var costcenterId = parseInt($("#ContentPlaceHolder1_ddlCostCentre").val());
            var index = $("#ContentPlaceHolder1_ddlCostCentre")[0].selectedIndex;
            var locationId = $("#ContentPlaceHolder1_ddlLocationFrom").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            PageMethods.LoadCurrentStockQuantity(costcenterId, locationId, itemId, projectId, OnLoadCurrentStockSucceeded, OnLoadCurrentStockFailed);
        }

        function GetInvItemStockInfoByItemAndAttributeId() {
            var locationId = parseInt($('#ContentPlaceHolder1_ddlLocation').val(), 10);
            var colorddlLength = $('#ContentPlaceHolder1_ddlColorAttribute > option').length;
            var sizeddlLength = $('#ContentPlaceHolder1_ddlSizeAttribute > option').length;
            var styleddlLength = $('#ContentPlaceHolder1_ddlStyleAttribute > option').length;
            var colorId = 0;
            if (colorddlLength > 0) {
                colorId = parseInt($("#ContentPlaceHolder1_ddlColorAttribute option:selected").val(), 10);
            }
            var sizeId = 0;
            if (sizeddlLength > 0) {
                sizeId = parseInt($("#ContentPlaceHolder1_ddlSizeAttribute option:selected").val(), 10);
            }
            var styleId = 0;
            if (styleddlLength > 0) {
                styleId = parseInt($("#ContentPlaceHolder1_ddlStyleAttribute option:selected").val(), 10);
            }
            var itemId = parseInt($("#ContentPlaceHolder1_hfProductId").val(), 10);

            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../PurchaseManagment/frmPMRequisition.aspx/GetInvItemStockInfoByItemAndAttributeId',
                data: "{'itemId':'" + itemId + "','colorId':'" + colorId + "','sizeId':'" + sizeId + "','styleId':'" + styleId + "','locationId':'" + locationId + "'}",
                dataType: "json",
                async: false,
                success: function (data) {

                    $("#ContentPlaceHolder1_ddlCurrentStock option").remove();

                    if (data.d != null) {
                        var str = '<option>' + data.d.StockQuantity + ' </option>';
                        $("#ContentPlaceHolder1_ddlCurrentStock").append(str);
                    }
                    else {
                        var str = '<option>' + 0 + ' </option>';
                        $("#ContentPlaceHolder1_ddlCurrentStock").append(str);
                    }
                },
                error: function (result) {
                    toastr.error("Please Contact With Admin");
                }
            });
        }
        function OnLoadCurrentStockSucceeded(result) {
            $("#ContentPlaceHolder1_ddlCurrentStock option").remove();
            if (result != null) {
                var str = '<option>' + result.StockQuantity + ' </option>';
                $("#ContentPlaceHolder1_ddlCurrentStock").append(str);
            }
            else {
                var str = '<option>' + 0 + ' </option>';
                $("#ContentPlaceHolder1_ddlCurrentStock").append(str);
            }
        }
        function OnLoadCurrentStockFailed(error) {
        }

        function ConfirmDelete() {
            return confirm("Press Ok to Cancel Requisition");
        }
    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCategoryId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfProductId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfRequisitionId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsSingle" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyAll" runat="server" />
    <asp:HiddenField ID="SingleprojectId" runat="server"></asp:HiddenField>
     <asp:HiddenField ID="hfIsItemAttributeEnable" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfSrcCompanyId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSrcProjectId" runat="server"></asp:HiddenField>
    <div style="display: none;">
        <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="customLargeDropDownSize"
            TabIndex="1">
            <asp:ListItem>Product</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div style="display: none">
        <asp:Label runat="server" ID="IsCanEdit"></asp:Label>
        <asp:Label runat="server" ID="IsCanDelete"></asp:Label>
    </div>
    <div id="DetailsRequisitionGridContaiiner" style="display: none;">
        <table id="DetailsRequisitionGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                    <th style="width: 16%;">Item
                    </th>
                    <th id="cIdd" style="width: 10%;">Color
                    </th>
                    <th id="sIdd" style="width: 10%;">Size
                    </th>
                    <th id="stIdd" style="width: 10%;">Style
                    </th>
                    <th style="width: 10%;">Unit
                    </th>
                    <th style="width: 10%;">Quantity
                    </th>
                    <th style="width: 10%;">Remarks
                    </th>
                    <th style="width: 10%;">Available Quantity
                    </th>
                    <th style="width: 7%;">Last Requisition Quantity
                    </th>
                    <th style="width: 7%;">Last Transfer Quantity
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <div id="requisitionDiv">
        </div>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Requisition Info</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Requisition </a></li>
        </ul>
        <div id="tab-1">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Requisition From"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCostCentre" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlLocation" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Requisition To"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlRequisitionTo" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label13" runat="server" class="control-label required-field" Text="From Location"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlLocationFrom" runat="server" CssClass="form-control" TabIndex="5">
                            </asp:DropDownList>
                        </div>
                        <div id="dvCostCenterTo">
                            <div class="col-md-2">
                                <asp:Label ID="Label14" runat="server" class="control-label required-field" Text="To Location"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlLocationTo" runat="server" CssClass="form-control" TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" id="CompanyProjectDiv">
                        <div class="col-md-12">
                            <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div id="EntryPanel" class="panel panel-default">
                        <div class="panel-heading">
                            Item Requisition
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group" id="categoryDiv">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group" id="ItemNamePanel">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblItemName" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtItemName" runat="server" placeholder="Enter minimum 3 characters" TabIndex="5" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="AttributeDiv">
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="Label6" runat="server" class="control-label" Text="Color"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlColorAttribute" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="Label7" runat="server" class="control-label" Text="Size"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlSizeAttribute" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="Label8" runat="server" class="control-label" Text="Style"></asp:Label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:DropDownList ID="ddlStyleAttribute" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Current Stock"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlCurrentStock" Enabled="false" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Stock Unit"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlStockBy" Enabled="false" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblQuantity" runat="server" class="control-label required-field" Text="Quantity"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control quantitydecimal" TabIndex="6"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Remarks"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="tbItemRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" TabIndex="7" MaxLength="150"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group" style="padding: 5px 0 5px 0;">
                                    <div class="col-md-12">
                                        <input id="btnAddRequsition" type="button" class="TransactionalButton btn btn-primary btn-sm"
                                            value="Add Requisition" />
                                        <input id="btnClearRequisition" type="button" class="TransactionalButton btn btn-primary btn-sm"
                                            value="Clear" />
                                    </div>
                                </div>
                                <div class="form-group" id="RequsitionTableContainer" style="overflow: scroll;">
                                    <table id="RequsitionGrid" class="table table-bordered table-condensed table-responsive"
                                        style="width: 100%;">
                                        <thead>
                                            <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                                <th style="width: 20%; display: none">Store Name (From)
                                                </th>
                                                <th style="width: 20%;">Item
                                                </th>
                                                <th style="width: 10%;">Stock By
                                                </th>
                                                <th style="width: 10%;">Quantity
                                                </th>
                                                
                                                <th id="cId" style="width: 10%;">Color
                                                </th>
                                                <th id="sId" style="width: 10%;">Size
                                                </th>
                                                <th id="stId" style="width: 10%;">Style
                                                </th>
                                                <th style="width: 10%;">Action
                                                </th>                                                
                                                <th style="display: none">Color Id
                                                </th>
                                                <th style="display: none">Size Id
                                                </th>
                                                <th style="display: none">Style Id
                                                </th>
                                                <th style="display: none">Requsiotion Id
                                                </th>
                                                <th style="display: none">Category Id
                                                </th>
                                                <th style="display: none">Product Id
                                                </th>
                                                <th style="display: none">Costcenter id
                                                </th>
                                                <th style="display: none">Stock By Id
                                                </th>
                                                <th style="display: none">Is Edited
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                TabIndex="8"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblReceivedByDate" runat="server" class="control-label required-field"
                                Text="Required Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtReceivedByDate" TabIndex="7" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" style="display: none">
                        <div class="col-md-2">
                            <asp:Label ID="lblRequisitionBy" runat="server" class="control-label required-field"
                                Text="Requisition By"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRequisitionBy" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="javascript: return ValidationBeforeSave();"
                                OnClick="btnSave_Click" CssClass="TransactionalButton btn btn-primary btn-sm"
                                TabIndex="10" />
                            <asp:Button ID="btnClear" runat="server" TabIndex="11" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformClearAction();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="InfoPanel" class="panel panel-default">
                <div class="panel-heading">
                    Requisition Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDateType" runat="server" Text="Filter On"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDateType" runat="server" CssClass="form-control" TabIndex="1">
                                    <asp:ListItem Value="Pending">Requisition Pending</asp:ListItem>
                                    <asp:ListItem Value="CreatedDate">Requisition Date</asp:ListItem>
                                    <asp:ListItem Value="ReceivedDate">Received By Date</asp:ListItem>
                                    <%--<asp:ListItem Value="CompanyProject">Company & Project</asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2" id="StatusLabelDiv">
                                <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4" id="StatusDiv">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Submitted</asp:ListItem>
                                    <asp:ListItem>Approved</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="DateDiv">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" id="CompanyProjectSrcDiv">
                            <div class="col-md-12">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" OnClientClick="javascript:return PerformClearRequisition();" />
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
                    <asp:GridView ID="gvRequisitionInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnPageIndexChanging="gvRequisitionInfo_PageIndexChanging"
                        OnRowCommand="gvRequisitionInfo_RowCommand" TabIndex="9" OnRowDataBound="gvRequisitionInfo_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("RequisitionId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PRNumber" HeaderText="PR Number" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FromCostCenter" HeaderText="From Store" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ToCostCenter" HeaderText="To Store" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="RequisitionBy" HeaderText="Requisition By" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    &nbsp;<asp:ImageButton ID="ImgDetailsRequisition" runat="server" CausesValidation="False"
                                        CommandName="CmdRequisitionDetails" CommandArgument='<%# bind("RequisitionId") %>'
                                        OnClientClick='<%#String.Format("return RequisitionDetails({0})", Eval("RequisitionId")) %>'
                                        ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Requisition Details" />
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("RequisitionId") %>' OnClientClick='<%#String.Format("return FillFormEdit({0})", Eval("RequisitionId")) %>'
                                        ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        OnClientClick="return ConfirmDelete()" CommandArgument='<%# bind("RequisitionId") %>' ImageUrl="~/Images/delete.png"
                                        Text="" AlternateText="Delete" ToolTip="Delete" />
                                    &nbsp;&nbsp;<asp:ImageButton ID="ImgReportRI" runat="server" CausesValidation="False"
                                        CommandName="CmdReportRI" CommandArgument='<%# bind("RequisitionId") %>' ImageUrl="~/Images/ReportDocument.png"
                                        Text="" AlternateText="Invoice" ToolTip="Requisition Order Invoice" />
                                    &nbsp;<asp:ImageButton ID="ImgApprove" runat="server" CausesValidation="False" CommandName="CmdApprove"
                                        OnClientClick='<%#String.Format("return ConfirmApprovalStatus(\"{0}\")", Eval("ApprovedStatus").ToString()) %>' CommandArgument='<%# bind("RequisitionId") %>' />
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIsCanChecked" runat="server" Text='<%#Eval("IsCanCheckeAble") %>'></asp:Label>
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIsCanApproved" runat="server" Text='<%#Eval("IsCanApproveAble") %>'></asp:Label>
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
        </div>
    </div>
</asp:Content>
