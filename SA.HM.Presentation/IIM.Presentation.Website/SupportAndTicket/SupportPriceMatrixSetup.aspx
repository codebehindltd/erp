<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="SupportPriceMatrixSetup.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.SupportPriceMatrixSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var ItemSelected = null;
        var PriceMatrixItem = new Array();
        isContinueSave = false;
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            isContinueSave = false;
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            CommonHelper.ApplyDecimalValidation();
            PriceMatrix = $("#tblMatrix").DataTable({
                data: [],
                columns: [
                    { title: "Company", "data": "Company", sWidth: '20%' },
                    { title: "Category", "data": "Category", sWidth: '20%' },
                    { title: "Item", "data": "Item", sWidth: '20%' },
                    { title: "Unit Head", "data": "UnitHead", sWidth: '15%' },
                    { title: "Unit Price", "data": "Price", sWidth: '15%' },
                    { title: "Action", "data": null, sWidth: '10%' },
                    { title: "", "data": "Id", visible: false }
                ],
                columnDefs: [
                    {
                        //"targets": 3,
                        //"className": "text-center",
                        //"render": function (data, type, full, meta) {
                        //    return (data == true ? "Active" : "Inactive");
                        // }
                    }],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEdit('" + aData.Id + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"PerformDelete('" + aData.Id + "');\"> <img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
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
            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
                dropdownParent: "#CreateNewDialog",
            });
            $("#ContentPlaceHolder1_txtItem").autocomplete({
                source: function (request, response) {
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SupportAndTicket/SupportPriceMatrixSetup.aspx/ItemSearch',
                        data: JSON.stringify({ searchTerm: request.term, categoryId: categoryId }),
                        dataType: "json",
                        async: false,
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    ItemName: m.Name,
                                    ItemId: m.ItemId,
                                    CategoryId: m.CategoryId,
                                    ManufacturerName: m.ManufacturerName,
                                    Model: m.Model,
                                    Description: m.Description,
                                    CategoryName: m.CategoryName,
                                    UnitHead: m.UnitHead,
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
                    ItemSelected = ui.item;
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfItemId").val(ui.item.value);
                    $("#ContentPlaceHolder1_lblCurrentStockBy").text(ui.item.UnitHead);
                    var brand = ui.item.ManufacturerName == '' ? '' : "Brand: " + ui.item.ManufacturerName + '';
                    var model = ui.item.Model == '' ? '' : ", Model: " + ui.item.Model + '';
                    var description = ui.item.Description == '' ? '' : ", Description: " + ui.item.Description;
                    $("#ContentPlaceHolder1_lblItemWiseRemarks").text(brand + model);
                }
            });
            $("#ContentPlaceHolder1_txtCompany").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SupportAndTicket/SupportPriceMatrixSetup.aspx/CompanySearch',
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyId,
                                    CompanyId: m.CompanyId,
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
                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.CompanyId);
                    $("#ContentPlaceHolder1_txtCompany").prop('disabled', true);
                }
            });
            $("#ContentPlaceHolder1_txtSearchCompany").autocomplete({
                source: function (request, response) {


                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SupportAndTicket/SupportPriceMatrixSetup.aspx/CompanySearch',
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    //label: m.CompanyNameWithCode,
                                    //value: m.CompanyId,
                                    //Lvl: m.Lvl,
                                    //Hierarchy: m.Hierarchy
                                    label: m.CompanyName,
                                    value: m.CompanyId,
                                    CompanyId: m.CompanyId
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

                    ClientSelected = ui.item;

                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfClientId").val(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_txtPurchasePrice").blur(function () {
                if ($("#ContentPlaceHolder1_hfId").val() == "0") {
                    return false;
                }
                else {
                    var price = $("#ContentPlaceHolder1_txtPurchasePrice").val();
                    PriceMatrixItem[0].Price = parseFloat(price);
                }
                return false;
            });

            GridPaging(1, 1);
        });
        function CreateNew() {
            PerformClearAction();
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '95%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Support Price Matrix",
                show: 'slide'
            });
            //$("#ContentPlaceHolder1_ddlForcastType").val("Deal");
            return false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#SourceTable tbody tr").length;
            var company = $("#ContentPlaceHolder1_txtSearchCompany").val();
            if (company != '') {
                var company = $("#ContentPlaceHolder1_hfClientId").val();
            }
            else
                company = "";
            var item = $("#ContentPlaceHolder1_txtSearchItem").val();
            PageMethods.SearchStage(company, item, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchucceed, OnSearchFailed);
            return false;
        }
        function OnSearchucceed(result) {
            $("#GridPagingContainer ul").empty();
            PriceMatrix.clear();
            PriceMatrix.rows.add(result.GridData);
            PriceMatrix.draw();
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }
        function OnSearchFailed(error) {
            toastr.error(error.get_message());
            return false;
        }
        function AddItem() {
            var companyId = $("#ContentPlaceHolder1_hfCompanyId").val();
            if (ItemSelected == null) {
                toastr.warning("Please Select Item.");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtPurchasePrice").val()) == "" || $.trim($("#ContentPlaceHolder1_txtPurchasePrice").val()) == "0") {
                toastr.warning("Please Give Purchase Price.");
                return false;
            }
            else if (companyId == "0") {
                toastr.warning("Please Select a Company.");
                return false;
            }
            var itm = _.findWhere(PriceMatrixItem, { ItemId: ItemSelected.ItemId });
            if (itm != null) {
                toastr.warning("Same Item Already Added. Duplicate Item Is Not Accepted.");
                return false;
            }
            PageMethods.DuplicateCheckDynamicaly("CompanyId", companyId, "ItemId", ItemSelected.ItemId, 0, 0, DuplicateCheckDynamicalySucceed, DuplicateCheckDynamicalyFailed);
            return false;
        }
        function DuplicateCheckDynamicalySucceed(result) {
            var companyId = $("#ContentPlaceHolder1_hfCompanyId").val();
            if (result > 0) {
                toastr.warning("Duplicate Company And Item");
                //ClearItemAdded();
                return false;
            }
            var total = 0, unitPrice = 0, quantity = 0, tr = "", remarks = "", t = "";
            unitPrice = $("#ContentPlaceHolder1_txtPurchasePrice").val();
            tr += "<tr>";
            tr += "<td style='width:20%;'>" + ItemSelected.CategoryName + "</td>";
            tr += "<td style='width:30%;'>" + ItemSelected.ItemName + "</td>";
            tr += "<td style='width:20%;'>" + ItemSelected.UnitHead + "</td>";
            tr += "<td style='width:25%;'>" +
                "<input type='text' value='" + unitPrice + "' id='pq" + ItemSelected.ItemId + "' class='form-control quantitydecimal' onblur='ChangePrice(this)' />" +
                "</td>";
            tr += "<td style='width:5%;'>" +
                "<a href='javascript:void()' onclick= 'DeleteSelectedItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>" +
                "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.ItemId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.CategoryId + "</td>";
            tr += "<td style='display:none;'>" + unitPrice + "</td>";
            tr += "<td style='display:none;'>0</td>";
            tr += "</tr>";
            $("#ItemForPurchase tbody").prepend(tr);
            tr = "";
            //CalculateGrandTotal();
            PriceMatrixItem.push({
                Id: 0,
                CompanyId: companyId,
                ItemId: parseInt(ItemSelected.ItemId, 10),
                Price: parseFloat(unitPrice)
            });
            CommonHelper.ApplyDecimalValidation();
            ClearItemAdded();
            $("#ContentPlaceHolder1_txtItem").focus();
        }
        function DuplicateCheckDynamicalyFailed(error) {

        }
        function DeleteSelectedItem(control) {
            if (!confirm("Do you want to delete item?")) { return false; }
            var tr = $(control).parent().parent();
            var itemId = parseInt($.trim($(tr).find("td:eq(9)").text()), 10);
            var item = _.findWhere(PriceMatrixItem, { ItemId: itemId });
            var index = _.indexOf(PriceMatrixItem, item);
            PriceMatrixItem.splice(index, 1);
            $(tr).remove();
        }
        function ChangePrice(control) {
            var tr = $(control).parent().parent();
            var Price = $.trim($(tr).find("td:eq(3)").find("input").val());
            if (Price == "" || Price == "0") {
                toastr.info("Purchase Price Cannot Be Zero Or Empty.");
                $(tr).find("td:eq(3)").find("input").val(oldPurchasePrice);
                return false;
            }
            var itemId = parseInt($.trim($(tr).find("td:eq(5)").text()), 10);
            var item = _.findWhere(PriceMatrixItem, { ItemId: itemId });
            var index = _.indexOf(PriceMatrixItem, item);
            PriceMatrixItem[index].Price = parseFloat(Price);
        }
        function ClearItemAdded() {
            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_lblCurrentStockBy").text("");
            $("#ContentPlaceHolder1_txtPurchasePrice").val("");
            $("#ContentPlaceHolder1_lblItemWiseRemarks").text("");
            ItemSelected = null;
        }
        function SaveOrUpdate() {
            var companyId = $("#ContentPlaceHolder1_hfCompanyId").val();
            if (companyId == "0") {
                toastr.warning("Please Select a Company.");
                $("#ContentPlaceHolder1_txtCompany").focus();
                isContinueSave = false;
                return false;
            }
            PageMethods.SaveOrUpdatePriceMatrix(PriceMatrixItem, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
            return false;
        }
        function OnSuccessSaveOrUpdate(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                if (!isContinueSave) {
                    $("#CreateNewDialog").dialog("close");
                }
                GridPaging(1, 1);
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            isContinueSave = false;
            return false;
        }
        function OnFailSaveOrUpdate(error) {
            toastr.error(error.get_message());
        }
        function PerformClearAction() {
            PriceMatrixItem = [];
            $("#ContentPlaceHolder1_ddlCategory").val("0").trigger('change').prop('disabled', false);
            $("#ContentPlaceHolder1_txtItem").val("").prop('disabled', false);
            $("#ItemForPurchase tbody").html("");
            $("#ContentPlaceHolder1_hfCompanyId").val("0");
            $("#ContentPlaceHolder1_txtCompany").val("").prop('disabled', false);
            $("#btnSave").val("Save And Close");
            $("#btnSaveNContinue").show();
            $("#btnClear").show();
            $("#btnAdd").show();
            $("#btnCancelOrder").show();
        }
        function PerformEdit(id) {
            FillForm(id);
        }
        function FillForm(id) {
            PageMethods.GetStageById(id, OnSuccessLoad, OnFailLoad)
            return false;
        }
        function OnSuccessLoad(result) {
            if (!confirm("Do you want to edit - " + result.Item + "?")) {
                return false;
            }
            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '95%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Update Price Matrix - " + result.Item,
                show: 'slide'
            });
            PriceMatrixItem.push({
                Id: result.Id,
                CompanyId: result.CompanyId,
                ItemId: parseInt(result.ItemId, 10),
                Price: parseFloat(result.Price)
            });
            $("#ItemForPurchase").hide();
            $("#ContentPlaceHolder1_txtCompany").val(result.Company).prop('disabled', true);
            $("#ContentPlaceHolder1_hfId").val(result.Id);
            $("#ContentPlaceHolder1_hfCompanyId").val(result.CompanyId);
            $("#ContentPlaceHolder1_ddlCategory").val(result.CategoryId).trigger('change').prop('disabled', true);
            $("#ContentPlaceHolder1_txtItem").val(result.Item).prop('disabled', true);
            $("#ContentPlaceHolder1_lblCurrentStockBy").text(result.UnitHead);
            $("#ContentPlaceHolder1_txtPurchasePrice").val(result.Price);
            if (result.Remarks != ', ')
                $("#ContentPlaceHolder1_lblItemWiseRemarks").text(result.Remarks);
            $("#btnSave").val("Update & Close");
            $("#btnSaveNContinue").hide();
            $("#btnClear").hide();
            $("#btnAdd").hide();
            $("#btnCancelOrder").hide();
            return false;
        }
        function OnFailLoad(error) {
            toastr.error(error.get_message());
        }
        function PerformDelete(id) {
            if (confirm("Want to delete?")) {
                PageMethods.PerformDelete(id, OnSuccessDelete, OnFailedDelete);
            }
            return false;
        }
        function OnSuccessDelete(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GridPaging(1, 1);
            }
            return false;
        }
        function OnFailedDelete(error) {
            toastr.error(error.get_message());
            return false;
        }
        function SaveNContinue() {
            isContinueSave = true;
            SaveOrUpdate();
            return false;
        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0" />

    <asp:HiddenField ID="hfClientId" runat="server" Value="" />

    <div class="panel panel-default">
        <div class="panel-heading">
            Support Price Matrix
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Company</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSearchCompany" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Item</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSearchItem" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
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

            <table id="tblMatrix" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div id="CreateNewDialog" style="display: none; overflow: unset" class="panel panel-default">
        <div class="panel panel-default">
            <div class="panel-heading">Company Info</div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Company</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel-heading">Item Info</div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Item"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtItem" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">

                        <div class="col-md-2">
                            <asp:Label ID="Label6" runat="server" class="control-label" Text="Stock Unit"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblCurrentStockBy" runat="server" class="form-control" Text=""></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="Label8" runat="server" class="control-label required-field" Text="Price"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtPurchasePrice" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label11" runat="server" class="control-label" Text="Remarks"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:Label ID="lblItemWiseRemarks" runat="server" Style="height: 65px;" CssClass="form-control"></asp:Label>
                        </div>
                    </div>
                </div>
                <hr />
                <div class="form-group" style="padding-top: 10px;">
                    <div class="col-md-12">
                        <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItem()" />
                        <input id="btnCancelOrder" type="button" value="Cancel" onclick="ClearItemAdded()"
                            class="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
            </div>
            <div id="AdhoqPurchaseItem" style="overflow-y: scroll;">
                <table id="ItemForPurchase" class="table table-bordered table-condensed table-hover">
                    <thead>
                        <tr>
                            <th style="width: 20%;">Category</th>
                            <th style="width: 30%;">Item</th>
                            <th style="width: 20%;">Unit Head</th>
                            <th style="width: 25%;">Unit Price</th>
                            <th style="width: 5%;">Action</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
            </div>
        </div>
        <div class="row" style="padding-bottom: 0; padding-top: 10px;">
            <div class="col-md-12">
                <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="SaveOrUpdate()" value="Save & Close" />
                <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return PerformClearAction();" />
                <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                    onclick="javascript: return SaveNContinue();" />
            </div>
        </div>
    </div>
</asp:Content>
