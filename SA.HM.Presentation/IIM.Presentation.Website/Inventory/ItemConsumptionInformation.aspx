<%@ Page Title="Item Consumption Information" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ItemConsumptionInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.ItemConsumptionInformation" %>

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

            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });
            if (IsCanSave) {
                $('#btnApprove').show();
            } else {
                $('#btnApprove').hide();
            }
            $("#ContentPlaceHolder1_txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });

            if (Cookies.getJSON('consumption') != undefined) {
                var SearchOption = Cookies.getJSON('consumption');

                $("#ContentPlaceHolder1_ddlStatus").val(SearchOption.Status);
                $("#ContentPlaceHolder1_ddlIssueType").val(SearchOption.IssueType);
                $("#ContentPlaceHolder1_txtFromDate").val(SearchOption.FromDate);
                $("#ContentPlaceHolder1_txtToDate").val(SearchOption.ToDate);

                var pageIndex = SearchOption.PageIndex;

                Cookies.remove('consumption');
                LoadConsumptionDetails(pageIndex, 1);
            }

        });

        function LoadConsumptionDetails(pageNumber, IsCurrentOrPreviousPage) {
            CommonHelper.SpinnerOpen();

            var gridRecordsCount = $("#tbConsumptionDetails tbody tr").length;
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            var status = $("#ContentPlaceHolder1_ddlStatus").val();
            var issueType = $("#ContentPlaceHolder1_ddlIssueType").val();
            var issueNumber = "";

            if (fromDate == "") {
                $("#ContentPlaceHolder1_txtFromDate").val(GetStringFromDateTime(new Date));
                fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(GetStringFromDateTime(new Date()), innBoarDateFormat);
            }
            else {
                fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(fromDate, innBoarDateFormat);
            }

            if (toDate == "") {
                $("#ContentPlaceHolder1_txtToDate").val(GetStringFromDateTime(new Date));
                toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(GetStringFromDateTime(new Date()), innBoarDateFormat);
            }
            else {
                toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(toDate, innBoarDateFormat);
            }
            PageMethods.GetConsumptionDetails(fromDate, toDate, status, issueType, issueNumber, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnsuccessLoading, OnfailLoading);
            return false;
        }
        function OnsuccessLoading(result) {
            CommonHelper.SpinnerClose();
            $("#tbConsumptionDetails tbody tr").remove();
            $("#GridPagingContainer ul").html("");
            var tr = "", totalRow = 0, editLink = "", deleteLink = "", invoiceLink = "", approvalLink = "", infoLink = "";

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"8\" >No Data Found</td> </tr>";
                $("#tbConsumptionDetails tbody ").append(emptyTr);
                return false;
            }

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tbConsumptionDetails tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:12%;\">" + gridObject.IssueNumber + "</td>";
                tr += "<td align='left' style=\"width:10%;\">" + GetStringFromDateTime(gridObject.OutDate) + "</td>";
                tr += "<td align='left' style=\"width:13%\">" + gridObject.IssueType + "</td>";
                tr += "<td align='left' style=\"width:10%\">" + gridObject.FromCostCenter + "</td>";
                tr += "<td align='left' style=\"width:12%\">" + gridObject.ToCostCenter + "</td>";
                tr += "<td align='left' style=\"width:14%\">" + gridObject.CreatedByName + "</td>";
                tr += "<td align='left' style=\"width:10%\">" + gridObject.Status + "</td>";

                if (gridObject.IsCanEdit && IsCanEdit)
                    editLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return EditItemOutWithConfirmation('" + gridObject.IssueType + "'," + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\"> <img alt=\"Edit\" src=\"../Images/edit.png\" /> </a>";


                if (gridObject.IsCanDelete && IsCanDelete)
                    deleteLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ConfirmDeleteOrCancel('" + gridObject.OutId + "');\"><img alt=\"Cancel\" src=\"../Images/delete.png\" /></a>";


                if (gridObject.IsCanChecked && IsCanSave) {
                    approvalLink += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return OutOrderApprovalWithConfirmation('" + gridObject.ProductOutFor + "','" + 'Checked' + "'," + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Check'  title='Check' border='0' />";
                }


                if (gridObject.IsCanApproved && IsCanSave) {
                    approvalLink += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return OutOrderApprovalWithConfirmation('" + gridObject.ProductOutFor + "','" + 'Approved' + "', " + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Approve'  title='Approve' border='0' />";
                }

                invoiceLink = "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ProductOutDetails('" + gridObject.OutId + "');\"><img alt=\"Invoice\" src=\"../Images/detailsInfo.png\" /></a>";

                infoLink = "&nbsp;&nbsp;<a href=\"javascript:void();\"  onclick=\"javascript:return ProductConsumptionReport('" + gridObject.OutId + "', 'cm');\"><img alt=\"Consumption\" src=\"../Images/ReportDocument.png\" title='Consumption Info'/></a>";

                if ($('#ContentPlaceHolder1_hfIsItemConsumptionDeliveryChallanEnable').val() == "1") {
                    infoLink = infoLink + "&nbsp;&nbsp;<a href=\"javascript:void();\"  onclick=\"javascript:return ProductConsumptionReport('" + gridObject.OutId + "', 'cn');\"><img alt=\"Delivery Challan\" src=\"../Images/ReportDocument.png\" title='Delivery Challan'/></a>";
                }

                tr += "<td align='center' style=\"width:19%;\">" + editLink + deleteLink + approvalLink + invoiceLink + infoLink + "</td>";

                tr += "</tr>";

                $("#tbConsumptionDetails tbody").append(tr);
                tr = "";
                editLink = "";
                deleteLink = "";
                approvalLink = "";

            });
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


        }
        function OnfailLoading() { CommonHelper.SpinnerClose(); }

        function OutOrderApproval(ProductOutFor, ApprovedStatus, OutId, RequisitionOrSalesId) {

            CommonHelper.SpinnerOpen();
            PageMethods.OutOrderApproval(ProductOutFor, ApprovedStatus, OutId, RequisitionOrSalesId, OnProductOutDetailsForApproveLoadSucceeded, OnProductOutDetailsForApproveLoadFailed);
            return false;
        }

        function OutOrderApprovalWithConfirmation(ProductOutFor, ApprovedStatus, OutId, RequisitionOrSalesId, OnProductOutDetailsForApproveLoadSucceeded, OnProductOutDetailsForApproveLoadFailed) {
            if (!confirm("Do you Want To " + (ApprovedStatus == 'Checked' ? 'Check' : 'Aprrove') + "?")) {
                return false;
            }
            OutOrderApproval(ProductOutFor, OutId, ApprovedStatus, RequisitionOrSalesId, OnProductOutDetailsForApproveLoadSucceeded, OnProductOutDetailsForApproveLoadFailed);
        }
        function OnProductOutDetailsForApproveLoadSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                LoadConsumptionDetails($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
        }
        function OnProductOutDetailsForApproveLoadFailed() {
            CommonHelper.SpinnerClose();
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadConsumptionDetails(pageNumber, IsCurrentOrPreviousPage);
        }

        function NewOutOrder() {
            window.location = "/Inventory/frmItemConsumption.aspx";
        }

        function ConfirmDeleteOrCancel(outId) {
            var status = confirm("Want to cancel consumption???");
            if (status) {
                PageMethods.CancelConsumption(outId, OnsuccessCancel, OnfailCancel);
            }
            return false;
        }
        function OnsuccessCancel(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#ContentPlaceHolder1_hfRequisitionId").val("0");
                LoadConsumptionDetails($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnfailCancel(error) {
            toastr.error(error.get_message());
        }

        function ProductConsumptionReport(outId, rType) {

            var iframeid = 'printDoc';
            var url = "Reports/frmReportProductConsumption.aspx?poOutId=" + outId + '&rType=' + rType;
            document.getElementById(iframeid).src = url;

            if (rType == "cn") {
                $("#displayBill").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 900,
                    height: 600,
                    closeOnEscape: false,
                    resizable: false,
                    fluid: true,
                    title: "Delivery Challan",
                    show: 'slide'
                });
            }
            else {
                $("#displayBill").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 900,
                    height: 600,
                    closeOnEscape: false,
                    resizable: false,
                    fluid: true,
                    title: "Item Consumption",
                    show: 'slide'
                });
            }
        }

        function ProductOutDetails(outId) {
            CommonHelper.SpinnerOpen();
            PageMethods.GetProductOutDetails(outId, OnProductOutDetailsLoadSucceeded, OnSaveProductOutDetailsLoadFailed);
            return false;
        }
        function OnProductOutDetailsLoadSucceeded(result) {
            $("#DetailsOutGrid tbody").html("");
            var totalRow = result.length, row = 0;

            var tr = "";

            for (row = 0; row < totalRow; row++) {

                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:25%;'>" + result[row].ItemName + "</td>";
                tr += "<td style='width:10%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:15%;'>" + result[row].StockBy + "</td>";
                tr += "<td style='width:10%;'>" + result[row].SerialNumber + "</td>";
                tr += "</tr>";

                $("#DetailsOutGrid tbody").append(tr);
                tr = "";
            }

            $("#DetailsOutGridContaiiner").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 800,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Item Details",
                show: 'slide'
            });
            CommonHelper.SpinnerClose();
        }
        function OnSaveProductOutDetailsLoadFailed(error) {
            CommonHelper.SpinnerClose();
        }

        function EditItemOut(issueType, outId, requisitionOrSalesId) {

            var productOutFor = '', issueNumber = "0", fromDate = null, toDate = null, status = "";
            var pageIndex = $("#GridPagingContainer").find("li.active").index();

            if (pageIndex < 0)
                pageIndex = 1;

            //issueNumber = $("#ContentPlaceHolder1_txtIssueNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            productOutFor = $("#ContentPlaceHolder1_ddlIssueType").val();

            var SearchOption = {
                //IssueNumber: issueNumber,
                Status: status,
                IssueType: productOutFor,
                FromDate: fromDate,
                ToDate: toDate,
                PageIndex: pageIndex
            };

            Cookies.set('consumption', SearchOption, { expires: 1 });

            var url = "/Inventory/frmItemConsumption.aspx?it=" + issueType + "&oid=" + outId;
            window.location = url;
            return false;
        }
        function EditItemOutWithConfirmation(issueType, outId, requisitionOrSalesId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            EditItemOut(issueType, outId, requisitionOrSalesId);
        }

        function ClearSearch() {
            //$("#ContentPlaceHolder1_txtIssueNumber").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("");
            $("#ContentPlaceHolder1_ddlIssueType").val("");
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");
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
    <asp:HiddenField ID="hfIsItemConsumptionDeliveryChallanEnable" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="1000" height="800" frameborder="0" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Consumption Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label CssClass="control-label" runat="server" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control" ID="txtFromDate" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label CssClass="control-label" runat="server" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control" ID="txtToDate" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label CssClass="control-label" runat="server" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList CssClass="form-control" ID="ddlStatus" runat="server">
                            <asp:ListItem Text="---All---" Value=""></asp:ListItem>
                            <asp:ListItem Value="Pending">Pending</asp:ListItem>
                            <asp:ListItem Value="Checked">Checked</asp:ListItem>
                            <asp:ListItem Value="Approved">Approved</asp:ListItem>
                            <asp:ListItem Value="Cancel">Cancel</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Consumtion Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlIssueType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="---All---" Value=""></asp:ListItem>
                            <asp:ListItem Text="For Employee" Value="Employee"></asp:ListItem>
                            <asp:ListItem Text="For Cost Center" Value="Costcenter"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" value="Search" onclick="LoadConsumptionDetails(1, 1)" />
                        <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="ClearSearch()" />
                        <input type="button" id="btnNewOutInfo" class="TransactionalButton btn btn-primary btn-sm" value="New Item Consumption" onclick="NewOutOrder()" />
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
            <table class="table table-bordered table-condensed table-responsive" id='tbConsumptionDetails'>
                <colgroup>
                    <col style="width: 12%;" />
                    <col style="width: 10%;" />
                    <col style="width: 13%;" />
                    <col style="width: 10%;" />
                    <col style="width: 12%;" />
                    <col style="width: 14%;" />
                    <col style="width: 10%;" />
                    <col style="width: 19%;" />
                </colgroup>
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th>Consumption No
                        </th>
                        <th>Date
                        </th>
                        <th>Consumption Type
                        </th>
                        <th>From
                        </th>
                        <th>For
                        </th>
                        <th>Created By
                        </th>
                        <th>Status
                        </th>
                        <th style="text-align: center;">Action
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

    <div id="DetailsOutGridContaiiner" style="display: none;">
        <table id="DetailsOutGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 20%;">Item Name
                    </th>
                    <th style="width: 10%;">Quantity
                    </th>
                    <th style="width: 15%;">Stock By
                    </th>
                    <th style="width: 10%;">Serial Number
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>

</asp:Content>
