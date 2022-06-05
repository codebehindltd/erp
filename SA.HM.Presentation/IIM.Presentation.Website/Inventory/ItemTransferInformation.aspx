<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="ItemTransferInformation.aspx.cs" EnableEventValidation="false" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.ItemTransferInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
       
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product Receive Approval</li>";
            var breadCrumbs = moduleName + formName;

            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if (Cookies.getJSON('outsoption') != undefined) {
                var SearchOption = Cookies.getJSON('outsoption');

                $("#ContentPlaceHolder1_txtIssueNumber").val(SearchOption.IssueNumber);
                $("#ContentPlaceHolder1_ddlStatus").val(SearchOption.Status);
                $("#ContentPlaceHolder1_ddlSearchProductOutFor").val(SearchOption.ProductOutFor);
                $("#ContentPlaceHolder1_txtFromDate").val(SearchOption.FromDate);
                $("#ContentPlaceHolder1_txtToDate").val(SearchOption.ToDate);

                var pageIndex = SearchOption.PageIndex;

                Cookies.remove('outsoption');
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

        function EditItemOut(issueType, outId, requisitionOrSalesId) {
           

            var productOutFor = '', issueNumber = "0", fromDate = null, toDate = null, status = "";
            var pageIndex = $("#GridPagingContainer").find("li.active").index();

            if (pageIndex < 0)
                pageIndex = 1;

            issueNumber = $("#ContentPlaceHolder1_txtIssueNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();
            productOutFor = $("#ContentPlaceHolder1_ddlSearchProductOutFor").val();

            var SearchOption = {
                IssueNumber: issueNumber,
                Status: status,
                ProductOutFor: productOutFor,
                FromDate: fromDate,
                ToDate: toDate,
                PageIndex: pageIndex
            };

            Cookies.set('outsoption', SearchOption, { expires: 1 });

            var url = "/Inventory/ItemTransfer.aspx?it=" + issueType + "&oid=" + outId + "&rosid=" + requisitionOrSalesId;
            window.location = url;
            return false;
        }
        function EditItemOutWithConfirmation(issueType, outId, requisitionOrSalesId) {
            if (!confirm("Do you Want To Edit?")) {
                return false;
            }
            EditItemOut(issueType, outId, requisitionOrSalesId);
        }

        function SearchOutOrder(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#OutOrderGrid tbody tr").length;
            var issueNumber = "0", status = "", issueType = "", fromDate = null, toDate = null;

            issueNumber = $("#ContentPlaceHolder1_txtIssueNumber").val();
            status = $("#ContentPlaceHolder1_ddlStatus").val();
            issueType = $("#ContentPlaceHolder1_ddlSearchProductOutFor").val();
            fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');

            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            $("#GridPagingContainer ul").html("");
            $("#OutOrderGrid tbody").html("");

            if (pageNumber < 0)
                pageNumber = 1;

            PageMethods.SearchOutOrder(issueType, fromDate, toDate, issueNumber, status,
                gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchPurchaseOrderSucceed, OnSearchPurchaseOrderFailed);

            return false;
        }
        function OnSearchPurchaseOrderSucceed(result) {

            var tr = "";

            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.IssueNumber + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.IssueType + "</td>";

                if (gridObject.OutDate != null)
                    tr += "<td style='width:10%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.OutDate, innBoarDateFormat) + "</td>";
                else
                    tr += "<td style='width:10%;'></td>";

                tr += "<td style='width:15%;'>" + gridObject.FromCostCenter + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.ToCostCenter + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.Status + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:15%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:15%;'></td>";

                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";

                
                if (gridObject.IsCanEdit && IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return EditItemOutWithConfirmation('" + gridObject.ProductOutFor + "'," + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Edit'  title='Edit' border='0' />";
                }


                if (gridObject.IsCanDelete && IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return OutOrderDelete('" + gridObject.ProductOutFor + "'," + gridObject.OutId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Delete'  title='Delete' border='0' />";
                }

                if (gridObject.IsCanChecked && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return OutOrderApprovalWithConfirmation('" + gridObject.ProductOutFor + "','" + 'Checked' + "'," + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Check'  title='Check' border='0' />";
                }

                
                if (gridObject.IsCanApproved && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return OutOrderApprovalWithConfirmation('" + gridObject.ProductOutFor + "','" + 'Approved' + "', " + gridObject.OutId + "," + gridObject.RequisitionOrSalesId + ")\" alt='Approve'  title='Approve' border='0' />";
                }

                //if (gridObject.Status == 'Approved') {
                
                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowReport('" + gridObject.ProductOutFor + "'," + gridObject.OutId + ",'" + gridObject.Status + "'," + gridObject.CreatedBy + ")\" alt='Invoice' title='Item Transfer Info' border='0' />";
                //}

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

        function OutOrderDelete(IssueType, OutId, ApprovedStatus, CreatedBy) {

            if (!confirm("Do you Want To Delete?")) {
                return false;
            }

            PageMethods.OutOrderDelete(IssueType, OutId, ApprovedStatus, CreatedBy, OnApprovalSucceed, OnApprovalFailed);
        }

        function OutOrderApproval(ProductOutFor, ApprovedStatus, OutId, RequisitionOrSalesId) {
            
            PageMethods.OutOrderApproval(ProductOutFor, OutId, ApprovedStatus, RequisitionOrSalesId, OnApprovalSucceed, OnApprovalFailed);
        }
        function OutOrderApprovalWithConfirmation(ProductOutFor, ApprovedStatus, OutId, RequisitionOrSalesId) {
            if (ApprovedStatus=='Checked') {
                if (!confirm("Do you Want To Check?")) {
                return false;
            }
            }
            if (ApprovedStatus == 'Approved') {
                if (!confirm("Do you Want To Approve?")) {
                    return false;
                }
            }
            OutOrderApproval(ProductOutFor, ApprovedStatus, OutId, RequisitionOrSalesId);
        }
        function OnApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                //LoadNotReceivedPurchaseOrder();
                SearchOutOrder($("#GridPagingContainer").find("li.active").index(), 1);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovalFailed() {

        }

        function ShowReport(IssueType, OutId, ApprovedStatus, CreatedBy) {
            var iframeid = 'printDoc';
            var url = "Reports/frmReportProductOut.aspx?poOutId=" + OutId;
            parent.document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Item Transfer",
                show: 'slide'
            });
        }

        function NewOutOrder() {
            window.location = "/Inventory/ItemTransfer.aspx";
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            SearchOutOrder(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }

    </script>

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />

    <div class="panel panel-default">
        <div class="panel-heading">
            Item Transfer Search
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label12" runat="server" class="control-label" Text="Out Type"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlSearchProductOutFor" runat="server" CssClass="form-control"
                                TabIndex="1">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lbl1000" runat="server" class="control-label" Text="Transfer Number"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtIssueNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
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
                            <input type="button" id="btnNewOutInfo" class="TransactionalButton btn btn-primary btn-sm" value="New Item Transfer" onclick="NewOutOrder()" />
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

            <table id="OutOrderGrid" class="table table-bordered table-condensed table-responsive">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th style="width: 10%;">Transfer Number
                        </th>
                        <th style="width: 10%;">Transfer Type
                        </th>
                        <th style="width: 10%;">Out Date
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
