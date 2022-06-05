﻿<%@ Page Title="Item Consumption Information" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="PurchaseReturnInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.PurchaseReturnInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            if (IsCanSave) {
                $('#btnApprove').show();
            } else {
                $('#btnApprove').hide();
            }

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

            if (Cookies.getJSON('receivereturn') != undefined) {
                var SearchOption = Cookies.getJSON('receivereturn');

                $("#ContentPlaceHolder1_txtReturnNumber").val(SearchOption.ReturnNumber);
                $("#ContentPlaceHolder1_ddlStatus").val(SearchOption.Status);
                $("#ContentPlaceHolder1_txtFromDate").val(SearchOption.FromDate);
                $("#ContentPlaceHolder1_txtToDate").val(SearchOption.ToDate);

                var pageIndex = SearchOption.PageIndex;

                Cookies.remove('receivereturn');
                GetReceivedOrderReturnForSearch(pageIndex, 1);
            }

        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            GetReceivedOrderReturnForSearch(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }

        function GetReceivedOrderReturnForSearch(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#OutOrderSearchGrid tbody tr").length;
            var returnNumber = "0", status = "", returnType = "", fromDate = null, toDate = null;

            returnNumber = $("#ContentPlaceHolder1_txtReturnNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            $("#GridPagingContainer ul").html("");
            $("#OutOrderSearchGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchReturnOrder(fromDate, toDate, returnNumber, status,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

            return false;
        }
        function OnSearchPurchaseOrderSucceed(result) {

            var tr = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.ReturnNumber + "</td>";

                if (gridObject.ReturnDate != null)
                    tr += "<td style='width:10%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.ReturnDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:10%;'></td>";

                tr += "<td style='width:15%;'>" + gridObject.CostCenter + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.LocationName + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.Status + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:15%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:15%;'></td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";

                if (gridObject.IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditItemReturn(" + gridObject.ReturnId + "," + gridObject.ReceivedId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return ReceiveReturnDelete(" + gridObject.ReturnId + "," + gridObject.ReceivedId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return ReceiveReturnApproval('" + 'Checked' + "'," + gridObject.ReturnId + "," + gridObject.ReceivedId + ")\" alt='Checked'  title='Checked' border='0' />";
                }

                if (gridObject.IsCanApproved) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return ReceiveReturnApproval('" + 'Approved' + "', " + gridObject.ReturnId + "," + gridObject.ReceivedId + ")\" alt='Approved'  title='Approved' border='0' />";
                }

                //if (gridObject.Status == 'Approved') {
                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport(" + gridObject.ReturnId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Invoice' title='Receive Return Information' border='0' />";
                //}

                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.ReturnId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.TransactionId + "</td>";

                tr += "</tr>";

                $("#OutOrderSearchGrid tbody").append(tr);
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
            $("#ContentPlaceHolder1_ddlStatus").val("All");
            $("#ContentPlaceHolder1_txtFromDate").datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_txtToDate").datepicker("setDate", DayOpenDate);
        }

        function ReceiveReturnApproval(ApprovedStatus, ReturnId, TransactionId) {
            if (!confirm("Do you Want To " + (ApprovedStatus == "Checked" ? "Check?" : "Approve?"))) {
                return false;
            }

            PageMethods.ReceiveReturnApproval(ReturnId, ApprovedStatus, TransactionId, OnApprovalSucceed, OnApprovalFailed);
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                GetReceivedOrderReturnForSearch($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }

        function ReceiveReturnDelete(ReturnId, ReceivedId, ApprovedStatus, CreatedBy) {

            if (!confirm("Do you Want To Delete?")) {
                return false;
            }

            PageMethods.ReceiveReturnDelete(ReturnId, ApprovedStatus, ReceivedId, CreatedBy, OnApprovalSucceed, OnApprovalFailed);
        }

        function NewReceiveReturnOrder() {
            window.location = "/PurchaseManagment/PurchaseReturn.aspx";
        }

        function EditItemReturn(returnId, receivedId) {

            var returnNumber = "0", fromDate = null, toDate = null, status = "";
            var pageIndex = $("#GridPagingContainer").find("li.active").index();

            if (pageIndex < 0)
                pageIndex = 1;

            returnNumber = $("#ContentPlaceHolder1_txtReturnNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();

            var SearchOption = {
                ReturnNumber: returnNumber,
                Status: status,
                FromDate: fromDate,
                ToDate: toDate,
                PageIndex: pageIndex
            };

            Cookies.set('receivereturn', SearchOption, { expires: 1 });

            var url = "/PurchaseManagment/PurchaseReturn.aspx?rid=" + returnId + "&tid=" + receivedId;
            window.location = url;
            return false;
        }
        function EditItemOutWithConfirmation(issueType, outId, requisitionOrSalesId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            EditItemOut(issueType, outId, requisitionOrSalesId);
        }

        function ShowReport(returnId, ApprovedStatus, CreatedBy) {
            var iframeid = 'printDoc';
            //var url = "../Inventory/Reports/frmReportProductOut.aspx?poOutId=" + returnId;
            var url = "Reports/ReportPMPurchaseReturn.aspx?returnId=" + returnId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Purchase Return",
                show: 'slide'
            });
        }

    </script>

    <div id="DetailsConsumptionDialog" style="display: none;">
        <div id="DetailsConsumptionGridContaiiner">
        </div>
        <div class="HMContainerRowButton" style="padding-bottom: 0; padding-top: 10px;">
            <input id="btnApprove" type="button" class="TransactionalButton btn btn-primary btn-sm"
                value="Approve Consumption" />
        </div>
    </div>
    <asp:HiddenField ID="hfConsumptionId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsConsumptionCheckedByEnable" runat="server" Value="0" />

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div class="panel panel-default">
        <div class="panel-heading">
            Return Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Return Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtReturnNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                            <asp:ListItem Text="Cancel" Value="Cancel"></asp:ListItem>
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
                        <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" value="Search" onclick="GetReceivedOrderReturnForSearch(1, 1)" />
                        <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="ClearSearch()" />
                        <input type="button" id="btnNewPurchaseInfo" class="TransactionalButton btn btn-primary btn-sm" value="New Purchase Return" onclick="NewReceiveReturnOrder()" />
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

            <table id="OutOrderSearchGrid" class="table table-bordered table-condensed table-responsive">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th style="width: 10%;">Return Number
                        </th>
                        <th style="width: 10%;">Return Date
                        </th>
                        <th style="width: 15%;">From Cost Center
                        </th>
                        <th style="width: 15%;">From location
                        </th>
                        <th style="width: 10%;">Status
                        </th>
                        <th style="width: 15%;">Remarks
                        </th>
                        <th style="width: 15%;">Action
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
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="1000" height="800" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>


</asp:Content>
