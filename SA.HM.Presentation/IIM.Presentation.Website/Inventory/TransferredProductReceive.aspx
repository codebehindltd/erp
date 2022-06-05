<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="TransferredProductReceive.aspx.cs" EnableEventValidation="false"
    Inherits="HotelManagement.Presentation.Website.PurchaseManagment.TransferredProductReceive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var TransferItemAdded = new Array();
        var AddedSerialzableProduct = new Array();
        var ReceivedOrderControl = null;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            if (IsCanSave) {
                $('#btnSave').show();
            } else {
                $('#btnSave').hide();
            }

            $("#ContentPlaceHolder1_ddlCostCenter").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_ddlCostCenter').change(function () {
                LoadLocationFrom($(this).val());
            });
            $('#ContentPlaceHolder1_ddlCostCenter').trigger('change');
        });

        function SearchReceiveOrder(pageNumber, IsCurrentOrPreviousPage) {

            $("#ItemForTransferTbl tbody").html("");

            TransferItemAdded = new Array();
            AddedSerialzableProduct = new Array();

            var gridRecordsCount = $("#OutOrderGrid tbody tr").length;
            var costCenter = "0", status = "", location = "", fromDate = null, toDate = null;

            costCenter = $("#ContentPlaceHolder1_ddlCostCenter").val();
            location = $("#ContentPlaceHolder1_ddlLocation").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            if ($("#OutOrderGrid tbody tr").length == 0) {
                if (costCenter == "0") {
                    toastr.warning("Please Select Cost Center.");
                    return false;
                }
            }
            else if (location == "0") {
                toastr.warning("Please Select Location.");
                return false;
            }

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            $("#GridPagingContainer ul").html("");
            $("#OutOrderGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchOutOrder(costCenter, location, fromDate, toDate, status,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

            return false;
        }
        function OnSearchPurchaseOrderSucceed(result) {

            var tr = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.IssueNumber + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.IssueType + "</td>";

                if (gridObject.PRNumber != null) {
                    tr += "<td style='width:10%;'>" + gridObject.PRNumber + "</td>";
                }
                else {
                    tr += "<td style='width:10%;'></td>";
                }

                if (gridObject.OutDate != null)
                    tr += "<td style='width:10%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.OutDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:10%;'></td>";

                tr += "<td style='width:10%;'>" + gridObject.FromCostCenter + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.ToCostCenter + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.ToLocation + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.Status + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:15%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:15%;'></td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/receiveitem.png' onClick= \"javascript:return ReceiveItemOut(this, '" + gridObject.ProductOutFor + "'," + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ",'" + gridObject.IssueNumber + "'" + ")\" alt='Receive'  title='Receive' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.OutId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.RequisitionOrSalesId + "</td>";

                tr += "</tr>";

                $("#OutOrderGrid tbody").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSearchPurchaseOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }
        function ClearSearch() {
            $("#ContentPlaceHolder1_txtIssueNumber").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("All");
            $("#ContentPlaceHolder1_ddlSearchProductOutFor").val("All");
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");
        }

        function LoadLocationFrom(costCenetrId) {
            PageMethods.InvLocationByCostCenter(costCenetrId, OnLoadLocationFromSucceeded, OnLoadLocationFailed);
        }
        function OnLoadLocationFromSucceeded(result) {
            var control = $('#ContentPlaceHolder1_ddlLocation');

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
            if (list.length == 1 && $("#ContentPlaceHolder1_hfLocationFromId").val() == "0")
                $("#ContentPlaceHolder1_ddlLocation").val($("#ContentPlaceHolder1_ddlLocation option:first").val());
            else
                control.val($("#ContentPlaceHolder1_hfLocationFromId").val());
            return false;
        }
        function OnLoadLocationFailed() { }

        function ReceiveItemOut(control, issueType, outId, requisitionOrSalesId, issueNumber) {

            $("#OutOrderGrid tbody tr").removeAttr("style");

            var tr = $(control).parent().parent();
            $(tr).css("background-color", "#e6ffe6");

            ReceivedOrderControl = {
                ProductOutFor: issueType,
                OutId: outId,
                RequisitionOrSalesId: requisitionOrSalesId
            };

            $("#issueNumber").text(" Of Issue No. " + issueNumber);

            PageMethods.EditItemOut(issueType, outId, requisitionOrSalesId, OnEditPurchaseOrderSucceed, OnEditPurchaseOrderFailed);
            return false;
        }
        function OnEditPurchaseOrderSucceed(result) {
            $("#ItemForTransferTbl tbody").html("");
            $("#SerialItemTable tbody").html("");
            AddedSerialzableProduct = new Array();
            TransferItemAdded = new Array();

            StockTransferOrderEdit(result);
        }
        function OnEditPurchaseOrderFailed() { }

        function StockTransferOrderEdit(result) {

            AddedSerialzableProduct = result.ProductSerialInfo;
            var tr = "";

            $.each(result.ProductOutDetails, function (count, obj) {

                tr += "<tr>";

                tr += "<td style='width:55%;'>" + obj.ItemName + "</td>";
                tr += "<td style='width:10%;'>" + obj.StockQuantity + "</td>";
                tr += "<td style='width:15%;'>" +
                    "<input type='text' disabled='disabled' value='" + obj.Quantity + "' id='pp" + obj.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)' />" +
                    "</td>";

                tr += "<td style='width:10%;'>" + obj.StockBy + "</td>";

                tr += "<td style='width:10%;'>";
                //tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                if (obj.ProductType == 'Serial Product') {
                    tr += "&nbsp;&nbsp;<a href='javascript:void()' onclick= 'SearialAddedWindow(this)' ><img alt='serial' src='../Images/serial.png' title='Serial' /></a>";
                }
                tr += "</td>";

                tr += "<td style='display:none;'>" + obj.ItemId + "</td>";
                tr += "<td style='display:none;'>0</td>";
                tr += "<td style='display:none;'>" + obj.StockById + "</td>";
                tr += "<td style='display:none;'>" + obj.Quantity + "</td>";
                tr += "<td style='display:none;'>" + obj.OutDetailsId + "</td>";

                tr += "</tr>";

                $("#ItemForTransferTbl tbody").append(tr);
                tr = "";
            });

            TransferItemAdded = result.ProductOutDetails;
            CommonHelper.ApplyDecimalValidation();

            $("#myTabs").tabs({ active: 0 });
        }

        function SearialAddedWindow(control) {
            var trr = $(control).parent().parent();

            var itemName = $(trr).find("td:eq(0)").text();
            var itemId = $(trr).find("td:eq(5)").text();
            var quantity = $(trr).find("td:eq(2)").find("input").val();

            $("#ContentPlaceHolder1_hfItemIdForSerial").val(itemId);
            $("#SerialItemTable tbody tr").remove();
            $("#SerialItemTable tbody").html("");

            if (AddedSerialzableProduct.length > 0) {
                var addedSerial = _.where(AddedSerialzableProduct, { ItemId: parseInt(itemId, 10) });
                var row = 0; rowCount = 0;
                var tr = "";

                if (addedSerial.length > 0) {
                    rowCount = addedSerial.length;

                    for (row = 0; row < rowCount; row++) {

                        tr += "<tr>";
                        tr += "<td style='width:90%;'>" + addedSerial[row].SerialNumber + "</td>";
                        tr += "<td style='width:10%; display:none;'>" +
                            "<a href='javascript:void()' onclick= 'DeleteItemSerial(this)' ><img alt='Delete' src='../Images/delete.png' /></a>" +
                            "</td>";
                        tr += "<td style='display:none;'>" + addedSerial[row].ItemId + "</td>";
                        tr += "<td style='display:none;'>" + addedSerial[row].OutSerialId + "</td>";

                        $("#SerialItemTable tbody").append(tr);
                        tr = "";
                    }
                }
            }

            $("#SerialWindow").dialog({
                width: 300,
                height: 500,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                title: "Serial Of Item: " + itemName,
                show: 'slide',
                open: function (event, ui) {
                    $('#SerialWindow').css('overflow', 'hidden');
                    $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                }
            });
        }

        function CancelAddSerial() {
            $("#SerialWindow").dialog("close");
            $("#ContentPlaceHolder1_hfItemIdForSerial").val("");
        }

        function PerformClearAction() {

            $('#ContentPlaceHolder1_ddlCostCenter').val('0').trigger('change');
            $('#ContentPlaceHolder1_ddlLocation').val('0').trigger('change');

            $("#ItemForTransferTbl tbody").html("");

            TransferItemAdded = new Array();
            AddedSerialzableProduct = new Array();
            ReceivedOrderControl = null;
        }

        function SaveItemOutOrder() {

            if ($("#OutOrderGrid tbody tr").length == 0) {
                toastr.warning("Please Search Issue Order First.");
                return false;
            }
            else if ($("#ItemForTransferTbl tbody tr").length == 0) {
                toastr.warning("Please Load Item From Issue Order.");
                return false;
            }

            var productOutFor = '', outId = 0, requisitionOrSalesId = 0;

            if (ReceivedOrderControl != null) {
                productOutFor = ReceivedOrderControl.ProductOutFor;
                outId = ReceivedOrderControl.OutId;
                requisitionOrSalesId = ReceivedOrderControl.RequisitionOrSalesId;
            }

            PageMethods.ItemReceiveOutOrder(productOutFor, outId, requisitionOrSalesId, OnSavePurchaseOrderSucceed, OnSavePurchaseOrderFailed);


            return false;
        }
        function OnSavePurchaseOrderSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchReceiveOrder($("#GridPagingContainer").find("li.active").index(), 1);
                $("#issueNumber").text("");
                // PerformClearAction();

            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSavePurchaseOrderFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }

        function PerformClearActionWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

    </script>
    <asp:HiddenField ID="hfLocationFromId" runat="server" Value="0" />
    <asp:HiddenField ID="hfItemIdForSerial" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div class="panel panel-default">
        <div class="panel-heading">
            Transfer Item Receive
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label12" runat="server" class="control-label" Text="Receive Cost Center"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCostCenter" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Location"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLocation" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" value="Search" onclick="SearchReceiveOrder(1, 1)" />
                        <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="ClearSearch()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <table id="OutOrderGrid" style="" class="table table-bordered table-condensed table-responsive">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th style="width: 10%;">Issue Number
                        </th>
                        <th style="width: 10%;">Issue Type
                        </th>
                        <th style="width: 10%;">Requisition Number
                        </th>
                        <th style="width: 10%;">Issue Date
                        </th>
                        <th style="width: 10%;">From Cost Center
                        </th>
                        <th style="width: 10%;">To Cost Center
                        </th>
                        <th style="width: 10%;">To Location
                        </th>
                        <th style="width: 10%;">Status
                        </th>
                        <th style="width: 15%;">Remarks
                        </th>
                        <th style="width: 5%;">Action
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
    <div class="panel panel-default">
        <div class="panel-heading">
            Receive Item&nbsp;<span id="issueNumber"></span>
        </div>
        <div class="panel-body">
            <div style="height: 300px; overflow-y: scroll;">
                <table id="ItemForTransferTbl" class="table table-bordered table-condensed table-hover">
                    <thead>
                        <tr>
                            <th style="width: 55%;">Item Name</th>
                            <th style="width: 10%;">Stock Quantity</th>
                            <th style="width: 15%;">Unit</th>
                            <th style="width: 10%;">Unit Head</th>
                            <th style="width: 10%;">Action</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-sm" value="Receive" onclick="SaveItemOutOrder()" />
            <input type="button" id="btnClear" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="PerformClearActionWithConfirmation()" />
        </div>
    </div>
    <div id="SerialWindow" style="display: none;">
        <div style="height: 350px; overflow-y: scroll;">
            <table id="SerialItemTable" class="table table-bordered table-hover table-condensed">
                <thead>
                    <tr>
                        <th style="width: 90%;">Serial Number</th>
                        <th style="width: 10%; display: none;">Action</th>
                        <th style="display: none;">Item Id</th>
                        <th style="display: none;">SerialId</th>
                        <th style="display: none;">Out SerialId</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
        <div class="row">
            <div class="col-md-12">
                <input type="button" class="btn btn-primary" style="padding-left: 30px; padding-right: 30px;" value="Ok" onclick="CancelAddSerial()" />
            </div>
        </div>
    </div>
</asp:Content>
