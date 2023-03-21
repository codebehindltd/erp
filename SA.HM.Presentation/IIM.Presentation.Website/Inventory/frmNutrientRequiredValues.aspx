<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmNutrientRequiredValues.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmNutrientRequiredValues" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            
            $("#ContentPlaceHolder1_ddlItemName").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlNutrient").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            
            $("#ContentPlaceHolder1_ddlItemNameSearch").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#myTabs").tabs();
        });       

        function AddNutrientRequiredValue() {
            if ($("#ContentPlaceHolder1_ddlNutrient").val() == "0") {
                toastr.warning("Please Select Nutrient.");
                $("#ContentPlaceHolder1_ddlNutrient").focus();
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtRequiredValue").val() == "") {
                toastr.warning("Please Give Required Value.");
                $("#ContentPlaceHolder1_txtRequiredValue").focus();
                return false;
            }
            AddNutrientRequiredValueTable();
            //ClearAfterNutrientRequiredValueAdded();
        }

        function AddNutrientRequiredValueTable() {
            var nutrientName = $("#ContentPlaceHolder1_ddlNutrient option:selected").text();
            var nutrientId = $("#ContentPlaceHolder1_ddlNutrient option:selected").val();
            var requiredValue = $("#ContentPlaceHolder1_txtRequiredValue").val();

            if (!IsNutrientRequiredValueExists(nutrientId)) {
                if ($("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val() == 0) {
                    var tr = "";

                    tr += "<tr>";
                    tr += "<td style='width:40%;'>" + nutrientName + "</td>";
                    tr += "<td style='width:40%;'>" + requiredValue + "</td>";
                    tr += "<td style=\"width:15%;\">";
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditNutrientRequiredValueItem('" + nutrientId + "','" + requiredValue + "')\" alt='Edit'  title='Edit' border='0' />";
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'DeleteNutrientRequiredValueItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                    tr += "</td>";

                    tr += "<td style='display:none;'>" + nutrientId + "</td>";

                    tr += "</tr>";

                    $("#NutrientRequiredValuesTbl tbody").prepend(tr);
                    tr = "";
                    ClearAfterNutrientRequiredValueAdded();
                }
            }
            else {
                $("#NutrientRequiredValuesTbl tr").each(function () {
                    var currentNutrientId = $(this).find("td").eq(3).html();
                    if ($("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val() == 1) {
                        if (currentNutrientId == nutrientId) {
                            $(this).find("td").eq(0).html(nutrientName);
                            $(this).find("td").eq(1).html(requiredValue);
                            $(this).find("td").eq(3).html(nutrientId);
                        }
                    }
                });
                if ($("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val() == 1) {
                    ClearAfterNutrientRequiredValueAdded();
                }
            }
        }

        function ClearAfterNutrientRequiredValueAdded() {
            $("#ContentPlaceHolder1_ddlNutrient").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtRequiredValue").val("");
            $("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val(0);
        }

        function IsNutrientRequiredValueExists(nutrientId) {
            var IsDuplicate = false;
            $("#NutrientRequiredValuesTbl tr").each(function (index) {
                
                if (index !== 0 && !IsDuplicate) {
                    var nutrientIdValueInTable = $(this).find("td").eq(3).html();
                    nutrientIdValueInTable = parseInt(nutrientIdValueInTable);
                    nutrientId = parseInt(nutrientId);
                    var IsNutrientIdFound;
                    if (nutrientId == nutrientIdValueInTable) {
                        IsNutrientIdFound = true;
                    }
                    else {
                        IsNutrientIdFound = false;
                    }
                    if (IsNutrientIdFound) {
                        if ($("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val() == 1) {
                            toastr.success('Nutrient Required Value Updated Successfully.');
                            IsDuplicate = true;
                        }
                        else {
                            toastr.warning('Nutrient Required Value Already Added.');
                            IsDuplicate = true;
                            return true;
                        }
                    }
                }
            });
            return IsDuplicate;
        }

        function ValidationBeforeSave() {
            if ($("#ContentPlaceHolder1_ddlItemName").val() == "0") {
                toastr.warning("Please Select Item Name.");
                $("#ContentPlaceHolder1_ddlItemName").focus();
                return false;
            }

            var rowCountAT = $('#NutrientRequiredValuesTbl tbody tr').length;
            if (rowCountAT == 0) {
                toastr.warning('Add at least one Nutrient Required Value.');
                $("#ContentPlaceHolder1_ddlNutrient").focus();
                return false;
            }

            var itemId = "0", itemName = "", nutrientId = "0", nutrientName = "", requiredValue = 0;
            itemId = $("#ContentPlaceHolder1_ddlItemName option:selected").val();
            itemName = $("#ContentPlaceHolder1_ddlItemName option:selected").text();
            var id = $("#ContentPlaceHolder1_hfNRVMasterId").val();

            var nutrientRequiredMasterInfo = {
                Id: id,
                ItemId: itemId,
                ItemName: itemName
            }

            var AddedNutrientRequiredValueInfo = [], EditNutrientRequiredValueInfo = [];

            $("#NutrientRequiredValuesTbl tbody tr").each(function (index, item) {

                nutrientName = $.trim($(item).find("td:eq(0)").text());
                requiredValue = $.trim($(item).find("td:eq(1)").text());
                nutrientId = $.trim($(item).find("td:eq(3)").text());

                AddedNutrientRequiredValueInfo.push({
                    NutrientId: nutrientId,
                    NutrientName: nutrientName,
                    RequiredValue: requiredValue
                });
            });

            PageMethods.SaveNutrientRequiredValues(nutrientRequiredMasterInfo, AddedNutrientRequiredValueInfo, deletedNutrientRequiredValueList, OnSaveNutrientRequiredValuesSucceeded, OnSaveNutrientRequiredValuesFailed);

            return false;
        }
        function OnSaveNutrientRequiredValuesSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                deletedNutrientRequiredValueList = [];
                $("#btnSave").val("Save");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnSaveNutrientRequiredValuesFailed(error) {
            toastr.error(error.get_message());
        }
                
        function EditNutrientRequiredValueItem(nutrientId, requiredValue) {
            if (!confirm("Do you want to edit item?")) {
                return false;
            }
            $("#ContentPlaceHolder1_hfEditNutrientRequiredValue").val(1);
            $("#ContentPlaceHolder1_ddlNutrient").val(nutrientId).trigger('change');
            $("#ContentPlaceHolder1_txtRequiredValue").val(requiredValue);
        }
        var deletedNutrientRequiredValueList = [];
        function DeleteNutrientRequiredValueItem(control) {
            if (!confirm("Do you want to delete this item?")) { return false; }

            var tr = $(control).parent().parent();
            let nutrientId = $(tr).find("td").eq(3).html();
            deletedNutrientRequiredValueList.push(parseInt(nutrientId, 10));
            $(tr).remove();
        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_hfNRVMasterId").val(0);
            $("#ContentPlaceHolder1_ddlItemName").val("0").trigger('change');
            $("#NutrientRequiredValuesTbl tbody").html("");
            $("#btnSave").val("Save");
            ClearAfterNutrientRequiredValueAdded();
        }
        function PerformClearActionWithConfirmation() {

            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        function SearchNutrientRequiredValues(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#NutrientRequiredValuesGrid tbody tr").length;
            var itemId = $("#ContentPlaceHolder1_ddlItemNameSearch option:selected").val();
            var itemName = $("#ContentPlaceHolder1_ddlItemNameSearch option:selected").text();

            $("#GridPagingContainer ul").html("");
            $("#NutrientRequiredValuesGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchNutrientRequiredValues(itemId,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchNutrientRequiredValuesSucceed, OnSearchNutrientRequiredValuesFailed);

            return false;
        }
        
        function OnSearchNutrientRequiredValuesSucceed(result) {
            var tr = "";
            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:70%;'>" + gridObject.ItemName + "</td>";

                tr += "<td style=\"text-align: center; width:30%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return NutrientRequiredValuesEditWithConfirmation(" + gridObject.Id + ")\" alt='Edit'  title='Edit' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return NutrientRequiredValuesDelete(" + gridObject.Id + ")\" alt='Delete'  title='Delete' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.ItemId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.Id + "</td>";

                tr += "</tr>";

                $("#NutrientRequiredValuesGrid tbody").append(tr);

                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSearchNutrientRequiredValuesFailed() {

        }

        
        function NutrientRequiredValuesEditWithConfirmation(Id) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            NutrientRequiredValuesEdit(Id);
        }
        function NutrientRequiredValuesEdit(Id) {

            PageMethods.NutrientRequiredValuesEdit(Id, OnNutrientRequiredValuesEditSucceed, OnNutrientRequiredValuesEditFailed);
            return false;
        }
        function OnNutrientRequiredValuesEditSucceed(result) {
            
            $("#btnSave").val("Update");
            $("#ContentPlaceHolder1_hfNRVMasterId").val(result.NRVMasterInfo.Id);
            $("#NutrientRequiredValuesTbl tbody").html("");

            $("#ContentPlaceHolder1_ddlItemName").val(result.NRVMasterInfo.ItemId).trigger('change');

            NutrientRequiredValueEdit(result.NRVDetails);

            $("#myTabs").tabs({ active: 0 });
        }
        function OnNutrientRequiredValuesEditFailed() { }

        function NutrientRequiredValueEdit(result) {
            $.each(result, function (count, obj) {

                var tr = "";

                tr += "<tr>";
                tr += "<td style='width:40%;'>" + obj.NutrientName + "</td>";
                tr += "<td style='width:40%;'>" + obj.RequiredValue + "</td>";
                tr += "<td style=\"width:20%;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditNutrientRequiredValueItem('" + obj.NutrientId + "','" + obj.RequiredValue + "')\" alt='Edit'  title='Edit' border='0' />";
                tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'DeleteNutrientRequiredValueItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.NutrientId + "</td>";
                tr += "<td style='display:none;'>" + obj.Id + "</td>";
                tr += "<td style='display:none;'>" + obj.CalculatedValue + "</td>";
                tr += "<td style='display:none;'>" + obj.Difference + "</td>";

                tr += "</tr>";

                $("#NutrientRequiredValuesTbl tbody").prepend(tr);

                tr = "";
            });
        }
        function ClearSearch() {
            $("#ContentPlaceHolder1_ddlItemNameSearch").val("0").trigger('change');
        }

        function NutrientRequiredValuesDelete(TicketId) {

            if (!confirm("Do you Want To Delete?")) {
                return false;
            }

            PageMethods.NutrientRequiredValuesDelete(TicketId, OnNutrientRequiredValuesDeleteSucceed, OnNutrientRequiredValuesDeleteFailed);
        }

        function OnNutrientRequiredValuesDeleteSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchNutrientRequiredValues(1, 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnNutrientRequiredValuesDeleteFailed() {
            toastr.error(error.get_message());
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchNutrientRequiredValues(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }

        function GoToAirlineTicketInfoPage() {
            window.location = "/AirTicketing/frmAirlineTicketInfo.aspx";
            return false;
        }
    </script>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanySearchId" runat="server" Value="0" />
    <asp:HiddenField ID="hfReferenceIdForCompany" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfReferenceIdForWalkIn" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfbankId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfRegistrationNumber" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfNRVMasterId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfEditNutrientRequiredValue" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfStopAddingExistingPayment" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hftotalForPaymentInfos" runat="server" Value="0" />
    <asp:HiddenField ID="hftotalForTicketInfos" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Nutrient Required Values</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Nutrient Required Values</a></li>
        </ul>
        <div id="tab-1">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblItemName" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlItemName" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div id="NutrientRequiredValue" class="childDivSection">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Nutrient Required Value
                    </div>
                    <div class="panel-body childDivSectionDivBlockBody">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNutrient" runat="server" class="control-label required-field" Text="Nutrient"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlNutrient" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblRequiredValue" runat="server" class="control-label required-field" Text="Required Value"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtRequiredValue" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="padding-left: 10px;">
                                <input id="btnAddNutrientRequiredValue" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddNutrientRequiredValue()" />
                                <input id="btnCancelPayment" type="button" value="Cancel" onclick="ClearAfterNutrientRequiredValueAdded()" class="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                            <div id="NutrientRequiredValues" style="overflow-y: scroll;">
                                <table id="NutrientRequiredValuesTbl" class="table table-bordered table-condensed table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width: 40%;">Nutrient</th>
                                            <th style="width: 40%;">Required Value</th>
                                            <th style="width: 20%;">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                    <tfoot></tfoot>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group" style="padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" value="Save" onclick="ValidationBeforeSave()"
                        class="TransactionalButton btn btn-primary btn-sm" />
                    <input id="btnCancelTicket" type="button" value="Cancel" onclick="PerformClearActionWithConfirmation()"
                        class="TransactionalButton btn btn-primary btn-sm" />
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Nutrient Required Values
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblItemNameSearch" runat="server" class="control-label" Text="Item Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlItemNameSearch" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchNutrientRequiredValues(1, 1)" />
                                <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="ClearSearch()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Div1" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <table id="NutrientRequiredValuesGrid" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th style="width: 70%;">Item Name
                                </th>
                                <th style="width: 30%;">Action
                                </th>
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
    </div>
</asp:Content>
