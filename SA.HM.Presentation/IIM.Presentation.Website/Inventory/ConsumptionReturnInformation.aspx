<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="ConsumptionReturnInformation.aspx.cs" EnableEventValidation="false" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.ConsumptionReturnInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product Receive Approval</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if (Cookies.getJSON('outreturnoption') != undefined) {
                var SearchOption = Cookies.getJSON('outreturnoption');

                $("#ContentPlaceHolder1_txtReturnNumber").val(SearchOption.ReturnNumber);
                $("#ContentPlaceHolder1_ddlStatus").val(SearchOption.Status);               
                $("#ContentPlaceHolder1_txtFromDate").val(SearchOption.FromDate);
                $("#ContentPlaceHolder1_txtToDate").val(SearchOption.ToDate);

                var pageIndex = SearchOption.PageIndex;

                Cookies.remove('outreturnoption');
                SearchOutOrder(pageIndex, 1);
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

        });

        function EditItemOut(returnType, returnId, TransactionId) {

            var productOutFor = '', returnNumber = "0", fromDate = null, toDate = null, status = "";
            var pageIndex = $("#GridPagingContainer").find("li.active").index();

            if (pageIndex < 0)
                pageIndex = 1;

            returnNumber = $("#ContentPlaceHolder1_txtReturnNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            productOutFor = 'OutReturn';

            var SearchOption = {
                ReturnNumber: returnNumber,
                Status: status,
                ProductOutFor: productOutFor,
                FromDate: fromDate,
                ToDate: toDate,
                PageIndex: pageIndex
            };

            Cookies.set('outreturnoption', SearchOption, { expires: 1 });

            var url = "/Inventory/ConsumptionReturn.aspx?it=" + returnType + "&rid=" + returnId + "&tid=" + TransactionId;
            window.location = url;
            return false;
        }

        function SearchOutOrder(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#OutOrderSearchGrid tbody tr").length;
            var returnNumber = "0", status = "", returnType = "", fromDate = null, toDate = null;

            returnNumber = $("#ContentPlaceHolder1_txtReturnNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            returnType = "OutReturn";
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

            PageMethods.SearchReturnOrder(returnType, fromDate, toDate, returnNumber, status,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

            return false;
        }
        function OnSearchPurchaseOrderSucceed(result) {

            var tr = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.ReturnNumber + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.ReturnType + "</td>";

                if (gridObject.ReturnDate != null)
                    tr += "<td style='width:10%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.ReturnDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:10%;'></td>";

                tr += "<td style='width:15%;'>" + gridObject.FromCostCenter + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.FromLocation + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.Status + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:15%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:15%;'></td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";

                if (gridObject.IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditItemOut('" + gridObject.ReturnType + "'," + gridObject.ReturnId + "," + gridObject.TransactionId + ")\" alt='Edit'  title='Edit' border='0' />";
                }

                if (gridObject.IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return OutOrderDelete('" + gridObject.ReturnType + "'," + gridObject.ReturnId + "," + gridObject.TransactionId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return OutOrderApproval('" + gridObject.ReturnType + "','" + 'Checked' + "'," + gridObject.ReturnId + "," + gridObject.TransactionId + ")\" alt='Checked'  title='Checked' border='0' />";
                }

                if (gridObject.IsCanApproved) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return OutOrderApproval('" + gridObject.ReturnType + "','" + 'Approved' + "', " + gridObject.ReturnId + "," + gridObject.TransactionId + ")\" alt='Approved'  title='Approved' border='0' />";
                }

                //if (gridObject.Status == 'Approved') {
                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.ReturnType + "'," + gridObject.ReturnId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Invoice' title='Consumption Return Information' border='0' />";
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
            $("#ContentPlaceHolder1_txtIssueNumber").val("");
            $("#ContentPlaceHolder1_ddlStatus").val("All");
            $("#ContentPlaceHolder1_ddlSearchProductOutFor").val("All");
            $("#ContentPlaceHolder1_txtFromDate").datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_txtToDate").datepicker("setDate", DayOpenDate);
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchOutOrder(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }

        function OutOrderApproval(ReturnType, ApprovedStatus, ReturnId, TransactionId) {
            
            if (!confirm("Do you Want To " + (ApprovedStatus == "Checked" ? "Check?" : "Approve?"))) {
                return false;
            }

            PageMethods.OutOrderApproval(ReturnType, ReturnId, ApprovedStatus, TransactionId, OnApprovalSucceed, OnApprovalFailed);
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchOutOrder($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }
        //gridObject.ReturnType + "'," + gridObject.ReturnId + "," + gridObject.TransactionId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy
        function OutOrderDelete(ReturnType,  ReturnId,  TransactionId, ApprovedStatus, CreatedBy) {

            if (!confirm("Do you Want To Delete?")) {
                return false;
            }

            PageMethods.OutOrderDelete(ReturnType, ReturnId, ApprovedStatus, TransactionId, CreatedBy, OnApprovalSucceed, OnApprovalFailed);
        }

        function ShowReport(IssueType, ReturnId, ApprovedStatus, CreatedBy) {
            var iframeid = 'printDoc';
            var url = "Reports/frmReportProductReturn.aspx?ReturnId=" + ReturnId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Product Return",
                show: 'slide'
            });
        }

        function NewOutOrder() {
            window.location = "/Inventory/ConsumptionReturn.aspx";
        }

    </script>

    <div class="panel panel-default">
        <div class="panel-heading">
            Item Return Search
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
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
                            <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" value="Search" onclick="SearchOutOrder(1, 1)" />
                            <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="ClearSearch()" />
                            <input type="button" id="btnNewOutInfo" class="TransactionalButton btn btn-primary btn-sm" value="New Item Consumption Return" onclick="NewOutOrder()" />
                        </div>
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
                        <th style="width: 10%;">Return Type
                        </th>
                        <th style="width: 10%;">Return Date
                        </th>
                        <th style="width: 15%;">From Cost Center
                        </th>
                        <th style="width: 15%;">To Cost Center
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
