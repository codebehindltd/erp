<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="RetailPosItemReturnToStoreInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.RetailPosItemReturnToStoreInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            $("#myTabs").tabs();

            $('#ContentPlaceHolder1_txtDateFrom').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtDateTo').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtDateTo').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtDateFrom').datepicker("option", "maxDate", selectedDate);
                }
            });

            
            $("[id=returnCheck]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#tblReturnItemDetails tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#tblReturnItemDetails tbody tr").find("td:eq(0)").find("input").prop("checked", false);
                }
            });

        });

        function SearchBills() {
            
            var billNumber = "0", fromDate = null, toDate = null;

            billNumber = $("#ContentPlaceHolder1_txtBillNumber").val();
            fromDate = $("#ContentPlaceHolder1_txtDateFrom").val();
            toDate = $("#ContentPlaceHolder1_txtDateTo").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(fromDate, innBoarDateFormat);
            else
                fromDate = null;
            if (toDate != "")
                toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(toDate, innBoarDateFormat);
            else
                toDate = null;

            if (fromDate == null && toDate != null) {
                fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(DayOpenDate, innBoarDateFormat);
                $("#ContentPlaceHolder1_txtDateFrom").val(DayOpenDate);
            }
            else if (fromDate != null && toDate == null) {
                toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(DayOpenDate, innBoarDateFormat);
                $("#ContentPlaceHolder1_txtDateTo").val(DayOpenDate);
            }
            else if (fromDate == null && toDate == null) {
                fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(DayOpenDate, innBoarDateFormat);
                toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(DayOpenDate, innBoarDateFormat);
            }

            CommonHelper.SpinnerOpen();
            PageMethods.GetBills(fromDate, toDate, billNumber, OnSearchBillSucceed, OnSearchBillFailed);

            return false;

        }

        function OnSearchBillSucceed(result) {
            var tr = "";

            $("#tblReturnInformation tbody").html(tr);

            if (result.length == 0) {
                tr += "<tr><td colspan='8'>No Record Found</td></tr>";
                $("#tblReturnInformation tbody").append(tr);
                CommonHelper.SpinnerClose();
                return false;
            }

            $.each(result, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:20%;'>" + gridObject.BillNumber + "</td>";
                tr += "<td style='width:25%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.ReturnDate, innBoarDateFormat) + "</td>";
                tr += "<td style='width:25%;'>" + gridObject.CostCenter + "</td>";
                tr += "<td style='width:25%;'>" + gridObject.Location + "</td>";

                tr += "<td style=\"text-align: center; width:5%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/receiveitem.png' onClick= \"javascript:return GetReturnItemDetails(this, " + gridObject.BillId + "," + gridObject.CostCenterId + "," + gridObject.LocationId + ")\" alt='Return Details'  title='Return Details' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.BillId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.CostCenterId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.LocationId + "</td>";

                tr += "</tr>";

                $("#tblReturnInformation tbody").append(tr);
                tr = "";
            });

            CommonHelper.SpinnerClose();
            return false;
        }

        function OnSearchBillFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }

        function GetReturnItemDetails(control, billId, costCenterId, locationId) {

            $("#ContentPlaceHolder1_hfCostCenterId").val(costCenterId);
            $("#ContentPlaceHolder1_hfLocationId").val(locationId);

            $("#tblReturnInformation tbody tr").removeAttr("style");

            var tr = $(control).parent().parent();
            $(tr).css("background-color", "#e6ffe6");

            $("#tblReturnItemDetails tbody").html("");

            PageMethods.GetReturnItemDetails(billId, OnLoadReturnItemsSucceed, OnLoadReturnItemsFailed);
            return false;
        }
        function OnLoadReturnItemsSucceed(result) {

            if (result.length == 0) {

                tr += "<tr><td colspan='6'>No Record Found</td></tr>";
                $("#tblReturnItemDetails tbody").append(tr);
                return false;
            }

            var tr = "";

            $.each(result, function (count, gridObject) {

                tr += "<tr>";

                if (gridObject.RemainingQuantity > 0) { //ReturnQuantity
                    tr += "<td style='width:5%; text-align: center;'>" +
                        "<input type='checkbox' checked='checked' id='chk" + gridObject.ItemId + "'" + " />" +
                        "</td>";
                }
                else {
                    tr += "<td style='width:5%;'></td>";
                }

                tr += "<td style='width:35%;'>" + gridObject.ItemName + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.StockBy + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.RemainingQuantity + "</td>";

                tr += "<td style='width:25%;'>" +
                    "<input type='text' value='" + gridObject.RemainingQuantity + "' id='pp" + gridObject.ItemId + "' class='form-control quantitydecimal' onblur='CheckTotalQuantity(this)' />" +
                    "</td>";

                tr += "<td style='display:none;'>" + gridObject.ReturnId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.BillId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ItemId + "</td>";

                tr += "</tr>";

                $("#tblReturnItemDetails tbody").append(tr);
                tr = "";
            });
            CommonHelper.ApplyDecimalValidation();
            //$("#hfTransactionId").val(result.ProductReceivedDetails[0].ReceivedId);
        }
        function OnLoadReturnItemsFailed() { }


        function CheckTotalQuantity(control) {
            
            var tr = $(control).parent().parent();
            
            var returnQuantity = $.trim($(tr).find("td:eq(3)").text());
            var inputedQuantity = $.trim($(tr).find("td:eq(4)").find("input").val());
            

            if (inputedQuantity == "" || inputedQuantity == "0") {
                toastr.info("Quantity Cannot Be Zero Or Empty.");
                $(tr).find("td:eq(4)").find("input").val(returnQuantity);
                return false;
            }
            else if (parseFloat(inputedQuantity) > parseFloat(returnQuantity)) {
                toastr.info("Return Quantity Cannot Greater Than Remaining Return Quantity.");
                $(tr).find("td:eq(4)").find("input").val(returnQuantity);
                return false;
            }

        }

        function ReturnItemToStore() {
            var returnItemsList = new Array();
            var returnId, returnQuantity, costCenterId, locationId, itemId, billId;

            costCenterId = parseInt($("#ContentPlaceHolder1_hfCostCenterId").val());
            locationId = parseInt($("#ContentPlaceHolder1_hfLocationId").val());
            

            $("#tblReturnItemDetails tbody tr").each(function () {

                returnId = parseInt($(this).find("td:eq(5)").text(), 10);
                returnQuantity = parseFloat($(this).find("td:eq(4)").find("input").val());
                billId = parseInt($(this).find("td:eq(6)").text(), 10);
                itemId = parseInt($(this).find("td:eq(7)").text(), 10);
                
                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {

                    returnItemsList.push({
                        ReturnId: returnId,
                        ReturnedUnit: returnQuantity,
                        CostCenterId:costCenterId,
                        LocationId: locationId,
                        ItemId: itemId,
                        BillId: billId
                    });
                }

            });

            PageMethods.SaveItemReturn(returnItemsList, OnSaveItemReturnSucceed, OnSaveItemReturnFailed);

            return false;
        }

        function OnSaveItemReturnSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                SearchBills();
                ClearAfterReturnToStore();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSaveItemReturnFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error.get_message());
        }

        function PerformClear() {
            ClearAfterReturnToStore();
            $("#tblReturnInformation tbody").html("");
            
            $("#ContentPlaceHolder1_txtBillNumber").val('');

            $("#ContentPlaceHolder1_txtDateFrom").val('');
            $("#ContentPlaceHolder1_txtDateTo").val('');

        }

        function ClearAfterReturnToStore() {

            $("#ContentPlaceHolder1_hfCostCenterId").val('');
            $("#ContentPlaceHolder1_hfLocationId").val('');            
            $("#tblReturnItemDetails tbody").html("");

        }
    </script>
    <asp:HiddenField ID="hfCostCenterId" runat="server" />
    <asp:HiddenField ID="hfLocationId" runat="server" />
    <div class="panel panel-default">
        <div class="panel panel-default">
            <div class="panel-heading">
                Search Return
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Bill Number</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtBillNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">From Date</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDateFrom" CssClass="form-control" runat="server" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">To Date</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDateTo" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <input type="button" id="btnReturnSearch" class="TransactionalButton btn btn-primary btn-sm" value="Search" onclick="SearchBills()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Return Information
            </div>
            <div class="panel-body">
                <table id="tblReturnInformation" class="table table-bordered table-condensed table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <th style="width: 20%;">Bill Number
                            </th>
                            <th style="width: 25%;">Return Date
                            </th>
                            <th style="width: 25%;">Store
                            </th>
                            <th style="width: 25%;">Location
                            </th>
                            <th style="width: 5%;">Action
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Return Item Information
            </div>
            <div class="panel-body">
                <table id="tblReturnItemDetails" class="table table-bordered table-condensed table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <th style="width: 5%; text-align: center;">
                                <input type="checkbox" value="" checked="checked" id="returnCheck" />
                            </th>
                            <th style="width: 35%;">Item Name
                            </th>
                            <th style="width: 15%;">Stock By
                            </th>
                            <th style="width: 20%;">Remaining Return Quantity
                            </th>
                            <th style="width: 25%;">Return To Store Quantity
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="form-horizontal">
            <div class="row" style="padding-top: 10px;">
                <div class="col-md-12">
                    <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-sm" value="Return To Store" onclick="ReturnItemToStore()" />
                    <input type="button" id="btnClear" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="PerformClear()" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
